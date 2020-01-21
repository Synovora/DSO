﻿Imports System.Collections.Specialized

Public Class RadFOrdonnanceDetail
    Private _SelectedPatient As Patient
    Private _SelectedEpisode As Episode
    Private _SelectedOrdonnanceId As Integer
    Private _SelectedOrdonnanceLigneId As Integer
    Private _Ald As Boolean
    Private _Allergie As Boolean
    Private _ContreIndication As Boolean
    Private _CodeRetour As Boolean

    Public Property SelectedPatient As Patient
        Get
            Return _SelectedPatient
        End Get
        Set(value As Patient)
            _SelectedPatient = value
        End Set
    End Property

    Public Property SelectedEpisode As Episode
        Get
            Return _SelectedEpisode
        End Get
        Set(value As Episode)
            _SelectedEpisode = value
        End Set
    End Property

    Public Property SelectedOrdonnanceId As Integer
        Get
            Return _SelectedOrdonnanceId
        End Get
        Set(value As Integer)
            _SelectedOrdonnanceId = value
        End Set
    End Property

    Public Property CodeRetour As Boolean
        Get
            Return _CodeRetour
        End Get
        Set(value As Boolean)
            _CodeRetour = value
        End Set
    End Property

    Public Property SelectedOrdonnanceLigneId As Integer
        Get
            Return _SelectedOrdonnanceLigneId
        End Get
        Set(value As Integer)
            _SelectedOrdonnanceLigneId = value
        End Set
    End Property

    Public Property Allergie As Boolean
        Get
            Return _Allergie
        End Get
        Set(value As Boolean)
            _Allergie = value
        End Set
    End Property

    Public Property ContreIndication As Boolean
        Get
            Return _ContreIndication
        End Get
        Set(value As Boolean)
            _ContreIndication = value
        End Set
    End Property

    Public Property Ald As Boolean
        Get
            Return _Ald
        End Get
        Set(value As Boolean)
            _Ald = value
        End Set
    End Property

    Enum EnumEditMode
        Modification = 1
        Creation = 2
    End Enum

    Dim ordonnanceDetailDao As New OrdonnanceDetailDao
    Dim ordonnanceDetail As OrdonnanceDetail

    Dim EditMode As Integer

    Private Sub RadFOrdonnanceDetail_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        ChargementEtatCivil()
        ChargementDetail()
    End Sub

    Private Sub ChargementDetail()
        If SelectedOrdonnanceLigneId <> 0 Then
            EditMode = EnumEditMode.Modification
            ordonnanceDetail = ordonnanceDetailDao.getOrdonnanceLigneById(SelectedOrdonnanceLigneId)
            NumDuree.Value = ordonnanceDetail.Duree
            TxtCommentaire.Text = ordonnanceDetail.PosologieCommentaire
        Else
            EditMode = EnumEditMode.Creation
            ordonnanceDetail = New OrdonnanceDetail
            ordonnanceDetail.OrdonnanceId = SelectedOrdonnanceId
            ordonnanceDetail.TraitementId = 0
            ordonnanceDetail.Traitement = False
            ordonnanceDetail.OrdreAffichage = 1000
            ordonnanceDetail.Ald = Ald
            ordonnanceDetail.ADelivrer = False
            ordonnanceDetail.MedicamentCis = 0
            ordonnanceDetail.MedicamentDci = ""
            ordonnanceDetail.DateDebut = Date.MaxValue
            ordonnanceDetail.DateFin = Date.MaxValue
            ordonnanceDetail.Duree = 0
            ordonnanceDetail.PosologieBase = ""
            ordonnanceDetail.PosologieRythme = 0
            ordonnanceDetail.PosologieMatin = 0
            ordonnanceDetail.PosologieMidi = 0
            ordonnanceDetail.PosologieApresMidi = 0
            ordonnanceDetail.PosologieSoir = 0
            ordonnanceDetail.FractionMatin = ""
            ordonnanceDetail.FractionMidi = ""
            ordonnanceDetail.FractionApresMidi = ""
            ordonnanceDetail.FractionSoir = ""
            ordonnanceDetail.PosologieCommentaire = ""
            ordonnanceDetail.Commentaire = ""
            ordonnanceDetail.Fenetre = False
            ordonnanceDetail.FenetreDateDebut = Date.MaxValue
            ordonnanceDetail.FenetreDateFin = Date.MaxValue
            ordonnanceDetail.Inactif = False
        End If
        If ordonnanceDetail.TraitementId = 0 Then
            LblLabelDuree.Hide()
            NumDuree.Hide()
        End If
    End Sub

    Private Sub ChargementEtatCivil()
        LblPatientNIR.Text = SelectedPatient.PatientNir
        LblPatientPrenom.Text = SelectedPatient.PatientPrenom
        LblPatientNom.Text = SelectedPatient.PatientNom
        LblPatientAge.Text = SelectedPatient.PatientAge
        LblPatientGenre.Text = SelectedPatient.PatientGenre
        LblPatientAdresse1.Text = SelectedPatient.PatientAdresse1
        LblPatientAdresse2.Text = SelectedPatient.PatientAdresse2
        LblPatientCodePostal.Text = SelectedPatient.PatientCodePostal
        LblPatientVille.Text = SelectedPatient.PatientVille
        LblPatientTel1.Text = SelectedPatient.PatientTel1
        LblPatientTel2.Text = SelectedPatient.PatientTel2
        LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
        LblPatientUniteSanitaire.Text = Environnement.Table_unite_sanitaire.GetUniteSanitaireDescription(SelectedPatient.PatientUniteSanitaireId)
        LblPatientDateMaj.Text = SelectedPatient.PatientSyntheseDateMaj.ToString("dd/MM/yyyy")
        If SelectedPatient.PharmacienId <> 0 Then
            Dim rordao As New RorDao
            Dim ror As Ror
            ror = rordao.getRorById(SelectedPatient.PharmacienId)
            LblPharmacienNom.Text = ror.Nom & " " & ror.Ville
        Else
            LblPharmacienNom.Text = "Pas de pharmacie référencée pour ce patient"
        End If

        'Vérification de l'existence d'ALD
        LblALD.Hide()
        Dim StringTooltip As String
        Dim aldDao As New AldDao
        StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.patientId)
        If StringTooltip <> "" Then
            LblALD.Show()
            ToolTip.SetToolTip(LblALD, StringTooltip)
        End If
        ChargementAllergie()
    End Sub

    Private Sub ChargementAllergie()
        'Allergie
        If Allergie = True Then
            LblAllergie.Show()
        Else
            LblAllergie.Hide()
        End If

        'Contre-indication
        If ContreIndication = True Then
            lblContreIndication.Show()
        Else
            lblContreIndication.Hide()
        End If

        'Si allergie, affichage des substances allergiques
        If Allergie = True Then
            LblAllergie.Visible = True
            Dim premierPassage As Boolean = True
            Dim LongueurChaine, LongueurSub As Integer
            Dim AllergieTooltip As String
            Dim LongueurMax As Integer = 10

            'Chargement du TextBox
            Dim allergieString As String
            Dim SubstancesAllergiques As New StringCollection()
            SubstancesAllergiques = MedocDao.ListeSubstancesAllergiques(SelectedPatient.PatientAllergieCis)
            Dim allergieEnumerator As StringEnumerator = SubstancesAllergiques.GetEnumerator()
            While allergieEnumerator.MoveNext()
                If premierPassage = True Then
                    allergieString = allergieEnumerator.Current.ToString
                    LongueurChaine = allergieString.Length
                    If LongueurChaine < LongueurMax Then
                        LongueurSub = LongueurChaine - 1
                    Else
                        LongueurSub = LongueurMax
                    End If
                    AllergieTooltip = allergieString
                    premierPassage = False
                Else
                    allergieString = allergieEnumerator.Current.ToString
                    LongueurChaine = allergieString.Length
                    If LongueurChaine < LongueurMax Then
                        LongueurSub = LongueurChaine - 1
                    Else
                        LongueurSub = LongueurMax
                    End If
                    AllergieTooltip = AllergieTooltip + vbCrLf + allergieString
                End If
            End While
            ToolTip.SetToolTip(LblAllergie, AllergieTooltip)
            'Chargement des médicaments génériques associés aux médicaments allergiques déclarés
            TraitementAllergies(Me.SelectedPatient)
        End If

        If ContreIndication = True Then
            lblContreIndication.Show()
            'Chargement des médicaments génériques associés aux médicaments contre-indiqués déclarés
            Dim premierPassage As Boolean = True
            Dim LongueurChaine, LongueurSub As Integer
            Dim CITooltip As String
            Dim LongueurMax As Integer = 10

            'Chargement du TextBox
            Dim CIString As String
            Dim SubstancesCI As New StringCollection()
            SubstancesCI = MedocDao.ListeSubstancesCI(SelectedPatient.PatientContreIndicationCis)
            Dim CIEnumerator As StringEnumerator = SubstancesCI.GetEnumerator()
            While CIEnumerator.MoveNext()
                If premierPassage = True Then
                    CIString = CIEnumerator.Current.ToString
                    LongueurChaine = CIString.Length
                    If LongueurChaine < LongueurMax Then
                        LongueurSub = LongueurChaine - 1
                    Else
                        LongueurSub = LongueurMax
                    End If
                    CITooltip = CIString
                    premierPassage = False
                Else
                    CIString = CIEnumerator.Current.ToString
                    LongueurChaine = CIString.Length
                    If LongueurChaine < LongueurMax Then
                        LongueurSub = LongueurChaine - 1
                    Else
                        LongueurSub = LongueurMax
                    End If
                    CITooltip = CITooltip + vbCrLf + CIString
                End If
            End While
            ToolTip.SetToolTip(lblContreIndication, CITooltip)
        End If
    End Sub

    Private Sub RadBtnAbandon_Click(sender As Object, e As EventArgs) Handles RadBtnAbandon.Click
        Close()
    End Sub

    Private Sub RadBtnValidation_Click(sender As Object, e As EventArgs) Handles RadBtnValidation.Click
        Dim CodeRetour As Boolean = True
        If ordonnanceDetail.TraitementId = 0 Then
            If TxtCommentaire.Text = "" Then
                CodeRetour = False
                MessageBox.Show("Saisie commentaire obligatoire")
            End If
        Else
            If NumDuree.Value = 0 Then
                CodeRetour = False
                MessageBox.Show("Saisie durée obligatoire")
            End If
        End If
        If CodeRetour = True Then
            Select Case EditMode
                Case EnumEditMode.Modification
                    ordonnanceDetailDao.ModificationOrdonnanceDetail(SelectedOrdonnanceLigneId, TxtCommentaire.Text, NumDuree.Value)
                Case EnumEditMode.Creation
                    ordonnanceDetail.PosologieCommentaire = TxtCommentaire.Text
                    ordonnanceDetailDao.CreationOrdonnanceDetail(ordonnanceDetail)
            End Select
        End If
    End Sub
End Class
