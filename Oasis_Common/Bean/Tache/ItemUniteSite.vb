Imports System.ComponentModel

Public Class ItemUniteSite

    Implements System.ComponentModel.INotifyPropertyChanged

    Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged

    Protected Overridable Sub OnPropertyChanged(ByVal propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Property Id As Integer
    Property ParentId As Integer
    Property Name As String
    Property IsActive As Boolean
    Property UniteSanitaireOuSite As Object

    Public Sub New(unitesanitaireOusite As Object, ByVal description As String, ByVal isActive As Boolean, ByVal parent_Id As Integer, ByVal id As Integer)
        Me.UniteSanitaireOuSite = unitesanitaireOusite
        Me.Name = description
        Me.IsActive = isActive
        Me.ParentId = parent_Id
        Me.Id = id
    End Sub

    Public Property UniteSanitaireOuSite1 As Object
        Get
            Return UniteSanitaireOuSite
        End Get
        Set(value As Object)
            UniteSanitaireOuSite = value
        End Set
    End Property

End Class

