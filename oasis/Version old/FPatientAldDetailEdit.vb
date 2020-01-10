Imports System.Data.SqlClient
Imports Oasis_WF

Public Class FPatientAldDetailEdit
    Private _SelectedPatient As Patient
    Private _UtilisateurConnecte As Utilisateur
    Private _SelectedAldClef As Integer
    Private _SelectedAldId As Integer
    Private _CodeRetour As Boolean

    Enum EnumEditMode
        Creation = 1
        Modification = 2
    End Enum

    Dim EditMode As Integer
    Dim utilisateurHisto As Utilisateur = New Utilisateur()

    Dim conxn As New SqlConnection(getConnectionString())

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

    Public Property SelectedAldClef As Integer
        Get
            Return _SelectedAldClef
        End Get
        Set(value As Integer)
            _SelectedAldClef = value
        End Set
    End Property

    Public Property SelectedAldId As Integer
        Get
            Return _SelectedAldId
        End Get
        Set(value As Integer)
            _SelectedAldId = value
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

    Private Sub FPatientAldDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementEtatCivil()
        If SelectedAldClef <> 0 Then
            Lblcim10Description.Text = ""
            LblLabelCodeALD.Hide()
            LblLabelCim10Slash.Hide()
            LblCodeALD.Text = ""
            LblCodeCim10.Text = ""
            'Modification
            EditMode = EnumEditMode.Modification
            ChargementAldExistante()
        Else
            'Création
            EditMode = EnumEditMode.Creation
            TxtAldId.Text = SelectedAldId
            If SelectedAldId <> 0 Then
                LblAldDescription.Text = Table_ald.GetAldDescription(CInt(TxtAldId.Text))
            Else
                LblAldDescription.Text = ""
            End If
            TxtCommentaire.Text = ""
            DteDateDebut.Value = Date.Now
            DteDateFin.Value = Date.Now
            'Inhiber boutons d'action de mise à jour
            BtnAnnuler.Hide()
            LblDateCreation.Hide()
            LblLabelDateCreation.Hide()
            LblUtilisateurCreation.Hide()
            LblLabelUtilisateurCreation.Hide()
            LblDateModification.Hide()
            LblLabelDateModification.Hide()
            LblUtilisateurModification.Hide()
            LblLabelUtilisateurModification.Hide()
            Lblcim10Description.Hide()
            LblLabelCodeALD.Hide()
            LblCodeALD.Hide()
            LblCodeCim10.Hide()
        End If
    End Sub

    Private Sub ChargementAldExistante()
        Dim AldDataReader As SqlDataReader
        Dim conxn As New SqlConnection(getConnectionString())
        Dim SQLString As String = "select * from oasis.oa_patient_ald where oa_patient_ald_clef = " & SelectedAldClef & ";"
        Dim myCommand As New SqlCommand(SQLString, conxn)

        Dim dateCreation, dateModification As Date

        conxn.Open()
        AldDataReader = myCommand.ExecuteReader()
        If AldDataReader.Read() Then
            'Commentaire ALD
            If AldDataReader("oa_patient_ald_commentaire") Is DBNull.Value Then
                TxtCommentaire.Text = ""
            Else
                TxtCommentaire.Text = AldDataReader("oa_patient_ald_commentaire")
            End If

            'Code ALD
            If AldDataReader("oa_patient_ald_id") Is DBNull.Value Then
                TxtAldId.Text = ""
                LblAldDescription.Text = ""
            Else
                TxtAldId.Text = AldDataReader("oa_patient_ald_id")
                LblAldDescription.Text = Table_ald.GetAldDescription(CInt(AldDataReader("oa_patient_ald_id")))
            End If

            'Date début
            Dim dateDebut As Date
            If AldDataReader("oa_patient_ald_date_debut") Is DBNull.Value Then
                DteDateDebut.Value = Date.Now
            Else
                dateDebut = AldDataReader("oa_patient_ald_date_debut")
                DteDateDebut.Value = dateDebut
            End If

            'Date fin
            Dim dateFin As Date
            If AldDataReader("oa_patient_ald_date_fin") Is DBNull.Value Then
                DteDateFin.Value = Date.Now
            Else
                dateFin = AldDataReader("oa_patient_ald_date_fin")
                DteDateFin.Value = dateFin
            End If

            'Date et utilisateur création
            LblDateCreation.Text = ""
            If AldDataReader("oa_patient_ald_date_creation") IsNot DBNull.Value Then
                dateCreation = AldDataReader("oa_patient_ald_date_creation")
                LblDateCreation.Text = dateCreation.ToString("dd.MM.yyyy")
            Else
                LblDateCreation.Hide()
                LblLabelDateCreation.Hide()
            End If

            LblUtilisateurCreation.Text = ""
            If AldDataReader("oa_patient_ald_utilisateur_creation") IsNot DBNull.Value Then
                If AldDataReader("oa_patient_ald_utilisateur_creation") <> 0 Then
                    SetUtilisateur(utilisateurHisto, AldDataReader("oa_patient_ald_utilisateur_creation"))
                    LblUtilisateurCreation.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
                End If
            Else
                LblUtilisateurCreation.Hide()
                LblLabelUtilisateurCreation.Hide()
            End If

            'Date et utilisateur modification
            LblDateModification.Text = ""
            If AldDataReader("oa_patient_ald_date_modification") IsNot DBNull.Value Then
                dateModification = AldDataReader("oa_patient_ald_date_modification")
                LblDateModification.Text = dateModification.ToString("dd.MM.yyyy")
            Else
                LblDateModification.Hide()
                LblLabelDateModification.Hide()
            End If

            LblUtilisateurModification.Text = ""
            If AldDataReader("oa_patient_ald_utilisateur_Modification") IsNot DBNull.Value Then
                If AldDataReader("oa_patient_ald_utilisateur_Modification") <> 0 Then
                    SetUtilisateur(utilisateurHisto, AldDataReader("oa_patient_Ald_utilisateur_Modification"))
                    LblUtilisateurModification.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
                End If
            Else
                LblUtilisateurModification.Hide()
                LblLabelUtilisateurModification.Hide()
            End If

            If AldDataReader("oa_patient_ald_cim10_id") IsNot DBNull.Value Then
                If AldDataReader("oa_patient_ald_cim10_id") <> 0 Then
                    TxtAldCim10Id.Text = AldDataReader("oa_patient_ald_cim10_id")
                    'TODO: Lecture ALD Cim10 pour récupérer la description Cim10 et le code ALD
                    Dim aldCim10 As AldCim10 = New AldCim10(CInt(TxtAldCim10Id.Text))
                    Lblcim10Description.Text = aldCim10.AldCim10Description
                    LblCodeALD.Text = aldCim10.AldCim10AldCode
                    LblCodeCim10.Text = aldCim10.AldCim10Code
                    Lblcim10Description.Show()
                    LblLabelCodeALD.Show()
                    LblLabelCim10Slash.Show()
                    LblCodeALD.Show()
                    LblCodeCim10.Show()
                End If
            Else
                TxtAldCim10Id.Text = ""
                Lblcim10Description.Hide()
                LblLabelCodeALD.Hide()
                LblLabelCim10Slash.Hide()
                LblCodeALD.Hide()
                LblCodeCim10.Hide()
            End If

        End If

        'Libération des ressources d'accès aux données
        conxn.Close()
        myCommand.Dispose()
    End Sub

    'Modification d'une Ald patient en base de données
    Private Function ModificationAld() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim dateDebut, dateFin As Date

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_patient_ald set oa_patient_ald_date_modification = @dateModification, oa_patient_ald_utilisateur_modification = @utilisateurModification, oa_patient_ald_commentaire = @aldCommentaire, oa_patient_ald_date_debut = @aldDateDebut, oa_patient_ald_date_fin = @aldDateFin, oa_patient_ald_id = @aldId, oa_patient_ald_cim10_id = @aldCim10Id where oa_patient_ald_clef = @aldClef"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        'Alimentation date début
        dateDebut = DteDateDebut.Value

        'Alimentation date fin
        dateFin = DteDateFin.Value

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd"))
            .AddWithValue("@aldCommentaire", TxtCommentaire.Text)
            .AddWithValue("@aldId", TxtAldId.Text)
            .AddWithValue("@aldDateDebut", dateDebut.ToString("yyyy-MM-dd"))
            .AddWithValue("@aldDateFin", dateFin.ToString("yyyy-MM-dd"))
            .AddWithValue("@aldCim10Id", TxtAldCim10Id.Text)
            .AddWithValue("@aldClef", SelectedAldClef)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            MessageBox.Show("Ald modifiée")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        Return codeRetour
    End Function

    'Création d'une ALD patient en base de données
    Private Function CreationAld() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim dateDebut, dateFin As Date
        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "insert into oasis.oa_patient_ald (oa_patient_ald_patient_id, oa_patient_ald_id, oa_patient_ald_commentaire, oa_patient_ald_date_debut, oa_patient_ald_date_fin, oa_patient_ald_utilisateur_creation, oa_patient_Ald_date_creation, oa_patient_ald_cim10_id) VALUES (@patientId, @aldId, @aldCommentaire, @aldDateDebut, @aldDateFin, @utilisateurCreation, @dateCreation, @aldCim10Id)"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        'Alimentation date début
        dateDebut = DteDateDebut.Value

        'Alimentation date fin
        dateFin = DteDateFin.Value


        With cmd.Parameters
            .AddWithValue("@patientId", SelectedPatient.patientId.ToString)
            .AddWithValue("@aldId", TxtAldId.Text)
            .AddWithValue("@aldCommentaire", TxtCommentaire.Text)
            .AddWithValue("@aldDateDebut", dateDebut)
            .AddWithValue("@aldDateFin", dateFin)
            .AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy-MM-dd"))
            .AddWithValue("@utilisateurCreation", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@aldCim10Id", TxtAldCim10Id.Text)
        End With

        Try
            conxn.Open()
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
            MessageBox.Show("ALD patient créée")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        Return codeRetour

    End Function

    Private Function AnnulationAldPatient() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_patient_ald set oa_patient_ald_date_modification = @dateModification, oa_patient_ald_utilisateur_modification = @utilisateurModification, oa_patient_ald_invalide = @invalide where oa_patient_ald_clef = @aldClef"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd"))
            .AddWithValue("@invalide", 1)
            .AddWithValue("@aldClef", SelectedAldClef)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            MessageBox.Show("ALD annulée")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        Return codeRetour
    End Function


    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)
    End Sub

    Private Sub BtnValidation_Click(sender As Object, e As EventArgs) Handles BtnValidation.Click
        If EditMode = EnumEditMode.Creation Then
            If ControleValiditationDonnees() = True Then
                If CreationAld() = True Then
                    CodeRetour = True
                    Close()
                End If
            End If
        Else
            If EditMode = EnumEditMode.Modification Then
                If ControleValiditationDonnees() = True Then
                    If ModificationAld() = True Then
                        CodeRetour = True
                        Close()
                    End If
                End If
            End If
        End If
    End Sub

    'Contrôle de la validation des données avant mise à jour de la base de données
    Private Function ControleValiditationDonnees() As Boolean
        Dim CodeRetour As Boolean = True
        Dim messageErreur As String = ""
        Dim messageErreur1 As String = ""
        Dim messageErreur2 As String = ""
        Dim messageErreur3 As String = ""
        Dim messageErreur4 As String = ""

        'ALD
        If TxtAldId.Text = "" Then
            messageErreur1 = "- La saisie du code ALD est obligatoire"
            CodeRetour = False
        Else
            'Contrôle validité du code ALD
            LblAldDescription.Text = Table_ald.GetAldDescription(CInt(TxtAldId.Text))
            If LblAldDescription.Text = "" Then
                messageErreur1 = "- Le code ALD saisi est invalide"
                CodeRetour = False
            End If
        End If

        'Date début obligatoire
        If DteDateDebut.Value = DteDateDebut.MinDate Then
            messageErreur2 = "- La saisie de la date de début est obligatoire"
            CodeRetour = False
        End If

        'Date fin obligatoire
        If DteDateFin.Value = DteDateFin.MinDate Then
            messageErreur2 = "- La saisie de la date de fin est obligatoire"
            CodeRetour = False
        End If

        'Date fin doit être supérieure à la date de début
        If DteDateFin.Value <= DteDateDebut.Value Then
            messageErreur3 = "- La date de fin doit être supérieure à la date de début"
            CodeRetour = False
        End If

        'Préparation de l'affichage des erreurs
        If CodeRetour = False Then
            If messageErreur1 <> "" Then
                messageErreur = messageErreur1 + " 
