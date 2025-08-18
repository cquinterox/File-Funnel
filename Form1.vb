Imports System.IO
Imports System.ComponentModel

Public Class frmMain
    Dim intCopyCount As Integer 'Count of files copied
    Dim strExtension As String
    Dim intTotalFiles As Integer 'Total files to process
    
    Dim objWriter As StreamWriter
    Dim WithEvents bgWorker As New BackgroundWorker

    Private Sub btnSource_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnSource.Click
        fbdSource.ShowDialog()
    End Sub

    Private Sub btnDestination_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnDestination.Click
        fbdDestination.ShowDialog()
    End Sub

    Private Sub btnCopy_Click(ByVal sender As System.Object, ByVal e As System.EventArgs) Handles btnCopy.Click
        Try
            If (btnCopy.Text = "Copy") Then
                If (inputsValid()) Then
                    ' Initialize BackgroundWorker
                    bgWorker.WorkerReportsProgress = True
                    bgWorker.WorkerSupportsCancellation = True
                    
                    ' Reset counters and setup logging
                    intCopyCount = 0
                    intTotalFiles = 0
                    objWriter = New StreamWriter(Now.ToString("yyyy-MM-dd--hh-mm-ss") & ".log")
                    
                    ' Update UI and start background work
                    enableControls(False)
                    bgWorker.RunWorkerAsync()
                Else
                    MsgBox("Please make sure the source, destination, and extension are set correctly.", MsgBoxStyle.Critical, "Parameters Missing")
                End If
            ElseIf (btnCopy.Text = "Cancel") Then
                bgWorker.CancelAsync()
            End If
        Catch ex As Exception
            MsgBox("Error starting copy operation: " & ex.Message, MsgBoxStyle.Critical, "Error")
            enableControls(True)
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
    Private Sub bgWorker_DoWork(ByVal sender As Object, ByVal e As DoWorkEventArgs) Handles bgWorker.DoWork
        Try
            Dim dirSRC As New DirectoryInfo(fbdSource.SelectedPath)
            Dim dirDEST As New DirectoryInfo(fbdDestination.SelectedPath)

            ' First pass: count total files to enable progress reporting
            If Not countTargetFiles(dirSRC, strExtension, bgWorker, e) Then
                e.Cancel = True
                Return
            End If
            bgWorker.ReportProgress(0, "Counting files... Found " & intTotalFiles & " files to process")

            ' Second pass: copy files with progress reporting
            copyTargetFilesInDir(dirSRC, dirDEST, strExtension, bgWorker, e)

        Catch ex As Exception
            e.Result = "Error: " & ex.Message
        End Try
    End Sub
    
    Private Sub bgWorker_ProgressChanged(ByVal sender As Object, ByVal e As ProgressChangedEventArgs) Handles bgWorker.ProgressChanged
        ' Update UI with progress information (runs on UI thread)
        If TypeOf e.UserState Is String Then
            Me.Text = "File Funnel - " & e.UserState.ToString()
        End If
    End Sub
    
    Private Sub bgWorker_RunWorkerCompleted(ByVal sender As Object, ByVal e As RunWorkerCompletedEventArgs) Handles bgWorker.RunWorkerCompleted
        ' This runs on the UI thread when work is complete
        Try
            objWriter.Close()
        Catch
            ' Ignore close errors
        End Try
        
        enableControls(True)
        Me.Text = "File Funnel"
        
        If e.Cancelled Then
            MessageBox.Show("Copy operation was cancelled.", "Cancelled", MessageBoxButtons.OK, MessageBoxIcon.Information)
        ElseIf e.Error IsNot Nothing Then
            MessageBox.Show("Error during copy: " & e.Error.Message, "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        ElseIf TypeOf e.Result Is String AndAlso e.Result.ToString().StartsWith("Error") Then
            MessageBox.Show(e.Result.ToString(), "Error", MessageBoxButtons.OK, MessageBoxIcon.Error)
        Else
            MessageBox.Show("Done! [" & intCopyCount & "] files copied", "Complete", MessageBoxButtons.OK, MessageBoxIcon.Information)
        End If
    End Sub

    Public Function inputsValid()
        'Set up before hand for ease of validation
        Dim regEx As New System.Text.RegularExpressions.Regex("^\.\S+$", System.Text.RegularExpressions.RegexOptions.IgnoreCase)
        strExtension = txtExtension.Text

        'Make sure selected paths are set and the extension is in extension format
        If Not (fbdSource.SelectedPath = String.Empty) And Not (fbdDestination.SelectedPath = String.Empty) And regEx.IsMatch(strExtension) Then
            Return True 'validates
        Else
            Return False 'doesn't validate
        End If
    End Function

    Public Function countTargetFiles(ByVal src_dir As DirectoryInfo, ByVal ext As String, ByVal worker As BackgroundWorker, ByVal e As DoWorkEventArgs) As Boolean
        Try
            ' Check for cancellation at start
            If worker IsNot Nothing AndAlso worker.CancellationPending Then
                If e IsNot Nothing Then e.Cancel = True
                Return False
            End If

            Dim strDir As String() = Directory.GetFileSystemEntries(src_dir.FullName)
            Dim strEntry As String

            For Each strEntry In strDir
                ' Check for cancellation in main loop
                If worker IsNot Nothing AndAlso worker.CancellationPending Then
                    If e IsNot Nothing Then e.Cancel = True
                    Return False
                End If

                Dim dirInner As New DirectoryInfo(strEntry)
                If (dirInner.Exists) Then
                    If Not countTargetFiles(dirInner, ext, worker, e) Then
                        Return False
                    End If
                End If

                Dim file As New FileInfo(strEntry)
                If (file.Exists And String.Equals(file.Extension, ext, StringComparison.OrdinalIgnoreCase)) Then
                    intTotalFiles += 1
                End If
            Next
        Catch ex As Exception
            ' Ignore counting errors, just continue
        End Try
        Return True
    End Function

    Public Function copyTargetFilesInDir(ByVal src_dir As DirectoryInfo, ByVal dest_dir As DirectoryInfo, ByVal ext As String, ByVal worker As BackgroundWorker, ByVal e As DoWorkEventArgs) As Boolean
        Try
            ' Check for cancellation at start
            If worker.CancellationPending Then
                e.Cancel = True
                Return False
            End If

            Dim strDir As String() = Directory.GetFileSystemEntries(src_dir.FullName)
            Dim strEntry As String
            Dim strTempString As String

            For Each strEntry In strDir
                ' Check for cancellation in main loop
                If worker.CancellationPending Then
                    e.Cancel = True
                    Return False
                End If

                Dim dirInner As New DirectoryInfo(strEntry)
                If (dirInner.Exists) Then
                    If (chkKeepDirStruc.Checked) Then
                        Dim newDest As New DirectoryInfo(dest_dir.FullName & dirInner.FullName.ToString.Replace(src_dir.FullName, ""))
                        System.IO.Directory.CreateDirectory(newDest.FullName)
                        ' Check return value from recursive call
                        If Not copyTargetFilesInDir(dirInner, newDest, ext, worker, e) Then
                            Return False
                        End If
                    Else
                        ' Check return value from recursive call
                        If Not copyTargetFilesInDir(dirInner, dest_dir, ext, worker, e) Then
                            Return False
                        End If
                    End If
                End If

                Dim file As New FileInfo(strEntry)
                Dim newFile As New FileInfo(dest_dir.FullName & file.FullName.ToString.Replace(src_dir.FullName, ""))

                If (file.Exists And String.Equals(file.Extension, ext, StringComparison.OrdinalIgnoreCase) And Not newFile.Exists) Then
                    ' Check for cancellation before copying
                    If worker.CancellationPending Then
                        e.Cancel = True
                        Return False
                    End If

                    System.IO.File.Copy(file.FullName, dest_dir.FullName & file.FullName.ToString.Replace(src_dir.FullName, ""))
                    intCopyCount += 1

                    ' Report progress
                    Dim progressPercent As Integer = CInt((intCopyCount / intTotalFiles) * 100)
                    worker.ReportProgress(progressPercent, "Copied " & intCopyCount & " of " & intTotalFiles & " files - " & file.Name)

                    ' Log the copy
                    strTempString = file.FullName & " >> " & newFile.FullName
                    objWriter.WriteLine(intCopyCount & ". " & strTempString)
                    objWriter.Flush() ' Ensure log is written immediately
                End If
            Next
        Catch ex As Exception
            ' Log error but continue processing
            objWriter.WriteLine("Error processing " & src_dir.FullName & ": " & ex.Message)
            objWriter.Flush()
            Return False ' Return False on error to propagate cancellation
        End Try
        Return True
    End Function

    Private Sub LinkLabel1_LinkClicked(ByVal sender As System.Object, ByVal e As System.Windows.Forms.LinkLabelLinkClickedEventArgs) Handles LinkLabel1.LinkClicked
        System.Diagnostics.Process.Start("http://atikik.tumblr.com/")
    End Sub
End Class

