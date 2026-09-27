# Tests d'intégration sur base réelle, étape 1

Date : 2026-09-27. Branche : `dev`.

## But

Le code sans SQL est couvert par `UnitTest` (451 tests). Ce qui reste non testé parle à SQL Server :
les DAO, les contrôleurs de `Oasis_Web`, les caches de données de référence. Cette étape pose le
harnais qui exécute ce code contre une vraie base jetable, puis couvre la partie la plus sensible :
comptes utilisateurs, ordonnances et signatures, restrictions de colonnes du compte `oasis_client`.

Contraintes fixées avec l'utilisateur :

- aucune fonctionnalité ne change ; seules les refactorisations à comportement identique sont
  permises, quand un test ne peut pas atteindre le code autrement, et chacune est signalée dans son
  commit ;
- le schéma vient de la production (export SSMS « schéma seulement »), déposé par l'utilisateur dans
  `IntegrationTest/Schema/00-schema.sql`. Le dépôt est public : le fichier est relu avant commit pour
  s'assurer qu'il ne contient ni données, ni logins, ni utilisateurs.

Étapes suivantes (hors de ce document) : étape 2, cœur clinique (Patient, Episode, SousEpisode,
Traitement, Antecedent, Vaccin, PPS, DRC) ; étape 3, tout le reste, les autres contrôleurs et les
singletons d'`EnvironnementBase`.

## Choix

SQL Server Express en authentification mixte sur le runner Windows, avec de vrais logins
`oasis_web` et `oasis_client`. LocalDB a été écarté : il ne connaît que l'authentification Windows et
ne permet donc pas de vérifier que le code du client lourd respecte les `DENY` de colonnes, qui est
précisément la classe de bug la plus coûteuse ici.

## Projet `IntegrationTest`

Nouveau projet MSTest à l'ancienne (liste `<Compile Include>` explicite), .NET 4.7.2, calqué sur
`UnitTest/UnitTest.vbproj` : mêmes paquets et mêmes redirections de liaison, références de projet vers
`Oasis_Common` et `Oasis_Web`. Ajouté à `Oasis_WF.sln` et à `.github/ci.proj`.

Il est séparé de `UnitTest` parce que ce dernier repose sur l'absence de chaîne de connexion :
`GetConnection()` y échoue instantanément. Mélanger les deux rendrait la suite rapide dépendante
d'une base.

`app.config` reprend les `appSettings` de `UnitTest` et déclare les deux chaînes
`Oasis_WF.My.MySettings.oasisConnection` et `Oasis_WF.My.MySettings.oasisConnectionClient` avec une
valeur vide ; le harnais les remplit à l'exécution.

### Configuration par variables d'environnement

| Variable | Défaut | Rôle |
|---|---|---|
| `OASIS_IT_SERVER` | `localhost\SQLEXPRESS` | instance SQL Server |
| `OASIS_IT_DATABASE` | `oasis_it` | base de test |
| `OASIS_IT_ADMIN_PASSWORD` | vide : sécurité intégrée | mot de passe `sa` |
| `OASIS_IT_WEB_PASSWORD` | aucun | login `oasis_web` |
| `OASIS_IT_CLIENT_PASSWORD` | aucun | login `oasis_client` |
| `OASIS_IT_REQUIRED` | vide | `1` en CI : base injoignable = échec, sinon `Inconclusive` |

Toutes les chaînes sont construites avec `Pooling=False` : la restauration d'instantané exige qu'aucune
session ne reste ouverte sur la base.

### Contrat du harnais (fichiers `IntegrationTest/Infrastructure/`)

Les autres fichiers de test s'appuient uniquement sur ce contrat.

`BaseDeTest.vb`, `Public Module BaseDeTest` :

```vb
Public Enum Compte
    Admin    ' sa ou sécurité intégrée : préparation des données, vérifications directes
    Web      ' oasis_web : ce que voit Oasis_Web
    Client   ' oasis_client : ce que voit le client lourd
End Enum

Function ChaineConnexion(compte As Compte) As String
' Réécrit, par réflexion (_bReadOnly, comme StandardDao.FixConnectionString), la chaîne
' oasisConnection vers le compte donné, et oasisConnectionClient vers le compte Client.
Sub UtiliserCompte(compte As Compte)
' SQL brut sous le compte Admin, paramètres nommés @p0, @p1... dans l'ordre de valeurs.
Function Executer(sql As String, ParamArray valeurs() As Object) As Integer
Function Scalaire(sql As String, ParamArray valeurs() As Object) As Object
' Même chose sous un compte choisi, pour éprouver les droits.
Function ExecuterSous(compte As Compte, sql As String, ParamArray valeurs() As Object) As Integer
Function ScalaireSous(compte As Compte, sql As String, ParamArray valeurs() As Object) As Object
' Remet la base dans l'état de l'instantané oasis_it_instantane.
Sub Reinitialiser()
```

