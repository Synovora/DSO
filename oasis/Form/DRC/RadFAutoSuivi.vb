Imports System.Web.UI.WebControls.Expressions
Imports Oasis_Common
Imports Telerik.WinControls.UI


Public Class RadFAutoSuivi

    Private Class AutoSuiviItem
        Property PatientId As Long
        Property ParametreId As Long
        Property Description As String
        Property IsActif As Boolean
    End Class

    Property SelectedPatient As PatientBase
    Private ReadOnly episodeProtocoleCollaboratifDao As New EpisodeProtocoleCollaboratifDao
    Private ReadOnly autoSuiviDao As New AutoSuiviDao
    ReadOnly parametreDao As New ParametreDao

    ReadOnly parametres As List(Of AutoSuiviItem) = New List(Of AutoSuiviItem)
    ReadOnly TypeActiviteAcode As String = Episode.EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE

    Private Sub BuildList()
        Dim ListParametres As List(Of Long) = episodeProtocoleCollaboratifDao.GetListeParametreByPatientEtTypeEpisode(SelectedPatient.patientId, TypeActiviteAcode)
        For i = 0 To ListParametres.Count - 1
            Dim parametre = parametreDao.GetParametreById(ListParametres.Item(i))
            If parametre.ExclusionAutoSuivi = True Then
                Continue For
            End If
            Dim autoSuivi = autoSuiviDao.GetAutoSuiviByPatientIdAndParametreId(SelectedPatient.patientId, ListParametres.Item(i))

            parametres.Add(New AutoSuiviItem With {
                .PatientId = SelectedPatient.patientId,
                .ParametreId = parametre.Id,
                .Description = If(parametre.DescriptionPatient = "", parametre.Description, parametre.DescriptionPatient),
                .IsActif = autoSuivi Is Nothing
            })
        Next
    End Sub

    Private Sub RadFAutoSuivi_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RadGridViewAutoSuivi.Rows.Clear()
        buildList()
        For i = 0 To parametres.Count - 1
            RadGridViewAutoSuivi.Rows.Add(i)
            RadGridViewAutoSuivi.Rows(i).Cells("description").Value = parametres.Item(i).Description
        Next
        RadFAutoSuivi_Refresh()
    End Sub

    Private Sub RadFAutoSuivi_Refresh()
        For i = 0 To parametres.Count - 1
            RadGridViewAutoSuivi.Rows(i).Cells("actif").Value = parametres.Item(i).IsActif
        Next
    End Sub

    Private Sub RadGridViewAutoSuivi_CellDoubleClick(ByVal sender As Object, ByVal e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadGridViewAutoSuivi.CellDoubleClick
        parametres.Item(e.RowIndex).IsActif = Not (parametres.Item(e.RowIndex).IsActif)
        Dim autoSuivi = New AutoSuivi With {
                .PatientId = parametres.Item(e.RowIndex).PatientId,
                .ParametreId = parametres.Item(e.RowIndex).ParametreId
                }
        If parametres.Item(e.RowIndex).IsActif Then
            autoSuiviDao.DeleteAutoSuivi(autoSuivi)
        Else
            autoSuiviDao.CreateAutoSuivi(autoSuivi, userLog)
        End If
        RadFAutoSuivi_Refresh()
    End Sub

End Class
