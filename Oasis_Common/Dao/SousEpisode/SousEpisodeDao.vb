Imports System.Configuration
Imports System.Data.SqlClient

Public Class SousEpisodeDao
    Inherits StandardDao

    Public Function GetLstSousEpisode(idEpisode As Long, Optional idSousEpisode As Long = 0, Optional isComplete As Boolean = True, Optional isWithInactif As Boolean = False) As List(Of SousEpisode)
        Dim con As SqlConnection = GetConnection()
        Dim sousEpisodes As List(Of SousEpisode) = New List(Of SousEpisode)
        Dim SQLString =
            "SELECT " & vbCrLf &
            "	  SE.id, " & vbCrLf &
            "     SE.episode_id, " & vbCrLf &
            "     SE.id_intervenant, " & vbCrLf &
            "     SE.id_sous_episode_type, " & vbCrLf &
            "     SE.id_sous_episode_sous_type, " & vbCrLf &
            "     SE.create_user_id, " & vbCrLf &
            "     SE.horodate_creation, " & vbCrLf &
            "     SE.last_update_user_id, " & vbCrLf &
            "     SE.horodate_last_update, " & vbCrLf &
            "     SE.validate_user_id, " & vbCrLf &
            "     SE.horodate_validate, " & vbCrLf &
            "	  SE.commentaire, " & vbCrLf &
            "	  SE.signature, " & vbCrLf &
            "	  SE.reference, " & vbCrLf &
            "	  SE.is_ald, " & vbCrLf &
            "	  SE.is_reponse, " & vbCrLf &
            "	  SE.delai_since_validation, " & vbCrLf &
            "	  SE.is_reponse_recue, " & vbCrLf &
            "	  SE.horodate_last_recu, " & vbCrLf &
            "     T.libelle as type_libelle, " & vbCrLf &
            "     S.libelle as sous_type_libelle, " & vbCrLf &
            "	  SE.is_inactif " & vbCrLf

        If isComplete Then
            SQLString += "" &
            ",UC.oa_utilisateur_prenom + ' ' + UC.oa_utilisateur_nom as user_create,  " & vbCrLf &
            "UU.oa_utilisateur_prenom + ' ' + UU.oa_utilisateur_nom as user_update, " & vbCrLf &
            "UV.oa_utilisateur_prenom + ' ' + UV.oa_utilisateur_nom as user_validate, " & vbCrLf &
            "S.redaction_profil_types, " & vbCrLf &
            "S.validation_profil_types, " & vbCrLf &
            "(SELECT COUNT(*) FROM oasis.oa_sous_episode_reponse SER WHERE SER.id_sous_episode = SE.id AND SER.validate_state = '!' AND SE.is_inactif = 'false' ) AS nb_reponse_waiting, " & vbCrLf &
            "(SELECT COUNT(*) FROM oasis.oa_sous_episode_reponse SER WHERE SER.id_sous_episode = SE.id AND SER.validate_state = 'm' AND SE.is_inactif = 'false' ) AS nb_med_reponse_waiting, " & vbCrLf &
            "(SELECT COUNT(*) FROM oasis.oa_sous_episode_reponse SER WHERE SER.id_sous_episode = SE.id AND SE.is_inactif = 'false' ) AS nb_reponse " & vbCrLf
        End If

        SQLString += "FROM [oasis].[oa_sous_episode] SE " & vbCrLf &
                     "JOIN oasis.oa_r_sous_episode_type T ON T.id =SE.id_sous_episode_type " & vbCrLf &
                     "JOIN oasis.oa_r_sous_episode_sous_type S ON S.id =SE.id_sous_episode_sous_type " & vbCrLf

        If isComplete Then
            SQLString += "" &
            "Join oasis.oa_utilisateur UC ON UC.oa_utilisateur_id =SE.create_user_id " & vbCrLf &
            "Left Join oasis.oa_utilisateur UU ON UC.oa_utilisateur_id =SE.last_update_user_id " & vbCrLf &
            "Left Join oasis.oa_utilisateur UV ON UV.oa_utilisateur_id =SE.validate_user_id " & vbCrLf
        End If

        SQLString += "WHERE 1=1 " & vbCrLf

        If idEpisode <> 0 Then
            SQLString += "AND SE.episode_id= @idEpisode " & vbCrLf
        End If

        If idSousEpisode <> 0 Then
            SQLString += "AND SE.id= @idSousEpisode " & vbCrLf
        End If

        If isWithInactif = False Then
            SQLString += "AND SE.is_inactif= @is_inactif " & vbCrLf
        End If

        SQLString += "ORDER by SE.id DESC"

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = SQLString
            If idSousEpisode <> 0 Then command.Parameters.AddWithValue("@idSousEpisode", idSousEpisode)
            If idEpisode <> 0 Then command.Parameters.AddWithValue("@idEpisode", idEpisode)
            If isWithInactif = False Then command.Parameters.AddWithValue("@is_inactif", False)
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    sousEpisodes.Add(BuildBean(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return sousEpisodes
    End Function

    Public Function GetAllSousEpisodeByPatient(episodeId As Integer, Optional isWithInactif As Boolean = False) As List(Of SousEpisode)
        Dim con As SqlConnection = GetConnection()
        Dim sousEpisodes As List(Of SousEpisode) = New List(Of SousEpisode)
        Dim SQLString As String = "SELECT " & vbCrLf &
            "	  SE.id, " & vbCrLf &
            "     SE.episode_id, " & vbCrLf &
            "     SE.id_intervenant, " & vbCrLf &
            "     SE.id_sous_episode_type, " & vbCrLf &
            "     SE.id_sous_episode_sous_type, " & vbCrLf &
            "     SE.create_user_id, " & vbCrLf &
            "     SE.horodate_creation, " & vbCrLf &
            "     SE.last_update_user_id, " & vbCrLf &
            "     SE.horodate_last_update, " & vbCrLf &
            "     SE.validate_user_id, " & vbCrLf &
            "     SE.horodate_validate, " & vbCrLf &
            "	  SE.commentaire, " & vbCrLf &
            "	  SE.signature, " & vbCrLf &
            "	  SE.reference, " & vbCrLf &
            "	  SE.is_ald, " & vbCrLf &
            "	  SE.is_reponse, " & vbCrLf &
            "	  SE.delai_since_validation, " & vbCrLf &
            "	  SE.is_reponse_recue, " & vbCrLf &
            "	  SE.horodate_last_recu, " & vbCrLf &
            "	  SE.is_inactif " & vbCrLf &
            ",UC.oa_utilisateur_prenom + ' ' + UC.oa_utilisateur_nom as user_create,  " & vbCrLf &
            "UU.oa_utilisateur_prenom + ' ' + UU.oa_utilisateur_nom as user_update, " & vbCrLf &
            "UV.oa_utilisateur_prenom + ' ' + UV.oa_utilisateur_nom as user_validate, " & vbCrLf &
            "T.libelle as type_libelle, " & vbCrLf &
            "S.libelle as sous_type_libelle, " & vbCrLf &
            "S.redaction_profil_types, " & vbCrLf &
            "S.validation_profil_types, " & vbCrLf &
            "(SELECT COUNT(*) FROM oasis.oa_sous_episode_reponse SER WHERE SER.id_sous_episode = SE.id AND SER.validate_state = '!' ) AS nb_reponse_waiting, " & vbCrLf &
            "(SELECT COUNT(*) FROM oasis.oa_sous_episode_reponse SER WHERE SER.id_sous_episode = SE.id AND SER.validate_state = 'm' ) AS nb_med_reponse_waiting, " & vbCrLf &
            "(SELECT COUNT(*) FROM oasis.oa_sous_episode_reponse SER WHERE SER.id_sous_episode = SE.id ) AS nb_reponse " & vbCrLf &
            "FROM [oasis].[oa_sous_episode] SE " & vbCrLf &
            "Join oasis.oa_r_sous_episode_type T On T.id =SE.id_sous_episode_type " & vbCrLf &
            "Join oasis.oa_r_sous_episode_sous_type S ON S.id =SE.id_sous_episode_sous_type " & vbCrLf &
            "Join oasis.oa_utilisateur UC ON UC.oa_utilisateur_id =SE.create_user_id " & vbCrLf &
            "Left Join oasis.oa_utilisateur UU ON UC.oa_utilisateur_id =SE.last_update_user_id " & vbCrLf &
            "Left Join oasis.oa_utilisateur UV ON UV.oa_utilisateur_id =SE.validate_user_id " & vbCrLf &
            "WHERE episode_id = @episodeId" & vbCrLf

        If isWithInactif = True Then
            SQLString += "AND SE.is_inactif= @is_inactif " & vbCrLf
        End If

        SQLString += "ORDER by SE.id DESC"

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = SQLString
            command.Parameters.AddWithValue("@episodeId", episodeId)
            command.Parameters.AddWithValue("@is_inactif", Not isWithInactif)
            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    sousEpisodes.Add(BuildBean(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return sousEpisodes
    End Function

    Public Function GetTableSousEpisode(idEpisode As Long, Optional idSousEpisode As Long = 0, Optional isComplete As Boolean = True, Optional isWithInactif As Boolean = False) As DataTable
        Dim SQLString =
            "SELECT " & vbCrLf &
            "	  SE.id, " & vbCrLf &
            "     SE.episode_id, " & vbCrLf &
            "     SE.id_intervenant, " & vbCrLf &
            "     SE.id_sous_episode_type, " & vbCrLf &
            "     SE.id_sous_episode_sous_type, " & vbCrLf &
            "     SE.create_user_id, " & vbCrLf &
            "     SE.horodate_creation, " & vbCrLf &
            "     SE.last_update_user_id, " & vbCrLf &
            "     SE.horodate_last_update, " & vbCrLf &
            "     SE.validate_user_id, " & vbCrLf &
            "     SE.horodate_validate, " & vbCrLf &
            "	  SE.commentaire, " & vbCrLf &
            "	  SE.signature, " & vbCrLf &
            "	  SE.reference, " & vbCrLf &
            "	  SE.is_ald, " & vbCrLf &
            "	  SE.is_reponse, " & vbCrLf &
            "	  SE.delai_since_validation, " & vbCrLf &
            "	  SE.is_reponse_recue, " & vbCrLf &
            "	  SE.horodate_last_recu, " & vbCrLf &
            "	  SE.is_inactif " & vbCrLf

        If isComplete Then
            SQLString += "" &
            ",UC.oa_utilisateur_prenom + ' ' + UC.oa_utilisateur_nom as user_create,  " & vbCrLf &
            "UU.oa_utilisateur_prenom + ' ' + UU.oa_utilisateur_nom as user_update, " & vbCrLf &
            "UV.oa_utilisateur_prenom + ' ' + UV.oa_utilisateur_nom as user_validate, " & vbCrLf &
            "T.libelle as type_libelle, " & vbCrLf &
            "S.libelle as sous_type_libelle, " & vbCrLf &
            "S.redaction_profil_types, " & vbCrLf &
            "S.validation_profil_types, " & vbCrLf &
            "(SELECT COUNT(*) FROM oasis.oa_sous_episode_reponse SER WHERE SER.id_sous_episode = SE.id AND SER.validate_state = '!' ) AS nb_reponse_waiting, " & vbCrLf &
            "(SELECT COUNT(*) FROM oasis.oa_sous_episode_reponse SER WHERE SER.id_sous_episode = SE.id AND SER.validate_state = 'm' ) AS nb_med_reponse_waiting, " & vbCrLf &
            "(SELECT COUNT(*) FROM oasis.oa_sous_episode_reponse SER WHERE SER.id_sous_episode = SE.id ) AS nb_reponse " & vbCrLf
        End If

        SQLString += "FROM [oasis].[oa_sous_episode] SE " & vbCrLf

        If isComplete Then
            SQLString += "" &
            "Join oasis.oa_r_sous_episode_type T On T.id =SE.id_sous_episode_type " & vbCrLf &
            "Join oasis.oa_r_sous_episode_sous_type S ON S.id =SE.id_sous_episode_sous_type " & vbCrLf &
            "Join oasis.oa_utilisateur UC ON UC.oa_utilisateur_id =SE.create_user_id " & vbCrLf &
            "Left Join oasis.oa_utilisateur UU ON UC.oa_utilisateur_id =SE.last_update_user_id " & vbCrLf &
            "Left Join oasis.oa_utilisateur UV ON UV.oa_utilisateur_id =SE.validate_user_id " & vbCrLf
        End If

        SQLString += "WHERE 1=1 " & vbCrLf

        If idEpisode <> 0 Then
            SQLString += "AND SE.episode_id= @idEpisode " & vbCrLf
        End If

        If idSousEpisode <> 0 Then
            SQLString += "AND SE.id= @idSousEpisode " & vbCrLf
        End If

        If isWithInactif = False Then
            SQLString += "AND SE.is_inactif= @is_inactif " & vbCrLf
        End If

        SQLString += "ORDER by SE.id DESC"

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                If idSousEpisode <> 0 Then tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@idSousEpisode", idSousEpisode)
                If idEpisode <> 0 Then tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@idEpisode", idEpisode)
                If isWithInactif = False Then tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@is_inactif", False)
                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        tacheDataAdapter.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return tacheDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function CountSousEpisode(selectedEpisodeId As Long, Optional isWithInactif As Boolean = False) As Integer
        Dim SqlString As String = "SELECT COUNT(*) FROM oasis.oa_sous_episode WHERE episode_id=" & selectedEpisodeId &
            If(isWithInactif, "", " AND is_inactif = 0 ")

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = New SqlCommand(SqlString, con)
            Using command
                Return command.ExecuteScalar()
            End Using
        End Using
    End Function

    Public Function ResumeSousEpisode(selectedEpisodeId As Long, Optional isComplete As Boolean = True) As String
        Dim str As String = ""
        Dim dicCount As New Dictionary(Of Long, Integer)

        ' --- on compte les evenements par sous type
        Try
            Dim lst = GetLstSousEpisode(selectedEpisodeId,, isComplete)
            For Each se In lst
                Dim i As Integer
                If dicCount.TryGetValue(se.IdSousEpisodeSousType, i) Then
                    dicCount(se.IdSousEpisodeSousType) = i + 1
                Else
                    dicCount.Add(se.IdSousEpisodeSousType, 1)
                End If
            Next
            If dicCount.Count > 0 Then
                Dim dictSTRef = New SousEpisodeSousTypeDao().getDictSousEpisodeSousType()
                For Each id In dicCount.Keys
                    str += If(str = "", "", vbCrLf) & dicCount(id).ToString & " " & dictSTRef(id).Libelle & If(dicCount(id) > 1, "s", "")
                Next
            End If
        Catch
        End Try
        Return str
    End Function

    Public Sub ResetReponseRecue(con As SqlConnection, sousEpisode As SousEpisode, transaction As SqlTransaction)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim SQLstring = " UPDATE oasis.oa_sous_episode SET is_reponse_recue = 'false', horodate_last_recu = @dateLastRecu WHERE id = @id"
        Dim cmd = New SqlCommand(SQLstring, con, transaction)
        With cmd.Parameters
            .AddWithValue("@id", sousEpisode.Id)
            .AddWithValue("@dateLastRecu", DBNull.Value)
        End With
        da.UpdateCommand = cmd
        Dim nbUpdate = da.UpdateCommand.ExecuteNonQuery()
        If nbUpdate <> 1 Then
            Throw New Exception("Problème : Enregistrements mouvementés : " & nbUpdate & " au lieu de 1 !")
        End If


    End Sub

    Public Function GenerateRandomString(ByRef len As Integer) As String
        Dim rand As New Random()
        Dim allowableChars() As Char = "ABCDEFGHIJKLOMNOPQRSTUVWXYZ0123456789".ToCharArray()
        Dim final As String = String.Empty
        For i As Integer = 0 To len - 1
            final += allowableChars(rand.Next(allowableChars.Length - 1))
        Next
        Return final
    End Function

    Public Function FormatPrenom(prenom As String) As String
        Return prenom.PadRight(5, "X").Substring(0, 5).ToUpper()
    End Function

    Public Function Create(sousEpisode As SousEpisode) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Dim reference As String = GenerateRandomString(6)
        Dim episodeDao As New EpisodeDao
        Dim patientDao As New PatientDao
        Dim episode As Episode = episodeDao.GetEpisodeById(sousEpisode.EpisodeId)
        Dim patient As Patient = patientDao.GetPatient(episode.PatientId)
        reference = FormatPrenom(patient.PatientPrenom) & "-" & reference
        Try
            Dim SQLstring As String = "INSERT INTO oasis.oa_sous_episode " &
                    "(episode_id , id_intervenant, id_sous_episode_type , id_sous_episode_sous_type , create_user_id , horodate_creation , " &
                    " commentaire , is_ald , is_reponse, delai_since_validation, reference )" &
            " VALUES (@episode_id, @id_intervenant, @id_sous_episode_type, @id_sous_episode_sous_type, @create_user_id, @horodate_creation, " &
                     " @commentaire, @is_ald, @is_reponse, @delai_since_validation, @reference); SELECT SCOPE_IDENTITY()"

            sousEpisode.HorodateCreation = DateTime.Now
            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@episode_id", sousEpisode.EpisodeId)
                .AddWithValue("@id_intervenant", If(sousEpisode.IdIntervenant = 0, DBNull.Value, sousEpisode.IdIntervenant))
                .AddWithValue("@id_sous_episode_type", sousEpisode.IdSousEpisodeType)
                .AddWithValue("@id_sous_episode_sous_type", sousEpisode.IdSousEpisodeSousType)
                .AddWithValue("@create_user_id", sousEpisode.CreateUserId)
                .AddWithValue("@horodate_creation", sousEpisode.HorodateCreation)

                .AddWithValue("@commentaire", sousEpisode.Commentaire)
                .AddWithValue("@is_ald", sousEpisode.IsALD)
                .AddWithValue("@is_reponse", sousEpisode.IsReponse)
                .AddWithValue("@delai_since_validation", sousEpisode.DelaiSinceValidation)
                .AddWithValue("@reference", reference)
            End With

            da.InsertCommand = cmd
            sousEpisode.Id = da.InsertCommand.ExecuteScalar()

            Console.WriteLine("ref created:" & reference)

            ' -- on fixe les idSousEpisode de la table fille (details)
            If sousEpisode.lstDetail.Count > 0 Then
                Dim sousEpisodeDetailSousTypeDao As SousEpisodeDetailSousTypeDao = New SousEpisodeDetailSousTypeDao
                For Each detail In sousEpisode.lstDetail
                    detail.IdSousEpisode = sousEpisode.Id
                    sousEpisodeDetailSousTypeDao.Create(con, detail, transaction)
                Next
            End If

            ' --- process de signature 
            'If isWithSign Then
            ' updateValidation(con, sousEpisode.Id, transaction)
            ' sousEpisode.HorodateValidate = Date.Now
            ' sousEpisode.ValidateUserId = userLog.UtilisateurId
            'End If

            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            transaction.Dispose()
            con.Close()
        End Try

        Return codeRetour
    End Function


    Public Sub WriteDocAndEventualySign(sousEpisode As SousEpisode, tbl As Byte(), signature As String, dateSignature As Date, userLog As Utilisateur, loginRequestLog As Object)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim transaction As SqlTransaction = con.BeginTransaction
        Try
            ' -- 1 : enregistrement en base si signataure
            If String.IsNullOrEmpty(signature) = False Then
                dateSignature = DateTime.Now

                Dim SQLstring As String = "UPDATE oasis.oa_sous_episode " &
                                      "SET validate_user_id = @ValidateUserId, horodate_validate = @HoroDateValidate, signature = @Signature " &
                                      "WHERE id=@Id"

                Dim cmd As New SqlCommand(SQLstring, con, transaction)
                With cmd.Parameters
                    .AddWithValue("@ValidateUserId", userLog.UtilisateurId)
                    .AddWithValue("@HoroDateValidate", dateSignature)
                    .AddWithValue("@id", sousEpisode.Id)
                    .AddWithValue("@Signature", signature)
                End With

                da.UpdateCommand = cmd
                Dim nb As Integer = da.UpdateCommand.ExecuteNonQuery()
                If (nb <> 1) Then
                    Throw New Exception("Validation échouée (" & nb & ")")
                End If
            End If

            ' -- 2 : serialisation du fichier
            sousEpisode.WriteContenuModel(tbl, loginRequestLog)

            ' -- 3 : commit et maj bean si signature
            If String.IsNullOrEmpty(signature) = False Then
                transaction.Commit()
                sousEpisode.ValidateUserId = userLog.UtilisateurId
                sousEpisode.HorodateValidate = dateSignature
            End If


        Catch ex As Exception
            If String.IsNullOrEmpty(signature) = False Then transaction.Rollback()
            Throw ex
        Finally
            transaction.Dispose()
            con.Close()
        End Try

    End Sub

    Public Function updateValidation(con As SqlConnection, idSousEpisode As Long, transaction As SqlTransaction, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim isMyTransaction As Boolean = (transaction Is Nothing)
        If isMyTransaction Then
            con = GetConnection()
            transaction = con.BeginTransaction
        End If


        Try
            Dim SQLstring As String = "UPDATE oasis.oa_sous_episode " &
                                      "SET validate_user_id = @ValidateUserId, horodate_validate = @HoroDateValidate " &
                                      "WHERE id=@Id"

            'SousEpisode.HorodateCreation = DateTime.Now



            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@ValidateUserId", userLog.UtilisateurId)
                .AddWithValue("@HoroDateValidate", DateTime.Now)
                .AddWithValue("@id", idSousEpisode)
            End With


            da.UpdateCommand = cmd
            Dim nb As Integer = da.UpdateCommand.ExecuteNonQuery()
            If (nb <> 1) Then
                Throw New Exception("Validation échouée (" & nb & ")")
            End If

            If isMyTransaction Then transaction.Commit()

        Catch ex As Exception
            If isMyTransaction Then
                transaction.Rollback()
            Else
                Throw ex    ' on remonte l'excaption a l'appelant
            End If
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            If isMyTransaction Then transaction.Dispose()
            If isMyTransaction Then con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function inactiverSousEpisode(con As SqlConnection, idSousEpisode As Long, transaction As SqlTransaction) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim isMyTransaction As Boolean = (transaction Is Nothing)
        If isMyTransaction Then
            con = GetConnection()
            transaction = con.BeginTransaction
        End If

        Try
            Dim SQLstring As String = "UPDATE oasis.oa_sous_episode " &
                                      "SET is_inactif = @is_inactif " &
                                      "WHERE id=@Id"

            'SousEpisode.HorodateCreation = DateTime.Now
            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@is_inactif", True)
                .AddWithValue("@id", idSousEpisode)
            End With

            da.UpdateCommand = cmd
            Dim nb As Integer = da.UpdateCommand.ExecuteNonQuery()
            If (nb <> 1) Then
                Throw New Exception("Inactivation échouée (" & nb & ")")
            End If

            If isMyTransaction Then transaction.Commit()

        Catch ex As Exception
            If isMyTransaction Then
                transaction.Rollback()
            Else
                Throw ex    ' on remonte l'excaption a l'appelant
            End If
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            If isMyTransaction Then transaction.Dispose()
            If isMyTransaction Then con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function getById(idSousEpisode As Long) As SousEpisode
        Return GetLstSousEpisode(0, idSousEpisode, True)(0)
    End Function

    'Public Function getById(idSousEpisode As Long) As SousEpisode
    '    Dim sousEpisode As SousEpisode
    '    Dim con As SqlConnection = GetConnection()
    '    Try
    '        Dim command As SqlCommand = con.CreateCommand()
    '        command.CommandText = "SELECT * FROM oasis.oa_sous_episode WHERE id=@id"
    '        command.Parameters.AddWithValue("@id", idSousEpisode)
    '        Using reader As SqlDataReader = command.ExecuteReader()
    '            If reader.Read() Then
    '                sousEpisode = BuildBean(reader)
    '            Else
    '                Throw New ArgumentException("SousEpisode inexistant !")
    '            End If
    '        End Using
    '    Catch ex As Exception
    '        Throw ex
    '    Finally
    '        con.Close()
    '    End Try
    '    Return sousEpisode
    'End Function

    Private Function BuildBean(reader As SqlDataReader) As SousEpisode
        Dim sousEpisode As New SousEpisode With {
            .Id = reader("id"),
            .EpisodeId = reader("episode_id"),
            .IdIntervenant = Coalesce(reader("id_intervenant"), 0),
            .IdSousEpisodeType = reader("id_sous_episode_type"),
            .IdSousEpisodeSousType = reader("id_sous_episode_sous_type"),
            .CreateUserId = reader("create_user_id"),
            .HorodateCreation = reader("horodate_creation"),
            .LastUpdateUserId = Coalesce(reader("last_update_user_id"), 0),
            .HorodateLastUpdate = Coalesce(reader("horodate_last_update"), Nothing),
            .ValidateUserId = Coalesce(reader("validate_user_id"), 0),
            .HorodateValidate = Coalesce(reader("horodate_validate"), Nothing),
            .Commentaire = Coalesce(reader("commentaire"), ""),
            .IsALD = reader("is_ald"),
            .IsReponse = Coalesce(reader("is_reponse"), False),
            .DelaiSinceValidation = Coalesce(reader("delai_since_validation"), ConfigurationManager.AppSettings("DelaiDefautReponseSousEpisode")),
            .IsReponseRecue = Coalesce(reader("is_reponse_recue"), False),
            .HorodateLastRecu = Coalesce(reader("horodate_last_recu"), Nothing),
            .isInactif = Coalesce(reader("is_inactif"), False),
            .Signature = Coalesce(reader("signature"), "NaN"),
            .Reference = Coalesce(reader("reference"), "NaN"),
            .SousTypeLibelle = Coalesce(reader("sous_type_libelle"), ""),
            .UserCreate = Coalesce(reader("user_create"), ""),
            .NbReponse = Coalesce(reader("nb_reponse"), 0),
            .NbReponseWaiting = Coalesce(reader("nb_reponse_waiting"), 0),
            .NbMedReponseWaiting = Coalesce(reader("nb_med_reponse_waiting"), 0)
            }
        Return sousEpisode
    End Function

End Class
