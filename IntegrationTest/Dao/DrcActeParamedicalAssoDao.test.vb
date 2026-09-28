Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' DrcActeParamedicalAssoDao contre la base de test : actes paramédicaux associés à
''' un protocole collaboratif. RadFDrcActePMAssocieEdit crée, liste et supprime,
''' EpisodeProtocoleCollaboratifDao liste à la création d'épisode : tout tourne sous
''' oasis_client. DELETE sur oa_drc_acte_paramedical est accordé au client
''' (suppression-client-complement).
''' </summary>
<TestClass()> Public Class DrcActeParamedicalAssoDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New DrcActeParamedicalAssoDao

    Private Shared Function Protocole() As Long
        Return CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.ProtocoleCollaboratif)
    End Function

    Private Shared Function Acte() As Long
        Return CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.ActeParamedical)
    End Function

    Private Function Associer(protocoleId As Long, acteId As Long) As Long
        Assert.IsTrue(dao.CreateDrcActeParamedicalAsso(New DrcActeParamedicalAsso With {
            .ProtocleCollabaratifDrcId = protocoleId,
            .ActeParamedicalDrcId = acteId
        }))
        Return CLng(Scalaire("SELECT MAX(id) FROM oasis.oa_drc_acte_paramedical" &
                             " WHERE drc_protocole_collaboratif_id = @p0 AND drc_acte_paramedical_id = @p1", protocoleId, acteId))
    End Function

    Private Shared Function NombreAssociations(Optional id As Long = 0) As Integer
        If id = 0 Then Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_drc_acte_paramedical"))
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_drc_acte_paramedical WHERE id = @p0", id))
    End Function

    Private Shared Function ActesDe(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(r) CLng(r("drc_acte_paramedical_id"))).OrderBy(Function(a) a).ToArray()
    End Function

    ' --- Création et lecture ---------------------------------------------------------

    <TestMethod()> Public Sub CreateDrcActeParamedicalAsso_EnregistreLeCouple()
        Dim idProtocole = Protocole()
        Dim idActe = Acte()

        Dim id = Associer(idProtocole, idActe)

        Assert.IsTrue(id > 0)
        Assert.AreEqual(idProtocole, CLng(Scalaire("SELECT drc_protocole_collaboratif_id FROM oasis.oa_drc_acte_paramedical WHERE id = @p0", id)))
        Assert.AreEqual(idActe, CLng(Scalaire("SELECT drc_acte_paramedical_id FROM oasis.oa_drc_acte_paramedical WHERE id = @p0", id)))
    End Sub

    <TestMethod()> Public Sub CreateDrcActeParamedicalAsso_Doublon_EstRefuse()
        Dim idProtocole = Protocole()
        Dim idActe = Acte()
        Associer(idProtocole, idActe)
        Dim avant = NombreAssociations()

        Dim erreur = Assert.ThrowsException(Of ArgumentException)(
            Sub() dao.CreateDrcActeParamedicalAsso(New DrcActeParamedicalAsso With {
                .ProtocleCollabaratifDrcId = idProtocole, .ActeParamedicalDrcId = idActe}))

        StringAssert.Contains(erreur.Message, "Collision")
        Assert.AreEqual(avant, NombreAssociations())
    End Sub

    <TestMethod()> Public Sub GetAllActeParamedicalAssoByProtocoleCollaboratifId_RetientLesActesDuProtocole()
        Dim idProtocole = Protocole()
        Dim autreProtocole = Protocole()
        Dim premierActe = Acte()
        Dim secondActe = Acte()
        Associer(idProtocole, premierActe)
        Associer(idProtocole, secondActe)
        Associer(autreProtocole, premierActe)

        Dim table = dao.GetAllActeParamedicalAssoByProtocoleCollaboratifId(CInt(idProtocole))

        CollectionAssert.AreEqual(New Long() {premierActe, secondActe}, ActesDe(table))
        Assert.IsTrue(table.Rows.Cast(Of DataRow)().All(Function(r) CLng(r("drc_protocole_collaboratif_id")) = idProtocole))
        Assert.IsTrue(table.Columns.Contains("id"))
    End Sub

    <TestMethod()> Public Sub GetAllActeParamedicalAssoByProtocoleCollaboratifId_SansActe_TableVide()
        Assert.AreEqual(0, dao.GetAllActeParamedicalAssoByProtocoleCollaboratifId(CInt(Protocole())).Rows.Count)
    End Sub

    <TestMethod()> Public Sub GetDrcActeParamedicalAssoById_FiltreSurUneColonneAbsente()
        ' Comportement actuel : la requête filtre sur oa_ror_id (copiée d'un DAO du
        ' ROR), colonne que la table n'a pas ; toute lecture échoue (erreur 207).
        ' La méthode n'a aucun appelant.
        Dim id = Associer(Protocole(), Acte())

        Dim erreur = Assert.ThrowsException(Of SqlException)(Sub() dao.GetDrcActeParamedicalAssoById(CInt(id)))

        Assert.AreEqual(207, erreur.Number, erreur.Message)
    End Sub

    ' --- Suppression ------------------------------------------------------------------

    <TestMethod()> Public Sub SuppressionDrcActeParamedicalAsso_SousClient_SupprimeLeCouple()
        Dim idProtocole = Protocole()
        Dim supprime = Associer(idProtocole, Acte())
        Dim garde = Associer(idProtocole, Acte())

        Assert.IsTrue(dao.SuppressionDrcActeParamedicalAsso(supprime))

        Assert.AreEqual(0, NombreAssociations(supprime))
        Assert.AreEqual(1, NombreAssociations(garde))
    End Sub

    <TestMethod()> Public Sub SuppressionDrcActeParamedicalAsso_Inexistant_RenvoieVrai()
        ' Comportement actuel : le nombre de lignes supprimées n'est pas contrôlé.
        Assert.IsTrue(dao.SuppressionDrcActeParamedicalAsso(987654321L))
    End Sub

End Class
