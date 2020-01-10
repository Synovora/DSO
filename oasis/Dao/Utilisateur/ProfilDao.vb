Imports System.Data.SqlClient

Public Class ProfilDao
    Inherits StandardDao

    Public Enum EnumProfilType
        PARAMEDICAL
        MEDICAL
        GESTION
    End Enum

    Public Function getProfilById(id As String) As Profil
        Dim profil As Profil
        Dim con As SqlClient.SqlConnection

        con = GetConnection()

        Try

            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
               "SELECT * " &
               "FROM oasis.oa_r_profil " &
               "WHERE oa_r_profil_id = @id"
            command.Parameters.AddWithValue("@id", id)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    profil = buildBean(con, reader)
                Else
                    Throw New ArgumentException("Profil non retrouvée !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try


        Return profil
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="con"></param>
    ''' <param name="reader"></param>
    ''' <returns></returns>
    Public Function buildBean(con As SqlConnection, reader As SqlDataReader) As Profil
        Dim profil As New Profil

        profil.Id = reader("oa_r_profil_id")
        profil.Designation = Coalesce(reader("oa_r_profil_designation"), "")
        profil.Type = Coalesce(reader("oa_r_profil_type"), "")
        profil.FonctionParDefautId = Coalesce(reader("oa_r_profil_fonction_id_defaut"), 0)
        profil.NiveauAcces = Coalesce(reader("oa_r_profil_niveau_acces"), 0)
        profil.Inactif = Coalesce(reader("oa_r_profil_inactif"), False)

        Return profil
    End Function
End Class
