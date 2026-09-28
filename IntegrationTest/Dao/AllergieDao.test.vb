Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' AllergieDao contre la base de test. Toutes ses méthodes sont appelées par le
''' client lourd (RadF_AllergieSelecteur, RadFPatientAllergieListe, PrtSynthese, et
''' TheriaqueDao.IsSpecialiteAllergique, le contrôle fait à la prescription) : elles
''' tournent sous oasis_client.
'''
''' La liste des allergies actives d'un patient est ce que le contrôle de
''' prescription compare aux substances du médicament : une allergie absente de
''' cette liste n'arrête aucune prescription. Les tests vérifient donc qu'elle
''' contient les allergies actives de ce patient, et elles seules.
''' </summary>
<TestClass()> Public Class AllergieDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New AllergieDao

    Private Const AllergieAbsente As Long = 987654321

    Private Shared Function Auteur(idUtilisateur As Long) As Utilisateur
        Return New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}
    End Function

    Private Function Declarer(idPatient As Long, substanceId As Long, denomination As String, idUtilisateur As Long) As Boolean
        Return dao.CreationAllergie(
            New Allergie With {.PatientId = idPatient, .SubstanceId = substanceId, .SubstancePereId = 0,
                               .DenominationSubstance = denomination},
            Auteur(idUtilisateur))
    End Function

    Private Shared Function IdAllergie(idPatient As Long, substanceId As Long) As Long
        Return CLng(Scalaire("SELECT MAX(allergie_id) FROM oasis.oa_patient_allergie WHERE patient_id = @p0 AND substance_id = @p1",
                             idPatient, substanceId))
    End Function

    Private Shared Function NombreLignes(idPatient As Long) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient_allergie WHERE patient_id = @p0", idPatient))
    End Function

    Private Function SubstancesActives(idPatient As Long) As Long()
        Dim table = dao.GetAllAllergiebyPatient(CInt(idPatient))
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("substance_id"))).ToArray()
    End Function

    ' --- Création ---------------------------------------------------------------------

    <TestMethod()> Public Sub CreationAllergie_SubstanceSansPere_EnregistreEtRelit()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()

        Assert.IsTrue(Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur))

        Dim relue = dao.GetAllergieSubstanceById(IdAllergie(idPatient, 111))
        Assert.AreEqual(idPatient, relue.PatientId)
        Assert.AreEqual(111L, relue.SubstanceId)
        Assert.AreEqual(0L, relue.SubstancePereId)
        Assert.AreEqual("SUBSTANCE A", relue.DenominationSubstance)
        Assert.AreEqual("", relue.DenominationSubstancePere, "sans père, le DAO écrit une dénomination vide")
        Assert.AreEqual(idUtilisateur, relue.UserCreation)
        Assert.AreEqual(Date.Today, relue.DateCreation.Date)
        Assert.AreEqual(0L, relue.UserAnnulation, "colonne NULL tant que l'allergie n'est pas annulée")
        Assert.AreEqual(Date.MinValue, relue.DateAnnulation)
        Assert.IsFalse(relue.Inactif)
    End Sub

    <TestMethod()> Public Sub CreationAllergie_DejaActive_RenvoieFalseSansDoublon()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Assert.IsTrue(Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur))

        Assert.IsFalse(Declarer(idPatient, 111, "SUBSTANCE A BIS", idUtilisateur))

        Assert.AreEqual(1, NombreLignes(idPatient))
        Assert.AreEqual("SUBSTANCE A", dao.GetAllergieSubstanceById(IdAllergie(idPatient, 111)).DenominationSubstance)
    End Sub

    <TestMethod()> Public Sub CreationAllergie_ApresAnnulation_RecreeUneLigneActive()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur)
        dao.AnnulationAllergie(CInt(IdAllergie(idPatient, 111)), Auteur(idUtilisateur))

        Assert.IsTrue(Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur), "le doublon ne compte que les allergies actives")

        Assert.AreEqual(2, NombreLignes(idPatient))
        CollectionAssert.AreEqual(New Long() {111}, SubstancesActives(idPatient))
    End Sub

    <TestMethod()> Public Sub CreationAllergie_MemeSubstanceChezUnAutrePatient_EstCreee()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAutre = CreerPatient("AUTRE", "Patient")
        Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur)

        Assert.IsTrue(Declarer(idAutre, 111, "SUBSTANCE A", idUtilisateur))

        Assert.AreEqual(1, NombreLignes(idPatient))
        Assert.AreEqual(1, NombreLignes(idAutre))
    End Sub

    <TestMethod()> Public Sub CreationAllergie_SansPere_NeVoitPasUneAllergieALaSubstancePere()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        CreerAllergieSubstancePere(idPatient, 900, "FAMILLE TEST", idUtilisateur)

        ' Le contrôle de doublon ne regarde que substance_id quand la substance n'a pas de père.
        Assert.IsTrue(Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur))

        Assert.AreEqual(2, NombreLignes(idPatient))
    End Sub

    <TestMethod()> Public Sub CreationAllergie_AvecSubstancePere_SansBaseTheriak_Echoue()
        If BaseTheriakPresente() Then Assert.Inconclusive("La base Theriak existe sur cette instance : ce test suppose son absence.")
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim declaration As New Allergie With {.PatientId = idPatient, .SubstanceId = 111, .SubstancePereId = 900,
                                           .DenominationSubstance = "SUBSTANCE A"}

        ' La dénomination de la substance père est lue dans la base Theriak avant l'INSERT.
        Assert.ThrowsException(Of SqlException)(Sub() dao.CreationAllergie(declaration, Auteur(idUtilisateur)))

        Assert.AreEqual(0, NombreLignes(idPatient))
        ' Comportement actuel : avec un père, le DAO efface la substance du bean avant
        ' d'aller en base ; l'allergie est enregistrée au niveau de la substance père.
        Assert.AreEqual(0L, declaration.SubstanceId)
        Assert.AreEqual("", declaration.DenominationSubstance)
    End Sub

    ' --- Liste d'un patient -------------------------------------------------------------

    <TestMethod()> Public Sub GetAllAllergiebyPatient_ActivesDuPatient_TrieesParDenomination()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAutre = CreerPatient("AUTRE", "Patient")
        Declarer(idPatient, 222, "SUBSTANCE B", idUtilisateur)
        Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur)
        CreerAllergieSubstancePere(idPatient, 900, "FAMILLE TEST", idUtilisateur)
        Declarer(idPatient, 333, "SUBSTANCE ANNULEE", idUtilisateur)
        dao.AnnulationAllergie(CInt(IdAllergie(idPatient, 333)), Auteur(idUtilisateur))
        Declarer(idAutre, 444, "AUTRE PATIENT", idUtilisateur)

        Dim table = dao.GetAllAllergiebyPatient(CInt(idPatient))

        ' L'allergie à la substance père a une dénomination de substance vide : elle vient en tête.
        CollectionAssert.AreEqual(New Long() {0, 111, 222}, SubstancesActives(idPatient))
        Assert.AreEqual(900L, CLng(table.Rows(0)("substance_pere_id")))
        Assert.AreEqual("FAMILLE TEST", CStr(table.Rows(0)("denomination_substance_pere")))
        Assert.AreEqual(0L, CLng(table.Rows(1)("substance_pere_id")))
        Assert.IsTrue(table.Rows.Cast(Of DataRow)().All(Function(r) CLng(r("patient_id")) = idPatient))
    End Sub

    <TestMethod()> Public Sub GetAllAllergiebyPatient_InactifNull_EstRetenue()
        If Not ColonneNullableTheriaque("oasis.oa_patient_allergie", "inactif") Then
            Assert.Inconclusive("oa_patient_allergie.inactif est NOT NULL dans ce schéma.")
        End If
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur)
        EffacerInactifTheriaque("oasis.oa_patient_allergie", "allergie_id", IdAllergie(idPatient, 111))

        CollectionAssert.AreEqual(New Long() {111}, SubstancesActives(idPatient))
        Assert.IsFalse(dao.GetAllergieSubstanceById(IdAllergie(idPatient, 111)).Inactif)
    End Sub

    <TestMethod()> Public Sub GetAllAllergiebyPatient_SansAllergie_TableVide()
        Dim idPatient = CreerPatient()
        Assert.AreEqual(0, dao.GetAllAllergiebyPatient(CInt(idPatient)).Rows.Count)
    End Sub

    ' --- Lecture par id ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetAllergieSubstanceById_SubstancePere_LitLePere()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        CreerAllergieSubstancePere(idPatient, 900, "FAMILLE TEST", idUtilisateur)

        Dim relue = dao.GetAllergieSubstanceById(IdAllergie(idPatient, 0))

        Assert.AreEqual(0L, relue.SubstanceId)
        Assert.AreEqual(900L, relue.SubstancePereId)
        Assert.AreEqual("", relue.DenominationSubstance)
        Assert.AreEqual("FAMILLE TEST", relue.DenominationSubstancePere)
    End Sub

    <TestMethod()> Public Sub GetAllergieSubstanceById_Inexistante_LeveArgumentException()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetAllergieSubstanceById(AllergieAbsente))
        ' Comportement actuel : message repris de ContreIndicationSubstanceDao.
        Assert.AreEqual("Contre-indication substance inexistante !", erreur.Message)
    End Sub

    ' --- Annulation ---------------------------------------------------------------------------

    <TestMethod()> Public Sub AnnulationAllergie_MarqueInactiveEtTraceLAuteur()
        Dim idCreateur = CreerUtilisateur(avecCle:=False)
        Dim idAnnuleur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Declarer(idPatient, 111, "SUBSTANCE A", idCreateur)
        Declarer(idPatient, 222, "SUBSTANCE B", idCreateur)
        Dim idAllergieA = IdAllergie(idPatient, 111)

        Assert.IsTrue(dao.AnnulationAllergie(CInt(idAllergieA), Auteur(idAnnuleur)))

        Dim relue = dao.GetAllergieSubstanceById(idAllergieA)
        Assert.IsTrue(relue.Inactif)
        Assert.AreEqual(idAnnuleur, relue.UserAnnulation)
        Assert.AreEqual(Date.Today, relue.DateAnnulation.Date)
        Assert.AreEqual(idCreateur, relue.UserCreation, "la création reste tracée")
        CollectionAssert.AreEqual(New Long() {222}, SubstancesActives(idPatient))
    End Sub

    <TestMethod()> Public Sub AnnulationAllergie_Inexistante_RenvoieFalse()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Assert.IsFalse(dao.AnnulationAllergie(CInt(AllergieAbsente), Auteur(idUtilisateur)))
    End Sub

End Class
