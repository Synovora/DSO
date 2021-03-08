Imports Oasis_Common

Public Class RadFDrcActePMAssocieEdit
    Private _drcId As Long

    Public Property ProtocoleCollaboratifDrcId As Long
        Get
            Return _drcId
        End Get
        Set(value As Long)
            _drcId = value
        End Set
    End Property

    ReadOnly drcActeParamedicalAssoDao As New DrcActeParamedicalAssoDao
    ReadOnly drcdao As New DrcDao
    ReadOnly ListDrcAssocie As List(Of Long) = New List(Of Long)

    Private Sub RadFDrcActePMAssocieEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim drc As New Drc
        drcdao.GetDrc(drc, ProtocoleCollaboratifDrcId)
        LblDrcDenomination.Text = drc.DrcLibelle
        ChargementActePMAssocies()
        ChargementDrc()
    End Sub

    Private Sub ChargementActePMAssocies()
        RadGridViewDrcAsso.Rows.Clear()
        ListDrcAssocie.Clear()

        Dim DrcDataTable As DataTable
        Dim ActeParamedicalDrcId As Long
        DrcDataTable = drcActeParamedicalAssoDao.GetAllActeParamedicalAssoByProtocoleCollaboratifId(ProtocoleCollaboratifDrcId)
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = DrcDataTable.Rows.Count - 1
        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            Dim drc As Drc = New Drc
            ActeParamedicalDrcId = Coalesce(DrcDataTable.Rows(i)("drc_acte_paramedical_id"), 0)
            If ActeParamedicalDrcId <> 0 Then
                drcdao.GetDrc(drc, ActeParamedicalDrcId)
                If ListDrcAssocie.Contains(ActeParamedicalDrcId) = False Then
                    ListDrcAssocie.Add(ActeParamedicalDrcId)
                End If
            Else
                Continue For
            End If

            iGrid += 1
            RadGridViewDrcAsso.Rows.Add(iGrid)
            RadGridViewDrcAsso.Rows(iGrid).Cells("id").Value = DrcDataTable.Rows(i)("id")
            RadGridViewDrcAsso.Rows(iGrid).Cells("denomination").Value = drc.DrcLibelle
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewDrcAsso.Rows.Count > 0 Then
            Me.RadGridViewDrcAsso.CurrentRow = RadGridViewDrcAsso.Rows(0)
        End If
    End Sub

    Private Sub ChargementDrc()
        DrcDataGridView.Rows.Clear()

        Dim drcDataTable As DataTable
        Dim CategorieOasis As Integer = Drc.EnumCategorieOasisCode.ActeParamedical
        Dim SelectAld As Boolean = False
        drcDataTable = drcdao.GetAllDrcByCategorieAndGenre("", 0, CategorieOasis, SelectAld, "")

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = drcDataTable.Rows.Count - 1
        Dim drcIdEnCours, Sexe As Integer

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        'drcIdPrecedent = 0
        For i = 0 To rowCount Step 1
            'Ne pas traiter les doublons liées à la requête (JOIN LEFT)
            drcIdEnCours = CInt(drcDataTable.Rows(i)("oa_drc_id"))
            'If drcIdEnCours = drcIdPrecedent Then
            'Continue For
            'Else
            'drcIdPrecedent = drcIdEnCours
            'End If

            If ListDrcAssocie.Contains(drcIdEnCours) = True Then
                Continue For
            End If

            iGrid += 1
            'Ajout d'une ligne au DataGridView
            DrcDataGridView.Rows.Add(iGrid)
            'Alimentation du Grid
            DrcDataGridView.Rows(iGrid).Cells("drcId").Value = drcDataTable.Rows(i)("oa_drc_id")
            DrcDataGridView.Rows(iGrid).Cells("drcDescription").Value = drcDataTable.Rows(i)("oa_drc_libelle")

            Sexe = Coalesce(drcDataTable.Rows(i)("oa_drc_sexe"), 0)
            Select Case Sexe
                Case 1
                    DrcDataGridView.Rows(iGrid).Cells("contexte").Value = "Homme"
                Case 2
                    DrcDataGridView.Rows(iGrid).Cells("contexte").Value = "Femme"
                Case 3
                    DrcDataGridView.Rows(iGrid).Cells("contexte").Value = "Homme et Femme"
                Case Else
                    DrcDataGridView.Rows(iGrid).Cells("contexte").Value = "Inconnu"
            End Select

            If Coalesce(drcDataTable.Rows(i)("oa_drc_oasis"), False) = True Then
                DrcDataGridView.Rows(iGrid).Cells("drcOasis").Value = "Oasis"
                DrcDataGridView.Rows(iGrid).Cells("drcOasis").Style.ForeColor = Color.Red
            Else
                DrcDataGridView.Rows(iGrid).Cells("drcOasis").Value = ""
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If DrcDataGridView.Rows.Count > 0 Then
            Me.DrcDataGridView.CurrentRow = DrcDataGridView.ChildRows(0)
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnSelect_Click(sender As Object, e As EventArgs) Handles RadBtnSelect.Click
        If DrcDataGridView.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.DrcDataGridView.Rows.IndexOf(Me.DrcDataGridView.CurrentRow)
            If aRow >= 0 Then
                Dim SelectedDrcId As Integer = DrcDataGridView.Rows(aRow).Cells("drcId").Value
                'Sélection de la DRC et association avec le protoole collaboratif
                If SelectedDrcId <> 0 Then
                    'Ajout de l'occurrence choisie (contrôle que cette DORC n'est pas déjà associée)
                    Dim drcActeParamedicalAsso As DrcActeParamedicalAsso = New DrcActeParamedicalAsso With {
                        .ProtocleCollabaratifDrcId = ProtocoleCollaboratifDrcId,
                        .ActeParamedicalDrcId = SelectedDrcId
                    }
                    Try
                        If drcActeParamedicalAssoDao.CreateDrcActeParamedicalAsso(drcActeParamedicalAsso) = True Then
                            ChargementActePMAssocies()
                            ChargementDrc()
                        End If
                    Catch ex As Exception
                        CreateLog(ex.ToString, Me.Name, Log.EnumTypeLog.ERREUR.ToString, userLog)
                        If ex.Message.StartsWith("Collisio") = True Then
                            MessageBox.Show("L'acte médical sélectionné existe déjà pour le protocole collaboratif")
                        End If
                    End Try
                End If
            End If
        End If
    End Sub

    Private Sub RadBtnSuppprimer_Click(sender As Object, e As EventArgs) Handles RadBtnSuppprimer.Click
        If RadGridViewDrcAsso.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewDrcAsso.Rows.IndexOf(Me.RadGridViewDrcAsso.CurrentRow)
            If aRow >= 0 Then
                Dim DrcId As Integer = RadGridViewDrcAsso.Rows(aRow).Cells("Id").Value
                'Suppression de l'association de la DRC
                If drcActeParamedicalAssoDao.SuppressionDrcActeParamedicalAsso(DrcId) = True Then
                    ChargementActePMAssocies()
                    ChargementDrc()
                End If
            End If
        End If
    End Sub
End Class
