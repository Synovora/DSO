Imports System.Data.SqlClient

Public Class AnnuaireProfessionnelSanteComplementDao
    Inherits StandardDao

    ''' <summary>
    ''' Lit une ligne telle quelle, sans toucher à la base. IDataRecord plutôt que
    ''' SqlDataReader pour qu'un test puisse fournir la ligne.
    ''' </summary>
    Public Shared Function BuildBean(record As System.Data.IDataRecord) As AnnuaireProfessionnelReferenceComplement
        Dim annuaireComplement As New AnnuaireProfessionnelReferenceComplement With {
            .Cle_entree = record("Cle_entree"),
            .RaisonSociale = Coalesce(record("raison_sociale"), ""),
            .Adresse1 = Coalesce(record("adresse1"), ""),
            .Adresse2 = Coalesce(record("adresse2"), ""),
            .Telephone = Coalesce(record("telephone"), ""),
            .Telecopie = Coalesce(record("telecopie"), ""),
            .EmailStructure = Coalesce(record("email_structure"), "")
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
