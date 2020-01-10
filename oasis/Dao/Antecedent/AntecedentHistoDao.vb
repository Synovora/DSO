Imports System.Data.SqlClient

Public Class AntecedentHistoDao
    Inherits StandardDao


    Public Function getAllAntecedentHistobyAntecedentId(antecedentId As Integer) As DataTable
        Dim SQLString As String

        SQLString = "SELECT * FROM oasis.oa_antecedent_histo" &
                    " WHERE oa_antecedent_id = '" & antecedentId.ToString &
                    "' ORDER BY oa_antecedent_histo_id DESC;"

        Using con As SqlConnection = GetConnection()
            Dim AntecedentHistoDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using AntecedentHistoDataAdapter
                AntecedentHistoDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim AntecedentHistoDataTable As DataTable = New DataTable()
                Using AntecedentHistoDataTable
                    Try
                        AntecedentHistoDataAdapter.Fill(AntecedentHistoDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return AntecedentHistoDataTable
                End Using
            End Using
        End Using
    End Function
End Class
