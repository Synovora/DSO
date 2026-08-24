Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne d'ordonnance par OrdonnanceDetailDao.BuildBean. Ces
''' valeurs entrent dans la charge signée (OrdonnanceDetail.Serialize) : un
''' défaut différent de celui en vigueur au moment de la signature rendrait
''' l'ordonnance non vérifiable.
''' </summary>
<TestClass()> Public Class TestOrdonnanceDetailDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_ordonnance_ligne_id", "oa_ordonnance_traitement", "oa_traitement_id",
        "oa_traitement_ordre_affichage", "oa_traitement_ald", "oa_traitement_a_delivrer",
        "oa_traitement_medicament_cis", "oa_traitement_medicament_dci", "oa_traitement_date_debut",
        "oa_traitement_date_fin", "oa_traitement_duree", "oa_traitement_posologie",
        "oa_traitement_posologie_base", "oa_traitement_posologie_rythme",
        "oa_traitement_posologie_matin", "oa_traitement_posologie_midi",
        "oa_traitement_posologie_apres_midi", "oa_traitement_posologie_soir",
        "oa_traitement_fraction_matin", "oa_traitement_fraction_midi",
        "oa_traitement_fraction_apres_midi", "oa_traitement_fraction_soir",
        "oa_traitement_posologie_commentaire", "oa_traitement_commentaire",
        "oa_traitement_fenetre", "oa_traitement_fenetre_date_debut",
        "oa_traitement_fenetre_date_fin", "oa_traitement_inactif"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim d = OrdonnanceDetailDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_ordonnance_ligne_id", 77}, {"oa_ordonnance_traitement", True}, {"oa_traitement_id", 9},
            {"oa_traitement_ordre_affichage", 3}, {"oa_traitement_ald", True}, {"oa_traitement_a_delivrer", True},
            {"oa_traitement_medicament_cis", 61234567}, {"oa_traitement_medicament_dci", "PARACETAMOL"},
            {"oa_traitement_date_debut", New Date(2024, 1, 4)}, {"oa_traitement_date_fin", New Date(2024, 2, 4)},
            {"oa_traitement_duree", 30}, {"oa_traitement_posologie", "1 cp matin et soir"},
            {"oa_traitement_posologie_base", "1 cp"}, {"oa_traitement_posologie_rythme", 1},
            {"oa_traitement_posologie_matin", 1}, {"oa_traitement_posologie_midi", 0},
            {"oa_traitement_posologie_apres_midi", 0}, {"oa_traitement_posologie_soir", 1},
            {"oa_traitement_fraction_matin", "1"}, {"oa_traitement_fraction_midi", ""},
            {"oa_traitement_fraction_apres_midi", ""}, {"oa_traitement_fraction_soir", "1"},
            {"oa_traitement_posologie_commentaire", "au repas"}, {"oa_traitement_commentaire", "RAS"},
            {"oa_traitement_fenetre", True}, {"oa_traitement_fenetre_date_debut", New Date(2024, 1, 10)},
            {"oa_traitement_fenetre_date_fin", New Date(2024, 1, 12)}, {"oa_traitement_inactif", True}}))

        Assert.AreEqual(77, d.LigneId)
        Assert.IsTrue(d.Traitement)
        Assert.AreEqual(9, d.TraitementId)
        Assert.AreEqual(3, d.OrdreAffichage)
        Assert.IsTrue(d.Ald)
        Assert.IsTrue(d.ADelivrer)
        Assert.AreEqual(61234567, d.MedicamentCis)
        Assert.AreEqual("PARACETAMOL", d.MedicamentDci)
        Assert.AreEqual(New Date(2024, 1, 4), d.DateDebut)
        Assert.AreEqual(New Date(2024, 2, 4), d.DateFin)
        Assert.AreEqual(30, d.Duree)
        Assert.AreEqual("1 cp matin et soir", d.Posologie)
        Assert.AreEqual("1 cp", d.PosologieBase)
        Assert.AreEqual(1, d.PosologieRythme)
        Assert.AreEqual(1, d.PosologieMatin)
        Assert.AreEqual(0, d.PosologieMidi)
        Assert.AreEqual(0, d.PosologieApresMidi)
        Assert.AreEqual(1, d.PosologieSoir)
        Assert.AreEqual("1", d.FractionMatin)
        Assert.AreEqual("", d.FractionMidi)
        Assert.AreEqual("", d.FractionApresMidi)
        Assert.AreEqual("1", d.FractionSoir)
        Assert.AreEqual("au repas", d.PosologieCommentaire)
        Assert.AreEqual("RAS", d.Commentaire)
        Assert.IsTrue(d.Fenetre)
        Assert.AreEqual(New Date(2024, 1, 10), d.FenetreDateDebut)
        Assert.AreEqual(New Date(2024, 1, 12), d.FenetreDateFin)
        Assert.IsTrue(d.Inactif)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim d = OrdonnanceDetailDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {{"oa_ordonnance_ligne_id", 1}}))

        Assert.AreEqual(1, d.LigneId)
        For Each texte In {d.MedicamentDci, d.Posologie, d.PosologieBase, d.FractionMatin, d.FractionMidi,
                           d.FractionApresMidi, d.FractionSoir, d.PosologieCommentaire, d.Commentaire}
            Assert.AreEqual("", texte)
        Next
        For Each entier In {d.TraitementId, d.OrdreAffichage, d.MedicamentCis, d.Duree, d.PosologieRythme,
                            d.PosologieMatin, d.PosologieMidi, d.PosologieApresMidi, d.PosologieSoir}
            Assert.AreEqual(0, entier)
        Next
        For Each moment In {d.DateDebut, d.DateFin, d.FenetreDateDebut, d.FenetreDateFin}
            Assert.AreEqual(Date.MinValue, moment)
        Next
        Assert.IsFalse(d.Traitement)
        Assert.IsFalse(d.Ald)
        Assert.IsFalse(d.ADelivrer)
        Assert.IsFalse(d.Fenetre)
        Assert.IsFalse(d.Inactif)
    End Sub

    <TestMethod()> Public Sub LaLigneLueSeSerialiseCommeAvant()
        ' Le format binaire est figé (voir Ordonnance.test.vb). Une ligne lue
        ' avec ses défauts doit se sérialiser puis se relire sans perte.
        Dim d = OrdonnanceDetailDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_ordonnance_ligne_id", 5}, {"oa_traitement_medicament_dci", "IBUPROFENE"}, {"oa_traitement_duree", 7}}))
        Dim relu = OrdonnanceDetail.Deserialize(d.Serialize())
        Assert.AreEqual(d.LigneId, relu.LigneId)
        Assert.AreEqual("IBUPROFENE", relu.MedicamentDci)
        Assert.AreEqual(7, relu.Duree)
        CollectionAssert.AreEqual(d.Serialize(), relu.Serialize())
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdentifiantEstUneErreur()
        OrdonnanceDetailDao.BuildBean(LigneDeTest.Ligne(Colonnes, Nothing))
    End Sub

End Class
