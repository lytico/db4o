/* Copyright (C) 2004   Versant Inc.   http://www.db4o.com */

using System;
using System.Collections;
using Sharpen.Lang;
using Db4objects.Db4o;

namespace Db4objects.Db4o.Config {

	/// <exclude />
    public class TDictionary : IObjectTranslator {

        public void OnActivate(IObjectContainer objectContainer, object obj, object members){
            IDictionary dict = (IDictionary)obj;
            dict.Clear();
            if(members != null){
                Entry[] entries = (Entry[]) members;
                for(int i = 0; i < entries.Length; i++){
                    if(entries[i].key != null && entries[i].value != null){
                        dict[entries[i].key] =  entries[i].value;
                    }
                }
            }
        }

        public Object OnStore(IObjectContainer objectContainer, object obj){
            IDictionary dict = (IDictionary)obj;
            Entry[] entries = new Entry[dict.Count];
            IDictionaryEnumerator e = dict.GetEnumerator();
            e.Reset();
            for(int i = 0; i < dict.Count; i++){
                e.MoveNext();
                entries[i] = new Entry();
                entries[i].key = e.Key;
                entries[i].value = e.Value;
            }
            return entries;
        }

        public System.Type StoredClass(){
            return typeof(Entry[]);
        }
    }
}
