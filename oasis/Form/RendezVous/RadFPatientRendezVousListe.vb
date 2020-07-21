Imports Telerik.WinControls
Imports Oasis_Common
Public Class RadFPatientRendezVousListe

    Private _selectedPatient As PatientBase

    Public Property SelectedPatient As PatientBase
        Get
            Return _selectedPatient
        End Get
        Set(value As PatientBase)
            _selectedPatient = value
        End Set
    End Property

    Dim userDao As New UserDao
    Dim tacheDao As New TacheDao
    Dim tache As Tache

    Private Sub RadFPatientRendezVousListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.TextAndButtonsElement.TextElement.ForeColor = Color.Red
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.BackColor = Color.DarkBlue
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.Font = New Font("Arial", 8.0F, FontStyle.Italic)
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.TextImageRelation = TextImageRelation.TextBeforeImage
        Me.RadDesktopAlert1.Popup.AlertElement.BackColor = Color.MistyRose
        Me.RadDesktopAlert1.Popup.AlertElement.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.BorderColor = Color.DarkBlue

        If userLog.TypeProfil <> ProfilDao.EnumProfilType.MEDICAL.ToString Then
            RadBtnAnnulation.Hide()
        End If

        ChargementPatient()
        ChargementListeRendezvous()
    End Sub

    Private Sub ChargementListeRendezvous()
        Dim RdvDataTable As DataTable
        Dim dateRendezVous, lastRendezVous As Date
        Dim TypeDemandeRdv As String
        Dim parcoursId, specialiteId As Long

        RadGridViewRDV.Rows.Clear()

        RdvDataTable = tacheDao.GetRDVByPatient(SelectedPatient.patientId)
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = RdvDataTable.Rows.Count - 1
        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            iGrid += 1
            RadGridViewRDV.Rows.Add(iGrid)
            RadGridViewRDV.Rows(iGrid).Cells("id").Value = RdvDataTable.Rows(i)("id")
            RadGridViewRDV.Rows(iGrid).Cells("parcours_id").Value = RdvDataTable.Rows(i)("parcours_id")
            parcoursId = Coalesce(RdvDataTable.Rows(i)("parcours_id"), 0)
            specialiteId = Coalesce(RdvDataTable.Rows(i)("oa_ror_specialite_id"), 0)
            If specialiteId <> 0 Then
                RadGridViewRDV.Rows(iGrid).Cells("specialite").Value = Table_specialite.GetSpecialiteDescription(specialiteId)
            Else
                RadGridViewRDV.Rows(iGrid).Cells("specialite").Value = ""
            End If
            RadGridViewRDV.Rows(iGrid).Cells("ror_id").Value = RdvDataTable.Rows(i)("ror_id")
            RadGridViewRDV.Rows(iGrid).Cells("oa_ror_nom").Value = RdvDataTable.Rows(i)("oa_ror_nom")
            RadGridViewRDV.Rows(iGrid).Cells("oa_ror_structure_nom").Value = RdvDataTable.Rows(i)("oa_ror_structure_nom")
            RadGridViewRDV.Rows(iGrid).Cells("nature").Value = tacheDao.GetItemNatureTacheByCode(RdvDataTable.Rows(i)("nature"))

            Select Case RdvDataTable.Rows(i)("etat")
                Case Tache.EtatTache.EN_COURS.ToString
                    RadGridViewRDV.Rows(iGrid).Cells("etat").Value = "En cours"
                Case Tache.EtatTache.EN_ATTENTE.ToString
                    RadGridViewRDV.Rows(iGrid).Cells("etat").Value = "En attente"
            End Select
            dateRendezVous = CDate(RdvDataTable.Rows(i)("date_rendez_vous"))

            RadGridViewRDV.Rows(iGrid).Cells("traitePar").Value = RdvDataTable.Rows(i)("oa_utilisateur_prenom") & " " & RdvDataTable.Rows(i)("oa_utilisateur_nom")

            Select Case RdvDataTable.Rows(i)("nature")
                Case Tache.EnumNatureTacheCode.RDV, Tache.EnumNatureTacheCode.RDV_SPECIALISTE
                    RadGridViewRDV.Rows(iGrid).Cells("dateRendezVous").Value = dateRendezVous.ToString("dd.MM.yyyy")
                    RadGridViewRDV.Rows(iGrid).Cells("heureRendezVous").Value = dateRendezVous.ToString("HH:mm")
                Case Tache.EnumNatureTacheCode.RDV_DEMANDE
                    TypeDemandeRdv = Coalesce(RdvDataTable.Rows(i)("type_demande_rendez_vous"), "")
                    Select Case TypeDemandeRdv
                        Case Tache.EnumDemandeRendezVous.ANNEE.ToString
                            dateRendezVous = CDate(RdvDataTable.Rows(i)("date_rendez_vous"))
                            RadGridViewRDV.Rows(iGrid).Cells("dateRendezVous").Value = dateRendezVous.ToString("yyyy")
                        Case Tache.EnumDemandeRendezVous.ANNEEMOIS.ToString
                            RadGridViewRDV.Rows(iGrid).Cells("dateRendezVous").Value = dateRendezVous.ToString("MM.yyyy")
                        Case Else
                            RadGridViewRDV.Rows(iGrid).Cells("dateRendezVous").Value = outils.FormatageDateAffichage(dateRendezVous)
                    End Select
                    RadGridViewRDV.Rows(iGrid).Cells("heureRendezVous").Value = ""
                Case Else
                    RadGridViewRDV.Rows(iGrid).Cells("dateRendezVous").Value = ""
                    RadGridViewRDV.Rows(iGrid).Cells("heureRendezVous").Value = ""
            End Select

            RadGridViewRDV.Rows(iGrid).Cells("dernierRendezVous").Value = ""
            If parcoursId <> 0 Then
                tache = tacheDao.GetDernierRenezVousByPatientId(SelectedPatient.patientId, parcoursId)
                lastRendezVous = tache.DateRendezVous
                If lastRendezVous <> Nothing Then
                    RadGridViewRDV.Rows(iGrid).Cells("dernierRendezVous").Value = lastRendezVous.ToString("dd.MM.yyyy")
                End If
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewRDV.Rows.Count > 0 Then
            Me.RadGridViewRDV.CurrentRow = RadGridViewRDV.Rows(0)
        End If
    End Sub

    Private Sub ChargementPatient()
        LblPatientNIR.Text = SelectedPatient.PatientNir
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        'LblDateNaissance.Text = SelectedPatient.PatientDateNaissance.ToString("dd.MM.yyyy")
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientAdresse1.Text = SelectedPatient.PatientAdresse1
        LblPatientAdresse2.Text = SelectedPatient.PatientAdresse2
        LblPatientCodePostal.Text = SelectedPatient.PatientCodePostal
        LblPatientVille.Text = SelectedPatient.PatientVille
        LblPatientTel1.Text = SelectedPatient.PatientTel1
        LblPatientTel2.Text = SelectedPatient.PatientTel2
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)
        LblPatientDateMaj.Text = SelectedPatient.PatientSyntheseDateMaj.ToString("dd.MM.yyyy")

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

    'Modification rendez-vous ou demande de rendez-vous
    Private Sub RadBtnModifRDV_Click(sender As Object, e As EventArgs) Handles RadBtnModifRDV.Click
        Modification()
    End Sub

    Private Sub RadGridViewRDV_CellDoubleClick(sender As Object, e As UI.GridViewCellEventArgs) Handles RadGridViewRDV.CellDoubleClick
        Modification()
    End Sub

    Private Sub Modification()
        Dim aRow, maxRow As Integer
        aRow = Me.RadGridViewRDV.Rows.IndexOf(Me.RadGridViewRDV.CurrentRow)
        maxRow = RadGridViewRDV.Rows.Count - 1
        If aRow <= maxRow And aRow > -1 Then
            If RadGridViewRDV.Rows(aRow).Cells("nature").Value = Tache.EnumNatureTacheItem.RDV OrElse
                RadGridViewRDV.Rows(aRow).Cells("nature").Value = Tache.EnumNatureTacheItem.RDV_SPECIALISTE Then
                Dim TacheId As Long = RadGridViewRDV.Rows(aRow).Cells("id").Value
                tache = tacheDao.GetTacheById(TacheId)
                ModificationRendezVous(tache)
            Else
                If RadGridViewRDV.Rows(aRow).Cells("nature").Value = Tache.EnumNatureTacheItem.RDV_DEMANDE Then
                    Dim TacheId As Long = RadGridViewRDV.Rows(aRow).Cells("id").Value
                    tache = tacheDao.GetTacheById(TacheId)
                    ModificationDemandeRendezVous(tache)
                End If
            End If
        End If
    End Sub

    Private Sub ModificationDemandeRendezVous(tache As Tache)
        Dim TacheALiberer As Boolean = False
        If tache.Id <> 0 Then
            tache = tacheDao.GetTacheById(tache.Id)
            If tache.Etat = Tache.EtatTache.EN_ATTENTE.ToString OrElse
                    (tache.Etat = Tache.EtatTache.EN_COURS.ToString And userLog.UtilisateurId = tache.TraiteUserId) Then
                Cursor.Current = Cursors.WaitCursor
                Me.Enabled = False

                'Si la tâche est en attente on attribue la tâche à l'utilisateur
                If tache.Etat = Tache.EtatTache.EN_ATTENTE.ToString Then
                    TacheALiberer = True
                    tacheDao.AttribueTacheToUserLog(tache.Id, userLog)
                End If

                Using form As New RadFTacheModificationDemandeRendezVous
                    form.SelectedPatient = Me.SelectedPatient
                    form.SelectedTacheId = tache.Id
                    form.ShowDialog()
                    If form.CodeRetour = True Then
                        Me.RadDesktopAlert1.CaptionText = "Notification demande de rendez-vous"
                        Me.RadDesktopAlert1.ContentText = "Demande de rendez-vous modifiée"
                        Me.RadDesktopAlert1.Show()

                        ChargementListeRendezvous()
                    End If
                End Using

                If TacheALiberer = True Then
                    'Si la tâche était initialement disponible, On libére la tâche
                    If tacheDao.DesattribueTache(tache.Id) = False Then
                        'Erreur
                    End If
                End If

                Me.Enabled = True
            Else
                Dim user As Utilisateur = UserDao.getUserById(tache.TraiteUserId)
                MessageBox.Show("La demande de rendez-vous n'est pas modifiable, elle est attribuée à : " & user.UtilisateurPrenom & " " & user.UtilisateurNom)
            End If
        End If
    End Sub

    Private Sub ModificationRendezVous(tache As Tache)
        Dim TacheALiberer As Boolean = False
        If tache.Id <> 0 AndAlso (tache.Nature = Tache.EnumNatureTacheCode.RDV Or tache.Nature = Tache.EnumNatureTacheCode.RDV_SPECIALISTE) Then
            'Si la tache est en attente (disponible pour traitement) ou attribuée à l'utilisateur authentifié(userlog)
            If tache.Etat = Tache.EtatTache.EN_ATTENTE.ToString OrElse
            (tache.Etat = Tache.EtatTache.EN_COURS.ToString And userLog.UtilisateurId = tache.TraiteUserId) Then
                Cursor.Current = Cursors.WaitCursor
                Me.Enabled = False
                'Si la tâche est en attente on attribue la tâche à l'utilisateur
                If tache.Etat = Tache.EtatTache.EN_ATTENTE.ToString Then
                    TacheALiberer = True
                    tacheDao.AttribueTacheToUserLog(tache.Id, userLog)
                End If

                Dim RDVisTranforme As Boolean = False

                Using form As New RadFTacheModificationRendezVous
                    form.SelectedPatient = Me.SelectedPatient
                    form.SelectedTacheId = tache.Id
                    form.ShowDialog()
                    RDVisTranforme = form.RDVisTransforme
                    If form.CodeRetour = True Then
                        Me.RadDesktopAlert1.CaptionText = "Notification rendez-vous"
                        Me.RadDesktopAlert1.ContentText = "Rendez-vous modifié"
                        Me.RadDesktopAlert1.Show()
                    End If
                End Using

                If (TacheALiberer = True AndAlso RDVisTranforme = False) Then
                    'Si la tâche était initialement disponible, On libére la tâche
                    tacheDao.DesattribueTache(tache.Id)
                End If

                ChargementListeRendezvous()
                Me.Enabled = True
            Else
                Dim user As Utilisateur = userDao.getUserById(tache.TraiteUserId)
                MessageBox.Show("Le rendez-vous n'est pas modifiable, il est attribué à : " & user.UtilisateurPrenom & " " & user.UtilisateurNom)
            End If
        End If
    End Sub


    'Déclaration rendez-vous honoré
    Private Sub RadBtnCloture_Click(sender As Object, e As EventArgs) Handles RadBtnCloture.Click
        Dim aRow, maxRow As Integer
        Dim TacheALiberer As Boolean = False

        aRow = Me.RadGridViewRDV.Rows.IndexOf(Me.RadGridViewRDV.CurrentRow)
        maxRow = RadGridViewRDV.Rows.Count - 1
        If aRow <= maxRow And aRow > -1 Then
            If RadGridViewRDV.Rows(aRow).Cells("nature").Value = Tache.EnumNatureTacheItem.RDV OrElse
                RadGridViewRDV.Rows(aRow).Cells("nature").Value = Tache.EnumNatureTacheItem.RDV_SPECIALISTE Then
                Dim TacheId As Long = RadGridViewRDV.Rows(aRow).Cells("id").Value
                tache = tacheDao.GetTacheById(TacheId)
                If tache.Etat = Tache.EtatTache.EN_ATTENTE.ToString OrElse
                (tache.Etat = Tache.EtatTache.EN_COURS.ToString And userLog.UtilisateurId = tache.TraiteUserId) Then

                    Dim dateNext As Date = tache.DateRendezVous
                    If dateNext <> Nothing Then
                        If tache.DateRendezVous.Date <= Date.Now.Date() Then

                            'Si la tâche est en attente on attribue la tâche à l'utilisateur
                            If tache.Etat = Tache.EtatTache.EN_ATTENTE.ToString Then
                                TacheALiberer = True
                                tacheDao.AttribueTacheToUserLog(tache.Id, userLog)
                            End If

                            If MsgBox("Confirmation de la clôture du rendez-vous", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
                                If tacheDao.ClotureTache(tache.Id, True, userLog) = True Then
                                    Me.RadDesktopAlert1.CaptionText = "Notification rendez-vous"
                                    Me.RadDesktopAlert1.ContentText = "Rendez-vous clôturé"
                                    Me.RadDesktopAlert1.Show()

                                    'Généreration d'une demande de rendez-vous suite à la cloture du rendez-vous en cours
                                    Dim parcoursId As Long = RadGridViewRDV.Rows(aRow).Cells("parcours_id").Value
                                    Dim parcoursDao As New ParcoursDao
                                    Dim parcours As Parcours = parcoursDao.GetParcoursById(parcoursId)
                                    If parcours.Rythme <> 0 Then
                                        If tacheDao.CreationAutomatiqueDeDemandeRendezVous(SelectedPatient, parcours, tache.DateRendezVous.Date, userLog) = True Then
                                            Me.RadDesktopAlert1.CaptionText = "Notification demande de rendez-vous"
                                            Me.RadDesktopAlert1.ContentText = "Une demande de rendez-vous a été automatiquement générée pour cet intervenant"
                                            Me.RadDesktopAlert1.Show()
                                        End If
                                    End If
                                    ChargementListeRendezvous()
                                End If
                            Else
                                If TacheALiberer = True Then
                                    'Si la tâche était initialement disponible, On libére la tâche
                                    tacheDao.DesattribueTache(tache.Id)
                                End If
                            End If
                        Else
                            MessageBox.Show("Il n'est pas possible de déclarer le rendez-vous 'honoré' car la date du rendez-vous est à venir", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
                        End If
                    End If

                Else
                    Dim user As Utilisateur = userDao.getUserById(tache.TraiteUserId)
                    MessageBox.Show("Le rendez-vous n'est pas modifiable, il est attribué à : " & user.UtilisateurPrenom & " " & user.UtilisateurNom, "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
                End If

            Else
                MessageBox.Show("Option opérationnelle uniquement pour les rendez-vous", "Erreur", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End If
        End If
    End Sub

    'Annulation rendez-vous ou demande de rendez-vous
    Private Sub RadBtnAnnulation_Click(sender As Object, e As EventArgs) Handles RadBtnAnnulation.Click
        Dim aRow, maxRow As Integer
        aRow = Me.RadGridViewRDV.Rows.IndexOf(Me.RadGridViewRDV.CurrentRow)
        maxRow = RadGridViewRDV.Rows.Count - 1
        If aRow <= maxRow And aRow > -1 Then
            Dim TacheId As Long = RadGridViewRDV.Rows(aRow).Cells("id").Value
            tache = tacheDao.GetTacheById(TacheId)
            Dim parcoursDao As New ParcoursDao
            Dim parcours As Parcours = parcoursDao.GetParcoursById(tache.ParcoursId)
            If parcours.Rythme = 0 Then
                If RadGridViewRDV.Rows(aRow).Cells("nature").Value = Tache.EnumNatureTacheItem.RDV OrElse
                RadGridViewRDV.Rows(aRow).Cells("nature").Value = Tache.EnumNatureTacheItem.RDV_SPECIALISTE Then
                    AnnulationRendezVous(tache)
                Else
                    If RadGridViewRDV.Rows(aRow).Cells("nature").Value = Tache.EnumNatureTacheItem.RDV_DEMANDE Then
                        AnnulationdemandeRendezVous(tache)
                    End If
                End If
            Else
                MessageBox.Show("Cet intervenant a un rythme de rendez-vous de renseigné, la suppression d'un rendez-vous ou d'une demande de rendez-vous est interdite", "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            End If
        End If
    End Sub

    Private Sub AnnulationRendezVous(tache As Tache)
        Dim TacheALiberer As Boolean = False
        'Si la tache est en attente (disponible pour traitement) ou attribuée à l'utilisateur authentifié(userlog)
        If tache.Etat = Tache.EtatTache.EN_ATTENTE.ToString OrElse
            (tache.Etat = Tache.EtatTache.EN_COURS.ToString And userLog.UtilisateurId = tache.TraiteUserId) Then

            'Si la tâche est en attente on attribue la tâche à l'utilisateur
            If tache.Etat = Tache.EtatTache.EN_ATTENTE.ToString Then
                tacheDao.AttribueTacheToUserLog(tache.Id, userLog)
                TacheALiberer = True
            End If

            If MsgBox("Confirmation de l'annulation du rendez-vous", vbYesNo + vbExclamation, "Annulation rendez-vous") = MsgBoxResult.Yes Then
                If tacheDao.AnnulationTache(tache.Id, userLog) = True Then
                    Me.RadDesktopAlert1.CaptionText = "Notification rendez-vous"
                    Me.RadDesktopAlert1.ContentText = "Rendez-vous annulé"
                    Me.RadDesktopAlert1.Show()

                    ChargementListeRendezvous()
                End If
            Else
                If TacheALiberer = True Then
                    'Si la tâche était initialement disponible, On libére la tâche
                    tacheDao.DesattribueTache(tache.Id)
                End If
            End If

        Else
            Dim user As Utilisateur
            user = UserDao.getUserById(tache.TraiteUserId)
            MessageBox.Show("Le rendez-vous n'est pas disponible, il est attribué à : " & user.UtilisateurPrenom & " " & user.UtilisateurNom, "Information", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Private Sub AnnulationdemandeRendezVous(tache As Tache)
        Dim TacheALiberer As Boolean = False
        'Si la tache est en attente (disponible pour traitement) ou attribuée à l'utilisateur authentifié(userlog)
        If tache.Etat = Tache.EtatTache.EN_ATTENTE.ToString OrElse
            (tache.Etat = Tache.EtatTache.EN_COURS.ToString And userLog.UtilisateurId = tache.TraiteUserId) Then

            'Si la tâche est en attente on attribue la tâche à l'utilisateur
            If tache.Etat = Tache.EtatTache.EN_ATTENTE.ToString Then
                tacheDao.AttribueTacheToUserLog(tache.Id, userLog)
                TacheALiberer = True
            End If

            If MsgBox("Confirmation de l'annulation de la demande de rendez-vous", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
                If tacheDao.AnnulationTache(tache.Id, userLog) = True Then
                    Me.RadDesktopAlert1.CaptionText = "Notification rendez-vous"
                    Me.RadDesktopAlert1.ContentText = "Demande de rendez-vous annulée"
                    Me.RadDesktopAlert1.Show()

                    ChargementListeRendezvous()
                End If
            Else
                If TacheALiberer = True Then
                    'Si la tâche était initialement disponible, On libére la tâche
                    tacheDao.DesattribueTache(tache.Id)
                End If
            End If

        Else
            Dim user As Utilisateur
            user = UserDao.getUserById(tache.TraiteUserId)
            MessageBox.Show("La demande de rendez-vous n'est pas disponible, elle est attribuée à : " & user.UtilisateurPrenom & " " & user.UtilisateurNom)
        End If
    End Sub


    Private Sub RadBtnRefresh_Click(sender As Object, e As EventArgs) Handles RadBtnRefresh.Click
        Cursor.Current = Cursors.WaitCursor
        ChargementListeRendezvous()
        Cursor.Current = Cursors.Default
    End Sub


    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

End Class
