Imports Oasis_Common

''' <summary>
''' Profils autorisés à rédiger ou valider un sous-type de sous-épisode : la
''' liste est une chaîne "MEDICAL,PARAMEDICAL" comparée au profil de l'utilisateur.
''' </summary>
<TestClass()> Public Class TestSousEpisodeSousTypeAutorisations

    Private Shared Function Profil(typeProfil As String) As Utilisateur
        Return New Utilisateur With {.TypeProfil = typeProfil}
    End Function

    <TestMethod()> Public Sub LeProfilEstAutoriseSIlFigureDansLaListe()
        Assert.IsTrue(SousEpisodeSousType.IsUserLogAutorise("MEDICAL,PARAMEDICAL", Profil("MEDICAL")))
        Assert.IsTrue(SousEpisodeSousType.IsUserLogAutorise("MEDICAL,PARAMEDICAL", Profil("PARAMEDICAL")))
        Assert.IsFalse(SousEpisodeSousType.IsUserLogAutorise("MEDICAL,PARAMEDICAL", Profil("GESTION")))
        Assert.IsTrue(SousEpisodeSousType.IsUserLogAutorise("GESTION", Profil("GESTION")), "un seul profil, sans virgule")
    End Sub

    <TestMethod()> Public Sub LesEspacesAutourDesVirgulesSontToleres()
        Assert.IsTrue(SousEpisodeSousType.IsUserLogAutorise("MEDICAL, PARAMEDICAL", Profil("PARAMEDICAL")))
    End Sub

    <TestMethod()> Public Sub UneListeVideNAutorisePersonne()
        Assert.IsFalse(SousEpisodeSousType.IsUserLogAutorise("", Profil("MEDICAL")))
        Assert.IsFalse(SousEpisodeSousType.IsUserLogAutorise(Nothing, Profil("MEDICAL")))
    End Sub

    <TestMethod()> Public Sub LesEntreesInconnuesDeLaListeSontIgnorees()
        Assert.IsTrue(SousEpisodeSousType.IsUserLogAutorise("INCONNU,MEDICAL", Profil("MEDICAL")))
        Assert.IsFalse(SousEpisodeSousType.IsUserLogAutorise("INCONNU", Profil("MEDICAL")))
    End Sub

    <TestMethod()> Public Sub UnProfilUtilisateurInconnuOuVideEstRefuse()
        Assert.IsFalse(SousEpisodeSousType.IsUserLogAutorise("MEDICAL", Profil("PAS_UN_PROFIL")))
        Assert.IsFalse(SousEpisodeSousType.IsUserLogAutorise("MEDICAL", Profil("medical")), "la casse compte")
        Assert.IsFalse(SousEpisodeSousType.IsUserLogAutorise("MEDICAL", Profil("")))
        Assert.IsFalse(SousEpisodeSousType.IsUserLogAutorise("MEDICAL", Profil(Nothing)))
    End Sub

    <TestMethod()> Public Sub RedactionEtValidationLisentChacuneLeurListe()
        Dim sousType = New SousEpisodeSousType With {.RedactionProfilTypes = "MEDICAL", .ValidationProfilTypes = "GESTION"}
        Assert.IsTrue(sousType.IsUserLogRedactionAutorise(Profil("MEDICAL")))
        Assert.IsFalse(sousType.IsUserLogRedactionAutorise(Profil("GESTION")))
        Assert.IsTrue(sousType.IsUserLogValidationAutorise(Profil("GESTION")))
        Assert.IsFalse(sousType.IsUserLogValidationAutorise(Profil("MEDICAL")))
    End Sub

    <TestMethod()> Public Sub SansListeNiRedactionNiValidationNeSontAutorisees()
        Dim sousType = New SousEpisodeSousType
        Assert.IsFalse(sousType.IsUserLogRedactionAutorise(Profil("MEDICAL")))
        Assert.IsFalse(sousType.IsUserLogValidationAutorise(Profil("MEDICAL")))
    End Sub

End Class
