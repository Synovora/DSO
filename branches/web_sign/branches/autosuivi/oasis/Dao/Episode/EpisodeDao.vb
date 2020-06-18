Imports System.Data.SqlClient
Imports Oasis_Common
Public Class EpisodeDao
    Inherits StandardDao

    Public Enum EnumTypeConclusionParamedicale
        ROLE_PROPRE
        SUR_PROTOCOLE
        DEMANDE_AVIS
    End Enum

    Public Structure EnumTypeActiviteEpisodeItem
        Const PATHOLOGIE_AIGUE = "Pathologie Aiguë"
        Const PREVENTION_AUTRE = "Autre prévention"
        Const PREVENTION_ENFANT_PRE_SCOLAIRE = "Prévention de l'enfant en âge pré-scolaire (0 à 40 mois)"
        Const PREVENTION_ENFANT_SCOLAIRE = "Prévention de l'enfant en âge scolaire (à partir de 3 ans)"
        Const PREVENTION_SUIVI_GROSSESSE = "Suivi grossesse"
        Const PREVENTION_SUIVI_GYNECOLOGIQUE = "Suivi gynécologique"
        Const SOCIAL = "Social"
        Const SUIVI_CHRONIQUE = "Suivi chronique"
    End Structure

    Public Structure EnumTypeActiviteEpisodeCode
        Const PATHOLOGIE_AIGUE = "PATHOLOGIE_AIGUE"
        Const PREVENTION_AUTRE = "PREVENTION_AUTRE"
        Const PREVENTION_ENFANT_PRE_SCOLAIRE = "PREVENTION_ENFANT_PRE_SCOLAIRE"
        Const PREVENTION_ENFANT_SCOLAIRE = "PREVENTION_ENFANT_SCOLAIRE"
        Const PREVENTION_SUIVI_GROSSESSE = "PREVENTION_SUIVI_GROSSESSE"
        Const PREVENTION_SUIVI_GYNECOLOGIQUE = "PREVENTION_SUIVI_GYNECOLOGIQUE"
        Const SOCIAL = "SOCIAL"
        Const SUIVI_CHRONIQUE = "SUIVI_CHRONIQUE"
    End Structure

    Public Enum EnumEtatEpisode
        EN_COURS
        CLOTURE
        ANNULE
    End Enum

    Public Enum EnumTypeEpisode
        CONSULTATION
        VIRTUEL
        PARAMETRE
    End Enum

    Friend Function GetItemTypeActiviteByCode(Code As String) As String
        Dim Item As String
        Select Case Code
            Case EnumTypeActiviteEpisodeCode.PATHOLOGIE_AIGUE
                Item = EnumTypeActiviteEpisodeItem.PATHOLOGIE_AIGUE
            Case EnumTypeActiviteEpisodeCode.PREVENTION_AUTRE
                Item = EnumTypeActiviteEpisodeItem.PREVENTION_AUTRE
            Case EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE
                Item = EnumTypeActiviteEpisodeItem.PREVENTION_ENFANT_PRE_SCOLAIRE
            Case EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE
                Item = EnumTypeActiviteEpisodeItem.PREVENTION_ENFANT_SCOLAIRE
            Case EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GROSSESSE
                Item = EnumTypeActiviteEpisodeItem.PREVENTION_SUIVI_GROSSESSE
            Case EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GYNECOLOGIQUE
                Item = EnumTypeActiviteEpisodeItem.PREVENTION_SUIVI_GYNECOLOGIQUE
            Case EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE
                Item = EnumTypeActiviteEpisodeItem.SUIVI_CHRONIQUE
            Case EnumTypeActiviteEpisodeCode.SOCIAL
                Item = EnumTypeActiviteEpisodeItem.SOCIAL
            Case Else
                Item = ""
        End Select

        Return Item
    End Function

    Friend Function GetCodeTypeActiviteByItem(Item As String) As String
        Dim Code As String
        Select Case Item
            Case EnumTypeActiviteEpisodeItem.PATHOLOGIE_AIGUE
                Code = EnumTypeActiviteEpisodeCode.PATHOLOGIE_AIGUE
            Case EnumTypeActiviteEpisodeItem.PREVENTION_AUTRE
                Code = EnumTypeActiviteEpisodeCode.PREVENTION_AUTRE
            Case EnumTypeActiviteEpisodeItem.PREVENTION_ENFANT_PRE_SCOLAIRE
                Code = EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE
            Case EnumTypeActiviteEpisodeItem.PREVENTION_ENFANT_SCOLAIRE
                Code = EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE
            Case EnumTypeActiviteEpisodeItem.PREVENTION_SUIVI_GROSSESSE
                Code = EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GROSSESSE
            Case EnumTypeActiviteEpisodeItem.PREVENTION_SUIVI_GYNECOLOGIQUE
                Code = EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GYNECOLOGIQUE
            Case EnumTypeActiviteEpisodeItem.SUIVI_CHRONIQUE
                Code = EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE
            Case EnumTypeActiviteEpisodeItem.SOCIAL
                Code = EnumTypeActiviteEpisodeCode.SOCIAL
            Case Else
                Code = ""
        End Select

        Return Code
    End Function

    Friend Function GetEpisodeById(episodeId As Integer) As Episode
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
        Dim episode As New Episode

        episode.Id = reader("episode_id")
        episode.PatientId = Coalesce(reader("patient_id"), 0)
        episode.Type = Coalesce(reader("type"), "")
        episode.TypeActivite = Coalesce(reader("type_activite"), "")
        episode.TypeProfil = Coalesce(reader("type_profil"), "")
        episode.DescriptionActivite = Coalesce(reader("description_activite"), "")
        episode.Commentaire = Coalesce(reader("commentaire"), "")
        episode.ObservationMedical = Coalesce(reader("observation_medical"), "")
        episode.ObservationParamedical = Coalesce(reader("observation_paramedical"), "")
        episode.Decision = Coalesce(reader("decision"), "")
        episode.ConclusionIdeType = Coalesce(reader("conclusion_ide_type"), "")
        episode.ConclusionMedConsigneDrcId = Coalesce(reader("conclusion_med_consigne_drc_id"), 0)
        episode.ConclusionMedConsigneDenomination = Coalesce(reader("conclusion_med_consigne_denomination"), "")
        episode.ConclusionMedContexte1DrcId = Coalesce(reader("conclusion_med_contexte1_drc_id"), 0)
        episode.ConclusionMedContexte1AntecedentId = Coalesce(reader("conclusion_med_contexte1_antecedent_id"), 0)
        episode.ConclusionMedContexte2DrcId = Coalesce(reader("conclusion_med_contexte2_drc_id"), 0)
        episode.ConclusionMedContexte2AntecedentId = Coalesce(reader("conclusion_med_contexte2_antecedent_id"), 0)
        episode.ConclusionMedContexte3DrcId = Coalesce(reader("conclusion_med_contexte3_drc_id"), 0)
        episode.ConclusionMedContexte3AntecedentId = Coalesce(reader("conclusion_med_contexte3_antecedent_id"), 0)
        episode.UserCreation = Coalesce(reader("user_creation"), 0)
        episode.DateCreation = Coalesce(reader("date_creation"), Nothing)
        episode.UserModification = Coalesce(reader("user_modification"), 0)
        episode.DateModification = Coalesce(reader("date_modification"), Nothing)
        episode.Etat = Coalesce(reader("etat"), "")
        episode.Inactif = Coalesce(reader("inactif"), False)
        Return episode
    End Function

    Friend Function GetEpisodeEnCoursByPatientId(patientId As Long) As Episode
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
                " AND ([type] = '" & EnumTypeEpisode.CONSULTATION.ToString & "' OR [type] = '" & EnumTypeEpisode.VIRTUEL.ToString & "')" &
                " ORDER BY date_creation DESC"

            With command.Parameters
                .AddWithValue("@patientId", patientId)
                .AddWithValue("@etat", EnumEtatEpisode.EN_COURS.ToString)
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

    Friend Function GetAllEpisodeByPatient(patientId As Long, dateDebut As Date, dateFin As Date, ligneDeVie As LigneDeVie) As DataTable
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
            TypeEpisodeString += EpisodeDao.EnumTypeEpisode.CONSULTATION.ToString & "'"
        End If
        If ligneDeVie.TypeVirtuel = True Then
            If RechercherTypeEpisode = True Then
                TypeEpisodeString += ", '"
            End If
            RechercherTypeEpisode = True
            TypeEpisodeString += EpisodeDao.EnumTypeEpisode.VIRTUEL.ToString & "'"
        End If
        If ligneDeVie.TypeParametre = True Then
            If RechercherTypeEpisode = True Then
                TypeEpisodeString += ", '"
            End If
            RechercherTypeEpisode = True
            TypeEpisodeString += EpisodeDao.EnumTypeEpisode.PARAMETRE.ToString & "'"
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
        If RechercherprofilEpisode = True Then
            ProfilEpisodeString += ")" & vbCrLf
        End If

        'Activité
        ActiviteEpisodeString = " AND type_activite IN ('"
        If ligneDeVie.ActivitePathologieAigue = True Then
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += EpisodeDao.EnumTypeActiviteEpisodeCode.PATHOLOGIE_AIGUE & "'"
        End If
        If ligneDeVie.ActivitePreventionAutre = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_AUTRE & "'"
        End If
        If ligneDeVie.ActivitePreventionEnfantPreScolaire = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE & "'"
        End If
        If ligneDeVie.ActivitePreventionEnfantScolaire = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE & "'"
        End If
        If ligneDeVie.ActiviteSocial = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += EpisodeDao.EnumTypeActiviteEpisodeCode.SOCIAL & "'"
        End If
        If ligneDeVie.ActiviteSuiviChronique = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += EpisodeDao.EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE & "'"
        End If
        If ligneDeVie.ActiviteSuiviGrossesse = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GROSSESSE & "'"
        End If
        If ligneDeVie.ActiviteSuiviGyncologique = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GYNECOLOGIQUE & "'"
        End If
        If ligneDeVie.TypeParametre = True Then
            If RechercherActiviteEpisode = True Then
                ActiviteEpisodeString += ", '"
            End If
            RechercherActiviteEpisode = True
            ActiviteEpisodeString += EpisodeDao.EnumTypeEpisode.PARAMETRE.ToString & "'"
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

    Friend Function GetAllEpisodeEnCours() As DataTable
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
                        " AND (etat = '" & TacheDao.EtatTache.EN_ATTENTE.ToString() & "' OR etat = '" & TacheDao.EtatTache.EN_COURS.ToString() & "')" &
                        " AND [type] = '" & TacheDao.TypeTache.AVIS_EPISODE.ToString() & "'" &
                        " AND categorie = 'SOIN') AS TACHE" &
                    " WHERE E.etat = 'EN_COURS'" &
                    " AND (E.[type] = '" & EnumTypeEpisode.CONSULTATION.ToString & "' OR E.[type] = '" & EnumTypeEpisode.VIRTUEL.ToString & "')" &
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

    Friend Function GetAllEpisodeEnAttenteValidation() As DataTable
        Dim SQLString As String
        SQLString = "SELECT E.episode_id, E.patient_id, E.[type], type_activite, date_creation," &
                    " P.oa_patient_site_id, S.oa_site_description, P.oa_patient_nom, P.oa_patient_prenom, P.oa_patient_date_naissance," &
                    " ORDO.oa_ordonnance_id, SSP.TotalSSP" &
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
                    " WHERE (E.etat = '" & EnumEtatEpisode.EN_COURS.ToString & "' OR E.etat = '" & EnumEtatEpisode.CLOTURE.ToString & "')" &
                    " AND (E.[type] = '" & EnumTypeEpisode.CONSULTATION.ToString & "' OR E.[type] = '" & EnumTypeEpisode.VIRTUEL.ToString & "')" &
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

    Friend Function CreateEpisode(episode As Episode) As Integer
        'Dim nbcreate As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim episodeIdCree As Integer = 0
        Dim CodeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String
        If episode.Type = EpisodeDao.EnumTypeEpisode.PARAMETRE.ToString Then
            SQLstring = " INSERT INTO oasis.oa_episode" &
            " (patient_id, type, type_activite, type_profil, description_activite, commentaire," &
            " user_creation, date_creation, date_modification, etat)" &
            " VALUES (@patientId, @type, @typeActivite, @typeProfil, @descriptionActivite, @commentaire," &
            " @userCreation, '" & episode.DateCreation.ToString("yyyy-MM-dd HH:mm:ss") & "',  @dateModification, '" & EpisodeDao.EnumEtatEpisode.CLOTURE.ToString & "'); SELECT SCOPE_IDENTITY()"
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
            .AddWithValue("@userCreation", userLog.UtilisateurId)
            .AddWithValue("@dateCreation", Date.Now)
            .AddWithValue("@dateModification", Date.Now)
            .AddWithValue("@etat", EpisodeDao.EnumEtatEpisode.EN_COURS.ToString)
        End With

        Try
            da.InsertCommand = cmd
            episodeIdCree = Coalesce(da.InsertCommand.ExecuteScalar(), 0)
            If episodeIdCree <= 0 Then
                Throw New Exception("Collision: Un épisode en cours existe déjà pour ce patient")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            CodeRetour = False
        Finally
            con.Close()
        End Try

        Return episodeIdCree
    End Function

    Friend Function ModificationEpisode(episode As Episode) As Boolean
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
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function CallEpisode(selectedPatient As Patient, rendezVousId As Long, Optional EcransPrecedent As EnumAccesEcranPrecedent = EnumAccesEcranPrecedent.SANS) As Boolean
        Dim IsRendezVousCloture As Boolean = False
        'Tester si l'utilisateur a une fonction de type MEDICAL ou PARAMEDICALE
        If Not (userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString Or userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString) Then
            Dim Message1 As String = "Votre profil de type (" & userLog.TypeProfil & ") ne vous permet pas de gérer un épisode patient, processus annulé"
            Dim Message2 As String = "Les types de profil autorisés sont : " & ProfilDao.EnumProfilType.MEDICAL.ToString() & " et " & ProfilDao.EnumProfilType.PARAMEDICAL.ToString()
            MessageBox.Show(Message1 & vbCrLf & Message2)
            Return False
            Exit Function
        End If

        Dim episodeDao As New EpisodeDao
        Dim episode As Episode
        episode = episodeDao.GetEpisodeEnCoursByPatientId(selectedPatient.patientId)
        If episode.Id = 0 Then
            Using vRadFEpisodeDetailCreation As New RadFEpisodeDetailCreation
                vRadFEpisodeDetailCreation.SelectedPatient = selectedPatient
                vRadFEpisodeDetailCreation.ShowDialog()
                If vRadFEpisodeDetailCreation.CodeRetour = True Then
                    Using vRadFEpisodeDetail As New RadFEpisodeDetail
                        vRadFEpisodeDetail.SelectedEpisodeId = vRadFEpisodeDetailCreation.EpisodeId
                        vRadFEpisodeDetail.SelectedPatient = selectedPatient
                        vRadFEpisodeDetail.RendezVousId = rendezVousId
                        vRadFEpisodeDetail.UtilisateurConnecte = userLog
                        vRadFEpisodeDetail.EcranPrecedent = EcransPrecedent
                        vRadFEpisodeDetail.ShowDialog()
                        IsRendezVousCloture = vRadFEpisodeDetail.IsRendezVousCloture
                    End Using
                End If
            End Using
        Else
            Cursor.Current = Cursors.WaitCursor
            Using vRadFEpisodeDetail As New RadFEpisodeDetail
                vRadFEpisodeDetail.SelectedEpisodeId = episode.Id
                vRadFEpisodeDetail.SelectedPatient = selectedPatient
                vRadFEpisodeDetail.RendezVousId = rendezVousId
                vRadFEpisodeDetail.UtilisateurConnecte = userLog
                vRadFEpisodeDetail.ShowDialog()
                IsRendezVousCloture = vRadFEpisodeDetail.IsRendezVousCloture
            End Using
        End If

            Return IsRendezVousCloture
    End Function

    Friend Function MajEpisodeConclusionMedicale(episodeId As Long) As Boolean
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
                MessageBox.Show(ex.Message)
                codeRetour = False
            Finally
                con.Close()
            End Try
        End If

        Return codeRetour
    End Function

End Class
