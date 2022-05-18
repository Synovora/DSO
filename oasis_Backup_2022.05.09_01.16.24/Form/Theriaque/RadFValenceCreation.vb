Imports System.Configuration
Imports Oasis_Common
Imports Telerik.WinControls

Public Class RadFValenceCreation

    Property Valence As Valence = New Valence()

    ReadOnly valenceDao As New ValenceDao

    Private Sub RadFValenceCreation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Valence - Creation", userLog)
        code.Text = If(Valence.Code, "")
        description.Text = If(Valence.Description, "")
        precaution.Text = If(Valence.Precaution, "")
    End Sub

    Private Sub Valider_Click(sender As Object, e As EventArgs) Handles valider.Click
        Valence.Code = code.Text
        Valence.Description = description.Text
        Valence.Precaution = precaution.Text
        Valence.UtilisateurCreation = If(Valence.UtilisateurCreation > 0, Valence.UtilisateurCreation, Convert.ToInt64(userLog.UtilisateurId))
        Valence.UtilisateurModification = Convert.ToInt64(userLog.UtilisateurId)
        If Valence.Id = 0 AndAlso valenceDao.Create(Valence) Then
            Dim form As New RadFNotification()
            form.Titre = "Valence"
            form.Message = "creation d'une nouvelle valence"
            form.Show()
            Close()
        ElseIf Valence.Id <> 0 AndAlso valenceDao.Update(Valence) Then
            Dim form As New RadFNotification()
            form.Titre = "Valence"
            form.Message = "Modification de la valence"
            form.Show()
            Close()
        Else
            Throw New ArgumentException("Valence erreur lors de la creation/modification")
        End If
    End Sub

End Class
