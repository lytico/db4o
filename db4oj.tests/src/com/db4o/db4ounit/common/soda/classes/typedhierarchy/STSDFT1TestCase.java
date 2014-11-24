/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.soda.classes.typedhierarchy;
import com.db4o.query.*;


/** SDFT: Same descendant field typed*/
public class STSDFT1TestCase extends com.db4o.db4ounit.common.soda.util.SodaBaseTestCase{
	
	public STSDFT1TestCase(){
	}
	
	public Object[] createData() {
		return new Object[]{
			new STSDFT1TestCase(),
			new STSDFT2(),
			new STSDFT2("str1"),
			new STSDFT2("str2"),
			new STSDFT3(),
			new STSDFT3("str1"),
			new STSDFT3("str3")
		};
	}
	
	public void testStrNull(){
		Query q = newQuery();
		q.constrain(new STSDFT1TestCase());
		q.descend("foo").constrain(null);
		
		expect(q, new int[] {0, 1, 4});
	}
	
	public void testStrVal(){
		Query q = newQuery();
		q.constrain(STSDFT1TestCase.class);
		q.descend("foo").constrain("str1");
		
		expect(q, new int[] {2, 5});
	}
	
	public void testOrValue(){
		Query q = newQuery();
		q.constrain(STSDFT1TestCase.class);
		Query foo = q.descend("foo");
		foo.constrain("str1").or(foo.constrain("str2"));
		
		expect(q, new int[] {2, 3, 5});
	}
	
	public void testOrNull(){
		Query q = newQuery();
		q.constrain(STSDFT1TestCase.class);
		Query foo = q.descend("foo");
		foo.constrain("str1").or(foo.constrain(null));
		
		expect(q, new int[] {0, 1, 2, 4, 5});
	}
	
	public void testTripleOrNull(){
		Query q = newQuery();
		q.constrain(STSDFT1TestCase.class);
		Query foo = q.descend("foo");
		foo.constrain("str1").or(foo.constrain(null)).or(foo.constrain("str2"));
		
		expect(q, new int[] {0, 1, 2,3, 4, 5});
	}

// work in progress
	
//	public void testOverConstrainedByClass(){
//		Query q = SodaTenewQuery();
//		q.constrain(STSDFT1TestCase.class).or(q.constrain(STSDFT2.class));
//		Query foo = q.descend("foo");
//		foo.constrain("str1").or(foo.constrain(null)).or(foo.constrain("str2"));
//		
//		SodaTeexpect(q, new int[] {0, 1, 2,3, 4, 5});
//	}
	
}

