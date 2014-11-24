/* Copyright (C) 2004 - 2007 Versant Inc. http://www.db4o.com */

package com.db4o.bench.delaying;

import com.db4o.bench.timing.*;
import com.db4o.ext.*;
import com.db4o.io.*;


public class DelayingStorage extends StorageDecorator {

	private static Delays NO_DELAYS = new Delays(0,0,0);

	private Delays _delays;
	
	public DelayingStorage(Storage delegateAdapter) {
		this(delegateAdapter, NO_DELAYS);
	}
	
	public DelayingStorage(Storage delegateAdapter, Delays delays) {
		super(delegateAdapter);
		_delays = delays;
	}
	
	protected Bin decorate(BinConfiguration config, Bin bin) {
		return new DelayingBin(bin, _delays);
	}
	
	private static class DelayingBin extends BinDecorator {
	
		private NanoTiming _timing;		
		private Delays _delays;
		
		public DelayingBin(Bin bin, Delays delays)throws Db4oIOException {
			super(bin);
			_delays = delays;
			_timing = new NanoTiming();
		}

		public int read(long pos, byte[] bytes, int length) throws Db4oIOException {
			delay(_delays.values[Delays.READ]);
			return _bin.read(pos, bytes, length);
	    }

	    public void sync() throws Db4oIOException {
			delay(_delays.values[Delays.SYNC]);
	    	_bin.sync();
	    }

	    public void write(long pos, byte[] buffer, int length) throws Db4oIOException {
			delay(_delays.values[Delays.WRITE]);
	    	_bin.write(pos, buffer, length);
	    }
		
	    private void delay(long time) {
	    	_timing.waitNano(time);
	    }
	}
}
