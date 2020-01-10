Imports System.Data.SqlClient
Imports Oasis_WF

Public Class FPatientNoteDetailEdit
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedNoteId As Integer
    Private privateCodeRetour As Boolean

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

    Public Property SelectedNoteId As Integer
        Get
            Return privateSelectedNoteId
        End Get
        Set(value As Integer)
            privateSelectedNoteId = value
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

    Enum EnumEditMode
        Creation = 1
        Modification = 2
    End Enum

    Dim EditMode As Integer
    Dim utilisateurHisto As Utilisateur = New Utilisateur()

    Dim conxn As New SqlConnection(getConnectionString())

    Private Sub FNotePatientDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementEtatCivil()
        If SelectedNoteId <> 0 Then
            'Modification
            EditMode = EnumEditMode.Modification
            ChargementNoteExistante()
        Else
            'Création
            EditMode = EnumEditMode.Creation
            TxtNote.Text = ""
            'Inhiber boutons d'action de mise à jour
            BtnAnnuler.Hide()
            LblDateCreation.Hide()
            LblLabelDateCreation.Hide()
            LblUtilisateurCreation.Hide()
            LblLabelUtilisateurCreation.Hide()
            LblDateModification.Hide()
            LblLabelDateModification.Hide()
            LblUtilisateurModification.Hide()
            LblLabelUtilisateurModification.Hide()
        End If
    End Sub

    Private Sub ChargementNoteExistante()
        Dim NoteDataReader As SqlDataReader
        Dim conxn As New SqlConnection(getConnectionString())
        Dim SQLString As String = "select * from oasis.oa_patient_note where oa_patient_note_id = " & SelectedNoteId & ";"
        Dim myCommand As New SqlCommand(SQLString, conxn)

        Dim dateCreation, dateModification As Date

        conxn.Open()
        NoteDataReader = myCommand.ExecuteReader()
        If NoteDataReader.Read() Then
            'Note
            If NoteDataReader("oa_patient_note") Is DBNull.Value Then
                TxtNote.Text = ""
            Else
                TxtNote.Text = NoteDataReader("oa_patient_note")
            End If

            'Date et utilisateur création
            LblDateCreation.Text = ""
            If NoteDataReader("oa_patient_note_date_creation") IsNot DBNull.Value Then
                dateCreation = NoteDataReader("oa_patient_note_date_creation")
                LblDateCreation.Text = dateCreation.ToString("dd.MM.yyyy")
            Else
                LblDateCreation.Hide()
                LblLabelDateCreation.Hide()
            End If

            LblUtilisateurCreation.Text = ""
            If NoteDataReader("oa_patient_note_utilisateur_creation") IsNot DBNull.Value Then
                If NoteDataReader("oa_patient_note_utilisateur_creation") <> 0 Then
                    SetUtilisateur(utilisateurHisto, NoteDataReader("oa_patient_note_utilisateur_creation"))
                    LblUtilisateurCreation.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
                End If
            Else
                LblUtilisateurCreation.Hide()
                LblLabelUtilisateurCreation.Hide()
            End If

            'Date et utilisateur modification
            LblDateModification.Text = ""
            If NoteDataReader("oa_patient_note_date_modification") IsNot DBNull.Value Then
                dateModification = NoteDataReader("oa_patient_note_date_modification")
                LblDateModification.Text = dateModification.ToString("dd.MM.yyyy")
            Else
                LblDateModification.Hide()
                LblLabelDateModification.Hide()
            End If

            LblUtilisateurModification.Text = ""
            If NoteDataReader("oa_patient_note_utilisateur_Modification") IsNot DBNull.Value Then
                If NoteDataReader("oa_patient_note_utilisateur_Modification") <> 0 Then
                    SetUtilisateur(utilisateurHisto, NoteDataReader("oa_patient_note_utilisateur_Modification"))
                    LblUtilisateurModification.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
                End If
            Else
                LblUtilisateurModification.Hide()
                LblLabelUtilisateurModification.Hide()
            End If
        End If

        Dim accesMiseAJour As Boolean = False
        If UtilisateurConnecte.UtilisateurId = CInt(NoteDataReader("oa_patient_note_utilisateur_creation")) Then
            accesMiseAJour = True
        Else
            If UtilisateurConnecte.UtilisateurAdmin = True Then
                accesMiseAJour = True
            End If
        End If

        If accesMiseAJour = False Then
            BtnAnnuler.Hide()
            BtnValidation.Hide()
            TxtNote.Enabled = False
        End If

        'Libération des ressources d'accès aux données
        conxn.Close()
        myCommand.Dispose()
    End Sub

    'Modification d'une note patient en base de données
    Private Function ModificationNote() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_patient_note set oa_patient_note_date_modification = @dateModification, oa_patient_note_utilisateur_modification = @utilisateurModification, oa_patient_note = @note where oa_patient_note_id = @noteId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@note", TxtNote.Text)
            .AddWithValue("@noteId", SelectedNoteId)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            MessageBox.Show("Note modifiée")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        Return codeRetour
    End Function

    'Création d'une note patient en base de données
    Private Function CreationNote() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "insert into oasis.oa_patient_note (oa_patient_id, oa_patient_note, oa_patient_note_utilisateur_creation, oa_patient_note_date_creation) VALUES (@patientId, @note, @utilisateurCreation, @dateCreation)"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@patientId", SelectedPatient.patientId.ToString)
            .AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@utilisateurCreation", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@note", TxtNote.Text)
        End With

        Try
            conxn.Open()
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
            MessageBox.Show("Note patient créée")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        Return codeRetour

    End Function

    Private Function AnnulationNotePatient() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_patient_note set oa_patient_note_date_modification = @dateModification, oa_patient_note_utilisateur_modification = @utilisateurModification, oa_patient_note_invalide = @invalide where oa_patient_note_id = @noteId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@invalide", 1)
            .AddWithValue("@noteId", SelectedNoteId)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            MessageBox.Show("Note supprimée")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        Return codeRetour
    End Function


    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)
    End Sub

    Private Sub BtnValidation_Click(sender As Object, e As EventArgs) Handles BtnValidation.Click
        If EditMode = EnumEditMode.Creation Then
            If CreationNote() = True Then
                CodeRetour = True
                Close()
            End If
        Else
            If EditMode = EnumEditMode.Modification Then
                If ModificationNote() = True Then
                    CodeRetour = True
                    Close()
                End If
            End If
        End If

    End Sub

    Private Sub BtnAbandonner_Click(sender As Object, e As EventArgs) Handles BtnAbandonner.Click
        CodeRetour = False
        Close()
    End Sub

    Private Sub BtnAnnuler_Click(sender As Object, e As EventArgs) Handles BtnAnnuler.Click
        If MsgBox("confirmation de la suppression de la note", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            'Annulation note patient
            If AnnulationNotePatient() = True Then
                Me.CodeRetour = True
                Close()
            Else
                Me.CodeRetour = False
            End If
        End If
    End Sub
End Class