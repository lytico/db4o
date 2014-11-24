/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.config.Configuration;
import com.db4o.internal.Const4;

import db4ounit.Assert;
import db4ounit.ConsoleTestRunner;
import db4ounit.extensions.AbstractDb4oTestCase;
import db4ounit.extensions.Db4oTestCase;
import db4ounit.extensions.fixtures.Db4oFixtureProvider;
import db4ounit.fixtures.FixtureBasedTestSuite;
import db4ounit.fixtures.FixtureProvider;
import db4ounit.fixtures.FixtureVariable;
import db4ounit.fixtures.SimpleFixtureProvider;

public class PersistStaticFieldValuesTestSuite extends FixtureBasedTestSuite implements Db4oTestCase {
	
	
	private static FixtureVariable<Integer> STACK_DEPTH = new FixtureVariable<Integer>("stackDepth");

	public static void main(String[] args) {
		new ConsoleTestRunner(PersistStaticFieldValuesTestSuite.class).run();
	}
	
	@Override
	public FixtureProvider[] fixtureProviders() {
		return new FixtureProvider[] {
				new Db4oFixtureProvider(),
				new SimpleFixtureProvider(STACK_DEPTH, 2, Const4.DEFAULT_MAX_STACK_DEPTH)
		};
	}

	@Override
	public Class[] testUnits() {
		return new Class[] {
				PersistStaticFieldValuesTestUnit.class
		};
	}

	
	public static class PersistStaticFieldValuesTestUnit extends AbstractDb4oTestCase {
	
	    public static class Data {
	        public static final PsfvHelper ONE = new PsfvHelper("ONE");
	        public static final PsfvHelper TWO = new PsfvHelper("TWO");
	        public static final transient PsfvHelper THREE = new PsfvHelper("THREE");
	        
	        /**
	         * field put here to simulate a setup order failure during
	         * ClassMetadata initialization.
	         * @sharpen.ignore
	         */
	        public static final Class CLASS = Data.class; 
	
	        public PsfvHelper one;
		    public PsfvHelper two;
		    public PsfvHelper three;
	    }    
	
	    protected void configure(Configuration config) {
	        config.objectClass(Data.class).persistStaticFieldValues();
	        config.maxStackDepth(STACK_DEPTH.value());
	    }
	    
	    protected void store(){
	        Data psfv = new Data();
	        psfv.one = Data.ONE;
	        psfv.two = Data.TWO;
	        psfv.three = Data.THREE; 
	        store(psfv);
	    }
	    
	    public void test(){
	        Data psfv = (Data)retrieveOnlyInstance(Data.class);
	        Assert.areSame(Data.ONE,psfv.one);
	        Assert.areSame(Data.TWO,psfv.two);
	        Assert.areNotSame(Data.THREE,psfv.three);
	    }
	    
	    public static class PsfvHelper{
	    	public String name;

			public PsfvHelper(String name) {
				super();
				this.name = name;
			}
			@Override
			public String toString() {
				// TODO Auto-generated method stub
				return "PsfvHelper["+name+"]";
			}
	    }
    
	}

}
