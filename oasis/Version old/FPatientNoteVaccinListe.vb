Imports System.Data.SqlClient

Public Class FPatientNoteVaccinListe
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

    Enum EnumEditMode
        Creation = 1
        Modification = 2
    End Enum

    Dim EditMode As Integer
    Dim utilisateurHisto As Utilisateur = New Utilisateur()
    Dim conxn As New SqlConnection(getConnectionString())
    Dim uniteSanitaireListe As Dictionary(Of Integer, String) = Table_unite_sanitaire.GetUniteSanitaireListe()
    'Dim siteListe As Dictionary(Of Integer, String) = Table_site.GetSiteListe()
    Dim genreListe As Dictionary(Of String, String) = Table_genre.GetGenreListe()

    Private Sub FPatientDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If SelectedPatientId <> 0 Then
            ChargementPatient()
            ChargementnotePatient()
        End If
    End Sub

    'Chargement de la Grid Notes patient
    Private Sub ChargementnotePatient()
        'Déclaration des données de connexion
        Dim conxn As New SqlConnection(getConnectionString())
        Dim notePatientDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim NotePatientDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select * from oasis.oa_patient_note_vaccin where (oa_patient_note_invalide = '0' or oa_patient_note_invalide is Null) and oa_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_patient_note_date_creation desc;"

        'Lecture des données en base
        notePatientDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        notePatientDataAdapter.Fill(NotePatientDataTable)
        conxn.Open()

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
                    Dim UtilisateurCreation = New Utilisateur()
                    SetUtilisateur(utilisateurHisto, NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation"))
                    Auteur = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
                End If
            End If

            'Date création
            AfficheDateCreation = ""
            If NotePatientDataTable.Rows(i)("oa_patient_note_date_creation") IsNot DBNull.Value Then
                dateCreation = NotePatientDataTable.Rows(i)("oa_patient_note_date_creation")
                AfficheDateCreation = outils.FormatageDateAffichage(dateCreation)
            Else
                If NotePatientDataTable.Rows(i)("oa_patient_note_date_creation") IsNot DBNull.Value Then
                    dateCreation = NotePatientDataTable.Rows(i)("oa_patient_note_date_creation")
                    AfficheDateCreation = outils.FormatageDateAffichage(dateCreation)
                End If
            End If

            AuteurId = 0
            If NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation") IsNot DBNull.Value Then
                AuteurId = NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation")
            End If

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            NotePatientDataGridView.RowTemplate.Height = 40
            NotePatientDataGridView.Rows.Insert(iGrid)

            'Alimentation du DataGridView
            NotePatientDataGridView("note", iGrid).Value = NotePatient

            'Identifiant notePatient
            NotePatientDataGridView("noteId", iGrid).Value = NotePatientDataTable.Rows(i)("oa_patient_note_id")

            'Auteur de la note
            NotePatientDataGridView("auteur", iGrid).Value = Auteur & vbCrLf & AfficheDateCreation
        Next
        conxn.Close()
        notePatientDataAdapter.Dispose()

        'Enlève le focus sur la première ligne de la Grid
        NotePatientDataGridView.ClearSelection()
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
    End Sub

    'Retour écran précédent sans action
    Private Sub BtnAbandonner_Click(sender As Object, e As EventArgs) Handles BtnAbandonner.Click
        Me.CodeRetour = False
        Close()
    End Sub

    'Appel détail note en modification
    Private Sub NotePatientDataGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles NotePatientDataGridView.CellDoubleClick
        If NotePatientDataGridView.CurrentRow IsNot Nothing Then
            Dim NoteId As Integer = NotePatientDataGridView.Rows(NotePatientDataGridView.CurrentRow.Index).Cells("noteId").Value

            Dim vFPatientNoteVaccinDetailEdit As New FPatientNoteVaccinDetailEdit
            vFPatientNoteVaccinDetailEdit.SelectedNoteId = NoteId
            vFPatientNoteVaccinDetailEdit.SelectedPatient = Me.SelectedPatient
            vFPatientNoteVaccinDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte

            vFPatientNoteVaccinDetailEdit.ShowDialog() 'Modal
            If vFPatientNoteVaccinDetailEdit.CodeRetour = True Then
                NotePatientDataGridView.Rows.Clear()
                ChargementnotePatient()
            End If

            vFPatientNoteVaccinDetailEdit.Dispose()
        End If
    End Sub

    'Appel détail note en création
    Private Sub CréerUneNoteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneNoteToolStripMenuItem.Click
        Dim vFPatientNoteVaccinDetailEdit As New FPatientNoteVaccinDetailEdit
        vFPatientNoteVaccinDetailEdit.SelectedNoteId = 0
        vFPatientNoteVaccinDetailEdit.SelectedPatient = Me.SelectedPatient
        vFPatientNoteVaccinDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte

        vFPatientNoteVaccinDetailEdit.ShowDialog() 'Modal
        If vFPatientNoteVaccinDetailEdit.CodeRetour = True Then
            NotePatientDataGridView.Rows.Clear()
            ChargementnotePatient()
        End If

        vFPatientNoteVaccinDetailEdit.Dispose()
    End Sub

    Private Function CalculmoduloNIR(NIR As Int64) As Integer
        Dim Reste As Integer
        Reste = NIR Mod 97
        Return 97 - Reste
    End Function

    Private Function AfficheDateCreationNote(dateCreation As Date) As String
        Dim dateCreationNote As String
        If dateCreation.Month = Date.Now.Month And dateCreation.Year = Date.Now.Year Then
            dateCreationNote = " " + dateCreation.ToString("dd.MM.yyyy")
        Else
            If DateDiff(DateInterval.Year, Date.Now, dateCreation) < 5 Then
                dateCreationNote = " " + dateCreation.ToString("MM.yyyy")
            Else
                dateCreationNote = " " + dateCreation.ToString("yyyy")
            End If
        End If

        Return dateCreationNote
    End Function

    Private Sub BtnCreation_Click(sender As Object, e As EventArgs) Handles BtnCreation.Click
        Dim vFPatientNoteVaccinDetailEdit As New FPatientNoteVaccinDetailEdit
        vFPatientNoteVaccinDetailEdit.SelectedNoteId = 0
        vFPatientNoteVaccinDetailEdit.SelectedPatient = Me.SelectedPatient
        vFPatientNoteVaccinDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte

        vFPatientNoteVaccinDetailEdit.ShowDialog() 'Modal
        If vFPatientNoteVaccinDetailEdit.CodeRetour = True Then
            NotePatientDataGridView.Rows.Clear()
            ChargementnotePatient()
        End If

        vFPatientNoteVaccinDetailEdit.Dispose()
    End Sub
End Class