call %~dp0%setEnvironment.bat
%VERSANT_BIN%\makedb %DATABASE_NAME%@localhost
%VERSANT_BIN%\createdb %DATABASE_NAME%@localhost
%VERSANT_BIN%\startdb %DATABASE_NAME%@localhost
%VERSANT_BIN%\dbuser -add -n drs -passwd drs %DATABASE_NAME%@localhost
