/* Copyright (C) 2009  Versant Inc.   http://www.db4o.com */
package com.db4o.polepos.continuous;

import org.polepos.framework.*;

public enum MeasurementType {
	TIME {
		@Override
		public long value(Result result) {
			return result.getTime();
		}
	}, 
	MEMORY {
		@Override
		public long value(Result result) {
			return result.getMemory();
		}
	};
	
	public abstract long value(Result result);
}
