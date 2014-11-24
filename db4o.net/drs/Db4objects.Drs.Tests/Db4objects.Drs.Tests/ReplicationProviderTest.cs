/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Drs.Foundation;
using Db4objects.Drs.Inside;
using Db4objects.Drs.Tests;
using Db4objects.Drs.Tests.Data;

namespace Db4objects.Drs.Tests
{
	public class ReplicationProviderTest : DrsTestCase
	{
		protected byte[] BSignatureBytes;

		protected IReadonlyReplicationProviderSignature BSignature;

		private IReadonlyReplicationProviderSignature ASignature;

		public virtual void TestReplicationLifeCycle()
		{
			BSignatureBytes = B().Provider().GetSignature().GetSignature();
			ASignature = A().Provider().GetSignature();
			BSignature = B().Provider().GetSignature();
			TstObjectUpdate();
			TstSignature();
			TstObjectsChangedSinceLastReplication();
			TstReferences();
			TstStore();
			TstRollback();
			TstDeletion();
		}

		protected virtual void TstDeletion()
		{
			A().Provider().StoreNew(new Pilot("Pilot1", 42));
			Pilot o = new Pilot("Pilot2", 43);
			A().Provider().StoreNew(o);
			A().Provider().Commit();
			A().Provider().StoreNew(new Pilot("Pilot3", 44));
			A().Provider().Delete(FindPilot("Pilot1"));
			Car car = new Car("Car1");
			car.SetPilot(FindPilot("Pilot2"));
			A().Provider().StoreNew(car);
			A().Provider().Commit();
			StartReplication();
			IDrsUUID uuidCar1 = Uuid(FindCar("Car1"));
			Assert.IsNotNull(uuidCar1);
			A().Provider().ReplicateDeletion(uuidCar1);
			CommitReplication();
			Assert.IsNull(FindCar("Car1"));
			StartReplication();
			IDrsUUID uuidPilot2 = Uuid(FindPilot("Pilot2"));
			Assert.IsNotNull(uuidPilot2);
			A().Provider().ReplicateDeletion(uuidPilot2);
			CommitReplication();
			Assert.IsNull(FindPilot("Pilot2"));
		}

		private void CommitReplication()
		{
			A().Provider().CommitReplicationTransaction();
			B().Provider().CommitReplicationTransaction();
		}

		private object FindCar(string model)
		{
			IEnumerator cars = A().Provider().GetStoredObjects(typeof(Car)).GetEnumerator();
			while (cars.MoveNext())
			{
				Car candidate = (Car)cars.Current;
				if (candidate.GetModel().Equals(model))
				{
					return candidate;
				}
			}
			return null;
		}

		private Pilot FindPilot(string name)
		{
			IEnumerator pilots = A().Provider().GetStoredObjects(typeof(Pilot)).GetEnumerator
				();
			while (pilots.MoveNext())
			{
				Pilot candidate = (Pilot)pilots.Current;
				if (candidate.Name().Equals(name))
				{
					return candidate;
				}
			}
			return null;
		}

		private SPCChild GetOneChildFromA()
		{
			return GetOneChild(A());
		}

		private SPCChild GetOneChild(IDrsProviderFixture fixture)
		{
			IObjectSet storedObjects = fixture.Provider().GetStoredObjects(typeof(SPCChild));
			Assert.AreEqual(1, storedObjects.Count);
			IEnumerator iterator = storedObjects.GetEnumerator();
			Assert.IsTrue(iterator.MoveNext());
			return (SPCChild)iterator.Current;
		}

		private void StartReplication()
		{
			A().Provider().StartReplicationTransaction(BSignature);
			B().Provider().StartReplicationTransaction(ASignature);
		}

		private void TstObjectUpdate()
		{
			SPCChild child = new SPCChild("c1");
			A().Provider().StoreNew(child);
			A().Provider().Commit();
			StartReplication();
			SPCChild reloaded = GetOneChildFromA();
			long oldVer = A().Provider().ProduceReference(reloaded, null, null).Version();
			CommitReplication();
			SPCChild reloaded2 = GetOneChildFromA();
			reloaded2.SetName("c3");
			//System.out.println("==============BEGIN DEBUG");
			A().Provider().Update(reloaded2);
			A().Provider().Commit();
			//System.out.println("==============END DEBUG");
			StartReplication();
			SPCChild reloaded3 = GetOneChildFromA();
			long newVer = A().Provider().ProduceReference(reloaded3, null, null).Version();
			CommitReplication();
			Assert.IsGreater(oldVer, newVer);
		}

