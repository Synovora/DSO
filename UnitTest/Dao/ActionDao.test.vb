Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par ActionDao.buildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestActionDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "action_id", "patient_id", "utilisateur_id", "horodatage", "action", "fonction",
        "fonction_id"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = ActionDao.buildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"action_id", 101L},
            {"patient_id", 102L},
            {"utilisateur_id", 103L},
            {"horodatage", New Date(2024, 5, 5)},
            {"action", "valeur_5"},
            {"fonction", "valeur_6"},
            {"fonction_id", 107L}}))

        Assert.AreEqual(101L, b.ActionId)
        Assert.AreEqual(102L, b.PatientId)
        Assert.AreEqual(103L, b.UtilisateurId)
        Assert.AreEqual(New Date(2024, 5, 5), b.Horodatage)
        Assert.AreEqual("valeur_5", b.Action)
        Assert.AreEqual("valeur_6", b.Fonction)
        Assert.AreEqual(107L, b.FonctionId)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = ActionDao.buildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"action_id", 101L}}))

        Assert.AreEqual(0L, b.PatientId)
        Assert.AreEqual(0L, b.UtilisateurId)
        Assert.AreEqual(Date.MinValue, b.Horodatage)
        Assert.AreEqual("", b.Action)
        Assert.AreEqual("", b.Fonction)
        Assert.AreEqual(0L, b.FonctionId)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansActionIdEstUneErreur()
        ' action_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"action_id", 101L}}
        valeurs.Remove("action_id")
        ActionDao.buildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
