Imports System.Configuration
Imports Oasis_Common
Imports Telerik.WinControls

Public Class RadFEpisodeDetailCreation
    Private _SelectedPatient As Patient
    Private _episodeId As Long
    Private _CodeRetour As Boolean
    Property EpisodeType As Episode.EnumTypeEpisode

    Public Property SelectedPatient As Patient
        Get
            Return _SelectedPatient
        End Get
        Set(value As Patient)
            _SelectedPatient = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return _CodeRetour
        End Get
        Set(value As Boolean)
            _CodeRetour = value
        End Set
    End Property

    Public Property EpisodeId As Long
        Get
            Return _episodeId
        End Get
        Set(value As Long)
            _episodeId = value
        End Set
    End Property

    ReadOnly episodeProtocoleCollaboratifDao As New EpisodeProtocoleCollaboratifDao
    ReadOnly episodeActiviteDao As New EpisodeTypeActiviteDao
    Dim episodeActiviteDT As New DataTable
    ReadOnly episodeDao As New EpisodeDao
    ReadOnly contexteDao As New ContexteDao
    ReadOnly antecedentDao As New AntecedentDao
    ReadOnly chaineEpisodeDao As New ChaineEpisodeDao
    ReadOnly userDao As New UserDao
    ReadOnly drcDao As New DrcDao

    Private Sub RadFEpisodeDetailCreation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim TypeActiviteEpisode As String
        Dim LimiteAgeEnfant As Integer = 0
        Dim AgeMinPreventionFemme As Integer = 0
        Dim LimiteAgeEnfantParm As Integer = ConfigurationManager.AppSettings("limiteAgeEnfant")
        If IsNumeric(LimiteAgeEnfantParm) Then
            LimiteAgeEnfant = CInt(LimiteAgeEnfantParm)
        End If
        Dim AgeMinPreventionFemmeParm As Integer = ConfigurationManager.AppSettings("AgeMinPreventionFemme")
        If IsNumeric(AgeMinPreventionFemmeParm) Then
            AgeMinPreventionFemme = CInt(AgeMinPreventionFemmeParm)
        End If

        Dim genre, enfant As String
        Dim agePatient As Integer = CalculAgeEnAnnee(SelectedPatient.PatientDateNaissance)

        Dim listActivite As New List(Of String)
        episodeActiviteDT = episodeActiviteDao.GetAllEpisodeActivite
        Dim i As Integer
        Dim rowCount As Integer = episodeActiviteDT.Rows.Count - 1
        For i = 0 To rowCount Step 1
            genre = Coalesce(episodeActiviteDT.Rows(i)("oa_activite_genre"), "")
            If genre = "F" Then
                If SelectedPatient.PatientGenreId.Trim = "M" Then
                    Continue For
                End If
                If agePatient < AgeMinPreventionFemme Then
                    Continue For
                End If
            End If
            enfant = Coalesce(episodeActiviteDT.Rows(i)("oa_activite_enfant"), False)
            If enfant = True Then
                If LimiteAgeEnfant <> 0 Then
                    If agePatient > LimiteAgeEnfant Then
                        Continue For
                    End If
                End If
            End If

            TypeActiviteEpisode = episodeDao.GetItemTypeActiviteByCode(episodeActiviteDT.Rows(i)("oa_activite_type"))
            If TypeActiviteEpisode <> "" Then
                listActivite.Add(TypeActiviteEpisode)
            End If
        Next

        CbxEpisodeActivite.DataSource = listActivite

        If EpisodeType = Episode.EnumTypeEpisode.VIRTUEL Then
            RadioBtnVirtuel.Checked = True
            RadioBtnConsultation.Checked = False
        Else
            RadioBtnConsultation.Checked = True
            RadioBtnVirtuel.Checked = False
        End If

        Me.CodeRetour = False
        ChargementEtatCivil()

    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientDateMaj.Text = FormatageDateAffichage(SelectedPatient.PatientSyntheseDateMaj)
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        'Création épisode
        Dim IsCreationOk As Boolean = False
        Dim episodeDao As New EpisodeDao
        Dim episode As New Episode With {
            .Commentaire = TxtCommentaire.Text,
            .DescriptionActivite = TxtDescriptionActivite.Text
        }
        If RadioBtnConsultation.Checked = True Then
            episode.Type = Episode.EnumTypeEpisode.CONSULTATION.ToString
        Else
            episode.Type = Episode.EnumTypeEpisode.VIRTUEL.ToString
        End If
        episode.TypeActivite = CbxEpisodeActivite.Text
        episode.TypeActivite = episodeDao.GetCodeTypeActiviteByItem(CbxEpisodeActivite.Text)
        If episode.TypeActivite <> "" Or episode.Type = Episode.EnumTypeEpisode.VIRTUEL.ToString Then
            If episode.TypeActivite = Episode.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GROSSESSE Then
                Dim DrcIdGrossesse As Integer = ConfigurationManager.AppSettings("DrcIdGrossesse")
                If DrcIdGrossesse = 0 Then
                    outils.CreateLog("DRC Grossesse non trouvée", Me.Name, Log.EnumTypeLog.ERREUR.ToString, userLog)
                    DrcIdGrossesse = 833
                End If
                If contexteDao.ExistContexteValideWithDrcId(SelectedPatient.PatientId, DrcIdGrossesse) Then
                    IsCreationOk = True
                Else
                    IsCreationOk = False
                    MessageBox.Show("Pour créer un épisode de type 'Suivi grossesse', un contexte de type 'Grossesse' (code : " & DrcIdGrossesse.ToString & ") doit exister au préalable pour la patiente ")
                End If
            Else
                IsCreationOk = True
            End If
            If IsCreationOk Then
                episode.PatientId = SelectedPatient.PatientId
                episode.TypeProfil = userLog.TypeProfil
                EpisodeId = episodeDao.CreateEpisode(episode, userLog.UtilisateurId)
                episode.Id = EpisodeId
                If EpisodeId <> 0 Then
                    Cursor.Current = Cursors.WaitCursor

                    'Génération paramètres et actes paramédicaux
                    episodeProtocoleCollaboratifDao.GenerateParametreEtProtocoleCollaboratifByEpisode(episode)

                    'Création automatique des intervenants Osis s'ils n'existent pas
                    Dim parcoursDao As New ParcoursDao
                    Try
                        Dim parcours As Parcours
                        parcours = parcoursDao.GetParcoursIDEbyPatient(SelectedPatient.PatientId)
                    Catch ex As Exception
                        If ex.Message.StartsWith("parcours inexistant") Then
                            parcoursDao.CreateIntervenantOasisByPatient(SelectedPatient.PatientId, userLog, False)
                        End If
                    End Try

                    ' Condition d'atrribution des CE
                    If episode.TypeActivite = Episode.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GROSSESSE Then
                        Dim contexte As Antecedent = contexteDao.GetByDrcId(SelectedPatient.PatientId, ConfigurationManager.AppSettings("DrcIdGrossesse"))
                        chaineEpisodeDao.AddRelation(New RelationChaineEpisode() With {
                        .Id = 0,
                        .EpisodeId = EpisodeId,
                        .ChaineId = contexte.Id
                    })
                    ElseIf episode.TypeActivite = Episode.EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE Then
                        Dim antecedents As List(Of Antecedent) = antecedentDao.GetListByPatient(SelectedPatient.PatientId)
                        For Each antecedent In antecedents
                            If antecedent.StatutAffichage <> Antecedent.EnumStatutAffichage.OCCULTE Then
                                chaineEpisodeDao.AddRelation(New RelationChaineEpisode() With {
                                .Id = 0,
                                .EpisodeId = EpisodeId,
                                .ChaineId = antecedent.Id
                            })
                            End If
                        Next
                    ElseIf episode.TypeActivite = Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE Then
                        Dim contexte As Antecedent = contexteDao.GetByDrcId(SelectedPatient.PatientId, ConfigurationManager.AppSettings("DrcIdSuiviPrescolaire"))

                        If contexte Is Nothing Then
                            Dim drc As Drc = drcDao.GetDrcById(ConfigurationManager.AppSettings("DrcIdSuiviPrescolaire"))
                            contexte = CreateAntecedentOrContexte(episode, drc, "C")
                        End If
                        If contexte IsNot Nothing Then
                            chaineEpisodeDao.AddRelation(New RelationChaineEpisode() With {
                                .Id = 0,
                                .EpisodeId = EpisodeId,
                                .ChaineId = contexte.Id
                        })
                        End If
                    ElseIf episode.TypeActivite = Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE Then
                        Dim contexte As Antecedent = contexteDao.GetByDrcId(SelectedPatient.PatientId, ConfigurationManager.AppSettings("DrcIdSuiviScolaire"))

                        If contexte Is Nothing Then
                            Dim drc As Drc = drcDao.GetDrcById(ConfigurationManager.AppSettings("DrcIdSuiviScolaire"))
                            contexte = CreateAntecedentOrContexte(episode, drc, "C")
                        End If
                        If contexte IsNot Nothing Then
                            chaineEpisodeDao.AddRelation(New RelationChaineEpisode() With {
                        .Id = 0,
                        .EpisodeId = EpisodeId,
                        .ChaineId = contexte.Id
                    })
                        End If
                    ElseIf episode.TypeActivite = Episode.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GYNECOLOGIQUE Then
                        Dim antecedent As Antecedent = antecedentDao.GetByDrcId(SelectedPatient.PatientId, ConfigurationManager.AppSettings("DrcIdGyneco"))
                        If antecedent Is Nothing Then
                            Dim drc As Drc = drcDao.GetDrcById(ConfigurationManager.AppSettings("DrcIdGyneco"))
                            antecedent = CreateAntecedentOrContexte(episode, drc, "A")
                        End If
                        If antecedent IsNot Nothing Then
                            chaineEpisodeDao.AddRelation(New RelationChaineEpisode() With {
                        .Id = 0,
                        .EpisodeId = EpisodeId,
                        .ChaineId = antecedent.Id
                        })
                        End If
                    End If

                    Cursor.Current = Cursors.Default
                    CodeRetour = True
                    Close()
                Else
                    MessageBox.Show("Erreur : la création de l'épisode n'a pas aboutie!")
                    CodeRetour = False
                    Close()
                End If
            End If
        Else
            MessageBox.Show("la saisie du type d'activité de l'épisode est obligatoire")
        End If

    End Sub

    Private Function CreateAntecedentOrContexte(SelectedEpisode As Episode, SelectedDrcId As Drc, Type As String) As Antecedent
        Dim contexteUpdate As New Antecedent
        Dim ContexteHistoACreer As New AntecedentHisto
        Dim user As Utilisateur = userDao.getUserById(1)

        contexteUpdate.PatientId = SelectedPatient.PatientId
        contexteUpdate.Type = Type
        contexteUpdate.Niveau = 1
        contexteUpdate.Nature = "Patient"
        contexteUpdate.Inactif = False
        contexteUpdate.CategorieContexte = ContexteCourrier.EnumParcoursBaseCode.Medical
        contexteUpdate.DrcId = SelectedDrcId.DrcId
        contexteUpdate.Description = SelectedDrcId.DrcLibelle
        contexteUpdate.DateCreation = Date.Now
        contexteUpdate.UserCreation = user.UtilisateurId
        contexteUpdate.DateModification = Date.Now
        contexteUpdate.UserModification = user.UtilisateurId
        contexteUpdate.DateDebut = Date.Now
        contexteUpdate.DateFin = New Date(2999, 12, 31, 0, 0, 0)
        contexteUpdate.ChaineEpisodeDateFin = Date.Now().AddMonths(Coalesce(ConfigurationManager.AppSettings("ChaineEpisodePeriode"), 0))
        contexteUpdate.StatutAffichage = "P"
        contexteUpdate.StatutAffichageTransformation = "P"
        contexteUpdate.Diagnostic = 1
        'contexteUpdate.AldId =
        '    contexteUpdate.AldCim10Id =


        AntecedentHistoCreationDao.InitAntecedentHistorisation(contexteUpdate, user, ContexteHistoACreer)

        If Type = "C" Then
            contexteUpdate.Id = contexteDao.CreationContexte(contexteUpdate, ContexteHistoACreer, user, False, SelectedEpisode)
        ElseIf Type = "A" Then
            contexteUpdate.Id = antecedentDao.CreationAntecedent(contexteUpdate, user)
        End If
        If contexteUpdate.Id <> 0 Then
            Try
                Dim form As New RadFNotification()
                form.Titre = If(Type = "C", "Notification contexte patient", "Notification antecedent patient")
                form.Message = If(Type = "C", "Contexte patient créé", "Antecdent patient créé")
                form.Show()
                Close()
                Return contexteUpdate
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try
        End If
        Return Nothing
    End Function

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        CodeRetour = False
        Close()
    End Sub

    Private Sub RadioBtnConsultation_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnConsultation.CheckedChanged
        CbxEpisodeActivite.Text = CbxEpisodeActivite.Items(0)
        CbxEpisodeActivite.Show()
    End Sub

    Private Sub RadioBtnVirtuel_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnVirtuel.CheckedChanged
        CbxEpisodeActivite.Text = ""
        CbxEpisodeActivite.Hide()
    End Sub
End Class
