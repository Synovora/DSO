Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class FrmSousEpisode

    Dim sousEpisodeDao As SousEpisodeDao = New SousEpisodeDao
    Dim sousEpisodeTypeDao As SousEpisodeTypeDao = New SousEpisodeTypeDao
    Dim sousEpisodeSousTypeDao As SousEpisodeSousTypeDao = New SousEpisodeSousTypeDao

    Dim episode As Episode, patient As Patient, sousEpisode As SousEpisode
    Dim isCreation As Boolean

    Dim lstSousEpisodeType As List(Of SousEpisodeType) = New List(Of SousEpisodeType)
    Dim lstSousEpisodeSousType As List(Of SousEpisodeSousType) = New List(Of SousEpisodeSousType)


    Sub New(episode As Episode, patient As Patient, sousEpisode As SousEpisode)

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        afficheTitleForm(Me, Me.Text)
        ' -- episode en cours
        Me.episode = episode
        Me.patient = patient
        Me.sousEpisode = sousEpisode
        '  -- sous en mode creation (sinon mode update)
        isCreation = If(sousEpisode.Id = 0, True, False)

        initControls()
    End Sub

    Private Sub DropDownType_SelectedIndexChanged(sender As Object, e As Data.PositionChangedEventArgs) Handles DropDownType.SelectedIndexChanged
        initSousTypes(lstSousEpisodeType(e.Position).Id)
    End Sub

    Private Sub initControls()
        ' -- listes de references
        lstSousEpisodeType = sousEpisodeTypeDao.getLstSousEpisodeType()
        lstSousEpisodeSousType = sousEpisodeSousTypeDao.getLstSousEpisodeSousType()
        With sousEpisode
            If .HorodateCreation = Nothing Then .HorodateCreation = DateTime.Now
            Me.lblDateCreation.Text = .HorodateCreation.ToString("dd/MM/yyyy HH:mm")
            Me.LblDateModif.Text = If(.HorodateLastUpdate = Nothing, "", .HorodateLastUpdate.ToString("dd/MM/yyyy HH:mm"))
            Me.LblDateValidation.Text = If(.HorodateValidate = Nothing, "", .HorodateValidate.ToString("dd/MM/yyyy HH:mm"))

            For Each sousEpisodeType As SousEpisodeType In lstSousEpisodeType
                Dim radListItem As New RadListDataItem(sousEpisodeType.Libelle, sousEpisodeType)
                Me.DropDownType.Items.Add(radListItem)
                'If TryCast(radListItem.Value, SousEpisodeType).Id = Me.sousEpisode.IdSousEpisodeType Then
                If sousEpisodeType.Id = Me.sousEpisode.IdSousEpisodeType Then
                    radListItem.Selected = True
                    ' -- init des sous types
                    initSousTypes(sousEpisodeType.Id)
                End If
            Next
            If isCreation Then
                Me.DropDownType.SelectedItem = Me.DropDownType.Items(0)
            Else
                If sousEpisode.HorodateValidate <> Nothing Then
                    Me.DropDownType.Enabled = False
                End If
            End If

        End With


    End Sub
    Private Sub initSousTypes(idType As Long)
        Me.DropDownSousType.Items.Clear()

        For Each sousEpisodeSousType As SousEpisodeSousType In lstSousEpisodeSousType
            If sousEpisodeSousType.IdSousEpisodeType <> idType Then Continue For
            Dim radListItemST As New RadListDataItem(sousEpisodeSousType.Libelle, sousEpisodeSousType)
            Me.DropDownSousType.Items.Add(radListItemST)
            If sousEpisodeSousType.Id = sousEpisode.IdSousEpisodeSousType Then
                radListItemST.Selected = True
            End If
        Next
        If DropDownSousType.SelectedItem Is Nothing Then
            Me.DropDownSousType.SelectedItem = Me.DropDownSousType.Items(0)
        End If
        If sousEpisode.HorodateValidate <> Nothing Then
            Me.DropDownSousType.Enabled = False
        End If

    End Sub

End Class

