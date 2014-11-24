package com.db4o.taj.tests.program;

import java.io.*;
import java.util.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.events.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;
import com.db4o.ta.*;
import com.db4o.taj.tests.model.*;

import db4ounit.*;

public class Main {
	
	private static final class TAJActivationListener implements EventListener4 {
		
		private Map _activations = new HashMap();
		
		public int activationCount(Class clazz) {
			Integer activationCount = (Integer) _activations.get(clazz);
			if(activationCount == null) {
				return 0;
			}
			return activationCount.intValue();
		}
		
		public void onEvent(Event4 event, EventArgs args) {
			Object obj = ((ObjectEventArgs) args).object();
			Class curClazz = obj.getClass();
			while(curClazz != Object.class) {
				
				_activations.put(curClazz, new Integer(activationCount(curClazz) + 1));
				curClazz = curClazz.getSuperclass();
			}
		}
		
		public void reset() {
			_activations.clear();
		}
	}

	private static final String DB_PATH = "pilots.db4o";

	public static void main(String[] args) throws Throwable {
		final CollectionHolder holder = new CollectionHolder("Holder");
		Item item = new Item("Item");
		holder.arrayList().add(item);
		holder.linkedList().add(new Item("Item"));
		holder.hashMap().put("Key", new Item("Item"));
		holder.hashtable().put("Key", new Item("Item"));
		holder.stack().push(new Item("Item"));
		holder.hashSet().add(new Item("Item"));
		holder.treeSet().add(new Item("Item"));
		
		deleteDatabase();

		try {
			withDatabase(new Procedure4() {
				public void apply(Object obj) {
					((ObjectContainer)obj).store(holder);
				}
			});
	
			withDatabase(new Procedure4() {
				public void apply(Object obj) {
					try {
						ObjectContainer db = (ObjectContainer) obj;
						TAJActivationListener listener = new TAJActivationListener();
						EventRegistryFactory.forObjectContainer(db).activated().addListener(listener);
						
						CollectionHolder holder = retrieveHolder(db);
						
						assertCollectionsAreNull(holder);
						
						assertItemActivation(listener, holder, hashMapExtractor(), mapItemExtractor(), HashMap.class);
						
						assertItemActivation(listener, holder, hashtableExtractor(), mapItemExtractor(), Hashtable.class);
						
						assertItemActivation(listener, holder, arrayListExtractor(), collectionItemExtractor(), ArrayList.class);
						
						assertItemActivation(listener, holder, linkedListExtractor(), collectionItemExtractor(), LinkedList.class);
						
						assertItemActivation(listener, holder, stackExtractor(), collectionItemExtractor(), Stack.class);						
						
						assertItemActivation(listener, holder, hashSetExtractor(), collectionItemExtractor(), HashSet.class);
						
						assertItemActivation(listener, holder, treeSetExtractor(), collectionItemExtractor(), TreeSet.class);						
					}
					catch(Exception exc) {
						Assert.fail("", exc);
					}
				}
			});
		}
		finally {
			deleteDatabase();
		}
	}
	
	private static CollectionHolder retrieveHolder(ObjectContainer db) {
		ObjectSet result = db.query(CollectionHolder.class);
		CollectionHolder holder = (CollectionHolder) result.next();
		return holder;
	}
	
	private static Function4 collectionItemExtractor() {
		return new Function4() {
			public Object apply(Object arg) {
				Collection collection = (Collection) arg;
				return collection.iterator().next();
			}							
		};
	}

	private static Function4 mapItemExtractor() {
		return new Function4() {
			public Object apply(Object arg) {
				Map map = (Map) arg;
				return map.get("Key");
			}							
		};
	}
	
	private static Function4 stackExtractor() {
		return new Function4() {
			public Object apply(Object arg) {
				CollectionHolder holder = (CollectionHolder) arg;
				return holder.stack();
			}							
		};
	}

