/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using System.Reflection;
using Db4oUnit;
using Db4objects.Db4o.Instrumentation.Api;
using Db4objects.Db4o.Tests.NativeQueries.Mocks;

namespace Db4objects.Db4o.Tests.NativeQueries.Mocks
{
	public class MockMethodRef : IMethodRef
	{
		private readonly MethodInfo _method;

		public MockMethodRef(MethodInfo method)
		{
			_method = method;
		}

		public virtual string Name
		{
			get
			{
				return _method.Name;
			}
		}

		public virtual ITypeRef[] ParamTypes
		{
			get
			{
				Type[] paramTypes = Sharpen.Runtime.GetParameterTypes(_method);
				ITypeRef[] types = new ITypeRef[paramTypes.Length];
				for (int i = 0; i < paramTypes.Length; ++i)
				{
					types[i] = TypeRef(paramTypes[i]);
				}
				return types;
			}
		}

		public virtual ITypeRef DeclaringType
		{
			get
			{
				return TypeRef(_method.DeclaringType);
			}
		}

		private ITypeRef TypeRef(Type type)
		{
			return new MockTypeRef(type);
		}

		public virtual ITypeRef ReturnType
		{
			get
			{
				return TypeRef(_method.ReturnType);
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is IMethodRef))
			{
				return false;
			}
			IMethodRef other = (IMethodRef)obj;
			return Name.Equals(other.Name) && Check.ObjectsAreEqual(DeclaringType, other.DeclaringType
				) && Check.ObjectsAreEqual(ReturnType, other.ReturnType) && Check.ArraysAreEqual
				(ParamTypes, other.ParamTypes);
		}
	}
}
#endif // !SILVERLIGHT
