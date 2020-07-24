Imports System.Data.SqlClient

Public Class AutoSuiviDao

    Inherits StandardDao

    Private Function BuildBean(reader As SqlDataReader) As AutoSuivi
        Return New AutoSuivi With {
            .PatientId = Coalesce(reader("patient_id"), 0),
            .ParametreId = Coalesce(reader("parametre_id"), 0)
        }
    End Function

    Public Function GetAutoSuiviByPatientIdAndParametreId(patientId As Long, parametreId As Long) As AutoSuivi
        Dim instance As AutoSuivi = Nothing
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "select * from oasis.oa_r_autosuivi where patient_id = @patient_id AND parametre_id = @parametre_id"
            command.Parameters.AddWithValue("@patient_id", patientId)
            command.Parameters.AddWithValue("@parametre_id", parametreId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    instance = BuildBean(reader)
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return instance
    End Function

    Public Function CreateAutoSuivi(autoSuivi As AutoSuivi) As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim autoSuiviId As Integer = 0
        Dim con As SqlConnection = GetConnection()
        Dim dateCreation As Date = Date.Now.Date
        Dim SQLstring As String = "INSERT INTO oasis.oa_r_autosuivi" &
        " (patient_id, parametre_id)" &
        " VALUES (@patient_id, @parametre_id)"
        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@patient_id", autoSuivi.PatientId)
            .AddWithValue("@parametre_id", autoSuivi.ParametreId)
        End With
        Try
            da.InsertCommand = cmd
            autoSuiviId = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return autoSuiviId
    End Function

    Public Sub DeleteAutoSuivi(autoSuivi As AutoSuivi)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim dateCreation As Date = Date.Now.Date
        Dim SQLstring As String = "DELETE oasis.oa_r_autosuivi" &
        " WHERE patient_id = @patient_id AND parametre_id = @parametre_id"
        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@patient_id", autoSuivi.PatientId)
            .AddWithValue("@parametre_id", autoSuivi.ParametreId)
        End With
        Try
            da.DeleteCommand = cmd
            da.DeleteCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub

End Class
