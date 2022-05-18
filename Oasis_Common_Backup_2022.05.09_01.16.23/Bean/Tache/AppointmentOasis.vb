Imports System.ComponentModel

Public Class AppointmentOasis
    Implements INotifyPropertyChanged

    ' --- variable appointment obligatoires
    Property Start As Date = Date.Now
    Property [End] As Date = Date.Now
    Property Subject As String = String.Empty
    Property Description As String = String.Empty
    Property Location As String = String.Empty
    Property Id As Long = 0
    Property Exceptions As List(Of AppointmentOasis)
    Property BackgroundId As Integer

    ' --- variables spécifiques
    ''Property _typeAppointment As TypeAppointment

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
        Me.BackgroundId = Tache.GetTypeTacheIndex(row("type"))
    End Sub

    Public Sub New(ByVal start As Date, ByVal [end] As Date, ByVal subject As String, ByVal description As String, ByVal location As String)
        Me._start = start
        Me._end = [end]
        Me._subject = subject
        Me._description = description
        Me._location = location
        Dim _exceptions As New List(Of AppointmentOasis)()
    End Sub

    Public Event PropertyChanged As PropertyChangedEventHandler Implements INotifyPropertyChanged.PropertyChanged
    Protected Overridable Sub OnPropertyChanged(ByVal propertyName As String)
        If Me.PropertyChangedEvent IsNot Nothing Then
            RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
        End If
    End Sub

End Class