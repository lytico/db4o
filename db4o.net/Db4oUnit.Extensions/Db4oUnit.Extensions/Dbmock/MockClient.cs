/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4oUnit.Extensions.Dbmock
{
	public partial class MockClient : IExtClient
	{
		public virtual bool IsAlive()
		{
			throw new NotImplementedException();
		}

		public virtual void SwitchToFile(string fileName)
		{
			throw new NotImplementedException();
		}

		public virtual void SwitchToMainFile()
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual void Activate(object obj)
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="System.NotSupportedException"></exception>
		public virtual void Backup(string path)
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="System.NotSupportedException"></exception>
		public virtual void Backup(IStorage targetStorage, string path)
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.InvalidIDException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual void Bind(object obj, long id)
		{
			throw new NotImplementedException();
		}

		public virtual IConfiguration Configure()
		{
			throw new NotImplementedException();
		}

		public virtual void Deactivate(object obj)
		{
			throw new NotImplementedException();
		}

		public virtual object Descend(object obj, string[] path)
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.InvalidIDException"></exception>
		public virtual object GetByID(long Id)
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual object GetByUUID(Db4oUUID uuid)
		{
			throw new NotImplementedException();
		}

		public virtual long GetID(object obj)
		{
			throw new NotImplementedException();
		}

		public virtual IObjectInfo GetObjectInfo(object obj)
		{
			throw new NotImplementedException();
		}

		public virtual Db4oDatabase Identity()
		{
			throw new NotImplementedException();
		}

		public virtual bool IsActive(object obj)
		{
			throw new NotImplementedException();
		}

		public virtual bool IsCached(long Id)
		{
			throw new NotImplementedException();
		}

		public virtual bool IsClosed()
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual bool IsStored(object obj)
		{
			throw new NotImplementedException();
		}

		public virtual IReflectClass[] KnownClasses()
		{
			throw new NotImplementedException();
		}

		public virtual object Lock()
		{
			throw new NotImplementedException();
		}

		public virtual object PeekPersisted(object @object, int depth, bool committed)
		{
			throw new NotImplementedException();
		}

		public virtual void Purge()
		{
			throw new NotImplementedException();
		}

		public virtual void Purge(object obj)
		{
			throw new NotImplementedException();
		}

		public virtual GenericReflector Reflector()
		{
			throw new NotImplementedException();
		}

		public virtual void Refresh(object obj, int depth)
		{
			throw new NotImplementedException();
		}

		public virtual void ReleaseSemaphore(string name)
		{
			throw new NotImplementedException();
		}

		public virtual bool SetSemaphore(string name, int waitForAvailability)
		{
			throw new NotImplementedException();
		}

		public virtual void Store(object obj, int depth)
		{
			throw new NotImplementedException();
		}

		public virtual IStoredClass StoredClass(object clazz)
		{
			throw new NotImplementedException();
		}

		public virtual IStoredClass[] StoredClasses()
		{
			throw new NotImplementedException();
		}

		public virtual ISystemInfo SystemInfo()
		{
			throw new NotImplementedException();
		}

		public virtual long Version()
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual void Activate(object obj, int depth)
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		public virtual bool Close()
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public virtual void Commit()
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual void Deactivate(object obj, int depth)
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public virtual void Delete(object obj)
		{
			throw new NotImplementedException();
		}

		public virtual IExtObjectContainer Ext()
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual IObjectSet Get(object template)
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual IQuery Query()
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual IObjectSet Query(Type clazz)
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual IObjectSet Query(Predicate predicate)
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual IObjectSet Query(Predicate predicate, IQueryComparator comparator)
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual IObjectSet Query(Predicate predicate, IComparer comparator)
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		public virtual IObjectSet QueryByExample(object template)
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public virtual void Rollback()
		{
			throw new NotImplementedException();
		}

		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseReadOnlyException"></exception>
		public virtual void Store(object obj)
		{
			throw new NotImplementedException();
		}

		public virtual IObjectContainer OpenSession()
		{
			throw new NotImplementedException();
		}

		public virtual IQLin From(Type clazz)
		{
			throw new NotImplementedException();
		}
	}
}
