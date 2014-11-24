#!/bin/bash
source $(dirname "$0")/setEnvironment.sh
LD_LIBRARY_PATH=$VERSANT_LIB:$VERSANT_SDK_LIB:$LD_LIBRARY_PATH $VERSANT_BIN/veddriver $DATABASE_NAME $VED_CONFIGFILE &
