Imports System.Data.SqlClient

Public Class VaccinProgramAdmin
    Property Id As Long
    Property VaccinProgramRelation As Long
    Property Lot As String
    Property Expiration As Date
    Property Comment As String

    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.VaccinProgramRelation = reader("vaccin_program_relation")
        Me.Lot = reader("lot")
        Me.Expiration = reader("expiration")
        Me.Comment = reader("comment")
    End Sub

End Class