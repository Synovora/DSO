Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Threading
Imports Oasis_Common

''' <summary>
''' PPSHistoCreationDao contre la base de test. CreationPPSHisto est appelée par
''' PpsDao dans le client lourd, sous oasis_client. Elle écrit la date de début en
''' texte selon la culture courante : les tests tournent en français comme le
''' client lourd.
''' </summary>
<TestClass()> Public Class PPSHistoCreationDaoTest
    Inherits TestIntegration

    Private cultureInitiale As CultureInfo

    <TestInitialize>
    Public Sub PasserEnFrancais()
        cultureInitiale = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
    End Sub

    <TestCleanup>
    Public Sub RetablirLaCulture()
        If cultureInitiale IsNot Nothing Then Thread.CurrentThread.CurrentCulture = cultureInitiale
    End Sub

    <TestMethod()> Public Sub CreationPPSHisto_EcritLaLigneTelleQuelle()
        Dim idConnecte = CreerUtilisateur(avecCle:=False)
        Dim idAutre = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idPps = CreerPps(idPatient, idConnecte, 4, 9)
        Dim idDrc = CreerDrc()
        Dim histo As New PpsHisto With {
            .HistorisationUtilisateurId = CInt(idAutre),
            .HistorisationEtat = PpsHisto.EnumEtatPPSHisto.Creation,
            .PpsId = CInt(idPps),
            .PatientId = CInt(idPatient),
            .Categorie = 4,
            .SousCategorie = 9,
            .Priorite = 6,
            .DrcId = CInt(idDrc),
            .AffichageSynthese = False,
            .Commentaire = "Arrêt du traitement",
            .DateDebut = New Date(2021, 4, 4),
            .Arret = True,
            .ArretCommentaire = "Effets indésirables",
            .Inactif = True
        }

        Assert.IsTrue(PPSHistoCreationDao.CreationPPSHisto(histo, New Utilisateur With {.UtilisateurId = CInt(idConnecte)},
                                                           PpsHisto.EnumEtatPPSHisto.Arret))

        Dim table = HistoriquePps(idPps)
        Assert.AreEqual(2, table.Rows.Count, "création du PPS, puis la ligne écrite ici")
        Dim ligne = table.Rows(1)
        Assert.AreEqual(3, CInt(ligne("oa_pps_histo_etat_historisation")), "l'état vient du paramètre, pas de l'objet")
        Assert.AreEqual(idConnecte, CLng(ligne("oa_pps_histo_utilisateur_historisation")),
                        "l'auteur est l'utilisateur connecté, pas HistorisationUtilisateurId")
        Assert.AreEqual(Date.Today, CDate(ligne("oa_pps_histo_date_historisation")).Date)
        Assert.AreEqual(idPatient, CLng(ligne("oa_pps_patient_id")))
        Assert.AreEqual(4, CInt(ligne("oa_pps_categorie")))
        Assert.AreEqual(9, CInt(ligne("oa_pps_sous_categorie")))
        Assert.AreEqual(6, CInt(ligne("oa_pps_priorite")))
        Assert.AreEqual(idDrc, CLng(ligne("oa_pps_drc_id")))
        Assert.IsFalse(CBool(ligne("oa_pps_affichage_synthese")))
        Assert.AreEqual("Arrêt du traitement", CStr(ligne("oa_pps_commentaire")))
        ' La date part en texte selon la culture (« 04/04/2021 00:00:00 ») et SQL Server
        ' la relit selon la langue du login : jour et mois égaux pour que le test ne
        ' dépende pas de cette langue.
        Assert.AreEqual(New Date(2021, 4, 4), CDate(ligne("oa_pps_date_debut")).Date)
        Assert.IsTrue(CBool(ligne("oa_pps_arret")))
        Assert.AreEqual("Effets indésirables", CStr(ligne("oa_pps_commentaire_arret")))
        Assert.IsTrue(CBool(ligne("oa_pps_inactif")))
    End Sub

    ''' <summary>Ligne d'oa_patient_pps lue sous le compte Client et passée à InitClassePPStHistorisation.</summary>
    Private Shared Function InitDepuisLaBase(idPps As Long, idUtilisateur As Long) As PpsHisto
        Dim histo As New PpsHisto With {.HistorisationEtat = 9}
        Using connexion As New SqlConnection(ChaineConnexion(Compte.Client))
            connexion.Open()
            Using commande As New SqlCommand("SELECT * FROM oasis.oa_patient_pps WHERE oa_pps_id = @id", connexion)
                commande.Parameters.AddWithValue("@id", idPps)
                Using lecteur = commande.ExecuteReader()
                    Assert.IsTrue(lecteur.Read())
                    PPSHistoCreationDao.InitClassePPStHistorisation(
                        lecteur, New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}, histo)
                End Using
            End Using
        End Using
        Return histo
    End Function

    <TestMethod()> Public Sub InitClassePPStHistorisation_RecopieLaLigneEtRemplaceLesNull()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idDrc = CreerDrc()
        Dim idPps = CreerPps(idPatient, idUtilisateur, 4, 11, priorite:=2, drcId:=idDrc, commentaire:="Bilan")

        Dim histo = InitDepuisLaBase(idPps, idUtilisateur)

        Assert.AreEqual(CInt(idUtilisateur), histo.HistorisationUtilisateurId)
        Assert.AreEqual(0, histo.HistorisationEtat, "remis à zéro")
        Assert.AreEqual(Date.Today, histo.HistorisationDate.Date)
        Assert.AreEqual(CInt(idPps), histo.PpsId)
        Assert.AreEqual(CInt(idPatient), histo.PatientId)
        Assert.AreEqual(4, histo.Categorie)
        Assert.AreEqual(11, histo.SousCategorie)
        Assert.AreEqual(2, histo.Priorite)
        Assert.AreEqual(CInt(idDrc), histo.DrcId)
        Assert.IsTrue(histo.AffichageSynthese)
        Assert.AreEqual("Bilan", histo.Commentaire)
        ' Colonnes que CreationPPS n'écrit pas.
        Assert.AreEqual(New Date(1, 1, 1), histo.DateDebut)
        Assert.IsFalse(histo.Arret)
        Assert.AreEqual("", histo.ArretCommentaire)
        Assert.IsFalse(histo.Inactif)
    End Sub

    <TestMethod()> Public Sub InitClassePPStHistorisation_PpsAnnule()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPps = CreerPps(CreerPatient(), idUtilisateur, 2, 2)
        Dim daoPps As New PpsDao
        Dim lu = daoPps.getPpsById(CInt(idPps))
        lu.ArretCommentaire = "Fait"
        daoPps.AnnulationPrevention(lu, New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)})

        Dim histo = InitDepuisLaBase(idPps, idUtilisateur)

        Assert.IsTrue(histo.Arret)
        Assert.AreEqual("Fait", histo.ArretCommentaire)
        Assert.IsTrue(histo.Inactif)
    End Sub

End Class
