Imports System.Configuration
Imports System.Data.SqlClient

Module TraitementHistoDao
    Public Enum EnumEtatTraitementHisto
        CreationTraitement = 1
        ModificationTraitement = 2
        ArretTraitement = 3
        AnnulationTraitement = 4
        CreationFenetreTherapeutique = 5
        ModificationFenetreTherapeutique = 6
        SuppressionFenetreTherapeutique = 7
        SuppressionTraitement = 8
    End Enum

    Public Function getAllHistoTraitementbyId(traitementId As Integer) As DataTable
        Dim SQLString As String = "select * from oasis.oa_traitement_histo where oa_traitement_id = '" + traitementId.ToString + "' order by oa_traitement_histo_id desc;"

        Using con As SqlConnection = getConnection()
            Dim TraitementHistoDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using TraitementHistoDataAdapter
                TraitementHistoDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim TraitementHistoDataTable As DataTable = New DataTable()
                Using TraitementHistoDataTable
                    Try
                        TraitementHistoDataAdapter.Fill(TraitementHistoDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return TraitementHistoDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function CreationTraitementHisto(TraitementHistoACreer As TraitementHisto, UtilisateurConnecte As Utilisateur, EtatHistorisation As Integer) As Boolean
        Dim conxn As New SqlConnection(getConnectionString())
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim dateCreation As DateTime = Date.Now.Date
        Dim PosologieMatin, PosologieMidi, PosologieApresMidi, PosologieSoir, Fenetre, Declaratif, allergie, contreIndication As Integer

        If TraitementHistoACreer.HistorisationPosologieMatin = True Then
            PosologieMatin = 1
        Else
            PosologieMatin = 0
        End If
        If TraitementHistoACreer.HistorisationPosologieMidi = True Then
            PosologieMidi = 1
        Else
            PosologieMidi = 0
        End If
        If TraitementHistoACreer.HistorisationPosologieApresMidi = True Then
            PosologieApresMidi = 1
        Else
            PosologieApresMidi = 0
        End If
        If TraitementHistoACreer.HistorisationPosologieSoir = True Then
            PosologieSoir = 1
        Else
            PosologieSoir = 0
        End If
        If TraitementHistoACreer.HistorisationFenetre = True Then
            Fenetre = 1
        Else
            Fenetre = 0
        End If
        If TraitementHistoACreer.HistorisationDeclaratifHorsTraitement = True Then
            Declaratif = 1
        Else
            Declaratif = 0
        End If
        If TraitementHistoACreer.HistorisationAllergie = True Then
            allergie = 1
        Else
            allergie = 0
        End If
        If TraitementHistoACreer.HistorisationContreIndication = True Then
            contreIndication = 1
        Else
            contreIndication = 0
        End If

        Dim SQLstring As String = "insert into oasis.oa_traitement_histo (oa_traitement_histo_date_historisation, oa_traitement_histo_utilisateur_historisation, oa_traitement_histo_etat_historisation, oa_traitement_patient_id, oa_traitement_id, oa_traitement_date_debut, oa_traitement_date_fin, oa_traitement_ordre_affichage, oa_traitement_posologie_base, oa_traitement_posologie_rythme, oa_traitement_posologie_matin, oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir, oa_traitement_posologie_commentaire, oa_traitement_fenetre, oa_traitement_fenetre_date_debut, oa_traitement_fenetre_date_fin, oa_traitement_fenetre_commentaire, oa_traitement_commentaire, oa_traitement_arret_commentaire, oa_traitement_declaratif_hors_traitement, oa_traitement_allergie, oa_traitement_contre_indication, oa_traitement_annulation_commentaire, oa_traitement_annulation, oa_traitement_arret) VALUES (@dateHistorisation, @utilisateurCreation, @etatHistorisation, @patientId, @traitementId, @dateDebut, @dateFin, @ordreAffichage, @posologieBase, @posologieRythme, @PosologieMatin, @PosologieMidi, @PosologieApresMidi, @PosologieSoir, @posologieCommentaire, @fenetre, @fenetreDateDebut, @fenetreDateFin, @fenetreCommentaire, @commentaire, @arretCommentaire, @declaratif, @allergie, @contreIndication, @annulationCommentaire, @annulation, @arret);"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@dateHistorisation", DateTime.Now())
            .AddWithValue("@utilisateurCreation", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@etatHistorisation", EtatHistorisation.ToString)
            .AddWithValue("@patientId", TraitementHistoACreer.HistorisationPatientId.ToString)
            .AddWithValue("@traitementId", TraitementHistoACreer.HistorisationTraitementId.ToString)
            .AddWithValue("@dateDebut", TraitementHistoACreer.HistorisationDateDebut.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@dateFin", TraitementHistoACreer.HistorisationDateFin.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@ordreAffichage", TraitementHistoACreer.HistorisationOrdreAffichage.ToString)
            .AddWithValue("@posologieBase", TraitementHistoACreer.HistorisationPosologieBase.ToString)
            .AddWithValue("@posologieRythme", TraitementHistoACreer.HistorisationPosologieRythme.ToString)
            .AddWithValue("@posologieMatin", PosologieMatin.ToString)
            .AddWithValue("@posologieMidi", PosologieMidi.ToString)
            .AddWithValue("@posologieApresMidi", PosologieApresMidi.ToString)
            .AddWithValue("@posologieSoir", PosologieSoir.ToString)
            .AddWithValue("@posologieCommentaire", TraitementHistoACreer.HistorisationPosologieCommentaire.ToString)
            .AddWithValue("@fenetre", Fenetre.ToString)
            .AddWithValue("@fenetreDateDebut", TraitementHistoACreer.HistorisationFenetreDateDebut.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@fenetreDateFin", TraitementHistoACreer.HistorisationFenetreDateFin.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@fenetreCommentaire", TraitementHistoACreer.HistorisationFenetreCommentaire.ToString)
            .AddWithValue("@commentaire", TraitementHistoACreer.HistorisationCommentaire.ToString)
            .AddWithValue("@arret", TraitementHistoACreer.HistorisationArret.ToString)
            .AddWithValue("@arretCommentaire", TraitementHistoACreer.HistorisationArretCommentaire.ToString)
            .AddWithValue("@declaratif", Declaratif.ToString)
            .AddWithValue("@allergie", allergie)
            .AddWithValue("@contreIndication", contreIndication.ToString)
            .AddWithValue("@annulation", TraitementHistoACreer.HistorisationAnnulation.ToString)
            .AddWithValue("@annulationCommentaire", TraitementHistoACreer.HistorisationAnnulationCommentaire.ToString)
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

    Public Sub InitClasseTraitementHistorisation(traitement As Traitement, UtilisateurConnecte As Utilisateur, TraitementHistoACreer As TraitementHisto)
        'Initialisation classe Historisation traitement 
        TraitementHistoACreer.HistorisationDate = Date.Now()
        TraitementHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
        TraitementHistoACreer.HistorisationEtat = 0
        TraitementHistoACreer.HistorisationPatientId = traitement.PatientId
        TraitementHistoACreer.HistorisationTraitementId = traitement.TraitementId
        TraitementHistoACreer.HistorisationDateDebut = traitement.DateDebut
        TraitementHistoACreer.HistorisationDateFin = traitement.DateFin
        TraitementHistoACreer.HistorisationCommentaire = traitement.Commentaire
        TraitementHistoACreer.HistorisationOrdreAffichage = traitement.OrdreAffichage
        TraitementHistoACreer.HistorisationPosologieBase = traitement.PosologieBase
        TraitementHistoACreer.HistorisationPosologieRythme = traitement.PosologieRythme
        TraitementHistoACreer.HistorisationPosologieMatin = traitement.PosologieMatin
        TraitementHistoACreer.HistorisationPosologieMidi = traitement.PosologieMidi
        TraitementHistoACreer.HistorisationPosologieApresMidi = traitement.PosologieApresMidi
        TraitementHistoACreer.HistorisationPosologieSoir = traitement.PosologieSoir
        TraitementHistoACreer.HistorisationPosologieCommentaire = traitement.PosologieCommentaire
        TraitementHistoACreer.HistorisationFenetre = traitement.Fenetre
        TraitementHistoACreer.HistorisationFenetreDateDebut = traitement.DateDebut
        TraitementHistoACreer.HistorisationFenetreDateFin = traitement.FenetreDateFin
        TraitementHistoACreer.HistorisationFenetreCommentaire = traitement.FenetreCommentaire
        TraitementHistoACreer.HistorisationArret = traitement.Arret
        TraitementHistoACreer.HistorisationArretCommentaire = traitement.ArretCommentaire
        TraitementHistoACreer.HistorisationDeclaratifHorsTraitement = traitement.DeclaratifHorsTraitement
        TraitementHistoACreer.HistorisationAllergie = traitement.Allergie
        TraitementHistoACreer.HistorisationContreIndication = traitement.ContreIndication
        TraitementHistoACreer.HistorisationAnnulation = traitement.Annulation
        TraitementHistoACreer.HistorisationAnnulationCommentaire = traitement.AnnulationCommentaire
    End Sub

    Public Sub InitClasseTraitementHistorisationBdd(traitementDataReader As SqlDataReader, UtilisateurConnecte As Utilisateur, TraitementHistoACreer As TraitementHisto)
        'Initialisation classe Historisation traitement 
        TraitementHistoACreer.HistorisationDate = Date.Now()
        TraitementHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
        TraitementHistoACreer.HistorisationEtat = 0
        TraitementHistoACreer.HistorisationPatientId = traitementDataReader("oa_traitement_Patient_id")
        TraitementHistoACreer.HistorisationTraitementId = traitementDataReader("oa_traitement_id")
        TraitementHistoACreer.HistorisationDateDebut = traitementDataReader("oa_traitement_date_debut")
        TraitementHistoACreer.HistorisationDateFin = traitementDataReader("oa_traitement_date_fin")
        If traitementDataReader("oa_traitement_commentaire") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationCommentaire = ""
        Else
            TraitementHistoACreer.HistorisationCommentaire = traitementDataReader("oa_traitement_commentaire")
        End If
        TraitementHistoACreer.HistorisationOrdreAffichage = traitementDataReader("oa_traitement_ordre_affichage")
        TraitementHistoACreer.HistorisationPosologieBase = traitementDataReader("oa_traitement_posologie_base")
        TraitementHistoACreer.HistorisationPosologieRythme = traitementDataReader("oa_traitement_posologie_rythme")
        If traitementDataReader("oa_traitement_posologie_matin") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationPosologieMatin = False
        Else
            TraitementHistoACreer.HistorisationPosologieMatin = traitementDataReader("oa_traitement_posologie_matin")
        End If
        If traitementDataReader("oa_traitement_posologie_midi") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationPosologieMidi = False
        Else
            TraitementHistoACreer.HistorisationPosologieMidi = traitementDataReader("oa_traitement_posologie_midi")
        End If
        If traitementDataReader("oa_traitement_posologie_apres_midi") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationPosologieApresMidi = False
        Else
            TraitementHistoACreer.HistorisationPosologieApresMidi = traitementDataReader("oa_traitement_posologie_apres_midi")
        End If
        If traitementDataReader("oa_traitement_posologie_soir") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationPosologieSoir = False
        Else
            TraitementHistoACreer.HistorisationPosologieSoir = traitementDataReader("oa_traitement_posologie_soir")
        End If
        If traitementDataReader("oa_traitement_posologie_commentaire") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationPosologieCommentaire = ""
        Else
            TraitementHistoACreer.HistorisationPosologieCommentaire = traitementDataReader("oa_traitement_posologie_commentaire")
        End If
        If traitementDataReader("oa_traitement_fenetre") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationFenetre = False
        Else
            TraitementHistoACreer.HistorisationFenetre = traitementDataReader("oa_traitement_fenetre")
        End If
        If traitementDataReader("oa_traitement_fenetre_date_debut") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationFenetreDateDebut = New Date(1, 1, 1, 0, 0, 0)
        Else
            TraitementHistoACreer.HistorisationFenetreDateDebut = traitementDataReader("oa_traitement_fenetre_date_debut")
        End If
        If traitementDataReader("oa_traitement_fenetre_date_fin") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationFenetreDateFin = New Date(1, 1, 1, 0, 0, 0)
        Else
            TraitementHistoACreer.HistorisationFenetreDateFin = traitementDataReader("oa_traitement_fenetre_date_fin")
        End If

        If traitementDataReader("oa_traitement_fenetre_commentaire") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationFenetreCommentaire = ""
        Else
            TraitementHistoACreer.HistorisationFenetreCommentaire = traitementDataReader("oa_traitement_fenetre_commentaire")
        End If
        If traitementDataReader("oa_traitement_arret") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationArret = ""
        Else
            TraitementHistoACreer.HistorisationArret = traitementDataReader("oa_traitement_arret")
        End If
        If traitementDataReader("oa_traitement_arret_commentaire") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationArretCommentaire = ""
        Else
            TraitementHistoACreer.HistorisationArretCommentaire = traitementDataReader("oa_traitement_arret_commentaire")
        End If
        If traitementDataReader("oa_traitement_declaratif_hors_traitement") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationDeclaratifHorsTraitement = False
        Else
            TraitementHistoACreer.HistorisationDeclaratifHorsTraitement = traitementDataReader("oa_traitement_declaratif_hors_traitement")
        End If
        If traitementDataReader("oa_traitement_allergie") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationAllergie = False
        Else
            TraitementHistoACreer.HistorisationAllergie = traitementDataReader("oa_traitement_allergie")
        End If
        If traitementDataReader("oa_traitement_contre_indication") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationContreIndication = False
        Else
            TraitementHistoACreer.HistorisationContreIndication = traitementDataReader("oa_traitement_contre_indication")
        End If
        If traitementDataReader("oa_traitement_annulation") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationAnnulation = ""
        Else
            TraitementHistoACreer.HistorisationAnnulation = traitementDataReader("oa_traitement_annulation")
        End If
        If traitementDataReader("oa_traitement_annulation_commentaire") Is DBNull.Value Then
            TraitementHistoACreer.HistorisationAnnulationCommentaire = ""
        Else
            TraitementHistoACreer.HistorisationAnnulationCommentaire = traitementDataReader("oa_traitement_annulation_commentaire")
        End If
    End Sub

    Private Function getConnection()

        Dim strConnect As String = ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnection").ConnectionString

        Dim conn As SqlConnection = New SqlConnection(strConnect)
        conn.Open()
        Return conn
    End Function

End Module
