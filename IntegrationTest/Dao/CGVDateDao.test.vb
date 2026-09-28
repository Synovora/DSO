Imports Oasis_Common

''' <summary>
''' CGVDateDao contre la base : les dates du calendrier vaccinal (en jours depuis
''' la naissance, patient 0 pour le calendrier général) et les valences cochées à
''' chaque date (oa_vaccin_cgv_relation_valence_date).
'''
''' Le client lourd appelle toutes les méthodes (RadFCGV, RadFCPV, RadFVaccinInfo,
''' RadFVaccinInput, RadFValenceSelecteur, RadFVaccin, PrtCarnetVaccinal,
''' RadFEpisodeEnAttenteValidation) : elles tournent sous Compte.Client. Le carnet
''' vaccinal du portail (CarnetVaccinalController) lit aussi GetListFromPatient,
''' vérifié en plus sous Compte.Web.
''' </summary>
<TestClass()> Public Class CGVDateDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New CGVDateDao

    Private Const DateAbsente As Long = 987654321

    Private Shared Function LigneDeLaDate(table As DataTable, idDate As Long) As DataRow
        Return table.Rows.Cast(Of DataRow)().FirstOrDefault(Function(r) CLng(r("id")) = idDate)
    End Function

    Private Function Relation(idRelation As Long, patientId As Long) As RelationValenceDate
        Return dao.GetRelationListFromPatient(patientId).Single(Function(r) r.Id = idRelation)
    End Function

    ' --- Create et lectures unitaires ----------------------------------------------

    <TestMethod()> Public Sub Create_EstRelueSansRealisationNiSignature()
        Dim idPatient = CreerPatient()

        Dim idDate = dao.Create(New CGVDate With {.Days = 60, .Patient = idPatient})

        Assert.IsTrue(idDate > 0)
        Dim lue = dao.GetById(idDate)
        Assert.AreEqual(idDate, lue.Id)
        Assert.AreEqual(60L, lue.Days)
        Assert.AreEqual(idPatient, lue.Patient)
        Assert.AreEqual(0L, lue.OperatedBy)
        Assert.AreEqual(Date.MinValue, lue.OperatedDate)
        Assert.AreEqual(0L, lue.SignedBy)
        Assert.AreEqual(Date.MinValue, lue.SignedDate)
        Assert.AreEqual(DBNull.Value, Scalaire("SELECT operated_by FROM oasis.oa_vaccin_cgv_date WHERE id = @p0", idDate))
        Assert.AreEqual(DBNull.Value, Scalaire("SELECT signed_date FROM oasis.oa_vaccin_cgv_date WHERE id = @p0", idDate))
    End Sub

    <TestMethod()> Public Sub Create_IgnoreLesChampsDeRealisation()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)

        Dim idDate = dao.Create(New CGVDate With {.Days = 30, .Patient = 0,
                                                  .OperatedBy = idAuteur, .OperatedDate = New Date(2026, 1, 5)})

        Assert.AreEqual(0L, dao.GetById(idDate).OperatedBy)
    End Sub

    <TestMethod()> Public Sub GetById_Inexistante_Nothing()
        Assert.IsNull(dao.GetById(DateAbsente))
    End Sub

    <TestMethod()> Public Sub GetByDaysPatient_TrouveParJoursEtPatient()
        Dim idPatient = CreerPatient()
        CreerDateCgv(120, 0)
        CreerDateCgv(90, idPatient)
        Dim attendue = CreerDateCgv(120, idPatient)

        Dim trouvee = dao.GetByDaysPatient(New CGVDate With {.Days = 120, .Patient = idPatient})

        Assert.AreEqual(attendue, trouvee.Id)
        Assert.AreEqual(120L, trouvee.Days)
        Assert.AreEqual(idPatient, trouvee.Patient)
    End Sub

    <TestMethod()> Public Sub GetByDaysPatient_AbsenteChezCePatient_Nothing()
        Dim idPatient = CreerPatient()
        CreerDateCgv(120, 0)

        Assert.IsNull(dao.GetByDaysPatient(New CGVDate With {.Days = 120, .Patient = idPatient}))
    End Sub

    ' --- GetListFromPatient (client et portail) ------------------------------------

    <TestMethod()> Public Sub GetListFromPatient_SeulementLesDatesDuPatient()
        Dim idPatient = CreerPatient()
        Dim d1 = CreerDateCgv(60, idPatient)
        Dim d2 = CreerDateCgv(120, idPatient)
        CreerDateCgv(60, 0)
        CreerDateCgv(60, CreerPatient("AUTRE", "Patient"))

        Dim liste = dao.GetListFromPatient(idPatient)

        CollectionAssert.AreEquivalent(New Long() {d1, d2}, liste.Select(Function(d) d.Id).ToList())
        Assert.IsTrue(liste.All(Function(d) d.Patient = idPatient))
    End Sub

    <TestMethod()> Public Sub GetListFromPatient_CalendrierGeneral_PatientZero()
        Dim generale = CreerDateCgv(30, 0)
        CreerDateCgv(30, CreerPatient())

        CollectionAssert.AreEqual(New Long() {generale}, dao.GetListFromPatient(0).Select(Function(d) d.Id).ToArray())
    End Sub

    <TestMethod()> Public Sub GetListFromPatient_SansDate_ListeVide()
        CreerDateCgv(30, 0)
        Assert.AreEqual(0, dao.GetListFromPatient(CreerPatient()).Count)
    End Sub

    <TestMethod()> Public Sub GetListFromPatient_SousWeb_RelitLesDatesEtLeurRealisation()
        Dim idPatient = CreerPatient()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim idDate = CreerDateCgv(60, idPatient)
        dao.Update(New CGVDate With {.Id = idDate, .Days = 60, .Patient = idPatient,
                                     .OperatedBy = idAuteur, .OperatedDate = New Date(2026, 2, 10)})
        CreerDateCgv(60, 0)

        UtiliserCompte(Compte.Web)
        Dim liste = dao.GetListFromPatient(idPatient)

        Assert.AreEqual(1, liste.Count)
        Assert.AreEqual(idDate, liste(0).Id)
        Assert.AreEqual(idAuteur, liste(0).OperatedBy)
        Assert.AreEqual(New Date(2026, 2, 10), liste(0).OperatedDate)
    End Sub

    ' --- GetListToSign -------------------------------------------------------------

    <TestMethod()> Public Sub GetListToSign_DatesRealiseesNonSigneesAvecLePatient()
        Dim idPatient = CreerPatient("VACCINE", "Paul")
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim aSigner = CreerDateCgv(60, idPatient)
        Dim nonRealisee = CreerDateCgv(90, idPatient)
        Dim dejaSignee = CreerDateCgv(120, idPatient)
        Dim sansPatient = CreerDateCgv(150, 0)
        dao.Update(New CGVDate With {.Id = aSigner, .Days = 60, .Patient = idPatient,
                                     .OperatedBy = idAuteur, .OperatedDate = New Date(2026, 3, 1)})
        dao.Update(New CGVDate With {.Id = dejaSignee, .Days = 120, .Patient = idPatient,
                                     .OperatedBy = idAuteur, .OperatedDate = New Date(2026, 3, 1),
                                     .SignedBy = idAuteur, .SignedDate = New Date(2026, 3, 2)})
        dao.Update(New CGVDate With {.Id = sansPatient, .Days = 150, .Patient = 0,
                                     .OperatedBy = idAuteur, .OperatedDate = New Date(2026, 3, 1)})

        Dim table = dao.GetListToSign()

        Assert.AreEqual(2, table.Rows.Count)
        Assert.IsNull(LigneDeLaDate(table, nonRealisee))
        Assert.IsNull(LigneDeLaDate(table, dejaSignee))
        Dim ligne = LigneDeLaDate(table, aSigner)
        Assert.IsNotNull(ligne)
        Assert.AreEqual(60L, CLng(ligne("days")))
        Assert.AreEqual(idAuteur, CLng(ligne("operated_by")))
        Assert.AreEqual("VACCINE", CStr(ligne("oa_patient_nom")))
        Assert.AreEqual("Paul", CStr(ligne("oa_patient_prenom")))
        Assert.AreEqual(New Date(1970, 1, 15), CDate(ligne("oa_patient_date_naissance")).Date)
        ' Jointure externe : une date sans patient reste listée, sans identité.
        Dim orpheline = LigneDeLaDate(table, sansPatient)
        Assert.IsNotNull(orpheline)
        Assert.AreEqual(DBNull.Value, orpheline("oa_patient_nom"))
    End Sub

    <TestMethod()> Public Sub GetListToSign_RienARealiser_TableVide()
        CreerDateCgv(60, CreerPatient())
        Assert.AreEqual(0, dao.GetListToSign().Rows.Count)
    End Sub

    ' --- Update ----------------------------------------------------------------------

    <TestMethod()> Public Sub Update_EnregistreRealisationEtSignature()
        Dim idPatient = CreerPatient()
        Dim autrePatient = CreerPatient("AUTRE", "Patient")
        Dim operateur = CreerUtilisateur(avecCle:=False)
        Dim signataire = CreerUtilisateur(avecCle:=False)
        Dim idDate = CreerDateCgv(60, idPatient)

        Dim retour = dao.Update(New CGVDate With {
            .Id = idDate, .Days = 75, .Patient = autrePatient,
            .OperatedBy = operateur, .OperatedDate = New Date(2026, 4, 1),
            .SignedBy = signataire, .SignedDate = New Date(2026, 4, 2)})

        ' Comportement actuel : Update renvoie toujours 0, l'id n'est jamais affecté.
        Assert.AreEqual(0L, retour)
        Dim lue = dao.GetById(idDate)
        Assert.AreEqual(75L, lue.Days)
        Assert.AreEqual(autrePatient, lue.Patient)
        Assert.AreEqual(operateur, lue.OperatedBy)
        Assert.AreEqual(New Date(2026, 4, 1), lue.OperatedDate)
        Assert.AreEqual(signataire, lue.SignedBy)
        Assert.AreEqual(New Date(2026, 4, 2), lue.SignedDate)
    End Sub

    <TestMethod()> Public Sub Update_ValeursAbsentes_RemetLesColonnesANull()
        ' RadFVaccinInfo annule une réalisation en remettant OperatedBy et OperatedDate à Nothing.
        Dim idPatient = CreerPatient()
        Dim operateur = CreerUtilisateur(avecCle:=False)
        Dim idDate = CreerDateCgv(60, idPatient)
        dao.Update(New CGVDate With {.Id = idDate, .Days = 60, .Patient = idPatient,
                                     .OperatedBy = operateur, .OperatedDate = New Date(2026, 4, 1),
                                     .SignedBy = operateur, .SignedDate = New Date(2026, 4, 2)})

        Dim annulee = dao.GetById(idDate)
        annulee.OperatedBy = Nothing
        annulee.OperatedDate = Nothing
        annulee.SignedBy = Nothing
        annulee.SignedDate = Nothing
        dao.Update(annulee)

        For Each colonne In {"operated_by", "operated_date", "signed_by", "signed_date"}
            Assert.AreEqual(DBNull.Value, Scalaire("SELECT " & colonne & " FROM oasis.oa_vaccin_cgv_date WHERE id = @p0", idDate), colonne)
        Next
        Assert.AreEqual(1, CompterLignesVaccin("oa_vaccin_cgv_date", "id = @p0 AND days = 60", idDate))
    End Sub

    ' --- Delete ----------------------------------------------------------------------

    <TestMethod()> Public Sub Delete_SupprimeLaDateEtSesValencesCochees_PasLesAutres()
        Dim idPatient = CreerPatient()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim v1 = CreerValence(utilisateurId:=idAuteur)
        Dim v2 = CreerValence(utilisateurId:=idAuteur)
        Dim cible = CreerDateCgv(60, idPatient)
        Dim gardee = CreerDateCgv(90, idPatient)
        LierValenceDateCgv(cible, v1, idPatient)
        LierValenceDateCgv(cible, v2, idPatient)
        Dim relationGardee = LierValenceDateCgv(gardee, v1, idPatient)

        Assert.AreEqual(cible, dao.Delete(New CGVDate With {.Id = cible}))

        Assert.IsNull(dao.GetById(cible))
        Assert.AreEqual(0, CompterLignesVaccin("oa_vaccin_cgv_relation_valence_date", "date = @p0", cible))
        Assert.IsNotNull(dao.GetById(gardee))
        CollectionAssert.AreEqual(New Long() {relationGardee},
                                  dao.GetRelationListFromPatient(idPatient).Select(Function(r) r.Id).ToArray())
        Assert.AreEqual(1, CompterLignesVaccin("oa_valence", "id = @p0", v1), "les valences restent")
    End Sub

    <TestMethod()> Public Sub Delete_DateInexistante_NeLevePasDErreur()
        Dim gardee = CreerDateCgv(60, 0)

        Assert.AreEqual(DateAbsente, dao.Delete(New CGVDate With {.Id = DateAbsente}))

        Assert.IsNotNull(dao.GetById(gardee))
    End Sub

    ' --- Valences cochées à une date ------------------------------------------------

    <TestMethod()> Public Sub CreateRelation_EstRelueParPatientEtParValence()
        Dim idPatient = CreerPatient()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim v1 = CreerValence(utilisateurId:=idAuteur)
        Dim v2 = CreerValence(utilisateurId:=idAuteur)
        Dim idDate = CreerDateCgv(60, idPatient)
        Dim dateGenerale = CreerDateCgv(60, 0)

        Dim r1 = dao.CreateRelation(New RelationValenceDate With {.Date = idDate, .Valence = v1, .Patient = idPatient, .Status = 2})
        Dim r2 = dao.CreateRelation(New RelationValenceDate With {.Date = idDate, .Valence = v2, .Patient = idPatient})
        Dim r3 = dao.CreateRelation(New RelationValenceDate With {.Date = dateGenerale, .Valence = v1, .Patient = 0})

        Assert.IsTrue(r1 > 0 AndAlso r2 > 0 AndAlso r3 > 0)
        Dim lue = Relation(r1, idPatient)
        Assert.AreEqual(v1, lue.Valence)
        Assert.AreEqual(idDate, lue.Date)
        Assert.AreEqual(idPatient, lue.Patient)
        Assert.AreEqual(2S, lue.Status)
        Assert.AreEqual(0S, Relation(r2, idPatient).Status)

        CollectionAssert.AreEquivalent(New Long() {r1, r2}, dao.GetRelationListFromPatient(idPatient).Select(Function(r) r.Id).ToList())
        CollectionAssert.AreEqual(New Long() {r3}, dao.GetRelationListFromPatient(0).Select(Function(r) r.Id).ToArray())
        CollectionAssert.AreEquivalent(New Long() {r1, r3}, dao.GetRelationListByValence(v1).Select(Function(r) r.Id).ToList())
        CollectionAssert.AreEqual(New Long() {r2}, dao.GetRelationListByValence(v2).Select(Function(r) r.Id).ToArray())
    End Sub

    <TestMethod()> Public Sub ListesDeRelations_SansRelation_Vides()
        Dim idPatient = CreerPatient()
        Dim idValence = CreerValence()
        CreerDateCgv(60, idPatient)

        Assert.AreEqual(0, dao.GetRelationListFromPatient(idPatient).Count)
        Assert.AreEqual(0, dao.GetRelationListByValence(idValence).Count)
    End Sub

    <TestMethod()> Public Sub GetRelationIfExist_TrouveParDateEtValence()
        Dim idPatient = CreerPatient()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim v1 = CreerValence(utilisateurId:=idAuteur)
        Dim v2 = CreerValence(utilisateurId:=idAuteur)
        Dim idDate = CreerDateCgv(60, idPatient)
        Dim attendue = LierValenceDateCgv(idDate, v1, idPatient, 1)

        Dim trouvee = dao.GetRelationIfExist(New RelationValenceDate With {.Date = idDate, .Valence = v1, .Patient = idPatient})

        Assert.AreEqual(attendue, trouvee.Id)
        Assert.AreEqual(1S, trouvee.Status)
        Assert.IsNull(dao.GetRelationIfExist(New RelationValenceDate With {.Date = idDate, .Valence = v2, .Patient = idPatient}))
    End Sub

    <TestMethod()> Public Sub GetRelationIfExist_NeFiltrePasLePatient()
        Dim idPatient = CreerPatient()
        Dim idValence = CreerValence()
        Dim idDate = CreerDateCgv(60, idPatient)
        Dim attendue = LierValenceDateCgv(idDate, idValence, idPatient)

        ' Comportement actuel : seuls la date et la valence comptent ; le patient
        ' du bean est ignoré (une date appartient de toute façon à un seul patient).
        Dim trouvee = dao.GetRelationIfExist(New RelationValenceDate With {.Date = idDate, .Valence = idValence, .Patient = 424242})

        Assert.AreEqual(attendue, trouvee.Id)
    End Sub

    <TestMethod()> Public Sub UpdateRelation_RemplaceTousLesChampsParId()
        Dim idPatient = CreerPatient()
        Dim autrePatient = CreerPatient("AUTRE", "Patient")
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim v1 = CreerValence(utilisateurId:=idAuteur)
        Dim v2 = CreerValence(utilisateurId:=idAuteur)
        Dim d1 = CreerDateCgv(60, idPatient)
        Dim d2 = CreerDateCgv(90, autrePatient)
        Dim idRelation = LierValenceDateCgv(d1, v1, idPatient)
        Dim temoin = LierValenceDateCgv(d1, v2, idPatient)

        Dim retour = dao.UpdateRelation(New RelationValenceDate With {.Id = idRelation, .Date = d2, .Valence = v2, .Patient = autrePatient, .Status = 1})

        Assert.AreEqual(idRelation, retour)
        Dim lue = Relation(idRelation, autrePatient)
        Assert.AreEqual(d2, lue.Date)
        Assert.AreEqual(v2, lue.Valence)
        Assert.AreEqual(1S, lue.Status)
        Dim inchangee = Relation(temoin, idPatient)
        Assert.AreEqual(d1, inchangee.Date)
        Assert.AreEqual(0S, inchangee.Status)
    End Sub

    <TestMethod()> Public Sub UpdateRelationStatus_CibleDateValenceEtPatient()
        Dim idPatient = CreerPatient()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim v1 = CreerValence(utilisateurId:=idAuteur)
        Dim v2 = CreerValence(utilisateurId:=idAuteur)
        Dim d1 = CreerDateCgv(60, idPatient)
        Dim d2 = CreerDateCgv(90, idPatient)
        Dim cible = LierValenceDateCgv(d1, v1, idPatient)
        Dim autreValence = LierValenceDateCgv(d1, v2, idPatient)
        Dim autreDate = LierValenceDateCgv(d2, v1, idPatient)

        ' RadFVaccinInput ne renseigne pas l'id : le DAO le renvoie tel quel, donc 0.
        Dim retour = dao.UpdateRelationStatus(New RelationValenceDate With {.Date = d1, .Valence = v1, .Patient = idPatient, .Status = 1})

        Assert.AreEqual(0L, retour)
        Assert.AreEqual(1S, Relation(cible, idPatient).Status)
        Assert.AreEqual(0S, Relation(autreValence, idPatient).Status)
        Assert.AreEqual(0S, Relation(autreDate, idPatient).Status)
    End Sub

    <TestMethod()> Public Sub UpdateRelationStatus_AutrePatient_NeToucheRien()
        Dim idPatient = CreerPatient()
        Dim idValence = CreerValence()
        Dim idDate = CreerDateCgv(60, idPatient)
        Dim idRelation = LierValenceDateCgv(idDate, idValence, idPatient)

        dao.UpdateRelationStatus(New RelationValenceDate With {.Date = idDate, .Valence = idValence, .Patient = 424242, .Status = 2})

        Assert.AreEqual(0S, Relation(idRelation, idPatient).Status)
    End Sub

    <TestMethod()> Public Sub DeleteRelation_SupprimeSeulementLeTripletDateValencePatient()
        Dim idPatient = CreerPatient()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim v1 = CreerValence(utilisateurId:=idAuteur)
        Dim v2 = CreerValence(utilisateurId:=idAuteur)
        Dim d1 = CreerDateCgv(60, idPatient)
        Dim d2 = CreerDateCgv(90, idPatient)
        Dim cible = LierValenceDateCgv(d1, v1, idPatient)
        Dim autreValence = LierValenceDateCgv(d1, v2, idPatient)
        Dim autreDate = LierValenceDateCgv(d2, v1, idPatient)

        Assert.AreEqual(cible, dao.DeleteRelation(New RelationValenceDate With {.Id = cible, .Date = d1, .Valence = v1, .Patient = idPatient}))
        ' Mauvais patient : rien n'est supprimé.
        dao.DeleteRelation(New RelationValenceDate With {.Date = d1, .Valence = v2, .Patient = 424242})

        CollectionAssert.AreEquivalent(New Long() {autreValence, autreDate},
                                       dao.GetRelationListFromPatient(idPatient).Select(Function(r) r.Id).ToList())
        Assert.IsNotNull(dao.GetById(d1), "la date reste")
    End Sub

End Class
