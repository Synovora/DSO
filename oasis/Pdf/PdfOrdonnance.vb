Imports System.IO
Imports iText.IO.Font.Constants
Imports iText.IO.Font.Otf
Imports iText.Kernel.Font
Imports iText.Kernel.Geom
Imports iText.Kernel.Pdf
Imports iText.Kernel.Pdf.Canvas.Parser.Listener
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

    Dim font As PdfFont

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
        font = PdfFontFactory.CreateFont(StandardFonts.TIMES_ROMAN)

        PrintEntete(document)
        PrintEtatCivil(document)
        PrintTraitement(document)


        document.Close()
    End Sub

    Private Sub PrintEntete(document As Document)
        PageNumero += 1

        Dim TextPage1 As New Text("")
        TextPage1.SetText("test textpage 1 - page" & PageNumero)
        TextPage1.SetFontSize(10)
        Dim p1 As New Paragraph("test paragraphe 1")
        p1.SetFont(font)
        p1.SetFixedLeading(20)
        'p1.SetFixedPosition(10, 10, 30)
        document.Add(p1).Add(TextPage1)


        document.Add(New Paragraph("Ordonnance").SetFontSize(14).Add(TextPage1).SetVerticalAlignment(TextAlignment.CENTER))

        Dim TextPage2 As New Text("")
        TextPage2.SetText("                               Service Oasis santé")
        TextPage2.SetFontSize(12)
        document.Add(New Paragraph("").SetFontSize(12).Add(TextPage2))
        document.Add(New Paragraph("Adresse").SetFontSize(10))
        document.Add(New Paragraph("Téléphone : xxxxxxx  Fax : xxxxxxxx").SetFontSize(10))

    End Sub


    Private Sub PrintEtatCivil(document As Document)
        document.Add(New Paragraph("--- Etat civil").SetFontSize(11))
        Dim ligne1 As String
        Dim TextEtatCivil As New Text("")
        Dim p As New Paragraph
        Dim table As New Table(1)

        Dim ALD As String = AldDao.DateFinALD(Me.SelectedPatient.patientId)
        ALD = ALD.Replace(vbCrLf, " ")

        ligne1 =
            "                            " & SelectedPatient.PatientNom & " " & SelectedPatient.PatientPrenom.ToUpper() &
            "       Date de naissance : " & SelectedPatient.PatientDateNaissance.ToString("dd.MM.yyyy") & "   Site " & Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId) &
            "   ,le " & SelectedPatient.PatientSyntheseDateMaj.ToString("dd.MM.yyyy")


        TextEtatCivil.SetText("  " & vbCrLf & ALD)
        TextEtatCivil.SetFontColor(iText.Kernel.Colors.ColorConstants.RED).SetFontSize(8)

        table.AddCell(New Cell().Add(p.Add(ligne1).SetFontSize(8).Add(TextEtatCivil)))
        document.Add(table)
        Comptage = 0
    End Sub

    Private Sub PrintTraitement(document As Document)
        Dim p As New Paragraph
        Dim table As New Table(3)
        Dim dt As DataTable
        Dim traitementDao As TraitementDao = New TraitementDao
        Dim ordonnanceDetailDao As New OrdonnanceDetailDao
        dt = ordonnanceDetailDao.getAllOrdonnanceLigneByOrdonnanceId(SelectedOrdonnanceId)

        'Ajout d'une colonne 'oa_traitement_posologie' dans le DataTable de traitement
        Dim i As Integer
        Dim rowCount As Integer = dt.Rows.Count - 1

        Comptage += dt.Rows.Count
        GestionSautDePage(document)
        document.Add(New Paragraph(vbCrLf & "--- Traitement").SetFontSize(11))

        Dim Base As String
        Dim Posologie As String
        Dim dateFin, dateDebut, dateModification As Date
        Dim posologieMatin, posologieMidi, posologieApresMidi, posologieSoir As Integer
        Dim Rythme As Integer
        Dim FenetreTherapeutiqueEnCours As Boolean
        Dim FenetreTherapeutiqueAVenir As Boolean

        Dim PremierPassage As Boolean = True

        'Dim Allergie As Boolean = False
        Dim FenetreDateDebut, FenetreDateFin As Date

        'Parcours du DataTable pour alimenter les colonnes du DataGridView
        For i = 0 To rowCount Step 1
            'Date de fin
            If dt.Rows(i)("oa_traitement_date_fin") IsNot DBNull.Value Then
                dateFin = dt.Rows(i)("oa_traitement_date_fin")
            Else
                dateFin = "31/12/2999"
            End If

            'Date début
            If dt.Rows(i)("oa_traitement_date_debut") IsNot DBNull.Value Then
                dateDebut = dt.Rows(i)("oa_traitement_date_debut")
            Else
                dateDebut = "01/01/1900"
            End If

            'Vérification de l'existence d'une fenêtre thérapeutique active et à venir
            FenetreTherapeutiqueEnCours = False
            FenetreTherapeutiqueAVenir = False

            If dt.Rows(i)("oa_traitement_fenetre_date_debut") IsNot DBNull.Value Then
                FenetreDateDebut = dt.Rows(i)("oa_traitement_fenetre_date_debut")
            Else
                FenetreDateDebut = "31/12/2999"
            End If

            If dt.Rows(i)("oa_traitement_fenetre_date_fin") IsNot DBNull.Value Then
                FenetreDateFin = dt.Rows(i)("oa_traitement_fenetre_date_fin")
            Else
                FenetreDateFin = "01/01/1900"
            End If

            Posologie = ""

            'Existence d'une fenêtre thérapeutique
            Dim FenetreTherapeutiqueExiste As Boolean = False
            If dt.Rows(i)("oa_traitement_fenetre") IsNot DBNull.Value Then
                If dt.Rows(i)("oa_traitement_fenetre") = "1" Then
                    'Fenêtre thérapeutique en cours, à venir ou obsolète
                    FenetreTherapeutiqueExiste = True
                    If FenetreDateDebut.Date <= Date.Now.Date And FenetreDateFin >= Date.Now.Date Then
                        'Fenêtre thérapeutique en cours
                        FenetreTherapeutiqueEnCours = True
                        Posologie = "Fenêtre Th."
                    Else
                        If FenetreDateDebut.Date > Date.Now.Date Then
                            FenetreTherapeutiqueAVenir = True
                        End If
                    End If
                End If
            End If

            If FenetreTherapeutiqueEnCours = False Then
                Posologie = Coalesce(dt.Rows(i)("oa_traitement_posologie"), "")
            End If

            'Formatage de la posologie
            If True = False Then
                Dim PosologieMatinString, PosologieMidiString, PosologieApresMidiString, PosologieSoirString As String
                Dim FractionMatin, FractionMidi, FractionApresMidi, FractionSoir As String
                Dim PosologieBase As String

                FractionMatin = Coalesce(dt.Rows(i)("oa_traitement_fraction_matin"), TraitementDao.EnumFraction.Non)
                FractionMidi = Coalesce(dt.Rows(i)("oa_traitement_fraction_midi"), TraitementDao.EnumFraction.Non)
                FractionApresMidi = Coalesce(dt.Rows(i)("oa_traitement_fraction_apres_midi"), TraitementDao.EnumFraction.Non)
                FractionSoir = Coalesce(dt.Rows(i)("oa_traitement_fraction_soir"), TraitementDao.EnumFraction.Non)

                posologieMatin = Coalesce(dt.Rows(i)("oa_traitement_Posologie_matin"), 0)
                posologieMidi = Coalesce(dt.Rows(i)("oa_traitement_Posologie_midi"), 0)
                posologieApresMidi = Coalesce(dt.Rows(i)("oa_traitement_Posologie_apres_midi"), 0)
                posologieSoir = Coalesce(dt.Rows(i)("oa_traitement_Posologie_soir"), 0)

                PosologieBase = Coalesce(dt.Rows(i)("oa_traitement_Posologie_base"), "")

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
                If dt.Rows(i)("oa_traitement_posologie_base") IsNot DBNull.Value Then
                    Rythme = dt.Rows(i)("oa_traitement_posologie_rythme")
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
                            Select Case dt.Rows(i)("oa_traitement_posologie_base")
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

            Dim commentaire As String = Coalesce(dt.Rows(i)("oa_traitement_commentaire"), "")
            Dim commentairePosologie As String = Coalesce(dt.Rows(i)("oa_traitement_posologie_commentaire"), "")

            'Stockage des médicaments prescrits (pour contrôle lors de la selection d'un médicament dans le cadre d'un nouveau traitement
            SelectedPatient.PatientMedicamentsPrescritsCis.Add(dt.Rows(i)("oa_traitement_medicament_cis"))

            Dim TextMedicamentDci As New Text(dt.Rows(i)("oa_traitement_medicament_dci"))
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
                table.AddCell(New Cell().Add(New Paragraph("Délivrance").SetFontSize(8).SetBold))
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
