/* Copyright (C) 2004 - 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.test;

import java.math.*;

import com.db4o.*;
import com.db4o.query.*;

/**
 */
@decaf.Ignore(decaf.Platform.JDK11)
public class StoreBigDecimal {
	public BigDecimal _bd;

	public void configure() {
		Db4o.configure().objectClass(BigInteger.class).storeTransientFields(true); // needed for JDK1.3
		Db4o.configure().objectClass(BigDecimal.class).storeTransientFields(true); // needed for JDK5
	}
	
	public void store() {
		StoreBigDecimal stored=new StoreBigDecimal();
		stored._bd=new BigDecimal("111.11");
		Test.store(stored);
	}
	
	public void testOne() {
		Query q=Test.query();
		q.constrain(StoreBigDecimal.class);
		ObjectSet r=q.execute();
		Test.ensureEquals(1, r.size());
		StoreBigDecimal stored=(StoreBigDecimal)r.next();
		Test.ensureEquals(new BigDecimal("111.11"),stored._bd);
	}
}
