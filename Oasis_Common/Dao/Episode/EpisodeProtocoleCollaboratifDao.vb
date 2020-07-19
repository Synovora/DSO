
Imports System.Data.SqlClient
Imports Oasis_Common
Public Class EpisodeProtocoleCollaboratifDao
    Inherits StandardDao

    Public Sub GenerateParametreEtProtocoleCollaboratifByEpisode(episode As Episode)
        Dim ListActePara As List(Of Long)
        Dim ListParam As List(Of Long)

        ListParam = GetListeParametreByPatientEtTypeEpisode(episode.PatientId, episode.TypeActivite)
        'Lecture de la liste des paramètres et création des paramètres attachés à l'épisode
        Dim parametreDao As New ParametreDao
        Dim episodeParametreDao As New EpisodeParametreDao
        Dim patientDao As New PatientDao
        Dim parametre As Parametre
        For i = 0 To ListParam.Count - 1
            parametre = parametreDao.GetParametreById(ListParam.Item(i))
            'Creation
            Dim episodeParametre As EpisodeParametre = New EpisodeParametre
            episodeParametre.EpisodeId = episode.Id
            episodeParametre.ParametreId = parametre.Id
            episodeParametre.PatientId = episode.PatientId
            episodeParametre.Entier = parametre.Entier
            episodeParametre.Decimal = parametre.Decimal
            episodeParametre.Unite = parametre.Unite
            episodeParametre.Ordre = parametre.Ordre
            episodeParametre.Description = parametre.Description
            episodeParametre.Valeur = 0
            episodeParametre.Inactif = False
            episodeParametreDao.CreateEpisodeParametre(episodeParametre)
        Next

        ListActePara = GetListeActeParamedicalByPatientEtTypeEpisode(episode.PatientId, episode.TypeActivite)
        'Lecture de la liste des actes paramédicaux et création des actes paramédicaux attachés à l'épisode
        Dim episodeActeParamedicalDao As New EpisodeActeParamedicalDao
        For i = 0 To ListActePara.Count - 1
            'parametre = parametreDao.GetParametreById(ListParam.Item(i))
            'Creation
            Dim episodeActeParamedical As EpisodeActeParamedical = New EpisodeActeParamedical
            episodeActeParamedical.EpisodeId = episode.Id
            episodeActeParamedical.PatientId = episode.PatientId
            episodeActeParamedical.DrcId = ListActePara.Item(i)
            episodeActeParamedical.Observation = ""
            episodeActeParamedical.TypeObservation = EpisodeObservationDao.EnumTypeEpisodeObservation.PARAMEDICAL.ToString
            episodeActeParamedical.UserId = 0
            episodeActeParamedical.DateObservation = Nothing
            episodeActeParamedical.DateModification = Nothing
            episodeActeParamedical.Inactif = False
            episodeActeParamedicalDao.CreateEpisodeActeParamedical(episodeActeParamedical)
        Next
    End Sub

    Public Function GetListeActeParamedicalByPatientEtTypeEpisode(patientId As Long, TypeActiviteEpisode As String) As List(Of Long)
        Dim ListActePara = New List(Of Long)
        Dim ListProtocol = New List(Of Long)
        Dim patientDao As New PatientDao

        Dim patient As PatientBase
        Dim agePatientEnJour As Integer = 0
        Dim agePatientEnAnnee As Integer = 0
        patient = patientDao.GetPatientById(patientId)
        Select Case TypeActiviteEpisode
            Case EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE
                'Calcul âge enfant en jour
                agePatientEnJour = CalculAgeEnJour(patient.PatientDateNaissance)
                agePatientEnJour += JoursAAjouterPourCalculAgePreScolaire
            Case EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE
                'Calcul âge enfant en année
                agePatientEnAnnee = CalculAgeEnAnnee(patient.PatientDateNaissance)
            Case Else
        End Select

        'Protocole standard par activité
        Dim DrcStandardDatatable As DataTable
        Dim drcStandardDao As New DrcStandardDao
        DrcStandardDatatable = drcStandardDao.GetAllDrcByTypeActivite(TypeActiviteEpisode)
        Dim i As Integer
        Dim drcId As Long
        Dim ageMinDrc, AgeMaxDrc As Integer
        Dim categorieOasis As Integer
        Dim rowCount As Integer = DrcStandardDatatable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            drcId = DrcStandardDatatable.Rows(i)("drc_id")
            categorieOasis = DrcStandardDatatable.Rows(i)("categorie_oasis")
            ageMinDrc = Coalesce(DrcStandardDatatable.Rows(i)("age_min"), 0)
            AgeMaxDrc = Coalesce(DrcStandardDatatable.Rows(i)("age_max"), 0)

            Select Case TypeActiviteEpisode
                Case EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE
                    If ageMinDrc <> 0 Then
                        If agePatientEnAnnee < ageMinDrc Then
                            Continue For
                        End If
                    End If
                    If AgeMaxDrc <> 0 Then
                        If agePatientEnAnnee > AgeMaxDrc Then
                            Continue For
                        End If
                    End If
                Case EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE
                    Dim ageMinDrcEnJour As Integer = outils.ConvertirEnJourDureeEnMois(ageMinDrc)
                    Dim ageMaxDrcEnJour As Integer = outils.ConvertirEnJourDureeEnMois(AgeMaxDrc)
                    If ageMinDrcEnJour <> 0 Then
                        If agePatientEnJour < ageMinDrcEnJour Then
                            Continue For
                        End If
                    End If
                    If ageMaxDrcEnJour <> 0 Then
                        If agePatientEnJour > ageMaxDrcEnJour Then
                            Continue For
                        End If
                    End If
            End Select

            Select Case categorieOasis
                Case DrcDao.EnumCategorieOasisCode.ActeParamedical
                    If Not ListActePara.Contains(drcId) Then
                        ListActePara.Add(drcId)
                    End If
                Case DrcDao.EnumCategorieOasisCode.ProtocoleCollaboratif
                    If Not ListProtocol.Contains(drcId) Then
                        ListProtocol.Add(drcId)
                    End If
            End Select
        Next

        'Protocole associée au patient
        Dim DrcConsignedDatatable As DataTable
        Dim parcoursConsigneDao As New ParcoursConsigneDao
        DrcConsignedDatatable = parcoursConsigneDao.GetDrcPatientByTypeActiviteEtPatient(TypeActiviteEpisode, patientId)

        rowCount = DrcConsignedDatatable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            drcId = DrcConsignedDatatable.Rows(i)("oa_parcours_consigne_drc_id")
            categorieOasis = DrcConsignedDatatable.Rows(i)("oa_drc_oasis_categorie")
            ageMinDrc = Coalesce(DrcConsignedDatatable.Rows(i)("oa_parcours_age_min"), 0)
            AgeMaxDrc = Coalesce(DrcConsignedDatatable.Rows(i)("oa_parcours_age_max"), 0)

            Select Case TypeActiviteEpisode
                Case EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE
                    If ageMinDrc <> 0 Then
                        If agePatientEnAnnee < ageMinDrc Then
                            Continue For
                        End If
                    End If
                    If AgeMaxDrc <> 0 Then
                        If agePatientEnAnnee > AgeMaxDrc Then
                            Continue For
                        End If
                    End If
                Case EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE
                    Dim ageMinEnJour As Integer = outils.ConvertirEnJourDureeEnMois(ageMinDrc)
                    Dim ageMaxEnJour As Integer = outils.ConvertirEnJourDureeEnMois(AgeMaxDrc)
                    If ageMinEnJour <> 0 Then
                        If agePatientEnJour < ageMinEnJour Then
                            Continue For
                        End If
                    End If
                    If ageMaxEnJour <> 0 Then
                        If agePatientEnJour > ageMaxEnJour Then
                            Continue For
                        End If
                    End If
            End Select

            'Contrôle dates d'application
            Dim dateDebut As Date = Coalesce(DrcConsignedDatatable.Rows(i)("oa_parcours_consigne_date_debut"), Nothing)
            If dateDebut <> Nothing Then
                If dateDebut.Date > Date.Now().Date Then
                    Continue For
                End If
            End If
            Dim dateFin As Date = Coalesce(DrcConsignedDatatable.Rows(i)("oa_parcours_consigne_date_fin"), Nothing)
            If dateFin <> Nothing Then
                If dateFin.Date < Date.Now().Date Then
                    Continue For
                End If
            End If

            Select Case categorieOasis
                Case DrcDao.EnumCategorieOasisCode.ActeParamedical, DrcDao.EnumCategorieOasisCode.Prevention
                    If Not ListActePara.Contains(drcId) Then
                        ListActePara.Add(drcId)
                    End If
                Case DrcDao.EnumCategorieOasisCode.ProtocoleCollaboratif
                    If Not ListProtocol.Contains(drcId) Then
                        ListProtocol.Add(drcId)
                    End If
            End Select
        Next

        'Récupération des actes paramédicaux associés aux protocoles collaboratifs
        Dim drcActeParamedicalAssoDao As New DrcActeParamedicalAssoDao
        Dim DrcDt As DataTable
        'Lecture groupe de paramètres
        For i = 0 To ListProtocol.Count - 1
            DrcDt = drcActeParamedicalAssoDao.getAllActeParamedicalAssoByProtocoleCollaboratifId(ListProtocol.Item(i))
            rowCount = DrcDt.Rows.Count - 1
            For J = 0 To rowCount Step 1
                drcId = Coalesce(DrcDt.Rows(J)("drc_acte_paramedical_id"), 0)
                If Not ListActePara.Contains(drcId) Then
                    ListActePara.Add(drcId)
                End If
            Next
        Next

        Return ListActePara
    End Function

    Public Function GetListeParametreByPatientEtTypeEpisode(patientId As Long, TypeActiviteEpisode As String) As List(Of Long)
        Dim patientDao As New PatientDao
        Dim ListGroupeParam = New List(Of Long)
        Dim ListParam = New List(Of Long)

        Dim patient As PatientBase
        Dim agePatientEnJour As Integer = 0
        Dim agePatientEnAnnee As Integer = 0

        patient = patientDao.GetPatientById(patientId)
        Select Case TypeActiviteEpisode
            Case EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE
                'Calcul âge enfant en jour
                agePatientEnJour = CalculAgeEnJour(patient.PatientDateNaissance)
                agePatientEnJour += JoursAAjouterPourCalculAgePreScolaire
            Case EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE
                'Calcul âge enfant en année
                agePatientEnAnnee = CalculAgeEnAnnee(patient.PatientDateNaissance)
            Case Else
        End Select

        'Protocole standard
        Dim DrcStandardDatatable As DataTable
        Dim drcStandardDao As New DrcStandardDao
        DrcStandardDatatable = drcStandardDao.GetAllDrcByTypeActivite(TypeActiviteEpisode)
        Dim i As Integer
        Dim drcId As Long
        Dim ageMinDrc, AgeMaxDrc As Integer
        Dim categorieOasis As Integer
        Dim rowCount As Integer = DrcStandardDatatable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            drcId = DrcStandardDatatable.Rows(i)("drc_id")
            categorieOasis = DrcStandardDatatable.Rows(i)("categorie_oasis")
            ageMinDrc = Coalesce(DrcStandardDatatable.Rows(i)("age_min"), 0)
            AgeMaxDrc = Coalesce(DrcStandardDatatable.Rows(i)("age_max"), 0)

            Select Case TypeActiviteEpisode
                Case EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE
                    If ageMinDrc <> 0 Then
                        If agePatientEnAnnee < ageMinDrc Then
                            Continue For
                        End If
                    End If
                    If AgeMaxDrc <> 0 Then
                        If agePatientEnAnnee > AgeMaxDrc Then
                            Continue For
                        End If
                    End If
                Case EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE
                    Dim ageMinDrcEnJour As Integer = outils.ConvertirEnJourDureeEnMois(ageMinDrc)
                    Dim ageMaxDrcEnJour As Integer = outils.ConvertirEnJourDureeEnMois(AgeMaxDrc)
                    If ageMinDrcEnJour <> 0 Then
                        If agePatientEnJour < ageMinDrcEnJour Then
                            Continue For
                        End If
                    End If
                    If ageMaxDrcEnJour <> 0 Then
                        If agePatientEnJour > ageMaxDrcEnJour Then
                            Continue For
                        End If
                    End If
            End Select

            Select Case categorieOasis
                Case DrcDao.EnumCategorieOasisCode.GroupeParametres
                    If Not ListGroupeParam.Contains(drcId) Then
                        ListGroupeParam.Add(drcId)
                    End If
            End Select
        Next

        'Protocole associée au patient
        Dim DrcConsignedDatatable As DataTable
        Dim parcoursConsigneDao As New ParcoursConsigneDao
        DrcConsignedDatatable = parcoursConsigneDao.GetDrcPatientByTypeActiviteEtPatient(TypeActiviteEpisode, patientId)

        rowCount = DrcConsignedDatatable.Rows.Count - 1
        For i = 0 To rowCount Step 1
            drcId = DrcConsignedDatatable.Rows(i)("oa_parcours_consigne_drc_id")
            categorieOasis = DrcConsignedDatatable.Rows(i)("oa_drc_oasis_categorie")
            ageMinDrc = Coalesce(DrcConsignedDatatable.Rows(i)("oa_parcours_age_min"), 0)
            AgeMaxDrc = Coalesce(DrcConsignedDatatable.Rows(i)("oa_parcours_age_max"), 0)

            Select Case TypeActiviteEpisode
                Case EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE
                    If ageMinDrc <> 0 Then
                        If agePatientEnAnnee < ageMinDrc Then
                            Continue For
                        End If
                    End If
                    If AgeMaxDrc <> 0 Then
                        If agePatientEnAnnee > AgeMaxDrc Then
                            Continue For
                        End If
                    End If
                Case EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE
                    Dim ageMinEnJour As Integer = outils.ConvertirEnJourDureeEnMois(ageMinDrc)
                    Dim ageMaxEnJour As Integer = outils.ConvertirEnJourDureeEnMois(AgeMaxDrc)
                    If ageMinEnJour <> 0 Then
                        If agePatientEnJour < ageMinEnJour Then
                            Continue For
                        End If
                    End If
                    If ageMaxEnJour <> 0 Then
                        If agePatientEnJour > ageMaxEnJour Then
                            Continue For
                        End If
                    End If
            End Select

            'Contrôle dates d'application
            Dim dateDebut As Date = Coalesce(DrcConsignedDatatable.Rows(i)("oa_parcours_consigne_date_debut"), Nothing)
            If dateDebut <> Nothing Then
                If dateDebut.Date > Date.Now().Date Then
                    Continue For
                End If
            End If
            Dim dateFin As Date = Coalesce(DrcConsignedDatatable.Rows(i)("oa_parcours_consigne_date_fin"), Nothing)
            If dateFin <> Nothing Then
                If dateFin.Date < Date.Now().Date Then
                    Continue For
                End If
            End If

            Select Case categorieOasis
                Case DrcDao.EnumCategorieOasisCode.GroupeParametres
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
            ParamDt = parametreDrcDao.getParametresByDrcId(ListGroupeParam.Item(i))
            rowCount = ParamDt.Rows.Count - 1
            For J = 0 To rowCount Step 1
                Dim ParamId As Integer = Coalesce(ParamDt.Rows(J)("parametre_id"), 0)
                If Not ListParam.Contains(ParamId) Then
                    ListParam.Add(ParamId)
                End If
            Next
        Next

        Return ListParam
    End Function

End Class
