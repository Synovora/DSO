Imports System.Data.SqlClient
Imports Oasis_Common
Public Class MedicamentDao
    Inherits StandardDao

    Public Function GetMedicamentById(medicamentCis As Integer) As Medicament
        Dim medicament As Medicament
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_r_medicament WHERE oa_medicament_cis = @Id"
            command.Parameters.AddWithValue("@id", medicamentCis)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    medicament = BuildBean(reader)
                Else
                    Throw New ArgumentException("Médicament inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return medicament
    End Function

    Private Function BuildBean(reader As SqlDataReader) As Medicament
        Dim medicament As New Medicament

        medicament.MedicamentCis = reader("oa_medicament_cis")
        medicament.MedicamentDci = Coalesce(reader("oa_medicament_dci"), "")
        medicament.Forme = Coalesce(reader("oa_medicament_forme"), "")
        medicament.Titulaire = Coalesce(reader("oa_medicament_titulaire"), "")
        medicament.VoieAdministration = Coalesce(reader("oa_medicament_voie_administration"), "")

        Return medicament
    End Function

End Class
