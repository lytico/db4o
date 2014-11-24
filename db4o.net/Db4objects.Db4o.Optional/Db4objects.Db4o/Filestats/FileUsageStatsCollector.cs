/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Filestats;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Classindex;
using Db4objects.Db4o.Internal.Fileheader;
using Db4objects.Db4o.Internal.Freespace;
using Db4objects.Db4o.Internal.Ids;
using Db4objects.Db4o.Internal.Slots;

namespace Db4objects.Db4o.Filestats
{
	/// <summary>
	/// Collects database file usage statistics and prints them
	/// to the console.
	/// </summary>
	/// <remarks>
	/// Collects database file usage statistics and prints them
	/// to the console.
	/// </remarks>
	public partial class FileUsageStatsCollector
	{
		private readonly IDictionary MiscCollectors;

		/// <summary>Usage: FileUsageStatsCollector <db path> [<collect gaps (true|false)>]</summary>
		public static void Main(string[] args)
		{
			string dbPath = args[0];
			bool collectSlots = args.Length > 1 && "true".Equals(args[1]);
			Sharpen.Runtime.Out.WriteLine(dbPath + ": " + new Sharpen.IO.File(dbPath).Length(
				));
			FileUsageStats stats = RunStats(dbPath, collectSlots);
			Sharpen.Runtime.Out.WriteLine(stats);
		}

		public static FileUsageStats RunStats(string dbPath)
		{
			return RunStats(dbPath, false);
		}

		public static FileUsageStats RunStats(string dbPath, bool collectSlots)
		{
			return RunStats(dbPath, collectSlots, Db4oEmbedded.NewConfiguration());
		}

		public static FileUsageStats RunStats(string dbPath, bool collectSlots, IEmbeddedConfiguration
			 config)
		{
			IEmbeddedObjectContainer db = Db4oEmbedded.OpenFile(config, dbPath);
			try
			{
				return new Db4objects.Db4o.Filestats.FileUsageStatsCollector(db, collectSlots).CollectStats
					();
			}
			finally
			{
				db.Close();
			}
		}

		private readonly LocalObjectContainer _db;

		private FileUsageStats _stats;

		private IBlockConverter _blockConverter;

		private readonly ISlotMap _slots;

		public FileUsageStatsCollector(IObjectContainer db, bool collectSlots)
		{
			MiscCollectors = new Hashtable();
			RegisterBigSetCollector();
			_db = (LocalObjectContainer)db;
			byte blockSize = _db.BlockSize();
			_blockConverter = blockSize > 1 ? (IBlockConverter)new BlockSizeBlockConverter(blockSize
				) : (IBlockConverter)new DisabledBlockConverter();
			_slots = collectSlots ? (ISlotMap)new SlotMapImpl(_db.FileLength()) : (ISlotMap)new 
				NullSlotMap();
		}

		public virtual FileUsageStats CollectStats()
		{
			_stats = new FileUsageStats(_db.FileLength(), FileHeaderUsage(), IdSystemUsage(), 
				Freespace(), ClassMetadataUsage(), FreespaceUsage(), UuidUsage(), _slots, CommitTimestampUsage
				());
			Sharpen.Util.ISet classRoots = ClassNode.BuildHierarchy(_db.ClassCollection());
			for (IEnumerator classRootIter = classRoots.GetEnumerator(); classRootIter.MoveNext
				(); )
			{
				ClassNode classRoot = ((ClassNode)classRootIter.Current);
				CollectClassSlots(classRoot.ClassMetadata());
				CollectClassStats(_stats, classRoot);
			}
			return _stats;
		}

		private long CollectClassStats(FileUsageStats stats, ClassNode classNode)
		{
			long subClassSlotUsage = 0;
			for (IEnumerator curSubClassIter = classNode.SubClasses().GetEnumerator(); curSubClassIter
				.MoveNext(); )
			{
				ClassNode curSubClass = ((ClassNode)curSubClassIter.Current);
				subClassSlotUsage += CollectClassStats(stats, curSubClass);
			}
			ClassMetadata clazz = classNode.ClassMetadata();
			long classIndexUsage = 0;
			if (clazz.HasClassIndex())
			{
				classIndexUsage = BTreeUsage(((BTreeClassIndexStrategy)clazz.Index()).Btree());
			}
			long fieldIndexUsage = FieldIndexUsage(clazz);
			FileUsageStatsCollector.InstanceUsage instanceUsage = ClassSlotUsage(clazz);
			long totalSlotUsage = instanceUsage.slotUsage;
			long ownSlotUsage = totalSlotUsage - subClassSlotUsage;
			ClassUsageStats classStats = new ClassUsageStats(clazz.GetName(), ownSlotUsage, classIndexUsage
				, fieldIndexUsage, instanceUsage.miscUsage, instanceUsage.numInstances);
			stats.AddClassStats(classStats);
			return totalSlotUsage;
		}

