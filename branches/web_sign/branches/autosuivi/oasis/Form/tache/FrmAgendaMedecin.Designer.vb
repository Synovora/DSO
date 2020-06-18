<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()>
Partial Class FrmAgendaMedecin
    Inherits Telerik.WinControls.UI.RadForm

    'Form overrides dispose to clean up the component list.
    <System.Diagnostics.DebuggerNonUserCode()>
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
    <System.Diagnostics.DebuggerStepThrough()>
    Private Sub InitializeComponent()
        Dim SchedulerDailyPrintStyle1 As Telerik.WinControls.UI.SchedulerDailyPrintStyle = New Telerik.WinControls.UI.SchedulerDailyPrintStyle()
        Me.RadSchedulerNavigator1 = New Telerik.WinControls.UI.RadSchedulerNavigator()
        Me.RadScheduler1 = New Telerik.WinControls.UI.RadScheduler()
        Me.object_b6873913_5c99_4de7_92c5_bfac85dad221 = New Telerik.WinControls.RootRadElement()
        CType(Me.RadSchedulerNavigator1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me.RadScheduler1, System.ComponentModel.ISupportInitialize).BeginInit()
        CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
        Me.SuspendLayout()
        '
        'RadSchedulerNavigator1
        '
        Me.RadSchedulerNavigator1.AssociatedScheduler = Me.RadScheduler1
        Me.RadSchedulerNavigator1.DateFormat = "dd MMMM yyyy"
        Me.RadSchedulerNavigator1.Dock = System.Windows.Forms.DockStyle.Top
        Me.RadSchedulerNavigator1.Location = New System.Drawing.Point(0, 0)
        Me.RadSchedulerNavigator1.Name = "RadSchedulerNavigator1"
        Me.RadSchedulerNavigator1.NavigationStepType = Telerik.WinControls.UI.NavigationStepTypes.Day
        '
        '
        '
        Me.RadSchedulerNavigator1.RootElement.StretchVertically = False
        Me.RadSchedulerNavigator1.Size = New System.Drawing.Size(963, 77)
        Me.RadSchedulerNavigator1.TabIndex = 0
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadToggleButtonElement).ToggleState = Telerik.WinControls.Enumerations.ToggleState.[On]
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.RadToggleButtonElement).Text = "Jour"
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(1), Telerik.WinControls.UI.RadToggleButtonElement).Text = "Semaine"
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(2), Telerik.WinControls.UI.RadToggleButtonElement).Text = "Mois"
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(3), Telerik.WinControls.UI.RadToggleButtonElement).Text = "Temps"
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(4), Telerik.WinControls.UI.RadToggleButtonElement).Text = "Agenda"
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(5), Telerik.WinControls.UI.LightVisualElement).Visibility = Telerik.WinControls.ElementVisibility.Visible
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(5).GetChildAt(0), Telerik.WinControls.UI.RadCheckBoxElement).Text = "Voir Weekend"
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(6).GetChildAt(0), Telerik.WinControls.UI.RadLabelElement).Text = "(UTC+01:00) Bruxelles, Copenhague, Madrid, Paris"
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(6).GetChildAt(0), Telerik.WinControls.UI.RadLabelElement).Visibility = Telerik.WinControls.ElementVisibility.Hidden
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(6).GetChildAt(1), Telerik.WinControls.UI.RadDropDownListElement).Visibility = Telerik.WinControls.ElementVisibility.Hidden
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(6).GetChildAt(1).GetChildAt(2), Telerik.WinControls.UI.StackLayoutElement).Visibility = Telerik.WinControls.ElementVisibility.Hidden
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(6).GetChildAt(2), Telerik.WinControls.UI.SchedulerNavigatorSearchTextbox).ToolTipText = "Chercher dans les RDV"
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(6).GetChildAt(2), Telerik.WinControls.UI.SchedulerNavigatorSearchTextbox).Visibility = Telerik.WinControls.ElementVisibility.Visible
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(6).GetChildAt(2), Telerik.WinControls.UI.SchedulerNavigatorSearchTextbox).MaxSize = New System.Drawing.Size(250, 0)
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(6).GetChildAt(2).GetChildAt(0), Telerik.WinControls.Primitives.FillPrimitive).Visibility = Telerik.WinControls.ElementVisibility.Visible
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(6).GetChildAt(2).GetChildAt(2), Telerik.WinControls.Layouts.StackLayoutPanel).Visibility = Telerik.WinControls.ElementVisibility.Visible
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(6).GetChildAt(2).GetChildAt(2).GetChildAt(0), Telerik.WinControls.UI.LightVisualButtonElement).Visibility = Telerik.WinControls.ElementVisibility.Collapsed
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(6).GetChildAt(2).GetChildAt(3), Telerik.WinControls.Layouts.DockLayoutPanel).LastChildFill = True
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(0).GetChildAt(2).GetChildAt(6).GetChildAt(2).GetChildAt(3), Telerik.WinControls.Layouts.DockLayoutPanel).Visibility = Telerik.WinControls.ElementVisibility.Visible
        CType(Me.RadSchedulerNavigator1.GetChildAt(0).GetChildAt(2).GetChildAt(1).GetChildAt(2).GetChildAt(3).GetChildAt(0), Telerik.WinControls.UI.RadButtonElement).Text = "Aujourd'hui"
        '
        'RadScheduler1
        '
        Me.RadScheduler1.Culture = New System.Globalization.CultureInfo("fr-FR")
        Me.RadScheduler1.Dock = System.Windows.Forms.DockStyle.Fill
        Me.RadScheduler1.Location = New System.Drawing.Point(0, 77)
        Me.RadScheduler1.Name = "RadScheduler1"
        SchedulerDailyPrintStyle1.AppointmentFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        SchedulerDailyPrintStyle1.DateEndRange = New Date(2019, 11, 11, 0, 0, 0, 0)
        SchedulerDailyPrintStyle1.DateHeadingFont = New System.Drawing.Font("Microsoft Sans Serif", 8.25!, System.Drawing.FontStyle.Bold)
        SchedulerDailyPrintStyle1.DateStartRange = New Date(2019, 11, 6, 0, 0, 0, 0)
        SchedulerDailyPrintStyle1.PageHeadingFont = New System.Drawing.Font("Microsoft Sans Serif", 22.0!, System.Drawing.FontStyle.Bold)
        Me.RadScheduler1.PrintStyle = SchedulerDailyPrintStyle1
        Me.RadScheduler1.Size = New System.Drawing.Size(963, 455)
        Me.RadScheduler1.TabIndex = 1
        CType(Me.RadScheduler1.GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.DayViewHeader).Text = ""
        CType(Me.RadScheduler1.GetChildAt(0).GetChildAt(0).GetChildAt(0).GetChildAt(0), Telerik.WinControls.UI.SchedulerHeaderCellElement).Visibility = Telerik.WinControls.ElementVisibility.Visible
        '
        'object_b6873913_5c99_4de7_92c5_bfac85dad221
        '
        Me.object_b6873913_5c99_4de7_92c5_bfac85dad221.Name = "object_b6873913_5c99_4de7_92c5_bfac85dad221"
        Me.object_b6873913_5c99_4de7_92c5_bfac85dad221.StretchHorizontally = True
        Me.object_b6873913_5c99_4de7_92c5_bfac85dad221.StretchVertically = True
        '
        'FrmAgendaMedecin
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(963, 532)
        Me.Controls.Add(Me.RadScheduler1)
        Me.Controls.Add(Me.RadSchedulerNavigator1)
        Me.MinimizeBox = False
        Me.Name = "FrmAgendaMedecin"
        '
        '
        '
        Me.RootElement.ApplyShapeToControl = True
        Me.ShowIcon = False
        Me.ShowInTaskbar = False
        Me.Text = "FrmAgendaMedecin"
        CType(Me.RadSchedulerNavigator1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me.RadScheduler1, System.ComponentModel.ISupportInitialize).EndInit()
        CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub

    Friend WithEvents RadSchedulerNavigator1 As Telerik.WinControls.UI.RadSchedulerNavigator
    Friend WithEvents RadScheduler1 As Telerik.WinControls.UI.RadScheduler
    Friend WithEvents object_b6873913_5c99_4de7_92c5_bfac85dad221 As Telerik.WinControls.RootRadElement
End Class

