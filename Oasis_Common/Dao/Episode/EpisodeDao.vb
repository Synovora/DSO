Imports System.Data.SqlClient

Public Class EpisodeDao
    Inherits StandardDao

    Public Function GetItemTypeActiviteByCode(Code As String) As String
        Dim Item As String
        Select Case Code
            Case Episode.EnumTypeActiviteEpisodeCode.PATHOLOGIE_AIGUE
                Item = Episode.EnumTypeActiviteEpisodeItem.PATHOLOGIE_AIGUE
            Case Episode.EnumTypeActiviteEpisodeCode.PREVENTION_AUTRE
                Item = Episode.EnumTypeActiviteEpisodeItem.PREVENTION_AUTRE
            Case Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE
                Item = Episode.EnumTypeActiviteEpisodeItem.PREVENTION_ENFANT_PRE_SCOLAIRE
            Case Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE
                Item = Episode.EnumTypeActiviteEpisodeItem.PREVENTION_ENFANT_SCOLAIRE
            Case Episode.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GROSSESSE
                Item = Episode.EnumTypeActiviteEpisodeItem.PREVENTION_SUIVI_GROSSESSE
            Case Episode.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GYNECOLOGIQUE
                Item = Episode.EnumTypeActiviteEpisodeItem.PREVENTION_SUIVI_GYNECOLOGIQUE
            Case Episode.EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE
                Item = Episode.EnumTypeActiviteEpisodeItem.SUIVI_CHRONIQUE
            Case Episode.EnumTypeActiviteEpisodeCode.SOCIAL
                Item = Episode.EnumTypeActiviteEpisodeItem.SOCIAL
            Case Else
                Item = ""
        End Select

        Return Item
    End Function

    Public Function GetCodeTypeActiviteByItem(Item As String) As String
        Dim Code As String
        Select Case Item
            Case Episode.EnumTypeActiviteEpisodeItem.PATHOLOGIE_AIGUE
                Code = Episode.EnumTypeActiviteEpisodeCode.PATHOLOGIE_AIGUE
            Case Episode.EnumTypeActiviteEpisodeItem.PREVENTION_AUTRE
                Code = Episode.EnumTypeActiviteEpisodeCode.PREVENTION_AUTRE
            Case Episode.EnumTypeActiviteEpisodeItem.PREVENTION_ENFANT_PRE_SCOLAIRE
                Code = Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE
            Case Episode.EnumTypeActiviteEpisodeItem.PREVENTION_ENFANT_SCOLAIRE
                Code = Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE
            Case Episode.EnumTypeActiviteEpisodeItem.PREVENTION_SUIVI_GROSSESSE
                Code = Episode.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GROSSESSE
            Case Episode.EnumTypeActiviteEpisodeItem.PREVENTION_SUIVI_GYNECOLOGIQUE
                Code = Episode.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GYNECOLOGIQUE
            Case Episode.EnumTypeActiviteEpisodeItem.SUIVI_CHRONIQUE
                Code = Episode.EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE
            Case Episode.EnumTypeActiviteEpisodeItem.SOCIAL
                Code = Episode.EnumTypeActiviteEpisodeCode.SOCIAL
            Case Else
                Code = ""
        End Select

        Return Code
    End Function

    Public Function GetEpisodeById(episodeId As Integer) As Episode
        Dim episode As Episode
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_episode WHERE episode_id = @id"
            command.Parameters.AddWithValue("@id", episodeId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    episode = BuildBean(reader)
                Else
                    Throw New ArgumentException("épisode inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return episode
    End Function

    Private Function BuildBean(reader As SqlDataReader) As Episode
        Dim episode As New Episode With {
            .Id = reader("episode_id"),
            .PatientId = Coalesce(reader("patient_id"), 0),
            .Type = Coalesce(reader("type"), ""),
            .TypeActivite = Coalesce(reader("type_activite"), ""),
            .TypeProfil = Coalesce(reader("type_profil"), ""),
            .DescriptionActivite = Coalesce(reader("description_activite"), ""),
            .Commentaire = Coalesce(reader("commentaire"), ""),
            .ObservationMedical = Coalesce(reader("observation_medical"), ""),
            .ObservationParamedical = Coalesce(reader("observation_paramedical"), ""),
            .Decision = Coalesce(reader("decision"), ""),
            .ConclusionIdeType = Coalesce(reader("conclusion_ide_type"), ""),
            .ConclusionMedConsigneDrcId = Coalesce(reader("conclusion_med_consigne_drc_id"), 0),
            .ConclusionMedConsigneDenomination = Coalesce(reader("conclusion_med_consigne_denomination"), ""),
            .ConclusionMedContexte1DrcId = Coalesce(reader("conclusion_med_contexte1_drc_id"), 0),
            .ConclusionMedContexte1AntecedentId = Coalesce(reader("conclusion_med_contexte1_antecedent_id"), 0),
            .ConclusionMedContexte2DrcId = Coalesce(reader("conclusion_med_contexte2_drc_id"), 0),
            .ConclusionMedContexte2AntecedentId = Coalesce(reader("conclusion_med_contexte2_antecedent_id"), 0),
            .ConclusionMedContexte3DrcId = Coalesce(reader("conclusion_med_contexte3_drc_id"), 0),
            .ConclusionMedContexte3AntecedentId = Coalesce(reader("conclusion_med_contexte3_antecedent_id"), 0),
            .UserCreation = Coalesce(reader("user_creation"), 0),
            .DateCreation = Coalesce(reader("date_creation"), Nothing),
            .UserModification = Coalesce(reader("user_modification"), 0),
            .DateModification = Coalesce(reader("date_modification"), Nothing),
            .Etat = Coalesce(reader("etat"), ""),
            .Inactif = Coalesce(reader("inactif"), False)
        }
        Return episode
    End Function

    Public Function GetEpisodeEnCoursByPatientId(patientId As Long) As Episode
        Dim con As SqlConnection
        Dim episode As New Episode
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT TOP (1) * FROM oasis.oasis.oa_episode" &
                " WHERE patient_Id = @patientId" &
                " AND etat = @etat" &
                " AND (inactif = 'False' OR inactif is Null)" &
                " AND ([type] = '" & Episode.EnumTypeEpisode.CONSULTATION.ToString & "' OR [type] = '" & Episode.EnumTypeEpisode.VIRTUEL.ToString & "')" &
                " ORDER BY date_creation DESC"

            With command.Parameters
                .AddWithValue("@patientId", patientId)
                .AddWithValue("@etat", Episode.EnumEtatEpisode.EN_COURS.ToString)
            End With

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    episode = BuildBean(reader)
                Else
                    episode.Id = 0
                    episode.PatientId = 0
                    episode.Type = ""
                    episode.TypeActivite = ""
                    episode.TypeProfil = ""
                    episode.DescriptionActivite = ""
                    episode.Commentaire = ""
                    episode.ObservationMedical = ""
                    episode.ObservationParamedical = ""
                    episode.Decision = ""
                    episode.ConclusionIdeType = ""
                    episode.ConclusionMedConsigneDrcId = 0
                    episode.ConclusionMedConsigneDenomination = ""
                    episode.ConclusionMedContexte1DrcId = 0
                    episode.ConclusionMedContexte1AntecedentId = 0
                    episode.ConclusionMedContexte2DrcId = 0
                    episode.ConclusionMedContexte2AntecedentId = 0
                    episode.ConclusionMedContexte3DrcId = 0
                    episode.ConclusionMedContexte3AntecedentId = 0
                    episode.UserCreation = 0
                    episode.DateCreation = Nothing
                    episode.UserModification = 0
                    episode.DateModification = Nothing
                    episode.Etat = ""
                    episode.Inactif = False
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return episode
    End Function

    Public Function GetAllEpisodeByPatient(patientId As Integer) As List(Of Episode)
        Dim con As SqlConnection = GetConnection()
        Dim episodes As List(Of Episode) = New List(Of Episode)
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_episode" &
                    " WHERE patient_id = @patientId" &
                    " ORDER BY episode_id DESC"
            command.Parameters.AddWithValue("@patientId", patientId)
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    episodes.Add(BuildBean(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return episodes
    End Function

    Public Function GetAllEpisodeByPatient(patientId As Long, dateDebut As Date, dateFin As Date, ligneDeVie As LigneDeVie) As DataTable
        Dim SQLString, ClauseWhereString, TypeEpisodeString, ActiviteEpisodeString, ProfilEpisodeString, OrderByString As String
        Dim Parametre1String, Parametre2String, Parametre3String, Parametre4String, Parametre5String As String
        Dim RechercherTypeEpisode, RechercherActiviteEpisode, RechercherprofilEpisode As Boolean
        Dim dateDebutRecherche As Date = dateDebut.AddDays(1)


        SQLString = "SELECT E.episode_id, patient_id, type, type_activite, description_activite, type_profil," & vbCrLf &
                    " commentaire, date_creation, observation_paramedical, observation_medical, etat, ORDO.oa_ordonnance_id, ORDO.oa_ordonnance_date_validation" & vbCrLf

        Parametre1String = ""
        If ligneDeVie.ParametreId1 <> 0 Then
            Parametre1String = ",(SELECT TOP (1) valeur FROM oasis.oa_episode_parametre PE WHERE PE.parametre_id = " & ligneDeVie.ParametreId1.ToString &
                                " AND PE.episode_id = E.episode_id) as ValeurParam1" & vbCrLf
        End If

        Parametre2String = ""
        If ligneDeVie.ParametreId2 <> 0 Then
            Parametre2String = ",(SELECT TOP (1) valeur FROM oasis.oa_episode_parametre PE WHERE PE.parametre_id = " & ligneDeVie.ParametreId2.ToString &
                                " AND PE.episode_id = E.episode_id) as ValeurParam2" & vbCrLf
        End If

        Parametre3String = ""
        If ligneDeVie.ParametreId3 <> 0 Then
            Parametre3String = ",(SELECT TOP (1) valeur FROM oasis.oa_episode_parametre PE WHERE PE.parametre_id = " & ligneDeVie.ParametreId3.ToString &
                                " AND PE.episode_id = E.episode_id) as ValeurParam3" & vbCrLf
        End If

        Parametre4String = ""
        If ligneDeVie.ParametreId4 <> 0 Then
            Parametre4String = ",(SELECT TOP (1) valeur FROM oasis.oa_episode_parametre PE WHERE PE.parametre_id = " & ligneDeVie.ParametreId4.ToString &
                                " AND PE.episode_id = E.episode_id) as ValeurParam4" & vbCrLf
        End If

        Parametre5String = ""
        If ligneDeVie.ParametreId5 <> 0 Then
            Parametre5String = ",(SELECT TOP (1) valeur FROM oasis.oa_episode_parametre PE WHERE PE.parametre_id = " & ligneDeVie.ParametreId5.ToString &
                                " AND PE.episode_id = E.episode_id) as ValeurParam5" & vbCrLf
        End If


        'Début Claude WHERE
        ClauseWhereString = " FROM oasis.oa_episode E" & vbCrLf &
                    " OUTER APPLY (Select TOP (1) * FROM oasis.oasis.oa_patient_ordonnance" &
                        " WHERE oa_ordonnance_episode_id = E.episode_id" &
                        " AND (oa_ordonnance_inactif = 'False' OR oa_ordonnance_inactif is NULL)) AS ORDO" &
                    " WHERE patient_id = " & patientId.ToString & vbCrLf &
                    " AND (inactif = 'False' OR inactif is Null)" & vbCrLf &
                    " AND date_creation <= '" & dateDebutRecherche.ToString("yyyy-MM-dd") & "'" & vbCrLf &
                    " AND date_creation >= '" & dateFin.Date.ToString("yyyy-MM-dd") & "'" & vbCrLf

        'Type
        TypeEpisodeString = " AND [type] IN ('"
        If ligneDeVie.TypeConsultation = True Then
            RechercherTypeEpisode = True
            TypeEpisodeString += Episode.EnumTypeEpisode.CONSULTATION.ToString & "'"
        End If
        If ligneDeVie.TypeVirtuel = True Then
            If RechercherTypeEpisode = True Then
                TypeEpisodeString += ", '"
            End If
            RechercherTypeEpisode = True
            TypeEpisodeString += Episode.EnumTypeEpisode.VIRTUEL.ToString & "'"
        End If
        If ligneDeVie.TypeParametre = True Then
            If RechercherTypeEpisode = True Then
                TypeEpisodeString += ", '"
            End If
            RechercherTypeEpisode = True
            TypeEpisodeString += Episode.EnumTypeEpisode.PARAMETRE.ToString & "'"
        End If
        If RechercherTypeEpisode = True Then
            TypeEpisodeString += ")" & vbCrLf
        End If

        'Profil
        ProfilEpisodeString = " AND type_profil IN ('"
        If ligneDeVie.ProfilMedical = True Then
            RechercherprofilEpisode = True
            ProfilEpisodeString += ProfilDao.EnumProfilType.MEDICAL.ToString & "'"
        End If
        If ligneDeVie.ProfilParamedical = True Then
            If RechercherprofilEpisode = True Then
                ProfilEpisodeString += ", '"
            End If
            RechercherprofilEpisode = True
            ProfilEpisodeString += ProfilDao.EnumProfilType.PARAMEDICAL.ToString & "'"
        End If
        If ligneDeVie.ProfilPatient = True Then
            If RechercherprofilEpisode = True Then
                ProfilEpisodeString += ", '"
            End If
            RechercherprofilEpisode = True
            ProfilEpisodeString += ProfilDao.EnumProfilType.PATIENT.ToString & "'"
        End If
        If RechercherprofilEpisode = True Then
            ProfilEpisodeString += ")" & vbCrLf
        End If

        'Activité
        ActiviteEpisodeString = " AND type_activite IN ('"
        If ligneDeVie.ActivitePathologieAigue = True Then
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += Episode.EnumTypeActiviteEpisodeCode.PATHOLOGIE_AIGUE & "'"
        End If
        If ligneDeVie.ActivitePreventionAutre = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += Episode.EnumTypeActiviteEpisodeCode.PREVENTION_AUTRE & "'"
        End If
        If ligneDeVie.ActivitePreventionEnfantPreScolaire = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE & "'"
        End If
        If ligneDeVie.ActivitePreventionEnfantScolaire = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE & "'"
        End If
        If ligneDeVie.ActiviteSocial = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += Episode.EnumTypeActiviteEpisodeCode.SOCIAL & "'"
        End If
        If ligneDeVie.ActiviteSuiviChronique = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += Episode.EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE & "'"
        End If
        If ligneDeVie.ActiviteSuiviGrossesse = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += Episode.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GROSSESSE & "'"
        End If
        If ligneDeVie.ActiviteSuiviGyncologique = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += Episode.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GYNECOLOGIQUE & "'"
        End If
        If ligneDeVie.TypeParametre = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += Episode.EnumTypeEpisode.PARAMETRE.ToString & "'"
        End If
        If ligneDeVie.TypeVirtuel = True Then
            If RechercherTypeEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += "'"
        End If
        If RechercherActiviteEpisode = True Then
            ActiviteEpisodeString += ")" & vbCrLf
        End If

        'Order by
        OrderByString = " ORDER BY date_creation DESC, episode_id DESC"

        If ligneDeVie.ParametreId1 <> 0 Then
            SQLString += Parametre1String
        End If

        If ligneDeVie.ParametreId2 <> 0 Then
            SQLString += Parametre2String
        End If

        If ligneDeVie.ParametreId3 <> 0 Then
            SQLString += Parametre3String
        End If

        If ligneDeVie.ParametreId4 <> 0 Then
            SQLString += Parametre4String
        End If

        If ligneDeVie.ParametreId5 <> 0 Then
            SQLString += Parametre5String
        End If

        SQLString += ClauseWhereString

        If RechercherTypeEpisode = True Then
            SQLString += TypeEpisodeString
        End If

        If RechercherprofilEpisode = True Then
            SQLString += ProfilEpisodeString
        End If

        If RechercherActiviteEpisode = True Then
            SQLString += ActiviteEpisodeString
        End If

        SQLString += OrderByString

        Dim ParcoursDataTable As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim ParcoursDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ParcoursDataAdapter
                ParcoursDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Try
                    ParcoursDataAdapter.Fill(ParcoursDataTable)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally
                    con.Close()
                End Try
            End Using
        End Using

        Return ParcoursDataTable
    End Function

    Public Function GetAllEpisodeEnCours() As DataTable
        Dim SQLString As String
        SQLString = "SELECT E.episode_id, E.patient_id, E.[type], type_activite, type_profil, commentaire, user_creation, date_creation," &
                    " U.oa_utilisateur_prenom, U.oa_utilisateur_nom, P.oa_patient_site_id, P.oa_patient_nom," &
                    " P.oa_patient_prenom, P.oa_patient_date_naissance, TACHE.nature, TACHE.destinataire_fonction_id, TACHE.etat, TACHE.oa_r_fonction_designation," &
                    " TACHE.oa_utilisateur_prenom, TACHE.oa_utilisateur_nom, TACHE.oa_r_fonction_type, TACHE.emetteur_commentaire, TACHE.priorite" &
                    " FROM oasis.oa_episode E" &
                    " LEFT JOIN oasis.oa_patient P ON P.oa_patient_id = E.patient_id" &
                    " LEFT JOIN oasis.oa_utilisateur U ON U.oa_utilisateur_id = user_creation" &
                    " OUTER APPLY (Select TOP (1) * FROM oasis.oasis.oa_tache" &
                        " LEFT JOIN oasis.oasis.oa_r_fonction ON destinataire_fonction_id = oa_r_fonction_id" &
                        " LEFT JOIN oasis.oasis.oa_utilisateur ON oa_utilisateur_id = traite_user_id" &
                        " WHERE episode_Id = E.episode_id" &
                        " AND (etat = '" & Tache.EtatTache.EN_ATTENTE.ToString() & "' OR etat = '" & Tache.EtatTache.EN_COURS.ToString() & "')" &
                        " AND [type] = '" & Tache.TypeTache.AVIS_EPISODE.ToString() & "'" &
                        " AND categorie = 'SOIN') AS TACHE" &
                    " WHERE E.etat = 'EN_COURS'" &
                    " AND (E.[type] = '" & Episode.EnumTypeEpisode.CONSULTATION.ToString & "' OR E.[type] = '" & Episode.EnumTypeEpisode.VIRTUEL.ToString & "')" &
                    " AND (inactif = 'False' OR inactif is Null)" &
                    " ORDER BY date_creation"

        Dim ParcoursDataTable As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim ParcoursDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ParcoursDataAdapter
                ParcoursDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Try
                    ParcoursDataAdapter.Fill(ParcoursDataTable)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally
                    con.Close()
                End Try
            End Using
        End Using

        Return ParcoursDataTable
    End Function

    Public Function GetAllEpisodeClosedByDate(closeDate As Date) As DataTable
        Dim dateDebutRecherche As Date = closeDate.AddDays(1)
        Dim SQLString As String = "SELECT E.episode_id, E.patient_id, E.[type], type_activite, type_profil, commentaire, user_creation, date_creation," &
                    " U.oa_utilisateur_prenom, U.oa_utilisateur_nom, P.oa_patient_site_id, P.oa_patient_nom, P.oa_patient_INS, P.oa_patient_nir," &
                    " P.oa_patient_prenom, P.oa_patient_date_naissance, TACHE.nature, TACHE.destinataire_fonction_id, TACHE.etat, TACHE.oa_r_fonction_designation," &
                    " S.oa_site_description, TACHE.oa_utilisateur_prenom, TACHE.oa_utilisateur_nom, TACHE.oa_r_fonction_type, TACHE.emetteur_commentaire, TACHE.priorite" &
                    " FROM oasis.oa_episode E" &
                    " LEFT JOIN oasis.oa_patient P ON P.oa_patient_id = E.patient_id" &
                    " LEFT JOIN oasis.oa_utilisateur U ON U.oa_utilisateur_id = user_creation" &
                    " LEFT JOIN oasis.oa_site S ON S.oa_site_id = P.oa_patient_site_id" &
                    " OUTER APPLY (Select TOP (1) * FROM oasis.oasis.oa_tache" &
                        " LEFT JOIN oasis.oasis.oa_r_fonction ON destinataire_fonction_id = oa_r_fonction_id" &
                        " LEFT JOIN oasis.oasis.oa_utilisateur ON oa_utilisateur_id = traite_user_id" &
                        " WHERE episode_Id = E.episode_id" &
                        " AND (etat = '" & Tache.EtatTache.EN_ATTENTE.ToString() & "' OR etat = '" & Tache.EtatTache.EN_COURS.ToString() & "')" &
                        " AND [type] = '" & Tache.TypeTache.AVIS_EPISODE.ToString() & "'" &
                        " AND categorie = 'SOIN') AS TACHE" &
                    " WHERE E.etat = '" & Episode.EnumEtatEpisode.CLOTURE.ToString & "'" &
                    " AND E.date_modification <= '" & dateDebutRecherche.ToString("yyyy-MM-dd") & "'" &
                    " AND E.date_modification >= '" & closeDate.Date.ToString("yyyy-MM-dd") & "'" &
                    " AND E.[type] = '" & Episode.EnumTypeEpisode.CONSULTATION.ToString & "'" &
                    " AND (inactif = 'False' OR inactif is Null)" &
                    " ORDER BY date_modification"

        Dim ParcoursDataTable As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim ParcoursDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ParcoursDataAdapter
                ParcoursDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Try
                    ParcoursDataAdapter.Fill(ParcoursDataTable)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally
                    con.Close()
                End Try
            End Using
        End Using

        Return ParcoursDataTable
    End Function

    Public Function GetAllEpisodeEnAttenteValidation() As DataTable
        Dim SQLString As String
        SQLString = "SELECT E.episode_id, E.patient_id, E.[type], type_activite, date_creation," &
                    " P.oa_patient_site_id, S.oa_site_description, P.oa_patient_nom, P.oa_patient_prenom, P.oa_patient_date_naissance," &
                    " ORDO.oa_ordonnance_id, SSP.TotalSSP, SER.totalSER" &
                    " FROM oasis.oa_episode E" &
                    " LEFT JOIN oasis.oa_patient P ON P.oa_patient_id = E.patient_id" &
                    " LEFT JOIN oasis.oa_site S ON P.oa_patient_site_id = S.oa_site_id" &
                    " OUTER APPLY (Select TOP (1) * FROM oasis.oasis.oa_patient_ordonnance" &
                        " WHERE oa_ordonnance_episode_id = E.episode_id" &
                        " AND (oa_ordonnance_inactif = 'False' OR oa_ordonnance_inactif is NULL)" &
                        " AND oa_ordonnance_date_validation is NULL) AS ORDO" &
                    " OUTER APPLY (Select COUNT(*) FROM oasis.oasis.oa_sous_episode SP" &
                        " WHERE SP.episode_id = E.episode_id" &
                        " AND is_inactif = 'False'" &
                        " AND horodate_validate is NULL) AS SSP(TotalSSP)" &
                    " OUTER APPLY (Select COUNT(*) FROM oasis.oasis.oa_sous_episode_reponse SER" &
                        " WHERE SER.episode_id = E.episode_id" &
                        " AND (validate_state = '!' Or validate_state = 'm')) AS SER(totalSER)" &
                    " WHERE SER.totalSER > 0 OR (E.etat = '" & Episode.EnumEtatEpisode.EN_COURS.ToString & "' OR E.etat = '" & Episode.EnumEtatEpisode.CLOTURE.ToString & "')" &
                    " AND (E.[type] = '" & Episode.EnumTypeEpisode.CONSULTATION.ToString & "' OR E.[type] = '" & Episode.EnumTypeEpisode.VIRTUEL.ToString & "')" &
                    " AND (inactif = 'False' OR inactif is Null)" &
                    " AND (ORDO.oa_ordonnance_id is not NULL OR SSP.TotalSSP > 0)" &
                    " ORDER BY date_creation"

        Dim ParcoursDataTable As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim ParcoursDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ParcoursDataAdapter
                ParcoursDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Try
                    ParcoursDataAdapter.Fill(ParcoursDataTable)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally
                    con.Close()
                End Try
            End Using
        End Using

        Return ParcoursDataTable
    End Function

    Public Function CreateEpisode(episode As Episode, utilisateurId As Long) As Integer
        'Dim nbcreate As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim episodeIdCree As Integer = 0
        Dim CodeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String
        If episode.Type = Episode.EnumTypeEpisode.PARAMETRE.ToString Then
            SQLstring = " INSERT INTO oasis.oa_episode" &
            " (patient_id, type, type_activite, type_profil, description_activite, commentaire," &
            " user_creation, date_creation, date_modification, etat)" &
            " VALUES (@patientId, @type, @typeActivite, @typeProfil, @descriptionActivite, @commentaire," &
            " @userCreation, '" & episode.DateCreation.ToString("yyyy-MM-dd HH:mm:ss") & "',  @dateModification, '" & Episode.EnumEtatEpisode.CLOTURE.ToString & "'); SELECT SCOPE_IDENTITY()"
        Else
            SQLstring = "IF Not EXISTS (SELECT 1 FROM oasis.oa_episode WHERE patient_id = @patientId And etat = @etat)" &
            " INSERT INTO oasis.oa_episode" &
            " (patient_id, type, type_activite, type_profil, description_activite, commentaire," &
            " user_creation, date_creation, etat)" &
            " VALUES (@patientId, @type, @typeActivite, @typeProfil, @descriptionActivite, @commentaire," &
            " @userCreation, @dateCreation, @etat); SELECT SCOPE_IDENTITY()"
        End If

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@patientId", episode.PatientId)
            .AddWithValue("@type", episode.Type)
            .AddWithValue("@typeActivite", episode.TypeActivite)
            .AddWithValue("@typeProfil", episode.TypeProfil)
            .AddWithValue("@descriptionActivite", episode.DescriptionActivite)
            .AddWithValue("@commentaire", episode.Commentaire)
            .AddWithValue("@userCreation", utilisateurId)
            .AddWithValue("@dateCreation", Date.Now)
            .AddWithValue("@dateModification", Date.Now)
            .AddWithValue("@etat", Episode.EnumEtatEpisode.EN_COURS.ToString)
        End With

        Try
            da.InsertCommand = cmd
            episodeIdCree = Coalesce(da.InsertCommand.ExecuteScalar(), 0)
            If episodeIdCree <= 0 Then
                Throw New Exception("Collision: Un épisode en cours existe déjà pour ce patient")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
            CodeRetour = False
        Finally
            con.Close()
        End Try

        Return episodeIdCree
    End Function

    Public Function ModificationEpisode(episode As Episode, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "UPDATE oasis.oa_episode SET" &
        " patient_id = @patientId," &
        " type = @type," &
        " type_activite = @typeActivite," &
        " type_profil = @typeProfil," &
        " description_activite = @descriptionActivite," &
        " commentaire = @commentaire," &
        " observation_medical = @observationMedical," &
        " observation_paramedical = @observationParamedical," &
        " decision = @decision," &
        " conclusion_ide_type = @conclusionIdeType," &
        " conclusion_med_consigne_drc_id = @conclusionMedConsigneDrcId," &
        " conclusion_med_consigne_denomination = @conclusionMedConsigneDenomination," &
        " conclusion_med_contexte1_drc_id = @conclusionMedContexte1DrcId," &
        " conclusion_med_contexte1_antecedent_id = @conclusionMedContexte1AntecedentId," &
        " conclusion_med_contexte2_drc_id = @conclusionMedContexte2DrcId," &
        " conclusion_med_contexte2_antecedent_id = @conclusionMedContexte2AntecedentId," &
        " conclusion_med_contexte3_drc_id = @conclusionMedContexte3DrcId," &
        " conclusion_med_contexte3_antecedent_id = @conclusionMedContexte3AntecedentId," &
        " user_modification = @userModification," &
        " date_modification = @dateModification," &
        " etat = @etat," &
        " inactif = @inactif" &
        " WHERE episode_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@patientId", episode.PatientId)
            .AddWithValue("@type", episode.Type)
            .AddWithValue("@typeActivite", episode.TypeActivite)
            .AddWithValue("@typeprofil", episode.TypeProfil)
            .AddWithValue("@descriptionActivite", episode.DescriptionActivite)
            .AddWithValue("@commentaire", episode.Commentaire)
            .AddWithValue("@observationMedical", episode.ObservationMedical)
            .AddWithValue("@observationParamedical", episode.ObservationParamedical)
            .AddWithValue("@decision", episode.Decision)
            .AddWithValue("@conclusionIdeType", episode.ConclusionIdeType)
            .AddWithValue("@conclusionMedConsigneDrcId", episode.ConclusionMedConsigneDrcId)
            .AddWithValue("@conclusionMedConsigneDenomination", episode.ConclusionMedConsigneDenomination)
            .AddWithValue("@conclusionMedContexte1DrcId", episode.ConclusionMedContexte1DrcId)
            .AddWithValue("@conclusionMedContexte1AntecedentId", episode.ConclusionMedContexte1AntecedentId)
            .AddWithValue("@conclusionMedContexte2DrcId", episode.ConclusionMedContexte2DrcId)
            .AddWithValue("@conclusionMedContexte2AntecedentId", episode.ConclusionMedContexte2AntecedentId)
            .AddWithValue("@conclusionMedContexte3DrcId", episode.ConclusionMedContexte3DrcId)
            .AddWithValue("@conclusionMedContexte3AntecedentId", episode.ConclusionMedContexte3AntecedentId)
            .AddWithValue("@userModification", userLog.UtilisateurId)
            .AddWithValue("@dateModification", Date.Now())
            .AddWithValue("@etat", episode.Etat)
            .AddWithValue("@inactif", episode.Inactif)
            .AddWithValue("@Id", episode.Id)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function MajEpisodeConclusionMedicale(episodeId As Long) As Boolean
        Dim conclusionMedicale As String = ""
        Dim PremierPassage As Boolean = True

        Dim episodeContexteDao As New EpisodeContexteDao
        Dim dt As DataTable
        dt = episodeContexteDao.GetAllEpisodeContexteByEpisodeId(episodeId)
        If dt.Rows.Count > 0 Then
            Dim rowCount As Integer = dt.Rows.Count - 1
            For i = 0 To rowCount Step 1
                Dim contexteDescription As String
                contexteDescription = Coalesce(dt.Rows(i)("oa_antecedent_description"), "")
                If PremierPassage = True Then
                    PremierPassage = False
                    conclusionMedicale += contexteDescription
                Else
                    conclusionMedicale += "; " & contexteDescription
                End If
            Next
        End If

        Dim codeRetour As Boolean = True

        If conclusionMedicale <> "" Then
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Dim con As SqlConnection = GetConnection()

            Dim SQLstring As String = "UPDATE oasis.oa_episode SET" &
            " observation_medical = @observationMedical" &
            " WHERE episode_id = @Id"

            Dim cmd As New SqlCommand(SQLstring, con)

            With cmd.Parameters
                .AddWithValue("@observationMedical", conclusionMedicale)
                .AddWithValue("@Id", episodeId)
            End With

            Try
                da.UpdateCommand = cmd
                da.UpdateCommand.ExecuteNonQuery()
            Catch ex As Exception
                Throw New Exception(ex.Message)
                codeRetour = False
            Finally
                con.Close()
            End Try
        End If

        Return codeRetour
    End Function

End Class
