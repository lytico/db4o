
Option Explicit
If (Wscript.Arguments.Count < 1) Then
Wscript.Echo "Windows Installer utility to execute SQL queries against an installer database." &_
vbLf & " The 1st argument specifies the path to the MSI database, relative or full path"
Wscript.Quit 1
End If
Dim openMode : openMode = 1 'msiOpenDatabaseModeTransact
On Error Resume Next
Dim installer : Set installer = Wscript.CreateObject("WindowsInstaller.Installer") : CheckError
' Open database
Dim database : Set database = installer.OpenDatabase(Wscript.Arguments(0), openMode) : CheckError
Dim query
query = "UPDATE `Control` SET `Control`.`Text` = '{\VSI_MS_Sans_Serif13.0_1_0} Welcome to db4objects ObjectManager Enterprise for Visual Studio" 
query = query & vbLf & "' WHERE `Control`.`Dialog_`='WelcomeForm' AND `Control`.`Control`='WelcomeText'"
Dim view : Set view = database.OpenView(query) : CheckError
view.Execute : CheckError
database.Commit
Wscript.Echo "Done."
Wscript.Quit 0
Sub CheckError
Dim message, errRec
If Err = 0 Then Exit Sub
message = Err.Source & " " & Hex(Err) & ": " & Err.Description
If Not installer Is Nothing Then
Set errRec = installer.LastErrorRecord
If Not errRec Is Nothing Then message = message & vbLf & errRec.FormatText
End If
Wscript.Echo message
Wscript.Quit 2
End Sub