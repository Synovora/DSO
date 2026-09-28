Imports System.Data.SqlClient
Imports System.Globalization
Imports System.Threading
Imports Oasis_Common

''' <summary>
''' ParametreOasisDao contre la base. Le client lourd appelle TraitementContexte à
''' l'ouverture de la liste des patients, de la recherche patient et de l'état
''' journalier : une fois par jour, les contextes échus deviennent des antécédents,
''' et la ligne 1 d'oa_parametre_oasis garde la date du dernier passage. Tout tourne
''' sous oasis_client, dans la culture du poste (fr-FR).
'''
''' Les deux méthodes d'écriture envoient la date en texte : CreationParametre par
''' Date.ToString() (culture du poste), ModificationParametre en
''' "yyyy-MM-dd HH:mm:ss". SQL Server lit ce texte selon la langue de la session,
''' ce que les tests reproduisent par JeuxInternaute.ConversionSqlDeTexte plutôt
''' que de supposer la langue des logins.
''' </summary>
<TestClass()> Public Class ParametreOasisDaoTest
    Inherits TestIntegration

    Private ReadOnly dao As New ParametreOasisDao

    Private Const TableParametre As String = "oasis.oa_parametre_oasis"
    Private Const ColonneDate As String = "oa_parametre_oasis_date"
    Private Const FormatModification As String = "yyyy-MM-dd HH:mm:ss"
    Private Const IdDeTest As Integer = 9901

    Private cultureAvant As CultureInfo

    <TestInitialize>
    Public Sub PasserEnFrancais()
        cultureAvant = Thread.CurrentThread.CurrentCulture
        Thread.CurrentThread.CurrentCulture = New CultureInfo("fr-FR")
    End Sub

    <TestCleanup>
    Public Sub RetablirLaCulture()
        Thread.CurrentThread.CurrentCulture = cultureAvant
    End Sub

    ''' <summary>Ce que la session Client stocke pour ce texte, ou Nothing si elle ne sait pas le lire.</summary>
    Private Shared Function LectureSql(texte As String) As Object
        Try
            Return ConversionSqlDeTexte(Compte.Client, TableParametre, ColonneDate, texte)
        Catch ex As SqlException
            Return Nothing
        End Try
    End Function

    Private Shared Function ContexteEchu(utilisateurId As Long, patientId As Long, description As String) As Long
        Dim id = CreerContexteMedical(patientId, utilisateurId, description:=description)
        Executer("UPDATE oasis.oa_antecedent SET oa_antecedent_date_fin = @p0 WHERE oa_antecedent_id = @p1",
                 Date.Today.AddDays(-2), id)
        Return id
    End Function

    Private Shared Function TypeAntecedent(id As Long) As String
        Return CStr(Scalaire("SELECT oa_antecedent_type FROM oasis.oa_antecedent WHERE oa_antecedent_id = @p0", id)).Trim()
    End Function

    ' ---------------------------------------------------------------------
    ' CreationParametre
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub CreationParametre_SousClient_JourInferieurA13_EstLuSelonLaSession()
        RetirerParametreOasis(IdDeTest)
        Dim valeur = New Date(2026, 3, 5, 10, 20, 30)
        Dim attendue = LectureSql(valeur.ToString())
        Assert.IsNotNull(attendue, "« " & valeur.ToString() & " » est lisible en jj/mm comme en mm/jj")

        Assert.IsTrue(dao.CreationParametre(IdDeTest, "Parametre de test", valeur))

        Assert.AreEqual(attendue, LireParametreOasis(IdDeTest))
        Assert.AreEqual("Parametre de test",
                        CStr(Scalaire("SELECT oa_parametre_oasis_description FROM oasis.oa_parametre_oasis WHERE oa_parametre_oasis_id = @p0", IdDeTest)))
        ' Comportement actuel : la valeur stockée est celle que la session tire du
        ' texte ; en session mm/jj, le 5 mars devient le 3 mai. La date n'est juste
        ' que si la langue du login lit jj/mm.
    End Sub

    <TestMethod()> Public Sub CreationParametre_SousClient_JourSuperieurA12_EchoueSiLaSessionLitMoisJour()
        RetirerParametreOasis(IdDeTest)
        Dim valeur = New Date(2026, 3, 25, 10, 20, 30)
        Dim attendue = LectureSql(valeur.ToString())

        If attendue Is Nothing Then
            ' Comportement actuel : « 25/03/2026 » n'est pas une date en mm/jj ; la
            ' création échoue et le traitement quotidien avec elle.
            Assert.ThrowsException(Of Exception)(Sub() dao.CreationParametre(IdDeTest, "Parametre de test", valeur))
            Assert.IsNull(LireParametreOasis(IdDeTest))
        Else
            dao.CreationParametre(IdDeTest, "Parametre de test", valeur)
            Assert.AreEqual(valeur, CDate(LireParametreOasis(IdDeTest)))
        End If
    End Sub

    <TestMethod()> Public Sub CreationParametre_IdDejaPris_Echoue()
        PoserParametreOasis(IdDeTest, "Existant", New Date(2026, 1, 1))
        Assert.ThrowsException(Of Exception)(Sub() dao.CreationParametre(IdDeTest, "Doublon", New Date(2026, 2, 2)))
        Assert.AreEqual(New Date(2026, 1, 1), CDate(LireParametreOasis(IdDeTest)))
    End Sub

    ' ---------------------------------------------------------------------
    ' ModificationParametre
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub ModificationParametre_SousClient_ModifieLaDateSelonLaSession()
        PoserParametreOasis(IdDeTest, "A modifier", New Date(2020, 1, 1))
        Dim valeur = New Date(2026, 3, 5, 10, 20, 30)
        Dim attendue = LectureSql(valeur.ToString(FormatModification))
        Assert.IsNotNull(attendue)

        Assert.IsTrue(dao.ModificationParametre(IdDeTest, valeur))

        Assert.AreEqual(attendue, LireParametreOasis(IdDeTest))
    End Sub

    <TestMethod()> Public Sub ModificationParametre_SousClient_JourSuperieurA12()
        PoserParametreOasis(IdDeTest, "A modifier", New Date(2020, 1, 1))
        Dim valeur = New Date(2026, 3, 25, 10, 20, 30)
        Dim attendue = LectureSql(valeur.ToString(FormatModification))

        If attendue Is Nothing Then
            ' Session en jj/mm et colonne datetime : « 2026-03-25 » se lit
            ' année-jour-mois, et le mois 25 n'existe pas.
            Assert.ThrowsException(Of Exception)(Sub() dao.ModificationParametre(IdDeTest, valeur))
            Assert.AreEqual(New Date(2020, 1, 1), CDate(LireParametreOasis(IdDeTest)))
        Else
            dao.ModificationParametre(IdDeTest, valeur)
            Assert.AreEqual(valeur, CDate(LireParametreOasis(IdDeTest)))
        End If
    End Sub

    <TestMethod()> Public Sub ModificationParametre_IdInexistant_NeCreeRien()
        RetirerParametreOasis(IdDeTest)
        Dim valeur = New Date(2026, 3, 5)
        If LectureSql(valeur.ToString(FormatModification)) Is Nothing Then Assert.Inconclusive("date illisible pour cette session")

        Assert.IsTrue(dao.ModificationParametre(IdDeTest, valeur))

        Assert.IsNull(LireParametreOasis(IdDeTest))
    End Sub

    ' ---------------------------------------------------------------------
    ' TraitementContexte
    ' ---------------------------------------------------------------------

    <TestMethod()> Public Sub TraitementContexte_DejaFaitAujourdhui_NeToucheNiALaDateNiAuxContextes()
        Dim utilisateurId = CreerUtilisateur(avecCle:=False)
        Dim patientId = CreerPatient()
        Dim contexteId = ContexteEchu(utilisateurId, patientId, "Contexte echu")
        Dim ceMatin = Date.Today.AddHours(0.5)
        PoserParametreOasis(1, "Date du dernier traitement des contextes obsolètes", ceMatin)

        dao.TraitementContexte()

        Assert.AreEqual(ceMatin, CDate(LireParametreOasis(1)))
        Assert.AreEqual("C", TypeAntecedent(contexteId), "le contexte échu attend le passage du lendemain")
    End Sub

    <TestMethod()> Public Sub TraitementContexte_DernierPassageHier_MetLaDateAJour()
        PoserParametreOasis(1, "Date du dernier traitement des contextes obsolètes", Date.Today.AddDays(-1).AddHours(8))
        Dim avant = Date.Now
        Dim erreur As Exception = Nothing

        Try
            dao.TraitementContexte()
        Catch ex As Exception
            erreur = ex
        End Try

        Dim possibles = ConversionsSqlPossibles(Compte.Client, TableParametre, ColonneDate, avant, Date.Now,
                                                Function(d) d.ToString(FormatModification))
        If possibles.Count = 0 Then
            ' Comportement actuel : la session lit yyyy-MM-dd en année-jour-mois et
            ' aujourd'hui n'est pas lisible ainsi ; le traitement échoue.
            Assert.IsNotNull(erreur)
            Return
        End If
        Assert.IsNull(erreur, If(erreur Is Nothing, "", erreur.Message))
        CollectionAssert.Contains(possibles, LireParametreOasis(1))
    End Sub

    <TestMethod()> Public Sub TraitementContexte_DateNulle_MetLaDateAJour()
        PoserParametreOasis(1, "Date du dernier traitement des contextes obsolètes", Nothing)
        Dim avant = Date.Now
        Dim erreur As Exception = Nothing

        Try
            dao.TraitementContexte()
        Catch ex As Exception
            erreur = ex
        End Try

        Dim possibles = ConversionsSqlPossibles(Compte.Client, TableParametre, ColonneDate, avant, Date.Now,
                                                Function(d) d.ToString(FormatModification))
        If possibles.Count = 0 Then
            Assert.IsNotNull(erreur)
            Return
        End If
        Assert.IsNull(erreur, If(erreur Is Nothing, "", erreur.Message))
        CollectionAssert.Contains(possibles, LireParametreOasis(1))
    End Sub

    <TestMethod()> Public Sub TraitementContexte_PremierPassage_CreeLaLigneAvecLaDateDuJour()
        RetirerParametreOasis(1)
        Dim avant = Date.Now
        Dim erreur As Exception = Nothing

        Try
            dao.TraitementContexte()
        Catch ex As Exception
            erreur = ex
        End Try

        ' CreationParametre formate Date.Now dans la culture du poste (jj/mm/aaaa).
        Dim possibles = ConversionsSqlPossibles(Compte.Client, TableParametre, ColonneDate, avant, Date.Now,
                                                Function(d) d.ToString())
        If possibles.Count = 0 Then
            ' Comportement actuel : en session mm/jj, un jour supérieur à 12 rend la
            ' création impossible ; le traitement échoue tant que la ligne n'existe pas.
            Assert.IsNotNull(erreur)
            Assert.IsNull(LireParametreOasis(1))
            Return
        End If
        Assert.IsNull(erreur, If(erreur Is Nothing, "", erreur.Message))
        CollectionAssert.Contains(possibles, LireParametreOasis(1))
        Assert.AreEqual("Date du dernier traitement des contextes obsolètes",
                        CStr(Scalaire("SELECT oa_parametre_oasis_description FROM oasis.oa_parametre_oasis WHERE oa_parametre_oasis_id = 1")))
    End Sub

    <TestMethod()> Public Sub TraitementContexte_DernierPassageHier_TransformeLesContextesEchusEnAntecedents()
        ' TraitementContexte relit l'utilisateur IdUserAuto (1 par défaut, absent de
        ' app.config) : le test n'a de sens que si le premier compte créé reçoit l'id 1.
        Dim utilisateurId = CreerUtilisateur(avecCle:=False)
        If utilisateurId <> 1 Then Assert.Inconclusive("le premier utilisateur de la base a reçu l'id " & utilisateurId & ", pas 1")
        If LectureSql(Date.Now.ToString(FormatModification)) Is Nothing Then
            Assert.Inconclusive("date du jour illisible en yyyy-MM-dd pour cette session (voir TraitementContexte_DernierPassageHier_MetLaDateAJour)")
        End If
        Dim patientId = CreerPatient()
        Dim echu = ContexteEchu(utilisateurId, patientId, "Contexte echu")
        Dim enCours = CreerContexteMedical(patientId, utilisateurId, description:="Contexte en cours")
        Dim masque = ContexteEchu(utilisateurId, patientId, "Contexte masque")
        Executer("UPDATE oasis.oa_antecedent SET oa_antecedent_statut_affichage = 'O' WHERE oa_antecedent_id = @p0", masque)
        PoserParametreOasis(1, "Date du dernier traitement des contextes obsolètes", Date.Today.AddDays(-1))

        dao.TraitementContexte()

        Assert.AreEqual("A", TypeAntecedent(echu))
        Dim description = CStr(Scalaire("SELECT oa_antecedent_description FROM oasis.oa_antecedent WHERE oa_antecedent_id = @p0", echu))
        Assert.AreEqual("Contexte echu (" & DateDebutAntecedentDeTest.ToString("MM.yyyy") & ")", description)
        Assert.AreEqual(2999, CDate(Scalaire("SELECT oa_antecedent_date_fin FROM oasis.oa_antecedent WHERE oa_antecedent_id = @p0", echu)).Year)
        Assert.AreEqual("C", TypeAntecedent(enCours), "un contexte sans fin échue reste un contexte")
        Assert.AreEqual("C", TypeAntecedent(masque), "seuls les statuts d'affichage P et C sont traités")
    End Sub

End Class
