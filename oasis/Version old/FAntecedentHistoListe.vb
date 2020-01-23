Imports System.Data.SqlClient
Imports Oasis_WF
Imports Oasis_Common
Public Class FAntecedentHistoListe
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedAntecedentId As Integer

    Private Sub FAntecedentHistoListe_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initZones()
        ChargementEtatCivil()
        ChargementAntecedent()
    End Sub

    Dim UtilisateurHisto As Utilisateur = New Utilisateur()

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

    '==========================================================
    '======================= Etat civil =======================
    '==========================================================

    'Chargement des données de l'état civil du patient dans les labels dédiés
    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir.ToString

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

    End Sub

    '==========================================================
    '======================= Antecedent =======================
    '==========================================================

    'Chargement de la Grid
    Private Sub ChargementAntecedent()
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()

        Dim conxn As New SqlConnection(outils.getConnectionString())

        Dim SQLString As String
        SQLString = "select * from oasis.oa_antecedent_histo where oa_antecedent_id = '" + SelectedAntecedentId.ToString + "' order by oa_antecedent_histo_id desc;"

        antecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        antecedentDataAdapter.Fill(antecedentDataTable)

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1
        Dim natureHisto As Integer
        Dim ActionHistoString, Publication, typeAntecedent, TypeAntecedentString, categorieContexte, CategorieContexteString, DiagnosticString As String
        Dim dateDebut, dateFin As Date
        Dim dateHisto As DateTime
        Dim Arret, Inactif As Boolean

        'Initialisation des variables de comparaison
        Dim DateDebutComp, DateFinComp As Date
        Dim ArretComp, InactifComp As Boolean
        Dim DescriptionComp, PublicationComp, ArretCommentaireComp, TypeAntecedentComp, CategorieContexteComp, DiagnosticComp As String
        Dim DrcComp, NiveauComp, AntecedentId1Comp, AntecedentId2Comp, Ordre1Comp, Ordre2Comp, Ordre3Comp As Integer


        DrcComp = 0
        TypeAntecedentComp = ""
        CategorieContexteComp = ""
        DescriptionComp = ""
        DiagnosticComp = ""
        PublicationComp = ""
        ArretComp = False
        ArretCommentaireComp = ""
        NiveauComp = 0
        AntecedentId1Comp = 0
        AntecedentId2Comp = 0
        InactifComp = False

        Dim premierPassage As Boolean = True

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1

            'Initialisation Booléens de travail
            Inactif = False
            Arret = False


            'Type (antécédent ou Contexte)
            typeAntecedent = antecedentDataTable.Rows(i)("oa_antecedent_type")
            Select Case typeAntecedent
                Case "A"
                    TypeAntecedentString = "Antécédent"
                Case "C"
                    TypeAntecedentString = "Contexte"
                Case Else
                    TypeAntecedentString = "Inconnu"
            End Select

            'Nature historisation
            natureHisto = antecedentDataTable.Rows(i)("oa_antecedent_histo_etat_historisation")
            Select Case natureHisto
                Case 1
                    If typeAntecedent = "A" Then
                        ActionHistoString = "Creation Antecedent"
                    Else
                        ActionHistoString = "Creation Contexte"
                    End If
                Case 2
                    If typeAntecedent = "A" Then
                        ActionHistoString = "Modification Antecedent"
                    Else
                        ActionHistoString = "Modification Contexte"
                    End If
                Case 3
                    ActionHistoString = "Arret Contexte"
                Case 4
                    If typeAntecedent = "A" Then
                        ActionHistoString = "Annulation Antecedent"
                    Else
                        ActionHistoString = "Annulation Contexte"
                    End If
                Case 5
                    If typeAntecedent = "A" Then
                        ActionHistoString = "Transformation en Antecedent"
                    Else
                        ActionHistoString = "Réactivation du contexte"
                    End If
                Case Else
                    ActionHistoString = "Inconnue"
            End Select

            'Type (antécédent ou Contexte)
            If typeAntecedent = "C" Then
                categorieContexte = antecedentDataTable.Rows(i)("oa_antecedent_categorie_contexte")
                Select Case categorieContexte
                    Case "M"
                        CategorieContexteString = "Médical"
                    Case "B"
                        CategorieContexteString = "Bio-environnemental"
                    Case Else
                        CategorieContexteString = "Inconnue"
                End Select
            Else
                CategorieContexteString = ""
            End If

            'Diagnostic
            DiagnosticString = ""
            If antecedentDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                Dim Diagnostic As Integer = antecedentDataTable.Rows(i)("oa_antecedent_diagnostic")
                Select Case Diagnostic
                    Case 1
                        DiagnosticString = "Confirmé"
                    Case 2
                        DiagnosticString = "Suspecté"
                    Case 3
                        DiagnosticString = "Supposé"
                    Case 4
                        DiagnosticString = "Notion"
                End Select
            End If

            'Date de début de l'antecedent
            If antecedentDataTable.Rows(i)("oa_antecedent_date_debut") IsNot DBNull.Value Then
                dateDebut = antecedentDataTable.Rows(i)("oa_antecedent_date_debut")
            Else
                dateDebut = Nothing
            End If

            'Date de fin de validité du contexte
            If antecedentDataTable.Rows(i)("oa_antecedent_date_fin") IsNot DBNull.Value Then
                dateFin = antecedentDataTable.Rows(i)("oa_antecedent_date_fin")
            Else
                dateFin = Nothing
            End If

            'Identification si l'antecedent a été arrêté
            If antecedentDataTable.Rows(i)("oa_antecedent_arret") IsNot DBNull.Value Then
                If antecedentDataTable.Rows(i)("oa_antecedent_arret") = "1" Then
                    Arret = True
                End If
            End If

            'Identification si le antecedent a été annulé
            If antecedentDataTable.Rows(i)("oa_antecedent_inactif") IsNot DBNull.Value Then
                If antecedentDataTable.Rows(i)("oa_antecedent_inactif") = "1" Then
                    Inactif = True
                End If
            End If

            'Alimentation de la >Grid
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            AntecedentDataGridView.Rows.Insert(iGrid)


            '------------------- Alimentation du DataGridView
            AntecedentDataGridView("antecedentId", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_id")

            'Date historisation
            dateHisto = antecedentDataTable.Rows(i)("oa_antecedent_histo_date_historisation")
            AntecedentDataGridView("histoDate", iGrid).Value = dateHisto.ToString("dd.MM.yyyy HH:mm:ss")

            'Utilisateur
            Dim UtilisateurId As Integer = antecedentDataTable.Rows(i)("oa_antecedent_histo_utilisateur_historisation")
            SetUtilisateur(UtilisateurHisto, UtilisateurId)
            AntecedentDataGridView("histoUtilisateur", iGrid).Value = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom

            'Action
            AntecedentDataGridView("histoAction", iGrid).Value = ActionHistoString

            'DRC
            AntecedentDataGridView("drc", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_drc_id")
            If antecedentDataTable.Rows(i)("oa_antecedent_drc_id").ToString <> DrcComp Or premierPassage = True Then
                AntecedentDataGridView("drc", iGrid).Style.ForeColor = Color.Red
            End If
            DrcComp = antecedentDataTable.Rows(i)("oa_antecedent_drc_id").ToString

            'Description
            AntecedentDataGridView("description", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_description")
            If antecedentDataTable.Rows(i)("oa_antecedent_description").ToString <> DescriptionComp Or premierPassage = True Then
                AntecedentDataGridView("description", iGrid).Style.ForeColor = Color.Red
            End If
            DescriptionComp = antecedentDataTable.Rows(i)("oa_antecedent_description")

            'Type
            AntecedentDataGridView("type", iGrid).Value = TypeAntecedentString
            If TypeAntecedentString <> TypeAntecedentComp Or premierPassage = True Then
                AntecedentDataGridView("type", iGrid).Style.ForeColor = Color.Red
            End If
            TypeAntecedentComp = TypeAntecedentString

            'Diagnostic
            AntecedentDataGridView("diagnostic", iGrid).Value = DiagnosticString
            If DiagnosticString <> DiagnosticComp Or premierPassage = True Then
                AntecedentDataGridView("diagnostic", iGrid).Style.ForeColor = Color.Red
            End If
            DiagnosticComp = DiagnosticString

            'Catégorie
            AntecedentDataGridView("categorieContexte", iGrid).Value = CategorieContexteString
            If CategorieContexteString <> CategorieContexteComp Or premierPassage = True Then
                AntecedentDataGridView("categorieContexte", iGrid).Style.ForeColor = Color.Red
            End If
            CategorieContexteComp = CategorieContexteString

            'Date début
            AntecedentDataGridView("dateDebut", iGrid).Value = dateDebut.ToString("dd.MM.yyyy")
            If dateDebut <> DateDebutComp Or premierPassage = True Then
                AntecedentDataGridView("dateDebut", iGrid).Style.ForeColor = Color.Red
            End If
            DateDebutComp = dateDebut

            'Date fin de validité
            If typeAntecedent = "C" Then
                AntecedentDataGridView("dateFin", iGrid).Value = dateFin.ToString("dd.MM.yyyy")
                If dateFin <> DateFinComp Or premierPassage = True Then
                    AntecedentDataGridView("dateFin", iGrid).Style.ForeColor = Color.Red
                End If
                DateFinComp = dateFin
            Else
                AntecedentDataGridView("dateFin", iGrid).Value = ""
            End If

            'Publication
            Publication = antecedentDataTable.Rows(i)("oa_antecedent_statut_affichage")
            Select Case Publication
                Case "P"
                    AntecedentDataGridView("publication", iGrid).Value = "Publié"
                Case "C"
                    AntecedentDataGridView("publication", iGrid).Value = "Caché"
                Case "O"
                    AntecedentDataGridView("publication", iGrid).Value = "Occulté"
                Case Else
                    AntecedentDataGridView("publication", iGrid).Value = "Inconnu"
            End Select
            If antecedentDataTable.Rows(i)("oa_antecedent_statut_affichage").ToString <> PublicationComp Or premierPassage = True Then
                AntecedentDataGridView("publication", iGrid).Style.ForeColor = Color.Red
            End If
            PublicationComp = antecedentDataTable.Rows(i)("oa_antecedent_statut_affichage")

            'Arrêt
            If Arret = True Then
                AntecedentDataGridView("arret", iGrid).Value = "Oui"
            Else
                AntecedentDataGridView("arret", iGrid).Value = "Non"
            End If
            If Arret <> ArretComp Or premierPassage = True Then
                AntecedentDataGridView("arret", iGrid).Style.ForeColor = Color.Red
            End If
            ArretComp = Arret

            'Commentaire arret
            AntecedentDataGridView("arretCommentaire", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_arret_commentaire")
            If antecedentDataTable.Rows(i)("oa_antecedent_arret_commentaire").ToString <> ArretCommentaireComp Or premierPassage = True Then
                AntecedentDataGridView("arretCommentaire", iGrid).Style.ForeColor = Color.Red
            End If
            ArretCommentaireComp = antecedentDataTable.Rows(i)("oa_antecedent_arret_commentaire").ToString

            'Niveau
            AntecedentDataGridView("niveau", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_niveau")
            If antecedentDataTable.Rows(i)("oa_antecedent_niveau").ToString <> NiveauComp Or premierPassage = True Then
                AntecedentDataGridView("niveau", iGrid).Style.ForeColor = Color.Red
            End If
            NiveauComp = antecedentDataTable.Rows(i)("oa_antecedent_niveau").ToString

            'Antecedent identifiant niveau 1
            AntecedentDataGridView("antecedentId1", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_id_niveau1")
            If antecedentDataTable.Rows(i)("oa_antecedent_id_niveau1").ToString <> AntecedentId1Comp Or premierPassage = True Then
                AntecedentDataGridView("antecedentId1", iGrid).Style.ForeColor = Color.Red
            End If
            AntecedentId1Comp = antecedentDataTable.Rows(i)("oa_antecedent_id_niveau1").ToString

            'Antecedent identifiant niveau 2
            AntecedentDataGridView("antecedentId2", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_id_niveau2")
            If antecedentDataTable.Rows(i)("oa_antecedent_id_niveau2").ToString <> AntecedentId2Comp Or premierPassage = True Then
                AntecedentDataGridView("antecedentId2", iGrid).Style.ForeColor = Color.Red
            End If
            AntecedentId2Comp = antecedentDataTable.Rows(i)("oa_antecedent_id_niveau2").ToString

            'Activité
            If Inactif = True Then
                AntecedentDataGridView("inactif", iGrid).Value = "Supprimé"
            Else
                AntecedentDataGridView("inactif", iGrid).Value = "Actif"
            End If
            If Inactif <> InactifComp Or premierPassage = True Then
                AntecedentDataGridView("inactif", iGrid).Style.ForeColor = Color.Red
            End If
            InactifComp = Inactif

            'Ordre 1
            AntecedentDataGridView("ordre1", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage1")
            If antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage1").ToString <> Ordre1Comp Or premierPassage = True Then
                AntecedentDataGridView("ordre1", iGrid).Style.ForeColor = Color.Red
            End If
            Ordre1Comp = antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage1").ToString

            'Ordre 2
            AntecedentDataGridView("ordre2", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage2")
            If antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage2").ToString <> Ordre2Comp Or premierPassage = True Then
                AntecedentDataGridView("ordre2", iGrid).Style.ForeColor = Color.Red
            End If
            Ordre2Comp = antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage2").ToString

            'Ordre 3
            AntecedentDataGridView("ordre3", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage3")
            If antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage3").ToString <> Ordre3Comp Or premierPassage = True Then
                AntecedentDataGridView("ordre3", iGrid).Style.ForeColor = Color.Red
            End If
            Ordre3Comp = antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage3").ToString

            premierPassage = False
        Next

        conxn.Close()
        antecedentDataAdapter.Dispose()

        'Enlève le focus sur la première ligne du Grid
        AntecedentDataGridView.ClearSelection()
    End Sub

    '===========================================================
    '======================= Généralités =======================
    '===========================================================

    'Initialisation de l'écran
    Private Sub initZones()
        'Etat civil
        LblPatientNIR.Text = ""
        LblPatientPrenom.Text = ""
        LblPatientNom.Text = ""
        LblPatientAge.Text = ""
        LblPatientGenre.Text = ""
        LblPatientAdresse1.Text = ""
        LblPatientAdresse2.Text = ""
        LblPatientCodePostal.Text = ""
        LblPatientVille.Text = ""
        LblPatientTel1.Text = ""
        LblPatientTel2.Text = ""
        LblPatientSite.Text = ""
        LblPatientUniteSanitaire.Text = ""
        LblPatientDateMaj.Text = ""
        'Antecedents
        AntecedentDataGridView.Rows.Clear()
    End Sub

    Private Sub AntecedentDataGridView_DoubleClick(sender As Object, e As EventArgs) Handles AntecedentDataGridView.DoubleClick
        'Dim AntecedentId As Integer
        'antecedentId = AntecedentDataGridView.Rows(AntecedentDataGridView.CurrentRow.Index).Cells("AntecedentId").Value

        'Dim vFAntecedentDetailEdit As New FAntecedentDetailEdit
        'vFAntecedentDetailEdit.SelectedAntecedentId = AntecedentId
        'vFAntecedentDetailEdit.SelectedPatient = Me.SelectedPatient
        'vFAntecedentDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte

        'vFAntecedentDetailEdit.ShowDialog() 'Modal

        'vFAntecedentDetailEdit.Dispose()
    End Sub
End Class