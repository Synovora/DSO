Imports System.Data.SqlClient
Imports Oasis_Common

''' <summary>
''' TheriaqueDao contre la base de test. Toutes ses méthodes lisent la base de
''' médicaments Theriaque, installée sur la même instance que la base oasis sous le
''' nom Theriak : procédures stockées theriaque.GET_THE_* après
''' SqlConnection.ChangeDatabase("Theriak"), lecture directe des tables
''' theriaque.GSAC_PERE_SUBACT et theriaque.SAC_SUBACTIVE après le même
''' ChangeDatabase, et nom en trois parties [Theriak].[theriaque].[CATC_CLASSEATC]
''' pour GetAllATC. Ce n'est ni un serveur lié ni une table de la base oasis :
''' l'export du schéma d'oasis n'en contient rien, et ces requêtes ne peuvent pas
''' tourner ici sur des lignes préparées.
'''
''' Ce qui reste vérifiable sans Theriak : chaque méthode échoue par une
''' SqlException au lieu de rendre une réponse vide, et en particulier les deux
''' contrôles de prescription (IsSpecialiteAllergique, IsSpecialiteContreIndique)
''' ne concluent pas « ni allergie ni contre-indication » quand la base de
''' médicaments manque. Le rapprochement lui-même (substance, famille de
''' substances, préfixe ATC) est du code VB entre deux appels à Theriak ; il
''' faudrait l'extraire pour le tester.
'''
''' Tous les appelants sont des écrans du client lourd : sous oasis_client. Les
''' tests deviennent Inconclusive si une base Theriak existe sur l'instance.
''' </summary>
<TestClass()> Public Class TheriaqueDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New TheriaqueDao

    Private Shared Sub EchoueSansTheriak(appel As System.Action)
        If BaseTheriakPresente() Then Assert.Inconclusive("La base Theriak existe sur cette instance : ces tests supposent son absence.")
        Assert.ThrowsException(Of SqlException)(appel)
    End Sub

    ' --- Contrôles de prescription ---------------------------------------------------------

    <TestMethod()> Public Sub IsSpecialiteAllergique_AllergieDeclaree_SansTheriak_EchoueAuLieuDeConclureNon()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim daoAllergie As New AllergieDao
        Assert.IsTrue(daoAllergie.CreationAllergie(
            New Allergie With {.PatientId = idPatient, .SubstanceId = 111, .SubstancePereId = 0, .DenominationSubstance = "SUBSTANCE A"},
            New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}))
        Dim lePatient As New Patient With {.PatientId = CInt(idPatient)}

        EchoueSansTheriak(Sub() dao.IsSpecialiteAllergique(lePatient, 1234))
    End Sub

    <TestMethod()> Public Sub IsSpecialiteContreIndique_ContreIndicationDeclaree_SansTheriak_EchoueAuLieuDeConclureNon()
        Dim idUtilisateur = CreerUtilisateur(avecCle:=False)
        Dim idPatient = CreerPatient()
        Dim daoAtc As New ContreIndicationATCDao
        Assert.IsTrue(daoAtc.CreationContreIndicationATC(
            New ContreIndicationATC With {.PatientId = idPatient, .ATCId = "N02", .DenominationATC = "ANALGESIQUES"},
            New Utilisateur With {.UtilisateurId = CInt(idUtilisateur)}))
        Dim lePatient As New Patient With {.PatientId = CInt(idPatient)}

        EchoueSansTheriak(Sub() dao.IsSpecialiteContreIndique(lePatient, 1234))
    End Sub

    ' --- ATC ------------------------------------------------------------------------------

    <TestMethod()> Public Sub GetAllATC_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetAllATC())
    End Sub

    <TestMethod()> Public Sub GetATCListeByATCPere_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetATCListeByATCPere("N02"))
    End Sub

    <TestMethod()> Public Sub GetATCDenominationById_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetATCDenominationById("N02BE01"))
    End Sub

    <TestMethod()> Public Sub GetATCCodeListBySubstanceId_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetATCCodeListBySubstanceId("111"))
    End Sub

    ' --- Spécialités ----------------------------------------------------------------------

    <TestMethod()> Public Sub GetSpecialiteByArgument_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetSpecialiteByArgument("1234", TheriaqueDao.EnumGetSpecialite.ID_THERIAQUE,
                                                            CInt(TheriaqueDao.EnumMonoVir.NULL)))
    End Sub

    <TestMethod()> Public Sub GetSpecialiteById_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetSpecialiteById(1234))
    End Sub

    <TestMethod()> Public Sub GetSpecialiteDenominationById_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetSpecialiteDenominationById("1234"))
    End Sub

    <TestMethod()> Public Sub GetCodeAtcBySpecialiteId_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetCodeAtcBySpecialiteId("1234"))
    End Sub

    <TestMethod()> Public Sub GetPharmacoCinetiqueBySpecialite_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetPharmacoCinetiqueBySpecialite("1234"))
    End Sub

    <TestMethod()> Public Sub GetEffetIndesirableBySpecialite_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetEffetIndesirableBySpecialite("1234", TheriaqueDao.EnumTypeEffetIndesirable.CLINIQUE))
    End Sub

    <TestMethod()> Public Sub GetPharmacoDynamiqueBySpecialite_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetPharmacoDynamiqueBySpecialite("1234"))
    End Sub

    ' --- Substances -----------------------------------------------------------------------

    <TestMethod()> Public Sub GetSubstanceById_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetSubstanceById("111"))
    End Sub

    <TestMethod()> Public Sub GetSubstanceDenominationById_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetSubstanceDenominationById("111"))
    End Sub

    <TestMethod()> Public Sub GetSubstanceCodeListBySpecialite_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetSubstanceCodeListBySpecialite("1234"))
    End Sub

    <TestMethod()> Public Sub GetSubstancePereDenominationById_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetSubstancePereDenominationById(900))
    End Sub

    <TestMethod()> Public Sub GetSubstanceActiveBySubstancePereId_SansTheriak_Echoue()
        EchoueSansTheriak(Sub() dao.GetSubstanceActiveBySubstancePereId(900))
    End Sub

End Class
