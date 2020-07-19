Imports System.Data.SqlClient
Imports Oasis_Common
Public Class ContexteDao
    Inherits StandardDao

    Dim patientDao As New PatientDao
    Public Structure EnumParcoursBaseItem
        Const Medical = "Médical"
        Const BioEnvironnemental = "Bio-environnemental"
    End Structure

    Public Structure EnumParcoursBaseCode
        Const Medical = "M"
        Const BioEnvironnemental = "B"
    End Structure

    Public Enum EnumDiagnostic
        SUSPICION_DE = 2
        NOTION_DE = 3
    End Enum

    Public Function GetContexteObsolete() As DataTable
        Dim SQLString As String

        SQLString = "SELECT * FROM oasis.oa_antecedent" &
        " WHERE oa_antecedent_type = 'C'" &
        " AND (oa_antecedent_statut_affichage = 'P' OR oa_antecedent_statut_affichage = 'C')" &
        " AND (oa_antecedent_inactif = '0' OR oa_antecedent_inactif is Null)" &
        " AND oa_antecedent_date_fin < GETDATE()"

        Using con As SqlConnection = GetConnection()
            Dim ContexteDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ContexteDataAdapter
                ContexteDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim ContexteDataTable As DataTable = New DataTable()
                Using ContexteDataTable
                    Try
                        ContexteDataAdapter.Fill(ContexteDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return ContexteDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function ExistContexteValideWithDrcId(patientId As Long, drcId As Long) As Boolean
        Dim con As SqlConnection
        con = GetConnection()
        Dim IsExist As Boolean = False

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT (1) FROM oasis.oa_antecedent" &
                " WHERE oa_antecedent_type = 'C'" &
                " AND (oa_antecedent_inactif = '0' OR oa_antecedent_inactif is Null)" &
                " AND oa_antecedent_patient_id = @patientId" &
                " AND oa_antecedent_drc_id = @drcId" &
                " AND (oa_antecedent_arret = '0' OR oa_antecedent_arret is Null)" &
                " AND oa_antecedent_date_fin >= GETDATE()"

            With command.Parameters
                .AddWithValue("@patientId", patientId)
                .AddWithValue("@drcId", drcId)
            End With

            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    IsExist = True
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return IsExist
    End Function

    Public Function TransformationEnAntecedent(contexteId As Integer, ContexteHistoACreer As AntecedentHisto, Description As String, Publication As String, user As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLString As String = "UPDATE oasis.oa_antecedent SET" &
            " oa_antecedent_type = 'A'," &
            " oa_antecedent_description = @description," &
            " oa_antecedent_date_modification = @dateModification," &
            " oa_antecedent_utilisateur_modification = @utilisateurModification," &
            " oa_antecedent_date_fin = @dateFin," &
            " oa_antecedent_nature = @nature," &
            " oa_antecedent_priorite = @priorite," &
            " oa_antecedent_statut_affichage = @publication," &
            " oa_antecedent_niveau = @niveau," &
            " oa_antecedent_id_niveau1 = @idNiveau1," &
            " oa_antecedent_id_niveau2 = @idNiveau2," &
            " oa_antecedent_ordre_affichage1 = @ordreAffichage1," &
            " oa_antecedent_ordre_affichage2 = @ordreAffichage2," &
            " oa_antecedent_ordre_affichage3 = @ordreAffichage3" &
            " WHERE oa_antecedent_id = @antecedentId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLString, con)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", 1)
            .AddWithValue("@description", Description)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@dateFin", New Date(2999, 12, 31, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@nature", "")
            .AddWithValue("@priorite", 0)
            .AddWithValue("@publication", Publication)
            .AddWithValue("@niveau", 1)
            .AddWithValue("@idNiveau1", 0)
            .AddWithValue("@idNiveau2", 0)
            .AddWithValue("@ordreAffichage1", 990)
            .AddWithValue("@ordreAffichage2", 0)
            .AddWithValue("@ordreAffichage3", 0)
            .AddWithValue("@antecedentId", contexteId.ToString)
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
            ContexteHistoACreer.HistorisationDate = Date.Now()
            ContexteHistoACreer.Type = "A"
            ContexteHistoACreer.UtilisateurId = user.UtilisateurId
            ContexteHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ReactivationAntecedent
            ContexteHistoACreer.Nature = ""
            ContexteHistoACreer.Niveau = 1
            ContexteHistoACreer.Niveau1Id = 0
            ContexteHistoACreer.Niveau2Id = 0
            ContexteHistoACreer.Ordre1 = 990
            ContexteHistoACreer.Ordre2 = 0
            ContexteHistoACreer.Ordre3 = 0

            'Création dans l'historique des modifications du contexte
            AntecedentHistoCreationDao.CreationAntecedentHisto(ContexteHistoACreer, user, EnumEtatAntecedentHisto.ReactivationAntecedent)
        End If

        Return codeRetour
    End Function

    Public Function CreationContexte(contexte As Antecedent, contexteHistoACreer As AntecedentHisto, Optional conclusionEpisode As Boolean = False, Optional episode As Episode = Nothing) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim contexteId As Long

        Dim SQLstring As String = "INSERT INTO oasis.oa_antecedent" &
        " (oa_antecedent_patient_id, oa_antecedent_type, oa_antecedent_drc_id, oa_antecedent_description, oa_antecedent_date_creation," &
        " oa_antecedent_date_modification, oa_antecedent_utilisateur_creation, oa_antecedent_utilisateur_modification," &
        " oa_antecedent_date_debut, oa_antecedent_niveau, oa_antecedent_nature, oa_antecedent_statut_affichage, oa_antecedent_statut_affichage_transformation, oa_antecedent_inactif," &
        " oa_antecedent_ordre_affichage1, oa_antecedent_ordre_affichage2, oa_antecedent_ordre_affichage3, oa_antecedent_categorie_contexte," &
        " oa_antecedent_date_fin, oa_antecedent_diagnostic, oa_episode_id)" &
        " VALUES (@patientId, @type, @drcId, @description, @dateCreation," &
        " @dateModification, @utilisateurCreation, @utilisateurModification," &
        " @dateDebut, @niveau, @nature, @publication, @publicationTransformation, @inactif," &
        " @ordreAffichage1, @ordreAffichage2, @ordreAffichage3, @categorieContexte, @dateFin, @diagnostic, @episodeId); SELECT SCOPE_IDENTITY()"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@patientId", contexte.PatientId.ToString)
            .AddWithValue("@type", "C")
            .AddWithValue("@drcId", contexte.DrcId)
            .AddWithValue("@description", contexte.Description)
            .AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@utilisateurCreation", userLog.UtilisateurId.ToString)
            .AddWithValue("@utilisateurModification", 0)
            .AddWithValue("@dateDebut", contexte.DateDebut)
            .AddWithValue("@niveau", 1)
            .AddWithValue("@nature", "Patient")
            .AddWithValue("@publication", contexte.StatutAffichage)
            .AddWithValue("@publicationTransformation", contexte.StatutAffichageTransformation)
            .AddWithValue("@inactif", False)
            .AddWithValue("@ordreAffichage1", contexte.Ordre1)
            .AddWithValue("@ordreAffichage2", 0)
            .AddWithValue("@ordreAffichage3", 0)
            .AddWithValue("@categorieContexte", contexte.CategorieContexte)
            .AddWithValue("@dateFin", contexte.DateFin)
            .AddWithValue("@diagnostic", contexte.Diagnostic)
            .AddWithValue("@episodeId", contexte.EpisodeId)
        End With

        Try
            'Dim n As Integer 'Pour récupérer le nombre d'occurences enregistrées
            da.InsertCommand = cmd
            'n = da.InsertCommand.ExecuteNonQuery()
            contexteId = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Dim ContexteConclusionEpisodeId As Long

        If codeRetour = True Then
            'Mise à jour des données dans l'instance de la classe Historisation antecedent
            contexteHistoACreer.AntecedentId = contexteId 'Récupération du contexte créé
            contexteHistoACreer.HistorisationDate = DateTime.Now()
            contexteHistoACreer.UtilisateurId = userLog.UtilisateurId
            contexteHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.CreationAntecedent
            contexteHistoACreer.PatientId = contexte.PatientId.ToString
            contexteHistoACreer.Type = "C"
            contexteHistoACreer.Description = contexte.Description
            contexteHistoACreer.DateDebut = contexte.DateDebut
            contexteHistoACreer.Niveau = 1
            contexteHistoACreer.Nature = "Patient"
            contexteHistoACreer.StatutAffichage = contexte.StatutAffichage
            contexteHistoACreer.Inactif = 0
            contexteHistoACreer.Ordre1 = contexte.Ordre1
            contexteHistoACreer.Ordre2 = 0
            contexteHistoACreer.Ordre3 = 0
            contexteHistoACreer.Categorie = contexte.CategorieContexte
            contexteHistoACreer.DateFin = contexte.DateFin
            contexteHistoACreer.Diagnostic = contexte.Diagnostic

            'Ajout contexte créé si conclusion médicale de l'épisode
            If conclusionEpisode = True Then
                If episode IsNot Nothing Then
                    Dim episodeContexteDao As New EpisodeContexteDao
                    Dim episodeContexte As New EpisodeContexte
                    episodeContexte.ContexteId = ContexteConclusionEpisodeId
                    episodeContexte.EpisodeId = episode.Id
                    episodeContexte.PatientId = episode.PatientId
                    episodeContexte.DateCreation = Date.Now()
                    episodeContexte.UserCreation = userLog.UtilisateurId
                    episodeContexteDao.CreateEpisodeContexte(episodeContexte)
                End If
            End If

            'Lecture du contexte créé avec toutes ses données pour communiquer le DataReader à la fonction dédiée
            Dim contexteCreeDataReader As SqlDataReader
            SQLstring = "select * from oasis.oa_antecedent where oa_antecedent_id = " & contexteHistoACreer.AntecedentId & ";"
            Dim contexteCreeCommand As New SqlCommand(SQLstring, con)
            con.Open()
            contexteCreeDataReader = contexteCreeCommand.ExecuteReader()
            If contexteCreeDataReader.Read() Then
                'Initialisation classe Historisation antecedent 
                AntecedentHistoCreationDao.InitClasseAntecedentHistorisation(contexteCreeDataReader, userLog, contexteHistoACreer)

                'Libération des ressources d'accès aux données
                con.Close()
                contexteCreeCommand.Dispose()
            End If

            'Création dans l'historique du contexte créé
            AntecedentHistoCreationDao.CreationAntecedentHisto(contexteHistoACreer, userLog, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.CreationAntecedent)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            patientDao.ModificationDateMajSynthesePatient(contexte.PatientId)
        End If

        Return codeRetour
    End Function

    Public Function ModificationContexte(contexte As Antecedent, contexteHistoACreer As AntecedentHisto) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "UPDATE oasis.oa_antecedent SET" &
            " oa_antecedent_date_modification = @dateModification," &
            " oa_antecedent_utilisateur_modification = @utilisateurModification," &
            " oa_antecedent_drc_id = @drcId," &
            " oa_antecedent_categorie_contexte = @categorieContexte," &
            " oa_antecedent_description = @description," &
            " oa_antecedent_date_debut = @dateDebut," &
            " oa_antecedent_date_fin = @dateFin," &
            " oa_antecedent_ordre_affichage1 = @ordreAffichage," &
            " oa_antecedent_diagnostic = @diagnostic," &
            " oa_antecedent_statut_affichage = @publication," &
            " oa_antecedent_statut_affichage_transformation = @publicationTransformation" &
            " WHERE oa_antecedent_id = @antecedentId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now)
            .AddWithValue("@drcId", contexte.DrcId)
            .AddWithValue("@categorieContexte", contexte.CategorieContexte)
            .AddWithValue("@description", contexte.Description)
            .AddWithValue("@dateDebut", contexte.DateDebut)
            .AddWithValue("@dateFin", contexte.DateFin)
            .AddWithValue("@publication", contexte.StatutAffichage)
            .AddWithValue("@publicationTransformation", contexte.StatutAffichageTransformation)
            .AddWithValue("@ordreAffichage", contexte.Ordre1)
            .AddWithValue("@antecedentId", contexte.Id)
            .AddWithValue("@diagnostic", contexte.Diagnostic)
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
            contexteHistoACreer.HistorisationDate = Date.Now()
            contexteHistoACreer.Categorie = contexte.CategorieContexte
            contexteHistoACreer.UtilisateurId = userLog.UtilisateurId
            contexteHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent
            contexteHistoACreer.DrcId = contexte.DrcId
            contexteHistoACreer.Description = contexte.Description
            contexteHistoACreer.DateDebut = contexte.DateDebut
            contexteHistoACreer.DateFin = contexte.DateFin
            contexteHistoACreer.Ordre1 = contexte.Ordre1
            contexteHistoACreer.Diagnostic = contexte.Diagnostic
            contexteHistoACreer.StatutAffichage = contexte.StatutAffichage

            'Création dans l'historique des modifications du contexte
            AntecedentHistoCreationDao.CreationAntecedentHisto(contexteHistoACreer, userLog, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            patientDao.ModificationDateMajSynthesePatient(contexte.PatientId)
        End If

        Return codeRetour
    End Function

    Public Function AnnulationContexte(contexte As Antecedent, contexteHistoACreer As AntecedentHisto) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "UPDATE oasis.oa_antecedent SET" &
        " oa_antecedent_date_modification = @dateModification," &
        " oa_antecedent_utilisateur_modification = @utilisateurModification," &
        " oa_antecedent_inactif = @inactif" &
        " WHERE oa_antecedent_id = @antecedentId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@inactif", "1")
            .AddWithValue("@antecedentId", contexte.Id.ToString)
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
            contexteHistoACreer.HistorisationDate = Date.Now()
            contexteHistoACreer.UtilisateurId = userLog.UtilisateurId
            contexteHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.AnnulationAntecedent
            contexteHistoACreer.Inactif = True

            'Création dans l'historique des modifications du contexte
            AntecedentHistoCreationDao.CreationAntecedentHisto(contexteHistoACreer, userLog, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.AnnulationAntecedent)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            patientDao.ModificationDateMajSynthesePatient(contexte.PatientId)
        End If

        Return codeRetour
    End Function

    Public Function GetListOfContextebyPatient(patientId As Integer) As List(Of ContexteCourrier)
        Dim ListContexte As New List(Of ContexteCourrier)
        Dim antecedentDao As New AntecedentDao
        Dim dt As DataTable
        dt = antecedentDao.GetContextebyPatient(patientId, True)

        Dim rowCount As Integer = dt.Rows.Count - 1
        For i = 0 To rowCount Step 1
            Dim contexteCourrier As New ContexteCourrier
            contexteCourrier.PatientId = patientId
            contexteCourrier.Id = dt.Rows(i)("oa_antecedent_id")

            'Préparation de l'affichage du contexte
            Dim longueurString As Integer
            Dim longueurMax As Integer = 150
            Dim contexteDescription As String = Coalesce(dt.Rows(i)("oa_antecedent_description"), "")
            If contexteDescription <> "" Then
                contexteDescription = Replace(contexteDescription, vbCrLf, " ")
                longueurString = contexteDescription.Length
                If longueurString > longueurMax Then
                    longueurString = longueurMax
                End If
                contexteDescription.Substring(0, longueurString)
            End If

            Dim AfficheDateModification, diagnostic As String
            Dim categorieContexte As String
            Dim dateModification As Date

            categorieContexte = ""
            If dt.Rows(i)("oa_antecedent_categorie_contexte") IsNot DBNull.Value Then
                categorieContexte = dt.Rows(i)("oa_antecedent_categorie_contexte")
            End If

            AfficheDateModification = ""
            If categorieContexte = "M" Then
                If dt.Rows(i)("oa_antecedent_date_modification") IsNot DBNull.Value Then
                    dateModification = dt.Rows(i)("oa_antecedent_date_modification")
                    AfficheDateModification = FormatageDateAffichage(dateModification) + " : "
                Else
                    If dt.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                        dateModification = dt.Rows(i)("oa_antecedent_date_creation")
                        AfficheDateModification = FormatageDateAffichage(dateModification) + " : "
                    End If
                End If
            End If

            diagnostic = ""
            If dt.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                If CInt(dt.Rows(i)("oa_antecedent_diagnostic")) = EnumDiagnostic.SUSPICION_DE Then
                    diagnostic = "Suspicion de "
                Else
                    If CInt(dt.Rows(i)("oa_antecedent_diagnostic")) = EnumDiagnostic.NOTION_DE Then
                        diagnostic = "Notion de "
                    End If
                End If
            End If

            contexteCourrier.Description = AfficheDateModification & diagnostic & " " & contexteDescription

            ListContexte.Add(contexteCourrier)
        Next

        Return ListContexte
    End Function

End Class
