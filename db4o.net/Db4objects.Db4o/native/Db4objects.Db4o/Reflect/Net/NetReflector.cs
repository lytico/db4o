/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Reflect.Net
{
	public class NetReflector : IReflector
	{

	    protected IReflector _parent;

		private IReflectArray _array;
		
		private IReflectorConfiguration _config;

        public NetReflector(IReflectorConfiguration config)
        {
            _config = config;
        }

        public NetReflector() : this(new DefaultConfiguration())
        {

        }


		public virtual IReflectArray Array()
		{
			if(_array == null)
			{
				_array = new NetArray(Parent());
			}
			return _array;
		}

		public virtual object DeepClone(object obj)
		{
			return new NetReflector(_config);
		}

		public virtual IReflectClass ForClass(Type forType)
		{
            if(forType == null)
            {
                return null;
            }
		    Type underlyingType = GetUnderlyingType(forType);
            if (underlyingType.IsPrimitive)
            {
                return CreateClass(forType);
            }
            return CreateClass(underlyingType);
		}

		protected virtual IReflectClass CreateClass(Type type)
		{
			if(type == null)
			{
				return null;
			}
			return new NetClass(Parent(), this, type);
		}

		private static Type GetUnderlyingType(Type type)
        {	
        	if(type == null)
        	{
        		return null;
        	}
            Type underlyingType = Nullable.GetUnderlyingType(type);
            if (underlyingType != null)
            {
                return underlyingType;
            }
            return type;
        }

		public virtual IReflectClass ForName(string className)
		{
			try
			{
				Type type = ReflectPlatform.ForName(className);
				if (type == null) return null;
				return ForClass(type);
			}
			catch
			{
			}
			return null;
		}

		public virtual IReflectClass ForObject(object a_object)
		{
			if (a_object == null)
			{
				return null;
			}
			return Parent().ForClass(a_object.GetType());
		}

		public virtual bool IsCollection(IReflectClass candidate)
		{
			if (candidate.IsArray())
			{
				return false;
			}
		    NetClass netClass = candidate as NetClass;
            if (null == netClass)
            {
                return false;
            }
		    return typeof(System.Collections.ICollection).IsAssignableFrom(netClass.GetNetType());
		}

		public virtual bool MethodCallsSupported()
		{
			return true;
		}

		public static IReflectClass[] ToMeta(IReflector reflector, Type[] clazz)
		{
			IReflectClass[] claxx = null;
			if (clazz != null)
			{
				claxx = new IReflectClass[clazz.Length];
				for (int i = 0; i < clazz.Length; i++)
				{
					if (clazz[i] != null)
					{
						claxx[i] = reflector.ForClass(clazz[i]);
					}
				}
			}
			return claxx;
		}

		public static Type[] ToNative(IReflectClass[] claxx)
		{
			Type[] clazz = null;
			if (claxx != null)
			{
				clazz = new Type[claxx.Length];
				for (int i = 0; i < claxx.Length; i++)
				{
					if (claxx[i] != null)
					{
						IReflectClass reflectClass = claxx[i];
						clazz[i] = ToNative(reflectClass);
					}
				}
			}
			return clazz;
		}

		public static Type ToNative(IReflectClass reflectClass)
		{
            NetClass netClass = reflectClass.GetDelegate() as NetClass;
            if(netClass == null)
            {
                return null;
            }
			return netClass.GetNetType();
		}

		public virtual void SetParent(IReflector reflector)
		{
			_parent = reflector;
		}

        public virtual void Configuration(IReflectorConfiguration config)
		{
			_config = config;
		}
		
        public virtual IReflectorConfiguration Configuration()
		{
			return _config;
		}

		public virtual object NullValue(IReflectClass clazz) 
		{
			return Platform4.NullValue(ToNative(clazz));
		}
		
		protected IReflector Parent()
		{
			if(_parent == null)
			{
				return this;
			}
			
			return _parent;
		}

        private class DefaultConfiguration : IReflectorConfiguration
	    {
            public bool TestConstructors()
            {
                return false;
            }

            public bool CallConstructor(IReflectClass clazz)
            {
                return false;
            }
	    }

	}
}
