Imports System.Data.SqlClient
Imports Oasis_Common

Public Class UniteSanitaireDao
    Inherits StandardDao

    ''' <summary>
    ''' retrouve une unite sanitaire par son id
    ''' </summary>
    ''' <param name="id"></param>
    ''' <param name="isWithInactif"></param>
    ''' <returns></returns>
    Public Function getUniteSanitaireById(id As Integer, Optional ByVal isWithInactif As Boolean = False) As UniteSanitaire
        Dim uniteSanitaire As UniteSanitaire
        Dim con As SqlConnection
        Dim isWhere As Boolean = False

        con = GetConnection()

        Try

            Dim command As SqlCommand = con.CreateCommand()
            Dim strRequete
            strRequete =
               "select * " &
               "from oasis.oa_unite_sanitaire " &
               "where oa_unite_sanitaire_id = @id"
            If isWithInactif = False Then
                isWhere = True
                strRequete += " AND COALESCE(oa_unite_sanitaire_inactif, 0) = 0 " + vbCrLf
            End If
            command.CommandText = strRequete
            command.Parameters.AddWithValue("@id", id)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    uniteSanitaire = buildBean(reader)
                Else
                    Throw New ArgumentException("Unité sanitaire non retrouvée !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try


        Return uniteSanitaire
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="isWithInactif"></param>
    ''' <param name="siegeId"></param>
    ''' <returns></returns>
    Public Function getList(isWithInactif As Boolean, Optional ByVal siegeId As Integer = 0) As List(Of UniteSanitaire)
        Dim lstLu As List(Of UniteSanitaire) = New List(Of UniteSanitaire)
        Dim strRequete As String
        Dim isWhere As Boolean = False

        Using con As SqlConnection = GetConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                strRequete =
                       "SELECT * " &
                       "FROM oasis.oa_unite_sanitaire " & vbCrLf
                ' -- filtrage des inactifs si pas isWithInactif
                If isWithInactif = False Then
                    isWhere = True
                    strRequete += "WHERE COALESCE(oa_unite_sanitaire_inactif, 0) = 0 " + vbCrLf
                End If
                ' -- filtre eventuel par profil
                If siegeId <> 0 Then
                    strRequete +=
                        If(isWhere, "AND ", " WHERE ") & " oa_unite_sanitaire_siege_id = " & siegeId & vbCrLf
                End If
                ' -- order
                strRequete += "ORDER BY oa_unite_sanitaire_description"
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
    Private Function buildBean(reader As SqlDataReader) As UniteSanitaire
        Dim bean As New UniteSanitaire With {
            .Oa_unite_sanitaire_id = reader("oa_unite_sanitaire_id"),
            .Oa_unite_sanitaire_description = Coalesce(reader("oa_unite_sanitaire_description"), ""),
            .Oa_unite_sanitaire_siege_id = Coalesce(reader("oa_unite_sanitaire_siege_id"), 0),
            .Oa_unite_sanitaire_adresse1 = Coalesce(reader("oa_unite_sanitaire_adresse1"), ""),
            .Oa_unite_sanitaire_adresse2 = Coalesce(reader("oa_unite_sanitaire_adresse2"), ""),
            .Oa_unite_sanitaire_ville = Coalesce(reader("oa_unite_sanitaire_ville"), ""),
            .Oa_unite_sanitaire_code_postal = Coalesce(reader("oa_unite_sanitaire_code_postal"), ""),
            .Telephone = Coalesce(reader("telephone"), ""),
            .Mail = Coalesce(reader("mail"), ""),
            .Fax = Coalesce(reader("fax"), ""),
            .Oa_unite_sanitaire_inactif = Coalesce(reader("oa_unite_sanitaire_inactif"), False),
            .NumeroStructure = Coalesce(reader("numero_structure"), 0)
        }

        Return bean
    End Function


End Class
