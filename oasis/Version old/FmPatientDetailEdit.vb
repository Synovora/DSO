Imports System.Data.SqlClient
Imports Oasis_Common
Public Class FmPatientDetailEdit
    Private privateSelectedPatientId As Integer
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateCodeRetour As Boolean
    Private _Action As String

    Public Property SelectedPatientId As Integer
        Get
            Return privateSelectedPatientId
        End Get
        Set(value As Integer)
            privateSelectedPatientId = value
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

    Public Property CodeRetour As Boolean
        Get
            Return privateCodeRetour
        End Get
        Set(value As Boolean)
            privateCodeRetour = value
        End Set
    End Property

    Public Property SelectedPatient As Patient
        Get
            Return privateSelectedPatient
        End Get
        Set(value As Patient)
            privateSelectedPatient = value
        End Set
    End Property

    Public Property Action As String
        Get
            Return _Action
        End Get
        Set(value As String)
            _Action = value
        End Set
    End Property

    Enum EnumEditMode
        Creation = 1
        Modification = 2
    End Enum

    Dim EditMode As Integer
    Dim utilisateurHisto As Utilisateur = New Utilisateur()
    'Dim conxn As New MySqlConnection(getConnectionString())
    Dim conxn As New SqlConnection(getConnectionString())
    Dim uniteSanitaireListe As Dictionary(Of Integer, String) = Table_unite_sanitaire.GetUniteSanitaireListe()
    'Dim siteListe As Dictionary(Of Integer, String) = Table_site.GetSiteListe()
    Dim genreListe As Dictionary(Of String, String) = Table_genre.GetGenreListe()
    Private Sub FmPatientDetailEdit_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        InitZone()
        InitAction()
        If SelectedPatientId <> 0 Then
            EditMode = EnumEditMode.Modification
            RadBtnValidationDateNaissance.Hide()
            ChargementPatient()
            ChargementNotesPatient()
            InhiberZoneEnSaisie()
        Else
            InhiberZoneEnSaisie()
            DteDateNaissance.Enabled = True

            EditMode = EnumEditMode.Creation
            LblIdentifiantOasis.Hide()
            LblLabelIdOasis.Hide()
            RadBtnModifier.Hide()
            RadBtnSortieOasis.Hide()
            RadBtnValider.Show()
            RadBtnValider.Enabled = True
            RadNotePatientDataGridView.Hide()
            NoteContextMenuStrip.Enabled = False
            Me.Width = 590
        End If
    End Sub

    'Chargement de la Grid Notes patient
    Private Sub ChargementNotesPatient()
        'Déclaration des données de connexion
        'Dim conxn As New MySqlConnection(getConnectionString())
        'Dim notePatientDataAdapter As MySqlDataAdapter = New MySqlDataAdapter()
        Dim conxn As New SqlConnection(getConnectionString())
        Dim notePatientDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim NotePatientDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select * from oasis.oa_patient_note where (oa_patient_note_invalide = '0' or oa_patient_note_invalide is Null)" &
            " And oa_patient_id = " + SelectedPatient.patientId.ToString + " order by oa_patient_note_date_creation desc;"

        'Lecture des données en base
        'notePatientDataAdapter.SelectCommand = New MySqlCommand(SQLString, conxn)
        notePatientDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        notePatientDataAdapter.Fill(NotePatientDataTable)
        conxn.Open()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateCreation As Date
        Dim AfficheDateCreation, NotePatient, Auteur As String
        Dim AuteurId As Integer
        Dim rowCount As Integer = NotePatientDataTable.Rows.Count - 1

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            If NotePatientDataTable.Rows(i)("oa_patient_note") IsNot DBNull.Value Then
                NotePatient = NotePatientDataTable.Rows(i)("oa_patient_note")
            Else
                NotePatient = ""
            End If

            'Utilisateur creation
            Auteur = ""
            If NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation") IsNot DBNull.Value Then
                If NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation") <> 0 Then
                    Dim UtilisateurCreation = New Utilisateur()
                    SetUtilisateur(utilisateurHisto, NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation"))
                    Auteur = Me.utilisateurHisto.UtilisateurPrenom & " " & Me.utilisateurHisto.UtilisateurNom
                End If
            End If

            'Date création
            AfficheDateCreation = ""
            If NotePatientDataTable.Rows(i)("oa_patient_note_date_creation") IsNot DBNull.Value Then
                dateCreation = NotePatientDataTable.Rows(i)("oa_patient_note_date_creation")
                AfficheDateCreation = outils.FormatageDateAffichage(dateCreation)
            Else
                If NotePatientDataTable.Rows(i)("oa_patient_note_date_creation") IsNot DBNull.Value Then
                    dateCreation = NotePatientDataTable.Rows(i)("oa_patient_note_date_creation")
                    AfficheDateCreation = outils.FormatageDateAffichage(dateCreation)
                End If
            End If

            AuteurId = 0
            If NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation") IsNot DBNull.Value Then
                AuteurId = NotePatientDataTable.Rows(i)("oa_patient_note_utilisateur_creation")
            End If

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadNotePatientDataGridView.AutoSizeRows = True
            RadNotePatientDataGridView.Rows.Add(iGrid)

            'Alimentation du DataGridView
            'RadNotePatientDataGridView("note", iGrid).Value = NotePatient
            RadNotePatientDataGridView.Rows(iGrid).Cells("note").Value = NotePatient

            'Identifiant notePatient
            'RadNotePatientDataGridView("noteId", iGrid).Value = NotePatientDataTable.Rows(i)("oa_patient_note_id")
            RadNotePatientDataGridView.Rows(iGrid).Cells("noteId").Value = NotePatientDataTable.Rows(i)("oa_patient_note_id")

            'Auteur de la note
            'RadNotePatientDataGridView("auteur", iGrid).Value = Auteur & vbCrLf & AfficheDateCreation
            RadNotePatientDataGridView.Rows(iGrid).Cells("auteur").Value = Auteur & vbCrLf & AfficheDateCreation
        Next
        conxn.Close()
        notePatientDataAdapter.Dispose()

        'Enlève le focus sur la première ligne de la Grid
        If iGrid > -1 Then
            Me.RadNotePatientDataGridView.Rows(0).IsSelected = True
            Me.RadNotePatientDataGridView.Rows(0).IsCurrent = True
            Me.RadNotePatientDataGridView.Rows(0).EnsureVisible()
            'RadNotePatientDataGridView.ClearSelection()
        End If

    End Sub

    Private Sub RadBtnGoogleMaps_Click(sender As Object, e As EventArgs) Handles RadBtnGoogleMaps.Click
        Dim GoogleOK As Boolean = False
        If TxtAdresse1.Text <> "" Then
            If TxtCodePostal.Text <> "" Then
                If TxtVille.Text <> "" Then
                    'lancer l'URL pour afficher l'adresse dans Google Maps
                    GoogleOK = True
                    Dim MonURL As String
                    MonURL = "http://www.google.fr/maps/place/" + TxtAdresse1.Text + " " + TxtCodePostal.Text + " " + TxtVille.Text
                    Process.Start(MonURL)
                End If
            End If
        End If
        If GoogleOK = False Then
            MessageBox.Show("L'adresse 1, le code postal et la ville doivent être renseignés pour lancer cette fonctionnalité")
        End If
    End Sub

    Private Sub InitZone()
        TxtPrenom.Text = ""
        TxtNom.Text = ""
        DteDateNaissance.Value = DteDateNaissance.MinDate
        DteDateNaissance.Format = DateTimePickerFormat.Custom
        DteDateNaissance.CustomFormat = " "
        LblAge.Text = ""
        CbxGenre.SelectedItem = vbNull
        TxtNIR.Text = ""
        TxtAdresse1.Text = ""
        TxtAdresse2.Text = ""
        TxtCodePostal.Text = ""
        TxtVille.Text = ""
        TxtTelFixe.Text = ""
        TxtTelMobile.Text = ""
        TxtEmail.Text = ""
        DteDateEntree.Value = DteDateEntree.MaxDate
        DteDateEntree.Format = DateTimePickerFormat.Custom
        DteDateEntree.CustomFormat = " "
        CbxSite.SelectedItem = vbNull
        CbxUniteSanitaire.SelectedItem = vbNull
        ChkCouvertureInternet.Checked = False
        DteDateSortie.Value = DteDateSortie.MaxDate
        DteDateSortie.Format = DateTimePickerFormat.Custom
        DteDateSortie.CustomFormat = " "
        DteDateDeces.Value = DteDateDeces.MaxDate
        DteDateDeces.Format = DateTimePickerFormat.Custom
        DteDateDeces.CustomFormat = " "
        TxtCommentaireSortie.Text = ""
        GbxSortieOasis.Hide()

        'genre
        Dim indice As Integer = genreListe.Count - 1
        Dim genreDescription(indice) As String
        Dim i As Integer = 0

        For Each kvp As KeyValuePair(Of String, String) In genreListe
            genreDescription(i) = kvp.Value
            i += 1
        Next kvp
        CbxGenre.DataSource = genreDescription

        'Unité sanitaire
        indice = uniteSanitaireListe.Count - 1
        Dim uniteSanitaireDescription(indice) As String
        i = 0

        For Each kvp As KeyValuePair(Of Integer, String) In uniteSanitaireListe
            uniteSanitaireDescription(i) = kvp.Value
            i += 1
        Next kvp
        CbxUniteSanitaire.DataSource = uniteSanitaireDescription

        'Site
        'indice = siteListe.Count - 1
        'Dim siteDescription(indice) As String
        'i = 0

        'For Each kvp As KeyValuePair(Of Integer, String) In siteListe
        'siteDescription(i) = kvp.Value
        'i += 1
        'Next kvp
        'CbxSite.DataSource = siteDescription
    End Sub

    Private Sub ChargementPatient()
        LblIdentifiantOasis.Text = SelectedPatient.patientId
        TxtPrenom.Text = SelectedPatient.PatientPrenom
        TxtNom.Text = SelectedPatient.PatientNom

        If SelectedPatient.PatientDateNaissance < DteDateNaissance.MinDate Or SelectedPatient.PatientDateNaissance > DteDateNaissance.MaxDate Then
            DteDateNaissance.Value = DteDateNaissance.MinDate
        Else
            DteDateNaissance.Value = SelectedPatient.PatientDateNaissance
            DteDateNaissance.Format = DateTimePickerFormat.Long
        End If

        If SelectedPatient.PatientGenreId = "M" Then
            TxtNomMarital.Text = ""
            TxtNomMarital.Hide()
        End If

        LblAge.Text = SelectedPatient.PatientAge
        CbxGenre.SelectedItem = SelectedPatient.PatientGenre
        TxtNIR.Text = SelectedPatient.PatientNir
        TxtAdresse1.Text = SelectedPatient.PatientAdresse1
        TxtAdresse2.Text = SelectedPatient.PatientAdresse2
        TxtCodePostal.Text = SelectedPatient.PatientCodePostal
        TxtVille.Text = SelectedPatient.PatientVille

        TxtTelFixe.Text = SelectedPatient.PatientTel1

        TxtTelMobile.Text = SelectedPatient.PatientTel2
        TxtEmail.Text = SelectedPatient.PatientEmail

        Dim PatientgSiteDescription As String = Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        CbxSite.SelectedItem = PatientgSiteDescription

        Dim PatientgUniteSanitaireDescription As String = Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)
        CbxUniteSanitaire.SelectedItem = PatientgUniteSanitaireDescription

        If SelectedPatient.PatientInternet = True Then
            ChkCouvertureInternet.Checked = True
        Else
            ChkCouvertureInternet.Checked = False
        End If

        'Date entrée
        If SelectedPatient.PatientDateEntree > DteDateEntree.MaxDate Then
            DteDateEntree.Value = DteDateEntree.MaxDate
        Else
            DteDateEntree.Value = SelectedPatient.PatientDateEntree
        End If
        If DteDateEntree.Value <> DteDateEntree.MaxDate Then
            DteDateEntree.Format = DateTimePickerFormat.Long
        End If

        'Date sortie
        If SelectedPatient.PatientDateSortie < DteDateSortie.MaxDate Then
            DteDateSortie.Value = DteDateSortie.MaxDate
        Else
            DteDateSortie.Value = SelectedPatient.PatientDateSortie
        End If
        If DteDateSortie.Value <> DteDateSortie.MaxDate Then
            DteDateSortie.Format = DateTimePickerFormat.Long
        End If

        'Date décès
        If SelectedPatient.PatientDateDeces < DteDateDeces.MinDate Or SelectedPatient.PatientDateDeces > DteDateDeces.MaxDate Then
            DteDateDeces.Value = DteDateDeces.MaxDate
        Else
            DteDateDeces.Value = SelectedPatient.PatientDateDeces
        End If
        If DteDateDeces.Value <> DteDateDeces.MaxDate Then
            DteDateDeces.Format = DateTimePickerFormat.Long
        End If

        TxtCommentaireSortie.Text = SelectedPatient.PatientCommentaireSortie

        If SelectedPatient.PatientUniteSanitaireId <> 0 Then
            'Unité sanitaire id
            For Each kvp As KeyValuePair(Of Integer, String) In uniteSanitaireListe
                If kvp.Key = SelectedPatient.PatientUniteSanitaireId Then
                    CbxUniteSanitaire.SelectedItem = kvp.Value
                    Exit For
                End If
            Next kvp

            'Chargement combo site par rapport à l'unité sanitaire choisie
            Dim siteListeParUniteSanitaire As Dictionary(Of Integer, String) = Table_site.GetSiteListeParUniteSanitaire(SelectedPatient.PatientUniteSanitaireId)
            Dim indice As Integer = siteListeParUniteSanitaire.Count - 1
            Dim siteDescription(indice) As String
            Dim i As Integer = 0

            For Each kvp As KeyValuePair(Of Integer, String) In siteListeParUniteSanitaire
                siteDescription(i) = kvp.Value
                i += 1
            Next kvp

            CbxSite.DataSource = siteDescription

            If SelectedPatient.PatientSiteId <> 0 Then
                'Unité sanitaire id
                For Each kvp As KeyValuePair(Of Integer, String) In siteListeParUniteSanitaire
                    If kvp.Key = SelectedPatient.PatientSiteId Then
                        CbxSite.SelectedItem = kvp.Value
                        Exit For
                    End If
                Next kvp
            End If
        End If


    End Sub

    Private Function ValidationDonnéesSaisies() As Boolean
        Dim Valide As Boolean = True
        Dim zoneObligatoire As Boolean = True
        Dim zoneNumerique As Boolean = True
        Dim messageErreur As String = ""
        Dim messageErreur1 As String = ""
        Dim messageErreur2 As String = ""
        Dim messageErreur3 As String = ""
        Dim messageErreur4 As String = ""

        'Nom, Prenom, date naissance, genre, adresse 1, code postal et ville obligatoire
        If TxtPrenom.Text = "" Then
            zoneObligatoire = False
        End If
        If TxtNom.Text = "" Then
            zoneObligatoire = False
        End If
        If CbxGenre.SelectedValue = "" Then
            zoneObligatoire = False
        End If
        If DteDateNaissance.Value = DteDateNaissance.MinDate Then
            zoneObligatoire = False
        End If
        If zoneObligatoire = False Then
            messageErreur1 = "- Les zones : prénom, nom, date de naissance et genre sont obligatoires"
            Valide = False
        End If

        'Les zones : NIR, téléphone fixe, téléphone mobile et code postal doivent être numériques si elles sont saisies
        If TxtNIR.Text.Trim <> "" Then
            If Not IsNumeric(TxtNIR.Text) Then
                zoneNumerique = False
            End If
        End If

        If TxtTelFixe.Text.Trim <> "" Then
            If Not IsNumeric(TxtTelFixe.Text) Then
                zoneNumerique = False
            End If
        End If

        If TxtTelMobile.Text.Trim <> "" Then
            If Not IsNumeric(TxtTelMobile.Text) Then
                zoneNumerique = False
            End If
        End If

        If TxtCodePostal.Text.Trim <> "" Then
            If Not IsNumeric(TxtCodePostal.Text) Then
                zoneNumerique = False
            End If
        End If

        If zoneNumerique = False Then
            messageErreur2 = "- Les zones : NIR, Téléphone fixe, téléphone mobile et code postal doivent être numériques si elles sont saisies"
            Valide = False
        End If

        If EditMode = EnumEditMode.Creation Then
            'Contrôle de l'existence du NIR
            If IsNumeric(TxtNIR.Text) Then
                Dim NirPatient As Int64
                NirPatient = TxtNIR.Text
                If NonExistencePatientNIR(NirPatient, 0) = False Then
                    messageErreur3 = "- Le NIR saisie existe déjà pour un autre patient défini dans le référentiel d'Oasis, création impossible"
                    Valide = False
                End If
            End If
        End If

        If EditMode = EnumEditMode.Modification Then
            'Contrôle de l'existence du NIR
            If IsNumeric(TxtNIR.Text) Then
                If TxtNIR.Text <> "0" Then
                    Dim NirPatient As Int64
                    NirPatient = TxtNIR.Text
                    If NonExistencePatientNIR(NirPatient, SelectedPatientId) = False Then
                        messageErreur3 = "- Le NIR saisie existe déjà pour un autre patient défini dans le référentiel d'Oasis, modification impossible"
                        Valide = False
                    End If
                End If
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

    Private Function ModificationPatient() As Boolean
        'Dim da As MySqlDataAdapter = New MySqlDataAdapter()
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim GenreId As String
        Dim SiteId, UniteSanitaireId, Internet, Modulo As Integer
        Dim NIR As Int64

        Dim SQLstring As String = "update oasis.oa_patient set oa_patient_nir = @nir, oa_patient_nir_modulo = @nirModulo, oa_patient_prenom = @prenom, oa_patient_nom = @nom, oa_patient_nom_marital = @nomMarital, oa_patient_date_naissance = @dateNaissance, oa_patient_genre_id = @genreId, oa_patient_adresse1 = @adresse1, oa_patient_adresse2 = @adresse2, oa_patient_code_postal = @codePostal, oa_patient_ville = @ville, oa_patient_tel1 = @tel1, oa_patient_tel2 = @tel2, oa_patient_email = @email, oa_patient_date_entree_oasis = @dateEntree, oa_patient_date_sortie_oasis = @dateSortie, oa_patient_commentaire_sortie = @commentaireSortie, oa_patient_date_deces = @dateDeces, oa_patient_site_id = @siteId, oa_patient_unite_sanitaire_id = @uniteSanitaireId, oa_patient_couverture_internet = @internet where oa_patient_id = @patientId"

        'Dim cmd As New MySqlCommand(SQLstring, conxn)
        Dim cmd As New SqlCommand(SQLstring, conxn)

        'Modulo

        'Genre id
        GenreId = 0
        For Each kvp As KeyValuePair(Of String, String) In genreListe
            If kvp.Value = CbxGenre.SelectedValue Then
                GenreId = kvp.Key
                Exit For
            End If
        Next kvp

        'Unité sanitaire id
        UniteSanitaireId = 0
        For Each kvp As KeyValuePair(Of Integer, String) In uniteSanitaireListe
            If kvp.Value = CbxUniteSanitaire.SelectedValue Then
                UniteSanitaireId = kvp.Key
                Exit For
            End If
        Next kvp

        'Site id
        Dim siteListeParUniteSanitaire As Dictionary(Of Integer, String) = Table_site.GetSiteListeParUniteSanitaire(UniteSanitaireId)
        SiteId = 0
        For Each kvp As KeyValuePair(Of Integer, String) In siteListeParUniteSanitaire
            If kvp.Value = CbxSite.SelectedValue Then
                SiteId = kvp.Key
                Exit For
            End If
        Next kvp

        'Internet
        If ChkCouvertureInternet.Checked = True Then
            Internet = True
        Else
            Internet = False
        End If

        'NIR
        If TxtNIR.Text <> "" And IsNumeric(TxtNIR.Text) Then
            NIR = CDec(TxtNIR.Text)
        Else
            NIR = 0
        End If


        With cmd.Parameters
            .AddWithValue("@nir", NIR.ToString)
            .AddWithValue("@nirModulo", Modulo.ToString)
            .AddWithValue("@prenom", TxtPrenom.Text)
            .AddWithValue("@nom", TxtNom.Text)
            .AddWithValue("@nomMarital", TxtNomMarital.Text)
            .AddWithValue("@dateNaissance", DteDateNaissance.Value.ToString("yyyy-MM-dd"))
            .AddWithValue("@genreId", GenreId.ToString)
            .AddWithValue("@adresse1", TxtAdresse1.Text)
            .AddWithValue("@adresse2", TxtAdresse2.Text)
            .AddWithValue("@codePostal", TxtCodePostal.Text)
            .AddWithValue("@ville", TxtVille.Text)
            .AddWithValue("@tel1", TxtTelFixe.Text)
            .AddWithValue("@tel2", TxtTelMobile.Text)
            .AddWithValue("@email", TxtEmail.Text)
            .AddWithValue("@dateEntree", DteDateEntree.Value.ToString("yyyy-MM-dd"))
            .AddWithValue("@dateSortie", DteDateSortie.Value.ToString("yyyy-MM-dd"))
            .AddWithValue("@commentaireSortie", TxtCommentaireSortie.Text)
            .AddWithValue("@dateDeces", DteDateDeces.Value.ToString("yyyy-MM-dd"))
            .AddWithValue("@siteId", SiteId.ToString)
            .AddWithValue("@uniteSanitaireId", UniteSanitaireId.ToString)
            .AddWithValue("@internet", Internet.ToString)
            .AddWithValue("@patientId", SelectedPatient.patientId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()

            RadDesktopAlert1.CaptionText = "Mise à jour du patient"
            RadDesktopAlert1.CaptionText = "La modification du patient a été réalisée avec succès"
            RadDesktopAlert1.Show()
            Action = "Modification"
            'MessageBox.Show("patient modifié")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation traitement

            'Rechargement du Bean Patient
            SelectedPatient.PatientNir = TxtNIR.Text
            SelectedPatient.PatientPrenom = TxtPrenom.Text
            SelectedPatient.PatientNom = TxtNom.Text
            SelectedPatient.PatientNomMarital = TxtNomMarital.Text
            SelectedPatient.PatientDateNaissance = DteDateNaissance.Value
            SelectedPatient.PatientGenreId = GenreId
            SelectedPatient.PatientAdresse1 = TxtAdresse1.Text
            SelectedPatient.PatientAdresse2 = TxtAdresse2.Text
            SelectedPatient.PatientCodePostal = TxtCodePostal.Text
            SelectedPatient.PatientVille = TxtVille.Text
            SelectedPatient.PatientTel1 = TxtTelFixe.Text
            SelectedPatient.PatientTel2 = TxtTelMobile.Text
            SelectedPatient.PatientEmail = TxtEmail.Text
            SelectedPatient.PatientDateEntree = DteDateEntree.Value
            SelectedPatient.PatientDateSortie = DteDateSortie.Value
            SelectedPatient.PatientDateDeces = DteDateDeces.Value
            SelectedPatient.PatientCommentaireSortie = TxtCommentaireSortie.Text
            SelectedPatient.PatientSiteId = SiteId
            SelectedPatient.PatientUniteSanitaireId = UniteSanitaireId
            SelectedPatient.PatientInternet = Internet
        End If

        Return codeRetour
    End Function

    'Déclaration de sortie
    Private Function DeclarationSortie() As Boolean
        'Dim da As MySqlDataAdapter = New MySqlDataAdapter()
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True

        Dim SQLstring As String = "update oasis.oa_patient set oa_patient_date_sortie_oasis = @dateSortie, oa_patient_commentaire_sortie = @commentaireSortie where oa_patient_id = @patientId"

        'Dim cmd As New MySqlCommand(SQLstring, conxn)
        Dim cmd As New SqlCommand(SQLstring, conxn)

        With cmd.Parameters
            .AddWithValue("@dateSortie", DteDateSortie.Value.ToString("yyyy-MM-dd"))
            .AddWithValue("@commentaireSortie", TxtCommentaireSortie.Text)
            .AddWithValue("@patientId", SelectedPatient.patientId.ToString)
        End With

        Try
            conxn.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()

            MessageBox.Show("Déclaration de sortie du patient effectuée")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        If codeRetour = True Then
            'Mise à jour des données modifiées dans l'instance de la classe Historisation traitement
        End If

        Return codeRetour
    End Function

    Private Function CreationPatient() As Boolean
        'Dim da As MySqlDataAdapter = New MySqlDataAdapter()
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        Dim codeRetour As Boolean = True
        Dim GenreId As String
        Dim SiteId, UniteSanitaireId, Internet, Modulo As Integer
        Dim NIR As Int64

        'Dim SQLstring As String = "insert into oasis.oa_patient (oa_patient_id, oa_patient_nir, oa_patient_nir_modulo, oa_patient_prenom, oa_patient_nom, oa_patient_nom_marital, oa_patient_date_naissance, oa_patient_genre_id, oa_patient_adresse1, oa_patient_adresse2, oa_patient_code_postal, oa_patient_ville, oa_patient_tel1, oa_patient_tel2, oa_patient_email, oa_patient_date_entree_oasis, oa_patient_date_sortie_oasis, oa_patient_commentaire_sortie, oa_patient_date_deces, oa_patient_site_id, oa_patient_unite_sanitaire_id, oa_patient_couverture_internet) VALUES (@patientId, @nir, @nirModulo, @prenom, @nom, @nomMarital, @dateNaissance, @genreId, @adresse1, @adresse2, @codePostal, @ville, @tel1, @tel2, @email, @dateEntree, @dateSortie, @commentaireSortie, @dateDeces, @siteId, @uniteSanitaireId, @internet)"
        Dim SQLstring As String = "insert into oasis.oa_patient (oa_patient_nir, oa_patient_nir_modulo, oa_patient_prenom, oa_patient_nom, oa_patient_nom_marital, oa_patient_date_naissance, oa_patient_genre_id, oa_patient_adresse1, oa_patient_adresse2, oa_patient_code_postal, oa_patient_ville, oa_patient_tel1, oa_patient_tel2, oa_patient_email, oa_patient_date_entree_oasis, oa_patient_date_sortie_oasis, oa_patient_commentaire_sortie, oa_patient_date_deces, oa_patient_site_id, oa_patient_unite_sanitaire_id, oa_patient_couverture_internet) VALUES (@nir, @nirModulo, @prenom, @nom, @nomMarital, @dateNaissance, @genreId, @adresse1, @adresse2, @codePostal, @ville, @tel1, @tel2, @email, @dateEntree, @dateSortie, @commentaireSortie, @dateDeces, @siteId, @uniteSanitaireId, @internet)"


        'Dim cmd As New MySqlCommand(SQLstring, conxn)
        Dim cmd As New SqlCommand(SQLstring, conxn)

        'NIR
        If TxtNIR.Text <> "" And IsNumeric(TxtNIR.Text) Then
            NIR = CDec(TxtNIR.Text)
        Else
            NIR = 0
        End If

        'Genre id
        GenreId = 0
        For Each kvp As KeyValuePair(Of String, String) In genreListe
            If kvp.Value = CbxGenre.SelectedValue Then
                GenreId = kvp.Key
                Exit For
            End If
        Next kvp

        'Site id
        Dim siteListeParUniteSanitaire As Dictionary(Of Integer, String) = Table_site.GetSiteListeParUniteSanitaire(UniteSanitaireId)
        SiteId = 0
        For Each kvp As KeyValuePair(Of Integer, String) In siteListeParUniteSanitaire
            If kvp.Value = CbxSite.SelectedValue Then
                SiteId = kvp.Key
                Exit For
            End If
        Next kvp

        'Unité sanitaire id
        UniteSanitaireId = 0
        For Each kvp As KeyValuePair(Of Integer, String) In uniteSanitaireListe
            If kvp.Value = CbxUniteSanitaire.SelectedValue Then
                UniteSanitaireId = kvp.Key
                Exit For
            End If
        Next kvp

        'Internet
        If ChkCouvertureInternet.Checked = True Then
            Internet = True
        Else
            Internet = False
        End If


        With cmd.Parameters
            '.AddWithValue("@patientId", SelectedPatient.patientId.ToString)
            .AddWithValue("@nir", NIR.ToString)
            .AddWithValue("@nirModulo", Modulo.ToString)
            .AddWithValue("@prenom", TxtPrenom.Text)
            .AddWithValue("@nom", TxtNom.Text)
            .AddWithValue("@nomMarital", TxtNomMarital.Text)
            .AddWithValue("@dateNaissance", DteDateNaissance.Value.ToString("yyyy-MM-dd"))
            .AddWithValue("@genreId", GenreId.ToString)
            .AddWithValue("@adresse1", TxtAdresse1.Text)
            .AddWithValue("@adresse2", TxtAdresse2.Text)
            .AddWithValue("@codePostal", TxtCodePostal.Text)
            .AddWithValue("@ville", TxtVille.Text)
            .AddWithValue("@tel1", TxtTelFixe.Text)
            .AddWithValue("@tel2", TxtTelMobile.Text)
            .AddWithValue("@email", TxtEmail.Text)
            .AddWithValue("@dateEntree", DteDateEntree.Value.ToString("yyyy-MM-dd"))
            .AddWithValue("@dateSortie", DteDateSortie.Value.ToString("yyyy-MM-dd"))
            .AddWithValue("@commentaireSortie", TxtCommentaireSortie.Text)
            .AddWithValue("@dateDeces", DteDateDeces.Value.ToString("yyyy-MM-dd"))
            .AddWithValue("@siteId", SiteId.ToString)
            .AddWithValue("@uniteSanitaireId", UniteSanitaireId.ToString)
            .AddWithValue("@internet", Internet.ToString)
        End With

        Try
            conxn.Open()
            da.InsertCommand = cmd
            da.InsertCommand.ExecuteNonQuery()
            MessageBox.Show("Patient créé")
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            codeRetour = False
        Finally
            conxn.Close()
        End Try

        'Traitement historisation patient créé
        'If codeRetour = True Then
        'Récupération de l'identifiant du patient créé
        'Dim antecedentLastDataReader As MySqlDataReader
        'SQLstring = "select max(oa_patient_id) from oa_patient where oa_patient_id = " & SelectedPatient.patientId & ";"
        'Dim antecedentLastCommand As New MySqlCommand(SQLstring, conxn)
        'conxn.Open()
        'antecedentLastDataReader = antecedentLastCommand.ExecuteReader()
        'If antecedentLastDataReader.Read() Then
        'Récupération de la clé de l'enregistrement créé
        'AntecedentHistoACreer.HistorisationAntecedentId = antecedentLastDataReader("max(oa_patient_id)")

        'Libération des ressources d'accès aux données
        'conxn.Close()
        'antecedentLastCommand.Dispose()
        'End If
        'End If

        Return codeRetour
    End Function

    Private Sub RadBtnModifier_Click(sender As Object, e As EventArgs) Handles RadBtnModifier.Click
        ActiverZoneEnSaisie()
        RadBtnSortieOasis.Enabled = False
        RadBtnValider.Enabled = True
        RadBtnModifier.Enabled = False
    End Sub

    'Appel saisie sortie Oasis
    Private Sub RadBtnSortieOasis_Click(sender As Object, e As EventArgs) Handles RadBtnSortieOasis.Click
        RadBtnSortieOasis.Hide()
        GbxSortieOasis.Show()
        InhiberZoneEnSaisie()
        RadBtnModifier.Enabled = False
        BtnCreerNote.Enabled = False
        DteDateSortie.Enabled = True
        DteDateSortie.Value = Date.Now
        DteDateSortie.Format = DateTimePickerFormat.Long
        TxtCommentaireSortie.Enabled = True
    End Sub

    'Validation sortie Oasis
    Private Sub RadBtnValidationSortie_Click(sender As Object, e As EventArgs) Handles RadBtnValidationSortie.Click
        If MsgBox("confirmation de la déclaration de sortie du patient", MsgBoxStyle.YesNo, "") = MsgBoxResult.Yes Then
            If DeclarationSortie() = True Then
                Me.CodeRetour = True
                Close()
            End If
        End If
    End Sub


    'Validation des modifications des données ou de la création d'un nouveau patient
    Private Sub RadBtnValider_Click(sender As Object, e As EventArgs) Handles RadBtnValider.Click
        If ValidationDonnéesSaisies() = True Then
            Select Case EditMode
                Case EnumEditMode.Creation
                    Me.CodeRetour = CreationPatient()
                Case EnumEditMode.Modification
                    Me.CodeRetour = ModificationPatient()
            End Select
            If Me.CodeRetour = True Then
                conxn.Dispose()
                Close()
            End If
        End If
    End Sub

    'Retour écran précédent sans action
    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Me.CodeRetour = False
        Close()
    End Sub

    Private Sub InhiberZoneEnSaisie()
        TxtPrenom.Enabled = False
        TxtNom.Enabled = False
        TxtNomMarital.Enabled = False
        DteDateNaissance.Enabled = False
        CbxGenre.Enabled = False
        TxtNIR.Enabled = False
        TxtAdresse1.Enabled = False
        TxtAdresse2.Enabled = False
        TxtCodePostal.Enabled = False
        TxtVille.Enabled = False
        TxtTelFixe.Enabled = False
        TxtTelMobile.Enabled = False
        TxtEmail.Enabled = False
        DteDateEntree.Enabled = False
        CbxSite.Enabled = False
        CbxUniteSanitaire.Enabled = False
        ChkCouvertureInternet.Enabled = False
        DteDateSortie.Enabled = False
        DteDateDeces.Enabled = False
        TxtCommentaireSortie.Enabled = False
    End Sub


    Private Sub ActiverZoneEnSaisie()
        TxtPrenom.Enabled = True
        TxtNom.Enabled = True
        TxtNomMarital.Enabled = True
        DteDateNaissance.Enabled = True
        CbxGenre.Enabled = True
        TxtNIR.Enabled = True
        TxtAdresse1.Enabled = True
        TxtAdresse2.Enabled = True
        TxtCodePostal.Enabled = True
        TxtVille.Enabled = True
        TxtTelFixe.Enabled = True
        TxtTelMobile.Enabled = True
        TxtEmail.Enabled = True
        DteDateEntree.Enabled = True
        CbxSite.Enabled = True
        CbxUniteSanitaire.Enabled = True
        ChkCouvertureInternet.Enabled = True
        DteDateSortie.Enabled = True
        DteDateDeces.Enabled = True
        TxtCommentaireSortie.Enabled = True
    End Sub

    Private Sub InitAction()
        RadBtnValider.Enabled = False
        RadBtnSortieOasis.Enabled = True
    End Sub

    'Gestion de l'affichage du contrôle de saisie de la date de naissance
    Private Sub DteDateNaissance_DropDown(sender As Object, e As EventArgs) Handles DteDateNaissance.DropDown
        If DteDateNaissance.Value = DteDateNaissance.MinDate Then
            DteDateNaissance.Value = Date.Now
            DteDateNaissance.Format = DateTimePickerFormat.Long
        End If
    End Sub

    'Gestion de l'affichage du contrôle de saisie de la date d'entrée
    Private Sub DteDateEntree_DropDown(sender As Object, e As EventArgs) Handles DteDateEntree.DropDown
        If DteDateEntree.Value = DteDateEntree.MaxDate Then
            DteDateEntree.Value = Date.Now
            DteDateEntree.Format = DateTimePickerFormat.Long
        End If
    End Sub

    'Gestion de l'affichage du contrôle de saisie de la date de décès
    Private Sub DteDateDeces_DropDown(sender As Object, e As EventArgs) Handles DteDateDeces.DropDown
        If DteDateDeces.Value = DteDateDeces.MaxDate Then
            DteDateDeces.Value = Date.Now
            DteDateDeces.Format = DateTimePickerFormat.Long
        End If
    End Sub

    'Gestion de l'affichage du contrôle de saisie de la date de sortie
    Private Sub DteDateSortie_DropDown(sender As Object, e As EventArgs) Handles DteDateSortie.DropDown
        If DteDateSortie.Value = DteDateSortie.MaxDate Then
            DteDateSortie.Value = Date.Now
            DteDateSortie.Format = DateTimePickerFormat.Long
        End If
    End Sub

    'Modification du genre par l'utilisateur
    Private Sub CbxGenre_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxGenre.SelectedIndexChanged
        If CbxGenre.SelectedValue = "Masculin" Then
            TxtNomMarital.Text = ""
            TxtNomMarital.Hide()
            LblNomMarital.Hide()
        Else
            TxtNomMarital.Show()
            LblNomMarital.Show()
        End If
    End Sub

    'Appel détail note en modification
    Private Sub RadNotePatientDataGridView_CellDoubleClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadNotePatientDataGridView.CellDoubleClick
        Dim aRow, maxRow As Integer
        aRow = Me.RadNotePatientDataGridView.Rows.IndexOf(Me.RadNotePatientDataGridView.CurrentRow)
        maxRow = RadNotePatientDataGridView.Rows.Count - 1

        If aRow <= maxRow And aRow > -1 Then
            'If RadNotePatientDataGridView.CurrentRow IsNot Nothing Then
            Dim NoteId As Integer = RadNotePatientDataGridView.Rows(aRow).Cells("noteId").Value

            Dim vFNotePatientDetailEdit As New FmPatientNoteDetailEdit
            vFNotePatientDetailEdit.SelectedNoteId = NoteId
            vFNotePatientDetailEdit.SelectedPatient = Me.SelectedPatient
            vFNotePatientDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte

            vFNotePatientDetailEdit.ShowDialog() 'Modal
            If vFNotePatientDetailEdit.CodeRetour = True Then
                RadNotePatientDataGridView.Rows.Clear()
                ChargementNotesPatient()
            End If

            vFNotePatientDetailEdit.Dispose()
        End If
    End Sub

    Private Sub BtnCreerNote_Click(sender As Object, e As EventArgs) Handles BtnCreerNote.Click
        CreerNote()
    End Sub
    'Appel détail note en création
    Private Sub CréerUneNoteToolStripMenuItem_Click_1(sender As Object, e As EventArgs) Handles CréerUneNoteToolStripMenuItem.Click
        CreerNote()
    End Sub

    Private Sub CreerNote()
        Dim vFNotePatientDetailEdit As New FmPatientNoteDetailEdit
        vFNotePatientDetailEdit.SelectedNoteId = 0
        vFNotePatientDetailEdit.SelectedPatient = Me.SelectedPatient
        vFNotePatientDetailEdit.UtilisateurConnecte = Me.UtilisateurConnecte

        vFNotePatientDetailEdit.ShowDialog() 'Modal
        If vFNotePatientDetailEdit.CodeRetour = True Then
            RadNotePatientDataGridView.Rows.Clear()
            ChargementNotesPatient()
        End If
        vFNotePatientDetailEdit.Dispose()
    End Sub
    Private Function CalculmoduloNIR(NIR As Int64) As Integer
        Dim Reste As Integer
        Reste = NIR Mod 97
        Return 97 - Reste
    End Function

    Private Sub TxtNIR_TextChanged(sender As Object, e As EventArgs) Handles TxtNIR.TextChanged
        If IsNumeric(TxtNIR.Text) Then
            Dim NIR As Int64 = CDec(TxtNIR.Text)
            Dim Modulo As Integer
            Modulo = CalculmoduloNIR(NIR)
            LblModulo.Text = Modulo
            LblModulo.Show()
        Else
            LblModulo.Hide()
        End If
    End Sub

    Private Sub CbxUniteSanitaire_SelectedIndexChanged(sender As Object, e As EventArgs) Handles CbxUniteSanitaire.SelectedIndexChanged
        Dim uniteSanitaireId, indice, i As Integer

        'Détermination de l'unité sanitaire id
        uniteSanitaireId = 0
        For Each kvp As KeyValuePair(Of Integer, String) In uniteSanitaireListe
            If kvp.Value = CbxUniteSanitaire.SelectedValue Then
                uniteSanitaireId = kvp.Key
                Exit For
            End If
        Next kvp

        'Site
        CbxSite.ResetText()

        If uniteSanitaireId <> 0 Then
            Dim siteListeParUniteSanitaire As Dictionary(Of Integer, String) = Table_site.GetSiteListeParUniteSanitaire(uniteSanitaireId)
            indice = siteListeParUniteSanitaire.Count - 1
            Dim siteDescription(indice) As String
            i = 0

            For Each kvp As KeyValuePair(Of Integer, String) In siteListeParUniteSanitaire
                siteDescription(i) = kvp.Value
                i += 1
            Next kvp
            CbxSite.DataSource = siteDescription
        End If

    End Sub

    Private Sub RadBtnValidationDateNaissance_Click(sender As Object, e As EventArgs) Handles RadBtnValidationDateNaissance.Click
        If EditMode = EnumEditMode.Creation Then
            If DteDateNaissance.Value <> DteDateNaissance.MinDate Then
                Dim DateNaissance As Date
                DateNaissance = DteDateNaissance.Value
                LblAge.Text = CalculAgeString(DateNaissance)

                'Vérifie s'il existe des patients ayant la même date de naissance
                Dim ListeDataTable As DataTable = ListePatientDateNaissance(DteDateNaissance.Value)
                If ListeDataTable.Rows.Count > 0 Then
                    Dim vFPatientListeDoublons As New FPatientListeDoublons
                    vFPatientListeDoublons.UtilisateurConnecte = Me.UtilisateurConnecte
                    vFPatientListeDoublons.ListeDataTable = ListeDataTable
                    vFPatientListeDoublons.ShowDialog() 'Modal
                    If vFPatientListeDoublons.CodeRetour = True Then
                        'Retour liste des patients
                        Close()
                    Else
                        'Retour écran de création d'un patient
                        ActiverZoneEnSaisie()
                    End If
                    vFPatientListeDoublons.Dispose()
                Else
                    'Si pas de patient avec cette date de naissance, activation des zones en saisie
                    ActiverZoneEnSaisie()
                End If
            End If
        End If
    End Sub

End Class