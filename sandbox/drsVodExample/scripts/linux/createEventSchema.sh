#!/bin/bash
source $(dirname "$0")/setEnvironment.sh
LD_LIBRARY_PATH=$VERSANT_LIB:$VERSANT_SDK_LIB:$LD_LIBRARY_PATH $JAVA -classpath $LOCAL_CLASSPATH com.db4o.drs.versant.eventprocessor.CreateEventSchema $DATABASE_NAME
