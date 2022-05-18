Imports System.Data.SqlClient

Public Class OrdonnanceDetailDao
    Inherits StandardDao

    Private Function BuildBean(reader As SqlDataReader) As OrdonnanceDetail
        Dim ordonnanceDetail As New OrdonnanceDetail With {
            .LigneId = reader("oa_ordonnance_ligne_id"),
            .Traitement = Coalesce(reader("oa_ordonnance_traitement"), False),
            .TraitementId = Coalesce(reader("oa_traitement_id"), 0),
            .OrdreAffichage = Coalesce(reader("oa_traitement_ordre_affichage"), 0),
            .Ald = Coalesce(reader("oa_traitement_ald"), False),
            .ADelivrer = Coalesce(reader("oa_traitement_a_delivrer"), False),
            .MedicamentCis = Coalesce(reader("oa_traitement_medicament_cis"), 0),
            .MedicamentDci = Coalesce(reader("oa_traitement_medicament_dci"), ""),
            .DateDebut = Coalesce(reader("oa_traitement_date_debut"), Nothing),
            .DateFin = Coalesce(reader("oa_traitement_date_fin"), Nothing),
            .Duree = Coalesce(reader("oa_traitement_duree"), 0),
            .Posologie = Coalesce(reader("oa_traitement_posologie"), ""),
            .PosologieBase = Coalesce(reader("oa_traitement_posologie_base"), ""),
            .PosologieRythme = Coalesce(reader("oa_traitement_posologie_rythme"), 0),
            .PosologieMatin = Coalesce(reader("oa_traitement_posologie_matin"), 0),
            .PosologieMidi = Coalesce(reader("oa_traitement_posologie_midi"), 0),
            .PosologieApresMidi = Coalesce(reader("oa_traitement_posologie_apres_midi"), 0),
            .PosologieSoir = Coalesce(reader("oa_traitement_posologie_soir"), 0),
            .FractionMatin = Coalesce(reader("oa_traitement_fraction_matin"), ""),
            .FractionMidi = Coalesce(reader("oa_traitement_fraction_midi"), ""),
            .FractionApresMidi = Coalesce(reader("oa_traitement_fraction_apres_midi"), ""),
            .FractionSoir = Coalesce(reader("oa_traitement_fraction_soir"), ""),
            .PosologieCommentaire = Coalesce(reader("oa_traitement_posologie_commentaire"), ""),
            .Commentaire = Coalesce(reader("oa_traitement_commentaire"), ""),
            .Fenetre = Coalesce(reader("oa_traitement_fenetre"), False),
            .FenetreDateDebut = Coalesce(reader("oa_traitement_fenetre_date_debut"), Nothing),
            .FenetreDateFin = Coalesce(reader("oa_traitement_fenetre_date_fin"), Nothing),
            .Inactif = Coalesce(reader("oa_traitement_inactif"), False)
        }
        Return ordonnanceDetail
    End Function

    Public Function GetOrdonnanceLigneById(OrdonnanceLigneId As Long) As OrdonnanceDetail
        Dim ordonnanceDetail As OrdonnanceDetail
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_patient_ordonnance_detail WHERE oa_ordonnance_ligne_id = @ordonnanceLigneId"
            command.Parameters.AddWithValue("@ordonnanceLigneId", OrdonnanceLigneId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    ordonnanceDetail = BuildBean(reader)
                Else
                    Throw New ArgumentException("Ordonnance inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return ordonnanceDetail
    End Function

    Public Function GetOrdonnanceLigneByOrdonnanceId(ordonnanceId As Integer) As List(Of OrdonnanceDetail)
        Dim ordonnanceDetail As New List(Of OrdonnanceDetail)
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText =
                "SELECT * FROM oasis.oa_patient_ordonnance_detail WHERE oa_ordonnance_id = @ordonnanceId " &
                "ORDER BY oa_traitement_ordre_affichage, oa_ordonnance_ligne_id;"
            command.Parameters.AddWithValue("@ordonnanceId", ordonnanceId)
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    ordonnanceDetail.Add(BuildBean(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return ordonnanceDetail
    End Function

    'TODO: change it
    Public Function GetAllOrdonnanceLigneSelectAldByOrdonnanceId(ordonnanceId As Integer, traitementAld As Boolean) As DataTable
        Dim SQLString As String = "SELECT * FROM oasis.oa_patient_ordonnance_detail" &
                    " WHERE oa_ordonnance_id = " & ordonnanceId.ToString &
                    " And oa_traitement_ald = '" & traitementAld & "'" &
                    " ORDER BY oa_traitement_ordre_affichage, oa_ordonnance_ligne_id"
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

    Public Function CreationOrdonnanceDetail(ordonnanceDetail As OrdonnanceDetail) As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim ordonnanceDetailId As Integer = 0
        Dim con As SqlConnection = GetConnection()
        Dim dateCreation As Date = Date.Now.Date
        Dim SQLstring As String = "insert into oasis.oa_patient_ordonnance_detail" &
        " (oa_ordonnance_id, oa_ordonnance_traitement, oa_traitement_id, oa_traitement_ordre_affichage, oa_traitement_ald," &
        " oa_traitement_a_delivrer, oa_traitement_medicament_cis, oa_traitement_medicament_dci, oa_traitement_duree, oa_traitement_posologie," &
        " oa_traitement_date_debut, oa_traitement_date_fin, oa_traitement_posologie_base, oa_traitement_posologie_rythme, oa_traitement_posologie_matin," &
        " oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir, oa_traitement_posologie_commentaire, oa_traitement_commentaire," &
        " oa_traitement_fraction_matin,oa_traitement_fraction_midi, oa_traitement_fraction_apres_midi, oa_traitement_fraction_soir," &
        " oa_traitement_fenetre, oa_traitement_fenetre_date_debut, oa_traitement_fenetre_date_fin, oa_traitement_inactif)" &
        " VALUES (@ordonnanceId, @traitement, @traitementId, @ordreAffichage, @ald," &
        " @aDelivrer, @medicamentCis, @medicamentDci, @duree, @posologie," &
        " @dateDebut, @dateFin, @posologieBase, @posologieRythme, @posologieMatin," &
        " @posologieMidi, @posologieApresMidi, @posologieSoir, @posologieCommentaire, @commentaire," &
        " @FractionMatin, @FractionMidi, @FractionApresMidi, @FractionSoir," &
        " @fenetre, @fenetreDateDebut, @fenetreDateFin, @inactif)"
        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@ordonnanceId", ordonnanceDetail.OrdonnanceId)
            .AddWithValue("@traitement", ordonnanceDetail.Traitement)
            .AddWithValue("@traitementId", ordonnanceDetail.TraitementId)
            .AddWithValue("@ordreAffichage", ordonnanceDetail.OrdreAffichage.ToString)
            .AddWithValue("@ald", ordonnanceDetail.Ald)
            .AddWithValue("@aDelivrer", ordonnanceDetail.ADelivrer)
            .AddWithValue("@medicamentCis", ordonnanceDetail.MedicamentCis.ToString)
            .AddWithValue("@medicamentDci", ordonnanceDetail.MedicamentDci)
            .AddWithValue("@dateDebut", ordonnanceDetail.DateDebut)
            .AddWithValue("@dateFin", ordonnanceDetail.DateFin)
            .AddWithValue("@duree", ordonnanceDetail.Duree)
            .AddWithValue("@posologie", ordonnanceDetail.Posologie)
            .AddWithValue("@posologieBase", ordonnanceDetail.PosologieBase)
            .AddWithValue("@posologieRythme", ordonnanceDetail.PosologieRythme.ToString)
            .AddWithValue("@posologieMatin", ordonnanceDetail.PosologieMatin.ToString)
            .AddWithValue("@posologieMidi", ordonnanceDetail.PosologieMidi.ToString)
            .AddWithValue("@posologieApresMidi", ordonnanceDetail.PosologieApresMidi.ToString)
            .AddWithValue("@posologieSoir", ordonnanceDetail.PosologieSoir.ToString)
            .AddWithValue("@fractionMatin", ordonnanceDetail.FractionMatin)
            .AddWithValue("@fractionMidi", ordonnanceDetail.FractionMidi)
            .AddWithValue("@fractionApresMidi", ordonnanceDetail.FractionApresMidi)
            .AddWithValue("@fractionSoir", ordonnanceDetail.FractionSoir)
            .AddWithValue("@posologieCommentaire", ordonnanceDetail.PosologieCommentaire)
            .AddWithValue("@commentaire", ordonnanceDetail.Commentaire)
            .AddWithValue("@fenetre", ordonnanceDetail.Fenetre)
            .AddWithValue("@fenetreDateDebut", ordonnanceDetail.FenetreDateDebut)
            .AddWithValue("@fenetreDateFin", ordonnanceDetail.FenetreDateFin)
            .AddWithValue("@inactif", False)
        End With
        Try
            da.InsertCommand = cmd
            ordonnanceDetailId = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return ordonnanceDetailId
    End Function

    Public Sub ModificationOrdonnanceDetailALD(OrdonnanceLigneId As Integer, ald As Boolean)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim SQLstring As String = "UPDATE oasis.oa_patient_ordonnance_detail SET" &
        " oa_traitement_ald = @ald" &
        " WHERE oa_ordonnance_ligne_id = @ordonnanceId"
        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@ordonnanceId", OrdonnanceLigneId)
            .AddWithValue("@ald", ald)
        End With
        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub

    Public Sub ModificationOrdonnanceDetailDelivrance(OrdonnanceLigneId As Integer, delivrance As Boolean)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim SQLstring As String = "UPDATE oasis.oa_patient_ordonnance_detail SET" &
        " oa_traitement_a_delivrer = @delivrance" &
        " WHERE oa_ordonnance_ligne_id = @ordonnanceId"
        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@ordonnanceId", OrdonnanceLigneId)
            .AddWithValue("@delivrance", delivrance)
        End With
        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub

    Public Sub ModificationOrdonnanceDetail(OrdonnanceLigneId As Integer, posologieCommentaire As String, duree As Integer, posologie As String)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim SQLstring As String = "UPDATE oasis.oa_patient_ordonnance_detail SET" &
        " oa_traitement_posologie_commentaire = @posologieCommentaire," &
        " oa_traitement_posologie = @posologie," &
        " oa_traitement_duree = @duree" &
        " WHERE oa_ordonnance_ligne_id = @ordonnanceId"
        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@ordonnanceId", OrdonnanceLigneId)
            .AddWithValue("@posologie", posologie)
            .AddWithValue("@posologieCommentaire", posologieCommentaire)
            .AddWithValue("@duree", duree)
        End With
        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub

    Public Sub SuppressionOrdonnanceDetailByDrcId(ordonnanceId As Long)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim SQLstring As String = "DELETE oasis.oa_patient_ordonnance_detail" &
        " WHERE oa_ordonnance_ligne_id = @ordonnanceId"
        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@ordonnanceId", ordonnanceId)
        End With
        Try
            da.DeleteCommand = cmd
            da.DeleteCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
    End Sub

End Class
