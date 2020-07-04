Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports Oasis_Common

Module MedocDao

    Public Function ListeSubstancesAllergiques(PatientMedocAllergiqueCis As StringCollection) As StringCollection
        Dim SubstancesAllergiques As New StringCollection()


        Dim conxn As New SqlConnection(getConnectionString())
        Dim MedocDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim MedocDataTable As DataTable = New DataTable()
        Dim SQLString As String

        Dim MedocAllergiqueCis As Integer
        Dim allergieCisEnumerator As StringEnumerator = PatientMedocAllergiqueCis.GetEnumerator()
        While allergieCisEnumerator.MoveNext()
            MedocAllergiqueCis = CInt(allergieCisEnumerator.Current)
            SQLString = "select * from oasis.oa_r_medicament_compo where oa_r_medicament_compo_cis = " + MedocAllergiqueCis.ToString + " order by oa_r_medicament_compo_nature asc;"
            Try
                'Lecture des données en base
                MedocDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
                MedocDataAdapter.Fill(MedocDataTable)
                'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
                Dim FractionTherapeutiqueOK As Boolean = False
                Dim i, iList As Integer
                Dim rowCount As Integer = MedocDataTable.Rows.Count - 1
                iList = 0
                'Parcours du DataTable pour alimenter le DataGridView
                For i = 0 To rowCount Step 1
                    If i = 0 Then
                        If MedocDataTable.Rows(i)("oa_r_medicament_compo_nature") = "FT" Then
                            FractionTherapeutiqueOK = True
                        End If
                    End If
                    If FractionTherapeutiqueOK = True Then
                        'On récupère les fractions thérapeutiques
                        If MedocDataTable.Rows(i)("oa_r_medicament_compo_nature") = "FT" Then
                            If SubstancesAllergiques.Contains(MedocDataTable.Rows(i)("oa_r_medicament_compo_denomination")) Then
                            Else
                                SubstancesAllergiques.Add(MedocDataTable.Rows(i)("oa_r_medicament_compo_denomination"))
                            End If
                        End If
                    Else
                        'Sinon, on récupère les substances actives (SA)
                        If SubstancesAllergiques.Contains(MedocDataTable.Rows(i)("oa_r_medicament_compo_denomination")) Then
                        Else
                            SubstancesAllergiques.Add(MedocDataTable.Rows(i)("oa_r_medicament_compo_denomination"))
                        End If
                    End If
                Next
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                conxn.Close()
                MedocDataAdapter.Dispose()
            End Try
            MedocDataTable.Clear()
        End While

        MedocDataTable.Dispose()

        Return SubstancesAllergiques
    End Function


    Public Function ListeSubstancesCI(PatientMedocCICis As StringCollection) As StringCollection
        Dim SubstancesCI As New StringCollection()


        Dim conxn As New SqlConnection(getConnectionString())
        Dim MedocDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim MedocDataTable As DataTable = New DataTable()
        Dim SQLString As String

        Dim MedocCICis As Integer
        Dim allergieCisEnumerator As StringEnumerator = PatientMedocCICis.GetEnumerator()
        While allergieCisEnumerator.MoveNext()
            MedocCICis = CInt(allergieCisEnumerator.Current)
            SQLString = "select * from oasis.oa_r_medicament_compo where oa_r_medicament_compo_cis = " + MedocCICis.ToString + " order by oa_r_medicament_compo_nature asc;"

            Try
                'Lecture des données en base
                MedocDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
                MedocDataAdapter.Fill(MedocDataTable)

                'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
                Dim FractionTherapeutiqueOK As Boolean = False
                Dim i As Integer
                Dim rowCount As Integer = MedocDataTable.Rows.Count - 1

                'Parcours du DataTable pour alimenter le DataGridView
                For i = 0 To rowCount Step 1
                    If i = 0 Then
                        If MedocDataTable.Rows(i)("oa_r_medicament_compo_nature") = "FT" Then
                            FractionTherapeutiqueOK = True
                        End If
                    End If
                    If FractionTherapeutiqueOK = True Then
                        'On récupère les fractions thérapeutiques
                        If MedocDataTable.Rows(i)("oa_r_medicament_compo_nature") = "FT" Then
                            If SubstancesCI.Contains(MedocDataTable.Rows(i)("oa_r_medicament_compo_denomination")) Then
                            Else
                                SubstancesCI.Add(MedocDataTable.Rows(i)("oa_r_medicament_compo_denomination"))
                            End If
                        End If
                    Else
                        'Sinon, on récupère les substances actives (SA)
                        If SubstancesCI.Contains(MedocDataTable.Rows(i)("oa_r_medicament_compo_denomination")) Then
                        Else
                            SubstancesCI.Add(MedocDataTable.Rows(i)("oa_r_medicament_compo_denomination"))
                        End If
                    End If
                Next
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                conxn.Close()
                MedocDataAdapter.Dispose()
            End Try
            MedocDataTable.Clear()
            MedocDataTable.Dispose()
        End While

        Return SubstancesCI
    End Function
End Module
