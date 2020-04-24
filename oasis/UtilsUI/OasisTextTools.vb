
Imports System.IO
Imports Telerik.WinControls.UI
Imports Telerik.WinForms.Documents.FormatProviders.Pdf
Imports Telerik.WinForms.Documents.Layout
Imports Telerik.WinForms.Documents.Model

Public Class OasisTextTools
    Public Property editor As RadRichTextEditor

    Dim paragraphe As Paragraph  ' para en cours

    Public Sub New()
        init()
    End Sub

    Private Sub init()
        editor = New RadRichTextEditor()
    End Sub

    ''' <summary>
    ''' ajoute une section au document, si le document est nothing, un document est instancié, dans tous les cas la fonction retourne un RadDocument
    ''' </summary>
    ''' <param name="document"></param>
    ''' <param name="section"></param>
    ''' <returns></returns>
    Public Function AddSectionIntoDocument(document As RadDocument, section As Section) As RadDocument
        If document Is Nothing Then document = New RadDocument
        document.Sections.Add(section)
        Return document
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function CreateSection(Optional orientation As PageOrientation = PageOrientation.Portrait) As Section
        ' caracteristiqe section
        Dim section As New Section()
        'section.PageMargin = New Padding(40, 40, 30, 30)
        'ex When the section has already been added to the document

        'editeur.ChangeSectionPageMargin(New Telerik.WinForms.Documents.Layout.Padding(40, 40, 30, 30))

        section.PageOrientation = PageOrientation.Portrait
        section.PageSize = PaperTypeConverter.ToSize(PaperTypes.A4)

        Return section
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="section"></param>
    ''' <param name="fontSize"></param>
    ''' <param name="textAlignment"></param>
    ''' <returns></returns>
    Public Function CreateParagraphIntoSection(section As Section,
                                      Optional fontSize As Double = 12,
                                      Optional textAlignment As RadTextAlignment = RadTextAlignment.Left) As Paragraph
        Dim paragraphe As New Paragraph()
        If IsNothing(fontSize) = False Then paragraphe.FontSize = fontSize
        paragraphe.TextAlignment = textAlignment
        section.Blocks.Add(paragraphe)    '--- ajout paragraphe à la section

        Me.paragraphe = paragraphe
        Return paragraphe

    End Function

    Public Function AddTexte(text As String,
                             Optional fontSize As Double = 12,
                             Optional fontWeight As Telerik.WinControls.RichTextEditor.UI.FontWeight = Nothing,
                             Optional paragraph As Paragraph = Nothing
                             ) As Span
        Dim span = New Span()
        span.FontSize = If(IsNothing(fontSize), paragraphe.FontSize, fontSize)
        If IsNothing(fontWeight) = False Then span.FontWeight = fontWeight   ' ex : Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold

        span.Text = text
        If paragraph Is Nothing Then recupParagraphe()

        Me.paragraphe.Inlines.Add(span)
        Return span

    End Function

    Private Sub recupParagraphe()
        If Me.paragraphe Is Nothing Then Me.paragraphe = New Paragraph()

    End Sub
    Public Function AddTexteLine(texte As String,
                                                     Optional fontSize As Double = 12,
                                                     Optional fontWeight As Telerik.WinControls.RichTextEditor.UI.FontWeight = Nothing,
                                                     Optional paragraph As Paragraph = Nothing
                                                     ) As Span
        Dim span = AddTexte(texte, fontSize, fontWeight, paragraphe)
        If paragraph Is Nothing Then recupParagraphe()
        addNewLigne(paragraph)
        Return span

    End Function

    Public Function addTexteAfterANewLine(texte As String,
                                        Optional fontSize As Double = 12,
                                        Optional fontWeight As Telerik.WinControls.RichTextEditor.UI.FontWeight = Nothing, Optional paragraphe As Paragraph = Nothing
                                        ) As Span
        addNewLigne(paragraphe)
        Return AddTexte(texte, fontSize, fontWeight, paragraphe)
    End Function

    Public Sub addNewLigne(Optional paragraphe As Paragraph = Nothing)
        If paragraphe Is Nothing Then recupParagraphe()

        Me.paragraphe.Inlines.Add(New Break(BreakType.LineBreak))
    End Sub

    Public Sub SaveAsPdfToFile(ByVal pathFile As String)
        SaveAsPdfToFile(editor.Document, pathFile)
    End Sub

    Public Function SaveAsPdfToBytes() As Byte()
        Return SaveAsPdfToBytes(editor.Document)
    End Function

    Private Sub SaveAsPdfToFile(ByVal document As RadDocument, ByVal pathFile As String)
        Dim provider = New PdfFormatProvider()

        Using output As Stream = New FileStream(pathFile, FileMode.OpenOrCreate)
            provider.Export(document, output)
        End Using
    End Sub

    Private Function SaveAsPdfToBytes(ByVal document As RadDocument) As Byte()
        Dim provider = New PdfFormatProvider()
        Return provider.Export(document)
    End Function

    Public Sub insertFragmentToEditor(document As RadDocument)
        editor.InsertFragment(New DocumentFragment(document))
    End Sub

    Public Sub printPreview()
        editor.LoadElementTree()
        editor.PrintPreview()
    End Sub

    Public Sub print()
        editor.LoadElementTree()
        editor.Print()
    End Sub

End Class
