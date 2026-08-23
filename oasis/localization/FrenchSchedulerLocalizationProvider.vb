Imports Telerik.WinControls.UI.Localization

Public Class FrenchSchedulerLocalizationProvider
    Inherits RadSchedulerLocalizationProvider

    Public Overrides Function GetLocalizedString(id As String) As String
        Select Case id
            Case RadSchedulerStringId.NextAppointment
                Return "Prochain RDV"
            Case RadSchedulerStringId.PreviousAppointment
                Return "Précédent RDV"
            Case RadSchedulerStringId.AppointmentDialogTitle
                Return "Modifier RDV"
            Case RadSchedulerStringId.AppointmentDialogSubject
                Return "Sujet:"
            Case RadSchedulerStringId.AppointmentDialogLocation
                Return "Endroit:"
            Case RadSchedulerStringId.AppointmentDialogBackground
                Return "Qualificatif:"
            Case RadSchedulerStringId.AppointmentDialogDescription
                Return "Description:"
            Case RadSchedulerStringId.AppointmentDialogStartTime
                Return "Heure début:"
            Case RadSchedulerStringId.AppointmentDialogEndTime
                Return "Heure fin:"
            Case RadSchedulerStringId.AppointmentDialogAllDay
                Return "Toute la journée"
            Case RadSchedulerStringId.AppointmentDialogResource
                Return "Ressource:"
            Case RadSchedulerStringId.AppointmentDialogStatus
                Return "Montrer l'heure comme :"
            Case RadSchedulerStringId.AppointmentDialogOK
                Return "OK"
            Case RadSchedulerStringId.AppointmentDialogCancel
                Return "Annuler"
            Case RadSchedulerStringId.AppointmentDialogDelete
                Return "Supprimer"
            Case RadSchedulerStringId.AppointmentDialogRecurrence
                Return "Récurrence"
            Case RadSchedulerStringId.OpenRecurringDialogTitle
                Return "Ouvrir l'élément récurrent"
            Case RadSchedulerStringId.DeleteRecurrenceDialogOK
                Return "OK"
            Case RadSchedulerStringId.OpenRecurringDialogOK
                Return "OK"
            Case RadSchedulerStringId.DeleteRecurrenceDialogCancel
                Return "Annuler"
            Case RadSchedulerStringId.OpenRecurringDialogCancel
                Return "Annuler"
            Case RadSchedulerStringId.OpenRecurringDialogLabel
                Return """{0}"" est un RDV" & vbLf & "récurrent. Voulez  vous ouvrir" & vbLf & "seulement cette occurrence ou les séries?"
            Case RadSchedulerStringId.OpenRecurringDialogRadioOccurrence
                Return "Ouvrir cette occurrence."
            Case RadSchedulerStringId.OpenRecurringDialogRadioSeries
                Return "Ouvrir les séries."
            Case RadSchedulerStringId.DeleteRecurrenceDialogTitle
                Return "Confirmez la Suppression"
            Case RadSchedulerStringId.DeleteRecurrenceDialogRadioOccurrence
                Return "Supprimer cette occurrence."
            Case RadSchedulerStringId.DeleteRecurrenceDialogRadioSeries
                Return "Supprimer cette série."
            Case RadSchedulerStringId.DeleteRecurrenceDialogLabel
                Return "Do you want to delete all occurrences of the recurring appointment ""{0}"", or just this one?"
            Case RadSchedulerStringId.RecurrenceDragDropCreateExceptionDialogText
                Return "You changed the date of a single occurrence of a recurring appointment. To change all the dates, open the series." & vbLf & "Do you want to change just this one?"
            Case RadSchedulerStringId.RecurrenceDragDropValidationSameDateText
                Return "Two occurrences of the same series cannot occur on the same day."
            Case RadSchedulerStringId.RecurrenceDragDropValidationSkipOccurrenceText
                Return "Cannot reschedule an occurrence of a recurring appointment if it skips over a later occurrence of the same appointment."
            Case RadSchedulerStringId.RecurrenceDialogMessageBoxText
                Return "Start date should be before EndBy date."
            Case RadSchedulerStringId.RecurrenceDialogMessageBoxWrongRecurrenceRuleText
                Return "The recurrence pattern is not valid."
            Case RadSchedulerStringId.RecurrenceDialogMessageBoxTitle
                Return "Validation error"
            Case RadSchedulerStringId.RecurrenceDialogTitle
                Return "Modifier Récurrence"
            Case RadSchedulerStringId.RecurrenceDialogAppointmentTimeGroup
                Return "Heure du RDV"
            Case RadSchedulerStringId.RecurrenceDialogDuration
                Return "Durée:"
            Case RadSchedulerStringId.RecurrenceDialogAppointmentEnd
                Return "Fin:"
            Case RadSchedulerStringId.RecurrenceDialogAppointmentStart
                Return "Début:"
            Case RadSchedulerStringId.RecurrenceDialogRecurrenceGroup
                Return "Modèle de Récurrence"
            Case RadSchedulerStringId.RecurrenceDialogRangeGroup
                Return "Intervalle de la récurrence"
            Case RadSchedulerStringId.RecurrenceDialogOccurrences
                Return "occurrences"
            Case RadSchedulerStringId.RecurrenceDialogRecurrenceStart
                Return "Début:"
            Case RadSchedulerStringId.RecurrenceDialogYearly
                Return "Annuel"
            Case RadSchedulerStringId.RecurrenceDialogHourly
                Return "Toute les heures"
            Case RadSchedulerStringId.RecurrenceDialogMonthly
                Return "Mensuel"
            Case RadSchedulerStringId.RecurrenceDialogWeekly
                Return "Hebdo"
            Case RadSchedulerStringId.RecurrenceDialogDaily
                Return "Journalier"
            Case RadSchedulerStringId.RecurrenceDialogEndBy
                Return "Fini le:"
            Case RadSchedulerStringId.RecurrenceDialogEndAfter
                Return "Fin après:"
            Case RadSchedulerStringId.RecurrenceDialogNoEndDate
                Return "Pas de date de fin"
            Case RadSchedulerStringId.RecurrenceDialogAllDay
                Return "Toute la journée"
            Case RadSchedulerStringId.RecurrenceDialogDurationDropDown1Day
                Return "1 jour"
            Case RadSchedulerStringId.RecurrenceDialogDurationDropDown2Days
                Return "2 jours"
            Case RadSchedulerStringId.RecurrenceDialogDurationDropDown3Days
                Return "3 jours"
            Case RadSchedulerStringId.RecurrenceDialogDurationDropDown4Days
                Return "4 jours"
            Case RadSchedulerStringId.RecurrenceDialogDurationDropDown1Week
                Return "1 semaine"
            Case RadSchedulerStringId.RecurrenceDialogDurationDropDown2Weeks
                Return "2 semaines"
            Case RadSchedulerStringId.RecurrenceDialogOK
                Return "OK"
            Case RadSchedulerStringId.RecurrenceDialogCancel
                Return "Annuler"
            Case RadSchedulerStringId.RecurrenceDialogRemoveRecurrence
                Return "Supprimer Recurrence"
            Case RadSchedulerStringId.HourlyRecurrenceEvery
                Return "Tout"
            Case RadSchedulerStringId.HourlyRecurrenceHours
                Return "Heure(s)"
            Case RadSchedulerStringId.DailyRecurrenceEveryDay
                Return "Chaque"
            Case RadSchedulerStringId.DailyRecurrenceEveryWeekday
                Return "Tous les jours de la semaine"
            Case RadSchedulerStringId.DailyRecurrenceDays
                Return "jour(s)"
            Case RadSchedulerStringId.WeeklyRecurrenceRecurEvery
                Return "Répete chaque"
            Case RadSchedulerStringId.WeeklyRecurrenceWeeksOn
                Return "semaine(s) sur:"
            Case RadSchedulerStringId.WeeklyRecurrenceSunday
                Return "Dimanche"
            Case RadSchedulerStringId.WeeklyRecurrenceMonday
                Return "Lundi"
            Case RadSchedulerStringId.WeeklyRecurrenceTuesday
                Return "Mardi"
            Case RadSchedulerStringId.WeeklyRecurrenceWednesday
                Return "Mercredi"
            Case RadSchedulerStringId.WeeklyRecurrenceThursday
                Return "Jeudi"
            Case RadSchedulerStringId.WeeklyRecurrenceFriday
                Return "Vendredi"
            Case RadSchedulerStringId.WeeklyRecurrenceSaturday
                Return "Samedi"
            Case RadSchedulerStringId.WeeklyRecurrenceDay
                Return "Jour"
            Case RadSchedulerStringId.WeeklyRecurrenceWeekday
                Return "Jour de la semaine"
            Case RadSchedulerStringId.WeeklyRecurrenceWeekendDay
                Return "Jour de la semaine"
            Case RadSchedulerStringId.MonthlyRecurrenceDay
                Return "Jour"
            Case RadSchedulerStringId.MonthlyRecurrenceWeek
                Return "Le"
            Case RadSchedulerStringId.MonthlyRecurrenceDayOfMonth
                Return "chaque"
            Case RadSchedulerStringId.MonthlyRecurrenceMonths
                Return "mois"
            Case RadSchedulerStringId.MonthlyRecurrenceWeekOfMonth
                Return "chaque"
            Case RadSchedulerStringId.MonthlyRecurrenceFirst
                Return "Premier"
            Case RadSchedulerStringId.MonthlyRecurrenceSecond
                Return "Second"
            Case RadSchedulerStringId.MonthlyRecurrenceThird
                Return "Troisième"
            Case RadSchedulerStringId.MonthlyRecurrenceFourth
                Return "Quatrième"
            Case RadSchedulerStringId.MonthlyRecurrenceLast
                Return "Dernier"
            Case RadSchedulerStringId.YearlyRecurrenceDayOfMonth
                Return "Chaque"
            Case RadSchedulerStringId.YearlyRecurrenceWeekOfMonth
                Return "Le"
            Case RadSchedulerStringId.YearlyRecurrenceOfMonth
                Return "de"
            Case RadSchedulerStringId.YearlyRecurrenceJanuary
                Return "Janvier"
            Case RadSchedulerStringId.YearlyRecurrenceFebruary
                Return "Février"
            Case RadSchedulerStringId.YearlyRecurrenceMarch
                Return "Mars"
            Case RadSchedulerStringId.YearlyRecurrenceApril
                Return "Avril"
            Case RadSchedulerStringId.YearlyRecurrenceMay
                Return "Mai"
            Case RadSchedulerStringId.YearlyRecurrenceJune
                Return "Juin"
            Case RadSchedulerStringId.YearlyRecurrenceJuly
                Return "Juillet"
            Case RadSchedulerStringId.YearlyRecurrenceAugust
                Return "Août"
            Case RadSchedulerStringId.YearlyRecurrenceSeptember
                Return "Septembre"
            Case RadSchedulerStringId.YearlyRecurrenceOctober
                Return "Octobre"
            Case RadSchedulerStringId.YearlyRecurrenceNovember
                Return "Novembre"
            Case RadSchedulerStringId.YearlyRecurrenceDecember
                Return "Decembre"
            Case RadSchedulerStringId.BackgroundNone
                Return "Aucun"
            Case RadSchedulerStringId.BackgroundImportant
                Return "Important"
            Case RadSchedulerStringId.BackgroundBusiness
                Return "Business"
            Case RadSchedulerStringId.BackgroundPersonal
                Return "Personnel"
            Case RadSchedulerStringId.BackgroundVacation
                Return "Vacances"
            Case RadSchedulerStringId.BackgroundMustAttend
                Return "Incontournable"
            Case RadSchedulerStringId.BackgroundTravelRequired
                Return "Déplacement requis"
            Case RadSchedulerStringId.BackgroundNeedsPreparation
                Return "Demande de la Preparation"
            Case RadSchedulerStringId.BackgroundBirthday
                Return "Anniversaire"
            Case RadSchedulerStringId.BackgroundAnniversary
                Return "Commémoration"
            Case RadSchedulerStringId.BackgroundPhoneCall
                Return "N° Tél"
            Case RadSchedulerStringId.StatusBusy
                Return "Occupé"
            Case RadSchedulerStringId.StatusFree
                Return "Libre"
            Case RadSchedulerStringId.StatusTentative
                Return "Tentative"
            Case RadSchedulerStringId.StatusUnavailable
                Return "Indisponible"
            Case RadSchedulerStringId.ReminderNone
                Return "Aucun"
            Case RadSchedulerStringId.ReminderOneMinute
                Return "1 minute"
            Case RadSchedulerStringId.ReminderMinutes
                Return " minutes"
            Case RadSchedulerStringId.ReminderOneSecond
                Return "1 seconde"
            Case RadSchedulerStringId.ReminderSeconds
                Return " secondes"
            Case RadSchedulerStringId.ReminderDays
                Return " jours"
            Case RadSchedulerStringId.ReminderWeeks
                Return " semaines"
            Case RadSchedulerStringId.ReminderHours
                Return " heures"
            Case RadSchedulerStringId.ReminderZeroMinutes
                Return "0 minute"
            Case RadSchedulerStringId.ReminderFiveMinutes
                Return "5 minutes"
            Case RadSchedulerStringId.ReminderTenMinutes
                Return "10 minutes"
            Case RadSchedulerStringId.ReminderFifteenMinutes
                Return "15 minutes"
            Case RadSchedulerStringId.ReminderThirtyMinutes
                Return "30 minutes"
            Case RadSchedulerStringId.ReminderOneHour
                Return "1 heure"
            Case RadSchedulerStringId.ReminderTwoHours
                Return "2 heures"
            Case RadSchedulerStringId.ReminderThreeHours
                Return "3 heures"
            Case RadSchedulerStringId.ReminderFourHours
                Return "4 heures"
            Case RadSchedulerStringId.ReminderFiveHours
                Return "5 heures"
            Case RadSchedulerStringId.ReminderSixHours
                Return "6 heures"
            Case RadSchedulerStringId.ReminderSevenHours
                Return "7 heures"
            Case RadSchedulerStringId.ReminderEightHours
                Return "8 heures"
            Case RadSchedulerStringId.ReminderNineHours
                Return "9 heures"
            Case RadSchedulerStringId.ReminderTenHours
                Return "10 heures"
            Case RadSchedulerStringId.ReminderElevenHours
                Return "11 heures"
            Case RadSchedulerStringId.ReminderTwelveHours
                Return "12 heures"
            Case RadSchedulerStringId.ReminderEighteenHours
                Return "18 heures"
            Case RadSchedulerStringId.ReminderOneDay
                Return "1 jour"
            Case RadSchedulerStringId.ReminderTwoDays
                Return "2 jours"
            Case RadSchedulerStringId.ReminderThreeDays
                Return "3 jours"
            Case RadSchedulerStringId.ReminderFourDays
                Return "4 jours"
            Case RadSchedulerStringId.ReminderOneWeek
                Return "1 semaine"
            Case RadSchedulerStringId.ReminderTwoWeeks
                Return "2 semaines"
            Case RadSchedulerStringId.Reminder
                Return "Rappel"
            Case RadSchedulerStringId.ContextMenuNewAppointment
                Return "Nouveau RDV"
            Case RadSchedulerStringId.ContextMenuEditAppointment
                Return "Modifier RDV"
            Case RadSchedulerStringId.ContextMenuNewRecurringAppointment
                Return "New RDV Récurrent"
            Case RadSchedulerStringId.ContextMenu60Minutes
                Return "60 Minutes"
            Case RadSchedulerStringId.ContextMenu30Minutes
                Return "30 Minutes"
            Case RadSchedulerStringId.ContextMenu15Minutes
                Return "15 Minutes"
            Case RadSchedulerStringId.ContextMenu10Minutes
                Return "10 Minutes"
            Case RadSchedulerStringId.ContextMenu6Minutes
                Return "6 Minutes"
            Case RadSchedulerStringId.ContextMenu5Minutes
                Return "5 Minutes"
            Case RadSchedulerStringId.ContextMenuNavigateToNextView
                Return "Vue Suivante"
            Case RadSchedulerStringId.ContextMenuNavigateToPreviousView
                Return "Vue Précédente"
            Case RadSchedulerStringId.ContextMenuTimescales
                Return "Echelle Temps"
            Case RadSchedulerStringId.ContextMenuTimescalesYear
                Return "Année"
            Case RadSchedulerStringId.ContextMenuTimescalesMonth
                Return "Mois"
            Case RadSchedulerStringId.ContextMenuTimescalesWeek
                Return "Semaine"
            Case RadSchedulerStringId.ContextMenuTimescalesDay
                Return "Jour"
            Case RadSchedulerStringId.ContextMenuTimescalesHour
                Return "Heure"
            Case RadSchedulerStringId.ContextMenuTimescalesHalfHour
                Return "30 minutes"
            Case RadSchedulerStringId.ContextMenuTimescalesFifteenMinutes
                Return "15 minutes"
            Case RadSchedulerStringId.ErrorProviderWrongAppointmentDates
                Return "L'heure de fin de RDV est inférieure ou égale à l'heure de début !"
            Case RadSchedulerStringId.ErrorProviderWrongExceptionDuration
                Return "L'intervalle de Récurrence doit être supérieur ou égale à la durée du RDV!"
            Case RadSchedulerStringId.ErrorProviderExceptionSameDate
                Return "Deux occurrences d'une même serie ne peuvent pas survenir le même jour."
            Case RadSchedulerStringId.ErrorProviderExceptionSkipOverDate
                Return "L'exception de récurrence ne peut pas ignorer une occurrence ultérieure du même RDV."
            Case RadSchedulerStringId.TimeZoneLocal
                Return "Local"
        End Select
        Return String.Empty
    End Function
End Class
