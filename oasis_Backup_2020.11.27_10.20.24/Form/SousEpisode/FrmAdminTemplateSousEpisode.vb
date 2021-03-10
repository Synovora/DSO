Imports System.IO
Imports Oasis_Common
Imports Telerik.WinControls.UI
Imports Telerik.WinForms.Documents.FormatProviders.OpenXml.Docx
Imports Telerik.WinForms.Documents.Model

Public Class FrmAdminTemplateSousEpisode
    Dim sousEpisodeTypeDao As SousEpisodeTypeDao = New SousEpisodeTypeDao
    Dim sousEpisodeSousTypeDao As SousEpisodeSousTypeDao = New SousEpisodeSousTypeDao
    Dim sousEpisodeSousSousTypeDao As SousEpisodeSousSousTypeDao = New SousEpisodeSousSousTypeDao
    Dim lstSousEpisodeType As List(Of SousEpisodeType) = New List(Of SousEpisodeType)
    Dim lstSousEpisodeSousType As List(Of SousEpisodeSousType) = New List(Of SousEpisodeSousType)

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        ' -- listes de references
        lstSousEpisodeType = sousEpisodeTypeDao.getLstSousEpisodeType()
        lstSousEpisodeSousType = sousEpisodeSousTypeDao.getLstSousEpisodeSousType()
        InitOneShot()

    End Sub

    Private Sub RadButton1_Click(sender As Object, e As EventArgs) Handles RadButton1.Click
        If Me.DropDownSousType.SelectedItem Is Nothing Then Return
        Dim sousType = (TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType))

        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            Dim lstSousEpisodeFusion As List(Of SousEpisodeFusion) = constitueFusion()
            Dim tbl = Telecharger_model(sousType)
            Dim ins = New MemoryStream(tbl)
            Dim provider = New DocxFormatProvider()

            Using frm = New FrmAdminTemplateDocx(sousType)
                If tbl.Count > 0 Then
                    frm.RadRichTextEditor1.Document = provider.Import(ins)
                End If
                frm.RadRichTextEditor1.Document.MailMergeDataSource.ItemsSource = lstSousEpisodeFusion
                frm.RadRichTextEditor1.UpdateAllFields(FieldDisplayMode.DisplayName)

                'Dim merged = frm.RadRichTextEditor1.MailMerge()
                'frm.RadRichTextEditor1.Document = merged
                ins.Dispose()
                tbl = Nothing
                Application.DoEvents()
                'frm.ReplaceAllMatches("toto", "titi")
                frm.ShowDialog()
            End Using

        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
        Exit Sub

    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DropDownType_SelectedIndexChanged(sender As Object, e As Data.PositionChangedEventArgs) Handles DropDownType.SelectedIndexChanged
        If Me.DropDownType.SelectedItem IsNot Nothing Then
            initSousTypes(TryCast(Me.DropDownType.SelectedItem.Value, SousEpisodeType).Id)
        End If
    End Sub

    Private Function Telecharger_model(sousEpisodeSousType As SousEpisodeSousType) As Byte()
        Dim tbl As Byte() = {}
        Try
            tbl = sousEpisodeSousType.getContenuModel(loginRequestLog)
            Return tbl
        Catch err As Exception
            If err IsNot Nothing AndAlso err.Message IsNot Nothing AndAlso err.Message.Contains("Fichier demandé inexistant") Then
                Return tbl
            End If
            Throw err
        End Try
    End Function

    Private Sub InitOneShot()

        Me.DropDownType.Items.Clear()
        For Each sousEpisodeType As SousEpisodeType In lstSousEpisodeType
            Dim radListItem As New RadListDataItem(sousEpisodeType.Libelle, sousEpisodeType)
            Me.DropDownType.Items.Add(radListItem)
        Next
        If Me.DropDownType.Items.Count > 0 Then
            Me.DropDownType.DefaultItemsCountInDropDown = DropDownType.Items.Count
            Me.DropDownType.SelectedItem = Me.DropDownType.Items(0)
            'initSousTypes(TryCast(Me.DropDownType.SelectedItem.Value, SousEpisodeType).Id)
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
            Me.DropDownSousType.DefaultItemsCountInDropDown = DropDownSousType.Items.Count
            Me.DropDownSousType.SelectedItem = Me.DropDownSousType.Items(0)
        End If

        Me.DropDownSousType.Enabled = True

    End Sub

    Private Function constitueFusion() As List(Of SousEpisodeFusion)
        Dim lstFusion = New List(Of SousEpisodeFusion)(1)
        Dim sousEF As New SousEpisodeFusion
        With sousEF
            .USNom = "Nom de l'unité sanitaire"
            .USAdr1 = "Adresse 1 de l'U.S."
            .USAdr2 = "Adresse 2 de l'U.S."
            .USCP = "64100"
            .USVille = "Ville de l'U.S."
            .USTel = "tel U.S."
            .USFax = "fax U.S."
            .USEmail = "email U.S."

            .Commentaire = "Ex. de commentaire sur 3 lignes" & vbCrLf & "Ligne 2" & vbCrLf & "Ligne 3"

            .SiteNom = "Nom du site"
            .SiteAdr1 = "adresse site ligne 1"
            .SiteAdr2 = "adresse site ligne 2"
            .SiteCP = "64200"
            .SiteVille = "Ville du site"
            .SiteTel = "tel du site"
            .SiteFax = "fax du site"
            .SiteEmail = "email du site"

            .IntervenantNom = "Prenom Nom Intervenant"
            .IntervenantSpecialite = "Spécialité intervenant"
            .IntervenantStructure = "Structure Intervenant"

            .Patient_PrenomNom = "Charles-Henri Dupond de la Motte"
            .Patient_Date_Naissance = "20-02-1998"
            .Patient_Age = "22 ans"
            .Patient_NIR = "1980264000000"
            .Type_Libelle = Me.DropDownType.SelectedItem.Text
            .Sous_Type_Libelle = Me.DropDownSousType.SelectedItem.Text

            .Signature_Date = Date.Now.ToString("dd MMM yyyy")
            .Signataire_Fonction = userLog.UtilisateurProfilId.ToLower.Trim.Replace("_", " ")
            .Signataire_PrenomNom = userLog.UtilisateurPrenom.Trim & " " & userLog.UtilisateurNom.Trim

            .Signataire_Fonction_ALD = .Signataire_Fonction
            .Signataire_PrenomNom_ALD = .Signataire_PrenomNom

            .Patient_sexe = "masculin"
            .Patient_Poids = "Poids 72,5 kg"
            .Patient_Dextro = "Dextro : 1,23 g/l"
            .Patient_FC = "FC : 88 /min"
            .Patient_SAT = "Saturation : 100 %"
            .Patient_PAS = "120"
            .Patient_PAD = "70"
            .Episode_DateHeure = Date.Now.ToString("dd MMMM yyyy à hh:mm")

            .Antecedent = "Ex. d'antécédents sur 3 lignes" & vbCrLf & "Ligne 2" & vbCrLf & "Ligne 3"
            .Contexte = "Ex. de contexte sur 3 lignes" & vbCrLf & "Ligne 2" & vbCrLf & "Ligne 3"
            .Traitement = "Ex. de traitement sur 3 lignes" & vbCrLf & "Ligne 2" & vbCrLf & "Ligne 3"

            Dim sousType = TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType)
            .ALD_Sans_FaireFaire = sousType.Commentaire
            .ALD_Avec_FaireFaire = sousType.Commentaire

            ' -- recherche des sous-type_detail (sousoustype)
            Dim lstSousTypeDetail = sousEpisodeSousSousTypeDao.getLstSousEpisodeSousSousType(sousType.Id)
            Dim isSautLigneAvant = lstSousTypeDetail.Count <= 5
            If lstSousTypeDetail.Count > 0 Then
                For Each detail As SousEpisodeSousSousType In lstSousTypeDetail
                    If isSautLigneAvant Then .Sous_Type_Libelle_Detail_ALD += vbCrLf
                    .Sous_Type_Libelle_Detail_ALD += (detail.Libelle & vbCrLf)
                    If isSautLigneAvant Then .Sous_Type_Libelle_Detail_commentaire_ALD += vbCrLf
                    .Sous_Type_Libelle_Detail_commentaire_ALD += (detail.Commentaire & vbCrLf)

                    If isSautLigneAvant Then .Sous_Type_Libelle_Detail_Non_ALD += vbCrLf
                    .Sous_Type_Libelle_Detail_Non_ALD += (detail.Libelle & vbCrLf)
                    If isSautLigneAvant Then .Sous_Type_Libelle_Detail_commentaire_non_ALD += vbCrLf
                    .Sous_Type_Libelle_Detail_commentaire_non_ALD += (detail.Commentaire & vbCrLf)
                Next
            End If
        End With

        lstFusion.Add(sousEF)

        Return lstFusion
    End Function

End Class
