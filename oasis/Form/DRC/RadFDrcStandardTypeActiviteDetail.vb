Imports Oasis_Common

Public Class RadFDrcStandardTypeActiviteDetail
    Private _DrcStandardId As Long
    Private _codeRetour As Boolean

    Public Property SelectedDrcStandardId As Long
        Get
            Return _DrcStandardId
        End Get
        Set(value As Long)
            _DrcStandardId = value
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

    Dim drcStandardDao As New DrcStandardDao
    Dim drcDao As New DrcDao
    Dim drcStandard As DrcStandard
    Dim drc As New Drc

    Private Sub RadFDrcStandardTypeActiviteDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CodeRetour = False
        ChargementDrcStandard()
    End Sub

    Private Sub ChargementDrcStandard()
        drcStandard = drcStandardDao.GetDrcStandardById(SelectedDrcStandardId)
        drcDao.GetDrc(drc, drcStandard.DrcId)
        TxtDrcDescription.Text = drc.DrcLibelle
        TxtActiviteEpisode.Text = drcStandard.TypeActivite
        TxtCategorieOasis.Text = drcDao.GetItemCategorieOasisByCode(drcStandard.CategorieOasis)
        NumAgeMin.Value = drcStandard.AgeMin
        NumAgeMax.Value = drcStandard.AgeMax
        If Not (drcStandard.TypeActivite = Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE Or
            drcStandard.TypeActivite = Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE) Then
            LblAgeMin.Hide()
            NumAgeMin.Hide()
            LblAgeMax.Hide()
            NumAgeMax.Hide()
            LblAgeUnite.Hide()
            NumAgeMin.Value = 0
            NumAgeMax.Value = 0
            RadBtnValidation.Hide()
        End If

        Select Case drcStandard.TypeActivite
            Case Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE
                LblAgeUnite.Text = "(Age exprimé en année)"
            Case Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE
                LblAgeUnite.Text = "(Age exprimé en mois)"
            Case Else
                LblAgeUnite.Text = ""
        End Select
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        If drcStandard.AgeMin > drcStandard.AgeMax Then
            MessageBox.Show("L'âge min doit être inférieur à la l'âge max")
        Else
            If drcStandardDao.ModificationDrcStandard(drcStandard, userLog) = True Then
                MessageBox.Show("La DRC standard a été modifiée")
                Close()
            End If
        End If
    End Sub

    Private Sub RadBtnAnnulation_Click(sender As Object, e As EventArgs) Handles RadBtnAnnulation.Click
        If MsgBox("Confirmation de l'annulation ", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            If drcStandardDao.AnnulationDrcStandard(SelectedDrcStandardId, userLog) = True Then
                MessageBox.Show("La DRC standard a été annulée")
                CodeRetour = True
                Close()
            End If
        End If
    End Sub

    Private Sub NumAgeMin_ValueChanged(sender As Object, e As EventArgs) Handles NumAgeMin.ValueChanged
        drcStandard.AgeMin = NumAgeMin.Value
    End Sub

    Private Sub NumAgeMax_ValueChanged(sender As Object, e As EventArgs) Handles NumAgeMax.ValueChanged
        drcStandard.AgeMax = NumAgeMax.Value
    End Sub

    Private Sub RadBtnDrcDetail_Click(sender As Object, e As EventArgs) Handles RadBtnDrcDetail.Click
        'Suppression de l'association de la DRC
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vRadFDrcDetailEdit As New RadFDrcDetailEdit
                vRadFDrcDetailEdit.SelectedDRCId = drcStandard.DrcId
                vRadFDrcDetailEdit.UtilisateurConnecte = userLog
                vRadFDrcDetailEdit.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Cursor.Current = Cursors.Default
    End Sub
End Class
