Imports Oasis_Common

''' <summary>
''' Base64url des signatures d'ordonnance.
'''
''' Le format doit rester celui que Microsoft.IdentityModel.Tokens produisait
''' avant son retrait : une ordonnance déjà imprimée porte une URL /Sign/Check
''' et un QR code qui doivent continuer à se décoder. Les vecteurs ci-dessous
''' sont donc figés, pas recalculés depuis l'implémentation.
'''
''' /Sign/Check décode une valeur venue de n'importe qui, sans authentification.
''' Les entrées mal formées doivent lever FormatException et rien d'autre.
''' </summary>
<TestClass()> Public Class TestBase64Url

    ' Signature de 65 octets, la taille d'une signature secp256k1 recomposée.
    Private Shared ReadOnly SignatureTemoin As Byte() = OctetsDeterministes(65)

    Private Const SignatureTemoinEncodee As String = "AwoRGB8mLTQ7QklQV15lbHN6gYiPlp2kq7K5wMfO1dzj6vH4_wYNFBsiKTA3PkVMU1phaG92fYSLkpmgp661vMM"

    Private Shared Function OctetsDeterministes(longueur As Integer) As Byte()
        Dim donnees(longueur - 1) As Byte
        For i = 0 To longueur - 1
            donnees(i) = CByte((i * 7 + 3) Mod 256)
        Next
        Return donnees
    End Function

    <TestMethod()> Public Sub LesTroisLongueursDeRemplissageSEncodent()
        ' Un, deux et trois octets couvrent les trois restes possibles modulo 4,
        ' donc les trois branches du rétablissement du remplissage au décodage.
        Assert.AreEqual("_w", Base64Url.Encoder(New Byte() {255}))
        Assert.AreEqual("__4", Base64Url.Encoder(New Byte() {255, 254}))
        Assert.AreEqual("__79", Base64Url.Encoder(New Byte() {255, 254, 253}))
    End Sub

    <TestMethod()> Public Sub LesCaracteresInterditsEnUrlSontRemplaces()
        ' 251, 255, 190 donne "+/++" en base64 standard : les deux caractères
        ' à remplacer, dans la même valeur.
        Assert.AreEqual("-_--", Base64Url.Encoder(New Byte() {251, 255, 190}))
    End Sub

    <TestMethod()> Public Sub LeFormatDesSignaturesDejaImprimeesEstConserve()
        Assert.AreEqual(SignatureTemoinEncodee, Base64Url.Encoder(SignatureTemoin))
        CollectionAssert.AreEqual(SignatureTemoin, Base64Url.DecoderEnOctets(SignatureTemoinEncodee))
    End Sub

    <TestMethod()> Public Sub LAllerRetourConserveLesOctetsQuelleQueSoitLaLongueur()
        For longueur = 1 To 64
            Dim donnees = OctetsDeterministes(longueur)
            Dim encode = Base64Url.Encoder(donnees)

            Assert.IsFalse(encode.Contains("="), "remplissage laissé pour " & longueur & " octets : " & encode)
            Assert.IsFalse(encode.Contains("+"), "caractère + laissé pour " & longueur & " octets : " & encode)
            Assert.IsFalse(encode.Contains("/"), "caractère / laissé pour " & longueur & " octets : " & encode)

            CollectionAssert.AreEqual(donnees, Base64Url.DecoderEnOctets(encode),
                                      "aller-retour rompu pour " & longueur & " octets")
        Next
    End Sub

    <TestMethod()> Public Sub RienAEncoderDonneUneChaineVide()
        Assert.AreEqual("", Base64Url.Encoder(Nothing))
        Assert.AreEqual("", Base64Url.Encoder(New Byte() {}))
    End Sub

    <TestMethod()> Public Sub UneValeurVideSeDecodeEnTableauVide()
        Assert.AreEqual(0, Base64Url.DecoderEnOctets("").Length)
    End Sub

    <TestMethod()> Public Sub UneEntreeMalFormeeEstRefuseeParFormatException()
        ' Ce sont les valeurs qu'une URL /Sign/Check peut porter n'importe quand.
        For Each valeur In {"A", "ABCDE", "!!!!", "AB CD", "===="}
            Try
                Base64Url.DecoderEnOctets(valeur)
                Assert.Fail("'" & valeur & "' aurait dû être refusée.")
            Catch ex As FormatException
                ' attendu
            End Try
        Next
    End Sub

    <TestMethod()> <ExpectedException(GetType(FormatException))>
    Public Sub UneValeurAbsenteEstRefuseeParFormatException()
        Base64Url.DecoderEnOctets(Nothing)
    End Sub

    <TestMethod()> Public Sub LeBase64StandardResteDecodable()
        ' Les deux alphabets ne se recouvrent pas : accepter aussi + et / évite
        ' de casser une valeur recopiée à la main depuis un ancien enregistrement.
        CollectionAssert.AreEqual(New Byte() {251, 255, 190}, Base64Url.DecoderEnOctets("+/++"))
    End Sub

End Class
