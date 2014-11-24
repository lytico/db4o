/* Copyright (C) 2009   Versant Inc.   http://www.db4o.com */

#if !SILVERLIGHT

using System;
using System.Collections;

namespace Db4objects.Db4o.Config
{
	/// <exclude />
	public class TQueue : IObjectTranslator
	{
		public void OnActivate(IObjectContainer objectContainer, object obj, object members)
		{
			Queue queue = (Queue) obj;
			queue.Clear();
			if (members != null)
			{
				object[] elements = (object[])members;
				for (int i = 0; i < elements.Length; i++)
				{
					queue.Enqueue(elements[i]);
				}
			}
		}

		public Object OnStore(IObjectContainer objectContainer, object obj)
		{
			Queue queue = (Queue)obj;
			int count = queue.Count;
			object[] elements = new object[count];
			IEnumerator e = queue.GetEnumerator();
			e.Reset();
			for (int i = 0; i < count; i++)
			{
				e.MoveNext();
				elements[i] = e.Current;
			}
			return elements;
		}

		public System.Type StoredClass()
		{
			return typeof(object[]);
		}
	}
}

#endif