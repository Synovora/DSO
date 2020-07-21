Imports Oasis_WF
Imports Oasis_Common
Public Class RadFOrdonnanceListe
    Private _SelectedPatient As PatientBase
    Private _UtilisateurConnecte As Utilisateur
    Private _Allergie As Boolean
    Private _ContreIndication As Boolean

    Public Property SelectedPatient As PatientBase
        Get
            Return _SelectedPatient
        End Get
        Set(value As PatientBase)
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

    Dim OrdonnanceDataTable As DataTable

    Private Sub RadFOrdonnanceListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementOrdonnanceListe()
        ChargementEtatCivil()

    End Sub

    Private Sub ChargementOrdonnanceListe()
        Dim ordonnanceDao As New OrdonnanceDao
        OrdonnanceDataTable = ordonnanceDao.getAllOrdonnancebyPatient(SelectedPatient.patientId)

        Dim i As Integer
        Dim rowCount As Integer = OrdonnanceDataTable.Rows.Count - 1
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim DateCreation, DateValidation, DateEdition As Date
        Dim Commentaire, AuteurNom As String
        Dim AuteurId As Integer

        For i = 0 To rowCount Step 1
            DateCreation = Coalesce(OrdonnanceDataTable.Rows(i)("oa_ordonnance_date_creation"), Nothing)
            DateValidation = Coalesce(OrdonnanceDataTable.Rows(i)("oa_ordonnance_date_validation"), Nothing)
            DateEdition = Coalesce(OrdonnanceDataTable.Rows(i)("oa_ordonnance_date_edition"), Nothing)
            Commentaire = Coalesce(OrdonnanceDataTable.Rows(i)("oa_ordonnance_commentaire"), "")
            AuteurId = Coalesce(OrdonnanceDataTable.Rows(i)("oa_ordonnance_utilisateur_creation"), 0)

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadOrdonnanceDataGridView.Rows.Add(iGrid)
            RadOrdonnanceDataGridView.Rows(iGrid).Cells("ordonnanceId").Value = (OrdonnanceDataTable.Rows(i)("oa_ordonnance_id"))

            AuteurNom = ""
            If AuteurId <> 0 Then
                Dim auteur As Utilisateur
                Dim userDao As New UserDao
                auteur = userDao.getUserById(AuteurId)
                'UtilisateurDao.SetUtilisateur(auteur, AuteurId)
                AuteurNom = auteur.UtilisateurPrenom & " " & auteur.UtilisateurNom
            End If
            RadOrdonnanceDataGridView.Rows(iGrid).Cells("auteur").Value = AuteurNom

            If DateCreation <> Nothing Then
                RadOrdonnanceDataGridView.Rows(iGrid).Cells("dateCreation").Value = DateCreation.ToString("dd.MM.yyyy")
            Else
                RadOrdonnanceDataGridView.Rows(iGrid).Cells("dateCreation").Value = ""
            End If

            If DateValidation <> Nothing Then
                RadOrdonnanceDataGridView.Rows(iGrid).Cells("dateValidation").Value = DateCreation.ToString("dd.MM.yyyy")
            Else
                RadOrdonnanceDataGridView.Rows(iGrid).Cells("dateValidation").Value = "En attente"
            End If

            If DateEdition <> Nothing Then
                RadOrdonnanceDataGridView.Rows(iGrid).Cells("dateEdition").Value = DateCreation.ToString("dd.MM.yyyy")
            Else
                RadOrdonnanceDataGridView.Rows(iGrid).Cells("dateEdition").Value = "En attente"
            End If

            RadOrdonnanceDataGridView.Rows(iGrid).Cells("commentaire").Value = Commentaire
        Next

        'Positionnement du grid sur la première occurrence
        If RadOrdonnanceDataGridView.Rows.Count > 0 Then
            Me.RadOrdonnanceDataGridView.CurrentRow = RadOrdonnanceDataGridView.ChildRows(0)
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
    End Sub

    Private Sub RadBtnCreation_Click(sender As Object, e As EventArgs) Handles RadBtnCreation.Click
        CreationOrdonnance()
    End Sub

    Private Sub CréerUneOrdonnanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUneOrdonnanceToolStripMenuItem.Click
        CreationOrdonnance()
    End Sub

    Private Sub CreationOrdonnance()
        'Appel création ordonnance

        Try
            Using vFOrdonnanceListeDetail As New RadFOrdonnanceListeDetail
                Dim OrdonnanceDao As New OrdonnanceDao
                Dim OrdonnanceId As Integer = OrdonnanceDao.CreateOrdonnance(SelectedPatient.PatientId, UtilisateurConnecte.UtilisateurId, userLog)
                vFOrdonnanceListeDetail.SelectedOrdonnanceId = OrdonnanceId
                vFOrdonnanceListeDetail.SelectedPatient = Me.SelectedPatient
                vFOrdonnanceListeDetail.UtilisateurConnecte = Me.UtilisateurConnecte
                vFOrdonnanceListeDetail.Allergie = Me.Allergie
                vFOrdonnanceListeDetail.ContreIndication = Me.ContreIndication
                vFOrdonnanceListeDetail.CommentaireOrdonnance = ""
                vFOrdonnanceListeDetail.ShowDialog() 'Modal
            End Using
            RadOrdonnanceDataGridView.Rows.Clear()
            ChargementOrdonnanceListe()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    'Modification ordonnance, affichage des lignes composant l'ordonnance
    Private Sub RadOrdonnanceDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadOrdonnanceDataGridView.CellDoubleClick
        ModifierOrdonnance()
    End Sub

    Private Sub ModifierUneOrdonnanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModifierUneOrdonnanceToolStripMenuItem.Click
        ModifierOrdonnance()
    End Sub

    Private Sub RadBtnModifier_Click(sender As Object, e As EventArgs) Handles RadBtnModifier.Click
        ModifierOrdonnance()
    End Sub
    Private Sub ModifierOrdonnance()
        If RadOrdonnanceDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadOrdonnanceDataGridView.Rows.IndexOf(Me.RadOrdonnanceDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim OrdonnanceId As Integer = RadOrdonnanceDataGridView.Rows(aRow).Cells("ordonnanceId").Value

                Try
                    Using vFOrdonnanceListeDetail As New RadFOrdonnanceListeDetail
                        vFOrdonnanceListeDetail.SelectedOrdonnanceId = OrdonnanceId
                        vFOrdonnanceListeDetail.SelectedPatient = Me.SelectedPatient
                        vFOrdonnanceListeDetail.Allergie = Me.Allergie
                        vFOrdonnanceListeDetail.ContreIndication = Me.ContreIndication
                        vFOrdonnanceListeDetail.UtilisateurConnecte = Me.UtilisateurConnecte
                        vFOrdonnanceListeDetail.CommentaireOrdonnance = RadOrdonnanceDataGridView.Rows(aRow).Cells("commentaire").Value
                        vFOrdonnanceListeDetail.ShowDialog() 'Modal
                    End Using
                    RadOrdonnanceDataGridView.Rows.Clear()
                    ChargementOrdonnanceListe()
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try

            End If
        Else
            MessageBox.Show("Veuillez sélectionner une ordonnance")
        End If
    End Sub

    'Supprimer une ordonnance
    Private Sub RadBtnSupprimer_Click(sender As Object, e As EventArgs) Handles RadBtnSupprimer.Click
        supprimerOrdonnance()
    End Sub

    Private Sub SupprimerUneOrdonnanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SupprimerUneOrdonnanceToolStripMenuItem.Click
        supprimerOrdonnance()
    End Sub

    Private Sub supprimerOrdonnance()
        If RadOrdonnanceDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadOrdonnanceDataGridView.Rows.IndexOf(Me.RadOrdonnanceDataGridView.CurrentRow)
            If aRow >= 0 Then
                If RadOrdonnanceDataGridView.Rows(aRow).Cells("dateValidation").Value = "En attente" Then
                    Dim OrdonnanceId As Integer = RadOrdonnanceDataGridView.Rows(aRow).Cells("ordonnanceId").Value
                    'Appel suppression ordonnance


                    RadOrdonnanceDataGridView.Rows.Clear()
                    ChargementOrdonnanceListe()
                Else
                    MessageBox.Show("Il n'est pas possible de supprimer une ordonnance validée")
                End If
            End If
        Else
            MessageBox.Show("Veuillez sélectionner une ordonnance")
        End If
    End Sub



    'Renouveller une ordonnance
    Private Sub RenouvellerUneOrdonnanceToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles RenouvellerUneOrdonnanceToolStripMenuItem.Click
        RenouvelerOrdonnance()
    End Sub
    Private Sub RadBtnRenouveler_Click(sender As Object, e As EventArgs) Handles RadBtnRenouveler.Click
        RenouvelerOrdonnance()
    End Sub

    Private Sub RenouvelerOrdonnance()
        'Copie de l'ordonnance
        'Vérification que toutes les lignes de l'ordonnance source sont toujours valides
        If RadOrdonnanceDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadOrdonnanceDataGridView.Rows.IndexOf(Me.RadOrdonnanceDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim OrdonnanceId As Integer = RadOrdonnanceDataGridView.Rows(aRow).Cells("ordonnanceId").Value

            End If
        Else
            MessageBox.Show("Veuillez sélectionner une ordonnance à renouveler")
        End If
    End Sub

    'Imprimer une ordonnance (non imprimée)
    Private Sub ImprimerUneOrdonnanceToolStripMenuItem_Click(sender As Object, e As EventArgs)
        ImprimerOrdonnance()
    End Sub

    Private Sub RadBtnImprimer_Click(sender As Object, e As EventArgs)
        ImprimerOrdonnance()
    End Sub

    Private Sub ImprimerOrdonnance()
        If RadOrdonnanceDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadOrdonnanceDataGridView.Rows.IndexOf(Me.RadOrdonnanceDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim OrdonnanceId As Integer = RadOrdonnanceDataGridView.Rows(aRow).Cells("ordonnanceId").Value
                'Tester si l'ordonnance sélectionnée est à valider

            End If
        Else
            MessageBox.Show("Veuillez sélectionner une ordonnance")
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

End Class
