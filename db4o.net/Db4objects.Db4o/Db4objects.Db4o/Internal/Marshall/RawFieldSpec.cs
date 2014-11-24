/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal.Marshall;

namespace Db4objects.Db4o.Internal.Marshall
{
	public class RawFieldSpec
	{
		private readonly AspectType _type;

		private readonly string _name;

		private readonly int _fieldTypeID;

		private readonly bool _isPrimitive;

		private readonly bool _isArray;

		private readonly bool _isNArray;

		private readonly bool _isVirtual;

		private int _indexID;

		public RawFieldSpec(AspectType aspectType, string name, int fieldTypeID, byte attribs
			)
		{
			_type = aspectType;
			_name = name;
			_fieldTypeID = fieldTypeID;
			BitMap4 bitmap = new BitMap4(attribs);
			_isPrimitive = bitmap.IsTrue(0);
			_isArray = bitmap.IsTrue(1);
			_isNArray = bitmap.IsTrue(2);
			_isVirtual = false;
			_indexID = 0;
		}

		public RawFieldSpec(AspectType aspectType, string name)
		{
			_type = aspectType;
			_name = name;
			_fieldTypeID = 0;
			_isPrimitive = false;
			_isArray = false;
			_isNArray = false;
			_isVirtual = true;
			_indexID = 0;
		}

		public virtual string Name()
		{
			return _name;
		}

		public virtual int FieldTypeID()
		{
			return _fieldTypeID;
		}

		public virtual bool IsPrimitive()
		{
			return _isPrimitive;
		}

		public virtual bool IsArray()
		{
			return _isArray;
		}

		public virtual bool IsNArray()
		{
			return _isNArray;
		}

		public virtual bool IsVirtual()
		{
			return _isVirtual;
		}

		public virtual bool IsVirtualField()
		{
			return IsVirtual() && IsField();
		}

		public virtual bool IsField()
		{
			return _type.IsField();
		}

		public virtual int IndexID()
		{
			return _indexID;
		}

		internal virtual void IndexID(int indexID)
		{
			_indexID = indexID;
		}

		public override string ToString()
		{
			return "RawFieldSpec(" + Name() + ")";
		}

		public virtual bool IsFieldMetadata()
		{
			return _type.IsFieldMetadata();
		}

		public virtual bool IsTranslator()
		{
			return _type.IsTranslator();
		}
	}
}
