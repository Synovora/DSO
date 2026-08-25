Imports System.IO
Imports Oasis_Common

''' <summary>
''' Fichiers reçus de l'extérieur : seuls des types de documents connus sont
''' ouvrables, et le nom écrit sur le disque est choisi par l'application, jamais
''' par l'expéditeur.
''' </summary>
<TestClass()> Public Class TestFichiersRecus

    Private ReadOnly ecrits As New List(Of String)

    <TestCleanup()> Public Sub Nettoyer()
        For Each chemin In ecrits
            If File.Exists(chemin) Then File.Delete(chemin)
        Next
    End Sub

    Private Function Ecrire(nomOrigine As String, Optional contenu As Byte() = Nothing) As String
        Dim chemin = FichiersRecus.EcrireDansCache(nomOrigine, If(contenu, New Byte() {1, 2, 3}))
        ecrits.Add(chemin)
        Return chemin
    End Function

    <TestMethod()> Public Sub LesDocumentsCourantsSontAutorises()
        For Each nom In {"a.pdf", "a.docx", "a.doc", "a.odt", "a.rtf", "a.txt", "a.csv", "a.xlsx", "a.xls", "a.pptx", "a.ppt", "a.jpg", "a.jpeg", "a.png", "a.gif", "a.bmp", "a.tif", "a.tiff", "a.eml"}
            Assert.IsTrue(FichiersRecus.EstExtensionAutorisee(nom), nom)
        Next
    End Sub

    <TestMethod()> Public Sub LaCasseDeLExtensionEstIgnoree()
        Assert.IsTrue(FichiersRecus.EstExtensionAutorisee("Rapport.PDF"))
        Assert.IsTrue(FichiersRecus.EstExtensionAutorisee("photo.Jpg"))
    End Sub

    <TestMethod()> Public Sub LesTypesExecutablesSontRefuses()
        For Each nom In {"a.exe", "a.bat", "a.cmd", "a.js", "a.vbs", "a.hta", "a.lnk", "a.scr", "a.ps1", "a.msi", "a.docm", "a.html", "a.zip"}
            Assert.IsFalse(FichiersRecus.EstExtensionAutorisee(nom), nom)
        Next
    End Sub

    <TestMethod()> Public Sub SeuleLaDerniereExtensionCompte()
        Assert.IsFalse(FichiersRecus.EstExtensionAutorisee("facture.pdf.exe"), "double extension")
        Assert.IsTrue(FichiersRecus.EstExtensionAutorisee("setup.exe.pdf"))
    End Sub

    <TestMethod()> Public Sub SansExtensionOuSansNomRienNEstAutorise()
        Assert.IsFalse(FichiersRecus.EstExtensionAutorisee("README"))
        Assert.IsFalse(FichiersRecus.EstExtensionAutorisee("dossier."))
        Assert.IsFalse(FichiersRecus.EstExtensionAutorisee(""))
        Assert.IsFalse(FichiersRecus.EstExtensionAutorisee("   "))
        Assert.IsFalse(FichiersRecus.EstExtensionAutorisee(Nothing))
    End Sub

    <TestMethod()> Public Sub LeDossierDeCacheEstPropreALUtilisateurEtExiste()
        Dim dossier = FichiersRecus.DossierCache()
        Assert.IsTrue(Directory.Exists(dossier))
        Assert.IsTrue(dossier.StartsWith(Environment.GetFolderPath(Environment.SpecialFolder.LocalApplicationData)), dossier)
    End Sub

    <TestMethod()> Public Sub LeContenuEstEcritSousUnNomChoisiParLApplication()
        Dim contenu = New Byte() {10, 20, 30, 40}
        Dim chemin = Ecrire("Courrier du docteur.pdf", contenu)

        Assert.IsTrue(File.Exists(chemin))
        CollectionAssert.AreEqual(contenu, File.ReadAllBytes(chemin))
        Assert.AreEqual(FichiersRecus.DossierCache(), Path.GetDirectoryName(chemin))
        Assert.AreEqual(".pdf", Path.GetExtension(chemin))
        Assert.AreEqual(32, Path.GetFileNameWithoutExtension(chemin).Length, "un Guid sans tirets")
        Assert.IsFalse(chemin.Contains("Courrier"), "le nom d'origine n'apparaît pas")
    End Sub

    <TestMethod()> Public Sub LExtensionEcriteEstEnMinuscules()
        Assert.AreEqual(".docx", Path.GetExtension(Ecrire("LETTRE.DOCX")))
    End Sub

    <TestMethod()> Public Sub UnNomDOrigineTraversantNeSortPasDuCache()
        Dim chemin = Ecrire("..\..\..\Windows\evil.pdf")
        Assert.AreEqual(FichiersRecus.DossierCache(), Path.GetDirectoryName(chemin))
        Assert.IsFalse(chemin.Contains(".."))
    End Sub

    <TestMethod()> Public Sub DeuxEcrituresDuMemeNomDonnentDeuxFichiers()
        Assert.AreNotEqual(Ecrire("a.txt"), Ecrire("a.txt"))
    End Sub

    <TestMethod()> Public Sub UnTypeRefuseNEstPasEcrit()
        Dim avant = Directory.GetFiles(FichiersRecus.DossierCache()).Length
        Try
            FichiersRecus.EcrireDansCache("virus.exe", New Byte() {1})
            Assert.Fail("aucune exception")
        Catch ex As NotSupportedException
            StringAssert.Contains(ex.Message, ".exe")
        End Try
        Assert.AreEqual(avant, Directory.GetFiles(FichiersRecus.DossierCache()).Length)
    End Sub

    <TestMethod()> Public Sub UnNomAbsentEstRefuseSansPlanter()
        Try
            FichiersRecus.EcrireDansCache(Nothing, New Byte() {1})
            Assert.Fail("aucune exception")
        Catch ex As NotSupportedException
        End Try
    End Sub

    <TestMethod()> Public Sub LaPurgeSupprimeLesFichiersAnciensEtGardeLesRecents()
        Dim ancien = Ecrire("ancien.txt")
        Dim recent = Ecrire("recent.txt")
        File.SetLastWriteTime(ancien, DateTime.Now.AddHours(-5))

        FichiersRecus.PurgerCache(ageMaxiHeures:=2)

        Assert.IsFalse(File.Exists(ancien), "l'ancien est purgé")
        Assert.IsTrue(File.Exists(recent), "le récent reste")
    End Sub

    <TestMethod()> Public Sub SansAgeLaPurgeVideToutLeCache()
        Ecrire("a.txt")
        Ecrire("b.pdf")
        FichiersRecus.PurgerCache()
        Assert.AreEqual(0, Directory.GetFiles(FichiersRecus.DossierCache()).Length)
    End Sub

End Class
