# Authorisation model

**Status: first draft, for review.** Derived from the checks that exist in the code today, plus the
decisions that have to be made because no equivalent exists. Sections marked **DECISION** are open
questions, not proposals to implement as written. Nothing here is enforced yet.

This document is the input to the API migration. Every endpoint will be expected to name the rule
it enforces, and the rule has to be in this file for that to be possible. It is also the input to
the DPIA and the reference an auditor will ask for.

## 1. Actors

Two kinds of account, easy to confuse because both are "users".

**`Utilisateur`** is a member of staff who signs into the desktop client. Carries:

| Field | Values | Purpose today |
|---|---|---|
| `UtilisateurProfilId` | `MEDECIN`, `SAGE_FEMME`, `IDE`, `IDE_REMPLACANT`, `CADRE_SANTE`, `SECRETAIRE_MEDICALE`, `ADMINISTRATIF`, `INFORMATICIEN` | job title, drives task routing |
| `TypeProfil` | `MEDICAL`, `PARAMEDICAL`, `GESTION`, `ACCUEIL`, `PATIENT` | the coarse clinical distinction, and the only one used for access |
| `UtilisateurAdmin` | boolean | override on everything |
| `UtilisateurNiveauAcces` | integer from `oa_r_profil`, defaults to 3 | used in exactly one place |
| `LstFonction` | from `oa_r_fonction` via the profile | task assignment, not access |
| `UtilisateurSiegeId` / `UniteSanitaireId` / `SiteId` | structure hierarchy | defaults and display only |

**`Internaute`** is a patient who signs into the web portal. Linked to a `Patient` through
`oa_internaute_permission` (`internaute`, `patient`, `permission`).

The structure hierarchy is **siège → unité sanitaire → site**, and a patient belongs to one site
(`oa_patient_site_id`).

## 2. The four overlapping mechanisms

`ProfilId`, `TypeProfil`, `NiveauAcces`, `Fonction` and the `Admin` flag all encode something close
to "what may this person do", and they disagree with each other. That is the first thing to fix,
because a rule that can be expressed four ways will be enforced inconsistently.

**DECISION 1.** Collapse to two concepts:

- **Role**, granting a named set of permissions. Derived from `ProfilId`, which is the value people
  actually understand. `TypeProfil` becomes a property of the role rather than a parallel axis.
- **Fonction** stays, but only for task routing (`oa_tache` emitter and recipient). It is workflow,
  not authorisation, and should never be consulted for an access decision.

`NiveauAcces` is dropped. It appears in one check (`RadFSynthese.vb:2776`) and its default of 3 means
nothing.

> Note: `FonctionDao.EnumFonction` defines `IDE = 4` and `IDE_REMPLACANT = 4`. The two are the same
> value, so a replacement nurse can never be distinguished from a permanent one anywhere in the
> system. Whatever is decided above, that needs correcting.

**DECISION 2.** `UtilisateurAdmin` is a single boolean that overrides every rule in the application.
Split it into named administrative roles so that "can manage staff accounts" is not the same grant as
"can edit anyone's clinical observation after the fact":

- `ADMIN_UTILISATEURS`: create, modify, deactivate staff accounts; trigger key generation.
- `ADMIN_REFERENTIEL`: reference data (sites, unités, spécialités, DRC, templates).
- `ADMIN_CLINIQUE`: the override on clinical records currently granted by the flag. Should be rare,
  and every use of it logged as an override.

## 3. Patient scope: the missing dimension

**There is no patient-scoping rule in the system today.** The site filter passed to
`PatientDao.GetAllPatientWithFilter` comes from a `FiltreTache` that starts empty, so no restriction
is applied, and the user's own site and unité sanitaire are never consulted for access. Any
authenticated member of staff can list and open any patient record.

This is the single most consequential gap, and it cannot be derived from the code because it was
never expressed. It is a clinical and legal decision.

**DECISION 3.** On what basis may a member of staff reach a patient record? Options, not exclusive:

| Basis | Rule | Consequence |
|---|---|---|
| Structure | patient's site is within the user's unité sanitaire, or siège | simple, coarse, matches how the data is already shaped |
| Care relationship | user participates in an episode for that patient, or is named on a PPS or parcours | tightest, matches "need to know", but needs a first contact to exist |
| Explicit assignment | a table linking user to patient | precise, administratively heavy |
| Break-glass | any patient, on stated justification, logged and reviewed | required for urgent care, and dangerous without the review |

A common shape for care settings is structure as the baseline, care relationship to widen it across
structures, and break-glass for the rest with mandatory justification and after-the-fact review.
Whatever is chosen, break-glass has to exist: a rule that blocks urgent care will be worked around.

**DECISION 4.** Does scope apply to *existence* or only to *content*? Being told "no such patient"
prevents an identity from being confirmed. Being told "access denied" is friendlier and admits the
patient exists. Health settings usually accept the second for staff and the first for the portal.

## 4. Rules that do exist and should be kept

These were derived from the code and are genuine, considered rules. They should survive the
migration rather than be reinvented.

**Medical lock (`BlocageMedical`).** `outils.AccesFonctionMedicaleSynthese`: a `MEDICAL` profile
always reaches the medical part of a record; a `PARAMEDICAL` profile reaches it only when the patient
is not medically locked. `GESTION` and `ACCUEIL` never do.

