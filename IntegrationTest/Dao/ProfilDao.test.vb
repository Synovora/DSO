Imports Oasis_Common

''' <summary>
''' ProfilDao contre la base, sous le compte du client lourd, qui charge la liste
''' des profils dans la fiche utilisateur. Les deux requêtes font un SELECT * sur
''' oa_r_profil : la table n'a aucune colonne refusée, et ces tests le vérifient.
''' </summary>
<TestClass()> Public Class TestProfilDao
    Inherits TestIntegration

    Private ReadOnly dao As New ProfilDao

    <TestMethod()> Public Sub GetProfilById_SousClient_LitLeProfil()
        Dim parDefaut = CreerFonction("IT infirmier")
        CreerProfil("IT_IDE", typeProfil:="PARAMEDICAL", niveauAcces:=2, fonctionDefautId:=parDefaut,
                    designation:="Infirmier de test")

        Dim lu = dao.getProfilById("IT_IDE")

        Assert.AreEqual("IT_IDE", lu.Id)
        Assert.AreEqual("Infirmier de test", lu.Designation)
        Assert.AreEqual("PARAMEDICAL", lu.Type)
        Assert.AreEqual(2, lu.NiveauAcces)
        Assert.AreEqual(parDefaut, lu.FonctionParDefautId)
        Assert.AreEqual("False", lu.Inactif)
    End Sub

    <TestMethod()> Public Sub GetProfilById_ProfilInactif_EstLuAvecSonEtat()
        CreerProfil("IT_FERME", inactif:=True)

        Dim lu = dao.getProfilById("IT_FERME")

        Assert.AreEqual("True", lu.Inactif, "Inactif est une chaîne qui tient lieu de booléen")
    End Sub

    <TestMethod()> Public Sub GetProfilById_ColonnesNulles_DonnentLesValeursParDefaut()
        CreerProfil("IT_NUL", typeProfil:=Nothing, fonctionDefautId:=0)

        Dim lu = dao.getProfilById("IT_NUL")

        Assert.AreEqual("", lu.Type)
        Assert.AreEqual(0L, lu.FonctionParDefautId)
    End Sub

    <TestMethod()> Public Sub GetProfilById_Inconnu_Echoue()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.getProfilById("IT_ABSENT"))
        StringAssert.Contains(erreur.Message, "non retrouv")
    End Sub

    <TestMethod()> Public Sub GetListProfil_SousClient_RenvoieActifsEtInactifs()
        ' L'écran filtre lui-même ; la requête ne le fait pas.
        CreerProfil("IT_ACTIF", designation:="Actif de test")
        CreerProfil("IT_CLOS", inactif:=True, designation:="Clos de test")

        Dim liste = dao.getListProfil()

        Dim actif = liste.SingleOrDefault(Function(p) p.Id = "IT_ACTIF")
        Dim clos = liste.SingleOrDefault(Function(p) p.Id = "IT_CLOS")
        Assert.IsNotNull(actif)
        Assert.IsNotNull(clos)
        Assert.AreEqual("Actif de test", actif.Designation)
        Assert.AreEqual("True", clos.Inactif)
    End Sub

    <TestMethod()> Public Sub GetListProfil_ChaqueProfilApparaitUneFois()
        CreerProfil("IT_UNIQUE")

        Dim liste = dao.getListProfil()

        Assert.AreEqual(1, liste.Where(Function(p) p.Id = "IT_UNIQUE").Count())
        Assert.AreEqual(CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_r_profil")), liste.Count)
    End Sub

End Class