	private static Function4 hashSetExtractor() {
		return new Function4() {
			public Object apply(Object arg) {
				CollectionHolder holder = (CollectionHolder) arg;
				return holder.hashSet();
			}							
		};
	}

	private static Function4 hashMapExtractor() {
		return new Function4() {
			public Object apply(Object arg) {
				CollectionHolder holder = (CollectionHolder) arg;
				return holder.hashMap();
			}							
		};
	}
	
	private static Function4 hashtableExtractor() {
		return new Function4() {
			public Object apply(Object arg) {
				CollectionHolder holder = (CollectionHolder) arg;
				return holder.hashtable();
			}							
		};
	}
	
	private static Function4 linkedListExtractor() {
		return new Function4() {

			public Object apply(Object arg) {
				CollectionHolder holder = (CollectionHolder) arg;
				return holder.linkedList();
			}							
		};
	}
	
	private static Function4 treeSetExtractor() {
		return new Function4() {
			public Object apply(Object arg) {
				CollectionHolder holder = (CollectionHolder) arg;
				return holder.treeSet();
			}							
		};
	}

	private static Function4 arrayListExtractor() {
		return new Function4() {

			public Object apply(Object arg) {
				CollectionHolder holder = (CollectionHolder) arg;
				return holder.arrayList();
			}							
		};
	}

	private static void assertCollectionsAreNull(CollectionHolder holder) throws IllegalAccessException {
		assertCollectionIsNull(holder, "_arrayList");
		assertCollectionIsNull(holder, "_linkedList");
		assertCollectionIsNull(holder, "_hashMap");
		assertCollectionIsNull(holder, "_hashtable");
		assertCollectionIsNull(holder, "_stack");
		assertCollectionIsNull(holder, "_hashSet");
		assertCollectionIsNull(holder, "_treeSet");
	}

	private static void assertItemActivation(
						TAJActivationListener listener,
						final CollectionHolder holder,
						Function4 collectionExtractor,
						Function4 itemExtractor,
						final Class clazz) {
		
		listener.reset();
		
		assertNoActivation(listener, new Class[] { ArrayList.class, HashMap.class, Hashtable.class, LinkedList.class, Stack.class, HashSet.class });
		
		assertActivationCount(listener, Item.class, 0);
		assertActivationCount(listener, clazz, 0);
		Item item = (Item) itemExtractor.apply(collectionExtractor.apply(holder));
		
		assertActivationCount(listener, clazz, 1);
		assertActivationCount(listener, Item.class, 0);
		Assert.areEqual("Item", item.name());					
		assertActivationCount(listener, Item.class, 1);
	}

	private static void assertCollectionIsNull(CollectionHolder holder,
			final String collectionFieldName)
			throws IllegalAccessException {
		
		Object fieldValue = Reflection4.getFieldValue(holder, collectionFieldName);
		Assert.isNull(fieldValue);
	}

	private static void deleteDatabase() {
		new File(DB_PATH).delete();
	}

	private static void assertActivationCount(TAJActivationListener listener, Class clazz, int expectedCount) {
		Assert.areEqual(expectedCount, listener.activationCount(clazz), clazz.getName());
	}

	private static void assertNoActivation(TAJActivationListener listener, Class[] clazzes) {
		for (int clazzIdx = 0; clazzIdx < clazzes.length; clazzIdx++) {
			Assert.areEqual(0, listener.activationCount(clazzes[clazzIdx]), clazzes[clazzIdx].getName());
		}
	}

	private static EmbeddedConfiguration config() {
		EmbeddedConfiguration config = Db4oEmbedded.newConfiguration();
		config.common().activationDepth(0);
		config.common().add(new TransparentPersistenceSupport());
		return config;
	}

	private static void withDatabase(Procedure4 block) {
		ObjectContainer db = Db4oEmbedded.openFile(config(), DB_PATH);
		try {
			block.apply(db);
		}
		finally {
			db.close();
		}
	}
}
