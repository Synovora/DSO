
Imports System.Data.SqlClient

Public Class FTraitementHistoListe
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateSelectedTraitementId As Integer
    Private privateMedicamentDenomination As String

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

    Public Property SelectedTraitementId As Integer
        Get
            Return privateSelectedTraitementId
        End Get
        Set(value As Integer)
            privateSelectedTraitementId = value
        End Set
    End Property

    Public Property MedicamentDenomination As String
        Get
            Return privateMedicamentDenomination
        End Get
        Set(value As String)
            privateMedicamentDenomination = value
        End Set
    End Property

    Private Sub FTraitementGestion_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        initZones()
        ChargementEtatCivil()
        LblMedicamentDenomination.Text = Me.MedicamentDenomination
        ChargementTraitement()
    End Sub

    Dim UtilisateurHisto As Utilisateur = New Utilisateur()

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
    '======================= Traitement =======================
    '==========================================================

    'Chargement de la Grid
    Private Sub ChargementTraitement()
        Dim traitementDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim traitementDataTable As DataTable = New DataTable()

        Dim conxn As New SqlConnection(outils.getConnectionString())

        Dim SQLString As String
        SQLString = "select * from oasis.oa_traitement_histo where oa_traitement_id = '" + SelectedTraitementId.ToString + "' order by oa_traitement_histo_id desc;"

        traitementDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        traitementDataAdapter.Fill(traitementDataTable)

        'Ajout d'une colonne 'oa_traitement_posologie' dans le DataTable de traitement
        traitementDataTable.Columns.Add("oa_traitement_posologie", Type.GetType("System.String"))

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = traitementDataTable.Rows.Count - 1
        Dim natureHisto As Integer
        Dim natureHistoString As String
        Dim Base As String
        Dim allergie, contreIndication As Boolean
        Dim Posologie As String
        Dim dateFin, dateDebut, datefinFenetre, dateDebutFenetre As Date
        Dim dateHisto As DateTime
        Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
        Dim Rythme As Integer
        Dim Arret, Annulation, Fenetre As Boolean

        'Initialisation des variables de comparaison
        Dim DateDebutComp, DateFinComp, DateDebutFenetreComp, DateFinFenetreComp As Date
        Dim FenetreComp, ArretComp, AllergieComp, ContreIndicationComp, AnnulationComp As Boolean
        Dim PosologieComp, OrdreAffichageComp, PosologieDureeComp, TraitementCommentaireComp, PosologieCommentaireComp, FenetreCommentaireComp, ArretCommentaireComp, AnnulationCommentaireComp As String

        OrdreAffichageComp = ""
        PosologieComp = ""
        PosologieDureeComp = ""
        PosologieCommentaireComp = ""
        FenetreCommentaireComp = ""
        ArretCommentaireComp = ""
        AnnulationCommentaireComp = ""
        TraitementCommentaireComp = ""

        Dim premierPassage As Boolean = True

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1

            'Initialisation Booléens de travail
            allergie = False
            contreIndication = False
            annulation = False
            Arret = False
            Fenetre = False


            'Nature historisation
            natureHisto = traitementDataTable.Rows(i)("oa_traitement_histo_etat_historisation")
            Select Case natureHisto
                Case 1
                    natureHistoString = "Creation Traitement"
                Case 2
                    natureHistoString = "Modification Traitement"
                Case 3
                    natureHistoString = "Arret Traitement"
                Case 4
                    natureHistoString = "Annulation Traitement"
                Case 5
                    natureHistoString = "Creation Fenêtre Thérapeutique"
                Case 6
                    natureHistoString = "Modification Fenêtre Thérapeutique"
                Case 7
                    natureHistoString = "Suppression Fenêtre Thérapeutique"
                Case 8
                    natureHistoString = "Suppression Traitement"
                Case Else
                    natureHistoString = "Inconnue"
            End Select

            'Date de début de traitement
            If traitementDataTable.Rows(i)("oa_traitement_date_debut") IsNot DBNull.Value Then
                DateDebut = traitementDataTable.Rows(i)("oa_traitement_date_debut")
            Else
                DateDebut = Nothing
            End If

            'Date de fin de traitement
            If traitementDataTable.Rows(i)("oa_traitement_date_fin") IsNot DBNull.Value Then
                dateFin = traitementDataTable.Rows(i)("oa_traitement_date_fin")
            Else
                dateFin = "31/12/2999"
            End If

            'Formatage de la posologie
            Posologie = ""

            If Fenetre = False Then
                If traitementDataTable.Rows(i)("oa_traitement_posologie_base") IsNot DBNull.Value Then
                    Rythme = traitementDataTable.Rows(i)("oa_traitement_posologie_rythme")
                    Select Case traitementDataTable.Rows(i)("oa_traitement_posologie_base")
                        Case "J"
                            Base = "Journalier : "
                            If traitementDataTable.Rows(i)("oa_traitement_posologie_matin") <> 0 Then
                                posologieMatin = Rythme
                            Else
                                posologieMatin = 0
                            End If
                            If traitementDataTable.Rows(i)("oa_traitement_posologie_midi") <> 0 Then
                                posologieMidi = Rythme
                            Else
                                posologieMidi = 0
                            End If
                            If traitementDataTable.Rows(i)("oa_traitement_posologie_soir") <> 0 Then
                                posologieSoir = Rythme
                            Else
                                posologieSoir = 0
                            End If
                            If traitementDataTable.Rows(i)("oa_traitement_posologie_apres_midi") <> 0 Then
                                posologieApresMidi = Rythme
                                Posologie = Base + posologieMatin.ToString + "." + posologieMidi.ToString + "." + posologieApresMidi.ToString + "." + posologieSoir.ToString
                            Else
                                Posologie = Base + " " + posologieMatin.ToString + "." + posologieMidi.ToString + "." + posologieSoir.ToString
                            End If
                        Case "H"
                            Base = "Hebdo : "
                            Posologie = Base + Rythme.ToString
                        Case "M"
                            Base = "Mensuel : "
                            Posologie = Base + Rythme.ToString
                        Case "A"
                            Base = "Annuel : "
                            Posologie = Base + Rythme.ToString
                        Case Else
                            Base = "Base inconnue ! "
                            Posologie = Base + Rythme.ToString
                    End Select
                End If
            End If

            'Identification si une fenêtre a été créée
            If traitementDataTable.Rows(i)("oa_traitement_fenetre") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_fenetre") = "1" Then
                    Fenetre = True
                End If
            End If

            'Date de début de fenêtre
            If traitementDataTable.Rows(i)("oa_traitement_fenetre_date_debut") IsNot DBNull.Value Then
                dateDebutFenetre = traitementDataTable.Rows(i)("oa_traitement_fenetre_date_debut")
            Else
                dateDebutFenetre = Nothing
            End If

            'Date de fin de fenêtre
            If traitementDataTable.Rows(i)("oa_traitement_fenetre_date_fin") IsNot DBNull.Value Then
                datefinFenetre = traitementDataTable.Rows(i)("oa_traitement_fenetre_date_fin")
            Else
                datefinFenetre = Nothing
            End If

            'Identification si le traitement a été arrêté
            If traitementDataTable.Rows(i)("oa_traitement_arret") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_arret") = "A" Then
                    Arret = True
                End If
            End If

            'Identification si le traitement a été annulé
            If traitementDataTable.Rows(i)("oa_traitement_annulation") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_annulation") = "A" Then
                    Annulation = True
                End If
            End If

            'Identification si le traitement a été déclaré "Allergie"
            If traitementDataTable.Rows(i)("oa_traitement_allergie") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_allergie") = "1" Then
                    allergie = True
                End If
            End If

            'Identification si le traitement a été déclaré "Contre-indication"
            If traitementDataTable.Rows(i)("oa_traitement_contre_indication") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_contre_indication") = "1" Then
                    contreIndication = True
                End If
            End If


            'Alimentation de la >Grid
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            TraitementDataGridView.Rows.Insert(iGrid)

            '------------------- Alimentation du DataGridView

            'Date historisation
            dateHisto = traitementDataTable.Rows(i)("oa_traitement_histo_date_historisation")
            TraitementDataGridView("histoDate", iGrid).Value = dateHisto.ToString("dd.MM.yyyy HH:mm:ss")

            'Utilisateur
            Dim UtilisateurId As Integer = traitementDataTable.Rows(i)("oa_traitement_histo_utilisateur_historisation")
            SetUtilisateur(UtilisateurHisto, UtilisateurId)
            TraitementDataGridView("histoUtilisateur", iGrid).Value = Me.UtilisateurHisto.UtilisateurPrenom & " " & Me.UtilisateurHisto.UtilisateurNom

            'Nature de l'action
            TraitementDataGridView("histoNature", iGrid).Value = natureHistoString

            'Date début
            TraitementDataGridView("dateDebut", iGrid).Value = dateDebut.ToString("dd.MM.yyyy")
            If dateDebut <> DateDebutComp Or premierPassage = True Then
                TraitementDataGridView("dateDebut", iGrid).Style.ForeColor = Color.Red
            End If
            DateDebutComp = dateDebut

            'Date fin
            If dateFin = "31/12/2999" Then
                TraitementDataGridView("dateFin", iGrid).Value = "Non définie"
            Else
                TraitementDataGridView("dateFin", iGrid).Value = dateFin.ToString("dd.MM.yyyy")
            End If
            'TraitementDataGridView("dateFin", iGrid).Value = dateFin.ToString("dd.MM.yyyy")
            If dateFin <> DateFinComp Or premierPassage = True Then
                TraitementDataGridView("dateFin", iGrid).Style.ForeColor = Color.Red
            End If
            DateFinComp = dateFin

            'Ordre d'affichage
            TraitementDataGridView("ordreAffichage", iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_ordre_affichage").ToString
            If traitementDataTable.Rows(i)("oa_traitement_ordre_affichage").ToString <> OrdreAffichageComp Or premierPassage = True Then
                TraitementDataGridView("ordreAffichage", iGrid).Style.ForeColor = Color.Red
            End If
            OrdreAffichageComp = traitementDataTable.Rows(i)("oa_traitement_ordre_affichage").ToString

            'Commentaire traitement
            TraitementDataGridView("commentaire", iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_commentaire")
            If traitementDataTable.Rows(i)("oa_traitement_commentaire").ToString <> TraitementCommentaireComp Or premierPassage = True Then
                TraitementDataGridView("commentaire", iGrid).Style.ForeColor = Color.Red
            End If
            TraitementCommentaireComp = traitementDataTable.Rows(i)("oa_traitement_commentaire").ToString

            'Posologie
            TraitementDataGridView("posologie", iGrid).Value = Posologie
            If Posologie <> PosologieComp Or premierPassage = True Then
                TraitementDataGridView("posologie", iGrid).Style.ForeColor = Color.Red
            End If
            PosologieComp = Posologie

            'Durée posologie
            If dateFin = "31/12/2999" Then
                TraitementDataGridView("PosologieDuree", iGrid).Value = ""
            Else
                TraitementDataGridView("PosologieDuree", iGrid).Value = CalculDureeTraitement(dateDebut, dateFin)
            End If
            If TraitementDataGridView("PosologieDuree", iGrid).Value <> PosologieDureeComp Or premierPassage = True Then
                TraitementDataGridView("posologieDuree", iGrid).Style.ForeColor = Color.Red
            End If
            PosologieDureeComp = TraitementDataGridView("PosologieDuree", iGrid).Value

            'Commentaire posologie
            TraitementDataGridView("posologieCommentaire", iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_posologie_commentaire")
            If traitementDataTable.Rows(i)("oa_traitement_posologie_commentaire").ToString <> PosologieCommentaireComp Or premierPassage = True Then
                TraitementDataGridView("posologieCommentaire", iGrid).Style.ForeColor = Color.Red
            End If
            PosologieCommentaireComp = traitementDataTable.Rows(i)("oa_traitement_posologie_commentaire").ToString

            'Fenetre
            If Fenetre = True Then
                TraitementDataGridView("fenetre", iGrid).Value = "Oui"
            Else
                TraitementDataGridView("fenetre", iGrid).Value = "Non"
            End If
            If Fenetre <> FenetreComp Or premierPassage = True Then
                TraitementDataGridView("fenetre", iGrid).Style.ForeColor = Color.Red
            End If
            FenetreComp = Fenetre

            'Date début fenêtre
            If dateDebutFenetre = Nothing Then
                TraitementDataGridView("fenetreDateDebut", iGrid).Value = "Non définie"
            Else
                TraitementDataGridView("fenetreDateDebut", iGrid).Value = dateDebutFenetre.ToString("dd.MM.yyyy")
            End If
            If dateDebutFenetre <> DateDebutFenetreComp Or premierPassage = True Then
                TraitementDataGridView("fenetreDateDebut", iGrid).Style.ForeColor = Color.Red
            End If
            DateDebutFenetreComp = dateDebutFenetre

            'Date fin fenêtre
            If datefinFenetre = Nothing Then
                TraitementDataGridView("fenetreDateFin", iGrid).Value = "Non définie"
            Else
                TraitementDataGridView("fenetreDateFin", iGrid).Value = datefinFenetre.ToString("dd.MM.yyyy")
            End If
            If datefinFenetre <> DateFinFenetreComp Or premierPassage = True Then
                TraitementDataGridView("fenetreDateFin", iGrid).Style.ForeColor = Color.Red
            End If
            DateFinFenetreComp = datefinFenetre

            'Commentaire fenêtre
            TraitementDataGridView("fenetreCommentaire", iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_fenetre_commentaire")
            If traitementDataTable.Rows(i)("oa_traitement_fenetre_commentaire").ToString <> FenetreCommentaireComp Or premierPassage = True Then
                TraitementDataGridView("fenetreCommentaire", iGrid).Style.ForeColor = Color.Red
            End If
            FenetreCommentaireComp = traitementDataTable.Rows(i)("oa_traitement_fenetre_commentaire").ToString

            'Arrêt traitement
            If Arret = True Then
                TraitementDataGridView("arret", iGrid).Value = "Oui"
            Else
                TraitementDataGridView("arret", iGrid).Value = "Non"
            End If
            If Arret <> ArretComp Or premierPassage = True Then
                TraitementDataGridView("arret", iGrid).Style.ForeColor = Color.Red
            End If
            ArretComp = Arret

            'Commentaire arrêt
            TraitementDataGridView("arretCommentaire", iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_arret_commentaire")
            If traitementDataTable.Rows(i)("oa_traitement_arret_commentaire").ToString <> ArretCommentaireComp Or premierPassage = True Then
                TraitementDataGridView("arretCommentaire", iGrid).Style.ForeColor = Color.Red
            End If
            ArretCommentaireComp = traitementDataTable.Rows(i)("oa_traitement_arret_commentaire").ToString

            'Allergie
            If allergie = True Then
                TraitementDataGridView("allergie", iGrid).Value = "Oui"
            Else
                TraitementDataGridView("allergie", iGrid).Value = "Non"
            End If
            If allergie <> AllergieComp Or premierPassage = True Then
                TraitementDataGridView("allergie", iGrid).Style.ForeColor = Color.Red
            End If
            AllergieComp = allergie

            'Contre-indication
            If contreIndication = True Then
                TraitementDataGridView("contreIndication", iGrid).Value = "Oui"
            Else
                TraitementDataGridView("contreIndication", iGrid).Value = "Non"
            End If
            If contreIndication <> ContreIndicationComp Or premierPassage = True Then
                TraitementDataGridView("contreIndication", iGrid).Style.ForeColor = Color.Red
            End If
            ContreIndicationComp = contreIndication

            'Annulation
            If Annulation = True Then
                TraitementDataGridView("annulation", iGrid).Value = "Oui"
            Else
                TraitementDataGridView("annulation", iGrid).Value = "Non"
            End If
            If Annulation <> AnnulationComp Or premierPassage = True Then
                TraitementDataGridView("annulation", iGrid).Style.ForeColor = Color.Red
            End If
            AnnulationComp = Annulation

            'Commentaire annulation
            TraitementDataGridView("annulationCommentaire", iGrid).Value = traitementDataTable.Rows(i)("oa_traitement_annulation_commentaire")
            If traitementDataTable.Rows(i)("oa_traitement_annulation_commentaire").ToString <> AnnulationCommentaireComp Or premierPassage = True Then
                TraitementDataGridView("annulationCommentaire", iGrid).Style.ForeColor = Color.Red
            End If
            AnnulationCommentaireComp = traitementDataTable.Rows(i)("oa_traitement_annulation_commentaire").ToString

            premierPassage = False
        Next

        conxn.Close()
        traitementDataAdapter.Dispose()

        'Enlève le focus sur la première ligne du Grid
        TraitementDataGridView.ClearSelection()
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
        'Traitements
        TraitementDataGridView.Rows.Clear()
    End Sub

    Private Sub TraitementDataGridView_DoubleClick(sender As Object, e As EventArgs) Handles TraitementDataGridView.DoubleClick
        'Dim TraitementId As Integer
        'TraitementId = TraitementDataGridView.Rows(TraitementDataGridView.CurrentRow.Index).Cells("TraitementId").Value

        'Dim vFTraitementDetailEdit As New FTraitementDetailEdit
        'vFTraitementDetailEdit.SelectedTraitementId = TraitementId
        'vFTraitementDetailEdit.SelectedPatient = Me.SelectedPatient
        'vFTraitementDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte

        'vFTraitementDetailEdit.ShowDialog() 'Modal

        'vFTraitementDetailEdit.Dispose()

        ''TraitementDataGridView.Rows.Clear()
        ''ChargementTraitement()
    End Sub
End Class