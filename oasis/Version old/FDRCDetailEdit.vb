﻿Imports System.Data.SqlClient
Imports Oasis_Common

Public Class FDRCDetailEdit
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

    Enum EnumCategorieOasis
        AntecedentContexte = 1
        Strategie = 2
        Prevention = 3
        Objectif = 4
    End Enum

    Dim EditMode As Integer
    Dim utilisateurHisto As Utilisateur = New Utilisateur()
    Dim categorieMajeureListe As Dictionary(Of Integer, String) = Table_categorie_majeure.GetCategorieMajeureListe()
    Dim TransformationEnCours As Boolean = False
    Dim TransformedDrcId As Integer 'Utilisé pour la duplication des synonymes

    Dim conxn As New SqlConnection(getConnectionString())

    Private Sub FDRCDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load

        CbxCategorieOasis.Items.Add("Antécédent et Contexte")
        CbxCategorieOasis.Items.Add("Stratégie")
        CbxCategorieOasis.Items.Add("Prévention")
        CbxCategorieOasis.Items.Add("Objectif")

        CbxSexe.Items.Add("Homme")
        CbxSexe.Items.Add("Femme")
        CbxSexe.Items.Add("Homme et femme")

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
            BtnAnnuler.Hide()
            BtnTransformer.Hide()
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
                BtnTransformer.Hide()
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
                Select Case categorieOasis
                    Case 1
                        CbxCategorieOasis.SelectedItem = "Antécédent et Contexte"
                    Case 2
                        CbxCategorieOasis.SelectedItem = "Stratégie"
                    Case 3
                        CbxCategorieOasis.SelectedItem = "Prévention"
                    Case 4
                        CbxCategorieOasis.SelectedItem = "Objectif"
                    Case Else
                        CbxCategorieOasis.SelectedItem = "Antécédent et Contexte"
                End Select
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
                Select Case sexe
                    Case 1
                        CbxSexe.SelectedItem = "Homme"
                    Case 2
                        CbxSexe.SelectedItem = "Femme"
                    Case 3
                        CbxSexe.SelectedItem = "Homme et femme"
                    Case Else
                        CbxSexe.SelectedItem = "Homme et femme"
                End Select
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

        Dim SQLstring As String = "update oasis.oa_drc set oa_drc_date_modification = @dateModification, oa_drc_utilisateur_modification = @utilisateurModification, oa_drc_libelle = @description, oa_drc_oasis_categorie = @categorieOasis, oa_drc_categorie_majeure_id = @categorieMajeureId, oa_drc_sexe = @sexe, oa_drc_age_min = @ageMin, oa_drc_age_max = @ageMax, oa_drc_code_cim_defaut = @cim10Code, oa_drc_code_cisp_defaut = @cispCode, oa_drc_ald_code = @aldCode where oa_drc_id = @drcId"

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
        Select Case CbxCategorieOasis.SelectedItem
            Case "Antécédent et Contexte"
                categorieOasis = 1
            Case "Stratégie"
                categorieOasis = 2
            Case "Prévention"
                categorieOasis = 3
            Case "Objectif"
                categorieOasis = 4
        End Select

        'Sexe
        Select Case CbxSexe.SelectedItem
            Case "Homme"
                sexe = 1
            Case "Femme"
                sexe = 2
            Case "Homme et femme"
                sexe = 3
            Case Else
                sexe = 3
        End Select

        With cmd.Parameters
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@dateModification", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@description", TxtLibelle.Text)
            .AddWithValue("@categorieOasis", categorieOasis)
            .AddWithValue("@categorieMajeureId", categorieMajeureId)
            .AddWithValue("@sexe", sexe)
            .AddWithValue("@ageMin", NumAgeMin.Value)
            .AddWithValue("@ageMax", NumAgeMax.Value)
            .AddWithValue("@cim10Code", TxtCIM10.Text)
            .AddWithValue("@cispCode", TxtCISP.Text)
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
            .AddWithValue("@utilisateurModification", UtilisateurConnecte.UtilisateurId.ToString)
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

        Dim SQLstring As String = "insert into oasis.oa_drc (oa_drc_id, oa_drc_libelle, oa_drc_utilisateur_creation, oa_drc_date_creation, oa_drc_oasis_categorie, oa_drc_categorie_majeure_id, oa_drc_sexe, oa_drc_age_min, oa_drc_age_max, oa_drc_oasis, oa_drc_code_cim_defaut, oa_drc_code_cisp_defaut, oa_drc_ald_code) VALUES (@drcId, @description, @utilisateurCreation, @dateCreation, @categorieOasis, @categorieMajeureId, @sexe, @ageMin, @ageMax, @drcOasis, @cim10Code, @cispCode, @aldCode)"

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
        Select Case CbxCategorieOasis.SelectedItem
            Case "Antécédent et Contexte"
                categorieOasis = 1
            Case "Stratégie"
                categorieOasis = 2
            Case "Prévention"
                categorieOasis = 3
            Case "Objectif"
                categorieOasis = 4
        End Select

        'Sexe
        Select Case CbxSexe.SelectedItem
            Case "Homme"
                sexe = 1
            Case "Femme"
                sexe = 2
            Case "Homme et femme"
                sexe = 3
            Case Else
                sexe = 3
        End Select

        'Recherche dernière identifiant
        drcId = RechercheIdentifiantDRC()
        TransformedDrcId = drcId

        With cmd.Parameters
            .AddWithValue("@drcId", drcId)
            .AddWithValue("@dateCreation", DateTime.Now.ToString("yyyy-MM-dd HH:mm:ss"))
            .AddWithValue("@utilisateurCreation", UtilisateurConnecte.UtilisateurId.ToString)
            .AddWithValue("@description", TxtLibelle.Text)
            .AddWithValue("@categorieOasis", categorieOasis)
            .AddWithValue("@categorieMajeureId", categorieMajeureId)
            .AddWithValue("@sexe", sexe)
            .AddWithValue("@ageMin", NumAgeMin.Value)
            .AddWithValue("@ageMax", NumAgeMax.Value)
            .AddWithValue("@cim10Code", TxtCIM10.Text)
            .AddWithValue("@cispCode", TxtCISP.Text)
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


    Private Sub BtnValidation_Click(sender As Object, e As EventArgs) Handles BtnValidation.Click
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
        'SQLString = "select max(oa_drc_id) as key_max from oasis.oasis.oa_drc;"

        'Catégorie Oasis
        Dim CategorieOasis As Integer
        Select Case CbxCategorieOasis.SelectedItem
            Case "Antécédent et Contexte"
                CategorieOasis = 1
            Case "Stratégie"
                CategorieOasis = 2
            Case "Prévention"
                CategorieOasis = 3
            Case "Objectif"
                CategorieOasis = 4
        End Select

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
                Case 1
                    If CMDCDateReader("oa_r_categorie_majeure_compteur_cat1") IsNot DBNull.Value Then
                        Compteur = CMDCDateReader("oa_r_categorie_majeure_compteur_cat1")
                    End If
                Case 2
                    If CMDCDateReader("oa_r_categorie_majeure_compteur_cat2") IsNot DBNull.Value Then
                        Compteur = CMDCDateReader("oa_r_categorie_majeure_compteur_cat2")
                    End If
                Case 3
                    If CMDCDateReader("oa_r_categorie_majeure_compteur_cat3") IsNot DBNull.Value Then
                        Compteur = CMDCDateReader("oa_r_categorie_majeure_compteur_cat3")
                    End If
                Case 4
                    If CMDCDateReader("oa_r_categorie_majeure_compteur_cat4") IsNot DBNull.Value Then
                        Compteur = CMDCDateReader("oa_r_categorie_majeure_compteur_cat4")
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
            Case 1
                SQLString = "update oasis.oa_r_categorie_majeure set oa_r_categorie_majeure_compteur_cat1 = " & Compteur & " where oa_r_categorie_majeure_id = " & CategorieMajeureId & ";"
            Case 2
                SQLString = "update oasis.oa_r_categorie_majeure set oa_r_categorie_majeure_compteur_cat2 = " & Compteur & " where oa_r_categorie_majeure_id = " & CategorieMajeureId & ";"
            Case 3
                SQLString = "update oasis.oa_r_categorie_majeure set oa_r_categorie_majeure_compteur_cat3 = " & Compteur & " where oa_r_categorie_majeure_id = " & CategorieMajeureId & ";"
            Case 4
                SQLString = "update oasis.oa_r_categorie_majeure set oa_r_categorie_majeure_compteur_cat4 = " & Compteur & " where oa_r_categorie_majeure_id = " & CategorieMajeureId & ";"
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
    Private Sub BtnAbandonner_Click(sender As Object, e As EventArgs) Handles BtnAbandonner.Click
        CodeRetour = False
        Close()
    End Sub

    Private Sub BtnAnnuler_Click(sender As Object, e As EventArgs) Handles BtnAnnuler.Click
        'Annulation de la DRC/ORC
        If MsgBox("confirmation de la suppression de la DRC", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            If AnnulationDRC() = True Then
                CodeRetour = True
                Close()
            End If
        Else
            Me.CodeRetour = False
        End If

    End Sub

    'Transformer une DRC en ORC
    Private Sub BtnTransformer_Click(sender As Object, e As EventArgs) Handles BtnTransformer.Click
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
        vFAldSelecteur.UtilisateurConnecte = Me.UtilisateurConnecte
        vFAldSelecteur.ShowDialog() 'Modal

        Dim SelectedAldCode As String = vFAldSelecteur.SelectedAldCode
        vFAldSelecteur.Dispose()
        'Si un code ALD a été sélectionné
        If SelectedAldCode <> "" Then
            TxtAld.Text = SelectedAldCode
            ChargeAldDescription()
        End If
    End Sub

    Private Sub TxtAld_TextChanged(sender As Object, e As EventArgs) Handles TxtAld.TextChanged
        ChargeAldDescription()
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

End Class
