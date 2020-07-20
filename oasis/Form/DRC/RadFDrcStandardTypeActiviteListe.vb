Imports Oasis_Common

Public Class RadFDrcStandardTypeActiviteListe
    ReadOnly drc As New Drc
    ReadOnly drcdao As New DrcDao
    ReadOnly drcStandardDao As New DrcStandardDao
    Dim TypeActivite As String


    Private Sub RadFDrcStandardTypeActiviteActivite_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        TypeActivite = EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GROSSESSE
        ChargementDrc()
        RadBtnSuiviGrossesse.ForeColor = Color.Red
        RadBtnSuiviGrossesse.Font = New Font(RadBtnSuiviGrossesse.Font, FontStyle.Bold)
    End Sub

    Private Sub ChargementDrc()
        Dim DrcDataTable As DataTable
        Dim DrcId As Long
        DrcDataTable = drcStandardDao.getAllDrcByTypeActivite(TypeActivite)

        If TypeActivite = EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE OrElse
            TypeActivite = EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE Then
            RadGridViewDrcAsso.Columns.Item("applicationDe").IsVisible = True
            RadGridViewDrcAsso.Columns.Item("applicationA").IsVisible = True
        Else
            RadGridViewDrcAsso.Columns.Item("applicationDe").IsVisible = False
            RadGridViewDrcAsso.Columns.Item("applicationA").IsVisible = False
        End If


        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = DrcDataTable.Rows.Count - 1
        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            Dim drc As Drc = New Drc
            DrcId = Coalesce(DrcDataTable.Rows(i)("drc_id"), 0)
            If DrcId <> 0 Then
                drcdao.GetDrc(drc, DrcId)
            Else
                Continue For
            End If
            iGrid += 1
            RadGridViewDrcAsso.Rows.Add(iGrid)
            RadGridViewDrcAsso.Rows(iGrid).Cells("id").Value = DrcDataTable.Rows(i)("id")
            RadGridViewDrcAsso.Rows(iGrid).Cells("drcId").Value = DrcDataTable.Rows(i)("drc_id")
            RadGridViewDrcAsso.Rows(iGrid).Cells("denomination").Value = drc.DrcLibelle
            RadGridViewDrcAsso.Rows(iGrid).Cells("categorieOasis").Value = drcdao.GetItemCategorieOasisByCode(DrcDataTable.Rows(i)("categorie_oasis"))

            Dim ageMin, ageMax As Integer
            ageMin = Coalesce(DrcDataTable.Rows(i)("age_min"), 0)
            ageMax = Coalesce(DrcDataTable.Rows(i)("age_max"), 0)
            Select Case TypeActivite
                Case EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE
                    If ageMin = 0 Then
                        RadGridViewDrcAsso.Rows(iGrid).Cells("applicationDe").Value = ""
                    Else
                        RadGridViewDrcAsso.Rows(iGrid).Cells("applicationDe").Value = ageMin.ToString & " mois"
                    End If
                    If ageMax = 0 Then
                        RadGridViewDrcAsso.Rows(iGrid).Cells("applicationA").Value = ""
                    Else
                        RadGridViewDrcAsso.Rows(iGrid).Cells("applicationA").Value = ageMax.ToString & " mois"
                    End If
                Case EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE
                    If ageMin = 0 Then
                        RadGridViewDrcAsso.Rows(iGrid).Cells("applicationDe").Value = ""
                    Else
                        RadGridViewDrcAsso.Rows(iGrid).Cells("applicationDe").Value = ageMin.ToString & " ans"
                    End If
                    If ageMax = 0 Then
                        RadGridViewDrcAsso.Rows(iGrid).Cells("applicationA").Value = ""
                    Else
                        RadGridViewDrcAsso.Rows(iGrid).Cells("applicationA").Value = ageMax.ToString & " ans"
                    End If
                Case Else
                    RadGridViewDrcAsso.Rows(iGrid).Cells("applicationDe").Value = ""
                    RadGridViewDrcAsso.Rows(iGrid).Cells("applicationA").Value = ""
            End Select
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewDrcAsso.Rows.Count > 0 Then
            Me.RadGridViewDrcAsso.CurrentRow = RadGridViewDrcAsso.Rows(0)
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnSupprimer_Click(sender As Object, e As EventArgs) Handles RadBtnSupprimer.Click
        If RadGridViewDrcAsso.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewDrcAsso.Rows.IndexOf(Me.RadGridViewDrcAsso.CurrentRow)
            If aRow >= 0 Then
                If MsgBox("Confirmation de l'annulation ", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
                    Dim DrcStandardId As Integer = RadGridViewDrcAsso.Rows(aRow).Cells("Id").Value
                    'Suppression de l'association de la DRC
                    If drcStandardDao.AnnulationDrcStandard(DrcStandardId, userLog) = True Then
                        MessageBox.Show("La DRC standard a été annulée")
                        RadGridViewDrcAsso.Rows.Clear()
                        ChargementDrc()
                    End If
                End If
            End If
        End If
    End Sub

    Private Sub RadBtnActePM_Click(sender As Object, e As EventArgs) Handles RadBtnActePM.Click
        Dim CategorieOasis = Drc.EnumCategorieOasisCode.ActeParamedical
        selectDrc(CategorieOasis)
    End Sub

    Private Sub RadBtnSlectProtocole_Click(sender As Object, e As EventArgs) Handles RadBtnSlectProtocole.Click
        Dim CategorieOasis = Drc.EnumCategorieOasisCode.ProtocoleCollaboratif
        selectDrc(CategorieOasis)
    End Sub

    Private Sub RadBtnSelectParm_Click(sender As Object, e As EventArgs) Handles RadBtnSelectParm.Click
        Dim CategorieOasis = Drc.EnumCategorieOasisCode.GroupeParametres
        selectDrc(CategorieOasis)
    End Sub
    Private Sub RadBtnMesurePreventive_Click(sender As Object, e As EventArgs) Handles RadBtnMesurePreventive.Click
        Dim CategorieOasis = Drc.EnumCategorieOasisCode.Prevention
        selectDrc(CategorieOasis)
    End Sub

    'Sélection DRC à implémenter
    Private Sub selectDrc(CategorieOasis As String)
        Dim SelectedDrcId As Integer
        Cursor.Current = Cursors.WaitCursor

        Try
            Using vFDrcSelecteur As New RadFDRCSelecteur
                vFDrcSelecteur.SelectedPatient = Nothing
                vFDrcSelecteur.CategorieOasis = CategorieOasis
                vFDrcSelecteur.ShowDialog()
                SelectedDrcId = vFDrcSelecteur.SelectedDrcId
                If SelectedDrcId <> 0 Then
                    'Ajout de l'occurrence choisie (contrôle que cette DORC n'est pas déjà associée)
                    Dim drcStandard As DrcStandard = New DrcStandard
                    drcStandard.TypeActivite = TypeActivite
                    drcStandard.DrcId = SelectedDrcId
                    Dim drcSelected As Drc = New Drc
                    drcdao.GetDrc(drcSelected, SelectedDrcId)
                    drcStandard.CategorieOasis = drcSelected.CategorieOasisId
                    Try
                        If drcStandardDao.CreationDrcStandard(drcStandard) = True Then
                            If TypeActivite = EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE Or
                                TypeActivite = EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE Then
                                'Récupérer le dernier DRC standard créé
                                Dim DrcStandardIdCreated As Integer = drcStandardDao.GetDrcStandardCreated(drcStandard)
                                Using vRadFDrcStandardTypeActiviteDetail As New RadFDrcStandardTypeActiviteDetail
                                    vRadFDrcStandardTypeActiviteDetail.SelectedDrcStandardId = DrcStandardIdCreated
                                    vRadFDrcStandardTypeActiviteDetail.ShowDialog()
                                End Using
                            End If
                            RadGridViewDrcAsso.Rows.Clear()
                            ChargementDrc()
                        End If
                    Catch ex As Exception
                        CreateLog(ex.ToString, Me.Name, LogDao.EnumTypeLog.ERREUR.ToString, userLog)
                        If ex.Message.StartsWith("Collision") = True Then
                            MessageBox.Show("La DRC sélectionnée existe déjà pour le type d'activité d'épisode")
                        End If
                    End Try
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub RadBtnSuiviGrossesse_Click(sender As Object, e As EventArgs) Handles RadBtnSuiviGrossesse.Click
        Cursor.Current = Cursors.WaitCursor
        RadGridViewDrcAsso.Rows.Clear()
        TypeActivite = EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GROSSESSE
        ChargementDrc()
        GestionBtnSelection()
        RadBtnSuiviGrossesse.ForeColor = Color.Red
        RadBtnSuiviGrossesse.Font = New Font(RadBtnSuiviGrossesse.Font, FontStyle.Bold)
        ShowBtnSelectDrc()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub RadBtnSuiviGynecologique_Click(sender As Object, e As EventArgs) Handles RadBtnSuiviGynecologique.Click
        Cursor.Current = Cursors.WaitCursor
        RadGridViewDrcAsso.Rows.Clear()
        TypeActivite = EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_SUIVI_GYNECOLOGIQUE
        ChargementDrc()
        GestionBtnSelection()
        RadBtnSuiviGynecologique.ForeColor = Color.Red
        RadBtnSuiviGynecologique.Font = New Font(RadBtnSuiviGynecologique.Font, FontStyle.Bold)
        ShowBtnSelectDrc()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub RadBtnSuiviEnfantPreScolaire_Click(sender As Object, e As EventArgs) Handles RadBtnSuiviEnfantPreScolaire.Click
        Cursor.Current = Cursors.WaitCursor
        RadGridViewDrcAsso.Rows.Clear()
        TypeActivite = EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE
        ChargementDrc()
        GestionBtnSelection()
        RadBtnSuiviEnfantPreScolaire.ForeColor = Color.Red
        RadBtnSuiviEnfantPreScolaire.Font = New Font(RadBtnSuiviEnfantPreScolaire.Font, FontStyle.Bold)
        ShowBtnSelectDrc()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub RadBtnSuiviEnfantScolaire_Click(sender As Object, e As EventArgs) Handles RadBtnSuiviEnfantScolaire.Click
        Cursor.Current = Cursors.WaitCursor
        RadGridViewDrcAsso.Rows.Clear()
        TypeActivite = EpisodeDao.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE
        ChargementDrc()
        GestionBtnSelection()
        RadBtnSuiviEnfantScolaire.ForeColor = Color.Red
        RadBtnSuiviEnfantScolaire.Font = New Font(RadBtnSuiviEnfantScolaire.Font, FontStyle.Bold)
        ShowBtnSelectDrc()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub RadBtnPathologieAigue_Click(sender As Object, e As EventArgs) Handles RadBtnPathologieAigue.Click
        Cursor.Current = Cursors.WaitCursor
        RadGridViewDrcAsso.Rows.Clear()
        TypeActivite = EpisodeDao.EnumTypeActiviteEpisodeCode.PATHOLOGIE_AIGUE
        ChargementDrc()
        GestionBtnSelection()
        RadBtnPathologieAigue.ForeColor = Color.Red
        RadBtnPathologieAigue.Font = New Font(RadBtnPathologieAigue.Font, FontStyle.Bold)
        RadBtnActePM.Hide()
        RadBtnSlectProtocole.Hide()
        RadBtnMesurePreventive.Hide()
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub GestionBtnSelection()
        RadBtnSuiviGrossesse.ForeColor = Color.FromArgb(21, 66, 139)
        RadBtnSuiviGrossesse.Font = New Font(RadBtnSuiviGrossesse.Font, FontStyle.Regular)
        RadBtnSuiviGynecologique.ForeColor = Color.FromArgb(21, 66, 139)
        RadBtnSuiviGynecologique.Font = New Font(RadBtnSuiviGynecologique.Font, FontStyle.Regular)
        RadBtnSuiviEnfantPreScolaire.ForeColor = Color.FromArgb(21, 66, 139)
        RadBtnSuiviEnfantPreScolaire.Font = New Font(RadBtnSuiviEnfantPreScolaire.Font, FontStyle.Regular)
        RadBtnSuiviEnfantScolaire.ForeColor = Color.FromArgb(21, 66, 139)
        RadBtnSuiviEnfantScolaire.Font = New Font(RadBtnSuiviEnfantScolaire.Font, FontStyle.Regular)
        RadBtnPathologieAigue.ForeColor = Color.FromArgb(21, 66, 139)
        RadBtnPathologieAigue.Font = New Font(RadBtnPathologieAigue.Font, FontStyle.Regular)
    End Sub

    'Appel gestion DRC Standard
    Private Sub RadGridViewDrcAsso_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadGridViewDrcAsso.CellDoubleClick
        modifierDRCStandard()
    End Sub

    Private Sub RadBtnModifier_Click(sender As Object, e As EventArgs) Handles RadBtnModifier.Click
        modifierDRCStandard()
    End Sub

    Private Sub modifierDRCStandard()
        If RadGridViewDrcAsso.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewDrcAsso.Rows.IndexOf(Me.RadGridViewDrcAsso.CurrentRow)
            If aRow >= 0 Then
                Dim DrcStandarddId As Integer = RadGridViewDrcAsso.Rows(aRow).Cells("Id").Value
                Cursor.Current = Cursors.WaitCursor

                Try
                    Using vRadFDrcStandardTypeActiviteDetail As New RadFDrcStandardTypeActiviteDetail
                        vRadFDrcStandardTypeActiviteDetail.SelectedDrcStandardId = DrcStandarddId
                        vRadFDrcStandardTypeActiviteDetail.ShowDialog()
                        If vRadFDrcStandardTypeActiviteDetail.CodeRetour = True Then
                            RadGridViewDrcAsso.Rows.Clear()
                            ChargementDrc()
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try

            End If
        End If
    End Sub

    'Détail DRC
    Private Sub RadBtnDRCDetail_Click(sender As Object, e As EventArgs) Handles RadBtnDRCDetail.Click
        If RadGridViewDrcAsso.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewDrcAsso.Rows.IndexOf(Me.RadGridViewDrcAsso.CurrentRow)
            If aRow >= 0 Then
                Dim DrcdId As Integer = RadGridViewDrcAsso.Rows(aRow).Cells("drcId").Value
                'Suppression de l'association de la DRC
                Cursor.Current = Cursors.WaitCursor

                Try
                    Using vRadFDrcDetailEdit As New RadFDrcDetailEdit
                        vRadFDrcDetailEdit.SelectedDRCId = DrcdId
                        vRadFDrcDetailEdit.UtilisateurConnecte = userLog
                        vRadFDrcDetailEdit.ShowDialog()
                        If vRadFDrcDetailEdit.CodeRetour = True Then
                            RadGridViewDrcAsso.Rows.Clear()
                            ChargementDrc()
                        End If
                    End Using
                Catch ex As Exception
                    MessageBox.Show(ex.Message)
                End Try

            End If
        End If
    End Sub

    Private Sub ShowBtnSelectDrc()
        RadBtnActePM.Show()
        RadBtnSlectProtocole.Show()
        RadBtnSelectParm.Show()
        RadBtnMesurePreventive.Show()
    End Sub

End Class
