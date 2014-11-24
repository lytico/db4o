/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */

using System;
using Sharpen.Lang;
using System.Reflection;
using Db4objects.Db4o.Ext;

namespace Db4objects.Db4o.Internal
{
	public class ReflectPlatform
	{

		public const char InnerClassSeparator = '+';

		public static Type ForName(string typeName)
		{
			try
			{
				return TypeReference.FromString(typeName).Resolve();
			}
			catch
			{
				return null;
			}
		}

		public static object CreateInstance(string typeName)
		{
            return ReflectPlatform.CreateInstance(ForName(typeName));
		}

        public static object CreateInstance(Type type)
        {
            try
            {
                return Activator.CreateInstance(type);
            }
            catch
            {
                return null;
            }
        }

	    public static string FullyQualifiedName(Type type)
	    {
	        return TypeReference.FromType(type).GetUnversionedName();
	    }

	    public static bool IsNamedClass(Type type)
	    {
	        return true;
	    }

        public static string SimpleName(Type type)
        {
            return type.Name;
        }
		
		public static object NewInstance(ConstructorInfo ctor, params object[] args)
		{
		
			try
			{
				return ctor.Invoke(args);
			} 
			catch(Exception e)
			{
				throw new Db4oException(e);
			}
		}
		
		public static string GetJavaInterfaceSimpleName(Type type) 
		{
			if (!type.IsInterface) 
			{
				return type.Name;
			}
			
			if (type.Name[0] != 'I') 
			{
				throw new ArgumentException("The interface name should start with an 'I'");
			}
			
			return type.Name.Substring(1);
		}
	
		public static string ContainerName(Type type) 
		{
			return type.Namespace;
		}
		
		public static string AdjustClassName(string className, Type type)
		{
			return className+","+type.Assembly.GetName().Name;
		}


    }
}
