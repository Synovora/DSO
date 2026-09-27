-- Complément à 2026-08-24-retrait-suppression-client.sql : dix tables oubliées.
--
-- Contexte. La migration du 2026-08-24 a retiré DELETE à oasis_client sur tout
-- le schéma, puis l'a rendu aux tables que le code supprime. Le relevé cherchait
-- « DELETE FROM ». Or neuf DAO écrivent « DELETE oasis.table » sans FROM, forme
-- tout aussi valide en T-SQL, et un écran écrit en minuscules une dixième
-- suppression qui a échappé au relevé. Depuis cette migration, ces dix
-- suppressions échouent sur les postes clients avec l'erreur 229 : retirer une
-- ligne d'ordonnance, un paramètre ou un acte paramédical d'épisode, un
-- contexte, une réponse de sous-épisode, un synonyme de DRC n'aboutit plus.
--
-- Ce script rend DELETE sur ces dix tables et rien d'autre. Aucune modification
-- applicative.
--
-- Prérequis : docs/migrations/2026-08-24-retrait-suppression-client.sql.

SET XACT_ABORT ON;

-- ---------------------------------------------------------------------------
-- 1. Tables supprimées par le code et absentes du relevé précédent
--
--    Nouveau relevé, au 2026-09-27, de toute instruction DELETE (avec ou sans
--    FROM, quelle que soit la casse) dans Oasis_Common, oasis, Oasis_Web et
--    OasisAdmini :
--
--      OrdonnanceDetailDao           oa_patient_ordonnance_detail
--      EpisodeParametreDao           oa_episode_parametre (aussi la copie de
--                                    oasis/Form/Episode)
--      EpisodeActeParamedicalDao     oa_episode_acte_paramedical
--      EpisodeContexteDao            oa_episode_contexte
--      SousEpisodeReponseDao         oa_sous_episode_reponse
--      DrcActeParamedicalAssoDao     oa_drc_acte_paramedical
--      DrcStandardDao                oa_drc_standard
--      ParametreDrcDao               oa_drc_parametre
--      AutoSuiviDao                  oa_r_autosuivi
--      RadFDrcSynonymeDetailEdit     oa_drc_synonyme
--
--    Les DataSet générés (DataSetPatient, DatSetDRC, DS_Ordonnance,
--    DS_Specialite, UniteSanitaireDataSet, dataSetUtilisateur1) contiennent
--    aussi des DeleteCommand, mais aucun code n'appelle Update ou Delete sur
--    leurs TableAdapter : ils ne servent qu'à lire. Ils ne justifient donc
--    aucun droit.
-- ---------------------------------------------------------------------------

GRANT DELETE ON oasis.oa_patient_ordonnance_detail TO oasis_client;
GRANT DELETE ON oasis.oa_episode_parametre TO oasis_client;
GRANT DELETE ON oasis.oa_episode_acte_paramedical TO oasis_client;
GRANT DELETE ON oasis.oa_episode_contexte TO oasis_client;
GRANT DELETE ON oasis.oa_sous_episode_reponse TO oasis_client;
GRANT DELETE ON oasis.oa_drc_acte_paramedical TO oasis_client;
GRANT DELETE ON oasis.oa_drc_standard TO oasis_client;
GRANT DELETE ON oasis.oa_drc_parametre TO oasis_client;
GRANT DELETE ON oasis.oa_r_autosuivi TO oasis_client;
GRANT DELETE ON oasis.oa_drc_synonyme TO oasis_client;
GO

-- ---------------------------------------------------------------------------
-- 2. Vérification
-- ---------------------------------------------------------------------------

-- Doit renvoyer vingt tables : les dix de la migration précédente et ces dix.
-- SELECT OBJECT_NAME(p.major_id) AS objet, p.permission_name, p.state_desc
--   FROM sys.database_permissions p
--  WHERE p.grantee_principal_id = DATABASE_PRINCIPAL_ID('oasis_client')
--    AND p.permission_name = 'DELETE'
--    AND p.state_desc = 'GRANT'
--  ORDER BY objet;

-- Doit réussir.
-- EXECUTE AS USER = 'oasis_client';
-- DELETE oasis.oa_patient_ordonnance_detail WHERE 1 = 0;
-- REVERT;
