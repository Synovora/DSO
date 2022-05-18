Imports System.Configuration
Imports System.Data.SqlClient

Public Module TraitementHistoDao
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

    Public Function GetAllHistoTraitementbyId(traitementId As Integer) As DataTable
        Dim SQLString As String = "SELECT * FROM oasis.oa_traitement_histo WHERE oa_traitement_id = '" + traitementId.ToString + "' ORDER BY oa_traitement_histo_id DESC;"

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
        Dim conxn As New SqlConnection(GetConnectionString())
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim dateCreation As DateTime = Date.Now.Date
        Dim Fenetre, Declaratif, allergie, contreIndication As Integer

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

        Dim SQLstring As String = "INSERT INTO oasis.oa_traitement_histo" &
        " (oa_traitement_histo_date_historisation, oa_traitement_histo_utilisateur_historisation, oa_traitement_histo_etat_historisation," &
        " oa_traitement_patient_id, oa_traitement_id, oa_traitement_date_debut, oa_traitement_date_fin," &
        " oa_traitement_ordre_affichage, oa_traitement_posologie_base, oa_traitement_posologie_rythme," &
        " oa_traitement_posologie_matin, oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir," &
        " oa_traitement_fraction_matin, oa_traitement_fraction_midi, oa_traitement_fraction_apres_midi, oa_traitement_fraction_soir," &
        " oa_traitement_posologie_commentaire, oa_traitement_fenetre, oa_traitement_fenetre_date_debut," &
        " oa_traitement_fenetre_date_fin, oa_traitement_fenetre_commentaire, oa_traitement_commentaire," &
        " oa_traitement_arret_commentaire, oa_traitement_declaratif_hors_traitement, oa_traitement_allergie," &
        " oa_traitement_contre_indication, oa_traitement_annulation_commentaire, oa_traitement_annulation, oa_traitement_arret, oa_traitement_medicament_cis, oa_traitement_medicament_dci)" &
        " VALUES (@dateHistorisation, @utilisateurCreation, @etatHistorisation," &
        " @patientId, @traitementId, @dateDebut, @dateFin," &
        " @ordreAffichage, @posologieBase, @posologieRythme," &
        " @PosologieMatin, @PosologieMidi, @PosologieApresMidi, @PosologieSoir," &
        " @FractionMatin, @FractionMidi, @FractionApresMidi, @FractionSoir," &
        " @posologieCommentaire, @fenetre, @fenetreDateDebut," &
        " @fenetreDateFin, @fenetreCommentaire, @commentaire," &
        " @arretCommentaire, @declaratif, @allergie," &
        " @contreIndication, @annulationCommentaire, @annulation, @arret, @cis, @dci);"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@dateHistorisation", DateTime.Now().ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@utilisateurCreation", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@etatHistorisation", EtatHistorisation.ToString)
            .AddWithValue("@patientId", TraitementHistoACreer.HistorisationPatientId.ToString)
            .AddWithValue("@traitementId", TraitementHistoACreer.HistorisationTraitementId.ToString)
            .AddWithValue("@dateDebut", TraitementHistoACreer.HistorisationDateDebut.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@dateFin", TraitementHistoACreer.HistorisationDateFin.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@ordreAffichage", TraitementHistoACreer.HistorisationOrdreAffichage.ToString)
            .AddWithValue("@posologieBase", TraitementHistoACreer.HistorisationPosologieBase.ToString)
            .AddWithValue("@posologieRythme", TraitementHistoACreer.HistorisationPosologieRythme.ToString)
            .AddWithValue("@posologieMatin", TraitementHistoACreer.HistorisationPosologieMatin.ToString)
            .AddWithValue("@posologieMidi", TraitementHistoACreer.HistorisationPosologieMidi.ToString)
            .AddWithValue("@posologieApresMidi", TraitementHistoACreer.HistorisationPosologieApresMidi.ToString)
            .AddWithValue("@posologieSoir", TraitementHistoACreer.HistorisationPosologieSoir.ToString)
            .AddWithValue("@FractionMatin", TraitementHistoACreer.HistorisationFractionMatin)
            .AddWithValue("@FractionMidi", TraitementHistoACreer.HistorisationFractionMidi)
            .AddWithValue("@FractionApresMidi", TraitementHistoACreer.HistorisationFractionApresMidi)
            .AddWithValue("@FractionSoir", TraitementHistoACreer.HistorisationFractionSoir)
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
            .AddWithValue("@cis", TraitementHistoACreer.HistorisationMedicamentId)
            .AddWithValue("@dci", TraitementHistoACreer.HistorisationMedicamentDci)
        End With

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

    Public Sub InitClasseTraitementHistorisation(traitement As Traitement, UtilisateurConnecte As Utilisateur, TraitementHistoACreer As TraitementHisto)
        'Initialisation classe Historisation traitement 
        TraitementHistoACreer.HistorisationDate = Date.Now()
        TraitementHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
        TraitementHistoACreer.HistorisationEtat = 0
        TraitementHistoACreer.HistorisationPatientId = traitement.PatientId
        TraitementHistoACreer.HistorisationTraitementId = traitement.TraitementId
        TraitementHistoACreer.HistorisationMedicamentId = traitement.MedicamentId
        TraitementHistoACreer.HistorisationMedicamentDci = traitement.MedicamentDci
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
        TraitementHistoACreer.HistorisationFractionMatin = traitement.FractionMatin
        TraitementHistoACreer.HistorisationFractionMidi = traitement.FractionMidi
        TraitementHistoACreer.HistorisationFractionApresMidi = traitement.FractionApresMidi
        TraitementHistoACreer.HistorisationFractionSoir = traitement.FractionSoir
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
        TraitementHistoACreer.HistorisationMedicamentId = Coalesce(traitementDataReader("oa_traitement_medicament_cis"), "")
        TraitementHistoACreer.HistorisationMedicamentDci = Coalesce(traitementDataReader("oa_traitement_medicament_dci"), "")
        TraitementHistoACreer.HistorisationCommentaire = Coalesce(traitementDataReader("oa_traitement_commentaire"), "")
        TraitementHistoACreer.HistorisationOrdreAffichage = Coalesce(traitementDataReader("oa_traitement_ordre_affichage"), 0)
        TraitementHistoACreer.HistorisationPosologieBase = Coalesce(traitementDataReader("oa_traitement_posologie_base"), "")
        TraitementHistoACreer.HistorisationPosologieRythme = Coalesce(traitementDataReader("oa_traitement_posologie_rythme"), 0)
        TraitementHistoACreer.HistorisationPosologieMatin = Coalesce(traitementDataReader("oa_traitement_posologie_matin"), 0)
        TraitementHistoACreer.HistorisationPosologieMidi = Coalesce(traitementDataReader("oa_traitement_posologie_midi"), 0)
        TraitementHistoACreer.HistorisationPosologieApresMidi = Coalesce(traitementDataReader("oa_traitement_posologie_apres_midi"), 0)
        TraitementHistoACreer.HistorisationPosologieSoir = Coalesce(traitementDataReader("oa_traitement_posologie_soir"), 0)
        TraitementHistoACreer.HistorisationFractionMatin = Coalesce(traitementDataReader("oa_traitement_fraction_matin"), Traitement.EnumFraction.Non)
        TraitementHistoACreer.HistorisationFractionMidi = Coalesce(traitementDataReader("oa_traitement_fraction_midi"), Traitement.EnumFraction.Non)
        TraitementHistoACreer.HistorisationFractionApresMidi = Coalesce(traitementDataReader("oa_traitement_fraction_apres_midi"), Traitement.EnumFraction.Non)
        TraitementHistoACreer.HistorisationFractionSoir = Coalesce(traitementDataReader("oa_traitement_fraction_soir"), Traitement.EnumFraction.Non)
        TraitementHistoACreer.HistorisationPosologieCommentaire = Coalesce(traitementDataReader("oa_traitement_posologie_commentaire"), "")
        TraitementHistoACreer.HistorisationFenetre = Coalesce(traitementDataReader("oa_traitement_fenetre"), False)
        TraitementHistoACreer.HistorisationFenetreDateDebut = Coalesce(traitementDataReader("oa_traitement_fenetre_date_debut"), New Date(1, 1, 1, 0, 0, 0))
        TraitementHistoACreer.HistorisationFenetreDateFin = Coalesce(traitementDataReader("oa_traitement_fenetre_date_fin"), New Date(1, 1, 1, 0, 0, 0))
        TraitementHistoACreer.HistorisationFenetreCommentaire = Coalesce(traitementDataReader("oa_traitement_fenetre_commentaire"), "")
        TraitementHistoACreer.HistorisationArret = Coalesce(traitementDataReader("oa_traitement_arret"), "")
        TraitementHistoACreer.HistorisationArretCommentaire = Coalesce(traitementDataReader("oa_traitement_arret_commentaire"), "")
        TraitementHistoACreer.HistorisationDeclaratifHorsTraitement = Coalesce(traitementDataReader("oa_traitement_declaratif_hors_traitement"), False)
        TraitementHistoACreer.HistorisationAllergie = Coalesce(traitementDataReader("oa_traitement_allergie"), False)
        TraitementHistoACreer.HistorisationContreIndication = Coalesce(traitementDataReader("oa_traitement_contre_indication"), False)
        TraitementHistoACreer.HistorisationAnnulation = Coalesce(traitementDataReader("oa_traitement_annulation"), "")
        TraitementHistoACreer.HistorisationAnnulationCommentaire = Coalesce(traitementDataReader("oa_traitement_annulation_commentaire"), "")
    End Sub

    Private Function getConnection()

        Dim strConnect As String = ConfigurationManager.ConnectionStrings("Oasis_WF.My.MySettings.oasisConnection").ConnectionString

        Dim conn As SqlConnection = New SqlConnection(strConnect)
        conn.Open()
        Return conn
    End Function

End Module
