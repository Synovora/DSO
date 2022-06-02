Imports System.Configuration
Imports Oasis_Common
Imports Telerik.WinControls
Imports Telerik.WinControls.UI

Public Class RadFVaccinInfo

    Property Lock As Boolean
    Property SelectedPatient As Patient
    Property SelectedCGVDate As CGVDate
    Property SelectedValences As List(Of CGVValence)
    Property Valences As List(Of Valence)
    Property Vaccins As List(Of VaccinValence)
    Property VaccinPrograms As List(Of VaccinProgramRelation)

    Property CodeRetour As Boolean

    ReadOnly rorDao As New RorDao
    ReadOnly valenceDao As New ValenceDao
    ReadOnly vaccinDao As New VaccinDao
    ReadOnly cgvDateDao As New CGVDateDao
    ReadOnly userDao As New UserDao
    ReadOnly antecedentDao As New AntecedentDao

    Private Sub RadFATCListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Vaccin - Information", userLog)
        Dim VaccinProgram = vaccinDao.GetFirstVaccinProgramRelationListDatePatient(SelectedCGVDate.Id, SelectedPatient.PatientId)

        Lock = If(VaccinProgram.RealisationDate = Nothing, False, True)
        Chargement()
        ChargementEtatCivil()
        ChargementVaccins()
        ChargementValences()
        RefreshSelectedVaccin()
        ColorVaccins()
        ChargementInformation()
        ChargementButtons()
    End Sub

    Private Sub Chargement()
        If (Lock) Then
            Me.DTPDate.Enabled = False
            Me.GVVaccin.Enabled = False
            Me.BtnValidationProgram.Enabled = False
            Me.BtnAdminVaccin.Text = "Information vaccins"
        End If

        DTPDate.MinDate = DateAndTime.Now()
        BtnAdminVaccin.Enabled = False
        Vaccins = vaccinDao.GetListVaccinValence()
        Valences = valenceDao.GetList()
    End Sub

    Private Sub ChargementButtons()
        If SelectedCGVDate.OperatedBy <> Nothing AndAlso SelectedCGVDate.OperatedDate <> Nothing Then
            BtnValidationProgram.Text = "Annuler la programmation"
            If (SelectedCGVDate.SignedBy <> Nothing AndAlso SelectedCGVDate.SignedDate <> Nothing) Then
                BtnOrdo.Text = "Imprimer l'ordonnance"
                BtnOrdo.Enabled = True
            Else
                BtnOrdo.Text = "Signer l'ordonnance"
                BtnOrdo.Enabled = userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString
                ToolTip1.Show("Vous ne disposez pas d'un profil de type 'Médical', pour valider une ordonnance." &
                                " Votre prodil est de type : " & userLog.TypeProfil, BtnOrdo)
            End If
        Else
            BtnOrdo.Text = "Signer l'ordonnance"
            BtnOrdo.Enabled = False
        End If
    End Sub

    Private Sub ChargementInformation()
        LblAgeVaccination.Text = CGVDate.DaysToDate(SelectedCGVDate.Days)
        Dim valenceIds As New List(Of Long)
        For Each valences As CGVValence In SelectedValences
            valenceIds.Add(valences.Valence)
        Next
        DTPDate.Value = If(SelectedCGVDate.OperatedDate = Nothing, Date.Now(), SelectedCGVDate.OperatedDate)

        If (VaccinPrograms.Count > 0 AndAlso VaccinPrograms(0).RealisationOperator <> Nothing) Then
            LblOperator.Text = GetProfilUserString(userDao.GetUserById(VaccinPrograms(0).RealisationOperator))
        ElseIf (VaccinPrograms.Count > 0 AndAlso VaccinPrograms(0).RealisationOperatorRor <> Nothing) Then
            LblOperator.Text = GetProfilUserString(rorDao.GetRorById(VaccinPrograms(0).RealisationOperatorRor))
        ElseIf (VaccinPrograms.Count > 0 AndAlso VaccinPrograms(0).RealisationOperatorText <> Nothing) Then
            LblOperator.Text = VaccinPrograms(0).RealisationOperatorText
        Else
            LblOperator.Text = GetProfilUserString(userLog)
        End If
    End Sub

    Private Sub ChargementVaccins()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        VaccinPrograms = vaccinDao.GetVaccinProgramRelationListDatePatient(SelectedCGVDate.Id, SelectedPatient.PatientId)

        GVVaccin.Rows.Clear()
        Dim iGrid As Integer = 0
        Dim checker = False

        For Each vaccin As VaccinValence In Vaccins.GroupBy(Function(x) x.Code).Select(Function(x) x.First).ToList
            If SelectedValences IsNot Nothing AndAlso SelectedValences.Any(Function(x) x.Valence = vaccin.Valence) Then
                GVVaccin.Rows.Add(iGrid)
                GVVaccin.Rows(iGrid).Cells("id").Value = vaccin.Id
                GVVaccin.Rows(iGrid).Cells("checked").Value = VaccinPrograms.Any(Function(x) x.Vaccin = vaccin.Id)
                GVVaccin.Rows(iGrid).Cells("code").Value = vaccin.Code
                GVVaccin.Rows(iGrid).Cells("dci").Value = vaccin.Dci
                Dim valenceList = (From _vaccins In Vaccins.FindAll(Function(y) y.Code = vaccin.Code) Select _vaccins.Valence).ToArray()
                Dim test = (From _valence In Valences.FindAll(Function(x) valenceList.Contains(x.Id)) Select _valence.Description).ToArray()

                GVVaccin.Rows(iGrid).Cells("dci").Tag = String.Join(vbCrLf, (From _valence In Valences.FindAll(Function(x) valenceList.Contains(x.Id)) Select _valence.Description).ToArray())
                GVVaccin.Rows(iGrid).Cells("valence").Value = vaccin.Valence
                If VaccinPrograms.Any(Function(x) x.Vaccin = vaccin.Id) Then
                    checker = True
                End If
                iGrid += 1
            End If
        Next
        If (GVVaccin.Rows.Count > 0) Then
            GVVaccin.TableElement.RowScroller.ScrollToItem(GVVaccin.Rows(0))
        End If
        Me.BtnAdminVaccin.Enabled = checker
        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub RefreshSelectedVaccin()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        GVSelectedVaccin.Rows.Clear()
        Dim iGrid As Integer = 0
        For Each row As GridViewRowInfo In GVVaccin.Rows
            If row.Cells("checked").Value = True Then
                GVSelectedVaccin.Rows.Add(iGrid)
                GVSelectedVaccin.Rows(iGrid).Cells("id").Value = row.Cells("id").Value
                GVSelectedVaccin.Rows(iGrid).Cells("dci").Value = row.Cells("dci").Value
                GVSelectedVaccin.Rows(iGrid).Cells("valence").Value = row.Cells("valence").Value
                iGrid += 1
            End If
        Next
        Cursor.Current = Cursors.Default
        Me.Enabled = True
        RefreshNoneRequireValence()
    End Sub

    Private Sub RefreshNoneRequireValence()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        GVValenceNonRequis.Rows.Clear()
        Dim iGrid As Integer = 0
        For Each row As GridViewRowInfo In GVSelectedVaccin.Rows
            For Each valenceNone In Vaccins.FindAll(Function(x) x.Id.ToString() = row.Cells("id").Value AndAlso Not SelectedValences.Any(Function(y) y.Valence = x.Valence))
                GVValenceNonRequis.Rows.Add(iGrid)
                GVValenceNonRequis.Rows(iGrid).Cells("id").Value = row.Cells("id").Value
                GVValenceNonRequis.Rows(iGrid).Cells("name").Value = Valences.Find(Function(x) x.Id = valenceNone.Valence).Description
                GVValenceNonRequis.Rows(iGrid).Cells("name").Tag = Valences.Find(Function(x) x.Id = valenceNone.Valence).Description
                iGrid += 1
            Next
        Next
        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub ColorVaccins()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        For Each row As GridViewRowInfo In GVVaccin.Rows
            Dim tmp = 2
            row.Cells("dci").Style.ForeColor = Color.Black
            If row.Cells("checked").Value = False Then
                If GVValence.Rows.Any(Function(x) x.Cells("checked").Value = True AndAlso Vaccins.Any(Function(y) y.Id = row.Cells("id").Value AndAlso y.Valence = x.Cells("valence").Value)) Then
                    row.Cells("dci").Style.ForeColor = Color.Red
                    tmp = 4
                ElseIf Vaccins.Any(Function(x) x.Id.ToString() = row.Cells("id").Value AndAlso Not SelectedValences.Any(Function(y) y.Valence = x.Valence)) Then
                    row.Cells("dci").Style.ForeColor = Color.Orange
                    tmp = 3
                ElseIf Vaccins.FindAll(Function(x) x.Id.ToString() = row.Cells("id").Value AndAlso GVValence.Rows.Any(Function(y) y.Cells("valence").Value = x.Valence AndAlso y.Cells("checked").Value = False)).Count = SelectedValences.Count Then
                    row.Cells("dci").Style.ForeColor = Color.Green
                    tmp = 1
                End If
            End If
            row.Cells("category").Value = tmp
        Next

        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub ChargementValences()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        If SelectedValences Is Nothing Then
            Return
        End If
        GVValence.Rows.Clear()
        Dim iGrid As Integer = 0

        For Each valence As CGVValence In SelectedValences
            GVValence.Rows.Add(iGrid)
            GVValence.Rows(iGrid).Cells("id").Value = valence.Id
            GVValence.Rows(iGrid).Cells("valence").Value = valence.Valence
            GVValence.Rows(iGrid).Cells("checked").Value = GVVaccin.Rows.Any(Function(x) x.Cells("checked").Value = True AndAlso Vaccins.Any(Function(y) y.Id = x.Cells("id").Value AndAlso y.Valence.ToString() = valence.Valence.ToString()))
            GVValence.Rows(iGrid).Cells("name").Value = valence.Description
            GVValence.Rows(iGrid).Cells("name").Tag = valence.Description
            iGrid += 1
        Next
        ColorVaccins()
        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblDateNaissance.Text = SelectedPatient.PatientDateNaissance.ToString("dd.MM.yyyy")
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientDateMaj.Text = FormatageDateAffichage(SelectedPatient.PatientSyntheseDateMaj, True)

        Dim DateMaxValue = New Date(9998, 12, 31, 0, 0, 0)
        Dim DateMinValue = New Date(1, 1, 1, 0, 0, 0)
        If SelectedPatient.PatientDateEntree = DateMaxValue OrElse SelectedPatient.PatientDateEntree = DateMinValue OrElse SelectedPatient.PatientDateSortie < Date.Now Then
            LblNonOasis.Show()
        Else
            LblNonOasis.Hide()
        End If


        'Vérification de l'existence d'ALD
        LblALD.Hide()
        ChargementToolTipAld()

        'Contre-indication
        GetContreIndication()

        'Allergie
        GetAllergie()
        GetAllergieNonMedicamenteuse()

    End Sub

    Private Sub ChargementToolTipAld()
        Dim StringTooltip As String
        Dim aldDao As New AldDao

        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.PatientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    Private Sub GetAllergie()
        Dim patientDao As New PatientDao
        Dim StringAllergieToolTip As String = patientDao.GetStringAllergieByPatient(SelectedPatient.PatientId)
        If StringAllergieToolTip = "" Then
            LblAllergie.Hide()
            ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Enabled = False
        Else
            LblAllergie.Show()
            ToolTip.SetToolTip(LblAllergie, StringAllergieToolTip)
            ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Enabled = True
        End If
    End Sub

    Private Sub GetAllergieNonMedicamenteuse()
        Dim patientDao As New PatientDao
        Dim antecedentAllergies As List(Of Antecedent) = antecedentDao.GetListByDrc(SelectedPatient.PatientId, 121009)
        If antecedentAllergies.Count = 0 Then
            LblAllergie.Hide()
        Else
            LblAllergie.Show()
            ToolTip.SetToolTip(LblAllergieNonMedicamenteuse, String.Join(vbCrLf, (From antecedentAllergie In antecedentAllergies Select antecedentAllergie.Description).ToArray()))
        End If
    End Sub

    Private Sub GetContreIndication()
        Dim patientDao As New PatientDao
        Dim StringContreIndicationToolTip As String = patientDao.GetStringContreIndicationByPatient(SelectedPatient.PatientId)
        If StringContreIndicationToolTip = "" Then
            LblContreIndication.Hide()
            ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem.Enabled = False
        Else
            LblContreIndication.Show()
            ToolTip.SetToolTip(LblContreIndication, StringContreIndicationToolTip)
            ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem.Enabled = True
        End If
    End Sub

    Private Sub GVVaccin_Click(sender As Object, ByVal e As GridViewCellEventArgs) Handles GVVaccin.CellClick
        Dim row As Integer = e.RowIndex
        Dim valenceCol As Integer = e.ColumnIndex

        If row >= 0 AndAlso valenceCol = 0 Then
            If GVVaccin.Rows(row).Cells("checked").Value Then
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                If (vaccinDao.GetVaccinProgramAdministrationByRelation(VaccinPrograms.Find(Function(x) x.Vaccin = GVVaccin.Rows(row).Cells("id").Value).Id) IsNot Nothing) Then
                    MessageBox.Show("Ce vaccin a deja ete administre, il ne peut pas etre revoque")
                Else
                    vaccinDao.DeleteVaccinProgramRelation(New VaccinProgramRelation() With {.Date = SelectedCGVDate.Id, .Vaccin = GVVaccin.Rows(row).Cells("id").Value, .RelationVaccinValence = GVVaccin.Rows(row).Cells("code").Value, .Patient = SelectedPatient.PatientId})
                End If
            ElseIf Not GVValence.Rows.Any(Function(x) x.Cells("checked").Value = True AndAlso x.Cells("valence").Value = GVVaccin.Rows(row).Cells("valence").Value) Then
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                vaccinDao.CreateVaccinProgramRelation(New VaccinProgramRelation() With {.Date = SelectedCGVDate.Id, .RelationVaccinValence = GVVaccin.Rows(row).Cells("code").Value, .Vaccin = GVVaccin.Rows(row).Cells("id").Value, .Patient = SelectedPatient.PatientId})
            Else Return
            End If
            ChargementVaccins()
            ChargementValences()
        End If

        RefreshSelectedVaccin()

        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub BtnAdminVaccin_Click(sender As Object, e As EventArgs) Handles BtnAdminVaccin.Click
        If Lock Then
            Dim vaccinList = (From _vaccin In GVVaccin.Rows Where _vaccin.Cells("checked").Value = True Select Convert.ToInt64(_vaccin.Cells("id").Value)).ToArray()
            Using radFVaccinInput As New RadFVaccinInput
                radFVaccinInput.Lock = True
                radFVaccinInput.VaccinPrograms = vaccinDao.GetVaccinProgramRelationListDatePatient(SelectedCGVDate.Id, SelectedPatient.PatientId)
                radFVaccinInput.Vaccins = Vaccins.FindAll(Function(x) vaccinList.Contains(x.Id))
                radFVaccinInput.Valences = SelectedValences
                radFVaccinInput.Patient = SelectedPatient
                radFVaccinInput.ShowDialog()
                ChargementInformation()
            End Using
        Else

            SelectedCGVDate.OperatedBy = userLog.UtilisateurId
            SelectedCGVDate.OperatedDate = DTPDate.Value
            cgvDateDao.Update(SelectedCGVDate)
            Dim vaccinList = (From _vaccin In GVVaccin.Rows Where _vaccin.Cells("checked").Value = True Select Convert.ToInt64(_vaccin.Cells("id").Value)).ToArray()
            Using radFVaccinInput As New RadFVaccinInput
                radFVaccinInput.VaccinPrograms = vaccinDao.GetVaccinProgramRelationListDatePatient(SelectedCGVDate.Id, SelectedPatient.PatientId)
                radFVaccinInput.Vaccins = Vaccins.FindAll(Function(x) vaccinList.Contains(x.Id))
                radFVaccinInput.Valences = SelectedValences
                radFVaccinInput.Patient = SelectedPatient
                radFVaccinInput.ShowDialog()
                ChargementInformation()
                CodeRetour = True

                If radFVaccinInput.CodeRetour = True Then
                    'SelectedCGVDate.PerformDate = radFVaccinInput.DTPRealisation.Value()
                    'SelectedCGVDate.PerformBy = userLog.UtilisateurId
                    'cgvDateDao.Update(SelectedCGVDate)
                    Close()
                End If
            End Using
        End If
    End Sub

    Private Sub GVVaccin_ToolTipTextNeeded(sender As Object, e As ToolTipTextNeededEventArgs) Handles GVVaccin.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "dci" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("dci").Tag
        End If
    End Sub

    Private Sub GVValence_ToolTipTextNeeded(sender As Object, e As ToolTipTextNeededEventArgs) Handles GVValence.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "name" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("name").Tag
        End If
    End Sub

    Private Sub GVValenceNonRequis_ToolTipTextNeeded(sender As Object, e As ToolTipTextNeededEventArgs) Handles GVValenceNonRequis.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "name" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("name").Tag
        End If
    End Sub

    Private Sub BtnValidationProgram_Click(sender As Object, e As EventArgs) Handles BtnValidationProgram.Click
        If SelectedCGVDate.OperatedBy <> Nothing AndAlso SelectedCGVDate.OperatedDate <> Nothing Then
            SelectedCGVDate.OperatedBy = Nothing
            SelectedCGVDate.OperatedDate = Nothing
            SelectedCGVDate.OperatedBy = Nothing
            SelectedCGVDate.OperatedDate = Nothing
        Else
            SelectedCGVDate.OperatedBy = userLog.UtilisateurId
            SelectedCGVDate.OperatedDate = DTPDate.Value
        End If
        cgvDateDao.Update(SelectedCGVDate)
        CodeRetour = True
        Close()
    End Sub

    Private Sub DTPDate_ValueChanged(sender As Object, e As EventArgs) Handles DTPDate.ValueChanged
        SelectedCGVDate.OperatedDate = DTPDate.Value
    End Sub

    Private Sub BtnPrintOrdo_Click(sender As Object, e As EventArgs) Handles BtnOrdo.Click
        If (SelectedCGVDate.SignedBy <> Nothing AndAlso SelectedCGVDate.SignedDate <> Nothing) Then
            Dim vaccinList = (From _vaccin In GVVaccin.Rows Where _vaccin.Cells("checked").Value = True Select Convert.ToInt64(_vaccin.Cells("id").Value)).ToArray()
            Try
                Dim printPdf As New PrtOrdonnanceVaccin
                printPdf.SelectedPatient = SelectedPatient
                printPdf.Vaccins = Vaccins.FindAll(Function(x) vaccinList.Contains(x.Id)).GroupBy(Function(x) x.Code).Select(Function(x) x.First).ToList
                printPdf.SelectedUserValidation = userDao.GetUserById(SelectedCGVDate.OperatedBy)
                printPdf.PrintDocument()
            Catch ex As Exception
                MessageBox.Show(ex.Message())
            End Try
        Else
            SelectedCGVDate.SignedBy = userLog.UtilisateurId
            SelectedCGVDate.SignedDate = Date.Now()
            cgvDateDao.Update(SelectedCGVDate)
            Dim form As New RadFNotification With {
                 .Message = "L'ordonnance a été signée numériquement par : " & userLog.UtilisateurPrenom & " " & userLog.UtilisateurNom & vbCrLf & ". L'ordonnance est à présent disponible pour être imprimée"
             }
            form.Show()
            ChargementButtons()
        End If
    End Sub
End Class
