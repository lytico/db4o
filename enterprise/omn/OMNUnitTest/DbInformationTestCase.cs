/* Copyright (C) 2004 - 2009  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using System.Collections.Generic;
using NUnit.Framework;
using OManager.DataLayer.DBInfo;
using Sharpen.Lang;

namespace OMNUnitTest
{
	[TestFixture]
	public class DbInformationTestCase : OMNTestCaseBase
	{
		[Test]
		public void TestStoredClasses()
		{
			Hashtable classes =DbInformation.StoredClasses();
			CollectionAssert.AreEqual(ClassesCollection(typeof(Item), typeof(Element)), classes);
		}

		[Test]
		public void TestStoredClassesByAssembly()
		{
			Hashtable classesByAssembly = DbInformation.StoredClassesByAssembly();

			foreach (DictionaryEntry entry in ClassesCollectionByAssembly(typeof(Item), typeof(Element)))
			{
				Assert.IsTrue(classesByAssembly.ContainsKey(entry.Key));
				CollectionAssert.AreEqual((IEnumerable) entry.Value, (IEnumerable) classesByAssembly[entry.Key]);
			}
		}

		private static Hashtable ClassesCollectionByAssembly(params Type[] types)
		{
			Hashtable classesByAssembly = new Hashtable();
			foreach (var type in types)
			{
				string assemblyName = type.Assembly.GetName().Name;
				if (!classesByAssembly.ContainsKey(assemblyName))
				{
					classesByAssembly[assemblyName] = new List<string>();
				}
				((List<string>)classesByAssembly[assemblyName]).Add(TypeReference.FromType(type).GetUnversionedName());
			}

			return classesByAssembly;
		}

		private static IEnumerable ClassesCollection(params Type[] types)
		{
			Hashtable coll = new Hashtable();

			foreach (var type in types)
			{
				TypeReference reference = TypeReference.FromType(type);
				coll.Add(reference.GetUnversionedName(), type.FullName);
			}

			return coll;
		}

		protected override void Store()
		{
			foreach (object item in Items())
			{
				Store(item);
			}
		}

		private static IEnumerable Items()
		{
			return new object[]
				{
					new Item("foo"),
					new Element("bar"),
					new Item("baz"),
					new ArrayList(new[] {1, 2, 3})
				};
		}
	}

	public class Element : Item
	{
		public Element(string name) : base(name)
		{
		}
	}

	public class Item
	{
		private readonly string _name;

		public Item(string name)
		{
			_name = name;
		}

		public string Name
		{
			get { return _name; }
		}
	}
}
