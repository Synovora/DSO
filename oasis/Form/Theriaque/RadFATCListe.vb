Imports Telerik.WinControls.UI

Public Class RadFATCListe

    Dim theriaqueDao As New TheriaqueDao

    Private Sub RadFATCListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RadioBtnVirtuel.Checked = True
        ChargementATC1()
        LblOccurrencesLues.Text = ""
    End Sub

    'Liste des médicaments par dénomination
    Private Sub RadBtnFiltre_Click(sender As Object, e As EventArgs) Handles RadBtnFiltreSpecialite.Click
        Dim NbCar As Integer = RadTxtSpecialite.Text.Length
        If RadTxtSpecialite.Text <> "" And NbCar > 2 Then
            Cursor.Current = Cursors.WaitCursor
            '1 - Recherche spécialités virtuelles en nom partiel
            Dim dt As DataTable
            Dim NomSpecialite As String = "%" & RadTxtSpecialite.Text & "%"
            dt = ChargementSpecialiteByNomSpecialite(NomSpecialite, TheriaqueDao.EnumMonoVir.VIRTUEL)
            If dt.Rows.Count > 0 Then
                ChargementSpecialite(dt)
            Else
                '2 - Recherche spécialités classiques en nom complet
                NomSpecialite = "%" & RadTxtSpecialite.Text & " %"
                dt = ChargementSpecialiteByNomSpecialite(NomSpecialite, TheriaqueDao.EnumMonoVir.CLASSIQUE)
                If dt.Rows.Count > 0 Then
                    ChargementSpecialite(dt)
                Else
                    '3 - Recherche spécialités classiques en nom partiel
                    NomSpecialite = "%" & RadTxtSpecialite.Text & "%"
                    dt = ChargementSpecialiteByNomSpecialite(NomSpecialite, TheriaqueDao.EnumMonoVir.CLASSIQUE)
                    ChargementSpecialite(dt)
                End If
            End If
            Cursor.Current = Cursors.Default
        Else
            MessageBox.Show("Vous devez saisir au moins 3 caractères pour lancer cette option de recherche de médicaments !")
        End If
    End Sub

    Private Sub ChargementATC1()
        RadGridViewATC1.Rows.Clear()
        Dim dt As DataTable
        dt = theriaqueDao.GetAllATC()

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            RadGridViewATC1.Rows.Add(iGrid)

            RadGridViewATC1.Rows(iGrid).Cells("catc_code_pk").Value = dt.Rows(i)("catc_code_pk")
            RadGridViewATC1.Rows(iGrid).Cells("catc_nomf").Value = dt.Rows(i)("catc_nomf")
        Next

        If RadGridViewATC1.Rows.Count > 0 Then
            RadGridViewATC1.CurrentRow = RadGridViewATC1.ChildRows(0)
            RadGridViewATC1.TableElement.VScrollBar.Value = 0
        End If
    End Sub

    Private Sub ChargementATC2(CodeATC As String)
        RadGridViewATC2.Rows.Clear()
        Dim dt As DataTable
        dt = theriaqueDao.getATCByATC(CodeATC)

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            RadGridViewATC2.Rows.Add(iGrid)

            RadGridViewATC2.Rows(iGrid).Cells("catc_code_pk").Value = dt.Rows(i)("catc_code_pk")
            RadGridViewATC2.Rows(iGrid).Cells("catc_nomf").Value = dt.Rows(i)("catc_nomf")
        Next

        If RadGridViewATC2.Rows.Count > 0 Then
            RadGridViewATC2.CurrentRow = RadGridViewATC2.ChildRows(0)
            RadGridViewATC2.TableElement.VScrollBar.Value = 0
        End If
    End Sub

    Private Sub ChargementATC3(CodeATC As String)
        RadGridViewATC3.Rows.Clear()
        Dim dt As DataTable
        dt = theriaqueDao.getATCByATC(CodeATC)

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            RadGridViewATC3.Rows.Add(iGrid)

            RadGridViewATC3.Rows(iGrid).Cells("catc_code_pk").Value = dt.Rows(i)("catc_code_pk")
            RadGridViewATC3.Rows(iGrid).Cells("catc_nomf").Value = dt.Rows(i)("catc_nomf")
        Next

        If RadGridViewATC3.Rows.Count > 0 Then
            RadGridViewATC3.CurrentRow = RadGridViewATC3.ChildRows(0)
            RadGridViewATC3.TableElement.VScrollBar.Value = 0
        End If
    End Sub

    Private Sub ChargementATC4(CodeATC As String)
        RadGridViewATC4.Rows.Clear()
        Dim dt As DataTable
        dt = theriaqueDao.getATCByATC(CodeATC)

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            RadGridViewATC4.Rows.Add(iGrid)

            RadGridViewATC4.Rows(iGrid).Cells("catc_code_pk").Value = dt.Rows(i)("catc_code_pk")
            RadGridViewATC4.Rows(iGrid).Cells("catc_nomf").Value = dt.Rows(i)("catc_nomf")
        Next

        If RadGridViewATC4.Rows.Count > 0 Then
            RadGridViewATC4.CurrentRow = RadGridViewATC4.ChildRows(0)
            RadGridViewATC4.TableElement.VScrollBar.Value = 0
        End If
    End Sub

    Private Sub MasterTemplate_Click(sender As Object, e As EventArgs) Handles RadGridViewATC1.Click
        If RadGridViewATC1.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC1.Rows.IndexOf(Me.RadGridViewATC1.CurrentRow)
            Dim ATCCode As String = RadGridViewATC1.Rows(aRow).Cells("catc_code_pk").Value
            If aRow >= 0 Then
                RadGridViewATC3.Rows.Clear()
                RadGridViewATC4.Rows.Clear()
                ChargementATC2(ATCCode)
            End If
        End If
    End Sub

    Private Sub RadGridViewATC2_Click(sender As Object, e As EventArgs) Handles RadGridViewATC2.Click
        If RadGridViewATC2.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC2.Rows.IndexOf(Me.RadGridViewATC2.CurrentRow)
            Dim ATCCode As String = RadGridViewATC2.Rows(aRow).Cells("catc_code_pk").Value
            If aRow >= 0 Then
                RadGridViewATC4.Rows.Clear()
                ChargementATC3(ATCCode)
            End If
        End If
    End Sub

    Private Sub RadGridViewATC3_Click(sender As Object, e As EventArgs) Handles RadGridViewATC3.Click
        If RadGridViewATC3.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC3.Rows.IndexOf(Me.RadGridViewATC3.CurrentRow)
            Dim ATCCode As String = RadGridViewATC3.Rows(aRow).Cells("catc_code_pk").Value
            If aRow >= 0 Then
                ChargementATC4(ATCCode)
            End If
        End If
    End Sub

    'Liste des médicaments ATC niveau 2
    Private Sub RadBtnSpec1_Click(sender As Object, e As EventArgs) Handles RadBtnSpec2.Click
        If RadGridViewATC2.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC2.Rows.IndexOf(Me.RadGridViewATC2.CurrentRow)
            If aRow >= 0 Then
                Dim CodeAtc As String = RadGridViewATC2.Rows(aRow).Cells("catc_code_pk").Value
                ChargementSpecialiteByATC(CodeAtc)
            End If
        End If
    End Sub

    'Liste des médicaments ATC niveau 3
    Private Sub RadBtnSpec2_Click(sender As Object, e As EventArgs) Handles RadBtnSpec3.Click
        If RadGridViewATC3.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC3.Rows.IndexOf(Me.RadGridViewATC3.CurrentRow)
            If aRow >= 0 Then
                Dim CodeAtc As String = RadGridViewATC3.Rows(aRow).Cells("catc_code_pk").Value
                ChargementSpecialiteByATC(CodeAtc)
            End If
        End If
    End Sub

    'Liste des médicaments ATC niveau 4
    Private Sub RadBtnSpec4_Click(sender As Object, e As EventArgs) Handles RadBtnSpec4.Click
        If RadGridViewATC4.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC4.Rows.IndexOf(Me.RadGridViewATC4.CurrentRow)
            If aRow >= 0 Then
                Dim CodeAtc As String = RadGridViewATC4.Rows(aRow).Cells("catc_code_pk").Value
                ChargementSpecialiteByATC(CodeAtc)
            End If
        End If
    End Sub

    Private Sub ChargementSpecialiteByATC(CodeAtc As String)
        Cursor.Current = Cursors.WaitCursor

        Dim Monovir As Integer = GetMonovir()
        CodeAtc &= "%"

        Dim dt As DataTable
        dt = theriaqueDao.getSpecialiteByArgument(CodeAtc, TheriaqueDao.EnumGetSpecialite.CLASSE_ATC, Monovir)
        ChargementSpecialite(dt)

        Cursor.Current = Cursors.Default
    End Sub

    Private Function ChargementSpecialiteByNomSpecialite(NomSpecialite As String, Monovir As Integer) As DataTable
        Dim dt As DataTable
        dt = theriaqueDao.getSpecialiteByArgument(NomSpecialite, TheriaqueDao.EnumGetSpecialite.NOM_SPECIALITE, Monovir)
        Return dt
    End Function

    Private Sub ChargementSpecialite(dt As DataTable)
        RadGridViewSpe.Rows.Clear()
        RadGridViewSpe.FilterDescriptors.Clear()

        Dim NombreOccurrencesLues As Integer = dt.Rows.Count
        If NombreOccurrencesLues > 1 Then
            LblOccurrencesLues.Text = NombreOccurrencesLues & " occurrences correspondant aux critères de recherche"
        Else
            LblOccurrencesLues.Text = NombreOccurrencesLues & " occurrence correspondant aux critères de recherche"
        End If

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            RadGridViewSpe.Rows.Add(iGrid)

            RadGridViewSpe.Rows(iGrid).Cells("SP_CODE_SQ_PK").Value = dt.Rows(i)("SP_CODE_SQ_PK")
            RadGridViewSpe.Rows(iGrid).Cells("SP_CATC_CODE_FK").Value = dt.Rows(i)("SP_CATC_CODE_FK")
            RadGridViewSpe.Rows(iGrid).Cells("SP_PR_CODE_FK").Value = dt.Rows(i)("SP_PR_CODE_FK")
            RadGridViewSpe.Rows(iGrid).Cells("SP_NOM").Value = dt.Rows(i)("SP_NOM")
            RadGridViewSpe.Rows(iGrid).Cells("SP_NOMCOMP").Value = dt.Rows(i)("SP_NOMCOMP")
            RadGridViewSpe.Rows(iGrid).Cells("SP_NOMLONG").Value = dt.Rows(i)("SP_NOMLONG")
            RadGridViewSpe.Rows(iGrid).Cells("SP_CIPUCD").Value = dt.Rows(i)("SP_CIPUCD")
        Next

        If RadGridViewSpe.Rows.Count > 0 Then
            RadGridViewSpe.CurrentRow = RadGridViewSpe.ChildRows(0)
            RadGridViewSpe.TableElement.VScrollBar.Value = 0
        End If
    End Sub

    Private Function GetMonovir() As Integer
        Dim Monovir As Integer

        If RadioBtnVirtuel.Checked = True Then
            Monovir = 1
        Else
            Monovir = 0
        End If

        Return Monovir
    End Function


    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub MasterTemplate_CellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs) Handles RadGridViewSpe.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub
End Class
