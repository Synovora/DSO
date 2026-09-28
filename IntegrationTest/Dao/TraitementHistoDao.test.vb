Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' TraitementHistoDao (module) contre la base de test. Le client lourd lit
''' l'historique d'un traitement (RadFTraitementHistoListe) et l'écrit directement
''' pour les fenêtres thérapeutiques (RadFTraitementFenetreTh) : tout tourne sous
''' oasis_client. Les appels passent par le nom du module pour ne pas dépendre des
''' noms des modules du harnais.
''' </summary>
<TestClass()> Public Class TraitementHistoDaoTest
    Inherits TestIntegration

    Private Const TraitementAbsent As Integer = 987654321

    Private Shared Function Auteur(idUtilisateur As Long) As Utilisateur
        Return New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}
    End Function

    Private Shared Function ValeurHisto(colonne As String, idTraitement As Long) As Object
        Return Scalaire("SELECT " & colonne & " FROM oasis.oa_traitement_histo WHERE oa_traitement_id = @p0", idTraitement)
    End Function

    Private Shared Function HistoriqueComplet(idPatient As Long, idTraitement As Integer) As TraitementHisto
        Return New TraitementHisto With {
            .HistorisationEtat = TraitementHistoDao.EnumEtatTraitementHisto.ModificationTraitement,
            .HistorisationUtilisateurId = 424242,
            .HistorisationPatientId = CInt(idPatient),
            .HistorisationTraitementId = idTraitement,
            .HistorisationMedicamentId = CisDeTest + 11,
            .HistorisationMedicamentDci = "DCI HISTO",
            .HistorisationDateDebut = Date.Today.AddDays(-7),
            .HistorisationDateFin = Date.Today.AddDays(21),
            .HistorisationCommentaire = "Commentaire histo",
            .HistorisationOrdreAffichage = 4,
            .HistorisationPosologieBase = Traitement.EnumBaseCode.MENSUEL,
            .HistorisationPosologieRythme = 1,
            .HistorisationPosologieMatin = 0,
            .HistorisationPosologieMidi = 1,
            .HistorisationPosologieApresMidi = 0,
            .HistorisationPosologieSoir = 0,
            .HistorisationFractionMatin = Traitement.EnumFraction.Non,
            .HistorisationFractionMidi = Traitement.EnumFraction.TroisQuart,
            .HistorisationFractionApresMidi = Traitement.EnumFraction.Non,
            .HistorisationFractionSoir = Traitement.EnumFraction.Non,
            .HistorisationPosologieCommentaire = "Le premier du mois",
            .HistorisationFenetre = True,
            .HistorisationFenetreDateDebut = Date.Today.AddDays(1),
            .HistorisationFenetreDateFin = Date.Today.AddDays(8),
            .HistorisationFenetreCommentaire = "Pause",
            .HistorisationArret = "",
            .HistorisationArretCommentaire = "",
            .HistorisationDeclaratifHorsTraitement = False,
            .HistorisationAllergie = False,
            .HistorisationContreIndication = True,
            .HistorisationAnnulation = "",
            .HistorisationAnnulationCommentaire = ""
        }
    End Function

    ' --- Lecture -----------------------------------------------------------------------

    <TestMethod()> Public Sub GetAllHistoTraitementbyId_DonneLHistoriqueDuPlusRecentAuPlusAncien()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim id = EnregistrerTraitement(TraitementDeTest(idPatient, 1), idUtilisateur)
        EnregistrerTraitement(TraitementDeTest(idPatient, 2), idUtilisateur)
        Dim dao As New TraitementDao
        Dim lu = dao.GetTraitementById(CInt(id))
        Dim histo As New TraitementHisto
        TraitementHistoDao.InitClasseTraitementHistorisation(lu, Auteur(idUtilisateur), histo)
        lu.PosologieSoir = 2
        lu.UserModification = CInt(idUtilisateur)
        lu.DateModification = Date.Now
        dao.ModificationTraitement(lu, histo, Auteur(idUtilisateur))
        lu.ArretCommentaire = "Fin"
        dao.ArretTraitement(lu, histo, Auteur(idUtilisateur))

        Dim table = TraitementHistoDao.GetAllHistoTraitementbyId(CInt(id))

        CollectionAssert.AreEqual(New Integer() {3, 2, 1},
                                  table.Rows.Cast(Of DataRow)().Select(Function(r) CInt(r("oa_traitement_histo_etat_historisation"))).ToArray())
        Assert.IsTrue(table.Rows.Cast(Of DataRow)().All(Function(r) CLng(r("oa_traitement_id")) = id))
        Assert.AreEqual(2, CInt(table.Rows(1)("oa_traitement_posologie_soir")), "la modification est historisée avec sa posologie")
        Assert.AreEqual("A", CStr(table.Rows(0)("oa_traitement_arret")))
    End Sub

    <TestMethod()> Public Sub GetAllHistoTraitementbyId_TraitementSansHistorique_TableVide()
        Assert.AreEqual(0, TraitementHistoDao.GetAllHistoTraitementbyId(TraitementAbsent).Rows.Count)
    End Sub

    ' --- Écriture ----------------------------------------------------------------------

    <TestMethod()> Public Sub CreationTraitementHisto_SousClient_EnregistreChaqueChamp()
        ' Id de traitement propre à ce test : l'historique ne contient que la ligne écrite ici.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idTraitement As Long = TraitementAbsent
        Dim histo = HistoriqueComplet(idPatient, TraitementAbsent)

        Assert.IsTrue(TraitementHistoDao.CreationTraitementHisto(histo, Auteur(idUtilisateur),
                                                                 TraitementHistoDao.EnumEtatTraitementHisto.CreationFenetreTherapeutique))

        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_traitement_histo WHERE oa_traitement_id = @p0", idTraitement)))
        ' L'état et l'auteur viennent des paramètres, pas de ceux portés par l'historique.
        Assert.AreEqual(5, CInt(ValeurHisto("oa_traitement_histo_etat_historisation", idTraitement)))
        Assert.AreEqual(CInt(idUtilisateur), CInt(ValeurHisto("oa_traitement_histo_utilisateur_historisation", idTraitement)))
        Assert.AreEqual(Date.Today, CDate(ValeurHisto("oa_traitement_histo_date_historisation", idTraitement)).Date)
        Assert.AreEqual(CInt(idPatient), CInt(ValeurHisto("oa_traitement_patient_id", idTraitement)))
        Assert.AreEqual(CisDeTest + 11, CInt(ValeurHisto("oa_traitement_medicament_cis", idTraitement)))
        Assert.AreEqual("DCI HISTO", CStr(ValeurHisto("oa_traitement_medicament_dci", idTraitement)))
        Assert.AreEqual(Date.Today.AddDays(-7), CDate(ValeurHisto("oa_traitement_date_debut", idTraitement)).Date)
        Assert.AreEqual(Date.Today.AddDays(21), CDate(ValeurHisto("oa_traitement_date_fin", idTraitement)).Date)
        Assert.AreEqual(4, CInt(ValeurHisto("oa_traitement_ordre_affichage", idTraitement)))
        Assert.AreEqual("M", CStr(ValeurHisto("oa_traitement_posologie_base", idTraitement)))
        Assert.AreEqual(1, CInt(ValeurHisto("oa_traitement_posologie_rythme", idTraitement)))
        Assert.AreEqual(1, CInt(ValeurHisto("oa_traitement_posologie_midi", idTraitement)))
        Assert.AreEqual("3/4", CStr(ValeurHisto("oa_traitement_fraction_midi", idTraitement)))
        Assert.AreEqual("Le premier du mois", CStr(ValeurHisto("oa_traitement_posologie_commentaire", idTraitement)))
        Assert.AreEqual("Commentaire histo", CStr(ValeurHisto("oa_traitement_commentaire", idTraitement)))
        Assert.IsTrue(CBool(ValeurHisto("oa_traitement_fenetre", idTraitement)))
        Assert.AreEqual(Date.Today.AddDays(1), CDate(ValeurHisto("oa_traitement_fenetre_date_debut", idTraitement)).Date)
        Assert.AreEqual(Date.Today.AddDays(8), CDate(ValeurHisto("oa_traitement_fenetre_date_fin", idTraitement)).Date)
        Assert.AreEqual("Pause", CStr(ValeurHisto("oa_traitement_fenetre_commentaire", idTraitement)))
        Assert.IsFalse(CBool(ValeurHisto("oa_traitement_allergie", idTraitement)))
        Assert.IsTrue(CBool(ValeurHisto("oa_traitement_contre_indication", idTraitement)))
        Assert.IsFalse(CBool(ValeurHisto("oa_traitement_declaratif_hors_traitement", idTraitement)))
    End Sub

    <TestMethod()> Public Sub CreationTraitementHisto_TexteAbsent_EchoueSansRienEcrire()
        ' Comportement actuel : les textes passent par .ToString ; un texte à Nothing
        ' lève une NullReferenceException avant toute écriture.
        Dim idPatient = CreerPatient()
        Dim histo = HistoriqueComplet(idPatient, TraitementAbsent)
        histo.HistorisationPosologieCommentaire = Nothing

        Assert.ThrowsException(Of NullReferenceException)(
            Sub() TraitementHistoDao.CreationTraitementHisto(histo, Auteur(CreerUtilisateur(avecCle:=False)),
                                                            TraitementHistoDao.EnumEtatTraitementHisto.ModificationTraitement))

        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_traitement_histo WHERE oa_traitement_id = @p0", TraitementAbsent)))
    End Sub

    ' --- Initialisation de l'historique -------------------------------------------

    <TestMethod()> Public Sub InitClasseTraitementHistorisation_RecopieLeTraitement()
        Dim source = TraitementDeTest(123, 2, Date.Today.AddDays(-3), Date.Today.AddDays(30))
        source.TraitementId = 77
        source.Fenetre = True
        source.FenetreDateDebut = Date.Today.AddDays(5)
        source.FenetreDateFin = Date.Today.AddDays(9)
        source.FenetreCommentaire = "Pause"
        source.Allergie = True
        source.Arret = "A"
        source.ArretCommentaire = "Arrêté"
        Dim histo As New TraitementHisto With {.HistorisationEtat = 3}

        TraitementHistoDao.InitClasseTraitementHistorisation(source, Auteur(55), histo)

        Assert.AreEqual(55, histo.HistorisationUtilisateurId)
        Assert.AreEqual(0, histo.HistorisationEtat)
        Assert.AreEqual(123, histo.HistorisationPatientId)
        Assert.AreEqual(77, histo.HistorisationTraitementId)
        Assert.AreEqual(CisDeTest + 2, histo.HistorisationMedicamentId)
        Assert.AreEqual("DCI TEST 2", histo.HistorisationMedicamentDci)
        Assert.AreEqual(Date.Today.AddDays(-3), histo.HistorisationDateDebut)
        Assert.AreEqual(Date.Today.AddDays(30), histo.HistorisationDateFin)
        Assert.AreEqual("J", histo.HistorisationPosologieBase)
        Assert.AreEqual(1, histo.HistorisationPosologieMatin)
        Assert.AreEqual(1, histo.HistorisationPosologieSoir)
        Assert.AreEqual("Pendant le repas", histo.HistorisationPosologieCommentaire)
        Assert.IsTrue(histo.HistorisationFenetre)
        ' Comportement actuel : la date de début de fenêtre reçoit la date de début
        ' du traitement, pas celle de la fenêtre.
        Assert.AreEqual(Date.Today.AddDays(-3), histo.HistorisationFenetreDateDebut)
        Assert.AreEqual(Date.Today.AddDays(9), histo.HistorisationFenetreDateFin)
        Assert.AreEqual("Pause", histo.HistorisationFenetreCommentaire)
        Assert.AreEqual("A", histo.HistorisationArret)
        Assert.AreEqual("Arrêté", histo.HistorisationArretCommentaire)
        Assert.IsTrue(histo.HistorisationAllergie)
        Assert.IsFalse(histo.HistorisationContreIndication)
    End Sub

    <TestMethod()> Public Sub InitClasseTraitementHistorisationBdd_LitLaLigneDuTraitement()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idTraitement = EnregistrerTraitement(TraitementDeTest(idPatient, 3, Date.Today.AddDays(-1), Date.Today.AddDays(40)), idUtilisateur)
        PoserFenetreTherapeutique(idTraitement, Date.Today.AddDays(2), Date.Today.AddDays(6), "Pause courte")
        Dim histo As New TraitementHisto

        Using connexion As New SqlConnection(ChaineConnexion(Compte.Client))
            connexion.Open()
            Using commande As New SqlCommand("SELECT * FROM oasis.oa_traitement WHERE oa_traitement_id = @id", connexion)
                commande.Parameters.AddWithValue("@id", idTraitement)
                Using lecteur = commande.ExecuteReader()
                    Assert.IsTrue(lecteur.Read())
                    TraitementHistoDao.InitClasseTraitementHistorisationBdd(lecteur, Auteur(idUtilisateur), histo)
                End Using
            End Using
        End Using

        Assert.AreEqual(CInt(idUtilisateur), histo.HistorisationUtilisateurId)
        Assert.AreEqual(0, histo.HistorisationEtat)
        Assert.AreEqual(CInt(idPatient), histo.HistorisationPatientId)
        Assert.AreEqual(CInt(idTraitement), histo.HistorisationTraitementId)
        Assert.AreEqual(CisDeTest + 3, histo.HistorisationMedicamentId)
        Assert.AreEqual("DCI TEST 3", histo.HistorisationMedicamentDci)
        Assert.AreEqual(Date.Today.AddDays(-1), histo.HistorisationDateDebut.Date)
        Assert.AreEqual(Date.Today.AddDays(40), histo.HistorisationDateFin.Date)
        Assert.AreEqual(3, histo.HistorisationOrdreAffichage)
        Assert.AreEqual("J", histo.HistorisationPosologieBase)
        Assert.AreEqual("0", histo.HistorisationFractionMatin)
        Assert.IsTrue(histo.HistorisationFenetre)
        ' Ici la fenêtre est bien lue dans ses propres colonnes.
        Assert.AreEqual(Date.Today.AddDays(2), histo.HistorisationFenetreDateDebut.Date)
        Assert.AreEqual(Date.Today.AddDays(6), histo.HistorisationFenetreDateFin.Date)
        Assert.AreEqual("Pause courte", histo.HistorisationFenetreCommentaire)
        Assert.AreEqual("", histo.HistorisationArret)
        Assert.AreEqual("", histo.HistorisationAnnulation)
        Assert.IsFalse(histo.HistorisationAllergie)
        Assert.IsFalse(histo.HistorisationDeclaratifHorsTraitement)
    End Sub

End Class
