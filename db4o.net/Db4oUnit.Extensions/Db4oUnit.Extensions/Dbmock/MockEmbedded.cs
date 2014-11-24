/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Qlin;
using Db4objects.Db4o.Query;

namespace Db4oUnit.Extensions.Dbmock
{
	public partial class MockEmbedded : IEmbeddedObjectContainer
	{
		/// <exception cref="Db4objects.Db4o.Ext.Db4oIOException"></exception>
		/// <exception cref="Db4objects.Db4o.Ext.DatabaseClosedException"></exception>
		/// <exception cref="System.NotSupportedException"></exception>
		public virtual void Backup(string path)
		{
			throw new NotImplementedException();
		}

		public virtual IObjectContainer OpenSession()
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

		public virtual IQLin From(Type clazz)
		{
			throw new NotImplementedException();
		}
	}
}
