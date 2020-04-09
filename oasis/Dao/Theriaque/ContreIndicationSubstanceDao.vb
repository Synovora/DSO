Imports System.Data.SqlClient
Imports Oasis_Common
Public Class ContreIndicationSubstanceDao
    Inherits StandardDao

    Friend Function GetContreIndicationSubstanceById(contreIndicationSubstanceId As Long) As ContreIndicationSubstance
        Dim contreIndication As ContreIndicationSubstance
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_patient_contre_indication_substance WHERE contre_indication_id = @Id"
            command.Parameters.AddWithValue("@id", contreIndicationSubstanceId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    contreIndication = BuildBean(reader)
                Else
                    Throw New ArgumentException("Contre-indication substance inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return contreIndication
    End Function

    Private Function BuildBean(reader As SqlDataReader) As ContreIndicationSubstance
        Dim contreIndication As New ContreIndicationSubstance

        contreIndication.ContreIndicationId = reader("contre_indication_id")
        contreIndication.PatientId = Coalesce(reader("patient_id"), 0)
        contreIndication.SubstanceId = Coalesce(reader("substance_id"), 0)
        contreIndication.SubstancePereId = Coalesce(reader("substance_pere_id"), 0)
        contreIndication.DenominationSubstance = Coalesce(reader("denomination_substance"), "")
        contreIndication.DenominationSubstancePere = Coalesce(reader("denomination_substance_pere"), "")
        contreIndication.UserCreation = Coalesce(reader("creation_user_id"), 0)
        contreIndication.DateCreation = Coalesce(reader("creation_date"), Nothing)
        contreIndication.UserAnnulation = Coalesce(reader("annulation_user_id"), 0)
        contreIndication.DateAnnulation = Coalesce(reader("annulation_date"), Nothing)
        contreIndication.Inactif = Coalesce(reader("inactif"), False)

        Return contreIndication
    End Function

    Public Function getAllContreIndicationSubstancebyPatient(patientId As Integer) As DataTable
        Dim SQLString As String = "SELECT * " &
        " FROM oasis.oa_patient_contre_indication_substance" &
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

    Friend Function CreationContreIndicationSubstance(contreIndication As ContreIndicationSubstance) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim n As Integer 'Pour récupérer le nombre d'occurences enregistrées
        Dim theriaqueDao As New TheriaqueDao

        Dim SQLstring As String = "IF NOT EXISTS" &
            " (SELECT 1 FROM oasis.oa_patient_contre_indication_substance" &
            " WHERE patient_id = @patientId"

        Dim SqlStringCond1 As String = " AND substance_id = @substanceId"

        Dim SqlStringCond2 As String = " AND substance_pere_id = @substancePereId"

        Dim SqlStringFin As String = " AND (inactif Is Null OR inactif = 'False'))" &
            " INSERT INTO oasis.oa_patient_contre_indication_substance" &
            " (patient_id, substance_id, substance_pere_id, denomination_substance, denomination_substance_pere, creation_user_id, creation_date, inactif)" &
            " VALUES" &
            " (@patientId, @substanceId, @substancePereId, @substanceDenomination, @substanceDenominationPere, @userCreation, @dateCreation, @inactif)"

        If contreIndication.SubstancePereId = 0 Then
            SQLstring += SqlStringCond1
            contreIndication.DenominationSubstancePere = ""
        Else
            SQLstring += SqlStringCond2
            contreIndication.SubstanceId = 0
            contreIndication.DenominationSubstance = ""
            contreIndication.DenominationSubstancePere = theriaqueDao.getSubstancePereDenominationById(contreIndication.SubstancePereId)
        End If

        SQLstring += SqlStringFin

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@patientId", contreIndication.PatientId.ToString)
            .AddWithValue("@substanceId", contreIndication.SubstanceId)
            .AddWithValue("@substancePereId", contreIndication.SubstancePereId)
            .AddWithValue("@substanceDenomination", contreIndication.DenominationSubstance)
            .AddWithValue("@substanceDenominationPere", contreIndication.DenominationSubstancePere)
            .AddWithValue("@userCreation", userLog.UtilisateurId)
            .AddWithValue("@dateCreation", Date.Now())
            .AddWithValue("@inactif", False)
        End With

        Try
            da.InsertCommand = cmd
            n = da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        If n <= 0 Then
            codeRetour = False
        End If

        Return codeRetour
    End Function

    Friend Function AnnulationContreIndicationSubstance(contreIndicationId) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim n As Integer 'Pour récupérer le nombre d'occurences enregistrées

        Dim SQLstring As String = "UPDATE oasis.oa_patient_contre_indication_substance" &
            " SET inactif = @inactif, annulation_user_id = @user, annulation_date = @dateAnnulation" &
            " WHERE contre_indication_id = @contreIndicationId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@contreIndicationId", contreIndicationId)
            .AddWithValue("@inactif", True)
            .AddWithValue("@user", userLog.UtilisateurId)
            .AddWithValue("@dateAnnulation", Date.Now())
        End With

        Try
            da.InsertCommand = cmd
            n = da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
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
