Imports System.Data.SqlClient
Imports Oasis_Common
Public Class TheriaqueDao
    Inherits StandardDao

    Friend Function GetMedicamentById(medicamentCis As Integer) As Medicament
        Dim medicament As Medicament
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_r_medicament WHERE oa_medicament_cis = @Id"
            command.Parameters.AddWithValue("@id", medicamentCis)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    medicament = BuildBean(reader)
                Else
                    Throw New ArgumentException("Médicament inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return medicament
    End Function

    Private Function BuildBean(reader As SqlDataReader) As Medicament
        Dim medicament As New Medicament

        medicament.MedicamentCis = reader("oa_medicament_cis")
        medicament.MedicamentDci = Coalesce(reader("oa_medicament_dci"), "")
        medicament.Forme = Coalesce(reader("oa_medicament_forme"), "")
        medicament.Titulaire = Coalesce(reader("oa_medicament_titulaire"), "")
        medicament.VoieAdministration = Coalesce(reader("oa_medicament_voie_administration"), "")

        Return medicament
    End Function

    Public Function GetAllATC() As DataTable
        Dim SQLString As String
        SQLString = "SELECT CATC_CODE_PK, CATC_NOMF FROM [Theriak].[theriaque].[CATC_CLASSEATC]" &
                    " WHERE (CATC_CATC_CODE_FK is Null)" &
                    " ORDER BY CATC_CODE_PK"

        Dim dt As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Using da
                da.SelectCommand = New SqlCommand(SQLString, con)
                Try
                    da.Fill(dt)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally
                    con.Close()
                End Try
            End Using
        End Using

        Return dt
    End Function

    Friend Function getATCByATC(CodeATC As String) As DataTable
        Dim dt As New DataTable
        Dim ds As New DataSet

        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("@CodeATC", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.CommandText = "theriaque.GET_THE_ATC_ATC"
                command.Parameters.AddWithValue("@codeId", CodeATC)

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return dt
    End Function

    Friend Function getSpecialiteByATC(CodeATC As String) As DataTable
        Dim dt As New DataTable
        Dim ds As New DataSet

        Using con As SqlConnection = GetConnection()
            Try
                Dim command As New SqlCommand("@CodeATC", con)
                command.CommandType = CommandType.StoredProcedure
                command.Connection.ChangeDatabase("Theriak")
                command.CommandText = "theriaque.GET_THE_SPECIALITE"
                command.Parameters.AddWithValue("@codeId", CodeATC & "%")
                command.Parameters.AddWithValue("@VarTyp", 10)
                command.Parameters.AddWithValue("@MonoVir", 0)

                Dim da As New SqlDataAdapter(command)
                da.Fill(dt)
            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return dt
    End Function

End Class
