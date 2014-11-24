/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Reflect;
using Db4objects.Db4o.Reflect.Generic;

namespace Db4objects.Db4o.Reflect.Generic
{
	/// <exclude></exclude>
	public class GenericClass : IReflectClass, IDeepClone
	{
		private static readonly GenericField[] NoFields = new GenericField[0];

		private readonly GenericReflector _reflector;

		private readonly IReflectClass _delegate;

		private readonly string _name;

		private Db4objects.Db4o.Reflect.Generic.GenericClass _superclass;

		private Db4objects.Db4o.Reflect.Generic.GenericClass _array;

		private bool _isPrimitive;

		private int _isCollection;

		protected IGenericConverter _converter;

		private GenericField[] _fields = NoFields;

		private int _declaredFieldCount = -1;

		private int _fieldCount = -1;

		private readonly int _hashCode;

		public GenericClass(GenericReflector reflector, IReflectClass delegateClass, string
			 name, Db4objects.Db4o.Reflect.Generic.GenericClass superclass)
		{
			_reflector = reflector;
			_delegate = delegateClass;
			_name = name;
			_superclass = superclass;
			_hashCode = _name.GetHashCode();
		}

		public virtual Db4objects.Db4o.Reflect.Generic.GenericClass ArrayClass()
		{
			if (_array != null)
			{
				return _array;
			}
			_array = new GenericArrayClass(_reflector, this, _name, _superclass);
			return _array;
		}

		public virtual object DeepClone(object obj)
		{
			GenericReflector reflector = (GenericReflector)obj;
			Db4objects.Db4o.Reflect.Generic.GenericClass superClass = null;
			if (_superclass != null)
			{
				_superclass = (Db4objects.Db4o.Reflect.Generic.GenericClass)reflector.ForName(_superclass
					.GetName());
			}
			Db4objects.Db4o.Reflect.Generic.GenericClass ret = new Db4objects.Db4o.Reflect.Generic.GenericClass
				(reflector, _delegate, _name, superClass);
			GenericField[] fields = new GenericField[_fields.Length];
			for (int i = 0; i < fields.Length; i++)
			{
				fields[i] = (GenericField)_fields[i].DeepClone(reflector);
			}
			ret.InitFields(fields);
			return ret;
		}

		public override bool Equals(object obj)
		{
			if (this == obj)
			{
				return true;
			}
			if (obj == null || GetType() != obj.GetType())
			{
				return false;
			}
			Db4objects.Db4o.Reflect.Generic.GenericClass otherGC = (Db4objects.Db4o.Reflect.Generic.GenericClass
				)obj;
			if (_hashCode != otherGC.GetHashCode())
			{
				return false;
			}
			return _name.Equals(otherGC._name);
		}

		public virtual IReflectClass GetComponentType()
		{
			if (_delegate != null)
			{
				return _delegate.GetComponentType();
			}
			return null;
		}

		// TODO: consider that classes may have two fields of
		// the same name after refactoring.
		public virtual IReflectField GetDeclaredField(string name)
		{
			if (_delegate != null)
			{
				return _delegate.GetDeclaredField(name);
			}
			for (int i = 0; i < _fields.Length; i++)
			{
				if (_fields[i].GetName().Equals(name))
				{
					return _fields[i];
				}
			}
			return null;
		}

		public virtual IReflectField[] GetDeclaredFields()
		{
			if (_delegate != null)
			{
				return _delegate.GetDeclaredFields();
			}
			return _fields;
		}

		public virtual IReflectClass GetDelegate()
		{
			if (_delegate != null)
			{
				return _delegate;
			}
			return this;
		}

		internal virtual int GetFieldCount()
		{
			if (_fieldCount != -1)
			{
				return _fieldCount;
			}
			_fieldCount = 0;
			if (_superclass != null)
			{
				_fieldCount = _superclass.GetFieldCount();
			}
			if (_declaredFieldCount == -1)
			{
				_declaredFieldCount = GetDeclaredFields().Length;
			}
			_fieldCount += _declaredFieldCount;
			return _fieldCount;
		}

		public virtual IReflectMethod GetMethod(string methodName, IReflectClass[] paramClasses
			)
		{
			if (_delegate != null)
			{
				return _delegate.GetMethod(methodName, paramClasses);
			}
			return null;
		}

