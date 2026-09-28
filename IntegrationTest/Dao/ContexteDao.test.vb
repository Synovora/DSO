Imports System.Globalization
Imports System.Threading
Imports Oasis_Common

''' <summary>
''' ContexteDao contre la base de test. Les contextes patient (lignes de
''' oa_antecedent de type C) sont créés, modifiés, annulés et transformés en
''' antécédents par le client lourd (RadFContextedetailEdit, RadFEpisodeDetail,
''' RadFEpisodeDetailCreation, FrmSousEpisode) ; la recherche des contextes échus et
''' leur transformation tournent au démarrage du client (ParametreOasisDao). Tout
''' est donc sous oasis_client.
''' </summary>
<TestClass()> Public Class ContexteDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New ContexteDao
    Private ReadOnly daoAntecedent As New AntecedentDao

    Private cultureAvant As CultureInfo

    ' Les UPDATE du DAO écrivent leurs dates par ToString("yyyy-MM-dd HH:mm:ss") :
    ' culture du poste, fr-FR.
    <TestInitialize>
    Public Sub PasserEnFrancais()
        cultureAvant = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
    End Sub

    <TestCleanup>
    Public Sub RetablirLaCulture()
        Thread.CurrentThread.CurrentCulture = cultureAvant
    End Sub

    Private Shared Function Auteur(utilisateurId As Long) As Utilisateur
        Return New Utilisateur With {.UtilisateurId = CInt(utilisateurId)}
    End Function

    Private Function Lire(contexteId As Long) As Antecedent
        Return daoAntecedent.GetAntecedentById(CInt(contexteId))
    End Function

    ''' <summary>
    ''' Objet d'historique préparé comme le fait RadFContextedetailEdit : recopie du
    ''' contexte relu. Un contexte écrit par CreationContexte n'a pas de dates ALD ;
    ''' InitAntecedentHistorisation les recopie en Date.MinValue, que SQL Server
    ''' refuse (voir ModificationContexte_DatesAldVides_EchoueSurLHistorique). On les
    ''' remplace par le 31/12/2999, comme ParametreOasisDao remplace les NULL.
    ''' </summary>
    Private Function HistoriqueCommeLEcran(contexteId As Long, utilisateurId As Long) As AntecedentHisto
        Dim histo As New AntecedentHisto
        AntecedentHistoCreationDao.InitAntecedentHistorisation(Lire(contexteId), Auteur(utilisateurId), histo)
        Dim sansDate As New Date(2999, 12, 31)
        If histo.AldDateDebut = Date.MinValue Then histo.AldDateDebut = sansDate
        If histo.AldDateFin = Date.MinValue Then histo.AldDateFin = sansDate
        If histo.AldDateDemande = Date.MinValue Then histo.AldDateDemande = sansDate
        Return histo
    End Function

    Private Shared Function DateSynthese(patientId As Long) As Object
        Return Scalaire("SELECT oa_patient_synthese_date_maj FROM oasis.oa_patient WHERE oa_patient_id = @p0", patientId)
    End Function

    Private Shared Function ColonneContexte(contexteId As Long, colonne As String) As Object
        Return Scalaire("SELECT " & colonne & " FROM oasis.oa_antecedent WHERE oa_antecedent_id = @p0", contexteId)
    End Function

    ' --- Création ------------------------------------------------------------------

    <TestMethod()> Public Sub CreationContexte_EcritLeContexteSonHistoriqueEtLaDateDeSynthese()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idDrc = CreerDrc()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        PoserDateMajSynthese(idPatient, Nothing)

        Dim idContexte = CreerContexteParcours(idPatient, idUtilisateur, idDrc, "Diabète de type 2",
                                               statutAffichage:="C", categorie:="B", diagnostic:=2, episodeId:=idEpisode)

        Assert.IsTrue(idContexte > 0)
        Dim lu = Lire(idContexte)
        Assert.AreEqual("C", lu.Type)
        Assert.AreEqual(CInt(idPatient), lu.PatientId)
        Assert.AreEqual(CInt(idDrc), lu.DrcId)
        Assert.AreEqual("Diabète de type 2", lu.Description)
        Assert.AreEqual(DateDebutAntecedentDeTest, lu.DateDebut.Date)
        Assert.AreEqual(FinContexteDeTest, lu.DateFin.Date)
        Assert.AreEqual(1, lu.Niveau)
        Assert.AreEqual("Patient", lu.Nature)
        Assert.AreEqual("C", lu.StatutAffichage)
        Assert.AreEqual("P", lu.StatutAffichageTransformation)
        Assert.AreEqual("B", lu.CategorieContexte)
        Assert.AreEqual(0, lu.Ordre1)
        Assert.AreEqual(2, lu.Diagnostic)
        Assert.AreEqual(idEpisode, lu.EpisodeId)
        Assert.IsFalse(lu.Inactif)
        Assert.AreEqual(CInt(idUtilisateur), lu.UserCreation)
        Assert.AreEqual(0, lu.UserModification)
        Assert.AreEqual(Date.Today, lu.DateCreation.Date)
        Assert.AreEqual(Date.Today, lu.DateModification.Date, "la création pose aussi la date de modification")

        Dim histo = HistoriqueAntecedent(idContexte)
        Assert.AreEqual(1, histo.Rows.Count)
        Assert.AreEqual(1, CInt(histo.Rows(0)("oa_antecedent_histo_etat_historisation")), "CreationAntecedent")
        Assert.AreEqual("C", CStr(histo.Rows(0)("oa_antecedent_type")))
        Assert.AreEqual(idUtilisateur, CLng(histo.Rows(0)("oa_antecedent_histo_utilisateur_historisation")))
        Assert.AreEqual(Date.Today, CDate(DateSynthese(idPatient)).Date)
    End Sub

    <TestMethod()> Public Sub CreationContexte_ConclusionDEpisode_RattacheLEpisodeAUnContexteZero()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim saisie As New Antecedent With {
            .PatientId = CInt(idPatient), .DrcId = CInt(CreerDrc()), .Description = "Conclusion",
            .DateDebut = DateDebutAntecedentDeTest, .DateFin = FinContexteDeTest,
            .StatutAffichage = "P", .StatutAffichageTransformation = "P", .CategorieContexte = "M",
            .Diagnostic = 1, .EpisodeId = idEpisode
        }

        Dim idContexte = dao.CreationContexte(saisie, New AntecedentHisto, Auteur(idUtilisateur), True,
                                              New Episode With {.Id = idEpisode, .PatientId = idPatient})

        ' Comportement actuel : ContexteConclusionEpisodeId n'est jamais affecté
        ' (ContexteDao.vb, lignes 240 et 268). Le lien épisode et contexte est écrit
        ' avec contexte_id = 0 au lieu de l'id du contexte créé.
        Assert.IsTrue(idContexte > 0)
        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode_contexte WHERE episode_id = @p0", idEpisode)))
        Assert.AreEqual(0L, CLng(Scalaire("SELECT contexte_id FROM oasis.oa_episode_contexte WHERE episode_id = @p0", idEpisode)))
        Assert.AreEqual(idPatient, CLng(Scalaire("SELECT patient_id FROM oasis.oa_episode_contexte WHERE episode_id = @p0", idEpisode)))
        Assert.AreEqual(1, HistoriqueAntecedent(idContexte).Rows.Count)
    End Sub

    <TestMethod()> Public Sub CreationContexte_DeuxConclusionsSurLeMemeEpisode_LaSecondeEchoueApresAvoirEcritLeContexte()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim episodeConclu As New Episode With {.Id = idEpisode, .PatientId = idPatient}
        Dim nouvelleConclusion = Function(texte As String) New Antecedent With {
            .PatientId = CInt(idPatient), .DrcId = CInt(CreerDrc()), .Description = texte,
            .DateDebut = DateDebutAntecedentDeTest, .DateFin = FinContexteDeTest,
            .StatutAffichage = "P", .StatutAffichageTransformation = "P", .CategorieContexte = "M",
            .Diagnostic = 1, .EpisodeId = idEpisode
        }
        Dim premier = dao.CreationContexte(nouvelleConclusion("Premier"), New AntecedentHisto, Auteur(idUtilisateur), True, episodeConclu)

        ' Comportement actuel : les deux liens porteraient contexte_id = 0 ;
        ' CreateEpisodeContexte refuse le doublon (épisode, 0). Le contexte est déjà
        ' inséré, son historique et la date de synthèse ne sont pas écrits.
        Dim erreur = Assert.ThrowsException(Of Exception)(
            Sub() dao.CreationContexte(nouvelleConclusion("Second"), New AntecedentHisto, Auteur(idUtilisateur), True, episodeConclu))

        StringAssert.Contains(erreur.Message, "Collision")
        Dim second = CLng(Scalaire("SELECT MAX(oa_antecedent_id) FROM oasis.oa_antecedent WHERE oa_antecedent_patient_id = @p0 AND oa_antecedent_type = 'C'",
                                   idPatient))
        Assert.AreNotEqual(premier, second)
        Assert.AreEqual("Second", CStr(ColonneContexte(second, "oa_antecedent_description")))
        Assert.AreEqual(0, HistoriqueAntecedent(second).Rows.Count)
        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode_contexte WHERE episode_id = @p0", idEpisode)))
    End Sub

    <TestMethod()> Public Sub CreationContexte_ConclusionSansEpisode_NeRattacheRien()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim saisie As New Antecedent With {
            .PatientId = CInt(idPatient), .DrcId = CInt(CreerDrc()), .Description = "Sans episode",
            .DateDebut = DateDebutAntecedentDeTest, .DateFin = FinContexteDeTest,
            .StatutAffichage = "P", .StatutAffichageTransformation = "P", .CategorieContexte = "M", .Diagnostic = 1
        }

        Dim idContexte = dao.CreationContexte(saisie, New AntecedentHisto, Auteur(idUtilisateur), True, Nothing)

        Assert.IsTrue(idContexte > 0)
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode_contexte WHERE patient_id = @p0", idPatient)))
    End Sub

    ' --- Contexte valide sur une DRC -----------------------------------------------

    <TestMethod()> Public Sub ExistContexteValideWithDrcId_ContexteEnCours_EstTrouve()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idDrc = CreerDrc()
        CreerContexteParcours(idPatient, idUtilisateur, idDrc)

        Assert.IsTrue(dao.ExistContexteValideWithDrcId(idPatient, idDrc))
        Assert.IsFalse(dao.ExistContexteValideWithDrcId(idPatient, CreerDrc()), "autre DRC")
        Assert.IsFalse(dao.ExistContexteValideWithDrcId(CreerPatient("AUTRE", "Patient"), idDrc), "autre patient")
    End Sub

    <TestMethod()> Public Sub ExistContexteValideWithDrcId_ContexteAnnuleArreteOuEchu_NestPasRetenu()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim drcAnnule = CreerDrc()
        Dim drcArrete = CreerDrc()
        Dim drcEchu = CreerDrc()
        Dim annule = CreerContexteParcours(idPatient, idUtilisateur, drcAnnule)
        dao.AnnulationContexte(Lire(annule), HistoriqueCommeLEcran(annule, idUtilisateur), Auteur(idUtilisateur))
        ArreterContexteMedical(CreerContexteParcours(idPatient, idUtilisateur, drcArrete))
        ' Échu hier : la comparaison se fait à GETDATE(), date et heure.
        CreerContexteParcours(idPatient, idUtilisateur, drcEchu, dateFin:=Date.Today.AddDays(-1))

        Assert.IsFalse(dao.ExistContexteValideWithDrcId(idPatient, drcAnnule), "annulé")
        Assert.IsFalse(dao.ExistContexteValideWithDrcId(idPatient, drcArrete), "arrêté")
        Assert.IsFalse(dao.ExistContexteValideWithDrcId(idPatient, drcEchu), "échu")
    End Sub

    <TestMethod()> Public Sub ExistContexteValideWithDrcId_AntecedentSurLaMemeDrc_NestPasUnContexte()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idDrc = CreerDrc()
        CreerAntecedent(idPatient, idUtilisateur, drcId:=idDrc)

        Assert.IsFalse(dao.ExistContexteValideWithDrcId(idPatient, idDrc))
        Assert.IsNull(dao.GetByDrcId(idPatient, idDrc))
    End Sub

    <TestMethod()> Public Sub GetByDrcId_RendLeContexteEnCoursOuNothing()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idDrc = CreerDrc()
        Dim drcEchu = CreerDrc()
        Dim idContexte = CreerContexteParcours(idPatient, idUtilisateur, idDrc, "Grossesse en cours")
        CreerContexteParcours(idPatient, idUtilisateur, drcEchu, dateFin:=Date.Today.AddDays(-1))

        Dim trouve = dao.GetByDrcId(idPatient, idDrc)

        Assert.IsNotNull(trouve)
        Assert.AreEqual(CInt(idContexte), trouve.Id)
        Assert.AreEqual("Grossesse en cours", trouve.Description)
        Assert.AreEqual("C", trouve.Type)
        Assert.IsNull(dao.GetByDrcId(idPatient, drcEchu), "échu")
        Assert.IsNull(dao.GetByDrcId(idPatient, CreerDrc()), "aucun contexte")
    End Sub

    ' --- Contextes échus et transformation -----------------------------------------

    <TestMethod()> Public Sub GetContexteObsolete_RendLesContextesPubliesEchusEtActifs()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim hier = Date.Today.AddDays(-1)
        Dim echuPublie = CreerContexteParcours(idPatient, idUtilisateur, CreerDrc(), "Echu publie", dateFin:=hier)
        Dim echuCache = CreerContexteParcours(idPatient, idUtilisateur, CreerDrc(), "Echu cache", dateFin:=hier, statutAffichage:="C")
        CreerContexteParcours(idPatient, idUtilisateur, CreerDrc(), "Echu occulte", dateFin:=hier, statutAffichage:="O")
        Dim echuAnnule = CreerContexteParcours(idPatient, idUtilisateur, CreerDrc(), "Echu annule", dateFin:=hier)
        dao.AnnulationContexte(Lire(echuAnnule), HistoriqueCommeLEcran(echuAnnule, idUtilisateur), Auteur(idUtilisateur))
        CreerContexteParcours(idPatient, idUtilisateur, CreerDrc(), "En cours")
        ' Un antécédent échu n'est pas un contexte.
        CreerAntecedent(idPatient, idUtilisateur)

        Dim table = dao.GetContexteObsolete()

        ' La requête couvre tous les patients : on ne garde que ceux du test.
        Dim retenus = table.Rows.Cast(Of DataRow)() _
            .Where(Function(r) CLng(r("oa_antecedent_patient_id")) = idPatient) _
            .Select(Function(r) CLng(r("oa_antecedent_id"))).OrderBy(Function(i) i).ToArray()
        CollectionAssert.AreEqual(New Long() {echuPublie, echuCache}, retenus)
    End Sub

    <TestMethod()> Public Sub TransformationEnAntecedent_ChangeLeTypeRemetLaHierarchieEtSupprimeLesChaines()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idContexte = CreerContexteParcours(idPatient, idUtilisateur, CreerDrc(), "Grossesse")
        Dim idAntecedent = CreerAntecedent(idPatient, idUtilisateur)
        Dim daoChaine As New ChaineEpisodeDao
        daoChaine.Create(New ChaineEpisode With {.AntecedentId = idContexte, .ChaineId = idAntecedent})
        daoChaine.Create(New ChaineEpisode With {.AntecedentId = idAntecedent, .ChaineId = idContexte})
        Dim histo = HistoriqueCommeLEcran(idContexte, idUtilisateur)

        Assert.IsTrue(dao.TransformationEnAntecedent(CInt(idContexte), histo, "Grossesse (03.2020)", "C", Auteur(idUtilisateur)))

        Dim lu = Lire(idContexte)
        Assert.AreEqual("A", lu.Type)
        Assert.AreEqual("Grossesse (03.2020)", lu.Description)
        Assert.AreEqual(New Date(2999, 12, 31), lu.DateFin.Date)
        Assert.AreEqual("", lu.Nature)
        Assert.AreEqual("C", lu.StatutAffichage)
        Assert.AreEqual(1, lu.Niveau)
        Assert.AreEqual(0, lu.Niveau1Id)
        Assert.AreEqual(0, lu.Niveau2Id)
        Assert.AreEqual(990, lu.Ordre1)
        Assert.AreEqual(0, lu.Ordre2)
        Assert.AreEqual(0, lu.Ordre3)
        Assert.AreEqual(Date.Today, lu.DateModification.Date)
        Assert.AreEqual(Date.Today.AddMonths(6), lu.ChaineEpisodeDateFin.Date, "ChaineEpisodePeriode = 6 mois")
        ' Comportement actuel : l'auteur de la modification est écrit en dur à 1,
        ' quel que soit l'utilisateur connecté (ContexteDao.vb, ligne 139).
        Assert.AreEqual(1, lu.UserModification)
        ' Seules les chaînes dont le contexte est l'origine sont supprimées.
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_chaine_episode WHERE antecedent_id = @p0", idContexte)))
        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_chaine_episode WHERE antecedent_id = @p0", idAntecedent)))

        Dim lignes = HistoriqueAntecedent(idContexte)
        Assert.AreEqual(2, lignes.Rows.Count, "création, puis transformation")
        Dim derniere = lignes.Rows(1)
        Assert.AreEqual(5, CInt(derniere("oa_antecedent_histo_etat_historisation")), "ReactivationAntecedent")
        Assert.AreEqual("A", CStr(derniere("oa_antecedent_type")))
        Assert.AreEqual(990, CInt(derniere("oa_antecedent_ordre_affichage1")))
        Assert.AreEqual(idUtilisateur, CLng(derniere("oa_antecedent_histo_utilisateur_historisation")))
        ' Comportement actuel : l'historique garde l'ancienne description, la nouvelle
        ' n'est pas recopiée dans l'objet d'historique.
        Assert.AreEqual("Grossesse", CStr(derniere("oa_antecedent_description")))
    End Sub

    ' --- Modification et annulation ------------------------------------------------

    <TestMethod()> Public Sub ModificationContexte_DescriptionChangee_DateLaModificationEtLaSynthese()
        Dim idCreateur = CreerUtilisateur(avecCle:=False)
        Dim idModificateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idContexte = CreerContexteParcours(idPatient, idCreateur, CreerDrc(), "Avant")
        Executer("UPDATE oasis.oa_antecedent SET oa_antecedent_date_modification = @p0 WHERE oa_antecedent_id = @p1",
                 New Date(2020, 1, 1), idContexte)
        PoserDateMajSynthese(idPatient, Nothing)
        Dim lu = Lire(idContexte)
        Dim histo = HistoriqueCommeLEcran(idContexte, idModificateur)
        Dim modifie = daoAntecedent.Clone(lu)
        Dim autreDrc = CreerDrc()
        modifie.Description = "Après"
        modifie.DrcId = CInt(autreDrc)
        modifie.CategorieContexte = "B"
        modifie.DateDebut = New Date(2021, 5, 1)
        modifie.DateFin = New Date(2030, 6, 30)
        modifie.Ordre1 = 7
        modifie.Diagnostic = 3
        modifie.StatutAffichage = "C"
        modifie.StatutAffichageTransformation = "O"

        Assert.IsTrue(dao.ModificationContexte(modifie, histo, Auteur(idModificateur), lu.Description, lu.DrcId))

        Dim relu = Lire(idContexte)
        Assert.AreEqual("Après", relu.Description)
        Assert.AreEqual(CInt(autreDrc), relu.DrcId)
        Assert.AreEqual("B", relu.CategorieContexte)
        Assert.AreEqual(New Date(2021, 5, 1), relu.DateDebut.Date)
        Assert.AreEqual(New Date(2030, 6, 30), relu.DateFin.Date)
        Assert.AreEqual(7, relu.Ordre1)
        Assert.AreEqual(3, relu.Diagnostic)
        Assert.AreEqual("C", relu.StatutAffichage)
        Assert.AreEqual("O", relu.StatutAffichageTransformation)
        Assert.AreEqual(CInt(idModificateur), relu.UserModification)
        Assert.AreEqual(Date.Today, relu.DateModification.Date)
        Assert.AreEqual(Date.Today, CDate(DateSynthese(idPatient)).Date)

        Dim lignes = HistoriqueAntecedent(idContexte)
        Assert.AreEqual(2, lignes.Rows.Count)
        Assert.AreEqual(2, CInt(lignes.Rows(1)("oa_antecedent_histo_etat_historisation")), "ModificationAntecedent")
        Assert.AreEqual("Après", CStr(lignes.Rows(1)("oa_antecedent_description")))
        Assert.AreEqual(idModificateur, CLng(lignes.Rows(1)("oa_antecedent_histo_utilisateur_historisation")))
    End Sub

    <TestMethod()> Public Sub ModificationContexte_SansChangementDeTexteNiDeDrc_GardeLaDateEtLaSynthese()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idContexte = CreerContexteParcours(idPatient, idUtilisateur, CreerDrc(), "Inchangé")
        Dim dateAvant As New Date(2020, 1, 1, 10, 30, 0)
        Executer("UPDATE oasis.oa_antecedent SET oa_antecedent_date_modification = @p0 WHERE oa_antecedent_id = @p1",
                 dateAvant, idContexte)
        PoserDateMajSynthese(idPatient, Nothing)
        Dim lu = Lire(idContexte)
        Dim modifie = daoAntecedent.Clone(lu)
        modifie.CategorieContexte = "B"
        modifie.DateFin = New Date(2030, 6, 30)

        Assert.IsTrue(dao.ModificationContexte(modifie, HistoriqueCommeLEcran(idContexte, idUtilisateur), Auteur(idUtilisateur),
                                               lu.Description, lu.DrcId))

        Dim relu = Lire(idContexte)
        Assert.AreEqual("B", relu.CategorieContexte)
        Assert.AreEqual(New Date(2030, 6, 30), relu.DateFin.Date)
        Assert.AreEqual(dateAvant, relu.DateModification, "date de modification reprise telle quelle")
        Assert.AreEqual(DBNull.Value, DateSynthese(idPatient), "synthèse non datée")
        Assert.AreEqual(2, HistoriqueAntecedent(idContexte).Rows.Count, "l'historique est écrit quand même")
    End Sub

    <TestMethod()> Public Sub ModificationContexte_DatesAldVides_EchoueSurLHistorique()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idContexte = CreerContexteParcours(idPatient, idUtilisateur, CreerDrc(), "Avant")
        ' État que laisse CreationContexte, qui n'écrit pas les colonnes ALD, si le
        ' schéma ne leur donne pas de valeur par défaut.
        Executer("UPDATE oasis.oa_antecedent SET oa_antecedent_ald_date_debut = NULL, oa_antecedent_ald_date_fin = NULL," &
                 " oa_antecedent_ald_demande_date = NULL WHERE oa_antecedent_id = @p0", idContexte)
        Dim lu = Lire(idContexte)
        Dim histo As New AntecedentHisto
        AntecedentHistoCreationDao.InitAntecedentHistorisation(lu, Auteur(idUtilisateur), histo)
        Dim modifie = daoAntecedent.Clone(lu)
        modifie.Description = "Après"

        ' Comportement actuel : InitAntecedentHistorisation recopie les dates ALD
        ' vides en Date.MinValue, que le paramètre SqlDbType.DateTime refuse
        ' (SqlDateTime overflow). La mise à jour du contexte est déjà faite ; seul
        ' l'historique manque, et l'écran reçoit une exception.
        Dim erreur = Assert.ThrowsException(Of Exception)(
            Sub() dao.ModificationContexte(modifie, histo, Auteur(idUtilisateur), lu.Description, lu.DrcId))

        StringAssert.Contains(erreur.Message, "SqlDateTime")
        Assert.AreEqual("Après", CStr(ColonneContexte(idContexte, "oa_antecedent_description")))
        Assert.AreEqual(1, HistoriqueAntecedent(idContexte).Rows.Count)
    End Sub

    <TestMethod()> Public Sub AnnulationContexte_RendLeContexteInactifEtLHistorise()
        Dim idCreateur = CreerUtilisateur(avecCle:=False)
        Dim idAnnulateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idContexte = CreerContexteParcours(idPatient, idCreateur, CreerDrc())
        Dim autre = CreerContexteParcours(idPatient, idCreateur, CreerDrc())
        PoserDateMajSynthese(idPatient, Nothing)

        Assert.IsTrue(dao.AnnulationContexte(Lire(idContexte), HistoriqueCommeLEcran(idContexte, idAnnulateur), Auteur(idAnnulateur)))

        Dim relu = Lire(idContexte)
        Assert.IsTrue(relu.Inactif)
        Assert.AreEqual(CInt(idAnnulateur), relu.UserModification)
        Assert.AreEqual(Date.Today, relu.DateModification.Date)
        Assert.IsFalse(Lire(autre).Inactif)
        Assert.AreEqual(Date.Today, CDate(DateSynthese(idPatient)).Date)
        Dim lignes = HistoriqueAntecedent(idContexte)
        Assert.AreEqual(2, lignes.Rows.Count)
        Assert.AreEqual(4, CInt(lignes.Rows(1)("oa_antecedent_histo_etat_historisation")), "AnnulationAntecedent")
        Assert.IsTrue(CBool(lignes.Rows(1)("oa_antecedent_inactif")))
        Assert.AreEqual(idAnnulateur, CLng(lignes.Rows(1)("oa_antecedent_histo_utilisateur_historisation")))
    End Sub

    ' --- Liste pour les courriers ---------------------------------------------------

    <TestMethod()> Public Sub GetListOfContextebyPatient_ComposeLeLibelleEtTrieParCategorie()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim bio = CreerContexteParcours(idPatient, idUtilisateur, CreerDrc(), "Vit seule", categorie:="B")
        Dim suspicion = CreerContexteParcours(idPatient, idUtilisateur, CreerDrc(), "Asthme", categorie:="M", diagnostic:=2)
        Dim notion = CreerContexteParcours(idPatient, idUtilisateur, CreerDrc(), "HTA" & vbCrLf & "ancienne", categorie:="M", diagnostic:=3)
        ' Même seconde de modification pour les deux contextes médicaux : l'ordre se joue sur l'id.
        Dim dateCommune As New Date(2026, 3, 10, 9, 0, 0)
        Executer("UPDATE oasis.oa_antecedent SET oa_antecedent_date_modification = @p0 WHERE oa_antecedent_id IN (@p1, @p2)",
                 dateCommune, suspicion, notion)
        ' Exclus : non publié, annulé, arrêté, autre patient.
        CreerContexteParcours(idPatient, idUtilisateur, CreerDrc(), "Cache", statutAffichage:="C")
        Dim annule = CreerContexteParcours(idPatient, idUtilisateur, CreerDrc(), "Annule")
        dao.AnnulationContexte(Lire(annule), HistoriqueCommeLEcran(annule, idUtilisateur), Auteur(idUtilisateur))
        ArreterContexteMedical(CreerContexteParcours(idPatient, idUtilisateur, CreerDrc(), "Arrete"))
        CreerContexteParcours(CreerPatient("AUTRE", "Patient"), idUtilisateur, CreerDrc(), "Autre patient")

        Dim liste = dao.GetListOfContextebyPatient(CInt(idPatient))

        ' Catégorie décroissante (M avant B), puis date de modification et id décroissants.
        CollectionAssert.AreEqual(New Long() {notion, suspicion, bio}, liste.Select(Function(c) c.Id).ToArray())
        Assert.IsTrue(liste.All(Function(c) c.PatientId = idPatient))
        Dim prefixe = " " & dateCommune.ToString("MM.yyyy") & " : "
        ' Le retour à la ligne devient une espace.
        Assert.AreEqual(prefixe & "Notion de " & " " & "HTA ancienne", liste(0).Description)
        Assert.AreEqual(prefixe & "Suspicion de " & " " & "Asthme", liste(1).Description)
        ' Hors catégorie médicale : ni date ni préfixe de diagnostic.
        Assert.AreEqual(" Vit seule", liste(2).Description)
    End Sub

    <TestMethod()> Public Sub GetListOfContextebyPatient_DescriptionLongue_NestPasTronquee()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim longue = New String("x"c, 200)
        CreerContexteParcours(idPatient, idUtilisateur, CreerDrc(), longue, categorie:="B")

        Dim liste = dao.GetListOfContextebyPatient(CInt(idPatient))

        ' Comportement actuel : le résultat de Substring(0, 150) est jeté
        ' (ContexteDao.vb, ligne 454), la description garde ses 200 caractères.
        Assert.AreEqual(1, liste.Count)
        Assert.AreEqual(" " & longue, liste(0).Description)
    End Sub

    <TestMethod()> Public Sub GetListOfContextebyPatient_PatientSansContexte_RendUneListeVide()
        Assert.AreEqual(0, dao.GetListOfContextebyPatient(CInt(CreerPatient())).Count)
    End Sub

End Class
