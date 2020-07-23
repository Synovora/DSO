Imports System.Data.SqlClient

Public Class ParametreDao
    Inherits StandardDao

    Private Function BuildBean(reader As SqlDataReader) As Parametre
        Dim parametre As New Parametre With {
            .Id = reader("id"),
            .Description = Coalesce(reader("description"), ""),
            .DescriptionPatient = Coalesce(reader("description_patient"), ""),
            .Entier = Coalesce(reader("entier"), 0),
            .Decimal = Coalesce(reader("decimal"), 0),
            .Unite = Coalesce(reader("unite"), ""),
            .ValeurMin = Coalesce(reader("valeur_min"), 0),
            .ValeurMax = Coalesce(reader("valeur_max"), 0),
            .Ordre = Coalesce(reader("ordre"), 0),
            .Inactif = Coalesce(reader("inactif"), False),
            .ExclusionAutoSuivi = Coalesce(reader("exclusion_auto_suivi"), False)
        }

        Return parametre
    End Function

    Public Function GetParametreById(parametreId As Integer) As Parametre
        Dim parametre As Parametre
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_r_parametre WHERE id = @id"
            command.Parameters.AddWithValue("@id", parametreId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    parametre = BuildBean(reader)
                Else
                    Throw New ArgumentException("Paramètre inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return parametre
    End Function

    Public Function GetAllParametre() As DataTable
        Dim SQLString As String = "SELECT * FROM oasis.oasis.oa_r_parametre" &
                " WHERE inactif is Null or inactif = 'False'" &
                " ORDER BY description"
        Dim ParametreDataTable As DataTable = New DataTable()
        Using con As SqlConnection = GetConnection()
            Dim ParametreDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ParametreDataAdapter
                ParametreDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Try
                    ParametreDataAdapter.Fill(ParametreDataTable)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally
                    con.Close()
                End Try
            End Using
        End Using
        Return ParametreDataTable
    End Function

End Class
