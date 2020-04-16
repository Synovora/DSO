Imports Oasis_Common
Imports Telerik.WinControls.UI
Imports Telerik.WinControls.UI.Localization

Public Class FrmUtilisateurListe

    Public Sub New()

        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        afficheTitleForm(Me, Me.Text)

        initCtrl()

    End Sub

    Private Sub initCtrl()

        refreshGrid()
    End Sub

    Private Sub refreshGrid()
        Dim exId As Long, index As Integer = -1, exPosit = 0, userDao = New UserDao

        Me.Cursor = Cursors.WaitCursor
        Try
            Dim data As DataTable = userDao.GetTableUtilisateurForGrid()

            Dim numRowGrid As Integer = 0

            ' -- recup eventuelle precedente selectionnée
            If RadGridView1.Rows.Count > 0 AndAlso Not IsNothing(Me.RadGridView1.CurrentRow) Then
                exId = Me.RadGridView1.CurrentRow.Cells("Id").Value
                exPosit = Me.RadGridView1.CurrentRow.Index
            End If
            RadGridView1.Rows.Clear()

            For Each row In data.Rows
                Dim newRow As GridViewRowInfo = RadGridView1.Rows.NewRow()
                '------------------- Alimentation du DataGridView
                With newRow
                    .Cells("Id").Value = row("oa_utilisateur_id")
                    If .Cells("Id").Value = exId Then index = numRowGrid   ' position exact
                    If numRowGrid <= exPosit Then exPosit = numRowGrid     ' position approchée 
                    .Cells("Prenom").Value = row("oa_utilisateur_prenom")
                    .Cells("Nom").Value = row("oa_utilisateur_nom")
                    .Cells("Identifiant").Value = row("oa_utilisateur_login")
                    .Cells("date_entree").Value = row("oa_utilisateur_date_entree")
                    .Cells("date_sortie").Value = row("oa_utilisateur_date_sortie")
                    .Cells("profil_designation").Value = Coalesce(row("oa_r_profil_designation"), "")
                    .Cells("siege").Value = Coalesce(row("oa_siege_description"), "")
                    .Cells("unite_sanitaire").Value = Coalesce(row("oa_unite_sanitaire_description"), "")
                    .Cells("site").Value = Coalesce(row("oa_site_description"), "")

                    ' -- on garnit le tag pour affichage tooltip
                    'newRow.Tag = "Créé le " & row("horodate_creation") & " par " & row("user_create") & vbCrLf &
                    ' If (IsDBNull(row("horodate_last_update")), "Non modifié.", "Modifié le " & row("horodate_last_update") & " par " & row("user_update")) & vbCrLf &
                    'If (IsDBNull(row("horodate_validate")), "Non Signé.", "Signé le " & row("horodate_validate") & " par " & row("user_validate")) & vbCrLf &
                    'If (Coalesce(row("is_ald"), False), " ... ALD" & vbCrLf, "") &
                    'If (Coalesce(row("is_reponse"), False) AndAlso Coalesce(row("is_reponse_recue"), False) = False,
                    '                            " ... Résultat requis sous " & row("delai_since_validation") & " j à partir de la date de signature" & vbCrLf, "") &
                    'If (Coalesce(row("is_reponse_recue"), False) AndAlso Coalesce(row("is_reponse"), False),
                    '                           " ... Dernier résultat reçu le  " & row("horodate_last_recu"),
                    'If (Coalesce(row("is_reponse"), False), " ... Résultat NON reçu ..." & vbCrLf, ""))Then
                    ' -- gestion du detail

                End With
                RadGridView1.Rows.Add(newRow)
                numRowGrid += 1

            Next
            ' -- positionnement a la ligne la plus proche de la precedente
            If data.Rows.Count > 0 Then
                'Me.RadGridView1.CurrentRow = RadGridView1.ChildRows(If(index >= 0, index, exPosit))
                Me.RadGridView1.CurrentRow = RadGridView1.Rows(If(index >= 0, index, exPosit))
            End If
            Me.Cursor = Cursors.Default


        Catch err As Exception
            MsgBox(err.Message)
        Finally
            Me.Cursor = Cursors.Default
        End Try


    End Sub

End Class
