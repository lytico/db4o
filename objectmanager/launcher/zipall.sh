#!/bin/bash
export LINUXTAR=dist/objectmanager_linux.tar
rm -f $LINUXTAR ${LINUXTAR}.gz
find lib -type f|grep -v /windows/|xargs tar -cvf $LINUXTAR
tar -uvf $LINUXTAR scripts/linux/* # Dave, how do I tell tar to use no paths?
gzip $LINUXTAR
