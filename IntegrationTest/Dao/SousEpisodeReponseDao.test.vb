Imports System.IO
Imports Oasis_Common

''' <summary>
''' SousEpisodeReponseDao contre la base. Le client lourd lit, valide, fait valider
''' et supprime les réponses (FrmSousEpisode, FrmSousEpisodeListe) et en crée
''' (FrmSousEpisode, FrmSousEpisodeReponseAttribution) : ces appels tournent sous
''' oasis_client. Le portail patient (ResultatsController) appelle
''' GetReponseCompleteByUser et GetAllFilterByUser : sous oasis_web.
'''
''' Le document d'une réponse vit sur le serveur de fichiers. Create et
''' CreateByMoving déposent ou renomment le fichier dans la transaction qui écrit
''' la ligne : seule leur partie base est testée, en faisant échouer l'appel au
''' serveur (ServeurOasis pointe sur tests.invalid). getContenu ne fait que
''' télécharger et n'est pas testé ici.
''' </summary>
<TestClass()> Public Class SousEpisodeReponseDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New SousEpisodeReponseDao

    Private Const ReponseAbsente As Long = 987654321

    ''' <summary>Auteur et sous-épisode dans un épisode d'un patient neuf.</summary>
    Private Class DossierReponse
        Public UtilisateurId As Long
        Public PatientId As Long
        Public EpisodeId As Long
        Public SousEpisodeId As Long
    End Class

    Private Shared Function NouveauDossier(Optional sousTypeId As Long = SousTypeSeAdressage) As DossierReponse
        Dim dossier As New DossierReponse
        dossier.UtilisateurId = CreerUtilisateur(avecCle:=False)
        dossier.PatientId = CreerPatient()
        dossier.EpisodeId = CreerEpisode(dossier.PatientId, dossier.UtilisateurId)
        dossier.SousEpisodeId = CreerSousEpisode(dossier.EpisodeId, dossier.UtilisateurId, sousTypeId)
        Return dossier
    End Function

    Private Shared Function ValeurReponse(colonne As String, idReponse As Long) As Object
        Return Scalaire("SELECT " & colonne & " FROM oasis.oa_sous_episode_reponse WHERE id = @p0", idReponse)
    End Function

    Private Shared Function ValeurSousEpisode(colonne As String, idSousEpisode As Long) As Object
        Return Scalaire("SELECT " & colonne & " FROM oasis.oa_sous_episode WHERE id = @p0", idSousEpisode)
    End Function

    Private Shared Function NombreDeReponses(idSousEpisode As Long) As Integer
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_sous_episode_reponse WHERE id_sous_episode = @p0", idSousEpisode))
    End Function

    Private Shared Sub VerifierProche(attendu As Date, lu As Date, Optional message As String = "")
        Assert.IsTrue(Math.Abs((lu - attendu).TotalSeconds) < 2, message & " : attendu " & attendu & ", lu " & lu)
    End Sub

    ''' <summary>
    ''' Le portail interroge [oasis].[oasis].oa_... : nom de base écrit en dur. Sur une
    ''' base de test nommée autrement (oasis_it par défaut), la requête vise une autre
    ''' base ; ces tests ne peuvent tourner que sur une base nommée oasis.
    ''' </summary>
    Private Shared Sub ExigerUneBaseNommeeOasis()
        If Not String.Equals(NomBase, "oasis", StringComparison.OrdinalIgnoreCase) Then
            Assert.Inconclusive("Requête écrite avec le nom de base [oasis] en dur : test possible seulement si OASIS_IT_DATABASE=oasis.")
        End If
    End Sub

    ' --- Lecture -----------------------------------------------------------------------

    <TestMethod()> Public Sub getLstSousEpisodeReponse_NeRenvoieQueLesReponsesDuSousEpisode()
        Dim dossier = NouveauDossier()
        Dim autre = NouveauDossier()
        Dim quand As New Date(2026, 3, 4, 14, 30, 0)
        Dim idReponse = CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId, "!", "resultat.pdf",
                                                "Resultat du bilan", quand)
        CreerReponseSousEpisode(autre.SousEpisodeId, autre.UtilisateurId)

        Dim liste = dao.getLstSousEpisodeReponse(dossier.SousEpisodeId)

        Dim relue = liste.Single()
        Assert.AreEqual(idReponse, relue.Id)
        Assert.AreEqual(dossier.SousEpisodeId, relue.IdSousEpisode)
        Assert.AreEqual(dossier.EpisodeId, relue.EpisodeId, "épisode recopié depuis le sous-épisode à l'INSERT")
        Assert.AreEqual(dossier.UtilisateurId, relue.CreateUserId)
        Assert.AreEqual(quand, relue.HorodateCreation)
        Assert.AreEqual("resultat.pdf", relue.NomFichier)
        Assert.AreEqual("Resultat du bilan", relue.Commentaire)
        Assert.AreEqual("!", relue.ValidateState)
        Assert.AreEqual(0L, relue.ValidateUserId)
        Assert.AreEqual(Date.MinValue, relue.ValidateDate)
        Assert.IsNull(relue.SousEpisodeLibelle, "colonne absente de cette requête")
    End Sub

    <TestMethod()> Public Sub getLstSousEpisodeReponse_EtatNull_DonneUneChaineVide()
        ' Comportement actuel : la requête remplace NULL par '' (COALESCE), le repli
        ' « ! » du bean ne sert donc jamais.
        Dim dossier = NouveauDossier()
        Dim idReponse = CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId, etat:=Nothing)

        Assert.AreEqual("", dao.getLstSousEpisodeReponse(dossier.SousEpisodeId, idReponse).Single().ValidateState)
    End Sub

    <TestMethod()> Public Sub getLstSousEpisodeReponse_FiltreParId()
        Dim dossier = NouveauDossier()
        CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId)
        Dim cible = CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId, "m")

        Assert.AreEqual(2, dao.getLstSousEpisodeReponse(dossier.SousEpisodeId).Count)
        Dim relue = dao.getLstSousEpisodeReponse(dossier.SousEpisodeId, cible).Single()
        Assert.AreEqual(cible, relue.Id)
        Assert.AreEqual("m", relue.ValidateState)
        Assert.AreEqual(cible, dao.getLstSousEpisodeReponse(0, cible).Single().Id)
    End Sub

    <TestMethod()> Public Sub getLstSousEpisodeReponse_SousEpisodeSansReponse_DonneUneListeVide()
        Assert.AreEqual(0, dao.getLstSousEpisodeReponse(NouveauDossier().SousEpisodeId).Count)
    End Sub

    <TestMethod()> Public Sub getTableSousEpisodeReponse_NommeLAuteurEtSansFiltreRenvoieTout()
        Dim dossier = NouveauDossier()
        Dim autre = NouveauDossier()
        Dim idReponse = CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId)
        CreerReponseSousEpisode(autre.SousEpisodeId, autre.UtilisateurId)

        Dim table = dao.getTableSousEpisodeReponse(dossier.SousEpisodeId)

        Assert.AreEqual(1, table.Rows.Count)
        Assert.AreEqual(idReponse, CLng(table.Rows(0)("id")))
        Assert.AreEqual("Utilisateur TEST", CStr(table.Rows(0)("user_create")))
        Assert.AreEqual(11, table.Columns.Count)
        Dim total = CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_sous_episode_reponse"))
        Assert.AreEqual(total, dao.getTableSousEpisodeReponse().Rows.Count)
    End Sub

    <TestMethod()> Public Sub getById_RelitLaReponse()
        Dim dossier = NouveauDossier()
        Dim idReponse = CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId, "v")

        Dim relue = dao.getById(idReponse)

        Assert.AreEqual(idReponse, relue.Id)
        Assert.AreEqual(dossier.SousEpisodeId, relue.IdSousEpisode)
        Assert.AreEqual("v", relue.ValidateState)
    End Sub

    <TestMethod()> <ExpectedException(GetType(ArgumentOutOfRangeException))>
    Public Sub getById_Inexistante_Leve()
        dao.getById(ReponseAbsente)
    End Sub

    ' --- Validation -----------------------------------------------------------------------

    <TestMethod()> Public Sub valider_PasseEnValideeAvecLeValideurEtLaDate()
        Dim dossier = NouveauDossier()
        Dim valideur = CreerUtilisateur(avecCle:=False)
        Dim idReponse = CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId)
        Dim voisine = CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId)
        Dim avant = Date.Now

        dao.valider(idReponse, New Utilisateur With {.UtilisateurId = CInt(valideur)})

        Dim relue = dao.getById(idReponse)
        Assert.AreEqual("v", relue.ValidateState)
        Assert.AreEqual(valideur, relue.ValidateUserId)
        VerifierProche(avant, relue.ValidateDate, "validate_date")
        Assert.AreEqual("!", dao.getById(voisine).ValidateState, "les autres réponses ne bougent pas")
    End Sub

    <TestMethod()> Public Sub askValider_PasseEnAttenteDeValidationMedicale()
        Dim dossier = NouveauDossier()
        Dim demandeur = CreerUtilisateur(avecCle:=False)
        Dim idReponse = CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId)
        Dim avant = Date.Now

        dao.askValider(idReponse, New Utilisateur With {.UtilisateurId = CInt(demandeur)})

        Dim relue = dao.getById(idReponse)
        Assert.AreEqual("m", relue.ValidateState)
        Assert.AreEqual(demandeur, relue.ValidateUserId)
        VerifierProche(avant, relue.ValidateDate, "validate_date")
        Assert.AreEqual(1L, New SousEpisodeDao().GetById(dossier.SousEpisodeId).NbMedReponseWaiting)
    End Sub

    <TestMethod()> Public Sub valider_ReponseInexistante_Leve()
        Try
            dao.valider(ReponseAbsente, New Utilisateur With {.UtilisateurId = 1})
            Assert.Fail("Aucune ligne touchée : une erreur est attendue.")
        Catch ex As AssertFailedException
            Throw
        Catch ex As Exception
            StringAssert.Contains(ex.Message, "0 au lieu de 1")
        End Try
    End Sub

    <TestMethod()> Public Sub askValider_ReponseInexistante_Leve()
        Try
            dao.askValider(ReponseAbsente, New Utilisateur With {.UtilisateurId = 1})
            Assert.Fail("Aucune ligne touchée : une erreur est attendue.")
        Catch ex As AssertFailedException
            Throw
        Catch ex As Exception
            StringAssert.Contains(ex.Message, "0 au lieu de 1")
        End Try
    End Sub

    ' --- Suppression (DELETE accordé au client par 2026-09-27-suppression-client-complement) ---

    <TestMethod()> Public Sub delete_DerniereReponse_RemetLeSousEpisodeEnAttente()
        Dim dossier = NouveauDossier()
        Dim idReponse = CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId)
        Dim pere = New SousEpisodeDao().GetById(dossier.SousEpisodeId)
        Assert.IsTrue(pere.IsReponseRecue)

        dao.delete(pere, idReponse, True)

        Assert.AreEqual(0, NombreDeReponses(dossier.SousEpisodeId))
        Assert.IsFalse(CBool(ValeurSousEpisode("is_reponse_recue", dossier.SousEpisodeId)))
        Assert.AreEqual(DBNull.Value, ValeurSousEpisode("horodate_last_recu", dossier.SousEpisodeId))
    End Sub

    <TestMethod()> Public Sub delete_PasLaDerniere_LaisseLeSousEpisodeInchange()
        Dim dossier = NouveauDossier()
        Dim idReponse = CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId)
        Dim restante = CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId)
        Dim pere = New SousEpisodeDao().GetById(dossier.SousEpisodeId)

        dao.delete(pere, idReponse, False)

        Assert.AreEqual(restante, dao.getLstSousEpisodeReponse(dossier.SousEpisodeId).Single().Id)
        Assert.IsTrue(CBool(ValeurSousEpisode("is_reponse_recue", dossier.SousEpisodeId)))
        Assert.AreNotEqual(DBNull.Value, ValeurSousEpisode("horodate_last_recu", dossier.SousEpisodeId))
    End Sub

    <TestMethod()> Public Sub delete_ReponseInexistante_LeveSansToucherAuSousEpisode()
        Dim dossier = NouveauDossier()
        CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId)
        Dim pere = New SousEpisodeDao().GetById(dossier.SousEpisodeId)

        Try
            dao.delete(pere, ReponseAbsente, True)
            Assert.Fail("Aucune ligne supprimée : une erreur est attendue.")
        Catch ex As AssertFailedException
            Throw
        Catch ex As Exception
            StringAssert.Contains(ex.Message, "0 au lieu de 1")
        End Try

        Assert.AreEqual(1, NombreDeReponses(dossier.SousEpisodeId))
        Assert.IsTrue(CBool(ValeurSousEpisode("is_reponse_recue", dossier.SousEpisodeId)))
    End Sub

    <TestMethod()> Public Sub delete_SousEpisodePereIntrouvable_AnnuleLaSuppression()
        ' La remise à zéro du sous-épisode échoue (aucune ligne) : la suppression,
        ' faite dans la même transaction, est annulée.
        Dim dossier = NouveauDossier()
        Dim idReponse = CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId)

        Try
            dao.delete(New SousEpisode With {.Id = 987654321}, idReponse, True)
            Assert.Fail("La remise à zéro d'un sous-épisode inexistant devrait échouer.")
        Catch ex As AssertFailedException
            Throw
        Catch ex As Exception
        End Try

        Assert.AreEqual(1, NombreDeReponses(dossier.SousEpisodeId))
    End Sub

    ' --- Création (partie base seulement) ------------------------------------------------

    <TestMethod()> Public Sub Create_FichierSourceIllisible_NeLaisseNiReponseNiIndicateur()
        Dim dossier = NouveauDossier()
        Dim pere = New SousEpisodeDao().GetById(dossier.SousEpisodeId)
        Dim reponse As New SousEpisodeReponse With {
            .IdSousEpisode = dossier.SousEpisodeId, .CreateUserId = dossier.UtilisateurId,
            .HorodateCreation = Date.Now, .NomFichier = "resultat.pdf", .Commentaire = ""}
        Dim absent = Path.Combine(Path.GetTempPath(), "oasis-it-" & Guid.NewGuid().ToString("N") & ".pdf")
        Dim indicateurAvant = ValeurSousEpisode("is_reponse_recue", dossier.SousEpisodeId)
        Dim dateAvant = ValeurSousEpisode("horodate_last_recu", dossier.SousEpisodeId)

        Try
            dao.Create(pere, reponse, absent, Nothing)
            Assert.Fail("La lecture du fichier source devrait échouer.")
        Catch ex As AssertFailedException
            Throw
        Catch ex As Exception
        End Try

        Assert.AreEqual(0, NombreDeReponses(dossier.SousEpisodeId), "INSERT annulé")
        Assert.AreEqual(indicateurAvant, ValeurSousEpisode("is_reponse_recue", dossier.SousEpisodeId),
                        "mise à jour du sous-épisode annulée")
        Assert.AreEqual(dateAvant, ValeurSousEpisode("horodate_last_recu", dossier.SousEpisodeId))
        Assert.AreEqual(0L, reponse.Id, "l'id n'est posé qu'après le dépôt")
    End Sub

    <TestMethod()> Public Sub CreateByMoving_RenommageEnEchec_NeLaisseNiReponseNiIndicateur()
        Dim dossier = NouveauDossier()
        Dim pere = New SousEpisodeDao().GetById(dossier.SousEpisodeId)
        Dim reponse As New SousEpisodeReponse With {
            .IdSousEpisode = dossier.SousEpisodeId, .CreateUserId = dossier.UtilisateurId,
            .HorodateCreation = Date.Now, .NomFichier = "piece-jointe.pdf", .Commentaire = "Attribuee depuis un mail"}
        Dim indicateurAvant = ValeurSousEpisode("is_reponse_recue", dossier.SousEpisodeId)

        Try
            dao.CreateByMoving(pere, reponse, "Mail\piece-jointe.pdf", Nothing)
            Assert.Fail("Le renommage sur tests.invalid devrait échouer.")
        Catch ex As AssertFailedException
            Throw
        Catch ex As Exception
        End Try

        Assert.AreEqual(0, NombreDeReponses(dossier.SousEpisodeId), "INSERT annulé")
        Assert.AreEqual(indicateurAvant, ValeurSousEpisode("is_reponse_recue", dossier.SousEpisodeId))
        Assert.AreEqual(0L, reponse.Id)
    End Sub

    ' --- Portail patient (serveur) ----------------------------------------------------------

    <TestMethod()> Public Sub GetReponseCompleteByUser_ReponsesDuPatientDeLaPlusRecenteALaPlusAncienne()
        ExigerUneBaseNommeeOasis()
        ' Jointure interne sur le type d'activité de l'épisode (PATHOLOGIE_AIGUE pour CreerEpisode).
        If CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_r_activite_episode WHERE oa_activite_type = 'PATHOLOGIE_AIGUE'")) = 0 Then
            Assert.Inconclusive("Type d'activité PATHOLOGIE_AIGUE absent du référentiel de l'instantané.")
        End If
        UtiliserCompte(Compte.Web)
        Dim dossier = NouveauDossier(SousTypeSeCompteRendu)
        Dim autre = NouveauDossier()
        Dim ancienne = CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId, horodate:=New Date(2026, 2, 1))
        Dim recente = CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId, horodate:=New Date(2026, 2, 3))
        CreerReponseSousEpisode(autre.SousEpisodeId, autre.UtilisateurId)

        Dim liste = dao.GetReponseCompleteByUser(CInt(dossier.PatientId))

        CollectionAssert.AreEqual(New Long() {recente, ancienne}, liste.Select(Function(r) r.Id).ToArray())
        Dim premiere = liste(0)
        Assert.AreEqual(dossier.EpisodeId, premiere.EpisodeId)
        Assert.AreEqual(dossier.SousEpisodeId, premiere.IdSousEpisode)
        Assert.AreEqual(LibelleTypeSeCourrier, premiere.SousEpisodeLibelle)
        Assert.AreEqual(LibelleSousTypeSeCompteRendu, premiere.SousEpisodeSousLibelle)
        Assert.IsNotNull(premiere.TypeActivite)
        Assert.IsNull(premiere.Conclusion, "aucun contexte d'épisode")
    End Sub

    <TestMethod()> Public Sub GetReponseCompleteByUser_PatientSansReponse_DonneUneListeVide()
        ExigerUneBaseNommeeOasis()
        UtiliserCompte(Compte.Web)
        Dim dossier = NouveauDossier()
        Assert.AreEqual(0, dao.GetReponseCompleteByUser(CInt(dossier.PatientId)).Count)
    End Sub

    <TestMethod()> Public Sub GetAllFilterByUser_CouplesDistinctsDeLibelles()
        ExigerUneBaseNommeeOasis()
        UtiliserCompte(Compte.Web)
        Dim dossier = NouveauDossier(SousTypeSeAdressage)
        CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId)
        CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId)
        Dim second = CreerSousEpisode(dossier.EpisodeId, dossier.UtilisateurId, SousTypeSeCompteRendu)
        CreerReponseSousEpisode(second, dossier.UtilisateurId)

        Dim couples = dao.GetAllFilterByUser(CInt(dossier.PatientId)).Select(Function(c) c(0) & "|" & c(1)).ToArray()

        CollectionAssert.AreEquivalent(
            New String() {LibelleTypeSeCourrier & "|" & LibelleSousTypeSeAdressage,
                          LibelleTypeSeCourrier & "|" & LibelleSousTypeSeCompteRendu},
            couples)
    End Sub

    <TestMethod()> Public Sub GetCountReponseCompleteByUser_ColonneInexistante_Echoue()
        ' Comportement actuel : la jointure porte sur P.patient_id, colonne qu'oa_patient
        ' n'a pas (sa clé est oa_patient_id). La requête échoue quoi qu'il arrive ;
        ' la méthode n'a aucun appelant.
        ExigerUneBaseNommeeOasis()
        UtiliserCompte(Compte.Web)
        Dim dossier = NouveauDossier()
        CreerReponseSousEpisode(dossier.SousEpisodeId, dossier.UtilisateurId)

        Try
            dao.GetCountReponseCompleteByUser(CInt(dossier.PatientId))
            Assert.Fail("La requête devrait être refusée par SQL Server.")
        Catch ex As AssertFailedException
            Throw
        Catch ex As System.Data.SqlClient.SqlException
        End Try
    End Sub

End Class