`TestIntegration.vb`, `Public MustInherit Class TestIntegration` : son `<TestInitialize>` appelle
`Reinitialiser()` puis `UtiliserCompte(Compte.Client)`. Toute classe de test en hérite et est donc
isolée des autres, test par test. Un test qui exerce du code serveur appelle
`UtiliserCompte(Compte.Web)` en tête.

`Assemblage.vb` : `<AssemblyInitialize>` vérifie que la base répond (sinon `Inconclusive`, ou échec si
`OASIS_IT_REQUIRED=1`), puis (re)crée l'instantané `oasis_it_instantane`. Instantanés disponibles
dans toutes les éditions depuis SQL Server 2016 SP1, Express compris.

Jeux de données, un fichier par domaine, chacun appartenant à un seul lot (voir plus bas). Règle :
passer par les méthodes de création des DAO quand elles existent (leur `INSERT` est celui que la
production accepte), et par `Executer` seulement pour ce qu'aucun DAO n'écrit.

```vb
' Infrastructure/JeuxUtilisateur.vb
Public Module JeuxUtilisateur
    Public Const MotDePasseParDefaut As String = "MotDePasse!2026"
    ' Crée un utilisateur actif, mot de passe haché comme en production,
    ' avec une paire de clés secp256k1 si avecCle. Renvoie son id.
    Function CreerUtilisateur(Optional login As String = Nothing,
                              Optional motDePasse As String = MotDePasseParDefaut,
                              Optional avecCle As Boolean = True) As Long
End Module

' Infrastructure/JeuxPatient.vb
Public Module JeuxPatient
    Function CreerPatient(Optional nom As String = "TEST", Optional prenom As String = "Patient") As Long
End Module

' Infrastructure/JeuxOrdonnance.vb
Public Module JeuxOrdonnance
    ' Ordonnance avec nbLignes lignes, non signée.
    Function CreerOrdonnance(patientId As Long, utilisateurId As Long, Optional nbLignes As Integer = 2) As Long
    ' Même chose, signée par utilisateurId comme le fait l'application. Renvoie l'id.
    Function CreerOrdonnanceSignee(patientId As Long, utilisateurId As Long) As Long
End Module
```

Les signatures ci-dessus peuvent recevoir des paramètres optionnels supplémentaires, jamais perdre
ceux qui sont listés.

### Données de schéma

- `IntegrationTest/Schema/00-schema.sql` : export de production, fourni par l'utilisateur.
- Les trois scripts de `docs/migrations/` sont rejoués ensuite, dans l'ordre de leur nom ; ils sont
  idempotents (`IF COL_LENGTH`, `IF NOT EXISTS`).
- `IntegrationTest/Schema/20-reference.sql` : données de référence minimales dont les tests ont
  besoin avant la prise d'instantané (au départ, vide et commenté).
- `IntegrationTest/Schema/Preparer-Base.ps1` : crée la base, les logins `oasis_web` et
  `oasis_client` avec des mots de passe aléatoires (la migration `comptes-sql-separes` suppose les
  logins existants), rejoue schéma, migrations et référence avec `sqlcmd -b`, puis écrit les
  variables `OASIS_IT_*` dans `$env:GITHUB_ENV` quand il existe. Utilisable aussi sur un poste de
  développement.

## Intégration continue

Nouveau job `integration` dans `.github/workflows/build.yml`, sur `windows-latest` :
installation de SQL Server Express en mode mixte, `Preparer-Base.ps1`, compilation de
`IntegrationTest`, `vstest.console` sur `IntegrationTest.dll` avec `OASIS_IT_REQUIRED=1`,
publication du `.trx`. Tant que `00-schema.sql` n'est pas dans le dépôt, le job le détecte à sa
première étape et termine en vert sans rien lancer, avec une annotation qui le dit.
`UnitTest` et le job `build` ne changent pas.

