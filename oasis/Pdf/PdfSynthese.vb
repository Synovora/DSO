Imports System.IO
Imports iText.IO.Font.Constants
Imports iText.Kernel.Font
Imports iText.Kernel.Geom
Imports iText.Kernel.Pdf
Imports iText.Layout
Imports iText.Layout.Element
Imports iText.Layout.Properties
Imports Oasis_Common

Public Class PdfSynthese
    Private _selectedPatient As Patient

    Public Property SelectedPatient As Patient
        Get
            Return _selectedPatient
        End Get
        Set(value As Patient)
            _selectedPatient = value
        End Set
    End Property

    Dim aldDao As New AldDao

    Dim DEST As String
    Dim NomPdf As String
    Dim Comptage, PageNumero As Integer
    Dim DateGeneration As Date = Date.Now()

    Friend Sub ImprimeSynthese()
        NomPdf = "Synthese_" & SelectedPatient.patientId & "_" & DateGeneration.ToString("yyyy-MM-dd_HH-mm-ss") & ".pdf"
        DEST = "c:\Temp\Oasis\" & NomPdf
        Dim file As FileInfo = New FileInfo(DEST)
        If Not file.Directory.Exists Then file.Directory.Create()
        CreatePdf()
    End Sub

    Private Sub CreatePdf()
        Try
            Dim writer As PdfWriter = New PdfWriter(DEST)
            GenerationPdf(writer)
            Process.Start(DEST)
        Catch ex As Exception
            MessageBox.Show(ex.ToString)
            Dim form As New RadFNotification()
            form.Titre = "Notification - Imprimer synthèse"
            form.Message = "Le pDF à générer est en consultation et ne peut être rempacé"
            form.Show()
            Exit Sub
        End Try
    End Sub

    Private Sub GenerationPdf(writer As PdfWriter)
        Dim pdf As PdfDocument = New PdfDocument(writer)
        Dim document As Document = New Document(pdf, PageSize.A4)
        Dim font As PdfFont = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN)

        PrintEntete(document)
        PrintEtatCivil(document)
        PrintAntecedent(document)
        PrintTraitement(document)
        PrintParcours(document)
        PrintContexte(document)
        PrintPPS(document)

        document.Close()
    End Sub

    Private Sub PrintEntete(document As Document)
        Dim TextPage As New Text("")
        PageNumero += 1
        TextPage.SetText("         -          Document généré le " & DateGeneration.ToString("dd.MM.yyyy") &
                         " à " & DateGeneration.ToString("HH:mm:ss") & "          -          Page " & PageNumero)
        TextPage.SetFontSize(10)

        document.Add(New Paragraph("         Synthèse patient").SetFontSize(14).Add(TextPage))

    End Sub

    Private Sub PrintEtatCivil(document As Document)
        document.Add(New Paragraph("--- Etat civil").SetFontSize(11))
        Dim ligne1 As String
        Dim TextEtatCivil As New Text("")
        Dim p As New Paragraph
        Dim table As New Table(1)

        Dim ALD As String = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        ALD = ALD.Replace(vbCrLf, " ")

        ligne1 = "Prénom / Nom : " &
            SelectedPatient.PatientPrenom & " " &
            SelectedPatient.PatientNom.ToUpper() &
            "       NIR : " & SelectedPatient.PatientNir.ToString &
            "          " & SelectedPatient.PatientGenre & vbCrLf &
            "   Date de naissance : " & SelectedPatient.PatientDateNaissance.ToString("dd.MM.yyyy") & "   -   âge : " & outils.CalculAgeString(SelectedPatient.PatientDateNaissance) & vbCrLf &
            "   Rattachement au site Oasis de " & Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId) &
            "   -  Dernière mise à jour de la synthèse : " & FormatageDateAffichage(SelectedPatient.PatientSyntheseDateMaj, True)


        TextEtatCivil.SetText("  " & vbCrLf & ALD)
        TextEtatCivil.SetFontColor(iText.Kernel.Colors.ColorConstants.RED).SetFontSize(8)

        table.AddCell(New Cell().Add(p.Add(ligne1).SetFontSize(8).Add(TextEtatCivil)))
        document.Add(table)
        Comptage = 0
    End Sub

    Private Sub PrintAntecedent(document As Document)
        Dim p As New Paragraph
        Dim table As New Table(1)

        Dim PrintLegendeALDValide As Boolean = False
        Dim PrintLegendeALDDemande As Boolean = False

        Dim antecedentDataTable As DataTable
        Dim antecedentDao As AntecedentDao = New AntecedentDao
        antecedentDataTable = antecedentDao.GetAllAntecedentbyPatient(SelectedPatient.patientId, True, True)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer
        Dim rowCount As Integer = antecedentDataTable.Rows.Count - 1

        Comptage += antecedentDataTable.Rows.Count
        GestionSautDePage(document)
        document.Add(New Paragraph(vbCrLf & "--- Antécédent").SetFontSize(11))

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

            Dim longueurString As Integer
            Dim longueurMax As Integer = 100
            Dim antecedentDescription As String

            '===== Affichage antécédent
            If antecedentDataTable.Rows(i)("oa_antecedent_description") Is DBNull.Value Or antecedentDataTable.Rows(i)("oa_antecedent_description") = "" Then
                antecedentDescription = ""
            Else
                antecedentDescription = antecedentDataTable.Rows(i)("oa_antecedent_description")
                antecedentDescription = Replace(antecedentDescription, vbCrLf, " ")
                longueurString = antecedentDescription.Length
                If longueurString > longueurMax Then
                    longueurString = longueurMax
                End If
                antecedentDescription = antecedentDescription.Substring(0, longueurString)
            End If

            Dim DescriptionDrcAld As String = ""
            If AldValideOK Or AldDemandeEnCours Then
                DescriptionDrcAld = Coalesce(antecedentDataTable.Rows(i)("oa_ald_cim10_description"), "")
            End If

            Dim TextAntecedent As New Text("")
            TextAntecedent.SetText(indentation & diagnostic & DescriptionDrcAld & " " & antecedentDescription & vbCrLf)
            TextAntecedent.SetFontSize(8)
            '==========

            If antecedentCache = True Then
                TextAntecedent.SetFontColor(iText.Kernel.Colors.ColorConstants.BLUE)
            Else
                If AldValideOK = True Then
                    TextAntecedent.SetFontColor(iText.Kernel.Colors.ColorConstants.RED)
                    PrintLegendeALDValide = True
                Else
                    If AldDemandeEnCours = True Then
                        TextAntecedent.SetFontColor(iText.Kernel.Colors.ColorConstants.ORANGE)
                        PrintLegendeALDDemande = True
                    End If
                End If
            End If
            table.AddCell(New Cell().Add(New Paragraph(TextAntecedent).SetFixedLeading(10)))
        Next
        document.Add(table)

        If PrintLegendeALDValide = True Then
            Dim TextLegendeALDValide As New Text("Antécédent rouge -> ALD Valide")
            TextLegendeALDValide.SetFontColor(iText.Kernel.Colors.ColorConstants.RED).SetFontSize(8)
            document.Add(New Paragraph().Add(TextLegendeALDValide).SetFixedLeading(10))
        End If
        If PrintLegendeALDDemande = True Then
            Dim TextLegendeALDDemande As New Text("Antécédent orange -> Demande ALD en cours")
            TextLegendeALDDemande.SetFontColor(iText.Kernel.Colors.ColorConstants.ORANGE).SetFontSize(8)
            document.Add(New Paragraph().Add(TextLegendeALDDemande).SetFixedLeading(10))
        End If
    End Sub

    Private Sub PrintTraitement(document As Document)
        Dim p As New Paragraph
        Dim table As New Table(3)
        Dim traitementDataTable As DataTable
        Dim traitementDao As TraitementDao = New TraitementDao
        traitementDataTable = traitementDao.getTraitementNotCancelledbyPatient(Me.SelectedPatient.patientId)

        'Ajout d'une colonne 'oa_traitement_posologie' dans le DataTable de traitement
        traitementDataTable.Columns.Add("oa_traitement_posologie", Type.GetType("System.String"))

        Dim i As Integer
        Dim rowCount As Integer = traitementDataTable.Rows.Count - 1

        Comptage += traitementDataTable.Rows.Count
        GestionSautDePage(document)
        document.Add(New Paragraph(vbCrLf & "--- Traitement").SetFontSize(11))

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
                                    Base = "Conditionnel : "
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
            End If

            Dim commentaire As String = Coalesce(traitementDataTable.Rows(i)("oa_traitement_commentaire"), "")
            Dim commentairePosologie As String = Coalesce(traitementDataTable.Rows(i)("oa_traitement_posologie_commentaire"), "")

            'Stockage des médicaments prescrits (pour contrôle lors de la selection d'un médicament dans le cadre d'un nouveau traitement
            SelectedPatient.PatientMedicamentsPrescritsCis.Add(traitementDataTable.Rows(i)("oa_traitement_medicament_cis"))

            Dim TextMedicamentDci As New Text(traitementDataTable.Rows(i)("oa_traitement_medicament_dci"))
            TextMedicamentDci.SetFontSize(8)
            'Posologie
            Dim TextPosologie As New Text(Posologie)
            'posologieTraitement.SetText(Posologie)
            TextPosologie.SetFontSize(8)

            If Posologie = "Fenêtre Th." Then
                'RadTraitementDataGridView.Rows(iGrid).Cells("posologie").Style.ForeColor = Color.Red
            End If

            'Traitement du format d'affichage de modification du traitement
            Dim DateModificationString As String = ""
            If dateModification = "01/01/1900" Then
                DateModificationString = "Date non définie"
            Else
                DateModificationString = FormatageDateAffichage(dateModification, True)
            End If
            Dim TextDateModification As New Text(DateModificationString)
            'dateModificationTraitement.SetText(DateModificationString)
            TextDateModification.SetFontSize(8)
            'Bouton gérer fenêtre thérapeutique

            If FenetreTherapeutiqueAVenir = True Or FenetreTherapeutiqueEnCours = True Then
                'RadTraitementDataGridView.Rows(iGrid).Cells("posologie").Style.ForeColor = Color.Red
            End If

            If PremierPassage = True Then
                table.AddCell(New Cell().Add(New Paragraph("Traitement").SetFontSize(8).SetBold))
                table.AddCell(New Cell().Add(New Paragraph("Posologie").SetFontSize(8).SetBold))
                table.AddCell(New Cell().Add(New Paragraph("Date maj").SetFontSize(8).SetBold))
                PremierPassage = False
            End If

            table.AddCell(New Cell().Add(New Paragraph(TextMedicamentDci)))
            table.AddCell(New Cell().Add(New Paragraph(TextPosologie)))
            table.AddCell(New Cell().Add(New Paragraph(TextDateModification)))
        Next
        document.Add(table)
    End Sub

    Private Sub PrintParcours(document As Document)
        Dim p As New Paragraph
        Dim table As New Table(6)

        Dim ParcoursDataTable As DataTable
        Dim parcoursDao As New ParcoursDao
        Dim tacheDao As New TacheDao
        Dim SousCategorie, SpecialiteId As Integer
        Dim IntervenantOasis As Boolean

        ParcoursDataTable = parcoursDao.getAllParcoursbyPatient(SelectedPatient.patientId)

        Dim rowCount As Integer = ParcoursDataTable.Rows.Count - 1

        Comptage += ParcoursDataTable.Rows.Count
        GestionSautDePage(document)
        document.Add(New Paragraph(vbCrLf & "--- Parcours de soin").SetFontSize(11))

        Dim SpecialiteDescription As String
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
            SpecialiteDescription = Environnement.Table_specialite.GetSpecialiteDescription(SpecialiteId)
            'RadParcoursDataGridView.Rows(iGrid).Cells("specialite").Value = SpecialiteDescription
            Dim TextSpecialite As New Text("")
            TextSpecialite.SetText(SpecialiteDescription)
            TextSpecialite.SetFontSize(8)

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

            Dim TextNomIntervenant As New Text("")
            Dim TextNomStructure As New Text("")
            TextNomIntervenant.SetFontSize(8)
            TextNomStructure.SetFontSize(8)
            If IntervenantOasis = True Then
                TextNomIntervenant.SetText("Oasis")
                TextNomStructure.SetText("Oasis")
            Else
                TextNomIntervenant.SetText(Coalesce(ParcoursDataTable.Rows(i)("oa_ror_nom"), ""))
                TextNomStructure.SetText(Coalesce(ParcoursDataTable.Rows(i)("oa_ror_structure_nom"), ""))
            End If

            'Recherche de la dernière consultation
            Dim dateLast, dateNext As Date
            Dim TypeDemandeRdv As String
            'Dim tache As Tache

            Dim TextConsultationLast As New Text("")
            TextConsultationLast.SetFontSize(8).SetHorizontalAlignment(HorizontalAlignment.CENTER)
            TextConsultationLast.SetText("-")
            dateLast = Coalesce(ParcoursDataTable.Rows(i)("LastRendezVous"), Nothing)
            If dateLast <> Nothing Then
                TextConsultationLast.SetText(outils.FormatageDateAffichage(dateLast, True))
            End If

            Dim TextConsultationNext As New Text("")
            TextConsultationNext.SetFontSize(8).SetHorizontalAlignment(HorizontalAlignment.CENTER)
            TextConsultationNext.SetText("-")
            dateNext = Coalesce(ParcoursDataTable.Rows(i)("NextRendezVous"), Nothing)
            If dateNext <> Nothing Then
                'Rendez-vous planifiée
                TextConsultationNext.SetText(dateNext.ToString("dd.MM.yyyy"))
            Else
                'Recherche si existe demande de rendez-vous
                dateNext = Coalesce(ParcoursDataTable.Rows(i)("DateDemandeRdv"), Nothing)
                If dateNext <> Nothing Then
                    'Rendez-vous prévisionnel, demande en cours
                    TypeDemandeRdv = Coalesce(ParcoursDataTable.Rows(i)("TypeDemandeRdv"), "")
                    Select Case TypeDemandeRdv
                        Case TacheDao.typeDemandeRendezVous.ANNEE.ToString
                            TextConsultationNext.SetText(dateNext.ToString("yyyy"))
                        Case TacheDao.typeDemandeRendezVous.ANNEEMOIS.ToString
                            TextConsultationNext.SetText(dateNext.ToString("MM.yyyy"))
                        Case Else
                            TextConsultationNext.SetText(dateNext.ToString(outils.FormatageDateAffichage(dateNext, True)))
                    End Select
                Else
                    Dim Rythme As Integer = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_rythme"), 0)
                    Dim Base As String = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_base"), 0)
                    If Rythme <> 0 And Base <> "" Then
                        If dateLast <> Nothing Then
                            'Rendez-vous prévisionnel, calculé selon le rythme saisi et le dernier rendez-vous passé
                            dateNext = CalculProchainRendezVous(dateLast, Rythme, Base)
                            TextConsultationNext.SetText(dateNext.ToString(outils.FormatageDateAffichage(dateNext, False)))
                        Else
                            Dim DateCreation As Date = Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_date_creation"), Nothing)
                            If DateCreation <> Nothing Then
                                'Rendez-vous prévisionnel, calculé selon le rythme saisi et la date de création de l'intervenant dans le parcours de soin du patient
                                dateNext = CalculProchainRendezVous(DateCreation, Rythme, Base)
                                TextConsultationNext.SetText(dateNext.ToString(outils.FormatageDateAffichage(dateNext, False)))
                            Else
                                'Rendez-vous à venir non calculable
                            End If
                        End If
                    Else
                        'Pas de rendez-vous à venir pour cet intervenant
                    End If
                End If
            End If

            Dim TextCommentaire As New Text("")
            TextCommentaire.SetFontSize(8)
            TextCommentaire.SetText(Coalesce(ParcoursDataTable.Rows(i)("oa_parcours_commentaire"), ""))

            If PremierPassage = True Then
                table.AddCell(New Cell().Add(New Paragraph("Intervenant").SetFontSize(8).SetBold))
                table.AddCell(New Cell().Add(New Paragraph("Nom").SetFontSize(8).SetBold))
                table.AddCell(New Cell().Add(New Paragraph("Structure").SetFontSize(8).SetBold))
                table.AddCell(New Cell().Add(New Paragraph("Dern. Consult.").SetFontSize(8).SetBold))
                table.AddCell(New Cell().Add(New Paragraph("Proch. Consult.").SetFontSize(8).SetBold))
                table.AddCell(New Cell().Add(New Paragraph("Remarque").SetFontSize(8).SetBold))
                PremierPassage = False
            End If

            table.AddCell(New Cell().Add(New Paragraph(TextSpecialite)))
            table.AddCell(New Cell().Add(New Paragraph(TextNomIntervenant)))
            table.AddCell(New Cell().Add(New Paragraph(TextNomStructure)))
            table.AddCell(New Cell().Add(New Paragraph(TextConsultationLast)))
            table.AddCell(New Cell().Add(New Paragraph(TextConsultationNext)))
            table.AddCell(New Cell().Add(New Paragraph(TextCommentaire)))
        Next

        document.Add(table)
    End Sub

    Private Sub PrintContexte(document As Document)
        Dim p As New Paragraph
        Dim table As New Table(1)

        Dim contexteDataTable As DataTable
        Dim antecedentDao As AntecedentDao = New AntecedentDao
        contexteDataTable = antecedentDao.GetContextebyPatient(SelectedPatient.patientId, True)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer

        Comptage += contexteDataTable.Rows.Count
        GestionSautDePage(document)
        document.Add(New Paragraph(vbCrLf & "--- Contexte").SetFontSize(11))

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
            Dim longueurString As Integer
            Dim longueurMax As Integer = 150
            Dim contexteDescription As String
            contexteDescription = Coalesce(contexteDataTable.Rows(i)("oa_antecedent_description"), "")
            If contexteDescription <> "" Then
                contexteDescription = Replace(contexteDescription, vbCrLf, " ")
                longueurString = contexteDescription.Length
                If longueurString > longueurMax Then
                    longueurString = longueurMax
                End If
                contexteDescription.Substring(0, longueurString)
            End If

            Dim TextContexte As New Text("")
            TextContexte.SetText(AfficheDateModification & diagnostic & " " & contexteDescription & vbCrLf)
            TextContexte.SetFontSize(8)
            table.AddCell(New Cell().Add(New Paragraph(TextContexte).SetFixedLeading(10)))
        Next

        document.Add(table)
    End Sub

    Private Sub PrintPPS(document As Document)
        Dim p As New Paragraph
        Dim table As New Table(1)

        Dim PPSDataTable As DataTable
        Dim PPSDao As PpsDao = New PpsDao
        PPSDataTable = PPSDao.getAllPPSbyPatient(SelectedPatient.patientId)

        'Déclaration des variables pour réaliser le parcours du DataTable pour alimenter le DataGridView
        Dim i As Integer

        Comptage += PPSDataTable.Rows.Count
        GestionSautDePage(document)
        document.Add(New Paragraph(vbCrLf & "--- Plan personnalisé de soin").SetFontSize(11))

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
                        'TODO: récupération spécialité
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

            'Ajout d'une ligne au DataGridView
            Dim TextPps As New Text("")
            TextPps.SetText(AffichePPS & vbCrLf)
            TextPps.SetFontSize(8)
            If ppsArret = True Then
                TextPps.SetFontColor(iText.Kernel.Colors.ColorConstants.RED)
            End If
            table.AddCell(New Cell().Add(New Paragraph(TextPps).SetFixedLeading(10)))
        Next
        document.Add(table)
    End Sub

    Private Sub GestionSautDePage(document As Document)
        If Comptage > 40 Then
            document.Add(New Paragraph(vbCrLf & "Voir page suivante").SetFontSize(10))
            document.Add(New AreaBreak())
            PrintEntete(document)
            PrintEtatCivil(document)
        End If
    End Sub

End Class
