Imports System.IO
Imports Oasis_WF.My.Resources
Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.RichTextEditorRibbonUI
Imports Telerik.WinForms.Documents.FormatProviders.OpenXml.Docx
Imports Telerik.WinForms.Documents.Model
Imports Telerik.WinForms.RichTextEditor

Public Class FrmEditDocxSousEpisode
    Inherits RadForm

    Dim sousEpisode As SousEpisode
    Dim isDocumentChange As Boolean = False

    Sub New(sousEpisode As SousEpisode)
        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        Me.sousEpisode = sousEpisode
        initCtrl()

    End Sub

    Private Sub backstageButtonSaveAs_Click(sender As Object, e As EventArgs)
        Dim tbl As Byte()
        Dim provider As DocxFormatProvider = New DocxFormatProvider()
        Me.Cursor = Cursors.WaitCursor
        Try
            tbl = provider.Export(Me.RadRichTextEditor1.Document)
            SousEpisode.writeContenuModel(tbl)
            ResetFlagChange()
            Notification.show("Sauvegarde", "Sauvegarde effectuée avec succès !")
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub RadButtonElement1_Click(sender As Object, e As EventArgs) Handles RadButtonElement1.Click
        backstageButtonSaveAs_Click(sender, e)
    End Sub

    Private Sub FrmEditDocxSousEpisode_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If isDocumentChange Then
            If Not MsgBox("Etes-vous sur de vouloir quitter sans sauvegarder ce fichier ?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Critical, "Suppression") = MsgBoxResult.Yes Then
                e.Cancel = True
            End If
        End If
    End Sub

    Private Sub FrmEditDocxSousEpisode_Shown(sender As Object, e As EventArgs) Handles MyBase.Shown
        AddHandler RadRichTextEditor1.DocumentChanged, AddressOf RadRichTextEditor1_DocumentChanged
        AddHandler RadRichTextEditor1.DocumentContentChanged, AddressOf RadRichTextEditor1_DocumentContentChanged
    End Sub

    Private Sub RadRichTextEditor1_DocumentContentChanged(sender As Object, e As EventArgs)
        isDocumentChange = True
    End Sub
    Private Sub RadRichTextEditor1_DocumentChanged(sender As Object, e As EventArgs)
        isDocumentChange = True
    End Sub

    Private Sub ResetFlagChange()
        isDocumentChange = False
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub initCtrl()
        afficheTitleForm(Me, Me.Text)

        'hide the default "Save as" button
        'Me.RichTextEditorRibbonBar1.BackstageControl.Items.Last().Visibility = ElementVisibility.Collapsed
        Dim radItemSaveAs As BackstageTabItem = Nothing
        Me.RichTextEditorRibbonBar1.BackstageControl.Items("backstageButtonNew").Visibility = ElementVisibility.Collapsed
        Me.RichTextEditorRibbonBar1.BackstageControl.Items("backstageButtonOpen").Visibility = ElementVisibility.Collapsed
        Me.RichTextEditorRibbonBar1.BackstageControl.Items("backstageButtonSave").Visibility = ElementVisibility.Collapsed
        Me.RichTextEditorRibbonBar1.BackstageControl.Items("backstageTabItemSaveAs").Visibility = ElementVisibility.Collapsed

        For Each radItem As RadItem In Me.RichTextEditorRibbonBar1.BackstageControl.Items
            Console.WriteLine(radItem.Name & "->" & radItem.Text)
            If radItem.Name = "backstageTabItemSaveAs" Then
                radItemSaveAs = radItem
            End If
        Next

        radItemSaveAs.Page.Controls(0).Controls("buttonSaveHTML").Visible = False
        radItemSaveAs.Page.Controls(0).Controls("buttonSavePlain").Visible = False
        radItemSaveAs.Page.Controls(0).Controls("buttonSaveRich").Visible = False
        radItemSaveAs.Page.Controls(0).Controls("buttonXAML").Visible = False
        radItemSaveAs.Page.Controls(0).Controls("buttonSaveWord").Visible = False
        radItemSaveAs.Page.Controls(0).Controls("buttonSavePDF").Visible = False


        'creation bouton "save dans oasis
        Dim backstageButtonSaveAs As New BackstageButtonItem()
        AddHandler backstageButtonSaveAs.Click, AddressOf backstageButtonSaveAs_Click
        backstageButtonSaveAs.Text = " Enregistrer dans Oasis"
        Dim tbl = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAIySURBVDhPrZLfS5NRGMfff6H7boIuuq2pMZyL1eAt11CWDcOKsB9vpFmaLtNExco0av6CbIVLJ61Wk3BSkT/AFCkRZSpZmrmiJQ41xSaCwdfznL15XEUX0Reem5f38znnec4j/Zc8fxYGla91CS3eRTx0z6OpMYS7jmnU1X6B/VYA18snUVoyjsKCt8jLHcH5c36ouCQR2NUJ1Nas4G9ZXlmFKbULh1Kf8lJxSfI+WeCCyopv6q+/h+DQ/DJ2WV5Ao1FgPegRAveDOS4oLfmq/h6dn/DH4AJizD4UXJrCAUuzEDgbZrjgou2DiohshIcnQtgme5GTPYbkJKcQ1N8OckHW2REVi+RXuM8fxGaDG4oyALPZIQQ11Z+5QDk1oKJ/hjv7P2FTfCMOH3mFxMQ6IbhROYWOdrCnBI4dfwPr0V4+bRoY9UzXppMjcDdSrC8hy3YhuFI2gTYf2A4Aza4f7N2/o/zaLB8qDYx6zszwr8P7k1thNFYIweXCMXgeAfedq2xxwjClZUeVJd2GtDNFETiJwfs8MBjKhMCWN8pgoLoqzE8miH1GjE7G4PsZjE7OQsm9ij2mFg7rdrug1xcJAa2l4w7Wr00Cgk/n38S7wBwC04u4UGxHrMHF4CbEJtyDLj5fCDIzhljfSxzeavRgyw4Zj9t64GvvQ0d3P3pfD2Kv2QqNvgFxDN6urYdWmyMElJMnevh60obRktA701PRtGlg1DOdSkXwzrisaMG/RZLWAE60OMW5fNhvAAAAAElFTkSuQmCC")
        Using ms As MemoryStream = New MemoryStream(tbl)
            backstageButtonSaveAs.Image = Image.FromStream(ms)
        End Using
        backstageButtonSaveAs.ImageAlignment = ContentAlignment.MiddleLeft

        Me.RichTextEditorRibbonBar1.BackstageControl.Items.Add(backstageButtonSaveAs)

        HideCommandGroups()
        'Me.RichTextEditorRibbonBar1.ApplicationMenuStyle = ApplicationMenuStyle.ApplicationMenu
        Me.RichTextEditorRibbonBar1.MinimizeButton = False
        Me.MinimizeBox = False

        Me.RadRichTextEditor1.LayoutMode = DocumentLayoutMode.Paged

    End Sub
    ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
    Private Sub HideCommandGroups()

        For Each commandTab As RichTextEditorRibbonTab In Me.RichTextEditorRibbonBar1.CommandTabs
            Console.WriteLine(commandTab.Name & " : " & commandTab.Text)
            If commandTab.Name <> "tabHome" Then ' AndAlso commandTab.Name <> "tabMailings" Then
                commandTab.Visibility = ElementVisibility.Collapsed
            Else

                For Each group As RadRibbonBarGroup In commandTab.Items
                    Console.WriteLine(vbTab & group.Name & " : " & group.Text)

                    ' If Not (group.Text = "Font" OrElse group.Text = "Paragraph") Then
                    ' group.Visibility = ElementVisibility.Collapsed
                    ' End If
                Next
            End If
        Next
    End Sub


End Class
