Imports System.Data.SqlClient

Public Module EnvironnementBase
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

End Module

