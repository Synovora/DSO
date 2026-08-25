Imports Oasis_Common

''' <summary>
''' Un mail porte une pièce jointe quand il a un nom de fichier et des octets.
''' </summary>
<TestClass()> Public Class TestMail

    <TestMethod()> Public Sub UnMailAUnContenuQuandFichierEtOctetsSontPresents()
        Assert.IsTrue((New Mail With {.Filename = "a.pdf", .Contenu = New Byte() {1, 2}}).IsWithContenu())
    End Sub

    <TestMethod()> Public Sub SansNomDeFichierIlNYAPasDeContenu()
        Assert.IsFalse((New Mail With {.Filename = "", .Contenu = New Byte() {1}}).IsWithContenu())
        Assert.IsFalse((New Mail With {.Filename = Nothing, .Contenu = New Byte() {1}}).IsWithContenu())
    End Sub

    <TestMethod()> Public Sub SansOctetsIlNYAPasDeContenu()
        Assert.IsFalse((New Mail With {.Filename = "a.pdf", .Contenu = Nothing}).IsWithContenu())
        Assert.IsFalse((New Mail With {.Filename = "a.pdf", .Contenu = New Byte() {}}).IsWithContenu())
        Assert.IsFalse(New Mail().IsWithContenu())
    End Sub

End Class
