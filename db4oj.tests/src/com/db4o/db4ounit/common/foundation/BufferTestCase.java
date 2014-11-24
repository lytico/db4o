/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
package com.db4o.db4ounit.common.foundation;

import com.db4o.internal.*;

import db4ounit.*;

public class BufferTestCase implements TestCase {

	private static final int READERLENGTH = 64;

	public void testCopy() {
		ByteArrayBuffer from=new ByteArrayBuffer(READERLENGTH);
		for(int i=0;i<READERLENGTH;i++) {
			from.writeByte((byte)i);
		}
		ByteArrayBuffer to=new ByteArrayBuffer(READERLENGTH-1);
		from.copyTo(to,1,2,10);
		
		Assert.areEqual(0,to.readByte());
		Assert.areEqual(0,to.readByte());
		for(int i=1;i<=10;i++) {
			Assert.areEqual((byte)i,to.readByte());
		}
		for(int i=12;i<READERLENGTH-1;i++) {
			Assert.areEqual(0,to.readByte());
		}
	}
	
}
