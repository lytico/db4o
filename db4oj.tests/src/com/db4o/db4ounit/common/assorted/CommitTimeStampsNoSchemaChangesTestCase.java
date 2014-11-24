package com.db4o.db4ounit.common.assorted;

import com.db4o.config.*;
import com.db4o.ext.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CommitTimeStampsNoSchemaChangesTestCase extends AbstractDb4oTestCase{
    
    @Override
    protected void configure(Configuration config) throws Exception {
    	config.generateCommitTimestamps(true);
    }

    @Override
    protected void store() throws Exception {
    	store(new Holder());
    	intializeCSClasses();
    }

	private void intializeCSClasses() {
		for (Holder holder : db().query(Holder.class)) {
			db().getID(holder);
    	}
	}

    public void testCommitTimestampsNoSchemaDetection() throws Exception{
    	fixture().configureAtRuntime(new RuntimeConfigureAction() {
			@Override
			public void apply(Configuration config) {
				config.detectSchemaChanges(false);
				config.generateCommitTimestamps(true);
			}
		});
    	reopen();
        store(new Holder());
        commit();
        
        for (Holder holder : db().query(Holder.class)) {
            final ObjectInfo objectInfo = db().ext().getObjectInfo(holder);
            final long ts = objectInfo.getCommitTimestamp();
            Assert.isGreater(0, ts);
        }
    }

    public static class Holder{
        public String data = "data";
    }
}
