Public Class RadFTacheModificationRendezVous
    Private _selectedTacheId As Long
    Private _selectedPatient As Patient
    Private _codeRetour As Boolean
    Public Property RDVisTransforme As Boolean = False

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

    Dim tache As Tache
    Dim tacheDao As New TacheDao

    Private Sub RadFTacheModificationRendezVous_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        afficheTitleForm(Me, "Modification rendez-vous")
        ChargementEtatCivil()
        tache = tacheDao.getTacheById(SelectedTacheId)
        TxtRDVCommentaire.Text = tache.EmetteurCommentaire
        NumDateRV.Value = tache.DateRendezVous.ToString("dd.MM.yyyy")
        NumheureRV.Value = tache.DateRendezVous.ToString("HH")
        Dim Minute As Integer = tache.DateRendezVous.ToString("mm")
        Select Case Minute
            Case 0
                RadioBtn0.Checked = True
            Case 15
                RadioBtn15.Checked = True
            Case 30
                RadioBtn30.Checked = True
            Case 45
                RadioBtn45.Checked = True
            Case Else
                RadioBtn0.Checked = True
        End Select
    End Sub

    Private Function Validation() As Boolean
        Me.CodeRetour = False
        If NumDateRV.Value.Date < Date.Now().Date Then
            Dim message As String = "Attention, La date de rendez-vous à programmer (" &
                NumDateRV.Value.ToString("dd.MM.yyyy") &
                "), est antérieure à la date du jour (" &
                Date.Now().ToString("dd.MM.yyyy") &
                "), après modification, le rendez-vous sera automatiquement clôturé." & vbCrLf &
                "Confirmation de la date du rendez-vous ?"
            If MsgBox(message, MsgBoxStyle.YesNo, "") = MsgBoxResult.No Then
                Return False
                Exit Function
            End If
        End If
        Dim minutesRV As Integer = CalculMinutes()
        Dim ClotureTache As Boolean
        Dim dateRendezVous As New DateTime(NumDateRV.Value.Year, NumDateRV.Value.Month, NumDateRV.Value.Day, NumheureRV.Value, minutesRV, 0)
        If NumDateRV.Value.Date < Date.Now().Date Then
            'Clôture du rendez-vous
            ClotureTache = True
            If ModificationRendezVous(dateRendezVous, ClotureTache) = True Then
                MessageBox.Show("Rendez-vous programmé et clôturé pour le " & NumDateRV.Value.ToString("dd.MM.yyyy"))
                Me.CodeRetour = True
                Close()
            End If
        Else
            ClotureTache = False
            If ModificationRendezVous(dateRendezVous, ClotureTache) = True Then
                MessageBox.Show("Rendez-vous programmé pour le " & NumDateRV.Value.ToString("dd.MM.yyyy"))
                Me.CodeRetour = True
                Close()
            End If
        End If

        Return CodeRetour
    End Function

    Private Function ModificationRendezVous(dateRendezVous As DateTime, clotureTache As Boolean) As Boolean
        Dim CodeRetour As Boolean = False

        tache.EmetteurCommentaire = TxtRDVCommentaire.Text
        tache.DateRendezVous = dateRendezVous

        If tacheDao.ModificationRendezVous(tache, tache.Etat) = True Then
            If clotureTache = True Then
                If tacheDao.ClotureTache(tache.Id, True) = True Then
                    CodeRetour = True
                End If
            Else
                CodeRetour = True
            End If
        End If

        Return CodeRetour
    End Function

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

    Private Function CalculMinutes() As Integer
        Dim minutes As Integer = 0
        If RadioBtn0.Checked = True Then
            minutes = 0
        Else
            If RadioBtn15.Checked = True Then
                minutes = 15
            Else
                If RadioBtn30.Checked = True Then
                    minutes = 30
                Else
                    minutes = 45
                End If
            End If
        End If
        Return minutes
    End Function

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        Validation()
    End Sub

    Private Sub RadBtnTransformerEnPrevisionnel_Click(sender As Object, e As EventArgs) Handles RadBtnTransformerEnPrevisionnel.Click
        If MsgBox("Confirmation de la transformation du rendez-vous planifié en prévisionnel", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            Dim tacheInit As Tache = tacheDao.GetTacheById(SelectedTacheId)
            If tacheDao.AnnulationTache(SelectedTacheId) = True Then
                Dim tache As New Tache
                tache.ParentId = SelectedTacheId
                tache.EmetteurUserId = tacheInit.EmetteurUserId
                tache.UniteSanitaireId = tacheInit.UniteSanitaireId
                tache.SiteId = tacheInit.SiteId
                tache.PatientId = tacheInit.PatientId
                tache.ParcoursId = tacheInit.ParcoursId
                tache.TraiteFonctionId = tacheInit.TraiteFonctionId
                tache.DestinataireFonctionId = tacheInit.DestinataireFonctionId
                tache.Priorite = tacheInit.Priorite
                tache.OrdreAffichage = tacheInit.OrdreAffichage
                tache.Categorie = tacheInit.Categorie
                tache.Type = TacheDao.TypeTache.RDV_DEMANDE.ToString
                tache.Nature = TacheDao.NatureTache.RDV_DEMANDE.ToString
                tache.Duree = tacheInit.Duree
                tache.EmetteurCommentaire = tacheInit.EmetteurCommentaire
                tache.HorodatageCreation = Date.Now()
                tache.TypedemandeRendezVous = TacheDao.TypeDemandeRendezVous.ANNEE.ToString
                tache.DateRendezVous = New Date(tacheInit.DateRendezVous.Year, tacheInit.DateRendezVous.Month, 1, 0, 0, 0)
                tache.Etat = TacheDao.EtatTache.EN_ATTENTE.ToString
                If tacheDao.CreateTache(tache) = True Then
                    Dim tacheId As Long = tacheDao.GetLastDemandeRendezVousByPatient(SelectedPatient.patientId)
                    tacheDao.AttribueTacheToUserLog(tacheId)
                    Using form As New RadFTacheModificationDemandeRendezVous
                        form.SelectedPatient = Me.SelectedPatient
                        form.SelectedTacheId = tacheId
                        form.ShowDialog()
                    End Using
                    tacheDao.DesattribueTache(tacheId)
                    RDVisTransforme = True
                    Close()
                End If
            End If
        End If
    End Sub
End Class
