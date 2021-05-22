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

    Dim lstSousEpisodeSousType As List(Of SousEpisodeSousType)
    Dim lstSousEpisodeSousSousType As List(Of SousEpisodeSousSousType)

    Private Sub FrmSousEpisodeReponseAttribution_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Liste des mails a traiter", userLog)

        lstSousEpisodeSousType = sousEpisodeSousTypeDao.getLstSousEpisodeSousType()
        lstSousEpisodeSousSousType = sousEpisodeSousSousTypeDao.getLstSousEpisodeSousSousType()

        refreshGrid()
        InitFrm()
    End Sub

    Private Sub InitFrm()
        RadTextBoxPrenom.Text = Nothing
        RadTextBoxNom.Text = Nothing
        RadDateTimeDDN.Value = RadDateTimeDDN.MinDate
        RadDateTimeDDN.Format = DateTimePickerFormat.Custom
        RadDateTimeDDN.CustomFormat = " "
    End Sub

    Private Sub refreshGrid()
        Me.Cursor = Cursors.WaitCursor
        Dim sousEpisodeReponseMailDao As SousEpisodeReponseMailDao = New SousEpisodeReponseMailDao
        Try
            Dim lstMail As List(Of SousEpisodeReponseMail) = sousEpisodeReponseMailDao.GetLstSousEpisodeReponseMail()
            Dim numRowGrid As Integer = 0
            ' -- recup eventuelle precedente selectionnée
            'If RadSousEpisodeGrid.Rows.Count > 0 AndAlso Not IsNothing(Me.RadSousEpisodeGrid.CurrentRow) Then
            '    exId = Me.RadSousEpisodeGrid.CurrentRow.Cells("IdSousEpisode").Value
            '    exPosit = Me.RadSousEpisodeGrid.CurrentRow.Index
            'End If
            RadMailGridView.Rows.Clear()
            For Each mail In lstMail
                Dim newRow As GridViewRowInfo = RadMailGridView.Rows.NewRow()
                With newRow
                    .Cells("objet").Value = mail.Objet
                    .Cells("auteur").Value = mail.Auteur
                    .Cells("date").Value = mail.HorodateCreation.ToShortDateString
                End With
                RadMailGridView.Rows.Add(newRow)
                numRowGrid += 1
            Next
            Me.Cursor = Cursors.Default
        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try
    End Sub

    Private Sub ChargementSousEpisodes(sousEpisodes As List(Of SousEpisode))
        RadGridViewSousEpisode.Rows.Clear()
        RadGridViewSousEpisode.Enabled = False

        Dim iGrid As Integer = 0
        For Each sousEpisode As SousEpisode In sousEpisodes
            Dim Text = ""
            RadGridViewSousEpisode.Rows.Add(iGrid)
            RadGridViewSousEpisode.Rows(iGrid).Cells("sousType").Value = sousEpisode.SousTypeLibelle
            sousEpisode.lstDetail = sousEpisodeDetailSousTypeDao.getLstSousEpisodeDetailSousType(sousEpisode.Id)
            For Each sousEpisodeSousType As SousEpisodeSousType In lstSousEpisodeSousType
                If sousEpisodeSousType.Id <> sousEpisode.IdSousEpisodeSousType Then Continue For
                For Each sousEpisodeSousSousType As SousEpisodeSousSousType In lstSousEpisodeSousSousType
                    If sousEpisodeSousSousType.IdSousEpisodeSousType <> sousEpisodeSousType.Id Then Continue For
                    If sousEpisode.IsThisSousSousTypePresent(sousEpisodeSousSousType.Id) = False Then Continue For
                    Text += sousEpisodeSousSousType.Libelle & vbCrLf
                Next
            Next
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
            RadGridViewEpisode.Rows(iGrid).Cells("conclusion").Value = episode.ConclusionMedConsigneDenomination
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
        Dim id As Integer = RadGridViewPatient.Rows(Me.RadGridViewPatient.Rows.IndexOf(Me.RadGridViewPatient.CurrentRow)).Cells("id").Value
        Dim selectedPatient As Patient = patientDao.GetPatient(id)
        Using vRadFEpisodeDetailCreation As New RadFEpisodeDetailCreation
            vRadFEpisodeDetailCreation.SelectedPatient = selectedPatient
            vRadFEpisodeDetailCreation.EpisodeType = Episode.EnumTypeEpisode.VIRTUEL
            vRadFEpisodeDetailCreation.ShowDialog()
        End Using
    End Sub

    Private Sub RadButtonCSE_Click(sender As Object, e As EventArgs) Handles RadButtonCSE.Click
        Dim patientId As Integer = RadGridViewPatient.Rows(Me.RadGridViewPatient.Rows.IndexOf(Me.RadGridViewPatient.CurrentRow)).Cells("id").Value
        Dim selectedPatient As Patient = patientDao.GetPatient(patientId)
        Dim episodeId As Integer = RadGridViewPatient.Rows(Me.RadGridViewPatient.Rows.IndexOf(Me.RadGridViewPatient.CurrentRow)).Cells("id").Value
        Dim selectedEpisode As Episode = episodeDao.GetEpisodeById(episodeId)
        Dim sousEpisode As SousEpisode
        Using frm = New FrmSousEpisode(selectedEpisode, selectedPatient, sousEpisode, "", "", "")
            frm.ShowDialog()
            frm.Dispose()
        End Using
    End Sub

    '
    ' CELLCLICK
    '

    Private Sub RadGridViewPatient_CellClick(sender As Object, e As GridViewCellEventArgs) Handles RadGridViewPatient.CellClick
        Dim id As Integer = RadGridViewPatient.Rows(Me.RadGridViewPatient.Rows.IndexOf(Me.RadGridViewPatient.CurrentRow)).Cells("id").Value
        Dim episodes As List(Of Episode) = episodeDao.GetAllEpisodeByPatient(id)
        ChargementEpisodes(episodes)
        RadButtonCEV.Enabled = True
        RadButtonCSE.Enabled = False
    End Sub

    Private Sub RadGridViewEpisode_CellClick(sender As Object, e As GridViewCellEventArgs) Handles RadGridViewEpisode.CellClick
        Dim id As Integer = RadGridViewEpisode.Rows(Me.RadGridViewEpisode.Rows.IndexOf(Me.RadGridViewEpisode.CurrentRow)).Cells("id").Value
        Dim sousEpisodes As List(Of SousEpisode) = sousEpisodeDao.GetAllSousEpisodeByPatient(id)
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

End Class
