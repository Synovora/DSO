Imports System.Data.SqlClient
Imports System.Collections.Specialized
Imports Oasis_Common

Public Class Patient

    Private privatePatientId As Integer
    Private privatePatientNir As Long
    Private privatePatientPrenom As String
    Private privatePatientNom As String
    Private privatePatientDateNaissance As Date
    Private privatePatientAge As String
    Private privatePatientAgeEnAnnee As Integer
    Private privatePatientGenreId As String
    Private privatePatientGenre As String
    Private privatePatientAdresse1 As String
    Private privatePatientAdresse2 As String
    Private privatePatientCodePostal As String
    Private privatePatientVille As String
    Private privatePatientTel1 As String
    Private privatePatientTel2 As String
    Private privatePatientSyntheseDateMaj As Date
    Private privatePatientSiteId As Integer
    Private privatePatientUniteSanitaireId As Integer
    Private privatePatientSiegeId As Integer
    Private privatePatientEmail As String
    Private privatePatientDateEntree As Date
    Private privatePatientDateSortie As Date
    Private privatePatientCommentaireSortie As String
    Private privatePatientDateDeces As Date
    Private privatePatientNomMarital As String
    Private privatePatientInternet As Boolean
    Private _profession As String
    Private _PharmacienId As Long
    Private privatePatientAllergieDci As StringCollection
    Private privatePatientAllergieCis As StringCollection
    Private privatePatientAllergiesGénériquesCis As StringCollection
    Private privatePatientContreIndicationDci As StringCollection
    Private privatePatientContreIndicationCis As StringCollection
    Private privatePatientMedicamentsPrescritsCis As StringCollection

    Sub New()
        InitInstance()
    End Sub

    Sub New(patientId As Integer)
        If patientId <> 0 Then
            Dim conxn As New SqlConnection(getConnectionString())
            Dim SQLString As String
            Dim patientDataReader As SqlDataReader
            SQLString = "select * from oasis.oa_patient where oa_patient_id = " & patientId & ";"
            Dim myCommand As New SqlCommand(SQLString, conxn)

            conxn.Open()
            patientDataReader = myCommand.ExecuteReader()
            If patientDataReader.Read() Then
                Me.patientId = patientId

                If patientDataReader("oa_patient_nir") Is DBNull.Value Then
                    Me.PatientNir = 0
                Else
                    Me.PatientNir = Convert.ToInt64(patientDataReader("oa_patient_nir"))
                End If

                If patientDataReader("oa_patient_nom") Is DBNull.Value Then
                    Me.PatientNom = ""
                Else
                    Me.PatientNom = patientDataReader("oa_patient_nom")
                End If

                If patientDataReader("oa_patient_prenom") Is DBNull.Value Then
                    Me.PatientPrenom = ""
                Else
                    Me.PatientPrenom = patientDataReader("oa_patient_prenom")
                End If

                If patientDataReader("oa_patient_date_naissance") Is DBNull.Value Then
                    Me.PatientDateNaissance = Nothing
                Else
                    Me.PatientDateNaissance = CDate(patientDataReader("oa_patient_date_naissance"))
                End If

                If patientDataReader("oa_patient_genre_id") Is DBNull.Value Then
                    Me.PatientGenreId = ""
                Else
                    Me.PatientGenreId = patientDataReader("oa_patient_genre_id")
                End If

                If patientDataReader("oa_patient_adresse1") Is DBNull.Value Then
                    Me.PatientAdresse1 = ""
                Else
                    Me.PatientAdresse1 = patientDataReader("oa_patient_adresse1")
                End If

                If patientDataReader("oa_patient_adresse2") Is DBNull.Value Then
                    Me.PatientAdresse2 = ""
                Else
                    Me.PatientAdresse2 = patientDataReader("oa_patient_adresse2")
                End If

                If patientDataReader("oa_patient_code_postal") Is DBNull.Value Then
                    Me.PatientCodePostal = ""
                Else
                    Me.PatientCodePostal = patientDataReader("oa_patient_code_postal")
                End If

                If patientDataReader("oa_patient_ville") Is DBNull.Value Then
                    Me.PatientVille = ""
                Else
                    Me.PatientVille = patientDataReader("oa_patient_ville")
                End If

                If patientDataReader("oa_patient_tel1") Is DBNull.Value Then
                    Me.PatientTel1 = ""
                Else
                    Me.PatientTel1 = patientDataReader("oa_patient_tel1")
                End If

                If patientDataReader("oa_patient_tel2") Is DBNull.Value Then
                    Me.PatientTel2 = ""
                Else
                    Me.PatientTel2 = patientDataReader("oa_patient_tel2")
                End If

                If patientDataReader("oa_patient_email") Is DBNull.Value Then
                    Me.PatientEmail = ""
                Else
                    Me.PatientEmail = patientDataReader("oa_patient_email")
                End If

                If patientDataReader("oa_patient_nom_marital") Is DBNull.Value Then
                    Me.PatientNomMarital = ""
                Else
                    Me.PatientNomMarital = patientDataReader("oa_patient_nom_marital")
                End If

                If patientDataReader("oa_patient_date_entree_oasis") Is DBNull.Value Then
                    Me.PatientDateEntree = Nothing
                Else
                    Me.PatientDateEntree = CDate(patientDataReader("oa_patient_date_entree_oasis"))
                End If

                If patientDataReader("oa_patient_date_sortie_oasis") Is DBNull.Value Then
                    Me.PatientDateSortie = Nothing
                Else
                    Me.PatientDateSortie = CDate(patientDataReader("oa_patient_date_sortie_oasis"))
                End If

                If patientDataReader("oa_patient_commentaire_sortie") Is DBNull.Value Then
                    Me.PatientCommentaireSortie = ""
                Else
                    Me.PatientCommentaireSortie = patientDataReader("oa_patient_commentaire_sortie")
                End If

                If patientDataReader("oa_patient_date_deces") Is DBNull.Value Then
                    Me.PatientDateDeces = Nothing
                Else
                    Me.PatientDateDeces = CDate(patientDataReader("oa_patient_date_deces"))
                End If

                If patientDataReader("oa_patient_couverture_internet") Is DBNull.Value Then
                    Me.PatientInternet = False
                Else
                    If patientDataReader("oa_patient_couverture_internet") = "1" Then
                        Me.PatientInternet = True
                    Else
                        Me.PatientInternet = False
                    End If
                End If

                If patientDataReader("oa_patient_site_id") Is DBNull.Value Then
                    Me.PatientSiteId = 0
                Else
                    Me.PatientSiteId = CInt(patientDataReader("oa_patient_site_id"))
                End If

                If patientDataReader("oa_patient_unite_sanitaire_id") Is DBNull.Value Then
                    Me.PatientUniteSanitaireId = 0
                Else
                    Me.PatientUniteSanitaireId = CInt(patientDataReader("oa_patient_unite_sanitaire_id"))
                End If

                If patientDataReader("oa_patient_synthese_date_maj") Is DBNull.Value Then
                    Me.PatientSyntheseDateMaj = Nothing
                Else
                    Me.PatientSyntheseDateMaj = CDate(patientDataReader("oa_patient_synthese_date_maj"))
                End If

                If patientDataReader("oa_patient_date_naissance") Is DBNull.Value Then
                    Me.PatientAge = "Inconnu"
                    Me.PatientAgeEnAnnee = 0
                Else
                    Dim lMois As Integer
                    Dim PatientMoisRestant, PatientAn As Integer
                    lMois = CalculAgeEnmois(Me.PatientDateNaissance)
                    Me.PatientAgeEnAnnee = lMois
                    If lMois > 36 Then
                        PatientMoisRestant = lMois Mod 12
                        lMois = lMois - PatientMoisRestant
                        PatientAn = lMois / 12
                    End If
                    Select Case lMois
                        Case 0 To 35
                            Me.PatientAge = "(" & lMois & " mois)"
                        Case 36 To 119
                            Me.PatientAge = "(" & PatientAn & " ans " & PatientMoisRestant & " mois)"
                        Case Else
                            Me.PatientAge = "(" & PatientAn.ToString & " ans)"
                    End Select
                End If

                Dim genre_description As String = table_genre.GetGenreDescription(Me.PatientGenreId)
                If genre_description = "" Then
                    Me.PatientGenre = "genre ?"
                Else
                    Me.PatientGenre = genre_description
                End If

                Me.Profession = Coalesce(patientDataReader("oa_patient_profession"), "")
                Me.PharmacienId = Coalesce(patientDataReader("oa_patient_pharmacie_id"), 0)

            Else
                MessageBox.Show("Erreur de lecture du patient")
            End If

            conxn.Close()
            myCommand.Dispose()
        Else
            InitInstance()
        End If
    End Sub

    Private Sub InitInstance()
        Me.patientId = patientId
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
        Me.privatePatientAllergiesGénériquesCis = New StringCollection()
        Me.PatientContreIndicationDci = New StringCollection()
        Me.PatientContreIndicationCis = New StringCollection()
        Me.PatientMedicamentsPrescritsCis = New StringCollection()
    End Sub

    Public Property patientId As Integer
        Get
            Return privatePatientId
        End Get
        Set(value As Integer)
            privatePatientId = value
        End Set
    End Property

    Public Property PatientNir As Long
        Get
            Return privatePatientNir
        End Get
        Set(value As Long)
            privatePatientNir = value
        End Set
    End Property

    Public Property PatientPrenom As String
        Get
            Return privatePatientPrenom
        End Get
        Set(value As String)
            privatePatientPrenom = value
        End Set
    End Property

    Public Property PatientNom As String
        Get
            Return privatePatientNom
        End Get
        Set(value As String)
            privatePatientNom = value
        End Set
    End Property

    Public Property PatientDateNaissance As Date
        Get
            Return privatePatientDateNaissance
        End Get
        Set(value As Date)
            privatePatientDateNaissance = value
        End Set
    End Property

    Public Property PatientAge As String
        Get
            Return privatePatientAge
        End Get
        Set(value As String)
            privatePatientAge = value
        End Set
    End Property

    Public Property PatientGenreId As String
        Get
            Return privatePatientGenreId
        End Get
        Set(value As String)
            privatePatientGenreId = value
        End Set
    End Property

    Public Property PatientGenre As String
        Get
            Return privatePatientGenre
        End Get
        Set(value As String)
            privatePatientGenre = value
        End Set
    End Property

    Public Property PatientAdresse1 As String
        Get
            Return privatePatientAdresse1
        End Get
        Set(value As String)
            privatePatientAdresse1 = value
        End Set
    End Property

    Public Property PatientAdresse2 As String
        Get
            Return privatePatientAdresse2
        End Get
        Set(value As String)
            privatePatientAdresse2 = value
        End Set
    End Property

    Public Property PatientCodePostal As String
        Get
            Return privatePatientCodePostal
        End Get
        Set(value As String)
            privatePatientCodePostal = value
        End Set
    End Property

    Public Property PatientVille As String
        Get
            Return privatePatientVille
        End Get
        Set(value As String)
            privatePatientVille = value
        End Set
    End Property

    Public Property PatientTel1 As String
        Get
            Return privatePatientTel1
        End Get
        Set(value As String)
            privatePatientTel1 = value
        End Set
    End Property

    Public Property PatientTel2 As String
        Get
            Return privatePatientTel2
        End Get
        Set(value As String)
            privatePatientTel2 = value
        End Set
    End Property

    Public Property PatientSyntheseDateMaj As Date
        Get
            Return privatePatientSyntheseDateMaj
        End Get
        Set(value As Date)
            privatePatientSyntheseDateMaj = value
        End Set
    End Property

    Public Property PatientSiteId As Integer
        Get
            Return privatePatientSiteId
        End Get
        Set(value As Integer)
            privatePatientSiteId = value
        End Set
    End Property

    Public Property PatientUniteSanitaireId As Integer
        Get
            Return privatePatientUniteSanitaireId
        End Get
        Set(value As Integer)
            privatePatientUniteSanitaireId = value
        End Set
    End Property

    Public Property PatientAllergieDci As StringCollection
        Get
            Return privatePatientAllergieDci
        End Get
        Set(value As StringCollection)
            privatePatientAllergieDci = value
        End Set
    End Property

    Public Property PatientAllergieCis As StringCollection
        Get
            Return privatePatientAllergieCis
        End Get
        Set(value As StringCollection)
            privatePatientAllergieCis = value
        End Set
    End Property

    Public Property PatientContreIndicationDci As StringCollection
        Get
            Return privatePatientContreIndicationDci
        End Get
        Set(value As StringCollection)
            privatePatientContreIndicationDci = value
        End Set
    End Property

    Public Property PatientContreIndicationCis As StringCollection
        Get
            Return privatePatientContreIndicationCis
        End Get
        Set(value As StringCollection)
            privatePatientContreIndicationCis = value
        End Set
    End Property

    Public Property PatientMedicamentsPrescritsCis As StringCollection
        Get
            Return privatePatientMedicamentsPrescritsCis
        End Get
        Set(value As StringCollection)
            privatePatientMedicamentsPrescritsCis = value
        End Set
    End Property

    Public Property PatientAllergiesGénériquesCis As StringCollection
        Get
            Return privatePatientAllergiesGénériquesCis
        End Get
        Set(value As StringCollection)
            privatePatientAllergiesGénériquesCis = value
        End Set
    End Property

    Public Property PatientAgeEnAnnee As Integer
        Get
            Return privatePatientAgeEnAnnee
        End Get
        Set(value As Integer)
            privatePatientAgeEnAnnee = value
        End Set
    End Property

    Public Property PatientSiegeId As Integer
        Get
            Return privatePatientSiegeId
        End Get
        Set(value As Integer)
            privatePatientSiegeId = value
        End Set
    End Property

    Public Property PatientEmail As String
        Get
            Return privatePatientEmail
        End Get
        Set(value As String)
            privatePatientEmail = value
        End Set
    End Property

    Public Property PatientDateEntree As Date
        Get
            Return privatePatientDateEntree
        End Get
        Set(value As Date)
            privatePatientDateEntree = value
        End Set
    End Property

    Public Property PatientDateSortie As Date
        Get
            Return privatePatientDateSortie
        End Get
        Set(value As Date)
            privatePatientDateSortie = value
        End Set
    End Property

    Public Property PatientCommentaireSortie As String
        Get
            Return privatePatientCommentaireSortie
        End Get
        Set(value As String)
            privatePatientCommentaireSortie = value
        End Set
    End Property

    Public Property PatientDateDeces As Date
        Get
            Return privatePatientDateDeces
        End Get
        Set(value As Date)
            privatePatientDateDeces = value
        End Set
    End Property

    Public Property PatientNomMarital As String
        Get
            Return privatePatientNomMarital
        End Get
        Set(value As String)
            privatePatientNomMarital = value
        End Set
    End Property

    Public Property PatientInternet As Boolean
        Get
            Return privatePatientInternet
        End Get
        Set(value As Boolean)
            privatePatientInternet = value
        End Set
    End Property

    Public Property Profession As String
        Get
            Return _profession
        End Get
        Set(value As String)
            _profession = value
        End Set
    End Property

    Public Property PharmacienId As Long
        Get
            Return _PharmacienId
        End Get
        Set(value As Long)
            _PharmacienId = value
        End Set
    End Property

    Public Function Clone() As Patient
        Dim newInstance As Patient = DirectCast(Me.MemberwiseClone(), Patient)
        Return newInstance
    End Function

End Class
