Imports System.Data.SqlClient
Imports Oasis_Common
Public Class ParcoursHistoDao
    Inherits StandardDao

    Public Enum EnumEtatParcoursHisto
        Creation = 1
        Modification = 2
        Annulation = 4
    End Enum

    Public Function CreationParcoursHisto(ParcoursHistoACreer As ParcoursHisto, UtilisateurConnecte As Utilisateur, EtatHistorisation As Integer) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "insert into oasis.oasis.oa_patient_Parcours_histo" &
        " (oa_parcours_histo_date_historisation, oa_Parcours_histo_user_historisation," &
        " oa_parcours_histo_etat, oa_parcours_id, oa_parcours_patient_id, oa_parcours_specialite, oa_parcours_categorie_id, oa_parcours_sous_categorie_id," &
        " oa_parcours_intervenant_oasis, oa_parcours_ror_id, oa_parcours_commentaire, oa_parcours_base," &
        " oa_parcours_rythme, oa_parcours_cacher, oa_parcours_inactif)" &
        " VALUES (@dateHistorisation, @utilisateurHistorisation," &
        " @etat, @ParcoursId, @patientId, @specialite, @categorie, @sousCategorie," &
        " @IntervenantOasis, @rorId, @commentaire, @base," &
        " @rythme, @cacher, @inactif)"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@dateHistorisation", Date.Now)
            .AddWithValue("@utilisateurHistorisation", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@etat", EtatHistorisation.ToString)
            .AddWithValue("@ParcoursId", ParcoursHistoACreer.Id.ToString)
            .AddWithValue("@patientId", ParcoursHistoACreer.PatientId.ToString)
            .AddWithValue("@specialite", ParcoursHistoACreer.SpecialiteId.ToString)
            .AddWithValue("@categorie", ParcoursHistoACreer.CategorieId.ToString)
            .AddWithValue("@sousCategorie", ParcoursHistoACreer.SousCategorieId.ToString)
            .AddWithValue("@IntervenantOasis", ParcoursHistoACreer.IntervenantOasis.ToString)
            .AddWithValue("@rorId", ParcoursHistoACreer.RorId.ToString)
            .AddWithValue("@commentaire", ParcoursHistoACreer.Commentaire.ToString)
            .AddWithValue("@base", ParcoursHistoACreer.Base)
            .AddWithValue("@rythme", ParcoursHistoACreer.Rythme.ToString)
            .AddWithValue("@cacher", ParcoursHistoACreer.Cacher)
            .AddWithValue("@inactif", ParcoursHistoACreer.Inactif)
        End With

        Try
            'conxn.Open()
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

    Public Function getAllParcoursHistobyParcoursId(parcoursId As Integer) As DataTable
        Dim SQLString As String

        SQLString = "select * from oasis.oa_patient_parcours_histo where oa_parcours_id = '" + parcoursId.ToString + "' order by oa_parcours_histo_id desc;"

        Using con As SqlConnection = GetConnection()
            Dim AntecedentHistoDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using AntecedentHistoDataAdapter
                AntecedentHistoDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim AntecedentHistoDataTable As DataTable = New DataTable()
                Using AntecedentHistoDataTable
                    Try
                        AntecedentHistoDataAdapter.Fill(AntecedentHistoDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return AntecedentHistoDataTable
                End Using
            End Using
        End Using
    End Function

End Class
