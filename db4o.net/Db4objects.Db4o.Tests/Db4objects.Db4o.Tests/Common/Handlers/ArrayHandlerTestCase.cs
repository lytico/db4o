/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Handlers;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Common.Handlers
{
	public class ArrayHandlerTestCase : AbstractDb4oTestCase
	{
		public class FloatArrayHolder
		{
			public float[] _floats;

			public float[][] _jaggedFloats;

			public float[][] _jaggedFloatWrappers;

			public FloatArrayHolder()
			{
			}

			public FloatArrayHolder(float[] floats)
			{
				// for jres that require instantiation through the constructor
				_floats = floats;
				_jaggedFloats = new float[][] { floats };
				_jaggedFloatWrappers = new float[][] { Lift(floats) };
			}

			public static float[] Lift(float[] floats)
			{
				float[] wrappers = new float[floats.Length];
				for (int i = 0; i < floats.Length; ++i)
				{
					wrappers[i] = floats[i];
				}
				return wrappers;
			}

			public virtual float[] Floats()
			{
				return _floats;
			}

			public virtual float[] JaggedFloats()
			{
				return _jaggedFloats[0];
			}

			public virtual float[] JaggedWrappers()
			{
				return _jaggedFloatWrappers[0];
			}
		}

		public class IntArrayHolder
		{
			public int[] _ints;

			public int[][] _jaggedInts;

			public IntArrayHolder(int[] ints)
			{
				_ints = ints;
				_jaggedInts = new int[][] { _ints };
			}

			public virtual int[] JaggedInts()
			{
				return _jaggedInts[0];
			}
		}

		public class StringArrayHolder
		{
			public string[] _strings;

			public StringArrayHolder(string[] strings)
			{
				_strings = strings;
			}
		}

		public static void Main(string[] args)
		{
			new ArrayHandlerTestCase().RunSolo();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestFloatArrayRoundtrip()
		{
			float[] expected = new float[] { float.MinValue, float.MinValue + 1, 0.0f, float.MaxValue
				 - 1, float.MaxValue };
			Store(new ArrayHandlerTestCase.FloatArrayHolder(expected));
			Reopen();
			ArrayHandlerTestCase.FloatArrayHolder stored = ((ArrayHandlerTestCase.FloatArrayHolder
				)RetrieveOnlyInstance(typeof(ArrayHandlerTestCase.FloatArrayHolder)));
			ArrayAssert.AreEqual(expected, stored.JaggedFloats());
			ArrayAssert.AreEqual(expected, stored.Floats());
			ArrayAssert.AreEqual(ArrayHandlerTestCase.FloatArrayHolder.Lift(expected), stored
				.JaggedWrappers());
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestArraysHaveNoIdentity()
		{
			float[] expected = new float[] { float.MinValue, float.MinValue + 1, 0.0f, float.MaxValue
				 - 1, float.MaxValue };
			Store(new ArrayHandlerTestCase.FloatArrayHolder(expected));
			Store(new ArrayHandlerTestCase.FloatArrayHolder(expected));
			Reopen();
			IObjectSet stored = Db().Query(typeof(ArrayHandlerTestCase.FloatArrayHolder));
			ArrayHandlerTestCase.FloatArrayHolder first = ((ArrayHandlerTestCase.FloatArrayHolder
				)stored.Next());
			ArrayHandlerTestCase.FloatArrayHolder second = ((ArrayHandlerTestCase.FloatArrayHolder
				)stored.Next());
			Assert.AreNotSame(first._floats, second._floats);
		}

		public virtual void TestHandlerVersion()
		{
			ArrayHandlerTestCase.IntArrayHolder intArrayHolder = new ArrayHandlerTestCase.IntArrayHolder
				(new int[0]);
			Store(intArrayHolder);
			IReflectClass claxx = Reflector().ForObject(intArrayHolder);
			ClassMetadata classMetadata = (ClassMetadata)Container().ProduceClassMetadata(claxx
				);
			FieldMetadata fieldMetadata = classMetadata.FieldMetadataForName("_ints");
			ITypeHandler4 arrayHandler = fieldMetadata.GetHandler();
			Assert.IsInstanceOf(typeof(ArrayHandler), arrayHandler);
			AssertCorrectedHandlerVersion(arrayHandler, 0, typeof(ArrayHandler0));
			AssertCorrectedHandlerVersion(arrayHandler, 1, typeof(ArrayHandler1));
			AssertCorrectedHandlerVersion(arrayHandler, 2, typeof(ArrayHandler3));
			AssertCorrectedHandlerVersion(arrayHandler, 3, typeof(ArrayHandler3));
			AssertCorrectedHandlerVersion(arrayHandler, HandlerRegistry.HandlerVersion, typeof(
				ArrayHandler));
		}

		public virtual void TestIntArrayReadWrite()
		{
			MockWriteContext writeContext = new MockWriteContext(Db());
			int[] expected = new int[] { 7, 8, 9 };
			IntArrayHandler().Write(writeContext, expected);
			MockReadContext readContext = new MockReadContext(writeContext);
			int[] actual = (int[])IntArrayHandler().Read(readContext);
			ArrayAssert.AreEqual(expected, actual);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestIntArrayStoreObject()
		{
			ArrayHandlerTestCase.IntArrayHolder expectedItem = new ArrayHandlerTestCase.IntArrayHolder
				(new int[] { 1, 2, 3 });
			Db().Store(expectedItem);
			Db().Purge(expectedItem);
			ArrayHandlerTestCase.IntArrayHolder readItem = (ArrayHandlerTestCase.IntArrayHolder
				)((ArrayHandlerTestCase.IntArrayHolder)RetrieveOnlyInstance(typeof(ArrayHandlerTestCase.IntArrayHolder
				)));
			Assert.AreNotSame(expectedItem, readItem);
			ArrayAssert.AreEqual(expectedItem._ints, readItem._ints);
			ArrayAssert.AreEqual(expectedItem._ints, readItem.JaggedInts());
		}

		public virtual void TestStringArrayReadWrite()
		{
			MockWriteContext writeContext = new MockWriteContext(Db());
			string[] expected = new string[] { "one", "two", "three" };
			StringArrayHandler().Write(writeContext, expected);
			MockReadContext readContext = new MockReadContext(writeContext);
			string[] actual = (string[])StringArrayHandler().Read(readContext);
			ArrayAssert.AreEqual(expected, actual);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestStringArrayStoreObject()
		{
			ArrayHandlerTestCase.StringArrayHolder expectedItem = new ArrayHandlerTestCase.StringArrayHolder
				(new string[] { "one", "two", "three" });
			Db().Store(expectedItem);
			Db().Purge(expectedItem);
			ArrayHandlerTestCase.StringArrayHolder readItem = (ArrayHandlerTestCase.StringArrayHolder
				)((ArrayHandlerTestCase.StringArrayHolder)RetrieveOnlyInstance(typeof(ArrayHandlerTestCase.StringArrayHolder
				)));
			Assert.AreNotSame(expectedItem, readItem);
			ArrayAssert.AreEqual(expectedItem._strings, readItem._strings);
		}

		private ArrayHandler ArrayHandler(Type clazz, bool isPrimitive)
		{
			ClassMetadata classMetadata = Container().ProduceClassMetadata(Reflector().ForClass
				(clazz));
			return new ArrayHandler(classMetadata.TypeHandler(), isPrimitive);
		}

		private void AssertCorrectedHandlerVersion(ITypeHandler4 arrayHandler, int version
			, Type handlerClass)
		{
			ITypeHandler4 correctedHandlerVersion = Container().Handlers.CorrectHandlerVersion
				(arrayHandler, version);
			Assert.IsInstanceOf(handlerClass, correctedHandlerVersion);
		}

		private ArrayHandler IntArrayHandler()
		{
			return ArrayHandler(typeof(int), true);
		}

		private ArrayHandler StringArrayHandler()
		{
			return ArrayHandler(typeof(string), false);
		}
	}
}
