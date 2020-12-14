Imports System.Data.SqlClient
Public Class MailDao
    Inherits StandardDao

    Private Function BuildBean(reader As SqlDataReader) As Mail
        Dim mail As New Mail With {
            .sendMailKey = reader("sendMailKey"),
            .sendMailTo = reader("sendMailTo"),
            .sendMailCc = reader("sendMailCc"),
            .sendMailBcc = reader("sendMailBcc"),
            .sendMailFrom = reader("sendMailFrom"),
            .sendMailSender = reader("sendMailSender"),
            .sendMailSubject = reader("sendMailSubject"),
            .sendMailMessage = reader("sendMailMessage"),
            .dateCreation = reader("date_creation"),
            .userCreation = reader("user_creation"),
            .sendMailSent = reader("sendMailSent")
        }
        Return mail
    End Function

    Public Function GetProfessionSanteById(sendMailKey As Integer) As Mail
        Dim mail As Mail
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.send_mail_trigger WHERE sendMailKey = @id"
            command.Parameters.AddWithValue("@id", sendMailKey)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    mail = BuildBean(reader)
                Else
                    Throw New ArgumentException("Mail inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return mail
    End Function

    Public Function CreateMail(mail As Mail, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "INSERT INTO oasis.send_mail_trigger " &
        " (sendMailTo, sendMailCc, sendMailBcc, sendMailFrom, sendMailSender, sendMailSubject, sendMailMessage, sendMailSent, date_creation, user_creation)" &
        " VALUES " &
        " (@sendMailTo, @sendMailCc, @sendMailBcc, @sendMailFrom, @sendMailSender, @sendMailSubject, @sendMailMessage, @sendMailSent, @date_creation, @user_creation)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@sendMailTo", mail.sendMailTo)
            .AddWithValue("@sendMailCc", mail.sendMailCc)
            .AddWithValue("@sendMailBcc", mail.sendMailBcc)
            .AddWithValue("@sendMailFrom", mail.sendMailFrom)
            .AddWithValue("@sendMailSender", mail.sendMailSender)
            .AddWithValue("@sendMailSubject", mail.sendMailSubject)
            .AddWithValue("@sendMailMessage", mail.sendMailMessage)
            .AddWithValue("@sendMailSent", "")
            .AddWithValue("@date_creation", Date.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@user_creation", userLog.UtilisateurId)
        End With

        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function
End Class
