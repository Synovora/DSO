Imports Oasis_Common

''' <summary>
''' Noms de fichiers construits par les beans de sous-épisode. Ils doivent
''' rester acceptés par CheminsDocuments, qui est ce que le serveur vérifie.
''' </summary>
<TestClass()> Public Class TestSousEpisodeFichiers

    <TestMethod()> Public Sub LeNomDUneReponseGardeLExtensionDOrigine()
        Dim reponse As New SousEpisodeReponse With {.Id = 7, .IdSousEpisode = 34, .NomFichier = "courrier recu.PDF"}
        Assert.AreEqual("SousEpisodeReponse\Episode_12_SousEpisode_34_SousEpisodeReponse_7.PDF", reponse.GetFilenameServer(12))
        Assert.IsTrue(CheminsDocuments.EstNomDocumentValide(reponse.GetFilenameServer(12)))
    End Sub

    <TestMethod()> Public Sub LIdentifiantDeReponsePeutEtreImpose()
        Dim reponse As New SousEpisodeReponse With {.Id = 7, .IdSousEpisode = 34, .NomFichier = "x.docx"}
        Assert.AreEqual("SousEpisodeReponse\Episode_12_SousEpisode_34_SousEpisodeReponse_99.docx", reponse.GetFilenameServer(12, 99))
    End Sub

    <TestMethod()> Public Sub UnFichierSansExtensionDonneUnNomSansExtension()
        Dim reponse As New SousEpisodeReponse With {.Id = 7, .IdSousEpisode = 34, .NomFichier = "sans-extension"}
        Assert.AreEqual("SousEpisodeReponse\Episode_12_SousEpisode_34_SousEpisodeReponse_7", reponse.GetFilenameServer(12))
        Assert.IsFalse(CheminsDocuments.EstNomDocumentValide(reponse.GetFilenameServer(12)), "le serveur le refusera")
    End Sub

    <TestMethod()> Public Sub LeNomLocalDUnePieceJointeVientDuCourrielEtDeLaPartie()
        Dim piece As New SousEpisodeReponseMailAttachment With {.MailId = 5, .Part = 2, .Filename = "resultat.pdf"}
        Assert.AreEqual("Mail\5_2.pdf", piece.GetLocalName())
    End Sub

    <TestMethod()> Public Sub LesDetailsRepondentSurLeSousSousTypeEtLAld()
        Dim se As New SousEpisode With {.lstDetail = New List(Of SousEpisodeDetailSousType) From {
            New SousEpisodeDetailSousType With {.IdSousEpisodeSousSousType = 1, .IsALD = True},
            New SousEpisodeDetailSousType With {.IdSousEpisodeSousSousType = 2, .IsALD = False}}}
        Assert.IsTrue(se.IsThisSousSousTypePresent(1))
        Assert.IsTrue(se.IsThisSousSousTypePresent(2))
        Assert.IsFalse(se.IsThisSousSousTypePresent(3))
        Assert.IsTrue(se.IsThisDetailALD(1))
        Assert.IsFalse(se.IsThisDetailALD(2))
        Assert.IsFalse(se.IsThisDetailALD(3))
    End Sub

    <TestMethod()> Public Sub SansDetailRienNEstPresent()
        Dim se As New SousEpisode
        Assert.IsFalse(se.IsThisSousSousTypePresent(1))
        Assert.IsFalse(se.IsThisDetailALD(1))
    End Sub

End Class
