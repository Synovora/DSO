
Imports System.ComponentModel
Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.Localization
Imports Oasis_Common
Public Class FrmTacheMain

    ' -- liste des RDV bindée
    Private lstAppointments As New BindingList(Of AppointmentOasis)()
    Private dateRefDeb As Date, dateRefFin As Date
    Private filterTache As FiltreTache = New FiltreTache()
    Private lstFonctionChoisie As List(Of Fonction) = New List(Of Fonction)

    Private tacheDao As TacheDao = New TacheDao
    Private chkATraite As Integer = 0        ' --  chk pour rafraichissement 

#Region "INITIALISATION"

    Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        AfficheTitleForm(Me, "", userLog)

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().
        Me.SplitContainerMain.Dock = DockStyle.Fill

        RadTacheToTreatGrid.Dock = DockStyle.Fill
        RadPanelGridATraiter.Dock = DockStyle.Fill
        RadTacheEmiseGrid.Dock = DockStyle.Fill
        ' -- permet de mettre toutes les popup standard grid en français
        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()

        initScheduler()
        initChkBoxFonction()
        initFiltre()

        ' -- load des grid
        refreshGridTacheATraiter()
        'RadioMesTachesEnCours.CheckState = CheckState.Checked
        RadioTachesFonctionEnCours.CheckState = CheckState.Checked
        'refreshGridTacheEnCours() : pas besoin fait par l'evenement du bouton radio qui est checked par defaut (RadioTachesFonctionEnCours)
        If RadTacheEnCoursGrid.Rows.Count > 0 Then
            Me.RadTacheEnCoursGrid.CurrentRow = RadTacheEnCoursGrid.Rows(0)
        End If
        'AddHandler PageTache.SelectedPageChanged, AddressOf PageTache_SelectedPageChanged

        TimerRefreshTaches.Enabled = True
    End Sub

    Private Sub initScheduler()
        ' ---------------------------------------------- AGENDA
        ' localisation french
        SchedulerNavigatorLocalizationProvider.CurrentProvider = New FrenchSchedulerNavigatorLocalizationProvider
        RadSchedulerLocalizationProvider.CurrentProvider = New FrenchSchedulerLocalizationProvider
        Me.RadSchedulerNavigator1.SchedulerNavigatorElement.SearchTextBox.Visibility = ElementVisibility.Collapsed
        ' on met en step une journee par defaut
        RadScheduler1.GetDayView.DayCount = 1
        setStepScheduler()

        ' -- fixe les tailles du splitter par defaut
        SplitContainerMain.SplitPanels("SplitPanelGauche").SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Absolute
        SplitContainerMain.SplitPanels("SplitPanelGauche").SizeInfo.AbsoluteSize = New Size(450, 0)
        RadSplitContainer1.SplitPanels("SplitPanelTaches").SizeInfo.SizeMode = Telerik.WinControls.UI.Docking.SplitPanelSizeMode.Absolute
        RadSplitContainer1.SplitPanels("SplitPanelTaches").SizeInfo.AbsoluteSize = New Size(0, 250)

        ' -- ajout initial du handler de chgt de date debut
        AddHandler Me.RadScheduler1.ActiveView.PropertyChanged, AddressOf ActiveView_PropertyChanged

        ' --- init du binding de l'agenda
        initCustomBindingAgenda()
    End Sub

    Private Sub setStepScheduler()
        ' on met en step d'un quart d'heure la vue jour et semaine
        If RadScheduler1.ActiveViewType = SchedulerViewType.Day Then
            RadScheduler1.GetDayView.RangeFactor = ScaleRange.QuarterHour
            RadScheduler1.GetDayView.RulerStartScale = 7
            RadScheduler1.GetDayView.RulerEndScale = 21
        End If
        If RadScheduler1.ActiveViewType = SchedulerViewType.Week Then
            RadScheduler1.GetWeekView.RangeFactor = ScaleRange.QuarterHour
            RadScheduler1.GetWeekView.RulerStartScale = 7
            RadScheduler1.GetWeekView.RulerEndScale = 21
        End If
        If RadScheduler1.ActiveViewType = SchedulerViewType.Month Then
            RadScheduler1.GetMonthView.ShowFullMonth = True
        End If

    End Sub

    Private Sub initCustomBindingAgenda()
        'create_and_configure_a_scheduler_binding_source
        Dim dataSource As New SchedulerBindingDataSource()

        'mapper les properties au scheduler
        Dim appointmentMappingInfo As New AppointmentMappingInfo()
        appointmentMappingInfo.Start = "Start"
        appointmentMappingInfo.[End] = "End"
        appointmentMappingInfo.Summary = "Subject"
        appointmentMappingInfo.Description = "Description"
        appointmentMappingInfo.Location = "Location"
        appointmentMappingInfo.UniqueId = "Id"
        appointmentMappingInfo.Exceptions = "Exceptions"
        appointmentMappingInfo.BackgroundId = "BackgroundId"

        dataSource.EventProvider.Mapping = appointmentMappingInfo

        'assigne la generic List de CustomAppointment comme EventProvider data source
        dataSource.EventProvider.DataSource = lstAppointments
        Me.RadScheduler1.DataSource = dataSource

    End Sub

    ''' <summary>
    ''' creation dynamique et Initialisation des chkbox fonctions user
    ''' </summary>
    Private Sub initChkBoxFonction()
        ' --- checkboxs fonctions
        Dim x As Integer
        Dim y As Integer = -10
        Dim chkBox As RadCheckBox
        Dim fonction As Fonction

        For i = 0 To userLog.LstFonction.Count - 1

            fonction = userLog.LstFonction(i)
            ' -- creation dynamique et fix emplacement
            chkBox = New RadCheckBox()
            chkBox.Tag = fonction
            chkBox.Name = "CheckFonction_" & i
            chkBox.Text = fonction.Designation
            chkBox.Checked = True
            lstFonctionChoisie.Add(fonction)
            If (i Mod 3) = 0 Then
                If y > 0 AndAlso i > 0 Then
                    RadPanelCheckBoxFonction.Height += 15
                    RadPanelFiltre.Height += 15
                End If
                x = -132
                y = y + 15
            End If
            x += 140
            chkBox.Location = New Point(x, y)
            ' -- fix evenementiel
            RadPanelCheckBoxFonction.Controls.Add(chkBox)
            AddHandler chkBox.PropertyChanged, AddressOf CheckFonction_PropertyChanged

        Next

    End Sub

    Private Sub initFiltre()
        Dim siteDao = New SiteDao
        Dim uniteSanitaireDao = New UniteSanitaireDao
        Dim uniteSanitaire As UniteSanitaire

        filterTache = New FiltreTache

        Try
            If userLog.UtilisateurUniteSanitaireId <> 0 Then
                uniteSanitaire = uniteSanitaireDao.getUniteSanitaireById(userLog.UtilisateurUniteSanitaireId)
                If userLog.UtilisateurSiteId <> 0 Then
                    uniteSanitaire.AddSite(siteDao.getSiteById(userLog.UtilisateurSiteId))
                Else
                    uniteSanitaire.LstSite = siteDao.getList(False, userLog.UtilisateurUniteSanitaireId)
                End If
                filterTache.AddUniteSanitaire(uniteSanitaire)
            End If

        Catch err As Exception
            MsgBox(err.Message)
        End Try
        refreshPanelFilter()

    End Sub

#End Region

#Region "Agenda"
    Sub ActiveView_PropertyChanged(ByVal sender As Object, ByVal e As PropertyChangedEventArgs)
        'Console.WriteLine("ActiveView_PropertyChanged => " & e.PropertyName)
        'load the data here
        Select Case e.PropertyName
            Case "StartDate", "DayCount", "EndDate"
                refreshSchedulerVisible(False)
        End Select
    End Sub

    Private Sub RadScheduler1_ActiveViewChanging(sender As Object, e As SchedulerViewChangingEventArgs) Handles RadScheduler1.ActiveViewChanging
        Console.WriteLine("ActiveViewChanging : " & e.NewView.StartDate & "  - " & e.NewView.EndDate)

        ' -- mise en place du handler de startdate
        RemoveHandler e.OldView.PropertyChanged, AddressOf ActiveView_PropertyChanged
        AddHandler e.NewView.PropertyChanged, AddressOf ActiveView_PropertyChanged

        refreshScheduler(e.NewView.StartDate, e.NewView.EndDate, False)
    End Sub

    Private Sub RadScheduler1_ActiveViewChanged(sender As Object, e As SchedulerViewChangedEventArgs) Handles RadScheduler1.ActiveViewChanged
        setStepScheduler()  ' pas d'un quart d'heure ...etc
    End Sub

    Private Sub refreshSchedulerVisible(force As Boolean)
        Dim dateDeb As Date = RadScheduler1.ActiveView.StartDate
        Dim dateFin As Date = RadScheduler1.ActiveView.EndDate
        refreshScheduler(dateDeb, dateFin, force)
    End Sub

    Private Sub refreshScheduler(dateDebut As Date, dateFin As Date, force As Boolean)
        'Dim exId As Long, index As Integer = -1, exPosit = 0
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim numRowGrid As Integer = 0
            If (force = False) Then
                If (dateRefDeb.Equals(dateDebut) AndAlso dateRefFin.Equals(dateFin)) Then Return
            End If
            Console.WriteLine("rechargement agenda : " & dateDebut & " - " & dateFin & " - force = " & force)
            dateRefDeb = dateDebut
            dateRefFin = dateFin
            Me.Cursor = Cursors.WaitCursor
            RadScheduler1.Enabled = False
            Application.DoEvents()

            Dim isMyTache As Boolean = RadioMesTachesEnCours.IsChecked
            Dim isWithNonAttribue = RadChkNonAttribuee.Checked
            Dim data As DataTable = tacheDao.GetAgendaMyRDV(dateDebut, dateFin, isMyTache, lstFonctionChoisie, filterTache, isWithNonAttribue, userLog)

            lstAppointments.Clear()

            Dim rdv As AppointmentOasis
            For Each row In data.Rows
                rdv = New AppointmentOasis(row)
                lstAppointments.Add(rdv)

                numRowGrid += 1
            Next

            'RadScheduler1.Enabled = True
            lstAppointments.ResetBindings()

        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
            RadScheduler1.Enabled = True
        End Try

    End Sub

#End Region

#Region "Taches à traiter"
    ''' <summary>
    ''' Evenmentiel des Checkbox Fonction (abonnement de l'utilsateur pour filtrer tâches à traiter)
    ''' </summary>
    ''' <param name="sender"></param>
    ''' <param name="e"></param>
    Private Sub CheckFonction_PropertyChanged(sender As Object, e As PropertyChangedEventArgs)
        Select Case e.PropertyName
            Case "IsChecked"
                Dim chkbox As RadCheckBox
                lstFonctionChoisie.Clear()
                ' -- on refixe la liste des fonctions choisies
                For i = 0 To userLog.LstFonction.Count - 1
                    chkbox = RadPanelCheckBoxFonction.Controls.Item("CheckFonction_" & i)
                    If chkbox.Checked Then lstFonctionChoisie.Add(chkbox.Tag)
                Next

                ' -- refresh du grid
                refreshGridTacheATraiter()
        End Select
    End Sub

    Private Sub btnFiltre_Click(sender As Object, e As EventArgs) Handles btnFiltreTAT.Click
        ShowFiltre()
    End Sub

    Private Sub RadTextBox1_Click(sender As Object, e As EventArgs) Handles RadTextBox1.Click
        ShowFiltre()
    End Sub

    Private Sub ShowFiltre()
        Try
            Me.Enabled = False
            Using frmFiltreTacheATraiter = New FrmFiltreTacheATraiter(filterTache)
                frmFiltreTacheATraiter.Tag = filterTache
                frmFiltreTacheATraiter.ShowDialog()
                If Not frmFiltreTacheATraiter.filtreTacheNouveau Is Nothing Then
                    filterTache = frmFiltreTacheATraiter.filtreTacheNouveau
                    refreshPanelFilter()
                    refreshGridTacheATraiter()
                End If
            End Using
        Catch err As Exception
            MsgBox(err.Message())
        Finally
            Me.Enabled = True

        End Try
    End Sub

    Private Sub refreshGridTacheATraiter(Optional chk As Integer = -1)
        Dim exId As Long, index As Integer = -1, exPosit = 0
        Dim dateRdv As Date, strToolTip As String
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim data As DataTable = tacheDao.getAllTacheATraiter(lstFonctionChoisie, filterTache)
            chkATraite = If(chk <> -1, chk, tacheDao.getAllTacheATraiterChk(lstFonctionChoisie, filterTache)) '' refixe valeur chk

            Dim numRowGrid As Integer = 0

            ' -- recup eventuelle precedente selectionnée
            If RadTacheToTreatGrid.Rows.Count > 0 AndAlso Not IsNothing(Me.RadTacheToTreatGrid.CurrentRow) Then
                exId = Me.RadTacheToTreatGrid.CurrentRow.Cells("id").Value
                exPosit = Me.RadTacheToTreatGrid.CurrentRow.Index
            End If
            RadTacheToTreatGrid.Rows.Clear()

            For Each row In data.Rows
                RadTacheToTreatGrid.Rows.Add(numRowGrid)
                '------------------- Alimentation du DataGridView
                With RadTacheToTreatGrid.Rows(numRowGrid)
                    .Cells("id").Value = row("id")
                    If .Cells("id").Value = exId Then index = numRowGrid   ' position exact
                    If numRowGrid <= exPosit Then exPosit = numRowGrid     ' position approchée 
                    .Cells("patient_nom").Value = row("patient_nom") + " " + row("patient_prenom")
                    .Cells("site_description").Value = row("site_description")
                    .Cells("typetache").Value = row("type") & If(row("nature") <> row("type"), "/" + row("nature"), "")
                    .Cells("typePicto").Value = getPictoType(.Cells("typetache").Value, row("priorite"))

                    strToolTip = " << " & Tache.getLibelleTacheNature(row("type"), row("nature")) & " >>" & vbCrLf

                    If row("type") = "RDV" OrElse row("type") = "RDV_MISSION" OrElse row("type") = "REUNION_STAFF" Then
                        Try
                            dateRdv = row("date_rendez_vous")
                            .Cells("heureRdv").Value = dateRdv.ToString("HH:mm")
                            If dateRdv < Now Then
                                .Cells("heureRdv").Style.ForeColor = Color.Red
                            End If
                            strToolTip += dateRdv.ToString("dd/MM/yyyy à HH:mm") & vbCrLf
                        Catch ex As Exception

                        End Try
                    End If
                    RadTacheToTreatGrid.Rows.Last.Tag = strToolTip &
                                                        .Cells("patient_nom").Value & vbCrLf &
                                                        "---- Emetteur ----" & vbCrLf &
                                                        row("emetteur_fonction") & vbCrLf &
                                                        If(Coalesce(row("emetteur_commentaire"), "") <> "", " ---- Commentaire ----" & vbCrLf & row("emetteur_commentaire"), "")
                End With

                numRowGrid += 1

            Next
            ' -- positionnement a la ligne la plus proche de la precedente
            If data.Rows.Count > 0 Then
                'Me.RadTacheToTreatGrid.CurrentRow = RadTacheToTreatGrid.ChildRows(If(index >= 0, index, exPosit))
                Me.RadTacheToTreatGrid.CurrentRow = RadTacheToTreatGrid.Rows(If(index >= 0, index, exPosit))
            End If
            Me.Cursor = Cursors.Default

            ' --- refresh eventuels tache en cours si 
            If RadioTachesFonctionEnCours.IsChecked Then
                refreshGridTacheEnCours()
            End If

        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try


    End Sub

    Private Function getPictoType(typeStr As String, priorite As Integer) As Image
        Return ImageList1.Images(typeStr + If(typeStr.StartsWith("AVIS"), "/" & priorite, ""))
    End Function

    Private Sub RadTextBox1_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RadTextBox1.ToolTipTextNeeded
        Dim rtbe As RadTextBoxElement = DirectCast(Me.RadTextBox1.RootElement.Children(0), RadTextBoxElement)
        rtbe.TextBoxItem.AutoToolTip = True
        rtbe.TextBoxItem.ToolTipText = filterTache.ResumeFiltre
        Me.RadTextBox1.ShowItemToolTips = True

    End Sub

    Private Sub btnRafraichirTAT_Click(sender As Object, e As EventArgs) Handles btnRafraichirTAT.Click
        refreshAll()
    End Sub

    Private Sub refreshPanelFilter()
        Dim i As Integer, j As Integer
        Me.RadTextBox1.Text = filterTache.ResumeFiltre
        j = Me.RadTextBox1.Lines.Length
        If j <= 0 Then j = 1
        i = 50 + ((j + 1) * 14)
        If i > 138 Then i = 138
        Me.RadPanelFiltre.Height = i
    End Sub

