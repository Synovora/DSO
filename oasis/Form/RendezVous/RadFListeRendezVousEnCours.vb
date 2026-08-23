Imports Oasis_Common
Public Class RadFListeRendezVousEnCours
    Dim tacheDao As New TacheDao

    Private Sub RadFListeRendezVousEnCours_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementListeRendezvous()
    End Sub

    Private Sub ChargementListeRendezvous()
        Dim dt As DataTable
        Dim dateRendezVous, dateCreation As Date
        Dim typeDemandeRendezVous As String

        RadGridView1.Rows.Clear()

        dt = TacheDao.GetAllRendezVousEnCours()
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = dt.Rows.Count - 1

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            iGrid += 1
            RadGridView1.Rows.Add(iGrid)
            RadGridView1.Rows(iGrid).Cells("id").Value = dt.Rows(i)("id")
            RadGridView1.Rows(iGrid).Cells("user_traiteur_prenom").Value = Coalesce(dt.Rows(i)("user_traiteur_prenom"), "")
            RadGridView1.Rows(iGrid).Cells("user_traiteur_nom").Value = Coalesce(dt.Rows(i)("user_traiteur_nom"), "")
            RadGridView1.Rows(iGrid).Cells("traite_fonction").Value = Coalesce(dt.Rows(i)("traite_fonction"), "")

            Dim nature As String = Coalesce(dt.Rows(i)("nature"), "")
            RadGridView1.Rows(iGrid).Cells("type").Value = tacheDao.GetItemNatureTacheByCode(dt.Rows(i)("nature"))

            dateRendezVous = Coalesce((dt.Rows(i)("date_rendez_vous")), Nothing)
            typeDemandeRendezVous = Coalesce(dt.Rows(i)("type_demande_rendez_vous"), "")

            If dateRendezVous <> Nothing Then
                If nature = Tache.NatureTache.RDV_DEMANDE.ToString Then
                    If typeDemandeRendezVous = Tache.EnumDemandeRendezVous.ANNEE.ToString Then
                        RadGridView1.Rows(iGrid).Cells("dateRendezVous").Value = dateRendezVous.Year
                    Else
                        RadGridView1.Rows(iGrid).Cells("dateRendezVous").Value = dateRendezVous.Month & "." & dateRendezVous.Year
                    End If
                Else
                    RadGridView1.Rows(iGrid).Cells("dateRendezVous").Value = dateRendezVous.ToString("dd.MM.yyyy")
                End If
            End If

            dateCreation = Coalesce((dt.Rows(i)("horodate_creation")), Nothing)
            RadGridView1.Rows(iGrid).Cells("dateCreation").Value = dateCreation.ToString("dd.MM.yyyy")
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridView1.Rows.Count > 0 Then
            Me.RadGridView1.CurrentRow = RadGridView1.Rows(0)
        End If
    End Sub

    Private Sub RadButton1_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnDesattribuer_Click(sender As Object, e As EventArgs) Handles RadBtnDesattribuer.Click
        Dim aRow, maxRow As Integer
        aRow = Me.RadGridView1.Rows.IndexOf(Me.RadGridView1.CurrentRow)
        maxRow = RadGridView1.Rows.Count - 1
        If aRow <= maxRow And aRow > -1 Then
            Dim TacheId As Long = RadGridView1.Rows(aRow).Cells("id").Value
            If tacheDao.DesattribueTache(TacheId) = True Then
                ChargementListeRendezvous()
            End If
        End If
    End Sub
End Class
