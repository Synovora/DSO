Imports System.Data.SqlClient

Public Class CGVValence

    Property Id As Long
    Property Code As String
    Property Description As String
    Property Precaution As String
    Property Valence As Long
    Property Ordre As Integer
    Property Patient As Long

    Public Sub New()
    End Sub

    Public Sub New(record As System.Data.IDataRecord)
        Me.Id = record("id")
        Me.Code = record("code")
        Me.Description = record("description")
        Me.Precaution = record("precaution")
        Me.Valence = record("valence")
        Me.Ordre = record("ordre")
        Me.Patient = record("patient")
    End Sub

End Class

