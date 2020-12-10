
Imports Oasis_Common
Public Class RadFRorDetailEdit
    Private _selectedRorId As Integer
    Private _selectedSpecialiteId As Integer
    Private _selectedTypeSpecialite As String
    Private _codeRetour As Boolean

    Public Property SelectedSpecialiteId As Integer
        Get
            Return _selectedSpecialiteId
        End Get
        Set(value As Integer)
            _selectedSpecialiteId = value
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

    Public Property SelectedRorId As Integer
        Get
            Return _selectedRorId
        End Get
        Set(value As Integer)
            _selectedRorId = value
        End Set
    End Property

    Public Property SelectedTypeSpecialite As String
        Get
            Return _selectedTypeSpecialite
        End Get
        Set(value As String)
            _selectedTypeSpecialite = value
        End Set
    End Property

    Enum EnumEditMode
        Modification = 1
        Creation = 2
    End Enum

    Dim EditMode As Integer
    Dim UtilisateurHisto As New Utilisateur
    Dim ror As New Ror
    Dim rorDao As New RorDao

    Private Sub RadFRorDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If SelectedRorId = 0 Then
            EditMode = EnumEditMode.Creation
            If SelectedSpecialiteId <> 0 Then
                TxtSpecialite.Text = Table_specialite.GetSpecialiteDescription(SelectedSpecialiteId)
                InitZoneCreation()
            End If
        Else
            EditMode = EnumEditMode.Modification
            ChargementRor()
        End If
    End Sub

    Private Sub ChargementRor()
        Dim dateCreation, dateModification As Date
        ror = rorDao.getRorById(Me.SelectedRorId)
        TxtSpecialite.Text = Table_specialite.GetSpecialiteDescription(ror.SpecialiteId)
        TxtSpecialite.Enabled = False
        TxtNomIntervenant.Text = ror.Nom
        CbxType.Text = ror.Type
        TxtNomStructure.Text = ror.StructureNom
        TxtAdresse1.Text = ror.Adresse1
        TxtAdresse2.Text = ror.Adresse2
        TxtCodePostal.Text = ror.CodePostal
        TxtVille.Text = ror.Ville
        TxtTelephone.Text = ror.Telephone
        TxtEmail.Text = ror.Email
        TxtCommentaire.Text = ror.Commentaire
        TxtRPPS.Text = ror.Rpps
        TxtFiness.Text = ror.Finess
        TxtAdeli.Text = ror.Adeli

        If ror.DateCreation <> Nothing Then
            dateCreation = ror.DateCreation
            LblDateCreation.Text = dateCreation.ToString("dd.MM.yyyy")
        Else
            LblDateCreation.Text = ""
            LblLabelDateCreation.Hide()
            LblLabelParCreation.Hide()
        End If

        LblUtilisateurCreation.Text = ""

        If ror.UserCreation <> 0 Then
            Dim userDao As New UserDao
            UtilisateurHisto = userDao.getUserById(ror.UserCreation)
            'SetUtilisateur(UtilisateurHisto, ror.UserCreation)
            LblUtilisateurCreation.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
        End If

        If ror.DateModification <> Nothing Then
            dateModification = ror.DateModification
            LblDateModification.Text = dateModification.ToString("dd.MM.yyyy")
        Else
            LblDateModification.Text = ""
            LblLabelDateModification.Hide()
            LblLabelParModification.Hide()
        End If

        LblUtilisateurModification.Text = ""
        If ror.UserModification <> 0 Then
            Dim userDao As New UserDao
            UtilisateurHisto = userDao.getUserById(ror.UserModification)
            'SetUtilisateur(UtilisateurHisto, ror.UserModification)
            LblUtilisateurModification.Text = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom
        End If

        If ror.Oasis = True Then
            RadBtnSpecialite.Hide()
        End If
    End Sub

    Private Sub InitZoneCreation()
        'RadBtnValider.Enabled = True
        RadBtnAnnuler.Hide()
        'Cacher les éléments de création de l'occurrence
        LblLabelDateModification.Hide()
        LblDateModification.Hide()
        LblLabelParModification.Hide()
        LblUtilisateurModification.Hide()
        'Cacher les éléments de modification de l'occurrence
        LblLabelDateCreation.Hide()
        LblDateCreation.Hide()
        LblLabelParCreation.Hide()
        LblUtilisateurCreation.Hide()

        ror.SpecialiteId = SelectedSpecialiteId
        ror.Nom = ""
        ror.Type = Ror.EnumIntervenant.Intervenants
        CbxType.Text = Ror.EnumIntervenant.Intervenants
        CbxType.Enabled = True
        ror.StructureNom = ""
        ror.Adresse1 = ""
        ror.Adresse2 = ""
        ror.CodePostal = ""
        ror.Ville = ""
        ror.Telephone = ""
        ror.Email = ""
        ror.Code = ""
        ror.Commentaire = ""
        ror.Rpps = 0
        ror.Finess = 0
        ror.Adeli = 0
        ror.Inactif = False
        ror.UserCreation = userLog.UtilisateurId
        ror.DateCreation = Date.Now()
        ror.ExtractionAnnuaire = False
        ror.IdentifiantNational = ""
        ror.IdentifiantStructure = ""
        ror.CodeModeExercice_r23 = ""
        ror.CodeProfessionSante_g15 = ""
        ror.CodeTypeSavoirFaire_r04 = ""
        ror.CodeSavoirFaire = ""
        ror.CleReferenceAnnuaire = 0
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnValider_Click(sender As Object, e As EventArgs) Handles RadBtnValider.Click
        CodeRetour = False
        Select Case EditMode
            Case EnumEditMode.Creation
                If ValidationDonneeSaisie() = True Then
                    If rorDao.CreationRor(ror, userLog) = True Then
                        MessageBox.Show("Elément créé dans le référentiel des professionnels de santé de type : " & CbxType.Text)
                        CodeRetour = True
                        Close()
                    End If
                End If
            Case EnumEditMode.Modification
                If ValidationDonneeSaisie() = True Then
                    If rorDao.ModificationRor(ror, userLog) = True Then
                        MessageBox.Show("Elément modifié dans le référentiel des professionnels de santé de type : " & CbxType.Text)
                        CodeRetour = True
                        Close()
                    End If
                End If
        End Select
    End Sub

    Private Function ValidationDonneeSaisie() As Boolean
        'Contrôle des données saisie
        Dim Valide As Boolean = True
        Dim MessageErreur1 As String = ""
        Dim MessageErreur2 As String = ""
        Dim MessageErreur3 As String = ""
        Dim MessageErreur4 As String = ""
        Dim MessageErreur5 As String = ""
        Dim MessageErreur6 As String = ""
        Dim MessageErreur As String = ""

        If TxtNomIntervenant.Text = "" Then
            Valide = False
            MessageErreur1 = "Le nom de l'intervenant est obligatoire"
        End If

        If TxtAdresse1.Text = "" Or TxtVille.Text = "" Then
            Valide = False
            MessageErreur2 = "La saisie est obligatoire pour l'adresse 1 et la ville"
        End If

        If TxtCodePostal.Text <> "" Then
            If IsNumeric(TxtCodePostal.Text) = False Then
                Valide = False
                MessageErreur3 = "Le code postal doit être numérique"
            End If
        End If

        If TxtRPPS.Text <> "" Then
            If IsNumeric(TxtRPPS.Text) = False Then
                Valide = False
                MessageErreur4 = "Le numéro RPPS doit être numérique"
            End If
        End If

        If TxtFiness.Text <> "" Then
            If IsNumeric(TxtFiness.Text) = False Then
                Valide = False
                MessageErreur5 = "Le numéro FINESS doit être numérique"
            End If
        End If

        If TxtAdeli.Text <> "" Then
            If IsNumeric(TxtAdeli.Text) = False Then
                Valide = False
                MessageErreur6 = "Le numéro ADELI doit être numérique"
            End If
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

            If MessageErreur5 <> "" Then
                MessageErreur = MessageErreur & MessageErreur4 & vbCrLf
            End If

            If MessageErreur6 <> "" Then
                MessageErreur = MessageErreur & MessageErreur4 & vbCrLf
            End If

            MessageErreur = MessageErreur & vbCrLf & "/!\ données incorrectes"
            MessageBox.Show(MessageErreur)
        End If

        Return Valide
    End Function

    Private Sub RadBtnSpecialite_Click(sender As Object, e As EventArgs) Handles RadBtnSpecialite.Click
        Using vRadFSpecialiteSelecteur As New RadFSpecialiteSelecteur
            vRadFSpecialiteSelecteur.Select()
            vRadFSpecialiteSelecteur.ShowDialog()
            SelectedSpecialiteId = vRadFSpecialiteSelecteur.SelectedSpecialiteId
            If SelectedSpecialiteId <> 0 Then
                ror.SpecialiteId = SelectedSpecialiteId
                TxtSpecialite.Text = Table_specialite.GetSpecialiteDescription(ror.SpecialiteId)
            End If
        End Using
    End Sub

    Private Sub TxtNomIntervenant_TextChanged(sender As Object, e As EventArgs) Handles TxtNomIntervenant.TextChanged
        ror.Nom = TxtNomIntervenant.Text
    End Sub

    Private Sub CbxType_TextChanged(sender As Object, e As EventArgs) Handles CbxType.TextChanged
        ror.Type = CbxType.Text
    End Sub

    Private Sub TxtNomStructure_TextChanged(sender As Object, e As EventArgs) Handles TxtNomStructure.TextChanged
        ror.StructureNom = TxtNomStructure.Text
    End Sub

    Private Sub TxtAdresse1_TextChanged(sender As Object, e As EventArgs) Handles TxtAdresse1.TextChanged
        ror.Adresse1 = TxtAdresse1.Text
    End Sub

    Private Sub TxtAdresse2_TextChanged(sender As Object, e As EventArgs) Handles TxtAdresse2.TextChanged
        ror.Adresse2 = TxtAdresse2.Text
    End Sub

    Private Sub TxtCodePostal_TextChanged(sender As Object, e As EventArgs) Handles TxtCodePostal.TextChanged
        ror.CodePostal = TxtCodePostal.Text
    End Sub

    Private Sub TxtVille_TextChanged(sender As Object, e As EventArgs) Handles TxtVille.TextChanged
        ror.Ville = TxtVille.Text

    End Sub

    Private Sub TxtTelephone_TextChanged(sender As Object, e As EventArgs) Handles TxtTelephone.TextChanged
        ror.Telephone = TxtTelephone.Text
    End Sub

    Private Sub TxtEmail_TextChanged(sender As Object, e As EventArgs) Handles TxtEmail.TextChanged
        ror.Email = TxtEmail.Text
    End Sub

    Private Sub TxtCommentaire_TextChanged(sender As Object, e As EventArgs) Handles TxtCommentaire.TextChanged
        ror.Commentaire = TxtCommentaire.Text
    End Sub

    Private Sub TxtRPPS_TextChanged(sender As Object, e As EventArgs) Handles TxtRPPS.TextChanged
        If TxtRPPS.Text <> "" Then
            If IsNumeric(TxtRPPS.Text) Then
                ror.Rpps = TxtRPPS.Text
            Else
                TxtRPPS.Text = ror.Rpps
            End If
        End If
    End Sub

    Private Sub TxtFiness_TextChanged(sender As Object, e As EventArgs) Handles TxtFiness.TextChanged
        If TxtFiness.Text <> "" Then
            If IsNumeric(TxtFiness.Text) Then
                ror.Finess = TxtFiness.Text
            Else
                TxtFiness.Text = ror.Finess
            End If
        End If
    End Sub

    Private Sub TxtAdeli_TextChanged(sender As Object, e As EventArgs) Handles TxtAdeli.TextChanged
        If TxtAdeli.Text <> "" Then
            If IsNumeric(TxtAdeli.Text) Then
                ror.Adeli = TxtAdeli.Text
            Else
                TxtAdeli.Text = ror.Adeli
            End If
        End If
    End Sub
End Class
