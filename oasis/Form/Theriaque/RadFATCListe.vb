Public Class RadFATCListe

    Dim CodeAtc2, CodeAtc3 As String
    Private Sub RadFATCListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementATC1()
    End Sub

    Private Sub RadBtnFiltre_Click(sender As Object, e As EventArgs) Handles RadBtnFiltre.Click
        If RadTxtATCFiltre.Text <> "" Then
            ChargementATC2(RadTxtATCFiltre.Text)
        End If
    End Sub

    Private Sub ChargementATC1()
        RadGridViewATC1.Rows.Clear()
        Dim dt As DataTable
        Dim theriaqueDao As New TheriaqueDao
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
            Me.RadGridViewATC1.CurrentRow = RadGridViewATC1.Rows(0)
        End If
    End Sub

    Private Sub ChargementATC2(CodeATC As String)
        RadGridViewATC2.Rows.Clear()
        Dim dt As DataTable
        Dim theriaqueDao As New TheriaqueDao
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
            Me.RadGridViewATC2.CurrentRow = RadGridViewATC2.Rows(0)
        End If
    End Sub

    Private Sub ChargementATC3(CodeATC As String)
        RadGridViewATC3.Rows.Clear()
        Dim dt As DataTable
        Dim theriaqueDao As New TheriaqueDao
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
            Me.RadGridViewATC3.CurrentRow = RadGridViewATC3.Rows(0)
        End If
    End Sub

    Private Sub ChargementATC4(CodeATC As String)
        RadGridViewATC4.Rows.Clear()
        Dim dt As DataTable
        Dim theriaqueDao As New TheriaqueDao
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
            Me.RadGridViewATC4.CurrentRow = RadGridViewATC4.Rows(0)
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
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

    Private Sub RadBtnSpec1_Click(sender As Object, e As EventArgs) Handles RadBtnSpec2.Click
        If RadGridViewATC2.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC2.Rows.IndexOf(Me.RadGridViewATC2.CurrentRow)
            If aRow >= 0 Then
                CodeAtc2 = RadGridViewATC2.Rows(aRow).Cells("catc_code_pk").Value
                RadGridViewSpe.Rows.Clear()
                Dim dt As DataTable
                Dim theriaqueDao As New TheriaqueDao
                dt = theriaqueDao.getSpecialiteByATC(CodeAtc2)

                Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
                Dim rowCount As Integer = dt.Rows.Count - 1

                For i = 0 To rowCount Step 1
                    iGrid += 1
                    RadGridViewSpe.Rows.Add(iGrid)

                    RadGridViewSpe.Rows(iGrid).Cells("SP_CODE_SQ_PK").Value = dt.Rows(i)("SP_CODE_SQ_PK")
                    RadGridViewSpe.Rows(iGrid).Cells("SP_CATC_CODE_FK").Value = dt.Rows(i)("SP_CATC_CODE_FK")
                    RadGridViewSpe.Rows(iGrid).Cells("SP_PR_CODE_FK").Value = dt.Rows(i)("SP_PR_CODE_FK")
                    RadGridViewSpe.Rows(iGrid).Cells("SP_NOM").Value = dt.Rows(i)("SP_NOM")
                    RadGridViewSpe.Rows(iGrid).Cells("SP_CIPUCD").Value = dt.Rows(i)("SP_CIPUCD")
                Next

                If RadGridViewSpe.Rows.Count > 0 Then
                    Me.RadGridViewSpe.CurrentRow = RadGridViewSpe.Rows(0)
                End If
            End If
        End If
    End Sub

    Private Sub RadBtnSpec2_Click(sender As Object, e As EventArgs) Handles RadBtnSpec3.Click
        If RadGridViewATC3.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC3.Rows.IndexOf(Me.RadGridViewATC3.CurrentRow)
            If aRow >= 0 Then
                CodeAtc3 = RadGridViewATC3.Rows(aRow).Cells("catc_code_pk").Value
                RadGridViewSpe.Rows.Clear()
                Dim dt As DataTable
                Dim theriaqueDao As New TheriaqueDao
                dt = theriaqueDao.getSpecialiteByATC(CodeAtc3)

                Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
                Dim rowCount As Integer = dt.Rows.Count - 1

                For i = 0 To rowCount Step 1
                    iGrid += 1
                    RadGridViewSpe.Rows.Add(iGrid)

                    RadGridViewSpe.Rows(iGrid).Cells("SP_CODE_SQ_PK").Value = dt.Rows(i)("SP_CODE_SQ_PK")
                    RadGridViewSpe.Rows(iGrid).Cells("SP_CATC_CODE_FK").Value = dt.Rows(i)("SP_CATC_CODE_FK")
                    RadGridViewSpe.Rows(iGrid).Cells("SP_PR_CODE_FK").Value = dt.Rows(i)("SP_PR_CODE_FK")
                    RadGridViewSpe.Rows(iGrid).Cells("SP_NOM").Value = dt.Rows(i)("SP_NOM")
                    RadGridViewSpe.Rows(iGrid).Cells("SP_CIPUCD").Value = dt.Rows(i)("SP_CIPUCD")
                Next

                If RadGridViewSpe.Rows.Count > 0 Then
                    Me.RadGridViewSpe.CurrentRow = RadGridViewSpe.Rows(0)
                End If
            End If
        End If
    End Sub

    Private Sub RadBtnSpec4_Click(sender As Object, e As EventArgs) Handles RadBtnSpec4.Click
        If RadGridViewATC4.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC4.Rows.IndexOf(Me.RadGridViewATC4.CurrentRow)
            If aRow >= 0 Then
                Dim CodeAtc As String = RadGridViewATC4.Rows(aRow).Cells("catc_code_pk").Value
                RadGridViewSpe.Rows.Clear()
                Dim dt As DataTable
                Dim theriaqueDao As New TheriaqueDao
                dt = theriaqueDao.getSpecialiteByATC(CodeAtc)

                Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
                Dim rowCount As Integer = dt.Rows.Count - 1

                For i = 0 To rowCount Step 1
                    iGrid += 1
                    RadGridViewSpe.Rows.Add(iGrid)

                    RadGridViewSpe.Rows(iGrid).Cells("SP_CODE_SQ_PK").Value = dt.Rows(i)("SP_CODE_SQ_PK")
                    RadGridViewSpe.Rows(iGrid).Cells("SP_CATC_CODE_FK").Value = dt.Rows(i)("SP_CATC_CODE_FK")
                    RadGridViewSpe.Rows(iGrid).Cells("SP_PR_CODE_FK").Value = dt.Rows(i)("SP_PR_CODE_FK")
                    RadGridViewSpe.Rows(iGrid).Cells("SP_NOM").Value = dt.Rows(i)("SP_NOM")
                    RadGridViewSpe.Rows(iGrid).Cells("SP_CIPUCD").Value = dt.Rows(i)("SP_CIPUCD")
                Next

                If RadGridViewSpe.Rows.Count > 0 Then
                    Me.RadGridViewSpe.CurrentRow = RadGridViewSpe.Rows(0)
                End If
            End If
        End If
    End Sub

End Class
