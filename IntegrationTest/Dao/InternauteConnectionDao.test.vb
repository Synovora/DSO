Imports Oasis_Common

''' <summary>
''' InternauteConnectionDao contre la base : journal des connexions au portail.
''' Écrit à chaque connexion réussie (AuthController.Login) et relu par le tableau
''' de bord (DashboardController) : tout tourne sous oasis_web.
''' </summary>
<TestClass()> Public Class InternauteConnectionDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New InternauteConnectionDao

    Private Function Tracer(internauteId As Long, quand As Date, Optional ip As String = "192.0.2.10") As Long
        Return dao.Create(New InternauteConnection With {.Internaute = internauteId, .Datetime = quand, .Ip = ip})
    End Function

    <TestInitialize>
    Public Sub PasserSousWeb()
        UtiliserCompte(Compte.Web)
    End Sub

    <TestMethod()> Public Sub Create_SousWeb_EnregistreLaConnexionEtRenvoieSonId()
        Dim internauteId = CreerInternaute()
        Dim quand = New Date(2026, 9, 1, 8, 30, 15)

        Dim id = Tracer(internauteId, quand, "198.51.100.7")

        Assert.IsTrue(id > 0)
        Dim filtre = " FROM oasis.oa_internaute_connection WHERE id = @p0"
        Assert.AreEqual(internauteId, CLng(Scalaire("SELECT internaute" & filtre, id)))
        Assert.AreEqual(quand, CDate(Scalaire("SELECT datetime" & filtre, id)))
        Assert.AreEqual("198.51.100.7", CStr(Scalaire("SELECT ip" & filtre, id)))
    End Sub

    <TestMethod()> Public Sub Create_SansAdresseIp_Echoue()
        ' Comportement actuel : AddWithValue avec Nothing n'envoie pas le paramètre,
        ' SQL Server le réclame et l'erreur remonte en Exception. AuthController
        ' fournit toujours une adresse (AdresseAppelant).
        Dim internauteId = CreerInternaute()
        Assert.ThrowsException(Of Exception)(Sub() Tracer(internauteId, Date.Now, Nothing))
        Assert.AreEqual(0, CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_internaute_connection WHERE internaute = @p0", internauteId)))
    End Sub

    <TestMethod()> Public Sub GetConnectionByInternaute_RenvoieLesSixDernieresDeLaPlusRecenteALaPlusAncienne()
        Dim internauteId = CreerInternaute()
        Dim autre = CreerInternaute()
        Dim ids As New List(Of Long)
        For rang = 1 To 8
            ids.Add(Tracer(internauteId, New Date(2026, 9, rang, 9, 0, 0), "192.0.2." & rang))
            Tracer(autre, New Date(2026, 9, rang, 10, 0, 0))
        Next

        Dim connexions = dao.GetConnectionByInternaute(CInt(internauteId))

        ' ORDER BY id DESC, TOP 6 : les six dernières créées, la plus récente d'abord.
        Dim attendus = Enumerable.Reverse(ids).Take(6).ToArray()
        CollectionAssert.AreEqual(attendus, connexions.Select(Function(c) c.Id).ToArray())
        Assert.IsTrue(connexions.All(Function(c) c.Internaute = internauteId))
        Assert.AreEqual(New Date(2026, 9, 8, 9, 0, 0), connexions(0).Datetime)
        Assert.AreEqual("192.0.2.8", connexions(0).Ip)
    End Sub

    <TestMethod()> Public Sub GetConnectionByInternaute_TrieParIdEtNonParDate()
        ' Une connexion enregistrée après une autre sort en tête même si sa date est antérieure.
        Dim internauteId = CreerInternaute()
        Dim ancienne = Tracer(internauteId, New Date(2026, 9, 20, 9, 0, 0))
        Dim recente = Tracer(internauteId, New Date(2026, 9, 1, 9, 0, 0))

        Dim connexions = dao.GetConnectionByInternaute(CInt(internauteId))

        CollectionAssert.AreEqual(New Long() {recente, ancienne}, connexions.Select(Function(c) c.Id).ToArray())
    End Sub

    <TestMethod()> Public Sub GetConnectionByInternaute_SansConnexion_ListeVide()
        Dim internauteId = CreerInternaute()
        Assert.AreEqual(0, dao.GetConnectionByInternaute(CInt(internauteId)).Count)
    End Sub

End Class
