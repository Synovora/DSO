Imports System.Data.SqlClient

Public Class RadFPPSDetailEdit
    Private _SelectedPatient As Patient
    Private _UtilisateurConnecte As Utilisateur
    Private _CategoriePPS As Integer
    Private _PPSId As Integer
    Private _codeRetour As Boolean
    Private _positionGaucheDroite As Integer

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

    Public Property PPSId As Integer
        Get
            Return _PPSId
        End Get
        Set(value As Integer)
            _PPSId = value
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

    Public Property CategoriePPS As Integer
        Get
            Return _CategoriePPS
        End Get
        Set(value As Integer)
            _CategoriePPS = value
        End Set
    End Property

    Public Property PositionGaucheDroite As Integer
        Get
            Return _positionGaucheDroite
        End Get
        Set(value As Integer)
            _positionGaucheDroite = value
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
    Dim PPSDesignation As String
    Dim SousCategoriePPs As Integer
    Dim drcdao As New DrcDao

    Private Sub RadFPPSMesurePreventive_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If PositionGaucheDroite = EnumPosition.Droite Then
            Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - Me.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        Else
            Me.Location = New Point(10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        End If

        Select Case CategoriePPS
            Case EnumCategoriePPS.MesurePreventive
                Me.Text = "Mesure préventive"
                PPSDesignation = "Mesure préventive"
                SousCategoriePPs = 2
                CbxTypeStrategie.Hide()
                LblTypeStrategie.Hide()
            Case EnumCategoriePPS.Objectif
                Me.Text = "Objectif"
                PPSDesignation = "Objectif"
                SousCategoriePPs = 1
                NumPriorite.Hide()
                LblPriorite.Hide()
                CbxTypeStrategie.Hide()
                LblTypeStrategie.Hide()
            Case EnumCategoriePPS.Strategie
                Me.Text = "Stratégie contextuelle"
                PPSDesignation = "Stratégie contextuelle"
                NumPriorite.Hide()
                LblPriorite.Hide()
        End Select
        Dim conxn As New SqlConnection(getConnectionString())
        ChargementEtatCivil()
        RadBtnConfirmationAnnulation.Hide()
        TxtCommentaireArret.Hide()
        LblLabelCommentaireArret.Hide()
        RadGbxAnnulation.Hide()

        If PPSId <> 0 Then
            EditMode = EnumEditMode.Modification
            ChargementPPS()
        Else
            EditMode = EnumEditMode.Creation
            InitZonesEnSaisie()
            RadBtnAnnulation.Hide()
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
        End If
        CodeRetour = False

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub InitZonesEnSaisie()
        TxtDrcId.Text = ""
        TxtCommentaire.Text = ""
        NumPriorite.Value = 0
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

    Private Sub ChargementPPS()
        Dim ppsDao As New PpsDao
        Dim pps As Pps = ppsDao.getPpsById(PPSId)
        Dim dateCreation, dateModification As Date

        TxtCommentaire.Text = pps.Commentaire

        TxtDrcId.Text = pps.DrcId
        Dim Drc As Drc = New Drc()
        If drcdao.GetDrc(Drc, TxtDrcId.Text) = True Then
            TxtDrcDescription.Text = Drc.DrcLibelle
        Else
            TxtDrcDescription.Text = ""
        End If

        SousCategoriePPs = pps.SousCategorieId
        Select Case pps.CategorieId
            Case EnumCategoriePPS.Objectif
            Case EnumCategoriePPS.MesurePreventive
            Case EnumCategoriePPS.Strategie
                Select Case pps.SousCategorieId
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
        End Select

        NumPriorite.Value = pps.Priorite

        If pps.DateCreation <> Nothing Then
            dateCreation = pps.DateCreation
            LblStrategieDateCreation.Text = dateCreation.ToString("dd.MM.yyyy")
        Else
            LblStrategieDateCreation.Text = ""
            LblLabelStrategieDateCreation.Hide()
            LblLabelStrategieParCreation.Hide()
        End If

        LblUtilisateurCreation.Text = ""
        If pps.UserCreation <> 0 Then
            SetUtilisateur(UtilisateurHisto, pps.UserCreation)
            LblUtilisateurCreation.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
        End If

        If pps.DateModification <> Nothing Then
            dateModification = pps.DateModification
            LblStrategieDateModification.Text = dateModification.ToString("dd.MM.yyyy")
        Else
            LblStrategieDateModification.Text = ""
            LblLabelStrategieDateModification.Hide()
            LblLabelStrategieParModification.Hide()
        End If

        LblUtilisateurModification.Text = ""
        If pps.UserModification <> 0 Then
            SetUtilisateur(UtilisateurHisto, pps.UserModification)
            LblUtilisateurModification.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
        End If

        RadBtnValidation.Hide()
    End Sub

    Private Sub TxtDrcId_DoubleClick(sender As Object, e As EventArgs) Handles TxtDrcId.DoubleClick
        AppelDrcSelecteur()
    End Sub
    Private Sub RadBtnDrcSelecteur_Click(sender As Object, e As EventArgs) Handles RadBtnDrcSelecteur.Click
        AppelDrcSelecteur()
    End Sub

    Private Sub AppelDrcSelecteur()
        Dim CategorieOasis As Integer
        CategorieOasis = drcdao.GetCategorieOasisByCategoriePPS(CategoriePPS)

        Using vFDrcSelecteur As New RadFDRCSelecteur
            vFDrcSelecteur.SelectedPatient = Me.SelectedPatient
            vFDrcSelecteur.CategorieOasis = CategorieOasis
            vFDrcSelecteur.ShowDialog() 'Modal
            Dim SelectedDrcId As Integer = vFDrcSelecteur.SelectedDrcId
            vFDrcSelecteur.Dispose()        'Si un médicament a été sélectionné
            If SelectedDrcId <> 0 Then
                TxtDrcId.Text = SelectedDrcId
                Dim Drc As Drc = New Drc()
                If drcdao.GetDrc(Drc, SelectedDrcId) = True Then
                    TxtDrcDescription.Text = Drc.DrcLibelle
                Else
                    TxtDrcDescription.Text = ""
                End If
            End If
        End Using
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        If ControleValidationDonnees() = True Then
            If PPSId <> 0 Then
                'Modification
                If ModificationPPS() = True Then
                    Me.CodeRetour = True
                    Close()
                End If
            Else
                'Création
                If CreationPPS() = True Then
                    Me.CodeRetour = True
                    Close()
                End If
            End If
            RadBtnValidation.Hide()
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

        If CategoriePPS = EnumCategoriePPS.Strategie Then
            SousCategoriePPs = DeterminationTypeStrategie()
            If SousCategoriePPs = 0 Then
                ValidationDonnees = False
                MessageErreur2 = "La sélection du type de la stratégie contextuelle doit être sélectionnée"
            End If
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

            MessageErreur = MessageErreur & vbCrLf & "/!\ Création " & PPSDesignation & " impossible, des données sont incorrectes"
            MessageBox.Show(MessageErreur)
        End If

        Return ValidationDonnees
    End Function
    Private Function CreationPPS() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "insert into oasis.oa_patient_pps" &
        " (oa_pps_patient_id, oa_pps_categorie, oa_pps_sous_categorie, oa_pps_priorite, oa_pps_drc_id, oa_pps_commentaire," &
        " oa_pps_utilisateur_creation, oa_pps_date_creation, oa_pps_affichage_synthese)" &
        " VALUES (@patientId, @categorie, @sousCategorie, @priorite, @drcId, @commentaire, @utilisateurCreation, @dateCreation, @affichageSynthese)"
        Dim cmd As New SqlCommand(SQLstring, conxn)

        If CategoriePPS = EnumCategoriePPS.Strategie Then
            SousCategoriePPs = DeterminationTypeStrategie()
        End If

        With cmd.Parameters
            .AddWithValue("@patientId", SelectedPatient.patientId.ToString)
            .AddWithValue("@categorie", CategoriePPS)
            .AddWithValue("@sousCategorie", SousCategoriePPs)
            .AddWithValue("@priorite", NumPriorite.Value.ToString)
            .AddWithValue("@drcId", TxtDrcId.Text)
            .AddWithValue("@commentaire", TxtCommentaire.Text)
            .AddWithValue("@utilisateurCreation", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateCreation", Date.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@affichageSynthese", 1)
        End With

        Try
            conxn.Open()
            Dim n As Integer 'Pour récupérer le nombre d'occurences enregistrées
            da.InsertCommand = cmd
            n = da.InsertCommand.ExecuteNonQuery()
            MessageBox.Show(PPSDesignation & " créée")
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
            PPSHistoACreer.Categorie = CategoriePPS
            PPSHistoACreer.SousCategorie = SousCategoriePPs
            PPSHistoACreer.Priorite = NumPriorite.Value
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

            'Lecture de l'antecedent créé avec toutes ses données pour communiquer le DataReader à la fonction dédiée
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

            'Création dans l'historique des PPS du PPS créé
            CreationPPSHisto(PPSHistoACreer, UtilisateurConnecte, PPSHistoCreationDao.EnumEtatPPSHisto.Creation)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            PatientDao.ModificationDateMajSynthesePatient(SelectedPatient.patientId)
        End If

        Return codeRetour
    End Function

    Private Function ModificationPPS() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_patient_pps set oa_pps_sous_categorie = @sousCategorie, oa_pps_date_modification = @dateModification," &
        " oa_pps_utilisateur_modification = @utilisateurModification, oa_pps_priorite = @priorite, oa_pps_drc_id = @drcId," &
        " oa_pps_commentaire = @commentaire where oa_pps_id = @ppsId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        If CategoriePPS = EnumCategoriePPS.Strategie Then
            SousCategoriePPs = DeterminationTypeStrategie()
        End If

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@sousCategorie", SousCategoriePPs)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@priorite", NumPriorite.Value.ToString)
            .AddWithValue("@commentaire", TxtCommentaire.Text)
            .AddWithValue("@drcId", TxtDrcId.Text)
            .AddWithValue("@ppsId", PPSId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            MessageBox.Show(PPSDesignation & " modifiée")
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
            PPSHistoACreer.Categorie = CategoriePPS
            PPSHistoACreer.SousCategorie = SousCategoriePPs
            PPSHistoACreer.Priorite = NumPriorite.Value
            PPSHistoACreer.PatientId = SelectedPatient.patientId.ToString
            PPSHistoACreer.PpsId = PPSId.ToString
            PPSHistoACreer.Commentaire = TxtCommentaire.Text
            PPSHistoACreer.Inactif = 0
            PPSHistoACreer.AffichageSynthese = 1
            PPSHistoACreer.DrcId = TxtDrcId.Text

            'Création dans l'historique des modifications de l'antecedent
            CreationPPSHisto(PPSHistoACreer, UtilisateurConnecte, EnumEtatPPSHisto.Modification)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            PatientDao.ModificationDateMajSynthesePatient(SelectedPatient.patientId)
        End If

        Return codeRetour
    End Function


    Private Function AnnulationPrevention() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_patient_pps set oa_pps_date_modification = @dateModification," &
        " oa_pps_utilisateur_modification = @utilisateurModification, oa_pps_inactif = @inactif, oa_pps_arret = @arret," &
        " oa_pps_commentaire_arret = @commentaireArret where oa_pps_id = @ppsId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@inactif", 1)
            .AddWithValue("@ppsId", PPSId.ToString)
            .AddWithValue("@arret", 1)
            .AddWithValue("@commentaireArret", TxtCommentaireArret.Text)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            MessageBox.Show(PPSDesignation & " annulée")
        Catch ex As Exception
            'PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If CategoriePPS = EnumCategoriePPS.Strategie Then
            SousCategoriePPs = DeterminationTypeStrategie()
        End If

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation antecedent
            PPSHistoACreer.HistorisationDate = Date.Now()
            PPSHistoACreer.HistorisationUtilisateurId = UtilisateurConnecte.UtilisateurId
            PPSHistoACreer.HistorisationEtat = EnumEtatPPSHisto.Annulation
            PPSHistoACreer.Categorie = CategoriePPS
            PPSHistoACreer.SousCategorie = SousCategoriePPs
            PPSHistoACreer.Priorite = NumPriorite.Value
            PPSHistoACreer.PatientId = SelectedPatient.patientId.ToString
            PPSHistoACreer.PpsId = PPSId.ToString
            PPSHistoACreer.Commentaire = TxtCommentaire.Text
            PPSHistoACreer.Arret = 1
            PPSHistoACreer.ArretCommentaire = TxtCommentaireArret.Text
            PPSHistoACreer.Inactif = 1
            PPSHistoACreer.DrcId = TxtDrcId.Text

            'Création dans l'historique des modifications de l'antecedent
            CreationPPSHisto(PPSHistoACreer, UtilisateurConnecte, EnumEtatPPSHisto.Annulation)

            'Mise à jour de la date de mise à jour de la synthèse (table patient)
            PatientDao.ModificationDateMajSynthesePatient(SelectedPatient.patientId)
        End If

        Return codeRetour
    End Function

    Private Sub RadBtnAnnulation_Click(sender As Object, e As EventArgs) Handles RadBtnAnnulation.Click
        RadGbxAnnulation.Show()
        TxtCommentaireArret.Show()
        LblLabelCommentaireArret.Show()
        RadBtnConfirmationAnnulation.Show()
        RadBtnAnnulation.Hide()
        RadBtnValidation.Hide()
        TxtDrcId.Enabled = False
        TxtCommentaire.Enabled = False
        NumPriorite.Enabled = False
    End Sub

    'Confirmation de l'annulation de la stratégie
    Private Sub RadBtnConfirmationAnnulation_Click(sender As Object, e As EventArgs) Handles RadBtnConfirmationAnnulation.Click
        'If TxtCommentaireArret.Text = "" Then
        'MessageBox.Show("La saisie du commentaire d'arrêt est obligatoire pour annuler une " & PPSDesignation)
        'Else
        '
        'End If
        If MsgBox("Confirmation de l'annulation de la " & PPSDesignation, MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            'Annulation antécédent
            If AnnulationPrevention() = True Then
                Me.CodeRetour = True
                Close()
            End If
        End If
    End Sub

    Private Sub NumPriorite_ValueChanged(sender As Object, e As EventArgs) Handles NumPriorite.ValueChanged
        RadBtnValidation.Show()
    End Sub

    Private Sub TxtDrcId_TextChanged(sender As Object, e As EventArgs) Handles TxtDrcId.TextChanged
        RadBtnValidation.Show()
    End Sub

    Private Sub TxtCommentaire_TextChanged(sender As Object, e As EventArgs) Handles TxtCommentaire.TextChanged
        RadBtnValidation.Show()
    End Sub

    Private Sub CbxTypeStrategie_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxTypeStrategie.SelectedIndexChanged
        RadBtnValidation.Show()
    End Sub

    Private Sub LblId_MouseHover(sender As Object, e As EventArgs) Handles LblId.MouseHover
        ToolTipPPS.SetToolTip(LblId, "Id : " + PPSId.ToString)
    End Sub

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

    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Close()
    End Sub

End Class
