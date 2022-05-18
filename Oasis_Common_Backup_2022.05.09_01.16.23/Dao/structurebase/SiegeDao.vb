Imports System.Data.SqlClient
Imports Oasis_Common

Public Class SiegeDao
    Inherits StandardDao

    ''' <summary>
    ''' retrouve un siege par son id
    ''' </summary>
    ''' <param name="id"></param>
    ''' <param name="isWithInactif"></param>
    ''' <returns></returns>
    Public Function getSiegeById(id As Integer, Optional ByVal isWithInactif As Boolean = False) As Siege
        Dim siege As Siege
        Dim con As SqlConnection
        Dim isWhere As Boolean = False

        con = GetConnection()

        Try

            Dim command As SqlCommand = con.CreateCommand()
            Dim strRequete
            strRequete =
               "select * " &
               "from oasis.oa_siege " &
               "where oa_siege_id = @id"
            If isWithInactif = False Then
                isWhere = True
                strRequete += " AND COALESCE(oa_siege_statut, 'A') = 'A' " + vbCrLf
            End If
            command.CommandText = strRequete
            command.Parameters.AddWithValue("@id", id)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    siege = BuildBean(reader)
                Else
                    Throw New ArgumentException("Siege non retrouvé !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return siege
    End Function

    Public Function getLstSiege(Optional isWithInactif As Boolean = False) As List(Of Siege)
        Dim lst As List(Of Siege) = New List(Of Siege)
        Dim data As DataTable = getTableSiege(isWithInactif)
        For Each row In data.Rows
            lst.Add(BuildBean(row))
        Next
        Return lst
    End Function

    Public Function getTableSiege(Optional isWithInactif As Boolean = False) As DataTable
        Dim SQLString As String
        'Console.WriteLine("----------> getTableSousEpisode")
        SQLString =
           "SELECT * " & vbCrLf &
           "FROM oasis.oa_siege " & vbCrLf

        SQLString += "WHERE 1=1 " & vbCrLf
        If isWithInactif = False Then
            SQLString += " AND COALESCE(oa_siege_statut, 'A') = 'A' " + vbCrLf
        End If

        SQLString += "ORDER by oa_siege_description"

        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim tacheDataTable As DataTable = New DataTable()
                Using tacheDataTable
                    Try
                        tacheDataAdapter.Fill(tacheDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return tacheDataTable
                End Using
            End Using
        End Using
    End Function




    Private Function BuildBean(reader As Object) As Siege
        Dim bean As New Siege

        bean.SiegeId = reader("oa_siege_id")
        bean.SiegeDescription = Coalesce(reader("oa_siege_description"), "")
        bean.SiegeAdresse1 = Coalesce(reader("oa_siege_adresse1"), "")
        bean.SiegeAdresse2 = Coalesce(reader("oa_siege_adresse2"), "")
        bean.SiegeVille = Coalesce(reader("oa_siege_ville"), "")
        bean.SiegeCodePostal = Coalesce(reader("oa_siege_code_postal"), "")
        bean.SiegeTelephone = Coalesce(reader("oa_siege_telephone"), "")
        bean.SiegeMail = Coalesce(reader("oa_siege_mail"), "")
        bean.SiegeFax = Coalesce(reader("oa_siege_fax"), "")
        bean.SiegeStatut = Coalesce(reader("oa_siege_statut"), False)

        Return bean
    End Function


End Class
