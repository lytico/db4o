/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

package com.db4o.drs.test;

import com.db4o.*;
import com.db4o.drs.*;
import com.db4o.drs.foundation.*;
import com.db4o.drs.inside.*;
import com.db4o.drs.test.data.*;

import db4ounit.*;

public class TimestampTestCase extends DrsTestCase {
	
	public void test(){
		
		final TestableReplicationProviderInside providerA = a().provider();
		final TestableReplicationProviderInside providerB = b().provider();
		
		providerA.storeNew(new Pilot());
		providerA.commit();
		
		ReplicationSession replication = Replication.begin(providerA, providerB, null, _fixtures.reflector);
		
		TimeStamps initialTimeStampsB = assertTimeStampsForFirstReplication(providerB);
		
		ObjectSet modifiedObjects = providerA.objectsChangedSinceLastReplication();
		while(modifiedObjects.hasNext()){
			replication.replicate(modifiedObjects.next());
		}
		replication.commit();
		
		Pilot replicatedPilot = (Pilot) providerB.getStoredObjects(Pilot.class).next();
		long version = providerB.objectVersion(replicatedPilot);
		Assert.areEqual(initialTimeStampsB.commit(), version);
		
		replication = Replication.begin(providerA, providerB, null, _fixtures.reflector);
		TimeStamps timestampsBAfterReplication = assertTimeStampsForSecondReplication(initialTimeStampsB, providerB);
		
		replication.commit();
		
		Pilot pilotStoredAfterReplication = new Pilot();
		providerB.storeNew(pilotStoredAfterReplication);
		providerB.commit();
		providerB.waitForPreviousCommits();
		version = providerB.objectVersion(pilotStoredAfterReplication);
		Assert.isGreater(timestampsBAfterReplication.commit(), version);
	}
	
	private TimeStamps assertTimeStampsForFirstReplication(
			TestableReplicationProviderInside provider) {
		TimeStamps timeStamps = provider.timeStamps();
		Assert.isNotNull(timeStamps);
		Assert.areEqual(0, timeStamps.from());
		Assert.isGreater(0, timeStamps.to());
		Assert.areEqual(timeStamps.to() + 1, timeStamps.commit());
		return timeStamps;
	}

	private TimeStamps assertTimeStampsForSecondReplication( 
			TimeStamps initialTimeStamps,
			TestableReplicationProviderInside provider) {
		TimeStamps timeStamps = provider.timeStamps();
		Assert.isNotNull(timeStamps);
		Assert.areEqual(initialTimeStamps.commit(), timeStamps.from());
		Assert.isGreater(timeStamps.from(), timeStamps.to());
		Assert.areEqual(timeStamps.to() + 1, timeStamps.commit());
		return timeStamps;
	}

}