**Observation authorship and window.** `RadFEpisodeObservationDetailEdit.vb:32-45`: an episode
observation is modifiable by its author, within 24 hours of creation, and by an administrator
thereafter. A `MEDICAL` user may not edit a `PARAMEDICAL` observation and the reverse. This is the
closest thing in the codebase to a proper medical-record integrity rule, and it is the right instinct.

**Task ownership.** `TacheClass.IsMyTacheATraiter`: a task is actionable by a user whose fonction
matches the task's `TraiteFonctionId`, or by an administrator.

**Screen access by profile.** `RadFPatientListe.InitHabilitation`: episode screens and the action
list require `MEDICAL` or `PARAMEDICAL`; patient creation additionally allows `ACCUEIL`.

## 5. Draft matrix

Operations: **R** read, **W** create and modify, **D** delete or deactivate, **S** sign, **X** export
or print or mail. A blank cell is no access. `adm` means the corresponding administrative role from
DECISION 2.

All of it is subject to patient scope (DECISION 3). Scope is a separate gate applied first: this
matrix says what a role may do *to a patient it may reach*.

| Resource | MEDECIN, SAGE_FEMME (`MEDICAL`) | IDE, IDE_REMPLACANT, CADRE_SANTE (`PARAMEDICAL`) | SECRETAIRE_MEDICALE (`ACCUEIL`) | ADMINISTRATIF (`GESTION`) | Internaute (patient) |
|---|---|---|---|---|---|
| Patient, état civil | R W | R W | R W | R | R own |
| Patient, NIR / INS | R W | R | R | | R own |
| Épisode, sous-épisode | R W D | R W D | | | R own |
| Observation d'épisode | R W (own, 24h) | R W (own, 24h) | | | |
| Synthèse médicale | R | R if not `BlocageMedical` | | | R own |
| Antécédent, contexte | R W | R W if not `BlocageMedical` | | | R own |
| Ordonnance | R W **S** X | R | | | R own X |
| Traitement | R W | R | | | R own |
| Vaccin, carnet vaccinal | R W X | R W X | | | R own X |
| PPS, parcours | R W | R W | | | R own |
| Ligne de vie, paramètres | R W | R W | | | R W own (auto-suivi) |
| DRC | R W | R | | | |
| Tâche, rendez-vous | R W | R W | R W | | R own |
| Document de sous-épisode | R W X | R W X | R X | | R own |
| Envoi de mail | X | X | X | | |
| Compte `Utilisateur` | | | | | |
| Compte `Internaute` | W | W | W | | |
| Référentiels | | | | R | |
| Journal d'accès | | | | | |

Rows left deliberately empty in every column are the ones that must be `adm` only: staff accounts
(`ADMIN_UTILISATEURS`), reference data (`ADMIN_REFERENTIEL`), and the access journal, which nobody
should be able to write and only a named auditor role should read.

**DECISION 5.** Prescription signing is currently open to any profile whose account happens to hold
a key. It should be restricted to roles with prescribing authority, and the RPPS number should be
mandatory for those roles (`FrmUtilisateur` already requires it for `MEDICAL` and `PARAMEDICAL`).
Whether `SAGE_FEMME` and `IDE` may prescribe, and what, is a regulatory question rather than a
technical one.

**DECISION 6.** Creating a portal account for a patient is currently available from the patient
screen to anyone who can open it. That grants a third party access to a medical record, so it
deserves its own permission and its own audit entry.

**DECISION 7.** The portal links one `Internaute` to patients through `oa_internaute_permission`,
but `PortailController.GetPatientConnecte` takes `permissions(0)` and the `permission` column is
always written as `1` and never read. Decide whether one account may cover several patients (a parent
and their children is the obvious case) and what the permission levels mean. Until then the column is
misleading and should either be used or removed.

## 6. Enforcement contract

Rules for the API migration. These are what make the model real rather than documentary.

1. **Deny by default.** The authentication filter is registered globally; an endpoint that wants to
   be anonymous says so explicitly.
2. **One decision point.** Every endpoint calls the same authorisation service with (caller,
   resource, operation, patient). No endpoint reimplements a rule.
3. **Scope before content.** The patient-scope gate runs before anything is read, not as a filter on
   results.
4. **Server-side only.** Hiding a button is a usability affordance, not a control. Every rule in this
   document is enforced on the server regardless of what the client sends. Today all of them are
   enforced in the UI only.
5. **The identity is the token, never the payload.** No endpoint accepts a user id, profile or
   patient id as an assertion of who is calling.
6. **Log the decision, not just the action.** Grants and denials both, with the rule that fired.
   Reads of a patient record are logged as well as writes: health data access logging is expected to
   cover consultation.
7. **Every override is an event.** Break-glass and administrative overrides produce a distinct,
   reviewable entry rather than blending into ordinary traffic.

## 7. Out of scope for this document

Authentication strength (MFA, CPS and Pro Santé Connect), signature conformance under eIDAS,
retention and deletion policy, and the hosting arrangements. Each needs its own decision and each has
regulatory input this document cannot supply.

## 8. Next step

Review sections marked DECISION with the clinical leads and whoever owns data protection. Decisions
3 and 1 block the API design; the rest can follow. Once agreed, this file becomes the reference the
endpoints are written against and the matrix becomes a test suite.
