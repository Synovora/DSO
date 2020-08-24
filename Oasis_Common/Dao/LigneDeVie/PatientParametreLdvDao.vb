Imports System.Data.SqlClient

Public Class PatientParametreLdvDao
    Inherits StandardDao

    Private Function BuildBean(reader As SqlDataReader) As PatientParametreLdv
        Dim patientParametreLdv As New PatientParametreLdv With {
            .PatientId = Coalesce(reader("patient_id"), 0),
            .ActivitePathologieAigue = Coalesce(reader("activite_pathologie_aigue"), False),
            .ActivitePreventionAutre = Coalesce(reader("activite_prevention_autre"), False),
            .ActivitePreventionEnfantPreScolaire = Coalesce(reader("activite_prevention_enfant_pre_scolaire"), False),
            .ActivitePreventionEnfantScolaire = Coalesce(reader("activite_prevention_enfant_scolaire"), False),
            .ActiviteSuiviGrossesse = Coalesce(reader("activite_suivi_grossesse"), False),
            .ActiviteSuiviGynecologique = Coalesce(reader("activite_suivi_gynecologique"), False),
            .ActiviteSocial = Coalesce(reader("activite_social"), False),
            .ActiviteSuiviChronique = Coalesce(reader("activite_suivi_chronique"), False),
            .TypeConsultation = Coalesce(reader("type_consultation"), False),
            .TypeVirtuel = Coalesce(reader("type_virtuel"), False),
            .TypeParametre = Coalesce(reader("type_parametre"), False),
            .ProfilMedical = Coalesce(reader("profil_medical"), False),
            .ProfilParamedical = Coalesce(reader("profil_paramedical"), False),
            .ProfilPatient = Coalesce(reader("profil_patient"), False),
            .Parametre1 = Coalesce(reader("parametre1"), 0),
            .Parametre2 = Coalesce(reader("parametre2"), 0),
            .Parametre3 = Coalesce(reader("parametre3"), 0),
            .Parametre4 = Coalesce(reader("parametre4"), 0),
            .Parametre5 = Coalesce(reader("parametre5"), 0),
            .UserModification = Coalesce(reader("user_modification"), 0),
            .DateModification = Coalesce(reader("date_modification"), Nothing)
        }
        Return patientParametreLdv
    End Function

    Public Function GetParametreByPatientId(patientId As Integer) As PatientParametreLdv
        Dim patientParametreLdv As PatientParametreLdv
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_patient_parametre_ldv WHERE patient_id = @id"
            command.Parameters.AddWithValue("@id", patientId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    patientParametreLdv = BuildBean(reader)
                Else
                    Throw New ArgumentException("RNF - Paramètres de ligne de vie inexistant pour ce patient !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return patientParametreLdv
    End Function

    Public Function CreateConfigurationParametre(patientparametreldv As PatientParametreLdv, userLog As Utilisateur) As Boolean
        Dim nbcreate As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim episodeIdCree As Integer = 0
        Dim CodeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String =
        "IF Not EXISTS (SELECT 1 FROM oasis.oa_patient_parametre_ldv WHERE patient_id = @patientId)" &
        " INSERT INTO oasis.oa_patient_parametre_ldv" &
        " (patient_id, activite_pathologie_aigue, activite_prevention_autre, activite_prevention_enfant_pre_scolaire, activite_prevention_enfant_scolaire," &
        " activite_suivi_grossesse, activite_suivi_gynecologique, activite_social, activite_suivi_chronique, type_consultation, type_virtuel, type_parametre," &
        " profil_medical, profil_paramedical, profil_patient, parametre1, parametre2, parametre3, parametre4, parametre5, user_modification, date_modification)" &
        " VALUES (@patientId, @activitePathologieAigue, @activitePreventionAutre, @activitePreventionEnfantPreScolaire, @activitePreventionEnfantScolaire," &
        " @activiteSuiviGrossesse, @activiteSuiviGynecologique, @activiteSocial, @activiteSuiviChronique, @typeConsultation, @typeVirtuel, @typeParametre," &
        " @profilMedical, @profilParamedical, @Parametre1, @Parametre2, @Parametre3, @Parametre4, @Parametre5, @UserCreation, @dateCreation)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@patientId", patientparametreldv.PatientId)
            .AddWithValue("@activitePathologieAigue", patientparametreldv.ActivitePathologieAigue)
            .AddWithValue("@activitePreventionAutre", patientparametreldv.ActivitePreventionAutre)
            .AddWithValue("@activitePreventionEnfantPreScolaire", patientparametreldv.ActivitePreventionEnfantPreScolaire)
            .AddWithValue("@activitePreventionEnfantScolaire", patientparametreldv.ActivitePreventionEnfantScolaire)
            .AddWithValue("@activiteSuiviGrossesse", patientparametreldv.ActiviteSuiviGrossesse)
            .AddWithValue("@activiteSuiviGynecologique", patientparametreldv.ActiviteSuiviGynecologique)
            .AddWithValue("@activiteSocial", patientparametreldv.ActiviteSocial)
            .AddWithValue("@activiteSuiviChronique", patientparametreldv.ActiviteSuiviChronique)
            .AddWithValue("@typeConsultation", patientparametreldv.TypeConsultation)
            .AddWithValue("@typeVirtuel", patientparametreldv.TypeVirtuel)
            .AddWithValue("@typeParametre", patientparametreldv.TypeParametre)
            .AddWithValue("@profilMedical", patientparametreldv.ProfilMedical)
            .AddWithValue("@profilParamedical", patientparametreldv.ProfilParamedical)
            .AddWithValue("@profilPatient", patientparametreldv.ProfilParamedical)
            .AddWithValue("@Parametre1", patientparametreldv.Parametre1)
            .AddWithValue("@Parametre2", patientparametreldv.Parametre2)
            .AddWithValue("@Parametre3", patientparametreldv.Parametre3)
            .AddWithValue("@Parametre4", patientparametreldv.Parametre4)
            .AddWithValue("@Parametre5", patientparametreldv.Parametre5)
            .AddWithValue("@userCreation", userLog.UtilisateurId)
            .AddWithValue("@dateCreation", Date.Now)
        End With

        Try
            da.InsertCommand = cmd
            nbcreate = da.InsertCommand.ExecuteNonQuery()
            If nbcreate <= 0 Then
                Throw New Exception("Collision: Un épisode en cours existe déjà pour ce patient")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
            CodeRetour = False
        Finally
            con.Close()
        End Try

        Return CodeRetour
    End Function

    Public Function UpdateConfigurationParametre(patientParametreLdv As PatientParametreLdv, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "UPDATE oasis.oa_patient_parametre_ldv SET" &
        " patient_id = @patientId," &
        " activite_pathologie_aigue = @activitePathologieAigue," &
        " activite_prevention_autre = @activitePreventionAutre," &
        " activite_prevention_enfant_pre_scolaire = @activitePreventionEnfantPreScolaire," &
        " activite_prevention_enfant_scolaire = @activitePreventionEnfantScolaire," &
        " activite_suivi_grossesse = @activiteSuiviGrossesse," &
        " activite_suivi_gynecologique = @activiteSuiviGynecologique," &
        " activite_social = @activiteSocial," &
        " activite_suivi_chronique = @activiteSuiviChronique," &
        " type_consultation = @typeConsultation," &
        " type_virtuel = @typeVirtuel," &
        " type_parametre = @typeParametre," &
        " profil_medical = @profilMedical," &
        " profil_paramedical = @profilParamedical," &
        " profil_patient = @profilPatient," &
        " parametre1 = @Parametre1," &
        " parametre2 = @Parametre2," &
        " parametre3 = @Parametre3," &
        " parametre4 = @Parametre4," &
        " parametre5 = @Parametre5," &
        " user_modification = @userModification," &
        " date_modification = @dateModification" &
        " WHERE patient_id = @patientId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@patientId", patientParametreLdv.PatientId)
            .AddWithValue("@activitePathologieAigue", patientParametreLdv.ActivitePathologieAigue)
            .AddWithValue("@activitePreventionAutre", patientParametreLdv.ActivitePreventionAutre)
            .AddWithValue("@activitePreventionEnfantPreScolaire", patientParametreLdv.ActivitePreventionEnfantPreScolaire)
            .AddWithValue("@activitePreventionEnfantScolaire", patientParametreLdv.ActivitePreventionEnfantScolaire)
            .AddWithValue("@activiteSuiviGrossesse", patientParametreLdv.ActiviteSuiviGrossesse)
            .AddWithValue("@activiteSuiviGynecologique", patientParametreLdv.ActiviteSuiviGynecologique)
            .AddWithValue("@activiteSocial", patientParametreLdv.ActiviteSocial)
            .AddWithValue("@activiteSuiviChronique", patientParametreLdv.ActiviteSuiviChronique)
            .AddWithValue("@typeConsultation", patientParametreLdv.TypeConsultation)
            .AddWithValue("@typeVirtuel", patientParametreLdv.TypeVirtuel)
            .AddWithValue("@typeParametre", patientParametreLdv.TypeParametre)
            .AddWithValue("@profilMedical", patientParametreLdv.ProfilMedical)
            .AddWithValue("@profilParamedical", patientParametreLdv.ProfilParamedical)
            .AddWithValue("@profilPatient", patientParametreLdv.ProfilPatient)
            .AddWithValue("@Parametre1", patientParametreLdv.Parametre1)
            .AddWithValue("@Parametre2", patientParametreLdv.Parametre2)
            .AddWithValue("@Parametre3", patientParametreLdv.Parametre3)
            .AddWithValue("@Parametre4", patientParametreLdv.Parametre4)
            .AddWithValue("@Parametre5", patientParametreLdv.Parametre5)
            .AddWithValue("@userModification", userLog.UtilisateurId)
            .AddWithValue("@dateModification", Date.Now)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function
End Class
