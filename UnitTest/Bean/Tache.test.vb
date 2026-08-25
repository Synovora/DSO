Imports Oasis_Common

''' <summary>
''' Règles d'état d'une tâche : à qui elle est attribuée, si un rendez-vous
''' reste à fixer, si elle peut être désattribuée, et les libellés affichés.
''' </summary>
<TestClass()> Public Class TestTache

    Private Shared Function Compte(id As Integer, Optional admin As Boolean = False) As Utilisateur
        Return New Utilisateur With {.UtilisateurId = id, .UtilisateurAdmin = admin}
    End Function

    <TestMethod()> Public Sub LIndexDUnTypeEstSaValeurDEnumeration()
        Assert.AreEqual(100, Tache.GetTypeTacheIndex("RDV_DEMANDE"))
        Assert.AreEqual(2, Tache.GetTypeTacheIndex("RDV"))
        Assert.AreEqual(400, Tache.GetTypeTacheIndex("AVIS_SOUS_EPISODE"))
    End Sub

    <TestMethod()> Public Sub UnTypeInconnuDonneZero()
        Assert.AreEqual(0, Tache.GetTypeTacheIndex("PAS_UN_TYPE"))
        Assert.AreEqual(0, Tache.GetTypeTacheIndex(""))
        Assert.AreEqual(0, Tache.GetTypeTacheIndex(Nothing))
    End Sub

    <TestMethod()> Public Sub LeLibelleDependDuTypeEtDeLaNature()
        Assert.AreEqual("Demande de rendez-vous", Tache.getLibelleTacheNature("RDV_DEMANDE", "x"))
        Assert.AreEqual("Rendez-vous", Tache.getLibelleTacheNature("RDV", "x"))
        Assert.AreEqual("Demande d'avis sur épisode", Tache.getLibelleTacheNature("AVIS_EPISODE", "DEMANDE"))
        Assert.AreEqual("Réponse d'avis sur sous-épisode", Tache.getLibelleTacheNature("AVIS_SOUS_EPISODE", "réponse"))
        Assert.AreEqual("Demande de Rendez-vous mission", Tache.getLibelleTacheNature("MISSION_DEMANDE", "x"))
        Assert.AreEqual("Rendez-vous mission", Tache.getLibelleTacheNature("RDV_MISSION", "x"))
        Assert.AreEqual("Réunion Staff", Tache.getLibelleTacheNature("REUNION_STAFF", "x"))
        Assert.AreEqual("Rendez-vous hors Oasis", Tache.getLibelleTacheNature("RDV_SPECIALISTE", "x"))
        Assert.AreEqual("", Tache.getLibelleTacheNature("PAS_UN_TYPE", "x"))
    End Sub

    <TestMethod()> Public Sub UneDemandeDeRendezVousDevientUnRendezVous()
        Assert.AreEqual(Tache.TypeTache.RDV, (New Tache With {.Type = "RDV_DEMANDE"}).GetTypeRdvFromDemande())
        Assert.AreEqual(Tache.TypeTache.RDV_MISSION, (New Tache With {.Type = "MISSION_DEMANDE"}).GetTypeRdvFromDemande())
        ' Tout autre type n'a pas de rendez-vous dérivé : la valeur rendue est zéro,
        ' qui n'est aucun TypeTache défini.
        Assert.AreEqual(CType(0, Tache.TypeTache), (New Tache With {.Type = "RDV"}).GetTypeRdvFromDemande())
    End Sub

    <TestMethod()> Public Sub UneTacheEstAttribueeQuandElleAUnTraitant()
        Assert.IsFalse(New Tache().IsAttribue())
        Assert.IsFalse((New Tache With {.TraiteUserId = 0}).IsAttribue())
        Assert.IsTrue((New Tache With {.TraiteUserId = 5}).IsAttribue())
    End Sub

    <TestMethod()> Public Sub LesEtatsTermineEtAnnuleSontFinaux()
        Assert.IsTrue((New Tache With {.Etat = "TERMINEE"}).IsStatutFinal())
        Assert.IsTrue((New Tache With {.Etat = "ANNULEE"}).IsStatutFinal())
        Assert.IsFalse((New Tache With {.Etat = "EN_COURS"}).IsStatutFinal())
        Assert.IsFalse((New Tache With {.Etat = "EN_ATTENTE"}).IsStatutFinal())
        Assert.IsFalse((New Tache With {.Etat = ""}).IsStatutFinal())
    End Sub

    <TestMethod()> Public Sub LaTacheEstAMoiSiJEnSuisLeTraitant()
        Dim tache = New Tache With {.TraiteUserId = 5}
        Assert.IsTrue(tache.IsMyTacheATraiter(Compte(5)))
        Assert.IsFalse(tache.IsMyTacheATraiter(Compte(6)))
    End Sub

    <TestMethod()> Public Sub UnRendezVousResteAFixerPourMaDemandeEnCours()
        Dim tache = New Tache With {.TraiteUserId = 5, .Etat = "EN_COURS", .Type = "RDV_DEMANDE"}
        Assert.IsTrue(tache.IsRendezVousAFixer(Compte(5)))
        tache.Type = "MISSION_DEMANDE"
        Assert.IsTrue(tache.IsRendezVousAFixer(Compte(5)))
    End Sub

    <TestMethod()> Public Sub UnRendezVousNEstPasAFixerHorsDeCesConditions()
        Dim tache = New Tache With {.TraiteUserId = 5, .Etat = "EN_COURS", .Type = "RDV_DEMANDE"}
        Assert.IsFalse(tache.IsRendezVousAFixer(Compte(6)), "pas le traitant")
        tache.Etat = "TERMINEE"
        Assert.IsFalse(tache.IsRendezVousAFixer(Compte(5)), "déjà terminée")
        tache.Etat = "EN_COURS"
        tache.Type = "RDV"
        Assert.IsFalse(tache.IsRendezVousAFixer(Compte(5)), "déjà un rendez-vous")
    End Sub

    <TestMethod()> Public Sub LeTraitantOuUnAdministrateurPeutDesattribuerUneTacheOuverte()
        Dim tache = New Tache With {.TraiteUserId = 5, .Etat = "EN_COURS"}
        Assert.IsTrue(tache.IsDesattribuable(Compte(5)))
        Assert.IsTrue(tache.IsDesattribuable(Compte(9, admin:=True)))
        Assert.IsFalse(tache.IsDesattribuable(Compte(6)), "ni traitant ni administrateur")
    End Sub

    <TestMethod()> Public Sub UneTacheNonAttribueeOuCloseNeSeDesattribuePas()
        Assert.IsFalse((New Tache With {.TraiteUserId = 0, .Etat = "EN_COURS"}).IsDesattribuable(Compte(9, admin:=True)))
        Assert.IsFalse((New Tache With {.TraiteUserId = 5, .Etat = "TERMINEE"}).IsDesattribuable(Compte(5)))
        Assert.IsFalse((New Tache With {.TraiteUserId = 5, .Etat = "ANNULEE"}).IsDesattribuable(Compte(9, admin:=True)))
    End Sub

End Class
