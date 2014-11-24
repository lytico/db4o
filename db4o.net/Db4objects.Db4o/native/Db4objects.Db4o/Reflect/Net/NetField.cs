/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using System.Collections;
using System.Collections.Generic;
using System.Reflection;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Reflect.Net
{
	public class NetField : IReflectField
	{
		private readonly IReflector _reflector;

		protected readonly FieldInfo _field;

        private static IList _transientMarkers;

		public NetField(IReflector reflector, FieldInfo field)
		{
			_reflector = reflector;
			_field = field;
		}

        public override string ToString()
        {
            return string.Format("NetField({0})", _field);
        }

		public virtual string GetName()
		{
			return _field.Name;
		}

		public virtual IReflectClass GetFieldType()
		{
			return _reflector.ForClass(_field.FieldType);
		}

		public virtual bool IsPublic()
		{
			return _field.IsPublic;
		}

		public virtual bool IsStatic()
		{
			return _field.IsStatic;
		}

		public virtual bool IsTransient()
		{
            return IsTransient(_field);
		}

		public virtual void SetAccessible()
		{	
		}

		public virtual object Get(object onObject)
		{
			try
			{
				return _field.GetValue(onObject);
			}
			catch(Exception e)
			{
				throw new Db4oException(e);
			}
		}

		public virtual void Set(object onObject, object attribute)
		{
			try
			{
				_field.SetValue(onObject, attribute);
			}
			catch(Exception e)
			{
				throw new Db4oException(e);
			}
		}
		
		public object IndexEntry(object orig)
		{
			return orig;
		}
		
		public IReflectClass IndexType()
		{
			return GetFieldType();
		}

        public static bool IsTransient(FieldInfo field)
        {
            if (field.IsNotSerialized) return true;
            if (field.IsDefined(typeof(TransientAttribute), true)) return true;
            if (_transientMarkers == null) return false;
            return CheckForTransient(field.GetCustomAttributes(true));
        }

        private static bool CheckForTransient(object[] attributes)
        {   
            if (attributes == null) return false;

            foreach (object attribute in attributes)
            {
                string attributeName = attribute.GetType().FullName;
                if (_transientMarkers.Contains(attributeName)) return true;
            }
            return false;
        }

        public static void MarkTransient(Type attributeType)
        {
            MarkTransient(attributeType.FullName);
        }

        public static void MarkTransient(string attributeName)
        {
            if (_transientMarkers == null)
            {
                _transientMarkers = new List<string>();
            }
            else if (_transientMarkers.Contains(attributeName))
            {
                return;
            }
            _transientMarkers.Add(attributeName);
        }

	    public static void ResetTransientMarkers()
	    {
            _transientMarkers = null;
	    }
	}
}
