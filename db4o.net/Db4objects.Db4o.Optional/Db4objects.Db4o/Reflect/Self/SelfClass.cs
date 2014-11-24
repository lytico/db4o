/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Self;

namespace Db4objects.Db4o.Reflect.Self
{
	public class SelfClass : IReflectClass
	{
		private static readonly SelfField[] EmptyFields = new SelfField[0];

		private bool _isAbstract;

		private SelfField[] _fields;

		private IReflector _parentReflector;

		private SelfReflectionRegistry _registry;

		private Type _class;

		private Type _superClass;

		public SelfClass(IReflector parentReflector, SelfReflectionRegistry registry, Type
			 clazz)
		{
			// public SelfClass() {
			// super();
			// }
			_parentReflector = parentReflector;
			_registry = registry;
			_class = clazz;
		}

		// TODO: Is this needed at all?
		public virtual Type GetJavaClass()
		{
			return _class;
		}

		public virtual IReflector Reflector()
		{
			return _parentReflector;
		}

		public virtual IReflectClass GetComponentType()
		{
			if (!IsArray())
			{
				return null;
			}
			return _parentReflector.ForClass(_registry.ComponentType(_class));
		}

		public virtual IReflectField[] GetDeclaredFields()
		{
			EnsureClassInfoLoaded();
			return _fields;
		}

		private void EnsureClassInfoLoaded()
		{
			if (_fields == null)
			{
				ClassInfo classInfo = _registry.InfoFor(_class);
				if (classInfo == null)
				{
					_fields = EmptyFields;
					return;
				}
				_superClass = classInfo.SuperClass();
				_isAbstract = classInfo.IsAbstract();
				FieldInfo[] fieldInfo = classInfo.FieldInfo();
				if (fieldInfo == null)
				{
					_fields = EmptyFields;
					return;
				}
				_fields = new SelfField[fieldInfo.Length];
				for (int idx = 0; idx < fieldInfo.Length; idx++)
				{
					_fields[idx] = SelfFieldFor(fieldInfo[idx]);
				}
			}
		}

		public virtual IReflectField GetDeclaredField(string name)
		{
			EnsureClassInfoLoaded();
			for (int idx = 0; idx < _fields.Length; idx++)
			{
				if (_fields[idx].GetName().Equals(name))
				{
					return _fields[idx];
				}
			}
			return null;
		}

		private SelfField SelfFieldFor(FieldInfo fieldInfo)
		{
			return new SelfField(fieldInfo.Name(), _parentReflector.ForClass(fieldInfo.Type()
				), this, _registry);
		}

		public virtual IReflectClass GetDelegate()
		{
			return this;
		}

		public virtual IReflectMethod GetMethod(string methodName, IReflectClass[] paramClasses
			)
		{
			// TODO !!!!
			return null;
		}

		public virtual string GetName()
		{
			return _class.FullName;
		}

		public virtual IReflectClass GetSuperclass()
		{
			EnsureClassInfoLoaded();
			if (_superClass == null)
			{
				return null;
			}
			return _parentReflector.ForClass(_superClass);
		}

		public virtual bool IsAbstract()
		{
			EnsureClassInfoLoaded();
			return _isAbstract || IsInterface();
		}

		public virtual bool IsArray()
		{
			return _class.IsArray;
		}

		public virtual bool IsAssignableFrom(IReflectClass type)
		{
			if (!(type is Db4objects.Db4o.Reflect.Self.SelfClass))
			{
				return false;
			}
			return _class.IsAssignableFrom(((Db4objects.Db4o.Reflect.Self.SelfClass)type).GetJavaClass
				());
		}

		public virtual bool IsCollection()
		{
			return _parentReflector.IsCollection(this);
		}

		public virtual bool IsInstance(object obj)
		{
			return _class.IsInstanceOfType(obj);
		}

		public virtual bool IsInterface()
		{
			return _class.IsInterface;
		}

		public virtual bool IsPrimitive()
		{
			return _registry.IsPrimitive(_class);
		}

		public virtual object NewInstance()
		{
			try
			{
				return System.Activator.CreateInstance(_class);
			}
			catch (Exception e)
			{
				Sharpen.Runtime.PrintStackTrace(e);
			}
			// Specialized exceptions break conversion to .NET
			//           
			//        
			//            
			// } catch (InstantiationException e) {
			// e.printStackTrace();
			// } catch (IllegalAccessException e) {
			// e.printStackTrace();
			// }
			return null;
		}

		public virtual object NullValue()
		{
			return null;
		}

		public virtual bool EnsureCanBeInstantiated()
		{
			return true;
		}

		public virtual bool IsSimple()
		{
			return IsPrimitive() || Platform4.IsSimple(_class);
		}
	}
}
