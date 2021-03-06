﻿'BasicPawn
'Copyright(C) 2017 TheTimocop

'This program Is free software: you can redistribute it And/Or modify
'it under the terms Of the GNU General Public License As published by
'the Free Software Foundation, either version 3 Of the License, Or
'(at your option) any later version.

'This program Is distributed In the hope that it will be useful,
'but WITHOUT ANY WARRANTY; without even the implied warranty Of
'MERCHANTABILITY Or FITNESS FOR A PARTICULAR PURPOSE.  See the
'GNU General Public License For more details.

'You should have received a copy Of the GNU General Public License
'along with this program. If Not, see < http: //www.gnu.org/licenses/>.


Imports System.ComponentModel
Imports System.Text
Imports System.Text.RegularExpressions

Public Class FormMain
    Public g_ClassSyntaxUpdater As ClassSyntaxUpdater
    Public g_ClassSyntaxTools As ClassSyntaxTools
    Public g_ClassAutocompleteUpdater As ClassAutocompleteUpdater
    Public g_ClassTextEditorTools As ClassTextEditorTools
    Public g_ClassLineState As ClassTextEditorTools.ClassLineState
    Public g_ClassCustomHighlighting As ClassTextEditorTools.ClassCustomHighlighting
    Public g_ClassPluginController As ClassPluginController
    Public g_ClassTabControl As ClassTabControl
#Disable Warning IDE1006 ' Naming Styles
    Public WithEvents g_ClassCrossAppComunication As ClassCrossAppComunication
#Enable Warning IDE1006 ' Naming Styles

    Public g_mSourceSyntaxSourceAnalysis As ClassSyntaxTools.ClassSyntaxSourceAnalysis

    Public g_mUCAutocomplete As UCAutocomplete
    Public g_mUCInformationList As UCInformationList
    Public g_mUCObjectBrowser As UCObjectBrowser
    Public g_mUCToolTip As UCToolTip
    Public g_mFormDebugger As FormDebugger
    Public g_mFormOpenTabFromInstances As FormOpenTabFromInstances

    Public g_cDarkTextEditorBackgroundColor As Color = Color.FromArgb(255, 26, 26, 26)
    Public g_cDarkFormDetailsBackgroundColor As Color = Color.FromArgb(255, 24, 24, 24)
    Public g_cDarkFormBackgroundColor As Color = Color.FromArgb(255, 48, 48, 48)
    Public g_cDarkFormMenuBackgroundColor As Color = Color.FromArgb(255, 64, 64, 64)

    Public Const COMMSG_SERVERNAME As String = "BasicPawnComServer-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_OPEN_FILE_BY_PID As String = "BasicPawnComServer-OpenFileByPID-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_REQUEST_TABS As String = "BasicPawnComServer-RequestTabs-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_REQUEST_TABS_ANSWER As String = "BasicPawnComServer-RequestTabsAnswer-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_CLOSE_TAB As String = "BasicPawnComServer-CloseTab-04e3632f-5472-42c5-929a-c3e0c2b35324"
    Public Const COMARG_SHOW_PING_FLASH As String = "BasicPawnComServer-ShowPingFlash-04e3632f-5472-42c5-929a-c3e0c2b35324"

    Private g_mPingFlashPanel As ClassPanelAlpha

    Public Class STRUC_AUTOCOMPLETE
        Public sInfo As String
        Public sFile As String
        Public mType As ENUM_TYPE_FLAGS
        Public sFunctionName As String
        Public sFullFunctionName As String

        Enum ENUM_TYPE_FLAGS
            NONE = 0
            DEBUG = (1 << 0)
            DEFINE = (1 << 1)
            [ENUM] = (1 << 2)
            FUNCENUM = (1 << 3)
            FUNCTAG = (1 << 4)
            STOCK = (1 << 5)
            [STATIC] = (1 << 6)
            [CONST] = (1 << 7)
            [PUBLIC] = (1 << 8)
            NATIVE = (1 << 9)
            FORWARD = (1 << 10)
            TYPESET = (1 << 11)
            METHODMAP = (1 << 12)
            TYPEDEF = (1 << 13)
            VARIABLE = (1 << 14)
            PUBLICVAR = (1 << 15)
            [PROPERTY] = (1 << 16)
            [FUNCTION] = (1 << 17)
        End Enum

        Public Function ParseTypeFullNames(sStr As String) As ENUM_TYPE_FLAGS
            Return ParseTypeNames(sStr.Split(New String() {" "}, 0))
        End Function

        Public Function ParseTypeNames(sStr As String()) As ENUM_TYPE_FLAGS
            Dim mTypes As ENUM_TYPE_FLAGS = ENUM_TYPE_FLAGS.NONE

            For i = 0 To sStr.Length - 1
                Select Case (sStr(i))
                    Case "debug" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.DEBUG)
                    Case "define" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.DEFINE)
                    Case "enum" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.ENUM)
                    Case "funcenum" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FUNCENUM)
                    Case "functag" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FUNCTAG)
                    Case "stock" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.STOCK)
                    Case "static" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.STATIC)
                    Case "const" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.CONST)
                    Case "public" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.PUBLIC)
                    Case "native" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.NATIVE)
                    Case "forward" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FORWARD)
                    Case "typeset" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.TYPESET)
                    Case "methodmap" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.METHODMAP)
                    Case "typedef" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.TYPEDEF)
                    Case "variable" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.VARIABLE)
                    Case "publicvar" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.PUBLICVAR)
                    Case "property" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.PROPERTY)
                    Case "function" : mTypes = (mTypes Or ENUM_TYPE_FLAGS.FUNCTION)
                End Select
            Next

            Return mTypes
        End Function

        Public Function GetTypeNames() As String()
            Dim lNames As New List(Of String)

            If ((mType And ENUM_TYPE_FLAGS.DEBUG) = ENUM_TYPE_FLAGS.DEBUG) Then lNames.Add("debug")
            If ((mType And ENUM_TYPE_FLAGS.DEFINE) = ENUM_TYPE_FLAGS.DEFINE) Then lNames.Add("define")
            If ((mType And ENUM_TYPE_FLAGS.ENUM) = ENUM_TYPE_FLAGS.ENUM) Then lNames.Add("enum")
            If ((mType And ENUM_TYPE_FLAGS.FUNCENUM) = ENUM_TYPE_FLAGS.FUNCENUM) Then lNames.Add("funcenum")
            If ((mType And ENUM_TYPE_FLAGS.FUNCTAG) = ENUM_TYPE_FLAGS.FUNCTAG) Then lNames.Add("functag")
            If ((mType And ENUM_TYPE_FLAGS.STOCK) = ENUM_TYPE_FLAGS.STOCK) Then lNames.Add("stock")
            If ((mType And ENUM_TYPE_FLAGS.STATIC) = ENUM_TYPE_FLAGS.STATIC) Then lNames.Add("static")
            If ((mType And ENUM_TYPE_FLAGS.CONST) = ENUM_TYPE_FLAGS.CONST) Then lNames.Add("const")
            If ((mType And ENUM_TYPE_FLAGS.PUBLIC) = ENUM_TYPE_FLAGS.PUBLIC) Then lNames.Add("public")
            If ((mType And ENUM_TYPE_FLAGS.NATIVE) = ENUM_TYPE_FLAGS.NATIVE) Then lNames.Add("native")
            If ((mType And ENUM_TYPE_FLAGS.FORWARD) = ENUM_TYPE_FLAGS.FORWARD) Then lNames.Add("forward")
            If ((mType And ENUM_TYPE_FLAGS.TYPESET) = ENUM_TYPE_FLAGS.TYPESET) Then lNames.Add("typeset")
            If ((mType And ENUM_TYPE_FLAGS.METHODMAP) = ENUM_TYPE_FLAGS.METHODMAP) Then lNames.Add("methodmap")
            If ((mType And ENUM_TYPE_FLAGS.TYPEDEF) = ENUM_TYPE_FLAGS.TYPEDEF) Then lNames.Add("typedef")
            If ((mType And ENUM_TYPE_FLAGS.VARIABLE) = ENUM_TYPE_FLAGS.VARIABLE) Then lNames.Add("variable")
            If ((mType And ENUM_TYPE_FLAGS.PUBLICVAR) = ENUM_TYPE_FLAGS.PUBLICVAR) Then lNames.Add("publicvar")
            If ((mType And ENUM_TYPE_FLAGS.PROPERTY) = ENUM_TYPE_FLAGS.PROPERTY) Then lNames.Add("property")
            If ((mType And ENUM_TYPE_FLAGS.FUNCTION) = ENUM_TYPE_FLAGS.FUNCTION) Then lNames.Add("function")

            Return lNames.ToArray
        End Function

        Public Function GetTypeFullNames() As String
            Return String.Join(" ", GetTypeNames())
        End Function
    End Class



