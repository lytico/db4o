/* Copyright (C) 2009  Versant Corp.  http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import com.db4o.query.*;

import db4ounit.*;

public class ReferenceSystemIsolationTestCase extends EmbeddedAndNetworkingClientTestCaseBase {
	
	public static final class IncludeAllEvaluation implements Evaluation {
		public void evaluate(Candidate candidate) {
			candidate.include(true);
		}
	}

	public static class Item{
		
	}
	
	public void test(){
		Item item = new Item();
		networkingClient().store(item);
		int id = (int) networkingClient().getID(item);
		
		Query query = networkingClient().query();
		query.constrain(Item.class);
		query.constrain(new IncludeAllEvaluation());
		query.execute();
		
		Assert.isNull(embeddedClient().transaction().referenceForId(id));
	}

}
