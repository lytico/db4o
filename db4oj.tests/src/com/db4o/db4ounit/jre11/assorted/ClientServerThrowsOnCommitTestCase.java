/* Copyright (C) 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.jre11.assorted;

import com.db4o.events.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;


public class ClientServerThrowsOnCommitTestCase extends AbstractDb4oTestCase implements OptOutSolo{
	
	public static void main(String[] arguments) {
		new ClientServerThrowsOnCommitTestCase().runNetworking();
	}
	
	public static class ExpectedException extends Db4oRecoverableException {
		public ExpectedException() {
			super("");
		}
	}

	public void test(){
		final EventListener4 listener = new EventListener4() {
			public void onEvent(Event4 e, EventArgs args) {
				throw new ExpectedException();
			}
		};
		serverEventRegistry().committing().addListener(listener);
		Assert.expect(ExpectedException.class, new CodeBlock() {
			public void run() throws Throwable {
				db().commit();
			}
		});
		serverEventRegistry().committing().removeListener(listener);
	}
}
