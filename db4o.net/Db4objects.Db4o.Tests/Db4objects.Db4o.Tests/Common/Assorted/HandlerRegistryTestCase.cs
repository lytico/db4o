/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Handlers;
using Db4objects.Db4o.Internal.Handlers.Array;
using Db4objects.Db4o.Internal.Handlers.Versions;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Assorted;
using Db4objects.Db4o.Typehandlers;

namespace Db4objects.Db4o.Tests.Common.Assorted
{
	public class HandlerRegistryTestCase : AbstractDb4oTestCase
	{
		public interface IFooInterface
		{
		}

		public class Item
		{
		}

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			Store(new HandlerRegistryTestCase.Item());
		}

		public virtual void TestCorrectHandlerVersion()
		{
			OpenTypeHandler openTypeHandler = new OpenTypeHandler(Container());
			AssertCorrectedHandlerVersion(typeof(OpenTypeHandler0), openTypeHandler, -1);
			AssertCorrectedHandlerVersion(typeof(OpenTypeHandler0), openTypeHandler, 0);
			AssertCorrectedHandlerVersion(typeof(OpenTypeHandler2), openTypeHandler, 1);
			AssertCorrectedHandlerVersion(typeof(OpenTypeHandler2), openTypeHandler, 2);
			AssertCorrectedHandlerVersion(typeof(OpenTypeHandler), openTypeHandler, HandlerRegistry
				.HandlerVersion);
			AssertCorrectedHandlerVersion(typeof(OpenTypeHandler), openTypeHandler, HandlerRegistry
				.HandlerVersion + 1);
			StandardReferenceTypeHandler stdReferenceHandler = new StandardReferenceTypeHandler
				(ItemClassMetadata());
			AssertCorrectedHandlerVersion(typeof(StandardReferenceTypeHandler0), stdReferenceHandler
				, 0);
			AssertCorrectedHandlerVersion(typeof(StandardReferenceTypeHandler), stdReferenceHandler
				, 2);
			PrimitiveTypeMetadata primitiveMetadata = new PrimitiveTypeMetadata(Container(), 
				openTypeHandler, 0, null);
			AssertPrimitiveHandlerDelegate(typeof(OpenTypeHandler0), primitiveMetadata, 0);
			AssertPrimitiveHandlerDelegate(typeof(OpenTypeHandler2), primitiveMetadata, 1);
			AssertPrimitiveHandlerDelegate(typeof(OpenTypeHandler2), primitiveMetadata, 2);
			AssertPrimitiveHandlerDelegate(typeof(OpenTypeHandler), primitiveMetadata, HandlerRegistry
				.HandlerVersion);
			ArrayHandler arrayHandler = new ArrayHandler(openTypeHandler, false);
			AssertCorrectedHandlerVersion(typeof(ArrayHandler0), arrayHandler, 0);
			AssertCorrectedHandlerVersion(typeof(ArrayHandler1), arrayHandler, 1);
			AssertCorrectedHandlerVersion(typeof(ArrayHandler3), arrayHandler, 2);
			AssertCorrectedHandlerVersion(typeof(ArrayHandler3), arrayHandler, 3);
			AssertCorrectedHandlerVersion(typeof(ArrayHandler), arrayHandler, HandlerRegistry
				.HandlerVersion);
			ArrayHandler multidimensionalArrayHandler = new MultidimensionalArrayHandler(openTypeHandler
				, false);
			AssertCorrectedHandlerVersion(typeof(MultidimensionalArrayHandler0), multidimensionalArrayHandler
				, 0);
			AssertCorrectedHandlerVersion(typeof(MultidimensionalArrayHandler3), multidimensionalArrayHandler
				, 1);
			AssertCorrectedHandlerVersion(typeof(MultidimensionalArrayHandler3), multidimensionalArrayHandler
				, 2);
			AssertCorrectedHandlerVersion(typeof(MultidimensionalArrayHandler3), multidimensionalArrayHandler
				, 3);
			AssertCorrectedHandlerVersion(typeof(MultidimensionalArrayHandler), multidimensionalArrayHandler
				, HandlerRegistry.HandlerVersion);
		}

		private void AssertPrimitiveHandlerDelegate(Type expectedClass, PrimitiveTypeMetadata
			 primitiveMetadata, int version)
		{
			ITypeHandler4 correctTypeHandler = (ITypeHandler4)CorrectHandlerVersion(primitiveMetadata
				.TypeHandler(), version);
			Assert.AreSame(expectedClass, correctTypeHandler.GetType());
		}

		private ClassMetadata ItemClassMetadata()
		{
			return Container().ClassMetadataForObject(new HandlerRegistryTestCase.Item());
		}

		private void AssertCorrectedHandlerVersion(Type expectedClass, ITypeHandler4 typeHandler
			, int version)
		{
			Assert.AreSame(expectedClass, CorrectHandlerVersion(typeHandler, version).GetType
				());
		}

		private ITypeHandler4 CorrectHandlerVersion(ITypeHandler4 typeHandler, int version
			)
		{
			return Handlers().CorrectHandlerVersion(typeHandler, version);
		}

		private HandlerRegistry Handlers()
		{
			return Stream().Handlers;
		}

		public virtual void TestTypeHandlerForID()
		{
			AssertTypeHandler(typeof(IntHandler), Handlers4.IntId);
			AssertTypeHandler(typeof(OpenTypeHandler), Handlers4.UntypedId);
			AssertTypeHandler(typeof(IntHandler), Handlers4.IntId);
			AssertTypeHandler(typeof(ArrayHandler), Handlers4.AnyArrayId);
			AssertTypeHandler(typeof(MultidimensionalArrayHandler), Handlers4.AnyArrayNId);
		}

		private void AssertTypeHandler(Type expectedHandlerClass, int classMetadataID)
		{
			ITypeHandler4 handler = Container().ClassMetadataForID(classMetadataID).TypeHandler
				();
			Assert.IsInstanceOf(expectedHandlerClass, handler);
		}

		public virtual void TestTypeHandlerForClass()
		{
			Assert.IsInstanceOf(typeof(IntHandler), Handlers().TypeHandlerForClass(IntegerClassReflector
				()));
			Assert.IsInstanceOf(typeof(OpenTypeHandler), Handlers().TypeHandlerForClass(ObjectClassReflector
				()));
		}

		public virtual void TestClassForID()
		{
			IReflectClass byReflector = IntegerClassReflector();
			IReflectClass byID = Handlers().ClassForID(Handlers4.IntId);
			Assert.IsNotNull(byID);
			Assert.AreEqual(byReflector, byID);
		}

		public virtual void TestClassReflectorForHandler()
		{
			IReflectClass byReflector = IntegerClassReflector();
			IReflectClass byID = Handlers().ClassForID(Handlers4.IntId);
			Assert.IsNotNull(byID);
			Assert.AreEqual(byReflector, byID);
		}

		private IReflectClass ObjectClassReflector()
		{
			return ReflectorFor(typeof(object));
		}

		private IReflectClass IntegerClassReflector()
		{
			return ReflectorFor(Platform4.NullableTypeFor(typeof(int)));
		}

		private IReflectClass ReflectorFor(Type clazz)
		{
			return Reflector().ForClass(clazz);
		}

		public static void Main(string[] arguments)
		{
			new HandlerRegistryTestCase().RunSolo();
		}
	}
}
