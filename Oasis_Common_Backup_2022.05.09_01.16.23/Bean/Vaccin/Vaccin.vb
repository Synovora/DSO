﻿Imports System.Data.SqlClient

Public Class Vaccin
    Inherits SpecialiteTheriaque

    Property Code As Long
    Property DateImport As DateTime
    Property UtilisateurImport As Long

    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.Code = reader("code")
        Me.CodeAtc = reader("code_atc")
        Me.Dci = reader("dci")
        Me.DciLongue = reader("dci_longue")
        Me.DateImport = reader("date_import")
        Me.UtilisateurImport = reader("utilisateur_import")
    End Sub

End Class

Public Class VaccinValence
    Inherits Vaccin

    Property Valence As Long

    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Valence = reader("valence")
        Me.Id = reader("id")
        Me.Code = reader("code")
        Me.CodeAtc = reader("code_atc")
        Me.Dci = reader("dci")
        Me.DciLongue = reader("dci_longue")
        Me.DateImport = reader("date_import")
        Me.UtilisateurImport = reader("utilisateur_import")
    End Sub

End Class

Public Class VaccinProgramRelation
    Property Id As Long
    Property Patient As Long
    Property [Date] As Long
    Property Vaccin As Long
    Property RelationVaccinValence As Long
    Property RealisationDate As Date
    Property RealisationOperator As Long
    Property RealisationOperatorRor As Long
    Property RealisationOperatorText As String

    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.Patient = reader("patient")
        Me.Date = reader("date")
        Me.Vaccin = reader("vaccin")
        Me.RelationVaccinValence = reader("relation_vaccin_valence")
        Me.RealisationDate = Coalesce(reader("realisation_date"), Nothing)
        Me.RealisationOperator = Coalesce(reader("realisation_operator"), Nothing)
        Me.RealisationOperatorRor = Coalesce(reader("realisation_operator_ror"), Nothing)
        Me.RealisationOperatorText = Coalesce(reader("realisation_operator_text"), Nothing)
    End Sub

End Class