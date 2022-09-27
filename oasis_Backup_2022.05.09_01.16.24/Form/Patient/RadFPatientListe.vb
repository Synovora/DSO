﻿Imports Telerik.WinControls
Imports Telerik.WinControls.UI.Localization
Imports Oasis_Common
Imports System.Configuration

Public Class RadFPatientListe
    Private privateUtilisateurConnecte As Utilisateur
    Private filterTache As New FiltreTache()

    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property

    ReadOnly aldDao As New AldDao
    ReadOnly patientDao As New PatientDao

    'Instanciation du patient pour le fournir aux Forms qui seront appelées depuis cette Form
    Dim SelectedPatient As New Patient

    Private Sub RadFPatientListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Liste des patients", userLog)

        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.TextAndButtonsElement.TextElement.ForeColor = Color.Red
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.BackColor = Color.DarkBlue
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.Font = New Font("Arial", 8.0F, FontStyle.Italic)
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.TextImageRelation = TextImageRelation.TextBeforeImage
        Me.RadDesktopAlert1.Popup.AlertElement.BackColor = Color.MistyRose
        Me.RadDesktopAlert1.Popup.AlertElement.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.BorderColor = Color.DarkBlue

        DTPDDN.Format = DateTimePickerFormat.Custom
        DTPDDN.CustomFormat = " "
        DTPDDN.Value = DTPDDN.MinDate

        TTValidation.ForeColor = Color.Red

        'Traitement des contextes obsolètes
        Dim parametreOasisDao As ParametreOasisDao = New ParametreOasisDao
        parametreOasisDao.TraitementContexte()

        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()

        'Provque le chargement de la liste des patients
        RadChkPatientOasis.CheckState = CheckState.Checked

        InitHabilitation()

        InitZonesSelectionPatient()

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ShowFilter()
        Try
            Me.Enabled = False
            Using frmFiltreTacheATraiter = New FrmFiltreTacheATraiter(filterTache)
                frmFiltreTacheATraiter.Tag = filterTache
                frmFiltreTacheATraiter.ShowDialog()
                If Not frmFiltreTacheATraiter.filtreTacheNouveau Is Nothing Then
                    filterTache = frmFiltreTacheATraiter.filtreTacheNouveau
                End If
            End Using
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
        End Try
        If filterTache.LstUniteSanitaire.Count > 0 Then
            BtnFilter.ForeColor = Color.Green
        Else
            BtnFilter.ForeColor = Nothing
        End If
    End Sub

    Private Sub ChargementPatient()
        If InputPrenom.Text.Length < 3 AndAlso InputNom.Text.Length < 3 AndAlso DTPDDN.Value = DTPDDN.MinDate AndAlso filterTache.LstUniteSanitaire.Count < 1 Then
            Return
        End If

        Dim patientDataTable As DataTable

        Dim Tous As Boolean
        Dim PatientOasis As Boolean

        RadPatientGridView.Rows.Clear()

        If RadChkPatientOasis.CheckState = CheckState.Checked Then
            PatientOasis = True
            Tous = False
        Else
            If RadChkPatientNonOasis.CheckState = CheckState.Checked Then
                PatientOasis = False
                Tous = False
            Else
                Tous = True
            End If
        End If
        Dim filter = ""
        filter = String.Format("{0} AND (LOWER(convert(varchar, oa_patient_prenom) COLLATE SQL_Latin1_General_Cp1251_CS_AS) LIKE '%{1}%' OR {2})", filter, InputPrenom.Text.Replace(" ", "").ToLower(), If(InputPrenom.Text.Length >= 3, "1!=1", "1=1"))
        filter = String.Format("{0} AND (LOWER(convert(varchar, oa_patient_nom) COLLATE SQL_Latin1_General_Cp1251_CS_AS) LIKE '%{1}%' OR {2})", filter, InputNom.Text.Replace(" ", "").ToLower(), If(InputNom.Text.Length >= 3, "1!=1", "1=1"))
        filter = String.Format("{0} AND (oa_patient_date_naissance = '{1}' OR {2})", filter, DTPDDN.Value.ToString("yyyy-MM-dd"), If(DTPDDN.Value <> DTPDDN.MinDate, "1!=1", "1=1"))
        filter = String.Format("{0} AND (oa_patient_site_id {1} OR {2})", filter, If(filterTache.GetListAllSite().Count > 0, "IN (" & String.Join(",", filterTache.GetListAllSite().Select(Function(x) x.Oa_site_id).ToArray()) & ")", "= ''"), If(filterTache.GetListAllSite().Count > 0, "1!=1", "1=1"))

        patientDataTable = patientDao.GetAllPatientWithFilter(Tous, PatientOasis, filter)

        Dim iGrid As Integer = -1
        Dim rowCount As Integer = patientDataTable.Rows.Count - 1
        LblOccurrenceLue.Text = patientDataTable.Rows.Count & " occurrence(s) lue(s)"
        For i = 0 To rowCount Step 1
            iGrid += 1
            RadPatientGridView.Rows.Add(iGrid)
            RadPatientGridView.Rows(iGrid).Cells("oa_patient_id").Value = patientDataTable.Rows(i)("oa_patient_id")
            Dim NIR As Long = Coalesce(patientDataTable.Rows(i)("oa_patient_nir"), 0)
            If NIR <> 0 Then
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_nir").Value = NIR
            Else
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_nir").Value = ""
            End If

            RadPatientGridView.Rows(iGrid).Cells("oa_patient_prenom").Value = Coalesce(patientDataTable.Rows(i)("oa_patient_prenom"), "")
            RadPatientGridView.Rows(iGrid).Cells("oa_patient_nom").Value = Coalesce(patientDataTable.Rows(i)("oa_patient_nom"), "")

            Dim DateNaissance As Date = Coalesce(patientDataTable.Rows(i)("oa_patient_date_naissance"), Nothing)
            If DateNaissance <> Nothing Then
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_naissance").Value = DateNaissance.ToString("dd/MM/yyyy")
                RadPatientGridView.Rows(iGrid).Cells("age").Value = CalculAgeEnAnneeEtMoisString(DateNaissance)
            Else
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_naissance").Value = ""
                RadPatientGridView.Rows(iGrid).Cells("age").Value = ""
            End If
            RadPatientGridView.Rows(iGrid).Cells("oa_patient_lieu_naissance").Value = Coalesce(patientDataTable.Rows(i)("oa_patient_lieu_naissance"), "")

            Dim DateEntreeOasis As Date = Coalesce(patientDataTable.Rows(i)("oa_patient_date_entree_oasis"), Nothing)
            If DateEntreeOasis <> Nothing Then
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_entree_oasis").Value = DateEntreeOasis.ToString("dd/MM/yyyy")
            Else
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_entree_oasis").Value = ""
            End If

            Dim DateSortieOasis As Date = Coalesce(patientDataTable.Rows(i)("oa_patient_date_sortie_oasis"), Nothing)
            If DateSortieOasis <> Nothing Then
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_sortie_oasis").Value = DateSortieOasis.ToString("dd/MM/yyyy")
            Else
                RadPatientGridView.Rows(iGrid).Cells("oa_patient_date_sortie_oasis").Value = ""
            End If

            Dim SiteId As Long = Coalesce(patientDataTable.Rows(i)("oa_patient_site_id"), 0)
            If SiteId <> 0 Then
                RadPatientGridView.Rows(iGrid).Cells("site").Value = Environnement.Table_site.GetSiteDescription(SiteId)
            Else
                RadPatientGridView.Rows(iGrid).Cells("site").Value = ""
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadPatientGridView.Rows.Count > 0 Then
            Me.RadPatientGridView.CurrentRow = RadPatientGridView.Rows(0)
        End If
    End Sub

    Private Sub DTPDDN_DropDown(sender As Object, e As EventArgs) Handles DTPDDN.DropDown
        If DTPDDN.Value = DTPDDN.MinDate Then
            DTPDDN.Value = Date.Now()
            DTPDDN.Format = DateTimePickerFormat.Long
        End If
    End Sub

    Private Sub RadGridView1_CellClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadPatientGridView.CellClick
        SelectionPatient()
    End Sub

    Private Sub SelectionPatient()
        Dim aRow As Integer
        Dim maxRow As Integer
        Dim DateSortie, DateEntree, dateNaissance As Date
        Dim DateMax As Date = "31/12/2999"

        aRow = Me.RadPatientGridView.Rows.IndexOf(Me.RadPatientGridView.CurrentRow)
        maxRow = RadPatientGridView.Rows.Count - 1

        If aRow <= maxRow And aRow > -1 Then
            TxtIdSelected.Text = RadPatientGridView.Rows(aRow).Cells("oa_patient_id").Value
            If RadPatientGridView.Rows(aRow).Cells("oa_patient_nir").Value Is DBNull.Value Then
                TxtNirSelected.Text = 0
            Else
                TxtNirSelected.Text = RadPatientGridView.Rows(aRow).Cells("oa_patient_nir").Value
            End If
            TxtPrenomSelected.Text = RadPatientGridView.Rows(aRow).Cells("oa_patient_prenom").Value
            TxtNomSelected.Text = RadPatientGridView.Rows(aRow).Cells("oa_patient_nom").Value

            If RadPatientGridView.Rows(aRow).Cells("oa_patient_date_naissance").Value <> "" Then
                dateNaissance = Coalesce(RadPatientGridView.Rows(aRow).Cells("oa_patient_date_naissance").Value, Nothing)
                LblDateNaissanceSelected.Text = dateNaissance.ToString("dd.MM.yyyy")
                LblDateNaissanceSelected.Show()
                LblAgeSelected.Text = CalculAgeEnAnneeEtMoisString(dateNaissance)
                LblAgeSelected.Show()
            Else
                LblDateNaissanceSelected.Hide()
                LblAgeSelected.Hide()
            End If

            DateSortie = DateMax
            If RadPatientGridView.Rows(aRow).Cells("oa_patient_date_sortie_oasis").Value <> "" Then
                DateSortie = Coalesce(RadPatientGridView.Rows(aRow).Cells("oa_patient_date_sortie_oasis").Value, Nothing)
            End If

            DateEntree = DateMax
            If RadPatientGridView.Rows(aRow).Cells("oa_patient_date_entree_oasis").Value <> "" Then
                DateEntree = RadPatientGridView.Rows(aRow).Cells("oa_patient_date_entree_oasis").Value
            End If

            If DateSortie < Date.Now() Then
                LblDateSortie.Text = DateSortie.ToString("dd.MM.yyyy")
                LblLabelDateSortie.Show()
                LblDateSortie.Show()
                LblPatientSorti.Text = "Attention, ce patient est sorti du dispositif Oasis"
                LblPatientSorti.Show()
            Else
                LblLabelDateSortie.Hide()
                LblDateSortie.Hide()
                LblPatientSorti.Hide()
            End If

            If DateEntree > Date.Now() Then
                LblPatientSorti.Text = "Attention, ce patient ne fait pas partie du dispositif Oasis"
                LblPatientSorti.Show()
            End If

            If aldDao.IsPatientALD(TxtIdSelected.Text) Then
                LblPatientALD.Show()
                Dim StringTooltip As String
                Dim aldDao As New AldDao
                StringTooltip = aldDao.DateFinALD(TxtIdSelected.Text)
                If StringTooltip <> "" Then
                    ToolTip.SetToolTip(LblPatientALD, StringTooltip)
                End If
            Else
                LblPatientALD.Hide()
            End If

            TxtIdSelected.Show()
            TxtNirSelected.Show()
            TxtPrenomSelected.Show()
            TxtNomSelected.Show()
            RadPnlSelectedPatient.Show()
            RadBtnDetailPatient.Show()
            RadBtnSynthese.Show()
            RadBtnRendezVous.Show()
            RadBtnEpisode.Show()

            TxtSite.Text = RadPatientGridView.Rows(aRow).Cells("site").Value
            TxtSite.Show()

            If TxtSite.Text = "" Then
                RadBtnSynthese.Hide()
                RadBtnRendezVous.Hide()
                RadBtnEpisode.Hide()
                'InitZonesSelectionPatient()
                MessageBox.Show("Options limitées, ce patient n'a pas de site d'affecté !")
            Else
                Dim episodeDao As New EpisodeDao
                Dim episode As Episode
                Dim patientId As Integer = CInt(TxtIdSelected.Text)
                episode = episodeDao.GetEpisodeEnCoursByPatientId(patientId)
                Dim BtnColor As New Color
                BtnColor = RadBtnEpisode.BackColor
                If episode.Id <> 0 Then
                    RadBtnEpisode.ForeColor = Color.Red
                    RadBtnEpisode.Font = New Font(RadBtnEpisode.Font, FontStyle.Bold)
                    Dim TypeActiviteEpisode As String
                    TypeActiviteEpisode = episodeDao.GetItemTypeActiviteByCode(episode.TypeActivite)
                    ToolTip.SetToolTip(RadBtnEpisode, "Un épisode de type : " & episode.Type & " " & TypeActiviteEpisode & " est en cours pour ce patient" &
                                           vbCrLf & "Episode créé par un profil : " & episode.TypeProfil)
                Else
                    RadBtnEpisode.ForeColor = Color.Black
                    RadBtnEpisode.Font = New Font(RadBtnEpisode.Font, FontStyle.Regular)
                    ToolTip.SetToolTip(RadBtnEpisode, "Ce patient n'a pas d'épisode en cours")
                End If
            End If

            'Gestion des habilitations
            If Not (userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString OrElse userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString) Then
                RadBtnEpisode.Hide()
                RadBtnLigneDeVie.Hide()
                RadBtnSynthese.Hide()
                If Not (userLog.TypeProfil = ProfilDao.EnumProfilType.ACCUEIL.ToString) Then
                    RadBtnRendezVous.Hide()
                End If
            End If
        Else
            InitZonesSelectionPatient()
        End If
    End Sub


    Private Sub RadPatientGridView_DoubleClick(sender As Object, e As EventArgs) Handles RadPatientGridView.DoubleClick
        SelectionPatient()
        If userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString OrElse userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
            Synthese()
        End If
    End Sub

    'Gestion de l'affichage des zones d'écran
    Private Sub InitZonesSelectionPatient()
        'Initialisation des TextBox
        TxtIdSelected.Text = ""
        TxtNirSelected.Text = ""
        TxtPrenomSelected.Text = ""
        TxtNomSelected.Text = ""
        TxtSite.Text = ""

        'Cacher les TextBox
        TxtIdSelected.Hide()
        TxtNirSelected.Hide()
        TxtPrenomSelected.Hide()
        TxtNomSelected.Hide()
        TxtSite.Hide()
        LblPatientALD.Hide()
        LblDateNaissanceSelected.Hide()
        LblAgeSelected.Hide()
        RadPnlSelectedPatient.Hide()

        'Cacher les boutons d'appel
        RadBtnDetailPatient.Hide()
        RadBtnSynthese.Hide()
        RadBtnEpisode.Hide()
    End Sub

    Private Sub InitHabilitation()
        'Accès au menu Admin si l'utilisateur est autorisé
        If userLog.UtilisateurAdmin = False Then
            RadBtnAdmin.Hide()
            RadBtnRdvEnCours.Hide()
            RadBtnIntervenantSansRdv.Hide()
        End If

        'Gestion des habilitations
        If Not (userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString OrElse
            userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString) Then
            RadBtnEpisodeEnCours.Hide()
            RadBtnListeAction.Hide()
        End If

        If Not (userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString OrElse
            userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString OrElse
            userLog.TypeProfil = ProfilDao.EnumProfilType.ACCUEIL.ToString) Then
            RadBtnCreatePatient.Hide()
        End If
    End Sub


    Private Sub RadBtnCreatePatient_Click(sender As Object, e As EventArgs) Handles RadBtnCreatePatient.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False

        Try
            Using form As New RadFPatientDetailEdit
                Me.SelectedPatient = patientDao.GetPatient(0)
                form.SelectedPatientId = 0
                form.UtilisateurConnecte = userLog
                form.SelectedPatient = Me.SelectedPatient
                form.ShowDialog() 'Modal
                'Si le patient a été créé, on recharge la grid
                If form.CodeRetour = True Then
                    InitZonesSelectionPatient()
                    ChargementPatient()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Me.Enabled = True
    End Sub

    Private Sub RadBtnAdmin_Click(sender As Object, e As EventArgs) Handles RadBtnAdmin.Click
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False

        Try
            Using vFMenuAdmin As New FrmMain
                Me.SelectedPatient = patientDao.GetPatient(0)
                vFMenuAdmin.ShowDialog() 'Modal
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Me.Enabled = True
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub RadBtnDetailPatient_Click(sender As Object, e As EventArgs) Handles RadBtnDetailPatient.Click
        If TxtIdSelected.Text <> "" Then
            'Initialisation du patient sélectionné
            Dim patientId As Integer = CInt(TxtIdSelected.Text)
            Me.SelectedPatient = patientDao.GetPatient(patientId)
            Cursor.Current = Cursors.WaitCursor
            Me.Enabled = False

            Try
                Using form As New RadFPatientDetailEdit
                    form.SelectedPatientId = patientId
                    form.SelectedPatient = Me.SelectedPatient
                    form.UtilisateurConnecte = userLog
                    form.ShowDialog()
                    'Si le patient a été modifié, on recharge la grid
                    If form.CodeRetour = True Then
                        InitZonesSelectionPatient()
                        ChargementPatient()
                    End If
                End Using
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

            Me.Enabled = True
        Else
            MessageBox.Show("Vous devez sélectionner un patient pour lancer cette option")
        End If
    End Sub

    'Création des intervenants par défaut
    Private Sub CréerLesIntervenantsOasisParDéfautToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerLesIntervenantsOasisParDéfautToolStripMenuItem.Click
        If RadPatientGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadPatientGridView.Rows.IndexOf(Me.RadPatientGridView.CurrentRow)
            If aRow >= 0 Then
                Dim patientId As Integer = RadPatientGridView.Rows(aRow).Cells("oa_patient_id").Value
                Dim parcoursDao As New ParcoursDao
                parcoursDao.CreateIntervenantOasisByPatient(patientId, userLog)
            End If
        End If
    End Sub

    Private Sub RadBtnSynthese_Click(sender As Object, e As EventArgs) Handles RadBtnSynthese.Click
        Synthese()
    End Sub

    Private Sub Synthese()
        If TxtIdSelected.Text <> "" Then
            'Création instance patient
            Dim patientId As Integer = CInt(TxtIdSelected.Text)
            Cursor.Current = Cursors.WaitCursor
            Me.Enabled = False

            Try
                Using form As New RadFSynthese
                    'patientDao.SetPatient(Me.SelectedPatient, patientId)
                    Me.SelectedPatient = patientDao.GetPatient(patientId)
                    form.SelectedPatient = Me.SelectedPatient
                    form.UtilisateurConnecte = userLog
                    form.EcranPrecedent = EnumAccesEcranPrecedent.SANS
                    form.ShowDialog()
                End Using
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

            Me.Enabled = True
        Else
            MessageBox.Show("Vous devez sélectionner un patient pour lancer cette option")
        End If
    End Sub

    Private Sub RadPatientGridView_CellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs) Handles RadPatientGridView.CellFormatting
        Dim columnName As String = e.Column.Name
        Dim value As Object = e.Row.Cells(columnName).Value

        If columnName = "oa_patient_date_entree_oasis" Then
            If value IsNot DBNull.Value Then
                If value = "31/12/9998" Then
                    e.CellElement.Text = ""
                End If
            End If
        End If

        If columnName = "oa_patient_date_sortie_oasis" Then
            If value IsNot DBNull.Value Then
                If value = "31/12/9998" Then
                    e.CellElement.Text = ""
                End If
            End If
        End If
    End Sub

    Private Sub RadChkPatientOasis_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkPatientOasis.ToggleStateChanged
        If RadChkPatientOasis.CheckState = CheckState.Checked Then
            Application.DoEvents()
            Cursor.Current = Cursors.WaitCursor
            'ChargementPatient()
            InitZonesSelectionPatient()
            Cursor.Current = Cursors.Default
        End If
    End Sub

    Private Sub RadChkPatientNonOasis_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkPatientNonOasis.ToggleStateChanged
        If RadChkPatientNonOasis.CheckState = CheckState.Checked Then
            Application.DoEvents()
            Cursor.Current = Cursors.WaitCursor
            'ChargementPatient()
            InitZonesSelectionPatient()
            Cursor.Current = Cursors.Default
        End If
    End Sub

    Private Sub RadChkPatientTous_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkPatientTous.ToggleStateChanged
        If RadChkPatientTous.CheckState = CheckState.Checked Then
            Application.DoEvents()
            Cursor.Current = Cursors.WaitCursor
            'ChargementPatient()
            InitZonesSelectionPatient()
            Cursor.Current = Cursors.Default
        End If
    End Sub

    Private Sub RadButtonAbandon_Click(sender As Object, e As EventArgs) Handles RadButtonAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnEpisode_Click(sender As Object, e As EventArgs) Handles RadBtnEpisode.Click
        Dim episodeDao As New EpisodeDao
        Dim episode As Episode
        If TxtIdSelected.Text <> "" Then
            'Initialisation du patient sélectionné
            Dim patientId As Integer = CInt(TxtIdSelected.Text)
            Me.SelectedPatient = patientDao.GetPatient(patientId)
            Cursor.Current = Cursors.WaitCursor
            Me.Enabled = False
            EpisodeUtils.CallEpisode(SelectedPatient, 0, userLog, EnumAccesEcranPrecedent.SANS)
            Me.Enabled = True
            episode = episodeDao.GetEpisodeEnCoursByPatientId(Me.SelectedPatient.PatientId)
            If episode.Id <> 0 Then
                RadBtnEpisode.ForeColor = Color.Red
                RadBtnEpisode.Font = New Font(RadBtnEpisode.Font, FontStyle.Bold)
                Dim TypeActiviteEpisode As String
                TypeActiviteEpisode = episodeDao.GetItemTypeActiviteByCode(episode.TypeActivite)
                ToolTip.SetToolTip(RadBtnEpisode, "Un épisode de type : " & episode.Type & " " & TypeActiviteEpisode &
                                   vbCrLf & "Créé par un profil : " & episode.TypeProfil & " est en cours pour ce patient")
            Else
                RadBtnEpisode.ForeColor = Color.FromArgb(21, 66, 139)
                RadBtnEpisode.Font = New Font(RadBtnEpisode.Font, FontStyle.Regular)
                ToolTip.SetToolTip(RadBtnEpisode, "")
            End If
        End If
    End Sub

    Private Sub RadBtnRendezVous_Click(sender As Object, e As EventArgs) Handles RadBtnRendezVous.Click
        If TxtIdSelected.Text <> "" Then
            Dim patientId As Integer = CInt(TxtIdSelected.Text)
            Cursor.Current = Cursors.WaitCursor
            Me.Enabled = False

            Try
                Using form As New RadFPatientRendezVousListe
                    Me.SelectedPatient = patientDao.GetPatient(patientId)
                    form.SelectedPatient = Me.SelectedPatient
                    form.ShowDialog() 'Modal
                End Using
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try

            Me.Enabled = True
        Else
            MessageBox.Show("Vous devez sélectionner un patient pour lancer cette option")
        End If
    End Sub

    Private Sub RadBtnListeAction_Click(sender As Object, e As EventArgs) Handles RadBtnListeAction.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False

        Try
            Using form As New RadFListeActions
                form.UserId = userLog.UtilisateurId
                form.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Me.Enabled = True
    End Sub

    Private Sub RadBtnLigneDeVie_Click(sender As Object, e As EventArgs) Handles RadBtnLigneDeVie.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Dim patientId As Integer = CInt(TxtIdSelected.Text)
        Using vadFEpisodeListe As New RadFEpisodeLigneDeVie
            Me.SelectedPatient = patientDao.GetPatient(patientId)
            vadFEpisodeListe.SelectedPatient = Me.SelectedPatient
            vadFEpisodeListe.UtilisateurConnecte = userLog
            vadFEpisodeListe.EcranPrecedent = EnumAccesEcranPrecedent.SANS
            vadFEpisodeListe.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadButton1_Click(sender As Object, e As EventArgs) Handles RadButton1.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False

        Try
            Using form As New RadFEpisodeEnAttenteValidation
                form.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Me.Enabled = True
    End Sub

    Private Sub RadBtnTache_Click(sender As Object, e As EventArgs) Handles RadBtnTache.Click
        Cursor.Current = Cursors.WaitCursor

        Try
            Me.Enabled = False
            Using form As New FrmTacheMain
                form.ShowDialog()
            End Using
        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub RadBtnTacheEnCours_Click(sender As Object, e As EventArgs) Handles RadBtnRdvEnCours.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False

        Try
            Using form As New RadFListeRendezVousEnCours
                form.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Me.Enabled = True
    End Sub

    Private Sub RadBtnIntervenantSansRdv_Click(sender As Object, e As EventArgs) Handles RadBtnIntervenantSansRdv.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False

        Try
            Using form As New RadFListeIntervenantSansRDV
                form.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Me.Enabled = True
    End Sub

    Private Sub RadBtnEpisodeEnCours_Click(sender As Object, e As EventArgs) Handles RadBtnEpisodeEnCours.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False

        Try
            Using form As New RadFEpisodeEnCoursListe
                form.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Me.Enabled = True
    End Sub

    Private Sub RadBtnWiki_Click(sender As Object, e As EventArgs) Handles RadBtnWiki.Click
        'Récupération de l'URL du WiKi dans les paramètres de l'application
        Dim UriProcedureTutorielle As String = ConfigurationManager.AppSettings("UriProcedureTutorielle")
        If UriProcedureTutorielle = "" Then
            CreateLog("Paramètre application 'UriProcedureTutorielle' non trouvé !", "Procédure tutorielle", Log.EnumTypeLog.ERREUR.ToString, userLog)
            UriProcedureTutorielle = "http://173.199.71.187/doku.php?id="
        End If

        Dim MonURL As String
        MonURL = UriProcedureTutorielle
        'Process.Start(MonURL)
        Try
            Using form As New RadFWebBrowser
                form.Url = MonURL
                form.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub RadMailButton_Click(sender As Object, e As EventArgs) Handles RadMailButton.Click
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False

        Try
            Using frm As New FrmSousEpisodeReponseAttribution
                'Me.SelectedPatient = patientDao.GetPatient(0)
                frm.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Me.Enabled = True
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub BtnVaccin_Click(sender As Object, e As EventArgs) Handles BtnVaccin.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        If TxtIdSelected.Text <> "" Then
            Dim patientId As Integer = CInt(TxtIdSelected.Text)
            Me.SelectedPatient = patientDao.GetPatient(patientId)

            Using radFCPV As New RadFCPV
                radFCPV.Patient = SelectedPatient
                radFCPV.ShowDialog()
            End Using
        End If

        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub BtnRequest_Click(sender As Object, e As EventArgs) Handles BtnRequest.Click
        Try
            Using form As New RadFPatientRequestSelector
                form.ShowDialog() 'Modal
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    Private Sub BtnSearch_Click(sender As Object, e As EventArgs) Handles BtnSearch.Click
        ChargementPatient()
    End Sub

    Private Sub RadButton5_Click(sender As Object, e As EventArgs) Handles RadButton5.Click
        DTPDDN.Value = DTPDDN.MinDate
    End Sub

    Private Sub RadButton3_Click(sender As Object, e As EventArgs) Handles BtnFilter.Click
        ShowFilter()
    End Sub

    Private Sub RadButton3_Hover(sender As Object, e As EventArgs) Handles BtnFilter.MouseHover
        TTSites.Show(filterTache.ResumeFiltre(), sender, 5000)
    End Sub

    Private Sub InputPrenom_TextChanged(sender As Object, e As EventArgs) Handles InputPrenom.Validated
        If InputPrenom.Text.Length > 0 AndAlso InputPrenom.Text.Length < 3 Then
            TTValidation.Show("Le champ de recherche prenom doit contenir au moins 3 characteres. (Utiliser un espace comme charactere échappatoire)", sender, 5000)
        End If
    End Sub

    Private Sub InputNom_TextChanged(sender As Object, e As EventArgs) Handles InputNom.Validated
        If InputNom.Text.Length > 0 AndAlso InputNom.Text.Length < 3 Then
            TTValidation.Show("Le champ de recherche nom doit contenir au moins 3 characteres. (Utiliser un espace comme charactere échappatoire)", sender, 5000)
        End If
    End Sub

    Private Sub RadButton6_Click(sender As Object, e As EventArgs) Handles RadButton6.Click
        filterTache.Clear()
        BtnFilter.ForeColor = Nothing
    End Sub
End Class