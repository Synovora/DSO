Imports System.Data.SqlClient
Public Class AldCim10
    Private _aldCim10Id As Integer
    Private _aldCim10AldId As Integer
    Private _aldCim10AldCode As String
    Private _aldCim10Code As String
    Private _aldCim10Description As String

    Public Property AldCim10Id As Integer
        Get
            Return _aldCim10Id
        End Get
        Set(value As Integer)
            _aldCim10Id = value
        End Set
    End Property

    Public Property AldCim10AldId As Integer
        Get
            Return _aldCim10AldId
        End Get
        Set(value As Integer)
            _aldCim10AldId = value
        End Set
    End Property

    Public Property AldCim10AldCode As String
        Get
            Return _aldCim10AldCode
        End Get
        Set(value As String)
            _aldCim10AldCode = value
        End Set
    End Property

    Public Property AldCim10Code As String
        Get
            Return _aldCim10Code
        End Get
        Set(value As String)
            _aldCim10Code = value
        End Set
    End Property

    Public Property AldCim10Description As String
        Get
            Return _aldCim10Description
        End Get
        Set(value As String)
            _aldCim10Description = value
        End Set
    End Property

    Sub New()
        InitInstance()
    End Sub

    Sub New(aldCim10Id As Integer)
        AldCim10Dao.SetAldCim10(Me, aldCim10Id)
    End Sub

    Private Sub InitInstance()
        Me.AldCim10Id = 0
        Me.AldCim10AldId = 0
        Me.AldCim10AldCode = ""
        Me.AldCim10Code = ""
        Me.AldCim10Description = ""
    End Sub

    Public Function Clone() As AldCim10
        Dim newInstance As AldCim10 = DirectCast(Me.MemberwiseClone(), AldCim10)
        Return newInstance
    End Function

End Class
