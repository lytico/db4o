#!/bin/bash
source $(dirname "$0")/setEnvironment.sh
$JAVA -classpath $LOCAL_CLASSPATH com.db4o.drs.versant.eventprocessor.EventProcessorApplication -client localhost -clientport $CLIENT_PORT -database $DATABASE_NAME -eventProcessorPort $EVENTPROCESSOR_PORT -logfile $LOG_FILE -server localhost -serverport $SERVER_PORT -verbose false &
