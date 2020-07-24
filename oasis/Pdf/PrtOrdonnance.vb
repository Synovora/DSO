Imports Telerik.WinForms.Documents.Model
Imports Telerik.WinForms.Documents.Layout
Imports Telerik.WinControls.RichTextEditor.UI
Imports Oasis_Common
Imports Microsoft.IdentityModel.Tokens
Imports Nethereum.Hex.HexConvertors.Extensions
Imports QRCoder

Public Class PrtOrdonnance
    Public Property SelectedPatient As Patient
    Public Property SelectedOrdonnanceId As Long

    ReadOnly EditTools As New OasisTextTools

    ReadOnly ordonnanceDao As New OrdonnanceDao
    ReadOnly ordonnanceDetailDao As New OrdonnanceDetailDao
    ReadOnly episodeParametreDao As New EpisodeParametreDao
    ReadOnly traitementDao As New TraitementDao
    ReadOnly userDao As New UserDao
    ReadOnly siteDao As New SiteDao
    ReadOnly uniteSanitaireDao As New UniteSanitaireDao
    ReadOnly siegeDao As New SiegeDao
    ReadOnly profilDao As New ProfilDao
    ReadOnly theriaqueDao As New TheriaqueDao
    ReadOnly aldDao As New AldDao
    ReadOnly SIGN_URL As String = "https://localhost:44355/Sign/Check/"

    Dim ordonnance As Ordonnance

    Dim PatientIsAld As Boolean = False
    Dim TraitementAldExiste As Boolean = False

    Public Sub PrintDocument()
        ordonnance = ordonnanceDao.GetOrdonnaceById(SelectedOrdonnanceId)
        If aldDao.IsPatientALD(SelectedPatient.PatientId) Then
            PatientIsAld = True
        End If

        Try
            Dim section = EditTools.CreateSection()
            Dim document = EditTools.AddSectionIntoDocument(Nothing, section)

            PrintEntete(section)
            PrintEtatCivil(section)
            PrintEnteteALD(section)
            EditTools.InsertFragmentToEditor(document)
            EditTools.InsertFragmentToEditor(PrintOrdonnanceDetail(True))

            'Si au moins un traitement ALD existe, il faut présenter la signature du praticien qui a validé l'ordonnance
            Dim sectionAld = EditTools.CreateSection()
            Dim documentAld = EditTools.AddSectionIntoDocument(Nothing, sectionAld)
            If TraitementAldExiste = True Then
                PrintBasPage(sectionAld)
                EditTools.InsertFragmentToEditor(documentAld)
            Else
                EditTools.CreateParagraphIntoSection(sectionAld)
                EditTools.AddNewLigne()
                EditTools.AddNewLigne()
                EditTools.AddNewLigne()
                EditTools.AddNewLigne()
                EditTools.InsertFragmentToEditor(documentAld)
            End If

            Dim sectionNonALD = EditTools.CreateSection()
            Dim documentNonALD = EditTools.AddSectionIntoDocument(Nothing, sectionNonALD)
            PrintEnteteNonALD(sectionNonALD)
            EditTools.InsertFragmentToEditor(documentNonALD)
            EditTools.InsertFragmentToEditor(PrintOrdonnanceDetail(False))

            Dim sectionFin = EditTools.CreateSection()
            Dim documentFin = EditTools.AddSectionIntoDocument(Nothing, sectionFin)
            PrintBasPage(sectionFin)
            EditTools.InsertFragmentToEditor(documentFin)

            EditTools.PrintPreview()
        Catch ex As Exception
            MsgBox(ex.Message())
        Finally
            EditTools.Dispose()
        End Try
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
            .AddTexte("Ordonnance", 16, FontWeights.Bold)
            .AddNewLigne()
            .AddTexteLine("Service Oasis Santé", 14)
            .AddTexteLine("Tel : " & siege.SiegeTelephone & " Fax : " & siege.SiegeFax)
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
            Dim Poids As Double = episodeParametreDao.GetPoidsByEpisodeIdOrLastKnow(0, SelectedPatient.PatientId)
            If Poids > 0 Then
                .AddTexteLine("Poids : " & Poids & " Kg")
            End If
            .AddTexte("Age : " & SelectedPatient.PatientAge)

            .CreateParagraphIntoSection(section,, RadTextAlignment.Right)
            Dim SiteDescription As String = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
            Dim DateCreationOrdonnance As Date = ordonnance.DateCreation
            .AddTexteLine(SiteDescription & ", le " & DateCreationOrdonnance.ToString("dd.MM.yyyy"))
        End With
    End Sub

    Private Sub PrintEnteteALD(section As Section)
        If PatientIsAld = False Then
            Return
        End If

        With EditTools
            .CreateParagraphIntoSection(section, 12, RadTextAlignment.Center)
            .AddTexteLine("-------------------------------------------------------------------------------------------------------------------")
            Dim enteteAld As String = "Prescriptions relatives au traitement de l'affection longue durée reconnue (liste ou hors liste)"
            .AddTexteLine(enteteAld, 11, FontWeights.Bold)
            .AddTexteLine("AFFECTION EXONERANTE", 14)
            .AddTexte("-------------------------------------------------------------------------------------------------------------------")
        End With
    End Sub

    Private Sub PrintEnteteNonALD(section As Section)
        If PatientIsAld = False Then
            Return
        End If

        With EditTools
            .CreateParagraphIntoSection(section, 12, RadTextAlignment.Center)
            .AddTexteLine("-------------------------------------------------------------------------------------------------------------------")
            Dim enteteAld As String = "Prescriptions sans rapport avec l'affection longue durée"
            .AddTexteLine(enteteAld, 11, FontWeights.Bold)
            .AddTexteLine("MALADIES INTERCURRENTES", 14)
            .AddTexte("-------------------------------------------------------------------------------------------------------------------")
        End With
    End Sub

    Private Function PrintOrdonnanceDetail(SelectionALD As Boolean) As RadDocument
        Dim document As New RadDocument()
        If PatientIsAld = False AndAlso SelectionALD = True Then
            Return document
        End If

        Const LargeurCol1 As Integer = 475
        Const LargeurCol2 As Integer = 70
        Const LargeurCol3 As Integer = 85


        Dim section As New Section()
        Dim table As New Table With {
            .LayoutMode = TableLayoutMode.Fixed,
            .StyleName = RadDocumentDefaultStyles.DefaultTableGridStyleName
        }

        Dim dt As DataTable
        dt = ordonnanceDetailDao.getAllOrdonnanceLigneSelectAldByOrdonnanceId(SelectedOrdonnanceId, SelectionALD)

        Dim i As Integer
        Dim rowCount As Integer = dt.Rows.Count - 1

        If dt.Rows.Count > 0 Then
            Dim row0 As New TableRow()

            Dim celleTitre1 As New TableCell With {
                .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
            }
            Dim spanTitre1 As New Span With {
                .FontSize = 10,
                .FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold
            }
            Dim paragrapheTitre1 As New Paragraph()
            spanTitre1.Text = "Spécialité"
            paragrapheTitre1.Inlines.Add(spanTitre1)
            celleTitre1.Blocks.Add(paragrapheTitre1)
            row0.Cells.Add(celleTitre1)

            Dim cellTitre2 As New TableCell With {
                .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol2)
            }
            Dim spanTitre2 As New Span With {
                .FontSize = 10,
                .FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Normal
            }
            Dim paragrapheTitre2 As New Paragraph()
            spanTitre2.Text = "Durée"
            paragrapheTitre2.TextAlignment = RadTextAlignment.Center
            paragrapheTitre2.Inlines.Add(spanTitre2)
            cellTitre2.Blocks.Add(paragrapheTitre2)
            row0.Cells.Add(cellTitre2)

            Dim cellTitre3 As New TableCell With {
                .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol3)
            }
            Dim spanTitre3 As New Span With {
                .FontSize = 10
            }
            spanTitre2.FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Normal
            Dim paragrapheTitre3 As New Paragraph()
            spanTitre3.Text = "Délivrance"
            paragrapheTitre3.Inlines.Add(spanTitre3)
            cellTitre3.Blocks.Add(paragrapheTitre3)
            row0.Cells.Add(cellTitre3)

            table.Rows.Add(row0)

            For i = 0 To rowCount Step 1
                Dim traitementALD As Boolean = Coalesce(dt.Rows(i)("oa_traitement_ald"), False)
                If SelectionALD = True Then
                    If traitementALD = False Then
                        Continue For
                    End If
                Else
                    If traitementALD = True Then
                        Continue For
                    End If
                End If

                If SelectionALD = True AndAlso TraitementAldExiste = False Then
                    TraitementAldExiste = True
                End If

                Dim Posologie As String = dt.Rows(i)("oa_traitement_posologie")
                Dim traitementId As Long = Coalesce(dt.Rows(i)("oa_traitement_id"), 0)

                Dim traitement As New Traitement
                If traitementId <> 0 Then
                    traitement = traitementDao.GetTraitementById(traitementId)
                End If

                Dim duree As Integer = Coalesce(dt.Rows(i)("oa_traitement_duree"), 0)
                Dim MedicamentAld As Boolean = Coalesce(dt.Rows(i)("oa_traitement_ald"), False)
                Dim MedicamentADelivrer As Boolean = Coalesce(dt.Rows(i)("oa_traitement_a_delivrer"), False)

                Dim row As New TableRow()

                Dim cellDetail1 As New TableCell With {
                    .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
                }
                Dim spanDetail11 As New Span With {
                    .FontSize = 10,
                    .FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold
                }
                Dim paragrapheDetail1 As New Paragraph()

                If traitementId <> 0 Then
                    spanDetail11.Text = traitement.DenominationLongue
                Else
                    spanDetail11.Text = Coalesce(dt.Rows(i)("oa_traitement_posologie_commentaire"), "")
                End If

                If spanDetail11.Text <> "" Then
                    paragrapheDetail1.Inlines.Add(spanDetail11)
                End If

                Dim spanDetail12 As New Span With {
                    .FontSize = 10,
                    .FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Normal
                }

                Dim PosologieBase As String = ""
                If Coalesce(dt.Rows(i)("oa_traitement_posologie_base"), "") = "J" Then
                    PosologieBase = " / jour"
                End If

                If traitementId <> 0 Then
                    spanDetail12.Text = vbCrLf & "Posologie " & Posologie & PosologieBase & Coalesce(dt.Rows(i)("oa_traitement_posologie_commentaire"), "")
                    If spanDetail12.Text <> "" Then
                        paragrapheDetail1.Inlines.Add(spanDetail12)
                    End If
                End If

                cellDetail1.Blocks.Add(paragrapheDetail1)
                row.Cells.Add(cellDetail1)

                Dim cellDetail2 As New TableCell With {
                    .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol2)
                }
                Dim spanDetail2 As New Span With {
                    .FontSize = 10
                }
                Dim paragrapheDetail2 As New Paragraph With {
                    .TextAlignment = TextAlignment.Center
                }
                spanDetail2.Text = duree & " jour(s)"
                If spanDetail2.Text <> "" Then
                    paragrapheDetail2.Inlines.Add(spanDetail2)
                End If
                cellDetail2.Blocks.Add(paragrapheDetail2)
                row.Cells.Add(cellDetail2)

                Dim Delivrance As String = ""
                If traitementId <> 0 Then
                    If MedicamentADelivrer = False Then
                        Delivrance = "Ne pas delivrer"
                    End If
                End If

                Dim cellDetail3 As New TableCell With {
                    .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol3)
                }
                Dim spanDetail3 As New Span With {
                    .FontSize = 9
                }
                Dim paragrapheDetail3 As New Paragraph()
                spanDetail3.Text = Delivrance
                If spanDetail3.Text <> "" Then
                    paragrapheDetail3.Inlines.Add(spanDetail3)
                End If
                cellDetail3.Blocks.Add(paragrapheDetail3)
                row.Cells.Add(cellDetail3)

                table.Rows.Add(row)
            Next

            section.Blocks.Add(table)
            section.Blocks.Add(New Paragraph())
            document.Sections.Add(section)
        End If

        Return document
    End Function

    Private Sub PrintBasPage(section As Section)
        Dim ordonnance As Ordonnance = ordonnanceDao.GetOrdonnaceById(SelectedOrdonnanceId)
        Dim QG As QRCodeGenerator = New QRCoder.QRCodeGenerator()
        Dim Data As QRCodeData = QG.CreateQrCode(SIGN_URL & Base64UrlEncoder.Encode(ordonnance.Signature.HexToByteArray()), QRCodeGenerator.ECCLevel.L)
        Dim my_qrCode = New QRCode(Data)
        With EditTools
            .CreateParagraphIntoSection(section,, RadTextAlignment.Right)
            Dim Medecin As Utilisateur = userDao.getUserById(ordonnance.UserValidation)
            Dim profil As Profil = profilDao.getProfilById(Medecin.UtilisateurProfilId)
            .AddTexteLine(Medecin.UtilisateurPrenom & " " & Medecin.UtilisateurNom & ", " & profil.Designation)
            .AddTexteLine("RPPS : " & Medecin.UtilisateurRPPS)
            .AddImage(New WriteableBitmap(my_qrCode.GetGraphic(3)), New Size(150, 150))
        End With
        With EditTools
            .CreateParagraphIntoSection(section,, RadTextAlignment.Left)
            .AddTexteLine(If(ordonnance.Renouvellement > 0, "A renouveller " & ordonnance.Renouvellement & " fois", "Non renouvelable"))
        End With
    End Sub
End Class
