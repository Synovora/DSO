Imports System.Configuration
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
    Property lstDetail As List(Of SousEpisodeDetailSousType)


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

        Me.isALD = row("is_ald")
        Me.IsReponse = Coalesce(row("is_reponse"), False)
        Me.DelaiSinceValidation = Coalesce(row("delai_since_validation"), ConfigurationManager.AppSettings("DelaiDefautReponseSousEpisode"))

        Me.IsReponseRecue = Coalesce(row("is_reponse_recue"), False)
        Me.HorodateLastRecu = Coalesce(row("horodate_last_recu"), Nothing)

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

End Class
