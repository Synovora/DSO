Imports System.Configuration
Imports System.IO
Imports System.Text.RegularExpressions

''' <summary>
''' Résolution sûre des chemins de documents servis ou reçus par l'API.
''' Empêche la traversée de répertoire (..\, chemins absolus, UNC) : un nom de
''' fichier fourni par un client ne peut désigner que la zone de dépôt et
''' seulement les dossiers et motifs attendus par l'application.
''' </summary>
Public Module CheminsDocuments

    ' Dossiers et motifs légitimes produits par getFilenameServer / GetFilenameServer :
    '   SousEpisode\Episode_1_SousEpisode_2_SousEpisodeSousType_3.DOCX
    '   SousEpisodeReponse\Episode_1_SousEpisode_2_SousEpisodeReponse_3.pdf
    '   Templates\SousEpisodeType_1_SousType_2.DOCX
    ' La pièce jointe d'une réponse conserve l'extension d'origine, d'où la liste
    ' d'extensions ci-dessous plutôt qu'un couple DOCX/PDF figé.
    Private ReadOnly NomValide As New Regex(
        "^(SousEpisode|SousEpisodeReponse|Templates)\\[A-Za-z0-9_\-]+\." &
        "(DOCX?|PDF|ODT|RTF|TXT|CSV|XLSX?|PPTX?|JPE?G|PNG|GIF|BMP|TIFF?|HTML?|XML|ZIP)$",
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

    ''' <summary>
    ''' Renvoie le chemin absolu du document correspondant à nomRelatif, ou lève
    ''' ArgumentException si le nom ne respecte pas le motif attendu ou tente de
    ''' sortir de la zone de dépôt.
    ''' </summary>
    Public Function ResoudreCheminDocument(nomRelatif As String) As String
        Dim racineConfig = ConfigurationManager.AppSettings("FileUploadLocation")
        If String.IsNullOrWhiteSpace(racineConfig) Then
            Throw New InvalidOperationException("FileUploadLocation n'est pas configuré.")
        End If
        Dim racine = Path.GetFullPath(racineConfig).TrimEnd(Path.DirectorySeparatorChar)

        Dim nettoye = NormaliserNomDocument(nomRelatif)
        If Not NomValide.IsMatch(nettoye) Then
            Throw New ArgumentException("Nom de fichier invalide.")
        End If

        Dim complet = Path.GetFullPath(Path.Combine(racine, nettoye))
        If Not complet.StartsWith(racine & Path.DirectorySeparatorChar, StringComparison.OrdinalIgnoreCase) Then
            Throw New ArgumentException("Nom de fichier invalide.")
        End If
        Return complet
    End Function

    ''' <summary>Normalise un nom reçu d'un client (séparateurs, espaces).</summary>
    Public Function NormaliserNomDocument(nomRelatif As String) As String
        Return If(nomRelatif, "").Trim().Replace("/"c, "\"c).TrimStart("\"c)
    End Function

    ''' <summary>
    ''' True si nomRelatif est un nom de document acceptable (sans résoudre le
    ''' chemin sur le disque). Utile pour valider avant écriture.
    ''' </summary>
    Public Function EstNomDocumentValide(nomRelatif As String) As Boolean
        Return NomValide.IsMatch(NormaliserNomDocument(nomRelatif))
    End Function

End Module
