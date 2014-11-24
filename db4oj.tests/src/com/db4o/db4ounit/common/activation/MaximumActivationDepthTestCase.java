/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.activation;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.query.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class MaximumActivationDepthTestCase
	extends AbstractDb4oTestCase
	implements OptOutTA {

	public static class Data {
		public int _id;
		public Data _prev;

		public Data(int id,Data prev) {
			_id=id;
			_prev = prev;
		}
	}

	protected void configure(Configuration config) {
		config.activationDepth(Integer.MAX_VALUE);
		config.objectClass(Data.class).maximumActivationDepth(1);
	}
	
	protected void store() throws Exception {
		Data data=new Data(2,null);
		data=new Data(1,data);
		data=new Data(0,data);
		store(data);
	}

	public void testActivationRestricted() {
		Query query=newQuery(Data.class);
		query.descend("_id").constrain(new Integer(0));
		ObjectSet result=query.execute();
		Assert.areEqual(1,result.size());
		Data data=(Data)result.next();
		Assert.isNotNull(data._prev);
		Assert.isNull(data._prev._prev);
	}
}
