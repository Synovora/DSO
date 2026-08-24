Imports System.Data.SqlClient
Imports Oasis_Common.ParametreMail

Public Class ParametreMailDao
    Inherits StandardDao

    ''' <summary>
    ''' smtp_params porte les identifiants du compte d'envoi. Seul le serveur en a
    ''' besoin, et lui seul a le droit de lire la colonne : les postes clients ne
    ''' viennent chercher ici que le modèle de message (objet, corps, format).
    ''' </summary>
    ''' <summary>
    ''' Lit une ligne de paramètre de courriel telle quelle, sans toucher à la base.
    ''' Le compte SMTP n'est restitué que si inclureSmtp est vrai : le client lourd
    ''' n'a pas à le connaître, et la base lui en refuse d'ailleurs la lecture.
    ''' </summary>
    Public Shared Function BuildBean(record As System.Data.IDataRecord, inclureSmtp As Boolean) As ParametreMail
        Dim parametre As New ParametreMail With {
            .Id = record("id"),
            .SiegeId = Coalesce(record("siege_id"), 0L),
            .TypeMailParam = DirectCast([Enum].Parse(GetType(TypeMailParams), record("type_mail_param")), TypeMailParams),
            .SmtpParams = If(inclureSmtp, Coalesce(record("smtp_params"), ""), ""),
            .Objet = Coalesce(record("objet"), ""),
            .Body = Coalesce(record("body"), ""),
            .IsBodyHtml = record("is_body_html")
         }
        Return parametre
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="siegeId"></param>
    ''' <param name="typeParam"></param>
    ''' <returns></returns>
    ''' <param name="inclureSmtp">
    ''' True uniquement côté serveur, pour l'envoi effectif du message. Un SELECT *
    ''' échouerait en bloc sur un poste client, à qui la lecture de smtp_params est
    ''' refusée, d'où la liste de colonnes explicite.
    ''' </param>
    Public Function GetParametreMailBySiegeIdTypeMailParam(siegeId As Long, typeParam As TypeMailParams,
                                                           Optional inclureSmtp As Boolean = False) As ParametreMail
        Dim parametre As ParametreMail
        Dim colonnes = "id, siege_id, type_mail_param, objet, body, is_body_html" &
                       If(inclureSmtp, ", smtp_params", "")
        Dim query As String = "
            SELECT TOP 1 " & colonnes & " FROM oasis.oa_r_mail_parameter
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
                    parametre = BuildBean(reader, inclureSmtp)
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
