Public Class SpecialiteAllergique
    Private _allergie As Boolean
    Private _messageAllergie As String

    Public Property Allergie As Boolean
        Get
            Return _allergie
        End Get
        Set(value As Boolean)
            _allergie = value
        End Set
    End Property

    Public Property MessageAllergie As String
        Get
            Return _messageAllergie
        End Get
        Set(value As String)
            _messageAllergie = value
        End Set
    End Property
End Class
