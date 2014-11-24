/* Copyright (C) 2007 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.jre5.generic;

import db4ounit.*;
import db4ounit.extensions.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class GenericStringTestCase extends AbstractDb4oTestCase {
	public static void main(String[] args) {
		new GenericStringTestCase().runAll();
	}

	public void test1() throws Exception {
		store(new StringWrapper1<String>("hello"));
		StringWrapper1 sw = (StringWrapper1) retrieveOnlyInstance(StringWrapper1.class);
		Assert.areEqual("hello", sw.str);
	}
	
	public void test2() throws Exception {
		store(new StringWrapper1<String>("hello"));
		reopen();
		StringWrapper1 sw = (StringWrapper1) retrieveOnlyInstance(StringWrapper1.class);
		Assert.areEqual("hello", sw.str);
	}
	
	public void test3() throws Exception {
		store(new StringWrapper2<String>("hello"));
		StringWrapper2 sw = (StringWrapper2) retrieveOnlyInstance(StringWrapper2.class);
		Assert.areEqual("hello", sw.str);
	}
	
	public void test4() throws Exception {
		store(new StringWrapper2<String>("hello"));
		reopen();
		StringWrapper2 sw = (StringWrapper2) retrieveOnlyInstance(StringWrapper2.class);
		Assert.areEqual("hello", sw.str);
	}

	static class StringWrapper1<T> {
		public T str;

		public StringWrapper1(T s) {
			str = s;
		}
	}

	static class StringWrapper2<T extends Comparable> {
		public T str;
		
		public StringWrapper2() {

		}

		public StringWrapper2(T s) {
			str = s;
		}
	}
}
