Imports Telerik.WinControls

Public Class FrmChoixDateHeureDuree

    Public DateChoisie As Date
    Property Commentaire As String
    Public Sub New(commentaireInit As String)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' --- on enleve le titre du formulaire
        'Me.FormElement.TitleBar.Visibility = ElementVisibility.Collapsed

        Me.TxtRDVCommentaire.Text = commentaireInit
    End Sub
    Private Sub RadButtonAbandon_Click(sender As Object, e As EventArgs) Handles RadButtonAbandon.Click
        Close()
    End Sub

    Private Function CalculMinutes() As Integer
        Dim minutes As Integer = 0
        If RadioBtn0.Checked = True Then
            minutes = 0
        Else
            If RadioBtn15.Checked = True Then
                minutes = 15
            Else
                If RadioBtn30.Checked = True Then
                    minutes = 30
                Else
                    minutes = 45
                End If
            End If
        End If
        Return minutes
    End Function



    Private Sub BtnValider_Click(sender As Object, e As EventArgs) Handles BtnValider.Click
        Dim minutesRV As Integer = CalculMinutes()
        Dim dateRendezVous As Date = New Date(NumDateRV.Value.Year, NumDateRV.Value.Month, NumDateRV.Value.Day, NumheureRV.Value, minutesRV, 0)
        If dateRendezVous < Date.Now() Then
            MsgBox("Erreur : La date de rendez-vous à programmer (" & dateRendezVous.ToString("dd.MM.yyyy") & "), ne doit pas être antérieure à la date du jour (" & Date.Now().ToString("dd.MM.yyyy") & ")", MsgBoxStyle.OkOnly Or MsgBoxStyle.Critical, "Choix de la date de rendez-vous")
            Return
        End If
        dateChoisie = dateRendezVous
        commentaire = Me.TxtRDVCommentaire.Text
        Close()
    End Sub
End Class
