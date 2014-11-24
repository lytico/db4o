/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Tests.Util;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class FloatHandlerUpdateTestCase : HandlerUpdateTestCaseBase
	{
		private readonly float[] data;

		public FloatHandlerUpdateTestCase()
		{
			data = new float[] { float.NegativeInfinity, float.MinValue, float.MinValue + 1, 
				-5, -1, 0, 1, 5, float.MaxValue - 1, float.MaxValue, float.PositiveInfinity, UsesNullMarkerValue
				() ? 0 : float.NaN };
		}

		public class Item
		{
			public float _typedPrimitive;

			public float _typedWrapper;

			public object _untyped;
		}

		public class ItemArrays
		{
			public float[] _typedPrimitiveArray;

			public float[] _typedWrapperArray;

			public object[] _untypedObjectArray;

			public object _primitiveArrayInObject;

			public object _wrapperArrayInObject;
		}

		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(FloatHandlerUpdateTestCase)).Run();
		}

		protected override void AssertArrays(IExtObjectContainer objectContainer, object 
			obj)
		{
			FloatHandlerUpdateTestCase.ItemArrays itemArrays = (FloatHandlerUpdateTestCase.ItemArrays
				)obj;
			AssertPrimitiveArray(itemArrays._typedPrimitiveArray);
			if (Db4oHeaderVersion() == VersionServices.Header3040)
			{
				// Bug in the oldest format: It accidentally float[] arrays to Float[] arrays.
				AssertWrapperArray((float[])itemArrays._primitiveArrayInObject);
			}
			else
			{
				AssertPrimitiveArray((float[])itemArrays._primitiveArrayInObject);
			}
			AssertWrapperArray(itemArrays._typedWrapperArray);
			AssertWrapperArray((float[])itemArrays._wrapperArrayInObject);
		}

		private void AssertPrimitiveArray(float[] primitiveArray)
		{
			for (int i = 0; i < data.Length; i++)
			{
				AssertAreEqual(data[i], primitiveArray[i]);
			}
		}

		private void AssertWrapperArray(float[] wrapperArray)
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
				FloatHandlerUpdateTestCase.Item item = (FloatHandlerUpdateTestCase.Item)values[i];
				AssertAreEqual(data[i], item._typedPrimitive);
				AssertAreEqual(data[i], item._typedWrapper);
				AssertAreEqual(data[i], item._untyped);
			}
			FloatHandlerUpdateTestCase.Item nullItem = (FloatHandlerUpdateTestCase.Item)values
				[values.Length - 1];
			AssertAreEqual((float)0, nullItem._typedPrimitive);
			Assert.IsNull(nullItem._untyped);
		}

		private void AssertAreEqual(float expected, float actual)
		{
			if (float.IsNaN(expected) && Db4oHandlerVersion() == 0)
			{
				expected = 0;
			}
			if (float.IsNaN(expected) && float.IsNaN(actual))
			{
				return;
			}
			Assert.AreEqual(expected, actual);
		}

		private void AssertAreEqual(object expected, object actual)
		{
			if (float.IsNaN(((float)expected)) && Db4oHandlerVersion() == 0)
			{
				expected = null;
			}
			Assert.AreEqual(expected, actual);
		}

		protected override object CreateArrays()
		{
			FloatHandlerUpdateTestCase.ItemArrays itemArrays = new FloatHandlerUpdateTestCase.ItemArrays
				();
			itemArrays._typedPrimitiveArray = new float[data.Length];
			System.Array.Copy(data, 0, itemArrays._typedPrimitiveArray, 0, data.Length);
			float[] dataWrapper = new float[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				dataWrapper[i] = data[i];
			}
			itemArrays._typedWrapperArray = new float[data.Length + 1];
			System.Array.Copy(dataWrapper, 0, itemArrays._typedWrapperArray, 0, dataWrapper.Length
				);
			float[] primitiveArray = new float[data.Length];
			System.Array.Copy(data, 0, primitiveArray, 0, data.Length);
			itemArrays._primitiveArrayInObject = primitiveArray;
			float[] wrapperArray = new float[data.Length + 1];
			System.Array.Copy(dataWrapper, 0, wrapperArray, 0, dataWrapper.Length);
			itemArrays._wrapperArrayInObject = wrapperArray;
			return itemArrays;
		}

		protected override object[] CreateValues()
		{
			FloatHandlerUpdateTestCase.Item[] values = new FloatHandlerUpdateTestCase.Item[data
				.Length + 1];
			for (int i = 0; i < data.Length; i++)
			{
				FloatHandlerUpdateTestCase.Item item = new FloatHandlerUpdateTestCase.Item();
				item._typedPrimitive = data[i];
				item._typedWrapper = data[i];
				item._untyped = data[i];
				values[i] = item;
			}
			values[values.Length - 1] = new FloatHandlerUpdateTestCase.Item();
			return values;
		}

		protected override string TypeName()
		{
			return "float";
		}
	}
}
