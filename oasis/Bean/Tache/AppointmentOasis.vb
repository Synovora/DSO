Imports System.ComponentModel

Public Class AppointmentOasis
    Implements INotifyPropertyChanged

    ' --- variable appointment obligatoires
    Private _start As Date = Date.Now
    Private _end As Date = Date.Now
    Private _subject As String = String.Empty
    Private _description As String = String.Empty
    Private _location As String = String.Empty
    Private _id As Long = 0
    Private _exceptions As List(Of AppointmentOasis)
    Private _backgroundId As Integer

    ' --- variables spécifiques
    ''Private _typeAppointment As TypeAppointment

    Public Sub New()
    End Sub

    Public Sub New(row As DataRow)
        Me.Id = row("id")
        Me.Start = row("date_rendez_vous")
        Me.End = row("date_rendez_vous")
        Dim duree As Integer = Coalesce(row("duree_mn"), 0)
        Me.End = Me.End.AddMinutes(If(duree < 15, 15, duree))
        Me.Subject = row("type")
        Me.Description = Coalesce(row("patient_prenom"), "") + " " + Coalesce(row("patient_nom"), "")
        Me.Location = Coalesce(row("site_description"), "")
        Me.BackgroundId = TacheDao.getTypeTacheIndex(row("type"))
    End Sub

    Public Sub New(ByVal start As Date, ByVal [end] As Date, ByVal subject As String, ByVal description As String, ByVal location As String)
        Me._start = start
        Me._end = [end]
        Me._subject = subject
        Me._description = description
        Me._location = location
        Dim _exceptions As New List(Of AppointmentOasis)()
    End Sub

    Public Property Exceptions() As List(Of AppointmentOasis)
        Get
            Return Me._exceptions
        End Get
        Set(ByVal value As List(Of AppointmentOasis))
            If Me._exceptions IsNot value Then
                Me._exceptions = value
                Me.OnPropertyChanged("Exceptions")
            End If
        End Set
    End Property
    Public Property Id() As Long
        Get
            Return Me._id
        End Get
        Set(ByVal value As Long)
            If Me._id <> value Then
                Me._id = value
                Me.OnPropertyChanged("Id")
            End If
        End Set
    End Property
    Public Property Start() As Date
        Get
            Return Me._start
        End Get
        Set(ByVal value As Date)
            If Me._start <> value Then
                Me._start = value
                Me.OnPropertyChanged("Start")
            End If
        End Set
    End Property
    Public Property [End]() As Date
        Get
            Return Me._end
        End Get
        Set(ByVal value As Date)
            If Me._end <> value Then
                Me._end = value
                Me.OnPropertyChanged("End")
            End If
        End Set
    End Property
    Public Property Subject() As String
        Get
            Return Me._subject
        End Get
        Set(ByVal value As String)
            If Me._subject <> value Then
                Me._subject = value
                Me.OnPropertyChanged("Subject")
            End If
        End Set
    End Property
    Public Property Description() As String
        Get
            Return Me._description
        End Get
        Set(ByVal value As String)
            If Me._description <> value Then
                Me._description = value
                Me.OnPropertyChanged("Description")
            End If
        End Set
    End Property
    Public Property Location() As String
        Get
            Return Me._location
        End Get
        Set(ByVal value As String)
            If Me._location <> value Then
                Me._location = value
                Me.OnPropertyChanged("Location")
            End If
        End Set
    End Property

    Public Property BackgroundId As Integer
        Get
            Return _backgroundId
        End Get
        Set(value As Integer)
            _backgroundId = value
        End Set
    End Property

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Overridable Sub OnPropertyChanged(ByVal propertyName As String)
        If Me.PropertyChangedEvent IsNot Nothing Then
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End If
    End Sub
End Class