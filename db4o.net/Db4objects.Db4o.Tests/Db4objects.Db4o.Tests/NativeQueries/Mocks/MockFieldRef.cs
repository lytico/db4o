/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

#if !SILVERLIGHT
using System;
using Db4objects.Db4o.Instrumentation.Api;
using Db4objects.Db4o.Tests.NativeQueries.Mocks;

namespace Db4objects.Db4o.Tests.NativeQueries.Mocks
{
	public class MockFieldRef : IFieldRef
	{
		private readonly string _name;

		private readonly ITypeRef _type;

		public MockFieldRef(string name) : this(name, new MockTypeRef(typeof(object)))
		{
		}

		public MockFieldRef(string name, ITypeRef typeRef)
		{
			if (null == name)
			{
				throw new ArgumentNullException();
			}
			if (null == typeRef)
			{
				throw new ArgumentNullException();
			}
			_name = name;
			_type = typeRef;
		}

		public virtual string Name
		{
			get
			{
				return _name;
			}
		}

		public virtual ITypeRef Type
		{
			get
			{
				return _type;
			}
		}

		public override string ToString()
		{
			return Name;
		}

		public override bool Equals(object obj)
		{
			if (!(obj is IFieldRef))
			{
				return false;
			}
			IFieldRef other = (IFieldRef)obj;
			return _name.Equals(other.Name) && _type.Equals(other.Type);
		}

		public override int GetHashCode()
		{
			return _name.GetHashCode() + 29 * _type.GetHashCode();
		}
	}
}
#endif // !SILVERLIGHT
