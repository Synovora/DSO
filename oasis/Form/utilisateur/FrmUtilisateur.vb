Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class FrmUtilisateur
    Dim utilisateur As Utilisateur
    Dim lstProfil As List(Of Profil)
    Dim isCreation As Boolean

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
        lstProfil = (New ProfilDao()).getListProfil()

        ' -- combo type
        Me.DropDownProfil.Items.Clear()
        For Each profil As Profil In lstProfil
            Dim radListItem As New RadListDataItem(profil.Designation, profil)
            Me.DropDownProfil.Items.Add(radListItem)
            'If TryCast(radListItem.Value, Profil).Id = Me.utilisateur.UtilisateurProfilId Then
            If profil.Id = Me.utilisateur.UtilisateurProfilId Then
                radListItem.Selected = True
                ' -- init des sous types
                'initSousTypes(profil.Id)
            End If
        Next
        If isCreation AndAlso Me.DropDownProfil.Items.Count > 0 Then
            Me.DropDownProfil.SelectedItem = Me.DropDownProfil.Items(0)
            'initSousTypes(TryCast(Me.DropDownProfil.SelectedItem.Value, Profil).Id)
        End If
        Me.DropDownProfil.DefaultItemsCountInDropDown = DropDownProfil.Items.Count

    End Sub
End Class
