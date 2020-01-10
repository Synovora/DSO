Imports System.Data.SqlClient

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
        Dim SQLString As String = "select oa_traitement_id, oa_traitement_medicament_dci, oa_traitement_arret, oa_traitement_posologie_base," &
        " oa_traitement_posologie_rythme, oa_traitement_posologie_matin, oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi," &
        " oa_traitement_posologie_soir, oa_traitement_posologie_commentaire, oa_traitement_ordre_affichage, oa_traitement_date_debut," &
        " oa_traitement_date_fin, oa_traitement_arret_commentaire, oa_traitement_allergie, oa_traitement_contre_indication" &
        " from oasis.oa_traitement where (oa_traitement_annulation Is Null Or oa_traitement_annulation = '')" &
        " And oa_traitement_contre_indication = '1' and oa_traitement_patient_id = " & patientId.ToString &
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

    Public Function getAllTraitementAllergiebyPatient(patientId As Integer) As DataTable
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

    Public Function getAllTraitementbyPatient(patientId As Integer, filtreDateFin As Date) As DataTable
        Dim SQLString As String = "select oa_traitement_id, oa_traitement_medicament_cis, oa_traitement_medicament_dci," &
        " oa_traitement_arret, oa_traitement_posologie_base, oa_traitement_posologie_rythme, oa_traitement_posologie_matin," &
        " oa_traitement_posologie_midi, oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir," &
        " oa_traitement_posologie_commentaire, oa_traitement_ordre_affichage, oa_traitement_date_debut, oa_traitement_date_fin," &
        " oa_traitement_arret_commentaire, oa_traitement_annulation, oa_traitement_annulation_commentaire," &
        " oa_traitement_declaratif_hors_traitement, oa_traitement_allergie, oa_traitement_contre_indication" &
        " from oasis.oa_traitement where oa_traitement_patient_id = " & patientId.ToString &
        " And oa_traitement_date_fin >= '" & filtreDateFin.ToString("yyyy-MM-dd") & "' order by oa_traitement_date_fin desc;"

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
        " oa_traitement_posologie_apres_midi, oa_traitement_posologie_soir, oa_traitement_posologie_commentaire, oa_traitement_ordre_affichage, oa_traitement_date_creation," &
        " oa_traitement_commentaire, oa_traitement_date_modification, oa_traitement_date_debut, oa_traitement_date_fin, oa_traitement_fenetre," &
        " oa_traitement_fenetre_date_debut, oa_traitement_fenetre_date_fin, oa_traitement_arret, oa_traitement_allergie," &
        " oa_traitement_contre_indication FROM oasis.oa_traitement" &
        " WHERE (oa_traitement_annulation Is Null OR oa_traitement_annulation = '')" &
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
        traitement.MedicamentCis = Coalesce(reader("oa_traitement_medicament_cis"), "")
        traitement.MedicamentDci = Coalesce(reader("oa_traitement_medicament_dci"), "")
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

End Class
