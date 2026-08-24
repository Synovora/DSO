Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne de tâche par TacheDao.BuildBean.
''' </summary>
<TestClass()> Public Class TestTacheDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "id", "parent_id", "emetteur_user_id", "emetteur_fonction_id", "unite_sanitaire_id",
        "site_id", "patient_id", "parcours_id", "episode_id", "sous_episode_id", "traite_user_id",
        "traite_fonction_id", "destinataire_fonction_id", "priorite", "ordre_affichage",
        "categorie", "type", "nature", "duree_mn", "emetteur_commentaire", "horodate_creation",
        "horodate_attrib", "horodate_cloture", "etat", "cloture", "type_demande_rendez_vous",
        "date_rendez_vous", "date_traitement_demande_rendez_vous"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim t = TacheDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"id", 500L}, {"parent_id", 499L}, {"emetteur_user_id", 5L}, {"emetteur_fonction_id", 2L},
            {"unite_sanitaire_id", 3L}, {"site_id", 4L}, {"patient_id", 42L}, {"parcours_id", 7L},
            {"episode_id", 100L}, {"sous_episode_id", 101L}, {"traite_user_id", 6L}, {"traite_fonction_id", 8L},
            {"destinataire_fonction_id", 9L}, {"priorite", 2}, {"ordre_affichage", 1}, {"categorie", "SOIN"},
            {"type", "RDV"}, {"nature", "Suivi"}, {"duree_mn", 30}, {"emetteur_commentaire", "à planifier"},
            {"horodate_creation", New Date(2024, 1, 2, 9, 0, 0)}, {"horodate_attrib", New Date(2024, 1, 2, 10, 0, 0)},
            {"horodate_cloture", New Date(2024, 1, 3, 11, 0, 0)}, {"etat", "Clos"}, {"cloture", True},
            {"type_demande_rendez_vous", "Consultation"}, {"date_rendez_vous", New Date(2024, 1, 15)},
            {"date_traitement_demande_rendez_vous", New Date(2024, 1, 4)}}))

        Assert.AreEqual(500L, t.Id)
        Assert.AreEqual(499L, t.ParentId)
        Assert.AreEqual(5L, t.EmetteurUserId)
        Assert.AreEqual(2L, t.EmetteurFonctionId)
        Assert.AreEqual(3L, t.UniteSanitaireId)
        Assert.AreEqual(4L, t.SiteId)
        Assert.AreEqual(42L, t.PatientId)
        Assert.AreEqual(7L, t.ParcoursId)
        Assert.AreEqual(100L, t.EpisodeId)
        Assert.AreEqual(101L, t.SousEpisodeId)
        Assert.AreEqual(6L, t.TraiteUserId)
        Assert.AreEqual(8L, t.TraiteFonctionId)
        Assert.AreEqual(9L, t.DestinataireFonctionId)
        Assert.AreEqual(2, t.Priorite)
        Assert.AreEqual(1, t.OrdreAffichage)
        Assert.AreEqual("SOIN", t.Categorie)
        Assert.AreEqual("RDV", t.Type)
        Assert.AreEqual("Suivi", t.Nature)
        Assert.AreEqual(30, t.Duree)
        Assert.AreEqual("à planifier", t.EmetteurCommentaire)
        Assert.AreEqual(New Date(2024, 1, 2, 9, 0, 0), t.HorodatageCreation)
        Assert.AreEqual(New Date(2024, 1, 2, 10, 0, 0), t.HorodatageAttribution)
        Assert.AreEqual(New Date(2024, 1, 3, 11, 0, 0), t.HorodatageCloture)
        Assert.AreEqual("Clos", t.Etat)
        Assert.IsTrue(t.Cloture)
        Assert.AreEqual("Consultation", t.TypedemandeRendezVous)
        Assert.AreEqual(New Date(2024, 1, 15), t.DateRendezVous)
        Assert.AreEqual(New Date(2024, 1, 4), t.DateTraitementDemandeRendezVous)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim t = TacheDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {{"id", 1L}}))

        Assert.AreEqual(1L, t.Id)
        For Each texte In {t.Categorie, t.Type, t.Nature, t.EmetteurCommentaire, t.Etat, t.TypedemandeRendezVous}
            Assert.AreEqual("", texte)
        Next
        For Each entier In {t.ParentId, t.EmetteurUserId, t.EmetteurFonctionId, t.UniteSanitaireId, t.SiteId,
                            t.PatientId, t.ParcoursId, t.EpisodeId, t.SousEpisodeId, t.TraiteUserId,
                            t.TraiteFonctionId, t.DestinataireFonctionId}
            Assert.AreEqual(0L, entier)
        Next
        Assert.AreEqual(0, t.Priorite)
        Assert.AreEqual(0, t.OrdreAffichage)
        Assert.AreEqual(0, t.Duree)
        For Each moment In {t.HorodatageCreation, t.HorodatageAttribution, t.HorodatageCloture,
                            t.DateRendezVous, t.DateTraitementDemandeRendezVous}
            Assert.AreEqual(Date.MinValue, moment)
        Next
        Assert.IsFalse(t.Cloture)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdentifiantEstUneErreur()
        TacheDao.BuildBean(LigneDeTest.Ligne(Colonnes, Nothing))
    End Sub

End Class
