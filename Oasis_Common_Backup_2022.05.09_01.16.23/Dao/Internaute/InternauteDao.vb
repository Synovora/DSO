Imports System.Data.SqlClient

Public Class InternauteDao
    Inherits StandardDao

    Public Function Create(internaute As Internaute) As Long
        Dim da As New SqlDataAdapter()
        Dim con As SqlConnection = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction
        Dim Id As Long

        Try
            Dim SQLstring As String = "INSERT INTO oasis.oa_internaute (" & vbCrLf &
                                     " username, email, recovery, code)" & vbCrLf &
                                     " VALUES (" & vbCrLf &
                                     " @username, @email, @recovery, @code);" & vbCrLf &
                                     " SELECT SCOPE_IDENTITY()"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            internaute.CryptePwd()
            With cmd.Parameters
                .AddWithValue("@username", internaute.Username)
                .AddWithValue("@email", internaute.Email)
                .AddWithValue("@recovery", internaute.Recovery)
                .AddWithValue("@code", internaute.Code)
            End With

            da.InsertCommand = cmd
            Id = da.InsertCommand.ExecuteScalar()

            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            Throw New Exception(ex.Message)
        Finally
            transaction.Dispose()
            con.Close()
        End Try
        Return Id
    End Function

    Public Function GetInternauteByLoginPassword(email As String, password As String) As Internaute
        Dim user As Internaute = Nothing

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText = "SELECT * FROM oasis.oa_internaute WHERE email = @email;"
                command.Parameters.AddWithValue("@email", email)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        user = New Internaute(reader)
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

    Public Function GetInternauteByRecoveryKey(recovery As String) As Internaute
        Dim user As Internaute = Nothing

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText = "SELECT * FROM oasis.oa_internaute WHERE recovery = @recovery;"
                command.Parameters.AddWithValue("@recovery", recovery)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        user = New Internaute(reader)
                    End If
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return user
    End Function

    Public Function GetInternauteById(id As Long) As Internaute
        Dim user As Internaute = Nothing

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText = "SELECT * FROM oasis.oa_internaute WHERE id=@id;"
                command.Parameters.AddWithValue("@id", id)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        user = New Internaute(reader)
                    End If
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return user
    End Function

    Public Function Update(internaute As Internaute) As Long
        Dim da As New SqlDataAdapter()
        Dim internauteId As Long

        Dim SQLstring As String = "UPDATE oasis.oa_internaute SET password=@password, recovery=@recovery, code=@code WHERE id=@id;"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@id", internaute.Id)
            .AddWithValue("@password", internaute.Password)
            .AddWithValue("@recovery", internaute.Recovery)
            .AddWithValue("@code", internaute.Code)
        End With

        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteScalar()
            internauteId = internaute.Id
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return internauteId
    End Function

    Public Function GetInternauteByNIR(nir As String) As Internaute
        Dim user As Internaute = Nothing

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText = "SELECT * FROM oasis.oa_internaute WHERE nir = @nir;"
                command.Parameters.AddWithValue("@nir", nir)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        user = New Internaute(reader)
                    End If
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return user
    End Function

    Public Function GetInternauteByEmail(email As String) As Internaute
        Dim user As Internaute = Nothing

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                command.CommandText = "SELECT * FROM oasis.oa_internaute WHERE email = @email;"
                command.Parameters.AddWithValue("@email", email)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        user = New Internaute(reader)
                    End If
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return user
    End Function

    Private Sub ControlPassword(user As Internaute, password As String)
        If user.Password = Internaute.CryptePwd(user.Email.ToString(), password) Then
            user.Password = password
            Return
        End If
        Throw New ArgumentException("Identifiant et/ou mot de passe erroné !")
    End Sub

End Class
