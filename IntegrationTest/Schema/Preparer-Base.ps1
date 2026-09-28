<#
.SYNOPSIS
    Prépare la base SQL Server jetable des tests d'intégration.

.DESCRIPTION
    Supprime puis recrée la base, crée ou réinitialise les logins oasis_web et
    oasis_client avec des mots de passe tirés au hasard, puis rejoue dans l'ordre :

        IntegrationTest/Schema/00-schema.sql   export SSMS du schéma de production
        docs/migrations/*.sql                  par ordre de nom
        IntegrationTest/Schema/*-reference*.sql    par ordre de nom, 20-reference.sql
                                               d'abord, puis un fichier par domaine

    Chaque script passe par sqlcmd -b : la première erreur arrête tout.

    Les variables OASIS_IT_* lues par le harnais sont ensuite écrites dans
    $env:GITHUB_ENV en CI, ou posées dans la session PowerShell courante sur un
    poste de développement. Aucun mot de passe n'est affiché.

    Compatible Windows PowerShell 5.1 et PowerShell 7. Le fichier est en UTF-8
    avec BOM, sans quoi Windows PowerShell 5.1 lit les accents de travers.

.PARAMETER Serveur
    Instance SQL Server. Défaut : localhost\SQLEXPRESS.

.PARAMETER Base
    Nom de la base de test. Elle est supprimée si elle existe. Défaut : oasis_it.

.PARAMETER MotDePasseAdmin
    Mot de passe du compte sa. Vide : sécurité intégrée Windows.

.EXAMPLE
    .\IntegrationTest\Schema\Preparer-Base.ps1

.EXAMPLE
    .\IntegrationTest\Schema\Preparer-Base.ps1 -Serveur 'localhost\SQLEXPRESS' -MotDePasseAdmin $motDePasseSa
#>
[CmdletBinding()]
param(
    [string]$Serveur = 'localhost\SQLEXPRESS',
    [string]$Base = 'oasis_it',
    [string]$MotDePasseAdmin = ''
)

Set-StrictMode -Version Latest
$ErrorActionPreference = 'Stop'

# Le nom de base entre tel quel dans du SQL : on n'accepte qu'un identifiant simple.
if ($Base -notmatch '^[A-Za-z_][A-Za-z0-9_]{0,99}$') {
    throw "Nom de base refusé : '$Base'. Lettres, chiffres et _ uniquement."
}
if ($Base -in @('master', 'model', 'msdb', 'tempdb')) {
    throw "La base '$Base' est une base système, elle ne peut pas servir de base de test."
}

$enCI = [bool]$env:GITHUB_ACTIONS

$racine = (Resolve-Path (Join-Path (Join-Path $PSScriptRoot '..') '..')).Path
$fichierSchema = Join-Path $PSScriptRoot '00-schema.sql'
$fichierReference = Join-Path $PSScriptRoot '20-reference.sql'
$dossierMigrations = Join-Path (Join-Path $racine 'docs') 'migrations'

# Tout ce qui peut manquer est vérifié avant de toucher à la base.
if (-not (Test-Path -LiteralPath $fichierSchema)) {
    throw ("$fichierSchema est absent. Exporter le schéma de production depuis SSMS " +
        "(voir IntegrationTest/README.md) et le déposer sous ce nom.")
}
if (-not (Test-Path -LiteralPath $fichierReference)) {
    throw "$fichierReference est absent."
}
$migrations = @(Get-ChildItem -LiteralPath $dossierMigrations -Filter '*.sql' | Sort-Object Name)
# Un fichier de référence par domaine, pour que deux jeux de tests n'aient pas
# à se partager le même fichier.
$references = @(Get-ChildItem -LiteralPath $PSScriptRoot -Filter '*-reference*.sql' | Sort-Object Name)

# ---------------------------------------------------------------------------
# sqlcmd
# ---------------------------------------------------------------------------

function Find-Sqlcmd {
    $commande = Get-Command 'sqlcmd' -CommandType Application -ErrorAction SilentlyContinue |
        Select-Object -First 1
    if ($commande) { return $commande.Path }

    # Les utilitaires installés par Visual Studio ou par SQL Server ne sont pas
    # toujours sur le PATH.
    if (-not $env:ProgramFiles) { throw "sqlcmd introuvable sur le PATH." }
    $motifs = @(
        (Join-Path $env:ProgramFiles 'Microsoft SQL Server\Client SDK\ODBC\*\Tools\Binn\SQLCMD.EXE'),
        (Join-Path $env:ProgramFiles 'Microsoft SQL Server\*\Tools\Binn\SQLCMD.EXE'),
        (Join-Path $env:ProgramFiles 'SqlCmd\sqlcmd.exe')
    )
    $trouve = Get-ChildItem -Path $motifs -ErrorAction SilentlyContinue |
        Sort-Object FullName -Descending | Select-Object -First 1
    if ($trouve) { return $trouve.FullName }

    throw ("sqlcmd introuvable. L'installer (winget install Microsoft.Sqlcmd, ou les " +
        "utilitaires en ligne de commande de SQL Server) puis relancer.")
}

$sqlcmd = Find-Sqlcmd

# -b  arrêt à la première erreur, code de sortie non nul
# -I  QUOTED_IDENTIFIER ON, comme SSMS
# -x  pas de substitution $(variable) : un export ou une migration peut contenir
#     cette suite de caractères dans une chaîne
# -C  certificat du serveur accepté tel quel : sqlcmd 18 chiffre par défaut et
#     une instance locale n'a qu'un certificat autosigné
$argumentsCommuns = @('-S', $Serveur, '-b', '-I', '-x', '-C', '-l', '30')
if ($MotDePasseAdmin) {
    # Le mot de passe passe par SQLCMDPASSWORD plutôt que par -P, qui le
    # laisserait visible dans la liste des processus.
    $argumentsCommuns += @('-U', 'sa')
} else {
    $argumentsCommuns += '-E'
}

$dossierTemporaire = Join-Path ([System.IO.Path]::GetTempPath()) ("oasis-it-" + [guid]::NewGuid().ToString('N'))
New-Item -ItemType Directory -Path $dossierTemporaire | Out-Null

function Invoke-FichierSql([string]$Chemin, [string]$BaseCible, [string]$Libelle) {
    Write-Host "==> $Libelle"
    $arguments = $argumentsCommuns + @('-d', $BaseCible, '-i', $Chemin)
    & $sqlcmd @arguments | Out-Host
    if ($LASTEXITCODE -ne 0) {
        throw "sqlcmd a échoué (code $LASTEXITCODE) sur : $Libelle"
    }
}

# Le texte passe par un fichier temporaire, jamais par -Q : les mots de passe des
# logins n'apparaissent ainsi ni dans la ligne de commande ni dans le journal.
function Invoke-RequeteSql([string]$Requete, [string]$BaseCible, [string]$Libelle) {
    $chemin = Join-Path $dossierTemporaire ([guid]::NewGuid().ToString('N') + '.sql')
    [System.IO.File]::WriteAllText($chemin, $Requete, [System.Text.Encoding]::Unicode)
    try {
        Invoke-FichierSql $chemin $BaseCible $Libelle
    } finally {
        Remove-Item -LiteralPath $chemin -Force -ErrorAction SilentlyContinue
    }
}

# ---------------------------------------------------------------------------
# Copie des scripts vers la base cible
# ---------------------------------------------------------------------------

function Read-TexteSql([string]$Chemin) {
    $octets = [System.IO.File]::ReadAllBytes($Chemin)
    if ($octets.Length -ge 2 -and $octets[0] -eq 0xFF -and $octets[1] -eq 0xFE) {
        return [System.Text.Encoding]::Unicode.GetString($octets, 2, $octets.Length - 2)
    }
    if ($octets.Length -ge 2 -and $octets[0] -eq 0xFE -and $octets[1] -eq 0xFF) {
        return [System.Text.Encoding]::BigEndianUnicode.GetString($octets, 2, $octets.Length - 2)
    }
    if ($octets.Length -ge 3 -and $octets[0] -eq 0xEF -and $octets[1] -eq 0xBB -and $octets[2] -eq 0xBF) {
        return [System.Text.Encoding]::UTF8.GetString($octets, 3, $octets.Length - 3)
    }
    # Sans BOM : UTF-8 s'il se décode proprement, sinon la page de code ANSI
    # d'un SSMS français.
    try {
        return (New-Object System.Text.UTF8Encoding($false, $true)).GetString($octets)
    } catch {
        return [System.Text.Encoding]::GetEncoding(1252).GetString($octets)
    }
}

# Un export SSMS vise la base d'origine : USE [oasis], ALTER DATABASE [oasis],
# parfois des noms en trois parties [oasis].[dbo].[...]. Tout cela est redirigé
# vers la base de test, et un éventuel lot CREATE DATABASE, qui porterait les
# chemins de fichiers du serveur de production, est retiré. La copie est écrite
# en UTF-16 avec BOM, que sqlcmd reconnaît sans option de page de code.
function New-CopiePourBase([string]$Source) {
    $texte = Read-TexteSql $Source

    if ($texte -match '(?im)^\s*CREATE\s+LOGIN\b') {
        throw ("$Source contient un CREATE LOGIN. Le dépôt est public : réexporter sans " +
            "les logins (voir IntegrationTest/README.md).")
    }

    $systeme = @('master', 'model', 'msdb', 'tempdb')
    $basesOrigine = @([regex]::Matches($texte, '(?im)^\s*USE\s+\[([^\]]+)\]') |
        ForEach-Object { $_.Groups[1].Value } |
        Where-Object { $systeme -notcontains $_ -and $_ -ne $Base } |
        Sort-Object -Unique)
    foreach ($nom in $basesOrigine) {
        $e = [regex]::Escape($nom)
        $texte = [regex]::Replace($texte, "(?im)^(\s*USE\s+)\[$e\]", "`${1}[$Base]")
        $texte = [regex]::Replace($texte, "(?i)\bALTER\s+DATABASE\s+\[$e\]", "ALTER DATABASE [$Base]")
        $texte = [regex]::Replace($texte, "(?i)\[$e\]\.(?=\[)", "[$Base].")
    }

    $lots = [regex]::Split($texte, '(?im)^[ \t]*GO[ \t]*(?:\r?\n|$)')
    $conserves = New-Object System.Collections.Generic.List[string]
    foreach ($lot in $lots) {
        $sansCommentaires = [regex]::Replace($lot, '(?m)^\s*--.*$', '').Trim()
        if ($sansCommentaires -match '^(?i)CREATE\s+DATABASE\b') {
            Write-Host "    lot CREATE DATABASE ignoré dans $(Split-Path -Leaf $Source)"
            continue
        }
        $conserves.Add($lot)
    }
    $texte = ($conserves -join "`r`nGO`r`n")

    $copie = Join-Path $dossierTemporaire (Split-Path -Leaf $Source)
    [System.IO.File]::WriteAllText($copie, $texte, [System.Text.Encoding]::Unicode)
    return $copie
}

# ---------------------------------------------------------------------------
# Mots de passe
# ---------------------------------------------------------------------------

# 24 caractères tirés par RNGCryptoServiceProvider, avec au moins une majuscule,
# une minuscule, un chiffre et un symbole pour passer CHECK_POLICY. L'alphabet
# exclut les guillemets, ; = { } et $ : le mot de passe entre dans du SQL, dans
# une chaîne de connexion et dans $GITHUB_ENV sans échappement.
function New-MotDePasse([int]$Longueur = 24) {
    $classes = @('ABCDEFGHJKLMNPQRSTUVWXYZ', 'abcdefghijkmnopqrstuvwxyz', '23456789', '-_.!*+#@')
    $alphabet = -join $classes
    # Tirage par rejet : un octet au-delà du dernier multiple complet de la
    # taille de l'alphabet est jeté, ce qui évite de favoriser les premiers caractères.
    $limite = 256 - (256 % $alphabet.Length)
    $rng = New-Object System.Security.Cryptography.RNGCryptoServiceProvider
    try {
        $octet = New-Object byte[] 1
        do {
            $tampon = New-Object System.Text.StringBuilder
            while ($tampon.Length -lt $Longueur) {
                $rng.GetBytes($octet)
                if ($octet[0] -lt $limite) {
                    [void]$tampon.Append($alphabet[$octet[0] % $alphabet.Length])
                }
            }
            $motDePasse = $tampon.ToString()
            $couvertes = @($classes | Where-Object { $motDePasse.IndexOfAny($_.ToCharArray()) -ge 0 }).Count
        } until ($couvertes -eq $classes.Count)
        return $motDePasse
    } finally {
        $rng.Dispose()
    }
}

$motDePasseWeb = New-MotDePasse
$motDePasseClient = New-MotDePasse

# Masqués avant tout usage : le runner remplace ensuite chaque occurrence par ***.
if ($enCI) {
    foreach ($secret in @($MotDePasseAdmin, $motDePasseWeb, $motDePasseClient)) {
        if ($secret) { Write-Host "::add-mask::$secret" }
    }
}

# ---------------------------------------------------------------------------
# Préparation
# ---------------------------------------------------------------------------

$ancienSqlcmdPassword = $env:SQLCMDPASSWORD
if ($MotDePasseAdmin) { $env:SQLCMDPASSWORD = $MotDePasseAdmin }

try {
    Invoke-RequeteSql 'SELECT @@VERSION;' 'master' "Connexion à $Serveur"

    # Un instantané empêche de supprimer sa base source : ceux qu'une exécution
    # précédente des tests aurait laissés partent d'abord.
    Invoke-RequeteSql @"
DECLARE @instantanes nvarchar(max) = N'';
SELECT @instantanes = @instantanes + N'DROP DATABASE ' + QUOTENAME(name) + N'; '
  FROM sys.databases
 WHERE source_database_id = DB_ID(N'$Base');
IF @instantanes <> N'' EXEC (@instantanes);

IF DB_ID(N'$Base') IS NOT NULL
BEGIN
    ALTER DATABASE [$Base] SET SINGLE_USER WITH ROLLBACK IMMEDIATE;
    DROP DATABASE [$Base];
END
GO
CREATE DATABASE [$Base];
GO
"@ 'master' "Base $Base supprimée et recréée"

    # La migration comptes-sql-separes crée les utilisateurs de base à partir de
    # logins qu'elle suppose existants.
    $requeteLogins = New-Object System.Text.StringBuilder
    foreach ($login in @(
            @{ Nom = 'oasis_web'; MotDePasse = $motDePasseWeb },
            @{ Nom = 'oasis_client'; MotDePasse = $motDePasseClient })) {
        $nom = $login.Nom
        $mdp = $login.MotDePasse
        [void]$requeteLogins.AppendLine(@"
IF SUSER_ID(N'$nom') IS NULL
    CREATE LOGIN [$nom] WITH PASSWORD = N'$mdp', DEFAULT_DATABASE = [$Base], CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;
ELSE
    ALTER LOGIN [$nom] WITH PASSWORD = N'$mdp', DEFAULT_DATABASE = [$Base], CHECK_POLICY = ON, CHECK_EXPIRATION = OFF;
ALTER LOGIN [$nom] ENABLE;
GO
"@)
    }
    Invoke-RequeteSql $requeteLogins.ToString() 'master' 'Logins oasis_web et oasis_client'

    Invoke-FichierSql (New-CopiePourBase $fichierSchema) $Base 'Schéma (00-schema.sql)'
    foreach ($migration in $migrations) {
        Invoke-FichierSql (New-CopiePourBase $migration.FullName) $Base "Migration $($migration.Name)"
    }
    foreach ($reference in $references) {
        Invoke-FichierSql (New-CopiePourBase $reference.FullName) $Base "Données de référence ($($reference.Name))"
    }
} finally {
    $env:SQLCMDPASSWORD = $ancienSqlcmdPassword
    Remove-Item -LiteralPath $dossierTemporaire -Recurse -Force -ErrorAction SilentlyContinue
}

# ---------------------------------------------------------------------------
# Variables pour le harnais
# ---------------------------------------------------------------------------

$variables = [ordered]@{
    OASIS_IT_SERVER          = $Serveur
    OASIS_IT_DATABASE        = $Base
    OASIS_IT_ADMIN_PASSWORD  = $MotDePasseAdmin
    OASIS_IT_WEB_PASSWORD    = $motDePasseWeb
    OASIS_IT_CLIENT_PASSWORD = $motDePasseClient
}

# Toujours posées dans la session : un vstest ou un Visual Studio lancé depuis
# cette console en hérite.
foreach ($nom in $variables.Keys) {
    Set-Item -Path "env:$nom" -Value $variables[$nom]
}

if ($env:GITHUB_ENV) {
    # AppendAllText écrit de l'UTF-8 sans BOM sous 5.1 comme sous 7.
    $lignes = ($variables.Keys | ForEach-Object { "$_=$($variables[$_])" }) -join "`n"
    [System.IO.File]::AppendAllText($env:GITHUB_ENV, $lignes + "`n")
    Write-Host "Base $Base prête. Variables OASIS_IT_* transmises aux étapes suivantes."
} else {
    Write-Host ""
    Write-Host "Base $Base prête sur $Serveur."
    Write-Host "Les variables OASIS_IT_* sont posées dans cette session PowerShell seulement."
    Write-Host "Lancer les tests (ou Visual Studio) depuis cette même console, ou les conserver"
    Write-Host "pour l'utilisateur Windows courant avec :"
    Write-Host ""
    foreach ($nom in $variables.Keys) {
        Write-Host "    [Environment]::SetEnvironmentVariable('$nom', `$env:$nom, 'User')"
    }
    Write-Host ""
    Write-Host "Les mots de passe changent à chaque exécution de ce script."
}
