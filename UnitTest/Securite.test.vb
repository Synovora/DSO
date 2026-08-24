Imports System.Text.RegularExpressions
Imports Nethereum.Signer
Imports Nethereum.Util
Imports Oasis_Common

''' <summary>
''' Tests des mécanismes de sécurité : empreintes de mots de passe, signature des
''' ordonnances, validation des noms de fichiers reçus des clients.
''' </summary>

<TestClass()> Public Class TestMotDePasse

    <TestMethod()> Public Sub HacherPuisVerifier()
        Dim empreinte = MotDePasse.Hacher("Qwertqwer1@")
        Assert.IsTrue(MotDePasse.Verifier("Qwertqwer1@", empreinte))
        Assert.IsFalse(MotDePasse.Verifier("Qwertqwer1", empreinte))
        Assert.IsFalse(MotDePasse.Verifier("", empreinte))
    End Sub

    <TestMethod()> Public Sub LEmpreinteNeContientPasLeMotDePasse()
        Dim empreinte = MotDePasse.Hacher("Qwertqwer1@")
        StringAssert.DoesNotMatch(empreinte, New Regex(Regex.Escape("Qwertqwer1@")))
        Assert.IsTrue(MotDePasse.EstFormatPbkdf2(empreinte))
    End Sub

    <TestMethod()> Public Sub DeuxComptesDeMemeMotDePasseOntDesEmpreintesDifferentes()
        ' Le sel est propre à chaque appel : c'est ce qui manquait à l'ancien SHA-1.
        Assert.AreNotEqual(MotDePasse.Hacher("Qwertqwer1@"), MotDePasse.Hacher("Qwertqwer1@"))
    End Sub

    <TestMethod()> Public Sub LAncienneEmpreinteEstAccepteePuisMigree()
        Dim ancienne = Utilisateur.CryptePwd("jdupont", "Qwertqwer1@")
        Dim doitEtreRehache As Boolean

        Assert.IsTrue(MotDePasse.VerifierAvecMigration("Qwertqwer1@", ancienne,
                                                       Utilisateur.CryptePwd("jdupont", "Qwertqwer1@"),
                                                       doitEtreRehache))
        Assert.IsTrue(doitEtreRehache, "un compte encore en SHA-1 doit être signalé pour migration")

        Assert.IsFalse(MotDePasse.VerifierAvecMigration("mauvais", ancienne,
                                                        Utilisateur.CryptePwd("jdupont", "mauvais"),
                                                        doitEtreRehache))
    End Sub

    <TestMethod()> Public Sub UneEmpreintePbkdf2NeDemandePasDeMigration()
        Dim doitEtreRehache As Boolean
        Dim empreinte = MotDePasse.Hacher("Qwertqwer1@")

        Assert.IsTrue(MotDePasse.VerifierAvecMigration("Qwertqwer1@", empreinte, "peu importe", doitEtreRehache))
        Assert.IsFalse(doitEtreRehache)
    End Sub

    <TestMethod()> Public Sub UneEmpreinteMalFormeeEstRejetee()
        For Each valeur In {"", "PBKDF2$", "PBKDF2$abc$sel$empreinte", "pas-du-tout-une-empreinte"}
            Assert.IsFalse(MotDePasse.Verifier("Qwertqwer1@", valeur), valeur)
        Next
    End Sub

End Class

