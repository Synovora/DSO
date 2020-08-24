Public Class Profil
    Private _Id As String
    Private _Designation As String
    Private _NiveauAcces As Integer
    Private _type As String
    Private _fonctionParDefautId As Long
    Private _inactif As String

    Public Property Id As String
        Get
            Return _Id
        End Get
        Set(value As String)
            _Id = value
        End Set
    End Property

    Public Property Designation As String
        Get
            Return _Designation
        End Get
        Set(value As String)
            _Designation = value
        End Set
    End Property

    Public Property NiveauAcces As Integer
        Get
            Return _NiveauAcces
        End Get
        Set(value As Integer)
            _NiveauAcces = value
        End Set
    End Property

    Public Property Inactif As String
        Get
            Return _inactif
        End Get
        Set(value As String)
            _inactif = value
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

    Public Property FonctionParDefautId As Long
        Get
            Return _fonctionParDefautId
        End Get
        Set(value As Long)
            _fonctionParDefautId = value
        End Set
    End Property
End Class
