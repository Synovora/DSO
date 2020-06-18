Imports System.Data.SqlClient
Imports Oasis_WF
Imports Telerik.WinControls.UI.Localization
Imports Oasis_Common

Public Class RadFAntecedentDetailEdit
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedAntecedentId As Integer
    Private privateSelectedDrcId As Integer
    Private _selectedDrc As Drc
    Private privateCodeRetour As Boolean
    Private privateReactivation As Boolean
    Private _positionGaucheDroite As Integer

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

    Public Property SelectedAntecedentId As Integer
        Get
            Return privateSelectedAntecedentId
        End Get
        Set(value As Integer)
            privateSelectedAntecedentId = value
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

    Public Property Reactivation As Boolean
        Get
            Return privateReactivation
        End Get
        Set(value As Boolean)
            privateReactivation = value
        End Set
    End Property

    Public Property SelectedDrc As Drc
        Get
            Return _selectedDrc
        End Get
        Set(value As Drc)
            _selectedDrc = value
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

    Enum EnumEditMode
        Modification = 1
        Creation = 2
    End Enum

    Enum EnumAction
        Creation = 1
        Modification = 2
        Publication = 3
        Sans = 4
    End Enum

    Dim EditMode, EditAction As Integer
    Dim utilisateurHisto As Utilisateur = New Utilisateur()
    Dim Drc As New Drc()
    Dim drcdao As New DrcDao

    Dim AntecedentDao As New AntecedentDao

    Dim AntecedentHistoACreer As New AntecedentHisto
    Dim antecedentRead As New Antecedent
    Dim antecedentUpdate As New Antecedent

    Dim conxn As New SqlConnection(getConnectionString())

    Private Sub RadFAntecedentDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()
        If positionGaucheDroite = EnumPosition.Droite Then
            Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - Me.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        Else
            Me.Location = New Point( 10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        End If

        InitZone()
        DroitAcces()
        ChargementEtatCivil()

        If SelectedAntecedentId <> 0 Then
            'Modification
            EditMode = EnumEditMode.Modification
            EditAction = EnumAction.Sans
            RadBtnValidation.Enabled = False
            ChargementAntecedentExistant()
        Else
            'Création
            EditMode = EnumEditMode.Creation
            EditAction = EnumAction.Creation
            RadBtnHistorique.Hide()
            antecedentUpdate.PatientId = SelectedPatient.patientId
            'Dénomination DRC
            TxtDrcId.Text = Me.SelectedDrcId
            If Me.SelectedDrcId <> 0 Then
                TxtDrcId.Text = Me.SelectedDrcId
                Drc = Me.SelectedDrc
                LblDrcDenomination.Text = Drc.DrcLibelle
                LblDrcDenomination.ForeColor = Color.DarkBlue
                'TxtAntecedentDescription.Text = Drc.DrcLibelle
                If Drc.AldId <> 0 Then
                    RadGbxAld.Show()
                    'AntecedentAldId = Drc.AldId
                    antecedentUpdate.AldId = Drc.AldId
                    TxtAldCode.Text = Drc.AldCode
                    LblAldDescription.Text = Drc.DrcLibelle
                Else
                    RadGbxAld.Hide()
                End If
            Else
                LblDrcDenomination.Text = ""
            End If
            GestionAffichageZoneAld()
            TxtAldCim10Code.Text = ""
            GestionAffichageZoneAldCim10()
            RadChkAldValide.Checked = False
            antecedentUpdate.AldValide = False
            RadChkAldDemandeEnCours.Checked = False
            antecedentUpdate.AldDemandeEnCours = False
            GestionAffichageZoneDeclarationAld()

            'Date début
            'DteDateDebut.Value = Date.Now
            'antecedentUpdate.DateDebut = Date.Now()
            DteDateDebut.Format = DateTimePickerFormat.Custom
            DteDateDebut.CustomFormat = " "
            DteDateDebut.Value = DteDateDebut.MinDate
            antecedentUpdate.DateDebut = DteDateDebut.MinDate

            'Publication
            ChkPublie.Checked = True
            ChkPublie.ForeColor = Color.Red
            LblPublication.Hide()
            antecedentUpdate.StatutAffichage = "P"  'Publié

            'Diagnostic
            ChkDiagnosticConfirme.Checked = True
            ChkDiagnosticConfirme.ForeColor = Color.Red
            antecedentUpdate.Diagnostic = 1 'Confirmé

            'ALD
            DteALDDateDebut.Value = Date.Now()
            antecedentUpdate.AldDateDebut = Date.Now()
            DteALDDateFin.Value = Date.Now()
            antecedentUpdate.AldDateFin = Date.Now()
            DteAldDateDemande.Value = Date.Now()
            antecedentUpdate.AldDateDemande = Date.Now()

            'Inhiber les zones d'arrêt
            'Affichage des boutons d'action
            RadBtnValidation.Show()
            RadBtnRecupereDrc.Show()
            'Inhiber boutons d'action de mise à jour
            RadBtnSupprimer.Hide()
            LblCreationAntecedent1.Hide()
            LblCreationAntecedent2.Hide()
            LblAntecedentDateCreation.Hide()
            LblUtilisateurCreation.Hide()
            LblModificationAntecedent1.Hide()
            LblModificationAntecedent2.Hide()
            LblAntecedentDateModification.Hide()
            LblUtilisateurModification.Hide()
        End If
        RadBtnDrcSelect.Select()
        Cursor.Current = Cursors.Default
    End Sub


    '=============================================================================================
    '==================================== Gestion de l'affichage des zones de l'écran ============
    '=============================================================================================
    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)

        LblPatientAdresse1.Text = SelectedPatient.PatientAdresse1
        LblPatientAdresse2.Text = SelectedPatient.PatientAdresse2
        LblPatientCodePostal.Text = SelectedPatient.PatientCodePostal
        LblPatientVille.Text = SelectedPatient.PatientVille
        LblPatientTel1.Text = SelectedPatient.PatientTel1
        LblPatientTel2.Text = SelectedPatient.PatientTel2
        LblPatientDateMaj.Text = SelectedPatient.PatientSyntheseDateMaj.ToString("dd/MM/yyyy")

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip1.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    Private Sub ChargementAntecedentExistant()
        antecedentRead = AntecedentDao.GetAntecedentById(SelectedAntecedentId)
        antecedentUpdate = AntecedentDao.CloneAntecedent(antecedentRead)

        Dim dateDebut, dateCreation, dateModification As Date

        TxtDrcId.Text = antecedentRead.DrcId

        'Dénomination DRC
        If drcdao.GetDrc(Drc, TxtDrcId.Text) = True Then
            LblDrcDenomination.Text = Drc.DrcLibelle
            LblDrcDenomination.ForeColor = Color.DarkBlue
        Else
            LblDrcDenomination.Text = ""
        End If

        'Description de l'antécédent
        TxtAntecedentDescription.Text = antecedentRead.Description

        'Récupération de la période de début de l'antecedent
        dateDebut = antecedentRead.DateDebut
        If dateDebut = Nothing Then
            dateDebut = Date.Now()
        End If
        DteDateDebut.Value = dateDebut
        If dateDebut = DteDateDebut.MinDate Then
            DteDateDebut.Format = DateTimePickerFormat.Custom
            DteDateDebut.CustomFormat = " "
        End If

        'Statut affichage de l'antécédent
        ChkCache.Checked = False
        ChkOcculte.Checked = False
        ChkPublie.Checked = False

        Select Case antecedentRead.StatutAffichage
            Case "P"
                ChkPublie.Checked = True
                ChkPublie.ForeColor = Color.Red
                LblPublication.Text = "Antécédent affiché"
            Case "C"
                ChkCache.Checked = True
                ChkCache.ForeColor = Color.Red
                LblPublication.Text = "Antécédent masqué"
            Case "O"
                ChkOcculte.Checked = True
                ChkOcculte.ForeColor = Color.Red
                LblPublication.Text = "Antécédent occulté"
        End Select

        'Diagnostic
        Select Case antecedentRead.Diagnostic
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

        If antecedentRead.DateCreation <> Nothing Then
            dateCreation = antecedentRead.DateCreation
            LblAntecedentDateCreation.Text = dateCreation.ToString("dd.MM.yyyy")
        Else
            LblAntecedentDateCreation.Text = ""
            LblCreationAntecedent1.Hide()
            LblCreationAntecedent2.Hide()
        End If

        LblUtilisateurCreation.Text = ""
        If antecedentRead.UserCreation <> 0 Then
            Dim userDao As New UserDao
            utilisateurHisto = userDao.getUserById(antecedentRead.UserCreation)
            'SetUtilisateur(utilisateurHisto, antecedentRead.UserCreation)
            LblUtilisateurCreation.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
        Else
            LblCreationAntecedent2.Hide()
        End If

        'Contrôle si on peut traiter la suppression, la suppression est permise si la date de création = date de suppression
        Dim dateCreationaComparer As New Date(dateCreation.Year, dateCreation.Month, dateCreation.Day, 0, 0, 0)
        If antecedentRead.DateModification <> Nothing Then
            dateModification = antecedentRead.DateModification
            LblAntecedentDateModification.Text = dateModification.ToString("dd.MM.yyyy")
        Else
            LblAntecedentDateModification.Text = ""
            LblModificationAntecedent1.Hide()
            LblModificationAntecedent2.Hide()
        End If

        LblUtilisateurModification.Text = ""
        If antecedentRead.UserModification <> 0 Then
            Dim userDao As New UserDao
            utilisateurHisto = userDao.getUserById(antecedentRead.UserModification)
            'SetUtilisateur(utilisateurHisto, antecedentRead.UserModification)
            LblUtilisateurModification.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
        Else
            LblCreationAntecedent2.Hide()
        End If

        'ALD
        'AntecedentAldId = antecedentRead.AldId
        If antecedentRead.AldId <> "0" Then
            RadGbxAld.Show()
            Dim AldDao As New AldDao
            Dim Ald As Ald
            Ald = AldDao.getAldById(antecedentRead.AldId)
            TxtAldCode.Text = Ald.AldCode
            LblAldDescription.Text = Ald.AldDescription
        Else
            RadGbxAld.Hide()
        End If

        'AntecedentAldCim10Id = antecedentRead.AldCim10Id
        If antecedentRead.AldCim10Id <> 0 Then
            Dim aldCim10 As AldCim10
            aldCim10 = AldCim10Dao.GetAldCim10ById(antecedentRead.AldCim10Id)
            TxtAldCim10Code.Text = aldCim10.AldCim10AldCode
            Lblcim10Description.Text = aldCim10.AldCim10Description
        Else
            TxtAldCim10Code.Text = ""
            Lblcim10Description.Text = ""
        End If

        RadChkAldValide.Checked = antecedentRead.AldValide
        If antecedentRead.AldDateDebut < DteALDDateDebut.MinDate Or antecedentRead.AldDateDebut > DteALDDateDebut.MaxDate Then
            DteALDDateDebut.Value = Date.Now()
            antecedentRead.AldDateDebut = Date.Now()
            antecedentUpdate.AldDateDebut = Date.Now()
        Else
            DteALDDateDebut.Value = antecedentRead.AldDateDebut
        End If

        If antecedentRead.AldDateFin < DteALDDateFin.MinDate Or antecedentRead.AldDateFin > DteALDDateFin.MaxDate Then
            DteALDDateFin.Value = Date.Now()
            antecedentRead.AldDateFin = Date.Now()
            antecedentUpdate.AldDateFin = Date.Now()
        Else
            DteALDDateFin.Value = antecedentRead.AldDateFin
        End If

        RadChkAldDemandeEnCours.Checked = antecedentRead.AldDemandeEnCours
        If antecedentRead.AldDateDemande < DteAldDateDemande.MinDate Or antecedentRead.AldDateDemande > DteAldDateDemande.MaxDate Then
            DteAldDateDemande.Value = Date.Now()
            antecedentRead.AldDateDemande = Date.Now()
            antecedentUpdate.AldDateDemande = Date.Now()
        Else
            DteAldDateDemande.Value = antecedentRead.AldDateDemande
        End If

        GestionAffichageZoneAld()
        GestionAffichageZoneAldCim10()
        GestionAffichageZoneDeclarationAld()
        GestionAffichageAldValide()
        GestionAffichageAldDemande()

        'Initialisation classe Historisation antecedent 
        AntecedentHistoCreationDao.InitAntecedentHistorisation(antecedentRead, userLog, AntecedentHistoACreer)
    End Sub

    'Suppression (annulation) de l'antécédent
    Private Sub BtnSupprimer_Click(sender As Object, e As EventArgs) Handles RadBtnSupprimer.Click
        If MsgBox("Attention, la suppression d'un antécédent est irréversible, confirmez-vous l'annulation", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            'Annulation antécédent
            If AnnulationAntecedent() = True Then
                Dim form As New RadFNotification()
                form.Message = "antécédent patient annulé"
                form.Show()
                Me.CodeRetour = True
                Close()
            Else
                Me.CodeRetour = False
            End If
        End If
    End Sub

    'Abandon et retour sur l'écran précédent
    Private Sub BtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Me.CodeRetour = False
        Close()
    End Sub

    'Validation en création ou modification
    Private Sub BtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        Select Case EditMode
            Case EnumEditMode.Creation
                If ControleSaisie() = True Then
                    If CreationAntecedent() = True Then
                        Me.CodeRetour = True
                        Close()
                    Else
                        Me.CodeRetour = False
                    End If
                End If

            Case EnumEditMode.Modification
                If ControleSaisie() = True Then
                    Dim Valide As Boolean = True
                    If Publication() = "O" Then
                        If MsgBox("Attention, un antécédent occulté ne sera plus accessible, confirmez-vous l'action d'occulter cet antécédent", MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation, "Avertissement") = MsgBoxResult.No Then
                            Valide = False
                        End If
                    End If
                    If Valide = True Then
                        If ModificationAntecedent() = True Then
                            Me.CodeRetour = True
                            Close()
                        Else
                            Me.CodeRetour = False
                        End If
                    End If
                End If
        End Select
    End Sub

    'Contrôle des données saisies
    Private Function ControleSaisie() As Boolean
        'Contrôle des données saisie
        Dim Valide As Boolean = True
        Dim MessageErreur1 As String = ""
        Dim MessageErreur2 As String = ""
        Dim MessageErreur3 As String = ""
        Dim MessageErreur4 As String = ""
        Dim MessageErreur5 As String = ""
        Dim MessageErreur As String = ""

        If TxtDrcId.Text = "" Then
            Valide = False
            MessageErreur1 = "Le code DRC est obligatoire"
        End If

        If TxtAntecedentDescription.Text.Trim() = "" Then
            Valide = False
            MessageErreur2 = "La description de l'antécédent est obligatoire"
        End If

        If DteDateDebut.Value > Date.Now() Then
            Valide = False
            MessageErreur3 = "La date de début de l'antécédent ne peut pas être supérieure à la date du jour"
        End If

        If ChkDiagnosticConfirme.Checked = False Then
            If ChkDiagnosticSuspecte.Checked = False Then
                If ChkDiagnosticNotion.Checked = False Then
                    Valide = False
                    MessageErreur4 = "Le diagnostic est obligatoire"
                End If
            End If
        End If

        'ALD
        If antecedentUpdate.AldId <> 0 And antecedentUpdate.AldCim10Id <> 0 Then
            'If AntecedentAldId <> 0 And AntecedentAldCim10Id <> 0 Then
            If RadChkAldValide.Checked = True Then
                If DteALDDateDebut.Value >= DteALDDateFin.Value Then
                    Valide = False
                    MessageErreur5 = "La date de fin de l'ALD doit être supérieure à la date de début"
                End If
            End If
        End If

        'Appel de la mise à jour des données
        If Valide = True Then
            Me.CodeRetour = True
        Else
            Me.CodeRetour = False
            'Préparation de l'affichage des erreurs
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

            If MessageErreur5 <> "" Then
                MessageErreur = MessageErreur & MessageErreur5 & vbCrLf
            End If
            If EnumEditMode.Creation Then
                MessageErreur = MessageErreur & vbCrLf & "/!\ Création de l'antécédent impossible, des données sont incorrectes"
            Else
                MessageErreur = MessageErreur & vbCrLf & "/!\ Mise à jour de l'antécédent impossible, des données sont incorrectes"
            End If
            MessageBox.Show(MessageErreur, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        Return CodeRetour
    End Function

    'Gestion des sélecteurs (DRC et ALDCIM10)
    Private Sub TxtDrcId_DoubleClick(sender As Object, e As EventArgs) Handles TxtDrcId.DoubleClick
        SelectDrc()
    End Sub


    Private Sub RadDrcSelect_Click(sender As Object, e As EventArgs) Handles RadBtnDrcSelect.Click
        SelectDrc()
    End Sub

    Private Sub SelectDrc()
        'Appel du sélecteur de DRC
        Using vFDrcSelecteur As New RadFDRCSelecteur
            vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
            vFDrcSelecteur.CategorieOasis = 1       'Catégorie "Antécédent et Contexte"
            vFDrcSelecteur.ShowDialog()             'Modal
            'Si une DRC a été sélectionné
            If vFDrcSelecteur.SelectedDrcId <> 0 Then
                Drc = vFDrcSelecteur.SelectedDrc
                TxtDrcId.Text = Drc.DrcId
                'Alimentation bean update
                antecedentUpdate.DrcId = Drc.DrcId
                LblDrcDenomination.Text = Drc.DrcLibelle
                LblDrcDenomination.ForeColor = Color.DarkBlue
                If Drc.AldId <> 0 Then
                    RadGbxAld.Show()
                    antecedentUpdate.AldId = Drc.AldId
                    antecedentUpdate.AldCim10Id = 0
                    TxtAldCode.Text = Drc.AldCode
                    LblAldDescription.Text = Drc.DrcLibelle
                    TxtAldCim10Code.Text = ""
                    Lblcim10Description.Text = ""
                    RadChkAldValide.Checked = False
                    RadChkAldDemandeEnCours.Checked = False
                Else
                    RadGbxAld.Hide()
                    antecedentUpdate.AldId = 0
                End If
            Else
                LblDrcDenomination.Text = ""
                antecedentUpdate.AldId = 0
            End If
            antecedentUpdate.AldCim10Id = 0
            RadChkAldValide.Checked = False
            RadChkAldDemandeEnCours.Checked = False
            GestionAffichageZoneAld()
            TxtAldCim10Code.Text = ""
            GestionAffichageZoneAldCim10()
            GestionAffichageZoneDeclarationAld()
            GestionAffichageBoutonValidation()
        End Using
    End Sub

    'Sélecteur DRC Cim10
    Private Sub RadBtnSelectionAldCim10_Click(sender As Object, e As EventArgs) Handles RadBtnSelectionAldCim10.Click
        SelectionAldCim10()
    End Sub

    Private Sub TxtAldCim10Id_DoubleClick(sender As Object, e As EventArgs) Handles TxtAldCim10Code.DoubleClick
        SelectionAldCim10()
    End Sub

    Private Sub SelectionAldCim10()
        Try
            Using vFAldCim10Selecteur As New RadFAldCim10Selecteur
                vFAldCim10Selecteur.UtilisateurConnecte = userLog
                vFAldCim10Selecteur.SelectedAldId = antecedentUpdate.AldId
                vFAldCim10Selecteur.ShowDialog() 'Modal
                'Si un code ALD CIM10 a été sélectionné
                If vFAldCim10Selecteur.SelectedAldCim10Id <> 0 Then
                    'AntecedentAldCim10Id = SelectedAldCim10Id
                    antecedentUpdate.AldCim10Id = vFAldCim10Selecteur.SelectedAldCim10Id
                    Dim aldCim10 As New AldCim10
                    aldCim10 = AldCim10Dao.GetAldCim10ById(vFAldCim10Selecteur.SelectedAldCim10Id)
                    TxtAldCim10Code.Text = aldCim10.AldCim10Code
                    Lblcim10Description.Text = aldCim10.AldCim10Description
                End If
            End Using
            GestionAffichageZoneAldCim10()
            GestionAffichageZoneDeclarationAld()
            GestionAffichageBoutonValidation()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub

    '======================================================
    'Gestion des implications des zones de saisie modifiées
    '======================================================
    Private Sub TxtDrcId_TextChanged(sender As Object, e As EventArgs) Handles TxtDrcId.TextChanged
        antecedentUpdate.DrcId = TxtDrcId.Text
        GestionAffichageBoutonValidation()
        TxtAldCode.Text = ""
        TxtAldCim10Code.Text = ""
    End Sub

    Private Sub TxtAntecedentDescription_TextChanged(sender As Object, e As EventArgs) Handles TxtAntecedentDescription.TextChanged
        antecedentUpdate.Description = TxtAntecedentDescription.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub DteDateDebut_ValueChanged(sender As Object, e As EventArgs) Handles DteDateDebut.ValueChanged
        antecedentUpdate.DateDebut = DteDateDebut.Value
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub DteDateDebut_DropDown(sender As Object, e As EventArgs) Handles DteDateDebut.DropDown
        If DteDateDebut.Value = DteDateDebut.MinDate Then
            DteDateDebut.Value = Date.Now()
            DteDateDebut.Format = DateTimePickerFormat.Long
        End If
    End Sub

    Private Sub CbxDiagnosticConfirme_CheckedChanged(sender As Object, e As EventArgs) Handles ChkDiagnosticConfirme.CheckedChanged
        If ChkDiagnosticConfirme.Checked = True Then
            ChkDiagnosticNotion.Checked = False
            ChkDiagnosticSuspecte.Checked = False
            ChkDiagnosticConfirme.ForeColor = Color.Red
            ChkDiagnosticNotion.ForeColor = Color.Black
            ChkDiagnosticSuspecte.ForeColor = Color.Black
        End If
        antecedentUpdate.Diagnostic = Diagnostic()
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub CbxDiagnosticSuspecte_CheckedChanged(sender As Object, e As EventArgs) Handles ChkDiagnosticSuspecte.CheckedChanged
        If ChkDiagnosticSuspecte.Checked = True Then
            ChkDiagnosticNotion.Checked = False
            ChkDiagnosticConfirme.Checked = False
            ChkDiagnosticSuspecte.ForeColor = Color.Red
            ChkDiagnosticNotion.ForeColor = Color.Black
            ChkDiagnosticConfirme.ForeColor = Color.Black
        End If
        antecedentUpdate.Diagnostic = Diagnostic()
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub CbxDiagnosticNotion_CheckedChanged(sender As Object, e As EventArgs) Handles ChkDiagnosticNotion.CheckedChanged
        If ChkDiagnosticNotion.Checked = True Then
            ChkDiagnosticConfirme.Checked = False
            ChkDiagnosticSuspecte.Checked = False
            ChkDiagnosticNotion.ForeColor = Color.Red
            ChkDiagnosticConfirme.ForeColor = Color.Black
            ChkDiagnosticSuspecte.ForeColor = Color.Black
        End If
        antecedentUpdate.Diagnostic = Diagnostic()
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub ChkPublie_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPublie.CheckedChanged
        If ChkPublie.Checked = True Then
            ChkCache.Checked = False
            ChkOcculte.Checked = False
            ChkPublie.ForeColor = Color.Red
            ChkCache.ForeColor = Color.Black
            ChkOcculte.ForeColor = Color.Black
        End If
        antecedentUpdate.StatutAffichage = Publication()
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub ChkCache_CheckedChanged(sender As Object, e As EventArgs) Handles ChkCache.CheckedChanged
        If ChkCache.Checked = True Then
            ChkPublie.Checked = False
            ChkOcculte.Checked = False
            ChkCache.ForeColor = Color.Red
            ChkPublie.ForeColor = Color.Black
            ChkOcculte.ForeColor = Color.Black
        End If
        antecedentUpdate.StatutAffichage = Publication()
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub ChkOcculte_CheckedChanged(sender As Object, e As EventArgs) Handles ChkOcculte.CheckedChanged
        If ChkOcculte.Checked = True Then
            ChkPublie.Checked = False
            ChkCache.Checked = False
            ChkOcculte.ForeColor = Color.Red
            ChkPublie.ForeColor = Color.Black
            ChkCache.ForeColor = Color.Black
        End If
        antecedentUpdate.StatutAffichage = Publication()
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub RadChkAldValide_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkAldValide.ToggleStateChanged
        GestionAffichageAldValide()
        antecedentUpdate.AldValide = RadChkAldValide.Checked
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub RadChkAldDemandeEnCours_CheckStateChanged(sender As Object, e As EventArgs) Handles RadChkAldDemandeEnCours.CheckStateChanged
        GestionAffichageAldDemande()
        antecedentUpdate.AldDemandeEnCours = RadChkAldDemandeEnCours.Checked
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub DteALDDateDebut_ValueChanged(sender As Object, e As EventArgs) Handles DteALDDateDebut.ValueChanged
        antecedentUpdate.AldDateDebut = DteALDDateDebut.Value
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub DteALDDateFin_ValueChanged(sender As Object, e As EventArgs) Handles DteALDDateFin.ValueChanged
        antecedentUpdate.AldDateFin = DteALDDateFin.Value
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub DteAldDateDemande_ValueChanged(sender As Object, e As EventArgs) Handles DteAldDateDemande.ValueChanged
        antecedentUpdate.AldDateDemande = DteAldDateDemande.Value
        GestionAffichageBoutonValidation()
    End Sub

    '============================================================================

    'Gestion de l'affichage de l'ALD

    Private Sub GestionAffichageAldDemande()
        If RadChkAldDemandeEnCours.Checked = True Then
            LblAldDateDemandeEnCours.Show()
            DteAldDateDemande.Show()
            GestionAffichageBoutonValidation()
            LockDiagnostic()
        Else
            LblAldDateDemandeEnCours.Hide()
            DteAldDateDemande.Hide()
            'Initialisation date demande
            antecedentUpdate.AldDateDemande = Date.Now()
            antecedentRead.AldDateDemande = Date.Now()
            DteAldDateDemande.Value = Date.Now()
            If RadChkAldValide.Checked = False Then
                UnlockDiagnostic()
            End If
        End If
    End Sub

    Private Sub GestionAffichageAldValide()
        If RadChkAldValide.Checked = True Then
            LblALDDateDebut.Show()
            DteALDDateDebut.Show()
            LblALDDateFin.Show()
            DteALDDateFin.Show()
            GestionAffichageBoutonValidation()
            LockDiagnostic()
        Else
            LblALDDateDebut.Hide()
            DteALDDateDebut.Hide()
            antecedentUpdate.AldDateDebut = Date.Now()
            antecedentRead.AldDateDebut = Date.Now()
            DteALDDateDebut.Value = Date.Now()
            LblALDDateFin.Hide()
            DteALDDateFin.Hide()
            antecedentUpdate.AldDateFin = Date.Now()
            antecedentRead.AldDateFin = Date.Now()
            DteALDDateFin.Value = Date.Now()
            If RadChkAldDemandeEnCours.Checked = False Then
                UnlockDiagnostic()
            End If
        End If
    End Sub

    Private Sub GestionAffichageZoneAld()
        If antecedentUpdate.AldId = 31 Or antecedentUpdate.AldId = 0 Then
            TxtAldCim10Code.Hide()
            Lblcim10Description.Hide()
            TxtAldCim10Code.Text = ""
        Else
            TxtAldCim10Code.Show()
            Lblcim10Description.Show()
            LblAldDescription.Text = Table_ald.GetAldDescription(antecedentUpdate.AldId)
        End If
    End Sub

    Private Sub GestionAffichageZoneAldCim10()
        If antecedentUpdate.AldCim10Id <> 0 Then
            Dim aldCim10 As AldCim10 = New AldCim10(antecedentUpdate.AldCim10Id)
            Lblcim10Description.Text = aldCim10.AldCim10Description
            Lblcim10Description.Show()
        Else
            Lblcim10Description.Text = ""
        End If
    End Sub

    Private Sub GestionAffichageZoneDeclarationAld()
        Dim affichageDeclarationAld As Boolean = True
        If antecedentUpdate.Niveau = 1 Then
            If antecedentUpdate.AldId = 31 Or antecedentUpdate.AldCim10Id <> 0 Then
                LblLabelAldValide.Show()
                RadChkAldValide.Show()
                If RadChkAldValide.Checked = True Then
                    LblALDDateDebut.Show()
                    DteALDDateDebut.Show()
                    LblALDDateFin.Show()
                    DteALDDateFin.Show()
                End If
                LblLabelAldDemandeEnCours.Show()
                RadChkAldDemandeEnCours.Show()
                If RadChkAldDemandeEnCours.Checked = True Then
                    LblAldDateDemandeEnCours.Show()
                    DteAldDateDemande.Show()
                End If
            Else
                affichageDeclarationAld = False
            End If
        Else
            affichageDeclarationAld = False
        End If

        If affichageDeclarationAld = False Then
            LblLabelAldValide.Hide()
            RadChkAldValide.Hide()
            LblALDDateDebut.Hide()
            DteALDDateDebut.Hide()
            LblALDDateFin.Hide()
            DteALDDateFin.Hide()
            LblLabelAldDemandeEnCours.Hide()
            RadChkAldDemandeEnCours.Hide()
            LblAldDateDemandeEnCours.Hide()
            DteAldDateDemande.Hide()
        End If

    End Sub

    Private Sub GestionAffichageBoutonValidation()
        If EditMode = EnumEditMode.Modification Then
            If AntecedentDao.Compare(antecedentUpdate, antecedentRead) = False Then
                RadBtnValidation.Enabled = True
            Else
                RadBtnValidation.Enabled = False
            End If
        End If
    End Sub

    Private Function Diagnostic() As Integer
        If ChkDiagnosticConfirme.Checked = True Then
            Return 1
        Else
            If ChkDiagnosticSuspecte.Checked = True Then
                Return 2
            Else
                If ChkDiagnosticNotion.Checked = True Then
                    Return 3
                End If
            End If
        End If

        Return 0
    End Function


    Private Function Publication() As String
        If ChkCache.Checked = True Then
            Return "C"
        Else
            If ChkOcculte.Checked = True Then
                Return "O"
            Else
                Return "P"
            End If
        End If
        Return ""
    End Function

    '=============================================================================================
    '==================================== Mise à jour de la base de données ======================
    '=============================================================================================

    'Modification d'un antécédent
    Private Function ModificationAntecedent() As Boolean
        Dim codeRetour As Boolean = True

        If AntecedentDao.ModificationAntecedent(antecedentUpdate, antecedentRead) = True Then
            Dim form As New RadFNotification()
            form.Message = "Antécédent patient modifié"
            form.Show()
        Else
            codeRetour = False
        End If

        Return codeRetour
    End Function

    'Annulation d'un antécédent
    Private Function AnnulationAntecedent() As Boolean
        Dim codeRetour As Boolean = True

        If AntecedentDao.AnnulationAntecedent(antecedentUpdate, antecedentRead) = True Then
            Dim form As New RadFNotification()
            form.Message = "Antécédent patient annulé"
            form.Show()
        Else
            codeRetour = False
        End If

        Return codeRetour
    End Function

    'Création d'un antecedent en base de données
    Private Function CreationAntecedent() As Boolean
        Dim codeRetour As Boolean = True

        If AntecedentDao.CreationAntecedent(antecedentUpdate) = True Then
            Dim form As New RadFNotification()
            form.Message = "Antécédent patient créé"
            form.Show()
        Else
            codeRetour = False
        End If

        Return codeRetour
    End Function


    '=============================================================================================
    '==================================== Gestion de l'affichage des zones d'écran ===============
    '=============================================================================================

    'Initialisation des zones de saisie
    Private Sub InitZone()
        Me.Reactivation = False
        TxtDrcId.Text = ""
        TxtAntecedentDescription.Text = ""
        DteDateDebut.Value = "01/01/1900"
        ChkCache.Checked = False
        ChkOcculte.Checked = False
        ChkPublie.Checked = False
        'RadBtnRecupereDrc.Hide()
    End Sub

    'Récupère la dénomination pour alimenter la description de l'antécédent
    Private Sub RadBtnRecupereDrc_Click(sender As Object, e As EventArgs) Handles RadBtnRecupereDrc.Click
        If DteDateDebut.Value = DteDateDebut.MinDate Then
            TxtAntecedentDescription.Text = LblDrcDenomination.Text
        Else
            TxtAntecedentDescription.Text = LblDrcDenomination.Text & " (" & DteDateDebut.Value.ToString("MM.yyyy") & ")"
        End If
    End Sub


    Private Sub RadBtnCopierALD_Click(sender As Object, e As EventArgs) Handles RadBtnCopierALD.Click
        If DteDateDebut.Value = DteDateDebut.MinDate Then
            TxtAntecedentDescription.Text = Lblcim10Description.Text
        Else
            TxtAntecedentDescription.Text = Lblcim10Description.Text & " (" & DteDateDebut.Value.ToString("MM.yyyy") & ")"
        End If
    End Sub

    Private Sub LockDiagnostic()
        ChkDiagnosticConfirme.Checked = True
        ChkDiagnosticConfirme.Enabled = False
        ChkDiagnosticNotion.Enabled = False
        ChkDiagnosticSuspecte.Enabled = False

        ChkPublie.Checked = True
        ChkPublie.Enabled = False
        ChkCache.Enabled = False
        ChkOcculte.Enabled = False
    End Sub

    Private Sub UnlockDiagnostic()
        ChkDiagnosticConfirme.Enabled = True
        ChkDiagnosticNotion.Enabled = True
        ChkDiagnosticSuspecte.Enabled = True
        ChkPublie.Enabled = True
        ChkCache.Enabled = True
        ChkOcculte.Enabled = True
    End Sub

    Private Sub LblId_MouseHover(sender As Object, e As EventArgs) Handles LblId.MouseHover
        ToolTip1.SetToolTip(LblId, "Id : " + SelectedAntecedentId.ToString)
    End Sub

    Private Sub DroitAcces()
        'Si l'utilisateur n'a pas les droits requis ou que le traitement a été arrêté, les zones de saisie ne sont pas modifiables 
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            InhiberZonesDeSaisie()
            RadBtnValidation.Hide()
            RadBtnSupprimer.Hide()
            RadBtnRecupereDrc.Hide()
        End If
    End Sub

    'Inhiber les zones de saisie
    Private Sub InhiberZonesDeSaisie()
        TxtDrcId.Enabled = False
        TxtAntecedentDescription.Enabled = False
        DteDateDebut.Enabled = False
        ChkCache.Enabled = False
        ChkOcculte.Enabled = False
        ChkPublie.Enabled = False
        ChkDiagnosticConfirme.Enabled = False
        ChkDiagnosticSuspecte.Enabled = False
        ChkDiagnosticNotion.Enabled = False
        RadBtnValidation.Show()
        RadBtnDrcSelect.Enabled = False
    End Sub

    Private Sub RadBtnHistorique_Click(sender As Object, e As EventArgs) Handles RadBtnHistorique.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vFAntecedenttHistoListe As New RadFAntecedentHistoListe
                vFAntecedenttHistoListe.SelectedAntecedentId = SelectedAntecedentId
                vFAntecedenttHistoListe.SelectedPatient = Me.SelectedPatient
                vFAntecedenttHistoListe.UtilisateurConnecte = userLog
                vFAntecedenttHistoListe.ShowDialog()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try

        Me.Enabled = True
    End Sub

End Class
