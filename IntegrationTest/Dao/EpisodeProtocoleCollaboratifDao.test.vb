Imports Oasis_Common

''' <summary>
''' EpisodeProtocoleCollaboratifDao contre la base de test. À la création d'un
''' épisode, le client lourd génère les paramètres à mesurer et les actes
''' paramédicaux à réaliser d'après le protocole standard de l'activité et les
''' consignes du parcours du patient : ces appels tournent sous oasis_client.
''' Oasis_Web lit la liste des paramètres pour l'auto-suivi (AutoSuiviController) :
''' ce cas tourne sous oasis_web.
''' </summary>
<TestClass()> Public Class EpisodeProtocoleCollaboratifDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New EpisodeProtocoleCollaboratifDao

    ''' <summary>Activité propre aux tests : aucun autre jeu ne lui attache de DRC standard.</summary>
    Private Const ActiviteDeTest As String = "IT_EPISODE"

    Private Const CategorieContexte As Integer = Drc.EnumCategorieOasisCode.Contexte
    Private Const CategoriePrevention As Integer = Drc.EnumCategorieOasisCode.Prevention
    Private Const CategorieActe As Integer = Drc.EnumCategorieOasisCode.ActeParamedical
    Private Const CategorieGroupe As Integer = Drc.EnumCategorieOasisCode.GroupeParametres
    Private Const CategorieProtocole As Integer = Drc.EnumCategorieOasisCode.ProtocoleCollaboratif

    ''' <summary>Groupe de paramètres (DRC classée GroupeParametres) contenant les paramètres donnés.</summary>
    Private Shared Function Groupe(libelle As String, ParamArray parametres() As Long) As Long
        Dim idDrc = CreerDrc(libelle)
        ClasserDrcPourEpisode(idDrc, CategorieGroupe)
        For Each idParametre In parametres
            AssocierParametreDrcEpisode(idDrc, idParametre)
        Next
        Return idDrc
    End Function

    Private Shared Function DrcClassee(libelle As String, categorie As Integer) As Long
        Dim idDrc = CreerDrc(libelle)
        ClasserDrcPourEpisode(idDrc, categorie)
        Return idDrc
    End Function

    ' --- Paramètres à mesurer ----------------------------------------------------------

    <TestMethod()> Public Sub LesParametresViennentDesGroupesStandardEtDesConsignesEnVigueur()
        Dim idPatient = CreerPatient()
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        Dim p1 = CreerParametreDeMesure("P1")
        Dim p2 = CreerParametreDeMesure("P2")
        Dim p3 = CreerParametreDeMesure("P3")
        Dim p4 = CreerParametreDeMesure("P4")
        Dim p5 = CreerParametreDeMesure("P5")
        Dim p6 = CreerParametreDeMesure("P6")
        Dim p7 = CreerParametreDeMesure("P7")
        Dim p8 = CreerParametreDeMesure("P8")
        Dim p9 = CreerParametreDeMesure("P9")
        ' Protocole standard : un groupe retenu, une DRC d'une autre catégorie et le
        ' groupe d'une autre activité écartés.
        CreerDrcStandardEpisode(ActiviteDeTest, Groupe("Constantes", p1, p2), CategorieGroupe)
        Dim acte = DrcClassee("Acte avec parametre", CategorieActe)
        AssocierParametreDrcEpisode(acte, p3)
        CreerDrcStandardEpisode(ActiviteDeTest, acte, CategorieActe)
        CreerDrcStandardEpisode("IT_AUTRE", Groupe("Autre activite", p4), CategorieGroupe)
        ' Consignes du patient : en vigueur, à venir, échue, annulée ; et celle d'un autre patient.
        CreerConsignePatientEpisode(idPatient, Groupe("Consigne", p5), ActiviteDeTest)
        CreerConsignePatientEpisode(idPatient, Groupe("A venir", p6), ActiviteDeTest, dateDebut:=Date.Today.AddDays(10))
        CreerConsignePatientEpisode(idPatient, Groupe("Echue", p7), ActiviteDeTest, dateFin:=Date.Today.AddDays(-1))
        CreerConsignePatientEpisode(idPatient, Groupe("Annulee", p8), ActiviteDeTest, inactif:=True)
        CreerConsignePatientEpisode(idAutrePatient, Groupe("Autre patient", p9), ActiviteDeTest)

        Dim liste = dao.GetListeParametreByPatientEtTypeEpisode(idPatient, ActiviteDeTest)

        CollectionAssert.AreEquivalent(New Long() {p1, p2, p5}, liste)
    End Sub

    <TestMethod()> Public Sub UnParametrePresentDansDeuxGroupesNApparaitQuUneFois()
        Dim idPatient = CreerPatient()
        Dim p1 = CreerParametreDeMesure("P1")
        Dim p2 = CreerParametreDeMesure("P2")
        CreerDrcStandardEpisode(ActiviteDeTest, Groupe("Groupe A", p1, p2), CategorieGroupe)
        CreerConsignePatientEpisode(idPatient, Groupe("Groupe B", p2), ActiviteDeTest)

        Dim liste = dao.GetListeParametreByPatientEtTypeEpisode(idPatient, ActiviteDeTest)

        CollectionAssert.AreEquivalent(New Long() {p1, p2}, liste)
    End Sub

    <TestMethod()> Public Sub SansProtocoleNiConsigneLaListeDesParametresEstVide()
        Assert.AreEqual(0, dao.GetListeParametreByPatientEtTypeEpisode(CreerPatient(), ActiviteDeTest).Count)
    End Sub

    <TestMethod()> Public Sub LAutoSuiviLitLesParametresSousLeCompteServeur()
        Dim idPatient = CreerPatient()
        Dim p1 = CreerParametreDeMesure("P1")
        CreerDrcStandardEpisode(ActiviteDeTest, Groupe("Constantes", p1), CategorieGroupe)
        UtiliserCompte(Compte.Web)

        CollectionAssert.AreEqual(New Long() {p1}, dao.GetListeParametreByPatientEtTypeEpisode(idPatient, ActiviteDeTest))
    End Sub

    <TestMethod()> Public Sub EnAgeScolaireLesBornesDAgeEnAnneesFiltrent()
        Dim idPatient = CreerPatient()
        PoserNaissancePatientEpisode(idPatient, Date.Today.AddYears(-8).AddMonths(-1))
        Const activite As String = "PREVENTION_ENFANT_SCOLAIRE"
        Dim dansLesBornes = CreerParametreDeMesure("5 a 10 ans")
        Dim tropJeune = CreerParametreDeMesure("10 ans et plus")
        Dim tropVieux = CreerParametreDeMesure("6 ans au plus")
        Dim sansBorne = CreerParametreDeMesure("Sans borne")
        Dim consigneTropJeune = CreerParametreDeMesure("Consigne 12 ans et plus")
        CreerDrcStandardEpisode(activite, Groupe("Scolaire 5-10", dansLesBornes), CategorieGroupe, ageMin:=5, ageMax:=10)
        CreerDrcStandardEpisode(activite, Groupe("Scolaire 10+", tropJeune), CategorieGroupe, ageMin:=10)
        CreerDrcStandardEpisode(activite, Groupe("Scolaire -6", tropVieux), CategorieGroupe, ageMax:=6)
        CreerDrcStandardEpisode(activite, Groupe("Scolaire", sansBorne), CategorieGroupe)
        CreerConsignePatientEpisode(idPatient, Groupe("Consigne 12+", consigneTropJeune), activite, ageMin:=12)

        Dim liste = dao.GetListeParametreByPatientEtTypeEpisode(idPatient, activite)

        CollectionAssert.Contains(liste, dansLesBornes)
        CollectionAssert.Contains(liste, sansBorne)
        CollectionAssert.DoesNotContain(liste, tropJeune)
        CollectionAssert.DoesNotContain(liste, tropVieux)
        CollectionAssert.DoesNotContain(liste, consigneTropJeune)
    End Sub

    <TestMethod()> Public Sub AvantTroisAnsLesBornesDAgeSontEnMois()
        ' Âge en jours (plus 4) comparé aux bornes en mois converties en jours.
        Dim idPatient = CreerPatient()
        PoserNaissancePatientEpisode(idPatient, Date.Today.AddMonths(-6))
        Const activite As String = "PREVENTION_ENFANT_PRE_SCOLAIRE"
        Dim dansLesBornes = CreerParametreDeMesure("3 a 9 mois")
        Dim tropJeune = CreerParametreDeMesure("12 mois et plus")
        Dim tropVieux = CreerParametreDeMesure("4 mois au plus")
        CreerDrcStandardEpisode(activite, Groupe("Prescolaire 3-9", dansLesBornes), CategorieGroupe, ageMin:=3, ageMax:=9)
        CreerDrcStandardEpisode(activite, Groupe("Prescolaire 12+", tropJeune), CategorieGroupe, ageMin:=12)
        CreerDrcStandardEpisode(activite, Groupe("Prescolaire -4", tropVieux), CategorieGroupe, ageMax:=4)

        Dim liste = dao.GetListeParametreByPatientEtTypeEpisode(idPatient, activite)

        CollectionAssert.Contains(liste, dansLesBornes)
        CollectionAssert.DoesNotContain(liste, tropJeune)
        CollectionAssert.DoesNotContain(liste, tropVieux)
    End Sub

    ' --- Actes paramédicaux à réaliser -------------------------------------------------

    <TestMethod()> Public Sub LesActesViennentDuStandardDesConsignesEtDesProtocolesCollaboratifs()
        Dim idPatient = CreerPatient()
        Dim acteStandard = DrcClassee("Acte standard", CategorieActe)
        Dim protocole = DrcClassee("Protocole collaboratif", CategorieProtocole)
        Dim acteDuProtocole1 = DrcClassee("Acte du protocole 1", CategorieActe)
        Dim acteDuProtocole2 = DrcClassee("Acte du protocole 2", CategorieActe)
        Dim prevention = DrcClassee("Prevention consignee", CategoriePrevention)
        Dim acteConsigne = DrcClassee("Acte consigne", CategorieActe)
        Dim contexte = DrcClassee("Contexte consigne", CategorieContexte)
        Dim acteAVenir = DrcClassee("Acte a venir", CategorieActe)
        CreerDrcStandardEpisode(ActiviteDeTest, acteStandard, CategorieActe)
        CreerDrcStandardEpisode(ActiviteDeTest, protocole, CategorieProtocole)
        CreerDrcStandardEpisode(ActiviteDeTest, Groupe("Groupe ignore", CreerParametreDeMesure("P1")), CategorieGroupe)
        AssocierActeProtocoleEpisode(protocole, acteDuProtocole1)
        AssocierActeProtocoleEpisode(protocole, acteDuProtocole2)
        ' L'acte standard, consigné aussi, ne doit apparaître qu'une fois.
        AssocierActeProtocoleEpisode(protocole, acteStandard)
        CreerConsignePatientEpisode(idPatient, prevention, ActiviteDeTest)
        CreerConsignePatientEpisode(idPatient, acteConsigne, ActiviteDeTest)
        CreerConsignePatientEpisode(idPatient, acteStandard, ActiviteDeTest)
        CreerConsignePatientEpisode(idPatient, contexte, ActiviteDeTest)
        CreerConsignePatientEpisode(idPatient, acteAVenir, ActiviteDeTest, dateDebut:=Date.Today.AddDays(3))

        Dim liste = dao.GetListeActeParamedicalByPatientEtTypeEpisode(idPatient, ActiviteDeTest)

        CollectionAssert.AreEquivalent(
            New Long() {acteStandard, prevention, acteConsigne, acteDuProtocole1, acteDuProtocole2}, liste)
    End Sub

    <TestMethod()> Public Sub UnProtocoleConsigneAjouteSesActes()
        Dim idPatient = CreerPatient()
        Dim protocole = DrcClassee("Protocole consigne", CategorieProtocole)
        Dim acte = DrcClassee("Acte du protocole", CategorieActe)
        AssocierActeProtocoleEpisode(protocole, acte)
        CreerConsignePatientEpisode(idPatient, protocole, ActiviteDeTest)

        CollectionAssert.AreEqual(New Long() {acte},
                                  dao.GetListeActeParamedicalByPatientEtTypeEpisode(idPatient, ActiviteDeTest))
    End Sub

    <TestMethod()> Public Sub SansProtocoleNiConsigneLaListeDesActesEstVide()
        Assert.AreEqual(0, dao.GetListeActeParamedicalByPatientEtTypeEpisode(CreerPatient(), ActiviteDeTest).Count)
    End Sub

    <TestMethod()> Public Sub EnAgeScolaireLesBornesDAgeFiltrentAussiLesActes()
        Dim idPatient = CreerPatient()
        PoserNaissancePatientEpisode(idPatient, Date.Today.AddYears(-8).AddMonths(-1))
        Const activite As String = "PREVENTION_ENFANT_SCOLAIRE"
        Dim retenu = DrcClassee("Acte 6 a 9 ans", CategorieActe)
        Dim ecarte = DrcClassee("Acte 11 ans et plus", CategorieActe)
        CreerDrcStandardEpisode(activite, retenu, CategorieActe, ageMin:=6, ageMax:=9)
        CreerDrcStandardEpisode(activite, ecarte, CategorieActe, ageMin:=11)

        Dim liste = dao.GetListeActeParamedicalByPatientEtTypeEpisode(idPatient, activite)

        CollectionAssert.Contains(liste, retenu)
        CollectionAssert.DoesNotContain(liste, ecarte)
    End Sub

    ' --- Génération sur un épisode -----------------------------------------------------

    <TestMethod()> Public Sub LaGenerationCreeLesParametresEtLesActesDeLEpisode()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim poids = CreerParametreDeMesure("Poids genere", "kg", ordre:=7, entier:=3, nbDecimales:=1)
        Dim acte = DrcClassee("Acte genere", CategorieActe)
        CreerDrcStandardEpisode(ActiviteDeTest, Groupe("Constantes", poids), CategorieGroupe)
        CreerDrcStandardEpisode(ActiviteDeTest, acte, CategorieActe)
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur, typeActivite:=ActiviteDeTest)
        Dim episodeLu = New EpisodeDao().GetEpisodeById(CInt(idEpisode))

        dao.GenerateParametreEtProtocoleCollaboratifByEpisode(episodeLu)

        Dim ligneParametre = New EpisodeParametreDao().GetEpisodeParametreByParametreIdAndEpisodeId(CInt(poids), idEpisode)
        Assert.IsTrue(ligneParametre.Id > 0)
        Assert.AreEqual(idPatient, ligneParametre.PatientId)
        Assert.AreEqual(0D, ligneParametre.Valeur.Value)
        Assert.AreEqual("Poids genere", ligneParametre.Description)
        Assert.AreEqual("kg", ligneParametre.Unite)
        Assert.AreEqual(7, ligneParametre.Ordre)
        Assert.AreEqual(3, ligneParametre.Entier)
        Assert.AreEqual(1, ligneParametre.Decimal)
        Assert.IsFalse(ligneParametre.Inactif)

        Dim actes = New EpisodeActeParamedicalDao().getAllEpisodeActeParamedicalByEpisodeId(idEpisode)
        Assert.AreEqual(1, actes.Rows.Count)
        Assert.AreEqual(acte, CLng(actes.Rows(0)("drc_id")))
        Assert.AreEqual("", CStr(actes.Rows(0)("observation")))
        Assert.AreEqual("PARAMEDICAL", CStr(actes.Rows(0)("type_observation")))
    End Sub

    <TestMethod()> Public Sub RegenererSurLeMemeEpisodeEchoueSurLePremierParametre()
        ' Comportement actuel : un paramètre déjà présent lève « Collision », que la
        ' génération ne rattrape pas ; les actes, eux, seraient ignorés sans bruit.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim poids = CreerParametreDeMesure("Poids genere", "kg")
        CreerDrcStandardEpisode(ActiviteDeTest, Groupe("Constantes", poids), CategorieGroupe)
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur, typeActivite:=ActiviteDeTest)
        Dim episodeLu = New EpisodeDao().GetEpisodeById(CInt(idEpisode))
        dao.GenerateParametreEtProtocoleCollaboratifByEpisode(episodeLu)

        Dim erreur = Assert.ThrowsException(Of Exception)(Sub() dao.GenerateParametreEtProtocoleCollaboratifByEpisode(episodeLu))

        StringAssert.Contains(erreur.Message, "Collision")
        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode_parametre WHERE episode_id = @p0", idEpisode)))
    End Sub

    <TestMethod()> Public Sub SansProtocoleLaGenerationNeCreeRien()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur, typeActivite:=ActiviteDeTest)

        dao.GenerateParametreEtProtocoleCollaboratifByEpisode(New EpisodeDao().GetEpisodeById(CInt(idEpisode)))

        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode_parametre WHERE episode_id = @p0", idEpisode)))
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode_acte_paramedical WHERE episode_id = @p0", idEpisode)))
    End Sub

End Class
