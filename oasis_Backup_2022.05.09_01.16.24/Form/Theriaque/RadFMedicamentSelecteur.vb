Imports System.Collections.Specialized
Imports Telerik.WinControls.UI
Imports Oasis_Common

Public Class RadFMedicamentSelecteur
    Private _SelectedSpecialiteId As Long
    Private _SelectedPatient As Patient
    Private _selectedClasseAtc As String

    Public Property SelectedSpecialiteId As Long
        Get
            Return _SelectedSpecialiteId
        End Get
        Set(value As Long)
            _SelectedSpecialiteId = value
        End Set
    End Property

    Public Property SelectedPatient As Patient
        Get
            Return _SelectedPatient
        End Get
        Set(value As Patient)
            _SelectedPatient = value
        End Set
    End Property

    Public Property SelectedClasseAtc As String
        Get
            Return _selectedClasseAtc
        End Get
        Set(value As String)
            _selectedClasseAtc = value
        End Set
    End Property

    Dim theriaqueDao As New TheriaqueDao

    Dim RowCountATC1 As Integer
    Dim PremierPassage As Boolean = False

    Private Sub RadFATCListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Thériaque - Recherche médicament", userLog)
        RadioBtnVirtuel.Checked = True
        ChargementEtatCivil()
        ChargementATC1()

        If SelectedClasseAtc <> "" Then
            '-- Si le sélecteur ATC est alimentée, cela signifie que l'on veut modifier la spécialité d'un traitement existant 
            '-- ceci induit de n'afficher que les spécialités de la même classe ATC
            RadTxtSpecialite.Hide()
            RadBtnFiltreSpecialite.Hide()
            RadBtnSpec2.Hide()
            RadBtnSpec3.Hide()
            RadBtnSpec4.Hide()
            RadGridViewATC1.Enabled = False
            RadGridViewATC2.Enabled = False
            RadGridViewATC3.Enabled = False
            RadGridViewATC4.Enabled = False
            ChargementMedicamentByAtc()
        End If

        LblOccurrencesLues.Text = ""
    End Sub

    Private Sub ChargementEtatCivil()
        If SelectedPatient Is Nothing Then
            Exit Sub
        End If

        LblPatientNIR.Text = SelectedPatient.PatientNir
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientAdresse1.Text = SelectedPatient.PatientAdresse1
        LblPatientAdresse2.Text = SelectedPatient.PatientAdresse2
        LblPatientCodePostal.Text = SelectedPatient.PatientCodePostal
        LblPatientVille.Text = SelectedPatient.PatientVille
        LblPatientTel1.Text = SelectedPatient.PatientTel1
        LblPatientTel2.Text = SelectedPatient.PatientTel2
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)
        LblPatientDateMaj.Text = SelectedPatient.PatientSyntheseDateMaj.ToString("dd/MM/yyyy")

        'Vérification de l'existence d'ALD
        Dim aldDao As New AldDao
        Dim StringALDTooltip As String = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringALDTooltip = "" Then
            LblALD.Hide()
        Else
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringALDTooltip)
        End If

        'Contre-indication
        GetContreIndication()

        'Allergie
        GetAllergie()
    End Sub

    Private Sub GetContreIndication()
        Dim patientDao As New PatientDao
        Dim StringContreIndicationToolTip As String = patientDao.GetStringContreIndicationByPatient(SelectedPatient.patientId)
        If StringContreIndicationToolTip = "" Then
            lblContreIndication.Hide()
        Else
            lblContreIndication.Show()
            ToolTip.SetToolTip(lblContreIndication, StringContreIndicationToolTip)
        End If
    End Sub


    Private Sub GetAllergie()
        Dim patientDao As New PatientDao
        Dim StringAllergieToolTip As String = patientDao.GetStringAllergieByPatient(SelectedPatient.patientId)
        If StringAllergieToolTip = "" Then
            LblAllergie.Hide()
        Else
            LblAllergie.Show()
            ToolTip.SetToolTip(LblAllergie, StringAllergieToolTip)
        End If
    End Sub

    'Liste des médicaments par classe ATC
    Private Sub ChargementMedicamentByAtc()
        Dim MonoVir As Integer = TheriaqueDao.EnumMonoVir.VIRTUEL
        If RadioBtnClassique.Checked = True Then
            MonoVir = TheriaqueDao.EnumMonoVir.CLASSIQUE
        End If

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

    'Chargement du Grid affichant la classe thérapeutique de niveau 1
    Private Sub ChargementATC1(Optional codeATCFocus As String = "")
        If codeATCFocus = "" Then
            RadGridViewATC1.Rows.Clear()
            Dim dt As DataTable
            dt = theriaqueDao.GetAllATC()

            Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
            Dim rowCount As Integer = dt.Rows.Count - 1
            RowCountATC1 = dt.Rows.Count - 1

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
        Else
            For i = 0 To RowCountATC1 Step 1
                If codeATCFocus = RadGridViewATC1.Rows(i).Cells("catc_code_pk").Value Then
                    RadGridViewATC1.CurrentRow = RadGridViewATC1.ChildRows(i)
                    RadGridViewATC1.TableElement.VScrollBar.Value = 0
                End If
            Next
        End If
    End Sub

    'Chargement du Grid affichant la classe thérapeutique de niveau 2
    Private Sub ChargementATC2(CodeATC As String, Optional codeATCFocus As String = "")
        RadGridViewATC2.Rows.Clear()
        Dim dt As DataTable
        dt = theriaqueDao.getATCListeByATCPere(CodeATC)

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            RadGridViewATC2.Rows.Add(iGrid)

            RadGridViewATC2.Rows(iGrid).Cells("catc_code_pk").Value = dt.Rows(i)("catc_code_pk")
            RadGridViewATC2.Rows(iGrid).Cells("catc_nomf").Value = dt.Rows(i)("catc_nomf")
        Next

        If codeATCFocus = "" Then
            If RadGridViewATC2.Rows.Count > 0 Then
                RadGridViewATC2.CurrentRow = RadGridViewATC2.ChildRows(0)
                RadGridViewATC2.TableElement.VScrollBar.Value = 0
            End If
        Else
            For i = 0 To rowCount Step 1
                If codeATCFocus = RadGridViewATC2.Rows(i).Cells("catc_code_pk").Value Then
                    RadGridViewATC2.CurrentRow = RadGridViewATC2.ChildRows(i)
                    RadGridViewATC2.TableElement.VScrollBar.Value = 0
                End If
            Next
        End If
    End Sub

    'Chargement du Grid affichant la classe thérapeutique de niveau 3
    Private Sub ChargementATC3(CodeATC As String, Optional codeATCFocus As String = "")
        RadGridViewATC3.Rows.Clear()
        Dim dt As DataTable
        dt = theriaqueDao.getATCListeByATCPere(CodeATC)

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            RadGridViewATC3.Rows.Add(iGrid)

            RadGridViewATC3.Rows(iGrid).Cells("catc_code_pk").Value = dt.Rows(i)("catc_code_pk")
            RadGridViewATC3.Rows(iGrid).Cells("catc_nomf").Value = dt.Rows(i)("catc_nomf")
        Next

        If codeATCFocus = "" Then
            If RadGridViewATC3.Rows.Count > 0 Then
                RadGridViewATC3.CurrentRow = RadGridViewATC3.ChildRows(0)
                RadGridViewATC3.TableElement.VScrollBar.Value = 0
            End If
        Else
            For i = 0 To rowCount Step 1
                If codeATCFocus = RadGridViewATC3.Rows(i).Cells("catc_code_pk").Value Then
                    RadGridViewATC3.CurrentRow = RadGridViewATC3.ChildRows(i)
                    RadGridViewATC3.TableElement.VScrollBar.Value = 0
                End If
            Next
        End If
    End Sub

    'Chargement du Grid affichant la classe thérapeutique de niveau 4
    Private Sub ChargementATC4(CodeATC As String, Optional codeATCFocus As String = "")
        RadGridViewATC4.Rows.Clear()
        Dim dt As DataTable
        dt = theriaqueDao.getATCListeByATCPere(CodeATC)

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            RadGridViewATC4.Rows.Add(iGrid)

            RadGridViewATC4.Rows(iGrid).Cells("catc_code_pk").Value = dt.Rows(i)("catc_code_pk")
            RadGridViewATC4.Rows(iGrid).Cells("catc_nomf").Value = dt.Rows(i)("catc_nomf")
        Next

        If codeATCFocus = "" Then
            If RadGridViewATC4.Rows.Count > 0 Then
                RadGridViewATC4.CurrentRow = RadGridViewATC4.ChildRows(0)
                RadGridViewATC4.TableElement.VScrollBar.Value = 0
            End If
        Else
            For i = 0 To rowCount Step 1
                If codeATCFocus = RadGridViewATC4.Rows(i).Cells("catc_code_pk").Value Then
                    RadGridViewATC4.CurrentRow = RadGridViewATC4.ChildRows(i)
                    RadGridViewATC4.TableElement.VScrollBar.Value = 0
                End If
            Next
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
                ChargementATC4(ATCCode)
            End If
        End If
    End Sub

    'Bouton d'appel de l'affichage de la Liste des médicaments depuis la classe thérapeutique
    Private Sub RadBtnSpec1_Click(sender As Object, e As EventArgs) Handles RadBtnSpec2.Click
        If RadGridViewATC2.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC2.Rows.IndexOf(Me.RadGridViewATC2.CurrentRow)
            If aRow >= 0 Then
                Dim CodeAtc As String = RadGridViewATC2.Rows(aRow).Cells("catc_code_pk").Value
                GetSpecialiteByATC(CodeAtc)
                RadTxtSpecialite.Text = ""
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
            Dim CodeATC As String = RadGridViewSpe.Rows(0).Cells("SP_CATC_CODE_FK").Value
            AfficheATC(CodeATC)
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

    'Focus sur une spécialité, affichage de l'ATC correspondante
    Private Sub RadGridViewSpe_Click_1(sender As Object, e As EventArgs) Handles RadGridViewSpe.Click
        If RadGridViewSpe.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewSpe.Rows.IndexOf(Me.RadGridViewSpe.CurrentRow)
            If aRow >= 0 Then
                Dim CodeATC As String = RadGridViewSpe.Rows(aRow).Cells("SP_CATC_CODE_FK").Value
                AfficheATC(CodeATC)
            End If
        End If
    End Sub

    Private Sub AfficheATC(CodeATC As String)
        Cursor.Current = Cursors.WaitCursor
        Dim codeATC1, codeATC2, codeATC3, codeATC4 As String
        codeATC1 = CodeATC.Substring(0, 1)
        ChargementATC1(codeATC1)
        If CodeATC.Length >= 3 Then
            codeATC2 = CodeATC.Substring(0, 3)
            ChargementATC2(codeATC1, codeATC2)
            If CodeATC.Length >= 4 Then
                codeATC3 = CodeATC.Substring(0, 4)
                ChargementATC3(codeATC2, codeATC3)
                If CodeATC.Length >= 5 Then
                    codeATC4 = CodeATC.Substring(0, 5)
                    ChargementATC4(codeATC3, codeATC4)
                Else
                    RadGridViewATC4.Rows.Clear()
                End If
            Else
                RadGridViewATC3.Rows.Clear()
                RadGridViewATC4.Rows.Clear()
            End If
        Else
            RadGridViewATC2.Rows.Clear()
            RadGridViewATC3.Rows.Clear()
            RadGridViewATC4.Rows.Clear()
        End If
        Cursor.Current = Cursors.Default
    End Sub

    'Affichage popup détail grid spécialité
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

    Private Sub lblContreIndication_Click(sender As Object, e As EventArgs) Handles lblContreIndication.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPatientContreIndicationListe As New RadFPatientContreIndicationListe
            vFPatientContreIndicationListe.SelectedPatient = Me.SelectedPatient
            vFPatientContreIndicationListe.ShowDialog()
        End Using
        GetContreIndication()
        Me.Enabled = True
    End Sub

    Private Sub LblAllergie_Click(sender As Object, e As EventArgs) Handles LblAllergie.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using Form As New RadFPatientAllergieListe
            Form.SelectedPatient = Me.SelectedPatient
            Form.ShowDialog()
        End Using
        GetAllergie()
        Me.Enabled = True
    End Sub

    'Sélection d'une spécialité, renvoi de la valeur de la clé Thériaque
    Private Sub RadBtnSelection_Click_1(sender As Object, e As EventArgs) Handles RadBtnSelection.Click
        SelectionSpecialite()
    End Sub

    Private Sub RadGridViewSpe_DoubleClick_1(sender As Object, e As EventArgs) Handles RadGridViewSpe.DoubleClick
        SelectionSpecialite()
    End Sub

    Private Sub SelectionSpecialite()
        If RadGridViewSpe.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewSpe.Rows.IndexOf(Me.RadGridViewSpe.CurrentRow)
            If aRow >= 0 Then
                Cursor.Current = Cursors.WaitCursor
                Dim SpecialiteId As Long = RadGridViewSpe.Rows(aRow).Cells("SP_CODE_SQ_PK").Value
                Dim SpecialiteATCId As String = RadGridViewSpe.Rows(aRow).Cells("SP_CATC_CODE_FK").Value
                Dim messageAlerte As String


                '===============================================================================================
                'Contrôle allergie
                '===============================================================================================
                Dim specialiteAllergie As SpecialiteAllergique = theriaqueDao.IsSpecialiteAllergique(SelectedPatient, SpecialiteId)
                If specialiteAllergie.Allergie = True Then
                    messageAlerte = specialiteAllergie.MessageAllergie
                    messageAlerte += vbCrLf & vbCrLf & "Ce médicament ne peut pas être prescrit pour ce patient !"
                    MessageBox.Show(messageAlerte, "Alerte", MessageBoxButtons.OK, MessageBoxIcon.Error)
                    Exit Sub
                End If

                '===============================================================================================
                'Contrôle contre-indication (ATC et Substance)
                '===============================================================================================

                Dim specialiteContreIndique As SpecialiteContreIndique = theriaqueDao.IsSpecialiteContreIndique(SelectedPatient, SpecialiteId)
                If specialiteContreIndique.ContreIndication = True Then
                    messageAlerte = specialiteContreIndique.MessageContreIndication
                    messageAlerte += vbCrLf & vbCrLf & "Confirmez-vous la sélection du médicament ?"
                    If MsgBox(messageAlerte, MsgBoxStyle.YesNo Or MsgBoxStyle.Exclamation, "Avertissement") <> MsgBoxResult.Yes Then
                        Exit Sub
                    End If
                End If

                Cursor.Current = Cursors.WaitCursor
                SelectedSpecialiteId = SpecialiteId
                Close()
            End If
        End If
    End Sub


    'Affichage Pharmacocinétique
    Private Sub RadBtnPharmacocinetique_Click_1(sender As Object, e As EventArgs) Handles RadBtnPharmacocinetique.Click
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
    Private Sub RadBtnParmacodynamique_Click_1(sender As Object, e As EventArgs) Handles RadBtnParmacodynamique.Click
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
    Private Sub RadBtnEffetIndesirable_Click_1(sender As Object, e As EventArgs) Handles RadBtnEffetIndesirable.Click
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
    Private Sub RadBtnSubstance_Click_1(sender As Object, e As EventArgs) Handles RadBtnSubstance.Click
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

    Private Sub RadioBtnClassique_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnClassique.CheckedChanged
        If SelectedClasseAtc <> "" Then
            If PremierPassage = False Then
                PremierPassage = True
            Else
                ChargementMedicamentByAtc()
            End If
        End If
    End Sub

    Private Sub RadioBtnVirtuel_CheckedChanged(sender As Object, e As EventArgs) Handles RadioBtnVirtuel.CheckedChanged
        If SelectedClasseAtc <> "" Then
            If PremierPassage = False Then
                PremierPassage = True
            Else
                ChargementMedicamentByAtc()
            End If
        End If
    End Sub
End Class
