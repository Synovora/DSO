Imports Oasis_Common

''' <summary>
''' Une chaîne d'épisodes reste active tant que sa date de fin est à venir.
''' </summary>
<TestClass()> Public Class TestAntecedentChaineEpisode

    <TestMethod()> Public Sub LaChaineEstActiveTantQueSaDateDeFinEstAVenir()
        Assert.IsTrue((New Antecedent With {.ChaineEpisodeDateFin = Date.Now.AddDays(1)}).isChaineEpisodeEnable())
        Assert.IsTrue((New Antecedent With {.ChaineEpisodeDateFin = Date.MaxValue}).isChaineEpisodeEnable())
    End Sub

    <TestMethod()> Public Sub LaChaineEstInactiveUneFoisSaDateDeFinPassee()
        Assert.IsFalse((New Antecedent With {.ChaineEpisodeDateFin = Date.Now.AddDays(-1)}).isChaineEpisodeEnable())
        Assert.IsFalse((New Antecedent With {.ChaineEpisodeDateFin = Date.Now.AddMinutes(-1)}).isChaineEpisodeEnable())
    End Sub

    <TestMethod()> Public Sub SansDateDeFinLaChaineEstInactive()
        Assert.IsFalse(New Antecedent().isChaineEpisodeEnable())
    End Sub

End Class
