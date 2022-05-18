Public Class Ald
    Property AldId As Integer
    Property AldCode As String
    Property AldDescription As String

    Public Function Clone() As Ald
        Dim newInstance As Ald = DirectCast(Me.MemberwiseClone(), Ald)
        Return newInstance
    End Function

End Class
