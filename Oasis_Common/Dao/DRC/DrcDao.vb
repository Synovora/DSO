Imports System.Data.SqlClient
Imports Oasis_Common
Public Class DrcDao
    Inherits StandardDao

    Public Function GetDrc(instanceDrc As Drc, DrcId As Integer) As Boolean
        Dim CodeRetour As Boolean = True
        Dim con As SqlConnection = GetConnection()
        Dim SQLString As String = "SELECT * FROM oasis.oa_drc WHERE oa_drc_id = @drcId"
        Dim drcDataReader As SqlDataReader
        Dim cmd As New SqlCommand(SQLString, con)

        cmd.Parameters.AddWithValue("@drcId", DrcId.ToString)

        Try
            drcDataReader = cmd.ExecuteReader()
            BuildInstance(instanceDrc, drcDataReader)
        Catch ex As Exception
            Throw New Exception(ex.Message)
            Return CodeRetour = False
        Finally
            con.Close()
            cmd.Dispose()
        End Try

        Return CodeRetour
    End Function

    Private Sub BuildInstance(instanceDrc As Drc, drcDataReader As SqlDataReader)
        If drcDataReader.Read() Then
            instanceDrc.DrcId = Convert.ToInt64(drcDataReader("oa_drc_id"))
            instanceDrc.DrcLibelle = Coalesce(drcDataReader("oa_drc_libelle"), "")
            instanceDrc.DrcSexe = Coalesce(drcDataReader("oa_drc_sexe"), 0)
            instanceDrc.DrcTypeEpisode = Coalesce(drcDataReader("oa_drc_typ_epi"), "")
            instanceDrc.DrcAgeMin = Coalesce(drcDataReader("oa_drc_age_min"), 0)
            instanceDrc.DrcAgeMax = Coalesce(drcDataReader("oa_drc_age_max"), 0)
            instanceDrc.CategorieMajeure = Coalesce(drcDataReader("oa_drc_categorie_majeure_id"), 0)
            instanceDrc.CategorieOasisId = Coalesce(drcDataReader("oa_drc_oasis_categorie"), 0)
            instanceDrc.CodeCim = Coalesce(drcDataReader("oa_drc_code_cim_defaut"), "")
            instanceDrc.CodeCisp = Coalesce(drcDataReader("oa_drc_code_cisp_defaut"), "")
            instanceDrc.AldId = Coalesce(drcDataReader("oa_drc_ald_id"), 0)
            instanceDrc.AldCode = Coalesce(drcDataReader("oa_drc_ald_code"), "")
            instanceDrc.Commentaire = Coalesce(drcDataReader("oa_drc_dur_prob_epis"), "")
            instanceDrc.ReponseCommentee = Coalesce(drcDataReader("oa_drc_typ_epi"), "")
            instanceDrc.DateCreation = Coalesce(drcDataReader("oa_drc_date_creation"), Nothing)
            instanceDrc.UserCreation = Coalesce(drcDataReader("oa_drc_utilisateur_creation"), 0)
            instanceDrc.DateModification = Coalesce(drcDataReader("oa_drc_date_modification"), Nothing)
            instanceDrc.UserModification = Coalesce(drcDataReader("oa_drc_utilisateur_modification"), 0)
        End If
    End Sub

    Public Function GetDrcById(DrcId As Long) As Drc
        Dim drc As Drc
        Using con As SqlConnection = GetConnection()

            Try
                Dim command As SqlCommand = con.CreateCommand()

                command.CommandText =
                    "SELECT * FROM oasis.oa_drc WHERE oa_drc_id = @id"
                command.Parameters.AddWithValue("@id", DrcId)
                Using reader As SqlDataReader = command.ExecuteReader()
                    If reader.Read() Then
                        drc = BuildBean(reader)
                    Else
                        Throw New ArgumentException("DRC inexistante !")
                    End If
                End Using

            Catch ex As Exception
                Throw ex
            Finally
                con.Close()
            End Try
        End Using

        Return drc
    End Function

    Private Function BuildBean(reader As SqlDataReader) As Drc
        Dim drc As New Drc With {
            .DrcId = Convert.ToInt64(reader("oa_drc_id")),
            .DrcLibelle = Coalesce(reader("oa_drc_libelle"), ""),
            .DrcSexe = Coalesce(reader("oa_drc_sexe"), 0),
            .DrcTypeEpisode = Coalesce(reader("oa_drc_typ_epi"), ""),
            .DrcAgeMin = Coalesce(reader("oa_drc_age_min"), 0),
            .DrcAgeMax = Coalesce(reader("oa_drc_age_max"), 0),
            .CategorieMajeure = Coalesce(reader("oa_drc_categorie_majeure_id"), 0),
            .CategorieOasisId = Coalesce(reader("oa_drc_oasis_categorie"), 0),
            .CodeCim = Coalesce(reader("oa_drc_code_cim_defaut"), ""),
            .CodeCisp = Coalesce(reader("oa_drc_code_cisp_defaut"), ""),
            .AldId = Coalesce(reader("oa_drc_ald_id"), 0),
            .AldCode = Coalesce(reader("oa_drc_ald_code"), ""),
            .Commentaire = Coalesce(reader("oa_drc_dur_prob_epis"), ""),
            .ReponseCommentee = Coalesce(reader("oa_drc_typ_epi"), ""),
            .DateCreation = Coalesce(reader("oa_drc_date_creation"), Nothing),
            .UserCreation = Coalesce(reader("oa_drc_utilisateur_creation"), 0),
            .DateModification = Coalesce(reader("oa_drc_date_modification"), Nothing),
            .UserModification = Coalesce(reader("oa_drc_utilisateur_modification"), 0)
        }
        Return drc
    End Function


    Public Function GetItemGenreByCode(Code As Integer) As String
        Dim Item As String
        Select Case Code
            Case Drc.EnumGenreItem.Homme
                Item = Drc.EnumGenre.Homme
            Case Drc.EnumGenreItem.Femme
                Item = Drc.EnumGenre.Femme
            Case Drc.EnumGenreItem.HommeEtFemme
                Item = Drc.EnumGenre.HommeEtFemme
            Case Else
                Item = Drc.EnumGenre.HommeEtFemme
        End Select

        Return Item
    End Function

    Public Function GetCodeGenreByItem(Item As String) As Integer
        Dim Code As Integer
        Select Case Item
            Case Drc.EnumGenre.Homme
                Code = Drc.EnumGenreItem.Homme
            Case Drc.EnumGenre.Femme
                Code = Drc.EnumGenreItem.Femme
            Case Drc.EnumGenre.HommeEtFemme
                Code = Drc.EnumGenreItem.HommeEtFemme
            Case Else
                Code = Drc.EnumGenreItem.HommeEtFemme
        End Select

        Return Code
    End Function

    Public Function GetItemCategorieOasisByCode(Code As Integer) As String
        Dim Item As String
        Select Case Code
            Case Drc.EnumCategorieOasisCode.Contexte
                Item = Drc.EnumCategorieOasisItem.Contexte
            Case Drc.EnumCategorieOasisCode.Objectif
                Item = Drc.EnumCategorieOasisItem.Objectif
            Case Drc.EnumCategorieOasisCode.Prevention
                Item = Drc.EnumCategorieOasisItem.Prevention
            Case Drc.EnumCategorieOasisCode.Strategie
                Item = Drc.EnumCategorieOasisItem.Strategie
            Case Drc.EnumCategorieOasisCode.ActeParamedical
                Item = Drc.EnumCategorieOasisItem.ActeParamedical
            Case Drc.EnumCategorieOasisCode.GroupeParametres
                Item = Drc.EnumCategorieOasisItem.GroupeParametres
            Case Drc.EnumCategorieOasisCode.ProtocoleCollaboratif
                Item = Drc.EnumCategorieOasisItem.ProtocoleCollaboratif
            Case Drc.EnumCategorieOasisCode.ProtocoleAigu
                Item = Drc.EnumCategorieOasisItem.ProtocoleAigu
            Case Else
                Item = "Inconnue"
        End Select

        Return Item
    End Function

    Public Function GetCodeCategorieOasisByItem(Item As String) As Integer
        Dim Code As Integer
        Select Case Item
            Case Drc.EnumCategorieOasisItem.Contexte
                Code = Drc.EnumCategorieOasisCode.Contexte
            Case Drc.EnumCategorieOasisItem.Objectif
                Code = Drc.EnumCategorieOasisCode.Objectif
            Case Drc.EnumCategorieOasisItem.Prevention
                Code = Drc.EnumCategorieOasisCode.Prevention
            Case Drc.EnumCategorieOasisItem.Strategie
                Code = Drc.EnumCategorieOasisCode.Strategie
            Case Drc.EnumCategorieOasisItem.ActeParamedical
                Code = Drc.EnumCategorieOasisCode.ActeParamedical
            Case Drc.EnumCategorieOasisItem.GroupeParametres
                Code = Drc.EnumCategorieOasisCode.GroupeParametres
            Case Drc.EnumCategorieOasisItem.ProtocoleCollaboratif
                Code = Drc.EnumCategorieOasisCode.ProtocoleCollaboratif
            Case Drc.EnumCategorieOasisItem.ProtocoleAigu
                Code = Drc.EnumCategorieOasisCode.ProtocoleAigu
            Case Else
                Code = 0
        End Select

        Return Code
    End Function

    Public Function GetCategorieOasisByCategoriePPS(CategoriePPS As Integer) As Integer
        Dim CategorieOasis As Integer
        Select Case CategoriePPS
            Case EnumCategoriePPS.Objectif
                CategorieOasis = Drc.EnumCategorieOasisCode.Objectif
            Case EnumCategoriePPS.MesurePreventive
                CategorieOasis = Drc.EnumCategorieOasisCode.Prevention
            Case EnumCategoriePPS.Strategie
                CategorieOasis = Drc.EnumCategorieOasisCode.Strategie
        End Select

        Return CategorieOasis
    End Function

    Public Function GetAllDrcByCategorieAndGenre(selectDrc As String, categorieMajeureId As Long, categorieOasis As Long, selectAld As Boolean, genre As String) As DataTable
        Dim SQLString As String
        Dim clauseDrc As String
        Dim clauseCategorieMajeure As String
        Dim clauseCategorieOasis As String
        Dim clauseALD As String

        If selectDrc = "" Then
            clauseDrc = "1 = 1"
        Else
            clauseDrc = "(oa_drc_libelle COLLATE Latin1_general_CI_AI LIKE '%" & selectDrc &
                "%' COLLATE Latin1_general_CI_AI OR syn_libs COLLATE Latin1_general_CI_AI like '%" & selectDrc &
                "%' COLLATE Latin1_general_CI_AI)"
        End If

        If categorieMajeureId = 0 Then
            clauseCategorieMajeure = "1 = 1"
        Else
            clauseCategorieMajeure = "oa_drc_categorie_majeure_id = " & categorieMajeureId & " "
        End If

        If categorieOasis = 0 Then
            clauseCategorieOasis = "1 = 1 "
        Else
            clauseCategorieOasis = "oa_drc_oasis_categorie = " & categorieOasis & " "
        End If

        If selectAld = True Then
            clauseALD = "oa_drc_ald_id <> 0"
        Else
            clauseALD = "1 = 1"
        End If

        SQLString = "SELECT oa_drc_id, oa_drc_libelle, oa_drc_categorie_majeure_id, oa_drc_oasis, oa_drc_sexe, oa_drc_age_min,oa_drc_age_max, " &
                    " oa_drc_ald_id, oa_drc_ald_code, oa_drc_oasis_categorie, oa_r_categorie_majeure_description, oa_ald_description, oa_drc_dur_prob_epis, oa_drc_typ_epi," &
                    " syn_libs" &
                    " FROM oasis.v_drc" &
                    " LEFT JOIN oasis.oa_ald A ON oa_drc_ald_id = A.oa_ald_id" &
                    " WHERE " & clauseCategorieMajeure & " AND " & clauseDrc & " AND " & clauseCategorieOasis & " AND " & clauseALD

        Select Case genre
            Case "M"
                SQLString += " AND (oa_drc_sexe = 1 or oa_drc_sexe = 3)"
            Case "F"
                SQLString += " AND (oa_drc_sexe = 2 Or oa_drc_sexe = 3)"
        End Select

        SQLString += " ORDER BY oa_drc_oasis DESC, oa_drc_id;"

        Using con As SqlConnection = GetConnection()

            Dim drcDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using drcDataAdapter
                drcDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim drcDataTable As DataTable = New DataTable()
                Using drcDataTable
                    Try
                        drcDataAdapter.Fill(drcDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return drcDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function GetAllDrcByCategorie(selectDrc As String, categorieMajeureId As Long, categorieOasis As Long, selectAld As Boolean, genre As String) As DataTable
        Dim SQLString As String
        Dim clauseDrc As String
        Dim clauseCategorieMajeure As String
        Dim clauseCategorieOasis As String
        Dim clauseALD As String

        If selectDrc = "" Then
            clauseDrc = "1 = 1"
        Else
            clauseDrc = "(oa_drc_libelle COLLATE Latin1_general_CI_AI LIKE '%" & selectDrc &
                "%' COLLATE Latin1_general_CI_AI or oa_drc_synonyme_libelle COLLATE Latin1_general_CI_AI like '%" & selectDrc &
                "%' COLLATE Latin1_general_CI_AI)"
        End If

        If categorieMajeureId = 0 Then
            clauseCategorieMajeure = "1 = 1"
        Else
            clauseCategorieMajeure = "oa_drc_categorie_majeure_id = " & categorieMajeureId & " "
        End If

        If categorieOasis = 0 Then
            clauseCategorieOasis = "1 = 1 "
        Else
            clauseCategorieOasis = "oa_drc_oasis_categorie = " & categorieOasis & " "
        End If

        If selectAld = True Then
            clauseALD = "oa_drc_ald_id <> 0"
        Else
            clauseALD = "1 = 1"
        End If

        SQLString = "SELECT oasis.oa_drc.oa_drc_id, oa_drc_libelle, oa_drc_categorie_majeure_id, oa_drc_oasis, oa_drc_sexe, oa_drc_age_min,oa_drc_age_max, " &
                    " oa_drc_ald_id, oa_drc_ald_code, oa_drc_oasis_categorie, oa_r_categorie_majeure_description, oa_ald_description, oa_drc_dur_prob_epis, oa_drc_typ_epi" &
                    " FROM oasis.oa_drc" &
                    " LEFT JOIN oasis.oa_ald ON oasis.oasis.oa_drc.oa_drc_ald_id = oasis.oa_ald.oa_ald_id" &
                    " LEFT JOIN oasis.oa_drc_synonyme ON oasis.oasis.oa_drc.oa_drc_id = oasis.oa_drc_synonyme.oa_drc_id" &
                    " LEFT JOIN oasis.oa_r_categorie_majeure ON oasis.oasis.oa_drc.oa_drc_categorie_majeure_id = oasis.oa_r_categorie_majeure.oa_r_categorie_majeure_id" &
                    " WHERE " & clauseCategorieMajeure & " And " & clauseDrc & " And " & clauseCategorieOasis & " And " & clauseALD &
                    " AND (oa_drc_oasis_invalide Is Null Or oa_drc_oasis_invalide = 'False')"

        Select Case genre
            Case "M"
                SQLString += " AND (oa_drc_sexe = 1 or oa_drc_sexe = 3)"
            Case "F"
                SQLString += " AND (oa_drc_sexe = 2 Or oa_drc_sexe = 3)"
        End Select

        SQLString += " ORDER BY oa_drc_oasis DESC, oasis.oasis.oa_drc.oa_drc_id;"

        Using con As SqlConnection = GetConnection()

            Dim drcDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Using drcDataAdapter
                drcDataAdapter.SelectCommand = New SqlCommand(SQLString, con)
                Dim drcDataTable As DataTable = New DataTable()
                Using drcDataTable
                    Try
                        drcDataAdapter.Fill(drcDataTable)
                    Catch ex As Exception
                        Throw ex
                    End Try
                    Return drcDataTable
                End Using
            End Using
        End Using
    End Function

    Public Function GetLastDrc(categorieOasisId As Long) As Long
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim LastDrcId As Integer = 0
        Dim SQLstring As String
        Dim con As SqlConnection
        con = GetConnection()

        'Récupération de l'identifiant de la dernière DRC créée
        Dim EpisodeLastDataReader As SqlDataReader
        SQLstring = "SELECT MAX(oa_drc_id) FROM oasis.oasis.oa_drc WHERE oa_drc_oasis_categorie = " & categorieOasisId
        Dim EpisodeLastCommand As New SqlCommand(SQLstring, con)
        EpisodeLastDataReader = EpisodeLastCommand.ExecuteReader()
        If EpisodeLastDataReader.HasRows Then
            EpisodeLastDataReader.Read()
            'Récupération de la clé de l'enregistrement créé
            LastDrcId = EpisodeLastDataReader(0)
            'Libération des ressources d'accès aux données
            con.Close()
            EpisodeLastCommand.Dispose()
        End If

        Return LastDrcId
    End Function

End Class
