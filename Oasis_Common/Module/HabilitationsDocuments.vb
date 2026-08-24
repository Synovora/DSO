Imports System.Text.RegularExpressions

''' <summary>
''' Contrôle d'accès aux documents servis et reçus par l'API.
'''
''' Les routes de fichiers vérifiaient un mot de passe puis agissaient. Prouver
''' qu'un compte existe ne dit rien de son droit sur le document demandé : les
''' noms sont prévisibles (Episode_12_SousEpisode_34_...), donc n'importe quel
''' compte parcourait l'espace des identifiants et lisait, écrasait ou renommait
''' l'ensemble du fonds documentaire.
'''
''' Ce module répond à deux questions distinctes :
'''   - le nom demandé correspond-il à un document réellement enregistré ?
'''   - le profil de l'appelant a-t-il affaire aux documents ?
'''
''' Il ne répond pas encore à la troisième, qui est « cet utilisateur a-t-il
''' affaire à CE patient ». Cette règle n'existe nulle part dans l'application :
''' voir la DÉCISION 3 de docs/specs/2026-08-24-modele-habilitations.md. Le point
''' d'insertion est PeutAccederAuPatient, et c'est le seul à modifier une fois la
''' règle arrêtée. En attendant, tout accès est journalisé avec le patient
''' concerné, ce qui rend l'abus constatable à défaut d'être empêché.
''' </summary>
Public Module HabilitationsDocuments

    ' Episode_{episode}_SousEpisode_{sousEpisode}_SousEpisodeSousType_{type}.DOCX
    Private ReadOnly MotifSousEpisode As New Regex(
        "^SousEpisode\\Episode_(?<episode>\d+)_SousEpisode_(?<sousEpisode>\d+)_SousEpisodeSousType_\d+\.[A-Za-z0-9]+$",
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

    ' Episode_{episode}_SousEpisode_{sousEpisode}_SousEpisodeReponse_{reponse}.{ext}
    Private ReadOnly MotifReponse As New Regex(
        "^SousEpisodeReponse\\Episode_(?<episode>\d+)_SousEpisode_(?<sousEpisode>\d+)_SousEpisodeReponse_\d+\.[A-Za-z0-9]+$",
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

    ' Templates\SousEpisodeType_{type}_SousType_{sousType}.DOCX
    Private ReadOnly MotifModele As New Regex(
        "^Templates\\SousEpisodeType_\d+_SousType_\d+\.[A-Za-z0-9]+$",
        RegexOptions.IgnoreCase Or RegexOptions.Compiled)

    ''' <summary>Résultat de la résolution d'un nom de document.</summary>
    Public Class DocumentDemande
        ''' <summary>Nom normalisé, tel qu'il sera passé à ResoudreCheminDocument.</summary>
        Public Property Nom As String
        ''' <summary>Modèle de document : contenu de référence, sans patient rattaché.</summary>
        Public Property EstModele As Boolean
        ''' <summary>Patient concerné. 0 pour un modèle.</summary>
        Public Property PatientId As Long
        ''' <summary>Épisode concerné. 0 pour un modèle.</summary>
        Public Property EpisodeId As Long
    End Class

    ''' <summary>
    ''' Résout un nom fourni par un client en document réel, ou lève
    ''' UnauthorizedAccessException si le nom ne désigne rien d'enregistré.
    '''
    ''' Un nom bien formé ne suffit pas : l'épisode doit exister et le
    ''' sous-épisode doit lui appartenir. Sans cette vérification, un appelant
    ''' compose n'importe quelle combinaison d'identifiants et l'API la sert dès
    ''' lors qu'un fichier porte ce nom sur le disque.
    ''' </summary>
    Public Function ResoudreDocument(nomRelatif As String) As DocumentDemande
        Dim nom = NormaliserNomDocument(nomRelatif)

        If MotifModele.IsMatch(nom) Then
            Return New DocumentDemande With {.Nom = nom, .EstModele = True}
        End If

        Dim correspondance = MotifSousEpisode.Match(nom)
        If Not correspondance.Success Then correspondance = MotifReponse.Match(nom)
        If Not correspondance.Success Then
            Throw New UnauthorizedAccessException("Document inconnu.")
        End If

        Dim episodeId = CLng(correspondance.Groups("episode").Value)
        Dim sousEpisodeId = CLng(correspondance.Groups("sousEpisode").Value)

        Dim sousEpisodeDao As New SousEpisodeDao
        If Not sousEpisodeDao.AppartientAEpisode(sousEpisodeId, episodeId) Then
            Throw New UnauthorizedAccessException("Document inconnu.")
        End If

        Dim episodeDao As New EpisodeDao
        Dim episode = episodeDao.GetEpisodeById(CInt(episodeId))
        If episode Is Nothing OrElse episode.PatientId = 0 Then
            Throw New UnauthorizedAccessException("Document inconnu.")
        End If

        Return New DocumentDemande With {
            .Nom = nom,
            .EstModele = False,
            .EpisodeId = episodeId,
            .PatientId = episode.PatientId
        }
    End Function

    ''' <summary>
    ''' Vrai si le profil de l'utilisateur a affaire aux documents de dossier.
    ''' Les profils de gestion n'ont pas à lire de compte rendu clinique : voir la
    ''' matrice de docs/specs/2026-08-24-modele-habilitations.md.
    ''' </summary>
    Public Function PeutAccederAuxDocuments(utilisateur As Utilisateur) As Boolean
        If utilisateur Is Nothing Then Return False
        If utilisateur.UtilisateurAdmin Then Return True

        Select Case utilisateur.TypeProfil
            Case ProfilDao.EnumProfilType.MEDICAL.ToString,
                 ProfilDao.EnumProfilType.PARAMEDICAL.ToString,
                 ProfilDao.EnumProfilType.ACCUEIL.ToString
                Return True
            Case Else
                Return False
        End Select
    End Function

    ''' <summary>
    ''' Vrai si l'utilisateur peut accéder au dossier de ce patient.
    '''
    ''' NON IMPLÉMENTÉ, ET C'EST DÉLIBÉRÉ. Aucune règle de périmètre patient
    ''' n'existe dans l'application : la liste des patients ne filtre sur rien, et
    ''' le site, l'unité sanitaire et le siège portés par l'utilisateur ne servent
    ''' qu'à préremplir des écrans. Inventer une règle ici la figerait sans que
    ''' personne ne l'ait arrêtée, et une règle trop stricte posée au hasard
    ''' bloquerait des soins.
    '''
    ''' La décision est en attente (DÉCISION 3 du modèle d'habilitations). Quand
    ''' elle sera prise, c'est cette fonction qu'il faut écrire, et elle seule :
    ''' tous les appelants passent par ici.
    ''' </summary>
    Public Function PeutAccederAuPatient(utilisateur As Utilisateur, patientId As Long) As Boolean
        Return utilisateur IsNot Nothing AndAlso patientId > 0
    End Function

End Module
