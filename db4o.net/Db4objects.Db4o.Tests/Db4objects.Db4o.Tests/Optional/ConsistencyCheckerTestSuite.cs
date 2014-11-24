/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Consistency;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;
using Db4objects.Db4o.Tests.Optional;

namespace Db4objects.Db4o.Tests.Optional
{
	public class ConsistencyCheckerTestSuite : FixtureBasedTestSuite
	{
		public class BlockSizeSpec : ILabeled
		{
			private int _blockSize;

			public BlockSizeSpec(int blockSize)
			{
				_blockSize = blockSize;
			}

			public virtual int BlockSize()
			{
				return _blockSize;
			}

			public virtual string Label()
			{
				return _blockSize.ToString();
			}
		}

		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(ConsistencyCheckerTestSuite)).Run();
		}

		private static readonly FixtureVariable BlockSize = FixtureVariable.NewInstance("blockSize"
			);

		public override Type[] TestUnits()
		{
			return new Type[] { typeof(ConsistencyCheckerTestSuite.ConsistencyCheckerTestUnit
				) };
		}

		public override IFixtureProvider[] FixtureProviders()
		{
			return new IFixtureProvider[] { new SimpleFixtureProvider(BlockSize, new ConsistencyCheckerTestSuite.BlockSizeSpec
				[] { new ConsistencyCheckerTestSuite.BlockSizeSpec(1), new ConsistencyCheckerTestSuite.BlockSizeSpec
				(7), new ConsistencyCheckerTestSuite.BlockSizeSpec(9), new ConsistencyCheckerTestSuite.BlockSizeSpec
				(13), new ConsistencyCheckerTestSuite.BlockSizeSpec(17), new ConsistencyCheckerTestSuite.BlockSizeSpec
				(19) }) };
		}

		public class Item
		{
			public byte[] bytes = new byte[((ConsistencyCheckerTestSuite.BlockSizeSpec)BlockSize
				.Value).BlockSize()];
		}

		public class ConsistencyCheckerTestUnit : ITestLifeCycle
		{
			private LocalObjectContainer _db;

			public virtual void TestFreeUsedSlot()
			{
				AssertInconsistencyDetected(new _IProcedure4_66(this));
			}

			private sealed class _IProcedure4_66 : IProcedure4
			{
				public _IProcedure4_66(ConsistencyCheckerTestUnit _enclosing)
				{
					this._enclosing = _enclosing;
				}

				public void Apply(object item)
				{
					int id = (int)this._enclosing._db.GetID(((ConsistencyCheckerTestSuite.Item)item));
					Slot slot = this._enclosing._db.IdSystem().CommittedSlot(id);
					this._enclosing._db.FreespaceManager().Free(slot);
				}

				private readonly ConsistencyCheckerTestUnit _enclosing;
			}

			public virtual void TestFreeShiftedUsedSlot()
			{
				AssertInconsistencyDetected(new _IProcedure4_76(this));
			}

			private sealed class _IProcedure4_76 : IProcedure4
			{
				public _IProcedure4_76(ConsistencyCheckerTestUnit _enclosing)
				{
					this._enclosing = _enclosing;
				}

				public void Apply(object item)
				{
					int id = (int)this._enclosing._db.GetID(((ConsistencyCheckerTestSuite.Item)item));
					Slot slot = this._enclosing._db.IdSystem().CommittedSlot(id);
					this._enclosing._db.FreespaceManager().Free(new Slot(slot.Address() + 1, slot.Length
						()));
				}

				private readonly ConsistencyCheckerTestUnit _enclosing;
			}

			public virtual void TestNegativeAddressSlot()
			{
				AssertBogusSlotDetected(-1, 10);
			}

			public virtual void TestExceedsFileLengthSlot()
			{
				AssertBogusSlotDetected(int.MaxValue - 1, 1);
			}

			private void AssertBogusSlotDetected(int address, int length)
			{
				AssertInconsistencyDetected(new _IProcedure4_94(this, address, length));
			}

			private sealed class _IProcedure4_94 : IProcedure4
			{
				public _IProcedure4_94(ConsistencyCheckerTestUnit _enclosing, int address, int length
					)
				{
					this._enclosing = _enclosing;
					this.address = address;
					this.length = length;
				}

				public void Apply(object item)
				{
					int id = (int)this._enclosing._db.GetID(((ConsistencyCheckerTestSuite.Item)item));
					this._enclosing._db.IdSystem().Commit(new _IVisitable_97(id, address, length), FreespaceCommitter
						.DoNothing);
				}

				private sealed class _IVisitable_97 : IVisitable
				{
					public _IVisitable_97(int id, int address, int length)
					{
						this.id = id;
						this.address = address;
						this.length = length;
					}

					public void Accept(IVisitor4 visitor)
					{
						SlotChange slotChange = new SlotChange(id);
						slotChange.NotifySlotCreated(new Slot(address, length));
						visitor.Visit(slotChange);
					}

					private readonly int id;

					private readonly int address;

					private readonly int length;
				}

				private readonly ConsistencyCheckerTestUnit _enclosing;

				private readonly int address;

				private readonly int length;
			}

			private void AssertInconsistencyDetected(IProcedure4 proc)
			{
				ConsistencyCheckerTestSuite.Item item = new ConsistencyCheckerTestSuite.Item();
				_db.Store(item);
				_db.Commit();
				Assert.IsTrue(new ConsistencyChecker(_db).CheckSlotConsistency().Consistent());
				proc.Apply(item);
				_db.Commit();
				Assert.IsFalse(new ConsistencyChecker(_db).CheckSlotConsistency().Consistent());
			}

			/// <exception cref="System.Exception"></exception>
			public virtual void SetUp()
			{
				IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
				config.File.Storage = new MemoryStorage();
				config.File.BlockSize = ((ConsistencyCheckerTestSuite.BlockSizeSpec)BlockSize.Value
					).BlockSize();
				_db = (LocalObjectContainer)Db4oEmbedded.OpenFile(config, "inmem.db4o");
			}

			/// <exception cref="System.Exception"></exception>
			public virtual void TearDown()
			{
				_db.Close();
			}
		}
	}
}
