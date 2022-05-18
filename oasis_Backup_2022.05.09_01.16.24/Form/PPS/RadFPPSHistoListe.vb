Imports Oasis_Common
Imports Oasis_WF

Public Class RadFPPSHistoListe
    Private _SelectedPatient As Patient
    Private _UtilisateurConnecte As Utilisateur
    Private _SelectedPPSId As Integer

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

    Public Property SelectedPPSId As Integer
        Get
            Return _SelectedPPSId
        End Get
        Set(value As Integer)
            _SelectedPPSId = value
        End Set
    End Property

    Dim UtilisateurHisto As New Utilisateur
    Dim ppsHistoDao As New PPSHistoDao

    Private Sub RadFPPSHistoListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementEtatCivil()
        ChargementHistorique()
    End Sub

    'Chargement de la Grid
    Private Sub ChargementHistorique()
        Dim ppsHistoDataTable As DataTable
        ppsHistoDataTable = ppsHistoDao.GetAllPPSHistobyPPSId(SelectedPPSId)

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = ppsHistoDataTable.Rows.Count - 1
        Dim natureHisto As Integer
        Dim ActionHistoString As String
        Dim dateHisto As DateTime
        Dim Inactif As Boolean

        'Initialisation des variables de comparaison
        Dim AffichageSyntheseComp, InactifComp, ArretComp As Boolean
        Dim CommentaireComp, CommentaireArretComp, DrcComp As String
        Dim PrioriteComp As Integer
        Dim DateDebutComp As Date

        CommentaireComp = ""
        CommentaireArretComp = ""
        DrcComp = ""
        PrioriteComp = 0
        InactifComp = False
        AffichageSyntheseComp = False
        ArretComp = False
        DateDebutComp = Nothing

        Dim premierPassage As Boolean = True

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            'Nature historisation
            natureHisto = ppsHistoDataTable.Rows(i)("oa_pps_histo_etat_historisation")
            Select Case natureHisto
                Case PpsHisto.EnumEtatPPSHisto.Creation
                    ActionHistoString = "Creation PPS"
                Case PpsHisto.EnumEtatPPSHisto.Modification
                    ActionHistoString = "Modification PPS"
                Case PpsHisto.EnumEtatPPSHisto.Annulation
                    ActionHistoString = "Annulation PPS"
                Case PpsHisto.EnumEtatPPSHisto.Arret
                    ActionHistoString = "Arrêt PPS"
                Case Else
                    ActionHistoString = "Action inconnue"
            End Select

            'Alimentation de la >Grid
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadPPSDataGridView.Rows.Add(iGrid)

            '------------------- Alimentation du DataGridView
            RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_id").Value = ppsHistoDataTable.Rows(i)("oa_pps_id")
            'Date historisation
            dateHisto = ppsHistoDataTable.Rows(i)("oa_pps_histo_date_historisation")
            RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_histo_date_historisation").Value = dateHisto.ToString("dd.MM.yyyy HH:mm:ss")
            'Utilisateur
            Dim UtilisateurId As Integer = ppsHistoDataTable.Rows(i)("oa_pps_histo_utilisateur_historisation")
            Dim userDao As New UserDao
            UtilisateurHisto = userDao.GetUserById(UtilisateurId)
            'SetUtilisateur(UtilisateurHisto, UtilisateurId)
            RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_histo_utilisateur_historisation").Value = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
            'Action
            RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_histo_etat_historisation").Value = ActionHistoString

            RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_priorite").Value = ppsHistoDataTable.Rows(i)("oa_pps_priorite")
            If ppsHistoDataTable.Rows(i)("oa_pps_priorite") <> PrioriteComp And premierPassage = False Then
                RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_priorite").Style.ForeColor = Color.Red
            End If
            PrioriteComp = ppsHistoDataTable.Rows(i)("oa_pps_priorite")

            RadPPSDataGridView.Rows(iGrid).Cells("oa_drc_libelle").Value = ppsHistoDataTable.Rows(i)("oa_drc_libelle")
            If ppsHistoDataTable.Rows(i)("oa_drc_libelle").ToString <> DrcComp And premierPassage = False Then
                RadPPSDataGridView.Rows(iGrid).Cells("oa_drc_libelle").Style.ForeColor = Color.Red
            End If
            DrcComp = ppsHistoDataTable.Rows(i)("oa_drc_libelle")

            RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_affichage_synthese").Value = ppsHistoDataTable.Rows(i)("oa_pps_affichage_synthese")
            If ppsHistoDataTable.Rows(i)("oa_pps_affichage_synthese") <> AffichageSyntheseComp And premierPassage = False Then
                RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_affichage_synthese").Style.ForeColor = Color.Red
            End If
            AffichageSyntheseComp = ppsHistoDataTable.Rows(i)("oa_pps_affichage_synthese")

            RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_commentaire").Value = ppsHistoDataTable.Rows(i)("oa_pps_commentaire")
            If ppsHistoDataTable.Rows(i)("oa_pps_commentaire").ToString <> CommentaireComp And premierPassage = False Then
                RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_commentaire").Style.ForeColor = Color.Red
            End If
            CommentaireComp = ppsHistoDataTable.Rows(i)("oa_pps_commentaire")

            RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_date_debut").Value = ppsHistoDataTable.Rows(i)("oa_pps_date_debut")
            If ppsHistoDataTable.Rows(i)("oa_pps_date_debut") <> DateDebutComp And premierPassage = False Then
                RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_date_debut").Style.ForeColor = Color.Red
            End If
            DateDebutComp = ppsHistoDataTable.Rows(i)("oa_pps_date_debut")

            RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_arret").Value = ppsHistoDataTable.Rows(i)("oa_pps_arret")
            If ppsHistoDataTable.Rows(i)("oa_pps_arret").ToString <> ArretComp And premierPassage = False Then
                RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_arret").Style.ForeColor = Color.Red
            End If
            ArretComp = ppsHistoDataTable.Rows(i)("oa_pps_arret")

            RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_commentaire_arret").Value = ppsHistoDataTable.Rows(i)("oa_pps_commentaire_arret")
            If ppsHistoDataTable.Rows(i)("oa_pps_commentaire_arret").ToString <> CommentaireArretComp And premierPassage = False Then
                RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_commentaire_arret").Style.ForeColor = Color.Red
            End If
            CommentaireArretComp = ppsHistoDataTable.Rows(i)("oa_pps_commentaire_arret")

            Inactif = Coalesce(ppsHistoDataTable.Rows(i)("oa_pps_inactif"), False)
            If Inactif = True Then
                RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_inactif").Value = "Supprimé"
            Else
                RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_inactif").Value = "Actif"
            End If
            If Inactif <> InactifComp And premierPassage = False Then
                RadPPSDataGridView.Rows(iGrid).Cells("oa_pps_inactif").Style.ForeColor = Color.Red
            End If
            InactifComp = Inactif

            premierPassage = False
        Next

        'Positionnement du grid sur la première occurrence
        If RadPPSDataGridView.Rows.Count > 0 Then
            Me.RadPPSDataGridView.CurrentRow = RadPPSDataGridView.ChildRows(0)
        End If
    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = Me.SelectedPatient.PatientNir
        LblPatientPrenom.Text = Me.SelectedPatient.PatientPrenom
        LblPatientNom.Text = Me.SelectedPatient.PatientNom
        LblPatientAge.Text = Me.SelectedPatient.PatientAge
        LblPatientGenre.Text = Me.SelectedPatient.PatientGenre
        LblPatientAdresse1.Text = Me.SelectedPatient.PatientAdresse1
        LblPatientAdresse2.Text = Me.SelectedPatient.PatientAdresse2
        LblPatientCodePostal.Text = Me.SelectedPatient.PatientCodePostal
        LblPatientVille.Text = Me.SelectedPatient.PatientVille
        LblPatientTel1.Text = Me.SelectedPatient.PatientTel1
        LblPatientTel2.Text = Me.SelectedPatient.PatientTel2
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(Me.SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(Me.SelectedPatient.PatientUniteSanitaireId)
        LblPatientDateMaj.Text = Me.SelectedPatient.PatientSyntheseDateMaj.ToString("dd/MM/yyyy")

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Close()
    End Sub
End Class
