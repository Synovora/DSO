Imports Oasis_Common

Public Class RadFParametreTest
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
    Dim parametreDao As New ParametreDao

    Dim parametre As Parametre

    Private Sub RadFActeParamedicalTest_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim listActivite As List(Of String)
        listActivite = episodeActiviteDao.GetTypeActiviteEpisodeByPatient(SelectedPatient)
        CbxActiviteEpisode.DataSource = listActivite
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        RadGridViewParametre.Rows.Clear()
        If CbxActiviteEpisode.Text <> "" Then
            Dim TypeActiviteAcode As String = episodeDao.GetCodeTypeActiviteByItem(CbxActiviteEpisode.Text)
            Dim ListParametre As List(Of Long)
            ListParametre = episodeProtocoleCollaboratifDao.GetListeParametreByPatientEtTypeEpisode(SelectedPatient.patientId, TypeActiviteAcode)
            Dim iGrid As Integer = -1
            For i = 0 To ListParametre.Count - 1
                iGrid += 1
                RadGridViewParametre.Rows.Add(iGrid)
                parametre = parametreDao.GetParametreById(ListParametre.Item(i))
                RadGridViewParametre.Rows(iGrid).Cells("parametre").Value = parametre.Description
            Next
        End If
    End Sub
End Class
