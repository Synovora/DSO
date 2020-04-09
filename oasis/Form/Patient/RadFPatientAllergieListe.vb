Imports System.Collections.Specialized
Imports Oasis_Common
Public Class RadFPatientAllergieListe
    Private privateSelectedPatient As Patient
    Private privateCodeRetour As Boolean

    Public Property CodeRetour As Boolean
        Get
            Return privateCodeRetour
        End Get
        Set(value As Boolean)
            privateCodeRetour = value
        End Set
    End Property

    Public Property SelectedPatient As Patient
        Get
            Return privateSelectedPatient
        End Get
        Set(value As Patient)
            privateSelectedPatient = value
        End Set
    End Property

    Dim allergieDao As New AllergieDao

    Private Sub RadFPatientAllergieListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementPatient()
        ChargementAllergiesPatient()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            RadBtnAnnulerSubstance.Hide()
        End If
    End Sub

    'Chargement de la Grid Notes patient
    Private Sub ChargementAllergiesPatient()
        Dim dt As DataTable = allergieDao.getAllAllergiebyPatient(SelectedPatient.patientId)

        RadCISubstancePatientDataGridView.Rows.Clear()

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            RadCISubstancePatientDataGridView.Rows.Add(iGrid)
            RadCISubstancePatientDataGridView.Rows(iGrid).Cells("allergie_id").Value = dt.Rows(i)("allergie_id")
            Dim substancePereId As Integer = Coalesce(dt.Rows(i)("substance_pere_id"), 0)
            If substancePereId <> 0 Then
                RadCISubstancePatientDataGridView.Rows(iGrid).Cells("substance_id").Value = dt.Rows(i)("substance_pere_id")
                RadCISubstancePatientDataGridView.Rows(iGrid).Cells("substance_id").Style.ForeColor = Color.DarkBlue
                RadCISubstancePatientDataGridView.Rows(iGrid).Cells("denomination_substance").Value = dt.Rows(i)("denomination_substance_pere")
                RadCISubstancePatientDataGridView.Rows(iGrid).Cells("denomination_substance").Style.ForeColor = Color.DarkBlue
            Else
                RadCISubstancePatientDataGridView.Rows(iGrid).Cells("substance_id").Value = dt.Rows(i)("substance_id")
                RadCISubstancePatientDataGridView.Rows(iGrid).Cells("denomination_substance").Value = dt.Rows(i)("denomination_substance")
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadCISubstancePatientDataGridView.Rows.Count > 0 Then
            Me.RadCISubstancePatientDataGridView.CurrentRow = RadCISubstancePatientDataGridView.ChildRows(0)
        End If
    End Sub

    Private Sub ChargementPatient()
        LblPatientNIR.Text = SelectedPatient.PatientNir
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientAdresse1.Text = SelectedPatient.PatientAdresse1
        LblPatientAdresse2.Text = SelectedPatient.PatientAdresse2
        LblPatientCodePostal.Text = SelectedPatient.PatientCodePostal
        LblPatientVille.Text = SelectedPatient.PatientVille
        LblPatientTel1.Text = SelectedPatient.PatientTel1
        LblPatientTel2.Text = SelectedPatient.PatientTel2
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)
        LblPatientDateMaj.Text = SelectedPatient.PatientSyntheseDateMaj.ToString("dd/MM/yyyy")
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnAnnulerSubstance_Click(sender As Object, e As EventArgs) Handles RadBtnAnnulerSubstance.Click
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        If RadCISubstancePatientDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadCISubstancePatientDataGridView.Rows.IndexOf(Me.RadCISubstancePatientDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim AllergieId As Integer = RadCISubstancePatientDataGridView.Rows(aRow).Cells("allergie_id").Value
                allergieDao.AnnulationAllergie(AllergieId)
            End If
        End If

        ChargementAllergiesPatient()
    End Sub
End Class
