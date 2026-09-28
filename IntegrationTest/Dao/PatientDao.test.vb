Imports System.Globalization
Imports System.Threading
Imports Oasis_Common

''' <summary>
''' PatientDao contre la base, sous le compte qui exécute chaque méthode en production.
'''
''' Le client lourd crée, modifie et fait sortir les patients, vérifie l'unicité du NIR,
''' cherche par nom, date de naissance, site et DRC : ces appels tournent sous
''' Compte.Client. Le portail (Oasis_Web) lit la fiche (PortailController,
''' SignController) et les chaînes d'allergies et de contre-indications
''' (SyntheseController) : ces lectures sont rejouées sous Compte.Web.
''' GetPatientByNIR n'a plus d'appelant actif (l'appel du portail, dans
''' AuthController, est commenté) ; il est éprouvé sous les deux comptes.
''' </summary>
<TestClass()> Public Class PatientDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New PatientDao

    Private Const PatientAbsent As Integer = 987654321

    Private Shared ReadOnly EntreeOasis As New Date(2020, 1, 1)

    Private Shared Function ValeurPatient(colonne As String, id As Long) As Object
        Return Scalaire("SELECT " & colonne & " FROM oasis.oa_patient WHERE oa_patient_id = @p0", id)
    End Function

    Private Shared Function LibelleGenre(code As String) As String
        Return CStr(Scalaire("SELECT oa_r_genre_description FROM oasis.oa_r_genre WHERE oa_r_genre_code = @p0", code))
    End Function

    Private Shared Function IdsDe(table As DataTable) As List(Of Long)
        Dim ids As New List(Of Long)
        For Each ligne As DataRow In table.Rows
            ids.Add(CLng(ligne("oa_patient_id")))
        Next
        Return ids
    End Function

    Private Shared Function IdsDe(liste As List(Of Patient)) As List(Of Long)
        Return liste.Select(Function(p) CLng(p.PatientId)).ToList()
    End Function

    Private Shared Function Auteur(idUtilisateur As Long, Optional typeProfil As String = "MEDICAL") As Utilisateur
        Return New Utilisateur With {.UtilisateurId = CInt(idUtilisateur), .TypeProfil = typeProfil}
    End Function

    ''' <summary>Exécute l'action sous la culture du poste (fr-FR), puis rétablit la culture du test.</summary>
    Private Shared Sub SousCultureFrancaise(action As System.Action)
        Dim cultureInitiale = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
        Try
            action()
        Finally
            Thread.CurrentThread.CurrentCulture = cultureInitiale
        End Try
    End Sub

    Private Function Filtrer(tous As Boolean, dispositif As Boolean,
                             Optional prenom As String = Nothing,
                             Optional nom As String = Nothing,
                             Optional dateNaissance As Date? = Nothing,
                             Optional sites As List(Of Long) = Nothing) As List(Of Long)
        Return IdsDe(dao.GetAllPatientWithFilter(tous, dispositif, prenom, nom, dateNaissance, sites))
    End Function

    ' ---------------------------------------------------------------------
    ' CreationPatient et GetPatient (client)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub CreationPatient_ToutesLesZonesSaisiesSontRelues()
        Dim fiche = PatientDeTest("DURAND", "Amina", ins:=1234567890123L, dateNaissance:=New Date(1985, 6, 30), siteId:=9101)
        fiche.PatientNomMarital = "MARTIN"
        fiche.PatientGenreId = Patient.EnumGenreId.Masculin
        fiche.PatientAdresse2 = "Batiment B"
        fiche.PatientTel1 = "0269000001"
        fiche.PatientTel2 = "0639000002"
        fiche.PatientEmail = "amina.durand@exemple.fr"
        fiche.PatientUniteSanitaireId = 9201
        fiche.PatientInternet = True
        fiche.Profession = "Enseignante"

        Dim id = EnregistrerPatient(fiche, siegeId:=7)

        Dim relu = dao.GetPatient(CInt(id))
        Assert.AreEqual(id, CLng(relu.PatientId))
        Assert.AreEqual(fiche.PatientNir, relu.PatientNir)
        Assert.AreEqual(1234567890123L, relu.INS)
        Assert.AreEqual("DURAND", relu.PatientNom)
        Assert.AreEqual("Amina", relu.PatientPrenom)
        Assert.AreEqual("MARTIN", relu.PatientNomMarital)
        Assert.AreEqual(New Date(1985, 6, 30), relu.PatientDateNaissance)
        Assert.AreEqual("M", relu.PatientGenreId.Trim())
        Assert.AreEqual("1 rue du Test", relu.PatientAdresse1)
        Assert.AreEqual("Batiment B", relu.PatientAdresse2)
        Assert.AreEqual("97600", relu.PatientCodePostal)
        Assert.AreEqual("Mamoudzou", relu.PatientVille)
        Assert.AreEqual("0269000001", relu.PatientTel1)
        Assert.AreEqual("0639000002", relu.PatientTel2)
        Assert.AreEqual("amina.durand@exemple.fr", relu.PatientEmail)
        Assert.AreEqual(DateNonRenseignee, relu.PatientDateEntree)
        Assert.AreEqual(DateNonRenseignee, relu.PatientDateSortie)
        Assert.AreEqual(DateNonRenseignee, relu.PatientDateDeces)
        Assert.AreEqual("", relu.PatientCommentaireSortie)
        Assert.AreEqual(9101, relu.PatientSiteId)
        Assert.AreEqual(9201, relu.PatientUniteSanitaireId)
        Assert.AreEqual(7, relu.PatientSiegeId, "le siège vient de l'utilisateur connecté, pas de la fiche")
        Assert.IsTrue(relu.PatientInternet)
        Assert.AreEqual("Enseignante", relu.Profession)
        Assert.AreEqual(0L, relu.PharmacienId)
        Assert.IsFalse(relu.BlocageMedical)
        ' Champs dérivés : libellé du référentiel (21-reference-patient.sql) et âge.
        Assert.AreEqual(LibelleGenre("M"), relu.PatientGenre)
        Assert.AreEqual(CalculAgeEnAnnee(New Date(1985, 6, 30)), relu.PatientAgeEnAnnee)
        Assert.AreNotEqual("", relu.PatientAge)
    End Sub

    <TestMethod()> Public Sub CreationPatient_CleDuNirToujoursEcriteAZero()
        ' Comportement actuel : oa_patient_nir_modulo reçoit 0 quel que soit le NIR.
        Dim id = EnregistrerPatient(PatientDeTest())
        Assert.AreEqual(0, CInt(ValeurPatient("oa_patient_nir_modulo", id)))
    End Sub

    <TestMethod()> Public Sub CreationPatient_SousCultureFrancaise_DatesInchangees()
        Dim id As Long
        SousCultureFrancaise(
            Sub()
                id = EnregistrerPatient(PatientDeTest(dateNaissance:=New Date(1999, 12, 31)))
            End Sub)

        Assert.AreEqual(New Date(1999, 12, 31), CDate(ValeurPatient("oa_patient_date_naissance", id)).Date)
        Assert.AreEqual(DateNonRenseignee, CDate(ValeurPatient("oa_patient_date_entree_oasis", id)).Date)
    End Sub

    <TestMethod()> Public Sub CreationPatient_SansDateEntree_NeCreePasDeParcours()
        Dim id = EnregistrerPatient(PatientDeTest())
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient_parcours WHERE oa_parcours_patient_id = @p0", id)))
    End Sub

    <TestMethod()> Public Sub CreationPatient_NirDejaPris_EstAcceptee()
        ' Comportement actuel : ni le DAO ni la base ne refusent un NIR déjà attribué.
        ' Le seul garde-fou est l'appel à NonExistencePatientNIR par la fiche patient
        ' (RadFPatientDetailEdit) avant l'enregistrement.
        Dim premier = EnregistrerPatient(PatientDeTest("PREMIER"))
        Dim nir = CLng(ValeurPatient("oa_patient_nir", premier))

        Dim second = EnregistrerPatient(PatientDeTest("SECOND", nir:=nir))

        Assert.AreNotEqual(premier, second)
        Assert.AreEqual(2, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient WHERE oa_patient_nir = @p0", nir)))
    End Sub

    <TestMethod()> Public Sub CreationPatient_InsDejaPris_EstAcceptee()
        ' Comportement actuel : aucun contrôle d'unicité de l'INS, ni dans le DAO, ni
        ' dans la fiche patient.
        EnregistrerPatient(PatientDeTest("PREMIER", ins:=2750912345678L))
        EnregistrerPatient(PatientDeTest("SECOND", ins:=2750912345678L))

        Assert.AreEqual(2, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient WHERE oa_patient_INS = @p0", 2750912345678L)))
    End Sub

    <TestMethod()> Public Sub GetPatient_Inexistant_LeveArgumentException()
        Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetPatient(PatientAbsent))
    End Sub

    <TestMethod()> Public Sub GetPatient_IdZero_RenvoieUnPatientVideEtLaisseUneConnexionOuverte()
        Dim relu = dao.GetPatient(0)

        Assert.AreEqual(0, relu.PatientId)
        Assert.AreEqual("", relu.PatientNom)
        ' Comportement actuel : GetConnection est appelé avant le test sur l'id et la
        ' connexion n'est fermée que dans le Try, que l'id 0 saute. Elle reste ouverte
        ' jusqu'au passage du ramasse-miettes.
        Dim ouvertes = CInt(Scalaire("SELECT COUNT(*) FROM sys.dm_exec_sessions WHERE login_name = 'oasis_client' AND database_id = DB_ID()"))
        Assert.IsTrue(ouvertes >= 1, "la connexion ouverte par GetPatient(0) devrait être encore là")
    End Sub

    <TestMethod()> Public Sub GetPatient_SousWeb_LitLaFicheCommeLePortail()
        Dim fiche = PatientDeTest("PORTAIL", "Nadia", ins:=1112223334445L)
        Dim id = EnregistrerPatient(fiche, siegeId:=3)
        UtiliserCompte(Compte.Web)

        Dim relu = dao.GetPatient(CInt(id))

        Assert.AreEqual("PORTAIL", relu.PatientNom)
        Assert.AreEqual("Nadia", relu.PatientPrenom)
        Assert.AreEqual(fiche.PatientNir, relu.PatientNir)
        Assert.AreEqual(1112223334445L, relu.INS)
        Assert.AreEqual(3, relu.PatientSiegeId)
        Assert.AreEqual(LibelleGenre("F"), relu.PatientGenre)
    End Sub

    <TestMethod()> Public Sub GetPatient_DatesNulles_DonnentLaDateParDefaut()
        Dim id = EnregistrerPatient(PatientDeTest())
        PoserDatesOasisPatient(id, Nothing, Nothing)
        PoserDateMajSynthese(id, Nothing)

        Dim relu = dao.GetPatient(CInt(id))

        Assert.AreEqual(Date.MinValue, relu.PatientDateEntree)
        Assert.AreEqual(Date.MinValue, relu.PatientDateSortie)
        Assert.AreEqual(Date.MinValue, relu.PatientSyntheseDateMaj)
    End Sub

    <TestMethod()> Public Sub CompareEtClonePatient_SurUneFicheRelue()
        Dim id = EnregistrerPatient(PatientDeTest("COMPARE"), siegeId:=5)
        Dim lue = dao.GetPatient(CInt(id))
        Dim relue = dao.GetPatient(CInt(id))

        Assert.IsTrue(dao.Compare(lue, relue))

        Dim copie = dao.ClonePatient(lue)
        Assert.IsTrue(dao.Compare(lue, copie))
        ' Comportement actuel : le clone perd le siège, que Compare ne regarde pas.
        Assert.AreEqual(0, copie.PatientSiegeId)

        copie.PatientVille = "Dzaoudzi"
        Assert.IsFalse(dao.Compare(lue, copie))
    End Sub

    ' ---------------------------------------------------------------------
    ' NIR : lecture et contrôle d'unicité
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetPatientByNIR_NirConnu_RenvoieLePatient()
        Dim fiche = PatientDeTest("NIR", "Connu")
        Dim id = EnregistrerPatient(fiche)
        EnregistrerPatient(PatientDeTest("AUTRE"))

        Dim relu = dao.GetPatientByNIR(fiche.PatientNir.ToString())

        Assert.AreEqual(id, CLng(relu.PatientId))
        Assert.AreEqual("NIR", relu.PatientNom)
    End Sub

    <TestMethod()> Public Sub GetPatientByNIR_SousWeb_RenvoieLePatient()
        Dim fiche = PatientDeTest("NIR", "Web")
        Dim id = EnregistrerPatient(fiche)
        UtiliserCompte(Compte.Web)

        Assert.AreEqual(id, CLng(dao.GetPatientByNIR(fiche.PatientNir.ToString()).PatientId))
    End Sub

    <TestMethod()> Public Sub GetPatientByNIR_NirInconnu_LeveArgumentException()
        EnregistrerPatient(PatientDeTest())
        Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetPatientByNIR("1999999999999"))
    End Sub

    <TestMethod()> Public Sub GetPatientByNIR_Zero_RenvoieUnPatientVideSansChercher()
        ' Comportement actuel : le NIR (une chaîne) sert de condition « If NIR Then » ;
        ' "0" vaut Faux et rien n'est cherché, même si un patient porte ce NIR.
        EnregistrerPatient(PatientDeTest("SANS", "Nir", nir:=0))

        Dim relu = dao.GetPatientByNIR("0")

        Assert.AreEqual(0, relu.PatientId)
        Assert.AreEqual("", relu.PatientNom)
    End Sub

    <TestMethod()> Public Sub GetPatientByNIR_ChaineVide_LeveInvalidCastException()
        ' Comportement actuel : "" ne se convertit pas en booléen.
        Assert.ThrowsException(Of InvalidCastException)(Sub() dao.GetPatientByNIR(""))
    End Sub

    <TestMethod()> Public Sub GetPatientByNIR_NirPartage_RenvoieLUnDesDeux()
        Dim premier = EnregistrerPatient(PatientDeTest("PREMIER"))
        Dim nir = CLng(ValeurPatient("oa_patient_nir", premier))
        Dim second = EnregistrerPatient(PatientDeTest("SECOND", nir:=nir))

        Dim relu = dao.GetPatientByNIR(nir.ToString())

        CollectionAssert.Contains(New Long() {premier, second}, CLng(relu.PatientId))
    End Sub

    <TestMethod()> Public Sub NonExistencePatientNIR_NirLibre_RenvoieVrai()
        EnregistrerPatient(PatientDeTest())
        Assert.IsTrue(dao.NonExistencePatientNIR(1999999999999L, 0))
    End Sub

    <TestMethod()> Public Sub NonExistencePatientNIR_EnCreation_NirPris_RenvoieFaux()
        Dim fiche = PatientDeTest()
        EnregistrerPatient(fiche)
        Assert.IsFalse(dao.NonExistencePatientNIR(fiche.PatientNir, 0))
    End Sub

    <TestMethod()> Public Sub NonExistencePatientNIR_EnModification_SonPropreNir_RenvoieVrai()
        Dim fiche = PatientDeTest()
        Dim id = EnregistrerPatient(fiche)
        Assert.IsTrue(dao.NonExistencePatientNIR(fiche.PatientNir, CInt(id)))
    End Sub

    <TestMethod()> Public Sub NonExistencePatientNIR_EnModification_NirDUnAutre_RenvoieFaux()
        Dim fiche = PatientDeTest()
        EnregistrerPatient(fiche)
        Dim autre = EnregistrerPatient(PatientDeTest())
        Assert.IsFalse(dao.NonExistencePatientNIR(fiche.PatientNir, CInt(autre)))
    End Sub

    <TestMethod()> Public Sub NonExistencePatientNIR_NirDejaEnDouble_RenvoieFauxPourChacun()
        Dim premier = EnregistrerPatient(PatientDeTest())
        Dim nir = CLng(ValeurPatient("oa_patient_nir", premier))
        Dim second = EnregistrerPatient(PatientDeTest(nir:=nir))

        Assert.IsFalse(dao.NonExistencePatientNIR(nir, CInt(premier)))
        Assert.IsFalse(dao.NonExistencePatientNIR(nir, CInt(second)))
    End Sub

    <TestMethod()> Public Sub NonExistencePatientNIR_Zero_DejaPorteParUnPatient_RenvoieFaux()
        ' Comportement actuel : les patients sans NIR sont enregistrés avec 0. En
        ' création, une fiche où l'on saisit « 0 » est donc refusée dès qu'un patient
        ' sans NIR existe (la modification, elle, saute le contrôle pour « 0 »).
        EnregistrerPatient(PatientDeTest("SANS", "Nir", nir:=0))
        Assert.IsFalse(dao.NonExistencePatientNIR(0, 0))
    End Sub

    ' ---------------------------------------------------------------------
    ' Recherches (client)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub ListePatientDateNaissance_RenvoieLesPatientsNesCeJour()
        Dim premier = EnregistrerPatient(PatientDeTest("UN", dateNaissance:=New Date(1985, 6, 30)))
        Dim second = EnregistrerPatient(PatientDeTest("DEUX", dateNaissance:=New Date(1985, 6, 30)))
        EnregistrerPatient(PatientDeTest("VEILLE", dateNaissance:=New Date(1985, 6, 29)))

        Dim table As DataTable = Nothing
        SousCultureFrancaise(Sub() table = dao.ListePatientDateNaissance(New Date(1985, 6, 30)))

        CollectionAssert.AreEquivalent(New Long() {premier, second}, IdsDe(table).ToArray())
    End Sub

    <TestMethod()> Public Sub ListePatientDateNaissance_AucunPatientNeCeJour_TableVide()
        EnregistrerPatient(PatientDeTest(dateNaissance:=New Date(1985, 6, 30)))
        Assert.AreEqual(0, dao.ListePatientDateNaissance(New Date(1901, 1, 1)).Rows.Count)
    End Sub

    <TestMethod()> Public Sub GetFilteredPatient_ParDebutDuNom()
        Dim dupont = EnregistrerPatient(PatientDeTest("DUPONT", "Jean"))
        Dim dupontel = EnregistrerPatient(PatientDeTest("DUPONTEL", "Marc"))
        EnregistrerPatient(PatientDeTest("LEDUPONT", "Paul"))

        Dim liste = dao.GetFilteredPatient(Nothing, "DUPONT", Nothing)

        CollectionAssert.AreEquivalent(New Long() {dupont, dupontel}, IdsDe(liste).ToArray())
        Assert.AreEqual("DUPONT", liste.Single(Function(p) p.PatientId = dupont).PatientNom)
    End Sub

    <TestMethod()> Public Sub GetFilteredPatient_ParDebutDuPrenomEtDuNom()
        Dim jean = EnregistrerPatient(PatientDeTest("DUPONT", "Jean"))
        EnregistrerPatient(PatientDeTest("DUPONT", "Marc"))
        EnregistrerPatient(PatientDeTest("MARTIN", "Jeanne"))

        CollectionAssert.AreEquivalent(New Long() {jean}, IdsDe(dao.GetFilteredPatient("Jea", "DUP", Nothing)).ToArray())
    End Sub

    <TestMethod()> Public Sub GetFilteredPatient_ParDateDeNaissance()
        Dim ne = EnregistrerPatient(PatientDeTest("UN", dateNaissance:=New Date(1985, 6, 30)))
        EnregistrerPatient(PatientDeTest("DEUX", dateNaissance:=New Date(1990, 1, 1)))

        CollectionAssert.AreEquivalent(New Long() {ne}, IdsDe(dao.GetFilteredPatient("", "", New Date(1985, 6, 30))).ToArray())
    End Sub

    <TestMethod()> Public Sub GetFilteredPatient_SansCritere_RenvoieTousLesPatients()
        Dim premier = EnregistrerPatient(PatientDeTest("UN"))
        Dim second = EnregistrerPatient(PatientDeTest("DEUX"))

        Dim ids = IdsDe(dao.GetFilteredPatient("", Nothing, Nothing))

        CollectionAssert.IsSubsetOf(New Long() {premier, second}, ids.ToArray())
        Assert.AreEqual(CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient")), ids.Count)
    End Sub

    <TestMethod()> Public Sub GetFilteredPatient_JokersSaisisPrisALaLettre()
        Dim pourcent = EnregistrerPatient(PatientDeTest("DU%PONT"))
        EnregistrerPatient(PatientDeTest("DUBOIS"))
        Dim souligne = EnregistrerPatient(PatientDeTest("A_B"))
        EnregistrerPatient(PatientDeTest("AXB"))

        CollectionAssert.AreEquivalent(New Long() {pourcent}, IdsDe(dao.GetFilteredPatient(Nothing, "DU%", Nothing)).ToArray())
        CollectionAssert.AreEquivalent(New Long() {souligne}, IdsDe(dao.GetFilteredPatient(Nothing, "A_", Nothing)).ToArray())
    End Sub

    <TestMethod()> Public Sub GetFilteredPatient_ApostropheSaisieSansErreur()
        Dim id = EnregistrerPatient(PatientDeTest("D'ARTAGNAN"))
        CollectionAssert.AreEquivalent(New Long() {id}, IdsDe(dao.GetFilteredPatient(Nothing, "D'ART", Nothing)).ToArray())
    End Sub

    <TestMethod()> Public Sub GetAllPatientWithFilter_NomContenuSansEgardALaCasse()
        Dim dupont = EnregistrerPatient(PatientDeTest("DUPONT"))
        Dim ledupont = EnregistrerPatient(PatientDeTest("LEDUPONT"))
        EnregistrerPatient(PatientDeTest("MARTIN"))

        CollectionAssert.AreEquivalent(New Long() {dupont, ledupont}, Filtrer(True, False, nom:="uPoN").ToArray())
    End Sub

    <TestMethod()> Public Sub GetAllPatientWithFilter_PrenomContenu()
        Dim id = EnregistrerPatient(PatientDeTest("DUPONT", "Jean-Pierre"))
        EnregistrerPatient(PatientDeTest("DUPONT", "Jean"))

        CollectionAssert.AreEquivalent(New Long() {id}, Filtrer(True, False, prenom:="PIERRE").ToArray())
    End Sub

    <TestMethod()> Public Sub GetAllPatientWithFilter_SaisieAvecEspace_NeTrouvePasLeNomCompose()
        ' Comportement actuel : les espaces sont retirés de la saisie mais pas de la
        ' colonne ; « Jean Pierre » cherche « jeanpierre » et ne trouve pas « Jean Pierre ».
        Dim id = EnregistrerPatient(PatientDeTest("DUPONT", "Jean Pierre"))

        Assert.AreEqual(0, Filtrer(True, False, prenom:="Jean Pierre").Count)
        CollectionAssert.AreEquivalent(New Long() {id}, Filtrer(True, False, prenom:="Pierre").ToArray())
    End Sub

    <TestMethod()> Public Sub GetAllPatientWithFilter_NomLongTronqueATrenteCaracteres()
        ' Comportement actuel : convert(varchar, ...) sans longueur tronque la colonne à
        ' 30 caractères avant le LIKE ; la fin d'un nom plus long n'est pas cherchée.
        Dim id = EnregistrerPatient(PatientDeTest(New String("X"c, 30) & "QWYZ"))

        Assert.AreEqual(0, Filtrer(True, False, nom:="QWYZ").Count)
        CollectionAssert.AreEquivalent(New Long() {id}, Filtrer(True, False, nom:="XXXX").ToArray())
    End Sub

    <TestMethod()> Public Sub GetAllPatientWithFilter_ParDateDeNaissance()
        Dim ne = EnregistrerPatient(PatientDeTest("UN", dateNaissance:=New Date(1985, 6, 30)))
        EnregistrerPatient(PatientDeTest("DEUX", dateNaissance:=New Date(1990, 1, 1)))

        CollectionAssert.AreEquivalent(New Long() {ne}, Filtrer(True, False, dateNaissance:=New Date(1985, 6, 30)).ToArray())
    End Sub

    <TestMethod()> Public Sub GetAllPatientWithFilter_ParSitesAutorises()
        Dim site1 = EnregistrerPatient(PatientDeTest("UN", siteId:=9101))
        Dim site2 = EnregistrerPatient(PatientDeTest("DEUX", siteId:=9102))
        Dim site3 = EnregistrerPatient(PatientDeTest("TROIS", siteId:=9103))

        CollectionAssert.AreEquivalent(New Long() {site1, site2}, Filtrer(True, False, sites:=New List(Of Long) From {9101, 9102}).ToArray())
        ' Liste vide ou absente : pas de filtre sur le site.
        CollectionAssert.AreEquivalent(New Long() {site1, site2, site3}, Filtrer(True, False, sites:=New List(Of Long)).ToArray())
        CollectionAssert.AreEquivalent(New Long() {site1, site2, site3}, Filtrer(True, False).ToArray())
    End Sub

    <TestMethod()> Public Sub GetAllPatientWithFilter_RenvoieLesColonnesDeLaListe()
        Dim fiche = PatientDeTest("COLONNES", "Liste", siteId:=9101)
        Dim id = EnregistrerPatient(fiche)

        Dim table = dao.GetAllPatientWithFilter(True, False, Nothing, Nothing, Nothing, Nothing)

        For Each colonne In {"oa_patient_id", "oa_patient_nir", "oa_patient_prenom", "oa_patient_nom", "oa_patient_date_naissance",
                             "oa_patient_lieu_naissance", "oa_patient_date_entree_oasis", "oa_patient_date_sortie_oasis", "oa_patient_site_id"}
            Assert.IsTrue(table.Columns.Contains(colonne), colonne)
        Next
        Assert.AreEqual(9, table.Columns.Count)
        Dim ligne = table.Rows.Cast(Of DataRow)().Single(Function(r) CLng(r("oa_patient_id")) = id)
        Assert.AreEqual(fiche.PatientNir, CLng(ligne("oa_patient_nir")))
        Assert.AreEqual("COLONNES", CStr(ligne("oa_patient_nom")))
        Assert.AreEqual(9101, CInt(ligne("oa_patient_site_id")))
    End Sub

    ''' <summary>
    ''' Sept patients qui couvrent les deux côtés de chaque condition des filtres
    ''' « patients Oasis » et « hors Oasis ». Ordre : dansOasis, sortieDemain,
    ''' sortieNulle, sortieAujourdhui, sortiHier, horsDispositif, entreeNulle.
    ''' </summary>
    Private Function PatientsDesDeuxCotes() As Long()
        Dim dansOasis = EnregistrerPatient(PatientDeTest("DANSOASIS"))
        PoserDatesOasisPatient(dansOasis, EntreeOasis, DateNonRenseignee)
        Dim sortieDemain = EnregistrerPatient(PatientDeTest("SORTIEDEMAIN"))
        PoserDatesOasisPatient(sortieDemain, EntreeOasis, Date.Today.AddDays(1))
        Dim sortieNulle = EnregistrerPatient(PatientDeTest("SORTIENULLE"))
        PoserDatesOasisPatient(sortieNulle, EntreeOasis, Nothing)
        Dim sortieAujourdhui = EnregistrerPatient(PatientDeTest("SORTIEAUJOURDHUI"))
        PoserDatesOasisPatient(sortieAujourdhui, EntreeOasis, Date.Today)
        Dim sortiHier = EnregistrerPatient(PatientDeTest("SORTIHIER"))
        PoserDatesOasisPatient(sortiHier, EntreeOasis, Date.Today.AddDays(-1))
        Dim horsDispositif = EnregistrerPatient(PatientDeTest("HORSDISPOSITIF"))
        Dim entreeNulle = EnregistrerPatient(PatientDeTest("ENTREENULLE"))
        PoserDatesOasisPatient(entreeNulle, Nothing, Nothing)
        Return {dansOasis, sortieDemain, sortieNulle, sortieAujourdhui, sortiHier, horsDispositif, entreeNulle}
    End Function

    <TestMethod()> Public Sub GetAllPatientWithFilter_PatientsOasis_EntresEtPasEncoreSortis()
        Dim p = PatientsDesDeuxCotes()
        CollectionAssert.AreEquivalent(New Long() {p(0), p(1), p(2)}, Filtrer(False, True).ToArray())
    End Sub

    <TestMethod()> Public Sub GetAllPatientWithFilter_PatientsHorsOasis_JamaisEntresOuSortis()
        Dim p = PatientsDesDeuxCotes()
        CollectionAssert.AreEquivalent(New Long() {p(3), p(4), p(5), p(6)}, Filtrer(False, False).ToArray())
    End Sub

    <TestMethod()> Public Sub GetAllPatientWithFilter_Tous_IgnoreLesDates()
        Dim p = PatientsDesDeuxCotes()
        CollectionAssert.AreEquivalent(p, Filtrer(True, True).ToArray())
    End Sub

    <TestMethod()> Public Sub GetAllPatientWithFilter_HorsOasis_LesSortisEchappentAuxAutresCriteres()
        ' Comportement actuel : le filtre « hors Oasis » ajoute « ... OR sortie <= aujourd'hui »
        ' sans parenthèses. Tout patient sorti est renvoyé, quels que soient le nom, le
        ' prénom, la date de naissance et les sites demandés.
        Dim dupontHors = EnregistrerPatient(PatientDeTest("DUPONT", siteId:=9101))
        Dim martinSorti = EnregistrerPatient(PatientDeTest("MARTIN", siteId:=9102))
        PoserDatesOasisPatient(martinSorti, EntreeOasis, Date.Today.AddDays(-1))
        Dim dupontDans = EnregistrerPatient(PatientDeTest("DUPONT", siteId:=9101))
        PoserDatesOasisPatient(dupontDans, EntreeOasis, DateNonRenseignee)
        EnregistrerPatient(PatientDeTest("MARTIN", siteId:=9102))

        Dim ids = Filtrer(False, False, nom:="dupont", sites:=New List(Of Long) From {9101})

        CollectionAssert.AreEquivalent(New Long() {dupontHors, martinSorti}, ids.ToArray())
    End Sub

    <TestMethod()> Public Sub GetAllPatient_LesTroisFiltres()
        Dim p = PatientsDesDeuxCotes()

        CollectionAssert.AreEquivalent(p, IdsDe(dao.GetAllPatient(True, False)).ToArray())
        CollectionAssert.AreEquivalent(New Long() {p(0), p(1), p(2)}, IdsDe(dao.GetAllPatient(False, True)).ToArray())
        CollectionAssert.AreEquivalent(New Long() {p(3), p(4), p(5), p(6)}, IdsDe(dao.GetAllPatient(False, False)).ToArray())
    End Sub

    <TestMethod()> Public Sub GetByDRC_PatientsAyantUnAntecedentSurLaDrc()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim drc = CreerDrc()
        Dim autreDrc = CreerDrc()
        Dim deuxAntecedents = EnregistrerPatient(PatientDeTest("DEUX"))
        CreerAntecedentSurDrc(deuxAntecedents, drc, idUtilisateur)
        CreerAntecedentSurDrc(deuxAntecedents, drc, idUtilisateur)
        Dim annule = EnregistrerPatient(PatientDeTest("ANNULE"))
        CreerAntecedentSurDrc(annule, drc, idUtilisateur, inactif:=True)
        Dim autre = EnregistrerPatient(PatientDeTest("AUTRE"))
        CreerAntecedentSurDrc(autre, autreDrc, idUtilisateur)
        EnregistrerPatient(PatientDeTest("SANS"))

        Dim liste = dao.GetByDRC(drc)

        ' Un patient n'apparaît qu'une fois (DISTINCT). Comportement actuel : un
        ' antécédent annulé (inactif) suffit à retenir le patient.
        CollectionAssert.AreEquivalent(New Long() {deuxAntecedents, annule}, IdsDe(liste).ToArray())
        Assert.AreEqual("DEUX", liste.Single(Function(x) x.PatientId = deuxAntecedents).PatientNom)
    End Sub

    <TestMethod()> Public Sub GetByDRC_DrcSansAntecedent_ListeVide()
        EnregistrerPatient(PatientDeTest())
        Assert.AreEqual(0, dao.GetByDRC(CreerDrc()).Count)
    End Sub

    ' ---------------------------------------------------------------------
    ' Modification, sortie du dispositif (client)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub ModificationPatient_RemplaceLesZonesSaisies()
        Dim id = EnregistrerPatient(PatientDeTest("AVANT", "Prenom", siteId:=9101), siegeId:=7)
        dao.ModificationPatientTaille(CInt(id), 172)
        PatientDao.BlocageMedical(CInt(id))
        Dim fiche = dao.GetPatient(CInt(id))
        fiche.PatientNir = 2850630123456L
        fiche.INS = 2850630123457L
        fiche.PatientNom = "APRES"
        fiche.PatientPrenom = "Autre"
        fiche.PatientNomMarital = "EPOUX"
        fiche.PatientDateNaissance = New Date(1985, 6, 30)
        fiche.PatientGenreId = Patient.EnumGenreId.Masculin
        fiche.PatientAdresse1 = "2 place du Marche"
        fiche.PatientAdresse2 = "Appartement 3"
        fiche.PatientCodePostal = "97615"
        fiche.PatientVille = "Pamandzi"
        fiche.PatientTel1 = "0269111111"
        fiche.PatientTel2 = "0639222222"
        fiche.PatientEmail = "apres@exemple.fr"
        fiche.PatientDateDeces = New Date(2026, 1, 2)
        fiche.PatientCommentaireSortie = "Commentaire"
        fiche.PatientSiteId = 9102
        fiche.PatientUniteSanitaireId = 9202
        fiche.PatientInternet = True
        fiche.Profession = "Pecheur"
        fiche.PharmacienId = 0
        fiche.PatientSiegeId = 8
        fiche.Taille = 150
        fiche.BlocageMedical = False

        Dim retour As Boolean
        SousCultureFrancaise(Sub() retour = dao.ModificationPatient(fiche, New Utilisateur))

        Assert.IsTrue(retour)
        Dim relu = dao.GetPatient(CInt(id))
        Assert.AreEqual(2850630123456L, relu.PatientNir)
        Assert.AreEqual(2850630123457L, relu.INS)
        Assert.AreEqual("APRES", relu.PatientNom)
        Assert.AreEqual("Autre", relu.PatientPrenom)
        Assert.AreEqual("EPOUX", relu.PatientNomMarital)
        Assert.AreEqual(New Date(1985, 6, 30), relu.PatientDateNaissance)
        Assert.AreEqual("M", relu.PatientGenreId.Trim())
        Assert.AreEqual("2 place du Marche", relu.PatientAdresse1)
        Assert.AreEqual("Appartement 3", relu.PatientAdresse2)
        Assert.AreEqual("97615", relu.PatientCodePostal)
        Assert.AreEqual("Pamandzi", relu.PatientVille)
        Assert.AreEqual("0269111111", relu.PatientTel1)
        Assert.AreEqual("0639222222", relu.PatientTel2)
        Assert.AreEqual("apres@exemple.fr", relu.PatientEmail)
        Assert.AreEqual(DateNonRenseignee, relu.PatientDateEntree)
        Assert.AreEqual(DateNonRenseignee, relu.PatientDateSortie)
        Assert.AreEqual(New Date(2026, 1, 2), relu.PatientDateDeces)
        Assert.AreEqual("Commentaire", relu.PatientCommentaireSortie)
        Assert.AreEqual(9102, relu.PatientSiteId)
        Assert.AreEqual(9202, relu.PatientUniteSanitaireId)
        Assert.IsTrue(relu.PatientInternet)
        Assert.AreEqual("Pecheur", relu.Profession)
        ' Colonnes que ModificationPatient n'écrit pas.
        Assert.AreEqual(7, relu.PatientSiegeId)
        Assert.AreEqual(172, relu.Taille)
        Assert.IsTrue(relu.BlocageMedical)
        Assert.AreEqual(0, CInt(ValeurPatient("oa_patient_nir_modulo", id)))
    End Sub

    <TestMethod()> Public Sub ModificationPatient_EntreeEtSortieRenseignees_NeCreePasDeParcours()
        Dim id = EnregistrerPatient(PatientDeTest())
        Dim fiche = dao.GetPatient(CInt(id))
        fiche.PatientDateEntree = New Date(2021, 3, 1)
        fiche.PatientDateSortie = New Date(2024, 1, 31)

        Assert.IsTrue(dao.ModificationPatient(fiche, New Utilisateur))

        Dim relu = dao.GetPatient(CInt(id))
        Assert.AreEqual(New Date(2021, 3, 1), relu.PatientDateEntree)
        Assert.AreEqual(New Date(2024, 1, 31), relu.PatientDateSortie)
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient_parcours WHERE oa_parcours_patient_id = @p0", id)))
    End Sub

    <TestMethod()> Public Sub ModificationPatient_VersUnNirDejaPris_EstAcceptee()
        ' Comportement actuel : comme à la création, seule la fiche patient contrôle le NIR.
        Dim premier = EnregistrerPatient(PatientDeTest("PREMIER"))
        Dim nir = CLng(ValeurPatient("oa_patient_nir", premier))
        Dim second = EnregistrerPatient(PatientDeTest("SECOND"))
        Dim fiche = dao.GetPatient(CInt(second))
        fiche.PatientNir = nir

        Assert.IsTrue(dao.ModificationPatient(fiche, New Utilisateur))

        Assert.AreEqual(2, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient WHERE oa_patient_nir = @p0", nir)))
    End Sub

    <TestMethod()> Public Sub ModificationPatient_Inexistant_RenvoieVraiSansRienEcrire()
        ' Comportement actuel : aucune ligne touchée n'est pas une erreur.
        Dim fiche = PatientDeTest("FANTOME")
        fiche.PatientId = PatientAbsent

        Assert.IsTrue(dao.ModificationPatient(fiche, New Utilisateur))
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient WHERE oa_patient_nom = 'FANTOME'")))
    End Sub

    <TestMethod()> Public Sub DeclarationSortie_EcritSeulementLaDateEtLeCommentaire()
        Dim id = EnregistrerPatient(PatientDeTest("SORTANT"))
        PoserDatesOasisPatient(id, EntreeOasis, DateNonRenseignee)
        Dim fiche = dao.GetPatient(CInt(id))
        fiche.PatientDateSortie = Date.Today.AddDays(-1)
        fiche.PatientCommentaireSortie = "Demenagement"
        fiche.PatientNom = "IGNORE"

        Dim retour As Boolean
        SousCultureFrancaise(Sub() retour = PatientDao.DeclarationSortie(fiche))

        Assert.IsTrue(retour)
        Dim relu = dao.GetPatient(CInt(id))
        Assert.AreEqual(Date.Today.AddDays(-1), relu.PatientDateSortie)
        Assert.AreEqual("Demenagement", relu.PatientCommentaireSortie)
        Assert.AreEqual("SORTANT", relu.PatientNom)
        Assert.AreEqual(EntreeOasis, relu.PatientDateEntree)
    End Sub

    <TestMethod()> Public Sub DeclarationSortie_LePatientPasseDansLaListeHorsOasis()
        Dim id = EnregistrerPatient(PatientDeTest("SORTANT"))
        PoserDatesOasisPatient(id, EntreeOasis, DateNonRenseignee)
        CollectionAssert.Contains(Filtrer(False, True), id)

        Dim fiche = dao.GetPatient(CInt(id))
        fiche.PatientDateSortie = Date.Today
        fiche.PatientCommentaireSortie = ""
        PatientDao.DeclarationSortie(fiche)

        CollectionAssert.DoesNotContain(Filtrer(False, True), id)
        CollectionAssert.Contains(Filtrer(False, False), id)
    End Sub

    <TestMethod()> Public Sub DeclarationSortie_PatientInexistant_RenvoieVrai()
        ' Comportement actuel : aucune ligne touchée n'est pas une erreur.
        Dim fiche As New Patient With {.PatientId = PatientAbsent, .PatientDateSortie = Date.Today, .PatientCommentaireSortie = ""}
        Assert.IsTrue(PatientDao.DeclarationSortie(fiche))
    End Sub

    ' ---------------------------------------------------------------------
    ' Synthèse, taille, blocage médical (client)
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub ModificationDateMajSynthese_ParUnMedecin_DateDuJourEtBlocage()
        Dim id = EnregistrerPatient(PatientDeTest())
        PoserDateMajSynthese(id, New Date(2020, 1, 1))
        Dim medecin = Auteur(CreerUtilisateur(avecCle:=False))
        Dim retour As Boolean

        SousCultureFrancaise(Sub() retour = dao.ModificationDateMajSynthesePatient(CInt(id), medecin))

        Assert.IsTrue(retour)
        Dim relu = dao.GetPatient(CInt(id))
        Assert.AreEqual(Date.Today, relu.PatientSyntheseDateMaj.Date)
        Assert.IsTrue(relu.BlocageMedical, "une mise à jour par un profil MEDICAL bloque la fiche")
    End Sub

    <TestMethod()> Public Sub ModificationDateMajSynthese_ParUnNonMedecin_SansBlocage()
        Dim id = EnregistrerPatient(PatientDeTest())

        dao.ModificationDateMajSynthesePatient(CInt(id), Auteur(CreerUtilisateur(avecCle:=False), "PARAMEDICAL"))

        Dim relu = dao.GetPatient(CInt(id))
        Assert.AreEqual(Date.Today, relu.PatientSyntheseDateMaj.Date)
        Assert.IsFalse(relu.BlocageMedical)
    End Sub

    <TestMethod()> Public Sub ModificationDateMajSynthese_FicheDejaBloquee_ResteBloquee()
        Dim id = EnregistrerPatient(PatientDeTest())
        PatientDao.BlocageMedical(CInt(id))

        dao.ModificationDateMajSynthesePatient(CInt(id), Auteur(CreerUtilisateur(avecCle:=False), "PARAMEDICAL"))

        Assert.IsTrue(dao.GetPatient(CInt(id)).BlocageMedical)
    End Sub

    <TestMethod()> Public Sub ModificationDateMajSynthese_PatientInexistant_LeveArgumentException()
        Assert.ThrowsException(Of ArgumentException)(
            Sub() dao.ModificationDateMajSynthesePatient(PatientAbsent, Auteur(CreerUtilisateur(avecCle:=False))))
    End Sub

    <TestMethod()> Public Sub ModificationPatientTaille_EstRelue()
        Dim id = EnregistrerPatient(PatientDeTest())

        Assert.IsTrue(dao.ModificationPatientTaille(CInt(id), 172))
        Assert.AreEqual(172, dao.GetPatient(CInt(id)).Taille)

        dao.ModificationPatientTaille(CInt(id), 0)
        Assert.AreEqual(0, dao.GetPatient(CInt(id)).Taille)
    End Sub

    <TestMethod()> Public Sub BlocageMedical_BloqueLaFicheDuSeulPatientVise()
        Dim id = EnregistrerPatient(PatientDeTest("BLOQUE"))
        Dim autre = EnregistrerPatient(PatientDeTest("LIBRE"))

        Assert.IsTrue(PatientDao.BlocageMedical(CInt(id)))

        Assert.IsTrue(dao.GetPatient(CInt(id)).BlocageMedical)
        Assert.IsFalse(dao.GetPatient(CInt(autre)).BlocageMedical)
    End Sub

    <TestMethod()> Public Sub BlocageMedical_PatientInexistant_RenvoieVrai()
        ' Comportement actuel : aucune ligne touchée n'est pas une erreur.
        Assert.IsTrue(PatientDao.BlocageMedical(PatientAbsent))
    End Sub

    ' ---------------------------------------------------------------------
    ' Contre-indications et allergies en texte (client et portail)
    ' ---------------------------------------------------------------------

    Private Shared Sub AjouterContreIndicationAtc(patientId As Long, code As String, libelle As String, idUtilisateur As Long)
        Dim daoAtc As New ContreIndicationATCDao
        Assert.IsTrue(daoAtc.CreationContreIndicationATC(
            New ContreIndicationATC With {.PatientId = patientId, .ATCId = code, .DenominationATC = libelle},
            Auteur(idUtilisateur)))
    End Sub

    Private Shared Sub AjouterContreIndicationSubstance(patientId As Long, substanceId As Long, libelle As String, idUtilisateur As Long)
        Dim daoSubstance As New ContreIndicationSubstanceDao
        Assert.IsTrue(daoSubstance.CreationContreIndicationSubstance(
            New ContreIndicationSubstance With {.PatientId = patientId, .SubstanceId = substanceId, .SubstancePereId = 0, .DenominationSubstance = libelle},
            Auteur(idUtilisateur)))
    End Sub

    Private Shared Sub AjouterAllergie(patientId As Long, substanceId As Long, libelle As String, idUtilisateur As Long)
        Dim daoAllergie As New AllergieDao
        Assert.IsTrue(daoAllergie.CreationAllergie(
            New Allergie With {.PatientId = patientId, .SubstanceId = substanceId, .SubstancePereId = 0, .DenominationSubstance = libelle},
            Auteur(idUtilisateur)))
    End Sub

    ''' <summary>
    ''' Deux ATC et trois substances actives (dont une substance père) pour le patient,
    ''' plus une de chaque annulée et une contre-indication d'un autre patient.
    ''' </summary>
    Private Shared Sub PoserContreIndications(idPatient As Long, idUtilisateur As Long)
        AjouterContreIndicationAtc(idPatient, "N02BE01", "ANTALGIQUE TEST", idUtilisateur)
        AjouterContreIndicationAtc(idPatient, "C01BD01", "ANTIARYTHMIQUE TEST", idUtilisateur)
        AjouterContreIndicationAtc(idPatient, "J01CA04", "ANTIBIOTIQUE ANNULE", idUtilisateur)
        Dim daoAtc As New ContreIndicationATCDao
        daoAtc.AnnulationContreIndicationATC(
            CInt(Scalaire("SELECT contre_indication_id FROM oasis.oa_patient_contre_indication_atc WHERE patient_id = @p0 AND code_atc = 'J01CA04'", idPatient)),
            Auteur(idUtilisateur))

        AjouterContreIndicationSubstance(idPatient, 222, "SUBSTANCE B", idUtilisateur)
        AjouterContreIndicationSubstance(idPatient, 111, "SUBSTANCE A", idUtilisateur)
        CreerContreIndicationSubstancePere(idPatient, 900, "FAMILLE TEST", idUtilisateur)
        AjouterContreIndicationSubstance(idPatient, 333, "SUBSTANCE ANNULEE", idUtilisateur)
        Dim daoSubstance As New ContreIndicationSubstanceDao
        daoSubstance.AnnulationContreIndicationSubstance(
            CInt(Scalaire("SELECT contre_indication_id FROM oasis.oa_patient_contre_indication_substance WHERE patient_id = @p0 AND substance_id = 333", idPatient)),
            Auteur(idUtilisateur))

        Dim autre = EnregistrerPatient(PatientDeTest("AUTRE"))
        AjouterContreIndicationAtc(autre, "A01AA01", "AUTRE PATIENT", idUtilisateur)
        AjouterContreIndicationSubstance(autre, 444, "AUTRE PATIENT", idUtilisateur)
    End Sub

    Private Shared ReadOnly ContreIndicationsAttendues As String =
        "ATC :" & vbCrLf &
        "C01BD01 : ANTIARYTHMIQUE TEST" & vbCrLf &
        "N02BE01 : ANTALGIQUE TEST" & vbCrLf &
        "Substance :" & vbCrLf &
        "900 : FAMILLE TEST" & vbCrLf &
        "111 : SUBSTANCE A" & vbCrLf &
        "222 : SUBSTANCE B" & vbCrLf

    <TestMethod()> Public Sub GetStringContreIndicationByPatient_AtcPuisSubstancesActives()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim id = EnregistrerPatient(PatientDeTest())
        PoserContreIndications(id, idUtilisateur)

        ' ATC par code, substances par dénomination : la substance père, dont la
        ' dénomination de substance est vide, vient en tête.
        Assert.AreEqual(ContreIndicationsAttendues, dao.GetStringContreIndicationByPatient(id))
    End Sub

    <TestMethod()> Public Sub GetStringContreIndicationByPatient_SousWeb_CommeLaSynthese()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim id = EnregistrerPatient(PatientDeTest())
        PoserContreIndications(id, idUtilisateur)
        UtiliserCompte(Compte.Web)

        Assert.AreEqual(ContreIndicationsAttendues, dao.GetStringContreIndicationByPatient(id))
    End Sub

    <TestMethod()> Public Sub GetStringContreIndicationByPatient_SeulementDesAtc_SansRubriqueSubstance()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim id = EnregistrerPatient(PatientDeTest())
        AjouterContreIndicationAtc(id, "C01BD01", "ANTIARYTHMIQUE TEST", idUtilisateur)

        Assert.AreEqual("ATC :" & vbCrLf & "C01BD01 : ANTIARYTHMIQUE TEST" & vbCrLf, dao.GetStringContreIndicationByPatient(id))
    End Sub

    <TestMethod()> Public Sub GetStringContreIndicationByPatient_Aucune_ChaineVide()
        Dim id = EnregistrerPatient(PatientDeTest())
        Assert.AreEqual("", dao.GetStringContreIndicationByPatient(id))
    End Sub

    Private Shared Sub PoserAllergies(idPatient As Long, idUtilisateur As Long)
        AjouterAllergie(idPatient, 111, "SUBSTANCE A", idUtilisateur)
        CreerAllergieSubstancePere(idPatient, 900, "FAMILLE TEST", idUtilisateur)
        AjouterAllergie(idPatient, 333, "SUBSTANCE ANNULEE", idUtilisateur)
        Dim daoAllergie As New AllergieDao
        daoAllergie.AnnulationAllergie(
            CInt(Scalaire("SELECT allergie_id FROM oasis.oa_patient_allergie WHERE patient_id = @p0 AND substance_id = 333", idPatient)),
            Auteur(idUtilisateur))
        Dim autre = EnregistrerPatient(PatientDeTest("AUTRE"))
        CreerAllergieSubstancePere(autre, 901, "AUTRE PATIENT", idUtilisateur)
    End Sub

    ' Comportement actuel : seule la dénomination de la substance père est écrite. Une
    ' allergie saisie sur une substance (sans père) donne une ligne vide, ici la
    ' dernière, et n'apparaît donc pas dans l'infobulle de la synthèse.
    Private Shared ReadOnly AllergiesAttendues As String =
        "Substance : " & vbCrLf &
        "FAMILLE TEST" & vbCrLf &
        "" & vbCrLf

    <TestMethod()> Public Sub GetStringAllergieByPatient_SubstancesPeresActives()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim id = EnregistrerPatient(PatientDeTest())
        PoserAllergies(id, idUtilisateur)

        Assert.AreEqual(AllergiesAttendues, dao.GetStringAllergieByPatient(id))
    End Sub

    <TestMethod()> Public Sub GetStringAllergieByPatient_SousWeb_CommeLaSynthese()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim id = EnregistrerPatient(PatientDeTest())
        PoserAllergies(id, idUtilisateur)
        UtiliserCompte(Compte.Web)

        Assert.AreEqual(AllergiesAttendues, dao.GetStringAllergieByPatient(id))
    End Sub

    <TestMethod()> Public Sub GetStringAllergieByPatient_Aucune_ChaineVide()
        Dim id = EnregistrerPatient(PatientDeTest())
        Assert.AreEqual("", dao.GetStringAllergieByPatient(id))
    End Sub

End Class
