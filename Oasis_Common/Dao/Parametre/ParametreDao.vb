Imports System.Data.SqlClient

Public Class ParametreDao

    Inherits StandardDao

    ''' <summary>
    ''' Lit une ligne telle quelle, sans toucher à la base. IDataRecord plutôt que
    ''' SqlDataReader pour qu'un test puisse fournir la ligne.
    ''' </summary>
    Public Shared Function BuildBean(record As System.Data.IDataRecord) As Parametre
        Dim parametre As New Parametre With {
            .Id = record("id"),
            .Description = Coalesce(record("description"), ""),
            .DescriptionPatient = Coalesce(record("description_patient"), ""),
            .Entier = Coalesce(record("entier"), 0),
            .Decimal = Coalesce(record("decimal"), 0),
            .Unite = Coalesce(record("unite"), ""),
            .ValeurMin = Coalesce(record("valeur_min"), 0),
            .ValeurMax = Coalesce(record("valeur_max"), 0),
            .Ordre = Coalesce(record("ordre"), 0),
            .Inactif = Coalesce(record("inactif"), False),
            .ExclusionAutoSuivi = Coalesce(record("exclusion_auto_suivi"), False),
            .AideAssociee = Coalesce(record("aide_associee"), ""),
            .Wiki = Coalesce(record("wiki"), "")
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

    'TODO: change it
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
