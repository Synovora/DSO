Imports System.Data.SqlClient
Imports Oasis_WF
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.Localization
Imports Oasis_Common

Public Class RadFDRCSelecteur
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedDrcId As Integer
    Private _SelectedDrc As Drc
    Private _CategorieOasis As Integer

    Public Property SelectedPatient As Patient
        Get
            Return privateSelectedPatient
        End Get
        Set(value As Patient)
            privateSelectedPatient = value
        End Set
    End Property

    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property

    Public Property SelectedDrcId As Integer
        Get
            Return privateSelectedDrcId
        End Get
        Set(value As Integer)
            privateSelectedDrcId = value
        End Set
    End Property

    Public Property CategorieOasis As Integer
        Get
            Return _CategorieOasis
        End Get
        Set(value As Integer)
            _CategorieOasis = value
        End Set
    End Property

    Public Property SelectedDrc As Drc
        Get
            Return _SelectedDrc
        End Get
        Set(value As Drc)
            _SelectedDrc = value
        End Set
    End Property

    Dim drcDataTable As DataTable = New DataTable()
    Dim SelectAld As Boolean
    ReadOnly drcSynonymeDataTable As DataTable = New DataTable()
    ReadOnly instanceDrc As New Drc
    ReadOnly drcdao As New DrcDao

    Private Sub RadFDRCSelecteur_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()

        ChargementEtatCivil()

        'Chargement du label affichant la catégorie Oasis en restriction dans l'affichage en entête
        If CategorieOasis <> 0 Then
            LblCategorieOasis.Text = drcdao.GetItemCategorieOasisByCode(CategorieOasis)
            If CategorieOasis <> Drc.EnumCategorieOasisCode.Contexte Then
                ChargementDrc()
            End If
        Else
            LblCategorieOasis.Text = ""
        End If

        InitAffichageLabel()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ChargementDrc()
        Cursor.Current = Cursors.WaitCursor

        drcSynonymeDataTable.Rows.Clear()
        DrcDataGridView.Rows.Clear()

        InitAffichageLabel()

        If RadChkAld.Checked = True Then
            SelectAld = True
        Else
            SelectAld = False
        End If

        If SelectedPatient IsNot Nothing Then
            drcDataTable = drcdao.GetAllDrcByCategorieAndGenre(TxtDrc.Text, 0, CategorieOasis, SelectAld, SelectedPatient.PatientGenreId)
        Else
            drcDataTable = drcdao.GetAllDrcByCategorieAndGenre(TxtDrc.Text, 0, CategorieOasis, SelectAld, "")
        End If

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = drcDataTable.Rows.Count - 1
        'Dim drcIdPrecedent, drcIdEnCours As Integer

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        'drcIdPrecedent = 0
        For i = 0 To rowCount Step 1
            'Ne pas traiter les doublons liées à la requête (JOIN LEFT)
            'drcIdEnCours = CInt(drcDataTable.Rows(i)("oa_drc_id"))
            'If drcIdEnCours = drcIdPrecedent Then
            'Continue For
            'Else
            'drcIdPrecedent = drcIdEnCours
            'End If

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            DrcDataGridView.Rows.Add(iGrid)
            'Alimentation du Grid
            DrcDataGridView.Rows(iGrid).Cells("drcId").Value = drcDataTable.Rows(i)("oa_drc_id")
            DrcDataGridView.Rows(iGrid).Cells("drcDescription").Value = drcDataTable.Rows(i)("oa_drc_libelle")
            DrcDataGridView.Rows(iGrid).Cells("categorieMajeure").Value = drcDataTable.Rows(i)("oa_r_categorie_majeure_description")
            DrcDataGridView.Rows(iGrid).Cells("oa_drc_age_min").Value = Coalesce(drcDataTable.Rows(i)("oa_drc_age_min"), 0)
            DrcDataGridView.Rows(iGrid).Cells("oa_drc_age_max").Value = Coalesce(drcDataTable.Rows(i)("oa_drc_age_max"), 0)
            DrcDataGridView.Rows(iGrid).Cells("oa_drc_ald_id").Value = Coalesce(drcDataTable.Rows(i)("oa_drc_ald_id"), 0)
            DrcDataGridView.Rows(iGrid).Cells("oa_drc_categorie_majeure_id").Value = Coalesce(drcDataTable.Rows(i)("oa_drc_categorie_majeure_id"), 0)
            DrcDataGridView.Rows(iGrid).Cells("oa_ald_description").Value = Coalesce(drcDataTable.Rows(i)("oa_ald_description"), "")
            DrcDataGridView.Rows(iGrid).Cells("contexte").Value = Coalesce(drcDataTable.Rows(i)("oa_drc_sexe"), 0)
            If Coalesce(drcDataTable.Rows(i)("oa_drc_oasis"), False) = True Then
                DrcDataGridView.Rows(iGrid).Cells("drcOasis").Value = "Oasis"
                DrcDataGridView.Rows(iGrid).Cells("drcOasis").Style.ForeColor = Color.Red
            Else
                DrcDataGridView.Rows(iGrid).Cells("drcOasis").Value = ""
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If DrcDataGridView.Rows.Count > 0 Then
            Me.DrcDataGridView.CurrentRow = DrcDataGridView.ChildRows(0)
        End If

        Cursor.Current = Cursors.Default
    End Sub

    'Chargement des synonymes
    Private Sub ChargementDrcSynonyme(drcId As Integer)
        Dim drcSynonymeDataAdapter As SqlDataAdapter = New SqlDataAdapter()

        Dim conxn As New SqlConnection(GetConnectionString())

        Dim SQLString As String
        'Exlusion des traitements : traitements déclarés 'annulé' 
        SQLString = getSQLStringDRCSynonyme(drcId)

        drcSynonymeDataTable.Rows.Clear()

        drcSynonymeDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        drcSynonymeDataAdapter.Fill(drcSynonymeDataTable)

        RadDrcDefinitionDataGridView.DataSource = drcSynonymeDataTable

        conxn.Close()
        drcSynonymeDataAdapter.Dispose()

        'Enlève le focus sur la première ligne de la Grid
        RadDrcDefinitionDataGridView.ClearSelection()
    End Sub


    Private Function GetSQLStringDRCSynonyme(drcId As Integer) As String
        Dim SQLString As String
        SQLString = "SELECT oa_drc_synonyme_libelle FROM oasis.oa_drc_synonyme WHERE oa_drc_id = " + drcId.ToString + " order by oa_drc_synonyme_id;"

        GetSQLStringDRCSynonyme = SQLString
    End Function

    Private Sub ChargementEtatCivil()
        If SelectedPatient Is Nothing Then
            RadGroupBoxEtatCivil.Hide()
            Exit Sub
        End If
        LblPatientNIR.Text = SelectedPatient.PatientNir
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientAdresse1.Text = SelectedPatient.PatientAdresse1
        LblPatientAdresse2.Text = SelectedPatient.PatientAdresse2
        LblPatientCodePostal.Text = SelectedPatient.PatientCodePostal
        LblPatientVille.Text = SelectedPatient.PatientVille
        LblPatientTel1.Text = SelectedPatient.PatientTel1
        LblPatientTel2.Text = SelectedPatient.PatientTel2
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)
        LblPatientDateMaj.Text = SelectedPatient.PatientSyntheseDateMaj.ToString("dd/MM/yyyy")
        'Recherche si ALD
        LblDrcAld.Hide()

        'Vérification de l'existence d'ALD
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblDrcAld.Show()
            ToolTip.SetToolTip(LblDrcAld, StringTooltip)
        End If

        'If outils.ControleExistenceALD(Me.SelectedPatient.patientId) = True Then
        'LblALD.Show()
        'End If
    End Sub
    Private Sub RadDrcDataGridView_CellClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles DrcDataGridView.CellClick
        Selection()
    End Sub

    Private Sub Selection()
        Dim Sexe As Integer
        Dim AgeMin, AgeMax As Integer
        Dim aRow, maxRow As Integer

        aRow = Me.DrcDataGridView.Rows.IndexOf(Me.DrcDataGridView.CurrentRow)
        maxRow = DrcDataGridView.Rows.Count - 1

        If aRow <= maxRow And aRow > -1 Then
            'If RadDrcDataGridView.CurrentRow IsNot Nothing Then
            Dim drcId As Integer = DrcDataGridView.Rows(aRow).Cells("drcId").Value
            ChargementDrcSynonyme(drcId)

            InitAffichageLabel()

            LblDrcId.Text = DrcDataGridView.Rows(aRow).Cells("drcId").Value
            LblDrcLibelle.Text = DrcDataGridView.Rows(aRow).Cells("drcDescription").Value
            LblDrcAgeMin.Text = DrcDataGridView.Rows(aRow).Cells("oa_drc_age_min").Value
            AgeMin = DrcDataGridView.Rows(aRow).Cells("oa_drc_age_min").Value
            LblDrcAgeMax.Text = DrcDataGridView.Rows(aRow).Cells("oa_drc_age_max").Value
            AgeMax = DrcDataGridView.Rows(aRow).Cells("oa_drc_age_max").Value
            Sexe = CInt(DrcDataGridView.Rows(aRow).Cells("contexte").Value)
            TxtCategorieMajeure.Text = Table_categorie_majeure.GetCategorieMajeureDescription(DrcDataGridView.Rows(aRow).Cells("oa_drc_categorie_majeure_id").Value)
            Dim AldId As Integer = Coalesce(DrcDataGridView.Rows(aRow).Cells("oa_drc_ald_id").Value, 0)
            If AldId <> 0 Then
                TxtAldDescription.Text = Table_ald.GetAldDescription(AldId)
                LblLabelAld.Show()
                TxtAldDescription.Show()
            Else
                TxtAldDescription.Text = ""
                LblLabelAld.Hide()
                TxtAldDescription.Hide()
            End If

            LblLabelDrcAge.Hide()
            If SelectedPatient IsNot Nothing Then
                If AgeMin <> 0 Then
                    If SelectedPatient.PatientAgeEnAnnee < AgeMin Then
                        LblLabelDrcAge.Show()
                    End If
                End If
                If AgeMax <> 0 Then
                    If SelectedPatient.PatientAgeEnAnnee > AgeMax Then
                        LblLabelDrcAge.Show()
                    End If
                End If
            End If

            If LblDrcId.Text <> "" Then
                RadBtnSelection.Show()
                RadPnlSelection.Show()
            End If
        End If
    End Sub

    Private Sub RadBtnSelection_Click(sender As Object, e As EventArgs) Handles RadBtnSelection.Click
        SelectionRetour()
    End Sub


    Private Sub SelectionRetour()
        Dim drcId As Integer
        If RadPnlSelection.Visible = True Then
            If LblDrcId.Text <> "" Then
                If IsNumeric(LblDrcId.Text) Then
                    drcId = CInt(LblDrcId.Text)
                    Me.SelectedDrcId = drcId
                    drcdao.GetDrc(instanceDrc, drcId)
                    SelectedDrc = instanceDrc
                    Me.Close()
                End If
            End If
        End If
    End Sub

    Private Sub DrcDataGridView_DoubleClick(sender As Object, e As EventArgs) Handles DrcDataGridView.DoubleClick
        Selection()
        SelectionRetour()
    End Sub


    Private Sub InitAffichageLabel()
        LblDrcId.Text = ""
        LblDrcLibelle.Text = ""
        LblDrcAgeMin.Text = ""
        LblDrcAgeMax.Text = ""
        RadPnlSelection.Hide()
        RadBtnSelection.Hide()
    End Sub

    Private Sub RadDrcDataGridView_CellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs) Handles DrcDataGridView.CellFormatting
        Dim columnName As String = e.Column.Name
        Dim value As Object = e.Row.Cells(columnName).Value
        If columnName = "oa_drc_oasis" Then
            If value IsNot DBNull.Value Then
                If value = "1" Then
                    e.CellElement.Text = "Oasis"
                    e.CellElement.ForeColor = Color.Red
                Else
                    e.CellElement.Text = ""
                End If
            Else
                e.CellElement.Text = ""
            End If
        End If
    End Sub

    Private Sub TxtDrc_TextChanged(sender As Object, e As EventArgs) Handles TxtDrc.TextChanged
        If TxtDrc.Text.Trim <> "" Then
            If Len(TxtDrc.Text) > 2 Then
                ChargementDrc()
            End If
        Else
            If CategorieOasis = Drc.EnumCategorieOasisCode.Contexte Then
                drcSynonymeDataTable.Rows.Clear()
                DrcDataGridView.Rows.Clear()
            Else
                ChargementDrc()
            End If
            'Application.DoEvents()
            'ChargementDrc()
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadChkAld_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles RadChkAld.ToggleStateChanged
        Application.DoEvents()
        ChargementDrc()
    End Sub

    Private Sub RadDrcDataGridView_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles DrcDataGridView.ToolTipTextNeeded
        Dim cell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If cell IsNot Nothing AndAlso (cell.ColumnInfo.Name = "oa_drc_libelle" Or cell.ColumnInfo.Name = "oa_r_categorie_majeure_description") Then
            If cell.Value IsNot Nothing Then
                e.ToolTipText = cell.Value.ToString()
            End If
        End If
    End Sub

End Class
