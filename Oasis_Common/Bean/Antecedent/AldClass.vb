Public Class Ald
    Private _AldId As Integer
    Private _AldCode As String
    Private _AldDescription As String

    Public Property AldId As Integer
        Get
            Return _AldId
        End Get
        Set(value As Integer)
            _AldId = value
        End Set
    End Property

    Public Property AldCode As String
        Get
            Return _AldCode
        End Get
        Set(value As String)
            _AldCode = value
        End Set
    End Property

    Public Property AldDescription As String
        Get
            Return _AldDescription
        End Get
        Set(value As String)
            _AldDescription = value
        End Set
    End Property

    Public Function Clone() As Ald
        Dim newInstance As Ald = DirectCast(Me.MemberwiseClone(), Ald)
        Return newInstance
    End Function

End Class
