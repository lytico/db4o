/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Config;

namespace Db4objects.Db4o.Tests.Common.Config
{
	public class ObjectTranslatorTestCase : AbstractDb4oTestCase
	{
		public class Thing
		{
			public string name;

			public Thing(string name)
			{
				this.name = name;
			}
		}

		public class ThingCounterTranslator : IObjectConstructor
		{
			private Hashtable4 _countCache = new Hashtable4();

			public virtual void OnActivate(IObjectContainer container, object applicationObject
				, object storedObject)
			{
			}

			public virtual object OnStore(IObjectContainer container, object applicationObject
				)
			{
				ObjectTranslatorTestCase.Thing t = (ObjectTranslatorTestCase.Thing)applicationObject;
				AddToCache(t);
				return t.name;
			}

			private void AddToCache(ObjectTranslatorTestCase.Thing t)
			{
				object o = (object)_countCache.Get(t.name);
				if (o == null)
				{
					o = 0;
				}
				_countCache.Put(t.name, ((int)o) + 1);
			}

			public virtual int GetCount(ObjectTranslatorTestCase.Thing t)
			{
				object o = (int)_countCache.Get(t.name);
				if (o == null)
				{
					return 0;
				}
				return ((int)o);
			}

			public virtual object OnInstantiate(IObjectContainer container, object storedObject
				)
			{
				string name = (string)storedObject;
				return new ObjectTranslatorTestCase.Thing(name);
			}

			public virtual Type StoredClass()
			{
				return typeof(string);
			}
		}

		private ObjectTranslatorTestCase.ThingCounterTranslator _trans;

		protected override void Configure(IConfiguration config)
		{
			config.ObjectClass(typeof(ObjectTranslatorTestCase.Thing)).Translate(_trans = new 
				ObjectTranslatorTestCase.ThingCounterTranslator());
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Db().Store(new ObjectTranslatorTestCase.Thing("jbe"));
		}

		public virtual void _testTranslationCount()
		{
			ObjectTranslatorTestCase.Thing t = (ObjectTranslatorTestCase.Thing)((ObjectTranslatorTestCase.Thing
				)RetrieveOnlyInstance(typeof(ObjectTranslatorTestCase.Thing)));
			Assert.IsNotNull(t);
			Assert.AreEqual("jbe", t.name);
			Assert.AreEqual(1, _trans.GetCount(t));
		}

		public static void Main(string[] args)
		{
			new ObjectTranslatorTestCase().RunSolo();
		}
	}
}
