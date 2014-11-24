/* Copyright (C) 2006  db4objects Inc.  http://www.db4o.com */

package org.polepos.drs;


import org.polepos.framework.*;
import org.polepos.framework.Car;

import com.db4o.*;
import com.db4o.drs.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.test.*;

/**
 * @exclude
 */
public class DrsDriver extends Driver {
    
    private DrsCar _car;
    
    @Override
    public void takeSeatIn(Car car, TurnSetup setup) throws CarMotorFailureException {
        super.takeSeatIn(car, setup);
        _car = (DrsCar)car;
        _car.clean();
    }
    
    public TestableReplicationProviderInside providerA(){
        return fixtureA().provider();
    }

    private DrsFixture fixtureA() {
        return _car.fixtureA();
    }
    
    public TestableReplicationProviderInside providerB(){
        return fixtureB().provider();
    }

    private DrsFixture fixtureB() {
        return _car.fixtureB();
    }


    @Override
    public void prepare() throws CarMotorFailureException {
        try {
            fixtureA().open();
            fixtureB().open();
            
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    @Override
    public void backToPit() {
        try {
            fixtureA().close();
            fixtureB().close();
        } catch (Exception e) {
            e.printStackTrace();
        }
    }

    protected void replicateAll() {
        ReplicationSession replication = Replication.begin(providerA(), providerB());
        ObjectSet allObjects = providerA().objectsChangedSinceLastReplication();
        while (allObjects.hasNext()) {
            Object changed = allObjects.next();
            if(changed instanceof CheckSummable){
                addToCheckSum(((CheckSummable)changed).checkSum());
            }
            replication.replicate(changed);
        }
        replication.commit();
    }
    
	protected void replicateAll(TestableReplicationProviderInside providerFrom, TestableReplicationProviderInside providerTo) {
		//System.out.println("from = " + providerFrom + ", to = " + providerTo);
		ReplicationSession replication = Replication.begin(providerFrom, providerTo);
		ObjectSet allObjects = providerFrom.objectsChangedSinceLastReplication();

		if (!allObjects.hasNext())
			throw new RuntimeException("Can't find any objects to replicate");

		while (allObjects.hasNext()) {
			Object changed = allObjects.next();
			//System.out.println("changed = " + changed);
			replication.replicate(changed);
		}
		replication.commit();
	}

	protected Object getOneInstance(TestableReplicationProviderInside provider, Class clazz) {
		ObjectSet objectSet = provider.getStoredObjects(clazz);

		if (1 != objectSet.size())
			throw new RuntimeException("Found more than one instance of + " + clazz + " in provider = " + provider);

		return objectSet.next();
	}

}
