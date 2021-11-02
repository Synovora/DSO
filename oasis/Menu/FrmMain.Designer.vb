<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class FrmMain
    Inherits Telerik.WinControls.UI.RadForm

    'Form overrides dispose to clean up the component list.
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

    'Required by the Windows Form Designer
    Private components As System.ComponentModel.IContainer

    'NOTE: The following procedure is required by the Windows Form Designer
    'It can be modified using the Windows Form Designer.  
    'Do not modify it using the code editor.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.TileGroupAdmin = New Telerik.WinControls.UI.TileGroupElement()
        Me.RadTileUser = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTileSite = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTileProfil = New Telerik.WinControls.UI.RadTileElement()
        Me.RadPanorama1 = New Telerik.WinControls.UI.RadPanorama()
        Me.RadBtnAbandon = New Telerik.WinControls.UI.RadButton()
        Me.TileGroupElement1 = New Telerik.WinControls.UI.TileGroupElement()
        Me.RadTileElement1 = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTileElement2 = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTileElement3 = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTileElement4 = New Telerik.WinControls.UI.RadTileElement()
        Me.TileGroupElement2 = New Telerik.WinControls.UI.TileGroupElement()
        Me.RadTileSpecialite = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTileROR = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTileDRCORC = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTilePatient = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTileRefMed = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTileDRCNouveau = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTileMedicament = New Telerik.WinControls.UI.RadTileElement()
        Me.TileGroupOrganisation = New Telerik.WinControls.UI.TileGroupElement()
        Me.RadTileElementProfil = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTileElementFonction = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTileElementUtilisateur = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTileElementHabilitation = New Telerik.WinControls.UI.RadTileElement()
        Me.TileGroupGeneraux = New Telerik.WinControls.UI.TileGroupElement()
        Me.RadTileElementParamMetier = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTileElementTemplateSE = New Telerik.WinControls.UI.RadTileElement()
        Me.TileGroupOutils = New Telerik.WinControls.UI.TileGroupElement()
        Me.RadTileElementRepublication = New Telerik.WinControls.UI.RadTileElement()
        Me.RadTileElementAPropos = New Telerik.WinControls.UI.RadTileElement()
        Me.TileGroupElement3 = New Telerik.WinControls.UI.TileGroupElement()
        Me.VaccinImport = New Telerik.WinControls.UI.RadTileElement()
        Me.CGV = New Telerik.WinControls.UI.RadTileElement()
        CType(Me.RadPanorama1, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.RadPanorama1.SuspendLayout()
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'TileGroupAdmin
        '
        Me.TileGroupAdmin.CellSize = New System.Drawing.Size(150, 100)
        Me.TileGroupAdmin.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.TileGroupAdmin.Items.AddRange(New Telerik.WinControls.RadItem() {Me.RadTileUser, Me.RadTileSite, Me.RadTileProfil})
        Me.TileGroupAdmin.Name = "TileGroupAdmin"
        Me.TileGroupAdmin.RowsCount = 3
        Me.TileGroupAdmin.Text = "Administration"
        Me.TileGroupAdmin.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.TileGroupAdmin.UseCompatibleTextRendering = False
        '
        'RadTileUser
        '
        Me.RadTileUser.AutoSize = True
        Me.RadTileUser.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.RadTileUser.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.RadTileUser.FitToSizeMode = Telerik.WinControls.RadFitToSizeMode.FitToParentBounds
        Me.RadTileUser.Name = "RadTileUser"
        Me.RadTileUser.Text = "Utilisateurs"
        Me.RadTileUser.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.RadTileUser.UseCompatibleTextRendering = False
        '
        'RadTileSite
        '
        Me.RadTileSite.AutoSize = True
        Me.RadTileSite.Column = 1
        Me.RadTileSite.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.RadTileSite.Name = "RadTileSite"
        Me.RadTileSite.Text = "Sites"
        Me.RadTileSite.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.RadTileSite.UseCompatibleTextRendering = False
        '
        'RadTileProfil
        '
        Me.RadTileProfil.Column = 2
        Me.RadTileProfil.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.RadTileProfil.Name = "RadTileProfil"
        Me.RadTileProfil.Text = "Profils"
        Me.RadTileProfil.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.RadTileProfil.UseCompatibleTextRendering = False
        '
        'RadPanorama1
        '
        Me.RadPanorama1.AutoScroll = True
        Me.RadPanorama1.Controls.Add(Me.RadBtnAbandon)
        Me.RadPanorama1.Cursor = System.Windows.Forms.Cursors.Default
        Me.RadPanorama1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadPanorama1.EnableGestures = False
        Me.RadPanorama1.EnableZooming = False
        Me.RadPanorama1.Groups.AddRange(New Telerik.WinControls.RadItem() {Me.TileGroupElement1, Me.TileGroupElement2, Me.TileGroupOrganisation, Me.TileGroupGeneraux, Me.TileGroupOutils, Me.TileGroupElement3})
        Me.RadPanorama1.Location = New System.Drawing.Point(0, 0)
        Me.RadPanorama1.Name = "RadPanorama1"
        Me.RadPanorama1.RowsCount = 6
        Me.RadPanorama1.ShowGroups = True
        Me.RadPanorama1.Size = New System.Drawing.Size(1523, 643)
        Me.RadPanorama1.TabIndex = 1
        Me.RadPanorama1.ThemeName = "ControlDefault"
        '
        'RadBtnAbandon
        '
        Me.RadBtnAbandon.DialogResult = System.Windows.Forms.DialogResult.Cancel
        Me.RadBtnAbandon.Location = New System.Drawing.Point(1401, 616)
        Me.RadBtnAbandon.Name = "RadBtnAbandon"
        Me.RadBtnAbandon.Size = New System.Drawing.Size(110, 24)
        Me.RadBtnAbandon.TabIndex = 0
        Me.RadBtnAbandon.Text = "Abandonner"
        Me.RadBtnAbandon.Visible = False
        '
        'TileGroupElement1
        '
        Me.TileGroupElement1.CellSize = New System.Drawing.Size(120, 100)
        Me.TileGroupElement1.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.TileGroupElement1.Items.AddRange(New Telerik.WinControls.RadItem() {Me.RadTileElement1, Me.RadTileElement2, Me.RadTileElement3, Me.RadTileElement4})
        Me.TileGroupElement1.Name = "TileGroupElement1"
        Me.TileGroupElement1.RowsCount = 3
        Me.TileGroupElement1.Text = "Structure"
        Me.TileGroupElement1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.TileGroupElement1.UseCompatibleTextRendering = False
        '
        'RadTileElement1
        '
        Me.RadTileElement1.AutoSize = True
        Me.RadTileElement1.AutoSizeMode = Telerik.WinControls.RadAutoSizeMode.WrapAroundChildren
        Me.RadTileElement1.ColSpan = 2
        Me.RadTileElement1.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.RadTileElement1.FitToSizeMode = Telerik.WinControls.RadFitToSizeMode.FitToParentBounds
        Me.RadTileElement1.Name = "RadTileElement1"
        Me.RadTileElement1.Text = "Unite Sanitaire"
        Me.RadTileElement1.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.RadTileElement1.UseCompatibleTextRendering = False
        '
        'RadTileElement2
        '
        Me.RadTileElement2.AutoSize = True
        Me.RadTileElement2.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.RadTileElement2.Name = "RadTileElement2"
        Me.RadTileElement2.Row = 1
        Me.RadTileElement2.Text = "Site"
        Me.RadTileElement2.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.RadTileElement2.UseCompatibleTextRendering = False
        '
        'RadTileElement3
        '
        Me.RadTileElement3.Column = 1
        Me.RadTileElement3.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.RadTileElement3.Name = "RadTileElement3"
        Me.RadTileElement3.Row = 1
        Me.RadTileElement3.Text = "Territoire"
        Me.RadTileElement3.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.RadTileElement3.UseCompatibleTextRendering = False
        '
        'RadTileElement4
        '
        Me.RadTileElement4.ColSpan = 2
        Me.RadTileElement4.Name = "RadTileElement4"
        Me.RadTileElement4.Row = 2
        Me.RadTileElement4.Text = "Etat Journalier"
        '
        'TileGroupElement2
        '
        Me.TileGroupElement2.Items.AddRange(New Telerik.WinControls.RadItem() {Me.RadTileSpecialite, Me.RadTileROR, Me.RadTileDRCORC, Me.RadTilePatient, Me.RadTileRefMed, Me.RadTileDRCNouveau, Me.RadTileMedicament})
        Me.TileGroupElement2.Name = "TileGroupElement2"
        Me.TileGroupElement2.RowsCount = 6
        Me.TileGroupElement2.Text = "Métier"
        '
        'RadTileSpecialite
        '
        Me.RadTileSpecialite.ColSpan = 2
        Me.RadTileSpecialite.Name = "RadTileSpecialite"
        Me.RadTileSpecialite.Text = "Spécialité"
        '
        'RadTileROR
        '
        Me.RadTileROR.Column = 2
        Me.RadTileROR.Name = "RadTileROR"
        Me.RadTileROR.Text = "ROR"
        '
        'RadTileDRCORC
        '
        Me.RadTileDRCORC.ColSpan = 2
        Me.RadTileDRCORC.Name = "RadTileDRCORC"
        Me.RadTileDRCORC.Row = 1
        Me.RadTileDRCORC.Text = "DRC/ORC"
        '
        'RadTilePatient
        '
        Me.RadTilePatient.Column = 2
        Me.RadTilePatient.Name = "RadTilePatient"
        Me.RadTilePatient.Row = 1
        Me.RadTilePatient.Text = "Patient"
        '
        'RadTileRefMed
        '
        Me.RadTileRefMed.ColSpan = 3
        Me.RadTileRefMed.Name = "RadTileRefMed"
        Me.RadTileRefMed.Row = 2
        Me.RadTileRefMed.Text = "Référentiel médicamenteux"
        '
        'RadTileDRCNouveau
        '
        Me.RadTileDRCNouveau.ColSpan = 2
        Me.RadTileDRCNouveau.Name = "RadTileDRCNouveau"
        Me.RadTileDRCNouveau.Row = 3
        Me.RadTileDRCNouveau.Text = "A définir"
        Me.RadTileDRCNouveau.Visibility = Telerik.WinControls.ElementVisibility.Hidden
        '
        'RadTileMedicament
        '
        Me.RadTileMedicament.ColSpan = 2
        Me.RadTileMedicament.Name = "RadTileMedicament"
        Me.RadTileMedicament.Row = 4
        Me.RadTileMedicament.Text = "Médicament"
        Me.RadTileMedicament.Visibility = Telerik.WinControls.ElementVisibility.Hidden
        '
        'TileGroupOrganisation
        '
        Me.TileGroupOrganisation.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.TileGroupOrganisation.Items.AddRange(New Telerik.WinControls.RadItem() {Me.RadTileElementProfil, Me.RadTileElementFonction, Me.RadTileElementUtilisateur, Me.RadTileElementHabilitation})
        Me.TileGroupOrganisation.Name = "TileGroupOrganisation"
        Me.TileGroupOrganisation.RowsCount = 2
        Me.TileGroupOrganisation.Text = "Organisation"
        Me.TileGroupOrganisation.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.TileGroupOrganisation.UseCompatibleTextRendering = False
        '
        'RadTileElementProfil
        '
        Me.RadTileElementProfil.DisabledTextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.RadTileElementProfil.Name = "RadTileElementProfil"
        Me.RadTileElementProfil.Text = "Profil"
        Me.RadTileElementProfil.TextRenderingHint = System.Drawing.Text.TextRenderingHint.SystemDefault
        Me.RadTileElementProfil.UseCompatibleTextRendering = False
        '
        'RadTileElementFonction
        '
        Me.RadTileElementFonction.Name = "RadTileElementFonction"
        Me.RadTileElementFonction.Row = 1
        Me.RadTileElementFonction.Text = "Fonction"
        '
        'RadTileElementUtilisateur
        '
        Me.RadTileElementUtilisateur.ColSpan = 2
        Me.RadTileElementUtilisateur.Column = 1
        Me.RadTileElementUtilisateur.Name = "RadTileElementUtilisateur"
        Me.RadTileElementUtilisateur.Row = 1
        Me.RadTileElementUtilisateur.Text = "Utilisateur"
        '
        'RadTileElementHabilitation
        '
        Me.RadTileElementHabilitation.ColSpan = 2
        Me.RadTileElementHabilitation.Column = 1
        Me.RadTileElementHabilitation.Name = "RadTileElementHabilitation"
        Me.RadTileElementHabilitation.Text = "Habilitation"
        '
        'TileGroupGeneraux
        '
        Me.TileGroupGeneraux.AutoSize = True
        Me.TileGroupGeneraux.CellSize = New System.Drawing.Size(130, 100)
        Me.TileGroupGeneraux.Items.AddRange(New Telerik.WinControls.RadItem() {Me.RadTileElementParamMetier, Me.RadTileElementTemplateSE})
        Me.TileGroupGeneraux.Name = "TileGroupGeneraux"
        Me.TileGroupGeneraux.RowsCount = 2
        Me.TileGroupGeneraux.Text = "Généraux"
        '
        'RadTileElementParamMetier
        '
        Me.RadTileElementParamMetier.Name = "RadTileElementParamMetier"
        Me.RadTileElementParamMetier.Text = "Paramètres " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "métier" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'RadTileElementTemplateSE
        '
        Me.RadTileElementTemplateSE.Name = "RadTileElementTemplateSE"
        Me.RadTileElementTemplateSE.Row = 1
        Me.RadTileElementTemplateSE.Text = "Modèles Sous-Episode"
        Me.RadTileElementTemplateSE.TextWrap = True
        '
        'TileGroupOutils
        '
        Me.TileGroupOutils.Items.AddRange(New Telerik.WinControls.RadItem() {Me.RadTileElementRepublication, Me.RadTileElementAPropos})
        Me.TileGroupOutils.Name = "TileGroupOutils"
        Me.TileGroupOutils.RowsCount = 5
        Me.TileGroupOutils.ShowHorizontalLine = False
        Me.TileGroupOutils.Text = "Outils"
        '
        'RadTileElementRepublication
        '
        Me.RadTileElementRepublication.ColSpan = 2
        Me.RadTileElementRepublication.Name = "RadTileElementRepublication"
        Me.RadTileElementRepublication.Text = "Re-publication " & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10) & "d'éléments occultés" & Global.Microsoft.VisualBasic.ChrW(13) & Global.Microsoft.VisualBasic.ChrW(10)
        '
        'RadTileElementAPropos
        '
        Me.RadTileElementAPropos.ColSpan = 2
        Me.RadTileElementAPropos.Name = "RadTileElementAPropos"
        Me.RadTileElementAPropos.Row = 4
        Me.RadTileElementAPropos.Text = "A Propos"
        '
        'TileGroupElement3
        '
        Me.TileGroupElement3.Items.AddRange(New Telerik.WinControls.RadItem() {Me.VaccinImport, Me.CGV})
        Me.TileGroupElement3.Name = "TileGroupElement3"
        Me.TileGroupElement3.RowsCount = 4
        Me.TileGroupElement3.Text = "Volet Vaccinal"
        '
        'VaccinImport
        '
        Me.VaccinImport.ColSpan = 2
        Me.VaccinImport.Name = "VaccinImport"
        Me.VaccinImport.RowSpan = 1
        Me.VaccinImport.Text = "Association Vaccin-Valence"
        '
        'CGV
        '
        Me.CGV.ColSpan = 2
        Me.CGV.Name = "CGV"
        Me.CGV.Row = 1
        Me.CGV.Text = "Calendrier General Vaccinal"
        '
        'FrmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.CancelButton = Me.RadBtnAbandon
        Me.ClientSize = New System.Drawing.Size(1523, 643)
        Me.Controls.Add(Me.RadPanorama1)
        Me.Cursor = System.Windows.Forms.Cursors.Default
        Me.Name = "FrmMain"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.StartPosition = System.Windows.Forms.FormStartPosition.CenterScreen
        Me.Text = "Oasis - Menu"
        CType(Me.RadPanorama1, System.ComponentModel.ISupportInitialize).EndInit()
        Me.RadPanorama1.ResumeLayout(False)
        CType(Me.RadBtnAbandon, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)

    End Sub

    Friend WithEvents TileGroupAdmin As Telerik.WinControls.UI.TileGroupElement
    Friend WithEvents RadTileUser As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadTileSite As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadTileProfil As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadPanorama1 As Telerik.WinControls.UI.RadPanorama
    Friend WithEvents TileGroupElement1 As Telerik.WinControls.UI.TileGroupElement
    Friend WithEvents RadTileElement1 As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadTileElement2 As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadTileElement3 As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents TileGroupOrganisation As Telerik.WinControls.UI.TileGroupElement
    Friend WithEvents RadTileElementProfil As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents TileGroupElement2 As Telerik.WinControls.UI.TileGroupElement
    Friend WithEvents RadTileSpecialite As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadTileROR As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadTileDRCORC As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadTilePatient As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadTileRefMed As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadTileDRCNouveau As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadTileMedicament As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadTileElementFonction As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadTileElementUtilisateur As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadTileElementHabilitation As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents TileGroupOutils As Telerik.WinControls.UI.TileGroupElement
    Friend WithEvents RadTileElementRepublication As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents TileGroupGeneraux As Telerik.WinControls.UI.TileGroupElement
    Friend WithEvents RadTileElementParamMetier As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadTileElementAPropos As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadBtnAbandon As Telerik.WinControls.UI.RadButton
    Friend WithEvents RadTileElementTemplateSE As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents RadTileElement4 As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents TileGroupElement3 As Telerik.WinControls.UI.TileGroupElement
    Friend WithEvents VaccinImport As Telerik.WinControls.UI.RadTileElement
    Friend WithEvents CGV As Telerik.WinControls.UI.RadTileElement
End Class

