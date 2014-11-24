/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4oUnit;
using Db4oUnit.Extensions.Fixtures;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.TA;

namespace Db4objects.Db4o.Tests.Common.TA
{
	public abstract class ItemTestCaseBase : TransparentActivationTestCaseBase, IOptOutDefragSolo
	{
		private Type _clazz;

		protected long id;

		protected Db4oUUID uuid;

		/// <exception cref="System.Exception"></exception>
		protected override void Store()
		{
			object value = CreateItem();
			_clazz = value.GetType();
			Store(value);
			id = Db().Ext().GetID(value);
			uuid = Db().Ext().GetObjectInfo(value).GetUUID();
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestQuery()
		{
			object item = RetrieveOnlyInstance();
			AssertRetrievedItem(item);
			AssertItemValue(item);
		}

		/// <exception cref="System.Exception"></exception>
		public virtual void TestDeactivate()
		{
			object item = RetrieveOnlyInstance();
			Db().Deactivate(item, 1);
			AssertNullItem(item);
			Db().Activate(item, 42);
			Db().Deactivate(item, 1);
			AssertNullItem(item);
		}

		protected virtual object RetrieveOnlyInstance()
		{
			return RetrieveOnlyInstance(_clazz);
		}

		/// <exception cref="System.Exception"></exception>
		protected virtual void AssertNullItem(object obj)
		{
			IReflectClass claxx = Reflector().ForObject(obj);
			IReflectField[] fields = claxx.GetDeclaredFields();
			for (int i = 0; i < fields.Length; ++i)
			{
				IReflectField field = fields[i];
				if (field.IsStatic() || field.IsTransient())
				{
					continue;
				}
				IReflectClass type = field.GetFieldType();
				if (Container().ClassMetadataForReflectClass(type).IsValueType())
				{
					continue;
				}
				object value = field.Get(obj);
				Assert.IsNull(value);
			}
		}

		/// <exception cref="System.Exception"></exception>
		protected abstract void AssertItemValue(object obj);

		/// <exception cref="System.Exception"></exception>
		protected abstract object CreateItem();

		/// <exception cref="System.Exception"></exception>
		protected abstract void AssertRetrievedItem(object obj);
	}
}
