Public Class RadFNotification

    Public Property Titre As String
    Public Property Message As String
    Public Property Duree As Integer

    Dim Interval As Integer

    Private Sub RadFNotification_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If Titre = "" OrElse Titre Is Nothing Then
            Me.Text = "Notification"
        Else
            Me.Text = "Notification : " & Titre
        End If

        Texte.Text = Message
        Texte.Enabled = False

        If Duree <> 0 Then
            Interval = Duree * 1000
        Else
            Interval = 2000
        End If

        Timer1.Start()
        Timer1.Interval = Interval
        SetOpacity()
    End Sub

    Private Sub RadFNotification_MouseLeave(sender As Object, e As EventArgs) Handles MyBase.MouseLeave
        SubMouseLeave()
    End Sub

    Private Sub Texte_MouseLeave(sender As Object, e As EventArgs)
        SubMouseLeave()
    End Sub

    Private Sub SubMouseLeave()
        Timer1.Start()
        Timer1.Interval = Interval
        SetOpacity()
    End Sub

    Private Sub RadFNotification_MouseHover(sender As Object, e As EventArgs) Handles MyBase.MouseHover
        SubMouseHover()
    End Sub

    Private Sub Texte_MouseHover(sender As Object, e As EventArgs)
        SubMouseHover()
    End Sub

    Private Sub SubMouseHover()
        Timer1.Stop()
        Timer2.Stop()
        Me.Opacity = 1
    End Sub


    Private Sub SetOpacity()
        Timer2.Start()
        Timer2.Interval = 300
    End Sub

    Private Sub Timer2_Tick(sender As Object, e As EventArgs) Handles Timer2.Tick
        Me.Opacity -= 0.05
    End Sub

    Private Sub Timer1_Tick(sender As Object, e As EventArgs) Handles Timer1.Tick
        Close()
    End Sub

    Private Sub RadFNotification_DoubleClick(sender As Object, e As EventArgs) Handles MyBase.DoubleClick
        Close()
    End Sub
End Class
