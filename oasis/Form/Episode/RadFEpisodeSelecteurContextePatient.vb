Imports Oasis_Common
Public Class RadFEpisodeSelecteurContextePatient
    Private _selectedEpisode As Episode
    Private _contexteId As Long
    Private _codeRetour As Boolean

    Public Property SelectedEpisode As Episode
        Get
            Return _selectedEpisode
        End Get
        Set(value As Episode)
            _selectedEpisode = value
        End Set
    End Property

    Public Property ContexteId As Long
        Get
            Return _contexteId
        End Get
        Set(value As Long)
            _contexteId = value
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

    Dim InitContextePublie As Boolean = False
    Dim SelectedContexteId As Long = 0

    Dim SelectedPatient As Patient

    Private Sub RadFEpisodeSelecteurContextePatient_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementEtatCivil()
        RadChkContextePublie.Checked = True

        ChargementContexte()

    End Sub

    Private Sub ChargementContexte()
        RadContexteDataGridView.Rows.Clear()

        SelectedContexteId = 0
        TxtContexte.Text = ""
        RadGbxContexteSelection.Hide()

        Dim contexteDataTable As DataTable
        Dim antecedentDao As AntecedentDao = New AntecedentDao
        If RadChkContextePublie.Checked = True Then
            contexteDataTable = antecedentDao.GetContextebyPatient(SelectedEpisode.PatientId, True)
        Else
            contexteDataTable = antecedentDao.GetContextebyPatient(SelectedEpisode.PatientId, False)
        End If

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateFin, dateModification As Date
        Dim AfficheDateModification, diagnostic As String
        Dim ordreAffichage As Integer
        Dim rowCount As Integer = contexteDataTable.Rows.Count - 1
        Dim categorieContexte, categorieContexteString As String
        Dim contexteCache As Boolean

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            categorieContexte = ""
            If contexteDataTable.Rows(i)("oa_antecedent_categorie_contexte") IsNot DBNull.Value Then
                categorieContexte = contexteDataTable.Rows(i)("oa_antecedent_categorie_contexte")
            End If
            Select Case categorieContexte
                Case ContexteDao.EnumParcoursBaseCode.Medical
                    categorieContexteString = ContexteDao.EnumParcoursBaseItem.Medical
                Case ContexteDao.EnumParcoursBaseCode.BioEnvironnemental
                    categorieContexteString = ContexteDao.EnumParcoursBaseItem.BioEnvironnemental
                Case Else
                    categorieContexteString = ""
            End Select

            'DateFin
            If contexteDataTable.Rows(i)("oa_antecedent_date_fin") IsNot DBNull.Value Then
                dateFin = contexteDataTable.Rows(i)("oa_antecedent_date_fin")
            Else
                dateFin = "31/12/9999"
            End If

            'Recherche si le contexte a été modifié (médical uniquement)
            AfficheDateModification = ""
            If categorieContexte = "M" Then
                If contexteDataTable.Rows(i)("oa_antecedent_date_modification") IsNot DBNull.Value Then
                    dateModification = contexteDataTable.Rows(i)("oa_antecedent_date_modification")
                    AfficheDateModification = FormatageDateAffichage(dateModification) + " : "
                Else
                    If contexteDataTable.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                        dateModification = contexteDataTable.Rows(i)("oa_antecedent_date_creation")
                        AfficheDateModification = FormatageDateAffichage(dateModification) + " : "
                    End If
                End If
            End If

            'Affichage de l'ordre d'affichage
            If contexteDataTable.Rows(i)("oa_antecedent_ordre_affichage1") IsNot DBNull.Value Then
                ordreAffichage = contexteDataTable.Rows(i)("oa_antecedent_ordre_affichage1")
            Else
                ordreAffichage = 0
            End If

            'prefixeContexte = "(Ordre : " + ordreAffichage.ToString + ") - "

            'Contexte caché
            contexteCache = False
            If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") = "C" Then
                    contexteCache = True
                End If
            End If

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadContexteDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadContexteDataGridView.Rows(iGrid).Cells("categorieContexte").Value = categorieContexteString

            diagnostic = ""
            If contexteDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                    diagnostic = "Suspicion de "
                Else
                    If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 3 Then
                        diagnostic = "Notion de "
                    End If
                End If
            End If

            'Affichage contexte ==========================
            Dim longueurString As Integer
            Dim longueurMax As Integer = 150
            Dim contexteDescription As String
            contexteDescription = Coalesce(contexteDataTable.Rows(i)("oa_antecedent_description"), "")
            If contexteDescription <> "" Then
                contexteDescription = Replace(contexteDescription, vbCrLf, " ")
                longueurString = contexteDescription.Length
                If longueurString > longueurMax Then
                    longueurString = longueurMax
                End If
                contexteDescription.Substring(0, longueurString)
            End If

            RadContexteDataGridView.Rows(iGrid).Cells("contexte").Value = AfficheDateModification & diagnostic & " " & contexteDescription
            '============================================

            If contexteCache = True Then
                RadContexteDataGridView.Rows(iGrid).Cells("contexte").Style.ForeColor = Color.CornflowerBlue
            End If

            'RadContexteDataGridView.Rows(iGrid).Cells("contexte").

            'Identifiant contexte
            RadContexteDataGridView.Rows(iGrid).Cells("contexteId").Value = contexteDataTable.Rows(i)("oa_antecedent_id")
        Next

        'Positionnement du grid sur la première occurrence
        If RadContexteDataGridView.Rows.Count > 0 Then
            Me.RadContexteDataGridView.CurrentRow = RadContexteDataGridView.ChildRows(0)
        End If
    End Sub

    Private Sub ChargementEtatCivil()
        SelectedPatient = PatientDao.getPatientById(SelectedEpisode.PatientId)
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)
    End Sub

    Private Sub RadChkContextePublie_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkContextePublie.ToggleStateChanged
        If RadChkContextePublie.Checked = True Then
            RadChkContexteTous.Checked = False
            If InitContextePublie = True Then
                Application.DoEvents()
                ChargementContexte()
            Else
                InitContextePublie = True
            End If
        Else
            If RadChkContexteTous.Checked = False Then
                RadChkContextePublie.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkContexteTous_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RadChkContexteTous.ToggleStateChanged
        If RadChkContexteTous.Checked = True Then
            RadChkContextePublie.Checked = False
            Application.DoEvents()
            ChargementContexte()
        Else
            If RadChkContextePublie.Checked = False Then
                RadChkContexteTous.Checked = True
            End If
        End If
    End Sub

    Private Sub RadContexteDataGridView_CellClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadContexteDataGridView.CellClick
        If RadContexteDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadContexteDataGridView.Rows.IndexOf(Me.RadContexteDataGridView.CurrentRow)
            If aRow >= 0 Then
                SelectedContexteId = RadContexteDataGridView.Rows(aRow).Cells("ContexteId").Value
                TxtContexte.Text = RadContexteDataGridView.Rows(aRow).Cells("contexte").Value
                RadGbxContexteSelection.Show()
            End If
        End If
    End Sub

    Private Sub RadContexteDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadContexteDataGridView.CellDoubleClick
        If RadContexteDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadContexteDataGridView.Rows.IndexOf(Me.RadContexteDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ContexteId As Integer = RadContexteDataGridView.Rows(aRow).Cells("ContexteId").Value
                Cursor.Current = Cursors.WaitCursor
                Me.Enabled = False
                Using vFContexteDetailEdit As New RadFContextedetailEdit
                    vFContexteDetailEdit.SelectedContexteId = ContexteId
                    vFContexteDetailEdit.SelectedPatient = SelectedPatient
                    vFContexteDetailEdit.UtilisateurConnecte = userLog
                    vFContexteDetailEdit.SelectedDrcId = 0
                    vFContexteDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                    vFContexteDetailEdit.ShowDialog() 'Modal
                    If vFContexteDetailEdit.CodeRetour = True Then
                        Select Case vFContexteDetailEdit.CodeResultat
                            Case EnumResultat.AnnulationOK
                                Dim form As New RadFNotification()
                                form.Titre = "Notification contexte patient"
                                form.Message = "Contexte patient annulé"
                                form.Show()
                            Case EnumResultat.ModificationOK
                                Dim form As New RadFNotification()
                                form.Titre = "Notification contexte patient"
                                form.Message = "Contexte patient modifié"
                                form.Show()
                        End Select
                        ChargementContexte()
                    End If
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Créer un contexte
    Private Sub RadBtnCreation_MouseCaptureChanged(sender As Object, e As EventArgs) Handles RadBtnCreation.MouseCaptureChanged
        CreationContexte()
    End Sub

    Private Sub CréerUnContexteToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréerUnContexteToolStripMenuItem.Click
        CreationContexte()
    End Sub

    Private Sub CreationContexte()
        Dim SelectedDrcId As Integer
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFDrcSelecteur As New RadFDRCSelecteur
            vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
            vFDrcSelecteur.CategorieOasis = DrcDao.EnumCategorieOasisCode.Contexte
            vFDrcSelecteur.ShowDialog()
            SelectedDrcId = vFDrcSelecteur.SelectedDrcId
            'Si un médicament a été sélectionné, on appelle le Formulaire de création
            If SelectedDrcId <> 0 Then
                Using vFContexteDetailEdit As New RadFContextedetailEdit
                    vFContexteDetailEdit.SelectedPatient = SelectedPatient
                    vFContexteDetailEdit.UtilisateurConnecte = userLog
                    vFContexteDetailEdit.SelectedDrcId = SelectedDrcId
                    vFContexteDetailEdit.SelectedContexteId = 0
                    vFContexteDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                    vFContexteDetailEdit.ShowDialog()
                    'Si le traitement a été créé, on recharge la grid
                    If vFContexteDetailEdit.CodeRetour = True Then
                        Dim form As New RadFNotification()
                        form.Titre = "Notification contexte patient"
                        form.Message = "Contexte patient créé"
                        form.Show()
                        ChargementContexte()
                    End If
                End Using
            End If
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnSelection_Click(sender As Object, e As EventArgs) Handles RadBtnSelection.Click
        If SelectedContexteId <> 0 Then
            ContexteId = SelectedContexteId


            Close()
        End If
    End Sub

End Class
