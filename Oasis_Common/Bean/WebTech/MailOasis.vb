Imports Oasis_Common.ParametreMail

Public Class MailOasis

    Property IdSiege As Long
    Property AliasFrom As String
    Property AdressTo As String
    Property Subject As String
    Property Body As String
    Property Filename As String
    Property Contenu As Byte()
    Property IsSousEpisode As Boolean
    Property Type As TypeMailParams

    Public Function IsWithContenu() As Boolean
        Return Filename <> Nothing AndAlso Not IsNothing(Contenu) AndAlso Contenu.Length > 0
    End Function

End Class
