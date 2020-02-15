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
        Dim ValeurParametreNonSaisie As Boolean = False
        Dim parmDataTable As DataTable
        parmDataTable = EpisodeParametreDao.getAllParametreEpisodeByEpisodeId(SelectedEpisodeId)
        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = parmDataTable.Rows.Count - 1
        Dim entier, nombreDecimal, longueurString, idString As Integer
        Dim Valeur, ValeurIMC, ValeurPAM As Decimal
        Dim description, unite, valeurString, parametreString As String
        Dim ParametreId, EpisodeParametreId, EpisodeParametreIdIMC, EpisodeParametreIdPAM As Long
        Dim valeurPoids, valeurTaille, valeurPAS, valeurPAD As Decimal
        Dim uniteIMC, unitePAM As String
        Dim IMCaCalculer As Boolean = False
        Dim PAMaCalculer As Boolean = False

        LblParametre1.Text = ""
        LblParametre2.Text = ""
        LblParametre3.Text = ""
        longueurString = 0
        idString = 1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            Valeur = parmDataTable.Rows(i)("valeur")
            If Valeur = 0 Then
                ValeurParametreNonSaisie = True
            End If
            description = parmDataTable.Rows(i)("description")
            entier = parmDataTable.Rows(i)("entier")
            nombreDecimal = parmDataTable.Rows(i)("decimal")
            unite = parmDataTable.Rows(i)("unite")
            ParametreId = parmDataTable.Rows(i)("parametre_id")
            EpisodeParametreId = parmDataTable.Rows(i)("episode_parametre_id")
            valeurString = ""

            Select Case entier
                Case 1
                    Select Case nombreDecimal
                        Case 0
                            valeurString = Valeur.ToString("0")
                        Case 1
                            valeurString = Valeur.ToString("0.0")
                        Case 2
                            valeurString = Valeur.ToString("0.00")
                        Case 3
                            valeurString = Valeur.ToString("0.000")
                    End Select
                Case 2
                    Select Case nombreDecimal
                        Case 0
                            valeurString = Valeur.ToString("#0")
                        Case 1
                            valeurString = Valeur.ToString("#0.0")
                        Case 2
                            valeurString = Valeur.ToString("#0.00")
                        Case 3
                            valeurString = Valeur.ToString("#0.000")
                    End Select
                Case 3
                    Select Case nombreDecimal
                        Case 0
                            valeurString = Valeur.ToString("##0")
                        Case 1
                            valeurString = Valeur.ToString("##0.0")
                        Case 2
                            valeurString = Valeur.ToString("##0.00")
                        Case 3
                            valeurString = Valeur.ToString("##0.000")
                    End Select
            End Select

            Select Case ParametreId
                Case 1
                    LblLabelPoids.Text = "Poids"
                    LblParmPoids.Text = valeurString & " " & unite
                    valeurPoids = Valeur
                Case 2
                    LblLabelTaille.Text = "Taille"
                    LblParmTaille.Text = valeurString & " " & unite
                    valeurTaille = Valeur
                Case 3
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
                Case 6
                    LblLabelPAS.Text = "PAS"
                    LblParmPAS.Text = valeurString & " " & unite
                    valeurPAS = Valeur
                Case 7
                    LblLabelPAD.Text = "PAD"
                    LblParmPAD.Text = valeurString & " " & unite
                    valeurPAD = Valeur
                Case 8
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

        If IMCaCalculer = True Then
            If valeurTaille = 0 Then
                valeurTaille = SelectedPatient.Taille
            End If
            If valeurPoids <> 0 And valeurTaille <> 0 Then
                Dim ValeurCalcul As Decimal = valeurPoids / ((valeurTaille * valeurTaille) / 10000)
                Dim ValeurCalculAComparer As Decimal = Decimal.Round(ValeurCalcul, 3)
                'Mise à jour du paramètre déduit
                If ValeurIMC <> ValeurCalculAComparer Then
                    ValeurIMC = ValeurCalculAComparer
                    episodeParametreDao.ModificationValeurEpisodeParametre(EpisodeParametreIdIMC, ValeurIMC)
                End If
            Else
                Valeur = 0
            End If
            valeurString = ValeurIMC.ToString("#0.0")
            LblParmIMC.Text = valeurString & " " & uniteIMC
        End If

        If PAMaCalculer = True Then
            If valeurPAD <> 0 And valeurPAS <> 0 Then
                Dim ValeurCalcul As Decimal = (valeurPAS + (2 * valeurPAD)) / 3
                Dim ValeurCalculAComparer As Decimal = Decimal.Round(ValeurCalcul, 3)
                'Mise à jour du paramètre déduit
                If ValeurPAM <> ValeurCalculAComparer Then
                    ValeurPAM = ValeurCalculAComparer
                    episodeParametreDao.ModificationValeurEpisodeParametre(EpisodeParametreIdPAM, ValeurPAM)
                End If
            Else
                Valeur = 0
            End If
            valeurString = ValeurPAM.ToString("##0")
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
            If ValeurParametreNonSaisie = True Then
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
        Me.Enabled = True
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub
End Class
