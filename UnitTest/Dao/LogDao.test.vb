Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par LogDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestLogDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "description", "origine", "type_log", "user_creation", "date_creation"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = LogDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"description", "valeur_2"},
            {"origine", "valeur_3"},
            {"type_log", "valeur_4"},
            {"date_creation", New Date(2024, 7, 7)}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual("valeur_2", b.Description)
        Assert.AreEqual("valeur_3", b.Origine)
        Assert.AreEqual("valeur_4", b.TypeLog)
        Assert.AreEqual(New Date(2024, 7, 7), b.DateLog)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = LogDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L}}))

        Assert.AreEqual("", b.Description)
        Assert.AreEqual("", b.Origine)
        Assert.AreEqual("", b.TypeLog)
        Assert.AreEqual(Date.MinValue, b.DateLog)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L}}
        valeurs.Remove("id")
        LogDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
