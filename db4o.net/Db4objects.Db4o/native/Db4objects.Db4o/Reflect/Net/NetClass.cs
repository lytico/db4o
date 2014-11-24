/* Copyright (C) 2007 Versant Inc.   http://www.db4o.com */
using System;
using System.Reflection;
using Db4objects.Db4o.Internal;
using Sharpen.Lang;
using Db4objects.Db4o.Reflect.Core;

namespace Db4objects.Db4o.Reflect.Net
{
	/// <summary>Reflection implementation for Class to map to .NET reflection.</summary>
	/// <remarks>Reflection implementation for Class to map to .NET reflection.</remarks>
	public class NetClass : IConstructorAwareReflectClass
	{
		protected readonly IReflector _reflector;
		
		private readonly NetReflector _netReflector;

		private readonly Type _type;

		private ReflectConstructorSpec _constructor;

		private string _name;
	    
	    private IReflectField[] _fields;

	    public NetClass(IReflector reflector, NetReflector netReflector, Type clazz)
		{
			if(reflector == null)
			{
				throw new ArgumentNullException("reflector");
			}
			
			if(netReflector == null)
			{
				throw new ArgumentNullException("netReflector");
			}
			
			_reflector = reflector;
			_netReflector = netReflector;
			_type = clazz;
			_constructor = ReflectConstructorSpec.UnspecifiedConstructor;
		}

		public virtual IReflectClass GetComponentType()
		{
			return _reflector.ForClass(_type.GetElementType());
		}

		private IReflectConstructor[] GetDeclaredConstructors()
		{
			ConstructorInfo[] constructors = _type.GetConstructors(BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public);
			IReflectConstructor[] reflectors = new IReflectConstructor[constructors.Length];
			for (int i = 0; i < constructors.Length; i++)
			{
				reflectors[i] = new NetConstructor(_reflector, constructors[i]);
			}
			return reflectors;
		}

		public virtual IReflectField GetDeclaredField(string name)
		{
			foreach (IReflectField field in GetDeclaredFields())
			{
				if (field.GetName() == name) return field;
			}
			return null;
		}

		public virtual IReflectField[] GetDeclaredFields()
		{
			if (_fields == null)
			{
				_fields = CreateDeclaredFieldsArray();
			}
			return _fields;
		}
		
		private IReflectField[] CreateDeclaredFieldsArray()
		{	
			FieldInfo[] fields = Sharpen.Runtime.GetDeclaredFields(_type);
			IReflectField[] reflectors = new IReflectField[fields.Length];
			for (int i = 0; i < reflectors.Length; i++)
			{
				reflectors[i] = CreateField(fields[i]);
			}
			return reflectors;
		}

		protected virtual IReflectField CreateField(FieldInfo field)
		{
			return new NetField(_reflector, field);
		}

		public virtual IReflectClass GetDelegate()
		{
			return this;
		}

		public virtual IReflectMethod GetMethod(string methodName, IReflectClass[] paramClasses)
		{
			try
			{
				Type[] parameterTypes = NetReflector.ToNative(paramClasses);
				MethodInfo method = GetMethod(_type, methodName, parameterTypes);
				if (method == null)
				{
					return null;
				}
				return new NetMethod(_reflector, method);
			}
			catch
			{
				return null;
			}
		}

		private static MethodInfo GetMethod(Type type, string methodName, Type[] parameterTypes)
		{
			MethodInfo found = Sharpen.Runtime.GetDeclaredMethod(type, methodName, parameterTypes);
			if (found != null) return found;

			Type baseType = type.BaseType;
			if (null == baseType) return null;
			return GetMethod(baseType, methodName, parameterTypes);
		}

		public virtual string GetName()
		{
            if (_name == null)
            {
                _name = TypeReference.FromType(_type).GetUnversionedName();
            }
            return _name;
		}

		public virtual IReflectClass GetSuperclass()
		{
			return _reflector.ForClass(_type.BaseType);
		}

		public virtual bool IsAbstract()
		{
			return _type.IsAbstract;
		}

		public virtual bool IsArray()
		{
			return _type.IsArray;
		}

		public virtual bool IsAssignableFrom(IReflectClass type)
		{
			if (!(type is NetClass))
			{
				return false;
			}
			return _type.IsAssignableFrom(((NetClass)type).GetNetType());
		}

		public virtual bool IsInstance(object obj)
		{
			return _type.IsInstanceOfType(obj);
		}

		public virtual bool IsInterface()
		{
			return _type.IsInterface;
		}

		public virtual bool IsCollection()
		{
			return _reflector.IsCollection(this);
		}

		public virtual bool IsPrimitive()
		{
			return _type.IsPrimitive
			       || _type == typeof(DateTime)
			       || _type == typeof(decimal);
		}

		public virtual object NewInstance()
		{
			CreateConstructor();
			return _constructor.NewInstance();
		}

		public virtual Type GetNetType()
		{
			return _type;
		}

		public virtual IReflector Reflector()
		{
			return _reflector;
		}
		
		public virtual IReflectConstructor GetSerializableConstructor()
		{
#if !CF && !SILVERLIGHT
			return new SerializationConstructor(GetNetType());
#else
			return null;
#endif
		}

		public override string ToString()
		{
			return "NetClass(" + _type + ")";
		}

		public virtual object NullValue() 
		{
			return _netReflector.NullValue(this);
		}
	
		private void CreateConstructor() 
		{
			if(!_constructor.CanBeInstantiated().IsUnspecified())
			{
				return;
			}
			_constructor = ConstructorSupport.CreateConstructor(this, _type, _netReflector.Configuration(), GetDeclaredConstructors());
		}
		
		public virtual bool EnsureCanBeInstantiated() {
			CreateConstructor();
			return _constructor.CanBeInstantiated().DefiniteYes();
		}

	    public bool IsSimple()
	    {
	        return IsPrimitive() || Platform4.IsSimple(_type);
	    }
	}
}
