Imports Telerik.WinControls.UI
Imports Oasis_Common
Public Class RadFPatientNoteListe
    Private _typeNote As Integer
    Private privateSelectedPatientId As Integer
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateCodeRetour As Boolean

    Public Property SelectedPatientId As Integer
        Get
            Return privateSelectedPatientId
        End Get
        Set(value As Integer)
            privateSelectedPatientId = value
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

    Public Property CodeRetour As Boolean
        Get
            Return privateCodeRetour
        End Get
        Set(value As Boolean)
            privateCodeRetour = value
        End Set
    End Property

    Public Property SelectedPatient As Patient
        Get
            Return privateSelectedPatient
        End Get
        Set(value As Patient)
            privateSelectedPatient = value
        End Set
    End Property

    Public Property TypeNote As Integer
        Get
            Return _typeNote
        End Get
        Set(value As Integer)
            _typeNote = value
        End Set
    End Property

    Enum EnumEditMode
        Creation = 1
        Modification = 2
    End Enum

    Dim EditMode As Integer
    Dim utilisateurHisto As Utilisateur = New Utilisateur()
    Dim uniteSanitaireListe As Dictionary(Of Integer, String) = Table_unite_sanitaire.GetUniteSanitaireListe()
    'Dim siteListe As Dictionary(Of Integer, String) = Table_site.GetSiteListe()
    Dim genreListe As Dictionary(Of String, String) = Table_genre.GetGenreListe()

    Private Sub RadFPatientNoteMedicalListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If SelectedPatientId <> 0 Then
            Select Case TypeNote
                Case EnumTypeNote.Medicale
                    Me.Text = "Note(s) médicale(s) du patient"
                Case EnumTypeNote.Vaccin
                    Me.Text = "Note(s) de vaccination du patient"
                Case EnumTypeNote.Social
                    Me.Text = "Note(s) du contexte social du patient"
                Case EnumTypeNote.Directive
                    Me.Text = "Directives anticipées"
                Case Else
                    Close()
            End Select
            ChargementPatient()
            ChargementnotePatient()
        End If
    End Sub

    'Chargement de la Grid Notes patient
    Private Sub ChargementnotePatient()
        Dim NotePatientDataTable As New DataTable

        Select Case TypeNote
            Case EnumTypeNote.Medicale
                Dim patientNoteMedicaleDao As PatientNoteMedicaleDao = New PatientNoteMedicaleDao()
                NotePatientDataTable = patientNoteMedicaleDao.getAllNoteMedicalebyPatient(SelectedPatient.patientId)
            Case EnumTypeNote.Vaccin
                Dim patientNoteVaccinDao As PatientNoteVaccinDao = New PatientNoteVaccinDao()
                NotePatientDataTable = patientNoteVaccinDao.getAllNoteVaccinbyPatient(SelectedPatient.patientId)
            Case EnumTypeNote.Social
                Dim patientNoteSocialeDao As PatientNoteSocialeDao = New PatientNoteSocialeDao()
                NotePatientDataTable = patientNoteSocialeDao.getAllNoteSocialebyPatient(SelectedPatient.patientId)
            Case EnumTypeNote.Directive
                Dim patientNoteDirectiveDao As PatientNoteDirectiveDao = New PatientNoteDirectiveDao()
                NotePatientDataTable = patientNoteDirectiveDao.getAllNoteDirectivebyPatient(SelectedPatient.patientId)
            Case Else
                Close()
        End Select

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateCreation As Date
        Dim AfficheDateCreation, NotePatient, Auteur As String
        Dim AuteurId As Integer
        Dim rowCount As Integer = NotePatientDataTable.Rows.Count - 1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            If NotePatientDataTable.Rows(i)("oa_patient_note") IsNot DBNull.Value Then
                NotePatient = NotePatientDataTable.Rows(i)("oa_patient_note")
            Else
                NotePatient = ""
            End If

            'Utilisateur creation
            Auteur = ""
            If NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation") IsNot DBNull.Value Then
                If NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation") <> 0 Then
                    Dim UtilisateurCreation As Utilisateur
                    Dim userDao As New UserDao
                    UtilisateurCreation = userDao.GetUserById(NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation"))
                    'SetUtilisateur(utilisateurHisto, NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation"))
                    Auteur = UtilisateurCreation.UtilisateurPrenom & " " & UtilisateurCreation.UtilisateurNom
                End If
            End If

            'Date création
            AfficheDateCreation = ""
            If NotePatientDataTable.Rows(i)("oa_patient_note_date_creation") IsNot DBNull.Value Then
                dateCreation = NotePatientDataTable.Rows(i)("oa_patient_note_date_creation")
                'AfficheDateCreation = outils.FormatageDateAffichage(dateCreation)
                AfficheDateCreation = dateCreation.ToString("dd-MM-yyyy")
            Else
                If NotePatientDataTable.Rows(i)("oa_patient_note_date_creation") IsNot DBNull.Value Then
                    dateCreation = NotePatientDataTable.Rows(i)("oa_patient_note_date_creation")
                    'AfficheDateCreation = outils.FormatageDateAffichage(dateCreation)
                    AfficheDateCreation = dateCreation.ToString("dd-MM-yyyy")
                End If
            End If

            AuteurId = 0
            If NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation") IsNot DBNull.Value Then
                AuteurId = NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation")
            End If

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadNotePatientDataGridView.Rows.Add(iGrid)
            RadNotePatientDataGridView.Rows(iGrid).Height = 40

            'Alimentation du DataGridView
            RadNotePatientDataGridView.Rows(iGrid).Cells("note").Value = NotePatient

            'Identifiant notePatient
            RadNotePatientDataGridView.Rows(iGrid).Cells("noteId").Value = NotePatientDataTable.Rows(i)("oa_patient_note_id")

            'Auteur de la note
            RadNotePatientDataGridView.Rows(iGrid).Cells("auteur").Value = Auteur & vbCrLf & AfficheDateCreation
        Next

        'Positionnement du grid sur la première occurrence
        If RadNotePatientDataGridView.Rows.Count > 0 Then
            Me.RadNotePatientDataGridView.CurrentRow = RadNotePatientDataGridView.ChildRows(0)
        End If
    End Sub

    Private Sub ChargementPatient()
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

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    'Appel détail note en modification
    Private Sub RadNotePatientDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadNotePatientDataGridView.CellDoubleClick

        If RadNotePatientDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadNotePatientDataGridView.Rows.IndexOf(Me.RadNotePatientDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim NoteId As Integer = RadNotePatientDataGridView.Rows(aRow).Cells("noteId").Value

                Try
                    Using vFPatientNoteDetailEdit As New RadFPatientNoteDetailEdit
                        vFPatientNoteDetailEdit.SelectedNoteId = NoteId
                        vFPatientNoteDetailEdit.SelectedPatient = Me.SelectedPatient
                        vFPatientNoteDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                        vFPatientNoteDetailEdit.TypeNote = Me.TypeNote
                        vFPatientNoteDetailEdit.ShowDialog() 'Modal
                        If vFPatientNoteDetailEdit.CodeRetour = True Then
                            RadNotePatientDataGridView.Rows.Clear()
                            ChargementnotePatient()
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try

            End If
        End If
    End Sub

    'Appel détail note en création
    Private Sub RadBtnCreation_Click(sender As Object, e As EventArgs) Handles RadBtnCreation.Click
        CreationNote()
    End Sub

    Private Sub CréerUneNoteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneNoteToolStripMenuItem.Click
        CreationNote()
    End Sub
    Private Sub CreationNote()

        Try
            Using vFPatientNoteDetailEdit As New RadFPatientNoteDetailEdit
                vFPatientNoteDetailEdit.SelectedNoteId = 0
                vFPatientNoteDetailEdit.SelectedPatient = Me.SelectedPatient
                vFPatientNoteDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                vFPatientNoteDetailEdit.TypeNote = Me.TypeNote
                vFPatientNoteDetailEdit.ShowDialog() 'Modal
                If vFPatientNoteDetailEdit.CodeRetour = True Then
                    RadNotePatientDataGridView.Rows.Clear()
                    ChargementnotePatient()
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Close()
    End Sub

    Private Sub RadNotePatientDataGridView_CellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs) Handles RadNotePatientDataGridView.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub
End Class
