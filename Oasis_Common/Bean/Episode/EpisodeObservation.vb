Public Class EpisodeObservation
    Property Id As Long
    Property EpisodeId As Long
    Property PatientId As Long
    Property TypeObservation As String      'M: Medicale, P: Paramédicale
    Property NatureObservation As String    'S: Spécifique, L: Libre
    Property NaturePresence As String       'P: Présentiel, T: A distance
    Property Observation As String
    Property UserCreation As Long
    Property DateCreation As DateTime
    Property DateModification As DateTime
    Property Inactif As Boolean

    Public Function Clone() As EpisodeObservation
        Dim newInstance As EpisodeObservation = DirectCast(Me.MemberwiseClone(), EpisodeObservation)
        Return newInstance
    End Function

End Class
