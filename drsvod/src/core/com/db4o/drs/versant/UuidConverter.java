/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.versant;

import com.db4o.foundation.*;

/**
 * Db4oUuid long part format:
 * 
 * | 1    |  6         | 42        | 9        | 6      | 
 * |unused| timestamp              | counter           |  
 * |      | zeroes     | used      | zeroes   | used   |    
 * 
 * Vod loid:
 * 
 * | 16   | 42         | 6          |
 * | dbid | timestamp  | counter    |
 * 
 * 
 * Conversion idea: Take the 6 bit from the timestamp and the 9 bits from the counter that are assumed to be unused in db4o and use them for the dbid on vod side. 
 * 
 */
public class UuidConverter {
	
	public static final int BITS_RESERVED_FOR_COUNTER_IN_48BIT_ID = 6;
	

	public static long vodLoidFrom(long databaseId, long db4oLongPart) {
		long vodObjectIdPart = convert64BitIdTo48BitId(db4oLongPart);
		return (databaseId << 48) | vodObjectIdPart;
	}
	
	public static long longPartFromVod(long vodId){
		return convert48BitIdTo64BitId(vodId & 0xFFFFFFFFFFFFL);
	}
	
	public static int databaseId(long value) {
		return (int)(value >>> 48);
	}

	public static long convert64BitIdTo48BitId(long id){
		return convert(
				id, 
				TimeStampIdGenerator.BITS_RESERVED_FOR_COUNTER, 
				BITS_RESERVED_FOR_COUNTER_IN_48BIT_ID);
	}
	
	public static long convert48BitIdTo64BitId(long id){
		return convert(
				id, 
				BITS_RESERVED_FOR_COUNTER_IN_48BIT_ID, 
				TimeStampIdGenerator.BITS_RESERVED_FOR_COUNTER);
	}

	private static long convert(long id, int shiftBitsFrom, int shiftBitsTo) {
		final long creationTimeInMillis = id >>> shiftBitsFrom;
		final long timeStampPart = creationTimeInMillis << shiftBitsFrom;
		final long counterPerMillisecond = id - timeStampPart;
		if(counterPerMillisecond >= TimeStampIdGenerator.COUNTER_LIMIT){
			throw new IllegalStateException("ID can't be converted");
		}
		return (creationTimeInMillis << shiftBitsTo) + counterPerMillisecond;
	}

}