<TestClass()> Public Class TestSignatureOrdonnance

    Private Shared Function UtilisateurAvecCle() As Utilisateur
        Dim k = EthECKey.GenerateKey()
        Return New Utilisateur With {
            .UtilisateurLogin = "test",
            .UtilisateurClePrivee = "0x" & BitConverter.ToString(k.GetPrivateKeyAsBytes()).Replace("-", ""),
            .UtilisateurAddress = k.GetPublicAddress()
        }
    End Function

    Private Shared Function ChargeDeTest() As Byte()
        Return New OrdonnanceFull With {
            .Ordonnance = TestOrdonnance.GenerateOrdonnance(),
            .Details = New List(Of OrdonnanceDetail) From {TestOrdonnanceDetail.GenerateOrdonnanceDetail()}
        }.Serialize()
    End Function

    <TestMethod()> Public Sub UneSignatureValideEstReconnue()
        Dim u = UtilisateurAvecCle()
        Dim charge = ChargeDeTest()

        Dim ordonnance = New Ordonnance With {
            .Signature = u.Sign(charge),
            .SignaturePayload = charge,
            .SignatureAdresse = u.UtilisateurAddress
        }

        Assert.AreEqual(VerificationSignature.ResultatVerification.Valide,
                        VerificationSignature.Verifier(ordonnance))
    End Sub

    <TestMethod()> Public Sub UneChargeAlterreeInvalideLaSignature()
        Dim u = UtilisateurAvecCle()
        Dim charge = ChargeDeTest()
        Dim signature = u.Sign(charge)

        ' Un seul octet modifié doit suffire.
        charge(charge.Length - 1) = CByte(charge(charge.Length - 1) Xor 1)

        Dim ordonnance = New Ordonnance With {
            .Signature = signature,
            .SignaturePayload = charge,
            .SignatureAdresse = u.UtilisateurAddress
        }

        Assert.AreEqual(VerificationSignature.ResultatVerification.Invalide,
                        VerificationSignature.Verifier(ordonnance))
    End Sub

    <TestMethod()> Public Sub UnAutreSignataireInvalideLaSignature()
        Dim signataire = UtilisateurAvecCle()
        Dim autre = UtilisateurAvecCle()
        Dim charge = ChargeDeTest()

        Dim ordonnance = New Ordonnance With {
            .Signature = signataire.Sign(charge),
            .SignaturePayload = charge,
            .SignatureAdresse = autre.UtilisateurAddress
        }

        Assert.AreEqual(VerificationSignature.ResultatVerification.Invalide,
                        VerificationSignature.Verifier(ordonnance))
    End Sub

    <TestMethod()> Public Sub SansChargeConserveeLOrdonnanceEstNonVerifiable()
        ' Ordonnances signées avant la conservation de la charge : elles ne doivent
        ' jamais être présentées comme authentifiées.
        Dim ordonnance = New Ordonnance With {.Signature = "0xabcdef", .SignaturePayload = Nothing}

        Assert.AreEqual(VerificationSignature.ResultatVerification.NonVerifiable,
                        VerificationSignature.Verifier(ordonnance))
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidOperationException))>
    Public Sub SignerSansCleLeveUneErreur()
        ' Aucun crochet installé : le poste ne doit pas produire de signature.
        Utilisateur.SignataireDistant = Nothing
        Dim sansCle = New Utilisateur With {.UtilisateurLogin = "test", .UtilisateurClePrivee = ""}
        sansCle.Sign(New Byte() {1, 2, 3})
    End Sub

    <TestMethod()> Public Sub LaChargeChangeAvecLeContenu()
        Dim avant = ChargeDeTest()
        Dim full = New OrdonnanceFull With {
            .Ordonnance = TestOrdonnance.GenerateOrdonnance(),
            .Details = New List(Of OrdonnanceDetail) From {TestOrdonnanceDetail.GenerateOrdonnanceDetail()}
        }
        full.Ordonnance.Commentaire = "commentaire modifié après signature"

        CollectionAssert.AreNotEqual(avant, full.Serialize())
    End Sub

    <TestMethod()> Public Sub LesFractionsVidesNeLeventPas()
        ' BinaryWriter.Write(Nothing) levait une exception : signer une ordonnance
        ' dont les fractions n'étaient pas renseignées échouait.
        Dim detail = TestOrdonnanceDetail.GenerateOrdonnanceDetail()
        detail.FractionMatin = Nothing
        detail.FractionMidi = Nothing
        detail.FractionSoir = Nothing

        Dim full = New OrdonnanceFull With {
            .Ordonnance = TestOrdonnance.GenerateOrdonnance(),
            .Details = New List(Of OrdonnanceDetail) From {detail}
        }
        Assert.IsTrue(full.Serialize().Length > 0)
    End Sub

    <TestMethod()> <ExpectedException(GetType(IO.InvalidDataException))>
    Public Sub UneChargeTronqueeEstRejetee()
        ' Taille annoncée supérieure à la charge : ne doit pas provoquer une
        ' allocation démesurée mais une erreur nette.
        Dim charge = ChargeDeTest()
        Dim tronquee(7) As Byte
        Array.Copy(charge, tronquee, 8)
        tronquee(0) = 255 : tronquee(1) = 255 : tronquee(2) = 255 : tronquee(3) = 127
        OrdonnanceFull.Deserialize(tronquee)
    End Sub

