Imports System.Data.SqlClient

Public Class SousEpisodeReponseMail
    Property Id As Long
    Property HorodateCreation As DateTime
    Property PatientId As Long
    Property Status As String
    Property Auteur As String
    Property Objet As String
    Property Corps As String

    Public Sub New(record As System.Data.Common.DbDataReader)
        Me.Id = record("id")
        Me.HorodateCreation = record("horodate_creation")
        Me.PatientId = Coalesce(record("patient_id"), Nothing)
        Me.Status = record("status")
        Me.Auteur = record("auteur")
        Me.Objet = Coalesce(record("objet"), "")
        Me.Corps = If(HasColumn(record, "corps"), Coalesce(record("corps"), ""), Nothing)
    End Sub

End Class
