<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FMenuAdmin
    Inherits System.Windows.Forms.Form

    'Form remplace la méthode Dispose pour nettoyer la liste des composants.
    <System.Diagnostics.DebuggerNonUserCode()> _
    Protected Overrides Sub Dispose(ByVal disposing As Boolean)
        Try
            If disposing AndAlso components IsNot Nothing Then
                components.Dispose()
            End If
        Finally
            MyBase.Dispose(disposing)
        End Try
    End Sub

    'Requise par le Concepteur Windows Form
    Private components As System.ComponentModel.IContainer

    'REMARQUE : la procédure suivante est requise par le Concepteur Windows Form
    'Elle peut être modifiée à l'aide du Concepteur Windows Form.  
    'Ne la modifiez pas à l'aide de l'éditeur de code.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.MenuStrip1 = New System.Windows.Forms.MenuStrip()
        Me.TablesDeBaseToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UnitéSanitaireToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SiteToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TerritoireToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TablesMétierToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SpécialitéToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RORToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DRCORCToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.PatientToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RéférentielMédicamenteuxToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.DRCNouveauToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.MédicamentsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OrganisationToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ProfilToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.FonctionToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.UtilisateursToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.HabilitationsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.GénérauxToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.ParamètresMétierToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.OutilsToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.RepublicationDantécédentsOccultésToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.TestToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelecteurDRCToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.SelecteurDRCCatégorie1ToolStripMenuItem = New System.Windows.Forms.ToolStripMenuItem()
        Me.Timer1 = New System.Windows.Forms.Timer(Me.components)
        Me.MenuStrip1.SuspendLayout()
        Me.SuspendLayout()
        '
        'MenuStrip1
        '
        Me.MenuStrip1.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.TablesDeBaseToolStripMenuItem, Me.TablesMétierToolStripMenuItem, Me.OrganisationToolStripMenuItem, Me.GénérauxToolStripMenuItem, Me.OutilsToolStripMenuItem, Me.TestToolStripMenuItem})
        Me.MenuStrip1.Location = New System.Drawing.Point(0, 0)
        Me.MenuStrip1.Name = "MenuStrip1"
        Me.MenuStrip1.Size = New System.Drawing.Size(800, 24)
        Me.MenuStrip1.TabIndex = 0
        Me.MenuStrip1.Text = "MenuStrip1"
        '
        'TablesDeBaseToolStripMenuItem
        '
        Me.TablesDeBaseToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.UnitéSanitaireToolStripMenuItem, Me.SiteToolStripMenuItem, Me.TerritoireToolStripMenuItem})
        Me.TablesDeBaseToolStripMenuItem.Name = "TablesDeBaseToolStripMenuItem"
        Me.TablesDeBaseToolStripMenuItem.Size = New System.Drawing.Size(67, 20)
        Me.TablesDeBaseToolStripMenuItem.Text = "Structure"
        '
        'UnitéSanitaireToolStripMenuItem
        '
        Me.UnitéSanitaireToolStripMenuItem.Name = "UnitéSanitaireToolStripMenuItem"
        Me.UnitéSanitaireToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.UnitéSanitaireToolStripMenuItem.Text = "Unité sanitaire"
        '
        'SiteToolStripMenuItem
        '
        Me.SiteToolStripMenuItem.Name = "SiteToolStripMenuItem"
        Me.SiteToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.SiteToolStripMenuItem.Text = "Site"
        '
        'TerritoireToolStripMenuItem
        '
        Me.TerritoireToolStripMenuItem.Name = "TerritoireToolStripMenuItem"
        Me.TerritoireToolStripMenuItem.Size = New System.Drawing.Size(149, 22)
        Me.TerritoireToolStripMenuItem.Text = "Territoire"
        '
        'TablesMétierToolStripMenuItem
        '
        Me.TablesMétierToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SpécialitéToolStripMenuItem, Me.RORToolStripMenuItem, Me.DRCORCToolStripMenuItem, Me.PatientToolStripMenuItem, Me.RéférentielMédicamenteuxToolStripMenuItem, Me.DRCNouveauToolStripMenuItem, Me.MédicamentsToolStripMenuItem})
        Me.TablesMétierToolStripMenuItem.Name = "TablesMétierToolStripMenuItem"
        Me.TablesMétierToolStripMenuItem.Size = New System.Drawing.Size(53, 20)
        Me.TablesMétierToolStripMenuItem.Text = "Métier"
        '
        'SpécialitéToolStripMenuItem
        '
        Me.SpécialitéToolStripMenuItem.Name = "SpécialitéToolStripMenuItem"
        Me.SpécialitéToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.SpécialitéToolStripMenuItem.Text = "Spécialité"
        '
        'RORToolStripMenuItem
        '
        Me.RORToolStripMenuItem.Name = "RORToolStripMenuItem"
        Me.RORToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.RORToolStripMenuItem.Text = "ROR"
        '
        'DRCORCToolStripMenuItem
        '
        Me.DRCORCToolStripMenuItem.Name = "DRCORCToolStripMenuItem"
        Me.DRCORCToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.DRCORCToolStripMenuItem.Text = "DRC/ORC"
        '
        'PatientToolStripMenuItem
        '
        Me.PatientToolStripMenuItem.Name = "PatientToolStripMenuItem"
        Me.PatientToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.PatientToolStripMenuItem.Text = "Patient"
        '
        'RéférentielMédicamenteuxToolStripMenuItem
        '
        Me.RéférentielMédicamenteuxToolStripMenuItem.Name = "RéférentielMédicamenteuxToolStripMenuItem"
        Me.RéférentielMédicamenteuxToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.RéférentielMédicamenteuxToolStripMenuItem.Text = "Référentiel médicamenteux"
        '
        'DRCNouveauToolStripMenuItem
        '
        Me.DRCNouveauToolStripMenuItem.Name = "DRCNouveauToolStripMenuItem"
        Me.DRCNouveauToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.DRCNouveauToolStripMenuItem.Text = "DRC Nouveau"
        '
        'MédicamentsToolStripMenuItem
        '
        Me.MédicamentsToolStripMenuItem.Name = "MédicamentsToolStripMenuItem"
        Me.MédicamentsToolStripMenuItem.Size = New System.Drawing.Size(219, 22)
        Me.MédicamentsToolStripMenuItem.Text = "Médicaments"
        '
        'OrganisationToolStripMenuItem
        '
        Me.OrganisationToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ProfilToolStripMenuItem, Me.FonctionToolStripMenuItem, Me.UtilisateursToolStripMenuItem, Me.HabilitationsToolStripMenuItem})
        Me.OrganisationToolStripMenuItem.Name = "OrganisationToolStripMenuItem"
        Me.OrganisationToolStripMenuItem.Size = New System.Drawing.Size(87, 20)
        Me.OrganisationToolStripMenuItem.Text = "Organisation"
        '
        'ProfilToolStripMenuItem
        '
        Me.ProfilToolStripMenuItem.Name = "ProfilToolStripMenuItem"
        Me.ProfilToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.ProfilToolStripMenuItem.Text = "Profil"
        '
        'FonctionToolStripMenuItem
        '
        Me.FonctionToolStripMenuItem.Name = "FonctionToolStripMenuItem"
        Me.FonctionToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.FonctionToolStripMenuItem.Text = "Fonction"
        '
        'UtilisateursToolStripMenuItem
        '
        Me.UtilisateursToolStripMenuItem.Name = "UtilisateursToolStripMenuItem"
        Me.UtilisateursToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.UtilisateursToolStripMenuItem.Text = "Utilisateurs"
        '
        'HabilitationsToolStripMenuItem
        '
        Me.HabilitationsToolStripMenuItem.Name = "HabilitationsToolStripMenuItem"
        Me.HabilitationsToolStripMenuItem.Size = New System.Drawing.Size(180, 22)
        Me.HabilitationsToolStripMenuItem.Text = "Habilitations"
        '
        'GénérauxToolStripMenuItem
        '
        Me.GénérauxToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ParamètresMétierToolStripMenuItem})
        Me.GénérauxToolStripMenuItem.Name = "GénérauxToolStripMenuItem"
        Me.GénérauxToolStripMenuItem.Size = New System.Drawing.Size(69, 20)
        Me.GénérauxToolStripMenuItem.Text = "Généraux"
        '
        'ParamètresMétierToolStripMenuItem
        '
        Me.ParamètresMétierToolStripMenuItem.Name = "ParamètresMétierToolStripMenuItem"
        Me.ParamètresMétierToolStripMenuItem.Size = New System.Drawing.Size(170, 22)
        Me.ParamètresMétierToolStripMenuItem.Text = "Paramètres métier"
        '
        'OutilsToolStripMenuItem
        '
        Me.OutilsToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.RepublicationDantécédentsOccultésToolStripMenuItem})
        Me.OutilsToolStripMenuItem.Name = "OutilsToolStripMenuItem"
        Me.OutilsToolStripMenuItem.Size = New System.Drawing.Size(50, 20)
        Me.OutilsToolStripMenuItem.Text = "Outils"
        '
        'RepublicationDantécédentsOccultésToolStripMenuItem
        '
        Me.RepublicationDantécédentsOccultésToolStripMenuItem.Name = "RepublicationDantécédentsOccultésToolStripMenuItem"
        Me.RepublicationDantécédentsOccultésToolStripMenuItem.Size = New System.Drawing.Size(276, 22)
        Me.RepublicationDantécédentsOccultésToolStripMenuItem.Text = "Re-publication d'antécédents occultés"
        '
        'TestToolStripMenuItem
        '
        Me.TestToolStripMenuItem.DropDownItems.AddRange(New System.Windows.Forms.ToolStripItem() {Me.SelecteurDRCToolStripMenuItem, Me.SelecteurDRCCatégorie1ToolStripMenuItem})
        Me.TestToolStripMenuItem.Name = "TestToolStripMenuItem"
        Me.TestToolStripMenuItem.Size = New System.Drawing.Size(39, 20)
        Me.TestToolStripMenuItem.Text = "Test"
        '
        'SelecteurDRCToolStripMenuItem
        '
        Me.SelecteurDRCToolStripMenuItem.Name = "SelecteurDRCToolStripMenuItem"
        Me.SelecteurDRCToolStripMenuItem.Size = New System.Drawing.Size(209, 22)
        Me.SelecteurDRCToolStripMenuItem.Text = "Selecteur DRC"
        '
        'SelecteurDRCCatégorie1ToolStripMenuItem
        '
        Me.SelecteurDRCCatégorie1ToolStripMenuItem.Name = "SelecteurDRCCatégorie1ToolStripMenuItem"
        Me.SelecteurDRCCatégorie1ToolStripMenuItem.Size = New System.Drawing.Size(209, 22)
        Me.SelecteurDRCCatégorie1ToolStripMenuItem.Text = "Selecteur DRC catégorie 1"
        '
        'FMenuAdmin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(800, 450)
        Me.Controls.Add(Me.MenuStrip1)
        Me.MainMenuStrip = Me.MenuStrip1
        Me.Name = "FMenuAdmin"
        Me.Text = "Administration du DPI"
        Me.MenuStrip1.ResumeLayout(False)
        Me.MenuStrip1.PerformLayout()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents MenuStrip1 As MenuStrip
    Friend WithEvents TablesDeBaseToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UnitéSanitaireToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SiteToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TerritoireToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TablesMétierToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SpécialitéToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RORToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DRCORCToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents PatientToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents OrganisationToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ProfilToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents FonctionToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents UtilisateursToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents HabilitationsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents GénérauxToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents ParamètresMétierToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RéférentielMédicamenteuxToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents Timer1 As Timer
    Friend WithEvents OutilsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents RepublicationDantécédentsOccultésToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents DRCNouveauToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents MédicamentsToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents TestToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelecteurDRCToolStripMenuItem As ToolStripMenuItem
    Friend WithEvents SelecteurDRCCatégorie1ToolStripMenuItem As ToolStripMenuItem
    'Private WithEvents WorkspaceManager1 As WorkspaceManager
End Class
