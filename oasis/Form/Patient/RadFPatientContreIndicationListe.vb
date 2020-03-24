Imports System.Collections.Specialized
Imports Oasis_Common

Public Class RadFPatientContreIndicationListe
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

    Dim contreIndicationDao As New ContreIndicationDao

    Private Sub RadFPatientContreIndicationListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementPatient()
        ChargementContreIndicationPatient()
    End Sub

    'Chargement de la Grid Notes patient
    Private Sub ChargementContreIndicationPatient()
        Dim dt As DataTable = contreIndicationDao.getAllContreIndicationbyPatient(SelectedPatient.patientId)

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            RadAllergiesPatientDataGridView.Rows.Add(iGrid)
            RadAllergiesPatientDataGridView.Rows(iGrid).Cells("code_atc").Value = dt.Rows(i)("code_atc")
            RadAllergiesPatientDataGridView.Rows(iGrid).Cells("denomination_atc").Value = dt.Rows(i)("denomination_atc")
        Next

        'Positionnement du grid sur la première occurrence
        If RadAllergiesPatientDataGridView.Rows.Count > 0 Then
            Me.RadAllergiesPatientDataGridView.CurrentRow = RadAllergiesPatientDataGridView.ChildRows(0)
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

    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Me.CodeRetour = False
        Close()
    End Sub
End Class
