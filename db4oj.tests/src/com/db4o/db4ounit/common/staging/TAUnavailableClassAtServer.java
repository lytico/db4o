package com.db4o.db4ounit.common.staging;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.internal.activation.*;
import com.db4o.query.*;
import com.db4o.ta.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class TAUnavailableClassAtServer extends AbstractDb4oTestCase implements CustomClientServerConfiguration, OptOutAllButNetworkingCS {

	public static void main(String[] args) {
		new TAUnavailableClassAtServer().runNetworking();
	}
	
	public class ParentWithMultipleChilds {
		private Child[] _children = new Child[0];
		
		public ParentWithMultipleChilds(Child...children) {
			_children = children;
		}

		public Child[] children() {
			return _children;
		}
		
		public void children(Child[] children) {
			_children = children;
		}
	}
	
	public class ParentWithSingleChild {
		private Child _child;
		
		public ParentWithSingleChild(Child child) {
			_child = child;			
		}

		public Child child() {
			return _child;
		}
		
		public void child(Child child) {
			_child = child;
		}
	}
		
	public class Child extends ActivatableBase {
		private int _value;

		public Child(int value) {
			_value = value;
		}

		public int value() {
			activateForRead();
			return _value;
		}
		
		public void value(int value) {
			activateForWrite();
			_value = value;
		}
	}
	
	public void configureServer(Configuration config) throws Exception {
		config.reflectWith(new ExcludingReflector(Child.class, ParentWithMultipleChilds.class, ParentWithSingleChild.class));
	}
	
	public void configureClient(Configuration config) throws Exception {
		config.add(new TransparentActivationSupport());
	}
	
	@Override
	protected void store() throws Exception {
		store(new ParentWithMultipleChilds(new Child(42)));
		store(new ParentWithSingleChild(new Child(43)));
	}
	
	public void testChildArray() {
		ExtObjectContainer client1 = openNewSession();
		Query query = client1.query();
		query.constrain(ParentWithMultipleChilds.class);
		
		ObjectSet result = query.execute();
		Assert.isTrue(result.hasNext());
		
		ParentWithMultipleChilds parent = (ParentWithMultipleChilds) result.next();
		Assert.isNotNull(parent.children());
		client1.close();
	}
	
	public void testSingleChild() {
		ExtObjectContainer client1 = openNewSession();
		Query query = client1.query();
		query.constrain(ParentWithSingleChild.class);
		
		ObjectSet result = query.execute();
		Assert.isTrue(result.hasNext());
		
		ParentWithSingleChild parent = (ParentWithSingleChild) result.next();
		Assert.areEqual(43, parent.child().value());
		client1.close();
	}
}
