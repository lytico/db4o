/* Copyright (C) 2004 - 2005  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.acid;

import com.db4o.ext.*;
import com.db4o.io.*;


public class CrashSimulatingStorage extends StorageDecorator {
	
	private final String _fileName;
    
    CrashSimulatingBatch _batch;
        
    public CrashSimulatingStorage(Storage storage, String fileName) {
        super(storage);
        _batch = new CrashSimulatingBatch();
        _fileName = fileName;
    }
    
    @Override
    protected Bin decorate(BinConfiguration config, Bin bin) {
    	return new CrashSimulatingBin(bin, _batch, _fileName);
    }

    static class CrashSimulatingBin extends BinDecorator {
    	
    	private final String _fileName;
    	
    	private CrashSimulatingBatch _batch;
    	
        long _curPos;
  	
	    public CrashSimulatingBin(Bin bin, CrashSimulatingBatch batch, String fileName) {
			super(bin);
			_batch = batch;
			_fileName = fileName;
		}

		public int read(long pos, byte[] bytes, int length) throws Db4oIOException {
	        _curPos=pos;
	        int readBytes = super.read(pos, bytes, length);
	        if(readBytes > 0){
	            _curPos += readBytes;
	        }
	        return readBytes;
	    }
	
	    public void write(long pos, byte[] buffer, int length) throws Db4oIOException {
	        _curPos=pos;
	        super.write(pos, buffer, length);
	        byte[] copy=new byte[buffer.length];
	        System.arraycopy(buffer, 0, copy, 0, length);
	        _batch.add(_fileName, copy, _curPos, length);
	        _curPos+= length;
	    }
	    
	    public void sync() throws Db4oIOException {
	        super.sync();
	        _batch.sync();
	    }
	    
	    @Override
	    public void sync(final Runnable runnable) {
	    	super.sync(new Runnable() {
				public void run() {
					_batch.sync();
					runnable.run();
				}
			});
	    	_batch.sync();
	    }
    
    }
}
