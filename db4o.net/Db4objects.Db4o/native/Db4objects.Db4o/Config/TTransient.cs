/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using Sharpen.Lang;
using Db4objects.Db4o.Config;

namespace Db4objects.Db4o.Config
{

    /// <exclude />
    public class TTransient : IObjectConstructor
    {
        public void OnActivate(IObjectContainer objectContainer, object obj, object members)
        {
        }

        public object OnStore(IObjectContainer objectContainer, object obj)
        {
            return null;
        }

        public System.Type StoredClass()
        {
            return typeof(object);
        }

        public object OnInstantiate(IObjectContainer objectContainer, object storedObject)
        {
            return null;
        }

    }
}
