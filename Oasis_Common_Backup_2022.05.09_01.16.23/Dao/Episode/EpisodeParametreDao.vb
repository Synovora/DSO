﻿Imports System.Data.SqlClient
Imports Oasis_Common
Public Class EpisodeParametreDao
    Inherits StandardDao

    Public Function GetEpisodeParametreById(Id As Integer) As EpisodeParametre
        Dim episodeParametre As EpisodeParametre
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_episode_parametre WHERE episode_parametre_id = @id"
            command.Parameters.AddWithValue("@id", Id)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    episodeParametre = BuildBean(reader)
                Else
                    Throw New ArgumentException("épisode inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return episodeParametre
    End Function

    Public Function GetEpisodeParametreByParametreIdAndEpisodeId(parametreId As Integer, episodeId As Long) As EpisodeParametre
        Dim episodeParametre As EpisodeParametre
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_episode_parametre WHERE parametre_id = @parametreId AND episode_id = @episodeId"
            command.Parameters.AddWithValue("@parametreId", parametreId)
            command.Parameters.AddWithValue("@episodeId", episodeId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    episodeParametre = BuildBean(reader)
                Else
                    episodeParametre = New EpisodeParametre
                    episodeParametre.Id = 0
                    episodeParametre.EpisodeId = 0
                    episodeParametre.ParametreId = 0
                    episodeParametre.PatientId = 0
                    episodeParametre.Valeur = 0
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return episodeParametre
    End Function

    Private Function BuildBean(reader As SqlDataReader) As EpisodeParametre
        Dim episodeParametre As New EpisodeParametre With {
            .Id = reader("episode_parametre_id"),
            .ParametreId = Coalesce(reader("parametre_id"), 0),
            .EpisodeId = Coalesce(reader("episode_id"), 0),
            .PatientId = Coalesce(reader("patient_id"), 0),
            .Valeur = Coalesce(reader("valeur"), 0),
            .Description = Coalesce(reader("description"), ""),
            .Entier = Coalesce(reader("entier"), 0),
            .Decimal = Coalesce(reader("decimal"), 0),
            .Unite = Coalesce(reader("unite"), ""),
            .ParametreAjoute = Coalesce(reader("parametre_ajoute"), False),
            .Ordre = Coalesce(reader("ordre"), 0),
            .Inactif = Coalesce(reader("inactif"), False)
        }
        Return episodeParametre
    End Function

    Public Function getAllParametreEpisodeByEpisodeId(episodeId As Long) As DataTable
        Dim SQLString As String = "SELECT * FROM oasis.oa_episode_parametre" &
        " WHERE episode_id = " & episodeId.ToString &
        " AND (inactif = '0' OR inactif is Null)" &
        " ORDER BY ordre"

        Using con As SqlConnection = GetConnection()
            Dim ContexteDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ContexteDataAdapter
                ContexteDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim dt As DataTable = New DataTable()
                Using dt
                    Try
                        ContexteDataAdapter.Fill(dt)
                        Dim command As SqlCommand = con.CreateCommand
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return dt
                End Using
            End Using
        End Using
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sousEpisodeFusion"></param>
    ''' <param name="idEpisode"></param>
    ''' <param name="idPatient"></param>
    Public Sub AlimenteFusionDocumentParametres(sousEpisodeFusion As SousEpisodeFusion, idEpisode As Long, idPatient As Long)
        Using con As SqlConnection = GetConnection()
            Dim valeur As Double
            Dim isComposite As Boolean
            Dim strTodo As String
            Dim dataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using dataAdapter
                Dim command As New SqlCommand("oasis.GET_PARAMETRE_FUSION_DOC", con)
                command.CommandType = CommandType.StoredProcedure

                dataAdapter.SelectCommand = command

                dataAdapter.SelectCommand.Parameters.AddWithValue("@ID_EPISODE", idEpisode)
                dataAdapter.SelectCommand.Parameters.AddWithValue("@ID_PATIENT", idPatient)
                Dim dataTable As DataTable = New DataTable()
                Using dataTable
                    Try
                        dataAdapter.Fill(dataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    For Each row In dataTable.Rows
                        valeur = Coalesce(row("valeur"), 0)
                        isComposite = Coalesce(row("is_composite_fusion_document"), False)
                        If isComposite Then
                            strTodo = "" & valeur
                        Else
                            strTodo = row("description") & " : " & valeur & " " & row("unite")
                        End If
                        If valeur <> 0 Then CallByName(sousEpisodeFusion, row("field_fusion_name"), CallType.Set, strTodo)
                    Next
                End Using
            End Using
        End Using

    End Sub

    Public Function GetPoidsByEpisodeIdOrLastKnow(idEpisode As Long, idPatient As Long) As Double
        Dim SQLString As String = "SELECT TOP 1 COALESCE(EP1.valeur, " & vbCrLf &
                                 "      (SELECT TOP 1 EP2.valeur " & vbCrLf &
                                 "  	FROM oasis.oa_episode_parametre EP2 " & vbCrLf &
                                 "  	WHERE EP2.parametre_id= @TypeParam " & vbCrLf &
                                 "  	AND EP2.patient_id = @PatientId " & vbCrLf &
                                 "  	AND EP2.valeur>0" & vbCrLf &
                                 "  	ORDER by EP2.episode_id DESC " & vbCrLf &
                                 "      )" & vbCrLf &
                                 ") as valeur" & vbCrLf &
                    "FROM oasis.oa_patient P " & vbCrLf &
                    "LEFT JOIN oasis.oa_episode_parametre EP1 " & vbCrLf &
                        "ON EP1.parametre_id=@TypeParam" & vbCrLf &
                            "AND EP1.patient_id=P.oa_patient_id" & vbCrLf &
                            "AND EP1.episode_id = @EpisodeId" & vbCrLf &
                            "AND EP1.valeur>0" & vbCrLf &
                            "AND COALESCE(EP1.inactif,0) = 0 " & vbCrLf &
                    "WHERE P.oa_patient_id = @PatientId"
        Using con As SqlConnection = GetConnection()
            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@TypeParam", Parametre.EnumParametreId.POIDS)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@EpisodeId", idEpisode)
                tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@PatientId", idPatient)
                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        tacheDataAdapter.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return Coalesce(tacheDataTable.Rows(0)("valeur"), 0)
                End Using
            End Using
        End Using
    End Function



    Public Function CreateEpisodeParametre(episodeParametre As EpisodeParametre) As Boolean
        Dim nbcreate As Integer
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim SQLstring As String =
        "IF NOT EXISTS (SELECT 1 FROM oasis.oa_episode_parametre WHERE episode_id = @episodeId AND parametre_id = @parametreId AND (inactif = Null OR inactif = '0'))" &
        "INSERT INTO oasis.oa_episode_parametre" &
        " (parametre_id, episode_id, patient_id, valeur, description, entier," &
        " decimal, unite, parametre_ajoute, ordre, inactif)" &
        " VALUES (@parametreId, @episodeId, @patientId, @valeur, @description, @entier," &
        " @decimal, @unite, @parametreAjoute, @ordre, @inactif)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@parametreId", episodeParametre.ParametreId)
            .AddWithValue("@patientId", episodeParametre.PatientId)
            .AddWithValue("@episodeId", episodeParametre.EpisodeId)
            .AddWithValue("@valeur", If(episodeParametre.Valeur.HasValue, episodeParametre.Valeur.Value, DBNull.Value))
            .AddWithValue("@description", episodeParametre.Description)
            .AddWithValue("@entier", episodeParametre.Entier)
            .AddWithValue("@decimal", episodeParametre.Decimal)
            .AddWithValue("@unite", episodeParametre.Unite)
            .AddWithValue("@parametreAjoute", episodeParametre.ParametreAjoute)
            .AddWithValue("@ordre", episodeParametre.Ordre)
            .AddWithValue("@inactif", episodeParametre.Inactif)
        End With

        Try
            da.InsertCommand = cmd
            nbcreate = da.InsertCommand.ExecuteNonQuery()
            If nbcreate <= 0 Then
                Throw New Exception("Collision: Le paramètre existe déjà pour cet épisode")
            End If
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function ModificationEpisodeParametre(episodeParametre As EpisodeParametre) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String = "UPDATE oasis.oa_episode_parametre SET" &
        " parametre_id = @parametreId, episode_id = @episodeId, patient_id = @patientId," &
        " valeur = @valeur, description = @description," &
        " entier = @entier, decimal = @decimal," &
        " unite = @unite, ordre = @ordre, inactif = @inactif" &
        " where episode_parametre_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", episodeParametre.Id)
            .AddWithValue("@patientId", episodeParametre.PatientId)
            .AddWithValue("@patientId", episodeParametre.PatientId)
            .AddWithValue("@episodeId", episodeParametre.EpisodeId)
            .AddWithValue("@valeur", If(episodeParametre.Valeur.HasValue, episodeParametre.Valeur.Value, DBNull.Value))
            .AddWithValue("@description", episodeParametre.Description)
            .AddWithValue("@entier", episodeParametre.Entier)
            .AddWithValue("@decimal", episodeParametre.Decimal)
            .AddWithValue("@unite", episodeParametre.Unite)
            .AddWithValue("@ordre", episodeParametre.Ordre)
            .AddWithValue("@inactif", episodeParametre.Inactif)
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

        Return codeRetour
    End Function

    Public Function ModificationValeurEpisodeParametre(episodeParametreId As Long, Valeur As Decimal?) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String = "UPDATE oasis.oa_episode_parametre SET" &
        " valeur = @valeur" &
        " WHERE episode_parametre_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", episodeParametreId)
            .AddWithValue("@valeur", If(Valeur.HasValue, Valeur.Value, DBNull.Value))
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

        Return codeRetour
    End Function

    Public Function AnnulationEpisodeParametre(episodeParametreId As Long) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String =
        "UPDATE oasis.oa_episode_parametre" &
        " SET inactif = @inactif" &
        " WHERE episode_parametre_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", episodeParametreId)
            .AddWithValue("@inactif", True)
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

        Return codeRetour
    End Function

    Public Function SuppressionEpisodeParametreByEpisodeId(episodeId As Long) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim SQLstring As String = "DELETE oasis.oa_episode_parametre" &
        " WHERE episode_id = @episodeId"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@episodeId", episodeId)
        End With

        Try
            da.DeleteCommand = cmd
            da.DeleteCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

End Class
