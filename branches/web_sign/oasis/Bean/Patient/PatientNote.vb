Public Class PatientNote
    Private _noteId As Integer
    Private _patientId As Integer
    Private _patientNote As String
    Private _userCreation As Integer
    Private _dateCreation As Date
    Private _userModification As Integer
    Private _dateModification As Date
    Private _invalide As Boolean

    Public Property NoteId As Integer
        Get
            Return _noteId
        End Get
        Set(value As Integer)
            _noteId = value
        End Set
    End Property

    Public Property PatientId As Integer
        Get
            Return _patientId
        End Get
        Set(value As Integer)
            _patientId = value
        End Set
    End Property

    Public Property UserCreation As Integer
        Get
            Return _userCreation
        End Get
        Set(value As Integer)
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

    Public Property UserModification As Integer
        Get
            Return _userModification
        End Get
        Set(value As Integer)
            _userModification = value
        End Set
    End Property

    Public Property DateModification As Date
        Get
            Return _dateModification
        End Get
        Set(value As Date)
            _dateModification = value
        End Set
    End Property

    Public Property PatientNote As String
        Get
            Return _patientNote
        End Get
        Set(value As String)
            _patientNote = value
        End Set
    End Property

    Public Property Invalide As Boolean
        Get
            Return _invalide
        End Get
        Set(value As Boolean)
            _invalide = value
        End Set
    End Property

    Public Function Clone() As PatientNote
        Dim newInstance As PatientNote = DirectCast(Me.MemberwiseClone(), PatientNote)
        Return newInstance
    End Function

End Class
