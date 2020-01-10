Imports Telerik.WinControls.UI

Public Class RadFTraitementHistoListe
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedTraitementId As Integer
    Private privateMedicamentDenomination As String

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

    Public Property SelectedTraitementId As Integer
        Get
            Return privateSelectedTraitementId
        End Get
        Set(value As Integer)
            privateSelectedTraitementId = value
        End Set
    End Property

    Public Property MedicamentDenomination As String
        Get
            Return privateMedicamentDenomination
        End Get
        Set(value As String)
            privateMedicamentDenomination = value
        End Set
    End Property


    Dim UtilisateurHisto As Utilisateur = New Utilisateur()
    Private Sub RadFTraitementHistoListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initZones()
        ChargementEtatCivil()
        LblMedicamentDenomination.Text = Me.MedicamentDenomination
        ChargementTraitement()
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

    'Chargement de la Grid
    Private Sub ChargementTraitement()
        Dim traitementDataTable As DataTable = New DataTable()
        traitementDataTable = TraitementHistoDao.getAllHistoTraitementbyId(SelectedTraitementId)

        'Ajout d'une colonne 'oa_traitement_posologie' dans le DataTable de traitement
        traitementDataTable.Columns.Add("oa_traitement_posologie", Type.GetType("System.String"))

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = traitementDataTable.Rows.Count - 1
        Dim natureHisto As Integer
        Dim natureHistoString As String
        Dim Base As String
        Dim allergie, contreIndication As Boolean
        Dim Posologie As String
        Dim dateFin, dateDebut, datefinFenetre, dateDebutFenetre As Date
        Dim dateHisto As DateTime
        Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
        Dim Rythme As Integer
        Dim Arret, Annulation, Fenetre As Boolean

        'Initialisation des variables de comparaison
        Dim DateDebutComp, DateFinComp, DateDebutFenetreComp, DateFinFenetreComp As Date
        Dim FenetreComp, ArretComp, AllergieComp, ContreIndicationComp, AnnulationComp As Boolean
        Dim PosologieComp, OrdreAffichageComp, PosologieDureeComp, TraitementCommentaireComp, PosologieCommentaireComp, FenetreCommentaireComp, ArretCommentaireComp, AnnulationCommentaireComp As String

        OrdreAffichageComp = ""
        PosologieComp = ""
        PosologieDureeComp = ""
        PosologieCommentaireComp = ""
        FenetreCommentaireComp = ""
        ArretCommentaireComp = ""
        AnnulationCommentaireComp = ""
        TraitementCommentaireComp = ""

        Dim premierPassage As Boolean = True

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1

            'Initialisation Booléens de travail
            allergie = False
            contreIndication = False
            Annulation = False
            Arret = False
            Fenetre = False


            'Nature historisation
            natureHisto = traitementDataTable.Rows(i)("oa_traitement_histo_etat_historisation")
            Select Case natureHisto
                Case 1
                    natureHistoString = "Creation Traitement"
                Case 2
                    natureHistoString = "Modification Traitement"
                Case 3
                    natureHistoString = "Arret Traitement"
                Case 4
                    natureHistoString = "Annulation Traitement"
                Case 5
                    natureHistoString = "Creation Fenêtre Thérapeutique"
                Case 6
                    natureHistoString = "Modification Fenêtre Thérapeutique"
                Case 7
                    natureHistoString = "Suppression Fenêtre Thérapeutique"
                Case 8
                    natureHistoString = "Suppression Traitement"
                Case Else
                    natureHistoString = "Inconnue"
            End Select

            'Date de début de traitement
            If traitementDataTable.Rows(i)("oa_traitement_date_debut") IsNot DBNull.Value Then
                dateDebut = traitementDataTable.Rows(i)("oa_traitement_date_debut")
            Else
                dateDebut = Nothing
            End If

            'Date de fin de traitement
            If traitementDataTable.Rows(i)("oa_traitement_date_fin") IsNot DBNull.Value Then
                dateFin = traitementDataTable.Rows(i)("oa_traitement_date_fin")
            Else
                dateFin = "31/12/2999"
            End If

            'Formatage de la posologie
            Posologie = ""

            If Fenetre = False Then
                If traitementDataTable.Rows(i)("oa_traitement_posologie_base") IsNot DBNull.Value Then
                    Rythme = traitementDataTable.Rows(i)("oa_traitement_posologie_rythme")
                    Select Case traitementDataTable.Rows(i)("oa_traitement_posologie_base")
                        Case "J"
                            Base = "Journalier : "
                            If traitementDataTable.Rows(i)("oa_traitement_posologie_matin") <> 0 Then
                                posologieMatin = Rythme
                            Else
                                posologieMatin = 0
                            End If
                            If traitementDataTable.Rows(i)("oa_traitement_posologie_midi") <> 0 Then
                                posologieMidi = Rythme
                            Else
                                posologieMidi = 0
                            End If
                            If traitementDataTable.Rows(i)("oa_traitement_posologie_soir") <> 0 Then
                                posologieSoir = Rythme
                            Else
                                posologieSoir = 0
                            End If
                            If traitementDataTable.Rows(i)("oa_traitement_posologie_apres_midi") <> 0 Then
                                posologieApresMidi = Rythme
                                Posologie = Base + posologieMatin.ToString + "." + posologieMidi.ToString + "." + posologieApresMidi.ToString + "." + posologieSoir.ToString
                            Else
                                Posologie = Base + " " + posologieMatin.ToString + "." + posologieMidi.ToString + "." + posologieSoir.ToString
                            End If
                        Case "H"
                            Base = "Hebdo : "
                            Posologie = Base + Rythme.ToString
                        Case "M"
                            Base = "Mensuel : "
                            Posologie = Base + Rythme.ToString
                        Case "A"
                            Base = "Annuel : "
                            Posologie = Base + Rythme.ToString
                        Case Else
                            Base = "Base inconnue ! "
                            Posologie = Base + Rythme.ToString
                    End Select
                End If
            End If

            'Identification si une fenêtre a été créée
            If traitementDataTable.Rows(i)("oa_traitement_fenetre") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_fenetre") = "1" Then
                    Fenetre = True
                End If
            End If

            'Date de début de fenêtre
            If traitementDataTable.Rows(i)("oa_traitement_fenetre_date_debut") IsNot DBNull.Value Then
                dateDebutFenetre = traitementDataTable.Rows(i)("oa_traitement_fenetre_date_debut")
            Else
                dateDebutFenetre = Nothing
            End If

            'Date de fin de fenêtre
            If traitementDataTable.Rows(i)("oa_traitement_fenetre_date_fin") IsNot DBNull.Value Then
                datefinFenetre = traitementDataTable.Rows(i)("oa_traitement_fenetre_date_fin")
            Else
                datefinFenetre = Nothing
            End If

            'Identification si le traitement a été arrêté
            If traitementDataTable.Rows(i)("oa_traitement_arret") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_arret") = "A" Then
                    Arret = True
                End If
            End If

            'Identification si le traitement a été annulé
            If traitementDataTable.Rows(i)("oa_traitement_annulation") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_annulation") = "A" Then
                    Annulation = True
                End If
            End If

            'Identification si le traitement a été déclaré "Allergie"
            If traitementDataTable.Rows(i)("oa_traitement_allergie") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_allergie") = "1" Then
                    allergie = True
                End If
            End If

            'Identification si le traitement a été déclaré "Contre-indication"
            If traitementDataTable.Rows(i)("oa_traitement_contre_indication") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_contre_indication") = "1" Then
                    contreIndication = True
                End If
            End If


            'Alimentation de la >Grid
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            TraitementDataGridView.Rows.Add(iGrid)

            '------------------- Alimentation du DataGridView

            'Date historisation
            dateHisto = traitementDataTable.Rows(i)("oa_traitement_histo_date_historisation")
            TraitementDataGridView.Rows(iGrid).Cells("histoDate").Value = dateHisto.ToString("dd.MM.yyyy HH:mm:ss")


            'Utilisateur
            Dim UtilisateurId As Integer = traitementDataTable.Rows(i)("oa_traitement_histo_utilisateur_historisation")
            SetUtilisateur(UtilisateurHisto, UtilisateurId)
            TraitementDataGridView.Rows(iGrid).Cells("histoUtilisateur").Value = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom

            'Nature de l'action
            TraitementDataGridView.Rows(iGrid).Cells("histoNature").Value = natureHistoString

            'Date début
            TraitementDataGridView.Rows(iGrid).Cells("dateDebut").Value = dateDebut.ToString("dd.MM.yyyy")
            If dateDebut <> DateDebutComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("dateDebut").Style.ForeColor = Color.Red
            End If
            DateDebutComp = dateDebut

            'Date fin
            If dateFin = "31/12/2999" Then
                TraitementDataGridView.Rows(iGrid).Cells("dateFin").Value = "Non définie"
            Else
                TraitementDataGridView.Rows(iGrid).Cells("dateFin").Value = dateFin.ToString("dd.MM.yyyy")
            End If
            'TraitementDataGridView("dateFin", iGrid).Value = dateFin.ToString("dd.MM.yyyy")
            If dateFin <> DateFinComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("dateFin").Style.ForeColor = Color.Red
            End If
            DateFinComp = dateFin

            'Ordre d'affichage
            TraitementDataGridView.Rows(iGrid).Cells("ordreAffichage").Value = traitementDataTable.Rows(i)("oa_traitement_ordre_affichage").ToString
            If traitementDataTable.Rows(i)("oa_traitement_ordre_affichage").ToString <> OrdreAffichageComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("ordreAffichage").Style.ForeColor = Color.Red
            End If
            OrdreAffichageComp = traitementDataTable.Rows(i)("oa_traitement_ordre_affichage").ToString

            'Commentaire traitement
            TraitementDataGridView.Rows(iGrid).Cells("commentaire").Value = traitementDataTable.Rows(i)("oa_traitement_commentaire")
            If traitementDataTable.Rows(i)("oa_traitement_commentaire").ToString <> TraitementCommentaireComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("commentaire").Style.ForeColor = Color.Red
            End If
            TraitementCommentaireComp = traitementDataTable.Rows(i)("oa_traitement_commentaire").ToString

            'Posologie
            TraitementDataGridView.Rows(iGrid).Cells("posologie").Value = Posologie
            If Posologie <> PosologieComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("posologie").Style.ForeColor = Color.Red
            End If
            PosologieComp = Posologie

            'Durée posologie
            If dateFin = "31/12/2999" Then
                TraitementDataGridView.Rows(iGrid).Cells("PosologieDuree").Value = ""
            Else
                TraitementDataGridView.Rows(iGrid).Cells("PosologieDuree").Value = CalculDureeTraitement(dateDebut, dateFin)
            End If
            If TraitementDataGridView.Rows(iGrid).Cells("PosologieDuree").Value <> PosologieDureeComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("posologieDuree").Style.ForeColor = Color.Red
            End If
            PosologieDureeComp = TraitementDataGridView.Rows(iGrid).Cells("PosologieDuree").Value

            'Commentaire posologie
            TraitementDataGridView.Rows(iGrid).Cells("posologieCommentaire").Value = traitementDataTable.Rows(i)("oa_traitement_posologie_commentaire")
            If traitementDataTable.Rows(i)("oa_traitement_posologie_commentaire").ToString <> PosologieCommentaireComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("posologieCommentaire").Style.ForeColor = Color.Red
            End If
            PosologieCommentaireComp = traitementDataTable.Rows(i)("oa_traitement_posologie_commentaire").ToString

            'Fenetre
            If Fenetre = True Then
                TraitementDataGridView.Rows(iGrid).Cells("fenetre").Value = "Oui"
            Else
                TraitementDataGridView.Rows(iGrid).Cells("fenetre").Value = "Non"
            End If
            If Fenetre <> FenetreComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("fenetre").Style.ForeColor = Color.Red
            End If
            FenetreComp = Fenetre

            'Date début fenêtre
            If dateDebutFenetre = Nothing Then
                TraitementDataGridView.Rows(iGrid).Cells("fenetreDateDebut").Value = "Non définie"
            Else
                TraitementDataGridView.Rows(iGrid).Cells("fenetreDateDebut").Value = dateDebutFenetre.ToString("dd.MM.yyyy")
            End If
            If dateDebutFenetre <> DateDebutFenetreComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("fenetreDateDebut").Style.ForeColor = Color.Red
            End If
            DateDebutFenetreComp = dateDebutFenetre

            'Date fin fenêtre
            If datefinFenetre = Nothing Then
                TraitementDataGridView.Rows(iGrid).Cells("fenetreDateFin").Value = "Non définie"
            Else
                TraitementDataGridView.Rows(iGrid).Cells("fenetreDateFin").Value = datefinFenetre.ToString("dd.MM.yyyy")
            End If
            If datefinFenetre <> DateFinFenetreComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("fenetreDateFin").Style.ForeColor = Color.Red
            End If
            DateFinFenetreComp = datefinFenetre

            'Commentaire fenêtre
            TraitementDataGridView.Rows(iGrid).Cells("fenetreCommentaire").Value = traitementDataTable.Rows(i)("oa_traitement_fenetre_commentaire")
            If traitementDataTable.Rows(i)("oa_traitement_fenetre_commentaire").ToString <> FenetreCommentaireComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("fenetreCommentaire").Style.ForeColor = Color.Red
            End If
            FenetreCommentaireComp = traitementDataTable.Rows(i)("oa_traitement_fenetre_commentaire").ToString

            'Arrêt traitement
            If Arret = True Then
                TraitementDataGridView.Rows(iGrid).Cells("arret").Value = "Oui"
            Else
                TraitementDataGridView.Rows(iGrid).Cells("arret").Value = "Non"
            End If
            If Arret <> ArretComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("arret").Style.ForeColor = Color.Red
            End If
            ArretComp = Arret

            'Commentaire arrêt
            TraitementDataGridView.Rows(iGrid).Cells("arretCommentaire").Value = traitementDataTable.Rows(i)("oa_traitement_arret_commentaire")
            If traitementDataTable.Rows(i)("oa_traitement_arret_commentaire").ToString <> ArretCommentaireComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("arretCommentaire").Style.ForeColor = Color.Red
            End If
            ArretCommentaireComp = traitementDataTable.Rows(i)("oa_traitement_arret_commentaire").ToString

            'Allergie
            If allergie = True Then
                TraitementDataGridView.Rows(iGrid).Cells("allergie").Value = "Oui"
            Else
                TraitementDataGridView.Rows(iGrid).Cells("allergie").Value = "Non"
            End If
            If allergie <> AllergieComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("allergie").Style.ForeColor = Color.Red
            End If
            AllergieComp = allergie

            'Contre-indication
            If contreIndication = True Then
                TraitementDataGridView.Rows(iGrid).Cells("contreIndication").Value = "Oui"
            Else
                TraitementDataGridView.Rows(iGrid).Cells("contreIndication").Value = "Non"
            End If
            If contreIndication <> ContreIndicationComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("contreIndication").Style.ForeColor = Color.Red
            End If
            ContreIndicationComp = contreIndication

            'Annulation
            If Annulation = True Then
                TraitementDataGridView.Rows(iGrid).Cells("annulation").Value = "Oui"
            Else
                TraitementDataGridView.Rows(iGrid).Cells("annulation").Value = "Non"
            End If
            If Annulation <> AnnulationComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("annulation").Style.ForeColor = Color.Red
            End If
            AnnulationComp = Annulation

            'Commentaire annulation
            TraitementDataGridView.Rows(iGrid).Cells("annulationCommentaire").Value = traitementDataTable.Rows(i)("oa_traitement_annulation_commentaire")
            If traitementDataTable.Rows(i)("oa_traitement_annulation_commentaire").ToString <> AnnulationCommentaireComp And premierPassage = False Then
                TraitementDataGridView.Rows(iGrid).Cells("annulationCommentaire").Style.ForeColor = Color.Red
            End If
            AnnulationCommentaireComp = traitementDataTable.Rows(i)("oa_traitement_annulation_commentaire").ToString

            premierPassage = False
        Next

        'Positionnement du grid sur la première occurrence
        If TraitementDataGridView.Rows.Count > 0 Then
            Me.TraitementDataGridView.CurrentRow = TraitementDataGridView.ChildRows(0)
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
        'Traitements
        TraitementDataGridView.Rows.Clear()
    End Sub

    Private Sub TraitementDataGridView_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles TraitementDataGridView.ToolTipTextNeeded
        Dim cell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If cell IsNot Nothing AndAlso (cell.ColumnInfo.Name = "commentaire" Or
            cell.ColumnInfo.Name = "posologieCommentaire" Or
            cell.ColumnInfo.Name = "arretCommentaire" Or
            cell.ColumnInfo.Name = "fenetreCommentaire" Or
            cell.ColumnInfo.Name = "annulationCommentaire") Then
            e.ToolTipText = cell.Value.ToString()
        End If
    End Sub
End Class
