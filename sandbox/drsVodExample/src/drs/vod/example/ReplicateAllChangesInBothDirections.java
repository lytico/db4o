/* Copyright (C) 2004 - 2010  Versant Inc.  http://www.db4o.com */

package drs.vod.example;

import com.db4o.*;
import com.db4o.drs.*;
import com.db4o.drs.db4o.*;
import com.db4o.drs.versant.*;
import com.db4o.drs.versant.jdo.reflect.*;

import drs.vod.example.utils.*;

public class ReplicateAllChangesInBothDirections {
	
	public static void main(String[] args) {
		
		ObjectContainer objectContainer = Db4oHelper.openObjectContainer();
		Db4oEmbeddedReplicationProvider db4oReplicationProvider = 
			new Db4oEmbeddedReplicationProvider(objectContainer);
		
		VodDatabase vodDatabase = new VodDatabase("dRSVodExample", VodHelper.properties());
		VodReplicationProvider vodReplicationProvider = 
			new VodReplicationProvider(vodDatabase);
		
		ReplicationSession replicationSession = 
			Replication.begin(db4oReplicationProvider, vodReplicationProvider, new JdoReflector(Thread.currentThread().getContextClassLoader()));
		
		boolean changesFound = false;
		
		ObjectSet changedInDb4o = db4oReplicationProvider.objectsChangedSinceLastReplication();
		for (Object object : changedInDb4o) {
			System.out.println("Replicating from db4o to Vod: " + object);
			replicationSession.replicate(object);
			changesFound = true;
		}
		
		ObjectSet changedInVod = vodReplicationProvider.objectsChangedSinceLastReplication();
		for (Object object : changedInVod) {
			System.out.println("Replicating from Vod to db4o: " + object);
			replicationSession.replicate(object);
			changesFound = true;
		}
		
		if(! changesFound){
			System.out.println("No changes found.");
		}
		
		replicationSession.commit();
		
		replicationSession.close();
		objectContainer.close();

	}

}
