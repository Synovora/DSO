Imports Oasis_Common

''' <summary>
''' CGVValenceDao contre la base : les valences inscrites au calendrier vaccinal,
''' patient 0 pour le calendrier général, sinon celui d'un patient. Seul le client
''' lourd l'appelle (RadFCGV, RadFCPV, RadFValenceSelecteur, PrtCarnetVaccinal,
''' RadFEpisodeEnAttenteValidation) : tout tourne sous Compte.Client. Chaque lecture
''' joint oa_valence pour le code, la description et la précaution.
''' </summary>
<TestClass()> Public Class CGVValenceDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New CGVValenceDao

    Private Const LigneAbsente As Integer = 987654321

    Private Shared Function NombreDeValencesCgv() As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_vaccin_cgv_valence"))
    End Function

    ' --- Create et lectures unitaires ----------------------------------------------

    <TestMethod()> Public Sub Create_EstRelueAvecLesLibellesDeLaValence()
        Dim idPatient = CreerPatient()
        Dim idValence = CreerValence(code:="COQ")

        Dim idLigne = dao.Create(New CGVValence With {.Valence = idValence, .Patient = idPatient,
                                                      .Code = "ignore", .Ordre = 99})

        Assert.IsTrue(idLigne > 0)
        Dim lue = dao.GetById(CInt(idLigne))
        Assert.AreEqual(idLigne, lue.Id)
        Assert.AreEqual(idValence, lue.Valence)
        Assert.AreEqual(idPatient, lue.Patient)
        ' Code, description et précaution viennent de oa_valence, pas du bean.
        Assert.AreEqual("COQ", lue.Code)
        Assert.AreEqual("Description COQ", lue.Description)
        Assert.AreEqual("Precaution COQ", lue.Precaution)
    End Sub

    <TestMethod()> Public Sub Create_PremiereLigneEnPositionZeroPuisALaSuite()
        Assert.AreEqual(0, NombreDeValencesCgv(), "l'instantané ne contient aucun calendrier")
        Dim idValence = CreerValence()

        Dim premiere = CreerValenceCgv(idValence, 0)
        Dim deuxieme = CreerValenceCgv(idValence, 0)

        Assert.AreEqual(0, dao.GetById(CInt(premiere)).Ordre)
        Assert.AreEqual(1, dao.GetById(CInt(deuxieme)).Ordre)
    End Sub

    <TestMethod()> Public Sub Create_PositionCalculeeSurTousLesPatients()
        Dim idPatient = CreerPatient()
        Dim autrePatient = CreerPatient("AUTRE", "Patient")
        Dim idValence = CreerValence()
        Dim chezLAutre = CreerValenceCgv(idValence, autrePatient)
        dao.SetOrder(chezLAutre, 25)

        Dim idLigne = CreerValenceCgv(idValence, idPatient)

        ' Comportement actuel : la position suit la plus grande de toute la table,
        ' calendrier général et autres patients compris, pas celle du patient.
        Assert.AreEqual(26, dao.GetById(CInt(idLigne)).Ordre)
    End Sub

    <TestMethod()> Public Sub GetById_Inexistante_LeveArgumentException()
        Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetById(LigneAbsente))
    End Sub

    <TestMethod()> Public Sub GetById_ValenceDisparue_EchoueALaLecture()
        ' La jointure est externe mais le bean exige un code : une ligne dont la
        ' valence n'existe plus ne se relit pas. Suppose l'absence de clé étrangère
        ' sur oa_vaccin_cgv_valence.valence.
        Dim idLigne = CreerValenceCgv(LigneAbsente, 0)

        Assert.ThrowsException(Of InvalidCastException)(Sub() dao.GetById(CInt(idLigne)))
    End Sub

    <TestMethod()> Public Sub GetByValencePatient_TrouveLaLigneDuPatient()
        Dim idPatient = CreerPatient()
        Dim idValence = CreerValence(code:="PNEUMO")
        CreerValenceCgv(idValence, 0)
        Dim attendue = CreerValenceCgv(idValence, idPatient)

        Dim lue = dao.GetByValencePatient(New CGVValence With {.Valence = idValence, .Patient = idPatient})

        Assert.AreEqual(attendue, lue.Id)
        Assert.AreEqual(idPatient, lue.Patient)
        Assert.AreEqual("PNEUMO", lue.Code)
    End Sub

    <TestMethod()> Public Sub GetByValencePatient_AbsenteChezCePatient_LeveArgumentException()
        Dim idPatient = CreerPatient()
        Dim idValence = CreerValence()
        CreerValenceCgv(idValence, 0)

        Assert.ThrowsException(Of ArgumentException)(
            Sub() dao.GetByValencePatient(New CGVValence With {.Valence = idValence, .Patient = idPatient}))
    End Sub

    ' --- Listes et positions -------------------------------------------------------

    <TestMethod()> Public Sub GetListFromPatient_SeulementCePatient_ParPositionCroissante()
        Dim idPatient = CreerPatient()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim v1 = CreerValence(code:="V1", utilisateurId:=idAuteur)
        Dim v2 = CreerValence(code:="V2", utilisateurId:=idAuteur)
        Dim v3 = CreerValence(code:="V3", utilisateurId:=idAuteur)
        Dim l1 = CreerValenceCgv(v1, idPatient)
        Dim l2 = CreerValenceCgv(v2, idPatient)
        Dim l3 = CreerValenceCgv(v3, idPatient)
        CreerValenceCgv(v1, 0)
        dao.SetOrder(l1, 30)
        dao.SetOrder(l2, 10)
        dao.SetOrder(l3, 20)

        Dim liste = dao.GetListFromPatient(idPatient)

        CollectionAssert.AreEqual(New Long() {l2, l3, l1}, liste.Select(Function(l) l.Id).ToArray())
        CollectionAssert.AreEqual(New String() {"V2", "V3", "V1"}, liste.Select(Function(l) l.Code).ToArray())
    End Sub

    <TestMethod()> Public Sub GetListFromPatient_CalendrierGeneral_PatientZero()
        Dim idPatient = CreerPatient()
        Dim idValence = CreerValence()
        Dim generale = CreerValenceCgv(idValence, 0)
        CreerValenceCgv(idValence, idPatient)

        CollectionAssert.AreEqual(New Long() {generale}, dao.GetListFromPatient(0).Select(Function(l) l.Id).ToArray())
    End Sub

    <TestMethod()> Public Sub GetListFromPatient_SansCalendrier_ListeVide()
        CreerValenceCgv(CreerValence(), 0)
        Assert.AreEqual(0, dao.GetListFromPatient(CreerPatient()).Count)
    End Sub

    <TestMethod()> Public Sub GetList_TousLesPatientsParPositionCroissante()
        Dim idPatient = CreerPatient()
        Dim idValence = CreerValence()
        Dim a = CreerValenceCgv(idValence, 0)
        Dim b = CreerValenceCgv(idValence, idPatient)
        Dim c = CreerValenceCgv(idValence, idPatient)
        dao.SetOrder(a, 3)
        dao.SetOrder(b, 1)
        dao.SetOrder(c, 2)

        CollectionAssert.AreEqual(New Long() {b, c, a}, dao.GetList().Select(Function(l) l.Id).ToArray())
    End Sub

    <TestMethod()> Public Sub GetList_SansCalendrier_ListeVide()
        Assert.AreEqual(0, NombreDeValencesCgv(), "l'instantané ne contient aucun calendrier")
        Assert.AreEqual(0, dao.GetList().Count)
    End Sub

    <TestMethod()> Public Sub GetLastOrder_RenvoieLaLigneDePlusGrandePosition()
        Dim idValence = CreerValence()
        Dim a = CreerValenceCgv(idValence, 0)
        Dim b = CreerValenceCgv(idValence, CreerPatient())
        dao.SetOrder(a, 40)
        dao.SetOrder(b, 4)

        Dim derniere = dao.GetLastOrder()

        Assert.AreEqual(a, derniere.Id)
        Assert.AreEqual(40, derniere.Ordre)
    End Sub

    <TestMethod()> Public Sub GetLastOrder_SansCalendrier_Nothing()
        Assert.AreEqual(0, NombreDeValencesCgv(), "l'instantané ne contient aucun calendrier")
        Assert.IsNull(dao.GetLastOrder())
    End Sub

    <TestMethod()> Public Sub GetByOrder_TrouveLaLigneOuNothing()
        Dim idValence = CreerValence(code:="MENC")
        Dim idLigne = CreerValenceCgv(idValence, 0)
        dao.SetOrder(idLigne, 14)

        Dim trouvee = dao.GetByOrder(14)
        Assert.AreEqual(idLigne, trouvee.Id)
        Assert.AreEqual("MENC", trouvee.Code)
        Assert.IsNull(dao.GetByOrder(15))
    End Sub

    <TestMethod()> Public Sub GetListFromOrder_RetientLaBorneEtAuDela_TousPatients()
        Dim idPatient = CreerPatient()
        Dim idValence = CreerValence()
        Dim avant = CreerValenceCgv(idValence, 0)
        Dim borne = CreerValenceCgv(idValence, 0)
        Dim apres = CreerValenceCgv(idValence, idPatient)
        dao.SetOrder(avant, 4)
        dao.SetOrder(borne, 5)
        dao.SetOrder(apres, 6)

        ' Comportement actuel : pas de filtre par patient.
        CollectionAssert.AreEquivalent(New Long() {borne, apres}, dao.GetListFromOrder(5).Select(Function(l) l.Id).ToList())
        Assert.AreEqual(0, dao.GetListFromOrder(7).Count)
    End Sub

    ' --- Modifications et suppression ----------------------------------------------

    <TestMethod()> Public Sub Update_ChangePatientEtPosition_PasLaValence()
        Dim idPatient = CreerPatient()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim idValence = CreerValence(code:="AVANT", utilisateurId:=idAuteur)
        Dim autreValence = CreerValence(code:="AUTRE", utilisateurId:=idAuteur)
        Dim idLigne = CreerValenceCgv(idValence, 0)

        Dim retour = dao.Update(New CGVValence With {.Id = idLigne, .Patient = idPatient, .Ordre = 7, .Valence = autreValence})

        Assert.AreEqual(idLigne, retour)
        Dim lue = dao.GetById(CInt(idLigne))
        Assert.AreEqual(idPatient, lue.Patient)
        Assert.AreEqual(7, lue.Ordre)
        ' Comportement actuel : le paramètre @valence est préparé mais absent de l'UPDATE.
        Assert.AreEqual(idValence, lue.Valence)
        Assert.AreEqual("AVANT", lue.Code)
    End Sub

    <TestMethod()> Public Sub SetOrder_ChangeSeulementLaPosition()
        Dim idPatient = CreerPatient()
        Dim idValence = CreerValence()
        Dim idLigne = CreerValenceCgv(idValence, idPatient)

        Assert.AreEqual(idLigne, dao.SetOrder(idLigne, 12))

        Dim lue = dao.GetById(CInt(idLigne))
        Assert.AreEqual(12, lue.Ordre)
        Assert.AreEqual(idPatient, lue.Patient)
        Assert.AreEqual(idValence, lue.Valence)
    End Sub

    <TestMethod()> Public Sub Delete_SupprimeLaLigneSeule()
        Dim idPatient = CreerPatient()
        Dim idValence = CreerValence()
        Dim cible = CreerValenceCgv(idValence, idPatient)
        Dim gardee = CreerValenceCgv(idValence, 0)
        Dim idDate = CreerDateCgv(30, idPatient)
        LierValenceDateCgv(idDate, idValence, idPatient)

        Assert.AreEqual(cible, dao.Delete(New CGVValence With {.Id = cible}))

        Assert.AreEqual(0, CompterLignesVaccin("oa_vaccin_cgv_valence", "id = @p0", cible))
        Assert.AreEqual(1, CompterLignesVaccin("oa_vaccin_cgv_valence", "id = @p0", gardee))
        Assert.AreEqual(1, CompterLignesVaccin("oa_valence", "id = @p0", idValence))
        ' Pas de cascade : RadFValenceSelecteur supprime lui-même les dates cochées avant.
        Assert.AreEqual(1, CompterLignesVaccin("oa_vaccin_cgv_relation_valence_date", "valence = @p0 AND patient = @p1",
                                              idValence, idPatient))
    End Sub

    <TestMethod()> Public Sub Delete_LigneInexistante_NeLevePasDErreur()
        Dim gardee = CreerValenceCgv(CreerValence(), 0)

        Assert.AreEqual(CLng(LigneAbsente), dao.Delete(New CGVValence With {.Id = LigneAbsente}))

        Assert.AreEqual(1, NombreDeValencesCgv())
        Assert.AreEqual(1, CompterLignesVaccin("oa_vaccin_cgv_valence", "id = @p0", gardee))
    End Sub

End Class
