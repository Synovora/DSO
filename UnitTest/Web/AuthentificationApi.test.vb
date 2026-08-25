Imports System.Net.Http
Imports System.Net.Http.Headers
Imports System.Text
Imports Oasis_Common
Imports Oasis_Web.Filters

''' <summary>
''' Lecture de l'en-tête Authorization: Basic par le filtre d'authentification
''' de l'API, et identité qui en découle.
''' </summary>
<TestClass()> Public Class TestAuthentificationApi

    Private Shared Function Requete(Optional entete As AuthenticationHeaderValue = Nothing) As HttpRequestMessage
        Dim requete = New HttpRequestMessage(HttpMethod.Post, "https://serveur/api/login")
        requete.Headers.Authorization = entete
        Return requete
    End Function

    Private Shared Function Basic(texte As String) As AuthenticationHeaderValue
        Return New AuthenticationHeaderValue("Basic", Convert.ToBase64String(Encoding.UTF8.GetBytes(texte)))
    End Function

    Private Shared Function Lire(requete As HttpRequestMessage) As Tuple(Of String, String)
        Return AuthentificationApiAttribute.LireIdentifiants(requete)
    End Function

    <TestMethod()> Public Sub LoginEtMotDePasseSontLusDeLEnTeteBasic()
        Dim identifiants = Lire(Requete(Basic("dupont:secret")))
        Assert.AreEqual("dupont", identifiants.Item1)
        Assert.AreEqual("secret", identifiants.Item2)
    End Sub

    <TestMethod()> Public Sub LeMotDePassePeutContenirDesDeuxPoints()
        Dim identifiants = Lire(Requete(Basic("dupont:a:b:c")))
        Assert.AreEqual("dupont", identifiants.Item1)
        Assert.AreEqual("a:b:c", identifiants.Item2)
    End Sub

    <TestMethod()> Public Sub LesAccentsSontDecodesEnUtf8()
        Assert.AreEqual("léa", Lire(Requete(Basic("léa:motdepasse"))).Item1)
    End Sub

    <TestMethod()> Public Sub LeSchemaEstCompareSansTenirCompteDeLaCasse()
        Dim param = Convert.ToBase64String(Encoding.UTF8.GetBytes("dupont:secret"))
        Assert.AreEqual("dupont", Lire(Requete(New AuthenticationHeaderValue("basic", param))).Item1)
    End Sub

    <TestMethod()> Public Sub SansEnTeteIlNYAPasDIdentifiants()
        Assert.IsNull(Lire(Requete()))
    End Sub

    <TestMethod()> Public Sub UnAutreSchemaEstIgnore()
        Dim param = Convert.ToBase64String(Encoding.UTF8.GetBytes("dupont:secret"))
        Assert.IsNull(Lire(Requete(New AuthenticationHeaderValue("Bearer", param))))
    End Sub

    <TestMethod()> Public Sub UnParametreAbsentEstIgnore()
        Assert.IsNull(Lire(Requete(New AuthenticationHeaderValue("Basic"))))
    End Sub

    <TestMethod()> Public Sub UnBase64IllisibleEstIgnore()
        Assert.IsNull(Lire(Requete(New AuthenticationHeaderValue("Basic", "pas-du-base64!"))))
    End Sub

    <TestMethod()> Public Sub SansDeuxPointsOuSansLoginIlNYAPasDIdentifiants()
        Assert.IsNull(Lire(Requete(Basic("dupontsecret"))), "pas de séparateur")
        Assert.IsNull(Lire(Requete(Basic(":secret"))), "login vide")
    End Sub

    <TestMethod()> Public Sub UnMotDePasseVideEstTransmisTelQuel()
        Dim identifiants = Lire(Requete(Basic("dupont:")))
        Assert.AreEqual("dupont", identifiants.Item1)
        Assert.AreEqual("", identifiants.Item2)
    End Sub

    <TestMethod()> Public Sub LIdentitePorteLUtilisateurResolu()
        Dim utilisateur = New Utilisateur With {.UtilisateurLogin = "dupont"}
        Dim identite = New IdentiteUtilisateur(utilisateur)
        Assert.IsTrue(identite.IsAuthenticated)
        Assert.AreEqual("dupont", identite.Name)
        Assert.AreEqual("Basic", identite.AuthenticationType)
        Assert.AreSame(utilisateur, identite.Utilisateur)
    End Sub

    <TestMethod()> Public Sub SansUtilisateurLIdentiteEstAnonyme()
        Dim identite = New IdentiteUtilisateur(Nothing)
        Assert.IsFalse(identite.IsAuthenticated)
        Assert.AreEqual("", identite.Name)
    End Sub

    <TestMethod()> Public Sub LeFiltreNeSAppliqueQuUneFois()
        Assert.IsFalse(New AuthentificationApiAttribute().AllowMultiple)
    End Sub

End Class
