Imports Oasis_Common

Public Class RadFAllergieEtCISuppressionDetail
    Private _selectedTraitement As Traitement
    Private _codeRetour As Boolean
    Private _SelectedPatient As PatientBase

    Public Property SelectedTraitement As Traitement
        Get
            Return _selectedTraitement
        End Get
        Set(value As Traitement)
            _selectedTraitement = value
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

    Public Property SelectedPatient As PatientBase
        Get
            Return _SelectedPatient
        End Get
        Set(value As PatientBase)
            _SelectedPatient = value
        End Set
    End Property

    Dim traitementDao As New TraitementDao
    Dim medicamentDao As New MedicamentDao
    Dim medicament As Medicament

    Private Sub RadFAllergieEtCISuppressionDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CodeRetour = False
        ChargementPatient()
        ChargementMedicament()
        ChargerZonesArret()
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

    Private Sub ChargementMedicament()
        medicament = medicamentDao.GetMedicamentById(SelectedTraitement.MedicamentId)
        LblMedicamentDCI.Text = medicament.MedicamentDci
        LblMedicamentForme.Text = medicament.Forme
        LblMedicamentAdministration.Text = medicament.VoieAdministration
        LblMedicamentTitulaire.Text = medicament.Titulaire
    End Sub

    Private Sub ChargerZonesArret()
        TxtCommentaireArret.Text = SelectedTraitement.ArretCommentaire
        ChkAllergie.Checked = False
        If SelectedTraitement.Allergie = True Then
            ChkAllergie.Checked = True
            ChkAllergie.ForeColor = Color.Red
        Else
            ChkAllergie.ForeColor = Color.Black
        End If

        ChkContreIndication.Checked = False
        If SelectedTraitement.ContreIndication = True Then
            ChkContreIndication.Checked = True
            ChkContreIndication.ForeColor = Color.Red
        Else
            ChkContreIndication.ForeColor = Color.Black
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnAnnuler_Click(sender As Object, e As EventArgs) Handles RadBtnAnnuler.Click
        Dim traitementHistoACreer As New TraitementHisto
        SelectedTraitement.Allergie = False
        SelectedTraitement.ContreIndication = False
        SelectedTraitement.ArretCommentaire = ""
        If SelectedTraitement.DateFin = Nothing Then
            SelectedTraitement.DateFin = Date.MaxValue
        End If
        traitementDao.ArretTraitement(SelectedTraitement, traitementHistoACreer)
        CodeRetour = True
        Close()
    End Sub
End Class
