REM #example: The imporatent configuration items
REM Specify the directory where the DRS and db4o jars are
REM This directory should contain the dRS distribution.
REM Or more specific it needs the dRS-X.XX-core.jar, 
REM db4o-X.XX-core-java5.jar or db4o-X.XX-core-java5.jar, commons-cli-1.2.jar
SET LIB_DIR=./lib

REM Specify the VOD database name
SET DATABASE_NAME=dRSVodExample

REM Set the directory where VOD is installed. For example "C:\Program Files\Versant\8"
SET VERSANT_ROOT=%VERSANT_ROOT%

REM This should work if the JAVA_HOME variable is set. 
REM Otherwise change the command to point to the java executatble or set the JAVA_HOME variable
SET JAVA=java
JAVA_HOME=$JAVA_HOME
REM #end example

SET VED_CONFIGFILE=config.ved.win
SET LOG_FILE="drsLogFile.log"

SET SERVER_PORT=4000
SET CLIENT_PORT=4100
SET EVENTPROCESSOR_PORT=4088

SET DB4O_JAR=%LIB_DIR%/*

SET VERSANT_BIN=%VERSANT_ROOT%\bin
SET VERSANT_LIB=%VERSANT_ROOT%\lib


SET PATH=%PATH%;%JAVA_HOME%\bin;%VERSANT_BIN%

SET JVI=%VERSANT_LIB%\jvi80.jar
SET VOD_JDO=%VERSANT_LIB%\vodjdo.jar
SET JDO_JAR=%VERSANT_LIB%\jdo2-api-2.1.jar
SET ASM=%VERSANT_LIB%\asm-all-3.1.jar


SET LOCAL_CLASSPATH=%VOD_JDO%;%JVI%;%JDO_JAR%;%ASM%;%DB4O_JAR%;
