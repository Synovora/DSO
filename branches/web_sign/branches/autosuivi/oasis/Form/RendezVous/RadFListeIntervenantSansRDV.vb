Imports Oasis_Common
Public Class RadFListeIntervenantSansRDV

    Private Sub RadFListeIntervenantSansRDV_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementGrid()
    End Sub

    Private Sub ChargementGrid()
        RadGridView.Rows.Clear()

        Dim dt As DataTable
        Dim parcoursDao As New ParcoursDao

        dt = parcoursDao.GetAllIntervenantSansRendezVous()

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = dt.Rows.Count - 1
        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadGridView.Rows.Add(iGrid)

            RadGridView.Rows(iGrid).Cells("oa_parcours_patient_id").Value = Coalesce(dt.Rows(i)("oa_parcours_patient_id"), 0)
            RadGridView.Rows(iGrid).Cells("oa_parcours_id").Value = Coalesce(dt.Rows(i)("oa_parcours_id"), 0)

            RadGridView.Rows(iGrid).Cells("patient").Value = Coalesce(dt.Rows(i)("oa_patient_prenom"), "") & " " & Coalesce(dt.Rows(i)("oa_patient_nom"), "")

            Dim NomIntervenant As String = Coalesce(dt.Rows(i)("oa_ror_nom"), "")
            Dim NomStructure As String = Coalesce(dt.Rows(i)("oa_ror_structure_nom"), "")
            Dim NomSpecialite As String = Coalesce(dt.Rows(i)("oa_r_specialite_description"), "")
            RadGridView.Rows(iGrid).Cells("intervenant").Value = NomIntervenant & " - " & NomSpecialite & " - " & NomStructure

            RadGridView.Rows(iGrid).Cells("rythme").Value = Coalesce(dt.Rows(i)("oa_parcours_rythme"), "") & " X " & Coalesce(dt.Rows(i)("oa_parcours_base"), "")
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridView.Rows.Count > 0 Then
            RadGridView.CurrentRow = RadGridView.ChildRows(0)
            RadGridView.TableElement.VScrollBar.Value = 0
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnParcours_Click(sender As Object, e As EventArgs) Handles RadBtnParcours.Click
        DetailParcours()
    End Sub

    Private Sub MasterTemplate_DoubleClick(sender As Object, e As EventArgs) Handles RadGridView.DoubleClick
        DetailParcours()
    End Sub

    Private Sub DetailParcours()
        If RadGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridView.Rows.IndexOf(Me.RadGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ParcoursId As Integer = RadGridView.Rows(aRow).Cells("oa_parcours_Id").Value
                Dim PatientId As Integer = RadGridView.Rows(aRow).Cells("oa_parcours_patient_id").Value
                Dim patient As Patient = PatientDao.GetPatientById(PatientId)
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor

                Try
                    Using vFParcoursDetailEdit As New RadFParcoursDetailEdit
                        vFParcoursDetailEdit.SelectedParcoursId = ParcoursId
                        vFParcoursDetailEdit.SelectedPatient = patient
                        'vFParcoursDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                        vFParcoursDetailEdit.RythmeObligatoire = False
                        vFParcoursDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                        vFParcoursDetailEdit.ShowDialog() 'Modal
                    End Using
                    ChargementGrid()
                Catch ex As Exception
                    MsgBox(ex.Message())
                End Try

                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadBtnReload_Click(sender As Object, e As EventArgs) Handles RadBtnReload.Click
        ChargementGrid()
    End Sub
End Class
