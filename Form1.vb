Imports System.IO
Imports System.Threading

Public Class frmMain
    Dim intCopyCount As Integer 'Count of files copied
    Dim strExtension As String

    Dim objWriter As StreamWriter

    Private Sub btnSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSource.Click
        fbdSource.ShowDialog()
    End Sub

    Private Sub btnDestination_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDestination.Click
        fbdDestination.ShowDialog()
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Try
            Dim appThread As New Thread(New ThreadStart(AddressOf startCopy)) 'For threading purposes. Tell the threading object which function. Must not include parameters : (
            objWriter = New StreamWriter(Now.ToString("yyyy-MM-dd--hh-mm-ss") & ".log") 'Log 
            intCopyCount = 0
            If (btnCopy.Text = "Copy") Then
                If (inputsValid()) Then
                    enableControls(False)
                    appThread.Start() ' start threaded process!
                    enableControls(True)
                    'appThread.Abort()
                Else
                    MsgBox("Please make sure the source, destination, and extension are set correctly.", MsgBoxStyle.Critical, "Parameters Missing")
                End If
            ElseIf (btnCopy.Text = "Cancel") Then
                enableControls(True)
                appThread.Abort() ' stop the thread process. 
                appThread = Nothing
                'End
            End If
        Catch ex As Exception
            ' future error logging 
        End Try
    End Sub
    Public Function enableControls(ByVal bool As Boolean)
        Select Case bool
            Case True
                btnSource.Enabled = True
                btnDestination.Enabled = True
                txtExtension.Enabled = True
                btnCopy.Text = "Copy"
            Case False
                btnSource.Enabled = False
                btnDestination.Enabled = False
                txtExtension.Enabled = False
                btnCopy.Text = "Cancel"
        End Select
        Return True
    End Function
    Public Function startCopy()
        Dim dirSRC As New DirectoryInfo(fbdSource.SelectedPath) 'set source
        Dim dirDEST As New DirectoryInfo(fbdDestination.SelectedPath) 'set destination
        copyTargetFilesInDir(dirSRC, dirDEST, strExtension) 'copyTargetFilesInDir(source, destination, extension)
        objWriter.Close() 'stop logging when done
        MessageBox.Show("Done! [" & intCopyCount & "] files copied") ' Done message
        'End
        Return True
    End Function

    Public Function inputsValid()
        'Set up before hand for ease of validation
        Dim regEx As New System.Text.RegularExpressions.Regex("^\.\S+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        strExtension = txtExtension.Text.ToUpper()

        'Make sure selected paths are set and the extension is in extension format
        If Not (fbdSource.SelectedPath = String.Empty) And Not (fbdDestination.SelectedPath = String.Empty) And regEx.IsMatch(strExtension) Then
            Return True 'validates
        Else
            Return False 'doesn't validate
        End If
    End Function

    Public Function copyTargetFilesInDir(ByVal src_dir As DirectoryInfo, ByVal dest_dir As DirectoryInfo, ByVal ext As String)
        'Dim objWriter As New StreamWriter("file_list.txt") // THIS MUST BE RUN OUTSIDE RECURSION FUNCTION
        Dim strDir As String() = Directory.GetFileSystemEntries(src_dir.FullName)
        Dim strEntry As String 'Generic entry for comparing filenames
        Dim strTempString As String ' For logging purposes primarily

        For Each strEntry In strDir 'For each directory
            Dim dirInner As New DirectoryInfo(strEntry)
            If (dirInner.Exists) Then ' If what we found is a directory then...do formula again in search for files
                If (chkKeepDirStruc.Checked) Then ' If the user wants to keep the directory structure
                    Dim newDest As New DirectoryInfo(dest_dir.FullName & dirInner.FullName.ToString.Replace(src_dir.FullName, ""))
                    System.IO.Directory.CreateDirectory(newDest.FullName)
                    copyTargetFilesInDir(dirInner, newDest, ext)
                Else
                    copyTargetFilesInDir(dirInner, dest_dir, ext)
                End If
            End If
            Dim file As New FileInfo(strEntry) 'ITS'S A FILE!
            'Set the location for the new file by changing it's original path for the new path
            Dim newFile As New FileInfo(dest_dir.FullName & file.FullName.ToString.Replace(src_dir.FullName, ""))
            'Check to see the file on hand is the type we were looking for then copy, count, log
            If (file.Exists And file.Extension.Equals(ext) And Not newFile.Exists) Then ' extension with period and makes sure new copy does not exist
                System.IO.File.Copy(file.FullName, dest_dir.FullName & file.FullName.ToString.Replace(src_dir.FullName, ""))
                intCopyCount += 1 'For file count
                strTempString = file.FullName & " >> " & newFile.FullName 'For logging primarily
                objWriter.WriteLine(intCopyCount & ". " & strTempString) 'Log copy
            End If
        Next
        'objWriter.Close() // THIS MUST BE RUN OUTSIDE RECURSION FUNCTION
        Return True
    End Function

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start("http://atikik.tumblr.com/")
    End Sub
End Class

