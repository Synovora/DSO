Public Class Parametre
    Private _id As Long
    Private _description As String
    Private _entier As Integer
    Private _decimal As Integer
    Private _unite As String
    Private _valeurMin As Decimal
    Private _valeurMax As Decimal
    Private _inactif As Boolean

    Public Property Id As Long
        Get
            Return _id
        End Get
        Set(value As Long)
            _id = value
        End Set
    End Property

    Public Property Description As String
        Get
            Return _description
        End Get
        Set(value As String)
            _description = value
        End Set
    End Property

    Public Property Entier As Integer
        Get
            Return _entier
        End Get
        Set(value As Integer)
            _entier = value
        End Set
    End Property

    Public Property [Decimal] As Integer
        Get
            Return _decimal
        End Get
        Set(value As Integer)
            _decimal = value
        End Set
    End Property

    Public Property Unite As String
        Get
            Return _unite
        End Get
        Set(value As String)
            _unite = value
        End Set
    End Property

    Public Property ValeurMin As Decimal
        Get
            Return _valeurMin
        End Get
        Set(value As Decimal)
            _valeurMin = value
        End Set
    End Property

    Public Property ValeurMax As Decimal
        Get
            Return _valeurMax
        End Get
        Set(value As Decimal)
            _valeurMax = value
        End Set
    End Property

    Public Property Inactif As Boolean
        Get
            Return _inactif
        End Get
        Set(value As Boolean)
            _inactif = value
        End Set
    End Property
End Class
