/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Marshall;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Types;

namespace Db4objects.Db4o.Internal.Query.Processor
{
	/// <exclude></exclude>
	public class QField : IUnversioned
	{
		[System.NonSerialized]
		internal Transaction i_trans;

		private string i_name;

		[System.NonSerialized]
		internal FieldMetadata _fieldMetadata;

		private int i_classMetadataID;

		private int _fieldHandle;

		public QField()
		{
		}

		public QField(Transaction a_trans, string name, FieldMetadata fieldMetadata, int 
			classMetadataID, int a_index)
		{
			// C/S only	
			i_trans = a_trans;
			i_name = name;
			_fieldMetadata = fieldMetadata;
			i_classMetadataID = classMetadataID;
			_fieldHandle = a_index;
			if (_fieldMetadata != null)
			{
				if (!_fieldMetadata.Alive())
				{
					_fieldMetadata = null;
				}
			}
		}

		public virtual string Name()
		{
			return i_name;
		}

		internal virtual object Coerce(object a_object)
		{
			IReflectClass claxx = null;
			if (a_object != null)
			{
				if (a_object is IReflectClass)
				{
					claxx = (IReflectClass)a_object;
				}
				else
				{
					claxx = i_trans.Reflector().ForObject(a_object);
				}
			}
			else
			{
				// TODO: Review this line for NullableArrayHandling 
				return a_object;
			}
			if (_fieldMetadata == null)
			{
				return a_object;
			}
			return _fieldMetadata.Coerce(claxx, a_object);
		}

		internal virtual ClassMetadata GetFieldType()
		{
			if (_fieldMetadata != null)
			{
				return _fieldMetadata.FieldType();
			}
			return null;
		}

		public virtual FieldMetadata GetFieldMetadata()
		{
			return _fieldMetadata;
		}

		internal virtual bool IsArray()
		{
			return _fieldMetadata != null && Handlers4.HandlesArray(_fieldMetadata.GetHandler
				());
		}

		internal virtual bool IsClass()
		{
			return _fieldMetadata == null || Handlers4.HandlesClass(_fieldMetadata.GetHandler
				());
		}

		internal virtual bool IsQueryLeaf()
		{
			return _fieldMetadata != null && Handlers4.IsQueryLeaf(_fieldMetadata.GetHandler(
				));
		}

		internal virtual IPreparedComparison PrepareComparison(IContext context, object obj
			)
		{
			if (_fieldMetadata != null)
			{
				return _fieldMetadata.PrepareComparison(context, obj);
			}
			if (obj == null)
			{
				return Null.Instance;
			}
			ClassMetadata yc = i_trans.Container().ProduceClassMetadata(i_trans.Reflector().ForObject
				(obj));
			FieldMetadata yf = yc.FieldMetadataForName(Name());
			if (yf != null)
			{
				return yf.PrepareComparison(context, obj);
			}
			return null;
		}

		internal virtual void Unmarshall(Transaction a_trans)
		{
			if (i_classMetadataID != 0)
			{
				ClassMetadata yc = a_trans.Container().ClassMetadataForID(i_classMetadataID);
				_fieldMetadata = (FieldMetadata)yc._aspects[_fieldHandle];
			}
		}

		public override string ToString()
		{
			if (_fieldMetadata != null)
			{
				return "QField " + _fieldMetadata.ToString();
			}
			return base.ToString();
		}
	}
}
