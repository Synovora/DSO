Imports Oasis_Common

''' <summary>
''' Sous-épisodes de test et ce qui gravite autour : réponses reçues, courriels de
''' réponse et leurs pièces jointes, lignes de référentiel supplémentaires.
'''
''' Les sous-épisodes passent par SousEpisodeDao.Create, l'INSERT que la fenêtre
''' FrmSousEpisode exécute en production. Les réponses sont écrites en SQL brut :
''' SousEpisodeReponseDao.Create et CreateByMoving déposent ou renomment le
''' document sur le serveur de fichiers dans la même transaction, ce que la base de
''' test ne peut pas simuler. Les courriels et pièces jointes n'ont aucun DAO
''' d'écriture (ils arrivent par l'intégration des mails). Tout le SQL brut du
''' domaine est ici.
'''
''' Le référentiel de base (types 901 et 902 et leurs sous-types) vient de
''' Schema/23-reference-sousepisode.sql et fait donc partie de l'instantané.
''' </summary>
Public Module JeuxSousEpisode

    ' --- Référentiel posé par 23-reference-sousepisode.sql -------------------------

    Public Const TypeSeCourrier As Long = 901
    Public Const TypeSeCertificat As Long = 902

    ''' <summary>Type 901, ALD possible, réponse requise, délai 10 jours.</summary>
    Public Const SousTypeSeAdressage As Long = 911
    ''' <summary>Type 901, ni ALD ni réponse requise, délai 15 jours.</summary>
    Public Const SousTypeSeCompteRendu As Long = 912
    ''' <summary>Type 902.</summary>
    Public Const SousTypeSeCertificat As Long = 921

    ''' <summary>Sous-type 911.</summary>
    Public Const SousSousTypeSeBilan As Long = 931
    ''' <summary>Sous-type 911.</summary>
    Public Const SousSousTypeSeImagerie As Long = 932
    ''' <summary>Sous-type 921.</summary>
    Public Const SousSousTypeSeAptitude As Long = 933

    Public Const LibelleTypeSeCourrier As String = "Courrier"
    Public Const LibelleTypeSeCertificat As String = "Certificat"
    Public Const LibelleSousTypeSeAdressage As String = "Adressage"
    Public Const LibelleSousTypeSeCompteRendu As String = "Compte rendu"
    Public Const LibelleSousTypeSeCertificat As String = "Certificat medical"

    ''' <summary>Horodatage de création des lignes du référentiel de base.</summary>
    Public ReadOnly HorodateReferentielSe As New Date(2026, 1, 5, 9, 0, 0)

    ' --- Sous-épisodes ---------------------------------------------------------------

    ''' <summary>
    ''' Crée un sous-épisode actif dans cet épisode, par SousEpisodeDao.Create sous le
    ''' compte courant, comme FrmSousEpisode : sans destinataire, une ligne de détail
    ''' par sous-sous-type donné. Le type est celui du sous-type. Renvoie l'id.
    ''' </summary>
    Function CreerSousEpisode(episodeId As Long, utilisateurId As Long,
                              Optional sousTypeId As Long = SousTypeSeAdressage,
                              Optional commentaire As String = "Sous-episode de test",
                              Optional estAld As Boolean = False,
                              Optional reponseAttendue As Boolean = True,
                              Optional delaiReponse As Integer = 15,
                              Optional sousSousTypes As Long() = Nothing) As Long
        Dim typeId = CLng(Scalaire("SELECT id_sous_episode_type FROM oasis.oa_r_sous_episode_sous_type WHERE id = @p0",
                                   sousTypeId))
        Dim nouveau As New SousEpisode With {
            .EpisodeId = episodeId,
            .IdSousEpisodeType = typeId,
            .IdSousEpisodeSousType = sousTypeId,
            .IdIntervenant = 0,
            .CreateUserId = utilisateurId,
            .Commentaire = commentaire,
            .IsALD = estAld,
            .IsReponse = reponseAttendue,
            .DelaiSinceValidation = delaiReponse,
            .lstDetail = New List(Of SousEpisodeDetailSousType)
        }
        If sousSousTypes IsNot Nothing Then
            For Each idSousSousType In sousSousTypes
                nouveau.lstDetail.Add(New SousEpisodeDetailSousType With {
                    .IdSousEpisodeSousSousType = idSousSousType,
                    .IsALD = estAld
                })
            Next
        End If
        Dim daoSousEpisode As New SousEpisodeDao
        daoSousEpisode.Create(nouveau)
        Return nouveau.Id
    End Function

    ''' <summary>
    ''' Réponse reçue pour un sous-épisode : mêmes colonnes que l'INSERT de
    ''' SousEpisodeReponseDao.Create, suivi de la même mise à jour du sous-épisode
    ''' (réponse reçue, date de dernière réception). Seul le dépôt du fichier manque.
    ''' etat à Nothing donne un validate_state NULL. Renvoie l'id.
    ''' </summary>
    Function CreerReponseSousEpisode(sousEpisodeId As Long, utilisateurId As Long,
                                     Optional etat As String = "!",
                                     Optional nomFichier As String = "compte-rendu.pdf",
                                     Optional commentaire As String = "Reponse de test",
                                     Optional horodate As Date? = Nothing) As Long
        Dim quand As Date = If(horodate.HasValue, horodate.Value, New Date(2026, 3, 1, 10, 0, 0))
        Return CLng(Scalaire(
            "INSERT INTO oasis.oa_sous_episode_reponse" &
            " (id_sous_episode, create_user_id, horodate_creation, nom_fichier, commentaire, validate_state, episode_id)" &
            " VALUES (@p0, @p1, @p2, @p3, @p4, @p5, (SELECT episode_id FROM oasis.oa_sous_episode WHERE id = @p0));" &
            " DECLARE @id BIGINT = CAST(SCOPE_IDENTITY() AS BIGINT);" &
            " UPDATE oasis.oa_sous_episode SET is_reponse_recue = 'true', horodate_last_recu = @p2 WHERE id = @p0;" &
            " SELECT @id;",
            sousEpisodeId, utilisateurId, quand, nomFichier, commentaire, etat))
    End Function

    ' --- Courriels de réponse ----------------------------------------------------------

    ''' <summary>
    ''' Courriel de réponse en attente d'attribution (status « unprocessed » par
    ''' défaut). patientId à 0 et objet à Nothing donnent NULL. Renvoie l'id.
    ''' </summary>
    Function CreerMailReponseSousEpisode(Optional statut As String = "unprocessed",
                                         Optional auteur As String = "laboratoire@exemple.fr",
                                         Optional objet As String = "Resultats d'analyse",
                                         Optional corps As String = "Corps du message de test",
                                         Optional patientId As Long = 0,
                                         Optional horodate As Date? = Nothing) As Long
        Dim quand As Date = If(horodate.HasValue, horodate.Value, New Date(2026, 3, 2, 8, 30, 0))
        Return InsererLigneDomaineSe("oasis.oa_sous_episode_reponse_mail", "id",
            "auteur, objet, status, horodate_creation, patient_id, corps",
            auteur, objet, statut, quand, If(patientId = 0, Nothing, CObj(patientId)), corps)
    End Function

    ''' <summary>Pièce jointe d'un courriel de réponse. nomFichier à Nothing donne NULL. Renvoie l'id.</summary>
    Function CreerPieceJointeMailSousEpisode(mailId As Long, part As Long,
                                             Optional nomFichier As String = "resultat.pdf") As Long
        Return InsererLigneDomaineSe("oasis.oa_sous_episode_reponse_mail_attachment", "id",
            "mailId, filename, part", mailId, nomFichier, part)
    End Function

    ' --- Référentiel supplémentaire, pour les cas limites -----------------------------

    ''' <summary>
    ''' Type de sous-épisode. categorie et avecDestinataire à Nothing donnent NULL.
    ''' Aucun singleton ne met cette table en cache : un test peut en créer.
    ''' </summary>
    Function CreerTypeSousEpisode(libelle As String,
                                  Optional categorie As String = Nothing,
                                  Optional avecDestinataire As Boolean? = Nothing) As Long
        Return InsererLigneDomaineSe("oasis.oa_r_sous_episode_type", "id",
            "categorie, horodate_creation, libelle, is_with_destinataire",
            categorie, HorodateReferentielSe, libelle,
            If(avecDestinataire.HasValue, CObj(avecDestinataire.Value), Nothing))
    End Function

    ''' <summary>
    ''' Sous-type. reponseRequise, delai et commentaire à Nothing donnent NULL ;
    ''' les colonnes que le bean lit sans Coalesce restent renseignées.
    ''' </summary>
    Function CreerSousTypeSousEpisode(typeId As Long, libelle As String,
                                      Optional reponseRequise As Boolean? = Nothing,
                                      Optional delai As Integer? = Nothing,
                                      Optional commentaire As String = Nothing,
                                      Optional aldPossible As Boolean = False) As Long
        Return InsererLigneDomaineSe("oasis.oa_r_sous_episode_sous_type", "id",
            "id_sous_episode_type, horodate_creation, libelle, redaction_profil_types, validation_profil_types," &
            " is_ald_possible, is_reponse_requise, delai_reponse, commentaire",
            typeId, HorodateReferentielSe, libelle, "MEDICAL", "MEDICAL", aldPossible,
            If(reponseRequise.HasValue, CObj(reponseRequise.Value), Nothing),
            If(delai.HasValue, CObj(delai.Value), Nothing),
            commentaire)
    End Function

    ''' <summary>Sous-sous-type. commentaire à Nothing donne NULL.</summary>
    Function CreerSousSousTypeSousEpisode(sousTypeId As Long, libelle As String,
                                          Optional commentaire As String = Nothing) As Long
        Return InsererLigneDomaineSe("oasis.oa_r_sous_episode_sous_sous_type", "id",
            "id_sous_episode_sous_type, horodate_creation, libelle, commentaire",
            sousTypeId, HorodateReferentielSe, libelle, commentaire)
    End Function

    ''' <summary>
    ''' INSERT sous le compte Admin qui fonctionne que la clé soit une identité ou
    ''' non (le schéma n'est pas encore connu). Les valeurs deviennent @p0, @p1...
    ''' dans l'ordre des colonnes. Renvoie l'id de la ligne créée.
    ''' </summary>
    Private Function InsererLigneDomaineSe(table As String, colonneId As String, colonnes As String,
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
