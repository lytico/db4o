#!/bin/bash
source $(dirname "$0")/setEnvironment.sh
$VERSANT_BIN/makedb $DATABASE_NAME
$VERSANT_BIN/createdb $DATABASE_NAME
$VERSANT_BIN/startdb $DATABASE_NAME
$VERSANT_BIN/dbuser -add -n drs -passwd drs $DATABASE_NAME
