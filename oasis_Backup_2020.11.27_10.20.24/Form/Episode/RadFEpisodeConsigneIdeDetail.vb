Imports Oasis_Common

Public Class RadFEpisodeConsigneIdeDetail
    Private _drcId As Long
    Private _denominationConsigneIde As String
    Private _codeRetour As Boolean

    Public Property DrcId As Long
        Get
            Return _drcId
        End Get
        Set(value As Long)
            _drcId = value
        End Set
    End Property

    Public Property DenominationConsigneIde As String
        Get
            Return _denominationConsigneIde
        End Get
        Set(value As String)
            _denominationConsigneIde = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return _codeRetour
        End Get
        Set(value As Boolean)
            _codeRetour = value
        End Set
    End Property

    ReadOnly drcdao As New DrcDao
    ReadOnly drc As New Drc

    Private Sub RadFEpisodeConsigneIdeDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CodeRetour = False
        drcdao.GetDrc(drc, DrcId)
        TxtDénominationDrc.Text = drc.DrcLibelle
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnCopierDenomination_Click(sender As Object, e As EventArgs) Handles RadBtnCopierDenomination.Click
        TxtDenominationConsigneIde.Text = TxtDénominationDrc.Text
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        If TxtDenominationConsigneIde.Text = "" Then
            TxtDenominationConsigneIde.Text = TxtDénominationDrc.Text
        End If
        CodeRetour = True
        Close()
    End Sub
End Class
