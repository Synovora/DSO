﻿Imports System.IO
Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.WinForms.Documents.FormatProviders.OpenXml.Docx
Imports Telerik.WinForms.Documents.Model

Public Class FrmAdminTemplateDocx
    Dim sousEpisodeSousType As SousEpisodeSousType
    Public Sub New(sousEpisodeSousType As SousEpisodeSousType)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.sousEpisodeSousType = sousEpisodeSousType
        initCtrl()
    End Sub

    Private Sub backstageButtonSaveAs_Click(sender As Object, e As EventArgs)
        Dim tbl As Byte()
        Dim provider As DocxFormatProvider = New DocxFormatProvider()
        Me.Cursor = Cursors.WaitCursor
        Try
            tbl = provider.Export(Me.RadRichTextEditor1.Document)
            sousEpisodeSousType.writeContenuModel(tbl)
            Notification.show("Sauvegarde modèle", "Sauvegarde effectuée avec succès !")
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub initCtrl()
        afficheTitleForm(Me, Me.Text)

        'hide the default "Save as" button
        'Me.RichTextEditorRibbonBar1.BackstageControl.Items.Last().Visibility = ElementVisibility.Collapsed
        Dim radItemSaveAs As BackstageTabItem = Nothing
        Me.RichTextEditorRibbonBar1.BackstageControl.Items("backstageButtonNew").Visibility = ElementVisibility.Collapsed
        'Me.RichTextEditorRibbonBar1.BackstageControl.Items("backstageButtonOpen").Visibility = ElementVisibility.Collapsed
        'Me.RichTextEditorRibbonBar1.BackstageControl.Items("backstageButtonSave").Visibility = ElementVisibility.Collapsed
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

        'HideCommandGroups()
        'Me.RichTextEditorRibbonBar1.ApplicationMenuStyle = ApplicationMenuStyle.ApplicationMenu
        Me.RichTextEditorRibbonBar1.MinimizeButton = False
        Me.MinimizeBox = False

        Me.RadRichTextEditor1.LayoutMode = DocumentLayoutMode.Paged


    End Sub

    Private Sub RadButtonElement1_Click(sender As Object, e As EventArgs) Handles RadButtonElement1.Click
        backstageButtonSaveAs_Click(sender, e)
    End Sub
End Class
