-- Données de référence du domaine Tâche (lot L).
--
-- Rejoué par Preparer-Base.ps1 après 20-reference.sql, avant la prise de
-- l'instantané. Mêmes règles que 20-reference.sql : données fictives, strict
-- nécessaire, rejouable sur une base neuve.
--
-- oa_r_specialite est lue une seule fois par le singleton Table_specialite
-- d'EnvironnementBase : ses lignes ne peuvent pas venir d'un test.
-- TacheDao.CreationAutomatiqueDeDemandeRendezVous y prend le délai de prise en
-- charge de la spécialité du parcours, et TacheDao.GetTacheBeanAssocie
-- l'indicateur Oasis (spécialité hors Oasis : l'intervenant vient du ROR).
-- Dépendants : Dao/TacheDaoWorkflow.test.vb et Dao/TacheDao.test.vb, par les
-- constantes JeuxTache.SpecialiteTacheNonOasis et JeuxTache.SpecialiteTacheOasis.
--
--   9701 : hors Oasis, délai de 400 jours (JeuxTache.DelaiSpecialiteTacheNonOasis) ;
--   9702 : Oasis, délai à 0, remplacé par le délai par défaut de Table_specialite.
--
-- L'id est fixé. L'export du schéma peut déclarer la colonne IDENTITY ou non :
-- l'insertion passe par IDENTITY_INSERT seulement dans le premier cas.

SET NOCOUNT ON;

IF COLUMNPROPERTY(OBJECT_ID('oasis.oa_r_specialite'), 'oa_r_specialite_id', 'IsIdentity') = 1
BEGIN
    SET IDENTITY_INSERT oasis.oa_r_specialite ON;
    IF NOT EXISTS (SELECT 1 FROM oasis.oa_r_specialite WHERE oa_r_specialite_id = 9701)
        INSERT INTO oasis.oa_r_specialite
            (oa_r_specialite_id, oa_specialite_code, oa_r_specialite_description, oa_r_specialite_nature,
             oa_r_specialite_genre, oa_r_parcours, oa_r_oasis, oa_r_specialite_age_min, oa_r_specialite_age_max,
             oa_r_delaiPriseEnCharge, oa_r_specialite_inactif)
        VALUES (9701, 'IT_TACHE_EXT', 'Spécialité de test hors Oasis', 'Spécialité', '', 1, 0, 0, 0, 400, 0);
    IF NOT EXISTS (SELECT 1 FROM oasis.oa_r_specialite WHERE oa_r_specialite_id = 9702)
        INSERT INTO oasis.oa_r_specialite
            (oa_r_specialite_id, oa_specialite_code, oa_r_specialite_description, oa_r_specialite_nature,
             oa_r_specialite_genre, oa_r_parcours, oa_r_oasis, oa_r_specialite_age_min, oa_r_specialite_age_max,
             oa_r_delaiPriseEnCharge, oa_r_specialite_inactif)
        VALUES (9702, 'IT_TACHE_OASIS', 'Spécialité de test Oasis', 'Spécialité', '', 1, 1, 0, 0, 0, 0);
    SET IDENTITY_INSERT oasis.oa_r_specialite OFF;
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT 1 FROM oasis.oa_r_specialite WHERE oa_r_specialite_id = 9701)
        INSERT INTO oasis.oa_r_specialite
            (oa_r_specialite_id, oa_specialite_code, oa_r_specialite_description, oa_r_specialite_nature,
             oa_r_specialite_genre, oa_r_parcours, oa_r_oasis, oa_r_specialite_age_min, oa_r_specialite_age_max,
             oa_r_delaiPriseEnCharge, oa_r_specialite_inactif)
        VALUES (9701, 'IT_TACHE_EXT', 'Spécialité de test hors Oasis', 'Spécialité', '', 1, 0, 0, 0, 400, 0);
    IF NOT EXISTS (SELECT 1 FROM oasis.oa_r_specialite WHERE oa_r_specialite_id = 9702)
        INSERT INTO oasis.oa_r_specialite
            (oa_r_specialite_id, oa_specialite_code, oa_r_specialite_description, oa_r_specialite_nature,
             oa_r_specialite_genre, oa_r_parcours, oa_r_oasis, oa_r_specialite_age_min, oa_r_specialite_age_max,
             oa_r_delaiPriseEnCharge, oa_r_specialite_inactif)
        VALUES (9702, 'IT_TACHE_OASIS', 'Spécialité de test Oasis', 'Spécialité', '', 1, 1, 0, 0, 0, 0);
END
