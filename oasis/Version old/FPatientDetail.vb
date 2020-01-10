'Détail patient

Imports Oasis_WF

Public Class FPatientDetail
    Private privateSelectedPatient As Patient

    Public Property SelectedPatient As Patient
        Get
            Return privateSelectedPatient
        End Get
        Set(value As Patient)
            privateSelectedPatient = value
        End Set
    End Property

    Private Sub Form5_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        LblPatientId.Text = SelectedPatient.patientId

        LblPatientNir.Text = SelectedPatient.PatientNir

        LblPatientNom.Text = SelectedPatient.PatientNom

        LblPatientPrenom.Text = SelectedPatient.PatientPrenom

        If SelectedPatient.PatientDateNaissance = Nothing Then
            LblPatientDateNaissance.Text = "Date non renseignée"
            LblPatientAge.Text = ""
        Else
            LblPatientDateNaissance.Text = SelectedPatient.PatientDateNaissance.ToString("dd/MM/yyyy")
            LblPatientAge.Text = SelectedPatient.PatientAge
        End If

        Dim genre_description As String = table_genre.GetGenreDescription(SelectedPatient.PatientGenreId)
        If genre_description = "" Then
            LblPatientGenre.Text = "Genre non renseigné"
        Else
            LblPatientGenre.Text = genre_description
        End If

        LblPatientVille.Text = SelectedPatient.PatientVille

        LblPatientSiteId.Text = SelectedPatient.PatientSiteId
    End Sub
End Class