Imports System.Data.SqlClient

Public Class G15_ProfessionSanteDao
    Inherits StandardDao

    Private Function BuildBean(reader As SqlDataReader) As G15_ProfessionSante
        Dim ProfessionSante As New G15_ProfessionSante With {
            .Oid = reader("oid"),
            .Code = Coalesce(reader("code"), 0),
            .Libelle = Coalesce(reader("libelle"), "")
        }
        Return ProfessionSante
    End Function

    Public Function GetAnnuaireProfessionnelById(annuaireProfessionneld As Integer) As G15_ProfessionSante
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
