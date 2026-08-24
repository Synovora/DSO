Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne de sous-épisode par SousEpisodeDao.BuildBean.
'''
''' Deux particularités : quatre colonnes n'existent que dans certaines
''' requêtes et sont lues via HasColumn, et le délai de réponse tombe sur la
''' configuration (DelaiDefautReponseSousEpisode, 15 dans app.config) quand la
''' base n'en porte pas.
''' </summary>
<TestClass()> Public Class TestSousEpisodeDaoLecture

    Private Shared ReadOnly ColonnesDeBase As String() = {
        "id", "episode_id", "id_intervenant", "id_sous_episode_type", "id_sous_episode_sous_type",
        "create_user_id", "horodate_creation", "last_update_user_id", "horodate_last_update",
        "validate_user_id", "horodate_validate", "commentaire", "is_ald", "is_reponse",
        "delai_since_validation", "is_reponse_recue", "horodate_last_recu", "is_inactif",
        "signature", "reference", "sous_type_libelle", "type_libelle"}

    Private Shared ReadOnly ColonnesOptionnelles As String() = {
        "user_create", "nb_reponse", "nb_reponse_waiting", "nb_med_reponse_waiting"}

    ''' <summary>Les colonnes sans valeur de repli dans BuildBean.</summary>
    Private Shared Function Obligatoires() As Dictionary(Of String, Object)
        Return New Dictionary(Of String, Object) From {
            {"id", 7L}, {"episode_id", 3L}, {"id_sous_episode_type", 1L}, {"id_sous_episode_sous_type", 2L},
            {"create_user_id", 9L}, {"horodate_creation", New Date(2024, 5, 6, 7, 8, 9)}, {"is_ald", False}}
    End Function

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim valeurs = Obligatoires()
        For Each paire In New Dictionary(Of String, Object) From {
            {"id_intervenant", 4L}, {"last_update_user_id", 10L}, {"horodate_last_update", New Date(2024, 6, 1)},
            {"validate_user_id", 11L}, {"horodate_validate", New Date(2024, 6, 2)}, {"commentaire", "Courrier envoyé"},
            {"is_reponse", True}, {"delai_since_validation", 30}, {"is_reponse_recue", True},
            {"horodate_last_recu", New Date(2024, 6, 3)}, {"is_inactif", True}, {"signature", "sig"},
            {"reference", "REF-1"}, {"sous_type_libelle", "Courrier"}, {"type_libelle", "Sortant"},
            {"user_create", "jdupont"}, {"nb_reponse", 2L}, {"nb_reponse_waiting", 1L}, {"nb_med_reponse_waiting", 1L}}
            valeurs(paire.Key) = paire.Value
        Next

        Dim se = SousEpisodeDao.BuildBean(LigneDeTest.Ligne(ColonnesDeBase.Concat(ColonnesOptionnelles), valeurs))

        Assert.AreEqual(7L, se.Id)
        Assert.AreEqual(3L, se.EpisodeId)
        Assert.AreEqual(4L, se.IdIntervenant)
        Assert.AreEqual(1L, se.IdSousEpisodeType)
        Assert.AreEqual(2L, se.IdSousEpisodeSousType)
        Assert.AreEqual(9L, se.CreateUserId)
        Assert.AreEqual(New Date(2024, 5, 6, 7, 8, 9), se.HorodateCreation)
        Assert.AreEqual(10L, se.LastUpdateUserId)
        Assert.AreEqual(New Date(2024, 6, 1), se.HorodateLastUpdate)
        Assert.AreEqual(11L, se.ValidateUserId)
        Assert.AreEqual(New Date(2024, 6, 2), se.HorodateValidate)
        Assert.AreEqual("Courrier envoyé", se.Commentaire)
        Assert.IsFalse(se.IsALD)
        Assert.IsTrue(se.IsReponse)
        Assert.AreEqual(30, se.DelaiSinceValidation)
        Assert.IsTrue(se.IsReponseRecue)
        Assert.AreEqual(New Date(2024, 6, 3), se.HorodateLastRecu)
        Assert.IsTrue(se.isInactif)
        Assert.AreEqual("sig", se.Signature)
        Assert.AreEqual("REF-1", se.Reference)
        Assert.AreEqual("Courrier", se.SousTypeLibelle)
        Assert.AreEqual("Sortant", se.TypeLibelle)
        Assert.AreEqual("jdupont", se.UserCreate)
        Assert.AreEqual(2L, se.NbReponse)
        Assert.AreEqual(1L, se.NbReponseWaiting)
        Assert.AreEqual(1L, se.NbMedReponseWaiting)
    End Sub

    <TestMethod()> Public Sub LesColonnesFacultativesAbsentesDonnentLeursDefauts()
        ' La requête de liste ne joint pas les compteurs : HasColumn doit les
        ' remplacer sans lever d'exception.
        Dim se = SousEpisodeDao.BuildBean(LigneDeTest.Ligne(ColonnesDeBase, Obligatoires()))

        Assert.AreEqual("", se.UserCreate)
        Assert.AreEqual(0L, se.NbReponse)
        Assert.AreEqual(0L, se.NbReponseWaiting)
        Assert.AreEqual(0L, se.NbMedReponseWaiting)
    End Sub

    <TestMethod()> Public Sub UneLigneMinimaleDonneLesValeursParDefaut()
        Dim se = SousEpisodeDao.BuildBean(LigneDeTest.Ligne(ColonnesDeBase, Obligatoires()))

        Assert.AreEqual(0L, se.IdIntervenant)
        Assert.AreEqual(0L, se.LastUpdateUserId)
        Assert.AreEqual(0L, se.ValidateUserId)
        Assert.AreEqual(Date.MinValue, se.HorodateLastUpdate)
        Assert.AreEqual(Date.MinValue, se.HorodateValidate)
        Assert.AreEqual(Date.MinValue, se.HorodateLastRecu)
        Assert.AreEqual("", se.Commentaire)
        Assert.AreEqual("", se.SousTypeLibelle)
        Assert.AreEqual("", se.TypeLibelle)
        Assert.IsFalse(se.IsReponse)
        Assert.IsFalse(se.IsReponseRecue)
        Assert.IsFalse(se.isInactif)
        ' "NaN" est le marqueur historique d'absence, les écrans le testent tel quel.
        Assert.AreEqual("NaN", se.Signature)
        Assert.AreEqual("NaN", se.Reference)
    End Sub

    <TestMethod()> Public Sub LeDelaiDeReponseAbsentVientDeLaConfiguration()
        Dim se = SousEpisodeDao.BuildBean(LigneDeTest.Ligne(ColonnesDeBase, Obligatoires()))
        Assert.AreEqual(15, se.DelaiSinceValidation)
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansHorodatageDeCreationEstUneErreur()
        Dim valeurs = Obligatoires()
        valeurs.Remove("horodate_creation")
        SousEpisodeDao.BuildBean(LigneDeTest.Ligne(ColonnesDeBase, valeurs))
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdentifiantEstUneErreur()
        Dim valeurs = Obligatoires()
        valeurs.Remove("id")
        SousEpisodeDao.BuildBean(LigneDeTest.Ligne(ColonnesDeBase, valeurs))
    End Sub

End Class
