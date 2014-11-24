#!/bin/sh

NUMMANAGERS=4
NUMCLIENTS=6
RT_CLASSPATH=bin:../freespace_scala/bin:../freespace_java/bin:/usr/local/scala/scala/share/scala/lib/scala-library.jar
for((mgrIdx=0;mgrIdx < $NUMMANAGERS;mgrIdx+=1)); do
	for((clientIdx=0;clientIdx < $NUMCLIENTS;clientIdx+=1)); do
		time java -ea -classpath $RT_CLASSPATH de.sangamon.freespace.main.FreespaceMain $mgrIdx $clientIdx
	done
done