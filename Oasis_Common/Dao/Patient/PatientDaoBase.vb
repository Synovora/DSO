
Imports System.Data.SqlClient

Public Class PatientDaoBase
    Inherits StandardDao

    Public Structure EnumGenreId
        Const Feminin = "F"
        Const Masculin = "M"
    End Structure

    'Initialisation des propriétés d'une instance de Patient depuis la BDD
    Public Overridable Function SetPatient(patientId As Integer) As Patient
        Dim patient As New Patient
        If patientId = 0 Then
            Return patient
        End If
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText =
                    "SELECT * FROM oasis.oa_patient where oa_patient_id = @patientId"
            command.Parameters.AddWithValue("@patientId", patientId.ToString)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    patient = BuildBean(reader)
                Else
                    Throw New ArgumentException("Patient inexistante !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return patient
    End Function

    Private Function BuildBean(reader As SqlDataReader) As Patient
        Dim patient As New Patient With {
            .PatientId = Convert.ToInt64(reader("oa_patient_id")),
            .PatientNir = Coalesce(reader("oa_patient_nir"), 0),
            .PatientNom = Coalesce(reader("oa_patient_nom"), ""),
            .PatientPrenom = Coalesce(reader("oa_patient_prenom"), ""),
            .PatientDateNaissance = Coalesce(reader("oa_patient_date_naissance"), Nothing),
            .PatientGenreId = Coalesce(reader("oa_patient_genre_id"), ""),
            .PatientAdresse1 = Coalesce(reader("oa_patient_adresse1"), ""),
            .PatientAdresse2 = Coalesce(reader("oa_patient_adresse2"), ""),
            .PatientCodePostal = Coalesce(reader("oa_patient_code_postal"), ""),
            .PatientVille = Coalesce(reader("oa_patient_ville"), ""),
            .PatientTel1 = Coalesce(reader("oa_patient_tel1"), ""),
            .PatientTel2 = Coalesce(reader("oa_patient_tel2"), ""),
            .PatientEmail = Coalesce(reader("oa_patient_email"), ""),
            .PatientNomMarital = Coalesce(reader("oa_patient_nom_marital"), ""),
            .PatientDateEntree = Coalesce(CDate(reader("oa_patient_date_entree_oasis")), Nothing),
            .PatientDateSortie = Coalesce(CDate(reader("oa_patient_date_sortie_oasis")), Nothing),
            .PatientCommentaireSortie = Coalesce(reader("oa_patient_commentaire_sortie"), ""),
            .PatientDateDeces = Coalesce(reader("oa_patient_date_deces"), Nothing),
            .PatientSiteId = Coalesce(reader("oa_patient_site_id"), 0),
            .PatientInternet = Coalesce(CInt(reader("oa_patient_couverture_internet")), False),
            .PatientUniteSanitaireId = Coalesce(CInt(reader("oa_patient_unite_sanitaire_id")), 0),
            .PatientSyntheseDateMaj = Coalesce(CDate(reader("oa_patient_synthese_date_maj")), 0),
            .Profession = Coalesce(reader("oa_patient_profession"), ""),
            .PharmacienId = Coalesce(reader("oa_patient_pharmacie_id"), 0),
            .Taille = Coalesce(reader("oa_patient_taille"), 0),
            .BlocageMedical = Coalesce(reader("oa_patient_blocage_medical"), False),
            .INS = Coalesce(reader("oa_patient_INS"), 0)
        }
        patient.PatientGenre = Coalesce(CType(Table_genre.GetGenreDescription(patient.PatientGenreId), String), "genre ?")
        patient.PatientAge = Coalesce(CalculAgeEnAnneeEtMoisString(patient.PatientDateNaissance), "Inconnu")
        patient.PatientAgeEnAnnee = Coalesce(CalculAgeEnAnnee(patient.PatientDateNaissance), "Inconnu")
        Return patient
    End Function

    Public Overridable Function GetPatientById(id As Long) As Patient
        Dim patient As Patient = SetPatient(id)
        If patient Is Nothing Then
            Throw New ArgumentException("Patient non retrouvé !")
        End If
        Return patient
    End Function

End Class
