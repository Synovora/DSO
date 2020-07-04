Imports Oasis_WF
Imports Oasis_Common
Public Class TacheBeanAssocie
    Private _userEmetteur As Utilisateur
    Private _fonctionEmetteur As Fonction
    Private _uniteSanitaire As UniteSanitaire
    Private _site As Site
    Private _patient As PatientBase
    Private _parcours As Parcours
    Private _episode As Episode
    Private _sousEpisode As Episode
    Private _specialite As Specialite
    Private _intervenant As String = "Oasis"
    Dim _fonctionTraiteur As Fonction
    Private _userTraiteur As Utilisateur

    Public Property UserEmetteur As Utilisateur
        Get
            Return _userEmetteur
        End Get
        Set(value As Utilisateur)
            _userEmetteur = value
        End Set
    End Property

    Public Property FonctionEmetteur As Fonction
        Get
            Return _fonctionEmetteur
        End Get
        Set(value As Fonction)
            _fonctionEmetteur = value
        End Set
    End Property

    Public Property UniteSanitaire As UniteSanitaire
        Get
            Return _uniteSanitaire
        End Get
        Set(value As UniteSanitaire)
            _uniteSanitaire = value
        End Set
    End Property

    Public Property Site As Site
        Get
            Return _site
        End Get
        Set(value As Site)
            _site = value
        End Set
    End Property

    Public Property Patient As PatientBase
        Get
            Return _patient
        End Get
        Set(value As PatientBase)
            _patient = value
        End Set
    End Property

    Public Property Parcours As Parcours
        Get
            Return _parcours
        End Get
        Set(value As Parcours)
            _parcours = value
        End Set
    End Property

    Public Property Episode As Episode
        Get
            Return _episode
        End Get
        Set(value As Episode)
            _episode = value
        End Set
    End Property

    Public Property SousEpisode As Episode
        Get
            Return _sousEpisode
        End Get
        Set(value As Episode)
            _sousEpisode = value
        End Set
    End Property

    Public Property Specialite As Specialite
        Get
            Return _specialite
        End Get
        Set(value As Specialite)
            _specialite = value
        End Set
    End Property

    Public Property Intervenant As String
        Get
            Return _intervenant
        End Get
        Set(value As String)
            _intervenant = value
        End Set
    End Property

    Public Property FonctionTraiteur As Fonction
        Get
            Return _fonctionTraiteur
        End Get
        Set(value As Fonction)
            _fonctionTraiteur = value
        End Set
    End Property

    Public Property UserTraiteur As Utilisateur
        Get
            Return _userTraiteur
        End Get
        Set(value As Utilisateur)
            _userTraiteur = value
        End Set
    End Property
End Class
