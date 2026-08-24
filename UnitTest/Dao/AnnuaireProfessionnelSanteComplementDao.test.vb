Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par AnnuaireProfessionnelSanteComplementDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestAnnuaireProfessionnelSanteComplementDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "Cle_entree", "raison_sociale", "adresse1", "adresse2", "telephone", "telecopie",
        "email_structure"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = AnnuaireProfessionnelSanteComplementDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"Cle_entree", 101},
            {"raison_sociale", "valeur_2"},
            {"adresse1", "valeur_3"},
            {"adresse2", "valeur_4"},
            {"telephone", "valeur_5"},
            {"telecopie", "valeur_6"},
            {"email_structure", "valeur_7"}}))

        Assert.AreEqual(101, b.Cle_entree)
        Assert.AreEqual("valeur_2", b.RaisonSociale)
        Assert.AreEqual("valeur_3", b.Adresse1)
        Assert.AreEqual("valeur_4", b.Adresse2)
        Assert.AreEqual("valeur_5", b.Telephone)
        Assert.AreEqual("valeur_6", b.Telecopie)
        Assert.AreEqual("valeur_7", b.EmailStructure)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = AnnuaireProfessionnelSanteComplementDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"Cle_entree", 101}}))

        Assert.AreEqual("", b.RaisonSociale)
        Assert.AreEqual("", b.Adresse1)
        Assert.AreEqual("", b.Adresse2)
        Assert.AreEqual("", b.Telephone)
        Assert.AreEqual("", b.Telecopie)
        Assert.AreEqual("", b.EmailStructure)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansCle_entreeEstUneErreur()
        ' Cle_entree n'a pas de valeur de repli : une ligne sans cette colonne est une erreur, pas un bean à zéro.
        Dim valeurs = New Dictionary(Of String, Object) From {
            {"Cle_entree", 101}}
        valeurs.Remove("Cle_entree")
        AnnuaireProfessionnelSanteComplementDao.BuildBean(LigneDeTest.Ligne(Colonnes, valeurs))
    End Sub

End Class
