
Imports System.Data.SqlClient

Public Class PatientDaoBase
    Inherits StandardDao

    Public Structure EnumGenreId
        Const Feminin = "F"
        Const Masculin = "M"
    End Structure

    'Initialisation des propriétés d'une instance de Patient depuis la BDD
    Public Overridable Function SetPatient(patientId As Integer) As PatientBase
        Dim patient = New PatientBase
        If patientId = 0 Then
            Return Nothing
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

    Private Function BuildBean(patientDataReader As SqlDataReader) As PatientBase
        Dim patient As New PatientBase With {
            .patientId = Convert.ToInt64(patientDataReader("oa_patient_id"))
        }

        If patientDataReader("oa_patient_nir") Is DBNull.Value Then
            patient.PatientNir = 0
        Else
            patient.PatientNir = Convert.ToInt64(patientDataReader("oa_patient_nir"))
        End If

        If patientDataReader("oa_patient_nom") Is DBNull.Value Then
            patient.PatientNom = ""
        Else
            patient.PatientNom = patientDataReader("oa_patient_nom")
        End If

        If patientDataReader("oa_patient_prenom") Is DBNull.Value Then
            patient.PatientPrenom = ""
        Else
            patient.PatientPrenom = patientDataReader("oa_patient_prenom")
        End If

        If patientDataReader("oa_patient_date_naissance") Is DBNull.Value Then
            patient.PatientDateNaissance = Nothing
        Else
            patient.PatientDateNaissance = CDate(patientDataReader("oa_patient_date_naissance"))
        End If

        If patientDataReader("oa_patient_genre_id") Is DBNull.Value Then
            patient.PatientGenreId = ""
        Else
            patient.PatientGenreId = patientDataReader("oa_patient_genre_id")
        End If

        If patientDataReader("oa_patient_adresse1") Is DBNull.Value Then
            patient.PatientAdresse1 = ""
        Else
            patient.PatientAdresse1 = patientDataReader("oa_patient_adresse1")
        End If

        If patientDataReader("oa_patient_adresse2") Is DBNull.Value Then
            patient.PatientAdresse2 = ""
        Else
            patient.PatientAdresse2 = patientDataReader("oa_patient_adresse2")
        End If

        If patientDataReader("oa_patient_code_postal") Is DBNull.Value Then
            patient.PatientCodePostal = ""
        Else
            patient.PatientCodePostal = patientDataReader("oa_patient_code_postal")
        End If

        If patientDataReader("oa_patient_ville") Is DBNull.Value Then
            patient.PatientVille = ""
        Else
            patient.PatientVille = patientDataReader("oa_patient_ville")
        End If

        If patientDataReader("oa_patient_tel1") Is DBNull.Value Then
            patient.PatientTel1 = ""
        Else
            patient.PatientTel1 = patientDataReader("oa_patient_tel1")
        End If

        If patientDataReader("oa_patient_tel2") Is DBNull.Value Then
            patient.PatientTel2 = ""
        Else
            patient.PatientTel2 = patientDataReader("oa_patient_tel2")
        End If

        If patientDataReader("oa_patient_email") Is DBNull.Value Then
            patient.PatientEmail = ""
        Else
            patient.PatientEmail = patientDataReader("oa_patient_email")
        End If

        If patientDataReader("oa_patient_nom_marital") Is DBNull.Value Then
            patient.PatientNomMarital = ""
        Else
            patient.PatientNomMarital = patientDataReader("oa_patient_nom_marital")
        End If

        If patientDataReader("oa_patient_date_entree_oasis") Is DBNull.Value Then
            patient.PatientDateEntree = Nothing
        Else
            patient.PatientDateEntree = CDate(patientDataReader("oa_patient_date_entree_oasis"))
        End If

        If patientDataReader("oa_patient_date_sortie_oasis") Is DBNull.Value Then
            patient.PatientDateSortie = Nothing
        Else
            patient.PatientDateSortie = CDate(patientDataReader("oa_patient_date_sortie_oasis"))
        End If

        If patientDataReader("oa_patient_commentaire_sortie") Is DBNull.Value Then
            patient.PatientCommentaireSortie = ""
        Else
            patient.PatientCommentaireSortie = patientDataReader("oa_patient_commentaire_sortie")
        End If

        If patientDataReader("oa_patient_date_deces") Is DBNull.Value Then
            patient.PatientDateDeces = Nothing
        Else
            patient.PatientDateDeces = CDate(patientDataReader("oa_patient_date_deces"))
        End If

        If patientDataReader("oa_patient_site_id") Is DBNull.Value Then
            patient.PatientSiteId = 0
        Else
            patient.PatientSiteId = CInt(patientDataReader("oa_patient_site_id"))
        End If

        If patientDataReader("oa_patient_couverture_internet") Is DBNull.Value Then
            patient.PatientInternet = False
        Else
            patient.PatientInternet = CInt(patientDataReader("oa_patient_couverture_internet"))
        End If

        If patientDataReader("oa_patient_unite_sanitaire_id") Is DBNull.Value Then
            patient.PatientUniteSanitaireId = 0
        Else
            patient.PatientUniteSanitaireId = CInt(patientDataReader("oa_patient_unite_sanitaire_id"))
        End If

        If patientDataReader("oa_patient_synthese_date_maj") Is DBNull.Value Then
            patient.PatientSyntheseDateMaj = Nothing
        Else
            patient.PatientSyntheseDateMaj = CDate(patientDataReader("oa_patient_synthese_date_maj"))
        End If

        If patientDataReader("oa_patient_date_naissance") Is DBNull.Value Then
            patient.PatientAge = "Inconnu"
        Else
            patient.PatientAge = CalculAgeEnAnneeEtMoisString(patient.PatientDateNaissance)
            patient.PatientAgeEnAnnee = CalculAgeEnAnnee(patient.PatientDateNaissance)
        End If
        Dim genre_description As String = Table_genre.GetGenreDescription(patient.PatientGenreId)
        If genre_description = "" Then
            patient.PatientGenre = "genre ?"
        Else
            patient.PatientGenre = genre_description
        End If
        patient.Profession = Coalesce(patientDataReader("oa_patient_profession"), "")
        patient.PharmacienId = Coalesce(patientDataReader("oa_patient_pharmacie_id"), 0)
        patient.Taille = Coalesce(patientDataReader("oa_patient_taille"), 0)
        patient.BlocageMedical = Coalesce(patientDataReader("oa_patient_blocage_medical"), False)
        patient.INS = Coalesce(patientDataReader("oa_patient_INS"), 0)
        Return patient
    End Function

    Public Overridable Function GetPatientById(id As Long) As PatientBase
        Dim patient As PatientBase = SetPatient(id)
        If patient Is Nothing Then
            Throw New ArgumentException("Patient non retrouvé !")
        End If
        Return patient
    End Function

End Class
