Imports System.IO
Imports iText.IO.Font.Constants
Imports iText.Kernel.Font
Imports iText.Kernel.Geom
Imports iText.Kernel.Pdf
Imports iText.Layout
Imports iText.Layout.Element
Imports iText.Layout.Properties
Imports Oasis_Common


Public Class PdfOrdonnance
    Private _selectedPatient As Patient
    Private _selectedOrdonnanceId As Long

    Public Property SelectedPatient As Patient
        Get
            Return _selectedPatient
        End Get
        Set(value As Patient)
            _selectedPatient = value
        End Set
    End Property

    Public Property SelectedOrdonnanceId As Long
        Get
            Return _selectedOrdonnanceId
        End Get
        Set(value As Long)
            _selectedOrdonnanceId = value
        End Set
    End Property

    Dim aldDao As New AldDao

    Dim DEST As String
    Dim NomPdf As String
    Dim Comptage, PageNumero As Integer
    Dim DateGeneration As Date = Date.Now()

    Friend Sub ImprimeOrdonnance()
        NomPdf = "Ordonnance_" & SelectedPatient.patientId & "_" & DateGeneration.ToString("yyyy-MM-dd_HH-mm-ss") & ".pdf"
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
            form.Message = "Le PDF à générer est en consultation et ne peut être remplacé"
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
        PrintTraitement(document)


        document.Close()
    End Sub

    Private Sub PrintEntete(document As Document)
        Dim TextPage As New Text("")
        PageNumero += 1
        TextPage.SetText("         -          Document généré le " & DateGeneration.ToString("dd.MM.yyyy") &
                         " à " & DateGeneration.ToString("HH:mm:ss") & "          -          Page " & PageNumero)
        TextPage.SetFontSize(10)

        document.Add(New Paragraph("         Ordonnance patient").SetFontSize(14).Add(TextPage))

    End Sub

    Private Sub PrintEtatCivil(document As Document)
        document.Add(New Paragraph("--- Etat civil").SetFontSize(11))
        Dim ligne1 As String
        Dim TextEtatCivil As New Text("")
        Dim p As New Paragraph
        Dim table As New Table(1)

        Dim ALD As String = AldDao.DateFinALD(Me.SelectedPatient.patientId)
        ALD = ALD.Replace(vbCrLf, " ")

        ligne1 = "Prénom / Nom : " &
            SelectedPatient.PatientPrenom & " " &
            SelectedPatient.PatientNom.ToUpper() &
            "       NIR : " & SelectedPatient.PatientNir.ToString &
            "          " & SelectedPatient.PatientGenre & vbCrLf &
            "   Date de naissance : " & SelectedPatient.PatientDateNaissance.ToString("dd.MM.yyyy") & "   -   âge : " & outils.CalculAgeEnAnneeEtMoisString(SelectedPatient.PatientDateNaissance) & vbCrLf &
            "   Rattachement au site Oasis de " & Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId) &
            "   -  Dernière mise à jour de la synthèse : " & FormatageDateAffichage(SelectedPatient.PatientSyntheseDateMaj, True)


        TextEtatCivil.SetText("  " & vbCrLf & ALD)
        TextEtatCivil.SetFontColor(iText.Kernel.Colors.ColorConstants.RED).SetFontSize(8)

        table.AddCell(New Cell().Add(p.Add(ligne1).SetFontSize(8).Add(TextEtatCivil)))
        document.Add(table)
        Comptage = 0
    End Sub

    Private Sub PrintTraitement(document As Document)
        Dim p As New Paragraph
        Dim table As New Table(3)
        Dim traitementDataTable As DataTable
        Dim traitementDao As TraitementDao = New TraitementDao
        traitementDataTable = traitementDao.getTraitementEnCoursbyPatient(Me.SelectedPatient.patientId)

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

    Private Sub GestionSautDePage(document As Document)
        If Comptage > 40 Then
            document.Add(New Paragraph(vbCrLf & "Voir page suivante").SetFontSize(10))
            document.Add(New AreaBreak())
            PrintEntete(document)
            PrintEtatCivil(document)
        End If
    End Sub

End Class
