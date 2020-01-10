Imports System.Collections.Specialized
Imports System.Data.SqlClient
Module MedicamentDao

    Friend Function GetCountMedicament(Filtre As String) As Integer
        Dim SqlString As String = "SELECT COUNT(*) FROM oasis.v_medoc WHERE "
        Dim RowCount As Integer = 0

        If Filtre <> "" Then
            SqlString += Filtre
        End If

        Dim conxn As New SqlConnection(getConnectionString())
        Dim cmd As New SqlCommand(SqlString, conxn)
        Try
            conxn.Open()
            RowCount = cmd.ExecuteScalar()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            conxn.Close()
            cmd.Dispose()
        End Try

        Return RowCount
    End Function



    'Lecture des médicaments déclarés allergiques
    Public Sub TraitementAllergies(Patient As Patient)
        'Intialisation de la StringCollection des médicaments cis génériques associés aux allergiques
        Patient.PatientAllergiesGénériquesCis.Clear()

        Dim MedicamentCis As Integer
        Dim allergieEnumerator As StringEnumerator = Patient.PatientAllergieCis.GetEnumerator()
        While allergieEnumerator.MoveNext()
            'Pour chaque médicament déclaré allergique, récupération des groupes génériques
            MedicamentCis = CInt(allergieEnumerator.Current)
            ChargementGenerique(MedicamentCis, Patient)
        End While
    End Sub

    Private Sub ChargementGenerique(MedicamentCis As Integer, patient As Patient)
        'Déclaration des données de connexion
        Dim conxn As New SqlConnection(getConnectionString())
        Dim MedicamentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim MedicamentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select oa_medicament_gener_id from oasis.oa_medicament_gener" &
        " where oa_medicament_gener_cis = " + MedicamentCis.ToString + ";"

        Try
            'Lecture des données en base
            MedicamentDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
            MedicamentDataAdapter.Fill(MedicamentDataTable)
            conxn.Open()

            'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
            Dim i As Integer
            Dim MedicamentGeneriqueId As Integer
            Dim rowCount As Integer = MedicamentDataTable.Rows.Count - 1
            'Parcours du DataTable pour alimenter le DataGridView
            For i = 0 To rowCount Step 1
                MedicamentGeneriqueId = MedicamentDataTable.Rows(i)("oa_medicament_gener_id")
                ChargementMedicamentCisAllergieCollectionPatient(MedicamentGeneriqueId, patient)
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            conxn.Close()
            MedicamentDataAdapter.Dispose()
        End Try

    End Sub

    Private Sub ChargementMedicamentCisAllergieCollectionPatient(MedicamentGeneriqueId As Integer, patient As Patient)
        'Déclaration des données de connexion
        Dim conxn As New SqlConnection(getConnectionString())
        Dim MedicamentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim MedicamentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select oa_medicament_gener_cis from oasis.oa_medicament_gener" &
        " where oa_medicament_gener_id = " + MedicamentGeneriqueId.ToString + ";"
        Try
            'Lecture des données en base
            MedicamentDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
            MedicamentDataAdapter.Fill(MedicamentDataTable)
            conxn.Open()

            'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
            Dim i As Integer
            Dim rowCount As Integer = MedicamentDataTable.Rows.Count - 1
            'Parcours du DataTable pour alimenter le DataGridView
            For i = 0 To rowCount Step 1
                'Alimentation de la StringCollection
                patient.PatientAllergiesGénériquesCis.Add(MedicamentDataTable.Rows(i)("oa_medicament_gener_cis"))
            Next
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            conxn.Close()
            MedicamentDataAdapter.Dispose()
        End Try
    End Sub
End Module
