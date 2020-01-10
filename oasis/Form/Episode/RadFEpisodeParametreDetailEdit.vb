Imports Telerik.WinControls
Imports Telerik.WinControls.UI

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

    Dim episodeParametreDao As New EpisodeParametreDao

    Dim listeParametreEpisode As New List(Of Long)

    Dim FinChargementParametres As Boolean = False

    Private Sub RadFParametreDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.TextAndButtonsElement.TextElement.ForeColor = Color.Red
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.BackColor = Color.DarkBlue
        Me.RadDesktopAlert1.Popup.AlertElement.CaptionElement.CaptionGrip.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.Font = New Font("Arial", 8.0F, FontStyle.Italic)
        Me.RadDesktopAlert1.Popup.AlertElement.ContentElement.TextImageRelation = TextImageRelation.TextBeforeImage
        Me.RadDesktopAlert1.Popup.AlertElement.BackColor = Color.MistyRose
        Me.RadDesktopAlert1.Popup.AlertElement.GradientStyle = GradientStyles.Solid
        Me.RadDesktopAlert1.Popup.AlertElement.BorderColor = Color.DarkBlue

        afficheTitleForm(Me, "Saisie paramètres")
        ChargementParametres()

    End Sub

    Private Sub ChargementParametres()
        FinChargementParametres = False

        Dim parmDataTable As DataTable
        parmDataTable = episodeParametreDao.getAllParametreEpisodeByEpisodeId(SelectedEpisodeId)
        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i, entier, nombreDecimal As Integer
        Dim Valeur As Decimal
        Dim ValeurString, definition As String
        Dim ParametreAjoute As Boolean
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = parmDataTable.Rows.Count - 1
        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            If parmDataTable.Rows(i)("parametre_id") = 3 Or parmDataTable.Rows(i)("parametre_id") = 8 Then
                Continue For
            End If
            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadGridViewParm.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadGridViewParm.Rows(iGrid).Cells("episode_parametre_id").Value = parmDataTable.Rows(i)("episode_parametre_id")
            RadGridViewParm.Rows(iGrid).Cells("parametre_id").Value = parmDataTable.Rows(i)("parametre_id")

            Dim parametreId As Long = parmDataTable.Rows(i)("parametre_id")
            RadGridViewParm.Rows(iGrid).Cells("valeur").Value = parmDataTable.Rows(i)("valeur")

            RadGridViewParm.Rows(iGrid).Cells("parametre_ajoute").Value = parmDataTable.Rows(i)("parametre_ajoute")
            ParametreAjoute = Coalesce(parmDataTable.Rows(i)("parametre_ajoute"), False)
            RadGridViewParm.Rows(iGrid).Cells("ajoute").Value = ""
            If ParametreAjoute = True Then
                RadGridViewParm.Rows(iGrid).Cells("ajoute").Value = "+"
            End If
            Valeur = parmDataTable.Rows(i)("valeur")

            'Console.WriteLine("Id : " & parametreId.ToString & " ajout : " & parmDataTable.Rows(i)("parametre_ajoute"))

            'RadGridViewParm.Rows(iGrid).Cells("valeurInput").Tag = ""
            RadGridViewParm.Rows(iGrid).Cells("description").Value = parmDataTable.Rows(i)("description")
            RadGridViewParm.Rows(iGrid).Cells("entier").Value = parmDataTable.Rows(i)("entier")
            entier = parmDataTable.Rows(i)("entier")
            RadGridViewParm.Rows(iGrid).Cells("decimal").Value = parmDataTable.Rows(i)("decimal")
            nombreDecimal = parmDataTable.Rows(i)("decimal")
            RadGridViewParm.Rows(iGrid).Cells("unite").Value = parmDataTable.Rows(i)("unite")
            definition = ""
            ValeurString = ""
            Select Case entier
                Case 1
                    Select Case nombreDecimal
                        Case 0
                            ValeurString = Valeur.ToString("0")
                            definition = "0"
                        Case 1
                            ValeurString = Valeur.ToString("0.0")
                            definition = "0,0"
                        Case 2
                            ValeurString = Valeur.ToString("0.00")
                            definition = "0,00"
                        Case 3
                            ValeurString = Valeur.ToString("0.000")
                            definition = "0,000"
                    End Select
                Case 2
                    Select Case nombreDecimal
                        Case 0
                            ValeurString = Valeur.ToString("#0")
                            definition = "00"
                        Case 1
                            ValeurString = Valeur.ToString("#0.0")
                            definition = "00,0"
                        Case 2
                            ValeurString = Valeur.ToString("#0.00")
                            definition = "00,00"
                        Case 3
                            ValeurString = Valeur.ToString("#0.000")
                            definition = "00,000"
                    End Select
                Case 3
                    Select Case nombreDecimal
                        Case 0
                            ValeurString = Valeur.ToString("##0")
                            definition = "000"
                        Case 1
                            ValeurString = Valeur.ToString("##0.0")
                            definition = "000,0"
                        Case 2
                            ValeurString = Valeur.ToString("##0.00")
                            definition = "000,00"
                        Case 3
                            ValeurString = Valeur.ToString("##0.000")
                            definition = "000,000"
                    End Select
            End Select
            RadGridViewParm.Rows(iGrid).Cells("valeurInput").Value = ValeurString
            If Valeur = 0 Then
                RadGridViewParm.Rows(iGrid).Cells("valeurInput").Style.ForeColor = Color.Red
                RadGridViewParm.Rows(iGrid).Cells("description").Style.ForeColor = Color.Red
                RadGridViewParm.Rows(iGrid).Cells("unite").Style.ForeColor = Color.Red
            End If
            If listeParametreEpisode.Contains(parametreId) = False Then
                listeParametreEpisode.Add(parametreId)
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewParm.Rows.Count > 0 Then
            Me.RadGridViewParm.CurrentRow = RadGridViewParm.ChildRows(0)
        End If

        ToolTip.SetToolTip(RadBtnSupprimer, "Seuls les paramètres ajoutés peuvent être supprimés")

        FinChargementParametres = True
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

    Private Sub Validation()
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
                'Mise à jour du paramètre
                'Console.WriteLine("Id : " & id.ToString & " Valeur saisie : " & valeurInput.ToString & " valeur initiale : " & valeur.ToString)
                episodeParametreDao.ModificationValeurEpisodeParametre(id, valeurInput)
                MiseAJour = True
            End If
        Next

        If MiseAJour = True Then
            Me.RadDesktopAlert1.CaptionText = "Notification saisie paramètres patient"
            Me.RadDesktopAlert1.ContentText = "Paramètres de l'épisode mis à jour"
            Me.RadDesktopAlert1.Show()
            'Rechargement grid
            RadGridViewParm.Rows.Clear()
            ChargementParametres()
            CodeRetour = True
        End If
    End Sub

    Private Sub RadBtnAjouter_Click(sender As Object, e As EventArgs) Handles RadBtnAjouter.Click
        Me.Enabled = False
        Using form As New RadFParametreSelecteur
            form.ListeParametreExistant = listeParametreEpisode
            form.ShowDialog()
            If form.IsSelected = True Then
                'Création paramètre
                Try
                    Dim episodeParametre As New EpisodeParametre
                    episodeParametre.ParametreId = form.SelectedParametre.Id
                    episodeParametre.EpisodeId = SelectedEpisodeId
                    episodeParametre.PatientId = SelectedPatient.patientId
                    episodeParametre.Valeur = 0
                    episodeParametre.Description = form.SelectedParametre.Description
                    episodeParametre.Entier = form.SelectedParametre.Entier
                    episodeParametre.Decimal = form.SelectedParametre.Decimal
                    episodeParametre.Unite = form.SelectedParametre.Unite
                    episodeParametre.ParametreAjoute = True
                    episodeParametre.Inactif = False
                    episodeParametreDao.CreateEpisodeParametre(episodeParametre)
                Catch ex As Exception
                    CreateLog(ex.ToString, Me.Name, LogDao.EnumTypeLog.ERREUR.ToString)
                    If ex.Message.StartsWith("Collisio") = True Then
                        MessageBox.Show("paramètre déjà existant pour cet épisode !")
                    End If
                End Try
                RadGridViewParm.Rows.Clear()
                ChargementParametres()
                CodeRetour = True
            End If
        End Using
        Me.Enabled = True
    End Sub

    Private Sub MasterTemplate_CellValueChanged(sender As Object, e As GridViewCellEventArgs) Handles RadGridViewParm.CellValueChanged
        If FinChargementParametres = True Then
            Validation()
        End If
    End Sub

    Private Sub RadBtnSupprimer_Click(sender As Object, e As EventArgs) Handles RadBtnSupprimer.Click
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
                    CodeRetour = True
                End If
            Else
                MessageBox.Show("Seuls les paramètres ajoutés peuvent être supprimés")
            End If
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
End Class
