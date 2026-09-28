Imports System.Globalization
Imports System.Threading
Imports Oasis_Common

''' <summary>
''' Épisodes de test et ce qui s'y rattache : paramètres, actes paramédicaux,
''' observations, contextes, et le paramétrage (activités, DRC standard, consignes
''' patient) que lisent la création d'épisode et les protocoles collaboratifs.
'''
''' Les épisodes passent par EpisodeDao.CreateEpisode, l'INSERT que la fenêtre de
''' création d'épisode du client lourd (RadFEpisodeDetailCreation) exécute en
''' production. Chaque table qu'un DAO écrit passe par ce DAO ; le SQL brut du
''' domaine est ici, pour ce qu'aucun DAO n'écrit. Quand le schéma réel fera
''' apparaître une colonne obligatoire oubliée, c'est le seul fichier à reprendre.
''' </summary>
Public Module JeuxEpisode

    ''' <summary>
    ''' Épisode ouvert (EN_COURS) de type CONSULTATION pour ce patient, créé comme le
    ''' fait l'application. Renvoie l'id.
    '''
    ''' CreateEpisode refuse un second épisode EN_COURS de type CONSULTATION ou
    ''' VIRTUEL pour le même patient : clôturer le précédent (CloturerEpisode) avant
    ''' d'en créer un autre.
    ''' </summary>
    Function CreerEpisode(patientId As Long, utilisateurId As Long,
                          Optional typeEpisode As String = "CONSULTATION",
                          Optional typeActivite As String = "PATHOLOGIE_AIGUE",
                          Optional typeProfil As String = "MEDICAL",
                          Optional commentaire As String = "Episode de test",
                          Optional descriptionActivite As String = "") As Long
        Dim nouveau As New Episode With {
            .PatientId = patientId,
            .Type = typeEpisode,
            .TypeActivite = typeActivite,
            .TypeProfil = typeProfil,
            .DescriptionActivite = descriptionActivite,
            .Commentaire = commentaire
        }
        Dim dao As New EpisodeDao
        Return CLng(dao.CreateEpisode(nouveau, utilisateurId))
    End Function

    ''' <summary>
    ''' Épisode de saisie de paramètres (type PARAMETRE, créé clôturé) daté de
    ''' dateCreation, comme RadFEpisodeParametresCreation. Renvoie l'id.
    ''' </summary>
    Function CreerEpisodeParametres(patientId As Long, utilisateurId As Long, dateCreation As Date,
                                    Optional typeProfil As String = "MEDICAL") As Long
        Dim nouveau As New Episode With {
            .PatientId = patientId,
            .Type = "PARAMETRE",
            .TypeActivite = "PARAMETRE",
            .TypeProfil = typeProfil,
            .DescriptionActivite = "",
            .Commentaire = "Saisie de parametres",
            .DateCreation = dateCreation,
            .UserCreation = utilisateurId,
            .Etat = "CLOTURE"
        }
        ' Pour ce type, CreateEpisode écrit la date par ToString("yyyy-MM-dd HH:mm:ss"),
        ' dont le séparateur horaire dépend de la culture : celle du poste, fr-FR.
        Dim cultureAvant = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
        Try
            Dim dao As New EpisodeDao
            Return CLng(dao.CreateEpisode(nouveau, utilisateurId))
        Finally
            Thread.CurrentThread.CurrentCulture = cultureAvant
        End Try
    End Function

    ''' <summary>Passe l'épisode à l'état CLOTURE par EpisodeDao.ModificationEpisode, comme la fiche épisode.</summary>
    Sub CloturerEpisode(episodeId As Long, utilisateurId As Long)
        Dim dao As New EpisodeDao
        Dim lu = dao.GetEpisodeById(CInt(episodeId))
        lu.Etat = "CLOTURE"
        dao.ModificationEpisode(lu, New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
    End Sub

    ''' <summary>Marque l'épisode inactif (annulé) par EpisodeDao.ModificationEpisode.</summary>
    Sub DesactiverEpisode(episodeId As Long, utilisateurId As Long)
        Dim dao As New EpisodeDao
        Dim lu = dao.GetEpisodeById(CInt(episodeId))
        lu.Inactif = True
        dao.ModificationEpisode(lu, New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
    End Sub

    ''' <summary>
    ''' Force la date de création d'un épisode. CreateEpisode horodate à l'instant
    ''' présent ; les tests de tri et de fenêtre de dates ont besoin de dates fixes.
    ''' </summary>
    Sub DaterEpisode(episodeId As Long, dateCreation As Date, Optional dateModification As Date? = Nothing)
        Executer("UPDATE oasis.oa_episode SET date_creation = @p0, date_modification = COALESCE(@p1, date_modification)" &
                 " WHERE episode_id = @p2",
                 dateCreation, If(dateModification.HasValue, CObj(dateModification.Value), Nothing), episodeId)
    End Sub

    ''' <summary>
    ''' Contexte patient (ligne de oa_antecedent de type « C »). Mêmes colonnes que
    ''' l'INSERT de ContexteDao.CreationContexte, sans l'historique ni la mise à jour
    ''' de la synthèse qu'elle enchaîne : ces tables relèvent d'autres domaines et ne
    ''' comptent pas pour les DAO d'épisode, qui ne lisent que la description, le
    ''' diagnostic, les dates et la catégorie. Renvoie l'id.
    ''' </summary>
    Function CreerContextePourEpisode(patientId As Long, utilisateurId As Long, description As String,
                                      Optional drcId As Long = 0, Optional episodeId As Long = 0) As Long
        Dim maintenant = Date.Now
        Return CLng(Scalaire(
            "INSERT INTO oasis.oa_antecedent (oa_antecedent_patient_id, oa_antecedent_type, oa_antecedent_drc_id," &
            " oa_antecedent_description, oa_antecedent_date_creation, oa_antecedent_date_modification," &
            " oa_antecedent_utilisateur_creation, oa_antecedent_utilisateur_modification, oa_antecedent_date_debut," &
            " oa_antecedent_niveau, oa_antecedent_nature, oa_antecedent_statut_affichage," &
            " oa_antecedent_statut_affichage_transformation, oa_antecedent_inactif, oa_antecedent_ordre_affichage1," &
            " oa_antecedent_ordre_affichage2, oa_antecedent_ordre_affichage3, oa_antecedent_categorie_contexte," &
            " oa_antecedent_date_fin, oa_antecedent_diagnostic, oa_episode_id)" &
            " VALUES (@p0, 'C', @p1, @p2, @p3, @p3, @p4, 0, @p5, 1, 'Patient', 'P', '', 0, 1, 0, 0, '', @p6, 0, @p7);" &
            " SELECT CAST(SCOPE_IDENTITY() AS BIGINT);",
            patientId, drcId, description, maintenant, utilisateurId, maintenant.Date,
            New Date(2999, 12, 31), episodeId))
    End Function

    ''' <summary>
    ''' Rattache un contexte à un épisode par EpisodeContexteDao.CreateEpisodeContexte,
    ''' qui ne renvoie pas l'id : on le retrouve par le couple épisode et contexte,
    ''' unique par construction. Renvoie l'id.
    ''' </summary>
    Function LierContexteEpisode(episodeId As Long, patientId As Long, contexteId As Long, utilisateurId As Long) As Long
        Dim dao As New EpisodeContexteDao
        dao.CreateEpisodeContexte(New EpisodeContexte With {
            .EpisodeId = episodeId,
            .PatientId = patientId,
            .ContexteId = contexteId
        }, New Utilisateur With {.UtilisateurId = CInt(utilisateurId)})
        Return CLng(Scalaire("SELECT MAX(episode_contexte_id) FROM oasis.oa_episode_contexte WHERE episode_id = @p0 AND contexte_id = @p1",
                             episodeId, contexteId))
    End Function

    ''' <summary>Force la date de création d'un lien épisode et contexte (tri par date_creation).</summary>
    Sub DaterContexteEpisode(episodeContexteId As Long, dateCreation As Date)
        Executer("UPDATE oasis.oa_episode_contexte SET date_creation = @p0 WHERE episode_contexte_id = @p1",
                 dateCreation, episodeContexteId)
    End Sub

    ''' <summary>
    ''' Acte paramédical d'épisode par EpisodeActeParamedicalDao.CreateEpisodeActeParamedical,
    ''' comme GenerateParametreEtProtocoleCollaboratifByEpisode. Renvoie l'id, 0 si le
    ''' DAO a refusé le doublon.
    ''' </summary>
    Function CreerActeParamedicalEpisode(episodeId As Long, patientId As Long, drcId As Long,
                                         Optional typeObservation As String = "PARAMEDICAL",
                                         Optional observation As String = "",
                                         Optional inactif As Boolean = False) As Long
        Dim dao As New EpisodeActeParamedicalDao
        Return dao.CreateEpisodeActeParamedical(New EpisodeActeParamedical With {
            .EpisodeId = episodeId,
            .PatientId = patientId,
            .DrcId = drcId,
            .Observation = observation,
            .TypeObservation = typeObservation,
            .UserId = 0,
            .Inactif = inactif
        })
    End Function

    ''' <summary>
    ''' Paramètre d'épisode par EpisodeParametreDao.CreateEpisodeParametre, qui ne
    ''' renvoie pas l'id : on le retrouve comme la dernière ligne du couple épisode et
    ''' paramètre. Renvoie l'id.
    ''' </summary>
    Function CreerParametreEpisode(episodeId As Long, patientId As Long, parametreId As Long, valeur As Decimal?,
                                   Optional ordre As Integer = 1,
                                   Optional description As String = "Parametre de test",
                                   Optional unite As String = "u",
                                   Optional inactif As Boolean = False) As Long
        Dim dao As New EpisodeParametreDao
        dao.CreateEpisodeParametre(New EpisodeParametre With {
            .EpisodeId = episodeId,
            .PatientId = patientId,
            .ParametreId = parametreId,
            .Valeur = valeur,
            .Description = description,
            .Entier = 3,
            .Decimal = 1,
            .Unite = unite,
            .ParametreAjoute = False,
            .Ordre = ordre,
            .Inactif = inactif
        })
        Return CLng(Scalaire("SELECT MAX(episode_parametre_id) FROM oasis.oa_episode_parametre WHERE episode_id = @p0 AND parametre_id = @p1",
                             episodeId, parametreId))
    End Function

    ''' <summary>
    ''' Observation d'épisode par EpisodeObservationDao.CreateEpisodeObservation, qui
    ''' ne renvoie pas l'id : on reprend le plus grand id de l'épisode. Renvoie l'id.
    ''' </summary>
    Function CreerObservationEpisode(episodeId As Long, patientId As Long, utilisateurId As Long, texte As String,
                                     Optional natureObservation As String = "LIBRE",
                                     Optional typeObservation As String = "MEDICAL",
                                     Optional inactif As Boolean = False) As Long
        Dim dao As New EpisodeObservationDao
        dao.CreateEpisodeObservation(New EpisodeObservation With {
            .EpisodeId = episodeId,
            .PatientId = patientId,
            .UserCreation = utilisateurId,
            .TypeObservation = typeObservation,
            .NatureObservation = natureObservation,
            .NaturePresence = "PRESENTIEL",
            .Observation = texte,
            .Inactif = inactif
        })
        Return CLng(Scalaire("SELECT MAX(episode_observation_id) FROM oasis.oa_episode_observation WHERE episode_id = @p0", episodeId))
    End Function

    ''' <summary>
    ''' Paramètre du référentiel oa_r_parametre. Aucun DAO n'écrit cette table ;
    ''' colonnes reprises de ParametreDao.BuildBean. Aucun singleton ne la met en
    ''' cache, un test peut donc la remplir. Renvoie l'id.
    ''' </summary>
    Function CreerParametreDeMesure(description As String,
                                    Optional unite As String = "u",
                                    Optional ordre As Integer = 1,
                                    Optional entier As Integer = 3,
                                    Optional nbDecimales As Integer = 1) As Long
        Return InsererLigneEpisode("oasis.oa_r_parametre", "id",
            "description, description_patient, entier, decimal, unite, valeur_min, valeur_max, ordre, inactif," &
            " exclusion_auto_suivi, aide_associee, wiki",
            description, description, entier, nbDecimales, unite, 0, 999, ordre, False, False, "", "")
    End Function

    ''' <summary>
    ''' Garantit la présence du paramètre POIDS (id 1, Parametre.EnumParametreId.POIDS)
    ''' que GetPoidsByEpisodeIdOrLastKnow cherche en dur, au cas où
    ''' oa_episode_parametre porterait une clé étrangère vers le référentiel.
    ''' </summary>
    Sub AssurerParametrePoids()
        Executer(
            "IF NOT EXISTS (SELECT 1 FROM oasis.oa_r_parametre WHERE id = 1)" & vbCrLf &
            "BEGIN" & vbCrLf &
            "    IF COLUMNPROPERTY(OBJECT_ID('oasis.oa_r_parametre'), 'id', 'IsIdentity') = 1" & vbCrLf &
            "        EXEC('SET IDENTITY_INSERT oasis.oa_r_parametre ON;" &
            " INSERT INTO oasis.oa_r_parametre (id, description, entier, decimal, unite, valeur_min, valeur_max, ordre, inactif)" &
            " VALUES (1, ''Poids'', 3, 1, ''kg'', 0, 500, 1, 0);" &
            " SET IDENTITY_INSERT oasis.oa_r_parametre OFF;')" & vbCrLf &
            "    ELSE" & vbCrLf &
            "        INSERT INTO oasis.oa_r_parametre (id, description, entier, decimal, unite, valeur_min, valeur_max, ordre, inactif)" &
            " VALUES (1, 'Poids', 3, 1, 'kg', 0, 500, 1, 0);" & vbCrLf &
            "END")
    End Sub

    ''' <summary>
    ''' Type d'activité d'épisode (oa_r_activite_episode). Aucun DAO n'écrit cette
    ''' table et aucun singleton ne la met en cache. Une ligne de même code déjà
    ''' présente est remplacée. genre à Nothing donne NULL.
    ''' </summary>
    Sub CreerActiviteEpisode(code As String, ordre As Integer,
                             Optional genre As String = Nothing,
                             Optional enfant As Boolean = False,
                             Optional inactif As Boolean = False,
                             Optional description As String = Nothing)
        Executer("DELETE FROM oasis.oa_r_activite_episode WHERE oa_activite_type = @p0;" &
                 " INSERT INTO oasis.oa_r_activite_episode (oa_activite_type, oa_activite_nature, oa_activite_description," &
                 " oa_activite_ordre, oa_activite_genre, oa_activite_enfant, oa_activite_inactif)" &
                 " VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6)",
                 code, "Nature " & code, If(description, "Activite " & code), ordre, genre, enfant, inactif)
    End Sub

    ''' <summary>Vide oa_r_activite_episode, pour les tests qui comparent la liste entière.</summary>
    Sub ViderActivitesEpisode()
        Executer("DELETE FROM oasis.oa_r_activite_episode")
    End Sub

    ''' <summary>
    ''' Donne une catégorie Oasis (Drc.EnumCategorieOasisCode) à une DRC. JeuxDrc.CreerDrc
    ''' ne prend que le libellé ; les consignes patient lisent la catégorie sur oa_drc.
    ''' </summary>
    Sub ClasserDrcPourEpisode(drcId As Long, categorie As Integer)
        Executer("UPDATE oasis.oa_drc SET oa_drc_oasis_categorie = @p0 WHERE oa_drc_id = @p1", categorie, drcId)
    End Sub

    ''' <summary>DRC du protocole standard d'une activité, par DrcStandardDao.CreationDrcStandard.</summary>
    Sub CreerDrcStandardEpisode(typeActivite As String, drcId As Long, categorie As Integer,
                                Optional ageMin As Integer = 0, Optional ageMax As Integer = 0)
        Dim dao As New DrcStandardDao
        dao.CreationDrcStandard(New DrcStandard With {
            .TypeActivite = typeActivite,
            .DrcId = drcId,
            .CategorieOasis = categorie,
            .AgeMin = ageMin,
            .AgeMax = ageMax
        })
    End Sub

    ''' <summary>Paramètre d'un groupe de paramètres (DRC), par ParametreDrcDao.CreationParametreDrc.</summary>
    Sub AssocierParametreDrcEpisode(drcId As Long, parametreId As Long)
        Dim dao As New ParametreDrcDao
        dao.CreationParametreDrc(New ParametreDrc With {.DrcId = drcId, .ParametreId = parametreId})
    End Sub

    ''' <summary>Acte paramédical d'un protocole collaboratif, par DrcActeParamedicalAssoDao.</summary>
    Sub AssocierActeProtocoleEpisode(protocoleDrcId As Long, acteDrcId As Long)
        Dim dao As New DrcActeParamedicalAssoDao
        dao.CreateDrcActeParamedicalAsso(New DrcActeParamedicalAsso With {
            .ProtocleCollabaratifDrcId = protocoleDrcId,
            .ActeParamedicalDrcId = acteDrcId
        })
    End Sub

    ''' <summary>
    ''' Consigne du parcours d'un patient, par ParcoursConsigneDao.CreateParcoursConsigne.
    ''' Sans parcours (oa_parcours_id à 0) : les DAO d'épisode ne le lisent pas. Les
    ''' dates absentes valent une fenêtre ouverte (2000 à 2999), le DAO n'acceptant pas
    ''' Date.MinValue.
    ''' </summary>
    Sub CreerConsignePatientEpisode(patientId As Long, drcId As Long, typeActivite As String,
                                    Optional ageMin As Integer = 0, Optional ageMax As Integer = 0,
                                    Optional dateDebut As Date? = Nothing, Optional dateFin As Date? = Nothing,
                                    Optional inactif As Boolean = False)
        Dim dao As New ParcoursConsigneDao
        dao.CreateParcoursConsigne(New ParcoursConsigne With {
            .ParcoursId = 0,
            .PatientId = patientId,
            .DrcId = drcId,
            .TypeEpisode = typeActivite,
            .Commentaire = "",
            .Ordre = 1,
            .AgeMin = ageMin,
            .AgeMax = ageMax,
            .AgeUnite = "A",
            .DateDebut = If(dateDebut, New Date(2000, 1, 1)),
            .DateFin = If(dateFin, New Date(2999, 12, 31)),
            .Inactif = inactif
        })
    End Sub

    ''' <summary>Change la date de naissance d'un patient (JeuxPatient.CreerPatient la fixe en 1970).</summary>
    Sub PoserNaissancePatientEpisode(patientId As Long, dateNaissance As Date)
        Executer("UPDATE oasis.oa_patient SET oa_patient_date_naissance = @p0 WHERE oa_patient_id = @p1", dateNaissance, patientId)
    End Sub

    ''' <summary>
    ''' INSERT qui fonctionne que la clé soit une identité ou non. Les valeurs
    ''' deviennent @p0, @p1... dans l'ordre des colonnes. Renvoie l'id créé.
    ''' </summary>
    Private Function InsererLigneEpisode(table As String, colonneId As String, colonnes As String,
                                         ParamArray valeurs() As Object) As Long
        Dim marques = String.Join(", ", Enumerable.Range(0, valeurs.Length).Select(Function(i) "@p" & i))
        Dim sql =
            "DECLARE @id BIGINT;" & vbCrLf &
            "IF COLUMNPROPERTY(OBJECT_ID('" & table & "'), '" & colonneId & "', 'IsIdentity') = 1" & vbCrLf &
            "BEGIN" & vbCrLf &
            "    INSERT INTO " & table & " (" & colonnes & ") VALUES (" & marques & ");" & vbCrLf &
            "    SET @id = SCOPE_IDENTITY();" & vbCrLf &
            "END" & vbCrLf &
            "ELSE" & vbCrLf &
            "BEGIN" & vbCrLf &
            "    SELECT @id = COALESCE(MAX(" & colonneId & "), 0) + 1 FROM " & table & ";" & vbCrLf &
            "    INSERT INTO " & table & " (" & colonneId & ", " & colonnes & ") VALUES (@id, " & marques & ");" & vbCrLf &
            "END" & vbCrLf &
            "SELECT @id;"
        Return CLng(Scalaire(sql, valeurs))
    End Function

End Module
