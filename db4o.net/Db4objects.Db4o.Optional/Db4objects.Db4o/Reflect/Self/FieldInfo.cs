/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

namespace Db4objects.Db4o.Reflect.Self
{
	public class FieldInfo
	{
		private string _name;

		private System.Type _clazz;

		private bool _isPublic;

		private bool _isStatic;

		private bool _isTransient;

		public FieldInfo(string name, System.Type clazz, bool isPublic, bool isStatic, bool
			 isTransient)
		{
			_name = name;
			_clazz = clazz;
			_isPublic = isPublic;
			_isStatic = isStatic;
			_isTransient = isTransient;
		}

		public virtual string Name()
		{
			return _name;
		}

		public virtual System.Type Type()
		{
			return _clazz;
		}

		public virtual bool IsPublic()
		{
			return _isPublic;
		}

		public virtual bool IsStatic()
		{
			return _isStatic;
		}

		public virtual bool IsTransient()
		{
			return _isTransient;
		}
	}
}
