Public Class ContreIndication
    Private _contreIndicationId As Long
    Private _patientId As Long
    Private _ATCId As String
    Private _denominationATC As String
    Private _userCreation As Long
    Private _dateCreation As DateTime
    Private _userAnnulation As Long
    Private _dateAnnulation As DateTime
    Private _inactif As Boolean

    Public Property ContreIndicationId As Long
        Get
            Return _contreIndicationId
        End Get
        Set(value As Long)
            _contreIndicationId = value
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

    Public Property ATCId As String
        Get
            Return _ATCId
        End Get
        Set(value As String)
            _ATCId = value
        End Set
    End Property

    Public Property DenominationATC As String
        Get
            Return _denominationATC
        End Get
        Set(value As String)
            _denominationATC = value
        End Set
    End Property

    Public Property UserCreation As Long
        Get
            Return _userCreation
        End Get
        Set(value As Long)
            _userCreation = value
        End Set
    End Property

    Public Property DateCreation As Date
        Get
            Return _dateCreation
        End Get
        Set(value As Date)
            _dateCreation = value
        End Set
    End Property

    Public Property UserAnnulation As Long
        Get
            Return _userAnnulation
        End Get
        Set(value As Long)
            _userAnnulation = value
        End Set
    End Property

    Public Property DateAnnulation As Date
        Get
            Return _dateAnnulation
        End Get
        Set(value As Date)
            _dateAnnulation = value
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
