Imports Oasis_Common

''' <summary>
''' DrcDao contre la base de test. Toutes ses méthodes sont appelées par le client
''' lourd (écrans DRC, PPS, épisode, sélecteur) : elles tournent sous oasis_client.
''' DrcDao ne fait que lire : aucune écriture, et en particulier aucune suppression
''' dans oa_drc, sur laquelle oasis_client n'a pas DELETE.
'''
''' GetAllDrcByCategorie et GetLastDrc écrivent le nom de la base en dur
''' (oasis.oasis.oa_drc, nom en trois parties) : ils ne fonctionnent que dans une
''' base nommée « oasis ». Contre la base de test oasis_it, ces tests sont
''' déclarés non concluants ; ils tournent si OASIS_IT_DATABASE=oasis.
''' </summary>
<TestClass()> Public Class DrcDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New DrcDao

    Private Const DrcAbsente As Integer = 987654321

    Private Shared Function IdsDe(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(ligne) CLng(ligne("oa_drc_id"))).ToArray()
    End Function

    ''' <summary>Ne garde que les DRC créées par le test, dans l'ordre de la table.</summary>
    Private Shared Function Parmi(table As DataTable, ParamArray creees() As Long) As Long()
        Return IdsDe(table).Where(Function(id) creees.Contains(id)).ToArray()
    End Function

    Private Shared Sub ExigerBaseNommeeOasis()
        If Not String.Equals(NomBase, "oasis", StringComparison.OrdinalIgnoreCase) Then
            Assert.Inconclusive("La requête vise oasis.oasis.oa_drc (nom de base en dur) : elle ne peut tourner que " &
                                "dans une base nommée « oasis », la base de test s'appelle « " & NomBase & " ».")
        End If
    End Sub

    ' --- Lecture d'une DRC -------------------------------------------------------------

    <TestMethod()> Public Sub GetDrcById_RelitChaqueColonne()
        Dim id = CreerDrc("Diabète de type 2", Drc.EnumCategorieOasisCode.Objectif, CategorieMajeureDrcDeTest,
                          Drc.EnumGenreItem.Femme, aldId:=8, utilisateurId:=31)

        Dim lue = dao.GetDrcById(id)

        Assert.AreEqual(CInt(id), lue.DrcId)
        Assert.AreEqual("Diabète de type 2", lue.DrcLibelle)
        Assert.AreEqual(2, lue.DrcSexe)
        Assert.AreEqual("C", lue.DrcTypeEpisode)
        Assert.AreEqual(0, lue.DrcAgeMin)
        Assert.AreEqual(120, lue.DrcAgeMax)
        Assert.AreEqual(CategorieMajeureDrcDeTest, lue.CategorieMajeure)
        Assert.AreEqual(4, lue.CategorieOasisId)
        Assert.AreEqual("Z00", lue.CodeCim)
        Assert.AreEqual("A97", lue.CodeCisp)
        Assert.AreEqual(8, lue.AldId)
        Assert.AreEqual("ALD8", lue.AldCode)
        Assert.AreEqual("Commentaire " & id, lue.Commentaire)
        Assert.AreEqual("https://wiki.exemple.fr/drc/" & id, lue.Wiki)
        ' Comportement actuel : la réponse commentée est lue dans la colonne du type
        ' d'épisode (oa_drc_typ_epi), comme l'écran l'y écrit.
        Assert.AreEqual("C", lue.ReponseCommentee)
        Assert.AreEqual(Date.Today, lue.DateCreation.Date)
        Assert.AreEqual(31L, lue.UserCreation)
        Assert.AreEqual(Date.MinValue, lue.DateModification)
        Assert.AreEqual(0L, lue.UserModification)
    End Sub

    <TestMethod()> Public Sub GetDrcById_ColonnesNulles_ValeursParDefaut()
        Dim id = CreerDrc()
        ViderColonnesFacultativesDrc(id)

        Dim lue = dao.GetDrcById(id)

        Assert.AreEqual(CInt(id), lue.DrcId)
        Assert.AreEqual("", lue.DrcLibelle)
        Assert.AreEqual(0, lue.DrcSexe)
        Assert.AreEqual("", lue.DrcTypeEpisode)
        Assert.AreEqual(0, lue.CategorieMajeure)
        Assert.AreEqual(0, lue.CategorieOasisId)
        Assert.AreEqual("", lue.CodeCim)
        Assert.AreEqual(0, lue.AldId)
        Assert.AreEqual("", lue.Wiki)
        Assert.AreEqual(Date.MinValue, lue.DateCreation)
        Assert.AreEqual(0L, lue.UserCreation)
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentException))>
    Public Sub GetDrcById_Inexistante_LeveUneErreur()
        dao.GetDrcById(DrcAbsente)
    End Sub

    <TestMethod()> Public Sub GetDrc_RemplitLInstanceFournie()
        Dim id = CreerDrc("Asthme", Drc.EnumCategorieOasisCode.Strategie, CategorieMajeureDrcAutre, Drc.EnumGenreItem.Homme)
        Dim instance As New Drc

        Assert.IsTrue(dao.GetDrc(instance, CInt(id)))

        Assert.AreEqual(CInt(id), instance.DrcId)
        Assert.AreEqual("Asthme", instance.DrcLibelle)
        Assert.AreEqual(1, instance.DrcSexe)
        Assert.AreEqual(CategorieMajeureDrcAutre, instance.CategorieMajeure)
        Assert.AreEqual(2, instance.CategorieOasisId)
        Assert.AreEqual("C", instance.ReponseCommentee)
    End Sub

    <TestMethod()> Public Sub GetDrc_Inexistante_RenvoieVraiSansToucherLInstance()
        ' Comportement actuel : aucune ligne lue n'est pas un échec pour GetDrc.
        Dim instance As New Drc With {.DrcLibelle = "Inchangé"}

        Assert.IsTrue(dao.GetDrc(instance, DrcAbsente))

        Assert.AreEqual(0, instance.DrcId)
        Assert.AreEqual("Inchangé", instance.DrcLibelle)
    End Sub

    ' --- Correspondances code / libellé --------------------------------------------

    <TestMethod()> Public Sub GetItemGenreByCode_EtGetCodeGenreByItem_SontReciproques()
        Assert.AreEqual("Homme", dao.GetItemGenreByCode(1))
        Assert.AreEqual("Femme", dao.GetItemGenreByCode(2))
        Assert.AreEqual("Homme et femme", dao.GetItemGenreByCode(3))
        Assert.AreEqual("Homme et femme", dao.GetItemGenreByCode(0), "code inconnu : les deux")
        Assert.AreEqual(1, dao.GetCodeGenreByItem("Homme"))
        Assert.AreEqual(2, dao.GetCodeGenreByItem("Femme"))
        Assert.AreEqual(3, dao.GetCodeGenreByItem("Homme et femme"))
        Assert.AreEqual(3, dao.GetCodeGenreByItem("Autre"), "libellé inconnu : les deux")
    End Sub

    <TestMethod()> Public Sub GetItemCategorieOasisByCode_EtGetCodeCategorieOasisByItem_SontReciproques()
        For code = 1 To 8
            Dim libelle = dao.GetItemCategorieOasisByCode(code)
            Assert.AreNotEqual("Inconnue", libelle, "code " & code)
            Assert.AreEqual(code, dao.GetCodeCategorieOasisByItem(libelle), libelle)
        Next
        Assert.AreEqual("Contexte et antécédent", dao.GetItemCategorieOasisByCode(1))
        Assert.AreEqual("Procédure collaborative", dao.GetItemCategorieOasisByCode(7))
        Assert.AreEqual("Inconnue", dao.GetItemCategorieOasisByCode(0))
        Assert.AreEqual("Inconnue", dao.GetItemCategorieOasisByCode(9))
        Assert.AreEqual(0, dao.GetCodeCategorieOasisByItem("Autre"))
    End Sub

    <TestMethod()> Public Sub GetCategorieOasisByCategoriePPS_TraduitLesCategoriesDuPPS()
        Assert.AreEqual(4, dao.GetCategorieOasisByCategoriePPS(EnvironnementBase.EnumCategoriePPS.Objectif))
        Assert.AreEqual(3, dao.GetCategorieOasisByCategoriePPS(EnvironnementBase.EnumCategoriePPS.MesurePreventive))
        Assert.AreEqual(2, dao.GetCategorieOasisByCategoriePPS(EnvironnementBase.EnumCategoriePPS.Strategie))
        Assert.AreEqual(0, dao.GetCategorieOasisByCategoriePPS(EnvironnementBase.EnumCategoriePPS.Suivi), "le suivi n'a pas de DRC")
    End Sub

    ' --- Recherche dans la vue v_drc ------------------------------------------------

    <TestMethod()> Public Sub GetAllDrcByCategorieAndGenre_SansFiltre_DrcOasisDAbordPuisParId()
        Dim premiere = CreerDrc("Premiere")
        Dim horsOasis = CreerDrc("Hors Oasis", drcOasis:=False)
        Dim troisieme = CreerDrc("Troisieme")

        Dim table = dao.GetAllDrcByCategorieAndGenre("", 0, 0, False, "")

        CollectionAssert.AreEqual(New Long() {premiere, troisieme, horsOasis}, Parmi(table, premiere, horsOasis, troisieme))
        Dim ligne = table.Rows.Cast(Of DataRow)().First(Function(r) CLng(r("oa_drc_id")) = premiere)
        Assert.AreEqual("Premiere", CStr(ligne("oa_drc_libelle")))
        Assert.AreEqual(CategorieMajeureDrcDeTest, CInt(ligne("oa_drc_categorie_majeure_id")))
        Assert.AreEqual(1, CInt(ligne("oa_drc_oasis_categorie")))
    End Sub

    <TestMethod()> Public Sub GetAllDrcByCategorieAndGenre_Libelle_SansAccentNiCasse()
        Dim cherchee = CreerDrc("Hypertension artérielle")
        Dim autre = CreerDrc("Insuffisance rénale")

        Dim table = dao.GetAllDrcByCategorieAndGenre("ARTERIELLE", 0, 0, False, "")

        CollectionAssert.AreEqual(New Long() {cherchee}, Parmi(table, cherchee, autre))
    End Sub

    <TestMethod()> Public Sub GetAllDrcByCategorieAndGenre_Synonyme_TrouveLaDrc()
        Dim cherchee = CreerDrc("Hypertension artérielle")
        AjouterSynonymeDrc(cherchee, "HTA essentielle")
        Dim autre = CreerDrc("Insuffisance rénale")

        Dim table = dao.GetAllDrcByCategorieAndGenre("hta", 0, 0, False, "")

        CollectionAssert.AreEqual(New Long() {cherchee}, Parmi(table, cherchee, autre))
    End Sub

    <TestMethod()> Public Sub GetAllDrcByCategorieAndGenre_JokersSaisis_SontPrisALaLettre()
        Dim cherchee = CreerDrc("Dose A_B test")
        Dim voisine = CreerDrc("Dose AxB test")

        Dim table = dao.GetAllDrcByCategorieAndGenre("A_B", 0, 0, False, "")

        CollectionAssert.AreEqual(New Long() {cherchee}, Parmi(table, cherchee, voisine))
    End Sub

    <TestMethod()> Public Sub GetAllDrcByCategorieAndGenre_FiltresDeCategorie()
        Dim contexteA = CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.Contexte, categorieMajeureId:=CategorieMajeureDrcDeTest)
        Dim contexteB = CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.Contexte, categorieMajeureId:=CategorieMajeureDrcAutre)
        Dim objectifA = CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.Objectif, categorieMajeureId:=CategorieMajeureDrcDeTest)

        Dim parOasis = dao.GetAllDrcByCategorieAndGenre("", 0, Drc.EnumCategorieOasisCode.Contexte, False, "")
        Dim parMajeure = dao.GetAllDrcByCategorieAndGenre("", CategorieMajeureDrcDeTest, 0, False, "")
        Dim lesDeux = dao.GetAllDrcByCategorieAndGenre("", CategorieMajeureDrcDeTest, Drc.EnumCategorieOasisCode.Contexte, False, "")

        CollectionAssert.AreEqual(New Long() {contexteA, contexteB}, Parmi(parOasis, contexteA, contexteB, objectifA))
        CollectionAssert.AreEqual(New Long() {contexteA, objectifA}, Parmi(parMajeure, contexteA, contexteB, objectifA))
        CollectionAssert.AreEqual(New Long() {contexteA}, Parmi(lesDeux, contexteA, contexteB, objectifA))
    End Sub

    <TestMethod()> Public Sub GetAllDrcByCategorieAndGenre_FiltreAld_NeGardeQueLesDrcEnAld()
        Dim sansAld = CreerDrc()
        Dim enAld = CreerDrc(aldId:=8)

        Dim filtree = dao.GetAllDrcByCategorieAndGenre("", 0, 0, True, "")
        Dim complete = dao.GetAllDrcByCategorieAndGenre("", 0, 0, False, "")

        CollectionAssert.AreEqual(New Long() {enAld}, Parmi(filtree, sansAld, enAld))
        CollectionAssert.AreEqual(New Long() {sansAld, enAld}, Parmi(complete, sansAld, enAld))
    End Sub

    <TestMethod()> Public Sub GetAllDrcByCategorieAndGenre_Genre_FiltreSurLeSexeDuPatient()
        Dim homme = CreerDrc(sexe:=Drc.EnumGenreItem.Homme)
        Dim femme = CreerDrc(sexe:=Drc.EnumGenreItem.Femme)
        Dim lesDeux = CreerDrc(sexe:=Drc.EnumGenreItem.HommeEtFemme)

        Dim pourUnHomme = dao.GetAllDrcByCategorieAndGenre("", 0, 0, False, Patient.EnumGenreId.Masculin)
        Dim pourUneFemme = dao.GetAllDrcByCategorieAndGenre("", 0, 0, False, Patient.EnumGenreId.Feminin)
        Dim sansGenre = dao.GetAllDrcByCategorieAndGenre("", 0, 0, False, "")

        CollectionAssert.AreEqual(New Long() {homme, lesDeux}, Parmi(pourUnHomme, homme, femme, lesDeux))
        CollectionAssert.AreEqual(New Long() {femme, lesDeux}, Parmi(pourUneFemme, homme, femme, lesDeux))
        CollectionAssert.AreEqual(New Long() {homme, femme, lesDeux}, Parmi(sansGenre, homme, femme, lesDeux))
    End Sub

    <TestMethod()> Public Sub GetAllDrcByCategorieAndGenre_RienNeCorrespond_TableVide()
        CreerDrc("Asthme")
        Assert.AreEqual(0, dao.GetAllDrcByCategorieAndGenre("ZZQXW IT", 0, 0, False, "").Rows.Count)
    End Sub

    ' --- Recherche sur la table (nom de base en dur) --------------------------------

    <TestMethod()> Public Sub GetAllDrcByCategorie_ExclutLesDrcInvalidees()
        ExigerBaseNommeeOasis()
        Dim valide = CreerDrc("Valide")
        Dim invalidee = CreerDrc("Invalidee")
        InvaliderDrc(invalidee)

        Dim table = dao.GetAllDrcByCategorie("", 0, 0, False, "")

        CollectionAssert.AreEqual(New Long() {valide}, Parmi(table, valide, invalidee))
    End Sub

    <TestMethod()> Public Sub GetAllDrcByCategorie_UneLigneParSynonyme()
        ExigerBaseNommeeOasis()
        Dim avecSynonymes = CreerDrc("Hypertension artérielle")
        AjouterSynonymeDrc(avecSynonymes, "HTA")
        AjouterSynonymeDrc(avecSynonymes, "Tension")
        Dim sansSynonyme = CreerDrc("Asthme")

        Dim table = dao.GetAllDrcByCategorie("", 0, 0, False, "")

        ' Comportement actuel : la jointure sur oa_drc_synonyme répète la DRC autant
        ' de fois qu'elle a de synonymes.
        CollectionAssert.AreEqual(New Long() {avecSynonymes, avecSynonymes, sansSynonyme}, Parmi(table, avecSynonymes, sansSynonyme))
        Dim filtree = dao.GetAllDrcByCategorie("tension", 0, 0, False, "")
        CollectionAssert.AreEqual(New Long() {avecSynonymes, avecSynonymes}, Parmi(filtree, avecSynonymes, sansSynonyme),
                                  "le libellé correspond sur chaque ligne jointe")
    End Sub

    <TestMethod()> Public Sub GetAllDrcByCategorie_FiltresDeCategorieEtDeGenre()
        ExigerBaseNommeeOasis()
        Dim retenue = CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.Objectif, categorieMajeureId:=CategorieMajeureDrcDeTest,
                               sexe:=Drc.EnumGenreItem.Femme, aldId:=8)
        Dim autreMajeure = CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.Objectif, categorieMajeureId:=CategorieMajeureDrcAutre,
                                    sexe:=Drc.EnumGenreItem.Femme, aldId:=8)
        Dim homme = CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.Objectif, categorieMajeureId:=CategorieMajeureDrcDeTest,
                             sexe:=Drc.EnumGenreItem.Homme, aldId:=8)
        Dim sansAld = CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.Objectif, categorieMajeureId:=CategorieMajeureDrcDeTest,
                               sexe:=Drc.EnumGenreItem.Femme)

        Dim table = dao.GetAllDrcByCategorie("", CategorieMajeureDrcDeTest, Drc.EnumCategorieOasisCode.Objectif, True, Patient.EnumGenreId.Feminin)

        CollectionAssert.AreEqual(New Long() {retenue}, Parmi(table, retenue, autreMajeure, homme, sansAld))
    End Sub

    <TestMethod()> Public Sub GetLastDrc_DonneLaPlusGrandeDrcDeLaCategorie()
        ExigerBaseNommeeOasis()
        CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.Strategie)
        Dim derniere = CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.Strategie)
        CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.Prevention)

        Assert.AreEqual(derniere, dao.GetLastDrc(Drc.EnumCategorieOasisCode.Strategie))
    End Sub

    <TestMethod()> Public Sub GetLastDrc_CategorieVide_Zero()
        ExigerBaseNommeeOasis()
        Dim avant = Scalaire("SELECT MAX(oa_drc_id) FROM oasis.oa_drc WHERE oa_drc_oasis_categorie = @p0",
                             CInt(Drc.EnumCategorieOasisCode.ProtocoleAigu))
        Assert.IsTrue(IsDBNull(avant), "la base de départ ne doit pas contenir de DRC de cette catégorie")

        Assert.AreEqual(0L, dao.GetLastDrc(Drc.EnumCategorieOasisCode.ProtocoleAigu))
    End Sub

End Class
