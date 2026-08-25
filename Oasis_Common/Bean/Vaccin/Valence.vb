Imports System.Data.SqlClient

Public Class Valence

    Property Id As Long
    Property Code As String
    Property Description As String
    Property Precaution As String
    Property DateCreation As DateTime
    Property DateModification As DateTime
    Property UtilisateurCreation As Long
    Property UtilisateurModification As Long
    Property Actif As Boolean
    Property Visible As Boolean
    Property Ordre As Integer

    Public Sub New()
    End Sub

    Public Sub New(record As System.Data.IDataRecord)
        Me.Id = record("id")
        Me.Code = record("code")
        Me.Description = record("description")
        Me.Precaution = record("precaution")
        Me.DateCreation = record("date_creation")
        Me.DateModification = record("date_modification")
        Me.UtilisateurCreation = record("utilisateur_creation")
        Me.UtilisateurModification = record("utilisateur_modification")
        Me.Actif = record("actif")
        Me.Visible = record("visible")
        Me.Ordre = record("ordre")
    End Sub

End Class

Public Class RelationVaccinValence

    Property Id As Long
    Property Vaccin As Long
    Property Valence As Long

    Public Sub New()
    End Sub

    Public Sub New(record As System.Data.IDataRecord)
        Me.Id = record("id")
        Me.Vaccin = record("vaccin")
        Me.Valence = record("valence")
    End Sub

End Class

