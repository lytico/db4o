#!/bin/sh
SCRIPT_DIR=`dirname $0`
java -classpath $SCRIPT_DIR/buildrename.jar:$SCRIPT_DIR/scala-library.jar com.db4o.buildrename.BuildPrepareConsoleMain $*
