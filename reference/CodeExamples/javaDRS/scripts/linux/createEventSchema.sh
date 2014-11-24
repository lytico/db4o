#!/bin/bash
source $(dirname "$0")/setEnvironment.sh
$JAVA -classpath $LOCAL_CLASSPATH com.db4o.drs.versant.eventprocessor.CreateEventSchema $DATABASE_NAME
