﻿Imports System.Collections.Specialized
Imports System.Configuration
Imports Telerik.WinControls
Imports Telerik.WinControls.UI

Public Class RadFEpisodeDetail
    Private _SelectedEpisodeId As Long
    Private _SelectedPatient As Patient
    Private _rendezVousId As Long
    Private _isRendezVousCloture As Boolean
    Private _UtilisateurConnecte As Utilisateur

    Public Property SelectedEpisodeId As Long
        Get
            Return _SelectedEpisodeId
        End Get
        Set(value As Long)
            _SelectedEpisodeId = value
        End Set
    End Property

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

    Public Property RendezVousId As Long
        Get
            Return _rendezVousId
        End Get
        Set(value As Long)
            _rendezVousId = value
        End Set
    End Property

    Public Property IsRendezVousCloture As Boolean
        Get
            Return _isRendezVousCloture
        End Get
        Set(value As Boolean)
            _isRendezVousCloture = value
        End Set
    End Property

    Dim logDao As New LogDao
    Dim episodeDao As New EpisodeDao
    Dim tacheDao As New TacheDao
    Dim fonctionDao As New FonctionDao
    Dim parcoursDao As New ParcoursDao
    Dim ParcoursConsigneDao As New ParcoursConsigneDao
    Dim UserDao As New UserDao
    Dim episodeProtocoleCollaboratifDao As New EpisodeProtocoleCollaboratifDao
    Dim episodeParametreDao As New EpisodeParametreDao
    Dim episodeActeParamedicalDao As New EpisodeActeParamedicalDao

    Dim log As Log
    Dim episode As Episode
    Dim tache As Tache
    Dim user As New Utilisateur
    Dim utilisateurHisto As Utilisateur = New Utilisateur()

    Dim ParcoursListProfilsOasis As New List(Of Integer)
    Dim ListeParametreExistant As New List(Of Long)

    Dim InitPublie, InitParPriorite, InitMajeur, InitContextePublie, InitParcoursNonCache As Boolean
    Dim PPSSuiviIdeExiste, PPSSuiviSageFemmeExiste, PPSSuiviMedecinExiste As Boolean
    Dim Allergie, ContreIndication As Boolean
    Dim WorkflowExistant As Boolean
    Dim ObservationMedicaleModifie As Boolean = False
    Dim ObservationParamedicaleModifie As Boolean = False
    Dim IsTraitementLoaded As Boolean = False
    Dim IsParcoursLoaded As Boolean = False
    Dim IsContexteLoaded As Boolean = False
    Dim IsPPSLoaded As Boolean = False
    Dim FinChargementActesParamedicauxParamedical As Boolean = False
    Dim FinChargementActesParamedicauxMedical As Boolean = False

    Dim LongueurStringAllergie As Integer

    Dim TypeEpisode, typeActiviteEpisode, typeProfilEpisode, DescriptionActiviteEpisode, CommentaireEpisode, UserCreation, DateCreation, UserModification, DateModification As String

    Private Sub RadFEpisodeDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitParametre()
        LblTypeEpisode.Text = ""
        LblTypeProfil.Text = ""
        LblDuree.Text = ""
        LblEpisodeCommentaire.Text = ""
        LblUserCreation.Text = ""
        LBlUserModification.Text = ""
        LblDateModification.Text = ""
        LblPatientNIR.Text = ""
        LblPatientPrenom.Text = ""
        LblPatientNom.Text = ""
        LblPatientAge.Text = ""
        LblPatientGenre.Text = ""
        LblPatientSite.Text = ""
        LblPatientDateMaj.Text = ""

        afficheTitleForm(Me, "Détail épisode")
        Me.WindowState = FormWindowState.Maximized

        RadGridViewObsIde.TableElement.RowHeight = 35
        RadGridViewObsMed.TableElement.RowHeight = 35

        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.TextAndButtonsElement.TextElement.ForeColor = Color.Red
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.BackColor = Color.DarkBlue
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.Font = New Font("Arial", 8.0F, FontStyle.Italic)
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.TextImageRelation = TextImageRelation.TextBeforeImage
        Me.RadDesktopAlert1.Popup.AlertElement.BackColor = Color.MistyRose
        Me.RadDesktopAlert1.Popup.AlertElement.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.BorderColor = Color.DarkBlue

        ClotureRendezVous()
        ChargementParametreApplication()
        initZones()
        ChargementEtatCivil()

        'Episode
        ChargementAffichageBlocWorkflow()
        ChargementCaracteristiquesEpisode()
        If episode.TypeActivite = EpisodeDao.EnumTypeActiviteEpisodeCode.SOCIAL Or episode.Type = EpisodeDao.EnumTypeEpisode.VIRTUEL.ToString Then
            'Me.RadSplitContainer4.EnableCollapsing = True
            'Me.RadSplitContainer4.UseSplitterButtons = True
            'Me.RadSplitContainer4.MoveSplitter(Me.RadSplitContainer4.Splitters(0), RadDirection.Up)
            'SplitPanelConsigneParamedicale.Hide()
            SplitPanel7.Hide()
            Me.RadSplitContainer2.EnableCollapsing = True
            Me.RadSplitContainer2.UseSplitterButtons = True
            Me.RadSplitContainer2.MoveSplitter(Me.RadSplitContainer2.Splitters(0), RadDirection.Up)
            SplitPanel4.Hide()
            Me.RadSplitContainer3.EnableCollapsing = True
            Me.RadSplitContainer3.UseSplitterButtons = True
            Me.RadSplitContainer3.MoveSplitter(Me.RadSplitContainer3.Splitters(0), RadDirection.Up)

            RadObsSpeParDataGridView.Hide()
            RadBtnParametre.Hide()
            RadGbxParametre.Text = ""
        Else
            ChargementEpisodeActesParamedicauxParamedical()
            ChargementEpisodeActesParamedicauxMedical()
            ChargementParametres()
        End If

        ChargementObservationLibre()


        'Synthèse, chargement de la page par défaut
        ChargementAntecedent()

        DroitAcces()

        Cursor.Current = Cursors.Default
    End Sub

    'Chargement des pages de la synthèse en fonction de leur demande (sauf antécédent qui est la page par défaut)
    Private Sub RadPageView1_SelectedPageChanged(sender As Object, e As EventArgs) Handles RadPageView1.SelectedPageChanged
        'Console.WriteLine("Page sélectionnée : " & RadPageView1.SelectedPage.Name)
        Select Case RadPageView1.SelectedPage.Name
            Case "PgvTraitement"
                If IsTraitementLoaded = False Then
                    ChargementTraitement()
                    IsTraitementLoaded = True
                    'Console.WriteLine("Chargement page : " & RadPageView1.SelectedPage.Name)
                End If
            Case "PgvParcours"
                If IsParcoursLoaded = False Then
                    ChargementParcoursDeSoin()
                    IsParcoursLoaded = True
                    'Console.WriteLine("Chargement page : " & RadPageView1.SelectedPage.Name)
                End If
            Case "PgvContexte"
                If IsContexteLoaded = False Then
                    ChargementContexte()
                    IsContexteLoaded = True
                    'Console.WriteLine("Chargement page : " & RadPageView1.SelectedPage.Name)
                End If
            Case "PgvPPS"
                If IsPPSLoaded = False Then
                    ChargementPPS()
                    IsPPSLoaded = True
                    'Console.WriteLine("Chargement page : " & RadPageView1.SelectedPage.Name)
                End If
        End Select
    End Sub

    Private Sub ClotureRendezVous()
        Me.IsRendezVousCloture = False
        If Me.RendezVousId <> 0 Then
            Dim tacheRendezVous As Tache
            tacheRendezVous = tacheDao.getTacheById(RendezVousId)
            If tacheRendezVous.isUnRdv Then
                'Controler que l'utilisateur est celui qui s'est attribué la tâche
                If tacheRendezVous.isMyTacheATraiter = False Then
                    If tacheRendezVous.isAttribuable Then
                        'Tâche non encore attribuée et l'utilisateur n'est pas encore propriétaire du rendez-vous, proposition d'attribution de la tâche à l'utilisateur
                        If MsgBox("Vous allez vous attribuer le traitement du rendez-vous qui sera clôturé à la sortie de l'épisode. Confirmez-vous son attribution", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
                            Try
                                tacheDao.attribueTacheToUserLog(tacheRendezVous.Id)
                            Catch ex As Exception
                                MessageBox.Show(ex.ToString)
                                Exit Sub
                            End Try
                        Else
                            Exit Sub
                        End If
                    Else
                        Exit Sub
                    End If
                End If
                Try
                    If tacheDao.ClotureTache(RendezVousId, True) = True Then
                        Me.IsRendezVousCloture = True
                    End If
                Catch ex As Exception
                    If ex.ToString.StartsWith("Collision") Then
                        Dim Description As String = "Traitement de clôture annulé, le rendez-vous (tâche n° " & Me.RendezVousId.ToString & ") était déjà clôturé"
                        CreateLog(Description, Me.Name, LogDao.EnumTypeLog.ERREUR.ToString)
                    Else
                        MessageBox.Show(ex.ToString)
                        Dim Description As String = "La clôture du rendez-vous (tâche n° " & Me.RendezVousId.ToString & ") a échouée"
                        CreateLog(Description, Me.Name, LogDao.EnumTypeLog.ERREUR.ToString)
                    End If
                End Try
            End If
        End If
    End Sub

    'Refresh épisode
    Private Sub RadBtnRefresh_Click(sender As Object, e As EventArgs) Handles RadBtnRefresh.Click
        Refresh()
    End Sub

    Private Sub Refresh()
        Cursor.Current = Cursors.WaitCursor
        ChargementAffichageBlocWorkflow()
        InitParametre()
        ChargementParametres()
        ChargementEpisodeActesParamedicauxParamedical()
        ChargementEpisodeActesParamedicauxMedical()
        ChargementObservationLibre()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = Me.SelectedPatient.PatientNir
        LblPatientPrenom.Text = Me.SelectedPatient.PatientPrenom
        LblPatientNom.Text = Me.SelectedPatient.PatientNom
        LblPatientAge.Text = Me.SelectedPatient.PatientAge
        LblPatientGenre.Text = Me.SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(Me.SelectedPatient.PatientSiteId)
        LblPatientDateMaj.Text = Me.SelectedPatient.PatientSyntheseDateMaj.ToString("dd/MM/yyyy")

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        ChargementToolTipAld()
    End Sub

    '=========================================================
    '=== Episode
    '=========================================================

    'Caractéristiques épisode
    Private Sub ChargementCaracteristiquesEpisode()
        episode = episodeDao.GetEpisodeById(Me.SelectedEpisodeId)
        If episode.DescriptionActivite = "" Then
            LblTypeEpisode.Text = episode.Type.Trim & " / " & episode.TypeActivite
        Else
            LblTypeEpisode.Text = episode.Type.Trim & " / " & episode.TypeActivite & " / " & episode.DescriptionActivite
        End If
        TypeEpisode = episode.Type
        typeActiviteEpisode = episode.TypeActivite
        DescriptionActiviteEpisode = episode.DescriptionActivite
        typeProfilEpisode = episode.TypeProfil
        LblTypeProfil.Text = episode.TypeProfil
        CommentaireEpisode = episode.Commentaire
        LblEpisodeCommentaire.Text = episode.Commentaire

        UtilisateurDao.SetUtilisateur(user, episode.UserCreation)
        LblUserCreation.Text = user.UtilisateurPrenom.Trim & " " & user.UtilisateurNom.Trim
        UserCreation = user.UtilisateurPrenom.Trim & " " & user.UtilisateurNom.Trim
        DateCreation = episode.DateCreation.ToString("dd/MM/yyyy HH:mm")

        LblDuree.Text = "Durée (" & outils.CalculDuree(DateCreation, Date.Now) & ")"

        If episode.UserModification <> 0 Then
            UtilisateurDao.SetUtilisateur(user, episode.UserModification)
            LBlUserModification.Text = user.UtilisateurPrenom.Trim & " " & user.UtilisateurNom.Trim
            UserModification = user.UtilisateurPrenom.Trim & " " & user.UtilisateurNom.Trim
        Else
            LblLabelUserModification.Text = ""
            LBlUserModification.Text = ""
        End If

        If episode.DateModification <> Nothing Then
            LblDateModification.Text = episode.DateModification.ToString("dd/MM/yyyy HH:mm")
            DateModification = LblDateModification.Text
        Else
            LblLabelDateModification.Text = ""
            LblDateModification.Text = ""
        End If
    End Sub

    Private Sub RadGroupBox3_MouseHover(sender As Object, e As EventArgs) Handles RadGroupBox3.MouseHover
        AfficheTooltipCaracetristiqueEpisode(sender)
    End Sub


    Private Sub Label3_MouseHover(sender As Object, e As EventArgs) Handles Label3.MouseHover
        AfficheTooltipCaracetristiqueEpisode(sender)
    End Sub

    Private Sub AfficheTooltipCaracetristiqueEpisode(sender As Object)
        ToolTip.SetToolTip(sender, ConstitutionNotification())

    End Sub

    Private Sub Label3_Click(sender As Object, e As EventArgs) Handles Label3.Click
        Dim form As New RadFNotification()
        form.Titre = "Caractéristique épisode patient"
        form.Message = ConstitutionNotification()
        form.Show()
    End Sub

    Private Function ConstitutionNotification() As String
        Dim message As String

        message = "Episode # " & Me.SelectedEpisodeId.ToString() & vbCrLf &
                            "Type : " & TypeEpisode & vbCrLf &
                            "Type activité : " & typeActiviteEpisode & " " & DescriptionActiviteEpisode & vbCrLf &
                            "Commentaire : " & CommentaireEpisode & vbCrLf &
                            "Créé par : " & UserCreation & " (" & typeProfilEpisode & ")" & " Le : " & DateCreation & vbCrLf &
                            "Modifié par : " & UserModification & " Le : " & DateModification
        Return message
    End Function

    '=========================================================
    '=== Génération actes paramédicaux et paramètres
    '=========================================================

    'Génération protocoles et paramètres
    Private Sub RadBtnGenProtocole_Click(sender As Object, e As EventArgs) Handles RadBtnGenProtocole.Click
        If MsgBox("Attention, le traitement va entrainer la suppression des éventuels paramètres et actes paramédicaux existants, confirmez-vous le traitement", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            Cursor.Current = Cursors.WaitCursor
            'Suppression préalable des paramètres existants
            episodeParametreDao.SuppressionEpisodeParametreByEpisodeId(SelectedEpisodeId)
            'Suppression préalable des actes paramédicaux existants
            episodeActeParamedicalDao.SuppressionEpisodeActeParamedicalByEpisodeId(SelectedEpisodeId)
            'Génération paramètres et actes paramédicaux
            episodeProtocoleCollaboratifDao.GenerateParametreEtProtocoleCollaboratifByEpisode(episode)
            Cursor.Current = Cursors.Default
            Refresh()
            Me.RadDesktopAlert1.CaptionText = "Notification épisode"
            Me.RadDesktopAlert1.ContentText = "Traitement de génération des paramètres et des protocoles terminé"
            Me.RadDesktopAlert1.Show()
        End If
    End Sub



    '=========================================================
    '=== Paramètres
    '=========================================================

    Private Sub InitParametre()
        LblLabelPoids.Text = ""
        LblParmPoids.Text = ""
        LblLabelTemperature.Text = ""
        LblParmTemperature.Text = ""
        LblLabelFC.Text = ""
        LblParmFC.Text = ""
        LblLabelFR.Text = ""
        LblParmFR.Text = ""

        LblLabelTaille.Text = ""
        LblParmTaille.Text = ""
        LblLabelDextro.Text = ""
        LblParmDextro.Text = ""
        LblLabelPAS.Text = ""
        LblParmPAS.Text = ""
        LblLabelSat.Text = ""
        LblParmSat.Text = ""

        LblLabelIMC.Text = ""
        LblParmIMC.Text = ""
        LblLabelObservance.Text = ""
        LblParmObservance.Text = ""
        LblLabelPAD.Text = ""
        LblParmPAD.Text = ""
        LblLabelDEP.Text = ""
        LblParmDEP.Text = ""

        LblLabelPerimetreCranien.Text = ""
        LblParmPerimetreCranien.Text = ""
        LblLabelINR.Text = ""
        LblParmINR.Text = ""
        LblLabelPAM.Text = ""
        LblParmPAM.Text = ""
        LblLabelEVA.Text = ""
        LblParmEVA.Text = ""

        LblParametre1.Text = ""
        LblParametre2.Text = ""
        LblParametre3.Text = ""
        LblParametre4.Text = ""
    End Sub


    'Chargement des paramètres
    Private Sub ChargementParametres()
        Dim ValeurParametreNonSaisie As Boolean = False
        Dim parmDataTable As DataTable
        parmDataTable = episodeParametreDao.getAllParametreEpisodeByEpisodeId(SelectedEpisodeId)
        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = parmDataTable.Rows.Count - 1
        Dim entier, nombreDecimal, longueurString, idString As Integer
        Dim Valeur As Decimal
        Dim description, unite, valeurString, parametreString As String
        Dim ParametreId As Long
        Dim valeurPoids, valeurTaille, valeurPAS, valeurPAD As Decimal
        Dim uniteIMC, unitePAM As String
        Dim IMCaCalculer As Boolean = False
        Dim PAMaCalculer As Boolean = False

        LblParametre1.Text = ""
        LblParametre2.Text = ""
        LblParametre3.Text = ""
        longueurString = 0
        idString = 1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            Valeur = parmDataTable.Rows(i)("valeur")
            If Valeur = 0 Then
                ValeurParametreNonSaisie = True
            End If
            description = parmDataTable.Rows(i)("description")
            entier = parmDataTable.Rows(i)("entier")
            nombreDecimal = parmDataTable.Rows(i)("decimal")
            unite = parmDataTable.Rows(i)("unite")
            ParametreId = parmDataTable.Rows(i)("parametre_id")
            valeurString = ""

            Select Case entier
                Case 1
                    Select Case nombreDecimal
                        Case 0
                            valeurString = Valeur.ToString("0")
                        Case 1
                            valeurString = Valeur.ToString("0.0")
                        Case 2
                            valeurString = Valeur.ToString("0.00")
                        Case 3
                            valeurString = Valeur.ToString("0.000")
                    End Select
                Case 2
                    Select Case nombreDecimal
                        Case 0
                            valeurString = Valeur.ToString("#0")
                        Case 1
                            valeurString = Valeur.ToString("#0.0")
                        Case 2
                            valeurString = Valeur.ToString("#0.00")
                        Case 3
                            valeurString = Valeur.ToString("#0.000")
                    End Select
                Case 3
                    Select Case nombreDecimal
                        Case 0
                            valeurString = Valeur.ToString("##0")
                        Case 1
                            valeurString = Valeur.ToString("##0.0")
                        Case 2
                            valeurString = Valeur.ToString("##0.00")
                        Case 3
                            valeurString = Valeur.ToString("##0.000")
                    End Select
            End Select

            Select Case ParametreId
                Case 1
                    LblLabelPoids.Text = "Poids"
                    LblParmPoids.Text = valeurString & " " & unite
                    valeurPoids = Valeur
                Case 2
                    LblLabelTaille.Text = "Taille"
                    LblParmTaille.Text = valeurString & " " & unite
                    valeurTaille = Valeur
                Case 3
                    LblLabelIMC.Text = "IMC"
                    uniteIMC = unite
                    IMCaCalculer = True
                Case 4
                    LblLabelPerimetreCranien.Text = "Per. cranien"
                    LblParmPerimetreCranien.Text = valeurString & " " & unite
                Case 5
                    LblLabelFC.Text = "FC"
                    LblParmFC.Text = valeurString & " " & unite
                Case 6
                    LblLabelPAS.Text = "PAS"
                    LblParmPAS.Text = valeurString & " " & unite
                    valeurPAS = Valeur
                Case 7
                    LblLabelPAD.Text = "PAD"
                    LblParmPAD.Text = valeurString & " " & unite
                    valeurPAD = Valeur
                Case 8
                    LblLabelPAM.Text = "PAM"
                    unitePAM = unite
                    PAMaCalculer = True
                Case 9
                    LblLabelFR.Text = "FR"
                    LblParmFR.Text = valeurString & " " & unite
                Case 10
                    LblLabelSat.Text = "Sat"
                    LblParmSat.Text = valeurString & " " & unite
                Case 11
                    LblLabelDEP.Text = "DEP"
                    LblParmDEP.Text = valeurString & " " & unite
                Case 12
                    LblLabelDextro.Text = "Dextro"
                    LblParmDextro.Text = valeurString & " " & unite
                Case 15
                    LblLabelINR.Text = "INR"
                    LblParmINR.Text = valeurString & " " & unite
                Case 16
                    LblLabelEVA.Text = "EVA"
                    LblParmEVA.Text = valeurString & " " & unite
                Case 17
                    LblLabelObservance.Text = "Observance"
                    LblParmObservance.Text = valeurString & " " & unite
                Case 20
                    LblLabelTemperature.Text = "Temperature"
                    LblParmTemperature.Text = valeurString & " " & unite
                Case Else
                    parametreString = "          " & description & " : " & valeurString & " " & unite
                    longueurString += parametreString.Length
                    Select Case idString
                        Case 1
                            If LblParametre1.Text = "" Then
                                LblParametre1.Text = description & " : " & valeurString & " " & unite
                            Else
                                LblParametre1.Text += parametreString
                            End If
                            idString = 2
                        Case 2
                            If LblParametre2.Text = "" Then
                                LblParametre2.Text = description & " : " & valeurString & " " & unite
                            Else
                                LblParametre2.Text += parametreString
                            End If
                            idString = 3
                        Case 3
                            If LblParametre3.Text = "" Then
                                LblParametre3.Text = description & " : " & valeurString & " " & unite
                            Else
                                LblParametre3.Text += parametreString
                            End If
                            idString = 4
                        Case 4
                            If LblParametre4.Text = "" Then
                                LblParametre4.Text = description & " : " & valeurString & " " & unite
                            Else
                                LblParametre4.Text += parametreString
                            End If
                            idString = 1
                    End Select
            End Select
        Next

        If IMCaCalculer = True Then
            If valeurPoids <> 0 And valeurTaille <> 0 Then
                Valeur = valeurPoids / ((valeurTaille * valeurTaille) / 10000)
            Else
                Valeur = 0
            End If
            valeurString = Valeur.ToString("#0.0")
            LblParmIMC.Text = valeurString & " " & uniteIMC
        End If

        If PAMaCalculer = True Then
            If valeurPAD <> 0 And valeurPAS <> 0 Then
                Valeur = (valeurPAS + (2 * valeurPAD)) / 3
            Else
                Valeur = 0
            End If
            valeurString = Valeur.ToString("##0")
            LblParmPAM.Text = valeurString & " " & unitePAM
        End If

        If ValeurParametreNonSaisie = True Then
            RadBtnParametre.ForeColor = Color.Red
            RadBtnParametre.Font = New Font(RadBtnParametre.Font, FontStyle.Bold)
            ToolTip.SetToolTip(RadBtnParametre, "Des paramètres requis ne sont pas saisis")
        Else
            RadBtnParametre.ForeColor = Color.FromArgb(21, 66, 139)
            RadBtnParametre.Font = New Font(RadBtnParametre.Font, FontStyle.Regular)
            ToolTip.SetToolTip(RadBtnParametre, "")
        End If

    End Sub

    'Appel saisie des paramètres
    Private Sub RadBtnParametre_Click(sender As Object, e As EventArgs) Handles RadBtnParametre.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using vRadFParametreDetailEdit As New RadFEpisodeParametreDetailEdit
            vRadFParametreDetailEdit.SelectedEpisodeId = Me.SelectedEpisodeId
            vRadFParametreDetailEdit.SelectedPatient = Me.SelectedPatient
            vRadFParametreDetailEdit.ShowDialog()
            If vRadFParametreDetailEdit.CodeRetour = True Then
                InitParametre()
                ChargementParametres()
            End If
        End Using
        Me.Enabled = True
    End Sub


    '=========================================================
    '=== Observations spécifiques (paramédical)
    '=========================================================

    'Chargement des actes paramédicaux associés à l'épisode
    Private Sub ChargementEpisodeActesParamedicauxParamedical()
        FinChargementActesParamedicauxParamedical = False
        Dim acteParamedicalDataTable As DataTable
        acteParamedicalDataTable = episodeActeParamedicalDao.getAllEpisodeActeParamedicalByEpisodeId(SelectedEpisodeId, EpisodeDao.EnumTypeProfil.PARAMEDICAL.ToString)

        RadObsSpeParDataGridView.Rows.Clear()

        Dim iGrid As Integer = -1
        Dim rowCount As Integer = acteParamedicalDataTable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadObsSpeParDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadObsSpeParDataGridView.Rows(iGrid).Cells("episodeActeParamedicalId").Value = acteParamedicalDataTable.Rows(i)("oa_episode_acte_paramedical_id")
            RadObsSpeParDataGridView.Rows(iGrid).Cells("drcId").Value = acteParamedicalDataTable.Rows(i)("drc_id")
            RadObsSpeParDataGridView.Rows(iGrid).Cells("drcDescription").Value = Coalesce(acteParamedicalDataTable.Rows(i)("oa_drc_libelle"), "")
            RadObsSpeParDataGridView.Rows(iGrid).Cells("observation").Value = Coalesce(acteParamedicalDataTable.Rows(i)("observation"), "")
            RadObsSpeParDataGridView.Rows(iGrid).Cells("observationInput").Value = Coalesce(acteParamedicalDataTable.Rows(i)("observation"), "")
        Next

        'Positionnement du grid sur la première occurrence
        If RadObsSpeParDataGridView.Rows.Count > 0 Then
            RadObsSpeParDataGridView.CurrentRow = RadObsSpeParDataGridView.ChildRows(0)
            RadObsSpeParDataGridView.TableElement.VScrollBar.Value = 0
        End If

        If userLog.TypeProfil <> ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
            RadObsSpeParDataGridView.AllowEditRow = False
            SaisieObservationToolStripMenuItem.Enabled = False
        End If

        FinChargementActesParamedicauxParamedical = True
    End Sub

    Private Sub MasterTemplate_CellValueChanged(sender As Object, e As GridViewCellEventArgs) Handles RadObsSpeParDataGridView.CellValueChanged
        If FinChargementActesParamedicauxParamedical = True Then
            ValidationSaisieObservationParamedicale()
        End If
    End Sub

    Private Sub ValidationSaisieObservationParamedicale()
        Dim MiseAJour As Boolean = False

        For Each rowInfo As GridViewRowInfo In RadObsSpeParDataGridView.Rows
            Dim observationInput As String = ""
            Dim observation As String = ""
            Dim id As Long
            For Each cellInfo As GridViewCellInfo In rowInfo.Cells
                If (cellInfo.ColumnInfo.Name = "observationInput") Then
                    If cellInfo.Value <> Nothing Then
                        observationInput = cellInfo.Value
                    Else
                        observationInput = ""
                    End If
                End If
                If (cellInfo.ColumnInfo.Name = "observation") Then
                    If cellInfo.Value <> Nothing Then
                        observation = cellInfo.Value
                    Else
                        observation = ""
                    End If
                End If
                If (cellInfo.ColumnInfo.Name = "episodeActeParamedicalId") Then
                    If cellInfo.Value <> Nothing Then
                        id = cellInfo.Value
                    Else
                        id = 0
                    End If
                End If
            Next
            If observationInput <> observation Then
                'Mise à jour de l'observation
                Console.WriteLine("Id : " & id.ToString & " Observation saisie : " & observationInput & " observation initiale : " & observation)
                episodeActeParamedicalDao.ModificationEpisodeActeParamedicalObservation(id, observationInput)
                MiseAJour = True
            End If
        Next

        If MiseAJour = True Then
            Me.RadDesktopAlert1.CaptionText = "Notification saisie observation spécifique patient"
            Me.RadDesktopAlert1.ContentText = "Observation spécifique mise à jour"
            Me.RadDesktopAlert1.Show()
            'Rechargement grid
            ChargementEpisodeActesParamedicauxParamedical()
        End If
    End Sub

    Private Sub SaisieObservationToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SaisieObservationToolStripMenuItem.Click
        If RadObsSpeParDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadObsSpeParDataGridView.Rows.IndexOf(Me.RadObsSpeParDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim episodeActeParamedicalId As Integer = RadObsSpeParDataGridView.Rows(aRow).Cells("episodeActeParamedicalId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using form As New RadFEpisodeActeParamedicalDetailEdit
                    form.EpisodeActeParamedicalId = episodeActeParamedicalId
                    form.ShowDialog()
                    If form.CodeRetour = True Then
                        ChargementEpisodeActesParamedicauxParamedical()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub MasterTemplate_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadObsSpeParDataGridView.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    '=========================================================
    '=== Observations spécifiques (médical)
    '=========================================================

    'Chargement des actes paramédicaux associés à l'épisode
    Private Sub ChargementEpisodeActesParamedicauxMedical()
        FinChargementActesParamedicauxMedical = False
        Dim acteParamedicalDataTable As DataTable
        acteParamedicalDataTable = episodeActeParamedicalDao.getAllEpisodeActeParamedicalByEpisodeId(SelectedEpisodeId, EpisodeDao.EnumTypeProfil.MEDICAL.ToString)

        RadObsSpeMedDataGridView.Rows.Clear()

        Dim iGrid As Integer = -1
        Dim rowCount As Integer = acteParamedicalDataTable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadObsSpeMedDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadObsSpeMedDataGridView.Rows(iGrid).Cells("episodeActeParamedicalId").Value = acteParamedicalDataTable.Rows(i)("oa_episode_acte_paramedical_id")
            RadObsSpeMedDataGridView.Rows(iGrid).Cells("drcId").Value = acteParamedicalDataTable.Rows(i)("drc_id")
            RadObsSpeMedDataGridView.Rows(iGrid).Cells("drcDescription").Value = Coalesce(acteParamedicalDataTable.Rows(i)("oa_drc_libelle"), "")
            RadObsSpeMedDataGridView.Rows(iGrid).Cells("observation").Value = Coalesce(acteParamedicalDataTable.Rows(i)("observation"), "")
            RadObsSpeMedDataGridView.Rows(iGrid).Cells("observationInput").Value = Coalesce(acteParamedicalDataTable.Rows(i)("observation"), "")
        Next

        'Positionnement du grid sur la première occurrence
        If RadObsSpeMedDataGridView.Rows.Count > 0 Then
            RadObsSpeMedDataGridView.CurrentRow = RadObsSpeMedDataGridView.ChildRows(0)
            RadObsSpeMedDataGridView.TableElement.VScrollBar.Value = 0
        End If

        If userLog.TypeProfil <> ProfilDao.EnumProfilType.MEDICAL.ToString Then
            RadObsSpeMedDataGridView.AllowEditRow = False
            SaisieObservationSpecifiqueMedicaleItem.Enabled = False
            AttributionDesObservationsSpécifiquesToolStripMenuItem.Enabled = False
        End If

        FinChargementActesParamedicauxMedical = True
    End Sub

    Private Sub RadObsSpeMedDataGridView_CellValueChanged(sender As Object, e As GridViewCellEventArgs) Handles RadObsSpeMedDataGridView.CellValueChanged
        If FinChargementActesParamedicauxMedical = True Then
            ValidationSaisieObservationMedicale()
        End If
    End Sub

    Private Sub RadObsSpeMedDataGridView_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadObsSpeMedDataGridView.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    Private Sub ToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles SaisieObservationSpecifiqueMedicaleItem.Click
        If RadObsSpeMedDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadObsSpeMedDataGridView.Rows.IndexOf(Me.RadObsSpeMedDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim episodeActeParamedicalId As Integer = RadObsSpeMedDataGridView.Rows(aRow).Cells("episodeActeParamedicalId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using form As New RadFEpisodeActeParamedicalDetailEdit
                    form.EpisodeActeParamedicalId = episodeActeParamedicalId
                    form.ShowDialog()
                    If form.CodeRetour = True Then
                        ChargementEpisodeActesParamedicauxMedical()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub AttributionDesObservationsSpécifiquesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AttributionDesObservationsSpécifiquesToolStripMenuItem.Click
        'Modification du type de PARAMEDICAL en MEDICAL, puis rechargement des deux grids
        episodeActeParamedicalDao.PutAllEpisodeActeParamedicalToMedicalByEpisodeId(SelectedEpisodeId)
        ChargementEpisodeActesParamedicauxParamedical()
        ChargementEpisodeActesParamedicauxMedical()
    End Sub

    Private Sub ValidationSaisieObservationMedicale()
        Dim MiseAJour As Boolean = False

        For Each rowInfo As GridViewRowInfo In RadObsSpeMedDataGridView.Rows
            Dim observationInput As String = ""
            Dim observation As String = ""
            Dim id As Long
            For Each cellInfo As GridViewCellInfo In rowInfo.Cells
                If (cellInfo.ColumnInfo.Name = "observationInput") Then
                    If cellInfo.Value <> Nothing Then
                        observationInput = cellInfo.Value
                    Else
                        observationInput = ""
                    End If
                End If
                If (cellInfo.ColumnInfo.Name = "observation") Then
                    If cellInfo.Value <> Nothing Then
                        observation = cellInfo.Value
                    Else
                        observation = ""
                    End If
                End If
                If (cellInfo.ColumnInfo.Name = "episodeActeParamedicalId") Then
                    If cellInfo.Value <> Nothing Then
                        id = cellInfo.Value
                    Else
                        id = 0
                    End If
                End If
            Next
            If observationInput <> observation Then
                'Mise à jour de l'observation
                Console.WriteLine("Id : " & id.ToString & " Observation saisie : " & observationInput & " observation initiale : " & observation)
                episodeActeParamedicalDao.ModificationEpisodeActeParamedicalObservation(id, observationInput)
                MiseAJour = True
            End If
        Next

        If MiseAJour = True Then
            Me.RadDesktopAlert1.CaptionText = "Notification saisie observation spécifique patient"
            Me.RadDesktopAlert1.ContentText = "Observation spécifique mise à jour"
            Me.RadDesktopAlert1.Show()
            'Rechargement grid
            ChargementEpisodeActesParamedicauxMedical()
        End If
    End Sub


    '=========================================================
    '=== Observations libres
    '=========================================================

    'Observations libres
    Private Sub ChargementObservationLibre()
        Dim episodeObservationDao As New EpisodeObservationDao
        Dim ObservationSpe As DataTable
        ObservationSpe = episodeObservationDao.GetEpisodeObservationLibreByEpisode(Me.SelectedEpisodeId)

        RadGridViewObsMed.Rows.Clear()
        RadGridViewObsIde.Rows.Clear()

        Dim AfficheDateCreation, Auteur As String
        Dim DateCreation As Date

        Dim i As Integer
        Dim rowCount As Integer = ObservationSpe.Rows.Count - 1
        Dim iGridMed As Integer = -1
        Dim iGridIde As Integer = -1

        For i = 0 To rowCount Step 1
            'Utilisateur creation
            Auteur = ""
            If Coalesce(ObservationSpe.Rows(i)("user_id"), 0) <> 0 Then
                Dim UtilisateurCreation = New Utilisateur()
                SetUtilisateur(UtilisateurCreation, ObservationSpe.Rows(i)("user_id"))
                Auteur = UtilisateurCreation.UtilisateurPrenom & " " & UtilisateurCreation.UtilisateurNom
            End If

            'Date création
            AfficheDateCreation = ""
            If Coalesce(ObservationSpe.Rows(i)("date_creation"), Nothing) <> Nothing Then
                DateCreation = ObservationSpe.Rows(i)("date_creation")
                AfficheDateCreation = DateCreation.ToString("dd.MM.yy HH:mm")
            End If

            Select Case ObservationSpe.Rows(i)("type_observation")
                Case EpisodeObservationDao.EnumTypeEpisodeObservation.MEDICAL.ToString
                    iGridMed += 1
                    RadGridViewObsMed.Rows.Add(iGridMed)
                    RadGridViewObsMed.Rows(iGridMed).Cells("observation").Value = ObservationSpe.Rows(i)("observation")
                    RadGridViewObsMed.Rows(iGridMed).Cells("Identification").Value = Auteur & vbCrLf & AfficheDateCreation
                    RadGridViewObsMed.Rows(iGridMed).Cells("observationId").Value = ObservationSpe.Rows(i)("episode_observation_id")
                Case EpisodeObservationDao.EnumTypeEpisodeObservation.PARAMEDICAL.ToString()
                    iGridIde += 1
                    RadGridViewObsIde.Rows.Add(iGridIde)
                    RadGridViewObsIde.Rows(iGridIde).Cells("observation").Value = ObservationSpe.Rows(i)("observation")
                    RadGridViewObsIde.Rows(iGridIde).Cells("Identification").Value = Auteur & vbCrLf & AfficheDateCreation
                    RadGridViewObsIde.Rows(iGridIde).Cells("observationId").Value = ObservationSpe.Rows(i)("episode_observation_id")
            End Select
        Next

        If RadGridViewObsMed.Rows.Count > 0 Then
            Me.RadGridViewObsMed.CurrentRow = RadGridViewObsMed.Rows(0)
            RadGridViewObsMed.TableElement.VScrollBar.Value = 0
        End If

        If RadGridViewObsIde.Rows.Count > 0 Then
            Me.RadGridViewObsIde.CurrentRow = RadGridViewObsIde.Rows(0)
            RadGridViewObsIde.TableElement.VScrollBar.Value = 0
        End If
    End Sub

    Private Sub RadGridViewObsIde_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadGridViewObsIde.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    Private Sub RadGridViewObsMed_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadGridViewObsMed.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    'Créer une observation libre
    Private Sub CréerUneObservationToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CréerUneObservationToolStripMenuItem1.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using vRadFEpisodeObservationDetailEdit As New RadFEpisodeObservationDetailEdit
            vRadFEpisodeObservationDetailEdit.SelectedEpisodeId = Me.SelectedEpisodeId
            vRadFEpisodeObservationDetailEdit.SelectedPatient = Me.SelectedPatient
            vRadFEpisodeObservationDetailEdit.SelectedObservationId = 0
            vRadFEpisodeObservationDetailEdit.ShowDialog()
            If vRadFEpisodeObservationDetailEdit.CodeRetour = True Then
                ChargementObservationLibre()
            End If
        End Using
        Me.Enabled = True
    End Sub

    'Modifier / Visualiser une observation libre paramédicale
    Private Sub RadGridViewObsIde_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles RadGridViewObsIde.CellDoubleClick
        If RadGridViewObsIde.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewObsIde.Rows.IndexOf(Me.RadGridViewObsIde.CurrentRow)
            If aRow >= 0 Then
                Dim ObservationId As Integer = RadGridViewObsIde.Rows(aRow).Cells("observationId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vRadFEpisodeObservationDetailEdit As New RadFEpisodeObservationDetailEdit
                    vRadFEpisodeObservationDetailEdit.SelectedEpisodeId = Me.SelectedEpisodeId
                    vRadFEpisodeObservationDetailEdit.SelectedPatient = Me.SelectedPatient
                    vRadFEpisodeObservationDetailEdit.SelectedObservationId = ObservationId
                    vRadFEpisodeObservationDetailEdit.ShowDialog()
                    If vRadFEpisodeObservationDetailEdit.CodeRetour = True Then
                        ChargementObservationLibre()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Modifier / Visualiser une observation libre médicale
    Private Sub RadGridViewObsMed_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles RadGridViewObsMed.CellDoubleClick
        If RadGridViewObsMed.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewObsMed.Rows.IndexOf(Me.RadGridViewObsMed.CurrentRow)
            If aRow >= 0 Then
                Dim ObservationId As Integer = RadGridViewObsMed.Rows(aRow).Cells("observationId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vRadFEpisodeObservationDetailEdit As New RadFEpisodeObservationDetailEdit
                    vRadFEpisodeObservationDetailEdit.SelectedEpisodeId = Me.SelectedEpisodeId
                    vRadFEpisodeObservationDetailEdit.SelectedPatient = Me.SelectedPatient
                    vRadFEpisodeObservationDetailEdit.SelectedObservationId = ObservationId
                    vRadFEpisodeObservationDetailEdit.ShowDialog()
                    If vRadFEpisodeObservationDetailEdit.CodeRetour = True Then
                        ChargementObservationLibre()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub


    '=========================================================
    '=== Workflow
    '=========================================================

    'Affichage Workflow
    Private Sub ChargementAffichageBlocWorkflow()
        'Identifier si Workflow en cours
        tache = tacheDao.GetDemandeEnCoursByEpisode(SelectedEpisodeId)
        If tache.Id <> 0 Then
            WorkflowExistant = True
            Dim fonctionDestinataire As Fonction
            fonctionDestinataire = fonctionDao.getFonctionById(tache.DestinataireFonctionId)
            Select Case fonctionDestinataire.Type
                Case ProfilDao.EnumProfilType.PARAMEDICAL.ToString
                    RadBtnWorkflowIde.Text = "Traiter"
                    RadBtnWorkflowIde.Show()
                    Select Case tache.Nature
                        Case TacheDao.NatureTache.DEMANDE.ToString
                            LblWorkflowIDE.Text = "Demande d'avis à traiter"
                            LblWorkflowMed.Text = "Attente rendu de demande d'avis"
                        Case TacheDao.NatureTache.REPONSE.ToString
                            LblWorkflowIDE.Text = "Réponse à valider"
                            LblWorkflowMed.Text = "Demande d'avis rendue"
                        Case TacheDao.NatureTache.COMPLEMENT.ToString
                            LblWorkflowIDE.Text = "Demande de complément d'information"
                            LblWorkflowMed.Text = "Attente complément d'information"
                    End Select
                    RadBtnWorkflowMed.Hide()
                    LblWorkflowMed.Show()
                    LblWorkflowIDE.Show()
                Case ProfilDao.EnumProfilType.MEDICAL.ToString
                    RadBtnWorkflowMed.Text = "Traiter"
                    RadBtnWorkflowMed.Show()
                    Select Case tache.Nature
                        Case TacheDao.NatureTache.DEMANDE.ToString
                            LblWorkflowMed.Text = "Demande d'avis à traiter"
                            LblWorkflowIDE.Text = "Attente rendu de demande d'avis"
                        Case TacheDao.NatureTache.REPONSE.ToString
                            LblWorkflowMed.Text = "Réponse à valider"
                            LblWorkflowIDE.Text = "Demande d'avis rendue"
                        Case TacheDao.NatureTache.COMPLEMENT.ToString
                            LblWorkflowMed.Text = "Demande de complément d'information"
                            LblWorkflowIDE.Text = "Attente complément d'information"
                    End Select
                    RadBtnWorkflowIde.Hide()
                    LblWorkflowIDE.Show()
                    LblWorkflowMed.Show()
            End Select
        Else
            WorkflowExistant = False
            Select Case userLog.TypeProfil
                Case ProfilDao.EnumProfilType.PARAMEDICAL.ToString
                    RadBtnWorkflowIde.Text = "Créer"
                    RadBtnWorkflowIde.Show()
                    LblWorkflowIDE.Text = "Créer une demande d'avis"
                    LblWorkflowIDE.Show()
                    RadBtnWorkflowMed.Hide()
                    LblWorkflowMed.Hide()
                Case ProfilDao.EnumProfilType.MEDICAL.ToString
                    RadBtnWorkflowMed.Text = "Créer"
                    RadBtnWorkflowMed.Show()
                    LblWorkflowMed.Text = "Créer une demande d'avis"
                    LblWorkflowMed.Show()
                    RadBtnWorkflowIde.Hide()
                    LblWorkflowIDE.Hide()
            End Select
        End If
    End Sub

    'Traiter une tâche de la demande d'avis (Workflow)
    Private Sub RadBtnWorkflowIde_Click(sender As Object, e As EventArgs) Handles RadBtnWorkflowIde.Click
        ControleGestionWorkflow()
    End Sub

    Private Sub RadBtnWorkflowMed_Click(sender As Object, e As EventArgs) Handles RadBtnWorkflowMed.Click
        ControleGestionWorkflow()
    End Sub

    Private Sub ControleGestionWorkflow()
        tache = tacheDao.GetDemandeEnCoursByEpisode(SelectedEpisodeId)
        If tache.Id <> 0 Then
            If WorkflowExistant = False Then
                ChargementAffichageBlocWorkflow()
                MessageBox.Show("Opération annulée, une demande d'avis vient d'être créée pour cet épisode par un autre utilisateur")
                Exit Sub
            End If

            If tache.TraiteUserId <> 0 Then
                If tache.TraiteUserId <> userLog.UtilisateurId Then
                    Dim userTraitement As Utilisateur
                    userTraitement = UserDao.getUserById(tache.TraiteUserId)
                    MessageBox.Show("Accès impossible, le traitement de cette demande d'avis est déjà attribué à " & userTraitement.UtilisateurPrenom & " " & userTraitement.UtilisateurNom & " (" & userTraitement.UtilisateurProfilId & ")")
                    Exit Sub
                End If
            Else
                'Tâche non encore attribuée, proposition d'attribution de la tâche à l'utilisateur
                If MsgBox("Par cette action, vous allez vous attribuer le traitement de cette demande d'avis. Confirmez-vous son attribution", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
                    Try
                        tacheDao.attribueTacheToUserLog(tache.Id)
                    Catch ex As Exception
                        'MessageBox.Show(ex.ToString)
                        Exit Sub
                    End Try
                Else
                    Exit Sub
                End If
            End If
        End If

        If tache.Id <> 0 Then
            Me.Enabled = False
            Cursor.Current = Cursors.WaitCursor
            Using vRadFWkfDemandeAvis As New RadFWkfDemandeAvis
                vRadFWkfDemandeAvis.SelectedEpisodeId = Me.SelectedEpisodeId
                vRadFWkfDemandeAvis.SelectedPatient = Me.SelectedPatient
                vRadFWkfDemandeAvis.SelectedTacheId = tache.Id
                vRadFWkfDemandeAvis.Creation = False
                vRadFWkfDemandeAvis.ShowDialog()
            End Using
            Me.Enabled = True
        Else
            Me.Enabled = False
            Cursor.Current = Cursors.WaitCursor
            Using vRadFWkfDemandeAvis As New RadFWkfDemandeAvis
                vRadFWkfDemandeAvis.SelectedEpisodeId = Me.SelectedEpisodeId
                vRadFWkfDemandeAvis.SelectedPatient = Me.SelectedPatient
                vRadFWkfDemandeAvis.SelectedTacheId = 0
                vRadFWkfDemandeAvis.Creation = True
                vRadFWkfDemandeAvis.ShowDialog()
            End Using
            Me.Enabled = True
        End If

        ChargementAffichageBlocWorkflow()
    End Sub

    'Liste des workflows de l'épisode
    Private Sub RadBtnHistoWorkflow_Click(sender As Object, e As EventArgs) Handles RadBtnHistoWorkflow.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using vRadFWkfDemandeAvishisto As New RadFWkfDemandeAvisHisto
            vRadFWkfDemandeAvishisto.SelectedEpisodeId = Me.SelectedEpisodeId
            vRadFWkfDemandeAvishisto.SelectedPatient = Me.SelectedPatient
            vRadFWkfDemandeAvishisto.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub


    '===============================================================================================================================
    '======================= Synthèse ==============================================================================================
    '===============================================================================================================================

    '==========================================================
    '======================= Antécédent =======================
    '==========================================================

    'Chargement de la Grid
    Private Sub ChargementAntecedent()
        Dim antecedentDataTable As DataTable
        Dim antecedentDao As AntecedentDao = New AntecedentDao

        Cursor.Current = Cursors.WaitCursor
        RadAntecedentDataGridView.Rows.Clear()

        If RadChkPublie.Checked = True Then
            If RadChkParPriorite.Checked = True Then
                antecedentDataTable = antecedentDao.GetAllAntecedentbyPatient(SelectedPatient.patientId, True, True)
            Else
                antecedentDataTable = antecedentDao.GetAllAntecedentbyPatient(SelectedPatient.patientId, True, False)
            End If
        Else
            If RadChkParPriorite.Checked = True Then
                antecedentDataTable = antecedentDao.GetAllAntecedentbyPatient(SelectedPatient.patientId, False, True)
            Else
                antecedentDataTable = antecedentDao.GetAllAntecedentbyPatient(SelectedPatient.patientId, False, False)
            End If
        End If

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim indentation As String
        Dim dateDateModification, AldDateFin As Date
        Dim AfficheDateModification As String
        Dim diagnostic As String
        Dim antecedentCache, AldValide, AldValideOK, AldDemandeEnCours As Boolean

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1

            If RadChkMajeurSeul.Checked = True Then
                If antecedentDataTable.Rows(i)("oa_antecedent_niveau") <> 1 Then
                    Continue For
                End If
            End If

            If RadChkParPriorite.Checked = True Then
                'Détermination de l'indentation à appliquer pour l'affichage de l'antécédent si affichage par priorité
                Select Case antecedentDataTable.Rows(i)("oa_antecedent_niveau")
                    Case 1
                        indentation = ""
                    Case 2
                        indentation = "           > "
                    Case 3
                        indentation = "                        >> "
                    Case Else
                        indentation = ""
                End Select
            Else
                indentation = ""
            End If

            'Recherche si le contexte a été modifié
            AfficheDateModification = ""
            If antecedentDataTable.Rows(i)("oa_antecedent_date_modification") IsNot DBNull.Value Then
                dateDateModification = antecedentDataTable.Rows(i)("oa_antecedent_date_modification")
                AfficheDateModification = " (" + FormatageDateAffichage(dateDateModification) + ")"
            Else
                If antecedentDataTable.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                    dateDateModification = antecedentDataTable.Rows(i)("oa_antecedent_date_creation")
                    AfficheDateModification = " (" + FormatageDateAffichage(dateDateModification) + ")"
                End If
            End If

            'Identification si l'antécédent est caché
            antecedentCache = False
            If antecedentDataTable.Rows(i)("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                If antecedentDataTable.Rows(i)("oa_antecedent_statut_affichage") = "C" Then
                    antecedentCache = True
                End If
            End If

            AldValide = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ald_valide"), False)
            AldDateFin = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ald_date_fin"), Nothing)
            AldValideOK = False
            If AldValide = True Then
                If AldDateFin > Date.Now() Then
                    AldValideOK = True
                End If
            End If
            AldDemandeEnCours = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ald_demande_en_cours"), False)

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadAntecedentDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            diagnostic = ""
            If antecedentDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                If CInt(antecedentDataTable.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                    diagnostic = "Suspicion de : "
                Else
                    If CInt(antecedentDataTable.Rows(i)("oa_antecedent_diagnostic")) = 3 Then
                        diagnostic = "Notion de : "
                    End If
                End If
            End If

            Dim longueurString As Integer
            Dim longueurMax As Integer = 100
            Dim antecedentDescription As String

            '===== Affichage antécédent
            If antecedentDataTable.Rows(i)("oa_antecedent_description") Is DBNull.Value Or antecedentDataTable.Rows(i)("oa_antecedent_description") = "" Then
                antecedentDescription = ""
            Else
                antecedentDescription = antecedentDataTable.Rows(i)("oa_antecedent_description")
                antecedentDescription = Replace(antecedentDescription, vbCrLf, " ")
                longueurString = antecedentDescription.Length
                If longueurString > longueurMax Then
                    longueurString = longueurMax
                End If
                antecedentDescription = antecedentDescription.Substring(0, longueurString)
                RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentDescription").Value = antecedentDataTable.Rows(i)("oa_antecedent_description")
            End If

            Dim DescriptionDrcAld As String = ""
            If AldValideOK Or AldDemandeEnCours Then
                DescriptionDrcAld = Coalesce(antecedentDataTable.Rows(i)("oa_ald_cim10_description"), "")
            End If

            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedent").Value = indentation & diagnostic & DescriptionDrcAld & " " & antecedentDescription
            'RadAntecedentDataGridView.Rows(iGrid).Cells("antecedent").Value = indentation + diagnostic + DescriptionDrcAld + " (" + DateDebutString + ") " + antecedentDescription
            '==========

            If antecedentCache = True Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("antecedent").Style.ForeColor = Color.CornflowerBlue
            Else
                If AldValideOK = True Then
                    RadAntecedentDataGridView.Rows(iGrid).Cells("antecedent").Style.ForeColor = Color.Red
                Else
                    If AldDemandeEnCours = True Then
                        RadAntecedentDataGridView.Rows(iGrid).Cells("antecedent").Style.ForeColor = Color.DarkOrange
                    End If
                End If
            End If

            If AldValideOK = True Or AldDemandeEnCours = True Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentAld").Value = "X"
            Else
                RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentAld").Value = ""
            End If

            'Id antécédent
            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentId").Value = antecedentDataTable.Rows(i)("oa_antecedent_id")
            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentDrcId").Value = antecedentDataTable.Rows(i)("oa_antecedent_drc_id")
            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentNiveau").Value = antecedentDataTable.Rows(i)("oa_antecedent_niveau")

            'Détermination de l'antécédent pere si niveau 2 et 3
            Select Case CInt(antecedentDataTable.Rows(i)("oa_antecedent_niveau"))
                Case 2
                    RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentPereId").Value = antecedentDataTable.Rows(i)("oa_antecedent_id_niveau1")
                Case 3
                    RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentPereId").Value = antecedentDataTable.Rows(i)("oa_antecedent_id_niveau2")
                Case Else
                    RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentPereId").Value = 0
            End Select
        Next

        'Positionnement du grid sur la première occurrence
        If RadAntecedentDataGridView.Rows.Count > 0 Then
            Me.RadAntecedentDataGridView.CurrentRow = RadAntecedentDataGridView.Rows(0)
        End If
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub RafraichirLaListeToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RafraichirLaListeToolStripMenuItem.Click
        ChargementAntecedent()
    End Sub

    'Appel de la modification d'un antécédent
    Private Sub MasterTemplate_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadAntecedentDataGridView.CellDoubleClick
        modificationAntecedent()
    End Sub

    Private Sub ModifierUnAntécédentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModifierUnAntécédentToolStripMenuItem.Click
        modificationAntecedent()
    End Sub

    Private Sub modificationAntecedent()
        If RadAntecedentDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow, antecedentId As Integer
            aRow = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
            If aRow >= 0 Then
                Cursor.Current = Cursors.WaitCursor
                antecedentId = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
                Me.Enabled = False
                Using vFAntecedentDetailEdit As New RadFAntecedentDetailEdit
                    vFAntecedentDetailEdit.SelectedAntecedentId = antecedentId
                    vFAntecedentDetailEdit.SelectedPatient = Me.SelectedPatient
                    vFAntecedentDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFAntecedentDetailEdit.SelectedDrcId = 0
                    vFAntecedentDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                    vFAntecedentDetailEdit.ShowDialog() 'Modal
                    If vFAntecedentDetailEdit.CodeRetour = True Then
                        ChargementAntecedent()
                        If vFAntecedentDetailEdit.Reactivation = True Then
                            'Rechargement des contextes si réactivation
                            ChargementContexte()
                        End If
                        ChargementToolTipAld()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Créer un antécédent
    Private Sub CreerAntecedentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerAntecedentToolStripMenuItem.Click
        Dim SelectedDrcId As Integer
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFDrcSelecteur As New RadFDRCSelecteur
            vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
            vFDrcSelecteur.CategorieOasis = 1       'Catégorie Oasis : "Antécédent et Contexte"
            vFDrcSelecteur.ShowDialog()             'Modal
            SelectedDrcId = vFDrcSelecteur.SelectedDrcId
            'Si un DRC a été sélectionné
            If SelectedDrcId <> 0 Then
                Using vFAntecedentDetailEdit As New RadFAntecedentDetailEdit
                    vFAntecedentDetailEdit.SelectedPatient = Me.SelectedPatient
                    vFAntecedentDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFAntecedentDetailEdit.SelectedDrcId = SelectedDrcId
                    vFAntecedentDetailEdit.SelectedDrc = vFDrcSelecteur.SelectedDrc
                    vFAntecedentDetailEdit.SelectedAntecedentId = 0
                    vFAntecedentDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                    vFAntecedentDetailEdit.ShowDialog() 'Modal
                    'Si le traitement a été créé, on recharge la grid
                    If vFAntecedentDetailEdit.CodeRetour = True Then
                        ChargementAntecedent()
                        ChargementToolTipAld()
                    End If
                End Using
            End If
        End Using
        Me.Enabled = True
    End Sub

    'Historique des modifications d'un antécédent
    Private Sub HistoriqueDesModificationsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem.Click
        If RadAntecedentDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim AntecedentId As Integer = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vFAntecedenttHistoListe As New RadFAntecedentHistoListe
                    vFAntecedenttHistoListe.SelectedAntecedentId = AntecedentId
                    vFAntecedenttHistoListe.SelectedPatient = Me.SelectedPatient
                    vFAntecedenttHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFAntecedenttHistoListe.ShowDialog() 'Modal
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Modifier l'ordre d'un antécédent
    Private Sub ModifierLordreDunAntecedentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModifierLordreDunAntécédentToolStripMenuItem.Click
        'Appel de la gestion de la modification de l'ordre d'un antécédent
        If RadAntecedentDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim AntecedentId As Integer = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vFAntecedentOrdreSelecteur As New RadFAntecedentOrdreSelecteur
                    vFAntecedentOrdreSelecteur.SelectedPatient = Me.SelectedPatient
                    vFAntecedentOrdreSelecteur.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFAntecedentOrdreSelecteur.AntecedentIdaOrdonner = AntecedentId
                    vFAntecedentOrdreSelecteur.AntecedentDescriptionAOrdonner = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentDescription").Value
                    vFAntecedentOrdreSelecteur.NiveauAntecedentAOrdonner = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentNiveau").Value
                    vFAntecedentOrdreSelecteur.AntecedentIdPere = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentPereId").Value
                    vFAntecedentOrdreSelecteur.PositionGaucheDroite = EnumPosition.Gauche
                    vFAntecedentOrdreSelecteur.ShowDialog() 'Modal
                    'Si le traitement a été modifié, on recharge la grid
                    If vFAntecedentOrdreSelecteur.CodeRetour = True Then
                        ChargementAntecedent()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Changer l'affectation d'un antécédent (changement d'association)
    Private Sub ChangerLaffectationDunAntecedentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ChangerLaffectationDunAntecedentToolStripMenuItem.Click
        If RadAntecedentDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
            If aRow >= 0 Then
                If RadAntecedentDataGridView.Rows(aRow).Cells("antecedentAld").Value = "X" Then
                    MessageBox.Show("Un antécédent avec une ALD en cours de validité ou une ALD en cours de demande est obligatoirement un 'Antécédent majeur' et" &
                                    " ne peut donc pas être affecté à un autre antécédent")
                Else
                    Dim AntecedentId As Integer = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
                    Me.Enabled = False
                    Cursor.Current = Cursors.WaitCursor
                    Using vFAntecedentAffectationSelecteur As New RadFAntecedentAffectationSelecteur
                        vFAntecedentAffectationSelecteur.SelectedPatient = Me.SelectedPatient
                        vFAntecedentAffectationSelecteur.UtilisateurConnecte = Me.UtilisateurConnecte
                        vFAntecedentAffectationSelecteur.AntecedentIdaAffecter = AntecedentId
                        vFAntecedentAffectationSelecteur.AntecedentDescriptionaAffecter = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentDescription").Value
                        vFAntecedentAffectationSelecteur.NiveauAntecedentaAffecter = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentNiveau").Value
                        vFAntecedentAffectationSelecteur.AntecedentIdPere = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentPereId").Value
                        vFAntecedentAffectationSelecteur.PositionGaucheDroite = EnumPosition.Gauche
                        vFAntecedentAffectationSelecteur.ShowDialog() 'Modal
                        'Si le traitement a été modifié, on recharge la grid
                        If vFAntecedentAffectationSelecteur.CodeRetour = True Then
                            ChargementAntecedent()
                        End If
                    End Using
                    Me.Enabled = True
                End If
            End If
        End If
    End Sub

    'Transformer En majeur (pour un antécédent non majeur)
    Private Sub TransformerEnMajeurToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TransformerEnMajeurToolStripMenuItem.Click
        'TODO: à réaliser
    End Sub

    'Gestion des options d'affichage des antécédents sur évènement
    Private Sub RadChkPublie_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkPublie.ToggleStateChanged
        If RadChkPublie.Checked = True Then
            RadChkTous.Checked = False
            If InitPublie = True Then
                Application.DoEvents()
                RadAntecedentDataGridView.Rows.Clear()
                ChargementAntecedent()
            Else
                InitPublie = True
            End If
        Else
            If RadChkTous.Checked = False Then
                RadChkPublie.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkTous_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkTous.ToggleStateChanged
        If RadChkTous.Checked = True Then
            RadChkPublie.Checked = False
            Application.DoEvents()
            RadAntecedentDataGridView.Rows.Clear()
            ChargementAntecedent()
        Else
            If RadChkPublie.Checked = False Then
                RadChkTous.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkParPriorite_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkParPriorite.ToggleStateChanged
        If RadChkParPriorite.Checked = True Then
            RadChkParChronologie.Checked = False
            If InitParPriorite = True Then
                Application.DoEvents()
                RadAntecedentDataGridView.Rows.Clear()
                ChargementAntecedent()
            Else
                InitParPriorite = True
            End If
        Else
            If RadChkParChronologie.Checked = False Then
                RadChkParPriorite.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkParChronologie_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkParChronologie.ToggleStateChanged
        If RadChkParChronologie.Checked = True Then
            RadChkParPriorite.Checked = False
            Application.DoEvents()
            RadAntecedentDataGridView.Rows.Clear()
            ChargementAntecedent()
        Else
            If RadChkParPriorite.Checked = False Then
                RadChkParChronologie.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkMajeurSeul_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles RadChkMajeurSeul.ToggleStateChanged
        If RadChkMajeurSeul.Checked = True Then
            RadChkMajeurTous.Checked = False
            Application.DoEvents()
            RadAntecedentDataGridView.Rows.Clear()
            ChargementAntecedent()
        Else
            If RadChkMajeurTous.Checked = False Then
                RadChkMajeurSeul.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkMajeurTous_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles RadChkMajeurTous.ToggleStateChanged
        If RadChkMajeurTous.Checked = True Then
            RadChkMajeurSeul.Checked = False
            If InitMajeur = True Then
                Application.DoEvents()
                RadAntecedentDataGridView.Rows.Clear()
                ChargementAntecedent()
            Else
                InitMajeur = True
            End If
        Else
            If RadChkMajeurSeul.Checked = False Then
                RadChkMajeurTous.Checked = True
            End If
        End If
    End Sub

    Private Sub ChargementToolTipAld()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    Private Sub RadAntecedentDataGridView_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadAntecedentDataGridView.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub


    '==========================================================
    '======================= Traitement =======================
    '==========================================================

    'Chargement de la Grid
    Private Sub ChargementTraitement()
        Dim traitementDataTable As DataTable
        Dim traitementDao As TraitementDao = New TraitementDao

        Cursor.Current = Cursors.WaitCursor
        RadTraitementDataGridView.Rows.Clear()

        traitementDataTable = traitementDao.getTraitementNotCancelledbyPatient(Me.SelectedPatient.patientId)

        'Ajout d'une colonne 'oa_traitement_posologie' dans le DataTable de traitement
        traitementDataTable.Columns.Add("oa_traitement_posologie", Type.GetType("System.String"))

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = traitementDataTable.Rows.Count - 1
        Dim Base As String
        Dim Posologie As String
        Dim dateFin, dateDebut, dateModification, dateCreation As Date
        Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
        Dim Rythme As Integer
        Dim FenetreTherapeutiqueEnCours As Boolean
        Dim FenetreTherapeutiqueAVenir As Boolean

        'Dim Allergie As Boolean = False
        Dim FenetreDateDebut, FenetreDateFin As Date

        Allergie = False

        ContreIndication = False
        LblAllergie.Visible = False
        LblSubstance.Hide()
        LblContreIndication.Visible = False
        SelectedPatient.PatientAllergieCis.Clear()
        SelectedPatient.PatientAllergieDci.Clear()
        SelectedPatient.PatientContreIndicationCis.Clear()
        SelectedPatient.PatientContreIndicationDci.Clear()
        SelectedPatient.PatientMedicamentsPrescritsCis.Clear()

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            'Récupération des médicaments déclarés 'allergique' et exclusion de l'affichage
            If traitementDataTable.Rows(i)("oa_traitement_allergie") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_allergie") = "1" Then
                    Allergie = True
                    SelectedPatient.PatientAllergieDci.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_dci"))
                    SelectedPatient.PatientAllergieCis.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_cis"))
                    Continue For
                End If
            End If

            'Récupération des médicaments déclarés 'contre-indiqué' et exclusion de l'affichage
            If traitementDataTable.Rows(i)("oa_traitement_contre_indication") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_contre_indication") = "1" Then
                    ContreIndication = True
                    SelectedPatient.PatientContreIndicationDci.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_dci"))
                    SelectedPatient.PatientContreIndicationCis.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_cis"))
                    Continue For
                End If
            End If

            'Exclusion de l'affichage des traitements déclarés 'arrêté'
            'Cette condition est traitée en exclusion (et non dans la requête SQL) pour stocker les allergies et les contre-indications arrêtés dans la StringCollection
            If traitementDataTable.Rows(i)("oa_traitement_arret") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_arret") = "A" Then
                    Continue For
                End If
            End If

            'Date de fin
            If traitementDataTable.Rows(i)("oa_traitement_date_fin") IsNot DBNull.Value Then
                dateFin = traitementDataTable.Rows(i)("oa_traitement_date_fin")
            Else
                dateFin = "31/12/2999"
            End If

            'Date début
            If traitementDataTable.Rows(i)("oa_traitement_date_debut") IsNot DBNull.Value Then
                dateDebut = traitementDataTable.Rows(i)("oa_traitement_date_debut")
            Else
                dateDebut = "01/01/1900"
            End If

            'Date création
            If traitementDataTable.Rows(i)("oa_traitement_date_creation") IsNot DBNull.Value Then
                dateCreation = traitementDataTable.Rows(i)("oa_traitement_date_creation")
            Else
                dateCreation = "01/01/1900"
            End If

            'Date modification
            If traitementDataTable.Rows(i)("oa_traitement_date_modification") IsNot DBNull.Value Then
                dateModification = traitementDataTable.Rows(i)("oa_traitement_date_modification")
            Else
                dateModification = dateCreation
            End If

            'Exclusion de l'affichage des traitements dont la date de fin est <à la date du jour
            'Cette condition est traitée en exclusion (et non dans la requête SQL) pour stocker les allergies et les contre-indications dans la StringCollection quel que soit leur date de fin
            Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
            Dim dateFinaComparer As New Date(dateFin.Year, dateFin.Month, dateFin.Day, 0, 0, 0)
            If (dateFinaComparer < dateJouraComparer) Then
                Continue For
            End If

            'Vérification de l'existence d'une fenêtre thérapeutique active et à venir
            FenetreTherapeutiqueEnCours = False
            FenetreTherapeutiqueAVenir = False

            If traitementDataTable.Rows(i)("oa_traitement_fenetre_date_debut") IsNot DBNull.Value Then
                FenetreDateDebut = traitementDataTable.Rows(i)("oa_traitement_fenetre_date_debut")
            Else
                FenetreDateDebut = "31/12/2999"
            End If

            If traitementDataTable.Rows(i)("oa_traitement_fenetre_date_fin") IsNot DBNull.Value Then
                FenetreDateFin = traitementDataTable.Rows(i)("oa_traitement_fenetre_date_fin")
            Else
                FenetreDateFin = "01/01/1900"
            End If

            Posologie = ""

            'Existence d'une fenêtre thérapeutique
            Dim FenetreTherapeutiqueExiste As Boolean = False
            Dim dateDebutFenetreaComparer As New Date(FenetreDateDebut.Year, FenetreDateDebut.Month, FenetreDateDebut.Day, 0, 0, 0)
            Dim dateFinFenetreaComparer As New Date(FenetreDateFin.Year, FenetreDateFin.Month, FenetreDateFin.Day, 0, 0, 0)
            If traitementDataTable.Rows(i)("oa_traitement_fenetre") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_fenetre") = "1" Then
                    'Fenêtre thérapeutique en cours, à venir ou obsolète
                    FenetreTherapeutiqueExiste = True
                    If FenetreDateDebut <= dateJouraComparer And FenetreDateFin >= dateJouraComparer Then
                        'Fenêtre thérapeutique en cours
                        FenetreTherapeutiqueEnCours = True
                        Posologie = "Fenêtre Th."
                    Else
                        If FenetreDateDebut > dateJouraComparer Then
                            FenetreTherapeutiqueAVenir = True
                        End If
                    End If
                End If
            End If

            'Formatage de la posologie
            If FenetreTherapeutiqueEnCours = False Then
                If traitementDataTable.Rows(i)("oa_traitement_posologie_base") IsNot DBNull.Value Then
                    Rythme = traitementDataTable.Rows(i)("oa_traitement_posologie_rythme")
                    Select Case traitementDataTable.Rows(i)("oa_traitement_posologie_base")
                        Case TraitementDao.EnumBaseCode.JOURNALIER, TraitementDao.EnumBaseCode.CONDITIONNEL
                            If traitementDataTable.Rows(i)("oa_traitement_posologie_matin") <> 0 Then
                                posologieMatin = traitementDataTable.Rows(i)("oa_traitement_posologie_matin")
                            Else
                                posologieMatin = 0
                            End If
                            If traitementDataTable.Rows(i)("oa_traitement_posologie_midi") <> 0 Then
                                posologieMidi = traitementDataTable.Rows(i)("oa_traitement_posologie_midi")
                            Else
                                posologieMidi = 0
                            End If
                            If traitementDataTable.Rows(i)("oa_traitement_posologie_soir") <> 0 Then
                                posologieSoir = traitementDataTable.Rows(i)("oa_traitement_posologie_soir")
                            Else
                                posologieSoir = 0
                            End If
                            Select Case traitementDataTable.Rows(i)("oa_traitement_posologie_base")
                                Case TraitementDao.EnumBaseCode.JOURNALIER
                                    Base = ""
                                    If traitementDataTable.Rows(i)("oa_traitement_posologie_apres_midi") <> 0 Then
                                        posologieApresMidi = traitementDataTable.Rows(i)("oa_traitement_posologie_apres_midi")
                                        Posologie = Base + posologieMatin.ToString + "." + posologieMidi.ToString + "." + posologieApresMidi.ToString + "." + posologieSoir.ToString
                                    Else
                                        Posologie = Base + " " + posologieMatin.ToString + "." + posologieMidi.ToString + "." + posologieSoir.ToString
                                    End If
                                Case TraitementDao.EnumBaseCode.CONDITIONNEL
                                    Base = "Conditionnel : "
                                    Dim sommePosologie = posologieMatin + posologieMidi + posologieApresMidi + posologieSoir
                                    If sommePosologie = posologieMatin OrElse sommePosologie = posologieMidi OrElse sommePosologie = posologieApresMidi OrElse sommePosologie = posologieSoir Then
                                        'Un seul item de saisie
                                        Posologie = Base + sommePosologie.ToString
                                    Else
                                        If posologieMatin <> 0 OrElse posologieMidi <> 0 OrElse posologieApresMidi <> 0 OrElse posologieSoir <> 0 Then
                                            If posologieApresMidi <> 0 Then
                                                Posologie = Base + posologieMatin.ToString + "." + posologieMidi.ToString + "." + posologieApresMidi.ToString + "." + posologieSoir.ToString
                                            Else
                                                Posologie = Base + " " + posologieMatin.ToString + "." + posologieMidi.ToString + "." + posologieSoir.ToString
                                            End If
                                        End If
                                    End If
                            End Select
                        Case TraitementDao.EnumBaseCode.HEBDOMADAIRE
                            Base = "Hebdo : "
                            Posologie = Base + Rythme.ToString
                        Case TraitementDao.EnumBaseCode.MENSUEL
                            Base = "Mensuel : "
                            Posologie = Base + Rythme.ToString
                        Case TraitementDao.EnumBaseCode.ANNUEL
                            Base = "Annuel : "
                            Posologie = Base + Rythme.ToString
                        Case Else
                            Base = "Base inconnue ! "
                            Posologie = Base + Rythme.ToString
                    End Select
                End If
            End If

            Dim commentaire As String = Coalesce(traitementDataTable.Rows(i)("oa_traitement_commentaire"), "")
            Dim commentairePosologie As String = Coalesce(traitementDataTable.Rows(i)("oa_traitement_posologie_commentaire"), "")

            'Stockage des médicaments prescrits (pour contrôle lors de la selection d'un médicament dans le cadre d'un nouveau traitement
            SelectedPatient.PatientMedicamentsPrescritsCis.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_cis"))

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadTraitementDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            'DCI
            RadTraitementDataGridView.Rows(iGrid).Cells("medicamentDci").Value = traitementDataTable.Rows(i)("oa_traitement_medicament_dci")
            'Posologie
            RadTraitementDataGridView.Rows(iGrid).Cells("posologie").Value = Posologie
            RadTraitementDataGridView.Rows(iGrid).Cells("commentaire").Value = commentaire
            RadTraitementDataGridView.Rows(iGrid).Cells("commentairePosologie").Value = commentairePosologie

            If Posologie = "Fenêtre Th." Then
                RadTraitementDataGridView.Rows(iGrid).Cells("posologie").Style.ForeColor = Color.Red
            End If

            'Fenêtre thérapeutique existe (en cours ou à venir ou obsolète)
            If FenetreTherapeutiqueExiste = True Then
                RadTraitementDataGridView.Rows(iGrid).Cells("fenetreTherapeutique").Value = "O"
            Else
                RadTraitementDataGridView.Rows(iGrid).Cells("fenetreTherapeutique").Value = ""
            End If

            'Traitement du format d'affichage de la fin du traitement
            If dateDebut = "31/12/2999" Then
                RadTraitementDataGridView.Rows(iGrid).Cells("dateDebut").Value = "Date non définie"
            Else
                RadTraitementDataGridView.Rows(iGrid).Cells("dateDebut").Value = FormatageDateAffichage(dateDebut, True)
            End If

            'Traitement du format d'affichage de modification du traitement
            If dateModification = "01/01/1900" Then
                RadTraitementDataGridView.Rows(iGrid).Cells("dateModification").Value = "Date non définie"
            Else
                RadTraitementDataGridView.Rows(iGrid).Cells("dateModification").Value = FormatageDateAffichage(dateModification, True)
            End If

            'Identifiant du traitement
            RadTraitementDataGridView.Rows(iGrid).Cells("traitementId").Value = traitementDataTable.Rows(i)("oa_traitement_id")

            'CIS du médicament
            RadTraitementDataGridView.Rows(iGrid).Cells("medicamentCis").Value = traitementDataTable.Rows(i)("oa_traitement_medicament_cis")

            'Bouton gérer fenêtre thérapeutique
            If FenetreTherapeutiqueAVenir = True Or FenetreTherapeutiqueEnCours = True Then
                RadTraitementDataGridView.Rows(iGrid).Cells("posologie").Style.ForeColor = Color.Red
            End If
        Next

        'Si allergie, affichage des substances allergiques
        If Allergie = True Then
            LblAllergie.Visible = True
            LblSubstance.Show()
            Dim premierPassage As Boolean = True
            Dim LongueurChaine, LongueurSub As Integer
            Dim AllergieTooltip As String
            Dim LongueurMax As Integer = LongueurStringAllergie

            'Chargement du TextBox
            Dim allergieString As String
            Dim SubstancesAllergiques As New StringCollection()
            SubstancesAllergiques = MedocDao.ListeSubstancesAllergiques(SelectedPatient.PatientAllergieCis)
            Dim allergieEnumerator As StringEnumerator = SubstancesAllergiques.GetEnumerator()
            While allergieEnumerator.MoveNext()
                If premierPassage = True Then
                    allergieString = allergieEnumerator.Current.ToString
                    LongueurChaine = allergieString.Length
                    If LongueurChaine > LongueurMax Then
                        LongueurSub = LongueurMax
                    Else
                        LongueurSub = LongueurChaine
                    End If
                    LblSubstance.Text = allergieString.Substring(0, LongueurSub)
                    AllergieTooltip = allergieString
                    premierPassage = False
                Else
                    allergieString = allergieEnumerator.Current.ToString
                    LongueurChaine = allergieString.Length
                    LongueurChaine = allergieString.Length
                    If LongueurChaine > LongueurMax Then
                        LongueurSub = LongueurMax
                    Else
                        LongueurSub = LongueurChaine
                    End If
                    LblSubstance.Text = LblSubstance.Text & " + " & allergieString.Substring(0, LongueurSub)
                    AllergieTooltip = AllergieTooltip + vbCrLf + allergieString
                End If
            End While
            ToolTip.SetToolTip(LblAllergie, AllergieTooltip)
            'Chargement des médicaments génériques associés aux médicaments allergiques déclarés
            TraitementAllergies(Me.SelectedPatient)
        Else
            ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Enabled = False
        End If

        If ContreIndication = True Then
            LblContreIndication.Show()
            'Chargement des médicaments génériques associés aux médicaments contre-indiqués déclarés
            Dim premierPassage As Boolean = True
            Dim CITooltip As String
            Dim LongueurMax As Integer = LongueurStringAllergie

            'Chargement du TextBox
            Dim CIString As String
            Dim SubstancesCI As New StringCollection()
            SubstancesCI = MedocDao.ListeSubstancesCI(SelectedPatient.PatientContreIndicationCis)
            Dim CIEnumerator As StringEnumerator = SubstancesCI.GetEnumerator()
            While CIEnumerator.MoveNext()
                If premierPassage = True Then
                    CIString = CIEnumerator.Current.ToString
                    CITooltip = CIString
                    premierPassage = False
                Else
                    CIString = CIEnumerator.Current.ToString
                    CITooltip = CITooltip + vbCrLf + CIString
                End If
            End While
            ToolTip.SetToolTip(LblContreIndication, CITooltip)
            'Chargement des médicaments génériques associés aux médicaments allergiques déclarés
            'TraitementAllergies(Me.SelectedPatient)
        Else
            ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem.Enabled = False
        End If

        'Traitements arrêtés
        Dim isTraitementArret As Boolean = False
        Dim TraitementArretTooltip As String = ""
        Dim DateArretString, TraitementArretString, TraitementArretMedicament, TraitementArretCommentaire As String
        Dim DateArret As Date
        Dim traitementArretDatatable As DataTable
        traitementArretDatatable = traitementDao.getAllTraitementArreteByPatient(Me.SelectedPatient.patientId)
        rowCount = traitementArretDatatable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            isTraitementArret = True
            TraitementArretMedicament = Coalesce(traitementArretDatatable.Rows(i)("oa_traitement_medicament_dci"), "")
            TraitementArretCommentaire = Coalesce(traitementArretDatatable.Rows(i)("oa_traitement_arret_commentaire"), "")
            DateArret = Coalesce(traitementArretDatatable.Rows(i)("oa_traitement_date_modification"), Nothing)
            DateArretString = outils.FormatageDateAffichage(DateArret, True)
            TraitementArretString = TraitementArretMedicament & " (" & DateArretString & ")  " & TraitementArretCommentaire & vbCrLf
            TraitementArretTooltip += TraitementArretString
        Next
        If isTraitementArret Then
            ToolTip.SetToolTip(LblTraitementArret, TraitementArretTooltip)
            LblTraitementArret.Show()
        Else
            LblTraitementArret.Hide()
        End If

        'Positionnement du grid sur la première occurrence
        If RadTraitementDataGridView.Rows.Count > 0 Then
            Me.RadTraitementDataGridView.CurrentRow = RadTraitementDataGridView.Rows(0)
        End If
        Cursor.Current = Cursors.Default
    End Sub


    Private Sub RafraichirLécranToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RafraichirLécranToolStripMenuItem.Click
        ChargementTraitement()
    End Sub


    'Tooltip commentaire sur posologie
    Private Sub RadTraitementDataGridView_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RadTraitementDataGridView.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "posologie" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("commentairePosologie").Value
        End If
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "dateModification" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("commentaire").Value
        End If
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "medicamentDci" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("medicamentDci").Value
        End If
    End Sub

    'Création d'un traitement
    Private Sub CréerUnTraitementToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles CréerUnTraitementToolStripMenuItem1.Click
        Dim SelectedMedicamentCis As Integer
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using vFMedocSelecteur As New RadFMedocSelecteur
            vFMedocSelecteur.SelectedPatient = Me.SelectedPatient
            vFMedocSelecteur.Allergie = Me.Allergie
            vFMedocSelecteur.ContreIndication = Me.ContreIndication
            vFMedocSelecteur.ShowDialog() 'Modal
            SelectedMedicamentCis = vFMedocSelecteur.SelectedMedicamentCis
            'Si un médicament a été sélectionné
            If SelectedMedicamentCis <> 0 Then
                Using vFTraitementDetailEdit As New RadFTraitementDetailEdit
                    vFTraitementDetailEdit.SelectedPatient = Me.SelectedPatient
                    vFTraitementDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFTraitementDetailEdit.SelectedMedicamentCis = SelectedMedicamentCis
                    vFTraitementDetailEdit.Allergie = Me.Allergie
                    vFTraitementDetailEdit.ContreIndication = Me.ContreIndication
                    vFTraitementDetailEdit.SelectedTraitementId = 0
                    vFTraitementDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                    vFTraitementDetailEdit.ShowDialog() 'Modal
                    'Si le traitement a été créé, on recharge la grid
                    If vFTraitementDetailEdit.CodeRetour = True Then
                        ChargementTraitement()
                    End If
                End Using
            End If
        End Using
        Me.Enabled = True
    End Sub

    'Affichage du détail d'un traitement dans un popup
    Private Sub RadTraitementDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadTraitementDataGridView.CellDoubleClick
        If RadTraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadTraitementDataGridView.Rows.IndexOf(Me.RadTraitementDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim TraitementId, SelectedMedicamentCis As Integer
                TraitementId = RadTraitementDataGridView.Rows(aRow).Cells("TraitementId").Value
                SelectedMedicamentCis = RadTraitementDataGridView.Rows(aRow).Cells("MedicamentCis").Value
                Cursor.Current = Cursors.WaitCursor
                Me.Enabled = False
                Using vFTraitementDetailEdit As New RadFTraitementDetailEdit
                    vFTraitementDetailEdit.SelectedTraitementId = TraitementId
                    vFTraitementDetailEdit.SelectedPatient = Me.SelectedPatient
                    vFTraitementDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFTraitementDetailEdit.SelectedMedicamentCis = SelectedMedicamentCis
                    vFTraitementDetailEdit.Allergie = Me.Allergie
                    vFTraitementDetailEdit.ContreIndication = Me.ContreIndication
                    vFTraitementDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                    vFTraitementDetailEdit.ShowDialog() 'Modal
                    If vFTraitementDetailEdit.CodeRetour = True Then
                        ChargementTraitement()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Liste des traitements obsolètes
    Private Sub TraitementsObsoletesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TraitementsObsoletesToolStripMenuItem.Click
        'Traitement : afficher les traitement stoppés dans un popup dédié
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFTraitementObsoletes As New RadFTraitementObsoletes
            vFTraitementObsoletes.SelectedPatient = Me.SelectedPatient
            vFTraitementObsoletes.UtilisateurConnecte = Me.UtilisateurConnecte
            vFTraitementObsoletes.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
    End Sub

    'Liste des substances contre-indiquées
    Private Sub LblContreIndication_Click(sender As Object, e As EventArgs) Handles LblContreIndication.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPatientContreIndicationListe As New RadFPatientContreIndicationListe
            vFPatientContreIndicationListe.SelectedPatient = Me.SelectedPatient
            vFPatientContreIndicationListe.SelectedPatientId = Me.SelectedPatient.patientId
            vFPatientContreIndicationListe.SelectedPatientCICis = Me.SelectedPatient.PatientContreIndicationCis
            vFPatientContreIndicationListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientContreIndicationListe.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
    End Sub

    Private Sub ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem.Click
        If ContreIndication = True Then
            Me.Enabled = False
            Cursor.Current = Cursors.WaitCursor
            Using vFPatientContreIndicationListe As New RadFPatientContreIndicationListe
                vFPatientContreIndicationListe.SelectedPatient = Me.SelectedPatient
                vFPatientContreIndicationListe.SelectedPatientId = Me.SelectedPatient.patientId
                vFPatientContreIndicationListe.SelectedPatientCICis = Me.SelectedPatient.PatientContreIndicationCis
                vFPatientContreIndicationListe.UtilisateurConnecte = Me.UtilisateurConnecte
                vFPatientContreIndicationListe.ShowDialog() 'Modal
            End Using
            Me.Enabled = True
        End If
    End Sub

    'Liste des substances allergiques
    Private Sub ListeDesMedicamentsDeclaresAllergiquesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPatientAllergieListe As New RadFPatientAllergieListe
            vFPatientAllergieListe.SelectedPatient = Me.SelectedPatient
            vFPatientAllergieListe.SelectedPatientId = Me.SelectedPatient.patientId
            vFPatientAllergieListe.SelectedPatientAllergieCis = Me.SelectedPatient.PatientAllergieCis
            vFPatientAllergieListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientAllergieListe.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
    End Sub

    Private Sub LblAllergie_Click(sender As Object, e As EventArgs) Handles LblAllergie.Click
        'Traitement : afficher les allergies dans un popup
        If Allergie = True Then
            Me.Enabled = False
            Cursor.Current = Cursors.WaitCursor
            Using vFPatientAllergieListe As New RadFPatientAllergieListe
                vFPatientAllergieListe.SelectedPatient = Me.SelectedPatient
                vFPatientAllergieListe.SelectedPatientId = Me.SelectedPatient.patientId
                vFPatientAllergieListe.SelectedPatientAllergieCis = Me.SelectedPatient.PatientAllergieCis
                vFPatientAllergieListe.UtilisateurConnecte = Me.UtilisateurConnecte
                vFPatientAllergieListe.ShowDialog() 'Modal
            End Using
            Me.Enabled = True
        End If
    End Sub

    'Visualisation de l'historique des actions réalisées sur un traitement
    Private Sub HistoriqueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HistoriqueToolStripMenuItem.Click
        If RadTraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadTraitementDataGridView.Rows.IndexOf(Me.RadTraitementDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim TraitementId As Integer = RadTraitementDataGridView.Rows(aRow).Cells("traitementId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vFTraitementHistoListe As New RadFTraitementHistoListe
                    vFTraitementHistoListe.SelectedTraitementId = TraitementId
                    vFTraitementHistoListe.SelectedPatient = Me.SelectedPatient
                    vFTraitementHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFTraitementHistoListe.MedicamentDenomination = RadTraitementDataGridView.Rows(aRow).Cells("medicamentDci").Value
                    vFTraitementHistoListe.ShowDialog() 'Modal
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Gestion d'une fenêtre thérapeutique pour un traitement donné
    Private Sub GérerUneFenetreTherapeutiqueToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles GérerUneFenêtreThérapeutiqueToolStripMenuItem.Click
        If RadTraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadTraitementDataGridView.Rows.IndexOf(Me.RadTraitementDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim SelectedTraitementId As Integer = RadTraitementDataGridView.Rows(aRow).Cells("TraitementId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vFTraitementFenetreTh As New RadFTraitementFenetreTh
                    vFTraitementFenetreTh.SelectedPatient = Me.SelectedPatient
                    vFTraitementFenetreTh.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFTraitementFenetreTh.SelectedTraitementId = SelectedTraitementId
                    Dim fenetreTherapeutiqueExiste As Char = RadTraitementDataGridView.Rows(aRow).Cells("fenetreTherapeutique").Value
                    If fenetreTherapeutiqueExiste = "O" Then
                        vFTraitementFenetreTh.FenetreTherapeutiqueExiste = True
                    Else
                        vFTraitementFenetreTh.FenetreTherapeutiqueExiste = False
                    End If
                    vFTraitementFenetreTh.ShowDialog() 'Modal
                    'Si le traitement a été créé, on recharge la grid
                    If vFTraitementFenetreTh.CodeRetour = True Then
                        ChargementTraitement()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub OrdonnanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OrdonnanceToolStripMenuItem.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFOrdonnanceListe As New RadFOrdonnanceListe
            vFOrdonnanceListe.SelectedPatient = Me.SelectedPatient
            vFOrdonnanceListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFOrdonnanceListe.Allergie = Me.Allergie
            vFOrdonnanceListe.ContreIndication = Me.ContreIndication
            vFOrdonnanceListe.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
    End Sub


    '================================================================
    '======================= Parcours de soin =======================
    '================================================================

    'Chargement de la Grid
    Private Sub ChargementParcoursDeSoin()
        Dim ParcoursDataTable As DataTable
        Dim parcoursDao As New ParcoursDao
        Dim tacheDao As New TacheDao
        Dim SousCategorie, SpecialiteId As Integer
        Dim IntervenantOasis As Boolean

        Cursor.Current = Cursors.WaitCursor
        ParcoursDataTable = parcoursDao.getAllParcoursbyPatient(SelectedPatient.patientId)

        RadParcoursDataGridView.Rows.Clear()

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = ParcoursDataTable.Rows.Count - 1
        Dim SpecialiteDescription As String
        Dim ParcoursCacher, ParcoursConsigneEnRouge As Boolean

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            Dim rorId As Integer = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_ror_id"), 0)
            ParcoursCacher = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_cacher"), False)
            If RadChkParcoursNonCache.Checked = True Then
                If ParcoursCacher = True Then
                    Continue For
                End If
            End If

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadParcoursDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadParcoursDataGridView.Rows(iGrid).Cells("parcoursId").Value = ParcoursDataTable.Rows(i)("oa_parcours_id")

            SpecialiteId = ParcoursDataTable.Rows(i)("oa_parcours_specialite")
            SpecialiteDescription = Environnement.Table_specialite.GetSpecialiteDescription(SpecialiteId)
            RadParcoursDataGridView.Rows(iGrid).Cells("specialite").Value = SpecialiteDescription

            'Nom intervenant et Structure
            IntervenantOasis = False
            ParcoursConsigneEnRouge = False
            SousCategorie = ParcoursDataTable.Rows(i)("oa_parcours_sous_categorie_id")
            Select Case SousCategorie
                Case EnumSousCategoriePPS.medecinReferent
                    IntervenantOasis = True
                    ParcoursListProfilsOasis.Add(EnumSpecialiteOasis.medecinReferent)
                Case EnumSousCategoriePPS.IDE
                    IntervenantOasis = True
                    ParcoursListProfilsOasis.Add(EnumSpecialiteOasis.IDE)
                    Dim pacoursConsigneDao As New ParcoursConsigneDao
                    If pacoursConsigneDao.ExisteParcoursConsigne(ParcoursDataTable.Rows(i)("oa_parcours_id")) = False Then
                        ParcoursConsigneEnRouge = True
                    End If
                Case EnumSousCategoriePPS.sageFemme
                    If ParcoursDataTable.Rows(i)("oa_parcours_intervenant_oasis") = True Then
                        IntervenantOasis = True
                        ParcoursListProfilsOasis.Add(EnumSpecialiteOasis.sageFemmeOasis)
                    End If
                Case EnumSousCategoriePPS.specialiste
            End Select

            If IntervenantOasis = True Then
                RadParcoursDataGridView.Rows(iGrid).Cells("nomIntervenant").Value = "Oasis"
                RadParcoursDataGridView.Rows(iGrid).Cells("nomStructure").Value = "Oasis"
            Else
                RadParcoursDataGridView.Rows(iGrid).Cells("nomIntervenant").Value = Coalesce(ParcoursDataTable.Rows(i)("oa_ror_nom"), "")
                RadParcoursDataGridView.Rows(iGrid).Cells("nomStructure").Value = Coalesce(ParcoursDataTable.Rows(i)("oa_ror_structure_nom"), "")
            End If

            'Recherche de la dernière consultation
            Dim dateLast, dateNext As Date
            Dim TypeDemandeRdv As String
            'Dim tache As Tache

            RadParcoursDataGridView.Rows(iGrid).Cells("consultationLast").Value = "-"
            dateLast = Coalesce(ParcoursDataTable.Rows(i)("LastRendezVous"), Nothing)
            'If tache.DateRendezVous <> Nothing Then
            If dateLast <> Nothing Then
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationLast").Value = outils.FormatageDateAffichage(dateLast, True)
            End If

            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = "-"
            dateNext = Coalesce(ParcoursDataTable.Rows(i)("NextRendezVous"), Nothing)
            If dateNext <> Nothing Then
                'Rendez-vous planifiée
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = dateNext.ToString("dd.MM.yyyy")
            Else
                'Recherche si existe demande de rendez-vous
                dateNext = Coalesce(ParcoursDataTable.Rows(i)("DateDemandeRdv"), Nothing)
                If dateNext <> Nothing Then
                    'Rendez-vous prévisionnel, demande en cours
                    TypeDemandeRdv = Coalesce(ParcoursDataTable.Rows(i)("TypeDemandeRdv"), "")
                    Select Case TypeDemandeRdv
                        Case TacheDao.typeDemandeRendezVous.ANNEE.ToString
                            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = dateNext.ToString("yyyy")
                        Case TacheDao.typeDemandeRendezVous.ANNEEMOIS.ToString
                            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = dateNext.ToString("MM.yyyy")
                        Case Else
                            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = outils.FormatageDateAffichage(dateNext, True)
                    End Select
                Else
                    Dim Rythme As Integer = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_rythme"), 0)
                    Dim Base As String = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_base"), 0)
                    If Rythme <> 0 And Base <> "" Then
                        If dateLast <> Nothing Then
                            'Rendez-vous prévisionnel, calculé selon le rythme saisi et le dernier rendez-vous passé
                            dateNext = CalculProchainRendezVous(dateLast, Rythme, Base)
                            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = outils.FormatageDateAffichage(dateNext, True)
                        Else
                            Dim DateCreation As Date = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_date_creation"), Nothing)
                            If DateCreation <> Nothing Then
                                'Rendez-vous prévisionnel, calculé selon le rythme saisi et la date de création de l'intervenant dans le parcours de soin du patient
                                dateNext = CalculProchainRendezVous(DateCreation, Rythme, Base)
                                RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = outils.FormatageDateAffichage(dateNext, True)
                            Else
                                'Rendez-vous à venir non calculable
                            End If
                        End If
                    Else
                        'Pas de rendez-vous à venir pour cet intervenant
                    End If
                End If
            End If

            RadParcoursDataGridView.Rows(iGrid).Cells("commentaire").Value = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_commentaire"), "")

            If ParcoursCacher = True Then
                RadParcoursDataGridView.Rows(iGrid).Cells("specialite").Style.ForeColor = Color.CornflowerBlue
                RadParcoursDataGridView.Rows(iGrid).Cells("nomIntervenant").Style.ForeColor = Color.CornflowerBlue
                RadParcoursDataGridView.Rows(iGrid).Cells("nomStructure").Style.ForeColor = Color.CornflowerBlue
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationLast").Style.ForeColor = Color.CornflowerBlue
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationnext").Style.ForeColor = Color.CornflowerBlue
                RadParcoursDataGridView.Rows(iGrid).Cells("commentaire").Style.ForeColor = Color.CornflowerBlue
            End If

            If ParcoursConsigneEnRouge = True Then
                RadParcoursDataGridView.Rows(iGrid).Cells("specialite").Style.ForeColor = Color.Red
                RadParcoursDataGridView.Rows(iGrid).Cells("nomIntervenant").Style.ForeColor = Color.Red
                RadParcoursDataGridView.Rows(iGrid).Cells("nomStructure").Style.ForeColor = Color.Red
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationLast").Style.ForeColor = Color.Red
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationnext").Style.ForeColor = Color.Red
                RadParcoursDataGridView.Rows(iGrid).Cells("commentaire").Style.ForeColor = Color.Red
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadParcoursDataGridView.Rows.Count > 0 Then
            Me.RadParcoursDataGridView.CurrentRow = RadParcoursDataGridView.Rows(0)
        End If
        Cursor.Current = Cursors.Default
    End Sub


    Private Sub RafraichirLaffichageToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RafraichirLaffichageToolStripMenuItem.Click
        ChargementParcoursDeSoin()
    End Sub

    'Créer un inetervenant
    Private Sub CréerUnIntervenantToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUnIntervenantToolStripMenuItem.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using vFSpecialiteSelecteur As New RadFSpecialiteSelecteur
            vFSpecialiteSelecteur.ListProfilOasis = ParcoursListProfilsOasis
            vFSpecialiteSelecteur.ShowDialog()                  'Sélection de spécialité
            If vFSpecialiteSelecteur.SelectedSpecialiteId <> 0 Then
                Using vRadFRorListe As New RadFRorListe
                    vRadFRorListe.Selecteur = True
                    vRadFRorListe.PatientId = Me.SelectedPatient.patientId
                    vRadFRorListe.SpecialiteId = vFSpecialiteSelecteur.SelectedSpecialiteId
                    vRadFRorListe.TypeRor = "Intervenant"
                    vRadFRorListe.ShowDialog()                  'Sélection d'un professionnel de santé
                    If vRadFRorListe.CodeRetour = True Then
                        Using vFParcoursDetailEdit As New RadFParcoursDetailEdit
                            vFParcoursDetailEdit.SelectedParcoursId = 0
                            vFParcoursDetailEdit.SelectedRorId = vRadFRorListe.SelectedRorId
                            vFParcoursDetailEdit.SelectedSpecialiteId = vFSpecialiteSelecteur.SelectedSpecialiteId
                            vFParcoursDetailEdit.SelectedPatient = Me.SelectedPatient
                            vFParcoursDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                            vFParcoursDetailEdit.RythmeObligatoire = False
                            vFParcoursDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                            vFParcoursDetailEdit.ShowDialog()   'Gestion de l'intervenant
                            If vFParcoursDetailEdit.CodeRetour = True Then
                                ChargementParcoursDeSoin()
                                ChargementPPS()
                            End If
                        End Using
                    End If
                End Using
            End If
        End Using
        Me.Enabled = True
    End Sub

    'Modifier un intervenant
    Private Sub RadParcoursDataGridView_CellDoubleClick(sender As Object, e As GridViewCellEventArgs) Handles RadParcoursDataGridView.CellDoubleClick
        If RadParcoursDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadParcoursDataGridView.Rows.IndexOf(Me.RadParcoursDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ParcoursId As Integer = RadParcoursDataGridView.Rows(aRow).Cells("parcoursId").Value
                Cursor.Current = Cursors.WaitCursor
                Me.Enabled = False
                Using vFParcoursDetailEdit As New RadFParcoursDetailEdit
                    vFParcoursDetailEdit.SelectedParcoursId = ParcoursId
                    vFParcoursDetailEdit.SelectedPatient = Me.SelectedPatient
                    vFParcoursDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFParcoursDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                    vFParcoursDetailEdit.ShowDialog() 'Modal
                    If vFParcoursDetailEdit.CodeRetour = True Then
                        ChargementParcoursDeSoin()
                        ChargementPPS()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Historique
    Private Sub HistoriqueDesModificationsToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem2.Click
        If RadParcoursDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadParcoursDataGridView.Rows.IndexOf(Me.RadParcoursDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ParcoursId As Integer = RadParcoursDataGridView.Rows(aRow).Cells("parcoursId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vRadFParcoursHistoListe As New RadFParcoursHistoListe
                    vRadFParcoursHistoListe.SelectedParcoursId = ParcoursId
                    vRadFParcoursHistoListe.SelectedPatient = Me.SelectedPatient
                    vRadFParcoursHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                    vRadFParcoursHistoListe.ShowDialog()
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadChkParcoursNonCache_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles RadChkParcoursNonCache.ToggleStateChanged
        If RadChkParcoursNonCache.Checked = True Then
            RadChkParcoursTous.Checked = False
            If InitParcoursNonCache = True Then
                Application.DoEvents()
                RadParcoursDataGridView.Rows.Clear()
                ChargementParcoursDeSoin()
            Else
                InitParcoursNonCache = True
            End If
        Else
            If RadChkParcoursTous.Checked = False Then
                RadChkParcoursNonCache.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkParcoursTous_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles RadChkParcoursTous.ToggleStateChanged
        If RadChkParcoursTous.Checked = True Then
            RadChkParcoursNonCache.Checked = False
            Application.DoEvents()
            RadParcoursDataGridView.Rows.Clear()
            ChargementParcoursDeSoin()
        Else
            If RadChkParcoursNonCache.Checked = False Then
                RadChkParcoursTous.Checked = True
            End If
        End If
    End Sub


    Private Sub RadParcoursDataGridView_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadParcoursDataGridView.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    '================================================================
    '======================= Contexte ===============================
    '================================================================

    'Chargement de la Grid
    Private Sub ChargementContexte()
        Dim contexteDataTable As DataTable
        Dim antecedentDao As AntecedentDao = New AntecedentDao

        Cursor.Current = Cursors.WaitCursor
        If RadChkContextePublie.Checked = True Then
            contexteDataTable = antecedentDao.GetContextebyPatient(SelectedPatient.patientId, True)
        Else
            contexteDataTable = antecedentDao.GetContextebyPatient(SelectedPatient.patientId, False)
        End If

        RadContexteDataGridView.Rows.Clear()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateFin, dateModification As Date
        Dim AfficheDateModification, diagnostic As String
        Dim ordreAffichage As Integer
        Dim rowCount As Integer = contexteDataTable.Rows.Count - 1
        Dim categorieContexte, categorieContexteString As String
        Dim contexteCache As Boolean

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            categorieContexte = ""
            If contexteDataTable.Rows(i)("oa_antecedent_categorie_contexte") IsNot DBNull.Value Then
                categorieContexte = contexteDataTable.Rows(i)("oa_antecedent_categorie_contexte")
            End If
            Select Case categorieContexte
                Case ContexteDao.EnumParcoursBaseCode.Medical
                    categorieContexteString = ContexteDao.EnumParcoursBaseItem.Medical
                Case ContexteDao.EnumParcoursBaseCode.BioEnvironnemental
                    categorieContexteString = ContexteDao.EnumParcoursBaseItem.BioEnvironnemental
                Case Else
                    categorieContexteString = ""
            End Select

            'DateFin
            If contexteDataTable.Rows(i)("oa_antecedent_date_fin") IsNot DBNull.Value Then
                dateFin = contexteDataTable.Rows(i)("oa_antecedent_date_fin")
            Else
                dateFin = "31/12/9999"
            End If

            'Recherche si le contexte a été modifié (médical uniquement)
            AfficheDateModification = ""
            If categorieContexte = "M" Then
                If contexteDataTable.Rows(i)("oa_antecedent_date_modification") IsNot DBNull.Value Then
                    dateModification = contexteDataTable.Rows(i)("oa_antecedent_date_modification")
                    AfficheDateModification = FormatageDateAffichage(dateModification) + " : "
                Else
                    If contexteDataTable.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                        dateModification = contexteDataTable.Rows(i)("oa_antecedent_date_creation")
                        AfficheDateModification = FormatageDateAffichage(dateModification) + " : "
                    End If
                End If
            End If

            'Affichage de l'ordre d'affichage
            If contexteDataTable.Rows(i)("oa_antecedent_ordre_affichage1") IsNot DBNull.Value Then
                ordreAffichage = contexteDataTable.Rows(i)("oa_antecedent_ordre_affichage1")
            Else
                ordreAffichage = 0
            End If

            'prefixeContexte = "(Ordre : " + ordreAffichage.ToString + ") - "

            'Contexte caché
            contexteCache = False
            If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") = "C" Then
                    contexteCache = True
                End If
            End If

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadContexteDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadContexteDataGridView.Rows(iGrid).Cells("categorieContexte").Value = categorieContexteString

            diagnostic = ""
            If contexteDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                    diagnostic = "Suspicion de "
                Else
                    If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 3 Then
                        diagnostic = "Notion de "
                    End If
                End If
            End If

            'Affichage contexte ==========================
            Dim longueurString As Integer
            Dim longueurMax As Integer = 150
            Dim contexteDescription As String
            contexteDescription = Coalesce(contexteDataTable.Rows(i)("oa_antecedent_description"), "")
            If contexteDescription <> "" Then
                contexteDescription = Replace(contexteDescription, vbCrLf, " ")
                longueurString = contexteDescription.Length
                If longueurString > longueurMax Then
                    longueurString = longueurMax
                End If
                contexteDescription.Substring(0, longueurString)
            End If

            RadContexteDataGridView.Rows(iGrid).Cells("contexte").Value = AfficheDateModification & diagnostic & " " & contexteDescription
            '============================================

            If contexteCache = True Then
                RadContexteDataGridView.Rows(iGrid).Cells("contexte").Style.ForeColor = Color.CornflowerBlue
            End If

            'Identifiant contexte
            RadContexteDataGridView.Rows(iGrid).Cells("contexteId").Value = contexteDataTable.Rows(i)("oa_antecedent_id")
        Next

        'Positionnement du grid sur la première occurrence
        If RadContexteDataGridView.Rows.Count > 0 Then
            Me.RadContexteDataGridView.CurrentRow = RadContexteDataGridView.ChildRows(0)
        End If
        Cursor.Current = Cursors.Default
    End Sub


    Private Sub RafraichirLaffichageToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles RafraichirLaffichageToolStripMenuItem1.Click
        ChargementContexte()
    End Sub

    'Appel détail contexte
    Private Sub MasterTemplate_CellDoubleClick_1(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadContexteDataGridView.CellDoubleClick
        If RadContexteDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadContexteDataGridView.Rows.IndexOf(Me.RadContexteDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ContexteId As Integer = RadContexteDataGridView.Rows(aRow).Cells("ContexteId").Value
                Cursor.Current = Cursors.WaitCursor
                Me.Enabled = False
                Using vFContexteDetailEdit As New RadFContextedetailEdit
                    vFContexteDetailEdit.SelectedContexteId = ContexteId
                    vFContexteDetailEdit.SelectedPatient = Me.SelectedPatient
                    vFContexteDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFContexteDetailEdit.SelectedDrcId = 0
                    vFContexteDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                    vFContexteDetailEdit.ShowDialog() 'Modal
                    If vFContexteDetailEdit.CodeRetour = True Then
                        ChargementContexte()
                        If vFContexteDetailEdit.ContexteTransformeEnAntecedent = True Then
                            'Rechargement des contextes si réactivation
                            ChargementAntecedent()
                        End If
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub CreerUnContexteMenuItem1_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem1.Click
        Dim SelectedDrcId As Integer
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using vFDrcSelecteur As New RadFDRCSelecteur
            vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
            vFDrcSelecteur.CategorieOasis = 1       'Catégorie Oasis : "Antécédent et Contexte"
            vFDrcSelecteur.ShowDialog()             'Modal
            SelectedDrcId = vFDrcSelecteur.SelectedDrcId
            'Si un médicament a été sélectionné, on appelle le Formulaire de création
            If SelectedDrcId <> 0 Then
                Using vFContexteDetailEdit As New RadFContextedetailEdit
                    vFContexteDetailEdit.SelectedPatient = Me.SelectedPatient
                    vFContexteDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFContexteDetailEdit.SelectedDrcId = SelectedDrcId
                    vFContexteDetailEdit.SelectedContexteId = 0
                    vFContexteDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                    vFContexteDetailEdit.ShowDialog() 'Modal
                    'Si le traitement a été créé, on recharge la grid
                    If vFContexteDetailEdit.CodeRetour = True Then
                        ChargementContexte()
                    End If
                End Using
            End If
        End Using
        Me.Enabled = True
    End Sub

    Private Sub HistoriqueDesModificationsToolStripMenuItem1_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem1.Click
        If RadContexteDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadContexteDataGridView.Rows.IndexOf(Me.RadContexteDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ContexteId As Integer = RadContexteDataGridView.Rows(aRow).Cells("ContexteId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Using vFAntecedenttHistoListe As New RadFAntecedentHistoListe
                    vFAntecedenttHistoListe.SelectedAntecedentId = ContexteId
                    vFAntecedenttHistoListe.SelectedPatient = Me.SelectedPatient
                    vFAntecedenttHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFAntecedenttHistoListe.ShowDialog() 'Modal
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadChkContextePublie_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkContextePublie.ToggleStateChanged
        If RadChkContextePublie.Checked = True Then
            RadChkContexteTous.Checked = False
            If InitContextePublie = True Then
                Application.DoEvents()
                RadContexteDataGridView.Rows.Clear()
                ChargementContexte()
            Else
                InitContextePublie = True
            End If
        Else
            If RadChkContexteTous.Checked = False Then
                RadChkContextePublie.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkContexteTous_CheckStateChanged(sender As Object, e As EventArgs) Handles RadChkContexteTous.CheckStateChanged
        If RadChkContexteTous.Checked = True Then
            RadChkContextePublie.Checked = False
            Application.DoEvents()
            RadContexteDataGridView.Rows.Clear()
            ChargementContexte()
        Else
            If RadChkContextePublie.Checked = False Then
                RadChkContexteTous.Checked = True
            End If
        End If
    End Sub


    Private Sub RadContexteDataGridView_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadContexteDataGridView.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub


    '===================================================
    '======================= PPS =======================
    '===================================================

    'Chargement de la Grid
    Private Sub ChargementPPS()
        Dim PPSDataTable As DataTable
        Dim PPSDao As PpsDao = New PpsDao

        Cursor.Current = Cursors.WaitCursor
        PPSDataTable = PPSDao.getAllPPSbyPatient(SelectedPatient.patientId)

        RadPPSDataGridView.Rows.Clear()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i, mesureCount As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateDebut, dateModification As Date
        Dim rowCount As Integer = PPSDataTable.Rows.Count - 1
        Dim categoriePPS, sousCategoriePPS, Rythme, SpecialiteId As Integer
        Dim ppsArret As Boolean
        Dim mesureMax As Boolean = False
        Dim NaturePPS, CommentairePPS, commentaireParcours, AffichePPS, AfficheDateModificationPPS, AfficheDateModificationParcours, Base, BaseItem, SpecialiteDescription As String

        PPSSuiviIdeExiste = False
        PPSSuiviMedecinExiste = False
        PPSSuiviSageFemmeExiste = False

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            categoriePPS = 0
            If PPSDataTable.Rows(i)("oa_r_pps_categorie_id") IsNot DBNull.Value Then
                categoriePPS = PPSDataTable.Rows(i)("oa_r_pps_categorie_id")
            End If

            sousCategoriePPS = 0
            If PPSDataTable.Rows(i)("oa_r_pps_sous_categorie_id") IsNot DBNull.Value Then
                sousCategoriePPS = PPSDataTable.Rows(i)("oa_r_pps_sous_categorie_id")
            End If

            'Date de début
            If PPSDataTable.Rows(i)("oa_pps_date_debut") IsNot DBNull.Value Then
                dateDebut = PPSDataTable.Rows(i)("oa_pps_date_debut")
            Else
                dateDebut = "01/01/1900"
            End If

            'Rythme
            Rythme = Coalesce(PPSDataTable.Rows(i)("oa_parcours_rythme"), 0)
            Base = Coalesce(PPSDataTable.Rows(i)("oa_parcours_base"), "")
            Select Case Base
                Case ParcoursDao.EnumParcoursBaseCode.Quotidien
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.Quotidien
                Case ParcoursDao.EnumParcoursBaseCode.Hebdomadaire
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.Hebdomadaire
                Case ParcoursDao.EnumParcoursBaseCode.ParMois
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.ParMois
                Case ParcoursDao.EnumParcoursBaseCode.ParAn
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.ParAn
                Case ParcoursDao.EnumParcoursBaseCode.TousLes2Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes2Ans
                Case ParcoursDao.EnumParcoursBaseCode.TousLes3Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes3Ans
                Case ParcoursDao.EnumParcoursBaseCode.TousLes4Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes4Ans
                Case ParcoursDao.EnumParcoursBaseCode.TousLes5Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes5Ans
                Case Else
                    BaseItem = ""
            End Select

            CommentairePPS = Coalesce(PPSDataTable.Rows(i)("oa_pps_commentaire"), "")
            commentaireParcours = Coalesce(PPSDataTable.Rows(i)("oa_parcours_commentaire"), "")

            'Détecter si les occurrences qui doivent être uniques existent pour ce patient
            If categoriePPS = 3 Then
                Select Case sousCategoriePPS
                    Case 3
                        PPSSuiviIdeExiste = True
                    Case 4
                        PPSSuiviMedecinExiste = True
                    Case 5
                        PPSSuiviSageFemmeExiste = True
                End Select
            End If

            'Recherche si le pps a été modifié
            AfficheDateModificationPPS = ""
            If PPSDataTable.Rows(i)("oa_pps_date_modification") IsNot DBNull.Value Then
                dateModification = PPSDataTable.Rows(i)("oa_pps_date_modification")
                AfficheDateModificationPPS = FormatageDateAffichage(dateModification) + " : "
            Else
                If PPSDataTable.Rows(i)("oa_pps_date_creation") IsNot DBNull.Value Then
                    dateModification = PPSDataTable.Rows(i)("oa_pps_date_creation")
                    AfficheDateModificationPPS = FormatageDateAffichage(dateModification) + " : "
                End If
            End If

            'Recherche si le parcours a été modifié
            AfficheDateModificationParcours = ""
            If PPSDataTable.Rows(i)("oa_parcours_date_modification") IsNot DBNull.Value Then
                dateModification = PPSDataTable.Rows(i)("oa_parcours_date_modification")
                AfficheDateModificationParcours = FormatageDateAffichage(dateModification) + " : "
            Else
                If PPSDataTable.Rows(i)("oa_parcours_date_creation") IsNot DBNull.Value Then
                    dateModification = PPSDataTable.Rows(i)("oa_parcours_date_creation")
                    AfficheDateModificationParcours = FormatageDateAffichage(dateModification) + " : "
                End If
            End If

            'PPS caché
            ppsArret = False
            If PPSDataTable.Rows(i)("oa_pps_arret") IsNot DBNull.Value Then
                If PPSDataTable.Rows(i)("oa_pps_arret") = "1" Then
                    ppsArret = True
                End If
            End If

            NaturePPS = ""
            AffichePPS = ""
            'Présentation PPS : Cible/Objectif de santé (commentaire)
            If categoriePPS = 1 Then
                NaturePPS = "Objectif santé : "
                AffichePPS = NaturePPS + " " + CommentairePPS
            End If

            If categoriePPS = 2 Then
                'Suivi mesures préventives (Code DRC, libellé DRC, commentaire)
                NaturePPS = "Mesures préventives : "
                AffichePPS = NaturePPS & " " & AfficheDateModificationPPS & " " & CommentairePPS
            End If

            SpecialiteDescription = ""
            'Présentation PPS : Suivi
            If categoriePPS = 3 Then
                'Un parcours caché ne doit être affiché
                Dim parcoursCache As Boolean = Coalesce(PPSDataTable.Rows(i)("oa_parcours_cacher"), False)
                If parcoursCache = True Then
                    'Continue For
                End If
                'Un suivi intervenant sans rythme ne doit pas être affiché dans le PPS
                If Rythme = 0 Then
                    Continue For
                End If

                'Suivi IDE, Médecin référent, Sage-femme et Spécialiste (Base, Rythme, Commentaire)
                Select Case sousCategoriePPS
                    Case 3
                        NaturePPS = "Suivi IDE : "
                    Case 4
                        NaturePPS = "Suivi médecin référent : "
                    Case 5
                        NaturePPS = "Suivi sage-femme : "
                    Case 6
                        'Récupération spécialité
                        If PPSDataTable.Rows(i)("oa_parcours_specialite") IsNot DBNull.Value Then
                            SpecialiteId = PPSDataTable.Rows(i)("oa_parcours_specialite")
                            SpecialiteDescription = Table_specialite.GetSpecialiteDescription(SpecialiteId)
                        End If
                        'TODO: récupération spécialité
                        NaturePPS = "Suivi " + SpecialiteDescription + " : "
                    Case Else
                        NaturePPS = "Inconnue "
                End Select
                If Base = ParcoursDao.EnumParcoursBaseCode.Hebdomadaire _
                    Or Base = ParcoursDao.EnumParcoursBaseCode.ParMois _
                    Or Base = ParcoursDao.EnumParcoursBaseCode.ParAn Then
                    AffichePPS = NaturePPS + Rythme.ToString + " / " + BaseItem + " " + CommentairePPS
                Else
                    AffichePPS = NaturePPS + BaseItem + " " + CommentairePPS
                End If
            End If

            'Présentation PPS : Stratégie contextuelle (Base, Rythme, Commentaire)
            If categoriePPS = 4 Then
                Select Case sousCategoriePPS
                    Case 7
                        NaturePPS = "Démarche prophylactique "
                    Case 8
                        NaturePPS = "Démarche sociale "
                    Case 9
                        NaturePPS = "Démarche symptomatique "
                    Case 10
                        NaturePPS = "Démarche curative "
                    Case 11
                        NaturePPS = "Démarche diagnostique "
                    Case 12
                        NaturePPS = "Démarche palliative "
                    Case Else
                        NaturePPS = "Inconnue "
                End Select
                AffichePPS = AfficheDateModificationPPS + NaturePPS + " " + CommentairePPS
            End If

            'Transformation des "Tab" et "Return" en espace pour afficher les éléments correctement
            AffichePPS = Replace(AffichePPS, vbTab, " ")
            AffichePPS = Replace(AffichePPS, vbCrLf, " ")

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadPPSDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            If ppsArret = True Then
                RadPPSDataGridView.Rows(iGrid).Cells("pps").Style.ForeColor = Color.Red
            End If

            'Affichage du PPS
            RadPPSDataGridView.Rows(iGrid).Cells("pps").Value = AffichePPS

            'Identifiant pps
            RadPPSDataGridView.Rows(iGrid).Cells("ppsId").Value = PPSDataTable.Rows(i)("oa_pps_id")
            RadPPSDataGridView.Rows(iGrid).Cells("parcoursId").Value = PPSDataTable.Rows(i)("oa_parcours_id")

            RadPPSDataGridView.Rows(iGrid).Cells("categorieId").Value = categoriePPS
            RadPPSDataGridView.Rows(iGrid).Cells("sousCategorieId").Value = sousCategoriePPS
            RadPPSDataGridView.Rows(iGrid).Cells("specialiteId").Value = SpecialiteId
        Next

        'Positionnement du grid sur la première occurrence
        If RadPPSDataGridView.Rows.Count > 0 Then
            Me.RadPPSDataGridView.CurrentRow = RadPPSDataGridView.ChildRows(0)
        End If
        Cursor.Current = Cursors.Default
    End Sub


    Private Sub RafraichirLaffichageToolStripMenuItem2_Click(sender As Object, e As EventArgs) Handles RafraichirLaffichageToolStripMenuItem2.Click
        ChargementPPS()
    End Sub

    Private Sub RadPPSDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadPPSDataGridView.CellDoubleClick
        'Appeler selon la nature du PPS le DetailEdit correspondant
        If RadPPSDataGridView.CurrentRow IsNot Nothing Then
            Dim PPSId, ParcoursId, categoriePPS, sousCategoriePPS, SpecialiteId As Integer
            Dim aRow As Integer = Me.RadPPSDataGridView.Rows.IndexOf(Me.RadPPSDataGridView.CurrentRow)
            If aRow >= 0 Then
                PPSId = RadPPSDataGridView.Rows(aRow).Cells("ppsId").Value
                ParcoursId = RadPPSDataGridView.Rows(aRow).Cells("parcoursId").Value
                categoriePPS = RadPPSDataGridView.Rows(aRow).Cells("categorieId").Value
                sousCategoriePPS = RadPPSDataGridView.Rows(aRow).Cells("sousCategorieId").Value
                SpecialiteId = RadPPSDataGridView.Rows(aRow).Cells("specialiteId").Value

                Select Case categoriePPS
                    Case 1 'Objectif de santé
                        Cursor.Current = Cursors.WaitCursor
                        Me.Enabled = False
                        Using vRadFPPSObjectifSanteDetail As New RadFPPSDetailEdit
                            vRadFPPSObjectifSanteDetail.PPSId = PPSId
                            vRadFPPSObjectifSanteDetail.CategoriePPS = EnumCategoriePPS.Objectif
                            vRadFPPSObjectifSanteDetail.SelectedPatient = Me.SelectedPatient
                            vRadFPPSObjectifSanteDetail.UtilisateurConnecte = Me.UtilisateurConnecte
                            vRadFPPSObjectifSanteDetail.PositionGaucheDroite = EnumPosition.Gauche
                            vRadFPPSObjectifSanteDetail.ShowDialog() 'Modal
                            If vRadFPPSObjectifSanteDetail.CodeRetour = True Then
                                ChargementPPS()
                            End If
                        End Using
                        Me.Enabled = True
                    Case 2 'Mesure préventive
                        Cursor.Current = Cursors.WaitCursor
                        Me.Enabled = False
                        Using vFFPPSMesurePreventive As New RadFPPSDetailEdit
                            vFFPPSMesurePreventive.PPSId = PPSId
                            vFFPPSMesurePreventive.CategoriePPS = EnumCategoriePPS.MesurePreventive
                            vFFPPSMesurePreventive.SelectedPatient = Me.SelectedPatient
                            vFFPPSMesurePreventive.UtilisateurConnecte = Me.UtilisateurConnecte
                            vFFPPSMesurePreventive.PositionGaucheDroite = EnumPosition.Gauche
                            vFFPPSMesurePreventive.ShowDialog() 'Modal
                            If vFFPPSMesurePreventive.CodeRetour = True Then
                                ChargementPPS()
                            End If
                        End Using
                        Me.Enabled = True
                    Case 3 'Suivi intervenant
                        Cursor.Current = Cursors.WaitCursor
                        Me.Enabled = False
                        Using vFParcoursDetailEdit As New RadFParcoursDetailEdit
                            vFParcoursDetailEdit.SelectedParcoursId = ParcoursId
                            vFParcoursDetailEdit.SelectedPatient = Me.SelectedPatient
                            vFParcoursDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                            vFParcoursDetailEdit.RythmeObligatoire = False
                            vFParcoursDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                            vFParcoursDetailEdit.ShowDialog() 'Modal
                            If vFParcoursDetailEdit.CodeRetour = True Then
                                ChargementParcoursDeSoin()
                                ChargementPPS()
                            End If
                        End Using
                        Me.Enabled = True
                    Case 4 'Stratégie
                        Cursor.Current = Cursors.WaitCursor
                        Me.Enabled = False
                        Using vFPPSStrategie As New RadFPPSDetailEdit
                            vFPPSStrategie.PPSId = PPSId
                            vFPPSStrategie.CategoriePPS = EnumCategoriePPS.Strategie
                            vFPPSStrategie.SelectedPatient = Me.SelectedPatient
                            vFPPSStrategie.UtilisateurConnecte = Me.UtilisateurConnecte
                            vFPPSStrategie.PositionGaucheDroite = EnumPosition.Gauche
                            vFPPSStrategie.ShowDialog() 'Modal
                            If vFPPSStrategie.CodeRetour = True Then
                                ChargementPPS()
                            End If
                        End Using
                        Me.Enabled = True
                End Select
            End If
        End If
    End Sub


    Private Sub CréerUnObjectifDeSantéToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUnObjectifDeSantéToolStripMenuItem.Click
        'Contrôler si un objectif de santé valide existe
        Dim ppsdao As New PpsDao
        If ppsdao.ExistPPSObjectifByPatientId(SelectedPatient.patientId) = False Then
            Me.Enabled = False
            Cursor.Current = Cursors.WaitCursor
            Using vRadFPPSObjectifSanteDetail As New RadFPPSDetailEdit
                vRadFPPSObjectifSanteDetail.PPSId = 0
                vRadFPPSObjectifSanteDetail.CategoriePPS = EnumCategoriePPS.Objectif
                vRadFPPSObjectifSanteDetail.SelectedPatient = Me.SelectedPatient
                vRadFPPSObjectifSanteDetail.UtilisateurConnecte = Me.UtilisateurConnecte
                vRadFPPSObjectifSanteDetail.PositionGaucheDroite = EnumPosition.Gauche
                vRadFPPSObjectifSanteDetail.ShowDialog() 'Modal
                If vRadFPPSObjectifSanteDetail.CodeRetour = True Then
                    ChargementPPS()
                End If
            End Using
            Me.Enabled = True
        Else
            MessageBox.Show("Création impossible, un Objectif de santé existe déjà pour ce patient")
        End If
    End Sub

    Private Sub CréerUneMesurePréventiveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneMesurePréventiveToolStripMenuItem.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFFPPSMesurePreventive As New RadFPPSDetailEdit
            vFFPPSMesurePreventive.PPSId = 0
            vFFPPSMesurePreventive.CategoriePPS = EnumCategoriePPS.MesurePreventive
            vFFPPSMesurePreventive.SelectedPatient = Me.SelectedPatient
            vFFPPSMesurePreventive.UtilisateurConnecte = Me.UtilisateurConnecte
            vFFPPSMesurePreventive.PositionGaucheDroite = EnumPosition.Gauche
            vFFPPSMesurePreventive.ShowDialog() 'Modal
            If vFFPPSMesurePreventive.CodeRetour = True Then
                ChargementPPS()
            End If
        End Using
        Me.Enabled = True
    End Sub

    Private Sub CréerUneStratégieContextuelleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneStratégieContextuelleToolStripMenuItem.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPPSStrategie As New RadFPPSDetailEdit
            vFPPSStrategie.PPSId = 0
            vFPPSStrategie.CategoriePPS = EnumCategoriePPS.Strategie
            vFPPSStrategie.SelectedPatient = Me.SelectedPatient
            vFPPSStrategie.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPPSStrategie.PositionGaucheDroite = EnumPosition.Gauche
            vFPPSStrategie.ShowDialog() 'Modal
            If vFPPSStrategie.CodeRetour = True Then
                ChargementPPS()
            End If
        End Using
        Me.Enabled = True
    End Sub

    Private Sub CréerUnSuiviToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUnSuiviToolStripMenuItem.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using vFSpecialiteSelecteur As New RadFSpecialiteSelecteur
            vFSpecialiteSelecteur.ListProfilOasis = ParcoursListProfilsOasis
            vFSpecialiteSelecteur.ShowDialog()                  'Sélection de spécialité
            If vFSpecialiteSelecteur.SelectedSpecialiteId <> 0 Then
                Using vRadFRorListe As New RadFRorListe
                    vRadFRorListe.Selecteur = True
                    vRadFRorListe.PatientId = Me.SelectedPatient.patientId
                    vRadFRorListe.SpecialiteId = vFSpecialiteSelecteur.SelectedSpecialiteId
                    vRadFRorListe.TypeRor = "Intervenant"
                    vRadFRorListe.ShowDialog()                  'Sélection d'un professionnel de santé
                    If vRadFRorListe.CodeRetour = True Then
                        Using vFParcoursDetailEdit As New RadFParcoursDetailEdit
                            vFParcoursDetailEdit.SelectedParcoursId = 0
                            vFParcoursDetailEdit.SelectedRorId = vRadFRorListe.SelectedRorId
                            vFParcoursDetailEdit.SelectedSpecialiteId = vFSpecialiteSelecteur.SelectedSpecialiteId
                            vFParcoursDetailEdit.SelectedPatient = Me.SelectedPatient
                            vFParcoursDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                            vFParcoursDetailEdit.RythmeObligatoire = True
                            vFParcoursDetailEdit.PositionGaucheDroite = EnumPosition.Gauche
                            vFParcoursDetailEdit.ShowDialog()   'Gestion de l'intervenant
                            If vFParcoursDetailEdit.CodeRetour = True Then
                                ChargementParcoursDeSoin()
                                ChargementPPS()
                            End If
                        End Using
                    End If
                End Using
            End If
        End Using
        Me.Enabled = True
    End Sub

    Private Sub HistoriqueDesModificationsToolStripMenuItem3_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem3.Click
        If RadPPSDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadPPSDataGridView.Rows.IndexOf(Me.RadPPSDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim categoriePPS As Integer = RadPPSDataGridView.Rows(aRow).Cells("categorieId").Value
                If categoriePPS = 3 Then
                    Dim ParcoursId As Integer = RadPPSDataGridView.Rows(aRow).Cells("parcoursId").Value
                    Me.Enabled = False
                    Cursor.Current = Cursors.WaitCursor
                    Using vRadFParcoursHistoListe As New RadFParcoursHistoListe
                        vRadFParcoursHistoListe.SelectedParcoursId = ParcoursId
                        vRadFParcoursHistoListe.SelectedPatient = Me.SelectedPatient
                        vRadFParcoursHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                        vRadFParcoursHistoListe.ShowDialog()
                    End Using
                    Me.Enabled = True
                Else
                    Dim PPSId As Integer = RadPPSDataGridView.Rows(aRow).Cells("ppsId").Value
                    Me.Enabled = False
                    Cursor.Current = Cursors.WaitCursor
                    Using vFPPSHistoListe As New RadFPPSHistoListe
                        vFPPSHistoListe.SelectedPPSId = PPSId
                        vFPPSHistoListe.SelectedPatient = Me.SelectedPatient
                        vFPPSHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                        vFPPSHistoListe.ShowDialog()
                    End Using
                    Me.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub RadPPSDataGridView_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadPPSDataGridView.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub


    'Clôture de l'épisode
    Private Sub RadBtnCloture_Click(sender As Object, e As EventArgs) Handles RadBtnCloture.Click

        '>>> Bloc à supprimer pour réhabiliter le contrôle de clôture ========
        episode.Etat = EpisodeDao.EnumEtatEpisode.CLOTURE.ToString
        episode.DateModification = Date.Now()
        episode.UserModification = userLog.UtilisateurId
        If episodeDao.ModificationEpisode(episode) = True Then
            Dim form As New RadFNotification()
            form.Titre = "Notification épisode patient"
            form.Message = "=== Episode clôturé ==="
            form.Show()
            Close()
        End If
        Exit Sub
        '>>> Bloc à supprimer pour réhabiliter le contrôle de clôture ========

        If userLog.TypeProfil = episode.TypeProfil Then
            episode.Etat = EpisodeDao.EnumEtatEpisode.CLOTURE.ToString
            episode.DateModification = Date.Now()
            episode.UserModification = userLog.UtilisateurId
            If episodeDao.ModificationEpisode(episode) = True Then
                Dim form As New RadFNotification()
                form.Titre = "Notification épisode patient"
                form.Message = "=== Episode clôturé ==="
                form.Show()
                Close()
            End If
        Else
            MessageBox.Show("Vous devez disposer d'un profil '" & episode.TypeProfil &
                            "', pour pouvoir clôturer cet épisode patient" & vbCrLf &
                            "(Vous disposez d'un profil de type '" & userLog.TypeProfil & "')")
        End If

    End Sub



    '===========================================================
    '======================= Généralités =======================
    '===========================================================

    Private Sub initZones()
        LblParametre1.Text = ""
        LblParametre2.Text = ""
        LblParametre3.Text = ""
        'Etat civil
        LblPatientNIR.Text = ""
        LblPatientPrenom.Text = ""
        LblPatientNom.Text = ""
        LblPatientAge.Text = ""
        LblPatientGenre.Text = ""
        LblPatientSite.Text = ""
        LblPatientDateMaj.Text = ""
        'Initialisation des filtres d'affichage pour les antécédents et les contextes
        InitPublie = False
        InitParPriorite = False
        InitMajeur = False
        RadChkPublie.Checked = True
        RadChkParPriorite.Checked = True
        RadChkMajeurTous.Checked = True
        InitContextePublie = False
        RadChkContextePublie.Checked = True
        'Antécédents
        RadAntecedentDataGridView.Rows.Clear()
        'Traitements
        RadTraitementDataGridView.Rows.Clear()
        'Parcours de soin
        InitParcoursNonCache = False
        RadChkParcoursNonCache.Checked = True
        RadParcoursDataGridView.Rows.Clear()
        'Contexte
        RadContexteDataGridView.Rows.Clear()
        'PPS
        RadPPSDataGridView.Rows.Clear()
    End Sub

    Private Sub ChargementParametreApplication()
        'Récupération du nom de l'organisation dans les paramètres de l'application
        Dim LongueurStringAllergieString As String = ConfigurationManager.AppSettings("longueurStringAllergie")
        If IsNumeric(LongueurStringAllergieString) Then
            LongueurStringAllergie = CInt(LongueurStringAllergieString)
        Else
            LongueurStringAllergie = 12
        End If
    End Sub


    '===========================================================
    '======================= Droits d'accès ====================
    '===========================================================
    Private Sub DroitAcces()
        InhibeAccesIDE()
        InhibeAccesMed()
        If userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString Or userLog.UtilisateurAdmin = True Then
            LibereAccesMed()
        End If
        If userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString Or userLog.UtilisateurAdmin = True Then
            LibereAccesIde()
        End If
    End Sub

    Private Sub InhibeAccesIDE()
        RadBtnWorkflowIde.Enabled = False
        RadPnlWorkflowIDE.Enabled = False
    End Sub

    Private Sub InhibeAccesMed()
        RadBtnWorkflowMed.Enabled = False
        RadPnlWorkflowMed.Enabled = False
    End Sub

    Private Sub LibereAccesIde()
        RadBtnWorkflowIde.Enabled = True
        RadPnlWorkflowIDE.Enabled = True
    End Sub

    Private Sub LibereAccesMed()
        RadBtnWorkflowMed.Enabled = True
        RadPnlWorkflowMed.Enabled = True
    End Sub


    '===========================================================
    '======================= Généralités =======================
    '===========================================================


    Private Sub RadBtnSocial_Click(sender As Object, e As EventArgs) Handles RadBtnSocial.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPatientNoteListe As New RadFPatientNoteListe
            vFPatientNoteListe.TypeNote = EnumTypeNote.Social
            vFPatientNoteListe.SelectedPatientId = Me.SelectedPatient.patientId
            vFPatientNoteListe.SelectedPatient = Me.SelectedPatient
            vFPatientNoteListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientNoteListe.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnVaccins_Click(sender As Object, e As EventArgs) Handles RadBtnVaccins.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPatientNoteListe As New RadFPatientNoteListe
            vFPatientNoteListe.TypeNote = EnumTypeNote.Vaccin
            vFPatientNoteListe.SelectedPatientId = Me.SelectedPatient.patientId
            vFPatientNoteListe.SelectedPatient = Me.SelectedPatient
            vFPatientNoteListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientNoteListe.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnNotesMedicales_Click(sender As Object, e As EventArgs) Handles RadBtnNotesMedicales.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPatientNoteListe As New RadFPatientNoteListe
            vFPatientNoteListe.TypeNote = EnumTypeNote.Medicale
            vFPatientNoteListe.SelectedPatientId = Me.SelectedPatient.patientId
            vFPatientNoteListe.SelectedPatient = Me.SelectedPatient
            vFPatientNoteListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientNoteListe.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnDirectives_Click(sender As Object, e As EventArgs) Handles RadBtnDirectives.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPatientNoteListe As New RadFPatientNoteListe
            vFPatientNoteListe.TypeNote = EnumTypeNote.Directive
            vFPatientNoteListe.SelectedPatientId = Me.SelectedPatient.patientId
            vFPatientNoteListe.SelectedPatient = Me.SelectedPatient
            vFPatientNoteListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientNoteListe.ShowDialog() 'Modal
        End Using
        Me.Enabled = True
    End Sub

    'Abandon
    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

End Class
