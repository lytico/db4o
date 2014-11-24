/* Copyright (C) 2004 - 2007 Versant Inc. http://www.db4o.com */

package com.db4o.db4ounit.common.exceptions;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.internal.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;


public class ActivationExceptionBubblesUpTestCase extends AbstractDb4oTestCase {
	
	public static void main(String[] args) {
		new ActivationExceptionBubblesUpTestCase().runAll();
	}
	
	public static class AcceptAllEvaluation implements Evaluation {
		public void evaluate(Candidate candidate) {
			candidate.include(true);
		}
	}

	public static final class ItemTranslator implements ObjectTranslator {

		public void onActivate(ObjectContainer container,
				Object applicationObject, Object storedObject) {
			
			throw new ItemException();
		}

		public Object onStore(ObjectContainer container, Object applicationObject) {
			return applicationObject;
		}

		public Class storedClass() {
			return Item.class;
		}
		
	}
	
	protected void configure(Configuration config) {
		config.objectClass(Item.class).translate(new ItemTranslator());
	}
	
	protected void store() throws Exception {
		store(new Item());
	}
	
	public void test() {
		Assert.expect(ReflectException.class, ItemException.class,
				new CodeBlock() {
					public void run() throws Throwable {
						final Query q = db().query();
						q.constrain(Item.class);
						q.constrain(new AcceptAllEvaluation());
						q.execute().next();
					}
				});
	}

}
