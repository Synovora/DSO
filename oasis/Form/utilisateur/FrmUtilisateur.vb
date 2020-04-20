Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class FrmUtilisateur
    Dim utilisateur As Utilisateur
    Dim lstProfil As List(Of Profil)
    Dim lstSiege As List(Of Siege)
    Dim lstUniteSan As List(Of UniteSanitaire)
    Dim lstSite As List(Of Site)

    Dim isCreation As Boolean
    Dim idSiegeDefaut As Integer = 0

    Public Sub New(utilisateur As Utilisateur)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        afficheTitleForm(Me, Me.Text)
        Me.utilisateur = utilisateur
        isCreation = If(utilisateur.UtilisateurId = 0, True, False)

        InitCtrl()

    End Sub

    Private Sub InitCtrl()
        ' -- chargement listes globales de reference
        lstProfil = (New ProfilDao()).getListProfil()
        lstSiege = (New SiegeDao).getLstSiege(Not isCreation)

        ' -- si mode modif => on recherche le siege
        If isCreation = False AndAlso utilisateur.UtilisateurUniteSanitaireId <> 0 Then
            Dim uniteS = (New UniteSanitaireDao()).getUniteSanitaireById(utilisateur.UtilisateurUniteSanitaireId, True)
            idSiegeDefaut = uniteS.Oa_unite_sanitaire_siege_id
        End If
        Me.TxtIdentifiant.Text = utilisateur.UtilisateurLogin
        Me.TxtNom.Text = utilisateur.UtilisateurNom
        Me.TxtPrenom.Text = utilisateur.UtilisateurPrenom
        Me.TxtTelephone.Text = utilisateur.UtilisateurTelephone
        Me.TxtMail.Text = utilisateur.UtilisateurMail

        ' -- Profil ----------------------------------------------------------
        Me.DropDownProfil.Items.Clear()
        For Each profil As Profil In lstProfil
            Dim radListItem As New RadListDataItem(profil.Designation, profil)
            Me.DropDownProfil.Items.Add(radListItem)
            'If TryCast(radListItem.Value, Profil).Id = Me.utilisateur.UtilisateurProfilId Then
            If profil.Id = Me.utilisateur.UtilisateurProfilId Then
                radListItem.Selected = True
            End If
        Next
        If isCreation AndAlso Me.DropDownProfil.Items.Count > 0 Then
            Me.DropDownProfil.SelectedItem = Me.DropDownProfil.Items(0)
        End If
        Me.DropDownProfil.DefaultItemsCountInDropDown = DropDownProfil.Items.Count

        ' -- Siege (qui va initer les unités sanitaire et les site en cascade sur event SelectedIndexChanged ) --------
        Me.DropDownSiege.Items.Clear()
        For Each siege As Siege In lstSiege
            Dim radListItem As New RadListDataItem(siege.SiegeDescription, siege)
            Me.DropDownSiege.Items.Add(radListItem)
            If siege.SiegeId = idSiegeDefaut Then
                radListItem.Selected = True
            End If
        Next
        If isCreation AndAlso Me.DropDownSiege.Items.Count > 0 Then
            Me.DropDownSiege.SelectedItem = Me.DropDownSiege.Items(0)
        End If
        Me.DropDownSiege.DefaultItemsCountInDropDown = DropDownSiege.Items.Count

    End Sub

    Private Sub initUniteSanitaire(siegeId As Long)
        lstUniteSan = (New UniteSanitaireDao).getList(Not isCreation, siegeId)

        ' -- Siege ----------------------------------------------------------
        Me.DropDownUS.Items.Clear()
        Me.DropDownUS.Items.Add(New RadListDataItem("", New UniteSanitaire()))  ' -- un vide pour sans unite sanitaire
        For Each uniteS As UniteSanitaire In lstUniteSan
            Dim radListItem As New RadListDataItem(uniteS.Oa_unite_sanitaire_description, uniteS)
            Me.DropDownUS.Items.Add(radListItem)
            If uniteS.Oa_unite_sanitaire_id = utilisateur.UtilisateurUniteSanitaireId Then
                radListItem.Selected = True
            End If
        Next
        If isCreation AndAlso Me.DropDownUS.Items.Count > 0 Then
            Me.DropDownUS.SelectedItem = Me.DropDownUS.Items(0)
        End If
        Me.DropDownUS.DefaultItemsCountInDropDown = DropDownUS.Items.Count

    End Sub

    Private Sub initSite(UniteSanId As Long)
        lstSite = If(UniteSanId = 0, New List(Of Site), (New SiteDao).getList(Not isCreation, UniteSanId))

        ' -- Siege ----------------------------------------------------------
        Me.DropDownSite.Items.Clear()
        Me.DropDownSite.Items.Add(New RadListDataItem("", New Site()))  ' -- un vide pour sans  site
        For Each site As Site In lstSite
            Dim radListItem As New RadListDataItem(site.Oa_site_description, site)
            Me.DropDownSite.Items.Add(radListItem)
            If site.Oa_site_id = utilisateur.UtilisateurSiteId Then
                radListItem.Selected = True
            End If
        Next
        If isCreation AndAlso Me.DropDownSite.Items.Count > 0 Then
            Me.DropDownSite.SelectedItem = Me.DropDownSite.Items(0)
        End If
        Me.DropDownSite.DefaultItemsCountInDropDown = DropDownSite.Items.Count

    End Sub

    Private Function ctrlFields() As String
        Dim message = ""
        If TxtIdentifiant.Text.Length < 5 Then
            message += ". L'identifiant doit faire au moins 5 caracetères" & vbCrLf
        End If
        If TxtNom.Text.Trim() = "" Then
            message += ". Le Nom est obligatoire" & vbCrLf
        End If
        If TxtPrenom.Text.Trim() = "" Then
            message += ". Le Prénom est obligatoire" & vbCrLf
        End If
        If TxtTelephone.Text.Trim() = "" Then
            message += ". Le téléphone est obligatoire" & vbCrLf
        End If
        If TxtMail.Text.Trim() = "" Then
            message += ". L'adresse Email est obligatoire" & vbCrLf
        ElseIf IsValidEmail(TxtMail.Text) = False Then
            message += ". L'adresse Email est incorrecte" & vbCrLf
        End If
        If TxtPassword1.Text.Trim().Length < 8 Then
            message += ". Le mot de passe doit faire au moins 8 caractères" & vbCrLf
        End If
        If TxtPassword1.Text.Trim() <> TxtPassword2.Text.Trim() Then
            message += ". Le mot de passe saisie est différent de la reSaisie " & vbCrLf
        End If

        Return message
    End Function

    Private Sub DropDownSiege_SelectedIndexChanged(sender As Object, e As Data.PositionChangedEventArgs) Handles DropDownSiege.SelectedIndexChanged
        Dim dropDown = TryCast(sender, RadDropDownList)
        If dropDown.SelectedItem IsNot Nothing Then
            initUniteSanitaire(TryCast(dropDown.SelectedItem.Value, Siege).SiegeId)
        End If
    End Sub

    Private Sub DropDownUS_SelectedIndexChanged(sender As Object, e As Data.PositionChangedEventArgs) Handles DropDownUS.SelectedIndexChanged
        Dim dropDown = TryCast(sender, RadDropDownList)
        If dropDown.SelectedItem IsNot Nothing Then
            initSite(TryCast(dropDown.SelectedItem.Value, UniteSanitaire).Oa_unite_sanitaire_id)
        End If

    End Sub

    Private Sub BtnValider_Click(sender As Object, e As EventArgs) Handles BtnValider.Click
        Dim message = ctrlFields()
        If message <> "" Then
            MsgBox(message, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Formulaire incorrectement renseigné")

            Exit Sub
            End If
    End Sub
End Class


