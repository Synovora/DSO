Public Class RadFAffichaeInfo
    Private _infoToDisplay1 As String
    Private _titre As String

    Public Property InfoToDisplay As String
        Get
            Return _infoToDisplay1
        End Get
        Set(value As String)
            _infoToDisplay1 = value
        End Set
    End Property

    Public Property Titre As String
        Get
            Return _titre
        End Get
        Set(value As String)
            _titre = value
        End Set
    End Property

    Private Sub RadFAffichaeInfo_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, Titre, userLog)
        TextBoxInfo.Text = InfoToDisplay
        RadBtnAbandon.Select()
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub
End Class
