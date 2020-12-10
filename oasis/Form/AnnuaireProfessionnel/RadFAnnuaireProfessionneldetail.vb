Imports Oasis_Common

Public Class RadFAnnuaireProfessionneldetail
    Property CleReferenceAnnuaire As Long
    Property Reference As AnnuaireReferenceDao.EnumSourceAnnuaire

    Dim annuaireReferenceDao As New AnnuaireReferenceDao
    Dim annuaireProfessionnelDao As New AnnuaireProfessionnelDao
    Dim annuaireProfessionnelBalDao As New AnnuaireProfessionnelBalDao

    Private DataTableMail As DataTable = New DataTable()
    Private DataTableStructure As DataTable = New DataTable()

    Private Sub RadFAnnuaireProfessionneldetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If CleReferenceAnnuaire <> 0 Then
            Dim annuaireProfessionnel As AnnuaireProfessionnel
            Try
                Select Case Reference
                    Case AnnuaireReferenceDao.EnumSourceAnnuaire.ANNUAIRE_REFERENCE
                        annuaireProfessionnel = annuaireReferenceDao.GetAnnuaireReferenceById(CleReferenceAnnuaire)
                    Case AnnuaireReferenceDao.EnumSourceAnnuaire.ANNUAIRE_NATIONAL
                        annuaireProfessionnel = annuaireProfessionnelDao.GetAnnuaireProfessionnelById(CleReferenceAnnuaire)
                    Case Else
                        MessageBox.Show("Type de l'annuaire inconnu : " & Reference)
                        Close()
                End Select

                ChargementData(annuaireProfessionnel)
                ChargementMail(annuaireProfessionnel)
                ChargementStructure(annuaireProfessionnel)
            Catch ex As Exception
                MessageBox.Show(ex.Message)
                Close()
            End Try
        Else
            Close()
        End If
    End Sub

    Private Sub ChargementData(annuaireProfessionnel As AnnuaireProfessionnel)
        Dim annuaireEtatCivil As AnnuaireEtatCivil
        annuaireEtatCivil = annuaireReferenceDao.ChargementEtatCivil(annuaireProfessionnel)

        LblPrenomNom.Text = annuaireEtatCivil.Nom
        LblProfessionSavoirFaire.Text = annuaireEtatCivil.Profession
        LblRaisonSociale.Text = annuaireEtatCivil.RaisonSociale
        LblAdresse1.Text = annuaireEtatCivil.Adresse1
        LblAdresse2.Text = annuaireEtatCivil.Adresse2

        LblTelephoneLabel.Text = ""
        LblTelephone.Text = ""
        If annuaireProfessionnel.TelephoneCoordonneeStructure.Trim <> "" Then
            LblTelephoneLabel.Text = "Téléphone :"
            LblTelephone.Text = annuaireProfessionnel.TelephoneCoordonneeStructure.Trim
            If annuaireProfessionnel.Telephone2CoordonneeStructure.Trim <> "" Then
                LblTelephone.Text += " - " & annuaireProfessionnel.Telephone2CoordonneeStructure.Trim
            End If
        Else
            If annuaireProfessionnel.Telephone2CoordonneeStructure.Trim <> "" Then
                LblTelephoneLabel.Text = "Téléphone :"
                LblTelephone.Text = annuaireProfessionnel.Telephone2CoordonneeStructure.Trim
            End If
        End If

        LblTelecopieLabel.Text = ""
        LblTelecopie.Text = ""
        If annuaireProfessionnel.TelepcopieCoordonneeStructure.Trim <> "" Then
            LblTelecopieLabel.Text = "Télécopie :"
            LblTelecopie.Text = annuaireProfessionnel.TelepcopieCoordonneeStructure.Trim
        End If

        LblIdentifiant.Text = annuaireProfessionnel.Identifiant
        Select Case annuaireProfessionnel.Typeidentifiant
            Case 0
                LblRPPS_ADELI.Text = "ADELI :"
            Case 8
                LblRPPS_ADELI.Text = "RPPS :"
            Case Else
                LblRPPS_ADELI.Text = ""
                LblIdentifiant.Text = ""
        End Select

        If annuaireProfessionnel.LibelleModeExercice <> "" Then
            LblModeExercice.Text = annuaireProfessionnel.LibelleModeExercice
        Else
            LblModeExerciceLabel.Text = ""
        End If

        If annuaireProfessionnel.LibelleSecteurActivite <> "" Then
            LblSecteurActivite.Text = annuaireProfessionnel.LibelleSecteurActivite
        Else
            LblSecteurActiviteLabel.Text = ""
            LblSecteurActivite.Text = ""
        End If

        If annuaireProfessionnel.emailCoordonneeStructure <> "" Then
            TextEmailStructure.Text = annuaireProfessionnel.emailCoordonneeStructure
        Else
            TextEmailStructure.Hide()
            LblMailStructureLabel.Text = ""
        End If
    End Sub

    Private Sub ChargementMail(annuaireProfessionnel As AnnuaireProfessionnel)
        Cursor.Current = Cursors.WaitCursor

        DataTableMail.Rows.Clear()
        RadGridViewMail.Rows.Clear()

        DataTableMail = annuaireProfessionnelBalDao.GetBalByTypeBalAndIdentifiant(AnnuaireProfessionnelBalDao.EnumTypeBal.PERSONNELLE, annuaireProfessionnel.IdentifiantNational)

        Dim i As Integer
        Dim iGrid As Integer = -1
        Dim rowCount As Integer = DataTableMail.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadGridViewMail.Rows.Add(iGrid)

            'Alimentation du Grid
            RadGridViewMail.Rows(iGrid).Cells("adresse_bal").Value = DataTableMail.Rows(i)("adresse_bal")
            RadGridViewMail.Rows(iGrid).Cells("raison_sociale_structure").Value = Coalesce(DataTableMail.Rows(i)("raison_sociale_structure"), "")
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewMail.Rows.Count > 0 Then
            RadGridViewMail.CurrentRow = RadGridViewMail.ChildRows(0)
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub ChargementStructure(annuaireProfessionnel As AnnuaireProfessionnel)
        Cursor.Current = Cursors.WaitCursor

        DataTableStructure.Rows.Clear()
        RadGridViewStructure.Rows.Clear()

        DataTableStructure = annuaireProfessionnelDao.GetStruturesByProfessionnel(annuaireProfessionnel.IdentifiantNational)

        Dim i As Integer
        Dim iGrid As Integer = -1
        Dim rowCount As Integer = DataTableStructure.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadGridViewStructure.Rows.Add(iGrid)

            'Alimentation du Grid
            RadGridViewStructure.Rows(iGrid).Cells("identifiant_technique_structure").Value = Coalesce(DataTableStructure.Rows(i)("identifiant_technique_structure"), "")
            RadGridViewStructure.Rows(iGrid).Cells("raison_sociale_site").Value = Coalesce(DataTableStructure.Rows(i)("raison_sociale_site"), "")
            RadGridViewStructure.Rows(iGrid).Cells("adresse").Value = Coalesce(DataTableStructure.Rows(i)("indice_repetition_voie_coord_structure"), "") &
                Coalesce(DataTableStructure.Rows(i)("libelle_type_voie_coord_structure"), "") & " " &
                Coalesce(DataTableStructure.Rows(i)("libelle_voie_coord_structure"), "") & " " &
                Coalesce(DataTableStructure.Rows(i)("bureau_cedex_coord_structure"), "")
            RadGridViewStructure.Rows(iGrid).Cells("cnt").Value = DataTableStructure.Rows(i)("cnt")
            If RadGridViewStructure.Rows(iGrid).Cells("cnt").Value <> "0" Then
                RadGridViewStructure.Rows(iGrid).Cells("identifiant_technique_structure").Style.ForeColor = Color.Red
                RadGridViewStructure.Rows(iGrid).Cells("raison_sociale_site").Style.ForeColor = Color.Red
                RadGridViewStructure.Rows(iGrid).Cells("adresse").Style.ForeColor = Color.Red
                RadGridViewStructure.Rows(iGrid).Cells("dso").Value = "DSO"
            Else
                RadGridViewStructure.Rows(iGrid).Cells("dso").Value = ""
            End If
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewStructure.Rows.Count > 0 Then
            RadGridViewStructure.CurrentRow = RadGridViewStructure.ChildRows(0)
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub
End Class
