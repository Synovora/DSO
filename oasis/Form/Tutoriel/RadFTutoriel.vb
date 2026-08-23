Imports System.Configuration
Imports Oasis_Common
Imports Telerik.WinControls.UI

Public Class RadFTutoriel
    Property drcId As Long
    Property parametreId As Long

    Dim drc As Drc
    Dim parametre As Parametre
    Dim Wiki As String

    Dim drcDao As New DrcDao
    Dim parametreDao As New ParametreDao


    Private Sub RadFDrcAideEnLigne_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        If drcId <> 0 Then
            drc = drcDao.GetDrcById(drcId)

            TxtDescriptionDrc.Text = drc.DrcLibelle
            TxtCommentaireDrc.Text = drc.Commentaire
            Wiki = drc.Wiki
        Else
            If parametreId <> 0 Then
                parametre = parametreDao.GetParametreById(parametreId)
                TxtDescriptionDrc.Text = parametre.Description
                TxtCommentaireDrc.Text = parametre.AideAssociee
                Wiki = parametre.Wiki
            Else
                MessageBox.Show("Pas de tutoriel existant !")
                Close()
            End If
        End If

        If Wiki <> "" Then
            'Récupération de l'URL du WiKi dans les paramètres de l'application
            Dim UriProcedureTutorielle As String = ConfigurationManager.AppSettings("UriProcedureTutorielle")
            If UriProcedureTutorielle = "" Then
                CreateLog("Paramètre application 'UriProcedureTutorielle' non trouvé !", "Procédure tutorielle", Log.EnumTypeLog.ERREUR.ToString, userLog)
                UriProcedureTutorielle = "http://173.199.71.187/doku.php?id="
            End If

            Dim Url_ProcvedureTutorielle As String = UriProcedureTutorielle & Wiki
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
