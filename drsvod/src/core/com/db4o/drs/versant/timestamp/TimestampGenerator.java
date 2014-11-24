/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant.timestamp;

import com.db4o.drs.versant.*;
import com.db4o.drs.versant.metadata.*;
import com.db4o.foundation.*;

public class TimestampGenerator {
	
	private final VodCobraFacade _cobra;
	
	public TimestampGenerator(VodCobraFacade cobra) {
		_cobra = cobra;
	}
	
	private long _minimumNext;
	
	public long generate(){
		return _cobra.withLock(TimestampToken.class, new Closure4<Long>() {
			@Override
			public Long run() {
				TimestampToken timestampToken = _cobra.from(TimestampToken.class).single();
				long minValue = timestampToken.value() + 1;
				long timeValue = TimeStampIdGenerator.millisecondsToId(System.currentTimeMillis());
				long newValue = Math.max(_minimumNext, Math.max(minValue, timeValue));
				timestampToken.value(newValue);
				_cobra.store(timestampToken);
				_cobra.commit();
				return newValue;
			}
		});
	}
	
	public void setMinimumNext(long minimumNext) {
		_minimumNext = minimumNext;
	}

}
