/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Tests.Util;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class BooleanHandlerUpdateTestCase : HandlerUpdateTestCaseBase
	{
		public class Item
		{
			public bool _typedPrimitive;

			public bool _typedWrapper;

			public object _untyped;
		}

		public class ItemArrays
		{
			public bool[] _typedPrimitiveArray;

			public bool[] _typedWrapperArray;

			public object[] _untypedObjectArray;

			public object _primitiveArrayInObject;

			public object _wrapperArrayInObject;
		}

		private static readonly bool[] data = new bool[] { true, false };

		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(BooleanHandlerUpdateTestCase)).Run();
		}

		protected override void AssertArrays(IExtObjectContainer objectContainer, object 
			obj)
		{
			BooleanHandlerUpdateTestCase.ItemArrays itemArrays = (BooleanHandlerUpdateTestCase.ItemArrays
				)obj;
			AssertPrimitiveArray(itemArrays._typedPrimitiveArray);
			if (Db4oHeaderVersion() == VersionServices.Header3040)
			{
				// Bug in the oldest format: It accidentally boolean[] arrays to
				// Boolean[] arrays.
				AssertWrapperArray((bool[])itemArrays._primitiveArrayInObject);
			}
			else
			{
				AssertPrimitiveArray((bool[])itemArrays._primitiveArrayInObject);
			}
			AssertWrapperArray(itemArrays._typedWrapperArray);
			AssertWrapperArray((bool[])itemArrays._wrapperArrayInObject);
		}

		private void AssertPrimitiveArray(bool[] primitiveArray)
		{
			for (int i = 0; i < data.Length; i++)
			{
				AssertAreEqual(data[i], primitiveArray[i]);
			}
		}

		private void AssertWrapperArray(bool[] wrapperArray)
		{
			for (int i = 0; i < data.Length; i++)
			{
				AssertAreEqual(data[i], wrapperArray[i]);
			}
		}

		// FIXME: Arrays should also get a null Bitmap to fix.
		// Assert.isNull(wrapperArray[wrapperArray.length - 1]);
		protected override void AssertValues(IExtObjectContainer objectContainer, object[]
			 values)
		{
			for (int i = 0; i < data.Length; i++)
			{
				BooleanHandlerUpdateTestCase.Item item = (BooleanHandlerUpdateTestCase.Item)values
					[i];
				AssertAreEqual(data[i], item._typedPrimitive);
				AssertAreEqual(data[i], item._typedWrapper);
				AssertAreEqual(data[i], item._untyped);
			}
			BooleanHandlerUpdateTestCase.Item nullItem = (BooleanHandlerUpdateTestCase.Item)values
				[values.Length - 1];
			AssertAreEqual(false, nullItem._typedPrimitive);
			Assert.IsNull(nullItem._untyped);
		}

		private void AssertAreEqual(bool expected, bool actual)
		{
			Assert.AreEqual(expected, actual);
		}

		private void AssertAreEqual(object expected, object actual)
		{
			Assert.AreEqual(expected, actual);
		}

		protected override object CreateArrays()
		{
			BooleanHandlerUpdateTestCase.ItemArrays itemArrays = new BooleanHandlerUpdateTestCase.ItemArrays
				();
			itemArrays._typedPrimitiveArray = new bool[data.Length];
			System.Array.Copy(data, 0, itemArrays._typedPrimitiveArray, 0, data.Length);
			bool[] dataWrapper = new bool[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				dataWrapper[i] = data[i];
			}
			itemArrays._typedWrapperArray = new bool[data.Length + 1];
			System.Array.Copy(dataWrapper, 0, itemArrays._typedWrapperArray, 0, dataWrapper.Length
				);
			bool[] primitiveArray = new bool[data.Length];
			System.Array.Copy(data, 0, primitiveArray, 0, data.Length);
			itemArrays._primitiveArrayInObject = primitiveArray;
			bool[] wrapperArray = new bool[data.Length + 1];
			System.Array.Copy(dataWrapper, 0, wrapperArray, 0, dataWrapper.Length);
			itemArrays._wrapperArrayInObject = wrapperArray;
			return itemArrays;
		}

		protected override object[] CreateValues()
		{
			BooleanHandlerUpdateTestCase.Item[] values = new BooleanHandlerUpdateTestCase.Item
				[data.Length + 1];
			for (int i = 0; i < data.Length; i++)
			{
				BooleanHandlerUpdateTestCase.Item item = new BooleanHandlerUpdateTestCase.Item();
				item._typedPrimitive = data[i];
				item._typedWrapper = data[i];
				item._untyped = data[i];
				values[i] = item;
			}
			values[values.Length - 1] = new BooleanHandlerUpdateTestCase.Item();
			return values;
		}

		protected override string TypeName()
		{
			return "boolean";
		}
	}
}
