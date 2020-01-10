﻿Imports System.Data.SqlClient

Public Class AldDao
    Inherits StandardDao

    Friend Function getAldById(AldId As Integer) As Ald
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
        'Déclaration des données de connexion
        Dim conxn As New SqlConnection(getConnectionString())
        Dim AldPatientDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim AldPatientDataTable As DataTable = New DataTable()

        Dim StringRetour As String = ""

        Dim PremierPassage As Boolean = True

        Dim SQLString As String = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' And oa_antecedent_statut_affichage = 'P'" &
        " And (oa_antecedent_inactif = '0' Or oa_antecedent_inactif is Null) And oa_antecedent_ald_valide = 1" &
        " And oa_antecedent_patient_id = " + patientId.ToString + ";"

        'Lecture des données en base
        AldPatientDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        AldPatientDataAdapter.Fill(AldPatientDataTable)
        conxn.Open()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim rowCount As Integer = AldPatientDataTable.Rows.Count - 1

        For i = 0 To rowCount Step 1
            Dim DateFin As Date = AldPatientDataTable.Rows(i)("oa_antecedent_ald_date_fin")
            Dim DateControle As Date = DateFin.AddMonths(1)
            If DateControle > Date.Now Then
                If PremierPassage = True Then
                    PremierPassage = False
                    StringRetour = "Expiration ALD : " + vbCrLf + "   " + DateFin.ToString("dd-MM-yyyy")
                Else
                    StringRetour = StringRetour + vbCrLf + "   " + DateFin.ToString("dd-MM-yyyy")
                End If
            End If
        Next

        Return StringRetour
    End Function

End Class
