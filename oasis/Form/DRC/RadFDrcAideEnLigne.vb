Imports System.Configuration
Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class RadFDrcAideEnLigne
    Property drcId As Long

    Dim drc As Drc
    Dim drcDao As New DrcDao


    Private Sub RadFDrcAideEnLigne_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        drc = drcDao.GetDrcById(drcId)

        TxtDescriptionDrc.Text = drc.DrcLibelle
        TxtCommentaireDrc.Text = drc.Commentaire

        If drc.Wiki <> "" Then
            'Récupération de l'URL du WiKi dans les paramètres de l'application
            Dim UriProcedureTutorielle As String = ConfigurationManager.AppSettings("UriProcedureTutorielle")
            If UriProcedureTutorielle = "" Then
                CreateLog("Paramètre application 'UriProcedureTutorielle' non trouvé !", "Procédure tutorielle", Log.EnumTypeLog.ERREUR.ToString, userLog)
                UriProcedureTutorielle = "http://173.199.71.187/doku.php?id="
            End If

            Dim Url_ProcvedureTutorielle As String = UriProcedureTutorielle & drc.Wiki
            WebBrowser.Navigate(Url_ProcvedureTutorielle)
        Else
            SplitPanel5.Hide()
            RadSplitContainer2.MoveSplitter(Me.RadSplitContainer2.Splitters(0), RadDirection.Up)
        End If

        RadBtnAbandonner.Select()
    End Sub

    Private Sub RadBtnAbandonner_Click(sender As Object, e As EventArgs) Handles RadBtnAbandonner.Click
        Close()
    End Sub
End Class
