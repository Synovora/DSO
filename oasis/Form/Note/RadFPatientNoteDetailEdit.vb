Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.Localization
Imports Telerik.WinForms.Documents.Model
Imports Oasis_Common
Public Class RadFPatientNoteDetailEdit
    Private _typeNote As Integer
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

    'Dim conxn As New SqlConnection(getConnectionString())
    Private Sub RadFPatientNoteDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()
        Select Case TypeNote
            Case EnumTypeNote.Medicale
                Me.Text = "Note(s) médicale(s) du patient"
            Case EnumTypeNote.Vaccin
                Me.Text = "Note(s) de vaccination du patient"
            Case EnumTypeNote.Social
                Me.Text = "Note(s) du contexte social du patient"
            Case EnumTypeNote.Administratif
                Me.Text = "Note(s) patient"
            Case EnumTypeNote.Directive
                Me.Text = "Directives anticipées du patient"
            Case Else
                Close()
        End Select
        ChargementEtatCivil()
        If SelectedNoteId <> 0 Then
            'Modification
            EditMode = EnumEditMode.Modification
            ChargementNoteExistante()
        Else
            'Création
            EditMode = EnumEditMode.Creation
            TxtNote.Text = ""
            'Inhiber le bouton d'action d'annulation d'une note qui n'est valide qu'en modification
            RadBtnAnnuler.Hide()
            'Cacher les zones d'information de création et de mise à jour
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
        Dim patientNote As New PatientNote


        Try
            Select Case TypeNote
                Case EnumTypeNote.Medicale
                    Dim patientNoteMedicaleDao As PatientNoteMedicaleDao = New PatientNoteMedicaleDao
                    patientNote = patientNoteMedicaleDao.getTraitementById(SelectedNoteId)
                Case EnumTypeNote.Vaccin
                    Dim patientNoteVaccinDao As PatientNoteVaccinDao = New PatientNoteVaccinDao
                    patientNote = patientNoteVaccinDao.getTraitementById(SelectedNoteId)
                Case EnumTypeNote.Social
                    Dim patientNoteSocialeDao As PatientNoteSocialeDao = New PatientNoteSocialeDao
                    patientNote = patientNoteSocialeDao.getTraitementById(SelectedNoteId)
                Case EnumTypeNote.Administratif
                    Dim patientNoteDao As PatientNoteDao = New PatientNoteDao
                    patientNote = patientNoteDao.getTraitementById(SelectedNoteId)
                Case EnumTypeNote.Directive
                    Dim patientNotedirectiveDao As PatientNoteDirectiveDao = New PatientNoteDirectiveDao
                    patientNote = patientNotedirectiveDao.getTraitementById(SelectedNoteId)
            End Select
        Catch ex As Exception
            MessageBox.Show("Note patient : " + ex.Message, "Problème", MessageBoxButtons.OK, MessageBoxIcon.Error)
            Return
        End Try

        TxtNote.Text = patientNote.PatientNote

        'Date et utilisateur création
        If patientNote.DateCreation <> Nothing Then
            LblDateCreation.Text = patientNote.DateCreation.ToString("dd.MM.yyyy")
        Else
            LblDateCreation.Hide()
            LblLabelDateCreation.Hide()
        End If

        If patientNote.UserCreation <> 0 Then
            Dim userDao As New UserDao
            utilisateurHisto = userDao.getUserById(patientNote.UserCreation)
            'SetUtilisateur(utilisateurHisto, patientNote.UserCreation)
            LblUtilisateurCreation.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
        Else
            LblUtilisateurCreation.Hide()
            LblLabelUtilisateurCreation.Hide()
        End If

        'Date et utilisateur modification
        If patientNote.DateModification <> Nothing Then
            LblDateModification.Text = patientNote.DateModification.ToString("dd.MM.yyyy")
        Else
            LblDateModification.Hide()
            LblLabelDateModification.Hide()
        End If

        If patientNote.UserModification <> 0 Then
            Dim userDao As New UserDao
            utilisateurHisto = userDao.getUserById(patientNote.UserModification)
            'SetUtilisateur(utilisateurHisto, patientNote.UserModification)
            LblUtilisateurModification.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
        Else
            LblUtilisateurModification.Hide()
            LblLabelUtilisateurModification.Hide()
        End If

        Dim accesMiseAJour As Boolean = False
        If UtilisateurConnecte.UtilisateurId = patientNote.UserCreation Then
            accesMiseAJour = True
        Else
            If UtilisateurConnecte.UtilisateurAdmin = True Then
                accesMiseAJour = True
            End If
        End If

        If accesMiseAJour = False Then
            RadBtnAnnuler.Hide()
            RadBtnValidation.Hide()
            TxtNote.Enabled = False
        End If
    End Sub

    'Modification d'une note patient
    Private Function ModificationNote() As Boolean
        Dim codeRetour As Boolean = True
        Select Case TypeNote
            Case EnumTypeNote.Medicale
                Dim patientNoteMedicaleDao As PatientNoteMedicaleDao = New PatientNoteMedicaleDao
                Dim patientNoteModification As PatientNote = New PatientNote
                patientNoteModification.NoteId = SelectedNoteId
                patientNoteModification.UserModification = UtilisateurConnecte.UtilisateurId
                patientNoteModification.PatientNote = TxtNote.Text
                If patientNoteMedicaleDao.ModificationNote(patientNoteModification) = True Then
                    'MessageBox.Show("Note patient modifiée")
                    Dim form As New RadFNotification()
                    form.Message = "Note médicale patient modifiée"
                    form.Show()
                Else
                    codeRetour = False
                End If
            Case EnumTypeNote.Vaccin
                Dim patientNoteVaccinDao As PatientNoteVaccinDao = New PatientNoteVaccinDao
                Dim patientNoteModification As PatientNote = New PatientNote
                patientNoteModification.NoteId = SelectedNoteId
                patientNoteModification.UserModification = UtilisateurConnecte.UtilisateurId
                patientNoteModification.PatientNote = TxtNote.Text
                If patientNoteVaccinDao.ModificationNote(patientNoteModification) = True Then
                    'MessageBox.Show("Note patient modifiée")
                    Dim form As New RadFNotification()
                    form.Message = "Note vaccin patient modifiée"
                    form.Show()
                Else
                    codeRetour = False
                End If
            Case EnumTypeNote.Social
                Dim patientNoteSocialeDao As PatientNoteSocialeDao = New PatientNoteSocialeDao
                Dim patientNoteModification As PatientNote = New PatientNote
                patientNoteModification.NoteId = SelectedNoteId
                patientNoteModification.UserModification = UtilisateurConnecte.UtilisateurId
                patientNoteModification.PatientNote = TxtNote.Text
                If patientNoteSocialeDao.ModificationNote(patientNoteModification) = True Then
                    'MessageBox.Show("Note patient modifiée")
                    Dim form As New RadFNotification()
                    form.Message = "Note sociale patient modifiée"
                    form.Show()
                Else
                    codeRetour = False
                End If
            Case EnumTypeNote.Administratif
                Dim patientNoteDao As PatientNoteDao = New PatientNoteDao
                Dim patientNoteModification As PatientNote = New PatientNote
                patientNoteModification.NoteId = SelectedNoteId
                patientNoteModification.UserModification = UtilisateurConnecte.UtilisateurId
                patientNoteModification.PatientNote = TxtNote.Text
                If patientNoteDao.ModificationNote(patientNoteModification) = True Then
                    'MessageBox.Show("Note patient modifiée")
                    Dim form As New RadFNotification()
                    form.Message = "Note administrative patient modifiée"
                    form.Show()
                Else
                    CodeRetour = False
                End If
            Case EnumTypeNote.Directive
                Dim patientNotedirectiveDao As PatientNoteDirectiveDao = New PatientNoteDirectiveDao
                Dim patientNoteModification As PatientNote = New PatientNote
                patientNoteModification.NoteId = SelectedNoteId
                patientNoteModification.UserModification = UtilisateurConnecte.UtilisateurId
                patientNoteModification.PatientNote = TxtNote.Text
                If patientNotedirectiveDao.ModificationNote(patientNoteModification) = True Then
                    'MessageBox.Show("Directive anticipée modifiée")
                    Dim form As New RadFNotification()
                    form.Message = "Directive anticipée patient modifiée"
                    form.Show()
                Else
                    codeRetour = False
                End If
            Case Else
                codeRetour = False
        End Select

        Return CodeRetour
    End Function

    'Création d'une note patient en base de données
    Private Function CreationNote() As Boolean
        Dim codeRetour As Boolean = True
        Select Case TypeNote
            Case EnumTypeNote.Medicale
                Dim patientNoteMedicaleDao As PatientNoteMedicaleDao = New PatientNoteMedicaleDao
                Dim patientNoteCreation As PatientNote = New PatientNote
                patientNoteCreation.PatientId = SelectedPatient.patientId
                patientNoteCreation.UserCreation = UtilisateurConnecte.UtilisateurId
                patientNoteCreation.PatientNote = TxtNote.Text

                If patientNoteMedicaleDao.CreationNote(patientNoteCreation) = True Then
                    'MessageBox.Show("Note patient créée")
                    Dim form As New RadFNotification()
                    form.Message = "Note médicale patient créée"
                    form.Show()
                Else
                    codeRetour = False
                End If
            Case EnumTypeNote.Vaccin
                Dim patientNoteVaccinDao As PatientNoteVaccinDao = New PatientNoteVaccinDao
                Dim patientNoteCreation As PatientNote = New PatientNote
                patientNoteCreation.PatientId = SelectedPatient.patientId
                patientNoteCreation.UserCreation = UtilisateurConnecte.UtilisateurId
                patientNoteCreation.PatientNote = TxtNote.Text

                If patientNoteVaccinDao.CreationNote(patientNoteCreation) = True Then
                    'MessageBox.Show("Note patient créée")
                    Dim form As New RadFNotification()
                    form.Message = "Note vaccin patient créée"
                    form.Show()
                Else
                    codeRetour = False
                End If
            Case EnumTypeNote.Social
                Dim patientNoteSocialeDao As PatientNoteSocialeDao = New PatientNoteSocialeDao
                Dim patientNoteCreation As PatientNote = New PatientNote
                patientNoteCreation.PatientId = SelectedPatient.patientId
                patientNoteCreation.UserCreation = UtilisateurConnecte.UtilisateurId
                patientNoteCreation.PatientNote = TxtNote.Text

                If patientNoteSocialeDao.CreationNote(patientNoteCreation) = True Then
                    'MessageBox.Show("Note patient créée")
                    Dim form As New RadFNotification()
                    form.Message = "Note sociale patient créée"
                    form.Show()
                Else
                    codeRetour = False
                End If
            Case EnumTypeNote.Administratif
                Dim patientNoteDao As PatientNoteDao = New PatientNoteDao
                Dim patientNoteCreation As PatientNote = New PatientNote
                patientNoteCreation.PatientId = SelectedPatient.patientId
                patientNoteCreation.UserCreation = UtilisateurConnecte.UtilisateurId
                patientNoteCreation.PatientNote = TxtNote.Text

                If patientNoteDao.CreationNote(patientNoteCreation) = True Then
                    'MessageBox.Show("Note patient créée")
                    Dim form As New RadFNotification()
                    form.Message = "Note administrative patient créée"
                    form.Show()
                Else
                    codeRetour = False
                End If
            Case EnumTypeNote.Directive
                Dim patientNotedirectiveDao As PatientNoteDirectiveDao = New PatientNoteDirectiveDao
                Dim patientNoteCreation As PatientNote = New PatientNote
                patientNoteCreation.PatientId = SelectedPatient.patientId
                patientNoteCreation.UserCreation = UtilisateurConnecte.UtilisateurId
                patientNoteCreation.PatientNote = TxtNote.Text

                If patientNotedirectiveDao.CreationNote(patientNoteCreation) = True Then
                    'MessageBox.Show("Directive anticipée patient créée")
                    Dim form As New RadFNotification()
                    form.Message = "Directive anticipée patient créée"
                    form.Show()
                Else
                    codeRetour = False
                End If
            Case Else
                codeRetour = False
        End Select

        Return CodeRetour
    End Function

    Private Function AnnulationNote() As Boolean
        Dim codeRetour As Boolean = True
        Select Case TypeNote
            Case EnumTypeNote.Medicale
                Dim patientNoteMedicaleDao As PatientNoteMedicaleDao = New PatientNoteMedicaleDao
                Dim patientNoteSuppression As PatientNote = New PatientNote
                patientNoteSuppression.NoteId = SelectedNoteId
                patientNoteSuppression.UserModification = UtilisateurConnecte.UtilisateurId
                If patientNoteMedicaleDao.AnnulationNote(patientNoteSuppression) = True Then
                    'MessageBox.Show("Note patient supprimée")
                    Dim form As New RadFNotification()
                    form.Message = "Note médicale patient supprimée"
                    form.Show()
                Else
                    codeRetour = False
                End If
            Case EnumTypeNote.Vaccin
                Dim patientNoteVaccinDao As PatientNoteVaccinDao = New PatientNoteVaccinDao
                Dim patientNoteSuppression As PatientNote = New PatientNote
                patientNoteSuppression.NoteId = SelectedNoteId
                patientNoteSuppression.UserModification = UtilisateurConnecte.UtilisateurId
                If patientNoteVaccinDao.AnnulationNote(patientNoteSuppression) = True Then
                    'MessageBox.Show("Note patient supprimée")
                    Dim form As New RadFNotification()
                    form.Message = "Note vaccin patient supprimée"
                    form.Show()
                Else
                    codeRetour = False
                End If
            Case EnumTypeNote.Social
                Dim patientNoteSocialeDao As PatientNoteSocialeDao = New PatientNoteSocialeDao
                Dim patientNoteSuppression As PatientNote = New PatientNote
                patientNoteSuppression.NoteId = SelectedNoteId
                patientNoteSuppression.UserModification = UtilisateurConnecte.UtilisateurId
                If patientNoteSocialeDao.AnnulationNote(patientNoteSuppression) = True Then
                    'MessageBox.Show("Note patient supprimée")
                    Dim form As New RadFNotification()
                    form.Message = "Note sociale patient supprimée"
                    form.Show()
                Else
                    codeRetour = False
                End If
            Case EnumTypeNote.Administratif
                Dim patientNoteDao As PatientNoteDao = New PatientNoteDao
                Dim patientNoteSuppression As PatientNote = New PatientNote
                patientNoteSuppression.NoteId = SelectedNoteId
                patientNoteSuppression.UserModification = UtilisateurConnecte.UtilisateurId
                If patientNoteDao.AnnulationNote(patientNoteSuppression) = True Then
                    'MessageBox.Show("Note patient supprimée")
                    Dim form As New RadFNotification()
                    form.Message = "Note administrative patient supprimée"
                    form.Show()
                Else
                    codeRetour = False
                End If
            Case EnumTypeNote.Directive
                Dim patientNotedirectiveDao As PatientNoteDirectiveDao = New PatientNoteDirectiveDao
                Dim patientNoteSuppression As PatientNote = New PatientNote
                patientNoteSuppression.NoteId = SelectedNoteId
                patientNoteSuppression.UserModification = UtilisateurConnecte.UtilisateurId
                If patientNotedirectiveDao.AnnulationNote(patientNoteSuppression) = True Then
                    'MessageBox.Show("Directive anticipée patient supprimée")
                    Dim form As New RadFNotification()
                    form.Message = "Directive anticipée patient supprimée"
                    form.Show()
                Else
                    codeRetour = False
                End If
            Case Else
                codeRetour = False
        End Select

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

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
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

    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        CodeRetour = False
        Close()
    End Sub

    Private Sub RadBtnAnnuler_Click(sender As Object, e As EventArgs) Handles RadBtnAnnuler.Click
        If MsgBox("confirmation de la suppression de la note", MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation, "Confirmation") = MsgBoxResult.Yes Then
            'Annulation note patient
            If AnnulationNote() = True Then
                Me.CodeRetour = True
                Close()
            Else
                Me.CodeRetour = False
            End If
        End If
    End Sub

End Class
