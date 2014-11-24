/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using System.Collections;
using Db4objects.Db4o;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Core;

namespace Db4objects.Db4o.Reflect.Core
{
	public class ConstructorSupport
	{
		public static ReflectConstructorSpec CreateConstructor(IConstructorAwareReflectClass
			 claxx, Type clazz, IReflectorConfiguration config, IReflectConstructor[] constructors
			)
		{
			if (claxx == null)
			{
				return ReflectConstructorSpec.InvalidConstructor;
			}
			if (claxx.IsAbstract() || claxx.IsInterface())
			{
				return ReflectConstructorSpec.InvalidConstructor;
			}
			if (!Platform4.CallConstructor())
			{
				bool skipConstructor = !config.CallConstructor(claxx);
				if (!claxx.IsCollection())
				{
					IReflectConstructor serializableConstructor = SkipConstructor(claxx, skipConstructor
						, config.TestConstructors());
					if (serializableConstructor != null)
					{
						return new ReflectConstructorSpec(serializableConstructor, null);
					}
				}
			}
			if (!config.TestConstructors())
			{
				return new ReflectConstructorSpec(new PlatformReflectConstructor(clazz), null);
			}
			if (ReflectPlatform.CreateInstance(clazz) != null)
			{
				return new ReflectConstructorSpec(new PlatformReflectConstructor(clazz), null);
			}
			Tree sortedConstructors = SortConstructorsByParamsCount(constructors);
			return FindConstructor(claxx, sortedConstructors);
		}

		private static ReflectConstructorSpec FindConstructor(IReflectClass claxx, Tree sortedConstructors
			)
		{
			if (sortedConstructors == null)
			{
				return ReflectConstructorSpec.InvalidConstructor;
			}
			IEnumerator iter = new TreeNodeIterator(sortedConstructors);
			while (iter.MoveNext())
			{
				object current = iter.Current;
				IReflectConstructor constructor = (IReflectConstructor)((TreeIntObject)current)._object;
				object[] args = NullArgumentsFor(constructor);
				object res = constructor.NewInstance(args);
				if (res != null)
				{
					return new ReflectConstructorSpec(constructor, args);
				}
			}
			return ReflectConstructorSpec.InvalidConstructor;
		}

		private static object[] NullArgumentsFor(IReflectConstructor constructor)
		{
			IReflectClass[] paramTypes = constructor.GetParameterTypes();
			object[] @params = new object[paramTypes.Length];
			for (int j = 0; j < @params.Length; j++)
			{
				@params[j] = paramTypes[j].NullValue();
			}
			return @params;
		}

		private static Tree SortConstructorsByParamsCount(IReflectConstructor[] constructors
			)
		{
			Tree sortedConstructors = null;
			// sort constructors by parameter count
			for (int i = 0; i < constructors.Length; i++)
			{
				int parameterCount = constructors[i].GetParameterTypes().Length;
				sortedConstructors = Tree.Add(sortedConstructors, new TreeIntObject(i + constructors
					.Length * parameterCount, constructors[i]));
			}
			return sortedConstructors;
		}

		public static IReflectConstructor SkipConstructor(IConstructorAwareReflectClass claxx
			, bool skipConstructor, bool testConstructor)
		{
			if (!skipConstructor)
			{
				return null;
			}
			IReflectConstructor serializableConstructor = claxx.GetSerializableConstructor();
			if (serializableConstructor == null)
			{
				return null;
			}
			if (!testConstructor || Deploy.csharp)
			{
				return serializableConstructor;
			}
			object obj = serializableConstructor.NewInstance((object[])null);
			if (obj != null)
			{
				return serializableConstructor;
			}
			return null;
		}
	}
}
