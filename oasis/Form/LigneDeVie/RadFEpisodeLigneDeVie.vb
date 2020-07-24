Imports System.Configuration
Imports Oasis_Common
Imports Telerik.WinControls
Imports Telerik.WinControls.UI

Public Class RadFEpisodeLigneDeVie
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private _EpisodeIdDejaOuvert As Long
    Private _ecranPrecedent As EnumAccesEcranPrecedent

    Public Property SelectedPatient As Patient
        Get
            Return privateSelectedPatient
        End Get
        Set(value As Patient)
            privateSelectedPatient = value
        End Set
    End Property

    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property

    Public Property EpisodeIdDejaOuvert As Long
        Get
            Return _EpisodeIdDejaOuvert
        End Get
        Set(value As Long)
            _EpisodeIdDejaOuvert = value
        End Set
    End Property

    Friend Property EcranPrecedent As EnumAccesEcranPrecedent
        Get
            Return _ecranPrecedent
        End Get
        Set(value As EnumAccesEcranPrecedent)
            _ecranPrecedent = value
        End Set
    End Property

    Dim ordonnanceDao As New OrdonnanceDao
    Dim episodeDao As New EpisodeDao
    Dim parametreDao As New ParametreDao
    Dim episodeParametreDao As New EpisodeParametreDao
    Dim patientParametreLdvDao As New PatientParametreLdvDao

    Dim patientParametreLdv As PatientParametreLdv
    Dim ligneDeVie As New LigneDeVie

    Dim listeParametreaAfficher As New List(Of Long)

    Dim Parametre1Id As Long
    Dim Parametre2Id As Long
    Dim Parametre3Id As Long
    Dim Parametre4Id As Long
    Dim Parametre5Id As Long

    Dim Entier1, Entier2, Entier3, Entier4, Entier5 As Integer
    Dim Decimale1, Decimale2, Decimale3, Decimale4, Decimale5 As Integer

    Dim ConfigurationParametreExiste As Boolean

    Private Sub RadFEpisodeListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        'Contrôle d'accès aux écran Synthèse, épisode et ligne de vie
        Environnement.ControleAccesForm.addFormToControl(EnumForm.LIGNE_DE_VIE.ToString)
        If Environnement.ControleAccesForm.IsAccessToFormOK(EnumForm.EPISODE.ToString) = False Then
            RadBtnEpisode.Hide()
        End If

        AfficheTitleForm(Me, "Ligne de vie du patient", userLog)
        If userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString OrElse userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString Then
        Else
            RadBtnCreationEpisodeParametre.Enabled = False
        End If
        GetParametresEtFiltres()
        InitFiltre()
        ChargementEtatCivil()
        ChargementEpisode(ligneDeVie)
    End Sub

    Private Sub GetParametresEtFiltres()
        ConfigurationParametreExiste = True
        Try
            patientParametreLdv = patientParametreLdvDao.GetParametreByPatientId(SelectedPatient.patientId)
        Catch ex As Exception
            Dim messageErreur As String = ex.Message
            If messageErreur.StartsWith("RNF") Then
                ConfigurationParametreExiste = False
                patientParametreLdv = New PatientParametreLdv()
                patientParametreLdv.PatientId = SelectedPatient.patientId
            End If
        End Try
    End Sub

    Private Sub InitFiltre()
        Dim limiteAgeEnfant As Integer
        Dim limiteAgeEnfantString As Integer = ConfigurationManager.AppSettings("limiteAgeEnfant")
        If IsNumeric(limiteAgeEnfantString) Then
            limiteAgeEnfant = CInt(limiteAgeEnfantString)
        Else
            limiteAgeEnfant = 16
        End If

        Dim AgeMinPreventionFemme As Integer
        Dim AgeMinPreventionFemmeString As Integer = ConfigurationManager.AppSettings("AgeMinPreventionFemme")
        If IsNumeric(AgeMinPreventionFemmeString) Then
            AgeMinPreventionFemme = CInt(AgeMinPreventionFemmeString)
        Else
            AgeMinPreventionFemme = 12
        End If

        ChkTypeConsultation.Checked = True
        ChkTypeVirtuel.Checked = True
        ChkTypeParametre.Checked = True

        ChkEnfantPreScolaire.Checked = True
        ChkEnfantScolaire.Checked = True
        ChkPathologieAigue.Checked = True
        ChkPreventionAutre.Checked = True
        ChkSocial.Checked = True
        ChkSuiviChronique.Checked = True
        ChkSuiviGrossesse.Checked = True
        ChkSuiviGynecologique.Checked = True

        ChkProfilMedical.Checked = True
        ChkProfilParamedical.Checked = True

        ligneDeVie.TypeConsultation = True
        ligneDeVie.TypeVirtuel = True
        ligneDeVie.TypeParametre = True

        ligneDeVie.ActivitePathologieAigue = True
        ligneDeVie.ActivitePreventionAutre = True
        ligneDeVie.ActivitePreventionEnfantPreScolaire = True
        ligneDeVie.ActivitePreventionEnfantScolaire = True
        ligneDeVie.ActiviteSocial = True
        ligneDeVie.ActiviteSuiviChronique = True
        ligneDeVie.ActiviteSuiviGrossesse = True
        ligneDeVie.ActiviteSuiviGyncologique = True

        ligneDeVie.ProfilMedical = True
        ligneDeVie.ProfilParamedical = True

        Dim Age As Integer = CalculAgeEnAnnee(SelectedPatient.PatientDateNaissance)
        If Age > limiteAgeEnfant Then
            ligneDeVie.ActivitePreventionEnfantPreScolaire = False
            ligneDeVie.ActivitePreventionEnfantScolaire = False
            ChkEnfantPreScolaire.Hide()
            ChkEnfantScolaire.Hide()
            If SelectedPatient.PatientGenreId = patientDao.EnumGenreId.Feminin OrElse Age >= AgeMinPreventionFemme Then
                ChkSuiviGrossesse.Location = New Point(427, 44)
                ChkSuiviGynecologique.Location = New Point(589, 44)
            End If
        End If

        If SelectedPatient.PatientGenreId = patientDao.EnumGenreId.Masculin OrElse Age < AgeMinPreventionFemme Then
            ligneDeVie.ActiviteSuiviGrossesse = False
            ligneDeVie.ActiviteSuiviGyncologique = False
            ChkSuiviGrossesse.Hide()
            ChkSuiviGynecologique.Hide()
        End If

        Lblparametre1.Text = ""
        LblParametre2.Text = ""
        LblParametre3.Text = ""
        LblParametre4.Text = ""
        LblParametre5.Text = ""

        Parametre1Id = 0
        Parametre2Id = 0
        Parametre3Id = 0
        Parametre4Id = 0
        Parametre5Id = 0

        Entier1 = 0
        Entier2 = 0
        Entier3 = 0
        Entier4 = 0
        Entier5 = 0

        Decimale1 = 0
        Decimale2 = 0
        Decimale3 = 0
        Decimale4 = 0
        Decimale5 = 0

        DteDepuis.Value = Date.Now()

        DteJusqua.Value = Date.Now().AddYears(-1)

        LblLabelParametre.Hide()

        If ConfigurationParametreExiste = True Then
            ChkTypeConsultation.Checked = patientParametreLdv.TypeConsultation
            ChkTypeVirtuel.Checked = patientParametreLdv.TypeVirtuel
            ChkTypeParametre.Checked = patientParametreLdv.TypeParametre

            ChkEnfantPreScolaire.Checked = patientParametreLdv.ActivitePreventionEnfantPreScolaire
            ChkEnfantScolaire.Checked = patientParametreLdv.ActivitePreventionEnfantScolaire
            ChkPathologieAigue.Checked = patientParametreLdv.ActivitePathologieAigue
            ChkPreventionAutre.Checked = patientParametreLdv.ActivitePreventionAutre
            ChkSocial.Checked = patientParametreLdv.ActiviteSocial
            ChkSuiviChronique.Checked = patientParametreLdv.ActiviteSuiviChronique
            ChkSuiviGrossesse.Checked = patientParametreLdv.ActiviteSuiviGrossesse
            ChkSuiviGynecologique.Checked = patientParametreLdv.ActiviteSuiviGynecologique

            ChkProfilMedical.Checked = patientParametreLdv.ProfilMedical
            ChkProfilParamedical.Checked = patientParametreLdv.ProfilParamedical

            ligneDeVie.TypeConsultation = patientParametreLdv.TypeConsultation
            ligneDeVie.TypeVirtuel = patientParametreLdv.TypeVirtuel
            ligneDeVie.TypeParametre = patientParametreLdv.TypeParametre

            ligneDeVie.ActivitePathologieAigue = patientParametreLdv.ActivitePathologieAigue
            ligneDeVie.ActivitePreventionAutre = patientParametreLdv.ActivitePreventionAutre
            ligneDeVie.ActivitePreventionEnfantPreScolaire = patientParametreLdv.ActivitePreventionEnfantPreScolaire
            ligneDeVie.ActivitePreventionEnfantScolaire = patientParametreLdv.ActivitePreventionEnfantScolaire
            ligneDeVie.ActiviteSocial = patientParametreLdv.ActiviteSocial
            ligneDeVie.ActiviteSuiviChronique = patientParametreLdv.ActiviteSuiviChronique
            ligneDeVie.ActiviteSuiviGrossesse = patientParametreLdv.ActiviteSuiviGrossesse
            ligneDeVie.ActiviteSuiviGyncologique = patientParametreLdv.ActiviteSuiviGynecologique

            ligneDeVie.ProfilMedical = patientParametreLdv.ProfilMedical
            ligneDeVie.ProfilParamedical = patientParametreLdv.ProfilParamedical

            If patientParametreLdv.Parametre1 <> 0 Then
                listeParametreaAfficher.Add(patientParametreLdv.Parametre1)
                ligneDeVie.ParametreId1 = patientParametreLdv.Parametre1
            Else
                ligneDeVie.ParametreId1 = 0
            End If
            If patientParametreLdv.Parametre2 <> 0 Then
                listeParametreaAfficher.Add(patientParametreLdv.Parametre2)
                ligneDeVie.ParametreId2 = patientParametreLdv.Parametre2
            Else
                ligneDeVie.ParametreId2 = 0
            End If
            If patientParametreLdv.Parametre3 <> 0 Then
                listeParametreaAfficher.Add(patientParametreLdv.Parametre3)
                ligneDeVie.ParametreId3 = patientParametreLdv.Parametre3
            Else
                ligneDeVie.ParametreId3 = 0
            End If
            If patientParametreLdv.Parametre4 <> 0 Then
                listeParametreaAfficher.Add(patientParametreLdv.Parametre4)
                ligneDeVie.ParametreId4 = patientParametreLdv.Parametre4
            Else
                ligneDeVie.ParametreId4 = 0
            End If
            If patientParametreLdv.Parametre5 <> 0 Then
                listeParametreaAfficher.Add(patientParametreLdv.Parametre5)
                ligneDeVie.ParametreId5 = patientParametreLdv.Parametre5
            Else
                ligneDeVie.ParametreId5 = 0
            End If

            AfficheParametres()
        End If

    End Sub

    Private Sub AfficheParametres()
        Lblparametre1.Text = ""
        LblParametre2.Text = ""
        LblParametre3.Text = ""
        LblParametre4.Text = ""
        LblParametre5.Text = ""

        Parametre1Id = 0
        Parametre2Id = 0
        Parametre3Id = 0
        Parametre4Id = 0
        Parametre5Id = 0

        Entier1 = 0
        Entier2 = 0
        Entier3 = 0
        Entier4 = 0
        Entier5 = 0

        Decimale1 = 0
        Decimale2 = 0
        Decimale3 = 0
        Decimale4 = 0
        Decimale5 = 0

        ligneDeVie.ParametreId1 = 0
        ligneDeVie.ParametreId2 = 0
        ligneDeVie.ParametreId3 = 0
        ligneDeVie.ParametreId4 = 0
        ligneDeVie.ParametreId5 = 0

        Dim i As Integer = 0
        Dim ParametreEnumerator As List(Of Long).Enumerator = listeParametreaAfficher.GetEnumerator()
        While ParametreEnumerator.MoveNext()
            i += 1
            If i <= 5 Then
                Dim parametre As Parametre
                Dim parametreId As Long = ParametreEnumerator.Current
                parametre = parametreDao.GetParametreById(parametreId)
                Select Case i
                    Case 1
                        Lblparametre1.Text = parametre.Description & vbCrLf & parametre.Unite
                        Parametre1Id = parametre.Id
                        ligneDeVie.ParametreId1 = parametre.Id
                        Entier1 = parametre.Entier
                        Decimale1 = parametre.Decimal
                    Case 2
                        LblParametre2.Text = parametre.Description & vbCrLf & parametre.Unite
                        Parametre2Id = parametre.Id
                        ligneDeVie.ParametreId2 = parametre.Id
                        Entier2 = parametre.Entier
                        Decimale2 = parametre.Decimal
                    Case 3
                        LblParametre3.Text = parametre.Description & vbCrLf & parametre.Unite
                        Parametre3Id = parametre.Id
                        ligneDeVie.ParametreId3 = parametre.Id
                        Entier3 = parametre.Entier
                        Decimale3 = parametre.Decimal
                    Case 4
                        LblParametre4.Text = parametre.Description & vbCrLf & parametre.Unite
                        Parametre4Id = parametre.Id
                        ligneDeVie.ParametreId4 = parametre.Id
                        Entier4 = parametre.Entier
                        Decimale4 = parametre.Decimal
                    Case 5
                        LblParametre5.Text = parametre.Description & vbCrLf & parametre.Unite
                        Parametre5Id = parametre.Id
                        ligneDeVie.ParametreId5 = parametre.Id
                        Entier5 = parametre.Entier
                        Decimale5 = parametre.Decimal
                End Select
            Else
                Exit Sub
            End If
        End While
    End Sub

    Private Sub ChargementEpisode(ligneDeVie As LigneDeVie)
        Cursor.Current = Cursors.WaitCursor
        RadGridViewEpisode.Rows.Clear()

        Dim dt As DataTable
        Dim episodeDao As New EpisodeDao
        dt = episodeDao.GetAllEpisodeByPatient(SelectedPatient.patientId, DteDepuis.Value, DteJusqua.Value, ligneDeVie)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateCreation As Date
        Dim conclusionMedicale As String
        Dim ValeurParametre As Decimal
        Dim ValeurString As String
        Dim rowCount As Integer = dt.Rows.Count - 1

        Dim etatCode As String
        Dim DatePrecedente As Date = Date.Now()

        If Lblparametre1.Text <> "" Then
            RadGridViewEpisode.Columns.Item("parametre1").HeaderText = Lblparametre1.Text
            RadGridViewEpisode.Columns.Item("parametre1").IsVisible = True
        Else
            RadGridViewEpisode.Columns.Item("parametre1").IsVisible = False
        End If
        If LblParametre2.Text <> "" Then
            RadGridViewEpisode.Columns.Item("parametre2").HeaderText = LblParametre2.Text
            RadGridViewEpisode.Columns.Item("parametre2").IsVisible = True
        Else
            RadGridViewEpisode.Columns.Item("parametre2").IsVisible = False
        End If
        If LblParametre3.Text <> "" Then
            RadGridViewEpisode.Columns.Item("parametre3").HeaderText = LblParametre3.Text
            RadGridViewEpisode.Columns.Item("parametre3").IsVisible = True
        Else
            RadGridViewEpisode.Columns.Item("parametre3").IsVisible = False
        End If
        If LblParametre4.Text <> "" Then
            RadGridViewEpisode.Columns.Item("parametre4").HeaderText = LblParametre4.Text
            RadGridViewEpisode.Columns.Item("parametre4").IsVisible = True
        Else
            RadGridViewEpisode.Columns.Item("parametre4").IsVisible = False
        End If
        If LblParametre5.Text <> "" Then
            RadGridViewEpisode.Columns.Item("parametre5").HeaderText = LblParametre5.Text
            RadGridViewEpisode.Columns.Item("parametre5").IsVisible = True
        Else
            RadGridViewEpisode.Columns.Item("parametre5").IsVisible = False
        End If

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadGridViewEpisode.Rows.Add(iGrid)
            'Alimentation du DataGridView
            Dim episodeId As Long = dt.Rows(i)("episode_id")

            RadGridViewEpisode.Rows(iGrid).Cells("episode_id").Value = dt.Rows(i)("episode_id")
            RadGridViewEpisode.Rows(iGrid).Cells("type").Value = Coalesce(dt.Rows(i)("type"), "")

            If dt.Rows(i)("type_activite") = Episode.EnumTypeEpisode.PARAMETRE.ToString Then
                RadGridViewEpisode.Rows(iGrid).Cells("type_activite").Value = "Prise de paramètres"
            Else
                RadGridViewEpisode.Rows(iGrid).Cells("type_activite").Value = episodeDao.GetItemTypeActiviteByCode(Coalesce(dt.Rows(i)("type_activite"), ""))
            End If

            'Activité pour un épisode virtuel
            If RadGridViewEpisode.Rows(iGrid).Cells("type_activite").Value = "" Then
                RadGridViewEpisode.Rows(iGrid).Cells("type_activite").Value = Coalesce(dt.Rows(i)("type"), "")
            End If

            RadGridViewEpisode.Rows(iGrid).Cells("type_profil").Value = Coalesce(dt.Rows(i)("type_profil"), "")
            RadGridViewEpisode.Rows(iGrid).Cells("description_activite").Value = Coalesce(dt.Rows(i)("description_activite"), "")

            'Conclusion selon le type de l'épisode
            conclusionMedicale = Coalesce(dt.Rows(i)("observation_medical"), "")
            If conclusionMedicale <> "" Then
                RadGridViewEpisode.Rows(iGrid).Cells("conclusion").Value = conclusionMedicale
            Else
                RadGridViewEpisode.Rows(iGrid).Cells("conclusion").Value = Coalesce(dt.Rows(i)("observation_paramedical"), "")
            End If

            If RadGridViewEpisode.Columns.Item("parametre1").IsVisible = True Then
                RadGridViewEpisode.Rows(iGrid).Cells("parametre1").Value = ""
                If Parametre1Id <> 0 Then
                    ValeurParametre = Coalesce(dt.Rows(i)("ValeurParam1"), 0)
                    If ValeurParametre <> 0 Then
                        ValeurString = FormatParametre(Entier1, Decimale1, ValeurParametre)
                        RadGridViewEpisode.Rows(iGrid).Cells("parametre1").Value = ValeurString
                    End If
                End If
            End If

            If RadGridViewEpisode.Columns.Item("parametre2").IsVisible = True Then
                RadGridViewEpisode.Rows(iGrid).Cells("parametre2").Value = ""
                If Parametre2Id <> 0 Then
                    ValeurParametre = Coalesce(dt.Rows(i)("ValeurParam2"), 0)
                    If ValeurParametre <> 0 Then
                        ValeurString = FormatParametre(Entier2, Decimale2, ValeurParametre)
                        RadGridViewEpisode.Rows(iGrid).Cells("parametre2").Value = ValeurString
                    End If
                End If
            End If

            If RadGridViewEpisode.Columns.Item("parametre3").IsVisible = True Then
                RadGridViewEpisode.Rows(iGrid).Cells("parametre3").Value = ""
                If Parametre3Id <> 0 Then
                    ValeurParametre = Coalesce(dt.Rows(i)("ValeurParam3"), 0)
                    If ValeurParametre <> 0 Then
                        ValeurString = FormatParametre(Entier3, Decimale3, ValeurParametre)
                        RadGridViewEpisode.Rows(iGrid).Cells("parametre3").Value = ValeurString
                    End If
                End If
            End If

            If RadGridViewEpisode.Columns.Item("parametre4").IsVisible = True Then
                RadGridViewEpisode.Rows(iGrid).Cells("parametre4").Value = ""
                If Parametre4Id <> 0 Then
                    ValeurParametre = Coalesce(dt.Rows(i)("ValeurParam4"), 0)
                    If ValeurParametre <> 0 Then
                        ValeurString = FormatParametre(Entier4, Decimale4, ValeurParametre)
                        RadGridViewEpisode.Rows(iGrid).Cells("parametre4").Value = ValeurString
                    End If
                End If
            End If

            If RadGridViewEpisode.Columns.Item("parametre5").IsVisible = True Then
                RadGridViewEpisode.Rows(iGrid).Cells("parametre5").Value = ""
                If Parametre5Id <> 0 Then
                    ValeurParametre = Coalesce(dt.Rows(i)("ValeurParam5"), 0)
                    If ValeurParametre <> 0 Then
                        ValeurString = FormatParametre(Entier5, Decimale5, ValeurParametre)
                        RadGridViewEpisode.Rows(iGrid).Cells("parametre5").Value = ValeurString
                    End If
                End If
            End If

            dateCreation = Coalesce(dt.Rows(i)("date_creation"), Nothing)
            If dateCreation <> Nothing Then
                RadGridViewEpisode.Rows(iGrid).Cells("date_creation").Value = dateCreation.ToString("dd.MM.yyyy")
            Else
                RadGridViewEpisode.Rows(iGrid).Cells("date_creation").Value = ""
            End If

            Dim periode As String = CalculDureeEnJourString(dateCreation, DatePrecedente)
            RadGridViewEpisode.Rows(iGrid).Cells("periode").Value = periode
            DatePrecedente = dateCreation

            Dim OrdonnanceId = Coalesce(dt.Rows(i)("oa_ordonnance_id"), 0)
            If OrdonnanceId <> 0 Then
                RadGridViewEpisode.Rows(iGrid).Cells("ordonnance").Value = True
            Else
                RadGridViewEpisode.Rows(iGrid).Cells("ordonnance").Value = False
            End If

            etatCode = Coalesce(dt.Rows(i)("etat"), "")
            Select Case etatCode
                Case Episode.EnumEtatEpisode.EN_COURS.ToString
                    RadGridViewEpisode.Rows(iGrid).Cells("etat").Value = "En cours"
                    RadGridViewEpisode.Rows(iGrid).Cells("etat").Style.ForeColor = Color.Red
                    RadGridViewEpisode.Rows(iGrid).Cells("date_creation").Style.ForeColor = Color.Red
                    RadGridViewEpisode.Rows(iGrid).Cells("type_activite").Style.ForeColor = Color.Red
                    RadGridViewEpisode.Rows(iGrid).Cells("type_profil").Style.ForeColor = Color.Red
                    RadGridViewEpisode.Rows(iGrid).Cells("conclusion").Style.ForeColor = Color.Red
                    RadGridViewEpisode.Rows(iGrid).Cells("parametre1").Style.ForeColor = Color.Red
                    RadGridViewEpisode.Rows(iGrid).Cells("parametre2").Style.ForeColor = Color.Red
                    RadGridViewEpisode.Rows(iGrid).Cells("parametre3").Style.ForeColor = Color.Red
                    RadGridViewEpisode.Rows(iGrid).Cells("parametre4").Style.ForeColor = Color.Red
                    RadGridViewEpisode.Rows(iGrid).Cells("parametre5").Style.ForeColor = Color.Red
                Case Episode.EnumEtatEpisode.CLOTURE.ToString
                    RadGridViewEpisode.Rows(iGrid).Cells("etat").Value = "Clôturé"
                Case Else
                    RadGridViewEpisode.Rows(iGrid).Cells("etat").Value = "Inconnu !"
            End Select
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewEpisode.Rows.Count > 0 Then
            Me.RadGridViewEpisode.CurrentRow = RadGridViewEpisode.ChildRows(0)
        End If
        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblDateNaissance.Text = SelectedPatient.PatientDateNaissance.ToString("dd.MM.yyyy")
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientDateMaj.Text = SelectedPatient.PatientSyntheseDateMaj.ToString("dd.MM.yyyy")

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

    Private Sub RadBtnEpisode_Click(sender As Object, e As EventArgs) Handles RadBtnEpisode.Click
        DetailEpisode()
    End Sub

    Private Sub MasterTemplate_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadGridViewEpisode.CellDoubleClick
        DetailEpisode()
    End Sub

    Private Sub DetailEpisode()
        If Not (userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString Or userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString) Then
            Dim Message1 As String = "Votre profil de type (" & userLog.TypeProfil & ") ne vous permet pas de gérer un épisode patient, processus annulé"
            Dim Message2 As String = "Les types de profil autorisés sont : " & ProfilDao.EnumProfilType.MEDICAL.ToString() & " et " & ProfilDao.EnumProfilType.PARAMEDICAL.ToString()
            MessageBox.Show(Message1 & vbCrLf & Message2)
            Exit Sub
        End If

        If RadGridViewEpisode.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewEpisode.Rows.IndexOf(Me.RadGridViewEpisode.CurrentRow)
            If aRow >= 0 Then
                Dim EpisodeId As Integer = RadGridViewEpisode.Rows(aRow).Cells("episode_Id").Value
                If EpisodeId = EpisodeIdDejaOuvert Then
                    MessageBox.Show("Cet épisode est déjà ouvert dans l'écran qui a conduit à la consultation de la ligne de vie du patient")
                    Exit Sub
                End If
                If RadGridViewEpisode.Rows(aRow).Cells("type").Value = Episode.EnumTypeEpisode.PARAMETRE.ToString Then
                    Me.Enabled = False
                    Cursor.Current = Cursors.WaitCursor

                    Try
                        Using form As New RadFEpisodeParametresSaisie
                            form.SelectedPatient = SelectedPatient
                            form.SelectedEpisodeId = EpisodeId
                            form.ShowDialog()
                            If form.CodeRetour = True Then
                                ChargementEpisode(ligneDeVie)
                            End If
                        End Using
                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                    End Try

                    Me.Enabled = True
                Else
                    Me.Enabled = False
                    Cursor.Current = Cursors.WaitCursor

                    Try
                        Using form As New RadFEpisodeDetail
                            form.SelectedEpisodeId = EpisodeId
                            form.SelectedPatient = Me.SelectedPatient
                            form.UtilisateurConnecte = Me.UtilisateurConnecte
                            form.ShowDialog()
                            ChargementEpisode(ligneDeVie)
                        End Using
                    Catch ex As Exception
                        MessageBox.Show(ex.Message)
                    End Try

                    Me.Enabled = True
                End If
            End If
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub OrdonnanceMédicaleToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles OrdonnanceMédicaleToolStripMenuItem.Click
        If RadGridViewEpisode.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewEpisode.Rows.IndexOf(Me.RadGridViewEpisode.CurrentRow)
            If aRow >= 0 Then
                Dim EpisodeId As Integer = RadGridViewEpisode.Rows(aRow).Cells("episode_Id").Value
                Dim episode As Episode = episodeDao.GetEpisodeById(EpisodeId)
                Me.Enabled = False
                Cursor.Current = Cursors.WaitCursor
                Dim OrdonnanceId As Long
                Dim dt As DataTable
                dt = ordonnanceDao.getOrdonnanceValidebyPatient(SelectedPatient.patientId, EpisodeId)
                If dt.Rows.Count > 0 Then
                    'Ordonnance existante
                    OrdonnanceId = dt.Rows(0)("oa_ordonnance_id")
                    AfficheOrdonnance(OrdonnanceId, episode)
                Else
                    If episode.Etat = Episode.EnumEtatEpisode.CLOTURE.ToString OrElse episode.Etat = Episode.EnumEtatEpisode.ANNULE.ToString Then
                        If episode.DateModification.Date < Date.Now.Date Then
                            MessageBox.Show("Il n'y a pas d'ordonnance de créée pour cet épisode clôturé !")
                            Cursor.Current = Cursors.Default
                            Me.Enabled = True
                            Exit Sub
                        End If
                    End If
                    OrdonnanceId = ordonnanceDao.CreateOrdonnance(SelectedPatient.PatientId, EpisodeId, userLog)
                    If OrdonnanceId <> 0 Then
                        If ordonnanceDao.CreateNewOrdonnanceDetail(SelectedPatient.patientId, OrdonnanceId, episode) = True Then
                            AfficheOrdonnance(OrdonnanceId, episode)
                        Else
                            'Erreur, l'ordonnance détail n'a pa été créée
                        End If
                    Else
                        'Erreur, l'ordonnance n'a pa été créée
                    End If
                End If
                Cursor.Current = Cursors.Default
                Me.Enabled = True
            End If
        End If
    End Sub

    Private Sub AfficheOrdonnance(OrdonnanceId As Long, episode As Episode)

        Try
            Using vFOrdonnanceListeDetail As New RadFOrdonnanceListeDetail
                vFOrdonnanceListeDetail.SelectedOrdonnanceId = OrdonnanceId
                vFOrdonnanceListeDetail.SelectedPatient = Me.SelectedPatient
                vFOrdonnanceListeDetail.SelectedEpisode = episode
                vFOrdonnanceListeDetail.UtilisateurConnecte = Me.UtilisateurConnecte
                vFOrdonnanceListeDetail.CommentaireOrdonnance = ""
                vFOrdonnanceListeDetail.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub RadBtnParametre_Click(sender As Object, e As EventArgs) Handles RadBtnParametre.Click

        Try
            Using form As New RadFLigneDeVieParametreSelecteur
                form.ListeParametreaAfficher = listeParametreaAfficher
                form.ShowDialog()
                listeParametreaAfficher = form.ListeParametreaAfficher
                AfficheParametres()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try

    End Sub

    Private Sub Lblparametre1_Click(sender As Object, e As EventArgs) Handles Lblparametre1.Click
        Dim nombreParametre As Integer = listeParametreaAfficher.Count()
        If nombreParametre >= 1 Then
            listeParametreaAfficher.RemoveAt(0)
            AfficheParametres()
        End If
    End Sub

    Private Sub LblParametre2_Click(sender As Object, e As EventArgs) Handles LblParametre2.Click
        Dim nombreParametre As Integer = listeParametreaAfficher.Count()
        If nombreParametre >= 2 Then
            listeParametreaAfficher.RemoveAt(1)
            AfficheParametres()
        End If
    End Sub

    Private Sub LblParametre3_Click(sender As Object, e As EventArgs) Handles LblParametre3.Click
        Dim nombreParametre As Integer = listeParametreaAfficher.Count()
        If nombreParametre >= 3 Then
            listeParametreaAfficher.RemoveAt(2)
            AfficheParametres()
        End If
    End Sub

    Private Sub SousépisodesToolStripMenuItem_Click(sender As Object, e As EventArgs) Handles SousépisodesToolStripMenuItem.Click
        If RadGridViewEpisode.CurrentRow IsNot Nothing Then
            Me.Cursor = Cursors.WaitCursor
            Me.Enabled = False
            Dim EpisodeId As Integer = RadGridViewEpisode.CurrentRow.Cells("episode_Id").Value
            Dim episode As Episode = episodeDao.GetEpisodeById(EpisodeId)

            Try
                Using frm = New FrmSousEpisodeListe(episode, SelectedPatient)
                    frm.ShowDialog()
                End Using
            Catch ex As Exception
                MessageBox.Show(ex.Message)
            End Try


            Me.Enabled = True
            Me.Cursor = Cursors.Default
        End If
    End Sub

    Private Sub RadFEpisodeLigneDeVie_FormClosed(sender As Object, e As FormClosedEventArgs) Handles MyBase.FormClosed
        Environnement.ControleAccesForm.removeFormToControl(EnumForm.LIGNE_DE_VIE.ToString)
    End Sub

    Private Sub MasterTemplate_CellFormatting(sender As Object, e As CellFormattingEventArgs) Handles RadGridViewEpisode.CellFormatting
        ' --- suppression du carre des checkbox
        Dim checkBoxCell As GridCheckBoxCellElement = TryCast(e.CellElement, GridCheckBoxCellElement)
        If checkBoxCell IsNot Nothing Then
            Dim editor As RadCheckBoxEditor = TryCast(checkBoxCell.Editor, RadCheckBoxEditor)
            Dim element As RadCheckBoxEditorElement = TryCast(editor.EditorElement, RadCheckBoxEditorElement)
            element.Checkmark.Border.Visibility = ElementVisibility.Collapsed
            element.Checkmark.Fill.Visibility = ElementVisibility.Collapsed
        End If
    End Sub

    Private Sub LblParametre4_Click(sender As Object, e As EventArgs) Handles LblParametre4.Click
        Dim nombreParametre As Integer = listeParametreaAfficher.Count()
        If nombreParametre >= 4 Then
            listeParametreaAfficher.RemoveAt(3)
            AfficheParametres()
        End If
    End Sub

    Private Sub LblParametre5_Click(sender As Object, e As EventArgs) Handles LblParametre5.Click
        Dim nombreParametre As Integer = listeParametreaAfficher.Count()
        If nombreParametre >= 5 Then
            listeParametreaAfficher.RemoveAt(4)
            AfficheParametres()
        End If
    End Sub

    Private Sub RadBtnParametreValidation_Click(sender As Object, e As EventArgs) Handles RadBtnParametreValidation.Click
        Dim dateDebut As Date = DteDepuis.Value
        Dim DateFin As Date = DteJusqua.Value
        If dateDebut.Date < DateFin.Date Then
            MessageBox.Show("Sélection incorrecte : La date 'depuis' " & dateDebut.ToString("dd.MM.yyyy") &
                            " ne doit pas être inférieure à la date 'jusqu'à' " & DateFin.ToString("dd.MM.yyyy") & " !")
            Exit Sub
        End If

        'Activité
        If ChkEnfantPreScolaire.Checked = True Then
            ligneDeVie.ActivitePreventionEnfantPreScolaire = True
        Else
            ligneDeVie.ActivitePreventionEnfantPreScolaire = False
        End If

        If ChkEnfantScolaire.Checked = True Then
            ligneDeVie.ActivitePreventionEnfantScolaire = True
        Else
            ligneDeVie.ActivitePreventionEnfantScolaire = False
        End If

        If ChkPathologieAigue.Checked = True Then
            ligneDeVie.ActivitePathologieAigue = True
        Else
            ligneDeVie.ActivitePathologieAigue = False
        End If

        If ChkPreventionAutre.Checked = True Then
            ligneDeVie.ActivitePreventionAutre = True
        Else
            ligneDeVie.ActivitePreventionAutre = False
        End If

        If ChkSocial.Checked = True Then
            ligneDeVie.ActiviteSocial = True
        Else
            ligneDeVie.ActiviteSocial = False
        End If

        If ChkSuiviChronique.Checked = True Then
            ligneDeVie.ActiviteSuiviChronique = True
        Else
            ligneDeVie.ActiviteSuiviChronique = False
        End If

        If ChkSuiviGrossesse.Checked = True Then
            ligneDeVie.ActiviteSuiviGrossesse = True
        Else
            ligneDeVie.ActiviteSuiviGrossesse = False
        End If

        If ChkSuiviGynecologique.Checked = True Then
            ligneDeVie.ActiviteSuiviGyncologique = True
        Else
            ligneDeVie.ActiviteSuiviGyncologique = False
        End If

        'Type
        If ChkTypeConsultation.Checked = True Then
            ligneDeVie.TypeConsultation = True
        Else
            ligneDeVie.TypeConsultation = False
        End If

        If ChkTypeVirtuel.Checked = True Then
            ligneDeVie.TypeVirtuel = True
        Else
            ligneDeVie.TypeVirtuel = False
        End If

        If ChkTypeParametre.Checked = True Then
            ligneDeVie.TypeParametre = True
        Else
            ligneDeVie.TypeParametre = False
        End If

        'Profil
        If ChkProfilMedical.Checked = True Then
            ligneDeVie.ProfilMedical = True
        Else
            ligneDeVie.ProfilMedical = False
        End If

        If ChkProfilParamedical.Checked = True Then
            ligneDeVie.ProfilParamedical = True
        Else
            ligneDeVie.ProfilParamedical = False
        End If

        ChargementEpisode(ligneDeVie)
    End Sub

    Private Sub MasterTemplate_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RadGridViewEpisode.ToolTipTextNeeded
        Dim hoveredCell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If hoveredCell IsNot Nothing Then
            If hoveredCell.ColumnInfo.Name = "type_activite" Then
                e.ToolTipText = hoveredCell.RowInfo.Cells("type_activite").Value & " " & hoveredCell.RowInfo.Cells("description_activite").Value
            Else
                e.ToolTipText = hoveredCell.Value.ToString()
            End If
        End If
    End Sub

    Private Sub RadBtnChart_Click(sender As Object, e As EventArgs) Handles RadBtnChart.Click
        Using Form As New RadFLigneDeVieGraphe
            Form.ShowDialog()
        End Using
    End Sub

    'Sauvegarder la configuration des filtres et des paramètres
    Private Sub RadBtnConfiguration_Click(sender As Object, e As EventArgs) Handles RadBtnConfiguration.Click
        If ChkEnfantPreScolaire.Checked = True Then
            patientParametreLdv.ActivitePreventionEnfantPreScolaire = True
        Else
            patientParametreLdv.ActivitePreventionEnfantPreScolaire = False
        End If

        If ChkEnfantScolaire.Checked = True Then
            patientParametreLdv.ActivitePreventionEnfantScolaire = True
        Else
            patientParametreLdv.ActivitePreventionEnfantScolaire = False
        End If

        If ChkPathologieAigue.Checked = True Then
            patientParametreLdv.ActivitePathologieAigue = True
        Else
            patientParametreLdv.ActivitePathologieAigue = False
        End If

        If ChkPreventionAutre.Checked = True Then
            patientParametreLdv.ActivitePreventionAutre = True
        Else
            patientParametreLdv.ActivitePreventionAutre = False
        End If

        If ChkSocial.Checked = True Then
            patientParametreLdv.ActiviteSocial = True
        Else
            patientParametreLdv.ActiviteSocial = False
        End If

        If ChkSuiviChronique.Checked = True Then
            patientParametreLdv.ActiviteSuiviChronique = True
        Else
            patientParametreLdv.ActiviteSuiviChronique = False
        End If

        If ChkSuiviGrossesse.Checked = True Then
            patientParametreLdv.ActiviteSuiviGrossesse = True
        Else
            patientParametreLdv.ActiviteSuiviGrossesse = False
        End If

        If ChkSuiviGynecologique.Checked = True Then
            patientParametreLdv.ActiviteSuiviGynecologique = True
        Else
            patientParametreLdv.ActiviteSuiviGynecologique = False
        End If

        'Type
        If ChkTypeConsultation.Checked = True Then
            patientParametreLdv.TypeConsultation = True
        Else
            patientParametreLdv.TypeConsultation = False
        End If

        If ChkTypeVirtuel.Checked = True Then
            patientParametreLdv.TypeVirtuel = True
        Else
            patientParametreLdv.TypeVirtuel = False
        End If

        If ChkTypeParametre.Checked = True Then
            patientParametreLdv.TypeParametre = True
        Else
            patientParametreLdv.TypeParametre = False
        End If

        'Profil
        If ChkProfilMedical.Checked = True Then
            patientParametreLdv.ProfilMedical = True
        Else
            patientParametreLdv.ProfilMedical = False
        End If

        If ChkProfilParamedical.Checked = True Then
            patientParametreLdv.ProfilParamedical = True
        Else
            patientParametreLdv.ProfilParamedical = False
        End If

        patientParametreLdv.Parametre1 = Coalesce(Parametre1Id, 0)
        patientParametreLdv.Parametre2 = Coalesce(Parametre2Id, 0)
        patientParametreLdv.Parametre3 = Coalesce(Parametre3Id, 0)
        patientParametreLdv.Parametre4 = Coalesce(Parametre4Id, 0)
        patientParametreLdv.Parametre5 = Coalesce(Parametre5Id, 0)

        If ConfigurationParametreExiste = True Then
            patientParametreLdvDao.UpdateConfigurationParametre(patientParametreLdv, userLog)
            Dim form As New RadFNotification()
            form.Titre = "Notification configuration filtre et paramètre de la ligne de vie"
            form.Message = "Configuration filtre et paramètre modifiée"
            form.Show()
        Else
            ConfigurationParametreExiste = True
            patientParametreLdvDao.CreateConfigurationParametre(patientParametreLdv, userLog)
            Dim form As New RadFNotification()
            form.Titre = "Notification configuration filtre et paramètre de la ligne de vie"
            form.Message = "Configuration filtre et paramètre créée"
            form.Show()
        End If

    End Sub

    'Création épisode de saisie de paramètres
    Private Sub RadBtnCreationEpisodeParametre_Click(sender As Object, e As EventArgs) Handles RadBtnCreationEpisodeParametre.Click
        Using form As New RadFEpisodeParametresCreation
            form.SelectedPatient = SelectedPatient
            form.ShowDialog()
            ChargementEpisode(ligneDeVie)
        End Using
    End Sub

    Private Function FormatParametre(Entier As Integer, Decimale As Integer, Valeur As Decimal) As String
        Dim ValeurString As String = ""
        Select Case Entier
            Case 1
                Select Case Decimale
                    Case 0
                        ValeurString = Valeur.ToString("0")
                    Case 1
                        ValeurString = Valeur.ToString("0.0")
                    Case 2
                        ValeurString = Valeur.ToString("0.00")
                    Case 3
                        ValeurString = Valeur.ToString("0.000")
                End Select
            Case 2
                Select Case Decimale
                    Case 0
                        ValeurString = Valeur.ToString("#0")
                    Case 1
                        ValeurString = Valeur.ToString("#0.0")
                    Case 2
                        ValeurString = Valeur.ToString("#0.00")
                    Case 3
                        ValeurString = Valeur.ToString("#0.000")
                End Select
            Case 3
                Select Case Decimale
                    Case 0
                        ValeurString = Valeur.ToString("##0")
                    Case 1
                        ValeurString = Valeur.ToString("##0.0")
                    Case 2
                        ValeurString = Valeur.ToString("##0.00")
                    Case 3
                        ValeurString = Valeur.ToString("##0.000")
                End Select
        End Select

        Return ValeurString
    End Function
End Class
