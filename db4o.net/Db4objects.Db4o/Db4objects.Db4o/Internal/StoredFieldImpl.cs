/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class StoredFieldImpl : IStoredField
	{
		private readonly Transaction _transaction;

		private readonly Db4objects.Db4o.Internal.FieldMetadata _fieldMetadata;

		public StoredFieldImpl(Transaction transaction, Db4objects.Db4o.Internal.FieldMetadata
			 fieldMetadata)
		{
			_transaction = transaction;
			_fieldMetadata = fieldMetadata;
		}

		public virtual void CreateIndex()
		{
			lock (Lock())
			{
				_fieldMetadata.CreateIndex();
			}
		}

		public virtual void DropIndex()
		{
			lock (Lock())
			{
				_fieldMetadata.DropIndex();
			}
		}

		private object Lock()
		{
			return _transaction.Container().Lock();
		}

		public virtual Db4objects.Db4o.Internal.FieldMetadata FieldMetadata()
		{
			return _fieldMetadata;
		}

		public virtual object Get(object onObject)
		{
			return _fieldMetadata.Get(_transaction, onObject);
		}

		public virtual string GetName()
		{
			return _fieldMetadata.GetName();
		}

		public virtual IReflectClass GetStoredType()
		{
			return _fieldMetadata.GetStoredType();
		}

		public virtual bool HasIndex()
		{
			return _fieldMetadata.HasIndex();
		}

		public virtual bool IsArray()
		{
			return _fieldMetadata.IsArray();
		}

		public virtual void Rename(string name)
		{
			lock (Lock())
			{
				_fieldMetadata.Rename(name);
			}
		}

		public virtual void TraverseValues(IVisitor4 visitor)
		{
			_fieldMetadata.TraverseValues(_transaction, visitor);
		}

		public override int GetHashCode()
		{
			return _fieldMetadata.GetHashCode();
		}

		public override bool Equals(object obj)
		{
			if (obj == null)
			{
				return false;
			}
			if (GetType() != obj.GetType())
			{
				return false;
			}
			return _fieldMetadata.Equals(((Db4objects.Db4o.Internal.StoredFieldImpl)obj)._fieldMetadata
				);
		}
	}
}
