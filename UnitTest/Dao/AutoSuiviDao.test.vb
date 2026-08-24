Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par AutoSuiviDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestAutoSuiviDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "patient_id", "parametre_id"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = AutoSuiviDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"patient_id", 101L},
            {"parametre_id", 102L}}))

        Assert.AreEqual(101L, b.PatientId)
        Assert.AreEqual(102L, b.ParametreId)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = AutoSuiviDao.BuildBean(LigneDeTest.Ligne(Colonnes, Nothing))

        Assert.AreEqual(0L, b.PatientId)
        Assert.AreEqual(0L, b.ParametreId)
    End Sub

End Class
