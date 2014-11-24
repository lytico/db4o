/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Defragment;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;
using Db4objects.Db4o.Internal.Collections;
using Db4objects.Db4o.Internal.Delete;
using Db4objects.Db4o.Internal.Marshall;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Internal.Collections
{
	/// <exclude></exclude>
	public class BigSetTypeHandler : IReferenceTypeHandler, ICascadingTypeHandler
	{
		public virtual void Defragment(IDefragmentContext context)
		{
			int pos = context.Offset();
			int id = context.ReadInt();
			BTree bTree = NewBTree(context, id);
			DefragmentServicesImpl services = (DefragmentServicesImpl)context.Services();
			IDMappingCollector collector = new IDMappingCollector();
			services.RegisterBTreeIDs(bTree, collector);
			collector.Flush(services);
			context.Seek(pos);
			context.CopyID();
			bTree.DefragBTree(services);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual void Delete(IDeleteContext context)
		{
			InvalidBigSet(context);
			int id = context.ReadInt();
			FreeBTree(context, id);
		}

		private void InvalidBigSet(IDeleteContext context)
		{
			IBigSetPersistence bigSet = (IBigSetPersistence)context.Transaction().ObjectForIdFromCache
				(context.ObjectId());
			if (bigSet != null)
			{
				bigSet.Invalidate();
			}
		}

		private void FreeBTree(IDeleteContext context, int id)
		{
			BTree bTree = NewBTree(context, id);
			bTree.Free(SystemTransaction(context));
			bTree = null;
		}

		private static LocalTransaction SystemTransaction(IContext context)
		{
			return (LocalTransaction)context.Transaction().SystemTransaction();
		}

		private BTree NewBTree(IContext context, int id)
		{
			return new BTree(SystemTransaction(context), id, new IDHandler());
		}

		public virtual void Write(IWriteContext context, object obj)
		{
			IBigSetPersistence bigSet = (IBigSetPersistence)obj;
			bigSet.Write(context);
		}

		public virtual IPreparedComparison PrepareComparison(IContext context, object obj
			)
		{
			// TODO Auto-generated method stub
			return null;
		}

		public virtual void Activate(IReferenceActivationContext context)
		{
			IBigSetPersistence bigSet = (IBigSetPersistence)context.PersistentObject();
			bigSet.Read(context);
		}

		public virtual void CascadeActivation(IActivationContext context)
		{
		}

		// TODO Auto-generated method stub
		public virtual void CollectIDs(QueryingReadContext context)
		{
			int id = context.ReadInt();
			BTree bTree = NewBTree(context, id);
			bTree.TraverseKeys(context.Transaction(), new _IVisitor4_83(context));
		}

		private sealed class _IVisitor4_83 : IVisitor4
		{
			public _IVisitor4_83(QueryingReadContext context)
			{
				this.context = context;
			}

			public void Visit(object id)
			{
				context.AddId((((int)id)));
			}

			private readonly QueryingReadContext context;
		}

		public virtual ITypeHandler4 ReadCandidateHandler(QueryingReadContext context)
		{
			// TODO Auto-generated method stub
			return null;
		}
	}
}
