#!/bin/bash
source $(dirname "$0")/setEnvironment.sh
$VERSANT_BIN/veddriver $DATABASE_NAME $VED_CONFIGFILE &
