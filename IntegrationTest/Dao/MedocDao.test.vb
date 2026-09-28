Imports System.Collections.Specialized
Imports System.Reflection
Imports Oasis_Common

''' <summary>
''' MedocDao contre la base de test. Il lit la composition des médicaments
''' (oa_r_medicament_compo, reprise de la base médicamenteuse externe), remplie ici
''' par JeuxTraitement.
'''
''' Le module est déclaré sans modificateur, donc Friend : il n'est pas visible
''' depuis ce projet (Oasis_Common ne déclare InternalsVisibleTo que pour UnitTest).
''' Ses fonctions sont appelées par réflexion. Aucun appelant en production ; elles
''' tournent sous oasis_client comme le reste du client lourd.
''' </summary>
<TestClass()> Public Class MedocDaoTest
    Inherits TestIntegration

    Private Const CisAvecFraction As Integer = 62000001
    Private Const CisSansFraction As Integer = 62000002
    Private Const CisPartage As Integer = 62000003
    Private Const CisInconnu As Integer = 62000009

    Private Shared Function Appeler(nomFonction As String, ParamArray cisListe() As Integer) As List(Of String)
        Dim moduleMedoc = GetType(TraitementDao).Assembly.GetType("Oasis_Common.MedocDao", True)
        Dim fonction = moduleMedoc.GetMethod(nomFonction, BindingFlags.Public Or BindingFlags.NonPublic Or BindingFlags.Static)
        Assert.IsNotNull(fonction, nomFonction & " introuvable dans MedocDao")
        Dim entree As New StringCollection()
        For Each cis In cisListe
            entree.Add(cis.ToString())
        Next
        Dim sortie = CType(fonction.Invoke(Nothing, New Object() {entree}), StringCollection)
        Return sortie.Cast(Of String)().ToList()
    End Function

    ''' <summary>
    ''' Trois médicaments : l'un avec substances actives et fractions thérapeutiques,
    ''' l'autre avec substances actives seulement, le troisième partageant une
    ''' substance avec le premier.
    ''' </summary>
    Private Shared Sub PreparerCompositions()
        AjouterCompositionMedicament(CisAvecFraction, "SA", "AMOXICILLINE TRIHYDRATEE")
        AjouterCompositionMedicament(CisAvecFraction, "FT", "AMOXICILLINE")
        AjouterCompositionMedicament(CisAvecFraction, "SA", "CLAVULANATE DE POTASSIUM")
        AjouterCompositionMedicament(CisAvecFraction, "FT", "ACIDE CLAVULANIQUE")
        AjouterCompositionMedicament(CisSansFraction, "SA", "PARACETAMOL")
        AjouterCompositionMedicament(CisSansFraction, "SA", "CAFEINE")
        AjouterCompositionMedicament(CisPartage, "FT", "AMOXICILLINE")
    End Sub

    ' --- Substances allergiques ------------------------------------------------------

    <TestMethod()> Public Sub ListeSubstancesAllergiques_AvecFraction_NeRetientQueLesFractions()
        PreparerCompositions()

        Dim substances = Appeler("ListeSubstancesAllergiques", CisAvecFraction)

        CollectionAssert.AreEquivalent(New String() {"AMOXICILLINE", "ACIDE CLAVULANIQUE"}, substances)
    End Sub

    <TestMethod()> Public Sub ListeSubstancesAllergiques_SansFraction_RetientLesSubstancesActives()
        PreparerCompositions()

        Dim substances = Appeler("ListeSubstancesAllergiques", CisSansFraction)

        CollectionAssert.AreEquivalent(New String() {"PARACETAMOL", "CAFEINE"}, substances)
    End Sub

    <TestMethod()> Public Sub ListeSubstancesAllergiques_PlusieursMedicaments_CumuleSansDoublon()
        PreparerCompositions()

        Dim substances = Appeler("ListeSubstancesAllergiques", CisAvecFraction, CisSansFraction, CisPartage, CisInconnu)

        CollectionAssert.AreEquivalent(New String() {"AMOXICILLINE", "ACIDE CLAVULANIQUE", "PARACETAMOL", "CAFEINE"}, substances)
    End Sub

    <TestMethod()> Public Sub ListeSubstancesAllergiques_ListeVide_RienNEstRetenu()
        PreparerCompositions()
        Assert.AreEqual(0, Appeler("ListeSubstancesAllergiques").Count)
    End Sub

    ' --- Substances contre-indiquées -------------------------------------------------

    <TestMethod()> Public Sub ListeSubstancesCI_AvecFraction_NeRetientQueLesFractions()
        PreparerCompositions()

        Dim substances = Appeler("ListeSubstancesCI", CisAvecFraction)

        CollectionAssert.AreEquivalent(New String() {"AMOXICILLINE", "ACIDE CLAVULANIQUE"}, substances)
    End Sub

    <TestMethod()> Public Sub ListeSubstancesCI_PlusieursMedicaments_CumuleSansDoublon()
        ' La table de travail est libérée à chaque tour puis réutilisée : le cumul
        ' sur plusieurs médicaments doit malgré tout aboutir.
        PreparerCompositions()

        Dim substances = Appeler("ListeSubstancesCI", CisSansFraction, CisAvecFraction, CisPartage)

        CollectionAssert.AreEquivalent(New String() {"PARACETAMOL", "CAFEINE", "AMOXICILLINE", "ACIDE CLAVULANIQUE"}, substances)
    End Sub

    <TestMethod()> Public Sub ListeSubstancesCI_MedicamentInconnu_RienNEstRetenu()
        PreparerCompositions()
        Assert.AreEqual(0, Appeler("ListeSubstancesCI", CisInconnu).Count)
    End Sub

End Class
