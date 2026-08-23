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

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.Code = reader("code")
        Me.Description = reader("description")
        Me.Precaution = reader("precaution")
        Me.DateCreation = reader("date_creation")
        Me.DateModification = reader("date_modification")
        Me.UtilisateurCreation = reader("utilisateur_creation")
        Me.UtilisateurModification = reader("utilisateur_modification")
        Me.Actif = reader("actif")
        Me.Visible = reader("visible")
        Me.Ordre = reader("ordre")
    End Sub

End Class

Public Class RelationVaccinValence

    Property Id As Long
    Property Vaccin As Long
    Property Valence As Long

    Public Sub New()
    End Sub

    Public Sub New(reader As SqlDataReader)
        Me.Id = reader("id")
        Me.Vaccin = reader("vaccin")
        Me.Valence = reader("valence")
    End Sub

End Class

