/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test.versant.objectid;

import com.db4o.drs.versant.*;
import com.db4o.foundation.*;

import db4ounit.*;

import static db4ounit.extensions.util.Binary.*;

/**
 * @sharpen.remove
 */
public class VodObjectIdConversionTestCase implements TestCase {
	
	/**
	 * We wrote the following code to calculate TimeStampIdGenerator.COUNTER_LIMIT
	 * Keep the knowledge for future use!
	 */
	public void milliSecondsInYears(){
		
		long yearCount = 120;
		
		long milliseconds = 1;
		long seconds = milliseconds * 1000;
		long minutes = seconds * 60;
		long hour = minutes * 60;
		long day = hour * 24;
		long year = day * 365;
		long totalMilliseconds = year * yearCount;
		
		System.out.println("Total milliseconds in " + yearCount + " years: " + totalMilliseconds);
		
		int yearBits = numberOfBits(totalMilliseconds);
		System.out.println("Bits needed for " + yearCount + " years:" + yearBits);
		
		int vodBitsAvailable = 48;
		
		int leftOverBitsForCounter = vodBitsAvailable - yearBits;
		
		System.out.println("Bits left for counter: " + leftOverBitsForCounter);
		
		System.out.println("Maximum object count per millisecond: " + longForBits(leftOverBitsForCounter));
	}
	
	public void testRoundTripFromDb4o(){
		int count = 1000;
		TimeStampIdGenerator generator = new TimeStampIdGenerator();
		long[] ids = new long[count];
		for (int i = 0; i < count; i++) {
			ids[i] = generator.generate();
		}
		for (int i = 0; i < ids.length; i++) {
			Assert.areEqual(ids[i], roundTripDb4oVodDb4o(ids[i]));
		}
	}
	
	public void testRoundTripFromVod(){
		long long48Bit = longForBits(48);
		long id = 0;
		for (id = 0; id < long48Bit; id += 1717177) {
			Assert.areEqual(id, roundTripVodDb4oVod(id));
		}
		for (id = 0; id < 1000; id ++) {
			Assert.areEqual(id, roundTripVodDb4oVod(id));
		}
		for (id = long48Bit; id > long48Bit - 1000; id --) {
			Assert.areEqual(id, roundTripVodDb4oVod(id));
		}
	}
	
	public void testInvalidDb4oIdThrows(){
		long validId = new TimeStampIdGenerator().generate();
		
		// db4o shifts the timestamp by 15 bits, then counts
		// up to a limit of 63 (so we stay convertible to 48 bits)
		
		// Old db4o engines did not have this limit.
		// The counter could have gone to 32767.
		
		// These Ids would not be convertible to VOD.
		// Let's make sure we throw if we see them.
		
		final long invalidId = validId + 64;
		
		Assert.expect(IllegalStateException.class, new CodeBlock() {
			public void run() throws Throwable {
				UuidConverter.convert64BitIdTo48BitId(invalidId);
			}
		});
	}
	
	public long roundTripDb4oVodDb4o(long id){
		return vodIdTodb4oId(db4oIdToVodId(id));
	}
	
	public long roundTripVodDb4oVod(long id){
		return db4oIdToVodId(vodIdTodb4oId(id));
	}

	long db4oIdToVodId(long id){
		return UuidConverter.convert64BitIdTo48BitId(id);
	}
	
	long vodIdTodb4oId(long id){
		return UuidConverter.convert48BitIdTo64BitId(id);
	}

}
