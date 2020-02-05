Imports System.IO
Imports Oasis_WF.My.Resources
Imports Telerik.WinForms.RichTextEditor

Public Class FrmTestRichText

    Sub New()
        RichTextBoxLocalizationProvider.CurrentProvider = RichTextBoxLocalizationProvider.FromStream(New MemoryStream(New System.Text.UTF8Encoding().GetBytes(FrenchRichTextBoxStrings.RichTextBoxStrings)))
        ' Cet appel est requis par le concepteur.
        InitializeComponent()

        ' Ajoutez une initialisation quelconque après l'appel InitializeComponent().

    End Sub
    Private Sub ee()
        'RichTextBoxLocalizationProvider.CurrentProvider
    End Sub
End Class
