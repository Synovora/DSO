Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Threading
Imports Oasis_Common

''' <summary>
''' PpsDao contre la base de test. Le client lourd crée, modifie, annule et lit les
''' PPS sous oasis_client ; la synthèse web (SyntheseController) lit aussi
''' getAllPPSbyPatient, éprouvée en plus sous oasis_web.
'''
''' L'historique (PPSHistoCreationDao) écrit la date de début sous forme de texte
''' selon la culture courante : le client lourd tourne en français, ces tests aussi.
'''
''' getAllPPSbyPatient et ExistPPSObjectifByPatientId nomment leurs tables en trois
''' parties (oasis.oasis.oa_...) : elles visent la base « oasis » quel que soit le
''' catalogue de la connexion. Sur une base de test nommée autrement, elles
''' échouent ; leurs tests fonctionnels ne tournent que sur une base nommée oasis.
''' </summary>
<TestClass()> Public Class PpsDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New PpsDao

    Private cultureInitiale As CultureInfo

    <TestInitialize>
    Public Sub PasserEnFrancais()
        cultureInitiale = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
    End Sub

    <TestCleanup>
    Public Sub RetablirLaCulture()
        If cultureInitiale IsNot Nothing Then Thread.CurrentThread.CurrentCulture = cultureInitiale
    End Sub

    Private Shared Function Auteur(idUtilisateur As Long) As Utilisateur
        Return New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}
    End Function

    Private Shared Function IdsTable(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("oa_pps_id"))).ToArray()
    End Function

    Private Shared Function NombrePps(idPatient As Long) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient_pps WHERE oa_pps_patient_id = @p0", idPatient))
    End Function

    Private Shared Sub ReculerDateSynthese(idPatient As Long)
        Executer("UPDATE oasis.oa_patient SET oa_patient_synthese_date_maj = @p0 WHERE oa_patient_id = @p1",
                 New Date(2000, 1, 1), idPatient)
    End Sub

    Private Shared Function DateSynthese(idPatient As Long) As Date
        Return CDate(Scalaire("SELECT oa_patient_synthese_date_maj FROM oasis.oa_patient WHERE oa_patient_id = @p0", idPatient))
    End Function

    Private Shared Function BaseNommeeOasis() As Boolean
        Return String.Equals(NomBase, "oasis", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Shared Sub ExigerBaseNommeeOasis()
        If Not BaseNommeeOasis() Then
            Assert.Inconclusive("Requête en oasis.oasis.* : ne tourne que sur une base de test nommée oasis (actuelle : " & NomBase & ").")
        End If
    End Sub

    ''' <summary>Même chose que CreationPPS, en rattrapant l'erreur.</summary>
    Private Function ErreurDeCreation(saisie As Pps, idUtilisateur As Long) As Exception
        Try
            dao.CreationPPS(saisie, Auteur(idUtilisateur))
        Catch ex As Exception
            Return ex
        End Try
        Return Nothing
    End Function

    ' --- Création et lecture -------------------------------------------------------

    <TestMethod()> Public Sub CreationPPS_EcritLeDossierEtRenvoieVrai()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idDrc = CreerDrc()
        ReculerDateSynthese(idPatient)
        Dim saisie As New Pps With {
            .PatientId = CInt(idPatient),
            .CategorieId = Pps.EnumCategoriePPS.STRATEGIE,
            .SousCategorieId = Pps.EnumSousCategoriePPS.Curative,
            .DrcId = CInt(idDrc),
            .Priorite = 3,
            .Commentaire = "Traitement de fond",
            .DateFin = New Date(2027, 6, 30)
        }

        Assert.IsTrue(dao.CreationPPS(saisie, Auteur(idUtilisateur)))

        Dim idPps = CLng(Scalaire("SELECT MAX(oa_pps_id) FROM oasis.oa_patient_pps WHERE oa_pps_patient_id = @p0", idPatient))
        Dim lu = dao.getPpsById(CInt(idPps))
        Assert.AreEqual(CInt(idPps), lu.Id)
        Assert.AreEqual(CInt(idPatient), lu.PatientId)
        Assert.AreEqual(4, lu.CategorieId)
        Assert.AreEqual(10, lu.SousCategorieId)
        Assert.AreEqual(CInt(idDrc), lu.DrcId)
        Assert.AreEqual(3, lu.Priorite)
        Assert.AreEqual("Traitement de fond", lu.Commentaire)
        Assert.IsTrue(lu.AffichageSynthese)
        Assert.AreEqual(New Date(2027, 6, 30), lu.DateFin.Value.Date)
        Assert.AreEqual(Date.MinValue, lu.DateDebut, "date de début non écrite")
        Assert.IsFalse(lu.Arret)
        Assert.AreEqual("", lu.ArretCommentaire)
        Assert.AreEqual(CInt(idUtilisateur), lu.UserCreation)
        Assert.AreEqual(Date.Today, lu.DateCreation.Date)
        Assert.AreEqual(0, lu.UserModification)
        Assert.AreEqual(Date.Today, DateSynthese(idPatient).Date)
    End Sub

    <TestMethod()> Public Sub CreationPPS_EcritUneLigneDHistoriqueDeCreation()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idDrc = CreerDrc()

        Dim idPps = CreerPps(idPatient, idUtilisateur, 2, 2, priorite:=5, drcId:=idDrc, commentaire:="Vaccination")

        Dim histo = HistoriquePps(idPps)
        Assert.AreEqual(1, histo.Rows.Count)
        Dim ligne = histo.Rows(0)
        Assert.AreEqual(1, CInt(ligne("oa_pps_histo_etat_historisation")), "Creation")
        Assert.AreEqual(idUtilisateur, CLng(ligne("oa_pps_histo_utilisateur_historisation")))
        Assert.AreEqual(Date.Today, CDate(ligne("oa_pps_histo_date_historisation")).Date)
        Assert.AreEqual(idPatient, CLng(ligne("oa_pps_patient_id")))
        Assert.AreEqual(2, CInt(ligne("oa_pps_categorie")))
        Assert.AreEqual(2, CInt(ligne("oa_pps_sous_categorie")))
        Assert.AreEqual(5, CInt(ligne("oa_pps_priorite")))
        Assert.AreEqual(idDrc, CLng(ligne("oa_pps_drc_id")))
        Assert.IsTrue(CBool(ligne("oa_pps_affichage_synthese")))
        Assert.AreEqual("Vaccination", CStr(ligne("oa_pps_commentaire")))
        ' Comportement actuel : la date de début absente est écrite en texte
        ' « 01/01/0001 00:00:00 » (colonne supposée date ou datetime2).
        Assert.AreEqual(New Date(1, 1, 1), CDate(ligne("oa_pps_date_debut")).Date)
        Assert.IsFalse(CBool(ligne("oa_pps_arret")))
        Assert.AreEqual("", CStr(ligne("oa_pps_commentaire_arret")))
        Assert.IsFalse(CBool(ligne("oa_pps_inactif")))
    End Sub

    <TestMethod()> Public Sub CreationPPS_SansDateDeFin_EchoueSansRienEcrire()
        ' Comportement actuel : un PPS sans date de fin (DateFin = Nothing) donne un
        ' paramètre @dateFin sans valeur, que SQL Server refuse.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim saisie As New Pps With {
            .PatientId = CInt(idPatient), .CategorieId = 1, .SousCategorieId = 1,
            .DrcId = CInt(CreerDrc()), .Priorite = 1, .Commentaire = "", .DateFin = Nothing
        }

        Assert.IsNotNull(ErreurDeCreation(saisie, idUtilisateur))
        Assert.AreEqual(0, NombrePps(idPatient))
    End Sub

    <TestMethod()> Public Sub CreationPPS_DateDeFinNonRenseigneeCommeLEcran_EchoueSansRienEcrire()
        ' Comportement actuel : RadFPPSDetailEdit.CreationPPS écrit
        ' If(DTPFin.Value = DTPFin.MaxDate, Nothing, DTPFin.Value). Le If est typé
        ' Date : Nothing y vaut 01/01/0001, hors de la plage d'un datetime SQL, et
        ' l'insertion échoue. Créer un PPS sans date de fin depuis l'écran échoue donc.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim saisie As New Pps With {
            .PatientId = CInt(idPatient), .CategorieId = 1, .SousCategorieId = 1,
            .DrcId = CInt(CreerDrc()), .Priorite = 1, .Commentaire = "", .DateFin = Date.MinValue
        }

        Assert.IsNotNull(ErreurDeCreation(saisie, idUtilisateur))
        Assert.AreEqual(0, NombrePps(idPatient))
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentException))>
    Public Sub GetPpsById_Inexistant_LeveUneErreur()
        dao.getPpsById(987654321)
    End Sub

    ' --- Modification et annulation ------------------------------------------------

    <TestMethod()> Public Sub ModificationPPS_EnregistreLesChampsEtLHistorique()
        Dim idCreateur = CreerUtilisateur(avecCle:=False)
        Dim idModificateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idPps = CreerPps(idPatient, idCreateur, 4, 7)
        Dim idNouvelleDrc = CreerDrc()
        Dim modifie = dao.getPpsById(CInt(idPps))
        modifie.SousCategorieId = 9
        modifie.Priorite = 2
        modifie.DrcId = CInt(idNouvelleDrc)
        modifie.Commentaire = "Revu"
        modifie.DateFin = Nothing
        ReculerDateSynthese(idPatient)

        Assert.IsTrue(dao.ModificationPPS(modifie, Auteur(idModificateur)))

        Dim relu = dao.getPpsById(CInt(idPps))
        Assert.AreEqual(4, relu.CategorieId, "la catégorie ne change pas")
        Assert.AreEqual(9, relu.SousCategorieId)
        Assert.AreEqual(2, relu.Priorite)
        Assert.AreEqual(CInt(idNouvelleDrc), relu.DrcId)
        Assert.AreEqual("Revu", relu.Commentaire)
        Assert.IsFalse(relu.DateFin.HasValue AndAlso relu.DateFin.Value <> Date.MinValue, "date de fin retirée")
        Assert.IsTrue(IsDBNull(Scalaire("SELECT oa_pps_date_fin FROM oasis.oa_patient_pps WHERE oa_pps_id = @p0", idPps)))
        Assert.AreEqual(CInt(idModificateur), relu.UserModification)
        Assert.AreEqual(Date.Today, relu.DateModification.Date)
        Assert.AreEqual(CInt(idCreateur), relu.UserCreation)
        Assert.AreEqual(Date.Today, DateSynthese(idPatient).Date)

        Dim histo = HistoriquePps(idPps)
        Assert.AreEqual(2, histo.Rows.Count)
        Dim ligne = histo.Rows(1)
        Assert.AreEqual(2, CInt(ligne("oa_pps_histo_etat_historisation")), "Modification")
        Assert.AreEqual(idModificateur, CLng(ligne("oa_pps_histo_utilisateur_historisation")))
        Assert.AreEqual(4, CInt(ligne("oa_pps_categorie")))
        Assert.AreEqual(9, CInt(ligne("oa_pps_sous_categorie")))
        Assert.AreEqual(2, CInt(ligne("oa_pps_priorite")))
        Assert.AreEqual(idNouvelleDrc, CLng(ligne("oa_pps_drc_id")))
        Assert.AreEqual("Revu", CStr(ligne("oa_pps_commentaire")))
        Assert.IsTrue(CBool(ligne("oa_pps_affichage_synthese")))
        Assert.IsFalse(CBool(ligne("oa_pps_inactif")))
    End Sub

    <TestMethod()> Public Sub ModificationPPS_AvecDateDeFin_LEnregistre()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPps = CreerPps(CreerPatient(), idUtilisateur, 1, 1)
        Dim modifie = dao.getPpsById(CInt(idPps))
        modifie.DateFin = New Date(2028, 2, 29)

        dao.ModificationPPS(modifie, Auteur(idUtilisateur))

        Assert.AreEqual(New Date(2028, 2, 29), dao.getPpsById(CInt(idPps)).DateFin.Value.Date)
    End Sub

    <TestMethod()> Public Sub AnnulationPrevention_DesactiveEtArreteSansSupprimer()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idPps = CreerPps(idPatient, idUtilisateur, 2, 2)
        Dim lu = dao.getPpsById(CInt(idPps))
        lu.ArretCommentaire = "Plus indiqué"
        ReculerDateSynthese(idPatient)

        Assert.IsTrue(dao.AnnulationPrevention(lu, Auteur(idUtilisateur)))

        Assert.AreEqual(1, NombrePps(idPatient), "un PPS annulé reste en base")
        Dim relu = dao.getPpsById(CInt(idPps))
        Assert.IsTrue(relu.Arret)
        Assert.AreEqual("Plus indiqué", relu.ArretCommentaire)
        Assert.AreEqual(CInt(idUtilisateur), relu.UserModification)
        Assert.IsTrue(CBool(Scalaire("SELECT oa_pps_inactif FROM oasis.oa_patient_pps WHERE oa_pps_id = @p0", idPps)))
        Assert.AreEqual(Date.Today, DateSynthese(idPatient).Date)
        Assert.AreEqual(0, dao.getAllPPSPreventionbyPatient(CInt(idPatient)).Rows.Count)

        Dim ligne = HistoriquePps(idPps).Rows(1)
        Assert.AreEqual(4, CInt(ligne("oa_pps_histo_etat_historisation")), "Annulation")
        Assert.IsTrue(CBool(ligne("oa_pps_arret")))
        Assert.AreEqual("Plus indiqué", CStr(ligne("oa_pps_commentaire_arret")))
        Assert.IsTrue(CBool(ligne("oa_pps_inactif")))
        ' Comportement actuel : l'historique d'annulation laisse AffichageSynthese à Faux.
        Assert.IsFalse(CBool(ligne("oa_pps_affichage_synthese")))
    End Sub

    ' --- Objectif de santé ---------------------------------------------------------

    <TestMethod()> Public Sub GetPpsObjectifByPatientId_RenvoieLObjectifMemeAnnule()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        CreerPps(idPatient, idUtilisateur, 2, 2)
        Dim idObjectif = CreerPps(idPatient, idUtilisateur, 1, 1, commentaire:="Perdre du poids")
        CreerPps(CreerPatient("AUTRE", "Patient"), idUtilisateur, 1, 1)

        Dim lu = dao.getPpsObjectifByPatientId(CInt(idPatient))
        Assert.AreEqual(CInt(idObjectif), lu.Id)
        Assert.AreEqual("Perdre du poids", lu.Commentaire)

        ' Comportement actuel : aucun filtre sur oa_pps_inactif.
        dao.AnnulationPrevention(lu, Auteur(idUtilisateur))
        Assert.AreEqual(CInt(idObjectif), dao.getPpsObjectifByPatientId(CInt(idPatient)).Id)
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentException))>
    Public Sub GetPpsObjectifByPatientId_SansObjectif_LeveUneErreur()
        Dim idPatient = CreerPatient()
        CreerPps(idPatient, CreerUtilisateur(avecCle:=False), 2, 2)
        dao.getPpsObjectifByPatientId(CInt(idPatient))
    End Sub

    <TestMethod()> Public Sub ExistPPSObjectifByPatientId_VraiSeulementPourUnObjectifActif()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idSansObjectif = CreerPatient("SANS", "Objectif")
        CreerPps(idSansObjectif, idUtilisateur, 2, 2)
        Dim idObjectif = CreerPps(idPatient, idUtilisateur, 1, 1)

        Assert.IsTrue(dao.ExistPPSObjectifByPatientId(CInt(idPatient)))
        Assert.IsFalse(dao.ExistPPSObjectifByPatientId(CInt(idSansObjectif)))

        dao.AnnulationPrevention(dao.getPpsById(CInt(idObjectif)), Auteur(idUtilisateur))
        Assert.IsFalse(dao.ExistPPSObjectifByPatientId(CInt(idPatient)))
    End Sub

    <TestMethod()> Public Sub ExistPPSObjectifByPatientId_HorsBaseNommeeOasis_Echoue()
        ' Comportement actuel : oasis.oasis.oa_patient_pps impose le nom de base
        ' « oasis ». Le client lourd appelle cette méthode à l'ouverture de la
        ' synthèse et de l'épisode.
        If BaseNommeeOasis() Then Assert.Inconclusive("Base de test nommée oasis : le nom en trois parties résout.")
        Dim idPatient = CreerPatient()
        Try
            dao.ExistPPSObjectifByPatientId(CInt(idPatient))
            Assert.Fail("La requête aurait dû viser une base oasis absente.")
        Catch ex As SqlException
            StringAssert.Contains(ex.Message, "oasis")
        End Try
    End Sub

    ' --- Listes par catégorie ------------------------------------------------------

    <TestMethod()> Public Sub GetAllPPSStrategiePatient_StrategiesActivesParPriorite()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim seconde = CreerPps(idPatient, idUtilisateur, 4, 10, priorite:=2)
        Dim premiere = CreerPps(idPatient, idUtilisateur, 4, 7, priorite:=1)
        Dim annulee = CreerPps(idPatient, idUtilisateur, 4, 8, priorite:=0)
        dao.AnnulationPrevention(dao.getPpsById(CInt(annulee)), Auteur(idUtilisateur))
        CreerPps(idPatient, idUtilisateur, 2, 2)
        CreerPps(CreerPatient("AUTRE", "Patient"), idUtilisateur, 4, 7)

        CollectionAssert.AreEqual(New Long() {premiere, seconde}, IdsTable(dao.getAllPPSStrategiePatient(CInt(idPatient))))
    End Sub

    <TestMethod()> Public Sub GetAllPPSSuivibyPatient_MesuresHorsPreventionAfficheesEnSynthese()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim sousCategorie5 = CreerPps(idPatient, idUtilisateur, 2, 5)
        Dim sousCategorie3 = CreerPps(idPatient, idUtilisateur, 2, 3)
        Dim masquee = CreerPps(idPatient, idUtilisateur, 2, 4)
        MasquerPpsDeLaSynthese(masquee)
        Dim annulee = CreerPps(idPatient, idUtilisateur, 2, 6)
        dao.AnnulationPrevention(dao.getPpsById(CInt(annulee)), Auteur(idUtilisateur))
        CreerPps(idPatient, idUtilisateur, 2, 2)
        CreerPps(idPatient, idUtilisateur, 3, 3)

        CollectionAssert.AreEqual(New Long() {sousCategorie3, sousCategorie5}, IdsTable(dao.getAllPPSSuivibyPatient(CInt(idPatient))))
    End Sub

    <TestMethod()> Public Sub GetAllPPSPreventionbyPatient_PreventionsActivesParPriorite()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim seconde = CreerPps(idPatient, idUtilisateur, 2, 2, priorite:=9)
        Dim premiere = CreerPps(idPatient, idUtilisateur, 2, 2, priorite:=1)
        CreerPps(idPatient, idUtilisateur, 2, 3)
        CreerPps(CreerPatient("AUTRE", "Patient"), idUtilisateur, 2, 2)

        CollectionAssert.AreEqual(New Long() {premiere, seconde}, IdsTable(dao.getAllPPSPreventionbyPatient(CInt(idPatient))))
    End Sub

    <TestMethod()> Public Sub ListesParCategorie_PatientSansPps_TablesVides()
        Dim idPatient = CInt(CreerPatient())
        Assert.AreEqual(0, dao.getAllPPSStrategiePatient(idPatient).Rows.Count)
        Assert.AreEqual(0, dao.getAllPPSSuivibyPatient(idPatient).Rows.Count)
        Assert.AreEqual(0, dao.getAllPPSPreventionbyPatient(idPatient).Rows.Count)
    End Sub

    ' --- getAllPPSbyPatient (synthèse) ---------------------------------------------

    ''' <summary>
    ''' Trois PPS actifs du patient dans trois sous-catégories d'ordres 10, 20 et 30,
    ''' un annulé, un d'un autre patient. Renvoie {objectif, prevention, strategie}.
    ''' </summary>
    Private Function PreparerSynthesePps(idPatient As Long, idUtilisateur As Long) As Long()
        AssurerSousCategoriePps(1, 1, 10)
        AssurerSousCategoriePps(2, 2, 20)
        AssurerSousCategoriePps(4, 7, 30, "Prophylactique")
        Dim strategie = CreerPps(idPatient, idUtilisateur, 4, 7, dateFin:=New Date(2020, 6, 1))
        Dim prevention = CreerPps(idPatient, idUtilisateur, 2, 2, dateFin:=New Date(2020, 3, 1))
        Dim objectif = CreerPps(idPatient, idUtilisateur, 1, 1)
        RetirerDateFinPps(objectif)
        Dim annulee = CreerPps(idPatient, idUtilisateur, 2, 2)
        dao.AnnulationPrevention(dao.getPpsById(CInt(annulee)), Auteur(idUtilisateur))
        CreerPps(CreerPatient("AUTRE", "Patient"), idUtilisateur, 1, 1)
        Return {objectif, prevention, strategie}
    End Function

    <TestMethod()> Public Sub GetAllPPSbyPatient_SuitLOrdreDesSousCategories()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim p = PreparerSynthesePps(idPatient, idUtilisateur)

        Dim table = dao.getAllPPSbyPatient(CInt(idPatient))

        CollectionAssert.AreEqual(New Long() {p(0), p(1), p(2)}, IdsTable(table))
        Assert.AreEqual("Prophylactique", CStr(table.Rows(2)("oa_r_pps_sous_categorie_type")))
        Assert.AreEqual(7, CInt(table.Rows(2)("oa_r_pps_sous_categorie_id")))
        Assert.IsTrue(IsDBNull(table.Rows(0)("oa_parcours_id")), "aucun parcours")
    End Sub

    <TestMethod()> Public Sub GetAllPPSbyPatient_ActifsAu_GardeSansFinEtFinFuture()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim p = PreparerSynthesePps(idPatient, idUtilisateur)

        CollectionAssert.AreEqual(New Long() {p(0), p(2)},
                                  IdsTable(dao.getAllPPSbyPatient(CInt(idPatient), actifsAu:=New Date(2020, 4, 1))))
    End Sub

    <TestMethod()> Public Sub GetAllPPSbyPatient_FinEntre_GardeLesFinsDansLIntervalle()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim p = PreparerSynthesePps(idPatient, idUtilisateur)

        CollectionAssert.AreEqual(New Long() {p(1)},
                                  IdsTable(dao.getAllPPSbyPatient(CInt(idPatient), finEntre:=New Date(2020, 1, 1), finEt:=New Date(2020, 3, 1))))
        ' Une seule borne : le filtre est ignoré.
        Assert.AreEqual(3, dao.getAllPPSbyPatient(CInt(idPatient), finEntre:=New Date(2020, 1, 1)).Rows.Count)
    End Sub

    <TestMethod()> Public Sub GetAllPPSbyPatient_LuParLeServeur()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim p = PreparerSynthesePps(idPatient, idUtilisateur)

        UtiliserCompte(Compte.Web)
        CollectionAssert.AreEqual(New Long() {p(0), p(1), p(2)}, IdsTable(dao.getAllPPSbyPatient(CInt(idPatient))))
    End Sub

    Private Shared Function ParcoursIde(idPatient As Long) As Long
        Return CreerParcoursPatient(idPatient, RorIdeOasis, specialiteId:=SpecialiteIdeOasis,
                                    sousCategorieId:=SousCategorieParcoursIde, categorieId:=CategorieParcoursSuivi)
    End Function

    <TestMethod()> Public Sub GetAllPPSbyPatient_UnParcoursDuPatientNeRameneQueSesPropresPps()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        AssurerSousCategoriePps(CategorieParcoursSuivi, SousCategorieParcoursIde, 1)
        Dim idAlice = CreerPatient("PPS", "Alice")
        Dim idBruno = CreerPatient("PPS", "Bruno")
        Dim parcoursAlice = ParcoursIde(idAlice)
        Dim ppsBruno = CreerPps(idBruno, idUtilisateur, CategorieParcoursSuivi, SousCategorieParcoursIde)
        Dim parcoursBruno = ParcoursIde(idBruno)

        Dim alice = dao.getAllPPSbyPatient(CInt(idAlice))
        Assert.AreEqual(1, alice.Rows.Count)
        Assert.IsTrue(IsDBNull(alice.Rows(0)("oa_pps_id")), "aucun PPS d'Alice")
        Assert.AreEqual(parcoursAlice, CLng(alice.Rows(0)("oa_parcours_id")))

        Dim bruno = dao.getAllPPSbyPatient(CInt(idBruno))
        Assert.AreEqual(1, bruno.Rows.Count)
        Assert.AreEqual(ppsBruno, CLng(bruno.Rows(0)("oa_pps_id")))
        Assert.AreEqual(parcoursBruno, CLng(bruno.Rows(0)("oa_parcours_id")))
    End Sub

    <TestMethod()> Public Sub GetAllPPSbyPatient_UnPpsDuPatientNeSeJointQuASesPropresParcours()
        ExigerBaseNommeeOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        AssurerSousCategoriePps(CategorieParcoursSuivi, SousCategorieParcoursIde, 1)
        Dim idAlice = CreerPatient("PPS", "Alice")
        Dim ppsAlice = CreerPps(idAlice, idUtilisateur, CategorieParcoursSuivi, SousCategorieParcoursIde)
        ParcoursIde(CreerPatient("PPS", "Bruno"))

        Dim sansParcours = dao.getAllPPSbyPatient(CInt(idAlice))
        Assert.AreEqual(1, sansParcours.Rows.Count)
        Assert.AreEqual(ppsAlice, CLng(sansParcours.Rows(0)("oa_pps_id")))
        Assert.IsTrue(IsDBNull(sansParcours.Rows(0)("oa_parcours_id")), "le parcours de Bruno ne se joint pas")

        Dim parcoursAlice = ParcoursIde(idAlice)
        Dim avecParcours = dao.getAllPPSbyPatient(CInt(idAlice))
        Assert.AreEqual(1, avecParcours.Rows.Count)
        Assert.AreEqual(ppsAlice, CLng(avecParcours.Rows(0)("oa_pps_id")))
        Assert.AreEqual(parcoursAlice, CLng(avecParcours.Rows(0)("oa_parcours_id")))
    End Sub

    <TestMethod()> Public Sub GetAllPPSbyPatient_HorsBaseNommeeOasis_Echoue()
        ' Comportement actuel : oasis.oasis.* impose le nom de base « oasis », pour
        ' la synthèse du client lourd comme pour celle du portail.
        If BaseNommeeOasis() Then Assert.Inconclusive("Base de test nommée oasis : le nom en trois parties résout.")
        Dim idPatient = CreerPatient()
        UtiliserCompte(Compte.Web)
        Try
            dao.getAllPPSbyPatient(CInt(idPatient))
            Assert.Fail("La requête aurait dû viser une base oasis absente.")
        Catch ex As SqlException
            StringAssert.Contains(ex.Message, "oasis")
        End Try
    End Sub

    ' --- Compare et DeterminationTypeStrategie -------------------------------------

    <TestMethod()> Public Sub Compare_ReconnaitUneCopieEtUnChangement()
        Dim lu = dao.getPpsById(CInt(CreerPps(CreerPatient(), CreerUtilisateur(avecCle:=False), 4, 7)))
        Dim copie = lu.Clone()

        Assert.IsTrue(dao.Compare(copie, lu))
        copie.Priorite += 1
        Assert.IsFalse(dao.Compare(copie, lu))
        copie = lu.Clone()
        copie.DateFin = New Date(2030, 1, 1)
        Assert.IsFalse(dao.Compare(copie, lu))
    End Sub

    <TestMethod()> Public Sub Compare_IgnoreUneDateDeFinAjouteeOuRetiree()
        ' Comportement actuel : DateFin est un Date? ; comparé à Nothing, <> donne
        ' Nothing, que If lit comme faux. Ajouter ou retirer la date de fin d'un PPS
        ' ne compte donc pas comme une modification dans RadFPPSDetailEdit.
        Dim avant As New Pps With {.Commentaire = "", .ArretCommentaire = "", .DateFin = Nothing}
        Dim apres As New Pps With {.Commentaire = "", .ArretCommentaire = "", .DateFin = New Date(2030, 1, 1)}

        Assert.IsTrue(dao.Compare(apres, avant))
        Assert.IsTrue(dao.Compare(avant, apres))
    End Sub

    <TestMethod()> Public Sub DeterminationTypeStrategie_TraduitLeLibelle()
        Assert.AreEqual(7, dao.DeterminationTypeStrategie("Prophylactique"))
        Assert.AreEqual(8, dao.DeterminationTypeStrategie("Sociale"))
        Assert.AreEqual(9, dao.DeterminationTypeStrategie("Symptomatique"))
        Assert.AreEqual(10, dao.DeterminationTypeStrategie("Curative"))
        Assert.AreEqual(11, dao.DeterminationTypeStrategie("Diagnostique"))
        Assert.AreEqual(12, dao.DeterminationTypeStrategie("Palliative"))
        Assert.AreEqual(0, dao.DeterminationTypeStrategie("curative"), "sensible à la casse")
        Assert.AreEqual(0, dao.DeterminationTypeStrategie(""))
    End Sub

End Class
