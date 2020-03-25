Imports System.Data.SqlClient
Imports Oasis_Common
Public Class ContreIndicationATCDao
    Inherits StandardDao

    Friend Function GetContreIndicationATCById(contreIndicationATCId As Long) As ContreIndicationATC
        Dim contreIndication As ContreIndicationATC
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_patient_contre_indication_atc WHERE contre_indication_id = @Id"
            command.Parameters.AddWithValue("@id", contreIndicationATCId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    contreIndication = BuildBean(reader)
                Else
                    Throw New ArgumentException("Contre-indication ATC inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return contreIndication
    End Function

    Private Function BuildBean(reader As SqlDataReader) As ContreIndicationATC
        Dim contreIndication As New ContreIndicationATC

        contreIndication.ContreIndicationId = reader("contre_indication_id")
        contreIndication.PatientId = Coalesce(reader("patient_id"), 0)
        contreIndication.ATCId = Coalesce(reader("code_atc"), "")
        contreIndication.DenominationATC = Coalesce(reader("Denomination_atc"), "")
        contreIndication.UserCreation = Coalesce(reader("creation_user_id"), 0)
        contreIndication.DateCreation = Coalesce(reader("creation_date"), Nothing)
        contreIndication.UserAnnulation = Coalesce(reader("annulation_user_id"), 0)
        contreIndication.DateAnnulation = Coalesce(reader("annulation_date"), Nothing)
        contreIndication.Inactif = Coalesce(reader("inactif"), False)

        Return contreIndication
    End Function

    Public Function getAllContreIndicationATCbyPatient(patientId As Integer) As DataTable
        Dim SQLString As String = "SELECT * " &
        " FROM oasis.oa_patient_contre_indication_atc" &
        " WHERE (inactif Is Null OR inactif = 'False')" &
        " AND patient_id = " & patientId.ToString &
        " ORDER BY code_atc"

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

    Friend Function CreationContreIndicationATC(contreIndicationATC As ContreIndicationATC) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim n As Integer 'Pour récupérer le nombre d'occurences enregistrées

        Dim SQLstring As String = "IF NOT EXISTS" &
            " (SELECT 1 FROM oasis.oa_patient_contre_indication_atc" &
            " WHERE patient_id = @patientId" &
            " AND code_atc = @atcCode)" &
            " AND (inactif Is Null OR inactif = 'False')" &
            " INSERT INTO oasis.oa_patient_contre_indication_atc" &
            " (patient_id, code_atc, denomination_atc, creation_user_id, creation_date, inactif)" &
            " VALUES" &
            " (@patientId, @atcCode, @atcDenomination, @userCreation, @dateCreation, @inactif)"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@patientId", contreIndicationATC.PatientId.ToString)
            .AddWithValue("@atcCode", contreIndicationATC.ATCId)
            .AddWithValue("@atcDenomination", contreIndicationATC.DenominationATC)
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


End Class
