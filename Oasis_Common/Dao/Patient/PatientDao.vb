
Imports System.Data.SqlClient
Imports Oasis_Common

Public Class PatientDao
    Inherits PatientDaoBase

    Public Overrides Function SetPatient(patientId As Integer) As PatientBase
        Dim patient As New PatientBase
        If patientId = 0 Then
            Return Nothing
        End If
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText =
                    "SELECT * FROM oasis.oa_patient where oa_patient_id = @patientId"
            command.Parameters.AddWithValue("@patientId", patientId.ToString)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    patient = BuildBean(reader)
                Else
                    Throw New ArgumentException("Patient inexistante !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return patient
    End Function

    Private Function BuildBean(patientDataReader As SqlDataReader) As PatientBase
        Dim patient As New PatientBase With {
            .patientId = Convert.ToInt64(patientDataReader("oa_patient_id"))
        }

        If patientDataReader("oa_patient_nir") Is DBNull.Value Then
            patient.PatientNir = 0
        Else
            patient.PatientNir = Convert.ToInt64(patientDataReader("oa_patient_nir"))
        End If

        If patientDataReader("oa_patient_nom") Is DBNull.Value Then
            patient.PatientNom = ""
        Else
            patient.PatientNom = patientDataReader("oa_patient_nom")
        End If

        If patientDataReader("oa_patient_prenom") Is DBNull.Value Then
            patient.PatientPrenom = ""
        Else
            patient.PatientPrenom = patientDataReader("oa_patient_prenom")
        End If

        If patientDataReader("oa_patient_date_naissance") Is DBNull.Value Then
            patient.PatientDateNaissance = Nothing
        Else
            patient.PatientDateNaissance = CDate(patientDataReader("oa_patient_date_naissance"))
        End If

        If patientDataReader("oa_patient_genre_id") Is DBNull.Value Then
            patient.PatientGenreId = ""
        Else
            patient.PatientGenreId = patientDataReader("oa_patient_genre_id")
        End If

        If patientDataReader("oa_patient_adresse1") Is DBNull.Value Then
            patient.PatientAdresse1 = ""
        Else
            patient.PatientAdresse1 = patientDataReader("oa_patient_adresse1")
        End If

        If patientDataReader("oa_patient_adresse2") Is DBNull.Value Then
            patient.PatientAdresse2 = ""
        Else
            patient.PatientAdresse2 = patientDataReader("oa_patient_adresse2")
        End If

        If patientDataReader("oa_patient_code_postal") Is DBNull.Value Then
            patient.PatientCodePostal = ""
        Else
            patient.PatientCodePostal = patientDataReader("oa_patient_code_postal")
        End If

        If patientDataReader("oa_patient_ville") Is DBNull.Value Then
            patient.PatientVille = ""
        Else
            patient.PatientVille = patientDataReader("oa_patient_ville")
        End If

        If patientDataReader("oa_patient_tel1") Is DBNull.Value Then
            patient.PatientTel1 = ""
        Else
            patient.PatientTel1 = patientDataReader("oa_patient_tel1")
        End If

        If patientDataReader("oa_patient_tel2") Is DBNull.Value Then
            patient.PatientTel2 = ""
        Else
            patient.PatientTel2 = patientDataReader("oa_patient_tel2")
        End If

        If patientDataReader("oa_patient_email") Is DBNull.Value Then
            patient.PatientEmail = ""
        Else
            patient.PatientEmail = patientDataReader("oa_patient_email")
        End If

        If patientDataReader("oa_patient_nom_marital") Is DBNull.Value Then
            patient.PatientNomMarital = ""
        Else
            patient.PatientNomMarital = patientDataReader("oa_patient_nom_marital")
        End If

        If patientDataReader("oa_patient_date_entree_oasis") Is DBNull.Value Then
            patient.PatientDateEntree = Nothing
        Else
            patient.PatientDateEntree = CDate(patientDataReader("oa_patient_date_entree_oasis"))
        End If

        If patientDataReader("oa_patient_date_sortie_oasis") Is DBNull.Value Then
            patient.PatientDateSortie = Nothing
        Else
            patient.PatientDateSortie = CDate(patientDataReader("oa_patient_date_sortie_oasis"))
        End If

        If patientDataReader("oa_patient_commentaire_sortie") Is DBNull.Value Then
            patient.PatientCommentaireSortie = ""
        Else
            patient.PatientCommentaireSortie = patientDataReader("oa_patient_commentaire_sortie")
        End If

        If patientDataReader("oa_patient_date_deces") Is DBNull.Value Then
            patient.PatientDateDeces = Nothing
        Else
            patient.PatientDateDeces = CDate(patientDataReader("oa_patient_date_deces"))
        End If

        If patientDataReader("oa_patient_site_id") Is DBNull.Value Then
            patient.PatientSiteId = 0
        Else
            patient.PatientSiteId = CInt(patientDataReader("oa_patient_site_id"))
        End If

        If patientDataReader("oa_patient_couverture_internet") Is DBNull.Value Then
            patient.PatientInternet = False
        Else
            patient.PatientInternet = CInt(patientDataReader("oa_patient_couverture_internet"))
        End If

        If patientDataReader("oa_patient_unite_sanitaire_id") Is DBNull.Value Then
            patient.PatientUniteSanitaireId = 0
        Else
            patient.PatientUniteSanitaireId = CInt(patientDataReader("oa_patient_unite_sanitaire_id"))
        End If

        If patientDataReader("oa_patient_synthese_date_maj") Is DBNull.Value Then
            patient.PatientSyntheseDateMaj = Nothing
        Else
            patient.PatientSyntheseDateMaj = CDate(patientDataReader("oa_patient_synthese_date_maj"))
        End If

        If patientDataReader("oa_patient_date_naissance") Is DBNull.Value Then
            patient.PatientAge = "Inconnu"
        Else
            patient.PatientAge = CalculAgeEnAnneeEtMoisString(patient.PatientDateNaissance)
            patient.PatientAgeEnAnnee = CalculAgeEnAnnee(patient.PatientDateNaissance)
        End If
        Dim genre_description As String = Table_genre.GetGenreDescription(patient.PatientGenreId)
        If genre_description = "" Then
            patient.PatientGenre = "genre ?"
        Else
            patient.PatientGenre = genre_description
        End If
        patient.Profession = Coalesce(patientDataReader("oa_patient_profession"), "")
        patient.PharmacienId = Coalesce(patientDataReader("oa_patient_pharmacie_id"), 0)
        patient.Taille = Coalesce(patientDataReader("oa_patient_taille"), 0)
        patient.BlocageMedical = Coalesce(patientDataReader("oa_patient_blocage_medical"), False)
        patient.INS = Coalesce(patientDataReader("oa_patient_INS"), 0)
        Return patient
    End Function

    Public Overrides Function GetPatientById(id As Long) As PatientBase
        Dim patient As PatientBase = SetPatient(id)
        If patient Is Nothing Then
            Throw New ArgumentException("Patient non retrouvé !")
        End If
        Return patient
    End Function

    Public Function Compare(source1 As PatientBase, source2 As PatientBase) As Boolean
        If source1.PatientId <> source2.PatientId Then
            Return False
        End If
        If source1.PatientNir <> source2.PatientNir Then
            Return False
        End If
        If source1.PatientNom <> source2.PatientNom Then
            Return False
        End If
        If source1.PatientPrenom <> source2.PatientPrenom Then
            Return False
        End If
        If source1.PatientNomMarital <> source2.PatientNomMarital Then
            Return False
        End If
        If source1.PatientDateNaissance <> source2.PatientDateNaissance Then
            Return False
        End If
        If source1.PatientGenreId <> source2.PatientGenreId Then
            Return False
        End If
        If source1.PatientAdresse1 <> source2.PatientAdresse1 Then
            Return False
        End If
        If source1.PatientAdresse2 <> source2.PatientAdresse2 Then
            Return False
        End If
        If source1.PatientCodePostal <> source2.PatientCodePostal Then
            Return False
        End If
        If source1.PatientVille <> source2.PatientVille Then
            Return False
        End If
        If source1.PatientTel1 <> source2.PatientTel1 Then
            Return False
        End If
        If source1.PatientTel2 <> source2.PatientTel2 Then
            Return False
        End If
        If source1.PatientEmail <> source2.PatientEmail Then
            Return False
        End If
        If source1.PatientDateEntree <> source2.PatientDateEntree Then
            Return False
        End If
        If source1.PatientDateSortie <> source2.PatientDateSortie Then
            Return False
        End If
        If source1.PatientDateDeces <> source2.PatientDateDeces Then
            Return False
        End If
        If source1.PatientCommentaireSortie <> source2.PatientCommentaireSortie Then
            Return False
        End If
        If source1.PatientInternet <> source2.PatientInternet Then
            Return False
        End If
        If source1.PatientSiteId <> source2.PatientSiteId Then
            Return False
        End If
        If source1.PatientUniteSanitaireId <> source2.PatientUniteSanitaireId Then
            Return False
        End If
        If source1.PatientSyntheseDateMaj <> source2.PatientSyntheseDateMaj Then
            Return False
        End If
        If source1.PatientAge <> source2.PatientAge Then
            Return False
        End If
        If source1.PatientGenre <> source2.PatientGenre Then
            Return False
        End If
        If source1.Profession <> source2.Profession Then
            Return False
        End If
        If source1.PharmacienId <> source2.PharmacienId Then
            Return False
        End If
        If source1.Taille <> source2.Taille Then
            Return False
        End If
        If source1.BlocageMedical <> source2.BlocageMedical Then
            Return False
        End If
        If source1.INS <> source2.INS Then
            Return False
        End If

        Return True
    End Function

    Public Function ClonePatient(Source As PatientBase) As PatientBase
        Dim Cible As New PatientBase With {
            .PatientId = Source.PatientId,
            .PatientNir = Source.PatientNir,
            .PatientNom = Source.PatientNom,
            .PatientPrenom = Source.PatientPrenom,
            .PatientNomMarital = Source.PatientNomMarital,
            .PatientDateNaissance = Source.PatientDateNaissance,
            .PatientGenreId = Source.PatientGenreId,
            .PatientAdresse1 = Source.PatientAdresse1,
            .PatientAdresse2 = Source.PatientAdresse2,
            .PatientCodePostal = Source.PatientCodePostal,
            .PatientVille = Source.PatientVille,
            .PatientTel1 = Source.PatientTel1,
            .PatientTel2 = Source.PatientTel2,
            .PatientEmail = Source.PatientEmail,
            .PatientDateEntree = Source.PatientDateEntree,
            .PatientDateSortie = Source.PatientDateSortie,
            .PatientDateDeces = Source.PatientDateDeces,
            .PatientCommentaireSortie = Source.PatientCommentaireSortie,
            .PatientInternet = Source.PatientInternet,
            .PatientSiteId = Source.PatientSiteId,
            .PatientUniteSanitaireId = Source.PatientUniteSanitaireId,
            .PatientSyntheseDateMaj = Source.PatientSyntheseDateMaj,
            .PatientAge = Source.PatientAge,
            .PatientGenre = Source.PatientGenre,
            .Profession = Source.Profession,
            .PharmacienId = Source.PharmacienId,
            .Taille = Source.Taille,
            .BlocageMedical = Source.BlocageMedical,
            .INS = Source.INS
        }

        Return Cible
    End Function

    Public Function NonExistencePatientNIR(NIR As Int64, PatientId As Integer) As Boolean
        Dim CodeRetour As Boolean = True
        Dim conxn As New SqlConnection(GetConnectionString())
        Dim NirPatientDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim NotePatientDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select * from oasis.oa_patient where oa_patient_nir = " + NIR.ToString + ";"

        Try
            'Lecture des données en base
            NirPatientDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
            NirPatientDataAdapter.Fill(NotePatientDataTable)
            conxn.Open()
            'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
            Dim i As Integer
            Dim rowCount As Integer = NotePatientDataTable.Rows.Count - 1
            'Parcours du DataTable pour alimenter le DataGridView
            For i = 0 To rowCount Step 1
                If PatientId = 0 Then
                    Return CodeRetour = False
                Else
                    If PatientId <> CInt(NotePatientDataTable.Rows(i)("oa_patient_id")) Then
                        Return CodeRetour = False
                    End If
                End If
            Next
        Catch ex As Exception
            Throw New Exception("7" & ex.Message)
            CodeRetour = False
            Throw ex
        Finally
            conxn.Close()
            NirPatientDataAdapter.Dispose()
        End Try

        Return CodeRetour
    End Function


    Public Function ListePatientDateNaissance(DateNaissance As Date) As DataTable
        Dim conxn As New SqlConnection(GetConnectionString())
        Dim ListePatientDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim ListePatientDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select * from oasis.oa_patient where oa_patient_date_naissance = '" + DateNaissance.ToString("yyyy-MM-dd") + "';"

        Try
            'Lecture des données en base
            ListePatientDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
            ListePatientDataAdapter.Fill(ListePatientDataTable)
            conxn.Open()
        Catch ex As Exception
            Throw New Exception("8" & ex.Message)
            Throw ex
        Finally
            conxn.Close()
            ListePatientDataAdapter.Dispose()
        End Try

        Return ListePatientDataTable
    End Function

    Public Function GetAllPatient(Tous As Boolean, PatientOasis As Boolean) As DataTable
        Dim conxn As New SqlConnection(GetConnectionString())
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim dt As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "SELECT oa_patient_id, oa_patient_nir, oa_patient_prenom, oa_patient_nom, oa_patient_date_naissance," &
                    " oa_patient_lieu_naissance, oa_patient_date_entree_oasis, oa_patient_date_sortie_oasis, oa_patient_site_id" &
                    " FROM oasis.oa_patient WHERE 1 = 1"

        Dim FiltrePatientOasis As String =
                    " AND oa_patient_date_entree_oasis Is Not NULL" &
                    " AND oa_patient_date_entree_oasis <> '9998-12-31'" &
                    " AND (oa_patient_date_sortie_oasis Is NULL OR oa_patient_date_sortie_oasis > '" & Date.Now.ToString("yyyy-MM-dd") & "')"

        Dim FiltrePatientNonOasis As String =
                    " AND (oa_patient_date_entree_oasis is NULL OR oa_patient_date_entree_oasis = '9998-12-31')" &
                    " OR oa_patient_date_sortie_oasis <= '" & Date.Now.ToString("yyyy-MM-dd") & "'"

        If Tous = False Then
            If PatientOasis = True Then
                SQLString += FiltrePatientOasis
            Else
                SQLString += FiltrePatientNonOasis
            End If
        End If

        Try
            'Lecture des données en base
            da.SelectCommand = New SqlCommand(SQLString, conxn)
            da.Fill(dt)
            conxn.Open()
        Catch ex As Exception
            Throw New Exception("1" & ex.Message)
            Throw ex
        Finally
            conxn.Close()
            da.Dispose()
        End Try

        Return dt
    End Function

    Public Function ModificationDateMajSynthesePatient(patientId As Integer, userLog As Utilisateur) As Boolean
        'Dim da As MySqlDataAdapter = New MySqlDataAdapter()
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim patient As PatientBase = GetPatientById(patientId)

        Dim SQLstring As String = "UPDATE oasis.oa_patient SET oa_patient_synthese_date_maj = @date WHERE oa_patient_id = @patientId"

        Dim conxn As New SqlConnection(GetConnectionString())
        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@date", Date.Now.ToString("yyyy-MM-dd"))
            .AddWithValue("@patientId", patientId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception("2" & ex.Message)
            Throw ex
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If patient.BlocageMedical = False AndAlso userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString Then
            BlocageMedical(patientId)
        End If

        Return codeRetour
    End Function

    'Modification taille
    Public Function ModificationPatientTaille(patientId As Integer, taille As Integer) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "UPDATE oasis.oa_patient SET oa_patient_taille = @taille WHERE oa_patient_id = @patientId"

        Dim conxn As New SqlConnection(GetConnectionString())
        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@taille", taille)
            .AddWithValue("@patientId", patientId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception("3" & ex.Message)
            codeRetour = False
            Throw ex
        Finally
            conxn.Close()
        End Try
        Return codeRetour
    End Function

    'Blocage médical de la fiche du patient
    Public Shared Function BlocageMedical(patientId As Integer) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "UPDATE oasis.oa_patient SET oa_patient_blocage_medical = @blocageMedical WHERE oa_patient_id = @patientId"

        Dim conxn As New SqlConnection(GetConnectionString())
        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@blocageMedical", True)
            .AddWithValue("@patientId", patientId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception("4" & ex.Message)
            codeRetour = False
            Throw ex
        Finally
            conxn.Close()
        End Try
        Return codeRetour
    End Function

    Public Function GetStringContreIndicationByPatient(patientId As Long) As String
        Dim contreIndicationATCDao As New ContreIndicationATCDao
        Dim contreIndicationSubstanceDao As New ContreIndicationSubstanceDao

        Dim dt As DataTable
        Dim StringContreIndication As String = ""
        Dim PremierPassage As Boolean = True
        Dim rowCount As Integer

        dt = contreIndicationATCDao.getAllContreIndicationATCbyPatient(patientId)
        If dt.Rows.Count > 0 Then
            rowCount = dt.Rows.Count - 1
            For i = 0 To rowCount Step 1
                If PremierPassage = True Then
                    StringContreIndication += "ATC :" & vbCrLf
                    PremierPassage = False
                End If
                StringContreIndication += dt.Rows(i)("code_atc") & " : " & dt.Rows(i)("Denomination_atc") & vbCrLf
            Next
        End If

        dt = contreIndicationSubstanceDao.GetAllContreIndicationSubstancebyPatient(patientId)
        If dt.Rows.Count > 0 Then
            rowCount = dt.Rows.Count - 1
            PremierPassage = True
            For i = 0 To rowCount Step 1
                If PremierPassage = True Then
                    StringContreIndication += "Substance :" & vbCrLf
                    PremierPassage = False
                End If
                Dim substanceId As Integer = dt.Rows(i)("substance_id")
                If substanceId <> 0 Then
                    StringContreIndication += dt.Rows(i)("substance_id") & " : " & dt.Rows(i)("denomination_substance") & vbCrLf
                Else
                    Dim substancePereId As Integer = dt.Rows(i)("substance_pere_id")
                    If substancePereId <> 0 Then
                        StringContreIndication += dt.Rows(i)("substance_pere_id") & " : " & dt.Rows(i)("denomination_substance_pere") & vbCrLf
                    End If
                End If

            Next
        End If

        Return StringContreIndication
    End Function

    Public Function GetStringAllergieByPatient(patientId As Long) As String
        Dim allergieDao As New AllergieDao

        Dim dt As DataTable
        Dim StringContreIndication As String = ""
        Dim PremierPassage As Boolean = True

        dt = allergieDao.GetAllAllergiebyPatient(patientId)
        Dim rowCount As Integer = dt.Rows.Count - 1
        For i = 0 To rowCount Step 1
            If PremierPassage = True Then
                StringContreIndication += "Substance :" & vbCrLf
                PremierPassage = False
            End If
            Dim substanceId As Integer = dt.Rows(i)("substance_id")
            If substanceId <> 0 Then
                StringContreIndication += dt.Rows(i)("substance_id") & " : " & dt.Rows(i)("denomination_substance") & vbCrLf
            Else
                Dim substancePereId As Integer = dt.Rows(i)("substance_pere_id")
                If substancePereId <> 0 Then
                    StringContreIndication += dt.Rows(i)("substance_pere_id") & " : " & dt.Rows(i)("denomination_substance_pere") & vbCrLf
                End If
            End If

        Next

        Return StringContreIndication
    End Function

    Public Function CreationPatient(patient As PatientBase) As Boolean
        'Dim da As MySqlDataAdapter = New MySqlDataAdapter()
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim patientId As Long
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "INSERT INTO oasis.oa_patient" &
        " (oa_patient_nir, oa_patient_nir_modulo, oa_patient_INS, oa_patient_prenom, oa_patient_nom, oa_patient_nom_marital, oa_patient_date_naissance," &
        " oa_patient_genre_id, oa_patient_adresse1, oa_patient_adresse2, oa_patient_code_postal, oa_patient_ville, oa_patient_tel1," &
        " oa_patient_tel2, oa_patient_email, oa_patient_date_entree_oasis, oa_patient_date_sortie_oasis, oa_patient_commentaire_sortie," &
        " oa_patient_date_deces, oa_patient_site_id, oa_patient_unite_sanitaire_id, oa_patient_couverture_internet, oa_patient_profession, oa_patient_pharmacie_id, oa_patient_siege_id)" &
        " VALUES (@nir, @nirModulo, @ins, @prenom, @nom, @nomMarital, @dateNaissance," &
        " @genreId, @adresse1, @adresse2, @codePostal, @ville, @tel1," &
        " @tel2, @email, @dateEntree, @dateSortie, @commentaireSortie, @dateDeces, @siteId, @uniteSanitaireId, @internet, @profession, @pharmacienId, @siegeId) ; SELECT SCOPE_IDENTITY()"

        Dim conxn As New SqlConnection(GetConnectionString())
        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@nir", patient.PatientNir)
            .AddWithValue("@nirModulo", 0)
            .AddWithValue("@ins", patient.INS)
            .AddWithValue("@prenom", patient.PatientPrenom)
            .AddWithValue("@nom", patient.PatientNom)
            .AddWithValue("@nomMarital", patient.PatientNomMarital)
            .AddWithValue("@dateNaissance", patient.PatientDateNaissance.ToString("yyyy-MM-dd"))
            .AddWithValue("@genreId", patient.PatientGenreId.ToString)
            .AddWithValue("@adresse1", patient.PatientAdresse1)
            .AddWithValue("@adresse2", patient.PatientAdresse2)
            .AddWithValue("@codePostal", patient.PatientCodePostal)
            .AddWithValue("@ville", patient.PatientVille)
            .AddWithValue("@tel1", patient.PatientTel1)
            .AddWithValue("@tel2", patient.PatientTel2)
            .AddWithValue("@email", patient.PatientEmail)
            .AddWithValue("@dateEntree", patient.PatientDateEntree.ToString("yyyy-MM-dd"))
            .AddWithValue("@dateSortie", patient.PatientDateSortie.ToString("yyyy-MM-dd"))
            .AddWithValue("@commentaireSortie", patient.PatientCommentaireSortie)
            .AddWithValue("@dateDeces", patient.PatientDateDeces.ToString("yyyy-MM-dd"))
            .AddWithValue("@siteId", patient.PatientSiteId.ToString)
            .AddWithValue("@uniteSanitaireId", patient.PatientUniteSanitaireId.ToString)
            .AddWithValue("@internet", patient.PatientInternet.ToString)
            .AddWithValue("@profession", patient.Profession)
            .AddWithValue("@pharmacienId", patient.PharmacienId.ToString)
            .AddWithValue("@siegeId", "1")
        End With

        Try
            conxn.Open()
            da.InsertCommand = cmd
            patientId = da.InsertCommand.ExecuteScalar()
            Throw New Exception("Patient créé")
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
            Throw ex
        Finally
            conxn.Close()
        End Try

        Dim MaxDate As New Date(9998, 12, 31, 0, 0, 0)
        If patient.PatientDateEntree.Date <> MaxDate.Date Then
            If patientId <> 0 Then
                Dim parcoursDao As New ParcoursDao
                parcoursDao.CreateIntervenantOasisByPatient(patientId)
            End If
        End If

        Return codeRetour
    End Function

    Public Function ModificationPatient(patient As PatientBase) As Boolean
        'Dim da As MySqlDataAdapter = New MySqlDataAdapter()
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "UPDATE oasis.oa_patient SET" &
        " oa_patient_nir = @nir, oa_patient_nir_modulo = @nirModulo, oa_patient_INS = @ins, oa_patient_prenom = @prenom, oa_patient_nom = @nom, oa_patient_nom_marital = @nomMarital," &
        " oa_patient_date_naissance = @dateNaissance, oa_patient_genre_id = @genreId, oa_patient_adresse1 = @adresse1, oa_patient_adresse2 = @adresse2," &
        " oa_patient_code_postal = @codePostal, oa_patient_ville = @ville, oa_patient_tel1 = @tel1, oa_patient_tel2 = @tel2, oa_patient_email = @email," &
        " oa_patient_date_entree_oasis = @dateEntree, oa_patient_date_sortie_oasis = @dateSortie, oa_patient_commentaire_sortie = @commentaireSortie," &
        " oa_patient_date_deces = @dateDeces, oa_patient_site_id = @siteId, oa_patient_unite_sanitaire_id = @uniteSanitaireId," &
        " oa_patient_couverture_internet = @internet, oa_patient_pharmacie_id = @pharmacienId, oa_patient_profession = @profession" &
        " WHERE oa_patient_id = @patientId"

        Dim conxn As New SqlConnection(GetConnectionString())
        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@nir", patient.PatientNir)
            .AddWithValue("@nirModulo", 0)
            .AddWithValue("@ins", patient.INS)
            .AddWithValue("@prenom", patient.PatientPrenom)
            .AddWithValue("@nom", patient.PatientNom)
            .AddWithValue("@nomMarital", patient.PatientNomMarital)
            .AddWithValue("@dateNaissance", patient.PatientDateNaissance.ToString("yyyy-MM-dd"))
            .AddWithValue("@genreId", patient.PatientGenreId)
            .AddWithValue("@adresse1", patient.PatientAdresse1)
            .AddWithValue("@adresse2", patient.PatientAdresse2)
            .AddWithValue("@codePostal", patient.PatientCodePostal)
            .AddWithValue("@ville", patient.PatientVille)
            .AddWithValue("@tel1", patient.PatientTel1)
            .AddWithValue("@tel2", patient.PatientTel2)
            .AddWithValue("@email", patient.PatientEmail)
            .AddWithValue("@dateEntree", patient.PatientDateEntree.ToString("yyyy-MM-dd"))
            .AddWithValue("@dateSortie", patient.PatientDateSortie.ToString("yyyy-MM-dd"))
            .AddWithValue("@commentaireSortie", patient.PatientCommentaireSortie)
            .AddWithValue("@dateDeces", patient.PatientDateDeces.ToString("yyyy-MM-dd"))
            .AddWithValue("@siteId", patient.PatientSiteId)
            .AddWithValue("@uniteSanitaireId", patient.PatientUniteSanitaireId)
            .AddWithValue("@internet", patient.PatientInternet)
            .AddWithValue("@pharmacienId", patient.PharmacienId)
            .AddWithValue("@profession", patient.Profession)
            .AddWithValue("@patientId", patient.PatientId)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception("5" & ex.Message)
            codeRetour = False
            Throw ex
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Contrôle si existence des intervenants Oasis par défaut
            Dim MaxDate As New Date(9998, 12, 31, 0, 0, 0)
            If patient.PatientDateEntree <> MaxDate And patient.PatientDateSortie = MaxDate Then
                Dim parcoursDao As New ParcoursDao
                parcoursDao.CreateIntervenantOasisByPatient(patient.PatientId)
            End If
        End If

        Return codeRetour
    End Function

    Public Shared Function DeclarationSortie(patient As PatientBase) As Boolean
        'Dim da As MySqlDataAdapter = New MySqlDataAdapter()
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "UPDATE oasis.oa_patient" &
            " SET oa_patient_date_sortie_oasis = @dateSortie, oa_patient_commentaire_sortie = @commentaireSortie" &
            " WHERE oa_patient_id = @patientId"

        Dim conxn As New SqlConnection(GetConnectionString())
        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@dateSortie", patient.PatientDateSortie.ToString("yyyy-MM-dd"))
            .AddWithValue("@commentaireSortie", patient.PatientCommentaireSortie)
            .AddWithValue("@patientId", patient.PatientId)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception("6" & ex.Message)
            codeRetour = False
            Throw ex
        Finally
            conxn.Close()
        End Try

        Return codeRetour
    End Function

End Class
