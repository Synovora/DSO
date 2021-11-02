Imports System.Collections.Specialized
Imports Telerik.WinControls.UI
Imports Oasis_Common
Imports System.Text.RegularExpressions

Public Class RadFCGV

    ReadOnly cgvValenceDao As New CGVValenceDao
    ReadOnly cgvDateDao As New CGVDateDao
    Dim valences As List(Of CGVValence)
    Dim relations As List(Of RelationValenceDate) = New List(Of RelationValenceDate)
    Dim cgvDates As List(Of CGVDate)

    Private Sub RadFATCListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Vaccin - Calendrier General", userLog)
        RBDateAll.CheckState = True
        ChargementValence()
        ChargementDate()
    End Sub

    Shared Function DaysToDate(days As Long) As String
        Dim dayPerMonth = 30.44
        Dim monthPerYear = 12
        Dim showMaxMonths = 40
        If days < dayPerMonth Then
            Return String.Format("{0} Jours", Math.Round(days))
        ElseIf days / dayPerMonth < showMaxMonths Then
            Return String.Format("{0} Mois", Math.Round(days / dayPerMonth))
        Else
            Return String.Format("{0} Ans", Math.Round(days / dayPerMonth / monthPerYear))
        End If
    End Function

    Shared Function DateToDays(days As Long, months As Long, years As Long) As Long
        Dim dayPerMonth = 30.44
        Dim monthPerYear = 12
        Return Math.Round(days + months * dayPerMonth + years * monthPerYear * dayPerMonth)
    End Function

    Private Sub ChargementValence()
        Grid.Columns.Clear()
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Dim iGrid As Integer = 0

        Grid.Columns.Add("Age")
        Grid.Columns(0).Width = 50

        valences = cgvValenceDao.GetListFromPatient(0)
        For Each valence As CGVValence In valences
            'If (valence.Visible = False) Then Continue For
            iGrid += 1
            Grid.Columns.Add(valence.Code)
            Grid.Columns(iGrid).Width = 80
            Grid.Columns(iGrid).TextAlignment = ContentAlignment.MiddleCenter
        Next
        Grid.Enabled = True
        ChargementDate()
        Cursor.Current = Cursors.Default
        Me.Enabled = True
    End Sub

    Private Sub ChargementDate()
        Grid.Rows.Clear()
        TextDay.Text = ""
        TextMonth.Text = ""
        TextYear.Text = ""
        Dim iGrid As Integer = 0

        cgvDates = cgvDateDao.GetListFromPatient(0)
        relations = cgvDateDao.GetRelationListFromPatient(0)
        cgvDates.Sort(Function(x, y) x.Days - y.Days)
        For Each cgvDate As CGVDate In cgvDates
            Dim actualRelations = relations.FindAll(Function(myObject) myObject.Date = cgvDate.Id)
            If (RBDateActif.CheckState AndAlso actualRelations.Count = 0) OrElse (RBDateInactif.CheckState AndAlso actualRelations.Count > 0) Then
                Continue For
            End If
            Grid.Rows.Add(iGrid)
            Grid.Rows(iGrid).Cells(0).Value = DaysToDate(cgvDate.Days)
            For Each actualRelation As RelationValenceDate In actualRelations
                Dim valence = valences.Find(Function(myObject) myObject.Id = actualRelation.Valence)
                'If (valence.Visible = False) Then Continue For
                Grid.Rows(iGrid).Cells(Grid.Columns.IndexOf(valence.Code)).Value = "✓"
            Next
            iGrid += 1
        Next
        Grid.Enabled = True
    End Sub

    Private Sub Grid_ToolTipTextNeeded(ByVal sender As Object, ByVal e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles Grid.ToolTipTextNeeded
        Dim dataCell As GridDataCellElement = TryCast(sender, GridDataCellElement)

        If dataCell IsNot Nothing Then
            Dim textPart As TextPart = New TextPart(dataCell)
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

    Public Function IsNumeric(input As String) As Boolean
        Return Regex.IsMatch(input.Trim, "^\d+$")
    End Function

    Shared Function IsValid(ByVal value As String, min As Long, max As Long) As Long
        Dim inputAsInteger As Integer = 0
        If Integer.TryParse(value, inputAsInteger) Then
            Return Math.Min(Math.Max(inputAsInteger, min), max)
        End If
        Return 0
    End Function

    Private Sub TextDay_TextChanged() Handles TextDay.TextChanged
        If IsNumeric(TextDay.Text) Then
            'TextDay.Text = IsValid(TextDay.Text, 0, 30).ToString()
            TextMonth.Text = ""
            TextYear.Text = ""
        Else
            TextDay.Text = ""
        End If
    End Sub
    Private Sub TextMonth_TextChanged() Handles TextMonth.TextChanged
        If IsNumeric(TextMonth.Text) Then
            'TextMonth.Text = IsValid(TextMonth.Text, 0, 40).ToString()
            TextDay.Text = ""
            TextYear.Text = ""
        Else
            TextMonth.Text = ""
        End If
    End Sub
    Private Sub TextYear_TextChanged() Handles TextYear.TextChanged
        If IsNumeric(TextYear.Text) Then
            'TextYear.Text = IsValid(TextYear.Text, 0, 120).ToString()
            TextDay.Text = ""
            TextMonth.Text = ""
        Else
            TextYear.Text = ""
        End If
    End Sub

    Private Sub BtnDateAdd_Click(sender As Object, e As EventArgs) Handles BtnDateAdd.Click
        Dim d, m, y
        Integer.TryParse(TextDay.Text, d)
        Integer.TryParse(TextMonth.Text, m)
        Integer.TryParse(TextYear.Text, y)

        Dim days = DateToDays(d, m, y)
        If days = 0 Then
            MessageBox.Show("Veuillez rentrer une date", "Calendrier", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            Return
        End If

        Dim cgvDate = New CGVDate With {
        .Days = days,
        .Patient = 0
        }

        If cgvDateDao.GetByDaysPatient(cgvDate) Is Nothing AndAlso cgvDateDao.Create(cgvDate) Then
            ChargementDate()
        Else
            MessageBox.Show("Cette date est deja existante", "Calendrier", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub BtnDateDelete_Click(sender As Object, e As EventArgs) Handles BtnDateDelete.Click
        Dim aRow As Integer = Grid.Rows.IndexOf(Grid.CurrentRow)
        If aRow >= 0 Then
            Dim cgvDateId As String = cgvDates(aRow).Id
            Dim actualRelations = relations.FindAll(Function(myObject) myObject.Date = cgvDateId)
            Dim index = 1
            For Each Col In Grid.Columns
                If Grid.CurrentRow.Cells(index).Value IsNot Nothing Then
                    Dim result As DialogResult = MessageBox.Show("Pour cette date des valences ont été programmées, etes-vous sure de vouloir supprimer cette date", "Calendrier", MessageBoxButtons.YesNo, MessageBoxIcon.Question)
                    If result = DialogResult.Yes Then
                        Exit For
                    ElseIf result = DialogResult.No Then
                        Return
                    End If
                End If
                index += 1
            Next

            cgvDateDao.Delete(New CGVDate() With {.Id = cgvDateId})
            ChargementValence()
        End If
    End Sub

    Private Sub RadButtonEditValence_Click(sender As Object, e As EventArgs) Handles RadButtonEditValence.Click
        Using form As New RadFValenceSelecteur
            form.Patient = New Patient With {
                .PatientId = 0
            }
            form.ShowDialog()
            ChargementValence()
        End Using
    End Sub

    Private Sub Grid_Click(sender As Object, ByVal e As GridViewCellEventArgs) Handles Grid.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex > 0 Then
            Dim dateRow As Integer = e.RowIndex
            Dim valenceCol As Integer = e.ColumnIndex - 1

            Dim mydate = cgvDates(dateRow)
            Dim valence = valences(valenceCol)
            If dateRow >= 0 AndAlso valenceCol >= 0 Then
                If (cgvDateDao.RelationExist(New RelationValenceDate() With {.Date = mydate.Id, .Valence = valence.Id, .Patient = 0})) Then
                    cgvDateDao.DeleteRelation(New RelationValenceDate() With {.Date = mydate.Id, .Valence = valence.Id, .Patient = 0})
                Else
                    cgvDateDao.CreateRelation(New RelationValenceDate() With {.Date = mydate.Id, .Valence = valence.Id, .Patient = 0})
                End If
                ChargementValence()
            End If
        End If
    End Sub

    Private Sub RadChkPatientTous_ToggleStateChanged(sender As Object, args As Telerik.WinControls.UI.StateChangedEventArgs) Handles RBDateAll.ToggleStateChanged, RBDateActif.ToggleStateChanged, RBDateInactif.ToggleStateChanged

        If RBDateAll.CheckState = CheckState.Checked OrElse RBDateActif.CheckState = CheckState.Checked OrElse RBDateInactif.CheckState = CheckState.Checked Then
            Application.DoEvents()
            ChargementValence()
        End If

    End Sub
End Class
