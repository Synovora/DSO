Imports Oasis_Common

''' <summary>
''' Un sous-épisode a un intervenant dès que son identifiant est renseigné.
''' </summary>
<TestClass()> Public Class TestSousEpisodeIntervenant

    <TestMethod()> Public Sub UnIdentifiantNonNulDesigneUnIntervenant()
        Assert.IsTrue((New SousEpisode With {.IdIntervenant = 12}).IsIntervenant())
        Assert.IsTrue((New SousEpisode With {.IdIntervenant = -1}).IsIntervenant(), "seul zéro vaut absence")
    End Sub

    <TestMethod()> Public Sub ZeroSignifieAucunIntervenant()
        Assert.IsFalse((New SousEpisode With {.IdIntervenant = 0}).IsIntervenant())
        Assert.IsFalse(New SousEpisode().IsIntervenant())
    End Sub

End Class
