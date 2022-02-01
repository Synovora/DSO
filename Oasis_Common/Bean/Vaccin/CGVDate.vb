Imports System.Data.SqlClient

Public Class CGVDate

    Property Id As Long
    Property Days As Long
    Property Patient As Long

    Property PerformBy As Long
    Property PerformDate As Date
    Property OperatedBy As Long
    Property OperatedDate As Date

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

    Shared Function DaysToDate(days As Long) As String
        Dim dayPerMonth = 30.44
        Dim monthPerYear = 12
        Dim showMaxMonths = 40
        If days < dayPerMonth Then
            Return String.Format("{0} Jours", Math.Round(days))
        ElseIf days / dayPerMonth < showMaxMonths Then
            Return String.Format("{0} Mois", Math.Round(days / dayPerMonth))
        Else
            Return String.Format("{0} Ans", Math.Round(days / dayPerMonth / monthPerYear))
        End If
    End Function

    Shared Function DateToDays(days As Long, months As Long, years As Long) As Long
        Dim dayPerMonth = 30.44
        Dim monthPerYear = 12
        Return Math.Round(days + months * dayPerMonth + years * monthPerYear * dayPerMonth)
    End Function

End Class

Public Class RelationValenceDate

    Property Id As Long
    Property Valence As Long
    Property [Date] As Long
    Property Patient As Long
    Property Status As Boolean


    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.Valence = reader("valence")
        Me.Date = reader("date")
        Me.Patient = reader("patient")
        Me.Status = reader("status")
    End Sub

End Class

