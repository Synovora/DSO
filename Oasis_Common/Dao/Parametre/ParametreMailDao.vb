Imports System.Data.SqlClient
Imports Oasis_Common.ParametreMail

Public Class ParametreMailDao
    Inherits StandardDao

    Private Function BuildBean(reader As SqlDataReader) As ParametreMail
        Dim parametre As New ParametreMail With {
            .Id = reader("id"),
            .SiegeId = Coalesce(reader("siege_id"), 0L),
            .TypeMailParam = DirectCast([Enum].Parse(GetType(TypeMailParams), reader("type_mail_param")), TypeMailParams),
            .SmtpParams = Coalesce(reader("smtp_params"), ""),
            .Objet = Coalesce(reader("objet"), ""),
            .Body = Coalesce(reader("body"), ""),
            .IsBodyHtml = reader("is_body_html")
         }
        Return parametre
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="siegeId"></param>
    ''' <param name="typeParam"></param>
    ''' <returns></returns>
    Public Function GetParametreMailBySiegeIdTypeMailParam(siegeId As Long, typeParam As TypeMailParams) As ParametreMail
        Dim parametre As ParametreMail
        Dim query As String = "
            SELECT TOP 1 * FROM oasis.oa_r_mail_parameter
            WHERE (siege_id is null OR siege_id=@SiegeId)
            AND type_mail_param = @TypeParam 
            ORDER BY ISNULL(siege_id, 0) DESC
        "


        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = query
            command.Parameters.AddWithValue("@SiegeId", siegeId)
            command.Parameters.AddWithValue("@TypeParam", typeParam.ToString())
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    parametre = BuildBean(reader)
                Else
                    Throw New ArgumentException("Paramètre Mail inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return parametre
    End Function

End Class
