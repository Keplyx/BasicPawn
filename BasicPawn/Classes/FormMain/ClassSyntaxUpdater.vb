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


Public Class ClassSyntaxUpdater
    Private g_mFormMain As FormMain
    Private g_mSourceSyntaxUpdaterThread As Threading.Thread

    Public Sub New(f As FormMain)
        g_mFormMain = f
    End Sub

    ''' <summary>
    ''' Starts the updater thread
    ''' </summary>
    Public Sub StartThread()
        If (g_mSourceSyntaxUpdaterThread Is Nothing OrElse Not g_mSourceSyntaxUpdaterThread.IsAlive) Then
            g_mSourceSyntaxUpdaterThread = New Threading.Thread(AddressOf SourceSyntaxUpdater_Thread) With {
                .Priority = Threading.ThreadPriority.Lowest,
                .IsBackground = True
            }
            g_mSourceSyntaxUpdaterThread.Start()
        End If
    End Sub

    ''' <summary>
    ''' Stops the updater thread
    ''' </summary>
    Public Sub StopThread()
        If (g_mSourceSyntaxUpdaterThread IsNot Nothing AndAlso g_mSourceSyntaxUpdaterThread.IsAlive) Then
            g_mSourceSyntaxUpdaterThread.Abort()
            g_mSourceSyntaxUpdaterThread.Join()
            g_mSourceSyntaxUpdaterThread = Nothing
        End If
    End Sub

    ''' <summary>
    ''' The main thread to update all kinds of stuff.
    ''' </summary>
    Private Sub SourceSyntaxUpdater_Thread()
        Dim dLastFullAutocompleteUpdate As Date = Now
        Dim dLastVarAutocompleteUpdate As Date = Now
        Dim dLastMethodAutocompleteUpdate As Date = Now
        Dim dLastFoldingUpdate As Date = Now

        While True
            Threading.Thread.Sleep(500)

            Try
                'Update Autocomplete
                If (dLastFullAutocompleteUpdate < Now OrElse g_mFormMain.g_ClassAutocompleteUpdater.g_bForceFullAutocompleteUpdate) Then
                    dLastFullAutocompleteUpdate = (Now + New TimeSpan(0, 0, 1, 0, 0))

                    g_mFormMain.BeginInvoke(Sub() g_mFormMain.g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.ALL))
                End If

                'Update Variable Autocomplete
                If (dLastVarAutocompleteUpdate < Now) Then
                    dLastVarAutocompleteUpdate = (Now + New TimeSpan(0, 0, 0, 10, 0))

                    g_mFormMain.BeginInvoke(Sub() g_mFormMain.g_ClassAutocompleteUpdater.StartUpdate(ClassAutocompleteUpdater.ENUM_AUTOCOMPLETE_UPDATE_TYPE_FLAGS.VARIABLES_AUTOCOMPLETE))
                End If

                'Update method Autocomplete
                If (dLastMethodAutocompleteUpdate < Now) Then
                    dLastMethodAutocompleteUpdate = (Now + New TimeSpan(0, 0, 0, 10, 0))

                    Dim sTextContent As String = CStr(g_mFormMain.Invoke(Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.TextContent))
                    g_mFormMain.g_mSourceSyntaxSourceAnalysis = New ClassSyntaxTools.ClassSyntaxSourceAnalysis(sTextContent)
                End If

                'Update Foldings
                If (dLastFoldingUpdate < Now) Then
                    dLastFoldingUpdate = Now + New TimeSpan(0, 0, 5)

                    'If ((Tools.WordCount(Me.Invoke(Function() TextEditorControl1.Document.TextContent), "{") + Tools.WordCount(Me.Invoke(Function() TextEditorControl1.Document.TextContent), "}")) Mod 2 = 0) Then
                    g_mFormMain.BeginInvoke(Sub() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.Document.FoldingManager.UpdateFoldings(Nothing, Nothing))
                    'End If
                End If


                Dim iCaretOffset As Integer = CInt(g_mFormMain.Invoke(Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Offset))
                Dim iCaretPos As Integer = CInt(g_mFormMain.Invoke(Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.ScreenPosition.X +
                                                                       g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.ScreenPosition.Y))

                'Update Method Autoupdate 
                Static iLastMethodAutoupdateCaretOffset As Integer = -1
                If (iLastMethodAutoupdateCaretOffset <> iCaretOffset) Then
                    iLastMethodAutoupdateCaretOffset = iCaretOffset

                    If (Not g_mFormMain.g_mUCAutocomplete.ParseMethodAutocomplete(True)) Then
                        g_mFormMain.BeginInvoke(Sub()
                                                    g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.m_CurrentMethod = ""
                                                    g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
                                                End Sub)
                    End If
                End If

                'Update Autocomplete
                Static iLastAutoupdateCaretOffset As Integer = -1
                Static iLastAutoupdateCaretPos As Integer = -1
                If (iLastAutoupdateCaretPos <> iCaretPos) Then
                    iLastAutoupdateCaretPos = iCaretPos

                    If (iLastAutoupdateCaretOffset = iCaretOffset) Then
                        g_mFormMain.BeginInvoke(Sub()
                                                    g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete("")
                                                    g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.m_CurrentMethod = ""
                                                    g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip()
                                                End Sub)
                    End If

                    iLastAutoupdateCaretOffset = iCaretOffset
                End If

                'Update Autocomplete
                Static iLastAutoupdateCaretOffset2 As Integer = -1
                If (iLastAutoupdateCaretOffset2 <> iCaretOffset) Then
                    iLastAutoupdateCaretOffset2 = iCaretOffset


                    If (iCaretOffset > -1 AndAlso iCaretOffset < CInt(g_mFormMain.Invoke(Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Document.TextLength))) Then
                        Dim iPosition As Integer = CInt(g_mFormMain.Invoke(Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.TextArea.Caret.Position.Column))
                        Dim iLineOffset As Integer = CInt(g_mFormMain.Invoke(Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iCaretOffset).Offset))
                        Dim iLineLen As Integer = CInt(g_mFormMain.Invoke(Function() g_mFormMain.g_ClassTabControl.m_ActiveTab.m_TextEditor.ActiveTextAreaControl.Document.GetLineSegmentForOffset(iCaretOffset).Length))

                        If ((iLineLen - iPosition) > 0) Then
                            Dim sFunctionName As String = CType(g_mFormMain.Invoke(Function() g_mFormMain.g_ClassTextEditorTools.GetCaretWord(True)), String)

                            If (CInt(g_mFormMain.Invoke(Function() g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete(sFunctionName))) < 1) Then
                                sFunctionName = CStr(g_mFormMain.Invoke(Function() g_mFormMain.g_ClassTextEditorTools.GetCaretWord(False)))

                                g_mFormMain.Invoke(Sub() g_mFormMain.g_mUCAutocomplete.UpdateAutocomplete(sFunctionName))
                                g_mFormMain.Invoke(Sub() g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip())
                            Else
                                g_mFormMain.Invoke(Sub() g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip())
                            End If
                        Else
                            g_mFormMain.Invoke(Sub() g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip())
                        End If
                    Else
                        g_mFormMain.Invoke(Sub() g_mFormMain.g_mUCAutocomplete.g_ClassToolTip.UpdateToolTip())
                    End If
                End If

                'Update caret word maker
                Static iLastAutoupdateCaretOffset3 As Integer = -1
                If (iLastAutoupdateCaretOffset3 <> iCaretOffset) Then
                    iLastAutoupdateCaretOffset3 = iCaretOffset

                    If (ClassSettings.g_iSettingsAutoMark) Then
                        g_mFormMain.BeginInvoke(Sub() g_mFormMain.g_ClassTextEditorTools.MarkCaretWord())
                    End If
                End If

            Catch ex As Threading.ThreadAbortException
                Throw
            Catch ex As Exception
                ClassExceptionLog.WriteToLogMessageBox(ex)
                Threading.Thread.Sleep(5000)
            End Try
        End While
    End Sub

End Class
