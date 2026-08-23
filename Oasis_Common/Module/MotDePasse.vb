Imports System.Security.Cryptography
Imports System.Text

''' <summary>
''' Stockage des mots de passe.
'''
''' Les mots de passe étaient dérivés par un SHA-1 en un seul tour, avec un
''' « poivre » constant écrit dans le source (donc public) et sans sel propre à
''' chaque compte : deux comptes de même mot de passe donnaient la même empreinte,
''' et un vol de table se cassait hors ligne en quelques minutes.
'''
''' Format produit : PBKDF2$iterations$sel$empreinte (sel et empreinte en base64).
''' La vérification accepte encore les anciennes empreintes SHA-1 le temps de la
''' migration : voir VerifierAvecMigration.
''' </summary>
Public Module MotDePasse

    Private Const Iterations As Integer = 200000
    Private Const TailleSel As Integer = 16
    Private Const TailleEmpreinte As Integer = 32
    Private Const Prefixe As String = "PBKDF2$"

    ''' <summary>Empreinte d'un mot de passe, avec sel aléatoire propre à l'appel.</summary>
    Public Function Hacher(motDePasse As String) As String
        Dim sel(TailleSel - 1) As Byte
        Using rng = RandomNumberGenerator.Create()
            rng.GetBytes(sel)
        End Using
        Using kdf As New Rfc2898DeriveBytes(motDePasse, sel, Iterations, HashAlgorithmName.SHA256)
            Return Prefixe & Iterations & "$" &
                   Convert.ToBase64String(sel) & "$" &
                   Convert.ToBase64String(kdf.GetBytes(TailleEmpreinte))
        End Using
    End Function

    ''' <summary>Vrai si la valeur stockée est au format PBKDF2 de ce module.</summary>
    Public Function EstFormatPbkdf2(valeurStockee As String) As Boolean
        Return valeurStockee IsNot Nothing AndAlso valeurStockee.StartsWith(Prefixe, StringComparison.Ordinal)
    End Function

    ''' <summary>
    ''' Vérifie un mot de passe contre une empreinte PBKDF2. Comparaison à temps
    ''' constant : le temps de réponse ne doit pas révéler le préfixe correct.
    ''' </summary>
    Public Function Verifier(motDePasse As String, valeurStockee As String) As Boolean
        If motDePasse Is Nothing OrElse Not EstFormatPbkdf2(valeurStockee) Then Return False

        Dim morceaux = valeurStockee.Split("$"c)
        If morceaux.Length <> 4 Then Return False

        Dim iterations As Integer
        If Not Integer.TryParse(morceaux(1), iterations) OrElse iterations <= 0 Then Return False

        Dim sel As Byte()
        Dim attendue As Byte()
        Try
            sel = Convert.FromBase64String(morceaux(2))
            attendue = Convert.FromBase64String(morceaux(3))
        Catch ex As FormatException
            Return False
        End Try

        Using kdf As New Rfc2898DeriveBytes(motDePasse, sel, iterations, HashAlgorithmName.SHA256)
            Return ComparaisonTempsConstant(kdf.GetBytes(attendue.Length), attendue)
        End Using
    End Function

    ''' <summary>
    ''' Vérifie un mot de passe en acceptant les deux formats.
    ''' </summary>
    ''' <param name="empreinteHeritee">
    ''' Empreinte calculée par l'ancien algorithme (Utilisateur.CryptePwd ou
    ''' Internaute.CryptePwd) pour ce couple identifiant / mot de passe.
    ''' </param>
    ''' <param name="doitEtreRehache">
    ''' Renvoyé à True quand le mot de passe est correct mais encore stocké à
    ''' l'ancien format : l'appelant doit alors réenregistrer Hacher(motDePasse).
    ''' </param>
    Public Function VerifierAvecMigration(motDePasse As String,
                                          valeurStockee As String,
                                          empreinteHeritee As String,
                                          ByRef doitEtreRehache As Boolean) As Boolean
        doitEtreRehache = False
        If valeurStockee Is Nothing Then Return False

        If EstFormatPbkdf2(valeurStockee) Then
            Return Verifier(motDePasse, valeurStockee)
        End If

        ' Ancien format : comparaison à temps constant également, puis demande de
        ' réenregistrement au format courant.
        If ComparaisonTempsConstant(Encoding.UTF8.GetBytes(valeurStockee),
                                    Encoding.UTF8.GetBytes(If(empreinteHeritee, ""))) Then
            doitEtreRehache = True
            Return True
        End If
        Return False
    End Function

    Private Function ComparaisonTempsConstant(a As Byte(), b As Byte()) As Boolean
        If a Is Nothing OrElse b Is Nothing Then Return False
        ' La longueur peut différer : on la révèle, jamais le contenu.
        If a.Length <> b.Length Then Return False
        Dim ecart As Integer = 0
        For i = 0 To a.Length - 1
            ecart = ecart Or (a(i) Xor b(i))
        Next
        Return ecart = 0
    End Function

End Module
