'Gestion des DRC/ORC

Imports System.Data.SqlClient
Imports Oasis_Common
Public Class FDRCListe
    Private privateUtilisateurConnecte As Utilisateur

    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property

    Public Enum EnumCategorieOasis
        Contexte = 1
        Strategie = 2
        Prevention = 3
        Objectif = 4
    End Enum

    Dim selectedDrcId As Integer
    Dim selectedDrcLibelle As String
    Dim categorieMajeureListe As Dictionary(Of Integer, String) = Table_categorie_majeure.GetCategorieMajeureListe()

    Dim drcSynonymeDataTable As DataTable = New DataTable()

    Private Sub FDRCListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Permet de gérer la touche entrée
        KeyPreview = True
        initZones()
        ChargementDRC()
    End Sub


    '==========================================================
    '======================= DRC ==============================
    '==========================================================

    'Chargement de la Grid
    Private Sub ChargementDRC()
        Dim DRCDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim DRCDataTable As DataTable = New DataTable()

        Dim conxn As New SqlConnection(outils.getConnectionString())

        Dim SQLString As String
        SQLString = getSQLString()

        Cursor = Cursors.WaitCursor

        DRCDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        DRCDataAdapter.Fill(DRCDataTable)

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = DRCDataTable.Rows.Count - 1
        Dim drcIdPrecedent, drcIdEnCours, oasisCategorie, categorieMajeureId As Integer
        Dim oasisCategorieDescription, categorieMajeureDescription As String
        Dim drcOasis As Boolean

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            'Ne pas traiter les doublons liées à la requête (JOIN LEFT)
            drcIdEnCours = CInt(DRCDataTable.Rows(i)("oa_drc_id"))
            If drcIdEnCours = drcIdPrecedent Then
                Continue For
            Else
                drcIdPrecedent = drcIdEnCours
            End If

            oasisCategorieDescription = "Non catégorisé"
            If DRCDataTable.Rows(i)("oa_drc_oasis_categorie") IsNot DBNull.Value Then
                oasisCategorie = DRCDataTable.Rows(i)("oa_drc_oasis_categorie")
                Select Case oasisCategorie
                    Case 1
                        oasisCategorieDescription = "Contexte/Antécédent"
                    Case 2
                        oasisCategorieDescription = "Stratégie"
                    Case 3
                        oasisCategorieDescription = "Prévention"
                    Case 4
                        oasisCategorieDescription = "Objectif"
                End Select
            End If

            categorieMajeureDescription = ""
            If DRCDataTable.Rows(i)("oa_drc_categorie_majeure_id") IsNot DBNull.Value Then
                categorieMajeureId = CInt(DRCDataTable.Rows(i)("oa_drc_categorie_majeure_id"))
                categorieMajeureDescription = Table_categorie_majeure.GetCategorieMajeureDescription(categorieMajeureId)
            End If

            drcOasis = False
            If DRCDataTable.Rows(i)("oa_drc_oasis") IsNot DBNull.Value Then
                If DRCDataTable.Rows(i)("oa_drc_oasis") = 1 Then
                    drcOasis = True
                End If
            End If

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            DRCDataGridView.Rows.Insert(iGrid)

            '------------------- Alimentation du DataGridView
            'Id DRC
            DRCDataGridView("drcId", iGrid).Value = DRCDataTable.Rows(i)("oa_drc_id")

            'DRC Description
            DRCDataGridView("drcDescription", iGrid).Value = DRCDataTable.Rows(i)("oa_drc_libelle")

            DRCDataGridView("categorieMajeure", iGrid).Value = categorieMajeureDescription

            DRCDataGridView("drcCategorie", iGrid).Value = oasisCategorieDescription

            If drcOasis = True Then
                DRCDataGridView("drcOasis", iGrid).Value = "Oasis"
                DRCDataGridView("drcOasis", iGrid).Style.ForeColor = Color.Red
            Else
                DRCDataGridView("drcOasis", iGrid).Value = ""
            End If
        Next

        conxn.Close()
        DRCDataAdapter.Dispose()

        'Enlève le focus sur la première ligne de la Grid
        DRCDataGridView.ClearSelection()

        Cursor = Cursors.Default
    End Sub

    Private Function getSQLString() As String
        Dim SQLString As String
        Dim clauseDescription As String
        Dim clauseCategorieMajeure As String
        Dim clauseCategorieOasis As String
        Dim clauseORC As String
        Dim categorieMajeureId As Integer
        Dim categorieOasis As Integer

        If TxtFiltreDescription.Text = "" Then
            clauseDescription = "1 = 1"
        Else
            clauseDescription = "(oa_drc_libelle LIKE '%" & TxtFiltreDescription.Text & "%' or oa_drc_synonyme_libelle like '%" & TxtFiltreDescription.Text & "%')"
        End If

        'Catégorie Majeure
        categorieMajeureId = 0
        For Each kvp As KeyValuePair(Of Integer, String) In categorieMajeureListe
            If kvp.Value = CbxFiltreCategorieMajeure.SelectedValue Then
                categorieMajeureId = kvp.Key
                Exit For
            End If
        Next kvp
        If categorieMajeureId = 0 Then
            clauseCategorieMajeure = "1 = 1"
        Else
            clauseCategorieMajeure = "oa_drc_categorie_majeure_id = " & categorieMajeureId & " "
        End If

        If CbxFiltreCategorieOasis.SelectedItem = "Tous" Then
            clauseCategorieOasis = "1 = 1"
        Else
            Select Case CbxFiltreCategorieOasis.SelectedItem
                Case "Antécédent et Contexte"
                    categorieOasis = 1
                Case "Stratégie"
                    categorieOasis = 2
                Case "Prévention"
                    categorieOasis = 3
                Case "Objectif"
                    categorieOasis = 4
            End Select
            clauseCategorieOasis = "oa_drc_oasis_categorie = " & categorieOasis & " "
        End If

        If ChkORC.Checked = True Then
            clauseORC = "oa_drc_oasis = 1"
        Else
            clauseORC = "1 = 1"
        End If

        SQLString = "select * from oasis.oa_drc left join oasis.oa_drc_synonyme on oasis.oasis.oa_drc.oa_drc_id = oasis.oa_drc_synonyme.oa_drc_id where (oa_drc_oasis_invalide is Null or oa_drc_oasis_invalide = 0) and " & clauseORC & " and " & clauseDescription & " AND " & clauseCategorieMajeure & " AND " & clauseCategorieOasis & " order by oasis.oa_drc.oa_drc_id;"

        getSQLString = SQLString
    End Function

    Private Sub ChargementDrcSynonyme(drcId As Integer)
        Dim drcSynonymeDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        'Dim drcSynonymeDataTable As DataTable = New DataTable()

        Dim conxn As New SqlConnection(outils.getConnectionString())

        selectedDrcId = drcId

        Dim SQLString As String
        'Exlusion des traitements : traitements déclarés 'annulé' 
        SQLString = getSQLStringDRCSynonyme(drcId)

        drcSynonymeDataTable.Clear()

        drcSynonymeDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        drcSynonymeDataAdapter.Fill(drcSynonymeDataTable)

        DrcDSynonymeDataGridView.DataSource = drcSynonymeDataTable

        conxn.Close()
        drcSynonymeDataAdapter.Dispose()

        'Enlève le focus sur la première ligne de la Grid
        DrcDSynonymeDataGridView.ClearSelection()
    End Sub


    Private Function getSQLStringDRCSynonyme(drcId As Integer) As String
        Dim SQLString As String
        SQLString = "SELECT oa_drc_synonyme_id, oa_drc_synonyme_libelle FROM oasis.oa_drc_synonyme WHERE oa_drc_id = " + drcId.ToString + " order by oa_drc_synonyme_id;"
        getSQLStringDRCSynonyme = SQLString
    End Function

    '===========================================================
    '======================= Généralités =======================
    '===========================================================

    'Initialisation de l'écran
    Private Sub initZones()
        'Catégorie Majeure
        Dim indice As Integer = categorieMajeureListe.Count - 1
        Dim categorieMajeureDescription(indice) As String
        Dim i As Integer = 0

        For Each kvp As KeyValuePair(Of Integer, String) In categorieMajeureListe
            categorieMajeureDescription(i) = kvp.Value
            i += 1
        Next kvp
        CbxFiltreCategorieMajeure.DataSource = categorieMajeureDescription
        'Catégorie Oasis
        CbxFiltreCategorieOasis.Items.Add("Antécédent et Contexte")
        CbxFiltreCategorieOasis.Items.Add("Stratégie")
        CbxFiltreCategorieOasis.Items.Add("Prévention")
        CbxFiltreCategorieOasis.Items.Add("Objectif")
        CbxFiltreCategorieOasis.Items.Add("Tous")
        CbxFiltreCategorieOasis.SelectedItem = "Tous"
        'Traitements
        DRCDataGridView.Rows.Clear()
    End Sub

    Private Sub DRCDataGridView_DoubleClick(sender As Object, e As EventArgs) Handles DRCDataGridView.DoubleClick
        If DRCDataGridView.CurrentRow IsNot Nothing Then
            Dim drcId As Integer = DRCDataGridView.Rows(DRCDataGridView.CurrentRow.Index).Cells("drcId").Value
            Dim vFDRCDetailEdit As New FDRCDetailEdit
            vFDRCDetailEdit.SelectedDRCId = drcId
            vFDRCDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
            vFDRCDetailEdit.ShowDialog() 'Modal
            If vFDRCDetailEdit.CodeRetour = True Then
                DRCDataGridView.Rows.Clear()
                ChargementDRC()
            End If
            vFDRCDetailEdit.Dispose()
        End If
    End Sub

    Private Sub BtnCreation_Click(sender As Object, e As EventArgs) Handles BtnCreationORC.Click
        Dim vFDRCDetailEdit As New FDRCDetailEdit
        vFDRCDetailEdit.SelectedDRCId = 0
        vFDRCDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
        vFDRCDetailEdit.ShowDialog() 'Modal
        If vFDRCDetailEdit.CodeRetour = True Then
            DRCDataGridView.Rows.Clear()
            ChargementDRC()
        End If
        vFDRCDetailEdit.Dispose()
    End Sub


    Private Sub DRCDataGridView_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DRCDataGridView.CellClick
        If DRCDataGridView.CurrentRow IsNot Nothing Then
            Dim drcId As Integer = DRCDataGridView.Rows(DRCDataGridView.CurrentRow.Index).Cells("drcId").Value
            Dim drcLibelle As String = DRCDataGridView.Rows(DRCDataGridView.CurrentRow.Index).Cells("drcDescription").Value
            selectedDrcId = drcId
            selectedDrcLibelle = drcLibelle
            ChargementDrcSynonyme(drcId)
        End If
    End Sub

    Private Sub BtnFiltrer_Click(sender As Object, e As EventArgs) Handles BtnFiltrer.Click
        DRCDataGridView.Rows.Clear()
        drcSynonymeDataTable.Clear()
        ChargementDRC()
    End Sub

    Private Sub BtnInitFiltre_Click(sender As Object, e As EventArgs) Handles BtnInitFiltre.Click
        TxtFiltreDescription.Text = ""
        'Catégorie Majeure
        For Each kvp As KeyValuePair(Of Integer, String) In categorieMajeureListe
            If kvp.Key = 0 Then
                CbxFiltreCategorieMajeure.SelectedItem = kvp.Value
                Exit For
            End If
        Next kvp
        'Catégorie Oasis
        CbxFiltreCategorieOasis.SelectedItem = "Tous"
        'ORC/Tous
        ChkORC.Checked = False
        'Rechargement Grid
        DRCDataGridView.Rows.Clear()
        drcSynonymeDataTable.Clear()
        ChargementDRC()
    End Sub

    Private Sub ModifierUneDRCORCToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneDRCORCToolStripMenuItem.Click
        If DRCDataGridView.CurrentRow IsNot Nothing Then
            Dim drcId As Integer = DRCDataGridView.Rows(DRCDataGridView.CurrentRow.Index).Cells("drcId").Value
            Dim vFDRCDetailEdit As New FDRCDetailEdit
            vFDRCDetailEdit.SelectedDRCId = drcId
            vFDRCDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
            vFDRCDetailEdit.ShowDialog() 'Modal
            If vFDRCDetailEdit.CodeRetour = True Then
                DRCDataGridView.Rows.Clear()
                ChargementDRC()
            End If
            vFDRCDetailEdit.Dispose()
        End If
    End Sub

    Private Sub ChkORC_CheckedChanged(sender As Object, e As EventArgs) Handles ChkORC.CheckedChanged
        DRCDataGridView.Rows.Clear()
        ChargementDRC()
    End Sub

    Private Sub DrcDefinitionDataGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles DrcDSynonymeDataGridView.CellDoubleClick
        If DrcDSynonymeDataGridView.CurrentRow IsNot Nothing Then
            Dim vFDRCSynonymeDetailEdit As New FDRCSynonymeDetailEdit
            vFDRCSynonymeDetailEdit.SelectedDrcId = selectedDrcId
            vFDRCSynonymeDetailEdit.SelectedDrcLibelle = selectedDrcLibelle
            Dim drcSynonymeId As Integer = DrcDSynonymeDataGridView.Rows(DrcDSynonymeDataGridView.CurrentRow.Index).Cells("oa_drc_synonyme_id").Value
            vFDRCSynonymeDetailEdit.SelectedDrcSynonymeId = drcSynonymeId
            vFDRCSynonymeDetailEdit.SelectedDrcSynonyme = DrcDSynonymeDataGridView.Rows(DrcDSynonymeDataGridView.CurrentRow.Index).Cells("oa_drc_synonyme_libelle").Value
            vFDRCSynonymeDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
            vFDRCSynonymeDetailEdit.ShowDialog() 'Modal
            If vFDRCSynonymeDetailEdit.CodeRetour = True Then
                ChargementDrcSynonyme(selectedDrcId)
            End If
            vFDRCSynonymeDetailEdit.Dispose()
        End If
    End Sub

    Private Sub BtnCreerSynonyme_Click(sender As Object, e As EventArgs) Handles BtnCreerSynonyme.Click
        If selectedDrcId <> 0 Then
            Dim vFDRCSynonymeDetailEdit As New FDRCSynonymeDetailEdit
            vFDRCSynonymeDetailEdit.SelectedDrcId = selectedDrcId
            vFDRCSynonymeDetailEdit.SelectedDrcLibelle = selectedDrcLibelle
            vFDRCSynonymeDetailEdit.SelectedDrcSynonymeId = 0
            vFDRCSynonymeDetailEdit.SelectedDrcSynonyme = ""
            vFDRCSynonymeDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
            vFDRCSynonymeDetailEdit.ShowDialog() 'Modal
            If vFDRCSynonymeDetailEdit.CodeRetour = True Then
                ChargementDrcSynonyme(selectedDrcId)
            End If
            vFDRCSynonymeDetailEdit.Dispose()
        End If
    End Sub

    Private Sub FDRCListe_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            DRCDataGridView.Rows.Clear()
            drcSynonymeDataTable.Clear()
            ChargementDRC()
        End If
    End Sub
End Class