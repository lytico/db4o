/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
namespace Db4objects.Db4o.Tests.SharpenLang
{
    using System;
    using System.Globalization;
    using System.Reflection;
    using Db4oUnit;
    using Sharpen.Lang;
    using System.Collections.Generic;


	class Generic<T>
	{
		public class Inner
		{
			public class Inner2<S>
			{
			}
		}

		public class InnerGeneric<S>
		{
			public class Inner2
			{
			}

			public class Generic<G>
			{
			}
		}
	}

    class SimpleGenericType<T>
    {
        public T Value;
    }    

    class GenericType<T1, T2>
    {
        public T1 First;
        public T2 Second;

        public class NestedInGeneric
        {
        }

        public GenericType(T1 first, T2 second)
        {
            First = first;
            Second = second;
        }
    }

    class TypeReferenceTestCase : ITestCase
    {
		class __Funny123Name_
		{
		}
    	
        class NestedType
        {
        }

		public void TestRoundTripOnOpenType()
		{
			AssertGenericType(
				typeof (Generic<>.Inner),
				typeof (Generic<>.Inner.Inner2<>),
				typeof (Generic<>.InnerGeneric<>));
		}

		public void TestRoundTripOnInnerGenericType()
		{
			AssertGenericType(
				typeof (Generic<int>.Inner),
				typeof (Generic<int>.Inner.Inner2<string[]>),
				typeof (Generic<int>.Inner.Inner2<Generic<int>.Inner>),
				typeof (Generic<int>.InnerGeneric<NestedType>),
				typeof (Generic<int[]>.InnerGeneric<NestedType>),
				typeof (Generic<int>.InnerGeneric<NestedType>.Inner2),
				typeof (Generic<int>.InnerGeneric<NestedType>.Generic<int>));
		}

    	private static void AssertGenericType(params Type[] types)
    	{
    		foreach (Type genericType in types)
    		{
    			EnsureRoundtrip(genericType);
    		}
    	}

    	public void TestFunnyName()
    	{
    		EnsureRoundtrip(typeof(__Funny123Name_));
    	}
    	
        public void TestSimpleName()
        {
			TypeReference stringName = TypeReference.FromString("System.String");
			Assert.AreEqual("System.String", stringName.SimpleName);
			Assert.IsTrue(stringName.AssemblyName == null);
            Assert.AreEqual(typeof(string), stringName.Resolve());
        }

		public void TestVoidPointer()
        {
			TypeReference voidPointer = TypeReference.FromString("System.Void*");
			Assert.AreEqual("System.Void", voidPointer.SimpleName);
			Assert.IsTrue(voidPointer is PointerTypeReference);
            Assert.AreEqual(Type.GetType("System.Void*", true), voidPointer.Resolve());
        }

        public void TestNestedType()
        {
			TypeReference typeName = TypeReference.FromType(typeof(NestedType));
			Assert.AreEqual("Db4objects.Db4o.Tests.SharpenLang.TypeReferenceTestCase+NestedType", typeName.SimpleName);
			Assert.AreEqual(typeof(NestedType), typeName.Resolve());
        }

		public void TestWrongVersion()
		{
			TypeReference stringName = TypeReference.FromString("System.String, mscorlib, Version=1.14.27.0");
			Assert.AreEqual(typeof(string), stringName.Resolve());
		}

		public void TestAssemblyNameWithSpaces()
		{
			TypeReference typeReference =
				TypeReference.FromString("Foo, Business Objects, Version=1.0.0.0, Culture=neutral, PublicKeyToken=null");
			Assert.AreEqual("Foo", typeReference.SimpleName);
			Assert.AreEqual("Business Objects", typeReference.AssemblyName.Name);
		}

