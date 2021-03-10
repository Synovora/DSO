Imports Telerik.WinControls.UI

Public Class FrenchSchedulerNavigatorLocalizationProvider
    Inherits SchedulerNavigatorLocalizationProvider
    Public Overrides Function GetLocalizedString(ByVal id As String) As String
        Select Case id
            Case SchedulerNavigatorStringId.DayViewButtonCaption
                Return "Jour"
            Case SchedulerNavigatorStringId.WeekViewButtonCaption
                Return "Semaine"
            Case SchedulerNavigatorStringId.MonthViewButtonCaption
                Return "Mois"
            Case SchedulerNavigatorStringId.TimelineViewButtonCaption
                Return "Temps"
            Case SchedulerNavigatorStringId.ShowWeekendCheckboxCaption
                Return "Voir Weekend"
            Case SchedulerNavigatorStringId.TodayButtonCaptionToday
                Return "Aujourd'hui"
            Case SchedulerNavigatorStringId.TodayButtonCaptionThisWeek
                Return "Cette semaine"
            Case SchedulerNavigatorStringId.TodayButtonCaptionThisMonth
                Return "Ce mois"
            Case SchedulerNavigatorStringId.SearchInAppointments
                Return "Rechercher"
        End Select
        Return String.Empty
    End Function
End Class
