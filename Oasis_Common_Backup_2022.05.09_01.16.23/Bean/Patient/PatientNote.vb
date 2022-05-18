Public Class PatientNote

    Property NoteId As Integer
    Property PatientId As Integer
    Property PatientNote As String
    Property UserCreation As Integer
    Property DateCreation As Date
    Property UserModification As Integer
    Property DateModification As Date
    Property Invalide As Boolean

    Public Function Clone() As PatientNote
        Dim newInstance As PatientNote = DirectCast(Me.MemberwiseClone(), PatientNote)
        Return newInstance
    End Function

End Class
