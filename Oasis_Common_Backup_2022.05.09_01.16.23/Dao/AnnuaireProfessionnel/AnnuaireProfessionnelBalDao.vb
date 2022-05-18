Imports System.Data.SqlClient

Public Class AnnuaireProfessionnelBalDao

    Inherits StandardDao

    Public Structure EnumTypeBal
        Const PERSONNELLE = "PER"
        Const ORGANISATION = "ORG"
    End Structure

    Public Function GetBalByTypeBalAndIdentifiant(TypeBal As String, IdentifiantNational As String) As DataTable
        Dim SQLString As String = "SELECT adresse_bal, raison_sociale_structure From oasis.ans_annuaire_professionnel_sante_bal"

        Dim ClauseWhere As String = " WHERE type_bal = '" & TypeBal & "' AND identifiant_national_pp = '" & IdentifiantNational.Trim & "'"

        Dim ClauseOrderBy As String = " ORDER BY adresse_bal ASC;"

        SQLString += ClauseWhere
        SQLString += ClauseOrderBy

        Using con As SqlConnection = GetConnection()
            Dim TraitementDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using TraitementDataAdapter
                TraitementDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim TraitementDataTable As DataTable = New DataTable()
                Using TraitementDataTable
                    Try
                        TraitementDataAdapter.Fill(TraitementDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return TraitementDataTable
                End Using
            End Using
        End Using
    End Function
End Class
