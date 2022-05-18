Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class RadFAnnuaireProfessionnelSelect
    Property InputSpecialiteId As Integer
    Property InputCodeProfessionId As Integer
    Property InputCodeSavoirFaireId As String
    Property InputTypeSavoirFaireId As String
    Property SelectedProfessionnelCle As Integer

    Private professionSanteDao As New NosProfessionSanteDao
    Private specialiteOrdinaleDao As New NosSpecialiteOrdinaleDao
    Private competenceExclusiveDao As New NosCompetenceExclusiveDao
    Private annuaireProfessionnelDao As New AnnuaireProfessionnelDao
    Private annuaireReferenceDao As New AnnuaireReferenceDao
    Private rorDao As New RorDao

    Dim SelectedAnnuaireProfessionnel As AnnuaireProfessionnel

    Private CreationRor As Boolean

    Private DataTableAnnuaire As DataTable = New DataTable()

    Private Sub RadFAnnuaireProfessionnelSelect_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Répertoire national - Sélection professionnel de santé", userLog)

        If InputCodeProfessionId <> 0 Then
            Dim professionSante As G15_ProfessionSante
            professionSante = professionSanteDao.GetProfessionSanteById(InputCodeProfessionId)
            LblInputProfessionSavoirFaire.Text = professionSante.Libelle
        End If

        If InputCodeSavoirFaireId <> "" Then
            Select Case InputTypeSavoirFaireId.Trim()
                Case Specialite.EnumTypeSavoirFaire.COMPETENCE_EXCLUSIVE
                    Dim savoirFaire As R40_CompetenceExclusive
                    savoirFaire = competenceExclusiveDao.GetCompetenceExclusiveById(InputCodeSavoirFaireId)
                    LblInputProfessionSavoirFaire.Text += " - " & savoirFaire.Libelle
                Case Specialite.EnumTypeSavoirFaire.SPECIALITE_ORDINALE
                    Dim savoirFaire As R38_SpecialiteOrdinale
                    savoirFaire = specialiteOrdinaleDao.GetSpecialiteOrdinaleById(InputCodeSavoirFaireId)
                    LblInputProfessionSavoirFaire.Text += " - " & savoirFaire.Libelle
            End Select
        End If

        InitSelection()
    End Sub

    Private Sub ChargementAnnuaire()
        Cursor.Current = Cursors.WaitCursor

        DataTableAnnuaire.Rows.Clear()
        RadGridViewAnnuaire.Rows.Clear()
        InitSelection()

        Dim adresse1 As String

        DataTableAnnuaire = annuaireProfessionnelDao.GetProfessionnelSanteByNomAndCommune(InputCodeProfessionId, InputCodeSavoirFaireId, FiltreNomExercice.Text, FiltreVilleExercice.Text, FiltreDépartementExercice.Text)

        Dim i As Integer
        Dim iGrid As Integer = -1 'Indice pour alimenter la Grid qui peut comporter moins d'occurrences que le DataTable
        Dim rowCount As Integer = DataTableAnnuaire.Rows.Count - 1

        For i = 0 To rowCount Step 1
            iGrid += 1
            'Ajout d'une ligne au DataGridView
            RadGridViewAnnuaire.Rows.Add(iGrid)

            'Alimentation du Grid
            RadGridViewAnnuaire.Rows(iGrid).Cells("cle").Value = DataTableAnnuaire.Rows(i)("Cle_entree")
            RadGridViewAnnuaire.Rows(iGrid).Cells("civilite_exercice").Value = Coalesce(DataTableAnnuaire.Rows(i)("code_civilite_exercice"), "")
            RadGridViewAnnuaire.Rows(iGrid).Cells("prenom_exercice").Value = Coalesce(DataTableAnnuaire.Rows(i)("prenom_exercice"), "")
            RadGridViewAnnuaire.Rows(iGrid).Cells("nom_exercice").Value = Coalesce(DataTableAnnuaire.Rows(i)("nom_exercice"), "")
            RadGridViewAnnuaire.Rows(iGrid).Cells("libelle_commune").Value = Coalesce(DataTableAnnuaire.Rows(i)("libelle_commune_coord_structure"), "")
            RadGridViewAnnuaire.Rows(iGrid).Cells("raison_sociale_site").Value = Coalesce(DataTableAnnuaire.Rows(i)("raison_sociale_site"), "")
            adresse1 = Coalesce(DataTableAnnuaire.Rows(i)("complement_point_geographique_coord_structure"), "").trim() & " " &
                Coalesce(DataTableAnnuaire.Rows(i)("numero_voie_coord_structure"), "").trim() & " " &
                Coalesce(DataTableAnnuaire.Rows(i)("indice_repetition_voie_coord_structure"), "").trim() & " " &
                Coalesce(DataTableAnnuaire.Rows(i)("libelle_type_voie_coord_structure"), "").trim() & " " &
                Coalesce(DataTableAnnuaire.Rows(i)("libelle_voie_coord_structure"), "").trim() & " " &
                Coalesce(DataTableAnnuaire.Rows(i)("bureau_cedex_coord_structure"), "").trim()
            RadGridViewAnnuaire.Rows(iGrid).Cells("adresse").Value = adresse1.Trim()
        Next

        'Positionnement du grid sur la première occurrence
        If RadGridViewAnnuaire.Rows.Count > 0 Then
            RadGridViewAnnuaire.CurrentRow = RadGridViewAnnuaire.ChildRows(0)
        End If

        Dim NombreOccurrencesLues As Integer = DataTableAnnuaire.Rows.Count

        If NombreOccurrencesLues > 1 Then
            LblOccurrencesLues.Text = NombreOccurrencesLues & " occurrences correspondant aux critères de recherche"
        Else
            LblOccurrencesLues.Text = NombreOccurrencesLues & " occurrence correspondant aux critères de recherche"
        End If

        Cursor.Current = Cursors.Default
    End Sub

    Private Sub InitSelection()
        LblPrenomNom.Text = ""
        LblProfessionSavoirFaire.Text = ""
        LblRaisonSociale.Text = ""
        LblAdresse1.Text = ""
        LblAdresse2.Text = ""
        LblAlerte.Text = ""
        LblOccurrencesLues.Text = ""
        RadBtnSelection.Hide()
        RadBtnDetail.Hide()
    End Sub


    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnRechercher_Click(sender As Object, e As EventArgs) Handles RadBtnRechercher.Click
        If FiltreNomExercice.Text = "" And FiltreVilleExercice.Text = "" And FiltreDépartementExercice.Text = "" Then
            MessageBox.Show("La saisie du nom ou de la ville d'exercice ou du département d'exercice est obligatoire pour lancer la recherche")
            Exit Sub
        End If

        Dim lancementChargement As Boolean = True

        If FiltreDépartementExercice.Text = "" And FiltreVilleExercice.Text = "" Then
            If FiltreNomExercice.Text <> "" Then
                If FiltreNomExercice.Text.Trim.Length < 3 Then
                    MessageBox.Show("Le filtre de recherche du nom d'exercice doit comporter au moins 3 caractères")
                    lancementChargement = False
                End If
            End If
        End If

        If FiltreDépartementExercice.Text = "" And FiltreNomExercice.Text = "" Then
            If FiltreVilleExercice.Text <> "" Then
                If FiltreVilleExercice.Text.Trim.Length < 3 Then
                    MessageBox.Show("Le filtre de recherche de la ville d'exercice doit comporter au moins 3 caractères")
                    lancementChargement = False
                End If
            End If
        End If

        If lancementChargement = True Then
            ChargementAnnuaire()
        End If
    End Sub

    Private Sub RadBtnInit_Click(sender As Object, e As EventArgs) Handles RadBtnInit.Click
        FiltreNomExercice.Text = ""
        FiltreVilleExercice.Text = ""
        FiltreDépartementExercice.Text = ""
        DataTableAnnuaire.Rows.Clear()
        RadGridViewAnnuaire.Rows.Clear()
        InitSelection()
    End Sub

    Private Sub MasterTemplate_CellFormatting(sender As Object, e As Telerik.WinControls.UI.CellFormattingEventArgs) Handles RadGridViewAnnuaire.CellFormatting
        If TypeOf e.Row Is GridViewDataRowInfo Then
            e.CellElement.ToolTipText = e.CellElement.Text
        End If
    End Sub

    Private Sub MasterTemplate_Click(sender As Object, e As EventArgs) Handles RadGridViewAnnuaire.Click
        AffichePersonnelSante()
    End Sub

    Private Sub MasterTemplate_DoubleClick(sender As Object, e As EventArgs) Handles RadGridViewAnnuaire.DoubleClick
        AffichePersonnelSante()
        CreationDansReferentienDSO()
    End Sub

    Private Sub RadBtnSelection_Click(sender As Object, e As EventArgs) Handles RadBtnSelection.Click
        CreationDansReferentienDSO()
    End Sub

    Private Sub AffichePersonnelSante()
        If RadGridViewAnnuaire.CurrentRow IsNot Nothing Then
            Dim aRow As Integer = Me.RadGridViewAnnuaire.Rows.IndexOf(Me.RadGridViewAnnuaire.CurrentRow)
            If aRow >= 0 Then
                Dim Cle As Integer = RadGridViewAnnuaire.Rows(aRow).Cells("Cle").Value

                SelectedAnnuaireProfessionnel = annuaireProfessionnelDao.GetAnnuaireProfessionnelById(Cle)

                Dim annuaireEtatCivil As AnnuaireEtatCivil
                annuaireEtatCivil = annuaireReferenceDao.ChargementEtatCivil(SelectedAnnuaireProfessionnel)

                LblPrenomNom.Text = annuaireEtatCivil.Nom
                LblProfessionSavoirFaire.Text = annuaireEtatCivil.Profession
                LblRaisonSociale.Text = annuaireEtatCivil.RaisonSociale
                LblAdresse1.Text = annuaireEtatCivil.Adresse1
                LblAdresse2.Text = annuaireEtatCivil.Adresse2

                'Contrôle existence du professionnel de santé dans le référentiel interne
                Dim dt As DataTable
                dt = rorDao.ExistProfessionnelSante(SelectedAnnuaireProfessionnel.IdentifiantNational,
                                                    SelectedAnnuaireProfessionnel.IdentifiantTechniqueStructure,
                                                    SelectedAnnuaireProfessionnel.CodeModeExercice,
                                                    SelectedAnnuaireProfessionnel.CodeProfession,
                                                    SelectedAnnuaireProfessionnel.CodeTypeSavoirFaire,
                                                    SelectedAnnuaireProfessionnel.CodeSavoirFaire)
                If dt.Rows.Count > 0 Then
                    LblAlerte.Text = "Sélection impossible : Professionnel de santé déjà existant dans le DSO"
                    RadBtnSelection.Hide()
                    RadBtnDetail.Hide()
                    CreationRor = False
                Else
                    LblAlerte.Text = ""
                    RadBtnSelection.Show()
                    RadBtnDetail.Show()
                    CreationRor = True
                End If
            End If
        End If
    End Sub

    Private Sub CreationDansReferentienDSO() 'TODO: Pro Sante -> ROR
        If CreationRor = True Then
            'Creation de l'instance dans le réferntiel de référence et récupération de la clé
            Dim CleRefenceAnnuaire As Long
            CleRefenceAnnuaire = annuaireReferenceDao.CreationAnnuaireReference(SelectedAnnuaireProfessionnel, userLog)
            'Creation de l'instance dans le référentiel interne
            Dim ror As New Ror

            ror.SpecialiteId = InputSpecialiteId
            ror.Oasis = False
            If SelectedAnnuaireProfessionnel.LibelleCivilite <> "" Then
                ror.Nom = SelectedAnnuaireProfessionnel.LibelleCiviliteExercice & " "
            Else
                ror.Nom = ""
            End If
            ror.Nom += SelectedAnnuaireProfessionnel.PrenomExercice & " " & SelectedAnnuaireProfessionnel.NomExercice
            ror.Type = Ror.EnumIntervenant.Intervenants
            ror.StructureId = 0
            ror.StructureNom = SelectedAnnuaireProfessionnel.RaisonSocialeSite
            ror.Adresse1 = SelectedAnnuaireProfessionnel.ComplementPointGeographiqueCoordonneeStructure.Trim()
            Dim adresse2 As String = SelectedAnnuaireProfessionnel.NumeroVoieCoordonneeStructure.Trim() & " " &
                    SelectedAnnuaireProfessionnel.IndiceRepetitionVoieCoordonneeStructure.Trim() & " " &
                    SelectedAnnuaireProfessionnel.LibelleTypeVoieCoordonneeStructure.Trim() & " " &
                    SelectedAnnuaireProfessionnel.LibelleVoieCoordonneeStructure
            ror.Adresse2 = adresse2.Trim()

            ror.CodePostal = SelectedAnnuaireProfessionnel.CodePostalCoordonneeStructure
            ror.Ville = SelectedAnnuaireProfessionnel.LibelleCommuneCoordonneeStructure
            ror.Telephone = SelectedAnnuaireProfessionnel.TelepcopieCoordonneeStructure
            ror.Email = SelectedAnnuaireProfessionnel.emailCoordonneeStructure
            If SelectedAnnuaireProfessionnel.Typeidentifiant = AnnuaireProfessionnel.EnumTypeIdentifiant.ADELI Then
                ror.Adeli = SelectedAnnuaireProfessionnel.Identifiant
                ror.Rpps = 0
            Else
                ror.Adeli = 0
                ror.Rpps = SelectedAnnuaireProfessionnel.Identifiant
            End If
            ror.ExtractionAnnuaire = True
            ror.IdentifiantNational = SelectedAnnuaireProfessionnel.IdentifiantNational
            ror.IdentifiantStructure = SelectedAnnuaireProfessionnel.IdentifiantTechniqueStructure
            ror.CodeModeExercice_r23 = SelectedAnnuaireProfessionnel.CodeModeExercice
            ror.CodeProfessionSante_g15 = SelectedAnnuaireProfessionnel.CodeProfession
            ror.CodeTypeSavoirFaire_r04 = SelectedAnnuaireProfessionnel.CodeTypeSavoirFaire
            ror.CodeSavoirFaire = SelectedAnnuaireProfessionnel.CodeSavoirFaire
            ror.CleReferenceAnnuaire = CleRefenceAnnuaire

            'Création dans le référentiel interne
            rorDao.CreationRor(ror, userLog)

            'Retour écran précédent
            SelectedProfessionnelCle = CleRefenceAnnuaire
            Close()
        End If
    End Sub

    Private Sub RadBtnDetail_Click(sender As Object, e As EventArgs) Handles RadBtnDetail.Click
        Try
            Using form As New RadFAnnuaireProfessionneldetail
                form.CleReferenceAnnuaire = SelectedAnnuaireProfessionnel.Cle_entree
                form.Reference = AnnuaireReferenceDao.EnumSourceAnnuaire.ANNUAIRE_NATIONAL
                form.ShowDialog()
            End Using
        Catch ex As Exception
            MessageBox.Show(ex.Message)
        End Try
    End Sub
End Class
