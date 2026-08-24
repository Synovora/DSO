Imports Oasis_Common

''' <summary>
''' Résolution du chemin absolu d'un document.
'''
''' EstNomDocumentValide est couvert par TestNomDocument dans Securite.test.vb.
''' Ce qui suit porte sur ResoudreCheminDocument, qui est la fonction que l'API
''' appelle réellement avant d'ouvrir un fichier : valider le nom et résoudre le
''' chemin sont deux barrières distinctes, et c'est la seconde qui décide de ce
''' qui est ouvert. FileUploadLocation est renseigné dans app.config.
''' </summary>
<TestClass()> Public Class TestResolutionCheminDocument

    Private Const NomLegitime As String = "SousEpisode\Episode_12_SousEpisode_34_SousEpisodeSousType_5.DOCX"

    <TestMethod()> Public Sub UnNomLegitimeDonneUnCheminAbsoluSousLaZoneDeDepot()
        Dim racine = IO.Path.GetFullPath(
            Configuration.ConfigurationManager.AppSettings("FileUploadLocation")).TrimEnd("\"c)
        Dim complet = CheminsDocuments.ResoudreCheminDocument(NomLegitime)

        Assert.IsTrue(IO.Path.IsPathRooted(complet), "le chemin rendu doit être absolu : " & complet)
        StringAssert.StartsWith(complet, racine & "\")
        StringAssert.EndsWith(complet, "Episode_12_SousEpisode_34_SousEpisodeSousType_5.DOCX")
    End Sub

    <TestMethod()> Public Sub UnNomEnBarresObliquesDonneLeMemeChemin()
        ' Un client web envoie volontiers des barres obliques.
        Assert.AreEqual(CheminsDocuments.ResoudreCheminDocument(NomLegitime),
                        CheminsDocuments.ResoudreCheminDocument(NomLegitime.Replace("\"c, "/"c)))
    End Sub

    <TestMethod()> Public Sub LesTroisDossiersDeLApplicationSeResolvent()
        For Each nom In {"SousEpisode\Episode_1_SousEpisode_2_SousEpisodeSousType_3.DOCX",
                         "SousEpisodeReponse\Episode_1_SousEpisode_2_SousEpisodeReponse_3.pdf",
                         "Templates\SousEpisodeType_1_SousType_2.DOCX"}
            Assert.IsTrue(IO.Path.IsPathRooted(CheminsDocuments.ResoudreCheminDocument(nom)), nom)
        Next
    End Sub

    <TestMethod()> Public Sub AucunCheminNeSortDeLaZoneDeDepot()
        For Each nom In {"..\..\windows\win.ini",
                         "SousEpisode\..\..\secret.DOCX",
                         "SousEpisode\..\Templates\x.DOCX",
                         "SousEpisode\.\Episode_1_SousEpisode_2_SousEpisodeSousType_3.DOCX",
                         "\\serveur\partage\x.DOCX",
                         "C:\Windows\win.ini",
                         "/etc/passwd",
                         "SousEpisode\sous\Episode_1_SousEpisode_2_SousEpisodeSousType_3.DOCX"}
            AssertRefuse(nom)
        Next
    End Sub

    <TestMethod()> Public Sub UneExtensionExecutableNeSeResoutPas()
        ' La liste blanche d'extensions est ce qui empêche de déposer puis de
        ' faire servir un fichier exécutable par le serveur.
        For Each extension In {"exe", "dll", "bat", "cmd", "ps1", "vbs", "js", "aspx", "asp", "config", "docx.exe"}
            AssertRefuse("SousEpisode\Episode_1_SousEpisode_2_SousEpisodeSousType_3." & extension)
        Next
    End Sub

    <TestMethod()> Public Sub UnNomSansExtensionNeSeResoutPas()
        AssertRefuse("SousEpisode\Episode_1_SousEpisode_2_SousEpisodeSousType_3")
        AssertRefuse("SousEpisode\")
        AssertRefuse("Templates")
    End Sub

    <TestMethod()> Public Sub UnNomAbsentEstRefuse()
        AssertRefuse(Nothing)
        AssertRefuse("")
        AssertRefuse("   ")
    End Sub

    <TestMethod()> Public Sub LesEspacesEtLeSeparateurDeTeteSontRetires()
        Assert.AreEqual("SousEpisode\x.DOCX", CheminsDocuments.NormaliserNomDocument("  SousEpisode/x.DOCX  "))
        Assert.AreEqual("SousEpisode\x.DOCX", CheminsDocuments.NormaliserNomDocument("\SousEpisode\x.DOCX"))
        Assert.AreEqual("SousEpisode\x.DOCX", CheminsDocuments.NormaliserNomDocument(" /SousEpisode/x.DOCX"))
    End Sub

    Private Shared Sub AssertRefuse(nom As String)
        Try
            Dim complet = CheminsDocuments.ResoudreCheminDocument(nom)
            Assert.Fail("nom résolu à tort : " & If(nom, "(Nothing)") & " -> " & complet)
        Catch ex As ArgumentException
            ' attendu
        End Try
    End Sub

End Class
