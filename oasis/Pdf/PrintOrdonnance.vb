Imports Telerik.WinControls.RichTextEditor.UI
Imports Telerik.WinControls.UI
Imports Telerik.WinForms.Documents.Model
Imports Oasis_Common
Imports Telerik.WinForms.Documents.Layout
Imports Telerik.WinForms.Documents.Model.Styles

Public Class PrintOrdonnance

    Public Property SelectedPatient As Patient
    Public Property ordonnanceId As Long

    Dim alddao As New AldDao

    Sub PrintDocument()

        Dim rte As New RadRichTextEditor

        rte.LayoutMode = DocumentLayoutMode.Paged
        rte.Document.SectionDefaultPageSize = PaperTypeConverter.ToSize(PaperTypes.A4)

        'CreateDoc(rte.Document)
        PrintEntete(rte.Document)

        rte.LoadElementTree()
        rte.PrintPreview()
    End Sub

    Private Sub CreateDoc(document As RadDocument)
        Dim s1 As New Section
        Dim p1 As New Paragraph()
        Dim span1 As New Span("test 1 --------------------")
        Dim span2 As New Span("test 2 --------------------")

        span2.Text = "bbbb"

        p1.Inlines.Add(span1)
        p1.Inlines.Add(span2)

        s1.Blocks.Add(p1)

        document.Sections.Add(s1)

    End Sub

    Private Sub PrintEntete(document As RadDocument)
        Dim s1 As New Section

        Dim p1 As New Paragraph()

        Dim sp1 As New Span("Ordonnance")
        sp1.FontSize = 16
        p1.Inlines.Add(sp1)

        p1.TextAlignment = RadTextAlignment.Center
        p1.FontSize = 16
        s1.Blocks.Add(p1)

        Dim p2 As New Paragraph()
        Dim sp2 As New Span("Service Oasis Santé")
        p2.Inlines.Add(sp2)
        p2.FontSize = 12
        p2.TextAlignment = RadTextAlignment.Center
        s1.Blocks.Add(p2)

        document.Sections.Add(s1)

    End Sub

    Private Sub PrintEtatCivil(document As RadDocument)
        'document.Add(New Paragraph("--- Etat civil").SetFontSize(11))
        Dim ligne1 As String
        'Dim TextEtatCivil As New Text("")
        Dim p As New Paragraph
        'Dim table As New Table(1)

        Dim ALD As String = alddao.DateFinALD(Me.SelectedPatient.patientId)
        ALD = ALD.Replace(vbCrLf, " ")

        ligne1 = "Prénom / Nom : " &
            SelectedPatient.PatientPrenom & " " &
            SelectedPatient.PatientNom.ToUpper() &
            "       NIR : " & SelectedPatient.PatientNir.ToString &
            "          " & SelectedPatient.PatientGenre & vbCrLf &
            "   Date de naissance : " & SelectedPatient.PatientDateNaissance.ToString("dd.MM.yyyy") & "   -   âge : " & outils.CalculAgeEnAnneeEtMoisString(SelectedPatient.PatientDateNaissance) & vbCrLf &
            "   Rattachement au site Oasis de " & Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId) &
            "   -  Dernière mise à jour de la synthèse : " & FormatageDateAffichage(SelectedPatient.PatientSyntheseDateMaj, True)


        'TextEtatCivil.SetText("  " & vbCrLf & ALD)
        'TextEtatCivil.SetFontColor(iText.Kernel.Colors.ColorConstants.RED).SetFontSize(8)

        'table.AddCell(New Cell().Add(p.Add(ligne1).SetFontSize(8).Add(TextEtatCivil)))
        'document.Add(table)
        'Comptage = 0
    End Sub

    Private Sub PrintTraitement(document As RadDocument)
        Dim p As New Paragraph
        'Dim table As New Table(3)
        Dim dt As DataTable
        Dim traitementDao As TraitementDao = New TraitementDao
        Dim ordonnanceDetailDao As New OrdonnanceDetailDao
        dt = ordonnanceDetailDao.getAllOrdonnanceLigneByOrdonnanceId(ordonnanceId)

        'Ajout d'une colonne 'oa_traitement_posologie' dans le DataTable de traitement
        Dim i As Integer
        Dim rowCount As Integer = dt.Rows.Count - 1

        'Comptage += dt.Rows.Count
        'GestionSautDePage(document)
        'document.Add(New Paragraph(vbCrLf & "--- Traitement").SetFontSize(11))

        Dim Posologie As String
        Dim dateFin, dateDebut, dateModification As Date

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



            Dim commentaire As String = Coalesce(dt.Rows(i)("oa_traitement_commentaire"), "")
            Dim commentairePosologie As String = Coalesce(dt.Rows(i)("oa_traitement_posologie_commentaire"), "")

            'Stockage des médicaments prescrits (pour contrôle lors de la selection d'un médicament dans le cadre d'un nouveau traitement
            SelectedPatient.PatientMedicamentsPrescritsCis.Add(dt.Rows(i)("oa_traitement_medicament_cis"))

            'Dim TextMedicamentDci As New Text(dt.Rows(i)("oa_traitement_medicament_dci"))
            'TextMedicamentDci.SetFontSize(8)
            'Posologie
            'Dim TextPosologie As New Text(Posologie)
            'posologieTraitement.SetText(Posologie)
            'TextPosologie.SetFontSize(8)

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
            'Dim TextDateModification As New Text(DateModificationString)
            'dateModificationTraitement.SetText(DateModificationString)
            'TextDateModification.SetFontSize(8)
            'Bouton gérer fenêtre thérapeutique

            If FenetreTherapeutiqueAVenir = True Or FenetreTherapeutiqueEnCours = True Then
                'RadTraitementDataGridView.Rows(iGrid).Cells("posologie").Style.ForeColor = Color.Red
            End If

            If PremierPassage = True Then
                'table.AddCell(New Cell().Add(New Paragraph("Traitement").SetFontSize(8).SetBold))
                'table.AddCell(New Cell().Add(New Paragraph("Posologie").SetFontSize(8).SetBold))
                'table.AddCell(New Cell().Add(New Paragraph("Délivrance").SetFontSize(8).SetBold))
                PremierPassage = False
            End If

            'table.AddCell(New Cell().Add(New Paragraph(TextMedicamentDci)))
            'table.AddCell(New Cell().Add(New Paragraph(TextPosologie)))
            'table.AddCell(New Cell().Add(New Paragraph(TextDateModification)))
        Next
        'document.Add(table)
    End Sub

    Private Sub init(rte As RadRichTextEditor)
        Dim charStyle As New StyleDefinition()
        charStyle.Type = StyleType.Character
        charStyle.SpanProperties.FontFamily = New Telerik.WinControls.RichTextEditor.UI.FontFamily("Calibri")
        charStyle.SpanProperties.FontSize = Unit.PointToDip(20)
        'charStyle.SpanProperties.ForeColor = Colors.Orange
        charStyle.DisplayName = "charStyle"
        charStyle.Name = "charStyle"
        rte.Document.StyleRepository.Add(charStyle)
    End Sub

End Class
