Imports System.Data.SqlClient

Public Class InternauteDao
    Inherits StandardDao

    Public Sub Create(internaute As Internaute)
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring As String = "INSERT INTO oasis.oa_internaute (" & vbCrLf &
                                     " oa_internaute_username,  oa_internaute_password)" & vbCrLf &
                                     " VALUES (" & vbCrLf &
                                     " @oa_internaute_username, @oa_internaute_password);"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            internaute.CryptePwd()
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

    Public Function GetInternauteByLoginPassword(username As String, password As String) As Internaute
        Dim user As Internaute = Nothing

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                'command.CommandText = "SELECT * FROM oasis.oa_internaute WHERE oa_internaute_username = '@oa_internaute_username';"
                command.CommandText = "SELECT * FROM oasis.oa_internaute WHERE oa_internaute_username = 'AmandaRine';"
                'command.Parameters.AddWithValue("@oa_internaute_username", username)
                Using reader As SqlDataReader = command.ExecuteReader()
                    Debug.WriteLine("-1")
                    If reader.Read() Then
                        Debug.WriteLine("0")
                        user = BuildBean(reader)
                        Debug.WriteLine("1")
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

    Public Function GetInternauteByNIR(nir As String) As Internaute
        Dim user As Internaute = Nothing

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText = "SELECT * FROM oasis.oa_internaute WHERE oa_internaute_nir = @nir;"
                command.Parameters.AddWithValue("@nir", nir)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        user = BuildBean(reader)
                    End If
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return user
    End Function

    Private Sub ControlPassword(user As Internaute, password As String)
        Debug.WriteLine("ControlPassword", password)
        If user.Password = Internaute.CryptePwd(user.Username.ToString(), password) Then
            user.Password = password
            Return
        End If
        Throw New ArgumentException("Identifiant et/ou mot de passe erroné !")
    End Sub

    Public Function BuildBean(reader As SqlDataReader) As Internaute
        Dim user As New Internaute With {
            .Username = reader("oa_internaute_username"),
            .Password = Coalesce(reader("oa_internaute_password"), ""),
            .Id = Coalesce(reader("oa_internaute_id"), 0)
        }
        Return user
    End Function
End Class
