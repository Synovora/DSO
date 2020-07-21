Public Class Medicament
    Private _medicamentCis As Integer
    Private _medicamentDci As String
    Private _forme As String
    Private _voieAdministration As String
    Private _titulaire As String

    Public Property MedicamentCis As Integer
        Get
            Return _medicamentCis
        End Get
        Set(value As Integer)
            _medicamentCis = value
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

    Public Property Forme As String
        Get
            Return _forme
        End Get
        Set(value As String)
            _forme = value
        End Set
    End Property

    Public Property VoieAdministration As String
        Get
            Return _voieAdministration
        End Get
        Set(value As String)
            _voieAdministration = value
        End Set
    End Property

    Public Property Titulaire As String
        Get
            Return _titulaire
        End Get
        Set(value As String)
            _titulaire = value
        End Set
    End Property
End Class
