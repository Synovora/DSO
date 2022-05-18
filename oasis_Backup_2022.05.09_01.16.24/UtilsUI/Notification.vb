Public Class Notification

    Public Shared Sub show(titre As String, message As String, Optional duree As Integer = 0)
        Dim form As New RadFNotification()
        form.Titre = titre
        form.Message = message
        form.Duree = 0
        form.StartPosition = FormStartPosition.CenterScreen
        'form.AutoSizeMode = AutoSizeMode.GrowOnly
        'form.AutoSize = True
        form.Show()
    End Sub


End Class
