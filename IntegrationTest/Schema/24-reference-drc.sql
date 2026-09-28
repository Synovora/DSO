-- Données de référence du domaine DRC (lot I).
--
-- Rejoué par Preparer-Base.ps1 après 20-reference.sql, avant la prise de
-- l'instantané. Mêmes règles que 20-reference.sql : données fictives, strict
-- nécessaire, rejouable sur une base neuve.
--
-- oa_r_categorie_majeure est lue une seule fois par le singleton
-- Table_categorie_majeure d'EnvironnementBase : ses lignes ne peuvent pas venir
-- d'un test. Les DRC de test (Infrastructure/JeuxDrc.vb) sont rattachées à ces
-- deux catégories, pour que la vue v_drc les montre même si elle joint la
-- catégorie majeure par une jointure interne. Dépendants :
-- Dao/DrcDao.test.vb (filtre par catégorie majeure, JeuxDrc.CategorieMajeureDrcDeTest
-- et JeuxDrc.CategorieMajeureDrcAutre).
--
-- L'id est fixé. L'export du schéma peut déclarer la colonne IDENTITY ou non :
-- l'insertion passe par IDENTITY_INSERT seulement dans le premier cas.

SET NOCOUNT ON;

IF COLUMNPROPERTY(OBJECT_ID('oasis.oa_r_categorie_majeure'), 'oa_r_categorie_majeure_id', 'IsIdentity') = 1
BEGIN
    SET IDENTITY_INSERT oasis.oa_r_categorie_majeure ON;
    IF NOT EXISTS (SELECT 1 FROM oasis.oa_r_categorie_majeure WHERE oa_r_categorie_majeure_id = 9401)
        INSERT INTO oasis.oa_r_categorie_majeure
            (oa_r_categorie_majeure_id, oa_r_categorie_majeure_code, oa_r_categorie_majeure_description, oa_r_categorie_majeure_inactif)
        VALUES (9401, 'ITA', 'Catégorie majeure de test A', 0);
    IF NOT EXISTS (SELECT 1 FROM oasis.oa_r_categorie_majeure WHERE oa_r_categorie_majeure_id = 9402)
        INSERT INTO oasis.oa_r_categorie_majeure
            (oa_r_categorie_majeure_id, oa_r_categorie_majeure_code, oa_r_categorie_majeure_description, oa_r_categorie_majeure_inactif)
        VALUES (9402, 'ITB', 'Catégorie majeure de test B', 0);
    SET IDENTITY_INSERT oasis.oa_r_categorie_majeure OFF;
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT 1 FROM oasis.oa_r_categorie_majeure WHERE oa_r_categorie_majeure_id = 9401)
        INSERT INTO oasis.oa_r_categorie_majeure
            (oa_r_categorie_majeure_id, oa_r_categorie_majeure_code, oa_r_categorie_majeure_description, oa_r_categorie_majeure_inactif)
        VALUES (9401, 'ITA', 'Catégorie majeure de test A', 0);
    IF NOT EXISTS (SELECT 1 FROM oasis.oa_r_categorie_majeure WHERE oa_r_categorie_majeure_id = 9402)
        INSERT INTO oasis.oa_r_categorie_majeure
            (oa_r_categorie_majeure_id, oa_r_categorie_majeure_code, oa_r_categorie_majeure_description, oa_r_categorie_majeure_inactif)
        VALUES (9402, 'ITB', 'Catégorie majeure de test B', 0);
END
