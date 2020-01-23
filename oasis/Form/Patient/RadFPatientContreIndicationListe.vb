Imports System.Collections.Specialized
Imports Oasis_Common

Public Class RadFPatientContreIndicationListe
    Private privateSelectedPatientId As Integer
    Private privateSelectedPatient As Patient
    Private privateSelectedPatientCICis As StringCollection
    Private privateUtilisateurConnecte As Utilisateur
    Private privateCodeRetour As Boolean

    Public Property SelectedPatientId As Integer
        Get
            Return privateSelectedPatientId
        End Get
        Set(value As Integer)
            privateSelectedPatientId = value
        End Set
    End Property

    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return privateCodeRetour
        End Get
        Set(value As Boolean)
            privateCodeRetour = value
        End Set
    End Property

    Public Property SelectedPatientCICis As StringCollection
        Get
            Return privateSelectedPatientCICis
        End Get
        Set(value As StringCollection)
            privateSelectedPatientCICis = value
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

    Private Sub RadFPatientContreIndicationListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementPatient()
        ChargementAllergiesPatient()
    End Sub

    'Chargement de la Grid Notes patient
    Private Sub ChargementAllergiesPatient()
        Dim allergieString As String
        Dim SubstancesAllergiques As StringCollection = ListeSubstancesAllergiques(SelectedPatientCICis)
        Dim allergieEnumerator As StringEnumerator = SubstancesAllergiques.GetEnumerator()

        Dim iGrid As Integer = -1

        While allergieEnumerator.MoveNext()
            allergieString = allergieEnumerator.Current.ToString
            iGrid += 1
            RadAllergiesPatientDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadAllergiesPatientDataGridView.Rows(iGrid).Cells("allergie").Value = allergieString
        End While

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

    Private Sub BtnMedicament_Click(sender As Object, e As EventArgs) Handles RadBtnMedicament.Click
        Using vFTraitementAllergieEtCI As New RadFTraitementAllergieEtCI
            vFTraitementAllergieEtCI.SelectedPatient = Me.SelectedPatient
            vFTraitementAllergieEtCI.UtilisateurConnecte = Me.UtilisateurConnecte
            vFTraitementAllergieEtCI.AllergieOuContreIndication = EnumAllergieOuContreIndication.ContreIndication
            vFTraitementAllergieEtCI.ShowDialog() 'Modal
        End Using
    End Sub

    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Me.CodeRetour = False
        Close()
    End Sub
End Class
