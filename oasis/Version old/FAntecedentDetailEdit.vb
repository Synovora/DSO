Imports System.Data.SqlClient
Imports Oasis_Common

Public Class FAntecedentDetailEdit
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedAntecedentId As Integer
    Private privateSelectedDrcId As Integer
    Private privateCodeRetour As Boolean
    Private privateReactivation As Boolean

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

    Public Property SelectedAntecedentId As Integer
        Get
            Return privateSelectedAntecedentId
        End Get
        Set(value As Integer)
            privateSelectedAntecedentId = value
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

    Public Property Reactivation As Boolean
        Get
            Return privateReactivation
        End Get
        Set(value As Boolean)
            privateReactivation = value
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
    Dim AntecedentHistoACreer As New AntecedentHisto
    Dim conxn As New SqlConnection(getConnectionString())

    Private Sub FAntecedentDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitZone()
        ChargementEtatCivil()

        If SelectedAntecedentId <> 0 Then
            'Modification
            EditMode = EnumEditMode.Modification
            BtnValidationCreationAntecedent.Hide()
            ChargementAntecedentExistant()
            InhiberZonesDeSaisie()
        Else
            'Création
            EditMode = EnumEditMode.Creation
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
            'Publication
            ChkPublie.Checked = True
            ChkPublie.ForeColor = Color.Red
            LblPublication.Hide()
            'Diagnostic
            ChkDiagnosticConfirme.Checked = True
            ChkDiagnosticConfirme.ForeColor = Color.Red
            'Inhiber les zones d'arrêt
            'Affichage des boutons d'action
            BtnValidationCreationAntecedent.Show()
            BtnRecupereDrc.Show()
            'Inhiber boutons d'action de mise à jour
            BtnModifier.Hide()
            BtnPublication.Hide()
            BtnReactiver.Hide()
            BtnSupprimer.Hide()
            LblCreationAntecedent1.Hide()
            LblCreationAntecedent2.Hide()
            LblAntecedentDateCreation.Hide()
            LblUtilisateurCreation.Hide()
            LblModificationAntecedent1.Hide()
            LblModificationAntecedent2.Hide()
            LblAntecedentDateModification.Hide()
            LblUtilisateurModification.Hide()
            BtnValidationPublication.Hide()
            BtnValider.Hide()
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

    Private Sub ChargementAntecedentExistant()
        Dim antecedentDataReader As SqlDataReader
        Dim conxn As New SqlConnection(getConnectionString())
        Dim SQLString As String = "select * from oasis.oa_antecedent where oa_antecedent_id = " & SelectedAntecedentId & ";"
        Dim myCommand As New SqlCommand(SQLString, conxn)

        Dim dateDebut, dateCreation, dateModification As Date
        Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)

        conxn.Open()
        antecedentDataReader = myCommand.ExecuteReader()
        If antecedentDataReader.Read() Then
            'Code DRC de l'antécédent
            If antecedentDataReader("oa_antecedent_drc_id") Is DBNull.Value Then
                TxtDrcId.Text = ""
            Else
                TxtDrcId.Text = antecedentDataReader("oa_antecedent_drc_id")
            End If

            'Dénomination DRC
            If drcdao.GetDrc(Drc, TxtDrcId.Text) = True Then
                LblDrcDenomination.Text = Drc.DrcLibelle
                LblDrcDenomination.ForeColor = Color.DarkBlue
            Else
                LblDrcDenomination.Text = ""
            End If

            'Description de l'antécédent
            If antecedentDataReader("oa_antecedent_description") Is DBNull.Value Then
                TxtAntecedentDescription.Text = ""
            Else
                TxtAntecedentDescription.Text = antecedentDataReader("oa_antecedent_description")
            End If

            'Récupération de la période de début de l'antecedent
            If antecedentDataReader("oa_antecedent_date_debut") IsNot DBNull.Value Then
                dateDebut = antecedentDataReader("oa_antecedent_date_debut")
            Else
                dateDebut = "31/12/2999"
            End If
            DteDateDebut.Value = dateDebut

            'Statut affichage de l'antécédent
            ChkCache.Checked = False
            ChkOcculte.Checked = False
            ChkPublie.Checked = False
            If antecedentDataReader("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                Dim StatutAffichage As String = antecedentDataReader("oa_antecedent_statut_affichage")
                Select Case StatutAffichage
                    Case "P"
                        ChkPublie.Checked = True
                        ChkPublie.ForeColor = Color.Red
                        LblPublication.Text = "Antécédent publié"
                    Case "C"
                        ChkCache.Checked = True
                        ChkCache.ForeColor = Color.Red
                        LblPublication.Text = "Antécédent caché"
                    Case "O"
                        ChkOcculte.Checked = True
                        ChkOcculte.ForeColor = Color.Red
                        LblPublication.Text = "Antécédent occulté"
                End Select
            End If
            BtnValidationPublication.Hide()

            'Diagnostic
            If antecedentDataReader("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                Dim Diagnostic As Integer = antecedentDataReader("oa_antecedent_diagnostic")
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

            'Création de l'antécédent : date et utilisateur
            If antecedentDataReader("oa_antecedent_date_creation") IsNot DBNull.Value Then
                dateCreation = antecedentDataReader("oa_antecedent_date_creation")
                LblAntecedentDateCreation.Text = dateCreation.ToString("dd.MM.yyyy")
            Else
                LblAntecedentDateCreation.Text = ""
                LblCreationAntecedent1.Hide()
                LblCreationAntecedent2.Hide()
            End If

            LblUtilisateurCreation.Text = ""
            If antecedentDataReader("oa_antecedent_utilisateur_creation") IsNot DBNull.Value Then
                If antecedentDataReader("oa_antecedent_utilisateur_creation") <> 0 Then
                    SetUtilisateur(utilisateurHisto, antecedentDataReader("oa_antecedent_utilisateur_creation"))
                    LblUtilisateurCreation.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
                End If
            Else
                LblCreationAntecedent2.Hide()
            End If

            'Contrôle si on peut traiter la suppression, la suppression est permise si la date de création = date de suppression
            Dim dateCreationaComparer As New Date(dateCreation.Year, dateCreation.Month, dateCreation.Day, 0, 0, 0)
            If antecedentDataReader("oa_antecedent_date_modification") IsNot DBNull.Value Then
                dateModification = antecedentDataReader("oa_antecedent_date_modification")
                LblAntecedentDateModification.Text = dateModification.ToString("dd.MM.yyyy")
            Else
                LblAntecedentDateModification.Text = ""
                LblModificationAntecedent1.Hide()
                LblModificationAntecedent2.Hide()
            End If

            'Contrôle si on peut traiter la réactivation (un antécédent peut ne pas être issu d'un contexte, dans ce cas la catégorie n'est pas renseignée et la réactivation impossible)
            If antecedentDataReader("oa_antecedent_categorie_contexte") Is DBNull.Value Then
                BtnReactiver.Enabled = False
            Else
                If antecedentDataReader("oa_antecedent_categorie_contexte") = "" Then
                    BtnReactiver.Enabled = False
                End If
            End If
        End If

        LblUtilisateurModification.Text = ""
        If antecedentDataReader("oa_antecedent_utilisateur_modification") IsNot DBNull.Value Then
            If antecedentDataReader("oa_antecedent_utilisateur_modification") <> 0 Then
                SetUtilisateur(utilisateurHisto, antecedentDataReader("oa_antecedent_utilisateur_modification"))
                LblUtilisateurModification.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
            Else
                LblModificationAntecedent2.Hide()
            End If
        End If

        'Initialisation classe Historisation antecedent 
        AntecedentHistoCreationDao.InitClasseAntecedentHistorisation(antecedentDataReader, UtilisateurConnecte, AntecedentHistoACreer)

        'Libération des ressources d'accès aux données
        conxn.Close()
        myCommand.Dispose()
    End Sub

    'Suppression (annulation) de l'antécédent
    Private Sub BtnSupprimer_Click(sender As Object, e As EventArgs) Handles BtnSupprimer.Click
        If MsgBox("Attention, la suppression d'un antécédent est irréversible, confirmez-vous l'annulation", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            'Annulation antécédent
            If AnnulationAntecedent() = True Then
                MessageBox.Show("L'antécédent patient a été annulé")
                Me.CodeRetour = True
                Close()
            Else
                Me.CodeRetour = False
            End If
        End If
    End Sub

    'Réactivation de l'antécédent en contexte
    Private Sub BtnReactiver_Click(sender As Object, e As EventArgs) Handles BtnReactiver.Click
        If MsgBox("confirmation de la réactivation", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            'Annulation antécédent
            If ReactivationAntecedent(SelectedAntecedentId) = True Then
                'Traitement des anatécédents liés à réactiver
                TraitementReactivationAntecedentLies(SelectedAntecedentId)
                MessageBox.Show("L'antécédent a été réactivé en contexte médical")
                Me.Reactivation = True
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

    'Lance la validation des données générales, code DRC, description et date de début
    Private Sub BtnValider_Click(sender As Object, e As EventArgs) Handles BtnValider.Click
        'Contrôle des données saisie
        Dim Valide As Boolean = True
        Dim MessageErreur1 As String = ""
        Dim MessageErreur2 As String = ""
        Dim MessageErreur3 As String = ""
        Dim MessageErreur As String = ""

        If TxtDrcId.Text = "" Then
            Valide = False
            MessageErreur1 = "Le code DRC est obligatoire"
        End If

        If TxtAntecedentDescription.Text = "" Then
            Valide = False
            MessageErreur2 = "La description de l'antécédent est obligatoire"
        End If

        If DteDateDebut.Value > Date.Now() Then
            Valide = False
            MessageErreur3 = "La date de début de l'antécédent ne peut pas être supérieure à la date du jour"
        End If

        'Appel de la mise à jour des données
        If Valide = True Then
            If ModificationAntecedent() = True Then
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

                MessageErreur = MessageErreur & vbCrLf & "/!\ Modification de l'antécédent impossible, des données sont incorrectes"
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
            If ChkCache.Checked = True Then
                publication = "C"
            Else
                publication = "O"
            End If
        End If

        If publication = "O" Then
            If MsgBox("Attention, un antécédent occulté ne sera plus accessible, confirmez-vous l'action d'occulter cet antécédent", MsgBoxStyle.YesNo, "") = MsgBoxResult.No Then
                Valide = False
            End If
        End If

        'Appel de la mise à jour des données
        If Valide = True Then
            If ModificationPublicationAntecedent(SelectedAntecedentId, publication) = True Then
                'Traitement des antécédents liés à occulter
                If publication = "O" Then
                    TraitementOccultationAntecedentLies(SelectedAntecedentId)
                End If
                MessageBox.Show("La publication de l'antécédent patient a été modifié")
                Me.CodeRetour = True
                Close()
            Else
                Me.CodeRetour = False
            End If
        End If
    End Sub

    Private Sub BtnValidationCreationAntecedent_Click(sender As Object, e As EventArgs) Handles BtnValidationCreationAntecedent.Click
        Dim Valide As Boolean = True
        Dim MessageErreur1 As String = ""
        Dim MessageErreur2 As String = ""
        Dim MessageErreur3 As String = ""
        Dim MessageErreur As String = ""

        If TxtDrcId.Text = "" Then
            Valide = False
            MessageErreur1 = "Le code DRC est obligatoire"
        End If

        If TxtAntecedentDescription.Text = "" Then
            Valide = False
            MessageErreur2 = "La description de l'antécédent est obligatoire"
        End If

        If DteDateDebut.Value > Date.Now() Then
            Valide = False
            MessageErreur3 = "La date de début de l'antécédent ne peut pas être supérieure à la date du jour"
        End If

        'Appel de la mise à jour des données
        If Valide = True Then
            If CreationAntecedent() = True Then
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

                MessageErreur = MessageErreur & vbCrLf & "/!\ Création de l'antécédent impossible, des données sont incorrectes"
                MessageBox.Show(MessageErreur)
            End If
        End If
    End Sub

    Private Sub TxtDrcId_DoubleClick(sender As Object, e As EventArgs) Handles TxtDrcId.DoubleClick
        'Appel du sélecteur de code DRC
        Dim vFDrcSelecteur As New FDrcSelecteur
        vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
        vFDrcSelecteur.CategorieOasis = 1       'Catégorie "Antécédent et Contexte"
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

    'Modification d'un antécédent en base de données
    Private Function ModificationAntecedent() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_antecedent set oa_antecedent_date_modification = @dateModification, oa_antecedent_utilisateur_modification = @utilisateurModification, oa_antecedent_drc_id = @drcId, oa_antecedent_description = @description, oa_antecedent_date_debut = @dateDebut, oa_antecedent_diagnostic = @diagnostic where oa_antecedent_id = @antecedentId"

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
            .AddWithValue("@description", TxtAntecedentDescription.Text)
            .AddWithValue("@dateDebut", DteDateDebut.Value)
            .AddWithValue("@diagnostic", Diagnostic)
            .AddWithValue("@antecedentId", SelectedAntecedentId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            MessageBox.Show("Antécédent patient modifié")
        Catch ex As Exception
            'PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            AntecedentHistoACreer.HistorisationDate = Date.Now()
            AntecedentHistoACreer.UtilisateurId = UtilisateurConnecte.UtilisateurId
            AntecedentHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent
            AntecedentHistoACreer.DrcId = TxtDrcId.Text
            AntecedentHistoACreer.Description = TxtAntecedentDescription.Text
            AntecedentHistoACreer.DateDebut = DteDateDebut.Value
            AntecedentHistoACreer.Diagnostic = Diagnostic

            'Création dans l'historique des modifications de l'antecedent
            AntecedentHistoCreationDao.CreationAntecedentHisto(AntecedentHistoACreer, UtilisateurConnecte, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent)
        End If

        Return codeRetour
    End Function
    'Modification de la publication d'un antécédent en base de données
    Private Function ModificationPublicationAntecedent(antecedentId As Integer, publication As String) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_antecedent set oa_antecedent_date_modification = @dateModification, oa_antecedent_utilisateur_modification = @utilisateurModification, oa_antecedent_statut_affichage = @publication where oa_antecedent_id = @antecedentId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@publication", publication)
            .AddWithValue("@antecedentId", antecedentId.ToString)
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
            AntecedentHistoACreer.HistorisationDate = Date.Now()
            AntecedentHistoACreer.UtilisateurId = UtilisateurConnecte.UtilisateurId
            AntecedentHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent
            AntecedentHistoACreer.StatutAffichage = publication

            'Création dans l'historique des modifications de l'antecedent
            AntecedentHistoCreationDao.CreationAntecedentHisto(AntecedentHistoACreer, UtilisateurConnecte, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent)
        End If

        Return codeRetour
    End Function

    'Réactivation d'un antécédent en contexte médical
    Private Function ReactivationAntecedent(antecedentId As Integer) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_antecedent set oa_antecedent_type = 'C', oa_antecedent_date_modification = @dateModification, oa_antecedent_utilisateur_modification = @utilisateurModification, oa_antecedent_date_fin = @dateFin, oa_antecedent_nature = @nature, oa_antecedent_priorite = @priorite, oa_antecedent_niveau = @niveau, oa_antecedent_id_niveau1 = @idNiveau1, oa_antecedent_id_niveau2 = @idNiveau2, oa_antecedent_ordre_affichage1 = @ordreAffichage1, oa_antecedent_ordre_affichage2 = @ordreAffichage2, oa_antecedent_ordre_affichage3 = @ordreAffichage3 where oa_antecedent_id = @antecedentId"

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
            .AddWithValue("@antecedentId", antecedentId.ToString)
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
            AntecedentHistoACreer.HistorisationDate = Date.Now()
            AntecedentHistoACreer.Type = "C"
            AntecedentHistoACreer.UtilisateurId = UtilisateurConnecte.UtilisateurId
            AntecedentHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ReactivationAntecedent
            AntecedentHistoACreer.Nature = ""
            AntecedentHistoACreer.Niveau = 1
            AntecedentHistoACreer.Niveau1Id = 0
            AntecedentHistoACreer.Niveau2Id = 0
            AntecedentHistoACreer.Ordre1 = 980
            AntecedentHistoACreer.Ordre2 = 0
            AntecedentHistoACreer.Ordre3 = 0

            'Création dans l'historique des modifications de l'antecedent
            AntecedentHistoCreationDao.CreationAntecedentHisto(AntecedentHistoACreer, UtilisateurConnecte, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ReactivationAntecedent)
        End If

        Return codeRetour
    End Function

    'Annulation d'un antécédent en base de données
    Private Function AnnulationAntecedent() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_antecedent set oa_antecedent_date_modification = @dateModification, oa_antecedent_utilisateur_modification = @utilisateurModification, oa_antecedent_inactif = @inactif where oa_antecedent_id = @antecedentId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@inactif", "1")
            .AddWithValue("@antecedentId", SelectedAntecedentId.ToString)
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
            AntecedentHistoACreer.HistorisationDate = Date.Now()
            AntecedentHistoACreer.UtilisateurId = UtilisateurConnecte.UtilisateurId
            AntecedentHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.AnnulationAntecedent
            AntecedentHistoACreer.Inactif = True

            'Création dans l'historique des modifications de l'antecedent
            AntecedentHistoCreationDao.CreationAntecedentHisto(AntecedentHistoACreer, UtilisateurConnecte, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.AnnulationAntecedent)
        End If

        Return codeRetour
    End Function

    'Création d'un antecedent en base de données
    Private Function CreationAntecedent() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        'Initialisation de la barre de progression
        'PgbMiseAJour.Show()
        'PgbMiseAJour.Style = ProgressBarStyle.Marquee
        'PgbMiseAJour.MarqueeAnimationSpeed = 60

        'Définition publication
        Dim Publication As String
        If ChkCache.Checked = True Then
            Publication = "C"
        Else
            If ChkOcculte.Checked = True Then
                Publication = "O"
            Else
                Publication = "P"
            End If
        End If

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "insert into oasis.oa_antecedent (oa_antecedent_patient_id, oa_antecedent_type, oa_antecedent_drc_id, oa_antecedent_description, oa_antecedent_date_creation, oa_antecedent_utilisateur_creation, oa_antecedent_utilisateur_modification, oa_antecedent_date_debut, oa_antecedent_niveau, oa_antecedent_nature, oa_antecedent_statut_affichage, oa_antecedent_inactif, oa_antecedent_ordre_affichage1, oa_antecedent_ordre_affichage2, oa_antecedent_ordre_affichage3, oa_antecedent_diagnostic) VALUES (@patientId, @type, @drcId, @description, @dateCreation, @utilisateurCreation, @utilisateurModification, @dateDebut, @niveau, @nature, @publication, @inactif, @ordreAffichage1, @ordreAffichage2, @ordreAffichage3, @diagnostic)"

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
            .AddWithValue("@type", "A")
            .AddWithValue("@drcId", TxtDrcId.Text)
            .AddWithValue("@description", TxtAntecedentDescription.Text)
            .AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@utilisateurCreation", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@utilisateurModification", 0)
            .AddWithValue("@dateDebut", DteDateDebut.Value.ToString("yyyy-MM-dd"))
            .AddWithValue("@niveau", 1)
            .AddWithValue("@nature", "Patient")
            .AddWithValue("@publication", Publication)
            .AddWithValue("@inactif", 0)
            .AddWithValue("@ordreAffichage1", 980)
            .AddWithValue("@ordreAffichage2", 0)
            .AddWithValue("@ordreAffichage3", 0)
            .AddWithValue("@diagnostic", Diagnostic)
        End With

        Try
            conxn.Open()
            Dim n As Integer 'Pour récupérer le nombre d'occurences enregistrées
            da.InsertCommand = cmd
            n = da.InsertCommand.ExecuteNonQuery()
            'PgbMiseAJour.Hide()
            MessageBox.Show("Antecedent patient créé")
        Catch ex As Exception
            'PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données dans l'instance de la classe Historisation antecedent
            AntecedentHistoACreer.HistorisationDate = DateTime.Now()
            AntecedentHistoACreer.UtilisateurId = UtilisateurConnecte.UtilisateurId
            AntecedentHistoACreer.Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.CreationAntecedent
            AntecedentHistoACreer.PatientId = SelectedPatient.patientId.ToString
            AntecedentHistoACreer.Type = "A"
            AntecedentHistoACreer.Description = TxtAntecedentDescription.Text
            AntecedentHistoACreer.DateDebut = DteDateDebut.Value.ToString("yyyy-MM-dd")
            AntecedentHistoACreer.Niveau = 1
            AntecedentHistoACreer.Nature = "Patient"
            AntecedentHistoACreer.StatutAffichage = Publication
            AntecedentHistoACreer.Inactif = 0
            AntecedentHistoACreer.Ordre1 = 980
            AntecedentHistoACreer.Ordre2 = 0
            AntecedentHistoACreer.Ordre3 = 0
            AntecedentHistoACreer.Diagnostic = Diagnostic

            'Récupération de l'identifiant du antecedent créé
            Dim antecedentLastDataReader As SqlDataReader
            SQLstring = "select max(oa_antecedent_id) from oasis.oa_antecedent where oa_antecedent_patient_id = " & SelectedPatient.patientId & ";"
            Dim antecedentLastCommand As New SqlCommand(SQLstring, conxn)
            conxn.Open()
            antecedentLastDataReader = antecedentLastCommand.ExecuteReader()
            If antecedentLastDataReader.HasRows Then
                antecedentLastDataReader.Read()
                'Récupération de la clé de l'enregistrement créé
                AntecedentHistoACreer.AntecedentId = antecedentLastDataReader(0)

                'Libération des ressources d'accès aux données
                conxn.Close()
                antecedentLastCommand.Dispose()
            End If

            'Lecture de l'antecedent créé avec toutes ses données pour communiquer le DataReader à la fonction dédiée
            Dim antecedentCreeDataReader As SqlDataReader
            SQLstring = "Select * from oasis.oa_antecedent where oa_antecedent_id = " & AntecedentHistoACreer.AntecedentId & ";"
            Dim antecedentCreeCommand As New SqlCommand(SQLstring, conxn)
            conxn.Open()
            antecedentCreeDataReader = antecedentCreeCommand.ExecuteReader()
            If antecedentCreeDataReader.Read() Then
                'Initialisation classe Historisation antecedent 
                AntecedentHistoCreationDao.InitClasseAntecedentHistorisation(antecedentCreeDataReader, UtilisateurConnecte, AntecedentHistoACreer)

                'Libération des ressources d'accès aux données
                conxn.Close()
                antecedentCreeCommand.Dispose()
            End If

            'Création dans l'historique des antecedents du antecedent créé
            AntecedentHistoCreationDao.CreationAntecedentHisto(AntecedentHistoACreer, UtilisateurConnecte, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.CreationAntecedent)
        End If

        Return codeRetour

    End Function

    'Occultation des antécédents liés à un antécédent réactivé 
    Private Function TraitementOccultationAntecedentLies(AntecedentId As Integer) As Boolean
        Dim codeRetour As Boolean = True
        'Déclaration des données de connexion
        Dim conxn As New SqlConnection(getConnectionString())
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "Select oa_antecedent_id from oasis.oa_antecedent where oa_antecedent_type = 'A' and (oa_antecedent_id_niveau1 = " + AntecedentId.ToString + " or oa_antecedent_id_niveau2 = " + AntecedentId.ToString + ");"

        'Lecture des données en base
        antecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        antecedentDataAdapter.Fill(antecedentDataTable)
        conxn.Open()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1

        'Parcours du DataTable pour réactiver les antécédents liés
        For i = 0 To rowCount Step 1
            'Traitement de réactivation des antécédents liés
            ModificationPublicationAntecedent(antecedentDataTable.Rows(i)("oa_antecedent_id"), "O")
        Next

        'Libération des ressources
        conxn.Close()
        antecedentDataAdapter.Dispose()

        Return codeRetour
    End Function

    'Réactivation des antécédents liés à un antécédent réactivé 
    Private Function TraitementReactivationAntecedentLies(AntecedentId As Integer) As Boolean
        Dim codeRetour As Boolean = True
        'Déclaration des données de connexion
        Dim conxn As New SqlConnection(getConnectionString())
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select oasis.oa_antecedent_id from oa_antecedent where oa_antecedent_type = 'A' and (oa_antecedent_id_niveau1 = " + AntecedentId.ToString + " or oa_antecedent_id_niveau2 = " + AntecedentId.ToString + ");"

        'Lecture des données en base
        antecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        antecedentDataAdapter.Fill(antecedentDataTable)
        conxn.Open()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1

        'Parcours du DataTable pour réactiver les antécédents liés
        For i = 0 To rowCount Step 1
            'Traitement de réactivation des antécédents liés
            ReactivationAntecedent(antecedentDataTable.Rows(i)("oa_antecedent_id"))
        Next

        'Libération des ressources
        conxn.Close()
        antecedentDataAdapter.Dispose()

        Return codeRetour
    End Function

    '=============================================================================================
    '==================================== Gestion de l'affichage des zones d'écran ===============
    '=============================================================================================
    Private Sub ChkPublie_CheckedChanged(sender As Object, e As EventArgs) Handles ChkPublie.CheckedChanged
        If ChkPublie.Checked = True Then
            ChkCache.Checked = False
            ChkOcculte.Checked = False
            ChkPublie.ForeColor = Color.Red
            ChkCache.ForeColor = Color.Black
            ChkOcculte.ForeColor = Color.Black
            If GbxStatutAffichage.Enabled = True And EditMode = EnumEditMode.Modification Then
                BtnValidationPublication.Show()
            End If
        End If
    End Sub

    Private Sub ChkCache_CheckedChanged(sender As Object, e As EventArgs) Handles ChkCache.CheckedChanged
        If ChkCache.Checked = True Then
            ChkPublie.Checked = False
            ChkOcculte.Checked = False
            ChkCache.ForeColor = Color.Red
            ChkPublie.ForeColor = Color.Black
            ChkOcculte.ForeColor = Color.Black
            If GbxStatutAffichage.Enabled = True And EditMode = EnumEditMode.Modification Then
                BtnValidationPublication.Show()
            End If
        End If
    End Sub

    Private Sub ChkOcculte_CheckedChanged(sender As Object, e As EventArgs) Handles ChkOcculte.CheckedChanged
        If ChkOcculte.Checked = True Then
            ChkPublie.Checked = False
            ChkCache.Checked = False
            ChkOcculte.ForeColor = Color.Red
            ChkPublie.ForeColor = Color.Black
            ChkCache.ForeColor = Color.Black
            If GbxStatutAffichage.Enabled = True And EditMode = EnumEditMode.Modification Then
                BtnValidationPublication.Show()
            End If
        End If
    End Sub

    'Initialisation des zones de saisie
    Private Sub InitZone()
        Me.Reactivation = False
        TxtDrcId.Text = ""
        TxtAntecedentDescription.Text = ""
        DteDateDebut.Value = "01/01/1900"
        ChkCache.Checked = False
        ChkOcculte.Checked = False
        ChkPublie.Checked = False
        BtnValidationPublication.Hide()
        BtnRecupereDrc.Hide()
    End Sub

    'Inhiber les zones de saisie
    Private Sub InhiberZonesDeSaisie()
        TxtDrcId.Enabled = False
        TxtAntecedentDescription.Enabled = False
        DteDateDebut.Enabled = False
        ChkCache.Enabled = False
        ChkOcculte.Enabled = False
        ChkPublie.Enabled = False
        ChkDiagnosticConfirme.Enabled = False
        ChkDiagnosticSuspecte.Enabled = False
        'ChkDiagnosticSuppose.Enabled = False
        ChkDiagnosticNotion.Enabled = False
        BtnValider.Hide()
    End Sub

    'Récupère la dénomination pour alimenter la description de l'antécédent
    Private Sub BtnRecupereDrc_Click(sender As Object, e As EventArgs) Handles BtnRecupereDrc.Click
        TxtAntecedentDescription.Text = LblDrcDenomination.Text
    End Sub

    'Libère le bloc principal pour permettre la saisie des données générales, code DRC, description et date de début
    Private Sub BtnModifier_Click(sender As Object, e As EventArgs) Handles BtnModifier.Click
        BtnPublication.Hide()
        BtnValidationPublication.Hide()
        BtnReactiver.Hide()
        BtnSupprimer.Hide()
        GbxPrincipal.Enabled = True
        TxtDrcId.Enabled = True
        TxtAntecedentDescription.Enabled = True
        DteDateDebut.Enabled = True
        ChkDiagnosticConfirme.Enabled = True
        ChkDiagnosticSuspecte.Enabled = True
        'ChkDiagnosticSuppose.Enabled = True
        ChkDiagnosticNotion.Enabled = True
        'BtnValider.Show()
        BtnRecupereDrc.Show()
        BtnModifier.Hide()
    End Sub

    'Libère le bloc de saisie de la publication
    Private Sub BtnPublication_Click(sender As Object, e As EventArgs) Handles BtnPublication.Click
        BtnModifier.Hide()
        BtnValider.Hide()
        BtnReactiver.Hide()
        BtnSupprimer.Hide()
        GbxStatutAffichage.Enabled = True
        ChkCache.Enabled = True
        ChkOcculte.Enabled = True
        ChkPublie.Enabled = True
        'BtnValidationPublication.Show()
        LblPublication.Hide()
        BtnPublication.Hide()
    End Sub

    Private Sub DteDateDebut_ValueChanged(sender As Object, e As EventArgs) Handles DteDateDebut.ValueChanged
        If EditMode = EnumEditMode.Modification Then
            BtnValider.Show()
        End If
    End Sub

    Private Sub TxtDrcId_TextChanged(sender As Object, e As EventArgs) Handles TxtDrcId.TextChanged
        If EditMode = EnumEditMode.Modification Then
            BtnValider.Show()
        End If
    End Sub

    Private Sub TxtAntecedentDescription_TextChanged(sender As Object, e As EventArgs) Handles TxtAntecedentDescription.TextChanged
        If EditMode = EnumEditMode.Modification Then
            BtnValider.Show()
        End If
    End Sub

    Private Sub LblId_MouseHover(sender As Object, e As EventArgs) Handles LblId.MouseHover
        ToolTip1.SetToolTip(LblId, "Id : " + SelectedAntecedentId.ToString)
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
            If EditMode = EnumEditMode.Modification Then
                BtnValider.Show()
            End If
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
            If EditMode = EnumEditMode.Modification Then
                BtnValider.Show()
            End If
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
            If EditMode = EnumEditMode.Modification Then
                BtnValider.Show()
            End If
        End If
    End Sub
End Class