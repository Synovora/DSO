Imports Oasis_Common

Public Class RadFContextedetailEdit
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedContexteId As Integer
    Private privateSelectedDrcId As Long
    Private privateCodeRetour As Boolean
    Private _CodeResultat As Integer
    Private privateContexteTransformeEnAntecedent As Boolean
    Private _positionGaucheDroite As Integer
    Private _conclusionEpisode As Boolean
    Private _episode As Episode
    Property antecedents As List(Of Antecedent)
    Property relationChaineEpisodes As List(Of ChaineEpisode)

    Dim InitPublie As Boolean


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

    Property NewContext As Antecedent

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

    Dim antecedentDao As New AntecedentDao
    Dim chaineEpisodeDao As New ChaineEpisodeDao
    Dim contexteReadDao As New AntecedentDao
    Dim contexteDao As New ContexteDao
    Dim contexteRead As New Antecedent
    Dim contexteUpdate As New Antecedent

    Private Sub RadFContextedetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RadChkPublie.Checked = True
        RefreshChaineEpisode()
        If PositionGaucheDroite = EnumPosition.Droite Then
            Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - Me.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        Else
            Me.Location = New Point(10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        End If
        CodeResultat = EnumResultat.AttenteAction

        If ConclusionEpisode = True Then
            AfficheTitleForm(Me, "Détail contexte de conclusion d'épisode", userLog)
        Else
            AfficheTitleForm(Me, "Détail contexte", userLog)
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

            contexteUpdate.PatientId = SelectedPatient.PatientId
            contexteUpdate.Type = "C"
            contexteUpdate.Niveau = 1
            contexteUpdate.Nature = "Patient"
            contexteUpdate.Inactif = False
            RadValidation.Show()

            'Catégorie
            'CbxCategorieContexte.Text = ContexteCourrier.EnumParcoursBaseItem.Medical
            contexteUpdate.CategorieContexte = ContexteCourrier.EnumParcoursBaseCode.Medical
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
                If Episode.TypeActivite <> Episode.EnumTypeActiviteEpisodeCode.PATHOLOGIE_AIGUE Then
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

    Private Sub RefreshChaineEpisode()
        Dim filter = " AND oasis.oa_antecedent.oa_antecedent_type = 'A' AND (oasis.oa_antecedent.oa_antecedent_inactif = '0' OR oasis.oa_antecedent.oa_antecedent_inactif is Null)"

        If RadChkPublie.Checked = False Then
            antecedents = antecedentDao.GetListByPatient(SelectedPatient.PatientId, filter & " AND (oasis.oa_antecedent.oa_antecedent_statut_affichage = 'P' OR oasis.oa_antecedent.oa_antecedent_statut_affichage = 'C') ORDER BY oasis.oa_antecedent.oa_antecedent_ordre_affichage1, oasis.oa_antecedent.oa_antecedent_ordre_affichage2, oasis.oa_antecedent.oa_antecedent_ordre_affichage3;")
        Else
            antecedents = antecedentDao.GetListByPatient(SelectedPatient.PatientId, filter & " AND oasis.oa_antecedent.oa_antecedent_statut_affichage = 'P' ORDER BY oasis.oa_antecedent.oa_antecedent_ordre_affichage1, oasis.oa_antecedent.oa_antecedent_ordre_affichage2, oasis.oa_antecedent.oa_antecedent_ordre_affichage3;")
        End If
        relationChaineEpisodes = chaineEpisodeDao.GetList(SelectedContexteId)

        RadGridViewChaineEpisode.Rows.Clear()

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim indentation As String
        Dim dateDateModification, AldDateFin As Date
        Dim AfficheDateModification As String
        Dim diagnostic As String
        Dim antecedentCache, AldValide, AldValideOK, AldDemandeEnCours As Boolean
        Dim antecedentIdPrecedent1, antecedentIdPrecedent2 As Long
        antecedentIdPrecedent1 = 0
        antecedentIdPrecedent2 = 0

        'Parcours du DataTable pour alimenter le DataGridView
        For Each antecedent In antecedents
            Select Case antecedent.Niveau
                Case 1
                    indentation = ""
                Case 2
                    indentation = "           > "
                Case 3
                    indentation = "                        >> "
                Case Else
                    indentation = ""
            End Select

            AfficheDateModification = ""
            If antecedent.DateModification <> Nothing Then
                dateDateModification = antecedent.DateModification
                AfficheDateModification = " (" + FormatageDateAffichage(dateDateModification) + ")"
            Else
                If antecedent.DateCreation <> Nothing Then
                    dateDateModification = antecedent.DateCreation
                    AfficheDateModification = " (" + FormatageDateAffichage(dateDateModification) + ")"
                End If
            End If

            'Identification si l'antécédent est caché
            antecedentCache = False
            If antecedent.StatutAffichage <> Nothing Then
                If antecedent.StatutAffichage = "C" Then
                    antecedentCache = True
                End If
            End If

            AldValide = Coalesce(antecedent.AldValide, False)
            AldDateFin = Coalesce(antecedent.AldDateFin, Nothing)
            AldValideOK = False
            If AldValide = True Then
                If AldDateFin > Date.Now() Then
                    AldValideOK = True
                End If
            End If
            AldDemandeEnCours = Coalesce(antecedent.AldDemandeEnCours, False)

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadGridViewChaineEpisode.Rows.Add(iGrid)
            'Alimentation du DataGridView
            diagnostic = ""
            If antecedent.Diagnostic <> Nothing Then
                If CInt(antecedent.Diagnostic) = 2 Then
                    diagnostic = "Suspicion de : "
                Else
                    If CInt(antecedent.Diagnostic) = 3 Then
                        diagnostic = "Notion de : "
                    End If
                End If
            End If

            Dim antecedentDescription As String

            '===== Affichage antécédent
            If antecedent.Description = Nothing Or antecedent.Description = "" Then
                antecedentDescription = ""
            Else
                antecedentDescription = antecedent.Description
                antecedentDescription = Replace(antecedentDescription, vbCrLf, " ")
                RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedentDescription").Value = antecedent.Description
            End If

            Dim DescriptionDrcAld As String = ""
            If AldValideOK Or AldDemandeEnCours Then
                'DescriptionDrcAld = Coalesce(antecedentDataTable.Rows(i)("oa_ald_cim10_description"), "")
            End If

            RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedent").Value = indentation & diagnostic & DescriptionDrcAld & " " & antecedentDescription
            '==========

            If antecedentCache = True Then
                RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedent").Style.ForeColor = Color.CornflowerBlue
            Else
                If AldValideOK = True Then
                    RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedent").Style.ForeColor = Color.Red
                Else
                    If AldDemandeEnCours = True Then
                        RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedent").Style.ForeColor = Color.DarkOrange
                    End If
                End If
            End If

            If AldValideOK = True Or AldDemandeEnCours = True Then
                RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedentAld").Value = "X"
            Else
                RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedentAld").Value = ""
            End If

            RadGridViewChaineEpisode.Rows(iGrid).Cells("id").Value = antecedent.Id
            RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedentId").Value = antecedent.Id
            RadGridViewChaineEpisode.Rows(iGrid).Cells("selected").Value = relationChaineEpisodes.Any(Function(myObject) myObject.ChaineId = antecedent.Id)
            RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedentDrcId").Value = antecedent.DrcId
            RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedentNiveau").Value = antecedent.Niveau
            RadGridViewChaineEpisode.Rows(iGrid).Cells("ordreAffichage1").Value = Coalesce(antecedent.Ordre1, 0)
            RadGridViewChaineEpisode.Rows(iGrid).Cells("ordreAffichage2").Value = Coalesce(antecedent.Ordre2, 0)
            RadGridViewChaineEpisode.Rows(iGrid).Cells("ordreAffichage3").Value = Coalesce(antecedent.Ordre3, 0)
            RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedentIdNiveau1").Value = Coalesce(antecedent.Niveau1Id, 0)
            RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedentIdNiveau2").Value = Coalesce(antecedent.Niveau2Id, 0)

            'Déplacement horizontal, détermination de l'antécédent précédent
            Select Case antecedent.Niveau
                Case 1
                    RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedentIdPrecedent").Value = antecedentIdPrecedent1
                    antecedentIdPrecedent1 = antecedent.Id
                    antecedentIdPrecedent2 = 0
                Case 2
                    RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedentIdPrecedent").Value = antecedentIdPrecedent2
                    antecedentIdPrecedent2 = antecedent.Id
                Case 3
                    'Non concerné
            End Select

            'Déplacement vertical, détermination de l'antécédent pere si niveau 2 et 3
            Select Case CInt(antecedent.Niveau)
                Case 2
                    RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedentPereId").Value = antecedent.Niveau1Id
                Case 3
                    RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedentPereId").Value = antecedent.Niveau2Id
                Case Else
                    RadGridViewChaineEpisode.Rows(iGrid).Cells("antecedentPereId").Value = 0
            End Select
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewChaineEpisode.Rows.Count > 0 Then
            RadGridViewChaineEpisode.CurrentRow = RadGridViewChaineEpisode.Rows(0)
            RadGridViewChaineEpisode.TableElement.VScrollBar.Value = 0
        End If
    End Sub


    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir
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
        contexteUpdate = contexteReadDao.Clone(contexteRead)

        Dim dateDebut, dateFin, dateCreation, dateModification As Date
        Dim ordreAffichage As Integer
        Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)

        Select Case contexteRead.CategorieContexte
            Case ContexteCourrier.EnumParcoursBaseCode.Medical
                'CbxCategorieContexte.Text = ContexteCourrier.EnumParcoursBaseItem.Medical
                RadioBtnMedical.Checked = True
            Case ContexteCourrier.EnumParcoursBaseCode.BioEnvironnemental
                'CbxCategorieContexte.Text = ContexteCourrier.EnumParcoursBaseItem.BioEnvironnemental
                RadioBtnBioEnvironnemental.Checked = True
            Case Else
                'CbxCategorieContexte.Text = ContexteCourrier.EnumParcoursBaseItem.Medical
                contexteUpdate.CategorieContexte = ContexteCourrier.EnumParcoursBaseCode.Medical
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
            utilisateurHisto = userDao.GetUserById(contexteRead.UserCreation)
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
            utilisateurHisto = userDao.GetUserById(contexteRead.UserModification)
            'SetUtilisateur(utilisateurHisto, contexteRead.UserModification)
            LblUtilisateurModification.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
        Else
            LblModificationContexte2.Hide()
        End If

        'Initialisation classe Historisation contexte 
        AntecedentHistoCreationDao.InitAntecedentHistorisation(contexteRead, userLog, ContexteHistoACreer)
    End Sub

    'Suppression (annulation) du contexte
    Private Sub RadBtnSupprimer_Click(sender As Object, e As EventArgs) Handles RadBtnSupprimer.Click
        If MsgBox("Attention, confirmez-vous l'annulation du contexte", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            'Annulation contexte
            If contexteDao.AnnulationContexte(contexteUpdate, ContexteHistoACreer, userLog) = True Then
                CodeResultat = EnumResultat.AnnulationOK
                Try
                    Dim form As New RadFNotification()
                    form.Titre = "Notification contexte patient"
                    form.Message = "Contexte patient annulé"
                    form.Show()
                    Me.CodeRetour = True
                    Close()
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
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
                Try
                    Dim form As New RadFNotification()
                    form.Titre = "Notification contexte patient"
                    form.Message = "Le contexte patient a été transformé en antécédent"
                    form.Show()
                    Me.ContexteTransformeEnAntecedent = True
                    Me.CodeRetour = True
                    Close()
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
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
                    contexteUpdate.Id = contexteDao.CreationContexte(contexteUpdate, ContexteHistoACreer, userLog, ConclusionEpisode, Episode)

                    If contexteUpdate.Id <> 0 Then
                        If ConclusionEpisode = True Then
                            Dim episodeDao As New EpisodeDao
                            episodeDao.MajEpisodeConclusionMedicale(Episode.Id)
                        End If
                        CodeResultat = EnumResultat.CreationOK

                        For Each row In RadGridViewChaineEpisode.Rows
                            If row.Cells("selected").Value = True Then
                                chaineEpisodeDao.Create(New ChaineEpisode With {
                                .Id = 0,
                                .ChaineId = row.Cells("id").Value,
                                .AntecedentId = contexteUpdate.Id
                            })
                            End If
                        Next

                        Try
                            Dim form As New RadFNotification()
                            form.Titre = "Notification contexte patient"
                            form.Message = "Contexte patient créé"
                            form.Show()
                            Me.CodeRetour = True
                            Me.NewContext = contexteUpdate
                            Close()
                        Catch ex As Exception
                            MessageBox.Show(ex.Message)
                        End Try

                    Else
                        Me.CodeRetour = False
                    End If
                End If
            Case EnumTraitement.Modification
                If ValidationContexte() = True Then
                    If contexteDao.ModificationContexte(contexteUpdate, ContexteHistoACreer, userLog, contexteRead.Description, contexteRead.DrcId) = True Then
                        CodeResultat = EnumResultat.ModificationOK

                        For Each row In RadGridViewChaineEpisode.Rows
                            Dim chaineId As Long = row.Cells("id").Value
                            Dim isRelationExist As Boolean = relationChaineEpisodes.Any(Function(myObject) myObject.ChaineId = chaineId)

                            If row.Cells("selected").Value = True AndAlso isRelationExist = False Then
                                chaineEpisodeDao.Create(New ChaineEpisode With {
                                        .Id = 0,
                                        .ChaineId = chaineId,
                                        .AntecedentId = SelectedContexteId
                                    })
                            ElseIf row.Cells("selected").Value = False AndAlso isRelationExist = True Then
                                chaineEpisodeDao.Delete(New ChaineEpisode With {
                                        .Id = 0,
                                        .ChaineId = chaineId,
                                        .AntecedentId = SelectedContexteId
                                    })
                            End If
                        Next

                        Try
                            Dim form As New RadFNotification()
                            form.Titre = "Notification contexte patient"
                            form.Message = "Contexte patient modifié"
                            form.Show()
                            Me.CodeRetour = True
                            Close()
                        Catch ex As Exception
                            MessageBox.Show(ex.Message)
                        End Try
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
        Try
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
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub


    '=============================================================================================
    '==================================== Gestion de l'affichage des zones d'écran ===============
    '=============================================================================================

    'Initialisation des zones de saisie
    Private Sub InitZone()
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

    Private Sub RadioBtnMedical_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnMedical.CheckedChanged
        contexteUpdate.CategorieContexte = ContexteCourrier.EnumParcoursBaseCode.Medical
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub RadioBtnBioEnvironnemental_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnBioEnvironnemental.CheckedChanged
        contexteUpdate.CategorieContexte = ContexteCourrier.EnumParcoursBaseCode.BioEnvironnemental
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

        If outils.AccesFonctionMedicaleSynthese(SelectedPatient, userLog) = False Then
            RadBtnDrcSelect.Hide()
            RadBtnRecupereDrc.Hide()
            RadBtnSupprimer.Hide()
            RadBtnTransformer.Hide()
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
                Form.UtilisateurConnecte = userLog
                Form.ShowDialog()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
        Me.Enabled = True
    End Sub

    Private Sub RadGridViewChaineEpisode_Click(sender As Object, e As EventArgs) Handles RadGridViewChaineEpisode.Click
        RadValidation.Enabled = True
        Dim isChecked = RadGridViewChaineEpisode.Rows(Me.RadGridViewChaineEpisode.Rows.IndexOf(Me.RadGridViewChaineEpisode.CurrentRow)).Cells("selected").Value
        If (isChecked) Then
            Me.RadGridViewChaineEpisode.CurrentRow.Cells("selected").Value = False
        Else
            Me.RadGridViewChaineEpisode.CurrentRow.Cells("selected").Value = True
        End If
    End Sub

    Private Sub RadChkPublie_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkPublie.ToggleStateChanged
        If RadChkPublie.Checked = True Then
            RadChkTous.Checked = False
            If InitPublie = True Then
                Application.DoEvents()
                RefreshChaineEpisode()
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
            RefreshChaineEpisode()
        Else
            If RadChkPublie.Checked = False Then
                RadChkTous.Checked = True
            End If
        End If
    End Sub
End Class
