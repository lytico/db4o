/* Copyright (C) 2010 Versant Inc. http://www.db4o.com */
package com.db4o.drs.test.versant;

import static db4ounit.extensions.util.Binary.*;

import com.db4o.drs.versant.*;
import com.db4o.foundation.*;

import db4ounit.*;

public class UuidConverterTestCase implements TestCase {

	// @see UuidConverter
	private static final long HIGHEST_POSSIBLE_DB4O_LONGPART = 0x01FFFFFFFFFF803FL;

	public void testDb4oVodDb4o() {
		assertConversionDb4oVodDb4o(0, 0);
		assertConversionDb4oVodDb4o(1, 1);
		assertConversionDb4oVodDb4o(0, 0x7FFF);
		assertConversionDb4oVodDb4o(HIGHEST_POSSIBLE_DB4O_LONGPART, 0);
		assertConversionDb4oVodDb4o(HIGHEST_POSSIBLE_DB4O_LONGPART, 0x7FFF);
		assertConversionDb4oVodDb4o(HIGHEST_POSSIBLE_DB4O_LONGPART, 0xFFFF);
	}
	
	public void testVodDb4oVod() {
		assertConversionVodDb4oVod(0);
		assertConversionVodDb4oVod(1);
		assertConversionVodDb4oVod(-1L);
		assertConversionVodDb4oVod(Long.MAX_VALUE);
		assertConversionVodDb4oVod(Long.MIN_VALUE);
	}	
	
	public void testConversionRoundTrips(){
		long[] ids = generateIds();
		for (int i = 1; i < ids.length; i++) {
			long converted = UuidConverter.convert64BitIdTo48BitId(ids[i]);
			Assert.isSmallerOrEqual(48, numberOfBits(converted));
			long roundTrip = UuidConverter.convert48BitIdTo64BitId(converted);
			Assert.areEqual(ids[i], roundTrip);
		}
	}	
	
	public void assertConversionDb4oVodDb4o(long uuid, long dbId) {
		long converted = UuidConverter.vodLoidFrom(dbId, uuid);
		long reconverted = UuidConverter.longPartFromVod(converted);
		
		Assert.areEqual(uuid, reconverted);
	}
	
	public void assertConversionVodDb4oVod(long loid) {
		long converted = UuidConverter.longPartFromVod(loid);
		long reconverted = UuidConverter.vodLoidFrom(UuidConverter.databaseId(loid), converted);
		
		Assert.areEqual(loid, reconverted);
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
}
