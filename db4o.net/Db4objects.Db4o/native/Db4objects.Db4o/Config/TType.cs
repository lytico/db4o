/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using Sharpen.Lang;
using Db4objects.Db4o;

namespace Db4objects.Db4o.Config
{
	/// <exclude />
    public class TType : IObjectConstructor
    {		
        public void OnActivate(IObjectContainer objectContainer, object obj, object members)
        {
        }
      
        public Object OnInstantiate(IObjectContainer objectContainer, object obj)
        {
        	if (obj != null)
        	{
            	try
	            {
    	            return TypeReference.FromString((string) obj).Resolve();
        	    }
            	catch
	            { 
	            }
			}
			return null; 
        }
      
        public Object OnStore(IObjectContainer objectContainer, object obj)
        {
        	if (obj == null) return null;
            return TypeReference.FromType((Type)obj).GetUnversionedName();
        }
      
        public Type StoredClass()
        {
            return typeof(string);
        }
    }
}