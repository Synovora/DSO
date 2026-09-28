-- Données de référence du domaine sous-épisode (lot H de l'étape 2).
--
-- Preparer-Base.ps1 rejoue ce script après 20-reference.sql : ces lignes font
-- partie de l'instantané. Aucun singleton d'EnvironnementBase ne lit ces tables,
-- mais tout sous-épisode a besoin d'un type et d'un sous-type (jointures internes
-- de SousEpisodeDao), et JeuxSousEpisode.CreerSousEpisode, que d'autres lots
-- appellent, s'appuie sur ces identifiants. Les constantes correspondantes sont
-- dans IntegrationTest/Infrastructure/JeuxSousEpisode.vb.
--
-- Toutes les colonnes sont renseignées : les cas de colonnes NULL sont créés par
-- les tests eux-mêmes, pour qu'une contrainte inattendue du schéma ne fasse
-- échouer que le test concerné et pas la préparation de la base.
--
-- Identifiants explicites : IDENTITY_INSERT n'est activé que si la colonne est
-- une identité (le schéma n'est pas encore connu).

SET NOCOUNT ON;

-- Types
IF OBJECTPROPERTY(OBJECT_ID('oasis.oa_r_sous_episode_type'), 'TableHasIdentity') = 1
    SET IDENTITY_INSERT oasis.oa_r_sous_episode_type ON;

INSERT INTO oasis.oa_r_sous_episode_type (id, categorie, horodate_creation, libelle, is_with_destinataire)
VALUES
    (901, 'COURRIER', '2026-01-05T09:00:00', 'Courrier', 1),
    (902, 'CERTIFICAT', '2026-01-05T09:00:00', 'Certificat', 0);

IF OBJECTPROPERTY(OBJECT_ID('oasis.oa_r_sous_episode_type'), 'TableHasIdentity') = 1
    SET IDENTITY_INSERT oasis.oa_r_sous_episode_type OFF;
GO

-- Sous-types
IF OBJECTPROPERTY(OBJECT_ID('oasis.oa_r_sous_episode_sous_type'), 'TableHasIdentity') = 1
    SET IDENTITY_INSERT oasis.oa_r_sous_episode_sous_type ON;

INSERT INTO oasis.oa_r_sous_episode_sous_type
    (id, id_sous_episode_type, horodate_creation, libelle, redaction_profil_types, validation_profil_types,
     is_ald_possible, is_reponse_requise, delai_reponse, commentaire)
VALUES
    (911, 901, '2026-01-05T09:00:00', 'Adressage', 'MEDICAL,PARAMEDICAL', 'MEDICAL', 1, 1, 10, 'Adressage a un specialiste'),
    (912, 901, '2026-01-05T09:00:00', 'Compte rendu', 'MEDICAL', 'MEDICAL', 0, 0, 15, 'Compte rendu de consultation'),
    (921, 902, '2026-01-05T09:00:00', 'Certificat medical', 'MEDICAL', 'MEDICAL', 0, 0, 15, 'Certificat de non contre-indication');

IF OBJECTPROPERTY(OBJECT_ID('oasis.oa_r_sous_episode_sous_type'), 'TableHasIdentity') = 1
    SET IDENTITY_INSERT oasis.oa_r_sous_episode_sous_type OFF;
GO

-- Sous-sous-types (lignes de détail proposées dans la fenêtre du sous-épisode)
IF OBJECTPROPERTY(OBJECT_ID('oasis.oa_r_sous_episode_sous_sous_type'), 'TableHasIdentity') = 1
    SET IDENTITY_INSERT oasis.oa_r_sous_episode_sous_sous_type ON;

INSERT INTO oasis.oa_r_sous_episode_sous_sous_type (id, id_sous_episode_sous_type, horodate_creation, libelle, commentaire)
VALUES
    (931, 911, '2026-01-05T09:00:00', 'Bilan biologique', 'Bilan'),
    (932, 911, '2026-01-05T09:00:00', 'Imagerie', 'Imagerie'),
    (933, 921, '2026-01-05T09:00:00', 'Aptitude au sport', 'Aptitude');

IF OBJECTPROPERTY(OBJECT_ID('oasis.oa_r_sous_episode_sous_sous_type'), 'TableHasIdentity') = 1
    SET IDENTITY_INSERT oasis.oa_r_sous_episode_sous_sous_type OFF;
GO
