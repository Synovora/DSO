Imports System.Data.SqlClient
Imports Oasis_Common
Public Class RadFContextedetailEdit
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedContexteId As Integer
    Private privateSelectedDrcId As Integer
    Private privateCodeRetour As Boolean
    Private _CodeResultat As Integer
    Private privateContexteTransformeEnAntecedent As Boolean
    Private _positionGaucheDroite As Integer
    Private _conclusionEpisode As Boolean
    Private _episode As Episode


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

    Public Property SelectedContexteId As Integer
        Get
            Return privateSelectedContexteId
        End Get
        Set(value As Integer)
            privateSelectedContexteId = value
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

    Public Property SelectedDrcId As Integer
        Get
            Return privateSelectedDrcId
        End Get
        Set(value As Integer)
            privateSelectedDrcId = value
        End Set
    End Property

    Public Property ContexteTransformeEnAntecedent As Boolean
        Get
            Return privateContexteTransformeEnAntecedent
        End Get
        Set(value As Boolean)
            privateContexteTransformeEnAntecedent = value
        End Set
    End Property

    Public Property PositionGaucheDroite As Integer
        Get
            Return _positionGaucheDroite
        End Get
        Set(value As Integer)
            _positionGaucheDroite = value
        End Set
    End Property

    Public Property CodeResultat As Integer
        Get
            Return _CodeResultat
        End Get
        Set(value As Integer)
            _CodeResultat = value
        End Set
    End Property

    Public Property ConclusionEpisode As Boolean
        Get
            Return _conclusionEpisode
        End Get
        Set(value As Boolean)
            _conclusionEpisode = value
        End Set
    End Property

    Public Property Episode As Episode
        Get
            Return _episode
        End Get
        Set(value As Episode)
            _episode = value
        End Set
    End Property

    Enum EnumTraitement
        Creation = 5
        Modification = 6
        Arret = 8
    End Enum

    Enum EnumDiagnostic
        Confirmé = 1
        Suspecté = 2
        Notion = 3
    End Enum

    Dim Traitement As Integer
    Dim utilisateurHisto As Utilisateur = New Utilisateur()
    Dim Drc As New Drc()
    Dim drcdao As New DrcDao
    Dim ContexteHistoACreer As New AntecedentHisto
    'Dim conxn As New SqlConnection(getConnectionString())
    Dim ControleAutorisationModification As Boolean = True

    Dim contexteReadDao As New AntecedentDao
    Dim contexteDao As New ContexteDao
    Dim contexteRead As New Antecedent
    Dim contexteUpdate As New Antecedent

    Private Sub RadFContextedetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If PositionGaucheDroite = EnumPosition.Droite Then
            Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - Me.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        Else
            Me.Location = New Point(10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        End If
        CodeResultat = EnumResultat.AttenteAction

        If ConclusionEpisode = True Then
            afficheTitleForm(Me, "Détail contexte de conclusion d'épisode")
        Else
            afficheTitleForm(Me, "Détail contexte")
        End If

        InitZone()
        DroitAcces()
        ChargementEtatCivil()
        LblEtatContexte.Text = ""
        If SelectedContexteId <> 0 Then
            Traitement = EnumTraitement.Modification
            ChargementContexteExistant()
            If contexteUpdate.Inactif = True Then
                RadValidation.Enabled = False
                LblEtatContexte.Text = "*** Contexte annulé (non modifiable) ***"
            End If
            If contexteUpdate.Type = "A" Then
                RadValidation.Enabled = False
                LblEtatContexte.Text = "*** Contexte transformé en antécédent (non modifiable) ***"
            End If
            'InhiberZonesDeSaisie()
            GestionAffichageBoutonValidation()
        Else
            Traitement = EnumTraitement.Creation
            'EditMode = EnumEditMode.Creation

            contexteUpdate.PatientId = SelectedPatient.patientId
            contexteUpdate.Type = "C"
            contexteUpdate.Niveau = 1
            contexteUpdate.Nature = "Patient"
            contexteUpdate.Inactif = False
            RadValidation.Show()

            'Catégorie
            'CbxCategorieContexte.Text = ContexteDao.EnumParcoursBaseItem.Medical
            contexteUpdate.CategorieContexte = ContexteDao.EnumParcoursBaseCode.Medical
            RadioBtnMedical.Checked = True

            'Dénomination DRC
            contexteUpdate.DrcId = SelectedDrcId
            TxtDrcId.Text = SelectedDrcId
            If drcdao.GetDrc(Drc, SelectedDrcId) = True Then
                LblDrcDenomination.Text = Drc.DrcLibelle
                LblDrcDenomination.ForeColor = Color.DarkBlue
                'TxtContexteDescription.Text = Drc.DrcLibelle
            Else
                LblDrcDenomination.Text = ""
            End If
            TxtContexteDescription.Text = ""
            contexteUpdate.Description = ""
            'Date début
            DteDateDebut.Value = Date.Now
            contexteUpdate.DateDebut = Date.Now
            'Cacher la date de fin et l'initialiser à la date virtuelle infinie
            DteDateFin.Format = DateTimePickerFormat.Custom
            DteDateFin.CustomFormat = " "
            DteDateFin.Value = New Date(2999, 12, 31, 0, 0, 0)
            contexteUpdate.DateFin = DteDateFin.Value
            'Publication
            ChkPublie.Checked = True
            ChkPublie.ForeColor = Color.Red
            ChkPublieTransformation.Checked = True
            ChkPublieTransformation.ForeColor = Color.Red
            LblPublication.Hide()
            contexteUpdate.StatutAffichage = "P"
            If ConclusionEpisode = True Then
                If Episode.TypeActivite <> EpisodeDao.EnumTypeActiviteEpisodeCode.PATHOLOGIE_AIGUE Then
                    ChkCache.Checked = True
                    ChkCache.ForeColor = Color.Red
                    LblPublication.Text = "Contexte masqué"
                    ChkCacheTransformation.Checked = True
                    ChkCacheTransformation.ForeColor = Color.Red
                End If
            End If

            'Diagnostic
            ChkDiagnosticConfirme.Checked = True
            ChkDiagnosticConfirme.ForeColor = Color.Red
            contexteUpdate.Diagnostic = EnumDiagnostic.Confirmé

            'Inhiber les zones d'arrêt
            'Affichage des boutons d'action
            'Inhiber les boutons d'action de mise à jour
            RadBtnTransformer.Hide()
            RadBtnSupprimer.Hide()
            RadBtnHistorique.Hide()
            LblCreationContexte1.Hide()
            LblCreationContexte2.Hide()
            LblContexteDateCreation.Hide()
            LblUtilisateurCreation.Hide()
            LblModificationContexte1.Hide()
            LblModificationContexte2.Hide()
            LblContexteDateModification.Hide()
            LblUtilisateurModification.Hide()
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)
    End Sub

    Private Sub ChargementContexteExistant()
        contexteRead = contexteReadDao.GetAntecedentById(SelectedContexteId)
        Dim MaxDate As New Date(2999, 12, 31, 0, 0, 0)
        contexteRead.AldDateDebut = MaxDate
        contexteRead.AldDateFin = MaxDate
        contexteRead.AldDateDemande = MaxDate
        contexteUpdate = contexteReadDao.CloneAntecedent(contexteRead)

        Dim dateDebut, dateFin, dateCreation, dateModification As Date
        Dim ordreAffichage As Integer
        Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)

        Select Case contexteRead.CategorieContexte
            Case ContexteDao.EnumParcoursBaseCode.Medical
                'CbxCategorieContexte.Text = ContexteDao.EnumParcoursBaseItem.Medical
                RadioBtnMedical.Checked = True
            Case ContexteDao.EnumParcoursBaseCode.BioEnvironnemental
                'CbxCategorieContexte.Text = ContexteDao.EnumParcoursBaseItem.BioEnvironnemental
                RadioBtnBioEnvironnemental.Checked = True
            Case Else
                'CbxCategorieContexte.Text = ContexteDao.EnumParcoursBaseItem.Medical
                contexteUpdate.CategorieContexte = ContexteDao.EnumParcoursBaseCode.Medical
                RadioBtnMedical.Checked = True
        End Select

        If contexteRead.DrcId = 0 Then
            TxtDrcId.Text = ""
            LblDrcDenomination.Text = ""
        Else
            TxtDrcId.Text = contexteRead.DrcId
            If drcdao.GetDrc(Drc, contexteRead.DrcId) = True Then
                LblDrcDenomination.Text = Drc.DrcLibelle
                LblDrcDenomination.ForeColor = Color.DarkBlue
            End If
        End If

        If contexteRead.Description = "" Then
            TxtContexteDescription.Text = ""
        Else
            TxtContexteDescription.Text = contexteRead.Description
        End If

        'Récupération de la date de début du contexte
        If contexteRead.DateDebut <> Nothing Then
            dateDebut = contexteRead.DateDebut
        Else
            dateDebut = MaxDate
            contexteRead.DateDebut = MaxDate
            contexteUpdate.DateDebut = MaxDate
        End If
        DteDateDebut.Value = dateDebut

        'Récupération de la date de fin de validité du contexte
        If contexteRead.DateFin <> Nothing Then
            dateFin = contexteRead.DateFin
        Else
            dateFin = MaxDate
            contexteRead.DateFin = MaxDate
            contexteUpdate.DateFin = MaxDate
        End If
        DteDateFin.Value = dateFin

        If DteDateFin.Value <> MaxDate Then
            DteDateFin.Format = DateTimePickerFormat.Long
        Else
            DteDateFin.Format = DateTimePickerFormat.Custom
            DteDateFin.CustomFormat = " "
        End If

        'Ordre d'affichage
        ordreAffichage = contexteRead.Ordre1
        NumOrdreAffichage.Value = ordreAffichage

        'Statut affichage du contexte
        ChkCache.Checked = False
        ChkPublie.Checked = False
        If contexteRead.StatutAffichage <> "" Then
            Dim StatutAffichage As String = contexteRead.StatutAffichage
            Select Case StatutAffichage
                Case "P"
                    ChkPublie.Checked = True
                    ChkPublie.ForeColor = Color.Red
                    LblPublication.Text = "Contexte affiché"
                Case "C"
                    ChkCache.Checked = True
                    ChkCache.ForeColor = Color.Red
                    LblPublication.Text = "Contexte masqué"
            End Select
        End If

        'Statut affichage de transformation du contexte
        ChkCacheTransformation.Checked = False
        ChkPublieTransformation.Checked = False
        ChkOcculteTransformation.Checked = False
        If contexteRead.StatutAffichageTransformation <> "" Then
            Dim StatutAffichageTransformation As String = contexteRead.StatutAffichageTransformation
            Select Case StatutAffichageTransformation
                Case "P"
                    ChkPublieTransformation.Checked = True
                    ChkPublieTransformation.ForeColor = Color.Red
                Case "C"
                    ChkCacheTransformation.Checked = True
                    ChkCacheTransformation.ForeColor = Color.Red
                Case "O"
                    ChkOcculteTransformation.Checked = True
                    ChkOcculteTransformation.ForeColor = Color.Red
            End Select
        End If

        'Diagnostic
        If contexteRead.Diagnostic <> 0 Then
            Dim Diagnostic As Integer = contexteRead.Diagnostic
            Select Case Diagnostic
                Case 1
                    ChkDiagnosticConfirme.Checked = True
                    ChkDiagnosticConfirme.ForeColor = Color.Red
                Case 2
                    ChkDiagnosticSuspecte.Checked = True
                    ChkDiagnosticSuspecte.ForeColor = Color.Red
                Case 3
                    ChkDiagnosticNotion.Checked = True
                    ChkDiagnosticNotion.ForeColor = Color.Red
            End Select
        End If

        'Création du contexte : date et utilisateur
        If contexteRead.DateCreation <> Nothing Then
            dateCreation = contexteRead.DateCreation
            LblContexteDateCreation.Text = dateCreation.ToString("dd.MM.yyyy")
        Else
            LblContexteDateCreation.Text = ""
            LblCreationContexte1.Hide()
            LblCreationContexte2.Hide()
        End If

        LblUtilisateurCreation.Text = ""
        If contexteRead.UserCreation <> 0 Then
            Dim userDao As New UserDao
            utilisateurHisto = userDao.getUserById(contexteRead.UserCreation)
            'SetUtilisateur(utilisateurHisto, contexteRead.UserCreation)
            LblUtilisateurCreation.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
        Else
            LblCreationContexte2.Hide()
        End If

        If contexteRead.DateModification <> Nothing Then
            dateModification = contexteRead.DateModification
            LblContexteDateModification.Text = dateModification.ToString("dd.MM.yyyy")
        Else
            LblContexteDateModification.Text = ""
            LblModificationContexte1.Hide()
            LblModificationContexte2.Hide()
        End If
        'End If

        LblUtilisateurModification.Text = ""
        If contexteRead.UserModification <> 0 Then
            Dim userDao As New UserDao
            utilisateurHisto = userDao.getUserById(contexteRead.UserModification)
            'SetUtilisateur(utilisateurHisto, contexteRead.UserModification)
            LblUtilisateurModification.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
        Else
            LblModificationContexte2.Hide()
        End If

        'Initialisation classe Historisation contexte 
        AntecedentHistoCreationDao.InitAntecedentHistorisation(contexteRead, UtilisateurConnecte, ContexteHistoACreer)
    End Sub

    'Suppression (annulation) du contexte
    Private Sub RadBtnSupprimer_Click(sender As Object, e As EventArgs) Handles RadBtnSupprimer.Click
        If MsgBox("Attention, confirmez-vous l'annulation du contexte", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            'Annulation contexte
            If contexteDao.AnnulationContexte(contexteUpdate, ContexteHistoACreer) = True Then
                CodeResultat = EnumResultat.AnnulationOK
                Dim form As New RadFNotification()
                form.Titre = "Notification contexte patient"
                form.Message = "Contexte patient annulé"
                form.Show()
                Me.CodeRetour = True
                Close()
            Else
                Me.CodeRetour = False
            End If
        End If
    End Sub

    'Transformation du contexte en antécédent
    Private Sub RadBtnTransformer_Click(sender As Object, e As EventArgs) Handles RadBtnTransformer.Click
        If MsgBox("confirmation de la transformation en antécédent", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            Dim Description As String = contexteUpdate.Description & " (" & contexteUpdate.DateDebut.ToString("MM.yyyy") & ")"
            If contexteDao.TransformationEnAntecedent(SelectedContexteId, ContexteHistoACreer, Description, contexteRead.StatutAffichageTransformation, userLog) = True Then
                Dim form As New RadFNotification()
                form.Titre = "Notification contexte patient"
                form.Message = "Le contexte patient a été transformé en antécédent"
                form.Show()
                Me.ContexteTransformeEnAntecedent = True
                Me.CodeRetour = True
                Close()
            Else
                Me.CodeRetour = False
            End If
        End If
    End Sub

    'Validation traitement
    Private Sub RadValidation_Click(sender As Object, e As EventArgs) Handles RadValidation.Click
        Select Case Traitement
            Case EnumTraitement.Creation
                If ValidationContexte() = True Then
                    If contexteDao.CreationContexte(contexteUpdate, ContexteHistoACreer, ConclusionEpisode, Episode) = True Then
                        If ConclusionEpisode = True Then
                            Dim episodeDao As New EpisodeDao
                            episodeDao.MajEpisodeConclusionMedicale(Episode.Id)
                        End If
                        CodeResultat = EnumResultat.CreationOK
                        Dim form As New RadFNotification()
                        form.Titre = "Notification contexte patient"
                        form.Message = "Contexte patient créé"
                        form.Show()
                        Me.CodeRetour = True
                        Close()
                    Else
                        Me.CodeRetour = False
                    End If
                End If
            Case EnumTraitement.Modification
                If ValidationContexte() = True Then
                    If contexteDao.ModificationContexte(contexteUpdate, ContexteHistoACreer) = True Then
                        CodeResultat = EnumResultat.ModificationOK
                        Dim form As New RadFNotification()
                        form.Titre = "Notification contexte patient"
                        form.Message = "Contexte patient modifié"
                        form.Show()
                        Me.CodeRetour = True
                        Close()
                    Else
                        Me.CodeRetour = False
                    End If
                End If
        End Select
    End Sub

    'Lance la validation des modifications des données générales, code DRC, description et date de début
    Private Function ValidationContexte() As Boolean
        'Contrôle des données saisie
        Dim Valide As Boolean = True
        Dim MessageErreur1 As String = ""
        Dim MessageErreur2 As String = ""
        Dim MessageErreur3 As String = ""
        Dim MessageErreur4 As String = ""
        Dim MessageErreur As String = ""

        If TxtDrcId.Text = "" Then
            Valide = False
            MessageErreur1 = "Le code DRC est obligatoire"
        End If

        If TxtContexteDescription.Text = "" Then
            Valide = False
            MessageErreur2 = "La description du contexte est obligatoire"
        End If

        If DteDateDebut.Value > Date.Now() Then
            Valide = False
            MessageErreur3 = "La date de début du contexte ne peut pas être supérieure à la date du jour"
        End If

        If DteDateFin.Value < DteDateDebut.Value Then
            Valide = False
            MessageErreur4 = "La date de fin de validité du contexte ne peut pas être inférieure à la date de début"
        End If

        'Appel de la mise à jour des données
        If Valide = False Then
            'Préparation de l'affichage des erreurs
            If Valide = False Then
                If MessageErreur1 <> "" Then
                    MessageErreur = MessageErreur & MessageErreur1 & vbCrLf
                End If

                If MessageErreur2 <> "" Then
                    MessageErreur = MessageErreur & MessageErreur2 & vbCrLf
                End If

                If MessageErreur3 <> "" Then
                    MessageErreur = MessageErreur & MessageErreur3 & vbCrLf
                End If

                If MessageErreur4 <> "" Then
                    MessageErreur = MessageErreur & MessageErreur4 & vbCrLf
                End If

                Select Case Traitement
                    Case EnumTraitement.Creation
                        MessageErreur = MessageErreur & vbCrLf & "/!\ Création du contexte impossible, des données sont incorrectes"
                    Case EnumTraitement.Modification
                        MessageErreur = MessageErreur & vbCrLf & "/!\ Modification du contexte impossible, des données sont incorrectes"
                End Select
                MessageBox.Show(MessageErreur, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If

        Return Valide
    End Function

    Private Sub TxtDrcId_DoubleClick(sender As Object, e As EventArgs) Handles TxtDrcId.DoubleClick
        SelectDrc()
    End Sub

    Private Sub RadDrcSelect_Click(sender As Object, e As EventArgs) Handles RadBtnDrcSelect.Click
        SelectDrc()
    End Sub

    Private Sub SelectDrc()
        'Appel du sélecteur de code DRC
        Using vFDrcSelecteur As New RadFDRCSelecteur
            vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
            vFDrcSelecteur.CategorieOasis = 1       'Catégorie Oasis : "Contexte et Antécédent"
            vFDrcSelecteur.ShowDialog()             'Modal
            Dim SelectedDrcId As Integer = vFDrcSelecteur.SelectedDrcId
            'Si un médicament a été sélectionné
            If SelectedDrcId <> 0 Then
                TxtDrcId.Text = SelectedDrcId
                If drcdao.GetDrc(Drc, SelectedDrcId) = True Then
                    LblDrcDenomination.Text = Drc.DrcLibelle
                    LblDrcDenomination.ForeColor = Color.DarkBlue
                Else
                    LblDrcDenomination.Text = ""
                End If
            End If
        End Using
    End Sub


    '=============================================================================================
    '==================================== Gestion de l'affichage des zones d'écran ===============
    '=============================================================================================

    'Initialisation des zones de saisie
    Private Sub InitZone()
        'CbxCategorieContexte.Items.Clear()
        'CbxCategorieContexte.Items.Add(ContexteDao.EnumParcoursBaseItem.Medical)
        'CbxCategorieContexte.Items.Add(ContexteDao.EnumParcoursBaseItem.BioEnvironnemental)

        Me.ContexteTransformeEnAntecedent = False
        TxtDrcId.Text = ""
        TxtContexteDescription.Text = ""
        DteDateDebut.Value = "01/01/1900"
        DteDateFin.Value = "31/12/2999"
        NumOrdreAffichage.Value = 0
        ChkCache.Checked = False
        ChkPublie.Checked = False
    End Sub

    'Inhiber les zones de saisie
    Private Sub InhiberZonesDeSaisie()
        'CbxCategorieContexte.Enabled = False
        RadioBtnMedical.Enabled = False
        RadioBtnBioEnvironnemental.Enabled = False
        TxtDrcId.Enabled = False
        TxtContexteDescription.Enabled = False
        DteDateDebut.Enabled = False
        DteDateFin.Enabled = False
        NumOrdreAffichage.Enabled = False
        ChkCache.Enabled = False
        ChkPublie.Enabled = False
        ChkDiagnosticConfirme.Enabled = False
        ChkDiagnosticSuspecte.Enabled = False
        'ChkDiagnosticSuppose.Enabled = False
        ChkDiagnosticNotion.Enabled = False
        RadBtnDrcSelect.Enabled = False
    End Sub

    'Récupère la dénomination pour alimenter la description du contexte
    Private Sub RadBtnRecupereDrc_Click(sender As Object, e As EventArgs) Handles RadBtnRecupereDrc.Click
        TxtContexteDescription.Text = LblDrcDenomination.Text
    End Sub

    'Si l'utilisateur lance le dateTimePicker de la date de fin alors que celle-ci est virtuellement "infinie", on initialise sa valeur avec la date du jour  
    Private Sub DteDateFin_DropDown(sender As Object, e As EventArgs) Handles DteDateFin.DropDown
        Dim DateInfinie As New Date(2999, 12, 31, 0, 0, 0)
        If DteDateFin.Value = DateInfinie Then
            DteDateFin.Value = DteDateDebut.Value
            DteDateFin.Format = DateTimePickerFormat.Long
        End If
    End Sub

    Private Sub LblId_MouseHover(sender As Object, e As EventArgs) Handles LblId.MouseHover
        ToolTip1.SetToolTip(LblId, "Id : " + SelectedContexteId.ToString)
    End Sub


    Private Sub RadDtnAbandon_Click(sender As Object, e As EventArgs) Handles RadDtnAbandon.Click
        Me.CodeRetour = False
        Close()
    End Sub


    '======================================================
    '   Gestion des zones de saisie modifiées
    '======================================================

    'Private Sub CbxCategorieContexte_SelectedIndexChanged(sender As Object, e As EventArgs)
    'Select Case CbxCategorieContexte.Text
    'Case ContexteDao.EnumParcoursBaseItem.Medical
    'contexteUpdate.CategorieContexte = ContexteDao.EnumParcoursBaseCode.Medical
    'Case ContexteDao.EnumParcoursBaseItem.BioEnvironnemental
    'contexteUpdate.CategorieContexte = ContexteDao.EnumParcoursBaseCode.BioEnvironnemental
    'End Select
    'GestionAffichageBoutonValidation()
    'End Sub


    Private Sub RadioBtnMedical_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnMedical.CheckedChanged
        contexteUpdate.CategorieContexte = ContexteDao.EnumParcoursBaseCode.Medical
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub RadioBtnBioEnvironnemental_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnBioEnvironnemental.CheckedChanged
        contexteUpdate.CategorieContexte = ContexteDao.EnumParcoursBaseCode.BioEnvironnemental
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtDrcId_TextChanged(sender As Object, e As EventArgs) Handles TxtDrcId.TextChanged
        contexteUpdate.DrcId = TxtDrcId.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtContexteDescription_TextChanged(sender As Object, e As EventArgs) Handles TxtContexteDescription.TextChanged
        contexteUpdate.Description = TxtContexteDescription.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub DteDateDebut_ValueChanged(sender As Object, e As EventArgs) Handles DteDateDebut.ValueChanged
        contexteUpdate.DateDebut = DteDateDebut.Value
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub DteDateFin_ValueChanged(sender As Object, e As EventArgs) Handles DteDateFin.ValueChanged
        contexteUpdate.DateFin = DteDateFin.Value
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub NumOrdreAffichage_ValueChanged(sender As Object, e As EventArgs) Handles NumOrdreAffichage.ValueChanged
        contexteUpdate.Ordre1 = NumOrdreAffichage.Value
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub CbxDiagnosticConfirme_CheckedChanged(sender As Object, e As EventArgs) Handles ChkDiagnosticConfirme.CheckedChanged
        If ChkDiagnosticConfirme.Checked = True Then
            ChkDiagnosticNotion.Checked = False
            ChkDiagnosticSuspecte.Checked = False
            ChkDiagnosticConfirme.ForeColor = Color.Red
            ChkDiagnosticNotion.ForeColor = Color.Black
            ChkDiagnosticSuspecte.ForeColor = Color.Black
            contexteUpdate.Diagnostic = EnumDiagnostic.Confirmé
            GestionAffichageBoutonValidation()
        End If
    End Sub

    Private Sub CbxDiagnosticSuspecte_CheckedChanged(sender As Object, e As EventArgs) Handles ChkDiagnosticSuspecte.CheckedChanged
        If ChkDiagnosticSuspecte.Checked = True Then
            ChkDiagnosticNotion.Checked = False
            ChkDiagnosticConfirme.Checked = False
            ChkDiagnosticSuspecte.ForeColor = Color.Red
            ChkDiagnosticNotion.ForeColor = Color.Black
            ChkDiagnosticConfirme.ForeColor = Color.Black
            contexteUpdate.Diagnostic = EnumDiagnostic.Suspecté
            GestionAffichageBoutonValidation()
        End If
    End Sub

    Private Sub CbxDiagnosticNotion_CheckedChanged(sender As Object, e As EventArgs) Handles ChkDiagnosticNotion.CheckedChanged
        If ChkDiagnosticNotion.Checked = True Then
            ChkDiagnosticConfirme.Checked = False
            ChkDiagnosticSuspecte.Checked = False
            ChkDiagnosticNotion.ForeColor = Color.Red
            ChkDiagnosticConfirme.ForeColor = Color.Black
            ChkDiagnosticSuspecte.ForeColor = Color.Black
            contexteUpdate.Diagnostic = EnumDiagnostic.Notion
            GestionAffichageBoutonValidation()
        End If
    End Sub

    Private Sub ChkPublie_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPublie.CheckedChanged
        If ChkPublie.Checked = True Then
            ChkCache.Checked = False
            ChkPublie.ForeColor = Color.Red
            ChkCache.ForeColor = Color.Black
            contexteUpdate.StatutAffichage = "P"
            GestionAffichageBoutonValidation()
        End If
    End Sub

    Private Sub ChkCache_CheckedChanged(sender As Object, e As EventArgs) Handles ChkCache.CheckedChanged
        If ChkCache.Checked = True Then
            ChkPublie.Checked = False
            ChkCache.ForeColor = Color.Red
            ChkPublie.ForeColor = Color.Black
            contexteUpdate.StatutAffichage = "C"
            GestionAffichageBoutonValidation()
        End If
    End Sub


    'Statut affichage de transformation
    Private Sub ChkPublieTransformation_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPublieTransformation.CheckedChanged
        If ChkPublieTransformation.Checked = True Then
            ChkCacheTransformation.Checked = False
            ChkOcculteTransformation.Checked = False
            ChkPublieTransformation.ForeColor = Color.Red
            ChkCacheTransformation.ForeColor = Color.Black
            ChkOcculteTransformation.ForeColor = Color.Black
            contexteUpdate.StatutAffichageTransformation = "P"
            GestionAffichageBoutonValidation()
        End If
    End Sub

    Private Sub ChkCacheTransformation_CheckedChanged(sender As Object, e As EventArgs) Handles ChkCacheTransformation.CheckedChanged
        If ChkCacheTransformation.Checked = True Then
            ChkOcculteTransformation.Checked = False
            ChkPublieTransformation.Checked = False
            ChkCacheTransformation.ForeColor = Color.Red
            ChkOcculteTransformation.ForeColor = Color.Black
            ChkPublieTransformation.ForeColor = Color.Black
            contexteUpdate.StatutAffichageTransformation = "C"
            GestionAffichageBoutonValidation()
        End If
    End Sub

    Private Sub ChkOcculteTransformation_CheckedChanged(sender As Object, e As EventArgs) Handles ChkOcculteTransformation.CheckedChanged
        If ChkOcculteTransformation.Checked = True Then
            ChkCacheTransformation.Checked = False
            ChkPublieTransformation.Checked = False
            ChkOcculteTransformation.ForeColor = Color.Red
            ChkCacheTransformation.ForeColor = Color.Black
            ChkPublieTransformation.ForeColor = Color.Black
            contexteUpdate.StatutAffichageTransformation = "O"
            GestionAffichageBoutonValidation()
        End If
    End Sub

    Private Sub GestionAffichageBoutonValidation()
        If Traitement = EnumTraitement.Modification Then
            If ControleAutorisationModification = True Then
                If contexteReadDao.Compare(contexteUpdate, contexteRead) = False Then
                    RadValidation.Enabled = True
                Else
                    RadValidation.Enabled = False
                End If
            End If
        End If
    End Sub

    Private Sub DroitAcces()
        If contexteUpdate.Inactif = True OrElse
            contexteUpdate.Type = "A" Then
            ControleAutorisationModification = False
            RadValidation.Enabled = False
            RadBtnTransformer.Enabled = False
            RadBtnSupprimer.Enabled = False
        End If

        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            RadBtnDrcSelect.Hide()
            RadBtnRecupereDrc.Hide()
            RadBtnSupprimer.Hide()
            RadBtnTransformer.Hide()
            'CbxCategorieContexte.Enabled = False
            RadioBtnMedical.Enabled = False
            RadioBtnBioEnvironnemental.Enabled = False
            TxtContexteDescription.Enabled = False
            DteDateDebut.Enabled = False
            DteDateFin.Enabled = False
            ChkCache.Enabled = False
            ChkCacheTransformation.Enabled = False
            ChkDiagnosticConfirme.Enabled = False
            ChkDiagnosticNotion.Enabled = False
            ChkDiagnosticSuspecte.Enabled = False
            ChkPublie.Enabled = False
            ChkPublieTransformation.Enabled = False
            ChkOcculteTransformation.Enabled = False
        End If
    End Sub

    Private Sub RadBtnHistorique_Click(sender As Object, e As EventArgs) Handles RadBtnHistorique.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Try
            Using Form As New RadFAntecedentHistoListe
                Form.SelectedAntecedentId = SelectedContexteId
                Form.SelectedPatient = Me.SelectedPatient
                Form.UtilisateurConnecte = Me.UtilisateurConnecte
                Form.ShowDialog()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
        Me.Enabled = True
    End Sub

End Class
