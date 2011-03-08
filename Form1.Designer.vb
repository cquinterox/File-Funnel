<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class frmMain
    Inherits System.Windows.Forms.Form

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
        Me.fbdSource = New System.Windows.Forms.FolderBrowserDialog()
        Me.btnSource = New System.Windows.Forms.Button()
        Me.btnDestination = New System.Windows.Forms.Button()
        Me.btnCopy = New System.Windows.Forms.Button()
        Me.txtExtension = New System.Windows.Forms.TextBox()
        Me.Label1 = New System.Windows.Forms.Label()
        Me.fbdDestination = New System.Windows.Forms.FolderBrowserDialog()
        Me.chkKeepDirStruc = New System.Windows.Forms.CheckBox()
        Me.LinkLabel1 = New System.Windows.Forms.LinkLabel()
        Me.SuspendLayout()
        '
        'fbdSource
        '
        Me.fbdSource.ShowNewFolderButton = False
        '
        'btnSource
        '
        Me.btnSource.Location = New System.Drawing.Point(17, 12)
        Me.btnSource.Name = "btnSource"
        Me.btnSource.Size = New System.Drawing.Size(85, 31)
        Me.btnSource.TabIndex = 0
        Me.btnSource.Text = "Source"
        Me.btnSource.UseVisualStyleBackColor = True
        '
        'btnDestination
        '
        Me.btnDestination.Location = New System.Drawing.Point(108, 12)
        Me.btnDestination.Name = "btnDestination"
        Me.btnDestination.Size = New System.Drawing.Size(85, 31)
        Me.btnDestination.TabIndex = 1
        Me.btnDestination.Text = "Destination"
        Me.btnDestination.UseVisualStyleBackColor = True
        '
        'btnCopy
        '
        Me.btnCopy.Location = New System.Drawing.Point(17, 112)
        Me.btnCopy.Name = "btnCopy"
        Me.btnCopy.Size = New System.Drawing.Size(173, 31)
        Me.btnCopy.TabIndex = 4
        Me.btnCopy.Text = "Copy"
        Me.btnCopy.UseVisualStyleBackColor = True
        '
        'txtExtension
        '
        Me.txtExtension.Location = New System.Drawing.Point(115, 54)
        Me.txtExtension.Name = "txtExtension"
        Me.txtExtension.Size = New System.Drawing.Size(73, 20)
        Me.txtExtension.TabIndex = 5
        '
        'Label1
        '
        Me.Label1.AutoSize = True
        Me.Label1.Location = New System.Drawing.Point(14, 57)
        Me.Label1.Name = "Label1"
        Me.Label1.Size = New System.Drawing.Size(97, 13)
        Me.Label1.TabIndex = 6
        Me.Label1.Text = "Extension to copy: "
        '
        'chkKeepDirStruc
        '
        Me.chkKeepDirStruc.AutoSize = True
        Me.chkKeepDirStruc.Location = New System.Drawing.Point(19, 84)
        Me.chkKeepDirStruc.Name = "chkKeepDirStruc"
        Me.chkKeepDirStruc.Size = New System.Drawing.Size(138, 17)
        Me.chkKeepDirStruc.TabIndex = 8
        Me.chkKeepDirStruc.Text = "Keep directory structure"
        Me.chkKeepDirStruc.UseVisualStyleBackColor = True
        '
        'LinkLabel1
        '
        Me.LinkLabel1.AutoSize = True
        Me.LinkLabel1.LinkBehavior = System.Windows.Forms.LinkBehavior.HoverUnderline
        Me.LinkLabel1.LinkColor = System.Drawing.Color.Gray
        Me.LinkLabel1.Location = New System.Drawing.Point(99, 159)
        Me.LinkLabel1.Name = "LinkLabel1"
        Me.LinkLabel1.Size = New System.Drawing.Size(92, 13)
        Me.LinkLabel1.TabIndex = 9
        Me.LinkLabel1.TabStop = True
        Me.LinkLabel1.Text = "C. Quinteros 2011"
        Me.LinkLabel1.VisitedLinkColor = System.Drawing.Color.Gray
        '
        'frmMain
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(6.0!, 13.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font
        Me.ClientSize = New System.Drawing.Size(209, 187)
        Me.Controls.Add(Me.LinkLabel1)
        Me.Controls.Add(Me.chkKeepDirStruc)
        Me.Controls.Add(Me.Label1)
        Me.Controls.Add(Me.txtExtension)
        Me.Controls.Add(Me.btnCopy)
        Me.Controls.Add(Me.btnDestination)
        Me.Controls.Add(Me.btnSource)
        Me.MaximumSize = New System.Drawing.Size(225, 225)
        Me.MinimumSize = New System.Drawing.Size(225, 225)
        Me.Name = "frmMain"
        Me.Text = "File Funnel "
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents fbdSource As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents btnSource As System.Windows.Forms.Button
    Friend WithEvents btnDestination As System.Windows.Forms.Button
    Friend WithEvents btnCopy As System.Windows.Forms.Button
    Friend WithEvents txtExtension As System.Windows.Forms.TextBox
    Friend WithEvents Label1 As System.Windows.Forms.Label
    Friend WithEvents fbdDestination As System.Windows.Forms.FolderBrowserDialog
    Friend WithEvents chkKeepDirStruc As System.Windows.Forms.CheckBox
    Friend WithEvents LinkLabel1 As System.Windows.Forms.LinkLabel

End Class
