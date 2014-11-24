/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

#if !SILVERLIGHT

using System;
using System.Collections;

namespace Db4objects.Db4o.Config {

	/// <exclude />
    public class TStack : IObjectTranslator {

        public void OnActivate(IObjectContainer objectContainer, object obj, object members){
            Stack stack = (Stack)obj;
            if(members != null){
                object[] elements = (object[]) members;
                for(int i = elements.Length - 1; i >= 0 ; i--){
                    stack.Push(elements[i]);
                }
            }
        }

        public Object OnStore(IObjectContainer objectContainer, object obj){
            Stack stack = (Stack)obj;
            int count = stack.Count;
            object[] elements = new object[count];
            IEnumerator e = stack.GetEnumerator();
            e.Reset();
            for(int i = 0; i < count; i++){
                e.MoveNext();
                elements[i] = e.Current;
            }
            return elements;
        }

        public System.Type StoredClass(){
            return typeof(object[]);
        }
    }
}

#endif