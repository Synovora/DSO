Imports System.Web.Mvc
Imports Oasis_Common
Imports Oasis_Web.Oasis_Web.Controllers

''' <summary>
''' Auto-suivi du portail (AutoSuiviController), sous oasis_web : liste des
''' paramètres que le patient peut mesurer lui-même, puis enregistrement de ses
''' mesures dans un épisode PARAMETRE clôturé. Les paramètres proposés viennent des
''' consignes SUIVI_CHRONIQUE du patient (EpisodeProtocoleCollaboratifDao).
''' </summary>
<TestClass()> Public Class AutoSuiviControllerTest
    Inherits TestIntegration

    Private Const Activite As String = "SUIVI_CHRONIQUE"

    ''' <summary>
    ''' Paramètre proposé à ce seul patient : groupe de paramètres (DRC) consigné
    ''' dans son parcours pour le suivi chronique. exclu pose exclusion_auto_suivi,
    ''' qu'aucun DAO n'écrit. Renvoie l'id du paramètre.
    ''' </summary>
    Private Shared Function ParametrePropose(idPatient As Long, description As String, Optional exclu As Boolean = False) As Long
        Dim idParametre = CreerParametreDeMesure(description)
        If exclu Then Executer("UPDATE oasis.oa_r_parametre SET exclusion_auto_suivi = 1 WHERE id = @p0", idParametre)
        Dim idGroupe = CreerDrc("Groupe " & description)
        ClasserDrcPourEpisode(idGroupe, Drc.EnumCategorieOasisCode.GroupeParametres)
        AssocierParametreDrcEpisode(idGroupe, idParametre)
        CreerConsignePatientEpisode(idPatient, idGroupe, Activite)
        Return idParametre
    End Function

    Private Shared Function Envoyer(idInternaute As Long, donnees As String, Optional ByRef controleur As AutoSuiviController = Nothing) As ActionResult
        controleur = ControleurPortail(Of AutoSuiviController)(idInternaute, "POST")
        Dim appele = controleur
        Return EnFrancaisPortail(Function() appele.AutoSuiviValidate(donnees))
    End Function

    Private Shared Function EpisodesAutoSuivi(idPatient As Long) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode WHERE patient_id = @p0 AND type = 'PARAMETRE'", idPatient))
    End Function

    Private Shared Function TotalEpisodesAutoSuivi() As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode WHERE type = 'PARAMETRE'"))
    End Function

    Private Shared Function Proposes(idInternaute As Long) As Long()
        Dim vue = VuePortail(ControleurPortail(Of AutoSuiviController)(idInternaute).Index())
        Return DirectCast(vue.ViewData("ParametresAutoSuivi"), List(Of Parametre)).Select(Function(p) p.Id).ToArray()
    End Function

    ' --- Contrôle d'accès ---------------------------------------------------------------

    <TestMethod()> Public Sub LeFiltreRefuseUnVisiteurAnonyme()
        Dim controleur = ControleurPortailAnonyme(Of AutoSuiviController)()
        VerifierNonAuthentifiePortail(AutorisationPortail(controleur, "Index"))
        VerifierNonAuthentifiePortail(AutorisationPortail(controleur, "AutoSuiviValidate", GetType(String)))
    End Sub

    <TestMethod()> Public Sub LeFiltreLaissePasserUnInternauteAuthentifie()
        Dim controleur = ControleurPortail(Of AutoSuiviController)(CreerComptePortail(CreerPatient()))
        Assert.IsNull(AutorisationPortail(controleur, "Index"))
        Assert.IsNull(AutorisationPortail(controleur, "AutoSuiviValidate", GetType(String)))
    End Sub

    <TestMethod()> Public Sub SansFiltreUnAnonymeEstQuandMemeRefuse()
        VerifierAccesRefusePortail(ControleurPortailAnonyme(Of AutoSuiviController)().Index())
        VerifierAccesRefusePortail(ControleurPortailAnonyme(Of AutoSuiviController)("POST").AutoSuiviValidate("1=1"))
        Assert.AreEqual(0, TotalEpisodesAutoSuivi())
    End Sub

    <TestMethod()> Public Sub UnInternauteSansPatientEstRefuse()
        Dim idInternaute = CreerInternaute()
        VerifierAccesRefusePortail(ControleurPortail(Of AutoSuiviController)(idInternaute).Index())
        VerifierAccesRefusePortail(Envoyer(idInternaute, "1=1"))
    End Sub

    ' --- Liste des paramètres -------------------------------------------------------

    <TestMethod()> Public Sub SeulsLesParametresDuPatientConnecteSontProposes()
        Dim patientA = CreerPatient("PORTAIL", "Alpha")
        Dim patientB = CreerPatient("PORTAIL", "Beta")
        Dim internauteA = CreerComptePortail(patientA)
        CreerComptePortail(patientB)
        Dim poids = ParametrePropose(patientA, "Poids A")
        Dim exclu = ParametrePropose(patientA, "Exclu A", exclu:=True)
        Dim deB = ParametrePropose(patientB, "Glycemie B")

        Dim ids = Proposes(internauteA)

        CollectionAssert.AreEqual(New Long() {poids}, ids)
        CollectionAssert.DoesNotContain(ids, exclu)
        CollectionAssert.DoesNotContain(ids, deB)
    End Sub

    <TestMethod()> Public Sub LIndexMontreLaFicheDuPatient()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)

        Dim vue = VuePortail(ControleurPortail(Of AutoSuiviController)(idInternaute).Index())

        Assert.AreEqual(CInt(idPatient), DirectCast(vue.ViewData("Patient"), Patient).PatientId)
        Assert.AreEqual(0, DirectCast(vue.ViewData("ParametresAutoSuivi"), List(Of Parametre)).Count)
        Assert.AreEqual("LAYOUT_VERTICAL", CStr(vue.ViewData("ModeName")))
    End Sub

    ' --- Enregistrement ---------------------------------------------------------------

    <TestMethod()> Public Sub UneMesureAutoriseeEstEnregistreeDansUnEpisodeDuPatient()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Dim poids = ParametrePropose(idPatient, "Poids")
        Dim controleur As AutoSuiviController = Nothing

        Dim resultat = Envoyer(idInternaute, $"{poids}=72.5", controleur)

        VerifierStatutPortail(resultat, 200)
        Assert.AreEqual(True, ContextePortail(controleur).FausseSession("autosuivi"))
        Assert.AreEqual(1, EpisodesAutoSuivi(idPatient))
        Dim idEpisode = CLng(Scalaire("SELECT MAX(episode_id) FROM oasis.oa_episode WHERE patient_id = @p0", idPatient))
        Assert.AreEqual("CLOTURE", CStr(Scalaire("SELECT etat FROM oasis.oa_episode WHERE episode_id = @p0", idEpisode)).Trim())
        Assert.AreEqual("PATIENT", CStr(Scalaire("SELECT type_profil FROM oasis.oa_episode WHERE episode_id = @p0", idEpisode)).Trim())
        Assert.AreEqual(0L, CLng(Scalaire("SELECT user_creation FROM oasis.oa_episode WHERE episode_id = @p0", idEpisode)))
        Assert.AreEqual(72.5D, CDec(Scalaire("SELECT valeur FROM oasis.oa_episode_parametre WHERE episode_id = @p0 AND parametre_id = @p1",
                                             idEpisode, poids)))
        Assert.AreEqual(CLng(idPatient), CLng(Scalaire("SELECT patient_id FROM oasis.oa_episode_parametre WHERE episode_id = @p0", idEpisode)))
    End Sub

    <TestMethod()> Public Sub PlusieursMesuresPartagentLeMemeEpisode()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Dim poids = ParametrePropose(idPatient, "Poids")
        Dim tension = ParametrePropose(idPatient, "Tension")

        VerifierStatutPortail(Envoyer(idInternaute, $"{poids}=70&{tension}=12.8&abc=3&{poids}x=1&{tension}="), 200)

        Assert.AreEqual(1, EpisodesAutoSuivi(idPatient))
        Assert.AreEqual(2, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode_parametre WHERE patient_id = @p0", idPatient)))
    End Sub

    <TestMethod()> Public Sub UnParametreDUnAutrePatientEstRefuseSansRienEcrire()
        Dim patientA = CreerPatient("PORTAIL", "Alpha")
        Dim patientB = CreerPatient("PORTAIL", "Beta")
        Dim internauteA = CreerComptePortail(patientA)
        CreerComptePortail(patientB)
        Dim deA = ParametrePropose(patientA, "Poids A")
        Dim deB = ParametrePropose(patientB, "Glycemie B")

        VerifierStatutPortail(Envoyer(internauteA, $"{deB}=5"), 400, "Paramètre non autorisé")
        ' Validation complète avant écriture : une mesure valide ne passe pas non plus.
        VerifierStatutPortail(Envoyer(internauteA, $"{deA}=70&{deB}=5"), 400, "Paramètre non autorisé")

        Assert.AreEqual(0, TotalEpisodesAutoSuivi())
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode_parametre WHERE patient_id IN (@p0, @p1)", patientA, patientB)))
    End Sub

    <TestMethod()> Public Sub UnParametreExcluDeLAutoSuiviEstRefuse()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Dim exclu = ParametrePropose(idPatient, "Exclu", exclu:=True)

        VerifierStatutPortail(Envoyer(idInternaute, $"{exclu}=1"), 400, "Paramètre non autorisé")
        Assert.AreEqual(0, EpisodesAutoSuivi(idPatient))
    End Sub

    <TestMethod()> Public Sub UneValeurIllisibleEstRefuseeSansRienEcrire()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Dim poids = ParametrePropose(idPatient, "Poids")

        VerifierStatutPortail(Envoyer(idInternaute, $"{poids}=soixante"), 400, "Valeur invalide")
        Assert.AreEqual(0, EpisodesAutoSuivi(idPatient))
    End Sub

    <TestMethod()> Public Sub SansMesureExploitableRienNEstEcrit()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        ParametrePropose(idPatient, "Poids")

        For Each donnees In New String() {Nothing, "", "abc", "=5", "1", "1=2=3"}
            VerifierStatutPortail(Envoyer(idInternaute, donnees), 400, "Aucune mesure valide")
        Next
        Assert.AreEqual(0, EpisodesAutoSuivi(idPatient))
    End Sub

    <TestMethod()> Public Sub UneVirguleDecimaleEstLueCommeSeparateurDeMilliers()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Dim poids = ParametrePropose(idPatient, "Poids")

        VerifierStatutPortail(Envoyer(idInternaute, $"{poids}=72,5"), 200)

        ' Comportement actuel : la valeur est lue en culture invariante avec
        ' NumberStyles.Number, qui accepte la virgule comme séparateur de milliers.
        ' « 72,5 », saisi à la française, est enregistré 725.
        Assert.AreEqual(725D, CDec(Scalaire("SELECT valeur FROM oasis.oa_episode_parametre WHERE patient_id = @p0 AND parametre_id = @p1",
                                            idPatient, poids)))
    End Sub

    <TestMethod()> Public Sub LesDonneesSontDecodeesAvantLeDecoupage()
        Dim idPatient = CreerPatient()
        Dim idInternaute = CreerComptePortail(idPatient)
        Dim poids = ParametrePropose(idPatient, "Poids")
        Dim tension = ParametrePropose(idPatient, "Tension")

        VerifierStatutPortail(Envoyer(idInternaute, $"{poids}%3D70%26{tension}%3D13"), 200)

        Assert.AreEqual(2, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode_parametre WHERE patient_id = @p0", idPatient)))
    End Sub

End Class
