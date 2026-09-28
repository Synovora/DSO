Imports Oasis_Common

''' <summary>
''' AnnuaireReferenceDao contre la base : l'annuaire de référence
''' (ans_annuaire_professionnel_sante_reference) garde les professionnels retenus
''' depuis l'annuaire national. RadFAnnuaireProfessionnelSelect et RadFOperatorSelect
''' y copient la fiche choisie (CreationAnnuaireReference), la fiche
''' RadFAnnuaireProfessionneldetail la relit : tout est dans le client lourd, donc
''' sous oasis_client.
''' </summary>
<TestClass()> Public Class AnnuaireReferenceDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New AnnuaireReferenceDao

    Private Shared Function ValeurReference(colonne As String, cle As Long) As Object
        Return Scalaire("SELECT " & colonne & " FROM oasis.ans_annuaire_professionnel_sante_reference WHERE Cle_entree = @p0", cle)
    End Function

    ' --- CreationAnnuaireReference ----------------------------------------------

    <TestMethod()> Public Sub CreationAnnuaireReference_SousClient_EcritLaFicheEtRenvoieSaCle()
        Dim fiche = ProfessionnelDeTest("ITREFDUPONT", identifiantNational:="810000000055", codeStructure:="S0055")

        Dim cle = dao.CreationAnnuaireReference(fiche, New Utilisateur)

        Assert.IsTrue(cle > 0, "la clé attribuée par la base doit revenir")
        Assert.AreEqual("ITREFDUPONT", CStr(ValeurReference("nom_exercice", cle)))
        Assert.AreEqual("810000000055", CStr(ValeurReference("identifiant_national_pp", cle)))
        Assert.AreEqual("S0055", CStr(ValeurReference("identifiant_technique_structure", cle)))
        Assert.AreEqual(10, CInt(ValeurReference("code_profression", cle)))
        Assert.AreEqual("Medecine generale", CStr(ValeurReference("libellé_savoir_faire", cle)))
        Assert.AreEqual("cabinet@exemple.fr", CStr(ValeurReference("adresse_email_coord_structure", cle)))
    End Sub

    <TestMethod()> Public Sub CreationAnnuaireReference_PuisLecture_RestitueChaqueChamp()
        Dim fiche = ProfessionnelDeTest("ITREFMOREAU", identifiantNational:="810000000066")

        Dim cle = dao.CreationAnnuaireReference(fiche, New Utilisateur)
        Dim relue = dao.GetAnnuaireReferenceById(CInt(cle))

        Assert.AreEqual(CInt(cle), relue.Cle_entree)
        ' Toutes les propriétés, hors la clé, doivent revenir telles qu'écrites.
        For Each propriete In GetType(AnnuaireProfessionnel).GetProperties()
            If propriete.Name = "Cle_entree" Then Continue For
            Assert.AreEqual(propriete.GetValue(fiche), propriete.GetValue(relue), propriete.Name)
        Next
    End Sub

    <TestMethod()> Public Sub CreationAnnuaireReference_DepuisLAnnuaireNational_CommeLEcranDeSelection()
        ' RadFAnnuaireProfessionnelSelect : lecture dans l'annuaire national, copie en référence.
        Dim cleNationale = CreerProfessionnelNational(ProfessionnelDeTest("ITREFCOPIE", identifiantNational:="810000000077"))
        Dim daoNational As New AnnuaireProfessionnelDao
        Dim choisi = daoNational.GetAnnuaireProfessionnelById(CInt(cleNationale))

        Dim cle = dao.CreationAnnuaireReference(choisi, New Utilisateur)

        Dim relue = dao.GetAnnuaireReferenceById(CInt(cle))
        Assert.AreEqual("ITREFCOPIE", relue.NomExercice)
        Assert.AreEqual("810000000077", relue.IdentifiantNational)
        Assert.AreEqual(choisi.RaisonSocialeSite, relue.RaisonSocialeSite)
    End Sub

    <TestMethod()> Public Sub CreationAnnuaireReference_FicheIncomplete_LeveUneExceptionGenerique()
        ' Comportement actuel : une propriété à Nothing donne un paramètre sans valeur,
        ' SQL Server refuse la commande et le DAO relance une Exception nue avec le
        ' seul message. Rien n'est écrit.
        Dim avant = CInt(Scalaire("SELECT COUNT(*) FROM oasis.ans_annuaire_professionnel_sante_reference"))
        Dim incomplete As New AnnuaireProfessionnel With {.NomExercice = "ITREFINCOMPLET"}

        Dim erreur = Assert.ThrowsException(Of Exception)(Sub() dao.CreationAnnuaireReference(incomplete, New Utilisateur))

        StringAssert.Contains(erreur.Message, "@")
        Assert.AreEqual(avant, CInt(Scalaire("SELECT COUNT(*) FROM oasis.ans_annuaire_professionnel_sante_reference")))
    End Sub

    ' --- GetAnnuaireReferenceById -----------------------------------------------

    <TestMethod()> Public Sub GetAnnuaireReferenceById_CleInconnue_LeveArgumentException()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetAnnuaireReferenceById(987654321))
        StringAssert.Contains(erreur.Message, "Professionnel de santé inexistant")
    End Sub

    ' --- ChargementEtatCivil ----------------------------------------------------

    <TestMethod()> Public Sub ChargementEtatCivil_FicheRelue_ComposeNomProfessionEtAdresse()
        Dim cle = dao.CreationAnnuaireReference(ProfessionnelDeTest("ITREFMARTIN"), New Utilisateur)
        Dim relue = dao.GetAnnuaireReferenceById(CInt(cle))

        Dim etatCivil = dao.ChargementEtatCivil(relue)

        Assert.AreEqual("Docteur Jean ITREFMARTIN", etatCivil.Nom)
        Assert.AreEqual("Medecin Medecine generale", etatCivil.Profession)
        Assert.AreEqual("CABINET DE TEST", etatCivil.RaisonSociale)
        Assert.AreEqual("Residence Les Palmiers 12 B Rue des Tests", etatCivil.Adresse1)
        Assert.AreEqual("97600 MAMOUDZOU", etatCivil.Adresse2)
    End Sub

    <TestMethod()> Public Sub ChargementEtatCivil_AdresseIncomplete_GardeLesEspacesInterieurs()
        ' Comportement actuel : seuls les espaces de début et de fin sont retirés ; un
        ' élément vide au milieu de l'adresse laisse deux espaces consécutifs.
        Dim fiche = ProfessionnelDeTest("ITREFPARTIEL")
        fiche.ComplementPointGeographiqueCoordonneeStructure = ""
        fiche.IndiceRepetitionVoieCoordonneeStructure = ""
        Dim cle = dao.CreationAnnuaireReference(fiche, New Utilisateur)

        Dim etatCivil = dao.ChargementEtatCivil(dao.GetAnnuaireReferenceById(CInt(cle)))

        Assert.AreEqual("12  Rue des Tests", etatCivil.Adresse1)
    End Sub

End Class
