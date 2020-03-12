Imports System.Configuration
Imports System.IO
Imports Oasis_Common
Imports Telerik.WinControls.Enumerations
Imports Telerik.WinControls.UI

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

        '  -- somme nous en mode creation (sinon mode update)
        isCreation = If(sousEpisode.Id = 0, True, False)
        isNotValidate = (sousEpisode.HorodateValidate = Nothing)
        ' -- le patient est il en ALD
        Dim aldDO = New AldDao()
        isPatientALD = aldDO.IsPatientALD(patient.patientId)

        ' -- initialisation des controles du formulaire
        initControls()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub DropDownType_SelectedIndexChanged(sender As Object, e As Data.PositionChangedEventArgs) Handles DropDownType.SelectedIndexChanged
        initSousTypes(lstSousEpisodeType(e.Position).Id)
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
                telecharger(gce)
            Case "supprimer"
                MsgBox("supprimer")
        End Select
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub BtnValidate_Click(sender As Object, e As EventArgs) Handles BtnValidate.Click
        serializeSousEpisode()
    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="gce"></param>
    Private Sub telecharger(gce As GridCommandCellElement)
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

    ''' <summary>
    ''' 
    ''' </summary>
    Private Sub initControls()
        ' -- listes de references
        lstSousEpisodeType = sousEpisodeTypeDao.getLstSousEpisodeType()
        lstSousEpisodeSousType = sousEpisodeSousTypeDao.getLstSousEpisodeSousType()
        lstSousEpisodeSousSousType = sousEpisodeSousSousTypeDao.getLstSousEpisodeSousSousType()
        With sousEpisode
            If .HorodateCreation = Nothing Then .HorodateCreation = DateTime.Now
            Me.lblDateCreation.Text = .HorodateCreation.ToString("dd/MM/yyyy HH:mm") & " par " & userCreateNom
            Me.LblDateModif.Text = If(.HorodateLastUpdate = Nothing, "Non Modifiée", .HorodateLastUpdate.ToString("dd/MM/yyyy HH:mm") & " par " & userUpdateNom)
            Me.LblDateValidation.Text = If(.HorodateValidate = Nothing, "Non Validée", .HorodateValidate.ToString("dd/MM/yyyy HH:mm") & " par " & userValidateNom)

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
            Me.DropDownType.Enabled = isCreation

            ChkALD.Checked = If(isCreation, isPatientALD, sousEpisode.IsALD)
            ChkALD.Enabled = isCreation

            Me.TxtRDVCommentaire.Text = .Commentaire
            Me.TxtRDVCommentaire.Enabled = isCreation
        End With

        '-- handler sur boutons grid reponse
        AddHandler RadReponseGrid.CommandCellClick, AddressOf gridReponse_CommandCellClick

        ' -- reponses
        If Not isCreation Then
            ChkBReponseAttendue.Checked = sousEpisode.IsReponse
            ChkBReponseAttendue.Enabled = isCreation
            TxtDelai.Value = If(sousEpisode.DelaiSinceValidation = Nothing, "", sousEpisode.DelaiSinceValidation)
            refreshGrid()
        End If
        TxtDelai.Enabled = isCreation
        BtnAjoutReponse.Visible = Not isCreation
        BtnValidate.Visible = isCreation
        Me.RadSousSousTypeGrid.ReadOnly = Not isCreation
    End Sub

    ''' <summary>
    ''' 
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

    End Sub

    ''' <summary>
    ''' 
    ''' </summary>
    ''' <param name="idSousType"></param>
    Private Sub initSousSousTypes(idSousType As Long)
        Me.Cursor = Cursors.WaitCursor
        Try
            'Dim data As DataTable = sousEpisodeReponseDao.getTableSousEpisodeReponse(sousEpisode.Id)

            Dim numRowGrid As Integer = 0

            RadSousSousTypeGrid.Rows.Clear()

            For Each sousEpisodeSousSousType As SousEpisodeSousSousType In lstSousEpisodeSousSousType
                If sousEpisodeSousSousType.IdSousEpisodeSousType <> idSousType Then Continue For ' pas pour ce sous type d'episode
                RadSousSousTypeGrid.Rows.Add(numRowGrid)
                '------------------- Alimentation du DataGridView
                With RadSousSousTypeGrid.Rows(numRowGrid)
                    .Cells("Id").Value = sousEpisodeSousSousType.Id
                    .Cells("IdSousEpisodeSousType").Value = sousEpisodeSousSousType.IdSousEpisodeSousType
                    .Cells("Libelle").Value = sousEpisodeSousSousType.Libelle
                    '.cells("ChkChoisir").value = TODO
                    If isCreation Then
                        .Cells("ChkALD").Value = isPatientALD
                    Else
                        '.Cells("ChkALD").Value = TODO
                    End If
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

            If isCreation Then
                ChkBReponseAttendue.Checked = sousType.IsReponseRequise
                ChkBReponseAttendue.Enabled = Not sousType.IsReponseRequise
            End If
        End If
    End Sub

    Private Sub serializeSousEpisode()
        With sousEpisode
            .HorodateCreation = DateTime.Now
            .EpisodeId = episode.Id
            .IdSousEpisodeType = TryCast(Me.DropDownType.SelectedItem.Value, SousEpisodeType).Id
            .IdSousEpisodeSousType = TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType).Id
            .CreateUserId = userLog.UtilisateurId
            .Commentaire = TxtRDVCommentaire.Text
            .IsALD = ChkALD.Checked And TryCast(Me.DropDownSousType.SelectedItem.Value, SousEpisodeSousType).IsALDPossible And isPatientALD
            .IsReponse = ChkALD.Checked
            .DelaiSinceValidation = TxtDelai.Value
        End With
        sousEpisodeDao.Create(sousEpisode)

        ' --- reaffiche le formulaire en mode update
        isCreation = False
        initControls()
    End Sub

End Class

