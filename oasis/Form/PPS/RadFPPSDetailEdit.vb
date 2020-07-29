Imports System.Data.SqlClient
Imports Oasis_Common
Public Class RadFPPSDetailEdit
    Private _SelectedPatient As Patient
    Private _UtilisateurConnecte As Utilisateur
    Private _CategoriePPS As Integer
    Private _PPSId As Integer
    Private _codeRetour As Boolean
    Private _positionGaucheDroite As Integer

    Public Property SelectedPatient As Patient
        Get
            Return _SelectedPatient
        End Get
        Set(value As Patient)
            _SelectedPatient = value
        End Set
    End Property

    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return _UtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            _UtilisateurConnecte = value
        End Set
    End Property

    Public Property PPSId As Integer
        Get
            Return _PPSId
        End Get
        Set(value As Integer)
            _PPSId = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return _codeRetour
        End Get
        Set(value As Boolean)
            _codeRetour = value
        End Set
    End Property

    Public Property CategoriePPS As Integer
        Get
            Return _CategoriePPS
        End Get
        Set(value As Integer)
            _CategoriePPS = value
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
        Creation = 1
        Modification = 2
    End Enum

    Dim conxn As New SqlConnection(getConnectionString())
    Dim ObjectifExiste As Boolean = False
    Dim PPSHistoACreer As New PpsHisto
    Dim EditMode As Integer
    Dim UtilisateurHisto As Utilisateur = New Utilisateur()
    Dim PPSDesignation As String
    Dim SousCategoriePPs As Integer

    Dim PPSRead As New Pps
    Dim PPSUpdate As New Pps

    Dim ppsDao As New PpsDao
    Dim drcdao As New DrcDao

    Private Sub RadFPPSMesurePreventive_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If PositionGaucheDroite = EnumPosition.Droite Then
            Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - Me.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        Else
            Me.Location = New Point(10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        End If

        Select Case CategoriePPS
            Case EnumCategoriePPS.MesurePreventive
                Me.Text = "Mesure préventive"
                PPSDesignation = "Mesure préventive"
                SousCategoriePPs = 2
                CbxTypeStrategie.Hide()
                LblTypeStrategie.Hide()
            Case EnumCategoriePPS.Objectif
                Me.Text = "Objectif"
                PPSDesignation = "Objectif"
                SousCategoriePPs = 1
                NumPriorite.Hide()
                LblPriorite.Hide()
                CbxTypeStrategie.Hide()
                LblTypeStrategie.Hide()
            Case EnumCategoriePPS.Strategie
                Me.Text = "Stratégie contextuelle"
                PPSDesignation = "Stratégie contextuelle"
                NumPriorite.Hide()
                LblPriorite.Hide()
        End Select
        Dim conxn As New SqlConnection(GetConnectionString())
        DroitAcces()
        ChargementEtatCivil()

        RadBtnConfirmationAnnulation.Hide()
        TxtCommentaireArret.Hide()
        LblLabelCommentaireArret.Hide()
        RadGbxAnnulation.Hide()

        If PPSId <> 0 Then
            '-- Modification
            EditMode = EnumEditMode.Modification
            ChargementPPS()
        Else
            '-- Création
            EditMode = EnumEditMode.Creation
            InitZonesEnSaisie()
            RadBtnAnnulation.Hide()
            'Cacher les éléments de création de l'occurrence
            LblLabelStrategieDateModification.Hide()
            LblStrategieDateModification.Hide()
            LblLabelStrategieParModification.Hide()
            LblUtilisateurModification.Hide()
            'Cacher les éléments de modification de l'occurrence
            LblLabelStrategieDateCreation.Hide()
            LblStrategieDateCreation.Hide()
            LblLabelStrategieParCreation.Hide()
            LblUtilisateurCreation.Hide()
            'Cacher les boutons d'action
            RadBtnHistorique.Hide()
        End If
        CodeRetour = False

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub InitZonesEnSaisie()
        TxtDrcId.Text = ""
        TxtCommentaire.Text = ""
        NumPriorite.Value = 0
    End Sub

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
        'Recherche si ALD
        LblALD.Hide()

        'Vérification de l'existence d'ALD
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTipPPS.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    Private Sub ChargementPPS()
        PPSRead = ppsDao.getPpsById(PPSId)
        PPSUpdate = PPSRead.Clone()
        Dim dateCreation, dateModification As Date

        TxtCommentaire.Text = PPSUpdate.Commentaire

        TxtDrcId.Text = PPSUpdate.DrcId
        Dim Drc As Drc = New Drc()
        If drcdao.GetDrc(Drc, TxtDrcId.Text) = True Then
            TxtDrcDescription.Text = Drc.DrcLibelle
        Else
            TxtDrcDescription.Text = ""
        End If

        SousCategoriePPs = PPSUpdate.SousCategorieId
        Select Case PPSUpdate.CategorieId
            Case EnumCategoriePPS.Objectif
            Case EnumCategoriePPS.MesurePreventive
            Case EnumCategoriePPS.Strategie
                Select Case PPSUpdate.SousCategorieId
                    Case 7
                        CbxTypeStrategie.Text = Pps.EnumSousCategoriePPS.Prophylactique.ToString
                    Case 8
                        CbxTypeStrategie.Text = Pps.EnumSousCategoriePPS.Sociale.ToString
                    Case 9
                        CbxTypeStrategie.Text = Pps.EnumSousCategoriePPS.Symptomatique.ToString
                    Case 10
                        CbxTypeStrategie.Text = Pps.EnumSousCategoriePPS.Curative.ToString
                    Case 11
                        CbxTypeStrategie.Text = Pps.EnumSousCategoriePPS.Diagnostique.ToString
                    Case 12
                        CbxTypeStrategie.Text = Pps.EnumSousCategoriePPS.Palliative.ToString
                    Case Else
                        CbxTypeStrategie.Text = ""
                End Select
        End Select

        NumPriorite.Value = PPSUpdate.Priorite

        If PPSUpdate.DateCreation <> Nothing Then
            dateCreation = PPSUpdate.DateCreation
            LblStrategieDateCreation.Text = dateCreation.ToString("dd.MM.yyyy")
        Else
            LblStrategieDateCreation.Text = ""
            LblLabelStrategieDateCreation.Hide()
            LblLabelStrategieParCreation.Hide()
        End If

        LblUtilisateurCreation.Text = ""
        If PPSUpdate.UserCreation <> 0 Then
            Dim userDao As New UserDao
            UtilisateurHisto = userDao.getUserById(PPSUpdate.UserCreation)
            LblUtilisateurCreation.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
        End If

        If PPSUpdate.DateModification <> Nothing Then
            dateModification = PPSUpdate.DateModification
            LblStrategieDateModification.Text = dateModification.ToString("dd.MM.yyyy")
        Else
            LblStrategieDateModification.Text = ""
            LblLabelStrategieDateModification.Hide()
            LblLabelStrategieParModification.Hide()
        End If

        LblUtilisateurModification.Text = ""
        If PPSUpdate.UserModification <> 0 Then
            Dim userDao As New UserDao
            UtilisateurHisto = userDao.getUserById(PPSUpdate.UserModification)
            LblUtilisateurModification.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
        End If

        RadBtnValidation.Enabled = False
    End Sub

    Private Sub TxtDrcId_DoubleClick(sender As Object, e As EventArgs) Handles TxtDrcId.DoubleClick
        AppelDrcSelecteur()
    End Sub
    Private Sub RadBtnDrcSelecteur_Click(sender As Object, e As EventArgs) Handles RadBtnDrcSelecteur.Click
        AppelDrcSelecteur()
    End Sub

    Private Sub AppelDrcSelecteur()
        Dim CategorieOasis As Integer
        CategorieOasis = drcdao.GetCategorieOasisByCategoriePPS(CategoriePPS)

        Using vFDrcSelecteur As New RadFDRCSelecteur
            vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
            vFDrcSelecteur.CategorieOasis = CategorieOasis
            vFDrcSelecteur.ShowDialog() 'Modal
            Dim SelectedDrcId As Integer = vFDrcSelecteur.SelectedDrcId
            vFDrcSelecteur.Dispose()        'Si un médicament a été sélectionné
            If SelectedDrcId <> 0 Then
                TxtDrcId.Text = SelectedDrcId
                Dim Drc As Drc = New Drc()
                If drcdao.GetDrc(Drc, SelectedDrcId) = True Then
                    TxtDrcDescription.Text = Drc.DrcLibelle
                Else
                    TxtDrcDescription.Text = ""
                End If
            End If
        End Using
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        If ControleValidationDonnees() = True Then
            If PPSId <> 0 Then
                'Modification
                If ModificationPPS() = True Then
                    Me.CodeRetour = True
                    Close()
                End If
            Else
                'Création
                If CreationPPS() = True Then
                    Me.CodeRetour = True
                    Close()
                End If
            End If
            RadBtnValidation.Enabled = False
        End If
    End Sub

    Private Function ControleValidationDonnees() As Boolean
        Dim ValidationDonnees As Boolean = True

        Dim MessageErreur1 As String = ""
        Dim MessageErreur2 As String = ""
        Dim MessageErreur3 As String = ""
        Dim MessageErreur4 As String = ""
        Dim MessageErreur As String = ""

        If TxtDrcId.Text = "" Then
            ValidationDonnees = False
            MessageErreur1 = "Le code DRC est obligatoire"
        End If

        If CategoriePPS = EnumCategoriePPS.Strategie Then
            SousCategoriePPs = DeterminationTypeStrategie()
            If SousCategoriePPs = 0 Then
                ValidationDonnees = False
                MessageErreur2 = "Le type de la stratégie contextuelle doit être sélectionné"
            End If
        End If

        'Appel de la mise à jour des données
        'Préparation de l'affichage des erreurs
        If ValidationDonnees = False Then
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

            MessageErreur = MessageErreur & vbCrLf & "/!\ Création " & PPSDesignation & " impossible, des données sont incorrectes"
            MessageBox.Show(MessageErreur, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        Return ValidationDonnees
    End Function
    Private Function CreationPPS() As Boolean
        Dim codeRetour As Boolean = True

        PPSUpdate = New Pps
        PPSUpdate.PatientId = SelectedPatient.patientId
        PPSUpdate.CategorieId = CategoriePPS
        If CategoriePPS = EnumCategoriePPS.Strategie Then
            PPSUpdate.SousCategorieId = DeterminationTypeStrategie()
        Else
            PPSUpdate.SousCategorieId = SousCategoriePPs
        End If
        PPSUpdate.DrcId = TxtDrcId.Text
        PPSUpdate.Priorite = NumPriorite.Value
        PPSUpdate.Commentaire = TxtCommentaire.Text
        PPSUpdate.DateCreation = Date.Now()
        PPSUpdate.UserCreation = userLog.UtilisateurId
        PPSUpdate.AffichageSynthese = True
        PPSUpdate.Inactif = False

        If ppsDao.CreationPPS(PPSUpdate, userLog) = True Then
            Dim form As New RadFNotification()
            form.Message = PPSDesignation & " créée"
            form.Show()
        Else
            codeRetour = False
        End If

        Return codeRetour
    End Function

    Private Function ModificationPPS() As Boolean
        Dim codeRetour As Boolean = True

        If ppsDao.ModificationPPS(PPSUpdate, userLog) = True Then
            Dim form As New RadFNotification With {
                .Message = PPSDesignation & " modifiée"
            }
            form.Show()
        Else
            codeRetour = False
        End If

        Return codeRetour
    End Function


    Private Function AnnulationPrevention() As Boolean
        Dim codeRetour As Boolean = True

        If ppsDao.AnnulationPrevention(PPSUpdate, userLog) = True Then
            Dim form As New RadFNotification()
            form.Message = PPSDesignation & " annulée"
            form.Show()
        Else
            codeRetour = False
        End If

        Return codeRetour
    End Function

    Private Sub RadBtnAnnulation_Click(sender As Object, e As EventArgs) Handles RadBtnAnnulation.Click
        RadGbxAnnulation.Show()
        TxtCommentaireArret.Show()
        LblLabelCommentaireArret.Show()
        RadBtnConfirmationAnnulation.Show()
        RadBtnAnnulation.Hide()
        RadBtnValidation.Enabled = False
        TxtDrcId.Enabled = False
        TxtCommentaire.Enabled = False
        NumPriorite.Enabled = False
    End Sub

    'Confirmation de l'annulation de la stratégie
    Private Sub RadBtnConfirmationAnnulation_Click(sender As Object, e As EventArgs) Handles RadBtnConfirmationAnnulation.Click
        If MsgBox("Confirmation de l'annulation de la " & PPSDesignation, MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            'Annulation antécédent
            If AnnulationPrevention() = True Then
                Me.CodeRetour = True
                Close()
            End If
        End If
    End Sub

    Private Sub NumPriorite_ValueChanged(sender As Object, e As EventArgs) Handles NumPriorite.ValueChanged
        'RadBtnValidation.Enabled = True
        PPSUpdate.Priorite = NumPriorite.Value
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtDrcId_TextChanged(sender As Object, e As EventArgs) Handles TxtDrcId.TextChanged
        'RadBtnValidation.Enabled = True
        PPSUpdate.DrcId = TxtDrcId.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtCommentaire_TextChanged(sender As Object, e As EventArgs) Handles TxtCommentaire.TextChanged
        'RadBtnValidation.Enabled = True
        PPSUpdate.Commentaire = TxtCommentaire.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub CbxTypeStrategie_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxTypeStrategie.SelectedIndexChanged
        'RadBtnValidation.Enabled = True
        PPSUpdate.SousCategorieId = DeterminationTypeStrategie()
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub LblId_MouseHover(sender As Object, e As EventArgs) Handles LblId.MouseHover
        ToolTipPPS.SetToolTip(LblId, "Id : " + PPSId.ToString)
    End Sub

    Private Sub GestionAffichageBoutonValidation()
        If EditMode = EnumEditMode.Modification Then
            If ppsDao.Compare(PPSUpdate, PPSRead) = False Then
                RadBtnValidation.Enabled = True
            Else
                RadBtnValidation.Enabled = False
            End If
        End If
    End Sub

    Private Function DeterminationTypeStrategie() As Integer
        Dim typeStrategie As Integer

        Select Case CbxTypeStrategie.Text
            Case "Prophylactique"
                typeStrategie = Pps.EnumSousCategoriePPS.Prophylactique
            Case "Sociale"
                typeStrategie = Pps.EnumSousCategoriePPS.Sociale
            Case "Symptomatique"
                typeStrategie = Pps.EnumSousCategoriePPS.Symptomatique
            Case "Curative"
                typeStrategie = Pps.EnumSousCategoriePPS.Curative
            Case "Diagnostique"
                typeStrategie = Pps.EnumSousCategoriePPS.Diagnostique
            Case "Palliative"
                typeStrategie = Pps.EnumSousCategoriePPS.Palliative
            Case Else
                typeStrategie = 0
        End Select

        Return typeStrategie

    End Function

    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Close()
    End Sub

    Private Sub RadBtnRecupereDrc_Click(sender As Object, e As EventArgs) Handles RadBtnRecupereDrc.Click
        TxtCommentaire.Text = TxtDrcDescription.Text
    End Sub

    Private Sub DroitAcces()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            RadBtnConfirmationAnnulation.Hide()
            RadBtnAnnulation.Hide()
            RadBtnConfirmationAnnulation.Hide()
            RadBtnDrcSelecteur.Hide()
            RadBtnRecupereDrc.Hide()
            RadBtnValidation.Hide()
            CbxTypeStrategie.Enabled = False
            TxtCommentaire.Enabled = False
            NumPriorite.Enabled = False
            TxtCommentaireArret.Enabled = False
        End If
    End Sub

    Private Sub RadBtnHistorique_Click(sender As Object, e As EventArgs) Handles RadBtnHistorique.Click
        Cursor.Current = Cursors.WaitCursor
        Try
            Using vFPPSHistoListe As New RadFPPSHistoListe
                vFPPSHistoListe.SelectedPPSId = PPSId
                vFPPSHistoListe.SelectedPatient = Me.SelectedPatient
                vFPPSHistoListe.UtilisateurConnecte = userLog
                vFPPSHistoListe.ShowDialog()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
        Me.Enabled = True
    End Sub
End Class
