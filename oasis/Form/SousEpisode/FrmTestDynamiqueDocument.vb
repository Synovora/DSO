Imports System.IO
Imports Telerik.WinControls.UI
Imports Telerik.WinForms.Documents
Imports Telerik.WinForms.Documents.FormatProviders.Pdf
Imports Telerik.WinForms.Documents.Layout
Imports Telerik.WinForms.Documents.Model

Public Class FrmTestDynamiqueDocument
    ReadOnly editeur As RadRichTextEditor

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
    End Sub


    Private Sub RadButton1_Click(sender As Object, e As EventArgs) Handles RadButton1.Click
        'editeur = makeDoc()
        Makedoc()
    End Sub

    Private Sub Makedoc()
        Using editTools As New OasisTextTools
            With editTools

                Dim section = .CreateSection()
                Dim document = .AddSectionIntoDocument(Nothing, section)

                .CreateParagraphIntoSection(section, 15, RadTextAlignment.Center)
                .AddTexte("Test de texte sans saut de ligne)",, Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold)
                .AddTexteLine("Une suite ajoutée ", 25)
                .AddTexte("Trés gros", 50, Telerik.WinControls.RichTextEditor.UI.FontWeights.Normal)
                .AddTexteLine("Test de texte suite", 12, Telerik.WinControls.RichTextEditor.UI.FontWeights.Normal)
                .AddTexteAfterANewLine("Une deuxieme ligne ajoutée après un saut de ligne", 5)

                .CreateParagraphIntoSection(section, 25, RadTextAlignment.Left)
                .AddTexte("Test de texte align left ",, Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold)
                .AddTexteLine("Une suite ajoutée ")
                .AddTexte("En gros", 50, Telerik.WinControls.RichTextEditor.UI.FontWeights.Normal)
                .AddTexteLine("Test de texte suite", 12, Telerik.WinControls.RichTextEditor.UI.FontWeights.Normal)
                .AddTexteAfterANewLine("Une deuxieme ligne ajoutée après un saut de ligne")

                ' --- Insertion du fragment generé
                .InsertFragmentToEditor(document)

                ' --- appe! de la construction d'un tableau et ajout du fragment généré
                .InsertFragmentToEditor(AjouteElementsPlusComplexes())

                .PrintPreview()

            End With
        End Using
    End Sub



    Public Shared Sub SaveAsPdf(ByVal document As RadDocument, ByVal path As String)
        Dim provider = New PdfFormatProvider()

        Using output As Stream = New FileStream(path, FileMode.OpenOrCreate)
            provider.Export(document, output)
        End Using
    End Sub


    Private Function AjouteElementsPlusComplexes() As RadDocument
        Dim document As New RadDocument()
        Dim section As New Section()

        Dim table As New Table()
        table.LayoutMode = TableLayoutMode.AutoFit
        table.StyleName = RadDocumentDefaultStyles.DefaultTableGridStyleName

        ' -- row 1
        Dim row1 As New TableRow()

        Dim cell1 As New TableCell()
        Dim p1 As New Paragraph()
        Dim s1 As New Span()
        s1.Text = "Cell 1"
        p1.Inlines.Add(s1)
        cell1.Blocks.Add(p1)
        row1.Cells.Add(cell1)

        Dim cell2 As New TableCell()
        Dim p2 As New Paragraph()
        Dim s2 As New Span()
        s2.Text = "Cell 2"
        p2.Inlines.Add(s2)
        cell2.Blocks.Add(p2)
        row1.Cells.Add(cell2)

        table.Rows.Add(row1)

        ' -------- row 2
        Dim row2 As New TableRow()
        Dim cell3 As New TableCell()
        cell3.ColumnSpan = 2
        Dim p3 As New Paragraph()
        Dim s3 As New Span()
        s3.Text = "Cell 3"
        p3.Inlines.Add(s3)
        cell3.Blocks.Add(p3)
        row2.Cells.Add(cell3)
        table.Rows.Add(row2)
        section.Blocks.Add(table)
        section.Blocks.Add(New Paragraph())
        document.Sections.Add(section)

        Return document

    End Function
End Class