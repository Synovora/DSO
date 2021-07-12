Imports System.Data.SqlClient

Public Class ChaineEpisode
    Property Id As Long
    Property AntecedentId As Long
    Property ChaineId As Long
    Property Actif As Boolean
    Property Antecedent As Antecedent

    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.AntecedentId = Coalesce(reader("antecedent_id"), Nothing)
        Me.ChaineId = Coalesce(reader("chaine_id"), Nothing)
        Me.Actif = reader("actif")
        Me.Antecedent = New Antecedent(reader)
    End Sub

End Class
