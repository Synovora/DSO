Imports System.Data.SqlClient

Public Class ChaineEpisode
    Property Id As Long
    Property AntecedentId As Long
    Property ChaineId As Long

    Public Sub New()
    End Sub

    Public Sub New(record As System.Data.IDataRecord)
        Me.Id = record("id")
        Me.AntecedentId = record("antecedent_id")
        Me.ChaineId = record("chaine_id")
    End Sub

End Class
