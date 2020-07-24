Imports System.Data.SqlClient
Imports Telerik.WinControls.UI.Localization
Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class RadFPatientDetailEdit
    Private privateSelectedPatientId As Integer
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateCodeRetour As Boolean
    Private _Action As String

    Public Property SelectedPatientId As Integer
        Get
            Return privateSelectedPatientId
        End Get
        Set(value As Integer)
            privateSelectedPatientId = value
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

    Public Property CodeRetour As Boolean
        Get
            Return privateCodeRetour
        End Get
        Set(value As Boolean)
            privateCodeRetour = value
        End Set
    End Property

    Public Property SelectedPatient As Patient
        Get
            Return privateSelectedPatient
        End Get
        Set(value As Patient)
            privateSelectedPatient = value
        End Set
    End Property

    Public Property Action As String
        Get
            Return _Action
        End Get
        Set(value As String)
            _Action = value
        End Set
    End Property

    Enum EnumEditMode
        Creation = 1
        Modification = 2
    End Enum

    Dim EditMode As Integer

    Dim patientUpdate As New Patient
    Dim patientRead As New Patient

    ReadOnly rorDao As New RorDao
    ReadOnly patientDao As New PatientDao

    Dim utilisateurHisto As Utilisateur = New Utilisateur()
    Dim ror As Ror
    Dim PharmacienRorId As Integer = 0

    ReadOnly uniteSanitaireListe As Dictionary(Of Integer, String) = Table_unite_sanitaire.GetUniteSanitaireListe()
    ReadOnly genreListe As Dictionary(Of String, String) = Table_genre.GetGenreListe()

    Private Sub RadFPatientDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        RadGridLocalizationProvider.CurrentProvider = New FrenchRadGridViewLocalizationProvider()

        InitZone()
        InitAction()
        If SelectedPatientId <> 0 Then
            'Modification
            EditMode = EnumEditMode.Modification
            RadBtnValidationDateNaissance.Hide()
            ChargementPatient()
            ChargementNotesPatient()
            InhiberZoneEnSaisie()
        Else
            'Création
            InhiberZoneEnSaisie()
            EditMode = EnumEditMode.Creation
            DteDateNaissance.Enabled = True
            LblIdentifiantOasis.Hide()
            LblLabelIdOasis.Hide()
            RadBtnModifier.Hide()
            RadBtnSortieOasis.Hide()
            RadBtnRDV.Hide()
            RadBtnValider.Show()
            RadBtnValider.Enabled = True
            RadBtnAnnulerAction.Hide()
            RadNotePatientDataGridView.Hide()
            NoteContextMenuStrip.Enabled = False
            Me.Width = 590
        End If
    End Sub


    Private Sub InitZone()
        LblHorsOasis.Hide()
        TxtPrenom.Text = ""
        TxtNom.Text = ""
        DteDateNaissance.Value = DteDateNaissance.MinDate
        DteDateNaissance.Format = DateTimePickerFormat.Custom
        DteDateNaissance.CustomFormat = " "
        LblAge.Text = ""
        CbxGenre.SelectedItem = vbNull
        TxtNIR.Text = ""
        TxtAdresse1.Text = ""
        TxtAdresse2.Text = ""
        TxtCodePostal.Text = ""
        TxtVille.Text = ""
        TxtTelFixe.Text = ""
        TxtTelMobile.Text = ""
        TxtEmail.Text = ""
        TxtProfession.Text = ""
        PharmacienRorId = 0
        DteDateEntree.Value = DteDateEntree.MaxDate
        DteDateEntree.Format = DateTimePickerFormat.Custom
        DteDateEntree.CustomFormat = " "
        CbxSite.SelectedItem = vbNull
        CbxUniteSanitaire.SelectedItem = vbNull
        ChkCouvertureInternet.Checked = False
        DteDateSortie.Value = DteDateSortie.MaxDate
        DteDateSortie.Format = DateTimePickerFormat.Custom
        DteDateSortie.CustomFormat = " "
        DteDateDeces.Value = DteDateDeces.MaxDate
        DteDateDeces.Format = DateTimePickerFormat.Custom
        DteDateDeces.CustomFormat = " "
        TxtCommentaireSortie.Text = ""
        GbxSortieOasis.Hide()

        'genre
        Dim indice As Integer = genreListe.Count - 1
        Dim genreDescription(indice) As String
        Dim i As Integer = 0

        For Each kvp As KeyValuePair(Of String, String) In genreListe
            genreDescription(i) = kvp.Value
            i += 1
        Next kvp
        CbxGenre.DataSource = genreDescription

        'Unité sanitaire
        indice = uniteSanitaireListe.Count - 1
        Dim uniteSanitaireDescription(indice) As String
        i = 0

        For Each kvp As KeyValuePair(Of Integer, String) In uniteSanitaireListe
            uniteSanitaireDescription(i) = kvp.Value
            i += 1
        Next kvp
        CbxUniteSanitaire.DataSource = uniteSanitaireDescription
    End Sub

    Private Sub InitAction()
        RadBtnValider.Enabled = False
        RadBtnSortieOasis.Enabled = True
        RadBtnAnnulerAction.Hide()

        'Droits personnel non Médical, non Paramédical, non Accueil paramédical
        If Not (userLog.TypeProfil = ProfilDao.EnumProfilType.MEDICAL.ToString OrElse
            userLog.TypeProfil = ProfilDao.EnumProfilType.PARAMEDICAL.ToString OrElse
            userLog.TypeProfil = ProfilDao.EnumProfilType.ACCUEIL.ToString) Then
            RadBtnModifier.Hide()
            RadBtnPharmacien.Hide()
            RadBtnRDV.Hide()
            RadBtnValidationDateNaissance.Hide()
            RadBtnSortieOasis.Hide()
            RadBtnValider.Hide()
            BtnCreerNote.Hide()
        End If
    End Sub

    'Chargement des données du patient
    Private Sub ChargementPatient()
        patientRead = patientDao.GetPatientById(SelectedPatientId)
        patientUpdate = patientDao.ClonePatient(patientRead)

        LblIdentifiantOasis.Text = patientUpdate.patientId
        TxtPrenom.Text = patientUpdate.PatientPrenom
        TxtNom.Text = patientUpdate.PatientNom

        If patientUpdate.PatientDateNaissance < DteDateNaissance.MinDate Or patientUpdate.PatientDateNaissance > DteDateNaissance.MaxDate Then
            DteDateNaissance.Value = DteDateNaissance.MinDate
        Else
            DteDateNaissance.Value = patientUpdate.PatientDateNaissance
            DteDateNaissance.Format = DateTimePickerFormat.Long
        End If

        If patientUpdate.PatientGenreId = "M" Then
            TxtNomMarital.Text = ""
            TxtNomMarital.Hide()
        End If

        LblAge.Text = patientUpdate.PatientAge
        CbxGenre.SelectedItem = patientUpdate.PatientGenre

        TxtNIR.Text = patientUpdate.PatientNir
        TxtINS.Text = patientUpdate.INS

        TxtAdresse1.Text = patientUpdate.PatientAdresse1
        TxtAdresse2.Text = patientUpdate.PatientAdresse2
        TxtCodePostal.Text = patientUpdate.PatientCodePostal
        TxtVille.Text = patientUpdate.PatientVille

        TxtTelFixe.Text = patientUpdate.PatientTel1

        TxtTelMobile.Text = patientUpdate.PatientTel2
        TxtEmail.Text = patientUpdate.PatientEmail

        TxtProfession.Text = patientUpdate.Profession
        PharmacienRorId = patientUpdate.PharmacienId
        If patientUpdate.PharmacienId <> 0 Then
            ror = rorDao.getRorById(patientUpdate.PharmacienId)
            TxtPharmacien.Text = ror.Nom
        End If

        Dim PatientgSiteDescription As String = Table_site.GetSiteDescription(patientUpdate.PatientSiteId)
        CbxSite.SelectedItem = PatientgSiteDescription

        Dim PatientgUniteSanitaireDescription As String = Table_unite_sanitaire.GetUniteSanitaireDescription(patientUpdate.PatientUniteSanitaireId)
        CbxUniteSanitaire.SelectedItem = PatientgUniteSanitaireDescription

        If patientUpdate.PatientInternet = True Then
            ChkCouvertureInternet.Checked = True
        Else
            ChkCouvertureInternet.Checked = False
        End If

        'Date entrée
        If patientUpdate.PatientDateEntree > DteDateEntree.MaxDate Then
            DteDateEntree.Value = DteDateEntree.MaxDate
        Else
            If patientUpdate.PatientDateEntree < DteDateEntree.MinDate Then
                DteDateEntree.Value = DteDateEntree.MaxDate
            Else
                DteDateEntree.Value = patientUpdate.PatientDateEntree
            End If
        End If
        If DteDateEntree.Value <> DteDateEntree.MaxDate Then
            DteDateEntree.Format = DateTimePickerFormat.Long
        Else
            'Si le patient n'a pas de date d'entrée dans Oasis, on n'affiche pas le bouton de sortie
            RadBtnSortieOasis.Hide()
            LblHorsOasis.Show()
            LblHorsOasis.Text = "Attention, ce patient ne fait pas partie du dispositif Oasis"
        End If

        'Date sortie
        DteDateSortie.Value = Coalesce(patientUpdate.PatientDateSortie, DteDateSortie.MaxDate)

        If DteDateSortie.Value <> DteDateSortie.MaxDate Then
            DteDateSortie.Format = DateTimePickerFormat.Long
            '-- Si le patient est sorti d'Oasis, on n'affiche pas le bouton de validation de la déclaration de sortie
            RadBtnValidationSortie.Hide()
            GbxSortieOasis.Show()
            RadBtnSortieOasis.Hide()
            LblHorsOasis.Show()
            LblHorsOasis.Text = "Attention, ce patient est sorti du dispositif Oasis"
        End If

        'Date décès
        If patientUpdate.PatientDateDeces < DteDateDeces.MinDate Or patientUpdate.PatientDateDeces > DteDateDeces.MaxDate Then
            DteDateDeces.Value = DteDateDeces.MaxDate
        Else
            DteDateDeces.Value = patientUpdate.PatientDateDeces
        End If
        If DteDateDeces.Value <> DteDateDeces.MaxDate Then
            DteDateDeces.Format = DateTimePickerFormat.Long
            LblHorsOasis.Show()
            LblHorsOasis.Text = "Patient décédé"
        End If

        TxtCommentaireSortie.Text = patientUpdate.PatientCommentaireSortie

        If patientUpdate.PatientUniteSanitaireId <> 0 Then
            'Unité sanitaire id
            For Each kvp As KeyValuePair(Of Integer, String) In uniteSanitaireListe
                If kvp.Key = patientUpdate.PatientUniteSanitaireId Then
                    CbxUniteSanitaire.SelectedItem = kvp.Value
                    Exit For
                End If
            Next kvp

            'Chargement Combo site par rapport à l'unité sanitaire choisie
            Dim siteListeParUniteSanitaire As Dictionary(Of Integer, String) = Table_site.GetSiteListeParUniteSanitaire(patientUpdate.PatientUniteSanitaireId)
            Dim indice As Integer = siteListeParUniteSanitaire.Count - 1
            Dim siteDescription(indice) As String
            Dim i As Integer = 0

            For Each kvp As KeyValuePair(Of Integer, String) In siteListeParUniteSanitaire
                siteDescription(i) = kvp.Value
                i += 1
            Next kvp

            CbxSite.DataSource = siteDescription

            If patientUpdate.PatientSiteId <> 0 Then
                'Unité sanitaire id
                For Each kvp As KeyValuePair(Of Integer, String) In siteListeParUniteSanitaire
                    If kvp.Key = patientUpdate.PatientSiteId Then
                        CbxSite.SelectedItem = kvp.Value
                        Exit For
                    End If
                Next kvp
            End If
        End If

    End Sub


    '====================================================================================================================================
    '-- Gestion de la mise à jour du patient
    '====================================================================================================================================

    'Appel de la modification des informations du patient
    Private Sub RadBtnModifier_Click(sender As Object, e As EventArgs) Handles RadBtnModifier.Click
        ActiverZoneEnSaisie()
        RadBtnSortieOasis.Enabled = False
        RadBtnModifier.Enabled = False
        RadBtnAnnulerAction.Show()
        '-- Si le patient est sorti d'Oasis, on ne peut pas modifier les éléments de sortie
        If patientUpdate.PatientDateSortie <> DteDateSortie.MaxDate Then
            DteDateSortie.Enabled = False
            TxtCommentaireSortie.Enabled = False
        End If
        GestionAffichageBoutonValidation()
    End Sub

    'Annulation de l'appel de modification ou de sortie
    Private Sub RadBtnAnnulerAction_Click(sender As Object, e As EventArgs) Handles RadBtnAnnulerAction.Click
        ChargementPatient()
        InhiberZoneEnSaisie()
        RadBtnAnnulerAction.Hide()
        RadBtnModifier.Enabled = True
        RadBtnSortieOasis.Show()
        GbxSortieOasis.Hide()
        RadBtnSortieOasis.Enabled = True

        '-- Si le patient est sorti d'Oasis, on n'affiche pas le bouton de validation de la déclaration de sortie
        If patientUpdate.PatientDateSortie <> DteDateSortie.MaxDate Then
            DteDateSortie.Format = DateTimePickerFormat.Long
            RadBtnValidationSortie.Hide()
            GbxSortieOasis.Show()
            RadBtnSortieOasis.Hide()
        End If

        '-- Si le patient n'a pas de date d'entrée, on n'affiche pas le bouton de sortie d'Oasis
        If patientUpdate.PatientDateEntree = DteDateEntree.MaxDate Then
            RadBtnSortieOasis.Hide()
        End If
    End Sub

    'Validation des modifications des données ou de la création d'un nouveau patient
    Private Sub RadBtnValider_Click(sender As Object, e As EventArgs) Handles RadBtnValider.Click
        If ValidationDonnéesSaisies() = True Then
            Select Case EditMode
                Case EnumEditMode.Creation
                    If DteDateEntree.Value = DteDateEntree.MaxDate Then
                        If MsgBox("Attention, la date d'entrée dans le disposition Oasis n'est pas renseignée." & vbCrLf &
                                  "Sans date d'entrée, ce nouveau patient ne fera pas partie du dispositif Oasis et sera accessible uniquement avec l'option 'Tous' dans la liste des patients." & vbCrLf &
                                  "Confirmez-vous la création de ce patient sans date d'entrée dans le dispositif Oasis", MsgBoxStyle.YesNo, "") = MsgBoxResult.No Then
                            Exit Sub
                        End If
                    End If
                    Me.CodeRetour = CreationPatient()
                Case EnumEditMode.Modification
                    Me.CodeRetour = ModificationPatient()
            End Select
            If Me.CodeRetour = True Then
                Close()
            End If
        End If
    End Sub

    Private Function ValidationDonnéesSaisies() As Boolean
        Dim Valide As Boolean = True
        Dim zoneObligatoire As Boolean = True
        Dim zoneNumerique As Boolean = True
        Dim messageErreur As String = ""
        Dim messageErreur1 As String = ""
        Dim messageErreur2 As String = ""
        Dim messageErreur3 As String = ""
        Dim messageErreur4 As String = ""
        Dim messageErreur5 As String = ""
        Dim messageErreur6 As String = ""

        'Nom, Prenom, date naissance, genre, adresse 1, code postal et ville obligatoire
        If TxtPrenom.Text = "" Then
            zoneObligatoire = False
        End If
        If TxtNom.Text = "" Then
            zoneObligatoire = False
        End If
        If CbxGenre.SelectedValue = "" Then
            zoneObligatoire = False
        End If
        If DteDateNaissance.Value = DteDateNaissance.MinDate Then
            zoneObligatoire = False
        End If
        If zoneObligatoire = False Then
            messageErreur1 = "- Les zones : prénom, nom, date de naissance et genre sont obligatoires"
            Valide = False
        End If

        'Les zones : NIR, INS, téléphone fixe, téléphone mobile et code postal doivent être numériques si elles sont saisies
        If TxtNIR.Text.Trim <> "" Then
            If Not IsNumeric(TxtNIR.Text) Then
                zoneNumerique = False
            End If
        End If

        If TxtINS.Text.Trim <> "" Then
            If Not IsNumeric(TxtINS.Text) Then
                zoneNumerique = False
            End If
        End If

        If TxtTelFixe.Text.Trim <> "" Then
            If Not IsNumeric(TxtTelFixe.Text) Then
                zoneNumerique = False
            End If
        End If

        If TxtTelMobile.Text.Trim <> "" Then
            If Not IsNumeric(TxtTelMobile.Text) Then
                zoneNumerique = False
            End If
        End If

        If TxtCodePostal.Text.Trim <> "" Then
            If Not IsNumeric(TxtCodePostal.Text) Then
                zoneNumerique = False
            End If
        End If

        If zoneNumerique = False Then
            messageErreur2 = "- Les zones : NIR, INS, Téléphone fixe, téléphone mobile et code postal doivent être numériques si elles sont saisies"
            Valide = False
        End If

        'Contrôle existence NIR
        If EditMode = EnumEditMode.Creation Then
            'Contrôle de l'existence du NIR
            If IsNumeric(TxtNIR.Text) Then
                Dim NirPatient As Int64
                NirPatient = TxtNIR.Text
                If patientDao.NonExistencePatientNIR(NirPatient, 0) = False Then
                    messageErreur3 = "- Le NIR saisie existe déjà pour un autre patient défini dans le référentiel d'Oasis, création impossible"
                    Valide = False
                End If
            End If
        End If

        If EditMode = EnumEditMode.Modification Then
            'Contrôle de l'existence du NIR
            If IsNumeric(TxtNIR.Text) Then
                If TxtNIR.Text <> "0" Then
                    Dim NirPatient As Int64
                    NirPatient = TxtNIR.Text
                    If patientDao.NonExistencePatientNIR(NirPatient, SelectedPatientId) = False Then
                        messageErreur3 = "- Le NIR saisie existe déjà pour un autre patient défini dans le référentiel d'Oasis, modification impossible"
                        Valide = False
                    End If
                End If
            End If
        End If

        'Date naissance
        If DteDateNaissance.Value.Date > Date.Now.Date Then
            messageErreur4 = "- La date de naissance ne doit pas être supérieure à la date du jour"
            Valide = False
        End If

        'Date décès
        If DteDateDeces.Value <> DteDateDeces.MaxDate Then
            If DteDateDeces.Value.Date > Date.Now.Date Then
                messageErreur5 = "- La date de décès ne doit pas être supérieure à la date du jour"
                Valide = False
            End If
        End If

        'TODO: PatientDetail -> Contrôle existence INS


        'Préparation de l'affichage des erreurs
        If Valide = False Then
            If messageErreur1 <> "" Then
                messageErreur = messageErreur1
            End If

            If messageErreur2 <> "" Then
                messageErreur = messageErreur + vbCrLf + messageErreur2
            End If

            If messageErreur3 <> "" Then
                messageErreur = messageErreur + vbCrLf + messageErreur3
            End If

            If messageErreur4 <> "" Then
                messageErreur = messageErreur + vbCrLf + messageErreur4
            End If

            If messageErreur5 <> "" Then
                messageErreur = messageErreur + vbCrLf + messageErreur5
            End If

            If messageErreur6 <> "" Then
                messageErreur = messageErreur + vbCrLf + messageErreur6
            End If

            messageErreur = messageErreur + vbCrLf + vbCrLf + "/!\ Validation impossible, des données sont incorrectes"

            MessageBox.Show(messageErreur)
        End If

        Return Valide
    End Function

    'Modification du patient
    Private Function ModificationPatient() As Boolean
        Dim codeRetour As Boolean = False

        If patientDao.ModificationPatient(patientUpdate, userLog) = True Then
            codeRetour = True
            Dim form As New RadFNotification With {
                .Message = "Patient modifié"
            }
            form.Show()
        End If

        Return codeRetour
    End Function

    'Création du patient
    Private Function CreationPatient() As Boolean
        Dim codeRetour As Boolean = False

        If patientDao.CreationPatient(patientUpdate, userLog) = True Then
            codeRetour = True
            Dim form As New RadFNotification With {
                .Message = "Patient créé"
            }
            form.Show()
        End If

        Return codeRetour
    End Function


    '====================================================================================================================================
    '-- Gestion de la sortie d'Oasis du patient
    '====================================================================================================================================

    'Appel de la saisie des informations de sortie Oasis
    Private Sub RadBtnSortieOasis_Click(sender As Object, e As EventArgs) Handles RadBtnSortieOasis.Click
        RadBtnSortieOasis.Hide()
        GbxSortieOasis.Show()
        InhiberZoneEnSaisie()
        RadBtnModifier.Enabled = False
        BtnCreerNote.Enabled = False
        DteDateSortie.Enabled = True
        DteDateSortie.Value = Date.Now
        DteDateSortie.Format = DateTimePickerFormat.Long
        TxtCommentaireSortie.Enabled = True
        RadBtnAnnulerAction.Show()
    End Sub

    'Validation sortie Oasis
    Private Sub RadBtnValidationSortie_Click(sender As Object, e As EventArgs) Handles RadBtnValidationSortie.Click
        If MsgBox("confirmation de la déclaration de sortie du patient", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            If DeclarationSortie() = True Then
                Me.CodeRetour = True
                Close()
            End If
        End If
    End Sub

    'Déclaration de sortie du patient
    Private Function DeclarationSortie() As Boolean
        Dim codeRetour As Boolean = False

        If patientDao.DeclarationSortie(patientUpdate) = True Then
            codeRetour = True
            Dim form As New RadFNotification With {
                .Message = "Déclaration de sortie du patient effectuée"
            }
            form.Show()
        End If

        Return codeRetour
    End Function


    '====================================================================================================================================
    '-- Gestion des modifications de zone
    '====================================================================================================================================
    Private Sub DteDateNaissance_ValueChanged(sender As Object, e As EventArgs) Handles DteDateNaissance.ValueChanged
        patientUpdate.PatientDateNaissance = DteDateNaissance.Value
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtPrenom_TextChanged(sender As Object, e As EventArgs) Handles TxtPrenom.TextChanged
        patientUpdate.PatientPrenom = TxtPrenom.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtNom_TextChanged(sender As Object, e As EventArgs) Handles TxtNom.TextChanged
        patientUpdate.PatientNom = TxtNom.Text
        GestionAffichageBoutonValidation()
    End Sub

    'Modification du genre par l'utilisateur
    Private Sub CbxGenre_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxGenre.SelectedIndexChanged
        patientUpdate.PatientGenre = CbxGenre.SelectedValue
        GestionAffichageBoutonValidation()
        If CbxGenre.SelectedValue = "Masculin" Then
            patientUpdate.PatientGenreId = patientDao.EnumGenreId.Masculin
            TxtNomMarital.Text = ""
            TxtNomMarital.Hide()
            LblNomMarital.Hide()
        Else
            patientUpdate.PatientGenreId = patientDao.EnumGenreId.Feminin
            TxtNomMarital.Show()
            LblNomMarital.Show()
        End If
    End Sub

    Private Sub TxtNomMarital_TextChanged(sender As Object, e As EventArgs) Handles TxtNomMarital.TextChanged
        patientUpdate.PatientNomMarital = TxtNomMarital.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtNIR_TextChanged(sender As Object, e As EventArgs) Handles TxtNIR.TextChanged
        If IsNumeric(TxtNIR.Text) Then
            Dim NIR As Int64 = CDec(TxtNIR.Text)
            Dim Modulo As Integer
            Modulo = CalculmoduloNIR(NIR)
            LblModulo.Text = Modulo
            LblModulo.Show()
            patientUpdate.PatientNir = NIR
            GestionAffichageBoutonValidation()
        Else
            LblModulo.Hide()
            patientUpdate.PatientNir = 0
        End If
    End Sub

    Private Sub TxtINS_TextChanged(sender As Object, e As EventArgs) Handles TxtINS.TextChanged
        If IsNumeric(TxtINS.Text) Then
            Dim INS As Int64 = CDec(TxtINS.Text)
            patientUpdate.INS = INS
            GestionAffichageBoutonValidation()
        Else
            patientUpdate.INS = 0
        End If
    End Sub

    Private Sub TxtAdresse1_TextChanged(sender As Object, e As EventArgs) Handles TxtAdresse1.TextChanged
        patientUpdate.PatientAdresse1 = TxtAdresse1.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtAdresse2_TextChanged(sender As Object, e As EventArgs) Handles TxtAdresse2.TextChanged
        patientUpdate.PatientAdresse2 = TxtAdresse2.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtCodePostal_TextChanged(sender As Object, e As EventArgs) Handles TxtCodePostal.TextChanged
        patientUpdate.PatientCodePostal = TxtCodePostal.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtVille_TextChanged(sender As Object, e As EventArgs) Handles TxtVille.TextChanged
        patientUpdate.PatientVille = TxtVille.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtTelFixe_TextChanged(sender As Object, e As EventArgs) Handles TxtTelFixe.TextChanged
        patientUpdate.PatientTel1 = TxtTelFixe.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtTelMobile_TextChanged(sender As Object, e As EventArgs) Handles TxtTelMobile.TextChanged
        patientUpdate.PatientTel2 = TxtTelMobile.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtEmail_TextChanged(sender As Object, e As EventArgs) Handles TxtEmail.TextChanged
        patientUpdate.PatientEmail = TxtEmail.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub ChkCouvertureInternet_CheckedChanged(sender As Object, e As EventArgs) Handles ChkCouvertureInternet.CheckedChanged
        If ChkCouvertureInternet.Checked = True Then
            patientUpdate.PatientInternet = True
        Else
            patientUpdate.PatientInternet = False
        End If
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtProfession_TextChanged(sender As Object, e As EventArgs) Handles TxtProfession.TextChanged
        patientUpdate.Profession = TxtProfession.Text
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub DteDateEntree_ValueChanged(sender As Object, e As EventArgs) Handles DteDateEntree.ValueChanged
        If DteDateEntree.Value <> DteDateEntree.MaxDate Then
            DteDateEntree.Format = DateTimePickerFormat.Long
        End If

        patientUpdate.PatientDateEntree = DteDateEntree.Value
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub CbxUniteSanitaire_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxUniteSanitaire.SelectedIndexChanged
        Dim uniteSanitaireId, indice, i As Integer

        'Détermination de l'unité sanitaire id
        uniteSanitaireId = 0
        For Each kvp As KeyValuePair(Of Integer, String) In uniteSanitaireListe
            If kvp.Value = CbxUniteSanitaire.SelectedValue Then
                uniteSanitaireId = kvp.Key
                Exit For
            End If
        Next kvp

        patientUpdate.PatientUniteSanitaireId = uniteSanitaireId
        GestionAffichageBoutonValidation()

        'Site
        CbxSite.ResetText()

        If uniteSanitaireId <> 0 Then
            Dim siteListeParUniteSanitaire As Dictionary(Of Integer, String) = Table_site.GetSiteListeParUniteSanitaire(uniteSanitaireId)
            indice = siteListeParUniteSanitaire.Count - 1
            Dim siteDescription(indice) As String
            i = 0

            For Each kvp As KeyValuePair(Of Integer, String) In siteListeParUniteSanitaire
                siteDescription(i) = kvp.Value
                i += 1
            Next kvp
            CbxSite.DataSource = siteDescription
        End If
    End Sub

    Private Sub CbxSite_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxSite.SelectedIndexChanged
        Dim siteListeParUniteSanitaire As Dictionary(Of Integer, String) = Table_site.GetSiteListeParUniteSanitaire(patientUpdate.PatientUniteSanitaireId)
        Dim SiteId As Integer = 0
        For Each kvp As KeyValuePair(Of Integer, String) In siteListeParUniteSanitaire
            If kvp.Value = CbxSite.SelectedValue Then
                SiteId = kvp.Key
                Exit For
            End If
        Next kvp

        If SiteId = 0 Then
            If siteListeParUniteSanitaire.Count > 0 Then
                Dim kvpSite As KeyValuePair(Of Integer, String) = siteListeParUniteSanitaire.ElementAt(0)
                patientUpdate.PatientSiteId = kvpSite.Key
                CbxSite.SelectedItem = kvpSite.Value
            End If
        Else
            patientUpdate.PatientSiteId = SiteId
        End If

        GestionAffichageBoutonValidation()
    End Sub

    Private Sub TxtPharmacien_TextChanged(sender As Object, e As EventArgs) Handles TxtPharmacien.TextChanged
        patientUpdate.PharmacienId = PharmacienRorId
        GestionAffichageBoutonValidation()
    End Sub

    Private Sub DteDateDeces_ValueChanged(sender As Object, e As EventArgs) Handles DteDateDeces.ValueChanged
        patientUpdate.PatientDateDeces = DteDateDeces.Value
        GestionAffichageBoutonValidation()
    End Sub


    '-- Sortie
    Private Sub DteDateSortie_ValueChanged(sender As Object, e As EventArgs) Handles DteDateSortie.ValueChanged
        patientUpdate.PatientDateSortie = DteDateSortie.Value
    End Sub

    Private Sub TxtCommentaireSortie_TextChanged(sender As Object, e As EventArgs) Handles TxtCommentaireSortie.TextChanged
        patientUpdate.PatientCommentaireSortie = TxtCommentaireSortie.Text
    End Sub


    '-- Gestion de l'affichage du bouton de validation de mise à jour des données
    Private Sub GestionAffichageBoutonValidation()
        If EditMode = EnumEditMode.Modification Then
            If patientDao.Compare(patientUpdate, patientRead) = False Then
                RadBtnValider.Enabled = True
            Else
                RadBtnValider.Enabled = False
            End If
        End If
    End Sub


    'Gestion de l'affichage du contrôle de saisie de la date de naissance
    Private Sub DteDateNaissance_DropDown(sender As Object, e As EventArgs) Handles DteDateNaissance.DropDown
        If DteDateNaissance.Value = DteDateNaissance.MinDate Then
            DteDateNaissance.Value = Date.Now
            DteDateNaissance.Format = DateTimePickerFormat.Long
        End If
    End Sub

    'Gestion de l'affichage du contrôle de saisie de la date d'entrée
    Private Sub DteDateEntree_DropDown(sender As Object, e As EventArgs) Handles DteDateEntree.DropDown
        If DteDateEntree.Value = DteDateEntree.MaxDate Then
            DteDateEntree.Value = Date.Now
            DteDateEntree.Format = DateTimePickerFormat.Long
        End If
    End Sub

    'Gestion de l'affichage du contrôle de saisie de la date de décès
    Private Sub DteDateDeces_DropDown(sender As Object, e As EventArgs) Handles DteDateDeces.DropDown
        If DteDateDeces.Value = DteDateDeces.MaxDate Then
            DteDateDeces.Value = Date.Now
            DteDateDeces.Format = DateTimePickerFormat.Long
        End If
    End Sub

    'Gestion de l'affichage du contrôle de saisie de la date de sortie
    Private Sub DteDateSortie_DropDown(sender As Object, e As EventArgs) Handles DteDateSortie.DropDown
        If DteDateSortie.Value = DteDateSortie.MaxDate Then
            DteDateSortie.Value = Date.Now
            DteDateSortie.Format = DateTimePickerFormat.Long
        End If
    End Sub

    Private Function CalculmoduloNIR(NIR As Int64) As Integer
        Dim Reste As Integer
        Reste = NIR Mod 97
        Return 97 - Reste
    End Function



    '====================================================================================================================================
    '-- Gestion des notes
    '====================================================================================================================================

    'Chargement de la Grid Notes patient
    Private Sub ChargementNotesPatient()
        Dim patientNoteDao As New PatientNoteDao
        Dim dt As DataTable = patientNoteDao.getAllNotebyPatient(patientUpdate.patientId)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateCreation As Date
        Dim AfficheDateCreation, NotePatient, Auteur As String
        Dim AuteurId As Integer
        Dim rowCount As Integer = dt.Rows.Count - 1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            If dt.Rows(i)("oa_patient_note") IsNot DBNull.Value Then
                NotePatient = dt.Rows(i)("oa_patient_note")
            Else
                NotePatient = ""
            End If

            'Utilisateur creation
            Auteur = ""
            If dt.Rows(i)("oa_patient_note_utilisateur_creation") IsNot DBNull.Value Then
                If dt.Rows(i)("oa_patient_note_utilisateur_creation") <> 0 Then
                    Dim userDao As New UserDao
                    utilisateurHisto = userDao.getUserById(dt.Rows(i)("oa_patient_note_utilisateur_creation"))
                    Auteur = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
                End If
            End If

            'Date création
            AfficheDateCreation = ""
            If dt.Rows(i)("oa_patient_note_date_creation") IsNot DBNull.Value Then
                dateCreation = dt.Rows(i)("oa_patient_note_date_creation")
                AfficheDateCreation = outils.FormatageDateAffichage(dateCreation)
            Else
                If dt.Rows(i)("oa_patient_note_date_creation") IsNot DBNull.Value Then
                    dateCreation = dt.Rows(i)("oa_patient_note_date_creation")
                    AfficheDateCreation = outils.FormatageDateAffichage(dateCreation)
                End If
            End If

            AuteurId = 0
            If dt.Rows(i)("oa_patient_note_utilisateur_creation") IsNot DBNull.Value Then
                AuteurId = dt.Rows(i)("oa_patient_note_utilisateur_creation")
            End If

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadNotePatientDataGridView.AutoSizeRows = True
            RadNotePatientDataGridView.Rows.Add(iGrid)

            'Alimentation du DataGridView
            RadNotePatientDataGridView.Rows(iGrid).Cells("note").Value = NotePatient

            'Identifiant notePatient
            RadNotePatientDataGridView.Rows(iGrid).Cells("noteId").Value = dt.Rows(i)("oa_patient_note_id")

            'Auteur de la note
            RadNotePatientDataGridView.Rows(iGrid).Cells("auteur").Value = Auteur & vbCrLf & AfficheDateCreation
        Next

        'Enlève le focus sur la première ligne de la Grid
        If iGrid > -1 Then
            Me.RadNotePatientDataGridView.Rows(0).IsSelected = True
            Me.RadNotePatientDataGridView.Rows(0).IsCurrent = True
            Me.RadNotePatientDataGridView.Rows(0).EnsureVisible()
            'RadNotePatientDataGridView.ClearSelection()
        End If

    End Sub

    'Appel détail note en modification
    Private Sub RadNotePatientDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadNotePatientDataGridView.CellDoubleClick
        Dim aRow, maxRow As Integer
        aRow = Me.RadNotePatientDataGridView.Rows.IndexOf(Me.RadNotePatientDataGridView.CurrentRow)
        maxRow = RadNotePatientDataGridView.Rows.Count - 1

        If aRow <= maxRow And aRow > -1 Then
            'If RadNotePatientDataGridView.CurrentRow IsNot Nothing Then
            Dim NoteId As Integer = RadNotePatientDataGridView.Rows(aRow).Cells("noteId").Value

            Using vFNotePatientDetailEdit As New RadFPatientNoteDetailEdit
                vFNotePatientDetailEdit.SelectedNoteId = NoteId
                vFNotePatientDetailEdit.SelectedPatient = patientUpdate
                vFNotePatientDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
                vFNotePatientDetailEdit.TypeNote = EnumTypeNote.Administratif
                vFNotePatientDetailEdit.ShowDialog() 'Modal
                If vFNotePatientDetailEdit.CodeRetour = True Then
                    RadNotePatientDataGridView.Rows.Clear()
                    ChargementNotesPatient()
                End If

            End Using
        End If
    End Sub

    Private Sub BtnCreerNote_Click(sender As Object, e As EventArgs) Handles BtnCreerNote.Click
        CreerNote()
    End Sub

    'Appel détail note en création
    Private Sub CréerUneNoteToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles CréerUneNoteToolStripMenuItem.Click
        CreerNote()
    End Sub

    Private Sub CreerNote()
        Using vFNotePatientDetailEdit As New RadFPatientNoteDetailEdit
            vFNotePatientDetailEdit.SelectedNoteId = 0
            vFNotePatientDetailEdit.SelectedPatient = patientUpdate
            vFNotePatientDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte
            vFNotePatientDetailEdit.TypeNote = EnumTypeNote.Administratif
            vFNotePatientDetailEdit.ShowDialog() 'Modal
            If vFNotePatientDetailEdit.CodeRetour = True Then
                RadNotePatientDataGridView.Rows.Clear()
                ChargementNotesPatient()
            End If
        End Using
    End Sub

    'Gestion du tooltip de l'affichage des notes
    Private Sub RadNotePatientDataGridView_ToolTipTextNeeded(sender As Object, e As Telerik.WinControls.ToolTipTextNeededEventArgs) Handles RadNotePatientDataGridView.ToolTipTextNeeded
        Dim cell As GridDataCellElement = TryCast(sender, GridDataCellElement)
        If cell IsNot Nothing AndAlso cell.ColumnInfo.Name = "note" Then
            e.ToolTipText = cell.Value.ToString()
        End If
    End Sub



    '====================================================================================================================================
    '-- Gestion divers
    '====================================================================================================================================
    Private Sub InhiberZoneEnSaisie()
        TxtPrenom.Enabled = False
        TxtNom.Enabled = False
        TxtNomMarital.Enabled = False
        DteDateNaissance.Enabled = False
        CbxGenre.Enabled = False
        TxtNIR.Enabled = False
        TxtINS.Enabled = False
        TxtAdresse1.Enabled = False
        TxtAdresse2.Enabled = False
        TxtCodePostal.Enabled = False
        TxtVille.Enabled = False
        TxtTelFixe.Enabled = False
        TxtTelMobile.Enabled = False
        TxtEmail.Enabled = False
        DteDateEntree.Enabled = False
        CbxSite.Enabled = False
        CbxUniteSanitaire.Enabled = False
        ChkCouvertureInternet.Enabled = False
        DteDateSortie.Enabled = False
        DteDateDeces.Enabled = False
        TxtCommentaireSortie.Enabled = False
        TxtProfession.Enabled = False
        RadBtnPharmacien.Enabled = False
    End Sub

    Private Sub ActiverZoneEnSaisie()
        TxtPrenom.Enabled = True
        TxtNom.Enabled = True
        TxtNomMarital.Enabled = True
        DteDateNaissance.Enabled = True
        CbxGenre.Enabled = True
        TxtNIR.Enabled = True
        TxtINS.Enabled = True
        TxtAdresse1.Enabled = True
        TxtAdresse2.Enabled = True
        TxtCodePostal.Enabled = True
        TxtVille.Enabled = True
        TxtTelFixe.Enabled = True
        TxtTelMobile.Enabled = True
        TxtEmail.Enabled = True
        DteDateEntree.Enabled = True
        CbxSite.Enabled = True
        CbxUniteSanitaire.Enabled = True
        ChkCouvertureInternet.Enabled = True
        DteDateSortie.Enabled = True
        DteDateDeces.Enabled = True
        TxtCommentaireSortie.Enabled = True
        TxtProfession.Enabled = True
        RadBtnPharmacien.Enabled = True
    End Sub


    '====================================================================================================================================
    '-- Gestion des boutons d'action
    '====================================================================================================================================

    'Appel google maps
    Private Sub RadBtnGoogleMaps_Click(sender As Object, e As EventArgs) Handles RadBtnGoogleMaps.Click
        Dim GoogleOK As Boolean = False
        If TxtAdresse1.Text <> "" Then
            If TxtCodePostal.Text <> "" Then
                If TxtVille.Text <> "" Then
                    'lancer l'URL pour afficher l'adresse dans Google Maps
                    GoogleOK = True
                    Dim MonURL As String
                    MonURL = "http://www.google.fr/maps/place/" + TxtAdresse1.Text + " " + TxtCodePostal.Text + " " + TxtVille.Text
                    Process.Start(MonURL)
                End If
            End If
        End If
        If GoogleOK = False Then
            MessageBox.Show("L'adresse 1, le code postal et la ville doivent être renseignés pour lancer la recherche", "Notification", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
        End If
    End Sub

    Private Sub RadBtnValidationDateNaissance_Click(sender As Object, e As EventArgs) Handles RadBtnValidationDateNaissance.Click
        If EditMode = EnumEditMode.Creation Then
            If DteDateNaissance.Value.Date > Date.Now.Date Then
                MessageBox.Show("La date de naissance ne doit pas être supérieure à la date du jour")
                Exit Sub
            End If

            If DteDateNaissance.Value <> DteDateNaissance.MinDate Then
                Dim DateNaissance As Date
                DateNaissance = DteDateNaissance.Value
                LblAge.Text = CalculAgeEnAnneeEtMoisString(DateNaissance)

                'Vérifie s'il existe des patients ayant la même date de naissance
                Dim ListeDataTable As DataTable = patientDao.ListePatientDateNaissance(DteDateNaissance.Value)
                If ListeDataTable.Rows.Count > 0 Then
                    Using vFPatientListeDoublons As New FPatientListeDoublons
                        vFPatientListeDoublons.UtilisateurConnecte = Me.UtilisateurConnecte
                        vFPatientListeDoublons.ListeDataTable = ListeDataTable
                        vFPatientListeDoublons.ShowDialog() 'Modal
                        If vFPatientListeDoublons.CodeRetour = True Then
                            'Retour liste des patients
                            Close()
                        Else
                            'Retour écran de création d'un patient
                            ActiverZoneEnSaisie()
                        End If
                    End Using
                Else
                    'Si pas de patient avec cette date de naissance, activation des zones en saisie
                    ActiverZoneEnSaisie()
                End If
            End If
        End If
    End Sub

    Private Sub RadBtnPharmacien_Click(sender As Object, e As EventArgs) Handles RadBtnPharmacien.Click
        Using vRadFRorListe As New RadFRorListe
            vRadFRorListe.Selecteur = True
            vRadFRorListe.PatientId = Me.SelectedPatientId
            vRadFRorListe.SpecialiteId = 50 'Pharmacien
            vRadFRorListe.TypeRor = "Intervenant"
            vRadFRorListe.ShowDialog()
            If vRadFRorListe.SelectedRorId <> 0 Then
                PharmacienRorId = vRadFRorListe.SelectedRorId
                ror = rorDao.getRorById(PharmacienRorId)
                TxtPharmacien.Text = ror.Nom
            End If
        End Using
    End Sub

    Private Sub RadBtnRDV_Click(sender As Object, e As EventArgs) Handles RadBtnRDV.Click
        Cursor.Current = Cursors.WaitCursor
        Me.Enabled = False
        Using form As New RadFPatientRendezVousListe
            form.SelectedPatient = patientUpdate
            form.ShowDialog()
        End Using
        Me.Enabled = True
    End Sub

    'Retour écran précédent sans action
    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Me.CodeRetour = False
        Close()
    End Sub

End Class
