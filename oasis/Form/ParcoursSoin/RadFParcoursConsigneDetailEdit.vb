Imports System.Configuration
Imports Oasis_WF
Imports Oasis_Common
Public Class RadFParcoursConsigneDetailEdit
    Private _SelectedPatient As Patient
    Private _UtilisateurConnecte As Utilisateur
    Private _SelectedParcoursId As Integer
    Private _SelectedConsigneId As Integer
    Private _SelectedDrcId As Integer
    Private _CodeRetour As Boolean

    Public Property SelectedPatient As Patient
        Get
            Return _SelectedPatient
        End Get
        Set(value As Patient)
            _SelectedPatient = value
        End Set
    End Property

    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return _UtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            _UtilisateurConnecte = value
        End Set
    End Property

    Public Property SelectedParcoursId As Integer
        Get
            Return _SelectedParcoursId
        End Get
        Set(value As Integer)
            _SelectedParcoursId = value
        End Set
    End Property

    Public Property SelectedConsigneId As Integer
        Get
            Return _SelectedConsigneId
        End Get
        Set(value As Integer)
            _SelectedConsigneId = value
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

    Public Property SelectedDrcId As Integer
        Get
            Return _SelectedDrcId
        End Get
        Set(value As Integer)
            _SelectedDrcId = value
        End Set
    End Property

    Enum EnumEditMode
        Creation = 1
        Modification = 2
    End Enum

    Dim EditMode As Integer
    ReadOnly drcdao As New DrcDao
    ReadOnly parcoursConsigneDao As New ParcoursConsigneDao
    Dim parcoursConsigne As New ParcoursConsigne
    ReadOnly episodeActiviteDao As New EpisodeTypeActiviteDao
    ReadOnly episodeDao As New EpisodeDao

    ReadOnly drc As New Drc
    Dim DateDebut, DateFin As Date
    Dim LimiteAgeEnfantParm As Integer


    Private Sub RadFParcoursConsigneDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementParametreApplication()
        SetCbxActiviteEpisode()
        ChargementEtatCivil()

        If SelectedConsigneId <> 0 Then
            EditMode = EnumEditMode.Modification
            'Modification
            parcoursConsigne = parcoursConsigneDao.GetParcoursConsigneById(SelectedConsigneId)
            TxtCommentaire.Text = parcoursConsigne.Commentaire
            drcdao.GetDrc(drc, parcoursConsigne.DrcId)
            TxtDrcDescription.Text = drc.DrcLibelle
            AlimentationCategorieOasis()

            CbxActiviteEpisode.Text = episodeDao.GetItemTypeActiviteByCode(parcoursConsigne.TypeEpisode)

            Select Case parcoursConsigne.TypeEpisode
                Case Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE
                    LblAgeUnite.Text = "(Age exprimé en mois)"
                    AfficheAge()
                Case Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE
                    LblAgeUnite.Text = "(Age exprimé en année)"
                    AfficheAge()
                Case Else
                    CacheAge()
            End Select

            NumAgeMin.Value = parcoursConsigne.AgeMin
            NumAgeMax.Value = parcoursConsigne.AgeMax

            NumOrdre.Value = parcoursConsigne.Ordre

            If parcoursConsigne.DateDebut <> Nothing Then
                DateDebut = parcoursConsigne.DateDebut
                DteDateDebut.Value = DateDebut
                DteDateDebut.Format = DateTimePickerFormat.Long
            Else
                DteDateDebut.Value = DteDateDebut.MaxDate
                DteDateDebut.Format = DateTimePickerFormat.Custom
                DteDateDebut.CustomFormat = " "
            End If

            If parcoursConsigne.DateFin <> DteDateFin.MaxDate Then
                DateFin = parcoursConsigne.DateFin
                DteDateFin.Value = DateFin
                DteDateFin.Format = DateTimePickerFormat.Long
            Else
                DteDateFin.Value = DteDateDebut.MaxDate
                DteDateFin.Format = DateTimePickerFormat.Custom
                DteDateFin.CustomFormat = " "
            End If
        Else
            'Création
            EditMode = EnumEditMode.Creation
            RadBtnAnnulation.Hide()

            'Initialisation bean
            parcoursConsigne.Id = 0
            parcoursConsigne.ParcoursId = SelectedParcoursId
            parcoursConsigne.PatientId = SelectedPatient.patientId
            parcoursConsigne.DrcId = SelectedDrcId
            parcoursConsigne.Commentaire = ""
            parcoursConsigne.Ordre = 0
            parcoursConsigne.DateDebut = New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
            parcoursConsigne.DateFin = DteDateFin.MaxDate
            parcoursConsigne.Inactif = False
            parcoursConsigne.AgeMin = 0
            parcoursConsigne.AgeMax = 0
            parcoursConsigne.AgeUnite = ""

            'Initialisation des zones de l'écran
            drcdao.GetDrc(drc, SelectedDrcId)
            TxtDrcDescription.Text = drc.DrcLibelle
            AlimentationCategorieOasis()

            DteDateDebut.Value = parcoursConsigne.DateDebut
            DteDateDebut.Format = DateTimePickerFormat.Long
            'Cacher la date de fin l'initialiser à la date virtuelle max
            DteDateFin.Value = DteDateFin.MaxDate
            DteDateFin.Format = DateTimePickerFormat.Custom
            DteDateFin.CustomFormat = " "
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ChargementParametreApplication()
        'Récupération du nom de l'organisation dans les paramètres de l'application
        Dim LimiteAgeEnfantString As String = ConfigurationManager.AppSettings("limiteAgeEnfant")
        If IsNumeric(LimiteAgeEnfantString) Then
            LimiteAgeEnfantParm = CInt(LimiteAgeEnfantString)
        Else
            LimiteAgeEnfantParm = 16
            Dim Description As String = "Paramètre 'LimiteAgeEnfant' non défini dans le fichier App.config"
            CreateLog(Description, Me.Name, Log.EnumTypeLog.ERREUR.ToString, userLog)
        End If
    End Sub

    Private Sub CacheAge()
        LblAgeMin.Hide()
        NumAgeMin.Hide()
        LblAgeMax.Hide()
        NumAgeMax.Hide()
        LblAgeUnite.Hide()
        NumAgeMin.Value = 0
        NumAgeMax.Value = 0
    End Sub

    Private Sub AfficheAge()
        LblAgeMin.Show()
        NumAgeMin.Show()
        LblAgeMax.Show()
        NumAgeMax.Show()
        LblAgeUnite.Show()
    End Sub

    Private Sub SetCbxActiviteEpisode()
        Dim listActivite As List(Of String)
        listActivite = episodeActiviteDao.GetTypeActiviteEpisodeByPatient(SelectedPatient)
        CbxActiviteEpisode.DataSource = listActivite
    End Sub

    Private Sub AlimentationCategorieOasis()
        TxtCategorieOasis.Text = drcdao.GetItemCategorieOasisByCode(drc.CategorieOasisId)
    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)

        LblPatientAdresse1.Text = SelectedPatient.PatientAdresse1
        LblPatientAdresse2.Text = SelectedPatient.PatientAdresse2
        LblPatientCodePostal.Text = SelectedPatient.PatientCodePostal
        LblPatientVille.Text = SelectedPatient.PatientVille
        LblPatientTel1.Text = SelectedPatient.PatientTel1
        LblPatientTel2.Text = SelectedPatient.PatientTel2
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

    Private Sub DteDateFin_DropDown(sender As Object, e As EventArgs) Handles DteDateFin.DropDown
        DteDateFin.Value = DteDateDebut.Value
        DteDateFin.Format = DateTimePickerFormat.Long
    End Sub

    Private Sub DteDateFin_ValueChanged(sender As Object, e As EventArgs) Handles DteDateFin.ValueChanged
        parcoursConsigne.DateFin = DteDateFin.Value
    End Sub

    Private Sub DteDateDebut_ValueChanged(sender As Object, e As EventArgs) Handles DteDateDebut.ValueChanged
        parcoursConsigne.DateDebut = DteDateDebut.Value
    End Sub

    Private Sub TxtCommentaire_TextChanged(sender As Object, e As EventArgs) Handles TxtCommentaire.TextChanged
        parcoursConsigne.Commentaire = TxtCommentaire.Text
    End Sub

    Private Sub NumOrdre_ValueChanged(sender As Object, e As EventArgs) Handles NumOrdre.ValueChanged
        parcoursConsigne.Ordre = NumOrdre.Value
    End Sub

    Private Sub NumAgeMin_ValueChanged(sender As Object, e As EventArgs) Handles NumAgeMin.ValueChanged
        parcoursConsigne.AgeMin = NumAgeMin.Value
    End Sub

    Private Sub NumAgeMax_ValueChanged(sender As Object, e As EventArgs) Handles NumAgeMax.ValueChanged
        parcoursConsigne.AgeMax = NumAgeMax.Value
    End Sub

    Private Sub CbxActiviteEpisode_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxActiviteEpisode.SelectedIndexChanged
        parcoursConsigne.TypeEpisode = episodeDao.GetCodeTypeActiviteByItem(CbxActiviteEpisode.Text)
        Select Case parcoursConsigne.TypeEpisode
            Case Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_PRE_SCOLAIRE
                AfficheAge()
                LblAgeUnite.Text = "(Age exprimé en mois, de 0 à 40 mois)"
                NumAgeMax.Maximum = 40
            Case Episode.EnumTypeActiviteEpisodeCode.PREVENTION_ENFANT_SCOLAIRE
                AfficheAge()
                LblAgeUnite.Text = "(Age exprimé en année, de 0 à " & LimiteAgeEnfantParm & " ans)"
                NumAgeMax.Maximum = LimiteAgeEnfantParm
            Case Else
                CacheAge()
                parcoursConsigne.AgeMin = 0
                parcoursConsigne.AgeMax = 0
        End Select
    End Sub

    'Validation
    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        If ControleDonnees() = True Then
            Select Case EditMode
                Case EnumEditMode.Modification
                    parcoursConsigneDao.ModificationParcoursConsigne(parcoursConsigne)
                    MessageBox.Show("Consigne paramédicale modifiée")
                    Close()
                Case EnumEditMode.Creation
                    parcoursConsigneDao.CreateParcoursConsigne(parcoursConsigne)
                    MessageBox.Show("Consigne paramédicale créée")
                    Close()
            End Select
        End If
    End Sub

    'Annulation consigne paramédicale
    Private Sub RadBtnAnnulation_Click(sender As Object, e As EventArgs) Handles RadBtnAnnulation.Click
        If MsgBox("Confirmation de l'annulation ", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            parcoursConsigneDao.AnnulationParcoursConsigne(parcoursConsigne)
            MessageBox.Show("Consigne paramédicale annulée")
            Close()
        End If
    End Sub

    Private Function ControleDonnees() As Boolean
        'Contrôle des données saisie
        Dim Valide As Boolean = True
        Dim MessageErreur1 As String = ""
        Dim MessageErreur2 As String = ""
        Dim MessageErreur3 As String = ""
        Dim MessageErreur4 As String = ""
        Dim MessageErreur As String = ""

        parcoursConsigne.TypeEpisode = episodeDao.GetCodeTypeActiviteByItem(CbxActiviteEpisode.Text)

        If DteDateDebut.Value = Nothing Then
            Valide = False
            MessageErreur1 = "La saisie de la date de début est obligatoire"
        End If

        If DteDateFin.Value <> DteDateFin.MaxDate Then
            If DteDateFin.Value.Date <= DteDateDebut.Value.Date Then
                Valide = False
                MessageErreur2 = "La date de fin doit être supérieure à la date de début"
            End If
        End If

        If CbxActiviteEpisode.Text = "" Then
            Valide = False
            MessageErreur3 = "La saisie du type d'épisode est obligatoire"
        End If

        If NumAgeMin.Value > NumAgeMax.Value Then
            Valide = False
            MessageErreur4 = "L'âge min doit être inférieur à l'âge max"
        End If

        'Préparation de l'affichage des erreurs
        If Valide = False Then
            If MessageErreur1 <> "" Then
                MessageErreur = MessageErreur & MessageErreur1 & vbCrLf
            End If

            If MessageErreur2 <> "" Then
                MessageErreur = MessageErreur & MessageErreur2 & vbCrLf
            End If

            If MessageErreur3 <> "" Then
                MessageErreur = MessageErreur & MessageErreur3 & vbCrLf
            End If

            If MessageErreur4 <> "" Then
                MessageErreur = MessageErreur & MessageErreur4 & vbCrLf
            End If

            MessageErreur = MessageErreur & vbCrLf & "/!\ données incorrectes"
            MessageBox.Show(MessageErreur)
        End If

        Return Valide
    End Function

    Private Sub RadBtnDRCDetail_Click(sender As Object, e As EventArgs) Handles RadBtnDRCDetail.Click
        Cursor.Current = Cursors.WaitCursor
        Using vRadFDrcDetailEdit As New RadFDrcDetailEdit
            vRadFDrcDetailEdit.SelectedDRCId = parcoursConsigne.DrcId
            vRadFDrcDetailEdit.UtilisateurConnecte = userLog
            vRadFDrcDetailEdit.ShowDialog()
        End Using
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

End Class
