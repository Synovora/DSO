Imports System.Data.SqlClient
Imports Oasis_Common
Public Class TraitementDao
    Inherits StandardDao

    Public Structure EnumBaseCode
        Const JOURNALIER = "J"
        Const HEBDOMADAIRE = "H"
        Const MENSUEL = "M"
        Const ANNUEL = "A"
        Const CONDITIONNEL = "C"
    End Structure

    Public Structure EnumBaseItem
        Const JOURNALIER = "Journalier"
        Const HEBDOMADAIRE = "Hebdomadaire"
        Const MENSUEL = "Mensuel"
        Const ANNUEL = "Annuel"
        Const CONDITIONNEL = "Conditionnel"
    End Structure

    Public Structure EnumFraction
        Const Non = "0"
        Const Quart = "1/4"
        Const Demi = "1/2"
        Const TroisQuart = "3/4"
    End Structure

    Friend Function GetBaseCodeByItem(Item As String) As String
        Dim Code As String
        Select Case Item
            Case EnumBaseItem.JOURNALIER
                Code = EnumBaseCode.JOURNALIER
            Case EnumBaseItem.HEBDOMADAIRE
                Code = EnumBaseCode.HEBDOMADAIRE
            Case EnumBaseItem.MENSUEL
                Code = EnumBaseCode.MENSUEL
            Case EnumBaseItem.ANNUEL
                Code = EnumBaseCode.ANNUEL
            Case EnumBaseItem.CONDITIONNEL
                Code = EnumBaseCode.CONDITIONNEL
            Case Else
                Code = ""
        End Select

        Return Code
    End Function

    Friend Function GetBseItemByCode(Code As String) As String
        Dim Item As String
        Select Case Code
            Case EnumBaseCode.JOURNALIER
                Item = EnumBaseItem.JOURNALIER
            Case EnumBaseCode.HEBDOMADAIRE
                Item = EnumBaseItem.HEBDOMADAIRE
            Case EnumBaseCode.MENSUEL
                Item = EnumBaseItem.MENSUEL
            Case EnumBaseCode.ANNUEL
                Item = EnumBaseItem.ANNUEL
            Case EnumBaseCode.CONDITIONNEL
                Item = EnumBaseItem.CONDITIONNEL
            Case Else
                Item = ""
        End Select

        Return Item
    End Function


    Public Function getAllTraitementCIbyPatient(patientId As Integer) As DataTable
        Dim SQLString As String = "SELECT oa_traitement_id, oa_traitement_medicament_dci, oa_traitement_arret, oa_traitement_posologie_base," &
        " oa_traitement_posologie_rythme, oa_traitement_posologie_matin, oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi," &
        " oa_traitement_posologie_soir, oa_traitement_posologie_commentaire, oa_traitement_ordre_affichage, oa_traitement_date_debut," &
        " oa_traitement_date_fin, oa_traitement_arret_commentaire, oa_traitement_allergie, oa_traitement_contre_indication" &
        " FROM oasis.oa_traitement" &
        " WHERE (oa_traitement_annulation IS Null Or oa_traitement_annulation = '')" &
        " AND oa_traitement_contre_indication = '1'" &
        " AND oa_traitement_patient_id = " & patientId.ToString &
        " ORDER BY oa_traitement_date_fin DESC;"

        Using con As SqlConnection = GetConnection()
            Dim TraitementDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using TraitementDataAdapter
                TraitementDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim TraitementDataTable As DataTable = New DataTable()
                Using TraitementDataTable
                    Try
                        TraitementDataAdapter.Fill(TraitementDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return TraitementDataTable
                End Using
            End Using
        End Using
    End Function


    'Obsolète ===================== à supprimer !!!!!!!!!!!!!!!!!!!!!!
    Public Function GetAllTraitementAllergiebyPatient(patientId As Integer) As DataTable
        Dim SQLString As String = "select oa_traitement_id, oa_traitement_medicament_dci, oa_traitement_arret, oa_traitement_posologie_base," &
        " oa_traitement_posologie_rythme, oa_traitement_posologie_matin, oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi," &
        " oa_traitement_posologie_soir, oa_traitement_posologie_commentaire, oa_traitement_ordre_affichage, oa_traitement_date_debut," &
        " oa_traitement_date_fin, oa_traitement_arret_commentaire, oa_traitement_allergie, oa_traitement_contre_indication" &
        " from oasis.oa_traitement where (oa_traitement_annulation Is Null Or oa_traitement_annulation = '')" &
        " And oa_traitement_allergie = '1' and oa_traitement_patient_id = " & patientId.ToString &
        " order by oa_traitement_date_fin desc;"


        Using con As SqlConnection = GetConnection()
            Dim TraitementDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using TraitementDataAdapter
                TraitementDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim TraitementDataTable As DataTable = New DataTable()
                Using TraitementDataTable
                    Try
                        TraitementDataAdapter.Fill(TraitementDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return TraitementDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function GetAllTraitementObsoletebyPatient(patientId As Integer, filtreDateFin As Date) As DataTable

        Dim DateJourSuivant As Date = Date.Now().AddDays(1)

        Dim SQLString As String = "SELECT oa_traitement_id, oa_traitement_medicament_cis, oa_traitement_medicament_dci," &
        " oa_traitement_arret, oa_traitement_posologie_base, oa_traitement_posologie_rythme, oa_traitement_posologie_matin," &
        " oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir," &
        " oa_traitement_posologie_commentaire, oa_traitement_ordre_affichage, oa_traitement_date_debut, oa_traitement_date_fin," &
        " oa_traitement_arret_commentaire, oa_traitement_annulation, oa_traitement_annulation_commentaire," &
        " oa_traitement_declaratif_hors_traitement, oa_traitement_allergie, oa_traitement_contre_indication" &
        " FROM oasis.oa_traitement" &
        " WHERE oa_traitement_patient_id = " & patientId.ToString &
        " AND oa_traitement_date_fin >= '" & filtreDateFin.ToString("yyyy-MM-dd") & "'" &
        " AND oa_traitement_date_fin < '" & DateJourSuivant.ToString("yyyy-MM-dd") & "'" &
        " ORDER BY oa_traitement_date_fin desc;"

        Using con As SqlConnection = GetConnection()
            Dim TraitementDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using TraitementDataAdapter
                TraitementDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim TraitementDataTable As DataTable = New DataTable()
                Using TraitementDataTable
                    Try
                        TraitementDataAdapter.Fill(TraitementDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return TraitementDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function getTraitementNotCancelledbyPatient(patientId As Integer) As DataTable
        Dim SQLString As String = "SELECT oa_traitement_id, oa_traitement_medicament_cis, oa_traitement_medicament_dci," &
        " oa_traitement_posologie_base, oa_traitement_posologie_rythme, oa_traitement_posologie_matin, oa_traitement_posologie_midi," &
        " oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir," &
        " oa_traitement_fraction_matin, oa_traitement_fraction_midi, oa_traitement_fraction_apres_midi, oa_traitement_fraction_soir," &
        " oa_traitement_posologie_commentaire, oa_traitement_ordre_affichage, oa_traitement_date_creation," &
        " oa_traitement_commentaire, oa_traitement_date_modification, oa_traitement_date_debut, oa_traitement_date_fin, oa_traitement_fenetre," &
        " oa_traitement_fenetre_date_debut, oa_traitement_fenetre_date_fin, oa_traitement_arret, oa_traitement_allergie," &
        " oa_traitement_contre_indication FROM oasis.oa_traitement" &
        " WHERE (oa_traitement_annulation Is Null Or oa_traitement_annulation = '')" &
        " AND (oa_traitement_date_fin >= CONVERT(DATE, GETDATE()) OR (oa_traitement_allergie = 'True') OR (oa_traitement_contre_indication = 'True'))" &
        " AND oa_traitement_patient_id = " & patientId.ToString &
        " ORDER BY oa_traitement_ordre_affichage;"

        Using con As SqlConnection = GetConnection()
            Dim TraitementDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using TraitementDataAdapter
                TraitementDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim TraitementDataTable As DataTable = New DataTable()
                Using TraitementDataTable
                    Try
                        TraitementDataAdapter.Fill(TraitementDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return TraitementDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function getAllTraitementArreteByPatient(patientId As Integer) As DataTable
        Dim SQLString As String =
            "SELECT oa_traitement_medicament_dci, oa_traitement_arret_commentaire, oa_traitement_date_modification" &
            " FROM oasis.oa_traitement" &
            " WHERE oa_traitement_arret = 'A'" &
            " AND oa_traitement_patient_id = " & patientId.ToString &
            " ORDER BY oa_traitement_date_modification DESC;"

        Using con As SqlConnection = GetConnection()
            Dim TraitementDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using TraitementDataAdapter
                TraitementDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim TraitementDataTable As DataTable = New DataTable()
                Using TraitementDataTable
                    Try
                        TraitementDataAdapter.Fill(TraitementDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return TraitementDataTable
                End Using
            End Using
        End Using
    End Function

    Friend Function getTraitementById(traitementId As Integer) As Traitement
        Dim traitement As Traitement
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "select * from oasis.oa_traitement where oa_traitement_id = @id"
            command.Parameters.AddWithValue("@id", traitementId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    traitement = buildBean(reader)
                Else
                    Throw New ArgumentException("Traitement inexistant !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return traitement
    End Function

    Private Function buildBean(reader As SqlDataReader) As Traitement
        Dim traitement As New Traitement

        traitement.TraitementId = reader("oa_traitement_id")
        traitement.PatientId = Coalesce(reader("oa_traitement_patient_id"), "")
        traitement.MedicamentId = Coalesce(reader("oa_traitement_medicament_cis"), "")
        traitement.MedicamentDci = Coalesce(reader("oa_traitement_medicament_dci"), "")
        traitement.DenominationLongue = Coalesce(reader("oa_traitement_denomination_longue"), "")
        traitement.UserCreation = Coalesce(reader("oa_traitement_identifiant_creation"), 0)
        traitement.DateCreation = Coalesce(reader("oa_traitement_date_creation"), Nothing)
        traitement.UserModification = Coalesce(reader("oa_traitement_identifiant_modification"), 0)
        traitement.DateModification = Coalesce(reader("oa_traitement_date_modification"), Nothing)
        traitement.DateDebut = Coalesce(reader("oa_traitement_date_debut"), Nothing)
        traitement.DateFin = Coalesce(reader("oa_traitement_date_fin"), Nothing)
        traitement.OrdreAffichage = Coalesce(reader("oa_traitement_ordre_affichage"), 0)
        traitement.PosologieBase = Coalesce(reader("oa_traitement_posologie_base"), "")
        traitement.PosologieRythme = Coalesce(reader("oa_traitement_posologie_rythme"), 0)
        traitement.PosologieMatin = Coalesce(reader("oa_traitement_posologie_matin"), 0)
        traitement.PosologieMidi = Coalesce(reader("oa_traitement_posologie_midi"), 0)
        traitement.PosologieApresMidi = Coalesce(reader("oa_traitement_posologie_apres_midi"), 0)
        traitement.PosologieSoir = Coalesce(reader("oa_traitement_posologie_soir"), 0)
        traitement.FractionMatin = Coalesce(reader("oa_traitement_fraction_matin"), "")
        traitement.FractionMidi = Coalesce(reader("oa_traitement_fraction_midi"), "")
        traitement.FractionApresMidi = Coalesce(reader("oa_traitement_fraction_apres_midi"), "")
        traitement.FractionSoir = Coalesce(reader("oa_traitement_fraction_soir"), "")
        traitement.PosologieCommentaire = Coalesce(reader("oa_traitement_posologie_commentaire"), "")
        traitement.Fenetre = Coalesce(reader("oa_traitement_fenetre"), False)
        traitement.FenetreDateDebut = Coalesce(reader("oa_traitement_fenetre_date_debut"), Nothing)
        traitement.FenetreDateFin = Coalesce(reader("oa_traitement_fenetre_date_fin"), Nothing)
        traitement.FenetreCommentaire = Coalesce(reader("oa_traitement_fenetre_commentaire"), "")
        traitement.Commentaire = Coalesce(reader("oa_traitement_commentaire"), "")
        traitement.Arret = Coalesce(reader("oa_traitement_arret"), "")
        traitement.ArretCommentaire = Coalesce(reader("oa_traitement_arret_commentaire"), "")
        traitement.Allergie = Coalesce(reader("oa_traitement_allergie"), False)
        traitement.ContreIndication = Coalesce(reader("oa_traitement_contre_indication"), False)
        traitement.DeclaratifHorsTraitement = Coalesce(reader("oa_traitement_declaratif_hors_traitement"), False)
        traitement.Annulation = Coalesce(reader("oa_traitement_annulation"), "")
        traitement.AnnulationCommentaire = Coalesce(reader("oa_traitement_annulation_commentaire"), "")
        Return traitement
    End Function

    Friend Function ModificationTraitement(traitement As Traitement, traitementHistoACreer As TraitementHisto) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "UPDATE oasis.oa_traitement SET" &
        " oa_traitement_identifiant_modification = @utilisateurModification," &
        " oa_traitement_ordre_affichage = @ordreAffichage," &
        " oa_traitement_posologie_base = @posologieBase," &
        " oa_traitement_posologie_rythme = @posologieRythme," &
        " oa_traitement_posologie_matin = @posologieMatin," &
        " oa_traitement_posologie_midi = @posologieMidi," &
        " oa_traitement_posologie_apres_midi = @posologieApresMidi," &
        " oa_traitement_posologie_soir = @posologieSoir," &
        " oa_traitement_fraction_matin = @fractionMatin," &
        " oa_traitement_fraction_midi = @fractionMidi," &
        " oa_traitement_fraction_apres_midi = @fractionApresMidi," &
        " oa_traitement_fraction_soir = @fractionSoir," &
        " oa_traitement_date_modification = @dateModification," &
        " oa_traitement_posologie_commentaire = @posologieCommentaire," &
        " oa_traitement_commentaire = @traitementCommentaire," &
        " oa_traitement_date_debut = @dateDebut," &
        " oa_traitement_date_fin = @dateFin" &
        " WHERE oa_traitement_id = @traitementId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", traitement.UserModification.ToString)
            .AddWithValue("@ordreAffichage", traitement.OrdreAffichage.ToString)
            .AddWithValue("@posologieBase", traitement.PosologieBase)
            .AddWithValue("@posologieRythme", traitement.PosologieRythme.ToString)
            .AddWithValue("@posologieMatin", traitement.PosologieMatin.ToString)
            .AddWithValue("@posologieMidi", traitement.PosologieMidi.ToString)
            .AddWithValue("@posologieApresMidi", traitement.PosologieApresMidi.ToString)
            .AddWithValue("@posologieSoir", traitement.PosologieSoir.ToString)
            .AddWithValue("@fractionMatin", traitement.FractionMatin)
            .AddWithValue("@fractionMidi", traitement.FractionMidi)
            .AddWithValue("@fractionApresMidi", traitement.FractionApresMidi)
            .AddWithValue("@fractionSoir", traitement.FractionSoir)
            .AddWithValue("@dateModification", traitement.DateModification.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@posologieCommentaire", traitement.PosologieCommentaire)
            .AddWithValue("@traitementCommentaire", traitement.Commentaire)
            .AddWithValue("@dateDebut", traitement.DateDebut)
            .AddWithValue("@dateFin", traitement.DateFin)
            .AddWithValue("@traitementId", traitement.TraitementId.ToString)
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

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation traitement
            traitementHistoACreer.HistorisationDate = Date.Now()
            traitementHistoACreer.HistorisationUtilisateurId = traitement.UserModification
            traitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.ModificationTraitement
            traitementHistoACreer.HistorisationOrdreAffichage = traitement.OrdreAffichage
            traitementHistoACreer.HistorisationPosologieBase = traitement.PosologieBase
            traitementHistoACreer.HistorisationPosologieRythme = traitement.PosologieRythme
            traitementHistoACreer.HistorisationPosologieMatin = traitement.PosologieMatin
            traitementHistoACreer.HistorisationPosologieMidi = traitement.PosologieMidi
            traitementHistoACreer.HistorisationPosologieApresMidi = traitement.PosologieApresMidi
            traitementHistoACreer.HistorisationPosologieSoir = traitement.PosologieSoir
            traitementHistoACreer.HistorisationFractionMatin = traitement.FractionMatin
            traitementHistoACreer.HistorisationFractionMidi = traitement.FractionMidi
            traitementHistoACreer.HistorisationFractionApresMidi = traitement.FractionApresMidi
            traitementHistoACreer.HistorisationFractionSoir = traitement.FractionSoir
            traitementHistoACreer.HistorisationPosologieCommentaire = traitement.PosologieCommentaire
            traitementHistoACreer.HistorisationCommentaire = traitement.Commentaire
            traitementHistoACreer.HistorisationDateDebut = traitement.DateDebut
            traitementHistoACreer.HistorisationDateFin = traitement.DateFin

            'Création dans l'historique des modifications de traitement
            CreationTraitementHisto(traitementHistoACreer, userLog, EnumEtatTraitementHisto.ModificationTraitement)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            PatientDao.ModificationDateMajSynthesePatient(traitement.PatientId)
        End If

        Return codeRetour
    End Function

    Friend Function CreationTraitement(traitement As Traitement, traitementHistoACreer As TraitementHisto) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "INSERT INTO oasis.oa_traitement" &
        " (oa_traitement_patient_id, oa_traitement_medicament_cis, oa_traitement_medicament_dci, oa_traitement_denomination_longue, oa_traitement_allergie," &
        " oa_traitement_contre_indication, oa_traitement_identifiant_creation, oa_traitement_identifiant_modification," &
        " oa_traitement_ordre_affichage, oa_traitement_posologie_base, oa_traitement_posologie_rythme," &
        " oa_traitement_posologie_matin, oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir," &
        " oa_traitement_fraction_matin,oa_traitement_fraction_midi, oa_traitement_fraction_apres_midi, oa_traitement_fraction_soir," &
        " oa_traitement_date_creation, oa_traitement_posologie_commentaire, oa_traitement_commentaire," &
        " oa_traitement_date_debut, oa_traitement_date_fin)" &
        " VALUES (@patientId, @cis, @dci, @denominationLongue, @allergie," &
        " @contreIndication, @utilisateurCreation, @utilisateurModification," &
        " @ordreAffichage, @posologieBase, @posologierythme," &
        " @PosologieMatin, @PosologieMidi, @PosologieApresMidi, @PosologieSoir," &
        " @FractionMatin, @FractionMidi, @FractionApresMidi, @FractionSoir," &
        " @dateCreation, @posologieCommentaire, @traitementCommentaire, @dateDebut, @dateFin)"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@patientId", traitement.PatientId.ToString)
            .AddWithValue("@cis", traitement.MedicamentId.ToString)
            .AddWithValue("@dci", traitement.MedicamentDci)
            .AddWithValue("@denominationLongue", traitement.DenominationLongue)
            .AddWithValue("@allergie", 0)
            .AddWithValue("@contreIndication", 0)
            .AddWithValue("@utilisateurCreation", userLog.UtilisateurId.ToString)
            .AddWithValue("@utilisateurModification", 0)
            .AddWithValue("@ordreAffichage", traitement.OrdreAffichage.ToString)
            .AddWithValue("@posologieBase", traitement.PosologieBase)
            .AddWithValue("@posologieRythme", traitement.PosologieRythme.ToString)
            .AddWithValue("@posologieMatin", traitement.PosologieMatin.ToString)
            .AddWithValue("@posologieMidi", traitement.PosologieMidi.ToString)
            .AddWithValue("@posologieApresMidi", traitement.PosologieApresMidi.ToString)
            .AddWithValue("@posologieSoir", traitement.PosologieSoir.ToString)
            .AddWithValue("@fractionMatin", traitement.FractionMatin)
            .AddWithValue("@fractionMidi", traitement.FractionMidi)
            .AddWithValue("@fractionApresMidi", traitement.FractionApresMidi)
            .AddWithValue("@fractionSoir", traitement.FractionSoir)
            .AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@posologieCommentaire", traitement.PosologieCommentaire)
            .AddWithValue("@traitementCommentaire", traitement.Commentaire)
            .AddWithValue("@dateDebut", traitement.DateDebut)
            .AddWithValue("@dateFin", traitement.DateFin)
        End With

        Try
            Dim n As Integer 'Pour récupérer le nombre d'occurences enregistrées
            da.InsertCommand = cmd
            n = da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données dans l'instance de la classe Historisation traitement
            traitementHistoACreer.HistorisationDate = DateTime.Now()
            traitementHistoACreer.HistorisationUtilisateurId = userLog.UtilisateurId
            traitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.CreationTraitement
            traitementHistoACreer.HistorisationOrdreAffichage = traitement.OrdreAffichage
            traitementHistoACreer.HistorisationPosologieBase = traitement.PosologieBase
            traitementHistoACreer.HistorisationPosologieRythme = traitement.PosologieRythme
            traitementHistoACreer.HistorisationPosologieMatin = traitement.PosologieMatin
            traitementHistoACreer.HistorisationPosologieMidi = traitement.PosologieMidi
            traitementHistoACreer.HistorisationPosologieApresMidi = traitement.PosologieApresMidi
            traitementHistoACreer.HistorisationPosologieSoir = traitement.PosologieSoir
            traitementHistoACreer.HistorisationFractionMatin = traitement.FractionMatin
            traitementHistoACreer.HistorisationFractionMidi = traitement.FractionMidi
            traitementHistoACreer.HistorisationFractionApresMidi = traitement.FractionApresMidi
            traitementHistoACreer.HistorisationFractionSoir = traitement.FractionSoir
            traitementHistoACreer.HistorisationPosologieCommentaire = traitement.PosologieCommentaire
            traitementHistoACreer.HistorisationCommentaire = traitement.Commentaire
            traitementHistoACreer.HistorisationDateDebut = traitement.DateDebut
            traitementHistoACreer.HistorisationDateFin = traitement.DateFin
            traitementHistoACreer.HistorisationAllergie = False
            traitementHistoACreer.HistorisationContreIndication = False

            'Récupération de l'identifiant du traitement créé
            Dim traitementLastDataReader As SqlDataReader
            SQLstring = "SELECT MAX(oa_traitement_id) FROM oasis.oa_traitement WHERE oa_traitement_patient_id = " & traitement.PatientId & ";"
            Dim traitementLastCommand As New SqlCommand(SQLstring, con)
            con.Open()
            traitementLastDataReader = traitementLastCommand.ExecuteReader()
            If traitementLastDataReader.HasRows Then
                traitementLastDataReader.Read()
                'Récupération de la clé de l'enregistrement créé
                traitementHistoACreer.HistorisationTraitementId = traitementLastDataReader(0)

                'Libération des ressources d'accès aux données
                con.Close()
                traitementLastCommand.Dispose()
            End If

            Dim traitementDao As TraitementDao = New TraitementDao
            Dim traitementCree As Traitement

            Try
                traitementCree = traitementDao.getTraitementById(traitementHistoACreer.HistorisationTraitementId)
            Catch ex As Exception
                MessageBox.Show("Traitement : " + ex.Message, "Problème", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return False
            End Try
            InitClasseTraitementHistorisation(traitementCree, userLog, traitementHistoACreer)


            'Création dans l'historique des traitements du traitement créé
            CreationTraitementHisto(traitementHistoACreer, userLog, EnumEtatTraitementHisto.CreationTraitement)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            PatientDao.ModificationDateMajSynthesePatient(traitement.PatientId)
        End If

        Return codeRetour
    End Function

    Friend Function AnnulationTraitement(traitement As Traitement, traitementHistoACreer As TraitementHisto) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "UPDATE oasis.oa_traitement SET" &
        " oa_traitement_identifiant_modification = @utilisateurModification," &
        " oa_traitement_date_modification = @dateModification," &
        " oa_traitement_date_fin = @dateFin," &
        " oa_traitement_annulation_commentaire = @commentaireAnnulation," &
        " oa_traitement_annulation = @annulation" &
        " WHERE oa_traitement_id = @traitementId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@dateFin", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@commentaireAnnulation", traitement.AnnulationCommentaire)
            .AddWithValue("@annulation", "A")
            .AddWithValue("@traitementId", traitement.TraitementId.ToString)
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

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            traitementHistoACreer.HistorisationDate = Date.Now()
            traitementHistoACreer.HistorisationUtilisateurId = userLog.UtilisateurId
            traitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.AnnulationTraitement
            traitementHistoACreer.HistorisationAnnulationCommentaire = traitement.AnnulationCommentaire
            traitementHistoACreer.HistorisationAnnulation = "A"

            'Création dans l'historique des modifications de traitement
            CreationTraitementHisto(traitementHistoACreer, userLog, EnumEtatTraitementHisto.AnnulationTraitement)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            PatientDao.ModificationDateMajSynthesePatient(traitement.PatientId)
        End If

        Return codeRetour
    End Function

    Friend Function ArretTraitement(traitement As Traitement, traitementHistoACreer As TraitementHisto) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "UPDATE oasis.oa_traitement SET" &
        " oa_traitement_identifiant_modification = @utilisateurModification," &
        " oa_traitement_date_modification = @dateModification," &
        " oa_traitement_date_fin = @dateFin," &
        " oa_traitement_arret_commentaire = @commentaireArret," &
        " oa_traitement_allergie = @allergie," &
        " oa_traitement_contre_indication = @contreIndication," &
        " oa_traitement_arret = @arret" &
        " WHERE oa_traitement_id = @traitementId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@datefin", traitement.DateFin)
            .AddWithValue("@commentaireArret", traitement.ArretCommentaire)
            .AddWithValue("@allergie", traitement.Allergie)
            .AddWithValue("@contreIndication", traitement.ContreIndication)
            .AddWithValue("@arret", "A")
            .AddWithValue("@traitementId", traitement.TraitementId.ToString)
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

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation traitement
            traitementHistoACreer.HistorisationDate = Date.Now()
            traitementHistoACreer.HistorisationUtilisateurId = userLog.UtilisateurId
            traitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.ArretTraitement
            traitementHistoACreer.HistorisationDateFin = traitement.DateFin
            traitementHistoACreer.HistorisationArretCommentaire = traitement.ArretCommentaire
            traitementHistoACreer.HistorisationAllergie = traitement.Allergie
            traitementHistoACreer.HistorisationContreIndication = traitement.ContreIndication
            traitementHistoACreer.HistorisationArret = "A"

            'Création dans l'historique des modifications de traitement
            CreationTraitementHisto(traitementHistoACreer, userLog, EnumEtatTraitementHisto.ArretTraitement)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            PatientDao.ModificationDateMajSynthesePatient(traitement.PatientId)
        End If

        Return codeRetour
    End Function


    Friend Function SuppressionTraitement(traitement As Traitement, traitementHistoACreer As TraitementHisto) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "DELETE FROM oasis.oa_traitement WHERE oa_traitement_id = @traitementId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@traitementId", traitement.TraitementId.ToString)
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

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation traitement 
            traitementHistoACreer.HistorisationDate = Date.Now()
            traitementHistoACreer.HistorisationUtilisateurId = userLog.UtilisateurId
            traitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.SuppressionTraitement
            traitementHistoACreer.HistorisationAnnulation = "A"

            'Création dans l'historique des modifications de traitement
            CreationTraitementHisto(traitementHistoACreer, userLog, EnumEtatTraitementHisto.SuppressionTraitement)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            PatientDao.ModificationDateMajSynthesePatient(traitement.PatientId)
        End If

        Return codeRetour
    End Function

    Friend Function DeclarationTraitementAllergieOuCI(traitement As Traitement) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "INSERT INTO oasis.oa_traitement" &
        " (oa_traitement_patient_id, oa_traitement_medicament_cis, oa_traitement_medicament_dci, oa_traitement_allergie," &
        " oa_traitement_contre_indication, oa_traitement_arret_commentaire, oa_traitement_identifiant_creation," &
        " oa_traitement_date_creation, oa_traitement_declaratif_hors_traitement)" &
        " VALUES (@patientId, @cis, @dci, @allergie," &
        " @contreIndication, @arretCommentaire, @utilisateurCreation," &
        " @dateCreation, @DeclaratifHorsTraitement)"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@patientId", traitement.PatientId.ToString)
            .AddWithValue("@cis", traitement.MedicamentId.ToString)
            .AddWithValue("@dci", traitement.MedicamentDci)
            .AddWithValue("@allergie", traitement.Allergie)
            .AddWithValue("@contreIndication", traitement.ContreIndication)
            .AddWithValue("@arretCommentaire", traitement.ArretCommentaire)
            .AddWithValue("@utilisateurCreation", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@DeclaratifHorsTraitement", True)
        End With

        Try
            Dim n As Integer 'Pour récupérer le nombre d'occurences enregistrées
            da.InsertCommand = cmd
            n = da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        'Mise à jour de la date de mise à jour de la synthèse (table patient)
        PatientDao.ModificationDateMajSynthesePatient(traitement.PatientId)

        Return codeRetour
    End Function

End Class
