Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par SiteDao.buildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestSiteDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_site_id", "oa_site_description", "oa_site_territoire_id", "oa_site_unite_sanitaire_id",
        "oa_site_adresse1", "oa_site_adresse2", "oa_site_ville", "oa_site_code_postal",
        "telephone", "mail", "fax", "oa_site_inactif"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = SiteDao.buildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_site_id", 101L},
            {"oa_site_description", "valeur_2"},
            {"oa_site_unite_sanitaire_id", 104L},
            {"oa_site_adresse1", "valeur_5"},
            {"oa_site_adresse2", "valeur_6"},
            {"oa_site_ville", "valeur_7"},
            {"oa_site_code_postal", "valeur_8"},
            {"telephone", "valeur_9"},
            {"mail", "valeur_10"},
            {"fax", "valeur_11"},
            {"oa_site_inactif", True}}))

        Assert.AreEqual(101L, b.Oa_site_id)
        Assert.AreEqual("valeur_2", b.Oa_site_description)
        Assert.AreEqual(104L, b.Oa_site_unite_sanitaire_id)
        Assert.AreEqual("valeur_5", b.Oa_site_adresse1)
        Assert.AreEqual("valeur_6", b.Oa_site_adresse2)
        Assert.AreEqual("valeur_7", b.Oa_site_ville)
        Assert.AreEqual("valeur_8", b.Oa_site_code_postal)
        Assert.AreEqual("valeur_9", b.Telephone)
        Assert.AreEqual("valeur_10", b.Mail)
        Assert.AreEqual("valeur_11", b.Fax)
        Assert.AreEqual(True, b.Oa_site_inactif)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = SiteDao.buildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_site_id", 101L}}))

        Assert.AreEqual("", b.Oa_site_description)
        Assert.AreEqual(0L, b.Oa_site_unite_sanitaire_id)
        Assert.AreEqual("", b.Oa_site_adresse1)
        Assert.AreEqual("", b.Oa_site_adresse2)
        Assert.AreEqual("", b.Oa_site_ville)
        Assert.AreEqual("", b.Oa_site_code_postal)
        Assert.AreEqual("", b.Telephone)
        Assert.AreEqual("", b.Mail)
        Assert.AreEqual("", b.Fax)
        Assert.AreEqual(False, b.Oa_site_inactif)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansOa_site_idEstUneErreur()
        ' oa_site_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_site_id", 101L}}
        valeurs.Remove("oa_site_id")
        SiteDao.buildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
