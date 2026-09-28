Imports Oasis_Common

''' <summary>
''' DRC de test. Aucun DAO n'écrit dans oa_drc ni dans oa_drc_synonyme : l'écran
''' RadFDrcDetailEdit porte lui-même ses INSERT et UPDATE. Ce module les reproduit
''' en SQL brut, avec les mêmes colonnes. Les tables que des DAO alimentent
''' (oa_drc_standard, oa_drc_acte_paramedical) passent par leur DAO dans les tests.
''' </summary>
Public Module JeuxDrc

    ''' <summary>Catégorie majeure active créée par 24-reference-drc.sql.</summary>
    Public Const CategorieMajeureDrcDeTest As Integer = 9401

    ''' <summary>Seconde catégorie majeure active de 24-reference-drc.sql, pour les filtres.</summary>
    Public Const CategorieMajeureDrcAutre As Integer = 9402

    ''' <summary>
    ''' Crée une DRC comme l'écran de création (RadFDrcDetailEdit.CreationDRC) : l'id
    ''' est fourni par l'application, pas par la base. Par défaut : catégorie Oasis
    ''' Contexte, catégorie majeure de test, les deux sexes, de 0 à 120 ans, sans ALD,
    ''' DRC Oasis. Renvoie son id.
    ''' </summary>
    Function CreerDrc(Optional libelle As String = Nothing,
                      Optional categorieOasis As Integer = Drc.EnumCategorieOasisCode.Contexte,
                      Optional categorieMajeureId As Integer = CategorieMajeureDrcDeTest,
                      Optional sexe As Integer = Drc.EnumGenreItem.HommeEtFemme,
                      Optional aldId As Integer = 0,
                      Optional drcOasis As Boolean = True,
                      Optional utilisateurId As Long = 0) As Long
        Dim id = CLng(Scalaire("SELECT ISNULL(MAX(oa_drc_id), 0) + 1 FROM oasis.oa_drc"))
        Dim texte = If(libelle, "DRC TEST " & id)
        Dim insertion =
            "INSERT INTO oasis.oa_drc" &
            " (oa_drc_id, oa_drc_libelle, oa_drc_dur_prob_epis, oa_drc_url, oa_drc_typ_epi, oa_drc_utilisateur_creation," &
            " oa_drc_date_creation, oa_drc_oasis_categorie, oa_drc_categorie_majeure_id," &
            " oa_drc_sexe, oa_drc_age_min, oa_drc_age_max, oa_drc_oasis," &
            " oa_drc_code_cim_defaut, oa_drc_code_cisp_defaut, oa_drc_ald_id, oa_drc_ald_code)" &
            " VALUES (@p0, @p1, @p2, @p3, @p4, @p5, @p6, @p7, @p8, @p9, @p10, @p11, @p12, @p13, @p14, @p15, @p16)"
        Executer(AvecIdentiteExplicite("oasis.oa_drc", "oa_drc_id", insertion),
                 id, texte, "Commentaire " & id, "https://wiki.exemple.fr/drc/" & id, "C", utilisateurId,
                 Date.Now.ToString("yyyy-MM-dd HH:mm:ss"), categorieOasis, categorieMajeureId,
                 sexe, 0, 120, If(drcOasis, 1, 0),
                 "Z00", "A97", aldId, If(aldId = 0, "", "ALD" & aldId))
        Return id
    End Function

    ''' <summary>
    ''' Ajoute un synonyme comme RadFDrcDetailEdit (l'écran écrit l'INSERT lui-même).
    ''' Renvoie l'id du synonyme.
    ''' </summary>
    Function AjouterSynonymeDrc(drcId As Long, libelle As String) As Long
        Return CLng(Scalaire(
            "INSERT INTO oasis.oa_drc_synonyme (oa_drc_id, oa_drc_synonyme_libelle) VALUES (@p0, @p1);" &
            " SELECT CAST(SCOPE_IDENTITY() AS bigint)", drcId, libelle))
    End Function

    ''' <summary>Invalide une DRC comme le bouton de suppression de RadFDrcDetailEdit.</summary>
    Sub InvaliderDrc(drcId As Long)
        Executer("UPDATE oasis.oa_drc SET oa_drc_date_modification = @p0, oa_drc_utilisateur_modification = @p1," &
                 " oa_drc_oasis_invalide = 1 WHERE oa_drc_id = @p2",
                 Date.Now.ToString("yyyy-MM-dd HH:mm:ss"), 0, drcId)
    End Sub

    ''' <summary>Passe à NULL toutes les colonnes facultatives d'une DRC, pour éprouver les valeurs par défaut du DAO.</summary>
    Sub ViderColonnesFacultativesDrc(drcId As Long)
        Executer("UPDATE oasis.oa_drc SET oa_drc_libelle = NULL, oa_drc_sexe = NULL, oa_drc_typ_epi = NULL," &
                 " oa_drc_age_min = NULL, oa_drc_age_max = NULL, oa_drc_categorie_majeure_id = NULL," &
                 " oa_drc_oasis_categorie = NULL, oa_drc_code_cim_defaut = NULL, oa_drc_code_cisp_defaut = NULL," &
                 " oa_drc_ald_id = NULL, oa_drc_ald_code = NULL, oa_drc_dur_prob_epis = NULL, oa_drc_url = NULL," &
                 " oa_drc_date_creation = NULL, oa_drc_utilisateur_creation = NULL," &
                 " oa_drc_date_modification = NULL, oa_drc_utilisateur_modification = NULL" &
                 " WHERE oa_drc_id = @p0", drcId)
    End Sub

    ''' <summary>
    ''' L'application fournit elle-même l'id de oa_drc. Si l'export du schéma déclare
    ''' pourtant la colonne IDENTITY, l'insertion est encadrée d'IDENTITY_INSERT.
    ''' </summary>
    Private Function AvecIdentiteExplicite(table As String, colonne As String, insertion As String) As String
        Dim identite = Scalaire("SELECT COLUMNPROPERTY(OBJECT_ID(@p0), @p1, 'IsIdentity')", table, colonne)
        If identite Is Nothing OrElse IsDBNull(identite) OrElse CInt(identite) = 0 Then Return insertion
        Return "SET IDENTITY_INSERT " & table & " ON; " & insertion & "; SET IDENTITY_INSERT " & table & " OFF;"
    End Function

End Module
