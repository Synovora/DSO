-- Séparation des comptes SQL et mise hors de portée des colonnes secrètes.
--
-- Contexte. /api/login remet au poste client la chaîne de connexion à la base,
-- chiffrée avec une clé que ClickOnce installe sur ce même poste. Le chiffrement
-- ne protège donc rien vis-à-vis de qui possède le client : n'importe quel
-- utilisateur, quel que soit son profil, pouvait ouvrir une connexion SQL directe
-- et lire toute la base, dont les clés privées de signature des prescripteurs,
-- les empreintes de mots de passe, les comptes du portail patient et le mot de
-- passe du compte d'envoi de mail.
--
-- Ce script crée deux utilisateurs de base distincts au lieu d'un seul :
--
--   oasis_web     le serveur, et lui seul. Renseigné dans Oasis_Web/Web.config.
--                 Vérifie les mots de passe, signe les ordonnances, envoie les
--                 mails. Il a besoin des colonnes secrètes.
--
--   oasis_client  ce que /api/login distribue aux postes. Mêmes droits métier,
--                 mais lecture refusée sur les colonnes secrètes et écriture
--                 refusée sur les colonnes de clé.
--
-- Ce que cela ne fait pas : un utilisateur reste capable de lire les données
-- cliniques que son profil ne lui montre pas dans l'écran. Fermer cela demande
-- des comptes par profil (étape suivante), puis le passage des écritures derrière
-- l'API. Ce que cela ferme, c'est « tout poste équivaut à un administrateur de
-- la base ».
--
-- Prérequis : le déploiement applicatif du 2026-08-24 (réponse de /api/login
-- portant l'utilisateur, /api/signature, listes de colonnes explicites). Avec une
-- version antérieure du client, les refus ci-dessous font échouer la connexion.
--
-- Ordre : ce script, puis Oasis_Web, puis la publication ClickOnce.

SET XACT_ABORT ON;

-- ---------------------------------------------------------------------------
-- 1. Les deux connexions
--
--    Les mots de passe ne figurent pas ici : ce dépôt est public. Créez les deux
--    connexions à part, avec des mots de passe distincts tirés au hasard, puis
--    exécutez ce script. Voir docs/runbooks/credential-rotation.md.
--
--      CREATE LOGIN oasis_web    WITH PASSWORD = '...', CHECK_POLICY = ON;
--      CREATE LOGIN oasis_client WITH PASSWORD = '...', CHECK_POLICY = ON;
--
--    L'ancien compte partagé (oasis_app, ou sa) ne doit plus servir à
--    l'application une fois les deux en place.
-- ---------------------------------------------------------------------------

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'oasis_web')
    CREATE USER oasis_web FOR LOGIN oasis_web;
GO

IF NOT EXISTS (SELECT 1 FROM sys.database_principals WHERE name = 'oasis_client')
    CREATE USER oasis_client FOR LOGIN oasis_client;
GO

-- ---------------------------------------------------------------------------
-- 2. Droits métier communs
--
--    Les deux comptes lisent et écrivent le schéma applicatif. Ni l'un ni l'autre
--    n'est db_owner, et aucun des deux ne peut modifier le schéma : une injection
--    ou un client détourné ne doit pas pouvoir créer une table, une procédure ou
--    un déclencheur.
-- ---------------------------------------------------------------------------

GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::oasis TO oasis_web;
GRANT EXECUTE ON SCHEMA::oasis TO oasis_web;
GO

GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::oasis TO oasis_client;
GRANT EXECUTE ON SCHEMA::oasis TO oasis_client;
GO

-- ---------------------------------------------------------------------------
-- 3. Ce que le poste client ne doit pas lire
--
--    Un refus l'emporte sur toute autorisation, y compris celle du schéma.
--
--    Attention : un SELECT * échoue en bloc dès qu'une colonne refusée figure
--    dans la table visée. Les requêtes concernées listent désormais leurs
--    colonnes (UserDao, ParametreMailDao) ; ne pas revenir en arrière.
--
--    Le DataSet typé oasis/dataSetUtilisateur1.Designer.vb sélectionne encore
--    oa_password, sur la table comme sur la vue v_user_full. Aucun écran ne
--    l'instancie aujourd'hui, il compile sans jamais interroger la base ; le
--    brancher tel quel sur un poste échouerait.
-- ---------------------------------------------------------------------------