        public void TestAssemblyQualifiedName()
        {
			string assemblyNameString = "mscorlib, Version=2.0.0.0, Culture=neutral, PublicKeyToken=969db8053d3322ac";
			TypeReference typeReference =
				TypeReference.FromString(
					"System.String, " + assemblyNameString);
			Assert.AreEqual("System.String", typeReference.SimpleName);
			
			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Name = "mscorlib";
			assemblyName.Version = new Version(2, 0, 0, 0);
			assemblyName.CultureInfo = CultureInfo.InvariantCulture;
			assemblyName.SetPublicKeyToken(ParsePublicKeyToken("969db8053d3322ac"));
			Assert.AreEqual(assemblyName.FullName, typeReference.AssemblyName.FullName, "string.Assembly.FullName");
        }

		public void TestNumberAssemblyQualifiedName()
		{
			string assemblyNameString = "4ofus, Version=1.2.3.4, Culture=neutral";
			TypeReference typeReference =TypeReference.FromString("ForOfUs.Foo, " + assemblyNameString);

			Assert.AreEqual("ForOfUs.Foo", typeReference.SimpleName);

			AssemblyName assemblyName = new AssemblyName();
			assemblyName.Name = "4ofus";
			assemblyName.Version = new Version(1, 2, 3, 4);
			assemblyName.CultureInfo = CultureInfo.InvariantCulture;
			Assert.AreEqual(assemblyName.FullName, typeReference.AssemblyName.FullName, "string.Assembly.FullName");
		}

		public void TestWeirdAssemblyQualifiedName()
		{
			var weirdAssemblyNames = new[]
			                         	{
			                         		"4starting-with-number",
			                         		"{starting-with-open-brace",
			                         		"}starting-with-close-brace",
			                         		"1starting-with-number",
			                         		"`starting-with-apostrophe1",
			                         		"´starting-with-apostrophe2",
			                         		"'starting-with-single-quotation-mark",
			                         		"^starting-with-caret",
			                         		//"with-comma-in-the\\,middle", // Not supported yet
			                         		//"\\,starting-with-comma", // Not supported yet
			                         	};

			foreach (var simpleName in weirdAssemblyNames)
			{
				string assemblyNameString = simpleName + ", Version=1.2.3.4, Culture=neutral";
				TypeReference typeReference = TypeReference.FromString("Namespace.TypeName, " + assemblyNameString);

				Assert.AreEqual("Namespace.TypeName", typeReference.SimpleName);

				AssemblyName assemblyName = new AssemblyName();
				assemblyName.Name = simpleName;
				assemblyName.Version = new Version(1, 2, 3, 4);
				assemblyName.CultureInfo = CultureInfo.InvariantCulture;
				
				Assert.AreEqual(assemblyName.FullName, typeReference.AssemblyName.FullName, simpleName);
			}
		}
    	
		static byte[] ParsePublicKeyToken(string token)
		{
			int len = token.Length / 2;
			byte[] bytes = new byte[len];
			for (int i = 0; i < len; ++i)
			{
				bytes[i] = byte.Parse(token.Substring(i * 2, 2), NumberStyles.HexNumber);
			}
			return bytes;
		}

        public void TestSimpleArray()
        {
            EnsureRoundtrip(typeof(byte[]));
        }

        private static void EnsureRoundtrip(Type type)
        {
            TypeReference typeName = TypeReference.FromType(type);
            Assert.AreEqual(type, typeName.Resolve(), type.FullName);
        }
        
