Imports System.Data.SqlClient
Imports Oasis_Common
Public Class ParametreDao
    Inherits StandardDao

    Public Enum enumParametreId
        POIDS = 1
        TAILLE = 2
        IMC = 3
        PAM = 8
        PAS = 6
        PAD = 7
    End Enum

    Private Function BuildBean(reader As SqlDataReader) As Parametre
        Dim parametre As New Parametre

        parametre.Id = reader("id")
        parametre.Description = Coalesce(reader("description"), "")
        parametre.DescriptionPatient = Coalesce(reader("description_patient"), "")
        parametre.Entier = Coalesce(reader("entier"), 0)
        parametre.Decimal = Coalesce(reader("decimal"), 0)
        parametre.Unite = Coalesce(reader("unite"), "")
        parametre.ValeurMin = Coalesce(reader("valeur_min"), 0)
        parametre.ValeurMax = Coalesce(reader("valeur_max"), 0)
        parametre.Ordre = Coalesce(reader("ordre"), 0)
        parametre.Inactif = Coalesce(reader("inactif"), False)

        Return parametre
    End Function

    Friend Function GetParametreById(parametreId As Integer) As Parametre
        Dim parametre As Parametre
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "select * from oasis.oa_r_parametre where id = @id"
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
        Dim SQLString As String
        SQLString = "SELECT * FROM oasis.oasis.oa_r_parametre" &
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
