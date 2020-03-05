Public Class Notification

    Public Shared Sub show(titre As String, message As String)
        Dim form As New RadFNotification()
        form.Titre = titre
        form.Message = message
        form.StartPosition = FormStartPosition.CenterScreen
        'form.AutoSizeMode = AutoSizeMode.GrowOnly
        'form.AutoSize = True
        form.Show()
    End Sub


End Class
