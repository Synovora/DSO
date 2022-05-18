Imports Oasis_WF
Imports Oasis_Common
Public Class RadFParcoursHistoListe
    Private _SelectedPatient As Patient
    Private _UtilisateurConnecte As Utilisateur
    Private _SelectedParcoursId As Integer

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

    Dim UtilisateurHisto As New Utilisateur

    Private Sub RadFParcoursHistoListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitZones()
        ChargementEtatCivil()
        ChargementParcours()
    End Sub

    'Chargement des données dans les labels dédiés
    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = Me.SelectedPatient.PatientNir
        LblPatientPrenom.Text = Me.SelectedPatient.PatientPrenom
        LblPatientNom.Text = Me.SelectedPatient.PatientNom
        LblPatientAge.Text = Me.SelectedPatient.PatientAge
        LblPatientGenre.Text = Me.SelectedPatient.PatientGenre
        LblPatientAdresse1.Text = Me.SelectedPatient.PatientAdresse1
        LblPatientAdresse2.Text = Me.SelectedPatient.PatientAdresse2
        LblPatientCodePostal.Text = Me.SelectedPatient.PatientCodePostal
        LblPatientVille.Text = Me.SelectedPatient.PatientVille
        LblPatientTel1.Text = Me.SelectedPatient.PatientTel1
        LblPatientTel2.Text = Me.SelectedPatient.PatientTel2
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(Me.SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(Me.SelectedPatient.PatientUniteSanitaireId)
        LblPatientDateMaj.Text = Me.SelectedPatient.PatientSyntheseDateMaj.ToString("dd/MM/yyyy")

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    'Chargement de la Grid
    Private Sub ChargementParcours()
        Dim parcoursHistoDataTable As DataTable = New DataTable()
        Dim parcoursHistoDao As New ParcoursHistoDao
        parcoursHistoDataTable = parcoursHistoDao.getAllParcoursHistobyParcoursId(SelectedParcoursId)

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = parcoursHistoDataTable.Rows.Count - 1
        Dim natureHisto, specialiteId, intervenantId As Integer
        Dim ActionHistoString, specialiteString, intervenantString As String
        Dim dateHisto As DateTime
        Dim Inactif, Cacher As Boolean

        'Initialisation des variables de comparaison
        Dim CacherComp, InactifComp As Boolean
        Dim CommentaireComp, baseComp, IntervenantComp As String
        Dim RythmeComp As Integer

        CommentaireComp = ""
        IntervenantComp = ""
        baseComp = ""
        RythmeComp = 0
        InactifComp = False
        CacherComp = False

        Dim premierPassage As Boolean = True
        specialiteString = ""
        intervenantString = ""

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            'Nature historisation
            natureHisto = parcoursHistoDataTable.Rows(i)("oa_parcours_histo_etat")
            Select Case natureHisto
                Case ParcoursHistoDao.EnumEtatParcoursHisto.Creation
                    ActionHistoString = "Creation parcours"
                Case ParcoursHistoDao.EnumEtatParcoursHisto.Modification
                    ActionHistoString = "Modification parcours"
                Case ParcoursHistoDao.EnumEtatParcoursHisto.Annulation
                    ActionHistoString = "Annulation parcours"
                Case Else
                    ActionHistoString = "Action inconnue"
            End Select

            If i = 0 Then
                specialiteId = Coalesce(parcoursHistoDataTable.Rows(i)("oa_parcours_specialite"), 0)
                If specialiteId <> 0 Then
                    specialiteString = Table_specialite.GetSpecialiteDescription(specialiteId)
                End If
            End If

            intervenantId = Coalesce(parcoursHistoDataTable.Rows(i)("oa_parcours_ror_id"), 0)
            If intervenantId <> 0 Then
                Dim rordao As New RorDao
                Dim ror As Ror
                ror = rordao.GetRorById(intervenantId)
                intervenantString = ror.Nom
            End If

            'Alimentation de la >Grid
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadParcoursDataGridView.Rows.Add(iGrid)

            '------------------- Alimentation du DataGridView
            RadParcoursDataGridView.Rows(iGrid).Cells("parcoursId").Value = parcoursHistoDataTable.Rows(i)("oa_parcours_id")

            'Date historisation
            dateHisto = parcoursHistoDataTable.Rows(i)("oa_parcours_histo_date_historisation")
            RadParcoursDataGridView.Rows(iGrid).Cells("histoDate").Value = dateHisto.ToString("dd.MM.yyyy HH:mm:ss")

            'Utilisateur
            Dim UtilisateurId As Integer = parcoursHistoDataTable.Rows(i)("oa_parcours_histo_user_historisation")
            Dim userDao As New UserDao
            UtilisateurHisto = userDao.GetUserById(UtilisateurId)
            'SetUtilisateur(UtilisateurHisto, UtilisateurId)
            RadParcoursDataGridView.Rows(iGrid).Cells("histoUtilisateur").Value = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom

            'Action
            RadParcoursDataGridView.Rows(iGrid).Cells("histoAction").Value = ActionHistoString

            'Spécialité et intervenant
            RadParcoursDataGridView.Rows(iGrid).Cells("specialite").Value = specialiteString

            RadParcoursDataGridView.Rows(iGrid).Cells("intervenant").Value = intervenantString
            If intervenantString <> IntervenantComp And premierPassage = False Then
                RadParcoursDataGridView.Rows(iGrid).Cells("intervenant").Style.ForeColor = Color.Red
            End If
            IntervenantComp = intervenantString

            'Commentaire
            RadParcoursDataGridView.Rows(iGrid).Cells("commentaire").Value = parcoursHistoDataTable.Rows(i)("oa_parcours_commentaire")
            If parcoursHistoDataTable.Rows(i)("oa_parcours_commentaire").ToString <> CommentaireComp And premierPassage = False Then
                RadParcoursDataGridView.Rows(iGrid).Cells("commentaire").Style.ForeColor = Color.Red
            End If
            CommentaireComp = parcoursHistoDataTable.Rows(i)("oa_parcours_commentaire")

            'Base
            RadParcoursDataGridView.Rows(iGrid).Cells("base").Value = parcoursHistoDataTable.Rows(i)("oa_parcours_base")
            If parcoursHistoDataTable.Rows(i)("oa_parcours_base") <> baseComp And premierPassage = False Then
                RadParcoursDataGridView.Rows(iGrid).Cells("base").Style.ForeColor = Color.Red
            End If
            baseComp = parcoursHistoDataTable.Rows(i)("oa_parcours_base")

            'Rythme
            RadParcoursDataGridView.Rows(iGrid).Cells("rythme").Value = parcoursHistoDataTable.Rows(i)("oa_parcours_rythme")
            If parcoursHistoDataTable.Rows(i)("oa_parcours_rythme") <> RythmeComp And premierPassage = False Then
                RadParcoursDataGridView.Rows(iGrid).Cells("rythme").Style.ForeColor = Color.Red
            End If
            RythmeComp = parcoursHistoDataTable.Rows(i)("oa_parcours_rythme")

            'Caché
            Cacher = Coalesce(parcoursHistoDataTable.Rows(i)("oa_parcours_cacher"), False)
            If Cacher = True Then
                RadParcoursDataGridView.Rows(iGrid).Cells("masque").Value = "Oui"
            Else
                RadParcoursDataGridView.Rows(iGrid).Cells("masque").Value = "Non"
            End If
            If Cacher <> CacherComp And premierPassage = False Then
                RadParcoursDataGridView.Rows(iGrid).Cells("masque").Style.ForeColor = Color.Red
            End If
            CacherComp = Cacher

            'Inactif
            Inactif = Coalesce(parcoursHistoDataTable.Rows(i)("oa_parcours_inactif"), False)
            If Inactif = True Then
                RadParcoursDataGridView.Rows(iGrid).Cells("inactif").Value = "Supprimé"
            Else
                RadParcoursDataGridView.Rows(iGrid).Cells("inactif").Value = "Actif"
            End If
            If Inactif <> InactifComp And premierPassage = False Then
                RadParcoursDataGridView.Rows(iGrid).Cells("inactif").Style.ForeColor = Color.Red
            End If
            InactifComp = Inactif

            premierPassage = False
        Next

        'Positionnement du grid sur la première occurrence
        If RadParcoursDataGridView.Rows.Count > 0 Then
            Me.RadParcoursDataGridView.CurrentRow = RadParcoursDataGridView.ChildRows(0)
        End If
    End Sub

    Private Sub InitZones()

    End Sub

    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Close()
    End Sub

End Class
