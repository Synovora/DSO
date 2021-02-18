Imports System.Collections.Specialized
Imports Telerik.WinControls.UI
Imports Oasis_Common
Public Class RadF_CI_ATC_Selecteur
    Private _SelectedPatient As Patient
    Private _selectedSpecialiteId As Integer
    Private _codeRetour As Boolean

    Public Property SelectedPatient As Patient
        Get
            Return _SelectedPatient
        End Get
        Set(value As Patient)
            _SelectedPatient = value
        End Set
    End Property

    Public Property SelectedSpecialiteId As Integer
        Get
            Return _selectedSpecialiteId
        End Get
        Set(value As Integer)
            _selectedSpecialiteId = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return _codeRetour
        End Get
        Set(value As Boolean)
            _codeRetour = value
        End Set
    End Property

    Dim theriaqueDao As New TheriaqueDao
    Dim contreIndicationATCDao As New ContreIndicationATCDao
    Dim contreIndicationSubstanceDao As New ContreIndicationSubstanceDao

    Dim ATCListe As New List(Of String)
    Dim SubstanceListe As New List(Of Integer)

    Dim RowCountATC1 As Integer

    Private Sub RadFATCListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Déclaration contre-indication - Sélection Substance / Classe Thérapeutique", userLog)

        Me.CodeRetour = False
        RadioBtnVirtuel.Checked = True

        ChargementEtatCivil()
        ChargementATC1()
        LblOccurrencesLues.Text = ""

        'Déclaration de traitement arrêté pour une spécialité donnée
        If SelectedSpecialiteId <> 0 Then
            RadTxtSpecialite.Text = theriaqueDao.getSpecialiteDenominationById(SelectedSpecialiteId)
            RadTxtSpecialite.ReadOnly = True
            RadBtnSpec2.Hide()
            RadBtnSpec3.Hide()
            RadBtnSpec4.Hide()
            RadBtnSpec5.Hide()
            RadBtnFiltreSpecialite.Hide()
            GetFiltre()
        End If
    End Sub

    Private Sub ChargementEtatCivil()
        Dim patientDao As New PatientDao
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
        Dim StringContreIndicationToolTip As String = patientDao.GetStringContreIndicationByPatient(SelectedPatient.patientId)
        If StringContreIndicationToolTip = "" Then
            lblContreIndication.Hide()
        Else
            lblContreIndication.Show()
            ToolTip.SetToolTip(lblContreIndication, StringContreIndicationToolTip)
        End If

        'Allergie
        Dim StringAllergieToolTip As String = patientDao.GetStringAllergieByPatient(SelectedPatient.patientId)
        If StringAllergieToolTip = "" Then
            LblAllergie.Hide()
        Else
            LblAllergie.Show()
            ToolTip.SetToolTip(LblAllergie, StringAllergieToolTip)
        End If

    End Sub

    'Liste des médicaments par dénomination
    Private Sub RadBtnFiltre_Click(sender As Object, e As EventArgs) Handles RadBtnFiltreSpecialite.Click
        Dim NbCar As Integer = RadTxtSpecialite.Text.Length
        If RadTxtSpecialite.Text <> "" And NbCar > 3 Then
            GetFiltre()
        Else
            MessageBox.Show("Vous devez saisir au moins 4 caractères pour lancer cette option de recherche des médicaments !")
        End If
    End Sub

    Private Sub GetFiltre()
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
                    LblOccurrencesLues.Text = "Aucune occurrence ne correspond aux critères de recherche"
                End If
            End If
        End If
        Cursor.Current = Cursors.Default
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

    'Chargement du Grid affichant la classe thérapeutique de niveau 5
    Private Sub ChargementATC5(CodeATC As String, Optional codeATCFocus As String = "")
        RadGridViewATC5.Rows.Clear()
        Dim dt As DataTable
        dt = theriaqueDao.getATCListeByATCPere(CodeATC)

        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            RadGridViewATC5.Rows.Add(iGrid)

            RadGridViewATC5.Rows(iGrid).Cells("catc_code_pk").Value = dt.Rows(i)("catc_code_pk")
            RadGridViewATC5.Rows(iGrid).Cells("catc_nomf").Value = dt.Rows(i)("catc_nomf")
        Next

        If codeATCFocus = "" Then
            If RadGridViewATC5.Rows.Count > 0 Then
                RadGridViewATC5.CurrentRow = RadGridViewATC5.ChildRows(0)
                RadGridViewATC5.TableElement.VScrollBar.Value = 0
            End If
        Else
            For i = 0 To rowCount Step 1
                If codeATCFocus = RadGridViewATC5.Rows(i).Cells("catc_code_pk").Value Then
                    RadGridViewATC5.CurrentRow = RadGridViewATC5.ChildRows(i)
                    RadGridViewATC5.TableElement.VScrollBar.Value = 0
                End If
            Next
        End If
    End Sub

    'Chargement substance
    Private Sub ChargementSubstance(SpecialiteId As Integer)
        Dim SubstanceListe As List(Of Integer)
        Dim iGrid As Integer = -1
        SubstanceListe = theriaqueDao.GetSubstanceCodeListBySpecialite(SpecialiteId)
        Dim EnumeratorSubstanceListe As IEnumerator = SubstanceListe.GetEnumerator()
        RadGridViewSubstance.Rows.Clear()
        While EnumeratorSubstanceListe.MoveNext()
            Dim CodeSubstance As Integer = EnumeratorSubstanceListe.Current
            Dim SubstanceDenomination As String = theriaqueDao.GetSubstanceDenominationById(CodeSubstance)
            iGrid += 1
            RadGridViewSubstance.Rows.Add(iGrid)
            RadGridViewSubstance.Rows(iGrid).Cells("SAC_CODE_SQ_PK").Value = CodeSubstance
            RadGridViewSubstance.Rows(iGrid).Cells("SAC_NOM").Value = SubstanceDenomination
        End While

        If RadGridViewSubstance.Rows.Count > 0 Then
            RadGridViewSubstance.CurrentRow = RadGridViewSubstance.ChildRows(0)
            RadGridViewSubstance.TableElement.VScrollBar.Value = 0
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
                RadTxtSpecialite.Text = ""
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
                RadTxtSpecialite.Text = ""
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
                RadTxtSpecialite.Text = ""
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
            Dim SpecialiteId As Integer = RadGridViewSpe.Rows(0).Cells("SP_CODE_SQ_PK").Value
            ChargementSubstance(SpecialiteId)
        End If
    End Sub

    Private Sub RadGridViewSpe_Click(sender As Object, e As EventArgs) Handles RadGridViewSpe.Click
        If RadGridViewSpe.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewSpe.Rows.IndexOf(Me.RadGridViewSpe.CurrentRow)
            If aRow >= 0 Then
                'Chargement ATC
                Dim CodeATC As String = RadGridViewSpe.Rows(aRow).Cells("SP_CATC_CODE_FK").Value
                AfficheATC(CodeATC)
                Dim SpecialiteId As Integer = RadGridViewSpe.Rows(aRow).Cells("SP_CODE_SQ_PK").Value
                ChargementSubstance(SpecialiteId)
            End If
        End If
    End Sub

    Private Sub AfficheATC(CodeATC As String)
        Cursor.Current = Cursors.WaitCursor
        Dim codeATC1, codeATC2, codeATC3, codeATC4, codeATC5 As String
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
                    If CodeATC.Length >= 7 Then
                        codeATC5 = CodeATC.Substring(0, 7)
                        ChargementATC5(codeATC4, codeATC5)
                    Else
                        RadGridViewATC5.Rows.Clear()
                    End If
                Else
                    RadGridViewATC4.Rows.Clear()
                    RadGridViewATC5.Rows.Clear()
                End If
            Else
                RadGridViewATC3.Rows.Clear()
                RadGridViewATC4.Rows.Clear()
                RadGridViewATC5.Rows.Clear()
            End If
        Else
            RadGridViewATC2.Rows.Clear()
            RadGridViewATC3.Rows.Clear()
            RadGridViewATC4.Rows.Clear()
            RadGridViewATC5.Rows.Clear()
        End If
        Cursor.Current = Cursors.Default
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

    Private Sub RadGridViewATC2_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadGridViewATC2.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    Private Sub RadGridViewATC3_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadGridViewATC3.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    Private Sub RadGridViewATC4_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadGridViewATC4.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    Private Sub RadGridViewATC5_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadGridViewATC5.CellFormatting
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

    'Sélection ATC
    Private Sub RadBtnSelectionATC2_Click(sender As Object, e As EventArgs) Handles RadBtnSelectionATC2.Click
        If RadGridViewATC2.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC2.Rows.IndexOf(Me.RadGridViewATC2.CurrentRow)
            If aRow >= 0 Then
                Dim codeATC As String = RadGridViewATC2.Rows(aRow).Cells("catc_code_pk").Value
                Selection(codeATC)
            End If
        End If
    End Sub

    Private Sub RadBtnSelectionATC3_Click(sender As Object, e As EventArgs) Handles RadBtnSelectionATC3.Click
        If RadGridViewATC3.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC3.Rows.IndexOf(Me.RadGridViewATC3.CurrentRow)
            If aRow >= 0 Then
                Dim codeATC As String = RadGridViewATC3.Rows(aRow).Cells("catc_code_pk").Value
                Selection(codeATC)
            End If
        End If
    End Sub

    Private Sub RadBtnSelectionATC4_Click(sender As Object, e As EventArgs) Handles RadBtnSelectionATC4.Click
        If RadGridViewATC4.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC4.Rows.IndexOf(Me.RadGridViewATC4.CurrentRow)
            If aRow >= 0 Then
                Dim codeATC As String = RadGridViewATC4.Rows(aRow).Cells("catc_code_pk").Value
                Selection(codeATC)
            End If
        End If
    End Sub

    Private Sub RadBtnSelectionATC5_Click(sender As Object, e As EventArgs) Handles RadBtnSelectionATC5.Click
        If RadGridViewATC5.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATC5.Rows.IndexOf(Me.RadGridViewATC5.CurrentRow)
            If aRow >= 0 Then
                Dim codeATC As String = RadGridViewATC5.Rows(aRow).Cells("catc_code_pk").Value
                Selection(codeATC)
            End If
        End If
    End Sub

    Private Sub Selection(CodeATC As String)
        If ATCListe.Contains(CodeATC) = False Then
            ATCListe.Add(CodeATC)
        End If
        ChargementATCListe()
    End Sub

    'Chargement des ATC sélectionnées
    Private Sub ChargementATCListe()
        Dim iGrid As Integer = -1
        Dim EnumeratorSubstanceListe As IEnumerator = ATCListe.GetEnumerator()
        RadGridViewATCListe.Rows.Clear()
        While EnumeratorSubstanceListe.MoveNext()
            Dim ATCId As String = EnumeratorSubstanceListe.Current
            iGrid += 1
            RadGridViewATCListe.Rows.Add(iGrid)
            RadGridViewATCListe.Rows(iGrid).Cells("catc_code_pk").Value = ATCId
            Dim ATCDenomination As String = theriaqueDao.GetATCDenominationById(ATCId)
            RadGridViewATCListe.Rows(iGrid).Cells("catc_nomf").Value = ATCDenomination
        End While
    End Sub

    'Chargement des substances sélectionnées
    Private Sub ChargementSubstanceListe()
        Dim iGrid As Integer = -1
        Dim EnumeratorSubstanceSelected As IEnumerator = SubstanceListe.GetEnumerator()
        RadGridViewSubstanceSelected.Rows.Clear()
        While EnumeratorSubstanceSelected.MoveNext()
            Dim SubstanceId As Integer = EnumeratorSubstanceSelected.Current
            iGrid += 1
            RadGridViewSubstanceSelected.Rows.Add(iGrid)
            RadGridViewSubstanceSelected.Rows(iGrid).Cells("SAC_CODE_SQ_PK").Value = SubstanceId
            Dim SubstanceDenomination As String = theriaqueDao.GetSubstanceDenominationById(SubstanceId)
            RadGridViewSubstanceSelected.Rows(iGrid).Cells("SAC_NOM").Value = SubstanceDenomination
        End While
    End Sub

    'Sélection substance et ATC pour création contre-indication
    Private Sub RadBtnSelectionCI_Click(sender As Object, e As EventArgs) Handles RadBtnSelectionCI.Click
        Dim NombreCICreation As Integer = 0
        Dim EnumeratorATCListe As IEnumerator = ATCListe.GetEnumerator()
        While EnumeratorATCListe.MoveNext()
            Dim ATCId As String = EnumeratorATCListe.Current
            Dim ATCDenomination As String = theriaqueDao.GetATCDenominationById(ATCId)
            Dim contreIndicationATC As New ContreIndicationATC
            contreIndicationATC.PatientId = SelectedPatient.patientId
            contreIndicationATC.ATCId = ATCId
            contreIndicationATC.DenominationATC = ATCDenomination

            If contreIndicationATCDao.CreationContreIndicationATC(contreIndicationATC, userLog) = True Then
                NombreCICreation += 1
            End If
        End While

        Dim EnumeratorSubstanceListe As IEnumerator = SubstanceListe.GetEnumerator()
        While EnumeratorSubstanceListe.MoveNext()
            Dim SubstanceId As Long = EnumeratorSubstanceListe.Current
            Dim contreIndicationSubstance As New ContreIndicationSubstance

            contreIndicationSubstance.PatientId = SelectedPatient.patientId
            contreIndicationSubstance.SubstanceId = SubstanceId
            Dim substance As Substance = theriaqueDao.GetSubstanceById(SubstanceId)

            'Dénomination
            contreIndicationSubstance.DenominationSubstance = substance.SubstanceDenomination

            'Substance père
            contreIndicationSubstance.SubstancePereId = substance.SubstancePereId

            'Dénomination substance père
            contreIndicationSubstance.DenominationSubstancePere = ""

            If contreIndicationSubstanceDao.CreationContreIndicationSubstance(contreIndicationSubstance, userLog) = True Then
                NombreCICreation += 1
            End If
        End While

        Dim form As New RadFNotification()
        Select Case NombreCICreation
            Case 0
                form.Message = "Attention, aucune contre-indication a été créée pour le patient"
                form.Show()
            Case 1
                form.Message = "1 contre-indication a été créée pour le patient"
                form.Show()
            Case Else
                form.Message = NombreCICreation & " Contre-indications ont été créées pour le patient"
                form.Show()
        End Select

        Me.CodeRetour = True
        Close()
    End Sub

    Private Sub RadBtnListeMedeocATCSelection_Click(sender As Object, e As EventArgs) Handles RadBtnListeMedeocATCSelection.Click
        If RadGridViewATCListe.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATCListe.Rows.IndexOf(Me.RadGridViewATCListe.CurrentRow)
            If aRow >= 0 Then
                Dim CodeAtc As String = RadGridViewATCListe.Rows(aRow).Cells("catc_code_pk").Value
                GetSpecialiteByATC(CodeAtc)
            End If
        End If
    End Sub

    'Liste des médicaments correspondant à la substance sélectionnée
    Private Sub RadBtnListeMedicamentSubstance_Click(sender As Object, e As EventArgs) Handles RadBtnListeMedicamentSubstance.Click
        If RadGridViewSubstanceSelected.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewSubstanceSelected.Rows.IndexOf(Me.RadGridViewSubstanceSelected.CurrentRow)
            If aRow >= 0 Then
                Cursor.Current = Cursors.WaitCursor
                Dim SubstanceId As Integer = RadGridViewSubstanceSelected.Rows(aRow).Cells("SAC_CODE_SQ_PK").Value
                Dim dt As DataTable
                dt = theriaqueDao.getSpecialiteByArgument(SubstanceId, TheriaqueDao.EnumGetSpecialite.SUBSTANCE_ACTIVE, TheriaqueDao.EnumMonoVir.VIRTUEL)
                ChargementSpecialite(dt, True)
                Cursor.Current = Cursors.Default
            End If
        End If
    End Sub

    Private Sub RadBtnEnleverATCListe_Click(sender As Object, e As EventArgs) Handles RadBtnEnleverATCListe.Click
        If RadGridViewATCListe.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewATCListe.Rows.IndexOf(Me.RadGridViewATCListe.CurrentRow)
            If aRow >= 0 Then
                Dim CodeAtc As String = RadGridViewATCListe.Rows(aRow).Cells("catc_code_pk").Value
                ATCListe.Remove(CodeAtc)
                ChargementATCListe()
            End If
        End If
    End Sub

    Private Sub RadBtnEnleverSubstance_Click(sender As Object, e As EventArgs) Handles RadBtnEnleverSubstance.Click
        If RadGridViewSubstanceSelected.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewSubstanceSelected.Rows.IndexOf(Me.RadGridViewSubstanceSelected.CurrentRow)
            If aRow >= 0 Then
                Dim SubstanceId As Integer = RadGridViewSubstanceSelected.Rows(aRow).Cells("SAC_CODE_SQ_PK").Value
                SubstanceListe.Remove(SubstanceId)
                ChargementSubstanceListe()
            End If
        End If
    End Sub

    Private Sub RadBtnViderListeATC_Click(sender As Object, e As EventArgs) Handles RadBtnViderListeATC.Click
        ATCListe.Clear()
        ChargementATCListe()
    End Sub

    Private Sub RadBtnViderSubstances_Click(sender As Object, e As EventArgs) Handles RadBtnViderSubstances.Click
        SubstanceListe.Clear()
        RadGridViewSubstanceSelected.Rows.Clear()
    End Sub

    Private Sub LblContreIndication_Click(sender As Object, e As EventArgs) Handles lblContreIndication.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPatientContreIndicationListe As New RadFPatientContreIndicationListe
            vFPatientContreIndicationListe.SelectedPatient = Me.SelectedPatient
            vFPatientContreIndicationListe.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    Private Sub LblAllergie_Click(sender As Object, e As EventArgs) Handles LblAllergie.Click
        Me.Enabled = False
        Cursor.Current = Cursors.WaitCursor
        Using vFPatientAllergieListe As New RadFPatientAllergieListe
            vFPatientAllergieListe.SelectedPatient = Me.SelectedPatient
            vFPatientAllergieListe.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    Private Sub RadBtnSelectionSubstance_Click(sender As Object, e As EventArgs) Handles RadBtnSelectionSubstance.Click
        If RadGridViewSubstance.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewSubstance.Rows.IndexOf(Me.RadGridViewSubstance.CurrentRow)
            If aRow >= 0 Then
                Dim SubstanceId As Integer = RadGridViewSubstance.Rows(aRow).Cells("SAC_CODE_SQ_PK").Value
                If SubstanceListe.Contains(SubstanceId) = False Then
                    SubstanceListe.Add(SubstanceId)
                End If
                ChargementSubstanceListe()
            End If
        End If
    End Sub

End Class
