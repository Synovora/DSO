Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne par PatientParametreLdvDao.BuildBean. Généré à partir du code : la liste des
''' colonnes et les valeurs de repli sont celles de la source, pas une reprise à la main.
''' </summary>
<TestClass()> Public Class TestPatientParametreLdvDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "patient_id", "activite_pathologie_aigue", "activite_prevention_autre",
        "activite_prevention_enfant_pre_scolaire", "activite_prevention_enfant_scolaire",
        "activite_suivi_grossesse", "activite_suivi_gynecologique", "activite_social",
        "activite_suivi_chronique", "type_consultation", "type_virtuel", "type_parametre",
        "profil_medical", "profil_paramedical", "profil_patient", "parametre1", "parametre2",
        "parametre3", "parametre4", "parametre5", "user_modification", "date_modification"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim b = PatientParametreLdvDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"patient_id", 101L},
            {"activite_pathologie_aigue", True},
            {"activite_prevention_autre", True},
            {"activite_prevention_enfant_pre_scolaire", True},
            {"activite_prevention_enfant_scolaire", True},
            {"activite_suivi_grossesse", True},
            {"activite_suivi_gynecologique", True},
            {"activite_social", True},
            {"activite_suivi_chronique", True},
            {"type_consultation", True},
            {"type_virtuel", True},
            {"type_parametre", True},
            {"profil_medical", True},
            {"profil_paramedical", True},
            {"profil_patient", True},
            {"parametre1", 116L},
            {"parametre2", 117L},
            {"parametre3", 118L},
            {"parametre4", 119L},
            {"parametre5", 120L},
            {"user_modification", 121L},
            {"date_modification", New Date(2024, 11, 23)}}))

        Assert.AreEqual(101L, b.PatientId)
        Assert.AreEqual(True, b.ActivitePathologieAigue)
        Assert.AreEqual(True, b.ActivitePreventionAutre)
        Assert.AreEqual(True, b.ActivitePreventionEnfantPreScolaire)
        Assert.AreEqual(True, b.ActivitePreventionEnfantScolaire)
        Assert.AreEqual(True, b.ActiviteSuiviGrossesse)
        Assert.AreEqual(True, b.ActiviteSuiviGynecologique)
        Assert.AreEqual(True, b.ActiviteSocial)
        Assert.AreEqual(True, b.ActiviteSuiviChronique)
        Assert.AreEqual(True, b.TypeConsultation)
        Assert.AreEqual(True, b.TypeVirtuel)
        Assert.AreEqual(True, b.TypeParametre)
        Assert.AreEqual(True, b.ProfilMedical)
        Assert.AreEqual(True, b.ProfilParamedical)
        Assert.AreEqual(True, b.ProfilPatient)
        Assert.AreEqual(116L, b.Parametre1)
        Assert.AreEqual(117L, b.Parametre2)
        Assert.AreEqual(118L, b.Parametre3)
        Assert.AreEqual(119L, b.Parametre4)
        Assert.AreEqual(120L, b.Parametre5)
        Assert.AreEqual(121L, b.UserModification)
        Assert.AreEqual(New Date(2024, 11, 23), b.DateModification)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim b = PatientParametreLdvDao.BuildBean(LigneDeTest.Ligne(Colonnes, Nothing))

        Assert.AreEqual(0L, b.PatientId)
        Assert.AreEqual(False, b.ActivitePathologieAigue)
        Assert.AreEqual(False, b.ActivitePreventionAutre)
        Assert.AreEqual(False, b.ActivitePreventionEnfantPreScolaire)
        Assert.AreEqual(False, b.ActivitePreventionEnfantScolaire)
        Assert.AreEqual(False, b.ActiviteSuiviGrossesse)
        Assert.AreEqual(False, b.ActiviteSuiviGynecologique)
        Assert.AreEqual(False, b.ActiviteSocial)
        Assert.AreEqual(False, b.ActiviteSuiviChronique)
        Assert.AreEqual(False, b.TypeConsultation)
        Assert.AreEqual(False, b.TypeVirtuel)
        Assert.AreEqual(False, b.TypeParametre)
        Assert.AreEqual(False, b.ProfilMedical)
        Assert.AreEqual(False, b.ProfilParamedical)
        Assert.AreEqual(False, b.ProfilPatient)
        Assert.AreEqual(0L, b.Parametre1)
        Assert.AreEqual(0L, b.Parametre2)
        Assert.AreEqual(0L, b.Parametre3)
        Assert.AreEqual(0L, b.Parametre4)
        Assert.AreEqual(0L, b.Parametre5)
        Assert.AreEqual(0L, b.UserModification)
        Assert.AreEqual(Date.MinValue, b.DateModification)
    End Sub

End Class
