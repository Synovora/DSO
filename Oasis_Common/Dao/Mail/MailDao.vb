Imports System.Data.SqlClient
Public Class MailDao
    Inherits StandardDao

    ''' <summary>
    ''' Lit une ligne telle quelle, sans toucher à la base. IDataRecord plutôt que
    ''' SqlDataReader pour qu'un test puisse fournir la ligne.
    ''' </summary>
    Public Shared Function BuildBean(record As System.Data.IDataRecord) As MailDB
        Dim mail As New MailDB With {
            .sendMailKey = record("sendMailKey"),
            .sendMailTo = record("sendMailTo"),
            .sendMailCc = record("sendMailCc"),
            .sendMailBcc = record("sendMailBcc"),
            .sendMailFrom = record("sendMailFrom"),
            .sendMailSender = record("sendMailSender"),
            .sendMailSubject = record("sendMailSubject"),
            .sendMailMessage = record("sendMailMessage"),
            .sendMailPath = record("sendMailPath"),
            .dateCreation = record("date_creation"),
            .userCreation = record("user_creation"),
            .sendMailSent = record("sendMailSent")
        }
        Return mail
    End Function

    Public Function GetProfessionSanteById(sendMailKey As Integer) As MailDB
        Dim mail As MailDB
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

    Public Function CreateMail(mail As MailDB, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "INSERT INTO oasis.send_mail_trigger " &
        " (sendMailTo, sendMailCc, sendMailBcc, sendMailFrom, sendMailSender, sendMailSubject, sendMailMessage, sendMailPath, sendMailSent, date_creation, user_creation)" &
        " VALUES " &
        " (@sendMailTo, @sendMailCc, @sendMailBcc, @sendMailFrom, @sendMailSender, @sendMailSubject, @sendMailMessage, @sendMailPath, @sendMailSent, @date_creation, @user_creation)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@sendMailTo", mail.sendMailTo)
            .AddWithValue("@sendMailCc", mail.sendMailCc)
            .AddWithValue("@sendMailBcc", mail.sendMailBcc)
            .AddWithValue("@sendMailFrom", mail.sendMailFrom)
            .AddWithValue("@sendMailSender", mail.sendMailSender)
            .AddWithValue("@sendMailSubject", mail.sendMailSubject)
            .AddWithValue("@sendMailMessage", mail.sendMailMessage)
            .AddWithValue("@sendMailPath", mail.sendMailPath)
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
