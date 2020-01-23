Imports System.Data.SqlClient
Imports Oasis_Common

Public Class FPPSStrategie
    Private _SelectedPatient As Patient
    Private _UtilisateurConnecte As Utilisateur
    Private _StrategieId As Integer
    Private _codeRetour As Boolean

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

    Public Property StrategieId As Integer
        Get
            Return _StrategieId
        End Get
        Set(value As Integer)
            _StrategieId = value
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

    Enum EnumEditMode
        Creation = 1
        Modification = 2
    End Enum

    Dim conxn As New SqlConnection(getConnectionString())
    Dim ObjectifExiste As Boolean = False
    Dim PPSHistoACreer As New PpsHisto
    Dim EditMode As Integer
    Dim UtilisateurHisto As Utilisateur = New Utilisateur()
    Dim drcdao As New DrcDao


    Private Sub FPPStrategieContextuelle_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementEtatCivil()
        BtnConfirmationAnnulation.Hide()
        TxtCommentaireArret.Hide()
        LblLabelCommentaireArret.Hide()
        If StrategieId <> 0 Then
            EditMode = EnumEditMode.Modification
            ChargementStrategieContextuelle()
        Else
            EditMode = EnumEditMode.Creation
            InitZonesEnSaisie()
            BtnAnnulation.Hide()
            'Cacher les éléments de création de l'occurrence
            LblLabelStrategieDateModification.Hide()
            LblStrategieDateModification.Hide()
            LblLabelStrategieParModification.Hide()
            LblUtilisateurModification.Hide()
            'Cacher les éléments de modification de l'occurrence
            LblLabelStrategieDateCreation.Hide()
            LblStrategieDateCreation.Hide()
            LblLabelStrategieParCreation.Hide()
            LblUtilisateurCreation.Hide()
            'Cacher les boutons d'action relatifs à l'annulation
            BtnAnnulation.Hide()
        End If
        CodeRetour = False
    End Sub

    Private Sub InitZonesEnSaisie()
        TxtDrcId.Text = ""
        TxtCommentaire.Text = ""
        CbxTypeStrategie.Text = ""
    End Sub

    'Chargement des données dans les labels dédiés
    Private Sub ChargementEtatCivil()
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
        'Recherche si ALD
        LblALD.Hide()
        'Vérification de l'existence d'ALD
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTipPPS.SetToolTip(LblALD, StringTooltip)
        End If
    End Sub

    Private Sub ChargementStrategieContextuelle()
        Dim StrategieDateReader As SqlDataReader
        Dim conxn As New SqlConnection(getConnectionString())
        Dim SQLString As String = "select * from oasis.oa_patient_pps where oa_pps_id = " + StrategieId.ToString + ";"
        Dim myCommand As New SqlCommand(SQLString, conxn)
        Dim dateCreation, dateModification As Date
        Dim TypeStrategie As Integer

        conxn.Open()
        StrategieDateReader = myCommand.ExecuteReader()
        If StrategieDateReader.Read() Then
            TxtCommentaire.Text = ""
            If StrategieDateReader("oa_pps_commentaire") IsNot DBNull.Value Then
                TxtCommentaire.Text = StrategieDateReader("oa_pps_commentaire")
            End If

            TypeStrategie = 0
            If StrategieDateReader("oa_pps_sous_categorie") IsNot DBNull.Value Then
                TypeStrategie = StrategieDateReader("oa_pps_sous_categorie")
            End If
            Select Case TypeStrategie
                Case 7
                    CbxTypeStrategie.Text = "Prophylactique"
                Case 8
                    CbxTypeStrategie.Text = "Sociale"
                Case 9
                    CbxTypeStrategie.Text = "Symptomatique"
                Case 10
                    CbxTypeStrategie.Text = "Curative"
                Case 11
                    CbxTypeStrategie.Text = "Diagnostique"
                Case 12
                    CbxTypeStrategie.Text = "Palliative"
                Case Else
                    CbxTypeStrategie.Text = ""
            End Select

            TxtDrcId.Text = ""
            If StrategieDateReader("oa_pps_drc_id") IsNot DBNull.Value Then
                TxtDrcId.Text = StrategieDateReader("oa_pps_drc_id")
                Dim Drc As Drc = New Drc()
                If drcdao.GetDrc(Drc, TxtDrcId.Text) = True Then
                    TxtDrcDescription.Text = Drc.DrcLibelle
                Else
                    TxtDrcDescription.Text = ""
                End If
            End If

            If StrategieDateReader("oa_pps_date_creation") IsNot DBNull.Value Then
                dateCreation = StrategieDateReader("oa_pps_date_creation")
                LblStrategieDateCreation.Text = dateCreation.ToString("dd.MM.yyyy")
            Else
                LblStrategieDateCreation.Text = ""
                LblLabelStrategieDateCreation.Hide()
                LblLabelStrategieParCreation.Hide()
            End If

            LblUtilisateurCreation.Text = ""
            If StrategieDateReader("oa_pps_utilisateur_creation") IsNot DBNull.Value Then
                If StrategieDateReader("oa_pps_utilisateur_creation") <> 0 Then
                    SetUtilisateur(UtilisateurHisto, StrategieDateReader("oa_pps_utilisateur_creation"))
                    LblUtilisateurCreation.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
                End If
            End If

            If StrategieDateReader("oa_pps_date_modification") IsNot DBNull.Value Then
                dateModification = StrategieDateReader("oa_pps_date_modification")
                LblStrategieDateModification.Text = dateModification.ToString("dd.MM.yyyy")
            Else
                LblStrategieDateModification.Text = ""
                LblLabelStrategieDateModification.Hide()
                LblLabelStrategieParModification.Hide()
            End If

            LblUtilisateurModification.Text = ""
            If StrategieDateReader("oa_pps_utilisateur_modification") IsNot DBNull.Value Then
                If StrategieDateReader("oa_pps_utilisateur_modification") <> 0 Then
                    SetUtilisateur(UtilisateurHisto, StrategieDateReader("oa_pps_utilisateur_modification"))
                    LblUtilisateurModification.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
                End If
            End If
        End If

        BtnValidation.Hide()

        'Libération des ressources d'accès aux données
        conxn.Close()
        myCommand.Dispose()

    End Sub
    Private Sub LblALD_Click(sender As Object, e As EventArgs) Handles LblALD.Click
        Dim vFPatientAldListe As New FPatientAldListe
        vFPatientAldListe.SelectedPatient = Me.SelectedPatient
        vFPatientAldListe.SelectedPatientId = Me.SelectedPatient.patientId
        vFPatientAldListe.UtilisateurConnecte = Me.UtilisateurConnecte
        vFPatientAldListe.ShowDialog() 'Modal
        vFPatientAldListe.Dispose()
    End Sub

    Private Sub TxtDrcId_DoubleClick(sender As Object, e As EventArgs) Handles TxtDrcId.DoubleClick
        AppelDrcSelecteur()
    End Sub

    Private Sub BtnDrcSelecteur_Click(sender As Object, e As EventArgs) Handles BtnDrcSelecteur.Click
        AppelDrcSelecteur()
    End Sub

    Private Sub AppelDrcSelecteur()
        Dim vFDrcSelecteur As New FDrcSelecteur
        vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
        vFDrcSelecteur.CategorieOasis = 2 'Catégorie Oasis "Stratégie"
        vFDrcSelecteur.ShowDialog() 'Modal
        Dim SelectedDrcId As Integer = vFDrcSelecteur.SelectedDrcId
        vFDrcSelecteur.Dispose()
        'Si un médicament a été sélectionné
        If SelectedDrcId <> 0 Then
            TxtDrcId.Text = SelectedDrcId
            Dim Drc As Drc = New Drc()
            If drcdao.GetDrc(Drc, SelectedDrcId) = True Then
                TxtDrcDescription.Text = Drc.DrcLibelle
            Else
                TxtDrcDescription.Text = ""
            End If
        End If
    End Sub

    Private Sub BtnValidation_Click(sender As Object, e As EventArgs) Handles BtnValidation.Click
        If ControleValidationDonnees() = True Then
            If StrategieId <> 0 Then
                'Modification
                If ModificationStrategie() = True Then
                    Me.CodeRetour = True
                    Close()
                End If
            Else
                'Création
                If CreationStrategie() = True Then
                    Me.CodeRetour = True
                    Close()
                End If
            End If
            BtnValidation.Hide()
        End If
    End Sub


    Private Function ControleValidationDonnees() As Boolean
        Dim ValidationDonnees As Boolean = True

        Dim MessageErreur1 As String = ""
        Dim MessageErreur2 As String = ""
        Dim MessageErreur3 As String = ""
        Dim MessageErreur4 As String = ""
        Dim MessageErreur As String = ""

        If TxtDrcId.Text = "" Then
            ValidationDonnees = False
            MessageErreur1 = "Le code DRC est obligatoire"
        End If

        'Appel de la mise à jour des données
        'Préparation de l'affichage des erreurs
        If ValidationDonnees = False Then
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

        Return ValidationDonnees
    End Function
    Private Function CreationStrategie() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "insert into oasis.oa_patient_pps (oa_pps_patient_id, oa_pps_categorie, oa_pps_sous_categorie, oa_pps_specialite, oa_pps_drc_id, oa_pps_commentaire, oa_pps_utilisateur_creation, oa_pps_date_creation, oa_pps_affichage_synthese) VALUES (@patientId, @categorie, @sousCategorie, @specialite, @drcId, @commentaire, @utilisateurCreation, @dateCreation, @affichageSynthese)"
        Dim cmd As New SqlCommand(SQLstring, conxn)


        With cmd.Parameters
            .AddWithValue("@patientId", SelectedPatient.patientId.ToString)
            .AddWithValue("@categorie", 3)
            .AddWithValue("@sousCategorie", DeterminationTypeStrategie().ToString)
            .AddWithValue("@specialite", 0)
            .AddWithValue("@drcId", TxtDrcId.Text)
            .AddWithValue("@commentaire", TxtCommentaire.Text)
            .AddWithValue("@utilisateurCreation", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateCreation", Date.Now.ToString)
            .AddWithValue("@affichageSynthese", 1)
        End With

        Try
            conxn.Open()
            Dim n As Integer 'Pour récupérer le nombre d'occurences enregistrées
            da.InsertCommand = cmd
            n = da.InsertCommand.ExecuteNonQuery()
            MessageBox.Show("Stratégie contextuelle créée")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données dans l'instance de la classe Historisation antecedent
            PPSHistoACreer.HistorisationDate = DateTime.Now()
            PPSHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
            PPSHistoACreer.HistorisationEtat = EnumEtatPPSHisto.Creation
            PPSHistoACreer.PatientId = SelectedPatient.patientId.ToString
            PPSHistoACreer.Categorie = 3
            PPSHistoACreer.SousCategorie = DeterminationTypeStrategie()
            PPSHistoACreer.DrcId = TxtDrcId.Text
            PPSHistoACreer.Commentaire = TxtCommentaire.Text
            PPSHistoACreer.Inactif = 0
            PPSHistoACreer.AffichageSynthese = 1

            'Récupération de l'identifiant du antecedent créé
            Dim PPSLastDataReader As SqlDataReader
            SQLstring = "select max(oa_pps_id) from oasis.oa_patient_pps where oa_pps_patient_id = " & SelectedPatient.patientId & ";"
            Dim PPSLastCommand As New SqlCommand(SQLstring, conxn)
            conxn.Open()
            PPSLastDataReader = PPSLastCommand.ExecuteReader()
            If PPSLastDataReader.HasRows Then
                PPSLastDataReader.Read()
                'Récupération de la clé de l'enregistrement créé
                PPSHistoACreer.PpsId = PPSLastDataReader(0)

                'Libération des ressources d'accès aux données
                conxn.Close()
                PPSLastCommand.Dispose()
            End If

            'Lecture de la stratégie créée avec toutes ses données pour communiquer le DataReader à la fonction dédiée
            Dim PPSCreeDataReader As SqlDataReader
            SQLstring = "Select * from oasis.oa_patient_pps where oa_pps_id = " & PPSHistoACreer.PpsId & ";"
            Dim antecedentCreeCommand As New SqlCommand(SQLstring, conxn)
            conxn.Open()
            PPSCreeDataReader = antecedentCreeCommand.ExecuteReader()
            If PPSCreeDataReader.Read() Then
                'Initialisation classe Historisation PPS
                InitClassePPStHistorisation(PPSCreeDataReader, UtilisateurConnecte, PPSHistoACreer)

                'Libération des ressources d'accès aux données
                conxn.Close()
                antecedentCreeCommand.Dispose()
            End If

            'Création dans l'historique des antecedents du antecedent créé
            CreationPPSHisto(PPSHistoACreer, UtilisateurConnecte, AntecedentHistoCreationDao.EnumEtatAntecedentHisto.CreationAntecedent)
        End If

        Return codeRetour
    End Function

    Private Function ModificationStrategie() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_patient_pps set oa_pps_date_modification = @dateModification, oa_pps_utilisateur_modification = @utilisateurModification, oa_pps_sous_categorie = @sousCategorie, oa_pps_drc_id = @drcId, oa_pps_commentaire = @commentaire where oa_pps_id = @ppsId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@sousCategorie", DeterminationTypeStrategie().ToString)
            .AddWithValue("@commentaire", TxtCommentaire.Text)
            .AddWithValue("@drcId", TxtDrcId.Text)
            .AddWithValue("@ppsId", StrategieId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            MessageBox.Show("Stratégie contextuelle modifiée")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            PPSHistoACreer.HistorisationDate = Date.Now()
            PPSHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
            PPSHistoACreer.HistorisationEtat = EnumEtatPPSHisto.Modification
            PPSHistoACreer.Categorie = 3
            PPSHistoACreer.SousCategorie = DeterminationTypeStrategie()
            PPSHistoACreer.PatientId = SelectedPatient.patientId.ToString
            PPSHistoACreer.PpsId = StrategieId.ToString
            PPSHistoACreer.Commentaire = TxtCommentaire.Text
            PPSHistoACreer.Inactif = 0
            PPSHistoACreer.AffichageSynthese = 1
            PPSHistoACreer.DrcId = TxtDrcId.Text

            'Création dans l'historique des modifications de l'antecedent
            CreationPPSHisto(PPSHistoACreer, UtilisateurConnecte, EnumEtatPPSHisto.Modification)
        End If

        Return codeRetour
    End Function


    Private Function AnnulationStrategie() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_patient_pps set oa_pps_date_modification = @dateModification, oa_pps_utilisateur_modification = @utilisateurModification, oa_pps_inactif = @inactif, oa_pps_arret = @arret, oa_pps_commentaire_arret = @commentaireArret where oa_pps_id = @ppsId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@inactif", 1)
            .AddWithValue("@ppsId", StrategieId.ToString)
            .AddWithValue("@arret", 1)
            .AddWithValue("@commentaireArret", TxtCommentaireArret.Text)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            MessageBox.Show("Stratégie contextuelle annulée")
        Catch ex As Exception
            'PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            PPSHistoACreer.HistorisationDate = Date.Now()
            PPSHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
            PPSHistoACreer.HistorisationEtat = EnumEtatPPSHisto.Annulation
            PPSHistoACreer.Categorie = 3
            PPSHistoACreer.SousCategorie = DeterminationTypeStrategie()
            PPSHistoACreer.PatientId = SelectedPatient.patientId.ToString
            PPSHistoACreer.PpsId = StrategieId.ToString
            PPSHistoACreer.Commentaire = TxtCommentaire.Text
            PPSHistoACreer.Arret = 1
            PPSHistoACreer.ArretCommentaire = TxtCommentaireArret.Text
            PPSHistoACreer.Inactif = 1
            PPSHistoACreer.DrcId = TxtDrcId.Text

            'Création dans l'historique des modifications de l'antecedent
            CreationPPSHisto(PPSHistoACreer, UtilisateurConnecte, EnumEtatPPSHisto.Annulation)
        End If

        Return codeRetour
    End Function

    Private Function DeterminationTypeStrategie() As Integer
        Dim typeStrategie As Integer

        Select Case CbxTypeStrategie.Text
            Case "Prophylactique"
                typeStrategie = 7
            Case "Sociale"
                typeStrategie = 8
            Case "Symptomatique"
                typeStrategie = 9
            Case "Curative"
                typeStrategie = 10
            Case "Diagnostique"
                typeStrategie = 11
            Case "Palliative"
                typeStrategie = 12
            Case Else
                typeStrategie = 0
        End Select

        Return typeStrategie

    End Function

    Private Sub BtnAbandonner_Click(sender As Object, e As EventArgs) Handles BtnAbandonner.Click
        Close()
    End Sub

    Private Sub BtnAnnulation_Click(sender As Object, e As EventArgs) Handles BtnAnnulation.Click
        TxtCommentaireArret.Show()
        LblLabelCommentaireArret.Show()
        BtnConfirmationAnnulation.Show()
        BtnAnnulation.Hide()
        BtnValidation.Hide()
        TxtDrcId.Enabled = False
        TxtCommentaire.Enabled = False
        CbxTypeStrategie.Enabled = False
    End Sub

    'Confirmation de l'annulation de la stratégie
    Private Sub BtnConfirmationAnnulation_Click(sender As Object, e As EventArgs) Handles BtnConfirmationAnnulation.Click
        If TxtCommentaireArret.Text = "" Then
            MessageBox.Show("La saisie du commentaire d'arrêt est obligatoire pour annuler une stratégie contextuelle")
        Else
            If MsgBox("Confirmation de l'annulation de la stratégie contextuelle", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
                'Annulation antécédent
                If AnnulationStrategie() = True Then
                    Me.CodeRetour = True
                    Close()
                End If
            End If
        End If
    End Sub

    Private Sub TxtDrcId_TextChanged(sender As Object, e As EventArgs) Handles TxtDrcId.TextChanged
        BtnValidation.Show()
    End Sub

    Private Sub TxtCommentaire_TextChanged(sender As Object, e As EventArgs) Handles TxtCommentaire.TextChanged
        BtnValidation.Show()
    End Sub

    Private Sub CbxTypeStrategie_TextChanged(sender As Object, e As EventArgs) Handles CbxTypeStrategie.TextChanged
        BtnValidation.Show()
    End Sub

    Private Sub LblId_MouseHover(sender As Object, e As EventArgs) Handles LblId.MouseHover
        ToolTipPPS.SetToolTip(LblId, "Id : " + StrategieId.ToString)
    End Sub
End Class