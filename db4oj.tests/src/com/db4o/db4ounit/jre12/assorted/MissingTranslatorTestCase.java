/* Copyright (C) 2012 Versant Inc. http://www.db4o.com */
package com.db4o.db4ounit.jre12.assorted;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.internal.*;
import com.db4o.internal.delete.*;
import com.db4o.marshall.*;
import com.db4o.query.*;
import com.db4o.reflect.*;
import com.db4o.typehandlers.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class MissingTranslatorTestCase extends AbstractDb4oTestCase implements OptOutDefragSolo, OptOutNetworkingCS {
	
	public static void main(String[] args) {
		new MissingTranslatorTestCase().runAll();
	}
	
	public static class ItemTypeHandler implements ReferenceTypeHandler {

		@Override
		public void delete(DeleteContext context) throws Db4oIOException {
		}

		@Override
		public void defragment(DefragmentContext context) {
		}

		@Override
		public void write(WriteContext context, Object obj) {
			Item item = (Item) obj;
			context.writeInt(item.value);
		}

		@Override
		public void activate(ReferenceActivationContext context) {
			Item item = (Item) context.persistentObject();
			item.value = context.readInt();
		}		
	}
	
    public static class Item {
        
        int value;
        
        public Item(int name){
            this.value = name;
        }

		@Override
		public boolean equals(Object obj) {
			if (this == obj) return true;
			if (obj == null) 
				return false;
			if (getClass() != obj.getClass()) return false;
			
			Item other = (Item) obj;
			return value == other.value;
		}
        
    }
    
	private ItemConstructor translator =  new ItemConstructor();

	
	private void configureTranslator(Configuration config) {
		translator =  new ItemConstructor();
		config.objectClass(Item.class).translate(translator);
	}
	
	@Override
	protected void store() throws Exception {
		configureTranslator(fixture().config());
		fixture().config().generateUUIDs(ConfigScope.GLOBALLY);
		reopen();		
		store(new Item(42));
		
		fixture().resetConfig();
		reopen();
	}

    public static class ItemConstructor implements ObjectConstructor{

        public Object onInstantiate(ObjectContainer container, Object storedObject) {
            callCount++;
        	return new Item((Integer) storedObject);
        }

        public void onActivate(ObjectContainer container, Object applicationObject, Object storedObject) {
            callCount++;
        }

        public Object onStore(ObjectContainer container, Object applicationObject) {
            callCount++;
            return ((Item)applicationObject).value;
        }

        public Class storedClass() {
            return Integer.class;
        }
        
        public void resetCounter() {
        	callCount = 0;
        }
        
        public int callCount;
    }

	public void testMissingTranslatorThrows() {
		
		Assert.expect(Db4oFatalException.class, new CodeBlock() {		
			@Override
			public void run() throws Throwable {
				triggerTranslatorValidation();
			}		
		});
		
	}
	
	public void testTypeHandlerAfterTranslator() throws Exception {

		fixture().clean();
		
		fixture().config().registerTypeHandler(new TypeHandlerPredicate() {			
			public boolean match(ReflectClass classReflector) {
				return classReflector.getName().equals(Item.class.getName());
			}
		}, new ItemTypeHandler());
		
		reopen();

		triggerTranslatorValidation();
	}

	private void triggerTranslatorValidation() {
		InternalObjectContainer ocs = (InternalObjectContainer) db();
		ocs.classMetadataForName(Item.class.getName());
	}
	
	public void testMissingTranslatorDoesNotThrowsInRecoveryMode() throws Exception {
		
		fixture().config().recoveryMode(true);
		reopen();
		
		triggerTranslatorValidation();		
		Assert.isGreater(0,  translator.callCount);
		
	}
	
	public void testDbIsUsableAfterException() throws Exception {
		boolean exceptionThrown = false;
		try {
			triggerTranslatorValidation();			
		}
		catch(Db4oFatalException ex) {
			exceptionThrown = true;
		}
		
		Assert.isTrue(exceptionThrown);
		fixture().resetConfig();
		configureTranslator(fixture().config());
		reopen();
		
		Query query = db().query();
		query.constrain(Item.class);
		
		ObjectSet<Object> result = query.execute();
		Assert.areEqual(1, result.size());
		Assert.areEqual(new Item(42), result.get(0));
	}
	
	public void testTranslatorInstalled() {
		Assert.isGreater(0, translator.callCount);
	}
	
	public void testConfiguringTranslatorForExistingClass() throws Exception {
			
		// get rid of translator config by deleting the database and starting fresh
		fixture().close();
		fixture().clean();
		reopen();
		
		store(new Item(1));
		fixture().resetConfig();
		configureTranslator(fixture().config());
		reopen();
		
		Assert.expect(Db4oFatalException.class, new CodeBlock() {
			@Override
			public void run() throws Throwable {
				triggerTranslatorValidation();
			}			
		});
	}
	
}
