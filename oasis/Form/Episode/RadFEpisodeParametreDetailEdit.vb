Imports Telerik.WinControls
Imports Telerik.WinControls.UI
Imports Oasis_Common
Imports System.Configuration
Imports System.Diagnostics

Public Class RadFEpisodeParametreDetailEdit
    Private _SelectedEpisodeId As Long
    Private _SelectedPatient As Patient
    Private _codeRetour As Boolean

    Public Property SelectedEpisodeId As Long
        Get
            Return _SelectedEpisodeId
        End Get
        Set(value As Long)
            _SelectedEpisodeId = value
        End Set
    End Property

    Public Property SelectedPatient As Patient
        Get
            Return _SelectedPatient
        End Get
        Set(value As Patient)
            _SelectedPatient = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return _codeRetour
        End Get
        Set(value As Boolean)
            _codeRetour = value
        End Set
    End Property

    ReadOnly episodeParametreDao As New EpisodeParametreDao
    ReadOnly parametreDao As New ParametreDao

    ReadOnly listeParametreEpisode As New List(Of Long)

    Dim FinChargementParametres As Boolean

    Dim ParametreIdTaille As Long
    Dim AgeAdulteHomme As Integer
    Dim AgeAdulteFemme As Integer

    'Dim ParmRowIndex As Integer
    Dim ParmRowCount As Integer

    Private Sub RadFParametreDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.TextAndButtonsElement.TextElement.ForeColor = Color.Red
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.BackColor = Color.DarkBlue
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.Font = New Font("Arial", 8.0F, FontStyle.Italic)
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.TextImageRelation = TextImageRelation.TextBeforeImage
        Me.RadDesktopAlert1.Popup.AlertElement.BackColor = Color.MistyRose
        Me.RadDesktopAlert1.Popup.AlertElement.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.BorderColor = Color.DarkBlue

        ChargementParametreApplication()

        AfficheTitleForm(Me, "Saisie paramètres", userLog)
        Me.Width = 491
        ChargementParametres()

    End Sub

    Private Sub ChargementParametreApplication()
        Dim ParametreIdTailleString As String = ConfigurationManager.AppSettings("ParametreIdTaille")
        Dim AgeAdulteHommeString As String = ConfigurationManager.AppSettings("AgeAdulteHomme")
        Dim AgeAdulteFemmeString As String = ConfigurationManager.AppSettings("AgeAdulteFemme")

        If IsNumeric(ParametreIdTailleString) Then
            ParametreIdTaille = CInt(ParametreIdTailleString)
        Else
            ParametreIdTaille = 2
            CreateLog("Paramètre application 'ParametreIdTaille' non trouvé !", Me.Name, Log.EnumTypeLog.ERREUR.ToString, userLog)
        End If

        If IsNumeric(AgeAdulteHommeString) Then
            AgeAdulteHomme = CInt(AgeAdulteHommeString)
        Else
            AgeAdulteHomme = 15
            CreateLog("Paramètre application 'AgeAdulteHomme' non trouvé !", Me.Name, Log.EnumTypeLog.ERREUR.ToString, userLog)
        End If

        If IsNumeric(AgeAdulteFemmeString) Then
            AgeAdulteFemme = CInt(AgeAdulteFemmeString)
        Else
            AgeAdulteFemme = 20
            CreateLog("Paramètre application 'AgeAdulteFemme' non trouvé !", Me.Name, Log.EnumTypeLog.ERREUR.ToString, userLog)
        End If
    End Sub

    Private Sub ChargementParametres(Optional Index As Integer = -1)
        FinChargementParametres = False
        RadGridViewParm.Rows.Clear()

        Dim parmDataTable As DataTable = episodeParametreDao.getAllParametreEpisodeByEpisodeId(SelectedEpisodeId)
        Dim i, entier, nombreDecimal As Integer
        Dim Valeur As Decimal?
        Dim ValeurString As String
        Dim ParametreAjoute As Boolean
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        ParmRowCount = parmDataTable.Rows.Count - 1
        Dim RowCount As Integer = parmDataTable.Rows.Count - 1
        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To RowCount Step 1
            Dim parametreId As Long = parmDataTable.Rows(i)("parametre_id")
            If listeParametreEpisode.Contains(parametreId) = False Then
                listeParametreEpisode.Add(parametreId)
            End If
            If parametreId = 3 Or parametreId = 8 Then   'IMC ou PAM qui sont des paramètres calculés
                ParmRowCount -= 1
                Continue For
            End If

            iGrid += 1
            RadGridViewParm.Rows.Add(iGrid)

            RadGridViewParm.Rows(iGrid).Cells("episode_parametre_id").Value = parmDataTable.Rows(i)("episode_parametre_id")
            RadGridViewParm.Rows(iGrid).Cells("parametre_id").Value = parmDataTable.Rows(i)("parametre_id")


            RadGridViewParm.Rows(iGrid).Cells("valeur").Value = parmDataTable.Rows(i)("valeur")

            RadGridViewParm.Rows(iGrid).Cells("parametre_ajoute").Value = parmDataTable.Rows(i)("parametre_ajoute")
            ParametreAjoute = Coalesce(parmDataTable.Rows(i)("parametre_ajoute"), False)
            RadGridViewParm.Rows(iGrid).Cells("ajoute").Value = ""
            If ParametreAjoute = True Then
                RadGridViewParm.Rows(iGrid).Cells("ajoute").Value = "+"
            End If
            Valeur = If(IsDBNull(parmDataTable.Rows(i)("valeur")), Nothing, parmDataTable.Rows(i)("valeur"))

            If parametreId = 2 Then 'Taille
                If Valeur Is Nothing Then
                    If SelectedPatient.Taille <> 0 Then
                        Valeur = SelectedPatient.Taille
                    End If
                End If
            End If

            RadGridViewParm.Rows(iGrid).Cells("description").Value = parmDataTable.Rows(i)("description")
            RadGridViewParm.Rows(iGrid).Cells("entier").Value = parmDataTable.Rows(i)("entier")
            entier = parmDataTable.Rows(i)("entier")
            RadGridViewParm.Rows(iGrid).Cells("decimal").Value = parmDataTable.Rows(i)("decimal")
            nombreDecimal = parmDataTable.Rows(i)("decimal")
            RadGridViewParm.Rows(iGrid).Cells("unite").Value = parmDataTable.Rows(i)("unite")

            ValeurString = ""
            If Valeur IsNot Nothing Then
                Select Case entier
                    Case 1
                        Select Case nombreDecimal
                            Case 0
                                ValeurString = Valeur.Value.ToString("0")
                            Case 1
                                ValeurString = Valeur.Value.ToString("0.0")
                            Case 2
                                ValeurString = Valeur.Value.ToString("0.00")
                            Case 3
                                ValeurString = Valeur.Value.ToString("0.000")
                        End Select
                    Case 2
                        Select Case nombreDecimal
                            Case 0
                                ValeurString = Valeur.Value.ToString("#0")
                            Case 1
                                ValeurString = Valeur.Value.ToString("#0.0")
                            Case 2
                                ValeurString = Valeur.Value.ToString("#0.00")
                            Case 3
                                ValeurString = Valeur.Value.ToString("#0.000")
                        End Select
                    Case 3
                        Select Case nombreDecimal
                            Case 0
                                ValeurString = Valeur.Value.ToString("##0")
                            Case 1
                                ValeurString = Valeur.Value.ToString("##0.0")
                            Case 2
                                ValeurString = Valeur.Value.ToString("##0.00")
                            Case 3
                                ValeurString = Valeur.Value.ToString("##0.000")
                        End Select
                End Select
            End If

            RadGridViewParm.Rows(iGrid).Cells("valeurInput").Value = ValeurString
            If IsNothing(Valeur) Then
                RadGridViewParm.Rows(iGrid).Cells("valeurInput").Style.ForeColor = Color.Red
                RadGridViewParm.Rows(iGrid).Cells("description").Style.ForeColor = Color.Red
                RadGridViewParm.Rows(iGrid).Cells("unite").Style.ForeColor = Color.Red
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If Index <> -1 Then
            If Index < RadGridViewParm.Rows.Count Then
                RadGridViewParm.CurrentRow = RadGridViewParm.ChildRows(Index)
                RadGridViewParm.Rows(Index).IsCurrent = True
                'RadGridViewParm.Columns(1).IsCurrent = True
                RadGridViewParm.CurrentRow.Cells(1).BeginEdit()
            End If
        Else
            If RadGridViewParm.Rows.Count > 0 Then
                Me.RadGridViewParm.CurrentRow = RadGridViewParm.ChildRows(0)
            End If
        End If

        ToolTip.SetToolTip(RadBtnSupprimer, "Seuls les paramètres ajoutés peuvent être supprimés")

        FinChargementParametres = True
    End Sub

    Private Sub ChargementParametresDisponibles()
        RadGridView1.Rows.Clear()

        Dim parmDataTable As DataTable
        parmDataTable = parametreDao.GetAllParametre()
        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = parmDataTable.Rows.Count - 1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            Dim parametreId As Long = parmDataTable.Rows(i)("id")
            If listeParametreEpisode.Contains(parametreId) Then
                Continue For
            End If
            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadGridView1.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadGridView1.Rows(iGrid).Cells("id").Value = parmDataTable.Rows(i)("id")
            RadGridView1.Rows(iGrid).Cells("description").Value = parmDataTable.Rows(i)("description")
            RadGridView1.Rows(iGrid).Cells("unite").Value = parmDataTable.Rows(i)("unite")
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridView1.Rows.Count > 0 Then
            Me.RadGridView1.CurrentRow = RadGridView1.ChildRows(0)
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Dim MiseAJour As Boolean = False
        For Each rowInfo As GridViewRowInfo In RadGridViewParm.Rows
            Dim valeurInput As Decimal = 0
            Dim valeur As Decimal = 0
            Dim id As Long
            For Each cellInfo As GridViewCellInfo In rowInfo.Cells
                If (cellInfo.ColumnInfo.Name = "valeurInput") Then
                    If cellInfo.Value <> Nothing Then
                        valeurInput = cellInfo.Value
                    Else
                        valeurInput = 0
                    End If
                End If
                If (cellInfo.ColumnInfo.Name = "valeur") Then
                    If cellInfo.Value <> Nothing Then
                        valeur = cellInfo.Value
                    Else
                        valeur = 0
                    End If
                End If
                If (cellInfo.ColumnInfo.Name = "episode_parametre_id") Then
                    If cellInfo.Value <> Nothing Then
                        id = cellInfo.Value
                    Else
                        id = 0
                    End If
                End If
            Next
            If valeurInput <> valeur Then
                MiseAJour = True
                Exit For
            End If
        Next
        If MiseAJour = True Then
            If MsgBox("Attention, la saisie de paramètre(s) n'a pas été validée, confirmez-vous la sortie sans validation", MsgBoxStyle.YesNo, "") = MsgBoxResult.No Then
                Validation()
            End If
        End If
        Close()
    End Sub

    Private Sub Validation(Optional row As GridViewCellEventArgs = Nothing)
        Dim MiseAJour As Boolean = False
        Dim ParmRowIndex As Integer = 0
        Dim patientDao As New PatientDao

        For Each rowInfo As GridViewRowInfo In RadGridViewParm.Rows
            Dim valeurInput As Decimal? = Nothing
            Dim valeur As Decimal? = Nothing
            Dim id, parametreId As Long

            If (row IsNot Nothing AndAlso row.RowIndex <> rowInfo.Index) Then
                Continue For
            End If

            For Each cellInfo As GridViewCellInfo In rowInfo.Cells
                If (cellInfo.ColumnInfo.Name = "valeurInput") Then
                    If Not String.IsNullOrEmpty(cellInfo.Value) Then
                        valeurInput = CDec(Val(cellInfo.Value))
                    Else
                        valeurInput = Nothing
                    End If
                End If
                If (cellInfo.ColumnInfo.Name = "valeur") Then
                    If Not String.IsNullOrEmpty(cellInfo.Value) Then
                        valeur = CDec(Val(cellInfo.Value))
                    Else
                        valeur = Nothing
                    End If
                End If
                If (cellInfo.ColumnInfo.Name = "episode_parametre_id") Then
                    If cellInfo.Value <> Nothing Then
                        id = cellInfo.Value
                    Else
                        id = 0
                    End If
                End If
                If (cellInfo.ColumnInfo.Name = "parametre_id") Then
                    If cellInfo.Value <> Nothing Then
                        parametreId = cellInfo.Value
                    Else
                        parametreId = 0
                    End If
                End If
            Next
            'If valeurInput IsNot Nothing Then
            episodeParametreDao.ModificationValeurEpisodeParametre(id, valeurInput)
            If parametreId = ParametreIdTaille Then
                Select Case SelectedPatient.PatientGenreId
                    Case "M"
                        If SelectedPatient.PatientAgeEnAnnee >= AgeAdulteHomme Then
                            patientDao.ModificationPatientTaille(SelectedPatient.PatientId, valeurInput)
                        End If
                    Case "F"
                        If SelectedPatient.PatientAgeEnAnnee >= AgeAdulteFemme Then
                            patientDao.ModificationPatientTaille(SelectedPatient.PatientId, valeurInput)
                        End If
                    Case Else
                        If SelectedPatient.PatientAgeEnAnnee >= AgeAdulteHomme Then
                            patientDao.ModificationPatientTaille(SelectedPatient.PatientId, valeurInput)
                        End If
                End Select
            End If
            MiseAJour = True
            If rowInfo.Index() >= ParmRowCount Then
                ParmRowIndex = 0
            Else
                ParmRowIndex = rowInfo.Index() + 1
            End If
            ' End If
        Next

        If MiseAJour = True Then
            Me.RadDesktopAlert1.CaptionText = "Notification saisie paramètres patient"
            Me.RadDesktopAlert1.ContentText = "Paramètres de l'épisode mis à jour"
            Me.RadDesktopAlert1.Show()

            'Rechargement grid
            ChargementParametres(ParmRowIndex)
            CodeRetour = True
        End If
    End Sub

    Private Sub RadBtnAjouter_Click(sender As Object, e As EventArgs) Handles RadBtnAjouter.Click
        'AjouterParametre()
        Me.Width = 925
        ChargementParametresDisponibles()
    End Sub

    Private Sub RadBtnCacher_Click(sender As Object, e As EventArgs) Handles RadBtnCacher.Click
        Me.Width = 491
    End Sub

    'Private Sub AjouterParametre()
    '    Me.Enabled = False
    '    Using form As New RadFParametreSelecteur
    '        form.ListeParametreExistant = listeParametreEpisode
    '        form.ShowDialog()
    '        If form.IsSelected = True Then
    '            'Création paramètre
    '            Try
    '                Dim episodeParametre As New EpisodeParametre With {
    '                    .ParametreId = form.SelectedParametre.Id,
    '                    .EpisodeId = SelectedEpisodeId,
    '                    .PatientId = SelectedPatient.PatientId,
    '                    .Valeur = 0,
    '                    .Description = form.SelectedParametre.Description,
    '                    .Entier = form.SelectedParametre.Entier,
    '                    .Decimal = form.SelectedParametre.Decimal,
    '                    .Unite = form.SelectedParametre.Unite,
    '                    .Ordre = form.SelectedParametre.Ordre,
    '                    .ParametreAjoute = True,
    '                    .Inactif = False
    '                }
    '                episodeParametreDao.CreateEpisodeParametre(episodeParametre)
    '            Catch ex As Exception
    '                CreateLog(ex.ToString, Me.Name, Log.EnumTypeLog.ERREUR.ToString, userLog)
    '                If ex.Message.StartsWith("Collisio") = True Then
    '                    MessageBox.Show("paramètre déjà existant pour cet épisode !")
    '                End If
    '            End Try
    '            RadGridViewParm.Rows.Clear()
    '            ChargementParametres()
    '            CodeRetour = True
    '        End If
    '    End Using
    '    Me.Enabled = True
    'End Sub


    'Ajouter un paramètre
    Private Sub RadBtnSelect_Click(sender As Object, e As EventArgs) Handles RadBtnSelect.Click
        Dim aRow, maxRow As Integer

        aRow = Me.RadGridView1.Rows.IndexOf(Me.RadGridView1.CurrentRow)
        maxRow = RadGridView1.Rows.Count - 1

        If aRow <= maxRow And aRow > -1 Then
            Dim SelectedParametreId As Long
            SelectedParametreId = RadGridView1.Rows(aRow).Cells("Id").Value
            'Création paramètre
            Dim SelectedParametre As Parametre
            SelectedParametre = parametreDao.GetParametreById(SelectedParametreId)
            Try
                Dim episodeParametre As New EpisodeParametre With {
                    .ParametreId = SelectedParametre.Id,
                    .EpisodeId = SelectedEpisodeId,
                    .PatientId = SelectedPatient.PatientId,
                    .Valeur = Nothing,
                    .Description = SelectedParametre.Description,
                    .Entier = SelectedParametre.Entier,
                    .Decimal = SelectedParametre.Decimal,
                    .Unite = SelectedParametre.Unite,
                    .Ordre = SelectedParametre.Ordre,
                    .ParametreAjoute = True,
                    .Inactif = False
                }
                If SelectedParametre.Id = ParametreIdTaille Then
                    episodeParametre.Valeur = SelectedPatient.Taille
                End If
                episodeParametreDao.CreateEpisodeParametre(episodeParametre)
            Catch ex As Exception
                CreateLog(ex.ToString, Me.Name, Log.EnumTypeLog.ERREUR.ToString, userLog)
                If ex.Message.StartsWith("Collisio") = True Then
                    MessageBox.Show("paramètre déjà existant pour cet épisode !")
                End If
            End Try
            ChargementParametres()
            ChargementParametresDisponibles()
            CodeRetour = True

        End If
    End Sub

    'Enlever un paramètre
    Private Sub RadBtnSuppprimer_Click(sender As Object, e As EventArgs) Handles RadBtnSuppprimer.Click
        SupprimerParametre()
    End Sub

    Private Sub RadBtnSupprimer_Click(sender As Object, e As EventArgs) Handles RadBtnSupprimer.Click
        SupprimerParametre()
    End Sub

    Private Sub SupprimerParametre()
        Dim aRow, maxRow As Integer

        aRow = Me.RadGridViewParm.Rows.IndexOf(Me.RadGridViewParm.CurrentRow)
        maxRow = RadGridViewParm.Rows.Count - 1

        If aRow <= maxRow And aRow > -1 Then
            Dim parametreAjoute As Boolean = RadGridViewParm.Rows(aRow).Cells("parametre_ajoute").Value
            Dim ParametreId As Long = RadGridViewParm.Rows(aRow).Cells("episode_parametre_id").Value
            If parametreAjoute = True Then
                If episodeParametreDao.AnnulationEpisodeParametre(ParametreId) = True Then
                    listeParametreEpisode.Remove(RadGridViewParm.Rows(aRow).Cells("parametre_id").Value)
                    RadGridViewParm.Rows.Clear()
                    ChargementParametres()
                    ChargementParametresDisponibles()
                    CodeRetour = True
                End If
            Else
                MessageBox.Show("Seuls les paramètres ajoutés peuvent être supprimés")
            End If
        End If
    End Sub

    Private Sub MasterTemplate_CellValueChanged(sender As Object, e As GridViewCellEventArgs) Handles RadGridViewParm.CellValueChanged
        If FinChargementParametres = True Then
            Validation(e)
        End If
    End Sub



    Private Sub MasterTemplate_ToolTipTextNeeded(sender As Object, e As ToolTipTextNeededEventArgs) Handles RadGridViewParm.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "ajoute" Then
            If hoveredCell.RowInfo.Cells("ajoute").Value IsNot Nothing AndAlso hoveredCell.RowInfo.Cells("ajoute").Value = "+" Then
                e.ToolTipText = "Paramètre ajouté"
            Else
                e.ToolTipText = ""
            End If
        End If
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "description" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("description").Value
        End If
    End Sub

    Private Sub TutorielToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles TutorielToolStripMenuItem.Click
        If RadGridViewParm.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewParm.Rows.IndexOf(Me.RadGridViewParm.CurrentRow)
            If aRow >= 0 Then
                Dim ParametreId As Integer = RadGridViewParm.Rows(aRow).Cells("parametre_id").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Try
                    Using form As New RadFTutoriel
                        form.parametreId = ParametreId
                        form.ShowDialog()
                    End Using
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadGridViewParm_KeyDown(
        ByVal sender As Object,
        ByVal e As System.Windows.Forms.KeyEventArgs) _
    Handles RadGridViewParm.KeyDown

        If e.KeyCode = Keys.Enter Then
            Dim row = RadGridViewParm.CurrentRow
            If row IsNot Nothing AndAlso row.Cells.Count > 2 Then
                row.Cells(1).BeginEdit()
            End If
        End If
    End Sub
End Class


