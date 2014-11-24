/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.IO;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Assorted;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class KnownClassesIndexTestCase : ITestLifeCycle
	{
		private static readonly string DbPath = "inmem";

		private IStorage _storage = new MemoryStorage();

		public class WithIndex
		{
			public int _id;

			public WithIndex(int id)
			{
				_id = id;
			}
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void SetUp()
		{
			IEmbeddedObjectContainer db = Db4oEmbedded.OpenFile(Config(), DbPath);
			db.Store(new KnownClassesIndexTestCase.WithIndex(42));
			db.Close();
		}

		private IEmbeddedConfiguration Config()
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.Common.ObjectClass(typeof(KnownClassesIndexTestCase.WithIndex)).ObjectField
				("_id").Indexed(true);
			config.File.Storage = _storage;
			return config;
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TearDown()
		{
		}

		public virtual void TestIndexInfoAvailableAfterInfoGathering()
		{
			IEmbeddedConfiguration config = Config();
			config.Common.ReflectWith(new ExcludingReflector(new Type[] { typeof(KnownClassesIndexTestCase.WithIndex
				) }));
			IEmbeddedObjectContainer db = Db4oEmbedded.OpenFile(config, DbPath);
			try
			{
				ScanThroughKnownClassesInfo(db);
				AssertHasIndexInfo(db);
			}
			finally
			{
				db.Close();
			}
		}

		private void ScanThroughKnownClassesInfo(IObjectContainer db)
		{
			IReflectClass[] clazzArray = db.Ext().KnownClasses();
			for (int clazzIndex = 0; clazzIndex < clazzArray.Length; ++clazzIndex)
			{
				IReflectClass clazz = clazzArray[clazzIndex];
				IReflectField[] fieldArray = clazz.GetDeclaredFields();
				for (int fieldIndex = 0; fieldIndex < fieldArray.Length; ++fieldIndex)
				{
					IReflectField field = fieldArray[fieldIndex];
					field.GetName();
					field.GetFieldType();
				}
			}
		}

		private void AssertHasIndexInfo(IObjectContainer db)
		{
			IStoredClass[] scArray = db.Ext().StoredClasses();
			for (int scIndex = 0; scIndex < scArray.Length; ++scIndex)
			{
				IStoredClass sc = scArray[scIndex];
				if (!sc.GetName().Equals(typeof(KnownClassesIndexTestCase.WithIndex).FullName))
				{
					continue;
				}
				IStoredField[] sfArray = sc.GetStoredFields();
				for (int sfIndex = 0; sfIndex < sfArray.Length; ++sfIndex)
				{
					IStoredField sf = sfArray[sfIndex];
					if (sf.HasIndex())
					{
						return;
					}
				}
				Assert.Fail("no index found");
			}
		}
	}
}
