Imports Telerik.WinControls
Imports Telerik.WinControls.UI.Localization
Imports Oasis_Common
Imports System.Configuration
Imports Telerik.WinControls.UI

Public Class RadFPatientRequest
    Private privateUtilisateurConnecte As Utilisateur
    Property SelectedDrcId As Long

    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property

    ReadOnly patientDao As New PatientDao

    Private Sub RadFPatientListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Liste des patients", userLog)

        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.TextAndButtonsElement.TextElement.ForeColor = Color.Red
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.BackColor = Color.DarkBlue
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.Font = New Font("Arial", 8.0F, FontStyle.Italic)
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.TextImageRelation = TextImageRelation.TextBeforeImage
        Me.RadDesktopAlert1.Popup.AlertElement.BackColor = Color.MistyRose
        Me.RadDesktopAlert1.Popup.AlertElement.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.BorderColor = Color.DarkBlue

        'Traitement des contextes obsolètes
        Dim parametreOasisDao As ParametreOasisDao = New ParametreOasisDao
        parametreOasisDao.TraitementContexte()

        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()

        'Provque le chargement de la liste des patients
        RadChkPatientOasis.CheckState = CheckState.Checked

        Cursor.Current = Cursors.Default
    End Sub


    Private Sub ChargementPatient()
        Dim Tous As Boolean
        Dim PatientOasis As Boolean
        Dim patients = patientDao.GetByDRC(SelectedDrcId)

        RadPatientGridView.Rows.Clear()

        If RadChkPatientOasis.CheckState = CheckState.Checked Then
            PatientOasis = True
            Tous = False
        Else
            If RadChkPatientNonOasis.CheckState = CheckState.Checked Then
                PatientOasis = False
                Tous = False
            Else
                Tous = True
            End If
        End If


        LblOccurrenceLue.Text = patients.Count & " occurrence(s) lue(s)"

        Dim iGrid As Integer = 0
        For Each patient As Patient In patients
            RadPatientGridView.Rows.Add(iGrid)
            RadPatientGridView.Rows(iGrid).Cells("oa_patient_id").Value = patient.PatientId
            Dim NIR As Long = Coalesce(patient.PatientNir, 0)
            If NIR <> 0 Then
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_nir").Value = NIR
            Else
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_nir").Value = ""
            End If

            RadPatientGridView.Rows(iGrid).Cells("oa_patient_prenom").Value = Coalesce(patient.PatientPrenom, "")
            RadPatientGridView.Rows(iGrid).Cells("oa_patient_nom").Value = Coalesce(patient.PatientNom, "")

            Dim DateNaissance As Date = Coalesce(patient.PatientDateNaissance, Nothing)
            If DateNaissance <> Nothing Then
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_naissance").Value = DateNaissance.ToString("dd/MM/yyyy")
                RadPatientGridView.Rows(iGrid).Cells("age").Value = CalculAgeEnAnneeEtMoisString(DateNaissance)
            Else
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_naissance").Value = ""
                RadPatientGridView.Rows(iGrid).Cells("age").Value = ""
            End If

            Dim DateEntreeOasis As Date = Coalesce(patient.PatientDateEntree, Nothing)
            If DateEntreeOasis <> Nothing Then
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_entree_oasis").Value = DateEntreeOasis.ToString("dd/MM/yyyy")
            Else
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_entree_oasis").Value = ""
            End If

            Dim DateSortieOasis As Date = Coalesce(patient.PatientDateSortie, Nothing)
            If DateSortieOasis <> Nothing Then
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_sortie_oasis").Value = DateSortieOasis.ToString("dd/MM/yyyy")
            Else
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_sortie_oasis").Value = ""
            End If

            Dim SiteId As Long = Coalesce(patient.PatientSiteId, 0)
            If SiteId <> 0 Then
                RadPatientGridView.Rows(iGrid).Cells("site").Value = Environnement.Table_site.GetSiteDescription(SiteId)
            Else
                RadPatientGridView.Rows(iGrid).Cells("site").Value = ""
            End If
            iGrid += 1
        Next

        'Positionnement du grid sur la première occurrence
        If RadPatientGridView.Rows.Count > 0 Then
            Me.RadPatientGridView.CurrentRow = RadPatientGridView.Rows(0)
        End If
    End Sub

    Private Sub RadPatientGridView_DoubleClick(sender As Object, e As GridViewCellEventArgs) Handles RadPatientGridView.CellDoubleClick
        Dim SelectedCell As GridDataCellElement = TryCast(sender, GridDataCellElement)

        If userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString OrElse userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
            If SelectedCell IsNot Nothing Then

                Dim patientId As Long = CLng(RadPatientGridView.Rows(SelectedCell.RowIndex).Cells("oa_patient_id").Value)
                Cursor.Current = Cursors.WaitCursor
                Me.Enabled = False

                Try
                    Using form As New RadFSynthese
                        form.SelectedPatient = patientDao.GetPatient(patientId)
                        form.UtilisateurConnecte = userLog
                        form.EcranPrecedent = EnumAccesEcranPrecedent.SANS
                        form.ShowDialog()
                    End Using
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try

                Me.Enabled = True

            End If
        End If
    End Sub


    Private Sub RadPatientGridView_CellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs) Handles RadPatientGridView.CellFormatting
        Dim columnName As String = e.Column.Name
        Dim value As Object = e.Row.Cells(columnName).Value

        If columnName = "oa_patient_date_entree_oasis" Then
            If value IsNot DBNull.Value Then
                If value = "31/12/9998" Then
                    e.CellElement.Text = ""
                End If
            End If
        End If

        If columnName = "oa_patient_date_sortie_oasis" Then
            If value IsNot DBNull.Value Then
                If value = "31/12/9998" Then
                    e.CellElement.Text = ""
                End If
            End If
        End If
    End Sub

    Private Sub RadChkPatientOasis_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkPatientOasis.ToggleStateChanged
        If RadChkPatientOasis.CheckState = CheckState.Checked Then
            Application.DoEvents()
            Cursor.Current = Cursors.WaitCursor
            ChargementPatient()
            Cursor.Current = Cursors.Default
        End If
    End Sub

    Private Sub RadChkPatientNonOasis_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkPatientNonOasis.ToggleStateChanged
        If RadChkPatientNonOasis.CheckState = CheckState.Checked Then
            Application.DoEvents()
            Cursor.Current = Cursors.WaitCursor
            ChargementPatient()
            Cursor.Current = Cursors.Default
        End If
    End Sub

    Private Sub RadChkPatientTous_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkPatientTous.ToggleStateChanged
        If RadChkPatientTous.CheckState = CheckState.Checked Then
            Application.DoEvents()
            Cursor.Current = Cursors.WaitCursor
            ChargementPatient()
            Cursor.Current = Cursors.Default
        End If
    End Sub

    Private Sub RadButtonAbandon_Click(sender As Object, e As EventArgs) Handles RadButtonAbandon.Click
        Close()
    End Sub

    Private Sub BtnDRC_Click(sender As Object, e As EventArgs) Handles BtnDRC.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Try
            Using vFDrcSelecteur As New RadFDRCSelecteur
                vFDrcSelecteur.SelectedPatient = Nothing
                'vFDrcSelecteur.CategorieOasis = CategorieOasis
                vFDrcSelecteur.ShowDialog()
                SelectedDrcId = vFDrcSelecteur.SelectedDrcId
                ChargementPatient()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
        Me.Enabled = True
        Cursor.Current = Cursors.Default
    End Sub
End Class
