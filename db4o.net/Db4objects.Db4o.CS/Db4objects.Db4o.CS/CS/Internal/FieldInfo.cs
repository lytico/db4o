/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal;

namespace Db4objects.Db4o.CS.Internal
{
	public class FieldInfo
	{
		public string _fieldName;

		public ClassInfo _fieldClass;

		public bool _isPrimitive;

		public bool _isArray;

		public bool _isNArray;

		public FieldInfo()
		{
		}

		public FieldInfo(string fieldName, ClassInfo fieldClass, bool isPrimitive, bool isArray
			, bool isNArray)
		{
			_fieldName = fieldName;
			_fieldClass = fieldClass;
			_isPrimitive = isPrimitive;
			_isArray = isArray;
			_isNArray = isNArray;
		}

		public virtual ClassInfo GetFieldClass()
		{
			return _fieldClass;
		}

		public virtual string GetFieldName()
		{
			return _fieldName;
		}
	}
}
