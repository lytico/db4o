/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Tests.Util;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class LongHandlerUpdateTestCase : HandlerUpdateTestCaseBase
	{
		private readonly long[] data;

		public LongHandlerUpdateTestCase()
		{
			data = new long[] { long.MinValue, long.MinValue + 1, -5, -1, 0, 1, 5, long.MaxValue
				 - 1, UsesNullMarkerValue() ? 0 : long.MaxValue };
		}

		public class Item
		{
			public long _typedPrimitive;

			public long _typedWrapper;

			public object _untyped;
		}

		public class ItemArrays
		{
			public long[] _typedPrimitiveArray;

			public long[] _typedWrapperArray;

			public object[] _untypedObjectArray;

			public object _primitiveArrayInObject;

			public object _wrapperArrayInObject;
		}

		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(LongHandlerUpdateTestCase)).Run();
		}

		protected override void AssertArrays(IExtObjectContainer objectContainer, object 
			obj)
		{
			LongHandlerUpdateTestCase.ItemArrays itemArrays = (LongHandlerUpdateTestCase.ItemArrays
				)obj;
			AssertPrimitiveArray(itemArrays._typedPrimitiveArray);
			if (Db4oHeaderVersion() == VersionServices.Header3040)
			{
				// Bug in the oldest format: It accidentally long[] arrays to Long[] arrays.
				AssertWrapperArray((long[])itemArrays._primitiveArrayInObject);
			}
			else
			{
				AssertPrimitiveArray((long[])itemArrays._primitiveArrayInObject);
			}
			AssertWrapperArray(itemArrays._typedWrapperArray);
			AssertWrapperArray((long[])itemArrays._wrapperArrayInObject);
		}

		private void AssertPrimitiveArray(long[] primitiveArray)
		{
			for (int i = 0; i < data.Length; i++)
			{
				AssertAreEqual(data[i], primitiveArray[i]);
			}
		}

		private void AssertWrapperArray(long[] wrapperArray)
		{
			for (int i = 0; i < data.Length; i++)
			{
				AssertAreEqual(data[i], wrapperArray[i]);
			}
		}

		//FIXME: Arrays should also get a null Bitmap to fix.
		//Assert.isNull(wrapperArray[wrapperArray.length - 1]);
		protected override void AssertValues(IExtObjectContainer objectContainer, object[]
			 values)
		{
			for (int i = 0; i < data.Length; i++)
			{
				LongHandlerUpdateTestCase.Item item = (LongHandlerUpdateTestCase.Item)values[i];
				AssertAreEqual(data[i], item._typedPrimitive);
				AssertAreEqual(data[i], item._typedWrapper);
				AssertAreEqual(data[i], item._untyped);
			}
			LongHandlerUpdateTestCase.Item nullItem = (LongHandlerUpdateTestCase.Item)values[
				values.Length - 1];
			AssertAreEqual(0, nullItem._typedPrimitive);
			Assert.IsNull(nullItem._untyped);
		}

		private void AssertAreEqual(long expected, long actual)
		{
			if (expected == long.MaxValue && Db4oHandlerVersion() == 0)
			{
				// Bug in the oldest format: It treats Long.MAX_VALUE as null.
				expected = 0;
			}
			Assert.AreEqual(expected, actual);
		}

		private void AssertAreEqual(object expected, object actual)
		{
			if (long.MaxValue.Equals(expected) && Db4oHandlerVersion() == 0)
			{
				// Bug in the oldest format: It treats Long.MAX_VALUE as null.
				expected = null;
			}
			Assert.AreEqual(expected, actual);
		}

		protected override object CreateArrays()
		{
			LongHandlerUpdateTestCase.ItemArrays itemArrays = new LongHandlerUpdateTestCase.ItemArrays
				();
			itemArrays._typedPrimitiveArray = new long[data.Length];
			System.Array.Copy(data, 0, itemArrays._typedPrimitiveArray, 0, data.Length);
			long[] dataWrapper = new long[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				dataWrapper[i] = data[i];
			}
			itemArrays._typedWrapperArray = new long[data.Length + 1];
			System.Array.Copy(dataWrapper, 0, itemArrays._typedWrapperArray, 0, dataWrapper.Length
				);
			long[] primitiveArray = new long[data.Length];
			System.Array.Copy(data, 0, primitiveArray, 0, data.Length);
			itemArrays._primitiveArrayInObject = primitiveArray;
			long[] wrapperArray = new long[data.Length + 1];
			System.Array.Copy(dataWrapper, 0, wrapperArray, 0, dataWrapper.Length);
			itemArrays._wrapperArrayInObject = wrapperArray;
			return itemArrays;
		}

		protected override object[] CreateValues()
		{
			LongHandlerUpdateTestCase.Item[] values = new LongHandlerUpdateTestCase.Item[data
				.Length + 1];
			for (int i = 0; i < data.Length; i++)
			{
				LongHandlerUpdateTestCase.Item item = new LongHandlerUpdateTestCase.Item();
				item._typedPrimitive = data[i];
				item._typedWrapper = data[i];
				item._untyped = data[i];
				values[i] = item;
			}
			values[values.Length - 1] = new LongHandlerUpdateTestCase.Item();
			return values;
		}

		protected override string TypeName()
		{
			return "long";
		}
	}
}
