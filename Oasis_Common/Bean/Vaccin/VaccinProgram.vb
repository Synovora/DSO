Imports System.Data.SqlClient

Public Class VaccinProgramAdmin
    Property Id As Long
    Property VaccinProgramRelation As Long
    Property Lot As String
    Property Expiration As Date
    Property Comment As String

    Public Sub New()
    End Sub

    Public Sub New(record As System.Data.IDataRecord)
        Me.Id = record("id")
        Me.VaccinProgramRelation = record("vaccin_program_relation")
        Me.Lot = record("lot")
        Me.Expiration = record("expiration")
        Me.Comment = record("comment")
    End Sub

End Class