#!/bin/bash
SCRIPT_DIR=$(dirname "$0")
EXAMPLE_HOME=$SCRIPT_DIR/../..
JAVA=java
DATABASE_NAME=dRSVodExample
VERSANT_BIN=$VERSANT_ROOT/bin
VERSANT_LIB=$VERSANT_ROOT/lib
VERSANT_SDK_LIB=$VERSANT_ROOT/sdk/lib
VED_CONFIGFILE=config.ved.linux
LOG_FILE="drsLogFile.log"
SERVER_PORT=4000
CLIENT_PORT=4100
EVENTPROCESSOR_PORT=4088
VED_CONFIGFILE=$SCRIPT_DIR/config.ved.linux
LOCAL_CLASSPATH="$(find $EXAMPLE_HOME/lib -iname "*.jar" | tr '\n' ':')$(find $VERSANT_LIB -iname "*.jar" | tr '\n' ':' | sed 's/:$//')"
