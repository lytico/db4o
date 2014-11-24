/* Copyright (C) 2004 - 2008  Versant Inc.  http://www.db4o.com

This file is part of the db4o open source object database.

db4o is free software; you can redistribute it and/or modify it under
the terms of version 2 of the GNU General Public License as published
by the Free Software Foundation and as clarified by db4objects' GPL 
interpretation policy, available at
http://www.db4o.com/about/company/legalpolicies/gplinterpretation/
Alternatively you can write to db4objects, Inc., 1900 S Norfolk Street,
Suite 350, San Mateo, CA 94403, USA.

db4o is distributed in the hope that it will be useful, but WITHOUT ANY
WARRANTY; without even the implied warranty of MERCHANTABILITY or
FITNESS FOR A PARTICULAR PURPOSE.  See the GNU General Public License
for more details.

You should have received a copy of the GNU General Public License along
with this program; if not, write to the Free Software Foundation, Inc.,
59 Temple Place - Suite 330, Boston, MA  02111-1307, USA. */
package com.db4o.drs.test;

import com.db4o.foundation.Closure4;
import com.db4o.reflect.Reflector;

import db4ounit.ReflectionTestSuiteBuilder;
import db4ounit.TestCase;

public class DrsTestSuiteBuilder extends ReflectionTestSuiteBuilder {
	
	private DrsFixture _fixtures;
	
	public DrsTestSuiteBuilder(DrsProviderFixture a, DrsProviderFixture b, Class clazz) {
		this(a, b, new Class[] { clazz }, null);
	}
	
	public DrsTestSuiteBuilder(DrsProviderFixture a, DrsProviderFixture b, Class clazz, Reflector reflector) {
		this(a, b, new Class[] { clazz }, reflector);
	}
	
	public DrsTestSuiteBuilder(DrsProviderFixture a, DrsProviderFixture b, Class[] classes, Reflector reflector) {
		super(appendDestructor(classes));
		_fixtures = new DrsFixture(a, b, reflector);
	}
	
	private static Class[] appendDestructor(Class[] classes){
		Class[] newClasses = new Class[classes.length + 1];
		System.arraycopy(classes, 0, newClasses, 0, classes.length);
		newClasses[newClasses.length - 1] = DrsFixtureDestructor.class;
		return newClasses;
	}
	
	public static class DrsFixtureDestructor implements TestCase {
		public void testFixtureDestruction(){
			DrsFixture fixturePair = DrsFixtureVariable.value();
			fixturePair.a.destroy();
			fixturePair.b.destroy();
		}
	}
	
	@Override
	protected Object withContext(Closure4 closure) {
		return DrsFixtureVariable.with(_fixtures, closure);
	}
	
}
