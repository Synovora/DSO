Imports System.Globalization
Imports System.Threading
Imports Oasis_Common

''' <summary>
''' AldDao contre la base de test. Tous ses appelants sont des écrans du client
''' lourd (RadFAntecedentDetailEdit, RadFOrdonnanceListeDetail, RadFDRCSelecteur,
''' PrtOrdonnance...) ou OrdonnanceDao.CreateNewOrdonnanceDetail, qu'ils appellent :
''' sous oasis_client.
'''
''' Les ALD viennent de Schema/29-reference-theriaque.sql (le singleton Table_ald les
''' garde en mémoire). Les antécédents ALD passent par AntecedentDao.CreationAntecedent
''' (JeuxTheriaque.CreerAntecedentAld).
'''
''' IsPatientALD décide si l'ordonnance est bizone : il retient une ALD valide dont
''' la fin est au 31/12/2999 ou date de moins de trente jours. DateFinALD, l'infobulle
''' des écrans, retient une fin postérieure à aujourd'hui moins un mois. Les dates
''' sont écrites dans le SQL au format yyyy-MM-dd par la culture courante : les tests
''' qui les exercent tournent en fr-FR, comme les postes.
''' </summary>
<TestClass()> Public Class AldDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New AldDao

    Private Const AldAbsente As Integer = 9599
    Private Shared ReadOnly FinSansLimite As New Date(2999, 12, 31)

    Private Shared Function EnFrancais(Of T)(appel As System.Func(Of T)) As T
        Dim cultureAvant = Thread.CurrentThread.CurrentCulture
        Try
            Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
            Return appel()
        Finally
            Thread.CurrentThread.CurrentCulture = cultureAvant
        End Try
    End Function

    Private Function EstEnAld(idPatient As Long) As Boolean
        Return EnFrancais(Function() dao.IsPatientALD(CInt(idPatient)))
    End Function

    Private Function Infobulle(idPatient As Long) As String
        Return EnFrancais(Function() dao.DateFinALD(CInt(idPatient)))
    End Function

    ''' <summary>Antécédent de diabète en ALD, valide, publié, avec la fin donnée.</summary>
    Private Shared Function AntecedentDiabete(idPatient As Long, idUtilisateur As Long, finAld As Date,
                                              Optional statut As String = "P",
                                              Optional valide As Boolean = True) As Long
        Dim idCim10 = CreerAldCim10Reference(AldReferenceDiabeteId, AldReferenceDiabeteCode, "E11", "Diabète de type 2")
        Return CreerAntecedentAld(idPatient, idUtilisateur, AldReferenceDiabeteId, idCim10, finAld,
                                  statutAffichage:=statut, aldValide:=valide)
    End Function

    ' --- GetAldById ---------------------------------------------------------------------

    <TestMethod()> Public Sub GetAldById_LitLaLigneDeReference()
        Dim lue = dao.GetAldById(AldReferenceDiabeteId)

        Assert.AreEqual(AldReferenceDiabeteId, lue.AldId)
        Assert.AreEqual(AldReferenceDiabeteCode, lue.AldCode)
        Assert.AreEqual(AldReferenceDiabeteDescription, lue.AldDescription)
    End Sub

    <TestMethod()> Public Sub GetAldById_DistingueLesAld()
        Dim lue = dao.GetAldById(AldReferenceCardiaqueId)

        Assert.AreEqual(AldReferenceCardiaqueCode, lue.AldCode)
        Assert.AreEqual(AldReferenceCardiaqueDescription, lue.AldDescription)
    End Sub

    <TestMethod()> Public Sub GetAldById_Inexistante_LeveArgumentException()
        Dim erreur = Assert.ThrowsException(Of ArgumentException)(Sub() dao.GetAldById(AldAbsente))
        Assert.AreEqual("ALD inexistante !", erreur.Message)
    End Sub

    ' --- IsPatientALD -------------------------------------------------------------------

    <TestMethod()> Public Sub IsPatientALD_SansAntecedent_Faux()
        Dim idPatient = CreerPatient()
        Assert.IsFalse(EstEnAld(idPatient))
    End Sub

    <TestMethod()> Public Sub IsPatientALD_AldSansLimite_Vrai()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        AntecedentDiabete(idPatient, idUtilisateur, FinSansLimite)

        Assert.IsTrue(EstEnAld(idPatient))
    End Sub

    <TestMethod()> Public Sub IsPatientALD_FinAVenir_Vrai()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        AntecedentDiabete(idPatient, idUtilisateur, Date.Today.AddDays(90))

        Assert.IsTrue(EstEnAld(idPatient))
    End Sub

    <TestMethod()> Public Sub IsPatientALD_FinDepuisMoinsDeTrenteJours_Vrai()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        AntecedentDiabete(idPatient, idUtilisateur, Date.Today.AddDays(-29))

        Assert.IsTrue(EstEnAld(idPatient))
    End Sub

    <TestMethod()> Public Sub IsPatientALD_FinDepuisPlusDeTrenteJours_Faux()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        AntecedentDiabete(idPatient, idUtilisateur, Date.Today.AddDays(-31))

        Assert.IsFalse(EstEnAld(idPatient))
    End Sub

    <TestMethod()> Public Sub IsPatientALD_AldNonValide_Faux()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        AntecedentDiabete(idPatient, idUtilisateur, FinSansLimite, valide:=False)

        Assert.IsFalse(EstEnAld(idPatient))
    End Sub

    <TestMethod()> Public Sub IsPatientALD_AntecedentNonPublie_Faux()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        AntecedentDiabete(idPatient, idUtilisateur, FinSansLimite, statut:="C")

        Assert.IsFalse(EstEnAld(idPatient))
    End Sub

    <TestMethod()> Public Sub IsPatientALD_AntecedentAnnule_Faux()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        DesactiverAntecedentAld(AntecedentDiabete(idPatient, idUtilisateur, FinSansLimite))

        Assert.IsFalse(EstEnAld(idPatient))
    End Sub

    <TestMethod()> Public Sub IsPatientALD_ContexteEnAld_Faux()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        ChangerTypeAntecedentAld(AntecedentDiabete(idPatient, idUtilisateur, FinSansLimite), "C")

        Assert.IsFalse(EstEnAld(idPatient), "seuls les antécédents de type A comptent")
    End Sub

    <TestMethod()> Public Sub IsPatientALD_AldDUnAutrePatient_Faux()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim idAutre = CreerPatient("AUTRE", "Patient")
        AntecedentDiabete(idAutre, idUtilisateur, FinSansLimite)

        Assert.IsFalse(EstEnAld(idPatient))
        Assert.IsTrue(EstEnAld(idAutre))
    End Sub

    ' --- DateFinALD ---------------------------------------------------------------------

    <TestMethod()> Public Sub DateFinALD_SansAld_ChaineVide()
        Dim idPatient = CreerPatient()
        Assert.AreEqual("", Infobulle(idPatient))
    End Sub

    <TestMethod()> Public Sub DateFinALD_AldSansLimite_AfficheLaDate()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        AntecedentDiabete(idPatient, idUtilisateur, FinSansLimite)

        Assert.AreEqual("Expiration ALD : " & vbCrLf & "   31-12-2999", Infobulle(idPatient))
    End Sub

    <TestMethod()> Public Sub DateFinALD_ExpireeDepuisMoinsDUnMois_Affichee()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim fin = Date.Today.AddDays(-20)
        AntecedentDiabete(idPatient, idUtilisateur, fin)

        Assert.AreEqual("Expiration ALD : " & vbCrLf & "   " & fin.ToString("dd-MM-yyyy"), Infobulle(idPatient))
    End Sub

    <TestMethod()> Public Sub DateFinALD_ExpireeDepuisPlusDUnMois_Ignoree()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        AntecedentDiabete(idPatient, idUtilisateur, Date.Today.AddDays(-40))

        Assert.AreEqual("", Infobulle(idPatient))
    End Sub

    <TestMethod()> Public Sub DateFinALD_DeuxAld_UneLigneParDate()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim fin = Date.Today.AddDays(120)
        AntecedentDiabete(idPatient, idUtilisateur, FinSansLimite)
        Dim idCim10 = CreerAldCim10Reference(AldReferenceCardiaqueId, AldReferenceCardiaqueCode, "I50", "Insuffisance cardiaque")
        CreerAntecedentAld(idPatient, idUtilisateur, AldReferenceCardiaqueId, idCim10, fin)

        Dim lignes = Infobulle(idPatient).Split(New String() {vbCrLf}, StringSplitOptions.None)

        ' Pas d'ORDER BY : l'ordre des deux dates n'est pas garanti.
        Assert.AreEqual(3, lignes.Length)
        Assert.AreEqual("Expiration ALD : ", lignes(0))
        CollectionAssert.AreEquivalent(New String() {"   31-12-2999", "   " & fin.ToString("dd-MM-yyyy")}, lignes.Skip(1).ToArray())
    End Sub

    <TestMethod()> Public Sub DateFinALD_AldNonValideAnnuleeNonPublieeOuContexte_Ignorees()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        AntecedentDiabete(idPatient, idUtilisateur, FinSansLimite, valide:=False)
        AntecedentDiabete(idPatient, idUtilisateur, FinSansLimite, statut:="C")
        DesactiverAntecedentAld(AntecedentDiabete(idPatient, idUtilisateur, FinSansLimite))
        ChangerTypeAntecedentAld(AntecedentDiabete(idPatient, idUtilisateur, FinSansLimite), "C")
        Dim idAutre = CreerPatient("AUTRE", "Patient")
        AntecedentDiabete(idAutre, idUtilisateur, FinSansLimite)

        Assert.AreEqual("", Infobulle(idPatient))
    End Sub

    <TestMethod()> Public Sub DateFinALD_AldValideSansDateDeFin_LeveInvalidCastException()
        If Not ColonneNullableTheriaque("oasis.oa_antecedent", "oa_antecedent_ald_date_fin") Then
            Assert.Inconclusive("oa_antecedent.oa_antecedent_ald_date_fin est NOT NULL dans ce schéma.")
        End If
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        EffacerDateFinAld(AntecedentDiabete(idPatient, idUtilisateur, FinSansLimite))

        ' Comportement actuel : la date NULL est convertie en Date sans contrôle,
        ' l'infobulle fait échouer l'ouverture de l'écran.
        Assert.ThrowsException(Of InvalidCastException)(Sub() Infobulle(idPatient))
    End Sub

End Class
