Imports Oasis_Common

''' <summary>
''' Filtre de tâches par unité sanitaire et site, et constructeurs de clause
''' IN. Ces derniers concatènent des identifiants numériques dans le SQL : le
''' test vérifie qu'il n'y passe que des nombres.
''' </summary>
<TestClass()> Public Class TestStructure

    Private Shared Function Unite(id As Integer, nom As String, ParamArray sites As Site()) As UniteSanitaire
        Dim u As New UniteSanitaire With {.Oa_unite_sanitaire_id = id, .Oa_unite_sanitaire_description = nom, .LstSite = New List(Of Site)}
        For Each s In sites
            u.AddSite(s)
        Next
        Return u
    End Function

    Private Shared Function LeSite(id As Long, nom As String) As Site
        Return New Site With {.Oa_site_id = id, .Oa_site_description = nom}
    End Function

    <TestMethod()> Public Sub LeResumeListeChaqueUniteEtSesSites()
        Dim filtre As New FiltreTache
        filtre.LstUniteSanitaire.Add(Unite(1, "Nord", LeSite(10, "Lille"), LeSite(11, "Roubaix")))
        filtre.LstUniteSanitaire.Add(Unite(2, "Sud"))

        Assert.AreEqual("NORD : Lille, Roubaix" & vbCrLf & "SUD : tous les sites", filtre.ResumeFiltre())
    End Sub

    <TestMethod()> Public Sub SansUniteLeResumeEstVide()
        Assert.AreEqual("", New FiltreTache().ResumeFiltre())
    End Sub

    <TestMethod()> Public Sub TousLesSitesSontAplatis()
        Dim filtre As New FiltreTache
        filtre.LstUniteSanitaire.Add(Unite(1, "Nord", LeSite(10, "Lille"), LeSite(11, "Roubaix")))
        filtre.LstUniteSanitaire.Add(Unite(2, "Sud", LeSite(20, "Nice")))

        CollectionAssert.AreEqual({10L, 11L, 20L}, filtre.GetListAllSite().Select(Function(s) s.Oa_site_id).ToArray())
    End Sub

    <TestMethod()> Public Sub AddSiteCreeLaListeAuPremierAjout()
        Dim u As New UniteSanitaire
        Assert.IsNull(u.LstSite)
        u.AddSite(LeSite(1, "a"))
        u.AddSite(LeSite(2, "b"))
        Assert.AreEqual(2, u.LstSite.Count)
    End Sub

    <TestMethod()> Public Sub LaClauseInNeContientQueDesIdentifiants()
        Assert.AreEqual(" in ( 10,11) ", Site.GetQueryInForIds(New List(Of Site) From {LeSite(10, "x' OR 1=1 --"), LeSite(11, "y")}))
        Assert.AreEqual(" in ( 1,2) ", UniteSanitaire.GetQueryInForIds(New List(Of UniteSanitaire) From {Unite(1, "a"), Unite(2, "b")}))
        Assert.AreEqual(" in ( 7) ", Fonction.GetQueryInForIds(New List(Of Fonction) From {New Fonction With {.Id = 7, .Libelle = "x'"}}))
    End Sub

    <TestMethod()> Public Sub SansListeLaClauseEstVide()
        Assert.AreEqual("", Site.GetQueryInForIds(Nothing))
        Assert.AreEqual("", UniteSanitaire.GetQueryInForIds(Nothing))
        Assert.AreEqual("", Fonction.GetQueryInForIds(Nothing))
    End Sub

    <TestMethod()> Public Sub UneListeVideDonneUneClauseInVide()
        ' Comportement en l'état : « in ( ) » n'est pas du SQL valide. À figer
        ' pour que le jour où on le corrige, le changement soit visible.
        Assert.AreEqual(" in ( ) ", Site.GetQueryInForIds(New List(Of Site)))
    End Sub

    <TestMethod()> Public Sub UneFonctionEstPossibleSiElleEstDansLaListeDuCompte()
        Dim u As New Utilisateur With {.LstFonction = New List(Of Fonction) From {New Fonction With {.Id = 3}, New Fonction With {.Id = 4}}}
        Assert.IsTrue(u.IsFonctionIdPossible(3))
        Assert.IsFalse(u.IsFonctionIdPossible(5))
        Assert.IsFalse(New Utilisateur().IsFonctionIdPossible(3), "sans liste, rien n'est possible")
    End Sub

End Class
