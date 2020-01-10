Imports System.Data.SqlClient

Public Class RadFMedocDetail
    Private privateMedicamentCis As Integer

    Public Property MedicamentCis As Integer
        Get
            Return privateMedicamentCis
        End Get
        Set(value As Integer)
            privateMedicamentCis = value
        End Set
    End Property

    Private Sub RadFMedocDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
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

        SQLString = "select oa_r_medicament_presentation_cip7, oa_r_medicament_presentation_presentation from oasis.oa_r_medicament_Presentation" &
        " where oa_r_medicament_presentation_cis = " + MedicamentCis.ToString + ";"

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
            RadmedicamentPresentationDataGridView.Rows.Add(i)
            'Alimentation du DataGridView
            RadmedicamentPresentationDataGridView.Rows(i).Cells("CIP7").Value = medicamentPresentationDataTable.Rows(i)("oa_r_medicament_presentation_cip7")
            RadmedicamentPresentationDataGridView.Rows(i).Cells("presentation").Value = medicamentPresentationDataTable.Rows(i)("oa_r_medicament_presentation_presentation")
        Next

        'Positionnement du grid sur la première occurrence
        If RadmedicamentPresentationDataGridView.Rows.Count > 0 Then
            Me.RadmedicamentPresentationDataGridView.CurrentRow = RadmedicamentPresentationDataGridView.ChildRows(0)
        End If

        conxn.Close()
        medicamentPresentationDataAdapter.Dispose()

        '==============================================
        '=========== Médicament composition ===========
        '==============================================

        'Déclaration des données de connexion
        Dim medicamentCompoDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim medicamentCompoDataTable As DataTable = New DataTable()

        SQLString = "select oa_r_medicament_compo_designation, oa_r_medicament_compo_denomination, oa_r_medicament_compo_dosage," &
        " oa_r_medicament_compo_reference_dosage, oa_r_medicament_compo_nature from oasis.oa_r_medicament_compo" &
        " where oa_r_medicament_compo_cis = " + MedicamentCis.ToString + ";"

        'Lecture des données en base
        conxn.Open()
        medicamentCompoDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        medicamentCompoDataAdapter.Fill(medicamentCompoDataTable)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        rowCount = medicamentCompoDataTable.Rows.Count - 1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            'Ajout d'une ligne au DataGridView
            RadmedicamentCompoDataGridView.Rows.Add(i)
            'Alimentation du DataGridView
            RadmedicamentCompoDataGridView.Rows(i).Cells("denomination").Value = medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_denomination")
            RadmedicamentCompoDataGridView.Rows(i).Cells("dosage").Value = medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_dosage")
            RadmedicamentCompoDataGridView.Rows(i).Cells("referenceDosage").Value = medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_reference_dosage")

            'medicamentCompoDataGridView.Item(0, i).Value = medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_designation")
            'medicamentCompoDataGridView.Item(1, i).Value = medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_denomination")
            'medicamentCompoDataGridView.Item(2, i).Value = medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_dosage")
            'medicamentCompoDataGridView.Item(3, i).Value = medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_reference_dosage")
            Select Case medicamentCompoDataTable.Rows(i)("oa_r_medicament_compo_nature")
                Case "SA"
                    'medicamentCompoDataGridView.Item(4, i).Value = "Substance active"
                    RadmedicamentCompoDataGridView.Rows(i).Cells("nature").Value = "Substance active"
                Case "FT"
                    'medicamentCompoDataGridView.Item(4, i).Value = "Fraction thérapeutique"
                    RadmedicamentCompoDataGridView.Rows(i).Cells("nature").Value = "Fraction thérapeutique"
                Case Else
                    'medicamentCompoDataGridView.Item(4, i).Value = ""
                    RadmedicamentCompoDataGridView.Rows(i).Cells("nature").Value = ""
            End Select
        Next

        'Positionnement du grid sur la première occurrence
        If RadmedicamentCompoDataGridView.Rows.Count > 0 Then
            Me.RadmedicamentCompoDataGridView.CurrentRow = RadmedicamentCompoDataGridView.ChildRows(0)
        End If

        conxn.Close()
        medicamentCompoDataAdapter.Dispose()
    End Sub


    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Close()
    End Sub
End Class
