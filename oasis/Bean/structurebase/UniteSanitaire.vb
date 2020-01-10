Imports Oasis_WF

Public Class UniteSanitaire


    Private _oa_unite_sanitaire_id As Integer
    Private _oa_unite_sanitaire_description As String
    Private _oa_unite_sanitaire_siege_id As Integer
    Private _oa_unite_sanitaire_adresse1 As String
    Private _oa_unite_sanitaire_adresse2 As String
    Private _oa_unite_sanitaire_ville As String
    Private _oa_unite_sanitaire_code_postal As String
    Private _oa_unite_sanitaire_inactif As Boolean
    ' liste des sites associés (uniquement rempli dans le filtre tache)
    Private _lstSite As List(Of Site)

    Public Property Oa_unite_sanitaire_id As Integer
        Get
            Return _oa_unite_sanitaire_id
        End Get
        Set(value As Integer)
            _oa_unite_sanitaire_id = value
        End Set
    End Property

    Public Property Oa_unite_sanitaire_description As String
        Get
            Return _oa_unite_sanitaire_description
        End Get
        Set(value As String)
            _oa_unite_sanitaire_description = value
        End Set
    End Property

    Public Property Oa_unite_sanitaire_siege_id As Integer
        Get
            Return _oa_unite_sanitaire_siege_id
        End Get
        Set(value As Integer)
            _oa_unite_sanitaire_siege_id = value
        End Set
    End Property

    Public Property Oa_unite_sanitaire_adresse1 As String
        Get
            Return _oa_unite_sanitaire_adresse1
        End Get
        Set(value As String)
            _oa_unite_sanitaire_adresse1 = value
        End Set
    End Property

    Public Property Oa_unite_sanitaire_adresse2 As String
        Get
            Return _oa_unite_sanitaire_adresse2
        End Get
        Set(value As String)
            _oa_unite_sanitaire_adresse2 = value
        End Set
    End Property

    Public Property Oa_unite_sanitaire_ville As String
        Get
            Return _oa_unite_sanitaire_ville
        End Get
        Set(value As String)
            _oa_unite_sanitaire_ville = value
        End Set
    End Property

    Public Property Oa_unite_sanitaire_code_postal As String
        Get
            Return _oa_unite_sanitaire_code_postal
        End Get
        Set(value As String)
            _oa_unite_sanitaire_code_postal = value
        End Set
    End Property

    Public Property Oa_unite_sanitaire_inactif As Boolean
        Get
            Return _oa_unite_sanitaire_inactif
        End Get
        Set(value As Boolean)
            _oa_unite_sanitaire_inactif = value
        End Set
    End Property

    Public Property LstSite As List(Of Site)
        Get
            Return _lstSite
        End Get
        Set(value As List(Of Site))
            _lstSite = value
        End Set
    End Property

    ''' <summary>
    ''' Ajout d'un site selectionné
    ''' </summary>
    ''' <param name="site"></param>
    Public Sub addSite(site As Site)
        If _lstSite Is Nothing Then
            _lstSite = New List(Of Site)
        End If
        _lstSite.Add(site)
    End Sub

    Public Function Clone() As Object
        Dim newInstance As UniteSanitaire = DirectCast(Me.MemberwiseClone(), UniteSanitaire)
        Return newInstance
    End Function

    ''' <summary>
    ''' return in "(id1,id2, ... , idn)"
    ''' </summary>
    ''' <param name="lstUS"></param>
    ''' <returns></returns>
    Public Shared Function getQueryInForIds(lstUS As List(Of UniteSanitaire)) As String
        Dim lstId As List(Of Long) = New List(Of Long)

        If lstUS Is Nothing Then Return ""
        For Each uniteSanitaire In lstUS
            lstId.Add(uniteSanitaire.Oa_unite_sanitaire_id)
        Next

        Return " in ( " + String.Join(",", lstId) + ") "

    End Function

End Class
