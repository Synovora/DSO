Imports System.Data.SqlClient
Imports Oasis_Common

Public Class FPPSInitCreation
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateCodeRetour As Boolean
    Private privatePPSSuiviIdeExiste As Boolean
    Private privatePPSSuiviMedecinExiste As Boolean
    Private PrivatePPSSuiviSageFemmeExiste As Boolean

    Public Property SelectedPatient As Patient
        Get
            Return privateSelectedPatient
        End Get
        Set(value As Patient)
            privateSelectedPatient = value
        End Set
    End Property

    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return privateCodeRetour
        End Get
        Set(value As Boolean)
            privateCodeRetour = value
        End Set
    End Property

    Public Property PPSSuiviIdeExiste As Boolean
        Get
            Return privatePPSSuiviIdeExiste
        End Get
        Set(value As Boolean)
            privatePPSSuiviIdeExiste = value
        End Set
    End Property

    Public Property PPSSuiviMedecinExiste As Boolean
        Get
            Return privatePPSSuiviMedecinExiste
        End Get
        Set(value As Boolean)
            privatePPSSuiviMedecinExiste = value
        End Set
    End Property

    Public Property PPSSuiviSageFemmeExiste As Boolean
        Get
            Return PrivatePPSSuiviSageFemmeExiste
        End Get
        Set(value As Boolean)
            PrivatePPSSuiviSageFemmeExiste = value
        End Set
    End Property

    Dim conxn As New SqlConnection(getConnectionString())
    Dim indice As Integer = 10
    Private CategoriePPSId(indice) As Integer
    Private CategoriePPSType(indice) As String

    Private Sub FPPSInitCreation_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementCbxCategorie()
    End Sub

    Private Sub ChargementCbxCategorie()
        Dim CategoriePPSDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim CategoriePPSDataTable As DataTable = New DataTable()

        'Lecture des données en base
        CategoriePPSDataAdapter.SelectCommand = New SqlCommand("select * from oasis.oa_r_pps_categorie where oa_r_pps_categorie_invalide is Null or oa_r_pps_categorie_invalide = 0;", conxn)
        CategoriePPSDataAdapter.Fill(CategoriePPSDataTable)
        conxn.Open()

        Dim i As Integer
        Dim rowCount As Integer = CategoriePPSDataTable.Rows.Count - 1

        'Redimensionnement du tableau selon le nombre d'occurrences lues
        indice = rowCount + 1
        ReDim CategoriePPSId(rowCount + 1)
        ReDim CategoriePPSType(rowCount + 1)

        'Alimentation du tableau
        For i = 0 To rowCount Step 1
            CategoriePPSId(i) = CategoriePPSDataTable.Rows(i)("oa_pps_categorie_id")

            If CategoriePPSDataTable.Rows(i)("oa_pps_categorie_type") Is DBNull.Value Then
                CategoriePPSType(i) = ""
            Else
                CategoriePPSType(i) = CategoriePPSDataTable.Rows(i)("oa_pps_categorie_type")
            End If
        Next

        conxn.Close()
        CategoriePPSDataAdapter.Dispose()
    End Sub

    Private Sub ChargementCbxSousCategorie()

    End Sub

    Private Sub CbxCategorie_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxCategorie.SelectedIndexChanged
        ChargementCbxSousCategorie()
    End Sub

End Class