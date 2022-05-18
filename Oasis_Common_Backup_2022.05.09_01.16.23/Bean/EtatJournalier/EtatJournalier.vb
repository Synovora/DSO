Imports System.Collections.Specialized

Public Class EtatJournalier

    Property Id As Integer
    Property Nir As Long
    Property Prenom As String
    Property Nom As String
    Property INS As Long
    Property SiteId As Integer
    Property Type As String
    Property PatientId As Long
    Property EpisodeId As Long

    Public Function Clone() As Episode
        Dim newInstance As Episode = DirectCast(Me.MemberwiseClone(), Episode)
        Return newInstance
    End Function

End Class
