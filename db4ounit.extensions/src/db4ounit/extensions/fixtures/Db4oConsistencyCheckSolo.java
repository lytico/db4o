/* Copyright (C) 2010  Versant Inc.   http://www.db4o.com */
package db4ounit.extensions.fixtures;

import java.io.*;

import com.db4o.*;
import com.db4o.config.*;
import com.db4o.consistency.*;
import com.db4o.defragment.*;
import com.db4o.foundation.*;
import com.db4o.internal.*;

import db4ounit.*;

public class Db4oConsistencyCheckSolo extends Db4oSolo {

	@Override
	protected ObjectContainer createDatabase(Configuration config) {
		check(cloneConfig(config));
        defrag(cloneConfig(config));
		check(cloneConfig(config));
		return super.createDatabase(config);
	}

	@Override
	protected void preClose() {
		super.preClose();
		if(db() != null && !db().isClosed()) {
			db().close();
		}
		check(cloneConfiguration());
        defrag(cloneConfiguration());
		check(cloneConfiguration());
		
	}

	@Override
	public boolean accept(Class clazz) {
        return super.accept(clazz) && !OptOutDefragSolo.class.isAssignableFrom(clazz);
	}
	
	private void check(Configuration config) {
		ObjectContainer db = super.createDatabase(config);
		ConsistencyReport report = new ConsistencyChecker(db).checkSlotConsistency();
		closeAndWait(db);
		if(!report.consistent()) {
			throw new TestException(report.toString(), null);
		}
	}

	private void defrag(Configuration config) {
		File origFile = new File(getAbsolutePath());
        if (origFile.exists()) {
            try {
                String backupFile = getAbsolutePath() + ".defrag.backup";
                IdMapping mapping = new InMemoryIdMapping();
                // new
                // BTreeIDMapping(getAbsolutePath()+".defrag.mapping",4096,1,1000);
                DefragmentConfig defragConfig = new DefragmentConfig(
                        getAbsolutePath(), backupFile, mapping);
                defragConfig.forceBackupDelete(true);
                // FIXME Cloning is ugly - wrap original in Decorator within
                // DefragContext instead?
                Configuration clonedConfig = (Configuration) ((DeepClone) config)
                        .deepClone(null);
                defragConfig.db4oConfig(clonedConfig);
                Defragment.defrag(defragConfig, new DefragmentListener() {
                    public void notifyDefragmentInfo(DefragmentInfo info) {
                        System.err.println(info);
                    }
                });
            } catch (IOException e) {
                e.printStackTrace();
            }
        }
	}
	
	private Configuration cloneConfig(Configuration config) {
		return (Configuration)((DeepClone)config).deepClone(null);
	}

	private void closeAndWait(ObjectContainer db) {
		db.close();
		try {
			((ObjectContainerBase)db).threadPool().join(3000);
		} 
		catch (InterruptedException exc) {
		}
	}
}
