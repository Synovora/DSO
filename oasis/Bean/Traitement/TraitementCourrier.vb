Public Class TraitementCourrier
    Private _traitementId As Integer
    Private _patientId As Integer
    Private _denomination As String
    Private _posologie As String

    Public Property TraitementId As Integer
        Get
            Return _traitementId
        End Get
        Set(value As Integer)
            _traitementId = value
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

    Public Property Denomination As String
        Get
            Return _denomination
        End Get
        Set(value As String)
            _denomination = value
        End Set
    End Property

    Public Property Posologie As String
        Get
            Return _posologie
        End Get
        Set(value As String)
            _posologie = value
        End Set
    End Property
End Class
