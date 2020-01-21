Public Class OrdonnanceDetailGrid
    Private _posologie As String
    Private _medicamentDci As String
    Private _medicamentCis As Long
    Private _traitementId As Long
    Private _OrdonnanceLigneId As Long
    Private _duree As String
    Private _fenetreTherapeutique As Boolean
    Private _commentairePosologie As String
    Private _aDelivrer As Boolean
    Private _ald As Boolean


    Public Property Posologie As String
        Get
            Return _posologie
        End Get
        Set(value As String)
            _posologie = value
        End Set
    End Property

    Public Property MedicamentDci As String
        Get
            Return _medicamentDci
        End Get
        Set(value As String)
            _medicamentDci = value
        End Set
    End Property

    Public Property MedicamentCis As Long
        Get
            Return _medicamentCis
        End Get
        Set(value As Long)
            _medicamentCis = value
        End Set
    End Property

    Public Property TraitementId As Long
        Get
            Return _traitementId
        End Get
        Set(value As Long)
            _traitementId = value
        End Set
    End Property

    Public Property OrdonnanceLigneId As Long
        Get
            Return _OrdonnanceLigneId
        End Get
        Set(value As Long)
            _OrdonnanceLigneId = value
        End Set
    End Property

    Public Property Duree As String
        Get
            Return _duree
        End Get
        Set(value As String)
            _duree = value
        End Set
    End Property

    Public Property FenetreTherapeutique As Boolean
        Get
            Return _fenetreTherapeutique
        End Get
        Set(value As Boolean)
            _fenetreTherapeutique = value
        End Set
    End Property

    Public Property CommentairePosologie As String
        Get
            Return _commentairePosologie
        End Get
        Set(value As String)
            _commentairePosologie = value
        End Set
    End Property

    Public Property ADelivrer As Boolean
        Get
            Return _aDelivrer
        End Get
        Set(value As Boolean)
            _aDelivrer = value
        End Set
    End Property

    Public Property Ald As Boolean
        Get
            Return _ald
        End Get
        Set(value As Boolean)
            _ald = value
        End Set
    End Property
End Class
