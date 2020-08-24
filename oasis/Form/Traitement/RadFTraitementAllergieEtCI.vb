Imports Oasis_Common

Public Class RadFTraitementAllergieEtCI
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateAllergieOuContreIndication As EnumAllergieOuContreIndication

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

    Public Property AllergieOuContreIndication As Integer
        Get
            Return privateAllergieOuContreIndication
        End Get
        Set(value As Integer)
            privateAllergieOuContreIndication = value
        End Set
    End Property

    Dim traitementDao As TraitementDao = New TraitementDao

    Private Sub RadFTraitementAllergieEtCI_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initZones()
        ChargementPatient()
        ChargementTraitement()
    End Sub

    Private Sub ChargementPatient()
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
    End Sub

    'Chargement de la Grid
    Private Sub ChargementTraitement()
        Dim traitementDataTable As DataTable

        RadTraitementDataGridView.Rows.Clear()

        'Dim traitementDataAdapter As SqlDataAdapter = New SqlDataAdapter()


        'Dim conxn As New SqlConnection(outils.getConnectionString())

        Select Case Me.AllergieOuContreIndication
            Case EnumAllergieOuContreIndication.Allergie
                traitementDataTable = traitementDao.GetAllTraitementAllergiebyPatient(SelectedPatient.patientId)
                afficheTitleForm(Me, "Liste des allergies du patient")
            Case EnumAllergieOuContreIndication.ContreIndication
                traitementDataTable = traitementDao.getAllTraitementCIbyPatient(SelectedPatient.patientId)
                afficheTitleForm(Me, "Liste des contre-indications")
            Case Else
                Close()
                Return
        End Select

        'Ajout d'une colonne 'oa_traitement_posologie' dans le DataTable de traitement
        traitementDataTable.Columns.Add("oa_traitement_posologie", Type.GetType("System.String"))

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = traitementDataTable.Rows.Count - 1
        Dim allergie, contreIndication As Boolean
        Dim dateFin As Date
        Dim DateDebut As Date
        Dim remarque As String
        Dim traitementArret As Boolean = False


        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            'Initialisation de la date de fin de traitement
            If traitementDataTable.Rows(i)("oa_traitement_date_fin") IsNot DBNull.Value Then
                dateFin = traitementDataTable.Rows(i)("oa_traitement_date_fin")
            Else
                dateFin = "31/12/2999"
            End If

            'Initialisation de la date de début de traitement
            If traitementDataTable.Rows(i)("oa_traitement_date_debut") IsNot DBNull.Value Then
                DateDebut = traitementDataTable.Rows(i)("oa_traitement_date_debut")
            Else
                DateDebut = Nothing
            End If

            'Identification si le traitement a été arrêté
            If traitementDataTable.Rows(i)("oa_traitement_arret") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_arret") = "A" Then
                    traitementArret = True
                End If
            End If

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadTraitementDataGridView.Rows.Add(iGrid)

            '------------------- Alimentation du DataGridView
            'DCI
            RadTraitementDataGridView.Rows(iGrid).Cells("dci").Value = traitementDataTable.Rows(i)("oa_traitement_medicament_dci")


            'Date arrêt
            RadTraitementDataGridView.Rows(iGrid).Cells("dateArret").Value = traitementDataTable.Rows(i)("oa_traitement_date_fin")

            'Commentaire arrêt
            RadTraitementDataGridView.Rows(iGrid).Cells("commentaireArret").Value = traitementDataTable.Rows(i)("oa_traitement_arret_commentaire")

            'Clé unique du traitement
            RadTraitementDataGridView.Rows(iGrid).Cells("traitementId").Value = traitementDataTable.Rows(i)("oa_traitement_id")

            'Remarque
            remarque = ""
            If allergie = True Then
                remarque = "Allergie"
            End If
            If contreIndication = True Then
                remarque = "Contre-indication"
            End If
            If traitementArret = True Then
                If remarque = "" Then
                    remarque = "Traitement arrêté"
                Else
                    remarque = remarque + " - Traitement arrêté"
                End If
            End If
            RadTraitementDataGridView.Rows(iGrid).Cells("remarque").Value = remarque
        Next

        'Positionnement du grid sur la première occurrence
        If RadTraitementDataGridView.Rows.Count > 0 Then
            Me.RadTraitementDataGridView.CurrentRow = RadTraitementDataGridView.ChildRows(0)
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
    End Sub


    Private Sub RadTraitementDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadTraitementDataGridView.CellDoubleClick
        If RadTraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadTraitementDataGridView.Rows.IndexOf(Me.RadTraitementDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim TraitementId As Integer
                TraitementId = RadTraitementDataGridView.Rows(aRow).Cells("TraitementId").Value
                Using vFTraitementDetailEdit As New RadFTraitementDetailEdit
                    vFTraitementDetailEdit.SelectedTraitementId = TraitementId
                    vFTraitementDetailEdit.SelectedPatient = Me.SelectedPatient
                    'vFTraitementDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFTraitementDetailEdit.ShowDialog()
                End Using
            End If
        End If
    End Sub

    Private Sub RadBtnAnnuler_Click(sender As Object, e As EventArgs) Handles RadBtnAnnuler.Click
        If RadTraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadTraitementDataGridView.Rows.IndexOf(Me.RadTraitementDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim TraitementId As Integer
                TraitementId = RadTraitementDataGridView.Rows(aRow).Cells("TraitementId").Value
                Dim traitement As Traitement
                traitement = traitementDao.getTraitementById(TraitementId)
                Using form As New RadFAllergieEtCISuppressionDetail
                    form.SelectedTraitement = traitement
                    form.SelectedPatient = Me.SelectedPatient
                    form.ShowDialog()
                    If form.CodeRetour = True Then
                        ChargementTraitement()
                    End If
                End Using
            End If
        End If
    End Sub

    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Close()
    End Sub

End Class
