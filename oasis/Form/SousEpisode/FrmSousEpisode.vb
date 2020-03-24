Imports System.Configuration
Imports System.IO
Imports Oasis_Common
Imports Telerik.WinControls.Enumerations
Imports Telerik.WinControls.UI
Imports Telerik.WinForms.Documents.FormatProviders.OpenXml.Docx
Imports Telerik.WinForms.Documents.Model

Public Class FrmSousEpisode

    Dim sousEpisodeDao As SousEpisodeDao = New SousEpisodeDao
    Dim sousEpisodeTypeDao As SousEpisodeTypeDao = New SousEpisodeTypeDao
    Dim sousEpisodeSousTypeDao As SousEpisodeSousTypeDao = New SousEpisodeSousTypeDao
    Dim sousEpisodeSousSousTypeDao As SousEpisodeSousSousTypeDao = New SousEpisodeSousSousTypeDao
    Dim sousEpisodeReponseDao As SousEpisodeReponseDao = New SousEpisodeReponseDao

    Dim episode As Episode, patient As Patient, sousEpisode As SousEpisode
    Dim userCreateNom As String, userUpdateNom As String, userValidateNom As String
    Dim isCreation As Boolean
    Dim isNotValidate As Boolean
    Dim isPatientALD As Boolean

    Dim lstSousEpisodeType As List(Of SousEpisodeType) = New List(Of SousEpisodeType)
    Dim lstSousEpisodeSousType As List(Of SousEpisodeSousType) = New List(Of SousEpisodeSousType)
    Dim lstSousEpisodeSousSousType As List(Of SousEpisodeSousSousType) = New List(Of SousEpisodeSousSousType)

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="episode"></param>
    ''' <param name="patient"></param>
    ''' <param name="sousEpisode"></param>
    ''' <param name="userCreateNom"></param>
    ''' <param name="userUpdateNom"></param>
    ''' <param name="userValidateNom"></param>
    Sub New(episode As Episode, patient As Patient, sousEpisode As SousEpisode, userCreateNom As String, userUpdateNom As String, userValidateNom As String)

        ' -- Cet appel est requis par le concepteur.
        InitializeComponent()

        ' -- Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        afficheTitleForm(Me, Me.Text)

        ' -- episode en cours
        Me.episode = episode
        Me.patient = patient
        Me.sousEpisode = sousEpisode
        Me.userCreateNom = userCreateNom
        Me.userUpdateNom = userUpdateNom
        Me.userValidateNom = userValidateNom

        ' -- initialisation des controles du formulaire
        initOneShot()
        initControls()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DropDownType_SelectedIndexChanged(sender As Object, e As Data.PositionChangedEventArgs) Handles DropDownType.SelectedIndexChanged
        If Me.DropDownType.SelectedItem IsNot Nothing Then
            initSousTypes(TryCast(Me.DropDownType.SelectedItem.Value, SousEpisodeType).Id)
            'initSousTypes(lstSousEpisodeType(e.Position).Id)
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DropDownSousType_SelectedIndexChanged(sender As Object, e As Data.PositionChangedEventArgs) Handles DropDownSousType.SelectedIndexChanged
        If Me.DropDownSousType.SelectedItem IsNot Nothing Then
            initSousSousTypes(TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType).Id)
            setDefaultALDGrid()
            If isCreation Then
                Dim sousEpisodeSousType As SousEpisodeSousType = TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType)
                TxtDelai.Value = Coalesce(sousEpisodeSousType.DelaiReponse, "")
                ChkBReponseAttendue.Checked = sousEpisodeSousType.IsReponseRequise
            End If
            initALDetReponse()

        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    Private Sub ChkALD_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles ChkALD.ToggleStateChanged
        setDefaultALDGrid()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="args"></param>
    Private Sub ChkBReponseAttendue_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles ChkBReponseAttendue.ToggleStateChanged
        Dim visible = (args.ToggleState = ToggleState.On)
        TxtDelai.Visible = visible
        LblDelai.Visible = visible
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnAjoutReponse_Click(sender As Object, e As EventArgs) Handles BtnAjoutReponse.Click
        Dim dr As DialogResult = OpenFileDialog1.ShowDialog()

        If dr = System.Windows.Forms.DialogResult.OK Then
            Dim fileName As String = OpenFileDialog1.FileName
            Dim comment = RadInputBox.Show("Fichier : " & fileName & vbCrLf & "Saisissez votre commentaire", "Introduction de document reçu", "#CANCEL#")

            If comment = "#CANCEL#" Then
                Notification.show("Ajout document", "Ajout annulé !")
                Return
            End If
            Try
                Me.Cursor = Cursors.WaitCursor
                Dim sousEpisodeReponse As SousEpisodeReponse = New SousEpisodeReponse
                With sousEpisodeReponse
                    .IdSousEpisode = sousEpisode.Id
                    .CreateUserId = userLog.UtilisateurId
                    .HorodateCreation = DateTime.Now
                    .NomFichier = Path.GetFileName(fileName)
                    .Commentaire = comment
                End With
                If sousEpisodeReponseDao.Create(sousEpisode, sousEpisodeReponse, fileName) = False Then
                    Notification.show("Ajout document", "ERREUR insertion du nouveau document !!!")
                Else
                    Notification.show("Ajout document", "Ajout terminée avec succès !")
                    refreshGrid()
                End If
            Finally
                Me.Cursor = Cursors.Default
            End Try
        End If
    End Sub


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub gridReponse_CommandCellClick(sender As Object, e As GridViewCellEventArgs)
        Dim gce As GridCommandCellElement = (TryCast(sender, GridCommandCellElement))
        Select Case gce.ColumnInfo.Name.ToLower
            Case "telecharger"
                TelechargerReponse(gce)
            Case "supprimer"
                supprimer(gce)
        End Select
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnValidate_Click(sender As Object, e As EventArgs) Handles BtnValidate.Click
        serializeSousEpisode(False)
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnValideAndSign_Click(sender As Object, e As EventArgs) Handles BtnValideAndSign.Click
        If isCreation Then
            serializeSousEpisode(True)
        Else
            sousEpisodeDao.updateValidation(Nothing, sousEpisode.Id, Nothing)
            sousEpisode.ValidateUserId = userLog.UtilisateurId
            sousEpisode.HorodateValidate = Date.Now
            userValidateNom = userLog.UtilisateurPrenom + " " + userLog.UtilisateurNom
            initControls()
        End If
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="gce"></param>
    Private Sub TelechargerReponse(gce As GridCommandCellElement)
        'MessageBox.Show("Telecharger fichier " & gce.RowInfo.Cells("NomFichier").Value & " : " & gce.RowInfo.Cells("IdSousEpisode").Value & "_" & gce.RowInfo.Cells("Id").Value)
        Dim sousEpisodeReponse As SousEpisodeReponse
        Try
            Me.Cursor = Cursors.WaitCursor
            sousEpisodeReponse = sousEpisodeReponseDao.getById(gce.RowInfo.Cells("Id").Value)

            Dim tbl As Byte() = sousEpisodeReponseDao.getContenu(episode.Id, sousEpisodeReponse)
            'Me.Cursor = Cursors.Default
            'SaveFileDialog1.FileName = sousEpisodeReponse.NomFichier
            'Select Case (SaveFileDialog1.ShowDialog())
            '    Case DialogResult.Abort, DialogResult.Cancel
            '        Notification.show("Réponse Sous-épisode", "Téléchargement abandonné !")
            '    Case DialogResult.OK, DialogResult
            '        File.WriteAllBytes(SaveFileDialog1.FileName, tbl)
            '        Notification.show("Réponse Sous-épisode", "Téléchargement de " & SaveFileDialog1.FileName & " Terminé !")
            'End Select

            Dim pathDownload = ConfigurationManager.AppSettings("CheminTelechargement")
            If (Not System.IO.Directory.Exists(pathDownload)) Then
                System.IO.Directory.CreateDirectory(pathDownload)
            End If

            File.WriteAllBytes(pathDownload & "\" & sousEpisodeReponse.NomFichier, tbl)
            Dim proc As New Process()
            ' Nom du fichier dont l'extension est connue du shell à ouvrir 
            Try
                proc.StartInfo.FileName = pathDownload & "\" & sousEpisodeReponse.NomFichier
                proc.Start()
                ' On libère les ressources 
                proc.Close()
                Notification.show("Lancement du logiciel associé", "Veuillez patienter pendant le lancement du logiciel associé à la visualisation de votre fichier !")
            Catch err As Exception
                MsgBox(err.Message() & vbCrLf & "Votre fichier est téléchargé et disponible dans le répertoire suivant : " & vbCrLf & pathDownload)
            End Try
        Catch Err As Exception
            MsgBox(Err.Message())
            Return
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub supprimer(gce As GridCommandCellElement)
        'MessageBox.Show("Telecharger fichier " & gce.RowInfo.Cells("NomFichier").Value & " : " & gce.RowInfo.Cells("IdSousEpisode").Value & "_" & gce.RowInfo.Cells("Id").Value)
        If MsgBox("Etes-vous sur de vouloir supprimer ce fichier ?", MsgBoxStyle.YesNo Or MsgBoxStyle.DefaultButton2 Or MsgBoxStyle.Critical, "Suppression") = MsgBoxResult.Yes Then
            Dim isDernier As Boolean = Me.RadReponseGrid.Rows.Count < 2
            sousEpisodeReponseDao.delete(sousEpisode, gce.RowInfo.Cells("Id").Value, isDernier)
            refreshGrid()
        End If


    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub setDefaultALDGrid()
        If isCreation Then
            Dim sousType As SousEpisodeSousType = TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType)
            Dim isALD = sousType.IsALDPossible AndAlso isPatientALD AndAlso ChkALD.Checked
            For Each row In RadSousSousTypeGrid.Rows
                row.Cells("ChkALD").Value = isALD
            Next
        End If

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub initOneShot()
        '  -- somme nous en mode creation (sinon mode update)
        isCreation = If(sousEpisode.Id = 0, True, False)
        ' -- le patient est il en ALD
        Dim aldDO = New AldDao()
        isPatientALD = aldDO.IsPatientALD(patient.patientId)

        ' -- init des details du bean SousEpisode
        If Not isCreation Then
            Dim sousEpisodeDetailSousTypeDao As SousEpisodeDetailSousTypeDao = New SousEpisodeDetailSousTypeDao
            sousEpisode.lstDetail = sousEpisodeDetailSousTypeDao.getLstSousEpisodeDetailSousType(sousEpisode.Id)
        End If

        ' -- listes de references
        lstSousEpisodeType = sousEpisodeTypeDao.getLstSousEpisodeType()
        lstSousEpisodeSousType = sousEpisodeSousTypeDao.getLstSousEpisodeSousType()
        lstSousEpisodeSousSousType = sousEpisodeSousSousTypeDao.getLstSousEpisodeSousSousType()
        With sousEpisode
            Me.DropDownType.Items.Clear()
            For Each sousEpisodeType As SousEpisodeType In lstSousEpisodeType
                If filtreTypeByProfil(sousEpisodeType) Then Continue For
                Dim radListItem As New RadListDataItem(sousEpisodeType.Libelle, sousEpisodeType)
                Me.DropDownType.Items.Add(radListItem)
                'If TryCast(radListItem.Value, SousEpisodeType).Id = Me.sousEpisode.IdSousEpisodeType Then
                If sousEpisodeType.Id = Me.sousEpisode.IdSousEpisodeType Then
                    radListItem.Selected = True
                    ' -- init des sous types
                    initSousTypes(sousEpisodeType.Id)
                End If
            Next
            If isCreation AndAlso Me.DropDownType.Items.Count > 0 Then
                Me.DropDownType.SelectedItem = Me.DropDownType.Items(0)
                initSousTypes(TryCast(Me.DropDownType.SelectedItem.Value, SousEpisodeType).Id)
            End If
            Me.DropDownType.DefaultItemsCountInDropDown = DropDownType.Items.Count

        End With

        '-- handler sur boutons grid reponse
        AddHandler RadReponseGrid.CommandCellClick, AddressOf gridReponse_CommandCellClick

    End Sub
    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub initControls()
        isNotValidate = (sousEpisode.HorodateValidate = Nothing)

        With sousEpisode
            If .HorodateCreation = Nothing Then .HorodateCreation = DateTime.Now
            Me.lblDateCreation.Text = .HorodateCreation.ToString("dd/MM/yyyy HH:mm") & " par " & userCreateNom
            Me.LblDateModif.Text = If(.HorodateLastUpdate = Nothing, "Non Modifié", .HorodateLastUpdate.ToString("dd/MM/yyyy HH:mm") & " par " & userUpdateNom)
            Me.LblDateValidation.Text = If(.HorodateValidate = Nothing, "Non Signé", .HorodateValidate.ToString("dd/MM/yyyy HH:mm") & " par " & userValidateNom)
        End With

        ' -- reponses
        If Not isCreation Then
            ChkBReponseAttendue.Checked = sousEpisode.IsReponse
            ChkBReponseAttendue.Enabled = isCreation
            TxtDelai.Value = If(sousEpisode.DelaiSinceValidation = Nothing, "", sousEpisode.DelaiSinceValidation)
            refreshGrid()
        End If
        TxtDelai.Enabled = isCreation
        BtnAjoutReponse.Visible = Not isCreation AndAlso isNotValidate = False
        BtnValidate.Visible = isCreation

        BtnEditerDocument.Visible = Not isCreation AndAlso isNotValidate

        Dim sousEpisodeSousType As SousEpisodeSousType = TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType)

        BtnValideAndSign.Visible = isCreation = False AndAlso isNotValidate _
                                  AndAlso SousEpisodeSousType.isUserLogAutorise(sousEpisodeSousType.ValidationProfilTypes)

        Me.DropDownType.Enabled = isCreation

        ChkALD.Checked = If(isCreation, isPatientALD, sousEpisode.IsALD)
        ChkALD.Enabled = isCreation

        Me.TxtRDVCommentaire.Text = sousEpisode.Commentaire
        Me.TxtRDVCommentaire.Enabled = isCreation
        Me.DropDownSousType.Enabled = isCreation
        Me.RadSousSousTypeGrid.ReadOnly = Not isCreation

    End Sub

    ''' <summary>
    ''' refresh du grid des reponses au sous-episode
    ''' </summary>
    Private Sub refreshGrid()
        Dim exId As Long, index As Integer = -1, exPosit = 0
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim data As DataTable = sousEpisodeReponseDao.getTableSousEpisodeReponse(sousEpisode.Id)

            Dim numRowGrid As Integer = 0

            ' -- recup eventuelle precedente selectionnée
            If RadReponseGrid.Rows.Count > 0 AndAlso Not IsNothing(Me.RadReponseGrid.CurrentRow) Then
                exId = Me.RadReponseGrid.CurrentRow.Cells("Id").Value
                exPosit = Me.RadReponseGrid.CurrentRow.Index
            End If
            RadReponseGrid.Rows.Clear()

            For Each row In data.Rows
                RadReponseGrid.Rows.Add(numRowGrid)
                '------------------- Alimentation du DataGridView
                With RadReponseGrid.Rows(numRowGrid)
                    .Cells("Id").Value = row("id")
                    If .Cells("Id").Value = exId Then index = numRowGrid   ' position exact
                    If numRowGrid <= exPosit Then exPosit = numRowGrid     ' position approchée 
                    .Cells("IdSousEpisode").Value = row("id_sous_episode")
                    .Cells("HorodateCreation").Value = row("horodate_creation")
                    .Cells("CreateUser").Value = row("user_create")
                    .Cells("commentaire").Value = row("commentaire")
                    .Cells("NomFichier").Value = row("nom_fichier")

                    ' -- on garnit le tag pour affichage tooltip
                    '                    RadTacheToTreatGrid.Rows.Last.Tag = " << " & .Cells("type").Value & " >>" & vbCrLf &
                    '                    If (Coalesce(row("is_ald"), False), " --> ALD" & vbCrLf, "") &
                    '                                "Fichier : " & row("nom_fichier") & vbCrLf &
                    '                    If (Coalesce(row("is_reponse"), False), " ... Réponse requise sous " & row("delai_since_validation") & " j à partir de la date de validation" & vbCrLf, "") &
                    '                    If (Coalesce(row("is_reponse_recue"), False), " ... Dernière reçue le  " & row("horodate_last_recu"), " ... NON REÇUE ...") & vbCrLf &
                    '                                " ------------------------------------------" & vbCrLf &
                    '                    row("commentaire") & vbCrLfThenThenThen
                End With

                numRowGrid += 1

            Next
            ' -- positionnement a la ligne la plus proche de la precedente
            If data.Rows.Count > 0 Then
                Me.RadReponseGrid.CurrentRow = RadReponseGrid.Rows(If(index >= 0, index, exPosit))
            End If
            Me.Cursor = Cursors.Default


        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try


    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sousEpisodeType"></param>
    ''' <returns></returns>
    Private Function filtreTypeByProfil(sousEpisodeType As SousEpisodeType) As Boolean
        If isCreation = False Then Return False   ' pas de filtre si pas en création
        ' -- on regarde si au moins un sousType autorisé pour ce profil
        For Each sousEpisodeSousType As SousEpisodeSousType In lstSousEpisodeSousType
            If sousEpisodeSousType.IdSousEpisodeType <> sousEpisodeType.Id Then Continue For
            If sousEpisodeSousType.isUserLogRedactionAutorise() Then Return False
        Next
        Return True '  => pas autorisé
    End Function

    Private Sub BtnEditerDocument_Click(sender As Object, e As EventArgs) Handles BtnEditerDocument.Click
        If Me.DropDownSousType.SelectedItem Is Nothing Then Return
        Dim sousType = (TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType))
        Try
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            Dim lstSousEpisodeFusion As List(Of SousEpisodeFusion) = constitueFusion()
            Dim tbl = telecharger_SousEpisodeDemande(sousType)
            Dim ins = New MemoryStream(tbl)
            Dim provider = New DocxFormatProvider()

            Using frm = New FrmEditDocxSousEpisode(sousEpisode)
                If tbl.Count > 0 Then
                    frm.RadRichTextEditor1.Document = provider.Import(ins)
                End If
                frm.RadRichTextEditor1.Document.MailMergeDataSource.ItemsSource = lstSousEpisodeFusion
                frm.RadRichTextEditor1.UpdateAllFields(FieldDisplayMode.DisplayName)

                Dim merged = frm.RadRichTextEditor1.MailMerge()
                frm.RadRichTextEditor1.Document = merged
                ins.Dispose()
                tbl = Nothing
                'frm.ReplaceAllMatches("toto", "titi")
                frm.ShowDialog()
            End Using
            refreshGrid()
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Function telecharger_model(sousEpisodeSousType As SousEpisodeSousType) As Byte()
        Dim tbl As Byte() = {}
        Try
            tbl = sousEpisodeSousType.getContenuModel()
            Return tbl
        Catch err As Exception
            If err IsNot Nothing AndAlso err.Message IsNot Nothing AndAlso err.Message.Contains("Fichier demandé inexistant") Then
                Return tbl
            End If
            Throw err
        End Try
    End Function

    Private Function telecharger_SousEpisodeDemande(sousEpisodeSousType As SousEpisodeSousType) As Byte()
        Dim tbl As Byte() = {}
        Try
            tbl = sousEpisode.getContenu()
            Return tbl
        Catch err As Exception
            ' --- si inexistant => 1ere saisie : on récupère le modèle
            If err IsNot Nothing AndAlso err.Message IsNot Nothing AndAlso err.Message.Contains("Fichier demandé inexistant") Then
                tbl = sousEpisodeSousType.getContenuModel()
                Return tbl
            End If
            Throw err
        End Try
    End Function

    Private Function constitueFusion() As List(Of SousEpisodeFusion)
        Dim lstFusion = New List(Of SousEpisodeFusion)(1)
        Dim sousEF As New SousEpisodeFusion
        Dim episodeParametreDao = New EpisodeParametreDao
        Dim uniteSanitaireDao = New UniteSanitaireDao
        Dim siteDao = New SiteDao
        ' -- recherche de l'unite sanitaire et du site du patient
        Dim uniteSanitaire = uniteSanitaireDao.getUniteSanitaireById(patient.PatientUniteSanitaireId)
        Dim site = siteDao.getSiteById(patient.PatientSiteId)
        With sousEF
            .USNom = uniteSanitaire.Oa_unite_sanitaire_description
            .USAdr1 = uniteSanitaire.Oa_unite_sanitaire_adresse1
            .USAdr2 = uniteSanitaire.Oa_unite_sanitaire_adresse2
            .USCP = uniteSanitaire.Oa_unite_sanitaire_code_postal
            .USVille = uniteSanitaire.Oa_unite_sanitaire_ville
            .USTel = ""
            .USFax = ""
            .USEmail = ""

            .SiteNom = site.Oa_site_description
            .SiteAdr1 = site.Oa_site_adresse1
            .SiteAdr2 = site.Oa_site_adresse2
            .SiteCP = site.Oa_site_code_postal
            .SiteVille = site.Oa_site_ville
            .SiteTel = "tel du site"
            .SiteFax = "tel du site"
            .SiteEmail = "tel du site"

            .Patient_PrenomNom = patient.PatientPrenom & " " & patient.PatientNom
            .Patient_NIR = patient.PatientNir
            .Patient_Date_Naissance = patient.PatientDateNaissance.ToString("dd/MM/yyyy")
            .Patient_Age = patient.PatientAge
            .Patient_Poids = "" & episodeParametreDao.getPoidsByEpisodeIdOrLastKnow(sousEpisode.EpisodeId, patient.patientId)
            .Patient_Poids = If(.Patient_Poids = "0", "", .Patient_Poids)
            .Patient_sexe = patient.PatientGenre.ToLower

            .Commentaire = sousEpisode.Commentaire

            .Episode_DateHeure = episode.DateCreation.ToString("dd MMMM yyyy à hh:mm")
            .Type_Libelle = Me.DropDownType.SelectedItem.Text
            .Sous_Type_Libelle = Me.DropDownSousType.SelectedItem.Text

            ' -- recherche des sous-type_detail (sousoustype)
            Dim isWithALD = False, isWithNonAld = False
            If Me.RadSousSousTypeGrid.Rows.Count > 0 Then
                For Each row In RadSousSousTypeGrid.Rows
                    If row.Cells("ChkChoice").Value Then
                        If row.Cells("ChkALD").Value Then ' ALD
                            isWithALD = True
                            .Sous_Type_Libelle_Detail_ALD += vbCrLf
                            .Sous_Type_Libelle_Detail_ALD += (row.Cells("Libelle").Value & vbCrLf)
                            .Sous_Type_Libelle_Detail_commentaire_ALD += vbCrLf
                            .Sous_Type_Libelle_Detail_commentaire_ALD += (row.Cells("Commentaire").Value & vbCrLf)
                        Else
                            isWithNonAld = True
                            .Sous_Type_Libelle_Detail_Non_ALD += vbCrLf
                            .Sous_Type_Libelle_Detail_Non_ALD += (row.Cells("Libelle").Value & vbCrLf)
                            .Sous_Type_Libelle_Detail_commentaire_non_ALD += vbCrLf
                            .Sous_Type_Libelle_Detail_commentaire_non_ALD += (row.Cells("Commentaire").Value & vbCrLf)
                        End If
                    End If
                Next
            End If
            If isWithALD = False Then .ALD_Avec_Entete = ""
            If isWithNonAld = False Then .ALD_Sans_Entete = ""

            .Signature_Date = Date.Now.ToString("dd MMM yyyy")
            .Signataire_Fonction = userLog.UtilisateurProfilId.ToLower.Trim.Replace("_", " ")
            .Signataire_PrenomNom = userLog.UtilisateurPrenom.Trim & " " & userLog.UtilisateurNom.Trim

        End With
        lstFusion.Add(sousEF)

        Return lstFusion
    End Function


    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idType"></param>
    Private Sub initSousTypes(idType As Long)
        Me.DropDownSousType.Items.Clear()

        For Each sousEpisodeSousType As SousEpisodeSousType In lstSousEpisodeSousType
            If sousEpisodeSousType.IdSousEpisodeType <> idType Then Continue For ' pas pour ce type d'episode
            If isCreation = False AndAlso sousEpisodeSousType.isUserLogRedactionAutorise() = False Then Continue For ' pas autorisé

            Dim radListItemST As New RadListDataItem(sousEpisodeSousType.Libelle, sousEpisodeSousType)
            Me.DropDownSousType.Items.Add(radListItemST)
            If sousEpisodeSousType.Id = sousEpisode.IdSousEpisodeSousType Then
                radListItemST.Selected = True
                'initSousSousTypes(sousEpisodeSousType.Id)
            End If
        Next
        If DropDownSousType.SelectedItem Is Nothing AndAlso DropDownSousType.Items.Count > 0 Then
            Me.DropDownSousType.SelectedItem = Me.DropDownSousType.Items(0)
        End If

        Me.DropDownSousType.Enabled = isCreation
        Me.DropDownSousType.DefaultItemsCountInDropDown = DropDownSousType.Items.Count

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idSousType"></param>
    Private Sub initSousSousTypes(idSousType As Long)
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim numRowGrid As Integer = 0

            RadSousSousTypeGrid.Rows.Clear()

            For Each sousEpisodeSousSousType As SousEpisodeSousSousType In lstSousEpisodeSousSousType
                If sousEpisodeSousSousType.IdSousEpisodeSousType <> idSousType Then Continue For ' pas pour ce sous type d'episode
                If Not isCreation AndAlso sousEpisode.isThisSousSousTypePresent(sousEpisodeSousSousType.Id) = False Then Continue For
                RadSousSousTypeGrid.Rows.Add(numRowGrid)
                '------------------- Alimentation du DataGridView
                With RadSousSousTypeGrid.Rows(numRowGrid)
                    .Cells("Id").Value = sousEpisodeSousSousType.Id
                    .Cells("IdSousEpisodeSousType").Value = sousEpisodeSousSousType.IdSousEpisodeSousType
                    .Cells("Libelle").Value = sousEpisodeSousSousType.Libelle
                    If isCreation Then
                        .Cells("ChkALD").Value = isPatientALD
                    Else
                        .Cells("ChkChoice").Value = True
                        .Cells("ChkALD").Value = sousEpisode.isThisDetailALD(sousEpisodeSousSousType.Id)
                    End If
                    .Cells("Commentaire").Value = sousEpisodeSousSousType.commentaire
                End With

                numRowGrid += 1

            Next
            Me.SplitPanelSousSousType.Collapsed = (numRowGrid = 0)
            Me.Cursor = Cursors.Default
        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub initALDetReponse()
        If Me.DropDownSousType.SelectedItem IsNot Nothing Then
            Dim sousType As SousEpisodeSousType = TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType)
            Dim visible = sousType.IsALDPossible AndAlso isPatientALD
            ChkALD.Visible = visible
            LblALD.Visible = visible
            RadSousSousTypeGrid.Columns("ChkALD").IsVisible = visible
            RadSousSousTypeGrid.Columns("ChkChoice").IsVisible = isCreation

            If isCreation Then
                ChkBReponseAttendue.Checked = sousType.IsReponseRequise
                'ChkBReponseAttendue.Enabled = Not sousType.IsReponseRequise
            End If
        End If
    End Sub

    Private Sub serializeSousEpisode(isWithSign As Boolean)
        With sousEpisode
            .HorodateCreation = DateTime.Now
            .EpisodeId = episode.Id
            .IdSousEpisodeType = TryCast(Me.DropDownType.SelectedItem.Value, SousEpisodeType).Id
            .IdSousEpisodeSousType = TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType).Id
            .CreateUserId = userLog.UtilisateurId
            .Commentaire = TxtRDVCommentaire.Text
            .IsALD = ChkALD.Checked And TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType).IsALDPossible And isPatientALD
            .IsReponse = ChkBReponseAttendue.Checked
            .DelaiSinceValidation = TxtDelai.Value

            Dim lstDetail As New List(Of SousEpisodeDetailSousType)
            Dim sousEpisodeDetail As SousEpisodeDetailSousType
            For Each row In RadSousSousTypeGrid.Rows
                If row.Cells("ChkChoice").Value Then
                    sousEpisodeDetail = New SousEpisodeDetailSousType()
                    sousEpisodeDetail.IdSousEpisode = sousEpisode.EpisodeId
                    sousEpisodeDetail.IdSousEpisodeSousSousType = row.Cells("Id").Value
                    sousEpisodeDetail.IsALD = row.Cells("ChkALD").Value
                    lstDetail.Add(sousEpisodeDetail)
                End If
            Next
            If lstDetail.Count = 0 AndAlso RadSousSousTypeGrid.Rows.Count > 0 Then
                MsgBox("Vous devez choisir au moins un élément dans le tableau des détails ! ")
                Return
            End If
            .lstDetail = lstDetail
        End With


        sousEpisodeDao.Create(sousEpisode, isWithSign)
        If isWithSign Then userValidateNom = userLog.UtilisateurPrenom + " " + userLog.UtilisateurNom

        ' --- reaffiche le formulaire en mode update
        isCreation = False
        initControls()
    End Sub

End Class

