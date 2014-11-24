@echo off
@echo Processing: %1

cscript.exe "%~dp0"\CustomAction_NoImpersonate.js "%1"
cscript.exe "%~dp0"\WiRunSQL.vbs "%1" "INSERT INTO `Error` (`Error`, `Message`) VALUES (1001, 'Error [1]: [2]')"
cscript.exe "%~dp0"\RemoveBannerText.vbs "%1"
cscript.exe "%~dp0"\BoldProductName.vbs "%1"