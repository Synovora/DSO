Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par ContreIndicationATCDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestContreIndicationATCDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "contre_indication_id", "patient_id", "code_atc", "Denomination_atc", "creation_user_id",
        "creation_date", "annulation_user_id", "annulation_date", "inactif"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = ContreIndicationATCDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"contre_indication_id", 101L},
            {"patient_id", 102L},
            {"code_atc", "valeur_3"},
            {"Denomination_atc", "valeur_4"},
            {"creation_user_id", 105L},
            {"creation_date", New Date(2024, 7, 7)},
            {"annulation_user_id", 107L},
            {"annulation_date", New Date(2024, 9, 9)},
            {"inactif", True}}))

        Assert.AreEqual(101L, b.ContreIndicationId)
        Assert.AreEqual(102L, b.PatientId)
        Assert.AreEqual("valeur_3", b.ATCId)
        Assert.AreEqual("valeur_4", b.DenominationATC)
        Assert.AreEqual(105L, b.UserCreation)
        Assert.AreEqual(New Date(2024, 7, 7), b.DateCreation)
        Assert.AreEqual(107L, b.UserAnnulation)
        Assert.AreEqual(New Date(2024, 9, 9), b.DateAnnulation)
        Assert.AreEqual(True, b.Inactif)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = ContreIndicationATCDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"contre_indication_id", 101L}}))

        Assert.AreEqual(0L, b.PatientId)
        Assert.AreEqual("", b.ATCId)
        Assert.AreEqual("", b.DenominationATC)
        Assert.AreEqual(0L, b.UserCreation)
        Assert.AreEqual(Date.MinValue, b.DateCreation)
        Assert.AreEqual(0L, b.UserAnnulation)
        Assert.AreEqual(Date.MinValue, b.DateAnnulation)
        Assert.AreEqual(False, b.Inactif)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansContreIndicationIdEstUneErreur()
        ' contre_indication_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"contre_indication_id", 101L}}
        valeurs.Remove("contre_indication_id")
        ContreIndicationATCDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
