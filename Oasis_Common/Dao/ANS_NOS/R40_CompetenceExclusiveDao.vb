Imports System.Data.SqlClient

Public Class NosCompetenceExclusiveDao
    Inherits StandardDao

    ''' <summary>
    ''' Lit une ligne telle quelle, sans toucher à la base. IDataRecord plutôt que
    ''' SqlDataReader pour qu'un test puisse fournir la ligne.
    ''' </summary>
    Public Shared Function BuildBean(record As System.Data.IDataRecord) As R40_CompetenceExclusive
        Dim CompetenceExclusive As New R40_CompetenceExclusive With {
            .Oid = record("oid"),
            .Code = Coalesce(record("code"), ""),
            .Libelle = Coalesce(record("libelle"), "")
        }
        Return CompetenceExclusive
    End Function

    Public Function GetCompetenceExclusiveById(codeld As String) As R40_CompetenceExclusive
        Dim CompetenceExclusive As R40_CompetenceExclusive
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.ans_nos_r40_competence_exclusive WHERE code = @id"
            command.Parameters.AddWithValue("@id", codeld)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    CompetenceExclusive = BuildBean(reader)
                Else
                    Throw New ArgumentException("Compétence exclusive inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return CompetenceExclusive
    End Function
End Class
