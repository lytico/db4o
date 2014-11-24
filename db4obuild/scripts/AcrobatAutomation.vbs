'***********************************************************
' Copyright (C) 2004 - 2007 db4objects Inc. http://www.db4o.com 
'***********************************************************

Function Main()
	If WScript.Arguments.Count < 3 Then
	   WScript.Echo "Usage: HTML2PDF <source-file-name> <output-file-name> <path/to/html-file-list-file>"
	   Exit Function
	End If

'	sourceFile = PrependPath(WScript.Arguments(0))
	sourceFile = WScript.Arguments(0)
	outputFile = PrependPath(WScript.Arguments(1))
	htmlFileListFile = WScript.Arguments(2)	

	Call PrintHeader()
	
	WScript.Echo "> Source file: " & sourceFile
	WScript.Echo "> Output file: " & outputFile
	WScript.Echo "> HTML file list file: " & htmlFileListFile

	WScript.Echo "Launching Acrobat ..."
	
	Dim AcroApp
	Dim AVDoc
	Dim ReferencePDDoc
	Dim TempPDDoc

	Set AcroApp = CreateObject("AcroExch.App")
	Set ReferenceAVDoc = CreateObject("AcroExch.AVDoc")
	Set ReferencePDDoc = CreateObject("AcroExch.PDDoc")

	AcroApp.Hide
	'AcroApp.Show

	WScript.Echo "Starting PDF conversion ..."
	
	Call ReferenceAVDoc.Open(sourceFile, "") 

	If ReferenceAVDoc.IsValid Then
	    
	    Set ReferencePDDoc = ReferenceAVDoc.GetPDDoc
		
		Dim fileStream
		Set fileStream = OpenHoldingFileNamesFile(htmlFileListFile)

		Do While Not fileStream.AtEndOfStream
			htmlFilePath = fileStream.ReadLine()
			If len(htmlFilePath) > 0 Then	'ignore empty lines
				WScript.Echo "> Converting " & htmlFilePath
				
				Set TempAVDoc = CreateObject("AcroExch.AVDoc")	'we need a new object every iteration
				Call TempAVDoc.Open(htmlFilePath, "")
				If TempAVDoc.IsValid Then
					Set TempPDDoc = TempAVDoc.GetPDDoc
					ReferencePDDoc.InsertPages ReferencePDDoc.GetNumPages()-1, TempPDDoc, 0, TempPDDoc.GetNumPages(), 0
					TempPDDoc.Close
					Set TempPDDoc = nothing
				Else
					WScript.Echo "Failed to open HTML document: " & htmlFilePath
				End If
				TempAVDoc.Close True
				Set TempAVDoc = Nothing
			End If
		Loop
		
		WScript.Echo "Saving complete reference PDF ..."
		Dim saveMode
		saveMode = 33 'PDSaveFull Or PDSaveCollectGarbage
		ReferencePDDoc.Save saveMode , outputFile
	    ReferencePDDoc.Close
		WScript.Echo "Done!"
	    
	Else
		WScript.Echo "Failed to open input document"
	End If 'ReferenceAVDoc.IsValid

	ReferenceAVDoc.Close true

	'AcroApp.Hide
	AcroApp.Exit	'App needs to be hidden for Exit to work!
End Function

Function PrependPath(fileName)
	Set shell = CreateObject("WScript.Shell")
	PrependPath = shell.CurrentDirectory & "\" & fileName
End Function

Function PrintHeader()
	WScript.Echo "-------------------------------------------"
	WScript.Echo "Acrobat Automation to create reference PDF"
	WScript.Echo "-------------------------------------------"
End Function

Function OpenHoldingFileNamesFile(fileName)
   Set fso  = CreateObject("Scripting.FileSystemObject")
   Set OpenHoldingFileNamesFile = fso.OpenTExtFile(fileName)
End Function

Call Main()