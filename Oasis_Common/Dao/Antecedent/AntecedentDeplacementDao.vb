Imports System.Data.SqlClient
Imports Oasis_Common
Public Class AntecedentDeplacementDao
    Inherits StandardDao

    Public Function GetAntecedentDeplacementById(antecedentId As Integer) As AntecedentDeplacement
        Dim antecedent As AntecedentDeplacement
        Dim con As SqlConnection
        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "SELECT * FROM oasis.oa_antecedent WHERE oa_antecedent_id = @id"
            command.Parameters.AddWithValue("@id", antecedentId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    antecedent = BuildBean(reader)
                Else
                    Throw New ArgumentException("Antécédent inexistant !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return antecedent
    End Function

    ''' <summary>
    ''' Lit une ligne telle quelle, sans toucher à la base. IDataRecord plutôt que
    ''' SqlDataReader pour qu'un test puisse fournir la ligne.
    ''' </summary>
    Public Shared Function BuildBean(record As System.Data.IDataRecord) As AntecedentDeplacement
        Dim antecedent As New AntecedentDeplacement

        antecedent.Id = record("oa_antecedent_id")
        antecedent.Niveau = Coalesce(record("oa_antecedent_niveau"), 0)
        antecedent.Niveau1Id = Coalesce(record("oa_antecedent_id_niveau1"), 0)
        antecedent.Niveau2Id = Coalesce(record("oa_antecedent_id_niveau2"), 0)
        antecedent.Ordre1 = Coalesce(record("oa_antecedent_ordre_affichage1"), 0)
        antecedent.Ordre2 = Coalesce(record("oa_antecedent_ordre_affichage2"), 0)
        antecedent.Ordre3 = Coalesce(record("oa_antecedent_ordre_affichage3"), 0)
        Return antecedent
    End Function
End Class
