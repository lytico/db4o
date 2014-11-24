Const ForReading = 1
Const ForWriting = 2

Set objFSO = CreateObject("Scripting.FileSystemObject")
Const ForAppending = 2
Dim objFSO:Set objFSO = CreateObject("Scripting.FileSystemObject")

LogFile = "exportme.log"
'Dim objLogFile:Set objLogFile = objFSO.CreateTextFile(logfile, 2, True)

objStartFolder = Wscript.Arguments.Item(0)

Set objFolder = objFSO.GetFolder(objStartFolder)
'objLogFile.Write objFolder.Path
'objLogFile.Writeline
Set colFiles = objFolder.Files
For Each objFile in colFiles
    If objFile.Name <> "welcome.htm" then
        ReplaceBodyTags(objFile)
    end if
    On Error Resume Next
Next


ShowSubfolders objFSO.GetFolder(objStartFolder)

Sub ShowSubFolders(Folder)
    For Each Subfolder in Folder.SubFolders
        'objLogFile.Write Subfolder.Path
        'objLogFile.Writeline
        Set objFolder = objFSO.GetFolder(Subfolder.Path)
        Set colFiles = objFolder.Files
        For Each objFile in colFiles
                ReplaceBodyTags(objFile)
            On Error Resume Next

        Next
        ShowSubFolders Subfolder
    Next
End Sub

Sub ReplaceBodyTags(strFileName)
    strOldText = "<body>"
    strNewText = "<body onload=" + Chr(34) + "prettyPrint()" + Chr(34) + ">"

    Set objFile1 = objFSO.OpenTextFile(strFileName, ForReading)

    strText = objFile1.ReadAll
    objFile1.Close
    strNewText = Replace(strText, strOldText, strNewText)

    Set objFile1 = objFSO.OpenTextFile(strFileName, ForWriting)
    objFile1.WriteLine strNewText
    objFile1.Close
'    objLogFile.Write objFile.Name
'            objLogFile.Writeline
End Sub

'objLogFile.Close