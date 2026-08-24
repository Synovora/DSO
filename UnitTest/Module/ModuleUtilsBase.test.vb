Imports Oasis_Common

''' <summary>
''' Compléments sur les utilitaires de ModuleUtilsBase : les cas limites que les
''' appels nominaux de ModuleUtils.test.vb ne touchent pas. Ces fonctions sont
''' appelées depuis à peu près tous les DAO, donc une erreur ici se propage
''' partout en silence, d'autant plus que Option Strict est désactivé.
''' </summary>
<TestClass()> Public Class TestUtilitairesDeBase

    <TestMethod()> Public Sub N2NRendDBNullQuandIlNYARienEtPasDeDefaut()
        ' C'est ce que les DAO passent à SqlParameter : DBNull, jamais Nothing.
        Assert.AreEqual(DBNull.Value, ModuleUtilsBase.N2N(Of String)(Nothing))
        Assert.AreEqual(DBNull.Value, ModuleUtilsBase.N2N(Of Integer)(Nothing))
    End Sub

    <TestMethod()> Public Sub N2NRendLaValeurQuandElleExiste()
        Assert.AreEqual(5, ModuleUtilsBase.N2N(Of Integer)(5))
        Assert.AreEqual("abc", ModuleUtilsBase.N2N(Of String)("abc"))
        Assert.AreEqual("", ModuleUtilsBase.N2N(Of String)(""))
    End Sub

    <TestMethod()> Public Sub N2NRendLeDefautPlutotQueDBNull()
        Assert.AreEqual("defaut", ModuleUtilsBase.N2N(Of String)(Nothing, "defaut"))
        Assert.AreEqual(0, ModuleUtilsBase.N2N(Of Integer)(Nothing, 0))
    End Sub

    <TestMethod()> Public Sub CoalesceIgnoreDBNullCommeNothing()
        ' Un SqlDataReader rend DBNull, pas Nothing : les deux doivent être sautés.
        Assert.AreEqual("valeur", ModuleUtilsBase.Coalesce(DBNull.Value, "valeur"))
        Assert.AreEqual("valeur", ModuleUtilsBase.Coalesce(Nothing, DBNull.Value, "valeur"))
        Assert.AreEqual(0, ModuleUtilsBase.Coalesce(DBNull.Value, 0))
    End Sub

    <TestMethod()> Public Sub CoalesceSansValeurUtilisableRendNothing()
        Assert.IsNull(ModuleUtilsBase.Coalesce(Nothing, DBNull.Value))
        Assert.IsNull(ModuleUtilsBase.Coalesce())
    End Sub

    <TestMethod()> Public Sub CoalesceRendLaPremiereValeurUtilisable()
        Assert.AreEqual("premier", ModuleUtilsBase.Coalesce("premier", "second"))
    End Sub

    <TestMethod()> Public Sub LUrlDuPortailEstConstruiteEnHttps()
        ' ServeurOasis vaut tests.invalid dans app.config. Les ordonnances portent
        ' cette adresse en QR code : elle doit être en https et sans barre finale.
        Assert.AreEqual("https://tests.invalid", ModuleUtilsBase.UrlPortail())
    End Sub

    <TestMethod()> Public Sub UneAdresseSansPointApresLArobaseEstRefusee()
        Assert.IsFalse(ModuleUtilsBase.IsValidEmail("test@testcom"))
        Assert.IsFalse(ModuleUtilsBase.IsValidEmail("test.nom@testcom"))
        Assert.IsFalse(ModuleUtilsBase.IsValidEmail("test@test.com."))
        Assert.IsTrue(ModuleUtilsBase.IsValidEmail("test.nom@test.com"))
    End Sub

    <TestMethod()> Public Sub UneAdresseAbsenteEstRefuseeSansLeverDException()
        Assert.IsFalse(ModuleUtilsBase.IsValidEmail(Nothing))
        Assert.IsFalse(ModuleUtilsBase.IsValidEmail(""))
        Assert.IsFalse(ModuleUtilsBase.IsValidEmail("   "))
    End Sub

    <TestMethod()> Public Sub LeChiffrementSupporteLAccentuationEtLeVide()
        For Each clair In {"", "a", "Épisode n°4 : compte rendu à relire",
                           "ligne1" & vbCrLf & "ligne2"}
            Assert.AreEqual(clair, ModuleUtilsBase.DecryptString(ModuleUtilsBase.EncryptString(clair)), "aller-retour rompu : " & clair)
        Next
    End Sub

    <TestMethod()> Public Sub DeuxChiffrementsDuMemeTexteSeDechiffrentPareillement()
        Dim clair = "chaîne de connexion"
        Assert.AreEqual(clair, ModuleUtilsBase.DecryptString(ModuleUtilsBase.EncryptString(clair)))
        Assert.AreEqual(clair, ModuleUtilsBase.DecryptString(ModuleUtilsBase.EncryptString(clair)))
    End Sub

    <TestMethod()> <ExpectedException(GetType(FormatException))>
    Public Sub UnTexteChiffreQuiNEstPasDuBase64EstRefuse()
        ' La valeur vient de /api/login : elle peut être n'importe quoi.
        ModuleUtilsBase.DecryptString("ceci n'est pas du base64 !")
    End Sub

    <TestMethod()> Public Sub EchapperLikeNeutraliseLeCrochetAvantLesAutres()
        ' L'ordre compte : échapper % avant [ produirait [[][%]] et casserait le
        ' motif. Le crochet doit être traité en premier.
        Assert.AreEqual("[[][%]]", ModuleUtilsBase.EchapperLike("[%]"))
        Assert.AreEqual("[[][_]]", ModuleUtilsBase.EchapperLike("[_]"))
        Assert.AreEqual("", ModuleUtilsBase.EchapperLike(""))
    End Sub

End Class
