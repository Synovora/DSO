-- Données de référence du lot M (parcours de soins, ROR, chaînes d'épisodes,
-- contextes, ligne de vie).
--
-- Rejoué par Preparer-Base.ps1 après 20-reference.sql, avant la prise de
-- l'instantané. Mêmes règles que 20-reference.sql : données fictives, strict
-- nécessaire, rejouable sur une base neuve.
--
-- Spécialités (oa_r_specialite). Le singleton Table_specialite
-- d'EnvironnementBase lit cette table une seule fois pour tout le processus :
-- ParcoursDao.GetListOfIntervenantNonOasisByPatient en tire le libellé de la
-- spécialité, TacheDao.CreationAutomatiqueDeDemandeRendezVous le délai de prise
-- en charge. Ses lignes ne peuvent donc pas venir d'un test.
--   1, 2       : médecin référent et IDE Oasis (EnumSpecialiteOasis), que
--                ParcoursDao.CreateIntervenantOasisByPatient pose en dur.
--   9801, 9802 : spécialités hors Oasis actives, pour les intervenants de test.
--   9803       : spécialité inactive : absente du singleton, lisible par
--                SpecialiteDao.GetSpecialiteById.
-- Toutes les colonnes que lisent SpecialiteDao.BuildBean et le singleton sont
-- renseignées. Dépendants : Dao/SpecialiteDao.test.vb, Dao/ParcoursDao.test.vb,
-- et les tests du singleton Table_specialite (lot P), qui lisent ces lignes.
-- Constantes : Infrastructure/JeuxParcours.vb.
--
-- Intervenants du ROR (oa_ror) 1 et 2 : CreateIntervenantOasisByPatient les
-- rattache en dur au parcours par défaut (médecin référent, IDE). oa_ror n'est
-- lue par aucun singleton, mais ces deux ids fixes doivent exister avant tout
-- test qui crée un patient entré dans le dispositif. oa_ror_oasis à 1 : ils sont
-- exclus de ParcoursDao.GetAllEmailParcoursbyPatient.
--
-- Les ids sont fixés. L'export du schéma peut déclarer les colonnes IDENTITY ou
-- non : l'insertion passe par IDENTITY_INSERT seulement dans le premier cas.
-- IF NOT EXISTS : un autre script de référence peut avoir posé les mêmes ids.

SET NOCOUNT ON;

DECLARE @specialites TABLE (
    id INT, code NVARCHAR(20), description NVARCHAR(100), nature NVARCHAR(20),
    parcours BIT, oasis BIT, genre NVARCHAR(1), age_min INT, age_max INT, delai INT,
    g15 INT, r04 NVARCHAR(10), savoir_faire NVARCHAR(10), inactif BIT);

INSERT INTO @specialites VALUES
    (1,    N'ITMR', N'Médecin référent de test', N'MEDICAL',     1, 1, N'', 0, 0, 60, 10, N'S', N'SM54', 0),
    (2,    N'ITIDE', N'IDE de test',             N'PARAMEDICAL', 1, 1, N'', 0, 0, 60, 60, N'S', N'SI01', 0),
    (9801, N'ITCA', N'Cardiologie de test',      N'MEDICAL',     1, 0, N'', 0, 0, 45, 10, N'S', N'SM04', 0),
    (9802, N'ITDE', N'Dermatologie de test',     N'MEDICAL',     1, 0, N'', 0, 0, 20, 10, N'S', N'SM15', 0),
    (9803, N'ITIN', N'Spécialité inactive de test', N'MEDICAL',  0, 0, N'F', 12, 60, 10, 10, N'S', N'SM99', 1);

IF COLUMNPROPERTY(OBJECT_ID('oasis.oa_r_specialite'), 'oa_r_specialite_id', 'IsIdentity') = 1
BEGIN
    SET IDENTITY_INSERT oasis.oa_r_specialite ON;
    INSERT INTO oasis.oa_r_specialite
        (oa_r_specialite_id, oa_specialite_code, oa_r_specialite_description, oa_r_specialite_nature,
         oa_r_parcours, oa_r_oasis, oa_r_specialite_genre, oa_r_specialite_age_min, oa_r_specialite_age_max,
         oa_r_delaiPriseEnCharge, oa_r_code_nos_g15_profession, oa_r_code_nos_r04_type_savoir_faire,
         oa_r_code_savoir_faire, oa_r_specialite_inactif)
    SELECT s.id, s.code, s.description, s.nature, s.parcours, s.oasis, s.genre, s.age_min, s.age_max,
           s.delai, s.g15, s.r04, s.savoir_faire, s.inactif
    FROM @specialites s
    WHERE NOT EXISTS (SELECT 1 FROM oasis.oa_r_specialite r WHERE r.oa_r_specialite_id = s.id);
    SET IDENTITY_INSERT oasis.oa_r_specialite OFF;
