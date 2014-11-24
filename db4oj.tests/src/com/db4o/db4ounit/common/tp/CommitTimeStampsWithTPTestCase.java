package com.db4o.db4ounit.common.tp;

import com.db4o.*;
import com.db4o.activation.*;
import com.db4o.config.*;
import com.db4o.io.*;
import com.db4o.ta.*;

import db4ounit.*;
import db4ounit.extensions.*;

public class CommitTimeStampsWithTPTestCase extends AbstractDb4oTestCase{

    public void testWorksWithoutTP() {
        assertUpdate(false);
    }

	private void assertUpdate(boolean isTP) {
		final NamedItem item = new NamedItem();
        store(item);
        commit();
        final long firstTS = commitTimestampFor(item);
        item.setName("New Name");
        if(! isTP){
        	store(item);
        }
        commit();
        final long secondTS = commitTimestampFor(item);
        assertChangesHaveBeenStored(db());
        Assert.isTrue(secondTS>firstTS);
	}

	private long commitTimestampFor(final NamedItem item) {
		return db().ext().getObjectInfo(item).getCommitTimestamp();
	}

    public void testWorksWithTP() throws Exception {
		fixture().configureAtRuntime(new RuntimeConfigureAction() {
			public void apply(Configuration config) {
				config.add(new TransparentPersistenceSupport());	
			}
		});
		reopen();
		assertUpdate(true);
    }

    private void assertChangesHaveBeenStored(ObjectContainer container) {
        ObjectContainer session = container.ext().openSession();
        try{
            final NamedItem item = session.query(NamedItem.class).get(0);
            Assert.areEqual("New Name", item.getName());
        } finally {
            session.close();
        }
    }
    
    @Override
    protected void configure(Configuration config) throws Exception {
    	config.generateCommitTimestamps(true);
    	config.storage(new MemoryStorage());
    }

    public static class NamedItem implements Activatable {

        private transient Activator _activator;

        public String name = "default";

        public void setName(String name) {
            activate(ActivationPurpose.WRITE);
            this.name = name;
        }

        public String getName() {
            activate(ActivationPurpose.READ);
            return this.name;
        }

        @Override
        public void activate(ActivationPurpose purpose) {
            if (_activator != null) {
                _activator.activate(purpose);
            }
        }

        @Override
        public void bind(Activator activator) {
            if (_activator == activator) {
                return;
            }
            if (activator != null && _activator != null) {
                throw new IllegalStateException();
            }
            _activator = activator;

        }

    }

}
