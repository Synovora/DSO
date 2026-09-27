Imports Oasis_Common

''' <summary>
''' FonctionDao contre la base, sous le compte du client lourd (écrans d'épisode et
''' de demande d'avis, chargement des fonctions d'un utilisateur). Les assertions
''' portent sur les fonctions créées par le test : la base de référence peut en
''' contenir d'autres.
''' </summary>
<TestClass()> Public Class TestFonctionDao
    Inherits TestIntegration

    Private ReadOnly dao As New FonctionDao

    Private Shared Function IdsDe(liste As List(Of Fonction)) As List(Of Long)
        Return liste.Select(Function(f) f.Id).ToList()
    End Function

    ' ---------------------------------------------------------------------
    ' GetList
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetList_SansInactives_ExclutLesInactives()
        Dim active = CreerFonction("IT active")
        Dim inactive = CreerFonction("IT inactive", inactif:=True)

        Dim ids = IdsDe(dao.GetList(False))

        Assert.IsTrue(ids.Contains(active))
        Assert.IsFalse(ids.Contains(inactive))
    End Sub

    <TestMethod()> Public Sub GetList_AvecInactives_LesInclut()
        Dim active = CreerFonction("IT active")
        Dim inactive = CreerFonction("IT inactive", inactif:=True)

        Dim ids = IdsDe(dao.GetList(True))

        Assert.IsTrue(ids.Contains(active))
        Assert.IsTrue(ids.Contains(inactive))
    End Sub

    <TestMethod()> Public Sub GetList_ParProfil_NeRenvoieQueLesFonctionsActivesAssociees()
        CreerProfil("IT_PROFIL")
        Dim associee = CreerFonction("IT associee")
        Dim associeeInactive = CreerFonction("IT associee inactive", inactif:=True)
        Dim etrangere = CreerFonction("IT etrangere")
        AssocierFonction("IT_PROFIL", associee)
        AssocierFonction("IT_PROFIL", associeeInactive)

        Dim ids = IdsDe(dao.GetList(False, "IT_PROFIL"))

        CollectionAssert.AreEqual(New List(Of Long) From {associee}, ids)
        Assert.IsFalse(ids.Contains(etrangere))
    End Sub

    <TestMethod()> Public Sub GetList_ParProfilAvecInactives_InclutLesAssocieesInactives()
        ' Autre branche de la requête : sans filtre d'activité, le filtre de profil
        ' ouvre lui-même la clause WHERE.
        CreerProfil("IT_PROFIL")
        Dim associee = CreerFonction("IT associee")
        Dim associeeInactive = CreerFonction("IT associee inactive", inactif:=True)
        Dim etrangere = CreerFonction("IT etrangere")
        AssocierFonction("IT_PROFIL", associee)
        AssocierFonction("IT_PROFIL", associeeInactive)

        Dim ids = IdsDe(dao.GetList(True, "IT_PROFIL"))

        Assert.AreEqual(2, ids.Count)
        Assert.IsTrue(ids.Contains(associee))
        Assert.IsTrue(ids.Contains(associeeInactive))
        Assert.IsFalse(ids.Contains(etrangere))
    End Sub

    <TestMethod()> Public Sub GetList_ProfilSansFonction_DonneUneListeVide()
        CreerProfil("IT_VIDE")
        CreerFonction("IT non rattachee")

        Assert.AreEqual(0, dao.GetList(True, "IT_VIDE").Count)
    End Sub

    <TestMethod()> Public Sub GetList_EstTrieeParLibelle()
        CreerProfil("IT_TRI")
        Dim derniere = CreerFonction("IT zz derniere")
        Dim premiere = CreerFonction("IT aa premiere")
        Dim milieu = CreerFonction("IT mm milieu")
        AssocierFonction("IT_TRI", derniere)
        AssocierFonction("IT_TRI", premiere)
        AssocierFonction("IT_TRI", milieu)

        Dim ids = IdsDe(dao.GetList(False, "IT_TRI"))

        CollectionAssert.AreEqual(New List(Of Long) From {premiere, milieu, derniere}, ids)
    End Sub

    ' ---------------------------------------------------------------------
    ' GetFonctionById
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetFonctionById_SousClient_LitLaFonction()
        Dim id = CreerFonction("IT sage-femme", typeFonction:="MEDICAL", rorId:=987654321L, designation:="Sage-femme de test")

        Dim lue = dao.GetFonctionById(id)

        Assert.AreEqual(id, lue.Id)
        Assert.AreEqual("IT sage-femme", lue.Libelle)
        Assert.AreEqual("Sage-femme de test", lue.Designation)
        Assert.AreEqual("MEDICAL", lue.Type)
        Assert.AreEqual(987654321L, lue.RorId)
        Assert.IsFalse(lue.IsInactif)
    End Sub

    <TestMethod()> Public Sub GetFonctionById_FonctionInactive_EstLueAvecSonEtat()
        Dim id = CreerFonction("IT retiree", inactif:=True)
        Assert.IsTrue(dao.GetFonctionById(id).IsInactif)
    End Sub

    <TestMethod()> Public Sub GetFonctionById_ColonnesNulles_DonnentLesValeursParDefaut()
        Dim id = CreerFonction("IT incomplete", typeFonction:=Nothing, rorId:=0)

        Dim lue = dao.GetFonctionById(id)

        Assert.AreEqual("", lue.Type)
        Assert.AreEqual(0L, lue.RorId)
    End Sub

    <TestMethod()> Public Sub GetFonctionById_Inconnue_Echoue()
        ' L'exception d'origine est réemballée dans une Exception simple.
        Dim erreur = Assert.ThrowsException(Of Exception)(Sub() dao.GetFonctionById(-1))
        StringAssert.Contains(erreur.Message, "non retrouvée")
    End Sub

    ' ---------------------------------------------------------------------
    ' GetFonctionByRorId
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetFonctionByRorId_SousClient_LitLaFonction()
        Dim id = CreerFonction("IT ror", rorId:=987654322L)

        Dim lue = dao.GetFonctionByRorId(987654322L)

        Assert.AreEqual(id, lue.Id)
        Assert.AreEqual("IT ror", lue.Libelle)
        Assert.AreEqual(987654322L, lue.RorId)
    End Sub

    <TestMethod()> Public Sub GetFonctionByRorId_Inconnu_Echoue()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetFonctionByRorId(987654329L))
        StringAssert.Contains(erreur.Message, "non retrouvée")
    End Sub

End Class