END
ELSE
BEGIN
    INSERT INTO oasis.oa_r_specialite
        (oa_r_specialite_id, oa_specialite_code, oa_r_specialite_description, oa_r_specialite_nature,
         oa_r_parcours, oa_r_oasis, oa_r_specialite_genre, oa_r_specialite_age_min, oa_r_specialite_age_max,
         oa_r_delaiPriseEnCharge, oa_r_code_nos_g15_profession, oa_r_code_nos_r04_type_savoir_faire,
         oa_r_code_savoir_faire, oa_r_specialite_inactif)
    SELECT s.id, s.code, s.description, s.nature, s.parcours, s.oasis, s.genre, s.age_min, s.age_max,
           s.delai, s.g15, s.r04, s.savoir_faire, s.inactif
    FROM @specialites s
    WHERE NOT EXISTS (SELECT 1 FROM oasis.oa_r_specialite r WHERE r.oa_r_specialite_id = s.id);
END

-- Mêmes colonnes que l'INSERT de RorDao.CreationRor, plus oa_ror_oasis.
DECLARE @rors TABLE (
    id INT, specialite INT, nom NVARCHAR(100), structure_nom NVARCHAR(100), email NVARCHAR(100));

INSERT INTO @rors VALUES
    (1, 1, N'MEDECIN REFERENT OASIS DE TEST', N'Centre Oasis de test', N'medecin.oasis@exemple.fr'),
    (2, 2, N'IDE OASIS DE TEST',              N'Centre Oasis de test', N'ide.oasis@exemple.fr');

IF COLUMNPROPERTY(OBJECT_ID('oasis.oa_ror'), 'oa_ror_id', 'IsIdentity') = 1
BEGIN
    SET IDENTITY_INSERT oasis.oa_ror ON;
    INSERT INTO oasis.oa_ror
        (oa_ror_id, oa_ror_specialite_id, oa_ror_nom, oa_ror_oasis, oa_ror_type, oa_ror_structure_id, oa_ror_structure_nom,
         oa_ror_adresse1, oa_ror_adresse2, oa_ror_code_postal, oa_ror_ville,
         oa_ror_code, oa_ror_telephone, oa_ror_email, oa_ror_commentaire, oa_ror_rpps, oa_ror_finess, oa_ror_adeli,
         oa_ror_inactif, oa_ror_user_creation, oa_ror_date_creation,
         oa_ror_extraction_annuaire, oa_ror_identifiant_national_pp, oa_ror_identifiant_technique_structure,
         oa_ror_code_nos_r23_mode_exercice, oa_ror_code_nos_g15_profession_sante, oa_ror_code_nos_r04_type_savoir_faire,
         oa_ror_code_savoir_faire, oa_ror_cle_reference)
    SELECT r.id, r.specialite, r.nom, 1, N'Intervenant', 0, r.structure_nom,
           N'1 place du Test', N'', N'97600', N'Mamoudzou',
           N'', N'0269000000', r.email, N'', 0, 0, 0,
           0, 0, CAST('20260101 00:00:00' AS DATETIME),
           0, N'', N'', N'', 0, N'', N'', 0
    FROM @rors r
    WHERE NOT EXISTS (SELECT 1 FROM oasis.oa_ror o WHERE o.oa_ror_id = r.id);
    SET IDENTITY_INSERT oasis.oa_ror OFF;
END
ELSE
BEGIN
    INSERT INTO oasis.oa_ror
        (oa_ror_id, oa_ror_specialite_id, oa_ror_nom, oa_ror_oasis, oa_ror_type, oa_ror_structure_id, oa_ror_structure_nom,
         oa_ror_adresse1, oa_ror_adresse2, oa_ror_code_postal, oa_ror_ville,
         oa_ror_code, oa_ror_telephone, oa_ror_email, oa_ror_commentaire, oa_ror_rpps, oa_ror_finess, oa_ror_adeli,
         oa_ror_inactif, oa_ror_user_creation, oa_ror_date_creation,
         oa_ror_extraction_annuaire, oa_ror_identifiant_national_pp, oa_ror_identifiant_technique_structure,
         oa_ror_code_nos_r23_mode_exercice, oa_ror_code_nos_g15_profession_sante, oa_ror_code_nos_r04_type_savoir_faire,
         oa_ror_code_savoir_faire, oa_ror_cle_reference)
    SELECT r.id, r.specialite, r.nom, 1, N'Intervenant', 0, r.structure_nom,
           N'1 place du Test', N'', N'97600', N'Mamoudzou',
           N'', N'0269000000', r.email, N'', 0, 0, 0,
           0, 0, CAST('20260101 00:00:00' AS DATETIME),
           0, N'', N'', N'', 0, N'', N'', 0
    FROM @rors r
    WHERE NOT EXISTS (SELECT 1 FROM oasis.oa_ror o WHERE o.oa_ror_id = r.id);
END
