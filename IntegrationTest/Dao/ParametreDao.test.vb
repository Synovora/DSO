Imports Oasis_Common

''' <summary>
''' ParametreDao contre la base : référentiel des paramètres de mesure
''' (oa_r_parametre). Une dizaine d'écrans du client lourd le lisent (saisie des
''' paramètres, ligne de vie, auto-suivi) : sous oasis_client. Le portail relit
''' aussi GetParametreById (AutoSuiviController), sous oasis_web.
'''
''' GetAllParametre nomme la base en toutes lettres (oasis.oasis.oa_r_parametre) :
''' ses tests exigent une base nommée oasis. Les paramètres sont créés par
''' JeuxEpisode.CreerParametreDeMesure ; la table peut contenir d'autres lignes,
''' les assertions ne portent que sur celles du test.
''' </summary>
<TestClass()> Public Class ParametreDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New ParametreDao

    Private Shared Function IdsParmi(table As DataTable, ParamArray crees() As Long) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(ligne) CLng(ligne("id"))).Where(Function(id) crees.Contains(id)).ToArray()
    End Function

    <TestMethod()> Public Sub GetParametreById_SousClient_RelitChaqueColonne()
        Dim id = CreerParametreDeMesure("Tension systolique IT", unite:="mmHg", ordre:=4, entier:=3, nbDecimales:=0)
        Executer("UPDATE oasis.oa_r_parametre SET description_patient = @p0, valeur_min = @p1, valeur_max = @p2," &
                 " exclusion_auto_suivi = 1, aide_associee = @p3, wiki = @p4 WHERE id = @p5",
                 "Ma tension", 50D, 250D, "Mesurer au repos", "https://wiki.exemple.fr/tension", id)

        Dim lu = dao.GetParametreById(CInt(id))

        Assert.AreEqual(id, lu.Id)
        Assert.AreEqual("Tension systolique IT", lu.Description)
        Assert.AreEqual("Ma tension", lu.DescriptionPatient)
        Assert.AreEqual(3, lu.Entier)
        Assert.AreEqual(0, lu.Decimal)
        Assert.AreEqual("mmHg", lu.Unite)
        Assert.AreEqual(50D, lu.ValeurMin)
        Assert.AreEqual(250D, lu.ValeurMax)
        Assert.AreEqual(4, lu.Ordre)
        Assert.IsFalse(lu.Inactif)
        ' Comportement actuel : ExclusionAutoSuivi est une chaîne ; le booléen lu en
        ' base y arrive converti en texte. RadFAutoSuivi le compare à True, ce que
        ' la conversion implicite de VB accepte.
        Assert.AreEqual("True", lu.ExclusionAutoSuivi)
        Assert.AreEqual("Mesurer au repos", lu.AideAssociee)
        Assert.AreEqual("https://wiki.exemple.fr/tension", lu.Wiki)
    End Sub

    <TestMethod()> Public Sub GetParametreById_SousWeb_RelitLeParametre()
        UtiliserCompte(Compte.Web)
        Dim id = CreerParametreDeMesure("Glycemie IT", unite:="g/L")

        Dim lu = dao.GetParametreById(CInt(id))

        Assert.AreEqual("Glycemie IT", lu.Description)
        Assert.AreEqual("g/L", lu.Unite)
    End Sub

    <TestMethod()> Public Sub GetParametreById_ColonnesNulles_DonnentDesValeursParDefaut()
        Dim id = CreerParametreDeMesure("Parametre vide IT")
        Executer("UPDATE oasis.oa_r_parametre SET description_patient = NULL, unite = NULL," &
                 " valeur_min = NULL, valeur_max = NULL, inactif = NULL, exclusion_auto_suivi = NULL," &
                 " aide_associee = NULL, wiki = NULL WHERE id = @p0", id)

        Dim lu = dao.GetParametreById(CInt(id))

        Assert.AreEqual("", lu.DescriptionPatient)
        Assert.AreEqual("", lu.Unite)
        Assert.AreEqual(0D, lu.ValeurMin)
        Assert.AreEqual(0D, lu.ValeurMax)
        Assert.IsFalse(lu.Inactif)
        Assert.AreEqual("False", lu.ExclusionAutoSuivi)
        Assert.AreEqual("", lu.AideAssociee)
        Assert.AreEqual("", lu.Wiki)
    End Sub

    <TestMethod()> Public Sub GetParametreById_Inexistant_LeveArgumentException()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetParametreById(987654321))
        StringAssert.Contains(erreur.Message, "Paramètre inexistant")
    End Sub

    <TestMethod()> Public Sub GetAllParametre_SousClient_ExclutLesInactifsEtTrieParDescription()
        ExigerBaseOasis()
        Dim zeta = CreerParametreDeMesure("IT zeta")
        Dim alpha = CreerParametreDeMesure("IT alpha")
        Dim inactif = CreerParametreDeMesure("IT inactif")
        Dim sansEtat = CreerParametreDeMesure("IT milieu")
        Executer("UPDATE oasis.oa_r_parametre SET inactif = 1 WHERE id = @p0", inactif)
        Executer("UPDATE oasis.oa_r_parametre SET inactif = NULL WHERE id = @p0", sansEtat)

        Dim table = dao.GetAllParametre()

        ' inactif NULL compte comme actif.
        CollectionAssert.AreEqual(New Long() {alpha, sansEtat, zeta}, IdsParmi(table, zeta, alpha, inactif, sansEtat))
        Dim ligneAlpha = table.Rows.Cast(Of DataRow)().Single(Function(ligne) CLng(ligne("id")) = alpha)
        Assert.AreEqual("IT alpha", CStr(ligneAlpha("description")))
        Assert.AreEqual("u", CStr(ligneAlpha("unite")))
    End Sub

    <TestMethod()> Public Sub GetAllParametre_SousClient_TousInactifs_TableVide()
        ExigerBaseOasis()
        CreerParametreDeMesure("IT inactif seul")
        Executer("UPDATE oasis.oa_r_parametre SET inactif = 1")

        Assert.AreEqual(0, dao.GetAllParametre().Rows.Count)
    End Sub

End Class
