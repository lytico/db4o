/* Copyright (C) 2004 - 2007  Versant Inc.  http://www.db4o.com */

package com.db4o.db4ounit.common.cs;

import com.db4o.cs.internal.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;
import db4ounit.extensions.fixtures.*;

public class ClientDisconnectTestCase extends Db4oClientServerTestCase implements
    OptOutAllButNetworkingCS {

    public static void main(String[] arguments) {
        new ClientDisconnectTestCase().runNetworking();
    }

    public void testDisconnect() {
        ExtObjectContainer oc1 = openNewSession();
        ExtObjectContainer oc2 = openNewSession();
        try {
            final ClientObjectContainer client1 = (ClientObjectContainer) oc1;
            final ClientObjectContainer client2 = (ClientObjectContainer) oc2;
            client1.socket().close();
            Assert.isFalse(oc1.isClosed());
            Assert.expect(Db4oException.class, new CodeBlock() {
                public void run() throws Throwable {
                    client1.queryByExample(null);
                }
            });
            // It's ok for client2 to get something.
            client2.queryByExample(null);
        } finally {
            oc1.close();
            oc2.close();
            Assert.isTrue(oc1.isClosed());
            Assert.isTrue(oc2.isClosed());
        }
    }


}
