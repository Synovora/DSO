Imports System.Data.SqlClient
Imports Oasis_Common

Public Class OrdonnanceDetailDao
    Inherits OrdonnanceDetailDaoBase

    Public Function CreationOrdonnanceDetail(ordonnanceDetail As OrdonnanceDetail) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
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
            .AddWithValue("@ordonnanceId", ordonnanceDetail.OrdonnanceId.ToString)
            .AddWithValue("@traitement", ordonnanceDetail.Traitement)
            .AddWithValue("@traitementId", ordonnanceDetail.TraitementId.ToString)
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
            da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function ModificationOrdonnanceDetailALD(OrdonnanceLigneId As Integer, ald As Boolean) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
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
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function ModificationOrdonnanceDetailDelivrance(OrdonnanceLigneId As Integer, delivrance As Boolean) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
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
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function ModificationOrdonnanceDetail(OrdonnanceLigneId As Integer, posologieCommentaire As String, duree As Integer, posologie As String) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
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
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function SuppressionOrdonnanceDetailByDrcId(ordonnanceId As Long) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
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
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

End Class