		private long FieldIndexUsage(ClassMetadata classMetadata)
		{
			LongByRef usage = new LongByRef();
			classMetadata.TraverseDeclaredFields(new _IProcedure4_125(this, usage));
			return usage.value;
		}

		private sealed class _IProcedure4_125 : IProcedure4
		{
			public _IProcedure4_125(FileUsageStatsCollector _enclosing, LongByRef usage)
			{
				this._enclosing = _enclosing;
				this.usage = usage;
			}

			public void Apply(object field)
			{
				if (((FieldMetadata)field).IsVirtual() || !((FieldMetadata)field).HasIndex())
				{
					return;
				}
				usage.value += this._enclosing.BTreeUsage(((FieldMetadata)field).GetIndex(this._enclosing
					._db.SystemTransaction()));
			}

			private readonly FileUsageStatsCollector _enclosing;

			private readonly LongByRef usage;
		}

		private long BTreeUsage(BTree btree)
		{
			return BTreeUsage(_db, btree, _slots);
		}

		internal static long BTreeUsage(LocalObjectContainer db, BTree btree, ISlotMap slotMap
			)
		{
			return BTreeUsage(db.SystemTransaction(), db.IdSystem(), btree, slotMap);
		}

		private static long BTreeUsage(Transaction transaction, IIdSystem idSystem, BTree
			 btree, ISlotMap slotMap)
		{
			IEnumerator nodeIter = btree.AllNodeIds(transaction);
			Db4objects.Db4o.Internal.Slots.Slot btreeSlot = idSystem.CommittedSlot(btree.GetID
				());
			slotMap.Add(btreeSlot);
			long usage = btreeSlot.Length();
			while (nodeIter.MoveNext())
			{
				int curNodeId = ((int)nodeIter.Current);
				Db4objects.Db4o.Internal.Slots.Slot slot = idSystem.CommittedSlot(curNodeId);
				slotMap.Add(slot);
				usage += slot.Length();
			}
			return usage;
		}

		private FileUsageStatsCollector.InstanceUsage ClassSlotUsage(ClassMetadata clazz)
		{
			if (!clazz.HasClassIndex())
			{
				return new FileUsageStatsCollector.InstanceUsage(0, 0, 0);
			}
			IMiscCollector miscCollector = ((IMiscCollector)MiscCollectors[clazz.GetName()]);
			LongByRef slotUsage = new LongByRef();
			LongByRef miscUsage = new LongByRef();
			IntByRef numInstances = new IntByRef();
			BTreeClassIndexStrategy index = (BTreeClassIndexStrategy)clazz.Index();
			index.TraverseIds(_db.SystemTransaction(), new _IVisitor4_167(this, numInstances, 
				slotUsage, miscCollector, miscUsage));
			return new FileUsageStatsCollector.InstanceUsage(slotUsage.value, miscUsage.value
				, numInstances.value);
		}

		private sealed class _IVisitor4_167 : IVisitor4
		{
			public _IVisitor4_167(FileUsageStatsCollector _enclosing, IntByRef numInstances, 
				LongByRef slotUsage, IMiscCollector miscCollector, LongByRef miscUsage)
			{
				this._enclosing = _enclosing;
				this.numInstances = numInstances;
				this.slotUsage = slotUsage;
				this.miscCollector = miscCollector;
				this.miscUsage = miscUsage;
			}

			public void Visit(object id)
			{
				numInstances.value++;
				slotUsage.value += this._enclosing.SlotSizeForId((((int)id)));
				if (miscCollector != null)
				{
					miscUsage.value += miscCollector.CollectFor(this._enclosing._db, (((int)id)), this
						._enclosing._slots);
				}
			}

			private readonly FileUsageStatsCollector _enclosing;

			private readonly IntByRef numInstances;

			private readonly LongByRef slotUsage;

			private readonly IMiscCollector miscCollector;

			private readonly LongByRef miscUsage;
		}

		private void CollectClassSlots(ClassMetadata clazz)
		{
			if (!clazz.HasClassIndex())
			{
				return;
			}
			BTreeClassIndexStrategy index = (BTreeClassIndexStrategy)clazz.Index();
			index.TraverseIds(_db.SystemTransaction(), new _IVisitor4_184(this));
		}

		private sealed class _IVisitor4_184 : IVisitor4
		{
			public _IVisitor4_184(FileUsageStatsCollector _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object id)
			{
				this._enclosing._slots.Add(this._enclosing.Slot((((int)id))));
			}

			private readonly FileUsageStatsCollector _enclosing;
		}

		private long Freespace()
		{
			_db.FreespaceManager().Traverse(new _IVisitor4_192(this));
			return _db.FreespaceManager().TotalFreespace();
		}

		private sealed class _IVisitor4_192 : IVisitor4
		{
			public _IVisitor4_192(FileUsageStatsCollector _enclosing)
			{
				this._enclosing = _enclosing;
			}

