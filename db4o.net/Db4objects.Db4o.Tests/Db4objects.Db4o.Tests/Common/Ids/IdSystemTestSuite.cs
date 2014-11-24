/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4oUnit.Fixtures;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Config;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Tests.Common.Ids;

namespace Db4objects.Db4o.Tests.Common.Ids
{
	public class IdSystemTestSuite : FixtureBasedTestSuite
	{
		private static int MaxValidId = 1000;

		private const int SlotLength = 10;

		public class IdSystemTestUnit : AbstractDb4oTestCase, IOptOutMultiSession, IDb4oTestCase
		{
			/// <exception cref="System.Exception"></exception>
			protected override void Configure(IConfiguration config)
			{
				IIdSystemConfiguration idSystemConfiguration = Db4oLegacyConfigurationBridge.AsIdSystemConfiguration
					(config);
				((IdSystemTestSuite.IIdSystemProvider)_fixture.Value).Apply(idSystemConfiguration
					);
			}

			public virtual void TestSlotForNewIdDoesNotExist()
			{
				int newId = IdSystem().NewId();
				Slot oldSlot = null;
				try
				{
					oldSlot = IdSystem().CommittedSlot(newId);
				}
				catch (InvalidIDException)
				{
				}
				Assert.IsFalse(IsValid(oldSlot));
			}

			public virtual void TestSingleNewSlot()
			{
				int id = IdSystem().NewId();
				Assert.AreEqual(AllocateNewSlot(id), IdSystem().CommittedSlot(id));
			}

			public virtual void TestSingleSlotUpdate()
			{
				int id = IdSystem().NewId();
				AllocateNewSlot(id);
				SlotChange slotChange = SlotChangeFactory.UserObjects.NewInstance(id);
				Slot updatedSlot = LocalContainer().AllocateSlot(SlotLength);
				slotChange.NotifySlotUpdated(FreespaceManager(), updatedSlot);
				Commit(new SlotChange[] { slotChange });
				Assert.AreEqual(updatedSlot, IdSystem().CommittedSlot(id));
			}

			public virtual void TestSingleSlotDelete()
			{
				int id = IdSystem().NewId();
				AllocateNewSlot(id);
				SlotChange slotChange = SlotChangeFactory.UserObjects.NewInstance(id);
				slotChange.NotifyDeleted(FreespaceManager());
				Commit(new SlotChange[] { slotChange });
				Assert.IsFalse(IsValid(IdSystem().CommittedSlot(id)));
			}

			public virtual void TestReturnUnusedIds()
			{
				int id = IdSystem().NewId();
				Slot slot = IdSystem().CommittedSlot(id);
				Assert.AreEqual(Slot.Zero, slot);
				IdSystem().ReturnUnusedIds(new _IVisitable_83(id));
				if (IdSystem() is PointerBasedIdSystem)
				{
					slot = IdSystem().CommittedSlot(id);
					Assert.AreEqual(Slot.Zero, slot);
				}
				else
				{
					Assert.Expect(typeof(InvalidIDException), new _ICodeBlock_93(this, id));
				}
			}

			private sealed class _IVisitable_83 : IVisitable
			{
				public _IVisitable_83(int id)
				{
					this.id = id;
				}

				public void Accept(IVisitor4 visitor)
				{
					visitor.Visit(id);
				}

				private readonly int id;
			}

			private sealed class _ICodeBlock_93 : ICodeBlock
			{
				public _ICodeBlock_93(IdSystemTestUnit _enclosing, int id)
				{
					this._enclosing = _enclosing;
					this.id = id;
				}

				/// <exception cref="System.Exception"></exception>
				public void Run()
				{
					this._enclosing.IdSystem().CommittedSlot(id);
				}

				private readonly IdSystemTestUnit _enclosing;

				private readonly int id;
			}

			private Slot AllocateNewSlot(int newId)
			{
				SlotChange slotChange = SlotChangeFactory.UserObjects.NewInstance(newId);
				Slot allocatedSlot = LocalContainer().AllocateSlot(SlotLength);
				slotChange.NotifySlotCreated(allocatedSlot);
				Commit(new SlotChange[] { slotChange });
				return allocatedSlot;
			}

			private void Commit(SlotChange[] slotChanges)
			{
				IdSystem().Commit(new _IVisitable_112(slotChanges), FreespaceCommitter.DoNothing);
			}

			private sealed class _IVisitable_112 : IVisitable
			{
				public _IVisitable_112(SlotChange[] slotChanges)
				{
					this.slotChanges = slotChanges;
				}

				public void Accept(IVisitor4 visitor)
				{
					for (int slotChangeIndex = 0; slotChangeIndex < slotChanges.Length; ++slotChangeIndex)
					{
						SlotChange slotChange = slotChanges[slotChangeIndex];
						visitor.Visit(slotChange);
					}
				}

				private readonly SlotChange[] slotChanges;
			}

			private LocalObjectContainer LocalContainer()
			{
				return (LocalObjectContainer)Container();
			}

			private bool IsValid(Slot slot)
			{
				return !Slot.IsNull(slot);
			}

			private IFreespaceManager FreespaceManager()
			{
				return LocalContainer().FreespaceManager();
			}

			private IIdSystem IdSystem()
			{
				return LocalContainer().IdSystem();
			}
		}

