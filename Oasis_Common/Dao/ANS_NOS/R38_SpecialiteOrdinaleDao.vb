Imports System.Data.SqlClient

Public Class NosSpecialiteOrdinaleDao
    Inherits StandardDao

    Private Function BuildBean(reader As SqlDataReader) As R38_SpecialiteOrdinale
        Dim SpecialiteOrdinale As New R38_SpecialiteOrdinale With {
            .Oid = reader("oid"),
            .Code = Coalesce(reader("code"), ""),
            .Libelle = Coalesce(reader("libelle"), "")
        }
        Return SpecialiteOrdinale
    End Function

    Public Function GetSpecialiteOrdinaleById(codeId As String) As R38_SpecialiteOrdinale
        Dim SpecialiteOrdinale As R38_SpecialiteOrdinale
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.ans_nos_r38_specialite_ordinale WHERE code = @id"
            command.Parameters.AddWithValue("@id", codeId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    SpecialiteOrdinale = BuildBean(reader)
                Else
                    Throw New ArgumentException("Spécialité ordinale inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return SpecialiteOrdinale
    End Function

End Class
