﻿Imports System.Configuration
Imports System.Diagnostics
Imports System.IO
Imports Oasis_Common
Imports Telerik.WinControls
Imports Telerik.WinControls.Enumerations
Imports Telerik.WinControls.UI
Imports Telerik.WinForms.Documents.FormatProviders.OpenXml.Docx
Imports Telerik.WinForms.Documents.Model


Public Class FrmSousEpisode

    Dim sousEpisodeDao As SousEpisodeDao = New SousEpisodeDao
    Dim sousEpisodeTypeDao As SousEpisodeTypeDao = New SousEpisodeTypeDao
    Dim sousEpisodeSousTypeDao As SousEpisodeSousTypeDao = New SousEpisodeSousTypeDao
    Dim sousEpisodeSousSousTypeDao As SousEpisodeSousSousTypeDao = New SousEpisodeSousSousTypeDao
    Dim sousEpisodeReponseDao As SousEpisodeReponseDao = New SousEpisodeReponseDao
    Dim parcoursDao As ParcoursDao = New ParcoursDao

    Dim episode As Episode, patient As Patient, sousEpisode As SousEpisode
    Dim userCreateNom As String, userUpdateNom As String, userValidateNom As String
    Dim isCreation As Boolean
    Dim isNotSigned As Boolean
    Dim isPatientALD As Boolean
    Dim isFusionTodo As Boolean

    Dim lstSousEpisodeType As List(Of SousEpisodeType)
    Dim lstSousEpisodeSousType As List(Of SousEpisodeSousType)
    Dim lstSousEpisodeSousSousType As List(Of SousEpisodeSousSousType)
    Dim lstIntervenant As List(Of IntervenantParcours)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="episode"></param>
    ''' <param name="patient"></param>
    ''' <param name="sousEpisode"></param>
    ''' <param name="userCreateNom"></param>
    ''' <param name="userUpdateNom"></param>
    ''' <param name="userValidateNom"></param>
    Sub New(episode As Episode, patient As Patient, sousEpisode As SousEpisode, userCreateNom As String, userUpdateNom As String, userValidateNom As String)

        ' -- Cet appel est requis par le concepteur.
        InitializeComponent()

        ' -- Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        AfficheTitleForm(Me, Me.Text, userLog)

        ' -- episode en cours
        Me.episode = episode
        Me.patient = patient
        Me.sousEpisode = sousEpisode
        Me.userCreateNom = userCreateNom
        Me.userUpdateNom = userUpdateNom
        Me.userValidateNom = userValidateNom

        ' -- initialisation des controles du formulaire
        initOneShot()
        initControls()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DropDownType_SelectedIndexChanged(sender As Object, e As Data.PositionChangedEventArgs) Handles DropDownType.SelectedIndexChanged
        If Me.DropDownType.SelectedItem IsNot Nothing Then
            Dim seType = TryCast(Me.DropDownType.SelectedItem.Value, SousEpisodeType)
            initSousTypes(seType.Id)
            initDestinataire(seType)
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DropDownSousType_SelectedIndexChanged(sender As Object, e As Data.PositionChangedEventArgs) Handles DropDownSousType.SelectedIndexChanged
        If Me.DropDownSousType.SelectedItem IsNot Nothing Then
            initSousSousTypes(TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType).Id)
            'setDefaultALDGrid()
            If isCreation Then
                Dim sousEpisodeSousType As SousEpisodeSousType = TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType)
                TxtDelai.Value = Coalesce(sousEpisodeSousType.DelaiReponse, "")
                ChkBReponseAttendue.Checked = sousEpisodeSousType.IsReponseRequise
            End If
            initALDetReponse()

        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    Private Sub ChkALD_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles ChkALD.ToggleStateChanged
        'setDefaultALDGrid()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    Private Sub ChkBReponseAttendue_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles ChkBReponseAttendue.ToggleStateChanged
        Dim visible = (args.ToggleState = ToggleState.On)
        TxtDelai.Visible = visible
        LblDelai.Visible = visible
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnAjoutReponse_Click(sender As Object, e As EventArgs) Handles BtnAjoutReponse.Click
        Dim dr As DialogResult = OpenFileDialog1.ShowDialog()

        If dr = System.Windows.Forms.DialogResult.OK Then
            Dim fileName As String = OpenFileDialog1.FileName
            Dim comment = RadInputBox.Show("Fichier : " & fileName & vbCrLf & "Saisissez votre commentaire", "Introduction de document reçu", "#CANCEL#")

            If comment = "#CANCEL#" Then
                Notification.show("Ajout document", "Ajout annulé !")
                Return
            End If
            Try
                Me.Cursor = Cursors.WaitCursor
                Dim sousEpisodeReponse As SousEpisodeReponse = New SousEpisodeReponse
                With sousEpisodeReponse
                    .IdSousEpisode = sousEpisode.Id
                    .CreateUserId = userLog.UtilisateurId
                    .HorodateCreation = DateTime.Now
                    .NomFichier = Path.GetFileName(fileName)
                    .Commentaire = comment
                End With
                If sousEpisodeReponseDao.Create(sousEpisode, sousEpisodeReponse, fileName, loginRequestLog) = False Then
                    Notification.show("Ajout document", "ERREUR insertion du nouveau document !!!")
                Else
                    Notification.show("Ajout document", "Ajout terminée avec succès !")
                    refreshGrid()
                End If
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gridReponse_CommandCellClick(sender As Object, e As GridViewCellEventArgs)
        Dim gce As GridCommandCellElement = (TryCast(sender, GridCommandCellElement))
        Select Case gce.ColumnInfo.Name.ToLower
            Case "telecharger"
                TelechargerReponse(gce)
            Case "valider"
                validerReponse(gce)
            Case "askvalider"
                askValiderReponse(gce)
            Case "supprimer"
                supprimer(gce)
        End Select
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnValidate_Click(sender As Object, e As EventArgs) Handles BtnValidate.Click
        If serializeSousEpisode() Then
            ' -- on enchaine directe sur edition doc
            BtnEditerDocument_Click(sender, e)
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="gce"></param>
    Private Sub TelechargerReponse(gce As GridCommandCellElement)
        'MessageBox.Show("Telecharger fichier " & gce.RowInfo.Cells("NomFichier").Value & " : " & gce.RowInfo.Cells("IdSousEpisode").Value & "_" & gce.RowInfo.Cells("Id").Value)
        Dim sousEpisodeReponse As SousEpisodeReponse
        Try
            Me.Cursor = Cursors.WaitCursor
            sousEpisodeReponse = sousEpisodeReponseDao.getById(gce.RowInfo.Cells("Id").Value)

            Dim tbl As Byte() = sousEpisodeReponseDao.getContenu(episode.Id, sousEpisodeReponse, loginRequestLog)
            'Me.Cursor = Cursors.Default
            'SaveFileDialog1.FileName = sousEpisodeReponse.NomFichier
            'Select Case (SaveFileDialog1.ShowDialog())
            '    Case DialogResult.Abort, DialogResult.Cancel
            '        Notification.show("Réponse Sous-épisode", "Téléchargement abandonné !")
            '    Case DialogResult.OK, DialogResult
            '        File.WriteAllBytes(SaveFileDialog1.FileName, tbl)
            '        Notification.show("Réponse Sous-épisode", "Téléchargement de " & SaveFileDialog1.FileName & " Terminé !")
            'End Select

            Dim pathDownload = ConfigurationManager.AppSettings("CheminTelechargement")
            If (Not System.IO.Directory.Exists(pathDownload)) Then
                System.IO.Directory.CreateDirectory(pathDownload)
            End If

            File.WriteAllBytes(pathDownload & "\" & sousEpisodeReponse.NomFichier, tbl)
            Dim proc As New Process()
            ' Nom du fichier dont l'extension est connue du shell à ouvrir 
            Try
                proc.StartInfo.FileName = pathDownload & "\" & sousEpisodeReponse.NomFichier
                proc.Start()
                ' On libère les ressources 
                proc.Close()
                Notification.show("Lancement du logiciel associé", "Veuillez patienter pendant le lancement du logiciel associé à la visualisation de votre fichier !")
            Catch err As Exception
                MsgBox(err.Message() & vbCrLf & "Votre fichier est téléchargé et disponible dans le répertoire suivant : " & vbCrLf & pathDownload)
            End Try
        Catch Err As Exception
            MsgBox(Err.Message())
            Return
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub askValiderReponse(gce As GridCommandCellElement)
        If MsgBox("Etes-vous sur de vouloir demander une validation medicale de ce fichier ?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information, "Validation") = MsgBoxResult.Yes Then
            sousEpisodeReponseDao.askValider(gce.RowInfo.Cells("Id").Value, userLog)
            refreshGrid()
        End If
    End Sub

    Private Sub validerReponse(gce As GridCommandCellElement)
        If MsgBox("Etes-vous sur de vouloir valider ce fichier ?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Information, "Validation") = MsgBoxResult.Yes Then
            sousEpisodeReponseDao.valider(gce.RowInfo.Cells("Id").Value, userLog)
            refreshGrid()
        End If
    End Sub

    Private Sub supprimer(gce As GridCommandCellElement)
        If MsgBox("Etes-vous sur de vouloir supprimer ce fichier ?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Critical, "Suppression") = MsgBoxResult.Yes Then
            Dim isDernier As Boolean = Me.RadReponseGrid.Rows.Count < 2
            sousEpisodeReponseDao.delete(sousEpisode, gce.RowInfo.Cells("Id").Value, isDernier)
            refreshGrid()
        End If
    End Sub

    Private Sub BtnSupprSousEpisode_Click(sender As Object, e As EventArgs) Handles BtnSupprSousEpisode.Click
        If MsgBox("Etes-vous sùr de vouloir supprimer ce sous épisode ?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Critical, "Suppression") = MsgBoxResult.Yes Then
            sousEpisode.isInactif = sousEpisodeDao.inactiverSousEpisode(Nothing, sousEpisode.Id, Nothing)
            Close()
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub setDefaultALDGrid()
        If isCreation Then
            Dim sousType As SousEpisodeSousType = TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType)
            Dim isALD = sousType.IsALDPossible AndAlso isPatientALD AndAlso ChkALD.Checked
            For Each row In RadSousSousTypeGrid.Rows
                row.Cells("ChkALD").Value = isALD
            Next
        End If

    End Sub

    'TODO: Paramedical / Medical

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub initOneShot()
        '  -- somme nous en mode creation (sinon mode update)
        isCreation = If(sousEpisode.Id = 0, True, False)
        ' -- le patient est il en ALD
        Dim aldDO = New AldDao()
        isPatientALD = aldDO.IsPatientALD(patient.PatientId)

        ' -- init des details du bean SousEpisode
        If Not isCreation Then
            Dim sousEpisodeDetailSousTypeDao As SousEpisodeDetailSousTypeDao = New SousEpisodeDetailSousTypeDao
            sousEpisode.lstDetail = sousEpisodeDetailSousTypeDao.getLstSousEpisodeDetailSousType(sousEpisode.Id)
        End If

        ' -- listes de references
        lstIntervenant = parcoursDao.GetListOfIntervenantNonOasisByPatient(patient.PatientId)
        lstSousEpisodeType = sousEpisodeTypeDao.getLstSousEpisodeType()
        lstSousEpisodeSousType = sousEpisodeSousTypeDao.getLstSousEpisodeSousType()
        lstSousEpisodeSousSousType = sousEpisodeSousSousTypeDao.getLstSousEpisodeSousSousType()

        With sousEpisode

            ' -- combo type
            Me.DropDownType.Items.Clear()
            For Each sousEpisodeType As SousEpisodeType In lstSousEpisodeType
                If FiltreTypeByProfil(sousEpisodeType, userLog) Then Continue For
                Dim radListItem As New RadListDataItem(sousEpisodeType.Libelle, sousEpisodeType)
                Me.DropDownType.Items.Add(radListItem)
                'If TryCast(radListItem.Value, SousEpisodeType).Id = Me.sousEpisode.IdSousEpisodeType Then
                If sousEpisodeType.Id = Me.sousEpisode.IdSousEpisodeType Then
                    radListItem.Selected = True
                    ' -- init des sous types
                    initSousTypes(sousEpisodeType.Id)
                End If
            Next
            If isCreation AndAlso Me.DropDownType.Items.Count > 0 Then
                Me.DropDownType.SelectedItem = Me.DropDownType.Items(0)
                initSousTypes(TryCast(Me.DropDownType.SelectedItem.Value, SousEpisodeType).Id)
            End If
            Me.DropDownType.DefaultItemsCountInDropDown = DropDownType.Items.Count

            ' -- combo destinataires
            Me.DropDownDestinataire.Items.Clear()
            For Each intervenant As IntervenantParcours In lstIntervenant
                Dim radListItem As New RadListDataItem(intervenant.Nom & " - " & intervenant.Specialite, intervenant)
                Me.DropDownDestinataire.Items.Add(radListItem)
                'If TryCast(radListItem.Value, SousEpisodeType).Id = Me.sousEpisode.IdSousEpisodeType Then
                If intervenant.IntervenantId = Me.sousEpisode.IdIntervenant Then radListItem.Selected = True
            Next
            If isCreation AndAlso Me.DropDownDestinataire.Items.Count > 0 Then
                Me.DropDownDestinataire.SelectedItem = Me.DropDownDestinataire.Items(0)
            End If
            Me.DropDownDestinataire.DefaultItemsCountInDropDown = DropDownDestinataire.Items.Count

        End With

        '-- handler sur boutons grid reponse
        AddHandler RadReponseGrid.CommandCellClick, AddressOf gridReponse_CommandCellClick
        AddHandler RadReponseGrid.CellFormatting, AddressOf RadReponseGrid_CellFormatting

    End Sub

    Private Sub RadReponseGrid_CellFormatting(ByVal sender As Object, ByVal e As CellFormattingEventArgs)
        e.CellElement.Enabled = True

        If e.RowIndex > -1 AndAlso e.Column.Name = "Valider" Or e.Column.Name = "AskValider" Then
            If (e.Row.Cells("NomFichier").Style.ForeColor) = Color.Green Or (e.Column.Name = "AskValider" And (e.Row.Cells("NomFichier").Style.ForeColor) = Color.Orange) Then
                e.CellElement.Enabled = False
            Else
                e.CellElement.Enabled = True
            End If
        End If
    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub initControls()
        isNotSigned = (sousEpisode.HorodateValidate = Nothing)

        With sousEpisode
            If .HorodateCreation = Nothing Then .HorodateCreation = DateTime.Now
            Me.lblDateCreation.Text = .HorodateCreation.ToString("dd/MM/yyyy HH:mm") & " par " & userCreateNom
            Me.LblDateModif.Text = If(.HorodateLastUpdate = Nothing, "Non Modifié", .HorodateLastUpdate.ToString("dd/MM/yyyy HH:mm") & " par " & userUpdateNom)
            Me.LblDateValidation.Text = If(.HorodateValidate = Nothing, "Non Signé", .HorodateValidate.ToString("dd/MM/yyyy HH:mm") & " par " & userValidateNom)
        End With

        ' -- reponses
        If Not isCreation Then
            ChkBReponseAttendue.Checked = sousEpisode.IsReponse
            ChkBReponseAttendue.Enabled = isCreation
            TxtDelai.Value = If(sousEpisode.DelaiSinceValidation = Nothing, "", sousEpisode.DelaiSinceValidation)
            refreshGrid()
        End If
        TxtDelai.Enabled = isCreation
        BtnAjoutReponse.Visible = isCreation = False AndAlso isNotSigned = False
        BtnMail.Enabled = isCreation = False AndAlso isNotSigned = False
        BtnValidate.Visible = isCreation

        BtnEditerDocument.Visible = Not isCreation

        Dim sousEpisodeSousType As SousEpisodeSousType = TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType)

        Me.DropDownType.Enabled = isCreation
        Me.DropDownDestinataire.Enabled = isCreation

        ChkALD.Checked = If(isCreation, isPatientALD, sousEpisode.IsALD)
        ChkALD.Enabled = isCreation

        Me.TxtRDVCommentaire.Text = sousEpisode.Commentaire
        Me.TxtRDVCommentaire.Enabled = isCreation
        Me.DropDownSousType.Enabled = isCreation
        Me.RadSousSousTypeGrid.ReadOnly = Not isCreation
        Me.BtnSupprSousEpisode.Visible = Not isCreation AndAlso Not sousEpisode.isInactif

    End Sub

    ''' <summary>
    ''' refresh du grid des reponses au sous-episode
    ''' </summary>
    Private Sub refreshGrid()
        Dim exId As Long, index As Integer = -1, exPosit = 0
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim data As DataTable = sousEpisodeReponseDao.getTableSousEpisodeReponse(sousEpisode.Id)

            Dim numRowGrid As Integer = 0

            ' -- recup eventuelle precedente selectionnée
            If RadReponseGrid.Rows.Count > 0 AndAlso Not IsNothing(Me.RadReponseGrid.CurrentRow) Then
                exId = Me.RadReponseGrid.CurrentRow.Cells("Id").Value
                exPosit = Me.RadReponseGrid.CurrentRow.Index
            End If
            RadReponseGrid.Rows.Clear()

            For Each row In data.Rows
                RadReponseGrid.Rows.Add(numRowGrid)
                '------------------- Alimentation du DataGridView
                With RadReponseGrid.Rows(numRowGrid)
                    .Cells("Id").Value = row("id")
                    If .Cells("Id").Value = exId Then index = numRowGrid   ' position exact
                    If numRowGrid <= exPosit Then exPosit = numRowGrid     ' position approchée 
                    .Cells("IdSousEpisode").Value = row("id_sous_episode")
                    .Cells("HorodateCreation").Value = row("horodate_creation")
                    .Cells("CreateUser").Value = row("user_create")
                    .Cells("commentaire").Value = row("commentaire")
                    .Cells("NomFichier").Value = row("nom_fichier")
                    .Cells("NomFichier").Style.ForeColor = If(row("validate_state") = "v", Color.Green, If(row("validate_state") = "!", Color.Red, If(row("validate_state") = "m", Color.Orange, Color.Black)))
                    '.Cells("Valider").Value = "Tests"
                    .Cells("AskValider").ColumnInfo.IsVisible = If(userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString, True, False)

                    'userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString
                    ' -- on garnit le tag pour affichage tooltip
                    '                    RadTacheToTreatGrid.Rows.Last.Tag = " << " & .Cells("type").Value & " >>" & vbCrLf &
                    '                    If (Coalesce(row("is_ald"), False), " --> ALD" & vbCrLf, "") &
                    '                                "Fichier : " & row("nom_fichier") & vbCrLf &
                    '                    If (Coalesce(row("is_reponse"), False), " ... Réponse requise sous " & row("delai_since_validation") & " j à partir de la date de validation" & vbCrLf, "") &
                    '                    If (Coalesce(row("is_reponse_recue"), False), " ... Dernière reçue le  " & row("horodate_last_recu"), " ... NON REÇUE ...") & vbCrLf &
                    '                                " ------------------------------------------" & vbCrLf &
                    '                    row("commentaire") & vbCrLfThenThenThen
                End With
                numRowGrid += 1
            Next
            ' -- positionnement a la ligne la plus proche de la precedente
            If data.Rows.Count > 0 Then
                Me.RadReponseGrid.CurrentRow = RadReponseGrid.Rows(If(index >= 0, index, exPosit))
            End If
            Me.Cursor = Cursors.Default


        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try


    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sousEpisodeType"></param>
    ''' <returns></returns>
    Private Function FiltreTypeByProfil(sousEpisodeType As SousEpisodeType, userLog As Object) As Boolean
        If isCreation = False Then Return False   ' pas de filtre si pas en création
        ' -- on regarde si au moins un sousType autorisé pour ce profil
        For Each sousEpisodeSousType As SousEpisodeSousType In lstSousEpisodeSousType
            If sousEpisodeSousType.IdSousEpisodeType <> sousEpisodeType.Id Then Continue For
            If sousEpisodeSousType.IsUserLogRedactionAutorise(userLog) Then Return False
        Next
        Return True '  => pas autorisé
    End Function

    Private Sub BtnEditerDocument_Click(sender As Object, e As EventArgs) Handles BtnEditerDocument.Click
        If Me.DropDownSousType.SelectedItem Is Nothing Then Return
        Dim sousType = (TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType))
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            Dim lstSousEpisodeFusion As List(Of SousEpisodeFusion) = Me.ConstitueFusion()
            Dim tbl = Telecharger_SousEpisodeDemande(sousType)
            Dim ins = New MemoryStream(tbl)
            Dim provider = New DocxFormatProvider()

            Using frm = New FrmEditDocxSousEpisode(sousEpisode, isNotSigned, sousType.ValidationProfilTypes)
                If tbl.Count > 0 Then
                    frm.RadRichTextEditor1.Document = provider.Import(ins)
                End If
                If isNotSigned AndAlso isFusionTodo Then
                    frm.RadRichTextEditor1.Document.MailMergeDataSource.ItemsSource = lstSousEpisodeFusion
                    frm.RadRichTextEditor1.UpdateAllFields(FieldDisplayMode.DisplayName)

                    Dim merged = frm.RadRichTextEditor1.MailMerge()
                    frm.RadRichTextEditor1.Document = merged
                End If
                ins.Dispose()
                tbl = Nothing
                frm.ShowDialog()
                Dim isNotSignedNew = (sousEpisode.HorodateValidate = Nothing)
                If isNotSignedNew = False AndAlso isNotSignedNew <> isNotSigned Then
                    isNotSigned = isNotSignedNew
                    userValidateNom = userLog.UtilisateurPrenom + " " + userLog.UtilisateurNom
                    initControls()
                End If
            End Using
            refreshGrid()
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
            Me.Close()

        End Try

    End Sub

    Private Sub RadSousSousTypeGrid_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadSousSousTypeGrid.CellFormatting
        ' --- on enleve le carre des checkbox
        If Not isCreation Then
            Dim checkBoxCell As GridCheckBoxCellElement = TryCast(e.CellElement, GridCheckBoxCellElement)
            If checkBoxCell IsNot Nothing Then
                Dim editor As RadCheckBoxEditor = TryCast(checkBoxCell.Editor, RadCheckBoxEditor)
                Dim element As RadCheckBoxEditorElement = TryCast(editor.EditorElement, RadCheckBoxEditorElement)
                element.Checkmark.Border.Visibility = ElementVisibility.Collapsed
                element.Checkmark.Fill.Visibility = ElementVisibility.Collapsed
            End If
        End If
    End Sub

    Private Function telecharger_model(sousEpisodeSousType As SousEpisodeSousType) As Byte()
        Dim tbl As Byte() = {}
        Try
            tbl = sousEpisodeSousType.getContenuModel(loginRequestLog)
            Return tbl
        Catch err As Exception
            If err IsNot Nothing AndAlso err.Message IsNot Nothing AndAlso err.Message.Contains("Fichier demandé inexistant") Then
                Return tbl
            End If
            Throw err
        End Try
    End Function

    Private Function Telecharger_SousEpisodeDemande(sousEpisodeSousType As SousEpisodeSousType) As Byte()
        Dim tbl As Byte()
        Try
            tbl = sousEpisode.GetContenu(loginRequestLog)
            isFusionTodo = False
            Return tbl
        Catch err As Exception
            ' --- si inexistant => 1ere saisie : on récupère le modèle
            If err IsNot Nothing AndAlso err.Message IsNot Nothing AndAlso err.Message.Contains("Fichier demandé inexistant") Then
                tbl = sousEpisodeSousType.getContenuModel(loginRequestLog)
                isFusionTodo = True ' 
                Return tbl
            End If
            Throw err
        End Try
    End Function

    Private Function ConstitueFusion() As List(Of SousEpisodeFusion)
        Dim lstFusion = New List(Of SousEpisodeFusion)(1)
        Dim sousEF As New SousEpisodeFusion
        Dim episodeParametreDao = New EpisodeParametreDao
        Dim uniteSanitaireDao = New UniteSanitaireDao
        Dim siteDao = New SiteDao
        Dim parcoursDaoDao = New ParcoursDao
        Dim contexteDao = New ContexteDao
        Dim antecedentDao = New AntecedentDao
        Dim traitementDao = New TraitementDao

        ' -- recherche de l'unite sanitaire et du site du patient
        Dim uniteSanitaire = uniteSanitaireDao.getUniteSanitaireById(patient.PatientUniteSanitaireId)
        Dim site = siteDao.getSiteById(patient.PatientSiteId)
        Dim lstContexte = contexteDao.GetListOfContextebyPatient(patient.PatientId)
        Dim lstAntecedent = antecedentDao.GetListOfAntecedentPatient(patient.PatientId)
        Dim lstTraitement = traitementDao.GetListOfTraitementPatient(patient.PatientId)
        Dim sousType As SousEpisodeSousType = TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType)
        Dim intervenant As IntervenantParcours
        intervenant = If(DropDownDestinataire.SelectedIndex = -1,
                        New IntervenantParcours,
                       TryCast(Me.DropDownDestinataire.SelectedItem.Value, IntervenantParcours))

        'update Sous-Episode
        sousEpisode = sousEpisodeDao.GetById(sousEpisode.Id)

        With sousEF
            .USNom = uniteSanitaire.Oa_unite_sanitaire_description
            .USAdr1 = uniteSanitaire.Oa_unite_sanitaire_adresse1
            .USAdr2 = uniteSanitaire.Oa_unite_sanitaire_adresse2
            .USCP = uniteSanitaire.Oa_unite_sanitaire_code_postal
            .USVille = uniteSanitaire.Oa_unite_sanitaire_ville
            .USTel = uniteSanitaire.Telephone
            .USFax = uniteSanitaire.Fax
            .USEmail = uniteSanitaire.Mail

            .SiteNom = site.Oa_site_description
            .SiteAdr1 = site.Oa_site_adresse1
            .SiteAdr2 = site.Oa_site_adresse2
            .SiteCP = site.Oa_site_code_postal
            .SiteVille = site.Oa_site_ville
            .SiteTel = site.Telephone
            .SiteFax = site.Fax
            .SiteEmail = site.Mail

            .IntervenantNom = Coalesce(intervenant.Nom, "")
            .IntervenantSpecialite = Coalesce(intervenant.Specialite, "")
            .IntervenantStructure = Coalesce(intervenant.Structure, "")

            ' -- alimente automatiquement tous les parametres (poids, FC ..etc) 
            episodeParametreDao.AlimenteFusionDocumentParametres(sousEF, sousEpisode.EpisodeId, patient.PatientId)

            .Patient_Prenom = patient.PatientPrenom
            .Patient_Nom = patient.PatientNom
            .Patient_Addresse_1 = patient.PatientAdresse1
            .Patient_Addresse_2 = patient.PatientAdresse2
            .Patient_Ville = patient.PatientVille
            .Patient_Tel_1 = patient.PatientTel1
            .Patient_Tel_2 = patient.PatientTel2
            .Patient_PrenomNom = patient.PatientPrenom & " " & patient.PatientNom
            .Patient_NIR = patient.PatientNir
            .Patient_Date_Naissance = patient.PatientDateNaissance.ToString("dd/MM/yyyy")
            .Patient_Age = patient.PatientAge
            '.Patient_Poids = "" & episodeParametreDao.getPoidsByEpisodeIdOrLastKnow(sousEpisode.EpisodeId, patient.patientId)
            .Patient_sexe = patient.PatientGenre.ToLower
            .Commentaire = sousEpisode.Commentaire

            .Episode_DateHeure = episode.DateCreation.ToString("dd MMMM yyyy à hh:mm")
            .Type_Libelle = Me.DropDownType.SelectedItem.Text
            .Sous_Type_Libelle = sousType.Libelle

            ' -- Ajout de la Reference
            .Reference = sousEpisode.Reference

            ' -- recherche des sous-type_detail (sousoustype)
            Dim isWithALD = False, isWithNonAld = False, isOnlyOne = False, nbSelected = 0
            If Me.RadSousSousTypeGrid.Rows.Count > 0 Then
                Dim isSautLigneAvant = True
                For Each row In RadSousSousTypeGrid.Rows
                    If row.Cells("ChkChoice").Value Then nbSelected += 1
                    If nbSelected > 5 Then isSautLigneAvant = False : Exit For
                Next
                If nbSelected = 1 Then isSautLigneAvant = False : isOnlyOne = True
                For Each row In RadSousSousTypeGrid.Rows
                    If row.Cells("ChkChoice").Value Then
                        If row.Cells("ChkALD").Value Then ' ALD
                            isWithALD = True
                            If isSautLigneAvant Then .Sous_Type_Libelle_Detail_ALD += vbCrLf
                            .Sous_Type_Libelle_Detail_ALD += (row.Cells("Libelle").Value & If(isOnlyOne, "", vbCrLf))
                            If isSautLigneAvant Then .Sous_Type_Libelle_Detail_commentaire_ALD += vbCrLf
                            .Sous_Type_Libelle_Detail_commentaire_ALD += (row.Cells("Commentaire").Value & If(isOnlyOne, "", vbCrLf))
                        Else
                            isWithNonAld = True
                            If isSautLigneAvant Then .Sous_Type_Libelle_Detail_Non_ALD += vbCrLf
                            .Sous_Type_Libelle_Detail_Non_ALD += (row.Cells("Libelle").Value & If(isOnlyOne, "", vbCrLf))
                            If isSautLigneAvant Then .Sous_Type_Libelle_Detail_commentaire_non_ALD += vbCrLf
                            .Sous_Type_Libelle_Detail_commentaire_non_ALD += (row.Cells("Commentaire").Value & If(isOnlyOne, "", vbCrLf))
                        End If
                    End If
                Next
            Else
                isWithALD = Me.ChkALD.Checked
                isWithNonAld = Not isWithALD
            End If

            ' -- gestion des entete et fairefaire et signature
            If isWithALD Then
                .ALD_Avec_FaireFaire = sousType.Commentaire
                .Signataire_Fonction_ALD = "@Signataire_Fonction"
                .Signataire_PrenomNom_ALD = "@Signataire_PrenomNom"
            Else
                .ALD_Avec_Entete = ""
                .Signataire_Fonction_ALD = ""
                .Signataire_PrenomNom_ALD = ""
            End If

            If isWithNonAld Then
                .ALD_Sans_FaireFaire = sousType.Commentaire
                .Signataire_Fonction = "@Signataire_Fonction"
                .Signataire_PrenomNom = "@Signataire_PrenomNom"
            Else
                .ALD_Sans_FaireFaire = ""
                .Signataire_Fonction = ""
                .Signataire_PrenomNom = ""
            End If
            ' -- pas d'entete sur le non ald si pas d'ald
            If isWithALD = False Then .ALD_Sans_Entete = ""

            ' --- gestion des context
            Dim isNotFirst = False
            For Each contexte In lstContexte
                If isNotFirst Then .Contexte += vbCrLf
                .Contexte += contexte.Description
                isNotFirst = True
            Next
            isNotFirst = False
            For Each antecedent In lstAntecedent
                If isNotFirst Then .Antecedent += vbCrLf
                .Antecedent += antecedent.Description
                isNotFirst = True
            Next
            isNotFirst = False
            For Each traitement In lstTraitement
                If isNotFirst Then .Traitement += vbCrLf
                .Traitement += traitement.Denomination & " : " & traitement.Posologie
                isNotFirst = True
            Next

            '.Signature_Date = Date.Now.ToString("dd MMM yyyy")
            '.Signataire_Fonction = userLog.UtilisateurProfilId.ToLower.Trim.Replace("_", " ")
            '.Signataire_PrenomNom = userLog.UtilisateurPrenom.Trim & " " & userLog.UtilisateurNom.Trim

            .Signature_Date = "@Signature_Date"

        End With
        lstFusion.Add(sousEF)

        Return lstFusion
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idType"></param>
    Private Sub initSousTypes(idType As Long)
        Me.DropDownSousType.Items.Clear()

        For Each sousEpisodeSousType As SousEpisodeSousType In lstSousEpisodeSousType
            If sousEpisodeSousType.IdSousEpisodeType <> idType Then Continue For ' pas pour ce type d'episode
            If isCreation = False AndAlso sousEpisodeSousType.IsUserLogRedactionAutorise(userLog) = False Then Continue For ' pas autorisé

            Dim radListItemST As New RadListDataItem(sousEpisodeSousType.Libelle, sousEpisodeSousType)
            Me.DropDownSousType.Items.Add(radListItemST)
            If isCreation = False AndAlso sousEpisodeSousType.Id = sousEpisode.IdSousEpisodeSousType Then
                radListItemST.Selected = True
                'initSousSousTypes(sousEpisodeSousType.Id)
            End If
        Next
        If DropDownSousType.SelectedItem Is Nothing AndAlso DropDownSousType.Items.Count > 0 Then
            Me.DropDownSousType.SelectedItem = Me.DropDownSousType.Items(0)
        End If

        Me.DropDownSousType.Enabled = isCreation
        Me.DropDownSousType.DefaultItemsCountInDropDown = DropDownSousType.Items.Count

    End Sub

    Private Sub MasterTemplate_CellValueChanged(sender As Object, e As GridViewCellEventArgs) Handles RadSousSousTypeGrid.CellValueChanged
        If e.Column.Name = "ChkALD" Then
            e.Row.Cells("ChkChoice").Value = e.Value
        End If
    End Sub

    Private Sub BtnMail_Click(sender As Object, e As EventArgs) Handles BtnMail.Click
        ' -- 1) telechargement du sous-episode
        Cursor.Current = Cursors.WaitCursor
        If Me.DropDownSousType.SelectedItem Is Nothing Then Return
        Dim sousType = (TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType))
        Dim tblByte As Byte()
        Try
            tblByte = Telecharger_SousEpisodeDemande(sousType)
        Catch ex As Exception
            MessageBox.Show(ex.Message())
            Return
        Finally
            Cursor.Current = Cursors.Default
        End Try

        Dim mailOasis As New MailOasis
        mailOasis.Contenu = tblByte
        mailOasis.Filename = "SousEpidode.docx"
        mailOasis.IsSousEpisode = True
        mailOasis.Type = ParametreMail.TypeMailParams.SOUS_EPISODE

        ' -- 2) lancement du formulaire de choix du destinataire
        Me.Enabled = False
        Try
            Cursor.Current = Cursors.WaitCursor
            Using frm = New FrmMailSousEpisodeOuSynthese(patient, sousEpisode, mailOasis)
                frm.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            Cursor.Current = Cursors.Default
            Me.Enabled = True
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idSousType"></param>
    Private Sub initSousSousTypes(idSousType As Long)
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim numRowGrid As Integer = 0

            RadSousSousTypeGrid.Rows.Clear()

            For Each sousEpisodeSousSousType As SousEpisodeSousSousType In lstSousEpisodeSousSousType
                If sousEpisodeSousSousType.IdSousEpisodeSousType <> idSousType Then Continue For ' pas pour ce sous type d'episode
                If Not isCreation AndAlso sousEpisode.IsThisSousSousTypePresent(sousEpisodeSousSousType.Id) = False Then Continue For
                RadSousSousTypeGrid.Rows.Add(numRowGrid)
                '------------------- Alimentation du DataGridView
                With RadSousSousTypeGrid.Rows(numRowGrid)
                    .Cells("Id").Value = sousEpisodeSousSousType.Id
                    .Cells("IdSousEpisodeSousType").Value = sousEpisodeSousSousType.IdSousEpisodeSousType
                    .Cells("Libelle").Value = sousEpisodeSousSousType.Libelle
                    If Not isCreation Then
                        .Cells("ChkALD").Value = sousEpisode.IsThisDetailALD(sousEpisodeSousSousType.Id)
                        .Cells("ChkChoice").Value = True
                    End If
                    .Cells("Commentaire").Value = sousEpisodeSousSousType.Commentaire
                End With

                numRowGrid += 1

            Next
            Me.SplitPanelSousSousType.Collapsed = (numRowGrid = 0)
            Me.Cursor = Cursors.Default
        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub initDestinataire(sousEpisodeType As SousEpisodeType)
        Me.LblDestinataire.Visible = sousEpisodeType.IsWithDestinataire()
        Me.DropDownDestinataire.Visible = sousEpisodeType.IsWithDestinataire()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub initALDetReponse()
        If Me.DropDownSousType.SelectedItem IsNot Nothing Then
            Dim sousType As SousEpisodeSousType = TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType)
            Dim visible = sousType.IsALDPossible AndAlso isPatientALD
            ChkALD.Visible = visible
            LblALD.Visible = visible
            RadSousSousTypeGrid.Columns("ChkALD").IsVisible = visible
            RadSousSousTypeGrid.Columns("ChkChoice").IsVisible = isCreation

            If isCreation Then
                ChkBReponseAttendue.Checked = sousType.IsReponseRequise
                'ChkBReponseAttendue.Enabled = Not sousType.IsReponseRequise
            End If
        End If
    End Sub

    Private Function serializeSousEpisode() As Boolean

        With sousEpisode
            .HorodateCreation = DateTime.Now
            .EpisodeId = episode.Id
            .IdSousEpisodeType = TryCast(Me.DropDownType.SelectedItem.Value, SousEpisodeType).Id
            .IdSousEpisodeSousType = TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType).Id
            .IdIntervenant = If(Me.DropDownDestinataire.SelectedIndex = -1, 0, TryCast(Me.DropDownDestinataire.SelectedItem.Value, IntervenantParcours).IntervenantId)
            .CreateUserId = userLog.UtilisateurId
            .Commentaire = TxtRDVCommentaire.Text
            .IsALD = ChkALD.Checked AndAlso TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType).IsALDPossible And isPatientALD
            .IsReponse = ChkBReponseAttendue.Checked
            .DelaiSinceValidation = TxtDelai.Value

            Dim lstDetail As New List(Of SousEpisodeDetailSousType)
            Dim sousEpisodeDetail As SousEpisodeDetailSousType
            Dim isSousTypeDetailALD As Boolean = .IsALD
            For Each row In RadSousSousTypeGrid.Rows
                If row.Cells("ChkChoice").Value Then
                    sousEpisodeDetail = New SousEpisodeDetailSousType()
                    sousEpisodeDetail.IdSousEpisode = sousEpisode.EpisodeId
                    sousEpisodeDetail.IdSousEpisodeSousSousType = row.Cells("Id").Value
                    sousEpisodeDetail.IsALD = row.Cells("ChkALD").Value
                    If sousEpisodeDetail.IsALD Then
                        isSousTypeDetailALD = True
                        .IsALD = True
                    End If

                    lstDetail.Add(sousEpisodeDetail)
                End If
            Next
            If RadSousSousTypeGrid.Rows.Count > 0 AndAlso isSousTypeDetailALD <> .IsALD Then .IsALD = isSousTypeDetailALD
            If lstDetail.Count = 0 AndAlso RadSousSousTypeGrid.Rows.Count > 0 Then
                MsgBox("Vous devez choisir au moins un élément dans le tableau des détails ! ")
                Return False
            End If
            .lstDetail = lstDetail
        End With


        If sousEpisodeDao.Create(sousEpisode) = False Then
            Return False
        End If

        ' --- reaffiche le formulaire en mode update
        isCreation = False
        initControls()
        Return True
    End Function

End Class
