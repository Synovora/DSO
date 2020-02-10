Imports Telerik.WinControls
Imports Oasis_Common

Public Class RadFPatientRendez_vousListe
    Private _selectedPatient As Patient

    Public Property SelectedPatient As Patient
        Get
            Return _selectedPatient
        End Get
        Set(value As Patient)
            _selectedPatient = value
        End Set
    End Property

    Dim tacheDao As New TacheDao
    Dim tache As Tache


    Private Sub RadFPatientRendez_vousListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.TextAndButtonsElement.TextElement.ForeColor = Color.Red
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.BackColor = Color.DarkBlue
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.Font = New Font("Arial", 8.0F, FontStyle.Italic)
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.TextImageRelation = TextImageRelation.TextBeforeImage
        Me.RadDesktopAlert1.Popup.AlertElement.BackColor = Color.MistyRose
        Me.RadDesktopAlert1.Popup.AlertElement.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.BorderColor = Color.DarkBlue

        ChargementPatient()
        ChargementListeRendezvous()
    End Sub

    Private Sub ChargementListeRendezvous()
        Dim RdvDataTable As DataTable
        Dim dateRendezVous, lastRendezVous As Date
        Dim TypeDemandeRdv As String
        Dim parcoursId, specialiteId As Long

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
                RadGridViewRDV.Rows(iGrid).Cells("specialite").Value = Environnement.Table_specialite.GetSpecialiteDescription(specialiteId)
            Else
                RadGridViewRDV.Rows(iGrid).Cells("specialite").Value = ""
            End If
            RadGridViewRDV.Rows(iGrid).Cells("ror_id").Value = RdvDataTable.Rows(i)("ror_id")
            RadGridViewRDV.Rows(iGrid).Cells("oa_ror_nom").Value = RdvDataTable.Rows(i)("oa_ror_nom")
            RadGridViewRDV.Rows(iGrid).Cells("oa_ror_structure_nom").Value = RdvDataTable.Rows(i)("oa_ror_structure_nom")
            RadGridViewRDV.Rows(iGrid).Cells("nature").Value = tacheDao.getItemNatureTacheByCode(RdvDataTable.Rows(i)("nature"))

            Select Case RdvDataTable.Rows(i)("etat")
                Case TacheDao.EtatTache.EN_COURS.ToString
                    RadGridViewRDV.Rows(iGrid).Cells("etat").Value = "En cours"
                Case TacheDao.EtatTache.EN_ATTENTE.ToString
                    RadGridViewRDV.Rows(iGrid).Cells("etat").Value = "En attente"
            End Select
            dateRendezVous = CDate(RdvDataTable.Rows(i)("date_rendez_vous"))

            Select Case RdvDataTable.Rows(i)("nature")
                Case TacheDao.EnumNatureTacheCode.RDV, TacheDao.EnumNatureTacheCode.RDV_SPECIALISTE
                    RadGridViewRDV.Rows(iGrid).Cells("dateRendezVous").Value = dateRendezVous.ToString("dd.MM.yyyy")
                    RadGridViewRDV.Rows(iGrid).Cells("heureRendezVous").Value = dateRendezVous.ToString("HH:mm")
                Case TacheDao.EnumNatureTacheCode.RDV_DEMANDE
                    TypeDemandeRdv = Coalesce(RdvDataTable.Rows(i)("type_demande_rendez_vous"), "")
                    Select Case TypeDemandeRdv
                        Case TacheDao.typeDemandeRendezVous.ANNEE.ToString
                            dateRendezVous = CDate(RdvDataTable.Rows(i)("date_rendez_vous"))
                            RadGridViewRDV.Rows(iGrid).Cells("dateRendezVous").Value = dateRendezVous.ToString("yyyy")
                        Case TacheDao.typeDemandeRendezVous.ANNEEMOIS.ToString
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
        LblDateNaissance.Text = SelectedPatient.PatientDateNaissance.ToString("dd.MM.yyyy")
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

    Private Sub RadBtnModifRDV_Click(sender As Object, e As EventArgs) Handles RadBtnModifRDV.Click
        ModificationDemandeRendezVous()
    End Sub

    Private Sub RadGridViewRDV_CellDoubleClick(sender As Object, e As UI.GridViewCellEventArgs) Handles RadGridViewRDV.CellDoubleClick
        ModificationDemandeRendezVous()
    End Sub

    Private Sub ModificationDemandeRendezVous()
        Dim aRow, maxRow As Integer
        aRow = Me.RadGridViewRDV.Rows.IndexOf(Me.RadGridViewRDV.CurrentRow)
        maxRow = RadGridViewRDV.Rows.Count - 1
        If aRow <= maxRow And aRow > -1 Then
            If RadGridViewRDV.Rows(aRow).Cells("nature").Value = TacheDao.EnumNatureTacheItem.RDV_DEMANDE Then
                Dim TacheId As Long = RadGridViewRDV.Rows(aRow).Cells("id").Value
                If TacheId <> 0 Then
                    tache = tacheDao.getTacheById(TacheId)
                    If tache.Etat = TacheDao.EtatTache.EN_ATTENTE.ToString Then
                        If tache.Nature = TacheDao.EnumNatureTacheCode.RDV_DEMANDE Then
                            Cursor.Current = Cursors.WaitCursor
                            Me.Enabled = False
                            Using form As New RadFTacheModificationDemandeRendezVous
                                form.SelectedPatient = Me.SelectedPatient
                                form.SelectedTacheId = TacheId
                                form.ShowDialog()
                                If form.CodeRetour = True Then
                                    Me.RadDesktopAlert1.CaptionText = "Notification demande de rendez-vous"
                                    Me.RadDesktopAlert1.ContentText = "Demande de rendez-vous modifiée"
                                    Me.RadDesktopAlert1.Show()

                                    RadGridViewRDV.Rows.Clear()
                                    ChargementListeRendezvous()
                                End If
                            End Using
                            Me.Enabled = True
                        End If
                    Else
                        Dim userDao As New UserDao
                        Dim user As Utilisateur = userDao.getUserById(tache.TraiteUserId)
                        MessageBox.Show("Le rendez-vous n'est pas modifiable, il est en cours de traitement par : " & user.UtilisateurPrenom & " " & user.UtilisateurNom)
                    End If
                End If
            Else
                MessageBox.Show("Option opérationnelle uniquement pour les demandes de rendez-vous")
            End If
        End If
    End Sub

    Private Sub RadBtnModifierRDV_Click(sender As Object, e As EventArgs) Handles RadBtnModifierRDV.Click
        Dim aRow, maxRow As Integer
        aRow = Me.RadGridViewRDV.Rows.IndexOf(Me.RadGridViewRDV.CurrentRow)
        maxRow = RadGridViewRDV.Rows.Count - 1
        If aRow <= maxRow And aRow > -1 Then
            If RadGridViewRDV.Rows(aRow).Cells("nature").Value = TacheDao.EnumNatureTacheItem.RDV OrElse
                RadGridViewRDV.Rows(aRow).Cells("nature").Value = TacheDao.EnumNatureTacheItem.RDV_SPECIALISTE Then
                Dim TacheId As Long = RadGridViewRDV.Rows(aRow).Cells("id").Value
                tache = tacheDao.getTacheById(TacheId)
                If tache.Id <> 0 AndAlso (tache.Nature = TacheDao.EnumNatureTacheCode.RDV Or tache.Nature = TacheDao.EnumNatureTacheCode.RDV_SPECIALISTE) Then
                    If tache.Etat = TacheDao.EtatTache.EN_ATTENTE.ToString OrElse
                    (tache.Etat = TacheDao.EtatTache.EN_COURS.ToString And userLog.UtilisateurId <> tache.TraiteUserId) Then
                        Cursor.Current = Cursors.WaitCursor
                        Me.Enabled = False
                        Using form As New RadFTacheModificationRendezVous
                            form.SelectedPatient = Me.SelectedPatient
                            form.SelectedTacheId = tache.Id
                            form.ShowDialog()
                            If form.CodeRetour = True Then
                                Me.RadDesktopAlert1.CaptionText = "Notification rendez-vous"
                                Me.RadDesktopAlert1.ContentText = "Rendez-vous modifié"
                                Me.RadDesktopAlert1.Show()

                                RadGridViewRDV.Rows.Clear()
                                ChargementListeRendezvous()
                            End If
                        End Using
                        Me.Enabled = True
                    Else
                        MessageBox.Show("Le rendez-vous n'est pas modifiable, il est en cours de traitement par : " & userLog.UtilisateurPrenom & " " & userLog.UtilisateurNom)
                    End If
                End If
            Else
                MessageBox.Show("Option opérationnelle uniquement pour les rendez-vous")
            End If
        End If

    End Sub

    Private Sub RadBtnClotureRDV_Click(sender As Object, e As EventArgs) Handles RadBtnClotureRDV.Click
        Dim aRow, maxRow As Integer
        aRow = Me.RadGridViewRDV.Rows.IndexOf(Me.RadGridViewRDV.CurrentRow)
        maxRow = RadGridViewRDV.Rows.Count - 1
        If aRow <= maxRow And aRow > -1 Then
            If RadGridViewRDV.Rows(aRow).Cells("nature").Value = TacheDao.EnumNatureTacheItem.RDV OrElse
                RadGridViewRDV.Rows(aRow).Cells("nature").Value = TacheDao.EnumNatureTacheItem.RDV_SPECIALISTE Then
                Dim TacheId As Long = RadGridViewRDV.Rows(aRow).Cells("id").Value
                tache = tacheDao.getTacheById(TacheId)
                Dim dateNext As Date = tache.DateRendezVous
                If dateNext <> Nothing Then
                    If tache.DateRendezVous.Date <= Date.Now.Date() Then
                        If tache.DateRendezVous.Date <= Date.Now.Date() Then
                            If tache.Id <> 0 AndAlso (tache.Nature = TacheDao.EnumNatureTacheCode.RDV_SPECIALISTE Or tache.Nature = TacheDao.EnumNatureTacheCode.RDV) Then
                                If MsgBox("Confirmation de la clôture du rendez-vous", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
                                    If tacheDao.ClotureTache(tache.Id, True) = True Then
                                        Me.RadDesktopAlert1.CaptionText = "Notification rendez-vous"
                                        Me.RadDesktopAlert1.ContentText = "Rendez-vous clôturé"
                                        Me.RadDesktopAlert1.Show()

                                        RadGridViewRDV.Rows.Clear()
                                        ChargementListeRendezvous()
                                    End If
                                End If
                            End If
                        End If
                    End If
                End If
            Else
                MessageBox.Show("Option opérationnelle uniquement pour les rendez-vous")
            End If
        End If
    End Sub
End Class
