'Gestion des traitements d'un patient

Imports System.Data.SqlClient
Imports Oasis_Common

Public Class FTraitementGestion
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur

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
    Private Sub FTraitementGestion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initZones()
        ChargementEtatCivil()
        ChargementTraitement()
    End Sub

    '==========================================================
    '======================= Etat civil =======================
    '==========================================================

    'Chargement des données dans les labels dédiés
    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = Me.SelectedPatient.PatientNir.ToString

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

    End Sub

    '==========================================================
    '======================= Traitement =======================
    '==========================================================

    'Chargement de la Grid
    Private Sub ChargementTraitement()
        Dim traitementDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim traitementDataTable As DataTable = New DataTable()

        Dim conxn As New SqlConnection(outils.getConnectionString())

        Dim SQLString As String
        SQLString = "select oa_traitement_id, oa_traitement_medicament_dci, oa_traitement_posologie_base, oa_traitement_posologie_rythme, oa_traitement_posologie_matin, oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir, oa_traitement_posologie_commentaire, oa_traitement_ordre_affichage, oa_traitement_date_debut, oa_traitement_date_fin, oa_traitement_fenetre, oa_traitement_fenetre_date_debut, oa_traitement_fenetre_date_fin, oa_traitement_allergie, oa_traitement_contre_indication, oa_traitement_commentaire from oasis.oa_traitement where (oa_traitement_annulation is Null or oa_traitement_annulation = '') and oa_traitement_date_fin >= CURDATE() and oa_traitement_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_traitement_ordre_affichage;"

        traitementDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        traitementDataAdapter.Fill(traitementDataTable)

        'Ajout d'une colonne 'oa_traitement_posologie' dans le DataTable de traitement
        traitementDataTable.Columns.Add("oa_traitement_posologie", Type.GetType("System.String"))

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = traitementDataTable.Rows.Count - 1
        Dim Base As String
        Dim Posologie As String
        Dim dateFin As Date
        Dim jours As Integer
        Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
        Dim Rythme As Integer
        Dim Fenetre As Boolean
        Dim FenetreDateDebut, FenetreDateFin As Date

        'Allergie = False
        'LblAllergie.Visible = False

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            If traitementDataTable.Rows(i)("oa_traitement_allergie") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_allergie") = "1" Then
                    Continue For
                End If
            End If

            If traitementDataTable.Rows(i)("oa_traitement_contre_indication") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_contre_indication") = "1" Then
                    Continue For
                End If
            End If

            If traitementDataTable.Rows(i)("oa_traitement_date_fin") IsNot DBNull.Value Then
                dateFin = traitementDataTable.Rows(i)("oa_traitement_date_fin")
            Else
                dateFin = "31/12/2999"
            End If

            'Vérification de l'existence d'une fenêtre thérapeutique
            Fenetre = False
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

            'Formatage de la posologie
            Posologie = ""
            If traitementDataTable.Rows(i)("oa_traitement_fenetre") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_fenetre") = "1" Then
                    If FenetreDateDebut <= Date.Now And FenetreDateFin > Date.Now Then
                        Posologie = "Fenêtre Th."
                        Fenetre = True
                    End If
                End If
            End If

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

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            TraitementDataGridView.Rows.Insert(iGrid)

            '------------------- Alimentation du DataGridView
            'DCI
            'TraitementDataGridView.Item(1, iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_medicament_dci")
            TraitementDataGridView("oa_traitement_medicament_dci", iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_medicament_dci")

            'Posologie
            TraitementDataGridView.Item(2, iGrid).Value = Posologie
            If Posologie = "Fenêtre Th." Then
                'TraitementDataGridView.Item(1, iGrid).Style.Font = New Font(Control.DefaultFont, FontStyle.Bold)
                TraitementDataGridView.Item(2, iGrid).Style.ForeColor = Color.Red
            End If

            'Commentaire posologie
            TraitementDataGridView.Item(3, iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_posologie_commentaire")

            'Traitement du format d'affichage de la fin du traitement
            Dim DateInfinie As New Date(2999, 12, 31, 0, 0, 0)
            If dateFin = DateInfinie Then
                TraitementDataGridView.Item(4, iGrid).Value = "Pas de fin de traitement"
            Else
                jours = (dateFin - Date.Now).TotalDays
                If jours > 30 Then
                    TraitementDataGridView.Item(4, iGrid).Value = dateFin.ToString("MM.yyyy")
                Else
                    TraitementDataGridView.Item(4, iGrid).Value = dateFin.ToString("dd.MM.yyyy")
                End If
            End If

            'Clé unique du traitement
            TraitementDataGridView.Item(0, iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_id")

            'Commentaire traitement
            TraitementDataGridView.Item(5, iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_commentaire")
        Next

        conxn.Close()
        traitementDataAdapter.Dispose()

        'Enlève le focus sur la première ligne de la Grid
        TraitementDataGridView.ClearSelection()
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

    Private Sub TraitementDataGridView_DoubleClick(sender As Object, e As EventArgs) Handles TraitementDataGridView.DoubleClick
        Dim TraitementId As Integer
        TraitementId = TraitementDataGridView.Rows(TraitementDataGridView.CurrentRow.Index).Cells("TraitementId").Value

        Dim vFTraitementDetailEdit As New FTraitementDetailEdit
        'vFTraitementDetailEdit.traitement_id = TraitementId

        vFTraitementDetailEdit.ShowDialog() 'Modal
        vFTraitementDetailEdit.Dispose()

        TraitementDataGridView.Rows.Clear()
        ChargementTraitement()
    End Sub

    Private Sub BtnCreation_Click(sender As Object, e As EventArgs) Handles BtnCreation.Click
        'session_oa_patient_traitement_id = 0
        'session_oa_patient_medicament_selecteur_cis = 0

        Dim vFMedocSelecteur As New FMedocSelecteur
        vFMedocSelecteur.ShowDialog() 'Modal
        vFMedocSelecteur.Dispose()
        'If session_oa_patient_medicament_selecteur_cis <> 0 Then
        'Dim vFTraitementDetailEdit As New FTraitementDetailEdit
        'vFTraitementDetailEdit.ShowDialog() 'Modal
        'vFTraitementDetailEdit.Dispose()
        'session_oa_patient_traitement_id = 0
        'TraitementDataGridView.Rows.Clear()
        'ChargementTraitement()
        'End If
    End Sub
End Class