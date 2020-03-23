﻿Imports System.Collections.Specialized
Imports Telerik.WinControls.UI
Public Class RadF_CI_ATC_Selecteur

    Private _SelectedCodeATC As String

    Public Property SelectedCodeATC As String
        Get
            Return _SelectedCodeATC
        End Get
        Set(value As String)
            _SelectedCodeATC = value
        End Set
    End Property

    Dim theriaqueDao As New TheriaqueDao

    Private Sub RadFATCListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        afficheTitleForm(Me, "Déclaration contre-indication - Sélection classe thérapeutique")
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
            Dim NomSpecialite As String = RadTxtSpecialite.Text & "%"
            dt = GetSpecialiteByNomSpecialite(NomSpecialite, TheriaqueDao.EnumMonoVir.VIRTUEL)
            If dt.Rows.Count > 0 Then
                ChargementSpecialite(dt, True)
            Else
                '2 - Recherche spécialités classiques en nom complet
                NomSpecialite = RadTxtSpecialite.Text & " %"
                dt = GetSpecialiteByNomSpecialite(NomSpecialite, TheriaqueDao.EnumMonoVir.CLASSIQUE)
                If dt.Rows.Count > 0 Then
                    ChargementSpecialite(dt, True)
                Else
                    '3 - Recherche spécialités classiques en nom partiel
                    NomSpecialite = RadTxtSpecialite.Text & "%"
                    dt = GetSpecialiteByNomSpecialite(NomSpecialite, TheriaqueDao.EnumMonoVir.CLASSIQUE)
                    RadGridViewSpe.Rows.Clear()
                    RadGridViewSpe.FilterDescriptors.Clear()
                    If dt.Rows.Count > 0 Then
                        'Récupération des ATC des spécialités classiques en nom partiel
                        Dim ResultatOk As Boolean = False
                        Dim ListATC As New StringCollection
                        Dim rowCount As Integer = dt.Rows.Count - 1
                        For i = 0 To rowCount Step 1
                            If ListATC.Contains(dt.Rows(i)("SP_CATC_CODE_FK")) = False Then
                                ListATC.Add(dt.Rows(i)("SP_CATC_CODE_FK"))
                            End If
                        Next
                        Dim EnumeratorATC As StringEnumerator = ListATC.GetEnumerator()
                        Dim RowsCount As Integer = 0
                        Dim iGrid As Integer = -1
                        While EnumeratorATC.MoveNext()
                            Dim CodeATC As String = EnumeratorATC.Current.ToString
                            dt = theriaqueDao.getSpecialiteByArgument(CodeATC, TheriaqueDao.EnumGetSpecialite.CLASSE_ATC, TheriaqueDao.EnumMonoVir.VIRTUEL)
                            If dt.Rows.Count > 0 Then
                                ResultatOk = True
                                RowsCount += dt.Rows.Count
                            End If
                            ChargementSpecialite(dt, False, RowsCount, iGrid)
                            iGrid += dt.Rows.Count
                        End While

                        'Si pas de correspondance ATC entre Virtuel et classique en nom pariel, on affiche les classiques en nom partiel
                        If ResultatOk = False Then
                            NomSpecialite = RadTxtSpecialite.Text & "%"
                            dt = GetSpecialiteByNomSpecialite(NomSpecialite, TheriaqueDao.EnumMonoVir.CLASSIQUE)
                            ChargementSpecialite(dt, True)
                        End If
                    Else
                        LblOccurrencesLues.Text = "0 occurrence correspondant aux critères de recherche"
                    End If
                End If
            End If
            Cursor.Current = Cursors.Default
        Else
            MessageBox.Show("Vous devez saisir au moins 3 caractères pour lancer cette option de recherche des médicaments !")
        End If
    End Sub

    'Chargement du Grid affichant la classe thérapeutique de niveau 1
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

    'Chargement du Grid affichant la classe thérapeutique de niveau 2
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

    'Chargement du Grid affichant la classe thérapeutique de niveau 3
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

    'Chargement du Grid affichant la classe thérapeutique de niveau 4
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

    'Chargement du Grid affichant la classe thérapeutique de niveau 5
    Private Sub ChargementATC5(CodeATC As String)
        RadGridViewATC5.Rows.Clear()
        Dim dt As DataTable
        dt = theriaqueDao.getATCByATC(CodeATC)

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            RadGridViewATC5.Rows.Add(iGrid)

            RadGridViewATC5.Rows(iGrid).Cells("catc_code_pk").Value = dt.Rows(i)("catc_code_pk")
            RadGridViewATC5.Rows(iGrid).Cells("catc_nomf").Value = dt.Rows(i)("catc_nomf")
        Next

        If RadGridViewATC5.Rows.Count > 0 Then
            RadGridViewATC5.CurrentRow = RadGridViewATC5.ChildRows(0)
            RadGridViewATC5.TableElement.VScrollBar.Value = 0
        End If
    End Sub

    'Lancement de l'affichage du Grid affichant la classe thérapeutique de niveau 2 depuis le niveau 1
    Private Sub MasterTemplate_Click(sender As Object, e As EventArgs) Handles RadGridViewATC1.Click
        If RadGridViewATC1.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC1.Rows.IndexOf(Me.RadGridViewATC1.CurrentRow)
            Dim ATCCode As String = RadGridViewATC1.Rows(aRow).Cells("catc_code_pk").Value
            If aRow >= 0 Then
                RadGridViewATC3.Rows.Clear()
                RadGridViewATC4.Rows.Clear()
                RadGridViewATC5.Rows.Clear()
                ChargementATC2(ATCCode)
            End If
        End If
    End Sub

    'Lancement de l'affichage du Grid affichant la classe thérapeutique de niveau 3 depuis le niveau 2
    Private Sub RadGridViewATC2_Click(sender As Object, e As EventArgs) Handles RadGridViewATC2.Click
        If RadGridViewATC2.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC2.Rows.IndexOf(Me.RadGridViewATC2.CurrentRow)
            Dim ATCCode As String = RadGridViewATC2.Rows(aRow).Cells("catc_code_pk").Value
            If aRow >= 0 Then
                RadGridViewATC4.Rows.Clear()
                RadGridViewATC5.Rows.Clear()
                ChargementATC3(ATCCode)
            End If
        End If
    End Sub

    'Lancement de l'affichage du Grid affichant la classe thérapeutique de niveau 4 depuis le niveau 3
    Private Sub RadGridViewATC3_Click(sender As Object, e As EventArgs) Handles RadGridViewATC3.Click
        If RadGridViewATC3.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC3.Rows.IndexOf(Me.RadGridViewATC3.CurrentRow)
            Dim ATCCode As String = RadGridViewATC3.Rows(aRow).Cells("catc_code_pk").Value
            If aRow >= 0 Then
                RadGridViewATC5.Rows.Clear()
                ChargementATC4(ATCCode)
            End If
        End If
    End Sub

    'Lancement de l'affichage du Grid affichant la classe thérapeutique de niveau 5 depuis le niveau 4
    Private Sub RadGridViewATC4_Click(sender As Object, e As EventArgs) Handles RadGridViewATC4.Click
        If RadGridViewATC4.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC4.Rows.IndexOf(Me.RadGridViewATC4.CurrentRow)
            Dim ATCCode As String = RadGridViewATC4.Rows(aRow).Cells("catc_code_pk").Value
            If aRow >= 0 Then
                ChargementATC5(ATCCode)
            End If
        End If
    End Sub


    'Bouton d'appel de l'affichage de la Liste des médicaments depuis la classe thérapeutique de niveau 2
    Private Sub RadBtnSpec1_Click(sender As Object, e As EventArgs) Handles RadBtnSpec2.Click
        If RadGridViewATC2.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC2.Rows.IndexOf(Me.RadGridViewATC2.CurrentRow)
            If aRow >= 0 Then
                Dim CodeAtc As String = RadGridViewATC2.Rows(aRow).Cells("catc_code_pk").Value
                GetSpecialiteByATC(CodeAtc)
            End If
        End If
    End Sub

    'Bouton d'appel de l'affichage de la Liste des médicaments depuis la classe thérapeutique de niveau 3
    Private Sub RadBtnSpec2_Click(sender As Object, e As EventArgs) Handles RadBtnSpec3.Click
        If RadGridViewATC3.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC3.Rows.IndexOf(Me.RadGridViewATC3.CurrentRow)
            If aRow >= 0 Then
                Dim CodeAtc As String = RadGridViewATC3.Rows(aRow).Cells("catc_code_pk").Value
                GetSpecialiteByATC(CodeAtc)
            End If
        End If
    End Sub

    'Bouton d'appel de l'affichage de la Liste des médicaments depuis la classe thérapeutique de niveau 4
    Private Sub RadBtnSpec4_Click(sender As Object, e As EventArgs) Handles RadBtnSpec4.Click
        If RadGridViewATC4.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC4.Rows.IndexOf(Me.RadGridViewATC4.CurrentRow)
            If aRow >= 0 Then
                Dim CodeAtc As String = RadGridViewATC4.Rows(aRow).Cells("catc_code_pk").Value
                GetSpecialiteByATC(CodeAtc)
            End If
        End If
    End Sub

    'Bouton d'appel de l'affichage de la Liste des médicaments depuis la classe thérapeutique de niveau 5
    Private Sub RadBtnSpec5_Click(sender As Object, e As EventArgs) Handles RadBtnSpec5.Click
        If RadGridViewATC5.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC5.Rows.IndexOf(Me.RadGridViewATC5.CurrentRow)
            If aRow >= 0 Then
                Dim CodeAtc As String = RadGridViewATC5.Rows(aRow).Cells("catc_code_pk").Value
                GetSpecialiteByATC(CodeAtc)
            End If
        End If
    End Sub

    'Appel API Thériaque : recherche spécialité par classe thérapeutique
    Private Sub GetSpecialiteByATC(CodeAtc As String)
        Cursor.Current = Cursors.WaitCursor

        Dim Monovir As Integer = GetMonovir()
        CodeAtc &= "%"

        Dim dt As DataTable
        dt = theriaqueDao.getSpecialiteByArgument(CodeAtc, TheriaqueDao.EnumGetSpecialite.CLASSE_ATC, Monovir)
        ChargementSpecialite(dt, True)

        Cursor.Current = Cursors.Default
    End Sub

    'Appel API Thériaque : recherche spécialité par nom de spécialité
    Private Function GetSpecialiteByNomSpecialite(NomSpecialite As String, Monovir As Integer) As DataTable
        Dim dt As DataTable
        dt = theriaqueDao.getSpecialiteByArgument(NomSpecialite, TheriaqueDao.EnumGetSpecialite.NOM_SPECIALITE, Monovir)
        Return dt
    End Function

    'Chargement du Grid affichant les spécialités
    Private Sub ChargementSpecialite(dt As DataTable, ClearRows As Boolean, Optional NombreOccurrencesParametre As Integer = 0, Optional iGridParametre As Integer = 0)
        Dim iGrid As Integer

        Dim NombreOccurrencesLues As Integer
        If NombreOccurrencesParametre <> 0 Then
            NombreOccurrencesLues = NombreOccurrencesParametre
        Else
            NombreOccurrencesLues = dt.Rows.Count
        End If

        If NombreOccurrencesLues > 1 Then
            LblOccurrencesLues.Text = NombreOccurrencesLues & " occurrences correspondant aux critères de recherche"
        Else
            LblOccurrencesLues.Text = NombreOccurrencesLues & " occurrence correspondant aux critères de recherche"
        End If

        If ClearRows = True Then
            RadGridViewSpe.Rows.Clear()
            RadGridViewSpe.FilterDescriptors.Clear()
            iGrid = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Else
            iGrid = iGridParametre
        End If

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

    'Récupération de la valeur des boutons radio (virtuel / classique)
    Private Function GetMonovir() As Integer
        Dim Monovir As Integer

        If RadioBtnVirtuel.Checked = True Then
            Monovir = 1
        Else
            Monovir = 0
        End If

        Return Monovir
    End Function

    'Affichage popup détail grid spécialité
    Private Sub MasterTemplate_CellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs) Handles RadGridViewSpe.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    'Sortie
    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnPharmacocinetique_Click(sender As Object, e As EventArgs) Handles RadBtnPharmacocinetique.Click
        If RadGridViewSpe.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewSpe.Rows.IndexOf(Me.RadGridViewSpe.CurrentRow)
            If aRow >= 0 Then
                Dim SpecialiteId As Long = RadGridViewSpe.Rows(aRow).Cells("SP_CODE_SQ_PK").Value
                Dim PharmacoCinetique As String = theriaqueDao.GetPharmacoCinetqueBySpecialite(SpecialiteId)
                Me.Enabled = False
                Using form As New RadFAffichaeInfo
                    form.InfoToDisplay = PharmacoCinetique
                    form.Titre = "Information Pharmacocinétique"
                    form.ShowDialog()
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadBtnParmacodynamique_Click(sender As Object, e As EventArgs) Handles RadBtnParmacodynamique.Click
        If RadGridViewSpe.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewSpe.Rows.IndexOf(Me.RadGridViewSpe.CurrentRow)
            If aRow >= 0 Then
                Dim SpecialiteId As Long = RadGridViewSpe.Rows(aRow).Cells("SP_CODE_SQ_PK").Value
                Dim PharmacoCinetique As String = theriaqueDao.GetPharmacoDynamiqueBySpecialite(SpecialiteId)
                Me.Enabled = False
                Using form As New RadFAffichaeInfo
                    form.InfoToDisplay = PharmacoCinetique
                    form.Titre = "Information Pharmacodynamique"
                    form.ShowDialog()
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadBtnEffetIndesirable_Click(sender As Object, e As EventArgs) Handles RadBtnEffetIndesirable.Click
        If RadGridViewSpe.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewSpe.Rows.IndexOf(Me.RadGridViewSpe.CurrentRow)
            If aRow >= 0 Then
                Dim SpecialiteId As Long = RadGridViewSpe.Rows(aRow).Cells("SP_CODE_SQ_PK").Value
                Me.Enabled = False
                Using form As New RadFEffetSecondaire
                    form.MedicamentId = SpecialiteId
                    form.ShowDialog()
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Sélection ARC niveau 2
    Private Sub RadBtnSelectionATC2_Click(sender As Object, e As EventArgs) Handles RadBtnSelectionATC2.Click
        If RadGridViewATC2.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC2.Rows.IndexOf(Me.RadGridViewATC2.CurrentRow)
            If aRow >= 0 Then
                Dim codeATC As String = RadGridViewATC2.Rows(aRow).Cells("catc_code_pk").Value
                Selection(codeATC)
                Close()
            End If
        End If
    End Sub

    Private Sub RadBtnSelectionATC3_Click(sender As Object, e As EventArgs) Handles RadBtnSelectionATC3.Click
        If RadGridViewATC3.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC3.Rows.IndexOf(Me.RadGridViewATC3.CurrentRow)
            If aRow >= 0 Then
                Dim codeATC As String = RadGridViewATC3.Rows(aRow).Cells("catc_code_pk").Value
                Selection(codeATC)
                Close()
            End If
        End If
    End Sub

    Private Sub RadBtnSelectionATC4_Click(sender As Object, e As EventArgs) Handles RadBtnSelectionATC4.Click
        If RadGridViewATC4.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC4.Rows.IndexOf(Me.RadGridViewATC4.CurrentRow)
            If aRow >= 0 Then
                Dim codeATC As String = RadGridViewATC4.Rows(aRow).Cells("catc_code_pk").Value
                Selection(codeATC)
                Close()
            End If
        End If
    End Sub

    Private Sub RadBtnSelectionATC5_Click(sender As Object, e As EventArgs) Handles RadBtnSelectionATC5.Click
        If RadGridViewATC5.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC5.Rows.IndexOf(Me.RadGridViewATC5.CurrentRow)
            If aRow >= 0 Then
                Dim codeATC As String = RadGridViewATC5.Rows(aRow).Cells("catc_code_pk").Value
                Selection(codeATC)
                Close()
            End If
        End If
    End Sub

    Private Sub Selection(CodeATC As String)
        SelectedCodeATC = CodeATC
        Close()
    End Sub

End Class
