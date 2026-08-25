Imports System.Data.SqlClient

Public Class CGVDate

    Property Id As Long
    Property Days As Long
    Property Patient As Long

    Property OperatedBy As Long
    Property OperatedDate As Date
    Property SignedBy As Long
    Property SignedDate As Date

    Public Sub New()
    End Sub

    Public Sub New(record As System.Data.IDataRecord)
        Me.Id = record("id")
        Me.Days = record("days")
        Me.Patient = record("patient")
        Me.OperatedBy = Coalesce(record("operated_by"), Nothing)
        Me.OperatedDate = Coalesce(record("operated_date"), Nothing)
        Me.SignedBy = Coalesce(record("signed_by"), Nothing)
        Me.SignedDate = Coalesce(record("signed_date"), Nothing)
    End Sub

    ''' <summary>
    ''' Libellé d'un âge en jours : jours seuls sous un mois, mois et jours
    ''' jusqu'à 40 mois, ans et mois au-delà. Mois de 30 jours, année de 12 mois,
    ''' comme DateToDays.
    '''
    ''' Les mois étaient obtenus par Math.Round sur days / 30 : 45 jours
    ''' donnaient « 2 Mois 15 Jours », soit 75 jours, et 105 jours « 4 Mois 15
    ''' Jours ». Un calendrier vaccinal ne doit pas surestimer l'âge d'un mois.
    ''' </summary>
    Shared Function DaysToDate(days As Long) As String
        Const dayPerMonth As Long = 30
        Const monthPerYear As Long = 12
        Const showMaxMonths As Long = 40

        Dim mois As Long = days \ dayPerMonth
        Dim joursRestants As Long = days Mod dayPerMonth

        If days < dayPerMonth Then
            Return String.Format("{0} Jours", days)
        ElseIf mois < showMaxMonths Then
            Return If(joursRestants > 0,
                      String.Format("{0} Mois {1} Jours", mois, joursRestants),
                      String.Format("{0} Mois", mois))
        Else
            Dim ans As Long = mois \ monthPerYear
            Dim moisRestants As Long = mois Mod monthPerYear
            Return If(moisRestants > 0,
                      String.Format("{0} Ans {1} Mois", ans, moisRestants),
                      String.Format("{0} Ans", ans))
        End If
    End Function

    Shared Function DateToDays(days As Long, months As Long, years As Long) As Long
        Dim dayPerMonth = 30
        Dim monthPerYear = 12
        Return Math.Round(days + months * dayPerMonth + years * monthPerYear * dayPerMonth)
    End Function

End Class

Public Class RelationValenceDate

    Property Id As Long
    Property Valence As Long
    Property [Date] As Long
    Property Patient As Long
    Property Status As Short


    Public Sub New()
    End Sub

    Public Sub New(record As System.Data.IDataRecord)
        Me.Id = record("id")
        Me.Valence = record("valence")
        Me.Date = record("date")
        Me.Patient = record("patient")
        Me.Status = record("status")
    End Sub

End Class

