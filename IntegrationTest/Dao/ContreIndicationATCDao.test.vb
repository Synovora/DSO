Imports Oasis_Common

''' <summary>
''' ContreIndicationATCDao contre la base de test. Toutes ses méthodes sont appelées
''' par le client lourd (RadF_CI_ATC_Selecteur, RadFPatientContreIndicationListe, et
''' TheriaqueDao.IsSpecialiteContreIndique, le contrôle fait à la prescription) :
''' elles tournent sous oasis_client. La liste sert aussi à la synthèse du portail
''' (SyntheseController, par PatientDao) : elle est aussi lue sous oasis_web.
'''
''' Le contrôle de prescription retient un médicament dont le code ATC commence par
''' l'un des codes de cette liste : une classe entière (N02) couvre tous ses
''' membres (N02BE01). Ce rapprochement par préfixe se fait en VB, après lecture des
''' codes ATC du médicament dans Theriak ; ici, on vérifie que la liste contient les
''' codes actifs de ce patient, et eux seuls, tels qu'ils ont été saisis.
''' </summary>
<TestClass()> Public Class ContreIndicationATCDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New ContreIndicationATCDao

    Private Const ContreIndicationAbsente As Long = 987654321

    Private Shared Function Auteur(idUtilisateur As Long) As Utilisateur
        Return New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}
    End Function

    Private Function Declarer(idPatient As Long, codeAtc As String, denomination As String, idUtilisateur As Long) As Boolean
        Return dao.CreationContreIndicationATC(
            New ContreIndicationATC With {.PatientId = idPatient, .ATCId = codeAtc, .DenominationATC = denomination},
            Auteur(idUtilisateur))
    End Function

    Private Shared Function IdContreIndication(idPatient As Long, codeAtc As String) As Long
        Return CLng(Scalaire("SELECT MAX(contre_indication_id) FROM oasis.oa_patient_contre_indication_atc" &
                             " WHERE patient_id = @p0 AND code_atc = @p1", idPatient, codeAtc))
    End Function

    Private Shared Function NombreLignes(idPatient As Long) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_patient_contre_indication_atc WHERE patient_id = @p0", idPatient))
    End Function

    Private Function CodesActifs(idPatient As Long) As String()
        Dim table = dao.getAllContreIndicationATCbyPatient(CInt(idPatient))
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CStr(r("code_atc"))).ToArray()
    End Function

    ' --- Création ---------------------------------------------------------------------

    <TestMethod()> Public Sub CreationContreIndicationATC_EnregistreEtRelit()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()

        Assert.IsTrue(Declarer(idPatient, "N02BE01", "PARACETAMOL", idUtilisateur))

        Dim relue = dao.GetContreIndicationATCById(IdContreIndication(idPatient, "N02BE01"))
        Assert.AreEqual(idPatient, relue.PatientId)
        Assert.AreEqual("N02BE01", relue.ATCId)
        Assert.AreEqual("PARACETAMOL", relue.DenominationATC)
        Assert.AreEqual(idUtilisateur, relue.UserCreation)
        Assert.AreEqual(Date.Today, relue.DateCreation.Date)
        Assert.AreEqual(0L, relue.UserAnnulation)
        Assert.AreEqual(Date.MinValue, relue.DateAnnulation)
        Assert.IsFalse(relue.Inactif)
    End Sub

    <TestMethod()> Public Sub CreationContreIndicationATC_DejaActive_RenvoieFalseSansDoublon()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Declarer(idPatient, "N02BE01", "PARACETAMOL", idUtilisateur)

        Assert.IsFalse(Declarer(idPatient, "N02BE01", "PARACETAMOL BIS", idUtilisateur))

        Assert.AreEqual(1, NombreLignes(idPatient))
    End Sub

    <TestMethod()> Public Sub CreationContreIndicationATC_ClasseEtSousClasse_SontDeuxLignes()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Declarer(idPatient, "N02", "ANALGESIQUES", idUtilisateur)

        ' Le doublon se juge sur le code exact : une classe déjà contre-indiquée
        ' n'empêche pas d'en contre-indiquer un membre.
        Assert.IsTrue(Declarer(idPatient, "N02BE01", "PARACETAMOL", idUtilisateur))

        CollectionAssert.AreEqual(New String() {"N02", "N02BE01"}, CodesActifs(idPatient))
    End Sub

    <TestMethod()> Public Sub CreationContreIndicationATC_ApresAnnulation_RecreeUneLigneActive()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Declarer(idPatient, "N02BE01", "PARACETAMOL", idUtilisateur)
        dao.AnnulationContreIndicationATC(CInt(IdContreIndication(idPatient, "N02BE01")), Auteur(idUtilisateur))

        Assert.IsTrue(Declarer(idPatient, "N02BE01", "PARACETAMOL", idUtilisateur))

        Assert.AreEqual(2, NombreLignes(idPatient))
        CollectionAssert.AreEqual(New String() {"N02BE01"}, CodesActifs(idPatient))
    End Sub

    <TestMethod()> Public Sub CreationContreIndicationATC_MemeCodeChezUnAutrePatient_EstCreee()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAutre = CreerPatient("AUTRE", "Patient")
        Declarer(idPatient, "N02BE01", "PARACETAMOL", idUtilisateur)

        Assert.IsTrue(Declarer(idAutre, "N02BE01", "PARACETAMOL", idUtilisateur))

        Assert.AreEqual(1, NombreLignes(idAutre))
    End Sub

    ' --- Liste d'un patient -------------------------------------------------------------

    <TestMethod()> Public Sub getAllContreIndicationATCbyPatient_ActivesDuPatient_TrieesParCode()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAutre = CreerPatient("AUTRE", "Patient")
        Declarer(idPatient, "N02BE01", "PARACETAMOL", idUtilisateur)
        Declarer(idPatient, "C01", "THERAPEUTIQUE CARDIAQUE", idUtilisateur)
        Declarer(idPatient, "J01CA04", "AMOXICILLINE", idUtilisateur)
        dao.AnnulationContreIndicationATC(CInt(IdContreIndication(idPatient, "J01CA04")), Auteur(idUtilisateur))
        Declarer(idAutre, "A01AA01", "AUTRE PATIENT", idUtilisateur)

        Dim table = dao.getAllContreIndicationATCbyPatient(CInt(idPatient))

        CollectionAssert.AreEqual(New String() {"C01", "N02BE01"}, CodesActifs(idPatient))
        Assert.AreEqual("THERAPEUTIQUE CARDIAQUE", CStr(table.Rows(0)("denomination_atc")))
        Assert.IsTrue(table.Rows.Cast(Of DataRow)().All(Function(r) CLng(r("patient_id")) = idPatient))
    End Sub

    <TestMethod()> Public Sub getAllContreIndicationATCbyPatient_SousWeb_CommeLaSynthesePortail()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Declarer(idPatient, "N02BE01", "PARACETAMOL", idUtilisateur)
        ' SyntheseController passe par PatientDao.GetStringContreIndicationByPatient, qui lit cette liste.
        UtiliserCompte(Compte.Web)

        CollectionAssert.AreEqual(New String() {"N02BE01"}, CodesActifs(idPatient))
    End Sub

    <TestMethod()> Public Sub getAllContreIndicationATCbyPatient_InactifNull_EstRetenue()
        If Not ColonneNullableTheriaque("oasis.oa_patient_contre_indication_atc", "inactif") Then
            Assert.Inconclusive("oa_patient_contre_indication_atc.inactif est NOT NULL dans ce schéma.")
        End If
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Declarer(idPatient, "N02BE01", "PARACETAMOL", idUtilisateur)
        EffacerInactifTheriaque("oasis.oa_patient_contre_indication_atc", "contre_indication_id",
                                IdContreIndication(idPatient, "N02BE01"))

        CollectionAssert.AreEqual(New String() {"N02BE01"}, CodesActifs(idPatient))
    End Sub

    <TestMethod()> Public Sub getAllContreIndicationATCbyPatient_SansContreIndication_TableVide()
        Dim idPatient = CreerPatient()
        Assert.AreEqual(0, dao.getAllContreIndicationATCbyPatient(CInt(idPatient)).Rows.Count)
    End Sub

    ' --- Lecture par id ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetContreIndicationATCById_Inexistante_LeveArgumentException()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetContreIndicationATCById(ContreIndicationAbsente))
        Assert.AreEqual("Contre-indication ATC inexistante !", erreur.Message)
    End Sub

    ' --- Annulation ---------------------------------------------------------------------------

    <TestMethod()> Public Sub AnnulationContreIndicationATC_MarqueInactiveEtTraceLAuteur()
        Dim idCreateur = CreerUtilisateur(avecCle:=False)
        Dim idAnnuleur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Declarer(idPatient, "N02BE01", "PARACETAMOL", idCreateur)
        Declarer(idPatient, "C01", "THERAPEUTIQUE CARDIAQUE", idCreateur)
        Dim idCi = IdContreIndication(idPatient, "N02BE01")

        Assert.IsTrue(dao.AnnulationContreIndicationATC(CInt(idCi), Auteur(idAnnuleur)))

        Dim relue = dao.GetContreIndicationATCById(idCi)
        Assert.IsTrue(relue.Inactif)
        Assert.AreEqual(idAnnuleur, relue.UserAnnulation)
        Assert.AreEqual(Date.Today, relue.DateAnnulation.Date)
        CollectionAssert.AreEqual(New String() {"C01"}, CodesActifs(idPatient))
    End Sub

    <TestMethod()> Public Sub AnnulationContreIndicationATC_Inexistante_RenvoieFalse()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Assert.IsFalse(dao.AnnulationContreIndicationATC(CInt(ContreIndicationAbsente), Auteur(idUtilisateur)))
    End Sub

End Class
