Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' ContreIndicationSubstanceDao contre la base de test. Toutes ses méthodes sont
''' appelées par le client lourd (RadF_CI_ATC_Selecteur, RadFPatientContreIndicationListe,
''' et TheriaqueDao.IsSpecialiteContreIndique, le contrôle fait à la prescription) :
''' elles tournent sous oasis_client. La liste sert aussi à la synthèse du portail
''' (SyntheseController, par PatientDao) : elle est aussi lue sous oasis_web.
'''
''' La liste des contre-indications actives d'un patient est ce que le contrôle de
''' prescription compare aux substances du médicament, directement (substance_id) ou
''' par la famille (substance_pere_id, quand substance_id vaut 0). Les tests
''' vérifient qu'elle contient les contre-indications actives de ce patient, et elles
''' seules, avec ces deux colonnes.
''' </summary>
<TestClass()> Public Class ContreIndicationSubstanceDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New ContreIndicationSubstanceDao

    Private Const ContreIndicationAbsente As Long = 987654321

    Private Shared Function Auteur(idUtilisateur As Long) As Utilisateur
        Return New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}
    End Function

    Private Function Declarer(idPatient As Long, substanceId As Long, denomination As String, idUtilisateur As Long) As Boolean
        Return dao.CreationContreIndicationSubstance(
            New ContreIndicationSubstance With {.PatientId = idPatient, .SubstanceId = substanceId, .SubstancePereId = 0,
                                                .DenominationSubstance = denomination},
            Auteur(idUtilisateur))
    End Function

    Private Shared Function IdContreIndication(idPatient As Long, substanceId As Long) As Long
        Return CLng(Scalaire("SELECT MAX(contre_indication_id) FROM oasis.oa_patient_contre_indication_substance" &
                             " WHERE patient_id = @p0 AND substance_id = @p1", idPatient, substanceId))
    End Function

    Private Shared Function NombreLignes(idPatient As Long) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient_contre_indication_substance WHERE patient_id = @p0", idPatient))
    End Function

    Private Function SubstancesActives(idPatient As Long) As Long()
        Dim table = dao.GetAllContreIndicationSubstancebyPatient(CInt(idPatient))
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("substance_id"))).ToArray()
    End Function

    ' --- Création ---------------------------------------------------------------------

    <TestMethod()> Public Sub CreationContreIndicationSubstance_SansPere_EnregistreEtRelit()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()

        Assert.IsTrue(Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur))

        Dim relue = dao.GetContreIndicationSubstanceById(IdContreIndication(idPatient, 111))
        Assert.AreEqual(idPatient, relue.PatientId)
        Assert.AreEqual(111L, relue.SubstanceId)
        Assert.AreEqual(0L, relue.SubstancePereId)
        Assert.AreEqual("SUBSTANCE A", relue.DenominationSubstance)
        Assert.AreEqual("", relue.DenominationSubstancePere)
        Assert.AreEqual(idUtilisateur, relue.UserCreation)
        Assert.AreEqual(Date.Today, relue.DateCreation.Date)
        Assert.AreEqual(0L, relue.UserAnnulation)
        Assert.AreEqual(Date.MinValue, relue.DateAnnulation)
        Assert.IsFalse(relue.Inactif)
    End Sub

    <TestMethod()> Public Sub CreationContreIndicationSubstance_DejaActive_RenvoieFalseSansDoublon()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur)

        Assert.IsFalse(Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur))

        Assert.AreEqual(1, NombreLignes(idPatient))
    End Sub

    <TestMethod()> Public Sub CreationContreIndicationSubstance_ApresAnnulation_RecreeUneLigneActive()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur)
        dao.AnnulationContreIndicationSubstance(CInt(IdContreIndication(idPatient, 111)), Auteur(idUtilisateur))

        Assert.IsTrue(Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur))

        Assert.AreEqual(2, NombreLignes(idPatient))
        CollectionAssert.AreEqual(New Long() {111}, SubstancesActives(idPatient))
    End Sub

    <TestMethod()> Public Sub CreationContreIndicationSubstance_MemeSubstanceChezUnAutrePatient_EstCreee()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAutre = CreerPatient("AUTRE", "Patient")
        Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur)

        Assert.IsTrue(Declarer(idAutre, 111, "SUBSTANCE A", idUtilisateur))

        Assert.AreEqual(1, NombreLignes(idAutre))
    End Sub

    <TestMethod()> Public Sub CreationContreIndicationSubstance_AvecSubstancePere_SansBaseTheriak_Echoue()
        If BaseTheriakPresente() Then Assert.Inconclusive("La base Theriak existe sur cette instance : ce test suppose son absence.")
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim declaration As New ContreIndicationSubstance With {.PatientId = idPatient, .SubstanceId = 111, .SubstancePereId = 900,
                                                               .DenominationSubstance = "SUBSTANCE A"}

        ' La dénomination de la substance père est lue dans la base Theriak avant l'INSERT.
        Assert.ThrowsException(Of SqlException)(Sub() dao.CreationContreIndicationSubstance(declaration, Auteur(idUtilisateur)))

        Assert.AreEqual(0, NombreLignes(idPatient))
        ' Comportement actuel : avec un père, la contre-indication porte sur la famille,
        ' la substance est effacée du bean avant l'accès à la base.
        Assert.AreEqual(0L, declaration.SubstanceId)
        Assert.AreEqual("", declaration.DenominationSubstance)
    End Sub

    ' --- Liste d'un patient -------------------------------------------------------------

    <TestMethod()> Public Sub GetAllContreIndicationSubstancebyPatient_ActivesDuPatient_TrieesParDenomination()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAutre = CreerPatient("AUTRE", "Patient")
        Declarer(idPatient, 222, "SUBSTANCE B", idUtilisateur)
        Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur)
        CreerContreIndicationSubstancePere(idPatient, 900, "FAMILLE TEST", idUtilisateur)
        Declarer(idPatient, 333, "SUBSTANCE ANNULEE", idUtilisateur)
        dao.AnnulationContreIndicationSubstance(CInt(IdContreIndication(idPatient, 333)), Auteur(idUtilisateur))
        Declarer(idAutre, 444, "AUTRE PATIENT", idUtilisateur)

        Dim table = dao.GetAllContreIndicationSubstancebyPatient(CInt(idPatient))

        CollectionAssert.AreEqual(New Long() {0, 111, 222}, SubstancesActives(idPatient))
        Assert.AreEqual(900L, CLng(table.Rows(0)("substance_pere_id")), "famille contre-indiquée, lue par le contrôle de prescription")
        Assert.AreEqual("FAMILLE TEST", CStr(table.Rows(0)("denomination_substance_pere")))
        Assert.IsTrue(table.Rows.Cast(Of DataRow)().All(Function(r) CLng(r("patient_id")) = idPatient))
    End Sub

    <TestMethod()> Public Sub GetAllContreIndicationSubstancebyPatient_SousWeb_CommeLaSynthesePortail()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur)
        ' SyntheseController passe par PatientDao.GetStringContreIndicationByPatient, qui lit cette liste.
        UtiliserCompte(Compte.Web)

        CollectionAssert.AreEqual(New Long() {111}, SubstancesActives(idPatient))
    End Sub

    <TestMethod()> Public Sub GetAllContreIndicationSubstancebyPatient_InactifNull_EstRetenue()
        If Not ColonneNullableTheriaque("oasis.oa_patient_contre_indication_substance", "inactif") Then
            Assert.Inconclusive("oa_patient_contre_indication_substance.inactif est NOT NULL dans ce schéma.")
        End If
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Declarer(idPatient, 111, "SUBSTANCE A", idUtilisateur)
        EffacerInactifTheriaque("oasis.oa_patient_contre_indication_substance", "contre_indication_id",
                                IdContreIndication(idPatient, 111))

        CollectionAssert.AreEqual(New Long() {111}, SubstancesActives(idPatient))
    End Sub

    <TestMethod()> Public Sub GetAllContreIndicationSubstancebyPatient_SansContreIndication_TableVide()
        Dim idPatient = CreerPatient()
        Assert.AreEqual(0, dao.GetAllContreIndicationSubstancebyPatient(CInt(idPatient)).Rows.Count)
    End Sub

    ' --- Lecture par id ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetContreIndicationSubstanceById_SubstancePere_LitLePere()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        CreerContreIndicationSubstancePere(idPatient, 900, "FAMILLE TEST", idUtilisateur)

        Dim relue = dao.GetContreIndicationSubstanceById(IdContreIndication(idPatient, 0))

        Assert.AreEqual(0L, relue.SubstanceId)
        Assert.AreEqual(900L, relue.SubstancePereId)
        Assert.AreEqual("", relue.DenominationSubstance)
        Assert.AreEqual("FAMILLE TEST", relue.DenominationSubstancePere)
    End Sub

    <TestMethod()> Public Sub GetContreIndicationSubstanceById_Inexistante_LeveArgumentException()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetContreIndicationSubstanceById(ContreIndicationAbsente))
        Assert.AreEqual("Contre-indication substance inexistante !", erreur.Message)
    End Sub

    ' --- Annulation ---------------------------------------------------------------------------

    <TestMethod()> Public Sub AnnulationContreIndicationSubstance_MarqueInactiveEtTraceLAuteur()
        Dim idCreateur = CreerUtilisateur(avecCle:=False)
        Dim idAnnuleur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Declarer(idPatient, 111, "SUBSTANCE A", idCreateur)
        Declarer(idPatient, 222, "SUBSTANCE B", idCreateur)
        Dim idCiA = IdContreIndication(idPatient, 111)

        Assert.IsTrue(dao.AnnulationContreIndicationSubstance(CInt(idCiA), Auteur(idAnnuleur)))

        Dim relue = dao.GetContreIndicationSubstanceById(idCiA)
        Assert.IsTrue(relue.Inactif)
        Assert.AreEqual(idAnnuleur, relue.UserAnnulation)
        Assert.AreEqual(Date.Today, relue.DateAnnulation.Date)
        CollectionAssert.AreEqual(New Long() {222}, SubstancesActives(idPatient))
    End Sub

    <TestMethod()> Public Sub AnnulationContreIndicationSubstance_Inexistante_RenvoieFalse()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Assert.IsFalse(dao.AnnulationContreIndicationSubstance(CInt(ContreIndicationAbsente), Auteur(idUtilisateur)))
    End Sub

End Class
