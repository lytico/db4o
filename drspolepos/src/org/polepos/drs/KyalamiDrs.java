/* Copyright (C) 2004 - 2006 db4objects Inc. http://www.db4o.com */

package org.polepos.drs;

import org.polepos.circuits.kyalami.*;

import com.db4o.*;
import com.db4o.drs.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.test.*;
import com.db4o.test.replication.*;

public class KyalamiDrs extends DrsDriver implements KyalamiDriver {

    public  void storeInA() {
        int count = setup().getObjectCount();
        for (int i = 0; i < count; i++) {
            KyalamiObject ko = new KyalamiObject(i);
            addToCheckSum(ko.checkSum());
            providerA().storeNew(ko);
        }
        providerA().commit();
    }

    public void replicate() {
        replicateAll();
    }

	public void modifyInB() {
		int count = setup().getObjectCount();
		
		for (int i = 0; i < count; i++) {
			ObjectSet objectSet = providerB().getStoredObjects(new KyalamiObject(i).getClass());
			
			KyalamiObject ko = (KyalamiObject)objectSet.next();
			
			ko.setVal(1000+i);
			addToCheckSum(ko.checkSum());
			providerB().update(ko);
		}
		providerB().commit();
	}

	public void replicate2() {
		replicateAll(providerB(), providerA());
	}

	public void modifyInA() {
		int count = setup().getObjectCount();
		
		for (int i = 0; i < count; i++) {
			ObjectSet objectSet = providerA().getStoredObjects(new KyalamiObject(i).getClass());
			
			KyalamiObject ko = (KyalamiObject)objectSet.next();
			
			ko.setVal(500+i);
			addToCheckSum(ko.checkSum());
			providerA().update(ko);
		}
		providerB().commit();
	}

	public void replicate3() {
		replicateClass(providerA(), providerB(), SPCChild.class);

	}

	protected void replicateClass(TestableReplicationProviderInside providerA, TestableReplicationProviderInside providerB, Class clazz) {
		//System.out.println("ReplicationTestcase.replicateClass");
		ReplicationSession replication = Replication.begin(providerA, providerB);
		ObjectSet allObjects = providerA.objectsChangedSinceLastReplication(clazz);
		while (allObjects.hasNext()) {
			final Object obj = allObjects.next();
			//System.out.println("obj = " + obj);
			replication.replicate(obj);
		}
		replication.commit();
	}

}