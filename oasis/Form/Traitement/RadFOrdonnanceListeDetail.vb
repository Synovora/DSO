Imports System.Collections.Specialized
Imports Oasis_WF

Public Class RadFOrdonnanceListeDetail
    Private _SelectedPatient As Patient
    Private _UtilisateurConnecte As Utilisateur
    Private _SelectedOrdonnanceId As Integer
    Private _commentaireOrdonnance As String
    Private _Allergie As Boolean
    Private _ContreIndication As Boolean
    Private _CodeRetour As Boolean

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

    Public Property SelectedOrdonnanceId As Integer
        Get
            Return _SelectedOrdonnanceId
        End Get
        Set(value As Integer)
            _SelectedOrdonnanceId = value
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

    Public Property Allergie As Boolean
        Get
            Return _Allergie
        End Get
        Set(value As Boolean)
            _Allergie = value
        End Set
    End Property

    Public Property ContreIndication As Boolean
        Get
            Return _ContreIndication
        End Get
        Set(value As Boolean)
            _ContreIndication = value
        End Set
    End Property

    Public Property CommentaireOrdonnance As String
        Get
            Return _commentaireOrdonnance
        End Get
        Set(value As String)
            _commentaireOrdonnance = value
        End Set
    End Property

    Dim CommentaireModified As Boolean = False

    Private Sub RadFOrdonnanceDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementEtatCivil()
        ChargementTraitement()

    End Sub

    Private Sub ChargementTraitement()
        Dim ordonnanceDataTable As DataTable
        Dim ordonnanceDaoDetail As OrdonnanceDetailDao = New OrdonnanceDetailDao
        ordonnanceDataTable = ordonnanceDaoDetail.getAllOrdonnanceLigneByOrdonnanceId(Me.SelectedOrdonnanceId)

        TxtCommentaire.Text = CommentaireOrdonnance

        If ordonnanceDataTable.Rows.Count > 0 Then
            RadBtnCreationLignes.Hide()
            RadBtnValidation.Show()
            RadBtnImprimer.Show()
        Else
            RadBtnCreationLignes.Show()
            RadBtnValidation.Hide()
            RadBtnImprimer.Hide()
        End If

        'Ajout d'une colonne 'oa_traitement_posologie' dans le DataTable de traitement
        ordonnanceDataTable.Columns.Add("oa_traitement_posologie", Type.GetType("System.String"))

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = ordonnanceDataTable.Rows.Count - 1
        Dim Base As String
        Dim Posologie As String
        Dim dateFin, dateDebut, dateModification As Date
        Dim jours As Integer
        Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
        Dim Rythme As Integer
        Dim FenetreTherapeutiqueEnCours As Boolean
        Dim FenetreTherapeutiqueAVenir As Boolean

        'Dim Allergie As Boolean = False
        Dim FenetreDateDebut, FenetreDateFin As Date

        Allergie = False

        ContreIndication = False
        LblAllergie.Visible = False
        lblContreIndication.Visible = False
        SelectedPatient.PatientAllergieCis.Clear()
        SelectedPatient.PatientAllergieDci.Clear()
        SelectedPatient.PatientContreIndicationCis.Clear()
        SelectedPatient.PatientContreIndicationDci.Clear()
        SelectedPatient.PatientMedicamentsPrescritsCis.Clear()

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            'Date de fin
            If ordonnanceDataTable.Rows(i)("oa_traitement_date_fin") IsNot DBNull.Value Then
                dateFin = ordonnanceDataTable.Rows(i)("oa_traitement_date_fin")
            Else
                dateFin = "31/12/2999"
            End If

            'Date début
            If ordonnanceDataTable.Rows(i)("oa_traitement_date_debut") IsNot DBNull.Value Then
                dateDebut = ordonnanceDataTable.Rows(i)("oa_traitement_date_debut")
            Else
                dateDebut = "01/01/1900"
            End If

            'Exclusion de l'affichage des traitements dont la date de fin est <à la date du jour
            'Cette condition est traitée en exclusion (et non dans la requête SQL) pour stocker les allergies et les contre-indications dans la StringCollection quel que soit leur date de fin
            Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
            Dim dateFinaComparer As New Date(dateFin.Year, dateFin.Month, dateFin.Day, 0, 0, 0)
            If (dateFinaComparer < dateJouraComparer) Then
                'Continue For
            End If

            'Vérification de l'existence d'une fenêtre thérapeutique active et à venir
            FenetreTherapeutiqueEnCours = False
            FenetreTherapeutiqueAVenir = False

            If ordonnanceDataTable.Rows(i)("oa_traitement_fenetre_date_debut") IsNot DBNull.Value Then
                FenetreDateDebut = ordonnanceDataTable.Rows(i)("oa_traitement_fenetre_date_debut")
            Else
                FenetreDateDebut = "31/12/2999"
            End If

            If ordonnanceDataTable.Rows(i)("oa_traitement_fenetre_date_fin") IsNot DBNull.Value Then
                FenetreDateFin = ordonnanceDataTable.Rows(i)("oa_traitement_fenetre_date_fin")
            Else
                FenetreDateFin = "01/01/1900"
            End If

            Posologie = ""

            'Existence d'une fenêtre thérapeutique
            Dim FenetreTherapeutiqueExiste As Boolean = False
            Dim dateDebutFenetreaComparer As New Date(FenetreDateDebut.Year, FenetreDateDebut.Month, FenetreDateDebut.Day, 0, 0, 0)
            Dim dateFinFenetreaComparer As New Date(FenetreDateFin.Year, FenetreDateFin.Month, FenetreDateFin.Day, 0, 0, 0)
            If ordonnanceDataTable.Rows(i)("oa_traitement_fenetre") IsNot DBNull.Value Then
                If ordonnanceDataTable.Rows(i)("oa_traitement_fenetre") = "1" Then
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
                If ordonnanceDataTable.Rows(i)("oa_traitement_posologie_base") IsNot DBNull.Value Then
                    Rythme = ordonnanceDataTable.Rows(i)("oa_traitement_posologie_rythme")
                    Select Case ordonnanceDataTable.Rows(i)("oa_traitement_posologie_base")
                        Case "J"
                            Base = "Journalier : "
                            If ordonnanceDataTable.Rows(i)("oa_traitement_posologie_matin") <> 0 Then
                                posologieMatin = ordonnanceDataTable.Rows(i)("oa_traitement_posologie_matin")
                            Else
                                posologieMatin = 0
                            End If
                            If ordonnanceDataTable.Rows(i)("oa_traitement_posologie_midi") <> 0 Then
                                posologieMidi = ordonnanceDataTable.Rows(i)("oa_traitement_posologie_midi")
                            Else
                                posologieMidi = 0
                            End If
                            If ordonnanceDataTable.Rows(i)("oa_traitement_posologie_soir") <> 0 Then
                                posologieSoir = ordonnanceDataTable.Rows(i)("oa_traitement_posologie_soir")
                            Else
                                posologieSoir = 0
                            End If
                            If ordonnanceDataTable.Rows(i)("oa_traitement_posologie_apres_midi") <> 0 Then
                                posologieApresMidi = ordonnanceDataTable.Rows(i)("oa_traitement_posologie_apres_midi")
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

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadTraitementDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            'DCI
            RadTraitementDataGridView.Rows(iGrid).Cells("medicamentDci").Value = ordonnanceDataTable.Rows(i)("oa_traitement_medicament_dci")
            'Posologie
            RadTraitementDataGridView.Rows(iGrid).Cells("posologie").Value = Posologie

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
                jours = (Date.Now - dateDebut).TotalDays
                If jours > 30 Then
                    RadTraitementDataGridView.Rows(iGrid).Cells("dateDebut").Value = dateDebut.ToString("MM.yyyy")
                Else
                    RadTraitementDataGridView.Rows(iGrid).Cells("dateDebut").Value = dateDebut.ToString("dd.MM.yyyy")
                End If
            End If

            'Traitement du format d'affichage de modification du traitement
            If dateModification = "01/01/1900" Then
                RadTraitementDataGridView.Rows(iGrid).Cells("dateModification").Value = "Date non définie"
            Else
                jours = (Date.Now - dateModification).TotalDays
                If jours > 30 Then
                    RadTraitementDataGridView.Rows(iGrid).Cells("dateModification").Value = dateModification.ToString("MM.yyyy")
                Else
                    RadTraitementDataGridView.Rows(iGrid).Cells("dateModification").Value = dateModification.ToString("dd.MM.yyyy")
                End If
            End If

            'Identifiant du traitement
            RadTraitementDataGridView.Rows(iGrid).Cells("traitementId").Value = ordonnanceDataTable.Rows(i)("oa_traitement_id")

            RadTraitementDataGridView.Rows(iGrid).Cells("ordonnanceLigneId").Value = ordonnanceDataTable.Rows(i)("oa_ordonnance_ligne_id")

            'CIS du médicament
            RadTraitementDataGridView.Rows(iGrid).Cells("medicamentCis").Value = ordonnanceDataTable.Rows(i)("oa_traitement_medicament_cis")

            'Bouton gérer fenêtre thérapeutique
            If FenetreTherapeutiqueAVenir = True Or FenetreTherapeutiqueEnCours = True Then
                RadTraitementDataGridView.Rows(iGrid).Cells("posologie").Style.ForeColor = Color.Red
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadTraitementDataGridView.Rows.Count > 0 Then
            Me.RadTraitementDataGridView.CurrentRow = RadTraitementDataGridView.ChildRows(0)
        End If
    End Sub

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

        ChargementAllergie()
    End Sub

    Private Sub ChargementAllergie()
        'Allergie
        If Allergie = True Then
            LblAllergie.Show()
        Else
            LblAllergie.Hide()
        End If

        'Contre-indication
        If ContreIndication = True Then
            lblContreIndication.Show()
        Else
            lblContreIndication.Hide()
        End If

        'Si allergie, affichage des substances allergiques
        If Allergie = True Then
            LblAllergie.Visible = True
            Dim premierPassage As Boolean = True
            Dim LongueurChaine, LongueurSub As Integer
            Dim AllergieTooltip As String
            Dim LongueurMax As Integer = 10

            'Chargement du TextBox
            Dim allergieString As String
            Dim SubstancesAllergiques As New StringCollection()
            SubstancesAllergiques = MedocDao.ListeSubstancesAllergiques(SelectedPatient.PatientAllergieCis)
            Dim allergieEnumerator As StringEnumerator = SubstancesAllergiques.GetEnumerator()
            While allergieEnumerator.MoveNext()
                If premierPassage = True Then
                    allergieString = allergieEnumerator.Current.ToString
                    LongueurChaine = allergieString.Length
                    If LongueurChaine < LongueurMax Then
                        LongueurSub = LongueurChaine - 1
                    Else
                        LongueurSub = LongueurMax
                    End If
                    AllergieTooltip = allergieString
                    premierPassage = False
                Else
                    allergieString = allergieEnumerator.Current.ToString
                    LongueurChaine = allergieString.Length
                    If LongueurChaine < LongueurMax Then
                        LongueurSub = LongueurChaine - 1
                    Else
                        LongueurSub = LongueurMax
                    End If
                    AllergieTooltip = AllergieTooltip + vbCrLf + allergieString
                End If
            End While
            ToolTip.SetToolTip(LblAllergie, AllergieTooltip)
            'Chargement des médicaments génériques associés aux médicaments allergiques déclarés
            TraitementAllergies(Me.SelectedPatient)
        End If

        If ContreIndication = True Then
            lblContreIndication.Show()
            'Chargement des médicaments génériques associés aux médicaments contre-indiqués déclarés
            Dim premierPassage As Boolean = True
            Dim LongueurChaine, LongueurSub As Integer
            Dim CITooltip As String
            Dim LongueurMax As Integer = 10

            'Chargement du TextBox
            Dim CIString As String
            Dim SubstancesCI As New StringCollection()
            SubstancesCI = MedocDao.ListeSubstancesCI(SelectedPatient.PatientContreIndicationCis)
            Dim CIEnumerator As StringEnumerator = SubstancesCI.GetEnumerator()
            While CIEnumerator.MoveNext()
                If premierPassage = True Then
                    CIString = CIEnumerator.Current.ToString
                    LongueurChaine = CIString.Length
                    If LongueurChaine < LongueurMax Then
                        LongueurSub = LongueurChaine - 1
                    Else
                        LongueurSub = LongueurMax
                    End If
                    CITooltip = CIString
                    premierPassage = False
                Else
                    CIString = CIEnumerator.Current.ToString
                    LongueurChaine = CIString.Length
                    If LongueurChaine < LongueurMax Then
                        LongueurSub = LongueurChaine - 1
                    Else
                        LongueurSub = LongueurMax
                    End If
                    CITooltip = CITooltip + vbCrLf + CIString
                End If
            End While
            ToolTip.SetToolTip(lblContreIndication, CITooltip)
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub LblAllergie_Click(sender As Object, e As EventArgs) Handles LblAllergie.Click
        'Traitement : afficher les allergies dans un popup
        If Allergie = True Then
            Using vFPatientAllergieListe As New RadFPatientAllergieListe
                vFPatientAllergieListe.SelectedPatient = Me.SelectedPatient
                vFPatientAllergieListe.SelectedPatientId = Me.SelectedPatient.patientId
                vFPatientAllergieListe.SelectedPatientAllergieCis = Me.SelectedPatient.PatientAllergieCis
                vFPatientAllergieListe.UtilisateurConnecte = Me.UtilisateurConnecte
                vFPatientAllergieListe.ShowDialog() 'Modal
            End Using
        End If
    End Sub

    Private Sub lblContreIndication_Click(sender As Object, e As EventArgs) Handles lblContreIndication.Click
        Using vFPatientContreIndicationListe As New RadFPatientContreIndicationListe
            vFPatientContreIndicationListe.SelectedPatient = Me.SelectedPatient
            vFPatientContreIndicationListe.SelectedPatientId = Me.SelectedPatient.patientId
            vFPatientContreIndicationListe.SelectedPatientCICis = Me.SelectedPatient.PatientContreIndicationCis
            vFPatientContreIndicationListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientContreIndicationListe.ShowDialog() 'Modal
        End Using
    End Sub

    Private Sub RadBtnCreationLignes_Click(sender As Object, e As EventArgs) Handles RadBtnCreationLignes.Click
        Dim ordonnanceDao As New OrdonnanceDao
        ordonnanceDao.CreateNewOrdonnanceDetail(SelectedPatient.patientId, SelectedOrdonnanceId)
        RadTraitementDataGridView.Rows.Clear()
        ChargementTraitement()
        RadBtnValidation.Show()
        RadBtnImprimer.Show()
        RadBtnCreationLignes.Hide()
    End Sub

    'Création d'une ligne de commentaire
    Private Sub CréerUneLigneDeCommentaireToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneLigneDeCommentaireToolStripMenuItem.Click

    End Sub

    'Suppression (traitement inhibé) d'une ligne d'ordonnance
    Private Sub SupprimerUneLigneToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupprimerUneLigneToolStripMenuItem.Click
        If RadTraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadTraitementDataGridView.Rows.IndexOf(Me.RadTraitementDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim OrdonnanceId As Integer = RadTraitementDataGridView.Rows(aRow).Cells("ordonnanceId").Value
                'Tester si l'ordonnance sélectionnée est à valider

            End If
        Else
            MessageBox.Show("Veuillez sélectionner une ligne d'ordonnance")
        End If
    End Sub

    'Modification d'une ligne d'ordonnance
    Private Sub MasterTemplate_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadTraitementDataGridView.CellDoubleClick
        If RadTraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadTraitementDataGridView.Rows.IndexOf(Me.RadTraitementDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim OrdonnanceId As Integer = RadTraitementDataGridView.Rows(aRow).Cells("ordonnanceLigneId").Value
                'Tester si l'ordonnance sélectionnée est à valider

            End If
        Else
            MessageBox.Show("Veuillez sélectionner une ligne d'ordonnance")
        End If
    End Sub

    Private Sub TxtCommentaire_TextChanged(sender As Object, e As EventArgs) Handles TxtCommentaire.TextChanged
        CommentaireModified = True
    End Sub

    Private Sub TxtCommentaire_MouseLeave(sender As Object, e As EventArgs) Handles TxtCommentaire.MouseLeave
        If CommentaireModified = True Then
            'Appel mise à jour de l'ordonnance
            Dim ordonnanceDao As New OrdonnanceDao
            ordonnanceDao.ModificationOrdonnanceCommentaire(SelectedOrdonnanceId, TxtCommentaire.Text)
            CommentaireModified = False
        End If
    End Sub
End Class
