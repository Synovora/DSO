Imports System.Data.SqlClient

Public Class ChaineEpisode
    Property Id As Long
    Property AntecedentId As Long
    Property ChaineId As Long

    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.AntecedentId = reader("antecedent_id")
        Me.ChaineId = reader("chaine_id")
    End Sub

End Class
