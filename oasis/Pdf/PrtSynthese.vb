Imports Telerik.WinForms.Documents.Model
Imports Telerik.WinForms.Documents.Layout
Imports Telerik.WinControls.RichTextEditor.UI
Imports Oasis_Common
Public Class PrtSynthese
    Public Property SelectedPatient As Patient

    Dim EditTools As OasisTextTools

    Dim aldDao As New AldDao

    Public Sub PrintDocument()
        EditTools = New OasisTextTools

        Dim section = EditTools.CreateSection()
        Dim document = EditTools.AddSectionIntoDocument(Nothing, section)

        Try
            PrintEntete(section)
            PrintEtatCivil(section)
            EditTools.insertFragmentToEditor(document)

            'EditTools.insertFragmentToEditor(PrintTitre("--- Antécédent ---"))
            PrintAntecedent()

            'EditTools.insertFragmentToEditor(PrintTitre("--- Traitement ---"))
            PrintTraitement()

            'EditTools.insertFragmentToEditor(PrintTitre("--- Parcours de soin ---"))
            PrintParcours()

            'EditTools.insertFragmentToEditor(PrintTitre("--- Contexte ---"))
            PrintContexte()

            'EditTools.insertFragmentToEditor(PrintTitre("--- PPS ---"))
            PrintPPS()

            EditTools.printPreview()
        Catch ex As Exception
            MsgBox(ex.Message())
        Finally
            EditTools.Dispose()
        End Try
    End Sub

    Private Sub PrintEntete(section As Section)
        With EditTools
            .CreateParagraphIntoSection(section, 12, RadTextAlignment.Center)
            .AddTexte("Synthèse patient - ", 14, FontWeights.Bold)
            Dim Titre As String = "Document généré le " & Date.Now.ToString("dd-MM-yyyy") & " à " & Date.Now.ToString("HH:mm")
            .AddTexte(Titre, 10, FontWeights.Normal)
        End With
    End Sub

    Private Sub PrintEtatCivil(section As Section)
        Dim Ligne1 As String = "Prénom / Nom : " &
            SelectedPatient.PatientPrenom & " " &
            SelectedPatient.PatientNom.ToUpper() &
            "    NIR : " & SelectedPatient.PatientNir.ToString &
            "    " & SelectedPatient.PatientGenre

        Dim ligne2 As String =
            "   Date de naissance : " & SelectedPatient.PatientDateNaissance.ToString("dd.MM.yyyy") & "   -   âge : " & outils.CalculAgeEnAnneeEtMoisString(SelectedPatient.PatientDateNaissance)

        Dim ligne3 As String =
            "   Rattachement au site Oasis de " & Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId) &
            "   -  Dernière mise à jour de la synthèse : " & FormatageDateAffichage(SelectedPatient.PatientSyntheseDateMaj, True)

        Dim ALD As String = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        ALD = ALD.Replace(vbCrLf, " ")

        With EditTools
            .CreateParagraphIntoSection(section, 12, RadTextAlignment.Center)
            .AddTexteLine(Ligne1)
            .AddTexteLine(ligne2)
            .AddTexteLine(ligne3)
            If ALD <> "" Then
                .AddTexteLine(ALD)
            End If
        End With
    End Sub

    Private Function PrintTitre(Titre As String) As RadDocument
        Dim section = EditTools.CreateSection()
        Dim document = EditTools.AddSectionIntoDocument(Nothing, section)

        With EditTools
            .CreateParagraphIntoSection(section, 16, RadTextAlignment.Center)
            .AddTexte(Titre, 16)
        End With

        Return document
    End Function

    Private Sub PrintAntecedent()
        Dim document As New RadDocument()

        Const LargeurCol1 As Integer = 630

        Dim section As New Section()
        Dim table As New Table()
        table.LayoutMode = TableLayoutMode.Fixed
        table.StyleName = RadDocumentDefaultStyles.DefaultTableGridStyleName

        Dim PrintLegendeALDValide As Boolean = False
        Dim PrintLegendeALDDemande As Boolean = False

        Dim PremierPassage As Boolean = True

        Dim antecedentDataTable As DataTable
        Dim antecedentDao As AntecedentDao = New AntecedentDao
        antecedentDataTable = antecedentDao.GetAllAntecedentbyPatient(SelectedPatient.patientId, True, True)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1

        'Comptage += antecedentDataTable.Rows.Count
        'GestionSautDePage(document)
        'document.Add(New Paragraph(vbCrLf & "--- Antécédent").SetFontSize(11))

        Dim indentation As String
        Dim dateDateModification, AldDateFin As Date
        Dim AfficheDateModification As String
        Dim diagnostic As String
        Dim antecedentCache, AldValide, AldValideOK, AldDemandeEnCours As Boolean

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            Select Case antecedentDataTable.Rows(i)("oa_antecedent_niveau")
                Case 1
                    indentation = ""
                Case 2
                    indentation = "-----------> "
                Case 3
                    indentation = "----------------------->> "
                Case Else
                    indentation = ""
            End Select

            'Recherche si le contexte a été modifié
            AfficheDateModification = ""
            If antecedentDataTable.Rows(i)("oa_antecedent_date_modification") IsNot DBNull.Value Then
                dateDateModification = antecedentDataTable.Rows(i)("oa_antecedent_date_modification")
                AfficheDateModification = " (" + FormatageDateAffichage(dateDateModification) + ")"
            Else
                If antecedentDataTable.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                    dateDateModification = antecedentDataTable.Rows(i)("oa_antecedent_date_creation")
                    AfficheDateModification = " (" + FormatageDateAffichage(dateDateModification) + ")"
                End If
            End If

            'Identification si l'antécédent est caché
            antecedentCache = False
            If antecedentDataTable.Rows(i)("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                If antecedentDataTable.Rows(i)("oa_antecedent_statut_affichage") = "C" Then
                    antecedentCache = True
                End If
            End If

            AldValide = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ald_valide"), False)
            AldDateFin = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ald_date_fin"), Nothing)
            AldValideOK = False
            If AldValide = True Then
                If AldDateFin > Date.Now() Then
                    AldValideOK = True
                End If
            End If
            AldDemandeEnCours = Coalesce(antecedentDataTable.Rows(i)("oa_antecedent_ald_demande_en_cours"), False)

            'Alimentation
            diagnostic = ""
            If antecedentDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                If CInt(antecedentDataTable.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                    diagnostic = "Suspicion de : "
                Else
                    If CInt(antecedentDataTable.Rows(i)("oa_antecedent_diagnostic")) = 3 Then
                        diagnostic = "Notion de : "
                    End If
                End If
            End If

            Dim antecedentDescription As String

            '===== Affichage antécédent
            If antecedentDataTable.Rows(i)("oa_antecedent_description") Is DBNull.Value Or antecedentDataTable.Rows(i)("oa_antecedent_description") = "" Then
                antecedentDescription = ""
            Else
                antecedentDescription = antecedentDataTable.Rows(i)("oa_antecedent_description")
                antecedentDescription = Replace(antecedentDescription, vbCrLf, " ")
            End If

            Dim DescriptionDrcAld As String = ""
            If AldValideOK Or AldDemandeEnCours Then
                DescriptionDrcAld = Coalesce(antecedentDataTable.Rows(i)("oa_ald_cim10_description"), "")
            End If

            Dim TextAntecedent As String
            TextAntecedent = indentation & diagnostic & DescriptionDrcAld & " " & antecedentDescription
            '==========

            If PremierPassage = True Then
                Dim rowTitre As New TableRow()

                Dim cellTitreAntecedent As New TableCell()
                cellTitreAntecedent.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
                EditTools.SetCell(cellTitreAntecedent, "Antécédent", 10,, Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold)
                rowTitre.Cells.Add(cellTitreAntecedent)
                table.Rows.Add(rowTitre)
                PremierPassage = False
            End If

            Dim row As New TableRow()

            Dim cellAntecedent As New TableCell()
            cellAntecedent.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
            Dim AntecedentColors As Color = Colors.Black
            If antecedentCache = True Then
                AntecedentColors = Colors.Blue
            Else
                If AldValideOK = True Then
                    AntecedentColors = Colors.Red
                    PrintLegendeALDValide = True
                Else
                    If AldDemandeEnCours = True Then
                        AntecedentColors = Colors.Orange
                        PrintLegendeALDDemande = True
                    End If
                End If
            End If
            EditTools.SetCell(cellAntecedent, TextAntecedent, 10, AntecedentColors)
            row.Cells.Add(cellAntecedent)

            table.Rows.Add(row)
        Next

        If PrintLegendeALDValide = True OrElse PrintLegendeALDDemande = True Then
            Dim rowTitre As New TableRow()
            Dim cellVide As New TableCell()
            cellVide.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
            EditTools.SetCell(cellVide, "-", 8)
            rowTitre.Cells.Add(cellVide)
            table.Rows.Add(rowTitre)

            If PrintLegendeALDValide = True Then
                Dim rowAldValide As New TableRow()
                Dim cellAldValide As New TableCell()
                cellAldValide.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
                EditTools.SetCell(cellAldValide, "Antécédent rouge : ALD valde", 8, Colors.Red)
                rowAldValide.Cells.Add(cellAldValide)
                table.Rows.Add(rowAldValide)
            End If

            If PrintLegendeALDDemande = True Then
                Dim rowAldDemande As New TableRow()
                Dim cellAldDemande As New TableCell()
                cellAldDemande.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
                EditTools.SetCell(cellAldDemande, "Antécédent orange : demande ALD en cours", 8, Colors.Orange)
                rowAldDemande.Cells.Add(cellAldDemande)
                table.Rows.Add(rowAldDemande)
            End If
        End If

        section.Blocks.Add(table)
        section.Blocks.Add(New Paragraph())
        document.Sections.Add(section)

        If PrintLegendeALDValide = True Then
            'Dim TextLegendeALDValide As String = "Antécédent rouge -> ALD Valide"
            'EditTools.CreateParagraphIntoSection(section, 10)
            'EditTools.AddTexte(TextLegendeALDValide)
            'TextLegendeALDValide.SetFontColor(iText.Kernel.Colors.ColorConstants.RED).SetFontSize(8)

        End If

        If PrintLegendeALDDemande = True Then
            'Dim TextLegendeALDDemande As String = "Antécédent orange -> Demande ALD en cours"
            'EditTools.CreateParagraphIntoSection(section, 10)
            'EditTools.AddTexte(TextLegendeALDDemande)
            'TextLegendeALDDemande.SetFontColor(iText.Kernel.Colors.ColorConstants.ORANGE).SetFontSize(8)
        End If

        EditTools.insertFragmentToEditor(document)
    End Sub

    Private Sub PrintTraitement()
        Dim document As New RadDocument()

        Const LargeurCol1 As Integer = 450
        Const LargeurCol2 As Integer = 180

        Dim section As New Section()
        Dim table As New Table()
        table.LayoutMode = TableLayoutMode.Fixed
        table.StyleName = RadDocumentDefaultStyles.DefaultTableGridStyleName

        Dim traitementDataTable As DataTable
        Dim traitementDao As TraitementDao = New TraitementDao
        traitementDataTable = traitementDao.GetTraitementEnCoursbyPatient(Me.SelectedPatient.patientId)

        'Ajout d'une colonne 'oa_traitement_posologie' dans le DataTable de traitement
        traitementDataTable.Columns.Add("oa_traitement_posologie", Type.GetType("System.String"))

        Dim i As Integer
        Dim rowCount As Integer = traitementDataTable.Rows.Count - 1

        'Comptage += traitementDataTable.Rows.Count
        'GestionSautDePage(document)
        'document.Add(New Paragraph(vbCrLf & "--- Traitement").SetFontSize(11))

        Dim Base As String
        Dim Posologie As String
        Dim dateFin, dateDebut, dateModification, dateCreation As Date
        Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
        Dim Rythme As Integer
        Dim FenetreTherapeutiqueEnCours As Boolean
        Dim FenetreTherapeutiqueAVenir As Boolean

        Dim PremierPassage As Boolean = True

        'Dim Allergie As Boolean = False
        Dim FenetreDateDebut, FenetreDateFin As Date

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            'Récupération des médicaments déclarés 'allergique' et exclusion de l'affichage
            If traitementDataTable.Rows(i)("oa_traitement_allergie") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_allergie") = "1" Then
                    Continue For
                End If
            End If

            'Récupération des médicaments déclarés 'contre-indiqué' et exclusion de l'affichage
            If traitementDataTable.Rows(i)("oa_traitement_contre_indication") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_contre_indication") = "1" Then
                    Continue For
                End If
            End If

            'Exclusion de l'affichage des traitements déclarés 'arrêté'
            'Cette condition est traitée en exclusion (et non dans la requête SQL) pour stocker les allergies et les contre-indications arrêtés dans la StringCollection
            If traitementDataTable.Rows(i)("oa_traitement_arret") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_arret") = "A" Then
                    Continue For
                End If
            End If

            'Date de fin
            If traitementDataTable.Rows(i)("oa_traitement_date_fin") IsNot DBNull.Value Then
                dateFin = traitementDataTable.Rows(i)("oa_traitement_date_fin")
            Else
                dateFin = "31/12/2999"
            End If

            'Date début
            If traitementDataTable.Rows(i)("oa_traitement_date_debut") IsNot DBNull.Value Then
                dateDebut = traitementDataTable.Rows(i)("oa_traitement_date_debut")
            Else
                dateDebut = "01/01/1900"
            End If

            'Date création
            If traitementDataTable.Rows(i)("oa_traitement_date_creation") IsNot DBNull.Value Then
                dateCreation = traitementDataTable.Rows(i)("oa_traitement_date_creation")
            Else
                dateCreation = "01/01/1900"
            End If

            'Date modification
            If traitementDataTable.Rows(i)("oa_traitement_date_modification") IsNot DBNull.Value Then
                dateModification = traitementDataTable.Rows(i)("oa_traitement_date_modification")
            Else
                dateModification = dateCreation
            End If

            'Exclusion de l'affichage des traitements dont la date de fin est < à la date du jour
            'Cette condition est traitée en exclusion (et non dans la requête SQL) pour stocker les allergies et les contre-indications dans la StringCollection quel que soit leur date de fin
            Dim dateJouraComparer As New Date(Date.Now.Year, Date.Now.Month, Date.Now.Day, 0, 0, 0)
            Dim dateFinaComparer As New Date(dateFin.Year, dateFin.Month, dateFin.Day, 0, 0, 0)
            If (dateFinaComparer < dateJouraComparer) Then
                Continue For
            End If

            'Vérification de l'existence d'une fenêtre thérapeutique active et à venir
            FenetreTherapeutiqueEnCours = False
            FenetreTherapeutiqueAVenir = False

            If traitementDataTable.Rows(i)("oa_traitement_fenetre_date_debut") IsNot DBNull.Value Then
                FenetreDateDebut = traitementDataTable.Rows(i)("oa_traitement_fenetre_date_debut")
            Else
                FenetreDateDebut = "31/12/2999"
            End If

            If traitementDataTable.Rows(i)("oa_traitement_fenetre_date_fin") IsNot DBNull.Value Then
                FenetreDateFin = traitementDataTable.Rows(i)("oa_traitement_fenetre_date_fin")
            Else
                FenetreDateFin = "01/01/1900"
            End If

            Posologie = ""

            'Existence d'une fenêtre thérapeutique
            Dim FenetreTherapeutiqueExiste As Boolean = False
            Dim dateDebutFenetreaComparer As New Date(FenetreDateDebut.Year, FenetreDateDebut.Month, FenetreDateDebut.Day, 0, 0, 0)
            Dim dateFinFenetreaComparer As New Date(FenetreDateFin.Year, FenetreDateFin.Month, FenetreDateFin.Day, 0, 0, 0)
            If traitementDataTable.Rows(i)("oa_traitement_fenetre") IsNot DBNull.Value Then
                If traitementDataTable.Rows(i)("oa_traitement_fenetre") = "1" Then
                    'Fenêtre thérapeutique en cours, à venir ou obsolète
                    FenetreTherapeutiqueExiste = True
                    If FenetreDateDebut <= dateJouraComparer And FenetreDateFin >= dateJouraComparer Then
                        'Fenêtre thérapeutique en cours
                        FenetreTherapeutiqueEnCours = True
                        Posologie = "Fenêtre Th."
                    Else
                        If FenetreDateDebut > dateJouraComparer Then
                            FenetreTherapeutiqueAVenir = True
                        End If
                    End If
                End If
            End If

            'Formatage de la posologie
            If FenetreTherapeutiqueEnCours = False Then
                Dim PosologieMatinString, PosologieMidiString, PosologieApresMidiString, PosologieSoirString As String
                Dim FractionMatin, FractionMidi, FractionApresMidi, FractionSoir As String
                Dim PosologieBase As String

                FractionMatin = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_matin"), TraitementDao.EnumFraction.Non)
                FractionMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_midi"), TraitementDao.EnumFraction.Non)
                FractionApresMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_apres_midi"), TraitementDao.EnumFraction.Non)
                FractionSoir = Coalesce(traitementDataTable.Rows(i)("oa_traitement_fraction_soir"), TraitementDao.EnumFraction.Non)

                posologieMatin = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_matin"), 0)
                posologieMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_midi"), 0)
                posologieApresMidi = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_apres_midi"), 0)
                posologieSoir = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_soir"), 0)

                PosologieBase = Coalesce(traitementDataTable.Rows(i)("oa_traitement_Posologie_base"), "")

                If FractionMatin <> "" AndAlso FractionMatin <> TraitementDao.EnumFraction.Non Then
                    If posologieMatin <> 0 Then
                        PosologieMatinString = posologieMatin.ToString & "+" & FractionMatin
                    Else
                        PosologieMatinString = FractionMatin
                    End If
                Else
                    If posologieMatin <> 0 Then
                        PosologieMatinString = posologieMatin.ToString
                    Else
                        PosologieMatinString = "0"
                    End If
                End If

                If FractionMidi <> "" AndAlso FractionMidi <> TraitementDao.EnumFraction.Non Then
                    If posologieMidi <> 0 Then
                        PosologieMidiString = posologieMidi.ToString & "+" & FractionMidi
                    Else
                        PosologieMidiString = FractionMidi
                    End If
                Else
                    If posologieMidi <> 0 Then
                        PosologieMidiString = posologieMidi.ToString
                    Else
                        PosologieMidiString = "0"
                    End If
                End If

                PosologieApresMidiString = ""
                If FractionApresMidi <> "" AndAlso FractionApresMidi <> TraitementDao.EnumFraction.Non Then
                    If posologieApresMidi <> 0 Then
                        PosologieApresMidiString = posologieApresMidi.ToString & "+" & FractionApresMidi
                    Else
                        PosologieApresMidiString = FractionApresMidi
                    End If
                Else
                    If posologieApresMidi <> 0 Then
                        PosologieApresMidiString = posologieApresMidi.ToString
                    End If
                End If

                If FractionSoir <> "" AndAlso FractionSoir <> TraitementDao.EnumFraction.Non Then
                    If posologieSoir <> 0 Then
                        PosologieSoirString = posologieSoir.ToString & "+" & FractionSoir
                    Else
                        PosologieSoirString = FractionSoir
                    End If
                Else
                    If posologieSoir <> 0 Then
                        PosologieSoirString = posologieSoir.ToString
                    Else
                        PosologieSoirString = "0"
                    End If
                End If
                If traitementDataTable.Rows(i)("oa_traitement_posologie_base") IsNot DBNull.Value Then
                    Rythme = traitementDataTable.Rows(i)("oa_traitement_posologie_rythme")
                    Select Case PosologieBase
                        Case TraitementDao.EnumBaseCode.JOURNALIER
                            Base = ""
                            If posologieApresMidi <> 0 OrElse FractionApresMidi <> TraitementDao.EnumFraction.Non Then
                                Posologie = Base + PosologieMatinString + ". " + PosologieMidiString + ". " + PosologieApresMidiString + ". " + PosologieSoirString
                            Else
                                Posologie = Base + " " + PosologieMatinString + ". " + PosologieMidiString + ". " + PosologieSoirString
                            End If
                        Case Else
                            Dim RythmeString As String = ""
                            If FractionMatin <> "" AndAlso FractionMatin <> TraitementDao.EnumFraction.Non Then
                                If Rythme <> 0 Then
                                    RythmeString = Rythme.ToString & "+" & FractionMatin
                                Else
                                    RythmeString = FractionMatin
                                End If
                            Else
                                If Rythme <> 0 Then
                                    RythmeString = Rythme.ToString
                                End If
                            End If
                            Select Case traitementDataTable.Rows(i)("oa_traitement_posologie_base")
                                Case TraitementDao.EnumBaseCode.CONDITIONNEL
                                    'Base = "Conditionnel : "
                                    Base = ""
                                Case TraitementDao.EnumBaseCode.HEBDOMADAIRE
                                    Base = "Hebdo : "
                                Case TraitementDao.EnumBaseCode.MENSUEL
                                    Base = "Mensuel : "
                                Case TraitementDao.EnumBaseCode.ANNUEL
                                    Base = "Annuel : "
                                Case Else
                                    Base = "Base inconnue ! "
                            End Select
                            Posologie = Base + RythmeString
                    End Select
                End If
                If PosologieBase = TraitementDao.EnumBaseCode.CONDITIONNEL Then
                    Dim commentairePosologie As String = Coalesce(traitementDataTable.Rows(i)("oa_traitement_posologie_commentaire"), "")
                    Posologie &= " " & commentairePosologie
                End If
            End If

            'Dim commentaire As String = Coalesce(traitementDataTable.Rows(i)("oa_traitement_commentaire"), "")
            'Dim commentairePosologie As String = Coalesce(traitementDataTable.Rows(i)("oa_traitement_posologie_commentaire"), "")


            Dim TextMedicamentDci As String = traitementDataTable.Rows(i)("oa_traitement_medicament_dci")
            'Dim TextMedicamentDci As String = traitementDataTable.Rows(i)("oa_traitement_denomination_longue")

            'Posologie
            Dim TextPosologie As String = Posologie

            'Traitement du format d'affichage de modification du traitement
            Dim DateModificationString As String
            If dateModification = "01/01/1900" Then
                DateModificationString = "Date non définie"
            Else
                DateModificationString = FormatageDateAffichage(dateModification, True)
            End If
            Dim TextDateModification As String = DateModificationString

            If PremierPassage = True Then
                Dim rowTitre As New TableRow()

                Dim cellTitreDci As New TableCell()
                cellTitreDci.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
                EditTools.SetCell(cellTitreDci, "Traitement", 10,, Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold)
                rowTitre.Cells.Add(cellTitreDci)

                Dim cellTitrePosologie As New TableCell()
                cellTitrePosologie.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol2)
                EditTools.SetCell(cellTitrePosologie, "Posologie", 10,, Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold)
                rowTitre.Cells.Add(cellTitrePosologie)

                table.Rows.Add(rowTitre)
                PremierPassage = False
            End If

            Dim row As New TableRow()

            Dim cellDci As New TableCell()
            cellDci.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
            EditTools.SetCell(cellDci, TextMedicamentDci, 10)
            row.Cells.Add(cellDci)

            Dim cellPosologie As New TableCell()
            cellPosologie.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol2)
            EditTools.SetCell(cellPosologie, TextPosologie, 10)
            row.Cells.Add(cellPosologie)

            table.Rows.Add(row)
        Next

        section.Blocks.Add(table)
        section.Blocks.Add(New Paragraph())
        document.Sections.Add(section)

        EditTools.insertFragmentToEditor(document)
    End Sub

    Private Sub PrintParcours()
        Dim document As New RadDocument()

        Const LargeurCol1 As Integer = 110
        Const LargeurCol2 As Integer = 120
        Const LargeurCol3 As Integer = 150
        Const LargeurCol4 As Integer = 70
        Const LargeurCol5 As Integer = 70
        Const LargeurCol6 As Integer = 110

        Dim section As New Section()
        Dim table As New Table()
        table.LayoutMode = TableLayoutMode.Fixed
        table.StyleName = RadDocumentDefaultStyles.DefaultTableGridStyleName

        Dim ParcoursDataTable As DataTable
        Dim parcoursDao As New ParcoursDao
        Dim tacheDao As New TacheDao
        Dim SousCategorie, SpecialiteId As Integer
        Dim IntervenantOasis As Boolean

        ParcoursDataTable = parcoursDao.getAllParcoursbyPatient(SelectedPatient.patientId)

        Dim rowCount As Integer = ParcoursDataTable.Rows.Count - 1

        'Comptage += ParcoursDataTable.Rows.Count
        'GestionSautDePage(document)
        'document.Add(New Paragraph(vbCrLf & "--- Parcours de soin").SetFontSize(11))

        Dim ParcoursCacher, ParcoursConsigneEnRouge As Boolean
        Dim PremierPassage As Boolean = True

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            Dim rorId As Integer = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_ror_id"), 0)
            ParcoursCacher = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_cacher"), False)
            If ParcoursCacher = True Then
                Continue For
            End If

            SpecialiteId = ParcoursDataTable.Rows(i)("oa_parcours_specialite")
            Dim TextSpecialite As String = Environnement.Table_specialite.GetSpecialiteDescription(SpecialiteId)

            'Nom intervenant et Structure
            IntervenantOasis = False
            ParcoursConsigneEnRouge = False
            SousCategorie = ParcoursDataTable.Rows(i)("oa_parcours_sous_categorie_id")
            Select Case SousCategorie
                Case EnumSousCategoriePPS.medecinReferent
                    IntervenantOasis = True
                Case EnumSousCategoriePPS.IDE
                    IntervenantOasis = True
                    Dim pacoursConsigneDao As New ParcoursConsigneDao
                    If pacoursConsigneDao.ExisteParcoursConsigne(ParcoursDataTable.Rows(i)("oa_parcours_id")) = False Then
                        ParcoursConsigneEnRouge = True
                    End If
                Case EnumSousCategoriePPS.sageFemme
                    If ParcoursDataTable.Rows(i)("oa_parcours_intervenant_oasis") = True Then
                        IntervenantOasis = True
                    End If
                Case EnumSousCategoriePPS.specialiste
            End Select

            Dim TextNomIntervenant As String
            Dim TextNomStructure As String
            If IntervenantOasis = True Then
                TextNomIntervenant = "Oasis"
                TextNomStructure = "Oasis"
            Else
                TextNomIntervenant = Coalesce(ParcoursDataTable.Rows(i)("oa_ror_nom"), "")
                TextNomStructure = Coalesce(ParcoursDataTable.Rows(i)("oa_ror_structure_nom"), "")
            End If

            'Recherche de la dernière consultation
            Dim dateLast, dateNext As Date
            Dim TypeDemandeRdv As String

            Dim TextConsultationLast As String
            TextConsultationLast = "-"
            dateLast = Coalesce(ParcoursDataTable.Rows(i)("LastRendezVous"), Nothing)
            If dateLast <> Nothing Then
                TextConsultationLast = outils.FormatageDateAffichage(dateLast, True)
            End If

            Dim TextConsultationNext As String
            TextConsultationNext = "-"
            dateNext = Coalesce(ParcoursDataTable.Rows(i)("NextRendezVous"), Nothing)
            If dateNext <> Nothing Then
                'Rendez-vous planifiée
                TextConsultationNext = dateNext.ToString("dd.MM.yyyy")
            Else
                'Recherche si existe demande de rendez-vous
                dateNext = Coalesce(ParcoursDataTable.Rows(i)("DateDemandeRdv"), Nothing)
                If dateNext <> Nothing Then
                    'Rendez-vous prévisionnel, demande en cours
                    TypeDemandeRdv = Coalesce(ParcoursDataTable.Rows(i)("TypeDemandeRdv"), "")
                    Select Case TypeDemandeRdv
                        Case TacheDao.TypeDemandeRendezVous.ANNEE.ToString
                            TextConsultationNext = dateNext.ToString("yyyy")
                        Case TacheDao.TypeDemandeRendezVous.ANNEEMOIS.ToString
                            TextConsultationNext = dateNext.ToString("MM.yyyy")
                        Case Else
                            TextConsultationNext = dateNext.ToString(outils.FormatageDateAffichage(dateNext, True))
                    End Select
                Else
                    Dim Rythme As Integer = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_rythme"), 0)
                    Dim Base As String = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_base"), 0)
                    If Rythme <> 0 And Base <> "" Then
                        If dateLast <> Nothing Then
                            'Rendez-vous prévisionnel, calculé selon le rythme saisi et le dernier rendez-vous passé
                            dateNext = CalculProchainRendezVous(dateLast, Rythme, Base)
                            TextConsultationNext = dateNext.ToString(outils.FormatageDateAffichage(dateNext, False))
                        Else
                            Dim DateCreation As Date = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_date_creation"), Nothing)
                            If DateCreation <> Nothing Then
                                'Rendez-vous prévisionnel, calculé selon le rythme saisi et la date de création de l'intervenant dans le parcours de soin du patient
                                dateNext = CalculProchainRendezVous(DateCreation, Rythme, Base)
                                TextConsultationNext = dateNext.ToString(outils.FormatageDateAffichage(dateNext, False))
                            Else
                                'Rendez-vous à venir non calculable
                            End If
                        End If
                    Else
                        'Pas de rendez-vous à venir pour cet intervenant
                    End If
                End If
            End If

            Dim TextCommentaire As String
            TextCommentaire = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_commentaire"), "")

            If PremierPassage = True Then
                Dim rowTitre As New TableRow()

                Dim cellTitreIntervenant As New TableCell()
                cellTitreIntervenant.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
                EditTools.SetCell(cellTitreIntervenant, "Parcours", 10,, Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold)
                rowTitre.Cells.Add(cellTitreIntervenant)

                Dim cellTitreNom As New TableCell()
                cellTitreNom.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol2)
                EditTools.SetCell(cellTitreNom, "Nom", 10,, Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold)
                rowTitre.Cells.Add(cellTitreNom)

                Dim cellTitreStructure As New TableCell()
                cellTitreStructure.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol3)
                EditTools.SetCell(cellTitreStructure, "Structure", 10,, Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold)
                rowTitre.Cells.Add(cellTitreStructure)

                Dim cellTitreLastRdv As New TableCell()
                cellTitreLastRdv.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol4)
                EditTools.SetCell(cellTitreLastRdv, "Dern. Consult.", 10,, Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold)
                rowTitre.Cells.Add(cellTitreLastRdv)

                Dim cellTitreNextRdv As New TableCell()
                cellTitreNextRdv.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol5)
                EditTools.SetCell(cellTitreNextRdv, "Proch. Consult.", 10,, Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold)
                rowTitre.Cells.Add(cellTitreNextRdv)

                Dim cellTitreRemarque As New TableCell()
                cellTitreRemarque.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol6)
                EditTools.SetCell(cellTitreRemarque, "Remarque", 10,, Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold)
                rowTitre.Cells.Add(cellTitreRemarque)

                table.Rows.Add(rowTitre)
                PremierPassage = False
            End If

            Dim row As New TableRow()

            Dim cellIntervenant As New TableCell()
            cellIntervenant.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
            EditTools.SetCell(cellIntervenant, TextSpecialite, 10)
            row.Cells.Add(cellIntervenant)

            Dim cellNom As New TableCell()
            cellNom.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol2)
            EditTools.SetCell(cellNom, TextNomIntervenant, 10)
            row.Cells.Add(cellNom)

            Dim cellStructure As New TableCell()
            cellStructure.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol3)
            EditTools.SetCell(cellStructure, TextNomStructure, 10)
            row.Cells.Add(cellStructure)

            Dim cellLastRdv As New TableCell()
            cellLastRdv.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol4)
            EditTools.SetCell(cellLastRdv, TextConsultationLast, 10)
            row.Cells.Add(cellLastRdv)

            Dim cellNextRdv As New TableCell()
            cellNextRdv.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol5)
            EditTools.SetCell(cellNextRdv, TextConsultationNext, 10)
            row.Cells.Add(cellNextRdv)

            Dim cellRemarque As New TableCell()
            cellRemarque.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol6)
            EditTools.SetCell(cellRemarque, TextCommentaire, 10)
            row.Cells.Add(cellRemarque)

            table.Rows.Add(row)
        Next

        section.Blocks.Add(table)
        section.Blocks.Add(New Paragraph())
        document.Sections.Add(section)

        EditTools.insertFragmentToEditor(document)
    End Sub

    Private Sub PrintContexte()
        Dim document As New RadDocument()

        Const LargeurCol1 As Integer = 630

        Dim section As New Section()
        Dim table As New Table()
        table.LayoutMode = TableLayoutMode.Fixed
        table.StyleName = RadDocumentDefaultStyles.DefaultTableGridStyleName

        Dim contexteDataTable As DataTable
        Dim antecedentDao As AntecedentDao = New AntecedentDao

        Dim PremierPassage As Boolean = True

        contexteDataTable = antecedentDao.GetContextebyPatient(SelectedPatient.patientId, True)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer

        'Comptage += contexteDataTable.Rows.Count
        'GestionSautDePage(document)
        'document.Add(New Paragraph(vbCrLf & "--- Contexte").SetFontSize(11))

        Dim dateFin, dateModification As Date
        Dim AfficheDateModification, diagnostic As String
        Dim ordreAffichage As Integer
        Dim rowCount As Integer = contexteDataTable.Rows.Count - 1
        Dim categorieContexte, categorieContexteString As String
        Dim contexteCache As Boolean

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            categorieContexte = ""
            If contexteDataTable.Rows(i)("oa_antecedent_categorie_contexte") IsNot DBNull.Value Then
                categorieContexte = contexteDataTable.Rows(i)("oa_antecedent_categorie_contexte")
            End If
            Select Case categorieContexte
                Case ContexteDao.EnumParcoursBaseCode.Medical
                    categorieContexteString = ContexteDao.EnumParcoursBaseItem.Medical
                Case ContexteDao.EnumParcoursBaseCode.BioEnvironnemental
                    categorieContexteString = ContexteDao.EnumParcoursBaseItem.BioEnvironnemental
                Case Else
                    categorieContexteString = ""
            End Select

            'DateFin
            If contexteDataTable.Rows(i)("oa_antecedent_date_fin") IsNot DBNull.Value Then
                dateFin = contexteDataTable.Rows(i)("oa_antecedent_date_fin")
            Else
                dateFin = "31/12/9999"
            End If

            'Recherche si le contexte a été modifié (médical uniquement)
            AfficheDateModification = ""
            If categorieContexte = "M" Then
                If contexteDataTable.Rows(i)("oa_antecedent_date_modification") IsNot DBNull.Value Then
                    dateModification = contexteDataTable.Rows(i)("oa_antecedent_date_modification")
                    AfficheDateModification = FormatageDateAffichage(dateModification) + " : "
                Else
                    If contexteDataTable.Rows(i)("oa_antecedent_date_creation") IsNot DBNull.Value Then
                        dateModification = contexteDataTable.Rows(i)("oa_antecedent_date_creation")
                        AfficheDateModification = FormatageDateAffichage(dateModification) + " : "
                    End If
                End If
            End If

            'Affichage de l'ordre d'affichage
            If contexteDataTable.Rows(i)("oa_antecedent_ordre_affichage1") IsNot DBNull.Value Then
                ordreAffichage = contexteDataTable.Rows(i)("oa_antecedent_ordre_affichage1")
            Else
                ordreAffichage = 0
            End If

            'Contexte caché
            contexteCache = False
            If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") IsNot DBNull.Value Then
                If contexteDataTable.Rows(i)("oa_antecedent_statut_affichage") = "C" Then
                    contexteCache = True
                End If
            End If

            diagnostic = ""
            If contexteDataTable.Rows(i)("oa_antecedent_diagnostic") IsNot DBNull.Value Then
                If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 2 Then
                    diagnostic = "Suspicion de "
                Else
                    If CInt(contexteDataTable.Rows(i)("oa_antecedent_diagnostic")) = 3 Then
                        diagnostic = "Notion de "
                    End If
                End If
            End If

            'Affichage contexte ==========================
            Dim contexteDescription As String
            contexteDescription = Coalesce(contexteDataTable.Rows(i)("oa_antecedent_description"), "")

            If PremierPassage = True Then
                Dim rowTitre As New TableRow()

                Dim cellTitreContexte As New TableCell()
                cellTitreContexte.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
                EditTools.SetCell(cellTitreContexte, "Contexte", 10,, Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold)
                rowTitre.Cells.Add(cellTitreContexte)

                table.Rows.Add(rowTitre)
                PremierPassage = False
            End If

            Dim row As New TableRow()
            Dim cellContexte As New TableCell()
            cellContexte.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
            EditTools.SetCell(cellContexte, contexteDescription, 10)
            row.Cells.Add(cellContexte)
            table.Rows.Add(row)
        Next

        section.Blocks.Add(table)
        section.Blocks.Add(New Paragraph())
        document.Sections.Add(section)

        EditTools.insertFragmentToEditor(document)
    End Sub

    Private Sub PrintPPS()
        Dim document As New RadDocument()

        Const LargeurCol1 As Integer = 630

        Dim section As New Section()
        Dim table As New Table()
        table.LayoutMode = TableLayoutMode.Fixed
        table.StyleName = RadDocumentDefaultStyles.DefaultTableGridStyleName

        Dim PremierPassage As Boolean = True

        Dim PPSDataTable As DataTable
        Dim PPSDao As PpsDao = New PpsDao
        PPSDataTable = PPSDao.getAllPPSbyPatient(SelectedPatient.patientId)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer

        'Comptage += PPSDataTable.Rows.Count
        'GestionSautDePage(document)
        'document.Add(New Paragraph(vbCrLf & "--- Plan personnalisé de soin").SetFontSize(11))

        Dim dateDebut, dateModification As Date
        Dim rowCount As Integer = PPSDataTable.Rows.Count - 1
        Dim categoriePPS, sousCategoriePPS, Rythme, SpecialiteId As Integer
        Dim ppsArret As Boolean
        Dim NaturePPS, CommentairePPS, commentaireParcours, AffichePPS, AfficheDateModificationPPS, AfficheDateModificationParcours, Base, BaseItem, SpecialiteDescription As String

        'Parcours du DataTable pour alimenter le DataGridView
        For i = 0 To rowCount Step 1
            categoriePPS = 0
            If PPSDataTable.Rows(i)("oa_r_pps_categorie_id") IsNot DBNull.Value Then
                categoriePPS = PPSDataTable.Rows(i)("oa_r_pps_categorie_id")
            End If

            sousCategoriePPS = 0
            If PPSDataTable.Rows(i)("oa_r_pps_sous_categorie_id") IsNot DBNull.Value Then
                sousCategoriePPS = PPSDataTable.Rows(i)("oa_r_pps_sous_categorie_id")
            End If

            'Date de début
            If PPSDataTable.Rows(i)("oa_pps_date_debut") IsNot DBNull.Value Then
                dateDebut = PPSDataTable.Rows(i)("oa_pps_date_debut")
            Else
                dateDebut = "01/01/1900"
            End If

            'Rythme
            Rythme = Coalesce(PPSDataTable.Rows(i)("oa_parcours_rythme"), 0)
            Base = Coalesce(PPSDataTable.Rows(i)("oa_parcours_base"), "")
            Select Case Base
                Case ParcoursDao.EnumParcoursBaseCode.Quotidien
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.Quotidien
                Case ParcoursDao.EnumParcoursBaseCode.Hebdomadaire
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.Hebdomadaire
                Case ParcoursDao.EnumParcoursBaseCode.ParMois
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.ParMois
                Case ParcoursDao.EnumParcoursBaseCode.ParAn
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.ParAn
                Case ParcoursDao.EnumParcoursBaseCode.TousLes2Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes2Ans
                Case ParcoursDao.EnumParcoursBaseCode.TousLes3Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes3Ans
                Case ParcoursDao.EnumParcoursBaseCode.TousLes4Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes4Ans
                Case ParcoursDao.EnumParcoursBaseCode.TousLes5Ans
                    BaseItem = ParcoursDao.EnumParcoursBaseItem.TousLes5Ans
                Case Else
                    BaseItem = ""
            End Select

            CommentairePPS = Coalesce(PPSDataTable.Rows(i)("oa_pps_commentaire"), "")
            commentaireParcours = Coalesce(PPSDataTable.Rows(i)("oa_parcours_commentaire"), "")

            'Recherche si le pps a été modifié
            AfficheDateModificationPPS = ""
            If PPSDataTable.Rows(i)("oa_pps_date_modification") IsNot DBNull.Value Then
                dateModification = PPSDataTable.Rows(i)("oa_pps_date_modification")
                AfficheDateModificationPPS = FormatageDateAffichage(dateModification) + " : "
            Else
                If PPSDataTable.Rows(i)("oa_pps_date_creation") IsNot DBNull.Value Then
                    dateModification = PPSDataTable.Rows(i)("oa_pps_date_creation")
                    AfficheDateModificationPPS = FormatageDateAffichage(dateModification) + " : "
                End If
            End If

            'Recherche si le parcours a été modifié
            AfficheDateModificationParcours = ""
            If PPSDataTable.Rows(i)("oa_parcours_date_modification") IsNot DBNull.Value Then
                dateModification = PPSDataTable.Rows(i)("oa_parcours_date_modification")
                AfficheDateModificationParcours = FormatageDateAffichage(dateModification) + " : "
            Else
                If PPSDataTable.Rows(i)("oa_parcours_date_creation") IsNot DBNull.Value Then
                    dateModification = PPSDataTable.Rows(i)("oa_parcours_date_creation")
                    AfficheDateModificationParcours = FormatageDateAffichage(dateModification) + " : "
                End If
            End If

            'PPS caché
            ppsArret = False
            If PPSDataTable.Rows(i)("oa_pps_arret") IsNot DBNull.Value Then
                If PPSDataTable.Rows(i)("oa_pps_arret") = "1" Then
                    ppsArret = True
                End If
            End If

            NaturePPS = ""
            AffichePPS = ""
            'Présentation PPS : Cible/Objectif de santé (commentaire)
            If categoriePPS = Environnement.EnumCategoriePPS.Objectif Then
                NaturePPS = "Objectif santé : "
                AffichePPS = NaturePPS + " " + CommentairePPS
            End If

            If categoriePPS = Environnement.EnumCategoriePPS.MesurePreventive Then
                'Suivi mesures préventives (Code DRC, libellé DRC, commentaire)
                NaturePPS = "Mesures préventives : "
                AffichePPS = NaturePPS & " " & CommentairePPS
            End If

            SpecialiteDescription = ""
            'Présentation PPS : Suivi
            If categoriePPS = Environnement.EnumCategoriePPS.Suivi Then
                'Un parcours caché ne doit être affiché
                Dim parcoursCache As Boolean = Coalesce(PPSDataTable.Rows(i)("oa_parcours_cacher"), False)
                If parcoursCache = True Then
                    'Continue For
                End If
                'Un suivi intervenant sans rythme ne doit pas être affiché dans le PPS
                If Rythme = 0 Then
                    Continue For
                End If

                'Suivi IDE, Médecin référent, Sage-femme et Spécialiste (Base, Rythme, Commentaire)
                Select Case sousCategoriePPS
                    Case Environnement.EnumSousCategoriePPS.IDE
                        NaturePPS = "Suivi IDE : "
                    Case Environnement.EnumSousCategoriePPS.medecinReferent
                        NaturePPS = "Suivi médecin télémédecine : "
                    Case Environnement.EnumSousCategoriePPS.sageFemme
                        NaturePPS = "Suivi sage-femme : "
                    Case Environnement.EnumSousCategoriePPS.specialiste
                        'Récupération spécialité
                        If PPSDataTable.Rows(i)("oa_parcours_specialite") IsNot DBNull.Value Then
                            SpecialiteId = PPSDataTable.Rows(i)("oa_parcours_specialite")
                            SpecialiteDescription = Table_specialite.GetSpecialiteDescription(SpecialiteId)
                        End If
                        NaturePPS = "Suivi " + SpecialiteDescription + " : "
                    Case Else
                        NaturePPS = "Inconnue "
                End Select
                If Base = ParcoursDao.EnumParcoursBaseCode.Hebdomadaire _
                    Or Base = ParcoursDao.EnumParcoursBaseCode.ParMois _
                    Or Base = ParcoursDao.EnumParcoursBaseCode.ParAn Then
                    AffichePPS = NaturePPS + Rythme.ToString + " / " + BaseItem + " " + CommentairePPS
                Else
                    AffichePPS = NaturePPS + BaseItem + " " + CommentairePPS
                End If
            End If

            'Présentation PPS : Stratégie contextuelle (Base, Rythme, Commentaire)
            If categoriePPS = Environnement.EnumCategoriePPS.Strategie Then
                Select Case sousCategoriePPS
                    Case 7
                        NaturePPS = "Démarche prophylactique "
                    Case 8
                        NaturePPS = "Démarche sociale "
                    Case 9
                        NaturePPS = "Démarche symptomatique "
                    Case 10
                        NaturePPS = "Démarche curative "
                    Case 11
                        NaturePPS = "Démarche diagnostique "
                    Case 12
                        NaturePPS = "Démarche palliative "
                    Case Else
                        NaturePPS = "Inconnue "
                End Select
                AffichePPS = AfficheDateModificationPPS + NaturePPS + " " + CommentairePPS
            End If

            'Transformation des "Tab" et "Return" en espace pour afficher les éléments correctement
            AffichePPS = Replace(AffichePPS, vbTab, " ")
            AffichePPS = Replace(AffichePPS, vbCrLf, " ")

            If PremierPassage = True Then
                Dim rowTitre As New TableRow()

                Dim cellTitrePPS As New TableCell()
                cellTitrePPS.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
                EditTools.SetCell(cellTitrePPS, "PPS", 10,, Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold)
                rowTitre.Cells.Add(cellTitrePPS)

                table.Rows.Add(rowTitre)
                PremierPassage = False
            End If

            Dim row As New TableRow()
            Dim cellPPS As New TableCell()
            cellPPS.PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
            If ppsArret = True Then
                EditTools.SetCell(cellPPS, AffichePPS, 10, Colors.Red)
            Else
                EditTools.SetCell(cellPPS, AffichePPS, 10)
            End If
            row.Cells.Add(cellPPS)
            table.Rows.Add(row)
        Next

        section.Blocks.Add(table)
        section.Blocks.Add(New Paragraph())
        document.Sections.Add(section)

        EditTools.insertFragmentToEditor(document)
    End Sub

End Class
