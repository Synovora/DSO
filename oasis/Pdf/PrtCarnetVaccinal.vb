Imports Telerik.WinForms.Documents.Model
Imports Telerik.WinForms.Documents.Layout
Imports Telerik.WinControls.RichTextEditor.UI
Imports Oasis_Common
Imports Microsoft.IdentityModel.Tokens
Imports Nethereum.Hex.HexConvertors.Extensions
Imports QRCoder
Imports System.Globalization

Public Class PrtCarnetVaccinal
    Public Property SelectedPatient As Patient
    Public Property startDate As Date
    Public Property endDate As Date

    ReadOnly EditTools As New OasisTextTools

    ReadOnly episodeParametreDao As New EpisodeParametreDao
    ReadOnly userDao As New UserDao
    ReadOnly rorDao As New RorDao
    ReadOnly siteDao As New SiteDao
    ReadOnly cgvValenceDao As New CGVValenceDao
    ReadOnly cgvDateDao As New CGVDateDao
    ReadOnly uniteSanitaireDao As New UniteSanitaireDao
    ReadOnly siegeDao As New SiegeDao
    ReadOnly vaccinDao As New VaccinDao

    Dim valences As List(Of CGVValence)
    Dim relations As New List(Of RelationValenceDate)
    Dim cgvDates As List(Of CGVDate)
    Dim Vaccins As List(Of VaccinValence)
    Dim ordonnance As Ordonnance

    Dim TraitementAldExiste As Boolean = False

    Public Sub PrintDocument()
        Try
            Vaccins = vaccinDao.GetListVaccinValence()
            valences = cgvValenceDao.GetListFromPatient(SelectedPatient.PatientId)
            cgvDates = cgvDateDao.GetListFromPatient(SelectedPatient.PatientId)
            relations = cgvDateDao.GetRelationListFromPatient(SelectedPatient.PatientId)
            GenereDocument()
            EditTools.PrintPreview()
        Catch ex As Exception
            MsgBox(ex.Message())
        Finally
            EditTools.Dispose()
        End Try
    End Sub

    Public Function ExportDocumenttoPdfBytes() As Byte()

        Try
            Vaccins = vaccinDao.GetListVaccinValence()
            valences = cgvValenceDao.GetListFromPatient(SelectedPatient.PatientId)
            cgvDates = cgvDateDao.GetListFromPatient(SelectedPatient.PatientId)
            relations = cgvDateDao.GetRelationListFromPatient(SelectedPatient.PatientId)
            GenereDocument()
            Return EditTools.exportToPdf()
        Finally
            EditTools.Dispose()
        End Try
    End Function

    Private Sub GenereDocument()
        Dim section = EditTools.CreateSection()
        Dim document = EditTools.AddSectionIntoDocument(Nothing, section)

        PrintEntete(section)
        PrintEtatCivil(section)
        EditTools.InsertFragmentToEditor(document)
        EditTools.InsertFragmentToEditor(PrintCarnetDetail())

        Dim sectionFin = EditTools.CreateSection()
        Dim documentFin = EditTools.AddSectionIntoDocument(Nothing, sectionFin)
        PrintBasPage(sectionFin)
        EditTools.InsertFragmentToEditor(documentFin)
        EditTools.AddPageNumber()
    End Sub


    Private Sub PrintEntete(section As Section)
        Dim site As Site
        site = siteDao.getSiteById(SelectedPatient.PatientSiteId)
        Dim uniteSanitaire As UniteSanitaire
        uniteSanitaire = uniteSanitaireDao.getUniteSanitaireById(site.Oa_site_unite_sanitaire_id)
        Dim siege As Siege
        siege = siegeDao.getSiegeById(uniteSanitaire.Oa_unite_sanitaire_siege_id)
        With EditTools
            .CreateParagraphIntoSection(section, 15, RadTextAlignment.Center)
            .AddTexte("Carnet Vaccinal", 16, FontWeights.Bold)
            .AddNewLigne()
            .AddTexteLine("Service Oasis Santé", 14)
            .AddTexteLine("Tel : " & siege.SiegeTelephone & "| Fax : " & siege.SiegeFax)
            .AddTexteLine("Mail : " & siege.SiegeMail)
            .AddTexte("Numéro structure : " & uniteSanitaire.NumeroStructure)
        End With
    End Sub

    Private Sub PrintEtatCivil(section As Section)
        With EditTools
            .CreateParagraphIntoSection(section,, RadTextAlignment.Left)
            .AddTexteLine(SelectedPatient.PatientNom & " " & SelectedPatient.PatientPrenom)

            Dim DateNaissancePatient As Date = SelectedPatient.PatientDateNaissance
            .AddTexteLine("Date de naissance : " & DateNaissancePatient.ToString("dd.MM.yyyy"))
            .AddTexteLine("Immatriculation CPAM : " & SelectedPatient.PatientNir)
            .AddNewLigne()
            .AddTexteLine("Document imprime le: " & Date.Now.ToString("dd/MM/yyyy"))
            .AddNewLigne()
            .AddTexteLine("Vaccins realises du " & startDate.ToString("dd/MM/yyyy") & " au " & endDate.ToString("dd/MM/yyyy"))
            .AddNewLigne()
            .CreateParagraphIntoSection(section,, RadTextAlignment.Right)
            Dim SiteDescription As String = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        End With
    End Sub

    Private Function PrintCarnetDetail() As RadDocument
        Try
            cgvDates.Sort(Function(x, y) x.Days - y.Days)
            Const LargeurCol As Integer = 60
            Dim document As New RadDocument()
            Dim section As New Section()

            Dim table As New Table With {
                .LayoutMode = TableLayoutMode.Fixed,
                .StyleName = RadDocumentDefaultStyles.DefaultTableGridStyleName
            }

            Dim rowTitle As New TableRow()

            Dim cellTitre2 As New TableCell With {
                    .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol * 3)
                }
            Dim spanTitre2 As New Span With {
                    .FontSize = 10,
                    .FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold
                }
            Dim paragrapheTitre2 As New Paragraph()
            spanTitre2.Text = "Realisation"
            paragrapheTitre2.TextAlignment = RadTextAlignment.Center
            paragrapheTitre2.Inlines.Add(spanTitre2)
            cellTitre2.Blocks.Add(paragrapheTitre2)
            rowTitle.Cells.Add(cellTitre2)

            Dim cellTitre3 As New TableCell With {
                    .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol * 5)
                }
            Dim spanTitre3 As New Span With {
                    .FontSize = 10,
                    .FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold
                }
            Dim paragrapheTitre3 As New Paragraph()
            spanTitre3.Text = "Vaccins"
            paragrapheTitre3.TextAlignment = RadTextAlignment.Center
            paragrapheTitre3.Inlines.Add(spanTitre3)
            cellTitre3.Blocks.Add(paragrapheTitre3)
            rowTitle.Cells.Add(cellTitre3)

            Dim cellTitre4 As New TableCell With {
                    .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol)
                }
            Dim spanTitre4 As New Span With {
                    .FontSize = 10,
                    .FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold
                }
            Dim paragrapheTitre4 As New Paragraph()
            spanTitre4.Text = "Lot"
            paragrapheTitre4.TextAlignment = RadTextAlignment.Center
            paragrapheTitre4.Inlines.Add(spanTitre4)
            cellTitre4.Blocks.Add(paragrapheTitre4)
            rowTitle.Cells.Add(cellTitre4)

            Dim cellTitre5 As New TableCell With {
                    .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol)
                }
            Dim spanTitre5 As New Span With {
                    .FontSize = 10,
                    .FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold
                }
            Dim paragrapheTitre5 As New Paragraph()
            spanTitre5.Text = "Exp"
            paragrapheTitre5.TextAlignment = RadTextAlignment.Center
            paragrapheTitre5.Inlines.Add(spanTitre5)
            cellTitre5.Blocks.Add(paragrapheTitre5)
            rowTitle.Cells.Add(cellTitre5)

            Dim celleTitre6 As New TableCell With {
                    .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol * 2)
                    }
            Dim spanTitre6 As New Span With {
                    .FontSize = 10,
                    .FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold
                }
            Dim paragrapheTitre6 As New Paragraph()
            spanTitre6.Text = "Valence"
            paragrapheTitre6.TextAlignment = RadTextAlignment.Center
            paragrapheTitre6.Inlines.Add(spanTitre6)
            celleTitre6.Blocks.Add(paragrapheTitre6)
            rowTitle.Cells.Add(celleTitre6)

            table.Rows.Add(rowTitle)

            For Each cgvDate As CGVDate In cgvDates
                Dim VaccinProgram = vaccinDao.GetFirstVaccinProgramRelationListDatePatient(cgvDate.Id, SelectedPatient.PatientId)
                If VaccinProgram Is Nothing OrElse VaccinProgram.RealisationDate = Nothing OrElse VaccinProgram.RealisationDate < startDate OrElse VaccinProgram.RealisationDate > endDate Then
                    Continue For
                End If
                Dim VaccinPrograms = vaccinDao.GetVaccinProgramRelationListDatePatient(cgvDate.Id, SelectedPatient.PatientId)
                Dim isFirstLine = True
                For Each VaccinProgram In VaccinPrograms
                    Dim row As New TableRow()
                    Dim vaccinProgramAdmin = vaccinDao.GetVaccinProgramAdministrationByRelation(VaccinPrograms.Find(Function(x) x.Vaccin = VaccinProgram.Vaccin).Id)
                    If (vaccinProgramAdmin Is Nothing) Then 'TODO: Error
                        Continue For
                    End If
                    Dim isLastRow = If(VaccinPrograms.IndexOf(VaccinProgram) = VaccinPrograms.Count - 1, True, False)
                    If isFirstLine = True Then
                        Dim cellDetail2 As New TableCell With {
                        .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol * 3),
                              .Borders = New TableCellBorders(
                           New Border(1, BorderStyle.Single, Colors.Black), New Border(1, BorderStyle.Single, Colors.Black), New Border(1, BorderStyle.Single, Colors.Black), If(isLastRow, New Border(1, BorderStyle.Single, Colors.Black), New Border(0, BorderStyle.None, Colors.Black))
                        )
                    }
                        Dim spanDetail2 As New Span With {
                        .FontSize = 10,
                        .FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold
                    }
                        Dim paragrapheDetail2 As New Paragraph()
                        Dim Text = ""
                        If (VaccinProgram IsNot Nothing AndAlso VaccinProgram.RealisationDate <> Nothing) Then
                            If (VaccinProgram.RealisationOperator <> Nothing) Then
                                Text = GetProfilUserString(userDao.GetUserById(VaccinProgram.RealisationOperator))
                            ElseIf (VaccinProgram.RealisationOperatorRor <> Nothing) Then
                                Text = GetProfilUserString(rorDao.GetRorById(VaccinProgram.RealisationOperatorRor))
                            ElseIf (VaccinProgram.RealisationOperatorText <> Nothing) Then
                                Text = VaccinProgram.RealisationOperatorText
                            End If
                        End If
                        spanDetail2.Text = String.Format("{0} - {1}", VaccinProgram.RealisationDate.ToShortDateString(), Text) 'If(VaccinProgram <> Nothing AndAlso VaccinProgram.RealisationDate <> Nothing, String.Format("{0} - {1}",  VaccinProgram.RealisationDate.ToShortDateString(), GetProfilUserString2(userDao.GetUserById( VaccinProgram.RealisationOperator))), "")
                        paragrapheDetail2.Inlines.Add(spanDetail2)
                        cellDetail2.Blocks.Add(paragrapheDetail2)
                        row.Cells.Add(cellDetail2)
                    Else
                        Dim cellDetail2 As New TableCell With {
                    .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol * 3),
                       .Borders = New TableCellBorders(
                           New Border(1, BorderStyle.Single, Colors.Black), New Border(0, BorderStyle.None, Colors.Black), New Border(1, BorderStyle.Single, Colors.Black), New Border(0, BorderStyle.None, Colors.Black)
                        )
                    }
                        row.Cells.Add(cellDetail2)
                    End If

                    Dim cellDetail3 As New TableCell With {
                    .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol * 5)
                }
                    Dim spanDetail3 As New Span With {
                    .FontSize = 10,
                    .FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold
                }
                    spanDetail3.Text = Vaccins.Find(Function(x) x.Id = VaccinProgram.Vaccin).Dci
                    Dim paragrapheDetail3 As New Paragraph()
                    paragrapheDetail3.Inlines.Add(spanDetail3)
                    cellDetail3.Blocks.Add(paragrapheDetail3)
                    row.Cells.Add(cellDetail3)

                    Dim cellDetail4 As New TableCell With {
                    .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol)
                }
                    Dim spanDetail4 As New Span With {
                    .FontSize = 10,
                    .FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold
                }
                    spanDetail4.Text = vaccinProgramAdmin.Lot
                    Dim paragrapheDetail4 As New Paragraph()
                    paragrapheDetail4.Inlines.Add(spanDetail4)
                    cellDetail4.Blocks.Add(paragrapheDetail4)
                    row.Cells.Add(cellDetail4)

                    Dim cellDetail5 As New TableCell With {
                    .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol)
                }
                    Dim spanDetail5 As New Span With {
                    .FontSize = 10,
                    .FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold
                }
                    spanDetail5.Text = vaccinProgramAdmin.Expiration.ToString("MM/yyyy", CultureInfo.InvariantCulture)
                    Dim paragrapheDetail5 As New Paragraph()
                    paragrapheDetail5.Inlines.Add(spanDetail5)
                    cellDetail5.Blocks.Add(paragrapheDetail5)
                    row.Cells.Add(cellDetail5)

                    Dim cellDetail6 As New TableCell With {
                    .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol)
                }
                    Dim spanDetail6 As New Span With {
                    .FontSize = 10,
                    .FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold
                }
                    Dim valenceList = (From _vaccins In Vaccins.FindAll(Function(y) y.Code = VaccinProgram.RelationVaccinValence) Select _vaccins.Valence).ToArray()
                    spanDetail6.Text = String.Join(", ", (From _valence In valences.FindAll(Function(x) valenceList.Contains(x.Valence)) Select _valence.Code).ToArray())
                    Dim paragrapheDetail6 As New Paragraph()
                    paragrapheDetail6.Inlines.Add(spanDetail6)
                    cellDetail6.Blocks.Add(paragrapheDetail6)
                    row.Cells.Add(cellDetail6)

                    table.Rows.Add(row)
                    isFirstLine = False
                Next
            Next

            section.Blocks.Add(table)
            section.Blocks.Add(New Paragraph())
            document.Sections.Add(section)

            Return document
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
    End Function

    Private Sub PrintBasPage(section As Section)
        Try
            'Dim ordonnance As Ordonnance = ordonnanceDao.GetOrdonnaceById(SelectedOrdonnanceId)
            'Dim QG As QRCodeGenerator = New QRCoder.QRCodeGenerator()
            'Dim Data As QRCodeData = QG.CreateQrCode(SIGN_URL & Base64UrlEncoder.Encode(ordonnance.Signature.HexToByteArray()), QRCodeGenerator.ECCLevel.L)
            'Dim my_qrCode = New QRCode(Data)
            'With EditTools
            '    .CreateParagraphIntoSection(section,, RadTextAlignment.Right)
            '    Dim Medecin As Utilisateur = userDao.GetUserById(ordonnance.UserValidation)
            '    Dim profil As Profil = profilDao.getProfilById(Medecin.UtilisateurProfilId)
            '    .AddTexteLine(Medecin.UtilisateurPrenom & " " & Medecin.UtilisateurNom & ", " & profil.Designation)
            '    .AddTexteLine("RPPS : " & Medecin.UtilisateurRPPS)
            '    .AddImage(New WriteableBitmap(my_qrCode.GetGraphic(3)), New Size(150, 150))
            'End With
            'With EditTools
            '    .CreateParagraphIntoSection(section,, RadTextAlignment.Left)
            '    .AddTexteLine(If(ordonnance.Renouvellement > 0, "A renouveller " & ordonnance.Renouvellement & " fois", "Non renouvelable"))
            'End With
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
    End Sub
End Class
