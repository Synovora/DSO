Imports System.Data.SqlClient

Public Class NosProfessionSanteDao
    Inherits StandardDao

    ''' <summary>
    ''' Lit une ligne telle quelle, sans toucher à la base. IDataRecord plutôt que
    ''' SqlDataReader pour qu'un test puisse fournir la ligne.
    ''' </summary>
    Public Shared Function BuildBean(record As System.Data.IDataRecord) As G15_ProfessionSante
        Dim ProfessionSante As New G15_ProfessionSante With {
            .Oid = record("oid"),
            .Code = Coalesce(record("code"), 0),
            .Libelle = Coalesce(record("libelle"), "")
        }
        Return ProfessionSante
    End Function

    Public Function GetProfessionSanteById(annuaireProfessionneld As Integer) As G15_ProfessionSante
        Dim ProfessionSante As G15_ProfessionSante
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.ans_nos_g15_profession_sante WHERE code = @id"
            command.Parameters.AddWithValue("@id", annuaireProfessionneld)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    ProfessionSante = BuildBean(reader)
                Else
                    Throw New ArgumentException("Profession de santé inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return ProfessionSante
    End Function
End Class
