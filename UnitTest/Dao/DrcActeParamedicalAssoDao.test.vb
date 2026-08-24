Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par DrcActeParamedicalAssoDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestDrcActeParamedicalAssoDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "drc_protocole_collaboratif_id", "drc_acte_paramedical_id"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = DrcActeParamedicalAssoDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L},
            {"drc_protocole_collaboratif_id", 102L},
            {"drc_acte_paramedical_id", 103L}}))

        Assert.AreEqual(101L, b.Id)
        Assert.AreEqual(102L, b.ProtocleCollabaratifDrcId)
        Assert.AreEqual(103L, b.ActeParamedicalDrcId)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = DrcActeParamedicalAssoDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 101L}}))

        Assert.AreEqual(0L, b.ProtocleCollabaratifDrcId)
        Assert.AreEqual(0L, b.ActeParamedicalDrcId)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdEstUneErreur()
        ' id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"id", 101L}}
        valeurs.Remove("id")
        DrcActeParamedicalAssoDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
