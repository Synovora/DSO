Imports System.Data.SqlClient
Imports Oasis_Common
Public Class AllergieDao
    Inherits StandardDao

    Public Function GetAllergieSubstanceById(SubstanceId As Long) As Allergie
        Dim allergie As Allergie
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_patient_allergie WHERE allergie_id = @Id"
            command.Parameters.AddWithValue("@id", SubstanceId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    allergie = BuildBean(reader)
                Else
                    Throw New ArgumentException("Contre-indication substance inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return allergie
    End Function

    Private Function BuildBean(reader As SqlDataReader) As Allergie
        Dim allergie As New Allergie

        allergie.AllergieId = reader("allergie_id")
        allergie.PatientId = Coalesce(reader("patient_id"), 0)
        allergie.SubstanceId = Coalesce(reader("substance_id"), 0)
        allergie.SubstancePereId = Coalesce(reader("substance_pere_id"), 0)
        allergie.DenominationSubstance = Coalesce(reader("denomination_substance"), "")
        allergie.DenominationSubstancePere = Coalesce(reader("denomination_substance_pere"), "")
        allergie.UserCreation = Coalesce(reader("creation_user_id"), 0)
        allergie.DateCreation = Coalesce(reader("creation_date"), Nothing)
        allergie.UserAnnulation = Coalesce(reader("annulation_user_id"), 0)
        allergie.DateAnnulation = Coalesce(reader("annulation_date"), Nothing)
        allergie.Inactif = Coalesce(reader("inactif"), False)

        Return allergie
    End Function

    Public Function GetAllAllergiebyPatient(patientId As Integer) As DataTable
        Dim SQLString As String = "SELECT * " &
        " FROM oasis.oa_patient_allergie" &
        " WHERE (inactif Is Null OR inactif = 'False')" &
        " AND patient_id = " & patientId.ToString &
        " ORDER BY denomination_substance"

        Using con As SqlConnection = GetConnection()
            Dim TraitementDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using TraitementDataAdapter
                TraitementDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim TraitementDataTable As DataTable = New DataTable()
                Using TraitementDataTable
                    Try
                        TraitementDataAdapter.Fill(TraitementDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return TraitementDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function CreationAllergie(allergie As Allergie, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim n As Integer 'Pour récupérer le nombre d'occurences enregistrées
        Dim theriaqueDao As New TheriaqueDao

        Dim SQLstring As String = "IF NOT EXISTS" &
            " (SELECT 1 FROM oasis.oa_patient_allergie" &
            " WHERE patient_id = @patientId"

        Dim SqlStringCond1 As String = " AND substance_id = @substanceId"

        Dim SqlStringCond2 As String = " AND substance_pere_id = @substancePereId"

        Dim SqlStringFin As String = " AND (inactif Is Null Or inactif = 'False'))" &
            " INSERT INTO oasis.oa_patient_allergie" &
            " (patient_id, substance_id, substance_pere_id, denomination_substance, denomination_substance_pere, creation_user_id, creation_date, inactif)" &
            " VALUES" &
            " (@patientId, @substanceId, @substancePereId, @substanceDenomination, @substanceDenominationPere, @userCreation, @dateCreation, @inactif)"

        If allergie.SubstancePereId = 0 Then
            SQLstring += SqlStringCond1
            allergie.DenominationSubstancePere = ""
        Else
            SQLstring += SqlStringCond2
            allergie.SubstanceId = 0
            allergie.DenominationSubstance = ""
            allergie.DenominationSubstancePere = theriaqueDao.GetSubstancePereDenominationById(allergie.SubstancePereId)
        End If

        SQLstring += SqlStringFin

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@patientId", allergie.PatientId.ToString)
            .AddWithValue("@substanceId", allergie.SubstanceId)
            .AddWithValue("@substancePereId", allergie.SubstancePereId)
            .AddWithValue("@substanceDenomination", allergie.DenominationSubstance)
            .AddWithValue("@substanceDenominationPere", allergie.DenominationSubstancePere)
            .AddWithValue("@userCreation", userLog.UtilisateurId)
            .AddWithValue("@dateCreation", Date.Now())
            .AddWithValue("@inactif", False)
        End With

        Try
            da.InsertCommand = cmd
            n = da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        If n <= 0 Then
            codeRetour = False
        End If

        Return codeRetour
    End Function

    Public Function AnnulationAllergie(allergieId As Integer, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim n As Integer 'Pour récupérer le nombre d'occurences enregistrées

        Dim SQLstring As String = "UPDATE oasis.oa_patient_allergie" &
            " SET inactif = @inactif, annulation_user_id = @user, annulation_date = @dateAnnulation" &
            " WHERE allergie_id = @allergieId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@allergieId", allergieId)
            .AddWithValue("@inactif", True)
            .AddWithValue("@user", userLog.UtilisateurId)
            .AddWithValue("@dateAnnulation", Date.Now())
        End With

        Try
            da.InsertCommand = cmd
            n = da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        If n <= 0 Then
            codeRetour = False
        End If

        Return codeRetour
    End Function
End Class