		public virtual string GetName()
		{
			return _name;
		}

		public virtual IReflectClass GetSuperclass()
		{
			if (_superclass != null)
			{
				return _superclass;
			}
			if (_delegate == null)
			{
				return _reflector.ForClass(typeof(object));
			}
			IReflectClass delegateSuperclass = _delegate.GetSuperclass();
			if (delegateSuperclass != null)
			{
				_superclass = _reflector.EnsureDelegate(delegateSuperclass);
			}
			return _superclass;
		}

		public override int GetHashCode()
		{
			return _hashCode;
		}

		public virtual void InitFields(GenericField[] fields)
		{
			int startIndex = 0;
			if (_superclass != null)
			{
				startIndex = _superclass.GetFieldCount();
			}
			_fields = fields;
			for (int i = 0; i < _fields.Length; i++)
			{
				_fields[i].SetIndex(startIndex + i);
			}
		}

		// TODO: Consider: Will this method still be necessary 
		// once constructor logic is pushed into the reflectors?
		public virtual bool IsAbstract()
		{
			if (_delegate != null)
			{
				return _delegate.IsAbstract();
			}
			return false;
		}

		public virtual bool IsArray()
		{
			if (_delegate != null)
			{
				return _delegate.IsArray();
			}
			return false;
		}

		public virtual bool IsAssignableFrom(IReflectClass subclassCandidate)
		{
			if (subclassCandidate == null)
			{
				return false;
			}
			if (Equals(subclassCandidate))
			{
				return true;
			}
			if (_delegate != null)
			{
				if (subclassCandidate is Db4objects.Db4o.Reflect.Generic.GenericClass)
				{
					subclassCandidate = ((Db4objects.Db4o.Reflect.Generic.GenericClass)subclassCandidate
						).GetDelegate();
				}
				return _delegate.IsAssignableFrom(subclassCandidate);
			}
			if (!(subclassCandidate is Db4objects.Db4o.Reflect.Generic.GenericClass))
			{
				return false;
			}
			return IsAssignableFrom(subclassCandidate.GetSuperclass());
		}

		public virtual bool IsCollection()
		{
			if (_isCollection == 1)
			{
				return true;
			}
			if (_isCollection == -1)
			{
				return false;
			}
			_isCollection = _reflector.IsCollection(this) ? 1 : -1;
			return IsCollection();
		}

		public virtual bool IsInstance(object candidate)
		{
			if (_delegate != null)
			{
				return _delegate.IsInstance(candidate);
			}
			if (!(candidate is GenericObject))
			{
				return false;
			}
			return IsAssignableFrom(((GenericObject)candidate)._class);
		}

		public virtual bool IsInterface()
		{
			if (_delegate != null)
			{
				return _delegate.IsInterface();
			}
			return false;
		}

		public virtual bool IsPrimitive()
		{
			if (_delegate != null)
			{
				return _delegate.IsPrimitive();
			}
			return _isPrimitive;
		}

		public virtual object NewInstance()
		{
			if (_delegate != null)
			{
				return _delegate.NewInstance();
			}
			return new GenericObject(this);
		}

		public virtual IReflector Reflector()
		{
			if (_delegate != null)
			{
				return _delegate.Reflector();
			}
			return _reflector;
		}

		internal virtual void SetConverter(IGenericConverter converter)
		{
			_converter = converter;
		}

		internal virtual void SetDeclaredFieldCount(int count)
		{
			_declaredFieldCount = count;
		}

		internal virtual void SetPrimitive()
		{
			_isPrimitive = true;
		}

		public override string ToString()
		{
			return "GenericClass " + _name;
		}

		public virtual string ToString(object obj)
		{
			if (_converter == null)
			{
				return "(G) " + GetName();
			}
			return _converter.ToString((GenericObject)obj);
		}

		public virtual bool EnsureCanBeInstantiated()
		{
			if (_delegate != null)
			{
				return _delegate.EnsureCanBeInstantiated();
			}
			return true;
		}

		public virtual object NullValue()
		{
			if (_delegate == null)
			{
				return null;
			}
			return _delegate.NullValue();
		}

		public virtual bool IsSimple()
		{
			if (_delegate != null)
			{
				return _delegate.IsSimple();
			}
			return IsPrimitive();
		}
	}
}