#End Region

#Region "Taches en cours et emises"

    Private Sub PageTache_SelectedPageChanged(sender As Object, e As EventArgs) Handles PageTache.SelectedPageChanged
        refreshGridDroitOngletActif()
    End Sub

    Private Sub refreshGridDroitOngletActif()
        Select Case PageTache.SelectedPage.Name
            Case "PageTachesEnCours"
                refreshGridTacheEnCours()
            Case "PageTachesEmises"
                refreshGridTacheEmise()
        End Select

    End Sub

    Private Sub refreshGridTacheEnCours()
        Dim dateRdv As Date, strToolTip As String
        Try
            Me.Cursor = Cursors.WaitCursor

            Dim isMyTache As Boolean = RadioMesTachesEnCours.IsChecked
            Dim data As DataTable = tacheDao.GetAllTacheEnCours(isMyTache, lstFonctionChoisie, filterTache, RadChkNonAttribuee.IsChecked, userLog)
            Dim numRowGrid As Integer = 0
            Dim exId As Long, index As Integer = -1, exPosit = 0
            Dim typeTache As String
            ' -- recup eventuelle precedente selectionnée
            If RadTacheEnCoursGrid.Rows.Count > 0 AndAlso Not IsNothing(Me.RadTacheEnCoursGrid.CurrentRow) Then
                exId = Me.RadTacheEnCoursGrid.CurrentRow.Cells("id").Value
            End If
            RadTacheEnCoursGrid.Rows.Clear()

            'RadTacheEnCoursGrid.Columns("user_traiteur").IsVisible = Not isMyTache
            'RadTacheEnCoursGrid.Columns("fonction_traiteur").IsVisible = Not isMyTache

            For Each row In data.Rows
                RadTacheEnCoursGrid.Rows.Add(numRowGrid)
                typeTache = row("type")
                '------------------- Alimentation du DataGridView
                With RadTacheEnCoursGrid.Rows(numRowGrid)
                    ' .Cells("categorie").Value = row("categorie")
                    .Cells("id").Value = row("id")
                    If .Cells("id").Value = exId Then index = numRowGrid   ' position exact
                    If numRowGrid <= exPosit Then exPosit = numRowGrid     ' position approchée 
                    .Cells("patient_nom").Value = row("patient_nom") + " " + row("patient_prenom")
                    .Cells("site_description").Value = row("site_description")
                    '.Cells("type").Value = Tache.getLibelleTacheNature(row("type"), row("nature")) +
                    '    If(typeTache = TacheDao.TypeTache.RDV.ToString OrElse typeTache = TacheDao.TypeTache.RDV_MISSION.ToString OrElse typeTache = TacheDao.TypeTache.REUNION_STAFF.ToString OrElse typeTache = TacheDao.TypeTache.RDV_SPECIALISTE.ToString,
                    '    " le " + Strings.Left(row("date_rendez_vous").ToString(), row("date_rendez_vous").ToString().Length() - 3) _
                    '    , " ")
                    .Cells("typetache").Value = row("type") & If(row("nature") <> row("type"), "/" + row("nature"), "")
                    .Cells("typePicto").Value = getPictoType(.Cells("typetache").Value, row("priorite"))
                    .Cells("user_traiteur").Value = row("user_traiteur_nom") + " " + row("user_traiteur_prenom")
                    .Cells("fonction_traiteur").Value = row("traite_fonction")
                    strToolTip = " << " & Tache.getLibelleTacheNature(row("type"), row("nature")) & " >>" & vbCrLf

                    If row("type") = "RDV" OrElse row("type") = "RDV_MISSION" OrElse row("type") = "REUNION_STAFF" Then
                        Try
                            dateRdv = row("date_rendez_vous")
                            .Cells("heureRdv").Value = dateRdv.ToString("HH:mm")
                            If dateRdv < Now Then
                                .Cells("heureRdv").Style.ForeColor = Color.Red
                            End If
                            strToolTip += dateRdv.ToString("dd/MM/yyyy à HH:mm") & vbCrLf
                        Catch ex As Exception

                        End Try
                    End If
                    RadTacheEnCoursGrid.Rows.Last.Tag = strToolTip &
                                                        .Cells("patient_nom").Value & vbCrLf &
                                                        "---- Emetteur ----" & vbCrLf &
                                                        row("emetteur_fonction") & vbCrLf &
                                                        If(Coalesce(row("emetteur_commentaire"), "") <> "", " ---- Commentaire ----" & vbCrLf & row("emetteur_commentaire"), "")

                End With
                numRowGrid += 1

            Next
            ' -- positionnement a la ligne la plus proche de la precedente
            If data.Rows.Count > 0 Then
                Me.RadTacheEnCoursGrid.CurrentRow = RadTacheEnCoursGrid.Rows(If(index >= 0, index, exPosit))
            End If
            Me.Cursor = Cursors.Default

            refreshSchedulerVisible(True)

        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub

    Private Sub RadTacheToTreatGrid_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadTacheToTreatGrid.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            ' e.CellElement.Padding = New Padding(5, 0, 0, 0)
            Try
                If e.Row.Tag <> Nothing Then e.CellElement.ToolTipText = e.Row.Tag '.ToString
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub RadTacheEnCoursGrid_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadTacheEnCoursGrid.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            ' e.CellElement.Padding = New Padding(5, 0, 0, 0)
            Try
                If e.Row.Tag <> Nothing Then e.CellElement.ToolTipText = e.Row.Tag '.ToString
            Catch ex As Exception

            End Try
        End If
    End Sub

    Private Sub RadTacheEmiseGrid_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadTacheEmiseGrid.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            ' e.CellElement.Padding = New Padding(5, 0, 0, 0)
            Try
                If e.Row.Tag <> Nothing Then e.CellElement.ToolTipText = e.Row.Tag '.ToString
            Catch ex As Exception

            End Try
        End If
    End Sub
    Private Sub RadTacheEnCoursGrid_CellPaint(sender As Object, e As GridViewCellPaintEventArgs) Handles RadTacheEnCoursGrid.CellPaint
        traceTriangle(sender, e)
    End Sub

    Private Sub traceTriangle(sender As Object, e As GridViewCellPaintEventArgs)
        If e.Cell IsNot Nothing AndAlso TypeOf e.Cell.RowInfo Is GridViewDataRowInfo AndAlso e.Cell.ColumnInfo.Name.ToLower.StartsWith("type") Then
            Dim brush As Brush
            brush = Brushes.Transparent.Clone()
            Select Case e.Cell.RowElement.RowInfo.Cells("typetache").Value
                Case Tache.TypeTache.RDV.ToString()
                    brush.Dispose()
                    brush = Brushes.LightGreen.Clone()
                Case Tache.TypeTache.RDV_DEMANDE.ToString(), Tache.TypeTache.REUNION_STAFF.ToString
                    brush.Dispose()
                    brush = Brushes.LightGray.Clone()
                Case Tache.TypeTache.RDV_MISSION.ToString()
                    brush.Dispose()
                    brush = Brushes.LightSalmon.Clone()
                Case Tache.TypeTache.RDV_SPECIALISTE.ToString()
                    brush.Dispose()
                    brush = New SolidBrush(Color.FromArgb(180, 160, 223))
            End Select
            e.Graphics.FillPolygon(brush, {New Point(1, 1), New Point(10, 1), New Point(1, 10), New Point(1, 1)})
            brush.Dispose()
        End If

    End Sub
    Private Sub RadTacheToTreatGrid_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles RadTacheToTreatGrid.MouseDoubleClick
        If Me.RadTacheToTreatGrid.Rows.Count = 0 OrElse Me.RadTacheToTreatGrid.CurrentRow.IsSelected = False Then Return
        Dim idTache As Long = Me.RadTacheToTreatGrid.CurrentRow.Cells("id").Value
        prendreTacheATraiter(idTache)
    End Sub

    Private Sub RadTacheToTreatGrid_MouseClick(sender As Object, e As MouseEventArgs) Handles RadTacheToTreatGrid.MouseClick
        If e.Button = MouseButtons.Right Then
            If Me.RadTacheToTreatGrid.Rows.Count = 0 OrElse Me.RadTacheToTreatGrid.CurrentRow.IsSelected = False Then Return
            ' -- menu contextuel
            Dim p As Point = (TryCast(sender, Control)).PointToScreen(e.Location)
            CtxMenuGridTacheATraiter.Show(p.X, p.Y)
        End If
    End Sub

    Private Sub MnuItemPrendreTacheATraiter_Click(sender As Object, e As EventArgs) Handles MnuItemPrendreTacheATraiter.Click
        ' -- on récupère la tache choisie
        Dim idTache As Long = Me.RadTacheToTreatGrid.CurrentRow.Cells("id").Value
        prendreTacheATraiter(idTache)
    End Sub

    Private Sub prendreTacheATraiter(idTache As Long)
        If tacheDao.AttribueTacheToUserLog(idTache, userLog) Then
            refreshGridTacheATraiter()
            refreshGridTacheEnCours()
        End If
        goToDetailTache(idTache)

    End Sub

    Private Sub MnuItemDetailTacheATraiter_Click(sender As Object, e As EventArgs) Handles MnuItemDetailTacheATraiter.Click
        goToDetailTache(RadTacheToTreatGrid)

    End Sub

    Private Sub goToDetailTache(radgrid As RadGridView)

        If radgrid.Rows.Count = 0 OrElse radgrid.CurrentRow.IsSelected = False Then Return
        ' -- on récupère la tache choisie
        Dim idTache As Long = radgrid.CurrentRow.Cells("id").Value
        goToDetailTache(idTache)
    End Sub

    Private Sub goToDetailTache(idTache As Long)
        ' -- on part sur le formulaire detail de tache pour la tache choisie
        Me.Cursor = Cursors.WaitCursor
        Dim isRefreshToDo As Boolean = False
        Try
            Me.Enabled = False
            Using frmTacheDetail = New FrmTacheDetail_vb(idTache)
                frmTacheDetail.ShowDialog()
                isRefreshToDo = frmTacheDetail.isActionEffectuee
            End Using
        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End Try
        ' -- refresh des grids tache et rdv
        If isRefreshToDo Then refreshAll()

    End Sub

    Private Sub refreshAll()
        ' -- refresh des grids tache et rdv
        refreshGridTacheATraiter()
        refreshGridDroitOngletActif()
        'refreshSchedulerVisible(True)
    End Sub

    Private Sub RadTacheEnCoursGrid_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles RadTacheEnCoursGrid.MouseDoubleClick
        goToDetailTache(RadTacheEnCoursGrid)
    End Sub

    Private Sub RadTacheEmiseGrid_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles RadTacheEmiseGrid.MouseDoubleClick
        goToDetailTache(RadTacheEmiseGrid)
    End Sub

    Private Sub RadioMesTaches_CheckStateChanged(sender As Object, e As EventArgs) Handles RadioMesTachesEnCours.CheckStateChanged
        If RadioMesTachesEnCours.IsChecked Then refreshGridDroitOngletActif()
    End Sub

    Private Sub RadioTachesFonctionEnCours_CheckStateChanged(sender As Object, e As EventArgs) Handles RadioTachesFonctionEnCours.CheckStateChanged
        If RadioTachesFonctionEnCours.IsChecked Then refreshGridDroitOngletActif()
    End Sub
    Private Sub RadioTacheEmiseEnCours_CheckStateChanged(sender As Object, e As EventArgs) Handles RadioTacheEmiseEnCours.CheckStateChanged
        If RadioTacheEmiseEnCours.IsChecked AndAlso PageTache.SelectedPage.Name = "PageTachesEmises" Then
            refreshGridDroitOngletActif()
        End If
    End Sub
    Private Sub RadioTacheEmiseToute_CheckStateChanged(sender As Object, e As EventArgs) Handles RadioTacheEmiseToute.CheckStateChanged
        If RadioTacheEmiseToute.IsChecked Then refreshGridDroitOngletActif()
    End Sub

    Private Sub RadScheduler1_MouseDoubleClick(sender As Object, e As MouseEventArgs) Handles RadScheduler1.MouseDoubleClick
        Dim appointmentElement As AppointmentElement = TryCast(Me.RadScheduler1.ElementTree.GetElementAtPoint(e.Location), AppointmentElement)

        If appointmentElement IsNot Nothing Then
            Dim appointment As AppointmentOasis = TryCast(appointmentElement.Appointment.DataItem, AppointmentOasis)
            goToDetailTache(appointment.Id)
        End If
    End Sub
    Private Sub RadScheduler1_ToolTipTextNeeded(sender As Object, e As ToolTipTextNeededEventArgs) Handles RadScheduler1.ToolTipTextNeeded
        Dim AppointmentElement As AppointmentElement = TryCast(sender, AppointmentElement)

        If appointmentElement IsNot Nothing Then
            Dim appointment As AppointmentOasis = TryCast(appointmentElement.Appointment.DataItem, AppointmentOasis)
            e.ToolTipText = appointment.Subject + vbCrLf + "Patient : " + appointment.Description + vbCrLf + "Début : " + appointment.Start + vbCrLf + "Fin    : " + appointment.End
        End If

    End Sub

    Private Sub RadTacheToTreatGrid_CellPaint(sender As Object, e As GridViewCellPaintEventArgs) Handles RadTacheToTreatGrid.CellPaint
        traceTriangle(sender, e)
    End Sub

    Private Sub RadTacheEmiseGrid_CellPaint(sender As Object, e As GridViewCellPaintEventArgs) Handles RadTacheEmiseGrid.CellPaint
        traceTriangle(sender, e)
    End Sub

    Private Sub BtnCancel_Click(sender As Object, e As EventArgs) Handles BtnCancel.Click
        Close()
    End Sub

    Private Sub TimerRefreshTaches_Tick(sender As Object, e As EventArgs) Handles TimerRefreshTaches.Tick
        Dim chk As Integer
        If Me.Enabled AndAlso Me.Cursor <> Cursors.WaitCursor Then
            Try
                TimerRefreshTaches.Stop()
                'Console.WriteLine("---> Début timer " & Date.Now)
                ' -- controle chk tache à traiter
                chk = tacheDao.getAllTacheATraiterChk(lstFonctionChoisie, filterTache)
                If chk <> chkATraite Then
                    'Console.WriteLine("         .. refresh tache à traiter")
                    refreshGridTacheATraiter(chk)
                End If
                'Console.WriteLine("---> Fin timer " & Date.Now)
            Catch err As Exception
            Finally
                TimerRefreshTaches.Start()
            End Try
        End If
    End Sub

    Private Sub RadChkNonAttribuee_CheckStateChanged(sender As Object, e As EventArgs) Handles RadChkNonAttribuee.CheckStateChanged
        If RadioTachesFonctionEnCours.IsChecked Then
            refreshGridDroitOngletActif()
        End If
    End Sub

    Private Sub refreshGridTacheEmise()
        Dim dateRdv As Date, strToolTip As String
        Dim exId As Long, index As Integer = -1, exPosit = 0
        Dim typeTache As String
        Me.Cursor = Cursors.WaitCursor
        Try
            Dim isNotFinal As Boolean = RadioTacheEmiseEnCours.IsChecked
            Dim data As DataTable = tacheDao.GetAllTacheEmise(isNotFinal, userLog)
            Dim numRowGrid As Integer = 0

            ' -- recup eventuelle precedente selectionnée
            If RadTacheEmiseGrid.Rows.Count > 0 AndAlso Not IsNothing(Me.RadTacheEmiseGrid.CurrentRow) Then
                exId = Me.RadTacheEmiseGrid.CurrentRow.Cells("id").Value
            End If
            RadTacheEmiseGrid.Rows.Clear()

            For Each row In data.Rows
                typeTache = row("Type")
                RadTacheEmiseGrid.Rows.Add(numRowGrid)
                With RadTacheEmiseGrid.Rows(numRowGrid)
                    '------------------- Alimentation du DataGridView
                    ' RadTacheEmiseGrid.Rows(numRowGrid).Cells("categorie").Value = row("categorie")
                    .Cells("id").Value = row("id")
                    If .Cells("id").Value = exId Then index = numRowGrid   ' position exact
                    If numRowGrid <= exPosit Then exPosit = numRowGrid     ' position approchée 
                    .Cells("Etat").Value = row("etat")
                    .Cells("patient_nom").Value = row("patient_nom") + " " + row("patient_prenom")
                    .Cells("site_description").Value = row("site_description")
                    '.Cells("type").Value = Tache.getLibelleTacheNature(row("type"), row("nature")) +
                    '    If(typeTache = TacheDao.TypeTache.RDV.ToString OrElse typeTache = TacheDao.TypeTache.RDV_MISSION.ToString OrElse typeTache = TacheDao.TypeTache.REUNION_STAFF.ToString OrElse typeTache = TacheDao.TypeTache.RDV_SPECIALISTE.ToString,
                    '    " le " + Strings.Left(row("date_rendez_vous").ToString(), row("date_rendez_vous").ToString().Length() - 3) _
                    '    , " ")
                    .Cells("typetache").Value = row("type") & If(row("nature") <> row("type"), "/" + row("nature"), "")
                    .Cells("typePicto").Value = getPictoType(.Cells("typetache").Value, row("priorite"))
                    .Cells("user_traiteur").Value = row("user_traiteur_nom") + " " + row("user_traiteur_prenom")
                    .Cells("fonction_traiteur").Value = row("user_traiteur_fonction")
                    strToolTip = " << " & Tache.getLibelleTacheNature(row("type"), row("nature")) & " >>" & vbCrLf

                    If row("type") <> "RDV_DEMANDE" AndAlso row("type").ToString.StartsWith("RDV") OrElse row("type") = "REUNION_STAFF" Then
                        Try
                            dateRdv = row("date_rendez_vous")
                            .Cells("heureRdv").Value = dateRdv.ToString("HH:mm")
                            If dateRdv < Now Then
                                .Cells("heureRdv").Style.ForeColor = Color.Red
                            End If
                            strToolTip += dateRdv.ToString("dd/MM/yyyy à HH:mm") & vbCrLf
                        Catch ex As Exception

                        End Try
                    End If
                    RadTacheEmiseGrid.Rows.Last.Tag = strToolTip &
                                                        .Cells("patient_nom").Value & vbCrLf &
                                                        "---- Emetteur ----" & vbCrLf &
                                                        row("emetteur_fonction") & vbCrLf &
                                                        If(Coalesce(row("emetteur_commentaire"), "") <> "", " ---- Commentaire ----" & vbCrLf & row("emetteur_commentaire"), "")


                End With

                numRowGrid += 1

            Next
            ' -- positionnement a la ligne la plus proche de la precedente
            If data.Rows.Count > 0 Then
                Me.RadTacheEmiseGrid.CurrentRow = RadTacheEmiseGrid.Rows(If(index >= 0, index, exPosit))
            End If
            Me.Cursor = Cursors.Default
        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try

    End Sub




#End Region
End Class
