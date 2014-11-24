/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class NestedArrayUpdateTestCase : HandlerUpdateTestCaseBase
	{
		private static readonly object[] primitiveArrayData = new object[] { new int[] { 
			1, 2, 3 }, new int[] { 4, 5 }, new int[] {  } };

		private static readonly object[] stringArrayData = new object[] { new string[] { 
			"one", null, string.Empty }, new string[] { "two" }, new string[] { string.Empty
			 }, new string[] {  } };

		private static readonly object[] nestedArrayData = new object[] { new object[] { 
			primitiveArrayData, stringArrayData }, new object[] { primitiveArrayData, stringArrayData
			 } };

		private static readonly object[] nestedNestedArrayData = new object[] { new object
			[] { nestedArrayData, nestedArrayData }, new object[] { nestedArrayData, nestedArrayData
			 } };

		public class ItemArrays
		{
			public object[] _primitiveArray;

			public object _primitiveArrayInObject;

			public object[] _stringArray;

			public object _stringArrayInObject;

			public object[] _nestedArray;

			public object _nestedArrayInObject;

			public object[] _nestedNestedArray;

			public object _nestedNestedArrayInObject;
		}

		protected override object CreateArrays()
		{
			NestedArrayUpdateTestCase.ItemArrays item = new NestedArrayUpdateTestCase.ItemArrays
				();
			item._primitiveArray = primitiveArrayData;
			item._primitiveArrayInObject = primitiveArrayData;
			item._stringArray = stringArrayData;
			item._stringArrayInObject = stringArrayData;
			item._nestedArray = nestedArrayData;
			item._nestedArrayInObject = nestedArrayData;
			item._nestedNestedArray = nestedNestedArrayData;
			item._nestedNestedArrayInObject = nestedNestedArrayData;
			return item;
		}

		protected override void AssertArrays(IExtObjectContainer objectContainer, object 
			obj)
		{
			NestedArrayUpdateTestCase.ItemArrays item = (NestedArrayUpdateTestCase.ItemArrays
				)obj;
			AssertPrimitiveArray(item._primitiveArray);
			AssertPrimitiveArray(item._primitiveArrayInObject);
			AssertStringArray(item._stringArray);
			AssertStringArray(item._stringArrayInObject);
			AssertNestedArray(nestedArrayData, item._nestedArray);
			AssertNestedArray(nestedArrayData, item._nestedArrayInObject);
			AssertNestedArray(nestedNestedArrayData, item._nestedNestedArray);
			AssertNestedArray(nestedNestedArrayData, item._nestedNestedArrayInObject);
		}

		private void AssertNestedArray(object expected, object actual)
		{
			object[] expectedArray = (object[])expected;
			object[] actualArray = (object[])actual;
			Assert.AreEqual(expectedArray.Length, actualArray.Length);
			for (int i = 0; i < expectedArray.Length; i++)
			{
				object[] expectedSubArray = (object[])expectedArray[i];
				object actualSubArray = actualArray[i];
				object template = expectedSubArray[0];
				if (template is int[])
				{
					AssertPrimitiveArray(actualSubArray);
				}
				else
				{
					if (template is string[])
					{
						AssertStringArray(actualSubArray);
					}
					else
					{
						AssertNestedArray(expectedSubArray, actualSubArray);
					}
				}
			}
		}

		private void AssertStringArray(object array)
		{
			object[] stringArray = (object[])array;
			for (int i = 0; i < stringArray.Length; i++)
			{
				string[] actual = (string[])stringArray[i];
				string[] expected = (string[])stringArrayData[i];
				Assert.AreEqual(actual.Length, expected.Length);
				for (int j = 0; j < expected.Length; j++)
				{
					Assert.AreEqual(expected[j], actual[j]);
				}
			}
		}

		private void AssertPrimitiveArray(object array)
		{
			object[] primitiveArray = (object[])array;
			for (int i = 0; i < primitiveArray.Length; i++)
			{
				int[] expected = (int[])primitiveArrayData[i];
				int[] actual = CastToIntArray(primitiveArray[i]);
				Assert.AreEqual(actual.Length, expected.Length);
				for (int j = 0; j < expected.Length; j++)
				{
					Assert.AreEqual(expected[j], actual[j]);
				}
			}
		}

		protected override object[] CreateValues()
		{
			// not used
			return null;
		}

		protected override void AssertValues(IExtObjectContainer objectContainer, object[]
			 values)
		{
		}

		// not used
		protected override string TypeName()
		{
			return "nested_array";
		}
	}
}
