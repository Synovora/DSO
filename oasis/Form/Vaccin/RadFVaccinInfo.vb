Imports Oasis_Common

Public Class RadFVaccinInfo
    'Property SelectedPatient As Patient

    'Private Sub ChargementEtatCivil()
    '    LblPatientNIR.Text = SelectedPatient.PatientNir
    '    LblPatientPrenom.Text = SelectedPatient.PatientPrenom
    '    LblPatientNom.Text = SelectedPatient.PatientNom
    '    LblPatientAge.Text = SelectedPatient.PatientAge
    '    LblDateNaissance.Text = SelectedPatient.PatientDateNaissance.ToString("dd.MM.yyyy")
    '    LblPatientGenre.Text = SelectedPatient.PatientGenre
    '    LblPatientSite.Text = Environnement.Table_site.GetSiteDescription(SelectedPatient.PatientSiteId)
    '    LblPatientDateMaj.Text = FormatageDateAffichage(SelectedPatient.PatientSyntheseDateMaj, True)

    '    Dim DateMaxValue = New Date(9998, 12, 31, 0, 0, 0)
    '    Dim DateMinValue = New Date(1, 1, 1, 0, 0, 0)
    '    If SelectedPatient.PatientDateEntree = DateMaxValue OrElse SelectedPatient.PatientDateEntree = DateMinValue OrElse SelectedPatient.PatientDateSortie < Date.Now Then
    '        LblNonOasis.Show()
    '    Else
    '        LblNonOasis.Hide()
    '    End If


    '    'Vérification de l'existence d'ALD
    '    LblALD.Hide()
    '    ChargementToolTipAld()

    '    'Contre-indication
    '    GetContreIndication()

    '    'Allergie
    '    GetAllergie()

    'End Sub

    'Private Sub ChargementToolTipAld()
    '    Dim StringTooltip As String
    '    Dim aldDao As New AldDao

    '    StringTooltip = aldDao.DateFinALD(Me.SelectedPatient.PatientId)
    '    If StringTooltip <> "" Then
    '        LblALD.Show()
    '        ToolTip.SetToolTip(LblALD, StringTooltip)
    '    End If
    'End Sub

    'Private Sub GetAllergie()
    '    Dim patientDao As New PatientDao
    '    Dim StringAllergieToolTip As String = patientDao.GetStringAllergieByPatient(SelectedPatient.PatientId)
    '    If StringAllergieToolTip = "" Then
    '        PatientAllergie = False
    '        LblAllergie.Hide()
    '        LblSubstance.Hide()
    '        ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Enabled = False
    '    Else
    '        PatientAllergie = True
    '        LblAllergie.Show()
    '        ToolTip.SetToolTip(LblAllergie, StringAllergieToolTip)
    '        LblSubstance.Show()
    '        LblSubstance.Text = StringAllergieToolTip.Replace(vbCrLf, " ")
    '        ListeDesMédicamentsDéclarésAllergiquesToolStripMenuItem.Enabled = True
    '    End If
    'End Sub

    'Private Sub GetContreIndication()
    '    Dim patientDao As New PatientDao
    '    Dim StringContreIndicationToolTip As String = patientDao.GetStringContreIndicationByPatient(SelectedPatient.PatientId)
    '    If StringContreIndicationToolTip = "" Then
    '        LblContreIndication.Hide()
    '        PatientContreIndication = False
    '        ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem.Enabled = False
    '    Else
    '        LblContreIndication.Show()
    '        ToolTip.SetToolTip(LblContreIndication, StringContreIndicationToolTip)
    '        PatientContreIndication = True
    '        ListeDesMédicamentsDéclarésContreindiquésToolStripMenuItem.Enabled = True
    '    End If
    'End Sub
End Class
