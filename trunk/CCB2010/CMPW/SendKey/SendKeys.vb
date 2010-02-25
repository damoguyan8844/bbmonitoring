Option Strict On
Option Explicit On
Imports System.Runtime.InteropServices
Imports System.Windows.Forms

Public Module SendKeys

#Region "Window"
    Const ASFW_ANY As Int32 = -1
    Const GW_HWNDNEXT As Int32 = 2
    Const GA_ROOT As Int32 = 2
    Const KEYEVENTF_KEYUP As Int32 = 2
    Const LSFW_LOCK As Int32 = 1
    Const LSFW_UNLOCK As Int32 = 2
    Const NEGATIVE As Int32 = -1
    Const NULL As Int32 = 0
    Const QS_ALLQUEUE As Int32 = 511
    Const SW_HIDE As Int32 = 0
    Const SW_SHOWNORMAL As Int32 = 1
    Const SW_SHOWMINIMIZED As Int32 = 2
    Const VK_MENU As Int32 = 18
    Private Structure GUITHREADINFO
        Public cbSize, flags, hWndActive, hWndFocus, hWndCapture, hWndMenuOwner, hWndMoveSize, hWndCaret As Int32, rcCaret As RECT
    End Structure
    Private Structure POINTAPI
        Public X, Y As Int32
    End Structure
    Private Structure RECT
        Public rLeft, rTop, rRight, rBottom As Int32
    End Structure
    Private Structure WINNAME
        Public lpText, lpClass, lpProcessName As String
    End Structure
    Private Structure WINSTATE
        Public IsIconic, IsHidden, IsDisabled, IsChildHidden, IsChildDisabled As Boolean
    End Structure
    Private Declare Function apiAllowSetForegroundWindow Lib "user32" Alias "AllowSetForegroundWindow" (ByVal dwProcessId As Int32) As Boolean
    Private Declare Function apiChildWindowFromPointEx Lib "user32" Alias "ChildWindowFromPointEx" (ByVal hWndParent As Int32, ByVal ptx As Int32, ByVal pty As Int32, ByVal uFlags As Int32) As Int32
    Private Declare Function apiEnableWindow Lib "user32" Alias "EnableWindow" (ByVal hWnd As Int32, ByVal fEnable As Boolean) As Boolean
    Private Declare Function apiFindWindow Lib "user32" Alias "FindWindowA" (ByVal lpClassName As String, ByVal lpWindowName As String) As Int32
    Private Declare Function apiFindWindowEx Lib "user32" Alias "FindWindowExA" (ByVal hWnd1 As Int32, ByVal hWnd2 As Int32, ByVal lpsz1 As String, ByVal lpsz2 As String) As Int32
    Private Declare Function apiGetActiveWindow Lib "user32" Alias "GetActiveWindow" () As Int32
    Private Declare Function apiGetAncestor Lib "user32" Alias "GetAncestor" (ByVal hWnd As Int32, ByVal gaFlags As Int32) As Int32
    Private Declare Function apiGetClassName Lib "user32" Alias "GetClassNameA" (ByVal hWnd As Int32, ByVal lpClassName As String, ByVal nMaxCount As Int32) As Int32
    Private Declare Function apiGetCursorPos Lib "user32" Alias "GetCursorPos" (ByRef lpPoint As POINTAPI) As Boolean
    Private Declare Function apiGetDesktopWindow Lib "user32" Alias "GetDesktopWindow" () As Int32
    Private Declare Function apiGetForegroundWindow Lib "user32" Alias "GetForegroundWindow" () As Int32
    Private Declare Function apiGetGUIThreadInfo Lib "user32" Alias "GetGUIThreadInfo" (ByVal dwThreadId As Int32, ByRef lpGUIThreadInfo As GUITHREADINFO) As Int32
    Private Declare Function apiGetParent Lib "user32" Alias "GetParent" (ByVal hWnd As Int32) As Int32
    Private Declare Function apiGetQueueStatus Lib "user32" Alias "GetQueueStatus" (ByVal fuFlags As Int32) As Int32
    Private Declare Function apiGetTopWindow Lib "user32" Alias "GetTopWindow" (ByVal hWnd As Int32) As Int32
    Private Declare Function apiGetWindow Lib "user32" Alias "GetWindow" (ByVal hWnd As Int32, ByVal wCmd As Int32) As Int32
    Private Declare Function apiGetWindowText Lib "user32" Alias "GetWindowTextA" (ByVal hWnd As Int32, ByVal lpString As String, ByVal cch As Int32) As Int32
    Private Declare Function apiGetWindowTextLength Lib "user32" Alias "GetWindowTextLengthA" (ByVal hWnd As Int32) As Int32
    Private Declare Function apiGetWindowThreadProcessId Lib "user32" Alias "GetWindowThreadProcessId" (ByVal hWnd As Int32, ByRef lpdwProcessId As Int32) As Int32
    Private Declare Function apiIsIconic Lib "user32" Alias "IsIconic" (ByVal hWnd As Int32) As Boolean
    Private Declare Function apiIsWindow Lib "user32" Alias "IsWindow" (ByVal hWnd As Int32) As Boolean
    Private Declare Function apiIsWindowEnabled Lib "user32" Alias "IsWindowEnabled" (ByVal hWnd As Int32) As Boolean
    Private Declare Function apiIsWindowVisible Lib "user32" Alias "IsWindowVisible" (ByVal hWnd As Int32) As Boolean
    Private Declare Function apikeybd_event Lib "user32" Alias "keybd_event" (ByVal vKey As Int32, ByVal bScan As Int32, ByVal dwFlags As Int32, ByVal dwExtraInfo As Int32) As Boolean
    Private Declare Function apiLockSetForegroundWindow Lib "user32" Alias "LockSetForegroundWindow" (ByVal uLockCode As Int32) As Boolean
    Private Declare Function apiPostMessage Lib "user32" Alias "PostMessageA" (ByVal hWnd As Int32, ByVal wMsg As Int32, ByVal wParam As Int32, ByVal lParam As String) As Int32
    Private Declare Function apiSendMessage Lib "user32" Alias "SendMessageA" (ByVal hWnd As Int32, ByVal wMsg As Int32, ByVal wParam As Int32, ByVal lParam As String) As Int32
    Private Declare Function apiSetFocus Lib "user32" Alias "SetFocus" (ByVal hWnd As Int32) As Int32
    Private Declare Function apiSetForegroundWindow Lib "user32" Alias "SetForegroundWindow" (ByVal hWnd As Int32) As Int32
    Private Declare Function apiShowWindow Lib "user32" Alias "ShowWindow" (ByVal hWnd As Int32, ByVal nCmdShow As Int32) As Boolean
    Private Declare Function apiSwitchToThread Lib "kernel32" Alias "SwitchToThread" () As Int32
    Private Declare Function apiWaitForInputIdle Lib "user32" Alias "WaitForInputIdle" (ByVal hProcess As Int32, ByVal dwMilliseconds As Int32) As Int32
    Private Declare Function apiWindowFromPoint Lib "user32" Alias "WindowFromPoint" (ByVal xPoint As Int32, ByVal yPoint As Int32) As Int32

    ''' <summary>
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' A group of integer values containing the handle of the main window, and the handle of the focus window.
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Use in the second parameter of the following functions: Send, Text, Message and Click.
    ''' </summary>
    ''' <remarks></remarks>
    Public Structure WINFOCUS
        Public Foreground, Focus As Int32
    End Structure

    ''' <summary>
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Processes any keyboard or mouse messages currently in the queue.
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Returns true is there are no messages in the queue to be processed.
    ''' </summary>
    ''' <remarks></remarks>
    Public Function Flush() As Boolean
        If apiGetQueueStatus(QS_ALLQUEUE) <> 0 Then System.Windows.Forms.SendKeys.Flush() 'Process any messages in the queue
        Flush = Not CBool(apiGetQueueStatus(QS_ALLQUEUE)) 'Set return value to current state
    End Function

    ''' <summary>
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Gets the available key commands, or window commands.
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Use to get parameters for the Send, or GetWinHandles function.
    ''' This sub has no return value, it displays the contents in a notepad.
    ''' </summary>
    ''' <remarks></remarks>
    Public Sub GetCommands()
        Dim o As New System.Windows.Forms.Keys
        FileOpen(1, System.Environment.CurrentDirectory & "\Commands.txt", OpenMode.Output, OpenAccess.Write) 'Open file for writing
        PrintLine(1, "Applies to the first and second parameter(wName1 and wIndex1) of the GetWinHandles method.") 'Print info
        PrintLine(1, "{Command}         Description") 'Show key legend
        PrintLine(1, "") '''''''''''''''''''''''''''''Print blank line
        PrintLine(1, "{Active}" & "          Gets the active window") 'Print some lines
        PrintLine(1, "{Desktop}" & "         Gets the desktop window")
        PrintLine(1, "{Focus}" & "           Gets the window with focus")
        PrintLine(1, "{Foreground}" & "      Gets the foreground window")
        PrintLine(1, "{Frompoint}" & "       Gets the window under the cursor")
        PrintLine(1, "{Ancestor}" & "        Gets the ancestor window.  Use with wIndex parameter.")
        PrintLine(1, "{Childfrompoint}" & "  Gets the child window under the cursor.  Use with wIndex parameter.")
        PrintLine(1, "{Parent}" & "          Gets the parent window.  Use with wIndex parameter.")
        PrintLine(1, "{Top}" & "             Gets the top window.  Use with wIndex parameter.")
        PrintLine(1, "") '''''''''''''''''''''''''''''Print a blank line
        PrintLine(1, "") '''''''''''''''''''''''''''''Print a blank line
        PrintLine(1, "Applies to the first parameter(cText) of the Send method.")
        PrintLine(1, "KeyCode" & "  " & "{Command}") 'Show key legend of what's below
        PrintLine(1, "") '''''''''''''''''''''''''''''Print a blank line
        For n As Int32 = 0 To 255 ''''''''''''''''''''Loop through all possible keys
            o = CType(n, System.Windows.Forms.Keys) ''Set the nth key
            PrintLine(1, System.String.Concat(n.ToString, " ", "{" & o.ToString & "}")) 'Print the nth key with it's numerical key code
        Next '''''''''''''''''''''''''''''''''''''''''Next key
        FileClose(1) '''''''''''''''''''''''''''''''''Close file
        If System.IO.File.Exists(System.Environment.CurrentDirectory & "\Commands.txt") = True Then 'If file now exists
            System.Diagnostics.Process.Start(System.Environment.CurrentDirectory & "\Commands.txt") 'Then start it
        Else '''''''''''''''''''''''''''''''''''''''''If no file found
            MessageBox.Show("Could not print the file", "File error", MessageBoxButtons.OK, MessageBoxIcon.Error) 'Signal error
        End If
    End Sub

    ''' <summary>
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Gets the handle of the current foreground window and the handle of the child window with keyboard focus.
    ''' If keyFocus is False then the handles retrieved are the windows under the cursor with mouse focus.
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Returns the WINFOCUS structure containing the current focus handles. Depends on the keyFocus parameter.
    ''' </summary>
    ''' <param name="showNames">(Optional) Shows the title and class name of the windows,
    ''' to be used with the GetWinHandles function.</param>
    ''' <param name="keyFocus">(Optional) Returns the handle of the main window, and child window with keyboard focus.
    ''' If false then the function returns the handles of the windows currently under the cursor.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWinFocus(Optional ByVal showNames As Boolean = False, Optional ByVal keyFocus As Boolean = True) As WINFOCUS
        If keyFocus = True Then ''''''''''''''''''''''Keyboard focus
            Dim g As New GUITHREADINFO '''''''''''''''Dimension a thread input structure
            g.cbSize = System.Runtime.InteropServices.Marshal.SizeOf(g) 'Initialize structure
            apiGetGUIThreadInfo(apiGetWindowThreadProcessId(NULL, 0), g) 'Retrieve information about the active window
            GetWinFocus.Foreground = g.hWndActive ''''Set handle of active foreground
            GetWinFocus.Focus = g.hWndFocus ''''''''''Set handle of focus window
        Else '''''''''''''''''''''''''''''''''''''''''Mouse focus instead
            Dim p As New POINTAPI ''''''''''''''''''''Dimension a point for the mouse position
            apiGetCursorPos(p) '''''''''''''''''''''''Get current cursor position
            GetWinFocus.Focus = apiWindowFromPoint(p.X, p.Y) 'Get handle of window under cursor
            GetWinFocus.Foreground = apiGetAncestor(GetWinFocus.Focus, GA_ROOT) 'Try to get it's ancestor
            If GetWinFocus.Foreground = 0 Then GetWinFocus.Foreground = GetWinFocus.Focus 'If no ancestor then set main focus to child focus
        End If
        If showNames = True Then GetWinAncestory(GetWinFocus.Focus, True) 'Set ancestory using focus window
    End Function

    ''' <summary>
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Gets the handle of the main window and the handle of the child window that is to recieve
    ''' keyboard or mouse focus, by using the specified title, class name, or index.
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Returns the WINFOCUS structure containing the specified focus handles.
    ''' </summary>
    ''' <param name="wName1">The main window title or class name.</param>
    ''' <param name="wIndex1">(Optional) The main window index.</param>
    ''' <param name="wName2">(Optional) The first child window.</param>
    ''' <param name="wIndex2">(Optional) The first child index.</param>
    ''' <param name="wName3">(Optional) The second child window.</param>
    ''' <param name="wIndex3">(Optional) The second child index.</param>
    ''' <param name="wName4">(Optional) The third child window.</param>
    ''' <param name="wIndex4">(Optional) The third child index.</param>
    ''' <param name="wName5">(Optional) The fourth child window.</param>
    ''' <param name="wIndex5">(Optional) The fourth child index.</param>
    ''' <param name="wName6">(Optional) The fifth child window.</param>
    ''' <param name="wIndex6">(Optional) The fifth child index.</param>
    ''' <param name="wName7">(Optional) The sixth child window.</param>
    ''' <param name="wIndex7">(Optional) The sixth child index.</param>
    ''' <param name="wName8">(Optional) The seventh child window.</param>
    ''' <param name="wIndex8">(Optional) The seventh child index.</param>
    ''' <param name="wName9">(Optional) The eighth child window.</param>
    ''' <param name="wIndex9">(Optional) The eighth child index.</param>
    ''' <param name="wName10">(Optional) The ninth child window.</param>
    ''' <param name="wIndex10">(Optional) The ninth child index.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function GetWinHandles(ByVal wName1 As String, Optional ByVal wIndex1 As Int32 = 1, Optional ByVal wName2 As String = " ", Optional ByVal wIndex2 As Int32 = 1, Optional ByVal wName3 As String = " ", Optional ByVal wIndex3 As Int32 = 1, Optional ByVal wName4 As String = " ", Optional ByVal wIndex4 As Int32 = 1, Optional ByVal wName5 As String = " ", Optional ByVal wIndex5 As Int32 = 1, Optional ByVal wName6 As String = " ", Optional ByVal wIndex6 As Int32 = 1, Optional ByVal wName7 As String = " ", Optional ByVal wIndex7 As Int32 = 1, Optional ByVal wName8 As String = " ", Optional ByVal wIndex8 As Int32 = 1, Optional ByVal wName9 As String = " ", Optional ByVal wIndex9 As Int32 = 1, Optional ByVal wName10 As String = " ", Optional ByVal wIndex10 As Int32 = 1) As WINFOCUS
        Dim hwnd, cwnd, i As Int32, wn As String, wNames(1) As String, wF As New WINFOCUS
        If wName1 <> " " Then i = 1 : ReDim wNames(1) 'Set i to index of parameters
        If wName2 <> " " Then i = 3 : ReDim wNames(3)
        If wName3 <> " " Then i = 5 : ReDim wNames(5)
        If wName4 <> " " Then i = 7 : ReDim wNames(7)
        If wName5 <> " " Then i = 9 : ReDim wNames(9)
        If wName6 <> " " Then i = 11 : ReDim wNames(11)
        If wName7 <> " " Then i = 13 : ReDim wNames(13)
        If wName8 <> " " Then i = 15 : ReDim wNames(15)
        If wName9 <> " " Then i = 17 : ReDim wNames(17)
        If wName10 <> " " Then i = 19 : ReDim wNames(19)
        If wName1 <> " " Then wNames(0) = wName1 : wNames(1) = wIndex1.ToString 'Set array elements
        If wName2 <> " " Then wNames(2) = wName2 : wNames(3) = wIndex2.ToString
        If wName3 <> " " Then wNames(4) = wName3 : wNames(5) = wIndex3.ToString
        If wName4 <> " " Then wNames(6) = wName4 : wNames(7) = wIndex4.ToString
        If wName5 <> " " Then wNames(8) = wName5 : wNames(9) = wIndex5.ToString
        If wName6 <> " " Then wNames(10) = wName6 : wNames(11) = wIndex6.ToString
        If wName7 <> " " Then wNames(12) = wName7 : wNames(13) = wIndex7.ToString
        If wName8 <> " " Then wNames(14) = wName8 : wNames(15) = wIndex8.ToString
        If wName9 <> " " Then wNames(16) = wName9 : wNames(17) = wIndex9.ToString
        If wName10 <> " " Then wNames(18) = wName10 : wNames(19) = wIndex10.ToString
        hwnd = apiFindWindow(Nothing, wName1) ''''''''Look for handle from title
        If hwnd = 0 Then hwnd = apiFindWindow(wName1, Nothing) 'If not found by title, then look for handle from class name
        If hwnd <> 0 AndAlso CInt(wNames(1)) > 1 Then  'If searching for window by index, ie more than 1
            Dim nxtwnd As Int32 = hwnd '''''''''''''''Initialize handle containing the next window in the top level z-order
            Dim Index As Int32 = 1 '''''''''''''''''''Initialize index to 1, since handle was found.
            Dim n As New WINNAME '''''''''''''''''''''Create structure for window names
            Do '''''''''''''''''''''''''''''''''''''''Do loop to look for handles matching paramaters
                nxtwnd = apiGetWindow(hwnd, GW_HWNDNEXT) 'Set next window
                If nxtwnd = 0 Then Exit Do '''''''''''Eject if there are no more top level windows
                If wNames(0) <> "" Then ''''''''''''''If title or class name was given
                    n = GetWinName(nxtwnd, True, True) 'Get title and classname of the window
                    If n.lpText = wNames(0) OrElse n.lpClass = wNames(0) Then 'If title or class name matches first wNames parameter
                        Index += 1 '''''''''''''''''''Increment Index for the matching window 
                        hwnd = nxtwnd ''''''''''''''''Set handle to the handle of the matching window
                        If Index >= CInt(wNames(1)) Then Exit Do 'If the specified index has been reached then exit with last matching handle
                    End If
                Else '''''''''''''''''''''''''''''''''Then "" indicates the search is by index only
                    Index += 1 '''''''''''''''''''''''Increment Index for the matching window 
                    hwnd = nxtwnd ''''''''''''''''''''Set handle to the handle of the matching window
                    If Index >= CInt(wNames(1)) Then Exit Do 'If index specified has been reached then exit with last matching handle
                End If
            Loop
        End If
        If hwnd = 0 Then '''''''''''''''''''''''''''''If not found by title or class name, then look for special commands
            wn = wName1.ToLower ''''''''''''''''''''''Does not need to be case sensitive
            If wn = "{focus}" Then '''''''''''''''''''If focus window is specified
                hwnd = GetWinFocus.Focus '''''''''''''Get the handle of the focus window
            ElseIf wn = "{foreground}" Then ''''''''''If foreground window is specified
                hwnd = apiGetForegroundWindow ''''''''Get the handle of the foreground window
            ElseIf wn = "{active}" Then ''''''''''''''If active window is specified
                hwnd = apiGetActiveWindow ''''''''''''Get the handle of the active window
            ElseIf wn = "{desktop}" Then '''''''''''''If desktop window is specified
                hwnd = apiGetDesktopWindow() '''''''''Get the handle of the desktop window
            ElseIf wn = "{top}" Then '''''''''''''''''If top window is specified
                hwnd = apiGetTopWindow(hwnd) '''''''''Get the handle of the top window
            ElseIf wn = "{ancestor}" Then ''''''''''''If ancestor window is specified
                hwnd = apiGetAncestor(wIndex1, GA_ROOT) 'Get the handle of the ancestor window
            ElseIf wn = "{parent}" Then ''''''''''''''If parent window is specified
                hwnd = apiGetParent(wIndex1) '''''''''Get the handle of the parent window
            ElseIf wn = "{ancestor}" Then ''''''''''''If ancestor window is specified
                hwnd = apiGetAncestor(wIndex1, GA_ROOT) 'Get the handle of the ancestor window
            ElseIf wn = "{frompoint}" Then '''''''''''If window under the cursor is specified
                Dim p As New POINTAPI ''''''''''''''''Dimension a point structure
                If wIndex1 <> 1 AndAlso wIndex2 <> 1 Then 'If point specified
                    p.X = wIndex1 ''''''''''''''''''''If x specified
                    p.Y = wIndex2 ''''''''''''''''''''If y specified
                Else
                    apiGetCursorPos(p) '''''''''''''''Get cursor position as POINTAPI
                End If
                hwnd = apiWindowFromPoint(p.X, p.Y) ''Get the handle of the window under the cursor
            ElseIf wn = "{childfrompoint}" Then ''''''If child window under the cursor is specified
                Dim p As New POINTAPI ''''''''''''''''Dimension a point structure
                If wIndex2 <> 1 AndAlso wIndex3 <> 1 Then 'If point specified
                    p.X = wIndex2 ''''''''''''''''''''If x specified
                    p.Y = wIndex3 ''''''''''''''''''''If y specified
                Else
                    apiGetCursorPos(p) '''''''''''''''Get cursor position as POINTAPI
                End If
                hwnd = apiChildWindowFromPointEx(wIndex1, p.X, p.Y, wIndex2) 'Get the handle of the child window under the cursor
            ElseIf wName1.IndexOf(".exe") <> NEGATIVE Then 'If process name specified
                Dim pIndex As Int32 = 0 ''''''''''''''Initialize process index in case there are more than one matching that name
                For Each p As System.Diagnostics.Process In System.Diagnostics.Process.GetProcessesByName(wName1.Substring(0, wName1.Length - 4)) 'Loop through processes matching that name
                    pIndex += 1 ''''''''''''''''''''''Increment index by one each time found
                    If pIndex = wIndex1 Then hwnd = p.MainWindowHandle.ToInt32 : Exit For 'If index matches index specified then set handle and exit loop now
                Next '''''''''''''''''''''''''''''''''Next process
            End If
        End If
        If hwnd = 0 Then '''''''''''''''''''''''''''''If handle still not found
            If System.IO.File.Exists(wName1) = True Then 'If window is a valid file path
                Dim p As New System.Diagnostics.Process 'Dimension a process
                p.StartInfo.FileName = wName1.Substring(wName1.LastIndexOf("\") + 1) 'Set file name
                p.StartInfo.WorkingDirectory = wName1.Substring(0, wName1.LastIndexOf("\")) 'Set working directory
                p.Start() ''''''''''''''''''''''''''''Start process
                If p.Handle.ToInt32 <> 0 Then ''''''''If there is a native process handle
                    p.WaitForInputIdle(2500) '''''''''Wait for it to be ready for input
                    apiWaitForInputIdle(p.Handle.ToInt32, 2500) 'Wait API style
                    hwnd = p.MainWindowHandle.ToInt32 'Set handle to new process
                End If
            End If
        End If
        If hwnd = 0 Then hwnd = -1 '''''''''''''''''''If main window not found, then set failure return
        wF.Foreground = hwnd '''''''''''''''''''''''''Set structure return
        wF.Focus = hwnd ''''''''''''''''''''''''''''''Set focus to main handle for now
        If hwnd = -1 Then Return wF ''''''''''''''''''If no handle found then return now
        If i = 1 Then ''''''''''''''''''''''''''''''''If only the main window was specified
            Dim prvswnd As Int32 = apiGetForegroundWindow 'Remember the current foreground window
            ForceForeground(hwnd) ''''''''''''''''''''Force foreground onto specified main window
            Sleep() ''''''''''''''''''''''''''''''''''Sleep a moment
            wF = GetWinFocus() '''''''''''''''''''''''Set structure
            ForceForeground(prvswnd) '''''''''''''''''Force foreground back to where it was
        ElseIf i > 1 Then ''''''''''''''''''''''''''''If there are child windows specified
            cwnd = GetChildWindow(hwnd, wNames(2), CInt(wNames(3))) 'Set first child handle
            wF.Focus = cwnd ''''''''''''''''''''''''''Set focus handle to first child
            If (i - 1) > 1 Then ''''''''''''''''''''''If more than one child specified
                For q As Int32 = 4 To wNames.Length - 1 Step 2 'Step through array looking for grandchildren.
                    cwnd = GetChildWindow(cwnd, wNames(q), CInt(wNames(q + 1))) 'Set new child window
                    wF.Focus = cwnd ''''''''''''''''''Set focus handle to youngest grandchild so far
                Next '''''''''''''''''''''''''''''''''Next in array
            End If
        End If
        Return wF ''''''''''''''''''''''''''''''''''''Return WINFOCUS structure
    End Function

    Public Function GetWinHandles1(ByVal wName1 As String) As WINFOCUS
        Return GetWinHandles(wName1)
    End Function
    ''' <summary>
    ''' _____________________________________________________________________________________________________________________________________________________________________________ 
    ''' Sleeps for the specified time, while flushing keyboard messages at the specified interval.
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Returns true if there are no more messages in the queue.
    ''' </summary>
    ''' <param name="dwMilliseconds">(Optional) The number of milliseconds to sleep in milliseconds, - 5000 = 5 seconds.
    ''' If 0 is specified then the function sleeps until there are no more input messages in the queue to process or 5 seconds elapses.
    ''' Specify an integer.</param>
    ''' <param name="fInterval">(Optional) The number of milliseconds between flushes.
    ''' Flushing helps process window messages that are still in the queue.  Specify an integer from 0-4999.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Sleep(Optional ByVal dwMilliseconds As Int32 = 0, Optional ByVal fInterval As Int32 = 1) As Boolean
        Dim tick As Int32 = System.Environment.TickCount 'Get the number of millisecons since last log on
        If fInterval > 4999 Then fInterval = 4999 ''''Make sure interval is not longer than 5 seconds to avoid hanging the main app window
        Do '''''''''''''''''''''''''''''''''''''''''''Loop until the specified timeout
            System.Threading.Thread.Sleep(fInterval) 'Sleep for one interval at a time
            apiSwitchToThread() : Flush() ''''''''''''Yield to other waiting threads of the OS's choice.  Process any messages in the queue
            If dwMilliseconds = 0 Then '''''''''''''''If short rest specified
                If apiGetQueueStatus(QS_ALLQUEUE) = 0 OrElse System.Environment.TickCount >= (tick + 5000) Then Return True 'If there are no key/mouse messages in the queue or 5 seconds has elapsed then return
            Else '''''''''''''''''''''''''''''''''''''If a timeout is specified
                If System.Environment.TickCount >= (tick + dwMilliseconds) Then Return Not CBool(apiGetQueueStatus(QS_ALLQUEUE)) 'If time is up then exit with return of state
            End If
        Loop
    End Function

    ''' <summary>
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Waits for a window to exist, and waits for it to be ready for keyboard and mouse input.
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Returns true if a window exists. Returns false if it does not, or the specified time ends.
    ''' </summary>
    ''' <param name="wTitle">The title or class name of the window.  Specify an application title or class name.</param>
    ''' <param name="dwMilliseconds">(Optional) The number of milliseconds to wait.  Specify an integer.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function WaitForWindow(ByVal wTitle As String, Optional ByVal dwMilliseconds As Int32 = 5000) As Boolean
        Dim tick As Int32 = System.Environment.TickCount 'Get the number of millisecons since last log on
        Dim hwnd As Int32 = 0 ''''''''''''''''''''''''Initialize handle
        Do '''''''''''''''''''''''''''''''''''''''''''Loop this thread until it's time
            hwnd = apiFindWindow(Nothing, wTitle) ''''Find window by title
            If hwnd = 0 Then hwnd = apiFindWindow(wTitle, Nothing) 'If not found by title then find by class name
            If hwnd <> 0 Then Return WaitForWindowIdle(hwnd) 'If found then wait for input idle and return
            If System.Environment.TickCount >= (tick + dwMilliseconds) Then Return False 'If time is up then return
            System.Threading.Thread.Sleep(1) : Flush() 'Sleep thread for one millisecond and process any messages in the queue
        Loop
    End Function

    Private Function ForceForeground(ByVal hWnd As Int32) As Boolean
        apiLockSetForegroundWindow(LSFW_UNLOCK) ''''''Unlock setforegroundwindow calls
        apiAllowSetForegroundWindow(ASFW_ANY) ''''''''Allow setforeground window calls
        KeyEvent(VK_MENU, False, True) '''''''''''''''Lift menu key if pressed, and it also allows the foreground window to be set
        ForceForeground = CBool(apiSetForegroundWindow(hWnd)) 'Set foreground window
        apiLockSetForegroundWindow(LSFW_LOCK) ''''''''Lock other apps from using setforegroundwindow
    End Function

    Private Function GetChildWindow(ByVal hWnd As Int32, Optional ByVal wName As String = "", Optional ByVal wIndex As Int32 = 1) As Int32
        Dim cwnd, TextCount, ClassCount, NullCount As Int32, w As New WINNAME
        Do '''''''''''''''''''''''''''''''''''''''''''Loop through sibling windows
            cwnd = apiFindWindowEx(hWnd, cwnd, Nothing, Nothing) 'Set child handle
            If cwnd = 0 Then Return cwnd '''''''''''''If no more sibling children then return
            w = GetWinName(cwnd, True, True, False) ''Get the text, and class from that window
            If w.lpText.IndexOf("&") <> NEGATIVE Then w.lpText = w.lpText.Remove(w.lpText.IndexOf("&"), 1) 'Remove shortcut symbols for easier evaluation
            If w.lpText = wName Then TextCount += 1 ''If text matches the specified wName then increment the text count
            If TextCount = wIndex Then Return cwnd ''If text count is equal to the specified wIndex then return
            If w.lpClass = wName Then ClassCount += 1 'If class name matches the specified wName then increment the class count
            If ClassCount = wIndex Then Return cwnd ''If class count is equal to the specified wIndex then return
            If w.lpText = "" AndAlso w.lpClass = "" AndAlso wName = "" Then NullCount += 1 'If text and class name not specified, then increment by index only
            If NullCount = wIndex Then Return cwnd ''If null count is equal to wIndex then return
        Loop
    End Function

    Private Function GetWinAncestory(ByVal cWnd As Int32, ByVal gName As Boolean) As Int32()
        Dim i, z, cwnd2 As Int32, hwnds() As Int32, w As New WINNAME 'Dimension some variables
        cwnd2 = cWnd '''''''''''''''''''''''''''''''''Remember handle for seconds loop
        w = GetWinName(cWnd, True, True, False) ''''''Get text and class name of the window
        If gName = True Then MessageBox.Show(z.ToString & ":  " & w.lpText & "  |  " & w.lpClass, "Focus window:  Title | Class name", MessageBoxButtons.OK, MessageBoxIcon.Information) 'Display to developer if specified
        Do '''''''''''''''''''''''''''''''''''''''''''Loop while counting ancestors
            cWnd = apiGetParent(cWnd) ''''''''''''''''Get new parent handle if any found
            If cWnd = 0 Then Exit Do '''''''''''''''''If there are no more parents then abort without setting anymore to the array
            i += 1 '''''''''''''''''''''''''''''''''''Increment the index by 1
        Loop
        ReDim hwnds(i) '''''''''''''''''''''''''''''''Re-Dimension array to number of ancestors including the handle specified
        Do '''''''''''''''''''''''''''''''''''''''''''Do look for parents of the window specified.
            hwnds(z) = cwnd2 '''''''''''''''''''''''''Set the indexed array element to the specified handle or it's ancestors
            cwnd2 = apiGetParent(cwnd2) ''''''''''''''Get new parent handle if any found
            If cwnd2 = 0 Then Exit Do ''''''''''''''''If there are no more parents then abort
            z += 1 '''''''''''''''''''''''''''''''''''Increment the index by 1
            w = GetWinName(cwnd2, True, True, False) 'Get text and class name of the window
            If gName = True Then MessageBox.Show(z.ToString & ":  " & w.lpText & "  |  " & w.lpClass, "Parent window:  Title | Class name", MessageBoxButtons.OK, MessageBoxIcon.Information) 'Display to developer if specified
        Loop
        System.Array.Reverse(CType(hwnds, System.Int32())) 'Reverse the order of the values so that the main handle is first, and the focus window is last.
        Return hwnds '''''''''''''''''''''''''''''''''Return array of integer handles, or handle.
    End Function

    Private Function GetWinName(ByVal hWnd As Int32, Optional ByVal gText As Boolean = True, Optional ByVal gClass As Boolean = True, Optional ByVal gProcess As Boolean = True) As WINNAME
        Dim tLength, rValue As Int32, n As New WINNAME
        n.lpText = "" ''''''''''''''''''''''''''''''''Initialize string for text name
        n.lpClass = "" '''''''''''''''''''''''''''''''Initialize string for class name
        n.lpProcessName = "" '''''''''''''''''''''''''Initialize string for process name
        If gText = True Then '''''''''''''''''''''''''If text is to be retrieved
            tLength = apiGetWindowTextLength(hWnd) + 4 'Get length
            n.lpText = n.lpText.PadLeft(tLength) '''''Pad with buffer
            rValue = apiGetWindowText(hWnd, n.lpText, tLength) 'Get text
            n.lpText = n.lpText.Substring(0, rValue) 'Strip buffer
        End If
        If gClass = True Then ''''''''''''''''''''''''If class name is to be retrieved
            n.lpClass = n.lpClass.PadLeft(260) '''''''Pad with buffer
            rValue = apiGetClassName(hWnd, n.lpClass, 260) 'Get classname
            n.lpClass = n.lpClass.Substring(0, rValue) 'Strip buffer
        End If
        If gProcess = True Then ''''''''''''''''''''''If process name is to be retrieved
            Dim phwnd As Int32 = 0 '''''''''''''''''''Initialize handle
            For Each p As System.Diagnostics.Process In System.Diagnostics.Process.GetProcesses() 'Loop through all processes
                phwnd = p.MainWindowHandle.ToInt32 ''Set handle of that process
                If phwnd = hWnd OrElse phwnd = apiGetAncestor(hWnd, GA_ROOT) Then 'If handle matches the specified handle, or the ancestor of it
                    n.lpProcessName = p.ProcessName ''Get process name
                    Exit For '''''''''''''''''''''''''Exit loop
                End If
            Next '''''''''''''''''''''''''''''''''''''Next process
        End If
        Return n '''''''''''''''''''''''''''''''''''''Return WINNAME structure
    End Function

    Private Function KeyEvent(Optional ByVal vKey As Int32 = 0, Optional ByVal kDown As Boolean = True, Optional ByVal kUp As Boolean = True) As Boolean
        If vKey < 0 OrElse vKey > 255 Then Return False 'If vKey is not valid between 0-255
        KeyEvent = True ''''''''''''''''''''''''''''''Value for return results
        If kDown = True Then '''''''''''''''''''''''''If key is to be depressed
            If apikeybd_event(vKey, 0, 0, -11) = False Then KeyEvent = False 'Press key down set return of any failure 
        End If
        If kUp = True Then '''''''''''''''''''''''''''If key is to be released
            If apikeybd_event(vKey, 0, KEYEVENTF_KEYUP, -11) = False Then KeyEvent = False 'Lift key up set return of any failure
        End If
    End Function

    Private Function WaitForWindowIdle(ByVal hWnd As Int32) As Boolean
        For Each p As Process In Process.GetProcesses() 'Iterate through all processes
            If p.MainWindowHandle.ToInt32 = hWnd Then 'If handle matches the specified handle
                Try
                    p.WaitForInputIdle(2500) '''''''''Wait for process to be ready for input messages
                    WaitForWindowIdle = CBool(apiWaitForInputIdle(p.Handle.ToInt32, 2500)) 'Get results with API
                Catch ex As Exception ''''''''''''''''If no graphical interface or other error
                    apiSwitchToThread() : Flush() ''''Yield to waiting threads.  Process messages in the queue
                End Try
                Return Not WaitForWindowIdle '''''''''Return result as inverse of API
            End If
        Next '''''''''''''''''''''''''''''''''''''''''Next process
    End Function

#End Region

#Region "KeyboardInput"
    Const HC_GETNEXT As Int32 = 1
    Const VK_SHIFT As Int32 = 16
    Const VK_CONTROL As Int32 = 17
    Const VK_CAPITAL As Int32 = 20
    Const VK_ESCAPE As Int32 = 27
    Const VK_LSHIFT As Int32 = 160
    Const VK_RSHIFT As Int32 = 161
    Const VK_LCONTROL As Int32 = 162
    Const VK_RCONTROL As Int32 = 163
    Const VK_LMENU As Int32 = 164
    Const VK_RMENU As Int32 = 165
    Const VK_LWIN As Int32 = 91
    Const VK_RWIN As Int32 = 92
    Const WM_SETTEXT As Int32 = 12
    Const WM_KEYDOWN As Int32 = 256
    Const WM_KEYUP As Int32 = 257
    Const WH_KEYBOARD_LL As Int32 = 13
    Private Structure KBDLLHOOKSTRUCT
        Public vkCode, scanCode, flags, time, dwExtraInfo As Int32
    End Structure
    Private Declare Function apiAttachThreadInput Lib "user32" Alias "AttachThreadInput" (ByVal idAttach As Int32, ByVal idAttachTo As Int32, ByVal fAttach As Int32) As Int32
    Private Declare Function apiCallNextKeyHookEx Lib "user32" Alias "CallNextHookEx" (ByVal hHook As Int32, ByVal nCode As Int32, ByVal wParam As Int32, ByVal lParam As KBDLLHOOKSTRUCT) As Int32
    Private Declare Function apiGetCurrentThreadId Lib "kernel32" Alias "GetCurrentThreadId" () As Int32
    Private Declare Function apiGetKeyState Lib "user32" Alias "GetKeyState" (ByVal vKey As Int32) As Int32
    Private Declare Function apiSetWindowsKeyHookEx Lib "user32" Alias "SetWindowsHookExA" (ByVal idHook As Int32, ByVal lpfn As KeyboardHookDelegate, ByVal hmod As Int32, ByVal dwThreadId As Int32) As Int32
    Private Declare Function apiUnhookWindowsHookEx Lib "user32" Alias "UnhookWindowsHookEx" (ByVal hHook As Int32) As Int32
    Private Declare Function apiVkKeyScan Lib "user32" Alias "VkKeyScanA" (ByVal cChar2 As Int32) As Int32
    Private Delegate Function KeyboardHookDelegate(ByVal Code As Int32, ByVal wParam As Int32, ByRef lParam As KBDLLHOOKSTRUCT) As Int32
    <System.Runtime.InteropServices.MarshalAs(System.Runtime.InteropServices.UnmanagedType.FunctionPtr)> Private kcallback As KeyboardHookDelegate
    Private hKey, fWnd, kSent As Int32 'Handle for the keyboard, and the main window with directive focus, and the count of the keys sent

    ''' <summary>
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Sends a windows message to the specified window.
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Use GetWinHandles or GetWinFocus to get a structure.
    ''' Returns true if the message was sent to press an individual key.
    ''' </summary>
    ''' <param name="vKey">Virtual key code number 0-255.</param>
    ''' <param name="wFocus">The structure of the window to focus on.
    ''' Use GetWinHandles or GetWinFocus to get a structure.</param>
    ''' <param name="kDown">(Optional) Specifies that key is to be pressed down.</param>
    ''' <param name="kUp">(Optional) Specifies that key is to be lifted up.</param>
    ''' <param name="bPost">(Optional) Posts the message into the queue,
    ''' instead of sending it and waiting for a return</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Message(ByVal vKey As Int32, ByVal wFocus As WINFOCUS, Optional ByVal kDown As Boolean = True, Optional ByVal kUp As Boolean = True, Optional ByVal bPost As Boolean = False) As Boolean
        Message = True '''''''''''''''''''''''''''''''Set default return value
        If bPost = True Then '''''''''''''''''''''''''If post specified
            If kDown = True AndAlso apiPostMessage(wFocus.Focus, WM_KEYDOWN, vKey, Nothing) = 0 Then Message = False 'Press down set return of any failure
            If kUp = True AndAlso apiPostMessage(wFocus.Focus, WM_KEYUP, vKey, Nothing) = 0 Then Message = False 'Lift up set return of any failure
        Else '''''''''''''''''''''''''''''''''''''''''If Sending message
            If kDown = True AndAlso apiSendMessage(wFocus.Focus, WM_KEYDOWN, vKey, Nothing) = 0 Then Message = False 'Press down set return of any failure
            If kUp = True AndAlso apiSendMessage(wFocus.Focus, WM_KEYUP, vKey, Nothing) = 0 Then Message = False 'Lift up set return of any failure
        End If
    End Function

    ''' <summary>
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Sends keyboard messages or events to the specified window.
    ''' If asMessage is false, then this function simulates keyboard events,
    ''' and directs keyboard focus onto the specified window structure.
    ''' _____________________________________________________________________________________________________________________________________________________________________________ 
    ''' Use GetWinHandles or GetWinFocus to get a structure.
    ''' Returns the number of keys sent.  Returns 0 only if the keyboard could not be hooked,
    ''' or a valid window handle cannot be found, or the foregroundwindow cannot be forced,
    ''' or focus could not be set, or the keys cannot be simulated.
    ''' </summary>
    ''' <param name="cText">The text or key commands to send.  See the GetCommands sub,
    ''' for more details about commands.</param>
    ''' <param name="wFocus">The structure of the window to focus on.
    ''' Use GetWinHandles or GetWinFocus to get a structure.</param>
    ''' <param name="asMessage">(Optional) Send as window message, or as a keyboard event.
    ''' It's recommended that you use True in this parameter for writing text. If false then rForeground applies.</param>
    ''' <param name="rForeground">(Optional) Returns the foreground window to it's previous focus
    ''' before the keys were sent. Applies only if asMessage is false. </param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Send(ByVal cText As String, ByVal wFocus As WINFOCUS, Optional ByVal asMessage As Boolean = True, Optional ByVal rForeground As Boolean = False) As Int32
        Dim ws As New WINSTATE
        Dim UserCapsLock, sThread, IsLetter As Boolean 'Declare some switches
        Dim hwnd, cwnd, prvWnd, txtLength, VK, repeat As Int32 ''Dimension some integers
        Dim txtRemain, Letter, PrevLetter As String ''Dimension some strings
        txtRemain = cText ''''''''''''''''''''''''''''Initialize remaining text
        txtLength = cText.Length '''''''''''''''''''''Initialize Text
        If wFocus.Foreground = -1 Then Return 0 ''''''Exit failure if GetWinHandles returned -1
        If wFocus.Foreground = 0 Then wFocus = GetWinFocus()
        hwnd = wFocus.Foreground '''''''''''''''''''''Read structure handles, and make easier to use
        cwnd = wFocus.Focus ''''''''''''''''''''''''''Set child handle
        repeat = 1 '''''''''''''''''''''''''''''''''''Initialize repeat to one, for commands
        Letter = "" ''''''''''''''''''''''''''''''''''Initialize the first letter to nothing
        If cwnd = 0 Then cwnd = hwnd '''''''''''''''''If no child specified, then set it to main window
        If apiIsWindow(hwnd) = False Then Return 0 ''Make sure it's a valid window, if not abort
        If apiIsIconic(hwnd) = True Then ws.IsIconic = apiShowWindow(hwnd, SW_SHOWNORMAL) 'If window is minimized then show it, and remember
        If apiIsWindowEnabled(hwnd) = False Then ws.IsDisabled = apiEnableWindow(hwnd, True) 'If window is disabled then enable it and remember
        If apiIsWindowEnabled(cwnd) = False Then ws.IsChildDisabled = apiEnableWindow(cwnd, True) 'If child window is disabled then enable it and remember
        If apiIsWindowVisible(hwnd) = False Then ws.IsHidden = Not apiShowWindow(hwnd, SW_SHOWNORMAL) 'If window is hidden then show it, and remember
        If apiIsWindowVisible(cwnd) = False Then ws.IsChildHidden = Not apiShowWindow(cwnd, SW_SHOWNORMAL) 'If child window is hidden then show it, and remember
        WaitForWindowIdle(hwnd) ''''''''''''''''''''''Wait for input idle
        If apiGetKeyState(VK_CAPITAL) = 1 Then '''''''Get state of the caps lock key.
            UserCapsLock = KeyEvent(VK_CAPITAL) ''''''Remember state if toggled, by return of keyevent
            System.Threading.Thread.Sleep(0) '''''''''Wait for other threads
        End If
        Lift() '''''''''''''''''''''''''''''''''''''''Lift shift, control and menu keys
        If asMessage = False Then ''''''''''''''''''''Send as chain of events
            kcallback = New KeyboardHookDelegate(AddressOf Callback) 'Set new hook delegate
            hKey = apiSetWindowsKeyHookEx(WH_KEYBOARD_LL, kcallback, System.Runtime.InteropServices.Marshal.GetHINSTANCE(System.Reflection.Assembly.GetExecutingAssembly.GetModules()(0)).ToInt32, 0) 'Set keyboard hook for verifying the time that a virtual key was pressed
            If hKey = 0 Then Return KeyAbort(wFocus, ws, UserCapsLock, sThread, False, prvWnd)
            If apiGetWindowThreadProcessId(hwnd, 0) = apiGetCurrentThreadId Then sThread = True
            If sThread = False Then ''''''''''''''''''If handle is a different thread than this one
                sThread = Not AttachInput(hwnd, True) 'Attach thread input to this thread 
            Else '''''''''''''''''''''''''''''''''''''If the same thread
                rForeground = False ''''''''''''''''''No reason to reset foreground window, because focus never leaves to begin with
            End If
            prvWnd = apiGetForegroundWindow ''''''''''Remember current foreground window
            If ForceForeground(hwnd) = False Then Return KeyAbort(wFocus, ws, UserCapsLock, sThread, False, prvWnd) 'If the foreground cannot be set within 5 seconds then abort
            If cwnd <> hwnd Then '''''''''''''''''''''If child window is different than main window
                If apiSetFocus(cwnd) = 0 Then Return KeyAbort(wFocus, ws, UserCapsLock, sThread, rForeground, prvWnd) 'If focus window cannot be set then abort
            End If
            fWnd = hwnd ''''''''''''''''''''''''''''''Set form class variable for the foregroundwindow handle
        Else '''''''''''''''''''''''''''''''''''''''''If message
            sThread = True '''''''''''''''''''''''''''Set same thread to true, so it doesn't detach for no reason, since it was never set
            rForeground = False ''''''''''''''''''''''Foreground is not set, so no need to return it
            If hwnd = cwnd Then ''''''''''''''''''''''If the main window is the same as child
                If ForceForeground(hwnd) = False Then 'If Force foreground window fails
                    Return KeyAbort(wFocus, ws, UserCapsLock, sThread, False, prvWnd) 'return abort's return
                End If
                Sleep() ''''''''''''''''''''''''''''''Sleep a moment and flush any messages so that the child window if any, gets focus.
                wFocus = GetWinFocus() '''''''''''''''Set by getting the WINFOCUS structure
                rForeground = True '''''''''''''''''''Should return it
            End If
        End If
        Lift() '''''''''''''''''''''''''''''''''''''''Lift shift, control and menu keys
        For i As Int32 = 1 To txtLength ''''''''''''''Loop through each character of the text specified
            Dim cmdKey As String = "" ''''''''''''''''Initialize a string for special command keys
            PrevLetter = Letter ''''''''''''''''''''''Set previous letter to the current before setting the current
            Letter = txtRemain.Substring(0, 1) '''''''Set current letter to the left most
            txtRemain = txtRemain.Substring(txtRemain.Length - (cText.Length - i)) 'Cut string to get remaining text
            VK = apiVkKeyScan(Asc(Letter)) And 255 ''Set keyscan of that letter
            If Letter <> Letter.ToLower OrElse Letter = "!" OrElse Letter = "@" OrElse Letter = "$" OrElse Letter = "&" OrElse Letter = "*" OrElse Letter = "_" OrElse Letter = "|" OrElse Letter = ":" OrElse Letter = "<" OrElse Letter = ">" OrElse Letter = "?" Then
                KeyEvent(VK_SHIFT, True, False)  '''''Press shift if necessary 
            End If
            If Letter = "{" Then '''''''''''''''''''''If letter is a command bracket
                VK = 0 '''''''''''''''''''''''''''''''Set virtual key to zero(none)
                cmdKey = txtRemain.Substring(0, txtRemain.IndexOf("}")) 'Get text within command brackets
                If txtRemain.Length - (cmdKey.Length + 1) > 0 Then txtRemain = txtRemain.Substring(cmdKey.Length + 1) 'Set remaining text, as the text to the right of the command bracket
                i += cmdKey.Length + 1 '''''''''''''''Increment i the number of characters within command brackets since they are not going to be processed individually
                If cmdKey.IndexOf(" ") <> NEGATIVE AndAlso cmdKey.IndexOf(", ") = NEGATIVE Then 'If there is a space and no comma preceeding it then the command is to be repeated
                    If IsNumeric(cmdKey.Substring(cmdKey.IndexOf(" ") + 1)) Then repeat = CInt(cmdKey.Substring(cmdKey.IndexOf(" ") + 1)) 'If a number can be identified, then set it otherwise it stays 1._+{}|:"<>?
                    cmdKey = cmdKey.Substring(0, cmdKey.IndexOf(" ")) 'Strip off left side, which is the actual command
                End If
                If cmdKey.Length = 1 Then ''''''''''''If command is a single character
                    If cmdKey = "#" OrElse cmdKey = "+" OrElse cmdKey = "^" OrElse cmdKey = "%" OrElse cmdKey = "~" OrElse cmdKey = "(" OrElse cmdKey = ")" OrElse cmdKey = "[" OrElse cmdKey = "]" Then
                        KeyEvent(VK_SHIFT, True, False) 'Press shift for single character commands
                    End If
                    VK = apiVkKeyScan(Asc(cmdKey)) And 255 'Set keyscan for that letter
                Else '''''''''''''''''''''''''''''''''If command is a key name
                    VK = NEGATIVE ''''''''''''''''''''Set to negative in case this is not a valid key command, then it's a simple string
                    Dim kEnum As System.Windows.Forms.Keys 'Dimension key enumeration
                    For n As Int32 = 0 To 255 ''''''''Loop through all possible keys
                        kEnum = CType(n, System.Windows.Forms.Keys) 'Set key enum to index n
                        If kEnum.ToString.ToLower = cmdKey.ToLower Then  'If key name is equal to the special key(non-case sensitive), then set key
                            VK = n
                        End If
                    Next n '''''''''''''''''''''''''''Next key
                End If
            ElseIf Letter = "~" Then '''''''''''''''''If letter is tilde
                VK = 13 ''''''''''''''''''''''''''''''Set to return(enter) key
            ElseIf Letter = "+" Then '''''''''''''''''If letter is plus
                VK = 0 '''''''''''''''''''''''''''''''Do not send a regular key
                KeyEvent(VK_SHIFT, True, False) ''''''Press shift instead
            ElseIf Letter = "^" Then '''''''''''''''''If letter is caret
                VK = 0 '''''''''''''''''''''''''''''''Do not send a regular key
                KeyEvent(VK_CONTROL, True, False) ''''Press control instead
            ElseIf Letter = "#" Then '''''''''''''''''If letter is number signifier
                VK = 0 '''''''''''''''''''''''''''''''Do not send a regular key
                KeyEvent(VK_LWIN, True, False) '''''''Press left window key instead
            ElseIf Letter = "%" Then '''''''''''''''''If letter is percent
                VK = 0 '''''''''''''''''''''''''''''''Do not send a regular key
                KeyEvent(VK_MENU, True, False) '''''''Press menu key instead
            ElseIf Letter = "(" Then '''''''''''''''''If letter is left parenthesis
                VK = 0 '''''''''''''''''''''''''''''''Do not send a regular key
            ElseIf Letter = ")" Then '''''''''''''''''If letter is right parenthesis
                VK = 0 '''''''''''''''''''''''''''''''Do not send a regular key
                Lift() '''''''''''''''''''''''''''''''Lift extented keys
            End If
            If VK > NEGATIVE Then ''''''''''''''''''''If valid key code
                If asMessage = True Then '''''''''''''If sending a message
                    If VK > 64 AndAlso VK < 91 OrElse VK > 47 AndAlso VK < 58 OrElse VK > 105 AndAlso VK < 112 OrElse VK > 185 AndAlso VK < 193 OrElse VK > 218 AndAlso VK < 224 Then IsLetter = True
                    Message(VK, wFocus, Not IsLetter, True, True) 'Send message down and up
                Else '''''''''''''''''''''''''''''''''If sending an event
                    If KeyEvent(VK) = False Then Return KeyAbort(wFocus, ws, UserCapsLock, sThread, rForeground, prvWnd) 'If key event fails return abort
                End If
                kSent += 1 '''''''''''''''''''''''''''Count the sent keys
            Else '''''''''''''''''''''''''''''''''''''It's a string to be repeated
                For n As Int32 = 1 To repeat '''''''''Repeat key press
                    Dim r As String = cmdKey '''''''''Initialize remaining text
                    Dim a As String = "" '''''''''''''Initialize current letter
                    For w As Int32 = 1 To cmdKey.Length 'Iterate the length of the string
                        a = r.Substring(0, 1) ''''''''Strip off left most character
                        r = r.Substring(1) '''''''''''Set remaining to right most characters
                        VK = apiVkKeyScan(Asc(a)) And 255 'Get scan code for this key
                        If asMessage = True Then '''''If sending a message
                            If VK > 64 AndAlso VK < 91 OrElse VK > 47 AndAlso VK < 58 OrElse VK > 105 AndAlso VK < 112 OrElse VK > 185 AndAlso VK < 193 OrElse VK > 218 AndAlso VK < 224 Then IsLetter = True
                            Message(VK, wFocus, Not IsLetter, True, True) 'Send message down and up
                        Else '''''''''''''''''''''''''If sending an event
                            If KeyEvent(VK) = False Then Return KeyAbort(wFocus, ws, UserCapsLock, sThread, rForeground, prvWnd) 'If key event fails return abort
                        End If
                        kSent += 1 '''''''''''''''''''Count the sent keys
                    Next w '''''''''''''''''''''''''''Next character in string
                    VK = NEGATIVE ''''''''''''''''''''Reset to non valid command for next loop
                Next n '''''''''''''''''''''''''''''''Next repeated
                repeat = 1 '''''''''''''''''''''''''''Reset repeat to one for next loop
            End If
            If Letter <> "(" Then ''''''''''''''''''''If character is not parenthesis
                If PrevLetter = "#" Then '''''''''''''If previous letter was numeric signifier
                    KeyEvent(VK_LWIN, False, True) ''Lift win key
                ElseIf PrevLetter = "+" Then '''''''''If previous letter was shift
                    KeyEvent(VK_SHIFT, False, True) ''Lift shift key
                ElseIf PrevLetter = "%" Then '''''''''If previous letter was percent
                    KeyEvent(VK_MENU, False, True) ''Lift menu key
                ElseIf PrevLetter = "^" Then '''''''''If previous letter was caret
                    KeyEvent(VK_CONTROL, False, True) 'Lift control key
                End If
            End If
            If cmdKey.Length = 1 AndAlso cmdKey = "#" OrElse cmdKey = "+" OrElse cmdKey = "^" OrElse cmdKey = "%" OrElse cmdKey = "~" OrElse cmdKey = "(" OrElse cmdKey = ")" OrElse cmdKey = "[" OrElse cmdKey = "]" Then
                KeyEvent(VK_SHIFT, False, True) ''''''Lift shift for command keys
            End If
            If Letter <> Letter.ToLower OrElse Letter = "!" OrElse Letter = "@" OrElse Letter = "$" OrElse Letter = "&" OrElse Letter = "*" OrElse Letter = "_" OrElse Letter = "|" OrElse Letter = ":" OrElse Letter = "<" OrElse Letter = ">" OrElse Letter = "?" Then
                KeyEvent(VK_SHIFT, False, True) ''''''Lift shift if necessary
            End If
            If asMessage = True Then System.Threading.Thread.Sleep(0) 'Wait for other threads
        Next i '''''''''''''''''''''''''''''''''''''''Next character
        Return KeyAbort(wFocus, ws, UserCapsLock, sThread, rForeground, prvWnd) 'Final abort upon completion
    End Function

    ''' <summary>
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Sets the text of the specified window.
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Use GetWinHandles or GetWinFocus to get a structure.
    ''' Returns true if the message was sent to set the text.
    ''' </summary>
    ''' <param name="sText">The text to send.</param>
    ''' <param name="wFocus">The WINFOCUS structure.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Text(ByVal sText As String, ByVal wFocus As WINFOCUS) As Boolean
        Return CBool(apiSendMessage(wFocus.Focus, WM_SETTEXT, 0, sText)) 'Return the full result of SendMessage
    End Function

    Private Function AttachInput(ByVal hWnd As Int32, Optional ByVal bAttach As Boolean = True) As Boolean
        Return CBool(apiAttachThreadInput(apiGetWindowThreadProcessId(hWnd, 0), apiGetCurrentThreadId, CInt(bAttach)))
    End Function

    Private Function Callback(ByVal Code As Int32, ByVal wParam As Int32, ByRef lParam As KBDLLHOOKSTRUCT) As Int32
        If fWnd <> 0 AndAlso apiGetForegroundWindow <> fWnd Then 'If foreground window is not where it's supposed to be
            If ForceForeground(fWnd) = False Then ''''If foreground window cannot be set
                If lParam.dwExtraInfo = -11 Then kSent -= 1 'Uncount this key, since it's blocked, and has been sent from the Send function
                Return HC_GETNEXT ''''''''''''''''''''If foreground cannot be set then block key no matter if it's a user or internally sent from here
            End If
        End If
        If lParam.dwExtraInfo <> -11 Then Return HC_GETNEXT 'If key action and stroke blocked then get next key in the hook chain
        Return apiCallNextKeyHookEx(hKey, Code, wParam, lParam) 'Call next key hook if no action
    End Function

    Private Function KeyAbort(ByVal WFOCUS As WINFOCUS, ByVal WSTATE As WINSTATE, ByVal uCapsLock As Boolean, ByVal sThread As Boolean, ByVal rForeground As Boolean, ByVal prvWnd As Int32) As Int32
        Lift() '''''''''''''''''''''''''''''''''''''''Lift any extended keys just in case
        If sThread = False Then AttachInput(WFOCUS.Foreground, False) 'If thread was attached then detatch it
        If WSTATE.IsIconic = True Then apiShowWindow(WFOCUS.Foreground, SW_SHOWMINIMIZED) 'If window was minimized, then re-minimize it
        If WSTATE.IsDisabled = True Then apiEnableWindow(WFOCUS.Foreground, False) 'If window was disabled then re-disable it
        If WSTATE.IsChildDisabled = True Then apiEnableWindow(WFOCUS.Focus, False) 'If child window was disabled then re-disable it
        If WSTATE.IsHidden = True Then apiShowWindow(WFOCUS.Foreground, SW_HIDE) 'If window was hidden, then re-hide it
        If WSTATE.IsChildHidden = True Then apiShowWindow(WFOCUS.Focus, SW_HIDE) 'If child window was hidden, then re-hide it
        Sleep() ''''''''''''''''''''''''''''''''''''''Sleep for a moment and flush keys if needed
        If uCapsLock = True Then KeyEvent(VK_CAPITAL) 'If caps lock was on toggle capslock
        If rForeground = True Then ForceForeground(prvWnd)
        If hKey <> 0 AndAlso apiUnhookWindowsHookEx(hKey) = 1 Then hKey = 0 'Unhook keyboard and free keyboard handle if unhooked  'If return foreground was specified then return it
        KeyAbort = kSent '''''''''''''''''''''''''''''Set return value
        fWnd = 0 '''''''''''''''''''''''''''''''''''''Free foregroundwindow handle
        kSent = 0 ''''''''''''''''''''''''''''''''''''Free number of keys sent
    End Function

    Private Function Lift() As Boolean
        KeyEvent(VK_CONTROL, False, True) ''''''''''''Lift control
        KeyEvent(VK_RCONTROL, False, True) '''''''''''Lift right control
        KeyEvent(VK_LCONTROL, False, True) '''''''''''Lift left control
        KeyEvent(VK_SHIFT, False, True) ''''''''''''''Lift shift
        KeyEvent(VK_RSHIFT, False, True) '''''''''''''Lift right shift
        KeyEvent(VK_LSHIFT, False, True) '''''''''''''Lift left shift
        KeyEvent(VK_MENU, False, True) '''''''''''''''Lift menu
        KeyEvent(VK_RMENU, False, True) ''''''''''''''Lift right menu
        KeyEvent(VK_LMENU, False, True) ''''''''''''''Lift left menu
        Return True
    End Function

