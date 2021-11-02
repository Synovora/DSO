Imports System.Collections.Specialized
Imports Telerik.WinControls.UI
Imports Oasis_Common

Public Class RadFVaccin

    Property SelectedClasseAtc As String
    Dim PremierPassage As Boolean

    ReadOnly theriaqueDao As New TheriaqueDao
    ReadOnly valenceDao As New ValenceDao
    ReadOnly vaccinDao As New VaccinDao
    Dim valences As List(Of Valence)
    Dim relations As List(Of RelationVaccinValence) = New List(Of RelationVaccinValence)

    Private Sub RadFATCListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Thériaque - Vaccin", userLog)
        ChargementATC3("J07")
        ChargementValence()

        LblOccurrencesLues.Text = ""
    End Sub

    Private Sub ChargementValence()
        GridValence.Rows.Clear()
        Dim iGrid As Integer = 0
        Dim vaccinRow As Integer = Me.RadGridViewSpe.Rows.IndexOf(Me.RadGridViewSpe.CurrentRow)
        If vaccinRow >= 0 Then
            relations = valenceDao.GetRelationListByVaccin(RadGridViewSpe.Rows(vaccinRow).Cells("SP_CODE_SQ_PK").Value)
        End If

        valences = valenceDao.GetList()
        For Each valence As Valence In valences
            GridValence.Rows.Add(iGrid)
            GridValence.Rows(iGrid).Cells("id").Value = valence.Id
            GridValence.Rows(iGrid).Cells("code").Value = valence.Code
            GridValence.Rows(iGrid).Cells("description").Value = valence.Description
            GridValence.Rows(iGrid).Cells("precaution").Value = valence.Precaution
            GridValence.Rows(iGrid).Cells("select").Value = relations.Any(Function(myObject) myObject.Valence = valence.Id)
            iGrid += 1
        Next
        GridValence.Enabled = True
    End Sub

    'Liste des médicaments par classe ATC
    Private Sub ChargementMedicamentByAtc()
        Dim MonoVir As Integer = TheriaqueDao.EnumMonoVir.CLASSIQUE

        Dim dt As DataTable

        Cursor.Current = Cursors.WaitCursor
        dt = theriaqueDao.GetSpecialiteByArgument(SelectedClasseAtc, TheriaqueDao.EnumGetSpecialite.CLASSE_ATC, MonoVir)
        If dt.Rows.Count > 0 Then
            ChargementSpecialite(dt, True)
        End If
        Cursor.Current = Cursors.Default
    End Sub


    'Liste des médicaments par dénomination
    Private Sub RadBtnFiltre_Click(sender As Object, e As EventArgs) Handles RadBtnFiltreSpecialite.Click
        Dim NbCar As Integer = RadTxtSpecialite.Text.Length
        If RadTxtSpecialite.Text <> "" And NbCar > 3 Then
            Cursor.Current = Cursors.WaitCursor
            '1 - Recherche spécialités virtuelles en nom partiel
            Dim dt As DataTable
            Dim NomSpecialite As String = RadTxtSpecialite.Text & "%"
            dt = GetSpecialiteByNomSpecialite(NomSpecialite, TheriaqueDao.EnumMonoVir.VIRTUEL)
            If dt.Rows.Count > 0 Then
                ChargementSpecialite(dt, True)
            Else
                '2 - Recherche spécialités classiques en nom complet -> un espace est positionné entre la chaîne de caractère saisie et le caractère joker, par exemple 'ASPIRIN %'
                NomSpecialite = RadTxtSpecialite.Text & " %"
                dt = GetSpecialiteByNomSpecialite(NomSpecialite, TheriaqueDao.EnumMonoVir.CLASSIQUE)
                If dt.Rows.Count > 0 Then
                    ChargementSpecialite(dt, True)
                Else
                    '3 - Si la recherche n'a pas aboutie, on recherche les spécialités classiques en nom partiel, par exemple 'ASPIRIN%'
                    NomSpecialite = RadTxtSpecialite.Text & "%"
                    dt = GetSpecialiteByNomSpecialite(NomSpecialite, TheriaqueDao.EnumMonoVir.CLASSIQUE)
                    RadGridViewSpe.Rows.Clear()
                    RadGridViewSpe.FilterDescriptors.Clear()
                    If dt.Rows.Count > 0 Then
                        'Puis on récupére les ATC des spécialités classiques obtenues en nom partiel
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
                            ' Recherche des spécialités virtuelles correspondant aux ATC des spécialités classiques obtenues précédemment
                            dt = theriaqueDao.GetSpecialiteByArgument(CodeATC, TheriaqueDao.EnumGetSpecialite.CLASSE_ATC, TheriaqueDao.EnumMonoVir.VIRTUEL)
                            If dt.Rows.Count > 0 Then
                                ResultatOk = True
                                RowsCount += dt.Rows.Count
                            End If
                            ChargementSpecialite(dt, False, RowsCount, iGrid)
                            iGrid += dt.Rows.Count
                        End While

                        'Si aucune spécialité Virtuelle a été retrouvée à partir des ATC obtenues des spécialités classiques en nom pariel, on affiche les spécialités classiques en nom partiel
                        If ResultatOk = False Then
                            NomSpecialite = RadTxtSpecialite.Text & "%"
                            dt = GetSpecialiteByNomSpecialite(NomSpecialite, TheriaqueDao.EnumMonoVir.CLASSIQUE)
                            ChargementSpecialite(dt, True)
                        End If
                    Else
                        LblOccurrencesLues.Text = "Aucune occurrence ne correspond aux critères de recherche"
                    End If
                End If
            End If
            Cursor.Current = Cursors.Default
        Else
            MessageBox.Show("Vous devez saisir au moins 4 caractères pour lancer cette option de recherche des médicaments !")
        End If
    End Sub

    Private Sub ChargementATC3(CodeATC As String)
        RadGridViewATC3.Rows.Clear()
        Dim dt As DataTable = theriaqueDao.GetATCListeByATCPere(CodeATC)

        Dim iGrid As Integer = 0
        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            RadGridViewATC3.Rows.Add(iGrid)
            RadGridViewATC3.Rows(iGrid).Cells("catc_code_pk").Value = dt.Rows(i)("catc_code_pk")
            RadGridViewATC3.Rows(iGrid).Cells("catc_nomf").Value = dt.Rows(i)("catc_nomf")
            iGrid += 1
        Next
    End Sub

    Private Sub ChargementATC4(CodeATC As String)
        RadGridViewATC4.Rows.Clear()
        Dim dt As DataTable
        dt = theriaqueDao.GetATCListeByATCPere(CodeATC)

        Dim iGrid As Integer = 0
        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            RadGridViewATC4.Rows.Add(iGrid)
            RadGridViewATC4.Rows(iGrid).Cells("catc_code_pk").Value = dt.Rows(i)("catc_code_pk")
            RadGridViewATC4.Rows(iGrid).Cells("catc_nomf").Value = dt.Rows(i)("catc_nomf")
            iGrid += 1
        Next
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

    Private Sub RadBtnSpec2_Click(sender As Object, e As EventArgs) Handles RadBtnSpec3.Click
        If RadGridViewATC3.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC3.Rows.IndexOf(Me.RadGridViewATC3.CurrentRow)
            If aRow >= 0 Then
                Dim CodeAtc As String = RadGridViewATC3.Rows(aRow).Cells("catc_code_pk").Value
                GetSpecialiteByATC(CodeAtc)
                RadTxtSpecialite.Text = ""
            End If
        End If
    End Sub

    Private Sub RadBtnSpec4_Click(sender As Object, e As EventArgs) Handles RadBtnSpec4.Click
        If RadGridViewATC4.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC4.Rows.IndexOf(Me.RadGridViewATC4.CurrentRow)
            If aRow >= 0 Then
                Dim CodeAtc As String = RadGridViewATC4.Rows(aRow).Cells("catc_code_pk").Value
                GetSpecialiteByATC(CodeAtc)
                RadTxtSpecialite.Text = ""
            End If
        End If
    End Sub

    'Appel API Thériaque : recherche spécialité par classe thérapeutique
    Private Sub GetSpecialiteByATC(CodeAtc As String)
        Cursor.Current = Cursors.WaitCursor

        Dim Monovir As Integer = TheriaqueDao.EnumMonoVir.CLASSIQUE
        CodeAtc &= "%"

        Dim dt As DataTable
        dt = theriaqueDao.GetSpecialiteByArgument(CodeAtc, TheriaqueDao.EnumGetSpecialite.CLASSE_ATC, Monovir)
        ChargementSpecialite(dt, True)

        Cursor.Current = Cursors.Default
    End Sub

    'Appel API Thériaque : recherche spécialité par nom de spécialité
    Private Function GetSpecialiteByNomSpecialite(NomSpecialite As String, Monovir As Integer) As DataTable
        Dim dt As DataTable
        dt = theriaqueDao.GetSpecialiteByArgument(NomSpecialite, TheriaqueDao.EnumGetSpecialite.NOM_SPECIALITE, Monovir)
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

        Dim relations = valenceDao.GetRelationList()


        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            RadGridViewSpe.Rows.Add(iGrid)

            RadGridViewSpe.Rows(iGrid).Cells("SP_NOM").Style.ForeColor = If(relations.Any(Function(myObject) myObject.Vaccin = dt.Rows(i)("SP_CODE_SQ_PK")), Color.Black, Color.Red)
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
            Dim CodeATC As String = RadGridViewSpe.Rows(0).Cells("SP_CATC_CODE_FK").Value
            'AfficheATC(CodeATC)
        End If
    End Sub

    Private Sub MasterTemplate_CellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs)
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    'Sortie
    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnPharmacocinetique_Click(sender As Object, e As EventArgs)
        If RadGridViewSpe.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewSpe.Rows.IndexOf(Me.RadGridViewSpe.CurrentRow)
            If aRow >= 0 Then
                Dim SpecialiteId As Long = RadGridViewSpe.Rows(aRow).Cells("SP_CODE_SQ_PK").Value
                Dim PharmacoCinetique As String = theriaqueDao.GetPharmacoCinetiqueBySpecialite(SpecialiteId)
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

    Private Sub RadBtnParmacodynamique_Click(sender As Object, e As EventArgs)
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

    Private Sub RadBtnEffetIndesirable_Click(sender As Object, e As EventArgs)
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

    Private Sub RadBtnSubstance_Click(sender As Object, e As EventArgs)
        If RadGridViewSpe.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewSpe.Rows.IndexOf(Me.RadGridViewSpe.CurrentRow)
            If aRow >= 0 Then
                Dim SpecialiteId As Long = RadGridViewSpe.Rows(aRow).Cells("SP_CODE_SQ_PK").Value
                Me.Enabled = False
                Using form As New RadFSubstancesListe
                    form.SelectedSpecialite = SpecialiteId
                    form.ShowDialog()
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadGridViewSpe_DoubleClick_1(sender As Object, e As EventArgs) Handles RadGridViewSpe.Click
        RadGridViewSubstance.Rows.Clear()
        GridValence.Rows.Clear()
        RadGridViewSubstance.Enabled = False
        GridValence.Enabled = False
        ImportVaccin()
        LoadSubstance()
        ChargementValence()
    End Sub

    Private Sub ImportVaccin()
        Dim aRow As Integer = Me.RadGridViewSpe.Rows.IndexOf(Me.RadGridViewSpe.CurrentRow)
        If aRow >= 0 Then
            Dim vaccin = vaccinDao.GetByCode(RadGridViewSpe.Rows(aRow).Cells("SP_CODE_SQ_PK").Value)
            If vaccin Is Nothing Then
                vaccinDao.Create(New Vaccin() With {
                              .Code = RadGridViewSpe.Rows(aRow).Cells("SP_CODE_SQ_PK").Value,
                              .CodeAtc = RadGridViewSpe.Rows(aRow).Cells("SP_CATC_CODE_FK").Value,
                              .Dci = RadGridViewSpe.Rows(aRow).Cells("SP_NOM").Value,
                              .DciLongue = RadGridViewSpe.Rows(aRow).Cells("SP_NOMLONG").Value,
                              .UtilisateurImport = userLog.UtilisateurId
                })
                'RadGridViewSpe.Rows(iGrid).Cells("SP_CODE_SQ_PK").Value = dt.Rows(i)("SP_CODE_SQ_PK")
                'RadGridViewSpe.Rows(iGrid).Cells("SP_CATC_CODE_FK").Value = dt.Rows(i)("SP_CATC_CODE_FK")
                'RadGridViewSpe.Rows(iGrid).Cells("SP_PR_CODE_FK").Value = dt.Rows(i)("SP_PR_CODE_FK")
                'RadGridViewSpe.Rows(iGrid).Cells("SP_NOM").Value = dt.Rows(i)("SP_NOM")
                'RadGridViewSpe.Rows(iGrid).Cells("SP_NOMCOMP").Value = dt.Rows(i)("SP_NOMCOMP")
                'RadGridViewSpe.Rows(iGrid).Cells("SP_NOMLONG").Value = dt.Rows(i)("SP_NOMLONG")
                'RadGridViewSpe.Rows(iGrid).Cells("SP_CIPUCD").Value = dt.Rows(i)("SP_CIPUCD")
            End If
        End If
    End Sub

    Private Sub LoadSubstance()
        Dim aRow As Integer = Me.RadGridViewSpe.Rows.IndexOf(Me.RadGridViewSpe.CurrentRow)
        If aRow >= 0 Then
            Dim SpecialiteId As Long = RadGridViewSpe.Rows(aRow).Cells("SP_CODE_SQ_PK").Value
            Dim dt As DataTable = theriaqueDao.GetSpecialiteByArgument(SpecialiteId, TheriaqueDao.EnumGetSpecialite.ID_THERIAQUE, TheriaqueDao.EnumMonoVir.NULL)

            Dim SubstanceListe As List(Of Integer)
            Dim iGrid As Integer = -1
            SubstanceListe = theriaqueDao.GetSubstanceCodeListBySpecialite(SpecialiteId)
            Dim EnumeratorSubstanceListe As IEnumerator = SubstanceListe.GetEnumerator()
            While EnumeratorSubstanceListe.MoveNext()
                Dim CodeSubstance As Integer = EnumeratorSubstanceListe.Current
                Dim SubstanceDenomination As String = theriaqueDao.GetSubstanceDenominationById(CodeSubstance)
                iGrid += 1
                RadGridViewSubstance.Rows.Add(iGrid)
                RadGridViewSubstance.Rows(iGrid).Cells("SAC_NOM").Value = SubstanceDenomination
            End While
        End If
        RadGridViewSubstance.Enabled = True
    End Sub

    'Affichage Pharmacocinétique
    Private Sub RadBtnPharmacocinetique_Click_1(sender As Object, e As EventArgs)
        If RadGridViewSpe.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewSpe.Rows.IndexOf(Me.RadGridViewSpe.CurrentRow)
            If aRow >= 0 Then
                Dim SpecialiteId As Long = RadGridViewSpe.Rows(aRow).Cells("SP_CODE_SQ_PK").Value
                Dim PharmacoCinetique As String = theriaqueDao.GetPharmacoCinetiqueBySpecialite(SpecialiteId)
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

    'Affichage Pharmacodynamie
    Private Sub RadBtnParmacodynamique_Click_1(sender As Object, e As EventArgs)
        If RadGridViewSpe.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewSpe.Rows.IndexOf(Me.RadGridViewSpe.CurrentRow)
            If aRow >= 0 Then
                Dim SpecialiteId As Long = RadGridViewSpe.Rows(aRow).Cells("SP_CODE_SQ_PK").Value
                Dim PharmacoDynamique As String = theriaqueDao.GetPharmacoDynamiqueBySpecialite(SpecialiteId)
                Me.Enabled = False
                Using form As New RadFAffichaeInfo
                    form.InfoToDisplay = PharmacoDynamique
                    form.Titre = "Information Pharmacodynamique"
                    form.ShowDialog()
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    'Affichage effets indésirables
    Private Sub RadBtnEffetIndesirable_Click_1(sender As Object, e As EventArgs)
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

    'Affichage substances actives
    Private Sub RadBtnSubstance_Click_1(sender As Object, e As EventArgs)
        If RadGridViewSpe.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewSpe.Rows.IndexOf(Me.RadGridViewSpe.CurrentRow)
            If aRow >= 0 Then
                Dim SpecialiteId As Long = RadGridViewSpe.Rows(aRow).Cells("SP_CODE_SQ_PK").Value
                Me.Enabled = False
                Using form As New RadFSubstancesListe
                    form.SelectedSpecialite = SpecialiteId
                    form.ShowDialog()
                End Using
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub RadioBtnClassique_CheckedChanged(sender As Object, e As EventArgs)
        If SelectedClasseAtc <> "" Then
            If PremierPassage = False Then
                PremierPassage = True
            Else
                ChargementMedicamentByAtc()
            End If
        End If
    End Sub

    Private Sub RadioBtnVirtuel_CheckedChanged(sender As Object, e As EventArgs)
        If SelectedClasseAtc <> "" Then
            If PremierPassage = False Then
                PremierPassage = True
            Else
                ChargementMedicamentByAtc()
            End If
        End If
    End Sub

    Private Sub RadButtonAddValence_Click(sender As Object, e As EventArgs) Handles RadButtonAddValence.Click
        Using formSelecteur As New RadFValenceCreation
            formSelecteur.ShowDialog()
            ChargementValence()
        End Using
    End Sub

    Private Sub RadButtonEditValence_Click(sender As Object, e As EventArgs) Handles RadButtonEditValence.Click
        Dim aRow As Integer = Me.GridValence.Rows.IndexOf(Me.GridValence.CurrentRow)
        If aRow >= 0 Then
            Using formSelecteur As New RadFValenceCreation
                formSelecteur.Valence = valences(aRow)
                formSelecteur.ShowDialog()
                ChargementValence()
            End Using
        End If
    End Sub

    Private Sub RadButtonRemoveValence_Click(sender As Object, e As EventArgs) Handles RadButtonRemoveValence.Click
        Dim aRow As Integer = Me.GridValence.Rows.IndexOf(Me.GridValence.CurrentRow)
        If aRow >= 0 Then
            Dim valenceId As String = GridValence.Rows(aRow).Cells("id").Value
            valenceDao.Delete(New Valence() With {.Id = valenceId})
            ChargementValence()
        End If
    End Sub

    Private Sub GridValence_Click(sender As Object, ByVal e As GridViewCellEventArgs) Handles GridValence.CellClick
        If e.RowIndex >= 0 AndAlso e.ColumnIndex = 0 Then
            Dim vaccinRow As Integer = Me.RadGridViewSpe.Rows.IndexOf(Me.RadGridViewSpe.CurrentRow)
            Dim valenceRow As Integer = e.RowIndex 'Me.GridValence.Rows.IndexOf(Me.GridValence.CurrentRow)

            If vaccinRow >= 0 AndAlso valenceRow >= 0 Then
                Dim vaccinId As String = RadGridViewSpe.Rows(vaccinRow).Cells("SP_CODE_SQ_PK").Value
                Dim valenceId As String = GridValence.Rows(valenceRow).Cells("id").Value
                Dim checked As Boolean = GridValence.Rows(valenceRow).Cells("select").Value

                If (checked) Then
                    valenceDao.DeleteRelation(New RelationVaccinValence() With {.Valence = valenceId, .Vaccin = vaccinId})
                Else
                    valenceDao.CreateRelation(New RelationVaccinValence() With {.Vaccin = vaccinId, .Valence = valenceId})
                End If
                ChargementValence()
            End If
        End If
    End Sub

    Private Sub GridValence_ToolTipTextNeeded(ByVal sender As Object, ByVal e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles GridValence.ToolTipTextNeeded, RadGridViewSpe.ToolTipTextNeeded, RadGridViewATC3.ToolTipTextNeeded, RadGridViewATC4.ToolTipTextNeeded, RadGridViewSubstance.ToolTipTextNeeded
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
End Class
