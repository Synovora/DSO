
Imports Telerik.WinControls
Imports Telerik.WinControls.UI

Public Class RadInputBox

    Shared Function Show(ByVal Prompt As String, ByVal Title As String, Optional ByVal DefaultResponse As String = "") As String
        Dim inputBox As New RadInputBoxInternal()
        inputBox.StartPosition = FormStartPosition.CenterParent
        inputBox.LabelQuestion.Text = Prompt
        inputBox.Text = Title

        If inputBox.ShowDialog() = DialogResult.OK Then
            Return inputBox.TextBoxInput.Text
        Else
            Return DefaultResponse
        End If
    End Function

    Private Class RadInputBoxInternal
        Inherits Telerik.WinControls.UI.RadForm

        Friend Sub New()
            Me.InitializeComponent()
        End Sub

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

        Private components As System.ComponentModel.IContainer

        <System.Diagnostics.DebuggerStepThrough()>
        Private Sub InitializeComponent()
            Me.ButtonOk = New Telerik.WinControls.UI.RadButton()
            Me.ButtonCancel = New Telerik.WinControls.UI.RadButton()
            Me.TextBoxInput = New Telerik.WinControls.UI.RadTextBox()
            Me.LabelQuestion = New Telerik.WinControls.UI.RadLabel()
            CType(Me.ButtonOk, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.ButtonCancel, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.TextBoxInput, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me.LabelQuestion, System.ComponentModel.ISupportInitialize).BeginInit()
            CType(Me, System.ComponentModel.ISupportInitialize).BeginInit()
            Me.SuspendLayout()
            '
            'ButtonOk
            '
            Me.ButtonOk.Location = New System.Drawing.Point(342, 12)
            Me.ButtonOk.Name = "ButtonOk"
            Me.ButtonOk.Size = New System.Drawing.Size(97, 24)
            Me.ButtonOk.TabIndex = 0
            Me.ButtonOk.Text = "Ok"
            '
            'ButtonCancel
            '
            Me.ButtonCancel.DialogResult = System.Windows.Forms.DialogResult.Cancel
            Me.ButtonCancel.Location = New System.Drawing.Point(342, 42)
            Me.ButtonCancel.Name = "ButtonCancel"
            Me.ButtonCancel.Size = New System.Drawing.Size(97, 24)
            Me.ButtonCancel.TabIndex = 1
            Me.ButtonCancel.Text = "Cancel"
            '
            'TextBoxInput
            '
            Me.TextBoxInput.Location = New System.Drawing.Point(13, 87)
            Me.TextBoxInput.Name = "TextBoxInput"
            Me.TextBoxInput.Size = New System.Drawing.Size(426, 20)
            Me.TextBoxInput.TabIndex = 2
            Me.TextBoxInput.TabStop = False
            '
            'LabelQuestion
            '
            Me.LabelQuestion.AutoSize = False
            Me.LabelQuestion.Location = New System.Drawing.Point(13, 12)
            Me.LabelQuestion.Name = "LabelQuestion"
            Me.LabelQuestion.Size = New System.Drawing.Size(323, 69)
            Me.LabelQuestion.TabIndex = 3
            Me.LabelQuestion.Text = "text"
            '
            'RadForm1
            '
            Me.AcceptButton = Me.ButtonOk
            Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
            Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
            Me.CancelButton = Me.ButtonCancel
            Me.ClientSize = New System.Drawing.Size(451, 119)
            Me.Controls.Add(Me.LabelQuestion)
            Me.Controls.Add(Me.TextBoxInput)
            Me.Controls.Add(Me.ButtonCancel)
            Me.Controls.Add(Me.ButtonOk)
            Me.FormBorderStyle = System.Windows.Forms.FormBorderStyle.FixedDialog
            Me.MaximizeBox = False
            Me.MinimizeBox = False
            Me.Name = "RadInputInternal"
            '
            '
            '
            Me.RootElement.ApplyShapeToControl = True
            Me.Text = "RadInputInternal"
            CType(Me.ButtonOk, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.ButtonCancel, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.TextBoxInput, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me.LabelQuestion, System.ComponentModel.ISupportInitialize).EndInit()
            CType(Me, System.ComponentModel.ISupportInitialize).EndInit()
            Me.ResumeLayout(False)
            Me.PerformLayout()

        End Sub
        Friend WithEvents ButtonOk As Telerik.WinControls.UI.RadButton
        Friend WithEvents ButtonCancel As Telerik.WinControls.UI.RadButton
        Friend WithEvents TextBoxInput As Telerik.WinControls.UI.RadTextBox
        Friend WithEvents LabelQuestion As Telerik.WinControls.UI.RadLabel

        Private Sub RadForm1_Shown(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles MyBase.Shown
            Me.TextBoxInput.SelectionLength = 0
            Me.TextBoxInput.Focus()
        End Sub

        Private Sub ButtonCancel_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonCancel.Click
            Me.DialogResult = Windows.Forms.DialogResult.Cancel
            Me.Close()
        End Sub

        Private Sub ButtonOk_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles ButtonOk.Click
            Me.DialogResult = DialogResult.OK
            Me.Close()
        End Sub

    End Class
End Class
