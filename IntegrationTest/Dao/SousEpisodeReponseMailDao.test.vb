Imports Oasis_Common

''' <summary>
''' SousEpisodeReponseMailDao contre la base. Les courriels de réponse arrivent par
''' l'intégration des mails ; le client lourd les liste, les ouvre, les écarte ou
''' les marque traités dans FrmSousEpisodeReponseAttribution : tout tourne sous
''' oasis_client. Aucun DAO n'écrit ces lignes, JeuxSousEpisode les insère.
''' </summary>
<TestClass()> Public Class SousEpisodeReponseMailDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New SousEpisodeReponseMailDao

    Private Const MailAbsent As Long = 987654321

    Private Shared Function Statut(idMail As Long) As String
        Return CStr(Scalaire("SELECT status FROM oasis.oa_sous_episode_reponse_mail WHERE id = @p0", idMail))
    End Function

    ' --- GetLstSousEpisodeReponseMail ------------------------------------------------

    <TestMethod()> Public Sub GetLst_SansPage_NeRenvoieQueLesNonTraites()
        Dim idPatient = CreerPatient()
        Dim quand As New Date(2026, 3, 5, 7, 45, 0)
        Dim attente = CreerMailReponseSousEpisode(auteur:="labo@exemple.fr", objet:="Bilan sanguin",
                                                  patientId:=idPatient, horodate:=quand)
        Dim traite = CreerMailReponseSousEpisode(statut:="processed")
        Dim ecarte = CreerMailReponseSousEpisode(statut:="deleted")

        Dim liste = dao.GetLstSousEpisodeReponseMail()

        Dim ids = liste.Select(Function(m) m.Id).ToList()
        CollectionAssert.Contains(ids, attente)
        CollectionAssert.DoesNotContain(ids, traite)
        CollectionAssert.DoesNotContain(ids, ecarte)
        Assert.IsTrue(liste.All(Function(m) m.Status = "unprocessed"))
        Dim relu = liste.Single(Function(m) m.Id = attente)
        Assert.AreEqual("labo@exemple.fr", relu.Auteur)
        Assert.AreEqual("Bilan sanguin", relu.Objet)
        Assert.AreEqual(idPatient, relu.PatientId)
        Assert.AreEqual(quand, relu.HorodateCreation)
        Assert.IsNull(relu.Corps, "le corps n'est pas lu dans la liste")
    End Sub

    <TestMethod()> Public Sub GetLst_ParPage_CinquanteParPageParIdCroissant()
        Dim crees As New List(Of Long)
        For i = 1 To 52
            crees.Add(CreerMailReponseSousEpisode(objet:="Mail " & i))
        Next
        Dim tous = dao.GetLstSousEpisodeReponseMail().Select(Function(m) m.Id).OrderBy(Function(x) x).ToList()

        Dim premiere = dao.GetLstSousEpisodeReponseMail(1).Select(Function(m) m.Id).ToList()
        Dim deuxieme = dao.GetLstSousEpisodeReponseMail(2).Select(Function(m) m.Id).ToList()

        CollectionAssert.AreEqual(tous.Take(50).ToList(), premiere)
        CollectionAssert.AreEqual(tous.Skip(50).Take(50).ToList(), deuxieme)
        CollectionAssert.IsSubsetOf(crees, tous)
    End Sub

    <TestMethod()> Public Sub GetLst_PageAuDelaDuDernier_DonneUneListeVide()
        CreerMailReponseSousEpisode()
        Dim nbPages = CInt(Math.Ceiling(dao.GetLstSousEpisodeReponseMail().Count / 50.0))

        Assert.AreEqual(0, dao.GetLstSousEpisodeReponseMail(nbPages + 1).Count)
    End Sub

    <TestMethod()> Public Sub GetLst_ObjetEtPatientNuls_DonnentVideEtZero()
        Dim id = CreerMailReponseSousEpisode(objet:=Nothing)

        Dim relu = dao.GetLstSousEpisodeReponseMail().Single(Function(m) m.Id = id)

        Assert.AreEqual("", relu.Objet)
        Assert.AreEqual(0L, relu.PatientId)
    End Sub

    ' --- GetSousEpisodeReponseMailById ------------------------------------------------

    <TestMethod()> Public Sub GetById_RelitAussiLeCorps()
        Dim id = CreerMailReponseSousEpisode(corps:="Veuillez trouver ci-joint les resultats.")

        Dim relu = dao.GetSousEpisodeReponseMailById(id)

        Assert.AreEqual(id, relu.Id)
        Assert.AreEqual("unprocessed", relu.Status)
        Assert.AreEqual("laboratoire@exemple.fr", relu.Auteur)
        Assert.AreEqual("Veuillez trouver ci-joint les resultats.", relu.Corps)
    End Sub

    <TestMethod()> Public Sub GetById_CorpsNull_DonneUneChaineVide()
        Dim id = CreerMailReponseSousEpisode(corps:=Nothing)
        Assert.AreEqual("", dao.GetSousEpisodeReponseMailById(id).Corps)
    End Sub

    <TestMethod()> Public Sub GetById_MailTraite_ResteLisible()
        Dim id = CreerMailReponseSousEpisode(statut:="processed")
        Assert.AreEqual("processed", dao.GetSousEpisodeReponseMailById(id).Status)
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentException))>
    Public Sub GetById_Inexistant_Leve()
        dao.GetSousEpisodeReponseMailById(MailAbsent)
    End Sub

    ' --- Changements de statut ---------------------------------------------------------

    <TestMethod()> Public Sub Delete_MarqueLeMailSansLeSupprimer()
        Dim id = CreerMailReponseSousEpisode()
        Dim voisin = CreerMailReponseSousEpisode()

        dao.DeleteSousEpisodeReponseMailById(id)

        Assert.AreEqual("deleted", Statut(id), "la ligne reste, seul le statut change")
        Assert.AreEqual("unprocessed", Statut(voisin))
        CollectionAssert.DoesNotContain(dao.GetLstSousEpisodeReponseMail().Select(Function(m) m.Id).ToList(), id)
    End Sub

    <TestMethod()> Public Sub Process_MarqueLeMailTraite()
        Dim id = CreerMailReponseSousEpisode()

        dao.ProcessSousEpisodeReponseMailById(id)

        Assert.AreEqual("processed", Statut(id))
        CollectionAssert.DoesNotContain(dao.GetLstSousEpisodeReponseMail().Select(Function(m) m.Id).ToList(), id)
    End Sub

    <TestMethod()> Public Sub ChangementsDeStatut_MailInexistant_SansErreurNiEffet()
        ' Aucune vérification du nombre de lignes touchées.
        Dim id = CreerMailReponseSousEpisode()

        dao.DeleteSousEpisodeReponseMailById(MailAbsent)
        dao.ProcessSousEpisodeReponseMailById(MailAbsent)

        Assert.AreEqual("unprocessed", Statut(id))
    End Sub

End Class
