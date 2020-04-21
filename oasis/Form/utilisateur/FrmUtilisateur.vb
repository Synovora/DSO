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

    Dim isNoChangePassword As Boolean
    Dim userDao As UserDao = New UserDao

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
        If isCreation = False Then
            If utilisateur.UtilisateurUniteSanitaireId <> 0 Then
                Dim uniteS = (New UniteSanitaireDao()).getUniteSanitaireById(utilisateur.UtilisateurUniteSanitaireId, True)
                idSiegeDefaut = uniteS.Oa_unite_sanitaire_siege_id
            End If
            Me.RadGroupPassword.Text += " (laisser les zones de saisie vides si vous ne souhaitez pas les changer)"
        End If
        Me.TxtIdentifiant.Text = utilisateur.UtilisateurLogin
        Me.TxtNom.Text = utilisateur.UtilisateurNom
        Me.TxtPrenom.Text = utilisateur.UtilisateurPrenom
        Me.TxtTelephone.Text = utilisateur.UtilisateurTelephone
        Me.TxtMail.Text = utilisateur.UtilisateurMail
        Me.ChkAdmin.Checked = utilisateur.UtilisateurAdmin
        Me.TxtRPPS.Text = utilisateur.UtilisateurRPPS

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
        If isCreation OrElse Me.DropDownSiege.SelectedItem Is Nothing Then
            If DropDownSiege.Items.Count > 0 Then Me.DropDownSiege.SelectedItem = Me.DropDownSiege.Items(0)
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
        If isCreation OrElse Me.DropDownUS.SelectedItem Is Nothing Then
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
        If isCreation OrElse Me.DropDownSite.SelectedItem Is Nothing Then
            Me.DropDownSite.SelectedItem = Me.DropDownSite.Items(0)
        End If
        Me.DropDownSite.DefaultItemsCountInDropDown = DropDownSite.Items.Count

    End Sub

    Private Function ctrlFields() As String
        Dim message = ""
        If TxtIdentifiant.Text.Length < 5 Then
            message += ". L'identifiant doit faire au moins 5 caractères" & vbCrLf
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
        ' -- si profil medical ou paramedical => no RPPS oblgatoire
        If isProfilMedicalOrParamedical() Then
            If TxtRPPS.Text.Trim() = "" Then
                message += ". Le No RPPS est obligatoire" & vbCrLf
            ElseIf TxtRPPS.Text.Length <> 11 Then
                message += ". Le No RPPS doit avoir 11 chiffres" & vbCrLf
            End If
        End If
        ' -- gestion ctrl pwd
        isNoChangePassword = (isCreation = False AndAlso TxtPassword1.Text.Trim() = "" AndAlso TxtPassword2.Text.Trim() = "")
        If isNoChangePassword = False Then
            If isValidePassword(TxtPassword1.Text.Trim()) = False Then
                message += ". Le mot de passe doit faire au moins 8 caractères et comprendre au moins une majuscule, une minuscule, un chiffre et un caractère spécial" & vbCrLf
            End If
            If TxtPassword1.Text.Trim() <> TxtPassword2.Text.Trim() Then
                message += ". Le mot de passe saisie est différent de la reSaisie " & vbCrLf
            End If
        End If

        Return message
    End Function

    Private Function isProfilMedicalOrParamedical() As Boolean
        Return isProfilMedicalOrParamedical(DirectCast(DropDownProfil.SelectedItem.Value, Profil).Type)
    End Function

    Private Function isProfilMedicalOrParamedical(userProfilType As String) As Boolean
        Return (userProfilType = ProfilDao.EnumProfilType.MEDICAL.ToString OrElse userProfilType = ProfilDao.EnumProfilType.PARAMEDICAL.ToString)
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
        ' -- controle des champs de saisie
        Dim message = ctrlFields()
        If message <> "" Then
            MsgBox(message, MsgBoxStyle.OkOnly Or MsgBoxStyle.Exclamation, "Formulaire incorrectement renseigné")
            Exit Sub
        End If
        ' --- fill du bean
        With utilisateur
            .UtilisateurAdmin = ChkAdmin.Checked
            .UtilisateurLogin = TxtIdentifiant.Text.Trim()
            .UtilisateurNom = TxtNom.Text.Trim()
            .UtilisateurPrenom = TxtPrenom.Text.Trim()
            .UtilisateurMail = TxtMail.Text.Trim().ToLower
            .UtilisateurTelephone = TxtTelephone.Text.Trim()
            .UtilisateurRPPS = TxtRPPS.Text.Trim()
            .UtilisateurProfilId = DirectCast(Me.DropDownProfil.SelectedItem.Value, Profil).Id
            .UtilisateurSiteId = DirectCast(Me.DropDownSite.SelectedItem.Value, Site).Oa_site_id
            .UtilisateurUniteSanitaireId = DirectCast(Me.DropDownUS.SelectedItem.Value, UniteSanitaire).Oa_unite_sanitaire_id
            .UtilisateurSiegeId = DirectCast(Me.DropDownSiege.SelectedItem.Value, Siege).SiegeId
            .FonctionParDefautId = DirectCast(Me.DropDownProfil.SelectedItem.Value, Profil).FonctionParDefautId
            If isNoChangePassword = False Then
                .Password = Utilisateur.CryptePwd(.UtilisateurLogin, TxtPassword1.Text.Trim)
                .IsPasswordUniqueUsage = True
            End If
            .UtilisateurRPPS = If(isProfilMedicalOrParamedical(), TxtRPPS.Text, "")
        End With
        ' --- enregistrement
        If isCreation Then
            If userDao.Create(utilisateur) Then
                Notification.show("Création Utilisateur", "Utilisateur créé avec succès !", 1)
                Me.Tag = utilisateur.UtilisateurId
                Close()
            End If
        Else
            If userDao.UpdateSansChangerEtatEtDates(utilisateur) Then
                Notification.show("Modification Utilisateur", "Utilisateur modifié avec succès !", 1)
                Me.Tag = utilisateur.UtilisateurId
                Close()
            End If
        End If
    End Sub

    Private Sub TxtRPPS_KeyPress(sender As Object, e As KeyPressEventArgs) Handles TxtRPPS.KeyPress
        If e.KeyChar <> Chr(8) AndAlso (e.KeyChar < Chr(48) OrElse e.KeyChar > Chr(57)) Then
            e.KeyChar = Chr(0)
        End If
    End Sub

    Private Sub DropDownProfil_SelectedIndexChanged(sender As Object, e As Data.PositionChangedEventArgs) Handles DropDownProfil.SelectedIndexChanged
        Me.TxtRPPS.Visible = isProfilMedicalOrParamedical()
    End Sub
End Class


