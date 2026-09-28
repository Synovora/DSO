Imports Oasis_Common

''' <summary>
''' AutoSuiviDao contre la base : paramètres qu'un patient ne suit pas lui-même
''' sur le portail (oa_r_autosuivi, une ligne par exclusion). Le client lourd lit,
''' crée et supprime ces lignes (RadFAutoSuivi) sous oasis_client, qui a le droit
''' de supprimer dans oa_r_autosuivi (migration 2026-09-27). Les contrôleurs du
''' portail déclarent le DAO sans l'appeler.
''' </summary>
<TestClass()> Public Class AutoSuiviDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New AutoSuiviDao

    Private Shared Function Nombre(patientId As Long, parametreId As Long) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_r_autosuivi WHERE patient_id = @p0 AND parametre_id = @p1",
                             patientId, parametreId))
    End Function

    <TestMethod()> Public Sub CreateAutoSuivi_SousClient_EnregistreLaLigne()
        Dim patientId = CreerPatient()
        Dim parametreId = CreerParametreDeMesure("IT poids")

        Dim retour = dao.CreateAutoSuivi(New AutoSuivi With {.PatientId = patientId, .ParametreId = parametreId})

        ' Comportement actuel : ExecuteScalar sur un INSERT sans SELECT renvoie
        ' Nothing, donc 0 ; aucun appelant ne lit la valeur.
        Assert.AreEqual(0, retour)
        Assert.AreEqual(1, Nombre(patientId, parametreId))
    End Sub

    <TestMethod()> Public Sub GetAutoSuiviByPatientIdAndParametreId_SousClient_LigneExistante_EstRelue()
        Dim patientId = CreerPatient()
        Dim parametreId = CreerParametreDeMesure("IT tension")
        dao.CreateAutoSuivi(New AutoSuivi With {.PatientId = patientId, .ParametreId = parametreId})

        Dim lu = dao.GetAutoSuiviByPatientIdAndParametreId(patientId, parametreId)

        Assert.IsNotNull(lu)
        Assert.AreEqual(patientId, lu.PatientId)
        Assert.AreEqual(parametreId, lu.ParametreId)
    End Sub

    <TestMethod()> Public Sub GetAutoSuiviByPatientIdAndParametreId_FiltreSurLesDeuxCles()
        ' Pour l'écran, Nothing veut dire « suivi actif ».
        Dim patientId = CreerPatient()
        Dim autrePatient = CreerPatient("AUTRE", "Patient")
        Dim parametreId = CreerParametreDeMesure("IT pouls")
        Dim autreParametre = CreerParametreDeMesure("IT temperature")
        dao.CreateAutoSuivi(New AutoSuivi With {.PatientId = patientId, .ParametreId = parametreId})

        Assert.IsNull(dao.GetAutoSuiviByPatientIdAndParametreId(patientId, autreParametre))
        Assert.IsNull(dao.GetAutoSuiviByPatientIdAndParametreId(autrePatient, parametreId))
    End Sub

    <TestMethod()> Public Sub DeleteAutoSuivi_SousClient_RetireLaSeuleLigneVisee()
        Dim patientId = CreerPatient()
        Dim autrePatient = CreerPatient("AUTRE", "Patient")
        Dim parametreId = CreerParametreDeMesure("IT saturation")
        Dim autreParametre = CreerParametreDeMesure("IT glycemie")
        dao.CreateAutoSuivi(New AutoSuivi With {.PatientId = patientId, .ParametreId = parametreId})
        dao.CreateAutoSuivi(New AutoSuivi With {.PatientId = patientId, .ParametreId = autreParametre})
        dao.CreateAutoSuivi(New AutoSuivi With {.PatientId = autrePatient, .ParametreId = parametreId})

        dao.DeleteAutoSuivi(New AutoSuivi With {.PatientId = patientId, .ParametreId = parametreId})

        Assert.AreEqual(0, Nombre(patientId, parametreId))
        Assert.AreEqual(1, Nombre(patientId, autreParametre))
        Assert.AreEqual(1, Nombre(autrePatient, parametreId))
        Assert.IsNull(dao.GetAutoSuiviByPatientIdAndParametreId(patientId, parametreId))
    End Sub

    <TestMethod()> Public Sub DeleteAutoSuivi_LigneAbsente_NeFaitRien()
        Dim patientId = CreerPatient()
        Dim parametreId = CreerParametreDeMesure("IT frequence")

        dao.DeleteAutoSuivi(New AutoSuivi With {.PatientId = patientId, .ParametreId = parametreId})

        Assert.AreEqual(0, Nombre(patientId, parametreId))
    End Sub

    <TestMethod()> Public Sub BasculeDeLEcran_ExclurePuisReactiver()
        ' Double clic sur la ligne : exclusion (Create) puis réactivation (Delete).
        Dim patientId = CreerPatient()
        Dim parametreId = CreerParametreDeMesure("IT imc")
        Dim ligne As New AutoSuivi With {.PatientId = patientId, .ParametreId = parametreId}

        dao.CreateAutoSuivi(ligne)
        Assert.IsNotNull(dao.GetAutoSuiviByPatientIdAndParametreId(patientId, parametreId))
        dao.DeleteAutoSuivi(ligne)
        Assert.IsNull(dao.GetAutoSuiviByPatientIdAndParametreId(patientId, parametreId))
    End Sub

End Class
