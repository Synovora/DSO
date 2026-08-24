Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par SiegeDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestSiegeDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_siege_id", "oa_siege_description", "oa_siege_adresse1", "oa_siege_adresse2",
        "oa_siege_ville", "oa_siege_code_postal", "oa_siege_telephone", "oa_siege_mail",
        "oa_siege_fax", "oa_siege_statut"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = SiegeDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_siege_id", 101L},
            {"oa_siege_description", "valeur_2"},
            {"oa_siege_adresse1", "valeur_3"},
            {"oa_siege_adresse2", "valeur_4"},
            {"oa_siege_ville", "valeur_5"},
            {"oa_siege_code_postal", "valeur_6"},
            {"oa_siege_telephone", "valeur_7"},
            {"oa_siege_mail", "valeur_8"},
            {"oa_siege_fax", "valeur_9"},
            {"oa_siege_statut", "valeur_10"}}))

        Assert.AreEqual(101L, b.SiegeId)
        Assert.AreEqual("valeur_2", b.SiegeDescription)
        Assert.AreEqual("valeur_3", b.SiegeAdresse1)
        Assert.AreEqual("valeur_4", b.SiegeAdresse2)
        Assert.AreEqual("valeur_5", b.SiegeVille)
        Assert.AreEqual("valeur_6", b.SiegeCodePostal)
        Assert.AreEqual("valeur_7", b.SiegeTelephone)
        Assert.AreEqual("valeur_8", b.SiegeMail)
        Assert.AreEqual("valeur_9", b.SiegeFax)
        Assert.AreEqual("valeur_10", b.SiegeStatut)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = SiegeDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_siege_id", 101L}}))

        Assert.AreEqual("", b.SiegeDescription)
        Assert.AreEqual("", b.SiegeAdresse1)
        Assert.AreEqual("", b.SiegeAdresse2)
        Assert.AreEqual("", b.SiegeVille)
        Assert.AreEqual("", b.SiegeCodePostal)
        Assert.AreEqual("", b.SiegeTelephone)
        Assert.AreEqual("", b.SiegeMail)
        Assert.AreEqual("", b.SiegeFax)
        ' Propriété String qui tient lieu de booléen : les appelants la comparent à True,
        ' ce qui fonctionne avec "False" et lèverait avec "". On fige donc "False".
        Assert.AreEqual("False", b.SiegeStatut)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansSiegeIdEstUneErreur()
        ' oa_siege_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_siege_id", 101L}}
        valeurs.Remove("oa_siege_id")
        SiegeDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
