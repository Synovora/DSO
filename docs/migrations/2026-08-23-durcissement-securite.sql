-- Migration accompagnant le durcissement de sécurité du 2026-08-23.
-- À exécuter sur la base `oasis` AVANT de déployer la nouvelle version du portail
-- et du client lourd. Toutes les instructions sont idempotentes.
--
-- Ordre : ce script d'abord, puis Oasis_Web, puis la publication ClickOnce.

SET XACT_ABORT ON;
BEGIN TRANSACTION;

-- ---------------------------------------------------------------------------
-- 1. Récupération de mot de passe du portail patient (A3, A11)
--    La clé de récupération devient à usage unique et expirante.
-- ---------------------------------------------------------------------------
IF COL_LENGTH('oasis.oa_internaute', 'recovery_expiration') IS NULL
    ALTER TABLE oasis.oa_internaute ADD recovery_expiration DATETIME2 NULL;
GO

-- Les clés déjà présentes n'ont pas de date d'expiration : on les neutralise
-- pour qu'aucun ancien lien ne reste utilisable indéfiniment.
UPDATE oasis.oa_internaute
   SET recovery = NULL, recovery_expiration = NULL
 WHERE recovery IS NOT NULL AND recovery_expiration IS NULL;
GO

-- Une chaîne vide n'est plus une valeur valide (elle rendait la clé « vide »
-- exploitable). On normalise en NULL.
UPDATE oasis.oa_internaute SET recovery = NULL WHERE recovery = '';
UPDATE oasis.oa_internaute SET code = NULL WHERE code = '';
GO

-- ---------------------------------------------------------------------------
-- 2. Verrouillage des comptes côté serveur (A10)
--    Le compteur d'essais vivait uniquement dans la base de registre du poste.
-- ---------------------------------------------------------------------------
IF COL_LENGTH('oasis.oa_internaute', 'tentatives') IS NULL
    ALTER TABLE oasis.oa_internaute ADD tentatives INT NOT NULL CONSTRAINT DF_oa_internaute_tentatives DEFAULT 0;
GO
IF COL_LENGTH('oasis.oa_internaute', 'verrou_jusqua') IS NULL
    ALTER TABLE oasis.oa_internaute ADD verrou_jusqua DATETIME2 NULL;
GO

IF COL_LENGTH('oasis.oa_utilisateur', 'oa_utilisateur_tentatives') IS NULL
    ALTER TABLE oasis.oa_utilisateur ADD oa_utilisateur_tentatives INT NOT NULL CONSTRAINT DF_oa_utilisateur_tentatives DEFAULT 0;
GO
IF COL_LENGTH('oasis.oa_utilisateur', 'oa_utilisateur_verrou_jusqua') IS NULL
    ALTER TABLE oasis.oa_utilisateur ADD oa_utilisateur_verrou_jusqua DATETIME2 NULL;
GO

-- ---------------------------------------------------------------------------
-- 3. Signature des ordonnances (A7)
--    On conserve la charge signée et l'adresse du signataire, sans quoi aucune
--    vérification cryptographique n'est possible après coup.
-- ---------------------------------------------------------------------------
IF COL_LENGTH('oasis.oa_patient_ordonnance', 'oa_ordonnance_signature_payload') IS NULL
    ALTER TABLE oasis.oa_patient_ordonnance ADD oa_ordonnance_signature_payload VARBINARY(MAX) NULL;
GO
IF COL_LENGTH('oasis.oa_patient_ordonnance', 'oa_ordonnance_signature_adresse') IS NULL
    ALTER TABLE oasis.oa_patient_ordonnance ADD oa_ordonnance_signature_adresse NVARCHAR(42) NULL;
GO

-- Les ordonnances signées avant cette migration n'ont pas de charge stockée :
-- elles resteront affichées comme « non vérifiables cryptographiquement ».

-- ---------------------------------------------------------------------------
-- 4. Compte de service pour l'envoi des mails (A1, B2)
--    Créer un utilisateur dédié, sans accès clinique, et renseigner son login
--    et son mot de passe dans MailServiceLogin / MailServicePassword.
--    Le mot de passe du compte Bertrand.Gambet, qui était codé en dur dans le
--    source, doit être changé : voir docs/runbooks/credential-rotation.md.
-- ---------------------------------------------------------------------------

COMMIT TRANSACTION;
GO