End Class

<TestClass()> Public Class TestEmpreinteConfiguration

    <TestMethod()> Public Sub LEmpreinteSha256EstComparee()
        Dim empreinteDeOasis As String
        Using sha = Security.Cryptography.SHA256.Create()
            empreinteDeOasis = BitConverter.ToString(
                sha.ComputeHash(Text.Encoding.UTF8.GetBytes("oasis"))).Replace("-", "")
        End Using

        Assert.IsTrue(EmpreinteSha256Egale("oasis", empreinteDeOasis))
        Assert.IsTrue(EmpreinteSha256Egale("oasis", empreinteDeOasis.ToLowerInvariant()))
        Assert.IsFalse(EmpreinteSha256Egale("Oasis", empreinteDeOasis))
        Assert.IsFalse(EmpreinteSha256Egale("mauvais", empreinteDeOasis))

        ' Une empreinte absente de la configuration ne doit jamais valider.
        Assert.IsFalse(EmpreinteSha256Egale("oasis", ""))
        Assert.IsFalse(EmpreinteSha256Egale("oasis", Nothing))
        Assert.IsFalse(EmpreinteSha256Egale(Nothing, empreinteDeOasis))
    End Sub

    <TestMethod()> Public Sub EchapperLikeNeutraliseLesJokers()
        Assert.AreEqual("100[%] coton", EchapperLike("100% coton"))
        Assert.AreEqual("a[_]b", EchapperLike("a_b"))
        Assert.AreEqual("[[]abc]", EchapperLike("[abc]"))
        Assert.AreEqual("", EchapperLike(Nothing))
    End Sub

End Class

<TestClass()> Public Class TestNomDocument

    <TestMethod()> Public Sub LesNomsProduitsParLApplicationSontAcceptes()
        For Each nom In {"SousEpisode\Episode_1_SousEpisode_2_SousEpisodeSousType_3.DOCX",
                         "SousEpisodeReponse\Episode_1_SousEpisode_2_SousEpisodeReponse_3.pdf",
                         "Templates\SousEpisodeType_1_SousType_2.DOCX"}
            Assert.IsTrue(EstNomDocumentValide(nom), nom)
        Next
    End Sub

    <TestMethod()> Public Sub LaTraverseeDeRepertoireEstRefusee()
        For Each nom In {"..\web.config",
                         "../web.config",
                         "SousEpisode\..\..\web.config",
                         "C:\Windows\system.ini",
                         "\\serveur\partage\fichier.docx",
                         "web.config",
                         "SousEpisode\fichier.aspx",
                         "SousEpisode\fichier.exe",
                         "Autre\fichier.docx",
                         "",
                         Nothing}
            Assert.IsFalse(EstNomDocumentValide(nom), If(nom, "(Nothing)"))
        Next
    End Sub

    <TestMethod()> Public Sub LesSeparateursSontNormalises()
        Assert.AreEqual("SousEpisode\a.DOCX", NormaliserNomDocument("/SousEpisode/a.DOCX"))
        Assert.AreEqual("", NormaliserNomDocument(Nothing))
    End Sub

End Class

