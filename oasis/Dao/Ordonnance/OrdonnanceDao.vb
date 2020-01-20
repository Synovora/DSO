Imports System.Data.SqlClient

Public Class OrdonnanceDao
    Inherits StandardDao

    Friend Function getOrdonnaceById(OrdonnanceId As Integer) As Ordonnance
        Dim ordonnance As Ordonnance
        Dim con As SqlConnection
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_patient_ordonnance WHERE oa_ordonnance_id = @ordonnanceId"
            command.Parameters.AddWithValue("@ordonnanceId", OrdonnanceId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    ordonnance = buildBean(reader)
                Else
                    Throw New ArgumentException("Ordonnance inexistante !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return ordonnance
    End Function

    Private Function buildBean(reader As SqlDataReader) As Ordonnance
        Dim ordonnance As New Ordonnance
        ordonnance.Id = reader("oa_ordonnance_id")
        ordonnance.PatientId = Coalesce(reader("oa_ordonnance_patient_id"), 0)
        ordonnance.EpisodeId = Coalesce(reader("oa_ordonnance_episode_id"), 0)
        ordonnance.UtilisateurCreation = Coalesce(reader("oa_ordonnance_utilisateur_creation"), 0)
        ordonnance.DateCreation = Coalesce(reader("oa_ordonnance_date_creation"), Nothing)
        ordonnance.DateValidation = Coalesce(reader("oa_ordonnance_date_validation"), Nothing)
        ordonnance.DateEdition = Coalesce(reader("oa_ordonnance_date_edition"), Nothing)
        ordonnance.Commentaire = Coalesce(reader("oa_ordonnance_commentaire"), "")
        ordonnance.Renouvellement = Coalesce(reader("oa_ordonnance_renouvellement"), 0)

        Return ordonnance
    End Function

    Friend Function getAllOrdonnancebyPatient(patientId As Integer) As DataTable
        Dim SQLString As String
        SQLString = "SELECT * FROM oasis.oa_patient_ordonnance" &
                    " WHERE oa_ordonnance_patient_id = " & patientId.ToString &
                    " ORDER BY oa_ordonnance_id DESC"

        Using con As SqlConnection = GetConnection()
            Dim OrdonnanceDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using OrdonnanceDataAdapter
                OrdonnanceDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim OrdonnanceDataTable As DataTable = New DataTable()
                Using OrdonnanceDataTable
                    Try
                        OrdonnanceDataAdapter.Fill(OrdonnanceDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return OrdonnanceDataTable
                End Using
            End Using
        End Using
    End Function

    Friend Function getOrdonnanceValidebyPatient(patientId As Integer, episodeId As Long) As DataTable
        Dim SQLString As String
        SQLString = "SELECT * FROM oasis.oa_patient_ordonnance" &
                    " WHERE oa_ordonnance_patient_id = " & patientId.ToString &
                    " AND oa_ordonnance_episode_id = " & episodeId.ToString &
                    " AND (oa_ordonnance_inactif = 'False' OR oa_ordonnance_inactif is Null)"

        Using con As SqlConnection = GetConnection()
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Using da
                da.SelectCommand = New SqlCommand(SQLString, con)
                Dim dt As DataTable = New DataTable()
                Using dt
                    Try
                        da.Fill(dt)
                        Dim command As SqlCommand = con.CreateCommand()
                        If dt.Rows.Count > 0 Then

                        End If
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return dt
                End Using
            End Using
        End Using
    End Function

    Friend Function CreateOrdonnance(patientId As Integer, episodeId As Integer) As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim OrdonnanceId As Integer = 0
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "INSERT into oasis.oa_patient_ordonnance" &
                                " (oa_ordonnance_patient_id, oa_ordonnance_utilisateur_creation, oa_ordonnance_date_creation," &
                                " oa_ordonnance_episode_id, oa_ordonnance_commentaire, oa_ordonnance_renouvellement)" &
                                " VALUES (@patientId, @userCreation, @dateCreation," &
                                " @episodeid, @commentaire, @renouvellement)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@patientId", patientId)
            .AddWithValue("@userCreation", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateCreation", Date.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@episodeId", episodeId.ToString)
            .AddWithValue("@commentaire", "")
            .AddWithValue("@renouvellement", 0)
        End With

        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        If codeRetour = True Then
            'Récupération de l'identifiant du traitement créé
            Dim ordonnanceLastDataReader As SqlDataReader
            SQLstring = "SELECT MAX(oa_ordonnance_id) FROM oasis.oa_patient_ordonnance" &
                        " WHERE oa_ordonnance_patient_id = " & patientId &
                        " AND oa_ordonnance_episode_id = " & episodeId.ToString

            Dim traitementLastCommand As New SqlCommand(SQLstring, con)
            Try
                con.Open()
                ordonnanceLastDataReader = traitementLastCommand.ExecuteReader()
                If ordonnanceLastDataReader.HasRows Then
                    ordonnanceLastDataReader.Read()
                    'Récupération de la clé de l'enregistrement créé
                    OrdonnanceId = ordonnanceLastDataReader(0)
                End If
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            Finally
                con.Close()
                traitementLastCommand.Dispose()
            End Try
        End If

        Return OrdonnanceId
    End Function

    Friend Function ModificationOrdonnanceCommentaire(OrdonnanceId As Integer, Commentaire As String) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String = "UPDATE oasis.oa_patient_ordonnance SET" &
        " oa_ordonnance_commentaire = @commentaire" &
        " WHERE oa_ordonnance_id = @ordonnanceId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@ordonnanceId", OrdonnanceId)
            .AddWithValue("@commentaire", Commentaire.Trim())
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function ModificationOrdonnanceRenouvellement(OrdonnanceId As Integer, Renouvellement As Integer) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String = "UPDATE oasis.oa_patient_ordonnance SET" &
        " oa_ordonnance_renouvellement = @renouvellement" &
        " WHERE oa_ordonnance_id = @ordonnanceId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@ordonnanceId", OrdonnanceId)
            .AddWithValue("@renouvellement", Renouvellement)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function AnnulerOrdonnance(OrdonnanceId As Integer) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String = "UPDATE oasis.oa_patient_ordonnance SET" &
        " oa_ordonnance_inactif = @inactif" &
        " WHERE oa_ordonnance_id = @ordonnanceId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@ordonnanceId", OrdonnanceId)
            .AddWithValue("@inactif", True)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function CreateNewOrdonnanceDetail(patientId As Integer, ordonnanceId As Integer) As Boolean
        Dim Coderetour As Boolean = True
        Dim TraitementDao As New TraitementDao
        Dim OrdonnanceDetailDao As New OrdonnanceDetailDao
        Dim TraitementDataTable As DataTable
        TraitementDataTable = TraitementDao.getTraitementNotCancelledbyPatient(patientId)

        Dim i As Integer
        Dim rowCount As Integer = TraitementDataTable.Rows.Count - 1

        Dim DateFin, DateDebut As Date

        For i = 0 To rowCount Step 1

            'Exclusion des médicaments déclarés 'allergique'
            If TraitementDataTable.Rows(i)("oa_traitement_allergie") IsNot DBNull.Value Then
                If TraitementDataTable.Rows(i)("oa_traitement_allergie") = "1" Then
                    Continue For
                End If
            End If

            'Exclusion des médicaments déclarés 'contre-indiqué'
            If TraitementDataTable.Rows(i)("oa_traitement_contre_indication") IsNot DBNull.Value Then
                If TraitementDataTable.Rows(i)("oa_traitement_contre_indication") = "1" Then
                    Continue For
                End If
            End If

            'Exclusion des traitements déclarés 'arrêté'
            'Cette condition est traitée en exclusion (et non dans la requête SQL) pour stocker les allergies et les contre-indications arrêtés dans la StringCollection
            If TraitementDataTable.Rows(i)("oa_traitement_arret") IsNot DBNull.Value Then
                If TraitementDataTable.Rows(i)("oa_traitement_arret") = "A" Then
                    Continue For
                End If
            End If

            'Date début
            DateDebut = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_date_debut"), "01/01/1900")

            'Exclusion des traitements dont la date de fin est < à la date du jour
            DateFin = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_date_fin"), "31/12/2999")
            Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
            Dim dateFinaComparer As New Date(DateFin.Year, DateFin.Month, DateFin.Day, 0, 0, 0)
            If (dateFinaComparer < dateJouraComparer) Then
                Continue For
            End If

            Dim ordonnanceDetail As New OrdonnanceDetail
            ordonnanceDetail.OrdonnanceId = ordonnanceId
            ordonnanceDetail.Traitement = True
            ordonnanceDetail.TraitementId = TraitementDataTable.Rows(i)("oa_traitement_id")
            ordonnanceDetail.OrdreAffichage = TraitementDataTable.Rows(i)("oa_traitement_ordre_affichage")
            ordonnanceDetail.MedicamentCis = TraitementDataTable.Rows(i)("oa_traitement_medicament_cis")
            ordonnanceDetail.MedicamentDci = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_medicament_dci"), "")
            ordonnanceDetail.DateDebut = DateDebut
            ordonnanceDetail.DateFin = DateFin
            ordonnanceDetail.PosologieBase = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_base"), "")
            ordonnanceDetail.PosologieRythme = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_rythme"), 0)
            ordonnanceDetail.PosologieMatin = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_matin"), 0)
            ordonnanceDetail.PosologieMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_midi"), 0)
            ordonnanceDetail.PosologieApresMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_apres_midi"), 0)
            ordonnanceDetail.PosologieSoir = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_soir"), 0)
            ordonnanceDetail.FractionMatin = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_matin"), "")
            ordonnanceDetail.FractionMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_midi"), "")
            ordonnanceDetail.FractionApresMidi = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_apres_midi"), "")
            ordonnanceDetail.FractionSoir = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fraction_soir"), "")
            ordonnanceDetail.PosologieCommentaire = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_posologie_commentaire"), "")
            ordonnanceDetail.Commentaire = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_commentaire"), "")
            ordonnanceDetail.Fenetre = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fenetre"), False)
            ordonnanceDetail.FenetreDateDebut = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fenetre_date_debut"), "31/12/2999")
            ordonnanceDetail.FenetreDateFin = Coalesce(TraitementDataTable.Rows(i)("oa_traitement_fenetre_date_fin"), "31/12/2999")

            'Existence d'une fenêtre thérapeutique
            Dim FenetreTherapeutiqueExiste As Boolean = False
            'Dim dateDebutFenetreaComparer As New Date(FenetreDateDebut.Year, FenetreDateDebut.Month, FenetreDateDebut.Day, 0, 0, 0)
            'Dim dateFinFenetreaComparer As New Date(FenetreDateFin.Year, FenetreDateFin.Month, FenetreDateFin.Day, 0, 0, 0)
            If TraitementDataTable.Rows(i)("oa_traitement_fenetre") IsNot DBNull.Value Then
                If TraitementDataTable.Rows(i)("oa_traitement_fenetre") = "1" Then
                    'Fenêtre thérapeutique en cours, à venir ou obsolète
                    FenetreTherapeutiqueExiste = True
                    'If FenetreDateDebut <= dateJouraComparer And FenetreDateFin >= dateJouraComparer Then
                    'Fenêtre thérapeutique en cours
                    'FenetreTherapeutiqueEnCours = True
                Else
                    '       If FenetreDateDebut > dateJouraComparer Then
                    '  FenetreTherapeutiqueAVenir = True
                End If
            End If

            OrdonnanceDetailDao.CreationOrdonnanceDetail(ordonnanceDetail)
        Next

        Return Coderetour
    End Function

End Class
