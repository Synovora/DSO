Imports System.Data.SqlClient

Public Class CGVDate

    Property Id As Long
    Property Days As Long
    Property Patient As Long

    Property PerformBy As Long?
    Property PerformDate As Date?
    Property OperatedBy As Long?
    Property OperatedDate As Date?

    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.Days = reader("days")
        Me.Patient = reader("patient")
        Me.PerformBy = Coalesce(reader("perform_by"), Nothing)
        Me.PerformDate = Coalesce(reader("perform_date"), Nothing)
        Me.OperatedBy = Coalesce(reader("operated_by"), Nothing)
        Me.OperatedDate = Coalesce(reader("operated_date"), Nothing)
    End Sub

End Class

Public Class RelationValenceDate

    Property Id As Long
    Property Valence As Long
    Property [Date] As Long
    Property Patient As Long


    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.Valence = reader("valence")
        Me.Date = reader("date")
        Me.Patient = reader("patient")
    End Sub

End Class

