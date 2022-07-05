Imports System.Globalization
Imports Oasis_Common
Imports Telerik.WinControls
Imports Telerik.WinControls.UI

Public Class RadFVaccinInput

    ReadOnly rorDao As RorDao = New RorDao
    ReadOnly userDao As UserDao = New UserDao
    ReadOnly vaccinDao As VaccinDao = New VaccinDao
    ReadOnly cgvDateDao As CGVDateDao = New CGVDateDao

    Property Lock As Boolean
    Property CodeRetour As Boolean

    Property SelectedValences As List(Of CGVValence)
    Property Vaccins As List(Of VaccinValence)
    Property VaccinPrograms As List(Of VaccinProgramRelation)
    Property RealisationOperator As Long
    Property RealisationOperatorRor As Long
    Property RealisationOperatorText As String = ""
    Property Patient As Patient

    Private Sub RadFVaccinInput_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Vaccin - Administrer", userLog)

        If (Lock) Then
            Me.DTPRealisation.Enabled = False
            Me.BtnSelectOperator.Enabled = False
            Me.BtnValidation.Enabled = False
        End If

        DTPRealisation.Format = DateTimePickerFormat.Custom
        DTPRealisation.CustomFormat = "dd/MM/yyyy"
        DTPRealisation.MaxDate = DateTime.Today
        DTPRealisation.MinDate = Patient.PatientDateNaissance

        TextOperator.Text = GetProfilUserString(userLog)
        RealisationOperator = userLog.UtilisateurId
        If (VaccinPrograms.Count > 0) Then
            If (VaccinPrograms(0).RealisationDate <> Nothing) Then
                DTPRealisation.Value = VaccinPrograms(0).RealisationDate
                DTPRealisation_ValueChanged(Nothing, Nothing)
            End If
            If (VaccinPrograms(0).RealisationOperator <> Nothing) Then
                TextOperator.Text = GetProfilUserString(userDao.GetUserById(VaccinPrograms(0).RealisationOperator))
                RealisationOperator = VaccinPrograms(0).RealisationOperator
            ElseIf (VaccinPrograms(0).RealisationOperatorRor <> Nothing) Then
                TextOperator.Text = GetProfilUserString(rorDao.GetRorById(VaccinPrograms(0).RealisationOperatorRor))
                RealisationOperatorRor = VaccinPrograms(0).RealisationOperatorRor
            ElseIf (VaccinPrograms(0).RealisationOperatorText <> Nothing) Then
                TextOperator.Text = VaccinPrograms(0).RealisationOperatorText
                RealisationOperatorText = VaccinPrograms(0).RealisationOperatorText
            End If
        End If
        Refresh()
    End Sub

    Private Sub Refresh()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        GVVaccin.Rows.Clear()
        Dim iGrid As Integer = 0
        Dim checker = 0

        For Each vaccin As VaccinValence In Vaccins.GroupBy(Function(x) x.Code).Select(Function(x) x.First).ToList
            GVVaccin.Rows.Add(iGrid)
            GVVaccin.Rows(iGrid).Cells("id").Value = vaccin.Id
            GVVaccin.Rows(iGrid).Cells("dci").Value = vaccin.Dci
            GVVaccin.Rows(iGrid).Cells("relation_id").Value = VaccinPrograms.Find(Function(x) x.Vaccin = vaccin.Id).Id
            GVVaccin.Rows(iGrid).Cells("realisation").Value = DTPRealisation.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) & " - " & TextOperator.Text
            Dim vaccinProgramAdmin = vaccinDao.GetVaccinProgramAdministrationByRelation(VaccinPrograms.Find(Function(x) x.Vaccin = vaccin.Id).Id)
            If (vaccinProgramAdmin IsNot Nothing) Then
                GVVaccin.Rows(iGrid).Cells("program_id").Value = vaccinProgramAdmin.Id
                GVVaccin.Rows(iGrid).Cells("lot").Value = vaccinProgramAdmin.Lot
                GVVaccin.Rows(iGrid).Cells("expiration").Value = vaccinProgramAdmin.Expiration.ToString("MM/yyyy", CultureInfo.InvariantCulture)
                GVVaccin.Rows(iGrid).Cells("comment").Value = vaccinProgramAdmin.Comment
                checker += 1
            End If

            If (Lock) Then
                GVVaccin.Rows(iGrid).Cells("dci").Style.ForeColor = Color.Green
                GVVaccin.Rows(iGrid).Cells("lot").Style.ForeColor = Color.Green
                GVVaccin.Rows(iGrid).Cells("expiration").Style.ForeColor = Color.Green
                GVVaccin.Rows(iGrid).Cells("realisation").Style.ForeColor = Color.Green
            End If
            iGrid += 1
        Next

        BtnValidation.Enabled = checker = iGrid

        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub GVVaccin_Click(sender As Object, ByVal e As GridViewCellEventArgs) Handles GVVaccin.CellClick
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False

        Dim row As Integer = e.RowIndex
        Dim valenceCol As Integer = e.ColumnIndex

        If row >= 0 Then
            Using radFVaccinInputComment As New RadFVaccinInputComment
                radFVaccinInputComment.TextLot.Text =
                     Coalesce(GVVaccin.Rows(row).Cells("lot").Value, "")
                radFVaccinInputComment.DTPExp.Value = Coalesce(GVVaccin.Rows(row).Cells("expiration").Value, Date.Now())
                radFVaccinInputComment.RTEComment.Text = Coalesce(GVVaccin.Rows(row).Cells("comment").Value, "")
                radFVaccinInputComment.ProgramId = GVVaccin.Rows(row).Cells("program_id").Value
                If radFVaccinInputComment.ProgramId <> Nothing Then
                    radFVaccinInputComment.Lock = True
                End If
                radFVaccinInputComment.ShowDialog()
                If (radFVaccinInputComment.codeRetour) Then
                    If (radFVaccinInputComment.ProgramId = Nothing) Then
                        vaccinDao.CreateVaccinProgramAdministration(New VaccinProgramAdmin With {
                    .Lot = radFVaccinInputComment.TextLot.Text,
                    .Comment = radFVaccinInputComment.RTEComment.Text,
                    .Expiration = radFVaccinInputComment.DTPExp.Value,
                    .VaccinProgramRelation = GVVaccin.Rows(row).Cells("relation_id").Value})
                        Refresh()
                    Else
                        vaccinDao.UpdateVaccinProgramAdministration(New VaccinProgramAdmin With {
                              .Id = radFVaccinInputComment.ProgramId,
                              .Lot = radFVaccinInputComment.TextLot.Text,
                              .Comment = radFVaccinInputComment.RTEComment.Text,
                              .Expiration = radFVaccinInputComment.DTPExp.Value,
                              .VaccinProgramRelation = GVVaccin.Rows(row).Cells("relation_id").Value
                                                      })
                        Refresh()
                    End If
                End If
            End Using
        End If

        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub GVVaccin_ToolTipTextNeeded(sender As Object, e As ToolTipTextNeededEventArgs) Handles GVVaccin.ToolTipTextNeeded
        Dim cell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If cell IsNot Nothing AndAlso cell.Value IsNot Nothing Then
            e.ToolTipText = cell.Value.ToString()
        End If
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        Me.Close()
    End Sub

    Private Sub BtnValidation_Click(sender As Object, e As EventArgs) Handles BtnValidation.Click
        If Not Lock Then
            Enabled = False
            Cursor.Current = Cursors.WaitCursor
            Dim result3 As DialogResult = MessageBox.Show("Avez vous deja reelement administre le vaccin? Cette action est definitive.",
            "The Question",
            MessageBoxButtons.YesNo,
            MessageBoxIcon.Question,
            MessageBoxDefaultButton.Button2)

            If result3 = DialogResult.Yes Then
                For Each vaccinProgram As VaccinProgramRelation In VaccinPrograms
                    vaccinDao.UpdateVaccinProgramRelation(New VaccinProgramRelation With {
                                                          .Id = vaccinProgram.Id,
                                                          .[Date] = vaccinProgram.Date,
                                                          .Patient = vaccinProgram.Patient,
                                                          .Vaccin = vaccinProgram.Vaccin,
                                                          .RelationVaccinValence = vaccinProgram.RelationVaccinValence,
                                                          .RealisationOperator = RealisationOperator,
                                                          .RealisationOperatorText = RealisationOperatorText,
                                                          .RealisationOperatorRor = RealisationOperatorRor,
                                                          .RealisationDate = DTPRealisation.Value})
                    Dim vaccinValences = vaccinDao.GetListRelationByVaccin(vaccinProgram.RelationVaccinValence)
                    SelectedValences.RemoveAll(Function(x) vaccinValences.Any(Function(y) y.Valence = x.Valence))
                    For Each vaccinValence In vaccinValences
                        cgvDateDao.UpdateRelationStatus(New RelationValenceDate With {
                                                    .[Date] = vaccinProgram.Date,
                                                    .Patient = vaccinProgram.Patient,
                                                    .Valence = vaccinValence.Valence,
                                                    .Status = 1
                    })
                    Next
                Next
                For Each valence In SelectedValences
                    cgvDateDao.UpdateRelationStatus(New RelationValenceDate With {
                                                    .[Date] = VaccinPrograms(0).Date,
                                                    .Patient = VaccinPrograms(0).Patient,
                                                    .Valence = valence.Valence,
                                                    .Status = 2
                    })
                Next
            Else
                Cursor.Current = Cursors.Default
                Me.Enabled = True
                Return
            End If

            Cursor.Current = Cursors.Default
            Me.Enabled = True
        End If
        CodeRetour = True
        Me.Close()
    End Sub

    Private Sub DTPRealisation_ValueChanged(sender As Object, e As EventArgs) Handles DTPRealisation.Validated, TextOperator.Validated
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        For Each row In GVVaccin.Rows
            row.Cells("realisation").Value = DTPRealisation.Value.ToString("dd/MM/yyyy", CultureInfo.InvariantCulture) & " - " & TextOperator.Text
        Next

        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub BtnSelectOperator_Click(sender As Object, e As EventArgs) Handles BtnSelectOperator.Click
        Try
            Using radFVaccinOperator As New RadFVaccinOperator
                radFVaccinOperator.ShowDialog()
                If radFVaccinOperator.CodeRetour = True Then
                    If radFVaccinOperator.SelectedRorId <> Nothing Then
                        TextOperator.Text = GetProfilUserString(rorDao.GetRorById(radFVaccinOperator.SelectedRorId))
                        RealisationOperatorRor = radFVaccinOperator.SelectedRorId
                        RealisationOperator = 0
                        RealisationOperatorText = ""
                    Else
                        TextOperator.Text = radFVaccinOperator.TextOperator.Text
                        RealisationOperatorRor = 0
                        RealisationOperator = 0
                        RealisationOperatorText = radFVaccinOperator.TextOperator.Text
                    End If

                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class
