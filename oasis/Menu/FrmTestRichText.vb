Imports System.IO
Imports Oasis_WF.My.Resources
Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.RichTextEditorRibbonUI
Imports Telerik.WinForms.RichTextEditor

Public Class FrmTestRichText

    Sub New()
        'RichTextBoxLocalizationProvider.CurrentProvider = RichTextBoxLocalizationProvider.FromStream(New MemoryStream(New System.Text.UTF8Encoding().GetBytes(FrenchRichTextBoxStrings.RichTextBoxStrings)))
        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        'hide the default "Save as" button
        Me.RichTextEditorRibbonBar1.BackstageControl.Items.Last().Visibility = ElementVisibility.Collapsed
        Dim radItemSaveAs As BackstageTabItem = Nothing
        For Each radItem As RadItem In Me.RichTextEditorRibbonBar1.BackstageControl.Items
            Console.WriteLine(radItem.Name & "->" & radItem.Text)
            If radItem.Name = "backstageTabItemSaveAs" Then
                radItemSaveAs = radItem
            End If
        Next

        ' Me.RichTextEditorRibbonBar1.BackstageControl.ImageList.Images

        'create your own button
        Dim backstageButtonSaveAs As New BackstageButtonItem()
        AddHandler backstageButtonSaveAs.Click, AddressOf backstageButtonSaveAs_Click
        backstageButtonSaveAs.Text = "Enregistrer dans Oasis"
        backstageButtonSaveAs.ImageIndex = radItemSaveAs.ImageIndex
        For Each propertie In radItemSaveAs.PropertyValues.Entries

        Next

        Me.RichTextEditorRibbonBar1.BackstageControl.Items.Add(backstageButtonSaveAs)

        'HideCommandGroups()

    End Sub

    Private Sub backstageButtonSaveAs_Click(sender As Object, e As EventArgs)
        RadMessageBox.Show("Save")
    End Sub

    ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
    Private Sub HideCommandGroups()

        For Each commandTab As RichTextEditorRibbonTab In Me.RichTextEditorRibbonBar1.CommandTabs
            Console.WriteLine(commandTab.Name & " : " & commandTab.Text)
            If commandTab.Name <> "tabHome" Then
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
