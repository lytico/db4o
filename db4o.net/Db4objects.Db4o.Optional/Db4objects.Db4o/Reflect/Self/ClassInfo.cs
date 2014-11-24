/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Reflect.Self
{
	public class ClassInfo
	{
		private Type _superClass;

		private bool _isAbstract;

		private Db4objects.Db4o.Reflect.Self.FieldInfo[] _fieldInfo;

		public ClassInfo(bool isAbstract, Type superClass, Db4objects.Db4o.Reflect.Self.FieldInfo
			[] fieldInfo)
		{
			_isAbstract = isAbstract;
			_superClass = superClass;
			_fieldInfo = fieldInfo;
		}

		public virtual bool IsAbstract()
		{
			return _isAbstract;
		}

		public virtual Type SuperClass()
		{
			return _superClass;
		}

		public virtual Db4objects.Db4o.Reflect.Self.FieldInfo[] FieldInfo()
		{
			return _fieldInfo;
		}

		public virtual Db4objects.Db4o.Reflect.Self.FieldInfo FieldByName(string name)
		{
			if (!(_fieldInfo.Length == 0))
			{
				for (int i = 0; i < _fieldInfo.Length; i++)
				{
					if (_fieldInfo[i].Name().Equals(name))
					{
						return _fieldInfo[i];
					}
				}
			}
			return null;
		}
	}
}
