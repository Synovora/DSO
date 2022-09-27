﻿Imports System.Configuration
Imports System.Diagnostics
Imports System.IO
Imports MimeKit
Imports Oasis_Common
Imports Telerik.WinControls
Imports Telerik.WinControls.UI

Public Class FrmSousEpisodeReponseAttribution
    Dim minInputLength As Integer = 3

    ReadOnly patientDao As New PatientDao
    ReadOnly episodeDao As New EpisodeDao
    ReadOnly sousEpisodeDao As New SousEpisodeDao
    ReadOnly sousEpisodeSousTypeDao As New SousEpisodeSousTypeDao
    ReadOnly sousEpisodeSousSousTypeDao As New SousEpisodeSousSousTypeDao
    ReadOnly sousEpisodeDetailSousTypeDao As New SousEpisodeDetailSousTypeDao
    ReadOnly sousEpisodeReponseMailAttachmentDao As New SousEpisodeReponseMailAttachmentDao
    ReadOnly sousEpisodeReponseMailDao As New SousEpisodeReponseMailDao
    ReadOnly sousEpisodeReponseDao As New SousEpisodeReponseDao
    ReadOnly rorDao As New RorDao

    Dim lstSousEpisodeSousType As List(Of SousEpisodeSousType)
    Dim lstSousEpisodeSousSousType As List(Of SousEpisodeSousSousType)
    Dim lstMail As List(Of SousEpisodeReponseMail)
    Dim lstAttachment As List(Of SousEpisodeReponseMailAttachment)

    Private Sub FrmSousEpisodeReponseAttribution_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Liste des mails a traiter", userLog)

        lstSousEpisodeSousType = sousEpisodeSousTypeDao.getLstSousEpisodeSousType()
        lstSousEpisodeSousSousType = sousEpisodeSousSousTypeDao.getLstSousEpisodeSousSousType()

        ChargementMails()
        InitFrm()
    End Sub

    Private Sub InitFrm()
        RadTextBoxPrenom.Text = Nothing
        RadTextBoxNom.Text = Nothing
        RadDateTimeDDN.Value = RadDateTimeDDN.MinDate
        RadDateTimeDDN.Format = DateTimePickerFormat.Custom
        RadDateTimeDDN.CustomFormat = " "
    End Sub

    Private Sub ChargementMails()
        Me.Cursor = Cursors.WaitCursor
        Dim sousEpisodeReponseMailDao As SousEpisodeReponseMailDao = New SousEpisodeReponseMailDao
        Try
            lstMail = sousEpisodeReponseMailDao.GetLstSousEpisodeReponseMail()
            Dim numRowGrid As Integer = 0
            ' -- recup eventuelle precedente selectionnée
            'If RadSousEpisodeGrid.Rows.Count > 0 AndAlso Not IsNothing(Me.RadSousEpisodeGrid.CurrentRow) Then
            '    exId = Me.RadSousEpisodeGrid.CurrentRow.Cells("IdSousEpisode").Value
            '    exPosit = Me.RadSousEpisodeGrid.CurrentRow.Index
            'End If
            RadGridViewMail.Rows.Clear()
            For Each mail In lstMail
                If mail.Status = "processed" Then
                    Continue For
                End If
                Dim newRow As GridViewRowInfo = RadGridViewMail.Rows.NewRow()
                With newRow
                    .Cells("id").Value = mail.Id
                    .Cells("objet").Value = mail.Objet
                    .Cells("auteur").Value = mail.Auteur
                    .Cells("date").Value = mail.HorodateCreation.ToShortDateString
                End With
                RadGridViewMail.Rows.Add(newRow)
                numRowGrid += 1
            Next
            Me.Cursor = Cursors.Default
        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try

        RadAttachmentGridView.Rows.Clear()
        RadAttachmentGridView.DataSource = Nothing
        RadAttachmentGridView.Refresh()
        WebBrowser.DocumentText = ""

        RadGridViewMail.ClearSelection()
    End Sub

    Private Sub ChargementSousEpisodes(sousEpisodes As List(Of SousEpisode))
        RadGridViewSousEpisode.Rows.Clear()
        RadGridViewSousEpisode.Enabled = False

        Dim iGrid As Integer = 0
        For Each sousEpisode As SousEpisode In sousEpisodes
            Dim Text = ""
            RadGridViewSousEpisode.Rows.Add(iGrid)
            RadGridViewSousEpisode.Rows(iGrid).Cells("sousType").Value = sousEpisode.SousTypeLibelle
            RadGridViewSousEpisode.Rows(iGrid).Cells("id").Value = sousEpisode.Id
            sousEpisode.lstDetail = sousEpisodeDetailSousTypeDao.getLstSousEpisodeDetailSousType(sousEpisode.Id)
            For Each sousEpisodeSousType As SousEpisodeSousType In lstSousEpisodeSousType
                If sousEpisodeSousType.Id <> sousEpisode.IdSousEpisodeSousType Then Continue For
                For Each sousEpisodeSousSousType As SousEpisodeSousSousType In lstSousEpisodeSousSousType
                    If sousEpisodeSousSousType.IdSousEpisodeSousType <> sousEpisodeSousType.Id Then Continue For
                    If sousEpisode.IsThisSousSousTypePresent(sousEpisodeSousSousType.Id) = False Then Continue For
                    Text += sousEpisodeSousSousType.Libelle & vbCrLf
                Next
            Next

            If sousEpisode.SousTypeLibelle = "Courrier" Then
                Dim intervenant = rorDao.GetRorById(sousEpisode.IdIntervenant)
                Dim specialite = Table_specialite.GetSpecialiteDescription(intervenant.SpecialiteId)
                Text = "Intervenant: " & intervenant.Nom & " - " & specialite
            End If

            RadGridViewSousEpisode.Rows(iGrid).Cells("sousType").Tag = Text
            iGrid += 1
        Next

        If RadGridViewSousEpisode.Rows.Count > 0 Then
            Me.RadGridViewSousEpisode.CurrentRow = RadGridViewSousEpisode.Rows(0)
        End If

        RadGridViewSousEpisode.Enabled = True
    End Sub

    Private Sub ChargementEpisodes(episodes As List(Of Episode))
        RadGridViewEpisode.Rows.Clear()
        RadGridViewSousEpisode.Rows.Clear()
        RadGridViewEpisode.Enabled = False
        RadGridViewSousEpisode.Enabled = False

        Dim iGrid As Integer = 0
        For Each episode As Episode In episodes
            If episode.Type = "PARAMETRE" Then
                Continue For
            End If
            RadGridViewEpisode.Rows.Add(iGrid)
            RadGridViewEpisode.Rows(iGrid).Cells("id").Value = episode.Id
            RadGridViewEpisode.Rows(iGrid).Cells("date").Value = episode.DateCreation.ToShortDateString
            RadGridViewEpisode.Rows(iGrid).Cells("type").Value = episode.Type
            RadGridViewEpisode.Rows(iGrid).Cells("conclusion").Value = episode.ObservationMedical
            iGrid += 1
        Next

        If RadGridViewEpisode.Rows.Count > 0 Then
            Me.RadGridViewEpisode.CurrentRow = RadGridViewEpisode.Rows(0)
        End If

        RadGridViewEpisode.Enabled = True
    End Sub

    Private Sub ChargementPatients()
        RadGridViewPatient.Rows.Clear()
        RadGridViewSousEpisode.Rows.Clear()
        RadGridViewEpisode.Rows.Clear()
        RadGridViewPatient.Enabled = True
        RadGridViewEpisode.Enabled = False
        RadGridViewSousEpisode.Enabled = False

        Dim patients As List(Of Patient) = patientDao.GetFilteredPatient(RadTextBoxPrenom.Text, RadTextBoxNom.Text, If(RadDateTimeDDN.Value = RadDateTimeDDN.MinDate, Nothing, RadDateTimeDDN.Value))

        Dim iGrid As Integer = -1
        For Each patient As Patient In patients
            iGrid += 1
            RadGridViewPatient.Rows.Add(iGrid)
            RadGridViewPatient.Rows(iGrid).Cells("prenom").Value = patient.PatientPrenom
            RadGridViewPatient.Rows(iGrid).Cells("nom").Value = patient.PatientNom
            RadGridViewPatient.Rows(iGrid).Cells("id").Value = patient.PatientId
        Next

        If RadGridViewPatient.Rows.Count > 0 Then
            Me.RadGridViewPatient.CurrentRow = RadGridViewPatient.Rows(0)
        End If
    End Sub

    '
    ' CHANGED
    '

    Private Sub RadDateTimeDDN_ValueChanged(sender As Object, e As EventArgs) Handles RadDateTimeDDN.ValueChanged
        If RadDateTimeDDN.Value <> RadDateTimeDDN.MaxDate Then
            RadDateTimeDDN.Format = DateTimePickerFormat.Long
        End If
    End Sub

    Private Sub RadTextBoxPrenom_TextChanged(sender As Object, e As EventArgs) Handles RadTextBoxPrenom.TextChanged
        If RadTextBoxPrenom.Text.Length >= minInputLength Then
            ChargementPatients()
        End If
    End Sub

    Private Sub RadTextBoxNom_TextChanged(sender As Object, e As EventArgs) Handles RadTextBoxNom.TextChanged
        If RadTextBoxNom.Text.Length >= minInputLength Then
            ChargementPatients()
        End If
    End Sub

    '
    ' CLICK
    '

    Private Sub RadButtonResearch_Click(sender As Object, e As EventArgs) Handles RadButtonResearch.Click
        ChargementPatients()
    End Sub

    Private Sub RadButtonReset_Click(sender As Object, e As EventArgs) Handles RadButtonReset.Click
        InitFrm()
        RadGridViewSousEpisode.Rows.Clear()
        RadGridViewEpisode.Rows.Clear()
        RadGridViewPatient.Rows.Clear()
        RadButtonCEV.Enabled = False
        RadButtonCSE.Enabled = False
        RadButtonAttribution.Enabled = False
    End Sub
    Private Sub RadButtonResetDate_Click(sender As Object, e As EventArgs) Handles RadButtonResetDate.Click
        RadDateTimeDDN.Value = RadDateTimeDDN.MinDate
        RadDateTimeDDN.Format = DateTimePickerFormat.Custom
        RadDateTimeDDN.CustomFormat = " "
    End Sub

    Private Sub RadButtonCEV_Click(sender As Object, e As EventArgs) Handles RadButtonCEV.Click
        Dim patientId As Integer = RadGridViewPatient.Rows(Me.RadGridViewPatient.Rows.IndexOf(Me.RadGridViewPatient.CurrentRow)).Cells("id").Value
        Dim selectedPatient As Patient = patientDao.GetPatient(patientId)
        Me.Enabled = False
        Try
            Using vRadFEpisodeDetailCreation As New RadFEpisodeDetailCreation
                vRadFEpisodeDetailCreation.SelectedPatient = selectedPatient
                vRadFEpisodeDetailCreation.EpisodeType = Episode.EnumTypeEpisode.VIRTUEL
                vRadFEpisodeDetailCreation.ShowDialog()
                If vRadFEpisodeDetailCreation.CodeRetour = True Then
                    ChargementEpisodes(episodeDao.GetAllEpisodeByPatient(patientId))
                    For Each item In RadGridViewEpisode.MasterView.ChildRows
                        If item.Cells("id").Value = vRadFEpisodeDetailCreation.EpisodeId.ToString() Then
                            item.IsSelected = True
                        End If
                    Next

                    Dim selectedEpisode As Episode = episodeDao.GetEpisodeById(vRadFEpisodeDetailCreation.EpisodeId)
                    Using frm = New FrmSousEpisode(selectedEpisode, selectedPatient, New SousEpisode, "", "", "")
                        frm.ShowDialog()
                        frm.Dispose()
                        RadGridViewEpisode_CellClick(Nothing, Nothing)
                        RadGridViewSousEpisode.MasterView.ChildRows.Last.IsSelected = True
                        Using vRadFEpisodeDetail As New RadFEpisodeDetail
                            vRadFEpisodeDetail.SelectedEpisodeId = vRadFEpisodeDetailCreation.EpisodeId
                            vRadFEpisodeDetail.SelectedPatient = selectedPatient
                            vRadFEpisodeDetail.RendezVousId = 0
                            vRadFEpisodeDetail.UtilisateurConnecte = userLog
                            vRadFEpisodeDetail.Editable = True
                            vRadFEpisodeDetail.ShowDialog()
                        End Using
                        RadGridViewSousEpisode_CellClick(Nothing, Nothing)
                    End Using
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Me.Enabled = True
        Dim episodes As List(Of Episode) = episodeDao.GetAllEpisodeByPatient(patientId)
        ChargementEpisodes(episodes)
    End Sub

    Private Sub RadButtonCSE_Click(sender As Object, e As EventArgs) Handles RadButtonCSE.Click
        Dim patientId As Integer = RadGridViewPatient.Rows(Me.RadGridViewPatient.Rows.IndexOf(Me.RadGridViewPatient.CurrentRow)).Cells("id").Value
        Dim selectedPatient As Patient = patientDao.GetPatient(patientId)
        Dim episodeId As Integer = RadGridViewEpisode.Rows(Me.RadGridViewEpisode.Rows.IndexOf(Me.RadGridViewEpisode.CurrentRow)).Cells("id").Value
        Dim selectedEpisode As Episode = episodeDao.GetEpisodeById(episodeId)
        Me.Enabled = False
        Using frm = New FrmSousEpisode(selectedEpisode, selectedPatient, New SousEpisode, "", "", "")
            frm.ShowDialog()
            frm.Dispose()
        End Using
        Me.Enabled = True
        Dim sousEpisodes As List(Of SousEpisode) = sousEpisodeDao.GetAllSousEpisodeByPatient(episodeId)
        ChargementSousEpisodes(sousEpisodes)
    End Sub

    '
    ' CELLCLICK
    '

    Private Sub RadAttachmentGridView_CellClick(sender As Object, e As GridViewCellEventArgs) Handles RadAttachmentGridView.CellClick
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim attachment = sousEpisodeReponseMailAttachmentDao.GetSousEpisodeReponseMailAttachmentById(RadAttachmentGridView.Rows(Me.RadAttachmentGridView.Rows.IndexOf(Me.RadAttachmentGridView.CurrentRow)).Cells("id").Value)

            Dim tbl As Byte() = sousEpisodeReponseDao.getContenu(attachment.GetLocalName(), loginRequestLog)

            Dim pathDownload = ConfigurationManager.AppSettings("CheminTelechargement")
            If (Not System.IO.Directory.Exists(pathDownload)) Then
                System.IO.Directory.CreateDirectory(pathDownload)
            End If

            File.WriteAllBytes(pathDownload & "\" & attachment.Filename, tbl)
            Dim proc As New Process()
            Try
                proc.StartInfo.FileName = pathDownload & "\" & attachment.Filename
                proc.Start()
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

    Private Sub RadMailGridView_CellClick(sender As Object, e As GridViewCellEventArgs) Handles RadGridViewMail.CellClick
        'Clear
        RadObjetTextBox.Clear()
        RadAttachmentGridView.Rows.Clear()
        WebBrowser.DocumentText = ""

        Dim id As Integer = RadGridViewMail.Rows(Me.RadGridViewMail.Rows.IndexOf(Me.RadGridViewMail.CurrentRow)).Cells("id").Value
        Dim sousEpisodeReponseMail As SousEpisodeReponseMail = sousEpisodeReponseMailDao.GetSousEpisodeReponseMailById(id)

        Try
            Me.Cursor = Cursors.WaitCursor
            lstAttachment = sousEpisodeReponseMailAttachmentDao.GetSousEpisodeReponseMailAttachmentByMailId(sousEpisodeReponseMail.Id)
            Dim attachment = lstAttachment(0)

            Dim tbl As Byte() = sousEpisodeReponseDao.getContenu(attachment.GetLocalName(), loginRequestLog)

            Dim pathDownload = ConfigurationManager.AppSettings("CheminTelechargement")
            If (Not System.IO.Directory.Exists(pathDownload)) Then
                System.IO.Directory.CreateDirectory(pathDownload)
            End If
            Dim path = pathDownload & "\" & attachment.Filename

            File.WriteAllBytes(path, tbl)
            Dim proc As New Process()
            Try
                Dim message = MimeMessage.Load(path)
                RadObjetTextBox.Text = message.Subject
                WebBrowser.DocumentText = If(message.HtmlBody, message.TextBody)

                For Each attachment In lstAttachment.Skip(1)
                    Dim newRow As GridViewRowInfo = RadAttachmentGridView.Rows.NewRow()
                    With newRow
                        .Cells("id").Value = attachment.Id
                        .Cells("filename").Value = attachment.Filename
                        .Cells("localName").Value = attachment.GetLocalName()
                    End With
                    RadAttachmentGridView.Rows.Add(newRow)
                Next
                RadButtonDelete.Enabled = True
                RadButtonOpen.Enabled = True
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

    Private Sub RadGridViewPatient_CellClick(sender As Object, e As GridViewCellEventArgs) Handles RadGridViewPatient.CellClick
        Dim patientId As Integer = RadGridViewPatient.Rows(Me.RadGridViewPatient.Rows.IndexOf(Me.RadGridViewPatient.CurrentRow)).Cells("id").Value
        Dim episodes As List(Of Episode) = episodeDao.GetAllEpisodeByPatient(patientId)
        ChargementEpisodes(episodes)
        RadButtonCEV.Enabled = True
        RadButtonCSE.Enabled = False
    End Sub

    Private Sub RadGridViewEpisode_CellClick(sender As Object, e As GridViewCellEventArgs) Handles RadGridViewEpisode.CellClick
        Dim episodeId As Integer = RadGridViewEpisode.Rows(Me.RadGridViewEpisode.Rows.IndexOf(Me.RadGridViewEpisode.CurrentRow)).Cells("id").Value
        Dim sousEpisodes As List(Of SousEpisode) = sousEpisodeDao.GetAllSousEpisodeByPatient(episodeId)
        ChargementSousEpisodes(sousEpisodes)
        RadButtonCSE.Enabled = True
    End Sub


    Private Sub RadGridViewSousEpisode_CellClick(sender As Object, e As GridViewCellEventArgs) Handles RadGridViewSousEpisode.CellClick
        RadButtonAttribution.Enabled = True
    End Sub

    '
    ' TOOLTIP
    '
    Private Sub RadGridViewEpisode_ToolTipTextNeeded(sender As Object, e As ToolTipTextNeededEventArgs) Handles RadGridViewEpisode.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "conclusion" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("conclusion").Value
        End If
    End Sub

    Private Sub RadGridViewSousEpisode_ToolTipTextNeeded(sender As Object, e As ToolTipTextNeededEventArgs) Handles RadGridViewSousEpisode.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "sousType" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("sousType").Tag
        End If
    End Sub

    Private Sub RadButtonDelete_Click(sender As Object, e As EventArgs) Handles RadButtonDelete.Click
        If MsgBox("Etes-vous sur de vouloir supprimer ce mail ?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Critical, "Suppression") = MsgBoxResult.Yes Then
            Dim reponseMailId As Integer = RadGridViewMail.Rows(Me.RadGridViewMail.Rows.IndexOf(Me.RadGridViewMail.CurrentRow)).Cells("id").Value
            sousEpisodeReponseMailDao.DeleteSousEpisodeReponseMailById(reponseMailId)
            ChargementMails()
        End If
    End Sub

    Private Sub RadButtonOpen_Click(sender As Object, e As EventArgs) Handles RadButtonOpen.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Dim attachment = lstAttachment(0)

            Dim tbl As Byte() = sousEpisodeReponseDao.getContenu(attachment.GetLocalName(), loginRequestLog)

            Dim pathDownload = ConfigurationManager.AppSettings("CheminTelechargement")
            If (Not System.IO.Directory.Exists(pathDownload)) Then
                System.IO.Directory.CreateDirectory(pathDownload)
            End If

            File.WriteAllBytes(pathDownload & "\" & attachment.Filename, tbl)
            Dim proc As New Process()
            Try
                proc.StartInfo.FileName = pathDownload & "\" & attachment.Filename
                proc.Start()
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

    '
    ' MENUSTRIP
    '

    Private Sub VoirLepisodeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles VoirLepisodeToolStripMenuItem.Click
        If RadGridViewEpisode.CurrentRow IsNot Nothing Then
            Dim episodeIndex As Integer = Me.RadGridViewEpisode.Rows.IndexOf(Me.RadGridViewEpisode.CurrentRow)
            Dim patientIndex As Integer = Me.RadGridViewPatient.Rows.IndexOf(Me.RadGridViewPatient.CurrentRow)

            If episodeIndex >= 0 And patientIndex >= 0 Then
                Dim episodeId As Integer = RadGridViewEpisode.Rows(episodeIndex).Cells("id").Value
                Dim patientId As Integer = RadGridViewPatient.Rows(patientIndex).Cells("id").Value

                Dim patient = patientDao.GetPatient(patientId)

                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor

                Try
                    Using vRadFEpisodeDetail As New RadFEpisodeDetail
                        vRadFEpisodeDetail.SelectedEpisodeId = episodeId
                        vRadFEpisodeDetail.SelectedPatient = patient
                        vRadFEpisodeDetail.RendezVousId = 0
                        vRadFEpisodeDetail.UtilisateurConnecte = userLog
                        vRadFEpisodeDetail.Editable = False
                        vRadFEpisodeDetail.ShowDialog()
                    End Using
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try

                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadButtonAttribution_Click(sender As Object, e As EventArgs) Handles RadButtonAttribution.Click
        Dim reponseMailId As Integer = RadGridViewMail.Rows(Me.RadGridViewMail.Rows.IndexOf(Me.RadGridViewMail.CurrentRow)).Cells("id").Value
        Dim sousEpisodeId As Integer = RadGridViewSousEpisode.Rows(Me.RadGridViewSousEpisode.Rows.IndexOf(Me.RadGridViewSousEpisode.CurrentRow)).Cells("id").Value
        Dim sousEpisode As SousEpisode = sousEpisodeDao.GetById(sousEpisodeId)

        For Each item In RadAttachmentGridView.MasterView.ChildRows
            Dim sousEpisodeReponse As SousEpisodeReponse = New SousEpisodeReponse
            With sousEpisodeReponse
                .IdSousEpisode = sousEpisode.Id
                .CreateUserId = userLog.UtilisateurId
                .HorodateCreation = DateTime.Now
                .NomFichier = Path.GetFileName(item.Cells("filename").Value)
                .Commentaire = "reponse automatique attribuee"
            End With
            sousEpisodeReponseDao.CreateByMoving(sousEpisode, sousEpisodeReponse, item.Cells("localName").Value, loginRequestLog)
        Next

        sousEpisodeReponseMailDao.ProcessSousEpisodeReponseMailById(reponseMailId)
        ChargementMails()
        RadButtonReset_Click(Nothing, Nothing)
    End Sub

    Private Sub BtnRefreshMail_Click(sender As Object, e As EventArgs)
        ChargementMails()
    End Sub
End Class