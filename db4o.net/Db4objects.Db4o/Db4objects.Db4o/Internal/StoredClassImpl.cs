/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal
{
	/// <exclude></exclude>
	public class StoredClassImpl : IStoredClass
	{
		private readonly Transaction _transaction;

		private readonly ClassMetadata _classMetadata;

		public StoredClassImpl(Transaction transaction, ClassMetadata classMetadata)
		{
			if (classMetadata == null)
			{
				throw new ArgumentException();
			}
			_transaction = transaction;
			_classMetadata = classMetadata;
		}

		public virtual long[] GetIDs()
		{
			return _classMetadata.GetIDs(_transaction);
		}

		public virtual string GetName()
		{
			return _classMetadata.GetName();
		}

		public virtual IStoredClass GetParentStoredClass()
		{
			ClassMetadata parentClassMetadata = _classMetadata.GetAncestor();
			if (parentClassMetadata == null)
			{
				return null;
			}
			return new Db4objects.Db4o.Internal.StoredClassImpl(_transaction, parentClassMetadata
				);
		}

		public virtual IStoredField[] GetStoredFields()
		{
			IStoredField[] fieldMetadata = _classMetadata.GetStoredFields();
			IStoredField[] storedFields = new IStoredField[fieldMetadata.Length];
			for (int i = 0; i < fieldMetadata.Length; i++)
			{
				storedFields[i] = new StoredFieldImpl(_transaction, (FieldMetadata)fieldMetadata[
					i]);
			}
			return storedFields;
		}

		public virtual bool HasClassIndex()
		{
			return _classMetadata.HasClassIndex();
		}

		public virtual void Rename(string newName)
		{
			IInternalObjectContainer container = (IInternalObjectContainer)_transaction.ObjectContainer
				();
			container.SyncExec(new _IClosure4_56(this, newName));
		}

		private sealed class _IClosure4_56 : IClosure4
		{
			public _IClosure4_56(StoredClassImpl _enclosing, string newName)
			{
				this._enclosing = _enclosing;
				this.newName = newName;
			}

			public object Run()
			{
				this._enclosing._classMetadata.Rename(newName);
				return null;
			}

			private readonly StoredClassImpl _enclosing;

			private readonly string newName;
		}

		public virtual IStoredField StoredField(string name, object type)
		{
			FieldMetadata fieldMetadata = (FieldMetadata)_classMetadata.StoredField(name, type
				);
			if (fieldMetadata == null)
			{
				return null;
			}
			return new StoredFieldImpl(_transaction, fieldMetadata);
		}

		public override int GetHashCode()
		{
			return _classMetadata.GetHashCode();
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
			return _classMetadata.Equals(((Db4objects.Db4o.Internal.StoredClassImpl)obj)._classMetadata
				);
		}

		public override string ToString()
		{
			return "StoredClass(" + _classMetadata + ")";
		}

		public virtual int InstanceCount()
		{
			return _classMetadata.InstanceCount(_transaction);
		}
	}
}
