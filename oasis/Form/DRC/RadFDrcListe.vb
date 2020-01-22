Imports Telerik.WinControls.UI.Localization

Public Class RadFDrcListe
    Private privateUtilisateurConnecte As Utilisateur

    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property

    Dim selectedDrcId As Integer
    Dim selectedDrcLibelle As String
    Dim categorieMajeureListe As Dictionary(Of Integer, String) = Table_categorie_majeure.GetCategorieMajeureListe()

    Dim drcDataTable As New DataTable
    Dim drcSynonymeDataTable As New DataTable
    Dim SelectAld As Boolean
    Dim drcdao As New DrcDao
    Dim alddao As New AldDao
    Dim Ald As Ald

    Private Sub RadFDrcListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CbxCategorieOasis.Items.Clear()
        CbxCategorieOasis.Items.Add("Toutes")
        CbxCategorieOasis.Items.Add(DrcDao.EnumCategorieOasisItem.Contexte)
        CbxCategorieOasis.Items.Add(DrcDao.EnumCategorieOasisItem.Objectif)
        CbxCategorieOasis.Items.Add(DrcDao.EnumCategorieOasisItem.Prevention)
        CbxCategorieOasis.Items.Add(DrcDao.EnumCategorieOasisItem.Strategie)
        CbxCategorieOasis.Items.Add(DrcDao.EnumCategorieOasisItem.ActeParamedical)
        CbxCategorieOasis.Items.Add(DrcDao.EnumCategorieOasisItem.GroupeParametres)
        CbxCategorieOasis.Items.Add(DrcDao.EnumCategorieOasisItem.ProtocoleCollaboratif)
        CbxCategorieOasis.Items.Add(DrcDao.EnumCategorieOasisItem.ProtocoleAigu)
        CbxCategorieOasis.Text = "Toutes"

        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()
        ChargementDrc()
    End Sub

    Private Sub ChargementDrc()
        Cursor.Current = Cursors.WaitCursor
        drcSynonymeDataTable.Rows.Clear()
        DrcDataGridView.Rows.Clear()
        Dim CategorieOasis As Integer

        If RadChkAld.Checked = True Then
            SelectAld = True
        Else
            SelectAld = False
        End If

        If CbxCategorieOasis.Text = "Toutes" Then
            CategorieOasis = 0
        Else
            CategorieOasis = drcdao.GetCodeCategorieOasisByItem(CbxCategorieOasis.Text)
        End If

        drcDataTable = drcdao.GetAllDrcByCategorie(TxtFiltreDescription.Text, 0, CategorieOasis, SelectAld, "")

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = drcDataTable.Rows.Count - 1
        Dim drcIdPrecedent, drcIdEnCours As Integer

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        drcIdPrecedent = 0
        For i = 0 To rowCount Step 1
            'Ne pas traiter les doublons liées à la requête (JOIN LEFT)
            drcIdEnCours = CInt(drcDataTable.Rows(i)("oa_drc_id"))
            If drcIdEnCours = drcIdPrecedent Then
                Continue For
            Else
                drcIdPrecedent = drcIdEnCours
            End If

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            DrcDataGridView.Rows.Add(iGrid)
            'Id DRC
            DrcDataGridView.Rows(iGrid).Cells("drcId").Value = drcDataTable.Rows(i)("oa_drc_id")
            DrcDataGridView.Rows(iGrid).Cells("drcDescription").Value = drcDataTable.Rows(i)("oa_drc_libelle")
            DrcDataGridView.Rows(iGrid).Cells("oa_drc_categorie_majeure_id").Value = Coalesce(drcDataTable.Rows(i)("oa_drc_categorie_majeure_id"), 0)
            DrcDataGridView.Rows(iGrid).Cells("categorieMajeure").Value = drcDataTable.Rows(i)("oa_r_categorie_majeure_description")

            DrcDataGridView.Rows(iGrid).Cells("oa_drc_age_min").Value = Coalesce(drcDataTable.Rows(i)("oa_drc_age_min"), 0)
            DrcDataGridView.Rows(iGrid).Cells("oa_drc_age_max").Value = Coalesce(drcDataTable.Rows(i)("oa_drc_age_max"), 0)
            DrcDataGridView.Rows(iGrid).Cells("oa_drc_ald_id").Value = Coalesce(drcDataTable.Rows(i)("oa_drc_ald_id"), 0)
            DrcDataGridView.Rows(iGrid).Cells("contexte").Value = Coalesce(drcDataTable.Rows(i)("oa_drc_sexe"), 0)

            DrcDataGridView.Rows(iGrid).Cells("oa_drc_ald_id").Value = Coalesce(drcDataTable.Rows(i)("oa_drc_ald_id"), 0)
            DrcDataGridView.Rows(iGrid).Cells("oa_drc_ald_code").Value = Coalesce(drcDataTable.Rows(i)("oa_drc_ald_code"), "")
            DrcDataGridView.Rows(iGrid).Cells("oa_ald_description").Value = Coalesce(drcDataTable.Rows(i)("oa_ald_description"), "")

            If Coalesce(drcDataTable.Rows(i)("oa_drc_oasis"), False) = True Then
                DrcDataGridView.Rows(iGrid).Cells("drcOasis").Value = "Oasis"
                DrcDataGridView.Rows(iGrid).Cells("drcOasis").Style.ForeColor = Color.Red
            Else
                DrcDataGridView.Rows(iGrid).Cells("drcOasis").Value = ""
            End If

            DrcDataGridView.Rows(iGrid).Cells("categorieOasis").Value = drcdao.GetItemCategorieOasisByCode(drcDataTable.Rows(i)("oa_drc_oasis_categorie"))
        Next

        'Positionnement du grid sur la première occurrence
        If DrcDataGridView.Rows.Count > 0 Then
            Me.DrcDataGridView.CurrentRow = DrcDataGridView.Rows(0)
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ChargementDrcSynonyme(drcId As Integer)
        Dim drcSynonymeDao As DrcSynonymeDao = New DrcSynonymeDao()
        drcSynonymeDataTable = drcSynonymeDao.getAllSynonymebyDrc(drcId)
        RadDrcDSynonymeDataGridView.DataSource = drcSynonymeDataTable

        'Enlève le focus sur la première ligne de la Grid
        RadDrcDSynonymeDataGridView.ClearSelection()
    End Sub

    Private Sub RadDRCDataGridView_CellClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles DrcDataGridView.CellClick
        If DrcDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.DrcDataGridView.Rows.IndexOf(Me.DrcDataGridView.CurrentRow)
            If aRow > -1 Then
                Dim drcId As Integer = DrcDataGridView.Rows(aRow).Cells("drcId").Value
                Dim drcLibelle As String = DrcDataGridView.Rows(aRow).Cells("drcDescription").Value
                selectedDrcId = drcId
                selectedDrcLibelle = drcLibelle
                ChargementDrcSynonyme(drcId)
            End If
        End If
    End Sub

    Private Sub RadDRCDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles DrcDataGridView.CellDoubleClick
        If DrcDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.DrcDataGridView.Rows.IndexOf(Me.DrcDataGridView.CurrentRow)
            If aRow > -1 Then
                Dim drcId As Integer = DrcDataGridView.Rows(aRow).Cells("drcId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vFDRCDetailEdit As New RadFDrcDetailEdit
                    vFDRCDetailEdit.SelectedDRCId = drcId
                    vFDRCDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFDRCDetailEdit.ShowDialog() 'Modal
                    If vFDRCDetailEdit.CodeRetour = True Then
                        ChargementDrc()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadBtnCreationORC_Click(sender As Object, e As EventArgs) Handles RadBtnCreationORC.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFDRCDetailEdit As New RadFDrcDetailEdit
            vFDRCDetailEdit.SelectedDRCId = 0
            vFDRCDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
            vFDRCDetailEdit.ShowDialog() 'Modal
            If vFDRCDetailEdit.CodeRetour = True Then
                ChargementDrc()
            End If
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadDrcDSynonymeDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadDrcDSynonymeDataGridView.CellDoubleClick
        If RadDrcDSynonymeDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadDrcDSynonymeDataGridView.Rows.IndexOf(Me.RadDrcDSynonymeDataGridView.CurrentRow)
            If aRow >= 0 Then
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vFDRCSynonymeDetailEdit As New RadFDrcSynonymeDetailEdit
                    vFDRCSynonymeDetailEdit.SelectedDrcId = selectedDrcId
                    vFDRCSynonymeDetailEdit.SelectedDrcLibelle = selectedDrcLibelle
                    Dim drcSynonymeId As Integer = RadDrcDSynonymeDataGridView.Rows(aRow).Cells("oa_drc_synonyme_id").Value
                    vFDRCSynonymeDetailEdit.SelectedDrcSynonymeId = drcSynonymeId
                    vFDRCSynonymeDetailEdit.SelectedDrcSynonyme = RadDrcDSynonymeDataGridView.Rows(aRow).Cells("oa_drc_synonyme_libelle").Value
                    vFDRCSynonymeDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFDRCSynonymeDetailEdit.ShowDialog() 'Modal
                    If vFDRCSynonymeDetailEdit.CodeRetour = True Then
                        ChargementDrcSynonyme(selectedDrcId)
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadChkAld_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkAld.ToggleStateChanged
        Application.DoEvents()
        ChargementDrc()
    End Sub

    Private Sub RadBtnCreerSynonyme_Click(sender As Object, e As EventArgs) Handles RadBtnCreerSynonyme.Click
        If selectedDrcId <> 0 Then
            Me.Enabled = False
            Cursor.Current = Cursors.WaitCursor
            Using vFDRCSynonymeDetailEdit As New RadFDrcSynonymeDetailEdit
                vFDRCSynonymeDetailEdit.SelectedDrcId = selectedDrcId
                vFDRCSynonymeDetailEdit.SelectedDrcLibelle = selectedDrcLibelle
                vFDRCSynonymeDetailEdit.SelectedDrcSynonymeId = 0
                vFDRCSynonymeDetailEdit.SelectedDrcSynonyme = ""
                vFDRCSynonymeDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                vFDRCSynonymeDetailEdit.ShowDialog() 'Modal
                If vFDRCSynonymeDetailEdit.CodeRetour = True Then
                    ChargementDrcSynonyme(selectedDrcId)
                End If
            End Using
            Me.Enabled = True
        Else
            MessageBox.Show("Vous devez sélectionner une DRC pour créer un synonyme")
        End If
    End Sub

    Private Sub TxtFiltreDescription_TextChanged(sender As Object, e As EventArgs) Handles TxtFiltreDescription.TextChanged
        If TxtFiltreDescription.Text.Trim <> "" Then
            If Len(TxtFiltreDescription.Text) > 2 Then
                Application.DoEvents()
                ChargementDrc()
            End If
        Else
            Application.DoEvents()
            ChargementDrc()
        End If
    End Sub

    Private Sub RadDRCDataGridView_FilterChanged(sender As Object, e As Telerik.WinControls.UI.GridViewCollectionChangedEventArgs) Handles DrcDataGridView.FilterChanged
        RadDrcDSynonymeDataGridView.DataSource = Nothing
    End Sub

    Private Sub RadBtnDrcStandard_Click(sender As Object, e As EventArgs) Handles RadBtnDrcStandard.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vRadFDrcStandardTypeActiviteActivite As New RadFDrcStandardTypeActiviteListe
            vRadFDrcStandardTypeActiviteActivite.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnFiltrer_Click(sender As Object, e As EventArgs) Handles RadBtnFiltrer.Click
        ChargementDrc()
    End Sub
End Class
