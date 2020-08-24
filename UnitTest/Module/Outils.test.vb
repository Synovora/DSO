Imports Oasis_Common
Imports Oasis_WF

<TestClass()> Public Class TestOutils
    Public Function GenerateUserLog() As Utilisateur
        'userLog.LstFonction As List(Of Fonction)
        Dim userLog = New Utilisateur With {
            .UtilisateurId = 1337,
            .UtilisateurNom = "MonNom",
            .UtilisateurPrenom = "MonPrenom",
            .UtilisateurProfilId = "MonProfilID",
            .UtilisateurRPPS = "MonRPPS",
            .UtilisateurAdmin = False,
            .UtilisateurLogin = "MonLogin",
            .UtilisateurSiegeId = 33,
            .UtilisateurSiteId = 22,
            .UtilisateurUniteSanitaireId = 11,
            .UtilisateurNiveauAcces = 1,
            .Password = "MyPassword",
            .IsPasswordUniqueUsage = True,
            .TypeProfil = "TypeProfil",
            .FonctionParDefautId = 0,
            .UtilisateurTelephone = "MonPhone",
            .UtilisateurFax = "MonFax",
            .UtilisateurMail = "MonMail",
            .UtilisateurClePrivee = "0x00000",
            .UtilisateurAddress = "MyAddress"
        }
        Return userLog
    End Function

    <TestMethod()> Public Sub CalculDureeEnJourEtHeureStringTest()
        Assert.AreEqual(CalculDureeEnJourEtHeureString(New DateTime(2001, 2, 1, 0, 0, 0), New DateTime(2001, 2, 2, 1, 0, 0)), CalculDureeEnJourEtHeureString(New DateTime(2000, 1, 1, 0, 0, 0), New DateTime(2000, 1, 2, 1, 0, 0)))
    End Sub

    <TestMethod()> Public Sub CalculDureeEnJourStringTest()
        Assert.AreEqual(CalculDureeEnJourString(New DateTime(2001, 5, 1, 0, 0, 0), New DateTime(2001, 6, 1, 0, 0, 0)), CalculDureeEnJourString(New DateTime(2000, 1, 1, 0, 0, 0), New DateTime(2000, 2, 1, 0, 0, 0)))
    End Sub

    <TestMethod()> Public Sub ConvertirEnJourDureeEnMoisTest()
        Assert.AreEqual(1278, ConvertirEnJourDureeEnMois(42))
    End Sub

    <TestMethod()> Public Sub CalculAgeEnmoisTest()
        Assert.AreEqual(CalculAgeEnmois(New Date(1590363869)), CalculAgeEnmois(New Date(1591363869)))
    End Sub

    <TestMethod()> Public Sub CalculAgeEnJourTest()
        Assert.AreEqual(CalculAgeEnJour(New Date(1580363869)), CalculAgeEnJour(New Date(1591363869)))
    End Sub

    <TestMethod()> Public Sub CalculDureeTraitementEnJourTest()
        Assert.AreEqual(12, CalculDureeTraitementEnJour(New DateTime(2001, 1, 1, 0, 0, 0), New DateTime(2001, 1, 12, 0, 0, 0)))
    End Sub

    <TestMethod()> Public Sub TestCalculAgeEnJourTest()
        Assert.AreEqual(CalculAgeEnJour(New Date(1480363869)), CalculAgeEnJour(New Date(1591363869)))
    End Sub

    <TestMethod()> Public Sub CalculDureeTraitementEnJourStringTest()
        Assert.AreEqual("32 jours", CalculDureeTraitementEnJourString(New DateTime(2000, 1, 1, 0, 0, 0), New DateTime(2000, 2, 1, 0, 0, 0)))
    End Sub

    <TestMethod()> Public Sub FormatageDateAffichageTest()
        Assert.AreEqual(" 01.2000", FormatageDateAffichage(New DateTime(2000, 1, 1, 0, 0, 0), True))
    End Sub

    <TestMethod()> Public Sub CalculAgeEnAnneeEtMoisStringTest()
        Assert.AreEqual(CalculAgeEnAnneeEtMoisString(New DateTime(2000, 1, 1, 0, 0, 0)), CalculAgeEnAnneeEtMoisString(New DateTime(2000, 1, 1, 0, 0, 0)))
    End Sub

    <TestMethod()> Public Sub CalculAgeEnAnneeTest()
        Assert.AreEqual(CalculAgeEnAnnee(New DateTime(2000, 1, 1, 0, 0, 0)) - 8, CalculAgeEnAnnee(New DateTime(2008, 5, 1, 8, 30, 52)))
    End Sub

    <TestMethod()> Public Sub CalculProchainRendezVousTest()
        Assert.AreEqual(New DateTime(2000, 1, 2, 0, 0, 0), CalculProchainRendezVous(New DateTime(2000, 1, 1, 0, 0, 0), 0, ParcoursDao.EnumParcoursBaseCode.Quotidien))
    End Sub

    '<TestMethod()> Public Sub GetProfilUserStringTest()
    '    Dim userLog = GenerateUserLog()
    '    Assert.AreEqual("", GetProfilUserString(userLog))
    'End Sub
End Class
