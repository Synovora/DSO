Imports Telerik.WinControls.UI
Imports Oasis_Common

Public Class RadFRorListe
    Private _selecteur As Boolean
    Private _patientId As Long
    Private _specialiteId As Integer
    Private _typeRor As String
    Private _selectedRorId As Integer
    Private _codeRetour As Boolean

    Public Property Selecteur As Boolean
        Get
            Return _selecteur
        End Get
        Set(value As Boolean)
            _selecteur = value
        End Set
    End Property

    Public Property SelectedRorId As Integer
        Get
            Return _selectedRorId
        End Get
        Set(value As Integer)
            _selectedRorId = value
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

    Public Property SpecialiteId As Integer
        Get
            Return _specialiteId
        End Get
        Set(value As Integer)
            _specialiteId = value
        End Set
    End Property

    Public Property TypeRor As String
        Get
            Return _typeRor
        End Get
        Set(value As String)
            _typeRor = value
        End Set
    End Property

    Public Property PatientId As Long
        Get
            Return _patientId
        End Get
        Set(value As Long)
            _patientId = value
        End Set
    End Property

    Dim RorIdDisplayed As Long
    Dim SpecialiteIdDisplayed As Long
    'Dim FiltreSpecialite As String

    Dim specialiteDao As New SpecialiteDao
    Dim rorDao As New RorDao

    Private DataTableRor As DataTable = New DataTable()

    Sub RadFRorListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Sélection professionnel de santé", userLog)

        If SpecialiteId <> 0 Then
            'FiltreSpecialite = " oa_ror_specialite_id = " & SpecialiteId.ToString & " and oa_ror_type = '" & TypeRor.Trim() & "'"
            'VrorBindingSource.Filter = FiltreSpecialite
            ChargementRor()
            Dim Specialite As Specialite
            Specialite = specialiteDao.GetSpecialiteById(SpecialiteId)
            LblSpecialiteFiltre.Text = Specialite.Description
        Else
            LblLabelSpecialite.Hide()
            LblSpecialiteFiltre.Hide()
            lblLabelType.Hide()
            LblType.Hide()
        End If

        InitHabilitation()

        CodeRetour = False
        GbxSelection.Hide()

        Cursor.Current = Cursors.Default
    End Sub

    Sub ChargementRor()
        Cursor.Current = Cursors.WaitCursor

        DataTableRor.Rows.Clear()
        RadGridViewRor.Rows.Clear()

        Dim adresse As String
        Dim ExtractionAnnuaireNational As Boolean

        DataTableRor = rorDao.GetRorBySpecialiteAndType(SpecialiteId, TypeRor)

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = DataTableRor.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadGridViewRor.Rows.Add(iGrid)

            'Alimentation du Grid
            RadGridViewRor.Rows(iGrid).Cells("rorId").Value = DataTableRor.Rows(i)("oa_ror_id")
            RadGridViewRor.Rows(iGrid).Cells("specialiteId").Value = Coalesce(DataTableRor.Rows(i)("oa_ror_specialite_id"), "")
            RadGridViewRor.Rows(iGrid).Cells("specialiteDescription").Value = Coalesce(DataTableRor.Rows(i)("oa_r_specialite_description"), "")
            RadGridViewRor.Rows(iGrid).Cells("nom").Value = Coalesce(DataTableRor.Rows(i)("oa_ror_nom"), "")
            ExtractionAnnuaireNational = Coalesce(DataTableRor.Rows(i)("oa_ror_extraction_annuaire"), False)
            If ExtractionAnnuaireNational = False Then
                RadGridViewRor.Rows(iGrid).Cells("nom").Style.ForeColor = Color.Red
            End If
            RadGridViewRor.Rows(iGrid).Cells("type").Value = Coalesce(DataTableRor.Rows(i)("oa_ror_type"), "")
            RadGridViewRor.Rows(iGrid).Cells("structure").Value = Coalesce(DataTableRor.Rows(i)("oa_ror_structure_nom"), "")
            adresse = Coalesce(DataTableRor.Rows(i)("oa_ror_adresse1"), "").trim()
            If adresse <> "" Then
                adresse += " - "
            End If
            adresse += Coalesce(DataTableRor.Rows(i)("oa_ror_adresse2"), "").trim()
            RadGridViewRor.Rows(iGrid).Cells("adresse").Value = adresse
            RadGridViewRor.Rows(iGrid).Cells("codePostal").Value = Coalesce(DataTableRor.Rows(i)("oa_ror_code_postal"), "")
            RadGridViewRor.Rows(iGrid).Cells("ville").Value = Coalesce(DataTableRor.Rows(i)("oa_ror_ville"), "")
            RadGridViewRor.Rows(iGrid).Cells("cleAnnuaire").Value = Coalesce(DataTableRor.Rows(i)("oa_ror_cle_reference"), "")
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewRor.Rows.Count > 0 Then
            RadGridViewRor.CurrentRow = RadGridViewRor.ChildRows(0)
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    'Création occurrence
    Private Sub RadBtnCreation_Click(sender As Object, e As EventArgs) Handles RadBtnCreation.Click
        CreationIntervenant()
    End Sub

    Private Sub CréationNouvelIntervenantToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles CréationNouvelIntervenantToolStripMenuItem.Click
        CreationIntervenant()
    End Sub

    Private Sub CreationIntervenant()
        If SpecialiteId = 0 Then
            Dim SelectedSpecialiteId As Integer
            Using vRadFSpecialiteSelecteur As New RadFSpecialiteSelecteur
                vRadFSpecialiteSelecteur.Select()
                vRadFSpecialiteSelecteur.ShowDialog()             'Modal
                SelectedSpecialiteId = vRadFSpecialiteSelecteur.SelectedSpecialiteId
                'Si un DRC a été sélectionné
                If SelectedSpecialiteId <> 0 Then
                    Using vFRorDetailEdit As New RadFRorDetailEdit
                        vFRorDetailEdit.SelectedRorId = 0
                        vFRorDetailEdit.SelectedSpecialiteId = SelectedSpecialiteId
                        vFRorDetailEdit.SelectedTypeSpecialite = vRadFSpecialiteSelecteur.SelectedTypeSpecialite
                        vFRorDetailEdit.ShowDialog() 'Modal
                        'Si le traitement a été créé, on recharge la grid
                        If vFRorDetailEdit.CodeRetour = True Then
                            ChargementRor()
                        End If
                    End Using
                End If
            End Using
        Else
            Using vFRorDetailEdit As New RadFRorDetailEdit
                vFRorDetailEdit.SelectedRorId = 0
                vFRorDetailEdit.SelectedSpecialiteId = SpecialiteId
                'vFRorDetailEdit.SelectedTypeSpecialite = vRadFSpecialiteSelecteur.SelectedTypeSpecialite
                vFRorDetailEdit.ShowDialog() 'Modal
                'Si le traitement a été créé, on recharge la grid
                If vFRorDetailEdit.CodeRetour = True Then
                    ChargementRor()
                End If
            End Using
        End If
    End Sub

    'Modification occurrence
    Private Sub ModificationIntervenantToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles ModificationIntervenantToolStripMenuItem.Click
        ModificationIntervenant()
    End Sub

    Private Sub RadBtnModification_Click(sender As Object, e As EventArgs) Handles RadBtnModification.Click
        ModificationIntervenant()
    End Sub

    Private Sub ModificationIntervenant()
        If RadGridViewRor.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewRor.Rows.IndexOf(Me.RadGridViewRor.CurrentRow)
            If aRow >= 0 Then
                Dim rorId As Integer = RadGridViewRor.Rows(aRow).Cells("rorId").Value
                Using vFRorDetailEdit As New RadFRorDetailEdit
                    vFRorDetailEdit.SelectedRorId = rorId
                    vFRorDetailEdit.ShowDialog() 'Modal
                    If vFRorDetailEdit.CodeRetour = True Then
                        ChargementRor()
                    End If
                End Using
            End If
        End If
    End Sub


    'Sélection
    Private Sub MasterTemplate_CellClick(sender As Object, e As GridViewCellEventArgs) Handles RadGridViewRor.CellClick
        Selection()
    End Sub

    Private Sub Selection()
        If Selecteur = True Then
            If RadGridViewRor.CurrentRow IsNot Nothing Then
                Dim aRow As Integer = Me.RadGridViewRor.Rows.IndexOf(Me.RadGridViewRor.CurrentRow)
                If aRow >= 0 Then
                    RorIdDisplayed = RadGridViewRor.Rows(aRow).Cells("rorId").Value
                    SpecialiteIdDisplayed = RadGridViewRor.Rows(aRow).Cells("specialiteId").Value
                    Dim ParcoursDataTable As DataTable
                    Dim parcoursDao As New ParcoursDao
                    Dim ExistRorId As Long
                    Dim ExistSpecialiteId As Long
                    Dim IntervenantExiste As Boolean = False
                    Dim specialiteExiste As Boolean = False
                    Dim IntervenantMasque As Boolean = False
                    Dim SpecialiteMasque As Boolean = False
                    ParcoursDataTable = parcoursDao.GetAllParcoursbyPatient(PatientId)
                    Dim rowCount As Integer = ParcoursDataTable.Rows.Count - 1
                    For i = 0 To rowCount Step 1
                        ExistRorId = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_ror_id"), 0)
                        ExistSpecialiteId = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_specialite"), 0)
                        If ExistRorId = RorIdDisplayed Then
                            IntervenantExiste = True
                            IntervenantMasque = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_cacher"), False)
                            Exit For
                        Else
                            If ExistSpecialiteId = SpecialiteIdDisplayed Then
                                specialiteExiste = True
                                If Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_cacher"), False) = True Then
                                    SpecialiteMasque = True
                                End If
                            End If
                        End If
                    Next
                    If IntervenantExiste = True Then
                        Dim messageIntervenant As String
                        If IntervenantMasque = True Then
                            messageIntervenant = "Cet intervenant (masqué) existe dèjà pour ce patient"
                        Else
                            messageIntervenant = "Cet intervenant existe déjà pour ce patient"
                        End If
                        LblNom.Text = ""
                        LblSpecialite.Text = ""
                        GbxSelection.Hide()
                        MessageBox.Show(messageIntervenant)
                    Else
                        If specialiteExiste = True Then
                            Dim messageSpecialite As String
                            If SpecialiteMasque = True Then
                                messageSpecialite = "Attention, un intervenant (masqué) existe déjà avec cette spécialité pour ce patient"
                            Else
                                messageSpecialite = "Attention, un intervenant existe déjà avec cette spécialité pour ce patient"
                            End If
                            MessageBox.Show(messageSpecialite)
                        End If
                        LblNom.Text = RadGridViewRor.Rows(aRow).Cells("nom").Value
                        LblSpecialite.Text = RadGridViewRor.Rows(aRow).Cells("specialiteDescription").Value
                        LblStructure.Text = RadGridViewRor.Rows(aRow).Cells("structure").Value
                        LblAdresse.Text = RadGridViewRor.Rows(aRow).Cells("adresse").Value
                        LblVille.Text = RadGridViewRor.Rows(aRow).Cells("codePostal").Value & " " & RadGridViewRor.Rows(aRow).Cells("ville").Value

                        If RadGridViewRor.Rows(aRow).Cells("cleAnnuaire").Value <> "" Then
                            RadBtnDetail.Show()
                        Else
                            RadBtnDetail.Hide()
                        End If
                        GbxSelection.Show()
                    End If
                    End If
            End If
        End If
    End Sub

    Private Sub RadBtnSelection_Click(sender As Object, e As EventArgs) Handles RadBtnSelection.Click
        SelectionRetour()
    End Sub

    Private Sub SelectionRetour()
        SelectedRorId = RorIdDisplayed
        CodeRetour = True
        Close()
    End Sub

    Private Sub RadGridView1_DoubleClick(sender As Object, e As EventArgs) Handles RadGridViewRor.DoubleClick
        Selection()
        SelectionRetour()
    End Sub

    Private Sub RadFRorListe_BindingContextChanged(sender As Object, e As EventArgs) Handles MyBase.BindingContextChanged
        RorIdDisplayed = 0
        LblNom.Text = ""
        LblSpecialite.Text = ""
        LblStructure.Text = ""
        LblAdresse.Text = ""
        LblVille.Text = ""
        GbxSelection.Hide()
    End Sub

    Private Sub RadBtnAnnuaireProf_Click(sender As Object, e As EventArgs) Handles RadBtnAnnuaireProf.Click
        Dim specialite As Specialite
        specialite = specialiteDao.GetSpecialiteById(SpecialiteId)
        Try
            Using form As New RadFAnnuaireProfessionnelSelect
                form.InputSpecialiteId = specialite.SpecialiteId
                form.InputCodeProfessionId = specialite.NosG15CodeProfession
                form.InputTypeSavoirFaireId = specialite.NosR40TypeSavoirFaire
                form.InputCodeSavoirFaireId = specialite.NosCodeSavoirFaire
                form.ShowDialog()
                If form.SelectedProfessionnelCle <> 0 Then
                    ChargementRor()
                End If
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
    End Sub

    Private Sub InitHabilitation()
        If userLog.UtilisateurAdmin = False Then
            RadBtnModification.Hide()
            RadBtnCreation.Hide()
        End If
    End Sub

    Private Sub RadBtnDetail_Click(sender As Object, e As EventArgs) Handles RadBtnDetail.Click
        If RorIdDisplayed <> 0 Then
            Dim ror As Ror
            ror = rorDao.GetRorById(RorIdDisplayed)
            If ror.CleReferenceAnnuaire <> 0 Then
                Try
                    Using form As New RadFAnnuaireProfessionneldetail
                        form.CleReferenceAnnuaire = ror.CleReferenceAnnuaire
                        form.Reference = AnnuaireReferenceDao.EnumSourceAnnuaire.ANNUAIRE_REFERENCE
                        form.ShowDialog()
                    End Using
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try
            Else
                MessageBox.Show("Option disponible uniquement pour les intervenants issus du répertoire national des professionnels de santé")
            End If
        End If
    End Sub
End Class
