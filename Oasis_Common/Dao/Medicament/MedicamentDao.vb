Imports System.Data.SqlClient

Public Class MedicamentDao
    Inherits StandardDao

    ''' <summary>
    ''' Lit une ligne telle quelle, sans toucher à la base. IDataRecord plutôt que
    ''' SqlDataReader pour qu'un test puisse fournir la ligne.
    ''' </summary>
    Public Shared Function BuildBean(record As System.Data.IDataRecord) As Medicament
        Dim medicament As New Medicament With {
            .MedicamentCis = record("oa_medicament_cis"),
            .MedicamentDci = Coalesce(record("oa_medicament_dci"), ""),
            .Forme = Coalesce(record("oa_medicament_forme"), ""),
            .Titulaire = Coalesce(record("oa_medicament_titulaire"), ""),
            .VoieAdministration = Coalesce(record("oa_medicament_voie_administration"), "")
        }
        Return medicament
    End Function

    Public Function GetMedicamentById(medicamentCis As Integer) As Medicament
        Dim medicament As Medicament
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_r_medicament WHERE oa_medicament_cis = @Id"
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

End Class
