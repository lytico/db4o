package com.db4o.db4ounit.jre5.query;

import java.util.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;


/**
 */
@decaf.Ignore(unlessCompatible=decaf.Platform.JDK15)
public class COR1212TestCase extends AbstractDb4oTestCase {
	
	public static void main(String[] args) {
		new COR1212TestCase().runSolo();
	}

	private final class TestEvaluation implements Evaluation {
		public void evaluate(Candidate candidate) {
			candidate.include(true);				
		}
	}

	public static class Item {
		String name = null;
		Date modified;
		Hashtable hashtable;
		public Item(String name_) {
			name = name_;
			modified = new Date();
		}
		public String getName() {
			return name;
		}
		public void setName(String name) {
			this.name = name;
		}
	}
	
	protected void store() throws Exception {
		for (int i = 0; i < 3; i++) {
			store(new Item("item " + Integer.valueOf(i)));
		}
		
	}
	
	protected void configure(Configuration config) throws Exception {
		config.objectClass(Item.class).cascadeOnDelete(true);
	}
	
	public void _test() throws Exception {
		Query query = newQuery();
		query.constrain(new TestEvaluation());
		query.constrain(Item.class);
		query.descend("name").orderDescending();
		ObjectSet set = query.execute();
		Assert.areEqual(3, set.size());
	}
}
