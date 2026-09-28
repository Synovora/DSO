Imports Oasis_Common

''' <summary>
''' EpisodeParametreDao (celui d'Oasis_Common) contre la base de test. Le client
''' lourd saisit, corrige, annule et supprime les paramètres d'épisode, et lit le
''' poids pour les ordonnances : ces appels tournent sous oasis_client, y compris la
''' suppression, que docs/migrations/2026-09-27-suppression-client-complement.sql
''' accorde à ce compte. Oasis_Web enregistre les mesures d'auto-suivi
''' (AutoSuiviController) : ce cas tourne sous oasis_web.
''' </summary>
<TestClass()> Public Class EpisodeParametreDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New EpisodeParametreDao

    Private Const LigneAbsente As Integer = 987654321

    Private Shared Function IdsParametres(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("episode_parametre_id"))).ToArray()
    End Function

    Private Shared Function ValeurEnBase(idLigne As Long) As Object
        Return Scalaire("SELECT valeur FROM oasis.oa_episode_parametre WHERE episode_parametre_id = @p0", idLigne)
    End Function

    ' --- Création et lecture -----------------------------------------------------------

    <TestMethod()> Public Sub UnParametreCreeEstRelueAvecSesValeurs()
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, CreerUtilisateur(avecCle:=False))
        Dim idParametre = CreerParametreDeMesure("Frequence cardiaque", "bpm")

        Dim idLigne = CreerParametreEpisode(idEpisode, idPatient, idParametre, 64.5D, ordre:=4,
                                            description:="Frequence cardiaque", unite:="bpm")

        Dim relu = dao.GetEpisodeParametreById(CInt(idLigne))
        Assert.AreEqual(idLigne, relu.Id)
        Assert.AreEqual(idParametre, relu.ParametreId)
        Assert.AreEqual(idEpisode, relu.EpisodeId)
        Assert.AreEqual(idPatient, relu.PatientId)
        Assert.AreEqual(64.5D, relu.Valeur.Value)
        Assert.AreEqual("Frequence cardiaque", relu.Description)
        Assert.AreEqual(3, relu.Entier)
        Assert.AreEqual(1, relu.Decimal)
        Assert.AreEqual("bpm", relu.Unite)
        Assert.IsFalse(relu.ParametreAjoute)
        Assert.AreEqual(4, relu.Ordre)
        Assert.IsFalse(relu.Inactif)
    End Sub

    <TestMethod()> Public Sub UneValeurAbsenteEstEnregistreeNullEtRelueZero()
        ' Comportement actuel : BuildBean remplace NULL par 0, le Decimal? ne revient jamais vide.
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, CreerUtilisateur(avecCle:=False))

        Dim idLigne = CreerParametreEpisode(idEpisode, idPatient, CreerParametreDeMesure("Temperature", "C"), Nothing)

        Assert.IsTrue(IsDBNull(ValeurEnBase(idLigne)))
        Dim relu = dao.GetEpisodeParametreById(CInt(idLigne))
        Assert.IsTrue(relu.Valeur.HasValue)
        Assert.AreEqual(0D, relu.Valeur.Value)
    End Sub

    <TestMethod()> Public Sub UneLigneInexistanteLeveUneErreur()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetEpisodeParametreById(LigneAbsente))
        StringAssert.Contains(erreur.Message, "inexistant")
    End Sub

    <TestMethod()> Public Sub LaLigneDUnParametreDeLEpisodeEstRetrouvee()
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, CreerUtilisateur(avecCle:=False))
        Dim idParametre = CreerParametreDeMesure("Saturation", "%")
        Dim idLigne = CreerParametreEpisode(idEpisode, idPatient, idParametre, 97D)

        Dim trouve = dao.GetEpisodeParametreByParametreIdAndEpisodeId(CInt(idParametre), idEpisode)

        Assert.AreEqual(idLigne, trouve.Id)
        Assert.AreEqual(97D, trouve.Valeur.Value)
    End Sub

    <TestMethod()> Public Sub UnParametreAbsentDeLEpisodeDonneUneLigneVide()
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, CreerUtilisateur(avecCle:=False))
        Dim idParametre = CreerParametreDeMesure("Saturation", "%")

        Dim trouve = dao.GetEpisodeParametreByParametreIdAndEpisodeId(CInt(idParametre), idEpisode)

        Assert.AreEqual(0L, trouve.Id)
        Assert.AreEqual(0L, trouve.EpisodeId)
        Assert.AreEqual(0L, trouve.ParametreId)
        Assert.AreEqual(0D, trouve.Valeur.Value)
    End Sub

    <TestMethod()> Public Sub UnDoublonActifEstRefuse()
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, CreerUtilisateur(avecCle:=False))
        Dim idParametre = CreerParametreDeMesure("Poids test", "kg")
        CreerParametreEpisode(idEpisode, idPatient, idParametre, 70D)

        Dim erreur = Assert.ThrowsException(Of Exception)(Sub() CreerParametreEpisode(idEpisode, idPatient, idParametre, 71D))

        StringAssert.Contains(erreur.Message, "Collision")
        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode_parametre WHERE episode_id = @p0", idEpisode)))
    End Sub

    <TestMethod()> Public Sub UnParametreAnnuleNEmpechePasUneNouvelleSaisie()
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, CreerUtilisateur(avecCle:=False))
        Dim idParametre = CreerParametreDeMesure("Poids test", "kg")
        Dim annule = CreerParametreEpisode(idEpisode, idPatient, idParametre, 70D)
        Assert.IsTrue(dao.AnnulationEpisodeParametre(annule))

        Dim nouveau = CreerParametreEpisode(idEpisode, idPatient, idParametre, 71D)

        Assert.IsTrue(nouveau > annule)
    End Sub

    <TestMethod()> Public Sub LAutoSuiviEnregistreSesMesuresSousLeCompteServeur()
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisodeParametres(idPatient, CreerUtilisateur(avecCle:=False), Date.Now, typeProfil:="PATIENT")
        Dim idParametre = CreerParametreDeMesure("Glycemie", "g/l")
        UtiliserCompte(Compte.Web)

        Assert.IsTrue(dao.CreateEpisodeParametre(New EpisodeParametre With {
            .EpisodeId = idEpisode, .ParametreId = idParametre, .PatientId = idPatient,
            .Entier = 1, .Decimal = 2, .Unite = "g/l", .Ordre = 1, .Description = "Glycemie",
            .Valeur = 1.5D, .Inactif = False
        }))

        Dim relu = dao.GetEpisodeParametreByParametreIdAndEpisodeId(CInt(idParametre), idEpisode)
        Assert.AreEqual(1.5D, relu.Valeur.Value)
        Assert.AreEqual("g/l", relu.Unite)
    End Sub

    ' --- Liste d'un épisode ------------------------------------------------------------

    <TestMethod()> Public Sub LaListeDonneLesParametresActifsDansLOrdreDAffichage()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        Dim idAutreEpisode = CreerEpisode(idAutrePatient, idUtilisateur)
        Dim troisieme = CreerParametreEpisode(idEpisode, idPatient, CreerParametreDeMesure("C"), 3D, ordre:=30)
        Dim premier = CreerParametreEpisode(idEpisode, idPatient, CreerParametreDeMesure("A"), 1D, ordre:=10)
        Dim annule = CreerParametreEpisode(idEpisode, idPatient, CreerParametreDeMesure("D"), 4D, ordre:=5)
        dao.AnnulationEpisodeParametre(annule)
        Dim deuxieme = CreerParametreEpisode(idEpisode, idPatient, CreerParametreDeMesure("B"), 2D, ordre:=20)
        CreerParametreEpisode(idAutreEpisode, idAutrePatient, CreerParametreDeMesure("E"), 5D, ordre:=1)

        Dim table = dao.getAllParametreEpisodeByEpisodeId(idEpisode)

        CollectionAssert.AreEqual(New Long() {premier, deuxieme, troisieme}, IdsParametres(table))
    End Sub

    <TestMethod()> Public Sub UnEpisodeSansParametreDonneUneTableVide()
        Dim idEpisode = CreerEpisode(CreerPatient(), CreerUtilisateur(avecCle:=False))
        Assert.AreEqual(0, dao.getAllParametreEpisodeByEpisodeId(idEpisode).Rows.Count)
    End Sub

    ' --- Modifications -----------------------------------------------------------------

    <TestMethod()> Public Sub LaModificationCompleteEchoueToujours()
        ' Comportement actuel : ModificationEpisodeParametre déclare deux fois
        ' @patientId et jamais @parametreId, alors que son UPDATE s'en sert. SQL Server
        ' refuse la commande ; le DAO la relance en Exception. Seule la copie non
        ' compilée de oasis/Form/Episode l'appelle.
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, CreerUtilisateur(avecCle:=False))
        Dim idLigne = CreerParametreEpisode(idEpisode, idPatient, CreerParametreDeMesure("Poids test", "kg"), 70D)
        Dim lu = dao.GetEpisodeParametreById(CInt(idLigne))
        lu.Valeur = 75D
        lu.Description = "Modifie"

        Assert.ThrowsException(Of Exception)(Sub() dao.ModificationEpisodeParametre(lu))

        Dim relu = dao.GetEpisodeParametreById(CInt(idLigne))
        Assert.AreEqual(70D, relu.Valeur.Value)
        Assert.AreNotEqual("Modifie", relu.Description)
    End Sub

    <TestMethod()> Public Sub LaValeurEstCorrigeePuisVidee()
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, CreerUtilisateur(avecCle:=False))
        Dim idLigne = CreerParametreEpisode(idEpisode, idPatient, CreerParametreDeMesure("Poids test", "kg"), 70D)

        Assert.IsTrue(dao.ModificationValeurEpisodeParametre(idLigne, 68.2D))
        Assert.AreEqual(68.2D, dao.GetEpisodeParametreById(CInt(idLigne)).Valeur.Value)

        Assert.IsTrue(dao.ModificationValeurEpisodeParametre(idLigne, Nothing))
        Assert.IsTrue(IsDBNull(ValeurEnBase(idLigne)))
    End Sub

    <TestMethod()> Public Sub LAnnulationRetireLeParametreDeLaListe()
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, CreerUtilisateur(avecCle:=False))
        Dim idLigne = CreerParametreEpisode(idEpisode, idPatient, CreerParametreDeMesure("Poids test", "kg"), 70D)

        Assert.IsTrue(dao.AnnulationEpisodeParametre(idLigne))

        Assert.IsTrue(dao.GetEpisodeParametreById(CInt(idLigne)).Inactif)
        Assert.AreEqual(0, dao.getAllParametreEpisodeByEpisodeId(idEpisode).Rows.Count)
    End Sub

    ' --- Suppression -------------------------------------------------------------------

    <TestMethod()> Public Sub LaSuppressionRetireTousLesParametresDeLEpisodeEtRienDAutre()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        Dim idAutreEpisode = CreerEpisode(idAutrePatient, idUtilisateur)
        CreerParametreEpisode(idEpisode, idPatient, CreerParametreDeMesure("A"), 1D)
        Dim annule = CreerParametreEpisode(idEpisode, idPatient, CreerParametreDeMesure("B"), 2D)
        dao.AnnulationEpisodeParametre(annule)
        Dim ailleurs = CreerParametreEpisode(idAutreEpisode, idAutrePatient, CreerParametreDeMesure("C"), 3D)

        Assert.IsTrue(dao.SuppressionEpisodeParametreByEpisodeId(idEpisode))

        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode_parametre WHERE episode_id = @p0", idEpisode)))
        Assert.AreEqual(ailleurs, dao.GetEpisodeParametreById(CInt(ailleurs)).Id)
    End Sub

    ' --- Poids pour les ordonnances ----------------------------------------------------

    <TestMethod()> Public Sub LePoidsDeLEpisodeEstPrisEnPriorite()
        AssurerParametrePoids()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim ancien = CreerEpisodeParametres(idPatient, idUtilisateur, New Date(2026, 1, 1, 8, 0, 0))
        CreerParametreEpisode(ancien, idPatient, 1, 70D)
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        CreerParametreEpisode(idEpisode, idPatient, 1, 72D)

        Assert.AreEqual(72.0, dao.GetPoidsByEpisodeIdOrLastKnow(idEpisode, idPatient), 0.0001)
    End Sub

    <TestMethod()> Public Sub SansPoidsDansLEpisodeLeDernierPoidsConnuEstRepris()
        AssurerParametrePoids()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        CreerParametreEpisode(CreerEpisodeParametres(idPatient, idUtilisateur, New Date(2026, 1, 1, 8, 0, 0)), idPatient, 1, 70D)
        CreerParametreEpisode(CreerEpisodeParametres(idPatient, idUtilisateur, New Date(2026, 2, 1, 8, 0, 0)), idPatient, 1, 68D)
        ' Un poids nul n'est pas un poids connu.
        CreerParametreEpisode(CreerEpisodeParametres(idPatient, idUtilisateur, New Date(2026, 3, 1, 8, 0, 0)), idPatient, 1, 0D)
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)

        Assert.AreEqual(68.0, dao.GetPoidsByEpisodeIdOrLastKnow(idEpisode, idPatient), 0.0001)
    End Sub

    <TestMethod()> Public Sub UnPoidsAnnuleResteLeDernierPoidsConnu()
        ' Comportement actuel : le poids de l'épisode ignore les lignes annulées, mais
        ' la recherche du dernier poids connu ne les filtre pas.
        AssurerParametrePoids()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim ancien = CreerEpisodeParametres(idPatient, idUtilisateur, New Date(2026, 1, 1, 8, 0, 0))
        dao.AnnulationEpisodeParametre(CreerParametreEpisode(ancien, idPatient, 1, 70D))
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        dao.AnnulationEpisodeParametre(CreerParametreEpisode(idEpisode, idPatient, 1, 90D))

        Assert.AreEqual(90.0, dao.GetPoidsByEpisodeIdOrLastKnow(idEpisode, idPatient), 0.0001)
    End Sub

    <TestMethod()> Public Sub UnPatientSansPoidsDonneZero()
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, CreerUtilisateur(avecCle:=False))

        Assert.AreEqual(0.0, dao.GetPoidsByEpisodeIdOrLastKnow(idEpisode, idPatient), 0.0001)
    End Sub

    <TestMethod()> Public Sub LePoidsDUnPatientInexistantLeveUneErreur()
        ' Comportement actuel : aucune ligne patient, et le DAO lit Rows(0) sans vérifier.
        Assert.ThrowsException(Of IndexOutOfRangeException)(Sub() dao.GetPoidsByEpisodeIdOrLastKnow(1, 987654321))
    End Sub

    ' --- Fusion documentaire -----------------------------------------------------------

    <TestMethod()> Public Sub SansParametreLaFusionNeRenseigneRien()
        ' Passe par la procédure stockée oasis.GET_PARAMETRE_FUSION_DOC, que le compte
        ' client doit pouvoir exécuter. Son corps vient de l'export du schéma : le cas
        ' avec valeurs dépendra des colonnes de fusion qu'elle lit.
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, CreerUtilisateur(avecCle:=False))
        Dim fusion As New SousEpisodeFusion

        dao.AlimenteFusionDocumentParametres(fusion, idEpisode, idPatient)

        Assert.IsNull(fusion.Patient_Poids)
        Assert.IsNull(fusion.Patient_FC)
        Assert.IsNull(fusion.Patient_PAS)
    End Sub

End Class
