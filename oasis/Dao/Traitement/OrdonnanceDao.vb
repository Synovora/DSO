Imports System.Data.SqlClient

Public Class OrdonnanceDao
    Inherits StandardDao

    Friend Function getAllOrdonnancebyPatient(patientId As Integer) As DataTable
        Dim SQLString As String
        SQLString = "select * from oasis.oa_patient_ordonnance where oa_ordonnance_patient_id = " & patientId.ToString &
                    " order by oa_ordonnance_id DESC"

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

    Friend Function CreateOrdonnance(patientId As Integer, userId As Integer) As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "insert into oasis.oa_patient_ordonnance" &
        " (oa_ordonnance_patient_id, oa_ordonnance_utilisateur_creation, oa_ordonnance_date_creation," &
        " oa_ordonnance_episode_id, oa_ordonnance_commentaire)" &
        " VALUES (@patientId, @userCreation, @dateCreation," &
        " @episodeid, @commentaire)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@patientId", patientId)
            .AddWithValue("@userCreation", userId.ToString)
            .AddWithValue("@dateCreation", Date.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@episodeId", 0)
            .AddWithValue("@commentaire", "")
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

        End If
        'Récupération de l'identifiant du traitement créé
        Dim OrdonnanceId As Integer
        Dim ordonnanceLastDataReader As SqlDataReader
        SQLstring = "select max(oa_ordonnance_id) from oasis.oa_patient_ordonnance where oa_ordonnance_patient_id = " & patientId & ";"
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

        Return OrdonnanceId
    End Function

    Friend Function ModificationOrdonnanceCommentaire(OrdonnanceId As Integer, Commentaire As String) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_patient_ordonnance set oa_ordonnance_commentaire = @commentaire where oa_ordonnance_id = @ordonnanceId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@ordonnanceId", OrdonnanceId)
            .AddWithValue("@commentaire", Commentaire.Trim())
        End With

        Try
            'con.Open()
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
            'End If
            'End If

            OrdonnanceDetailDao.CreationOrdonnanceDetail(ordonnanceDetail)
        Next

        Return Coderetour
    End Function

End Class
