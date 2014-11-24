/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.foundation;

import com.db4o.foundation.*;

import db4ounit.*;

import static db4ounit.extensions.util.Binary.*;

public class TimeStampIdGeneratorTestCase implements TestCase {
	
	public void testObjectCounterPartOnlyUses6Bits(){
		
		long[] ids = generateIds();
		
		for (int i = 1; i < ids.length; i++) {
			Assert.isGreater(ids[i] - 1, ids[i]);
			long creationTime = TimeStampIdGenerator.idToMilliseconds(ids[i]);
			long timePart = TimeStampIdGenerator.millisecondsToId(creationTime);
			long objectCounter = ids[i] - timePart;
			
			// 6 bits
			Assert.isSmallerOrEqual(longForBits(6), objectCounter);
		}
	}

	private long[] generateIds() {
		int count = 500;
		TimeStampIdGenerator generator = new TimeStampIdGenerator();
		long[] ids = new long[count];
		for (int i = 0; i < ids.length; i++) {
			ids[i] = generator.generate(); 
		}
		return ids;
	}
	
	public void testContinousIncrement(){
		TimeStampIdGenerator generator = new TimeStampIdGenerator();
		assertContinousIncrement(generator);
	}

	private void assertContinousIncrement(TimeStampIdGenerator generator) {
		long oldId = generator.generate();
		for (int i = 0; i < 1000000; i++) {
			long newId = generator.generate();
			Assert.isGreater(oldId, newId);
			oldId = newId;
		}
	}
	
	public void testTimeStaysTheSame(){
		TimeStampIdGenerator generatorWithSameTime = new TimeStampIdGenerator(){
			protected long now() {
				return 1;
			};
		};
		assertContinousIncrement(generatorWithSameTime);
	}

}
