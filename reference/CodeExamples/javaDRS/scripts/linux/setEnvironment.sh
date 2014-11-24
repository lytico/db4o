#!/bin/bash
SCRIPT_DIR=$(dirname "$0")
#example: The imporatent configuration items
# Specify the directory where the DRS and db4o jars are
# This directory should contain the dRS distribution.
# Or more specific it needs the dRS-X.XX-core.jar, 
# db4o-X.XX-core-java5.jar or db4o-X.XX-core-java5.jar, commons-cli-1.2.jar
LIB_DIR=./lib

# Specify the VOD database name
DATABASE_NAME=dRSVodExample

# Set the directory where VOD is installed. For example "C:\Program Files\Versant\8"
VERSANT_ROOT=$VERSANT_ROOT

# This should work if the JAVA_HOME variable is set. 
# Otherwise change the command to point to the java executatble or set the JAVA_HOME variable
JAVA=java.exe
JAVA_HOME=$JAVA_HOME
#end example

LOG_FILE="drsLogFile.log"
VED_CONFIGFILE=$SCRIPT_DIR/config.ved.linux

SERVER_PORT=4000
CLIENT_PORT=4100
EVENTPROCESSOR_PORT=4088

DB4O_JAR=$LIB_DIR/*

VERSANT_BIN=$VERSANT_ROOT/bin
VERSANT_LIB=$VERSANT_ROOT/lib
VERSANT_SDK_LIB=$VERSANT_ROOT/sdk/lib

PATH=$PATH:$JAVA_HOME/bin:$VERSANT_BIN

JVI=$VERSANT_LIB/jvi80.jar
VOD_JDO=$VERSANT_LIB/vodjdo.jar
JDO_JAR=$VERSANT_LIB/jdo2-api-2.1.jar
ASM=$VERSANT_LIB/asm-all-3.1.jar


LOCAL_CLASSPATH=$VOD_JDO:$JVI:$JDO_JAR:$ASM:$DB4O_JAR: