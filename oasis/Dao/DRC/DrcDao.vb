Imports System.Data.SqlClient
Imports Oasis_Common
Public Class DrcDao
    Inherits StandardDao

    Public Enum EnumGenreItem
        Homme = 1
        Femme = 2
        HommeEtFemme = 3
    End Enum

    Public Structure EnumGenre
        Const Homme = "Homme"
        Const Femme = "Femme"
        Const HommeEtFemme = "Homme et femme"
    End Structure

    Public Enum EnumCategorieOasisCode
        Contexte = 1
        Strategie = 2
        Prevention = 3
        Objectif = 4
        ActeParamedical = 5
        GroupeParametres = 6
        ProtocoleCollaboratif = 7
        ProtocoleAigu = 8
    End Enum

    Public Structure EnumCategorieOasisItem
        Const Contexte = "Contexte et antécédent"
        Const Strategie = "Stratégie"
        Const Prevention = "Prévention"
        Const Objectif = "Objectif"
        Const ActeParamedical = "Acte paramédical"
        Const GroupeParametres = "Groupe de paramètres"
        Const ProtocoleCollaboratif = "Procédure collaborative"
        Const ProtocoleAigu = "Procédure pathologie aigüe"
    End Structure

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
            MessageBox.Show(ex.Message)
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

    Friend Function GetDrcById(DrcId As Long) As Drc
        Dim drc As Drc
        Dim con As SqlConnection
        con = GetConnection()

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

        Return drc
    End Function

    Private Function BuildBean(reader As SqlDataReader) As Drc
        Dim drc As New Drc
        drc.DrcId = Convert.ToInt64(reader("oa_drc_id"))
        drc.DrcLibelle = Coalesce(reader("oa_drc_libelle"), "")
        drc.DrcSexe = Coalesce(reader("oa_drc_sexe"), 0)
        drc.DrcTypeEpisode = Coalesce(reader("oa_drc_typ_epi"), "")
        drc.DrcAgeMin = Coalesce(reader("oa_drc_age_min"), 0)
        drc.DrcAgeMax = Coalesce(reader("oa_drc_age_max"), 0)
        drc.CategorieMajeure = Coalesce(reader("oa_drc_categorie_majeure_id"), 0)
        drc.CategorieOasisId = Coalesce(reader("oa_drc_oasis_categorie"), 0)
        drc.CodeCim = Coalesce(reader("oa_drc_code_cim_defaut"), "")
        drc.CodeCisp = Coalesce(reader("oa_drc_code_cisp_defaut"), "")
        drc.AldId = Coalesce(reader("oa_drc_ald_id"), 0)
        drc.AldCode = Coalesce(reader("oa_drc_ald_code"), "")
        drc.Commentaire = Coalesce(reader("oa_drc_dur_prob_epis"), "")
        drc.ReponseCommentee = Coalesce(reader("oa_drc_typ_epi"), "")
        drc.DateCreation = Coalesce(reader("oa_drc_date_creation"), Nothing)
        drc.UserCreation = Coalesce(reader("oa_drc_utilisateur_creation"), 0)
        drc.DateModification = Coalesce(reader("oa_drc_date_modification"), Nothing)
        drc.UserModification = Coalesce(reader("oa_drc_utilisateur_modification"), 0)
        Return drc
    End Function


    Friend Function GetItemGenreByCode(Code As Integer) As String
        Dim Item As String
        Select Case Code
            Case DrcDao.EnumGenreItem.Homme
                Item = DrcDao.EnumGenre.Homme
            Case DrcDao.EnumGenreItem.Femme
                Item = DrcDao.EnumGenre.Femme
            Case DrcDao.EnumGenreItem.HommeEtFemme
                Item = DrcDao.EnumGenre.HommeEtFemme
            Case Else
                Item = DrcDao.EnumGenre.HommeEtFemme
        End Select

        Return Item
    End Function

    Friend Function GetCodeGenreByItem(Item As String) As Integer
        Dim Code As Integer
        Select Case Item
            Case DrcDao.EnumGenre.Homme
                Code = DrcDao.EnumGenreItem.Homme
            Case DrcDao.EnumGenre.Femme
                Code = DrcDao.EnumGenreItem.Femme
            Case DrcDao.EnumGenre.HommeEtFemme
                Code = DrcDao.EnumGenreItem.HommeEtFemme
            Case Else
                Code = DrcDao.EnumGenreItem.HommeEtFemme
        End Select

        Return Code
    End Function

    Friend Function GetItemCategorieOasisByCode(Code As Integer) As String
        Dim Item As String
        Select Case Code
            Case DrcDao.EnumCategorieOasisCode.Contexte
                Item = DrcDao.EnumCategorieOasisItem.Contexte
            Case DrcDao.EnumCategorieOasisCode.Objectif
                Item = DrcDao.EnumCategorieOasisItem.Objectif
            Case DrcDao.EnumCategorieOasisCode.Prevention
                Item = DrcDao.EnumCategorieOasisItem.Prevention
            Case DrcDao.EnumCategorieOasisCode.Strategie
                Item = DrcDao.EnumCategorieOasisItem.Strategie
            Case DrcDao.EnumCategorieOasisCode.ActeParamedical
                Item = DrcDao.EnumCategorieOasisItem.ActeParamedical
            Case DrcDao.EnumCategorieOasisCode.GroupeParametres
                Item = DrcDao.EnumCategorieOasisItem.GroupeParametres
            Case DrcDao.EnumCategorieOasisCode.ProtocoleCollaboratif
                Item = DrcDao.EnumCategorieOasisItem.ProtocoleCollaboratif
            Case DrcDao.EnumCategorieOasisCode.ProtocoleAigu
                Item = DrcDao.EnumCategorieOasisItem.ProtocoleAigu
            Case Else
                Item = "Inconnue"
        End Select

        Return Item
    End Function

    Friend Function GetCodeCategorieOasisByItem(Item As String) As Integer
        Dim Code As Integer
        Select Case Item
            Case DrcDao.EnumCategorieOasisItem.Contexte
                Code = DrcDao.EnumCategorieOasisCode.Contexte
            Case DrcDao.EnumCategorieOasisItem.Objectif
                Code = DrcDao.EnumCategorieOasisCode.Objectif
            Case DrcDao.EnumCategorieOasisItem.Prevention
                Code = DrcDao.EnumCategorieOasisCode.Prevention
            Case DrcDao.EnumCategorieOasisItem.Strategie
                Code = DrcDao.EnumCategorieOasisCode.Strategie
            Case DrcDao.EnumCategorieOasisItem.ActeParamedical
                Code = DrcDao.EnumCategorieOasisCode.ActeParamedical
            Case DrcDao.EnumCategorieOasisItem.GroupeParametres
                Code = DrcDao.EnumCategorieOasisCode.GroupeParametres
            Case DrcDao.EnumCategorieOasisItem.ProtocoleCollaboratif
                Code = DrcDao.EnumCategorieOasisCode.ProtocoleCollaboratif
            Case DrcDao.EnumCategorieOasisItem.ProtocoleAigu
                Code = DrcDao.EnumCategorieOasisCode.ProtocoleAigu
            Case Else
                Code = 0
        End Select

        Return Code
    End Function

    Friend Function GetCategorieOasisByCategoriePPS(CategoriePPS As Integer) As Integer
        Dim CategorieOasis As Integer
        Select Case CategoriePPS
            Case EnumCategoriePPS.Objectif
                CategorieOasis = DrcDao.EnumCategorieOasisCode.Objectif
            Case EnumCategoriePPS.MesurePreventive
                CategorieOasis = DrcDao.EnumCategorieOasisCode.Prevention
            Case EnumCategoriePPS.Strategie
                CategorieOasis = DrcDao.EnumCategorieOasisCode.Strategie
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

    Friend Function GetLastDrc(categorieOasisId As Long) As Long
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

    Public Function ModificationDRC(drc As Drc) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "UPDATE oasis.oa_drc SET" &
        " oa_drc_date_modification = @dateModification," &
        " oa_drc_utilisateur_modification = @utilisateurModification," &
        " oa_drc_libelle = @description," &
        " oa_drc_dur_prob_epis = @commentaire," &
        " oa_drc_typ_epi = @reponseCommentee," &
        " oa_drc_oasis_categorie = @categorieOasis," &
        " oa_drc_categorie_majeure_id = @categorieMajeureId," &
        " oa_drc_sexe = @sexe," &
        " oa_drc_age_min = @ageMin," &
        " oa_drc_age_max = @ageMax," &
        " oa_drc_code_cim_defaut = @cim10Code," &
        " oa_drc_code_cisp_defaut = @cispCode," &
        " oa_drc_ald_id = @aldId," &
        " oa_drc_ald_code = @aldCode" &
        " WHERE oa_drc_id = @drcId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@description", drc.DrcLibelle)
            .AddWithValue("@commentaire", drc.Commentaire)
            .AddWithValue("@reponseCommentee", drc.DrcTypeEpisode)
            .AddWithValue("@categorieOasis", drc.CategorieOasisId)
            .AddWithValue("@categorieMajeureId", drc.CategorieMajeure)
            .AddWithValue("@sexe", drc.DrcSexe)
            .AddWithValue("@ageMin", drc.DrcAgeMin)
            .AddWithValue("@ageMax", drc.DrcAgeMax)
            .AddWithValue("@cim10Code", drc.CodeCim)
            .AddWithValue("@cispCode", drc.CodeCisp)
            .AddWithValue("@aldId", drc.AldId)
            .AddWithValue("@aldCode", drc.AldCode)
            .AddWithValue("@drcId", drc.DrcId)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            MessageBox.Show("DRC/ORC modifiée")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        End Try

        Return codeRetour
    End Function

    Public Function AnnulationDRC(drc As Drc, TransformationEnCours As Boolean) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "UPDATE oasis.oa_drc" &
            " SET oa_drc_date_modification = @dateModification, oa_drc_utilisateur_modification = @utilisateurModification," &
            " oa_drc_oasis_invalide = @invalide where oa_drc_id = @drcId"

        Dim con As SqlConnection = GetConnection()
        Dim cmd As New SqlCommand(SQLstring, con)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@invalide", 1)
            .AddWithValue("@drcId", drc.DrcId)
        End With

        Try
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            If TransformationEnCours = False Then
                MessageBox.Show("DRC/ORC modifiée")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        End Try

        Return codeRetour
    End Function

End Class
