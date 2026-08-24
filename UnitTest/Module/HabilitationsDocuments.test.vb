Imports Oasis_Common

''' <summary>
''' Habilitations sur les documents. Les tests portent sur ce qui se décide sans
''' la base : la forme du nom demandé et le profil de l'appelant. La partie qui
''' vérifie l'existence de l'épisode a besoin de SQL Server et n'est pas couverte
''' ici.
''' </summary>
<TestClass()> Public Class TestHabilitationsDocuments

    Private Shared Function Compte(profil As String, Optional admin As Boolean = False) As Utilisateur
        Return New Utilisateur With {.UtilisateurId = 1, .TypeProfil = profil, .UtilisateurAdmin = admin}
    End Function

    <TestMethod()> Public Sub UnModeleNEstRattacheAAucunPatient()
        Dim demande = HabilitationsDocuments.ResoudreDocument("Templates\SousEpisodeType_1_SousType_2.DOCX")
        Assert.IsTrue(demande.EstModele)
        Assert.AreEqual(0L, demande.PatientId)
        Assert.AreEqual(0L, demande.EpisodeId)
        Assert.AreEqual("Templates\SousEpisodeType_1_SousType_2.DOCX", demande.Nom)
    End Sub

    <TestMethod()> Public Sub UnNomQuiNeDesigneRienEstRefuse()
        For Each nom In {"",
                         "n-importe-quoi.DOCX",
                         "SousEpisode\Episode_x_SousEpisode_2_SousEpisodeSousType_3.DOCX",
                         "Templates\..\SousEpisode\Episode_1_SousEpisode_2_SousEpisodeSousType_3.DOCX",
                         "..\..\web.config"}
            Try
                HabilitationsDocuments.ResoudreDocument(nom)
                Assert.Fail("nom résolu à tort : " & nom)
            Catch ex As UnauthorizedAccessException
                ' attendu
            End Try
        Next
    End Sub

    <TestMethod()> Public Sub LesProfilsSoignantsAccedentAuxDocuments()
        Assert.IsTrue(HabilitationsDocuments.PeutAccederAuxDocuments(Compte("MEDICAL")))
        Assert.IsTrue(HabilitationsDocuments.PeutAccederAuxDocuments(Compte("PARAMEDICAL")))
        Assert.IsTrue(HabilitationsDocuments.PeutAccederAuxDocuments(Compte("ACCUEIL")))
    End Sub

    <TestMethod()> Public Sub LesProfilsDeGestionNAccedentPasAuxDocuments()
        ' Un compte de gestion n'a pas à lire de compte rendu clinique.
        Assert.IsFalse(HabilitationsDocuments.PeutAccederAuxDocuments(Compte("GESTION")))
        Assert.IsFalse(HabilitationsDocuments.PeutAccederAuxDocuments(Compte("PATIENT")))
    End Sub

    <TestMethod()> Public Sub UnProfilInconnuEstRefuse()
        ' Le défaut est le refus : un profil ajouté en base sans passer par ici
        ' ne doit pas obtenir l'accès par inadvertance.
        Assert.IsFalse(HabilitationsDocuments.PeutAccederAuxDocuments(Compte("")))
        Assert.IsFalse(HabilitationsDocuments.PeutAccederAuxDocuments(Compte(Nothing)))
        Assert.IsFalse(HabilitationsDocuments.PeutAccederAuxDocuments(Compte("MEDECIN")))
        Assert.IsFalse(HabilitationsDocuments.PeutAccederAuxDocuments(Compte("medical")))
    End Sub

    <TestMethod()> Public Sub UnAdministrateurPasseQuelQueSoitSonProfil()
        Assert.IsTrue(HabilitationsDocuments.PeutAccederAuxDocuments(Compte("GESTION", admin:=True)))
    End Sub

    <TestMethod()> Public Sub SansUtilisateurAucunAcces()
        Assert.IsFalse(HabilitationsDocuments.PeutAccederAuxDocuments(Nothing))
        Assert.IsFalse(HabilitationsDocuments.PeutAccederAuPatient(Nothing, 1))
    End Sub

    <TestMethod()> Public Sub UnPatientNonIdentifieEstRefuse()
        Assert.IsFalse(HabilitationsDocuments.PeutAccederAuPatient(Compte("MEDICAL"), 0))
        Assert.IsFalse(HabilitationsDocuments.PeutAccederAuPatient(Compte("MEDICAL"), -1))
        Assert.IsTrue(HabilitationsDocuments.PeutAccederAuPatient(Compte("MEDICAL"), 1))
    End Sub

End Class
