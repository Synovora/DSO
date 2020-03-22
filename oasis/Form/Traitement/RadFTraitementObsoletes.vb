Imports System.Configuration
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.Localization
Imports Oasis_Common
Public Class RadFTraitementObsoletes

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

    Dim Horizon As Integer


    Private Sub RadFTraitementObsoletes_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()
        ChargementParametreApplication()
        DteHorizonAffichage.CustomFormat = "MMMM-yyyy"
        DteHorizonAffichage.Value = Date.Now.AddYears(-1 * Horizon)
        initZones()
        ChargementEtatCivil()
        ChargementTraitement()
    End Sub


    Private Sub ChargementParametreApplication()
        'Récupération du nom de l'organisation dans les paramètres de l'application
        Dim HorizonString As String = ConfigurationManager.AppSettings("horizonTraitementObsolete")
        If IsNumeric(HorizonString) Then
            Horizon = CInt(HorizonString)
        Else
            Horizon = 3
        End If
    End Sub

    '==========================================================
    '======================= Etat civil =======================
    '==========================================================

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
    '======================= Traitements obsolètes ============
    '==========================================================

    'Chargement de la Grid
    Private Sub ChargementTraitement()
        Dim filtreDateFin As Date
        filtreDateFin = DteHorizonAffichage.Value

        Dim traitementDataTable As DataTable
        Dim traitementDao As TraitementDao = New TraitementDao
        traitementDataTable = traitementDao.getAllTraitementbyPatient(Me.SelectedPatient.patientId, filtreDateFin)

        'Ajout d'une colonne 'oa_traitement_posologie' dans le DataTable de traitement
        traitementDataTable.Columns.Add("oa_traitement_posologie", Type.GetType("System.String"))

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = traitementDataTable.Rows.Count - 1
        Dim Base As String
        Dim allergie, contreIndication As Boolean
        Dim Posologie As String
        Dim dateFin As Date
        Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
        Dim Rythme As Integer
        Dim Fenetre As Boolean
        Dim DateDebut, dateArret As Date
        Dim Remarque As String
        Dim TraitementArret, TraitementAnnule As Boolean

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            'Initialisation Booléens de travail
            allergie = False
            contreIndication = False
            TraitementArret = False
            TraitementAnnule = False

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
                DateDebut = "01/01/0001"
            End If

            'Identification si le traitement a été arrêté
            If traitementDataTable.Rows(i)("oa_traitement_arret") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_arret") = "A" Then
                    TraitementArret = True
                End If
            End If

            'Identification si le traitement a été annulé
            If traitementDataTable.Rows(i)("oa_traitement_annulation") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_annulation") = "A" Then
                    TraitementAnnule = True
                End If
            End If

            'Ne pas afficher les allergies ou les contre-indications sur déclaration du patient, c'est à dire n'ayant pas fait l'objet d'un traitement
            If traitementDataTable.Rows(i)("oa_traitement_declaratif_hors_traitement") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_declaratif_hors_traitement") = "1" Then
                    Continue For
                End If
            End If

            'Ne pas afficher les traitements qui n'ont pas fait l'objet d'un arrêt et d'une annulation et dont la date de fin est supérieure ou égale à aujourd'hui
            If TraitementArret = False And TraitementAnnule = False Then
                Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
                Dim dateFinaComparer As New Date(dateFin.Year, dateFin.Month, dateFin.Day, 0, 0, 0)
                If (dateFinaComparer >= dateJouraComparer) Then
                    Continue For
                End If
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

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadTraitementDataGridView.Rows.Add(iGrid)

            '------------------- Alimentation du DataGridView
            'DCI
            RadTraitementDataGridView.Rows(iGrid).Cells("dci").Value = traitementDataTable.Rows(i)("oa_traitement_medicament_dci")

            'CIS
            RadTraitementDataGridView.Rows(iGrid).Cells("medicamentCis").Value = traitementDataTable.Rows(i)("oa_traitement_medicament_cis")

            'Posologie
            RadTraitementDataGridView.Rows(iGrid).Cells("posologie").Value = Posologie

            'Durée posologie
            If dateFin = "31/12/2999" Then
                RadTraitementDataGridView.Rows(iGrid).Cells("dureePosologie").Value = ""
            Else
                RadTraitementDataGridView.Rows(iGrid).Cells("dureePosologie").Value = CalculDureeTraitementEnJourString(DateDebut, dateFin)
            End If

            'Date arrêt
            dateArret = traitementDataTable.Rows(i)("oa_traitement_date_fin")
            If dateArret = "31/12/2999" Then
                RadTraitementDataGridView.Rows(iGrid).Cells("dateArret").Value = "Non définie"
            Else
                RadTraitementDataGridView.Rows(iGrid).Cells("dateArret").Value = dateArret.ToString("dd.MM.yyyy")
            End If

            'Commentaire arrêt
            If TraitementArret = True Then
                RadTraitementDataGridView.Rows(iGrid).Cells("commentaireArret").Value = traitementDataTable.Rows(i)("oa_traitement_arret_commentaire")
            Else
                If TraitementAnnule = True Then
                    RadTraitementDataGridView.Rows(iGrid).Cells("commentaireArret").Value = traitementDataTable.Rows(i)("oa_traitement_annulation_commentaire")
                End If
            End If

            'Clé unique du traitement
            RadTraitementDataGridView.Rows(iGrid).Cells("traitementId").Value = traitementDataTable.Rows(i)("oa_traitement_id")

            'Remarque
            Remarque = ""

            If TraitementArret = True Then
                Remarque = "Arrêt du traitement"
            End If

            If TraitementAnnule = True Then
                Remarque = "Traitement annulé"
            End If

            If allergie = True Then
                If Remarque = "" Then
                    Remarque = "(Allergie)"
                Else
                    Remarque = Remarque + " (Allergie)"
                End If
            End If
            If contreIndication = True Then
                If Remarque = "" Then
                    Remarque = "(Contre-indication)"
                Else
                    Remarque = Remarque + " (Contre-indication)"
                End If
            End If

            RadTraitementDataGridView.Rows(iGrid).Cells("remarque").Value = Remarque
        Next

        'Positionnement du grid sur la première occurrence
        If RadTraitementDataGridView.Rows.Count > 0 Then
            Me.RadTraitementDataGridView.CurrentRow = RadTraitementDataGridView.ChildRows(0)
        End If

        'Enlève le focus sur la première ligne du Grid
        RadTraitementDataGridView.ClearSelection()
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
        RadTraitementDataGridView.Rows.Clear()
    End Sub

    Private Sub TraitementDataGridView_DoubleClick(sender As Object, e As EventArgs) Handles RadTraitementDataGridView.DoubleClick
        If RadTraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadTraitementDataGridView.Rows.IndexOf(Me.RadTraitementDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim TraitementId As Integer = RadTraitementDataGridView.Rows(aRow).Cells("TraitementId").Value
                Using vFTraitementDetailEdit As New RadFTraitementDetailEdit
                    vFTraitementDetailEdit.SelectedTraitementId = TraitementId
                    vFTraitementDetailEdit.SelectedPatient = Me.SelectedPatient
                    vFTraitementDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFTraitementDetailEdit.SelectedMedicamentId = RadTraitementDataGridView.Rows(aRow).Cells("medicamentCis").Value
                    vFTraitementDetailEdit.ShowDialog() 'Modal
                End Using
            End If
        End If
    End Sub

    Private Sub DteHorizonAffichage_ValueChanged(sender As Object, e As EventArgs) Handles DteHorizonAffichage.ValueChanged
        'Rechargement grid
        RadTraitementDataGridView.Rows.Clear()
        ChargementTraitement()
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub HistoriqueDesModificationsToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles HistoriqueDesModificationsToolStripMenuItem.Click
        If RadTraitementDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadTraitementDataGridView.Rows.IndexOf(Me.RadTraitementDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim TraitementId As Integer = RadTraitementDataGridView.Rows(aRow).Cells("traitementId").Value
                Using vFTraitementHistoListe As New RadFTraitementHistoListe
                    vFTraitementHistoListe.SelectedTraitementId = TraitementId
                    vFTraitementHistoListe.SelectedPatient = Me.SelectedPatient
                    vFTraitementHistoListe.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFTraitementHistoListe.MedicamentDenomination = RadTraitementDataGridView.Rows(aRow).Cells("dci").Value
                    vFTraitementHistoListe.ShowDialog() 'Modal
                End Using
            End If
        End If
    End Sub

    Private Sub RadTraitementDataGridView_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RadTraitementDataGridView.ToolTipTextNeeded
        Dim cell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        'If cell IsNot Nothing AndAlso (cell.ColumnInfo.Name = "commentaire" Or
        'cell.ColumnInfo.Name = "dci" Or
        'cell.ColumnInfo.Name = "commentaireArret" Or
        'cell.ColumnInfo.Name = "remarque") Then
        'e.ToolTipText = cell.Value.ToString()
        'End If
    End Sub
End Class
