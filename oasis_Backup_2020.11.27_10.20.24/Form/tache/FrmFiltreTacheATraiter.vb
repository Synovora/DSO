

Imports System.ComponentModel
Imports Oasis_Common
Imports Telerik.WinControls.Enumerations
Imports Telerik.WinControls.UI

Public Class FrmFiltreTacheATraiter
    Private filtreTacheOrigine As FiltreTache
    Private isValidation As Boolean = False
    Public filtreTacheNouveau As FiltreTache

    Sub New(ByRef _filterTache As FiltreTache)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        filtreTacheNouveau = New FiltreTache
        filtreTacheOrigine = cloneFiltre(_filterTache)
        initWithFiltreTache()

    End Sub

    Private Function cloneFiltre(filtreTache As FiltreTache) As FiltreTache
        Dim filtreClone = New FiltreTache
        For Each us In filtreTache.LstUniteSanitaire
            ' - clone de 1er niveau
            Dim usanit As UniteSanitaire = us.Clone
            filtreClone.AddUniteSanitaire(usanit)
            ' -- clone de la liste des site selectionnés
            usanit.LstSite = New List(Of Site)
            For Each sitelu In us.LstSite
                usanit.AddSite(sitelu)
            Next
        Next
        Return filtreClone
    End Function

    Private Sub initWithFiltreTache()
        Dim uniteSanitaireDao = New UniteSanitaireDao
        Dim siteDao As New SiteDao
        Dim lstUS As List(Of UniteSanitaire)
        Dim lstUniteSite As New BindingList(Of ItemUniteSite)
        Dim item As ItemUniteSite
        Dim index As Integer = 0, indexUS As Integer

        ' --- active le  mode trois etats
        Me.RadTreeView1.TriStateMode = True
        Me.RadTreeView1.AutoCheckChildNodes = True

        lstUS = uniteSanitaireDao.getList(False)
        For Each uniteSanitaire In lstUS
            index += 1
            indexUS = index
            uniteSanitaire.LstSite = siteDao.getList(False, uniteSanitaire.Oa_unite_sanitaire_id)

            item = New ItemUniteSite(uniteSanitaire, uniteSanitaire.Oa_unite_sanitaire_description, isUniteSanitaireChecked(uniteSanitaire), 0, index)
            lstUniteSite.Add(item)
            For Each sitelu In uniteSanitaire.LstSite
                index += 1
                item = New ItemUniteSite(sitelu, sitelu.Oa_site_description, isSiteChecked(uniteSanitaire, sitelu), indexUS, index)
                lstUniteSite.Add(item)
            Next
        Next


        Me.RadTreeView1.DisplayMember = "Name"
        Me.RadTreeView1.ChildMember = "Id"
        Me.RadTreeView1.ParentMember = "ParentId"
        Me.RadTreeView1.CheckedMember = "IsActive"
        RadTreeView1.DataSource = lstUniteSite

        RadTextBox1.Text = filtreTacheOrigine.ResumeFiltre
    End Sub

    Private Function isUniteSanitaireChecked(uniteS As UniteSanitaire) As ToggleState
        For Each us In filtreTacheOrigine.LstUniteSanitaire
            'If us.LstSite.Count > 0 Then Return ToggleState.Indeterminate
            If us.Oa_unite_sanitaire_id = uniteS.Oa_unite_sanitaire_id Then Return ToggleState.On
        Next
        Return ToggleState.Off
    End Function

    Private Function isSiteChecked(uniteSanitaire As UniteSanitaire, site As Site) As ToggleState
        For Each us In filtreTacheOrigine.LstUniteSanitaire
            If us.Oa_unite_sanitaire_id <> uniteSanitaire.Oa_unite_sanitaire_id Then Continue For
            For Each sitelu In us.LstSite
                If sitelu.Oa_site_id = site.Oa_site_id Then Return ToggleState.On
            Next
        Next
        Return ToggleState.Off
    End Function

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub construitFiltreNouveau()
        Dim filtreTache As New FiltreTache
        Dim usParent As UniteSanitaire = Nothing
        Dim itemUS As ItemUniteSite

        ' -- on depile les unite sanitaires
        For Each node In RadTreeView1.Nodes
            itemUS = DirectCast(node.DataBoundItem, ItemUniteSite)
            usParent = DirectCast(itemUS.UniteSanitaireOuSite1, UniteSanitaire)

            If itemUS.IsActive <> ToggleState.Off Then ' indeterminate ossible que sur les unites sanitaire
                filtreTache.AddUniteSanitaire(usParent)
                usParent.LstSite.Clear()

                ' --- on depile les sites
                For Each nodeS In node.Nodes
                    itemUS = DirectCast(nodeS.DataBoundItem, ItemUniteSite)
                    If itemUS.IsActive <> ToggleState.Off Then
                        filtreTache.AddSiteToUniteSanitaire(usParent, DirectCast(itemUS.UniteSanitaireOuSite1, Site))
                    End If
                Next
            End If
        Next

        filtreTacheNouveau = filtreTache

    End Sub

    Private Sub BtnValidate_Click(sender As Object, e As EventArgs) Handles BtnValidate.Click
        construitFiltreNouveau()
        isValidation = True
        Me.Close()
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        filtreTacheNouveau = Nothing
    End Sub

    Private Sub RadTreeView1_NodeCheckedChanged(sender As Object, e As TreeNodeCheckedEventArgs) Handles RadTreeView1.NodeCheckedChanged
        construitFiltreNouveau()
        Me.RadTextBox1.Text = filtreTacheNouveau.ResumeFiltre
    End Sub

    Private Sub FrmFiltreTacheATraiter_FormClosing(sender As Object, e As FormClosingEventArgs) Handles MyBase.FormClosing
        If isValidation = False Then
            filtreTacheNouveau = Nothing
        End If
    End Sub
End Class
