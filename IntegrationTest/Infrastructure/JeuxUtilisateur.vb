Imports Nethereum.Signer
Imports Oasis_Common

''' <summary>
''' Jeux de données du domaine utilisateur : comptes, profils, fonctions, traces
''' d'action, et les lignes que les tests de droits viennent éprouver (compte du
''' portail patient, paramètre de courriel).
'''
''' Les comptes passent par UserDao.Create et les internautes par
''' InternauteDao.Create, dont l'INSERT est celui que la production accepte. Tout
''' le SQL brut du domaine est ici : quand le schéma réel fera apparaître une
''' colonne obligatoire oubliée, c'est le seul fichier à reprendre.
''' </summary>
Public Module JeuxUtilisateur

    Public Const MotDePasseParDefaut As String = "MotDePasse!2026"

    ''' <summary>Profil actif, de type MEDICAL, que reçoivent les comptes créés sans profil.</summary>
    Public Const ProfilDeTest As String = "IT_MEDECIN"

    ''' <summary>
    ''' Crée un utilisateur actif, mot de passe haché comme en production, avec une
    ''' paire de clés secp256k1 si avecCle. Renvoie son id.
    '''
    ''' L'INSERT passe par UserDao.Create sous le compte courant, que le client lourd
    ''' a le droit d'exécuter. La clé, elle, est écrite sous le compte Admin : c'est
    ''' ce que fait /api/signature/cle avec le compte du serveur, et le compte
    ''' courant de l'appelant n'a pas à changer pour autant.
    ''' </summary>
    Function CreerUtilisateur(Optional login As String = Nothing,
                              Optional motDePasse As String = MotDePasseParDefaut,
                              Optional avecCle As Boolean = True,
                              Optional profilId As String = Nothing,
                              Optional admin As Boolean = False) As Long
        Dim profilRetenu = If(profilId, ProfilDeTest)
        If profilRetenu = ProfilDeTest Then AssurerProfil(ProfilDeTest)

        ' Le paramètre motDePasse masque le module MotDePasse : VB ne distingue pas
        ' la casse, d'où le nom complet.
        Dim nouveau As New Utilisateur With {
            .UtilisateurLogin = If(login, NouveauLogin()),
            .UtilisateurNom = "TEST",
            .UtilisateurPrenom = "Utilisateur",
            .UtilisateurProfilId = profilRetenu,
            .UtilisateurAdmin = admin,
            .UtilisateurTelephone = "0102030405",
            .UtilisateurFax = "0102030406",
            .UtilisateurMail = "utilisateur.test@exemple.fr",
            .UtilisateurRPPS = "10001234567",
            .Password = Oasis_Common.MotDePasse.Hacher(motDePasse),
            .IsPasswordUniqueUsage = False
        }
        Dim dao As New UserDao
        dao.Create(nouveau)
        Dim idCree As Long = nouveau.UtilisateurId

        If avecCle Then
            Dim cle = EthECKey.GenerateKey()
            Executer("UPDATE oasis.oa_utilisateur SET cle_privee = @p0, cle_publique = @p1 WHERE oa_utilisateur_id = @p2",
                     "0x" & BitConverter.ToString(cle.GetPrivateKeyAsBytes()).Replace("-", ""),
                     cle.GetPublicAddress(), idCree)
        End If
        Return idCree
    End Function

    ''' <summary>Identifiant de connexion unique, court (15 caractères).</summary>
    Function NouveauLogin() As String
        Return "it_" & Guid.NewGuid().ToString("N").Substring(0, 12)
    End Function

    ''' <summary>Crée un profil. fonctionDefautId à 0 et typeProfil à Nothing donnent NULL.</summary>
    Function CreerProfil(id As String,
                         Optional typeProfil As String = "MEDICAL",
                         Optional inactif As Boolean = False,
                         Optional niveauAcces As Integer = 1,
                         Optional fonctionDefautId As Long = 0,
                         Optional designation As String = Nothing) As String
        Executer("INSERT INTO oasis.oa_r_profil (oa_r_profil_id, oa_r_profil_designation, oa_r_profil_type," &
                 " oa_r_profil_fonction_id_defaut, oa_r_profil_niveau_acces, oa_r_profil_inactif)" &
                 " VALUES (@p0, @p1, @p2, @p3, @p4, @p5)",
                 id, If(designation, "Profil de test " & id), typeProfil,
                 If(fonctionDefautId = 0, Nothing, CObj(fonctionDefautId)), niveauAcces, inactif)
        Return id
    End Function

    ''' <summary>Crée le profil s'il n'existe pas encore, avec les valeurs par défaut de CreerProfil.</summary>
    Sub AssurerProfil(id As String)
        If CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_r_profil WHERE oa_r_profil_id = @p0", id)) = 0 Then
            CreerProfil(id)
        End If
    End Sub

    ''' <summary>
    ''' Crée une fonction et renvoie son id. rorId à 0 et typeFonction à Nothing
    ''' donnent NULL. L'id est attribué par la base si la colonne est une identité,
    ''' sinon pris au-delà du plus grand existant.
    ''' </summary>
    Function CreerFonction(libelle As String,
                           Optional typeFonction As String = "MEDICAL",
                           Optional rorId As Long = 0,
                           Optional inactif As Boolean = False,
                           Optional designation As String = Nothing) As Long
        Return InsererAvecIdentifiant("oasis.oa_r_fonction", "oa_r_fonction_id",
            "oa_r_fonction_designation, oa_r_fonction_libelle, oa_r_fonction_type, oa_r_fonction_ror_id, oa_r_fonction_inactif",
            If(designation, libelle), libelle, typeFonction,
            If(rorId = 0, Nothing, CObj(rorId)), inactif)
    End Function

    ''' <summary>Rattache une fonction à un profil (oa_asso_profil_fonction).</summary>
    Sub AssocierFonction(profilId As String, fonctionId As Long)
        Executer("INSERT INTO oasis.oa_asso_profil_fonction (profil_fonction_id_profil, profil_fonction_id_fonction)" &
                 " VALUES (@p0, @p1)", profilId, fonctionId)
    End Sub

    ''' <summary>
    ''' Trace d'action à une date choisie. ActionDao.CreationAction horodate
    ''' toujours à l'instant présent ; les tests de filtrage par jour ont besoin
    ''' d'une date fixe. Mêmes colonnes que l'INSERT du DAO ; fonctionId à 0 donne
    ''' NULL, au cas où la colonne porterait une clé étrangère.
    ''' </summary>
    Function CreerAction(utilisateurId As Long, patientId As Long, horodatage As Date, libelle As String,
                         Optional fonctionLibelle As String = "", Optional fonctionId As Long = 0) As Long
        Return CLng(Scalaire(
            "INSERT INTO oasis.oa_action (utilisateur_id, patient_id, horodatage, action, fonction, fonction_id)" &
            " VALUES (@p0, @p1, @p2, @p3, @p4, @p5);" &
            " SELECT CAST(SCOPE_IDENTITY() AS BIGINT);",
            utilisateurId, patientId, horodatage, libelle, fonctionLibelle,
            If(fonctionId = 0, Nothing, CObj(fonctionId))))
    End Function

    ''' <summary>Compte du portail patient, créé par InternauteDao.Create sous le compte courant.</summary>
    Function CreerInternaute(Optional email As String = Nothing,
                             Optional recovery As String = "cle-recuperation-test") As Long
        Dim adresse = If(email, NouveauLogin() & "@exemple.fr")
        Dim dao As New InternauteDao
        Return dao.Create(New Internaute With {
            .Username = adresse,
            .Email = adresse,
            .Password = MotDePasseParDefaut,
            .Recovery = recovery
        })
    End Function

    ''' <summary>
    ''' Paramètre de courriel, sans siège. Aucun DAO n'écrit cette table : colonnes
    ''' reprises de ParametreMailDao.BuildBean.
    ''' </summary>
    Function CreerParametreMail(Optional smtpParams As String = "smtp.exemple.fr;compte;secret",
                                Optional typeParam As String = "ORDONNANCE") As Long
        Return InsererAvecIdentifiant("oasis.oa_r_mail_parameter", "id",
            "siege_id, type_mail_param, objet, body, is_body_html, smtp_params",
            Nothing, typeParam, "Objet de test", "Corps de test", False, smtpParams)
    End Function

    ''' <summary>Pose le compteur d'échecs et la date de fin de verrou d'un compte.</summary>
    Sub PoserVerrou(utilisateurId As Long, tentatives As Integer, verrouJusqua As Date?)
        Executer("UPDATE oasis.oa_utilisateur SET oa_utilisateur_tentatives = @p0, oa_utilisateur_verrou_jusqua = @p1" &
                 " WHERE oa_utilisateur_id = @p2",
                 tentatives, If(verrouJusqua.HasValue, CObj(verrouJusqua.Value), Nothing), utilisateurId)
    End Sub

    ''' <summary>Remplace l'empreinte du mot de passe, par exemple par une ancienne empreinte SHA-1.</summary>
    Sub PoserEmpreinte(utilisateurId As Long, empreinte As String)
        Executer("UPDATE oasis.oa_utilisateur SET oa_password = @p0 WHERE oa_utilisateur_id = @p1", empreinte, utilisateurId)
    End Sub

    ''' <summary>Passe à NULL les colonnes facultatives que UserDao.Create renseigne toujours.</summary>
    Sub ViderColonnesFacultativesUtilisateur(utilisateurId As Long)
        Executer("UPDATE oasis.oa_utilisateur SET oa_utilisateur_telephone = NULL, oa_utilisateur_fax = NULL," &
                 " oa_utilisateur_mail = NULL, oa_utilisateur_rpps = NULL, cle_publique = NULL" &
                 " WHERE oa_utilisateur_id = @p0", utilisateurId)
    End Sub

    ''' <summary>Clé privée enregistrée, lue sous le compte Admin. Nothing si NULL.</summary>
    Function ClePriveeUtilisateur(utilisateurId As Long) As String
        Return ChaineLue(Scalaire("SELECT cle_privee FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", utilisateurId))
    End Function

    ''' <summary>Adresse publique enregistrée, lue sous le compte Admin. Nothing si NULL.</summary>
    Function AdresseUtilisateur(utilisateurId As Long) As String
        Return ChaineLue(Scalaire("SELECT cle_publique FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", utilisateurId))
    End Function

    ''' <summary>Empreinte du mot de passe enregistrée, lue sous le compte Admin. Nothing si NULL.</summary>
    Function EmpreinteUtilisateur(utilisateurId As Long) As String
        Return ChaineLue(Scalaire("SELECT oa_password FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", utilisateurId))
    End Function

    ''' <summary>Valeur de Scalaire en chaîne, Nothing pour une ligne absente ou un NULL.</summary>
    Function ChaineLue(valeur As Object) As String
        If valeur Is Nothing OrElse valeur Is DBNull.Value Then Return Nothing
        Return CStr(valeur)
    End Function

    ''' <summary>
    ''' INSERT qui fonctionne que la clé soit une identité ou non : on ne connaît
    ''' pas encore le schéma. Les valeurs deviennent @p0, @p1... dans l'ordre des
    ''' colonnes. Renvoie l'id de la ligne créée.
    ''' </summary>
    Private Function InsererAvecIdentifiant(table As String, colonneId As String, colonnes As String,
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
