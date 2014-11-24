/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Btree;

namespace Db4oUnit.Extensions
{
	public class FieldIndexAssert
	{
		private readonly Type _clazz;

		private readonly string _name;

		public FieldIndexAssert(Type clazz, string name)
		{
			_clazz = clazz;
			_name = name;
		}

		public virtual void AssertSingleEntry(LocalObjectContainer container, long id)
		{
			BooleanByRef called = new BooleanByRef();
			Index(container).TraverseKeys(container.SystemTransaction(), new _IVisitor4_24(id
				, called));
			Assert.IsTrue(called.value);
		}

		private sealed class _IVisitor4_24 : IVisitor4
		{
			public _IVisitor4_24(long id, BooleanByRef called)
			{
				this.id = id;
				this.called = called;
			}

			public void Visit(object key)
			{
				Assert.AreEqual(id, ((IFieldIndexKey)key).ParentID());
				Assert.IsFalse(called.value);
				called.value = true;
			}

			private readonly long id;

			private readonly BooleanByRef called;
		}

		private BTree Index(LocalObjectContainer container)
		{
			return FieldMetadata(container).GetIndex(null);
		}

		private Db4objects.Db4o.Internal.FieldMetadata FieldMetadata(LocalObjectContainer
			 container)
		{
			return ClassMetadata(container).FieldMetadataForName(_name);
		}

		private Db4objects.Db4o.Internal.ClassMetadata ClassMetadata(LocalObjectContainer
			 container)
		{
			return container.ClassMetadataForReflectClass(container.Reflector().ForClass(_clazz
				));
		}
	}
}
