Imports System.Data.SqlClient

Public Class RadFDrcDetailEdit
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedDRCId As Integer
    Private privateCodeRetour As Boolean

    Public Property UtilisateurConnecte As Utilisateur
        Get
            Return privateUtilisateurConnecte
        End Get
        Set(value As Utilisateur)
            privateUtilisateurConnecte = value
        End Set
    End Property

    Public Property SelectedDRCId As Integer
        Get
            Return privateSelectedDRCId
        End Get
        Set(value As Integer)
            privateSelectedDRCId = value
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

    Enum EnumEditMode
        Creation = 1
        Modification = 2
    End Enum

    Dim drcDao As New DrcDao

    Dim EditMode As Integer
    Dim utilisateurHisto As Utilisateur = New Utilisateur()
    Dim categorieMajeureListe As Dictionary(Of Integer, String) = Table_categorie_majeure.GetCategorieMajeureListe()
    Dim TransformationEnCours As Boolean = False
    Dim TransformedDrcId As Integer 'Utilisé pour la duplication des synonymes

    Dim SelectedAldId As Integer

    Dim conxn As New SqlConnection(getConnectionString())

    Private Sub RadFDrcDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        CodeRetour = False

        CbxCategorieOasis.Items.Add(DrcDao.EnumCategorieOasisItem.Contexte)
        CbxCategorieOasis.Items.Add(DrcDao.EnumCategorieOasisItem.Strategie)
        CbxCategorieOasis.Items.Add(DrcDao.EnumCategorieOasisItem.Prevention)
        CbxCategorieOasis.Items.Add(DrcDao.EnumCategorieOasisItem.Objectif)
        CbxCategorieOasis.Items.Add(DrcDao.EnumCategorieOasisItem.ActeParamedical)
        CbxCategorieOasis.Items.Add(DrcDao.EnumCategorieOasisItem.GroupeParametres)
        CbxCategorieOasis.Items.Add(DrcDao.EnumCategorieOasisItem.ProtocoleCollaboratif)

        CbxSexe.Items.Add(DrcDao.EnumGenre.Homme)
        CbxSexe.Items.Add(DrcDao.EnumGenre.Femme)
        CbxSexe.Items.Add(DrcDao.EnumGenre.HommeEtFemme)

        LblALDDescription.Text = ""

        'Catégorie Majeure
        Dim indice As Integer = categorieMajeureListe.Count - 1
        Dim categorieMajeureDescription(indice) As String
        Dim i As Integer = 0

        For Each kvp As KeyValuePair(Of Integer, String) In categorieMajeureListe
            categorieMajeureDescription(i) = kvp.Value
            i += 1
        Next kvp
        CbxCategorieMajeure.DataSource = categorieMajeureDescription

        If SelectedDRCId <> 0 Then
            'Modification
            EditMode = EnumEditMode.Modification
            ChargementDRCxistante()
            'En modification d'une ORC (uniquement), on ne peut pas modifier la catégorie majeure et la catégorie DRC car elles sont significatives pour la clé
            If SelectedDRCId > 9999 Then
                CbxCategorieMajeure.Enabled = False
                CbxCategorieOasis.Enabled = False
            End If
        Else
            'Création
            EditMode = EnumEditMode.Creation
            TxtId.Hide()
            LblDRCId.Hide()
            TxtLibelle.Text = ""
            'Inhiber boutons d'action de mise à jour
            LblDateCreation.Hide()
            LblLabelDateCreation.Hide()
            LblUtilisateurCreation.Hide()
            LblLabelUtilisateurCreation.Hide()
            LblDateModification.Hide()
            LblLabelDateModification.Hide()
            LblUtilisateurModification.Hide()
            LblLabelUtilisateurModification.Hide()
            RadBtnAnnuler.Hide()
            RadBtnTransformer.Hide()
        End If
    End Sub

    Private Sub ChargementDRCxistante()
        Dim DRCDateReader As SqlDataReader
        Dim conxn As New SqlConnection(getConnectionString())
        Dim SQLString As String = "select * from oasis.oa_drc where oa_drc_id = " & SelectedDRCId & ";"
        Dim myCommand As New SqlCommand(SQLString, conxn)

        Dim dateCreation, dateModification As Date

        conxn.Open()
        DRCDateReader = myCommand.ExecuteReader()
        If DRCDateReader.Read() Then
            TxtId.Text = DRCDateReader("oa_drc_id")
            'Description
            If DRCDateReader("oa_drc_libelle") Is DBNull.Value Then
                TxtLibelle.Text = ""
            Else
                TxtLibelle.Text = DRCDateReader("oa_drc_libelle")
            End If

            'Le bouton Transformer ne doit être accessible que pour les DRC
            If CInt(DRCDateReader("oa_drc_id")) > 9999 Then
                RadBtnTransformer.Hide()
            End If

            'Date et utilisateur création
            LblDateCreation.Text = ""
            If DRCDateReader("oa_drc_date_creation") IsNot DBNull.Value Then
                dateCreation = DRCDateReader("oa_drc_date_creation")
                LblDateCreation.Text = dateCreation.ToString("dd.MM.yyyy")
            Else
                LblDateCreation.Hide()
                LblLabelDateCreation.Hide()
            End If

            LblUtilisateurCreation.Text = ""
            If DRCDateReader("oa_drc_utilisateur_creation") IsNot DBNull.Value Then
                If DRCDateReader("oa_drc_utilisateur_creation") <> 0 Then
                    SetUtilisateur(utilisateurHisto, DRCDateReader("oa_drc_utilisateur_creation"))
                    LblUtilisateurCreation.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
                End If
            Else
                LblUtilisateurCreation.Hide()
                LblLabelUtilisateurCreation.Hide()
            End If

            'Date et utilisateur modification
            LblDateModification.Text = ""
            If DRCDateReader("oa_drc_date_modification") IsNot DBNull.Value Then
                dateModification = DRCDateReader("oa_drc_date_modification")
                LblDateModification.Text = dateModification.ToString("dd.MM.yyyy")
            Else
                LblDateModification.Hide()
                LblLabelDateModification.Hide()
            End If

            LblUtilisateurModification.Text = ""
            If DRCDateReader("oa_drc_utilisateur_Modification") IsNot DBNull.Value Then
                If DRCDateReader("oa_drc_utilisateur_Modification") <> 0 Then
                    SetUtilisateur(utilisateurHisto, DRCDateReader("oa_drc_utilisateur_Modification"))
                    LblUtilisateurModification.Text = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
                End If
            Else
                LblUtilisateurModification.Hide()
                LblLabelUtilisateurModification.Hide()
            End If

            'Catégorie Oasis
            If DRCDateReader("oa_drc_oasis_categorie") IsNot DBNull.Value Then
                Dim categorieOasis As Integer = DRCDateReader("oa_drc_oasis_categorie")
                CbxCategorieOasis.SelectedItem = drcDao.GetItemCategorieOasisByCode(categorieOasis)
            End If

            'Catégorie majeure
            If DRCDateReader("oa_drc_categorie_majeure_id") IsNot DBNull.Value Then
                Dim categorieMajeureId As Integer = DRCDateReader("oa_drc_categorie_majeure_id")
                Dim categorieMajeuerDescription As String = Table_categorie_majeure.GetCategorieMajeureDescription(categorieMajeureId)
                CbxCategorieMajeure.SelectedItem = categorieMajeuerDescription
            Else
                CbxCategorieMajeure.SelectedItem = ""
            End If


            'Age min et max
            NumAgeMin.Value = 0
            If DRCDateReader("oa_drc_age_min") IsNot DBNull.Value Then
                NumAgeMin.Value = DRCDateReader("oa_drc_age_min")
            End If

            NumAgeMax.Value = 0
            If DRCDateReader("oa_drc_age_max") IsNot DBNull.Value Then
                NumAgeMax.Value = DRCDateReader("oa_drc_age_max")
            End If

            'Sexe
            If DRCDateReader("oa_drc_sexe") IsNot DBNull.Value Then
                Dim sexe As Integer = DRCDateReader("oa_drc_sexe")
                CbxSexe.SelectedItem = drcDao.GetItemGenreByCode(sexe)
            End If

            'Code CIM10
            If DRCDateReader("oa_drc_code_cim_defaut") IsNot DBNull.Value Then
                TxtCIM10.Text = DRCDateReader("oa_drc_code_cim_defaut")
            End If

            'Code CISP
            If DRCDateReader("oa_drc_code_cisp_defaut") IsNot DBNull.Value Then
                TxtCISP.Text = DRCDateReader("oa_drc_code_cisp_defaut")
            End If

            'Code ALD
            If DRCDateReader("oa_drc_ald_code") IsNot DBNull.Value Then
                TxtAld.Text = DRCDateReader("oa_drc_ald_code")
                If TxtAld.Text <> "" Then
                    Dim AldDescription As String
                    AldDescription = Table_ald.GetAldDescription(TxtAld.Text)
                    If AldDescription <> "" Then
                        LblALDDescription.Text = AldDescription
                    Else
                        LblALDDescription.Text = ""
                    End If
                End If
            End If
            SelectedAldId = Coalesce(DRCDateReader("oa_drc_ald_id"), 0)
        End If

        'Libération des ressources d'accès aux données
        conxn.Close()
        myCommand.Dispose()
    End Sub

    'Modification d'une DRC/ORC en base de données
    Private Function ModificationDRC() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date
        Dim categorieOasis, categorieMajeureId, sexe As Integer

        Dim SQLstring As String = "update oasis.oa_drc set oa_drc_date_modification = @dateModification, oa_drc_utilisateur_modification = @utilisateurModification," &
        " oa_drc_libelle = @description, oa_drc_oasis_categorie = @categorieOasis, oa_drc_categorie_majeure_id = @categorieMajeureId, oa_drc_sexe = @sexe," &
        " oa_drc_age_min = @ageMin, oa_drc_age_max = @ageMax, oa_drc_code_cim_defaut = @cim10Code, oa_drc_code_cisp_defaut = @cispCode," &
        " oa_drc_ald_id = @aldId, oa_drc_ald_code = @aldCode WHERE oa_drc_id = @drcId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        'Catégorie Majeure
        categorieMajeureId = 0
        For Each kvp As KeyValuePair(Of Integer, String) In categorieMajeureListe
            If kvp.Value = CbxCategorieMajeure.SelectedValue Then
                categorieMajeureId = kvp.Key
                Exit For
            End If
        Next kvp

        'Catégorie Oasis
        categorieOasis = drcDao.GetCodeCategorieOasisByItem(CbxCategorieOasis.SelectedItem)

        'Sexe
        sexe = drcDao.GetCodeGenreByItem(CbxSexe.SelectedItem)

        If TxtAld.Text = "" Then
            SelectedAldId = 0
        End If

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@description", TxtLibelle.Text)
            .AddWithValue("@categorieOasis", categorieOasis)
            .AddWithValue("@categorieMajeureId", categorieMajeureId)
            .AddWithValue("@sexe", sexe)
            .AddWithValue("@ageMin", NumAgeMin.Value)
            .AddWithValue("@ageMax", NumAgeMax.Value)
            .AddWithValue("@cim10Code", TxtCIM10.Text)
            .AddWithValue("@cispCode", TxtCISP.Text)
            .AddWithValue("@aldId", SelectedAldId)
            .AddWithValue("@aldCode", TxtAld.Text)
            .AddWithValue("@drcId", SelectedDRCId)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            MessageBox.Show("DRC/ORC modifiée")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        Return codeRetour
    End Function

    'Annulation d'une DRC/ORC en base de données
    Private Function AnnulationDRC() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateModification As Date = Date.Now.Date

        Dim SQLstring As String = "update oasis.oa_drc set oa_drc_date_modification = @dateModification, oa_drc_utilisateur_modification = @utilisateurModification, oa_drc_oasis_invalide = @invalide where oa_drc_id = @drcId"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", userLog.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@invalide", 1)
            .AddWithValue("@drcId", SelectedDRCId)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
            If TransformationEnCours = False Then
                MessageBox.Show("DRC/ORC modifiée")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        Return codeRetour
    End Function

    'Création d'une ORC en base de données
    Private Function CreationDRC() As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateCreation As Date = Date.Now.Date
        Dim drcId, categorieOasis, categorieMajeureId, sexe As Integer

        Dim SQLstring As String = "insert into oasis.oa_drc" &
        " (oa_drc_id, oa_drc_libelle, oa_drc_utilisateur_creation, oa_drc_date_creation, oa_drc_oasis_categorie, oa_drc_categorie_majeure_id," &
        " oa_drc_sexe, oa_drc_age_min, oa_drc_age_max, oa_drc_oasis, oa_drc_code_cim_defaut, oa_drc_code_cisp_defaut, oa_drc_ald_id, oa_drc_ald_code)" &
        " VALUES (@drcId, @description, @utilisateurCreation, @dateCreation, @categorieOasis, @categorieMajeureId," &
        " @sexe, @ageMin, @ageMax, @drcOasis, @cim10Code, @cispCode, @aldId, @aldCode)"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        'Catégorie Majeure
        categorieMajeureId = 0
        For Each kvp As KeyValuePair(Of Integer, String) In categorieMajeureListe
            If kvp.Value = CbxCategorieMajeure.SelectedValue Then
                categorieMajeureId = kvp.Key
                Exit For
            End If
        Next kvp

        'Catégorie Oasis
        categorieOasis = drcDao.GetCodeCategorieOasisByItem(CbxCategorieOasis.SelectedItem)

        'Sexe
        sexe = drcDao.GetCodeGenreByItem(CbxSexe.SelectedItem)

        'Recherche dernière identifiant
        drcId = RechercheIdentifiantDRC()
        TransformedDrcId = drcId

        With cmd.Parameters
            .AddWithValue("@drcId", drcId)
            .AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@utilisateurCreation", userLog.UtilisateurId.ToString)
            .AddWithValue("@description", TxtLibelle.Text)
            .AddWithValue("@categorieOasis", categorieOasis)
            .AddWithValue("@categorieMajeureId", categorieMajeureId)
            .AddWithValue("@sexe", sexe)
            .AddWithValue("@ageMin", NumAgeMin.Value)
            .AddWithValue("@ageMax", NumAgeMax.Value)
            .AddWithValue("@cim10Code", TxtCIM10.Text)
            .AddWithValue("@cispCode", TxtCISP.Text)
            .AddWithValue("@aldId", SelectedAldId)
            .AddWithValue("@aldCode", TxtAld.Text)
            .AddWithValue("@drcOasis", 1)
        End With

        Try
            conxn.Open()
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
            If TransformationEnCours = False Then
                MessageBox.Show("ORC créée")
            End If
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        Return codeRetour

    End Function

    Private Sub RecuperationSynonyme()
        Dim SynonymeDRCDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim SynonymeDRCDataTable As DataTable = New DataTable()
        Dim conxn As New SqlConnection(outils.getConnectionString())
        Dim SQLString As String
        SQLString = "SELECT oa_drc_synonyme_id, oa_drc_synonyme_libelle FROM oasis.oa_drc_synonyme WHERE oa_drc_id = " + SelectedDRCId.ToString + " order by oa_drc_synonyme_id;"

        SynonymeDRCDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        SynonymeDRCDataAdapter.Fill(SynonymeDRCDataTable)

        DuplicationSynonyme(SynonymeDRCDataTable)

        conxn.Close()
        SynonymeDRCDataAdapter.Dispose()

    End Sub

    Private Sub DuplicationSynonyme(SynonymeDRCDataTable As DataTable)
        Dim Synonyme As String
        Dim i As Integer
        Dim rowCount As Integer = SynonymeDRCDataTable.Rows.Count - 1

        'Parcours du DataTable pour dupliquer les synonymes sur l'ORC issue de la transformation de la DRC
        For i = 0 To rowCount Step 1
            Synonyme = SynonymeDRCDataTable.Rows(i)("oa_drc_synonyme_libelle")
            CreationSynonyme(Synonyme)
        Next

    End Sub

    Private Function CreationSynonyme(Synonyme As String) As Boolean
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim dateCreation As Date = Date.Now.Date

        Dim SQLstring As String = "insert into oasis.oa_drc_synonyme (oa_drc_id, oa_drc_synonyme_libelle) VALUES (@drcId, @synonyme)"

        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            '.AddWithValue("@drcSynonymeId", SelectedDrcSynonymeId.ToString)
            .AddWithValue("@drcId", TransformedDrcId.ToString)
            .AddWithValue("@synonyme", Synonyme)
        End With

        Try
            conxn.Open()
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        Return codeRetour
    End Function


    Private Sub BtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        If EditMode = EnumEditMode.Creation Then
            If ValidationDonnees() Then
                If CreationDRC() = True Then
                    CodeRetour = True
                    Close()
                End If
            End If
        Else
            If EditMode = EnumEditMode.Modification Then
                If ValidationDonnees() Then
                    If ModificationDRC() = True Then
                        CodeRetour = True
                        Close()
                    End If
                End If
            End If
        End If

    End Sub
    Private Function ValidationDonnees() As Boolean
        Dim Valide As Boolean = True
        Dim zoneObligatoire As Boolean = True
        Dim messageErreur As String = ""
        Dim messageErreur1 As String = ""
        Dim messageErreur2 As String = ""
        Dim messageErreur3 As String = ""
        Dim messageErreur4 As String = ""

        'Nom, Prenom, date naissance, genre, adresse 1, code postal et ville obligatoire
        If TxtLibelle.Text.Trim() = "" Then
            zoneObligatoire = False
        End If

        If zoneObligatoire = False Then
            messageErreur1 = "- La saisie de la description est obligatoire"
            Valide = False
        End If

        'Vérification de la validité du code ALD s'il est renseigné
        If TxtAld.Text.Trim() <> "" Then
            Dim AldDescription As String
            AldDescription = Table_ald.GetAldDescription(TxtAld.Text)
            If AldDescription = "" Then
                LblALDDescription.Text = ""
                messageErreur2 = "- L'ALD n'est pas valide"
                Valide = False
            Else
                LblALDDescription.Text = AldDescription
            End If
        End If

        'Catégorie majeur obligatore pour antécédent et contexte
        If CbxCategorieOasis.SelectedItem = DrcDao.EnumCategorieOasisItem.Contexte Then
            If CbxCategorieMajeure.SelectedItem = "" Then
                messageErreur3 = "- La catégorie majeure est obligatoire pour la catégorie Oasis : (" & DrcDao.EnumCategorieOasisItem.Contexte & ")"
                Valide = False
            End If
        End If

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

            messageErreur = messageErreur + vbCrLf + vbCrLf + "/!\ Validation impossible, des données sont incorrectes"

            MessageBox.Show(messageErreur)
        End If

        Return Valide
    End Function

    Private Function RechercheIdentifiantDRC() As Integer
        Dim CMDCDateReader As SqlDataReader
        Dim conxn As New SqlConnection(getConnectionString())
        Dim SQLString As String

        'Catégorie Oasis
        Dim CategorieOasis As Integer
        CategorieOasis = drcDao.GetCodeCategorieOasisByItem(CbxCategorieOasis.SelectedItem)

        'Catégorie Majeure
        Dim CategorieMajeureId As Integer
        CategorieMajeureId = 0
        For Each kvp As KeyValuePair(Of Integer, String) In categorieMajeureListe
            If kvp.Value = CbxCategorieMajeure.SelectedValue Then
                CategorieMajeureId = kvp.Key
                Exit For
            End If
        Next kvp

        SQLString = "select * from oasis.oa_r_categorie_majeure where oa_r_categorie_majeure_id = " & CategorieMajeureId & ";"
        Dim cmd As New SqlCommand(SQLString, conxn)
        Dim drcId As Integer
        Dim Compteur As Integer

        drcId = 0
        conxn.Open()
        CMDCDateReader = cmd.ExecuteReader()
        If CMDCDateReader.HasRows Then
            CMDCDateReader.Read()
            Select Case CategorieOasis
                Case DrcDao.EnumCategorieOasisCode.Contexte
                    If CMDCDateReader("oa_r_categorie_majeure_compteur_cat1") IsNot DBNull.Value Then
                        Compteur = CMDCDateReader("oa_r_categorie_majeure_compteur_cat1")
                    End If
                Case DrcDao.EnumCategorieOasisCode.Strategie
                    If CMDCDateReader("oa_r_categorie_majeure_compteur_cat2") IsNot DBNull.Value Then
                        Compteur = CMDCDateReader("oa_r_categorie_majeure_compteur_cat2")
                    End If
                Case DrcDao.EnumCategorieOasisCode.Prevention
                    If CMDCDateReader("oa_r_categorie_majeure_compteur_cat3") IsNot DBNull.Value Then
                        Compteur = CMDCDateReader("oa_r_categorie_majeure_compteur_cat3")
                    End If
                Case DrcDao.EnumCategorieOasisCode.Objectif
                    If CMDCDateReader("oa_r_categorie_majeure_compteur_cat4") IsNot DBNull.Value Then
                        Compteur = CMDCDateReader("oa_r_categorie_majeure_compteur_cat4")
                    End If
                Case DrcDao.EnumCategorieOasisCode.ActeParamedical
                    If CMDCDateReader("oa_r_categorie_majeure_compteur_cat5") IsNot DBNull.Value Then
                        Compteur = CMDCDateReader("oa_r_categorie_majeure_compteur_cat5")
                    End If
                Case DrcDao.EnumCategorieOasisCode.GroupeParametres
                    If CMDCDateReader("oa_r_categorie_majeure_compteur_cat6") IsNot DBNull.Value Then
                        Compteur = CMDCDateReader("oa_r_categorie_majeure_compteur_cat6")
                    End If
                Case DrcDao.EnumCategorieOasisCode.ProtocoleCollaboratif
                    If CMDCDateReader("oa_r_categorie_majeure_compteur_cat7") IsNot DBNull.Value Then
                        Compteur = CMDCDateReader("oa_r_categorie_majeure_compteur_cat7")
                    End If
            End Select
            Compteur += 1
        Else
            Return 0
        End If
        'Libération des ressources d'accès aux données

        conxn.Close()
        cmd.Dispose()

        'Mise à jour du compteur
        Select Case CategorieOasis
            Case DrcDao.EnumCategorieOasisCode.Contexte
                SQLString = "update oasis.oa_r_categorie_majeure set oa_r_categorie_majeure_compteur_cat1 = " & Compteur & " where oa_r_categorie_majeure_id = " & CategorieMajeureId & ";"
            Case DrcDao.EnumCategorieOasisCode.Strategie
                SQLString = "update oasis.oa_r_categorie_majeure set oa_r_categorie_majeure_compteur_cat2 = " & Compteur & " where oa_r_categorie_majeure_id = " & CategorieMajeureId & ";"
            Case DrcDao.EnumCategorieOasisCode.Prevention
                SQLString = "update oasis.oa_r_categorie_majeure set oa_r_categorie_majeure_compteur_cat3 = " & Compteur & " where oa_r_categorie_majeure_id = " & CategorieMajeureId & ";"
            Case DrcDao.EnumCategorieOasisCode.Objectif
                SQLString = "update oasis.oa_r_categorie_majeure set oa_r_categorie_majeure_compteur_cat4 = " & Compteur & " where oa_r_categorie_majeure_id = " & CategorieMajeureId & ";"
            Case DrcDao.EnumCategorieOasisCode.ActeParamedical
                SQLString = "update oasis.oa_r_categorie_majeure set oa_r_categorie_majeure_compteur_cat5 = " & Compteur & " where oa_r_categorie_majeure_id = " & CategorieMajeureId & ";"
            Case DrcDao.EnumCategorieOasisCode.GroupeParametres
                SQLString = "update oasis.oa_r_categorie_majeure set oa_r_categorie_majeure_compteur_cat6 = " & Compteur & " where oa_r_categorie_majeure_id = " & CategorieMajeureId & ";"
            Case DrcDao.EnumCategorieOasisCode.ProtocoleCollaboratif
                SQLString = "update oasis.oa_r_categorie_majeure set oa_r_categorie_majeure_compteur_cat7 = " & Compteur & " where oa_r_categorie_majeure_id = " & CategorieMajeureId & ";"
        End Select

        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim cmd2 As New SqlCommand(SQLString, conxn)

        Try
            conxn.Open()
            da.UpdateCommand = cmd2
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        Finally
            conxn.Close()
        End Try

        drcId = (CategorieOasis * 100000) + (CategorieMajeureId * 1000) + Compteur
        Return drcId
    End Function
    Private Sub BtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Close()
    End Sub

    Private Sub BtnAnnuler_Click(sender As Object, e As EventArgs) Handles RadBtnAnnuler.Click
        'Annulation de la DRC/ORC
        If MsgBox("confirmation de l'annulation de la DRC", MsgBoxStyle.YesNo Or MsgBoxStyle.Critical Or MsgBoxStyle.DefaultButton2, "Annulation DRC") = MsgBoxResult.Yes Then
            If AnnulationDRC() = True Then
                CodeRetour = True
                Close()
            End If
        Else
            Me.CodeRetour = False
        End If

    End Sub

    'Transformer une DRC en ORC
    Private Sub BtnTransformer_Click(sender As Object, e As EventArgs) Handles RadBtnTransformer.Click
        'Catégorie Majeure
        Dim CategorieMajeureId As Integer
        CategorieMajeureId = 0
        For Each kvp As KeyValuePair(Of Integer, String) In categorieMajeureListe
            If kvp.Value = CbxCategorieMajeure.SelectedValue Then
                CategorieMajeureId = kvp.Key
                Exit For
            End If
        Next kvp
        If CategorieMajeureId = 0 Then
            MessageBox.Show("/!\ Traitement impossible, vous devez renseigner la catégorie majeure pour transformer une DRC en ORC")
        Else
            If MsgBox("Confirmation de la transformation de la DRC en ORC", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
                TransformationEnCours = True
                'Création Orc
                CreationDRC()
                'Duplication des synonymes
                RecuperationSynonyme()
                'Annulation DRC
                AnnulationDRC()
                'Sortie écran
                Me.CodeRetour = True
                MessageBox.Show("DRC transformée en ORC")
                Close()
            End If
        End If

    End Sub

    Private Sub TxtAld_DoubleClick(sender As Object, e As EventArgs) Handles TxtAld.DoubleClick
        Dim vFAldSelecteur As New FAldSelecteur
        vFAldSelecteur.UtilisateurConnecte = userLog
        vFAldSelecteur.ShowDialog() 'Modal

        Dim SelectedAldCode As String = vFAldSelecteur.SelectedAldCode
        'Si un code ALD a été sélectionné
        If SelectedAldCode <> "" Then
            TxtAld.Text = SelectedAldCode
            SelectedAldId = vFAldSelecteur.SelectedAldId
            ChargeAldDescription()
        End If
        vFAldSelecteur.Dispose()
    End Sub

    Private Sub TxtAld_TextChanged(sender As Object, e As EventArgs) Handles TxtAld.TextChanged
        ChargeAldDescription()
        If TxtAld.Text = "" Then
            SelectedAldId = 0
        End If
    End Sub

    Private Sub TxtAld_Leave(sender As Object, e As EventArgs) Handles TxtAld.Leave
        ChargeAldDescription()
    End Sub

    Private Sub ChargeAldDescription()
        Dim AldDescription As String
        AldDescription = Table_ald.GetAldDescription(TxtAld.Text)
        If AldDescription <> "" Then
            LblALDDescription.Text = AldDescription
        Else
            LblALDDescription.Text = ""
        End If
    End Sub

    Private Sub CbxCategorieOasis_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxCategorieOasis.SelectedIndexChanged
        If CbxCategorieOasis.Text = DrcDao.EnumCategorieOasisItem.ActeParamedical Or
            CbxCategorieOasis.Text = DrcDao.EnumCategorieOasisItem.GroupeParametres Or
            CbxCategorieOasis.Text = DrcDao.EnumCategorieOasisItem.ProtocoleCollaboratif Then
            RadGroupBox2.Hide()
            CbxCategorieMajeure.SelectedItem = ""
        End If
        If CbxCategorieOasis.Text <> DrcDao.EnumCategorieOasisItem.GroupeParametres Then
            RadBtnParametre.Hide()
        End If
        If CbxCategorieOasis.Text <> DrcDao.EnumCategorieOasisItem.ProtocoleCollaboratif Then
            RadBtnProtocole.Hide()
        End If
    End Sub

    Private Sub RadBtnParametre_Click(sender As Object, e As EventArgs) Handles RadBtnParametre.Click
        Dim DrcId As Integer = CreationDrcAvantAction()
        If DrcId <> 0 Then
            Using vRadFDrcParametresEdit As New RadFDrcParametresEdit
                vRadFDrcParametresEdit.DrcId = DrcId
                vRadFDrcParametresEdit.ShowDialog()
            End Using
        End If
    End Sub

    Private Sub RadBtnProtocole_Click(sender As Object, e As EventArgs) Handles RadBtnProtocole.Click
        Dim DrcId As Integer = CreationDrcAvantAction()
        If DrcId <> 0 Then
            Using vRadFDrcActePMAssocieEdit As New RadFDrcActePMAssocieEdit
                vRadFDrcActePMAssocieEdit.ProtocoleCollaboratifDrcId = DrcId
                vRadFDrcActePMAssocieEdit.ShowDialog()
            End Using
        End If
    End Sub

    Private Function CreationDrcAvantAction() As Integer
        Dim DrcIdParm As Integer
        If EditMode = EnumEditMode.Creation Then
            If ValidationDonnees() Then
                If CreationDRC() = True Then
                    EditMode = EnumEditMode.Modification
                    CodeRetour = True
                    'Recherche identifiant DORC créée
                    Dim categorieOasis As Integer
                    categorieOasis = drcDao.GetCodeCategorieOasisByItem(CbxCategorieOasis.SelectedItem)
                    DrcIdParm = drcDao.GetLastDrc(categorieOasis)
                Else
                    Return 0
                End If
            Else
                Return 0
            End If
        Else
            DrcIdParm = SelectedDRCId
        End If

        Return DrcIdParm
    End Function
End Class
