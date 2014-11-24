call setEnvironment.bat
%VERSANT_BIN%\makedb %DATABASE_NAME%@localhost
%VERSANT_BIN%\createdb %DATABASE_NAME%@localhost
%VERSANT_BIN%\startdb %DATABASE_NAME%@localhost