        public void TestJagged2DArray() 
        {
            EnsureRoundtrip(typeof(byte[][]));
        }

#if MONO
        public void _TestJaggedXDArray() { 
#else
		public void TestJaggedXDArray() 
        {
#endif
            EnsureRoundtrip(typeof(byte[][][,]));
        }

        class NestedGeneric<Key, Value>
        {
        }

#if CF
		public void _TestDeepGenericTypeName()
#else
        public void TestDeepGenericTypeName()
#endif
        {
            EnsureRoundtrip(typeof(Dictionary<string, List<string>>));
            EnsureRoundtrip(typeof(Dictionary<string, List<List<string>>>));

            EnsureRoundtrip(typeof(Dictionary<string, List<List<NestedType>>>));
            EnsureRoundtrip(typeof(NestedGeneric<string, List<string>[]>));
            EnsureRoundtrip(typeof(NestedGeneric<string, List<string>>[]));

            EnsureRoundtrip(typeof(GenericType<string, List<string>>.NestedInGeneric));
        }

        public void TestGenericArrays()
        {
            EnsureRoundtrip(typeof(SimpleGenericType<string>));
			EnsureRoundtrip(typeof(SimpleGenericType<int>[]));
            EnsureRoundtrip(typeof(SimpleGenericType<int>[,]));
            EnsureRoundtrip(typeof(SimpleGenericType<int>[][]));
#if !MONO
            EnsureRoundtrip(typeof(SimpleGenericType<int>[][,,]));
#endif
        }

        public void TestGenericOfArrays()
        {
            EnsureRoundtrip(typeof(SimpleGenericType<string[]>));
            EnsureRoundtrip(typeof(SimpleGenericType<string[]>[]));
#if !MONO
            EnsureRoundtrip(typeof(SimpleGenericType<string[,]>[][]));
#endif
            EnsureRoundtrip(typeof(SimpleGenericType<string[][]>[]));
            EnsureRoundtrip(typeof(SimpleGenericType<string[][]>[][]));
#if !MONO
            EnsureRoundtrip(typeof(SimpleGenericType<SimpleGenericType<string[][]>[][,]>[][]));
#endif
        }

        public void TestUnversionedGenericName()
        {
			string simpleAssemblyName = GetExecutingAssemblySimpleName();
			Type t = typeof(GenericType<int, GenericType<int, string>>);
			TypeReference tn = TypeReference.FromString(t.AssemblyQualifiedName);
			Assert.AreEqual(
				"Db4objects.Db4o.Tests.SharpenLang.GenericType`2[[System.Int32, mscorlib], [Db4objects.Db4o.Tests.SharpenLang.GenericType`2[[System.Int32, mscorlib], [System.String, mscorlib]], " + simpleAssemblyName +"]], " + simpleAssemblyName,
				tn.GetUnversionedName());
        }

        public void TestGenericName()
        {
            GenericType<int, string> o = new GenericType<int, string>(3, "42");
            Type t = Type.GetType(o.GetType().FullName);

            TypeReference stringName = TypeReference.FromString(typeof(string).AssemblyQualifiedName);

            GenericTypeReference genericTypeName = (GenericTypeReference)TypeReference.FromString(t.AssemblyQualifiedName);
            Assert.AreEqual("Db4objects.Db4o.Tests.SharpenLang.GenericType`2", genericTypeName.SimpleName);
            Assert.AreEqual(2, genericTypeName.GenericArguments.Length);

            Assert.AreEqual(TypeReference.FromString(typeof(int).AssemblyQualifiedName), genericTypeName.GenericArguments[0]);
            Assert.AreEqual(stringName, genericTypeName.GenericArguments[1]);

            Type complexType = typeof(GenericType<string, GenericType<int, string>>);
            GenericTypeReference complexTypeName = (GenericTypeReference) TypeReference.FromString(complexType.AssemblyQualifiedName);
            Assert.AreEqual(genericTypeName.SimpleName, complexTypeName.SimpleName);
            Assert.AreEqual(genericTypeName.AssemblyName.FullName, complexTypeName.AssemblyName.FullName);
            Assert.AreEqual(2, complexTypeName.GenericArguments.Length);
            Assert.AreEqual(stringName, complexTypeName.GenericArguments[0]);
            Assert.AreEqual(genericTypeName, complexTypeName.GenericArguments[1]);

            Assert.AreEqual(typeof(string), TypeReference.FromString("System.String, mscorlib").Resolve());
            Assert.AreEqual(t, TypeReference.FromString("Db4objects.Db4o.Tests.SharpenLang.GenericType`2[[System.Int32, mscorlib],[System.String, mscorlib]], " + GetExecutingAssemblySimpleName()).Resolve());
        }

    	private static string GetExecutingAssemblySimpleName()
    	{
    		return Assembly.GetExecutingAssembly().GetName().Name;
    	}
    }
}