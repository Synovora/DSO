Imports System.Data
Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne de oa_patient par PatientDao.LireLigne.
'''
''' La ligne est fabriquée avec un DataTable : DataTableReader implémente
''' IDataRecord, ce qui évite de simuler un SqlDataReader et permet de placer
''' DBNull dans n'importe quelle colonne. C'est là que vivent les erreurs de
''' valeur par défaut : Option Strict est désactivé et Coalesce rend Object, donc
''' un mauvais défaut se convertit sans bruit jusqu'à l'écran.
''' </summary>
<TestClass()> Public Class TestPatientDaoLecture

    ' Les colonnes que LireLigne consomme, dans l'ordre du SELECT.
    Private Shared ReadOnly Colonnes As String() = {
        "oa_patient_id", "oa_patient_nir", "oa_patient_nom", "oa_patient_prenom",
        "oa_patient_date_naissance", "oa_patient_genre_id", "oa_patient_adresse1",
        "oa_patient_adresse2", "oa_patient_code_postal", "oa_patient_ville",
        "oa_patient_tel1", "oa_patient_tel2", "oa_patient_email", "oa_patient_nom_marital",
        "oa_patient_date_entree_oasis", "oa_patient_date_sortie_oasis",
        "oa_patient_commentaire_sortie", "oa_patient_date_deces", "oa_patient_site_id",
        "oa_patient_siege_id", "oa_patient_couverture_internet",
        "oa_patient_unite_sanitaire_id", "oa_patient_synthese_date_maj",
        "oa_patient_profession", "oa_patient_pharmacie_id", "oa_patient_taille",
        "oa_patient_blocage_medical", "oa_patient_INS"}

    ''' <summary>Une ligne où tout ce qui n'est pas fourni vaut DBNull.</summary>
    Private Shared Function Ligne(valeurs As Dictionary(Of String, Object)) As IDataRecord
        Dim table As New DataTable()
        For Each colonne In Colonnes
            table.Columns.Add(colonne, GetType(Object))
        Next
        Dim rangee = table.NewRow()
        For Each colonne In Colonnes
            rangee(colonne) = If(valeurs.ContainsKey(colonne), valeurs(colonne), DBNull.Value)
        Next
        table.Rows.Add(rangee)

        Dim lecteur = table.CreateDataReader()
        Assert.IsTrue(lecteur.Read())
        Return lecteur
    End Function

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim patient = PatientDao.LireLigne(Ligne(New Dictionary(Of String, Object) From {
            {"oa_patient_id", 42},
            {"oa_patient_nir", 180057512345678L},
            {"oa_patient_nom", "Dupont"},
            {"oa_patient_prenom", "Marie"},
            {"oa_patient_date_naissance", New Date(1980, 5, 17)},
            {"oa_patient_genre_id", "F"},
            {"oa_patient_adresse1", "12 rue des Lilas"},
            {"oa_patient_adresse2", "Bât. B"},
            {"oa_patient_code_postal", "75011"},
            {"oa_patient_ville", "Paris"},
            {"oa_patient_tel1", "0102030405"},
            {"oa_patient_tel2", "0607080910"},
            {"oa_patient_email", "marie.dupont@exemple.fr"},
            {"oa_patient_nom_marital", "Martin"},
            {"oa_patient_date_entree_oasis", New Date(2020, 1, 2)},
            {"oa_patient_date_sortie_oasis", New Date(2021, 3, 4)},
            {"oa_patient_commentaire_sortie", "Transfert"},
            {"oa_patient_date_deces", New Date(2022, 5, 6)},
            {"oa_patient_site_id", 3},
            {"oa_patient_siege_id", 4},
            {"oa_patient_couverture_internet", True},
            {"oa_patient_unite_sanitaire_id", 5},
            {"oa_patient_synthese_date_maj", New Date(2023, 7, 8)},
            {"oa_patient_profession", "Enseignante"},
            {"oa_patient_pharmacie_id", 6L},
            {"oa_patient_taille", 168},
            {"oa_patient_blocage_medical", True},
            {"oa_patient_INS", 280057512345678L}}))

        Assert.AreEqual(42, patient.PatientId)
        Assert.AreEqual(180057512345678L, patient.PatientNir)
        Assert.AreEqual("Dupont", patient.PatientNom)
        Assert.AreEqual("Marie", patient.PatientPrenom)
        Assert.AreEqual(New Date(1980, 5, 17), patient.PatientDateNaissance)
        Assert.AreEqual("F", patient.PatientGenreId)
        Assert.AreEqual("12 rue des Lilas", patient.PatientAdresse1)
        Assert.AreEqual("Bât. B", patient.PatientAdresse2)
        Assert.AreEqual("75011", patient.PatientCodePostal)
        Assert.AreEqual("Paris", patient.PatientVille)
        Assert.AreEqual("0102030405", patient.PatientTel1)
        Assert.AreEqual("0607080910", patient.PatientTel2)
        Assert.AreEqual("marie.dupont@exemple.fr", patient.PatientEmail)
        Assert.AreEqual("Martin", patient.PatientNomMarital)
        Assert.AreEqual(New Date(2020, 1, 2), patient.PatientDateEntree)
        Assert.AreEqual(New Date(2021, 3, 4), patient.PatientDateSortie)
        Assert.AreEqual("Transfert", patient.PatientCommentaireSortie)
        Assert.AreEqual(New Date(2022, 5, 6), patient.PatientDateDeces)
        Assert.AreEqual(3, patient.PatientSiteId)
        Assert.AreEqual(4, patient.PatientSiegeId)
        Assert.IsTrue(patient.PatientInternet)
        Assert.AreEqual(5, patient.PatientUniteSanitaireId)
        Assert.AreEqual(New Date(2023, 7, 8), patient.PatientSyntheseDateMaj)
        Assert.AreEqual("Enseignante", patient.Profession)
        Assert.AreEqual(6L, patient.PharmacienId)
        Assert.AreEqual(168, patient.Taille)
        Assert.IsTrue(patient.BlocageMedical)
        Assert.AreEqual(280057512345678L, patient.INS)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        ' Seul l'identifiant est obligatoire. Tout le reste doit tomber sur un
        ' défaut sûr : chaîne vide plutôt que Nothing (les écrans concatènent),
        ' zéro, False, et Date.MinValue pour les dates absentes.
        Dim patient = PatientDao.LireLigne(Ligne(New Dictionary(Of String, Object) From {{"oa_patient_id", 7}}))

        Assert.AreEqual(7, patient.PatientId)
        Assert.AreEqual(0L, patient.PatientNir)
        Assert.AreEqual(0L, patient.INS)
        Assert.AreEqual(0L, patient.PharmacienId)

        For Each texte In {patient.PatientNom, patient.PatientPrenom, patient.PatientGenreId,
                           patient.PatientAdresse1, patient.PatientAdresse2, patient.PatientCodePostal,
                           patient.PatientVille, patient.PatientTel1, patient.PatientTel2,
                           patient.PatientEmail, patient.PatientNomMarital,
                           patient.PatientCommentaireSortie, patient.Profession}
            Assert.AreEqual("", texte)
        Next

        For Each entier In {patient.PatientSiteId, patient.PatientSiegeId,
                            patient.PatientUniteSanitaireId, patient.Taille}
            Assert.AreEqual(0, entier)
        Next

        For Each moment In {patient.PatientDateNaissance, patient.PatientDateEntree,
                            patient.PatientDateSortie, patient.PatientDateDeces,
                            patient.PatientSyntheseDateMaj}
            Assert.AreEqual(Date.MinValue, moment)
        Next

        Assert.IsFalse(patient.PatientInternet)
        Assert.IsFalse(patient.BlocageMedical)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdentifiantEstUneErreur()
        ' Un patient sans identifiant n'existe pas : mieux vaut une exception
        ' qu'un patient 0 qui irait écraser un enregistrement réel.
        PatientDao.LireLigne(Ligne(New Dictionary(Of String, Object)))
    End Sub

    <TestMethod()> Public Sub LesChampsDerivesNeSontPasLusDepuisLaLigne()
        ' Le libellé du genre et l'âge viennent de Completer, pas de la table.
        ' Si LireLigne se mettait à toucher au référentiel, ce test partirait en
        ' erreur de connexion, ce qui est le signal voulu.
        Dim patient = PatientDao.LireLigne(Ligne(New Dictionary(Of String, Object) From {
            {"oa_patient_id", 1}, {"oa_patient_genre_id", "M"},
            {"oa_patient_date_naissance", New Date(1990, 1, 1)}}))
        Assert.AreEqual("M", patient.PatientGenreId)
    End Sub

End Class
