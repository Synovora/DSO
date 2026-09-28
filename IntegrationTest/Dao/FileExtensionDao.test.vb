Imports Oasis_Common

''' <summary>
''' FileExtensionDao contre la base. Seul le portail (ResultatsController) lit les
''' extensions de fichiers : les tests tournent sous oasis_web. La table peut
''' contenir des lignes chargées par un autre script de référence : les
''' assertions ne portent que sur les lignes créées ici.
''' </summary>
<TestClass()> Public Class FileExtensionDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New FileExtensionDao

    <TestInitialize>
    Public Sub PasserSousWeb()
        UtiliserCompte(Compte.Web)
    End Sub

    <TestMethod()> Public Sub GetAllFileExtension_RenvoieChaqueExtensionAvecSaDescription()
        Dim pdf = CreerExtensionFichier(".itpdf", "Document PDF de test")
        Dim jpg = CreerExtensionFichier(".itjpg", "Image de test")

        Dim extensions = dao.GetAllFileExtension()

        Dim luePdf = extensions.Single(Function(e) e.Id = pdf)
        Assert.AreEqual(".itpdf", luePdf.Extension)
        Assert.AreEqual("Document PDF de test", luePdf.Description)
        Assert.AreEqual(".itjpg", extensions.Single(Function(e) e.Id = jpg).Extension)
    End Sub

    <TestMethod()> Public Sub GetAllFileExtension_ColonnesNulles_DonnentDesChainesVides()
        Dim id = CreerExtensionFichier(Nothing, Nothing)

        Dim lue = dao.GetAllFileExtension().Single(Function(e) e.Id = id)

        Assert.AreEqual("", lue.Extension)
        Assert.AreEqual("", lue.Description)
    End Sub

    <TestMethod()> Public Sub GetAllFileExtension_TableVide_ListeVide()
        Executer("DELETE FROM oasis.oa_r_file_extension")
        Assert.AreEqual(0, dao.GetAllFileExtension().Count)
    End Sub

    <TestMethod()> Public Sub GetFileExtensionById_RelitLExtension()
        ' Aucun appelant aujourd'hui ; même compte que la liste.
        Dim id = CreerExtensionFichier(".itdoc", "Traitement de texte")

        Dim lue = dao.GetFileExtensionById(CInt(id))

        Assert.AreEqual(id, lue.Id)
        Assert.AreEqual(".itdoc", lue.Extension)
        Assert.AreEqual("Traitement de texte", lue.Description)
    End Sub

    <TestMethod()> Public Sub GetFileExtensionById_Inexistante_LeveArgumentException()
        ' Comportement actuel : le message parle d'un « Log », copié de LogDao.
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetFileExtensionById(987654321))
        StringAssert.Contains(erreur.Message, "Log inexistante")
    End Sub

End Class
