Imports System.Configuration
Imports System.Data.SqlClient
Imports System.Reflection

''' <summary>
''' Accès à la base de test jetable. Construit les chaînes de connexion des trois
''' comptes à partir des variables d'environnement OASIS_IT_*, les injecte dans la
''' configuration à la place de celles que distribue /api/login, exécute du SQL brut
''' et remet la base dans l'état de l'instantané entre deux tests.
'''
''' Toutes les chaînes portent Pooling=False : la restauration d'instantané exige
''' qu'aucune session ne reste ouverte sur la base, et une connexion rendue au pool
''' en garde une.
''' </summary>
Public Module BaseDeTest

    Public Enum Compte
        ''' <summary>sa ou sécurité intégrée : préparation des données, vérifications directes.</summary>
        Admin
        ''' <summary>oasis_web : ce que voit Oasis_Web.</summary>
        Web
        ''' <summary>oasis_client : ce que voit le client lourd.</summary>
        Client
    End Enum

    ''' <summary>Nom de l'instantané que crée Assemblage et que restaure Reinitialiser.</summary>
    Public Const NomInstantane As String = "oasis_it_instantane"

    Private Const CleConnexion As String = "Oasis_WF.My.MySettings.oasisConnection"
    Private Const CleConnexionClient As String = "Oasis_WF.My.MySettings.oasisConnectionClient"

    ''' <summary>Instance SQL Server (OASIS_IT_SERVER).</summary>
    Public ReadOnly Property Serveur As String
        Get
            Return Variable("OASIS_IT_SERVER", "localhost\SQLEXPRESS")
        End Get
    End Property

    ''' <summary>Base de test (OASIS_IT_DATABASE).</summary>
    Public ReadOnly Property NomBase As String
        Get
            Return Variable("OASIS_IT_DATABASE", "oasis_it")
        End Get
    End Property

    ''' <summary>Chaîne de connexion à la base de test sous le compte donné.</summary>
    Public Function ChaineConnexion(quelCompte As Compte) As String
        Return Construire(quelCompte, NomBase, 15)
    End Function

    ''' <summary>
    ''' Chaîne Admin vers un autre catalogue (master pour la restauration) ou avec un
    ''' délai de connexion plus court (vérification de disponibilité).
    ''' </summary>
    Friend Function ChaineAdmin(catalogue As String, Optional delaiSecondes As Integer = 15) As String
        Return Construire(Compte.Admin, catalogue, delaiSecondes)
    End Function

    ''' <summary>
    ''' Fait pointer la configuration sur la base de test : oasisConnection vers le
    ''' compte donné, oasisConnectionClient vers le compte Client, comme Web.config
    ''' côté serveur. Les entrées de ConfigurationManager sont en lecture seule ; on
    ''' lève le verrou par réflexion, exactement comme StandardDao.FixConnectionString.
    ''' </summary>
    Public Sub UtiliserCompte(quelCompte As Compte)
        Remplacer(CleConnexion, ChaineConnexion(quelCompte))
        Remplacer(CleConnexionClient, ChaineConnexion(Compte.Client))
    End Sub

    ''' <summary>
    ''' SQL brut sous le compte Admin. Les valeurs deviennent les paramètres @p0, @p1...
    ''' dans l'ordre ; Nothing est envoyé comme NULL. Renvoie le nombre de lignes touchées.
    ''' </summary>
    Public Function Executer(sql As String, ParamArray valeurs() As Object) As Integer
        Return ExecuterSous(Compte.Admin, sql, valeurs)
    End Function

    ''' <summary>
    ''' Première colonne de la première ligne, sous le compte Admin. Comme
    ''' SqlCommand.ExecuteScalar : Nothing quand il n'y a aucune ligne, DBNull.Value
    ''' quand la valeur est NULL.
    ''' </summary>
    Public Function Scalaire(sql As String, ParamArray valeurs() As Object) As Object
        Return ScalaireSous(Compte.Admin, sql, valeurs)
    End Function

    ''' <summary>Comme Executer, sous un compte choisi, pour éprouver les droits.</summary>
    Public Function ExecuterSous(quelCompte As Compte, sql As String, ParamArray valeurs() As Object) As Integer
        Using connexion As New SqlConnection(ChaineConnexion(quelCompte))
            connexion.Open()
            Using commande = Preparer(connexion, sql, valeurs)
                Return commande.ExecuteNonQuery()
            End Using
        End Using
    End Function

    ''' <summary>Comme Scalaire, sous un compte choisi, pour éprouver les droits.</summary>
    Public Function ScalaireSous(quelCompte As Compte, sql As String, ParamArray valeurs() As Object) As Object
        Using connexion As New SqlConnection(ChaineConnexion(quelCompte))
            connexion.Open()
            Using commande = Preparer(connexion, sql, valeurs)
                Return commande.ExecuteScalar()
            End Using
        End Using
    End Function

    ''' <summary>
    ''' Remet la base dans l'état de l'instantané oasis_it_instantane. La base passe
    ''' en utilisateur unique le temps de la restauration, ce qui coupe toute session
    ''' restée ouverte, puis repasse en multi-utilisateur même si la restauration échoue.
    ''' </summary>
    Public Sub Reinitialiser()
        SqlConnection.ClearAllPools()
        Dim cible = Crochets(NomBase)
        Using connexion As New SqlConnection(ChaineAdmin("master"))
            connexion.Open()
            ExecuterSur(connexion, $"ALTER DATABASE {cible} SET SINGLE_USER WITH ROLLBACK IMMEDIATE;")
            Try
                ExecuterSur(connexion, $"RESTORE DATABASE {cible} FROM DATABASE_SNAPSHOT = {Litteral(NomInstantane)};")
            Finally
                ExecuterSur(connexion, $"ALTER DATABASE {cible} SET MULTI_USER;")
            End Try
        End Using
    End Sub

    ''' <summary>Identifiant T-SQL entre crochets, crochet fermant doublé.</summary>
    Friend Function Crochets(identifiant As String) As String
        Return "[" & identifiant.Replace("]", "]]") & "]"
    End Function

    ''' <summary>Chaîne T-SQL entre apostrophes, apostrophes doublées.</summary>
    Friend Function Litteral(texte As String) As String
        Return "'" & texte.Replace("'", "''") & "'"
    End Function

    ''' <summary>Exécute une instruction d'administration, avec un délai large (RESTORE, CREATE DATABASE).</summary>
    Friend Sub ExecuterSur(connexion As SqlConnection, instruction As String)
        Using commande As New SqlCommand(instruction, connexion)
            commande.CommandTimeout = 300
            commande.ExecuteNonQuery()
        End Using
    End Sub

    Private Function Construire(quelCompte As Compte, catalogue As String, delaiSecondes As Integer) As String
        Dim constructeur As New SqlConnectionStringBuilder With {
            .DataSource = Serveur,
            .InitialCatalog = catalogue,
            .Pooling = False,
            .ConnectTimeout = delaiSecondes
        }
        Select Case quelCompte
            Case Compte.Admin
                Dim motDePasse = Variable("OASIS_IT_ADMIN_PASSWORD")
                If motDePasse = "" Then
                    constructeur.IntegratedSecurity = True
                Else
                    constructeur.UserID = "sa"
                    constructeur.Password = motDePasse
                End If
            Case Compte.Web
                constructeur.UserID = "oasis_web"
                constructeur.Password = MotDePasseExige("OASIS_IT_WEB_PASSWORD", "oasis_web")
            Case Compte.Client
                constructeur.UserID = "oasis_client"
                constructeur.Password = MotDePasseExige("OASIS_IT_CLIENT_PASSWORD", "oasis_client")
            Case Else
                Throw New ArgumentOutOfRangeException(NameOf(quelCompte), quelCompte, "Compte de test inconnu.")
        End Select
        Return constructeur.ConnectionString
    End Function

    Private Function MotDePasseExige(nomVariable As String, login As String) As String
        Dim motDePasse = Variable(nomVariable)
        If motDePasse = "" Then
            Throw New InvalidOperationException(
                $"La variable d'environnement {nomVariable} n'est pas renseignée : impossible de se connecter en {login}. " &
                "Lancer IntegrationTest\Schema\Preparer-Base.ps1, qui crée les logins et écrit les variables OASIS_IT_*.")
        End If
        Return motDePasse
    End Function

    Private Function Variable(nomVariable As String, Optional defaut As String = "") As String
        Dim valeur = Environment.GetEnvironmentVariable(nomVariable)
        Return If(String.IsNullOrEmpty(valeur), defaut, valeur)
    End Function

    Private Sub Remplacer(nomChaine As String, valeur As String)
        Dim entree = ConfigurationManager.ConnectionStrings(nomChaine)
        If entree Is Nothing Then
            Throw New InvalidOperationException($"La chaîne de connexion '{nomChaine}' est absente de app.config.")
        End If
        Dim verrou = GetType(ConfigurationElement).GetField("_bReadOnly", BindingFlags.Instance Or BindingFlags.NonPublic)
        verrou.SetValue(entree, False)
        entree.ConnectionString = valeur
    End Sub

    ''' <summary>
    ''' Commande paramétrée : la valeur d'indice i devient @pi. Un appel avec un seul
    ''' Nothing en guise de valeurs arrive ici comme un tableau Nothing (règle de
    ''' ParamArray) ; il est lu comme un unique paramètre NULL.
    ''' </summary>
    Private Function Preparer(connexion As SqlConnection, sql As String, valeurs() As Object) As SqlCommand
        Dim commande As New SqlCommand(sql, connexion)
        Dim liste = If(valeurs, New Object() {Nothing})
        For i = 0 To liste.Length - 1
            commande.Parameters.AddWithValue("@p" & i, If(liste(i), DBNull.Value))
        Next
        Return commande
    End Function

End Module
