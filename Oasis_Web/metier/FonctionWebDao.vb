Imports System.Data.SqlClient
Imports Oasis_Web

Public Class FonctionDao
    Inherits StandardWebDao

    Public Enum enumTypeFonction
        MEDICAL
        PARAMEDICAL
        GESTION
    End Enum


    Public Enum enumFonction
        IDE = 4
        IDE_REMPLACANT = 4
        MEDECIN = 1
        SAGE_FEMME = 7
        CADRE_SANTE = 13
        SECRETAIRE_MEDICALE = 10
        ADMINISTRATIF = 11
        INCONNU = 14
        SPECIALISTE_NON_OASIS = 15
    End Enum


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="isWithInactif"></param>
    ''' <param name="profilId"></param>
    ''' <returns></returns>
    Public Function getList(isWithInactif As Boolean, Optional ByVal profilId As String = "") As List(Of Fonction)
        Dim lstFonction As List(Of Fonction) = New List(Of Fonction)
        Dim strRequete As String
        Dim isWhere As Boolean = False

        Using con As SqlConnection = getConnection()
            Dim command As SqlCommand = con.CreateCommand()
            Try
                strRequete =
                       "SELECT F.* " &
                       "FROM oasis.oa_r_fonction F " & vbCrLf
                ' -- filtrage des inactifs si pas isWithInactif
                If isWithInactif = False Then
                    isWhere = True
                    strRequete += "WHERE COALESCE(oa_r_fonction_inactif, 0) = 0 " + vbCrLf
                End If
                ' -- filtre eventuel par profil
                If profilId <> "" Then
                    strRequete +=
                        If(isWhere, "AND EXISTS ", " WHERE EXISTS ") & vbCrLf &
                        "   (SELECT 1 FROM oasis.oa_asso_profil_fonction APF " & vbCrLf &
                        "    WHERE APF.profil_fonction_id_fonction = F.oa_r_fonction_id AND APF.profil_fonction_id_profil = @profil_id )" & vbCrLf
                End If
                ' -- order
                strRequete += "ORDER BY oa_r_fonction_libelle"
                command.CommandText = strRequete
                If profilId <> "" Then command.Parameters.AddWithValue("@profil_id", profilId)
                Using reader As SqlDataReader = command.ExecuteReader()
                    Do While reader.Read()
                        lstFonction.Add(buildBean(con, reader))
                    Loop
                End Using
            Catch ex As Exception
                Throw ex
            End Try
        End Using
        Return lstFonction

    End Function


    ''' <summary>
    ''' retrouve une fonction par son id
    ''' </summary>
    ''' <param name="id"></param>
    ''' <returns></returns>
    Public Function getFonctionById(id As Long) As Fonction
        Dim user As Fonction
        Dim con As SqlConnection

        con = GetConnection()

        Try

            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
               "select * " &
               "from oasis.oa_r_fonction " &
               "where oa_r_fonction_id = @id"
            command.Parameters.AddWithValue("@id", id)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    user = buildBean(con, reader)
                Else
                    Throw New ArgumentException("fonction non retrouvée !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try


        Return user
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="reader"></param>
    ''' <returns></returns>
    Public Function buildBean(con As SqlConnection, reader As SqlDataReader) As Fonction
        Dim fonction As New Fonction

        fonction.Id = reader("oa_r_fonction_id")
        fonction.Designation = Coalesce(reader("oa_r_fonction_designation"), "")
        fonction.Libelle = Coalesce(reader("oa_r_fonction_libelle"), "")
        fonction.Type = Coalesce(reader("oa_r_fonction_type"), "")
        fonction.RorId = Coalesce(reader("oa_r_fonction_ror_id"), 0)
        fonction.IsInactif = Coalesce(reader("oa_r_fonction_inactif"), False)
        Return fonction
    End Function


    Public Function getFonctionByRorId(RorId As Long) As Fonction
        Dim fonction As Fonction
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText =
               "SELECT TOP(1) * " &
               "FROM oasis.oa_r_fonction " &
               "WHERE oa_r_fonction_ror_id = @RorId"
            command.Parameters.AddWithValue("@RorId", RorId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    fonction = buildBean(con, reader)
                Else
                    Throw New ArgumentException("fonction non retrouvée !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return fonction
    End Function
End Class
