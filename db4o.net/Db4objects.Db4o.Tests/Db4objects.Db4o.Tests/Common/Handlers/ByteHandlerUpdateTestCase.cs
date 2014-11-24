/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Tests.Util;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class ByteHandlerUpdateTestCase : HandlerUpdateTestCaseBase
	{
		public class Item
		{
			public byte _typedPrimitive;

			public byte _typedWrapper;

			public object _untyped;
		}

		public class ItemArrays
		{
			public byte[] _typedPrimitiveArray;

			public byte[] _typedWrapperArray;

			public object[] _untypedObjectArray;

			public object _primitiveArrayInObject;

			public object _wrapperArrayInObject;
		}

		public static readonly byte[] data = new byte[] { byte.MinValue, byte.MinValue + 
			1, (byte)unchecked((int)(0xFB)), (byte)unchecked((int)(0xFF)), 0, 1, 5, byte.MaxValue
			 - 1, byte.MaxValue };

		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(ByteHandlerUpdateTestCase)).Run();
		}

		protected override void AssertArrays(IExtObjectContainer objectContainer, object 
			obj)
		{
			ByteHandlerUpdateTestCase.ItemArrays itemArrays = (ByteHandlerUpdateTestCase.ItemArrays
				)obj;
			AssertPrimitiveArray(itemArrays._typedPrimitiveArray);
			if (Db4oHeaderVersion() == VersionServices.Header3040)
			{
			}
			else
			{
				// Bug in the oldest format: It accidentally byte[] arrays to Byte[]
				// arrays.
				if (Db4oHandlerVersion() == 1 && Db4oHeaderVersion() == 100 && itemArrays._primitiveArrayInObject
					 == null)
				{
				}
				else
				{
					// do nothing
					// We started treating byte[] in untyped variables differently
					// but we forgot to update the handler version.
					// Concerns only 6.3.500: Updates are not possible.
					AssertPrimitiveArray((byte[])itemArrays._primitiveArrayInObject);
				}
			}
		}

		private void AssertPrimitiveArray(byte[] primitiveArray)
		{
			for (int i = 0; i < data.Length; i++)
			{
				AssertAreEqual(data[i], primitiveArray[i]);
			}
		}

		// FIXME: Arrays should also get a null Bitmap to fix.
		// Assert.isNull(wrapperArray[wrapperArray.length - 1]);
		protected override void AssertValues(IExtObjectContainer objectContainer, object[]
			 values)
		{
			for (int i = 0; i < data.Length; i++)
			{
				ByteHandlerUpdateTestCase.Item item = (ByteHandlerUpdateTestCase.Item)values[i];
				AssertAreEqual(data[i], item._typedPrimitive);
				AssertAreEqual(data[i], item._typedWrapper);
				AssertAreEqual(data[i], item._untyped);
			}
			ByteHandlerUpdateTestCase.Item nullItem = (ByteHandlerUpdateTestCase.Item)values[
				values.Length - 1];
			AssertAreEqual((byte)0, nullItem._typedPrimitive);
			Assert.IsNull(nullItem._untyped);
		}

		private void AssertAreEqual(byte expected, byte actual)
		{
			Assert.AreEqual(expected, actual);
		}

		private void AssertAreEqual(object expected, object actual)
		{
			Assert.AreEqual(expected, actual);
		}

		protected override object CreateArrays()
		{
			ByteHandlerUpdateTestCase.ItemArrays itemArrays = new ByteHandlerUpdateTestCase.ItemArrays
				();
			itemArrays._typedPrimitiveArray = new byte[data.Length];
			System.Array.Copy(data, 0, itemArrays._typedPrimitiveArray, 0, data.Length);
			byte[] dataWrapper = new byte[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				dataWrapper[i] = data[i];
			}
			itemArrays._typedWrapperArray = new byte[data.Length + 1];
			System.Array.Copy(dataWrapper, 0, itemArrays._typedWrapperArray, 0, dataWrapper.Length
				);
			byte[] primitiveArray = new byte[data.Length];
			System.Array.Copy(data, 0, primitiveArray, 0, data.Length);
			if (PrimitiveArrayInUntypedVariableSupported())
			{
				itemArrays._primitiveArrayInObject = primitiveArray;
			}
			byte[] wrapperArray = new byte[data.Length + 1];
			System.Array.Copy(dataWrapper, 0, wrapperArray, 0, dataWrapper.Length);
			if (PrimitiveArrayInUntypedVariableSupported() || !Deploy.csharp)
			{
				itemArrays._wrapperArrayInObject = wrapperArray;
			}
			return itemArrays;
		}

		private bool PrimitiveArrayInUntypedVariableSupported()
		{
			// We can't convert primitive arrays in object variables in 6.3
			// Keeping the member to null value helps to identify the exact version.
			return !(Db4oMajorVersion() == 6 && Db4oMinorVersion() == 3);
		}

		protected override object[] CreateValues()
		{
			ByteHandlerUpdateTestCase.Item[] values = new ByteHandlerUpdateTestCase.Item[data
				.Length + 1];
			for (int i = 0; i < data.Length; i++)
			{
				ByteHandlerUpdateTestCase.Item item = new ByteHandlerUpdateTestCase.Item();
				item._typedPrimitive = data[i];
				item._typedWrapper = data[i];
				item._untyped = data[i];
				values[i] = item;
			}
			values[values.Length - 1] = new ByteHandlerUpdateTestCase.Item();
			return values;
		}

		// Bug when reading old format:
		// Null wrappers are converted to 0
		protected override string TypeName()
		{
			return "byte";
		}
	}
}