-- Clé privée de signature et empreinte du mot de passe des utilisateurs.
-- La signature passe par /api/signature, la vérification du mot de passe par
-- /api/login : le poste n'a plus besoin ni de l'une ni de l'autre.
DENY SELECT ON oasis.oa_utilisateur (cle_privee) TO oasis_client;
DENY SELECT ON oasis.oa_utilisateur (oa_password) TO oasis_client;
GO

-- Écriture des colonnes sensibles. Refuser la lecture ne suffit pas :
--   * sans refus sur cle_privee, un poste remplace la clé d'un prescripteur par
--     une clé qu'il maîtrise et signe en son nom, ce que la page publique
--     /Sign/Check présente alors comme authentique ;
--   * sans refus sur oa_password, un poste écrase l'empreinte d'un confrère par
--     une valeur choisie et se connecte à sa place.
-- Le changement de mot de passe passe par /api/motdepasse, la génération de clé
-- par /api/signature/cle.
DENY UPDATE ON oasis.oa_utilisateur (cle_privee) TO oasis_client;
DENY UPDATE ON oasis.oa_utilisateur (cle_publique) TO oasis_client;
DENY UPDATE ON oasis.oa_utilisateur (oa_password) TO oasis_client;
GO

-- Comptes du portail patient. Le client lourd crée et réinitialise ces comptes
-- depuis la fiche patient, il conserve donc l'écriture ; il n'a en revanche
-- aucune raison de lire l'empreinte du mot de passe ni la clé de récupération
-- d'un patient. Le test d'existence passe par ExisteInternautePourEmail, qui ne
-- sélectionne aucune de ces colonnes.
--
-- Reste ouvert : le poste peut encore écrire une clé de récupération et prendre
-- ainsi la main sur un compte portail. Le gain serait nul aujourd'hui (le portail
-- ne montre qu'un sous-ensemble de ce que le poste lit déjà en base), et la
-- fermeture propre passe par le déplacement de ces deux boutons derrière l'API.
DENY SELECT ON oasis.oa_internaute (password) TO oasis_client;
DENY SELECT ON oasis.oa_internaute (recovery) TO oasis_client;
GO

-- Identifiants du compte SMTP. Le poste ne vient chercher dans cette table que
-- le modèle de message ; l'envoi se fait par /api/sendMail.
DENY SELECT ON oasis.oa_r_mail_parameter (smtp_params) TO oasis_client;
GO

-- Suppression d'un utilisateur : les comptes se désactivent (oa_utilisateur_etat),
-- ils ne se suppriment pas. Interdire la suppression ferme le contournement qui
-- consisterait à supprimer une fiche pour la recréer avec une clé choisie.
DENY DELETE ON oasis.oa_utilisateur TO oasis_client;
GO

-- Limite connue. SQL Server ne sait pas refuser l'INSERT colonne par colonne :
-- la création d'un compte, qui reste une opération d'administration, écrit encore
-- oa_password depuis le poste. Elle produit une fiche neuve, elle ne permet donc
-- pas de prendre la place d'un compte existant. Le passage des écritures derrière
-- l'API (étape 2) ferme aussi ce reste.

-- ---------------------------------------------------------------------------
-- 4. Vérification
--
--    À exécuter après coup. La première requête doit renvoyer les six refus
--    ci-dessus, la seconde doit échouer avec « SELECT permission was denied ».
-- ---------------------------------------------------------------------------

-- SELECT p.permission_name, p.state_desc, OBJECT_NAME(p.major_id) AS objet, c.name AS colonne
--   FROM sys.database_permissions p
--   LEFT JOIN sys.columns c ON c.object_id = p.major_id AND c.column_id = p.minor_id
--  WHERE p.grantee_principal_id = DATABASE_PRINCIPAL_ID('oasis_client')
--    AND p.state_desc = 'DENY';

-- EXECUTE AS USER = 'oasis_client';
-- SELECT TOP 1 cle_privee FROM oasis.oa_utilisateur;   -- doit échouer
-- REVERT;
