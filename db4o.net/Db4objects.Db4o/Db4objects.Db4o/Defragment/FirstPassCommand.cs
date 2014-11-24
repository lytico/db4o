/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Metadata;

namespace Db4objects.Db4o.Defragment
{
	/// <summary>
	/// First step in the defragmenting process: Allocates pointer slots in the target file for
	/// each ID (but doesn't fill them in, yet) and registers the mapping from source pointer address
	/// to target pointer address.
	/// </summary>
	/// <remarks>
	/// First step in the defragmenting process: Allocates pointer slots in the target file for
	/// each ID (but doesn't fill them in, yet) and registers the mapping from source pointer address
	/// to target pointer address.
	/// </remarks>
	/// <exclude></exclude>
	public sealed class FirstPassCommand : IPassCommand
	{
		private IDMappingCollector _collector = new IDMappingCollector();

		public void ProcessClass(DefragmentServicesImpl context, ClassMetadata classMetadata
			, int id, int classIndexID)
		{
			_collector.CreateIDMapping(context, id, true);
			classMetadata.TraverseAllAspects(new _TraverseFieldCommand_24(this, context));
		}

		private sealed class _TraverseFieldCommand_24 : TraverseFieldCommand
		{
			public _TraverseFieldCommand_24(FirstPassCommand _enclosing, DefragmentServicesImpl
				 context)
			{
				this._enclosing = _enclosing;
				this.context = context;
			}

			protected override void Process(FieldMetadata field)
			{
				if (!field.IsVirtual() && field.HasIndex())
				{
					this._enclosing.ProcessBTree(context, field.GetIndex(context.SystemTrans()));
				}
			}

			private readonly FirstPassCommand _enclosing;

			private readonly DefragmentServicesImpl context;
		}

		public void ProcessObjectSlot(DefragmentServicesImpl context, ClassMetadata classMetadata
			, int sourceID)
		{
			_collector.CreateIDMapping(context, sourceID, false);
		}

		/// <exception cref="Db4objects.Db4o.CorruptionException"></exception>
		public void ProcessClassCollection(DefragmentServicesImpl context)
		{
			_collector.CreateIDMapping(context, context.SourceClassCollectionID(), false);
		}

		public void ProcessBTree(DefragmentServicesImpl context, BTree btree)
		{
			context.RegisterBTreeIDs(btree, _collector);
		}

		public void Flush(DefragmentServicesImpl context)
		{
			_collector.Flush(context);
		}
	}
}
