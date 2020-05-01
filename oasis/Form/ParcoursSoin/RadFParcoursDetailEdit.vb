Imports System.Configuration
Imports Oasis_WF
Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports Oasis_Common
Public Class RadFParcoursDetailEdit
    Private _SelectedPatient As Patient
    'Private _UtilisateurConnecte As Utilisateur
    Private _SelectedParcoursId As Integer
    Private _SelectedSpecialiteId As Integer
    Private _SelectedRorId As Integer
    Private _CodeRetour As Boolean
    Private _RythmeObligatoire As Boolean
    Private _positionGaucheDroite As Integer

    Public Property SelectedPatient As Patient
        Get
            Return _SelectedPatient
        End Get
        Set(value As Patient)
            _SelectedPatient = value
        End Set
    End Property

    'Public Property UtilisateurConnecte As Utilisateur
    'Get
    'Return _UtilisateurConnecte
    'End Get
    'Set(value As Utilisateur)
 _ 'UtilisateurConnecte = value
    'Set
    'End Property

    Public Property SelectedParcoursId As Integer
        Get
            Return _SelectedParcoursId
        End Get
        Set(value As Integer)
            _SelectedParcoursId = value
        End Set
    End Property

    Public Property SelectedSpecialiteId As Integer
        Get
            Return _SelectedSpecialiteId
        End Get
        Set(value As Integer)
            _SelectedSpecialiteId = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return _CodeRetour
        End Get
        Set(value As Boolean)
            _CodeRetour = value
        End Set
    End Property

    Public Property SelectedRorId As Integer
        Get
            Return _SelectedRorId
        End Get
        Set(value As Integer)
            _SelectedRorId = value
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

    Public Property RythmeObligatoire As Boolean
        Get
            Return _RythmeObligatoire
        End Get
        Set(value As Boolean)
            _RythmeObligatoire = value
        End Set
    End Property

    Enum EnumEditMode
        Creation = 1
        Modification = 2
    End Enum

    Enum EnumModicationMode
        Intervenant = 1
        Intervention = 2
    End Enum

    Dim EditMode As Integer

    Dim UtilisateurHisto As Utilisateur = New Utilisateur()
    Dim ParcoursDao As New ParcoursDao
    Dim ParcoursUpdate As New Parcours
    Dim parcoursRead As New Parcours
    Dim parcoursConsigneDao As New ParcoursConsigneDao

    Dim rordao As New RorDao
    Dim tacheDao As New TacheDao
    Dim ror As Ror
    Dim specialite As Specialite
    Dim masquerIntervenant As Boolean = True

    Dim DureeRendezVous As Integer
    Dim EmetteurFonctionId As Long
    Dim TraiteFonctionId As Long
    Dim DestinataireFonctionId As Long

    Dim RendezVousPlanifie As Boolean = False
    Dim DemandeRendezVous As Boolean = False

    Dim DateRendezVous As Date
    Dim RendezVousPlanifieExiste As Boolean = False

    Private Sub RadFParcoursDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Init()
        ChargementEtatCivil()
        DroitAcces()

        If SelectedParcoursId <> 0 Then
            'Modification
            RadBtnRendezVous.Show()
            ChargementParcours()
            ChargementhistoriqueConsultation()
            If masquerIntervenant = False Then
                LblLabelMasqueIntervenant.Hide()
                ChkMasquerIntervenant.Hide()
            End If
            EditMode = EnumEditMode.Modification
            RbtInterventionProgramme.CheckState = CheckState.Checked
        Else
            'Création
            'Données spécialité
            specialite = Environnement.Table_specialite.GetSpecialiteById(Me.SelectedSpecialiteId)
            If SelectedSpecialiteId <> 0 Then
                If SelectedSpecialiteId = EnumSpecialiteOasis.IDE Then
                    'FixeTailleEcranPourIDE()
                End If
            End If
            ParcoursUpdate.SpecialiteId = Me.SelectedSpecialiteId
            TxtSpecialiteDescription.Text = specialite.Code
            If specialite.Oasis = True Then
                CbxOasisExterne.Text = "Oasis"
            Else
                CbxOasisExterne.Text = "Externe"
            End If

            'Données ROR
            ror = rordao.getRorById(Me.SelectedRorId)
            ParcoursUpdate.RorId = Me.SelectedRorId
            TxtNomIntervenant.Text = ror.Nom
            TxtTypeIntervenant.Text = ror.Type
            TxtNomStructure.Text = ror.StructureNom

            'Initialisation bean parcours
            ParcoursUpdate.PatientId = Me.SelectedPatient.patientId
            ParcoursUpdate.CategorieId = EnumCategoriePPS.Suivi
            Select Case SelectedSpecialiteId
                Case EnumSpecialiteOasis.medecinReferent
                    ParcoursUpdate.SousCategorieId = EnumSousCategoriePPS.medecinReferent
                    ParcoursUpdate.IntervenantOasis = True
                Case EnumSpecialiteOasis.IDE
                    ParcoursUpdate.SousCategorieId = EnumSousCategoriePPS.IDE
                    ParcoursUpdate.IntervenantOasis = True
                Case EnumSpecialiteOasis.sageFemmeOasis
                    ParcoursUpdate.SousCategorieId = EnumSousCategoriePPS.sageFemme
                    ParcoursUpdate.IntervenantOasis = True
                Case Else
                    ParcoursUpdate.SousCategorieId = EnumSousCategoriePPS.specialiste
                    ParcoursUpdate.IntervenantOasis = False
            End Select
            ParcoursUpdate.Commentaire = ""
            TxtCommentaire.Text = ""
            ParcoursUpdate.Base = ""
            CbxBase.Text = ""
            ParcoursUpdate.Rythme = 0
            NumRythme.Value = 0
            ParcoursUpdate.Cacher = False
            ParcoursUpdate.Inactif = False
            ParcoursUpdate.DateCreation = Date.Now()
            ParcoursUpdate.UserCreation = userLog.UtilisateurId

            'Intialisation zones en création
            EditMode = EnumEditMode.Creation
            RadBtnValidation.Enabled = True
            RadBtnRorDetail.Hide()
            RadBtnAnnuler.Hide()
            RadGbxConsultationEnCours.Hide()
            RadBtnHistorique.Hide()

            'Cacher les éléments de création et modification de l'occurrence
            LblLabelDateModification.Hide()
            LblDateModification.Hide()
            LblLabelParModification.Hide()
            LblUtilisateurModification.Hide()
            LblLabelDateCreation.Hide()
            LblDateCreation.Hide()
            LblLabelParCreation.Hide()
            LblUtilisateurCreation.Hide()
        End If

        If PositionGaucheDroite = EnumPosition.Droite Then
            Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - Me.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        Else
            Me.Location = New Point(10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        End If
    End Sub

    Private Sub Init()
        afficheTitleForm(Me, "Parcours de soin")
        Me.Width = 980
        Me.Height = 650
        DureeRendezVous = 15
        Try
            If IsNumeric(ConfigurationManager.AppSettings("dureeRendezVous")) Then
                DureeRendezVous = ConfigurationManager.AppSettings("dureeRendezVous")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

        Me.CodeRetour = False
        RadBtnRendezVous.Hide()
        GbxIntervention.Hide()
        RadBtnValidation.Enabled = False

        'Chargement comboBox rythme intervention
        CbxBase.Items.Clear()
        CbxBase.Items.Add(ParcoursDao.EnumParcoursBaseItem.Quotidien)
        CbxBase.Items.Add(ParcoursDao.EnumParcoursBaseItem.Hebdomadaire)
        CbxBase.Items.Add(ParcoursDao.EnumParcoursBaseItem.ParMois)
        CbxBase.Items.Add(ParcoursDao.EnumParcoursBaseItem.ParAn)
        CbxBase.Items.Add(ParcoursDao.EnumParcoursBaseItem.TousLes2Ans)
        CbxBase.Items.Add(ParcoursDao.EnumParcoursBaseItem.TousLes3Ans)
        CbxBase.Items.Add(ParcoursDao.EnumParcoursBaseItem.TousLes4Ans)
        CbxBase.Items.Add(ParcoursDao.EnumParcoursBaseItem.TousLes5Ans)
        CbxBase.Items.Add("")

        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.TextAndButtonsElement.TextElement.ForeColor = Color.Red
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.BackColor = Color.DarkBlue
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.Font = New Font("Arial", 8.0F, FontStyle.Italic)
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.TextImageRelation = TextImageRelation.TextBeforeImage
        Me.RadDesktopAlert1.Popup.AlertElement.BackColor = Color.MistyRose
        Me.RadDesktopAlert1.Popup.AlertElement.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.BorderColor = Color.DarkBlue
    End Sub

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

    Private Sub ChargementParcours()
        Dim dateCreation, dateModification As Date
        parcoursRead = ParcoursDao.getParcoursById(SelectedParcoursId)
        ParcoursUpdate = ParcoursDao.CloneParcours(parcoursRead)

        'Cacher le bouton d'annulation de l'intervenant pour le Médecin référent et l'IDE sur site
        If parcoursRead.SousCategorieId = EnumSousCategoriePPS.IDE OrElse
            parcoursRead.SousCategorieId = EnumSousCategoriePPS.medecinReferent Then
            RadBtnAnnuler.Hide()
        End If

        'Données spécialité
        specialite = Environnement.Table_specialite.GetSpecialiteById(ParcoursUpdate.SpecialiteId)
        If ParcoursUpdate.SpecialiteId = EnumSpecialiteOasis.IDE Then
            FixeTailleEcranPourIDE()
            ChargementConsigne()
        End If

        TxtSpecialiteDescription.Text = specialite.Code
        If specialite.Oasis = True Then
            CbxOasisExterne.Text = "Oasis"
            RadBtnRORSelect.Hide()
        Else
            CbxOasisExterne.Text = "Externe"
        End If

        'Données ROR
        ror = rordao.getRorById(ParcoursUpdate.RorId)
        TxtNomIntervenant.Text = ror.Nom
        TxtTypeIntervenant.Text = ror.Type
        TxtNomStructure.Text = ror.StructureNom

        'Données parcours
        ChkMasquerIntervenant.Checked = ParcoursUpdate.Cacher

        Select Case ParcoursUpdate.Base
            Case ParcoursDao.EnumParcoursBaseCode.Quotidien
                CbxBase.Text = ParcoursDao.EnumParcoursBaseItem.Quotidien
            Case ParcoursDao.EnumParcoursBaseCode.Hebdomadaire
                CbxBase.Text = ParcoursDao.EnumParcoursBaseItem.Hebdomadaire
            Case ParcoursDao.EnumParcoursBaseCode.ParMois
                CbxBase.Text = ParcoursDao.EnumParcoursBaseItem.ParMois
            Case ParcoursDao.EnumParcoursBaseCode.ParAn
                CbxBase.Text = ParcoursDao.EnumParcoursBaseItem.ParAn
            Case ParcoursDao.EnumParcoursBaseCode.TousLes2Ans
                CbxBase.Text = ParcoursDao.EnumParcoursBaseItem.TousLes2Ans
            Case ParcoursDao.EnumParcoursBaseCode.TousLes3Ans
                CbxBase.Text = ParcoursDao.EnumParcoursBaseItem.TousLes3Ans
            Case ParcoursDao.EnumParcoursBaseCode.TousLes4Ans
                CbxBase.Text = ParcoursDao.EnumParcoursBaseItem.TousLes4Ans
            Case ParcoursDao.EnumParcoursBaseCode.TousLes5Ans
                CbxBase.Text = ParcoursDao.EnumParcoursBaseItem.TousLes5Ans
            Case Else
                CbxBase.Text = ""
        End Select

        NumRythme.Value = ParcoursUpdate.Rythme
        TxtCommentaire.Text = ParcoursUpdate.Commentaire
        If ParcoursUpdate.Rythme <> 0 And ParcoursUpdate.Base <> "" Then
            masquerIntervenant = False
        End If

        'Affichage information création et modification
        If ParcoursUpdate.DateCreation <> Nothing Then
            dateCreation = ParcoursUpdate.DateCreation
            LblDateCreation.Text = dateCreation.ToString("dd.MM.yyyy")
        Else
            LblDateCreation.Text = ""
            LblLabelDateCreation.Hide()
            LblLabelParCreation.Hide()
        End If

        LblUtilisateurCreation.Text = ""

        If ParcoursUpdate.UserCreation <> 0 Then
            Dim userDao As New UserDao
            UtilisateurHisto = userDao.getUserById(ParcoursUpdate.UserCreation)
            'SetUtilisateur(UtilisateurHisto, ParcoursUpdate.UserCreation)
            LblUtilisateurCreation.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
        End If

        If ParcoursUpdate.DateModification <> Nothing Then
            dateModification = ParcoursUpdate.DateModification
            LblDateModification.Text = dateModification.ToString("dd.MM.yyyy")
        Else
            LblDateModification.Text = ""
            LblLabelDateModification.Hide()
            LblLabelParModification.Hide()
        End If

        LblUtilisateurModification.Text = ""
        If ParcoursUpdate.UserModification <> 0 Then
            Dim userDao As New UserDao
            UtilisateurHisto = userDao.getUserById(ParcoursUpdate.UserModification)
            'SetUtilisateur(UtilisateurHisto, ParcoursUpdate.UserModification)
            LblUtilisateurModification.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
        End If
    End Sub

    'Chargement de l'historique du dernier rendez-vous et du rendez-vous à venir
    Private Sub ChargementhistoriqueConsultation()
        RadBtnModifRDV.Hide()
        RadBtnClotureRDV.Hide()
        'Recherche dernier rendez-vous
        Dim dateLast, dateNext As Date
        Dim tache As Tache

        tache = tacheDao.GetDernierRenezVousByPatientId(SelectedPatient.patientId, SelectedParcoursId)
        dateLast = tache.DateRendezVous
        If dateLast <> Nothing Then
            LblDateDernierRendezVous.Text = outils.FormatageDateAffichage(dateLast, True)
        Else
            LblDateDernierRendezVous.Text = "Pas de rendez-vous précédent"
        End If

        'Recherche prochain rendez-vous
        tache = tacheDao.GetProchainRendezVousByPatientIdEtParcours(SelectedPatient.patientId, SelectedParcoursId)
        dateNext = tache.DateRendezVous
        If dateNext <> Nothing Then
            LblDateProchainRendezVous.Text = dateNext.ToString("dd.MM.yyyy")
            LblDateNextType.Text = "(Rendez-vous planifiée)"
            RendezVousPlanifieExiste = True
            DateRendezVous = dateNext
            masquerIntervenant = False
            RadBtnRendezVous.Enabled = False
            RadBtnModifRDV.Show()
            If tache.Nature = TacheDao.EnumNatureTacheCode.RDV_SPECIALISTE OrElse
                tache.Nature = TacheDao.EnumNatureTacheCode.RDV Then
                If tache.DateRendezVous.Date <= Date.Now.Date() Then
                    RadBtnClotureRDV.Show()
                End If
            End If
            RendezVousPlanifie = True
        Else
            'Recherche si existe demande de rendez-vous
            tache = tacheDao.GetProchaineDemandeRendezVousByPatientId(SelectedPatient.patientId, SelectedParcoursId)
            dateNext = tache.DateRendezVous
            If dateNext <> Nothing Then
                Select Case tache.TypedemandeRendezVous
                    Case TacheDao.typeDemandeRendezVous.ANNEE.ToString
                        LblDateProchainRendezVous.Text = dateNext.ToString("yyyy")
                    Case TacheDao.typeDemandeRendezVous.ANNEEMOIS.ToString
                        LblDateProchainRendezVous.Text = dateNext.ToString("MM.yyyy")
                    Case Else
                        LblDateProchainRendezVous.Text = outils.FormatageDateAffichage(dateNext)
                End Select
                Select Case tache.Etat
                    Case TacheDao.EtatTache.EN_COURS.ToString
                        LblDateNextType.Text = "(Rendez-vous prévisionnel, demande en cours de traitement)"
                    Case TacheDao.EtatTache.EN_ATTENTE.ToString
                        LblDateNextType.Text = "(Rendez-vous prévisionnel, demande en attente de traitement)"
                        RadBtnModifRDV.Show()
                        DemandeRendezVous = True
                    Case Else
                        LblDateNextType.Text = "(Rendez-vous prévisionnel, demande en : " & tache.Etat & ")"
                End Select
                masquerIntervenant = False
                RadBtnRendezVous.Enabled = False
            Else
                If ParcoursUpdate.Rythme <> 0 And ParcoursUpdate.Base <> "" Then
                    If dateLast <> Nothing Then
                        LblDateNextType.Text = "(Rendez-vous prévisionnel, calculé selon le rythme saisi et le dernier rendez-vous passé)"
                        dateNext = CalculProchainRendezVous(dateLast, ParcoursUpdate.Rythme, ParcoursUpdate.Base)
                        LblDateProchainRendezVous.Text = outils.FormatageDateAffichage(dateNext)
                    Else
                        If ParcoursUpdate.DateCreation <> Nothing Then
                            LblDateNextType.Text = "(Rendez-vous prévisionnel, calculé selon le rythme saisi et la date de création de l'intervenant dans le parcours de soin du patient)"
                            dateNext = CalculProchainRendezVous(ParcoursUpdate.DateCreation, ParcoursUpdate.Rythme, ParcoursUpdate.Base)
                            LblDateProchainRendezVous.Text = outils.FormatageDateAffichage(dateNext)
                        Else
                            LblDateNextType.Text = ""
                            LblDateProchainRendezVous.Text = "(Rendez-vous à venir non calculable)"
                        End If
                    End If
                Else
                    LblDateNextType.Text = ""
                    LblDateProchainRendezVous.Text = "(pas de rendez-vous à venir pour cet intervenant)"
                End If
            End If
        End If


    End Sub

    Private Sub ChargementConsigne()
        Dim ParcoursConsigneDataTable As DataTable
        Dim DateDebut, DateFin As Date
        Dim MaxDate As New Date(9998, 12, 31, 0, 0, 0)

        ParcoursConsigneDataTable = parcoursConsigneDao.getAllConsignebyParcoursId(SelectedParcoursId)

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = ParcoursConsigneDataTable.Rows.Count - 1

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadParcoursConsigneDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadParcoursConsigneDataGridView.Rows(iGrid).Cells("consigneId").Value = ParcoursConsigneDataTable.Rows(i)("oa_parcours_consigne_id")
            RadParcoursConsigneDataGridView.Rows(iGrid).Cells("drcId").Value = ParcoursConsigneDataTable.Rows(i)("oa_parcours_consigne_drc_id")
            RadParcoursConsigneDataGridView.Rows(iGrid).Cells("drcDescription").Value = Coalesce(ParcoursConsigneDataTable.Rows(i)("oa_drc_libelle"), "")
            RadParcoursConsigneDataGridView.Rows(iGrid).Cells("commentaire").Value = Coalesce(ParcoursConsigneDataTable.Rows(i)("oa_parcours_consigne_commentaire"), "")
            RadParcoursConsigneDataGridView.Rows(iGrid).Cells("activite_type_episode").Value = Coalesce(ParcoursConsigneDataTable.Rows(i)("activite_type_episode"), "")

            DateDebut = Coalesce(ParcoursConsigneDataTable.Rows(i)("oa_parcours_consigne_date_debut"), Nothing)
            RadParcoursConsigneDataGridView.Rows(iGrid).Cells("dateDebut").Value = ""
            If DateDebut <> Nothing Then
                RadParcoursConsigneDataGridView.Rows(iGrid).Cells("dateDebut").Value = DateDebut.ToString("dd.MM.yyyy")
            End If

            DateFin = Coalesce(ParcoursConsigneDataTable.Rows(i)("oa_parcours_consigne_date_fin"), Nothing)
            RadParcoursConsigneDataGridView.Rows(iGrid).Cells("dateFin").Value = ""
            If DateFin.Date <> MaxDate.Date Then
                RadParcoursConsigneDataGridView.Rows(iGrid).Cells("dateFin").Value = DateFin.ToString("dd.MM.yyyy")
            End If

            If Coalesce(ParcoursConsigneDataTable.Rows(i)("activite_type_episode"), "") = EpisodeDao.EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE Then
                RadParcoursConsigneDataGridView.Rows(iGrid).Cells("drcDescription").Style.ForeColor = Color.Red
                RadParcoursConsigneDataGridView.Rows(iGrid).Cells("commentaire").Style.ForeColor = Color.Red
                RadParcoursConsigneDataGridView.Rows(iGrid).Cells("dateDebut").Style.ForeColor = Color.Red
                RadParcoursConsigneDataGridView.Rows(iGrid).Cells("dateFin").Style.ForeColor = Color.Red
            End If

        Next

        'Positionnement du grid sur la première occurrence
        If RadParcoursConsigneDataGridView.Rows.Count > 0 Then
            Me.RadParcoursConsigneDataGridView.CurrentRow = RadParcoursConsigneDataGridView.ChildRows(0)
        End If
    End Sub

    Private Sub RadBtnROR_Click(sender As Object, e As EventArgs) Handles RadBtnRORSelect.Click
        Using vRadFRorListe As New RadFRorListe
            vRadFRorListe.Selecteur = True
            vRadFRorListe.SpecialiteId = ParcoursUpdate.SpecialiteId
            vRadFRorListe.TypeRor = RorDao.EnumTypeRor.Intervenant
            vRadFRorListe.ShowDialog() 'Modal
            If vRadFRorListe.CodeRetour = True Then
                If vRadFRorListe.SelectedRorId <> 0 Then
                    Me.SelectedRorId = vRadFRorListe.SelectedRorId
                    ror = rordao.getRorById(vRadFRorListe.SelectedRorId)
                    ParcoursUpdate.RorId = Me.SelectedRorId
                    GestionAffichageBoutonValidation()
                    TxtNomIntervenant.Text = ror.Nom
                    TxtTypeIntervenant.Text = ror.Type
                    'Structure
                    If ror.StructureId <> 0 Then
                        Dim structureIntervenant As Ror
                        structureIntervenant = rordao.getRorById(ror.StructureId)
                        TxtNomStructure.Text = structureIntervenant.Nom
                    Else
                        TxtNomStructure.Text = ror.StructureNom
                    End If
                End If
            End If
        End Using
    End Sub

    '====================================================================================
    '===================== Contrôle des zones modifiées =================================
    '====================================================================================

    Private Sub NumRythme_ValueChanged(sender As Object, e As EventArgs) Handles NumRythme.ValueChanged
        ParcoursUpdate.Rythme = NumRythme.Value
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub CbxBase_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxBase.SelectedIndexChanged
        If CbxBase.Text.Trim() <> "" Then
            Select Case CbxBase.Text
                Case ParcoursDao.EnumParcoursBaseItem.Quotidien
                    ParcoursUpdate.Base = ParcoursDao.EnumParcoursBaseCode.Quotidien
                Case ParcoursDao.EnumParcoursBaseItem.Hebdomadaire
                    ParcoursUpdate.Base = ParcoursDao.EnumParcoursBaseCode.Hebdomadaire
                Case ParcoursDao.EnumParcoursBaseItem.ParMois
                    ParcoursUpdate.Base = ParcoursDao.EnumParcoursBaseCode.ParMois
                Case ParcoursDao.EnumParcoursBaseItem.ParAn
                    ParcoursUpdate.Base = ParcoursDao.EnumParcoursBaseCode.ParAn
                Case ParcoursDao.EnumParcoursBaseItem.TousLes2Ans
                    ParcoursUpdate.Base = ParcoursDao.EnumParcoursBaseCode.TousLes2Ans
                Case ParcoursDao.EnumParcoursBaseItem.TousLes3Ans
                    ParcoursUpdate.Base = ParcoursDao.EnumParcoursBaseCode.TousLes3Ans
                Case ParcoursDao.EnumParcoursBaseItem.TousLes4Ans
                    ParcoursUpdate.Base = ParcoursDao.EnumParcoursBaseCode.TousLes4Ans
                Case ParcoursDao.EnumParcoursBaseItem.TousLes5Ans
                    ParcoursUpdate.Base = ParcoursDao.EnumParcoursBaseCode.TousLes5Ans
                Case Else
                    ParcoursUpdate.Base = ""
            End Select
            'ParcoursUpdate.Base = CbxBase.Text

            NumRythme.Hide()
            LblSlash.Hide()
            If CbxBase.Text = ParcoursDao.EnumParcoursBaseItem.Hebdomadaire _
                Or CbxBase.Text = ParcoursDao.EnumParcoursBaseItem.ParMois _
                Or CbxBase.Text = ParcoursDao.EnumParcoursBaseItem.ParAn Then
                NumRythme.Value = ParcoursUpdate.Rythme
                NumRythme.Show()
                LblSlash.Show()
            Else
                NumRythme.Value = 1
            End If
        Else
            ParcoursUpdate.Base = ""
            NumRythme.Value = 0
            NumRythme.Show()
        End If
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtCommentaire_TextChanged(sender As Object, e As EventArgs) Handles TxtCommentaire.TextChanged
        ParcoursUpdate.Commentaire = TxtCommentaire.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub ChkMasquerIntervenant_CheckedChanged(sender As Object, e As EventArgs) Handles ChkMasquerIntervenant.CheckedChanged
        ParcoursUpdate.Cacher = ChkMasquerIntervenant.CheckState
        GestionAffichageBoutonValidation()
    End Sub

    '====================================================================================
    '===================== Contrôle des zones de demande de rendez-vous =================
    '====================================================================================

    Private Sub NumDateRV_ValueChanged(sender As Object, e As EventArgs) Handles NumDateRV.ValueChanged

    End Sub

    Private Sub NumheureRV_ValueChanged(sender As Object, e As EventArgs) Handles NumheureRV.ValueChanged

    End Sub

    Private Sub NumMois_ValueChanged(sender As Object, e As EventArgs) Handles NumMois.ValueChanged

    End Sub

    Private Sub NumAn_ValueChanged(sender As Object, e As EventArgs) Handles NumAn.ValueChanged

    End Sub

    '====================================================================================
    '===================== Gestion de l'écran ===========================================
    '====================================================================================

    'Gérer un rendez-vous
    Private Sub RadBtnRV_Click(sender As Object, e As EventArgs) Handles RadBtnRendezVous.Click
        GbxIntervention.Show()
        GbxIntervenant.Enabled = False
        RadBtnValidation.Enabled = True
        NumDateRV.Value = Date.Now()
        NumheureRV.Value = 12
        RadioBtn0.Checked = True
        NumMois.Value = Date.Now.Month
        NumAn.Value = Date.Now.Year
        RadBtnRendezVous.Enabled = False
    End Sub

    Private Sub RbtInterventionProgramme_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RbtInterventionProgramme.ToggleStateChanged
        If RbtInterventionProgramme.CheckState = CheckState.Checked Then
            LblLabelDateRV.Show()
            NumDateRV.Show()
            LblLabelHeureRV.Show()
            NumheureRV.Show()
            RadioBtn0.Show()
            RadioBtn15.Show()
            RadioBtn30.Show()
            RadioBtn45.Show()

            RadChkDRVAnneeSeulement.Hide()
            RadChkDRVAnneeSeulement.Checked = False
            LblPeriode.Hide()
            NumMois.Hide()
            NumAn.Hide()
        End If
    End Sub

    Private Sub RbtInterventionPrevisionnel_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RbtInterventionPrevisionnel.ToggleStateChanged
        If RbtInterventionPrevisionnel.CheckState = CheckState.Checked Then
            LblLabelDateRV.Hide()
            NumDateRV.Hide()
            LblLabelHeureRV.Hide()
            NumheureRV.Hide()
            RadioBtn0.Hide()
            RadioBtn15.Hide()
            RadioBtn30.Hide()
            RadioBtn45.Hide()

            RadChkDRVAnneeSeulement.Show()
            LblPeriode.Show()
            NumMois.Show()
            NumAn.Show()
        End If
    End Sub

    Private Sub RadChkDRVAnneeSeulement_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkDRVAnneeSeulement.ToggleStateChanged
        If RadChkDRVAnneeSeulement.Checked = True Then
            NumMois.Hide()
        Else
            NumMois.Show()
        End If
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        'Après validation on revient à l'état initial pour permettre la prise de rendez-vous
        Select Case EditMode
            Case EnumEditMode.Creation
                'Création intervenant
                If ValidationDonneeSaisie() = True Then
                    ParcoursUpdate.Id = ParcoursDao.CreateIntervenantParcours(ParcoursUpdate)
                    If ParcoursUpdate.Id <> 0 Then
                        Me.CodeRetour = True
                        Dim form As New RadFNotification()
                        form.Message = "Intervenant du parcours de soin créé"
                        form.Show()
                        'Création automatique de demande de rendez-vous
                        If ParcoursUpdate.Rythme <> 0 AndAlso ParcoursUpdate.Base.Trim() <> "" Then
                            'Appel création demande de rendez-vous
                            Dim tacheDao As New TacheDao
                            tacheDao.CreationAutomatiqueDeDemandeRendezVous(SelectedPatient, ParcoursUpdate, Date.Now())
                        End If
                        'Fermeture de l'écran en création après validation
                        Close()
                    End If
                    RadBtnRORSelect.Hide()
                    RadBtnRendezVous.Show()
                    EditMode = EnumEditMode.Modification
                    'ChargementParcours()
                    RbtInterventionProgramme.CheckState = CheckState.Checked
                End If
            Case EnumEditMode.Modification
                If GbxIntervenant.Enabled = True Then
                    'Appel modification
                    If ValidationDonneeSaisie() = True Then
                        If ParcoursDao.ModificationIntervenantParcours(ParcoursUpdate) = True Then
                            Dim form As New RadFNotification()
                            form.Message = "Intervenant du parcours de soin modifié"
                            form.Show()
                            'Création automatique de demande de rendez-vous
                            If parcoursRead.Rythme = 0 AndAlso parcoursRead.Base.Trim() = "" Then
                                If ParcoursUpdate.Rythme <> 0 AndAlso ParcoursUpdate.Base.Trim() <> "" Then
                                    'Appel création demande de rendez-vous
                                    If tacheDao.CreationAutomatiqueDeDemandeRendezVous(SelectedPatient, ParcoursUpdate, Date.Now()) = True Then
                                        ChargementhistoriqueConsultation()
                                    End If
                                End If
                            End If
                            Me.CodeRetour = True
                            If ParcoursUpdate.Cacher = True Then
                                Close()
                            End If
                        End If
                        ChargementParcours()
                    End If
                    GestionAffichageBoutonValidation()
                Else
                    'Contrôler l'existence d'un rendez-vous en cours
                    Dim RendezVousPossible As Boolean = True
                    Dim tache As Tache
                    tache = tacheDao.GetProchainRendezVousByPatientIdEtParcours(SelectedPatient.patientId, SelectedParcoursId)
                    If tache.DateRendezVous <> Nothing Then
                        RendezVousPossible = False
                        MessageBox.Show("Un rendez-vous est déjà planifié pour cet intervenant, opération impossible", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Else
                        tache = tacheDao.GetProchaineDemandeRendezVousByPatientId(SelectedPatient.patientId, SelectedParcoursId)
                        If tache.DateRendezVous <> Nothing Then
                            RendezVousPossible = False
                            MessageBox.Show("Une demande de rendez-vous est déjà planifiée pour cet intervenant, opération impossible", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    End If
                    If RendezVousPossible = True Then
                        GestionRendezVous()
                    End If
                End If
        End Select
    End Sub

    'Annuler un intervenant
    Private Sub RadBtnAnnuler_Click(sender As Object, e As EventArgs) Handles RadBtnAnnuler.Click
        Dim tache As Tache
        Dim AnnulationIntervevant As Boolean = True
        If MsgBox("Confirmation de l'annulation de l'intervenant du parcours de soin de ce patient", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            'Contrôler l'existence d'un rendez-vous en cours
            tache = tacheDao.GetProchainRendezVousByPatientIdEtParcours(SelectedPatient.patientId, SelectedParcoursId)
            If tache.DateRendezVous <> Nothing Then
                'Si un rendez-vous existe et que la tache est déjà attribuée > annulation impossible
                If tache.Etat = TacheDao.EtatTache.EN_COURS.ToString Then
                    MessageBox.Show("Rendez-vous déjà attribué, annulation impossible", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                    AnnulationIntervevant = False
                Else
                    If MsgBox("Un rendez-vous est planifié pour cet intervenant, confirmation de l'annulation", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
                        Try
                            If tacheDao.AnnulationTache(tache) = True Then
                                Dim form As New RadFNotification()
                                form.Message = "Rendez-vous annulé"
                                form.Show()
                                'MessageBox.Show("Rendez-vous annulé")
                            Else
                                MessageBox.Show("Problème de suppression du prochain rendez-vous, annulation impossible")
                                AnnulationIntervevant = False
                            End If
                        Catch ex As Exception
                            MessageBox.Show(ex.ToString)
                        End Try
                    Else
                        AnnulationIntervevant = False
                    End If
                End If
            Else
                tache = tacheDao.GetProchaineDemandeRendezVousByPatientId(SelectedPatient.patientId, SelectedParcoursId)
                If tache.DateRendezVous <> Nothing Then
                    'si la tache est attribuée annulation impossible
                    If tache.Etat = TacheDao.EtatTache.EN_COURS.ToString Then
                        MessageBox.Show("Demande de rendez-vous déjà attribuée, annulation impossible", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                        AnnulationIntervevant = False
                    Else
                        If MsgBox("Une demande de rendez-vous est planifiée pour cet intervenant, confirmation de l'annulation ", MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation, "Confirmation") = MsgBoxResult.Yes Then
                            'Suppression demande de rendez-vous
                            If tacheDao.AnnulationTache(tache) = True Then
                                Dim form As New RadFNotification()
                                form.Message = "Demande de rendez-vous annulée"
                                form.Show()
                            Else
                                MessageBox.Show("Problème de suppression de la demande de rendez-vous, annulation impossible")
                                AnnulationIntervevant = False
                            End If
                        Else
                            AnnulationIntervevant = False
                        End If
                    End If
                End If
            End If

            If AnnulationIntervevant = True Then
                'Annulation de l'intervenant (inactif = True)
                If ParcoursDao.AnnulationIntervenantParcours(ParcoursUpdate) = True Then
                    Me.CodeRetour = True
                    Dim form As New RadFNotification()
                    form.Message = "Intervenant annulé pour le parcours de soin du patient"
                    form.Show()
                    'MessageBox.Show("Intervenant annulé pour le parcours de soin du patient")
                    Close()
                End If
            End If
        End If
    End Sub

    Private Function ValidationDonneeSaisie() As Boolean
        'Contrôle des données saisie
        Dim Valide As Boolean = True
        Dim MessageErreur1 As String = ""
        Dim MessageErreur2 As String = ""
        Dim MessageErreur3 As String = ""
        Dim MessageErreur4 As String = ""
        Dim MessageErreur As String = ""

        If ParcoursUpdate.Base.Trim() <> "" Then
            If ParcoursUpdate.Rythme = 0 Then
                Valide = False
                MessageErreur1 = "La saisie du rythme est obligatoire si la base est renseignée"
            End If
        End If

        If ParcoursUpdate.Rythme <> 0 Then
            If ParcoursUpdate.Base.Trim() = "" Then
                Valide = False
                MessageErreur2 = "La saisie de la base est obligatoire si le rythme est renseigné"
            End If
        End If

        If RythmeObligatoire = True Then
            If ParcoursUpdate.Base.Trim() = "" Then
                Valide = False
                MessageErreur3 = "La saisie de la base est obligatoire"
            End If
            If ParcoursUpdate.Rythme = 0 Then
                Valide = False
                MessageErreur4 = "La saisie du rythme est obligatoire"
            End If
        End If

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

            MessageErreur = MessageErreur & vbCrLf & "/!\ données incorrectes"
            MessageBox.Show(MessageErreur, "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End If

        Return Valide
    End Function

    '========================================================================================================================
    '============= Gestion rendez-vous
    '========================================================================================================================

    Private Sub GestionRendezVous()
        CodeRetour = False

        If RbtInterventionProgramme.IsChecked = True Then
            If NumDateRV.Value.Date < Date.Now().Date Then
                Dim message As String = "Attention, La date de rendez-vous à programmer (" & NumDateRV.Value.ToString("dd.MM.yyyy") & "), est antérieure à la date du jour (" & Date.Now().ToString("dd.MM.yyyy") & "), confirmation de la date du rendez-vous"
                If MsgBox(message, MsgBoxStyle.YesNo, "") = MsgBoxResult.No Then
                    Exit Sub
                End If
            End If

            Dim minutesRV As Integer = CalculMinutes()
            Dim dateRendezVous As New DateTime(NumDateRV.Value.Year, NumDateRV.Value.Month, NumDateRV.Value.Day, NumheureRV.Value, minutesRV, 0)
            If NumDateRV.Value.Date < Date.Now().Date Then
                'Clôture du rendez-vous
                If CreationRendezVous(dateRendezVous, TacheDao.EtatTache.TERMINEE.ToString) = True Then
                    Dim tache As Tache = tacheDao.GetProchainRendezVousByPatientIdEtParcours(SelectedPatient.patientId, SelectedParcoursId)
                    MessageBox.Show("Rendez-vous programmé et clôturé pour le " & NumDateRV.Value.ToString("dd.MM.yyyy"), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    'La clôture du rendez-vous génère automatiquement une demande de rendez-vous
                    tacheDao.CreationAutomatiqueDeDemandeRendezVous(SelectedPatient, ParcoursUpdate, Date.Now())
                    'Si l'intervenant est masqué, il faut l'afficher par défaut
                    If ParcoursUpdate.Cacher = True Then
                        ParcoursUpdate.Cacher = False
                        ParcoursDao.ModificationIntervenantParcours(ParcoursUpdate)
                    End If
                    Me.CodeRetour = True
                    Close()
                End If
            Else
                If CreationRendezVous(dateRendezVous, TacheDao.EtatTache.EN_ATTENTE.ToString) = True Then
                    Dim tache As Tache = tacheDao.GetProchainRendezVousByPatientIdEtParcours(SelectedPatient.patientId, SelectedParcoursId)
                    MessageBox.Show("Rendez-vous programmé pour le " & NumDateRV.Value.ToString("dd.MM.yyyy"), "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                    'Si l'intervenant est masqué, il faut l'afficher par défaut
                    If ParcoursUpdate.Cacher = True Then
                        ParcoursUpdate.Cacher = False
                        ParcoursDao.ModificationIntervenantParcours(ParcoursUpdate)
                    End If
                    Me.CodeRetour = True
                    Close()
                End If
            End If
        Else
            If RbtInterventionPrevisionnel.IsChecked = True Then
                If NumAn.Value < Date.Now().Year Then
                    MessageBox.Show("Erreur : l'année de la demande de rendez-vous à créer : " & NumAn.Value.ToString & " est inférieure à l'année en cours", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Else
                    If RadChkDRVAnneeSeulement.Checked = True Then
                        'Création demande de rendez-vous pour une année donnée (AAAA)
                        Dim dateRendezVous As New DateTime(NumAn.Value, 1, 1, 0, 0, 0)
                        If CreationDemandeRendezVous(dateRendezVous, TacheDao.typeDemandeRendezVous.ANNEE.ToString) = True Then
                            MessageBox.Show("demande de rendez-vous créée pour " & NumAn.Value.ToString, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                            Me.CodeRetour = True
                            Close()
                        End If
                    Else
                        If NumAn.Value = Date.Now().Year And NumMois.Value < Date.Now().Month Then
                            MessageBox.Show("Erreur : la période demandée (" & NumMois.Value.ToString & "/" & NumAn.Value.ToString & ") est inférieure à la période en cours", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        Else
                            'Création demande de rendez-vous pour une période donnée (MM/AAAA)
                            Dim dateRendezVous As New DateTime(NumAn.Value, NumMois.Value, 1, 0, 0, 0)
                            If CreationDemandeRendezVous(dateRendezVous, TacheDao.typeDemandeRendezVous.ANNEEMOIS.ToString) = True Then
                                MessageBox.Show("Demande de rendez-vous créée pour " & NumMois.Value.ToString & "/" & NumAn.Value.ToString, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
                                Me.CodeRetour = True
                                Close()
                            End If
                        End If
                    End If
                End If
            End If
        End If
    End Sub

    Private Function CalculMinutes() As Integer
        Dim minutes As Integer = 0
        If RadioBtn0.Checked = True Then
            minutes = 0
        Else
            If RadioBtn15.Checked = True Then
                minutes = 15
            Else
                If RadioBtn30.Checked = True Then
                    minutes = 30
                Else
                    minutes = 45
                End If
            End If
        End If
        Return minutes
    End Function

    Private Function CreationRendezVous(dateRendezVous As DateTime, etatRendezVous As String) As Boolean
        Dim CodeRetour As Boolean = False
        Dim tache As New Tache
        Dim tacheDao As New TacheDao

        'SetEmetteurId()
        Dim tacheEmetteurEtDestinataire As TacheEmetteurEtDestinataire
        tacheEmetteurEtDestinataire = tacheDao.SetTacheEmetteurEtDestinatiareBySpecialiteEtSousCategorie(ParcoursUpdate.SpecialiteId, ParcoursUpdate.SousCategorieId)

        tache.ParentId = 0
        tache.EmetteurUserId = userLog.UtilisateurId
        tache.EmetteurFonctionId = tacheEmetteurEtDestinataire.EmetteurFonctionId
        tache.UniteSanitaireId = SelectedPatient.PatientUniteSanitaireId
        tache.SiteId = SelectedPatient.PatientSiteId
        tache.PatientId = SelectedPatient.patientId
        tache.ParcoursId = ParcoursUpdate.Id
        tache.EpisodeId = 0
        tache.SousEpisodeId = 0
        tache.TraiteUserId = 0
        tache.TraiteFonctionId = tacheEmetteurEtDestinataire.TraiteFonctionId
        tache.DestinataireFonctionId = tacheEmetteurEtDestinataire.DestinataireFonctionId
        tache.Priorite = TacheDao.Priorite.BASSE
        tache.OrdreAffichage = 30
        tache.Categorie = TacheDao.CategorieTache.SOIN.ToString
        'Si le destinataire du rendez-vous n'est pas Oasis, on déclare un rendez-vous de type Spécialiste
        If ParcoursUpdate.SousCategorieId = EnumSousCategoriePPS.IDE Or
           ParcoursUpdate.SousCategorieId = EnumSousCategoriePPS.medecinReferent Or
           ParcoursUpdate.SousCategorieId = EnumSousCategoriePPS.sageFemme Then
            tache.Type = TacheDao.TypeTache.RDV.ToString
            tache.Nature = TacheDao.NatureTache.RDV.ToString
        Else
            tache.Type = TacheDao.TypeTache.RDV_SPECIALISTE.ToString()
            tache.Nature = TacheDao.NatureTache.RDV_SPECIALISTE.ToString
        End If
        tache.Duree = DureeRendezVous
        tache.EmetteurCommentaire = TxtRDVCommentaire.Text
        tache.HorodatageCreation = Date.Now()
        tache.Etat = etatRendezVous
        If etatRendezVous = TacheDao.EtatTache.TERMINEE.ToString Then
            tache.Cloture = True
        End If
        tache.TypedemandeRendezVous = ""
        tache.DateRendezVous = dateRendezVous

        If tacheDao.CreateTache(tache) = True Then
            CodeRetour = True
        End If

        Return CodeRetour
    End Function

    Private Function CreationDemandeRendezVous(dateRendezVous As DateTime, typedemandeRendezVous As String) As Boolean
        Dim CodeRetour As Boolean = False
        Dim tache As New Tache
        Dim tacheDao As New TacheDao

        'SetEmetteurId()
        Dim tacheEmetteurEtDestinataire As TacheEmetteurEtDestinataire
        tacheEmetteurEtDestinataire = tacheDao.SetTacheEmetteurEtDestinatiareBySpecialiteEtSousCategorie(ParcoursUpdate.SpecialiteId, ParcoursUpdate.SousCategorieId)

        tache.ParentId = 0
        tache.EmetteurUserId = userLog.UtilisateurId
        tache.EmetteurFonctionId = tacheEmetteurEtDestinataire.EmetteurFonctionId
        tache.UniteSanitaireId = SelectedPatient.PatientUniteSanitaireId
        tache.SiteId = SelectedPatient.PatientSiteId
        tache.PatientId = SelectedPatient.patientId
        tache.ParcoursId = ParcoursUpdate.Id
        tache.EpisodeId = 0
        tache.SousEpisodeId = 0
        tache.TraiteUserId = 0
        tache.TraiteFonctionId = tacheEmetteurEtDestinataire.TraiteFonctionId
        tache.DestinataireFonctionId = tacheEmetteurEtDestinataire.DestinataireFonctionId
        tache.Priorite = TacheDao.Priorite.BASSE
        tache.OrdreAffichage = 20
        tache.Categorie = TacheDao.CategorieTache.SOIN.ToString
        tache.Type = TacheDao.TypeTache.RDV_DEMANDE.ToString
        tache.Nature = TacheDao.NatureTache.RDV_DEMANDE.ToString
        tache.Duree = DureeRendezVous
        tache.EmetteurCommentaire = TxtRDVCommentaire.Text
        tache.HorodatageCreation = Date.Now()
        tache.Etat = TacheDao.EtatTache.EN_ATTENTE.ToString
        tache.TypedemandeRendezVous = typedemandeRendezVous
        tache.DateRendezVous = dateRendezVous

        If tacheDao.CreateTache(tache) = True Then
            CodeRetour = True
        End If

        Return CodeRetour
    End Function


    'Modifier un RDV ou une Demande de RDV
    Private Sub RadBtnModifRDV_Click(sender As Object, e As EventArgs) Handles RadBtnModifRDV.Click
        Dim tache As Tache

        If RendezVousPlanifie = True Then
            tache = tacheDao.GetProchainRendezVousByPatientIdEtParcours(SelectedPatient.patientId, SelectedParcoursId)
            If tache.Id <> 0 AndAlso (tache.Nature = TacheDao.EnumNatureTacheCode.RDV Or tache.Nature = TacheDao.EnumNatureTacheCode.RDV_SPECIALISTE) Then
                If tache.Etat = TacheDao.EtatTache.EN_ATTENTE.ToString OrElse
                    (tache.Etat = TacheDao.EtatTache.EN_COURS.ToString And userLog.UtilisateurId = tache.TraiteUserId) Then
                    Cursor.Current = Cursors.WaitCursor
                    Me.Enabled = False
                    Using form As New RadFTacheModificationRendezVous
                        form.SelectedPatient = Me.SelectedPatient
                        form.SelectedTacheId = tache.Id
                        form.ShowDialog()
                        If form.CodeRetour = True Then
                            Me.RadDesktopAlert1.CaptionText = "Notification rendez-vous"
                            Me.RadDesktopAlert1.ContentText = "Rendez-vous modifié"
                            Me.RadDesktopAlert1.Show()
                            ChargementhistoriqueConsultation()
                            Me.CodeRetour = True
                        End If
                    End Using
                    Me.Enabled = True
                Else
                    MessageBox.Show("Le rendez-vous n'est pas modifiable, il est en cours de traitement par : " & userLog.UtilisateurPrenom & " " & userLog.UtilisateurNom, "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End If
        End If

        If DemandeRendezVous = True Then
            tache = tacheDao.GetProchaineDemandeRendezVousByPatientId(SelectedPatient.patientId, SelectedParcoursId)
            If tache.Id <> 0 AndAlso tache.Nature = TacheDao.EnumNatureTacheCode.RDV_DEMANDE Then
                If tache.Etat = TacheDao.EtatTache.EN_ATTENTE.ToString Then
                    Cursor.Current = Cursors.WaitCursor
                    Me.Enabled = False
                    Using form As New RadFTacheModificationDemandeRendezVous
                        form.SelectedPatient = Me.SelectedPatient
                        form.SelectedTacheId = tache.Id
                        form.ShowDialog()
                        If form.CodeRetour = True Then
                            Me.RadDesktopAlert1.CaptionText = "Notification demande de rendez-vous"
                            Me.RadDesktopAlert1.ContentText = "Demande de rendez-vous modifiée"
                            Me.RadDesktopAlert1.Show()
                            ChargementhistoriqueConsultation()
                            Me.CodeRetour = True
                        End If
                    End Using
                    Me.Enabled = True
                Else
                    MessageBox.Show("Le rendez-vous n'est pas modifiable, il est en cours de traitement par : " & userLog.UtilisateurPrenom & " " & userLog.UtilisateurNom, "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Warning)
                End If
            End If
        End If

    End Sub


    'Clôturer un rendez-vous spécialiste
    Private Sub RadBtnClotureRDV_Click(sender As Object, e As EventArgs) Handles RadBtnClotureRDV.Click
        If RendezVousPlanifie = True Then
            Dim tache As Tache
            tache = tacheDao.GetProchainRendezVousByPatientIdEtParcours(SelectedPatient.patientId, SelectedParcoursId)
            If tache.DateRendezVous.Date <= Date.Now.Date() Then
                If tache.Id <> 0 AndAlso (tache.Nature = TacheDao.EnumNatureTacheCode.RDV_SPECIALISTE Or tache.Nature = TacheDao.EnumNatureTacheCode.RDV) Then
                    If MsgBox("Confirmation de la clôture du rendez-vous", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
                        If tacheDao.ClotureTache(tache.Id, True) = True Then
                            Me.RadDesktopAlert1.CaptionText = "Notification rendez-vous"
                            Me.RadDesktopAlert1.ContentText = "Rendez-vous clôturé"
                            Me.RadDesktopAlert1.Show()
                            Dim tacheDao As New TacheDao
                            tacheDao.CreationAutomatiqueDeDemandeRendezVous(SelectedPatient, ParcoursUpdate, Date.Now())
                            ChargementhistoriqueConsultation()
                            Me.CodeRetour = True
                        End If
                    End If
                End If
            End If
        End If
    End Sub



    '============================================================================================================
    '============== Consignes IDE
    '============================================================================================================

    'Ajouter un acte paramédical (consigne IDE)
    Private Sub RadBtnConsigne_Click(sender As Object, e As EventArgs) Handles RadBtnActeParamedical.Click
        AjoutConsigneActeParamedical(DrcDao.EnumCategorieOasisCode.ActeParamedical)
    End Sub

    Private Sub CréerUneConsigneParamédicaleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneConsigneParamédicaleToolStripMenuItem.Click
        AjoutConsigneActeParamedical(DrcDao.EnumCategorieOasisCode.ActeParamedical)
    End Sub

    'Ajouter un groupe de paramètres (consigne IDE)
    Private Sub RadBtnParametreConsigne_Click(sender As Object, e As EventArgs) Handles RadBtnParametreConsigne.Click
        AjoutConsigneActeParamedical(DrcDao.EnumCategorieOasisCode.GroupeParametres)
    End Sub

    Private Sub AjouterUnGroupeDeParamètresToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AjouterUnGroupeDeParamètresToolStripMenuItem.Click
        AjoutConsigneActeParamedical(DrcDao.EnumCategorieOasisCode.GroupeParametres)
    End Sub

    'Ajouter un protocole collaboratif (consigne IDE)
    Private Sub AjouterUnProtocoleCollaboratifToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AjouterUnProtocoleCollaboratifToolStripMenuItem.Click
        AjoutConsigneActeParamedical(DrcDao.EnumCategorieOasisCode.ProtocoleCollaboratif)
    End Sub

    Private Sub RadBtnprotocoleConsigne_Click(sender As Object, e As EventArgs) Handles RadBtnprotocoleConsigne.Click
        AjoutConsigneActeParamedical(DrcDao.EnumCategorieOasisCode.ProtocoleCollaboratif)
    End Sub

    'Ajouter une mesure préventive (Consigne IDE)
    Private Sub RadBtnMesurePreventive_Click(sender As Object, e As EventArgs) Handles RadBtnMesurePreventive.Click
        AjoutConsigneActeParamedical(DrcDao.EnumCategorieOasisCode.Prevention)
    End Sub

    Private Sub AjouterUneMesurePréventiveToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles AjouterUneMesurePréventiveToolStripMenuItem.Click
        AjoutConsigneActeParamedical(DrcDao.EnumCategorieOasisCode.Prevention)
    End Sub

    'Ajout consigne IDE
    Private Sub AjoutConsigneActeParamedical(categorieOasisId As Integer)
        Dim SelectedDrcId As Integer
        Cursor.Current = Cursors.WaitCursor
        Using vFDrcSelecteur As New RadFDRCSelecteur
            vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
            vFDrcSelecteur.CategorieOasis = categorieOasisId
            vFDrcSelecteur.ShowDialog()
            SelectedDrcId = vFDrcSelecteur.SelectedDrcId
            'Si une DORC a été sélectionnée, on appelle le Formulaire de création
            If SelectedDrcId <> 0 Then
                Cursor.Current = Cursors.WaitCursor
                Using vRadFParcoursConsigneDetailEdit As New RadFParcoursConsigneDetailEdit
                    vRadFParcoursConsigneDetailEdit.SelectedPatient = Me.SelectedPatient
                    vRadFParcoursConsigneDetailEdit.UtilisateurConnecte = userLog
                    vRadFParcoursConsigneDetailEdit.SelectedParcoursId = SelectedParcoursId
                    vRadFParcoursConsigneDetailEdit.SelectedDrcId = SelectedDrcId
                    vRadFParcoursConsigneDetailEdit.SelectedConsigneId = 0
                    vRadFParcoursConsigneDetailEdit.ShowDialog() 'Modal
                    'Si le traitement a été créé, on recharge la grid
                    If vRadFParcoursConsigneDetailEdit.CodeRetour = True Then
                        RadParcoursConsigneDataGridView.Rows.Clear()
                        ChargementConsigne()
                    End If
                End Using
            End If
        End Using
    End Sub

    'Modification consigne IDE
    Private Sub MasterTemplate_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadParcoursConsigneDataGridView.CellDoubleClick
        If RadParcoursConsigneDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadParcoursConsigneDataGridView.Rows.IndexOf(Me.RadParcoursConsigneDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ConsigneId As Integer = RadParcoursConsigneDataGridView.Rows(aRow).Cells("consigneId").Value
                Cursor.Current = Cursors.WaitCursor
                Using vRadFParcoursConsigneDetailEdit As New RadFParcoursConsigneDetailEdit
                    vRadFParcoursConsigneDetailEdit.SelectedPatient = Me.SelectedPatient
                    'vRadFParcoursConsigneDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vRadFParcoursConsigneDetailEdit.SelectedParcoursId = SelectedParcoursId
                    vRadFParcoursConsigneDetailEdit.SelectedConsigneId = ConsigneId
                    vRadFParcoursConsigneDetailEdit.ShowDialog()
                    'Si le traitement a été créé, on recharge la grid
                    If vRadFParcoursConsigneDetailEdit.CodeRetour = True Then
                        RadParcoursConsigneDataGridView.Rows.Clear()
                        ChargementConsigne()
                    End If
                End Using
            End If
        End If
    End Sub

    Private Sub RadBtnTestActePara_Click(sender As Object, e As EventArgs) Handles RadBtnTestActePara.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using form As New RadFActeParamedicalTest
            form.SelectedPatient = Me.SelectedPatient
            form.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnTestParametre_Click(sender As Object, e As EventArgs) Handles RadBtnTestParametre.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using form As New RadFParametreTest
            form.SelectedPatient = Me.SelectedPatient
            form.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub


    '============================================================================================================
    '============== Général
    '============================================================================================================

    'Fonction remplacée par l'appel de la fonction dans tachedao  ===> Fonction à supprimer !!!!!!!!
    Private Sub SetEmetteurId()
        Select Case userLog.UtilisateurProfilId.Trim()
            Case "IDE"
                EmetteurFonctionId = FonctionDao.EnumFonction.IDE
            Case "IDE_REMPLACANT"
                EmetteurFonctionId = FonctionDao.EnumFonction.IDE_REMPLACANT
            Case "MEDECIN"
                EmetteurFonctionId = FonctionDao.EnumFonction.MEDECIN
            Case "SAGE_FEMME"
                EmetteurFonctionId = FonctionDao.EnumFonction.SAGE_FEMME
            Case "CADRE_SANTE"
                EmetteurFonctionId = FonctionDao.EnumFonction.CADRE_SANTE
            Case "SECRETAIRE_MEDICALE"
                EmetteurFonctionId = FonctionDao.EnumFonction.SECRETAIRE_MEDICALE
            Case "ADMINISTRATIF"
                EmetteurFonctionId = FonctionDao.EnumFonction.ADMINISTRATIF
            Case Else
                EmetteurFonctionId = FonctionDao.EnumFonction.INCONNU
        End Select

        Select Case ParcoursUpdate.SousCategorieId
            Case EnumSousCategoriePPS.medecinReferent
                DestinataireFonctionId = FonctionDao.EnumFonction.MEDECIN
                TraiteFonctionId = FonctionDao.EnumFonction.MEDECIN
            Case EnumSousCategoriePPS.IDE
                DestinataireFonctionId = FonctionDao.EnumFonction.IDE
                TraiteFonctionId = FonctionDao.EnumFonction.IDE
            Case EnumSousCategoriePPS.sageFemme
                If ParcoursUpdate.SpecialiteId = EnumSpecialiteOasis.sageFemmeOasis Then
                    DestinataireFonctionId = FonctionDao.EnumFonction.SAGE_FEMME
                    TraiteFonctionId = FonctionDao.EnumFonction.SAGE_FEMME
                Else
                    DestinataireFonctionId = FonctionDao.EnumFonction.SPECIALISTE_NON_OASIS
                    TraiteFonctionId = FonctionDao.EnumFonction.IDE
                End If
            Case EnumSousCategoriePPS.specialiste
                DestinataireFonctionId = FonctionDao.EnumFonction.SPECIALISTE_NON_OASIS
                TraiteFonctionId = FonctionDao.EnumFonction.IDE
            Case Else
                DestinataireFonctionId = FonctionDao.EnumFonction.INCONNU
                TraiteFonctionId = FonctionDao.EnumFonction.IDE
        End Select
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub GestionAffichageBoutonValidation()
        If EditMode = EnumEditMode.Modification Then
            If ParcoursDao.Compare(ParcoursUpdate, parcoursRead) = False Then
                RadBtnValidation.Enabled = True
                RadBtnRendezVous.Enabled = False
            Else
                RadBtnValidation.Enabled = False
                RadBtnRendezVous.Enabled = True
            End If
        End If
    End Sub

    'Visualisation détail intervenant (ROR)
    Private Sub RadBtnRorDetail_Click(sender As Object, e As EventArgs) Handles RadBtnRorDetail.Click
        Using vFRorDetailEdit As New RadFRorDetailEdit
            vFRorDetailEdit.SelectedRorId = ParcoursUpdate.RorId
            vFRorDetailEdit.ShowDialog() 'Modal
            If vFRorDetailEdit.CodeRetour = True Then
                ror = rordao.getRorById(ParcoursUpdate.RorId)
                TxtNomIntervenant.Text = ror.Nom
                TxtTypeIntervenant.Text = ror.Type
                TxtNomStructure.Text = ror.StructureNom
            End If
        End Using
    End Sub

    Private Sub FixeTailleEcranPourIDE()
        Me.Width = 1560
        Me.Height = 650
    End Sub


    Private Sub LblId_MouseHover(sender As Object, e As EventArgs) Handles LblId.MouseHover
        ToolTip1.SetToolTip(LblId, "Id : " + SelectedParcoursId.ToString)
    End Sub

    Private Sub RadParcoursConsigneDataGridView_ToolTipTextNeeded(sender As Object, e As ToolTipTextNeededEventArgs) Handles RadParcoursConsigneDataGridView.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "drcDescription" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("activite_type_episode").Value
        End If
    End Sub

    Private Sub LblDateProchainRendezVous_MouseHover(sender As Object, e As EventArgs) Handles LblDateProchainRendezVous.MouseHover
        If RendezVousPlanifieExiste = True Then
            ToolTip1.SetToolTip(LblDateProchainRendezVous, "Heure rendez-vous : " & DateRendezVous.ToString("HH:mm"))
        End If
    End Sub

    Private Sub DroitAcces()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            RadBtnActeParamedical.Hide()
            RadBtnMesurePreventive.Hide()
            RadBtnParametreConsigne.Hide()
            RadBtnprotocoleConsigne.Hide()
            RadBtnRORSelect.Hide()
            RadBtnAnnuler.Hide()
            NumRythme.Enabled = False
            CbxBase.Enabled = False
            TxtCommentaire.Enabled = False
            ChkMasquerIntervenant.Enabled = False
            CbxOasisExterne.Enabled = False
        End If
    End Sub

    Private Sub RadBtnHistorique_Click(sender As Object, e As EventArgs) Handles RadBtnHistorique.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Try
            Using vRadFParcoursHistoListe As New RadFParcoursHistoListe
                vRadFParcoursHistoListe.SelectedParcoursId = SelectedParcoursId
                vRadFParcoursHistoListe.SelectedPatient = Me.SelectedPatient
                vRadFParcoursHistoListe.UtilisateurConnecte = userLog
                vRadFParcoursHistoListe.ShowDialog()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
        Me.Enabled = True
    End Sub

    Private Sub RadBtnHistoRDV_Click(sender As Object, e As EventArgs) Handles RadBtnHistoRDV.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Try
            Using form As New RadFHistoriqueRDVPatient
                form.SelectedParcoursId = SelectedParcoursId
                form.SelectedPatient = Me.SelectedPatient
                form.ShowDialog()
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
        Me.Enabled = True
    End Sub
End Class
