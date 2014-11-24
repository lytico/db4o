/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Reflect
{
	/// <exclude></exclude>
	public class ArrayInfo
	{
		private int _elementCount;

		private bool _primitive;

		private bool _nullable;

		private IReflectClass _reflectClass;

		public virtual int ElementCount()
		{
			return _elementCount;
		}

		public virtual void ElementCount(int count)
		{
			_elementCount = count;
		}

		public virtual bool Primitive()
		{
			return _primitive;
		}

		public virtual void Primitive(bool flag)
		{
			_primitive = flag;
		}

		public virtual bool Nullable()
		{
			return _nullable;
		}

		public virtual void Nullable(bool flag)
		{
			_nullable = flag;
		}

		public virtual IReflectClass ReflectClass()
		{
			return _reflectClass;
		}

		public virtual void ReflectClass(IReflectClass claxx)
		{
			_reflectClass = claxx;
		}
	}
}
