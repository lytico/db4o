/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Tests.Common.Reflect.Custom;

namespace Db4objects.Db4o.Tests.Common.Reflect.Custom
{
	public class CustomUidField : IReflectField
	{
		public CustomClassRepository _repository;

		public CustomUidField()
		{
		}

		public CustomUidField(CustomClassRepository repository)
		{
			_repository = repository;
		}

		public virtual object Get(object onObject)
		{
			return Entry(onObject).uid;
		}

		private PersistentEntry Entry(object onObject)
		{
			return ((PersistentEntry)onObject);
		}

		public virtual IReflectClass GetFieldType()
		{
			return _repository.ForFieldType(typeof(object));
		}

		public virtual string GetName()
		{
			return "uid";
		}

		public virtual object IndexEntry(object orig)
		{
			throw new NotImplementedException();
		}

		public virtual IReflectClass IndexType()
		{
			throw new NotImplementedException();
		}

		public virtual bool IsPublic()
		{
			return true;
		}

		public virtual bool IsStatic()
		{
			return false;
		}

		public virtual bool IsTransient()
		{
			return false;
		}

		public virtual void Set(object onObject, object value)
		{
			Entry(onObject).uid = value;
		}

		public override string ToString()
		{
			return "CustomUidField()";
		}
	}
}
