Imports System.Data.SqlClient

Public Class InternauteDao
    Inherits StandardDao

    Public Sub Create(internaute As Internaute)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring As String = "INSERT INTO oasis.oa_internaute (" & vbCrLf &
                                     " oa_internaute_username, oa_internaute_password)" & vbCrLf &
                                     " VALUES (" & vbCrLf &
                                     " @oa_internaute_username, @oa_internaute_password);"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@oa_internaute_username", internaute.Username)
                .AddWithValue("@oa_internaute_password", internaute.Password)
            End With

            da.InsertCommand = cmd
            da.InsertCommand.ExecuteScalar()

            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
        Finally
            transaction.Dispose()
            con.Close()
        End Try

    End Sub

    Public Function getUserByLoginPassword(login As String, password As String) As Internaute
        Dim user As Internaute = Nothing

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText =
                   "select U.*, p.* " &
                   "from oasis.oa_utilisateur u " &
                   "inner join oasis.oa_r_profil p on p.oa_r_profil_id = oa_utilisateur_profil_id And COALESCE(oa_r_profil_inactif,'false')='false' " &
                   "where oa_utilisateur_login = @login AND oa_utilisateur_etat='A'"
                command.Parameters.AddWithValue("@login", login)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        user = BuildBean(reader)
                        ControlPassword(user, password)
                    Else
                        Throw New ArgumentException("Identifiant et/ou mot de passe erroné !")
                    End If
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return user
    End Function

    Private Sub ControlPassword(user As Internaute, password As String)
        If user.Password = Internaute.CryptePwd(user.Username, password) Then
            user.Password = password   ' on ne garde que le pasword crypté
            Return
        End If
        Throw New ArgumentException("Identifiant et/ou mot de passe erroné !")
    End Sub

    Public Function GetUserById(userId As Integer) As Internaute
        Dim user As Internaute
        Dim con As SqlConnection

        con = GetConnection()

        Try

            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
               "select U.*, p.* " &
               "from oasis.oa_utilisateur u " &
               "left join oasis.oa_r_profil p on p.oa_r_profil_id = oa_utilisateur_profil_id " &
               "where oa_utilisateur_id = @id"
            command.Parameters.AddWithValue("@id", userId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    user = BuildBean(reader)
                Else
                    Throw New ArgumentException("Utilisateur non retrouvé !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try


        Return user
    End Function

    Public Function BuildBean(reader As SqlDataReader) As Internaute
        Dim user As New Internaute With {
            .Username = reader("oa_internaute_username"),
            .Password = Coalesce(reader("oa_internuate_password"), "")
        }
        Return user
    End Function
End Class
