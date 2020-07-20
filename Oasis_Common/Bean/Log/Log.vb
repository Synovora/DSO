Public Class Log
    Public Enum EnumTypeLog
        ERREUR
        INFO
    End Enum

    Private _id As Long
    Private _description As String
    Private _origine As String
    Private _typeLog As String
    Private _dateLog As Date
    Private _userLog As Long

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

    Public Property Origine As String
        Get
            Return _origine
        End Get
        Set(value As String)
            _origine = value
        End Set
    End Property

    Public Property DateLog As Date
        Get
            Return _dateLog
        End Get
        Set(value As Date)
            _dateLog = value
        End Set
    End Property

    Public Property UserLog As Long
        Get
            Return _userLog
        End Get
        Set(value As Long)
            _userLog = value
        End Set
    End Property

    Public Property TypeLog As String
        Get
            Return _typeLog
        End Get
        Set(value As String)
            _typeLog = value
        End Set
    End Property
End Class
