Imports System.Data.SqlClient

Public Class SpecialiteDao
    Inherits StandardDao

    Private Function BuildBean(reader As SqlDataReader) As Specialite
        Dim specialite As New Specialite With {
            .SpecialiteId = reader("oa_r_specialite_id"),
            .Code = Coalesce(reader("oa_specialite_code"), ""),
            .Description = Coalesce(reader("oa_r_specialite_description"), ""),
            .Nature = Coalesce(reader("oa_r_specialite_nature"), ""),
            .Parcours = Coalesce(reader("oa_r_parcours"), False),
            .Oasis = Coalesce(reader("oa_r_oasis"), False),
            .Genre = Coalesce(reader("oa_r_specialite_genre"), ""),
            .AgeMin = Coalesce(reader("oa_r_specialite_age_min"), 0),
            .AgeMax = Coalesce(reader("oa_r_specialite_age_max"), 0),
            .DelaiPriseEnCharge = Coalesce(reader("oa_r_delaiPriseEnCharge"), 0),
            .NosG15CodeProfession = Coalesce(reader("oa_r_code_nos_g15_profession"), 0),
            .NosR40TypeSavoirFaire = Coalesce(reader("oa_r_code_nos_r04_type_savoir_faire"), ""),
            .NosCodeSavoirFaire = Coalesce(reader("oa_r_code_savoir_faire"), "")
        }
        Return Specialite
    End Function

    Public Function GetSpecialiteById(specialiteld As Integer) As Specialite
        Dim specialite As Specialite
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_r_specialite WHERE oa_r_specialite_id = @id"
            command.Parameters.AddWithValue("@id", specialiteld)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    specialite = BuildBean(reader)
                Else
                    Throw New ArgumentException("Spécialité inexistante !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return specialite
    End Function
End Class
