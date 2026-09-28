Imports System.Web.Mvc
Imports Oasis_Common

''' <summary>
''' /Synthese : synthèse du dossier affichée à l'internaute du portail patient. Le
''' patient est résolu à partir de l'identité Forms (id de l'internaute), puis de sa
''' permission en base ; un internaute ne doit jamais voir le dossier d'un autre.
'''
''' L'action est appelée directement, la vue n'est pas rendue : les tests portent
''' sur le type de résultat et sur les listes posées dans ViewBag. Les données sont
''' créées par les DAO sous le compte du client lourd, l'action tourne sous celui du
''' serveur (DocumentsDeTest.ConsulterSyntheseDeTest). getAllPPSbyPatient et
''' GetAllParcoursbyPatient nomment la base en toutes lettres : tout test qui va
''' jusqu'aux listes commence par ExigerBaseOasis.
''' </summary>
<TestClass()> Public Class SyntheseControllerTest
    Inherits TestIntegration

    Private Shared Function Vue(resultat As ActionResult) As ViewResult
        Assert.IsInstanceOfType(resultat, GetType(ViewResult))
        Return DirectCast(resultat, ViewResult)
    End Function

    Private Shared Function Liste(page As ViewResult, cle As String) As List(Of List(Of String))
        Return DirectCast(page.ViewData(cle), List(Of List(Of String)))
    End Function

    ''' <summary>Valeurs de la colonne donnée dans une liste de la synthèse.</summary>
    Private Shared Function Colonne(page As ViewResult, cle As String, rang As Integer) As List(Of String)
        Return Liste(page, cle).Select(Function(ligne) ligne(rang)).ToList()
    End Function

    ''' <summary>Tout le texte affiché, toutes listes confondues.</summary>
    Private Shared Function TexteAffiche(page As ViewResult) As String
        Dim cellules As New List(Of String)
        For Each cle In {"Contexts", "Antecedents", "Traitements", "PPS", "PS", "Vaccins"}
            For Each ligne In Liste(page, cle)
                cellules.AddRange(ligne)
            Next
        Next
        cellules.Add(CStr(page.ViewData("Allergies")))
        cellules.Add(CStr(page.ViewData("ContreIndication")))
        Return String.Join("|", cellules)
    End Function

    Private Shared Sub AssertRefus(resultat As ActionResult)
        Assert.IsInstanceOfType(resultat, GetType(HttpStatusCodeResult))
        Dim refus = DirectCast(resultat, HttpStatusCodeResult)
        Assert.AreEqual(403, refus.StatusCode)
        Assert.AreEqual("Accès refusé", refus.StatusDescription)
    End Sub

    Private Shared Function Consulter(idInternaute As Long, Optional culture As String = "fr-FR") As ActionResult
        Return ConsulterSyntheseDeTest(PrincipalPortailDeTest(idInternaute.ToString()), culture:=culture)
    End Function

    ''' <summary>Note de vaccination par PatientNoteVaccinDao.CreationNote. Renvoie son id.</summary>
    Private Shared Function NoterVaccin(idPatient As Long, idUtilisateur As Long, note As String) As Long
        Dim dao As New PatientNoteVaccinDao
        dao.CreationNote(New PatientNote With {
            .PatientId = CInt(idPatient),
            .PatientNote = note,
            .UserCreation = CInt(idUtilisateur)
        }, New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)})
        Return CLng(Scalaire("SELECT MAX(oa_patient_note_id) FROM oasis.oa_patient_note_vaccin WHERE oa_patient_id = @p0", idPatient))
    End Function

    ''' <summary>Identifiants des éléments d'un dossier complet, pour les comparer d'une vue à l'autre.</summary>
    Private Class DossierSynthese
        Friend PatientId As Long
        Friend AntecedentId As Long
        Friend ContexteId As Long
        Friend TraitementId As Long
        Friend PpsId As Long
        Friend NoteId As Long
        Friend Suffixe As String
    End Class

    ''' <summary>
    ''' Un antécédent, un contexte, un traitement en cours, un objectif de santé et
    ''' une note de vaccination, dont les libellés finissent par « de {suffixe} ».
    ''' </summary>
    Private Shared Function DossierComplet(idUtilisateur As Long, suffixe As String) As DossierSynthese
        AssurerSousCategoriePps(1, 1, 1)
        Dim idPatient = CreerPatient("SYNTHESE", suffixe)
        Return New DossierSynthese With {
            .PatientId = idPatient,
            .Suffixe = suffixe,
            .AntecedentId = CreerAntecedent(idPatient, idUtilisateur, description:="Asthme de " & suffixe),
            .ContexteId = CreerContexteMedical(idPatient, idUtilisateur, description:="Contexte de " & suffixe),
            .TraitementId = EnregistrerTraitement(TraitementDeTest(idPatient, 1), idUtilisateur),
            .PpsId = CreerPps(idPatient, idUtilisateur, 1, 1, commentaire:="Objectif de " & suffixe),
            .NoteId = NoterVaccin(idPatient, idUtilisateur, "Rappel DTP de " & suffixe)
        }
    End Function

    ' --- Identité et permission ------------------------------------------------------

    <TestMethod()> Public Sub IndexExigeUneAuthentificationMvc()
        Dim methode = GetType(Global.Oasis_Web.Oasis_Web.Controllers.SyntheseController).GetMethod("Index")

        Assert.AreEqual(1, methode.GetCustomAttributes(GetType(AuthorizeAttribute), True).Length)
    End Sub

    <TestMethod()> Public Sub SansIdentiteLaSyntheseEstRefusee()
        AssertRefus(ConsulterSyntheseDeTest(Nothing))
        AssertRefus(ConsulterSyntheseDeTest(PrincipalPortailDeTest("")))
    End Sub

    <TestMethod()> Public Sub UneIdentiteNonNumeriqueEstRefusee()
        Dim idInternaute = CreerAccesPortailDeTest(CreerPatient())

        AssertRefus(ConsulterSyntheseDeTest(PrincipalPortailDeTest("admin")))
        AssertRefus(ConsulterSyntheseDeTest(PrincipalPortailDeTest(idInternaute & " OR 1=1")))
        AssertRefus(ConsulterSyntheseDeTest(PrincipalPortailDeTest(idInternaute & ";" & idInternaute)))
    End Sub

    <TestMethod()> Public Sub UnInternauteSansPermissionEstRefuse()
        Dim idInternaute = CreerInternaute()

        AssertRefus(Consulter(idInternaute))
    End Sub

    <TestMethod()> Public Sub UnInternauteInconnuEstRefuse()
        CreerAccesPortailDeTest(CreerPatient())

        AssertRefus(Consulter(987654321))
    End Sub

    ' --- Contenu de la synthèse ---------------------------------------------------------

    <TestMethod()> Public Sub LaSyntheseMontreLeDossierDuPatientRattache()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim dossier = DossierComplet(idUtilisateur, "Alice")
        Dim idInternaute = CreerAccesPortailDeTest(dossier.PatientId)

        Dim page = Vue(Consulter(idInternaute))

        Assert.AreEqual("", page.ViewName)
        Assert.AreEqual(dossier.PatientId, CLng(DirectCast(page.ViewData("Patient"), Patient).PatientId))

        Dim antecedents = Liste(page, "Antecedents")
        Assert.AreEqual(1, antecedents.Count)
        Assert.AreEqual("color: inherit;", antecedents(0)(0))
        Assert.AreEqual("Asthme de Alice", antecedents(0)(1))
        Assert.AreEqual(" Asthme de Alice", antecedents(0)(2), "niveau 1, diagnostic certain : ni retrait ni préfixe")
        Assert.AreEqual("", antecedents(0)(3), "pas d'ALD")
        Assert.AreEqual(dossier.AntecedentId.ToString(), antecedents(0)(4))

        Dim contextes = Liste(page, "Contexts")
        Assert.AreEqual(1, contextes.Count)
        Assert.AreEqual(ContexteCourrier.EnumParcoursBaseItem.Medical, contextes(0)(0))
        Assert.IsTrue(contextes(0)(1).EndsWith(" Contexte de Alice"), contextes(0)(1))
        Assert.AreEqual(dossier.ContexteId.ToString(), contextes(0)(2))

        Dim traitements = Liste(page, "Traitements")
        Assert.AreEqual(1, traitements.Count)
        Assert.AreEqual("DCI TEST 1", traitements(0)(0))
        Assert.AreEqual(" 1. 0. 1", traitements(0)(1))
        Assert.AreEqual("Commentaire 1", traitements(0)(2))
        Assert.AreEqual("Pendant le repas", traitements(0)(3))
        Assert.AreEqual("", traitements(0)(4), "pas de fenêtre thérapeutique")
        Assert.AreEqual(" " & Date.Today.ToString("dd.MM.yyyy"), traitements(0)(5))
        Assert.AreEqual(dossier.TraitementId.ToString(), traitements(0)(7))
        Assert.AreEqual((CisDeTest + 1).ToString(), traitements(0)(8))

        Dim objectifs = Liste(page, "PPS")
        Assert.AreEqual(1, objectifs.Count)
        Assert.AreEqual("Objectif santé :  Objectif de Alice", objectifs(0)(0))
        Assert.AreEqual(dossier.PpsId.ToString(), objectifs(0)(1))

        Dim vaccins = Liste(page, "Vaccins")
        Assert.AreEqual(1, vaccins.Count)
        Assert.AreEqual("Rappel DTP de Alice", vaccins(0)(0))
        Assert.AreEqual(dossier.NoteId.ToString(), vaccins(0)(1))
        Assert.IsTrue(vaccins(0)(2).StartsWith("Utilisateur TEST" & vbCrLf), vaccins(0)(2))

        Assert.AreEqual(0, Liste(page, "PS").Count)
        Assert.AreEqual("", CStr(page.ViewData("Allergies")))
        Assert.AreEqual("", CStr(page.ViewData("ContreIndication")))
    End Sub

    <TestMethod()> Public Sub UnInternauteNeVoitJamaisLeDossierDUnAutrePatient()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim alice = DossierComplet(idUtilisateur, "Alice")
        Dim bruno = DossierComplet(idUtilisateur, "Bruno")
        Dim internauteAlice = CreerAccesPortailDeTest(alice.PatientId)
        Dim internauteBruno = CreerAccesPortailDeTest(bruno.PatientId)

        For Each cas In {Tuple.Create(internauteAlice, alice, bruno), Tuple.Create(internauteBruno, bruno, alice)}
            Dim page = Vue(Consulter(cas.Item1))
            Dim lecteur = cas.Item2
            Dim autre = cas.Item3

            Assert.AreEqual(lecteur.PatientId, CLng(DirectCast(page.ViewData("Patient"), Patient).PatientId))
            CollectionAssert.AreEqual({lecteur.AntecedentId.ToString()}, Colonne(page, "Antecedents", 4))
            CollectionAssert.AreEqual({lecteur.ContexteId.ToString()}, Colonne(page, "Contexts", 2))
            CollectionAssert.AreEqual({lecteur.TraitementId.ToString()}, Colonne(page, "Traitements", 7))
            CollectionAssert.AreEqual({lecteur.PpsId.ToString()}, Colonne(page, "PPS", 1))
            CollectionAssert.AreEqual({lecteur.NoteId.ToString()}, Colonne(page, "Vaccins", 1))
            Dim texte = TexteAffiche(page)
            Assert.IsFalse(texte.Contains("de " & autre.Suffixe), "rien du dossier de " & autre.Suffixe & " : " & texte)
        Next
    End Sub

    <TestMethod()> Public Sub UnDossierVideDonneDesListesVides()
        ExigerBaseOasis()
        Dim idPatient = CreerPatient("SYNTHESE", "Vide")
        Dim idInternaute = CreerAccesPortailDeTest(idPatient)

        Dim page = Vue(Consulter(idInternaute))

        Assert.AreEqual(idPatient, CLng(DirectCast(page.ViewData("Patient"), Patient).PatientId))
        For Each cle In {"Contexts", "Antecedents", "Traitements", "PPS", "PS", "Vaccins"}
            Assert.AreEqual(0, Liste(page, cle).Count, cle)
        Next
        Assert.AreEqual("", CStr(page.ViewData("Allergies")))
        Assert.AreEqual("", CStr(page.ViewData("ContreIndication")))
    End Sub

    <TestMethod()> Public Sub LesElementsCachesArretesOuTerminesNeSontPasPublies()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient("SYNTHESE", "Masques")
        CreerAntecedent(idPatient, idUtilisateur, description:="Antécédent caché", statutAffichage:="C")
        CreerContexteMedical(idPatient, idUtilisateur, description:="Contexte caché", statutAffichage:="C")
        ArreterContexteMedical(CreerContexteMedical(idPatient, idUtilisateur, description:="Contexte arrêté"))
        EnregistrerTraitement(TraitementDeTest(idPatient, 2, Date.Today.AddDays(-30), Date.Today.AddDays(-1)), idUtilisateur)
        Dim idInternaute = CreerAccesPortailDeTest(idPatient)

        Dim page = Vue(Consulter(idInternaute))

        Assert.AreEqual(0, Liste(page, "Antecedents").Count)
        Assert.AreEqual(0, Liste(page, "Contexts").Count)
        Assert.AreEqual(0, Liste(page, "Traitements").Count)
    End Sub

    ' --- Mise en page ----------------------------------------------------------------

    <TestMethod()> Public Sub LaMiseEnPageParDefautEstVerticale()
        ExigerBaseOasis()
        Dim idInternaute = CreerAccesPortailDeTest(CreerPatient())

        Dim page = Vue(Consulter(idInternaute))

        Assert.AreEqual(Global.Oasis_Web.Oasis_Web.Constants.LAYOUT_VERTICAL, CStr(page.ViewData("ModeName")))
        Assert.AreEqual("Dashboard", CStr(page.ViewData("WelcomeText")))
    End Sub

    <TestMethod()> Public Sub LaMiseEnPageVientDeTempData()
        ExigerBaseOasis()
        Dim idInternaute = CreerAccesPortailDeTest(CreerPatient())

        Dim page = Vue(ConsulterSyntheseDeTest(PrincipalPortailDeTest(idInternaute.ToString()),
                                               modeAffichage:=Global.Oasis_Web.Oasis_Web.Constants.LAYOUT_HORIZONTAL,
                                               texteAccueil:="Bienvenue"))

        Assert.AreEqual(Global.Oasis_Web.Oasis_Web.Constants.LAYOUT_HORIZONTAL, CStr(page.ViewData("ModeName")))
        Assert.AreEqual("Bienvenue", CStr(page.ViewData("WelcomeText")))
    End Sub

    ' --- Comportements actuels à corriger ---------------------------------------------

    <TestMethod()> Public Sub UnParcoursDuPatientFaitApparaitreLesPpsDeSuiviDUnAutrePatient()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        AssurerSousCategoriePps(3, 3, 1)
        Dim idAlice = CreerPatient("SYNTHESE", "Alice")
        Dim idBruno = CreerPatient("SYNTHESE", "Bruno")
        Dim idParcoursAlice = CreerParcoursSyntheseDeTest(idAlice, idUtilisateur, 3, "Parcours IDE de Alice")
        Dim idPpsBruno = CreerPps(idBruno, idUtilisateur, 3, 3, commentaire:="Suivi IDE de Bruno")
        Dim idInternaute = CreerAccesPortailDeTest(idAlice)

        Dim page = Vue(Consulter(idInternaute))

        ' Le parcours d'Alice est bien le sien.
        CollectionAssert.AreEqual({idParcoursAlice.ToString()}, Colonne(page, "PS", 0))
        Assert.AreEqual("Parcours IDE de Alice", Liste(page, "PS")(0)(7))

        ' Comportement actuel : fuite entre dossiers. PpsDao.getAllPPSbyPatient joint
        ' oa_patient_pps et oa_patient_parcours sur la seule sous-catégorie, sans
        ' condition de patient dans les jointures, puis garde la ligne dès que le PPS
        ' OU le parcours appartient au patient. Le parcours IDE d'Alice ramène donc le
        ' PPS de suivi IDE de Bruno, commentaire compris, dans la synthèse d'Alice.
        CollectionAssert.AreEqual({idPpsBruno.ToString()}, Colonne(page, "PPS", 1))
        Assert.AreEqual("Suivi IDE : 1 / Par mois Suivi IDE de Bruno", Liste(page, "PPS")(0)(0))
    End Sub

    <TestMethod()> Public Sub UnContexteSansDateDeFinSAfficheEnFrancais()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient("SYNTHESE", "Culture")
        Dim idContexte = CreerContexteMedical(idPatient, idUtilisateur, description:="Contexte ancien")
        RetirerDateFinContexteDeTest(idContexte)
        Dim idInternaute = CreerAccesPortailDeTest(idPatient)

        Dim page = Vue(Consulter(idInternaute, "fr-FR"))

        CollectionAssert.AreEqual({idContexte.ToString()}, Colonne(page, "Contexts", 2))
    End Sub

    <TestMethod()> Public Sub UnContexteSansDateDeFinFaitEchouerLaSyntheseHorsCultureFrancaise()
        ExigerBaseOasis()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient("SYNTHESE", "Culture")
        RetirerDateFinContexteDeTest(CreerContexteMedical(idPatient, idUtilisateur, description:="Contexte ancien"))
        Dim idInternaute = CreerAccesPortailDeTest(idPatient)

        ' Comportement actuel : BuildContexte écrit dateFin = "31/12/9999", conversion
        ' implicite d'une chaîne selon la culture du thread. Web.config ne fixe pas de
        ' culture : sur un serveur qui n'est pas en français, la synthèse entière
        ' échoue dès qu'un contexte n'a pas de date de fin.
        Assert.ThrowsException(Of InvalidCastException)(Sub() Consulter(idInternaute, "en-US"))
    End Sub

End Class
