Imports System.Data.SqlClient

Public Class AldDao
    Inherits StandardDao

    Public Function getAldById(AldId As Integer) As Ald
        Dim ald As Ald
        Dim con As SqlConnection
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "select * from oasis.oa_ald where oa_ald_id = @id"
            command.Parameters.AddWithValue("@id", AldId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    ald = buildBean(reader)
                Else
                    Throw New ArgumentException("ALD inexistante !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return ald
    End Function

    Private Function buildBean(reader As SqlDataReader) As Ald
        Dim ald As New Ald
        ald.AldId = reader("oa_ald_id")
        ald.AldCode = Coalesce(reader("oa_ald_code"), "")
        ald.AldDescription = Coalesce(reader("oa_ald_description"), "")
        Return ald
    End Function

    Public Function getAllAld() As DataTable
        Dim SQLString As String

        SQLString = "SELECT oa_ald_id, oa_ald_code, oa_ald_description FROM oasis.oa_ald;"

        Using con As SqlConnection = GetConnection()
            Dim AldDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using AldDataAdapter
                AldDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim AldDataTable As DataTable = New DataTable()
                Using AldDataTable
                    Try
                        AldDataAdapter.Fill(AldDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return AldDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function DateFinALD(patientId As Integer) As String
        Dim SQLString As String
        Dim StringRetour As String = ""
        Dim PremierPassage As Boolean = True

        SQLString = "SELECT * FROM oasis.oa_antecedent" &
            " WHERE oa_antecedent_type = 'A'" &
            " AND oa_antecedent_statut_affichage = 'P'" &
            " AND (oa_antecedent_inactif = '0' OR oa_antecedent_inactif is Null)" &
            " AND oa_antecedent_ald_valide = 1" &
            " AND oa_antecedent_patient_id = " + patientId.ToString + ";"

        Using con As SqlConnection = GetConnection()
            Dim AldDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using AldDataAdapter
                AldDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim AldDataTable As DataTable = New DataTable()
                Using AldDataTable
                    Try
                        AldDataAdapter.Fill(AldDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try

                    'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
                    Dim rowCount As Integer = AldDataTable.Rows.Count - 1

                    For i = 0 To rowCount Step 1
                        Dim DateFin As Date = AldDataTable.Rows(i)("oa_antecedent_ald_date_fin")
                        Dim DateControle As Date = DateFin.AddMonths(1)
                        Dim DateMax As New Date(2999, 12, 31, 0, 0, 0)
                        If DateControle.Date > Date.Now.Date OrElse DateFin.Date = DateMax.Date Then
                            If PremierPassage = True Then
                                PremierPassage = False
                                StringRetour = "Expiration ALD : " + vbCrLf + "   " + DateFin.ToString("dd-MM-yyyy")
                            Else
                                StringRetour = StringRetour + vbCrLf + "   " + DateFin.ToString("dd-MM-yyyy")
                            End If
                        End If
                    Next

                    Return StringRetour
                End Using
            End Using
        End Using
    End Function

    Public Function IsPatientALD(patientId As Integer) As Boolean
        Dim CodeRetour As Boolean = False
        Dim SQLString As String
        Dim DateMax As New Date(2999, 12, 31, 0, 0, 0)
        Dim DateValide As Date = Date.Now().AddDays(-30)

        Dim PremierPassage As Boolean = True

        SQLString = "SELECT oa_antecedent_id, oa_antecedent_ald_date_fin" &
            " FROM oasis.oa_antecedent" &
            " WHERE oa_antecedent_type = 'A'" &
            " AND oa_antecedent_statut_affichage = 'P'" &
            " AND (oa_antecedent_inactif = '0' OR oa_antecedent_inactif is Null)" &
            " AND oa_antecedent_ald_valide = 1" &
            " AND (oa_antecedent_ald_date_fin = '" & DateMax.ToString("yyyy-MM-dd") & "' OR oa_antecedent_ald_date_fin >= '" & DateValide.ToString("yyyy-MM-dd") & "')" &
            " AND oa_antecedent_patient_id = " + patientId.ToString + ";"

        Using con As SqlConnection = GetConnection()
            Dim AldDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using AldDataAdapter
                AldDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim AldDataTable As DataTable = New DataTable()
                Using AldDataTable
                    Try
                        AldDataAdapter.Fill(AldDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try

                    If AldDataTable.Rows.Count > 0 Then
                        CodeRetour = True
                    End If

                    Return CodeRetour
                End Using
            End Using
        End Using
    End Function

End Class
