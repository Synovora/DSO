Imports Telerik.WinControls.UI
Imports Oasis_Common
Public Class RadFAntecedentHistoListe
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedAntecedentId As Integer

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

    Dim UtilisateurHisto As Utilisateur = New Utilisateur()
    Dim alddao As New AldDao
    Dim ald As Ald

    Private Sub RadFAntecedentHistoListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - Me.Width, Screen.PrimaryScreen.WorkingArea.Height - Me.Height)
        initZones()
        ChargementEtatCivil()
        ChargementAntecedent()
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

    '==========================================================
    '======================= Antecedent =======================
    '==========================================================

    'Chargement de la Grid
    Private Sub ChargementAntecedent()
        Dim antecedentHistoDataTable As DataTable = New DataTable()
        Dim antecedentHistoDao As AntecedentHistoDao = New AntecedentHistoDao
        antecedentHistoDataTable = antecedentHistoDao.getAllAntecedentHistobyAntecedentId(SelectedAntecedentId)

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = antecedentHistoDataTable.Rows.Count - 1
        Dim natureHisto, ALDId, ALDCim10Id As Integer
        Dim ActionHistoString, Publication, typeAntecedent, TypeAntecedentString, categorieContexte, CategorieContexteString, DiagnosticString As String
        Dim dateDebut, dateFin, AldDateDebut, AldDateFin, AldDemandeDate As Date
        Dim dateHisto As DateTime
        Dim Arret, Inactif As Boolean

        'Initialisation des variables de comparaison
        Dim MaxDate As New Date(9999, 12, 31, 0, 0, 0)
        Dim DateDebutComp, DateFinComp, ALDDateDebutComp, ALDDateFinComp, ALDDemandeDateComp As Date
        Dim ArretComp, InactifComp, AldValide, AldDemande As Boolean
        Dim DescriptionComp, PublicationComp, ArretCommentaireComp, TypeAntecedentComp, CategorieContexteComp, DiagnosticComp As String
        Dim ALDDescription, ALDValideComp, ALDDemandeComp As String
        Dim DrcComp, NiveauComp, AntecedentId1Comp, AntecedentId2Comp, Ordre1Comp, Ordre2Comp, Ordre3Comp, ALDCim10IdComp As Integer


        DrcComp = 0
        TypeAntecedentComp = ""
        CategorieContexteComp = ""
        DescriptionComp = ""
        DiagnosticComp = ""
        PublicationComp = ""
        ArretComp = False
        ArretCommentaireComp = ""
        ALDDescription = ""
        NiveauComp = 0
        AntecedentId1Comp = 0
        AntecedentId2Comp = 0
        ALDCim10IdComp = 0
        InactifComp = False
        ALDValideComp = False
        ALDDemandeComp = False
        ALDDateDebutComp = Nothing
        ALDDateFinComp = Nothing
        ALDDemandeDateComp = Nothing

        Dim premierPassage As Boolean = True

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1

            'Initialisation Booléens de travail
            Inactif = False
            Arret = False


            'Type (antécédent ou Contexte)
            typeAntecedent = antecedentHistoDataTable.Rows(i)("oa_antecedent_type")
            Select Case typeAntecedent
                Case "A"
                    TypeAntecedentString = "Antécédent"
                Case "C"
                    TypeAntecedentString = "Contexte"
                Case Else
                    TypeAntecedentString = "Inconnu"
            End Select

            'Nature historisation
            natureHisto = antecedentHistoDataTable.Rows(i)("oa_antecedent_histo_etat_historisation")
            Select Case natureHisto
                Case 1
                    If typeAntecedent = "A" Then
                        ActionHistoString = "Creation Antecedent"
                    Else
                        ActionHistoString = "Creation Contexte"
                    End If
                Case 2
                    If typeAntecedent = "A" Then
                        ActionHistoString = "Modification Antecedent"
                    Else
                        ActionHistoString = "Modification Contexte"
                    End If
                Case 3
                    ActionHistoString = "Arret Contexte"
                Case 4
                    If typeAntecedent = "A" Then
                        ActionHistoString = "Annulation Antecedent"
                    Else
                        ActionHistoString = "Annulation Contexte"
                    End If
                Case 5
                    If typeAntecedent = "A" Then
                        ActionHistoString = "Transformation en Antecedent"
                    Else
                        ActionHistoString = "Réactivation du contexte"
                    End If
                Case Else
                    ActionHistoString = "Inconnue"
            End Select

            'Type (antécédent ou Contexte)
            If typeAntecedent = "C" Then
                categorieContexte = antecedentHistoDataTable.Rows(i)("oa_antecedent_categorie_contexte")
                Select Case categorieContexte
                    Case "M"
                        CategorieContexteString = "Médical"
                    Case "B"
                        CategorieContexteString = "Bio-environnemental"
                    Case Else
                        CategorieContexteString = "Inconnue"
                End Select
            Else
                CategorieContexteString = ""
            End If

            'Diagnostic
            DiagnosticString = ""
            If antecedentHistoDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                Dim Diagnostic As Integer = antecedentHistoDataTable.Rows(i)("oa_antecedent_diagnostic")
                Select Case Diagnostic
                    Case 1
                        DiagnosticString = "Confirmé"
                    Case 2
                        DiagnosticString = "Suspecté"
                    Case 3
                        DiagnosticString = "Supposé"
                    Case 4
                        DiagnosticString = "Notion"
                End Select
            End If

            'Date de début de l'antecedent
            If antecedentHistoDataTable.Rows(i)("oa_antecedent_date_debut") IsNot DBNull.Value Then
                dateDebut = antecedentHistoDataTable.Rows(i)("oa_antecedent_date_debut")
            Else
                dateDebut = Nothing
            End If

            'Date de fin de validité du contexte
            If antecedentHistoDataTable.Rows(i)("oa_antecedent_date_fin") IsNot DBNull.Value Then
                dateFin = antecedentHistoDataTable.Rows(i)("oa_antecedent_date_fin")
            Else
                dateFin = Nothing
            End If

            'Identification si l'antecedent a été arrêté
            If antecedentHistoDataTable.Rows(i)("oa_antecedent_arret") IsNot DBNull.Value Then
                If antecedentHistoDataTable.Rows(i)("oa_antecedent_arret") = "1" Then
                    Arret = True
                End If
            End If

            'Identification si le antecedent a été annulé
            If antecedentHistoDataTable.Rows(i)("oa_antecedent_inactif") IsNot DBNull.Value Then
                If antecedentHistoDataTable.Rows(i)("oa_antecedent_inactif") = "1" Then
                    Inactif = True
                End If
            End If

            'Alimentation de la >Grid
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadAntecedentDataGridView.Rows.Add(iGrid)


            '------------------- Alimentation du DataGridView
            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentId").Value = antecedentHistoDataTable.Rows(i)("oa_antecedent_id")


            'Date historisation
            dateHisto = antecedentHistoDataTable.Rows(i)("oa_antecedent_histo_date_historisation")
            RadAntecedentDataGridView.Rows(iGrid).Cells("histoDate").Value = dateHisto.ToString("dd.MM.yyyy HH:mm:ss")

            'Utilisateur
            Dim UtilisateurId As Integer = antecedentHistoDataTable.Rows(i)("oa_antecedent_histo_utilisateur_historisation")
            Dim userDao As New UserDao
            UtilisateurHisto = userDao.getUserById(UtilisateurId)
            'SetUtilisateur(UtilisateurHisto, UtilisateurId)
            RadAntecedentDataGridView.Rows(iGrid).Cells("histoUtilisateur").Value = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom

            'Action
            RadAntecedentDataGridView.Rows(iGrid).Cells("histoAction").Value = ActionHistoString

            'DRC
            RadAntecedentDataGridView.Rows(iGrid).Cells("drc").Value = antecedentHistoDataTable.Rows(i)("oa_antecedent_drc_id")
            Dim drcdao As New DrcDao
            Dim drc As Drc = New Drc()
            If drcdao.GetDrc(drc, antecedentHistoDataTable.Rows(i)("oa_antecedent_drc_id")) = True Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("oa_drc_libelle").Value = drc.DrcLibelle
            Else
                RadAntecedentDataGridView.Rows(iGrid).Cells("oa_drc_libelle").Value = ""
            End If
            If antecedentHistoDataTable.Rows(i)("oa_antecedent_drc_id").ToString <> DrcComp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("drc").Style.ForeColor = Color.Red
                RadAntecedentDataGridView.Rows(iGrid).Cells("oa_drc_libelle").Style.ForeColor = Color.Red
            End If
            DrcComp = antecedentHistoDataTable.Rows(i)("oa_antecedent_drc_id").ToString



            'Description
            RadAntecedentDataGridView.Rows(iGrid).Cells("description").Value = antecedentHistoDataTable.Rows(i)("oa_antecedent_description")
            If antecedentHistoDataTable.Rows(i)("oa_antecedent_description").ToString <> DescriptionComp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("description").Style.ForeColor = Color.Red
            End If
            DescriptionComp = antecedentHistoDataTable.Rows(i)("oa_antecedent_description")

            'Type
            RadAntecedentDataGridView.Rows(iGrid).Cells("type").Value = TypeAntecedentString
            If TypeAntecedentString <> TypeAntecedentComp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("type").Style.ForeColor = Color.Red
            End If
            TypeAntecedentComp = TypeAntecedentString

            'Diagnostic
            RadAntecedentDataGridView.Rows(iGrid).Cells("diagnostic").Value = DiagnosticString
            If DiagnosticString <> DiagnosticComp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("diagnostic").Style.ForeColor = Color.Red
            End If
            DiagnosticComp = DiagnosticString

            'Catégorie
            RadAntecedentDataGridView.Rows(iGrid).Cells("categorieContexte").Value = CategorieContexteString
            If CategorieContexteString <> CategorieContexteComp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("categorieContexte").Style.ForeColor = Color.Red
            End If
            CategorieContexteComp = CategorieContexteString

            'Date début
            Dim MinDate As New Date(1753, 1, 1, 0, 0, 0)
            If dateDebut = Nothing Or dateDebut = MinDate Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("dateDebut").Value = "-"
            Else
                RadAntecedentDataGridView.Rows(iGrid).Cells("dateDebut").Value = dateDebut.ToString("dd.MM.yyyy")
            End If
            If dateDebut <> DateDebutComp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("dateDebut").Style.ForeColor = Color.Red
            End If
            DateDebutComp = dateDebut

            'Date fin de validité
            If typeAntecedent = "C" Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("dateFin").Value = dateFin.ToString("dd.MM.yyyy")
                If dateFin <> DateFinComp And premierPassage = False Then
                    RadAntecedentDataGridView.Rows(iGrid).Cells("dateFin").Style.ForeColor = Color.Red
                End If
                DateFinComp = dateFin
            Else
                RadAntecedentDataGridView.Rows(iGrid).Cells("dateFin").Value = "-"
            End If

            'Publication
            Publication = antecedentHistoDataTable.Rows(i)("oa_antecedent_statut_affichage")
            Select Case Publication
                Case "P"
                    RadAntecedentDataGridView.Rows(iGrid).Cells("publication").Value = "Publié"
                Case "C"
                    RadAntecedentDataGridView.Rows(iGrid).Cells("publication").Value = "Caché"
                Case "O"
                    RadAntecedentDataGridView.Rows(iGrid).Cells("publication").Value = "Occulté"
                Case Else
                    RadAntecedentDataGridView.Rows(iGrid).Cells("publication").Value = "Inconnu"
            End Select
            If antecedentHistoDataTable.Rows(i)("oa_antecedent_statut_affichage").ToString <> PublicationComp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("publication").Style.ForeColor = Color.Red
            End If
            PublicationComp = antecedentHistoDataTable.Rows(i)("oa_antecedent_statut_affichage")

            'ALD
            ALDId = Coalesce(antecedentHistoDataTable.Rows(i)("oa_antecedent_ald_id"), 0)
            If ALDId <> 0 Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_id").Value = ALDId.ToString
                ald = alddao.GetAldById(ALDId)
                RadAntecedentDataGridView.Rows(iGrid).Cells("oa_ald_description").Value = ald.AldDescription

                ALDCim10Id = Coalesce(antecedentHistoDataTable.Rows(i)("oa_antecedent_ald_cim_10_id"), 0)
                RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_cim_10_id").Value = ALDCim10Id.ToString
                If antecedentHistoDataTable.Rows(i)("oa_antecedent_ald_cim_10_id").ToString <> ALDCim10IdComp And premierPassage = False Then
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_cim_10_id").Style.ForeColor = Color.Red
                End If
                ALDCim10IdComp = antecedentHistoDataTable.Rows(i)("oa_antecedent_ald_cim_10_id")

                AldValide = Coalesce(antecedentHistoDataTable.Rows(i)("oa_antecedent_ald_valide"), False)
                If AldValide = True Then
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_valide").Value = "ALD Valide"
                Else
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_valide").Value = "ALD non valide"
                End If
                If antecedentHistoDataTable.Rows(i)("oa_antecedent_ald_valide").ToString <> ALDValideComp And premierPassage = False Then
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_valide").Style.ForeColor = Color.Red
                End If
                ALDValideComp = antecedentHistoDataTable.Rows(i)("oa_antecedent_ald_valide")

                AldDateDebut = Coalesce(antecedentHistoDataTable.Rows(i)("oa_antecedent_ald_date_debut"), Nothing)
                If AldDateDebut.Date = MaxDate.Date Or AldDateDebut.Date = Nothing Then
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_date_debut").Value = "-"
                Else
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_date_debut").Value = AldDateDebut.ToString("dd.MM.yyyy")
                End If
                If AldDateDebut.Date <> ALDDateDebutComp.Date And premierPassage = False Then
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_date_debut").Style.ForeColor = Color.Red
                End If
                ALDDateDebutComp = AldDateDebut

                AldDateFin = Coalesce(antecedentHistoDataTable.Rows(i)("oa_antecedent_ald_date_fin"), Nothing)
                If AldDateFin.Date = MaxDate.Date Or AldDateFin.Date = Nothing Then
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_date_fin").Value = "-"
                Else
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_date_fin").Value = AldDateFin.ToString("dd.MM.yyyy")
                End If
                If AldDateFin.Date <> ALDDateFinComp.Date And premierPassage = False Then
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_date_fin").Style.ForeColor = Color.Red
                End If
                ALDDateFinComp = AldDateFin

                AldDemande = Coalesce(antecedentHistoDataTable.Rows(i)("oa_antecedent_ald_demande_en_cours"), False)
                If AldDemande = True Then
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_demande_en_cours").Value = "demande en cours"
                Else
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_demande_en_cours").Value = "Pas de demande"
                End If
                If AldDemande <> ALDDemandeComp And premierPassage = False Then
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_demande_en_cours").Style.ForeColor = Color.Red
                End If
                ALDDemandeComp = AldDemande

                AldDemandeDate = Coalesce(antecedentHistoDataTable.Rows(i)("oa_antecedent_ald_demande_date"), Nothing)
                If AldDemandeDate.Date = MaxDate.Date Or AldDemandeDate.Date = Nothing Then
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_demande_date").Value = "-"
                Else
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_demande_date").Value = AldDemandeDate.ToString("dd.MM.yyyy")
                End If
                If AldDemandeDate.Date <> ALDDemandeDateComp.Date And premierPassage = False Then
                    RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_demande_date").Style.ForeColor = Color.Red
                End If
                ALDDemandeDateComp = AldDemandeDate
            Else
                RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_id").Value = ""
                RadAntecedentDataGridView.Rows(iGrid).Cells("oa_ald_description").Value = ""
                RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_cim_10_id").Value = ""
                RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_valide").Value = ""
                RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_date_debut").Value = ""
                RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_date_fin").Value = ""
                RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_demande_en_cours").Value = ""
                RadAntecedentDataGridView.Rows(iGrid).Cells("oa_antecedent_ald_demande_date").Value = ""
                ALDCim10IdComp = 0
                ALDValideComp = ""
                ALDDateDebutComp = Nothing
                ALDDateFinComp = Nothing
                ALDDemandeComp = ""
                ALDDemandeDateComp = Nothing
            End If

            'Arrêt
            If Arret = True Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("arret").Value = "Oui"
            Else
                RadAntecedentDataGridView.Rows(iGrid).Cells("arret").Value = "Non"
            End If
            If Arret <> ArretComp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("arret").Style.ForeColor = Color.Red
            End If
            ArretComp = Arret

            'Commentaire arret
            RadAntecedentDataGridView.Rows(iGrid).Cells("arretCommentaire").Value = antecedentHistoDataTable.Rows(i)("oa_antecedent_arret_commentaire")
            If antecedentHistoDataTable.Rows(i)("oa_antecedent_arret_commentaire").ToString <> ArretCommentaireComp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("arretCommentaire").Style.ForeColor = Color.Red
            End If
            ArretCommentaireComp = antecedentHistoDataTable.Rows(i)("oa_antecedent_arret_commentaire").ToString

            'Niveau
            RadAntecedentDataGridView.Rows(iGrid).Cells("niveau").Value = antecedentHistoDataTable.Rows(i)("oa_antecedent_niveau")
            If antecedentHistoDataTable.Rows(i)("oa_antecedent_niveau").ToString <> NiveauComp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("niveau").Style.ForeColor = Color.Red
            End If
            NiveauComp = antecedentHistoDataTable.Rows(i)("oa_antecedent_niveau").ToString

            'Antecedent identifiant niveau 1
            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentId1").Value = antecedentHistoDataTable.Rows(i)("oa_antecedent_id_niveau1")
            If antecedentHistoDataTable.Rows(i)("oa_antecedent_id_niveau1").ToString <> AntecedentId1Comp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentId1").Style.ForeColor = Color.Red
            End If
            AntecedentId1Comp = antecedentHistoDataTable.Rows(i)("oa_antecedent_id_niveau1").ToString

            'Antecedent identifiant niveau 2
            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentId2").Value = antecedentHistoDataTable.Rows(i)("oa_antecedent_id_niveau2")
            If antecedentHistoDataTable.Rows(i)("oa_antecedent_id_niveau2").ToString <> AntecedentId2Comp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentId2").Style.ForeColor = Color.Red
            End If
            AntecedentId2Comp = antecedentHistoDataTable.Rows(i)("oa_antecedent_id_niveau2").ToString

            'Activité
            If Inactif = True Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("inactif").Value = "Supprimé"
            Else
                RadAntecedentDataGridView.Rows(iGrid).Cells("inactif").Value = "Actif"
            End If
            If Inactif <> InactifComp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("inactif").Style.ForeColor = Color.Red
            End If
            InactifComp = Inactif

            'Ordre 1
            RadAntecedentDataGridView.Rows(iGrid).Cells("ordre1").Value = antecedentHistoDataTable.Rows(i)("oa_antecedent_ordre_affichage1")
            If antecedentHistoDataTable.Rows(i)("oa_antecedent_ordre_affichage1").ToString <> Ordre1Comp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("ordre1").Style.ForeColor = Color.Red
            End If
            Ordre1Comp = antecedentHistoDataTable.Rows(i)("oa_antecedent_ordre_affichage1").ToString

            'Ordre 2
            RadAntecedentDataGridView.Rows(iGrid).Cells("ordre2").Value = antecedentHistoDataTable.Rows(i)("oa_antecedent_ordre_affichage2")
            If antecedentHistoDataTable.Rows(i)("oa_antecedent_ordre_affichage2").ToString <> Ordre2Comp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("ordre2").Style.ForeColor = Color.Red
            End If
            Ordre2Comp = antecedentHistoDataTable.Rows(i)("oa_antecedent_ordre_affichage2").ToString

            'Ordre 3
            RadAntecedentDataGridView.Rows(iGrid).Cells("ordre3").Value = antecedentHistoDataTable.Rows(i)("oa_antecedent_ordre_affichage3")
            If antecedentHistoDataTable.Rows(i)("oa_antecedent_ordre_affichage3").ToString <> Ordre3Comp And premierPassage = False Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("ordre3").Style.ForeColor = Color.Red
            End If
            Ordre3Comp = antecedentHistoDataTable.Rows(i)("oa_antecedent_ordre_affichage3").ToString

            premierPassage = False
        Next

        'Positionnement du grid sur la première occurrence
        If RadAntecedentDataGridView.Rows.Count > 0 Then
            Me.RadAntecedentDataGridView.CurrentRow = RadAntecedentDataGridView.ChildRows(0)
        End If
    End Sub

    '===========================================================
    '======================= Généralités =======================
    '===========================================================

    'Initialisation de l'écran
    Private Sub initZones()
        'Etat civil
        LblPatientNIR.Text = ""
        LblPatientPrenom.Text = ""
        LblPatientNom.Text = ""
        LblPatientAge.Text = ""
        LblPatientGenre.Text = ""
        LblPatientAdresse1.Text = ""
        LblPatientAdresse2.Text = ""
        LblPatientCodePostal.Text = ""
        LblPatientVille.Text = ""
        LblPatientTel1.Text = ""
        LblPatientTel2.Text = ""
        LblPatientSite.Text = ""
        LblPatientUniteSanitaire.Text = ""
        LblPatientDateMaj.Text = ""
        'Antecedents
        RadAntecedentDataGridView.Rows.Clear()
    End Sub

    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Close()
    End Sub

    Private Sub RadAntecedentDataGridView_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RadAntecedentDataGridView.ToolTipTextNeeded
        Dim cell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If cell IsNot Nothing AndAlso (cell.ColumnInfo.Name = "description" Or
            cell.ColumnInfo.Name = "histoAction") Then
            e.ToolTipText = cell.Value.ToString()
        End If
    End Sub
End Class
