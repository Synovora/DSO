Imports System.Globalization
Imports System.Threading
Imports Oasis_Common

''' <summary>
''' EpisodeDao contre la base de test. Le client lourd crée, lit, modifie et liste
''' les épisodes : ces appels tournent sous oasis_client. Oasis_Web crée les
''' épisodes d'auto-suivi (AutoSuiviController) et relit un épisode pour vérifier
''' les droits sur un document (HabilitationsDocuments) : ces deux cas tournent
''' sous oasis_web.
'''
''' Plusieurs requêtes du DAO nomment leurs tables en trois parties
''' (oasis.oasis.oa_episode, [oasis].[oasis].[oa_relation_chaine_episode]...) : elles
''' visent la base « oasis » quel que soit le catalogue de la connexion. Leurs tests
''' ne peuvent tourner que si la base de test s'appelle oasis (OASIS_IT_DATABASE) ;
''' sinon ils finissent Inconclusive.
''' </summary>
<TestClass()> Public Class EpisodeDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New EpisodeDao

    Private Const EpisodeAbsent As Integer = 987654321

    Private Shared Sub ExigerBaseNommeeOasis()
        If Not String.Equals(NomBase, "oasis", StringComparison.OrdinalIgnoreCase) Then
            Assert.Inconclusive("Requête écrite avec des noms en trois parties oasis.oasis.* : elle vise la base " &
                                "« oasis » et non " & NomBase & ". Nommer la base de test oasis (OASIS_IT_DATABASE) pour l'exécuter.")
        End If
    End Sub

    Private Shared Function Auteur(idUtilisateur As Long) As Utilisateur
        Return New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}
    End Function

    Private Shared Function Ids(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("episode_id"))).ToArray()
    End Function

    ' --- Correspondance code et libellé d'activité ------------------------------------

    <TestMethod()> Public Sub ChaqueCodeDActiviteDonneSonLibelleEtRevient()
        Dim codes = {"PATHOLOGIE_AIGUE", "PREVENTION_AUTRE", "PREVENTION_ENFANT_PRE_SCOLAIRE", "PREVENTION_ENFANT_SCOLAIRE",
                     "PREVENTION_SUIVI_GROSSESSE", "PREVENTION_SUIVI_GYNECOLOGIQUE", "SUIVI_CHRONIQUE", "SOCIAL"}
        For Each code In codes
            Dim libelle = dao.GetItemTypeActiviteByCode(code)
            Assert.AreNotEqual("", libelle, code)
            Assert.AreEqual(code, dao.GetCodeTypeActiviteByItem(libelle))
        Next
        Assert.AreEqual("Pathologie Aiguë", dao.GetItemTypeActiviteByCode("PATHOLOGIE_AIGUE"))
    End Sub

    <TestMethod()> Public Sub UnCodeOuUnLibelleInconnuDonneUneChaineVide()
        ' VACCINATION existe comme code mais n'a pas de libellé.
        Assert.AreEqual("", dao.GetItemTypeActiviteByCode("VACCINATION"))
        Assert.AreEqual("", dao.GetItemTypeActiviteByCode("INCONNU"))
        Assert.AreEqual("", dao.GetCodeTypeActiviteByItem("Libellé inconnu"))
    End Sub

    ' --- Création et lecture -----------------------------------------------------------

    <TestMethod()> Public Sub UnEpisodeCreeEstRelueAvecSesValeursInitiales()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()

        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur, typeActivite:="SUIVI_CHRONIQUE", typeProfil:="PARAMEDICAL",
                                     commentaire:="Premiere visite", descriptionActivite:="Diabete")

        Assert.IsTrue(idEpisode > 0)
        Dim relu = dao.GetEpisodeById(CInt(idEpisode))
        Assert.AreEqual(idEpisode, relu.Id)
        Assert.AreEqual(idPatient, relu.PatientId)
        Assert.AreEqual("CONSULTATION", relu.Type)
        Assert.AreEqual("SUIVI_CHRONIQUE", relu.TypeActivite)
        Assert.AreEqual("PARAMEDICAL", relu.TypeProfil)
        Assert.AreEqual("Diabete", relu.DescriptionActivite)
        Assert.AreEqual("Premiere visite", relu.Commentaire)
        Assert.AreEqual(idUtilisateur, relu.UserCreation)
        Assert.AreEqual(Date.Today, relu.DateCreation.Date)
        Assert.AreEqual("EN_COURS", relu.Etat)
        Assert.IsFalse(relu.Inactif)
        ' Colonnes que CreateEpisode n'écrit pas : NULL, lues comme valeurs par défaut.
        Assert.AreEqual(0L, relu.UserModification)
        Assert.AreEqual(Date.MinValue, relu.DateModification)
        Assert.AreEqual("", relu.ObservationMedical)
        Assert.AreEqual("", relu.ObservationParamedical)
        Assert.AreEqual("", relu.Decision)
        Assert.AreEqual("", relu.ConclusionIdeType)
        Assert.AreEqual(0L, relu.ConclusionMedConsigneDrcId)
        Assert.AreEqual("", relu.ConclusionMedConsigneDenomination)
        Assert.AreEqual(0L, relu.ConclusionMedContexte1DrcId)
        Assert.AreEqual(0L, relu.ConclusionMedContexte3AntecedentId)
    End Sub

    <TestMethod()> Public Sub UnEpisodeInexistantLeveUneErreur()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetEpisodeById(EpisodeAbsent))
        StringAssert.Contains(erreur.Message, "inexistant")
    End Sub

    <TestMethod()> Public Sub LeServeurRelitUnEpisodePourLesDroitsDocumentaires()
        Dim idEpisode = CreerEpisode(CreerPatient(), CreerUtilisateur(avecCle:=False))
        UtiliserCompte(Compte.Web)

        Dim relu = dao.GetEpisodeById(CInt(idEpisode))

        Assert.AreEqual(idEpisode, relu.Id)
        Assert.IsTrue(relu.PatientId > 0)
    End Sub

    <TestMethod()> Public Sub UnSecondEpisodeEnCoursPourLeMemePatientEstRefuse()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        CreerEpisode(idPatient, idUtilisateur)

        Dim erreur = Assert.ThrowsException(Of Exception)(Sub() CreerEpisode(idPatient, idUtilisateur, typeEpisode:="VIRTUEL"))

        StringAssert.Contains(erreur.Message, "Collision")
        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode WHERE patient_id = @p0", idPatient)))
    End Sub

    <TestMethod()> Public Sub UnEpisodeEnCoursNeBloquePasUnAutrePatient()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        CreerEpisode(CreerPatient(), idUtilisateur)

        Assert.IsTrue(CreerEpisode(CreerPatient("AUTRE", "Patient"), idUtilisateur) > 0)
    End Sub

    <TestMethod()> Public Sub ApresClotureUnNouvelEpisodePeutEtreOuvert()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim premier = CreerEpisode(idPatient, idUtilisateur)
        CloturerEpisode(premier, idUtilisateur)

        Dim second = CreerEpisode(idPatient, idUtilisateur)

        Assert.IsTrue(second > premier)
        Assert.AreEqual("CLOTURE", dao.GetEpisodeById(CInt(premier)).Etat)
        Assert.AreEqual("EN_COURS", dao.GetEpisodeById(CInt(second)).Etat)
    End Sub

    <TestMethod()> Public Sub UnEpisodeEnCoursAnnuleBloqueToujoursLaCreation()
        ' Comportement actuel : le contrôle de collision ne regarde que l'état, pas
        ' la colonne inactif. Un épisode EN_COURS annulé empêche d'en ouvrir un autre.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim annule = CreerEpisode(idPatient, idUtilisateur)
        DesactiverEpisode(annule, idUtilisateur)

        Assert.ThrowsException(Of Exception)(Sub() CreerEpisode(idPatient, idUtilisateur))
    End Sub

    <TestMethod()> Public Sub UnEpisodeDeParametresEstCreeClotureALaDateDonnee()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        ' Un épisode EN_COURS n'empêche pas la saisie de paramètres.
        CreerEpisode(idPatient, idUtilisateur)
        Dim quand As New Date(2026, 3, 14, 9, 30, 0)

        Dim idEpisode = CreerEpisodeParametres(idPatient, idUtilisateur, quand)

        Assert.IsTrue(idEpisode > 0)
        Dim relu = dao.GetEpisodeById(CInt(idEpisode))
        Assert.AreEqual("PARAMETRE", relu.Type)
        Assert.AreEqual("PARAMETRE", relu.TypeActivite)
        Assert.AreEqual("CLOTURE", relu.Etat)
        Assert.AreEqual(quand, relu.DateCreation)
        Assert.AreEqual(Date.Today, relu.DateModification.Date)
        Assert.AreEqual(idUtilisateur, relu.UserCreation)
    End Sub

    <TestMethod()> Public Sub LAutoSuiviCreeUnEpisodeDeParametresSansUtilisateur()
        ' AutoSuiviController, sous le compte du serveur, avec l'utilisateur 0.
        Dim idPatient = CreerPatient()
        UtiliserCompte(Compte.Web)
        Dim episodeAutoSuivi As New Episode With {
            .Commentaire = "AutoSuivi",
            .DateCreation = Date.Now,
            .UserCreation = 0,
            .PatientId = idPatient,
            .Type = "PARAMETRE",
            .TypeActivite = "PARAMETRE",
            .DescriptionActivite = "",
            .TypeProfil = "PATIENT",
            .Etat = "CLOTURE"
        }
        Dim cultureAvant = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
        Dim idEpisode As Long
        Try
            idEpisode = dao.CreateEpisode(episodeAutoSuivi, 0)
        Finally
            Thread.CurrentThread.CurrentCulture = cultureAvant
        End Try

        Assert.IsTrue(idEpisode > 0)
        Dim relu = dao.GetEpisodeById(CInt(idEpisode))
        Assert.AreEqual("PATIENT", relu.TypeProfil)
        Assert.AreEqual("CLOTURE", relu.Etat)
        Assert.AreEqual(0L, relu.UserCreation)
        Assert.AreEqual("AutoSuivi", relu.Commentaire)
    End Sub

    <TestMethod()> Public Sub UnEpisodeDeVaccinationEchappeAuControleDeCollision()
        ' Comportement actuel : la branche VACCINATION n'a pas de IF NOT EXISTS. Elle
        ' crée un second épisode EN_COURS, qui bloque ensuite toute consultation.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim consultation = CreerEpisode(idPatient, idUtilisateur)

        Dim vaccination = CreerEpisode(idPatient, idUtilisateur, typeEpisode:="VACCINATION", typeActivite:="VACCINATION")

        Assert.IsTrue(vaccination > consultation)
        Assert.AreEqual("EN_COURS", dao.GetEpisodeById(CInt(vaccination)).Etat)
        CloturerEpisode(consultation, idUtilisateur)
        Assert.ThrowsException(Of Exception)(Sub() CreerEpisode(idPatient, idUtilisateur))
    End Sub

    ' --- Modification ----------------------------------------------------------------

    <TestMethod()> Public Sub LaModificationEnregistreChaqueChampEtLAuteur()
        Dim idCreateur = CreerUtilisateur(avecCle:=False)
        Dim idModificateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idCreateur)
        Dim lu = dao.GetEpisodeById(CInt(idEpisode))
        lu.Type = "VIRTUEL"
        lu.TypeActivite = "PREVENTION_AUTRE"
        lu.TypeProfil = "PARAMEDICAL"
        lu.DescriptionActivite = "Description modifiee"
        lu.Commentaire = "Commentaire modifie"
        lu.ObservationMedical = "Observation medicale"
        lu.ObservationParamedical = "Observation paramedicale"
        lu.Decision = "Decision"
        lu.ConclusionIdeType = "ROLE_PROPRE"
        lu.ConclusionMedConsigneDrcId = 11
        lu.ConclusionMedConsigneDenomination = "Consigne"
        lu.ConclusionMedContexte1DrcId = 21
        lu.ConclusionMedContexte1AntecedentId = 22
        lu.ConclusionMedContexte2DrcId = 31
        lu.ConclusionMedContexte2AntecedentId = 32
        lu.ConclusionMedContexte3DrcId = 41
        lu.ConclusionMedContexte3AntecedentId = 42
        lu.Etat = "CLOTURE"
        lu.Inactif = True

        Assert.IsTrue(dao.ModificationEpisode(lu, Auteur(idModificateur)))

        Dim relu = dao.GetEpisodeById(CInt(idEpisode))
        Assert.AreEqual("VIRTUEL", relu.Type)
        Assert.AreEqual("PREVENTION_AUTRE", relu.TypeActivite)
        Assert.AreEqual("PARAMEDICAL", relu.TypeProfil)
        Assert.AreEqual("Description modifiee", relu.DescriptionActivite)
        Assert.AreEqual("Commentaire modifie", relu.Commentaire)
        Assert.AreEqual("Observation medicale", relu.ObservationMedical)
        Assert.AreEqual("Observation paramedicale", relu.ObservationParamedical)
        Assert.AreEqual("Decision", relu.Decision)
        Assert.AreEqual("ROLE_PROPRE", relu.ConclusionIdeType)
        Assert.AreEqual(11L, relu.ConclusionMedConsigneDrcId)
        Assert.AreEqual("Consigne", relu.ConclusionMedConsigneDenomination)
        Assert.AreEqual(21L, relu.ConclusionMedContexte1DrcId)
        Assert.AreEqual(22L, relu.ConclusionMedContexte1AntecedentId)
        Assert.AreEqual(31L, relu.ConclusionMedContexte2DrcId)
        Assert.AreEqual(32L, relu.ConclusionMedContexte2AntecedentId)
        Assert.AreEqual(41L, relu.ConclusionMedContexte3DrcId)
        Assert.AreEqual(42L, relu.ConclusionMedContexte3AntecedentId)
        Assert.AreEqual("CLOTURE", relu.Etat)
        Assert.IsTrue(relu.Inactif)
        Assert.AreEqual(idModificateur, relu.UserModification)
        Assert.AreEqual(Date.Today, relu.DateModification.Date)
        Assert.AreEqual(idCreateur, relu.UserCreation, "l'auteur de la création ne change pas")
    End Sub

    <TestMethod()> Public Sub ModifierUnEpisodeInexistantNeLevePasDErreur()
        ' Comportement actuel : aucune ligne touchée, et le DAO renvoie tout de même True.
        Dim fantome As New Episode With {
            .Id = EpisodeAbsent, .Type = "CONSULTATION", .TypeActivite = "", .TypeProfil = "", .DescriptionActivite = "",
            .Commentaire = "", .ObservationMedical = "", .ObservationParamedical = "", .Decision = "",
            .ConclusionIdeType = "", .ConclusionMedConsigneDenomination = "", .Etat = "EN_COURS"
        }

        Assert.IsTrue(dao.ModificationEpisode(fantome, Auteur(CreerUtilisateur(avecCle:=False))))
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_episode WHERE episode_id = @p0", EpisodeAbsent)))
    End Sub

    ' --- Conclusion médicale depuis les contextes ---------------------------------------

    <TestMethod()> Public Sub LaConclusionMedicaleReprendLesContextesDansLOrdreDeLiaison()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim diabete = CreerContextePourEpisode(idPatient, idUtilisateur, "Diabete")
        Dim hta = CreerContextePourEpisode(idPatient, idUtilisateur, "HTA")
        ' Lié en premier mais daté après : c'est la date de liaison qui ordonne.
        DaterContexteEpisode(LierContexteEpisode(idEpisode, idPatient, diabete, idUtilisateur), New Date(2026, 2, 2))
        DaterContexteEpisode(LierContexteEpisode(idEpisode, idPatient, hta, idUtilisateur), New Date(2026, 1, 1))

        Assert.IsTrue(dao.MajEpisodeConclusionMedicale(idEpisode))

        Assert.AreEqual("HTA; Diabete", dao.GetEpisodeById(CInt(idEpisode)).ObservationMedical)
    End Sub

    <TestMethod()> Public Sub SansContexteLaConclusionMedicaleResteInchangee()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idEpisode = CreerEpisode(CreerPatient(), idUtilisateur)
        Dim lu = dao.GetEpisodeById(CInt(idEpisode))
        lu.ObservationMedical = "Saisie manuelle"
        dao.ModificationEpisode(lu, Auteur(idUtilisateur))

        Assert.IsTrue(dao.MajEpisodeConclusionMedicale(idEpisode))

        Assert.AreEqual("Saisie manuelle", dao.GetEpisodeById(CInt(idEpisode)).ObservationMedical)
    End Sub

    ' --- Listes par patient ------------------------------------------------------------

    <TestMethod()> Public Sub LesEpisodesDUnPatientVontDuPlusRecentAuPlusAncien()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim premier = CreerEpisode(idPatient, idUtilisateur)
        CloturerEpisode(premier, idUtilisateur)
        Dim deuxieme = CreerEpisodeParametres(idPatient, idUtilisateur, New Date(2026, 1, 5, 8, 0, 0))
        Dim troisieme = CreerEpisode(idPatient, idUtilisateur)
        CreerEpisode(CreerPatient("AUTRE", "Patient"), idUtilisateur)
        ' Comportement actuel : cette liste ne filtre pas les épisodes annulés.
        DesactiverEpisode(troisieme, idUtilisateur)

        Dim liste = dao.GetAllEpisodeByPatient(CInt(idPatient))

        CollectionAssert.AreEqual(New Long() {troisieme, deuxieme, premier}, liste.Select(Function(e) e.Id).ToArray())
        Assert.IsTrue(liste(0).Inactif)
    End Sub

    <TestMethod()> Public Sub UnPatientSansEpisodeDonneUneListeVide()
        Assert.AreEqual(0, dao.GetAllEpisodeByPatient(CInt(CreerPatient())).Count)
    End Sub

    ' --- Épisode en cours d'un patient (noms en trois parties) --------------------------

    <TestMethod()> Public Sub LEpisodeEnCoursDUnPatientEstRetrouve()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        CreerEpisodeParametres(idPatient, idUtilisateur, Date.Now)
        Dim enCours = CreerEpisode(idPatient, idUtilisateur, typeEpisode:="VIRTUEL")

        Dim trouve = dao.GetEpisodeEnCoursByPatientId(idPatient)

        Assert.AreEqual(enCours, trouve.Id)
        Assert.AreEqual("VIRTUEL", trouve.Type)
        Assert.AreEqual("EN_COURS", trouve.Etat)
    End Sub

    <TestMethod()> Public Sub SansEpisodeEnCoursLeDaoRenvoieUnEpisodeVide()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        CloturerEpisode(CreerEpisode(idPatient, idUtilisateur), idUtilisateur)
        CreerEpisodeParametres(idPatient, idUtilisateur, Date.Now)

        Dim trouve = dao.GetEpisodeEnCoursByPatientId(idPatient)

        Assert.AreEqual(0L, trouve.Id)
        Assert.AreEqual(0L, trouve.PatientId)
        Assert.AreEqual("", trouve.Etat)
        Assert.AreEqual(Date.MinValue, trouve.DateCreation)
    End Sub

    <TestMethod()> Public Sub UnEpisodeEnCoursAnnuleNEstPasRetrouve()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        DesactiverEpisode(CreerEpisode(idPatient, idUtilisateur), idUtilisateur)

        Assert.AreEqual(0L, dao.GetEpisodeEnCoursByPatientId(idPatient).Id)
    End Sub

    ' --- Ligne de vie (noms en trois parties) --------------------------------------------

    Private Shared Function LigneDeVieConsultationMedicale() As LigneDeVie
        Return New LigneDeVie With {
            .TypeConsultation = True,
            .ProfilMedical = True,
            .ActivitePathologieAigue = True
        }
    End Function

    Private Function LigneDeVieDuPatient(idPatient As Long, filtre As LigneDeVie,
                                        Optional debut As Date? = Nothing, Optional fin As Date? = Nothing) As DataTable
        ' Les bornes partent dans le SQL par ToString("yyyy-MM-dd") : culture du poste.
        Dim cultureAvant = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
        Try
            Return dao.GetAllEpisodeByPatient(idPatient, If(debut, Date.Today), If(fin, Date.Today.AddDays(-365)),
                                              filtre, New List(Of Long))
        Finally
            Thread.CurrentThread.CurrentCulture = cultureAvant
        End Try
    End Function

    <TestMethod()> Public Sub LaLigneDeVieFiltreParTypeProfilActiviteEtPeriode()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim ancien = CreerEpisode(idPatient, idUtilisateur)
        CloturerEpisode(ancien, idUtilisateur)
        DaterEpisode(ancien, Date.Today.AddDays(-10))
        Dim horsPeriode = CreerEpisode(idPatient, idUtilisateur)
        CloturerEpisode(horsPeriode, idUtilisateur)
        DaterEpisode(horsPeriode, Date.Today.AddDays(-400))
        Dim autreActivite = CreerEpisode(idPatient, idUtilisateur, typeActivite:="SUIVI_CHRONIQUE")
        CloturerEpisode(autreActivite, idUtilisateur)
        Dim autreProfil = CreerEpisode(idPatient, idUtilisateur, typeProfil:="PARAMEDICAL")
        CloturerEpisode(autreProfil, idUtilisateur)
        Dim annule = CreerEpisode(idPatient, idUtilisateur)
        DesactiverEpisode(annule, idUtilisateur)
        CloturerEpisode(annule, idUtilisateur)
        Dim recent = CreerEpisode(idPatient, idUtilisateur)

        Dim table = LigneDeVieDuPatient(idPatient, LigneDeVieConsultationMedicale())

        CollectionAssert.AreEqual(New Long() {recent, ancien}, Ids(table))
        Assert.AreEqual(0, CInt(table.Rows(0)("nb_sous_episode")))
        Assert.IsTrue(IsDBNull(table.Rows(0)("oa_ordonnance_id")))
    End Sub

    <TestMethod()> Public Sub LaLigneDeVieAjouteLaValeurDesParametresDemandes()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim poids = CreerParametreDeMesure("Poids test", "kg")
        Dim taille = CreerParametreDeMesure("Taille test", "cm")
        CreerParametreEpisode(idEpisode, idPatient, poids, 72.5D)
        Dim filtre = LigneDeVieConsultationMedicale()
        filtre.ParametreId1 = poids
        filtre.ParametreId2 = taille

        Dim table = LigneDeVieDuPatient(idPatient, filtre)

        Assert.AreEqual(1, table.Rows.Count)
        Assert.AreEqual(72.5D, CDec(table.Rows(0)("ValeurParam1")))
        Assert.IsTrue(IsDBNull(table.Rows(0)("ValeurParam2")), "paramètre non saisi")
        Assert.IsFalse(table.Columns.Contains("ValeurParam3"))
    End Sub

    <TestMethod()> Public Sub LaLigneDeVieVirtuelleRetientLesEpisodesSansActivite()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim virtuel = CreerEpisode(idPatient, idUtilisateur, typeEpisode:="VIRTUEL", typeActivite:="")
        CloturerEpisode(virtuel, idUtilisateur)
        Dim consultation = CreerEpisode(idPatient, idUtilisateur)
        Dim filtre As New LigneDeVie With {.TypeVirtuel = True, .ProfilMedical = True}

        Dim table = LigneDeVieDuPatient(idPatient, filtre)

        CollectionAssert.AreEqual(New Long() {virtuel}, Ids(table))
        CollectionAssert.DoesNotContain(Ids(table), consultation)
    End Sub

    <TestMethod()> Public Sub LaLigneDeVieSignaleLOrdonnanceActiveDeLEpisode()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idEpisode = CreerEpisode(idPatient, idUtilisateur)
        Dim idOrdonnance = CreerOrdonnance(idPatient, idUtilisateur, nbLignes:=0, episodeId:=idEpisode, avecTraitements:=False)

        Dim table = LigneDeVieDuPatient(idPatient, LigneDeVieConsultationMedicale())

        Assert.AreEqual(1, table.Rows.Count)
        Assert.AreEqual(idOrdonnance, CLng(table.Rows(0)("oa_ordonnance_id")))
    End Sub

    ' --- Tableaux de bord (noms en trois parties) ---------------------------------------

    <TestMethod()> Public Sub LaListeDesEpisodesEnCoursDonneLePatientEtLAuteur()
        ExigerBaseNommeeOasis()
        ' La tâche jointe par OUTER APPLY est un SELECT * sur oa_tache, oa_r_fonction et
        ' oa_utilisateur : si le compte client se voit refuser cle_privee ou oa_password
        ' à travers ce *, l'écran des épisodes en cours échoue en production.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient("ENCOURS", "Liste")
        Dim enCours = CreerEpisode(idPatient, idUtilisateur)
        Dim idAutrePatient = CreerPatient("CLOS", "Liste")
        CloturerEpisode(CreerEpisode(idAutrePatient, idUtilisateur), idUtilisateur)
        CreerEpisodeParametres(idAutrePatient, idUtilisateur, Date.Now)

        Dim table = dao.GetAllEpisodeEnCours()

        CollectionAssert.AreEqual(New Long() {enCours}, Ids(table))
        Dim ligne = table.Rows(0)
        Assert.AreEqual(idPatient, CLng(ligne("patient_id")))
        Assert.AreEqual("ENCOURS", CStr(ligne("oa_patient_nom")))
        Assert.AreEqual("TEST", CStr(ligne("oa_utilisateur_nom")))
        Assert.IsTrue(IsDBNull(ligne("nature")), "aucune tâche")
    End Sub

    <TestMethod()> Public Sub LesConsultationsClotureesLeJourDemandeSontListees()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient("CLOTURE", "Jour")
        Dim cloture = CreerEpisode(idPatient, idUtilisateur)
        CloturerEpisode(cloture, idUtilisateur)
        Dim idVirtuel = CreerPatient("VIRTUEL", "Jour")
        CloturerEpisode(CreerEpisode(idVirtuel, idUtilisateur, typeEpisode:="VIRTUEL"), idUtilisateur)
        CreerEpisode(CreerPatient("OUVERT", "Jour"), idUtilisateur)

        Dim cultureAvant = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
        Try
            Dim duJour = dao.GetAllEpisodeClosedByDate(Date.Today)
            CollectionAssert.AreEqual(New Long() {cloture}, Ids(duJour))
            Assert.AreEqual("CLOTURE", CStr(duJour.Rows(0)("oa_patient_nom")))
            Assert.AreEqual(0, dao.GetAllEpisodeClosedByDate(Date.Today.AddDays(-5)).Rows.Count)
        Finally
            Thread.CurrentThread.CurrentCulture = cultureAvant
        End Try
    End Sub

    <TestMethod()> Public Sub SeulsLesEpisodesAvecUneOrdonnanceNonSigneeAttendentValidation()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur()
        Dim idPatient = CreerPatient("ATTENTE", "Ordonnance")
        Dim enAttente = CreerEpisode(idPatient, idUtilisateur)
        Dim idOrdonnance = CreerOrdonnance(idPatient, idUtilisateur, nbLignes:=0, episodeId:=enAttente, avecTraitements:=False)
        Dim idSigne = CreerPatient("SIGNEE", "Ordonnance")
        Dim signe = CreerEpisode(idSigne, idUtilisateur)
        CreerOrdonnanceSignee(idSigne, idUtilisateur, nbLignes:=1, episodeId:=signe)
        CreerEpisode(CreerPatient("SANS", "Ordonnance"), idUtilisateur)

        Dim table = dao.GetAllEpisodeEnAttenteValidation()

        CollectionAssert.AreEqual(New Long() {enAttente}, Ids(table))
        Assert.AreEqual(idOrdonnance, CLng(table.Rows(0)("oa_ordonnance_id")))
        Assert.AreEqual(0, CInt(table.Rows(0)("TotalSSP")))
        Assert.AreEqual(0, CInt(table.Rows(0)("totalSER")))
    End Sub

End Class
