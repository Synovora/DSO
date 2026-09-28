Imports System.Reflection
Imports Oasis_Common

''' <summary>
''' Singletons de données de référence d'EnvironnementBase : Table_genre,
''' Table_specialite, Table_categorie_majeure et Table_ald. Chacun lit sa table au
''' premier usage et la garde pour tout le processus.
'''
''' Ces tests ne font que lire ce que les scripts de référence ont chargé avant
''' l'instantané (21 genres, 24 catégories majeures, 27 et 28 spécialités,
''' 29 ALD, 31 lignes inactives) ; ils n'écrivent jamais dans ces tables. Le cache
''' est commun à tout le processus et l'ordre des classes n'est pas garanti : aucun
''' test ne suppose être le premier à charger un singleton. Les tests de chargement
''' vident eux-mêmes le cache (champ privé instance, par réflexion) pour relire la
''' table sous oasis_client, le compte du client lourd qui les charge en production ;
''' le cache se reconstruit à l'identique depuis l'instantané.
''' </summary>
<TestClass()> Public Class EnvironnementBaseTest
    Inherits TestIntegration

    ' Lignes de 31-reference-structure.sql.
    Private Const GenreInactif As String = "Z"
    Private Const GenreSansIndicateur As String = "Y"
    Private Const CategorieInactive As Integer = 9403
    Private Const CategorieSansIndicateur As Integer = 9404

    ''' <summary>
    ''' Délai de prise en charge par défaut de Table_specialite quand l'appSetting
    ''' SpecialiteDelaiPriseEnCharge manque, ce qui est le cas dans app.config des tests
    ''' (le client lourd le fixe à 90).
    ''' </summary>
    Private Const DelaiParDefautSansParametre As Integer = 30

    Private Const SpecialiteAbsente As Integer = 9899
    Private Const CategorieAbsente As Integer = 9499
    Private Const AldAbsente As Integer = 9599

    ''' <summary>Vide le cache d'un singleton : le prochain appel relit sa table.</summary>
    Private Shared Sub OublierInstance(classe As Type)
        ChampInstance(classe).SetValue(Nothing, Nothing)
    End Sub

    Private Shared Function InstanceEnCache(classe As Type) As Object
        Return ChampInstance(classe).GetValue(Nothing)
    End Function

    Private Shared Function ChampInstance(classe As Type) As FieldInfo
        Dim champ = classe.GetField("instance", BindingFlags.NonPublic Or BindingFlags.Static)
        Assert.IsNotNull(champ, "champ instance introuvable sur " & classe.Name)
        Return champ
    End Function

    ''' <summary>
    ''' Rend le test Inconclusive si la ligne de référence attendue manque : soit la
    ''' colonne n'accepte pas NULL (ligne volontairement non posée), soit le script
    ''' 31-reference-structure.sql n'a pas été rejoué par Preparer-Base.ps1.
    ''' </summary>
    Private Shared Sub ExigerLigneDeReference(quoi As String, sql As String, ParamArray valeurs() As Object)
        If CInt(Scalaire(sql, valeurs)) = 0 Then
            Assert.Inconclusive(quoi & " absente de la base : 31-reference-structure.sql non rejoué, ou colonne NOT NULL.")
        End If
    End Sub

    Private Shared Function DescriptionGenre(code As String) As String
        Return CStr(Scalaire("SELECT oa_r_genre_description FROM oasis.oa_r_genre WHERE oa_r_genre_code = @p0", code))
    End Function

    Private Shared Function DescriptionCategorie(id As Integer) As String
        Return CStr(Scalaire("SELECT oa_r_categorie_majeure_description FROM oasis.oa_r_categorie_majeure WHERE oa_r_categorie_majeure_id = @p0", id))
    End Function

    Private Shared Function ValeurSpecialite(colonne As String, id As Integer) As Object
        Return Scalaire("SELECT " & colonne & " FROM oasis.oa_r_specialite WHERE oa_r_specialite_id = @p0", id)
    End Function

    ' =========================================================================
    ' Table_genre
    ' =========================================================================

    <TestMethod()> Public Sub Genre_CodesActifs_RenvoientLeurLibelle()
        Assert.AreEqual(DescriptionGenre("F"), Table_genre.GetGenreDescription("F"))
        Assert.AreEqual(DescriptionGenre("M"), Table_genre.GetGenreDescription("M"))
        Assert.AreNotEqual("", Table_genre.GetGenreDescription("F"))
    End Sub

    <TestMethod()> Public Sub Genre_CodeInconnu_RenvoieUneChaineVide()
        Assert.AreEqual("", Table_genre.GetGenreDescription("Q"))
        Assert.AreEqual("", Table_genre.GetGenreDescription(""))
    End Sub

    <TestMethod()> Public Sub Genre_CodeNothing_LeveArgumentNullException()
        ' Comportement actuel : le code sert de clé de dictionnaire sans garde.
        Assert.ThrowsException(Of ArgumentNullException)(Sub() Table_genre.GetGenreDescription(Nothing))
    End Sub

    <TestMethod()> Public Sub Genre_Inactif_EstAbsentDuCache()
        ExigerLigneDeReference("Le genre inactif Z", "SELECT COUNT(*) FROM oasis.oa_r_genre WHERE oa_r_genre_code = @p0", GenreInactif)

        Assert.AreEqual("", Table_genre.GetGenreDescription(GenreInactif))
        Assert.IsFalse(Table_genre.GetGenreListe().ContainsKey(GenreInactif))
    End Sub

    <TestMethod()> Public Sub Genre_IndicateurInactifNul_CompteCommeActif()
        ExigerLigneDeReference("Le genre Y sans indicateur", "SELECT COUNT(*) FROM oasis.oa_r_genre WHERE oa_r_genre_code = @p0", GenreSansIndicateur)

        Assert.AreEqual(DescriptionGenre(GenreSansIndicateur), Table_genre.GetGenreDescription(GenreSansIndicateur))
    End Sub

    <TestMethod()> Public Sub Genre_Liste_ContientExactementLesGenresActifs()
        Dim liste = Table_genre.GetGenreListe()

        Assert.AreEqual(DescriptionGenre("F"), liste("F"))
        Assert.AreEqual(DescriptionGenre("M"), liste("M"))
        Dim actifs = CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_r_genre WHERE oa_r_genre_inactif = 0 OR oa_r_genre_inactif IS NULL"))
        Assert.AreEqual(actifs, liste.Count)
    End Sub

    <TestMethod()> Public Sub Genre_SecondAppel_RenvoieLeMemeCache()
        Dim premiere = Table_genre.GetGenreListe()
        Dim instance = InstanceEnCache(GetType(Table_genre))

        Table_genre.GetGenreDescription("F")

        Assert.AreSame(premiere, Table_genre.GetGenreListe())
        Assert.AreSame(instance, InstanceEnCache(GetType(Table_genre)))
    End Sub

    <TestMethod()> Public Sub Genre_ChargementSousClient_LitLaTable()
        OublierInstance(GetType(Table_genre))
        UtiliserCompte(Compte.Client)

        Assert.AreEqual(DescriptionGenre("F"), Table_genre.GetGenreDescription("F"))
        Assert.IsNotNull(InstanceEnCache(GetType(Table_genre)))
    End Sub

    ' =========================================================================
    ' Table_specialite
    ' =========================================================================

    <TestMethod()> Public Sub Specialite_Active_RenvoieToutesSesValeurs()
        Dim lue = Table_specialite.GetSpecialiteById(SpecialiteParcoursTest)

        Assert.AreEqual(CLng(SpecialiteParcoursTest), lue.SpecialiteId)
        Assert.AreEqual(CStr(ValeurSpecialite("oa_specialite_code", SpecialiteParcoursTest)), lue.Code)
        Assert.AreEqual(CStr(ValeurSpecialite("oa_r_specialite_description", SpecialiteParcoursTest)), lue.Description)
        Assert.AreEqual("MEDICAL", lue.Nature)
        Assert.AreEqual("", lue.Genre)
        Assert.IsTrue(lue.Parcours)
        Assert.IsFalse(lue.Oasis)
        Assert.AreEqual(0, lue.AgeMin)
        Assert.AreEqual(0, lue.AgeMax)
        Assert.AreEqual(CInt(ValeurSpecialite("oa_r_delaiPriseEnCharge", SpecialiteParcoursTest)), lue.DelaiPriseEnCharge)
        Assert.AreNotEqual(0, lue.DelaiPriseEnCharge)
        ' Les codes NOS ne sont pas repris par le singleton.
        Assert.AreEqual(0, lue.NosG15CodeProfession)
        Assert.IsNull(lue.NosCodeSavoirFaire)
    End Sub

    <TestMethod()> Public Sub Specialite_DelaiAZero_PrendLeDelaiParDefaut()
        ' Comportement actuel : app.config des tests n'a pas SpecialiteDelaiPriseEnCharge,
        ' le singleton retombe sur 30 jours (le client lourd livre 90).
        Assert.AreEqual(0, CInt(ValeurSpecialite("oa_r_delaiPriseEnCharge", SpecialiteTacheOasis)))

        Dim lue = Table_specialite.GetSpecialiteById(SpecialiteTacheOasis)

        Assert.AreEqual(DelaiParDefautSansParametre, lue.DelaiPriseEnCharge)
        Assert.IsTrue(lue.Oasis)
    End Sub

    <TestMethod()> Public Sub Specialite_DelaiRenseigne_EstRepris()
        Assert.AreEqual(DelaiSpecialiteTacheNonOasis, Table_specialite.GetSpecialiteById(SpecialiteTacheNonOasis).DelaiPriseEnCharge)
    End Sub

    <TestMethod()> Public Sub Specialite_Description_ParId()
        Assert.AreEqual(CStr(ValeurSpecialite("oa_r_specialite_description", SpecialiteParcoursTest)),
                        Table_specialite.GetSpecialiteDescription(SpecialiteParcoursTest))
    End Sub

    <TestMethod()> Public Sub Specialite_Inactive_EstAbsenteDuCache()
        Assert.IsTrue(CBool(ValeurSpecialite("oa_r_specialite_inactif", SpecialiteParcoursInactive)), "ligne inactive de 28-reference-parcours.sql")

        Assert.AreEqual("", Table_specialite.GetSpecialiteDescription(SpecialiteParcoursInactive))
        Dim lue = Table_specialite.GetSpecialiteById(SpecialiteParcoursInactive)
        Assert.AreEqual(CLng(SpecialiteParcoursInactive), lue.SpecialiteId, "l'id demandé est renvoyé tel quel")
        Assert.AreEqual("", lue.Code)
        Assert.AreEqual("", lue.Description)
        Assert.AreEqual("", lue.Genre)
        Assert.AreEqual(0, lue.AgeMin)
        Assert.AreEqual(0, lue.AgeMax)
        Assert.AreEqual(0, lue.DelaiPriseEnCharge, "pas de délai par défaut pour une spécialité absente")
    End Sub

    <TestMethod()> Public Sub Specialite_IdInconnu_RenvoieUneFicheVide()
        Assert.IsNull(ValeurSpecialite("oa_r_specialite_id", SpecialiteAbsente))

        Assert.AreEqual("", Table_specialite.GetSpecialiteDescription(SpecialiteAbsente))
        Dim lue = Table_specialite.GetSpecialiteById(SpecialiteAbsente)
        Assert.AreEqual(CLng(SpecialiteAbsente), lue.SpecialiteId)
        Assert.AreEqual("", lue.Description)
        Assert.AreEqual("", lue.Nature)
        Assert.IsFalse(lue.Parcours)
        Assert.IsFalse(lue.Oasis)
        Assert.AreEqual(0, lue.DelaiPriseEnCharge)
        Assert.AreEqual("", Table_specialite.GetSpecialiteDescription(0))
    End Sub

    <TestMethod()> Public Sub Specialite_SecondAppel_RenvoieLeMemeCache()
        Table_specialite.GetSpecialiteDescription(SpecialiteParcoursTest)
        Dim instance = InstanceEnCache(GetType(Table_specialite))
        Assert.IsNotNull(instance)

        Table_specialite.GetSpecialiteById(SpecialiteParcoursTest)

        Assert.AreSame(instance, InstanceEnCache(GetType(Table_specialite)))
    End Sub

    <TestMethod()> Public Sub Specialite_ChargementSousClient_LitLaTable()
        OublierInstance(GetType(Table_specialite))
        UtiliserCompte(Compte.Client)

        Assert.AreEqual(CStr(ValeurSpecialite("oa_r_specialite_description", SpecialiteParcoursTest)),
                        Table_specialite.GetSpecialiteDescription(SpecialiteParcoursTest))
        Assert.AreEqual(DelaiParDefautSansParametre, Table_specialite.GetSpecialiteById(SpecialiteTacheOasis).DelaiPriseEnCharge)
    End Sub

    <TestMethod()> Public Sub Specialite_ChargementSousWeb_LitLaTable()
        ' Oasis_Web (Dashboard, RDV, Synthese) affiche le libellé de la spécialité.
        OublierInstance(GetType(Table_specialite))
        UtiliserCompte(Compte.Web)

        Assert.AreEqual(SpecialiteParcoursTestLibelle, Table_specialite.GetSpecialiteDescription(SpecialiteParcoursTest))
    End Sub

    ' =========================================================================
    ' Table_categorie_majeure
    ' =========================================================================

    <TestMethod()> Public Sub CategorieMajeure_Active_RenvoieSonLibelle()
        Assert.AreEqual(DescriptionCategorie(CategorieMajeureDrcDeTest),
                        Table_categorie_majeure.GetCategorieMajeureDescription(CategorieMajeureDrcDeTest))
        Assert.AreEqual(DescriptionCategorie(CategorieMajeureDrcAutre),
                        Table_categorie_majeure.GetCategorieMajeureDescription(CategorieMajeureDrcAutre))
    End Sub

    <TestMethod()> Public Sub CategorieMajeure_IdInconnu_RenvoieUneChaineVide()
        Assert.AreEqual("", Table_categorie_majeure.GetCategorieMajeureDescription(CategorieAbsente))
        Assert.AreEqual("", Table_categorie_majeure.GetCategorieMajeureDescription(0))
    End Sub

    <TestMethod()> Public Sub CategorieMajeure_Inactive_EstAbsenteDuCache()
        ExigerLigneDeReference("La catégorie majeure inactive 9403",
                               "SELECT COUNT(*) FROM oasis.oa_r_categorie_majeure WHERE oa_r_categorie_majeure_id = @p0", CategorieInactive)

        Assert.AreEqual("", Table_categorie_majeure.GetCategorieMajeureDescription(CategorieInactive))
        Assert.IsFalse(Table_categorie_majeure.GetCategorieMajeureListe().ContainsKey(CategorieInactive))
    End Sub

    <TestMethod()> Public Sub CategorieMajeure_IndicateurInactifNul_EstAbsenteDuCache()
        ' Comportement actuel : le filtre n'accepte que 'False', une catégorie à
        ' indicateur NULL est écartée, alors que Table_genre et Table_specialite la gardent.
        ExigerLigneDeReference("La catégorie majeure 9404 sans indicateur",
                               "SELECT COUNT(*) FROM oasis.oa_r_categorie_majeure WHERE oa_r_categorie_majeure_id = @p0", CategorieSansIndicateur)

        Assert.AreEqual("", Table_categorie_majeure.GetCategorieMajeureDescription(CategorieSansIndicateur))
        Assert.IsFalse(Table_categorie_majeure.GetCategorieMajeureListe().ContainsKey(CategorieSansIndicateur))
    End Sub

    <TestMethod()> Public Sub CategorieMajeure_Liste_ContientExactementLesCategoriesActives()
        Dim liste = Table_categorie_majeure.GetCategorieMajeureListe()

        Assert.AreEqual(DescriptionCategorie(CategorieMajeureDrcDeTest), liste(CategorieMajeureDrcDeTest))
        Assert.AreEqual(DescriptionCategorie(CategorieMajeureDrcAutre), liste(CategorieMajeureDrcAutre))
        Dim actives = CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_r_categorie_majeure WHERE oa_r_categorie_majeure_inactif = 0"))
        Assert.AreEqual(actives, liste.Count)
    End Sub

    <TestMethod()> Public Sub CategorieMajeure_SecondAppel_RenvoieLeMemeCache()
        Dim premiere = Table_categorie_majeure.GetCategorieMajeureListe()
        Dim instance = InstanceEnCache(GetType(Table_categorie_majeure))

        Table_categorie_majeure.GetCategorieMajeureDescription(CategorieMajeureDrcDeTest)

        Assert.AreSame(premiere, Table_categorie_majeure.GetCategorieMajeureListe())
        Assert.AreSame(instance, InstanceEnCache(GetType(Table_categorie_majeure)))
    End Sub

    <TestMethod()> Public Sub CategorieMajeure_ChargementSousClient_LitLaTable()
        OublierInstance(GetType(Table_categorie_majeure))
        UtiliserCompte(Compte.Client)

        Assert.AreEqual(DescriptionCategorie(CategorieMajeureDrcDeTest),
                        Table_categorie_majeure.GetCategorieMajeureDescription(CategorieMajeureDrcDeTest))
    End Sub

    ' =========================================================================
    ' Table_ald
    ' =========================================================================

    <TestMethod()> Public Sub Ald_ParCode_RenvoieSaDescription()
        Assert.AreEqual(AldReferenceDiabeteDescription, Table_ald.GetAldDescription(AldReferenceDiabeteCode))
        Assert.AreEqual(AldReferenceCardiaqueDescription, Table_ald.GetAldDescription(AldReferenceCardiaqueCode))
    End Sub

    <TestMethod()> Public Sub Ald_ParId_RenvoieSaDescription()
        Assert.AreEqual(AldReferenceDiabeteDescription, Table_ald.GetAldDescription(AldReferenceDiabeteId))
        Assert.AreEqual(AldReferenceCardiaqueDescription, Table_ald.GetAldDescription(AldReferenceCardiaqueId))
    End Sub

    <TestMethod()> Public Sub Ald_CodeOuIdInconnu_RenvoieUneChaineVide()
        Assert.AreEqual("", Table_ald.GetAldDescription("00"))
        Assert.AreEqual("", Table_ald.GetAldDescription(AldAbsente))
    End Sub

    <TestMethod()> Public Sub Ald_Liste_ContientToutesLesAldSansFiltre()
        ' oa_ald n'a pas d'indicateur d'inactivité : le singleton lit toute la table.
        Dim liste = Table_ald.GetAldListe()

        Assert.AreEqual(AldReferenceDiabeteDescription, liste(AldReferenceDiabeteCode))
        Assert.AreEqual(AldReferenceCardiaqueDescription, liste(AldReferenceCardiaqueCode))
        Assert.AreEqual(CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_ald")), liste.Count)
    End Sub

    <TestMethod()> Public Sub Ald_SecondAppel_RenvoieLeMemeCache()
        Dim premiere = Table_ald.GetAldListe()
        Dim instance = InstanceEnCache(GetType(Table_ald))

        Table_ald.GetAldDescription(AldReferenceDiabeteCode)
        Table_ald.GetAldDescription(AldReferenceDiabeteId)

        Assert.AreSame(premiere, Table_ald.GetAldListe())
        Assert.AreSame(instance, InstanceEnCache(GetType(Table_ald)))
    End Sub

    <TestMethod()> Public Sub Ald_ChargementSousClient_LitLaTable()
        OublierInstance(GetType(Table_ald))
        UtiliserCompte(Compte.Client)

        Assert.AreEqual(AldReferenceDiabeteDescription, Table_ald.GetAldDescription(AldReferenceDiabeteCode))
    End Sub

End Class
