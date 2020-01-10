Imports System.Data.SqlClient

Public Class SiteDao
    Inherits StandardDao

    ''' <summary>
    ''' retrouve un site par son id
    ''' </summary>
    ''' <param name="id"></param>
    ''' <param name="isWithInactif"></param>
    ''' <returns></returns>
    Public Function getSiteById(id As Integer, Optional ByVal isWithInactif As Boolean = False) As Site
        Dim site As Site
        Dim con As SqlConnection
        Dim isWhere As Boolean = False

        con = GetConnection()

        Try

            Dim command As SqlCommand = con.CreateCommand()
            Dim strRequete
            strRequete =
               "select * " &
               "from oasis.oa_site " &
               "where oa_site_id = @id"
            If isWithInactif = False Then
                isWhere = True
                strRequete += " AND COALESCE(oa_site_inactif, 0) = 0 " + vbCrLf
            End If
            command.CommandText = strRequete
            command.Parameters.AddWithValue("@id", id)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    site = buildBean(reader)
                Else
                    Throw New ArgumentException("Site non retrouvé !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try


        Return site
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="isWithInactif"></param>
    ''' <param name="uniteSanitaireId"></param>
    ''' <returns></returns>
    Public Function getList(isWithInactif As Boolean, Optional ByVal uniteSanitaireId As Integer = 0) As List(Of Site)
        Dim lstLu As List(Of Site) = New List(Of Site)
        Dim strRequete As String
        Dim isWhere As Boolean = False

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                strRequete =
                       "SELECT * " &
                       "FROM oasis.oa_site " & vbCrLf
                ' -- filtrage des inactifs si pas isWithInactif
                If isWithInactif = False Then
                    isWhere = True
                    strRequete += "WHERE COALESCE(oa_site_inactif, 0) = 0 " + vbCrLf
                End If
                ' -- filtre eventuel par profil
                If uniteSanitaireId <> 0 Then
                    strRequete +=
                        If(isWhere, "AND ", " WHERE ") & " oa_site_unite_sanitaire_id = " & uniteSanitaireId & vbCrLf
                End If
                ' -- order
                strRequete += "ORDER BY oa_site_description"
                command.CommandText = strRequete
                Using reader As SqlDataReader = command.ExecuteReader()
                    Do While reader.Read()
                        lstLu.Add(buildBean(reader))
                    Loop
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return lstLu

    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="reader"></param>
    ''' <returns></returns>
    Private Function buildBean(reader As SqlDataReader) As Site
        Dim bean As New Site

        bean.Oa_site_id = reader("oa_site_id")
        bean.Oa_site_description = Coalesce(reader("oa_site_description"), "")
        bean.Oa_site_territoire_id = Coalesce(reader("oa_site_territoire_id"), 0)
        bean.Oa_site_unite_sanitaire_id = Coalesce(reader("oa_site_unite_sanitaire_id"), 0)
        bean.Oa_site_adresse1 = Coalesce(reader("oa_site_adresse1"), "")
        bean.Oa_site_adresse2 = Coalesce(reader("oa_site_adresse2"), "")
        bean.Oa_site_ville = Coalesce(reader("oa_site_ville"), "")
        bean.Oa_site_code_postal = Coalesce(reader("oa_site_code_postal"), "")
        bean.Oa_site_inactif = Coalesce(reader("oa_site_inactif"), False)

        Return bean
    End Function

End Class
