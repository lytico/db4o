/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.fatalerror;

import db4ounit.extensions.*;


public class FatalExceptionInNestedCallTestCase extends AbstractDb4oTestCase{
	
	public static void main(String[] arguments) {
		new FatalExceptionInNestedCallTestCase().runSolo();
	}
	
	public static class Item {
		
		public Item _child;
		
		public int _depth;
		
		public Item(){
			
		}
		
		public Item(Item child, int depth){
			_child = child;
			_depth = depth;
		}

		
	}
	
	public static class FatalError extends Error{
		
	}
	
	protected void store() throws Exception {
		Item childItem = new Item(null, 1);
		Item parentItem = new Item(childItem, 0);
		store(parentItem);
	}
	
	public void test(){
		/* TODO FIXME
		eventRegistry().updated().addListener(new EventListener4() {
			public void onEvent(Event4 e, EventArgs args) {
				ObjectEventArgs objectArgs = (ObjectEventArgs) args;
				Item item = (Item) objectArgs.object();
				if(item._depth == 0){
					throw new FatalError();
				}
		
			}
		});
		Query q = this.newQuery(Item.class);
		q.descend("_depth").constrain(new Integer(0));
		ObjectSet objectSet = q.execute();
		final Item parentItem = (Item) objectSet.next();
		Assert.expect(FatalError.class, new CodeBlock() {
			public void run() throws Throwable {
				db().set(parentItem, 3);
			}
		});
		*/
	}
	
//	private EventRegistry eventRegistry() {
//		return EventRegistryFactory.forObjectContainer(db());
//	}


}