		public class IdOverflowTestUnit : AbstractDb4oTestCase, IOptOutMultiSession, IDb4oTestCase
		{
			public virtual void TestNewIdOverflow()
			{
				if (!((IdSystemTestSuite.IIdSystemProvider)_fixture.Value).SupportsIdOverflow())
				{
					return;
				}
				LocalObjectContainer container = (LocalObjectContainer)Container();
				IIdSystem idSystem = ((IdSystemTestSuite.IIdSystemProvider)_fixture.Value).NewInstance
					(container);
				IList allFreeIds = AllocateAllAvailableIds(idSystem);
				AssertNoMoreIdAvailable(idSystem);
				IList subSetOfIds = new ArrayList();
				int counter = 0;
				for (IEnumerator currentIdIter = allFreeIds.GetEnumerator(); currentIdIter.MoveNext
					(); )
				{
					int currentId = ((int)currentIdIter.Current);
					counter++;
					if (counter % 3 == 0)
					{
						subSetOfIds.Add(currentId);
					}
				}
				AssertFreeAndReallocate(idSystem, subSetOfIds);
				AssertFreeAndReallocate(idSystem, allFreeIds);
			}

			private void AssertFreeAndReallocate(IIdSystem idSystem, IList ids)
			{
				// Boundary condition: Last ID. Produced a bug when implementing. 
				if (!ids.Contains(MaxValidId))
				{
					ids.Add(MaxValidId);
				}
				Assert.IsGreater(0, ids.Count);
				idSystem.ReturnUnusedIds(new _IVisitable_184(ids));
				int freedCount = ids.Count;
				for (int i = 0; i < freedCount; i++)
				{
					int newId = idSystem.NewId();
					Assert.IsTrue(ids.Contains(newId));
					ids.Remove((object)newId);
				}
				Assert.IsTrue(ids.Count == 0);
				AssertNoMoreIdAvailable(idSystem);
			}

			private sealed class _IVisitable_184 : IVisitable
			{
				public _IVisitable_184(IList ids)
				{
					this.ids = ids;
				}

				public void Accept(IVisitor4 visitor)
				{
					for (IEnumerator expectedFreeIdIter = ids.GetEnumerator(); expectedFreeIdIter.MoveNext
						(); )
					{
						int expectedFreeId = ((int)expectedFreeIdIter.Current);
						visitor.Visit(expectedFreeId);
					}
				}

				private readonly IList ids;
			}

			private IList AllocateAllAvailableIds(IIdSystem idSystem)
			{
				IList ids = new ArrayList();
				int newId = 0;
				do
				{
					newId = idSystem.NewId();
					ids.Add(newId);
				}
				while (newId < MaxValidId);
				return ids;
			}

			private void AssertNoMoreIdAvailable(IIdSystem idSystem)
			{
				Assert.Expect(typeof(Db4oFatalException), new _ICodeBlock_219(idSystem));
			}

			private sealed class _ICodeBlock_219 : ICodeBlock
			{
				public _ICodeBlock_219(IIdSystem idSystem)
				{
					this.idSystem = idSystem;
				}

				/// <exception cref="System.Exception"></exception>
				public void Run()
				{
					idSystem.NewId();
				}

				private readonly IIdSystem idSystem;
			}
		}

		private static FixtureVariable _fixture = FixtureVariable.NewInstance("IdSystem");

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[] { new Db4oFixtureProvider(), new SimpleFixtureProvider
				(_fixture, new IdSystemTestSuite.IIdSystemProvider[] { new _IIdSystemProvider_236
				(), new _IIdSystemProvider_253(), new _IIdSystemProvider_270() }) };
		}

		private sealed class _IIdSystemProvider_236 : IdSystemTestSuite.IIdSystemProvider
		{
			public _IIdSystemProvider_236()
			{
			}

			public void Apply(IIdSystemConfiguration idSystemConfiguration)
			{
				idSystemConfiguration.UsePointerBasedSystem();
			}

			public IIdSystem NewInstance(LocalObjectContainer container)
			{
				return null;
			}

			public bool SupportsIdOverflow()
			{
				return false;
			}

			public string Label()
			{
				return "PointerBased";
			}
		}

		private sealed class _IIdSystemProvider_253 : IdSystemTestSuite.IIdSystemProvider
		{
			public _IIdSystemProvider_253()
			{
			}

			public void Apply(IIdSystemConfiguration idSystemConfiguration)
			{
				idSystemConfiguration.UseInMemorySystem();
			}

			public IIdSystem NewInstance(LocalObjectContainer container)
			{
				return new InMemoryIdSystem(container, IdSystemTestSuite.MaxValidId);
			}

			public bool SupportsIdOverflow()
			{
				return true;
			}

			public string Label()
			{
				return "InMemory";
			}
		}

		private sealed class _IIdSystemProvider_270 : IdSystemTestSuite.IIdSystemProvider
		{
			public _IIdSystemProvider_270()
			{
			}

			public void Apply(IIdSystemConfiguration idSystemConfiguration)
			{
				idSystemConfiguration.UseStackedBTreeSystem();
			}

			public IIdSystem NewInstance(LocalObjectContainer container)
			{
				return new BTreeIdSystem(container, new InMemoryIdSystem(container), IdSystemTestSuite
					.MaxValidId);
			}

			public bool SupportsIdOverflow()
			{
				// FIXME: implement next
				return false;
			}

			public string Label()
			{
				return "BTree";
			}
		}

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(IdSystemTestSuite.IdSystemTestUnit), typeof(IdSystemTestSuite.IdOverflowTestUnit
				) };
		}

		private interface IIdSystemProvider : ILabeled
		{
			void Apply(IIdSystemConfiguration idSystemConfiguration);

			bool SupportsIdOverflow();

			IIdSystem NewInstance(LocalObjectContainer container);
		}
	}
}
