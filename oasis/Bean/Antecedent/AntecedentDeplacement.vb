Public Class AntecedentDeplacement
    Private _id As Integer
    Private _niveau As Integer
    Private _niveau1Id As Integer
    Private _niveau2Id As Integer
    Private _ordre1 As Integer
    Private _ordre2 As Integer
    Private _ordre3 As Integer

    Public Property Id As Integer
        Get
            Return _id
        End Get
        Set(value As Integer)
            _id = value
        End Set
    End Property

    Public Property Niveau As Integer
        Get
            Return _niveau
        End Get
        Set(value As Integer)
            _niveau = value
        End Set
    End Property

    Public Property Niveau1Id As Integer
        Get
            Return _niveau1Id
        End Get
        Set(value As Integer)
            _niveau1Id = value
        End Set
    End Property

    Public Property Niveau2Id As Integer
        Get
            Return _niveau2Id
        End Get
        Set(value As Integer)
            _niveau2Id = value
        End Set
    End Property

    Public Property Ordre1 As Integer
        Get
            Return _ordre1
        End Get
        Set(value As Integer)
            _ordre1 = value
        End Set
    End Property

    Public Property Ordre2 As Integer
        Get
            Return _ordre2
        End Get
        Set(value As Integer)
            _ordre2 = value
        End Set
    End Property

    Public Property Ordre3 As Integer
        Get
            Return _ordre3
        End Get
        Set(value As Integer)
            _ordre3 = value
        End Set
    End Property
End Class
