
Imports System.IO
Imports Telerik.WinControls.UI
Imports Telerik.WinForms.Documents.FormatProviders.Pdf
Imports Telerik.WinForms.Documents.Layout
Imports Telerik.WinForms.Documents.Model
Imports Telerik.WinControls.RichTextEditor.UI

Public Class OasisTextTools
    Implements IDisposable
    Public Property Editor As RadRichTextEditor

    Dim paragrapheEnCours As Paragraph  ' paragraphe en cours

    Public Sub New()
        Init()
    End Sub

    Private Sub Init()
        Editor = New RadRichTextEditor()
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
        ' caracteristique section
        Dim section As New Section()
        'section.PageMargin = New Padding(40, 40, 30, 30)
        'ex When the section has already been added to the document

        'editeur.ChangeSectionPageMargin(New Telerik.WinForms.Documents.Layout.Padding(40, 40, 30, 30))
        'editor.ChangeFontFamily(New FontFamily("Times New Roman"))
        section.PageOrientation = orientation
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

        Me.paragrapheEnCours = paragraphe
        Return paragraphe

    End Function

    Public Sub AddImage(image As WriteableBitmap, size As Size)
        If paragrapheEnCours Is Nothing Then Throw New Exception("Pas de paragraphe en cours")
        Dim _img As ImageInline = New ImageInline(image)
        _img.Size = size
        paragrapheEnCours.Inlines.Add(_img)
        Return

    End Sub

    Public Sub AddTexte(text As String,
                             Optional fontSize As Double = 12,
                             Optional fontWeight As Telerik.WinControls.RichTextEditor.UI.FontWeight = Nothing,
                             Optional fontForeColor As Color = Nothing
                             )
        If paragrapheEnCours Is Nothing Then Throw New Exception("Pas de paragraphe en cours")
        Dim span = New Span()
        span.FontSize = If(IsNothing(fontSize), paragrapheEnCours.FontSize, fontSize)
        If IsNothing(fontWeight) = False Then span.FontWeight = fontWeight   ' ex : Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold

        If fontForeColor.ToString <> "#00000000" Then
            span.ForeColor = fontForeColor
        End If
        'span.ForeColor = If(IsNothing(fontForeColor), Colors.Black, fontForeColor)
        span.Text = text

        Me.paragrapheEnCours.Inlines.Add(span)
        Return

    End Sub

    Public Sub AddTexteLine(texte As String,
                                                     Optional fontSize As Double = 12,
                                                     Optional fontWeight As Telerik.WinControls.RichTextEditor.UI.FontWeight = Nothing,
                                                     Optional fontForeColor As Color = Nothing
                                                     )
        If paragrapheEnCours Is Nothing Then Throw New Exception("Pas de paragraphe en cours")
        AddTexte(texte, fontSize, fontWeight, fontForeColor)
        AddNewLigne()
        Return

    End Sub

    Public Sub AddTexteAfterANewLine(texte As String,
                                        Optional fontSize As Double = 12,
                                        Optional fontWeight As Telerik.WinControls.RichTextEditor.UI.FontWeight = Nothing,
                                        Optional fontForeColor As Color = Nothing
                                        )
        If paragrapheEnCours Is Nothing Then Throw New Exception("Pas de paragraphe en cours")
        AddNewLigne()
        AddTexte(texte, fontSize, fontWeight, fontForeColor)
    End Sub

    Public Sub AddNewLigne()
        If paragrapheEnCours Is Nothing Then Throw New Exception("Pas de paragraphe en cours")
        Me.paragrapheEnCours.Inlines.Add(New Break(BreakType.LineBreak))
    End Sub

    Public Sub AddNewPage()
        If paragrapheEnCours Is Nothing Then Throw New Exception("Pas de paragraphe en cours")
        Me.paragrapheEnCours.Inlines.Add(New Break(BreakType.PageBreak))
    End Sub

    Public Sub SaveAsPdfToFile(ByVal pathFile As String)
        SaveAsPdfToFile(Editor.Document, pathFile)
    End Sub

    Public Function SaveAsPdfToBytes() As Byte()
        Return SaveAsPdfToBytes(Editor.Document)
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

    Public Sub InsertFragmentToEditor(document As RadDocument)
        Editor.InsertFragment(New DocumentFragment(document))
    End Sub

    Public Sub PrintPreview()
        Editor.LoadElementTree()
        Editor.PrintPreview()
    End Sub

    Public Sub Print()
        Editor.LoadElementTree()
        Editor.Print()
    End Sub

    Public Sub Dispose() Implements IDisposable.Dispose
        Editor.Dispose()
    End Sub

    Public Sub SetCell(Cell As TableCell, Texte As String, Optional FontSize As Double = 12, Optional color As Color = Nothing, Optional FontWeight As FontWeight = Nothing)
        Dim span As New Span()
        Dim paragraphe As New Paragraph()
        'paragraphe.TextAlignment = TextAlignment.Justify
        span.Text = Texte
        If span.Text <> "" Then
            paragraphe.Inlines.Add(span)
            span.FontSize = FontSize
            If color = Nothing Then
                span.ForeColor = Colors.Black
            Else
                span.ForeColor = color
            End If
            If FontWeight = Nothing Then
                span.FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Normal
            Else
                span.FontWeight = FontWeight
            End If
        End If
        Cell.Blocks.Add(paragraphe)
    End Sub

End Class
