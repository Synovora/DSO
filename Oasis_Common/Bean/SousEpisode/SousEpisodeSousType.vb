Imports System.Configuration

Public Class SousEpisodeSousType

    Property Id As Long
    Property IdSousEpisodeType As Long
    Property HorodateCreation As DateTime
    Property Libelle As String
    Property RedactionProfilTypes As String
    Property ValidationProfilTypes As String
    Property IsALDPossible As Boolean
    Property IsReponseRequise As Boolean
    Property DelaiReponse As Integer
    Property Commentaire As String

    Public Sub New()
    End Sub

    '''
    Public Sub New(row As DataRow)
        Me.Id = row("id")
        Me.IdSousEpisodeType = row("id_sous_episode_type")
        Me.HorodateCreation = row("horodate_creation")
        Me.Libelle = row("libelle")
        Me.ValidationProfilTypes = row("validation_profil_types")
        Me.RedactionProfilTypes = row("redaction_profil_types")
        Me.IsALDPossible = row("is_ald_possible")
        Me.IsReponseRequise = Coalesce(row("is_reponse_requise"), False)
        Me.DelaiReponse = Coalesce(row("delai_reponse"), ConfigurationManager.AppSettings("DelaiDefautReponseSousEpisode"))
        Me.Commentaire = Coalesce(row("commentaire"), False)
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function IsUserLogRedactionAutorise(userLog As Object) As Boolean
        Return IsUserLogAutorise(Me.RedactionProfilTypes, userLog)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function IsUserLogValidationAutorise(userLog As Object) As Boolean
        Return IsUserLogAutorise(Me.ValidationProfilTypes, userLog)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strProfil"></param>
    ''' <returns></returns>
    Public Shared Function IsUserLogAutorise(strProfil As String, userLog As Object) As Boolean
        Dim lst As List(Of ProfilDao.EnumProfilType) = getListProfilsAutoriseList(strProfil)
        Try
            Return lst.Contains([Enum].Parse(GetType(ProfilDao.EnumProfilType), userLog.TypeProfil))
        Catch
        End Try
        Return False
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="profilsString"></param>
    ''' <returns></returns>
    Private Shared Function getListProfilsAutoriseList(profilsString As String) As List(Of ProfilDao.EnumProfilType)
        Dim lst As List(Of ProfilDao.EnumProfilType) = New List(Of ProfilDao.EnumProfilType)
        If String.IsNullOrEmpty(profilsString) Then Return lst
        Dim tbl = profilsString.Split(",")
        For Each strP In tbl
            Try
                Dim enumTypeProfilAutorises As ProfilDao.EnumProfilType = [Enum].Parse(GetType(ProfilDao.EnumProfilType), strP)
                lst.Add(enumTypeProfilAutorises)
            Catch
            End Try
        Next
        Return lst
    End Function

    Public Function getContenuModel(loginRequestLog As Object) As Byte()
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

    Public Sub writeContenuModel(tblContenu As Byte(), loginRequestLog As Object)
        ' --- tentative d'upload
        Using apiOasis As New ApiOasis()
            apiOasis.uploadFileRest(loginRequestLog.login,
                                        loginRequestLog.password,
                                        getFilenameServer(),
                                        tblContenu)
        End Using

    End Sub

    Private Function getFilenameServer() As String
        Return "\Templates\SousEpisodeType_" & Me.IdSousEpisodeType & "_SousType_" & Me.Id & ".DOCX"
    End Function


End Class
