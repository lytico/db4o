/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre12.collections;

import java.util.*;

import db4ounit.*;
import db4ounit.extensions.*;

@decaf.Ignore(decaf.Platform.JDK11)
public class ByteArrayAsMapValueTestCase extends AbstractDb4oTestCase{
	
	public static class Item{
		
		public Map<String, Object> _map;
		
	}
	
	public static class ByteArrayHolder{
		
		byte[] _bytes;
		
		public ByteArrayHolder(byte[] bytes) {
			_bytes = bytes;
		
		}
	}
	
	public void test() throws Exception {
		Item item = new Item();
		item._map = new HashMap();
		Map<String, Object> map = item._map;
		store(item);
		long initialLength = fileSession().fileLength();
		map.put("one", new ByteArrayHolder(newByteArray()));
		store(map);
		db().commit();
		long lengthAfterStoringHolder = fileSession().fileLength();
		map.put("two", newByteArray());
		store(map);
		db().commit();
		long lengthAfterStoringByteArray = fileSession().fileLength();
		long increaseForHolder = lengthAfterStoringHolder - initialLength;
		long increaseForByteArray = lengthAfterStoringByteArray - lengthAfterStoringHolder;
		Assert.isSmaller(2, increaseForByteArray / increaseForHolder);
	}

	private byte[] newByteArray() {
		return new byte[1000];
	}
	

}
