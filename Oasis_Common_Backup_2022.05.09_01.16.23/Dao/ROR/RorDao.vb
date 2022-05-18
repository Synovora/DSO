Imports System.Data.SqlClient

Public Class RorDao
    Inherits StandardDao

    Public Structure EnumTypeRor
        Const Intervenant = "Intervenant"
        Const StructureRor = "Structure"
    End Structure

    Public Function GetRorById(RorId As Integer) As Ror
        Dim ror As Ror
        Dim con As SqlConnection = GetConnection()

        Try
            Dim command As SqlCommand = con.CreateCommand()

            command.CommandText =
                "select * from oasis.oa_ror where oa_ror_id = @id"
            command.Parameters.AddWithValue("@id", RorId)
            Using reader As SqlDataReader = command.ExecuteReader()
                If reader.Read() Then
                    ror = New Ror(reader)
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

    Public Function GetAllRor() As DataTable
        Dim SQLString As String = "SELECT * FROM oasis.oa_ror where oa_ror_inactif = 'False' or oa_ror_inactif is Null"

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

    Public Function GetRorBySpecialiteAndType(specialiteId As Integer, typeRor As String) As DataTable
        Dim SQLString As String = "SELECT oa_ror_id, oa_ror_specialite_id, oa_ror_nom, oa_ror_type," &
        " oa_ror_structure_nom, oa_ror_adresse1, oa_ror_adresse2, oa_ror_code_postal," &
        " oa_ror_ville, oa_ror_cle_reference, oa_ror_extraction_annuaire, S.oa_r_specialite_description" &
        " FROM oasis.oa_ror" &
        " LEFT JOIN oasis.oa_r_specialite S ON S.oa_r_specialite_id = oa_ror_specialite_id"

        Dim ClauseWhere As String = " WHERE (oa_ror_annuaire_inactif = '0' OR oa_ror_annuaire_inactif is Null) "

        If specialiteId <> 0 Then
            ClauseWhere += " AND oa_ror_specialite_id = " & specialiteId
        End If

        If typeRor <> "" Then
            ClauseWhere += " AND oa_ror_type = '" & typeRor.Trim & "'"
        End If

        Dim ClauseOrderBy As String = " ORDER BY oa_ror_nom ASC;"

        SQLString += ClauseWhere
        SQLString += ClauseOrderBy

        Using con As SqlConnection = GetConnection()
            Dim da As SqlDataAdapter = New SqlDataAdapter()
            Using da
                da.SelectCommand = New SqlCommand(SQLString, con)
                Dim dt As DataTable = New DataTable()
                Using dt
                    Try
                        da.Fill(dt)
                        Dim command As SqlCommand = con.CreateCommand()
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return dt
                End Using
            End Using
        End Using
    End Function

    Public Function GetListRorByNomAndCommune(nomExercice As String, villeExercice As String, departementExercice As String) As List(Of Ror)
        Dim con As SqlConnection = GetConnection()
        Dim rors As New List(Of Ror)

        Try
            Dim command As SqlCommand = con.CreateCommand()
            command.CommandText = "SELECT * FROM oasis.oa_ror WHERE 1=1"

            If nomExercice.Trim() <> "" Then
                command.CommandText += " AND oa_ror_nom LIKE '%" & nomExercice & "%'"
            End If

            If villeExercice.Trim() <> "" Then
                command.CommandText += " AND oa_ror_ville LIKE '%" & villeExercice & "%'"
            End If

            If departementExercice.Trim() <> "" Then
                command.CommandText += " AND oa_ror_code_postal LIKE '" & departementExercice & "%'"
            End If

            Using reader As SqlDataReader = command.ExecuteReader()
                While (reader.Read())
                    rors.Add(New Ror(reader))
                End While
            End Using
        Catch ex As Exception
            Throw ex
        Finally
            con.Close()
        End Try

        Return rors
    End Function

    Public Function CreationRor(ror As Ror, userLog As Utilisateur) As Long
        Dim da As New SqlDataAdapter()
        Dim id As Long

        Dim con As SqlConnection = GetConnection()
        Dim dateCreation As Date = Date.Now.Date
        Dim SQLstring As String = "INSERT INTO oasis.oa_ror" &
        " (oa_ror_specialite_id, oa_ror_nom, oa_ror_type, oa_ror_structure_id, oa_ror_structure_nom," &
        " oa_ror_adresse1, oa_ror_adresse2, oa_ror_code_postal, oa_ror_ville," &
        " oa_ror_code, oa_ror_telephone, oa_ror_email, oa_ror_commentaire, oa_ror_rpps, oa_ror_finess, oa_ror_adeli, oa_ror_inactif, oa_ror_user_creation, oa_ror_date_creation," &
        " oa_ror_extraction_annuaire, oa_ror_identifiant_national_pp, oa_ror_identifiant_technique_structure, oa_ror_code_nos_r23_mode_exercice," &
        " oa_ror_code_nos_g15_profession_sante, oa_ror_code_nos_r04_type_savoir_faire, oa_ror_code_savoir_faire, oa_ror_cle_reference)" &
        " VALUES " &
        " (@specialiteId, @nom, @type, @structureId, @structureNom," &
        " @adresse1, @adresse2, @codePostal, @ville," &
        " @code, @telephone, @email, @commentaire, @rpps, @finess, @adeli, @inactif, @userCreation, @dateCreation," &
        " @extractionAnnuaire, @identifiantNational, @identifiantStructure, @codeModeExercice," &
        " @codeProfessionSante, @codeTypeSavoirFaire, @codeSavoirFaire, @cleReference);" &
        " SELECT SCOPE_IDENTITY();"

        Dim cmd As New SqlCommand(SQLstring, con)
        With cmd.Parameters
            .AddWithValue("@specialiteId", ror.SpecialiteId)
            .AddWithValue("@nom", Coalesce(ror.Nom, ""))
            .AddWithValue("@type", Coalesce(ror.Type, ""))
            .AddWithValue("@structureId", Coalesce(ror.StructureId, 0))
            .AddWithValue("@structureNom", Coalesce(ror.StructureNom, ""))
            .AddWithValue("@adresse1", Coalesce(ror.Adresse1, ""))
            .AddWithValue("@adresse2", Coalesce(ror.Adresse2, ""))
            .AddWithValue("@codePostal", Coalesce(ror.CodePostal, ""))
            .AddWithValue("@ville", Coalesce(ror.Ville, ""))
            .AddWithValue("@code", Coalesce(ror.Code, ""))
            .AddWithValue("@telephone", Coalesce(ror.Telephone, ""))
            .AddWithValue("@email", Coalesce(ror.Email, ""))
            .AddWithValue("@commentaire", Coalesce(ror.Commentaire, ""))
            .AddWithValue("@rpps", Coalesce(ror.Rpps, 0))
            .AddWithValue("@finess", Coalesce(ror.Finess, 0))
            .AddWithValue("@adeli", Coalesce(ror.Adeli, 0))
            .AddWithValue("@inactif", ror.Inactif)
            .AddWithValue("@userCreation", userLog.UtilisateurId)
            .AddWithValue("@dateCreation", Date.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@extractionAnnuaire", Coalesce(ror.ExtractionAnnuaire, False))
            .AddWithValue("@identifiantNational", Coalesce(ror.IdentifiantNational, ""))
            .AddWithValue("@identifiantStructure", Coalesce(ror.IdentifiantStructure, ""))
            .AddWithValue("@codeModeExercice", Coalesce(ror.CodeModeExercice_r23, ""))
            .AddWithValue("@codeProfessionSante", Coalesce(ror.CodeProfessionSante_g15, 0))
            .AddWithValue("@codeTypeSavoirFaire", Coalesce(ror.CodeTypeSavoirFaire_r04, ""))
            .AddWithValue("@codeSavoirFaire", Coalesce(ror.CodeSavoirFaire, ""))
            .AddWithValue("@cleReference", Coalesce(ror.CleReferenceAnnuaire, 0))
        End With

        Try
            da.InsertCommand = cmd
            id = da.InsertCommand.ExecuteScalar()
        Catch ex As Exception
            Throw New Exception(ex.Message)
        Finally
            con.Close()
        End Try

        Return id
    End Function

    Public Function ModificationRor(ror As Ror, userLog As Utilisateur) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()
        Dim dateModification As Date = Date.Now.Date
        Dim SQLstring As String = "UPDATE oasis.oa_ror SET" &
        " oa_ror_specialite_id = @specialiteId," &
        " oa_ror_nom = @nom," &
        " oa_ror_type = @type," &
        " oa_ror_structure_id = @structureId," &
        " oa_ror_structure_nom = @structureNom," &
        " oa_ror_adresse1 = @adresse1," &
        " oa_ror_adresse2 = @adresse2," &
        " oa_ror_code_postal = @codePostal," &
        " oa_ror_ville = @ville," &
        " oa_ror_code = @code," &
        " oa_ror_telephone = @telephone," &
        " oa_ror_email = @email," &
        " oa_ror_commentaire = @commentaire," &
        " oa_ror_rpps = @rpps," &
        " oa_ror_finess = @finess," &
        " oa_ror_adeli = @adeli," &
        " oa_ror_inactif = @inactif," &
        " oa_ror_user_modification = @userModification," &
        " oa_ror_date_modification = @dateModification," &
        " oa_ror_extraction_annuaire = @extractionAnnuaire," &
        " oa_ror_identifiant_national_pp = @identifiantNational," &
        " oa_ror_identifiant_technique_structure = @identifiantStructure," &
        " oa_ror_code_nos_r23_mode_exercice = @codeModeExercice," &
        " oa_ror_code_nos_g15_profession_sante = @codeProfessionSante," &
        " oa_ror_code_nos_r04_type_savoir_faire = @codeTypeSavoirFaire," &
        " oa_ror_code_savoir_faire = @codeSavoirFaire," &
        " oa_ror_cle_reference = @cleReference" &
        " WHERE oa_ror_id = @Id"

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
            .AddWithValue("@extractionAnnuaire", ror.ExtractionAnnuaire)
            .AddWithValue("@identifiantNational", ror.IdentifiantNational)
            .AddWithValue("@identifiantStructure", ror.IdentifiantStructure)
            .AddWithValue("@codeModeExercice", ror.CodeModeExercice_r23)
            .AddWithValue("@codeProfessionSante", ror.CodeProfessionSante_g15)
            .AddWithValue("@codeTypeSavoirFaire", ror.CodeTypeSavoirFaire_r04)
            .AddWithValue("@codeSavoirFaire", ror.CodeSavoirFaire)
            .AddWithValue("@cleReference", ror.CleReferenceAnnuaire)
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

    Public Function ExistProfessionnelSante(IdNational As String, IdStructure As String, ModeExercice As String, professionId As Integer, TypeSavoirFaire As String, CodeSavoirFaire As String) As DataTable
        Dim SQLString As String
        SQLString =
            "SELECT oa_ror_id" &
            " FROM oasis.oa_ror" &
            " WHERE (oa_ror_inactif = 'False' OR oa_ror_inactif is Null)" &
            " AND oa_ror_extraction_annuaire = 'True'" &
            " AND oa_ror_identifiant_national_pp = '" & IdNational.Trim() & "'" &
            " AND oa_ror_identifiant_technique_structure = '" & IdStructure.Trim() & "'" &
            " AND oa_ror_code_nos_r23_mode_exercice = '" & ModeExercice.Trim() & "'" &
            " AND oa_ror_code_nos_g15_profession_sante = " & professionId.ToString &
            " AND oa_ror_code_nos_r04_type_savoir_faire = '" & TypeSavoirFaire.Trim & "'" &
            " AND oa_ror_code_savoir_faire = '" & CodeSavoirFaire.ToString & "'"

        Dim dt As DataTable = New DataTable()

        Using con As SqlConnection = GetConnection()
            Dim ParcoursDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using ParcoursDataAdapter
                ParcoursDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                'Using ParcoursDataTable
                Try
                    ParcoursDataAdapter.Fill(dt)
                    Dim command As SqlCommand = con.CreateCommand()
                Catch ex As Exception
                    Throw ex
                End Try
            End Using
        End Using

        Return dt
    End Function
End Class
