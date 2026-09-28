Imports Oasis_Common

''' <summary>
''' AnnuaireProfessionnelBalDao contre la base : boîtes aux lettres de messagerie
''' sécurisée de l'annuaire importé. La fiche RadFAnnuaireProfessionneldetail du
''' client lourd liste les boîtes personnelles d'un professionnel, sous
''' oasis_client. ExisteAdresse sert au contrôle des destinataires de
''' /api/sendMail (DestinatairesMail.EstAutorise) : sous oasis_web.
''' </summary>
<TestClass()> Public Class AnnuaireProfessionnelBalDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New AnnuaireProfessionnelBalDao

    Private Const Identifiant As String = "810000000101"

    Private Shared Function Adresses(table As DataTable) As List(Of String)
        Dim liste As New List(Of String)
        For Each ligne As DataRow In table.Rows
            liste.Add(CStr(ligne("adresse_bal")))
        Next
        Return liste
    End Function

    ' --- GetBalByTypeBalAndIdentifiant ------------------------------------------

    <TestMethod()> Public Sub GetBal_BoitesPersonnelles_TrieesParAdresseSansLesAutres()
        CreerBalAnnuaire(Identifiant, "z.durand@medecin.mssante.fr")
        CreerBalAnnuaire(Identifiant, "a.durand@medecin.mssante.fr", raisonSociale:="CABINET DURAND")
        CreerBalAnnuaire(Identifiant, "secretariat@cabinet.mssante.fr", typeBal:=AnnuaireProfessionnelBalDao.EnumTypeBal.ORGANISATION)
        CreerBalAnnuaire("810000000102", "autre@medecin.mssante.fr")

        Dim table = dao.GetBalByTypeBalAndIdentifiant(AnnuaireProfessionnelBalDao.EnumTypeBal.PERSONNELLE, Identifiant)

        CollectionAssert.AreEqual(New List(Of String) From {"a.durand@medecin.mssante.fr", "z.durand@medecin.mssante.fr"},
                                  Adresses(table), "ORDER BY adresse_bal")
        Assert.AreEqual("CABINET DURAND", CStr(table.Rows(0)("raison_sociale_structure")))
        Assert.AreEqual(2, table.Columns.Count, "seules l'adresse et la raison sociale sont lues")
    End Sub

    <TestMethod()> Public Sub GetBal_BoitesDOrganisation_SontSepareesDesPersonnelles()
        CreerBalAnnuaire(Identifiant, "perso@medecin.mssante.fr")
        CreerBalAnnuaire(Identifiant, "orga@cabinet.mssante.fr", typeBal:=AnnuaireProfessionnelBalDao.EnumTypeBal.ORGANISATION)

        Dim table = dao.GetBalByTypeBalAndIdentifiant(AnnuaireProfessionnelBalDao.EnumTypeBal.ORGANISATION, Identifiant)

        CollectionAssert.AreEqual(New List(Of String) From {"orga@cabinet.mssante.fr"}, Adresses(table))
    End Sub

    <TestMethod()> Public Sub GetBal_IdentifiantEntoureDEspaces_EstRetrouve()
        CreerBalAnnuaire(Identifiant, "espaces@medecin.mssante.fr")

        Dim table = dao.GetBalByTypeBalAndIdentifiant(AnnuaireProfessionnelBalDao.EnumTypeBal.PERSONNELLE, "  " & Identifiant & " ")

        CollectionAssert.AreEqual(New List(Of String) From {"espaces@medecin.mssante.fr"}, Adresses(table))
    End Sub

    <TestMethod()> Public Sub GetBal_TypeOuIdentifiantAbsent_RenvoieUneTableVide()
        CreerBalAnnuaire(Identifiant, "present@medecin.mssante.fr")

        Assert.AreEqual(0, dao.GetBalByTypeBalAndIdentifiant(Nothing, Identifiant).Rows.Count)
        Assert.AreEqual(0, dao.GetBalByTypeBalAndIdentifiant(AnnuaireProfessionnelBalDao.EnumTypeBal.PERSONNELLE, Nothing).Rows.Count)
        Assert.AreEqual(0, dao.GetBalByTypeBalAndIdentifiant(AnnuaireProfessionnelBalDao.EnumTypeBal.PERSONNELLE, "810000000999").Rows.Count)
    End Sub

    ' --- ExisteAdresse (serveur) ------------------------------------------------

    <TestMethod()> Public Sub ExisteAdresse_SousWeb_AdresseConnue_RenvoieVrai()
        UtiliserCompte(Compte.Web)
        CreerBalAnnuaire(Identifiant, "connu@medecin.mssante.fr")
        CreerBalAnnuaire(Identifiant, "orga-connue@cabinet.mssante.fr", typeBal:=AnnuaireProfessionnelBalDao.EnumTypeBal.ORGANISATION)

        Assert.IsTrue(dao.ExisteAdresse("connu@medecin.mssante.fr"))
        Assert.IsTrue(dao.ExisteAdresse("orga-connue@cabinet.mssante.fr"), "tous types de boîte confondus")
        Assert.IsTrue(dao.ExisteAdresse("  connu@medecin.mssante.fr  "), "la saisie est rognée")
    End Sub

    <TestMethod()> Public Sub ExisteAdresse_SousWeb_AdresseInconnue_RenvoieFaux()
        UtiliserCompte(Compte.Web)
        CreerBalAnnuaire(Identifiant, "connu@medecin.mssante.fr")

        Assert.IsFalse(dao.ExisteAdresse("inconnu@medecin.mssante.fr"))
        ' Pas de recherche partielle : un fragment d'adresse connue ne suffit pas.
        Assert.IsFalse(dao.ExisteAdresse("medecin.mssante.fr"))
    End Sub

    <TestMethod()> Public Sub ExisteAdresse_AdresseVide_RenvoieFauxSansInterrogerLaBase()
        UtiliserCompte(Compte.Web)
        CreerBalAnnuaire(Identifiant, "")

        Assert.IsFalse(dao.ExisteAdresse(Nothing))
        Assert.IsFalse(dao.ExisteAdresse(""))
        Assert.IsFalse(dao.ExisteAdresse("   "), "une ligne à adresse vide existe pourtant")
    End Sub

End Class
