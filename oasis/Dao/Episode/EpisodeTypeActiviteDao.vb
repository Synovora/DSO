Imports System.Configuration
Imports System.Data.SqlClient

Public Class EpisodeTypeActiviteDao
    Inherits StandardDao

    Friend Function GetActiviteEpisodeById(typeId As String) As EpisodeTypeActivite
        Dim episodeActivite As EpisodeTypeActivite
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "select * from oasis.oa_r_activite_episode where activite_type = @typeId"
            command.Parameters.AddWithValue("@typeId", typeId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    episodeActivite = BuildBean(reader)
                Else
                    Throw New ArgumentException("Activité épisode inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return episodeActivite
    End Function

    Private Function BuildBean(reader As SqlDataReader) As EpisodeTypeActivite
        Dim episodeActivite As New EpisodeTypeActivite

        episodeActivite.Type = reader("oa_activite_type")
        episodeActivite.Nature = Coalesce(reader("oa_activite_nature"), "")
        episodeActivite.Description = Coalesce(reader("oa_activite_description"), "")
        episodeActivite.Inactif = Coalesce(reader("oa_activite_inactif"), False)
        Return episodeActivite
    End Function

    Public Function GetAllEpisodeActivite() As DataTable
        Dim SQLString As String
        SQLString = "SELECT * FROM oasis.oa_r_activite_episode" &
                        " WHERE (oa_activite_inactif = 'False' or oa_activite_inactif is Null)" &
                        " ORDER BY oa_activite_ordre"

        Dim dt As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Using da
                da.SelectCommand = New SqlCommand(SQLString, con)
                Try
                    da.Fill(dt)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                Finally
                    con.Close()
                End Try
            End Using
        End Using

        Return dt
    End Function

    Friend Function GetTypeActiviteEpisodeByPatient(patient As Patient) As List(Of String)
        Dim LimiteAgeEnfantParm As Integer

        Dim episodeActiviteDT As DataTable
        Dim episodeActiviteDao As New EpisodeTypeActiviteDao
        Dim episodeDao As New EpisodeDao

        Dim TypeActiviteEpisode As String
        Dim LimiteAgeEnfant As Integer = 0
        Dim AgeMinPreventionFemme As Integer = 0
        LimiteAgeEnfantParm = ConfigurationManager.AppSettings("limiteAgeEnfant")
        If IsNumeric(LimiteAgeEnfantParm) Then
            LimiteAgeEnfant = CInt(LimiteAgeEnfantParm)
        End If
        Dim AgeMinPreventionFemmeParm As Integer = ConfigurationManager.AppSettings("AgeMinPreventionFemme")
        If IsNumeric(AgeMinPreventionFemmeParm) Then
            AgeMinPreventionFemme = CInt(AgeMinPreventionFemmeParm)
        End If

        Dim genre, enfant As String
        Dim agePatient As Integer = outils.CalculAge(patient.PatientDateNaissance)

        Dim listActivite As New List(Of String)
        episodeActiviteDT = episodeActiviteDao.GetAllEpisodeActivite
        Dim i As Integer
        Dim rowCount As Integer = episodeActiviteDT.Rows.Count - 1
        For i = 0 To rowCount Step 1
            genre = Coalesce(episodeActiviteDT.Rows(i)("oa_activite_genre"), "")
            If genre = "F" Then
                If patient.PatientGenreId.Trim = "M" Then
                    Continue For
                End If
                If agePatient < AgeMinPreventionFemme Then
                    Continue For
                End If
            End If
            enfant = Coalesce(episodeActiviteDT.Rows(i)("oa_activite_enfant"), False)
            If enfant = True Then
                If LimiteAgeEnfant <> 0 Then
                    If agePatient > LimiteAgeEnfant Then
                        Continue For
                    End If
                End If
            End If
            If episodeActiviteDT.Rows(i)("oa_activite_type") = EpisodeDao.EnumTypeActiviteEpisodeCode.SOCIAL Then
                Continue For
            End If

            TypeActiviteEpisode = EpisodeDao.GetItemTypeActiviteByCode(episodeActiviteDT.Rows(i)("oa_activite_type"))

            If TypeActiviteEpisode <> "" Then
                listActivite.Add(TypeActiviteEpisode)
            End If
        Next

        Return listActivite
    End Function

End Class
