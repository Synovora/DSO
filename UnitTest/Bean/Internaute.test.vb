Imports Oasis_Common

''' <summary>
''' Expiration des liens du portail patient : durée configurée pour les liens
''' envoyés depuis le poste, date enregistrée avec la clé, validité de la clé.
''' </summary>
<TestClass()> Public Class TestInternaute

    Private Shared ReadOnly Maintenant As New Date(2026, 9, 27, 10, 0, 0)

    <TestMethod()> Public Sub SansReglageLaDureeEstDeSoixanteDouzeHeures()
        Assert.AreEqual(72, Internaute.DureeLienPosteParDefautHeures)
        Assert.AreEqual(72, Internaute.DureeLienPosteHeures(Nothing))
        Assert.AreEqual(72, Internaute.DureeLienPosteHeures(""))
        Assert.AreEqual(72, Internaute.DureeLienPosteHeures("   "))
    End Sub

    <TestMethod()> Public Sub UneDureeConfigureeEstRetenue()
        Assert.AreEqual(24, Internaute.DureeLienPosteHeures("24"))
        Assert.AreEqual(1, Internaute.DureeLienPosteHeures(" 1 "))
    End Sub

    <TestMethod()> Public Sub UneDureeInvalideDonneLaDureeParDefaut()
        For Each invalide In {"0", "-5", "abc", "12.5", "12,5", "+12", "99999999999"}
            Assert.AreEqual(72, Internaute.DureeLienPosteHeures(invalide), invalide)
        Next
    End Sub

    <TestMethod()> Public Sub UneCleSansDateRecoitLaDureeDuPoste()
        Assert.AreEqual(Maintenant.AddHours(72), Internaute.ExpirationAEnregistrer("CLE", Nothing, Maintenant, 72).Value)
        Assert.AreEqual(Maintenant.AddHours(5), Internaute.ExpirationAEnregistrer("CLE", Nothing, Maintenant, 5).Value)
    End Sub

    <TestMethod()> Public Sub UneDateDejaFixeeEstConservee()
        Dim fixee = Maintenant.AddHours(1)
        Assert.AreEqual(fixee, Internaute.ExpirationAEnregistrer("CLE", fixee, Maintenant, 72).Value)
    End Sub

    <TestMethod()> Public Sub SansCleIlNYAPasDeDate()
        Assert.IsFalse(Internaute.ExpirationAEnregistrer(Nothing, Nothing, Maintenant, 72).HasValue)
        Assert.IsFalse(Internaute.ExpirationAEnregistrer("", Nothing, Maintenant, 72).HasValue)
    End Sub

    <TestMethod()> Public Sub UneCleNonExpireeEstValide()
        Dim fiche As New Internaute With {.Recovery = "CLE", .RecoveryExpiration = Maintenant.AddMinutes(1)}
        Assert.IsTrue(fiche.CleRecuperationValide(Maintenant))
        fiche.RecoveryExpiration = Maintenant
        Assert.IsTrue(fiche.CleRecuperationValide(Maintenant), "valable jusqu'à l'échéance incluse")
    End Sub

    <TestMethod()> Public Sub UneCleExpireeEstRefusee()
        Dim fiche As New Internaute With {.Recovery = "CLE", .RecoveryExpiration = Maintenant.AddSeconds(-1)}
        Assert.IsFalse(fiche.CleRecuperationValide(Maintenant))
    End Sub

    <TestMethod()> Public Sub UneCleSansDateDExpirationEstRefusee()
        Dim fiche As New Internaute With {.Recovery = "CLE", .RecoveryExpiration = Nothing}
        Assert.IsFalse(fiche.CleRecuperationValide(Maintenant))
    End Sub

    <TestMethod()> Public Sub SansCleRienNEstValide()
        Dim sansCle As New Internaute With {.Recovery = Nothing, .RecoveryExpiration = Maintenant.AddHours(1)}
        Assert.IsFalse(sansCle.CleRecuperationValide(Maintenant))
        sansCle.Recovery = ""
        Assert.IsFalse(sansCle.CleRecuperationValide(Maintenant))
    End Sub

End Class