#Region "GUI Stuff"
    Public Sub New()
        ' This call is required by the designer.
        InitializeComponent()

        ' Add any initialization after the InitializeComponent() call. 

        g_ClassSyntaxUpdater = New ClassSyntaxUpdater(Me)
        g_ClassSyntaxTools = New ClassSyntaxTools(Me)
        g_ClassAutocompleteUpdater = New ClassAutocompleteUpdater(Me)
        g_ClassTextEditorTools = New ClassTextEditorTools(Me)
        g_ClassLineState = New ClassTextEditorTools.ClassLineState(Me)
        g_ClassCustomHighlighting = New ClassTextEditorTools.ClassCustomHighlighting(Me)
        g_ClassPluginController = New ClassPluginController(Me)
        g_ClassTabControl = New ClassTabControl(Me)
        g_ClassCrossAppComunication = New ClassCrossAppComunication

        ' Load other Forms/Controls
        g_mUCAutocomplete = New UCAutocomplete(Me) With {
            .Parent = TabPage_Autocomplete,
            .Dock = DockStyle.Fill
        }
        g_mUCAutocomplete.Show()

        g_mUCInformationList = New UCInformationList(Me) With {
            .Parent = TabPage_Information,
            .Dock = DockStyle.Fill
        }
        g_mUCInformationList.Show()

        g_mUCObjectBrowser = New UCObjectBrowser(Me) With {
            .Parent = TabPage_ObjectBrowser,
            .Dock = DockStyle.Fill
        }
        g_mUCObjectBrowser.Show()

        g_mUCToolTip = New UCToolTip(Me) With {
            .Parent = SplitContainer_ToolboxAndEditor.Panel2
        }
        g_mUCToolTip.BringToFront()
        g_mUCToolTip.Hide()

        SplitContainer_ToolboxSourceAndDetails.SplitterDistance = SplitContainer_ToolboxSourceAndDetails.Height - 175
        g_ClassCrossAppComunication.Hook(COMMSG_SERVERNAME)

        g_mPingFlashPanel = New ClassPanelAlpha
        Me.Controls.Add(g_mPingFlashPanel)
        g_mPingFlashPanel.Name = "#Ignore"
        g_mPingFlashPanel.Parent = Me
        g_mPingFlashPanel.Dock = DockStyle.Fill
        g_mPingFlashPanel.m_TransparentBackColor = Color.FromKnownColor(KnownColor.RoyalBlue)
        g_mPingFlashPanel.m_Opacity = 0
        g_mPingFlashPanel.BringToFront()
        g_mPingFlashPanel.Visible = False
    End Sub

    Public Sub UpdateFormConfigText()
        ToolStripStatusLabel_CurrentConfig.Text = "Config: " & ClassConfigs.m_ActiveConfig.GetName
    End Sub

    Public Sub PrintInformation(sType As String, sMessage As String, Optional bClear As Boolean = False, Optional bShowInformationTab As Boolean = False, Optional iLatestNoDuplicateLines As Integer = 0)
        Me.BeginInvoke(
            Sub()
                If (g_mUCInformationList Is Nothing) Then
                    Return
                End If

                Dim bExist As Boolean = False

                If (iLatestNoDuplicateLines > 0) Then
                    For Each item As String In g_mUCInformationList.ListBox_Information.Items
                        If (iLatestNoDuplicateLines < 1) Then
                            Exit For
                        End If

                        If (item.StartsWith(sType) AndAlso item.EndsWith(sMessage)) Then
                            bExist = True
                            Exit For
                        End If

                        iLatestNoDuplicateLines -= 1
                    Next
                End If

                If (bClear) Then
                    g_mUCInformationList.ListBox_Information.Items.Clear()
                End If

                If (Not bExist) Then
                    g_mUCInformationList.ListBox_Information.Items.Insert(0, String.Format("{0} ({1}) {2}", sType, Now.ToString, sMessage))
                End If

                ToolStripStatusLabel_LastInformation.Text = sMessage

                If (bShowInformationTab) Then
                    SplitContainer_ToolboxSourceAndDetails.Panel2Collapsed = False
                    SplitContainer_ToolboxSourceAndDetails.SplitterDistance = SplitContainer_ToolboxSourceAndDetails.Height - 200
                    TabControl_Details.SelectTab(1)
                End If
            End Sub)
    End Sub

