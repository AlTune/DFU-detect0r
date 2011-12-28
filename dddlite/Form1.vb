Imports System
Imports System.Management
Imports System.Windows.Forms
Public Class Form1
    Public iBoot As String = ""
    Public CPID As String = ""
    Public isbr As Boolean = False
    Public Sub GoGoGadgetiBoot()
        Dim icountMatch As Integer
        icountMatch = HighlightWords(console, "IBOOT", System.Drawing.Color.Red)
        If iBoot = "IBOOT-359.3]""" Then
            MsgBox("Bootrom: Old")
        Else
            MsgBox("Bootrom: New")
        End If
    End Sub
    Function HighlightWords(ByVal rtb As RichTextBox, ByVal sFindString As String, ByVal lColor As System.Drawing.Color) As Integer

        Dim iFoundPos As Integer 'Position of first character of match
        Dim iFindLength As Integer       'Length of string to find
        Dim iOriginalSelStart As Integer
        Dim iOriginalSelLength As Integer
        Dim iMatchCount As Integer      'Number of matches

        'Save the insertion points current location and length
        iOriginalSelStart = rtb.SelectionStart
        iOriginalSelLength = rtb.SelectionLength

        'Cache the length of the string to find
        iFindLength = Len(sFindString) + 16

        'Attempt to find the first match
        iFoundPos = rtb.Find(sFindString, 0, RichTextBoxFinds.NoHighlight)
        While iFoundPos > 0
            iMatchCount = iMatchCount + 1

            console.SelectionStart = iFoundPos
            'The SelLength property is set to 0 as soon as you change SelStart
            console.SelectionLength = iFindLength
            'rtb.SelectionBackColor = lColor

            console.Select(iFoundPos, iFindLength - 8)
            iBoot = console.SelectedText
            'MsgBox(iBoot)
            'Attempt to find the next match
            iFoundPos = rtb.Find(sFindString, iFoundPos + iFindLength, RichTextBoxFinds.NoHighlight)
        End While

        'Restore the insertion point to its original location and length
        rtb.SelectionStart = iOriginalSelStart
        rtb.SelectionLength = iOriginalSelLength

        'Return the number of matches
        HighlightWords = iMatchCount
    End Function
    Function HighlightWords2(ByVal rtb As RichTextBox, ByVal sFindString2 As String, ByVal lColor2 As System.Drawing.Color) As Integer

        Dim iFoundPos2 As Integer 'Position of first character of match
        Dim iFindLength2 As Integer       'Length of string to find
        Dim iOriginalSelStart2 As Integer
        Dim iOriginalSelLength2 As Integer
        Dim iMatchCount2 As Integer      'Number of matches

        'Save the insertion points current location and length
        iOriginalSelStart2 = rtb.SelectionStart
        iOriginalSelLength2 = rtb.SelectionLength

        'Cache the length of the string to find
        iFindLength2 = Len(sFindString2)

        'Attempt to find the first match
        iFoundPos2 = rtb.Find(sFindString2, 0, RichTextBoxFinds.NoHighlight)
        While iFoundPos2 > 0
            iMatchCount2 = iMatchCount2 + 1

            console.SelectionStart = iFoundPos2
            'The SelLength property is set to 0 as soon as you change SelStart
            console.SelectionLength = iFindLength2
            'rtb.SelectionBackColor = lColor2

            console.Select(iFoundPos2 + 5, iFindLength2 - 1)
            CPID = console.SelectedText
            'MsgBox(CPID)
            'Attempt to find the next match
            iFoundPos2 = rtb.Find(sFindString2, iFoundPos2 + iFindLength2, RichTextBoxFinds.NoHighlight)
        End While

        'Restore the insertion point to its original location and length
        rtb.SelectionStart = iOriginalSelStart2
        rtb.SelectionLength = iOriginalSelLength2

        'Return the number of matches
        HighlightWords2 = iMatchCount2
    End Function

    Private Sub Button1_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles Button2.Click
        Button2.Text = "Searching for DFU..."
        Button2.Enabled = False
        Dim searcher As New ManagementObjectSearcher( _
                    "root\CIMV2", _
                    "SELECT * FROM Win32_PnPDevice")

        For Each queryObj As ManagementObject In searcher.Get()
            console.Text += ("SystemElement: {0}" & queryObj("SystemElement"))
        Next
        If console.Text.Contains("ECID") Then
            Dim icountMatch2 As Integer
            'CPID List:
            'iPhone 2G, iPod Touch 1G and iPhone 3G = 8900
            'iPhone 3G[S] = 8920
            'iPhone 4, AppleTV 2,iPad 1G and iPod Touch 4G = 8930
            'iPod Touch 2G = 8720
            'iPod Touch 3G = 8922
            'iPad 2G = 8940
            icountMatch2 = HighlightWords2(console, "CPID:", System.Drawing.Color.Red)
            If CPID = "8920" Then
                MsgBox("iPhone 3G[S] detected" + vbNewLine)
                Call GoGoGadgetiBoot()
                isbr = True
            ElseIf CPID = "8930" Then
                MsgBox("A4 Core detected:" + vbNewLine + "iPhone 4 (GSM/CDMA)" + vbNewLine + _
                                   "iPod Touch 4G" + vbNewLine + "iPad 1G" + vbNewLine + "AppleTV 2" + vbNewLine)
            ElseIf CPID = "8900" Then
                MsgBox("First Generation detected:" + vbNewLine + "iPhone 2G" + vbNewLine + _
                                   "iPod Touch 2G" + vbNewLine + "iPhone 3G" + vbNewLine)
            ElseIf CPID = "8720" Then
                MsgBox("iPod Touch 2G detected" + vbNewLine)
            ElseIf CPID = "8922" Then
                MsgBox("iPod Touch 3G detected" + vbNewLine)
            ElseIf CPID = "8940" Then
                MsgBox("A5 Core detected:" + vbNewLine + "iPad 2" + vbNewLine)
            Else
                MsgBox("An unknown idevice detected CPID: " + CPID + vbNewLine)
            End If
        Else
            MsgBox("No iDevice Detected!")
        End If
        Button2.Enabled = True
        Button2.Text = "Search"
    End Sub

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        about.Show()
    End Sub
End Class
