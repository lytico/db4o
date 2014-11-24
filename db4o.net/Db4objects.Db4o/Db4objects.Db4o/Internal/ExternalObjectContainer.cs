/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Activation;
using Db4objects.Db4o.Internal.Events;
using Db4objects.Db4o.Query;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public abstract class ExternalObjectContainer : ObjectContainerBase
	{
		public ExternalObjectContainer(IConfiguration config) : base(config)
		{
		}

		public sealed override void Activate(object obj)
		{
			Activate(null, obj);
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public sealed override void Activate(object obj, int depth)
		{
			Activate(null, obj, ActivationDepthProvider().ActivationDepth(depth, ActivationMode
				.Activate));
		}

		public sealed override void Deactivate(object obj)
		{
			Deactivate(null, obj);
		}

		/// <exception cref="System.ArgumentNullException"></exception>
		/// <exception cref="System.ArgumentException"></exception>
		public sealed override void Bind(object obj, long id)
		{
			Bind(null, obj, id);
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public sealed override void Commit()
		{
			Commit(null);
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public sealed override void Deactivate(object obj, int depth)
		{
			Deactivate(null, obj, depth);
		}

		public sealed override void Delete(object a_object)
		{
			Delete(null, a_object);
		}

		public override object Descend(object obj, string[] path)
		{
			return Descend(null, obj, path);
		}

		public override IExtObjectContainer Ext()
		{
			return this;
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public sealed override IObjectSet QueryByExample(object template)
		{
			return QueryByExample(null, template);
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.InvalidIDException"></exception>
		public sealed override object GetByID(long id)
		{
			return GetByID(null, id);
		}

		public sealed override object GetByUUID(Db4oUUID uuid)
		{
			return GetByUUID(null, uuid);
		}

		public sealed override long GetID(object obj)
		{
			return GetID(null, obj);
		}

		public sealed override IObjectInfo GetObjectInfo(object obj)
		{
			return GetObjectInfo(null, obj);
		}

		public override bool IsActive(object obj)
		{
			return IsActive(null, obj);
		}

		public override bool IsCached(long id)
		{
			return IsCached(null, id);
		}

		public override bool IsStored(object obj)
		{
			return IsStored(null, obj);
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public sealed override object PeekPersisted(object obj, int depth, bool committed
			)
		{
			return PeekPersisted(null, obj, ActivationDepthProvider().ActivationDepth(depth, 
				ActivationMode.Peek), committed);
		}

		public sealed override void Purge(object obj)
		{
			Purge(null, obj);
		}

		public override IQuery Query()
		{
			return Query((Transaction)null);
		}

		public sealed override IObjectSet Query(Type clazz)
		{
			return QueryByExample(clazz);
		}

		public sealed override IObjectSet Query(Predicate predicate)
		{
			return Query(predicate, (IQueryComparator)null);
		}

		public sealed override IObjectSet Query(Predicate predicate, IQueryComparator comparator
			)
		{
			return Query(null, predicate, comparator);
		}

		public sealed override void Refresh(object obj, int depth)
		{
			Refresh(null, obj, depth);
		}

		public sealed override void Rollback()
		{
			Rollback(null);
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public sealed override void Store(object obj)
		{
			Store(obj, Const4.Unspecified);
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public sealed override void Store(object obj, int depth)
		{
			Store(null, obj, depth == Const4.Unspecified ? (IUpdateDepth)UpdateDepthProvider(
				).Unspecified(NullModifiedObjectQuery.Instance) : (IUpdateDepth)UpdateDepthProvider
				().ForDepth(depth));
		}

		public sealed override IStoredClass StoredClass(object clazz)
		{
			return StoredClass(null, clazz);
		}

		public override IStoredClass[] StoredClasses()
		{
			return StoredClasses(null);
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="System.NotSupportedException"></exception>
		public abstract override void Backup(IStorage targetStorage, string path);

		public abstract override Db4oDatabase Identity();

		public override bool InCallback()
		{
			return EventRegistryImpl.InCallback(this);
		}
	}
}
