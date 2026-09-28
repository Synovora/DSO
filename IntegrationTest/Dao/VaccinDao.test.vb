Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' VaccinDao contre la base : vaccins importés de Theriaque, programmation d'un
''' vaccin à une date du calendrier d'un patient (oa_vaccin_program_relation) et
''' administration (lot, péremption : oa_vaccin_program_admin).
'''
''' Le client lourd appelle toutes les méthodes sauf GetById et getFromValences,
''' qui n'ont aucun appelant : tout tourne sous Compte.Client. Le carnet vaccinal
''' du portail (CarnetVaccinalController) lit en plus GetListVaccinValence,
''' GetFirstVaccinProgramRelationListDatePatient,
''' GetVaccinProgramRelationListDatePatient et
''' GetVaccinProgramAdministrationByRelation, vérifiés aussi sous Compte.Web.
'''
''' GetListVaccinValence et getFromValences écrivent leurs tables en trois parties,
''' [oasis].[oasis].[oa_vaccin] : elles lisent la base nommée oasis, quelle que soit
''' celle de la chaîne de connexion. Leurs cas nominaux ne peuvent donc tourner que
''' sur une base de test nommée oasis (OASIS_IT_DATABASE=oasis) ; ailleurs ils
''' finissent Inconclusive, et un test fixe l'échec observé.
''' </summary>
<TestClass()> Public Class VaccinDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New VaccinDao

    Private Const VaccinAbsent As Integer = 987654321

    Private Shared Function BaseNommeeOasis() As Boolean
        Return String.Equals(NomBase, "oasis", StringComparison.OrdinalIgnoreCase)
    End Function

    Private Shared Sub ExigerBaseNommeeOasis()
        If Not BaseNommeeOasis() Then
            Assert.Inconclusive("La requête vise [oasis].[oasis] en dur : elle ne lit pas la base de test " &
                                NomBase & ". Relancer avec OASIS_IT_DATABASE=oasis pour l'exercer.")
        End If
    End Sub

    ''' <summary>Patient, date de calendrier et vaccin prêts à être programmés.</summary>
    Private Structure Programme
        Public PatientId As Long
        Public DateId As Long
        Public VaccinId As Long
        Public Code As Long
    End Structure

    Private Shared Function PreparerProgramme(Optional jours As Long = 60) As Programme
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim codeVaccin = NouveauCodeVaccin()
        Return New Programme With {
            .PatientId = idPatient,
            .DateId = CreerDateCgv(jours, idPatient),
            .VaccinId = CreerVaccin(codeVaccin, utilisateurId:=idAuteur),
            .Code = codeVaccin
        }
    End Function

    ' --- Vaccins ---------------------------------------------------------------------

    <TestMethod()> Public Sub Create_EstRelueAvecTousSesChamps()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim codeVaccin = NouveauCodeVaccin()

        Dim idVaccin = dao.Create(New Vaccin With {
            .Code = codeVaccin, .CodeAtc = "J07BD52", .Dci = "PRIORIX",
            .DciLongue = "PRIORIX, poudre et solvant pour solution injectable", .UtilisateurImport = idAuteur})

        Assert.IsTrue(idVaccin > 0)
        Dim lu = dao.GetById(CInt(idVaccin))
        Assert.AreEqual(CInt(idVaccin), lu.Id)
        Assert.AreEqual(codeVaccin, lu.Code)
        Assert.AreEqual("J07BD52", lu.CodeAtc)
        Assert.AreEqual("PRIORIX", lu.Dci)
        Assert.AreEqual("PRIORIX, poudre et solvant pour solution injectable", lu.DciLongue)
        Assert.AreEqual(idAuteur, lu.UtilisateurImport)
        Assert.AreEqual(Date.Today, lu.DateImport.Date, "date d'import absente du bean : l'instant présent")
    End Sub

    <TestMethod()> Public Sub Create_ConserveLaDateDImportFournie()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim jourImport As New Date(2025, 11, 20)

        Dim idVaccin = dao.Create(New Vaccin With {
            .Code = NouveauCodeVaccin(), .CodeAtc = "J07", .Dci = "d", .DciLongue = "dl",
            .DateImport = jourImport, .UtilisateurImport = idAuteur})

        Assert.AreEqual(jourImport, dao.GetById(CInt(idVaccin)).DateImport)
    End Sub

    <TestMethod()> Public Sub GetById_Inexistant_LeveArgumentException()
        Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetById(VaccinAbsent))
    End Sub

    <TestMethod()> Public Sub GetByCode_TrouveLeVaccinImporte()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim codeVaccin = NouveauCodeVaccin()
        CreerVaccin(utilisateurId:=idAuteur)
        Dim idVaccin = CreerVaccin(codeVaccin, "REVAXIS", idAuteur)

        ' RadFVaccin passe la valeur de la cellule SP_CODE_SQ_PK ; le paramètre est une chaîne.
        Dim lu = dao.GetByCode(codeVaccin.ToString())

        Assert.AreEqual(CInt(idVaccin), lu.Id)
        Assert.AreEqual("REVAXIS", lu.Dci)
    End Sub

    <TestMethod()> Public Sub GetByCode_CodeInconnu_Nothing()
        CreerVaccin()
        Assert.IsNull(dao.GetByCode(NouveauCodeVaccin().ToString()))
    End Sub

    <TestMethod()> Public Sub GetList_RenvoieTousLesVaccins()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim a = CreerVaccin(utilisateurId:=idAuteur)
        Dim b = CreerVaccin(utilisateurId:=idAuteur)

        CollectionAssert.AreEquivalent(New Integer() {CInt(a), CInt(b)}, dao.GetList().Select(Function(v) v.Id).ToList())
    End Sub

    <TestMethod()> Public Sub GetList_SansVaccin_ListeVide()
        Assert.AreEqual(0, CompterLignesVaccin("oa_vaccin", "1 = 1"), "l'instantané ne contient aucun vaccin")
        Assert.AreEqual(0, dao.GetList().Count)
    End Sub

    <TestMethod()> Public Sub GetListRelationByVaccin_FiltreParCodeDuVaccin()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim v1 = CreerValence(utilisateurId:=idAuteur)
        Dim v2 = CreerValence(utilisateurId:=idAuteur)
        Dim codeA = NouveauCodeVaccin()
        Dim codeB = NouveauCodeVaccin()
        Dim r1 = LierVaccinValence(codeA, v1)
        Dim r2 = LierVaccinValence(codeA, v2)
        LierVaccinValence(codeB, v1)

        Dim liste = dao.GetListRelationByVaccin(codeA)

        CollectionAssert.AreEquivalent(New Long() {r1, r2}, liste.Select(Function(r) r.Id).ToList())
        Assert.IsTrue(liste.All(Function(r) r.Vaccin = codeA))
        Assert.AreEqual(0, dao.GetListRelationByVaccin(NouveauCodeVaccin()).Count)
    End Sub

    ' --- Vaccins et valences (requêtes en trois parties) ---------------------------

    <TestMethod()> Public Sub GetListVaccinValence_BaseAutreQueOasis_Echoue()
        If BaseNommeeOasis() Then
            Assert.Inconclusive("La base de test s'appelle oasis : la requête en trois parties la lit normalement.")
        End If
        Dim codeVaccin = NouveauCodeVaccin()
        CreerVaccin(codeVaccin)
        LierVaccinValence(codeVaccin, CreerValence())

        ' Comportement actuel : [oasis].[oasis].[oa_vaccin] désigne une autre base
        ' que celle de la connexion. Le carnet vaccinal du portail et du client ne
        ' fonctionne que si la base de production s'appelle exactement oasis.
        Assert.ThrowsException(Of SqlException)(Sub() dao.GetListVaccinValence())
    End Sub

    <TestMethod()> Public Sub GetListVaccinValence_UneLigneParValenceRattachee()
        ExigerBaseNommeeOasis()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim v1 = CreerValence(utilisateurId:=idAuteur)
        Dim v2 = CreerValence(utilisateurId:=idAuteur)
        Dim codeA = NouveauCodeVaccin()
        Dim idA = CreerVaccin(codeA, "HEXYON", idAuteur)
        CreerVaccin(utilisateurId:=idAuteur)
        LierVaccinValence(codeA, v1)
        LierVaccinValence(codeA, v2)
        ' Relation vers un code jamais importé : la jointure part de oa_vaccin, elle l'écarte.
        LierVaccinValence(NouveauCodeVaccin(), v1)

        Dim liste = dao.GetListVaccinValence()

        Assert.AreEqual(2, liste.Count)
        ' Id est celui du vaccin (première colonne id du SELECT *), pas celui de la relation.
        Assert.IsTrue(liste.All(Function(l) l.Id = CInt(idA) AndAlso l.Code = codeA AndAlso l.Dci = "HEXYON"))
        CollectionAssert.AreEquivalent(New Long() {v1, v2}, liste.Select(Function(l) l.Valence).ToList())
    End Sub

    <TestMethod()> Public Sub GetListVaccinValence_SousWeb_LitLesMemesLignes()
        ExigerBaseNommeeOasis()
        Dim codeVaccin = NouveauCodeVaccin()
        Dim idVaccin = CreerVaccin(codeVaccin, "BOOSTRIX")
        Dim idValence = CreerValence()
        LierVaccinValence(codeVaccin, idValence)

        UtiliserCompte(Compte.Web)
        Dim liste = dao.GetListVaccinValence()

        Assert.AreEqual(1, liste.Count)
        Assert.AreEqual(CInt(idVaccin), liste(0).Id)
        Assert.AreEqual(idValence, liste(0).Valence)
        Assert.AreEqual("BOOSTRIX", liste(0).Dci)
    End Sub

    <TestMethod()> Public Sub GetListVaccinValence_SansRelation_ListeVide()
        ExigerBaseNommeeOasis()
        CreerVaccin()
        Assert.AreEqual(0, dao.GetListVaccinValence().Count)
    End Sub

    <TestMethod()> Public Sub GetFromValences_SeulementLesValencesDemandees()
        ExigerBaseNommeeOasis()
        Dim idAuteur = CreerUtilisateur(avecCle:=False)
        Dim v1 = CreerValence(utilisateurId:=idAuteur)
        Dim v2 = CreerValence(utilisateurId:=idAuteur)
        Dim v3 = CreerValence(utilisateurId:=idAuteur)
        Dim codeA = NouveauCodeVaccin()
        Dim codeB = NouveauCodeVaccin()
        Dim codeC = NouveauCodeVaccin()
        Dim idA = CreerVaccin(codeA, utilisateurId:=idAuteur)
        Dim idB = CreerVaccin(codeB, utilisateurId:=idAuteur)
        CreerVaccin(codeC, utilisateurId:=idAuteur)
        LierVaccinValence(codeA, v1)
        LierVaccinValence(codeB, v2)
        LierVaccinValence(codeC, v3)

        Dim liste = dao.getFromValences(New List(Of Long) From {v1, v2})

        CollectionAssert.AreEquivalent(New Integer() {CInt(idA), CInt(idB)}, liste.Select(Function(l) l.Id).ToList())
        CollectionAssert.AreEquivalent(New Long() {v1, v2}, liste.Select(Function(l) l.Valence).ToList())
    End Sub

    <TestMethod()> Public Sub GetFromValences_ListeVide_ErreurDeSyntaxe()
        ' Comportement actuel : la liste est concaténée telle quelle, « IN () » est
        ' refusé par SQL Server avant même la résolution des noms.
        Assert.ThrowsException(Of SqlException)(Sub() dao.getFromValences(New List(Of Long)))
    End Sub

    ' --- Programmation d'un vaccin à une date du calendrier ------------------------

    <TestMethod()> Public Sub CreateVaccinProgramRelation_EstRelueSansRealisation()
        Dim prevu = PreparerProgramme()

        Dim idProgramme = dao.CreateVaccinProgramRelation(New VaccinProgramRelation With {
            .Date = prevu.DateId, .Patient = prevu.PatientId, .Vaccin = prevu.VaccinId, .RelationVaccinValence = prevu.Code})

        Assert.IsTrue(idProgramme > 0)
        Dim liste = dao.GetVaccinProgramRelationListDatePatient(prevu.DateId, prevu.PatientId)
        Assert.AreEqual(1, liste.Count)
        Dim lu = liste(0)
        Assert.AreEqual(idProgramme, lu.Id)
        Assert.AreEqual(prevu.DateId, lu.Date)
        Assert.AreEqual(prevu.PatientId, lu.Patient)
        Assert.AreEqual(prevu.VaccinId, lu.Vaccin)
        Assert.AreEqual(prevu.Code, lu.RelationVaccinValence)
        Assert.AreEqual(Date.MinValue, lu.RealisationDate)
        Assert.AreEqual(0L, lu.RealisationOperator)
        Assert.AreEqual(0L, lu.RealisationOperatorRor)
        Assert.IsNull(lu.RealisationOperatorText)
    End Sub

    <TestMethod()> Public Sub GetVaccinProgramRelationListDatePatient_FiltreDateEtPatient()
        Dim prevu = PreparerProgramme()
        Dim autreVaccin = CreerVaccin()
        Dim autreDate = CreerDateCgv(120, prevu.PatientId)
        Dim autrePatient = CreerPatient("AUTRE", "Patient")
        Dim p1 = ProgrammerVaccin(prevu.DateId, prevu.PatientId, prevu.VaccinId, prevu.Code)
        Dim p2 = ProgrammerVaccin(prevu.DateId, prevu.PatientId, autreVaccin, prevu.Code)
        ProgrammerVaccin(autreDate, prevu.PatientId, prevu.VaccinId, prevu.Code)
        ProgrammerVaccin(prevu.DateId, autrePatient, prevu.VaccinId, prevu.Code)

        Dim liste = dao.GetVaccinProgramRelationListDatePatient(prevu.DateId, prevu.PatientId)

        CollectionAssert.AreEquivalent(New Long() {p1, p2}, liste.Select(Function(p) p.Id).ToList())
        Assert.AreEqual(0, dao.GetVaccinProgramRelationListDatePatient(prevu.DateId, 424242).Count)
    End Sub

    <TestMethod()> Public Sub GetVaccinProgramRelationListDatePatient_SousWeb()
        Dim prevu = PreparerProgramme()
        Dim idProgramme = ProgrammerVaccin(prevu.DateId, prevu.PatientId, prevu.VaccinId, prevu.Code)

        UtiliserCompte(Compte.Web)
        Dim liste = dao.GetVaccinProgramRelationListDatePatient(prevu.DateId, prevu.PatientId)

        CollectionAssert.AreEqual(New Long() {idProgramme}, liste.Select(Function(p) p.Id).ToArray())
    End Sub

    <TestMethod()> Public Sub GetFirstVaccinProgramRelationListDatePatient_UneLigneDeLaDateOuNothing()
        Dim prevu = PreparerProgramme()
        Dim autreVaccin = CreerVaccin()
        Dim p1 = ProgrammerVaccin(prevu.DateId, prevu.PatientId, prevu.VaccinId, prevu.Code)
        Dim p2 = ProgrammerVaccin(prevu.DateId, prevu.PatientId, autreVaccin, prevu.Code)
        Dim dateVide = CreerDateCgv(180, prevu.PatientId)

        ' TOP 1 sans ORDER BY : n'importe laquelle des lignes de la date.
        Dim premiere = dao.GetFirstVaccinProgramRelationListDatePatient(prevu.DateId, prevu.PatientId)

        Assert.IsTrue(premiere.Id = p1 OrElse premiere.Id = p2)
        Assert.AreEqual(prevu.PatientId, premiere.Patient)
        Assert.IsNull(dao.GetFirstVaccinProgramRelationListDatePatient(dateVide, prevu.PatientId))
        Assert.IsNull(dao.GetFirstVaccinProgramRelationListDatePatient(prevu.DateId, 424242))
    End Sub

    <TestMethod()> Public Sub GetFirstVaccinProgramRelationListDatePatient_SousWeb()
        Dim prevu = PreparerProgramme()
        Dim idProgramme = ProgrammerVaccin(prevu.DateId, prevu.PatientId, prevu.VaccinId, prevu.Code)

        UtiliserCompte(Compte.Web)

        Assert.AreEqual(idProgramme, dao.GetFirstVaccinProgramRelationListDatePatient(prevu.DateId, prevu.PatientId).Id)
    End Sub

    <TestMethod()> Public Sub UpdateVaccinProgramRelation_EnregistreLaRealisation()
        Dim prevu = PreparerProgramme()
        Dim operateur = CreerUtilisateur(avecCle:=False)
        Dim idProgramme = ProgrammerVaccin(prevu.DateId, prevu.PatientId, prevu.VaccinId, prevu.Code)
        Dim temoin = ProgrammerVaccin(prevu.DateId, prevu.PatientId, CreerVaccin(), prevu.Code)

        ' Comme RadFVaccinInput.BtnValidation : opérateur connu, ROR à 0, texte vide.
        Dim retour = dao.UpdateVaccinProgramRelation(New VaccinProgramRelation With {
            .Id = idProgramme, .Date = prevu.DateId, .Patient = prevu.PatientId, .Vaccin = prevu.VaccinId,
            .RelationVaccinValence = prevu.Code, .RealisationDate = New Date(2026, 5, 12),
            .RealisationOperator = operateur, .RealisationOperatorRor = 0, .RealisationOperatorText = ""})

        Assert.AreEqual(idProgramme, retour)
        Dim liste = dao.GetVaccinProgramRelationListDatePatient(prevu.DateId, prevu.PatientId)
        Dim lu = liste.Single(Function(p) p.Id = idProgramme)
        Assert.AreEqual(New Date(2026, 5, 12), lu.RealisationDate)
        Assert.AreEqual(operateur, lu.RealisationOperator)
        Assert.AreEqual(0L, lu.RealisationOperatorRor)
        Assert.AreEqual("", lu.RealisationOperatorText)
        ' Les zéros sont écrits tels quels, pas en NULL.
        Assert.AreEqual(0L, CLng(Scalaire("SELECT realisation_operator_ror FROM oasis.oa_vaccin_program_relation WHERE id = @p0", idProgramme)))
        Assert.AreEqual(Date.MinValue, liste.Single(Function(p) p.Id = temoin).RealisationDate, "l'autre vaccin de la date n'est pas touché")
    End Sub

    <TestMethod()> Public Sub UpdateVaccinProgramRelation_OperateurHorsOasis_TexteLibre()
        Dim prevu = PreparerProgramme()
        Dim idProgramme = ProgrammerVaccin(prevu.DateId, prevu.PatientId, prevu.VaccinId, prevu.Code)

        dao.UpdateVaccinProgramRelation(New VaccinProgramRelation With {
            .Id = idProgramme, .Date = prevu.DateId, .Patient = prevu.PatientId, .Vaccin = prevu.VaccinId,
            .RelationVaccinValence = prevu.Code, .RealisationDate = New Date(2026, 5, 12),
            .RealisationOperator = 0, .RealisationOperatorRor = 0, .RealisationOperatorText = "Dr Martin, PMI"})

        Dim lu = dao.GetFirstVaccinProgramRelationListDatePatient(prevu.DateId, prevu.PatientId)
        Assert.AreEqual(0L, lu.RealisationOperator)
        Assert.AreEqual("Dr Martin, PMI", lu.RealisationOperatorText)
    End Sub

    <TestMethod()> Public Sub DeleteVaccinProgramRelation_SupprimeParPatientDateEtVaccin()
        Dim prevu = PreparerProgramme()
        Dim autreVaccin = CreerVaccin()
        Dim autreDate = CreerDateCgv(120, prevu.PatientId)
        Dim cible = ProgrammerVaccin(prevu.DateId, prevu.PatientId, prevu.VaccinId, prevu.Code)
        Dim memeDate = ProgrammerVaccin(prevu.DateId, prevu.PatientId, autreVaccin, prevu.Code)
        Dim memeVaccin = ProgrammerVaccin(autreDate, prevu.PatientId, prevu.VaccinId, prevu.Code)

        ' RadFVaccinInfo ne renseigne pas l'id : le DAO le renvoie tel quel, donc 0.
        Dim retour = dao.DeleteVaccinProgramRelation(New VaccinProgramRelation With {
            .Date = prevu.DateId, .Patient = prevu.PatientId, .Vaccin = prevu.VaccinId, .RelationVaccinValence = prevu.Code})

        Assert.AreEqual(0L, retour)
        Assert.AreEqual(0, CompterLignesVaccin("oa_vaccin_program_relation", "id = @p0", cible))
        Assert.AreEqual(1, CompterLignesVaccin("oa_vaccin_program_relation", "id = @p0", memeDate))
        Assert.AreEqual(1, CompterLignesVaccin("oa_vaccin_program_relation", "id = @p0", memeVaccin))
        Assert.IsNotNull(New CGVDateDao().GetById(prevu.DateId), "la date du calendrier reste")
    End Sub

    ' --- Administration (lot, péremption) ------------------------------------------

    <TestMethod()> Public Sub CreateVaccinProgramAdministration_EstRelueParLaProgrammation()
        Dim prevu = PreparerProgramme()
        Dim idProgramme = ProgrammerVaccin(prevu.DateId, prevu.PatientId, prevu.VaccinId, prevu.Code)
        Dim autreProgramme = ProgrammerVaccin(prevu.DateId, prevu.PatientId, CreerVaccin(), prevu.Code)

        Dim idAdmin = dao.CreateVaccinProgramAdministration(New VaccinProgramAdmin With {
            .VaccinProgramRelation = idProgramme, .Lot = "AB1234", .Expiration = New Date(2027, 3, 1), .Comment = "Bras gauche"})

        Assert.IsTrue(idAdmin > 0)
        Dim lu = dao.GetVaccinProgramAdministrationByRelation(CInt(idProgramme))
        Assert.AreEqual(idAdmin, lu.Id)
        Assert.AreEqual(idProgramme, lu.VaccinProgramRelation)
        Assert.AreEqual("AB1234", lu.Lot)
        Assert.AreEqual(New Date(2027, 3, 1), lu.Expiration)
        Assert.AreEqual("Bras gauche", lu.Comment)
        Assert.IsNull(dao.GetVaccinProgramAdministrationByRelation(CInt(autreProgramme)), "vaccin programmé mais pas encore administré")
    End Sub

    <TestMethod()> Public Sub GetVaccinProgramAdministrationByRelation_SousWeb()
        Dim prevu = PreparerProgramme()
        Dim idProgramme = ProgrammerVaccin(prevu.DateId, prevu.PatientId, prevu.VaccinId, prevu.Code)
        Dim idAdmin = CreerAdministrationVaccin(idProgramme, "LOT-WEB")

        UtiliserCompte(Compte.Web)
        Dim lu = dao.GetVaccinProgramAdministrationByRelation(CInt(idProgramme))

        Assert.AreEqual(idAdmin, lu.Id)
        Assert.AreEqual("LOT-WEB", lu.Lot)
        Assert.AreEqual(New Date(2027, 6, 1), lu.Expiration)
        Assert.IsNull(dao.GetVaccinProgramAdministrationByRelation(VaccinAbsent))
    End Sub

    <TestMethod()> Public Sub UpdateVaccinProgramAdministration_ModifieLotPeremptionEtCommentaire()
        Dim prevu = PreparerProgramme()
        Dim idProgramme = ProgrammerVaccin(prevu.DateId, prevu.PatientId, prevu.VaccinId, prevu.Code)
        Dim idAdmin = CreerAdministrationVaccin(idProgramme, "AVANT", "c avant")

        Dim retour = dao.UpdateVaccinProgramAdministration(New VaccinProgramAdmin With {
            .Id = idAdmin, .VaccinProgramRelation = idProgramme, .Lot = "APRES",
            .Expiration = New Date(2028, 1, 1), .Comment = "c apres"})

        ' Comportement actuel : renvoie toujours 0, l'affectation de l'id est commentée.
        Assert.AreEqual(0L, retour)
        Dim lu = dao.GetVaccinProgramAdministrationByRelation(CInt(idProgramme))
        Assert.AreEqual(idAdmin, lu.Id)
        Assert.AreEqual("APRES", lu.Lot)
        Assert.AreEqual(New Date(2028, 1, 1), lu.Expiration)
        Assert.AreEqual("c apres", lu.Comment)
    End Sub

End Class
