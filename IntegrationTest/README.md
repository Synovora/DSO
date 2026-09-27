# IntegrationTest

Tests d'intégration d'Oasis contre une vraie base SQL Server jetable.

`UnitTest` couvre le code qui ne parle pas à la base. Ce projet couvre le reste : les DAO
d'`Oasis_Common`, les contrôleurs d'`Oasis_Web` et les droits des deux comptes SQL. Il exécute ce
code sous les logins `oasis_web` et `oasis_client` de production, avec les mêmes `DENY` de colonnes,
pour vérifier que le client lourd ne tombe sur aucun refus et que les colonnes secrètes lui restent
fermées.

Les deux projets sont séparés parce que `UnitTest` repose sur l'absence de chaîne de connexion
(`GetConnection()` y échoue tout de suite). La suite rapide ne dépend donc d'aucune base.

## Préparer une base locale

Il faut une instance SQL Server Express ou Developer (2016 SP1 ou plus récente, pour les
instantanés) en **authentification mixte**. LocalDB ne convient pas : il n'accepte que les comptes
Windows et ne peut donc pas se connecter en `oasis_client`.

Si l'instance est déjà installée en authentification Windows seule, activer le mode mixte dans
SSMS (propriétés du serveur, page Sécurité), redémarrer le service, puis activer `sa` avec un mot de
passe.

`sqlcmd` doit être installé (`winget install Microsoft.Sqlcmd`, ou les utilitaires en ligne de
commande livrés avec SQL Server ou Visual Studio).

Depuis la racine du dépôt, dans PowerShell :

```powershell
# Sécurité intégrée (le compte Windows courant est sysadmin de l'instance)
.\IntegrationTest\Schema\Preparer-Base.ps1

# Ou avec sa
.\IntegrationTest\Schema\Preparer-Base.ps1 -Serveur 'localhost\SQLEXPRESS' -MotDePasseAdmin (Read-Host 'sa')
```

Le script :

1. supprime puis recrée la base `oasis_it` (paramètre `-Base`), avec ses éventuels instantanés ;
2. crée ou réinitialise les logins `oasis_web` et `oasis_client` avec des mots de passe aléatoires ;
3. rejoue `Schema/00-schema.sql`, puis chaque script de `docs/migrations/` par ordre de nom, puis
   `Schema/20-reference.sql`, en s'arrêtant à la première erreur ;
4. pose les variables d'environnement lues par le harnais dans la session PowerShell courante.

Les mots de passe ne sont jamais affichés et changent à chaque exécution. Pour garder les variables
au-delà de la session, le script affiche les commandes `SetEnvironmentVariable` à copier.

| Variable | Défaut | Rôle |
|---|---|---|
| `OASIS_IT_SERVER` | `localhost\SQLEXPRESS` | instance SQL Server |
| `OASIS_IT_DATABASE` | `oasis_it` | base de test |
| `OASIS_IT_ADMIN_PASSWORD` | vide : sécurité intégrée | mot de passe de `sa` |
| `OASIS_IT_WEB_PASSWORD` | aucun | login `oasis_web` |
| `OASIS_IT_CLIENT_PASSWORD` | aucun | login `oasis_client` |
| `OASIS_IT_REQUIRED` | vide | `1` : base injoignable = échec au lieu de `Inconclusive` |

## Lancer les tests

Compiler `IntegrationTest` dans Visual Studio, puis lancer l'Explorateur de tests. Visual Studio doit
avoir été démarré depuis la console où `Preparer-Base.ps1` a tourné, ou après avoir enregistré les
variables pour l'utilisateur ; sinon il ne les voit pas.

En ligne de commande :

```powershell
msbuild IntegrationTest\IntegrationTest.vbproj /p:Configuration=Debug
vstest.console.exe IntegrationTest\bin\Debug\IntegrationTest.dll
```

Sans base joignable, tous les tests finissent `Inconclusive`. En CI, `OASIS_IT_REQUIRED=1` les fait
échouer.

