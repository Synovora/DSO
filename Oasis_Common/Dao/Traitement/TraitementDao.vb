Imports System.Data.SqlClient

Public Class TraitementDao
    Inherits StandardDao

    ReadOnly patientDao As New PatientDao

    Public Function GetBaseCodeByItem(Item As String) As String
        Dim Code As String
        Select Case Item
            Case Traitement.EnumBaseItem.JOURNALIER
                Code = Traitement.EnumBaseCode.JOURNALIER
            Case Traitement.EnumBaseItem.HEBDOMADAIRE
                Code = Traitement.EnumBaseCode.HEBDOMADAIRE
            Case Traitement.EnumBaseItem.MENSUEL
                Code = Traitement.EnumBaseCode.MENSUEL
            Case Traitement.EnumBaseItem.ANNUEL
                Code = Traitement.EnumBaseCode.ANNUEL
            Case Traitement.EnumBaseItem.CONDITIONNEL
                Code = Traitement.EnumBaseCode.CONDITIONNEL
            Case Else
                Code = ""
        End Select

        Return Code
    End Function

    Public Function GetBseItemByCode(Code As String) As String
        Dim Item As String
        Select Case Code
            Case Traitement.EnumBaseCode.JOURNALIER
                Item = Traitement.EnumBaseItem.JOURNALIER
            Case Traitement.EnumBaseCode.HEBDOMADAIRE
                Item = Traitement.EnumBaseItem.HEBDOMADAIRE
            Case Traitement.EnumBaseCode.MENSUEL
                Item = Traitement.EnumBaseItem.MENSUEL
            Case Traitement.EnumBaseCode.ANNUEL
                Item = Traitement.EnumBaseItem.ANNUEL
            Case Traitement.EnumBaseCode.CONDITIONNEL
                Item = Traitement.EnumBaseItem.CONDITIONNEL
            Case Else
                Item = ""
        End Select

        Return Item
    End Function

    Public Function GetTraitementById(traitementId As Integer) As Traitement
        Dim traitement As Traitement
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_traitement WHERE oa_traitement_id = @id"
            command.Parameters.AddWithValue("@id", traitementId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    traitement = BuildBean(reader)
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

    Private Function BuildBean(reader As SqlDataReader) As Traitement
        Dim traitement As New Traitement With {
            .TraitementId = reader("oa_traitement_id"),
            .PatientId = Coalesce(reader("oa_traitement_patient_id"), ""),
            .MedicamentId = Coalesce(reader("oa_traitement_medicament_cis"), ""),
            .MedicamentDci = Coalesce(reader("oa_traitement_medicament_dci"), ""),
            .ClasseAtc = Coalesce(reader("oa_traitement_classe_atc"), ""),
            .DenominationLongue = Coalesce(reader("oa_traitement_denomination_longue"), ""),
            .UserCreation = Coalesce(reader("oa_traitement_identifiant_creation"), 0),
            .DateCreation = Coalesce(reader("oa_traitement_date_creation"), Nothing),
            .UserModification = Coalesce(reader("oa_traitement_identifiant_modification"), 0),
            .DateModification = Coalesce(reader("oa_traitement_date_modification"), Nothing),
            .DateDebut = Coalesce(reader("oa_traitement_date_debut"), Nothing),
            .DateFin = Coalesce(reader("oa_traitement_date_fin"), Nothing),
            .OrdreAffichage = Coalesce(reader("oa_traitement_ordre_affichage"), 0),
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
            .Fenetre = Coalesce(reader("oa_traitement_fenetre"), False),
            .FenetreDateDebut = Coalesce(reader("oa_traitement_fenetre_date_debut"), Nothing),
            .FenetreDateFin = Coalesce(reader("oa_traitement_fenetre_date_fin"), Nothing),
            .FenetreCommentaire = Coalesce(reader("oa_traitement_fenetre_commentaire"), ""),
            .Commentaire = Coalesce(reader("oa_traitement_commentaire"), ""),
            .Arret = Coalesce(reader("oa_traitement_arret"), ""),
            .ArretCommentaire = Coalesce(reader("oa_traitement_arret_commentaire"), ""),
            .Allergie = Coalesce(reader("oa_traitement_allergie"), False),
            .ContreIndication = Coalesce(reader("oa_traitement_contre_indication"), False),
            .DeclaratifHorsTraitement = Coalesce(reader("oa_traitement_declaratif_hors_traitement"), False),
            .Annulation = Coalesce(reader("oa_traitement_annulation"), ""),
            .AnnulationCommentaire = Coalesce(reader("oa_traitement_annulation_commentaire"), "")
        }
        Return traitement
    End Function

    Public Function GetBaseDescription(base As String) As String
        Dim baseString As String
        Select Case base
            Case Traitement.EnumBaseCode.CONDITIONNEL
                baseString = "Conditionnel : "
            Case Traitement.EnumBaseCode.HEBDOMADAIRE
                baseString = "Hebdo : "
            Case Traitement.EnumBaseCode.MENSUEL
                baseString = "Mensuel : "
            Case Traitement.EnumBaseCode.ANNUEL
                baseString = "Annuel : "
            Case Else
                baseString = "Base inconnue ! "
        End Select

        Return baseString
    End Function


    Public Function GetAllTraitementCIbyPatient(patientId As Integer) As DataTable
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

    Public Function GetTraitementsEnCoursbyPatient(patientId As Integer) As List(Of Traitement)
        Dim con As SqlConnection = GetConnection()
        Dim traitements As List(Of Traitement) = New List(Of Traitement)
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText =
                "SELECT * FROM oasis.oa_traitement WHERE (oa_traitement_annulation Is Null Or oa_traitement_annulation = '')" &
                " AND (oa_traitement_date_fin >= CONVERT(DATE, GETDATE()))" &
                " AND (oa_traitement_arret is Null OR oa_traitement_arret <> 'A')" &
                " AND (oa_traitement_allergie is Null OR oa_traitement_allergie = 'False')" &
                " AND (oa_traitement_contre_indication is Null OR oa_traitement_contre_indication = 'False')" &
                " AND oa_traitement_patient_id = @patienId" &
                " ORDER BY oa_traitement_ordre_affichage;"
            command.Parameters.AddWithValue("@patienId", patientId)
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    traitements.Add(BuildBean(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return traitements
    End Function

    Public Function GetTraitementEnCoursbyPatient(patientId As Integer) As DataTable
        Dim SQLString As String = "SELECT oa_traitement_id, oa_traitement_medicament_cis, oa_traitement_medicament_dci, oa_traitement_denomination_longue," &
        " oa_traitement_posologie_base, oa_traitement_posologie_rythme, oa_traitement_posologie_matin, oa_traitement_posologie_midi," &
        " oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir," &
        " oa_traitement_fraction_matin, oa_traitement_fraction_midi, oa_traitement_fraction_apres_midi, oa_traitement_fraction_soir," &
        " oa_traitement_posologie_commentaire, oa_traitement_ordre_affichage, oa_traitement_date_creation," &
        " oa_traitement_commentaire, oa_traitement_date_modification, oa_traitement_date_debut, oa_traitement_date_fin, oa_traitement_fenetre," &
        " oa_traitement_fenetre_date_debut, oa_traitement_fenetre_date_fin, oa_traitement_arret, oa_traitement_allergie, oa_traitement_classe_atc," &
        " oa_traitement_contre_indication FROM oasis.oa_traitement" &
        " WHERE (oa_traitement_annulation Is Null Or oa_traitement_annulation = '')" &
        " AND (oa_traitement_date_fin >= CONVERT(DATE, GETDATE()))" &
        " AND (oa_traitement_arret is Null OR oa_traitement_arret <> 'A')" &
        " AND (oa_traitement_allergie is Null OR oa_traitement_allergie = 'False')" &
        " AND (oa_traitement_contre_indication is Null OR oa_traitement_contre_indication = 'False')" &
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

    Public Function GetAllTraitementArreteByPatient(patientId As Integer) As DataTable
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

    Public Function ModificationTraitement(traitement As Traitement, traitementHistoACreer As TraitementHisto, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "UPDATE oasis.oa_traitement SET" &
        " oa_traitement_medicament_cis = @cis," &
        " oa_traitement_medicament_dci = @dci," &
        " oa_traitement_denomination_longue = @denominationLongue," &
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
            .AddWithValue("@cis", traitement.MedicamentId.ToString)
            .AddWithValue("@dci", traitement.MedicamentDci)
            .AddWithValue("@denominationLongue", traitement.DenominationLongue)
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
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation traitement
            traitementHistoACreer.HistorisationDate = Date.Now()
            traitementHistoACreer.HistorisationUtilisateurId = traitement.UserModification
            traitementHistoACreer.HistorisationEtat = EnumEtatTraitementHisto.ModificationTraitement
            traitementHistoACreer.HistorisationMedicamentId = traitement.MedicamentId
            traitementHistoACreer.HistorisationMedicamentDci = traitement.MedicamentDci
            traitementHistoACreer.HistorisationOrdreAffichage = traitement.OrdreAffichage
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
            patientDao.ModificationDateMajSynthesePatient(traitement.PatientId, userLog)
        End If

        Return codeRetour
    End Function

    Public Function CreationTraitement(traitement As Traitement, traitementHistoACreer As TraitementHisto, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim traitementId As Long

        Dim SQLstring As String = "INSERT INTO oasis.oa_traitement" &
        " (oa_traitement_patient_id, oa_traitement_medicament_cis, oa_traitement_medicament_dci, oa_traitement_denomination_longue, oa_traitement_allergie," &
        " oa_traitement_contre_indication, oa_traitement_identifiant_creation, oa_traitement_identifiant_modification," &
        " oa_traitement_ordre_affichage, oa_traitement_posologie_base, oa_traitement_posologie_rythme," &
        " oa_traitement_posologie_matin, oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir," &
        " oa_traitement_fraction_matin,oa_traitement_fraction_midi, oa_traitement_fraction_apres_midi, oa_traitement_fraction_soir," &
        " oa_traitement_date_creation, oa_traitement_posologie_commentaire, oa_traitement_commentaire," &
        " oa_traitement_date_debut, oa_traitement_date_fin, oa_traitement_medicament_monographie, oa_traitement_classe_atc)" &
        " VALUES (@patientId, @cis, @dci, @denominationLongue, @allergie," &
        " @contreIndication, @utilisateurCreation, @utilisateurModification," &
        " @ordreAffichage, @posologieBase, @posologierythme," &
        " @PosologieMatin, @PosologieMidi, @PosologieApresMidi, @PosologieSoir," &
        " @FractionMatin, @FractionMidi, @FractionApresMidi, @FractionSoir," &
        " @dateCreation, @posologieCommentaire, @traitementCommentaire, @dateDebut, @dateFin, @monographie, @classeAtc); SELECT SCOPE_IDENTITY()"

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
            .AddWithValue("@monographie", traitement.MedicamentMonographie)
            .AddWithValue("@classeAtc", traitement.ClasseAtc)
        End With

        Try
            da.InsertCommand = cmd
            traitementId = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données dans l'instance de la classe Historisation traitement
            traitementHistoACreer.HistorisationTraitementId = traitementId 'Récupération du traitement créé
            traitementHistoACreer.HistorisationDate = DateTime.Now()
            traitementHistoACreer.HistorisationUtilisateurId = userLog.UtilisateurId
            traitementHistoACreer.HistorisationMedicamentId = traitement.MedicamentId
            traitementHistoACreer.HistorisationMedicamentDci = traitement.MedicamentDci
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

            Dim traitementDao As TraitementDao = New TraitementDao
            Dim traitementCree As Traitement

            Try
                traitementCree = traitementDao.GetTraitementById(traitementHistoACreer.HistorisationTraitementId)
            Catch ex As Exception
                Throw New Exception("Traitement : " + ex.Message + "Problème")
                Return False
            End Try
            InitClasseTraitementHistorisation(traitementCree, userLog, traitementHistoACreer)


            'Création dans l'historique des traitements du traitement créé
            CreationTraitementHisto(traitementHistoACreer, userLog, EnumEtatTraitementHisto.CreationTraitement)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            patientDao.ModificationDateMajSynthesePatient(traitement.PatientId, userLog)
        End If

        Return codeRetour
    End Function

    Public Function AnnulationTraitement(traitement As Traitement, traitementHistoACreer As TraitementHisto, userLog As Utilisateur) As Boolean
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
            Throw New Exception(ex.Message)
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
            patientDao.ModificationDateMajSynthesePatient(traitement.PatientId, userLog)
        End If

        Return codeRetour
    End Function

    Public Function ArretTraitement(traitement As Traitement, traitementHistoACreer As TraitementHisto, userLog As Utilisateur) As Boolean
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
            Throw New Exception(ex.Message)
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
            patientDao.ModificationDateMajSynthesePatient(traitement.PatientId, userLog)
        End If

        Return codeRetour
    End Function


    Public Function SuppressionTraitement(traitement As Traitement, traitementHistoACreer As TraitementHisto, userLog As Utilisateur) As Boolean
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
            Throw New Exception(ex.Message)
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
            patientDao.ModificationDateMajSynthesePatient(traitement.PatientId, userLog)
        End If

        Return codeRetour
    End Function

    Public Function DeclarationTraitementAllergieOuCI(traitement As Traitement, userLog As Utilisateur) As Boolean
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
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        'Mise à jour de la date de mise à jour de la synthèse (table patient)
        patientDao.ModificationDateMajSynthesePatient(traitement.PatientId, userLog)

        Return codeRetour
    End Function

    Public Function GetListOfTraitementPatient(patientId As Integer) As List(Of TraitementCourrier)
        Dim ListTraitement As New List(Of TraitementCourrier)
        Dim traitementDao As New TraitementDao
        Dim dt As DataTable
        dt = traitementDao.GetTraitementEnCoursbyPatient(patientId)

        Dim rowCount As Integer = dt.Rows.Count - 1
        For i = 0 To rowCount Step 1
            Dim traitementCourrier As New TraitementCourrier
            traitementCourrier.PatientId = patientId
            traitementCourrier.TraitementId = dt.Rows(i)("oa_traitement_id")
            traitementCourrier.Denomination = dt.Rows(i)("oa_traitement_medicament_dci")

            Dim Base As String
            Dim Posologie As String
            Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
            Dim Rythme As Integer
            Dim FenetreDateDebut, FenetreDateFin As Date
            Dim dateFin As Date
            Dim FenetreTherapeutiqueEnCours As Boolean

            'Exclusion de l'affichage des traitements dont la date de fin est < à la date du jour
            'Cette condition est traitée en exclusion (et non dans la requête SQL) pour stocker les allergies et les contre-indications dans la StringCollection quel que soit leur date de fin
            'Date de fin
            If dt.Rows(i)("oa_traitement_date_fin") IsNot DBNull.Value Then
                dateFin = dt.Rows(i)("oa_traitement_date_fin")
            Else
                dateFin = "31/12/2999"
            End If
            Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
            Dim dateFinaComparer As New Date(dateFin.Year, dateFin.Month, dateFin.Day, 0, 0, 0)
            If (dateFinaComparer < dateJouraComparer) Then
                Continue For
            End If

            'Vérification de l'existence d'une fenêtre thérapeutique active et à venir
            FenetreTherapeutiqueEnCours = False

            'Formatage de la posologie
            Posologie = ""

            'Existence d'une fenêtre thérapeutique
            Dim dateDebutFenetreaComparer As New Date(FenetreDateDebut.Year, FenetreDateDebut.Month, FenetreDateDebut.Day, 0, 0, 0)
            Dim dateFinFenetreaComparer As New Date(FenetreDateFin.Year, FenetreDateFin.Month, FenetreDateFin.Day, 0, 0, 0)
            If dt.Rows(i)("oa_traitement_fenetre") IsNot DBNull.Value Then
                If dt.Rows(i)("oa_traitement_fenetre") = "1" Then
                    'Fenêtre thérapeutique en cours, à venir ou obsolète
                    If FenetreDateDebut <= dateJouraComparer And FenetreDateFin >= dateJouraComparer Then
                        'Fenêtre thérapeutique en cours
                        FenetreTherapeutiqueEnCours = True
                        Posologie = "Fenêtre Th."
                    End If
                End If
            End If

            If FenetreTherapeutiqueEnCours = False Then
                Dim PosologieMatinString, PosologieMidiString, PosologieApresMidiString, PosologieSoirString As String
                Dim FractionMatin, FractionMidi, FractionApresMidi, FractionSoir As String
                Dim PosologieBase As String

                FractionMatin = Coalesce(dt.Rows(i)("oa_traitement_fraction_matin"), Traitement.EnumFraction.Non)
                FractionMidi = Coalesce(dt.Rows(i)("oa_traitement_fraction_midi"), Traitement.EnumFraction.Non)
                FractionApresMidi = Coalesce(dt.Rows(i)("oa_traitement_fraction_apres_midi"), Traitement.EnumFraction.Non)
                FractionSoir = Coalesce(dt.Rows(i)("oa_traitement_fraction_soir"), Traitement.EnumFraction.Non)

                posologieMatin = Coalesce(dt.Rows(i)("oa_traitement_Posologie_matin"), 0)
                posologieMidi = Coalesce(dt.Rows(i)("oa_traitement_Posologie_midi"), 0)
                posologieApresMidi = Coalesce(dt.Rows(i)("oa_traitement_Posologie_apres_midi"), 0)
                posologieSoir = Coalesce(dt.Rows(i)("oa_traitement_Posologie_soir"), 0)

                PosologieBase = Coalesce(dt.Rows(i)("oa_traitement_Posologie_base"), "")

                If FractionMatin <> "" AndAlso FractionMatin <> Traitement.EnumFraction.Non Then
                    If posologieMatin <> 0 Then
                        PosologieMatinString = posologieMatin.ToString & "+" & FractionMatin
                    Else
                        PosologieMatinString = FractionMatin
                    End If
                Else
                    If posologieMatin <> 0 Then
                        PosologieMatinString = posologieMatin.ToString
                    Else
                        PosologieMatinString = "0"
                    End If
                End If

                If FractionMidi <> "" AndAlso FractionMidi <> Traitement.EnumFraction.Non Then
                    If posologieMidi <> 0 Then
                        PosologieMidiString = posologieMidi.ToString & "+" & FractionMidi
                    Else
                        PosologieMidiString = FractionMidi
                    End If
                Else
                    If posologieMidi <> 0 Then
                        PosologieMidiString = posologieMidi.ToString
                    Else
                        PosologieMidiString = "0"
                    End If
                End If

                PosologieApresMidiString = ""
                If FractionApresMidi <> "" AndAlso FractionApresMidi <> Traitement.EnumFraction.Non Then
                    If posologieApresMidi <> 0 Then
                        PosologieApresMidiString = posologieApresMidi.ToString & "+" & FractionApresMidi
                    Else
                        PosologieApresMidiString = FractionApresMidi
                    End If
                Else
                    If posologieApresMidi <> 0 Then
                        PosologieApresMidiString = posologieApresMidi.ToString
                    End If
                End If

                If FractionSoir <> "" AndAlso FractionSoir <> Traitement.EnumFraction.Non Then
                    If posologieSoir <> 0 Then
                        PosologieSoirString = posologieSoir.ToString & "+" & FractionSoir
                    Else
                        PosologieSoirString = FractionSoir
                    End If
                Else
                    If posologieSoir <> 0 Then
                        PosologieSoirString = posologieSoir.ToString
                    Else
                        PosologieSoirString = "0"
                    End If
                End If
                If dt.Rows(i)("oa_traitement_posologie_base") IsNot DBNull.Value Then
                    Rythme = dt.Rows(i)("oa_traitement_posologie_rythme")
                    Select Case PosologieBase
                        Case Traitement.EnumBaseCode.JOURNALIER
                            Base = ""
                            If posologieApresMidi <> 0 OrElse FractionApresMidi <> Traitement.EnumFraction.Non Then
                                Posologie = Base + PosologieMatinString + ". " + PosologieMidiString + ". " + PosologieApresMidiString + ". " + PosologieSoirString
                            Else
                                Posologie = Base + " " + PosologieMatinString + ". " + PosologieMidiString + ". " + PosologieSoirString
                            End If
                        Case Else
                            Dim RythmeString As String = ""
                            If FractionMatin <> "" AndAlso FractionMatin <> Traitement.EnumFraction.Non Then
                                If Rythme <> 0 Then
                                    RythmeString = Rythme.ToString & "+" & FractionMatin
                                Else
                                    RythmeString = FractionMatin
                                End If
                            Else
                                If Rythme <> 0 Then
                                    RythmeString = Rythme.ToString
                                End If
                            End If

                            Base = GetBaseDescription(dt.Rows(i)("oa_traitement_posologie_base"))
                            Posologie = Base + RythmeString
                    End Select
                End If
            End If

            traitementCourrier.Posologie = Posologie

            ListTraitement.Add(traitementCourrier)
        Next

        Return ListTraitement
    End Function
End Class
