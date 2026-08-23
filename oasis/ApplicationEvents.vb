Namespace My
    ' Les événements suivants sont disponibles pour MyApplication :
    ' Startup : Déclenché au démarrage de l'application avant la création du formulaire de démarrage.
    ' Shutdown : Déclenché après la fermeture de tous les formulaires de l'application.  Cet événement n'est pas déclenché si l'application se termine de façon anormale.
    ' UnhandledException : Déclenché si l'application rencontre une exception non gérée.
    ' StartupNextInstance : Déclenché lors du lancement d'une application à instance unique et si cette application est déjà active. 
    ' NetworkAvailabilityChanged : Déclenché quand la connexion réseau est connectée ou déconnectée.
    Partial Friend Class MyApplication

        Private Sub MyApplication_Startup(sender As Object, e As Microsoft.VisualBasic.ApplicationServices.StartupEventArgs) Handles Me.Startup
            ' Le client lourd est interactif : il peut proposer de réessayer une
            ' connexion à la base. StandardDao ne le fait plus de lui-même, car la
            ' même classe sert au portail web où une boîte de dialogue bloque le
            ' thread IIS indéfiniment.
            Oasis_Common.StandardDao.DemanderNouvelEssaiConnexion =
                Function(messageErreur As String) As Boolean
                    Return MsgBox("Problème de connexion à la base de données (" & messageErreur & ")" & vbCrLf &
                                  "Voulez-vous réessayer ?",
                                  MsgBoxStyle.RetryCancel Or MsgBoxStyle.Exclamation,
                                  "Oasis") = MsgBoxResult.Retry
                End Function
        End Sub

    End Class
End Namespace
