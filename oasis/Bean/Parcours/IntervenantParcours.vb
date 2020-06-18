Public Class IntervenantParcours
    Private _intervenantId As Long
    Private _patientId As Long
    Private _nom As String
    Private _specialite As String
    Private _structure As String

    Public Property IntervenantId As Long
        Get
            Return _intervenantId
        End Get
        Set(value As Long)
            _intervenantId = value
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

    Public Property Nom As String
        Get
            Return _nom
        End Get
        Set(value As String)
            _nom = value
        End Set
    End Property

    Public Property Specialite As String
        Get
            Return _specialite
        End Get
        Set(value As String)
            _specialite = value
        End Set
    End Property

    Public Property [Structure] As String
        Get
            Return _structure
        End Get
        Set(value As String)
            _structure = value
        End Set
    End Property
End Class
