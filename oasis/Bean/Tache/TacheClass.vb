Imports Oasis_Common

Public Class Tache
    Private _id As Long
    Private _parentId As Long
    Private _emetteurUserId As Long
    Private _emetteurFonctionId As Long
    Private _uniteSanitaireId As Long
    Private _siteId As Long
    Private _patientId As Long
    Private _parcoursId As Long
    Private _episodeId As Long
    Private _sousEpisodeId As Long
    Private _traiteUserId As Long
    Private _traiteFonctionId As Long
    Private _destinataireFonctionId As Long
    Private _priorite As Integer
    Private _ordreAffichage As Integer
    Private _categorie As String
    Private _type As String
    Private _nature As String
    Private _duree As Integer
    Private _emetteurCommentaire As String
    Private _horodatageCreation As DateTime
    Private _horodatageAttribution As DateTime
    Private _horodatageCloture As DateTime
    Private _etat As String
    Private _Cloture As Boolean
    Private _typedemandeRendezVous As String
    Private _dateTraitementDemandeRendezVous As DateTime
    Private _dateRendezVous As Date

    Public Property Id As Long
        Get
            Return _id
        End Get
        Set(value As Long)
            _id = value
        End Set
    End Property

    Public Property ParentId As Long
        Get
            Return _parentId
        End Get
        Set(value As Long)
            _parentId = value
        End Set
    End Property

    Public Property EmetteurUserId As Long
        Get
            Return _emetteurUserId
        End Get
        Set(value As Long)
            _emetteurUserId = value
        End Set
    End Property

    Public Property EmetteurFonctionId As Long
        Get
            Return _emetteurFonctionId
        End Get
        Set(value As Long)
            _emetteurFonctionId = value
        End Set
    End Property

    Public Property UniteSanitaireId As Long
        Get
            Return _uniteSanitaireId
        End Get
        Set(value As Long)
            _uniteSanitaireId = value
        End Set
    End Property

    Public Property SiteId As Long
        Get
            Return _siteId
        End Get
        Set(value As Long)
            _siteId = value
        End Set
    End Property

    Public Property PatientId As Long
        Get
            Return _patientId
        End Get
        Set(value As Long)
            _patientId = value
        End Set
    End Property

    Public Property ParcoursId As Long
        Get
            Return _parcoursId
        End Get
        Set(value As Long)
            _parcoursId = value
        End Set
    End Property

    Public Property EpisodeId As Long
        Get
            Return _episodeId
        End Get
        Set(value As Long)
            _episodeId = value
        End Set
    End Property

    Public Property SousEpisodeId As Long
        Get
            Return _sousEpisodeId
        End Get
        Set(value As Long)
            _sousEpisodeId = value
        End Set
    End Property

    Public Property TraiteUserId As Long
        Get
            Return _traiteUserId
        End Get
        Set(value As Long)
            _traiteUserId = value
        End Set
    End Property

    Public Property TraiteFonctionId As Long
        Get
            Return _traiteFonctionId
        End Get
        Set(value As Long)
            _traiteFonctionId = value
        End Set
    End Property

    Public Property DestinataireFonctionId As Long
        Get
            Return _destinataireFonctionId
        End Get
        Set(value As Long)
            _destinataireFonctionId = value
        End Set
    End Property

    Public Property Priorite As Integer
        Get
            Return _priorite
        End Get
        Set(value As Integer)
            _priorite = value
        End Set
    End Property

    Public Property OrdreAffichage As Integer
        Get
            Return _ordreAffichage
        End Get
        Set(value As Integer)
            _ordreAffichage = value
        End Set
    End Property

    Public Property Categorie As String
        Get
            Return _categorie
        End Get
        Set(value As String)
            _categorie = value
        End Set
    End Property

    Public Property Type As String
        Get
            Return _type
        End Get
        Set(value As String)
            _type = value
        End Set
    End Property

    Public Property Nature As String
        Get
            Return _nature
        End Get
        Set(value As String)
            _nature = value
        End Set
    End Property

    Public Property Duree As Integer
        Get
            Return _duree
        End Get
        Set(value As Integer)
            _duree = value
        End Set
    End Property

    Public Property EmetteurCommentaire As String
        Get
            Return _emetteurCommentaire
        End Get
        Set(value As String)
            _emetteurCommentaire = value
        End Set
    End Property

    Public Property HorodatageCreation As Date
        Get
            Return _horodatageCreation
        End Get
        Set(value As Date)
            _horodatageCreation = value
        End Set
    End Property

    Public Property HorodatageAttribution As Date
        Get
            Return _horodatageAttribution
        End Get
        Set(value As Date)
            _horodatageAttribution = value
        End Set
    End Property

    Public Property HorodatageCloture As Date
        Get
            Return _horodatageCloture
        End Get
        Set(value As Date)
            _horodatageCloture = value
        End Set
    End Property

    Public Property Etat As String
        Get
            Return _etat
        End Get
        Set(value As String)
            _etat = value
        End Set
    End Property

    Public Property DateRendezVous As Date
        Get
            Return _dateRendezVous
        End Get
        Set(value As Date)
            _dateRendezVous = value
        End Set
    End Property

    Public Property TypedemandeRendezVous As String
        Get
            Return _typedemandeRendezVous
        End Get
        Set(value As String)
            _typedemandeRendezVous = value
        End Set
    End Property

    Public Property Cloture As Boolean
        Get
            Return _Cloture
        End Get
        Set(value As Boolean)
            _Cloture = value
        End Set
    End Property

    Public Property DateTraitementDemandeRendezVous As Date
        Get
            Return _dateTraitementDemandeRendezVous
        End Get
        Set(value As Date)
            _dateTraitementDemandeRendezVous = value
        End Set
    End Property

    Public Function getLibelleTacheNature() As String
        Return Tache.getLibelleTacheNature(Me.Type, Me.Nature)
    End Function

    Public Shared Function getLibelleTacheNature(type As String, nature As String) As String
        Dim libel As String = ""
        Select Case type
            Case TacheDao.TypeTache.RDV_DEMANDE.ToString
                libel = "Demande de rendez-vous"
            Case TacheDao.TypeTache.RDV.ToString
                libel = "Rendez-vous"
            Case TacheDao.TypeTache.AVIS_EPISODE.ToString
                libel = StrConv(nature, VbStrConv.ProperCase) + " d'avis sur épisode"
            Case TacheDao.TypeTache.AVIS_SOUS_EPISODE.ToString
                libel = StrConv(nature, VbStrConv.ProperCase) + " d'avis sur sous-épisode"
            Case TacheDao.TypeTache.MISSION_DEMANDE.ToString
                libel = "Demande de Rendez-vous mission"
            Case TacheDao.TypeTache.RDV_MISSION.ToString
                libel = "Rendez-vous mission"
            Case TacheDao.TypeTache.REUNION_STAFF.ToString
                libel = "Réunion Staff"
            Case TacheDao.TypeTache.RDV_SPECIALISTE.ToString
                libel = "Rendez-vous hors Oasis"
        End Select

        Return libel
    End Function
    Public Function isAttribue() As Boolean
        Return (Coalesce(TraiteUserId, 0) <> 0)
    End Function

    Public Function isMyTacheEmetteur() As Boolean
        Return (userLog.UtilisateurId = EmetteurUserId)
    End Function
    Public Function isMyTacheATraiter() As Boolean
        Return (userLog.UtilisateurId = TraiteUserId)
    End Function

    Public Function isAnnulable() As Boolean
        Return ((isMyTacheEmetteur() AndAlso isAttribue() = False) OrElse (isMyTacheATraiter())) AndAlso isStatutFinal() = False
    End Function

    Public Function isRendezVousAFixer() As Boolean
        Return (isMyTacheATraiter() _
                AndAlso Etat = TacheDao.EtatTache.EN_COURS.ToString _
                AndAlso (Type = TacheDao.TypeTache.RDV_DEMANDE.ToString OrElse Type = TacheDao.TypeTache.MISSION_DEMANDE.ToString)
               )
    End Function

    Public Function isAttribuable() As Boolean
        Return isAttribue() = False AndAlso userLog.IsFonctionIdPossible(TraiteFonctionId) AndAlso isStatutFinal() = False
    End Function

    Public Function isDesattribuable() As Boolean
        Return isAttribue() _
            AndAlso (isMyTacheATraiter() OrElse userLog.UtilisateurAdmin) _
            AndAlso (isStatutFinal() = False)
    End Function

    Public Function isStatutFinal() As Boolean
        Return Etat = TacheDao.EtatTache.ANNULEE.ToString OrElse Etat = TacheDao.EtatTache.TERMINEE.ToString
    End Function

    Friend Function isUnRdv() As Boolean
        Return Type = TacheDao.TypeTache.RDV.ToString OrElse Type = TacheDao.TypeTache.RDV_MISSION.ToString OrElse Type = TacheDao.TypeTache.RDV_SPECIALISTE.ToString OrElse Type = TacheDao.TypeTache.REUNION_STAFF.ToString
    End Function

    Friend Function getTypeRdvFromDemande() As TacheDao.TypeTache
        Select Case Type
            Case TacheDao.TypeTache.RDV_DEMANDE.ToString
                Return TacheDao.TypeTache.RDV
            Case TacheDao.TypeTache.MISSION_DEMANDE.ToString
                Return TacheDao.TypeTache.RDV_MISSION
        End Select
        Return Nothing
    End Function


    Public Function isFonctionPossiblePourUser(user As Utilisateur) As Boolean
        Return user.IsFonctionIdPossible(TraiteFonctionId)
    End Function
    Public Function isFonctionPossiblePourUserLog() As Boolean
        Return isFonctionPossiblePourUser(userLog)
    End Function

End Class
