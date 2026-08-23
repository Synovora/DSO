Imports System.Configuration
Imports System.Data.SqlClient
Imports Oasis_Common
Imports Telerik.WinControls.UI

Module Environnement

    Public ReadOnly Property AssemblyVersion() As String
        Get
            Return System.Reflection.Assembly.GetExecutingAssembly().GetName().Version.ToString()
        End Get
    End Property

    Public Sub AfficheTitleForm(form As RadForm, titre As String, userLog As Utilisateur)
        ' --- centrage et chgt de style du titre du formulaire
        With form
            .Text = titre & " - " & GetProfilUserString(userLog) & " - " & String.Format("Version {0}", AssemblyVersion) & "   Date : " & Date.Now.ToString("dd.MM.yyyy")
            If form.FormElement IsNot Nothing Then
                Try
                    .FormElement.TitleBar.TitlePrimitive.StretchHorizontally = True
                    .FormElement.TitleBar.TitlePrimitive.TextAlignment = ContentAlignment.MiddleCenter
                    .FormElement.TitleBar.TitlePrimitive.ForeColor = Color.DarkBlue
                    '.FormElement.TitleBar.TitlePrimitive.Font = New Font(.FormElement.TitleBar.Font, FontStyle.Bold)
                Catch
                Finally
                End Try
            End If

        End With
    End Sub

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


    '=======================================================================================================================
    'Singleton : Sites
    '=======================================================================================================================
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
            Dim conxn As New SqlConnection(GetConnectionString())
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


    '=======================================================================================================================
    'Singleton : Unités sanitaires
    '=======================================================================================================================
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
        Private unite_sanitaire_numero_structure(indice) As Long

        ' Le constructeur est Private
        Private Sub New()
            'Déclaration des données de connexion
            Dim conxn As New SqlConnection(GetConnectionString())
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
            ReDim unite_sanitaire_numero_structure(rowCount + 1)

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
                unite_sanitaire_numero_structure(i) = Coalesce(unite_sanitaireDataTable.Rows(i)("numero_structure"), 0)
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



End Module

