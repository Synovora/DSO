Public Class SousEpisodeType
    Public Property Id As Long
    Public Property Category As String
    Public Property HorodateCreation As DateTime
    Public Property Libelle As String

    Public Property LstSousEpisodeSousType As List(Of SousEpisodeSousType)

    Public Sub New()
    End Sub

    Public Sub New(row As DataRow)
        Me.Id = row("id")
        Me.Category = row("categorie")
        Me.HorodateCreation = row("horodate_creation")
        Me.Libelle = row("libelle")
    End Sub

End Class
