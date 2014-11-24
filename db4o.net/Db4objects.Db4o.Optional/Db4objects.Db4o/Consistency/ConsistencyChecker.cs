/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Consistency;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Classindex;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Consistency
{
	public class ConsistencyChecker
	{
		private readonly IList _bogusSlots = new ArrayList();

		private readonly LocalObjectContainer _db;

		private readonly OverlapMap _overlaps;

		public static void Main(string[] args)
		{
			IEmbeddedObjectContainer db = Db4oEmbedded.OpenFile(args[0]);
			try
			{
				Sharpen.Runtime.Out.WriteLine(new Db4objects.Db4o.Consistency.ConsistencyChecker(
					db).CheckSlotConsistency());
			}
			finally
			{
				db.Close();
			}
		}

		public ConsistencyChecker(IObjectContainer db)
		{
			_db = (LocalObjectContainer)db;
			_overlaps = new OverlapMap(_db.BlockConverter());
		}

		public virtual ConsistencyReport CheckSlotConsistency()
		{
			return ((ConsistencyReport)_db.SyncExec(new _IClosure4_38(this)));
		}

		private sealed class _IClosure4_38 : IClosure4
		{
			public _IClosure4_38(ConsistencyChecker _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public object Run()
			{
				this._enclosing.MapIdSystem();
				this._enclosing.MapFreespace();
				return new ConsistencyReport(this._enclosing._bogusSlots, this._enclosing._overlaps
					, this._enclosing.CheckClassIndices(), this._enclosing.CheckFieldIndices());
			}

			private readonly ConsistencyChecker _enclosing;
		}

		private IList CheckClassIndices()
		{
			IList invalidIds = new ArrayList();
			IIdSystem idSystem = _db.IdSystem();
			if (!(idSystem is BTreeIdSystem))
			{
				return invalidIds;
			}
			ClassMetadataIterator clazzIter = _db.ClassCollection().Iterator();
			while (clazzIter.MoveNext())
			{
				ClassMetadata clazz = clazzIter.CurrentClass();
				if (!clazz.HasClassIndex())
				{
					continue;
				}
				BTreeClassIndexStrategy index = (BTreeClassIndexStrategy)clazz.Index();
				index.TraverseIds(_db.SystemTransaction(), new _IVisitor4_64(this, invalidIds, clazz
					));
			}
			return invalidIds;
		}

		private sealed class _IVisitor4_64 : IVisitor4
		{
			public _IVisitor4_64(ConsistencyChecker _enclosing, IList invalidIds, ClassMetadata
				 clazz)
			{
				this._enclosing = _enclosing;
				this.invalidIds = invalidIds;
				this.clazz = clazz;
			}

			public void Visit(object id)
			{
				if (!this._enclosing.IdIsValid((((int)id))))
				{
					invalidIds.Add(new Pair(clazz.GetName(), ((int)id)));
				}
			}

			private readonly ConsistencyChecker _enclosing;

			private readonly IList invalidIds;

			private readonly ClassMetadata clazz;
		}

		private IList CheckFieldIndices()
		{
			IList invalidIds = new ArrayList();
			ClassMetadataIterator clazzIter = _db.ClassCollection().Iterator();
			while (clazzIter.MoveNext())
			{
				ClassMetadata clazz = clazzIter.CurrentClass();
				clazz.TraverseDeclaredFields(new _IProcedure4_80(this, invalidIds, clazz));
			}
			return invalidIds;
		}

		private sealed class _IProcedure4_80 : IProcedure4
		{
			public _IProcedure4_80(ConsistencyChecker _enclosing, IList invalidIds, ClassMetadata
				 clazz)
			{
				this._enclosing = _enclosing;
				this.invalidIds = invalidIds;
				this.clazz = clazz;
			}

			public void Apply(object field)
			{
				if (!((FieldMetadata)field).HasIndex())
				{
					return;
				}
				BTree fieldIndex = ((FieldMetadata)field).GetIndex(this._enclosing._db.SystemTransaction
					());
				fieldIndex.TraverseKeys(this._enclosing._db.SystemTransaction(), new _IVisitor4_86
					(this, invalidIds, clazz, field));
			}

			private sealed class _IVisitor4_86 : IVisitor4
			{
				public _IVisitor4_86(_IProcedure4_80 _enclosing, IList invalidIds, ClassMetadata 
					clazz, object field)
				{
					this._enclosing = _enclosing;
					this.invalidIds = invalidIds;
					this.clazz = clazz;
					this.field = field;
				}

				public void Visit(object fieldIndexKey)
				{
					int parentID = ((IFieldIndexKey)fieldIndexKey).ParentID();
					if (!this._enclosing._enclosing.IdIsValid(parentID))
					{
						invalidIds.Add(new Pair(clazz.GetName() + "#" + ((FieldMetadata)field).GetName(), 
							parentID));
					}
				}

				private readonly _IProcedure4_80 _enclosing;

				private readonly IList invalidIds;

				private readonly ClassMetadata clazz;

				private readonly object field;
			}

			private readonly ConsistencyChecker _enclosing;

			private readonly IList invalidIds;

			private readonly ClassMetadata clazz;
		}

		private bool IdIsValid(int id)
		{
			try
			{
				return !Slot.IsNull(_db.IdSystem().CommittedSlot(id));
			}
			catch (InvalidIDException)
			{
				return false;
			}
		}

		private void MapFreespace()
		{
			_db.FreespaceManager().Traverse(new _IVisitor4_110(this));
		}

		private sealed class _IVisitor4_110 : IVisitor4
		{
			public _IVisitor4_110(ConsistencyChecker _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object slot)
			{
				FreespaceSlotDetail detail = new FreespaceSlotDetail(((Slot)slot));
				if (this._enclosing.IsBogusSlot(((Slot)slot).Address(), ((Slot)slot).Length()))
				{
					this._enclosing._bogusSlots.Add(detail);
				}
				this._enclosing._overlaps.Add(detail);
			}

			private readonly ConsistencyChecker _enclosing;
		}

		private void MapIdSystem()
		{
			IIdSystem idSystem = _db.IdSystem();
			if (!(idSystem is BTreeIdSystem))
			{
				Sharpen.Runtime.Err.WriteLine("No btree id system found - not mapping ids.");
				return;
			}
			((BTreeIdSystem)idSystem).TraverseIds(new _IVisitor4_127(this));
			idSystem.TraverseOwnSlots(new _IProcedure4_138(this));
		}

		private sealed class _IVisitor4_127 : IVisitor4
		{
			public _IVisitor4_127(ConsistencyChecker _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object mapping)
			{
				SlotDetail detail = new IdObjectSlotDetail(((IdSlotMapping)mapping)._id, ((IdSlotMapping
					)mapping).Slot());
				if (this._enclosing.IsBogusSlot(((IdSlotMapping)mapping)._address, ((IdSlotMapping
					)mapping)._length))
				{
					this._enclosing._bogusSlots.Add(detail);
				}
				if (((IdSlotMapping)mapping)._address > 0)
				{
					this._enclosing._overlaps.Add(detail);
				}
			}

			private readonly ConsistencyChecker _enclosing;
		}

		private sealed class _IProcedure4_138 : IProcedure4
		{
			public _IProcedure4_138(ConsistencyChecker _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Apply(object idSlot)
			{
				int id = (((int)((Pair)idSlot).first));
				Slot slot = ((Slot)((Pair)idSlot).second);
				SlotDetail detail = id > 0 ? (SlotDetail)new IdObjectSlotDetail(id, slot) : (SlotDetail
					)new RawObjectSlotDetail(slot);
				if (this._enclosing.IsBogusSlot(((Slot)((Pair)idSlot).second).Address(), ((Slot)(
					(Pair)idSlot).second).Length()))
				{
					this._enclosing._bogusSlots.Add(detail);
				}
				this._enclosing._overlaps.Add(detail);
			}

			private readonly ConsistencyChecker _enclosing;
		}

		private bool IsBogusSlot(int address, int length)
		{
			return address < 0 || (long)address + length > _db.FileLength();
		}
	}
}
