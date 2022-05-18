Imports System.Configuration
Imports Oasis_Common
Imports Telerik.WinControls.UI
Public Class RadFEpisodeParametresSaisie
    Private _SelectedPatient As Patient
    Private _SelectedEpisodeId As Long
    Private _CodeRetour As Boolean

    Public Property SelectedPatient As Patient
        Get
            Return _SelectedPatient
        End Get
        Set(value As Patient)
            _SelectedPatient = value
        End Set
    End Property

    Public Property SelectedEpisodeId As Long
        Get
            Return _SelectedEpisodeId
        End Get
        Set(value As Long)
            _SelectedEpisodeId = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return _CodeRetour
        End Get
        Set(value As Boolean)
            _CodeRetour = value
        End Set
    End Property

    Dim episodeParametreDao As New EpisodeParametreDao
    Dim episodeDao As New EpisodeDao

    Dim episode As Episode

    Private Sub RadFEpisodeParametresSaisie_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.CodeRetour = False
        ChargementEtatCivil()
        InitParametre()
        ChargementParametres()
    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientDateMaj.Text = SelectedPatient.PatientSyntheseDateMaj.ToString("dd/MM/yyyy")

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip1.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    Private Sub InitParametre()
        LblLabelPoids.Text = ""
        LblParmPoids.Text = ""
        LblLabelTemperature.Text = ""
        LblParmTemperature.Text = ""
        LblLabelFC.Text = ""
        LblParmFC.Text = ""
        LblLabelFR.Text = ""
        LblParmFR.Text = ""

        LblLabelTaille.Text = ""
        LblParmTaille.Text = ""
        LblLabelDextro.Text = ""
        LblParmDextro.Text = ""
        LblLabelPAS.Text = ""
        LblParmPAS.Text = ""
        LblLabelSat.Text = ""
        LblParmSat.Text = ""

        LblLabelIMC.Text = ""
        LblParmIMC.Text = ""
        LblLabelObservance.Text = ""
        LblParmObservance.Text = ""
        LblLabelPAD.Text = ""
        LblParmPAD.Text = ""
        LblLabelDEP.Text = ""
        LblParmDEP.Text = ""

        LblLabelPerimetreCranien.Text = ""
        LblParmPerimetreCranien.Text = ""
        LblLabelINR.Text = ""
        LblParmINR.Text = ""
        LblLabelPAM.Text = ""
        LblParmPAM.Text = ""
        LblLabelEVA.Text = ""
        LblParmEVA.Text = ""

        LblParametre1.Text = ""
        LblParametre2.Text = ""
        LblParametre3.Text = ""
        LblParametre4.Text = ""
    End Sub

    'Chargement des paramètres
    Private Sub ChargementParametres()
        Dim ValeurParametreNonSaisieGlobal As Boolean = False
        Dim parmDataTable As DataTable
        parmDataTable = episodeParametreDao.getAllParametreEpisodeByEpisodeId(SelectedEpisodeId)
        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = parmDataTable.Rows.Count - 1
        Dim entier, nombreDecimal, longueurString, idString As Integer
        Dim description, unite, valeurString, parametreString As String
        Dim ParametreId, EpisodeParametreId, EpisodeParametreIdIMC, EpisodeParametreIdPAM As Long
        Dim valeurPoids, valeurTaille, valeurPAS, valeurPAD, ValeurIMC, ValeurPAM, Valeur As Decimal?
        Dim uniteIMC, unitePAM As String
        Dim IMCaCalculer As Boolean = False
        Dim PAMaCalculer As Boolean = False

        LblParametre1.Text = ""
        LblParametre2.Text = ""
        LblParametre3.Text = ""
        uniteIMC = ""
        unitePAM = ""
        longueurString = 0
        idString = 1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            Dim ValeurParametreNonSaisie As Boolean = False
            Valeur = If(IsDBNull(parmDataTable.Rows(i)("valeur")), Nothing, parmDataTable.Rows(i)("valeur"))
            If Valeur Is Nothing Then
                ValeurParametreNonSaisie = True
                ValeurParametreNonSaisieGlobal = True
            End If
            description = parmDataTable.Rows(i)("description")
            entier = parmDataTable.Rows(i)("entier")
            nombreDecimal = parmDataTable.Rows(i)("decimal")
            unite = parmDataTable.Rows(i)("unite")
            ParametreId = parmDataTable.Rows(i)("parametre_id")
            EpisodeParametreId = parmDataTable.Rows(i)("episode_parametre_id")
            valeurString = ""

            If ParametreId = 2 Then
                If Valeur Is Nothing Then
                    Valeur = SelectedPatient.Taille
                End If
            End If

            If Not ValeurParametreNonSaisie Then
                Select Case entier
                    Case 1
                        Select Case nombreDecimal
                            Case 0
                                valeurString = Valeur.Value.ToString("0")
                            Case 1
                                valeurString = Valeur.Value.ToString("0.0")
                            Case 2
                                valeurString = Valeur.Value.ToString("0.00")
                            Case 3
                                valeurString = Valeur.Value.ToString("0.000")
                        End Select
                    Case 2
                        Select Case nombreDecimal
                            Case 0
                                valeurString = Valeur.Value.ToString("#0")
                            Case 1
                                valeurString = Valeur.Value.ToString("#0.0")
                            Case 2
                                valeurString = Valeur.Value.ToString("#0.00")
                            Case 3
                                valeurString = Valeur.Value.ToString("#0.000")
                        End Select
                    Case 3
                        Select Case nombreDecimal
                            Case 0
                                valeurString = Valeur.Value.ToString("##0")
                            Case 1
                                valeurString = Valeur.Value.ToString("##0.0")
                            Case 2
                                valeurString = Valeur.Value.ToString("##0.00")
                            Case 3
                                valeurString = Valeur.Value.ToString("##0.000")
                        End Select
                End Select
            End If

            Select Case ParametreId
                Case Parametre.EnumParametreId.POIDS
                    LblLabelPoids.Text = "Poids"
                    LblParmPoids.Text = valeurString & " " & unite
                    valeurPoids = Valeur
                Case Parametre.EnumParametreId.TAILLE
                    LblLabelTaille.Text = "Taille"
                    LblParmTaille.Text = valeurString & " " & unite
                    valeurTaille = Valeur
                    If ValeurParametreNonSaisie Then
                        valeurTaille = SelectedPatient.Taille
                    End If
                Case Parametre.EnumParametreId.IMC
                    LblLabelIMC.Text = "IMC"
                    uniteIMC = unite
                    EpisodeParametreIdIMC = EpisodeParametreId
                    ValeurIMC = Valeur
                    IMCaCalculer = True
                Case 4
                    LblLabelPerimetreCranien.Text = "Per. cranien"
                    LblParmPerimetreCranien.Text = valeurString & " " & unite
                Case 5
                    LblLabelFC.Text = "FC"
                    LblParmFC.Text = valeurString & " " & unite
                Case Parametre.EnumParametreId.PAS
                    LblLabelPAS.Text = "PAS"
                    LblParmPAS.Text = valeurString & " " & unite
                    valeurPAS = Valeur
                Case Parametre.EnumParametreId.PAD
                    LblLabelPAD.Text = "PAD"
                    LblParmPAD.Text = valeurString & " " & unite
                    valeurPAD = Valeur
                Case Parametre.EnumParametreId.PAM
                    LblLabelPAM.Text = "PAM"
                    unitePAM = unite
                    EpisodeParametreIdPAM = EpisodeParametreId
                    ValeurPAM = Valeur
                    PAMaCalculer = True
                Case 9
                    LblLabelFR.Text = "FR"
                    LblParmFR.Text = valeurString & " " & unite
                Case 10
                    LblLabelSat.Text = "Sat"
                    LblParmSat.Text = valeurString & " " & unite
                Case 11
                    LblLabelDEP.Text = "DEP"
                    LblParmDEP.Text = valeurString & " " & unite
                Case 12
                    LblLabelDextro.Text = "Dextro"
                    LblParmDextro.Text = valeurString & " " & unite
                Case 15
                    LblLabelINR.Text = "INR"
                    LblParmINR.Text = valeurString & " " & unite
                Case 16
                    LblLabelEVA.Text = "EVA"
                    LblParmEVA.Text = valeurString & " " & unite
                Case 17
                    LblLabelObservance.Text = "Observance"
                    LblParmObservance.Text = valeurString & " " & unite
                Case 20
                    LblLabelTemperature.Text = "Temperature"
                    LblParmTemperature.Text = valeurString & " " & unite
                Case Else
                    parametreString = "          " & description & " : " & valeurString & " " & unite
                    longueurString += parametreString.Length
                    Select Case idString
                        Case 1
                            If LblParametre1.Text = "" Then
                                LblParametre1.Text = description & " : " & valeurString & " " & unite
                            Else
                                LblParametre1.Text += parametreString
                            End If
                            idString = 2
                        Case 2
                            If LblParametre2.Text = "" Then
                                LblParametre2.Text = description & " : " & valeurString & " " & unite
                            Else
                                LblParametre2.Text += parametreString
                            End If
                            idString = 3
                        Case 3
                            If LblParametre3.Text = "" Then
                                LblParametre3.Text = description & " : " & valeurString & " " & unite
                            Else
                                LblParametre3.Text += parametreString
                            End If
                            idString = 4
                        Case 4
                            If LblParametre4.Text = "" Then
                                LblParametre4.Text = description & " : " & valeurString & " " & unite
                            Else
                                LblParametre4.Text += parametreString
                            End If
                            idString = 1
                    End Select
            End Select
        Next

        '20/02/2020 - BGA - Si la taille n'est pas traitée et qu'elle est stockée dans la table Patient on l'affiche à l'écran
        If LblLabelTaille.Text = "" Then
            Dim parametre As Parametre
            Dim parametreDao As New ParametreDao
            parametre = parametreDao.GetParametreById(Parametre.EnumParametreId.TAILLE) 'Taille
            LblLabelTaille.Text = "Taille"
            valeurString = SelectedPatient.Taille.ToString("##0")
            LblParmTaille.Text = valeurString & " " & parametre.Unite
        End If

        If IMCaCalculer = True Then
            If valeurTaille = 0 Then
                valeurTaille = SelectedPatient.Taille
            End If
            If valeurPoids <> 0 And valeurTaille <> 0 Then
                Dim ValeurCalcul As Decimal = valeurPoids / ((valeurTaille * valeurTaille) / 10000)
                Dim ValeurCalculAComparer As Decimal = Decimal.Round(ValeurCalcul, 3)
                'Mise à jour du paramètre déduit
                If Not ValeurIMC.HasValue OrElse ValeurIMC <> ValeurCalculAComparer Then
                    ValeurIMC = ValeurCalculAComparer
                    episodeParametreDao.ModificationValeurEpisodeParametre(EpisodeParametreIdIMC, ValeurIMC)
                End If
            Else
                ValeurIMC = 0
            End If
            valeurString = ValeurIMC.Value.ToString("#0.0")
            LblParmIMC.Text = valeurString & " " & uniteIMC
        End If

        If PAMaCalculer = True Then
            If valeurPAD.HasValue And valeurPAS.HasValue Then
                Dim ValeurCalcul As Decimal = (valeurPAS + (2 * valeurPAD)) / 3
                Dim ValeurCalculAComparer As Decimal = Decimal.Round(ValeurCalcul, 3)
                If Not ValeurPAM.HasValue OrElse ValeurPAM <> ValeurCalculAComparer Then
                    ValeurPAM = ValeurCalculAComparer
                    episodeParametreDao.ModificationValeurEpisodeParametre(EpisodeParametreIdPAM, ValeurPAM)
                End If
            Else
                ValeurPAM = 0
            End If
            valeurString = ValeurPAM.Value.ToString("##0")
            LblParmPAM.Text = valeurString & " " & unitePAM
        End If

        Dim SaisieParametreOK As Boolean = True
        If SelectedEpisodeId <> 0 Then
            episode = episodeDao.GetEpisodeById(SelectedEpisodeId)
            If userLog.UtilisateurAdmin = False Then
                If episode.DateModification.Date <> Date.Now.Date Then
                    RadBtnParametre.Enabled = False
                    SaisieParametreOK = False
                End If
            End If
        End If
        If SaisieParametreOK = True Then
            If ValeurParametreNonSaisieGlobal = True Then
                RadBtnParametre.ForeColor = Color.Red
                RadBtnParametre.Font = New Font(RadBtnParametre.Font, FontStyle.Bold)
                ToolTip1.SetToolTip(RadBtnParametre, "Des paramètres requis ne sont pas saisis")
            Else
                RadBtnParametre.ForeColor = Color.FromArgb(21, 66, 139)
                RadBtnParametre.Font = New Font(RadBtnParametre.Font, FontStyle.Regular)
                ToolTip1.SetToolTip(RadBtnParametre, "")
            End If
        End If
    End Sub

    'Appel saisie des paramètres
    Private Sub RadBtnParametre_Click(sender As Object, e As EventArgs) Handles RadBtnParametre.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False

        Try
            Using vRadFParametreDetailEdit As New RadFEpisodeParametreDetailEdit
                vRadFParametreDetailEdit.SelectedEpisodeId = Me.SelectedEpisodeId
                vRadFParametreDetailEdit.SelectedPatient = Me.SelectedPatient
                vRadFParametreDetailEdit.ShowDialog()
                If vRadFParametreDetailEdit.CodeRetour = True Then
                    InitParametre()
                    ChargementParametres()
                    Me.CodeRetour = True
                End If
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

        Me.Enabled = True
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub
End Class
