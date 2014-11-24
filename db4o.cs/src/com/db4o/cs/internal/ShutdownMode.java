/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
package com.db4o.cs.internal;

public abstract class ShutdownMode {
	
	public static class NormalMode extends ShutdownMode {
		
		NormalMode() {
		}
		
		@Override
		public boolean isFatal() {
			return false;
		}
	}
	
	public static class FatalMode extends ShutdownMode {
		
		private Throwable _exc;
		
		FatalMode(Throwable exc) {
			_exc = exc;
		}
		
		public Throwable exc() {
			return _exc;
		}

		@Override
		public boolean isFatal() {
			return true;
		}
	}
	
	public final static ShutdownMode NORMAL = new NormalMode();
	
	public static ShutdownMode fatal(Throwable exc) {
		return new FatalMode(exc);
	}

	public abstract boolean isFatal();
	
	private ShutdownMode() {
	}
}