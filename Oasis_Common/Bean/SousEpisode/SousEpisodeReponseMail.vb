Public Class SousEpisodeReponseMail
    Property Id As Long
    Property HorodateCreation As DateTime
    Property PatientId As Long
    Property Status As String
    Property Auteur As String

    Public Sub New()
    End Sub

    Public Sub New(row As DataRow)
        Me.Id = row("id")
        Me.HorodateCreation = row("horodate_creation")
        Me.PatientId = Coalesce(row("patient_id"), Nothing)
        Me.Status = row("status")
        Me.Auteur = row("auteur")
    End Sub

End Class
