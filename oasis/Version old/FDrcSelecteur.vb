Imports System.Data.SqlClient
Imports Oasis_Common
Public Class FDrcSelecteur
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedDrcId As Integer
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

    'Le DataAdapter a pour objet de récupérer les données de la BDD et permettre le renvoi des modifications à la BDD
    Dim drcDataAdapter As SqlDataAdapter = New SqlDataAdapter()

    'Le DataTable contient les données que le Grid va afficher (on pourrait utiliser un Dataset si on utilise plusieurs tables)
    Dim drcDataTable As DataTable = New DataTable()
    Dim drcSynonymeDataTable As DataTable = New DataTable()

    Dim categorieMajeureListe As Dictionary(Of Integer, String) = Table_categorie_majeure.GetCategorieMajeureListe()

    Private Sub FDrcSelecteur_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementEtatCivil()
        'Permet de gérer la touche entrée
        KeyPreview = True
        initZones()
        InitAffichageLabel()
        Me.SelectedDrcId = 0
        'Récupération des données de la table [oa_patient] dans un DataTable et liason du DataTable avec la grid
        ChargementDrc()
        If CategorieOasis <> 0 Then
            Select Case CategorieOasis
                Case 1
                    LblCategorieOasis.Text = "Contexte et Antécédent"
                Case 2
                    LblCategorieOasis.Text = "Stratégie"
                Case 3
                    LblCategorieOasis.Text = "Prévention"
                Case 4
                    LblCategorieOasis.Text = "Objectif"
            End Select
        Else
            LblCategorieOasis.Text = ""
        End If
    End Sub

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
        'Traitements
        DrcDataGridView.Rows.Clear()
    End Sub
    Private Sub ChargementDrc()
        Dim drcDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim drcDataTable As DataTable = New DataTable()

        Dim conxn As New SqlConnection(outils.getConnectionString())

        Dim SQLString As String
        'Exlusion des traitements : traitements déclarés 'annulé' 
        SQLString = getSQLStringDRC()

        drcDataTable.Clear()

        drcDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        drcDataAdapter.Fill(drcDataTable)

        'DrcDataGridView.DataSource = drcDataTable

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = drcDataTable.Rows.Count - 1
        Dim drcIdPrecedent, drcIdEnCours, categorieMajeureId As Integer
        Dim categorieMajeureDescription As String
        Dim AgeMin, AgeMax, GenreId As Integer
        Dim drcOasis As Boolean

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

            categorieMajeureDescription = ""
            If drcDataTable.Rows(i)("oa_drc_categorie_majeure_id") IsNot DBNull.Value Then
                categorieMajeureId = CInt(drcDataTable.Rows(i)("oa_drc_categorie_majeure_id"))
                categorieMajeureDescription = Table_categorie_majeure.GetCategorieMajeureDescription(categorieMajeureId)
            End If

            AgeMin = 0
            If drcDataTable.Rows(i)("oa_drc_age_min") IsNot DBNull.Value Then
                AgeMin = drcDataTable.Rows(i)("oa_drc_age_min")
            End If

            AgeMax = 0
            If drcDataTable.Rows(i)("oa_drc_age_max") IsNot DBNull.Value Then
                AgeMax = drcDataTable.Rows(i)("oa_drc_age_max")
            End If

            GenreId = 0
            If drcDataTable.Rows(i)("oa_drc_sexe") IsNot DBNull.Value Then
                GenreId = drcDataTable.Rows(i)("oa_drc_sexe")
            End If

            drcOasis = False
            If drcDataTable.Rows(i)("oa_drc_oasis") IsNot DBNull.Value Then
                If drcDataTable.Rows(i)("oa_drc_oasis") = 1 Then
                    drcOasis = True
                End If
            End If

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            DrcDataGridView.Rows.Insert(iGrid)

            '------------------- Alimentation du DataGridView
            'Id DRC
            DrcDataGridView("drcId", iGrid).Value = drcDataTable.Rows(i)("oa_drc_id")

            'DRC Description
            DrcDataGridView("drcDescription", iGrid).Value = drcDataTable.Rows(i)("oa_drc_libelle")

            DrcDataGridView("categorieMajeure", iGrid).Value = categorieMajeureDescription

            DrcDataGridView("oa_drc_age_min", iGrid).Value = AgeMin

            DrcDataGridView("oa_drc_age_max", iGrid).Value = AgeMax

            DrcDataGridView("contexte", iGrid).Value = GenreId

            If drcOasis = True Then
                DrcDataGridView("drcOasis", iGrid).Value = "Oasis"
                DrcDataGridView("drcOasis", iGrid).Style.ForeColor = Color.Red
            Else
                DrcDataGridView("drcOasis", iGrid).Value = ""
            End If
        Next

        conxn.Close()
        drcDataAdapter.Dispose()

        'Enlève le focus sur la première ligne de la Grid
        DrcDataGridView.ClearSelection()
    End Sub

    Private Sub ChargementDrcSynonyme(drcId As Integer)
        Dim drcSynonymeDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        'Dim drcSynonymeDataTable As DataTable = New DataTable()

        Dim conxn As New SqlConnection(outils.getConnectionString())

        Dim SQLString As String
        'Exlusion des traitements : traitements déclarés 'annulé' 
        SQLString = getSQLStringDRCSynonyme(drcId)

        drcSynonymeDataTable.Clear()

        drcSynonymeDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        drcSynonymeDataAdapter.Fill(drcSynonymeDataTable)

        DrcDefinitionDataGridView.DataSource = drcSynonymeDataTable

        conxn.Close()
        drcSynonymeDataAdapter.Dispose()

        'Enlève le focus sur la première ligne de la Grid
        DrcDefinitionDataGridView.ClearSelection()
    End Sub

    Private Function getSQLStringDRC() As String
        Dim SQLString As String
        Dim clauseDrc As String
        Dim clauseCategorieMajeure As String
        Dim clauseCategorieOasis As String
        Dim clauseORC As String
        Dim categorieMajeureId As Integer

        If TxtDrc.Text = "" Then
            clauseDrc = "1 = 1"
        Else
            clauseDrc = "(oa_drc_libelle LIKE '%" & TxtDrc.Text & "%' or oa_drc_synonyme_libelle like '%" & TxtDrc.Text & "%')"
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

        If Me.CategorieOasis = 0 Then
            clauseCategorieOasis = "1 = 1 "
        Else
            clauseCategorieOasis = "oa_drc_oasis_categorie = " & CategorieOasis & " "
        End If

        If ChkORC.Checked = True Then
            clauseORC = "oa_drc_oasis = 1"
        Else
            clauseORC = "1 = 1"
        End If

        SQLString = "SELECT * FROM oasis.oa_drc left join oasis.oa_drc_synonyme on oasis.oasis.oa_drc.oa_drc_id = oasis.oa_drc_synonyme.oa_drc_id WHERE " & clauseCategorieMajeure & " AND " & clauseDrc & " AND " & clauseCategorieOasis & " AND " & clauseORC & "order by oasis.oasis.oa_drc.oa_drc_id;"

        Select Case SelectedPatient.PatientGenreId
            Case "M"
                SQLString = "SELECT * FROM oasis.oa_drc left join oasis.oa_drc_synonyme on oasis.oasis.oa_drc.oa_drc_id = oasis.oa_drc_synonyme.oa_drc_id WHERE " & clauseCategorieMajeure & " AND " & clauseDrc & " AND " & clauseCategorieOasis & " AND " & clauseORC & " AND (oa_drc_sexe = 1 or oa_drc_sexe = 3) order by oasis.oasis.oa_drc.oa_drc_id;"
            Case "F"
                SQLString = "Select * FROM oasis.oa_drc left join oasis.oa_drc_synonyme on oasis.oasis.oa_drc.oa_drc_id = oasis.oa_drc_synonyme.oa_drc_id WHERE " & clauseCategorieMajeure & " And " & clauseDrc & " And " & clauseCategorieOasis & " AND " & clauseORC & " And (oa_drc_sexe = 2 Or oa_drc_sexe = 3) order by oasis.oasis.oa_drc.oa_drc_id;"
        End Select

        getSQLStringDRC = SQLString
    End Function

    'Chargement des données dans les labels dédiés
    Private Sub ChargementEtatCivil()
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
    End Sub

    Private Function getSQLStringDRCSynonyme(drcId As Integer) As String
        Dim SQLString As String
        'SQLString = "SELECT oa_drc_criteres FROM oasis.oa_drc_definition WHERE oa_drc_validite = 1 and oa_drc_def_drc_id = " + drcId.ToString + " order by oa_drc_ordre_affichage;"
        SQLString = "SELECT oa_drc_synonyme_libelle FROM oasis.oa_drc_synonyme WHERE oa_drc_id = " + drcId.ToString + " order by oa_drc_synonyme_id;"

        getSQLStringDRCSynonyme = SQLString
    End Function

    Private Sub InitAffichageLabel()
        LblDrcId.Text = ""
        LblDrcLibelle.Text = ""
        'LblLabelDrcAge.Text = ""
        LblDrcAgeMin.Text = ""
        LblDrcAgeMax.Text = ""
        PnlSelection.Hide()
        BtnSelection.Hide()
    End Sub

    Private Sub BtnSelection_Click(sender As Object, e As EventArgs) Handles BtnSelection.Click
        Dim drcId As Integer
        drcId = CInt(LblDrcId.Text)
        Me.SelectedDrcId = drcId
        Me.Close()
    End Sub

    Private Sub FDrcSelecteur_KeyDown(sender As Object, e As KeyEventArgs) Handles MyBase.KeyDown
        If e.KeyCode = Keys.Enter Then
            DrcDataGridView.Rows.Clear()
            drcSynonymeDataTable.Clear()
            ChargementDrc()
            InitAffichageLabel()
        End If
    End Sub

    Private Sub BtnFiltrer_Click(sender As Object, e As EventArgs) Handles BtnFiltrer.Click
        DrcDataGridView.Rows.Clear()
        drcSynonymeDataTable.Clear()
        ChargementDrc()
        InitAffichageLabel()
    End Sub

    Private Sub BtnInitialiser_Click(sender As Object, e As EventArgs) Handles BtnInitialiser.Click
        TxtDrc.Text = ""
        'Catégorie Majeure
        For Each kvp As KeyValuePair(Of Integer, String) In categorieMajeureListe
            If kvp.Key = 0 Then
                CbxFiltreCategorieMajeure.SelectedItem = kvp.Value
                Exit For
            End If
        Next kvp
        'ORC/Tous
        ChkORC.Checked = False
        'Rechargement Grid
        drcSynonymeDataTable.Clear()
        DrcDataGridView.Rows.Clear()
        ChargementDrc()
    End Sub

    Private Sub DrcDataGridView_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles DrcDataGridView.CellClick
        Dim Sexe As Integer
        Dim AgeMin, AgeMax As Integer

        If DrcDataGridView.CurrentRow IsNot Nothing Then
            Dim drcId As Integer = DrcDataGridView.Rows(DrcDataGridView.CurrentRow.Index).Cells("drcId").Value
            ChargementDrcSynonyme(drcId)

            InitAffichageLabel()

            LblDrcId.Text = DrcDataGridView.Rows(DrcDataGridView.CurrentRow.Index).Cells("drcId").Value
            LblDrcLibelle.Text = DrcDataGridView.Rows(DrcDataGridView.CurrentRow.Index).Cells("drcDescription").Value
            LblDrcAgeMin.Text = DrcDataGridView.Rows(DrcDataGridView.CurrentRow.Index).Cells("oa_drc_age_min").Value
            AgeMin = DrcDataGridView.Rows(DrcDataGridView.CurrentRow.Index).Cells("oa_drc_age_min").Value
            LblDrcAgeMax.Text = DrcDataGridView.Rows(DrcDataGridView.CurrentRow.Index).Cells("oa_drc_age_max").Value
            AgeMax = DrcDataGridView.Rows(DrcDataGridView.CurrentRow.Index).Cells("oa_drc_age_max").Value
            Sexe = CInt(DrcDataGridView.Rows(DrcDataGridView.CurrentRow.Index).Cells("oa_drc_sexe").Value)

            If SelectedPatient.PatientAgeEnAnnee < AgeMin Or SelectedPatient.PatientAgeEnAnnee > AgeMax Then
                LblLabelDrcAge.Show()
            Else
                LblLabelDrcAge.Hide()
            End If

            If LblDrcId.Text <> "" Then
                    BtnSelection.Show()
                    PnlSelection.Show()
                End If
            End If
    End Sub
End Class