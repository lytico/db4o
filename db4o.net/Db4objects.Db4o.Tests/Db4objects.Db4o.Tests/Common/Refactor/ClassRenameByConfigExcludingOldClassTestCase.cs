/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Refactor;

namespace Db4objects.Db4o.Tests.Common.Refactor
{
	public class ClassRenameByConfigExcludingOldClassTestCase : ITestLifeCycle
	{
		private static readonly string DbPath = string.Empty;

		private const int NumItems = 10;

		public class OldItem
		{
			public int _id;

			public OldItem(int id)
			{
				_id = id;
			}
		}

		public class NewItem
		{
			public int _id;
		}

		private IStorage storage;

		public virtual void Test()
		{
			IEmbeddedConfiguration config = Config();
			config.Common.ObjectClass(typeof(ClassRenameByConfigExcludingOldClassTestCase.OldItem
				)).Rename(typeof(ClassRenameByConfigExcludingOldClassTestCase.NewItem).FullName);
			config.Common.ReflectWith(new ExcludingReflector(new Type[] { typeof(ClassRenameByConfigExcludingOldClassTestCase.OldItem
				) }));
			IEmbeddedObjectContainer db = Db4oEmbedded.OpenFile(config, DbPath);
			AssertExtentSize(0, typeof(ClassRenameByConfigExcludingOldClassTestCase.OldItem), 
				db);
			AssertExtentSize(NumItems, typeof(ClassRenameByConfigExcludingOldClassTestCase.NewItem
				), db);
			db.Close();
		}

		private void AssertExtentSize(int expectedCount, Type extent, IEmbeddedObjectContainer
			 db)
		{
			IQuery query = db.Query();
			query.Constrain(db.Ext().Reflector().ForName(extent.FullName));
			IObjectSet result = query.Execute();
			Assert.AreEqual(expectedCount, result.Count);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			storage = new MemoryStorage();
			IEmbeddedObjectContainer db = Db4oEmbedded.OpenFile(Config(), DbPath);
			for (int i = 0; i < NumItems; i++)
			{
				db.Store(new ClassRenameByConfigExcludingOldClassTestCase.OldItem(i));
			}
			db.Close();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
		}

		private IEmbeddedConfiguration Config()
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.File.Storage = storage;
			return config;
		}
	}
}
