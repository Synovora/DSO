Imports System.Data.SqlClient
Imports Oasis_Common
Public Class PPSHistoDao
    Inherits StandardDao

    Public Function GetAllPPSHistobyPPSId(ppsId As Integer) As DataTable
        Dim SQLString As String = "SELECT oa_pps_histo_id, oa_pps_histo_date_historisation, oa_pps_histo_utilisateur_historisation, oa_pps_histo_etat_historisation," &
            " oa_pps_id, oa_pps_priorite, oa_pps_drc_id, oa_pps_affichage_synthese, oa_pps_commentaire, oa_pps_date_debut, oa_pps_arret," &
            " oa_pps_commentaire_arret, oa_pps_inactif, oa_drc_libelle FROM oasis.oa_patient_pps_histo" &
            " LEFT JOIN oasis.oa_drc ON oa_drc_id = oa_pps_drc_id" &
            " WHERE oa_pps_id = '" + ppsId.ToString + "' ORDER BY oa_pps_histo_id desc;"

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
