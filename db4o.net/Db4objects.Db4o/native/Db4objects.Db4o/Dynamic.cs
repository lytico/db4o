/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using System.Reflection;

namespace Db4objects.Db4o {

	/// <exclude />
    public class Dynamic {

		private const BindingFlags AllMembers = BindingFlags.Static | BindingFlags.Instance | BindingFlags.NonPublic | BindingFlags.Public;

        public static object GetProperty(object obj, string prop){
            if(obj != null){
                Type type = TypeForObject(obj);
                try {
                    PropertyInfo pi = type.GetProperty(prop, AllMembers);
                    return pi.GetValue(obj,null);
                } catch {
                }
            }
            return null;
        }

        public static void SetProperty(object obj, string prop, object val){
            if(obj != null){
                Type type = TypeForObject(obj);
                try {
                    PropertyInfo pi = type.GetProperty(prop, AllMembers);
                    pi.SetValue(obj, val, null);
                } catch {
                }
            }
        }

        private static Type TypeForObject(object obj){
            Type type = obj as Type;
            if(type != null){
                return type;
            }
            return obj.GetType();
        }
    }
}
