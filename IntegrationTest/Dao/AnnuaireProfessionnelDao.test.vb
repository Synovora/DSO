Imports Oasis_Common

''' <summary>
''' AnnuaireProfessionnelDao contre la base : lecture de l'annuaire national des
''' professionnels de santé importé (ans_annuaire_professionnel_sante). Les écrans
''' de recherche RadFAnnuaireProfessionnelSelect, RadFOperatorSelect et la fiche
''' RadFAnnuaireProfessionneldetail sont dans le client lourd : tout tourne sous
''' oasis_client. Les lignes de l'annuaire viennent de JeuxStructure, en SQL brut,
''' comme les dépose l'import.
''' </summary>
<TestClass()> Public Class AnnuaireProfessionnelDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New AnnuaireProfessionnelDao

    Private Shared Function Cles(table As DataTable) As List(Of Long)
        Dim liste As New List(Of Long)
        For Each ligne As DataRow In table.Rows
            liste.Add(CLng(ligne("Cle_entree")))
        Next
        Return liste
    End Function

    Private Shared Function Rechercher(nom As String,
                                       Optional commune As String = "",
                                       Optional departement As String = "",
                                       Optional codeProfession As Integer = 0,
                                       Optional codeSavoirFaire As String = Nothing) As List(Of Long)
        Dim daoRecherche As New AnnuaireProfessionnelDao
        Return Cles(daoRecherche.GetProfessionnelSanteByNomAndCommune(codeProfession, codeSavoirFaire, nom, commune, departement))
    End Function

    ' --- GetAnnuaireProfessionnelById -------------------------------------------

    <TestMethod()> Public Sub GetAnnuaireProfessionnelById_FicheExistante_RelitLesColonnesImportees()
        Dim cle = CreerProfessionnelNational(ProfessionnelDeTest("ITANNUDURAND", identifiantNational:="810000000011"))

        Dim lu = dao.GetAnnuaireProfessionnelById(CInt(cle))

        Assert.AreEqual(CInt(cle), lu.Cle_entree)
        Assert.AreEqual(8, lu.Typeidentifiant)
        Assert.AreEqual("10000000011", lu.Identifiant)
        Assert.AreEqual("810000000011", lu.IdentifiantNational)
        Assert.AreEqual("Docteur", lu.LibelleCiviliteExercice)
        Assert.AreEqual("Monsieur", lu.LibelleCivilite)
        Assert.AreEqual("ITANNUDURAND", lu.NomExercice)
        Assert.AreEqual("Jean", lu.PrenomExercice)
        Assert.AreEqual(10, lu.CodeProfession)
        Assert.AreEqual("SM54", lu.CodeSavoirFaire)
        Assert.AreEqual("Medecine generale", lu.LibelleSavoirFaire)
        Assert.AreEqual("S0001", lu.IdentifiantTechniqueStructure)
        Assert.AreEqual("CABINET DE TEST", lu.RaisonSocialeSite)
        Assert.AreEqual("MAMOUDZOU", lu.LibelleCommuneCoordonneeStructure)
        Assert.AreEqual("97600", lu.CodePostalCoordonneeStructure)
        Assert.AreEqual("0269000033", lu.TelepcopieCoordonneeStructure)
        Assert.AreEqual("cabinet@exemple.fr", lu.emailCoordonneeStructure)
        Assert.AreEqual("", lu.CodeSectionTableauPharmacien)
    End Sub

    <TestMethod()> Public Sub GetAnnuaireProfessionnelById_CleInconnue_LeveArgumentException()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetAnnuaireProfessionnelById(987654321))
        StringAssert.Contains(erreur.Message, "Professionnel de santé inexistant")
    End Sub

    ' --- GetProfessionnelSanteByNomAndCommune -----------------------------------

    <TestMethod()> Public Sub Recherche_ParNom_RenvoieLesNomsContenantLaSaisieTriesParNom()
        Dim cleB = CreerProfessionnelNational(ProfessionnelDeTest("ITANNUMARTIN"))
        Dim cleA = CreerProfessionnelNational(ProfessionnelDeTest("ITANNUDUMARTINET"))
        CreerProfessionnelNational(ProfessionnelDeTest("ITANNUBERNARD"))

        Dim table = dao.GetProfessionnelSanteByNomAndCommune(0, Nothing, "MARTIN", "", "")

        CollectionAssert.AreEqual(New List(Of Long) From {cleA, cleB}, Cles(table), "ORDER BY nom_exercice")
        ' Colonnes utilisées par la grille de sélection.
        Dim premiere = table.Rows(0)
        Assert.AreEqual("ITANNUDUMARTINET", CStr(premiere("nom_exercice")))
        Assert.AreEqual("Jean", CStr(premiere("prenom_exercice")))
        Assert.AreEqual("DR", CStr(premiere("code_civilite_exercice")))
        Assert.AreEqual("CABINET DE TEST", CStr(premiere("raison_sociale_site")))
        Assert.AreEqual("MAMOUDZOU", CStr(premiere("libelle_commune_coord_structure")))
        Assert.AreEqual("Rue", CStr(premiere("libelle_type_voie_coord_structure")))
        Assert.AreEqual("97600 MAMOUDZOU", CStr(premiere("bureau_cedex_coord_structure")))
    End Sub

    <TestMethod()> Public Sub Recherche_ParCommune_FiltreSurLeLibelleDeLaCommune()
        Dim cleSada = CreerProfessionnelNational(ProfessionnelDeTest("ITANNUCOMMUNE", commune:="SADA"))
        CreerProfessionnelNational(ProfessionnelDeTest("ITANNUCOMMUNE", commune:="DEMBENI"))

        CollectionAssert.AreEqual(New List(Of Long) From {cleSada}, Rechercher("ITANNUCOMMUNE", commune:="ADA"))
    End Sub

    <TestMethod()> Public Sub Recherche_ParDepartement_FiltreSurLeDebutDuCodePostal()
        Dim cleMayotte = CreerProfessionnelNational(ProfessionnelDeTest("ITANNUDEPT", codePostal:="97610"))
        ' 976 figure dans ce code postal, mais pas au début.
        CreerProfessionnelNational(ProfessionnelDeTest("ITANNUDEPT", codePostal:="19760"))

        CollectionAssert.AreEqual(New List(Of Long) From {cleMayotte}, Rechercher("ITANNUDEPT", departement:="976"))
    End Sub

    <TestMethod()> Public Sub Recherche_ParProfessionEtSavoirFaire_FiltreSurLesDeux()
        Dim cleGeneraliste = CreerProfessionnelNational(ProfessionnelDeTest("ITANNUPROF", codeProfession:=10, codeSavoirFaire:="SM54"))
        CreerProfessionnelNational(ProfessionnelDeTest("ITANNUPROF", codeProfession:=10, codeSavoirFaire:="SM26"))
        CreerProfessionnelNational(ProfessionnelDeTest("ITANNUPROF", codeProfession:=60, codeSavoirFaire:="SM54"))

        CollectionAssert.AreEqual(New List(Of Long) From {cleGeneraliste},
                                  Rechercher("ITANNUPROF", codeProfession:=10, codeSavoirFaire:="SM54"))
    End Sub

    <TestMethod()> Public Sub Recherche_ProfessionSansSavoirFaire_NeFiltrePasSurLaProfession()
        ' Comportement actuel : le filtre profession n'est posé qu'avec un savoir-faire.
        ' Une profession seule (ou un savoir-faire seul) est ignorée.
        Dim cle10 = CreerProfessionnelNational(ProfessionnelDeTest("ITANNUSEULE", codeProfession:=10))
        Dim cle60 = CreerProfessionnelNational(ProfessionnelDeTest("ITANNUSEULE", codeProfession:=60))

        CollectionAssert.AreEquivalent(New List(Of Long) From {cle10, cle60},
                                       Rechercher("ITANNUSEULE", codeProfession:=10))
        CollectionAssert.AreEquivalent(New List(Of Long) From {cle10, cle60},
                                       Rechercher("ITANNUSEULE", codeSavoirFaire:="SM54"))
    End Sub

    <TestMethod()> Public Sub Recherche_CaracteresJokersSaisis_SontCherchesLitteralement()
        ' Le _ et le % saisis sont échappés (EchapperLike) : ils ne jouent pas le rôle de jokers.
        CreerProfessionnelNational(ProfessionnelDeTest("ITANNUJOKERAB"))
        Dim cleSouligne = CreerProfessionnelNational(ProfessionnelDeTest("ITANNUJOKER_B"))

        CollectionAssert.AreEqual(New List(Of Long) From {cleSouligne}, Rechercher("JOKER_B"))
        Assert.AreEqual(0, Rechercher("JOKER%B").Count)
    End Sub

    <TestMethod()> Public Sub Recherche_SansAucunCritere_RenvoieToutLAnnuaire()
        Dim cle1 = CreerProfessionnelNational(ProfessionnelDeTest("ITANNUTOUT1"))
        Dim cle2 = CreerProfessionnelNational(ProfessionnelDeTest("ITANNUTOUT2"))

        Dim trouvees = Rechercher("  ", "  ", "  ")

        CollectionAssert.Contains(trouvees, cle1)
        CollectionAssert.Contains(trouvees, cle2)
        Assert.AreEqual(CInt(Scalaire("SELECT COUNT(*) FROM oasis.ans_annuaire_professionnel_sante")), trouvees.Count)
    End Sub

    <TestMethod()> Public Sub Recherche_AucunResultat_RenvoieUneTableVide()
        CreerProfessionnelNational(ProfessionnelDeTest("ITANNUPRESENT"))

        Assert.AreEqual(0, Rechercher("ITANNUABSENT").Count)
    End Sub

    <TestMethod()> Public Sub Recherche_NomNothing_LeveNullReferenceException()
        ' Comportement actuel : les écrans passent toujours le texte d'une zone de
        ' saisie, jamais Nothing ; la méthode ne s'en protège pas.
        Assert.ThrowsException(Of NullReferenceException)(
            Sub() dao.GetProfessionnelSanteByNomAndCommune(0, Nothing, Nothing, "", ""))
    End Sub

    ' --- GetStruturesByProfessionnel --------------------------------------------

    <TestMethod()> Public Sub GetStruturesByProfessionnel_CompteLesEntreesDeReferenceParStructure()
        Const identifiant As String = "810000000022"
        Dim cleCabinet = CreerProfessionnelNational(ProfessionnelDeTest("ITANNUSTRUCT", identifiantNational:=identifiant,
                                                                         codeStructure:="S0002", raisonSociale:="B CABINET"))
        Dim cleClinique = CreerProfessionnelNational(ProfessionnelDeTest("ITANNUSTRUCT", identifiantNational:=identifiant,
                                                                          codeStructure:="S0003", raisonSociale:="A CLINIQUE"))
        CreerProfessionnelNational(ProfessionnelDeTest("ITANNUAUTRE", identifiantNational:="810000000099"))
        ' Le cabinet a déjà été retenu une fois dans l'annuaire de référence.
        Dim daoReference As New AnnuaireReferenceDao
        daoReference.CreationAnnuaireReference(dao.GetAnnuaireProfessionnelById(CInt(cleCabinet)), New Utilisateur)

        Dim table = dao.GetStruturesByProfessionnel(" " & identifiant & " ")

        CollectionAssert.AreEqual(New List(Of Long) From {cleClinique, cleCabinet}, Cles(table), "ORDER BY raison_sociale_site")
        Assert.AreEqual(0, CInt(table.Rows(0)("cnt")))
        Assert.AreEqual(1, CInt(table.Rows(1)("cnt")))
        Assert.AreEqual("S0003", CStr(table.Rows(0)("identifiant_technique_structure")))
        Assert.AreEqual(identifiant, CStr(table.Rows(1)("identifiant_national_pp")))
        Assert.AreEqual("MAMOUDZOU", CStr(table.Rows(1)("libelle_commune_coord_structure")))
    End Sub

    <TestMethod()> Public Sub GetStruturesByProfessionnel_IdentifiantInconnuOuNothing_RenvoieUneTableVide()
        CreerProfessionnelNational(ProfessionnelDeTest("ITANNUSEUL", identifiantNational:="810000000033"))

        Assert.AreEqual(0, dao.GetStruturesByProfessionnel("810000000044").Rows.Count)
        Assert.AreEqual(0, dao.GetStruturesByProfessionnel(Nothing).Rows.Count)
    End Sub

End Class