''' <summary>
''' Signature déléguée au serveur.
'''
''' Le poste ne détient plus la clé privée du prescripteur : Utilisateur.Sign
''' passe par le crochet SignataireDistant, que le client lourd branche sur
''' /api/signature. Les tests remplacent ce crochet par une signature locale,
''' ce qui vérifie le chemin sans serveur.
''' </summary>
<TestClass()> Public Class TestSignatureDeleguee

    <TestCleanup()> Public Sub Nettoyer()
        ' Le crochet est partagé par tout le processus : le laisser en place
        ' ferait passer des tests voisins pour de mauvaises raisons.
        Utilisateur.SignataireDistant = Nothing
    End Sub

    <TestMethod()> Public Sub SansCleLocaleLaSignatureEstDemandeeAuServeur()
        Dim serveur = EthECKey.GenerateKey()
        Dim adresseServeur = serveur.GetPublicAddress()
        Dim clePriveeServeur = "0x" & BitConverter.ToString(serveur.GetPrivateKeyAsBytes()).Replace("-", "")
        Dim appels = 0

        Utilisateur.SignataireDistant =
            Function(charge As Byte()) As SignatureResponse
                appels += 1
                Return New SignatureResponse With {
                    .Signature = New MessageSigner().HashAndSign(charge, clePriveeServeur),
                    .Adresse = adresseServeur
                }
            End Function

        ' Le poste porte l'adresse publique mais aucune clé privée.
        Dim poste = New Utilisateur With {
            .UtilisateurLogin = "test",
            .UtilisateurClePrivee = "",
            .UtilisateurAddress = adresseServeur
        }
        Dim payload = New Byte() {1, 2, 3, 4, 5}
        Dim ordonnance = New Ordonnance With {
            .Signature = poste.Sign(payload),
            .SignaturePayload = payload
        }
        ordonnance.SignatureAdresse = poste.UtilisateurAddress

        Assert.AreEqual(1, appels)
        Assert.AreEqual(VerificationSignature.ResultatVerification.Valide,
                        VerificationSignature.Verifier(ordonnance))
    End Sub

    <TestMethod()> Public Sub LAdresseRenvoyeeParLeServeurRemplaceCelleDuPoste()
        ' Cas d'une clé changée entre la connexion et la signature : c'est
        ' l'adresse du signataire réel qui doit être enregistrée.
        Dim serveur = EthECKey.GenerateKey()
        Dim clePriveeServeur = "0x" & BitConverter.ToString(serveur.GetPrivateKeyAsBytes()).Replace("-", "")

        Utilisateur.SignataireDistant =
            Function(charge As Byte()) As SignatureResponse
                Return New SignatureResponse With {
                    .Signature = New MessageSigner().HashAndSign(charge, clePriveeServeur),
                    .Adresse = serveur.GetPublicAddress()
                }
            End Function

        Dim poste = New Utilisateur With {
            .UtilisateurLogin = "test",
            .UtilisateurClePrivee = "",
            .UtilisateurAddress = EthECKey.GenerateKey().GetPublicAddress()
        }
        Dim payload = New Byte() {9, 9, 9}
        Dim signature = poste.Sign(payload)

        Assert.IsTrue(AddressUtil.Current.AreAddressesTheSame(serveur.GetPublicAddress(),
                                                              poste.UtilisateurAddress))
        Assert.AreEqual(VerificationSignature.ResultatVerification.Valide,
                        VerificationSignature.Verifier(New Ordonnance With {
                            .Signature = signature,
                            .SignaturePayload = payload,
                            .SignatureAdresse = poste.UtilisateurAddress
                        }))
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidOperationException))>
    Public Sub UneReponseVideDuServeurEstRefusee()
        Utilisateur.SignataireDistant =
            Function(charge As Byte()) As SignatureResponse
                Return New SignatureResponse With {.Signature = "", .Adresse = ""}
            End Function

        Dim poste = New Utilisateur With {.UtilisateurLogin = "test", .UtilisateurClePrivee = ""}
        poste.Sign(New Byte() {1})
    End Sub

    <TestMethod()> Public Sub UneCleLocaleResteUtiliseeCoteServeur()
        ' Le serveur, lui, a la clé en main : il ne doit pas appeler le crochet.
        Utilisateur.SignataireDistant =
            Function(charge As Byte()) As SignatureResponse
                Throw New InvalidOperationException("Le crochet ne doit pas etre appele.")
            End Function

        Dim k = EthECKey.GenerateKey()
        Dim serveur = New Utilisateur With {
            .UtilisateurLogin = "test",
            .UtilisateurClePrivee = "0x" & BitConverter.ToString(k.GetPrivateKeyAsBytes()).Replace("-", ""),
            .UtilisateurAddress = k.GetPublicAddress()
        }
        Dim payload = New Byte() {7, 7}

        Assert.AreEqual(VerificationSignature.ResultatVerification.Valide,
                        VerificationSignature.Verifier(New Ordonnance With {
                            .Signature = serveur.Sign(payload),
                            .SignaturePayload = payload,
                            .SignatureAdresse = serveur.UtilisateurAddress
                        }))
    End Sub

End Class
