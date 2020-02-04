Imports Oasis_Common

Public Class RadFListeActions
    Private _userId As Long

    Public Property UserId As Long
        Get
            Return _userId
        End Get
        Set(value As Long)
            _userId = value
        End Set
    End Property

    Dim userdao As New UserDao
    Dim actiondao As New ActionDao
    Dim User As Utilisateur
    Dim UserActionId As Long

    Private Sub RadFListeActions_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        afficheTitleForm(Me, "Liste des actions réalisées")

        DteSelection.Value = Date.Now()
        RadBtnApres.Enabled = False

        If UserId = 0 Then
            UserActionId = userLog.UtilisateurId
        Else
            UserActionId = UserId
        End If
        User = userdao.getUserById(UserActionId)
        LblNomUtilisateur.Text = User.UtilisateurPrenom & "  " & User.UtilisateurNom & "  - " & User.UtilisateurProfilId & " / " & User.TypeProfil

        ChargementActions()
    End Sub

    Private Sub ChargementActions()
        Dim actionDataTable As DataTable
        actionDataTable = actiondao.getAllActionByUserAndDate(UserActionId, DteSelection.Value.Date)
        Dim DateAction As Date
        RadGridViewAction.Rows.Clear()

        Dim iGrid As Integer = -1
        Dim rowCount As Integer = actionDataTable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadGridViewAction.Rows.Add(iGrid)
            'Alimentation du DataGridView
            DateAction = Coalesce(actionDataTable.Rows(i)("horodatage"), Nothing)
            RadGridViewAction.Rows(iGrid).Cells("date").Value = DateAction.ToString("dd.MM.yyyy")
            RadGridViewAction.Rows(iGrid).Cells("heure").Value = DateAction.ToString("HH:mm")
            RadGridViewAction.Rows(iGrid).Cells("patientPrenom").Value = Coalesce(actionDataTable.Rows(i)("oa_patient_prenom"), "")
            RadGridViewAction.Rows(iGrid).Cells("patientNom").Value = Coalesce(actionDataTable.Rows(i)("oa_patient_nom"), "")
            RadGridViewAction.Rows(iGrid).Cells("action").Value = Coalesce(actionDataTable.Rows(i)("action"), "")
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewAction.Rows.Count > 0 Then
            RadGridViewAction.CurrentRow = RadGridViewAction.ChildRows(0)
            RadGridViewAction.TableElement.VScrollBar.Value = 0
        End If

    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnAvant_Click(sender As Object, e As EventArgs) Handles RadBtnAvant.Click
        DteSelection.Value = DteSelection.Value.AddDays(-1)
        RadBtnApres.Enabled = True
        ChargementActions()
    End Sub

    Private Sub RadBtnApres_Click(sender As Object, e As EventArgs) Handles RadBtnApres.Click
        DteSelection.Value = DteSelection.Value.AddDays(1)
        If DteSelection.Value.Date >= Date.Now().Date Then
            RadBtnApres.Enabled = False
        Else
            RadBtnApres.Enabled = True
        End If
        ChargementActions()
    End Sub

    Private Sub DteSelection_ValueChanged(sender As Object, e As EventArgs) Handles DteSelection.ValueChanged

    End Sub
End Class
