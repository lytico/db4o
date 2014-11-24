/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.CS.Internal;

namespace Db4objects.Db4o.CS.Internal
{
	public class ClassInfo
	{
		public static Db4objects.Db4o.CS.Internal.ClassInfo NewSystemClass(string className
			)
		{
			return new Db4objects.Db4o.CS.Internal.ClassInfo(className, true);
		}

		public static Db4objects.Db4o.CS.Internal.ClassInfo NewUserClass(string className
			)
		{
			return new Db4objects.Db4o.CS.Internal.ClassInfo(className, false);
		}

		public string _className;

		public bool _isSystemClass;

		public Db4objects.Db4o.CS.Internal.ClassInfo _superClass;

		public FieldInfo[] _fields;

		public ClassInfo()
		{
		}

		private ClassInfo(string className, bool systemClass)
		{
			_className = className;
			_isSystemClass = systemClass;
		}

		public virtual FieldInfo[] GetFields()
		{
			return _fields;
		}

		public virtual void SetFields(FieldInfo[] fields)
		{
			this._fields = fields;
		}

		public virtual Db4objects.Db4o.CS.Internal.ClassInfo GetSuperClass()
		{
			return _superClass;
		}

		public virtual void SetSuperClass(Db4objects.Db4o.CS.Internal.ClassInfo superClass
			)
		{
			this._superClass = superClass;
		}

		public virtual string GetClassName()
		{
			return _className;
		}

		public virtual bool IsSystemClass()
		{
			return _isSystemClass;
		}
	}
}