## Tests de l'étape 1

- **Restrictions de colonnes** (`Securite/RestrictionsColonnes.test.vb`) : pour chaque ligne du
  tableau de `CLAUDE.md`, sous `Client` l'opération refusée échoue (erreur SQL 229/230) et les
  colonnes voisines restent lisibles ; sous `Web` les mêmes opérations passent. `DELETE` sur
  `oa_utilisateur` refusé au client ; les `GRANT DELETE` de `retrait-suppression-client` accordés.
- **`UserDao` et DAO du dossier `Utilisateur`** (`Dao/UserDao.test.vb`, `Dao/ProfilDao.test.vb`,
  `Dao/FonctionDao.test.vb`, `Dao/ActionDao.test.vb`) : chaque méthode publique, sous `Client` sauf
  celles qui ne tournent que côté serveur (`getUserByLoginPassword`, sous `Web`). Aucune méthode
  appelée par le client lourd ne doit lever d'erreur de permission.
- **Ordonnances** (`Dao/OrdonnanceDao.test.vb`, `Dao/OrdonnanceDetailDao.test.vb`) : création,
  lecture, mise à jour, suppression des lignes, persistance de la signature, charge utile et adresse ;
  relecture et vérification de la signature d'une ordonnance signée.
- **Contrôleurs** (`Web/LoginController.test.vb`, `Web/SignatureController.test.vb`,
  `Web/MotDePasseController.test.vb`, `Web/SignController.test.vb`), sous `Web` : bon et mauvais mot
  de passe, compte verrouillé, forme de `LoginResponse` (chaîne chiffrée qui se déchiffre vers le
  compte client, utilisateur sans `Password` ni `UtilisateurClePrivee`) ; signature déléguée et
  changement de clé ; changement de mot de passe ; `/Sign/Check` avec signature connue, inconnue,
  malformée.

Chaque test vérifie un comportement observable (valeurs relues en base ou réponse HTTP), pas
seulement l'absence d'exception.

## Répartition en lots parallèles

Chaque lot ne modifie que ses propres fichiers.

| Lot | Fichiers |
|---|---|
| A, harnais | `IntegrationTest/IntegrationTest.vbproj`, `app.config`, `packages.config`, `My Project/*`, `Infrastructure/BaseDeTest.vb`, `TestIntegration.vb`, `Assemblage.vb`, `Oasis_WF.sln`, `.github/ci.proj` |
| B, CI et schéma | `.github/workflows/build.yml`, `IntegrationTest/Schema/Preparer-Base.ps1`, `Schema/20-reference.sql`, `IntegrationTest/README.md` |
| C, sécurité et utilisateurs | `Securite/RestrictionsColonnes.test.vb`, `Dao/UserDao.test.vb`, `Dao/ProfilDao.test.vb`, `Dao/FonctionDao.test.vb`, `Dao/ActionDao.test.vb`, `Infrastructure/JeuxUtilisateur.vb` |
| D, ordonnances | `Dao/OrdonnanceDao.test.vb`, `Dao/OrdonnanceDetailDao.test.vb`, `Infrastructure/JeuxPatient.vb`, `Infrastructure/JeuxOrdonnance.vb` |
| E, contrôleurs | `Web/*.test.vb` listés plus haut ; si un attribut `InternalsVisibleTo("IntegrationTest")` est nécessaire, à côté de celui de `UnitTest` dans `Oasis_Web/Filters/AuthentificationApi.vb` |

Le lot A inscrit dans le `.vbproj` tous les fichiers de ce tableau. Un lot qui a besoin d'un fichier
de plus le signale au lieu de toucher le `.vbproj`.

## Risques connus

- Rien ne compile sur macOS : la première validation réelle est la CI. Les tests écrits avant
  l'arrivée du schéma devront probablement être ajustés (colonnes `NOT NULL` sans défaut, types).
- Les singletons d'`EnvironnementBase` gardent en mémoire la première lecture pour tout le processus :
  ce qu'ils lisent doit venir de `20-reference.sql`, donc de l'instantané, jamais d'un test.
- Fichiers VB : UTF-8 avec BOM et fins de ligne CRLF ; VB ne distingue pas la casse, un local ne peut
  pas porter le nom de sa fonction.
