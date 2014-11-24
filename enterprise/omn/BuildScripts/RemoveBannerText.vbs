
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
Wscript.Echo "Changing WelcomeForm BannerText..."
Dim query
query = "UPDATE `Control` SET `Control`.`Text` = '"
query = query & vbLf & "' WHERE ((`Control`.`Dialog_`='WelcomeForm'OR `Control`.`Dialog_`='ConfirmInstallForm' OR `Control`.`Dialog_`='FinishedForm' OR `Control`.`Dialog_`='UserExitForm' OR `Control`.`Dialog_`='FatalErrorForm' OR `Control`.`Dialog_`='MaintenanceForm' OR `Control`.`Dialog_`='ResumeForm' ) AND `Control`.`Control`='BannerText')" &_
 "OR (`Control`.`Dialog_`='ProgressForm' AND (`Control`.`Control`='RemoveBannerText' OR `Control`.`Control`='InstalledBannerText'))"
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