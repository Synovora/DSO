Imports System.Diagnostics
Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class RadFValenceSelecteur
    Property Patient As Patient
    Property ValencesVisible As List(Of CGVValence)
    Property ValencesNotVisible As List(Of Valence)

    ReadOnly valenceDao As New ValenceDao
    ReadOnly cgvValenceDao As New CGVValenceDao
    ReadOnly cgvDateDao As New CGVDateDao

    Private Sub RadFValenceSelecteur_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        ChargementValence()
    End Sub

    Private Sub ChargementValence()
        RGVValenceVisible.Rows.Clear()
        RGVValenceNotVisible.Rows.Clear()

        ValencesVisible = cgvValenceDao.GetListFromPatient(Patient.PatientId)
        Dim iGrid As Integer = 0
        For Each valence As CGVValence In ValencesVisible
            RGVValenceVisible.Rows.Add(iGrid)
            RGVValenceVisible.Rows(iGrid).Cells(0).Value = valence.Id
            RGVValenceVisible.Rows(iGrid).Cells(1).Value = valence.Code
            RGVValenceVisible.Rows(iGrid).Cells(2).Value = valence.Description
            RGVValenceVisible.Rows(iGrid).Cells("order").Value = valence.Ordre
            iGrid += 1
        Next

        ValencesNotVisible = valenceDao.GetList()
        iGrid = 0
        For Each valence As Valence In ValencesNotVisible
            If ValencesVisible.FindIndex(Function(myObject) myObject.Valence = valence.Id) <> -1 Then
                Continue For
            End If
            RGVValenceNotVisible.Rows.Add(iGrid)
            RGVValenceNotVisible.Rows(iGrid).Cells(0).Value = valence.Id
            RGVValenceNotVisible.Rows(iGrid).Cells(1).Value = valence.Code
            RGVValenceNotVisible.Rows(iGrid).Cells(2).Value = valence.Description
            iGrid += 1
        Next
    End Sub

    Private Sub Grid_ToolTipTextNeeded(ByVal sender As Object, ByVal e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RGVValenceVisible.ToolTipTextNeeded, RGVValenceNotVisible.ToolTipTextNeeded
        Dim dataCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If dataCell IsNot Nothing Then
            Dim textPart As New TextPart(dataCell)
            Dim size As SizeF = textPart.Measure(New SizeF(Single.PositiveInfinity, Single.PositiveInfinity))
            Dim sizeInCell As SizeF = textPart.Measure(New SizeF(dataCell.ColumnInfo.Width, Single.PositiveInfinity))
            Dim toolTipText As String = Nothing
            Dim cellWidth As Single = dataCell.ColumnInfo.Width

            If TypeOf dataCell.MasterTemplate.ViewDefinition Is HtmlViewDefinition Then
                cellWidth = (CType(dataCell.TableElement.ViewElement.RowLayout, HtmlViewRowLayout)).GetArrangeInfo(dataCell.ColumnInfo).Bounds.Width - dataCell.BorderWidth * 2
            End If

            Dim cellHeight As Single = dataCell.Size.Height - dataCell.BorderWidth * 2

            If size.Width > cellWidth OrElse cellHeight < sizeInCell.Height Then
                toolTipText = dataCell.Text
            End If

            e.ToolTipText = toolTipText
        End If
    End Sub

    Private Sub BtnAdd_Click(sender As Object, e As EventArgs) Handles BtnAdd.Click
        Dim aRow As Integer = RGVValenceNotVisible.CurrentRow.Index
        If aRow >= 0 Then
            Dim valenceId = RGVValenceNotVisible.Rows(aRow).Cells(0).Value
            Dim valence = ValencesNotVisible.Find(Function(p) p.Id = Long.Parse(valenceId))
            cgvValenceDao.Create(New CGVValence() With {
                          .Code = valence.Code,
                          .Description = valence.Description,
                          .Precaution = valence.Precaution,
                          .Valence = valence.Id,
                          .Patient = Patient.PatientId
            })
            ChargementValence()
        End If
    End Sub

    Private Sub BtnRemove_Click(sender As Object, e As EventArgs) Handles BtnRemove.Click
        Dim aRow As Integer = RGVValenceVisible.CurrentRow.Index
        If aRow >= 0 Then
            Dim valenceId = RGVValenceVisible.Rows(aRow).Cells(0).Value
            Dim valenceOrder = Integer.Parse(RGVValenceVisible.Rows(aRow).Cells("order").Value)

            Dim relations = cgvDateDao.GetRelationListFromPatient(Patient.PatientId)
            Dim actualRelations = relations.FindAll(Function(myObject) myObject.Valence = valenceId)
            Dim index = 1
            If actualRelations.Count > 0 Then
                Dim result As DialogResult = MessageBox.Show("Des dates ont été programmées pour cette valence, etes-vous sure de vouloir inactiver cette valence du calendrier general", "caption", MessageBoxButtons.YesNo)
                If result = DialogResult.No Then
                    Return
                End If
                For Each relation As RelationValenceDate In actualRelations
                    cgvDateDao.DeleteRelation(relation)
                Next
            End If


            Dim valenceFrom = valenceDao.GetListFromOrder(valenceOrder + 1)
            cgvValenceDao.Delete(New CGVValence() With {.Id = Long.Parse(valenceId)})
            For Each valence As Valence In valenceFrom
                valenceDao.SetOrder(valence.Id, valence.Ordre - 1)
            Next

            ChargementValence()
        End If
    End Sub

    Private Sub BtnUp_Click(sender As Object, e As EventArgs) Handles BtnUp.Click
        Dim aRow As Integer = RGVValenceVisible.CurrentRow.Index
        If aRow >= 0 Then
            Dim valenceId = RGVValenceVisible.Rows(aRow).Cells(0).Value
            Dim valenceOrder = RGVValenceVisible.Rows(aRow).Cells("order").Value
            If valenceOrder > 0 Then
                Dim swapValence = valenceDao.GetByOrder(Integer.Parse(valenceOrder) - 1)
                valenceDao.SetOrder(valenceId, Integer.Parse(valenceOrder) - 1)
                valenceDao.SetOrder(swapValence.Id, swapValence.Ordre + 1)
            End If
            ChargementValence()
        End If
    End Sub

    Private Sub Button1_Click(sender As Object, e As EventArgs) Handles Button1.Click
        Dim aRow As Integer = RGVValenceVisible.CurrentRow.Index
        If aRow >= 0 Then
            Dim valenceId = RGVValenceVisible.Rows(aRow).Cells(0).Value
            Dim valenceOrder = RGVValenceVisible.Rows(aRow).Cells("order").Value
            Dim lastValence = valenceDao.GetLastOrder()
            If lastValence IsNot Nothing AndAlso valenceOrder < lastValence.Ordre Then
                Dim swapValence = valenceDao.GetByOrder(Integer.Parse(valenceOrder) + 1)
                valenceDao.SetOrder(valenceId, Integer.Parse(valenceOrder) + 1)
                valenceDao.SetOrder(swapValence.Id, swapValence.Ordre - 1)
            End If
            ChargementValence()
        End If
    End Sub

    Private Sub BtnAddValence_Click(sender As Object, e As EventArgs)
        Using formSelecteur As New RadFValenceCreation
            formSelecteur.ShowDialog()
            ChargementValence()
        End Using
    End Sub

    Private Sub BtnAddValence_Click_1(sender As Object, e As EventArgs) Handles BtnAddValence.Click

    End Sub
End Class
