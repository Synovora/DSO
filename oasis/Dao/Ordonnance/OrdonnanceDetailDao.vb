Imports System.Data.SqlClient

Public Class OrdonnanceDetailDao
    Inherits StandardDao

    Public Structure EnumDelivrance
        Const A_DELIVRER = "A délivrer"
        Const NE_PAS_DELIVRER = "Ne pas délivrer"
    End Structure

    Friend Function getAllOrdonnanceLigneByOrdonnanceId(ordonnanceId As Integer) As DataTable
        Dim SQLString As String
        SQLString = "SELECT * FROM oasis.oa_patient_ordonnance_detail WHERE oa_ordonnance_id = " & ordonnanceId.ToString &
                    " ORDER BY oa_traitement_ordre_affichage"

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

    Friend Function CreationOrdonnanceDetail(ordonnanceDetail As OrdonnanceDetail) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "insert into oasis.oa_patient_ordonnance_detail" &
        " (oa_ordonnance_id, oa_ordonnance_traitement, oa_traitement_id, oa_traitement_ordre_affichage, oa_traitement_ald," &
        " oa_traitement_a_delivrer, oa_traitement_medicament_cis, oa_traitement_medicament_dci, oa_traitement_duree," &
        " oa_traitement_date_debut, oa_traitement_date_fin, oa_traitement_posologie_base, oa_traitement_posologie_rythme, oa_traitement_posologie_matin," &
        " oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir, oa_traitement_posologie_commentaire, oa_traitement_commentaire," &
        " oa_traitement_fraction_matin,oa_traitement_fraction_midi, oa_traitement_fraction_apres_midi, oa_traitement_fraction_soir," &
        " oa_traitement_fenetre, oa_traitement_fenetre_date_debut, oa_traitement_fenetre_date_fin, oa_traitement_inactif)" &
        " VALUES (@ordonnanceId, @traitement, @traitementId, @ordreAffichage, @ald," &
        " @aDelivrer, @medicamentCis, @medicamentDci, @duree," &
        " @dateDebut, @dateFin, @posologieBase, @posologieRythme, @posologieMatin," &
        " @posologieMidi, @posologieApresMidi, @posologieSoir, @posologieCommentaire, @commentaire," &
        " @FractionMatin, @FractionMidi, @FractionApresMidi, @FractionSoir," &
        " @fenetre, @fenetreDateDebut, @fenetreDateFin, @inactif)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@ordonnanceId", ordonnanceDetail.OrdonnanceId.ToString)
            .AddWithValue("@traitement", True)
            .AddWithValue("@traitementId", ordonnanceDetail.TraitementId.ToString)
            .AddWithValue("@ordreAffichage", ordonnanceDetail.OrdreAffichage.ToString)
            .AddWithValue("@ald", ordonnanceDetail.Ald)
            .AddWithValue("@aDelivrer", ordonnanceDetail.ADelivrer)
            .AddWithValue("@medicamentCis", ordonnanceDetail.MedicamentCis.ToString)
            .AddWithValue("@medicamentDci", ordonnanceDetail.MedicamentDci)
            .AddWithValue("@dateDebut", ordonnanceDetail.DateDebut)
            .AddWithValue("@dateFin", ordonnanceDetail.DateFin)
            .AddWithValue("@duree", ordonnanceDetail.Duree)
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
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function ModificationOrdonnanceDetailALD(OrdonnanceLigneId As Integer, ald As Boolean) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

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
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Friend Function ModificationOrdonnanceDetailDelivrance(OrdonnanceLigneId As Integer, delivrance As Boolean) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

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
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

End Class
