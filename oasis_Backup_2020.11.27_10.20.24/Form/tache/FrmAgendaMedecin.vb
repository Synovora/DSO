Imports System.ComponentModel
Imports Oasis_Common
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.Localization

Public Class FrmAgendaMedecin

    Private lstAppointments As New BindingList(Of AppointmentOasis)()

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' localisation french
        SchedulerNavigatorLocalizationProvider.CurrentProvider = New FrenchSchedulerNavigatorLocalizationProvider
        RadSchedulerLocalizationProvider.CurrentProvider = New FrenchSchedulerLocalizationProvider

        ' on met en step d'un quart d'heure la vue jour
        RadScheduler1.GetDayView.RangeFactor = ScaleRange.QuarterHour
        RadScheduler1.GetDayView.RulerStartScale = 7
        RadScheduler1.GetDayView.RulerEndScale = 21

        ' -- ajout initial du handler de chgt de date debut
        AddHandler Me.RadScheduler1.ActiveView.PropertyChanged, AddressOf ActiveView_PropertyChanged

        initCustomBinding()


    End Sub
    Sub ActiveView_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs)
        Console.WriteLine("ActiveView_PropertyChanged => " & e.PropertyName)
        'load the data here
        Select Case e.PropertyName
            Case "StartDate", "DayCount", "EndDate", "Scheduler"
                Dim dateDeb As Date = RadScheduler1.ActiveView.StartDate
                Dim dateFin As Date = RadScheduler1.ActiveView.EndDate
                Console.WriteLine("Retrouvé : " & dateDeb & "  - " & dateFin)
                refreshScheduler(dateDeb, dateFin)
        End Select
    End Sub

    Private Sub RadScheduler1_ActiveViewChanging(sender As Object, e As SchedulerViewChangingEventArgs) Handles RadScheduler1.ActiveViewChanging
        ''Console.WriteLine("ActiveViewChanging : " & e.NewView.StartDate & "  - " & e.NewView.EndDate)

    End Sub

    Private Sub RadScheduler1_BindingContextChanged(sender As Object, e As EventArgs) Handles RadScheduler1.BindingContextChanged
        '' Console.WriteLine("BindingContextChanged : " & e.ToString)

    End Sub

    Private Sub RadScheduler1_ActiveViewChanged(sender As Object, e As SchedulerViewChangedEventArgs) Handles RadScheduler1.ActiveViewChanged
        '   Console.WriteLine("ActiveViewChanged : " & e.NewView.StartDate & "  - " & e.NewView.EndDate)

        ' -- mise en place du handler de startdate
        RemoveHandler Me.RadScheduler1.ActiveView.PropertyChanged, AddressOf ActiveView_PropertyChanged
        AddHandler Me.RadScheduler1.ActiveView.PropertyChanged, AddressOf ActiveView_PropertyChanged

    End Sub

    Private Sub RadScheduler1_AppointmentAdded(sender As Object, e As AppointmentAddedEventArgs) Handles RadScheduler1.AppointmentAdded
        '   Console.WriteLine("AppointmentAdded " & e.Appointment.Start)
    End Sub
    Private Sub RadScheduler1_AppointmentChanged(sender As Object, e As AppointmentChangedEventArgs) Handles RadScheduler1.AppointmentChanged
        Console.WriteLine("AppointmentChanged " & e.Appointment.Start)


    End Sub


    Private Sub initCustomBinding()
        'create a list of CustomAppointment objects
        For i As Integer = 0 To 9
            'add every other appointment, populate with sample data
            If (i Mod 2) = 0 Then
                Dim appointmentNumber As Integer = i + 1
                Dim myAppointment As New AppointmentOasis(DateTime.Now.AddMinutes(appointmentNumber), DateTime.Now.AddMinutes(appointmentNumber + 15), "Appointment " + appointmentNumber.ToString(), "Description for Appointment " + appointmentNumber.ToString(), "Conference room " + appointmentNumber.ToString())
                lstAppointments.Add(myAppointment)
            End If
        Next

        'create_and_configure_a_scheduler_binding_source
        Dim dataSource As New SchedulerBindingDataSource()

        'map the MyAppointment properties to the scheduler
        Dim appointmentMappingInfo As New AppointmentMappingInfo()
        appointmentMappingInfo.Start = "Start"
        appointmentMappingInfo.[End] = "End"
        appointmentMappingInfo.Summary = "Subject"
        appointmentMappingInfo.Description = "Description"
        appointmentMappingInfo.Location = "Location"
        appointmentMappingInfo.UniqueId = "Id"
        appointmentMappingInfo.Exceptions = "Exceptions"

        dataSource.EventProvider.Mapping = appointmentMappingInfo

        'assign the generic List of CustomAppointment as the EventProvider data source
        dataSource.EventProvider.DataSource = lstAppointments
        Me.RadScheduler1.DataSource = dataSource

    End Sub

    Private Sub refreshScheduler(dateDebut As Date, dateFin As Date)

        lstAppointments.Clear()

        Dim appointmentNumber As Integer = 17
        Dim myAppointment As New AppointmentOasis(DateTime.Now.AddHours(appointmentNumber), DateTime.Now.AddHours(appointmentNumber + 1), "Appointment " + appointmentNumber.ToString(), "Description for Appointment " + appointmentNumber.ToString(), "Conference room " + appointmentNumber.ToString())
        lstAppointments.Add(myAppointment)

        lstAppointments.ResetBindings()
    End Sub

End Class
