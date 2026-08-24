Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par UniteSanitaireDao.buildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestUniteSanitaireDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_unite_sanitaire_id", "oa_unite_sanitaire_description", "oa_unite_sanitaire_siege_id",
        "oa_unite_sanitaire_adresse1", "oa_unite_sanitaire_adresse2", "oa_unite_sanitaire_ville",
        "oa_unite_sanitaire_code_postal", "telephone", "mail", "fax", "oa_unite_sanitaire_inactif",
        "numero_structure"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = UniteSanitaireDao.buildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_unite_sanitaire_id", 101},
            {"oa_unite_sanitaire_description", "valeur_2"},
            {"oa_unite_sanitaire_siege_id", 103},
            {"oa_unite_sanitaire_adresse1", "valeur_4"},
            {"oa_unite_sanitaire_adresse2", "valeur_5"},
            {"oa_unite_sanitaire_ville", "valeur_6"},
            {"oa_unite_sanitaire_code_postal", "valeur_7"},
            {"telephone", "valeur_8"},
            {"mail", "valeur_9"},
            {"fax", "valeur_10"},
            {"oa_unite_sanitaire_inactif", True},
            {"numero_structure", 112L}}))

        Assert.AreEqual(101, b.Oa_unite_sanitaire_id)
        Assert.AreEqual("valeur_2", b.Oa_unite_sanitaire_description)
        Assert.AreEqual(103, b.Oa_unite_sanitaire_siege_id)
        Assert.AreEqual("valeur_4", b.Oa_unite_sanitaire_adresse1)
        Assert.AreEqual("valeur_5", b.Oa_unite_sanitaire_adresse2)
        Assert.AreEqual("valeur_6", b.Oa_unite_sanitaire_ville)
        Assert.AreEqual("valeur_7", b.Oa_unite_sanitaire_code_postal)
        Assert.AreEqual("valeur_8", b.Telephone)
        Assert.AreEqual("valeur_9", b.Mail)
        Assert.AreEqual("valeur_10", b.Fax)
        Assert.AreEqual(True, b.Oa_unite_sanitaire_inactif)
        Assert.AreEqual(112L, b.NumeroStructure)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = UniteSanitaireDao.buildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_unite_sanitaire_id", 101}}))

        Assert.AreEqual("", b.Oa_unite_sanitaire_description)
        Assert.AreEqual(0, b.Oa_unite_sanitaire_siege_id)
        Assert.AreEqual("", b.Oa_unite_sanitaire_adresse1)
        Assert.AreEqual("", b.Oa_unite_sanitaire_adresse2)
        Assert.AreEqual("", b.Oa_unite_sanitaire_ville)
        Assert.AreEqual("", b.Oa_unite_sanitaire_code_postal)
        Assert.AreEqual("", b.Telephone)
        Assert.AreEqual("", b.Mail)
        Assert.AreEqual("", b.Fax)
        Assert.AreEqual(False, b.Oa_unite_sanitaire_inactif)
        Assert.AreEqual(0L, b.NumeroStructure)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansOa_unite_sanitaire_idEstUneErreur()
        ' oa_unite_sanitaire_id n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"oa_unite_sanitaire_id", 101}}
        valeurs.Remove("oa_unite_sanitaire_id")
        UniteSanitaireDao.buildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
