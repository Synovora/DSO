Imports Telerik.WinForms.Documents.Model
Imports Telerik.WinForms.Documents.Layout
Imports Telerik.WinControls.RichTextEditor.UI
Imports Oasis_Common

Public Class PrtOrdonnance
    Public Property SelectedPatient As Patient
    Public Property SelectedOrdonnanceId As Long

    ReadOnly EditTools As New OasisTextTools

    ReadOnly ordonnanceDao As New OrdonnanceDao
    ReadOnly ordonnanceDetailDao As New OrdonnanceDetailDao
    ReadOnly episodeParametreDao As New EpisodeParametreDao
    ReadOnly userDao As New UserDao
    ReadOnly profilDao As New ProfilDao
    ReadOnly theriaqueDao As New TheriaqueDao

    Dim ordonnance As Ordonnance

    Public Sub PrintDocument()
        ordonnance = ordonnanceDao.getOrdonnaceById(SelectedOrdonnanceId)

        With EditTools
            Dim section = .CreateSection()
            Dim document = .AddSectionIntoDocument(Nothing, section)

            PrintEntete(section)
            PrintEtatCivil(section)
            PrintOrdonnanceDetail(section)
            PrintBasPage(section)

            ' --- Insertion du fragment generé
            .insertFragmentToEditor(document)
            .printPreview()
        End With
    End Sub

    Private Sub PrintEntete(section As Section)

        With EditTools
            .CreateParagraphIntoSection(section, 15, RadTextAlignment.Center)
            .AddTexte("Ordonnance", 16, FontWeights.Bold)
            .AddNewLigne()
            .AddTexteLine("Service Oasis Santé", 14)
            .AddTexteLine("Tel : xxxxxxxxxx   Fax : xxxxxxxxxx")
            .AddTexteLine("Mail : xxxxxxxxxx@xxxxxxx.xx")
        End With
    End Sub

    Private Sub PrintEtatCivil(section As Section)
        With EditTools
            .CreateParagraphIntoSection(section,, RadTextAlignment.Left)
            .AddTexteLine(SelectedPatient.PatientNom & " " & SelectedPatient.PatientPrenom)

            Dim DateNaissancePatient As Date = SelectedPatient.PatientDateNaissance
            .AddTexteLine(DateNaissancePatient.ToString("dd.MM.yyyy"))
            .AddTexteLine(SelectedPatient.PatientNir)
            .AddNewLigne()

            Dim Poids As Double = episodeParametreDao.GetPoidsByEpisodeIdOrLastKnow(0, SelectedPatient.patientId)
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

    Private Sub PrintOrdonnanceDetail(section As Section)
        EditTools.CreateParagraphIntoSection(section,, RadTextAlignment.Left)
        Dim dt As DataTable
        dt = ordonnanceDetailDao.getAllOrdonnanceLigneByOrdonnanceId(SelectedOrdonnanceId)

        Dim i As Integer
        Dim rowCount As Integer = dt.Rows.Count - 1

        For i = 0 To rowCount Step 1
            Dim Posologie As String = dt.Rows(i)("oa_traitement_posologie")
            Dim specialite As SpecialiteTheriaque = theriaqueDao.GetSpecialiteById(dt.Rows(i)("oa_traitement_medicament_cis"))
            Dim duree As Integer = Coalesce(dt.Rows(i)("oa_traitement_duree"), 0)
            Dim MedicamentAld As Boolean = Coalesce(dt.Rows(i)("oa_traitement_ald"), False)
            Dim MedicamentADelivrer As Boolean = Coalesce(dt.Rows(i)("oa_traitement_a_delivrer"), False)
            With EditTools
                .AddTexteLine(specialite.DciLongue & " " & duree)
                .AddTexteLine(Posologie)
                If MedicamentADelivrer = False Then
                    .AddTexteLine("Ne pas delivrer")
                End If
            End With
        Next
    End Sub

    Private Sub PrintBasPage(section As Section)
        With EditTools
            .CreateParagraphIntoSection(section,, RadTextAlignment.Right)

            Dim Medecin As Utilisateur = userDao.getUserById(ordonnance.UserValidation)
            Dim profil As Profil = profilDao.getProfilById(Medecin.UtilisateurProfilId)
            .AddTexteLine(Medecin.UtilisateurPrenom & " " & Medecin.UtilisateurNom & ", " & profil.Designation)
            .AddTexteLine("RPPS : " & Medecin.UtilisateurRPPS)
        End With
    End Sub
End Class
