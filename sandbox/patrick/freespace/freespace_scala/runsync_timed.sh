#!/bin/sh
NUMCLIENTS=6
for((clientIdx=0;clientIdx < $NUMCLIENTS;clientIdx+=1)); do
	time scala -classpath ../freespace_java/bin:bin de.sangamon.freespace.actors.sync.SyncActorFreespaceMain $clientIdx
done