﻿Imports System.Configuration
Imports System.IO
Imports Oasis_Common
Public Class SousEpisode
    Property Id As Long
    Property IdIntervenant As Long
    Property EpisodeId As Long
    Property IdSousEpisodeType As Long
    Property IdSousEpisodeSousType As Long
    Property CreateUserId As Long
    Property HorodateCreation As DateTime
    Property LastUpdateUserId As Long
    Property HorodateLastUpdate As DateTime
    Property ValidateUserId As Long
    Property HorodateValidate As DateTime
    Property Commentaire As String
    Property IsALD As Boolean
    Property IsReponse As Boolean
    Property DelaiSinceValidation As Integer
    Property IsReponseRecue As Boolean
    Property HorodateLastRecu As DateTime
    Property isInactif As Boolean
    Property lstDetail As List(Of SousEpisodeDetailSousType)
    Property Signature As String


    Public Sub New()
    End Sub

    Public Sub New(row As DataRow)
        Me.Id = row("id")
        Me.EpisodeId = row("episode_id")
        Me.IdIntervenant = Coalesce(row("id_intervenant"), 0)
        Me.IdSousEpisodeType = row("id_sous_episode_type")
        Me.IdSousEpisodeSousType = row("id_sous_episode_sous_type")

        Me.CreateUserId = row("create_user_id")
        Me.HorodateCreation = row("horodate_creation")

        Me.LastUpdateUserId = Coalesce(row("last_update_user_id"), 0)
        Me.HorodateLastUpdate = Coalesce(row("horodate_last_update"), Nothing)

        Me.ValidateUserId = Coalesce(row("Validate_user_id"), 0)
        Me.HorodateValidate = Coalesce(row("horodate_Validate"), Nothing)

        Me.Commentaire = Coalesce(row("commentaire"), "")

        Me.IsALD = row("is_ald")
        Me.IsReponse = Coalesce(row("is_reponse"), False)
        Me.DelaiSinceValidation = Coalesce(row("delai_since_validation"), ConfigurationManager.AppSettings("DelaiDefautReponseSousEpisode"))

        Me.IsReponseRecue = Coalesce(row("is_reponse_recue"), False)
        Me.HorodateLastRecu = Coalesce(row("horodate_last_recu"), Nothing)
        Me.isInactif = Coalesce(row("is_inactif"), False)

    End Sub

    Public Function isThisDetailALD(idSousSousType As Long) As Boolean
        If Me.lstDetail Is Nothing Then Return False
        For Each s In Me.lstDetail
            If s.IdSousEpisodeSousSousType = idSousSousType Then Return s.IsALD
        Next
        Return False
    End Function

    Public Function isThisSousSousTypePresent(idSousSousType As Long) As Boolean
        If Me.lstDetail Is Nothing Then Return False
        For Each s In Me.lstDetail
            If s.IdSousEpisodeSousSousType = idSousSousType Then Return True
        Next
        Return False
    End Function

    Public Function isIntervenant() As Boolean
        Return Me.IdIntervenant <> 0
    End Function

    Public Function getContenu() As Byte()
        Dim filename = getFilenameServer()
        ' -- download
        Using apiOasis As New ApiOasis()
            Dim downloadRequest As New DownloadRequest With {
               .LoginRequest = loginRequestLog,
               .FileName = filename
               }
            Return apiOasis.downloadFileRest(downloadRequest)
        End Using

    End Function

    Public Sub writeContenuModel(tblContenu As Byte())
        ' --- tentative d'upload
        Using apiOasis As New ApiOasis()
            apiOasis.uploadFileRest(loginRequestLog.login,
                                        loginRequestLog.password,
                                        getFilenameServer(),
                                        tblContenu)
        End Using

    End Sub
    Private Function getFilenameServer() As String
        Return "Episode_" & Me.EpisodeId & "_SousEpisode_" & Me.Id & "_SousEpisodeSousType_" & Me.IdSousEpisodeSousType & ".DOCX"
    End Function

    Public Function Serialize() As Byte()
        Using m As MemoryStream = New MemoryStream()
            Using writer As BinaryWriter = New BinaryWriter(m)
                writer.Write(Id) 'Long
                writer.Write(IdIntervenant) 'Long
                writer.Write(EpisodeId) 'Long
                writer.Write(IdSousEpisodeType) 'Long
                writer.Write(IdSousEpisodeSousType) 'Long
                writer.Write(CreateUserId) 'Long
                writer.Write(HorodateCreation.Ticks) 'Date -> Long
                writer.Write(LastUpdateUserId) 'Long
                writer.Write(HorodateLastUpdate.Ticks) 'Date -> Long
                writer.Write(ValidateUserId) 'Long
                writer.Write(HorodateValidate.Ticks) 'Date -> Long
                writer.Write(Commentaire) 'String
                writer.Write(IsALD) 'Boolean
                writer.Write(IsReponse) 'Boolean
                writer.Write(DelaiSinceValidation) 'Int
                writer.Write(IsReponseRecue) 'Boolean
                writer.Write(HorodateLastRecu.Ticks) 'Date -> Long
                'writer.Write(isInactif) 'Boolean

                'writer.Write(lstDetail) 'Int

            End Using
            Return m.ToArray()
        End Using
    End Function

    Public Shared Function Deserialize(ByVal data As Byte()) As SousEpisode
        Dim result As SousEpisode = New SousEpisode()
        Using m As MemoryStream = New MemoryStream(data)
            Using reader As BinaryReader = New BinaryReader(m)
                result.Id = reader.ReadInt64()
                result.IdIntervenant = reader.ReadInt64()
                result.EpisodeId = reader.ReadInt64()
                result.IdSousEpisodeType = reader.ReadInt64()
                result.IdSousEpisodeSousType = reader.ReadInt64()
                result.CreateUserId = reader.ReadInt64()
                result.HorodateCreation = New Date(reader.ReadInt64())
                result.LastUpdateUserId = reader.ReadInt64()
                result.HorodateLastUpdate = New Date(reader.ReadInt64())
                result.ValidateUserId = reader.ReadInt64()
                result.HorodateValidate = New Date(reader.ReadInt64())
                result.Commentaire = reader.ReadString()
                result.IsALD = reader.ReadBoolean()
                result.IsReponse = reader.ReadBoolean()
                result.DelaiSinceValidation = reader.ReadInt32()
                result.IsReponseRecue = reader.ReadBoolean()
                result.HorodateLastRecu = New Date(reader.ReadInt64())
                'result.isInactif = reader.ReadBoolean()

            End Using
        End Using
        Return result
    End Function

End Class
