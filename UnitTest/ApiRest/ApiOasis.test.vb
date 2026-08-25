Imports System.Net
Imports System.Net.Http
Imports Oasis_Common

''' <summary>
''' Client REST du poste vers le serveur. Le réseau est remplacé par
''' FauxServeur : chaque test vérifie la requête que le client aurait émise
''' (route, en-tête d'authentification, corps) et ce qu'il fait de la réponse.
''' </summary>
<TestClass()> Public Class TestApiOasis

    Private Const Domaine As String = "tests.invalid"
    Private serveur As FauxServeur
    Private api As ApiOasis

    Private Shared ReadOnly Identifiants As New LoginRequest With {.login = "jdupont", .password = "Secret1@"}

    <TestInitialize()> Public Sub Preparer()
        serveur = New FauxServeur()
        api = New ApiOasis(Domaine, serveur)
    End Sub

    <TestCleanup()> Public Sub Nettoyer()
        api.Dispose()
    End Sub

    Private Shared Function Basic(login As String, motDePasse As String) As String
        Return Convert.ToBase64String(Text.Encoding.UTF8.GetBytes(login & ":" & motDePasse))
    End Function

    <TestMethod()> Public Sub LaConnexionInterrogeLaBonneRouteEnHttps()
        serveur.CorpsReponse = FauxServeur.Json(New LoginResponse With {
            .ChaineConnexion = ModuleUtilsBase.EncryptString("Server=x;Database=y"),
            .Utilisateur = New Utilisateur With {.UtilisateurLogin = "jdupont"}})

        api.loginRest(Identifiants)

        Assert.AreEqual(HttpMethod.Post, serveur.Requete.Method)
        Assert.AreEqual("https://tests.invalid/api/login", serveur.Requete.RequestUri.ToString())
        StringAssert.Contains(serveur.Requete.Headers.Accept.ToString(), "application/json")
    End Sub

    <TestMethod()> Public Sub LesIdentifiantsVoyagentDansLEnTeteAuthorizationEtNullePartAilleurs()
        serveur.CorpsReponse = FauxServeur.Json(New LoginResponse With {.ChaineConnexion = ModuleUtilsBase.EncryptString("x")})

        api.loginRest(Identifiants)

        Dim auth = serveur.Requete.Headers.Authorization
        Assert.IsNotNull(auth)
        Assert.AreEqual("Basic", auth.Scheme)
        Assert.AreEqual(Basic("jdupont", "Secret1@"), auth.Parameter)
        ' Le corps de /api/login est vide : le mot de passe ne s'y trouve pas.
        Assert.AreEqual("", serveur.CorpsRequete)
    End Sub

    <TestMethod()> Public Sub LaChaineDeConnexionRevientDechiffree()
        serveur.CorpsReponse = FauxServeur.Json(New LoginResponse With {
            .ChaineConnexion = ModuleUtilsBase.EncryptString("Server=srv;Database=oasis;User Id=oasis_client"),
            .Utilisateur = New Utilisateur With {.UtilisateurLogin = "jdupont", .UtilisateurId = 12}})

        Dim reponse = api.loginRest(Identifiants)

        Assert.AreEqual("Server=srv;Database=oasis;User Id=oasis_client", reponse.ChaineConnexion)
        Assert.AreEqual(12, reponse.Utilisateur.UtilisateurId)
    End Sub

    <TestMethod()> Public Sub UnRefusDuServeurEstSignaleCommeUneErreurDIdentifiants()
        serveur.Statut = HttpStatusCode.Unauthorized
        Try
            api.loginRest(Identifiants)
            Assert.Fail("aurait dû lever")
        Catch ex As Exception
            Assert.AreEqual("Identifiant et/ou mot de passe erroné !", ex.Message)
        End Try
    End Sub

    <TestMethod()> Public Sub UneAutreErreurRemonteLeMotifDuServeur()
        serveur.Statut = HttpStatusCode.InternalServerError
        serveur.Motif = "Base indisponible"
        Try
            api.loginRest(Identifiants)
            Assert.Fail("aurait dû lever")
        Catch ex As Exception
            Assert.AreEqual("Base indisponible", ex.Message)
        End Try
    End Sub

    <TestMethod()> Public Sub UneReponseSansChaineDeConnexionEstRefusee()
        serveur.CorpsReponse = FauxServeur.Json(New LoginResponse With {.ChaineConnexion = ""})
        Try
            api.loginRest(Identifiants)
            Assert.Fail("aurait dû lever")
        Catch ex As Exception
            Assert.AreEqual("Réponse d'authentification inexploitable.", ex.Message)
        End Try
    End Sub

    <TestMethod()> Public Sub SansIdentifiantsAucunEnTeteAuthorization()
        serveur.CorpsReponse = New ByteArrayContent(New Byte() {1})
        api.downloadFileRest(Nothing, New DownloadRequest With {.FileName = "Episode_1.DOCX"})
        Assert.IsNull(serveur.Requete.Headers.Authorization)
    End Sub

    <TestMethod()> Public Sub LaSignatureEnvoieLaChargeEnBase64()
        serveur.CorpsReponse = FauxServeur.Json(New SignatureResponse With {.Signature = "sig", .Adresse = "0xabc"})

        Dim reponse = api.signerRest(Identifiants, New Byte() {1, 2, 3})

        Assert.AreEqual("https://tests.invalid/api/signature", serveur.Requete.RequestUri.ToString())
        StringAssert.Contains(serveur.CorpsRequete, """Charge"":""AQID""")
        Assert.AreEqual(Basic("jdupont", "Secret1@"), serveur.Requete.Headers.Authorization.Parameter)
        Assert.AreEqual("sig", reponse.Signature)
        Assert.AreEqual("0xabc", reponse.Adresse)
    End Sub

    <TestMethod()> Public Sub LaGenerationDeCleNeRendQueLAdresse()
        serveur.CorpsReponse = FauxServeur.Json(New CleSignatureResponse With {.Adresse = "0xpub"})

        Dim reponse = api.genererCleRest(Identifiants, New CleSignatureRequest With {.UtilisateurId = 7, .Remplacer = True})

        Assert.AreEqual("https://tests.invalid/api/signature/cle", serveur.Requete.RequestUri.ToString())
        StringAssert.Contains(serveur.CorpsRequete, """UtilisateurId"":7")
        StringAssert.Contains(serveur.CorpsRequete, """Remplacer"":true")
        Assert.AreEqual("0xpub", reponse.Adresse)
    End Sub

    <TestMethod()> Public Sub LeChangementDeMotDePassePasseParLeServeur()
        api.changerMotDePasseRest(Identifiants, New MotDePasseRequest With {.UtilisateurId = 7, .NouveauMotDePasse = "Nouveau1@"})

        Assert.AreEqual("https://tests.invalid/api/motdepasse", serveur.Requete.RequestUri.ToString())
        StringAssert.Contains(serveur.CorpsRequete, """NouveauMotDePasse"":""Nouveau1@""")
        StringAssert.Contains(serveur.CorpsRequete, """UtilisateurId"":7")
    End Sub

    <TestMethod()> Public Sub LeTelechargementRendLesOctetsTelsQuels()
        serveur.CorpsReponse = New ByteArrayContent(New Byte() {0, 255, 10, 13})

        Dim octets = api.downloadFileRest(Identifiants, New DownloadRequest With {.FileName = "Episode_1.DOCX"})

        Assert.AreEqual("https://tests.invalid/api/docfiledownload", serveur.Requete.RequestUri.ToString())
        StringAssert.Contains(serveur.CorpsRequete, """FileName"":""Episode_1.DOCX""")
        CollectionAssert.AreEqual(New Byte() {0, 255, 10, 13}, octets)
    End Sub

    <TestMethod()> Public Sub LeDepotEnvoieLeFichierEnMultipart()
        api.uploadFileRest(Identifiants, "Episode_1.DOCX", Text.Encoding.ASCII.GetBytes("contenu du document"))

        Assert.AreEqual("https://tests.invalid/api/docfileupload", serveur.Requete.RequestUri.ToString())
        Assert.IsInstanceOfType(serveur.Requete.Content, GetType(MultipartFormDataContent))
        StringAssert.Contains(serveur.CorpsRequete, "filekey")
        StringAssert.Contains(serveur.CorpsRequete, "Episode_1.DOCX")
        StringAssert.Contains(serveur.CorpsRequete, "contenu du document")
        ' Plus d'identifiants dans le formulaire : ils sont dans l'en-tête.
        Assert.IsFalse(serveur.CorpsRequete.Contains("Secret1@"))
    End Sub

    <TestMethod()> Public Sub LeRenommagePorteLesDeuxNoms()
        api.renameFileRest(Identifiants, New RenameRequest With {.OldName = "Episode_1.DOCX", .NewName = "Episode_2.DOCX"})

        Assert.AreEqual("https://tests.invalid/api/rename", serveur.Requete.RequestUri.ToString())
        StringAssert.Contains(serveur.CorpsRequete, """OldName"":""Episode_1.DOCX""")
        StringAssert.Contains(serveur.CorpsRequete, """NewName"":""Episode_2.DOCX""")
    End Sub

    <TestMethod()> Public Sub LEnvoiDeCourrielPorteChaqueChampEtLeDossier()
        serveur.CorpsReponse = FauxServeur.Json("ok")
        Dim courriel As New MailOasis With {
            .AliasFrom = "Oasis", .AddressTo = "a@exemple.fr", .Subject = "Objet du message", .Body = "Corps du message",
            .IsHTML = True, .Filename = "piece.txt", .Contenu = Text.Encoding.ASCII.GetBytes("piece jointe")}

        Dim resultat = api.sendMailRest(Identifiants, courriel, patientId:=42)

        Assert.AreEqual("https://tests.invalid/api/sendMail", serveur.Requete.RequestUri.ToString())
        Assert.AreEqual("ok", resultat)
        For Each attendu In {"patientId", "42", "aliasFrom", "Oasis", "adressTo", "a@exemple.fr", "subject", "Objet du message",
                             "body", "Corps du message", "isSousEpisode", "isHTML", "True", "filekey", "piece.txt", "piece jointe"}
            StringAssert.Contains(serveur.CorpsRequete, attendu, attendu)
        Next
    End Sub

    <TestMethod()> Public Sub SansPieceJointeLeCourrielNEnvoiePasDeFichier()
        serveur.CorpsReponse = FauxServeur.Json("ok")
        api.sendMailRest(Identifiants, New MailOasis With {.AliasFrom = "Oasis", .AddressTo = "a@exemple.fr", .Subject = "s", .Body = "b"})
        Assert.IsFalse(serveur.CorpsRequete.Contains("filekey"))
    End Sub

End Class
