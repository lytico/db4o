/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4oUnit;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Tests.Common.Handlers;
using Sharpen;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class CharHandlerUpdateTestCase : HandlerUpdateTestCaseBase
	{
		public class Item
		{
			public char _typedPrimitive;

			public char _typedWrapper;

			public object _untyped;
		}

		public class ItemArrays
		{
			public char[] _typedPrimitiveArray;

			public char[] _typedWrapperArray;

			public object[] _untypedObjectArray;

			public object _primitiveArrayInObject;

			public object _wrapperArrayInObject;
		}

		private static readonly char[] data = new char[] { char.MinValue, (char)unchecked(
			(int)(0x0000)), (char)unchecked((int)(0x000F)), (char)unchecked((int)(0x00F0)), 
			(char)unchecked((int)(0x00FF)), (char)unchecked((int)(0x0F00)), (char)unchecked(
			(int)(0x0F0F)), (char)unchecked((int)(0x0FF0)), (char)unchecked((int)(0x0FFF)), 
			(char)unchecked((int)(0xF000)), (char)unchecked((int)(0xF00F)), (char)unchecked(
			(int)(0xF0F0)), (char)unchecked((int)(0xF0FF)), (char)unchecked((int)(0xFF00)), 
			(char)unchecked((int)(0xFF0F)), (char)unchecked((int)(0xFFF0)), (char)unchecked(
			(int)(0xFFFF)), char.MaxValue };

		public static void Main(string[] args)
		{
			new ConsoleTestRunner(typeof(CharHandlerUpdateTestCase)).Run();
		}

		protected override void AssertArrays(IExtObjectContainer objectContainer, object 
			obj)
		{
			CharHandlerUpdateTestCase.ItemArrays itemArrays = (CharHandlerUpdateTestCase.ItemArrays
				)obj;
			AssertPrimitiveArray(itemArrays._typedPrimitiveArray);
			AssertPrimitiveArray(CastToCharArray(itemArrays._primitiveArrayInObject));
			AssertWrapperArray(itemArrays._typedWrapperArray);
			AssertWrapperArray((char[])itemArrays._wrapperArrayInObject);
		}

		private void AssertPrimitiveArray(char[] primitiveArray)
		{
			for (int i = 0; i < data.Length; i++)
			{
				AssertAreEqual(data[i], primitiveArray[i]);
			}
		}

		private void AssertWrapperArray(char[] wrapperArray)
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
				CharHandlerUpdateTestCase.Item item = (CharHandlerUpdateTestCase.Item)values[i];
				AssertAreEqual(data[i], item._typedPrimitive);
				AssertAreEqual(data[i], item._typedWrapper);
				AssertAreEqual(data[i], item._untyped);
			}
			CharHandlerUpdateTestCase.Item nullItem = (CharHandlerUpdateTestCase.Item)values[
				values.Length - 1];
			AssertAreEqual((char)0, nullItem._typedPrimitive);
			Assert.IsNull(nullItem._untyped);
		}

		protected override object CreateArrays()
		{
			CharHandlerUpdateTestCase.ItemArrays itemArrays = new CharHandlerUpdateTestCase.ItemArrays
				();
			itemArrays._typedPrimitiveArray = new char[data.Length];
			System.Array.Copy(data, 0, itemArrays._typedPrimitiveArray, 0, data.Length);
			char[] dataWrapper = new char[data.Length];
			for (int i = 0; i < data.Length; i++)
			{
				dataWrapper[i] = data[i];
			}
			itemArrays._typedWrapperArray = new char[data.Length + 1];
			System.Array.Copy(dataWrapper, 0, itemArrays._typedWrapperArray, 0, dataWrapper.Length
				);
			char[] primitiveArray = new char[data.Length];
			System.Array.Copy(data, 0, primitiveArray, 0, data.Length);
			itemArrays._primitiveArrayInObject = primitiveArray;
			char[] wrapperArray = new char[data.Length + 1];
			System.Array.Copy(dataWrapper, 0, wrapperArray, 0, dataWrapper.Length);
			itemArrays._wrapperArrayInObject = wrapperArray;
			return itemArrays;
		}

		protected override object[] CreateValues()
		{
			CharHandlerUpdateTestCase.Item[] values = new CharHandlerUpdateTestCase.Item[data
				.Length + 1];
			for (int i = 0; i < data.Length; i++)
			{
				CharHandlerUpdateTestCase.Item item = new CharHandlerUpdateTestCase.Item();
				item._typedPrimitive = data[i];
				item._typedWrapper = data[i];
				item._untyped = data[i];
				values[i] = item;
			}
			values[values.Length - 1] = new CharHandlerUpdateTestCase.Item();
			return values;
		}

		protected override string TypeName()
		{
			return "char";
		}

		private void AssertAreEqual(char expected, char actual)
		{
			Assert.AreEqual(expected, actual);
		}

		private void AssertAreEqual(object expected, object actual)
		{
			Assert.AreEqual(expected, actual);
		}

		// Bug when reading old format:
		// Null wrappers are converted to Character.MAX_VALUE
		private char[] CastToCharArray(object obj)
		{
			ObjectByRef byRef = new ObjectByRef(obj);
			return (char[])byRef.value;
		}
		// Bug in the oldest format: 
		// It accidentally converted char[] arrays to Character[] arrays.
	}
}