Le job `integration` de `.github/workflows/build.yml` fait la même chose sur un runner Windows :
installation de SQL Server Express, `Preparer-Base.ps1`, compilation, tests. Tant que
`Schema/00-schema.sql` n'est pas dans le dépôt, il le signale et s'arrête en vert. Une modification
limitée à `docs/migrations/` ne le déclenche pas (le workflow ignore `docs/**`) : le lancer à la main
depuis l'onglet Actions.

## Isolation

Au démarrage, le harnais (`Infrastructure/Assemblage.vb`) vérifie que la base répond puis prend
l'instantané `oasis_it_instantane`. Avant chaque test, `TestIntegration` restaure la base depuis cet
instantané. Chaque test part donc de l'état produit par le schéma, les migrations et
`20-reference.sql`, quel que soit l'ordre d'exécution.

La restauration exige qu'aucune autre session ne soit ouverte sur la base : toutes les chaînes de
connexion du harnais portent `Pooling=False`. Ne pas garder SSMS connecté à `oasis_it` pendant une
exécution.

Toute classe de test hérite de `TestIntegration`. Elle tourne par défaut sous `oasis_client`, le
compte du client lourd ; un test qui exerce du code serveur appelle `UtiliserCompte(Compte.Web)` en
tête.

## Jeux de données

Les fichiers `Infrastructure/Jeux*.vb` créent les données dont les tests ont besoin. Règle : passer
par les méthodes de création des DAO quand elles existent, parce que leur `INSERT` est celui que la
production exécute et accepte. `Executer` (SQL brut sous le compte administrateur) ne sert qu'à ce
qu'aucun DAO n'écrit.

## Données de référence et singletons

Les singletons d'`Oasis_Common/Module/EnvironnementBase.vb` (genres, sites, unités sanitaires,
spécialités, ALD...) lisent leur table au premier usage et gardent le résultat pour tout le
processus de test. Une ligne de référence créée par un test serait vue ou non selon l'ordre des
tests, puis effacée par la restauration alors que le cache la garde encore.

Ce que ces singletons lisent doit donc venir de `Schema/20-reference.sql`, qui fait partie de
l'instantané, jamais d'un test.

## Exporter le schéma depuis SSMS

`Schema/00-schema.sql` est un export du schéma de production. **Le dépôt est public** : le fichier
ne doit contenir ni données, ni logins, ni utilisateurs, ni mots de passe, ni chemins ou noms de
serveurs.

Dans SSMS, clic droit sur la base `oasis`, Tâches, Générer des scripts :

1. **Choisir les objets** : « Sélectionner des objets de base de données spécifiques », puis cocher
   les schémas, tables, vues, procédures stockées, fonctions et types définis par l'utilisateur.
   Ne pas cocher Utilisateurs ni Rôles de base de données. Cette option évite aussi le
   `CREATE DATABASE` et ses chemins de fichiers.
2. **Options avancées** :
   - Types de données à inclure dans le script : **Schéma uniquement** ;
   - Générer le script des connexions (logins) : **False** ;
   - Générer le script des autorisations au niveau objet : **False** (les migrations posent les
     droits) ;
   - Générer le script du propriétaire : **False** ;
   - Générer le script pour la version du serveur : SQL Server 2022 ;
   - Index, clés, contraintes de vérification, valeurs par défaut, déclencheurs : **True**.
3. Enregistrer dans un seul fichier, `IntegrationTest/Schema/00-schema.sql`. L'encodage Unicode
   proposé par défaut convient.

Les `USE [oasis]`, `ALTER DATABASE [oasis]` et noms en trois parties `[oasis].[...]` de l'export sont
redirigés vers la base de test par `Preparer-Base.ps1`, dans une copie temporaire. Le script refuse
un fichier qui contient un `CREATE LOGIN`.

Avant de committer, relire le fichier : chercher `INSERT`, `LOGIN`, `CREATE USER`, `PASSWORD`,
`FILENAME`, le nom du serveur de production et tout nom de personne dans les commentaires.
