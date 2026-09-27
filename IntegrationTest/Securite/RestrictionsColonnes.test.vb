Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' Droits du compte oasis_client, que /api/login remet à chaque poste.
'''
''' Une ligne de test par refus de docs/migrations/2026-08-24-comptes-sql-separes.sql
''' et par ligne du tableau de CLAUDE.md : sous Client l'opération échoue avec
''' l'erreur 229 (objet) ou 230 (colonne), les colonnes voisines restent lisibles et
''' modifiables, et sous Web la même opération passe. S'y ajoutent les suppressions
''' que 2026-08-24-retrait-suppression-client.sql rend table par table.
''' </summary>
<TestClass()> Public Class TestRestrictionsColonnes
    Inherits TestIntegration

    ''' <summary>Exécute l'instruction et renvoie l'erreur SQL levée, ou Nothing si elle passe.</summary>
    Private Shared Function ErreurSql(sousCompte As Compte, sql As String, ParamArray valeurs() As Object) As SqlException
        Try
            ExecuterSous(sousCompte, sql, valeurs)
        Catch ex As Exception
            Dim courante As Exception = ex
            Do While courante IsNot Nothing
                If TypeOf courante Is SqlException Then Return DirectCast(courante, SqlException)
                courante = courante.InnerException
            Loop
            Throw
        End Try
        Return Nothing
    End Function

    Private Shared Sub VerifierRefus(sousCompte As Compte, sql As String, ParamArray valeurs() As Object)
        Dim erreur = ErreurSql(sousCompte, sql, valeurs)
        Assert.IsNotNull(erreur, "l'opération devait être refusée : " & sql)
        Assert.IsTrue(erreur.Number = 229 OrElse erreur.Number = 230,
                      "refus de permission attendu, erreur " & erreur.Number & " : " & erreur.Message)
    End Sub

    ' ---------------------------------------------------------------------
    ' oa_utilisateur.cle_privee : ni lecture ni écriture pour le client
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub LectureClePrivee_SousClient_EstRefusee()
        Dim id = CreerUtilisateur()
        VerifierRefus(Compte.Client, "SELECT cle_privee FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", id)
    End Sub

    <TestMethod()> Public Sub LectureClePrivee_SousWeb_RenvoieLaCle()
        Dim id = CreerUtilisateur()
        Dim lue = ChaineLue(ScalaireSous(Compte.Web, "SELECT cle_privee FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", id))
        Assert.AreEqual(ClePriveeUtilisateur(id), lue)
        StringAssert.StartsWith(lue, "0x")
    End Sub

    <TestMethod()> Public Sub EcritureClePrivee_SousClient_EstRefuseeEtLaCleResteInchangee()
        Dim id = CreerUtilisateur()
        Dim avant = ClePriveeUtilisateur(id)
        VerifierRefus(Compte.Client, "UPDATE oasis.oa_utilisateur SET cle_privee = @p0 WHERE oa_utilisateur_id = @p1", "0xdeadbeef", id)
        Assert.AreEqual(avant, ClePriveeUtilisateur(id))
    End Sub

    <TestMethod()> Public Sub EcritureClePrivee_SousWeb_EstEnregistree()
        Dim id = CreerUtilisateur()
        Assert.AreEqual(1, ExecuterSous(Compte.Web, "UPDATE oasis.oa_utilisateur SET cle_privee = @p0 WHERE oa_utilisateur_id = @p1", "0xdeadbeef", id))
        Assert.AreEqual("0xdeadbeef", ClePriveeUtilisateur(id))
    End Sub

    ' ---------------------------------------------------------------------
    ' oa_utilisateur.cle_publique : lisible par tous, écriture refusée au client
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub LectureClePublique_SousClient_EstPermise()
        ' C'est la colonne qu'interroge UserDao.ACleSignature depuis le poste.
        Dim id = CreerUtilisateur()
        Dim lue = ChaineLue(ScalaireSous(Compte.Client, "SELECT cle_publique FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", id))
        Assert.AreEqual(AdresseUtilisateur(id), lue)
        StringAssert.StartsWith(lue, "0x")
    End Sub

    <TestMethod()> Public Sub EcritureClePublique_SousClient_EstRefuseeEtLAdresseResteInchangee()
        Dim id = CreerUtilisateur()
        Dim avant = AdresseUtilisateur(id)
        VerifierRefus(Compte.Client, "UPDATE oasis.oa_utilisateur SET cle_publique = @p0 WHERE oa_utilisateur_id = @p1",
                      "0x0000000000000000000000000000000000000001", id)
        Assert.AreEqual(avant, AdresseUtilisateur(id))
    End Sub

    <TestMethod()> Public Sub EcritureClePublique_SousWeb_EstEnregistree()
        Dim id = CreerUtilisateur()
        Assert.AreEqual(1, ExecuterSous(Compte.Web, "UPDATE oasis.oa_utilisateur SET cle_publique = @p0 WHERE oa_utilisateur_id = @p1",
                                        "0x0000000000000000000000000000000000000001", id))
        Assert.AreEqual("0x0000000000000000000000000000000000000001", AdresseUtilisateur(id))
    End Sub

    ' ---------------------------------------------------------------------
    ' oa_utilisateur.oa_password : ni lecture ni écriture pour le client
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub LectureMotDePasse_SousClient_EstRefusee()
        Dim id = CreerUtilisateur()
        VerifierRefus(Compte.Client, "SELECT oa_password FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", id)
    End Sub

    <TestMethod()> Public Sub LectureMotDePasse_SousWeb_RenvoieLEmpreinte()
        Dim id = CreerUtilisateur()
        Dim lue = ChaineLue(ScalaireSous(Compte.Web, "SELECT oa_password FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", id))
        Assert.AreEqual(EmpreinteUtilisateur(id), lue)
        Assert.IsTrue(MotDePasse.Verifier(MotDePasseParDefaut, lue))
    End Sub

    <TestMethod()> Public Sub EcritureMotDePasse_SousClient_EstRefuseeEtLEmpreinteResteInchangee()
        Dim id = CreerUtilisateur()
        Dim avant = EmpreinteUtilisateur(id)
        VerifierRefus(Compte.Client, "UPDATE oasis.oa_utilisateur SET oa_password = @p0 WHERE oa_utilisateur_id = @p1",
                      MotDePasse.Hacher("Choisi!2026"), id)
        Assert.AreEqual(avant, EmpreinteUtilisateur(id))
    End Sub

    <TestMethod()> Public Sub EcritureMotDePasse_SousWeb_EstEnregistree()
        Dim id = CreerUtilisateur()
        Dim empreinte = MotDePasse.Hacher("Choisi!2026")
        Assert.AreEqual(1, ExecuterSous(Compte.Web, "UPDATE oasis.oa_utilisateur SET oa_password = @p0 WHERE oa_utilisateur_id = @p1", empreinte, id))
        Assert.AreEqual(empreinte, EmpreinteUtilisateur(id))
    End Sub

    ' ---------------------------------------------------------------------
    ' oa_utilisateur : le reste de la ligne, SELECT * et DELETE
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub ColonnesVoisines_SousClient_RestentLisibles()
        ' Les colonnes que lit UserDao côté client, jointure du profil comprise.
        Dim identifiant = NouveauLogin()
        Dim id = CreerUtilisateur(login:=identifiant)
        ' Le login vient en tête pour que Scalaire le renvoie ; toutes les autres
        ' colonnes figurent dans le résultat, donc dans la vérification des droits.
        Dim lu = ScalaireSous(Compte.Client,
            "SELECT u.oa_utilisateur_login, u.oa_utilisateur_id, u.oa_utilisateur_nom, u.oa_utilisateur_prenom," &
            " u.oa_utilisateur_telephone, u.oa_utilisateur_fax, u.oa_utilisateur_mail," &
            " u.oa_utilisateur_profil_id, u.oa_utilisateur_admin," &
            " u.oa_utilisateur_site_id, u.oa_utilisateur_unite_sanitaire_id, u.oa_utilisateur_siege_id," &
            " u.oa_utilisateur_rpps, u.oa_utilisateur_password_is_unique_usage," &
            " u.oa_utilisateur_tentatives, u.oa_utilisateur_verrou_jusqua, u.cle_publique," &
            " u.oa_utilisateur_etat, u.oa_utilisateur_date_entree, u.oa_utilisateur_date_sortie," &
            " p.oa_r_profil_fonction_id_defaut, p.oa_r_profil_niveau_acces, p.oa_r_profil_type" &
            " FROM oasis.oa_utilisateur u" &
            " LEFT JOIN oasis.oa_r_profil p ON p.oa_r_profil_id = u.oa_utilisateur_profil_id" &
            " WHERE u.oa_utilisateur_id = @p0", id)
        Assert.AreEqual(identifiant, ChaineLue(lu))
    End Sub

    <TestMethod()> Public Sub ColonnesVoisines_SousClient_RestentModifiables()
        Dim id = CreerUtilisateur()
        Assert.AreEqual(1, ExecuterSous(Compte.Client,
            "UPDATE oasis.oa_utilisateur SET oa_utilisateur_nom = @p0, oa_utilisateur_telephone = @p1," &
            " oa_utilisateur_tentatives = @p2 WHERE oa_utilisateur_id = @p3", "MODIFIE", "0607080910", 2, id))
        Assert.AreEqual("MODIFIE", ChaineLue(Scalaire("SELECT oa_utilisateur_nom FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", id)))
        Assert.AreEqual("0607080910", ChaineLue(Scalaire("SELECT oa_utilisateur_telephone FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", id)))
        Assert.AreEqual(2, CInt(Scalaire("SELECT oa_utilisateur_tentatives FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", id)))
    End Sub

    <TestMethod()> Public Sub SelectEtoileUtilisateur_SousClient_EstRefuse()
        ' Un SELECT * échoue en bloc dès qu'une colonne de la table est refusée.
        Dim id = CreerUtilisateur()
        VerifierRefus(Compte.Client, "SELECT * FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", id)
    End Sub

    <TestMethod()> Public Sub SelectEtoileUtilisateur_SousWeb_EstPermis()
        Dim id = CreerUtilisateur()
        Assert.IsNotNull(ScalaireSous(Compte.Web, "SELECT * FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", id),
                         "la ligne doit revenir entière au serveur")
    End Sub

    <TestMethod()> Public Sub SuppressionUtilisateur_SousClient_EstRefuseeEtLaLigneReste()
        Dim id = CreerUtilisateur()
        VerifierRefus(Compte.Client, "DELETE FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", id)
        Assert.AreEqual(1, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", id)))
    End Sub

    <TestMethod()> Public Sub SuppressionUtilisateur_SousWeb_EstPermise()
        Dim id = CreerUtilisateur()
        Assert.AreEqual(1, ExecuterSous(Compte.Web, "DELETE FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", id))
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_utilisateur WHERE oa_utilisateur_id = @p0", id)))
    End Sub

    ' ---------------------------------------------------------------------
    ' oa_internaute : password et recovery illisibles pour le client
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub LectureMotDePasseInternaute_SousClient_EstRefusee()
        Dim id = CreerInternaute()
        VerifierRefus(Compte.Client, "SELECT password FROM oasis.oa_internaute WHERE id = @p0", id)
    End Sub

    <TestMethod()> Public Sub LectureMotDePasseInternaute_SousWeb_RenvoieLEmpreinte()
        Dim id = CreerInternaute()
        Dim lue = ChaineLue(ScalaireSous(Compte.Web, "SELECT password FROM oasis.oa_internaute WHERE id = @p0", id))
        Assert.IsTrue(MotDePasse.Verifier(MotDePasseParDefaut, lue))
    End Sub

    <TestMethod()> Public Sub LectureRecoveryInternaute_SousClient_EstRefusee()
        Dim id = CreerInternaute(recovery:="cle-a-proteger")
        VerifierRefus(Compte.Client, "SELECT recovery FROM oasis.oa_internaute WHERE id = @p0", id)
    End Sub

    <TestMethod()> Public Sub LectureRecoveryInternaute_SousWeb_RenvoieLaCle()
        Dim id = CreerInternaute(recovery:="cle-a-proteger")
        Assert.AreEqual("cle-a-proteger", ChaineLue(ScalaireSous(Compte.Web, "SELECT recovery FROM oasis.oa_internaute WHERE id = @p0", id)))
    End Sub

    <TestMethod()> Public Sub ColonnesVoisinesInternaute_SousClient_RestentLisibles()
        ' ExisteInternautePourEmail ne lit que l'adresse.
        Dim adresse = NouveauLogin() & "@exemple.fr"
        Dim id = CreerInternaute(email:=adresse)
        Assert.AreEqual(adresse, ChaineLue(ScalaireSous(Compte.Client,
            "SELECT email, id, username, code, tentatives, verrou_jusqua, recovery_expiration" &
            " FROM oasis.oa_internaute WHERE id = @p0", id)))
    End Sub

    <TestMethod()> Public Sub SelectEtoileInternaute_SousClient_EstRefuse()
        Dim id = CreerInternaute()
        VerifierRefus(Compte.Client, "SELECT * FROM oasis.oa_internaute WHERE id = @p0", id)
    End Sub

    <TestMethod()> Public Sub EcritureMotDePasseInternaute_SousClient_ResteAutorisee()
        ' Limite connue et assumée par la migration : le poste crée et réinitialise
        ' les comptes du portail, il garde donc l'écriture. Ce test la documente ;
        ' il devra changer quand ces écritures passeront derrière l'API.
        Dim id = CreerInternaute()
        Dim empreinte = MotDePasse.Hacher("Portail!2026")
        Assert.AreEqual(1, ExecuterSous(Compte.Client,
            "UPDATE oasis.oa_internaute SET password = @p0, recovery = @p1 WHERE id = @p2", empreinte, "nouvelle-cle", id))
        Assert.AreEqual(empreinte, ChaineLue(Scalaire("SELECT password FROM oasis.oa_internaute WHERE id = @p0", id)))
        Assert.AreEqual("nouvelle-cle", ChaineLue(Scalaire("SELECT recovery FROM oasis.oa_internaute WHERE id = @p0", id)))
    End Sub

    ' ---------------------------------------------------------------------
    ' oa_r_mail_parameter.smtp_params : illisible pour le client
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub LectureSmtpParams_SousClient_EstRefusee()
        Dim id = CreerParametreMail()
        VerifierRefus(Compte.Client, "SELECT smtp_params FROM oasis.oa_r_mail_parameter WHERE id = @p0", id)
    End Sub

    <TestMethod()> Public Sub LectureSmtpParams_SousWeb_RenvoieLeCompte()
        Dim id = CreerParametreMail(smtpParams:="smtp.exemple.fr;compte;secret")
        Assert.AreEqual("smtp.exemple.fr;compte;secret",
                        ChaineLue(ScalaireSous(Compte.Web, "SELECT smtp_params FROM oasis.oa_r_mail_parameter WHERE id = @p0", id)))
    End Sub

    <TestMethod()> Public Sub ColonnesVoisinesParametreMail_SousClient_RestentLisibles()
        ' Les colonnes que ParametreMailDao lit pour le poste.
        Dim id = CreerParametreMail()
        Assert.AreEqual("Objet de test", ChaineLue(ScalaireSous(Compte.Client,
            "SELECT objet, id, siege_id, type_mail_param, body, is_body_html" &
            " FROM oasis.oa_r_mail_parameter WHERE id = @p0", id)))
    End Sub

    <TestMethod()> Public Sub SelectEtoileParametreMail_SousClient_EstRefuse()
        Dim id = CreerParametreMail()
        VerifierRefus(Compte.Client, "SELECT * FROM oasis.oa_r_mail_parameter WHERE id = @p0", id)
    End Sub

    ' ---------------------------------------------------------------------
    ' Suppression : retirée au client sur tout le schéma, rendue sur vingt tables
    ' (dix le 2026-08-24, dix oubliées le 2026-09-27)
    ' ---------------------------------------------------------------------

    <DataTestMethod()>
    <DataRow("oa_chaine_episode")>
    <DataRow("oa_relation_chaine_episode")>
    <DataRow("oa_traitement")>
    <DataRow("oa_relation_vaccin_valence")>
    <DataRow("oa_valence")>
    <DataRow("oa_vaccin_cgv_date")>
    <DataRow("oa_vaccin_cgv_valence")>
    <DataRow("oa_vaccin_cgv_relation_valence_date")>
    <DataRow("oa_vaccin_program")>
    <DataRow("oa_vaccin_program_relation")>
    <DataRow("oa_patient_ordonnance_detail")>
    <DataRow("oa_episode_parametre")>
    <DataRow("oa_episode_acte_paramedical")>
    <DataRow("oa_episode_contexte")>
    <DataRow("oa_sous_episode_reponse")>
    <DataRow("oa_drc_acte_paramedical")>
    <DataRow("oa_drc_standard")>
    <DataRow("oa_drc_parametre")>
    <DataRow("oa_r_autosuivi")>
    <DataRow("oa_drc_synonyme")>
    Public Sub SuppressionSurTableAutorisee_SousClient_EstPermise(table As String)
        ' WHERE 1 = 0 : le droit est vérifié à la compilation de l'instruction,
        ' aucune ligne n'a besoin d'exister.
        Dim erreur = ErreurSql(Compte.Client, "DELETE FROM oasis." & table & " WHERE 1 = 0")
        Assert.IsNull(erreur, table & " : " & If(erreur Is Nothing, "", erreur.Message))
    End Sub

    <DataTestMethod()>
    <DataRow("oa_patient")>
    <DataRow("oa_episode")>
    <DataRow("oa_patient_ordonnance")>
    <DataRow("oa_action")>
    <DataRow("oa_internaute")>
    <DataRow("oa_r_profil")>
    Public Sub SuppressionHorsListe_SousClient_EstRefusee(table As String)
        VerifierRefus(Compte.Client, "DELETE FROM oasis." & table & " WHERE 1 = 0")
    End Sub

    <DataTestMethod()>
    <DataRow("oa_patient")>
    <DataRow("oa_patient_ordonnance")>
    <DataRow("oa_action")>
    Public Sub SuppressionHorsListe_SousWeb_EstPermise(table As String)
        Dim erreur = ErreurSql(Compte.Web, "DELETE FROM oasis." & table & " WHERE 1 = 0")
        Assert.IsNull(erreur, table & " : " & If(erreur Is Nothing, "", erreur.Message))
    End Sub

    ' ---------------------------------------------------------------------
    ' Catalogue : les refus et autorisations posés sont exactement ceux attendus
    ' ---------------------------------------------------------------------

    Private Const PermissionsClient As String =
        "SELECT COUNT(*) FROM sys.database_permissions p" &
        " LEFT JOIN sys.columns c ON c.object_id = p.major_id AND c.column_id = p.minor_id" &
        " WHERE p.grantee_principal_id = DATABASE_PRINCIPAL_ID('oasis_client')"

    <TestMethod()> Public Sub Catalogue_LesNeufRefusDuClientSontPoses()
        Dim attendus = {
            {"SELECT", "oa_utilisateur", "cle_privee"},
            {"SELECT", "oa_utilisateur", "oa_password"},
            {"UPDATE", "oa_utilisateur", "cle_privee"},
            {"UPDATE", "oa_utilisateur", "cle_publique"},
            {"UPDATE", "oa_utilisateur", "oa_password"},
            {"SELECT", "oa_internaute", "password"},
            {"SELECT", "oa_internaute", "recovery"},
            {"SELECT", "oa_r_mail_parameter", "smtp_params"},
            {"DELETE", "oa_utilisateur", ""}}

        For i = 0 To attendus.GetLength(0) - 1
            Dim nombre = CInt(Scalaire(PermissionsClient &
                " AND p.state_desc = 'DENY' AND p.permission_name = @p0" &
                " AND OBJECT_NAME(p.major_id) = @p1 AND COALESCE(c.name, '') = @p2",
                attendus(i, 0), attendus(i, 1), attendus(i, 2)))
            Assert.AreEqual(1, nombre, "DENY " & attendus(i, 0) & " ON " & attendus(i, 1) & " (" & attendus(i, 2) & ")")
        Next

        Assert.AreEqual(attendus.GetLength(0), CInt(Scalaire(PermissionsClient & " AND p.state_desc = 'DENY'")),
                        "un refus inattendu est posé sur oasis_client")
    End Sub

    <TestMethod()> Public Sub Catalogue_LaSuppressionNEstRendueQueSurLesVingtTables()
        Assert.AreEqual(0, CInt(Scalaire(PermissionsClient &
            " AND p.class_desc = 'SCHEMA' AND p.permission_name = 'DELETE' AND p.state_desc = 'GRANT'")),
            "DELETE ne doit plus être accordé sur le schéma")
        Assert.AreEqual(20, CInt(Scalaire(PermissionsClient &
            " AND p.class_desc = 'OBJECT_OR_COLUMN' AND p.permission_name = 'DELETE' AND p.state_desc = 'GRANT'")))
    End Sub

End Class
