Imports System.IO
Imports Telerik.WinControls.UI
Imports Telerik.WinForms.Documents.FormatProviders.OpenXml.Docx
Imports Telerik.WinForms.Documents.Model

Public Class FrmAdminTemplateSousEpisode
    Dim sousEpisodeTypeDao As SousEpisodeTypeDao = New SousEpisodeTypeDao
    Dim sousEpisodeSousTypeDao As SousEpisodeSousTypeDao = New SousEpisodeSousTypeDao
    Dim lstSousEpisodeType As List(Of SousEpisodeType) = New List(Of SousEpisodeType)
    Dim lstSousEpisodeSousType As List(Of SousEpisodeSousType) = New List(Of SousEpisodeSousType)

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        ' -- listes de references
        lstSousEpisodeType = sousEpisodeTypeDao.getLstSousEpisodeType()
        lstSousEpisodeSousType = sousEpisodeSousTypeDao.getLstSousEpisodeSousType()
        initOneShot()

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DropDownType_SelectedIndexChanged(sender As Object, e As Data.PositionChangedEventArgs) Handles DropDownType.SelectedIndexChanged
        initSousTypes(lstSousEpisodeType(e.Position).Id)
    End Sub

    Private Sub initOneShot()

        Me.DropDownType.Items.Clear()
        For Each sousEpisodeType As SousEpisodeType In lstSousEpisodeType
            Dim radListItem As New RadListDataItem(sousEpisodeType.Libelle, sousEpisodeType)
            Me.DropDownType.Items.Add(radListItem)
        Next
        If Me.DropDownType.Items.Count > 0 Then
            Me.DropDownType.SelectedItem = Me.DropDownType.Items(0)
            initSousTypes(TryCast(Me.DropDownType.SelectedItem.Value, SousEpisodeType).Id)
        End If


    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idType"></param>
    Private Sub initSousTypes(idType As Long)
        Me.DropDownSousType.Items.Clear()

        For Each sousEpisodeSousType As SousEpisodeSousType In lstSousEpisodeSousType
            If sousEpisodeSousType.IdSousEpisodeType <> idType Then Continue For ' pas pour ce type d'episode

            Dim radListItemST As New RadListDataItem(sousEpisodeSousType.Libelle, sousEpisodeSousType)
            Me.DropDownSousType.Items.Add(radListItemST)
        Next
        If DropDownSousType.SelectedItem Is Nothing AndAlso DropDownSousType.Items.Count > 0 Then
            Me.DropDownSousType.SelectedItem = Me.DropDownSousType.Items(0)
        End If

        Me.DropDownSousType.Enabled = True

    End Sub

    Private Function constitueFusion() As List(Of SousEpisodeFusion)
        Dim lstFusion = New List(Of SousEpisodeFusion)(1)
        Dim sousEF As New SousEpisodeFusion With {
           .USNom = "Nom de l'unité sanitaire modifiée",
           .USAdr1 = "Maison Xori Lur",
           .USAdr2 = "154 allée Hégui Eder",
           .USCP = "64990",
           .USVille = "Mouguerre",
           .Commentaire = "Mon commentaire sur 3 ligne" & vbCrLf & "Ligne 2" & vbCrLf & "Ligne 3",
           .SiteNom = "Nom du site",
           .SiteAdr1 = "adresse site ligne 1",
           .SiteAdr2 = "adresse site ligne 2",
           .SiteCP = "64200",
           .SiteVille = "Ville du site",
           .SiteTel = "tel du site",
           .SiteFax = "tel du site",
           .SiteEmail = "tel du site",
           .Patient_PrenomNom = "Charles-Henri Dupond de la Motte",
           .Patient_Date_Naissance = "20-02-1998",
           .Patient_Age = "22 ans et 1 mois",
           .Patient_NIR = "1980264000000",
           .Patient_Poids = "72,5",
           .Type_Libelle = Me.DropDownType.SelectedItem.Text
        }


        lstFusion.Add(sousEF)

        Return lstFusion
    End Function

    Private Sub RadButton1_Click(sender As Object, e As EventArgs) Handles RadButton1.Click
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            Dim lstSousEpisodeFusion As List(Of SousEpisodeFusion) = constitueFusion()

            Using frm = New FrmAdminTemplateDocx
                Dim tbl = File.ReadAllBytes("c:\db\oasis\modeleradiologie.docx")
                Dim ins = New MemoryStream(tbl)
                Dim provider = New DocxFormatProvider()
                frm.RadRichTextEditor1.Document = provider.Import(ins)
                frm.RadRichTextEditor1.Document.MailMergeDataSource.ItemsSource = lstSousEpisodeFusion
                'frm.RadRichTextEditor1.UpdateAllFields(FieldDisplayMode.Result)
                'Dim merged = frm.RadRichTextEditor1.MailMerge()
                'frm.RadRichTextEditor1.Document = merged
                ins.Dispose()
                tbl = Nothing
                frm.ShowDialog()
                frm.Dispose()
            End Using

        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try


    End Sub
End Class
