Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' EpisodeTypeActiviteDao contre la base de test. La création d'épisode et les
''' écrans de paramétrage DRC du client lourd lisent les types d'activité : tout
''' tourne sous oasis_client. La table oa_r_activite_episode n'est mise en cache
''' par aucun singleton ; chaque test y pose les lignes dont il a besoin.
''' </summary>
<TestClass()> Public Class EpisodeTypeActiviteDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New EpisodeTypeActiviteDao

    Private Shared Function CodesActivite(table As DataTable) As String()
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CStr(r("oa_activite_type"))).ToArray()
    End Function

    <TestMethod()> Public Sub LesActivitesActivesSontDonneesDansLOrdreDuReferentiel()
        CreerActiviteEpisode("IT_ACT_C", 30)
        CreerActiviteEpisode("IT_ACT_A", 10)
        CreerActiviteEpisode("IT_ACT_X", 5, inactif:=True)
        CreerActiviteEpisode("IT_ACT_B", 20, description:="Activite B")

        Dim table = dao.GetAllEpisodeActivite()

        Dim miennes = CodesActivite(table).Where(Function(c) c.StartsWith("IT_ACT_")).ToArray()
        CollectionAssert.AreEqual(New String() {"IT_ACT_A", "IT_ACT_B", "IT_ACT_C"}, miennes)
        Dim ligneB = table.Rows.Cast(Of DataRow)().Single(Function(r) CStr(r("oa_activite_type")) = "IT_ACT_B")
        Assert.AreEqual("Activite B", CStr(ligneB("oa_activite_description")))
    End Sub

    <TestMethod()> Public Sub LaLectureDUneActiviteParSonCodeEchoue()
        ' Comportement actuel : la requête filtre sur activite_type alors que la
        ' colonne, lue partout ailleurs, s'appelle oa_activite_type. Aucun appelant.
        CreerActiviteEpisode("PATHOLOGIE_AIGUE", 1)

        Dim erreur = Assert.ThrowsException(Of SqlException)(Sub() dao.GetActiviteEpisodeById("PATHOLOGIE_AIGUE"))

        Assert.AreEqual(207, erreur.Number, "nom de colonne non valide")
    End Sub

    ' --- Activités proposées pour un patient -------------------------------------------

    Private Shared Sub PoserReferentielDActivites()
        ViderActivitesEpisode()
        CreerActiviteEpisode("PATHOLOGIE_AIGUE", 1)
        CreerActiviteEpisode("PREVENTION_SUIVI_GYNECOLOGIQUE", 2, genre:="F")
        CreerActiviteEpisode("SOCIAL", 3)
        CreerActiviteEpisode("PREVENTION_ENFANT_SCOLAIRE", 4, enfant:=True)
        CreerActiviteEpisode("IT_SANS_LIBELLE", 5)
        CreerActiviteEpisode("SUIVI_CHRONIQUE", 6, inactif:=True)
        CreerActiviteEpisode("PREVENTION_AUTRE", 7, genre:="H")
    End Sub

    <TestMethod()> Public Sub UnePatienteSeVoitProposerLesActivitesFemininesSaufSocial()
        PoserReferentielDActivites()
        Dim patiente As New Patient With {.PatientGenreId = "F", .PatientDateNaissance = New Date(1976, 5, 1)}

        Dim libelles = dao.GetTypeActiviteEpisodeByPatient(patiente)

        ' Sans limiteAgeEnfant ni AgeMinPreventionFemme dans la configuration, l'âge ne
        ' filtre rien. Un code sans libellé est écarté.
        CollectionAssert.AreEqual(New String() {
            "Pathologie Aiguë",
            "Suivi gynécologique",
            "Prévention de l'enfant en âge scolaire (à partir de 3 ans)",
            "Autre prévention"
        }, libelles)
    End Sub

    <TestMethod()> Public Sub UnPatientNeSeVoitPasProposerLesActivitesFeminines()
        PoserReferentielDActivites()
        ' Le genre est lu tel que la table patient le stocke, espaces compris.
        Dim patientHomme As New Patient With {.PatientGenreId = "M ", .PatientDateNaissance = New Date(1976, 5, 1)}

        Dim libelles = dao.GetTypeActiviteEpisodeByPatient(patientHomme)

        CollectionAssert.DoesNotContain(libelles, "Suivi gynécologique")
        CollectionAssert.Contains(libelles, "Pathologie Aiguë")
        ' Comportement actuel : seul « F » restreint ; un autre genre (ici « H ») ne filtre rien.
        CollectionAssert.Contains(libelles, "Autre prévention")
    End Sub

    <TestMethod()> Public Sub SansActiviteLaListeEstVide()
        ViderActivitesEpisode()
        Dim patiente As New Patient With {.PatientGenreId = "F", .PatientDateNaissance = New Date(1976, 5, 1)}

        Assert.AreEqual(0, dao.GetTypeActiviteEpisodeByPatient(patiente).Count)
    End Sub

End Class
