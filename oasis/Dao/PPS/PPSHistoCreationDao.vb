Imports System.Data.SqlClient
Module PPSHistoCreationDao

    Public Enum EnumEtatPPSHisto
        Creation = 1
        Modification = 2
        Arret = 3
        Annulation = 4
    End Enum

    Public Function CreationPPSHisto(PPSHistoACreer As PpsHisto, UtilisateurConnecte As Utilisateur, EtatHistorisation As Integer) As Boolean
        Dim conxn As New SqlConnection(getConnectionString())
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim dateCreation As DateTime = Date.Now.Date
        Dim AffichageSynthese, Arret, Inactif As Integer

        If PPSHistoACreer.AffichageSynthese = True Then
            AffichageSynthese = 1
        Else
            AffichageSynthese = 0
        End If

        If PPSHistoACreer.Arret = True Then
            Arret = 1
        Else
            Arret = 0
        End If

        If PPSHistoACreer.Inactif = True Then
            Inactif = 1
        Else
            Inactif = 0
        End If

        Dim SQLstring As String = "insert into oasis.oa_patient_pps_histo (oa_pps_histo_date_historisation, oa_pps_histo_utilisateur_historisation," &
        " oa_pps_histo_etat_historisation, oa_pps_id, oa_pps_patient_id, oa_pps_categorie, oa_pps_sous_categorie," &
        " oa_pps_priorite, oa_pps_drc_id, oa_pps_affichage_synthese, oa_pps_commentaire, oa_pps_date_debut," &
        " oa_pps_arret, oa_pps_commentaire_arret, oa_pps_inactif)" &
        " VALUES (@dateHistorisation, @utilisateurHistorisation, @etatHistorisation, @ppsId, @patientId, @categorie, @sousCategorie," &
        " @priorite, @drcId, @affichageSynthese, @commentaire, @dateDebut, @arret, @arretCommentaire, @inactif);"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@dateHistorisation", DateTime.Now().ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@utilisateurHistorisation", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@etatHistorisation", EtatHistorisation.ToString)
            .AddWithValue("@ppsId", PPSHistoACreer.PpsId.ToString)
            .AddWithValue("@patientId", PPSHistoACreer.PatientId.ToString)
            .AddWithValue("@categorie", PPSHistoACreer.Categorie.ToString)
            .AddWithValue("@sousCategorie", PPSHistoACreer.SousCategorie.ToString)
            .AddWithValue("@priorite", PPSHistoACreer.Priorite.ToString)
            .AddWithValue("@drcId", PPSHistoACreer.DrcId.ToString)
            .AddWithValue("@affichageSynthese", AffichageSynthese.ToString)
            .AddWithValue("@commentaire", PPSHistoACreer.Commentaire.ToString)
            .AddWithValue("@dateDebut", PPSHistoACreer.DateDebut.ToString)
            .AddWithValue("@arret", Arret.ToString)
            .AddWithValue("@arretCommentaire", PPSHistoACreer.ArretCommentaire.ToString)
            .AddWithValue("@inactif", Inactif.ToString)
        End With

        Try
            conxn.Open()
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        Return codeRetour
    End Function

    Public Sub InitClassePPStHistorisation(PPSDataReader As SqlDataReader, UtilisateurConnecte As Utilisateur, PPSHistoACreer As PpsHisto)
        'Initialisation classe Historisation PPS
        PPSHistoACreer.HistorisationDate = Date.Now()
        PPSHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
        PPSHistoACreer.HistorisationEtat = 0
        PPSHistoACreer.PpsId = PPSDataReader("oa_pps_id")
        PPSHistoACreer.PatientId = PPSDataReader("oa_pps_patient_id")
        PPSHistoACreer.Categorie = PPSDataReader("oa_pps_categorie")
        PPSHistoACreer.SousCategorie = PPSDataReader("oa_pps_sous_categorie")
        If PPSDataReader("oa_pps_priorite") Is DBNull.Value Then
            PPSHistoACreer.Priorite = 0
        Else
            PPSHistoACreer.Priorite = PPSDataReader("oa_pps_priorite")
        End If
        If PPSDataReader("oa_pps_drc_id") Is DBNull.Value Then
            PPSHistoACreer.DrcId = 0
        Else
            PPSHistoACreer.DrcId = PPSDataReader("oa_pps_drc_id")
        End If
        If PPSDataReader("oa_pps_affichage_synthese") Is DBNull.Value Then
            PPSHistoACreer.AffichageSynthese = 0
        Else
            PPSHistoACreer.AffichageSynthese = PPSDataReader("oa_pps_affichage_synthese")
        End If
        If PPSDataReader("oa_pps_commentaire") Is DBNull.Value Then
            PPSHistoACreer.Commentaire = ""
        Else
            PPSHistoACreer.Commentaire = PPSDataReader("oa_pps_commentaire")
        End If
        If PPSDataReader("oa_pps_date_debut") Is DBNull.Value Then
            PPSHistoACreer.DateDebut = New Date(1, 1, 1)
        Else
            PPSHistoACreer.DateDebut = PPSDataReader("oa_pps_date_debut")
        End If
        If PPSDataReader("oa_pps_arret") Is DBNull.Value Then
            PPSHistoACreer.Arret = 0
        Else
            PPSHistoACreer.Arret = PPSDataReader("oa_pps_arret")
        End If
        If PPSDataReader("oa_pps_commentaire_arret") Is DBNull.Value Then
            PPSHistoACreer.ArretCommentaire = ""
        Else
            PPSHistoACreer.ArretCommentaire = PPSDataReader("oa_pps_commentaire_arret")
        End If
        If PPSDataReader("oa_pps_inactif") Is DBNull.Value Then
            PPSHistoACreer.Inactif = False
        Else
            PPSHistoACreer.Inactif = PPSDataReader("oa_pps_inactif")
        End If
    End Sub

End Module
