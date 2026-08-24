Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par AllergieDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestAllergieDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "allergie_id", "patient_id", "substance_id", "substance_pere_id", "denomination_substance",
        "denomination_substance_pere", "creation_user_id", "creation_date", "annulation_user_id",
        "annulation_date", "inactif"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = AllergieDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"allergie_id", 101L},
            {"patient_id", 102L},
            {"substance_id", 103L},
            {"substance_pere_id", 104L},
            {"denomination_substance", "valeur_5"},
            {"denomination_substance_pere", "valeur_6"},
            {"creation_user_id", 107L},
            {"creation_date", New Date(2024, 9, 9)},
            {"annulation_user_id", 109L},
            {"annulation_date", New Date(2024, 11, 11)},
            {"inactif", True}}))

        Assert.AreEqual(101L, b.AllergieId)
        Assert.AreEqual(102L, b.PatientId)
        Assert.AreEqual(103L, b.SubstanceId)
        Assert.AreEqual(104L, b.SubstancePereId)
        Assert.AreEqual("valeur_5", b.DenominationSubstance)
        Assert.AreEqual("valeur_6", b.DenominationSubstancePere)
        Assert.AreEqual(107L, b.UserCreation)
        Assert.AreEqual(New Date(2024, 9, 9), b.DateCreation)
        Assert.AreEqual(109L, b.UserAnnulation)
        Assert.AreEqual(New Date(2024, 11, 11), b.DateAnnulation)
        Assert.AreEqual(True, b.Inactif)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = AllergieDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"allergie_id", 101L}}))

        Assert.AreEqual(0L, b.PatientId)
        Assert.AreEqual(0L, b.SubstanceId)
        Assert.AreEqual(0L, b.SubstancePereId)
        Assert.AreEqual("", b.DenominationSubstance)
        Assert.AreEqual("", b.DenominationSubstancePere)
        Assert.AreEqual(0L, b.UserCreation)
        Assert.AreEqual(Date.MinValue, b.DateCreation)
        Assert.AreEqual(0L, b.UserAnnulation)
        Assert.AreEqual(Date.MinValue, b.DateAnnulation)
        Assert.AreEqual(False, b.Inactif)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansAllergieIdEstUneErreur()
        ' allergie_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"allergie_id", 101L}}
        valeurs.Remove("allergie_id")
        AllergieDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
