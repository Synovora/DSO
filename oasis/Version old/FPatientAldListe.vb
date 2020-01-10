Imports System.Data.SqlClient

Public Class FPatientAldListe
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

    Private Sub FPatientAldDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If SelectedPatientId <> 0 Then
            ChargementPatient()
            ChargementAldPatient()
        End If
    End Sub

    'Chargement de la Grid Notes patient
    Private Sub ChargementAldPatient()
        'Déclaration des données de connexion
        Dim conxn As New SqlConnection(getConnectionString())
        Dim AldPatientDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim AldPatientDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select * from oasis.oa_patient_ald where (oa_patient_ald_invalide = '0' or oa_patient_ald_invalide is Null) and oa_patient_ald_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_patient_ald_date_fin desc;"

        'Lecture des données en base
        AldPatientDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        AldPatientDataAdapter.Fill(AldPatientDataTable)
        conxn.Open()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateCreation, dateDebut, dateFin As Date
        Dim AfficheDateCreation As String
        Dim rowCount As Integer = AldPatientDataTable.Rows.Count - 1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            'Date création
            AfficheDateCreation = ""
            If AldPatientDataTable.Rows(i)("oa_patient_ald_date_creation") IsNot DBNull.Value Then
                dateCreation = AldPatientDataTable.Rows(i)("oa_patient_ald_date_creation")
                AfficheDateCreation = outils.FormatageDateAffichage(dateCreation)
            Else
                If AldPatientDataTable.Rows(i)("oa_patient_ald_date_creation") IsNot DBNull.Value Then
                    dateCreation = AldPatientDataTable.Rows(i)("oa_patient_ald_date_creation")
                    AfficheDateCreation = outils.FormatageDateAffichage(dateCreation)
                End If
            End If

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            'AldPatientDataGridView.RowTemplate.Height = 40
            AldPatientDataGridView.Rows.Insert(iGrid)

            'Alimentation du DataGridView
            AldPatientDataGridView("aldClef", iGrid).Value = AldPatientDataTable.Rows(i)("oa_patient_ald_clef")
            AldPatientDataGridView("aldId", iGrid).Value = AldPatientDataTable.Rows(i)("oa_patient_ald_id")
            AldPatientDataGridView("aldDescription", iGrid).Value = Table_ald.GetAldDescription(CInt(AldPatientDataTable.Rows(i)("oa_patient_ald_id")))
            AldPatientDataGridView("aldCommentaire", iGrid).Value = AldPatientDataTable.Rows(i)("oa_patient_ald_commentaire")
            dateDebut = AldPatientDataTable.Rows(i)("oa_patient_ald_date_debut")
            AldPatientDataGridView("aldDateDebut", iGrid).Value = dateDebut.ToString("dd/MM/yyyy")
            dateFin = AldPatientDataTable.Rows(i)("oa_patient_ald_date_fin")
            AldPatientDataGridView("aldDateFin", iGrid).Value = dateFin.ToString("dd/MM/yyyy")
            If dateFin > Date.Now Then
                AldPatientDataGridView("aldStatut", iGrid).Value = "ALD En cours"
            Else
                AldPatientDataGridView("aldStatut", iGrid).Value = "ALD expirée"
                AldPatientDataGridView("aldStatut", iGrid).Style.ForeColor = Color.Red
            End If
        Next
        conxn.Close()
        AldPatientDataAdapter.Dispose()

        'Enlève le focus sur la première ligne de la Grid
        AldPatientDataGridView.ClearSelection()
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

    'Appel détail ALD en modification
    Private Sub AldPatientDataGridView_CellDoubleClick(sender As Object, e As DataGridViewCellEventArgs) Handles AldPatientDataGridView.CellDoubleClick
        If AldPatientDataGridView.CurrentRow IsNot Nothing Then
            Dim AldClef As Integer = AldPatientDataGridView.Rows(AldPatientDataGridView.CurrentRow.Index).Cells("aldClef").Value

            Dim vFPatientAldDetailEdit As New FPatientAldDetailEdit
            vFPatientAldDetailEdit.SelectedAldClef = AldClef
            vFPatientAldDetailEdit.SelectedPatient = Me.SelectedPatient
            vFPatientAldDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte

            vFPatientAldDetailEdit.ShowDialog() 'Modal
            If vFPatientAldDetailEdit.CodeRetour = True Then
                AldPatientDataGridView.Rows.Clear()
                ChargementAldPatient()
            End If

            vFPatientAldDetailEdit.Dispose()
        End If
    End Sub

    'Appel détail ALD en création
    Private Sub CréerUneAldToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneNoteToolStripMenuItem.Click
        Dim vFAldSelecteur As New FAldSelecteur
        vFAldSelecteur.UtilisateurConnecte = Me.UtilisateurConnecte
        vFAldSelecteur.ShowDialog() 'Modal
        Dim SelectedAldId As Integer = vFAldSelecteur.SelectedAldId
        vFAldSelecteur.Dispose()
        'Si une ALD a été sélectionnée
        If SelectedAldId <> 0 Then
            Dim vFPatientAldDetailEdit As New FPatientAldDetailEdit
            vFPatientAldDetailEdit.SelectedAldClef = 0
            vFPatientAldDetailEdit.SelectedPatient = Me.SelectedPatient
            vFPatientAldDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientAldDetailEdit.SelectedAldId = SelectedAldId
            vFPatientAldDetailEdit.ShowDialog() 'Modal
            If vFPatientAldDetailEdit.CodeRetour = True Then
                AldPatientDataGridView.Rows.Clear()
                ChargementAldPatient()
            End If
            vFPatientAldDetailEdit.Dispose()
        End If
    End Sub

    Private Sub BtnCreation_Click(sender As Object, e As EventArgs) Handles BtnCreation.Click
        Dim vFPatientNoteVaccinDetailEdit As New FPatientNoteVaccinDetailEdit
        vFPatientNoteVaccinDetailEdit.SelectedNoteId = 0
        vFPatientNoteVaccinDetailEdit.SelectedPatient = Me.SelectedPatient
        vFPatientNoteVaccinDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte

        vFPatientNoteVaccinDetailEdit.ShowDialog() 'Modal
        If vFPatientNoteVaccinDetailEdit.CodeRetour = True Then
            AldPatientDataGridView.Rows.Clear()
            ChargementAldPatient()
        End If

        vFPatientNoteVaccinDetailEdit.Dispose()
    End Sub
End Class