#End Region

#Region "Syntax Stuff"


    Private Sub FormMain_Load(sender As Object, e As EventArgs) Handles MyBase.Load
        Dim sWineVersion As String = ClassTools.ClassOperatingSystem.GetWineVersion()

        ToolStripStatusLabel_AppVersion.Text = String.Format("v.{0} {1}", Application.ProductVersion, If(sWineVersion Is Nothing, "", "| Running on Wine " & sWineVersion)).Trim

        'Some control init
        ToolStripComboBox_ToolsAutocompleteSyntax.SelectedIndex = 0

        'Load Settings 
        ClassSettings.LoadSettings()

        'Load default configs
        For Each mConfig As ClassConfigs.STRUC_CONFIG_ITEM In ClassConfigs.GetConfigs(False)
            If (mConfig.g_bAutoload) Then
                ClassConfigs.m_ActiveConfig = mConfig
                UpdateFormConfigText()
                Exit For
            End If
        Next

        'Clean tabs
        g_ClassTabControl.Init()

        'Load source files via Arguments
        Dim sArgs As String() = Environment.GetCommandLineArgs

        While True
            Dim lFileList As New List(Of String)
            For i = 1 To sArgs.Length - 1
                If (IO.File.Exists(sArgs(i))) Then
                    lFileList.Add(sArgs(i))
                End If
            Next

            If (lFileList.Count < 1) Then
                Exit While
            End If



            'Open all files in the oldes BasicPawn instance
            If (Not ClassSettings.g_iSettingsAlwaysOpenNewInstance AndAlso Array.IndexOf(sArgs, "-newinstance") = -1) Then
                Dim pBasicPawnProc As Process() = Process.GetProcessesByName(IO.Path.GetFileNameWithoutExtension(Application.ExecutablePath))
                If (pBasicPawnProc.Length > 0) Then
                    Dim iCurrentPID As Integer = Process.GetCurrentProcess.Id
                    Dim iMyTick As Long = Process.GetCurrentProcess.StartTime.Ticks
                    Dim iLastTick As Long = Date.MinValue.Ticks
                    Dim pLastProcess As Process = Nothing

                    For Each pProcess As Process In pBasicPawnProc
                        Try
                            If (pProcess.Id = iCurrentPID OrElse pProcess.MainModule.FileName.ToLower <> Application.ExecutablePath.ToLower) Then
                                Continue For
                            End If

                            If (iMyTick < pProcess.StartTime.Ticks) Then
                                Continue For
                            End If

                            If (iLastTick > Date.MinValue.Ticks AndAlso iLastTick < pProcess.StartTime.Ticks) Then
                                Continue For
                            End If

                            pLastProcess = pProcess
                            iLastTick = pProcess.StartTime.Ticks
                        Catch ex As Exception
                            'Ignore random exceptions
                        End Try
                    Next


                    'If (pLastProcess IsNot Nothing AndAlso MessageBox.Show("Open in a existing BasicPawn instance?", "Open files", MessageBoxButtons.YesNo, MessageBoxIcon.Question) = DialogResult.Yes) Then
                    If (pLastProcess IsNot Nothing) Then
                        Try
                            For i = 0 To lFileList.Count - 1
                                Dim mMsg As New ClassCrossAppComunication.ClassMessage(COMARG_OPEN_FILE_BY_PID, CStr(pLastProcess.Id), lFileList(i))
                                g_ClassCrossAppComunication.SendMessage(mMsg)
                            Next
                        Catch ex As Exception
                            ClassExceptionLog.WriteToLogMessageBox(ex)
                        End Try

                        Me.WindowState = FormWindowState.Minimized
                        Me.ShowInTaskbar = False
                        Application.Exit()
                    End If
                End If
            End If

            'Open all files here
            For i = 0 To lFileList.Count - 1
                g_ClassTabControl.AddTab(False)
                g_ClassTabControl.OpenFileTab(g_ClassTabControl.m_TabsCount - 1, lFileList(i), True)

                If (i = 0 AndAlso g_ClassTabControl.m_TabsCount > 0) Then
                    g_ClassTabControl.RemoveTab(0, False)
                End If
            Next
            Exit While
        End While

        'Update Autocomplete
        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)

        'UpdateTextEditorControl1Colors()
        g_ClassSyntaxTools.UpdateFormColors()

        g_ClassSyntaxUpdater.StartThread()

        g_ClassPluginController.LoadPlugins(IO.Path.Combine(Application.StartupPath, "plugins"))
        g_ClassPluginController.PluginsExecute(Sub(j As BasicPawnPluginInterface.IPluginInterface) j.OnPluginStart(Me))
    End Sub

