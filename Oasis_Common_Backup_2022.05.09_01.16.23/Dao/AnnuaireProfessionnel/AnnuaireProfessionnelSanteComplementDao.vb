Imports System.Data.SqlClient

Public Class AnnuaireProfessionnelSanteComplementDao
    Inherits StandardDao

    Private Function BuildBean(reader As SqlDataReader) As AnnuaireProfessionnelReferenceComplement
        Dim annuaireComplement As New AnnuaireProfessionnelReferenceComplement With {
            .Cle_entree = reader("Cle_entree"),
            .RaisonSociale = Coalesce(reader("raison_sociale"), ""),
            .Adresse1 = Coalesce(reader("adresse1"), ""),
            .Adresse2 = Coalesce(reader("adresse2"), ""),
            .Telephone = Coalesce(reader("telephone"), ""),
            .Telecopie = Coalesce(reader("telecopie"), ""),
            .EmailStructure = Coalesce(reader("email_structure"), "")
        }
        Return annuaireComplement
    End Function

    Public Function GetAnnuaireProfessionnelById(annuaireProfessionneld As Integer) As AnnuaireProfessionnelReferenceComplement
        Dim annuaireComplement As AnnuaireProfessionnelReferenceComplement
        Dim con As SqlConnection = GetConnection()
        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.ans_annuaire_professionnel_sante_reference_complement WHERE Cle_entree = @id"
            command.Parameters.AddWithValue("@id", annuaireProfessionneld)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    annuaireComplement = BuildBean(reader)
                Else
                    Throw New ArgumentException("Professionnel de santé inexistant !")
                End If
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try
        Return annuaireComplement
    End Function
End Class
