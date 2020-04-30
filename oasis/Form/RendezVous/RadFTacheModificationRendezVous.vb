Imports System.Configuration

Public Class RadFTacheModificationRendezVous
    Private _selectedTacheId As Long
    Private _selectedPatient As Patient
    Private _tacheDemandeRdv As Tache
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

    Public Property TacheDemandeRdv As Tache
        Get
            Return _tacheDemandeRdv
        End Get
        Set(value As Tache)
            _tacheDemandeRdv = value
        End Set
    End Property

    Dim tache As Tache
    Dim tacheDao As New TacheDao
    Dim parcoursDao As New ParcoursDao

    Private Sub RadFTacheModificationRendezVous_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        afficheTitleForm(Me, "Modification rendez-vous")
        ChargementEtatCivil()

        If SelectedTacheId <> 0 Then    '===> Modification de rendez-vous
            tache = tacheDao.GetTacheById(SelectedTacheId)
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
        Else                            '===> Création de rendez-vous
            '--- Le bouton d'action pour transformer un rendez-vous prévisionnel est inhibé (le rendez-vous n'existe pas encore)
            RadBtnTransformerEnPrevisionnel.Hide()

            '--- Initialisation de la date de rendez-vous à créer en fonction de la demande
            Select Case TacheDemandeRdv.TypedemandeRendezVous
                Case TacheDao.TypeDemandeRendezVous.ANNEE.ToString
                    If Date.Now.Year = TacheDemandeRdv.DateRendezVous.Year Then
                        NumDateRV.Value = Date.Now.ToString("dd.MM.yyyy")
                    Else
                        If Date.Now.Year < TacheDemandeRdv.DateRendezVous.Year Then
                            Dim dateRendezVous As New Date(TacheDemandeRdv.DateRendezVous.Year, 1, 1, 0, 0, 0)
                            NumDateRV.Value = dateRendezVous.ToString("dd.MM.yyyy")
                        Else
                            NumDateRV.Value = Date.Now.ToString("dd.MM.yyyy")
                        End If
                    End If
                Case TacheDao.TypeDemandeRendezVous.ANNEEMOIS.ToString
                    If Date.Now.Year = TacheDemandeRdv.DateRendezVous.Year Then
                        If Date.Now.Month >= TacheDemandeRdv.DateRendezVous.Month Then
                            NumDateRV.Value = Date.Now.ToString("dd.MM.yyyy")
                        Else
                            NumDateRV.Value = New Date(Date.Now.Year, TacheDemandeRdv.DateRendezVous.Month, 1, 0, 0, 0)
                        End If
                    Else
                        If Date.Now.Year < TacheDemandeRdv.DateRendezVous.Year Then
                            Dim dateRendezVous As New Date(TacheDemandeRdv.DateRendezVous.Year, TacheDemandeRdv.DateRendezVous.Month, 1, 0, 0, 0)
                            NumDateRV.Value = dateRendezVous.ToString("dd.MM.yyyy")
                        Else
                            NumDateRV.Value = Date.Now.ToString("dd.MM.yyyy")
                        End If
                    End If
            End Select

            '--- Initialisation des minutes à 0
            RadioBtn0.Checked = True
        End If

    End Sub

    Private Function Validation() As Boolean
        Me.CodeRetour = False
        If SelectedTacheId <> 0 Then    '===> Modification rendez-vous
            If ValidationModificationrendezVous() = True Then
                CodeRetour = True
                Close()
            End If
        Else                            '===> Création rendez-vous
            If ValidationCreationRendezVous() = True Then
                CodeRetour = True
                Close()
            End If
        End If


        Return CodeRetour
    End Function

    Private Function ValidationCreationRendezVous() As Boolean
        Dim CodeRetour As Boolean = False

        If NumDateRV.Value.Date < Date.Now().Date Then
            Dim message As String = "Attention, La date de rendez-vous à programmer (" &
                NumDateRV.Value.ToString("dd.MM.yyyy") &
                "), est antérieure à la date du jour (" &
                Date.Now().ToString("dd.MM.yyyy") &
                "), après validation, le rendez-vous sera automatiquement clôturé." & vbCrLf &
                "Confirmation de la date du rendez-vous ?"
            If MsgBox(message, MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation, "Avertissement") = MsgBoxResult.No Then
                Return False
                Exit Function
            End If
        End If

        Dim parcours As Parcours
        parcours = parcoursDao.getParcoursById(TacheDemandeRdv.ParcoursId)

        Dim minutesRV As Integer = CalculMinutes()
        Dim ClotureTache As Boolean
        Dim dateRendezVous As New DateTime(NumDateRV.Value.Year, NumDateRV.Value.Month, NumDateRV.Value.Day, NumheureRV.Value, minutesRV, 0)
        If NumDateRV.Value.Date < Date.Now().Date Then
            'Clôture du rendez-vous
            ClotureTache = True
            If CreationRendezVous(parcours, dateRendezVous, ClotureTache) = True Then
                MessageBox.Show("Rendez-vous programmé et clôturé pour le " & NumDateRV.Value.ToString("dd.MM.yyyy"))
                CodeRetour = True
                '--- Création automatique d'une demande de rendez-vous car le rendez-vous créé est clôturé du fait qu'il est antérieur à la date du jour
                tacheDao.CreationAutomatiqueDeDemandeRendezVous(SelectedPatient, parcours, Date.Now())
            End If
        Else
            ClotureTache = False
            If CreationRendezVous(parcours, dateRendezVous, ClotureTache) = True Then
                MessageBox.Show("Rendez-vous programmé pour le " & NumDateRV.Value.ToString("dd.MM.yyyy"))
                CodeRetour = True
            End If
        End If

        Return CodeRetour
    End Function

    Private Function CreationRendezVous(parcours As Parcours, dateRendezVous As Date, ClotureTache As Boolean) As Boolean
        CodeRetour = False

        Dim tacheEmetteurEtDestinataire As TacheEmetteurEtDestinataire
        tacheEmetteurEtDestinataire = tacheDao.SetTacheEmetteurEtDestinatiareBySpecialiteEtSousCategorie(parcours.SpecialiteId, parcours.SousCategorieId)

        Dim TacheCreation As New Tache

        TacheCreation.ParentId = 0
        TacheCreation.EmetteurUserId = userLog.UtilisateurId
        TacheCreation.EmetteurFonctionId = tacheEmetteurEtDestinataire.EmetteurFonctionId
        TacheCreation.UniteSanitaireId = SelectedPatient.PatientUniteSanitaireId
        TacheCreation.SiteId = SelectedPatient.PatientSiteId
        TacheCreation.PatientId = SelectedPatient.patientId
        TacheCreation.ParcoursId = parcours.Id
        TacheCreation.EpisodeId = 0
        TacheCreation.SousEpisodeId = 0
        TacheCreation.TraiteUserId = 0
        TacheCreation.TraiteFonctionId = tacheEmetteurEtDestinataire.TraiteFonctionId
        TacheCreation.DestinataireFonctionId = tacheEmetteurEtDestinataire.DestinataireFonctionId
        TacheCreation.Priorite = TacheDao.Priorite.BASSE
        TacheCreation.OrdreAffichage = 30
        TacheCreation.Categorie = TacheDao.CategorieTache.SOIN.ToString
        'Si le destinataire du rendez-vous n'est pas Oasis, on déclare un rendez-vous de type Spécialiste
        If parcours.SousCategorieId = EnumSousCategoriePPS.IDE Or
           parcours.SousCategorieId = EnumSousCategoriePPS.medecinReferent Or
           parcours.SousCategorieId = EnumSousCategoriePPS.sageFemme Then
            TacheCreation.Type = TacheDao.TypeTache.RDV.ToString
            TacheCreation.Nature = TacheDao.NatureTache.RDV.ToString
        Else
            TacheCreation.Type = TacheDao.TypeTache.RDV_SPECIALISTE.ToString()
            TacheCreation.Nature = TacheDao.NatureTache.RDV_SPECIALISTE.ToString
        End If

        Dim DureeRendezVous As Integer = 15
        Try
            If IsNumeric(ConfigurationManager.AppSettings("dureeRendezVous")) Then
                DureeRendezVous = ConfigurationManager.AppSettings("dureeRendezVous")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
        End Try

        TacheCreation.Duree = DureeRendezVous
        TacheCreation.EmetteurCommentaire = TxtRDVCommentaire.Text
        TacheCreation.HorodatageCreation = Date.Now()
        If ClotureTache = True Then
            TacheCreation.Etat = TacheDao.EtatTache.TERMINEE.ToString
            TacheCreation.Cloture = True
        Else
            TacheCreation.Etat = TacheDao.EtatTache.EN_ATTENTE.ToString
            TacheCreation.Cloture = False
        End If

        TacheCreation.TypedemandeRendezVous = ""
        TacheCreation.DateRendezVous = dateRendezVous

        If tacheDao.CreateTache(TacheCreation) = True Then
            CodeRetour = True
        End If

        Return CodeRetour
    End Function


    Private Function ValidationModificationrendezVous() As Boolean
        Me.CodeRetour = False

        If NumDateRV.Value.Date < Date.Now().Date Then
            Dim message As String = "Attention, La date de rendez-vous à reprogrammer (" &
                NumDateRV.Value.ToString("dd.MM.yyyy") &
                "), est antérieure à la date du jour (" &
                Date.Now().ToString("dd.MM.yyyy") &
                "), après modification, le rendez-vous sera automatiquement clôturé." & vbCrLf &
                "Confirmation de la date du rendez-vous ?"
            If MsgBox(message, MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation, "Avertissement") = MsgBoxResult.No Then
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
                CodeRetour = True

                '--- Création automatique d'une demande de rendez-vous car le rendez-vous modifié est clôturé du fait qu'il est antérieur à la date du jour
                Dim Tache As Tache = tacheDao.GetTacheById(SelectedTacheId)
                Dim parcours As Parcours
                parcours = parcoursDao.getParcoursById(Tache.ParcoursId)
                tacheDao.CreationAutomatiqueDeDemandeRendezVous(SelectedPatient, Parcours, Date.Now())
            End If
        Else
            ClotureTache = False
            If ModificationRendezVous(dateRendezVous, ClotureTache) = True Then
                MessageBox.Show("Rendez-vous programmé pour le " & NumDateRV.Value.ToString("dd.MM.yyyy"))
                CodeRetour = True
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
                'tache.ParentId = SelectedTacheId
                tache.ParentId = 0
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
