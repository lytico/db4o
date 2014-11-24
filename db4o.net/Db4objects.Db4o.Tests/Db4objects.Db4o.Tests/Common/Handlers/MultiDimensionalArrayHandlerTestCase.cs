/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4oUnit;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class MultiDimensionalArrayHandlerTestCase : TypeHandlerTestCaseBase
	{
		public static void Main(string[] args)
		{
			new MultiDimensionalArrayHandlerTestCase().RunSolo();
		}

		internal static readonly int[][] ArrayData = new int[][] { new int[] { 1, 2, 3 }, 
			new int[] { 6, 5, 4 } };

		internal static readonly int[] Data = new int[] { 1, 2, 3, 6, 5, 4 };

		public class Item
		{
			public int[][] _int;

			public Item(int[][] int_)
			{
				_int = int_;
			}

			public override bool Equals(object obj)
			{
				if (obj == this)
				{
					return true;
				}
				if (!(obj is MultiDimensionalArrayHandlerTestCase.Item))
				{
					return false;
				}
				MultiDimensionalArrayHandlerTestCase.Item other = (MultiDimensionalArrayHandlerTestCase.Item
					)obj;
				if (_int.Length != other._int.Length)
				{
					return false;
				}
				for (int i = 0; i < _int.Length; i++)
				{
					if (_int[i].Length != other._int[i].Length)
					{
						return false;
					}
					for (int j = 0; j < _int[i].Length; j++)
					{
						if (_int[i][j] != other._int[i][j])
						{
							return false;
						}
					}
				}
				return true;
			}
		}

		private ArrayHandler IntArrayHandler()
		{
			return ArrayHandler(typeof(int), true);
		}

		private ArrayHandler ArrayHandler(Type clazz, bool isPrimitive)
		{
			ITypeHandler4 typeHandler = (ITypeHandler4)Container().TypeHandlerForClass(Reflector
				().ForClass(clazz));
			return new MultidimensionalArrayHandler(typeHandler, isPrimitive);
		}

		public virtual void TestReadWrite()
		{
			MockWriteContext writeContext = new MockWriteContext(Db());
			MultiDimensionalArrayHandlerTestCase.Item expected = new MultiDimensionalArrayHandlerTestCase.Item
				(ArrayData);
			IntArrayHandler().Write(writeContext, expected._int);
			MockReadContext readContext = new MockReadContext(writeContext);
			int[][] arr = (int[][])IntArrayHandler().Read(readContext);
			MultiDimensionalArrayHandlerTestCase.Item actualValue = new MultiDimensionalArrayHandlerTestCase.Item
				(arr);
			Assert.AreEqual(expected, actualValue);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestStoreObject()
		{
			MultiDimensionalArrayHandlerTestCase.Item storedItem = new MultiDimensionalArrayHandlerTestCase.Item
				(new int[][] { new int[] { 1, 2, 3 }, new int[] { 6, 5, 4 } });
			DoTestStoreObject(storedItem);
		}

		public virtual void TestAllElements()
		{
			int pos = 0;
			IEnumerator allElements = IntArrayHandler().AllElements(Container(), ArrayData);
			while (allElements.MoveNext())
			{
				Assert.AreEqual(Data[pos++], allElements.Current);
			}
			Assert.AreEqual(pos, Data.Length);
		}
	}
}
