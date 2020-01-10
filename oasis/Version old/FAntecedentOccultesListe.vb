Imports System.Data.SqlClient
Public Class FAntecedentOccultesListe
    Private privateUtilisateurConnecte As Utilisateur
    Private privateCodeRetour As Boolean

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

    Dim conxn As New SqlConnection(getConnectionString())
    Dim NouveauOrdreAffichage As Integer
    Dim Traitement As Integer
    Dim iGridMax As Integer
    Private Enum EnumTraitement
        AOrdonner = 1
        MiseAJour = 2
    End Enum

    Private Sub FAntecedentOrdreSelecteur_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Me.Traitement = EnumTraitement.AOrdonner
        BtnConfirmer.Hide()
        ChargementAntecedent()
    End Sub

    'Chargement de la Grid
    Private Sub ChargementAntecedent()
        'Déclaration des données de connexion
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'O' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) order by oa_antecedent_ordre_affichage1, oa_antecedent_ordre_affichage2, oa_antecedent_ordre_affichage3;"

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

            'Ajout d'une ligne au DataGridView
            iGrid += 1
            ordreAffichage += 20
            AntecedentDataGridView.Rows.Insert(iGrid)

            'Alimentation du DataGridView
            If antecedentDataTable.Rows(i)("oa_antecedent_description") Is DBNull.Value Or antecedentDataTable.Rows(i)("oa_antecedent_description") = "" Then
                'Récupération du libellé de la DRC/ORC
                Dim Drc As Drc = New Drc(antecedentDataTable.Rows(i)("oa_antecedent_drc_id"))
                AntecedentDataGridView("antecedent", iGrid).Value = Drc.DrcLibelle + AfficheDateModification + " "
            Else
                AntecedentDataGridView("antecedent", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_description") + AfficheDateModification + " "
            End If

            'Alimentation ordre affichage
            If antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage1") IsNot DBNull.Value Then
                AntecedentDataGridView("ordreAffichage", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_ordre_affichage1")
            Else
                AntecedentDataGridView("ordreAffichage", iGrid).Value = 0
            End If
            AntecedentDataGridView("ordreAffichage", iGrid).Value = ordreAffichage.ToString("0000")

            AntecedentDataGridView("antecedentId", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_id")
            AntecedentDataGridView("patientId", iGrid).Value = antecedentDataTable.Rows(i)("oa_antecedent_patient_id")
        Next

        'Récupération du nombre de lignes stockées dans la Grid
        iGridMax = iGrid

        'Enlève le focus sur la première ligne de la Grid
        AntecedentDataGridView.ClearSelection()

        'Libération des ressources
        conxn.Close()
        antecedentDataAdapter.Dispose()
    End Sub

    Private Sub BtnConfirmer_Click(sender As Object, e As EventArgs) Handles BtnConfirmer.Click
        Dim antecedentId, patientId As Integer
        antecedentId = AntecedentDataGridView.Rows(AntecedentDataGridView.CurrentRow.Index).Cells("antecedentId").Value
        patientId = AntecedentDataGridView.Rows(AntecedentDataGridView.CurrentRow.Index).Cells("patientId").Value

        If UpdateAntecedentSelected(antecedentId) = True Then
            AntecedentDataGridView.Rows.Clear()
            ChargementAntecedent()
        End If
    End Sub

    Private Sub BtnAbandonner_Click(sender As Object, e As EventArgs) Handles BtnAbandonner.Click
        Me.CodeRetour = False
        Close()
    End Sub

    Private Function UpdateAntecedentSelected(antecedentId As Integer) As Boolean
        Dim conxn2 As New SqlConnection(getConnectionString())
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        CodeRetour = True

        Dim dateModification As Date = Date.Now.Date
        Dim SQLstring As String

        SQLstring = "update oasis.oa_antecedent set oa_antecedent_niveau = @niveau, oa_antecedent_id_niveau1 = @antecedentId1, oa_antecedent_id_niveau2 = @antecedentId2, oa_antecedent_ordre_affichage1 = @ordre1, oa_antecedent_ordre_affichage2 = @ordre2, oa_antecedent_ordre_affichage3 = @ordre3, oa_antecedent_statut_affichage = @publication where oa_antecedent_id = @antecedentId;"

        Dim cmd As New SqlCommand(SQLstring, conxn2)

        With cmd.Parameters
            .AddWithValue("@niveau", 1)
            .AddWithValue("@antecedentId1", 0)
            .AddWithValue("@antecedentId2", 0)
            .AddWithValue("@ordre1", 990)
            .AddWithValue("@ordre2", 0)
            .AddWithValue("@ordre3", 0)
            .AddWithValue("@publication", "P")
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
    Private Function AntecedentReorganisationOrdre(patientId As Integer) As Boolean
        'Déclaration des données de connexion
        Dim antecedentDataAdapter As SqlDataAdapter = New SqlDataAdapter()
        Dim antecedentDataTable As DataTable = New DataTable()
        Dim SQLString As String

        SQLString = "select * from oasis.oa_antecedent where oa_antecedent_type = 'A' and oa_antecedent_statut_affichage = 'P' and (oa_antecedent_inactif = '0' or oa_antecedent_inactif is Null) and oa_antecedent_patient_id = " + patientId.ToString + " and oa_antecedent_niveau = 1 order by oa_antecedent_ordre_affichage1;"

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
        Next

        conxn.Close()

        Return CodeRetour
    End Function

    Private Function UpdateAntecedent(antecedentId As Integer, ordreAffichage As Integer) As Boolean
        Dim conxn2 As New SqlConnection(getConnectionString())
        Dim da As SqlDataAdapter = New SqlDataAdapter()
        CodeRetour = True

        Dim dateModification As Date = Date.Now.Date
        Dim SQLstring As String
        SQLstring = "update oasis.oa_antecedent set oa_antecedent_ordre_affichage1 = @ordreAffichage where oa_antecedent_id = @antecedentId;"

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
            'PgbMiseAJour.Hide()
            MessageBox.Show(ex.Message)
            CodeRetour = False
        Finally
            conxn2.Close()
        End Try

        Return CodeRetour
    End Function

    Private Sub AntecedentDataGridView_CellClick(sender As Object, e As DataGridViewCellEventArgs) Handles AntecedentDataGridView.CellClick
        BtnConfirmer.Show()
    End Sub
End Class