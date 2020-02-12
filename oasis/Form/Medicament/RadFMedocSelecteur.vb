Imports System.Collections.Specialized
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.Localization
Imports Oasis_Common
Public Class RadFMedocSelecteur

    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedMedicamentCis As Integer
    Private privateAllergie As Boolean
    Private privateContreIndication As Boolean

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

    Public Property SelectedMedicamentCis As Integer
        Get
            Return privateSelectedMedicamentCis
        End Get
        Set(value As Integer)
            privateSelectedMedicamentCis = value
        End Set
    End Property

    Public Property Allergie As Boolean
        Get
            Return privateAllergie
        End Get
        Set(value As Boolean)
            privateAllergie = value
        End Set
    End Property

    Public Property ContreIndication As Boolean
        Get
            Return privateContreIndication
        End Get
        Set(value As Boolean)
            privateContreIndication = value
        End Set
    End Property

    Dim FiltreDenomination As String

    Private Sub RadFMedocSelecteur_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()

        TxtFiltreDenomination.Focus()
        TxtFiltreDenomination.Select()

        ChargementEtatCivil()
        InitAffichageLabel()

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

        LblAffichage.Text = "Veuillez saisir au moins 3 caractères dans le filte d'affichage (" & LblLabelFiltreAffichage.Text & ")"
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

    Private Sub InitAffichageLabel()
        LblMedicamentCis.Text = ""
        LblMedicamentDci.Text = ""
        LblMedicamentForme.Text = ""
        'LblMedicamentAlerte.Text = ""
        RadPnlSelectedMedicament.Hide()
        'LblMedicamentAlerte.Visible = False
        RadBtnSelect.Hide()
    End Sub

    Private Sub RadGridView1_CellClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadMedicamentGridView.CellClick
        Selection()
    End Sub

    Private Sub Selection()
        Dim aRow, maxRow As Integer

        aRow = Me.RadMedicamentGridView.Rows.IndexOf(Me.RadMedicamentGridView.CurrentRow)
        maxRow = RadMedicamentGridView.Rows.Count - 1

        If aRow <= maxRow And aRow > -1 Then
            Dim medicamentAllergique As Boolean = False
            Dim medicamentContreIndication As Boolean = False
            Dim medicamentDejaPrescrit As Boolean = False

            InitAffichageLabel()
            LblMedicamentCis.Text = RadMedicamentGridView.Rows(aRow).Cells("oa_medicament_cis").Value
            LblMedicamentDci.Text = RadMedicamentGridView.Rows(aRow).Cells("oa_medicament_dci").Value
            LblMedicamentForme.Text = RadMedicamentGridView.Rows(aRow).Cells("oa_medicament_forme").Value

            If LblMedicamentCis.Text <> "" Then
                'Contrôle que le médicament n'est pas déjà prescrit
                Dim medicamentsPrescritsEnumerator As StringEnumerator = SelectedPatient.PatientMedicamentsPrescritsCis.GetEnumerator()
                While medicamentsPrescritsEnumerator.MoveNext()
                    If LblMedicamentCis.Text = medicamentsPrescritsEnumerator.Current.ToString Then
                        medicamentDejaPrescrit = True
                        Exit While
                    End If
                End While

                If medicamentDejaPrescrit = True Then
                    'LblMedicamentAlerte.Visible = True
                    'LblMedicamentAlerte.Text = "Attention, cette dénomination fait déjà l'objet d'un traitement en cours pour ce patient"
                End If

                RadBtnSelect.Show()
                RadPnlSelectedMedicament.Show()
            End If
        End If
    End Sub

    Private Sub RadBtnSelect_Click(sender As Object, e As EventArgs) Handles RadBtnSelect.Click
        SelectionRetour()
    End Sub

    Private Sub SelectionRetour()
        Dim medicamentCIS As Integer
        medicamentCIS = CInt(LblMedicamentCis.Text)
        Me.SelectedMedicamentCis = medicamentCIS
        Me.Close()
    End Sub

    Private Sub RadMedicamentGridView_DoubleClick(sender As Object, e As EventArgs) Handles RadMedicamentGridView.DoubleClick
        Selection()
        SelectionRetour()
    End Sub

    Private Sub DétailMédicamentToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles DétailMédicamentToolStripMenuItem.Click
        DetailMedicament()
    End Sub

    Private Sub RadBtnDetailMedoc_Click(sender As Object, e As EventArgs) Handles RadBtnDetailMedoc.Click
        DetailMedicament()
    End Sub

    'Détail médicament
    Private Sub DetailMedicament()
        If RadMedicamentGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadMedicamentGridView.Rows.IndexOf(Me.RadMedicamentGridView.CurrentRow)
            If aRow >= 0 Then
                Dim TraitementMedicamentCIS As Integer = RadMedicamentGridView.Rows(aRow).Cells("oa_medicament_cis").Value
                Using vFMedocDetail As New RadFMedocDetail
                    vFMedocDetail.MedicamentCis = TraitementMedicamentCIS
                    vFMedocDetail.ShowDialog() 'Modal
                End Using
            End If
        End If
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

    Private Sub LblContreIndication_Click(sender As Object, e As EventArgs) Handles lblContreIndication.Click
        Using vFPatientContreIndicationListe As New RadFPatientContreIndicationListe
            vFPatientContreIndicationListe.SelectedPatient = Me.SelectedPatient
            vFPatientContreIndicationListe.SelectedPatientId = Me.SelectedPatient.patientId
            vFPatientContreIndicationListe.SelectedPatientCICis = Me.SelectedPatient.PatientContreIndicationCis
            vFPatientContreIndicationListe.UtilisateurConnecte = Me.UtilisateurConnecte
            vFPatientContreIndicationListe.ShowDialog() 'Modal
        End Using
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub MasterTemplate_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RadMedicamentGridView.ToolTipTextNeeded
        Dim cell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        'If cell IsNot Nothing AndAlso (cell.ColumnInfo.Name = "oa_medicament_dci" Or
        'cell.ColumnInfo.Name = "oa_medicament_forme" Or
        'cell.ColumnInfo.Name = "oa_medicament_titulaire" Or
        'cell.ColumnInfo.Name = "oa_medicament_voie_administration") Then
        'e.ToolTipText = cell.Value.ToString()
        'End If
    End Sub

    Private Sub RadBtnFiltre_Click(sender As Object, e As EventArgs) Handles RadBtnFiltre.Click
        Cursor.Current = Cursors.WaitCursor
        VmedocBindingSource.RemoveFilter()
        OasisDataSet2.Tables("v_medoc").Clear()
        LblAffichage.Text = "Veuillez saisir au moins 3 caractères dans le filte d'affichage (" & LblLabelFiltreAffichage.Text & ")"
        If TxtFiltreDenomination.Text <> "" Then
            FiltreDenomination = TxtFiltreDenomination.Text
            Dim FiltreLongueur As Integer
            FiltreLongueur = TxtFiltreDenomination.Text.Length
            If FiltreLongueur >= 3 Then
                Dim FiltreString As String = "oa_medicament_dci LIKE '" & TxtFiltreDenomination.Text & "%'"
                Dim Rowcount As Integer
                VmedocBindingSource.Filter = FiltreString
                Rowcount = GetCountMedicament(FiltreString)
                LblAffichage.Text = Rowcount.ToString & " occurrence(s) lue(s)"
                Me.V_medocTableAdapter.Fill(Me.OasisDataSet2.v_medoc)
            End If
        End If
        Cursor.Current = Cursors.Default
    End Sub

End Class
