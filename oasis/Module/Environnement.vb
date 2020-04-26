Imports System.Collections.Specialized
Imports System.Configuration
Imports System.Data.SqlClient
Imports Oasis_Common

Module Environnement

    Public Enum EnumAccesEcranPrecedent
        SANS
        LIGNE_DE_VIE
        LIGNE_DE_VIE_ET_SYNTHESE
        LIGNE_DE_VIE_ET_EPISODE
        EPISODE
        EPISODE_ET_SYNTHESE
        SYNTHESE
    End Enum

    Public Enum EnumResultat
        CreationOK
        ModificationOK
        AnnulationOK
        SuppressionOK
        Abandon
        AttenteAction
    End Enum


    Public Enum EnumSpecialiteOasis
        medecinReferent = 1
        IDE = 2
        sageFemmeOasis = 3
    End Enum

    Enum EnumCategoriePPS
        Objectif = 1
        MesurePreventive = 2
        Suivi = 3
        Strategie = 4
    End Enum

    Public Enum EnumSousCategoriePPS
        medecinReferent = 4
        IDE = 3
        sageFemme = 5
        specialiste = 6
    End Enum

    Public Enum EnumJourParFrequence
        parjour = 1
        parsemaine = 7
        parmois = 30
        paran = 365
        tousles2ans = 730
        tousles3ans = 1095
        tousles4ans = 1460
        tousles5ans = 1825
    End Enum

    Public Enum EnumAllergieOuContreIndication
        Allergie = 1
        ContreIndication = 2
    End Enum

    Public Enum EnumTypeNote
        Medicale = 1
        Vaccin = 2
        Social = 3
        Administratif = 4
        Directive = 5
    End Enum

    Public Enum EnumPosition
        Droite = 0
        Gauche = 1
    End Enum

    Public Enum EnumForm
        SYNTHESE
        EPISODE
        LIGNE_DE_VIE
    End Enum

    '=========================================================================
    '================== Contrôle d'accès pour les principaux écrans (Synthèse, Episode et Ligne de vie
    '=========================================================================

    Public Class ControleAccesForm
        Private Shared instance As ControleAccesForm = Nothing
        Private ReadOnly form_acces As New List(Of String)()

        Public Shared Function IsAccessToFormOK(formAcces As String) As Boolean
            ' Création de l'instance si elle n'existe pas
            If instance Is Nothing Then
                instance = New ControleAccesForm
            End If

            If instance.form_acces.Contains(formAcces) = True Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Shared Sub AddFormToControl(formAcces As String)
            ' Création de l'instance si elle n'existe pas
            If instance Is Nothing Then
                instance = New ControleAccesForm
            End If

            If instance.form_acces.Contains(formAcces) = False Then
                instance.form_acces.Add(formAcces)
            End If
        End Sub

        Public Shared Sub RemoveFormToControl(formAcces As String)
            ' Création de l'instance si elle n'existe pas
            If instance Is Nothing Then
                instance = New ControleAccesForm
            End If

            If instance.form_acces.Contains(formAcces) = True Then
                instance.form_acces.Remove(formAcces)
            End If
        End Sub

    End Class

    'Contrôle d'accès aux épisodes
    Public Class ControleAccesEpisode
        Private Shared instance As ControleAccesEpisode = Nothing
        Private ReadOnly episode_acces As New List(Of Long)()

        Public Shared Function IsAccessToEpisodeOK(episodeAcces As Long) As Boolean
            ' Création de l'instance si elle n'existe pas
            If instance Is Nothing Then
                instance = New ControleAccesEpisode
            End If

            If instance.episode_acces.Contains(episodeAcces) = True Then
                Return False
            Else
                Return True
            End If
        End Function

        Public Shared Sub AddEpisodeToControl(episodeAcces As Long)
            ' Création de l'instance si elle n'existe pas
            If instance Is Nothing Then
                instance = New ControleAccesEpisode
            End If

            If instance.episode_acces.Contains(episodeAcces) = False Then
                instance.episode_acces.Add(episodeAcces)
            End If
        End Sub

        Public Shared Sub RemoveEpisodeToControl(episodeAcces As String)
            ' Création de l'instance si elle n'existe pas
            If instance Is Nothing Then
                instance = New ControleAccesEpisode
            End If

            If instance.episode_acces.Contains(episodeAcces) = True Then
                instance.episode_acces.Remove(episodeAcces)
            End If
        End Sub

    End Class

    '=========================================================================
    '================== Données persistante de session =======================
    '=========================================================================

    'Singletons : Genre, Site, Unité sanitaire, Spécialité médicale, Catégorie majeure, ALD


    'Singleton : Genres
    Public Class Table_genre
        ' Variable locale pour stocker une référence vers l'instance
        Private Shared instance As Table_genre = Nothing
        ' Déclaration du Dictionnaire comme membre de la classe pour stocker les occurences lues
        Private ReadOnly genre As New Dictionary(Of String, String)()

        ' Le constructeur est Private, on ne doit pas pouvoir créer une instance autrement que depuis la classe elle-même
        Private Sub New()
            'Déclaration des données de connexion
            Dim conxn As New SqlConnection(getConnectionString())
            Dim genreDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Dim genreDataTable As DataTable = New DataTable()

            'Lecture des données en base
            genreDataAdapter.SelectCommand = New SqlCommand("Select * from oasis.oa_r_genre where oa_r_genre_inactif = 'False' or oa_r_genre_inactif is Null;", conxn)
            genreDataAdapter.Fill(genreDataTable)
            conxn.Open()

            'Parcours du DataTable pour alimenter les colonnes à calculer (hors BDD)
            Dim i As Integer
            Dim rowCount As Integer = genreDataTable.Rows.Count - 1

            'Alimentation du tableau
            For i = 0 To rowCount Step 1
                genre.Add(genreDataTable.Rows(i)("oa_r_genre_code"), genreDataTable.Rows(i)("oa_r_genre_description"))
            Next

            conxn.Close()
            genreDataAdapter.Dispose()
        End Sub

        Public Shared Function GetGenreDescription(pGenre_code As String) As String
            ' Création de l'instance si elle n'existe pas
            If instance Is Nothing Then
                instance = New Table_genre
            End If

            If instance.genre.ContainsKey(pGenre_code) = True Then
                Return instance.genre(pGenre_code).ToString()
            Else
                Return ""
            End If

        End Function

        Public Shared Function GetGenreListe() As Dictionary(Of String, String)
            If instance Is Nothing Then
                instance = New Table_genre
            End If

            Return Table_genre.instance.genre
        End Function
    End Class

    'Singleton : Sites
    Public Class Table_site
        Private ReadOnly SiteDico As New Dictionary(Of Integer, String)()
        Private ReadOnly SiteDicoParUniteSanitaire As New Dictionary(Of Integer, String)()
        Dim indice As Integer = 10
        Dim rowCount As Integer
        ' Variable locale pour stocker une référence vers l'instance
        Private Shared instance As Table_site = Nothing
        Private site_id(indice) As Integer
        Private site_description(indice) As String
        Private site_territoire_id(indice) As Integer
        Private site_unite_sanitaire_id(indice) As Integer
        Private site_adresse1(indice) As String
        Private site_adresse2(indice) As String
        Private site_ville(indice) As String
        Private site_code_postal(indice) As String

        ' Le constructeur est Private
        Private Sub New()
            'Déclaration des données de connexion
            Dim conxn As New SqlConnection(getConnectionString())
            Dim siteDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Dim siteDataTable As DataTable = New DataTable()

            'Lecture des données en base
            siteDataAdapter.SelectCommand = New SqlCommand("select * from oasis.oa_site where oa_site_inactif = 'False' or oa_site_inactif is Null;", conxn)
            siteDataAdapter.Fill(siteDataTable)
            conxn.Open()

            'Parcours du DataTable pour alimenter les colonnes à calculer (hors BDD) : dans ce cas il s'agit du cacul de l'age à l'aide de la date de naissance du DataTable
            Dim i As Integer
            rowCount = siteDataTable.Rows.Count - 1

            'Redimensionnement du tableau selon le nombre d'occurrences lues
            indice = rowCount + 1
            ReDim site_id(rowCount + 1)
            ReDim site_description(rowCount + 1)
            ReDim site_territoire_id(rowCount + 1)
            ReDim site_unite_sanitaire_id(rowCount + 1)
            ReDim site_adresse1(rowCount + 1)
            ReDim site_adresse2(rowCount + 1)
            ReDim site_ville(rowCount + 1)
            ReDim site_code_postal(rowCount + 1)

            'Alimentation du tableau
            For i = 0 To rowCount Step 1
                SiteDico.Add(siteDataTable.Rows(i)("oa_site_id"), siteDataTable.Rows(i)("oa_site_description"))
                site_id(i) = siteDataTable.Rows(i)("oa_site_id")

                If siteDataTable.Rows(i)("oa_site_description") Is DBNull.Value Then
                    site_description(i) = ""
                Else
                    site_description(i) = siteDataTable.Rows(i)("oa_site_description")
                End If

                If siteDataTable.Rows(i)("oa_site_territoire_id") Is DBNull.Value Then
                    site_territoire_id(i) = 0
                Else
                    site_territoire_id(i) = siteDataTable.Rows(i)("oa_site_territoire_id")
                End If

                If siteDataTable.Rows(i)("oa_site_unite_sanitaire_id") Is DBNull.Value Then
                    site_unite_sanitaire_id(i) = 0
                Else
                    site_unite_sanitaire_id(i) = siteDataTable.Rows(i)("oa_site_unite_sanitaire_id")
                End If

                If siteDataTable.Rows(i)("oa_site_adresse1") Is DBNull.Value Then
                    site_adresse1(i) = ""
                Else
                    site_adresse1(i) = siteDataTable.Rows(i)("oa_site_adresse1")
                End If

                If siteDataTable.Rows(i)("oa_site_adresse2") Is DBNull.Value Then
                    site_adresse2(i) = ""
                Else
                    site_adresse2(i) = siteDataTable.Rows(i)("oa_site_adresse2")
                End If

                If siteDataTable.Rows(i)("oa_site_ville") Is DBNull.Value Then
                    site_ville(i) = ""
                Else
                    site_ville(i) = siteDataTable.Rows(i)("oa_site_ville")
                End If

                If siteDataTable.Rows(i)("oa_site_code_postal") Is DBNull.Value Then
                    site_code_postal(i) = ""
                Else
                    site_code_postal(i) = siteDataTable.Rows(i)("oa_site_code_postal")
                End If
            Next

            conxn.Close()
            siteDataAdapter.Dispose()
        End Sub

        Public Shared Function GetSiteDescription(pSite_id As String) As String
            ' Création de l'instance si elle n'existe pas
            If instance Is Nothing Then
                instance = New Table_site
            End If
            ' Renvoi de l'information demandée
            For i = 0 To instance.indice - 1 Step 1
                If pSite_id = instance.site_id(i) Then
                    Return instance.site_description(i)
                End If
            Next

            ' Renvoi du paramètre vide si l'élément demandé n'existe pas
            GetSiteDescription = ""

        End Function

        Public Shared Function GetSiteListe() As Dictionary(Of Integer, String)
            If instance Is Nothing Then
                instance = New Table_site
            End If

            Return Table_site.instance.SiteDico
        End Function


        Public Shared Function GetSiteListeParUniteSanitaire(uniteSantaireId As Integer) As Dictionary(Of Integer, String)
            If instance Is Nothing Then
                instance = New Table_site
            End If

            Table_site.instance.SiteDicoParUniteSanitaire.Clear()

            'Alimentation du tableau
            For i = 0 To instance.rowCount Step 1
                If instance.site_unite_sanitaire_id(i) = uniteSantaireId Then
                    instance.SiteDicoParUniteSanitaire.Add(instance.site_id(i), instance.site_description(i))
                End If
            Next

            Return Table_site.instance.SiteDicoParUniteSanitaire
        End Function
    End Class


    'Singleton : Unités sanitaires
    Public Class Table_unite_sanitaire
        Dim indice As Integer = 10
        ' Variable locale pour stocker une référence vers l'instance
        Private UniteSanitaireDico As New Dictionary(Of Integer, String)()
        Private Shared instance As Table_unite_sanitaire = Nothing
        Private unite_sanitaire_id(indice) As Integer
        Private unite_sanitaire_description(indice) As String
        Private unite_sanitaire_siege_id(indice) As Integer
        Private unite_sanitaire_adresse1(indice) As String
        Private unite_sanitaire_adresse2(indice) As String
        Private unite_sanitaire_ville(indice) As String
        Private unite_sanitaire_code_postal(indice) As String

        ' Le constructeur est Private
        Private Sub New()
            'Déclaration des données de connexion
            Dim conxn As New SqlConnection(getConnectionString())
            Dim unite_sanitaireDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Dim unite_sanitaireDataTable As DataTable = New DataTable()

            'Lecture des données en base
            unite_sanitaireDataAdapter.SelectCommand = New SqlCommand("select * from oasis.oa_unite_sanitaire where oa_unite_sanitaire_inactif = 'False' or oa_unite_sanitaire_inactif is Null;", conxn)
            unite_sanitaireDataAdapter.Fill(unite_sanitaireDataTable)
            conxn.Open()

            'Parcours du DataTable pour alimenter les colonnes à calculer (hors BDD) : dans ce cas il s'agit du cacul de l'age à l'aide de la date de naissance du DataTable
            Dim i As Integer
            Dim rowCount As Integer = unite_sanitaireDataTable.Rows.Count - 1

            'Redimensionnement du tableau selon le nombre d'occurrences lues
            indice = rowCount + 1
            ReDim unite_sanitaire_id(rowCount + 1)
            ReDim unite_sanitaire_description(rowCount + 1)
            ReDim unite_sanitaire_siege_id(rowCount + 1)
            ReDim unite_sanitaire_adresse1(rowCount + 1)
            ReDim unite_sanitaire_adresse2(rowCount + 1)
            ReDim unite_sanitaire_ville(rowCount + 1)
            ReDim unite_sanitaire_code_postal(rowCount + 1)

            'Alimentation du tableau
            For i = 0 To rowCount Step 1
                UniteSanitaireDico.Add(unite_sanitaireDataTable.Rows(i)("oa_unite_sanitaire_id"), unite_sanitaireDataTable.Rows(i)("oa_unite_sanitaire_description"))
                unite_sanitaire_id(i) = unite_sanitaireDataTable.Rows(i)("oa_unite_sanitaire_id")

                If unite_sanitaireDataTable.Rows(i)("oa_unite_sanitaire_description") Is DBNull.Value Then
                    unite_sanitaire_description(i) = ""
                Else
                    unite_sanitaire_description(i) = unite_sanitaireDataTable.Rows(i)("oa_unite_sanitaire_description")
                End If

                If unite_sanitaireDataTable.Rows(i)("oa_unite_sanitaire_siege_id") Is DBNull.Value Then
                    unite_sanitaire_siege_id(i) = 0
                Else
                    unite_sanitaire_siege_id(i) = unite_sanitaireDataTable.Rows(i)("oa_unite_sanitaire_siege_id")
                End If

                If unite_sanitaireDataTable.Rows(i)("oa_unite_sanitaire_adresse1") Is DBNull.Value Then
                    unite_sanitaire_adresse1(i) = ""
                Else
                    unite_sanitaire_adresse1(i) = unite_sanitaireDataTable.Rows(i)("oa_unite_sanitaire_adresse1")
                End If

                If unite_sanitaireDataTable.Rows(i)("oa_unite_sanitaire_adresse2") Is DBNull.Value Then
                    unite_sanitaire_adresse2(i) = ""
                Else
                    unite_sanitaire_adresse2(i) = unite_sanitaireDataTable.Rows(i)("oa_unite_sanitaire_adresse2")
                End If

                If unite_sanitaireDataTable.Rows(i)("oa_unite_sanitaire_ville") Is DBNull.Value Then
                    unite_sanitaire_ville(i) = ""
                Else
                    unite_sanitaire_ville(i) = unite_sanitaireDataTable.Rows(i)("oa_unite_sanitaire_ville")
                End If

                If unite_sanitaireDataTable.Rows(i)("oa_unite_sanitaire_code_postal") Is DBNull.Value Then
                    unite_sanitaire_code_postal(i) = ""
                Else
                    unite_sanitaire_code_postal(i) = unite_sanitaireDataTable.Rows(i)("oa_unite_sanitaire_code_postal")
                End If
            Next

            conxn.Close()
            unite_sanitaireDataAdapter.Dispose()
        End Sub

        Public Shared Function GetUniteSanitaireDescription(pUnite_sanitaire_id As String) As String
            ' Création de l'instance si elle n'existe pas
            If instance Is Nothing Then
                instance = New Table_unite_sanitaire
            End If
            ' Renvoi de l'information demandée
            For i = 0 To instance.indice - 1 Step 1
                If pUnite_sanitaire_id = instance.unite_sanitaire_id(i) Then
                    Return instance.unite_sanitaire_description(i)
                End If
            Next

            ' Renvoi du paramètre vide si l'élément demandé n'existe pas
            GetUniteSanitaireDescription = ""

        End Function


        Public Shared Function GetUniteSanitaireListe() As Dictionary(Of Integer, String)
            If instance Is Nothing Then
                instance = New Table_unite_sanitaire
            End If

            Return Table_unite_sanitaire.instance.UniteSanitaireDico
        End Function
    End Class


    'Singleton : Spécialités médicales
    Public Class Table_specialite
        Dim indice As Integer = 60
        ' Variable locale pour stocker une référence vers l'instance
        Private Shared instance As Table_specialite = Nothing
        Private specialite_id(indice) As Integer
        Private specialite_Code(indice) As String
        Private specialite_description(indice) As String
        Private specialite_nature(indice) As String
        Private specialite_genre(indice) As String
        Private specialite_parcours(indice) As Boolean
        Private specialite_oasis(indice) As Boolean
        Private specialite_ageMin(indice) As Integer
        Private specialite_ageMax(indice) As Integer
        Private specialite_delaiPriseEnCharge(indice) As Integer

        Private Shared Delai As Integer

        Private Sub New()
            'Déclaration des données de connexion
            Dim conxn As New SqlConnection(getConnectionString())
            Dim specialiteDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Dim specialiteDataTable As DataTable = New DataTable()

            'Lecture des données en base
            specialiteDataAdapter.SelectCommand = New SqlCommand("select * from oasis.oa_r_specialite where oa_r_specialite_inactif = 'False' or oa_r_specialite_inactif is Null;", conxn)

            specialiteDataAdapter.Fill(specialiteDataTable)
            conxn.Open()

            'Parcours du DataTable pour alimenter les colonnes à calculer (hors BDD) : dans ce cas il s'agit du cacul de l'age à l'aide de la date de naissance du DataTable
            Dim i As Integer
            Dim rowCount As Integer = specialiteDataTable.Rows.Count - 1

            'Redimensionnement du tableau selon le nombre d'occurrences lues
            indice = rowCount + 1
            ReDim specialite_id(rowCount + 1)
            ReDim specialite_Code(rowCount + 1)
            ReDim specialite_description(rowCount + 1)
            ReDim specialite_nature(rowCount + 1)
            ReDim specialite_genre(rowCount + 1)
            ReDim specialite_parcours(rowCount + 1)
            ReDim specialite_oasis(rowCount + 1)
            ReDim specialite_ageMin(rowCount + 1)
            ReDim specialite_ageMax(rowCount + 1)
            ReDim specialite_delaiPriseEnCharge(rowCount + 1)

            'Alimentation du tableau
            For i = 0 To rowCount Step 1
                specialite_id(i) = specialiteDataTable.Rows(i)("oa_r_specialite_id")
                specialite_Code(i) = Coalesce(specialiteDataTable.Rows(i)("oa_specialite_code"), "")
                specialite_description(i) = Coalesce(specialiteDataTable.Rows(i)("oa_r_specialite_description"), "")
                specialite_nature(i) = Coalesce(specialiteDataTable.Rows(i)("oa_r_specialite_nature"), "")
                specialite_genre(i) = Coalesce(specialiteDataTable.Rows(i)("oa_r_specialite_genre"), "")
                specialite_parcours(i) = Coalesce(specialiteDataTable.Rows(i)("oa_r_parcours"), False)
                specialite_oasis(i) = Coalesce(specialiteDataTable.Rows(i)("oa_r_oasis"), False)
                specialite_ageMin(i) = Coalesce(specialiteDataTable.Rows(i)("oa_r_specialite_age_min"), 0)
                specialite_ageMax(i) = Coalesce(specialiteDataTable.Rows(i)("oa_r_specialite_age_max"), 0)
                specialite_delaiPriseEnCharge(i) = Coalesce(specialiteDataTable.Rows(i)("oa_r_delaiPriseEnCharge"), 0)
            Next

            conxn.Close()
            specialiteDataAdapter.Dispose()

            Dim DelaiString As String = ConfigurationManager.AppSettings("SpecialiteDelaiPriseEnCharge")
            If IsNumeric(DelaiString) Then
                Delai = CInt(DelaiString)
            Else
                CreateLog("Paramètre application 'SpecialiteDelaiPriseEnCharge' non trouvé !", "Environnement.Table_specialite.New()", LogDao.EnumTypeLog.ERREUR.ToString)
                Delai = 30
            End If

        End Sub

        Public Shared Function GetSpecialiteDescription(pSpecialite_id As Integer) As String
            ' Création de l'instance si elle n'existe pas
            If instance Is Nothing Then
                instance = New Table_specialite
            End If
            ' Renvoi de l'information demandée
            For i = 0 To instance.indice - 1 Step 1
                If pSpecialite_id = instance.specialite_id(i) Then
                    Return instance.specialite_description(i)
                End If
            Next

            ' Renvoi du paramètre vide si l'élément demandé n'existe pas
            GetSpecialiteDescription = ""
        End Function
        Public Shared Function GetSpecialiteById(pSpecialite_id As Integer) As Specialite
            Dim specialite As New Specialite

            specialite.SpecialiteId = pSpecialite_id
            specialite.Code = ""
            specialite.Description = ""
            specialite.Nature = ""
            specialite.Genre = ""
            specialite.Parcours = False
            specialite.Oasis = False
            specialite.AgeMin = 0
            specialite.AgeMax = 0
            specialite.DelaiPriseEnCharge = 0
            ' Création de l'instance si elle n'existe pas
            If instance Is Nothing Then
                instance = New Table_specialite
            End If
            ' Renvoi de l'information demandée
            For i = 0 To instance.indice - 1 Step 1
                If pSpecialite_id = instance.specialite_id(i) Then
                    specialite.Code = instance.specialite_Code(i)
                    specialite.Description = instance.specialite_description(i)
                    specialite.Nature = instance.specialite_nature(i)
                    specialite.Genre = instance.specialite_genre(i)
                    specialite.Parcours = instance.specialite_parcours(i)
                    specialite.Oasis = instance.specialite_oasis(i)
                    specialite.AgeMin = instance.specialite_ageMin(i)
                    specialite.AgeMax = instance.specialite_ageMax(i)
                    If instance.specialite_delaiPriseEnCharge(i) <> 0 Then
                        specialite.DelaiPriseEnCharge = instance.specialite_delaiPriseEnCharge(i)
                    Else
                        specialite.DelaiPriseEnCharge = delai
                    End If
                End If
            Next
            Return specialite
        End Function

    End Class



    'Singleton : Catégories majeures
    Public Class Table_categorie_majeure
        Dim indice As Integer = 10
        ' Variable locale pour stocker une référence vers l'instance
        Private CategorieMajeureDico As New Dictionary(Of Integer, String)()
        Private Shared instance As Table_categorie_majeure = Nothing
        Private categorie_majeure_id(indice) As Integer
        Private categorie_majeure_code(indice) As String
        Private categorie_majeure_description(indice) As String

        ' Le constructeur est Private
        Private Sub New()
            'Déclaration des données de connexion
            Dim conxn As New SqlConnection(getConnectionString())
            Dim CategorieMajeureDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Dim CategorieMajeureDataTable As DataTable = New DataTable()

            'Lecture des données en base
            CategorieMajeureDataAdapter.SelectCommand = New SqlCommand("select * from oasis.oa_r_categorie_majeure where oa_r_categorie_majeure_inactif = 'False';", conxn)
            CategorieMajeureDataAdapter.Fill(CategorieMajeureDataTable)
            conxn.Open()

            'Parcours du DataTable
            Dim i As Integer
            Dim rowCount As Integer = CategorieMajeureDataTable.Rows.Count - 1

            'Redimensionnement du tableau selon le nombre d'occurrences lues
            indice = rowCount + 1
            ReDim categorie_majeure_id(rowCount + 1)
            ReDim categorie_majeure_code(rowCount + 1)
            ReDim categorie_majeure_description(rowCount + 1)

            'Alimentation du tableau
            For i = 0 To rowCount Step 1
                CategorieMajeureDico.Add(CategorieMajeureDataTable.Rows(i)("oa_r_categorie_majeure_id"), CategorieMajeureDataTable.Rows(i)("oa_r_categorie_majeure_description"))
                categorie_majeure_id(i) = CategorieMajeureDataTable.Rows(i)("oa_r_categorie_majeure_id")

                If CategorieMajeureDataTable.Rows(i)("oa_r_categorie_majeure_code") Is DBNull.Value Then
                    categorie_majeure_code(i) = ""
                Else
                    categorie_majeure_code(i) = CategorieMajeureDataTable.Rows(i)("oa_r_categorie_majeure_code")
                End If

                If CategorieMajeureDataTable.Rows(i)("oa_r_categorie_majeure_description") Is DBNull.Value Then
                    categorie_majeure_description(i) = 0
                Else
                    categorie_majeure_description(i) = CategorieMajeureDataTable.Rows(i)("oa_r_categorie_majeure_description")
                End If
            Next

            conxn.Close()
            CategorieMajeureDataAdapter.Dispose()
        End Sub

        Public Shared Function GetCategorieMajeureDescription(pCategorieMajeureId As Integer) As String
            ' Création de l'instance si elle n'existe pas
            If instance Is Nothing Then
                instance = New Table_categorie_majeure
            End If
            ' Renvoi de l'information demandée
            For i = 0 To instance.indice - 1 Step 1
                If pCategorieMajeureId = instance.categorie_majeure_id(i) Then
                    Return instance.categorie_majeure_description(i)
                End If
            Next

            ' Renvoi du paramètre vide si l'élément demandé n'existe pas
            GetCategorieMajeureDescription = ""

        End Function


        Public Shared Function GetCategorieMajeureListe() As Dictionary(Of Integer, String)
            If instance Is Nothing Then
                instance = New Table_categorie_majeure
            End If

            Return Table_categorie_majeure.instance.CategorieMajeureDico
        End Function
    End Class



    'Singleton : ALD
    Public Class Table_ald
        Dim indice As Integer = 10
        ' Variable locale pour stocker une référence vers l'instance
        Private AldDico As New Dictionary(Of String, String)()
        Private Shared instance As Table_ald = Nothing
        Private ald_id(indice) As Integer
        Private ald_code(indice) As String
        Private ald_description(indice) As String

        ' Le constructeur est Private
        Private Sub New()
            'Déclaration des données de connexion
            Dim conxn As New SqlConnection(getConnectionString())
            Dim AldDataAdapter As SqlDataAdapter = New SqlDataAdapter()
            Dim AldDataTable As DataTable = New DataTable()

            'Lecture des données en base
            AldDataAdapter.SelectCommand = New SqlCommand("select * from oasis.oa_ald;", conxn)
            AldDataAdapter.Fill(AldDataTable)
            conxn.Open()

            'Parcours du DataTable
            Dim i As Integer
            Dim rowCount As Integer = AldDataTable.Rows.Count - 1

            'Redimensionnement du tableau selon le nombre d'occurrences lues
            indice = rowCount + 1
            ReDim ald_id(rowCount + 1)
            ReDim ald_code(rowCount + 1)
            ReDim ald_description(rowCount + 1)

            'Alimentation du tableau
            For i = 0 To rowCount Step 1
                AldDico.Add(AldDataTable.Rows(i)("oa_ald_code"), AldDataTable.Rows(i)("oa_ald_description"))
                ald_id(i) = AldDataTable.Rows(i)("oa_ald_id")

                If AldDataTable.Rows(i)("oa_ald_code") Is DBNull.Value Then
                    ald_code(i) = ""
                Else
                    ald_code(i) = AldDataTable.Rows(i)("oa_ald_code")
                End If

                If AldDataTable.Rows(i)("oa_ald_description") Is DBNull.Value Then
                    ald_description(i) = 0
                Else
                    ald_description(i) = AldDataTable.Rows(i)("oa_ald_description")
                End If
            Next

            conxn.Close()
            AldDataAdapter.Dispose()
        End Sub

        Public Shared Function GetAldDescription(pAldCode As String) As String
            ' Création de l'instance si elle n'existe pas
            If instance Is Nothing Then
                instance = New Table_ald
            End If
            ' Renvoi de l'information demandée
            For i = 0 To instance.indice - 1 Step 1
                If pAldCode = instance.ald_code(i) Then
                    Return instance.ald_description(i)
                End If
            Next

            ' Renvoi du paramètre vide si l'élément demandé n'existe pas
            GetAldDescription = ""

        End Function


        Public Shared Function GetAldDescription(pAldId As Integer) As String
            ' Création de l'instance si elle n'existe pas
            If instance Is Nothing Then
                instance = New Table_ald
            End If
            ' Renvoi de l'information demandée
            For i = 0 To instance.indice - 1 Step 1
                If pAldId = instance.ald_id(i) Then
                    Return instance.ald_description(i)
                End If
            Next

            ' Renvoi du paramètre vide si l'élément demandé n'existe pas
            GetAldDescription = ""

        End Function


        Public Shared Function GetAldListe() As Dictionary(Of String, String)
            If instance Is Nothing Then
                instance = New Table_ald
            End If

            Return Table_ald.instance.AldDico
        End Function
    End Class


End Module

