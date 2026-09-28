-- Données de référence du domaine Patient (lot F des tests d'intégration).
--
-- Rejoué par Preparer-Base.ps1 après 20-reference.sql, donc inclus dans
-- l'instantané. Mêmes règles que 20-reference.sql : données fictives, le strict
-- nécessaire, instructions rejouables sur une base neuve.

SET NOCOUNT ON;

-- Genres (oa_r_genre), lus une fois pour tout le processus par le singleton
-- Table_genre d'EnvironnementBase. PatientDao.Completer en tire le libellé du
-- genre (Patient.PatientGenre) : IntegrationTest/Dao/PatientDao.test.vb relit ce
-- libellé et le compare à cette table. Les codes sont ceux de
-- Patient.EnumGenreId. IF NOT EXISTS : un autre script de référence peut déjà
-- les avoir posés.
IF NOT EXISTS (SELECT 1 FROM oasis.oa_r_genre WHERE oa_r_genre_code = 'F')
    INSERT INTO oasis.oa_r_genre (oa_r_genre_code, oa_r_genre_description, oa_r_genre_inactif)
    VALUES ('F', N'Féminin', 0);

IF NOT EXISTS (SELECT 1 FROM oasis.oa_r_genre WHERE oa_r_genre_code = 'M')
    INSERT INTO oasis.oa_r_genre (oa_r_genre_code, oa_r_genre_description, oa_r_genre_inactif)
    VALUES ('M', N'Masculin', 0);
