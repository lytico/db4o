/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Query;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Tests.Util;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class ArrayListUpdateTestCase : HandlerUpdateTestCaseBase
	{
		private static readonly object[] Data = new object[] { "one", "aAzZ|!Â§$%&/()=?ÃŸÃ¶Ã¤Ã¼Ã„Ã–ÃœYZ;:-_+*~#^Â°'@"
			, string.Empty, CreateNestedList(10), null };

		private static IList CreateNestedList(int depth)
		{
			IList list = new ArrayList();
			list.Add("nested1");
			list.Add("nested2");
			if (depth > 0)
			{
				list.Add(CreateNestedList(depth - 1));
			}
			return list;
		}

		protected override string TypeName()
		{
			return "ArrayList";
		}

		public class Item
		{
			public string _listClassName;

			public ArrayList _typed;

			public object _untyped;

			public ArrayList _emptyTyped;

			public object _emptyUntyped;

			public IList _interface;

			public IList _emptyInterface;
		}

		/// <summary>Todo: add as type to Item</summary>
		[System.Serializable]
		public class ArrayListExtensionWithField : ArrayList
		{
			public static readonly string StoredName = "outListsName";

			public string name;

			public override bool Equals(object obj)
			{
				if (!base.Equals(obj))
				{
					return false;
				}
				ArrayListUpdateTestCase.ArrayListExtensionWithField other = (ArrayListUpdateTestCase.ArrayListExtensionWithField
					)obj;
				if (name == null)
				{
					return other.name == null;
				}
				return name.Equals(other.name);
			}
		}

		/// <summary>Todo: add as type to Item</summary>
		[System.Serializable]
		public class ArrayListExtensionWithoutField : ArrayList
		{
		}

		protected override object[] CreateValues()
		{
			if (TestNotCompatibleToOldVersion())
			{
				return new ArrayListUpdateTestCase.Item[0];
			}
			ArrayListUpdateTestCase.Item[] values = new ArrayListUpdateTestCase.Item[3];
			values[0] = CreateItem(typeof(ArrayList));
			values[1] = CreateItem(typeof(ArrayListUpdateTestCase.ArrayListExtensionWithField
				));
			values[2] = CreateItem(typeof(ArrayListUpdateTestCase.ArrayListExtensionWithoutField
				));
			return values;
		}

		private ArrayListUpdateTestCase.Item CreateItem(Type clazz)
		{
			ArrayListUpdateTestCase.Item item = new ArrayListUpdateTestCase.Item();
			item._listClassName = clazz.FullName;
			CreateLists(item, clazz);
			return item;
		}

		private void CreateLists(ArrayListUpdateTestCase.Item item, Type clazz)
		{
			item._typed = (ArrayList)CreateFilledList(clazz);
			item._untyped = CreateFilledList(clazz);
			item._interface = CreateFilledList(clazz);
			item._emptyTyped = (ArrayList)CreateList(clazz);
			item._emptyUntyped = CreateList(clazz);
			item._emptyInterface = CreateList(clazz);
		}

		private IList CreateFilledList(Type clazz)
		{
			IList list = CreateList(clazz);
			FillList(list);
			if (list is ArrayListUpdateTestCase.ArrayListExtensionWithField)
			{
				ArrayListUpdateTestCase.ArrayListExtensionWithField typedList = (ArrayListUpdateTestCase.ArrayListExtensionWithField
					)list;
				typedList.name = ArrayListUpdateTestCase.ArrayListExtensionWithField.StoredName;
			}
			return list;
		}

		private IList CreateList(Type clazz)
		{
			IList list = null;
			try
			{
				list = (IList)System.Activator.CreateInstance(clazz);
			}
			catch (Exception e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
			return list;
		}

		private void FillList(object list)
		{
			for (int i = 0; i < Data.Length; i++)
			{
				((IList)list).Add(Data[i]);
			}
		}

		protected override object CreateArrays()
		{
			return null;
		}

		protected override void AssertValues(IExtObjectContainer objectContainer, object[]
			 values)
		{
			if (TestNotCompatibleToOldVersion())
			{
				return;
			}
			AssertItem(values[0], typeof(ArrayList));
			AssertItem(values[1], typeof(ArrayListUpdateTestCase.ArrayListExtensionWithField)
				);
			AssertItem(values[2], typeof(ArrayListUpdateTestCase.ArrayListExtensionWithoutField
				));
		}

		protected override void AssertQueries(IExtObjectContainer objectContainer)
		{
			if (TestNotCompatibleToOldVersion())
			{
				return;
			}
			AssertQueries(objectContainer, typeof(ArrayList));
			AssertQueries(objectContainer, typeof(ArrayListUpdateTestCase.ArrayListExtensionWithField
				));
			AssertQueries(objectContainer, typeof(ArrayListUpdateTestCase.ArrayListExtensionWithoutField
				));
		}

		private void AssertQueries(IExtObjectContainer objectContainer, Type clazz)
		{
			AssertQuery(objectContainer, clazz, "_typed");
		}

		//        assertQuery(objectContainer, clazz, "_untyped");
		//        assertQuery(objectContainer, clazz, "_interface");
		private void AssertQuery(IExtObjectContainer objectContainer, Type clazz, string 
			fieldName)
		{
			IQuery q = objectContainer.Query();
			q.Constrain(typeof(ArrayListUpdateTestCase.Item));
			q.Descend("_listClassName").Constrain(clazz.FullName);
			q.Descend(fieldName).Constrain("one");
			IObjectSet objectSet = q.Execute();
			Assert.AreEqual(1, objectSet.Count);
			ArrayListUpdateTestCase.Item item = (ArrayListUpdateTestCase.Item)objectSet.Next(
				);
			AssertItem(item, clazz);
		}

		private void AssertItem(object obj, Type clazz)
		{
			ArrayListUpdateTestCase.Item item = (ArrayListUpdateTestCase.Item)obj;
			AssertList(item._typed, clazz);
			AssertList(item._untyped, clazz);
			AssertList(item._interface, clazz);
			AssertEmptyList(item._emptyTyped);
			AssertEmptyList(item._emptyUntyped);
			AssertEmptyList(item._emptyInterface);
		}

		private void AssertEmptyList(object obj)
		{
			IList list = (IList)obj;
			Assert.AreEqual(0, list.Count);
		}

		private void AssertList(object obj, Type clazz)
		{
			IList list = (IList)obj;
			object[] array = new object[list.Count];
			int idx = 0;
			IEnumerator i = list.GetEnumerator();
			while (i.MoveNext())
			{
				array[idx++] = i.Current;
			}
			ArrayAssert.AreEqual(Data, array);
			Assert.IsInstanceOf(clazz, list);
			if (list is ArrayListUpdateTestCase.ArrayListExtensionWithField)
			{
				ArrayListUpdateTestCase.ArrayListExtensionWithField typedList = (ArrayListUpdateTestCase.ArrayListExtensionWithField
					)list;
				Assert.AreEqual(ArrayListUpdateTestCase.ArrayListExtensionWithField.StoredName, typedList
					.name);
			}
		}

		protected override void AssertArrays(IExtObjectContainer objectContainer, object 
			obj)
		{
		}

		// do nothing
		private bool TestNotCompatibleToOldVersion()
		{
			// This test fails for 3.0 and 4.0 versions, probably
			// because translators are incompatible.
			if (Db4oMajorVersion() < 5)
			{
				return true;
			}
			return Db4oHeaderVersion() == VersionServices.Header3040;
		}
	}
}
