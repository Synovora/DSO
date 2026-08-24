Imports Oasis_Common

''' <summary>
''' Lecture d'une ligne d'utilisateur par UserDao.LireLigne.
'''
''' C'est la lecture la plus sensible du dépôt : la ligne porte l'empreinte du
''' mot de passe et la clé privée de signature. Le compte SQL du client lourd
''' ne peut pas lire ces colonnes, et LireLigne ne doit pas les demander non
''' plus, sauf côté serveur où inclureSecrets est vrai. Une clé absente doit
''' rester absente : une valeur de repli rendrait toute signature falsifiable.
''' </summary>
<TestClass()> Public Class TestUserDaoLecture

    Private Shared ReadOnly ColonnesClient As String() = {
        "oa_utilisateur_id", "oa_utilisateur_nom", "oa_utilisateur_prenom", "oa_utilisateur_telephone",
        "oa_utilisateur_fax", "oa_utilisateur_mail", "oa_utilisateur_profil_id", "oa_utilisateur_admin",
        "oa_utilisateur_login", "oa_utilisateur_site_id", "oa_utilisateur_unite_sanitaire_id",
        "oa_utilisateur_siege_id", "oa_utilisateur_rpps", "oa_utilisateur_password_is_unique_usage",
        "oa_utilisateur_tentatives", "oa_utilisateur_verrou_jusqua", "cle_publique",
        "oa_r_profil_fonction_id_defaut", "oa_r_profil_niveau_acces", "oa_r_profil_type"}

    Private Shared ReadOnly ColonnesSecretes As String() = {"oa_password", "cle_privee"}

    Private Shared Function Complet() As Dictionary(Of String, Object)
        Return New Dictionary(Of String, Object) From {
            {"oa_utilisateur_id", 12}, {"oa_utilisateur_nom", "Dupont"}, {"oa_utilisateur_prenom", "Jean"},
            {"oa_utilisateur_telephone", "0102030405"}, {"oa_utilisateur_fax", "0102030406"},
            {"oa_utilisateur_mail", "jean.dupont@exemple.fr"}, {"oa_utilisateur_profil_id", "MED"},
            {"oa_utilisateur_admin", True}, {"oa_utilisateur_login", "jdupont"}, {"oa_utilisateur_site_id", 3},
            {"oa_utilisateur_unite_sanitaire_id", 4}, {"oa_utilisateur_siege_id", 5},
            {"oa_utilisateur_rpps", "10001234567"}, {"oa_utilisateur_password_is_unique_usage", True},
            {"oa_utilisateur_tentatives", 2}, {"oa_utilisateur_verrou_jusqua", New Date(2030, 1, 1, 12, 0, 0)},
            {"cle_publique", "0xpublique"}, {"oa_r_profil_fonction_id_defaut", 8L},
            {"oa_r_profil_niveau_acces", 1}, {"oa_r_profil_type", "MEDICAL"},
            {"oa_password", "  empreinte  "}, {"cle_privee", "0xprivee"}}
    End Function

    <TestMethod()> Public Sub UneLigneCompleteEstLueChampParChamp()
        Dim u = UserDao.LireLigne(LigneDeTest.Ligne(ColonnesClient, Complet()))

        Assert.AreEqual(12, u.UtilisateurId)
        Assert.AreEqual("Dupont", u.UtilisateurNom)
        Assert.AreEqual("Jean", u.UtilisateurPrenom)
        Assert.AreEqual("0102030405", u.UtilisateurTelephone)
        Assert.AreEqual("0102030406", u.UtilisateurFax)
        Assert.AreEqual("jean.dupont@exemple.fr", u.UtilisateurMail)
        Assert.AreEqual("MED", u.UtilisateurProfilId)
        Assert.IsTrue(u.UtilisateurAdmin)
        Assert.AreEqual("jdupont", u.UtilisateurLogin)
        Assert.AreEqual(3, u.UtilisateurSiteId)
        Assert.AreEqual(4, u.UtilisateurUniteSanitaireId)
        Assert.AreEqual(5, u.UtilisateurSiegeId)
        Assert.AreEqual("10001234567", u.UtilisateurRPPS)
        Assert.IsTrue(u.IsPasswordUniqueUsage)
        Assert.AreEqual(2, u.Tentatives)
        Assert.IsTrue(u.VerrouJusqua.HasValue)
        Assert.AreEqual(New Date(2030, 1, 1, 12, 0, 0), u.VerrouJusqua.Value)
        Assert.AreEqual("0xpublique", u.UtilisateurAddress)
        Assert.AreEqual(8L, u.FonctionParDefautId)
        Assert.AreEqual(1, u.UtilisateurNiveauAcces)
        Assert.AreEqual("MEDICAL", u.TypeProfil)
    End Sub

    <TestMethod()> Public Sub ParDefautLesSecretsNeSontNiLusNiRenvoyes()
        ' La ligne n'a pas les colonnes secrètes, comme côté client. La lecture
        ' ne doit pas les demander.
        Dim u = UserDao.LireLigne(LigneDeTest.Ligne(ColonnesClient, Complet()))
        Assert.IsNull(u.Password)
        Assert.AreEqual("", u.UtilisateurClePrivee)
    End Sub

    <TestMethod()> Public Sub LesSecretsPresentsSontIgnoresSansLeDrapeau()
        ' Même si la requête les rapporte, sans inclureSecrets ils ne montent pas
        ' dans le bean, donc pas dans la réponse de /api/login.
        Dim u = UserDao.LireLigne(LigneDeTest.Ligne(ColonnesClient.Concat(ColonnesSecretes), Complet()))
        Assert.IsNull(u.Password)
        Assert.AreEqual("", u.UtilisateurClePrivee)
    End Sub

    <TestMethod()> Public Sub AvecLeDrapeauLesSecretsSontLus()
        Dim u = UserDao.LireLigne(LigneDeTest.Ligne(ColonnesClient.Concat(ColonnesSecretes), Complet()), inclureSecrets:=True)
        Assert.AreEqual("empreinte", u.Password, "l'empreinte est stockée sur une colonne à largeur fixe : espaces retirés")
        Assert.AreEqual("0xprivee", u.UtilisateurClePrivee)
    End Sub

    <TestMethod()> Public Sub UneCleAbsenteResteAbsente()
        ' Aucune clé de repli, ni privée ni publique. Les versions précédentes
        ' retombaient sur une clé publiquement connue.
        Dim valeurs = Complet()
        valeurs.Remove("cle_privee")
        valeurs.Remove("cle_publique")
        Dim u = UserDao.LireLigne(LigneDeTest.Ligne(ColonnesClient.Concat(ColonnesSecretes), valeurs), inclureSecrets:=True)
        Assert.AreEqual("", u.UtilisateurClePrivee)
        Assert.AreEqual("", u.UtilisateurAddress)
    End Sub

    <TestMethod()> Public Sub SansProfilJointLAccesEstAuNiveauLePlusRestreint()
        ' Profil absent ou inactif : la jointure rend NULL. Le niveau d'accès
        ' retombe sur 3, le plus restreint, et le type de profil reste vide,
        ' donc HabilitationsDocuments refuse.
        Dim valeurs = Complet()
        For Each colonne In {"oa_r_profil_fonction_id_defaut", "oa_r_profil_niveau_acces", "oa_r_profil_type"}
            valeurs.Remove(colonne)
        Next
        Dim u = UserDao.LireLigne(LigneDeTest.Ligne(ColonnesClient, valeurs))
        Assert.AreEqual(3, u.UtilisateurNiveauAcces)
        Assert.AreEqual("", u.TypeProfil)
        Assert.AreEqual(0L, u.FonctionParDefautId)
    End Sub

    <TestMethod()> Public Sub UneLigneVideDonneLesValeursParDefaut()
        Dim u = UserDao.LireLigne(LigneDeTest.Ligne(ColonnesClient, New Dictionary(Of String, Object) From {{"oa_utilisateur_id", 1}}))

        Assert.AreEqual(1, u.UtilisateurId)
        For Each texte In {u.UtilisateurNom, u.UtilisateurPrenom, u.UtilisateurTelephone, u.UtilisateurFax,
                           u.UtilisateurMail, u.UtilisateurProfilId, u.UtilisateurLogin, u.UtilisateurRPPS,
                           u.UtilisateurAddress, u.TypeProfil}
            Assert.AreEqual("", texte)
        Next
        Assert.IsFalse(u.UtilisateurAdmin)
        Assert.IsFalse(u.IsPasswordUniqueUsage)
        Assert.AreEqual(0, u.UtilisateurSiteId)
        Assert.AreEqual(0, u.UtilisateurUniteSanitaireId)
        Assert.AreEqual(0, u.UtilisateurSiegeId)
        Assert.AreEqual(0, u.Tentatives)
        Assert.IsFalse(u.VerrouJusqua.HasValue, "pas de verrou en base : pas de verrou")
    End Sub

    <TestMethod()> <ExpectedException(GetType(InvalidCastException))>
    Public Sub UneLigneSansIdentifiantEstUneErreur()
        UserDao.LireLigne(LigneDeTest.Ligne(ColonnesClient, Nothing))
    End Sub

End Class
