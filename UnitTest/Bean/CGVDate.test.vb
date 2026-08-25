Imports Oasis_Common

''' <summary>
''' Arithmétique d'âge du calendrier vaccinal : un mois vaut 30 jours, une
''' année 12 mois, dans les deux sens. Les libellés sont ceux affichés dans le
''' carnet, donc figés tels quels, y compris « 1 Jours ».
''' </summary>
<TestClass()> Public Class TestCGVDate

    <TestMethod()> Public Sub SousUnMoisLeLibelleEstEnJours()
        Assert.AreEqual("0 Jours", CGVDate.DaysToDate(0))
        Assert.AreEqual("1 Jours", CGVDate.DaysToDate(1))
        Assert.AreEqual("29 Jours", CGVDate.DaysToDate(29))
    End Sub

    <TestMethod()> Public Sub EntreUnEtQuaranteMoisLeLibelleEstEnMoisEtJours()
        Assert.AreEqual("1 Mois", CGVDate.DaysToDate(30))
        Assert.AreEqual("1 Mois 1 Jours", CGVDate.DaysToDate(31))
        Assert.AreEqual("1 Mois 29 Jours", CGVDate.DaysToDate(59))
        Assert.AreEqual("2 Mois", CGVDate.DaysToDate(60))
        Assert.AreEqual("39 Mois 29 Jours", CGVDate.DaysToDate(1199))
    End Sub

    <TestMethod()> Public Sub UnMoisEntameNEstPasCompteCommeUnMoisPlein()
        ' 45 jours donnaient « 2 Mois 15 Jours » par arrondi : 75 jours.
        Assert.AreEqual("1 Mois 15 Jours", CGVDate.DaysToDate(45))
        Assert.AreEqual("3 Mois 15 Jours", CGVDate.DaysToDate(105))
        Assert.AreEqual("2 Mois 15 Jours", CGVDate.DaysToDate(75))
    End Sub

    <TestMethod()> Public Sub APartirDeQuaranteMoisLeLibelleEstEnAnsEtMois()
        Assert.AreEqual("3 Ans 4 Mois", CGVDate.DaysToDate(1200))
        ' Les jours au-delà du mois entamé ne sont plus affichés à cette échelle.
        Assert.AreEqual("3 Ans 4 Mois", CGVDate.DaysToDate(1215))
        Assert.AreEqual("4 Ans", CGVDate.DaysToDate(1440))
        Assert.AreEqual("6 Ans", CGVDate.DaysToDate(2160))
        Assert.AreEqual("11 Ans 11 Mois", CGVDate.DaysToDate(4290))
    End Sub

    <TestMethod()> Public Sub LaConversionEnJoursAdditionneLesTroisUnites()
        Assert.AreEqual(0L, CGVDate.DateToDays(0, 0, 0))
        Assert.AreEqual(5L, CGVDate.DateToDays(5, 0, 0))
        Assert.AreEqual(30L, CGVDate.DateToDays(0, 1, 0))
        Assert.AreEqual(360L, CGVDate.DateToDays(0, 0, 1))
        Assert.AreEqual(425L, CGVDate.DateToDays(5, 2, 1))
    End Sub

    <TestMethod()> Public Sub LesDeuxSensSeRepondent()
        Assert.AreEqual("3 Ans 2 Mois", CGVDate.DaysToDate(CGVDate.DateToDays(0, 2, 3)))
        Assert.AreEqual("1 Mois 15 Jours", CGVDate.DaysToDate(CGVDate.DateToDays(15, 1, 0)))
        Assert.AreEqual("11 Mois", CGVDate.DaysToDate(CGVDate.DateToDays(0, 11, 0)))
        ' Sous quarante mois, une année entière s'affiche encore en mois.
        Assert.AreEqual("12 Mois", CGVDate.DaysToDate(CGVDate.DateToDays(0, 0, 1)))
    End Sub

End Class
