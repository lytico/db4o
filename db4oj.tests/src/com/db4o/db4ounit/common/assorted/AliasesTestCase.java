/* Copyright (C) 2006  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.assorted;

import com.db4o.config.*;
import com.db4o.db4ounit.util.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;


public class AliasesTestCase extends AbstractDb4oTestCase implements OptOutDefragSolo {
	
	public static void main(String[] args) {
		new AliasesTestCase().runSolo();
	}
	
	
	private int id;
	
	private Alias alias;

	
	public static class AFoo{
		public String foo;
	}
	
	public static class ABar extends AFoo {
		public String bar;
	}
	
	public static class BFoo {
		public String foo;
	}
	
	public static class BBar extends BFoo {
		public String bar;
	}
	
	public static class CFoo{
		public String foo;
	}
	
	public static class CBar extends CFoo {
		public String bar;
	}
	
	protected void store() throws Exception{
		addACAlias();
		CBar bar = new CBar();
		bar.foo = "foo";
		bar.bar = "bar";
		store(bar);
		id = (int)db().getID(bar);
	}
	
	public void testAccessByChildClass() throws Exception{
		addABAlias();
		BBar bar = (BBar) retrieveOnlyInstance(BBar.class);
		assertInstanceOK(bar);
	}
	
	public void testAccessByParentClass() throws Exception{
		addABAlias();
		BBar bar = (BBar) retrieveOnlyInstance(BFoo.class);
		assertInstanceOK(bar);
	}
	
	public void testAccessById() throws Exception{
		addABAlias();
		BBar bar = (BBar) db().getByID(id);
		db().activate(bar, 2);
		assertInstanceOK(bar);
	}
	
	public void testAccessWithoutAlias() throws Exception{
		removeAlias();
		ABar bar = (ABar) retrieveOnlyInstance(ABar.class);
		assertInstanceOK(bar);
	}
	
	private void assertInstanceOK (BBar bar) {
		Assert.areEqual("foo", bar.foo);
		Assert.areEqual("bar", bar.bar);
	}
	
	private void assertInstanceOK (ABar bar) {
		Assert.areEqual("foo", bar.foo);
		Assert.areEqual("bar", bar.bar);
	}
	
	private void addABAlias() throws Exception{
		addAlias("A", "B");
	}
	
	private void addACAlias() throws Exception{
		addAlias("A", "C");
	}
	
	private void addAlias(String storedLetter, String runtimeLetter) throws Exception{
		removeAlias();
		alias = createAlias(storedLetter, runtimeLetter);
		fixture().configureAtRuntime(new RuntimeConfigureAction() {
			public void apply(Configuration config) {
				config.addAlias(alias);	
			}
		});
		reopen();
	}
	
	private void removeAlias() throws Exception{
		if(alias != null){
			fixture().configureAtRuntime(new RuntimeConfigureAction() {
				public void apply(Configuration config) {
					config.removeAlias(alias);
				}
			});
			alias = null;
		}
		reopen();

	}
	
	private WildcardAlias createAlias(String storedLetter, String runtimeLetter){
		String className = reflector().forObject(new ABar()).getName();
		String storedPattern = Strings.replace(className, "ABar", storedLetter + "*");
		String runtimePattern = Strings.replace(className, "ABar", runtimeLetter + "*");
		return new WildcardAlias(storedPattern, runtimePattern);
	}

}
