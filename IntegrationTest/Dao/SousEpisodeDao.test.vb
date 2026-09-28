Imports System.Data.SqlClient
Imports System.Text.RegularExpressions
Imports Oasis_Common

''' <summary>
''' SousEpisodeDao contre la base. Le client lourd crée, lit, compte, valide et
''' inactive les sous-épisodes (FrmSousEpisode, FrmSousEpisodeListe,
''' FrmSousEpisodeReponseAttribution, FrmEditDocxSousEpisode, RadFEpisodeDetail,
''' RadFEpisodeLigneDeVie) : ces appels tournent sous oasis_client.
''' AppartientAEpisode sert au serveur (HabilitationsDocuments, appelé par les
''' contrôleurs de dépôt, de téléchargement et de renommage) : sous oasis_web.
'''
''' Le contenu DOCX du sous-épisode vit sur le serveur de fichiers :
''' WriteDocAndEventualySign n'est testé que pour sa partie base, en faisant échouer
''' le dépôt (ServeurOasis pointe sur tests.invalid dans app.config).
''' </summary>
<TestClass()> Public Class SousEpisodeDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New SousEpisodeDao

    Private Const SousEpisodeAbsent As Long = 987654321

    ''' <summary>Auteur, patient et épisode ouverts pour un test.</summary>
    Private Class DossierSe
        Public UtilisateurId As Long
        Public PatientId As Long
        Public EpisodeId As Long
    End Class

    Private Shared Function NouveauContexte(Optional prenom As String = "Patient") As DossierSe
        Dim ctx As New DossierSe
        ctx.UtilisateurId = CreerUtilisateur(avecCle:=False)
        ctx.PatientId = CreerPatient(prenom:=prenom)
        ctx.EpisodeId = CreerEpisode(ctx.PatientId, ctx.UtilisateurId)
        Return ctx
    End Function

    Private Shared Function Valeur(colonne As String, idSousEpisode As Long) As Object
        Return Scalaire("SELECT " & colonne & " FROM oasis.oa_sous_episode WHERE id = @p0", idSousEpisode)
    End Function

    Private Shared Function NombreDeSousEpisodes() As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_sous_episode"))
    End Function

    Private Sub Inactiver(idSousEpisode As Long)
        dao.inactiverSousEpisode(Nothing, idSousEpisode, Nothing)
    End Sub

    Private Shared Sub VerifierProche(attendu As Date, lu As Date, Optional message As String = "")
        Assert.IsTrue(Math.Abs((lu - attendu).TotalSeconds) < 2, message & " : attendu " & attendu & ", lu " & lu)
    End Sub

    Private Shared Function Ids(liste As IEnumerable(Of SousEpisode)) As Long()
        Return liste.Select(Function(s) s.Id).ToArray()
    End Function

    Private Shared Function IdsDeTable(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("id"))).ToArray()
    End Function

    ' --- Create --------------------------------------------------------------------

    <TestMethod()> Public Sub Create_EnregistreLeSousEpisodeSesDetailsEtUneReference()
        Dim ctx = NouveauContexte("Josephine")
        Dim nouveau As New SousEpisode With {
            .EpisodeId = ctx.EpisodeId,
            .IdSousEpisodeType = TypeSeCourrier,
            .IdSousEpisodeSousType = SousTypeSeAdressage,
            .IdIntervenant = 0,
            .CreateUserId = ctx.UtilisateurId,
            .Commentaire = "Adresser au cardiologue",
            .IsALD = True,
            .IsReponse = True,
            .DelaiSinceValidation = 21,
            .lstDetail = New List(Of SousEpisodeDetailSousType) From {
                New SousEpisodeDetailSousType With {.IdSousEpisodeSousSousType = SousSousTypeSeBilan, .IsALD = True},
                New SousEpisodeDetailSousType With {.IdSousEpisodeSousSousType = SousSousTypeSeImagerie, .IsALD = False}
            }
        }
        Dim avant = Date.Now

        Assert.IsTrue(dao.Create(nouveau))

        Assert.IsTrue(nouveau.Id > 0, "l'id attribué revient sur le bean")
        VerifierProche(avant, nouveau.HorodateCreation, "horodatage posé par Create")
        Assert.IsTrue(nouveau.lstDetail.All(Function(d) d.IdSousEpisode = nouveau.Id AndAlso d.Id > 0))

        Dim relu = dao.GetById(nouveau.Id)
        Assert.AreEqual(ctx.EpisodeId, relu.EpisodeId)
        Assert.AreEqual(TypeSeCourrier, relu.IdSousEpisodeType)
        Assert.AreEqual(SousTypeSeAdressage, relu.IdSousEpisodeSousType)
        Assert.AreEqual(ctx.UtilisateurId, relu.CreateUserId)
        VerifierProche(nouveau.HorodateCreation, relu.HorodateCreation, "horodate_creation relue")
        Assert.AreEqual("Adresser au cardiologue", relu.Commentaire)
        Assert.IsTrue(relu.IsALD)
        Assert.IsTrue(relu.IsReponse)
        Assert.AreEqual(21, relu.DelaiSinceValidation)
        Assert.IsFalse(relu.isInactif)
        ' Référence : cinq premières lettres du prénom, puis six caractères base 33.
        StringAssert.Matches(relu.Reference, New Regex("^JOSEP-[1-9A-HJ-NP-Z]{6}$"))

        Dim details = New SousEpisodeDetailSousTypeDao().getLstSousEpisodeDetailSousType(nouveau.Id)
        Assert.AreEqual(2, details.Count)
        Assert.IsTrue(details.Single(Function(d) d.IdSousEpisodeSousSousType = SousSousTypeSeBilan).IsALD)
        Assert.IsFalse(details.Single(Function(d) d.IdSousEpisodeSousSousType = SousSousTypeSeImagerie).IsALD)
    End Sub

    <TestMethod()> Public Sub Create_LaisseANullCeQueLaCreationNeRenseignePas()
        Dim ctx = NouveauContexte()

        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)

        Assert.AreEqual(DBNull.Value, Valeur("id_intervenant", id), "sans destinataire")
        Assert.AreEqual(DBNull.Value, Valeur("validate_user_id", id))
        Assert.AreEqual(DBNull.Value, Valeur("horodate_validate", id))
        Assert.AreEqual(DBNull.Value, Valeur("signature", id))
        Assert.AreEqual(DBNull.Value, Valeur("last_update_user_id", id))
        Dim relu = dao.GetById(id)
        Assert.AreEqual(0L, relu.IdIntervenant)
        Assert.AreEqual(0L, relu.ValidateUserId)
        Assert.AreEqual(Date.MinValue, relu.HorodateValidate)
        Assert.AreEqual(Date.MinValue, relu.HorodateLastUpdate)
        Assert.AreEqual("NaN", relu.Signature, "valeur de repli du bean pour une signature absente")
        Assert.IsFalse(relu.IsReponseRecue)
        Assert.AreEqual(Date.MinValue, relu.HorodateLastRecu)
    End Sub

    <TestMethod()> Public Sub Create_SansListeDeDetails_EchoueEtNeLaisseRien()
        ' Comportement actuel : lstDetail n'est pas initialisé par le constructeur du
        ' bean. Sans liste, Create lève une NullReferenceException après l'INSERT,
        ' que la transaction annule. FrmSousEpisode renseigne toujours la liste.
        Dim ctx = NouveauContexte()
        Dim avant = NombreDeSousEpisodes()
        Dim nouveau As New SousEpisode With {
            .EpisodeId = ctx.EpisodeId, .IdSousEpisodeType = TypeSeCourrier,
            .IdSousEpisodeSousType = SousTypeSeAdressage, .CreateUserId = ctx.UtilisateurId,
            .Commentaire = "", .DelaiSinceValidation = 15}

        Try
            dao.Create(nouveau)
            Assert.Fail("Create sans lstDetail devrait échouer.")
        Catch ex As AssertFailedException
            Throw
        Catch ex As Exception
        End Try

        Assert.AreEqual(avant, NombreDeSousEpisodes())
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentException))>
    Public Sub Create_EpisodeInexistant_Leve()
        ' La lecture de l'épisode précède la transaction : l'erreur remonte telle quelle.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        dao.Create(New SousEpisode With {
            .EpisodeId = 987654, .IdSousEpisodeType = TypeSeCourrier, .IdSousEpisodeSousType = SousTypeSeAdressage,
            .CreateUserId = idUtilisateur, .Commentaire = "", .lstDetail = New List(Of SousEpisodeDetailSousType)})
    End Sub

    ' --- GetById ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetById_CompteLesReponsesParEtatEtNommeLAuteur()
        Dim ctx = NouveauContexte()
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId, SousTypeSeCompteRendu)
        CreerReponseSousEpisode(id, ctx.UtilisateurId, "!")
        CreerReponseSousEpisode(id, ctx.UtilisateurId, "!")
        CreerReponseSousEpisode(id, ctx.UtilisateurId, "m")
        CreerReponseSousEpisode(id, ctx.UtilisateurId, "v")

        Dim relu = dao.GetById(id)

        Assert.AreEqual(4L, relu.NbReponse)
        Assert.AreEqual(2L, relu.NbReponseWaiting)
        Assert.AreEqual(1L, relu.NbMedReponseWaiting)
        Assert.AreEqual("Utilisateur TEST", relu.UserCreate)
        Assert.AreEqual(LibelleTypeSeCourrier, relu.TypeLibelle)
        Assert.AreEqual(LibelleSousTypeSeCompteRendu, relu.SousTypeLibelle)
        Assert.IsTrue(relu.IsReponseRecue, "posé par la création de réponse")
        Assert.AreEqual(New Date(2026, 3, 1, 10, 0, 0), relu.HorodateLastRecu)
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentOutOfRangeException))>
    Public Sub GetById_Inexistant_Leve()
        dao.GetById(SousEpisodeAbsent)
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentOutOfRangeException))>
    Public Sub GetById_SousEpisodeInactif_Leve()
        ' Comportement actuel : GetById passe par GetLstSousEpisode sans les inactifs,
        ' un sous-épisode inactivé n'est plus lisible par son id.
        Dim ctx = NouveauContexte()
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Inactiver(id)
        dao.GetById(id)
    End Sub

    ' --- GetLstSousEpisode -------------------------------------------------------------

    <TestMethod()> Public Sub GetLstSousEpisode_ActifsDeLEpisodeDuPlusRecentAuPlusAncien()
        Dim ctx = NouveauContexte()
        Dim autre = NouveauContexte()
        Dim premier = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Dim inactif = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Dim troisieme = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId, SousTypeSeCompteRendu)
        CreerSousEpisode(autre.EpisodeId, autre.UtilisateurId)
        Inactiver(inactif)

        CollectionAssert.AreEqual(New Long() {troisieme, premier}, Ids(dao.GetLstSousEpisode(ctx.EpisodeId)))
        CollectionAssert.AreEqual(New Long() {troisieme, inactif, premier},
                                  Ids(dao.GetLstSousEpisode(ctx.EpisodeId, isWithInactif:=True)))
    End Sub

    <TestMethod()> Public Sub GetLstSousEpisode_FiltreParIdEtSansEpisode()
        Dim ctx = NouveauContexte()
        Dim autre = NouveauContexte()
        Dim cible = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Dim ailleurs = CreerSousEpisode(autre.EpisodeId, autre.UtilisateurId)

        CollectionAssert.AreEqual(New Long() {cible}, Ids(dao.GetLstSousEpisode(ctx.EpisodeId, cible)))
        ' Épisode à 0 : pas de filtre d'épisode.
        CollectionAssert.AreEqual(New Long() {ailleurs}, Ids(dao.GetLstSousEpisode(0, ailleurs)))
        Dim tous = Ids(dao.GetLstSousEpisode(0))
        CollectionAssert.IsSubsetOf(New Long() {cible, ailleurs}, tous)
        CollectionAssert.AreEqual(tous.OrderByDescending(Function(i) i).ToArray(), tous)
    End Sub

    <TestMethod()> Public Sub GetLstSousEpisode_NonComplet_NeRemplitNiAuteurNiCompteurs()
        Dim ctx = NouveauContexte()
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        CreerReponseSousEpisode(id, ctx.UtilisateurId, "!")

        Dim relu = dao.GetLstSousEpisode(ctx.EpisodeId, isComplete:=False).Single()

        Assert.AreEqual(id, relu.Id)
        Assert.AreEqual("", relu.UserCreate)
        Assert.AreEqual(0L, relu.NbReponse)
        Assert.AreEqual(0L, relu.NbReponseWaiting)
        Assert.AreEqual(LibelleSousTypeSeAdressage, relu.SousTypeLibelle, "les libellés restent joints")
    End Sub

    <TestMethod()> Public Sub GetLstSousEpisode_UnSousEpisodeInactifNaAucuneReponseComptee()
        ' Comportement actuel : les compteurs de GetLstSousEpisode exigent un
        ' sous-épisode actif ; relu avec les inactifs, il affiche zéro réponse.
        Dim ctx = NouveauContexte()
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        CreerReponseSousEpisode(id, ctx.UtilisateurId, "!")
        Inactiver(id)

        Dim relu = dao.GetLstSousEpisode(ctx.EpisodeId, isWithInactif:=True).Single()

        Assert.IsTrue(relu.isInactif)
        Assert.AreEqual(0L, relu.NbReponse)
        Assert.AreEqual(0L, relu.NbReponseWaiting)
    End Sub

    <TestMethod()> Public Sub GetLstSousEpisode_ModificateurEgalAuCreateur_DupliqueLaLigneParUtilisateur()
        ' Comportement actuel : la jointure du modificateur porte sur UC (le créateur)
        ' au lieu de UU : « LEFT JOIN oa_utilisateur UU ON UC.oa_utilisateur_id =
        ' SE.last_update_user_id ». Quand le modificateur est le créateur, la
        ' condition est vraie pour chaque ligne d'oa_utilisateur et le sous-épisode
        ' revient une fois par utilisateur de la base. Aucun DAO n'écrit encore
        ' last_update_user_id, d'où la mise à jour directe.
        Dim ctx = NouveauContexte()
        CreerUtilisateur(avecCle:=False)
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Executer("UPDATE oasis.oa_sous_episode SET last_update_user_id = @p0, horodate_last_update = @p1 WHERE id = @p2",
                 ctx.UtilisateurId, New Date(2026, 4, 1, 12, 0, 0), id)
        Dim nbUtilisateurs = CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_utilisateur"))
        Assert.IsTrue(nbUtilisateurs >= 2)

        Dim liste = dao.GetLstSousEpisode(ctx.EpisodeId)

        Assert.AreEqual(nbUtilisateurs, liste.Count)
        Assert.IsTrue(liste.All(Function(s) s.Id = id))
        Assert.AreEqual(ctx.UtilisateurId, liste(0).LastUpdateUserId)
        Assert.AreEqual(New Date(2026, 4, 1, 12, 0, 0), liste(0).HorodateLastUpdate)
        Assert.AreEqual(nbUtilisateurs, dao.GetAllSousEpisodeByPatient(CInt(ctx.EpisodeId)).Count)
        Assert.AreEqual(nbUtilisateurs, dao.GetTableSousEpisode(ctx.EpisodeId).Rows.Count)
        ' La version non complète ne joint pas les utilisateurs : une seule ligne.
        Assert.AreEqual(1, dao.GetLstSousEpisode(ctx.EpisodeId, isComplete:=False).Count)
    End Sub

    ' --- GetLstSousEpisodeByEpisodeId ------------------------------------------------

    <TestMethod()> Public Sub GetLstSousEpisodeByEpisodeId_InclutLesInactifsDuPlusRecentAuPlusAncien()
        Dim ctx = NouveauContexte()
        Dim autre = NouveauContexte()
        Dim premier = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Dim second = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId, SousTypeSeCompteRendu)
        CreerSousEpisode(autre.EpisodeId, autre.UtilisateurId)
        Inactiver(premier)

        Dim liste = dao.GetLstSousEpisodeByEpisodeId(ctx.EpisodeId)

        CollectionAssert.AreEqual(New Long() {second, premier}, Ids(liste))
        Assert.IsTrue(liste(1).isInactif)
        Assert.AreEqual(LibelleSousTypeSeCompteRendu, liste(0).SousTypeLibelle)
        Assert.AreEqual(LibelleTypeSeCourrier, liste(0).TypeLibelle)
        Assert.AreEqual("", liste(0).UserCreate, "colonne absente de cette requête")
    End Sub

    <TestMethod()> Public Sub GetLstSousEpisodeByEpisodeId_EpisodeSansSousEpisode_DonneUneListeVide()
        Dim ctx = NouveauContexte()
        Assert.AreEqual(0, dao.GetLstSousEpisodeByEpisodeId(ctx.EpisodeId).Count)
    End Sub

    ' --- GetAllSousEpisodeByPatient ----------------------------------------------------

    <TestMethod()> Public Sub GetAllSousEpisodeByPatient_ParDefaut_InclutLesInactifs()
        ' Comportement actuel : le paramètre est inversé par rapport à son nom. Par
        ' défaut (isWithInactif = False) aucun filtre ; avec True, seuls les actifs.
        ' Le paramètre porte en outre un id d'épisode, pas de patient.
        Dim ctx = NouveauContexte()
        Dim actif = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Dim inactif = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Inactiver(inactif)

        CollectionAssert.AreEqual(New Long() {inactif, actif}, Ids(dao.GetAllSousEpisodeByPatient(CInt(ctx.EpisodeId))))
        CollectionAssert.AreEqual(New Long() {actif}, Ids(dao.GetAllSousEpisodeByPatient(CInt(ctx.EpisodeId), True)))
    End Sub

    <TestMethod()> Public Sub GetAllSousEpisodeByPatient_CompteLesReponsesMemeDUnInactif()
        Dim ctx = NouveauContexte()
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        CreerReponseSousEpisode(id, ctx.UtilisateurId, "!")
        CreerReponseSousEpisode(id, ctx.UtilisateurId, "m")
        Inactiver(id)

        Dim relu = dao.GetAllSousEpisodeByPatient(CInt(ctx.EpisodeId)).Single()

        Assert.AreEqual(2L, relu.NbReponse)
        Assert.AreEqual(1L, relu.NbReponseWaiting)
        Assert.AreEqual(1L, relu.NbMedReponseWaiting)
        Assert.AreEqual("Utilisateur TEST", relu.UserCreate)
        Assert.AreEqual(LibelleSousTypeSeAdressage, relu.SousTypeLibelle)
    End Sub

    ' --- GetTableSousEpisode ------------------------------------------------------------

    <TestMethod()> Public Sub GetTableSousEpisode_Complet_JointAuteursLibellesEtCompteurs()
        Dim ctx = NouveauContexte()
        Dim autreAuteur = CreerUtilisateur(avecCle:=False)
        Dim premier = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Dim second = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId, SousTypeSeCompteRendu)
        CreerReponseSousEpisode(second, ctx.UtilisateurId, "!")
        ' Modifié par un autre utilisateur que le créateur : une seule ligne, mais
        ' user_update reste NULL (jointure sur le créateur, voir le test de duplication).
        Executer("UPDATE oasis.oa_sous_episode SET last_update_user_id = @p0, horodate_last_update = @p1 WHERE id = @p2",
                 autreAuteur, New Date(2026, 4, 2, 9, 0, 0), premier)

        Dim table = dao.GetTableSousEpisode(ctx.EpisodeId, isComplete:=True)

        CollectionAssert.AreEqual(New Long() {second, premier}, IdsDeTable(table))
        For Each colonne In {"user_create", "user_update", "user_validate", "type_libelle", "sous_type_libelle",
                             "redaction_profil_types", "validation_profil_types", "nb_reponse_waiting",
                             "nb_med_reponse_waiting", "nb_reponse", "reference", "is_inactif"}
            Assert.IsTrue(table.Columns.Contains(colonne), "colonne " & colonne)
        Next
        Dim ligneSecond = table.Rows(0)
        Assert.AreEqual("Utilisateur TEST", CStr(ligneSecond("user_create")))
        Assert.AreEqual(1, CInt(ligneSecond("nb_reponse")))
        Assert.AreEqual(1, CInt(ligneSecond("nb_reponse_waiting")))
        Assert.AreEqual(LibelleSousTypeSeCompteRendu, CStr(ligneSecond("sous_type_libelle")))
        Dim lignePremier = table.Rows(1)
        Assert.AreEqual(autreAuteur, CLng(lignePremier("last_update_user_id")))
        ' Comportement actuel : nom du modificateur jamais renseigné quand il diffère du créateur.
        Assert.AreEqual(DBNull.Value, lignePremier("user_update"))
        Assert.AreEqual(DBNull.Value, lignePremier("user_validate"))
    End Sub

    <TestMethod()> Public Sub GetTableSousEpisode_NonComplet_SansJointure()
        Dim ctx = NouveauContexte()
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)

        Dim table = dao.GetTableSousEpisode(ctx.EpisodeId, isComplete:=False)

        CollectionAssert.AreEqual(New Long() {id}, IdsDeTable(table))
        Assert.IsFalse(table.Columns.Contains("user_create"))
        Assert.IsFalse(table.Columns.Contains("type_libelle"))
        Assert.IsFalse(table.Columns.Contains("nb_reponse"))
        Assert.AreEqual(20, table.Columns.Count)
    End Sub

    <TestMethod()> Public Sub GetTableSousEpisode_FiltresIdEtInactifs()
        Dim ctx = NouveauContexte()
        Dim actif = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Dim inactif = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Inactiver(inactif)

        CollectionAssert.AreEqual(New Long() {actif}, IdsDeTable(dao.GetTableSousEpisode(ctx.EpisodeId)))
        CollectionAssert.AreEqual(New Long() {inactif, actif},
                                  IdsDeTable(dao.GetTableSousEpisode(ctx.EpisodeId, isWithInactif:=True)))
        CollectionAssert.AreEqual(New Long() {inactif},
                                  IdsDeTable(dao.GetTableSousEpisode(0, inactif, True, True)))
        Assert.AreEqual(0, dao.GetTableSousEpisode(0, inactif).Rows.Count)
    End Sub

    ' --- CountSousEpisode et ResumeSousEpisode -------------------------------------------

    <TestMethod()> Public Sub CountSousEpisode_CompteLesActifsOuTous()
        Dim ctx = NouveauContexte()
        Dim autre = NouveauContexte()
        CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Inactiver(CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId))
        CreerSousEpisode(autre.EpisodeId, autre.UtilisateurId)

        Assert.AreEqual(2, dao.CountSousEpisode(ctx.EpisodeId))
        Assert.AreEqual(3, dao.CountSousEpisode(ctx.EpisodeId, True))
    End Sub

    <TestMethod()> Public Sub CountSousEpisode_EpisodeSansSousEpisode_DonneZero()
        Assert.AreEqual(0, dao.CountSousEpisode(NouveauContexte().EpisodeId))
    End Sub

    <TestMethod()> Public Sub ResumeSousEpisode_UneLigneParSousTypeAuPlurielSiBesoin()
        ' Ordre : premier sous-type rencontré dans la liste, elle-même du plus récent
        ' au plus ancien. Le pluriel ajoute un « s » au libellé.
        Dim ctx = NouveauContexte()
        CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId, SousTypeSeAdressage)
        CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId, SousTypeSeAdressage)
        CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId, SousTypeSeCompteRendu)
        Inactiver(CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId, SousTypeSeCertificat))

        Dim attendu = "1 " & LibelleSousTypeSeCompteRendu & vbCrLf & "2 " & LibelleSousTypeSeAdressage & "s"
        Assert.AreEqual(attendu, dao.ResumeSousEpisode(ctx.EpisodeId))
        Assert.AreEqual(attendu, dao.ResumeSousEpisode(ctx.EpisodeId, False))
    End Sub

    <TestMethod()> Public Sub ResumeSousEpisode_EpisodeSansSousEpisode_DonneUneChaineVide()
        Assert.AreEqual("", dao.ResumeSousEpisode(NouveauContexte().EpisodeId))
    End Sub

    ' --- ResetReponseRecue --------------------------------------------------------------

    <TestMethod()> Public Sub ResetReponseRecue_EffaceLIndicateurEtLaDate()
        Dim ctx = NouveauContexte()
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        CreerReponseSousEpisode(id, ctx.UtilisateurId)
        Assert.IsTrue(CBool(Valeur("is_reponse_recue", id)))

        Using connexion As New SqlConnection(ChaineConnexion(Compte.Client))
            connexion.Open()
            Using transaction = connexion.BeginTransaction()
                dao.ResetReponseRecue(connexion, New SousEpisode With {.Id = id}, transaction)
                transaction.Commit()
            End Using
        End Using

        Assert.IsFalse(CBool(Valeur("is_reponse_recue", id)))
        Assert.AreEqual(DBNull.Value, Valeur("horodate_last_recu", id))
    End Sub

    <TestMethod()> Public Sub ResetReponseRecue_SousEpisodeInexistant_Leve()
        Using connexion As New SqlConnection(ChaineConnexion(Compte.Client))
            connexion.Open()
            Using transaction = connexion.BeginTransaction()
                Try
                    dao.ResetReponseRecue(connexion, New SousEpisode With {.Id = SousEpisodeAbsent}, transaction)
                    Assert.Fail("Aucune ligne touchée : une erreur est attendue.")
                Catch ex As AssertFailedException
                    Throw
                Catch ex As Exception
                    StringAssert.Contains(ex.Message, "0 au lieu de 1")
                End Try
                transaction.Rollback()
            End Using
        End Using
    End Sub

    ' --- updateValidation --------------------------------------------------------------

    <TestMethod()> Public Sub updateValidation_HorsTransaction_EnregistreLeValideur()
        Dim ctx = NouveauContexte()
        Dim valideur = CreerUtilisateur(avecCle:=False)
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Dim avant = Date.Now

        Assert.IsTrue(dao.updateValidation(Nothing, id, Nothing, New Utilisateur With {.UtilisateurId = CInt(valideur)}))

        Assert.AreEqual(valideur, CLng(Valeur("validate_user_id", id)))
        VerifierProche(avant, CDate(Valeur("horodate_validate", id)), "horodate_validate")
        Assert.AreEqual(DBNull.Value, Valeur("signature", id), "la validation seule ne signe pas")
    End Sub

    <TestMethod()> Public Sub updateValidation_DansUneTransactionAnnulee_NeLaisseRien()
        Dim ctx = NouveauContexte()
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)

        Using connexion As New SqlConnection(ChaineConnexion(Compte.Client))
            connexion.Open()
            Using transaction = connexion.BeginTransaction()
                dao.updateValidation(connexion, id, transaction, New Utilisateur With {.UtilisateurId = CInt(ctx.UtilisateurId)})
                transaction.Rollback()
            End Using
        End Using

        Assert.AreEqual(DBNull.Value, Valeur("validate_user_id", id))
    End Sub

    <TestMethod()> Public Sub updateValidation_SousEpisodeInexistant_Leve()
        Try
            dao.updateValidation(Nothing, SousEpisodeAbsent, Nothing, New Utilisateur With {.UtilisateurId = 1})
            Assert.Fail("Aucune ligne touchée : une erreur est attendue.")
        Catch ex As AssertFailedException
            Throw
        Catch ex As Exception
            StringAssert.Contains(ex.Message, "(0)")
        End Try
    End Sub

    ' --- inactiverSousEpisode -------------------------------------------------------------

    <TestMethod()> Public Sub inactiverSousEpisode_PoseLIndicateurSansToucherAuReste()
        Dim ctx = NouveauContexte()
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Dim voisin = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)

        Assert.IsTrue(dao.inactiverSousEpisode(Nothing, id, Nothing))

        Assert.IsTrue(CBool(Valeur("is_inactif", id)))
        Assert.IsFalse(CBool(Valeur("is_inactif", voisin)))
        Assert.AreEqual(1, dao.CountSousEpisode(ctx.EpisodeId))
    End Sub

    <TestMethod()> Public Sub inactiverSousEpisode_DansUneTransactionAnnulee_NeLaisseRien()
        Dim ctx = NouveauContexte()
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)

        Using connexion As New SqlConnection(ChaineConnexion(Compte.Client))
            connexion.Open()
            Using transaction = connexion.BeginTransaction()
                Assert.IsTrue(dao.inactiverSousEpisode(connexion, id, transaction))
                transaction.Rollback()
            End Using
        End Using

        Assert.IsFalse(CBool(Valeur("is_inactif", id)))
    End Sub

    <TestMethod()> Public Sub inactiverSousEpisode_SousEpisodeInexistant_Leve()
        Try
            dao.inactiverSousEpisode(Nothing, SousEpisodeAbsent, Nothing)
            Assert.Fail("Aucune ligne touchée : une erreur est attendue.")
        Catch ex As AssertFailedException
            Throw
        Catch ex As Exception
            StringAssert.Contains(ex.Message, "(0)")
        End Try
    End Sub

    ' --- WriteDocAndEventualySign (partie base seulement) ------------------------------

    <TestMethod()> Public Sub WriteDocAndEventualySign_DepotEnEchec_AnnuleLaSignature()
        Dim ctx = NouveauContexte()
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Dim bean = dao.GetById(id)
        Dim signataire As New Utilisateur With {.UtilisateurId = CInt(ctx.UtilisateurId)}

        Try
            dao.WriteDocAndEventualySign(bean, New Byte() {1, 2, 3}, "signature-de-test", Date.Now, signataire, Nothing)
            Assert.Fail("Le dépôt sur tests.invalid devrait échouer.")
        Catch ex As AssertFailedException
            Throw
        Catch ex As Exception
        End Try

        Assert.AreEqual(DBNull.Value, Valeur("signature", id))
        Assert.AreEqual(DBNull.Value, Valeur("validate_user_id", id))
        Assert.AreEqual(DBNull.Value, Valeur("horodate_validate", id))
        Assert.AreEqual(0L, bean.ValidateUserId, "le bean n'est mis à jour qu'après validation")
    End Sub

    <TestMethod()> Public Sub WriteDocAndEventualySign_SansSignature_NEcritRienEnBase()
        Dim ctx = NouveauContexte()
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Dim bean = dao.GetById(id)

        Try
            dao.WriteDocAndEventualySign(bean, New Byte() {1, 2, 3}, "", Date.Now,
                                         New Utilisateur With {.UtilisateurId = CInt(ctx.UtilisateurId)}, Nothing)
            Assert.Fail("Le dépôt sur tests.invalid devrait échouer.")
        Catch ex As AssertFailedException
            Throw
        Catch ex As Exception
        End Try

        Assert.AreEqual(DBNull.Value, Valeur("validate_user_id", id))
        Assert.AreEqual(DBNull.Value, Valeur("signature", id))
    End Sub

    ' --- AppartientAEpisode (serveur) ------------------------------------------------------

    <TestMethod()> Public Sub AppartientAEpisode_VraiSeulementPourLaBonnePaire()
        UtiliserCompte(Compte.Web)
        Dim ctx = NouveauContexte()
        Dim autre = NouveauContexte()
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)

        Assert.IsTrue(dao.AppartientAEpisode(id, ctx.EpisodeId))
        Assert.IsFalse(dao.AppartientAEpisode(id, autre.EpisodeId))
        Assert.IsFalse(dao.AppartientAEpisode(SousEpisodeAbsent, ctx.EpisodeId))
    End Sub

    <TestMethod()> Public Sub AppartientAEpisode_IdentifiantsNulsOuNegatifs_Faux()
        UtiliserCompte(Compte.Web)
        Assert.IsFalse(dao.AppartientAEpisode(0, 1))
        Assert.IsFalse(dao.AppartientAEpisode(1, 0))
        Assert.IsFalse(dao.AppartientAEpisode(-1, -1))
    End Sub

    <TestMethod()> Public Sub AppartientAEpisode_SousEpisodeInactif_ResteRattache()
        UtiliserCompte(Compte.Web)
        Dim ctx = NouveauContexte()
        Dim id = CreerSousEpisode(ctx.EpisodeId, ctx.UtilisateurId)
        Inactiver(id)

        Assert.IsTrue(dao.AppartientAEpisode(id, ctx.EpisodeId))
    End Sub

End Class
