Imports System.Data.SqlClient
Imports Oasis_Common
Public Class RadFAntecedentAffectationSelecteur
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateAntecedentIdaAffecter As Integer
    Private privateAntecedentDescriptionaAffecter As String
    Private privateNiveauAntecedentaAffecter As Integer
    Private privateAntecedentIdPere As Integer
    Private privateCodeRetour As Boolean
    Private _positionGaucheDroite As Integer

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

    Public Property AntecedentIdaAffecter As Integer
        Get
            Return privateAntecedentIdaAffecter
        End Get
        Set(value As Integer)
            privateAntecedentIdaAffecter = value
        End Set
    End Property

    Public Property NiveauAntecedentaAffecter As Integer
        Get
            Return privateNiveauAntecedentaAffecter
        End Get
        Set(value As Integer)
            privateNiveauAntecedentaAffecter = value
        End Set
    End Property

    Public Property AntecedentIdPere As Integer
        Get
            Return privateAntecedentIdPere
        End Get
        Set(value As Integer)
            privateAntecedentIdPere = value
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

    Public Property AntecedentDescriptionaAffecter As String
        Get
            Return privateAntecedentDescriptionaAffecter
        End Get
        Set(value As String)
            privateAntecedentDescriptionaAffecter = value
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

    Dim conxn As New SqlConnection(getConnectionString())
    Dim NouveauOrdreAffichage As Integer
    Dim iGridMax As Integer
    Private Sub RadFAntecedentAffectationSelecteur_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If PositionGaucheDroite = EnumPosition.Droite Then
            Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - Me.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        Else
            Me.Location = New Point(10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        End If

        RadBtnConfirmer.Hide()
        ChargementEtatCivil()
        ChargementAntecedent()
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
    End Sub

    'Chargement de la Grid
    Private Sub ChargementAntecedent()
        'Alimentation des données pour l'affichage en en-tête de l'antécédent à affecter
        TxtAntecedentAAffecter.Text = AntecedentDescriptionaAffecter

        If NiveauAntecedentaAffecter <> 1 Then
            ChkAntecedentaAffecter.Checked = True
            ChkAntecedentMajeur.Checked = False
            ChkAntecedentMajeur.Show()
            ChkAntecedentaAffecter.Show()
        Else
            ChkAntecedentaAffecter.Checked = True
            ChkAntecedentaAffecter.Hide()
            ChkAntecedentMajeur.Checked = False
            ChkAntecedentMajeur.Hide()
            LblAffectationOu.Text = "Sélectionner un antécédent dans la liste ci-dessous, auquel l'antécédent ci-dessus sera affecté"
        End If

        'Déclaration des données de connexion
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " and (oa_antecedent_niveau = 1 or oa_antecedent_niveau = 2) order by oa_antecedent_ordre_affichage1, oa_antecedent_ordre_affichage2, oa_antecedent_ordre_affichage3;"

        'Lecture des données en base
        antecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        antecedentDataAdapter.Fill(antecedentDataTable)
        conxn.Open()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim dateDateModification As Date
        Dim AfficheDateModification As String
        Dim niveau As Integer
        Dim AntecedentIdNiveau1 As Integer

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            'Niveau de l'antécédent
            niveau = 0
            If antecedentDataTable.Rows(i)("oa_antecedent_niveau") IsNot DBNull.Value Then
                niveau = CInt(antecedentDataTable.Rows(i)("oa_antecedent_niveau"))
            End If

            'Id de l'antécédent de niveau 1 (pour les anétcédent de niveau 2)
            AntecedentIdNiveau1 = 0
            If niveau = 2 Then
                If antecedentDataTable.Rows(i)("oa_antecedent_id_niveau1") IsNot DBNull.Value Then
                    AntecedentIdNiveau1 = CInt(antecedentDataTable.Rows(i)("oa_antecedent_id_niveau1"))
                End If
            End If

            '-------> Traitement des antécédents à exclure de l'affichage
            'Ne pas afficher l'antécédent à réordonner
            If AntecedentIdaAffecter = antecedentDataTable.Rows(i)("oa_antecedent_id") Then
                Continue For
            End If
            'Ne pas afficher les antécédents fils de l'antécédent à réaffecter (on ne peut pas affecter le père au fils sinon celui-ci deviendrait orphelin)
            If niveau = 2 Then
                If AntecedentIdNiveau1 = AntecedentIdaAffecter Then
                    Continue For
                End If
            End If
            'Ne pas afficher l'antécédent père (ce qui n'a pas de sens dans le cas d'une réaffectation)
            If AntecedentIdPere = antecedentDataTable.Rows(i)("oa_antecedent_id") Then
                Continue For
            End If
            '-------> Traitement des antécédents à exclure de l'affichage

            'Recherche si le contexte médical a été modifié
            AfficheDateModification = ""
            If antecedentDataTable.Rows(i)("oa_antecedent_date_modification") IsNot DBNull.Value Then
                dateDateModification = antecedentDataTable.Rows(i)("oa_antecedent_date_modification")
                AfficheDateModification = " (" + dateDateModification.ToString("MM.yyyy") + ")"
            Else
                If antecedentDataTable.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                    dateDateModification = antecedentDataTable.Rows(i)("oa_antecedent_date_creation")
                    AfficheDateModification = " (" + dateDateModification.ToString("MM.yyyy") + ")"
                End If
            End If

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            RadAntecedentDataGridView.Rows.Add(iGrid)

            'Alimentation du DataGridView
            If antecedentDataTable.Rows(i)("oa_antecedent_description") Is DBNull.Value Or antecedentDataTable.Rows(i)("oa_antecedent_description") = "" Then
                'Récupération du libellé de la DRC/ORC
                Dim Drc As Drc = New Drc(antecedentDataTable.Rows(i)("oa_antecedent_drc_id"))
                RadAntecedentDataGridView.Rows(iGrid).Cells("antecedent").Value = Drc.DrcLibelle + AfficheDateModification

            Else
                RadAntecedentDataGridView.Rows(iGrid).Cells("antecedent").Value = antecedentDataTable.Rows(i)("oa_antecedent_description") + AfficheDateModification
            End If

            'Alimentation ordre affichage
            If antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage1") IsNot DBNull.Value Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("ordreAffichage1").Value = antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage1")
            Else
                RadAntecedentDataGridView.Rows(iGrid).Cells("ordreAffichage1").Value = 0
            End If

            If antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage2") IsNot DBNull.Value Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("ordreAffichage2").Value = antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage2")
            Else
                RadAntecedentDataGridView.Rows(iGrid).Cells("ordreAffichage2").Value = 0
            End If

            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentId").Value = antecedentDataTable.Rows(i)("oa_antecedent_id")

            RadAntecedentDataGridView.Rows(iGrid).Cells("niveau").Value = niveau

            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentIdNiveau1").Value = AntecedentIdNiveau1
        Next

        'Récupération du nombre de lignes stockées dans la Grid
        iGridMax = iGrid

        'Positionnement du grid sur la première occurrence
        If RadAntecedentDataGridView.Rows.Count > 0 Then
            Me.RadAntecedentDataGridView.CurrentRow = RadAntecedentDataGridView.ChildRows(0)
        End If

        'Libération des ressources
        conxn.Close()
        antecedentDataAdapter.Dispose()
    End Sub

    'Confirmation de l'affectation
    Private Sub BtnConfirmer_Click(sender As Object, e As EventArgs) Handles RadBtnConfirmer.Click
        Dim antecedentIdSelected, antecedentIdNiveau1, NiveauAntecedentSelected, ordre1AntecedentSelected, ordre2AntecedentSelected As Integer
        Dim NiveauAffected, AntecedentId1, AntecedentId2, ordre1, ordre2, ordre3 As Integer
        Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
        antecedentIdSelected = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
        ordre1AntecedentSelected = RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage1").Value
        ordre2AntecedentSelected = RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage2").Value
        NiveauAntecedentSelected = CInt(RadAntecedentDataGridView.Rows(aRow).Cells("niveau").Value)
        antecedentIdNiveau1 = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentIdNiveau1").Value
        If ChkAntecedentMajeur.Checked = True Then
            NiveauAffected = 1
            AntecedentId1 = 0
            AntecedentId2 = 0
            ordre1 = 990
            ordre2 = 0
            ordre3 = 0
        Else
            Select Case NiveauAntecedentSelected
                Case 1
                    NiveauAffected = 2
                    AntecedentId1 = antecedentIdSelected
                    AntecedentId2 = 0
                    ordre1 = ordre1AntecedentSelected
                    ordre2 = 990
                    ordre3 = 0
                Case 2
                    NiveauAffected = 3
                    AntecedentId1 = antecedentIdNiveau1
                    AntecedentId2 = antecedentIdSelected
                    ordre1 = ordre1AntecedentSelected
                    ordre2 = ordre2AntecedentSelected
                    ordre3 = 990
            End Select
        End If

        UpdateAntecedentaAffecter(AntecedentIdaAffecter, NiveauAffected, AntecedentId1, AntecedentId2, ordre1, ordre2, ordre3)
        AntecedentReorganisationOrdre(AntecedentIdaAffecter, NiveauAffected)

        Select Case NiveauAntecedentaAffecter 'Niveau actuel de l'antécédent à affecter
            Case 1
                Select Case NiveauAffected  'Nouveau niveau affecté à l'antécédent
                    Case 1 'Antécédent de niveau 1 reste niveau 1
                        'Cas sans objet : Un antécédent de niveau 1 n'ayant pas de père ne peut pas réaffecté
                    Case 2 'Antécédent niveau 1 devient niveau 2
                        '(1) Réaffecter les antécédents liés de niveau 2 en niveau 3
                        AffectationAntecedenetsLies(1, AntecedentIdaAffecter, antecedentIdSelected, ordre1AntecedentSelected)
                        AntecedentReorganisationOrdre(0, 1)
                        AntecedentReorganisationOrdre(antecedentIdSelected, 2)
                        AntecedentReorganisationOrdre(AntecedentIdaAffecter, 3)
                        '(5) Occulter les antécédents liés de niveau 3
                        AffectationAntecedenetsLies(5, AntecedentIdaAffecter, 0, 0)
                    Case 3 'Antécédent niveau 1 devient niveau 3
                        AntecedentReorganisationOrdre(antecedentIdSelected, 3)
                        '(4) Occulter les antécédents liés de niveau 2
                        AffectationAntecedenetsLies(4, AntecedentIdaAffecter, 0, 0)
                        '(5) Occulter les antécédents liés de niveau 3
                        AffectationAntecedenetsLies(5, AntecedentIdaAffecter, 0, 0)
                End Select
            Case 2
                Select Case NiveauAffected
                    Case 1 'Antécédent niveau 2 devient niveau 1 (Majeur)
                        '(3) Réaffecter les antécédents liés de niveau 3 en niveau 2
                        AffectationAntecedenetsLies(3, AntecedentIdaAffecter, 0, 990)
                        AntecedentReorganisationOrdre(AntecedentIdaAffecter, NiveauAffected)
                        AntecedentReorganisationOrdre(AntecedentIdaAffecter, 2)
                    Case 2 'Antécédent niveau 2 devient reste niveau 2, mais change de père
                        '(2) Réaffecter les antécédents liés de niveau 3
                        AffectationAntecedenetsLies(2, AntecedentIdaAffecter, antecedentIdSelected, ordre1AntecedentSelected)
                        AntecedentReorganisationOrdre(antecedentIdSelected, 2)
                        AntecedentReorganisationOrdre(AntecedentIdaAffecter, 3)
                    Case 3 'Antécédent niveau 2 devient niveau 3
                        '(6) Occulter les antécédents liés de niveau 3
                        AntecedentReorganisationOrdre(antecedentIdSelected, 3)
                        AffectationAntecedenetsLies(6, AntecedentIdaAffecter, 0, 0)
                End Select
            Case 3
                Select Case NiveauAffected
                    Case 1 'Antécédent niveau 3 devient niveau 1 (Majeur)
                        AntecedentReorganisationOrdre(0, 1)
                        'AntecedentReorganisationOrdre(AntecedentIdaAffecter, 3)
                    Case 2 'Antécédent niveau 3 devient niveau 2
                        AntecedentReorganisationOrdre(antecedentIdSelected, 2)
                        'AntecedentReorganisationOrdre(AntecedentIdaAffecter, 3)
                    Case 3 'Antécédent niveau 3 reste niveau 3, mais change de père
                        AntecedentReorganisationOrdre(antecedentIdSelected, 3)
                End Select
        End Select

        If CodeRetour = True Then
            Close()
        End If
    End Sub

    Private Function UpdateAntecedentaAffecter(antecedentId As Integer, Niveau As Integer, AntecedentId1 As Integer, AntecedentId2 As Integer, ordre1 As Integer, ordre2 As Integer, ordre3 As Integer) As Boolean
        Dim conxn2 As New SqlConnection(getConnectionString())
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        CodeRetour = True

        Dim dateModification As Date = Date.Now.Date
        Dim SQLstring As String

        SQLstring = "update oasis.oa_antecedent set oa_antecedent_niveau = @niveau, oa_antecedent_id_niveau1 = @antecedentId1, oa_antecedent_id_niveau2 = @antecedentId2, oa_antecedent_ordre_affichage1 = @ordre1, oa_antecedent_ordre_affichage2 = @ordre2, oa_antecedent_ordre_affichage3 = @ordre3 where oa_antecedent_id = @antecedentId"

        Dim cmd As New SqlCommand(SQLstring, conxn2)

        With cmd.Parameters
            .AddWithValue("@niveau", Niveau.ToString)
            .AddWithValue("@antecedentId1", AntecedentId1.ToString)
            .AddWithValue("@antecedentId2", AntecedentId2.ToString)
            .AddWithValue("@ordre1", ordre1.ToString)
            .AddWithValue("@ordre2", ordre2.ToString)
            .AddWithValue("@ordre3", ordre3.ToString)
            .AddWithValue("@antecedentId", antecedentId.ToString)
        End With

        Try
            conxn2.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            MessageBox.Show(ex.Message)
            CodeRetour = False
        Finally
            conxn2.Close()
        End Try

        Return CodeRetour
    End Function

    'Mise à jour de l'ordre des antécédents en réorganisant l'ordre sur un pas de 20
    Private Function AntecedentReorganisationOrdre(AntecedentId As Integer, niveau As Integer) As Boolean
        'Déclaration des données de connexion
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        Select Case niveau
            Case 1
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " and oa_antecedent_niveau = 1 order by oa_antecedent_ordre_affichage1;"
            Case 2
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " and oa_antecedent_id_niveau1 = " + AntecedentId.ToString + " and oa_antecedent_niveau = 2 order by oa_antecedent_ordre_affichage2;"
            Case 3
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " and oa_antecedent_id_niveau2 = " + AntecedentId.ToString + " and oa_antecedent_niveau = 3 order by oa_antecedent_ordre_affichage3;"
            Case Else
                Return False
        End Select

        'Lecture des données en base
        antecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        antecedentDataAdapter.Fill(antecedentDataTable)
        conxn.Open()

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1
        Dim ordreAffichage As Integer = 0

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            'Ajout d'une ligne au DataGridView
            ordreAffichage += 20
            Dim AntecedentIdAModifier As Integer = CInt(antecedentDataTable.Rows(i)("oa_antecedent_id"))
            UpdateOrdreAffichageAntecedent(AntecedentIdAModifier, niveau, ordreAffichage)
            AffectationOrdreAntecedenetsLies(AntecedentIdAModifier, niveau, ordreAffichage)
        Next

        conxn.Close()

        Return CodeRetour
    End Function


    'Affectation aux antécédents fils, de l'ordre d'affichage attribué à l'antécédent père
    Private Function AffectationOrdreAntecedenetsLies(antecedentIdRef As Integer, niveau As Integer, OrdreAffichageRef As Integer) As Boolean
        'Déclaration des données de connexion
        Dim conxn3 As New SqlConnection(getConnectionString())
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        Select Case niveau
            Case 1
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " and (oa_antecedent_niveau = 2 or oa_antecedent_niveau = 3) and oa_antecedent_id_niveau1 = " + antecedentIdRef.ToString + ";"
            Case 2
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " and oa_antecedent_niveau = 3 and oa_antecedent_id_niveau2 = " + antecedentIdRef.ToString + ";"
            Case Else
                Return False
        End Select

        'Lecture des données en base
        antecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        antecedentDataAdapter.Fill(antecedentDataTable)
        conxn3.Open()


        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1
        'Parcours du DataTable pour mettre à jour l'ordre d'affichage
        For i = 0 To rowCount Step 1
            Dim AntecedentIdaTraiter As Integer = CInt(antecedentDataTable.Rows(i)("oa_antecedent_id"))
            UpdateOrdreAffichageAntecedent(AntecedentIdaTraiter, niveau, OrdreAffichageRef)
        Next

        conxn3.Close()

        Return CodeRetour
    End Function

    Private Function UpdateOrdreAffichageAntecedent(antecedentId As Integer, niveau As Integer, OrdreAffichage As Integer) As Boolean
        Dim conxn2 As New SqlConnection(getConnectionString())
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        CodeRetour = True

        Dim dateModification As Date = Date.Now.Date
        Dim SQLstring As String

        Select Case niveau
            Case 1
                SQLstring = "update oasis.oa_antecedent set oa_antecedent_ordre_affichage1 = @ordreAffichage where oa_antecedent_id = @antecedentId"
            Case 2
                SQLstring = "update oasis.oa_antecedent set oa_antecedent_ordre_affichage2 = @ordreAffichage where oa_antecedent_id = @antecedentId"
            Case 3
                SQLstring = "update oasis.oa_antecedent set oa_antecedent_ordre_affichage3 = @ordreAffichage where oa_antecedent_id = @antecedentId"
            Case Else
                Return False
        End Select

        Dim cmd As New SqlCommand(SQLstring, conxn2)

        With cmd.Parameters
            .AddWithValue("@ordreAffichage", OrdreAffichage.ToString)
            .AddWithValue("@antecedentId", antecedentId.ToString)
        End With

        Try
            conxn2.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            'PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            CodeRetour = False
        Finally
            conxn2.Close()
        End Try

        Return CodeRetour
    End Function

    'Affectation aux antécédents fils, des éléments du père ou occultation des fils
    Private Function AffectationAntecedenetsLies(Traitement As Integer, antecedentIdaAffecter As Integer, antecedentIdCible As Integer, Ordre1 As Integer) As Boolean
        'Déclaration des données de connexion
        Dim conxn3 As New SqlConnection(getConnectionString())
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        Select Case Traitement
            Case 1, 4
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " and oa_antecedent_niveau = 2 and oa_antecedent_id_niveau1 = " + antecedentIdaAffecter.ToString + ";"
            Case 2, 3, 6
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " and oa_antecedent_niveau = 3 and oa_antecedent_id_niveau2 = " + antecedentIdaAffecter.ToString + ";"
            Case 5
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " and oa_antecedent_niveau = 3 and oa_antecedent_id_niveau1 = " + antecedentIdaAffecter.ToString + ";"
            Case Else
                Return False
        End Select

        'Lecture des données en base
        antecedentDataAdapter.SelectCommand = New SqlCommand(SQLString, conxn)
        antecedentDataAdapter.Fill(antecedentDataTable)
        conxn3.Open()


        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1
        'Parcours du DataTable pour mettre à jour l'ordre d'affichage
        For i = 0 To rowCount Step 1
            Dim AntecedentIdaTraiter As Integer = CInt(antecedentDataTable.Rows(i)("oa_antecedent_id"))
            Select Case Traitement
                Case 1, 2
                    UpdateAntecedentaAffecter(AntecedentIdaTraiter, 3, antecedentIdCible, antecedentIdaAffecter, Ordre1, 990, 990)
                Case 3
                    UpdateAntecedentaAffecter(AntecedentIdaTraiter, 2, antecedentIdaAffecter, 0, 990, 990, 0)
                Case 4, 5, 6
                    UpdateOccultationAntecedent(AntecedentIdaTraiter)
            End Select
        Next

        conxn3.Close()

        Return CodeRetour
    End Function


    Private Function UpdateOccultationAntecedent(antecedentId As Integer) As Boolean
        Dim conxn2 As New SqlConnection(getConnectionString())
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        CodeRetour = True

        Dim dateModification As Date = Date.Now.Date
        Dim SQLstring As String

        SQLstring = "update oasis.oa_antecedent set oa_antecedent_niveau = 1, oa_antecedent_id_niveau1 = 0, oa_antecedent_id_niveau2 = 0, oa_antecedent_ordre_affichage1 = 0, oa_antecedent_ordre_affichage2 = 0, oa_antecedent_ordre_affichage3 = 0, oa_antecedent_statut_affichage = 'O' where oa_antecedent_id = @antecedentId"

        Dim cmd As New SqlCommand(SQLstring, conxn2)

        With cmd.Parameters
            .AddWithValue("@antecedentId", antecedentId.ToString)
        End With

        Try
            conxn2.Open()
            da.UpdateCommand = cmd
            da.UpdateCommand.ExecuteNonQuery()
        Catch ex As Exception
            'PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            CodeRetour = False
        Finally
            conxn2.Close()
        End Try

        Return CodeRetour
    End Function

    Private Sub BtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Me.CodeRetour = False
        Close()
    End Sub

    Private Sub ChkAntecedentMajeur_CheckedChanged(sender As Object, e As EventArgs) Handles ChkAntecedentMajeur.CheckedChanged
        If ChkAntecedentMajeur.Checked = True Then
            ChkAntecedentaAffecter.Checked = False
            RadBtnConfirmer.Show()
        Else
            If ChkAntecedentaAffecter.Checked = False Then
                ChkAntecedentMajeur.Checked = True
            End If
        End If
    End Sub

    Private Sub ChkAntecedentaAffecter_CheckedChanged(sender As Object, e As EventArgs) Handles ChkAntecedentaAffecter.CheckedChanged
        If ChkAntecedentaAffecter.Checked = True Then
            ChkAntecedentMajeur.Checked = False
            RadBtnConfirmer.Hide()
        Else
            If ChkAntecedentMajeur.Checked = False Then
                ChkAntecedentaAffecter.Checked = True
            End If
        End If
    End Sub

    Private Sub RadAntecedentDataGridView_CellClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadAntecedentDataGridView.CellClick
        RadBtnConfirmer.Show()
    End Sub
End Class
