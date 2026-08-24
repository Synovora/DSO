# Credential rotation runbook

Written 2026-08-23, after the repository was made public with a live `sa` password,
a hardcoded encryption key, and a publicly known fallback signing key in its history.

**Nothing in this runbook has been executed.** The application-side changes it depends on have
been made (see the migration script `docs/migrations/2026-08-23-durcissement-securite.sql` and the
commits of 2026-08-23); the credential changes below still need a database, a maintenance window
and a client release. The audit report behind this work is held outside the repository until the
steps below are done, because publishing it describes holes that are still open in production. It needs SQL Server access and a maintenance
window. Read the whole thing before starting: the order matters, and the wrong order takes the
system down for every clinician.

## Why order matters

Desktop clients do not hold database credentials. They call `/api/login`, and the server hands
back the connection string encrypted with `OasisCryptoKey`. So:

- Change the database password without updating the server config, and the portal and every
  client break at once.
- Change `OasisCryptoKey` on the server without shipping the same value to clients, and every
  client fails to decrypt the login response. They cannot self-recover; the value is in their
  own `App.config`.

Both values have to move together with a client release.

## Before you start

- [ ] Confirm a maintenance window. Clients holding a cached connection string keep working until
      they next log in, so pick a time when few people are mid-session.
- [ ] Have the ClickOnce publish ready to go. The client release is part of this, not a follow-up.
- [ ] Take a database backup.
- [ ] Know how to roll back: keep the old login enabled until the new one is proven.

## Step 1: Stop using `sa`

The application connects as `sa`, so a SQL injection or a leaked string gives away the whole
instance rather than one database. Create a least-privilege login first.

```sql
USE master;
GO
CREATE LOGIN oasis_app
    WITH PASSWORD = N'<generate: openssl rand -base64 24>',
         CHECK_POLICY = ON;
GO

USE oasis;
GO
CREATE USER oasis_app FOR LOGIN oasis_app;
GO

-- Data access only, scoped to the application schema. No DDL, no server-level rights.
GRANT SELECT, INSERT, UPDATE, DELETE ON SCHEMA::oasis TO oasis_app;
GRANT EXECUTE ON SCHEMA::oasis TO oasis_app;
GO
```

If the application turns out to need more, add it explicitly rather than granting `db_owner`.
Watch the error log during the window and grant what actually fails.

## Step 2: Move the application onto the new logins

There are two now, and they are not interchangeable. `oasis_web` is the server's own account and
never leaves the machine. `oasis_client` is what `/api/login` distributes to every workstation, and
it is denied the credential columns. Putting the server's account in the client entry undoes the
whole separation, so check that line twice.

- [ ] Run [`../migrations/2026-08-24-comptes-sql-separes.sql`](../migrations/2026-08-24-comptes-sql-separes.sql)
      after creating both logins. It grants the schema and applies the column-level denials.
- [ ] Update both `connectionStrings` entries in the server's `Oasis_Web/Web.config`:
      `oasisConnection` to `user id=oasis_web`, `oasisConnectionClient` to `user id=oasis_client`.
- [ ] Set `trustServerCertificate=false` in both, and install a certificate SQL Server can present.
- [ ] Recycle the IIS application pool.
- [ ] Confirm the patient portal loads and a patient can sign in.
- [ ] Confirm one desktop client can log in, which proves `/api/login` still returns a working
      connection string.
- [ ] Confirm a prescriber can sign a prescription, which proves `/api/signature` works and that the
      client no longer needs the key column.
- [ ] Confirm a user can change their own password, which proves `/api/motdepasse` works.
- [ ] Verify the denials took, as `oasis_client`:
      `SELECT TOP 1 cle_privee FROM oasis.oa_utilisateur;` must fail.

Do not continue until both work. Rolling back at this point is just restoring the old
`Web.config` and recycling.

## Step 3: Rotate the `sa` password

Only once step 2 is proven, so `sa` is no longer on the application's path.

```sql
USE master;
GO
ALTER LOGIN sa WITH PASSWORD = N'<generate: openssl rand -base64 24>';
GO

-- Preferred, if nothing else depends on it:
ALTER LOGIN sa DISABLE;
GO
```

The old value (see the previous credential store) is public. Treat it as compromised on every system where it was reused,
not only this database.

- [ ] Update the SSIS package connection in `Oasis_IS`, which also connects as `sa`
      (`Oasis_IS/Package.dtsx`, connection manager `ns3119889.ip-51-38-181.eu.sa`). Its password is
      stored as a sensitive parameter and is not in the repository, but it exists on whichever
      machine runs the package.
