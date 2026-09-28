Imports System.Globalization
Imports System.Threading
Imports Oasis_Common

''' <summary>
''' EpisodeActeParamedicalDao contre la base de test. Seule la fiche épisode du
''' client lourd l'appelle : tout tourne sous oasis_client, y compris la
''' suppression, que docs/migrations/2026-09-27-suppression-client-complement.sql
''' accorde à ce compte.
''' </summary>
<TestClass()> Public Class EpisodeActeParamedicalDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New EpisodeActeParamedicalDao

    Private Const ActeAbsent As Integer = 987654321

    Private Shared Function IdsActes(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("oa_episode_acte_paramedical_id"))).ToArray()
    End Function

    Private Shared Function NombreActes(idEpisode As Long) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode_acte_paramedical WHERE episode_id = @p0", idEpisode))
    End Function

    Private Shared Function NouvelEpisode(ByRef idPatient As Long) As Long
        idPatient = CreerPatient()
        Return CreerEpisode(idPatient, CreerUtilisateur(avecCle:=False))
    End Function

    ' --- Création et lecture -----------------------------------------------------------

    <TestMethod()> Public Sub UnActeCreeEstRelueAvecSesValeurs()
        Dim idPatient As Long
        Dim idEpisode = NouvelEpisode(idPatient)
        Dim idDrc = CreerDrc("Pansement simple")

        Dim idActe = CreerActeParamedicalEpisode(idEpisode, idPatient, idDrc, observation:="A surveiller")

        Assert.IsTrue(idActe > 0)
        Dim relu = dao.GetEpisodeActeParamedicalById(CInt(idActe))
        Assert.AreEqual(idActe, relu.Id)
        Assert.AreEqual(idEpisode, relu.EpisodeId)
        Assert.AreEqual(idPatient, relu.PatientId)
        Assert.AreEqual(idDrc, relu.DrcId)
        Assert.AreEqual("A surveiller", relu.Observation)
        Assert.AreEqual("PARAMEDICAL", relu.TypeObservation)
        Assert.IsFalse(relu.Inactif)
        ' Colonnes que la création n'écrit pas : NULL, lues comme valeurs par défaut.
        Assert.AreEqual(0L, relu.UserId)
        Assert.AreEqual(Date.MinValue, relu.DateObservation)
        Assert.AreEqual(Date.MinValue, relu.DateModification)
    End Sub

    <TestMethod()> Public Sub UnActeInexistantLeveUneErreur()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetEpisodeActeParamedicalById(ActeAbsent))
        StringAssert.Contains(erreur.Message, "inexistant")
    End Sub

    <TestMethod()> Public Sub UnDoublonActifEstRefuseSansErreur()
        ' Comportement actuel : la collision est détectée puis avalée ; l'appelant ne
        ' reçoit que 0. Toute autre erreur SQL serait avalée de la même façon.
        Dim idPatient As Long
        Dim idEpisode = NouvelEpisode(idPatient)
        Dim idDrc = CreerDrc("Pansement simple")
        CreerActeParamedicalEpisode(idEpisode, idPatient, idDrc)

        Dim doublon = CreerActeParamedicalEpisode(idEpisode, idPatient, idDrc)

        Assert.AreEqual(0L, doublon)
        Assert.AreEqual(1, NombreActes(idEpisode))
    End Sub

    <TestMethod()> Public Sub UnActeAnnuleNEmpechePasDeLeRecreer()
        Dim idPatient As Long
        Dim idEpisode = NouvelEpisode(idPatient)
        Dim idDrc = CreerDrc("Pansement simple")
        Dim annule = CreerActeParamedicalEpisode(idEpisode, idPatient, idDrc, inactif:=True)

        Dim recree = CreerActeParamedicalEpisode(idEpisode, idPatient, idDrc)

        Assert.IsTrue(recree > annule)
        Assert.AreEqual(2, NombreActes(idEpisode))
    End Sub

    <TestMethod()> Public Sub LeMemeActeSurUnAutreEpisodeEstAccepte()
        Dim idPatient As Long
        Dim idEpisode = NouvelEpisode(idPatient)
        Dim idAutrePatient As Long
        Dim idAutreEpisode = NouvelEpisode(idAutrePatient)
        Dim idDrc = CreerDrc("Pansement simple")
        CreerActeParamedicalEpisode(idEpisode, idPatient, idDrc)

        Assert.IsTrue(CreerActeParamedicalEpisode(idAutreEpisode, idAutrePatient, idDrc) > 0)
    End Sub

    ' --- Liste d'un épisode ------------------------------------------------------------

    <TestMethod()> Public Sub LaListeRetientLesActesActifsDuTypeDemandeAvecLeLibelleDRC()
        Dim idPatient As Long
        Dim idEpisode = NouvelEpisode(idPatient)
        Dim idAutrePatient As Long
        Dim idAutreEpisode = NouvelEpisode(idAutrePatient)
        Dim pansement = CreerDrc("Pansement simple")
        Dim injection = CreerDrc("Injection sous-cutanee")
        Dim ecg = CreerDrc("ECG")
        Dim prise = CreerDrc("Prise de sang")
        Dim acte1 = CreerActeParamedicalEpisode(idEpisode, idPatient, pansement)
        Dim acte2 = CreerActeParamedicalEpisode(idEpisode, idPatient, injection)
        Dim medical = CreerActeParamedicalEpisode(idEpisode, idPatient, ecg, typeObservation:="MEDICAL")
        CreerActeParamedicalEpisode(idEpisode, idPatient, prise, inactif:=True)
        CreerActeParamedicalEpisode(idAutreEpisode, idAutrePatient, pansement)

        Dim paramedicaux = dao.getAllEpisodeActeParamedicalByEpisodeId(idEpisode)

        CollectionAssert.AreEquivalent(New Long() {acte1, acte2}, IdsActes(paramedicaux))
        Dim lignePansement = paramedicaux.Rows.Cast(Of DataRow)().Single(Function(r) CLng(r("oa_episode_acte_paramedical_id")) = acte1)
        Assert.AreEqual("Pansement simple", CStr(lignePansement("oa_drc_libelle")))
        Assert.AreEqual(pansement, CLng(lignePansement("drc_id")))

        CollectionAssert.AreEqual(New Long() {medical}, IdsActes(dao.getAllEpisodeActeParamedicalByEpisodeId(idEpisode, "MEDICAL")))
    End Sub

    <TestMethod()> Public Sub UnEpisodeSansActeDonneUneTableVide()
        Dim idPatient As Long
        Assert.AreEqual(0, dao.getAllEpisodeActeParamedicalByEpisodeId(NouvelEpisode(idPatient)).Rows.Count)
    End Sub

    ' --- Passage au médical ------------------------------------------------------------

    <TestMethod()> Public Sub TousLesActesActifsPassentAuMedical()
        Dim idPatient As Long
        Dim idEpisode = NouvelEpisode(idPatient)
        Dim idAutrePatient As Long
        Dim idAutreEpisode = NouvelEpisode(idAutrePatient)
        Dim acte1 = CreerActeParamedicalEpisode(idEpisode, idPatient, CreerDrc("Pansement simple"))
        Dim acte2 = CreerActeParamedicalEpisode(idEpisode, idPatient, CreerDrc("ECG"))
        Dim annule = CreerActeParamedicalEpisode(idEpisode, idPatient, CreerDrc("Prise de sang"), inactif:=True)
        Dim ailleurs = CreerActeParamedicalEpisode(idAutreEpisode, idAutrePatient, CreerDrc("Injection"))

        Assert.IsTrue(dao.PutAllEpisodeActeParamedicalToMedicalByEpisodeId(idEpisode))

        Assert.AreEqual("MEDICAL", dao.GetEpisodeActeParamedicalById(CInt(acte1)).TypeObservation)
        Assert.AreEqual("MEDICAL", dao.GetEpisodeActeParamedicalById(CInt(acte2)).TypeObservation)
        Assert.AreEqual("PARAMEDICAL", dao.GetEpisodeActeParamedicalById(CInt(annule)).TypeObservation)
        Assert.AreEqual("PARAMEDICAL", dao.GetEpisodeActeParamedicalById(CInt(ailleurs)).TypeObservation)
        Assert.AreEqual(0, dao.getAllEpisodeActeParamedicalByEpisodeId(idEpisode).Rows.Count)
        ' Les dates restées vides repartent à NULL, pas à Date.MinValue.
        Assert.IsTrue(IsDBNull(Scalaire("SELECT date_saisie_observation FROM oasis.oa_episode_acte_paramedical" &
                                        " WHERE oa_episode_acte_paramedical_id = @p0", acte1)))
    End Sub

    <TestMethod()> Public Sub PasserAuMedicalUnEpisodeSansActeNeFaitRien()
        Dim idPatient As Long
        Assert.IsTrue(dao.PutAllEpisodeActeParamedicalToMedicalByEpisodeId(NouvelEpisode(idPatient)))
    End Sub

    ' --- Modifications -----------------------------------------------------------------

    <TestMethod()> Public Sub LaModificationEnregistreChaqueChamp()
        Dim idPatient As Long
        Dim idEpisode = NouvelEpisode(idPatient)
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idActe = CreerActeParamedicalEpisode(idEpisode, idPatient, CreerDrc("Pansement simple"))
        Dim autreDrc = CreerDrc("Pansement complexe")
        Dim saisie As New Date(2026, 4, 1, 10, 15, 0)
        Dim modification As New Date(2026, 4, 2, 11, 45, 0)
        Dim lu = dao.GetEpisodeActeParamedicalById(CInt(idActe))
        lu.DrcId = autreDrc
        lu.Observation = "Plaie propre"
        lu.TypeObservation = "MEDICAL"
        lu.UserId = idUtilisateur
        lu.DateObservation = saisie
        lu.DateModification = modification
        lu.Inactif = True

        Assert.IsTrue(dao.ModificationEpisodeActeParamedical(lu))

        Dim relu = dao.GetEpisodeActeParamedicalById(CInt(idActe))
        Assert.AreEqual(autreDrc, relu.DrcId)
        Assert.AreEqual("Plaie propre", relu.Observation)
        Assert.AreEqual("MEDICAL", relu.TypeObservation)
        Assert.AreEqual(idUtilisateur, relu.UserId)
        Assert.AreEqual(saisie, relu.DateObservation)
        Assert.AreEqual(modification, relu.DateModification)
        Assert.IsTrue(relu.Inactif)
        Assert.AreEqual(idEpisode, relu.EpisodeId, "l'épisode ne change pas")
    End Sub

    <TestMethod()> Public Sub LObservationPrendLeProfilEtLHeureDeLAuteur()
        Dim idPatient As Long
        Dim idEpisode = NouvelEpisode(idPatient)
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idActe = CreerActeParamedicalEpisode(idEpisode, idPatient, CreerDrc("Pansement simple"))
        Dim auteurIde As New Utilisateur With {.UtilisateurId = CInt(idUtilisateur), .TypeProfil = "PARAMEDICAL"}

        ' Les dates partent en chaînes "yyyy-MM-dd HH:mm:ss" : culture du poste.
        Dim cultureAvant = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
        Try
            Assert.IsTrue(dao.ModificationEpisodeActeParamedicalObservation(idActe, "Soin realise", auteurIde))
        Finally
            Thread.CurrentThread.CurrentCulture = cultureAvant
        End Try

        Dim relu = dao.GetEpisodeActeParamedicalById(CInt(idActe))
        Assert.AreEqual("Soin realise", relu.Observation)
        Assert.AreEqual("PARAMEDICAL", relu.TypeObservation)
        Assert.AreEqual(idUtilisateur, relu.UserId)
        Assert.AreEqual(Date.Today, relu.DateObservation.Date)
        Assert.AreEqual(Date.Today, relu.DateModification.Date)
        Assert.IsFalse(relu.Inactif)
    End Sub

    ' --- Suppression -------------------------------------------------------------------

    <TestMethod()> Public Sub LaSuppressionRetireTousLesActesDeLEpisodeEtRienDAutre()
        Dim idPatient As Long
        Dim idEpisode = NouvelEpisode(idPatient)
        Dim idAutrePatient As Long
        Dim idAutreEpisode = NouvelEpisode(idAutrePatient)
        CreerActeParamedicalEpisode(idEpisode, idPatient, CreerDrc("Pansement simple"))
        CreerActeParamedicalEpisode(idEpisode, idPatient, CreerDrc("ECG"), inactif:=True)
        Dim ailleurs = CreerActeParamedicalEpisode(idAutreEpisode, idAutrePatient, CreerDrc("Injection"))

        Assert.IsTrue(dao.SuppressionEpisodeActeParamedicalByEpisodeId(idEpisode))

        Assert.AreEqual(0, NombreActes(idEpisode))
        Assert.AreEqual(ailleurs, dao.GetEpisodeActeParamedicalById(CInt(ailleurs)).Id)
    End Sub

End Class
