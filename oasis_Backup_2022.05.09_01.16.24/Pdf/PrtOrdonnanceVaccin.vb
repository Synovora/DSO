Imports Telerik.WinForms.Documents.Model
Imports Telerik.WinForms.Documents.Layout
Imports Telerik.WinControls.RichTextEditor.UI
Imports Oasis_Common
Imports Microsoft.IdentityModel.Tokens
Imports Nethereum.Hex.HexConvertors.Extensions
Imports QRCoder

Public Class PrtOrdonnanceVaccin

    Public Property SelectedPatient As Patient
    Public Property SelectedUserValidation As Utilisateur
    Property Vaccins As List(Of VaccinValence)

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
    ReadOnly SIGN_URL As String = "https://ns3119889.ip-51-38-181.eu/Sign/Check/"

    Dim ordonnance As Ordonnance

    Dim PatientIsAld As Boolean = False
    Dim TraitementAldExiste As Boolean = False

    Public Sub PrintDocument()
        Try
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
            GenereDocument()
            Return EditTools.exportToPdf()
        Finally
            EditTools.Dispose()
        End Try
    End Function

    Private Sub GenereDocument()
        If aldDao.IsPatientALD(SelectedPatient.PatientId) Then
            PatientIsAld = True
        End If

        Dim section = EditTools.CreateSection()
        Dim document = EditTools.AddSectionIntoDocument(Nothing, section)

        PrintEtatCivil(section)
        EditTools.InsertFragmentToEditor(document)
        EditTools.InsertFragmentToEditor(PrintOrdonnanceDetail())

        Dim sectionFin = EditTools.CreateSection()
        Dim documentFin = EditTools.AddSectionIntoDocument(Nothing, sectionFin)
        PrintBasPage(sectionFin)
        EditTools.InsertFragmentToEditor(documentFin)
        EditTools.AddHeader(SelectedPatient, "Ordonnance Vaccin")
        EditTools.AddPageNumber()
    End Sub


    Private Sub PrintEtatCivil(section As Section)
        With EditTools
            .CreateParagraphIntoSection(section,, RadTextAlignment.Left)
            Dim Poids As Double = episodeParametreDao.GetPoidsByEpisodeIdOrLastKnow(0, SelectedPatient.PatientId)
            If Poids > 0 Then
                .AddTexteLine("Poids : " & Poids & " Kg")
            End If
            .AddTexte("Age : " & SelectedPatient.PatientAge)

            .CreateParagraphIntoSection(section,, RadTextAlignment.Right)
            Dim SiteDescription As String = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
            .AddTexteLine(SiteDescription & ", le " & Date.Now().AddDays(30).ToString("dd.MM.yyyy"))
        End With
    End Sub

    Private Function PrintOrdonnanceDetail() As RadDocument
        Try
            Dim document As New RadDocument()

            Const LargeurCol1 As Integer = 680
            Const LargeurCol2 As Integer = 70
            Const LargeurCol3 As Integer = 85

            Dim section As New Section()
            Dim table As New Table With {
                .LayoutMode = TableLayoutMode.Fixed,
                .StyleName = RadDocumentDefaultStyles.DefaultTableGridStyleName
            }

            For Each vaccin As VaccinValence In Vaccins
                Dim row As New TableRow()

                Dim spanDetail11 As New Span With {
                    .FontSize = 11,
                    .FontWeight = Telerik.WinControls.RichTextEditor.UI.FontWeights.Bold,
                    .Text = vaccin.Dci
                }

                Dim cellDetail1 As New TableCell With {
                .PreferredWidth = New TableWidthUnit(TableWidthUnitType.Fixed, LargeurCol1)
                }
                Dim paragrapheDetail1 As New Paragraph()
                paragrapheDetail1.Inlines.Add(spanDetail11)
                cellDetail1.Blocks.Add(paragrapheDetail1)
                row.Cells.Add(cellDetail1)

                table.Rows.Add(row)
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
            Dim QG As QRCodeGenerator = New QRCodeGenerator()
            'Dim Data As QRCodeData = QG.CreateQrCode(SIGN_URL & Base64UrlEncoder.Encode(ordonnance.Signature.HexToByteArray()), QRCodeGenerator.ECCLevel.L)
            'Dim my_qrCode = New QRCode(Data)
            With EditTools
                .CreateParagraphIntoSection(section,, RadTextAlignment.Right)
                Dim profil As Profil = profilDao.getProfilById(SelectedUserValidation.UtilisateurProfilId)
                .AddTexteLine(SelectedUserValidation.UtilisateurPrenom & " " & SelectedUserValidation.UtilisateurNom & ", " & profil.Designation)
                .AddTexteLine("RPPS : " & SelectedUserValidation.UtilisateurRPPS)
                '.AddImage(New WriteableBitmap(my_qrCode.GetGraphic(3)), New Size(150, 150))
            End With
            With EditTools
                .CreateParagraphIntoSection(section,, RadTextAlignment.Left)
                .AddTexteLine("Non renouvelable")
            End With
        Catch ex As Exception
            MsgBox(ex.Message())
        End Try
    End Sub
End Class
