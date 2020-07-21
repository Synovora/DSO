Imports System.ComponentModel
Public Class ItemUniteSite
    Implements System.ComponentModel.INotifyPropertyChanged
    Public Event PropertyChanged As PropertyChangedEventHandler Implements System.ComponentModel.INotifyPropertyChanged.PropertyChanged
    Protected Overridable Sub OnPropertyChanged(ByVal propertyName As String)
        RaiseEvent PropertyChanged(Me, New PropertyChangedEventArgs(propertyName))
    End Sub

    Private m_id As Integer
    Private m_parentId As Integer
    Private m_name As String
    Private m_isActive As Boolean
    Private uniteSanitaireOuSite As Object

    Public Sub New(unitesanitaireOusite As Object, ByVal description As String, ByVal isActive As Boolean, ByVal parent_Id As Integer, ByVal id As Integer)
        Me.uniteSanitaireOuSite = unitesanitaireOusite
        Me.m_name = description
        Me.m_isActive = isActive
        Me.m_parentId = parent_Id
        Me.m_id = id
    End Sub

    Public Property Id As Integer
        Get
            Return m_id
        End Get
        Set(ByVal value As Integer)
            If Me.m_id <> value Then
                Me.m_id = value
                OnPropertyChanged("Id")
            End If
        End Set
    End Property
    Public Property ParentId As Integer
        Get
            Return m_parentId
        End Get
        Set(ByVal value As Integer)
            If Me.m_parentId <> value Then
                Me.m_parentId = value
                OnPropertyChanged("ParentId")
            End If
        End Set
    End Property
    Public Property Name As String
        Get
            Return m_name
        End Get
        Set(ByVal value As String)
            If Me.m_name <> value Then
                Me.m_name = value
                OnPropertyChanged("Name")
            End If
        End Set
    End Property
    Public Property IsActive As Boolean
        Get
            Return m_isActive
        End Get
        Set(ByVal value As Boolean)
            If Me.m_isActive <> value Then
                Me.m_isActive = value
                OnPropertyChanged("IsActive")
            End If
        End Set
    End Property

    Public Property UniteSanitaireOuSite1 As Object
        Get
            Return uniteSanitaireOuSite
        End Get
        Set(value As Object)
            uniteSanitaireOuSite = value
        End Set
    End Property
End Class

