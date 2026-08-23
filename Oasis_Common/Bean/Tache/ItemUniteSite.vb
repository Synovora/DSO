Imports System.ComponentModel
Imports Telerik.WinControls.Enumerations

Public Class ItemUniteSite

    Implements System.ComponentModel.INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Protected Overridable Sub OnPropertyChanged(ByVal propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Property Id As Integer
    Property ParentId As Integer
    Property Name As String
    Property IsActive As ToggleState
    Property UniteSanitaireOuSite As Object

    Public Sub New(unitesanitaireOusite As Object, ByVal description As String, ByVal isActive As ToggleState, ByVal parent_Id As Integer, ByVal id As Integer)
        Me.UniteSanitaireOuSite = unitesanitaireOusite
        Me.Name = description
        Me.IsActive = isActive
        Me.ParentId = parent_Id
        Me.Id = id
    End Sub

End Class

