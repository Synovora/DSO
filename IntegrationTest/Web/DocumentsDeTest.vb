Imports System.Configuration
Imports System.Globalization
Imports System.IO
Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Security.Principal
Imports System.Threading
Imports System.Web
Imports System.Web.Hosting
Imports System.Web.Http
Imports System.Web.Routing
Imports Oasis_Common

''' <summary>
''' Aides des tests de la zone de dépôt (DocFileUpload, DocFileDownload, Rename),
''' de l'envoi de courriel (SendMail) et de la synthèse du portail.
'''
''' Zone de dépôt : app.config fixe FileUploadLocation à C:\OasisTests\Documents.
''' Rien n'est écrit ailleurs ; OuvrirZoneDocumentsDeTest relève ce que le dossier
''' contient déjà et FermerZoneDocumentsDeTest supprime tout le reste, sans jamais
''' sortir du dossier.
'''
''' Corps multipart : Upload et SendMail ne lisent pas Request.Content mais
''' HttpContext.Current.Request.GetBufferlessInputStream, qui tire les octets du
''' HttpWorkerRequest d'IIS. RequeteHttpBruteDeTest en tient lieu.
'''
''' Tout le SQL brut de ces tests est ici.
''' </summary>
Friend Module DocumentsDeTest

    ''' <summary>Valeur de FileUploadLocation dans IntegrationTest/app.config.</summary>
    Friend Const RacineAttendueDocumentsDeTest As String = "C:\OasisTests\Documents"

    Private contenuInitialZoneDeTest As HashSet(Of String)
    Private racineCreeeParLeTest As Boolean
    Private parentCreeParLeTest As Boolean

    ' --- Zone de dépôt --------------------------------------------------------------

    ''' <summary>
    ''' Racine de la zone de dépôt, lue dans la configuration comme le fait
    ''' ResoudreCheminDocument. Refuse de servir toute autre valeur : un app.config
    ''' modifié ne doit pas conduire les tests à écrire ailleurs.
    ''' </summary>
    Friend Function RacineZoneDocumentsDeTest() As String
        Dim configuree = ConfigurationManager.AppSettings("FileUploadLocation")
        Dim racine = Path.GetFullPath(If(configuree, "")).TrimEnd("\"c)
        If Not String.Equals(racine, RacineAttendueDocumentsDeTest, StringComparison.OrdinalIgnoreCase) Then
            Throw New InvalidOperationException(
                "FileUploadLocation vaut '" & configuree & "' : les tests de documents n'écrivent que dans " &
                RacineAttendueDocumentsDeTest & ".")
        End If
        Return racine
    End Function

    ''' <summary>Crée la zone si besoin et relève son contenu actuel.</summary>
    Friend Sub OuvrirZoneDocumentsDeTest()
        Dim racine = RacineZoneDocumentsDeTest()
        parentCreeParLeTest = Not Directory.Exists(Path.GetDirectoryName(racine))
        racineCreeeParLeTest = Not Directory.Exists(racine)
        Directory.CreateDirectory(racine)
        contenuInitialZoneDeTest = New HashSet(Of String)(
            Directory.GetFileSystemEntries(racine, "*", SearchOption.AllDirectories),
            StringComparer.OrdinalIgnoreCase)
    End Sub

    ''' <summary>
    ''' Supprime ce que le test a créé dans la zone : fichiers d'abord, puis dossiers
    ''' vides du plus profond au moins profond, puis la racine et son parent s'ils
    ''' n'existaient pas avant.
    ''' </summary>
    Friend Sub FermerZoneDocumentsDeTest()
        If contenuInitialZoneDeTest Is Nothing Then Return
        Dim racine = RacineZoneDocumentsDeTest()
        Try
            If Not Directory.Exists(racine) Then Return
            For Each fichier In Directory.GetFiles(racine, "*", SearchOption.AllDirectories)
                If EstDansZoneDeTest(fichier) AndAlso Not contenuInitialZoneDeTest.Contains(fichier) Then
                    File.SetAttributes(fichier, FileAttributes.Normal)
                    File.Delete(fichier)
                End If
            Next
            For Each dossier In Directory.GetDirectories(racine, "*", SearchOption.AllDirectories).
                                          OrderByDescending(Function(d) d.Length)
                If EstDansZoneDeTest(dossier) AndAlso Not contenuInitialZoneDeTest.Contains(dossier) AndAlso
                   Not Directory.EnumerateFileSystemEntries(dossier).Any() Then
                    Directory.Delete(dossier)
                End If
            Next
            If racineCreeeParLeTest AndAlso Not Directory.EnumerateFileSystemEntries(racine).Any() Then
                Directory.Delete(racine)
                Dim parent = Path.GetDirectoryName(racine)
                If parentCreeParLeTest AndAlso Not Directory.EnumerateFileSystemEntries(parent).Any() Then
                    Directory.Delete(parent)
                End If
            End If
        Finally
            contenuInitialZoneDeTest = Nothing
        End Try
    End Sub

    Private Function EstDansZoneDeTest(chemin As String) As Boolean
        Return Path.GetFullPath(chemin).StartsWith(RacineZoneDocumentsDeTest() & "\", StringComparison.OrdinalIgnoreCase)
    End Function

    ''' <summary>Chemin absolu d'un nom relatif, garanti dans la zone de dépôt.</summary>
    Friend Function CheminZoneDocumentsDeTest(nomRelatif As String) As String
        Dim chemin = Path.GetFullPath(Path.Combine(RacineZoneDocumentsDeTest(),
                                                   nomRelatif.Replace("/"c, "\"c).TrimStart("\"c)))
        If Not EstDansZoneDeTest(chemin) Then
            Throw New ArgumentException("Chemin hors de la zone de dépôt de test : " & nomRelatif)
        End If
        Return chemin
    End Function

    ''' <summary>Écrit un fichier dans la zone de dépôt, comme un dépôt antérieur.</summary>
    Friend Sub DeposerFichierDeTest(nomRelatif As String, octets As Byte())
        Dim chemin = CheminZoneDocumentsDeTest(nomRelatif)
        Directory.CreateDirectory(Path.GetDirectoryName(chemin))
        File.WriteAllBytes(chemin, octets)
    End Sub

    Friend Function ExisteDansZoneDeTest(nomRelatif As String) As Boolean
        Return File.Exists(CheminZoneDocumentsDeTest(nomRelatif))
    End Function

    Friend Function LireDansZoneDeTest(nomRelatif As String) As Byte()
        Return File.ReadAllBytes(CheminZoneDocumentsDeTest(nomRelatif))
    End Function

    ''' <summary>
    ''' Fichiers restés dans le sous-dossier tmp où Upload et SendMail reçoivent les
    ''' parties multipart. Ils doivent tous avoir été supprimés après l'appel.
    ''' </summary>
    Friend Function FichiersTemporairesRestantsDeTest() As Integer
        Dim dossier = Path.Combine(RacineZoneDocumentsDeTest(), "tmp")
        If Not Directory.Exists(dossier) Then Return 0
        Return Directory.GetFiles(dossier).Length
    End Function

    ''' <summary>
    ''' Contenu déterministe qui passe par toutes les valeurs d'octet, y compris 0,
    ''' CR, LF et les octets de fin de partie multipart.
    ''' </summary>
    Friend Function OctetsDocumentDeTest(taille As Integer, Optional graine As Integer = 0) As Byte()
        Dim octets(taille - 1) As Byte
        For i = 0 To taille - 1
            octets(i) = CByte((i * 7 + graine) Mod 256)
        Next
        Return octets
    End Function

    ' --- Noms de documents, tels que les construisent les beans ------------------------

    ''' <summary>Même forme que SousEpisode.getFilenameServer.</summary>
    Friend Function NomDocumentSousEpisodeDeTest(episodeId As Long, sousEpisodeId As Long,
                                                 Optional sousTypeId As Long = SousTypeSeAdressage,
                                                 Optional extension As String = "DOCX") As String
        Return "SousEpisode\Episode_" & episodeId & "_SousEpisode_" & sousEpisodeId &
               "_SousEpisodeSousType_" & sousTypeId & "." & extension
    End Function

    ''' <summary>Même forme que SousEpisodeReponse.GetFilenameServer.</summary>
    Friend Function NomDocumentReponseDeTest(episodeId As Long, sousEpisodeId As Long, reponseId As Long,
                                             Optional extension As String = "pdf") As String
        Return "SousEpisodeReponse\Episode_" & episodeId & "_SousEpisode_" & sousEpisodeId &
               "_SousEpisodeReponse_" & reponseId & "." & extension
    End Function

    ''' <summary>Même forme que SousEpisodeSousType.getFilenameServer (barre initiale comprise).</summary>
    Friend Function NomModeleDocumentDeTest(typeId As Long, sousTypeId As Long,
                                            Optional extension As String = "DOCX") As String
        Return "\Templates\SousEpisodeType_" & typeId & "_SousType_" & sousTypeId & "." & extension
    End Function

    ' --- Dossiers et comptes ---------------------------------------------------------

    ''' <summary>Patient, épisode et sous-épisode réels, auxquels un nom de document peut renvoyer.</summary>
    Friend Class DossierDocumentDeTest
        Friend Property PatientId As Long
        Friend Property EpisodeId As Long
        Friend Property SousEpisodeId As Long

        ''' <summary>Nom du document du sous-épisode (DOCX).</summary>
        Friend ReadOnly Property NomDocument As String
            Get
                Return NomDocumentSousEpisodeDeTest(EpisodeId, SousEpisodeId)
            End Get
        End Property
    End Class

    ''' <summary>
    ''' Crée un patient, un épisode et un sous-épisode par les DAO, sous le compte
    ''' courant (celui du client lourd par défaut).
    ''' </summary>
    Friend Function CreerDossierDocumentDeTest(utilisateurId As Long,
                                               Optional nomPatient As String = "DOSSIER") As DossierDocumentDeTest
        Dim idPatient = CreerPatient(nomPatient, "Documents")
        Dim idEpisode = CreerEpisode(idPatient, utilisateurId)
        Dim idSousEpisode = CreerSousEpisode(idEpisode, utilisateurId)
        Return New DossierDocumentDeTest With {
            .PatientId = idPatient,
            .EpisodeId = idEpisode,
            .SousEpisodeId = idSousEpisode
        }
    End Function

    ''' <summary>
    ''' Compte dont le profil est du type donné (MEDICAL, PARAMEDICAL, ACCUEIL,
    ''' GESTION...). Le profil IT_D_{5 premières lettres du type} est créé s'il
    ''' manque (dix caractères, comme IT_MEDECIN). Renvoie l'id.
    ''' </summary>
    Friend Function CreerCompteDocumentsDeTest(login As String,
                                               Optional typeProfil As String = "MEDICAL",
                                               Optional admin As Boolean = False) As Long
        Dim idProfil = "IT_D_" & Left(typeProfil, 5)
        If CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_r_profil WHERE oa_r_profil_id = @p0", idProfil)) = 0 Then
            CreerProfil(idProfil, typeProfil)
        End If
        Return CreerUtilisateur(login, avecCle:=False, profilId:=idProfil, admin:=admin)
    End Function

    ''' <summary>En-tête Basic avec le mot de passe par défaut des jeux.</summary>
    Friend Function EnteteDocumentsDeTest(login As String) As AuthenticationHeaderValue
        Return EnteteBasic(login, MotDePasseParDefaut)
    End Function

    ' --- Journal des accès (oa_action) -----------------------------------------------

    ''' <summary>Lignes de journal de cet utilisateur pour ce patient portant exactement ce libellé.</summary>
    Friend Function CompterJournalDocumentsDeTest(utilisateurId As Long, patientId As Long, libelle As String) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_action WHERE utilisateur_id = @p0 AND patient_id = @p1 AND action = @p2",
                             utilisateurId, patientId, libelle))
    End Function

    ''' <summary>Lignes de journal de cet utilisateur, tous patients, dont le libellé commence ainsi.</summary>
    Friend Function CompterJournalCommencantParDeTest(utilisateurId As Long, prefixe As String) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_action WHERE utilisateur_id = @p0 AND action LIKE @p1 + '%'",
                             utilisateurId, prefixe))
    End Function

    ' --- Corps multipart ---------------------------------------------------------------

    ''' <summary>Formulaire de dépôt tel que l'envoie ApiOasis.uploadFile (partie « filekey »).</summary>
    Friend Function CorpsDepotDeTest(nomFichier As String, octets As Byte()) As MultipartFormDataContent
        Dim corps As New MultipartFormDataContent()
        corps.Add(New ByteArrayContent(octets), "filekey", nomFichier)
        Return corps
    End Function

    ''' <summary>
    ''' Formulaire d'envoi tel que l'envoie ApiOasis.sendMail. adresses à Nothing :
    ''' pas de champ adressTo. avecIndicateurs faux : ni isSousEpisode ni isHTML.
    ''' </summary>
    Friend Function CorpsCourrielDeTest(adresses As String,
                                        Optional patientId As String = "0",
                                        Optional pieceJointe As Byte() = Nothing,
                                        Optional nomPieceJointe As String = "compte-rendu.pdf",
                                        Optional avecIndicateurs As Boolean = True) As MultipartFormDataContent
        Dim corps As New MultipartFormDataContent()
        corps.Add(New StringContent(patientId), "patientId")
        corps.Add(New StringContent("Cabinet de test"), "aliasFrom")
        If adresses IsNot Nothing Then corps.Add(New StringContent(adresses), "adressTo")
        corps.Add(New StringContent("Objet de test"), "subject")
        corps.Add(New StringContent("Corps de test"), "body")
        If avecIndicateurs Then
            corps.Add(New StringContent("False"), "isSousEpisode")
            corps.Add(New StringContent("False"), "isHTML")
        End If
        If pieceJointe IsNot Nothing Then
            corps.Add(New ByteArrayContent(pieceJointe), "filekey", nomPieceJointe)
        End If
        Return corps
    End Function

    ''' <summary>
    ''' Comme AppelerApi, pour les actions qui lisent le corps brut : les octets du
    ''' formulaire passent par un HttpContext adossé à RequeteHttpBruteDeTest, et son
    ''' Content-Type (frontière comprise) par Request.Content, d'où l'action le recopie.
    ''' OuvrirContexteHttp doit avoir été appelé (principal à restaurer) ;
    ''' FermerContexteHttp remet HttpContext.Current à Nothing.
    ''' </summary>
    Friend Function AppelerApiAvecCorpsDeTest(Of T As {ApiController, New})(entete As AuthenticationHeaderValue,
                                                                           nomAction As String,
                                                                           appel As Func(Of T, HttpResponseMessage),
                                                                           corps As MultipartFormDataContent) As HttpResponseMessage
        Dim octets = corps.ReadAsByteArrayAsync().Result
        Dim typeContenu = corps.Headers.ContentType.ToString()
        HttpContext.Current = New HttpContext(New RequeteHttpBruteDeTest(octets, typeContenu))
        Return AppelerApi(Of T)(entete, nomAction, appel, corps)
    End Function

    ' --- Courriel ----------------------------------------------------------------------

    ''' <summary>
    ''' Retire tout paramètre SMTP. Sans lui, SendMail échoue en lisant sa
    ''' configuration, après les contrôles et avant toute connexion : aucun courriel
    ''' ne peut quitter la machine, quoi qu'un script de référence ait posé.
    ''' </summary>
    Friend Sub RetirerParametresSmtpDeTest()
        Executer("DELETE FROM oasis.oa_r_mail_parameter WHERE type_mail_param = @p0",
                 ParametreMail.TypeMailParams.SMTP_PARAMETERS.ToString())
    End Sub

    Friend Function CompterParametresSmtpDeTest() As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_r_mail_parameter WHERE type_mail_param = @p0",
                             ParametreMail.TypeMailParams.SMTP_PARAMETERS.ToString()))
    End Function

    ''' <summary>
    ''' Boîte aux lettres de l'annuaire professionnel. La table est remplie par
    ''' l'import SSIS, aucun DAO n'y écrit : SQL brut, mêmes colonnes que l'import.
    ''' </summary>
    Friend Sub InscrireAdresseAnnuaireDeTest(adresse As String)
        Executer("INSERT INTO oasis.ans_annuaire_professionnel_sante_bal" &
                 " (identifiant_national_pp, type_bal, adresse_bal, raison_sociale_structure)" &
                 " VALUES (@p0, @p1, @p2, @p3)",
                 "810000000001", "PER", adresse, "CABINET DE TEST")
    End Sub

    ''' <summary>Patient portant une adresse de courriel, par PatientDao.CreationPatient. Renvoie son id.</summary>
    Friend Function CreerPatientJoignableDeTest(adresse As String) As Long
        Dim fiche = PatientDeTest("JOIGNABLE", "Courriel")
        fiche.PatientEmail = adresse
        Dim daoPatient As New PatientDao
        daoPatient.CreationPatient(fiche, New Utilisateur)
        Return CLng(Scalaire("SELECT MAX(oa_patient_id) FROM oasis.oa_patient WHERE oa_patient_nir = @p0", fiche.PatientNir))
    End Function

    ' --- Portail : synthèse ------------------------------------------------------------

    ''' <summary>
    ''' Compte du portail rattaché à ce patient, comme le bouton de création de la
    ''' fiche patient : InternauteDao.Create puis InternautePermissionDao.Create.
    ''' Renvoie l'id de l'internaute.
    ''' </summary>
    Friend Function CreerAccesPortailDeTest(patientId As Long) As Long
        Dim idInternaute = CreerInternaute()
        Dim daoPermission As New InternautePermissionDao
        daoPermission.Create(New InternautePermission With {
            .Internaute = CInt(idInternaute),
            .Patient = CInt(patientId),
            .Permission = 1
        })
        Return idInternaute
    End Function

    ''' <summary>Identité Forms telle que la pose AuthController : le nom est l'id de l'internaute.</summary>
    Friend Function PrincipalPortailDeTest(nom As String) As IPrincipal
        Return New GenericPrincipal(New GenericIdentity(nom, "Forms"), New String() {})
    End Function

    ''' <summary>
    ''' HttpContextBase minimal : PortailController ne lit que User. Le reste du
    ''' contexte MVC n'est pas sollicité par un appel direct de l'action.
    ''' </summary>
    Friend Class ContexteHttpPortailDeTest
        Inherits HttpContextBase

        Private principalCourant As IPrincipal

        Friend Sub New(principal As IPrincipal)
            principalCourant = principal
        End Sub

        Public Overrides Property User As IPrincipal
            Get
                Return principalCourant
            End Get
            Set(value As IPrincipal)
                principalCourant = value
            End Set
        End Property
    End Class

    ''' <summary>
    ''' Appelle SyntheseController.Index sous le compte du serveur, avec cette
    ''' identité, dans la culture donnée (fr-FR par défaut, celle du serveur de
    ''' production), rétablie ensuite. modeAffichage et texteAccueil vont dans
    ''' TempData comme les y dépose LayoutsController.
    ''' </summary>
    Friend Function ConsulterSyntheseDeTest(principal As IPrincipal,
                                            Optional modeAffichage As String = Nothing,
                                            Optional texteAccueil As String = Nothing,
                                            Optional culture As String = "fr-FR") As System.Web.Mvc.ActionResult
        UtiliserCompte(Compte.Web)
        Dim controleur As New Global.Oasis_Web.Oasis_Web.Controllers.SyntheseController()
        controleur.ControllerContext = New System.Web.Mvc.ControllerContext(
            New ContexteHttpPortailDeTest(principal), New RouteData(), controleur)
        If modeAffichage IsNot Nothing Then controleur.TempData("ModeName") = modeAffichage
        If texteAccueil IsNot Nothing Then controleur.TempData("WelcomeText") = texteAccueil

        Dim cultureAvant = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo(culture)
        Try
            Return controleur.Index()
        Finally
            Thread.CurrentThread.CurrentCulture = cultureAvant
        End Try
    End Function

    ''' <summary>
    ''' Parcours de soins par ParcoursDao.CreateIntervenantParcours, comme l'écran
    ''' des intervenants. Spécialité et intervenant ROR 2 : l'IDE Oasis de
    ''' 28-reference-parcours.sql. Renvoie son id.
    ''' </summary>
    Friend Function CreerParcoursSyntheseDeTest(patientId As Long, utilisateurId As Long,
                                                sousCategorieId As Integer,
                                                Optional commentaire As String = "Parcours de test") As Long
        Dim nouveau As New Parcours With {
            .PatientId = CInt(patientId),
            .SpecialiteId = 2,
            .CategorieId = 3,
            .SousCategorieId = sousCategorieId,
            .IntervenantOasis = False,
            .RorId = 2,
            .Commentaire = commentaire,
            .Base = ParcoursDao.EnumParcoursBaseCode.ParMois,
            .Rythme = 1,
            .Cacher = False,
            .Inactif = False,
            .UserCreation = CInt(utilisateurId),
            .DateCreation = Date.Now
        }
        Dim daoParcours As New ParcoursDao
        Return daoParcours.CreateIntervenantParcours(nouveau, New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
    End Function

    ''' <summary>
    ''' Contexte sans date de fin, comme en laissent les saisies anciennes. Les écrans
    ''' actuels posent toujours 31/12/2999 : aucun DAO n'écrit NULL, d'où le SQL brut.
    ''' </summary>
    Friend Sub RetirerDateFinContexteDeTest(contexteId As Long)
        Executer("UPDATE oasis.oa_antecedent SET oa_antecedent_date_fin = NULL WHERE oa_antecedent_id = @p0", contexteId)
    End Sub

End Module

''' <summary>
''' Requête HTTP portant un corps brut, à la place du HttpWorkerRequest d'IIS.
''' HttpBufferlessInputStream demande la longueur par GetKnownRequestHeader puis lit
''' par ReadEntityBody, sans rien de préchargé. Le constructeur à cinq arguments de
''' SimpleWorkerRequest est celui prévu hors d'un domaine d'application ASP.NET.
''' </summary>
Friend Class RequeteHttpBruteDeTest
    Inherits SimpleWorkerRequest

    Private ReadOnly fluxCorps As MemoryStream
    Private ReadOnly longueurCorps As Integer
    Private ReadOnly typeCorps As String

    Friend Sub New(octets As Byte(), typeContenu As String)
        MyBase.New("/", AppDomain.CurrentDomain.BaseDirectory, "api", "", New StringWriter())
        fluxCorps = New MemoryStream(octets, False)
        longueurCorps = octets.Length
        typeCorps = typeContenu
    End Sub

    Public Overrides Function GetHttpVerbName() As String
        Return "POST"
    End Function

    Public Overrides Function GetKnownRequestHeader(index As Integer) As String
        If index = HeaderContentLength Then Return longueurCorps.ToString(CultureInfo.InvariantCulture)
        If index = HeaderContentType Then Return typeCorps
        Return MyBase.GetKnownRequestHeader(index)
    End Function

    Public Overrides Function GetTotalEntityBodyLength() As Integer
        Return longueurCorps
    End Function

    Public Overrides Function IsEntireEntityBodyIsPreloaded() As Boolean
        Return False
    End Function

    Public Overrides Function GetPreloadedEntityBodyLength() As Integer
        Return 0
    End Function

    Public Overrides Function GetPreloadedEntityBody() As Byte()
        Return Nothing
    End Function

    Public Overrides Function ReadEntityBody(buffer As Byte(), size As Integer) As Integer
        Return fluxCorps.Read(buffer, 0, size)
    End Function

    Public Overrides Function ReadEntityBody(buffer As Byte(), offset As Integer, size As Integer) As Integer
        Return fluxCorps.Read(buffer, offset, size)
    End Function

End Class
