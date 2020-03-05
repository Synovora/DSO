Imports Oasis_Common
Imports Telerik.WinControls.Enumerations
Imports Telerik.WinControls.UI

Public Class FrmSousEpisode

    Dim sousEpisodeDao As SousEpisodeDao = New SousEpisodeDao
    Dim sousEpisodeTypeDao As SousEpisodeTypeDao = New SousEpisodeTypeDao
    Dim sousEpisodeSousTypeDao As SousEpisodeSousTypeDao = New SousEpisodeSousTypeDao
    Dim sousEpisodeReponseDao As SousEpisodeReponseDao = New SousEpisodeReponseDao

    Dim episode As Episode, patient As Patient, sousEpisode As SousEpisode
    Dim userCreateNom As String, userUpdateNom As String, userValidateNom As String
    Dim isCreation As Boolean
    Dim isNotValidate As Boolean

    Dim lstSousEpisodeType As List(Of SousEpisodeType) = New List(Of SousEpisodeType)
    Dim lstSousEpisodeSousType As List(Of SousEpisodeSousType) = New List(Of SousEpisodeSousType)


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

        ' -- initialisation des controles du formulaire
        initControls()
    End Sub

    Private Sub DropDownType_SelectedIndexChanged(sender As Object, e As Data.PositionChangedEventArgs) Handles DropDownType.SelectedIndexChanged
        initSousTypes(lstSousEpisodeType(e.Position).Id)
    End Sub


    Private Sub ChkBReponseAttendue_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles ChkBReponseAttendue.ToggleStateChanged
        TxtDelai.Visible = If(args.ToggleState = ToggleState.On, True, False)
        LblDelai.Visible = If(args.ToggleState = ToggleState.On, True, False)
    End Sub

    Private Sub BtnAjoutReponse_Click(sender As Object, e As EventArgs) Handles BtnAjoutReponse.Click
        Dim dr As DialogResult = OpenFileDialog1.ShowDialog()

        If dr = System.Windows.Forms.DialogResult.OK Then
            Dim fileName As String = OpenFileDialog1.FileName
            Dim comment = RadInputBox.Show("Fichier : " & fileName & vbCrLf & "Saisissez votre commentaire", "Introduction de document reçu", "#CANCEL#")

            If comment = "#CANCEL#" Then
                Notification.show("Ajout document", "Ajout annulé !")
                Return
            End If


            MsgBox(fileName)
            Notification.show("Ajout document", "Ajout terminée avec succès !")
        End If
    End Sub

    Private Sub initControls()
        ' -- listes de references
        lstSousEpisodeType = sousEpisodeTypeDao.getLstSousEpisodeType()
        lstSousEpisodeSousType = sousEpisodeSousTypeDao.getLstSousEpisodeSousType()
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
            End If
            Me.DropDownType.Enabled = isCreation

            Me.LblFichier.Text = .NomFichier
            Me.TxtRDVCommentaire.Text = .Commentaire
            Me.TxtRDVCommentaire.Enabled = isCreation
        End With
        ' -- reponses
        ChkBReponseAttendue.Checked = sousEpisode.IsReponse
        TxtDelai.Value = If(sousEpisode.DelaiSinceValidation = Nothing, "", sousEpisode.DelaiSinceValidation)
        ChkBReponseAttendue.Enabled = isCreation
        TxtDelai.Enabled = isCreation
        BtnAjoutReponse.Visible = Not isCreation
        '-- refresh grid reponse
        refreshGrid()

    End Sub

    Private Sub refreshGrid()
        Dim exId As Long, index As Integer = -1, exPosit = 0
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim data As DataTable = sousEpisodeReponseDao.getTableSousEpisodeReponse(sousEpisode.Id)

            Dim numRowGrid As Integer = 0

            ' -- recup eventuelle precedente selectionnée
            If RadTacheToTreatGrid.Rows.Count > 0 AndAlso Not IsNothing(Me.RadTacheToTreatGrid.CurrentRow) Then
                exId = Me.RadTacheToTreatGrid.CurrentRow.Cells("Id").Value
                exPosit = Me.RadTacheToTreatGrid.CurrentRow.Index
            End If
            RadTacheToTreatGrid.Rows.Clear()

            For Each row In data.Rows
                RadTacheToTreatGrid.Rows.Add(numRowGrid)
                '------------------- Alimentation du DataGridView
                With RadTacheToTreatGrid.Rows(numRowGrid)
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
                Me.RadTacheToTreatGrid.CurrentRow = RadTacheToTreatGrid.Rows(If(index >= 0, index, exPosit))
            End If
            Me.Cursor = Cursors.Default


        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try


    End Sub


    Private Function filtreTypeByProfil(sousEpisodeType As SousEpisodeType) As Boolean
        If isCreation = False Then Return False   ' pas de filtre si pas en création
        ' -- on regarde si au moins un sousType autorisé pour ce profil
        For Each sousEpisodeSousType As SousEpisodeSousType In lstSousEpisodeSousType
            If sousEpisodeSousType.IdSousEpisodeType <> sousEpisodeType.Id Then Continue For
            If sousEpisodeSousType.isUserLogRedactionAutorise() Then Return False
        Next
        Return True '  => pas autorisé
    End Function

    Private Sub initSousTypes(idType As Long)
        Me.DropDownSousType.Items.Clear()

        For Each sousEpisodeSousType As SousEpisodeSousType In lstSousEpisodeSousType
            If sousEpisodeSousType.IdSousEpisodeType <> idType Then Continue For ' pas pour ce type d'episode
            If isCreation = False AndAlso sousEpisodeSousType.isUserLogRedactionAutorise() = False Then Continue For ' pas autorisé

            Dim radListItemST As New RadListDataItem(sousEpisodeSousType.Libelle, sousEpisodeSousType)
            Me.DropDownSousType.Items.Add(radListItemST)
            If sousEpisodeSousType.Id = sousEpisode.IdSousEpisodeSousType Then
                radListItemST.Selected = True
            End If
        Next
        If DropDownSousType.SelectedItem Is Nothing AndAlso DropDownSousType.Items.Count > 0 Then
            Me.DropDownSousType.SelectedItem = Me.DropDownSousType.Items(0)
        End If
        Me.DropDownSousType.Enabled = isCreation

    End Sub

End Class

