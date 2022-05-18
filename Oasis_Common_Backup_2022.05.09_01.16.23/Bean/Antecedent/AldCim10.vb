
Public Class AldCim10
    Property AldCim10Id As Integer
    Property AldCim10AldId As Integer
    Property AldCim10AldCode As String
    Property AldCim10Code As String
    Property AldCim10Description As String

    Sub New()
        InitInstance()
    End Sub

    Public Sub InitInstance()
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