"
            End If

            If messageErreur2 <> "" Then
                messageErreur = messageErreur + messageErreur2 + " 
"
            End If

            If messageErreur3 <> "" Then
                messageErreur = messageErreur + messageErreur3 + " 
"
            End If

            If messageErreur4 <> "" Then
                messageErreur = messageErreur + messageErreur4 + " 
"
            End If

            messageErreur = messageErreur + vbCrLf + vbCrLf + "/!\ Validation impossible, des données sont incorrectes"

            MessageBox.Show(messageErreur)
        End If

        'Contrôler qu'une données a au moins été modifiée
        'TODO: contrôle qu'une données a au moins été modifiée

        Return CodeRetour
    End Function

    Private Sub BtnAbandonner_Click(sender As Object, e As EventArgs) Handles BtnAbandonner.Click
        CodeRetour = False
        Close()
    End Sub

    Private Sub BtnAnnuler_Click(sender As Object, e As EventArgs) Handles BtnAnnuler.Click
        If MsgBox("confirmation de la suppression de la Ald", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            'Annulation Ald patient
            If AnnulationAldPatient() = True Then
                Me.CodeRetour = True
                Close()
            Else
                Me.CodeRetour = False
            End If
        End If
    End Sub

    Private Sub TxtAldCode_DoubleClick(sender As Object, e As EventArgs) Handles TxtAldId.DoubleClick
        Dim vFAldSelecteur As New FAldSelecteur
        vFAldSelecteur.UtilisateurConnecte = Me.UtilisateurConnecte
        vFAldSelecteur.ShowDialog() 'Modal
        Dim SelectedAldId As Integer = vFAldSelecteur.SelectedAldId
        vFAldSelecteur.Dispose()
        'Si un médicament a été sélectionné
        If SelectedAldId <> 0 Then
            TxtAldId.Text = SelectedAldId
        End If
    End Sub

    Private Sub TxtAldId_TextChanged(sender As Object, e As EventArgs) Handles TxtAldId.TextChanged
        If TxtAldId.Text = "31" Then
            TxtAldCim10Id.Hide()
            Lblcim10Description.Hide()
            LblLabelCodeALD.Hide()
            LblLabelCim10Slash.Hide()
            LblCodeALD.Hide()
            LblCodeCim10.Hide()
        Else
            TxtAldCim10Id.Show()
            Lblcim10Description.Show()
            LblLabelCodeALD.Show()
            LblLabelCim10Slash.Show()
            LblCodeALD.Show()
            LblCodeCim10.Show()
            LblAldDescription.Text = Table_ald.GetAldDescription(CInt(TxtAldId.Text))
        End If
        TxtAldCim10Id.Text = ""
    End Sub

    Private Sub TxtAldIdCim10_TextChanged(sender As Object, e As EventArgs) Handles TxtAldCim10Id.TextChanged
        If TxtAldCim10Id.Text <> "" Then
            Dim aldCim10 As AldCim10 = New AldCim10(CInt(TxtAldCim10Id.Text))
            Lblcim10Description.Text = aldCim10.AldCim10Description
            LblCodeALD.Text = aldCim10.AldCim10AldCode
            LblCodeCim10.Text = aldCim10.AldCim10Code
            Lblcim10Description.Show()
            LblLabelCodeALD.Show()
            LblLabelCim10Slash.Show()
            LblCodeALD.Show()
            LblCodeCim10.Show()
        Else
            Lblcim10Description.Text = ""
            LblLabelCodeALD.Hide()
            LblLabelCim10Slash.Hide()
            LblCodeALD.Text = ""
            LblCodeCim10.Text = ""
        End If
    End Sub

    Private Sub BtnSelectionAldCim10_Click(sender As Object, e As EventArgs) Handles BtnSelectionAldCim10.Click
        Dim vFAldCim10Selecteur As New FAldCim10Selecteur
        vFAldCim10Selecteur.UtilisateurConnecte = Me.UtilisateurConnecte
        vFAldCim10Selecteur.SelectedAldId = CInt(TxtAldId.Text)
        vFAldCim10Selecteur.ShowDialog() 'Modal
        Dim SelectedAldCim10Id As Integer = vFAldCim10Selecteur.SelectedAldCim10Id
        vFAldCim10Selecteur.Dispose()
        'Si un code ALD CIM10 a été sélectionné
        If SelectedAldCim10Id <> 0 Then
            TxtAldCim10Id.Text = SelectedAldCim10Id
        End If
    End Sub

    Private Sub BtnInitialiserAldCim10Id_Click(sender As Object, e As EventArgs) Handles BtnInitialiserAldCim10Id.Click
        TxtAldCim10Id.Text = ""
    End Sub
End Class