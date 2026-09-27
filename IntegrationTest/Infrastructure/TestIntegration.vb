''' <summary>
''' Classe de base de tous les tests d'intégration. Avant chaque test, la base est
''' ramenée à l'instantané pris au démarrage et la configuration pointe sur le compte
''' du client lourd : chaque test part d'une base propre, quel que soit l'ordre
''' d'exécution. Un test qui exerce du code serveur appelle UtiliserCompte(Compte.Web)
''' en tête.
''' </summary>
Public MustInherit Class TestIntegration

    ''' <summary>
    ''' Renseignée par MSTest avant chaque test. MSTest ne la cherche que parmi les
    ''' propriétés publiques, d'où sa visibilité.
    ''' </summary>
    Public Property TestContext As TestContext

    <TestInitialize>
    Public Sub RemettreBaseAZero()
        Reinitialiser()
        UtiliserCompte(Compte.Client)
    End Sub

End Class
