/* Copyright (C) 2004 - 2007   Versant Inc.   http://www.db4o.com */

using Db4objects.Db4o.Ext;
using Db4objects.Db4o.Foundation;

namespace Db4objects.Db4o.Internal.Handlers
{
    internal class WeakReferenceHandlerQueue
	{
        private List4 _list;

        internal void Add(WeakReferenceHandler reference) {
            lock(this){
                _list = new List4(_list, reference);
            }
        }

        internal void Poll(ObjectContainerBase objectContainer) {
            List4 remove = null;
            lock(this){
                System.Collections.IEnumerator i = new Iterator4Impl(_list);
                _list = null;
                while(i.MoveNext()){
					WeakReferenceHandler refHandler = (WeakReferenceHandler)i.Current;
                    if(refHandler.IsAlive){
                        _list = new List4(_list, refHandler);
                    }else{
                        remove = new List4(remove, refHandler.ObjectReference);
                    }
                }
            }
            System.Collections.IEnumerator j = new Iterator4Impl(remove);
            while (j.MoveNext())
            {
                lock (objectContainer.Lock())
                {
                    if (objectContainer.IsClosed())
                    {
                        return;
                    }
                    objectContainer.RemoveFromAllReferenceSystems(j.Current);
                }
            }
        }
    }
}