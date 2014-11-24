/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Tests.Util;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class DoubleHandlerUpdateTestCase : HandlerUpdateTestCaseBase
	{
		private readonly double[] data;

		public DoubleHandlerUpdateTestCase()
		{
			data = new double[] { double.MinValue, double.MinValue + 1, -3.1415926535789, -1, 
				0, UsesNullMarkerValue() ? 0 : double.NaN, double.NegativeInfinity, double.PositiveInfinity
				, 1, 3.1415926535789, double.MaxValue - 1, double.MaxValue };
		}

		public class Item
		{
			public double _typedPrimitive;

			public double _typedWrapper;

			public object _untyped;
		}

		public class ItemArrays
		{
			public double[] _typedPrimitiveArray;

			public double[] _typedWrapperArray;

			public object[] _untypedObjectArray;

			public object _primitiveArrayInObject;

			public object _wrapperArrayInObject;
		}

		protected override void AssertArrays(IExtObjectContainer objectContainer, object 
			obj)
		{
			DoubleHandlerUpdateTestCase.ItemArrays item = (DoubleHandlerUpdateTestCase.ItemArrays
				)obj;
			AssertTypedPrimitiveArray(item);
			AssertTypedWrapperArray(item);
			// Will be removed for .NET by sharpen.
			AssertPrimitiveArrayInObject(item);
			AssertWrapperArrayInObject(item);
		}

		private void AssertTypedPrimitiveArray(DoubleHandlerUpdateTestCase.ItemArrays item
			)
		{
			AssertData(item._typedPrimitiveArray);
		}

		private void AssertTypedWrapperArray(DoubleHandlerUpdateTestCase.ItemArrays item)
		{
			AssertWrapperData(item._typedWrapperArray);
		}

		private void AssertPrimitiveArrayInObject(DoubleHandlerUpdateTestCase.ItemArrays 
			item)
		{
			if (Db4oHeaderVersion() == VersionServices.Header3040)
			{
				// Bug in the oldest format: It accidentally double[] arrays to Double[] arrays.
				AssertWrapperData((double[])item._primitiveArrayInObject);
			}
			else
			{
				AssertData((double[])item._primitiveArrayInObject);
			}
		}

		private void AssertWrapperArrayInObject(DoubleHandlerUpdateTestCase.ItemArrays item
			)
		{
			AssertWrapperData((double[])item._wrapperArrayInObject);
		}

		private void AssertData(double[] values)
		{
			for (int i = 0; i < data.Length; i++)
			{
				AssertAreEqual(data[i], values[i]);
			}
		}

		private void AssertWrapperData(double[] values)
		{
			for (int i = 0; i < data.Length; i++)
			{
				AssertAreEqual(data[i], values[i]);
			}
		}

		// FIXME: The following fails as is because of a deficiency 
		//        in the storage format of arrays.
		//        Arrays should also get a null Bitmap to fix.
		// Assert.isNull(values[values.length - 1]);
		protected override void AssertValues(IExtObjectContainer objectContainer, object[]
			 values)
		{
			for (int i = 0; i < data.Length; i++)
			{
				DoubleHandlerUpdateTestCase.Item item = (DoubleHandlerUpdateTestCase.Item)values[
					i];
				AssertAreEqual(data[i], item._typedPrimitive);
				AssertAreEqual(data[i], item._typedWrapper);
				AssertAreEqual(data[i], item._untyped);
			}
			DoubleHandlerUpdateTestCase.Item nullItem = (DoubleHandlerUpdateTestCase.Item)values
				[values.Length - 1];
			Assert.AreEqual(0, nullItem._typedPrimitive);
			Assert.IsNull(nullItem._untyped);
		}

		protected override object[] CreateValues()
		{
			DoubleHandlerUpdateTestCase.Item[] values = new DoubleHandlerUpdateTestCase.Item[
				data.Length + 1];
			for (int i = 0; i < data.Length; i++)
			{
				DoubleHandlerUpdateTestCase.Item item = new DoubleHandlerUpdateTestCase.Item();
				values[i] = item;
				item._typedPrimitive = data[i];
				item._typedWrapper = data[i];
				item._untyped = data[i];
			}
			values[values.Length - 1] = new DoubleHandlerUpdateTestCase.Item();
			return values;
		}

		protected override object CreateArrays()
		{
			DoubleHandlerUpdateTestCase.ItemArrays item = new DoubleHandlerUpdateTestCase.ItemArrays
				();
			CreateTypedPrimitiveArray(item);
			CreateTypedWrapperArray(item);
			// Will be removed for .NET by sharpen.
			CreatePrimitiveArrayInObject(item);
			CreateWrapperArrayInObject(item);
			return item;
		}

		private void CreateTypedPrimitiveArray(DoubleHandlerUpdateTestCase.ItemArrays item
			)
		{
			item._typedPrimitiveArray = new double[data.Length];
			System.Array.Copy(data, 0, item._typedPrimitiveArray, 0, data.Length);
		}

		private void CreateTypedWrapperArray(DoubleHandlerUpdateTestCase.ItemArrays item)
		{
			item._typedWrapperArray = new double[data.Length + 1];
			for (int i = 0; i < data.Length; i++)
			{
				item._typedWrapperArray[i] = data[i];
			}
		}

		private void CreatePrimitiveArrayInObject(DoubleHandlerUpdateTestCase.ItemArrays 
			item)
		{
			double[] arr = new double[data.Length];
			System.Array.Copy(data, 0, arr, 0, data.Length);
			item._primitiveArrayInObject = arr;
		}

		private void CreateWrapperArrayInObject(DoubleHandlerUpdateTestCase.ItemArrays item
			)
		{
			double[] arr = new double[data.Length + 1];
			for (int i = 0; i < data.Length; i++)
			{
				arr[i] = data[i];
			}
			item._wrapperArrayInObject = arr;
		}

		protected override string TypeName()
		{
			return "double";
		}

		private void AssertAreEqual(double expected, double actual)
		{
			if (double.IsNaN(expected) && UsesNullMarkerValue())
			{
				expected = 0;
			}
			if (double.IsNaN(expected) && double.IsNaN(actual))
			{
				return;
			}
			Assert.AreEqual(expected, actual);
		}

		private void AssertAreEqual(object expected, object actual)
		{
			if (double.IsNaN(((double)expected)) && UsesNullMarkerValue())
			{
				expected = null;
			}
			if (expected != null && actual != null && double.IsNaN(((double)expected)) && double.IsNaN
				(((double)actual)))
			{
				return;
			}
			Assert.AreEqual(expected, actual);
		}
	}
}
