Imports Oasis_Common

Public Class RadFActeParamedicalTest
    Private _selectedPatient As Patient

    Public Property SelectedPatient As Patient
        Get
            Return _selectedPatient
        End Get
        Set(value As Patient)
            _selectedPatient = value
        End Set
    End Property


    Dim episodeProtocoleCollaboratifDao As New EpisodeProtocoleCollaboratifDao
    Dim episodeActiviteDao As New EpisodeTypeActiviteDao
    Dim episodeDao As New EpisodeDao
    Dim drcDao As New DrcDao

    Dim drc As New Drc

    Private Sub RadFActeParamedicalTest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim listActivite As List(Of String)
        listActivite = episodeActiviteDao.GetTypeActiviteEpisodeByPatient(SelectedPatient)
        CbxActiviteEpisode.DataSource = listActivite
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        RadGridViewActePara.Rows.Clear()
        If CbxActiviteEpisode.Text <> "" Then
            Dim TypeActiviteAcode As String = episodeDao.GetCodeTypeActiviteByItem(CbxActiviteEpisode.Text)
            Dim ListActePara As List(Of Long)
            ListActePara = episodeProtocoleCollaboratifDao.GetListeActeParamedicalByPatientEtTypeEpisode(SelectedPatient.patientId, TypeActiviteAcode)
            Dim iGrid As Integer = -1
            For i = 0 To ListActePara.Count - 1
                iGrid += 1
                RadGridViewActePara.Rows.Add(iGrid)
                drcDao.GetDrc(drc, ListActePara.Item(i))
                RadGridViewActePara.Rows(iGrid).Cells("drc").Value = drc.DrcLibelle
            Next
        End If
    End Sub
End Class