			public void Visit(object slot)
			{
				this._enclosing._slots.Add(((Db4objects.Db4o.Internal.Slots.Slot)slot));
			}

			private readonly FileUsageStatsCollector _enclosing;
		}

		private long FreespaceUsage()
		{
			return FreespaceUsage(_db.FreespaceManager());
		}

		private long FreespaceUsage(IFreespaceManager fsm)
		{
			if (fsm is InMemoryFreespaceManager)
			{
				return 0;
			}
			if (fsm is BTreeFreespaceManager)
			{
				return BTreeUsage((BTree)FieldValue(fsm, "_slotsByAddress")) + BTreeUsage((BTree)
					FieldValue(fsm, "_slotsByLength"));
			}
			if (fsm is BlockAwareFreespaceManager)
			{
				return FreespaceUsage((IFreespaceManager)FieldValue(fsm, "_delegate"));
			}
			throw new InvalidOperationException("Unknown freespace manager: " + fsm);
		}

		private long IdSystemUsage()
		{
			IntByRef usage = new IntByRef();
			_db.IdSystem().TraverseOwnSlots(new _IProcedure4_219(this, usage));
			return usage.value;
		}

		private sealed class _IProcedure4_219 : IProcedure4
		{
			public _IProcedure4_219(FileUsageStatsCollector _enclosing, IntByRef usage)
			{
				this._enclosing = _enclosing;
				this.usage = usage;
			}

			public void Apply(object idSlot)
			{
				Db4objects.Db4o.Internal.Slots.Slot slot = ((Db4objects.Db4o.Internal.Slots.Slot)
					((Pair)idSlot).second);
				usage.value += slot.Length();
				this._enclosing._slots.Add(slot);
			}

			private readonly FileUsageStatsCollector _enclosing;

			private readonly IntByRef usage;
		}

		private long ClassMetadataUsage()
		{
			Db4objects.Db4o.Internal.Slots.Slot classRepositorySlot = Slot(_db.ClassCollection
				().GetID());
			_slots.Add(classRepositorySlot);
			long usage = classRepositorySlot.Length();
			IEnumerator classIdIter = _db.ClassCollection().Ids();
			while (classIdIter.MoveNext())
			{
				int curClassId = (((int)classIdIter.Current));
				Db4objects.Db4o.Internal.Slots.Slot classSlot = Slot(curClassId);
				_slots.Add(classSlot);
				usage += classSlot.Length();
			}
			return usage;
		}

		private long FileHeaderUsage()
		{
			int headerLength = _db.GetFileHeader().Length();
			int usage = _blockConverter.BlockAlignedBytes(headerLength);
			FileHeaderVariablePart2 variablePart = (FileHeaderVariablePart2)FieldValue(_db.GetFileHeader
				(), "_variablePart");
			usage += _blockConverter.BlockAlignedBytes(variablePart.MarshalledLength());
			_slots.Add(new Db4objects.Db4o.Internal.Slots.Slot(0, headerLength));
			_slots.Add(new Db4objects.Db4o.Internal.Slots.Slot(variablePart.Address(), variablePart
				.MarshalledLength()));
			return usage;
		}

		private long UuidUsage()
		{
			if (_db.SystemData().UuidIndexId() <= 0)
			{
				return 0;
			}
			BTree index = _db.UUIDIndex().GetIndex(_db.SystemTransaction());
			return index == null ? 0 : BTreeUsage(index);
		}

		private long CommitTimestampUsage()
		{
			LocalTransaction st = (LocalTransaction)_db.SystemTransaction();
			CommitTimestampSupport commitTimestampSupport = st.CommitTimestampSupport();
			if (commitTimestampSupport == null)
			{
				return 0;
			}
			BTree idToTimestampBtree = commitTimestampSupport.IdToTimestamp();
			long idToTimestampBTreeSize = idToTimestampBtree == null ? 0 : BTreeUsage(idToTimestampBtree
				);
			BTree timestampToIdBtree = commitTimestampSupport.TimestampToId();
			long timestampToIdBTreeSize = timestampToIdBtree == null ? 0 : BTreeUsage(timestampToIdBtree
				);
			return idToTimestampBTreeSize + timestampToIdBTreeSize;
		}

		private int SlotSizeForId(int id)
		{
			return Slot(id).Length();
		}

		private static object FieldValue(object parent, string fieldName)
		{
			return (object)Reflection4.GetFieldValue(parent, fieldName);
		}

		private class InstanceUsage
		{
			public readonly long slotUsage;

			public readonly long miscUsage;

			public readonly int numInstances;

			public InstanceUsage(long slotUsage, long miscUsage, int numInstances)
			{
				this.slotUsage = slotUsage;
				this.miscUsage = miscUsage;
				this.numInstances = numInstances;
			}
		}

		private Slot Slot(int id)
		{
			return _db.IdSystem().CommittedSlot(id);
		}
	}
}
#endif // !SILVERLIGHT
