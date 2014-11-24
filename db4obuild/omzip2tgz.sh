#!/bin/sh
OMVERSION=1.8
cd objectmanager-builds
mkdir tmp
cd tmp
unzip ../objectmanager-$OMVERSION-linux.zip
chmod a+x *.sh
rm -f ../objectmanager-$OMVERSION-linux.tar.gz
rm -f ../objectmanager-$OMVERSION-linux.zip
tar -czvf ../objectmanager-$OMVERSION-linux.tar.gz *
cd ..
rm -rf ./tmp
cd ..
