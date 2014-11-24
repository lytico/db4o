$strComputer = "."
$colItems = Get-WmiObject -Class "Win32_ComputerSystem" -Namespace  "\root\cimv2" -ComputerName $strComputer
foreach($objItem in $colItems)
{
	Write-Host 
     Wscript.Echo "System Name: " & objItem.Name
     Wscript.Echo "Manufacturer: " & objItem.Manufacturer
     Wscript.Echo "Model: " & objItem.Model
     Wscript.Echo
}