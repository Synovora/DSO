'Détail d'un médicament

Imports System.Data.SqlClient
Public Class FMedocDetail
    Private privateMedicamentCis As Integer

    Public Property MedicamentCis As Integer
        Get
            Return privateMedicamentCis
        End Get
        Set(value As Integer)
            privateMedicamentCis = value
        End Set
    End Property

    Private Sub FMedocDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim conxn As New SqlConnection(getConnectionString())

        'vShown += 1

        Dim SQLString As String
        Dim medicamentDataReader As SqlDataReader
        SQLString = "select * from oasis.oa_r_medicament where oa_medicament_cis = " & MedicamentCis & ";"
        Dim myCommand As New SqlCommand(SQLString, conxn)

        conxn.Open()
        medicamentDataReader = myCommand.ExecuteReader()
        If medicamentDataReader.Read() Then
            LblMedicamentCIS.Text = MedicamentCis

            If medicamentDataReader("oa_medicament_dci") Is DBNull.Value Then
                LblMedicamentDCI.Text = ""
            Else
                LblMedicamentDCI.Text = medicamentDataReader("oa_medicament_dci")
            End If

            If medicamentDataReader("oa_medicament_forme") Is DBNull.Value Then
                LblMedicamentForme.Text = ""
            Else
                LblMedicamentForme.Text = medicamentDataReader("oa_medicament_forme")
            End If

            If medicamentDataReader("oa_medicament_voie_administration") Is DBNull.Value Then
                LblMedicamentAdministration.Text = ""
            Else
                LblMedicamentAdministration.Text = medicamentDataReader("oa_medicament_voie_administration")
            End If

            If medicamentDataReader("oa_medicament_titulaire") Is DBNull.Value Then
                LblMedicamentTitulaire.Text = ""
            Else
                LblMedicamentTitulaire.Text = medicamentDataReader("oa_medicament_titulaire")
            End If
        End If
        conxn.Close()
        myCommand.Dispose()

        '==============================================
        '=========== Médicament présentation===========
        '==============================================

        'Déclaration des données de connexion
        Dim medicamentPresentationDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim medicamentPresentationDataTable As DataTable = New DataTable()

        SQLString = "select oa_r_medicament_presentation_cip7, oa_r_medicament_presentation_presentation from oasis.oa_r_medicament_Presentation where oa_r_medicament_presentation_cis = " + MedicamentCis.ToString + ";"

        'Lecture des données en base
        conxn.Open()
        medicamentPresentationDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        medicamentPresentationDataAdapter.Fill(medicamentPresentationDataTable)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = medicamentPresentationDataTable.Rows.Count - 1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            'Ajout d'une ligne au DataGridView
            medicamentPresentationDataGridView.Rows.Insert(i)
            'Alimentation du DataGridView
            medicamentPresentationDataGridView.Item(0, i).Value = medicamentPresentationDataTable.Rows(i)("oa_r_medicament_presentation_cip7")
            medicamentPresentationDataGridView.Item(1, i).Value = medicamentPresentationDataTable.Rows(i)("oa_r_medicament_presentation_presentation")
        Next
        'Enlève le focus sur la première ligne de la Grid
        medicamentPresentationDataGridView.ClearSelection()
        conxn.Close()
        medicamentPresentationDataAdapter.Dispose()

        '==============================================
        '=========== Médicament composition ===========
        '==============================================

        'Déclaration des données de connexion
        Dim medicamentCompoDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim medicamentCompoDataTable As DataTable = New DataTable()

        SQLString = "select oa_r_medicament_compo_designation, oa_r_medicament_compo_denomination, oa_r_medicament_compo_dosage, oa_r_medicament_compo_reference_dosage, oa_r_medicament_compo_nature from oasis.oa_r_medicament_compo where oa_r_medicament_compo_cis = " + MedicamentCis.ToString + ";"

        'Lecture des données en base
        conxn.Open()
        medicamentCompoDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        medicamentCompoDataAdapter.Fill(medicamentCompoDataTable)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        rowCount = medicamentCompoDataTable.Rows.Count - 1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            'Ajout d'une ligne au DataGridView
            medicamentCompoDataGridView.Rows.Insert(i)
            'Alimentation du DataGridView
            medicamentCompoDataGridView("Designation", i).Value = medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_designation")
            medicamentCompoDataGridView("Denomination", i).Value = medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_denomination")
            medicamentCompoDataGridView("Dosage", i).Value = medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_dosage")
            medicamentCompoDataGridView("ReferenceDosage", i).Value = medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_reference_dosage")

            'medicamentCompoDataGridView.Item(0, i).Value = medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_designation")
            'medicamentCompoDataGridView.Item(1, i).Value = medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_denomination")
            'medicamentCompoDataGridView.Item(2, i).Value = medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_dosage")
            'medicamentCompoDataGridView.Item(3, i).Value = medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_reference_dosage")
            Select Case medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_nature")
                Case "SA"
                    'medicamentCompoDataGridView.Item(4, i).Value = "Substance active"
                    medicamentCompoDataGridView("Nature", i).Value = "Substance active"
                Case "FT"
                    'medicamentCompoDataGridView.Item(4, i).Value = "Fraction thérapeutique"
                    medicamentCompoDataGridView("Nature", i).Value = "Fraction thérapeutique"
                Case Else
                    'medicamentCompoDataGridView.Item(4, i).Value = ""
                    medicamentCompoDataGridView("Nature", i).Value = ""
            End Select
        Next
        'Enlève le focus sur la première ligne de la Grid
        medicamentCompoDataGridView.ClearSelection()
        conxn.Close()
        medicamentCompoDataAdapter.Dispose()
    End Sub

    Private Sub FMedocDetail_Activated(sender As Object, e As EventArgs) Handles MyBase.Activated
        'If vShown > 0 Then
        'medicamentPresentationDataGridView.Rows.Clear()
        'medicamentCompoDataGridView.Rows.Clear()
        'FMedocDetail_Load(sender, e)
        'End If
    End Sub
End Class