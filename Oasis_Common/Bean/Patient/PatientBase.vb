Imports System.Data.SqlClient
Imports System.Collections.Specialized

Public Class PatientBase
    Property PatientId As Integer
    Property PatientNir As Long
    Property PatientPrenom As String
    Property PatientNom As String
    Property PatientDateNaissance As Date
    Property PatientAge As String
    Property PatientAgeEnAnnee As Integer
    Property PatientGenreId As String
    Property PatientGenre As String
    Property Taille As Integer
    Property PatientAdresse1 As String
    Property PatientAdresse2 As String
    Property PatientCodePostal As String
    Property PatientVille As String
    Property PatientTel1 As String
    Property PatientTel2 As String
    Property PatientSyntheseDateMaj As Date
    Property PatientSiteId As Integer
    Property PatientUniteSanitaireId As Integer
    Property PatientSiegeId As Integer
    Property PatientEmail As String
    Property PatientDateEntree As Date
    Property PatientDateSortie As Date
    Property PatientCommentaireSortie As String
    Property PatientDateDeces As Date
    Property PatientNomMarital As String
    Property PatientInternet As Boolean
    Property Profession As String
    Property PharmacienId As Long
    Property PatientAllergieDci As StringCollection
    Property PatientAllergieCis As StringCollection
    Property PatientAllergiesGénériquesCis As StringCollection
    Property PatientContreIndicationDci As StringCollection
    Property PatientContreIndicationCis As StringCollection
    Property PatientMedicamentsPrescritsCis As StringCollection
    Property BlocageMedical As Boolean
    Property INS As Long

    Sub New()
        InitInstance()
    End Sub

    Private Sub InitInstance()
        Me.PatientId = PatientId
        Me.PatientNir = 0
        Me.PatientNom = ""
        Me.PatientNomMarital = ""
        Me.PatientPrenom = ""
        Me.PatientDateNaissance = Nothing
        Me.PatientGenreId = ""
        Me.PatientAdresse1 = ""
        Me.PatientAdresse2 = ""
        Me.PatientCodePostal = ""
        Me.PatientVille = ""
        Me.PatientTel1 = ""
        Me.PatientTel2 = ""
        Me.PatientEmail = ""
        Me.PatientDateEntree = Nothing
        Me.PatientDateSortie = Nothing
        Me.PatientDateDeces = Nothing
        Me.PatientCommentaireSortie = ""
        Me.PatientSiteId = 0
        Me.PatientUniteSanitaireId = 0
        Me.PatientSyntheseDateMaj = Nothing
        Me.PatientAge = ""
        Me.PatientAgeEnAnnee = 0
        Me.PatientGenre = ""
        Me.PatientInternet = False
        Me.Profession = ""
        Me.PharmacienId = 0
        Me.PatientAllergieDci = New StringCollection()
        Me.PatientAllergieCis = New StringCollection()
        Me.PatientAllergiesGénériquesCis = New StringCollection()
        Me.PatientContreIndicationDci = New StringCollection()
        Me.PatientContreIndicationCis = New StringCollection()
        Me.PatientMedicamentsPrescritsCis = New StringCollection()
        Me.BlocageMedical = False
        Me.INS = 0
    End Sub

    Public Function Clone() As PatientBase
        Dim newInstance As PatientBase = DirectCast(Me.MemberwiseClone(), PatientBase)
        Return newInstance
    End Function

End Class
