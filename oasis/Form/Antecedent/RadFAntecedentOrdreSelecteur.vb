Imports System.Data.SqlClient
Imports Oasis_Common
Public Class RadFAntecedentOrdreSelecteur
    Private privateSelectedPatient As Patient
    Private privateUtilisateurConnecte As Utilisateur
    Private privateAntecedentIdaOrdonner As Integer
    Private privateAntecedentDescriptionAOrdonner As String
    Private privateNiveauAntecedentAOrdonner As Integer
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

    Public Property AntecedentIdaOrdonner As Integer
        Get
            Return privateAntecedentIdaOrdonner
        End Get
        Set(value As Integer)
            privateAntecedentIdaOrdonner = value
        End Set
    End Property

    Public Property NiveauAntecedentAOrdonner As Integer
        Get
            Return privateNiveauAntecedentAOrdonner
        End Get
        Set(value As Integer)
            privateNiveauAntecedentAOrdonner = value
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

    Public Property AntecedentDescriptionAOrdonner As String
        Get
            Return privateAntecedentDescriptionAOrdonner
        End Get
        Set(value As String)
            privateAntecedentDescriptionAOrdonner = value
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
    Dim Traitement As Integer
    Dim iGridMax As Integer
    Private Enum EnumTraitement
        AOrdonner = 1
        MiseAJour = 2
    End Enum

    Private Sub RadFAntecedentOrdreSelecteur_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If PositionGaucheDroite = EnumPosition.Droite Then
            Me.Location = New Point(Screen.PrimaryScreen.WorkingArea.Width - Me.Width - 10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        Else
            Me.Location = New Point(10, Screen.PrimaryScreen.WorkingArea.Height - Me.Height - 10)
        End If
        Me.Traitement = EnumTraitement.AOrdonner
        RadBtnConfirmerApres.Hide()
        RadBtnConfirmerAvant.Hide()
        ChargementPatient()
        ChargementAntecedent()
    End Sub

    Private Sub ChargementPatient()
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
        'Alimentation des données pour l'affichage en en-tête de l'antécédent à ordonner
        TxtAntecedentAOrdonner.Text = AntecedentDescriptionAOrdonner

        'Déclaration des données de connexion
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "SELECT * FROM oasis.oa_antecedent" &
        " WHERE oa_antecedent_type = 'A'" &
        " AND oa_antecedent_statut_affichage = 'P'" &
        " AND (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null)" &
        " AND oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString &
        " AND oa_antecedent_niveau = " + NiveauAntecedentAOrdonner.ToString &
        " ORDER BY oa_antecedent_ordre_affichage1, oa_antecedent_ordre_affichage2, oa_antecedent_ordre_affichage3;"

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
        Dim ordreAffichage As Integer = 0

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
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

            'Sélection des antécédents du même groupe (pour les niveau 2 et 3)
            Select Case NiveauAntecedentAOrdonner
                Case 2
                    If antecedentDataTable.Rows(i)("oa_antecedent_id_niveau1") IsNot DBNull.Value Then
                        If antecedentDataTable.Rows(i)("oa_antecedent_id_niveau1") <> AntecedentIdPere Then
                            Continue For
                        End If
                    End If
                Case 3
                    If antecedentDataTable.Rows(i)("oa_antecedent_id_niveau2") IsNot DBNull.Value Then
                        If antecedentDataTable.Rows(i)("oa_antecedent_id_niveau2") <> AntecedentIdPere Then
                            Continue For
                        End If
                    End If
            End Select

            'Ne pas afficher l'antécédent à réordonner
            If Traitement = EnumTraitement.AOrdonner Then
                If AntecedentIdaOrdonner = antecedentDataTable.Rows(i)("oa_antecedent_id") Then
                    Continue For
                End If
            End If

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            ordreAffichage += 20
            RadAntecedentDataGridView.Rows.Add(iGrid)

            'Alimentation du DataGridView
            If antecedentDataTable.Rows(i)("oa_antecedent_description") Is DBNull.Value Or antecedentDataTable.Rows(i)("oa_antecedent_description") = "" Then
                'Récupération du libellé de la DRC/ORC
                Dim Drc As Drc = New Drc(antecedentDataTable.Rows(i)("oa_antecedent_drc_id"))
                RadAntecedentDataGridView.Rows(iGrid).Cells("antecedent").Value = Drc.DrcLibelle + AfficheDateModification + " "

            Else
                RadAntecedentDataGridView.Rows(iGrid).Cells("antecedent").Value = antecedentDataTable.Rows(i)("oa_antecedent_description") + AfficheDateModification + " "
            End If

            'Alimentation ordre affichage
            If antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage1") IsNot DBNull.Value Then
                RadAntecedentDataGridView.Rows(iGrid).Cells("ordreAffichage").Value = antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage1")
            Else
                RadAntecedentDataGridView.Rows(iGrid).Cells("ordreAffichage").Value = 0
            End If
            RadAntecedentDataGridView.Rows(iGrid).Cells("ordreAffichage").Value = ordreAffichage.ToString("0000")

            RadAntecedentDataGridView.Rows(iGrid).Cells("antecedentId").Value = antecedentDataTable.Rows(i)("oa_antecedent_id")
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

    'Mise à jour de l'ordre des antécédents en réorganisant l'ordre sur un pas de 20
    Private Function AntecedentReorganisationOrdre(niveau As Integer) As Boolean
        'Déclaration des données de connexion
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        Select Case niveau
            Case 1
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " and oa_antecedent_niveau = 1 order by oa_antecedent_ordre_affichage1;"
            Case 2
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " and oa_antecedent_id_niveau1 = " + AntecedentIdPere.ToString + " and oa_antecedent_niveau = 2 order by oa_antecedent_ordre_affichage2;"
            Case 3
                SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + SelectedPatient.patientId.ToString + " and oa_antecedent_id_niveau2 = " + AntecedentIdPere.ToString + " and oa_antecedent_niveau = 3 order by oa_antecedent_ordre_affichage3;"
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
            Dim AntecedentId As Integer = CInt(antecedentDataTable.Rows(i)("oa_antecedent_id"))
            UpdateAntecedent(AntecedentId, ordreAffichage)
            AffectationOrdreAntecedenetsLies(AntecedentId, niveau, ordreAffichage)
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
            UpdateAntecedent(AntecedentIdaTraiter, OrdreAffichageRef)
        Next

        conxn3.Close()

        Return CodeRetour
    End Function

    Private Function UpdateAntecedent(antecedentId As Integer, ordreAffichage As Integer) As Boolean
        Dim conxn2 As New SqlConnection(getConnectionString())
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        CodeRetour = True

        Dim dateModification As Date = Date.Now.Date
        Dim SQLstring As String

        Select Case NiveauAntecedentAOrdonner
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
            .AddWithValue("@ordreAffichage", ordreAffichage.ToString)
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

    Private Sub RadAntecedentDataGridView_CellClick(sender As Object, e As Telerik.WinControls.UI.GridViewCellEventArgs) Handles RadAntecedentDataGridView.CellClick
        RadBtnConfirmerApres.Show()
        RadBtnConfirmerAvant.Show()
    End Sub

    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Me.CodeRetour = False
        Close()
    End Sub

    'Mise à jour de l'ordre des antécédents en base de données à partir de la valeur attribuée dans la DataGrid
    Private Function AntecedentModificationOrdre() As Boolean

        For i = 0 To iGridMax Step 1
            Dim ordreAffichage As Integer = RadAntecedentDataGridView.Rows(i).Cells("ordreAffichage").Value
            Dim AntecedentId As Integer = CInt(RadAntecedentDataGridView.Rows(i).Cells("antecedentId").Value)
            UpdateAntecedent(AntecedentId, ordreAffichage)
        Next

        Return CodeRetour
    End Function

    Private Sub RadBtnConfirmerApres_Click(sender As Object, e As EventArgs) Handles RadBtnConfirmerApres.Click
        Dim antecedentId As Integer
        Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
        If aRow >= 0 Then
            antecedentId = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
            NouveauOrdreAffichage = CInt(RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage").Value) + 10
            'Ajout ligne présente dans l'entête
            RadAntecedentDataGridView.Rows.Add(TxtAntecedentAOrdonner.Text, NouveauOrdreAffichage.ToString, AntecedentIdaOrdonner.ToString)
            iGridMax += 1

            AntecedentModificationOrdre()
            AntecedentReorganisationOrdre(NiveauAntecedentAOrdonner)
            If CodeRetour = True Then
                Close()
            End If
        End If
    End Sub

    Private Sub RadBtnConfirmerAvant_Click(sender As Object, e As EventArgs) Handles RadBtnConfirmerAvant.Click
        Dim antecedentId As Integer
        Dim aRow As Integer = Me.RadAntecedentDataGridView.Rows.IndexOf(Me.RadAntecedentDataGridView.CurrentRow)
        If aRow >= 0 Then
            antecedentId = RadAntecedentDataGridView.Rows(aRow).Cells("antecedentId").Value
            NouveauOrdreAffichage = CInt(RadAntecedentDataGridView.Rows(aRow).Cells("ordreAffichage").Value) - 10
            'Ajout ligne présente dans l'entête
            RadAntecedentDataGridView.Rows.Add(TxtAntecedentAOrdonner.Text, NouveauOrdreAffichage.ToString, AntecedentIdaOrdonner.ToString)
            iGridMax += 1

            AntecedentModificationOrdre()
            AntecedentReorganisationOrdre(NiveauAntecedentAOrdonner)
            If CodeRetour = True Then
                Close()
            End If
        End If
    End Sub
End Class
