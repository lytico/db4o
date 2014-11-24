/* Copyright (C) 2009  Versant Inc.  http://www.db4o.com */
using System;
using Db4objects.Db4o.Ext;
using Db4oUnit;

namespace Db4objects.Db4o.Tests.CLI1.Handlers
{
	internal abstract class ValueTypeHandlerUpdateTestCaseBase<T> : LenientHandlerUpdateTestCaseBase where T : struct
	{
		protected abstract T[] GetData();

		protected ValueTypeHandlerUpdateTestCaseBase()
		{
			Data = GetData();
		}

		protected override string TypeName()
		{
			return typeof(T).Name;
		}

		#region Overrides of HandlerUpdateTestCaseBase

		protected override void AssertArrays(IExtObjectContainer objectContainer, object obj)
		{
			ItemArrays itemArrays = (ItemArrays)obj;
			T[] valueTypeArray = (T[]) itemArrays.ArrayInObject;
			for (int i = 0; i < Data.Length; i++)
			{
				AssertAreEqual(Data[i], itemArrays.TypedArray[i]);
				AssertAreEqual(Data[i], (T)itemArrays.UntypedObjectArray[i]);
				AssertAreEqual(Data[i], valueTypeArray[i]);
				if (NullableSupported())
				{
					AssertAreEqual(Data[i], (T)itemArrays.NullableArray[i]);
				}
			}

			Assert.IsNull(itemArrays.UntypedObjectArray[Data.Length]);
			AssertAreEqual(default(T), itemArrays.TypedArray[Data.Length]);
			AssertAreEqual(default(T), valueTypeArray[Data.Length]);
			if (NullableSupported())
			{
				Assert.IsNull(itemArrays.NullableArray[Data.Length]);
			}
		}

		protected override void AssertValues(IExtObjectContainer objectContainer, object[] values)
		{
			for (int i = 0; i < Data.Length; i++)
			{
				Item item = (Item)values[i];
				AssertAreEqual(Data[i], item.Typed);
				AssertAreEqual(Data[i], (T) item.Untyped);
				AssertAreEqual(Data[i], (T) item.Nullable);
			}

			Item nullItem = (Item) values[values.Length - 1];

			AssertAreEqual(default(T), nullItem.Typed);
			Assert.IsNull(nullItem.Untyped);
			Assert.IsNull(nullItem.Nullable);

			AssertNoClassIndex(objectContainer);
		}

		private static void AssertNoClassIndex(IExtObjectContainer container)
		{
			Assert.AreEqual(0, container.StoredClass(typeof(T)).InstanceCount());
		}

		private static void AssertAreEqual<T>(T expected, T actual)
		{
			Assert.AreEqual(expected, actual);
		}

		protected override object CreateArrays()
		{
			ItemArrays itemArrays = new ItemArrays();
			itemArrays.TypedArray = new T[Data.Length + 1];
			Array.Copy(Data, 0, itemArrays.TypedArray, 0, Data.Length);

			itemArrays.UntypedObjectArray = new object[Data.Length + 1];
			Array.Copy(Data, 0, itemArrays.UntypedObjectArray, 0, Data.Length);

			T[] valueTypeArray = new T[Data.Length + 1];
			Array.Copy(Data, 0, valueTypeArray, 0, Data.Length);
			itemArrays.ArrayInObject = valueTypeArray;

			itemArrays.NullableArray = new T?[Data.Length + 1];
			for (int i = 0; i < Data.Length; i++)
			{
				itemArrays.NullableArray[i] = Data[i];
			}
			return itemArrays;
		}

		protected override object[] CreateValues()
		{
			Item[] values = new Item[Data.Length + 1];
			for (int i = 0; i < Data.Length; i++)
			{
				values[i] = new Item(Data[i]);
			}
			values[values.Length - 1] = new Item();

			return values;
		}

		protected override bool DefragmentInReadWriteMode()
		{
			return true;
		}

		protected class Item
		{
			public readonly T Typed;
			public readonly Object Untyped;
			public readonly T? Nullable;

			public Item(T value)
			{
				Typed = value;
				Untyped = value;
				Nullable = value;
			}

			public Item()
			{
			}
		}

		class ItemArrays
		{
			public T[] TypedArray;
			public object[] UntypedObjectArray;
			public object ArrayInObject;
			public T?[] NullableArray;
		}

		private readonly T[] Data;

		#endregion
	}
}
