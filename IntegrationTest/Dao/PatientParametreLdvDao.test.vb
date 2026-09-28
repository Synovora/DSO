Imports Oasis_Common

''' <summary>
''' PatientParametreLdvDao contre la base de test. La ligne de vie du client lourd
''' (RadFEpisodeLigneDeVie) lit, crée et met à jour la configuration d'affichage du
''' patient : tout tourne sous oasis_client.
''' </summary>
<TestClass()> Public Class PatientParametreLdvDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New PatientParametreLdvDao

    Private Shared Function Auteur(utilisateurId As Long) As Utilisateur
        Return New Utilisateur With {.UtilisateurId = CInt(utilisateurId)}
    End Function

    ''' <summary>Configuration où chaque case a une valeur distincte de sa voisine, pour repérer une colonne décalée.</summary>
    Private Shared Function ConfigurationLdv(patientId As Long, parametres() As Long,
                                          Optional profilParamedical As Boolean = False,
                                          Optional profilPatient As Boolean = True) As PatientParametreLdv
        Return New PatientParametreLdv With {
            .PatientId = patientId,
            .TypeConsultation = True,
            .TypeVirtuel = False,
            .TypeParametre = True,
            .ActivitePathologieAigue = True,
            .ActiviteSuiviChronique = False,
            .ActivitePreventionAutre = True,
            .ActivitePreventionEnfantPreScolaire = False,
            .ActivitePreventionEnfantScolaire = True,
            .ActiviteSuiviGrossesse = False,
            .ActiviteSuiviGynecologique = True,
            .ActiviteSocial = False,
            .ProfilMedical = True,
            .ProfilParamedical = profilParamedical,
            .ProfilPatient = profilPatient,
            .Parametre1 = parametres(0),
            .Parametre2 = parametres(1),
            .Parametre3 = parametres(2),
            .Parametre4 = parametres(3),
            .Parametre5 = parametres(4)
        }
    End Function

    Private Shared Function CinqParametres() As Long()
        Return Enumerable.Range(1, 5).Select(Function(i) CreerParametreDeMesure("Parametre LDV " & i, ordre:=i)).ToArray()
    End Function

    <TestMethod()> Public Sub GetParametreByPatientId_SansConfiguration_LeveUneErreur()
        Dim idPatient = CreerPatient()

        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetParametreByPatientId(CInt(idPatient)))

        StringAssert.Contains(erreur.Message, "inexistant")
    End Sub

    <TestMethod()> Public Sub CreateConfigurationParametre_PuisLecture_RelitChaqueColonne()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim parametres = CinqParametres()

        Assert.IsTrue(dao.CreateConfigurationParametre(ConfigurationLdv(idPatient, parametres), Auteur(idUtilisateur)))

        Dim lue = dao.GetParametreByPatientId(CInt(idPatient))
        Assert.AreEqual(idPatient, lue.PatientId)
        Assert.IsTrue(lue.TypeConsultation)
        Assert.IsFalse(lue.TypeVirtuel)
        Assert.IsTrue(lue.TypeParametre)
        Assert.IsTrue(lue.ActivitePathologieAigue)
        Assert.IsFalse(lue.ActiviteSuiviChronique)
        Assert.IsTrue(lue.ActivitePreventionAutre)
        Assert.IsFalse(lue.ActivitePreventionEnfantPreScolaire)
        Assert.IsTrue(lue.ActivitePreventionEnfantScolaire)
        Assert.IsFalse(lue.ActiviteSuiviGrossesse)
        Assert.IsTrue(lue.ActiviteSuiviGynecologique)
        Assert.IsFalse(lue.ActiviteSocial)
        Assert.IsTrue(lue.ProfilMedical)
        Assert.IsFalse(lue.ProfilParamedical)
        CollectionAssert.AreEqual(parametres,
            New Long() {lue.Parametre1, lue.Parametre2, lue.Parametre3, lue.Parametre4, lue.Parametre5})
        Assert.AreEqual(idUtilisateur, lue.UserModification)
        Assert.AreEqual(Date.Today, lue.DateModification.Date)
    End Sub

    <TestMethod()> Public Sub CreateConfigurationParametre_ProfilPatient_RecoitLaValeurDuProfilParamedical()
        Dim idPatient = CreerPatient()
        Dim autrePatient = CreerPatient("AUTRE", "Patient")
        Dim parametres = CinqParametres()

        dao.CreateConfigurationParametre(ConfigurationLdv(idPatient, parametres, profilParamedical:=False, profilPatient:=True), Auteur(0))
        dao.CreateConfigurationParametre(ConfigurationLdv(autrePatient, parametres, profilParamedical:=True, profilPatient:=False), Auteur(0))

        ' Comportement actuel : le paramètre @profilPatient de l'INSERT reçoit
        ' ProfilParamedical (PatientParametreLdvDao.vb, ligne 133). La case « patient »
        ' cochée à la création est perdue ; UpdateConfigurationParametre, lui, écrit la
        ' bonne valeur.
        Assert.IsFalse(dao.GetParametreByPatientId(CInt(idPatient)).ProfilPatient)
        Assert.IsTrue(dao.GetParametreByPatientId(CInt(autrePatient)).ProfilPatient)
    End Sub

    <TestMethod()> Public Sub CreateConfigurationParametre_DeuxFois_EchoueSansToucherALaPremiere()
        Dim idPatient = CreerPatient()
        Dim parametres = CinqParametres()
        dao.CreateConfigurationParametre(ConfigurationLdv(idPatient, parametres), Auteur(0))
        Dim seconde = ConfigurationLdv(idPatient, parametres)
        seconde.TypeConsultation = False

        ' Le message est celui, recopié, d'une collision d'épisode.
        Dim erreur = Assert.ThrowsException(Of Exception)(Sub() dao.CreateConfigurationParametre(seconde, Auteur(0)))

        StringAssert.Contains(erreur.Message, "Collision")
        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient_parametre_ldv WHERE patient_id = @p0", idPatient)))
        Assert.IsTrue(dao.GetParametreByPatientId(CInt(idPatient)).TypeConsultation)
    End Sub

    <TestMethod()> Public Sub UpdateConfigurationParametre_ReecritChaqueColonne()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim parametres = CinqParametres()
        dao.CreateConfigurationParametre(ConfigurationLdv(idPatient, parametres), Auteur(0))
        Executer("UPDATE oasis.oa_patient_parametre_ldv SET date_modification = @p0 WHERE patient_id = @p1",
                 New Date(2020, 1, 1), idPatient)

        Dim inverse = ConfigurationLdv(idPatient, Enumerable.Reverse(parametres).ToArray(), profilParamedical:=True, profilPatient:=False)
        inverse.TypeConsultation = False
        inverse.TypeVirtuel = True
        inverse.TypeParametre = False
        inverse.ActivitePathologieAigue = False
        inverse.ActiviteSuiviChronique = True
        inverse.ActivitePreventionAutre = False
        inverse.ActivitePreventionEnfantPreScolaire = True
        inverse.ActivitePreventionEnfantScolaire = False
        inverse.ActiviteSuiviGrossesse = True
        inverse.ActiviteSuiviGynecologique = False
        inverse.ActiviteSocial = True
        inverse.ProfilMedical = False

        Assert.IsTrue(dao.UpdateConfigurationParametre(inverse, Auteur(idUtilisateur)))

        Dim lue = dao.GetParametreByPatientId(CInt(idPatient))
        Assert.IsFalse(lue.TypeConsultation)
        Assert.IsTrue(lue.TypeVirtuel)
        Assert.IsFalse(lue.TypeParametre)
        Assert.IsFalse(lue.ActivitePathologieAigue)
        Assert.IsTrue(lue.ActiviteSuiviChronique)
        Assert.IsFalse(lue.ActivitePreventionAutre)
        Assert.IsTrue(lue.ActivitePreventionEnfantPreScolaire)
        Assert.IsFalse(lue.ActivitePreventionEnfantScolaire)
        Assert.IsTrue(lue.ActiviteSuiviGrossesse)
        Assert.IsFalse(lue.ActiviteSuiviGynecologique)
        Assert.IsTrue(lue.ActiviteSocial)
        Assert.IsFalse(lue.ProfilMedical)
        Assert.IsTrue(lue.ProfilParamedical)
        Assert.IsFalse(lue.ProfilPatient, "la mise à jour écrit bien le profil patient")
        CollectionAssert.AreEqual(Enumerable.Reverse(parametres).ToArray(),
            New Long() {lue.Parametre1, lue.Parametre2, lue.Parametre3, lue.Parametre4, lue.Parametre5})
        Assert.AreEqual(idUtilisateur, lue.UserModification)
        Assert.AreEqual(Date.Today, lue.DateModification.Date)
    End Sub

    <TestMethod()> Public Sub UpdateConfigurationParametre_NeTouchePasLesAutresPatients()
        Dim idPatient = CreerPatient()
        Dim autrePatient = CreerPatient("AUTRE", "Patient")
        Dim parametres = CinqParametres()
        dao.CreateConfigurationParametre(ConfigurationLdv(idPatient, parametres), Auteur(0))
        dao.CreateConfigurationParametre(ConfigurationLdv(autrePatient, parametres), Auteur(0))
        Dim modifiee = ConfigurationLdv(idPatient, parametres)
        modifiee.TypeConsultation = False

        dao.UpdateConfigurationParametre(modifiee, Auteur(0))

        Assert.IsFalse(dao.GetParametreByPatientId(CInt(idPatient)).TypeConsultation)
        Assert.IsTrue(dao.GetParametreByPatientId(CInt(autrePatient)).TypeConsultation)
    End Sub

    <TestMethod()> Public Sub UpdateConfigurationParametre_SansConfiguration_NeCreeRienEtRenvoieVrai()
        Dim idPatient = CreerPatient()

        ' Comportement actuel : aucune ligne touchée n'est pas une erreur, et rien n'est créé.
        Assert.IsTrue(dao.UpdateConfigurationParametre(ConfigurationLdv(idPatient, CinqParametres()), Auteur(0)))

        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient_parametre_ldv WHERE patient_id = @p0", idPatient)))
    End Sub

End Class
