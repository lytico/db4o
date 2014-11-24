/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Tests.Common.Api;
using Db4objects.Db4o.Tests.Common.Ext;

namespace Db4objects.Db4o.Tests.Common.Ext
{
	public class UnavailableClassesWithTranslatorTestCase : TestWithTempFile, IOptOutNetworkingCS
	{
		public class HolderForClassWithTranslator
		{
			public override bool Equals(object obj)
			{
				if (obj == null)
				{
					return false;
				}
				if (GetType() != obj.GetType())
				{
					return false;
				}
				UnavailableClassesWithTranslatorTestCase.HolderForClassWithTranslator other = (UnavailableClassesWithTranslatorTestCase.HolderForClassWithTranslator
					)obj;
				return Check.ObjectsAreEqual(_fieldWithTranslator, other._fieldWithTranslator);
			}

			public HolderForClassWithTranslator(UnavailableClassesWithTranslatorTestCase.TranslatedType
				 value)
			{
				_fieldWithTranslator = value;
			}

			public UnavailableClassesWithTranslatorTestCase.TranslatedType _fieldWithTranslator;
		}

		public class TranslatedType
		{
			public override bool Equals(object obj)
			{
				if (null == obj)
				{
					return false;
				}
				return GetType() == obj.GetType();
			}
		}

		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(UnavailableClassesWithTranslatorTestCase)).Run();
		}

		public virtual void Test()
		{
			Store(TempFile(), new UnavailableClassesWithTranslatorTestCase.HolderForClassWithTranslator
				(new UnavailableClassesWithTranslatorTestCase.TranslatedType()));
			AssertStoredClasses(TempFile());
		}

		private void AssertStoredClasses(string databaseFileName)
		{
			IObjectContainer db = Db4oEmbedded.OpenFile(ConfigExcludingStack(), databaseFileName
				);
			try
			{
				Assert.IsGreater(2, db.Ext().StoredClasses().Length);
			}
			finally
			{
				db.Close();
			}
		}

		private void Store(string databaseFileName, object obj)
		{
			IObjectContainer db = Db4oEmbedded.OpenFile(NewConfiguration(), databaseFileName);
			try
			{
				db.Store(obj);
				db.Ext().Purge(obj);
				Assert.AreEqual(obj, db.Query(obj.GetType()).Next());
			}
			finally
			{
				db.Close();
			}
		}

		private IEmbeddedConfiguration NewConfiguration()
		{
			IEmbeddedConfiguration config = Db4oEmbedded.NewConfiguration();
			config.Common.ObjectClass(typeof(UnavailableClassesWithTranslatorTestCase.TranslatedType
				).FullName).Translate(new UnavailableClassesWithTranslatorTestCase.Translator());
			return config;
		}

		private IEmbeddedConfiguration ConfigExcludingStack()
		{
			IEmbeddedConfiguration config = NewConfiguration();
			config.Common.ReflectWith(new ExcludingReflector(new Type[] { typeof(UnavailableClassesWithTranslatorTestCase.TranslatedType
				) }));
			return config;
		}

		private sealed class Translator : IObjectTranslator
		{
			public void OnActivate(IObjectContainer container, object applicationObject, object
				 storedObject)
			{
			}

			public object OnStore(IObjectContainer container, object applicationObject)
			{
				return 42;
			}

			public Type StoredClass()
			{
				return typeof(int);
			}
		}
	}
}
