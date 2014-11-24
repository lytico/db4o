/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.acid;

import java.io.*;

import com.db4o.ext.*;
import com.db4o.internal.transactionlog.*;

public class CrashSimulatingWrite {
	
	int _index;
    
    byte[] _data;
    long _offset;
    int _length;
    
    byte[] _lockFileData;
    byte[] _logFileData;
    
    public CrashSimulatingWrite(int index, byte[] data, long offset, int length, byte[] lockFileData, byte[] logFileData) {
    	_index = index;
        _data = data;
        _offset = offset;
        _length = length;
        _lockFileData = lockFileData;
        _logFileData = logFileData;
    }

    public void write(String path, RandomAccessFile raf, boolean writeTrash) throws IOException {
    	if(_offset == 0){
    		writeTrash = false;
    	}
        raf.seek(_offset);
        raf.write(bytesToWrite(_data, writeTrash), 0, _length);
        write(FileBasedTransactionLogHandler.lockFileName(path), _lockFileData, writeTrash);
        write(FileBasedTransactionLogHandler.logFileName(path), _logFileData, writeTrash);
    }
    
    public String toString(){
        return "" + _index + " A:(" + _offset + ") L:(" + _length + ")";
    }
    
    private void write(String fileName, byte[] bytes, boolean writeTrash){
    	if(bytes == null){
    		return;
    	}
    	try {
        	RandomAccessFile raf = new RandomAccessFile(fileName, "rw");
        	raf.write(bytesToWrite(bytes, writeTrash));
			raf.close();
		} catch (IOException e) {
			throw new Db4oException(e);
		}
    }
    
    private byte[] bytesToWrite(byte[] bytes, boolean writeTrash){
    	if(! writeTrash){
    		return bytes;
    	}
		byte[] trash = new byte[bytes.length];
		for (int i = 0; i < trash.length; i++) {
			trash[i] = (byte) (i + 100);
		}
		return trash;
    }

}