		private void TstObjectsChangedSinceLastReplication()
		{
			Pilot object1 = new Pilot("John Cleese", 42);
			Pilot object2 = new Pilot("Terry Gilliam", 53);
			Car object3 = new Car("Volvo");
			A().Provider().StoreNew(object1);
			A().Provider().StoreNew(object2);
			A().Provider().StoreNew(object3);
			A().Provider().Commit();
			StartReplication();
			IObjectSet changed = A().Provider().ObjectsChangedSinceLastReplication();
			Assert.AreEqual(3, changed.Count);
			IObjectSet os = A().Provider().ObjectsChangedSinceLastReplication(typeof(Pilot));
			Assert.AreEqual(2, os.Count);
			IEnumerator pilots = os.GetEnumerator();
			//		Assert.isTrue(pilots.contains(findPilot("John Cleese")));
			//	Assert.isTrue(pilots.contains(findPilot("Terry Gilliam")));
			IEnumerator cars = A().Provider().ObjectsChangedSinceLastReplication(typeof(Car))
				.GetEnumerator();
			Assert.AreEqual("Volvo", ((Car)Next(cars)).GetModel());
			Assert.IsFalse(cars.MoveNext());
			CommitReplication();
			StartReplication();
			Assert.IsFalse(A().Provider().ObjectsChangedSinceLastReplication().GetEnumerator(
				).MoveNext());
			CommitReplication();
			Pilot pilot = (Pilot)Next(A().Provider().GetStoredObjects(typeof(Pilot)).GetEnumerator
				());
			pilot.SetName("Terry Jones");
			Car car = (Car)Next(A().Provider().GetStoredObjects(typeof(Car)).GetEnumerator());
			car.SetModel("McLaren");
			A().Provider().Update(pilot);
			A().Provider().Update(car);
			A().Provider().Commit();
			StartReplication();
			Assert.AreEqual(2, A().Provider().ObjectsChangedSinceLastReplication().Count);
			pilots = A().Provider().ObjectsChangedSinceLastReplication(typeof(Pilot)).GetEnumerator
				();
			Assert.AreEqual("Terry Jones", ((Pilot)Next(pilots)).Name());
			Assert.IsFalse(pilots.MoveNext());
			cars = A().Provider().ObjectsChangedSinceLastReplication(typeof(Car)).GetEnumerator
				();
			Assert.AreEqual("McLaren", ((Car)Next(cars)).GetModel());
			Assert.IsFalse(cars.MoveNext());
			CommitReplication();
			A().Provider().DeleteAllInstances(typeof(Pilot));
			A().Provider().DeleteAllInstances(typeof(Car));
			A().Provider().Commit();
		}

		private object Next(IEnumerator iterator)
		{
			Assert.IsTrue(iterator.MoveNext());
			return iterator.Current;
		}

		private void TstReferences()
		{
			Pilot pilot = new Pilot("tst References", 42);
			A().Provider().StoreNew(pilot);
			A().Provider().Commit();
			StartReplication();
			Pilot object1 = (Pilot)Next(A().Provider().GetStoredObjects(typeof(Pilot)).GetEnumerator
				());
			IReplicationReference reference = A().Provider().ProduceReference(object1, null, 
				null);
			Assert.AreEqual(object1, reference.Object());
			IDrsUUID uuid = reference.Uuid();
			IReplicationReference ref2 = A().Provider().ProduceReferenceByUUID(uuid, typeof(Pilot
				));
			Assert.AreEqual(reference, ref2);
			A().Provider().ClearAllReferences();
			IDrsUUID db4oUUID = A().Provider().ProduceReference(object1, null, null).Uuid();
			Assert.AreEqual(uuid, db4oUUID);
			CommitReplication();
			A().Provider().DeleteAllInstances(typeof(Pilot));
			A().Provider().Commit();
		}

		private void TstRollback()
		{
			if (!A().Provider().SupportsRollback())
			{
				return;
			}
			if (!B().Provider().SupportsRollback())
			{
				return;
			}
			StartReplication();
			Pilot object1 = new Pilot("Albert Kwan", 25);
			IDrsUUID uuid = new DrsUUIDImpl(new Db4oUUID(5678, BSignatureBytes));
			IReplicationReference @ref = new ReplicationReferenceImpl(object1, uuid, 1);
			A().Provider().ReferenceNewObject(object1, @ref, null, null);
			A().Provider().StoreReplica(object1);
			Assert.IsFalse(A().Provider().WasModifiedSinceLastReplication(@ref));
			A().Provider().RollbackReplication();
			A().Provider().StartReplicationTransaction(BSignature);
			Assert.IsNull(A().Provider().ProduceReference(object1, null, null));
			IReplicationReference byUUID = A().Provider().ProduceReferenceByUUID(uuid, object1
				.GetType());
			Assert.IsNull(byUUID);
			A().Provider().RollbackReplication();
			B().Provider().RollbackReplication();
		}

		private void TstSignature()
		{
			Assert.IsNotNull(A().Provider().GetSignature());
		}

		private void TstStore()
		{
			StartReplication();
			Pilot object1 = new Pilot("John Cleese", 42);
			IDrsUUID uuid = new DrsUUIDImpl(new Db4oUUID(15, BSignatureBytes));
			IReplicationReference @ref = new ReplicationReferenceImpl("ignoredSinceInOtherProvider"
				, uuid, 1);
			A().Provider().ReferenceNewObject(object1, @ref, null, null);
			A().Provider().StoreReplica(object1);
			IReplicationReference reference = A().Provider().ProduceReferenceByUUID(uuid, object1
				.GetType());
			Assert.AreEqual(reference, A().Provider().ProduceReference(object1, null, null));
			Assert.AreEqual(object1, reference.Object());
			CommitReplication();
			StartReplication();
			IEnumerator storedObjects = A().Provider().GetStoredObjects(typeof(Pilot)).GetEnumerator
				();
			Pilot reloaded = (Pilot)Next(storedObjects);
			Assert.IsFalse(storedObjects.MoveNext());
			reference = A().Provider().ProduceReferenceByUUID(uuid, object1.GetType());
			Assert.AreEqual(reference, A().Provider().ProduceReference(reloaded, null, null));
			reloaded.SetName("i am updated");
			A().Provider().StoreReplica(reloaded);
			A().Provider().ClearAllReferences();
			CommitReplication();
			StartReplication();
			reference = A().Provider().ProduceReferenceByUUID(uuid, reloaded.GetType());
			Assert.AreEqual("i am updated", ((Pilot)reference.Object()).Name());
			CommitReplication();
			A().Provider().DeleteAllInstances(typeof(Pilot));
			A().Provider().Commit();
		}

		private IDrsUUID Uuid(object obj)
		{
			return A().Provider().ProduceReference(obj, null, null).Uuid();
		}
	}
}
