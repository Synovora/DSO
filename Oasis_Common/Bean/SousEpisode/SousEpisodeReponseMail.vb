Imports System.Data.SqlClient

Public Class SousEpisodeReponseMail
    Property Id As Long
    Property HorodateCreation As DateTime
    Property PatientId As Long
    Property Status As String
    Property Auteur As String
    Property Objet As String
    Property Corps As String

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.HorodateCreation = reader("horodate_creation")
        Me.PatientId = Coalesce(reader("patient_id"), Nothing)
        Me.Status = reader("status")
        Me.Auteur = reader("auteur")
        Me.Objet = Coalesce(reader("objet"), "")
        Me.Corps = If(HasColumn(reader, "corps"), Coalesce(reader("corps"), ""), Nothing)
    End Sub

End Class
