/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Text;
using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class StringBufferHandlerUpdateTestCase : HandlerUpdateTestCaseBase
	{
		private static readonly StringBuilder[] data = new StringBuilder[] { new StringBuilder
			("one"), new StringBuilder("aAzZ\u05d0\u05d1\u4e2d"), new StringBuilder(string.Empty
			), null };

		public class Item
		{
			public StringBuilder _typed;

			public object _untyped;
			//$NON-NLS-1$
			//$NON-NLS-1$
			//$NON-NLS-1$
		}

		public class ItemArrays
		{
			public StringBuilder[] _typedArray;

			public object[] _untypedArray;

			public object _arrayInObject;
		}

		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(StringBufferHandlerUpdateTestCase)).Run();
		}

		protected override void AssertArrays(IExtObjectContainer objectContainer, object 
			obj)
		{
			StringBufferHandlerUpdateTestCase.ItemArrays item = (StringBufferHandlerUpdateTestCase.ItemArrays
				)obj;
			AssertTypedArray(item);
			AssertUntypedArray(item);
			AssertArrayInObject(item);
		}

		private void AssertArrayInObject(StringBufferHandlerUpdateTestCase.ItemArrays item
			)
		{
			AssertData((StringBuilder[])item._arrayInObject);
		}

		private void AssertUntypedArray(StringBufferHandlerUpdateTestCase.ItemArrays item
			)
		{
			for (int i = 0; i < data.Length; i++)
			{
				AssertAreEqual(data[i], (StringBuilder)item._untypedArray[i]);
			}
			Assert.IsNull(item._untypedArray[item._untypedArray.Length - 1]);
		}

		private void AssertTypedArray(StringBufferHandlerUpdateTestCase.ItemArrays item)
		{
			AssertData(item._typedArray);
		}

		private void AssertData(StringBuilder[] values)
		{
			for (int i = 0; i < data.Length; i++)
			{
				AssertAreEqual(data[i], values[i]);
			}
		}

		protected override void AssertValues(IExtObjectContainer objectContainer, object[]
			 values)
		{
			for (int i = 0; i < data.Length; i++)
			{
				StringBufferHandlerUpdateTestCase.Item item = (StringBufferHandlerUpdateTestCase.Item
					)values[i];
				AssertAreEqual(data[i], item._typed);
				AssertAreEqual(data[i], (StringBuilder)item._untyped);
			}
			StringBufferHandlerUpdateTestCase.Item nullItem = (StringBufferHandlerUpdateTestCase.Item
				)values[values.Length - 1];
			Assert.IsNull(nullItem._typed);
			Assert.IsNull(nullItem._untyped);
		}

		private void AssertAreEqual(StringBuilder expected, StringBuilder actual)
		{
			string expectedString = (expected == null) ? null : expected.ToString();
			string actualString = (actual == null) ? null : actual.ToString();
			Assert.AreEqual(expectedString, actualString);
		}

		protected override object CreateArrays()
		{
			StringBufferHandlerUpdateTestCase.ItemArrays item = new StringBufferHandlerUpdateTestCase.ItemArrays
				();
			CreateTypedArray(item);
			CreateUntypedArray(item);
			CreateArrayInObject(item);
			return item;
		}

		private void CreateArrayInObject(StringBufferHandlerUpdateTestCase.ItemArrays item
			)
		{
			StringBuilder[] stringBufferArray = new StringBuilder[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				stringBufferArray[i] = data[i];
			}
			item._arrayInObject = stringBufferArray;
		}

		private void CreateUntypedArray(StringBufferHandlerUpdateTestCase.ItemArrays item
			)
		{
			item._untypedArray = new StringBuilder[data.Length + 1];
			for (int i = 0; i < data.Length; i++)
			{
				item._untypedArray[i] = data[i];
			}
		}

		private void CreateTypedArray(StringBufferHandlerUpdateTestCase.ItemArrays item)
		{
			item._typedArray = new StringBuilder[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				item._typedArray[i] = data[i];
			}
		}

		protected override object[] CreateValues()
		{
			StringBufferHandlerUpdateTestCase.Item[] items = new StringBufferHandlerUpdateTestCase.Item
				[data.Length + 1];
			for (int i = 0; i < data.Length; i++)
			{
				StringBufferHandlerUpdateTestCase.Item item = new StringBufferHandlerUpdateTestCase.Item
					();
				item._typed = data[i];
				item._untyped = data[i];
				items[i] = item;
			}
			items[items.Length - 1] = new StringBufferHandlerUpdateTestCase.Item();
			return items;
		}

		protected override string TypeName()
		{
			return "StringBuffer";
		}
		//$NON-NLS-1$
	}
}
