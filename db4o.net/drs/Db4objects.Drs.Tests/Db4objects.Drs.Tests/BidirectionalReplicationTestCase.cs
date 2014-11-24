/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Drs;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class BidirectionalReplicationTestCase : DrsTestCase
	{
		public virtual void TestObjectsAreOnlyReplicatedOnce()
		{
			ITestableReplicationProviderInside providerA = A().Provider();
			ITestableReplicationProviderInside providerB = B().Provider();
			StoreNewPilotIn(providerA);
			int replicatedObjects = ReplicateBidirectional(providerA, providerB, null);
			Assert.AreEqual(1, replicatedObjects);
			ModifyPilotIn(providerA, "modifiedInA");
			replicatedObjects = ReplicateBidirectional(providerA, providerB, typeof(Pilot));
			Assert.AreEqual(1, replicatedObjects);
			ModifyPilotIn(providerB, "modifiedInB");
			replicatedObjects = ReplicateBidirectional(providerA, providerB, null);
			Assert.AreEqual(1, replicatedObjects);
			StoreNewPilotIn(providerA);
			StoreNewPilotIn(providerB);
			replicatedObjects = ReplicateBidirectional(providerA, providerB, typeof(Pilot));
			Assert.AreEqual(2, replicatedObjects);
		}

		private void ModifyPilotIn(ITestableReplicationProviderInside provider, string newName
			)
		{
			Pilot pilot;
			pilot = (Pilot)provider.GetStoredObjects(typeof(Pilot)).Next();
			pilot.SetName(newName);
			provider.Update(pilot);
			provider.Commit();
			provider.WaitForPreviousCommits();
		}

		private int ReplicateBidirectional(ITestableReplicationProviderInside providerA, 
			ITestableReplicationProviderInside providerB, Type clazz)
		{
			int replicatedObjects = 0;
			IReplicationSession replicationSession = Replication.Begin(providerA, providerB, 
				null, _fixtures.reflector);
			IObjectSet changedInA = clazz == null ? providerA.ObjectsChangedSinceLastReplication
				() : providerA.ObjectsChangedSinceLastReplication(clazz);
			foreach (object obj in changedInA)
			{
				replicatedObjects++;
				replicationSession.Replicate(obj);
			}
			IObjectSet changedInB = clazz == null ? providerB.ObjectsChangedSinceLastReplication
				() : providerB.ObjectsChangedSinceLastReplication(clazz);
			foreach (object obj in changedInB)
			{
				replicatedObjects++;
				replicationSession.Replicate(obj);
			}
			replicationSession.Commit();
			return replicatedObjects;
		}

		private void StoreNewPilotIn(ITestableReplicationProviderInside provider)
		{
			Pilot pilot = new Pilot();
			provider.StoreNew(pilot);
			provider.Commit();
			provider.WaitForPreviousCommits();
		}
	}
}
