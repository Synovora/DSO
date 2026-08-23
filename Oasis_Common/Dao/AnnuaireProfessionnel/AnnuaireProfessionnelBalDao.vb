Imports System.Data.SqlClient

Public Class AnnuaireProfessionnelBalDao

    Inherits StandardDao

    Public Structure EnumTypeBal
        Const PERSONNELLE = "PER"
        Const ORGANISATION = "ORG"
    End Structure

    Public Function GetBalByTypeBalAndIdentifiant(TypeBal As String, IdentifiantNational As String) As DataTable
        Dim SQLString As String = "SELECT adresse_bal, raison_sociale_structure From oasis.ans_annuaire_professionnel_sante_bal"

        ' Valeurs issues de l'annuaire importé : paramétrées pour éviter une
        ' injection de second ordre via les données reprises.
        Dim ClauseWhere As String = " WHERE type_bal = @typeBal AND identifiant_national_pp = @identifiantNational"

        Dim ClauseOrderBy As String = " ORDER BY adresse_bal ASC;"

        SQLString += ClauseWhere
        SQLString += ClauseOrderBy

        Using con As SqlConnection = GetConnection()
            Dim TraitementDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using TraitementDataAdapter
                TraitementDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                With TraitementDataAdapter.SelectCommand.Parameters
                    .AddWithValue("@typeBal", If(TypeBal, ""))
                    .AddWithValue("@identifiantNational", If(IdentifiantNational, "").Trim())
                End With
                Dim TraitementDataTable As DataTable = New DataTable()
                Using TraitementDataTable
                    Try
                        TraitementDataAdapter.Fill(TraitementDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return TraitementDataTable
                End Using
            End Using
        End Using
    End Function
End Class
