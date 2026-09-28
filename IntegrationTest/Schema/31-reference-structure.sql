-- Données de référence du lot P (structures, annuaire, singletons d'EnvironnementBase).
--
-- Rejoué par Preparer-Base.ps1 après les autres scripts de référence, avant la
-- prise de l'instantané. Mêmes règles que 20-reference.sql : données fictives,
-- strict nécessaire, rejouable sur une base neuve.
--
-- Sièges, unités sanitaires, sites et annuaire professionnel ne sont lus par
-- aucun singleton : les tests les créent eux-mêmes (Infrastructure/JeuxStructure.vb).
-- Ce script ne pose que ce qui manque aux tests des singletons
-- (Module/EnvironnementBase.test.vb), qui lisent leur table une seule fois pour
-- tout le processus :
--
--   oa_r_genre, code Z : genre inactif, absent de Table_genre.
--   oa_r_genre, code Y : inactif NULL, présent dans Table_genre (le filtre
--                        accepte NULL). Posé seulement si la colonne l'accepte.
--   oa_r_categorie_majeure 9403 (ITC) : inactive, absente de Table_categorie_majeure.
--   oa_r_categorie_majeure 9404 (ITD) : inactif NULL, absente aussi (le filtre
--                        n'accepte que 'False'). Posée seulement si la colonne
--                        accepte NULL.
--
-- Les spécialités viennent de 27-reference-tache.sql et 28-reference-parcours.sql,
-- les ALD de 29-reference-theriaque.sql. Rien n'est ajouté à ces tables : leurs
-- tests de DAO comptent les lignes.
--
-- Aucune description n'est NULL : les singletons les rangent dans un
-- dictionnaire de chaînes et DBNull ne se convertit pas, le chargement échouerait
-- pour tout le processus.
--
-- L'id des catégories est fixé. L'export du schéma peut déclarer la colonne
-- IDENTITY ou non : l'insertion passe par IDENTITY_INSERT seulement dans le
-- premier cas. IF NOT EXISTS : un autre script peut avoir posé les mêmes lignes.

SET NOCOUNT ON;

IF NOT EXISTS (SELECT 1 FROM oasis.oa_r_genre WHERE oa_r_genre_code = 'Z')
    INSERT INTO oasis.oa_r_genre (oa_r_genre_code, oa_r_genre_description, oa_r_genre_inactif)
    VALUES ('Z', N'Genre inactif de test', 1);

IF COLUMNPROPERTY(OBJECT_ID('oasis.oa_r_genre'), 'oa_r_genre_inactif', 'AllowsNull') = 1
    AND NOT EXISTS (SELECT 1 FROM oasis.oa_r_genre WHERE oa_r_genre_code = 'Y')
    INSERT INTO oasis.oa_r_genre (oa_r_genre_code, oa_r_genre_description, oa_r_genre_inactif)
    VALUES ('Y', N'Genre sans indicateur de test', NULL);

IF COLUMNPROPERTY(OBJECT_ID('oasis.oa_r_categorie_majeure'), 'oa_r_categorie_majeure_id', 'IsIdentity') = 1
    SET IDENTITY_INSERT oasis.oa_r_categorie_majeure ON;

IF NOT EXISTS (SELECT 1 FROM oasis.oa_r_categorie_majeure WHERE oa_r_categorie_majeure_id = 9403)
    INSERT INTO oasis.oa_r_categorie_majeure
        (oa_r_categorie_majeure_id, oa_r_categorie_majeure_code, oa_r_categorie_majeure_description, oa_r_categorie_majeure_inactif)
    VALUES (9403, 'ITC', N'Catégorie majeure inactive de test', 1);

IF COLUMNPROPERTY(OBJECT_ID('oasis.oa_r_categorie_majeure'), 'oa_r_categorie_majeure_inactif', 'AllowsNull') = 1
    AND NOT EXISTS (SELECT 1 FROM oasis.oa_r_categorie_majeure WHERE oa_r_categorie_majeure_id = 9404)
    INSERT INTO oasis.oa_r_categorie_majeure
        (oa_r_categorie_majeure_id, oa_r_categorie_majeure_code, oa_r_categorie_majeure_description, oa_r_categorie_majeure_inactif)
    VALUES (9404, 'ITD', N'Catégorie majeure sans indicateur de test', NULL);

IF COLUMNPROPERTY(OBJECT_ID('oasis.oa_r_categorie_majeure'), 'oa_r_categorie_majeure_id', 'IsIdentity') = 1
    SET IDENTITY_INSERT oasis.oa_r_categorie_majeure OFF;
