﻿Public Class Action
    Private _actionId As Long
    Private _utilisateurId As Long
    Private _patientId As Long
    Private _horodatage As DateTime
    Private _action As String

    Public Property ActionId As Long
        Get
            Return _actionId
        End Get
        Set(value As Long)
            _actionId = value
        End Set
    End Property

    Public Property UtilisateurId As Long
        Get
            Return _utilisateurId
        End Get
        Set(value As Long)
            _utilisateurId = value
        End Set
    End Property

    Public Property PatientId As Long
        Get
            Return _patientId
        End Get
        Set(value As Long)
            _patientId = value
        End Set
    End Property

    Public Property Horodatage As Date
        Get
            Return _horodatage
        End Get
        Set(value As Date)
            _horodatage = value
        End Set
    End Property

    Public Property Action As String
        Get
            Return _action
        End Get
        Set(value As String)
            _action = value
        End Set
    End Property
End Class
