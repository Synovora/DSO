
Imports System.Data.SqlClient

Public Class PatientDao
    Inherits StandardDao

    Shared Function BuildBean(reader As SqlDataReader) As Patient
        Dim patient As New Patient With {
            .PatientId = Convert.ToInt64(reader("oa_patient_id")),
            .PatientNir = Coalesce(reader("oa_patient_nir"), 0),
            .PatientNom = Coalesce(reader("oa_patient_nom"), ""),
            .PatientPrenom = Coalesce(reader("oa_patient_prenom"), ""),
            .PatientDateNaissance = Coalesce((reader("oa_patient_date_naissance")), Nothing),
            .PatientGenreId = Coalesce(reader("oa_patient_genre_id"), ""),
            .PatientAdresse1 = Coalesce(reader("oa_patient_adresse1"), ""),
            .PatientAdresse2 = Coalesce(reader("oa_patient_adresse2"), ""),
            .PatientCodePostal = Coalesce(reader("oa_patient_code_postal"), ""),
            .PatientVille = Coalesce(reader("oa_patient_ville"), ""),
            .PatientTel1 = Coalesce(reader("oa_patient_tel1"), ""),
            .PatientTel2 = Coalesce(reader("oa_patient_tel2"), ""),
            .PatientEmail = Coalesce(reader("oa_patient_email"), ""),
            .PatientNomMarital = Coalesce(reader("oa_patient_nom_marital"), ""),
            .PatientDateEntree = Coalesce((reader("oa_patient_date_entree_oasis")), Nothing),
            .PatientDateSortie = Coalesce((reader("oa_patient_date_sortie_oasis")), Nothing),
            .PatientCommentaireSortie = Coalesce(reader("oa_patient_commentaire_sortie"), ""),
            .PatientDateDeces = Coalesce((reader("oa_patient_date_deces")), Nothing),
            .PatientSiteId = Coalesce(reader("oa_patient_site_id"), 0),
            .PatientSiegeId = Coalesce(reader("oa_patient_siege_id"), 0),
            .PatientInternet = Coalesce((reader("oa_patient_couverture_internet")), False),
            .PatientUniteSanitaireId = Coalesce((reader("oa_patient_unite_sanitaire_id")), 0),
            .PatientSyntheseDateMaj = Coalesce((reader("oa_patient_synthese_date_maj")), Nothing),
            .Profession = Coalesce(reader("oa_patient_profession"), ""),
            .PharmacienId = Coalesce(reader("oa_patient_pharmacie_id"), 0),
            .Taille = Coalesce(reader("oa_patient_taille"), 0),
            .BlocageMedical = Coalesce(reader("oa_patient_blocage_medical"), False),
            .INS = Coalesce(reader("oa_patient_INS"), 0)
        }
        patient.PatientGenre = Coalesce(CType(Table_genre.GetGenreDescription(patient.PatientGenreId), String), "genre ?")
        patient.PatientAge = Coalesce(CalculAgeEnAnneeEtMoisString(patient.PatientDateNaissance), "Inconnu")
        patient.PatientAgeEnAnnee = Coalesce(CalculAgeEnAnnee(patient.PatientDateNaissance), "Inconnu")
        Return patient
    End Function

    Public Function GetPatient(patientId As Integer) As Patient
        Dim patient As New Patient
        Dim con As SqlConnection = GetConnection()
        If patientId <> 0 Then
            Try
                Dim command As SqlCommand = con.CreateCommand()
                command.CommandText =
                    "SELECT * FROM oasis.oa_patient where oa_patient_id = @patientId"
                command.Parameters.AddWithValue("@patientId", patientId.ToString)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        patient = BuildBean(reader)
                    Else
                        Throw New ArgumentException("Patient inexistant !")
                    End If
                End Using

            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End If
        Return patient
    End Function

    Public Function Compare(source1 As Patient, source2 As Patient) As Boolean
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

    Public Function ClonePatient(Source As Patient) As Patient
        Dim Cible As New Patient With {
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

    Public Function GetPatientByNIR(NIR As String) As Patient
        Dim patient As New Patient
        Dim con As SqlConnection = GetConnection()

        If NIR Then
            Try
                Dim command As SqlCommand = con.CreateCommand()
                command.CommandText =
                    "SELECT * FROM oasis.oa_patient WHERE oa_patient_nir = @patientNIR"
                command.Parameters.AddWithValue("@patientNIR", NIR)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        patient = BuildBean(reader)
                    Else
                        Throw New ArgumentException("Patient inexistant !")
                    End If
                End Using

            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End If
        Return patient
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

    Public Function GetFilteredPatient(Prenom As String, Nom As String, NDD As Date) As List(Of Patient)
        Dim con As SqlConnection = GetConnection()
        Dim patients As List(Of Patient) = New List(Of Patient)
        Dim SQLString As String = "SELECT * FROM oasis.oa_patient WHERE 1 = 1"

        If Prenom <> Nothing Then
            SQLString += "AND oa_patient_prenom LIKE '" & Prenom & "%'"
        End If
        If Nom <> Nothing Then
            SQLString += "AND oa_patient_nom LIKE '" & Nom & "%'"
        End If
        If NDD <> Nothing Then
            SQLString += "AND oa_patient_date_naissance LIKE '" & NDD & "'"
        End If

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = SQLString
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    patients.Add(BuildBean(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return patients
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

        Dim patient As Patient = GetPatient(patientId)

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

    Public Sub CreationPatient(patient As Patient, userLog As Utilisateur)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim patientId As Long
        Dim parcoursDao As New ParcoursDao
        Dim MaxDate As New Date(9998, 12, 31, 0, 0, 0)
        Dim con As SqlConnection = GetConnection()
        Dim SQLstring As String = "INSERT INTO oasis.oa_patient" &
        " (oa_patient_nir, oa_patient_nir_modulo, oa_patient_INS, oa_patient_prenom, oa_patient_nom, oa_patient_nom_marital, oa_patient_date_naissance," &
        " oa_patient_genre_id, oa_patient_adresse1, oa_patient_adresse2, oa_patient_code_postal, oa_patient_ville, oa_patient_tel1," &
        " oa_patient_tel2, oa_patient_email, oa_patient_date_entree_oasis, oa_patient_date_sortie_oasis, oa_patient_commentaire_sortie," &
        " oa_patient_date_deces, oa_patient_site_id, oa_patient_unite_sanitaire_id, oa_patient_couverture_internet, oa_patient_profession, oa_patient_pharmacie_id, oa_patient_siege_id)" &
        " VALUES (@nir, @nirModulo, @ins, @prenom, @nom, @nomMarital, @dateNaissance," &
        " @genreId, @adresse1, @adresse2, @codePostal, @ville, @tel1," &
        " @tel2, @email, @dateEntree, @dateSortie, @commentaireSortie, @dateDeces, @siteId, @uniteSanitaireId, @internet, @profession, @pharmacienId, @siegeId) ; SELECT SCOPE_IDENTITY()"

        Dim cmd As New SqlCommand(SQLstring, con)
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
            .AddWithValue("@siegeId", userLog.UtilisateurSiegeId)
        End With
        Try
            da.InsertCommand = cmd
            patientId = da.InsertCommand.ExecuteScalar()
            If patient.PatientDateEntree.Date <> MaxDate.Date Then
                parcoursDao.CreateIntervenantOasisByPatient(patientId, userLog)
            End If
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub

    Public Function ModificationPatient(patient As Patient, userLog As Utilisateur) As Boolean
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
                parcoursDao.CreateIntervenantOasisByPatient(patient.PatientId, userLog)
            End If
        End If

        Return codeRetour
    End Function

    Public Shared Function DeclarationSortie(patient As Patient) As Boolean
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
