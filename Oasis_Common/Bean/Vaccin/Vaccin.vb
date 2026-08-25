Imports System.Data.SqlClient

Public Class Vaccin
    Inherits SpecialiteTheriaque

    Property Code As Long
    Property DateImport As DateTime
    Property UtilisateurImport As Long

    Public Sub New()
    End Sub

    Public Sub New(record As System.Data.IDataRecord)
        Me.Id = record("id")
        Me.Code = record("code")
        Me.CodeAtc = record("code_atc")
        Me.Dci = record("dci")
        Me.DciLongue = record("dci_longue")
        Me.DateImport = record("date_import")
        Me.UtilisateurImport = record("utilisateur_import")
    End Sub

End Class

Public Class VaccinValence
    Inherits Vaccin

    Property Valence As Long

    Public Sub New()
    End Sub

    Public Sub New(record As System.Data.IDataRecord)
        Me.Valence = record("valence")
        Me.Id = record("id")
        Me.Code = record("code")
        Me.CodeAtc = record("code_atc")
        Me.Dci = record("dci")
        Me.DciLongue = record("dci_longue")
        Me.DateImport = record("date_import")
        Me.UtilisateurImport = record("utilisateur_import")
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

    Public Sub New(record As System.Data.IDataRecord)
        Me.Id = record("id")
        Me.Patient = record("patient")
        Me.Date = record("date")
        Me.Vaccin = record("vaccin")
        Me.RelationVaccinValence = record("relation_vaccin_valence")
        Me.RealisationDate = Coalesce(record("realisation_date"), Nothing)
        Me.RealisationOperator = Coalesce(record("realisation_operator"), Nothing)
        Me.RealisationOperatorRor = Coalesce(record("realisation_operator_ror"), Nothing)
        Me.RealisationOperatorText = Coalesce(record("realisation_operator_text"), Nothing)
    End Sub

End Class