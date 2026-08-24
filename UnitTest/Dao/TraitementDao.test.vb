Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne de traitement par TraitementDao.BuildBean. Les
''' posologies lues ici sont celles imprimées sur l'ordonnance.
''' </summary>
<TestClass()> Public Class TestTraitementDaoLecture

    Private Shared ReadOnly Colonnes As String() = {
        "oa_traitement_id", "oa_traitement_patient_id", "oa_traitement_medicament_cis",
        "oa_traitement_medicament_dci", "oa_traitement_classe_atc",
        "oa_traitement_denomination_longue", "oa_traitement_identifiant_creation",
        "oa_traitement_date_creation", "oa_traitement_identifiant_modification",
        "oa_traitement_date_modification", "oa_traitement_date_debut", "oa_traitement_date_fin",
        "oa_traitement_ordre_affichage", "oa_traitement_posologie_base",
        "oa_traitement_posologie_rythme", "oa_traitement_posologie_matin",
        "oa_traitement_posologie_midi", "oa_traitement_posologie_apres_midi",
        "oa_traitement_posologie_soir", "oa_traitement_fraction_matin",
        "oa_traitement_fraction_midi", "oa_traitement_fraction_apres_midi",
        "oa_traitement_fraction_soir", "oa_traitement_posologie_commentaire",
        "oa_traitement_fenetre", "oa_traitement_fenetre_date_debut",
        "oa_traitement_fenetre_date_fin", "oa_traitement_fenetre_commentaire",
        "oa_traitement_commentaire", "oa_traitement_arret", "oa_traitement_arret_commentaire",
        "oa_traitement_allergie", "oa_traitement_contre_indication",
        "oa_traitement_declaratif_hors_traitement", "oa_traitement_annulation",
        "oa_traitement_annulation_commentaire"}

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim t = TraitementDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {
            {"oa_traitement_id", 9}, {"oa_traitement_patient_id", 42}, {"oa_traitement_medicament_cis", 61234567},
            {"oa_traitement_medicament_dci", "PARACETAMOL"}, {"oa_traitement_classe_atc", "N02BE01"},
            {"oa_traitement_denomination_longue", "PARACETAMOL 1 g cp"},
            {"oa_traitement_identifiant_creation", 5}, {"oa_traitement_date_creation", New Date(2024, 1, 2)},
            {"oa_traitement_identifiant_modification", 6}, {"oa_traitement_date_modification", New Date(2024, 1, 3)},
            {"oa_traitement_date_debut", New Date(2024, 1, 4)}, {"oa_traitement_date_fin", New Date(2024, 2, 4)},
            {"oa_traitement_ordre_affichage", 2}, {"oa_traitement_posologie_base", "1 cp"},
            {"oa_traitement_posologie_rythme", 1}, {"oa_traitement_posologie_matin", 1},
            {"oa_traitement_posologie_midi", 0}, {"oa_traitement_posologie_apres_midi", 0},
            {"oa_traitement_posologie_soir", 1}, {"oa_traitement_fraction_matin", "1"},
            {"oa_traitement_fraction_midi", ""}, {"oa_traitement_fraction_apres_midi", ""},
            {"oa_traitement_fraction_soir", "1/2"}, {"oa_traitement_posologie_commentaire", "si douleur"},
            {"oa_traitement_fenetre", True}, {"oa_traitement_fenetre_date_debut", New Date(2024, 1, 10)},
            {"oa_traitement_fenetre_date_fin", New Date(2024, 1, 12)}, {"oa_traitement_fenetre_commentaire", "pause"},
            {"oa_traitement_commentaire", "RAS"}, {"oa_traitement_arret", "A"},
            {"oa_traitement_arret_commentaire", "fin"}, {"oa_traitement_allergie", True},
            {"oa_traitement_contre_indication", True}, {"oa_traitement_declaratif_hors_traitement", True},
            {"oa_traitement_annulation", "N"}, {"oa_traitement_annulation_commentaire", "erreur"}}))

        Assert.AreEqual(9, t.TraitementId)
        Assert.AreEqual(42, t.PatientId)
        Assert.AreEqual(61234567, t.MedicamentId)
        Assert.AreEqual("PARACETAMOL", t.MedicamentDci)
        Assert.AreEqual("N02BE01", t.ClasseAtc)
        Assert.AreEqual("PARACETAMOL 1 g cp", t.DenominationLongue)
        Assert.AreEqual(5, t.UserCreation)
        Assert.AreEqual(New Date(2024, 1, 2), t.DateCreation)
        Assert.AreEqual(6, t.UserModification)
        Assert.AreEqual(New Date(2024, 1, 3), t.DateModification)
        Assert.AreEqual(New Date(2024, 1, 4), t.DateDebut)
        Assert.AreEqual(New Date(2024, 2, 4), t.DateFin)
        Assert.AreEqual(2, t.OrdreAffichage)
        Assert.AreEqual("1 cp", t.PosologieBase)
        Assert.AreEqual(1, t.PosologieRythme)
        Assert.AreEqual(1, t.PosologieMatin)
        Assert.AreEqual(0, t.PosologieMidi)
        Assert.AreEqual(0, t.PosologieApresMidi)
        Assert.AreEqual(1, t.PosologieSoir)
        Assert.AreEqual("1", t.FractionMatin)
        Assert.AreEqual("", t.FractionMidi)
        Assert.AreEqual("", t.FractionApresMidi)
        Assert.AreEqual("1/2", t.FractionSoir)
        Assert.AreEqual("si douleur", t.PosologieCommentaire)
        Assert.IsTrue(t.Fenetre)
        Assert.AreEqual(New Date(2024, 1, 10), t.FenetreDateDebut)
        Assert.AreEqual(New Date(2024, 1, 12), t.FenetreDateFin)
        Assert.AreEqual("pause", t.FenetreCommentaire)
        Assert.AreEqual("RAS", t.Commentaire)
        Assert.AreEqual("A", t.Arret)
        Assert.AreEqual("fin", t.ArretCommentaire)
        Assert.IsTrue(t.Allergie)
        Assert.IsTrue(t.ContreIndication)
        Assert.IsTrue(t.DeclaratifHorsTraitement)
        Assert.AreEqual("N", t.Annulation)
        Assert.AreEqual("erreur", t.AnnulationCommentaire)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim t = TraitementDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {{"oa_traitement_id", 1}}))

        Assert.AreEqual(1, t.TraitementId)
        For Each texte In {t.MedicamentDci, t.ClasseAtc, t.DenominationLongue, t.PosologieBase,
                           t.FractionMatin, t.FractionMidi, t.FractionApresMidi, t.FractionSoir,
                           t.PosologieCommentaire, t.FenetreCommentaire, t.Commentaire, t.Arret,
                           t.ArretCommentaire, t.Annulation, t.AnnulationCommentaire}
            Assert.AreEqual("", texte)
        Next
        For Each entier In {t.UserCreation, t.UserModification, t.OrdreAffichage, t.PosologieRythme,
                            t.PosologieMatin, t.PosologieMidi, t.PosologieApresMidi, t.PosologieSoir}
            Assert.AreEqual(0, entier)
        Next
        For Each moment In {t.DateCreation, t.DateModification, t.DateDebut, t.DateFin,
                            t.FenetreDateDebut, t.FenetreDateFin}
            Assert.AreEqual(Date.MinValue, moment)
        Next
        Assert.IsFalse(t.Fenetre)
        Assert.IsFalse(t.Allergie)
        Assert.IsFalse(t.ContreIndication)
        Assert.IsFalse(t.DeclaratifHorsTraitement)
    End Sub

    <TestMethod()> Public Sub UnPatientOuUnMedicamentAbsentDonneZero()
        ' Les deux propriétés sont des entiers. Le repli était une chaîne vide,
        ' que la conversion en Integer refuse : un traitement déclaratif sans
        ' code CIS faisait échouer le chargement de toute la liste.
        Dim t = TraitementDao.BuildBean(LigneDeTest.Ligne(Colonnes, New Dictionary(Of String, Object) From {{"oa_traitement_id", 1}}))
        Assert.AreEqual(0, t.PatientId)
        Assert.AreEqual(0, t.MedicamentId)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdentifiantEstUneErreur()
        TraitementDao.BuildBean(LigneDeTest.Ligne(Colonnes, Nothing))
    End Sub

End Class
