﻿Imports System.Configuration
Imports Oasis_Common
Imports Telerik.WinControls.UI
Public Class RadFEpisodeParametresCreation
    Private _SelectedPatient As Patient
    Private _CodeRetour As Boolean

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

    ReadOnly episodeDao As New EpisodeDao

    Private Sub RadFEpisodeParametresCreation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CodeRetour = False
        ChargementEtatCivil()
        NumDateRV.Value = Date.Now()
        NumheureRV.Value = Date.Now.Hour
        RadioBtn0.Checked = True
    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
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

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        Dim Minutes As Integer
        If NumDateRV.Value.Date > Date.Now.Date Then
            MessageBox.Show("La date de saisie des paramètres doit être inférieure ou égale à la date du jour !")
        Else
            If RadioBtn0.Checked = True Then
                Minutes = 0
            Else
                If RadioBtn15.Checked = True Then
                    Minutes = 15
                Else
                    If RadioBtn30.Checked = True Then
                        Minutes = 30
                    Else
                        Minutes = 45
                    End If
                End If
            End If

            Dim episode As New Episode With {
                .Commentaire = TxtCommentaire.Text,
                .DateCreation = NumDateRV.Value.Date.AddHours(NumheureRV.Value).AddMinutes(Minutes),
                .UserCreation = userLog.UtilisateurId,
                .PatientId = SelectedPatient.PatientId,
                .Type = Episode.EnumTypeEpisode.PARAMETRE.ToString,
                .TypeActivite = Episode.EnumTypeEpisode.PARAMETRE.ToString,
                .DescriptionActivite = "",
                .TypeProfil = userLog.TypeProfil,
                .Etat = Episode.EnumEtatEpisode.CLOTURE.ToString
            }

            Dim episodeId As Long
            episodeId = episodeDao.CreateEpisode(episode, userLog.UtilisateurId)
            If episodeId <> 0 Then
                'Création parametres aigus standards
                Dim ListGroupeParam = New List(Of Long)
                Dim ListParam = New List(Of Long)
                'Groupe de paramètres Protocole standard pathologie aiguë
                Dim DrcStandardDatatable As DataTable
                Dim drcStandardDao As New DrcStandardDao
                DrcStandardDatatable = drcStandardDao.getAllDrcByTypeActivite(Episode.EnumTypeActiviteEpisodeCode.PATHOLOGIE_AIGUE)
                Dim i As Integer
                Dim drcId As Long
                Dim categorieOasis As Integer
                Dim rowCount As Integer = DrcStandardDatatable.Rows.Count - 1
                For i = 0 To rowCount Step 1
                    drcId = DrcStandardDatatable.Rows(i)("drc_id")
                    categorieOasis = DrcStandardDatatable.Rows(i)("categorie_oasis")
                    Select Case categorieOasis
                        Case Drc.EnumCategorieOasisCode.GroupeParametres
                            If Not ListGroupeParam.Contains(drcId) Then
                                ListGroupeParam.Add(drcId)
                            End If
                    End Select
                Next

                'Récupération de la liste des paramètres à mesurer pour l'épisode patient
                Dim parametreDrcDao As New ParametreDrcDao
                Dim ParamDt As DataTable
                'Lecture groupe de paramètres
                For i = 0 To ListGroupeParam.Count - 1
                    ParamDt = parametreDrcDao.GetParametresByDrcId(ListGroupeParam.Item(i))
                    rowCount = ParamDt.Rows.Count - 1
                    For J = 0 To rowCount Step 1
                        Dim ParamId As Integer = Coalesce(ParamDt.Rows(J)("parametre_id"), 0)
                        If Not ListParam.Contains(ParamId) Then
                            ListParam.Add(ParamId)
                        End If
                    Next
                Next

                Dim parametreDao As New ParametreDao
                Dim episodeParametreDao As New EpisodeParametreDao
                Dim parametre As Parametre
                For i = 0 To ListParam.Count - 1
                    parametre = parametreDao.GetParametreById(ListParam.Item(i))
                    'Creation
                    Dim episodeParametre As EpisodeParametre = New EpisodeParametre With {
                        .EpisodeId = episodeId,
                        .ParametreId = parametre.Id,
                        .PatientId = episode.PatientId,
                        .Entier = parametre.Entier,
                        .Decimal = parametre.Decimal,
                        .Unite = parametre.Unite,
                        .Ordre = parametre.Ordre,
                        .Description = parametre.Description,
                        .Valeur = Nothing,
                        .Inactif = False
                    }
                    episodeParametreDao.CreateEpisodeParametre(episodeParametre)
                Next

                Using form As New RadFEpisodeParametresSaisie
                    form.SelectedPatient = SelectedPatient
                    form.SelectedEpisodeId = episodeId
                    form.ShowDialog()
                End Using

                Me.CodeRetour = True
                Close()
            End If
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub
End Class