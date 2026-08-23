Imports System.Configuration
Imports Oasis_Common

Public Class RadFWebBrowser
    Property Url As String

    Private Sub RadFWebBrowser_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        AfficheTitleForm(Me, "Browser", userLog)

        If Url <> "" Then
            WebBrowserDso.Navigate(Url)
        Else
            'Récupération de l'URL du WiKi dans les paramètres de l'application
            Dim UrlByDefault As String = ConfigurationManager.AppSettings("UrlByDefault")
            If UrlByDefault = "" Then
                CreateLog("Paramètre application 'UrlByDefault' non trouvé !", "Web Browser", Log.EnumTypeLog.ERREUR.ToString, userLog)
                UrlByDefault = "http://www.google.com"
            End If
            WebBrowserDso.Navigate(UrlByDefault)
        End If
    End Sub
End Class
