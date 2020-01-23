Imports Telerik.WinControls
Imports Telerik.WinControls.UI.Localization
Imports Oasis_Common
Public Class RadFPatientListe
    Private privateUtilisateurConnecte As Utilisateur
    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property

    Dim aldDao As New AldDao

    'Instanciation du patient pour le fournir aux Forms qui seront appelées depuis cette Form
    Dim SelectedPatient As New Patient
    Dim IndexGrid As Integer
    Dim Tous, PatientOasis As Boolean

    Private Sub RadFPatientListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        afficheTitleForm(Me, "Liste des patients")

        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.TextAndButtonsElement.TextElement.ForeColor = Color.Red
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.BackColor = Color.DarkBlue
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.Font = New Font("Arial", 8.0F, FontStyle.Italic)
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.TextImageRelation = TextImageRelation.TextBeforeImage
        Me.RadDesktopAlert1.Popup.AlertElement.BackColor = Color.MistyRose
        Me.RadDesktopAlert1.Popup.AlertElement.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.BorderColor = Color.DarkBlue

        'Traitement des contextes obsolètes
        Dim parametreOasisDao As ParametreOasisDao = New ParametreOasisDao
        parametreOasisDao.TraitementContexte()

        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()

        RadChkPatientOasis.CheckState = CheckState.Checked

        'Accès au menu Admin si l'utilisateur est autorisé
        If UtilisateurConnecte.UtilisateurAdmin = False Then
            RadBtnAdmin.Hide()
        End If

        InitZonesSelectionPatient()

        Cursor.Current = Cursors.Default
    End Sub


    Private Sub ChargementPatient()
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

        patientDataTable = PatientDao.GetAllPatient(Tous, PatientOasis)

        Dim iGrid As Integer = -1
        Dim rowCount As Integer = patientDataTable.Rows.Count - 1
        LblOccurrenceLue.Text = patientDataTable.Rows.Count & " occurrence(s) lue(s)"
        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadPatientGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
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
                RadPatientGridView.Rows(iGrid).Cells("age").Value = outils.CalculAgeString(DateNaissance)
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


    Private Sub RadGridView1_CellClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadPatientGridView.CellClick
        Dim aRow As Integer
        Dim maxRow As Integer
        Dim DateSortie, DateEntree, dateNaissance As Date
        Dim DateIllimite As Date = "31/12/2999"

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
                LblAgeSelected.Text = outils.CalculAgeString(dateNaissance)
                LblAgeSelected.Show()
            Else
                LblDateNaissanceSelected.Hide()
                LblAgeSelected.Hide()
            End If

            DateSortie = DateIllimite
            If RadPatientGridView.Rows(aRow).Cells("oa_patient_date_sortie_oasis").Value <> "" Then
                DateSortie = Coalesce(RadPatientGridView.Rows(aRow).Cells("oa_patient_date_sortie_oasis").Value, Nothing)
            End If

            DateEntree = DateIllimite
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
                        ToolTip.SetToolTip(RadBtnEpisode, "Un épisode de type : " & episode.Type & " " & TypeActiviteEpisode & " est en cours pour ce patient")
                    Else
                        RadBtnEpisode.ForeColor = Color.Black
                        RadBtnEpisode.Font = New Font(RadBtnEpisode.Font, FontStyle.Regular)
                        ToolTip.SetToolTip(RadBtnEpisode, "Ce patient n'a pas d'épisode en cours")
                    End If
                End If
                IndexGrid = aRow
            Else
                InitZonesSelectionPatient()
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

    Private Sub RadBtnCreatePatient_Click(sender As Object, e As EventArgs) Handles RadBtnCreatePatient.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using form As New RadFPatientDetailEdit
            PatientDao.SetPatient(Me.SelectedPatient, 0)
            form.SelectedPatientId = 0
            form.UtilisateurConnecte = Me.UtilisateurConnecte
            form.SelectedPatient = Me.SelectedPatient
            form.ShowDialog() 'Modal
            'Si le patient a été créé, on recharge la grid
            If form.CodeRetour = True Then
                InitZonesSelectionPatient()
                ChargementPatient()
                Me.RadDesktopAlert1.CaptionText = "Notification patient"
                Me.RadDesktopAlert1.ContentText = "Patient créé"
                Me.RadDesktopAlert1.Show()
            End If
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnAdmin_Click(sender As Object, e As EventArgs) Handles RadBtnAdmin.Click
        Me.Cursor = Cursors.WaitCursor
        Me.Enabled = False
        Using vFMenuAdmin As New FrmMain
            PatientDao.SetPatient(Me.SelectedPatient, 0)
            vFMenuAdmin.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
        Me.Cursor = Cursors.Default
    End Sub

    Private Sub RadBtnDetailPatient_Click(sender As Object, e As EventArgs) Handles RadBtnDetailPatient.Click
        If TxtIdSelected.Text <> "" Then
            'Initialisation du patient sélectionné
            Dim patientId As Integer = CInt(TxtIdSelected.Text)
            PatientDao.SetPatient(Me.SelectedPatient, patientId)
            Cursor.Current = Cursors.WaitCursor
            Me.Enabled = False
            Using form As New RadFPatientDetailEdit
                form.SelectedPatientId = patientId
                form.SelectedPatient = Me.SelectedPatient
                form.UtilisateurConnecte = Me.UtilisateurConnecte
                form.ShowDialog()
                'Si le patient a été modifié, on recharge la grid
                If form.CodeRetour = True Then
                    InitZonesSelectionPatient()
                    ChargementPatient()
                    Me.RadDesktopAlert1.CaptionText = "Notification du patient"
                    Me.RadDesktopAlert1.ContentText = "Patient modifié"
                    Me.RadDesktopAlert1.Show()
                End If
            End Using
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
                parcoursDao.CreateIntervenantOasisByPatient(patientId)
            End If
        End If
    End Sub

    Private Sub RadBtnSynthese_Click(sender As Object, e As EventArgs) Handles RadBtnSynthese.Click
        If TxtIdSelected.Text <> "" Then
            'Création instance patient
            Dim patientId As Integer = CInt(TxtIdSelected.Text)
            Cursor.Current = Cursors.WaitCursor
            Me.Enabled = False
            Using form As New RadFSynthese
                PatientDao.SetPatient(Me.SelectedPatient, patientId)
                form.SelectedPatient = Me.SelectedPatient
                form.UtilisateurConnecte = Me.UtilisateurConnecte
                form.ShowDialog()
            End Using
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
            ChargementPatient()
            InitZonesSelectionPatient()
            Cursor.Current = Cursors.Default
        End If
    End Sub

    Private Sub RadChkPatientNonOasis_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkPatientNonOasis.ToggleStateChanged
        If RadChkPatientNonOasis.CheckState = CheckState.Checked Then
            Application.DoEvents()
            Cursor.Current = Cursors.WaitCursor
            ChargementPatient()
            InitZonesSelectionPatient()
            Cursor.Current = Cursors.Default
        End If
    End Sub

    Private Sub RadChkPatientTous_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkPatientTous.ToggleStateChanged
        If RadChkPatientTous.CheckState = CheckState.Checked Then
            Application.DoEvents()
            Cursor.Current = Cursors.WaitCursor
            ChargementPatient()
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
            PatientDao.SetPatient(Me.SelectedPatient, patientId)
            Cursor.Current = Cursors.WaitCursor
            Me.Enabled = False
            episodeDao.CallEpisode(SelectedPatient, 0)
            Me.Enabled = True
            episode = episodeDao.GetEpisodeEnCoursByPatientId(Me.SelectedPatient.patientId)
            If episode.Id <> 0 Then
                RadBtnEpisode.ForeColor = Color.Red
                RadBtnEpisode.Font = New Font(RadBtnEpisode.Font, FontStyle.Bold)
                Dim TypeActiviteEpisode As String
                TypeActiviteEpisode = episodeDao.GetItemTypeActiviteByCode(episode.TypeActivite)
                ToolTip.SetToolTip(RadBtnEpisode, "Un épisode de type : " & episode.Type & " " & TypeActiviteEpisode & " est en cours pour ce patient")
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
            Using form As New RadFPatientRendez_vousListe
                PatientDao.SetPatient(Me.SelectedPatient, patientId)
                form.SelectedPatient = Me.SelectedPatient
                form.ShowDialog() 'Modal
            End Using
            Me.Enabled = True
        Else
            MessageBox.Show("Vous devez sélectionner un patient pour lancer cette option")
        End If
    End Sub

    Private Sub RadBtnListeAction_Click(sender As Object, e As EventArgs) Handles RadBtnListeAction.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using form As New RadFListeActions
            form.UserId = userLog.UtilisateurId
            form.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnEpisodeEnCours_Click(sender As Object, e As EventArgs) Handles RadBtnEpisodeEnCours.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using form As New RadFEpisodeEnCoursListe
            form.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

End Class
