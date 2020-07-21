Imports System.Data.SqlClient

Public Class RorDao
    Inherits StandardDao

    Public Structure EnumTypeRor
        Const Intervenant = "Intervenant"
        Const StructureRor = "Structure"
    End Structure

    Public Function getRorById(RorId As Integer) As Ror
        Dim ror As Ror
        Dim con As SqlConnection

        con = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "select * from oasis.oa_ror where oa_ror_id = @id"
            command.Parameters.AddWithValue("@id", RorId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    ror = buildBean(reader)
                Else
                    Throw New ArgumentException("ROR inexistant !")
                End If
            End Using

        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return ror
    End Function

    Private Function buildBean(reader As SqlDataReader) As Ror
        Dim ror As New Ror With {
            .Id = reader("oa_ror_id"),
            .SpecialiteId = Coalesce(reader("oa_ror_specialite_id"), 0),
            .Nom = Coalesce(reader("oa_ror_nom"), ""),
            .Oasis = Coalesce(reader("oa_ror_oasis"), False),
            .Type = Coalesce(reader("oa_ror_type"), ""),
            .StructureId = Coalesce(reader("oa_ror_structure_id"), 0),
            .StructureNom = Coalesce(reader("oa_ror_structure_nom"), ""),
            .Adresse1 = Coalesce(reader("oa_ror_adresse1"), ""),
            .Adresse2 = Coalesce(reader("oa_ror_adresse2"), ""),
            .CodePostal = Coalesce(reader("oa_ror_code_postal"), ""),
            .Ville = Coalesce(reader("oa_ror_ville"), ""),
            .Code = Coalesce(reader("oa_ror_code"), ""),
            .Telephone = Coalesce(reader("oa_ror_telephone"), ""),
            .Email = Coalesce(reader("oa_ror_email"), ""),
            .Commentaire = Coalesce(reader("oa_ror_commentaire"), ""),
            .Rpps = Coalesce(reader("oa_ror_rpps"), 0),
            .Finess = Coalesce(reader("oa_ror_finess"), 0),
            .Adeli = Coalesce(reader("oa_ror_adeli"), 0),
            .Inactif = Coalesce(reader("oa_ror_inactif"), False),
            .UserCreation = Coalesce(reader("oa_ror_user_creation"), 0),
            .DateCreation = Coalesce(reader("oa_ror_date_creation"), Nothing),
            .UserModification = Coalesce(reader("oa_ror_user_modification"), 0),
            .DateModification = Coalesce(reader("oa_ror_date_modification"), Nothing)
        }
        Return ror
    End Function

    Public Function getAllRor() As DataTable
        Dim SQLString As String

        SQLString = "SELECT * FROM oasis.oa_ror where oa_ror_inactif = 'False' or oa_ror_inactif is Null"

        Using con As SqlConnection = GetConnection()
            Dim AldDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using AldDataAdapter
                AldDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim AldDataTable As DataTable = New DataTable()
                Using AldDataTable
                    Try
                        AldDataAdapter.Fill(AldDataTable)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return AldDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function CreationRor(ror As Ror, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection
        con = GetConnection()

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "insert into oasis.oa_ror" &
        " (oa_ror_specialite_id, oa_ror_nom, oa_ror_type, oa_ror_structure_id, oa_ror_structure_nom," &
        " oa_ror_adresse1, oa_ror_adresse2, oa_ror_code_postal, oa_ror_ville," &
        " oa_ror_code, oa_ror_telephone, oa_ror_email, oa_ror_commentaire, oa_ror_rpps, oa_ror_finess, oa_ror_adeli, oa_ror_inactif, oa_ror_user_creation, oa_ror_date_creation)" &
        " VALUES (@specialiteId, @nom, @type, @structureId, @structureNom," &
        " @adresse1, @adresse2, @codePostal, @ville," &
        " @code, @telephone, @email, @commentaire, @rpps, @finess, @adeli, @inactif, @userCreation, @dateCreation)"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@specialiteId", ror.SpecialiteId)
            .AddWithValue("@nom", ror.Nom)
            .AddWithValue("@type", ror.Type)
            .AddWithValue("@structureId", ror.StructureId)
            .AddWithValue("@structureNom", ror.StructureNom)
            .AddWithValue("@adresse1", ror.Adresse1)
            .AddWithValue("@adresse2", ror.Adresse2)
            .AddWithValue("@codePostal", ror.CodePostal)
            .AddWithValue("@ville", ror.Ville)
            .AddWithValue("@code", ror.Code)
            .AddWithValue("@telephone", ror.Telephone)
            .AddWithValue("@email", ror.Email)
            .AddWithValue("@commentaire", ror.Commentaire)
            .AddWithValue("@rpps", ror.Rpps)
            .AddWithValue("@finess", ror.Finess)
            .AddWithValue("@adeli", ror.Adeli)
            .AddWithValue("@inactif", ror.Inactif)
            .AddWithValue("@userCreation", userLog.UtilisateurId)
            .AddWithValue("@dateCreation", Date.Now.ToString("yyyy-MM-dd HH:mm:ss"))
        End With

        Try
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function

    Public Function ModificationRor(ror As Ror, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_ror set" &
        " oa_ror_specialite_id = @specialiteId, oa_ror_nom = @nom, oa_ror_type = @type, oa_ror_structure_id = @structureId," &
        " oa_ror_structure_nom = @structureNom, oa_ror_adresse1 = @adresse1, oa_ror_adresse2 = @adresse2," &
        " oa_ror_code_postal = @codePostal, oa_ror_ville = @ville, oa_ror_code = @code, oa_ror_telephone = @telephone, oa_ror_email = @email, oa_ror_commentaire = @commentaire, oa_ror_rpps = @rpps," &
        " oa_ror_finess = @finess, oa_ror_adeli = @adeli, oa_ror_inactif = @inactif," &
        " oa_ror_user_modification = @userModification, oa_ror_date_modification = @dateModification" &
        " where oa_ror_id = @Id"

        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@Id", ror.Id)
            .AddWithValue("@specialiteId", ror.SpecialiteId)
            .AddWithValue("@nom", ror.Nom)
            .AddWithValue("@type", ror.Type)
            .AddWithValue("@structureId", ror.StructureId)
            .AddWithValue("@structureNom", ror.StructureNom)
            .AddWithValue("@adresse1", ror.Adresse1)
            .AddWithValue("@adresse2", ror.Adresse2)
            .AddWithValue("@codePostal", ror.CodePostal)
            .AddWithValue("@ville", ror.Ville)
            .AddWithValue("@code", "")
            .AddWithValue("@telephone", ror.Telephone)
            .AddWithValue("@email", ror.Email)
            .AddWithValue("@commentaire", ror.Commentaire)
            .AddWithValue("@rpps", ror.Rpps)
            .AddWithValue("@finess", ror.Finess)
            .AddWithValue("@adeli", ror.Adeli)
            .AddWithValue("@inactif", ror.Inactif)
            .AddWithValue("@userModification", userLog.UtilisateurId)
            .AddWithValue("@dateModification", Date.Now.ToString("yyyy-MM-dd HH:mm:ss"))
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            Throw New Exception(ex.Message)
            codeRetour = False
        Finally
            con.Close()
        End Try

        Return codeRetour
    End Function
End Class
