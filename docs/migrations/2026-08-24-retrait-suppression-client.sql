-- Retrait du droit de suppression au compte des postes clients.
--
-- Contexte. oasis_client, la connexion que /api/login remet à chaque poste,
-- dispose de SELECT, INSERT, UPDATE et DELETE sur tout le schéma. La suppression
-- est celui des quatre dont l'application n'a presque pas besoin : les dossiers,
-- épisodes, ordonnances, antécédents et comptes se désactivent par un indicateur
-- (oa_*_inactif, oa_utilisateur_etat, etat), ils ne se suppriment pas.
--
-- Un poste détourné peut donc aujourd'hui effacer un dossier médical par une
-- connexion SQL directe, sans passer par l'application et sans laisser de trace
-- applicative. Pour un dossier de santé, l'atteinte à l'intégrité est plus grave
-- que l'atteinte à la confidentialité : une lecture indue se répare mal mais se
-- constate, une ligne supprimée ne se constate pas.
--
-- Ce script retire DELETE sur tout le schéma et le rend uniquement aux dix tables
-- que le code supprime réellement. Aucune modification applicative.
--
-- Prérequis : docs/migrations/2026-08-24-comptes-sql-separes.sql, qui crée
-- oasis_client.

SET XACT_ABORT ON;

-- ---------------------------------------------------------------------------
-- 1. Retrait général
--
--    REVOKE retire l'autorisation accordée au schéma sans poser de refus
--    explicite, ce qui laisse la possibilité de rendre le droit table par table
--    juste après. Un DENY au niveau du schéma l'emporterait au contraire sur
--    toute autorisation plus fine et rendrait la section 2 sans effet.
-- ---------------------------------------------------------------------------

REVOKE DELETE ON SCHEMA::oasis FROM oasis_client;
GO

-- ---------------------------------------------------------------------------
-- 2. Tables réellement supprimées par le code
--
--    Relevé exhaustif des DELETE FROM présents dans Oasis_Common et oasis au
--    2026-08-24. Il s'agit de lignes de liaison et de brouillons de saisie, pas
--    de dossiers.
--
--      ChaineEpisodeDao          oa_chaine_episode, oa_relation_chaine_episode
--      TraitementDao             oa_traitement
--      ValenceDao                oa_relation_vaccin_valence, oa_valence
--      CGVDateDao                oa_vaccin_cgv_date, oa_vaccin_cgv_valence,
--                                oa_vaccin_cgv_relation_valence_date
--      VaccinDao                 oa_vaccin_program, oa_vaccin_program_relation
--
--    Toute nouvelle suppression dans le code demandera une ligne ici. C'est
--    voulu : la question « pourquoi cette table doit-elle être supprimable »
--    mérite d'être posée à chaque fois.
-- ---------------------------------------------------------------------------

GRANT DELETE ON oasis.oa_chaine_episode TO oasis_client;
GRANT DELETE ON oasis.oa_relation_chaine_episode TO oasis_client;
GRANT DELETE ON oasis.oa_traitement TO oasis_client;
GRANT DELETE ON oasis.oa_relation_vaccin_valence TO oasis_client;
GRANT DELETE ON oasis.oa_valence TO oasis_client;
GRANT DELETE ON oasis.oa_vaccin_cgv_date TO oasis_client;
GRANT DELETE ON oasis.oa_vaccin_cgv_valence TO oasis_client;
GRANT DELETE ON oasis.oa_vaccin_cgv_relation_valence_date TO oasis_client;
GRANT DELETE ON oasis.oa_vaccin_program TO oasis_client;
GRANT DELETE ON oasis.oa_vaccin_program_relation TO oasis_client;
GO

-- Le refus explicite posé sur oa_utilisateur par la migration précédente reste en
-- place et l'emporte de toute façon : un compte se désactive, il ne se supprime
-- pas.

-- ---------------------------------------------------------------------------
-- 3. Vérification
-- ---------------------------------------------------------------------------

-- Doit renvoyer exactement les dix tables ci-dessus.
-- SELECT OBJECT_NAME(p.major_id) AS objet, p.permission_name, p.state_desc
--   FROM sys.database_permissions p
--  WHERE p.grantee_principal_id = DATABASE_PRINCIPAL_ID('oasis_client')
--    AND p.permission_name = 'DELETE'
--    AND p.state_desc = 'GRANT'
--  ORDER BY objet;

-- Doit échouer.
-- EXECUTE AS USER = 'oasis_client';
-- DELETE FROM oasis.oa_patient WHERE 1 = 0;
-- REVERT;
