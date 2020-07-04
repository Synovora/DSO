Imports Oasis_Common
Imports System.IO
Imports Telerik.WinControls
Imports Telerik.WinControls.UI

Public Class RadFPPSListeParcours

    Private _selectedPatient As PatientBase

    Public Property SelectedPatient As PatientBase
        Get
            Return _selectedPatient
        End Get
        Set(value As PatientBase)
            _selectedPatient = value
        End Set
    End Property

    Dim ParcoursListProfilsOasis As New List(Of Integer)

    Dim InitParcoursNonCache As Boolean

    Private Sub RadFPPSListeParcours_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Placement de la fenêtre en bas à droite de l'écran parent
        Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - Me.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)

        InitParcoursNonCache = False
        RadChkParcoursNonCache.Checked = True

        ChargementParcoursDeSoin()
    End Sub

    Private Sub ChargementParcoursDeSoin()
        RadParcoursDataGridView.Rows.Clear()

        Dim ParcoursDataTable As DataTable
        Dim parcoursDao As New ParcoursDao
        Dim tacheDao As New TacheDao
        Dim SousCategorie, SpecialiteId As Integer
        Dim IntervenantOasis As Boolean

        ParcoursDataTable = parcoursDao.getAllParcoursbyPatient(SelectedPatient.patientId)

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = ParcoursDataTable.Rows.Count - 1
        Dim SpecialiteDescription As String
        Dim ParcoursCacher, ParcoursConsigneEnRouge As Boolean

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            Dim rorId As Integer = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_ror_id"), 0)
            ParcoursCacher = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_cacher"), False)
            If RadChkParcoursNonCache.Checked = True Then
                If ParcoursCacher = True Then
                    Continue For
                End If
            End If

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadParcoursDataGridView.Rows.Add(iGrid)
            'Alimentation du DataGridView
            RadParcoursDataGridView.Rows(iGrid).Cells("parcoursId").Value = ParcoursDataTable.Rows(i)("oa_parcours_id")

            SpecialiteId = ParcoursDataTable.Rows(i)("oa_parcours_specialite")
            SpecialiteDescription = Environnement.Table_specialite.GetSpecialiteDescription(SpecialiteId)
            RadParcoursDataGridView.Rows(iGrid).Cells("specialite").Value = SpecialiteDescription

            'Nom intervenant et Structure
            IntervenantOasis = False
            ParcoursConsigneEnRouge = False
            SousCategorie = ParcoursDataTable.Rows(i)("oa_parcours_sous_categorie_id")
            Select Case SousCategorie
                Case EnumSousCategoriePPS.medecinReferent
                    IntervenantOasis = True
                    ParcoursListProfilsOasis.Add(EnumSpecialiteOasis.medecinReferent)
                Case EnumSousCategoriePPS.IDE
                    IntervenantOasis = True
                    ParcoursListProfilsOasis.Add(EnumSpecialiteOasis.IDE)
                    Dim pacoursConsigneDao As New ParcoursConsigneDao
                    If pacoursConsigneDao.ExisteParcoursConsigne(ParcoursDataTable.Rows(i)("oa_parcours_id")) = False Then
                        ParcoursConsigneEnRouge = True
                    End If
                Case EnumSousCategoriePPS.sageFemme
                    If ParcoursDataTable.Rows(i)("oa_parcours_intervenant_oasis") = True Then
                        IntervenantOasis = True
                        ParcoursListProfilsOasis.Add(EnumSpecialiteOasis.sageFemmeOasis)
                    End If
                Case EnumSousCategoriePPS.specialiste
            End Select

            If IntervenantOasis = True Then
                RadParcoursDataGridView.Rows(iGrid).Cells("nomIntervenant").Value = "Oasis"
                RadParcoursDataGridView.Rows(iGrid).Cells("nomStructure").Value = "Oasis"
            Else
                RadParcoursDataGridView.Rows(iGrid).Cells("nomIntervenant").Value = Coalesce(ParcoursDataTable.Rows(i)("oa_ror_nom"), "")
                RadParcoursDataGridView.Rows(iGrid).Cells("nomStructure").Value = Coalesce(ParcoursDataTable.Rows(i)("oa_ror_structure_nom"), "")
            End If

            'Recherche de la dernière consultation
            Dim dateLast, dateNext As Date
            Dim TypeDemandeRdv As String
            'Dim tache As Tache

            RadParcoursDataGridView.Rows(iGrid).Cells("consultationLast").Value = "-"
            dateLast = Coalesce(ParcoursDataTable.Rows(i)("LastRendezVous"), Nothing)
            If dateLast <> Nothing Then
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationLast").Value = outils.FormatageDateAffichage(dateLast, True)
            End If

            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = "-"
            dateNext = Coalesce(ParcoursDataTable.Rows(i)("NextRendezVous"), Nothing)
            If dateNext <> Nothing Then
                'Rendez-vous planifiée
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = dateNext.ToString("dd.MM.yyyy")
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationNextHeure").Value = dateNext.ToString("HH:mm")
            Else
                'Recherche si existe demande de rendez-vous
                dateNext = Coalesce(ParcoursDataTable.Rows(i)("DateDemandeRdv"), Nothing)
                If dateNext <> Nothing Then
                    'Rendez-vous prévisionnel, demande en cours
                    TypeDemandeRdv = Coalesce(ParcoursDataTable.Rows(i)("TypeDemandeRdv"), "")
                    Select Case TypeDemandeRdv
                        Case TacheDao.typeDemandeRendezVous.ANNEE.ToString
                            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = dateNext.ToString("yyyy")
                        Case TacheDao.typeDemandeRendezVous.ANNEEMOIS.ToString
                            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = dateNext.ToString("MM.yyyy")
                        Case Else
                            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = outils.FormatageDateAffichage(dateNext, True)
                    End Select
                Else
                    Dim Rythme As Integer = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_rythme"), 0)
                    Dim Base As String = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_base"), 0)
                    If Rythme <> 0 And Base <> "" Then
                        If dateLast <> Nothing Then
                            'Rendez-vous prévisionnel, calculé selon le rythme saisi et le dernier rendez-vous passé
                            dateNext = CalculProchainRendezVous(dateLast, Rythme, Base)
                            RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = outils.FormatageDateAffichage(dateNext, False)
                        Else
                            Dim DateCreation As Date = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_date_creation"), Nothing)
                            If DateCreation <> Nothing Then
                                'Rendez-vous prévisionnel, calculé selon le rythme saisi et la date de création de l'intervenant dans le parcours de soin du patient
                                dateNext = CalculProchainRendezVous(DateCreation, Rythme, Base)
                                RadParcoursDataGridView.Rows(iGrid).Cells("consultationNext").Value = outils.FormatageDateAffichage(dateNext, False)
                            Else
                                'Rendez-vous à venir non calculable
                            End If
                        End If
                    Else
                        'Pas de rendez-vous à venir pour cet intervenant
                    End If
                End If
            End If

            RadParcoursDataGridView.Rows(iGrid).Cells("commentaire").Value = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_commentaire"), "")

            If ParcoursCacher = True Then
                RadParcoursDataGridView.Rows(iGrid).Cells("specialite").Style.ForeColor = Color.CornflowerBlue
                RadParcoursDataGridView.Rows(iGrid).Cells("nomIntervenant").Style.ForeColor = Color.CornflowerBlue
                RadParcoursDataGridView.Rows(iGrid).Cells("nomStructure").Style.ForeColor = Color.CornflowerBlue
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationLast").Style.ForeColor = Color.CornflowerBlue
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationnext").Style.ForeColor = Color.CornflowerBlue
                RadParcoursDataGridView.Rows(iGrid).Cells("commentaire").Style.ForeColor = Color.CornflowerBlue
            End If

            If ParcoursConsigneEnRouge = True Then
                RadParcoursDataGridView.Rows(iGrid).Cells("specialite").Style.ForeColor = Color.Red
                RadParcoursDataGridView.Rows(iGrid).Cells("nomIntervenant").Style.ForeColor = Color.Red
                RadParcoursDataGridView.Rows(iGrid).Cells("nomStructure").Style.ForeColor = Color.Red
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationLast").Style.ForeColor = Color.Red
                RadParcoursDataGridView.Rows(iGrid).Cells("consultationnext").Style.ForeColor = Color.Red
                RadParcoursDataGridView.Rows(iGrid).Cells("commentaire").Style.ForeColor = Color.Red
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadParcoursDataGridView.Rows.Count > 0 Then
            RadParcoursDataGridView.CurrentRow = RadParcoursDataGridView.ChildRows(0)
            RadParcoursDataGridView.TableElement.VScrollBar.Value = 0
        End If
    End Sub

    Private Sub RadParcoursDataGridView_ToolTipTextNeeded(sender As Object, e As ToolTipTextNeededEventArgs) Handles RadParcoursDataGridView.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing AndAlso hoveredCell.ColumnInfo.Name = "consultationNext" Then
            e.ToolTipText = hoveredCell.RowInfo.Cells("consultationNextHeure").Value
        End If
    End Sub


    Private Sub RadBtnAjout_Click(sender As Object, e As EventArgs) Handles RadBtnAjout.Click
        CreationIntervenant()
    End Sub

    Private Sub CreationIntervenant()
        If outils.AccesFonctionMedicaleSynthese(SelectedPatient) = False Then
            Exit Sub
        End If

        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Try
            Using vFSpecialiteSelecteur As New RadFSpecialiteSelecteur
                vFSpecialiteSelecteur.ListProfilOasis = ParcoursListProfilsOasis
                vFSpecialiteSelecteur.ShowDialog()                  'Sélection de spécialité
                If vFSpecialiteSelecteur.SelectedSpecialiteId <> 0 Then
                    Using vRadFRorListe As New RadFRorListe
                        vRadFRorListe.Selecteur = True
                        vRadFRorListe.PatientId = Me.SelectedPatient.patientId
                        vRadFRorListe.SpecialiteId = vFSpecialiteSelecteur.SelectedSpecialiteId
                        vRadFRorListe.TypeRor = "Intervenant"
                        vRadFRorListe.ShowDialog()                  'Sélection d'un professionnel de santé
                        If vRadFRorListe.CodeRetour = True Then
                            Using vFParcoursDetailEdit As New RadFParcoursDetailEdit
                                vFParcoursDetailEdit.SelectedParcoursId = 0
                                vFParcoursDetailEdit.SelectedRorId = vRadFRorListe.SelectedRorId
                                vFParcoursDetailEdit.SelectedSpecialiteId = vFSpecialiteSelecteur.SelectedSpecialiteId
                                vFParcoursDetailEdit.SelectedPatient = Me.SelectedPatient
                                'vFParcoursDetailEdit.UtilisateurConnecte = userLog
                                vFParcoursDetailEdit.RythmeObligatoire = False
                                vFParcoursDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                                vFParcoursDetailEdit.ShowDialog()   'Gestion de l'intervenant
                                If vFParcoursDetailEdit.CodeRetour = True Then
                                    ChargementParcoursDeSoin()
                                End If
                            End Using
                        End If
                    End Using
                End If
            End Using
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub RadParcoursDataGridView_DoubleClick(sender As Object, e As EventArgs) Handles RadParcoursDataGridView.DoubleClick
        If RadParcoursDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadParcoursDataGridView.Rows.IndexOf(Me.RadParcoursDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim ParcoursId As Integer = RadParcoursDataGridView.Rows(aRow).Cells("parcoursId").Value
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Try
                    Using vFParcoursDetailEdit As New RadFParcoursDetailEdit
                        vFParcoursDetailEdit.SelectedParcoursId = ParcoursId
                        vFParcoursDetailEdit.SelectedPatient = Me.SelectedPatient
                        'vFParcoursDetailEdit.UtilisateurConnecte = userLog
                        vFParcoursDetailEdit.RythmeObligatoire = False
                        vFParcoursDetailEdit.PositionGaucheDroite = EnumPosition.Droite
                        vFParcoursDetailEdit.ShowDialog() 'Modal
                        If vFParcoursDetailEdit.CodeRetour = True Then
                            ChargementParcoursDeSoin()
                        End If
                    End Using
                Catch ex As Exception
                    MsgBox(ex.Message())
                End Try
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadChkParcoursNonCache_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles RadChkParcoursNonCache.ToggleStateChanged
        If RadChkParcoursNonCache.Checked = True Then
            RadChkParcoursTous.Checked = False
            If InitParcoursNonCache = True Then
                Application.DoEvents()
                ChargementParcoursDeSoin()
            Else
                InitParcoursNonCache = True
            End If
        Else
            If RadChkParcoursTous.Checked = False Then
                RadChkParcoursNonCache.Checked = True
            End If
        End If
    End Sub

    Private Sub RadChkParcoursTous_ToggleStateChanged(sender As Object, args As StateChangedEventArgs) Handles RadChkParcoursTous.ToggleStateChanged
        If RadChkParcoursTous.Checked = True Then
            RadChkParcoursNonCache.Checked = False
            Application.DoEvents()
            ChargementParcoursDeSoin()
        Else
            If RadChkParcoursNonCache.Checked = False Then
                RadChkParcoursTous.Checked = True
            End If
        End If
    End Sub

    Private Sub RadButtonAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

End Class
