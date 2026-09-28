-- Données de référence du lot N (Theriaque, médicaments, ALD, nomenclatures NOS).
--
-- Rejoué par Preparer-Base.ps1 après 20-reference.sql, avant la prise de
-- l'instantané. Mêmes règles que 20-reference.sql : données fictives, strict
-- nécessaire, rejouable sur une base neuve.
--
-- oa_ald est lue une seule fois, au premier usage, par le singleton Table_ald
-- d'EnvironnementBase : ses lignes ne peuvent pas venir d'un test. Table_ald
-- range chaque ligne dans un dictionnaire code -> description : les codes sont
-- donc uniques et aucune description n'est NULL (DBNull ne se convertit pas en
-- String, le chargement du singleton échouerait).
--
-- Les codes sont des chiffres, pour que l'insertion passe que la colonne
-- oa_ald_code soit numérique ou texte.
--
-- Dépendants :
--   Dao/AldDao.test.vb, Dao/AldCim10Dao.test.vb (GetAldById, rattachement des
--   codes CIM-10 et des antécédents ALD) ;
--   les tests du singleton Table_ald (lot P), qui lisent ces deux lignes.
-- Les constantes correspondantes sont dans Infrastructure/JeuxTheriaque.vb
-- (AldReferenceDiabeteId, AldReferenceCardiaqueId, codes et descriptions).
--
-- Les autres tables du lot (oa_r_medicament, oa_ald_cim10, ans_nos_*, allergies
-- et contre-indications des patients) ne sont lues par aucun singleton : les
-- tests les remplissent eux-mêmes, par JeuxTheriaque.
--
-- L'id est fixé. L'export du schéma peut déclarer la colonne IDENTITY ou non :
-- l'insertion passe par IDENTITY_INSERT seulement dans le premier cas.

SET NOCOUNT ON;

IF COLUMNPROPERTY(OBJECT_ID('oasis.oa_ald'), 'oa_ald_id', 'IsIdentity') = 1
BEGIN
    SET IDENTITY_INSERT oasis.oa_ald ON;
    IF NOT EXISTS (SELECT 1 FROM oasis.oa_ald WHERE oa_ald_id = 9501)
        INSERT INTO oasis.oa_ald (oa_ald_id, oa_ald_code, oa_ald_description)
        VALUES (9501, '91', N'ALD de test : diabète');
    IF NOT EXISTS (SELECT 1 FROM oasis.oa_ald WHERE oa_ald_id = 9502)
        INSERT INTO oasis.oa_ald (oa_ald_id, oa_ald_code, oa_ald_description)
        VALUES (9502, '92', N'ALD de test : insuffisance cardiaque');
    SET IDENTITY_INSERT oasis.oa_ald OFF;
END
ELSE
BEGIN
    IF NOT EXISTS (SELECT 1 FROM oasis.oa_ald WHERE oa_ald_id = 9501)
        INSERT INTO oasis.oa_ald (oa_ald_id, oa_ald_code, oa_ald_description)
        VALUES (9501, '91', N'ALD de test : diabète');
    IF NOT EXISTS (SELECT 1 FROM oasis.oa_ald WHERE oa_ald_id = 9502)
        INSERT INTO oasis.oa_ald (oa_ald_id, oa_ald_code, oa_ald_description)
        VALUES (9502, '92', N'ALD de test : insuffisance cardiaque');
END