#End Region


#Region "Open/Save/Dialog"
    Private Sub FormMain_Closing(sender As Object, e As CancelEventArgs) Handles Me.Closing
        g_ClassPluginController.PluginsExecute(Sub(j As BasicPawnPluginInterface.IPluginInterface)
                                                   If (Not j.OnPluginEnd()) Then
                                                       e.Cancel = True
                                                   End If
                                               End Sub)

        For i = 0 To g_ClassTabControl.m_TabsCount - 1
            If (g_ClassTabControl.PromptSaveTab(i)) Then
                e.Cancel = True
            End If
        Next
    End Sub

    Private Sub ContextMenuStrip1_Opening(sender As Object, e As CancelEventArgs) Handles ContextMenuStrip_RightClick.Opening
        g_mUCAutocomplete.UpdateAutocomplete("")
        g_mUCAutocomplete.g_ClassToolTip.m_CurrentMethod = ""
        g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
    End Sub
#End Region

#Region "ContextMenuStrip"
    Private Sub ToolStripMenuItem_Mark_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Mark.Click
        g_ClassTextEditorTools.MarkSelectedWord()
    End Sub

    Private Sub ToolStripMenuItem_Cut_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Cut.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Cut(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_Copy_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Copy.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Copy(sender, e)
    End Sub

    Private Sub ToolStripMenuItem_Paste_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Paste.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.ClipboardHandler.Paste(sender, e)
    End Sub
#End Region

#Region "MenuStrip"

#Region "MenuStrip_File"
    Private Sub ToolStripMenuItem_FileNew_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileNew.Click
        g_ClassTabControl.AddTab(True, True, False)

        PrintInformation("[INFO]", "User created a new source file")
    End Sub

    Private Sub ToolStripMenuItem_FileOpen_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileOpen.Click
        Using i As New OpenFileDialog
            i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|Pawn (Not fully supported)|*.pwn;*.p|AMX Mod X|*.sma|All files|*.*"
            i.FileName = g_ClassTabControl.m_ActiveTab.m_File
            i.Multiselect = True

            If (i.ShowDialog = DialogResult.OK) Then
                For Each sFile As String In i.FileNames
                    g_ClassTabControl.AddTab(True)
                    g_ClassTabControl.OpenFileTab(g_ClassTabControl.m_TabsCount - 1, sFile)
                Next
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem_FileSave_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSave.Click
        g_ClassTabControl.SaveFileTab(g_ClassTabControl.m_ActiveTabIndex)
    End Sub

    Private Sub ToolStripMenuItem_FileSaveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSaveAll.Click
        For i = 0 To g_ClassTabControl.m_TabsCount - 1
            g_ClassTabControl.SaveFileTab(i)
        Next
    End Sub

    Private Sub ToolStripMenuItem_FileSaveAs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSaveAs.Click
        g_ClassTabControl.SaveFileTab(g_ClassTabControl.m_ActiveTabIndex, True)
    End Sub

    Private Sub ToolStripMenuItem_FileSaveAsTemp_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSaveAsTemp.Click
        Dim sTempFile As String = String.Format("{0}.src", IO.Path.Combine(IO.Path.GetTempPath, Guid.NewGuid.ToString))
        IO.File.WriteAllText(sTempFile, "")

        g_ClassTabControl.m_ActiveTab.m_File = sTempFile
        g_ClassTabControl.SaveFileTab(g_ClassTabControl.m_ActiveTabIndex)
    End Sub

    Private Sub ToolStripMenuItem_FileSavePacked_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileSavePacked.Click
        Try
            Dim sTempFile As String = ""
            Dim sLstSource As String = g_ClassTextEditorTools.GetCompilerPreProcessCode(True, True, sTempFile)
            If (String.IsNullOrEmpty(sLstSource)) Then
                MessageBox.Show("Could not export packed source. See information tab for more information.", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)

                Return
            End If

            If (String.IsNullOrEmpty(sTempFile)) Then
                Throw New ArgumentException("Last Pre-Process source invalid")
            End If

            With New ClassDebuggerRunner.ClassPreProcess(Nothing)
                .FixPreProcessFiles(sLstSource)
            End With

            'Replace the temp source file with the currently opened one
            sLstSource = Regex.Replace(sLstSource,
                                       String.Format("^\s*\#file ""{0}""\s*$", Regex.Escape(sTempFile)),
                                       String.Format("#file ""{0}""", g_ClassTabControl.m_ActiveTab.m_File),
                                       RegexOptions.IgnoreCase Or RegexOptions.Multiline)

            Using i As New SaveFileDialog
                i.Filter = "All supported files|*.sp;*.inc;*.sma|SourcePawn|*.sp|Include|*.inc|Pawn (Not fully supported)|*.pwn;*.p|AMX Mod X|*.sma|All files|*.*"
                i.FileName = IO.Path.Combine(IO.Path.GetDirectoryName(g_ClassTabControl.m_ActiveTab.m_File), IO.Path.GetFileNameWithoutExtension(g_ClassTabControl.m_ActiveTab.m_File) & ".packed" & IO.Path.GetExtension(g_ClassTabControl.m_ActiveTab.m_File))

                If (i.ShowDialog = DialogResult.OK) Then
                    IO.File.WriteAllText(i.FileName, sLstSource)
                End If
            End Using
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileLoadTabs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileLoadTabs.Click
        If (g_mFormOpenTabFromInstances Is Nothing OrElse g_mFormOpenTabFromInstances.IsDisposed) Then
            g_mFormOpenTabFromInstances = New FormOpenTabFromInstances(Me)
            g_mFormOpenTabFromInstances.Show()
        End If
    End Sub

    Private Sub ToolStripMenuItem_FileOpenFolder_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileOpenFolder.Click
        Try
            If (String.IsNullOrEmpty(g_ClassTabControl.m_ActiveTab.m_File) OrElse Not IO.File.Exists(g_ClassTabControl.m_ActiveTab.m_File)) Then
                MessageBox.Show("Can't open current folder. Source file can't be found!", "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
                Return
            End If

            Process.Start("explorer.exe", "/select,""" & g_ClassTabControl.m_ActiveTab.m_File & """")
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_FileExit_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_FileExit.Click
        Me.Close()
    End Sub
#End Region

#Region "MenuStrip_Tools"
    Private Sub ToolStripMenuItem_ToolsSettingsAndConfigs_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsSettingsAndConfigs.Click
        Using i As New FormSettings(Me)
            If (i.ShowDialog() = DialogResult.OK) Then
                UpdateFormConfigText()

                g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)

                For j = 0 To g_ClassTabControl.m_TabsCount - 1
                    g_ClassTabControl.m_Tab(j).m_TextEditor.ActiveTextAreaControl.TextEditorProperties.Font = ClassSettings.g_iSettingsTextEditorFont
                    g_ClassTabControl.m_Tab(j).m_TextEditor.Refresh()
                Next

                g_ClassSyntaxTools.UpdateFormColors()
            End If
        End Using
    End Sub

    Private Sub ToolStripMenuItem_ToolsFormatCode_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsFormatCode.Click
        Try
            If (Not g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
                MessageBox.Show("Nothing selected to format!", "Unable to format", MessageBoxButtons.OK, MessageBoxIcon.Information)
                Return
            End If

            Dim sFormatedSource As String = g_ClassSyntaxTools.FormatCode(g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent)
            Dim lFormatedSourceLines As New List(Of String)
            Using mStringReader As New IO.StringReader(sFormatedSource)
                Dim sLine As String
                While True
                    sLine = mStringReader.ReadLine
                    If (sLine Is Nothing) Then
                        Exit While
                    End If

                    lFormatedSourceLines.Add(sLine)
                End While
            End Using

            If (g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TotalNumberOfLines - 1 <> lFormatedSourceLines.Count) Then
                Throw New ArgumentException("Formated number of lines are not equal with document number of lines!")
            End If

            Try
                g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.StartUndoGroup()

                For i = lFormatedSourceLines.Count - 1 To 0 Step -1
                    Dim mLineSeg = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetLineSegment(i)
                    Dim sLine As String = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.GetText(mLineSeg.Offset, mLineSeg.Length)

                    If (Not g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.IsSelected(mLineSeg.Offset) AndAlso
                            Not g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.IsSelected(mLineSeg.Offset + mLineSeg.Length)) Then
                        Continue For
                    End If

                    If (sLine = lFormatedSourceLines(i)) Then
                        Continue For
                    End If

                    g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Remove(mLineSeg.Offset, mLineSeg.Length)
                    g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.Insert(mLineSeg.Offset, lFormatedSourceLines(i))
                Next
            Finally
                g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.UndoStack.EndUndoGroup()
                g_ClassTabControl.m_ActiveTab.m_TextEditor.Refresh()
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_ToolsSearchReplace_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsSearchReplace.Click
        If (g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.HasSomethingSelected) Then
            g_ClassTextEditorTools.ShowSearchAndReplace(g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.SelectionManager.SelectedText)
        Else
            g_ClassTextEditorTools.ShowSearchAndReplace("")
        End If
    End Sub

    Private Sub ToolStripMenuItem_ToolsShowInformation_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsShowInformation.Click
        SplitContainer_ToolboxSourceAndDetails.Panel2Collapsed = False
        SplitContainer_ToolboxSourceAndDetails.SplitterDistance = SplitContainer_ToolboxSourceAndDetails.Height - 200
        TabControl_Details.SelectTab(1)
    End Sub

    Private Sub ToolStripMenuItem_ToolsClearInformationLog_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsClearInformationLog.Click
        PrintInformation("[INFO]", "Information log cleaned!", True)
    End Sub

    Private Sub ToolStripMenuItem_ToolsAutocompleteUpdate_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsAutocompleteUpdate.Click
        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)
    End Sub

    Private Sub ToolStripComboBox_ToolsAutocompleteSyntax_SelectedIndexChanged(sender As Object, e As EventArgs) Handles ToolStripComboBox_ToolsAutocompleteSyntax.SelectedIndexChanged
        Select Case (ToolStripComboBox_ToolsAutocompleteSyntax.SelectedIndex)
            Case 0
                ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_MIX
            Case 1
                ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_6
            Case 2
                ClassSettings.g_iSettingsAutocompleteSyntax = ClassSettings.ENUM_AUTOCOMPLETE_SYNTAX.SP_1_7
        End Select

        g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)
    End Sub

    Private Sub ToolStripMenuItem_ToolsAutocompleteShowAutocomplete_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ToolsAutocompleteShowAutocomplete.Click
        SplitContainer_ToolboxSourceAndDetails.Panel2Collapsed = False
        SplitContainer_ToolboxSourceAndDetails.SplitterDistance = SplitContainer_ToolboxSourceAndDetails.Height - 200
        TabControl_Details.SelectTab(0)
    End Sub

    Private Sub ToolStripMenuItem_ListReferences_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_ListReferences.Click
        g_ClassTextEditorTools.ListReferences()
    End Sub
#End Region

#Region "MenuStrip_Build"
    Private Sub ToolStripMenuItem_Build_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Build.Click
        With New ClassDebuggerParser(Me)
            If (.HasDebugPlaceholder(g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent)) Then
                Select Case (MessageBox.Show("All BasicPawn Debugger placeholders need to be removed before compiling the source. Remove all placeholder?", "Warning", MessageBoxButtons.OKCancel, MessageBoxIcon.Exclamation))
                    Case DialogResult.OK
                        .CleanupDebugPlaceholder(Me)
                    Case Else
                        Return
                End Select
            End If
        End With

        g_ClassTextEditorTools.CompileSource(False)
    End Sub
#End Region

#Region "MenuStrip_Test"
    Private Sub ToolStripMenuItem_Test_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Test.Click
        Dim sSource As String = g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent
        With New ClassDebuggerParser(Me)
            If (.HasDebugPlaceholder(sSource)) Then
                .CleanupDebugPlaceholder(sSource)
            End If
        End With

        Dim sSourceFile As String = Nothing
        If (Not String.IsNullOrEmpty(g_ClassTabControl.m_ActiveTab.m_File) AndAlso IO.File.Exists(g_ClassTabControl.m_ActiveTab.m_File)) Then
            sSourceFile = g_ClassTabControl.m_ActiveTab.m_File
        End If

        Dim sOutput As String = ""
        g_ClassTextEditorTools.CompileSource(True, sSource, sOutput, IO.Path.GetDirectoryName(sSourceFile), Nothing, Nothing, sSourceFile)
    End Sub
#End Region

#Region "MenuStrip_Debug"
    Private Sub ToolStripMenuItem_Debug_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Debug.Click
        If (g_mFormDebugger Is Nothing OrElse g_mFormDebugger.IsDisposed) Then
            g_mFormDebugger = New FormDebugger(Me)
            g_mFormDebugger.Show()
        Else
            g_mFormDebugger.BringToFront()
        End If
    End Sub
#End Region

#Region "MenuStrip_Shell"
    Private Sub ToolStripMenuItem_Shell_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Shell.Click
        Try
            Dim sShell As String = ClassConfigs.m_ActiveConfig.g_sExecuteShell

            For Each shellModule In ClassSettings.GetShellArguments(Me)
                sShell = sShell.Replace(shellModule.g_sMarker, shellModule.g_sArgument)
            Next

            Try
                If (String.IsNullOrEmpty(sShell)) Then
                    Throw New ArgumentException("Shell is empty")
                End If

                Shell(sShell, AppWinStyle.NormalFocus)
            Catch ex As Exception
                MessageBox.Show(ex.Message & Environment.NewLine & Environment.NewLine & sShell, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
            End Try
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub
#End Region

#Region "MenuStrip_Help"
    Private Sub ToolStripMenuItem_HelpAbout_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_HelpAbout.Click
        Dim SB As New StringBuilder
        SB.AppendLine(String.Format("{0} v.{1}", Application.ProductName, Application.ProductVersion))
        SB.AppendLine("Created by Externet (aka Timocop)")
        SB.AppendLine()
        SB.AppendLine("Source and Releases")
        SB.AppendLine("     https://github.com/Timocop/BasicPawn")
        SB.AppendLine()
        SB.AppendLine("Third-Party tools:")
        SB.AppendLine("     SharpDevelop - TextEditor (LGPL-2.1)")
        SB.AppendLine()
        SB.AppendLine("         Authors:")
        SB.AppendLine("         Daniel Grunwald and SharpDevelop Community")
        SB.AppendLine("         https://github.com/icsharpcode/SharpDevelop")
        SB.AppendLine()
        SB.AppendLine("     SSH.NET (MIT)")
        SB.AppendLine()
        SB.AppendLine("         Authors:")
        SB.AppendLine("         Gert Driesen and Community")
        SB.AppendLine("         https://github.com/sshnet/SSH.NET")
        MessageBox.Show(SB.ToString, "About", MessageBoxButtons.OK, MessageBoxIcon.Information)
    End Sub
#End Region

#Region "MenuStrip_Undo"
    Private Sub ToolStripMenuItem_Undo_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Undo.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.Undo()
    End Sub
#End Region

#Region "MenuStrip_Redo"
    Private Sub ToolStripMenuItem_Redo_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_Redo.Click
        g_ClassTabControl.m_ActiveTab.m_TextEditor.Redo()
    End Sub
#End Region


    Private Sub ToolStripStatusLabel_CurrentConfig_Click(sender As Object, e As EventArgs) Handles ToolStripStatusLabel_CurrentConfig.Click
        Using i As New FormSettings(Me)
            i.TabControl1.SelectTab(1)
            If (i.ShowDialog() = DialogResult.OK) Then
                UpdateFormConfigText()

                g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL)

                For j = 0 To g_ClassTabControl.m_TabsCount - 1
                    g_ClassTabControl.m_Tab(j).m_TextEditor.ActiveTextAreaControl.TextEditorProperties.Font = ClassSettings.g_iSettingsTextEditorFont
                    g_ClassTabControl.m_Tab(j).m_TextEditor.Refresh()
                Next

                g_ClassSyntaxTools.UpdateFormColors()
            End If
        End Using
    End Sub

#End Region


    Private Sub ToolStripMenuItem_DebuggerBreakpointInsert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointInsert.Click
        With New ClassDebuggerParser.ClassBreakpoints(Me)
            .TextEditorInsertBreakpointAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerBreakpointRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointRemove.Click
        With New ClassDebuggerParser.ClassBreakpoints(Me)
            .TextEditorRemoveBreakpointAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerBreakpointRemoveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerBreakpointRemoveAll.Click
        With New ClassDebuggerParser.ClassBreakpoints(Me)
            .TextEditorRemoveAllBreakpoints()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherInsert_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherInsert.Click
        With New ClassDebuggerParser.ClassWatchers(Me)
            .TextEditorInsertWatcherAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherRemove_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherRemove.Click
        With New ClassDebuggerParser.ClassWatchers(Me)
            .TextEditorRemoveWatcherAtCaret()
        End With
    End Sub

    Private Sub ToolStripMenuItem_DebuggerWatcherRemoveAll_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_DebuggerWatcherRemoveAll.Click
        With New ClassDebuggerParser.ClassWatchers(Me)
            .TextEditorRemoveAllWatchers()
        End With
    End Sub


    Private Sub FormMain_FormClosing(sender As Object, e As FormClosingEventArgs) Handles Me.FormClosing
        If (g_mFormDebugger IsNot Nothing AndAlso Not g_mFormDebugger.IsDisposed) Then
            g_mFormDebugger.Close()
        End If

        If (g_mFormDebugger IsNot Nothing AndAlso Not g_mFormDebugger.IsDisposed) Then
            MessageBox.Show("You can't close BasicPawn while debugging!", "Warning", MessageBoxButtons.OK, MessageBoxIcon.Exclamation)
            e.Cancel = True
        End If
    End Sub

    Private Sub ToolStripMenuItem_CheckUpdate_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_CheckUpdate.Click
        Try
            Process.Start("https://github.com/Timocop/BasicPawn/releases")
        Catch ex As Exception
            MessageBox.Show(ex.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        End Try
    End Sub

    Private Sub ToolStripMenuItem_TabClose_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TabClose.Click
        g_ClassTabControl.RemoveTab(g_ClassTabControl.m_ActiveTabIndex, True)
    End Sub

    Private Sub ToolStripMenuItem_TabMoveRight_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TabMoveRight.Click
        Dim iActiveIndex As Integer = g_ClassTabControl.m_ActiveTabIndex
        Dim iToIndex As Integer = iActiveIndex + 1

        If (iToIndex > g_ClassTabControl.m_TabsCount - 1) Then
            Return
        End If

        g_ClassTabControl.SwapTabs(iActiveIndex, iToIndex)
    End Sub

    Private Sub ToolStripMenuItem_TabMoveLeft_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TabMoveLeft.Click
        Dim iActiveIndex As Integer = g_ClassTabControl.m_ActiveTabIndex
        Dim iToIndex As Integer = iActiveIndex - 1

        If (iToIndex < 0) Then
            Return
        End If

        g_ClassTabControl.SwapTabs(iActiveIndex, iToIndex)
    End Sub

    Private Sub ToolStripMenuItem_TabOpenInstance_Click(sender As Object, e As EventArgs) Handles ToolStripMenuItem_TabOpenInstance.Click
        If (g_mFormOpenTabFromInstances Is Nothing OrElse g_mFormOpenTabFromInstances.IsDisposed) Then
            g_mFormOpenTabFromInstances = New FormOpenTabFromInstances(Me)
            g_mFormOpenTabFromInstances.Show()
        End If
    End Sub

    Private Sub OnMessageReceive(mClassMessage As ClassCrossAppComunication.ClassMessage) Handles g_ClassCrossAppComunication.OnMessageReceive
        Try
            Select Case (mClassMessage.m_MessageName)
                Case COMARG_OPEN_FILE_BY_PID
                    Dim iPID As Integer = CInt(mClassMessage.m_Messages(0))
                    Dim sFile As String = mClassMessage.m_Messages(1)

                    If (iPID <> Process.GetCurrentProcess.Id) Then
                        Return
                    End If

                    Me.BeginInvoke(Sub()
                                       g_ClassTabControl.AddTab(True)
                                       g_ClassTabControl.OpenFileTab(g_ClassTabControl.m_TabsCount - 1, sFile)
                                   End Sub)

                Case COMARG_REQUEST_TABS
                    Dim sIdentifier As String = mClassMessage.m_Messages(0)

                    For i = 0 To g_ClassTabControl.m_TabsCount - 1
                        Dim iTabIndex As Integer = i
                        Dim sProcessName As String = Process.GetCurrentProcess.ProcessName
                        Dim iPID As Integer = Process.GetCurrentProcess.Id
                        Dim sFile As String = g_ClassTabControl.m_Tab(i).m_File

                        g_ClassCrossAppComunication.SendMessage(New ClassCrossAppComunication.ClassMessage(COMARG_REQUEST_TABS_ANSWER, CStr(iTabIndex), sProcessName, CStr(iPID), sFile, sIdentifier), False)
                    Next

                Case COMARG_REQUEST_TABS_ANSWER
                    Dim iTabIndex As Integer = CInt(mClassMessage.m_Messages(0))
                    Dim sProcessName As String = mClassMessage.m_Messages(1)
                    Dim iPID As Integer = CInt(mClassMessage.m_Messages(2))
                    Dim sFile As String = mClassMessage.m_Messages(3)
                    Dim sIdentifier As String = mClassMessage.m_Messages(4)

                    If (g_mFormOpenTabFromInstances Is Nothing OrElse g_mFormOpenTabFromInstances.IsDisposed) Then
                        Return
                    End If

                    If (g_mFormOpenTabFromInstances IsNot Nothing) Then
                        g_mFormOpenTabFromInstances.AddListViewItem(iTabIndex, sProcessName, iPID, sFile, sIdentifier)
                    End If

                Case COMARG_CLOSE_TAB
                    Dim iTabIndex As Integer = CInt(mClassMessage.m_Messages(0))
                    Dim iPID As Integer = CInt(mClassMessage.m_Messages(1))
                    Dim sFile As String = mClassMessage.m_Messages(2)

                    If (iPID <> Process.GetCurrentProcess.Id) Then
                        Return
                    End If

                    If (iTabIndex < 0 OrElse iTabIndex > g_ClassTabControl.m_TabsCount - 1) Then
                        Return
                    End If

                    If (Not String.IsNullOrEmpty(sFile) AndAlso sFile <> g_ClassTabControl.m_Tab(iTabIndex).m_File) Then
                        Return
                    End If

                    g_ClassTabControl.RemoveTab(iTabIndex, True)

                Case COMARG_SHOW_PING_FLASH
                    Dim iPID As Integer = CInt(mClassMessage.m_Messages(0))

                    If (iPID <> Process.GetCurrentProcess.Id) Then
                        Return
                    End If

                    ShowPingFlash()
            End Select
        Catch ex As Exception
            ClassExceptionLog.WriteToLogMessageBox(ex)
        End Try
    End Sub

    Public Sub ShowPingFlash()
        g_mPingFlashPanel.m_Opacity = 50
        g_mPingFlashPanel.Visible = True

        Timer_PingFlash.Start()
    End Sub

    Private Sub Timer_PingFlash_Tick(sender As Object, e As EventArgs) Handles Timer_PingFlash.Tick
        If (g_mPingFlashPanel Is Nothing OrElse g_mPingFlashPanel.IsDisposed) Then
            Return
        End If

        g_mPingFlashPanel.m_Opacity -= 10

        If (g_mPingFlashPanel.m_Opacity > 0) Then
            Return
        End If

        g_mPingFlashPanel.Visible = False
        Timer_PingFlash.Stop()
    End Sub
End Class
