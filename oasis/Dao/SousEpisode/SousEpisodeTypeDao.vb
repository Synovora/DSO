Imports System.Data.SqlClient
Imports Oasis_WF
Imports Oasis_Common

Public Enum CategorieSE
    PRESCRIPTION
    CERTIFICAT
    COURRIER
    COMPTE_RENDU
End Enum

Public Class SousEpisodeTypeDao
    Inherits StandardDao

    Public Function getLstSousEpisodeType() As List(Of SousEpisodeType)
        Dim lst As List(Of SousEpisodeType) = New List(Of SousEpisodeType)
        Dim data As DataTable = getTableSousEpisodeType()
        For Each row In data.Rows
            lst.Add(buildBean(row))
        Next
        Return lst
    End Function

    Public Function getTableSousEpisodeType() As DataTable
        Dim SQLString As String
        'Console.WriteLine("----------> getAllTacheEnCours")
        SQLString =
            "SELECT " & vbCrLf &
            "	  id, " & vbCrLf &
            "     categorie, " & vbCrLf &
            "     horodate_creation, " & vbCrLf &
            "	  libelle " & vbCrLf &
            "FROM [oasis].[oa_r_sous_episode_type] " & vbCrLf '&
        '"WHERE type<> @type " & vbCrLf

        'SQLString += "ORDER BY priorite,ordre_affichage, COALESCE(date_rendez_vous, horodate_creation) "
        'Console.WriteLine(SQLString)

        Using con As SqlConnection = GetConnection()

            Dim tacheDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using tacheDataAdapter
                tacheDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                'tacheDataAdapter.SelectCommand.Parameters.AddWithValue("@etat", EtatTache.EN_COURS.ToString)
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


    Public Function Create(seType As SousEpisodeType) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection

        con = GetConnection()
        Dim transaction As SqlClient.SqlTransaction = con.BeginTransaction

        Try
            Dim SQLstring As String = "INSERT INTO oa_r_sous_episode_type " &
                    "(categorie, horodate_creation, libelle)" &
            " VALUES (@categorie, @dateCreation, @libelle)"

            Dim cmd As New SqlCommand(SQLstring, con, transaction)
            With cmd.Parameters
                .AddWithValue("@categorie", seType.Category)
                .AddWithValue("@dateCreation", seType.HorodateCreation)
                .AddWithValue("@libelle", seType.Libelle)
            End With

            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()

            transaction.Commit()

        Catch ex As Exception
            transaction.Rollback()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            transaction.Dispose()
            con.Close()
        End Try

        Return codeRetour
    End Function

    Private Function buildBean(row As DataRow) As SousEpisodeType
        Dim seType As New SousEpisodeType
        seType.Id = row("id")
        seType.Category = Coalesce(row("categorie"), "")
        seType.HorodateCreation = row("horodate_creation")
        seType.Libelle = row("libelle")
        seType.LstSousEpisodeSousType = New SousEpisodeSousTypeDao().getLstSousEpisodeSousType(seType.Id)
        Return seType
    End Function

End Class
