Imports System.Data.SqlClient
Imports Oasis_Common
Public Class DrcSynonymeDao
    Inherits StandardDao

    Friend Function getAllSynonymebyDrc(DrcId As Integer) As DataTable
        Dim SQLString As String = "SELECT oa_drc_synonyme_id, oa_drc_synonyme_libelle FROM oasis.oa_drc_synonyme" &
        " WHERE oa_drc_id = " + DrcId.ToString + " order by oa_drc_synonyme_id;"

        Using con As SqlConnection = GetConnection()
            Dim DrcSynonymeDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using DrcSynonymeDataAdapter
                DrcSynonymeDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim DrcSynonymeDataTable As DataTable = New DataTable()
                Using DrcSynonymeDataTable
                    Try
                        DrcSynonymeDataAdapter.Fill(DrcSynonymeDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return DrcSynonymeDataTable
                End Using
            End Using
        End Using
    End Function

End Class
