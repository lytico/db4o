/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4objects.Db4o.Instrumentation.Api;

namespace Db4objects.Db4o.Tests.NativeQueries.Mocks
{
	public class MockTypeRef : ITypeRef
	{
		private readonly Type _type;

		public MockTypeRef(Type type)
		{
			_type = type;
		}

		public virtual ITypeRef ElementType
		{
			get
			{
				throw new NotImplementedException();
			}
		}

		public virtual bool IsPrimitive
		{
			get
			{
				return _type.IsPrimitive;
			}
		}

		public virtual string Name
		{
			get
			{
				return _type.FullName;
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is ITypeRef))
			{
				return false;
			}
			ITypeRef other = (ITypeRef)obj;
			return IsPrimitive == other.IsPrimitive && Name.Equals(other.Name);
		}

		public override int GetHashCode()
		{
			return _type.GetHashCode();
		}
	}
}
#endif // !SILVERLIGHT
