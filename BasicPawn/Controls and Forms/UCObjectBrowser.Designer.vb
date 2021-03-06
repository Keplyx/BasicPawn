﻿<Global.Microsoft.VisualBasic.CompilerServices.DesignerGenerated()> _
Partial Class UCObjectBrowser
    Inherits System.Windows.Forms.UserControl

    'UserControl überschreibt den Löschvorgang, um die Komponentenliste zu bereinigen.
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

    'Wird vom Windows Form-Designer benötigt.
    Private components As System.ComponentModel.IContainer

    'Hinweis: Die folgende Prozedur ist für den Windows Form-Designer erforderlich.
    'Das Bearbeiten ist mit dem Windows Form-Designer möglich.  
    'Das Bearbeiten mit dem Code-Editor ist nicht möglich.
    <System.Diagnostics.DebuggerStepThrough()> _
    Private Sub InitializeComponent()
        Me.components = New System.ComponentModel.Container()
        Me.TreeView_ObjectBrowser = New ClassTreeViewWine()
        Me.ContextMenuStrip_ObjectBrowser = New System.Windows.Forms.ContextMenuStrip(Me.components)
        Me.ToolStripMenuItem_OpenFile = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripMenuItem_Copy = New System.Windows.Forms.ToolStripMenuItem()
        Me.ToolStripSeparator1 = New System.Windows.Forms.ToolStripSeparator()
        Me.ToolStripMenuItem_ListReferences = New System.Windows.Forms.ToolStripMenuItem()
        Me.TextboxWatermark_Search = New BasicPawn.ClassTextboxWatermark()
        Me.ContextMenuStrip_ObjectBrowser.SuspendLayout()
        Me.SuspendLayout()
        '
        'TreeView_ObjectBrowser
        '
        Me.TreeView_ObjectBrowser.BackColor = System.Drawing.Color.White
        Me.TreeView_ObjectBrowser.BorderStyle = System.Windows.Forms.BorderStyle.None
        Me.TreeView_ObjectBrowser.ContextMenuStrip = Me.ContextMenuStrip_ObjectBrowser
        Me.TreeView_ObjectBrowser.Dock = System.Windows.Forms.DockStyle.Fill
        Me.TreeView_ObjectBrowser.ForeColor = System.Drawing.Color.Black
        Me.TreeView_ObjectBrowser.HideSelection = False
        Me.TreeView_ObjectBrowser.Location = New System.Drawing.Point(0, 22)
        Me.TreeView_ObjectBrowser.Name = "TreeView_ObjectBrowser"
        Me.TreeView_ObjectBrowser.Size = New System.Drawing.Size(269, 634)
        Me.TreeView_ObjectBrowser.TabIndex = 1
        '
        'ContextMenuStrip_ObjectBrowser
        '
        Me.ContextMenuStrip_ObjectBrowser.Items.AddRange(New System.Windows.Forms.ToolStripItem() {Me.ToolStripMenuItem_OpenFile, Me.ToolStripMenuItem_Copy, Me.ToolStripSeparator1, Me.ToolStripMenuItem_ListReferences})
        Me.ContextMenuStrip_ObjectBrowser.Name = "ContextMenuStrip_ObjectBrowser"
        Me.ContextMenuStrip_ObjectBrowser.RenderMode = System.Windows.Forms.ToolStripRenderMode.System
        Me.ContextMenuStrip_ObjectBrowser.ShowImageMargin = False
        Me.ContextMenuStrip_ObjectBrowser.Size = New System.Drawing.Size(128, 98)
        '
        'ToolStripMenuItem_OpenFile
        '
        Me.ToolStripMenuItem_OpenFile.Name = "ToolStripMenuItem_OpenFile"
        Me.ToolStripMenuItem_OpenFile.Size = New System.Drawing.Size(127, 22)
        Me.ToolStripMenuItem_OpenFile.Text = "Open file"
        '
        'ToolStripMenuItem_Copy
        '
        Me.ToolStripMenuItem_Copy.Name = "ToolStripMenuItem_Copy"
        Me.ToolStripMenuItem_Copy.Size = New System.Drawing.Size(127, 22)
        Me.ToolStripMenuItem_Copy.Text = "Copy"
        '
        'ToolStripSeparator1
        '
        Me.ToolStripSeparator1.Name = "ToolStripSeparator1"
        Me.ToolStripSeparator1.Size = New System.Drawing.Size(124, 6)
        '
        'ToolStripMenuItem_ListReferences
        '
        Me.ToolStripMenuItem_ListReferences.Name = "ToolStripMenuItem_ListReferences"
        Me.ToolStripMenuItem_ListReferences.Size = New System.Drawing.Size(127, 22)
        Me.ToolStripMenuItem_ListReferences.Text = "List references"
        '
        'TextboxWatermark_Search
        '
        Me.TextboxWatermark_Search.Dock = System.Windows.Forms.DockStyle.Top
        Me.TextboxWatermark_Search.Location = New System.Drawing.Point(0, 0)
        Me.TextboxWatermark_Search.m_sWatermarkText = "Search..."
        Me.TextboxWatermark_Search.Name = "TextboxWatermark_Search"
        Me.TextboxWatermark_Search.Size = New System.Drawing.Size(269, 22)
        Me.TextboxWatermark_Search.TabIndex = 2
        Me.TextboxWatermark_Search.Text = "Search..."
        '
        'UCObjectBrowser
        '
        Me.AutoScaleDimensions = New System.Drawing.SizeF(96.0!, 96.0!)
        Me.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Dpi
        Me.BackColor = System.Drawing.Color.White
        Me.Controls.Add(Me.TreeView_ObjectBrowser)
        Me.Controls.Add(Me.TextboxWatermark_Search)
        Me.Font = New System.Drawing.Font("Segoe UI", 8.25!, System.Drawing.FontStyle.Regular, System.Drawing.GraphicsUnit.Point, CType(0, Byte))
        Me.Name = "UCObjectBrowser"
        Me.Size = New System.Drawing.Size(269, 656)
        Me.ContextMenuStrip_ObjectBrowser.ResumeLayout(False)
        Me.ResumeLayout(False)
        Me.PerformLayout()

    End Sub
    Friend WithEvents TreeView_ObjectBrowser As ClassTreeViewWine
    Friend WithEvents TextboxWatermark_Search As ClassTextboxWatermark
    Friend WithEvents ContextMenuStrip_ObjectBrowser As ContextMenuStrip
    Friend WithEvents ToolStripMenuItem_OpenFile As ToolStripMenuItem
    Friend WithEvents ToolStripSeparator1 As ToolStripSeparator
    Friend WithEvents ToolStripMenuItem_ListReferences As ToolStripMenuItem
    Friend WithEvents ToolStripMenuItem_Copy As ToolStripMenuItem
End Class
