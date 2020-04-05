
Imports System.Data.SqlClient
Imports Oasis_Common

Module PatientDao
    Public Structure EnumGenreId
        Const Feminin = "F"
        Const Masculin = "M"
    End Structure

    'Initialisation des propriétés d'une instance de Patient depuis la BDD
    Public Function SetPatient(instancePatient As Patient, patientId As Integer) As Boolean
        Dim CodeRetour As Boolean = True
        If patientId <> 0 Then
            Dim conxn As New SqlConnection(getConnectionString())
            Dim SQLString As String = "select * from oasis.oa_patient where oa_patient_id = @patientId"
            Dim patientDataReader As SqlDataReader
            Dim cmd As New SqlCommand(SQLString, conxn)
            cmd.Parameters.AddWithValue("@patientId", patientId.ToString)

            Try
                conxn.Open()
                patientDataReader = cmd.ExecuteReader()
                setPatientProperties(instancePatient, patientDataReader)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                CodeRetour = False
            Finally
                conxn.Close()
                cmd.Dispose()
            End Try
        Else
            InitPatientProperties(instancePatient)
        End If

        Return CodeRetour
    End Function

    Public Function getPatientById(id As Long)
        Dim patient As Patient = New Patient
        If SetPatient(patient, id) = False Then
            Throw New ArgumentException("Patient non retrouvé !")
        End If
        Return patient
    End Function

    Private Sub setPatientProperties(instancePatient As Patient, patientDataReader As SqlDataReader)
        If patientDataReader.Read() Then
            instancePatient.patientId = Convert.ToInt64(patientDataReader("oa_patient_id"))

            If patientDataReader("oa_patient_nir") Is DBNull.Value Then
                instancePatient.PatientNir = 0
            Else
                instancePatient.PatientNir = Convert.ToInt64(patientDataReader("oa_patient_nir"))
            End If

            If patientDataReader("oa_patient_nom") Is DBNull.Value Then
                instancePatient.PatientNom = ""
            Else
                instancePatient.PatientNom = patientDataReader("oa_patient_nom")
            End If

            If patientDataReader("oa_patient_prenom") Is DBNull.Value Then
                instancePatient.PatientPrenom = ""
            Else
                instancePatient.PatientPrenom = patientDataReader("oa_patient_prenom")
            End If

            If patientDataReader("oa_patient_date_naissance") Is DBNull.Value Then
                instancePatient.PatientDateNaissance = Nothing
            Else
                instancePatient.PatientDateNaissance = CDate(patientDataReader("oa_patient_date_naissance"))
            End If

            If patientDataReader("oa_patient_genre_id") Is DBNull.Value Then
                instancePatient.PatientGenreId = ""
            Else
                instancePatient.PatientGenreId = patientDataReader("oa_patient_genre_id")
            End If

            If patientDataReader("oa_patient_adresse1") Is DBNull.Value Then
                instancePatient.PatientAdresse1 = ""
            Else
                instancePatient.PatientAdresse1 = patientDataReader("oa_patient_adresse1")
            End If

            If patientDataReader("oa_patient_adresse2") Is DBNull.Value Then
                instancePatient.PatientAdresse2 = ""
            Else
                instancePatient.PatientAdresse2 = patientDataReader("oa_patient_adresse2")
            End If

            If patientDataReader("oa_patient_code_postal") Is DBNull.Value Then
                instancePatient.PatientCodePostal = ""
            Else
                instancePatient.PatientCodePostal = patientDataReader("oa_patient_code_postal")
            End If

            If patientDataReader("oa_patient_ville") Is DBNull.Value Then
                instancePatient.PatientVille = ""
            Else
                instancePatient.PatientVille = patientDataReader("oa_patient_ville")
            End If

            If patientDataReader("oa_patient_tel1") Is DBNull.Value Then
                instancePatient.PatientTel1 = ""
            Else
                instancePatient.PatientTel1 = patientDataReader("oa_patient_tel1")
            End If

            If patientDataReader("oa_patient_tel2") Is DBNull.Value Then
                instancePatient.PatientTel2 = ""
            Else
                instancePatient.PatientTel2 = patientDataReader("oa_patient_tel2")
            End If

            If patientDataReader("oa_patient_email") Is DBNull.Value Then
                instancePatient.PatientEmail = ""
            Else
                instancePatient.PatientEmail = patientDataReader("oa_patient_email")
            End If

            If patientDataReader("oa_patient_nom_marital") Is DBNull.Value Then
                instancePatient.PatientNomMarital = ""
            Else
                instancePatient.PatientNomMarital = patientDataReader("oa_patient_nom_marital")
            End If

            If patientDataReader("oa_patient_date_entree_oasis") Is DBNull.Value Then
                instancePatient.PatientDateEntree = Nothing
            Else
                instancePatient.PatientDateEntree = CDate(patientDataReader("oa_patient_date_entree_oasis"))
            End If

            If patientDataReader("oa_patient_date_sortie_oasis") Is DBNull.Value Then
                instancePatient.PatientDateSortie = Nothing
            Else
                instancePatient.PatientDateSortie = CDate(patientDataReader("oa_patient_date_sortie_oasis"))
            End If

            If patientDataReader("oa_patient_commentaire_sortie") Is DBNull.Value Then
                instancePatient.PatientCommentaireSortie = ""
            Else
                instancePatient.PatientCommentaireSortie = patientDataReader("oa_patient_commentaire_sortie")
            End If

            If patientDataReader("oa_patient_date_deces") Is DBNull.Value Then
                instancePatient.PatientDateDeces = Nothing
            Else
                instancePatient.PatientDateDeces = CDate(patientDataReader("oa_patient_date_deces"))
            End If

            If patientDataReader("oa_patient_site_id") Is DBNull.Value Then
                instancePatient.PatientSiteId = 0
            Else
                instancePatient.PatientSiteId = CInt(patientDataReader("oa_patient_site_id"))
            End If

            If patientDataReader("oa_patient_couverture_internet") Is DBNull.Value Then
                instancePatient.PatientInternet = False
            Else
                instancePatient.PatientInternet = CInt(patientDataReader("oa_patient_couverture_internet"))
            End If

            If patientDataReader("oa_patient_unite_sanitaire_id") Is DBNull.Value Then
                instancePatient.PatientUniteSanitaireId = 0
            Else
                instancePatient.PatientUniteSanitaireId = CInt(patientDataReader("oa_patient_unite_sanitaire_id"))
            End If

            If patientDataReader("oa_patient_synthese_date_maj") Is DBNull.Value Then
                instancePatient.PatientSyntheseDateMaj = Nothing
            Else
                instancePatient.PatientSyntheseDateMaj = CDate(patientDataReader("oa_patient_synthese_date_maj"))
            End If

            If patientDataReader("oa_patient_date_naissance") Is DBNull.Value Then
                instancePatient.PatientAge = "Inconnu"
            Else
                instancePatient.PatientAge = CalculAgeEnAnneeEtMoisString(instancePatient.PatientDateNaissance)
                instancePatient.PatientAgeEnAnnee = CalculAgeEnAnnee(instancePatient.PatientDateNaissance)
            End If
            Dim genre_description As String = Table_genre.GetGenreDescription(instancePatient.PatientGenreId)
            If genre_description = "" Then
                instancePatient.PatientGenre = "genre ?"
            Else
                instancePatient.PatientGenre = genre_description
            End If
            instancePatient.Profession = Coalesce(patientDataReader("oa_patient_profession"), "")
            instancePatient.PharmacienId = Coalesce(patientDataReader("oa_patient_pharmacie_id"), 0)
            instancePatient.Taille = Coalesce(patientDataReader("oa_patient_taille"), 0)
            instancePatient.BlocageMedical = Coalesce(patientDataReader("oa_patient_blocage_medical"), False)
            instancePatient.INS = Coalesce(patientDataReader("oa_patient_INS"), 0)
        End If
    End Sub

    Private Sub InitPatientProperties(instancePatient As Patient)
        instancePatient.PatientNir = 0
        instancePatient.PatientNom = ""
        instancePatient.PatientPrenom = ""
        instancePatient.PatientNomMarital = ""
        instancePatient.PatientDateNaissance = Nothing
        instancePatient.PatientGenreId = ""
        instancePatient.PatientAdresse1 = ""
        instancePatient.PatientAdresse2 = ""
        instancePatient.PatientCodePostal = ""
        instancePatient.PatientVille = ""
        instancePatient.PatientTel1 = ""
        instancePatient.PatientTel2 = ""
        instancePatient.PatientEmail = ""
        instancePatient.PatientDateEntree = Nothing
        instancePatient.PatientDateSortie = Nothing
        instancePatient.PatientDateDeces = Nothing
        instancePatient.PatientCommentaireSortie = ""
        instancePatient.PatientInternet = False
        instancePatient.PatientSiteId = 0
        instancePatient.PatientUniteSanitaireId = 0
        instancePatient.PatientSyntheseDateMaj = Nothing
        instancePatient.PatientAge = ""
        instancePatient.PatientGenre = ""
        instancePatient.Profession = ""
        instancePatient.PharmacienId = 0
        instancePatient.Taille = 0
        instancePatient.BlocageMedical = False
        instancePatient.INS = 0
    End Sub

    Public Function NonExistencePatientNIR(NIR As Int64, PatientId As Integer) As Boolean
        Dim CodeRetour As Boolean = True
        Dim conxn As New SqlConnection(getConnectionString())
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
            MessageBox.Show(ex.Message)
            CodeRetour = False
        Finally
            conxn.Close()
            NirPatientDataAdapter.Dispose()
        End Try

        Return CodeRetour
    End Function


    Public Function ListePatientDateNaissance(DateNaissance As Date) As DataTable
        Dim conxn As New SqlConnection(getConnectionString())
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
            MessageBox.Show(ex.Message)
        Finally
            conxn.Close()
            ListePatientDataAdapter.Dispose()
        End Try

        Return ListePatientDataTable
    End Function

    Public Function GetAllPatient(Tous As Boolean, PatientOasis As Boolean) As DataTable
        Dim conxn As New SqlConnection(getConnectionString())
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
            MessageBox.Show(ex.Message)
        Finally
            conxn.Close()
            da.Dispose()
        End Try

        Return dt
    End Function

    Friend Function ModificationDateMajSynthesePatient(patientId As Integer) As Boolean
        'Dim da As MySqlDataAdapter = New MySqlDataAdapter()
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim patient As Patient = getPatientById(patientId)

        Dim SQLstring As String = "UPDATE oasis.oa_patient SET oa_patient_synthese_date_maj = @date WHERE oa_patient_id = @patientId"

        Dim conxn As New SqlConnection(getConnectionString())
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
            MessageBox.Show(ex.Message)
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
    Friend Function ModificationPatientTaille(patientId As Integer, taille As Integer) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "UPDATE oasis.oa_patient SET oa_patient_taille = @taille WHERE oa_patient_id = @patientId"

        Dim conxn As New SqlConnection(getConnectionString())
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
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try
        Return codeRetour
    End Function

    'Modification taille
    Friend Function BlocageMedical(patientId As Integer) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "UPDATE oasis.oa_patient SET oa_patient_blocage_medical = @blocageMedical WHERE oa_patient_id = @patientId"

        Dim conxn As New SqlConnection(getConnectionString())
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
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try
        Return codeRetour
    End Function

    Friend Function GetStringContreIndicationByPatient(patientId As Long) As String
        Dim contreIndicationDao As New ContreIndicationATCDao
        Dim dt As DataTable
        Dim StringContreIndication As String = ""
        dt = contreIndicationDao.getAllContreIndicationATCbyPatient(patientId)
        Dim rowCount As Integer = dt.Rows.Count - 1
        For i = 0 To rowCount Step 1
            StringContreIndication += dt.Rows(i)("code_atc") & " : " & dt.Rows(i)("Denomination_atc") & vbCrLf
        Next

        Return StringContreIndication
    End Function

End Module
