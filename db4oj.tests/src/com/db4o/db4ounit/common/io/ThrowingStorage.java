package com.db4o.db4ounit.common.io;

import com.db4o.ext.*;
import com.db4o.io.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class ThrowingStorage extends StorageDecorator {

	private final ThrowCondition _condition;
	
	public ThrowingStorage(Storage storage, ThrowCondition condition) {
		super(storage);
		_condition = condition;
	}

	@Override
	protected Bin decorate(BinConfiguration config, Bin bin) {
		return new ThrowingBin(bin, _condition);
	}

	static class ThrowingBin extends BinDecorator {
		private final ThrowCondition _condition;

		public ThrowingBin(Bin bin, ThrowCondition condition) {
			super(bin);
			_condition = condition;
		}

		public void write(long pos, byte[] buffer, int length) throws Db4oIOException {
			if(_condition.shallThrow(pos, length)) {
				throw new Db4oIOException("FAIL");
			}
			_bin.write(pos, buffer, length);
		}
	}
}
