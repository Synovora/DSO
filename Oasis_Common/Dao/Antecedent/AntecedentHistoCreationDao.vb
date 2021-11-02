Imports System.Configuration
Imports System.Data.SqlClient

Public Class AntecedentHistoCreationDao

    Public Enum EnumEtatAntecedentHisto
        CreationAntecedent = 1
        ModificationAntecedent = 2
        ArretAntecedent = 3
        AnnulationAntecedent = 4
        ReactivationAntecedent = 5
    End Enum

    Public Shared Function CreationAntecedentHisto(AntecedentHistoACreer As AntecedentHisto, UtilisateurConnecte As Utilisateur, EtatHistorisation As Integer) As Boolean
        Dim conxn As New SqlConnection(GetConnectionString())
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim dateCreation As DateTime = Date.Now.Date

        Dim cmd As New SqlCommand("insert into oasis.oa_antecedent_histo (oa_antecedent_histo_date_historisation, oa_antecedent_histo_utilisateur_historisation," & vbCrLf &
        " oa_antecedent_histo_etat_historisation, oa_antecedent_id, oa_antecedent_patient_id, oa_antecedent_type, oa_antecedent_drc_id, oa_antecedent_description," & vbCrLf &
        " oa_antecedent_date_debut, oa_antecedent_date_fin, oa_antecedent_arret, oa_antecedent_arret_commentaire, oa_antecedent_nature," & vbCrLf &
        " oa_antecedent_niveau, oa_antecedent_id_niveau1, oa_antecedent_id_niveau2, oa_antecedent_ordre_affichage1, oa_antecedent_ordre_affichage2, oa_antecedent_ordre_affichage3," & vbCrLf &
        " oa_antecedent_statut_affichage, oa_antecedent_categorie_contexte, oa_antecedent_inactif, oa_antecedent_diagnostic, oa_antecedent_ald_id, oa_antecedent_ald_cim_10_id," & vbCrLf &
        " oa_antecedent_ald_valide, oa_antecedent_ald_date_debut, oa_antecedent_ald_date_fin, oa_antecedent_ald_demande_en_cours, oa_antecedent_ald_demande_date, oa_chaine_episode_date_fin)" & vbCrLf &
        " VALUES (@dateHistorisation, @utilisateurHistorisation, @etatHistorisation, @antecedentId, @patientId, @type, @drcId, @description, @dateDebut," & vbCrLf &
        " @dateFin, @arret, @arretCommentaire, @nature, @niveau, @antecedentId1, @antecedentId2, @ordreAffichage1, @ordreAffichage2, @ordreAffichage3, @statutAffichage," & vbCrLf &
        " @categorie, @inactif, @diagnostic, @aldId, @aldCim10Id, @aldValide, @alddateDebut, @aldDateFin, @aldDemandeEnCours, @aldDateDemande, @chaineEpisodeDateFin);", conxn)


        With cmd.Parameters
            .AddWithValue("@dateHistorisation", DateTime.Now().ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@utilisateurHistorisation", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@etatHistorisation", EtatHistorisation.ToString)
            .AddWithValue("@antecedentId", (AntecedentHistoACreer.AntecedentId.ToString))
            .AddWithValue("@patientId", (AntecedentHistoACreer.PatientId.ToString))
            .AddWithValue("@type", (AntecedentHistoACreer.Type))
            .AddWithValue("@drcId", (AntecedentHistoACreer.DrcId.ToString))
            .AddWithValue("@description", (AntecedentHistoACreer.Description))
            .AddWithValue("@dateDebut", (AntecedentHistoACreer.DateDebut.ToString("yyyy-MM-dd HH:mm:ss")))
            .AddWithValue("@dateFin", (AntecedentHistoACreer.DateFin.ToString("yyyy-MM-dd HH:mm:ss")))
            .AddWithValue("@arret", (If(AntecedentHistoACreer.Arret = True, "1", "0")))
            .AddWithValue("@arretCommentaire", N2N(Of String)(AntecedentHistoACreer.ArretCommentaire, ""))
            .AddWithValue("@nature", AntecedentHistoACreer.Nature)
            .AddWithValue("@niveau", AntecedentHistoACreer.Niveau)
            .AddWithValue("@antecedentId1", AntecedentHistoACreer.Niveau1Id)
            .AddWithValue("@antecedentId2", (AntecedentHistoACreer.Niveau2Id))
            .AddWithValue("@ordreAffichage1", (AntecedentHistoACreer.Ordre1.ToString))
            .AddWithValue("@ordreAffichage2", (AntecedentHistoACreer.Ordre2.ToString))
            .AddWithValue("@ordreAffichage3", (AntecedentHistoACreer.Ordre3.ToString))
            .AddWithValue("@statutAffichage", (AntecedentHistoACreer.StatutAffichage.ToString))
            .AddWithValue("@categorie", N2N(Of String)(AntecedentHistoACreer.Categorie, ""))
            .AddWithValue("@inactif", (If(AntecedentHistoACreer.Inactif = True, "1", "0")))
            .AddWithValue("@diagnostic", (AntecedentHistoACreer.Diagnostic.ToString()))
            .AddWithValue("@aldId", AntecedentHistoACreer.AldId)
            .AddWithValue("@aldCim10Id", AntecedentHistoACreer.AldCim10Id)
            .AddWithValue("@aldValide", AntecedentHistoACreer.AldValide)
            .AddWithValue("@alddateDebut", (AntecedentHistoACreer.AldDateDebut))
            .AddWithValue("@alddateFin", (AntecedentHistoACreer.AldDateFin))
            .AddWithValue("@aldDemandeEnCours", If(AntecedentHistoACreer.AldDemandeEnCours = True, "1", "0"))
            .AddWithValue("@aldDateDemande", (AntecedentHistoACreer.AldDateDemande))
            .AddWithValue("@chaineEpisodeDateFin", Date.Now().AddMonths(Coalesce(ConfigurationManager.AppSettings("ChaineEpisodePeriode"), 0))) 'AntecedentHistoACreer.ChaineEpisodeDateFin)
        End With
        Debug.WriteLine(GetSqlCommandTextForLogs(cmd))
        Try
            conxn.Open()
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        Return codeRetour
    End Function

    Public Shared Function InitAntecedentHistorisation(antecedent As Antecedent, UtilisateurConnecte As Utilisateur, AntecedentHistoACreer As AntecedentHisto)
        AntecedentHistoACreer.HistorisationDate = Date.Now()
        AntecedentHistoACreer.UtilisateurId = UtilisateurConnecte.UtilisateurId
        AntecedentHistoACreer.Etat = 0
        AntecedentHistoACreer.AntecedentId = antecedent.Id
        AntecedentHistoACreer.PatientId = antecedent.PatientId
        AntecedentHistoACreer.Type = antecedent.Type
        AntecedentHistoACreer.DrcId = antecedent.DrcId
        AntecedentHistoACreer.Description = antecedent.Description
        AntecedentHistoACreer.DateCreation = antecedent.DateCreation
        AntecedentHistoACreer.UtilisateurCreation = antecedent.UserCreation
        AntecedentHistoACreer.DateModification = antecedent.DateModification
        AntecedentHistoACreer.UtilisateurModification = antecedent.UserModification
        AntecedentHistoACreer.Diagnostic = antecedent.Diagnostic
        AntecedentHistoACreer.DateDebut = antecedent.DateDebut
        AntecedentHistoACreer.DateFin = antecedent.DateFin
        AntecedentHistoACreer.Arret = antecedent.Arret
        AntecedentHistoACreer.ArretCommentaire = antecedent.ArretCommentaire
        AntecedentHistoACreer.Nature = antecedent.Nature
        AntecedentHistoACreer.Niveau = antecedent.Niveau
        AntecedentHistoACreer.Niveau1Id = antecedent.Niveau1Id
        AntecedentHistoACreer.Niveau2Id = antecedent.Niveau2Id
        AntecedentHistoACreer.Ordre1 = antecedent.Ordre1
        AntecedentHistoACreer.Ordre2 = antecedent.Ordre2
        AntecedentHistoACreer.Ordre3 = antecedent.Ordre3
        AntecedentHistoACreer.StatutAffichage = antecedent.StatutAffichage
        AntecedentHistoACreer.Categorie = antecedent.CategorieContexte
        AntecedentHistoACreer.Inactif = antecedent.Inactif
        AntecedentHistoACreer.AldId = antecedent.AldId
        AntecedentHistoACreer.AldCim10Id = antecedent.AldCim10Id
        AntecedentHistoACreer.AldValide = antecedent.AldValide
        AntecedentHistoACreer.AldDateDebut = antecedent.AldDateDebut
        AntecedentHistoACreer.AldDateFin = antecedent.AldDateFin
        AntecedentHistoACreer.AldDemandeEnCours = antecedent.AldDemandeEnCours
        AntecedentHistoACreer.AldDateDemande = antecedent.AldDateDemande
        AntecedentHistoACreer.ChaineEpisodeDateFin = antecedent.ChaineEpisodeDateFin
    End Function

    Public Shared Function InitClasseAntecedentHistorisation(antecedentDataReader As SqlDataReader, UtilisateurConnecte As Utilisateur, AntecedentHistoACreer As AntecedentHisto)
        'Initialisation classe Historisation antecedent
        AntecedentHistoACreer.HistorisationDate = Date.Now()
        AntecedentHistoACreer.UtilisateurId = UtilisateurConnecte.UtilisateurId
        AntecedentHistoACreer.Etat = 0
        AntecedentHistoACreer.AntecedentId = antecedentDataReader("oa_antecedent_id")
        AntecedentHistoACreer.PatientId = antecedentDataReader("oa_antecedent_patient_id")
        AntecedentHistoACreer.Type = antecedentDataReader("oa_antecedent_type")
        AntecedentHistoACreer.DrcId = antecedentDataReader("oa_antecedent_drc_id")
        If antecedentDataReader("oa_antecedent_description") Is DBNull.Value Then
            AntecedentHistoACreer.Description = ""
        Else
            AntecedentHistoACreer.Description = antecedentDataReader("oa_antecedent_description")
        End If
        If antecedentDataReader("oa_antecedent_date_creation") Is DBNull.Value Then
            AntecedentHistoACreer.DateCreation = New Date(1, 1, 1, 0, 0, 0)
        Else
            AntecedentHistoACreer.DateCreation = antecedentDataReader("oa_antecedent_date_creation")
        End If
        If antecedentDataReader("oa_antecedent_utilisateur_creation") Is DBNull.Value Then
            AntecedentHistoACreer.UtilisateurCreation = 0
        Else
            AntecedentHistoACreer.UtilisateurCreation = antecedentDataReader("oa_antecedent_utilisateur_creation")
        End If
        If antecedentDataReader("oa_antecedent_date_modification") Is DBNull.Value Then
            AntecedentHistoACreer.DateModification = New Date(1, 1, 1, 0, 0, 0)
        Else
            AntecedentHistoACreer.DateModification = antecedentDataReader("oa_antecedent_date_modification")
        End If
        If antecedentDataReader("oa_antecedent_utilisateur_modification") Is DBNull.Value Then
            AntecedentHistoACreer.UtilisateurModification = 0
        Else
            AntecedentHistoACreer.UtilisateurModification = antecedentDataReader("oa_antecedent_utilisateur_modification")
        End If
        If antecedentDataReader("oa_antecedent_diagnostic") Is DBNull.Value Then
            AntecedentHistoACreer.Diagnostic = 0
        Else
            AntecedentHistoACreer.Diagnostic = antecedentDataReader("oa_antecedent_diagnostic")
        End If
        If antecedentDataReader("oa_antecedent_date_debut") Is DBNull.Value Then
            AntecedentHistoACreer.DateDebut = New Date(1, 1, 1, 0, 0, 0)
        Else
            AntecedentHistoACreer.DateDebut = antecedentDataReader("oa_antecedent_date_debut")
        End If
        If antecedentDataReader("oa_antecedent_date_fin") Is DBNull.Value Then
            AntecedentHistoACreer.DateFin = New Date(1, 1, 1, 0, 0, 0)
        Else
            AntecedentHistoACreer.DateFin = antecedentDataReader("oa_antecedent_date_fin")
        End If
        If antecedentDataReader("oa_antecedent_arret") Is DBNull.Value Then
            AntecedentHistoACreer.Arret = False
        Else
            AntecedentHistoACreer.Arret = antecedentDataReader("oa_antecedent_arret")
        End If
        If antecedentDataReader("oa_antecedent_arret_commentaire") Is DBNull.Value Then
            AntecedentHistoACreer.ArretCommentaire = ""
        Else
            AntecedentHistoACreer.ArretCommentaire = antecedentDataReader("oa_antecedent_arret_commentaire")
        End If
        If antecedentDataReader("oa_antecedent_nature") Is DBNull.Value Then
            AntecedentHistoACreer.Nature = ""
        Else
            AntecedentHistoACreer.Nature = antecedentDataReader("oa_antecedent_nature")
        End If
        If antecedentDataReader("oa_antecedent_niveau") Is DBNull.Value Then
            AntecedentHistoACreer.Niveau = 0
        Else
            AntecedentHistoACreer.Niveau = antecedentDataReader("oa_antecedent_niveau")
        End If
        If antecedentDataReader("oa_antecedent_id_niveau1") Is DBNull.Value Then
            AntecedentHistoACreer.Niveau1Id = 0
        Else
            AntecedentHistoACreer.Niveau1Id = antecedentDataReader("oa_antecedent_id_niveau1")
        End If
        If antecedentDataReader("oa_antecedent_id_niveau2") Is DBNull.Value Then
            AntecedentHistoACreer.Niveau2Id = 0
        Else
            AntecedentHistoACreer.Niveau2Id = antecedentDataReader("oa_antecedent_id_niveau2")
        End If
        If antecedentDataReader("oa_antecedent_ordre_affichage1") Is DBNull.Value Then
            AntecedentHistoACreer.Ordre1 = 0
        Else
            AntecedentHistoACreer.Ordre1 = antecedentDataReader("oa_antecedent_ordre_affichage1")
        End If
        If antecedentDataReader("oa_antecedent_ordre_affichage2") Is DBNull.Value Then
            AntecedentHistoACreer.Ordre2 = 0
        Else
            AntecedentHistoACreer.Ordre2 = antecedentDataReader("oa_antecedent_ordre_affichage2")
        End If
        If antecedentDataReader("oa_antecedent_ordre_affichage3") Is DBNull.Value Then
            AntecedentHistoACreer.Ordre3 = 0
        Else
            AntecedentHistoACreer.Ordre3 = antecedentDataReader("oa_antecedent_ordre_affichage3")
        End If
        If antecedentDataReader("oa_antecedent_statut_affichage") Is DBNull.Value Then
            AntecedentHistoACreer.StatutAffichage = ""
        Else
            AntecedentHistoACreer.StatutAffichage = antecedentDataReader("oa_antecedent_statut_affichage")
        End If
        If antecedentDataReader("oa_antecedent_categorie_contexte") Is DBNull.Value Then
            AntecedentHistoACreer.Categorie = ""
        Else
            AntecedentHistoACreer.Categorie = antecedentDataReader("oa_antecedent_categorie_contexte")
        End If
        If antecedentDataReader("oa_antecedent_inactif") Is DBNull.Value Then
            AntecedentHistoACreer.Inactif = False
        Else
            AntecedentHistoACreer.Inactif = antecedentDataReader("oa_antecedent_inactif")
        End If
        AntecedentHistoACreer.AldId = Coalesce(antecedentDataReader("oa_antecedent_ald_id"), 0)
        AntecedentHistoACreer.AldCim10Id = Coalesce(antecedentDataReader("oa_antecedent_ald_cim_10_id"), 0)
        AntecedentHistoACreer.AldValide = Coalesce(antecedentDataReader("oa_antecedent_ald_valide"), False)
        AntecedentHistoACreer.AldDateDebut = Coalesce(antecedentDataReader("oa_antecedent_ald_date_debut"), Date.Now())
        AntecedentHistoACreer.AldDateFin = Coalesce(antecedentDataReader("oa_antecedent_ald_date_fin"), Date.Now())
        AntecedentHistoACreer.AldDemandeEnCours = Coalesce(antecedentDataReader("oa_antecedent_ald_demande_en_cours"), False)
        AntecedentHistoACreer.AldDateDemande = Coalesce(antecedentDataReader("oa_antecedent_ald_demande_date"), Date.Now())
        AntecedentHistoACreer.ChaineEpisodeDateFin = Coalesce(antecedentDataReader("oa_chaine_episode_date_fin"), Nothing)
    End Function

End Class
