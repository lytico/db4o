/* Copyright (C) 2007  Versant Inc.   http://www.db4o.com */

package com.db4o.db4ounit.common.staging;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.ext.*;
import com.db4o.foundation.*;
import com.db4o.messaging.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class PingTestCase extends Db4oClientServerTestCase implements OptOutAllButNetworkingCS {

	public static void main(String[] args) {
		new PingTestCase().runAll();
	}

	protected void configure(Configuration config) {
		config.clientServer().timeoutClientSocket(1000);
	}

	TestMessageRecipient recipient = new TestMessageRecipient();

	public void test() {
		clientServerFixture().server().ext().configure().clientServer()
				.setMessageRecipient(recipient);

		final ExtObjectContainer client = clientServerFixture().db();
		final MessageSender sender = client.configure().clientServer()
				.getMessageSender();
		
		if(isEmbedded()){
		    Assert.expect(NotSupportedException.class, new CodeBlock(){
                public void run() throws Throwable {
                    sender.send(new Data());
                }
		    });
		    return;
		}
		
	    sender.send(new Data());
		

		// The following query will be block by the sender
		ObjectSet os = client.queryByExample(null);
		while (os.hasNext()) {
			os.next();
		}
		Assert.isFalse(client.isClosed());
	}

	public static class TestMessageRecipient implements MessageRecipient {
		public void processMessage(MessageContext con, Object message) {
			Runtime4.sleep(3000);
		}
	}

	public static class Data {
	}
}