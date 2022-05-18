Imports Oasis_Common

Public Class RadFTacheModificationDemandeRendezVous
    Private _selectedTacheId As Long
    Private _selectedPatient As Patient
    Private _codeRetour As Boolean

    Public Property SelectedTacheId As Long
        Get
            Return _selectedTacheId
        End Get
        Set(value As Long)
            _selectedTacheId = value
        End Set
    End Property

    Public Property SelectedPatient As Patient
        Get
            Return _selectedPatient
        End Get
        Set(value As Patient)
            _selectedPatient = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return _codeRetour
        End Get
        Set(value As Boolean)
            _codeRetour = value
        End Set
    End Property

    Dim EmetteurFonctionId As Long

    Dim tache As Tache
    Dim tacheDao As New TacheDao

    Private Sub RadFTacheModificationRendezVous_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Modification demande de rendez-vous", userLog)
        ChargementEtatCivil()
        tache = tacheDao.GetTacheById(SelectedTacheId)
        Select Case tache.TypedemandeRendezVous
            Case Tache.EnumDemandeRendezVous.ANNEE.ToString
                NumAn.Value = tache.DateRendezVous.Year()
                RadChkDRVAnneeSeulement.Checked = True
            Case Tache.EnumDemandeRendezVous.ANNEEMOIS.ToString
                NumAn.Value = tache.DateRendezVous.Year()
                NumMois.Value = tache.DateRendezVous.Month()
                RadChkDRVAnneeSeulement.Checked = False
            Case Else
                NumAn.Value = tache.DateRendezVous.Year()
                NumMois.Value = tache.DateRendezVous.Month()
                RadChkDRVAnneeSeulement.Checked = False
        End Select

        TxtRDVCommentaire.Text = tache.EmetteurCommentaire

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

    Private Sub Validation()
        If NumAn.Value < Date.Now().Year Then
            MessageBox.Show("Erreur : l'année de la demande de rendez-vous à créer : " & NumAn.Value.ToString & " est inférieure à l'année en cours")
        Else
            If RadChkDRVAnneeSeulement.Checked = True Then
                'Création demande de rendez-vous pour une année donnée (AAAA)
                Dim dateRendezVous As New DateTime(NumAn.Value, 1, 1, 0, 0, 0)
                If ModificationDemandeRendezVous(dateRendezVous, Tache.EnumDemandeRendezVous.ANNEE.ToString) = True Then
                    MessageBox.Show("demande de rendez-vous modifiée pour " & NumAn.Value.ToString)
                    Me.CodeRetour = True
                    Close()
                End If
            Else
                If NumAn.Value = Date.Now().Year And NumMois.Value < Date.Now().Month Then
                    MessageBox.Show("Erreur : la période demandée (" & NumMois.Value.ToString & "/" & NumAn.Value.ToString & ") est inférieure à la période en cours")
                Else
                    'Création demande de rendez-vous pour une période donnée (MM/AAAA)
                    Dim dateRendezVous As New DateTime(NumAn.Value, NumMois.Value, 1, 0, 0, 0)
                    If ModificationDemandeRendezVous(dateRendezVous, Tache.EnumDemandeRendezVous.ANNEEMOIS.ToString) = True Then
                        MessageBox.Show("Demande de rendez-vous modifiée pour " & NumMois.Value.ToString & "/" & NumAn.Value.ToString)
                        Me.CodeRetour = True
                        Close()
                    End If
                End If
            End If
        End If
    End Sub

    Private Function ModificationDemandeRendezVous(dateRendezVous As DateTime, typedemandeRendezVous As String) As Boolean
        Dim CodeRetour As Boolean = False
        tache = tacheDao.GetTacheById(SelectedTacheId)

        tache.DateRendezVous = dateRendezVous
        tache.TypedemandeRendezVous = typedemandeRendezVous
        tache.EmetteurCommentaire = TxtRDVCommentaire.Text
        Try
            If tacheDao.ModificationDemandeRendezVous(tache) = True Then
                CodeRetour = True
                Close()
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

        Return CodeRetour
    End Function

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        Validation()
    End Sub

    Private Sub RadChkDRVAnneeSeulement_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkDRVAnneeSeulement.ToggleStateChanged
        If RadChkDRVAnneeSeulement.Checked = True Then
            NumMois.Hide()
        Else
            NumMois.Show()
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnPlanifierRdv_Click(sender As Object, e As EventArgs) Handles RadBtnPlanifierRdv.Click
        Me.Enabled = False
        'Appeler l'écran de création du rendez-vous
        Using form As New RadFTacheModificationRendezVous
            form.SelectedPatient = Me.SelectedPatient
            form.TacheDemandeRdv = tache
            form.SelectedTacheId = 0
            form.DemanderendezVousOrigine = True  'On doit fournir cette information quand on vient de cet écran pour traiter les cas de RDV antérieur à la date du jour
            form.ShowDialog()
            If form.CodeRetour = True Then
                'Cloturer la tache de demande de rendez-vous
                tacheDao.ClotureTache(SelectedTacheId, True, userLog)
                '--> Quand on vient d'une demande de rendez-vous pour laquelle on planifie le rendez-vous
                '--- Si le RDV est antérieur à la date du jour, la création automatique de la demande de rendez-vous doit se faire après la clôture de la demande de rendez-vous initiale
                '--- car on ne peut pas créer une demande de rendez-vous si une autre existe
                If form.DemandeRendezVousCreation = True Then
                    Dim parcoursDao As New ParcoursDao
                    Dim parcours As Parcours
                    parcours = parcoursDao.GetParcoursById(tache.ParcoursId)
                    Dim DateDemandeDrv As Date
                    If form.DemandeRendezVousDate <> Nothing Then
                        DateDemandeDrv = form.DemandeRendezVousDate
                    Else
                        DateDemandeDrv = Date.Now()
                    End If
                    tacheDao.CreationAutomatiqueDeDemandeRendezVous(SelectedPatient, parcours, DateDemandeDrv, userLog)
                End If
                Me.CodeRetour = True
            End If
        End Using
        If Me.CodeRetour = True Then
            Close()
        End If
        Me.Enabled = True
    End Sub
End Class
