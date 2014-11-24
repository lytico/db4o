/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using System.Collections;
using Sharpen.Lang;
using Db4objects.Db4o;

namespace Db4objects.Db4o.Config {

	/// <exclude />
    public class TList : IObjectTranslator {

        public void OnActivate(IObjectContainer objectContainer, object obj, object members){
            IList list = (IList)obj;
            list.Clear();
            if(members != null){
                object[] elements = (object[]) members;
                for(int i = 0; i < elements.Length; i++){
                    list.Add(elements[i]);
                }
            }
        }

        public Object OnStore(IObjectContainer objectContainer, object obj){
            IList list = (IList)obj;
            object[] elements = new object[list.Count];
            for(int i = 0; i < list.Count; i++){
                elements[i] = list[i];
            }
            return elements;
        }

        public System.Type StoredClass(){
            return typeof(object[]);
        }
    }
}
