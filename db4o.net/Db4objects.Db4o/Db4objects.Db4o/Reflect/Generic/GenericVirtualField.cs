/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4objects.Db4o.Reflect.Generic
{
	/// <exclude></exclude>
	public class GenericVirtualField : GenericField
	{
		public GenericVirtualField(string name) : base(name, null, false)
		{
		}

		public override object DeepClone(object obj)
		{
			return new Db4objects.Db4o.Reflect.Generic.GenericVirtualField(GetName());
		}

		public override object Get(object onObject)
		{
			return null;
		}

		public override IReflectClass GetFieldType()
		{
			return null;
		}

		public override bool IsPublic()
		{
			return false;
		}

		public override bool IsStatic()
		{
			return true;
		}

		public override bool IsTransient()
		{
			return true;
		}

		public override void Set(object onObject, object value)
		{
		}
		// do nothing
	}
}
