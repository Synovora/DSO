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
    Dim FiltreSpecialite As String

    Sub RadFRorListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'cette ligne de code charge les données dans la table 'RORDS.v_ror'. Vous pouvez la déplacer ou la supprimer selon les besoins.
        Me.V_rorTableAdapter.Fill(Me.RORDS.v_ror)

        If SpecialiteId <> 0 Then
            FiltreSpecialite = " oa_ror_specialite_id = " & SpecialiteId.ToString & " and oa_ror_type = '" & TypeRor.Trim() & "'"
            VrorBindingSource.Filter = FiltreSpecialite
            LblSpecialiteFiltre.Text = Environnement.Table_specialite.GetSpecialiteDescription(SpecialiteId)
        Else
            LblLabelSpecialite.Hide()
            LblSpecialiteFiltre.Hide()
            lblLabelType.Hide()
            LblType.Hide()
        End If

        CodeRetour = False
        GbxSelection.Hide()

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
                            Me.V_rorTableAdapter.Fill(Me.RORDS.v_ror)
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
                    Me.V_rorTableAdapter.Fill(Me.RORDS.v_ror)
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
        If RadGridView1.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridView1.Rows.IndexOf(Me.RadGridView1.CurrentRow)
            If aRow >= 0 Then
                Dim rorId As Integer = RadGridView1.Rows(aRow).Cells("oa_ror_id").Value
                Using vFRorDetailEdit As New RadFRorDetailEdit
                    vFRorDetailEdit.SelectedRorId = rorId
                    vFRorDetailEdit.ShowDialog() 'Modal
                    If vFRorDetailEdit.CodeRetour = True Then
                        Me.V_rorTableAdapter.Fill(Me.RORDS.v_ror)
                    End If
                End Using
            End If
        End If
    End Sub


    'Sélection
    Private Sub MasterTemplate_CellClick(sender As Object, e As GridViewCellEventArgs) Handles RadGridView1.CellClick
        Selection()
    End Sub

    Private Sub Selection()
        If Selecteur = True Then
            If RadGridView1.CurrentRow IsNot Nothing Then
                Dim aRow As Integer = Me.RadGridView1.Rows.IndexOf(Me.RadGridView1.CurrentRow)
                If aRow >= 0 Then
                    RorIdDisplayed = RadGridView1.Rows(aRow).Cells("oa_ror_id").Value
                    SpecialiteIdDisplayed = RadGridView1.Rows(aRow).Cells("oa_ror_specialite_id").Value
                    Dim ParcoursDataTable As DataTable
                    Dim parcoursDao As New ParcoursDao
                    Dim ExistRorId As Long
                    Dim ExistSpecialiteId As Long
                    Dim IntervenantExiste As Boolean = False
                    Dim specialiteExiste As Boolean = False
                    Dim IntervenantMasque As Boolean = False
                    Dim SpecialiteMasque As Boolean = False
                    ParcoursDataTable = parcoursDao.getAllParcoursbyPatient(PatientId)
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
                        LblNom.Text = RadGridView1.Rows(aRow).Cells("oa_ror_nom").Value
                        LblSpecialite.Text = RadGridView1.Rows(aRow).Cells("oa_r_specialite_description").Value
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

    Private Sub RadGridView1_DoubleClick(sender As Object, e As EventArgs) Handles RadGridView1.DoubleClick
        Selection()
        SelectionRetour()
    End Sub

    Private Sub RadFRorListe_BindingContextChanged(sender As Object, e As EventArgs) Handles MyBase.BindingContextChanged
        RorIdDisplayed = 0
        LblNom.Text = ""
        LblSpecialite.Text = ""
        GbxSelection.Hide()
    End Sub

End Class