#End Region

#Region "MouseInput"
    Const HWND_DESKTOP As Int32 = 0
    Const HWND_NOTOPMOST As Int32 = -2
    Const HWND_TOP As Int32 = 0
    Const HWND_TOPMOST As Int32 = -1
    Const GW_HWNDFIRST As Int32 = 0
    Const MOUSEEVENTF_MOVE As Int32 = 1
    Const MOUSEEVENTF_LEFTDOWN As Int32 = 2
    Const MOUSEEVENTF_LEFTUP As Int32 = 4
    Const MOUSEEVENTF_RIGHTDOWN As Int32 = 8
    Const MOUSEEVENTF_RIGHTUP As Int32 = 16
    Const MOUSEEVENTF_MIDDLEDOWN As Int32 = 32
    Const MOUSEEVENTF_MIDDLEUP As Int32 = 64
    Const MOUSEEVENTF_XDOWN As Int32 = 128
    Const MOUSEEVENTF_XUP As Int32 = 256
    Const MOUSEEVENTF_WHEEL As Int32 = 2048
    Const MOUSEEVENTF_VIRTUALDESK As Int32 = 16384
    Const MOUSEEVENTF_ABSOLUTE As Int32 = 32768
    Const SM_CXSCREEN As Int32 = 0
    Const SM_CYSCREEN As Int32 = 1
    Const SM_FULLSCREEN As Int32 = 65535
    Const SWP_NOSIZE As Int32 = 1
    Const SWP_NOMOVE As Int32 = 2
    Const SWP_NOACTIVATE As Int32 = 16
    Const SWP_SHOWWINDOW As Int32 = 64
    Const WM_COMMAND As Int32 = 273
    Const WM_LBUTTONDBLCLK As Int32 = 515
    Const WM_LBUTTONDOWN As Int32 = 513
    Const WM_LBUTTONUP As Int32 = 514
    Const WM_MBUTTONDBLCLK As Int32 = 521
    Const WM_MBUTTONDOWN As Int32 = 519
    Const WM_MBUTTONUP As Int32 = 520
    Const WM_RBUTTONDBLCLK As Int32 = 518
    Const WM_RBUTTONDOWN As Int32 = 516
    Const WM_RBUTTONUP As Int32 = 517
    Private Structure ITEMINFO
        Public Width, Height, Right, Left, Top, Bottom As Int32, Center As POINTAPI
    End Structure
    Private Structure MENUINFO
        Public hwnd, hMenu, hSubMenu As Int32
    End Structure
    Private Structure EVENTCLICK
        Public mUp, mDown As Boolean, mButtons, x, y As Int32, wFocus As WINFOCUS
    End Structure
    Private Declare Function apiGetMenu Lib "user32" Alias "GetMenu" (ByVal hWnd As Int32) As Int32
    Private Declare Function apiGetMenuItemCount Lib "user32" Alias "GetMenuItemCount" (ByVal hMenu As Int32) As Int32
    Private Declare Function apiGetMenuItemID Lib "user32" Alias "GetMenuItemID" (ByVal hMenu As Int32, ByVal nPos As Int32) As Int32
    Private Declare Function apiGetMenuItemRect Lib "user32" Alias "GetMenuItemRect" (ByVal hWnd As Int32, ByVal hMenu As Int32, ByVal uItem As Int32, ByRef lprcItem As RECT) As Int32
    Private Declare Function apiGetMenuString Lib "user32" Alias "GetMenuStringA" (ByVal hMenu As Int32, ByVal wIDItem As Int32, ByVal lpString As String, ByVal nMaxCount As Int32, ByVal wFlag As Int32) As Int32
    Private Declare Function apiGetMessageExtraInfo Lib "user32" Alias "GetMessageExtraInfo" () As Int32
    Private Declare Function apiGetSubMenu Lib "user32" Alias "GetSubMenu" (ByVal hMenu As Int32, ByVal nPos As Int32) As Int32
    Private Declare Function apiGetSystemMetrics Lib "user32" Alias "GetSystemMetrics" (ByVal nIndex As Int32) As Int32
    Private Declare Function apiGetWindowRect Lib "user32" Alias "GetWindowRect" (ByVal hWnd As Int32, ByRef lpRect As RECT) As Boolean
    Private Declare Function apiIsMenu Lib "user32" Alias "IsMenu" (ByVal hMenu As Int32) As Boolean
    Private Declare Function apimouse_event Lib "user32" Alias "mouse_event" (ByVal dwFlags As Int32, ByVal dx As Int32, ByVal dy As Int32, ByVal cButtons As Int32, ByVal dwExtraInfo As Int32) As Boolean
    Private Declare Function apiMoveWindow Lib "user32" Alias "MoveWindow" (ByVal hWnd As Int32, ByVal x As Int32, ByVal y As Int32, ByVal nWidth As Int32, ByVal nHeight As Int32, ByVal bRepaint As Boolean) As Boolean
    Private Declare Function apiSetCursorPos Lib "user32" Alias "SetCursorPos" (ByVal X As Int32, ByVal Y As Int32) As Boolean
    Private Declare Function apiSetWindowPos Lib "User32" Alias "SetWindowPos" (ByVal hWnd As Int32, ByVal hWndInsertAfter As Int32, ByVal X As Int32, ByVal Y As Int32, ByVal cx As Int32, ByVal cy As Int32, ByVal wFlags As Int32) As Int32
    Private eClick As New EVENTCLICK, mnuClickNames(1) As String

    ''' <summary>
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' An enumeration of different mouse events.
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Use in the first parameter of the Click function.
    ''' </summary>
    ''' <remarks></remarks>
    Public Enum Buttons
        LeftClick = MOUSEEVENTF_LEFTDOWN + MOUSEEVENTF_LEFTUP
        LeftDoubleClick = MOUSEEVENTF_LEFTDOWN + MOUSEEVENTF_LEFTUP + MOUSEEVENTF_LEFTDOWN + MOUSEEVENTF_LEFTUP
        MiddleClick = MOUSEEVENTF_MIDDLEDOWN + MOUSEEVENTF_MIDDLEUP
        MiddleDoubleClick = MOUSEEVENTF_MIDDLEDOWN + MOUSEEVENTF_MIDDLEUP + MOUSEEVENTF_MIDDLEDOWN + MOUSEEVENTF_MIDDLEUP
        Move = MOUSEEVENTF_MOVE
        MoveAbsolute = MOUSEEVENTF_ABSOLUTE + MOUSEEVENTF_MOVE
        RightClick = MOUSEEVENTF_RIGHTDOWN + MOUSEEVENTF_RIGHTUP
        RightDoubleClick = MOUSEEVENTF_RIGHTDOWN + MOUSEEVENTF_RIGHTUP + MOUSEEVENTF_RIGHTDOWN + MOUSEEVENTF_RIGHTUP
        VirtualDesk = MOUSEEVENTF_VIRTUALDESK
        Wheel = MOUSEEVENTF_WHEEL
        xClick = MOUSEEVENTF_XDOWN + MOUSEEVENTF_XUP
        xDoubleClick = MOUSEEVENTF_XDOWN + MOUSEEVENTF_XUP + MOUSEEVENTF_XDOWN + MOUSEEVENTF_XUP
    End Enum

    ''' <summary>
    ''' _____________________________________________________________________________________________________________________________________________________________________________ 
    ''' This function sends window messages directly to a window.
    ''' If the asMessage parameter is false then, this function simulates mouse events directly to a window.
    ''' _____________________________________________________________________________________________________________________________________________________________________________ 
    ''' Returns false if the specified window/s cannot be found.
    ''' If the asMessage parameter is false then, this function returns false if a handle cannot be established or
    ''' the top window could not be set, or a window rectangle could not be found, or the cursor could not be set,
    ''' or the mouse event could not be simulated.
    ''' </summary>
    ''' <param name="mButtons">The button to click.  Use the Buttons enumeration for this parameter.</param>
    ''' <param name="wFocus">The structure of the window to focus on.
    ''' Use GetWinHandles or GetWinFocus to get a structure.</param>
    ''' <param name="mDown">(Optional) Press mouse button down only.</param>
    ''' <param name="mUp">(Optional) Lift mouse button up only.</param>
    ''' <param name="asMessage">(Optional) Send as window message, or as a mouse event.
    ''' It's recommended that you use True in this parameter.
    ''' If false then the x and y parameters may apply.</param>
    ''' <param name="x">(Optional) The x coordinate only applies if asMessage is false, and mButtons is
    ''' Buttons.Move or Buttons.MoveAbsolute.</param>
    ''' <param name="y">(Optional) The y coordinate only applies if asMessage is false, and mButtons is
    ''' Buttons.Move or Buttons.MoveAbsolute.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function Click(ByVal mButtons As Int32, ByVal wFocus As WINFOCUS, Optional ByVal mDown As Boolean = True, Optional ByVal mUp As Boolean = True, Optional ByVal asMessage As Boolean = True, Optional ByVal x As Int32 = -1, Optional ByVal y As Int32 = -1) As Boolean
        If wFocus.Foreground = -1 Then Return False
        If wFocus.Foreground = 0 Then wFocus = GetWinFocus(False, False)
        If asMessage = True Then  ''''''''''''''''''''If sending as message
            Dim hwnd, cwnd, zOrder, zOrderChild, WM_DOWN, WM_UP As Int32, ws As New WINSTATE
            If mButtons = Buttons.Move Then ''''''''''If moving from current position
                Dim cP As New POINTAPI
                apiGetCursorPos(cP) ''''''''''''''''''Get cursor position
                apiSetCursorPos(cP.X + x, cP.Y + y) ''Add to coordinates, and set cursor there
                Return True ''''''''''''''''''''''''''Return success
            ElseIf mButtons = Buttons.MoveAbsolute Then
                apiSetCursorPos(x, y) ''''''''''''''''Set to absolute coordinates
                Return True ''''''''''''''''''''''''''Return success
            ElseIf mButtons = Buttons.LeftClick OrElse mButtons = Buttons.LeftDoubleClick Then 'If button specified is left
                WM_DOWN = WM_LBUTTONDOWN : WM_UP = WM_LBUTTONUP 'Then set left messages
            ElseIf mButtons = Buttons.RightClick OrElse mButtons = Buttons.RightDoubleClick Then 'If button specified is right
                WM_DOWN = WM_RBUTTONDOWN : WM_UP = WM_RBUTTONUP 'Then set right messages
            ElseIf mButtons = Buttons.MiddleClick OrElse mButtons = Buttons.MiddleDoubleClick Then 'If button specified is middle
                WM_DOWN = WM_MBUTTONDOWN : WM_UP = WM_MBUTTONUP ' Then set middle messages
            End If
            hwnd = wFocus.Foreground '''''''''''''''''Set main handle to something smaller
            cwnd = wFocus.Focus ''''''''''''''''''''''Set child handle(if any) to something smaller
            If cwnd = 0 Then cwnd = hwnd '''''''''''''If no child specified, then set it to the main window
            If apiIsIconic(hwnd) = True Then ws.IsIconic = apiShowWindow(hwnd, SW_SHOWNORMAL) : Sleep(25) 'if minimized then show normal
            If apiIsWindowEnabled(hwnd) = False Then ws.IsDisabled = apiEnableWindow(hwnd, True) : Sleep(25) 'If disabled then enable
            If apiIsWindowEnabled(cwnd) = False Then ws.IsChildDisabled = apiEnableWindow(cwnd, True) : Sleep(25) 'If child disabled then enable
            If apiIsWindowVisible(hwnd) = False Then ws.IsHidden = Not apiShowWindow(hwnd, SW_SHOWNORMAL) : Sleep(25) 'If hidden then show
            If apiIsWindowVisible(cwnd) = False Then ws.IsChildHidden = Not apiShowWindow(cwnd, SW_SHOWNORMAL) : Sleep(25) 'If child hidden then show
            zOrder = GetSetZOrder(hwnd) ''''''''''''''Remember main window's place in the z-order
            zOrderChild = GetSetZOrder(cwnd) '''''''''Remember child window's place in the z-order
            If hwnd <> apiGetTopWindow(HWND_DESKTOP) Then apiSetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOACTIVATE + SWP_SHOWWINDOW + SWP_NOMOVE + SWP_NOSIZE)
            If cwnd <> apiGetTopWindow(apiGetParent(cwnd)) Then apiSetWindowPos(cwnd, HWND_TOP, 0, 0, 0, 0, SWP_NOACTIVATE + SWP_SHOWWINDOW + SWP_NOMOVE + SWP_NOSIZE)
            If mDown = True Then apiSendMessage(cwnd, WM_DOWN, 0, Nothing) 'If button down is specified, then press down
            If mUp = True Then apiSendMessage(cwnd, WM_UP, 0, Nothing) 'If button up is specified, then lift up
            If mButtons = Buttons.LeftDoubleClick OrElse mButtons = Buttons.RightDoubleClick OrElse mButtons = Buttons.MiddleDoubleClick Then 'If it's a double click
                If mDown = True Then apiSendMessage(cwnd, WM_DOWN, 0, Nothing) 'Down again
                If mUp = True Then apiSendMessage(cwnd, WM_UP, 0, Nothing) 'Up again
            End If
            MouseAbort(ws, False, hwnd, cwnd, zOrder, zOrderChild, Nothing) 'Final abort return without failure
        Else '''''''''''''''''''''''''''''''''''''''''Else it's an event to send
            Dim t As New System.Threading.Thread(AddressOf ClickEventThread)
            eClick.mButtons = mButtons '''''''''''''''Set parameters to structure for event thread
            eClick.mDown = mDown
            eClick.mUp = mUp
            eClick.wFocus = wFocus
            eClick.x = x
            eClick.y = y
            t.Start() ''''''''''''''''''''''''''''''''Start thread
        End If
        Return True
    End Function
    Private Sub ClickEventThread()
        Dim hwnd, cwnd, zOrder, zOrderChild As Int32, p As New POINTAPI, ws As New WINSTATE
        Dim rCursor As Boolean, repeat As Int32
        If eClick.mButtons = Buttons.Move Then '''''''Move cursor to click point
            MouseEvent(eClick.mButtons, eClick.x, eClick.y) 'Move cursor to click point
            Exit Sub '''''''''''''''''''''''''''''''''Exit thread
        ElseIf eClick.mButtons = Buttons.MoveAbsolute Then
            Dim pts As POINTAPI = ToScreen(eClick.x, eClick.y) 'Convert to screen coordinates
            If pts.X <> 0 Then eClick.x = pts.X ''''''If x point found then set
            If pts.Y <> 0 Then eClick.y = pts.Y ''''''If y point found then set
            MouseEvent(eClick.mButtons, eClick.x, eClick.y)  'Move cursor to click point
            Exit Sub '''''''''''''''''''''''''''''''''Exit thread
        End If
        If eClick.wFocus.Foreground = -1 Then Exit Sub 'Exit if return from GetWinHandles is negative
        If eClick.wFocus.Foreground = 0 Then eClick.wFocus = GetWinFocus(False, False) 'Get current focus
        hwnd = eClick.wFocus.Foreground ''''''''''''''Set main handle to something smaller
        cwnd = eClick.wFocus.Focus '''''''''''''''''''Set child handle(if any) to something smaller
        If cwnd = 0 Then cwnd = hwnd '''''''''''''''''If no child specified, then set it to the main window
        If apiIsIconic(hwnd) = True Then ws.IsIconic = apiShowWindow(hwnd, SW_SHOWNORMAL) : Sleep(25) 'if minimized then show normal
        If apiIsWindowEnabled(hwnd) = False Then ws.IsDisabled = apiEnableWindow(hwnd, True) : Sleep(25) 'If disabled then enable
        If apiIsWindowEnabled(cwnd) = False Then ws.IsChildDisabled = apiEnableWindow(cwnd, True) : Sleep(25) 'If child disabled then enable
        If apiIsWindowVisible(hwnd) = False Then ws.IsHidden = Not apiShowWindow(hwnd, SW_SHOWNORMAL) : Sleep(25) 'If hidden then show
        If apiIsWindowVisible(cwnd) = False Then ws.IsChildHidden = Not apiShowWindow(cwnd, SW_SHOWNORMAL) : Sleep(25) 'If child hidden then show
        zOrder = GetSetZOrder(hwnd) ''''''''''''''''''Remember main window's place in the z-order
        zOrderChild = GetSetZOrder(cwnd) '''''''''''''Remember child window's place in the z-order
        If hwnd <> apiGetTopWindow(HWND_DESKTOP) Then apiSetWindowPos(hwnd, HWND_TOPMOST, 0, 0, 0, 0, SWP_NOACTIVATE + SWP_SHOWWINDOW + SWP_NOMOVE + SWP_NOSIZE)
        If cwnd <> apiGetTopWindow(apiGetParent(cwnd)) Then apiSetWindowPos(cwnd, HWND_TOP, 0, 0, 0, 0, SWP_NOACTIVATE + SWP_SHOWWINDOW + SWP_NOMOVE + SWP_NOSIZE)
        If eClick.mButtons = 12 OrElse eClick.mButtons = 48 OrElse eClick.mButtons = 768 Then repeat = 1 'If double click
        If eClick.mDown = True AndAlso eClick.mUp = False Then 'If mouse down
            If eClick.mButtons = MOUSEEVENTF_LEFTDOWN + MOUSEEVENTF_LEFTUP Then 'If left click
                eClick.mButtons = MOUSEEVENTF_LEFTDOWN 'Set as left down
            ElseIf eClick.mButtons = MOUSEEVENTF_RIGHTDOWN + MOUSEEVENTF_RIGHTUP Then 'If right click
                eClick.mButtons = MOUSEEVENTF_RIGHTDOWN 'Set as right down
            ElseIf eClick.mButtons = MOUSEEVENTF_MIDDLEDOWN + MOUSEEVENTF_MIDDLEUP Then 'If middle click
                eClick.mButtons = MOUSEEVENTF_MIDDLEDOWN 'Set as middle down
            End If
        ElseIf eClick.mDown = False AndAlso eClick.mUp = True Then 'If mouse up
            If eClick.mButtons = MOUSEEVENTF_LEFTDOWN + MOUSEEVENTF_LEFTUP Then 'If left click
                eClick.mButtons = MOUSEEVENTF_LEFTUP 'Set as left up
            ElseIf eClick.mButtons = MOUSEEVENTF_RIGHTDOWN + MOUSEEVENTF_RIGHTUP Then 'If right click
                eClick.mButtons = MOUSEEVENTF_RIGHTUP 'Set as right up
            ElseIf eClick.mButtons = MOUSEEVENTF_MIDDLEDOWN + MOUSEEVENTF_MIDDLEUP Then 'If middle click
                eClick.mButtons = MOUSEEVENTF_MIDDLEUP 'Set as middle up
            End If
        End If
        If eClick.mButtons <> Buttons.Wheel AndAlso eClick.mButtons <> Buttons.VirtualDesk Then 'If it's a click
            Dim pts As New POINTAPI, r As New RECT
            If apiGetWindowRect(cwnd, r) = False Then 'If no rectangle is found
                MouseAbort(ws, rCursor, hwnd, cwnd, zOrder, zOrderChild, p) 'If rectangle not found, then exit failure
                Exit Sub '''''''''''''''''''''''''''''Exit
            End If
            pts.X = CInt(r.rLeft + ((r.rRight - r.rLeft) / 2)) 'Set to the center of the horizon
            pts.Y = CInt(r.rTop + ((r.rBottom - r.rTop) / 2)) 'Set to the center of the vertical
            pts = ToScreen(pts.X, pts.Y) '''''''''''''Convert to screen coordinates
            rCursor = True '''''''''''''''''''''''''''Cursor position changed, remember to return it later
            eClick.x = 0 '''''''''''''''''''''''''''''null for click
            eClick.y = 0 '''''''''''''''''''''''''''''null for click
            apiGetCursorPos(p) '''''''''''''''''''''''Get the current cursor position, to be returned later
            If MouseEvent(Buttons.MoveAbsolute, pts.X, pts.Y) = False Then 'Move cursor to click point
                MouseAbort(ws, rCursor, hwnd, cwnd, zOrder, zOrderChild, p) 'Abot if fails to move
                Exit Sub '''''''''''''''''''''''''''''Exit thread
            End If
        End If
        For i As Int32 = 1 To repeat + 1 '''''''''''''Loop the number of repeats
            If MouseEvent(eClick.mButtons, eClick.x, eClick.y) = False Then 'Do mouse event repeated number of times
                MouseAbort(ws, rCursor, hwnd, cwnd, zOrder, zOrderChild, p) 'Abort if failure
                Exit Sub '''''''''''''''''''''''''''''Exit thread
            End If
        Next
        MouseAbort(ws, rCursor, hwnd, cwnd, zOrder, zOrderChild, p) 'Final abort return without failure
    End Sub

    ''' <summary>
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Clicks on the specified menu item, by sending it a command message.
    ''' If asMessage is false then this function clicks on the specified menu item,
    ''' by simulating an entire chain of events.
    ''' _____________________________________________________________________________________________________________________________________________________________________________
    ''' Returns false if window or menu cannot be found.
    ''' If asMessage is false then this function returns false if a window or menu or rectangle cannot be found,
    ''' or an event fails to be simulated.
    ''' </summary>
    ''' <param name="asMessage">Send as window message, or as a mouse event.
    ''' It's recommended that you use True in this parameter.</param>
    ''' <param name="wName">Title or class name of the main window.</param>
    ''' <param name="wIndex">Index of the main window.</param>
    ''' <param name="mnuName1">Main menu title.</param>
    ''' <param name="mnuIndex1">Main menu index.</param>
    ''' <param name="mnuName2">(Optional) Sub menu title.</param>
    ''' <param name="mnuIndex2">(Optional) Sub menu index.</param>
    ''' <param name="mnuName3">(Optional) Sub menu title.</param>
    ''' <param name="mnuIndex3">(Optional) Sub menu index.</param>
    ''' <param name="mnuName4">(Optional) Sub menu title.</param>
    ''' <param name="mnuIndex4">(Optional) Sub menu index.</param>
    ''' <param name="mnuName5">(Optional) Sub menu title.</param>
    ''' <param name="mnuIndex5">(Optional) Sub menu index.</param>
    ''' <param name="mnuName6">(Optional) Sub menu title.</param>
    ''' <param name="mnuIndex6">(Optional) Sub menu index.</param>
    ''' <param name="mnuName7">(Optional) Sub menu title.</param>
    ''' <param name="mnuIndex7">(Optional) Sub menu index.</param>
    ''' <param name="mnuName8">(Optional) Sub menu title.</param>
    ''' <param name="mnuIndex8">(Optional) Sub menu index.</param>
    ''' <param name="mnuName9">(Optional) Sub menu title.</param>
    ''' <param name="mnuIndex9">(Optional) Sub menu index.</param>
    ''' <param name="mnuName10">Sub menu title.</param>
    ''' <param name="mnuIndex10">(Optional) Sub menu index.</param>
    ''' <returns></returns>
    ''' <remarks></remarks>
    Public Function ClickMenu(ByVal asMessage As Boolean, ByVal wName As String, ByVal wIndex As Int32, ByVal mnuName1 As String, Optional ByVal mnuIndex1 As Int32 = 1, Optional ByVal mnuName2 As String = " ", Optional ByVal mnuIndex2 As Int32 = 1, Optional ByVal mnuName3 As String = " ", Optional ByVal mnuIndex3 As Int32 = 1, Optional ByVal mnuName4 As String = " ", Optional ByVal mnuIndex4 As Int32 = 1, Optional ByVal mnuName5 As String = " ", Optional ByVal mnuIndex5 As Int32 = 1, Optional ByVal mnuName6 As String = " ", Optional ByVal mnuIndex6 As Int32 = 1, Optional ByVal mnuName7 As String = " ", Optional ByVal mnuIndex7 As Int32 = 1, Optional ByVal mnuName8 As String = " ", Optional ByVal mnuIndex8 As Int32 = 1, Optional ByVal mnuName9 As String = " ", Optional ByVal mnuIndex9 As Int32 = 1, Optional ByVal mnuName10 As String = " ", Optional ByVal mnuIndex10 As Int32 = 1) As Boolean
        If asMessage = True Then
            Dim hwnd, hMenu, hSubMenu, hId, nPos, n As Int32, mnuNames(1) As String, ws As New WINSTATE
            hwnd = GetWinHandles(wName, wIndex).Foreground 'Set handle to specification
            If hwnd = 0 Then Return False ''''''''''''If handle not found then exit failure
            If apiIsIconic(hwnd) = True Then ws.IsIconic = apiShowWindow(hwnd, SW_SHOWNORMAL) : Sleep(25) 'If minimized then show it
            If apiIsWindowEnabled(hwnd) = False Then ws.IsDisabled = apiEnableWindow(hwnd, True) : Sleep(25) 'If disabled then enabled it
            If apiIsWindowVisible(hwnd) = False Then ws.IsHidden = Not apiShowWindow(hwnd, SW_SHOWNORMAL) : Sleep(25) 'If Hidden then show it
            hMenu = apiGetMenu(hwnd) '''''''''''''''''Set handle of main menu
            If hMenu = 0 Then Return False '''''''''''If handle not found then exit failure
            hSubMenu = hMenu '''''''''''''''''''''''''Initialize sub menu to main menu
            If mnuName1 <> " " Then n = 1 : ReDim mnuNames(n) 'Re-dimension mnuNames array
            If mnuName2 <> " " Then n = 3 : ReDim mnuNames(n)
            If mnuName3 <> " " Then n = 5 : ReDim mnuNames(n)
            If mnuName4 <> " " Then n = 7 : ReDim mnuNames(n)
            If mnuName5 <> " " Then n = 9 : ReDim mnuNames(n)
            If mnuName6 <> " " Then n = 11 : ReDim mnuNames(n)
            If mnuName7 <> " " Then n = 13 : ReDim mnuNames(n)
            If mnuName8 <> " " Then n = 15 : ReDim mnuNames(n)
            If mnuName9 <> " " Then n = 17 : ReDim mnuNames(n)
            If mnuName10 <> " " Then n = 19 : ReDim mnuNames(n)
            If n > 0 Then mnuNames(0) = mnuName1 : mnuNames(1) = mnuIndex1.ToString 'Set elements of array to specification
            If n > 2 Then mnuNames(2) = mnuName2 : mnuNames(3) = mnuIndex2.ToString
            If n > 4 Then mnuNames(4) = mnuName3 : mnuNames(5) = mnuIndex3.ToString
            If n > 6 Then mnuNames(6) = mnuName4 : mnuNames(7) = mnuIndex4.ToString
            If n > 8 Then mnuNames(8) = mnuName5 : mnuNames(9) = mnuIndex5.ToString
            If n > 10 Then mnuNames(10) = mnuName6 : mnuNames(11) = mnuIndex6.ToString
            If n > 12 Then mnuNames(12) = mnuName7 : mnuNames(13) = mnuIndex7.ToString
            If n > 14 Then mnuNames(14) = mnuName8 : mnuNames(15) = mnuIndex8.ToString
            If n > 16 Then mnuNames(16) = mnuName9 : mnuNames(17) = mnuIndex9.ToString
            If n > 18 Then mnuNames(18) = mnuName10 : mnuNames(19) = mnuIndex10.ToString
            For i As Int32 = 0 To (n - 1) Step 2 '''''Loop through menu tree
                If mnuNames(i) = "" Then '''''''''''''If menu name not specified then 
                    nPos = CInt(mnuNames(i + 1)) - 1 'Set position as an index
                Else '''''''''''''''''''''''''''''''''Then name was set
                    nPos = FindMenuItemPos(hSubMenu, mnuNames(i), CInt(mnuNames(i + 1))) 'Set position to name and index of specified item.
                End If
                If nPos <> -1 AndAlso apiGetSubMenu(hSubMenu, nPos) <> 0 Then hSubMenu = apiGetSubMenu(hSubMenu, nPos) 'If sub menu exits, and is specified then get the handle
            Next
            If nPos <> -1 Then '''''''''''''''''''''''If final item has a valid position 
                hId = apiGetMenuItemID(hSubMenu, nPos) 'Get menu id
                If hId <> -1 Then ''''''''''''''''''''If item has no sub menus
                    ClickMenu = Not CBool(apiSendMessage(hwnd, WM_COMMAND, hId, Nothing)) 'Send command message
                Else
                    ClickMenu = False ''''''''''''''''Return failure
                End If
            End If
            If ws.IsIconic = True Then Sleep(25) : apiShowWindow(hwnd, SW_SHOWMINIMIZED) 'If window was minimized then re-minimize it
            If ws.IsDisabled = True Then Sleep(25) : apiEnableWindow(hwnd, False) 'If window was disabled then re-disable it
            If ws.IsHidden = True Then Sleep(25) : apiShowWindow(hwnd, SW_HIDE) 'If window was hidden then re-hide it
        Else
            Dim mnuClickThread As New System.Threading.Thread(AddressOf ClickMenuEventThread) 'Dimension a thread for the set of menu events
            Dim n As Int32, mnuNames(1) As String
            If mnuName1 <> " " Then n = 3 : ReDim mnuNames(n) 'Re-dimension mnuNames array
            If mnuName2 <> " " Then n = 5 : ReDim mnuNames(n)
            If mnuName3 <> " " Then n = 7 : ReDim mnuNames(n)
            If mnuName4 <> " " Then n = 9 : ReDim mnuNames(n)
            If mnuName5 <> " " Then n = 11 : ReDim mnuNames(n)
            If mnuName6 <> " " Then n = 13 : ReDim mnuNames(n)
            If mnuName7 <> " " Then n = 15 : ReDim mnuNames(n)
            If mnuName8 <> " " Then n = 17 : ReDim mnuNames(n)
            If mnuName9 <> " " Then n = 19 : ReDim mnuNames(n)
            If mnuName10 <> " " Then n = 21 : ReDim mnuNames(n)
            If n > 0 Then mnuNames(0) = wName : mnuNames(1) = wIndex.ToString 'Set elements of array to specification
            If n > 2 Then mnuNames(2) = mnuName1 : mnuNames(3) = mnuIndex1.ToString
            If n > 4 Then mnuNames(4) = mnuName2 : mnuNames(5) = mnuIndex2.ToString
            If n > 6 Then mnuNames(6) = mnuName3 : mnuNames(7) = mnuIndex3.ToString
            If n > 8 Then mnuNames(8) = mnuName4 : mnuNames(9) = mnuIndex4.ToString
            If n > 10 Then mnuNames(10) = mnuName5 : mnuNames(11) = mnuIndex5.ToString
            If n > 12 Then mnuNames(12) = mnuName6 : mnuNames(13) = mnuIndex6.ToString
            If n > 14 Then mnuNames(14) = mnuName7 : mnuNames(15) = mnuIndex7.ToString
            If n > 16 Then mnuNames(16) = mnuName8 : mnuNames(17) = mnuIndex8.ToString
            If n > 18 Then mnuNames(18) = mnuName9 : mnuNames(19) = mnuIndex9.ToString
            If n > 20 Then mnuNames(20) = mnuName10 : mnuNames(21) = mnuIndex10.ToString
            ReDim mnuClickNames(mnuNames.Length - 1) 'Re-dimension the size of the global array
            mnuClickNames = mnuNames '''''''''''''''''Set array to the newly created one.
            mnuClickThread.Start() '''''''''''''''''''Start menu click thread
            ClickMenu = True '''''''''''''''''''''''''Signal true indicating that the thread was started
        End If
    End Function
    Private Sub ClickMenuEventThread()
        Dim i, nPos, rOffset, tOffset, LeftMost, TopMost, ArrayLength As Int32, isRect As Boolean
        Dim m As New MENUINFO, mi As New ITEMINFO, p, poi As New POINTAPI, r As New RECT, ws As New WINSTATE
        ArrayLength = mnuClickNames.Length - 1 '''''''Set length of array
        m.hwnd = GetWinHandles(mnuClickNames(0)).Foreground 'Get the handle of the specified window
        If m.hwnd = 0 Then Exit Sub ''''''''''''''''''If window not found, then exit failure
        If apiIsIconic(m.hwnd) = True Then ws.IsIconic = apiShowWindow(m.hwnd, SW_SHOWNORMAL) : Sleep(25) 'If minimized then show it
        If apiIsWindowEnabled(m.hwnd) = False Then ws.IsDisabled = apiEnableWindow(m.hwnd, True) : Sleep(25) 'If disabled then enable it
        If apiIsWindowVisible(m.hwnd) = False Then ws.IsHidden = Not apiShowWindow(m.hwnd, SW_SHOWNORMAL) : Sleep(25) 'If hidden then show it
        isRect = apiGetWindowRect(m.hwnd, r) '''''''''Set confirmation of window rectangle
        If isRect = True Then apiMoveWindow(m.hwnd, 0, 0, r.rRight - r.rLeft, r.rBottom - r.rTop, True) : Sleep(25) 'If rectangle found then move window with coordinates
        apiGetCursorPos(poi) '''''''''''''''''''''''''Get the current position of the user's cursor, so that it can be returned
        m.hMenu = apiGetMenu(m.hwnd) '''''''''''''''''Set handle of the main menu
        If m.hMenu = 0 Then Exit Sub '''''''''''''''''If no handle found then exit sub with failure
        m.hSubMenu = apiGetSubMenu(m.hMenu, 0) '''''''Set handle of the first sub menu if any
        mi = MenuItemDim(m.hwnd, m.hMenu, 0) '''''''''Get the dimensions of the menu item
        If mi.Top = -1 AndAlso mi.Bottom = -1 AndAlso mi.Left = -1 AndAlso mi.Right = -1 Then Exit Sub 'Exit upon negative results
        LeftMost = mi.Left '''''''''''''''''''''''''''Initialize the left most coordinate
        If mnuClickNames(2) = "" Then ''''''''''''''''If no name specified
            nPos = CInt(mnuClickNames(3)) - 1 ''''''''Set position by index only
        Else '''''''''''''''''''''''''''''''''''''''''Otherwise set position by name and index
            nPos = FindMenuItemPos(m.hMenu, mnuClickNames(2), CInt(mnuClickNames(3))) 'Find position of the menu item
        End If
        If nPos = -1 Then Exit Sub '''''''''''''''''''Exit upon failure
        mi = MenuItemDim(m.hwnd, m.hMenu, nPos) ''''''Get item  dimensions
        If mi.Top = -1 AndAlso mi.Bottom = -1 AndAlso mi.Left = -1 AndAlso mi.Right = -1 Then Exit Sub 'Exit upon failure
        p = ToScreen(mi.Center.X, mi.Center.Y) '''''''Convert point to screen coordinates
        If MouseEvent(MOUSEEVENTF_ABSOLUTE + MOUSEEVENTF_MOVE, p.X, p.Y) = False Then Exit Sub 'Move mouse, and exit if failure
        rOffset = mi.Left - LeftMost '''''''''''''''''Initialize offset from the left
        TopMost = mi.Bottom ''''''''''''''''''''''''''Initialize offset from the top
        If MouseEvent(MOUSEEVENTF_LEFTDOWN + MOUSEEVENTF_LEFTUP) = False Then Exit Sub 'Click mouse, and exit upon failure
        m.hSubMenu = apiGetSubMenu(m.hMenu, nPos) ''''Set handle of submenu
        If m.hSubMenu <> 0 Then ''''''''''''''''''''''If handle found
            If mnuClickNames(4) = "" Then ''''''''''''If no name specified
                nPos = CInt(mnuClickNames(5)) - 1 ''''Set by index only
            Else '''''''''''''''''''''''''''''''''''''Name and index specified
                nPos = FindMenuItemPos(m.hSubMenu, mnuClickNames(4), CInt(mnuClickNames(5))) 'Find by name and index
            End If
            If nPos = -1 Then Exit Sub '''''''''''''''Exit if position is invalid
            mi = MenuItemDim(m.hwnd, m.hSubMenu, nPos) 'Get dimensinos
            If mi.Top = -1 AndAlso mi.Bottom = -1 AndAlso mi.Left = -1 AndAlso mi.Right = -1 Then Exit Sub 'Exit if fails
            p = ToScreen(rOffset + mi.Center.X, mi.Center.Y) 'Convert point
            If MouseEvent(MOUSEEVENTF_ABSOLUTE + MOUSEEVENTF_MOVE, p.X, p.Y) = False Then Exit Sub 'Move and exit if failure
            For i = 6 To ArrayLength Step 2 ''''''''''Step through the array
                If MoveItemToItem(mnuClickNames(i), mnuClickNames(i + 1), TopMost, nPos, rOffset, tOffset, m, mi) = False Then Exit Sub
            Next
            If MouseEvent(MOUSEEVENTF_LEFTDOWN + MOUSEEVENTF_LEFTUP) = False Then Exit Sub 'Click final menu item in the chain, and exit if failure
        End If
        If isRect = True Then apiMoveWindow(m.hwnd, r.rLeft, r.rTop, r.rRight - r.rLeft, r.rBottom - r.rTop, True) 'If there was a rectangle move the window back to where it was
        apiSetCursorPos(poi.X, poi.Y) ''''''''''''''''Return the position of the cursor back to the user
        If ws.IsIconic = True Then Sleep(25) : apiShowWindow(m.hwnd, SW_SHOWMINIMIZED) 'If was minimized then re-minimize
        If ws.IsDisabled = True Then Sleep(25) : apiEnableWindow(m.hwnd, False) 'If was disabled then re-disable
        If ws.IsHidden = True Then Sleep(25) : apiShowWindow(m.hwnd, SW_HIDE) 'If was Hidden then re-Hide
    End Sub

    Private Function FindMenuItemPos(ByVal hMenu As Int32, Optional ByVal iName As String = "", Optional ByVal iIndex As Int32 = 1) As Int32
        Dim i, itemCount, indexCount, retValue As Int32, mnuCaption, woShortcut As String
        If apiIsMenu(hMenu) = False Then Return -1 ''Return negative result if it's not a menu handle
        FindMenuItemPos = NEGATIVE '''''''''''''''''''Set a default return value
        itemCount = apiGetMenuItemCount(hMenu) '''''''Count the number of menu items
        For i = 0 To itemCount - 1 '''''''''''''''''''Loop through all menu items
            mnuCaption = "" ''''''''''''''''''''''''''Initialize
            mnuCaption = mnuCaption.PadLeft(1024) ''''Pad with a buffer
            retValue = apiGetMenuString(hMenu, i, mnuCaption, mnuCaption.Length, 1024) 'Get menu caption
            mnuCaption = mnuCaption.Substring(0, retValue) 'Strip off buffer
            woShortcut = "" ''''''''''''''''''''''''''Initialize
            If mnuCaption.IndexOf("&") <> NEGATIVE Then woShortcut = mnuCaption.Remove(mnuCaption.IndexOf("&"), 1) 'If the & character exists, then remove it, so the developer doesn't have to specify
            If iName.ToLower = woShortcut.ToLower OrElse iName.ToLower = mnuCaption.ToLower Then 'if specified name matches menu name, as non-case sensitive
                FindMenuItemPos = i ''''''''''''''''''Set return value as that position
                indexCount += 1 ''''''''''''''''''''''Increment index by one
                If indexCount = iIndex Then Exit For 'If index matches the specification then exit loop
            End If
        Next
    End Function

    Private Function GetSetZOrder(ByVal hWnd As Int32, Optional ByVal sPosition As Int32 = NEGATIVE) As Int32
        Dim z, swnd As Int32
        swnd = apiGetWindow(hWnd, GW_HWNDFIRST) ''''''Get top or topmost window in context
        Do
            If sPosition = NEGATIVE Then '''''''''''''If not setting the z-order 
                If swnd = hWnd Then Return (z + 1) ''If handle specified matches sibling window, then return the position in the z-order
            Else '''''''''''''''''''''''''''''''''''''Then this function sets the z-order to the specified position
                If z = sPosition - 1 Then Return apiSetWindowPos(hWnd, swnd, 0, 0, 0, 0, SWP_NOACTIVATE + SWP_SHOWWINDOW + SWP_NOMOVE + SWP_NOSIZE)
            End If
            swnd = apiGetWindow(swnd, GW_HWNDNEXT) ''Get the next sibling window handle, in the loop
            If swnd = 0 Then Exit Do '''''''''''''''''If there are no more sibling windows, then exit loop with default return
            z += 1 '''''''''''''''''''''''''''''''''''Increment i by one
        Loop
        Return swnd
    End Function

    Private Function MenuItemDim(ByVal hWnd As Int32, ByVal hMenu As Int32, ByVal nPos As Int32) As ITEMINFO
        Dim m As New ITEMINFO, r As New RECT
        If apiGetMenuItemRect(hWnd, hMenu, nPos, r) = 0 Then 'If rectangle not found then set negative returns
            r.rTop = NEGATIVE ''''''''''''''''''''''''Fail
            r.rBottom = NEGATIVE '''''''''''''''''''''Fail
            r.rLeft = NEGATIVE '''''''''''''''''''''''Fail
            r.rRight = NEGATIVE ''''''''''''''''''''''Fail
        Else '''''''''''''''''''''''''''''''''''''''''Or set dimensions of menu item
            m.Width = (r.rRight - r.rLeft) '''''''''''Set width
            m.Height = (r.rBottom - r.rTop) ''''''''''Set height
            m.Center.X = CInt(r.rLeft + (m.Width / 2)) 'Set center point x
            m.Center.Y = CInt(r.rTop + (m.Height / 2)) 'Set center point y
        End If
        m.Left = r.rLeft '''''''''''''''''''''''''''''Set left coordinate
        m.Right = r.rRight '''''''''''''''''''''''''''Set right coordinate
        m.Top = r.rTop '''''''''''''''''''''''''''''''Set top coordinate
        m.Bottom = r.rBottom '''''''''''''''''''''''''Set bottom coordinate
        Return m
    End Function

    Private Function MouseAbort(ByVal WSTATE As WINSTATE, ByVal rCursor As Boolean, ByVal hwnd As Int32, ByVal cwnd As Int32, ByVal zOrder As Int32, ByVal zOrderChild As Int32, ByVal p As POINTAPI) As Boolean
        If rCursor = True Then apiSetCursorPos(p.X, p.Y) 'If it was a click then return cursor to user position
        If WSTATE.IsIconic = True Then Sleep(25) : apiShowWindow(hwnd, SW_SHOWMINIMIZED) 'If main window was minimized before, then re-minimize it
        If WSTATE.IsDisabled = True Then Sleep(25) : apiEnableWindow(hwnd, False) 'If main window was disabled before, then re-disable it
        If WSTATE.IsChildDisabled = True Then Sleep(25) : apiEnableWindow(cwnd, False) 'If child window was disabled before, then re-disable it
        If WSTATE.IsHidden = True Then Sleep(25) : apiShowWindow(hwnd, SW_HIDE) 'If main window was hidden before, then re-hide it
        If WSTATE.IsChildHidden = True Then Sleep(25) : apiShowWindow(cwnd, SW_HIDE) 'If main window was hidden before, then re-hide it
        apiSetWindowPos(hwnd, HWND_NOTOPMOST, 0, 0, 0, 0, SWP_NOACTIVATE + SWP_SHOWWINDOW + SWP_NOMOVE + SWP_NOSIZE) : Sleep(25) 'Set the window position to not topmost window.  TODO Get topmost status first, so original state can be restored.
        If zOrderChild > 0 Then Sleep(25) : GetSetZOrder(cwnd, zOrderChild) 'If a z-order for the child window was obtained, reset the z-order of the child window
        If zOrder > 0 Then Sleep(25) : GetSetZOrder(hwnd, zOrder) 'If a z-order for the main window was obtained, reset the z-order of the main window
        Return True ''''''''''''''''''''''''''''''''''Return when finished
    End Function

    Private Function MouseEvent(Optional ByVal mEvents As Int32 = 0, Optional ByVal x As Int32 = 0, Optional ByVal y As Int32 = 0) As Boolean
        Return apimouse_event(mEvents, x, y, 0, apiGetMessageExtraInfo) 'Return results
    End Function

    Private Function MoveItemToItem(ByVal wName As String, ByVal wName2 As String, ByVal tMost As Int32, ByRef nPos As Int32, ByRef rOffset As Int32, ByRef tOffset As Int32, ByRef m As MENUINFO, ByRef mi As ITEMINFO) As Boolean
        Dim p As New POINTAPI
        tOffset += mi.Top - tMost ''''''''''''''''''''Keep offset from top most
        rOffset += mi.Width ''''''''''''''''''''''''''Keep offset from left most
        If MouseEvent(MOUSEEVENTF_LEFTDOWN + MOUSEEVENTF_LEFTUP) = False Then Return False 'click sub menu, and return false if failure
        m.hSubMenu = apiGetSubMenu(m.hSubMenu, nPos) 'Get handle of submenu
        If wName = "" Then '''''''''''''''''''''''''''If name not specified
            nPos = CInt(wName2) - 1 ''''''''''''''''''Then the search is by index
        Else '''''''''''''''''''''''''''''''''''''''''Then the search is by name
            nPos = FindMenuItemPos(m.hSubMenu, wName, CInt(wName2)) 'Get position from handle and name
        End If
        If nPos = -1 Then Return False '''''''''''''''If return is negative then exit and return false
        mi = MenuItemDim(m.hwnd, m.hSubMenu, 0) ''''''Get menu item dimensions
        If mi.Top = -1 AndAlso mi.Bottom = -1 AndAlso mi.Left = -1 AndAlso mi.Right = -1 Then Return False 'Exit if there is a negative return
        p = ToScreen(rOffset + mi.Center.X, tOffset + mi.Center.Y) 'Covert to screen coordinates
        If MouseEvent(MOUSEEVENTF_ABSOLUTE + MOUSEEVENTF_MOVE, p.X, p.Y) = False Then Return False 'Move to new screen location
        mi = MenuItemDim(m.hwnd, m.hSubMenu, nPos) ''Get menu item dimensions
        If mi.Top = -1 AndAlso mi.Bottom = -1 AndAlso mi.Left = -1 AndAlso mi.Right = -1 Then Return False 'Exit upon negative result
        p = ToScreen(rOffset + mi.Center.X, tOffset + mi.Center.Y) 'Convert to screen
        If MouseEvent(MOUSEEVENTF_ABSOLUTE + MOUSEEVENTF_MOVE, p.X, p.Y) = False Then Return False 'Move to point
        Return True
    End Function

    Private Function ToScreen(ByVal x As Int32, ByVal y As Int32) As POINTAPI
        ToScreen.X = CInt(x * SM_FULLSCREEN / apiGetSystemMetrics(SM_CXSCREEN)) 'Set the return value for x.
        ToScreen.Y = CInt(y * SM_FULLSCREEN / apiGetSystemMetrics(SM_CYSCREEN)) 'Set the return value for y.
    End Function

#End Region

End Module
