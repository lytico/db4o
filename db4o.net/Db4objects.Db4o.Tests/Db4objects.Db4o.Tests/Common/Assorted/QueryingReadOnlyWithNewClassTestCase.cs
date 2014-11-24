/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class QueryingReadOnlyWithNewClassTestCase : TestWithTempFile, IOptOutSilverlight
	{
		public virtual void TestWithoutReadOnly()
		{
			CreateData();
			QueryDb(Db4oEmbedded.NewConfiguration(), typeof(QueryingReadOnlyWithNewClassTestCase.B
				), 0);
		}

		public virtual void TestWithReadOnly()
		{
			CreateData();
			QueryDb(ReadOnlyConfiguration(), typeof(QueryingReadOnlyWithNewClassTestCase.B), 
				0);
		}

		private void CreateData()
		{
			IEmbeddedObjectContainer database = Db4oEmbedded.OpenFile(TempFile());
			database.Store(new QueryingReadOnlyWithNewClassTestCase.A("Item1"));
			database.Commit();
			database.Close();
		}

		public virtual void QueryDb(IEmbeddedConfiguration config, Type clazz, int count)
		{
			IEmbeddedObjectContainer database = Db4oEmbedded.OpenFile(config, TempFile());
			try
			{
				IList list = database.Query(clazz);
				Assert.AreEqual(count, list.Count);
			}
			finally
			{
				database.Close();
			}
		}

		private IEmbeddedConfiguration ReadOnlyConfiguration()
		{
			IEmbeddedConfiguration configuration = Db4oEmbedded.NewConfiguration();
			configuration.File.ReadOnly = true;
			return configuration;
		}

		public class A
		{
			private string _name;

			public A(string name)
			{
				_name = name;
			}

			public virtual string GetName()
			{
				return _name;
			}

			public virtual void SetName(string name)
			{
				_name = name;
			}

			public override string ToString()
			{
				return "Name: " + _name + " Type: " + GetType().FullName;
			}
		}

		public class B
		{
			private string _name;

			public virtual string GetName()
			{
				return _name;
			}

			public virtual void SetName(string name)
			{
				_name = name;
			}

			public override string ToString()
			{
				return "Name: " + _name + " Type: " + GetType().FullName;
			}
		}
	}
}