- [ ] Check for other consumers: scheduled jobs, backup scripts, monitoring, linked servers.

## Step 4: Replace the encryption key

`OasisCryptoKey` replaces the constant that used to live in
`Oasis_Common/Module/ModuleUtilsBase.vb`. That old value is public, so the new one must differ.

```bash
openssl rand -base64 32
```

- [ ] Set the same value in the server's `Web.config` and in `oasis/App.config`.
- [ ] Build and publish a new ClickOnce release carrying that `App.config`.
- [ ] Raise `MinimumRequiredVersion` in `oasis/Oasis_WF.vbproj` so clients are forced to take it
      rather than being allowed to skip.
- [ ] Deploy the server and the client release together.

A client on the old release will fail at login with a decryption error until it updates. Forcing
the update is what keeps that window short.

## Step 5: Reissue signing keys

Prescription signing keys are stored in plaintext in `oasis.oa_utilisateur.cle_privee`.

Until 2026-08-23 the code fell back to the private key
`0x0000000000000000000000000000000000000000000000000000000000000001` whenever that column was
null, together with its derived address `0x7E5F4552091A69125d5DfCb7b8C2659029395Bdf`. Both are
publicly known values. Any prescription signed on that fallback can be forged by anyone and will
still verify at `/Sign/Check/`.

The fallback is now removed: signing raises an error instead. Find who was affected.

```sql
-- Users with no key. Signing now fails for these until a key is generated.
SELECT oa_utilisateur_id, oa_utilisateur_login
FROM   oasis.oa_utilisateur
WHERE  cle_privee IS NULL OR LTRIM(RTRIM(cle_privee)) = '';

-- Users explicitly carrying the known-compromised key.
SELECT oa_utilisateur_id, oa_utilisateur_login
FROM   oasis.oa_utilisateur
WHERE  cle_privee = '0x0000000000000000000000000000000000000000000000000000000000000001'
   OR  cle_publique = '0x7E5F4552091A69125d5DfCb7b8C2659029395Bdf';

-- Prescriptions signed by anyone in that second set. These signatures are forgeable
-- and should be treated as unverified.
SELECT o.oa_ordonnance_id, o.oa_ordonnance_signature, u.oa_utilisateur_login
FROM   oasis.oa_patient_ordonnance o
JOIN   oasis.oa_utilisateur u ON u.oa_utilisateur_id = o.oa_ordonnance_user_validation
WHERE  u.cle_publique = '0x7E5F4552091A69125d5DfCb7b8C2659029395Bdf';
```

- [ ] Generate a fresh key for every affected user. Keys are now made by the server: post to
      `/api/signature/cle` with `Remplacer = true` and an administrator's credentials, or clear
      `cle_privee` and `cle_publique` for those users and reopen each record, which makes the
      desktop client request a key for any account that has none.
      Note that rotating a key does not invalidate signatures already made with the old one:
      `oa_ordonnance_signature_adresse` records the address used at signing time.
- [ ] Decide what to do about prescriptions already signed with the compromised key. They cannot
      be made trustworthy retroactively. At minimum, record which ones they are.

## Step 6: Encrypt signing keys at rest

Not done, and not safe to do without a migration.

Option 1 below is done: signing moved to `/api/signature` on 2026-08-24, and the `oasis_client`
login is denied both read and write on `cle_privee`. A workstation can no longer steal or replace a
prescriber's key. What remains is that the column still holds plaintext, so anyone with `oasis_web`
rights, a database backup, or `sa` can sign as any clinician.

Encrypting the column means migrating existing rows, and a half-applied change (code expecting
ciphertext, database holding plaintext) destroys every signing key in the system.

Options, roughly in order of preference:

1. ~~Move signing behind an API so private keys never leave the server.~~ Done 2026-08-24.
2. SQL Server Always Encrypted on that column, so the database never sees plaintext. Randomized
   encryption is fine, nothing searches or joins on the column. The column master key belongs in
   the web server's certificate store, which also puts the DBA outside the trust boundary.
3. Application-level encryption under a key held outside the database. Cheapest, and the weakest,
   since anyone with the application config plus the database has both halves.

Whichever is chosen, it needs its own plan with a migration and a rehearsal on a restored backup.

## After

- [ ] Confirm no plaintext credential remains in `Oasis_Web/Web.config` on the server outside of
      the deployment's own secret handling.
- [ ] Confirm the local archive tag `archive/pre-reset-2026-08-23`, which still contains the old
      leaked values, is not pushed anywhere.
- [ ] Assume any credential that was ever in the public repository is known. Rotation limits
      future damage, it does not undo past exposure.
