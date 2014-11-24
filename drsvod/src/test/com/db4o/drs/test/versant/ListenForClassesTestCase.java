package com.db4o.drs.test.versant;


import com.db4o.drs.test.data.*;
import com.db4o.drs.test.versant.data.Item;
import com.db4o.drs.versant.VodReplicationProvider;
import com.db4o.foundation.Closure4;
import db4ounit.ConsoleTestRunner;

public class ListenForClassesTestCase extends VodProviderTestCaseBase{

    public static void main(String[] args) {
        new ConsoleTestRunner(ListenForClassesTestCase.class).run();
    }

    public void testListenToFullyQualifiedClassName() throws Exception {
        withEventProcessor(new Closure4<Void>() {
            public Void run() {
                VodReplicationProvider replicationProvider = new VodReplicationProvider(_vod);
                replicationProvider.listenForReplicationEvents(Item.class);

                return null;
            }
        });
    }



    public void testListenToShortClassName() throws Exception {

        withEventProcessor(new Closure4<Void>() {
            public Void run() {
                VodReplicationProvider replicationProvider = new VodReplicationProvider(_vod);
                replicationProvider.listenForReplicationEvents(UnqualifiedNamed.class);

                return null;
            }
        });
    }

    /**
     * By by checked exceptions
     */
    @Override
    protected void withEventProcessor(Closure4<Void> closure) {
        try {
            super.withEventProcessor(closure);
        } catch (Exception e) {
            JDOUtilities.reThrow(e);
        }
    }
}
