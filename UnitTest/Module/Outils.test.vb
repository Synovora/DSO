Imports Oasis_Common
Imports Oasis_WF

<TestClass()> Public Class TestOutils
    Public Function GenerateUserLog() As Utilisateur
        Dim userLog = New Utilisateur
        userLog.UtilisateurId = 1337
        userLog.UtilisateurNom = "MonNom"
        userLog.UtilisateurPrenom = "MonPrenom"
        userLog.UtilisateurProfilId = "MonProfilID"
        userLog.UtilisateurRPPS = "MonRPPS"
        userLog.UtilisateurAdmin = False
        userLog.UtilisateurLogin = "MonLogin"
        userLog.UtilisateurSiegeId = 33
        userLog.UtilisateurSiteId = 22
        userLog.UtilisateurUniteSanitaireId = 11
        userLog.UtilisateurNiveauAcces = 1
        userLog.Password = "MyPassword"
        userLog.IsPasswordUniqueUsage = True
        'userLog.LstFonction As List(Of Fonction)
        userLog.TypeProfil = "TypeProfil"
        userLog.FonctionParDefautId = 0
        userLog.UtilisateurTelephone = "MonPhone"
        userLog.UtilisateurFax = "MonFax"
        userLog.UtilisateurMail = "MonMail"
        userLog.UtilisateurClePrivee = "0x00000"
        userLog.UtilisateurAddress = "MyAddress"
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

    <TestMethod()> Public Sub GetProfilUserStringTest()
        Dim userLog = GenerateUserLog()
        Assert.AreEqual("", GetProfilUserString())
    End Sub
End Class
