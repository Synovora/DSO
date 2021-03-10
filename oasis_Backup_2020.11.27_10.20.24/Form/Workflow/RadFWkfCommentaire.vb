Imports Oasis_Common
Public Class RadFWkfCommentaire
    Private _workflowId As Long

    Public Property WorkflowId As Long
        Get
            Return _workflowId
        End Get
        Set(value As Long)
            _workflowId = value
        End Set
    End Property

    Dim tacheDao As New TacheDao
    Dim userDao As New UserDao

    Private Sub RadFWkfCommentaire_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementCommentaire()
    End Sub

    Private Sub ChargementCommentaire()
        If WorkflowId <> 0 Then
            Dim Continu As Boolean = True
            Dim iGrid As Integer = -1
            Dim LastWorkflowId As Long = WorkflowId
            While Continu
                Dim workflow As Tache
                workflow = tacheDao.getTacheById(LastWorkflowId)
                If workflow.EmetteurCommentaire <> "" Then
                    iGrid += 1
                    RadGridViewWkfCommentaire.Rows.Add(iGrid)
                    RadGridViewWkfCommentaire.Rows(iGrid).Cells("emetteurCommentaire").Value = workflow.EmetteurCommentaire
                    Dim emetteur As Utilisateur = userDao.getUserById(workflow.EmetteurUserId)
                    Dim emetteurNom As String = emetteur.UtilisateurPrenom & " " & emetteur.UtilisateurNom
                    RadGridViewWkfCommentaire.Rows(iGrid).Cells("emetteurNom").Value = emetteurNom
                End If
                If workflow.ParentId = 0 Then
                    Continu = False
                Else
                    LastWorkflowId = workflow.ParentId
                End If
            End While
        End If

        'Positionnement du grid sur la première occurrence
        If RadGridViewWkfCommentaire.Rows.Count > 0 Then
            RadGridViewWkfCommentaire.CurrentRow = RadGridViewWkfCommentaire.ChildRows(0)
            RadGridViewWkfCommentaire.TableElement.VScrollBar.Value = 0
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub
End Class
