Imports System.Data.SqlClient
Imports Oasis_Common

Public Class FContexteDetailEdit
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedContexteId As Integer
    Private privateSelectedDrcId As Integer
    Private privateCodeRetour As Boolean
    Private privateContexteTransformeEnAntecedent As Boolean
    Private privateCategorieContexte As String

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

    Public Property SelectedContexteId As Integer
        Get
            Return privateSelectedContexteId
        End Get
        Set(value As Integer)
            privateSelectedContexteId = value
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

    Public Property SelectedDrcId As Integer
        Get
            Return privateSelectedDrcId
        End Get
        Set(value As Integer)
            privateSelectedDrcId = value
        End Set
    End Property

    Public Property ContexteTransformeEnAntecedent As Boolean
        Get
            Return privateContexteTransformeEnAntecedent
        End Get
        Set(value As Boolean)
            privateContexteTransformeEnAntecedent = value
        End Set
    End Property

    Public Property CategorieContexte As String
        Get
            Return privateCategorieContexte
        End Get
        Set(value As String)
            privateCategorieContexte = value
        End Set
    End Property

    Enum EnumEditMode
        Creation = 1
        Modification = 2
    End Enum

    Dim EditMode As Integer
    Dim utilisateurHisto As Utilisateur = New Utilisateur()
    Dim Drc As New Drc
    Dim drcdao As New DrcDao
    Dim ContexteHistoACreer As New AntecedentHisto
    Dim conxn As New SqlConnection(getConnectionString())

    Private Sub FContexteDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitZone()
        ChargementEtatCivil()
        If SelectedContexteId <> 0 Then
            EditMode = EnumEditMode.Modification
            BtnValidationCreationContexte.Hide()
            ChargementContexteExistant()
            InhiberZonesDeSaisie()
        Else
            EditMode = EnumEditMode.Creation
            'Catégorie
            Select Case CategorieContexte
                Case "M"
                    CbxCategorieContexte.Text = "Médical"
                Case "B"
                    CbxCategorieContexte.Text = "Bio-environnemental"
            End Select
            'Dénomination DRC
            TxtDrcId.Text = SelectedDrcId
            If drcdao.GetDrc(Drc, SelectedDrcId) = True Then
                LblDrcDenomination.Text = Drc.DrcLibelle
                LblDrcDenomination.ForeColor = Color.DarkBlue
            Else
                LblDrcDenomination.Text = ""
            End If
            'Date début
            DteDateDebut.Value = Date.Now
            'Cacher la date de fin et l'initialiser à la date virtuelle infinie
            DteDateFin.Format = DateTimePickerFormat.Custom
            DteDateFin.CustomFormat = " "
            DteDateFin.Value = New Date(2999, 12, 31, 0, 0, 0)
            'Publication
            ChkPublie.Checked = True
            ChkPublie.ForeColor = Color.Red
            LblPublication.Hide()
            'Diagnostic
            ChkDiagnosticConfirme.Checked = True
            ChkDiagnosticConfirme.ForeColor = Color.Red
            'Inhiber les zones d'arrêt
            GbxArret.Hide()
            'Affichage des boutons d'action
            BtnValidationCreationContexte.Show()
            BtnRecupereDrc.Show()
            'Inhiber les boutons d'action de mise à jour
            BtnArret.Hide()
            BtnModifier.Hide()
            BtnPublication.Hide()
            BtnTransformer.Hide()
            BtnSupprimer.Hide()
            LblCreationContexte1.Hide()
            LblCreationContexte2.Hide()
            LblContexteDateCreation.Hide()
            LblUtilisateurCreation.Hide()
            LblModificationContexte1.Hide()
            LblModificationContexte2.Hide()
            LblContexteDateModification.Hide()
            LblUtilisateurModification.Hide()
        End If

    End Sub


    '=============================================================================================
    '==================================== Gestion de l'affichage des zones de l'écran ============
    '=============================================================================================
    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)
    End Sub

    Private Sub ChargementContexteExistant()
        Dim contexteDataReader As SqlDataReader
        Dim conxn As New SqlConnection(getConnectionString())
        Dim SQLString As String = "select * from oasis.oa_antecedent where oa_antecedent_id = " & SelectedContexteId & ";"
        Dim myCommand As New SqlCommand(SQLString, conxn)

        Dim contexteArrete As Boolean
        Dim dateDebut, dateFin, dateCreation, dateModification As Date
        Dim ordreAffichage As Integer
        Dim categorieContexte As String
        Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)

        conxn.Open()
        contexteDataReader = myCommand.ExecuteReader()
        If contexteDataReader.Read() Then
            'Catégorie contexte
            If contexteDataReader("oa_antecedent_categorie_contexte") Is DBNull.Value Then
                categorieContexte = "M"
            Else
                categorieContexte = contexteDataReader("oa_antecedent_categorie_contexte")
            End If
            Select Case categorieContexte
                Case "M"
                    CbxCategorieContexte.Text = "Médical"
                Case "B"
                    CbxCategorieContexte.Text = "Bio-environnemental"
            End Select


            'Code DRC du contexte
            If contexteDataReader("oa_antecedent_drc_id") Is DBNull.Value Then
                TxtDrcId.Text = ""
            Else
                TxtDrcId.Text = contexteDataReader("oa_antecedent_drc_id")
            End If

            'Dénomination DRC
            If drcdao.GetDrc(Drc, TxtDrcId.Text) = True Then
                LblDrcDenomination.Text = Drc.DrcLibelle
                LblDrcDenomination.ForeColor = Color.DarkBlue
            Else
                LblDrcDenomination.Text = ""
            End If

            'Description du contexte
            If contexteDataReader("oa_antecedent_description") Is DBNull.Value Then
                TxtContexteDescription.Text = ""
            Else
                TxtContexteDescription.Text = contexteDataReader("oa_antecedent_description")
            End If

            'Récupération de la date de début du contexte
            If contexteDataReader("oa_antecedent_date_debut") IsNot DBNull.Value Then
                dateDebut = contexteDataReader("oa_antecedent_date_debut")
            Else
                dateDebut = "31/12/2999"
            End If
            DteDateDebut.Value = dateDebut

            'Récupération de la date de fin de validité du contexte
            If contexteDataReader("oa_antecedent_date_fin") IsNot DBNull.Value Then
                dateFin = contexteDataReader("oa_antecedent_date_fin")
            Else
                dateFin = "31/12/2999"
            End If
            DteDateFin.Value = dateFin
            Dim DateSansLimite As New Date(2999, 12, 31, 0, 0, 0)
            If DteDateFin.Value <> DateSansLimite Then
                DteDateFin.Format = DateTimePickerFormat.Long
            Else
                DteDateFin.Format = DateTimePickerFormat.Custom
                DteDateFin.CustomFormat = " "
            End If

            'Ordre d'affichage
            If contexteDataReader("oa_antecedent_ordre_affichage1") IsNot DBNull.Value Then
                ordreAffichage = CInt(contexteDataReader("oa_antecedent_ordre_affichage1"))
            Else
                ordreAffichage = 0
            End If
            NumOrdreAffichage.Value = ordreAffichage

            'Statut affichage du contexte
            ChkCache.Checked = False
            ChkPublie.Checked = False
            If contexteDataReader("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                Dim StatutAffichage As String = contexteDataReader("oa_antecedent_statut_affichage")
                Select Case StatutAffichage
                    Case "P"
                        ChkPublie.Checked = True
                        ChkPublie.ForeColor = Color.Red
                        LblPublication.Text = "Contexte publié"
                    Case "C"
                        ChkCache.Checked = True
                        ChkCache.ForeColor = Color.Red
                        LblPublication.Text = "Contexte caché"
                End Select
            End If

            'Diagnostic
            If contexteDataReader("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                Dim Diagnostic As Integer = contexteDataReader("oa_antecedent_diagnostic")
                Select Case Diagnostic
                    Case 1
                        ChkDiagnosticConfirme.Checked = True
                        ChkDiagnosticConfirme.ForeColor = Color.Red
                    Case 2
                        ChkDiagnosticSuspecte.Checked = True
                        ChkDiagnosticSuspecte.ForeColor = Color.Red
                    Case 3
                        'ChkDiagnosticSuppose.Checked = True
                        'ChkDiagnosticSuppose.ForeColor = Color.Red
                    Case 4
                        ChkDiagnosticNotion.Checked = True
                        ChkDiagnosticNotion.ForeColor = Color.Red
                End Select
            End If

            'Si le contexte a été déclaré arrêté, ce contexte ne doit pas pouvoir être modifié
            If contexteDataReader("oa_antecedent_arret") IsNot DBNull.Value Then
                If contexteDataReader("oa_antecedent_arret") = "1" Then
                    contexteArrete = True
                    Me.Text = "Visualisation détail contexte patient (contexte arrêté)"
                    AfficherZonesArret()
                    InhiberZonesDeSaisieArret()
                Else

                End If
            End If

            If contexteDataReader("oa_antecedent_arret_commentaire") Is DBNull.Value Then
                TxtArretCommentaire.Text = ""
            Else
                TxtArretCommentaire.Text = contexteDataReader("oa_antecedent_arret_commentaire")
            End If

            'Création du contexte : date et utilisateur
            If contexteDataReader("oa_antecedent_date_creation") IsNot DBNull.Value Then
                dateCreation = contexteDataReader("oa_antecedent_date_creation")
                LblContexteDateCreation.Text = dateCreation.ToString("dd.MM.yyyy")
            Else
                LblContexteDateCreation.Text = ""
                LblCreationContexte1.Hide()
                LblCreationContexte2.Hide()
            End If

            LblUtilisateurCreation.Text = ""
            If contexteDataReader("oa_antecedent_utilisateur_creation") IsNot DBNull.Value Then
                If contexteDataReader("oa_antecedent_utilisateur_creation") <> 0 Then
                    SetUtilisateur(utilisateurHisto, contexteDataReader("oa_antecedent_utilisateur_creation"))
                    LblUtilisateurCreation.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
                End If
            Else
                LblCreationContexte2.Hide()
            End If

            If contexteDataReader("oa_antecedent_date_modification") IsNot DBNull.Value Then
                dateModification = contexteDataReader("oa_antecedent_date_modification")
                LblContexteDateModification.Text = dateModification.ToString("dd.MM.yyyy")
            Else
                LblContexteDateModification.Text = ""
                LblModificationContexte1.Hide()
                LblModificationContexte2.Hide()
            End If
        End If

        LblUtilisateurModification.Text = ""
        If contexteDataReader("oa_antecedent_utilisateur_modification") IsNot DBNull.Value Then
            If contexteDataReader("oa_antecedent_utilisateur_modification") <> 0 Then
                SetUtilisateur(utilisateurHisto, contexteDataReader("oa_antecedent_utilisateur_modification"))
                LblUtilisateurModification.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
            Else
                LblModificationContexte2.Hide()
            End If
        End If

        'Initialisation classe Historisation contexte 
        AntecedentHistoCreationDao.InitClasseAntecedentHistorisation(contexteDataReader, UtilisateurConnecte, ContexteHistoACreer)

        'Libération des ressources d'accès aux données
        conxn.Close()
        myCommand.Dispose()
    End Sub

    'Suppression (annulation) du contexte
    Private Sub BtnSupprimer_Click(sender As Object, e As EventArgs) Handles BtnSupprimer.Click
        If MsgBox("Attention, confirmez-vous l'annulation du contexte", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            'Annulation contexte
            If AnnulationContexte() = True Then
                MessageBox.Show("Le contexte patient a été annulé")
                Me.CodeRetour = True
                Close()
            Else
                Me.CodeRetour = False
            End If
        End If
    End Sub

    'Transformation du contexte en antécédent
    Private Sub BtnReactiver_Click(sender As Object, e As EventArgs) Handles BtnTransformer.Click
        If MsgBox("confirmation de la transformation en antécédent", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            If TransformationEnAntecedent(SelectedContexteId) = True Then
                MessageBox.Show("Le contexte a été transformé en antécédent")
                Me.ContexteTransformeEnAntecedent = True
                Me.CodeRetour = True
                Close()
            Else
                Me.CodeRetour = False
            End If
        End If
    End Sub
    'Abandon et retour sur l'écran précédent
    Private Sub DtnAbandon_Click(sender As Object, e As EventArgs) Handles DtnAbandon.Click
        Me.CodeRetour = False
        Close()
    End Sub

    'Lance la validation des modifications des données générales, code DRC, description et date de début
    Private Sub BtnValider_Click(sender As Object, e As EventArgs) Handles BtnValider.Click
        'Contrôle des données saisie
        Dim Valide As Boolean = True
        Dim MessageErreur1 As String = ""
        Dim MessageErreur2 As String = ""
        Dim MessageErreur3 As String = ""
        Dim MessageErreur4 As String = ""
        Dim MessageErreur As String = ""

        If TxtDrcId.Text = "" Then
            Valide = False
            MessageErreur1 = "Le code DRC est obligatoire"
        End If

        If TxtContexteDescription.Text = "" Then
            Valide = False
            MessageErreur2 = "La description du contexte est obligatoire"
        End If

        If DteDateDebut.Value > Date.Now() Then
            Valide = False
            MessageErreur3 = "La date de début du contexte ne peut pas être supérieure à la date du jour"
        End If

        If DteDateFin.Value < DteDateDebut.Value Then
            Valide = False
            MessageErreur4 = "La date de fin de validité du contexte ne peut pas être inférieure à la date de début"
        End If

        'Appel de la mise à jour des données
        If Valide = True Then
            If ModificationContexte() = True Then
                Me.CodeRetour = True
                Close()
            Else
                Me.CodeRetour = False
            End If
        Else
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

                MessageErreur = MessageErreur & vbCrLf & "/!\ Modification du contexte impossible, des données sont incorrectes"
                MessageBox.Show(MessageErreur)
            End If
        End If
    End Sub

    'Validation de la publication
    Private Sub BtnValidationPublication_Click(sender As Object, e As EventArgs) Handles BtnValidationPublication.Click
        Dim Valide As Boolean = True
        Dim publication As String
        'Contrôle des données saisie
        If ChkPublie.Checked = True Then
            publication = "P"
        Else
            publication = "C"
        End If

        'Appel de la mise à jour des données
        If Valide = True Then
            If ModificationPublicationContexte(SelectedContexteId, publication) = True Then
                MessageBox.Show("La publication du contexte patient a été modifié")
                Me.CodeRetour = True
                Close()
            Else
                Me.CodeRetour = False
            End If
        End If
    End Sub

    'Validation de l'arrêt du contexte
    Private Sub BtnValidationArret_Click(sender As Object, e As EventArgs) Handles BtnValidationArret.Click
        Dim Valide As Boolean = True
        'Contrôle des données saisies

        'Appel de la mise à jour des données
        If Valide = True Then
            If ArretContexte() = True Then
                MessageBox.Show("L'arrêt du contexte patient a été réalisé")
                Me.CodeRetour = True
                Close()
            Else
                Me.CodeRetour = False
            End If
        End If
    End Sub

    'Validation de la création d'un contexte
    Private Sub BtnValidationCreationContexte_Click(sender As Object, e As EventArgs) Handles BtnValidationCreationContexte.Click
        Dim Valide As Boolean = True
        Dim MessageErreur1 As String = ""
        Dim MessageErreur2 As String = ""
        Dim MessageErreur3 As String = ""
        Dim MessageErreur4 As String = ""
        Dim MessageErreur As String = ""

        If TxtDrcId.Text = "" Then
            Valide = False
            MessageErreur1 = "Le code DRC est obligatoire"
        End If

        If TxtContexteDescription.Text = "" Then
            Valide = False
            MessageErreur2 = "La description du contexte est obligatoire"
        End If

        If DteDateDebut.Value > Date.Now() Then
            Valide = False
            MessageErreur3 = "La date de début du contexte ne peut pas être supérieure à la date du jour"
        End If

        If DteDateFin.Value < DteDateDebut.Value Then
            Valide = False
            MessageErreur4 = "La date de fin de validité du contexte ne peut pas être inférieure à la date de début"
        End If

        'Appel de la mise à jour des données
        If Valide = True Then
            If CreationContexte() = True Then
                Me.CodeRetour = True
                Close()
            Else
                Me.CodeRetour = False
            End If
        Else
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

                MessageErreur = MessageErreur & vbCrLf & "/!\ Création du contexte impossible, des données sont incorrectes"
                MessageBox.Show(MessageErreur)
            End If
        End If
    End Sub

    Private Sub TxtDrcId_DoubleClick(sender As Object, e As EventArgs) Handles TxtDrcId.DoubleClick
        'Appel du sélecteur de code DRC
        Dim vFDrcSelecteur As New FDrcSelecteur
        vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
        vFDrcSelecteur.CategorieOasis = 1       'Catégorie Oasis : "Contexte et Antécédent"
        vFDrcSelecteur.ShowDialog()             'Modal
        Dim SelectedDrcId As Integer = vFDrcSelecteur.SelectedDrcId
        vFDrcSelecteur.Dispose()

        'Si un médicament a été sélectionné
        If SelectedDrcId <> 0 Then
            TxtDrcId.Text = SelectedDrcId
            If drcdao.GetDrc(Drc, SelectedDrcId) = True Then
                LblDrcDenomination.Text = Drc.DrcLibelle
                LblDrcDenomination.ForeColor = Color.DarkBlue
            Else
                LblDrcDenomination.Text = ""
            End If
        End If
    End Sub

    '=============================================================================================
    '==================================== Mise à jour de la base de données ======================
    '=============================================================================================

    'Modification d'un contexte en base de données
    Private Function ModificationContexte() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim categorieContexteMaj As String

        Dim dateModification As Date = Date.Now.Date

        categorieContexteMaj = ""
        Select Case CbxCategorieContexte.Text
            Case "Médical"
                categorieContexteMaj = "M"
            Case "Bio-environnemental"
                categorieContexteMaj = "B"
        End Select

        Dim SQLstring As String = "update oasis.oa_antecedent set oa_antecedent_date_modification = @dateModification, oa_antecedent_utilisateur_modification = @utilisateurModification, oa_antecedent_drc_id = @drcId, oa_antecedent_categorie_contexte = @categorieContexte, oa_antecedent_description = @description, oa_antecedent_date_debut = @dateDebut, oa_antecedent_date_fin = @dateFin, oa_antecedent_ordre_affichage1 = @ordreAffichage, oa_antecedent_diagnostic = @diagnostic where oa_antecedent_id = @antecedentId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        Dim Diagnostic As Integer
        If ChkDiagnosticConfirme.Checked = True Then
            Diagnostic = 1
        Else
            If ChkDiagnosticSuspecte.Checked = True Then
                Diagnostic = 2
            Else
                If ChkDiagnosticNotion.Checked = True Then
                    Diagnostic = 4
                End If
            End If
        End If

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@drcId", TxtDrcId.Text)
            .AddWithValue("@categorieContexte", categorieContexteMaj)
            .AddWithValue("@description", TxtContexteDescription.Text)
            .AddWithValue("@dateDebut", DteDateDebut.Value)
            .AddWithValue("@dateFin", DteDateFin.Value)
            .AddWithValue("@ordreAffichage", NumOrdreAffichage.Value)
            .AddWithValue("@antecedentId", SelectedContexteId.ToString)
            .AddWithValue("@diagnostic", Diagnostic)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            MessageBox.Show("Contexte patient modifié")
        Catch ex As Exception
            'PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            ContexteHistoACreer.HistorisationDate = Date.Now()
            ContexteHistoACreer.Categorie = categorieContexteMaj
            ContexteHistoACreer.UtilisateurId = UtilisateurConnecte.UtilisateurId
            ContexteHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent
            ContexteHistoACreer.DrcId = TxtDrcId.Text
            ContexteHistoACreer.Description = TxtContexteDescription.Text
            ContexteHistoACreer.DateDebut = DteDateDebut.Value
            ContexteHistoACreer.DateFin = DteDateFin.Value
            ContexteHistoACreer.Ordre1 = NumOrdreAffichage.Value
            ContexteHistoACreer.Diagnostic = Diagnostic

            'Création dans l'historique des modifications du contexte
            AntecedentHistoCreationDao.CreationAntecedentHisto(ContexteHistoACreer, UtilisateurConnecte, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent)
        End If

        Return codeRetour
    End Function

    'Modification de la publication d'un contexte en base de données
    Private Function ModificationPublicationContexte(contexteId As Integer, publication As String) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_antecedent set oa_antecedent_date_modification = @dateModification, oa_antecedent_utilisateur_modification = @utilisateurModification, oa_antecedent_statut_affichage = @publication where oa_antecedent_id = @antecedentId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@publication", publication)
            .AddWithValue("@antecedentId", contexteId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            'PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            ContexteHistoACreer.HistorisationDate = Date.Now()
            ContexteHistoACreer.UtilisateurId = UtilisateurConnecte.UtilisateurId
            ContexteHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent
            ContexteHistoACreer.StatutAffichage = publication

            'Création dans l'historique des modifications du contexte
            AntecedentHistoCreationDao.CreationAntecedentHisto(ContexteHistoACreer, UtilisateurConnecte, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent)
        End If

        Return codeRetour
    End Function

    'Mise à jour de l'arrêt d'un contexte en base de données
    Private Function ArretContexte() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_antecedent set oa_antecedent_date_modification = @dateModification, oa_antecedent_utilisateur_modification = @utilisateurModification, oa_antecedent_arret = @arret, oa_antecedent_arret_commentaire = @arretCommentaire where oa_antecedent_id = @antecedentId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@arret", "1")
            .AddWithValue("@arretCommentaire", TxtArretCommentaire.Text)
            .AddWithValue("@antecedentId", SelectedContexteId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            'PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            ContexteHistoACreer.HistorisationDate = Date.Now()
            ContexteHistoACreer.UtilisateurId = UtilisateurConnecte.UtilisateurId
            ContexteHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ArretAntecedent
            ContexteHistoACreer.Arret = True
            ContexteHistoACreer.ArretCommentaire = TxtArretCommentaire.Text

            'Création dans l'historique des modifications du contexte
            AntecedentHistoCreationDao.CreationAntecedentHisto(ContexteHistoACreer, UtilisateurConnecte, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ArretAntecedent)
        End If

        Return codeRetour
    End Function


    'Transformation d'un contexte en antécédent
    Private Function TransformationEnAntecedent(contexteId As Integer) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_antecedent set oa_antecedent_type = 'A', oa_antecedent_date_modification = @dateModification, oa_antecedent_utilisateur_modification = @utilisateurModification, oa_antecedent_date_fin = @dateFin, oa_antecedent_nature = @nature, oa_antecedent_priorite = @priorite, oa_antecedent_niveau = @niveau, oa_antecedent_id_niveau1 = @idNiveau1, oa_antecedent_id_niveau2 = @idNiveau2, oa_antecedent_ordre_affichage1 = @ordreAffichage1, oa_antecedent_ordre_affichage2 = @ordreAffichage2, oa_antecedent_ordre_affichage3 = @ordreAffichage3 where oa_antecedent_id = @antecedentId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@dateFin", New Date(2999, 12, 31, 0, 0, 0).ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@nature", "")
            .AddWithValue("@priorite", 0)
            .AddWithValue("@niveau", 1)
            .AddWithValue("@idNiveau1", 0)
            .AddWithValue("@idNiveau2", 0)
            .AddWithValue("@ordreAffichage1", 990)
            .AddWithValue("@ordreAffichage2", 0)
            .AddWithValue("@ordreAffichage3", 0)
            .AddWithValue("@antecedentId", contexteId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            'PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            ContexteHistoACreer.HistorisationDate = Date.Now()
            ContexteHistoACreer.Type = "A"
            ContexteHistoACreer.UtilisateurId = UtilisateurConnecte.UtilisateurId
            ContexteHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ReactivationAntecedent
            ContexteHistoACreer.Nature = ""
            ContexteHistoACreer.Niveau = 1
            ContexteHistoACreer.Niveau1Id = 0
            ContexteHistoACreer.Niveau2Id = 0
            ContexteHistoACreer.Ordre1 = 990
            ContexteHistoACreer.Ordre2 = 0
            ContexteHistoACreer.Ordre3 = 0

            'Création dans l'historique des modifications du contexte
            AntecedentHistoCreationDao.CreationAntecedentHisto(ContexteHistoACreer, UtilisateurConnecte, EnumEtatAntecedentHisto.ReactivationAntecedent)
        End If

        Return codeRetour
    End Function

    'Annulation d'un contexte en base de données
    Private Function AnnulationContexte() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_antecedent set oa_antecedent_date_modification = @dateModification, oa_antecedent_utilisateur_modification = @utilisateurModification, oa_antecedent_inactif = @inactif where oa_antecedent_id = @antecedentId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@inactif", "1")
            .AddWithValue("@antecedentId", SelectedContexteId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            ContexteHistoACreer.HistorisationDate = Date.Now()
            ContexteHistoACreer.UtilisateurId = UtilisateurConnecte.UtilisateurId
            ContexteHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.AnnulationAntecedent
            ContexteHistoACreer.Inactif = True

            'Création dans l'historique des modifications du contexte
            AntecedentHistoCreationDao.CreationAntecedentHisto(ContexteHistoACreer, UtilisateurConnecte, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.AnnulationAntecedent)
        End If

        Return codeRetour
    End Function

    'Création d'un contexte en base de données
    Private Function CreationContexte() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        'Définition publication
        Dim Publication As String
        If ChkCache.Checked = True Then
            Publication = "C"
        Else
            Publication = "P"
        End If

        'Catégorie contexte
        Dim categorieContexteCre As String
        categorieContexteCre = ""
        Select Case CbxCategorieContexte.Text
            Case "Médical"
                categorieContexteCre = "M"
            Case "Bio-environnemental"
                categorieContexteCre = "B"
        End Select

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "insert into oasis.oa_antecedent (oa_antecedent_patient_id, oa_antecedent_type, oa_antecedent_drc_id, oa_antecedent_description, oa_antecedent_date_creation, oa_antecedent_date_modification, oa_antecedent_utilisateur_creation, oa_antecedent_utilisateur_modification, oa_antecedent_date_debut, oa_antecedent_niveau, oa_antecedent_nature, oa_antecedent_statut_affichage, oa_antecedent_inactif, oa_antecedent_ordre_affichage1, oa_antecedent_ordre_affichage2, oa_antecedent_ordre_affichage3, oa_antecedent_categorie_contexte, oa_antecedent_date_fin, oa_antecedent_diagnostic) VALUES (@patientId, @type, @drcId, @description, @dateCreation, @dateModification, @utilisateurCreation, @utilisateurModification, @dateDebut, @niveau, @nature, @publication, @inactif, @ordreAffichage1, @ordreAffichage2, @ordreAffichage3, @categorieContexte, @dateFin, @diagnostic)"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        Dim Diagnostic As Integer
        If ChkDiagnosticConfirme.Checked = True Then
            Diagnostic = 1
        Else
            If ChkDiagnosticSuspecte.Checked = True Then
                Diagnostic = 2
            Else
                If ChkDiagnosticNotion.Checked = True Then
                    Diagnostic = 4
                End If
            End If
        End If

        With cmd.Parameters
            .AddWithValue("@patientId", SelectedPatient.patientId.ToString)
            .AddWithValue("@type", "C")
            .AddWithValue("@drcId", TxtDrcId.Text)
            .AddWithValue("@description", TxtContexteDescription.Text)
            .AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@utilisateurCreation", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@utilisateurModification", 0)
            .AddWithValue("@dateDebut", DteDateDebut.Value.ToString("yyyy-MM-dd"))
            .AddWithValue("@niveau", 1)
            .AddWithValue("@nature", "Patient")
            .AddWithValue("@publication", Publication)
            .AddWithValue("@inactif", 0)
            .AddWithValue("@ordreAffichage1", NumOrdreAffichage.Value)
            .AddWithValue("@ordreAffichage2", 0)
            .AddWithValue("@ordreAffichage3", 0)
            .AddWithValue("@categorieContexte", categorieContexteCre)
            .AddWithValue("@dateFin", DteDateFin.Value.ToString("yyyy-MM-dd"))
            .AddWithValue("@diagnostic", Diagnostic)
        End With

        Try
            conxn.Open()
            Dim n As Integer 'Pour récupérer le nombre d'occurences enregistrées
            da.InsertCommand = cmd
            n = da.InsertCommand.ExecuteNonQuery()
            MessageBox.Show("Contexte patient créé")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données dans l'instance de la classe Historisation antecedent
            ContexteHistoACreer.HistorisationDate = DateTime.Now()
            ContexteHistoACreer.UtilisateurId = UtilisateurConnecte.UtilisateurId
            ContexteHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.CreationAntecedent
            ContexteHistoACreer.PatientId = SelectedPatient.patientId.ToString
            ContexteHistoACreer.Type = "C"
            ContexteHistoACreer.Description = TxtContexteDescription.Text
            ContexteHistoACreer.DateDebut = DteDateDebut.Value.ToString("yyyy-MM-dd")
            ContexteHistoACreer.Niveau = 1
            ContexteHistoACreer.Nature = "Patient"
            ContexteHistoACreer.StatutAffichage = Publication
            ContexteHistoACreer.Inactif = 0
            ContexteHistoACreer.Ordre1 = NumOrdreAffichage.Value
            ContexteHistoACreer.Ordre2 = 0
            ContexteHistoACreer.Ordre3 = 0
            ContexteHistoACreer.Categorie = categorieContexteCre
            ContexteHistoACreer.DateFin = DteDateFin.Value.ToString("yyyy-MM-dd")
            ContexteHistoACreer.Diagnostic = Diagnostic

            'Récupération de l'identifiant du contexte créé
            Dim contexteLastDataReader As SqlDataReader
            SQLstring = "select max(oa_antecedent_id) from oasis.oa_antecedent where oa_antecedent_patient_id = " & SelectedPatient.patientId & ";"
            Dim contexteLastCommand As New SqlCommand(SQLstring, conxn)
            conxn.Open()
            contexteLastDataReader = contexteLastCommand.ExecuteReader()
            If contexteLastDataReader.HasRows Then
                contexteLastDataReader.Read()
                'Récupération de la clé de l'enregistrement créé
                ContexteHistoACreer.AntecedentId = contexteLastDataReader(0)

                'Libération des ressources d'accès aux données
                conxn.Close()
                contexteLastCommand.Dispose()
            End If

            'Lecture du contexte créé avec toutes ses données pour communiquer le DataReader à la fonction dédiée
            Dim contexteCreeDataReader As SqlDataReader
            SQLstring = "select * from oasis.oa_antecedent where oa_antecedent_id = " & ContexteHistoACreer.AntecedentId & ";"
            Dim contexteCreeCommand As New SqlCommand(SQLstring, conxn)
            conxn.Open()
            contexteCreeDataReader = contexteCreeCommand.ExecuteReader()
            If contexteCreeDataReader.Read() Then
                'Initialisation classe Historisation antecedent 
                AntecedentHistoCreationDao.InitClasseAntecedentHistorisation(contexteCreeDataReader, UtilisateurConnecte, ContexteHistoACreer)

                'Libération des ressources d'accès aux données
                conxn.Close()
                contexteCreeCommand.Dispose()
            End If

            'Création dans l'historique du contexte créé
            AntecedentHistoCreationDao.CreationAntecedentHisto(ContexteHistoACreer, UtilisateurConnecte, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.CreationAntecedent)
        End If

        Return codeRetour

    End Function

    '=============================================================================================
    '==================================== Gestion de l'affichage des zones d'écran ===============
    '=============================================================================================
    Private Sub ChkPublie_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPublie.CheckedChanged
        If ChkPublie.Checked = True Then
            ChkCache.Checked = False
            ChkPublie.ForeColor = Color.Red
            ChkCache.ForeColor = Color.Black
        End If
    End Sub

    Private Sub ChkCache_CheckedChanged(sender As Object, e As EventArgs) Handles ChkCache.CheckedChanged
        If ChkCache.Checked = True Then
            ChkPublie.Checked = False
            ChkCache.ForeColor = Color.Red
            ChkPublie.ForeColor = Color.Black
        End If
    End Sub

    Private Sub AfficherZonesArret()
        GbxArret.Show()
    End Sub

    Private Sub InhiberZonesDeSaisieArret()
        TxtArretCommentaire.Enabled = False
    End Sub

    'Initialisation des zones de saisie
    Private Sub InitZone()
        Me.ContexteTransformeEnAntecedent = False
        TxtDrcId.Text = ""
        TxtContexteDescription.Text = ""
        DteDateDebut.Value = "01/01/1900"
        DteDateFin.Value = "31/12/2999"
        NumOrdreAffichage.Value = 0
        ChkCache.Checked = False
        ChkPublie.Checked = False
        TxtArretCommentaire.Text = ""
        BtnValider.Hide()
        BtnValidationArret.Hide()
        BtnValidationPublication.Hide()
        BtnRecupereDrc.Hide()
    End Sub

    'Inhiber les zones de saisie
    Private Sub InhiberZonesDeSaisie()
        CbxCategorieContexte.Enabled = False
        TxtDrcId.Enabled = False
        TxtContexteDescription.Enabled = False
        DteDateDebut.Enabled = False
        DteDateFin.Enabled = False
        NumOrdreAffichage.Enabled = False
        ChkCache.Enabled = False
        ChkPublie.Enabled = False
        ChkDiagnosticConfirme.Enabled = False
        ChkDiagnosticSuspecte.Enabled = False
        'ChkDiagnosticSuppose.Enabled = False
        ChkDiagnosticNotion.Enabled = False
        TxtArretCommentaire.Enabled = False
    End Sub

    'Récupère la dénomination pour alimenter la description du contexte
    Private Sub BtnRecupereDrc_Click(sender As Object, e As EventArgs) Handles BtnRecupereDrc.Click
        TxtContexteDescription.Text = LblDrcDenomination.Text
    End Sub

    'Libère le bloc principal pour permettre la saisie des données générales, code DRC, description et date de début
    Private Sub BtnModifier_Click(sender As Object, e As EventArgs) Handles BtnModifier.Click
        BtnArret.Hide()
        BtnValidationArret.Hide()
        BtnPublication.Hide()
        BtnValidationPublication.Hide()
        BtnTransformer.Hide()
        BtnSupprimer.Hide()
        GbxPrincipal.Enabled = True
        TxtDrcId.Enabled = True
        TxtContexteDescription.Enabled = True
        DteDateDebut.Enabled = True
        DteDateFin.Enabled = True
        ChkDiagnosticConfirme.Enabled = True
        ChkDiagnosticSuspecte.Enabled = True
        'ChkDiagnosticSuppose.Enabled = True
        ChkDiagnosticNotion.Enabled = True
        CbxCategorieContexte.Enabled = True
        NumOrdreAffichage.Enabled = True
        BtnValider.Show()
        BtnRecupereDrc.Show()
        BtnModifier.Hide()
    End Sub

    'Libère le bloc de saisie de la publication
    Private Sub BtnPublication_Click(sender As Object, e As EventArgs) Handles BtnPublication.Click
        BtnArret.Hide()
        BtnValidationArret.Hide()
        BtnModifier.Hide()
        BtnValider.Hide()
        BtnTransformer.Hide()
        BtnSupprimer.Hide()
        GbxStatutAffichage.Enabled = True
        ChkCache.Enabled = True
        ChkPublie.Enabled = True
        BtnValidationPublication.Show()
        LblPublication.Hide()
        BtnPublication.Hide()
    End Sub

    'Libère le bloc de saisie de l'arrêt
    Private Sub BtnArret_Click(sender As Object, e As EventArgs) Handles BtnArret.Click
        BtnModifier.Hide()
        BtnValider.Hide()
        BtnPublication.Hide()
        BtnValidationPublication.Hide()
        BtnTransformer.Hide()
        BtnSupprimer.Hide()
        GbxArret.Enabled = True
        TxtArretCommentaire.Enabled = True
        BtnValidationArret.Show()
        BtnArret.Hide()
    End Sub

    'Si l'utilisateur lance le dateTimePicker de la date de fin alors que celle-ci est virtuellement "infinie", on initialise sa valeur avec la date du jour  
    Private Sub DteDateFin_DropDown(sender As Object, e As EventArgs) Handles DteDateFin.DropDown
        Dim DateInfinie As New Date(2999, 12, 31, 0, 0, 0)
        If DteDateFin.Value = DateInfinie Then
            DteDateFin.Value = DteDateDebut.Value
            DteDateFin.Format = DateTimePickerFormat.Long
        End If
    End Sub

    Private Sub LblId_MouseHover(sender As Object, e As EventArgs) Handles LblId.MouseHover
        ToolTip1.SetToolTip(LblId, "Id : " + SelectedContexteId.ToString)
    End Sub

    Private Sub CbxDiagnosticConfirme_CheckedChanged(sender As Object, e As EventArgs) Handles ChkDiagnosticConfirme.CheckedChanged
        If ChkDiagnosticConfirme.Checked = True Then
            ChkDiagnosticNotion.Checked = False
            'ChkDiagnosticSuppose.Checked = False
            ChkDiagnosticSuspecte.Checked = False
            ChkDiagnosticConfirme.ForeColor = Color.Red
            ChkDiagnosticNotion.ForeColor = Color.Black
            'ChkDiagnosticSuppose.ForeColor = Color.Black
            ChkDiagnosticSuspecte.ForeColor = Color.Black
        End If
    End Sub

    Private Sub CbxDiagnosticSuspecte_CheckedChanged(sender As Object, e As EventArgs) Handles ChkDiagnosticSuspecte.CheckedChanged
        If ChkDiagnosticSuspecte.Checked = True Then
            ChkDiagnosticNotion.Checked = False
            'ChkDiagnosticSuppose.Checked = False
            ChkDiagnosticConfirme.Checked = False
            ChkDiagnosticSuspecte.ForeColor = Color.Red
            ChkDiagnosticNotion.ForeColor = Color.Black
            'ChkDiagnosticSuppose.ForeColor = Color.Black
            ChkDiagnosticConfirme.ForeColor = Color.Black
        End If
    End Sub

    Private Sub CbxDiagnosticNotion_CheckedChanged(sender As Object, e As EventArgs) Handles ChkDiagnosticNotion.CheckedChanged
        If ChkDiagnosticNotion.Checked = True Then
            ChkDiagnosticConfirme.Checked = False
            'ChkDiagnosticSuppose.Checked = False
            ChkDiagnosticSuspecte.Checked = False
            ChkDiagnosticNotion.ForeColor = Color.Red
            ChkDiagnosticConfirme.ForeColor = Color.Black
            'ChkDiagnosticSuppose.ForeColor = Color.Black
            ChkDiagnosticSuspecte.ForeColor = Color.Black
        End If
    End Sub
End Class