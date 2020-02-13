Imports System.Windows.Documents

Public Class SousEpisodeSousType
    Property Id As Long
    Property IdSousEpisodeType As Long
    Property HorodateCreation As DateTime
    Property Libelle As String
    Property RedactionProfilTypes As String
    Property ValidationProfilTypes As String
    Property isALDPossible As Boolean

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
        Me.isALDPossible = row("is_ald_possible")
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="profilsString"></param>
    ''' <returns></returns>
    Public Function getListProfilsAutoriseList(profilsString As String) As List(Of EpisodeDao.EnumTypeProfil)
        Dim lst As List(Of EpisodeDao.EnumTypeProfil) = New List(Of EpisodeDao.EnumTypeProfil)
        If String.IsNullOrEmpty(profilsString) Then Return lst
        Dim tbl = profilsString.Split(",")
        For Each strP In tbl
            Try
                Dim enumTypeProfilAutorises As EpisodeDao.EnumTypeProfil = [Enum].Parse(GetType(EpisodeDao.EnumTypeProfil), strP)
                lst.Add(enumTypeProfilAutorises)
            Catch
            End Try
        Next
        Return lst
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function isUserLogRedactionAutorise() As Boolean
        Return isUserLogAutorise(Me.RedactionProfilTypes)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <returns></returns>
    Public Function isUserLogValidationAutorise() As Boolean
        Return isUserLogAutorise(Me.ValidationProfilTypes)
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="strProfil"></param>
    ''' <returns></returns>
    Private Function isUserLogAutorise(strProfil As String) As Boolean
        Dim lst As List(Of EpisodeDao.EnumTypeProfil) = getListProfilsAutoriseList(strProfil)
        Try
            Return lst.Exists([Enum].Parse(GetType(EpisodeDao.EnumTypeProfil), userLog.TypeProfil))
        Catch
        End Try
        Return False
    End Function


End Class
