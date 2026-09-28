Imports Oasis_Common

''' <summary>
''' DrcStandardDao contre la base de test. Les DRC standard par type d'activité se
''' gèrent dans RadFDrcStandardTypeActiviteListe et RadFDrcStandardTypeActiviteDetail,
''' et EpisodeProtocoleCollaboratifDao les lit à la création d'épisode : tout tourne
''' sous oasis_client. DELETE sur oa_drc_standard est accordé au client
''' (suppression-client-complement).
''' </summary>
<TestClass()> Public Class DrcStandardDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New DrcStandardDao

    Private Const StandardAbsent As Integer = 987654321

    Private Const Chronique As String = Episode.EnumTypeActiviteEpisodeCode.SUIVI_CHRONIQUE
    Private Const Aigu As String = Episode.EnumTypeActiviteEpisodeCode.PATHOLOGIE_AIGUE

    Private Shared Function Standard(typeActivite As String, drcId As Long,
                                     Optional ageMin As Integer = 18, Optional ageMax As Integer = 75) As DrcStandard
        Return New DrcStandard With {
            .TypeActivite = typeActivite,
            .DrcId = drcId,
            .CategorieOasis = Drc.EnumCategorieOasisCode.ProtocoleCollaboratif,
            .AgeMin = ageMin,
            .AgeMax = ageMax
        }
    End Function

    ''' <summary>Crée comme l'écran de liste : création puis relecture de l'id.</summary>
    Private Function Enregistrer(typeActivite As String, drcId As Long) As Long
        Dim saisi = Standard(typeActivite, drcId)
        dao.CreationDrcStandard(saisi)
        Return dao.GetDrcStandardCreated(saisi)
    End Function

    Private Shared Function IdsDe(table As DataTable) As Long()
        Return table.Rows.Cast(Of DataRow)().Select(Function(ligne) CLng(ligne("id"))).OrderBy(Function(id) id).ToArray()
    End Function

    Private Shared Function NombreStandards(Optional id As Long = 0) As Integer
        If id = 0 Then Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_drc_standard"))
        Return CInt(Scalaire("SELECT COUNT(*) FROM oasis.oa_drc_standard WHERE id = @p0", id))
    End Function

    ' --- Création ----------------------------------------------------------------------

    <TestMethod()> Public Sub CreationDrcStandard_EnregistreEtSeRelit()
        Dim idDrc = CreerDrc(categorieOasis:=Drc.EnumCategorieOasisCode.ProtocoleCollaboratif)
        Dim saisi = Standard(Chronique, idDrc, 40, 90)

        Assert.IsTrue(dao.CreationDrcStandard(saisi))
        Dim id = dao.GetDrcStandardCreated(saisi)

        Assert.IsTrue(id > 0)
        Dim relu = dao.GetDrcStandardById(CInt(id))
        Assert.AreEqual(id, relu.Id)
        Assert.AreEqual(Chronique, relu.TypeActivite)
        Assert.AreEqual(idDrc, relu.DrcId)
        Assert.AreEqual(7, relu.CategorieOasis)
        Assert.AreEqual(40, relu.AgeMin)
        Assert.AreEqual(90, relu.AgeMax)
        Assert.IsFalse(relu.Inactif)
        Assert.AreEqual(Date.MinValue, relu.DateModification, "jamais modifiée")
    End Sub

    <TestMethod()> Public Sub CreationDrcStandard_DoublonActif_EstRefuse()
        Dim idDrc = CreerDrc()
        Enregistrer(Chronique, idDrc)
        Dim avant = NombreStandards()

        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.CreationDrcStandard(Standard(Chronique, idDrc)))

        StringAssert.Contains(erreur.Message, "Collision")
        Assert.AreEqual(avant, NombreStandards())
    End Sub

    <TestMethod()> Public Sub CreationDrcStandard_MemeDrcAutreTypeActivite_EstAcceptee()
        Dim idDrc = CreerDrc()
        Dim chroniqueId = Enregistrer(Chronique, idDrc)

        Dim aiguId = Enregistrer(Aigu, idDrc)

        Assert.AreNotEqual(chroniqueId, aiguId)
        Assert.AreEqual(Aigu, dao.GetDrcStandardById(CInt(aiguId)).TypeActivite)
    End Sub

    <TestMethod()> Public Sub CreationDrcStandard_ApresAnnulation_PeutEtreRecreee()
        Dim idDrc = CreerDrc()
        Dim ancien = Enregistrer(Chronique, idDrc)
        dao.AnnulationDrcStandard(ancien, New Utilisateur)

        Dim nouveau = Enregistrer(Chronique, idDrc)

        Assert.AreNotEqual(ancien, nouveau)
        Assert.IsTrue(dao.GetDrcStandardById(CInt(ancien)).Inactif)
        Assert.IsFalse(dao.GetDrcStandardById(CInt(nouveau)).Inactif)
    End Sub

    ' --- Relecture de l'id créé --------------------------------------------------------

    <TestMethod()> Public Sub GetDrcStandardCreated_PlusDeLigneActive_EchoueEnConversion()
        ' Comportement actuel : MAX(id) renvoie toujours une ligne, NULL quand rien ne
        ' correspond ; la conversion de NULL en Long échoue.
        Dim idDrc = CreerDrc()
        Dim id = Enregistrer(Chronique, idDrc)
        dao.AnnulationDrcStandard(id, New Utilisateur)

        Assert.ThrowsException(Of InvalidCastException)(Sub() dao.GetDrcStandardCreated(Standard(Chronique, idDrc)))
    End Sub

    <TestMethod()> Public Sub GetDrcStandardCreated_ApostropheDansLeType_RetrouveLaLigne()
        Dim idDrc = CreerDrc()
        Dim saisi = Standard("SUIVI'CHRONIQUE", idDrc)
        dao.CreationDrcStandard(saisi)

        Dim id = dao.GetDrcStandardCreated(saisi)

        Assert.AreEqual(CLng(Scalaire("SELECT MAX(id) FROM oasis.oa_drc_standard WHERE drc_id = @p0", idDrc)), id)
        Assert.AreEqual("SUIVI'CHRONIQUE", dao.GetDrcStandardById(CInt(id)).TypeActivite)
    End Sub

    Private Const Injection As String = "x' OR '1'='1"

    <TestMethod()> Public Sub GetDrcStandardCreated_TentativeInjection_NeRetientQueLeTypeExact()
        ' La ligne Chronique, plus récente, est celle que l'injection ramènerait.
        Dim idDrc = CreerDrc()
        Dim litteral = Enregistrer(Injection, idDrc)
        Dim chronique = Enregistrer(Chronique, idDrc)
        Assert.IsTrue(chronique > litteral)

        Assert.AreEqual(litteral, dao.GetDrcStandardCreated(Standard(Injection, idDrc)))
        Assert.AreEqual(Injection, dao.GetDrcStandardById(CInt(litteral)).TypeActivite)
    End Sub

    <TestMethod()> Public Sub GetDrcStandardCreated_TentativeInjectionSansCorrespondance_NeRamenePasAutreLigne()
        Dim idDrc = CreerDrc()
        Dim chronique = Enregistrer(Chronique, idDrc)

        ' Rien ne correspond : l'échec de conversion de MAX(id) NULL est couvert par
        ' GetDrcStandardCreated_PlusDeLigneActive_EchoueEnConversion.
        Dim trouve As Long = 0
        Try
            trouve = dao.GetDrcStandardCreated(Standard(Injection, idDrc))
        Catch ex As InvalidCastException
        Catch ex As ArgumentException
        End Try

        Assert.AreNotEqual(chronique, trouve)
        Assert.AreEqual(0L, trouve)
    End Sub

    ' --- Lectures -------------------------------------------------------------------------

    <TestMethod()> <ExpectedException(GetType(ArgumentException))>
    Public Sub GetDrcStandardById_Inexistant_LeveUneErreur()
        dao.GetDrcStandardById(StandardAbsent)
    End Sub

    <TestMethod()> Public Sub GetAllDrcByTypeActivite_RetientLesActifsDuType()
        Dim premier = Enregistrer(Chronique, CreerDrc())
        Dim second = Enregistrer(Chronique, CreerDrc())
        Dim annule = Enregistrer(Chronique, CreerDrc())
        dao.AnnulationDrcStandard(annule, New Utilisateur)
        Enregistrer(Aigu, CreerDrc())

        Dim table = dao.GetAllDrcByTypeActivite(Chronique)

        CollectionAssert.AreEqual(New Long() {premier, second}, IdsDe(table))
        Assert.IsTrue(table.Columns.Contains("inactif"), "SELECT * : toutes les colonnes")
        Assert.IsTrue(table.Columns.Contains("date_modification"))
    End Sub

    <TestMethod()> Public Sub GetAllDrcByTypeActivite_TypeAbsent_TableVide()
        Enregistrer(Chronique, CreerDrc())

        Assert.AreEqual(0, dao.GetAllDrcByTypeActivite(Episode.EnumTypeActiviteEpisodeCode.VACCINATION).Rows.Count)
        Assert.AreEqual(0, dao.GetAllDrcByTypeActivite(Nothing).Rows.Count, "Nothing est cherché comme chaîne vide")
    End Sub

    <TestMethod()> Public Sub GetDrcStandardByTypeActivite_RetientLesActifsAvecSixColonnes()
        Dim idDrc = CreerDrc()
        Dim actif = Enregistrer(Aigu, idDrc)
        Dim annule = Enregistrer(Aigu, CreerDrc())
        dao.AnnulationDrcStandard(annule, New Utilisateur)
        Enregistrer(Chronique, CreerDrc())

        Dim table = dao.GetDrcStandardByTypeActivite(Aigu)

        CollectionAssert.AreEqual(New Long() {actif}, IdsDe(table))
        CollectionAssert.AreEqual(New String() {"id", "type_activite_episode", "drc_id", "categorie_oasis", "age_min", "age_max"},
                                  table.Columns.Cast(Of DataColumn)().Select(Function(c) c.ColumnName).ToArray())
        Assert.AreEqual(idDrc, CLng(table.Rows(0)("drc_id")))
        Assert.AreEqual(18, CInt(table.Rows(0)("age_min")))
    End Sub

    ' --- Modification, annulation, suppression --------------------------------------

    <TestMethod()> Public Sub ModificationDrcStandard_ChangeLesAgesEtDateLaModification()
        Dim idDrc = CreerDrc()
        Dim id = Enregistrer(Chronique, idDrc)
        Dim modifie = dao.GetDrcStandardById(CInt(id))
        modifie.AgeMin = 50
        modifie.AgeMax = 65
        modifie.TypeActivite = Aigu
        modifie.DrcId = 1

        Assert.IsTrue(dao.ModificationDrcStandard(modifie, New Utilisateur))

        Dim relu = dao.GetDrcStandardById(CInt(id))
        Assert.AreEqual(50, relu.AgeMin)
        Assert.AreEqual(65, relu.AgeMax)
        Assert.AreEqual(Date.Today, relu.DateModification.Date)
        Assert.AreEqual(Chronique, relu.TypeActivite, "seuls les âges sont modifiables")
        Assert.AreEqual(idDrc, relu.DrcId)
        Assert.IsFalse(relu.Inactif)
    End Sub

    <TestMethod()> Public Sub ModificationDrcStandard_Inexistant_LeveUneErreur()
        Dim fantome = Standard(Chronique, 12)
        fantome.Id = StandardAbsent

        Dim erreur = Assert.ThrowsException(Of Exception)(Sub() dao.ModificationDrcStandard(fantome, New Utilisateur))

        StringAssert.Contains(erreur.Message, "n'a pas abouti")
    End Sub

    <TestMethod()> Public Sub AnnulationDrcStandard_RendLaLigneInactive()
        Dim id = Enregistrer(Chronique, CreerDrc())

        Assert.IsTrue(dao.AnnulationDrcStandard(id, New Utilisateur))

        Dim relu = dao.GetDrcStandardById(CInt(id))
        Assert.IsTrue(relu.Inactif)
        Assert.AreEqual(Date.Today, relu.DateModification.Date)
        Assert.AreEqual(1, NombreStandards(id), "annulée, pas supprimée")
    End Sub

    <TestMethod()> Public Sub AnnulationDrcStandard_Inexistant_LeveUneErreur()
        Dim erreur = Assert.ThrowsException(Of Exception)(Sub() dao.AnnulationDrcStandard(StandardAbsent, New Utilisateur))
        StringAssert.Contains(erreur.Message, "n'a pas abouti")
    End Sub

    <TestMethod()> Public Sub SuppressionDrcStandard_SousClient_SupprimeLaLigne()
        Dim supprime = Enregistrer(Chronique, CreerDrc())
        Dim garde = Enregistrer(Chronique, CreerDrc())

        Assert.IsTrue(dao.SuppressionDrcStandard(supprime))

        Assert.AreEqual(0, NombreStandards(supprime))
        Assert.AreEqual(1, NombreStandards(garde))
    End Sub

    <TestMethod()> Public Sub SuppressionDrcStandard_Inexistant_RenvoieVrai()
        ' Comportement actuel : le nombre de lignes supprimées n'est pas contrôlé.
        Assert.IsTrue(dao.SuppressionDrcStandard(StandardAbsent))
    End Sub

End Class
