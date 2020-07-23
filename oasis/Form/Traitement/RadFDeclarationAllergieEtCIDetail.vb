Imports System.Data.SqlClient
Imports Telerik.WinControls
Imports Telerik.WinControls.UI.Localization
Imports Oasis_Common
Public Class RadFDeclarationAllergieEtCIDetail
    Private _SelectedPatient As PatientBase
    Private _SelectedMedicamentId As Integer
    Private _SelectedTraitementId As Integer
    Private _CodeRetour As Boolean

    Public Property SelectedPatient As PatientBase
        Get
            Return _SelectedPatient
        End Get
        Set(value As PatientBase)
            _SelectedPatient = value
        End Set
    End Property

    Public Property SelectedMedicamentId As Integer
        Get
            Return _SelectedMedicamentId
        End Get
        Set(value As Integer)
            _SelectedMedicamentId = value
        End Set
    End Property

    Public Property SelectedTraitementId As Integer
        Get
            Return _SelectedTraitementId
        End Get
        Set(value As Integer)
            _SelectedTraitementId = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return _CodeRetour
        End Get
        Set(value As Boolean)
            _CodeRetour = value
        End Set
    End Property

    Dim theriaqueDao As New TheriaqueDao
    Dim traitementDao As New TraitementDao

    Dim traitement As Traitement

    Private Sub RadFDeclarationAllergieEtCIDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementEtatCivil()
        ChargementMedicament()
        If SelectedTraitementId <> 0 Then
            ChargementTraitementExistant()
        Else
            ChkAllergie.Checked = False
            ChkContreIndication.Checked = False
            traitement = New Traitement()
            traitement.PatientId = SelectedPatient.patientId
            traitement.MedicamentId = SelectedMedicamentId
            traitement.MedicamentDci = LblMedicamentDCI.Text
            traitement.DenominationLongue = LblMedicamentDenominationLongue.Text
            traitement.Allergie = False
            traitement.ContreIndication = False
            traitement.ArretCommentaire = ""
        End If

    End Sub

    Private Sub ChargementEtatCivil()
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

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    Private Sub ChargementMedicament()
        LblTraitementMedicamentCIS.Text = SelectedMedicamentId
        Dim dt As DataTable
        dt = theriaqueDao.getSpecialiteByArgument(SelectedMedicamentId.ToString, TheriaqueDao.EnumGetSpecialite.ID_THERIAQUE, TheriaqueDao.EnumMonoVir.NULL)
        If dt.Rows.Count > 0 Then
            LblMedicamentDCI.Text = dt.Rows(0)("SP_NOM")
            LblMedicamentDCI.Text = LblMedicamentDCI.Text.Replace(" §", "")
            LblMedicamentDenominationLongue.Text = dt.Rows(0)("SP_NOMLONG")
            LblMedicamentDenominationLongue.Text = LblMedicamentDenominationLongue.Text.Replace("(MEDICAMENT VIRTUEL)", "")
        Else
            LblMedicamentDCI.Text = ""
            LblMedicamentDenominationLongue.Text = ""
            LblMedicamentAdministration.Text = ""
            LblMedicamentTitulaire.Text = ""
        End If
    End Sub

    Private Sub ChargementTraitementExistant()
        traitement = traitementDao.getTraitementById(SelectedTraitementId)
        ChargerZonesArret(traitement)
    End Sub

    Private Sub ChargerZonesArret(traitement As Traitement)
        TxtCommentaireArret.Text = traitement.ArretCommentaire
        ChkAllergie.Checked = False
        If traitement.Allergie = True Then
            ChkAllergie.Checked = True
            ChkAllergie.ForeColor = Color.Red
        Else
            ChkAllergie.ForeColor = Color.Black
        End If

        ChkContreIndication.Checked = False
        If traitement.ContreIndication = True Then
            ChkContreIndication.Checked = True
            ChkContreIndication.ForeColor = Color.Red
        Else
            ChkContreIndication.ForeColor = Color.Black
        End If
    End Sub

    Private Sub ChkAllergie_Click(sender As Object, e As EventArgs) Handles ChkAllergie.Click
        If ChkAllergie.Checked = True Then
            traitement.Allergie = True
            traitement.ContreIndication = False
            ChkContreIndication.Checked = False
            ChkContreIndication.ForeColor = Color.Black
            ChkAllergie.ForeColor = Color.Red
        Else
            ChkAllergie.Checked = False
            traitement.Allergie = False
        End If
    End Sub

    Private Sub ChkContreIndication_Click(sender As Object, e As EventArgs) Handles ChkContreIndication.Click
        If ChkContreIndication.Checked = True Then
            traitement.Allergie = False
            traitement.ContreIndication = True
            ChkAllergie.Checked = False
            ChkContreIndication.ForeColor = Color.Red
            ChkAllergie.ForeColor = Color.Black
        Else
            ChkContreIndication.Checked = False
            traitement.ContreIndication = False
        End If
    End Sub

    Private Sub RadBtnRetour_Click(sender As Object, e As EventArgs) Handles RadBtnRetour.Click
        Close()
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        If traitement.Allergie = False Then
            If traitement.ContreIndication = False Then
                MessageBox.Show("Validation impossible, vous devez déclarer soit une allergie ou une contre-indication !")
                Exit Sub
            End If
        End If

        If DeclarationArret() = True Then
            Me.CodeRetour = True
            Close()
        End If
    End Sub


    'Annulation d'un traitement en base de données
    Private Function DeclarationArret() As Boolean
        Cursor.Current = Cursors.WaitCursor
        Dim codeRetour As Boolean

        codeRetour = traitementDao.DeclarationTraitementAllergieOuCI(traitement, userLog)
        If codeRetour = True Then
            Dim form As New RadFNotification()
            If traitement.Allergie = True Then
                form.Message = "Déclaration allergie validée"
            Else
                form.Message = "Déclaration contre-indication validée"
            End If
            form.Show()
        End If

        Cursor.Current = Cursors.Default
        Return codeRetour
    End Function

    Private Sub TxtCommentaireArret_TextChanged(sender As Object, e As EventArgs) Handles TxtCommentaireArret.TextChanged
        traitement.ArretCommentaire = TxtCommentaireArret.Text
    End Sub
End Class
