/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.exceptions;

import com.db4o.ext.*;
import com.db4o.io.*;

public class ExceptionSimulatingStorage extends StorageDecorator {

	static class ExceptionTriggerCondition {
		public boolean _triggersException = false;
		public boolean _isClosed = false;
	}
	
	private final ExceptionFactory _exceptionFactory;

	private final ExceptionTriggerCondition _triggerCondition = new ExceptionTriggerCondition();

	public ExceptionSimulatingStorage(Storage storage, ExceptionFactory exceptionFactory) {
		super(storage);
		_exceptionFactory = exceptionFactory;
	}
	
	@Override
	protected Bin decorate(BinConfiguration config, Bin bin) {
		resetShutdownState();
		return new ExceptionSimulatingBin(bin, _exceptionFactory, _triggerCondition);
	}

	private void resetShutdownState() {
		_triggerCondition._isClosed = false;
	}

	public void triggerException(boolean exception) {
		resetShutdownState();
		_triggerCondition._triggersException = exception;
	}

	public boolean triggersException() {
		return this._triggerCondition._triggersException;
	}

	public boolean isClosed() {
		return _triggerCondition._isClosed;
	}
	
	static class ExceptionSimulatingBin extends BinDecorator {

		private final ExceptionFactory _exceptionFactory;
		private final ExceptionTriggerCondition _triggerCondition;
		
		ExceptionSimulatingBin(Bin bin, ExceptionFactory exceptionFactory, ExceptionTriggerCondition triggerCondition) {
			super(bin);
			_exceptionFactory = exceptionFactory;
			_triggerCondition = triggerCondition;
		}

		public int read(long pos, byte[] bytes, int length) throws Db4oIOException {
			if (triggersException()) {
				_exceptionFactory.throwException();
			} 
			return _bin.read(pos, bytes, length);
		}

		public void sync() throws Db4oIOException {
			if (triggersException()) {
				_exceptionFactory.throwException();
			} 
			_bin.sync();
		}
		
		@Override
		public void sync(Runnable runnable) {
			if (triggersException()) {
				_exceptionFactory.throwException();
			}
			_bin.sync(runnable);
		}

		public void write(long pos, byte[] buffer, int length) throws Db4oIOException {
			if (triggersException()) {
				_exceptionFactory.throwException();
			}
			_bin.write(pos, buffer, length);
		}

		public void close() throws Db4oIOException {
			_triggerCondition._isClosed = true;
			_bin.close();
			if(triggersException()) {
				_exceptionFactory.throwOnClose();
			}
		}

		public long length() throws Db4oIOException {
			if (triggersException()) {
				_exceptionFactory.throwException();
			}
			return _bin.length();
		}
		
		private boolean triggersException() {
			return _triggerCondition._triggersException;
		}
		
		
	}
}
