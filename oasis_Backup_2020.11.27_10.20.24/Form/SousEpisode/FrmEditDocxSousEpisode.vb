Imports System.IO
Imports Oasis_Common
Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.RichTextEditorRibbonUI
Imports Telerik.WinForms.Documents
Imports Telerik.WinForms.Documents.FormatProviders.OpenXml.Docx
Imports Telerik.WinForms.Documents.Model
Imports Telerik.WinForms.Documents.TextSearch
Imports Telerik.WinForms.RichTextEditor

Public Class FrmEditDocxSousEpisode
    Inherits RadForm

    Dim sousEpisode As SousEpisode
    Dim isDocumentChange As Boolean = False
    Dim isNotSigned As Boolean
    Dim validationProfilType As String
    Dim backstageButtonSaveAs As New BackstageButtonItem()

    Enum ActionDOC
        QUITTER = -1
        RETOUR = 0
        ENREGISTRER = 1
        ENREGISTRER_ET_SIGNER = 2
    End Enum

    Sub New(sousEpisode As SousEpisode, isNotSigned As Boolean, validationProfilType As String)
        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        Me.sousEpisode = sousEpisode
        Me.isNotSigned = isNotSigned
        Me.validationProfilType = validationProfilType

        initCtrl()

    End Sub

    Private Sub backstageButtonSaveAs_Click(sender As Object, e As EventArgs)
        enregister_and_sign(False)
    End Sub

    Private Sub RadButtonElement1_Click(sender As Object, e As EventArgs) Handles RadButtonElement1.Click
        backstageButtonSaveAs_Click(sender, e)
    End Sub

    Private Sub FrmEditDocxSousEpisode_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If isNotSigned Then
            Dim choix = ChoixAction()
            Select Case choix
                Case ActionDOC.RETOUR
                    e.Cancel = True : Return
                Case ActionDOC.ENREGISTRER
                    enregister_and_sign(False)
                Case ActionDOC.ENREGISTRER_ET_SIGNER
                    If MsgBox("Etes-vous sur de vouloir signer ce sous-épisode ?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Critical, "Signature Sous-Episode") = MsgBoxResult.Yes Then
                        enregister_and_sign(True)
                        isNotSigned = False
                        initCtrl()
                    End If
                    e.Cancel = True
                Case ActionDOC.QUITTER
                    If isDocumentChange Then
                        If Not MsgBox("Des modifications vont être abandonnées !" & vbCrLf & "Etes vous sùr ? ", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Critical, "Suppression") = MsgBoxResult.Yes Then
                            e.Cancel = True
                        End If
                    End If
            End Select
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

    Private Sub enregister_and_sign(isSignAlso As Boolean)
        Dim tbl As Byte()
        Dim provider As DocxFormatProvider = New DocxFormatProvider()
        Dim sauvegarde As RadDocument = Nothing
        Dim dateSign = Date.Now
        Dim signature As String = Nothing
        Me.Cursor = Cursors.WaitCursor
        Try
            If isSignAlso Then
                sauvegarde = Me.RadRichTextEditor1.MailMerge()
                signature = userLog.Sign(sousEpisode.Serialize())
                appose_signature(dateSign, signature)
            End If
            tbl = provider.Export(Me.RadRichTextEditor1.Document)
            Dim sousEpisodeDao = New SousEpisodeDao
            sousEpisodeDao.writeDocAndEventualySign(sousEpisode, tbl, signature, dateSign, userLog, loginRequestLog)
            ResetFlagChange()
            Notification.show("Sauvegarde", "Action effectuée avec succès !", 1)
        Catch err As Exception
            If sauvegarde IsNot Nothing Then
                Me.RadRichTextEditor1.Document = sauvegarde
            End If
            MsgBox(err.Message())
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub appose_signature(dateSign As Date, signature As String)
        ReplaceAllMatches("@Signataire_Fonction", userLog.UtilisateurProfilId.ToLower.Trim.Replace("_", " "))
        ReplaceAllMatches("@Signataire_PrenomNom", userLog.UtilisateurPrenom.Trim & " " & userLog.UtilisateurNom.Trim)
        ReplaceAllMatches("@Signature_Date", dateSign.ToString("dd MMM yyyy"))
        'ReplaceAllMatches("@Signature", signature)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub initCtrl()
        AfficheTitleForm(Me, Me.Text, userLog)

        'hide the default "Save as" button
        'Me.RichTextEditorRibbonBar1.BackstageControl.Items.Last().Visibility = ElementVisibility.Collapsed
        Dim radItemSaveAs As BackstageTabItem = Nothing
        Me.RichTextEditorRibbonBar1.BackstageControl.Items("backstageButtonNew").Visibility = ElementVisibility.Collapsed
        Me.RichTextEditorRibbonBar1.BackstageControl.Items("backstageButtonOpen").Visibility = ElementVisibility.Collapsed
        Me.RichTextEditorRibbonBar1.BackstageControl.Items("backstageButtonSave").Visibility = ElementVisibility.Collapsed
        Me.RichTextEditorRibbonBar1.BackstageControl.Items("backstageTabItemSaveAs").Visibility = ElementVisibility.Collapsed
        ' -- si besoin : à réactiver
        'If isNotSigned Then
        'Me.RichTextEditorRibbonBar1.BackstageControl.Items("backstageButtonPrint").Visibility = ElementVisibility.Collapsed
        'Me.RichTextEditorRibbonBar1.BackstageControl.Items("backstageButtonPrintPreview").Visibility = ElementVisibility.Collapsed
        'End If

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
        If isNotSigned Then
            AddHandler backstageButtonSaveAs.Click, AddressOf backstageButtonSaveAs_Click
            backstageButtonSaveAs.Text = " Enregistrer dans Oasis"
            Dim tbl = Convert.FromBase64String("iVBORw0KGgoAAAANSUhEUgAAABAAAAAQCAYAAAAf8/9hAAAAAXNSR0IArs4c6QAAAARnQU1BAACxjwv8YQUAAAAJcEhZcwAADsMAAA7DAcdvqGQAAAIySURBVDhPrZLfS5NRGMfff6H7boIuuq2pMZyL1eAt11CWDcOKsB9vpFmaLtNExco0av6CbIVLJ61Wk3BSkT/AFCkRZSpZmrmiJQ41xSaCwdfznL15XEUX0Reem5f38znnec4j/Zc8fxYGla91CS3eRTx0z6OpMYS7jmnU1X6B/VYA18snUVoyjsKCt8jLHcH5c36ouCQR2NUJ1Nas4G9ZXlmFKbULh1Kf8lJxSfI+WeCCyopv6q+/h+DQ/DJ2WV5Ao1FgPegRAveDOS4oLfmq/h6dn/DH4AJizD4UXJrCAUuzEDgbZrjgou2DiohshIcnQtgme5GTPYbkJKcQ1N8OckHW2REVi+RXuM8fxGaDG4oyALPZIQQ11Z+5QDk1oKJ/hjv7P2FTfCMOH3mFxMQ6IbhROYWOdrCnBI4dfwPr0V4+bRoY9UzXppMjcDdSrC8hy3YhuFI2gTYf2A4Aza4f7N2/o/zaLB8qDYx6zszwr8P7k1thNFYIweXCMXgeAfedq2xxwjClZUeVJd2GtDNFETiJwfs8MBjKhMCWN8pgoLoqzE8miH1GjE7G4PsZjE7OQsm9ij2mFg7rdrug1xcJAa2l4w7Wr00Cgk/n38S7wBwC04u4UGxHrMHF4CbEJtyDLj5fCDIzhljfSxzeavRgyw4Zj9t64GvvQ0d3P3pfD2Kv2QqNvgFxDN6urYdWmyMElJMnevh60obRktA701PRtGlg1DOdSkXwzrisaMG/RZLWAE60OMW5fNhvAAAAAElFTkSuQmCC")
            Using ms As MemoryStream = New MemoryStream(tbl)
                backstageButtonSaveAs.Image = Image.FromStream(ms)
            End Using
            backstageButtonSaveAs.ImageAlignment = ContentAlignment.MiddleLeft
            Me.RichTextEditorRibbonBar1.BackstageControl.Items.Add(backstageButtonSaveAs)
            ' bouton imprimer en haut à gauche
            'RadButtonElement2.Visibility = ElementVisibility.Collapsed
        Else
            Me.RadRichTextEditor1.IsContextMenuEnabled = False
            backstageButtonSaveAs.Visibility = ElementVisibility.Collapsed
            RadRichTextEditor1.IsReadOnly = True
            RadRichTextEditor1.IsSelectionEnabled = False
            ' bouton picto save en haut à gauche
            RadButtonElement1.Visibility = ElementVisibility.Collapsed
            ' bouton picto imprimer en haut à gauche
            'RadButtonElement2.Visibility = ElementVisibility.Visible
        End If

        HideCommandGroups()

        Me.RichTextEditorRibbonBar1.MinimizeButton = False
        Me.MinimizeBox = False

        Me.RadRichTextEditor1.LayoutMode = DocumentLayoutMode.Paged
        Me.RadRichTextEditor1.Document.SectionDefaultPageSize = PaperTypeConverter.ToSize(PaperTypes.A4)

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub HideCommandGroups()

        For Each commandTab As RichTextEditorRibbonTab In Me.RichTextEditorRibbonBar1.CommandTabs
            Console.WriteLine(commandTab.Name & " : " & commandTab.Text)
            If commandTab.Name <> "tabHome" AndAlso commandTab.Name <> "tabInsert" Then commandTab.Visibility = ElementVisibility.Collapsed

            For Each group As RadRibbonBarGroup In commandTab.Items
                'Console.WriteLine(vbTab & group.Name & " : " & group.Text)
                If isNotSigned = False Then
                    group.Enabled = False
                End If
            Next
        Next
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Private Function ChoixAction() As ActionDOC
        Dim choix As ActionDOC
        Using frmAction As FrmActionDoc = New FrmActionDoc
            frmAction.Location = New Point(MousePosition.X - frmAction.Size.Width - 30, MousePosition.Y)
            frmAction.BtnSigner.Visible = isNotSigned AndAlso SousEpisodeSousType.IsUserLogAutorise(validationProfilType, userLog)

            frmAction.ShowDialog()
            choix = frmAction.ActionChoisie
            Return choix
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="toSearch"></param>
    ''' <param name="toReplaceWith"></param>
    Public Sub ReplaceAllMatches(ByVal toSearch As String, ByVal toReplaceWith As String)
        Me.RadRichTextEditor1.Document.Selection.Clear() ' this clears the selection before processing
        Dim search As New DocumentTextSearch(Me.RadRichTextEditor1.Document)
        Dim rangesTrackingDocumentChanges As New List(Of Telerik.WinForms.Documents.TextSearch.TextRange)()
        For Each textRange In search.FindAll(toSearch)
            Dim newRange As New TextSearch.TextRange(New Telerik.WinForms.Documents.DocumentPosition(textRange.StartPosition, True), New DocumentPosition(textRange.EndPosition, True))
            rangesTrackingDocumentChanges.Add(newRange)
        Next textRange
        For Each textRange In rangesTrackingDocumentChanges
            Me.RadRichTextEditor1.Document.Selection.AddSelectionStart(textRange.StartPosition)
            Me.RadRichTextEditor1.Document.Selection.AddSelectionEnd(textRange.EndPosition)
            Me.RadRichTextEditor1.Insert(toReplaceWith)
            textRange.StartPosition.Dispose()
            textRange.EndPosition.Dispose()
        Next textRange
    End Sub

    Private Sub RichTextEditorRibbonBar1_CommandTabSelecting(sender As Object, args As CommandTabSelectingEventArgs) Handles RichTextEditorRibbonBar1.CommandTabSelecting
        If isNotSigned = False Then 'And args.NewCommandTab.Name <> "tabHome" Then
            args.NewCommandTab.Enabled = False
            args.Cancel = True
        End If
    End Sub
End Class
