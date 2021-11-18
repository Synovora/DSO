Imports System.Data.SqlClient

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