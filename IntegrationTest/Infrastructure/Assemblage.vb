Imports System.Data.SqlClient

''' <summary>
''' Préparation commune à tout l'assemblage : vérifie que la base de test répond,
''' puis prend l'instantané oasis_it_instantane que Reinitialiser restaure avant
''' chaque test. MSTest n'exécute AssemblyInitialize que dans une classe marquée
''' TestClass, d'où l'attribut sur une classe sans test.
''' </summary>
<TestClass>
Public NotInheritable Class Assemblage

    ''' <summary>Vrai une fois l'instantané créé ; le nettoyage n'a rien à faire sinon.</summary>
    Private Shared instantanePret As Boolean

    <AssemblyInitialize>
    Public Shared Sub Initialiser(context As TestContext)
        VerifierBase()

        Using connexion As New SqlConnection(ChaineAdmin("master"))
            connexion.Open()
            Dim source = LireValeur(connexion,
                "SELECT source_database_id FROM sys.databases WHERE name = @nom",
                NomInstantane)
            If source IsNot Nothing AndAlso Not IsDBNull(source) Then
                ' Instantané laissé par une exécution interrompue. S'il vient bien de la
                ' base de test, on y revient d'abord : la base contient encore les données
                ' du dernier test, qu'il ne faut pas figer dans le nouvel instantané.
                Dim idBase = LireValeur(connexion, "SELECT DB_ID(@nom)", NomBase)
                If idBase IsNot Nothing AndAlso Not IsDBNull(idBase) AndAlso CInt(source) = CInt(idBase) Then
                    Reinitialiser()
                End If
            End If
            SupprimerInstantane(connexion)
        End Using

        CreerInstantane()
        instantanePret = True
    End Sub

    <AssemblyCleanup>
    Public Shared Sub Nettoyer()
        If Not instantanePret Then Return
        ' La base retrouve l'état de départ, sans les données du dernier test, pour
        ' que la prochaine exécution prenne son instantané sur une base propre.
        Try
            Reinitialiser()
        Finally
            Using connexion As New SqlConnection(ChaineAdmin("master"))
                connexion.Open()
                SupprimerInstantane(connexion)
            End Using
            instantanePret = False
        End Try
    End Sub

    ''' <summary>
    ''' Ouvre une connexion Admin avec un délai court. Base injoignable : Inconclusive
    ''' sur un poste de développement, échec franc quand OASIS_IT_REQUIRED=1 (CI).
    ''' </summary>
    Private Shared Sub VerifierBase()
        Try
            Using connexion As New SqlConnection(ChaineAdmin(NomBase, 5))
                connexion.Open()
            End Using
        Catch ex As Exception
            Dim message = $"Base de test injoignable (serveur {Serveur}, base {NomBase}) : {ex.Message} " &
                "Lancer IntegrationTest\Schema\Preparer-Base.ps1 ou renseigner les variables OASIS_IT_*."
            If Environment.GetEnvironmentVariable("OASIS_IT_REQUIRED") = "1" Then
                Throw New InvalidOperationException(message, ex)
            End If
            Assert.Inconclusive(message)
        End Try
    End Sub

    ''' <summary>
    ''' CREATE DATABASE ... AS SNAPSHOT OF, avec une clause ON par fichier de données
    ''' (type ROWS) de la base de test, chacune rangée à côté de son fichier source.
    ''' </summary>
    Private Shared Sub CreerInstantane()
        Dim clauses As New List(Of String)
        Using connexion As New SqlConnection(ChaineAdmin(NomBase))
            connexion.Open()
            Using commande As New SqlCommand(
                    "SELECT name, physical_name FROM sys.database_files WHERE type = 0 ORDER BY file_id", connexion)
                Using lecteur = commande.ExecuteReader()
                    Dim rang = 0
                    While lecteur.Read()
                        Dim nomLogique = CStr(lecteur("name"))
                        Dim physique = CStr(lecteur("physical_name"))
                        Dim dossier = physique.Substring(0, physique.LastIndexOf("\"c) + 1)
                        Dim fichier = If(rang = 0, NomInstantane & ".ss", $"{NomInstantane}_{rang}.ss")
                        clauses.Add($"(NAME = {Crochets(nomLogique)}, FILENAME = {Litteral(dossier & fichier)})")
                        rang += 1
                    End While
                End Using
            End Using
        End Using

        If clauses.Count = 0 Then
            Throw New InvalidOperationException($"Aucun fichier de données trouvé pour la base {NomBase}.")
        End If

        Using connexion As New SqlConnection(ChaineAdmin("master"))
            connexion.Open()
            ExecuterSur(connexion,
                $"CREATE DATABASE {Crochets(NomInstantane)} ON {String.Join(", ", clauses)} AS SNAPSHOT OF {Crochets(NomBase)};")
        End Using
    End Sub

    ''' <summary>
    ''' Supprime l'instantané s'il existe. Seul un instantané est visé (source_database_id
    ''' renseigné) : une vraie base qui porterait ce nom n'est jamais supprimée, et
    ''' CREATE DATABASE échoue alors avec un message explicite.
    ''' </summary>
    Private Shared Sub SupprimerInstantane(connexion As SqlConnection)
        ExecuterSur(connexion,
            $"IF EXISTS (SELECT 1 FROM sys.databases WHERE name = {Litteral(NomInstantane)} AND source_database_id IS NOT NULL) " &
            $"DROP DATABASE {Crochets(NomInstantane)};")
    End Sub

    Private Shared Function LireValeur(connexion As SqlConnection, requete As String, valeur As String) As Object
        Using commande As New SqlCommand(requete, connexion)
            commande.Parameters.AddWithValue("@nom", valeur)
            Return commande.ExecuteScalar()
        End Using
    End Function

End Class
