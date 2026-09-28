Imports System.Globalization
Imports System.Threading
Imports Oasis_Common

''' <summary>
''' RorDao contre la base de test. Le répertoire des intervenants est lu, créé et
''' modifié par le client lourd (RadFRorListe, RadFRorDetailEdit,
''' RadFAnnuaireProfessionnelSelect, RadFOperatorSelect, fiches ordonnance et
''' rendez-vous) : tout tourne sous oasis_client. Les intervenants Oasis 1 et 2 de
''' 28-reference-parcours.sql sont toujours présents : les listes sont comparées
''' sur les intervenants créés par le test.
''' </summary>
<TestClass()> Public Class RorDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New RorDao

    Private Shared Function IdsDe(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("oa_ror_id"))).ToArray()
    End Function

    Private Shared Function Parmi(ids As IEnumerable(Of Long), ParamArray retenus() As Long) As Long()
        Return ids.Where(Function(i) retenus.Contains(i)).ToArray()
    End Function

    <TestMethod()> Public Sub CreationRor_PuisGetRorById_RelitChaqueColonne()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)

        Dim idRor = CreerRorParcours("DUPONT Jean", email:="jean.dupont@exemple.fr", rpps:=10001234567L,
                                     extractionAnnuaire:=True, identifiantNational:="810001234567",
                                     identifiantStructure:="S123", modeExercice:="L", professionId:=10,
                                     typeSavoirFaire:="S", codeSavoirFaire:="SM04", utilisateurId:=idUtilisateur)

        Assert.IsTrue(idRor > 0)
        Dim lu = dao.GetRorById(CInt(idRor))
        Assert.AreEqual(idRor, lu.Id)
        Assert.AreEqual(CLng(SpecialiteParcoursTest), lu.SpecialiteId)
        Assert.AreEqual("DUPONT Jean", lu.Nom)
        Assert.IsFalse(lu.Oasis, "non écrit par CreationRor")
        Assert.AreEqual("Intervenant", lu.Type)
        Assert.AreEqual(0L, lu.StructureId)
        Assert.AreEqual("Cabinet de test", lu.StructureNom)
        Assert.AreEqual("1 rue du Test", lu.Adresse1)
        Assert.AreEqual("Batiment B", lu.Adresse2)
        Assert.AreEqual("97600", lu.CodePostal)
        Assert.AreEqual("Mamoudzou", lu.Ville)
        Assert.AreEqual("ITC", lu.Code)
        Assert.AreEqual("0269123456", lu.Telephone)
        Assert.AreEqual("jean.dupont@exemple.fr", lu.Email)
        Assert.AreEqual("Intervenant de test", lu.Commentaire)
        Assert.AreEqual(10001234567L, lu.Rpps)
        Assert.AreEqual(0L, lu.Finess)
        Assert.AreEqual(0L, lu.Adeli)
        Assert.IsFalse(lu.Inactif)
        Assert.AreEqual(idUtilisateur, lu.UserCreation)
        Assert.AreEqual(Date.Today, lu.DateCreation.Date)
        Assert.AreEqual(0L, lu.UserModification)
        Assert.AreEqual(Date.MinValue, lu.DateModification)
        Assert.IsTrue(lu.ExtractionAnnuaire)
        Assert.AreEqual("810001234567", lu.IdentifiantNational)
        Assert.AreEqual("S123", lu.IdentifiantStructure)
        Assert.AreEqual("L", lu.CodeModeExercice_r23)
        Assert.AreEqual(10, lu.CodeProfessionSante_g15)
        Assert.AreEqual("S", lu.CodeTypeSavoirFaire_r04)
        Assert.AreEqual("SM04", lu.CodeSavoirFaire)
        Assert.AreEqual(0L, lu.CleReferenceAnnuaire)
    End Sub

    <TestMethod()> Public Sub CreationRor_ChampsNonRenseignes_SontEnregistresVides()
        Dim cultureAvant = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
        Dim idRor As Long
        Try
            ' Fiche minimale, comme RadFOperatorSelect : les chaînes absentes passent par Coalesce.
            idRor = dao.CreationRor(New Ror With {.SpecialiteId = SpecialiteParcoursTest, .Nom = "MINIMAL"},
                                    New Utilisateur With {.UtilisateurId = 0})
        Finally
            Thread.CurrentThread.CurrentCulture = cultureAvant
        End Try

        Dim lu = dao.GetRorById(CInt(idRor))
        Assert.AreEqual("MINIMAL", lu.Nom)
        Assert.AreEqual("", lu.Type)
        Assert.AreEqual("", lu.StructureNom)
        Assert.AreEqual("", lu.Adresse1)
        Assert.AreEqual("", lu.Email)
        Assert.AreEqual("", lu.IdentifiantNational)
        Assert.AreEqual("", lu.CodeSavoirFaire)
        Assert.AreEqual(0L, lu.Rpps)
        Assert.IsFalse(lu.ExtractionAnnuaire)
    End Sub

    <TestMethod()> Public Sub GetRorById_IntervenantAbsent_LeveUneErreur()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetRorById(987654321))
        StringAssert.Contains(erreur.Message, "inexistant")
    End Sub

    <TestMethod()> Public Sub GetRorById_IntervenantOasisDeReference_EstMarqueOasis()
        Dim lu = dao.GetRorById(CInt(RorMedecinReferentOasis))

        Assert.IsTrue(lu.Oasis)
        Assert.AreEqual(CLng(SpecialiteMedecinReferentOasis), lu.SpecialiteId)
    End Sub

    <TestMethod()> Public Sub ModificationRor_ReecritLaFicheEtEffaceLeCode()
        Dim idCreateur = CreerUtilisateur(avecCle:=False)
        Dim idModificateur = CreerUtilisateur(avecCle:=False)
        Dim idRor = CreerRorParcours("AVANT", utilisateurId:=idCreateur)
        Dim fiche = dao.GetRorById(CInt(idRor))
        fiche.SpecialiteId = SpecialiteParcoursAutre
        fiche.Nom = "APRES"
        fiche.Type = "Structure"
        fiche.StructureId = 12
        fiche.StructureNom = "Clinique de test"
        fiche.Adresse1 = "2 avenue du Test"
        fiche.Adresse2 = ""
        fiche.CodePostal = "97610"
        fiche.Ville = "Dzaoudzi"
        fiche.Code = "NOUVEAU"
        fiche.Telephone = "0269999999"
        fiche.Email = "apres@exemple.fr"
        fiche.Commentaire = "Modifie"
        fiche.Rpps = 10009999999L
        fiche.Finess = 976000001L
        fiche.Adeli = 976123456L
        fiche.Inactif = True
        fiche.ExtractionAnnuaire = True
        fiche.IdentifiantNational = "810009999999"
        fiche.IdentifiantStructure = "S999"
        fiche.CodeModeExercice_r23 = "S"
        fiche.CodeProfessionSante_g15 = 60
        fiche.CodeTypeSavoirFaire_r04 = "C"
        fiche.CodeSavoirFaire = "SI01"
        fiche.CleReferenceAnnuaire = 77

        Dim cultureAvant = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
        Try
            Assert.IsTrue(dao.ModificationRor(fiche, New Utilisateur With {.UtilisateurId = CInt(idModificateur)}))
        Finally
            Thread.CurrentThread.CurrentCulture = cultureAvant
        End Try

        Dim relu = dao.GetRorById(CInt(idRor))
        Assert.AreEqual(CLng(SpecialiteParcoursAutre), relu.SpecialiteId)
        Assert.AreEqual("APRES", relu.Nom)
        Assert.AreEqual("Structure", relu.Type)
        Assert.AreEqual(12L, relu.StructureId)
        Assert.AreEqual("Clinique de test", relu.StructureNom)
        Assert.AreEqual("2 avenue du Test", relu.Adresse1)
        Assert.AreEqual("", relu.Adresse2)
        Assert.AreEqual("97610", relu.CodePostal)
        Assert.AreEqual("Dzaoudzi", relu.Ville)
        ' Comportement actuel : ModificationRor écrit toujours un code vide (@code = "").
        Assert.AreEqual("", relu.Code)
        Assert.AreEqual("0269999999", relu.Telephone)
        Assert.AreEqual("apres@exemple.fr", relu.Email)
        Assert.AreEqual("Modifie", relu.Commentaire)
        Assert.AreEqual(10009999999L, relu.Rpps)
        Assert.AreEqual(976000001L, relu.Finess)
        Assert.AreEqual(976123456L, relu.Adeli)
        Assert.IsTrue(relu.Inactif)
        Assert.IsTrue(relu.ExtractionAnnuaire)
        Assert.AreEqual("810009999999", relu.IdentifiantNational)
        Assert.AreEqual("S999", relu.IdentifiantStructure)
        Assert.AreEqual("S", relu.CodeModeExercice_r23)
        Assert.AreEqual(60, relu.CodeProfessionSante_g15)
        Assert.AreEqual("C", relu.CodeTypeSavoirFaire_r04)
        Assert.AreEqual("SI01", relu.CodeSavoirFaire)
        Assert.AreEqual(77L, relu.CleReferenceAnnuaire)
        Assert.AreEqual(idCreateur, relu.UserCreation, "l'auteur de la création ne change pas")
        Assert.AreEqual(idModificateur, relu.UserModification)
        Assert.AreEqual(Date.Today, relu.DateModification.Date)
    End Sub

    <TestMethod()> Public Sub GetAllRor_ExclutLesIntervenantsInactifs()
        Dim actif = CreerRorParcours("ACTIF")
        Dim inactif = CreerRorParcours("INACTIF", inactif:=True)

        Dim ids = IdsDe(dao.GetAllRor())

        CollectionAssert.Contains(ids, actif)
        CollectionAssert.DoesNotContain(ids, inactif)
        CollectionAssert.Contains(ids, RorMedecinReferentOasis)
    End Sub

    <TestMethod()> Public Sub GetRorBySpecialiteAndType_FiltreEtTrieParNom()
        Dim cardioB = CreerRorParcours("B CARDIO", specialiteId:=SpecialiteParcoursTest)
        Dim cardioA = CreerRorParcours("A CARDIO", specialiteId:=SpecialiteParcoursTest)
        Dim cardioStructure = CreerRorParcours("C CLINIQUE", specialiteId:=SpecialiteParcoursTest, typeRor:="Structure")
        Dim dermato = CreerRorParcours("D DERMATO", specialiteId:=SpecialiteParcoursAutre)
        Dim tous = New Long() {cardioA, cardioB, cardioStructure, dermato}

        Dim parSpecialite = dao.GetRorBySpecialiteAndType(SpecialiteParcoursTest, "")
        Dim parType = dao.GetRorBySpecialiteAndType(0, "Intervenant")
        Dim lesDeux = dao.GetRorBySpecialiteAndType(SpecialiteParcoursTest, " Intervenant ")
        Dim sansFiltre = dao.GetRorBySpecialiteAndType(0, "")

        CollectionAssert.AreEqual(New Long() {cardioA, cardioB, cardioStructure}, IdsDe(parSpecialite))
        CollectionAssert.AreEqual(New Long() {cardioA, cardioB, dermato}, Parmi(IdsDe(parType), tous))
        CollectionAssert.AreEqual(New Long() {cardioA, cardioB}, IdsDe(lesDeux), "le type est comparé sans ses espaces")
        CollectionAssert.AreEqual(New Long() {cardioA, cardioB, cardioStructure, dermato}, Parmi(IdsDe(sansFiltre), tous))
        Dim ligne = parSpecialite.Rows(0)
        Assert.AreEqual("A CARDIO", CStr(ligne("oa_ror_nom")))
        Assert.AreEqual(SpecialiteParcoursTestLibelle, CStr(ligne("oa_r_specialite_description")))
        Assert.AreEqual("Cabinet de test", CStr(ligne("oa_ror_structure_nom")))
        Assert.AreEqual("Mamoudzou", CStr(ligne("oa_ror_ville")))
    End Sub

    <TestMethod()> Public Sub GetRorBySpecialiteAndType_FiltreLAnnuaireEtPasLIndicateurInactif()
        Dim present = CreerRorParcours("PRESENT")
        Dim retire = CreerRorParcours("RETIRE")
        RetirerRorAnnuaire(retire)
        Dim inactif = CreerRorParcours("INACTIF", inactif:=True)

        Dim ids = IdsDe(dao.GetRorBySpecialiteAndType(SpecialiteParcoursTest, ""))

        CollectionAssert.Contains(ids, present)
        CollectionAssert.DoesNotContain(ids, retire, "retiré de l'annuaire")
        ' Comportement actuel : la liste filtre oa_ror_annuaire_inactif, pas
        ' oa_ror_inactif ; un intervenant désactivé depuis sa fiche reste proposé.
        CollectionAssert.Contains(ids, inactif)
    End Sub

    <TestMethod()> Public Sub GetListRorByNomAndCommune_FiltreParNomVilleEtDepartement()
        Dim mamoudzou = CreerRorParcours("MARTIN Paul", ville:="Mamoudzou", codePostal:="97600")
        Dim dzaoudzi = CreerRorParcours("MARTINEZ Ana", ville:="Dzaoudzi", codePostal:="97615")
        Dim paris = CreerRorParcours("DURAND Luc", ville:="Paris", codePostal:="75011")
        Dim inactif = CreerRorParcours("MARTIN Inactif", ville:="Mamoudzou", codePostal:="97600", inactif:=True)
        Dim tous = New Long() {mamoudzou, dzaoudzi, paris, inactif}
        Dim idsRetenus = Function(liste As List(Of Ror)) Parmi(liste.Select(Function(r) r.Id).OrderBy(Function(i) i), tous)

        CollectionAssert.AreEqual(New Long() {mamoudzou, dzaoudzi, inactif}, idsRetenus(dao.GetListRorByNomAndCommune("MARTIN", "", "")))
        CollectionAssert.AreEqual(New Long() {dzaoudzi}, idsRetenus(dao.GetListRorByNomAndCommune("", "Dzaou", "")))
        CollectionAssert.AreEqual(New Long() {mamoudzou, dzaoudzi, inactif}, idsRetenus(dao.GetListRorByNomAndCommune("", "", "976")))
        CollectionAssert.AreEqual(New Long() {}, idsRetenus(dao.GetListRorByNomAndCommune("", "", "600")), "le département est un préfixe")
        CollectionAssert.AreEqual(New Long() {mamoudzou, inactif}, idsRetenus(dao.GetListRorByNomAndCommune("MARTIN", "Mamoudzou", "976")))
        ' Sans critère : tout le répertoire, inactifs compris.
        CollectionAssert.AreEqual(tous, idsRetenus(dao.GetListRorByNomAndCommune(" ", "", "")))
        Dim lu = dao.GetListRorByNomAndCommune("DURAND", "", "")
        Assert.AreEqual("Paris", lu.Single(Function(r) r.Id = paris).Ville)
    End Sub

    <TestMethod()> Public Sub GetListRorByNomAndCommune_LesJokersSaisisSontLusLitteralement()
        Dim pourcent = CreerRorParcours("CABINET 100% SANTE")
        Dim chiffres = CreerRorParcours("CABINET 1000 SANTE")
        Dim souligne = CreerRorParcours("CABINET A_B")
        Dim lettres = CreerRorParcours("CABINET AXB")
        Dim tous = New Long() {pourcent, chiffres, souligne, lettres}
        Dim idsRetenus = Function(liste As List(Of Ror)) Parmi(liste.Select(Function(r) r.Id).OrderBy(Function(i) i), tous)

        CollectionAssert.AreEqual(New Long() {pourcent}, idsRetenus(dao.GetListRorByNomAndCommune("100%", "", "")))
        CollectionAssert.AreEqual(New Long() {souligne}, idsRetenus(dao.GetListRorByNomAndCommune("A_B", "", "")))
    End Sub

    <TestMethod()> Public Sub ExistProfessionnelSante_NeRetientQueLaFicheExtraiteIdentique()
        Dim creerFiche = Function(nom As String, inactif As Boolean, extraction As Boolean) CreerRorParcours(
            nom, inactif:=inactif, extractionAnnuaire:=extraction, identifiantNational:="810001234567",
            identifiantStructure:="S123", modeExercice:="L", professionId:=10, typeSavoirFaire:="S", codeSavoirFaire:="SM04")
        Dim retenu = creerFiche("EXTRAIT", False, True)
        Dim saisiALaMain = creerFiche("SAISI A LA MAIN", False, False)
        Dim desactive = creerFiche("DESACTIVE", True, True)

        Dim trouves = IdsDe(dao.ExistProfessionnelSante(" 810001234567 ", "S123", "L", 10, "S", "SM04"))

        CollectionAssert.AreEqual(New Long() {retenu}, trouves, "les identifiants sont comparés sans leurs espaces")
        CollectionAssert.DoesNotContain(trouves, saisiALaMain, "fiche non extraite de l'annuaire")
        CollectionAssert.DoesNotContain(trouves, desactive, "fiche inactive")
        Assert.AreEqual(0, dao.ExistProfessionnelSante("810001234567", "S123", "L", 10, "S", "SM05").Rows.Count, "savoir-faire différent")
        Assert.AreEqual(0, dao.ExistProfessionnelSante("810001234567", "S999", "L", 10, "S", "SM04").Rows.Count, "autre structure")
        Assert.AreEqual(0, dao.ExistProfessionnelSante("810001234567", "S123", "L", 60, "S", "SM04").Rows.Count, "autre profession")
        Assert.AreEqual(0, dao.ExistProfessionnelSante(Nothing, Nothing, Nothing, 0, Nothing, Nothing).Rows.Count, "valeurs absentes")
    End Sub

End Class
