/* Copyright (C) 2007   Versant Inc.   http://www.db4o.com */
using System;
using Db4objects.Db4o.Reflect.Core;

namespace Db4objects.Db4o.Reflect.Net
{
#if !CF && !SILVERLIGHT
	/// <summary>Constructs objects by using System.Runtime.Serialization.FormatterServices.GetUninitializedObject
	/// and bypasses calls to user contructors this way. Not available on CompactFramework
	/// </summary>
	public class SerializationConstructor : IReflectConstructor
	{
        private Type _type;

		public SerializationConstructor(Type type){
            _type = type;
		}

        public virtual IReflectClass[] GetParameterTypes() {
            return null;
        }

#if NET_4_0
		[System.Security.SecurityCritical]
#endif		
        public virtual object NewInstance(object[] parameters) {
            return System.Runtime.Serialization.FormatterServices.GetUninitializedObject(_type);
        }
	}
#endif
}

