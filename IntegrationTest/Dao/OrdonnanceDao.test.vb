Imports System.Globalization
Imports System.Threading
Imports Nethereum.Signer
Imports Oasis_Common

''' <summary>
''' OrdonnanceDao contre la base de test. Le client lourd crée, modifie, annule et
''' signe les ordonnances : ces appels tournent sous oasis_client. Seule la recherche
''' par signature sert au serveur (page publique /Sign/Check) et tourne sous
''' oasis_web.
''' </summary>
<TestClass()> Public Class OrdonnanceDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New OrdonnanceDao
    Private ReadOnly daoDetail As New OrdonnanceDetailDao

    Private Const OrdonnanceAbsente As Long = 987654321

    <TestCleanup>
    Public Sub RetirerLeCrochetDeSignature()
        Utilisateur.SignataireDistant = Nothing
    End Sub

    Private Function Lignes(idOrdonnance As Long) As List(Of OrdonnanceDetail)
        Return daoDetail.GetOrdonnanceLigneByOrdonnanceId(CInt(idOrdonnance))
    End Function

    Private Shared Function Adresse(idUtilisateur As Long) As String
        Return CStr(Scalaire("SELECT cle_publique FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", idUtilisateur))
    End Function

    ' --- Création et lecture -------------------------------------------------------

    <TestMethod()> Public Sub UneOrdonnanceCreeeEstRelueAvecSesValeursInitiales()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()

        Dim idOrdonnance As Long = dao.CreateOrdonnance(idPatient, 4242, New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)})

        Assert.IsTrue(idOrdonnance > 0)
        Dim relue = dao.GetOrdonnaceById(idOrdonnance)
        Assert.AreEqual(idOrdonnance, relue.Id)
        Assert.AreEqual(idPatient, relue.PatientId)
        Assert.AreEqual(4242L, relue.EpisodeId)
        Assert.AreEqual(idUtilisateur, relue.UtilisateurCreation)
        Assert.AreEqual(Date.Today, relue.DateCreation.Date)
        Assert.AreEqual(Date.MinValue, relue.DateValidation, "pas encore signée")
        Assert.AreEqual(0L, relue.UserValidation)
        Assert.AreEqual(Date.MinValue, relue.DateEdition)
        Assert.AreEqual("", relue.Commentaire)
        Assert.AreEqual(0, relue.Renouvellement)
        Assert.IsFalse(relue.Inactif)
        Assert.AreEqual("", relue.Signature)
        Assert.IsNull(relue.SignaturePayload)
        Assert.AreEqual("", relue.SignatureAdresse)
        Assert.AreEqual(0, Lignes(idOrdonnance).Count)
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentException))>
    Public Sub UneOrdonnanceInexistanteLeveUneErreur()
        dao.GetOrdonnaceById(OrdonnanceAbsente)
    End Sub

    <TestMethod()> Public Sub LesOrdonnancesDUnPatientVontDeLaPlusRecenteALaPlusAncienne()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        Dim premiere = CreerOrdonnance(idPatient, idUtilisateur, nbLignes:=0)
        Dim deuxieme = CreerOrdonnance(idPatient, idUtilisateur, nbLignes:=0)
        CreerOrdonnance(idAutrePatient, idUtilisateur, nbLignes:=0)
        Dim troisieme = CreerOrdonnance(idPatient, idUtilisateur, nbLignes:=0)

        Dim liste = dao.GetAllOrdonnanceByPatient(idPatient)

        CollectionAssert.AreEqual(New Long() {troisieme, deuxieme, premiere}, liste.Select(Function(o) o.Id).ToArray())
        Assert.IsTrue(liste.All(Function(o) o.PatientId = idPatient))
    End Sub

    <TestMethod()> Public Sub UnPatientSansOrdonnanceDonneUneListeVide()
        Dim idPatient = CreerPatient()
        Assert.AreEqual(0, dao.GetAllOrdonnanceByPatient(idPatient).Count)
    End Sub

    <TestMethod()> Public Sub SeulesLesOrdonnancesActivesDeLEpisodeSontRetenues()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAutrePatient = CreerPatient("AUTRE", "Patient")
        Dim active = CreerOrdonnance(idPatient, idUtilisateur, nbLignes:=0, episodeId:=101)
        Dim annulee = CreerOrdonnance(idPatient, idUtilisateur, nbLignes:=0, episodeId:=101)
        CreerOrdonnance(idPatient, idUtilisateur, nbLignes:=0, episodeId:=102)
        CreerOrdonnance(idAutrePatient, idUtilisateur, nbLignes:=0, episodeId:=101)
        dao.AnnulerOrdonnance(annulee)

        Dim liste = dao.GetOrdonnanceValideByPatient(idPatient, 101)

        CollectionAssert.AreEqual(New Long() {active}, liste.Select(Function(o) o.Id).ToArray())
    End Sub

    ' --- Modifications avant signature ---------------------------------------------

    <TestMethod()> Public Sub LeCommentaireEstEnregistreSansEspacesAutour()
        Dim idOrdonnance = CreerOrdonnance(CreerPatient(), CreerUtilisateur(avecCle:=False), nbLignes:=0)

        dao.ModificationOrdonnanceCommentaire(idOrdonnance, "  A renouveler  ")

        Assert.AreEqual("A renouveler", dao.GetOrdonnaceById(idOrdonnance).Commentaire)
    End Sub

    <TestMethod()> Public Sub LeRenouvellementEstEnregistre()
        Dim idOrdonnance = CreerOrdonnance(CreerPatient(), CreerUtilisateur(avecCle:=False), nbLignes:=0)

        dao.ModificationOrdonnanceRenouvellement(idOrdonnance, 3)

        Assert.AreEqual(3, dao.GetOrdonnaceById(idOrdonnance).Renouvellement)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidOperationException))>
    Public Sub ModifierLeCommentaireDUneOrdonnanceInexistanteLeveUneErreur()
        ' Aucune ligne touchée : le DAO ne distingue pas ce cas d'une ordonnance signée.
        dao.ModificationOrdonnanceCommentaire(OrdonnanceAbsente, "x")
    End Sub

    <TestMethod()> Public Sub LeCommentaireDUneOrdonnanceSigneeNEstPlusModifiable()
        Dim idUtilisateur = CreerUtilisateur()
        Dim idOrdonnance = CreerOrdonnance(CreerPatient(), idUtilisateur)
        dao.ModificationOrdonnanceCommentaire(idOrdonnance, "Avant signature")
        SignerCommeLeClient(idOrdonnance, idUtilisateur)

        Try
            dao.ModificationOrdonnanceCommentaire(idOrdonnance, "Apres signature")
            Assert.Fail("Le commentaire fait partie de la charge signée : il ne doit plus changer.")
        Catch ex As InvalidOperationException
        End Try

        Dim relue = dao.GetOrdonnaceById(idOrdonnance)
        Assert.AreEqual("Avant signature", relue.Commentaire)
        Assert.AreEqual(VerificationSignature.ResultatVerification.Valide, VerificationSignature.Verifier(relue))
    End Sub

    <TestMethod()> Public Sub LeRenouvellementDUneOrdonnanceSigneeNEstPlusModifiable()
        Dim idUtilisateur = CreerUtilisateur()
        Dim idOrdonnance = CreerOrdonnance(CreerPatient(), idUtilisateur)
        dao.ModificationOrdonnanceRenouvellement(idOrdonnance, 2)
        SignerCommeLeClient(idOrdonnance, idUtilisateur)

        Try
            dao.ModificationOrdonnanceRenouvellement(idOrdonnance, 5)
            Assert.Fail("Le renouvellement fait partie de la charge signée : il ne doit plus changer.")
        Catch ex As InvalidOperationException
        End Try

        Assert.AreEqual(2, dao.GetOrdonnaceById(idOrdonnance).Renouvellement)
    End Sub

    ' --- Annulation ----------------------------------------------------------------

    <TestMethod()> Public Sub UneOrdonnanceAnnuleeEstInactiveEtGardeSesLignes()
        Dim idOrdonnance = CreerOrdonnance(CreerPatient(), CreerUtilisateur(avecCle:=False), avecTraitements:=False)

        dao.AnnulerOrdonnance(idOrdonnance)

        Assert.IsTrue(dao.GetOrdonnaceById(idOrdonnance).Inactif)
        Assert.AreEqual(2, Lignes(idOrdonnance).Count)
    End Sub

    <TestMethod()> Public Sub UneOrdonnanceSigneePuisAnnuleeResteVerifiable()
        ' L'indicateur inactif ne fait pas partie de la charge : l'annuler ne touche pas la signature.
        Dim idUtilisateur = CreerUtilisateur()
        Dim idOrdonnance = CreerOrdonnanceSignee(CreerPatient(), idUtilisateur)

        dao.AnnulerOrdonnance(idOrdonnance)

        Dim relue = dao.GetOrdonnaceById(idOrdonnance)
        Assert.IsTrue(relue.Inactif)
        Assert.AreEqual(VerificationSignature.ResultatVerification.Valide, VerificationSignature.Verifier(relue))
    End Sub

    ' --- Signature -----------------------------------------------------------------

    <TestMethod()> Public Sub LaSignatureEnregistreLaDateLeSignataireEtSonAdresse()
        Dim idUtilisateur = CreerUtilisateur()
        Dim idOrdonnance = CreerOrdonnance(CreerPatient(), idUtilisateur)

        SignerCommeLeClient(idOrdonnance, idUtilisateur)

        Dim relue = dao.GetOrdonnaceById(idOrdonnance)
        Assert.AreEqual(Date.Today, relue.DateValidation, "tronquée au jour, pour que l'empreinte soit reproductible")
        Assert.AreEqual(Date.Today, relue.DateEdition.Date)
        Assert.AreEqual(idUtilisateur, relue.UserValidation)
        StringAssert.StartsWith(relue.Signature, "0x")
        Assert.AreEqual(132, relue.Signature.Length, "r, s et v en hexadécimal")
        Assert.AreEqual(Adresse(idUtilisateur), relue.SignatureAdresse)
        Assert.IsNotNull(relue.SignaturePayload)
        Assert.IsTrue(relue.SignaturePayload.Length > 0)
    End Sub

    <TestMethod()> Public Sub LePosteSigneParLeServeurSansDisposerDeLaCle()
        Dim idUtilisateur = CreerUtilisateur()
        Dim idOrdonnance = CreerOrdonnance(CreerPatient(), idUtilisateur)
        Dim serveur = SignataireServeur(idUtilisateur)
        Dim appels = 0
        Utilisateur.SignataireDistant =
            Function(charge As Byte()) As SignatureResponse
                appels += 1
                Return New SignatureResponse With {.Signature = serveur.Sign(charge), .Adresse = serveur.UtilisateurAddress}
            End Function
        Dim daoUtilisateur As New UserDao
        Dim poste = daoUtilisateur.GetUserById(CInt(idUtilisateur))
        Assert.AreEqual("", poste.UtilisateurClePrivee, "le poste ne lit jamais la clé privée")

        dao.ValidationOrdonnance(idOrdonnance, poste)

        Assert.AreEqual(1, appels)
        Dim relue = dao.GetOrdonnaceById(idOrdonnance)
        Assert.AreEqual(serveur.UtilisateurAddress, relue.SignatureAdresse)
        Assert.AreEqual(VerificationSignature.ResultatVerification.Valide, VerificationSignature.Verifier(relue))
    End Sub

    <TestMethod()> Public Sub SansCleNiServeurLaSignatureEchoueSansRienEcrire()
        Dim idUtilisateur = CreerUtilisateur()
        Dim idOrdonnance = CreerOrdonnance(CreerPatient(), idUtilisateur)
        Utilisateur.SignataireDistant = Nothing
        Dim daoUtilisateur As New UserDao
        Dim poste = daoUtilisateur.GetUserById(CInt(idUtilisateur))

        Try
            dao.ValidationOrdonnance(idOrdonnance, poste)
            Assert.Fail("Sans clé ni crochet, aucune signature ne doit être produite.")
        Catch ex As InvalidOperationException
        End Try

        Dim relue = dao.GetOrdonnaceById(idOrdonnance)
        Assert.AreEqual(Date.MinValue, relue.DateValidation)
        Assert.AreEqual(0L, relue.UserValidation)
        Assert.AreEqual("", relue.Signature)
        Assert.IsNull(relue.SignaturePayload)
    End Sub

    <TestMethod()> Public Sub LaChargeConserveeRedonneLOrdonnanceEtSesLignes()
        Dim idUtilisateur = CreerUtilisateur()
        Dim idOrdonnance = CreerOrdonnance(CreerPatient(), idUtilisateur, nbLignes:=3)
        dao.ModificationOrdonnanceCommentaire(idOrdonnance, "Pendant 1 mois")
        dao.ModificationOrdonnanceRenouvellement(idOrdonnance, 2)
        SignerCommeLeClient(idOrdonnance, idUtilisateur)

        Dim relue = dao.GetOrdonnaceById(idOrdonnance)
        Dim lignesEnBase = Lignes(idOrdonnance)
        Dim signee = OrdonnanceFull.Deserialize(relue.SignaturePayload)

        Assert.AreEqual(relue.Id, signee.Ordonnance.Id)
        Assert.AreEqual(relue.PatientId, signee.Ordonnance.PatientId)
        Assert.AreEqual(relue.EpisodeId, signee.Ordonnance.EpisodeId)
        Assert.AreEqual(relue.UtilisateurCreation, signee.Ordonnance.UtilisateurCreation)
        Assert.AreEqual(relue.DateCreation, signee.Ordonnance.DateCreation)
        Assert.AreEqual(relue.DateValidation, signee.Ordonnance.DateValidation)
        Assert.AreEqual(idUtilisateur, signee.Ordonnance.UserValidation)
        Assert.AreEqual("Pendant 1 mois", signee.Ordonnance.Commentaire)
        Assert.AreEqual(2, signee.Ordonnance.Renouvellement)

        Assert.AreEqual(3, signee.Details.Count)
        For i = 0 To lignesEnBase.Count - 1
            Assert.AreEqual(lignesEnBase(i).TraitementId, signee.Details(i).TraitementId, "ligne " & i)
            Assert.AreEqual(lignesEnBase(i).MedicamentDci, signee.Details(i).MedicamentDci, "ligne " & i)
            Assert.AreEqual(lignesEnBase(i).Posologie, signee.Details(i).Posologie, "ligne " & i)
            CollectionAssert.AreEqual(lignesEnBase(i).Serialize(), signee.Details(i).Serialize(), "ligne " & i)
        Next

        ' Le contenu relu en base, resérialisé, redonne exactement les octets signés.
        Dim contenu As New OrdonnanceFull With {.Ordonnance = relue, .Details = lignesEnBase}
        CollectionAssert.AreEqual(relue.SignaturePayload, contenu.Serialize())
    End Sub

    <TestMethod()> Public Sub LaSignatureConserveeEstVerifiee()
        Dim idUtilisateur = CreerUtilisateur()
        Dim idOrdonnance = CreerOrdonnanceSignee(CreerPatient(), idUtilisateur)

        Dim relue = dao.GetOrdonnaceById(idOrdonnance)

        Assert.AreEqual(VerificationSignature.ResultatVerification.Valide, VerificationSignature.Verifier(relue))
        Assert.IsNotNull(VerificationSignature.OrdonnanceSignee(relue))
    End Sub

    <TestMethod()> Public Sub UneOrdonnanceSansLigneSeSigneEtSeVerifie()
        Dim idUtilisateur = CreerUtilisateur()
        Dim idOrdonnance = CreerOrdonnanceSignee(CreerPatient(), idUtilisateur, nbLignes:=0)

        Dim relue = dao.GetOrdonnaceById(idOrdonnance)

        Assert.AreEqual(VerificationSignature.ResultatVerification.Valide, VerificationSignature.Verifier(relue))
        Assert.AreEqual(0, VerificationSignature.OrdonnanceSignee(relue).Details.Count)
    End Sub

    <TestMethod()> Public Sub UneLigneModifieeApresSignatureNeCorrespondPlusALaSignature()
        Dim idUtilisateur = CreerUtilisateur()
        Dim idOrdonnance = CreerOrdonnanceSignee(CreerPatient(), idUtilisateur)
        Dim premiere = Lignes(idOrdonnance)(0)

        ' Le DAO accepte de modifier une ligne d'ordonnance signée.
        daoDetail.ModificationOrdonnanceDetail(premiere.LigneId, "", 90, " 2. 0. 2")

        Dim relue = dao.GetOrdonnaceById(idOrdonnance)
        Dim contenu As New OrdonnanceFull With {.Ordonnance = relue, .Details = Lignes(idOrdonnance)}
        Dim chargeVivante = contenu.Serialize()
        CollectionAssert.AreNotEqual(relue.SignaturePayload, chargeVivante)
        Assert.AreEqual(VerificationSignature.ResultatVerification.Invalide,
                        VerificationSignature.Verifier(New Ordonnance With {
                            .Signature = relue.Signature,
                            .SignaturePayload = chargeVivante,
                            .SignatureAdresse = relue.SignatureAdresse
                        }))

        ' La charge conservée reste valide et montre la ligne telle qu'elle a été signée.
        Assert.AreEqual(VerificationSignature.ResultatVerification.Valide, VerificationSignature.Verifier(relue))
        Dim signee = VerificationSignature.OrdonnanceSignee(relue)
        Assert.AreEqual(" 1. 0. 1", signee.Details(0).Posologie)
        Assert.AreEqual(30, signee.Details(0).Duree)
    End Sub

    <TestMethod()> Public Sub UneChargeAltereeEnBaseInvalideLaSignature()
        Dim idUtilisateur = CreerUtilisateur()
        Dim idOrdonnance = CreerOrdonnanceSignee(CreerPatient(), idUtilisateur)
        Dim alteree = CType(dao.GetOrdonnaceById(idOrdonnance).SignaturePayload.Clone(), Byte())
        alteree(alteree.Length - 1) = CByte(alteree(alteree.Length - 1) Xor 1)

        Executer("UPDATE oasis.oa_patient_ordonnance SET oa_ordonnance_signature_payload = @p0 WHERE oa_ordonnance_id = @p1",
                 alteree, idOrdonnance)

        Assert.AreEqual(VerificationSignature.ResultatVerification.Invalide,
                        VerificationSignature.Verifier(dao.GetOrdonnaceById(idOrdonnance)))
    End Sub

    <TestMethod()> Public Sub UneAdresseRemplaceeEnBaseInvalideLaSignature()
        Dim idUtilisateur = CreerUtilisateur()
        Dim idOrdonnance = CreerOrdonnanceSignee(CreerPatient(), idUtilisateur)

        Executer("UPDATE oasis.oa_patient_ordonnance SET oa_ordonnance_signature_adresse = @p0 WHERE oa_ordonnance_id = @p1",
                 EthECKey.GenerateKey().GetPublicAddress(), idOrdonnance)

        Assert.AreEqual(VerificationSignature.ResultatVerification.Invalide,
                        VerificationSignature.Verifier(dao.GetOrdonnaceById(idOrdonnance)))
    End Sub

    ' --- Recherche par signature (serveur, /Sign/Check) ----------------------------

    <TestMethod()> Public Sub LeServeurRetrouveChaqueOrdonnanceParSaSignature()
        Dim idUtilisateur = CreerUtilisateur()
        Dim idPatient = CreerPatient()
        Dim premiere = CreerOrdonnanceSignee(idPatient, idUtilisateur)
        Dim seconde = CreerOrdonnanceSignee(idPatient, idUtilisateur)
        Dim signaturePremiere = dao.GetOrdonnaceById(premiere).Signature
        Dim signatureSeconde = dao.GetOrdonnaceById(seconde).Signature
        Assert.AreNotEqual(signaturePremiere, signatureSeconde)

        UtiliserCompte(Compte.Web)
        Dim trouvee = dao.GetOrdonnaceBySignature(signaturePremiere)
        Dim autreTrouvee = dao.GetOrdonnaceBySignature(signatureSeconde)

        Assert.AreEqual(premiere, trouvee.Id)
        Assert.AreEqual(seconde, autreTrouvee.Id)
        Assert.AreEqual(idPatient, trouvee.PatientId)
        Assert.AreEqual(Adresse(idUtilisateur), trouvee.SignatureAdresse)
        Assert.AreEqual(VerificationSignature.ResultatVerification.Valide, VerificationSignature.Verifier(trouvee))
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentException))>
    Public Sub UneSignatureInconnueLeveUneErreur()
        UtiliserCompte(Compte.Web)
        dao.GetOrdonnaceBySignature("0x" & New String("0"c, 130))
    End Sub

    ' --- Lignes générées depuis les traitements en cours ---------------------------

    <TestMethod()> Public Sub SansTraitementEnCoursAucuneLigneNEstGeneree()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idOrdonnance = CreerOrdonnance(idPatient, idUtilisateur, nbLignes:=0)

        dao.CreateNewOrdonnanceDetail(idPatient, idOrdonnance,
                                      New Episode With {.TypeActivite = Episode.EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE})

        Assert.AreEqual(0, Lignes(idOrdonnance).Count)
    End Sub

    <TestMethod()> Public Sub UnTraitementEnCoursDevientUneLigneDOrdonnance()
        ' Les fenêtres thérapeutiques absentes sont remplacées par la chaîne
        ' "31/12/2999", convertie en date selon la culture courante : le client lourd
        ' tourne en français, le test aussi.
        Dim cultureInitiale = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
        Try
            Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
            Dim idPatient = CreerPatient()
            Dim idTraitement = CreerTraitement(idPatient, idUtilisateur, 1)
            Dim idOrdonnance = CreerOrdonnance(idPatient, idUtilisateur, nbLignes:=0)

            dao.CreateNewOrdonnanceDetail(idPatient, idOrdonnance,
                                          New Episode With {.TypeActivite = Episode.EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE})

            Dim generees = Lignes(idOrdonnance)
            Assert.AreEqual(1, generees.Count)
            Dim ligne = generees(0)
            Assert.IsTrue(ligne.Traitement)
            Assert.AreEqual(CInt(idTraitement), ligne.TraitementId)
            Assert.AreEqual(CisDeTest + 1, ligne.MedicamentCis)
            Assert.AreEqual("DCI TEST 1", ligne.MedicamentDci)
            Assert.AreEqual(1, ligne.OrdreAffichage)
            Assert.AreEqual(Date.Today, ligne.DateDebut)
            Assert.AreEqual(Date.Today.AddDays(60), ligne.DateFin)
            Assert.AreEqual(30, ligne.Duree, "plafonnée à 30 jours")
            Assert.AreEqual(" 1. 0. 1", ligne.Posologie)
            Assert.AreEqual(Traitement.EnumBaseCode.JOURNALIER, ligne.PosologieBase)
            Assert.AreEqual(1, ligne.PosologieMatin)
            Assert.AreEqual(1, ligne.PosologieSoir)
            Assert.IsTrue(ligne.ADelivrer, "suivi chronique, traitement débutant ce jour")
            Assert.IsFalse(ligne.Ald, "patient sans ALD")
            Assert.IsFalse(ligne.Fenetre)
            Assert.AreEqual(FenetreAbsente, ligne.FenetreDateDebut)
            Assert.AreEqual(FenetreAbsente, ligne.FenetreDateFin)
            Assert.IsFalse(ligne.Inactif)
        Finally
            Thread.CurrentThread.CurrentCulture = cultureInitiale
        End Try
    End Sub

End Class
