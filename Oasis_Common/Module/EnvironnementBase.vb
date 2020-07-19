Imports System.Data.SqlClient

Public Module EnvironnementBase

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
        IDE = 3
        medecinReferent = 4
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
    '================== Données persistante de session =======================
    '=========================================================================

    'Singletons : Genre, Site, Unité sanitaire, Spécialité médicale, Catégorie majeure, ALD


    '=======================================================================================================================
    'Singleton : Genres
    '=======================================================================================================================
    Public Class Table_genre
        ' Variable locale pour stocker une référence vers l'instance
        Private Shared instance As Table_genre = Nothing
        ' Déclaration du Dictionnaire comme membre de la classe pour stocker les occurences lues
        Private ReadOnly genre As New Dictionary(Of String, String)()

        ' Le constructeur est Private, on ne doit pas pouvoir créer une instance autrement que depuis la classe elle-même
        Private Sub New()
            'Déclaration des données de connexion
            Dim conxn As New SqlConnection(GetConnectionString())
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

            Return instance.genre
        End Function
    End Class


    '=======================================================================================================================
    'Singleton : Spécialités médicales
    '=======================================================================================================================
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
            Dim conxn As New SqlConnection(GetConnectionString())
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

            Dim DelaiString As String = "test" 'TODO: ConfigurationManager.AppSettings("SpecialiteDelaiPriseEnCharge")
            If IsNumeric(DelaiString) Then
                Delai = CInt(DelaiString)
            Else
                'TODO: CreateLog("Paramètre application 'SpecialiteDelaiPriseEnCharge' non trouvé !", "Environnement.Table_specialite.New()", LogDao.EnumTypeLog.ERREUR.ToString, userLog)
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
                        specialite.DelaiPriseEnCharge = Delai
                    End If
                End If
            Next
            Return specialite
        End Function

    End Class


    '=======================================================================================================================
    'Singleton : Catégories majeures
    '=======================================================================================================================
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
            Dim conxn As New SqlConnection(GetConnectionString())
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


    '=======================================================================================================================
    'Singleton : ALD
    '=======================================================================================================================
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
            Dim conxn As New SqlConnection(GetConnectionString())
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

