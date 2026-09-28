Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' AntecedentHistoCreationDao contre la base de test. CreationAntecedentHisto est
''' appelée par AntecedentDao et ContexteDao dans le client lourd, sous
''' oasis_client ; les deux Init* ne font que recopier un antécédent ou une ligne
''' lue dans l'objet d'historique.
''' </summary>
<TestClass()> Public Class AntecedentHistoCreationDaoTest
    Inherits TestIntegration

    <TestMethod()> Public Sub CreationAntecedentHisto_EcritLaLigneTelleQuelle()
        Dim idConnecte = CreerUtilisateur(avecCle:=False)
        Dim idAutre = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAntecedent = CreerAntecedent(idPatient, idConnecte)
        Dim idDrc = CreerDrc()
        Dim histo As New AntecedentHisto With {
            .UtilisateurId = CInt(idAutre),
            .Etat = AntecedentHistoCreationDao.EnumEtatAntecedentHisto.CreationAntecedent,
            .AntecedentId = CInt(idAntecedent),
            .PatientId = CInt(idPatient),
            .Type = "C",
            .DrcId = idDrc.ToString,
            .Description = "Contexte arrêté",
            .DateDebut = New Date(2020, 1, 2),
            .DateFin = New Date(2022, 3, 4),
            .Arret = True,
            .ArretCommentaire = "Guéri",
            .Nature = "Patient",
            .Niveau = 2,
            .Niveau1Id = 11,
            .Niveau2Id = 12,
            .Ordre1 = 20,
            .Ordre2 = 40,
            .Ordre3 = 60,
            .StatutAffichage = "C",
            .Categorie = "B",
            .Inactif = True,
            .Diagnostic = 2,
            .AldId = 0,
            .AldCim10Id = 0,
            .AldValide = False,
            .AldDateDebut = New Date(2021, 1, 1),
            .AldDateFin = New Date(2023, 1, 1),
            .AldDemandeEnCours = True,
            .AldDateDemande = New Date(2020, 12, 1)
        }

        Assert.IsTrue(AntecedentHistoCreationDao.CreationAntecedentHisto(
            histo, New Utilisateur With {.UtilisateurId = CInt(idConnecte)},
            AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ArretAntecedent))

        Dim table = HistoriqueAntecedent(idAntecedent)
        Assert.AreEqual(2, table.Rows.Count, "création de l'antécédent, puis la ligne écrite ici")
        Dim ligne = table.Rows(1)
        Assert.AreEqual(3, CInt(ligne("oa_antecedent_histo_etat_historisation")), "l'état vient du paramètre, pas de l'objet")
        Assert.AreEqual(idConnecte, CLng(ligne("oa_antecedent_histo_utilisateur_historisation")),
                        "l'auteur est l'utilisateur connecté, pas UtilisateurId de l'objet")
        Assert.AreEqual(Date.Today, CDate(ligne("oa_antecedent_histo_date_historisation")).Date)
        Assert.AreEqual(idPatient, CLng(ligne("oa_antecedent_patient_id")))
        Assert.AreEqual("C", CStr(ligne("oa_antecedent_type")))
        Assert.AreEqual(idDrc, CLng(ligne("oa_antecedent_drc_id")))
        Assert.AreEqual("Contexte arrêté", CStr(ligne("oa_antecedent_description")))
        Assert.AreEqual(New Date(2020, 1, 2), CDate(ligne("oa_antecedent_date_debut")))
        Assert.AreEqual(New Date(2022, 3, 4), CDate(ligne("oa_antecedent_date_fin")))
        Assert.IsTrue(CBool(ligne("oa_antecedent_arret")))
        Assert.AreEqual("Guéri", CStr(ligne("oa_antecedent_arret_commentaire")))
        Assert.AreEqual("Patient", CStr(ligne("oa_antecedent_nature")))
        Assert.AreEqual(2, CInt(ligne("oa_antecedent_niveau")))
        Assert.AreEqual(11, CInt(ligne("oa_antecedent_id_niveau1")))
        Assert.AreEqual(12, CInt(ligne("oa_antecedent_id_niveau2")))
        Assert.AreEqual(20, CInt(ligne("oa_antecedent_ordre_affichage1")))
        Assert.AreEqual(40, CInt(ligne("oa_antecedent_ordre_affichage2")))
        Assert.AreEqual(60, CInt(ligne("oa_antecedent_ordre_affichage3")))
        Assert.AreEqual("C", CStr(ligne("oa_antecedent_statut_affichage")))
        Assert.AreEqual("B", CStr(ligne("oa_antecedent_categorie_contexte")))
        Assert.IsTrue(CBool(ligne("oa_antecedent_inactif")))
        Assert.AreEqual(2, CInt(ligne("oa_antecedent_diagnostic")))
        Assert.AreEqual(New Date(2021, 1, 1), CDate(ligne("oa_antecedent_ald_date_debut")))
        Assert.AreEqual(New Date(2023, 1, 1), CDate(ligne("oa_antecedent_ald_date_fin")))
        Assert.IsTrue(CBool(ligne("oa_antecedent_ald_demande_en_cours")))
        Assert.AreEqual(New Date(2020, 12, 1), CDate(ligne("oa_antecedent_ald_demande_date")))
        ' Comportement actuel : ChaineEpisodeDateFin de l'objet est ignorée, la
        ' date écrite est maintenant + ChaineEpisodePeriode (6 mois dans app.config).
        Assert.AreEqual(Date.Today.AddMonths(6), CDate(ligne("oa_chaine_episode_date_fin")).Date)
    End Sub

    <TestMethod()> Public Sub CreationAntecedentHisto_CommentaireEtCategorieAbsents_DeviennentVides()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAntecedent = CreerAntecedent(idPatient, idUtilisateur)
        Dim histo As New AntecedentHisto With {
            .AntecedentId = CInt(idAntecedent),
            .PatientId = CInt(idPatient),
            .Type = "A",
            .DrcId = CreerDrc().ToString,
            .Description = "Sans commentaire",
            .DateDebut = DateDebutAntecedentDeTest,
            .DateFin = FinContexteDeTest,
            .ArretCommentaire = Nothing,
            .Categorie = Nothing,
            .StatutAffichage = "P",
            .AldDateDebut = Date.MaxValue,
            .AldDateFin = Date.MaxValue,
            .AldDateDemande = Date.MaxValue
        }

        AntecedentHistoCreationDao.CreationAntecedentHisto(histo, New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)},
                                                          AntecedentHistoCreationDao.EnumEtatAntecedentHisto.ModificationAntecedent)

        Dim ligne = HistoriqueAntecedent(idAntecedent).Rows(1)
        Assert.AreEqual("", CStr(ligne("oa_antecedent_arret_commentaire")))
        Assert.AreEqual("", CStr(ligne("oa_antecedent_categorie_contexte")))
        Assert.IsFalse(CBool(ligne("oa_antecedent_arret")))
        Assert.IsFalse(CBool(ligne("oa_antecedent_inactif")))
    End Sub

    <TestMethod()> Public Sub InitAntecedentHistorisation_RecopieLAntecedent()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idContexte = CreerContexteMedical(CreerPatient(), idUtilisateur, description:="Tabac", categorie:="B", ordre1:=30)
        Dim lu = (New AntecedentDao).GetAntecedentById(CInt(idContexte))
        Dim histo As New AntecedentHisto With {.Etat = 9}

        AntecedentHistoCreationDao.InitAntecedentHistorisation(lu, New Utilisateur With {.UtilisateurId = 77}, histo)

        Assert.AreEqual(77, histo.UtilisateurId)
        Assert.AreEqual(0, histo.Etat, "remis à zéro")
        Assert.AreEqual(Date.Today, histo.HistorisationDate.Date)
        Assert.AreEqual(lu.Id, histo.AntecedentId)
        Assert.AreEqual(lu.PatientId, histo.PatientId)
        Assert.AreEqual("C", histo.Type)
        Assert.AreEqual(lu.DrcId.ToString, histo.DrcId)
        Assert.AreEqual("Tabac", histo.Description)
        Assert.AreEqual(lu.DateCreation, histo.DateCreation)
        Assert.AreEqual(lu.UserCreation, histo.UtilisateurCreation)
        Assert.AreEqual(lu.DateDebut, histo.DateDebut)
        Assert.AreEqual(FinContexteDeTest, histo.DateFin.Date)
        Assert.AreEqual("Patient", histo.Nature)
        Assert.AreEqual(1, histo.Niveau)
        Assert.AreEqual(30, histo.Ordre1)
        Assert.AreEqual("P", histo.StatutAffichage)
        Assert.AreEqual("B", histo.Categorie, "CategorieContexte devient Categorie")
        Assert.IsFalse(histo.Inactif)
        Assert.AreEqual(lu.ChaineEpisodeDateFin, histo.ChaineEpisodeDateFin)
    End Sub

    ''' <summary>Première ligne d'oa_antecedent pour cet id, lue sous le compte Client, passée à InitClasseAntecedentHistorisation.</summary>
    Private Shared Function InitDepuisLaBase(idAntecedent As Long, idUtilisateur As Long) As AntecedentHisto
        Dim histo As New AntecedentHisto
        Using connexion As New SqlConnection(ChaineConnexion(Compte.Client))
            connexion.Open()
            Using commande As New SqlCommand("SELECT * FROM oasis.oa_antecedent WHERE oa_antecedent_id = @id", connexion)
                commande.Parameters.AddWithValue("@id", idAntecedent)
                Using lecteur = commande.ExecuteReader()
                    Assert.IsTrue(lecteur.Read())
                    AntecedentHistoCreationDao.InitClasseAntecedentHistorisation(
                        lecteur, New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}, histo)
                End Using
            End Using
        End Using
        Return histo
    End Function

    <TestMethod()> Public Sub InitClasseAntecedentHistorisation_RecopieUnContexte()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idDrc = CreerDrc()
        Dim idContexte = CreerContexteMedical(idPatient, idUtilisateur, idDrc, "Vit seul", categorie:="B", ordre1:=30)

        Dim histo = InitDepuisLaBase(idContexte, idUtilisateur)

        Assert.AreEqual(CInt(idUtilisateur), histo.UtilisateurId)
        Assert.AreEqual(0, histo.Etat)
        Assert.AreEqual(CInt(idContexte), histo.AntecedentId)
        Assert.AreEqual(CInt(idPatient), histo.PatientId)
        Assert.AreEqual("C", histo.Type)
        Assert.AreEqual(idDrc.ToString, histo.DrcId)
        Assert.AreEqual("Vit seul", histo.Description)
        Assert.AreEqual(Date.Today, histo.DateCreation.Date)
        Assert.AreEqual(CInt(idUtilisateur), histo.UtilisateurCreation)
        Assert.AreEqual(DateDebutAntecedentDeTest, histo.DateDebut)
        Assert.AreEqual(FinContexteDeTest, histo.DateFin.Date)
        Assert.AreEqual("Patient", histo.Nature)
        Assert.AreEqual(1, histo.Niveau)
        Assert.AreEqual(30, histo.Ordre1)
        Assert.AreEqual("P", histo.StatutAffichage)
        Assert.AreEqual("B", histo.Categorie)
        Assert.IsFalse(histo.Inactif)
    End Sub

    <TestMethod()> Public Sub InitClasseAntecedentHistorisation_ColonnesNullesDeviennentDesValeursParDefaut()
        ' CreationAntecedent n'écrit ni date de fin, ni arrêt, ni commentaire d'arrêt,
        ' ni catégorie, ni date de modification : ces colonnes restent NULL.
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idAntecedent = CreerAntecedent(CreerPatient(), idUtilisateur)

        Dim histo = InitDepuisLaBase(idAntecedent, idUtilisateur)

        Assert.AreEqual(New Date(1, 1, 1), histo.DateFin)
        Assert.AreEqual(New Date(1, 1, 1), histo.DateModification)
        Assert.IsFalse(histo.Arret)
        Assert.AreEqual("", histo.ArretCommentaire)
        Assert.AreEqual("", histo.Categorie)
        Assert.AreEqual(0, histo.UtilisateurModification)
        Assert.AreEqual(0, histo.Niveau1Id)
        Assert.AreEqual(0, histo.Niveau2Id)
        Assert.AreEqual(980, histo.Ordre1)
        Assert.AreEqual(0, histo.AldId)
        Assert.IsFalse(histo.AldValide)
        Assert.AreEqual(Date.MaxValue.Date, histo.AldDateDebut.Date)
        Assert.AreEqual(Date.Today.AddMonths(6), histo.ChaineEpisodeDateFin.Date)
    End Sub

End Class
