/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System.Collections;
using Db4objects.Db4o.Foundation;
using Db4objects.Db4o.Internal;
using Sharpen.Lang;

namespace Db4objects.Db4o.Internal
{
	internal class ShutDownRunnable : IRunnable
	{
		private Collection4 _containers = new Collection4();

		public volatile bool dontRemove = false;

		public virtual void Ensure(ObjectContainerBase container)
		{
			_containers.Ensure(container);
		}

		public virtual void Remove(ObjectContainerBase container)
		{
			_containers.Remove(container);
		}

		public virtual void Run()
		{
			dontRemove = true;
			Collection4 copy = new Collection4(_containers);
			IEnumerator i = copy.GetEnumerator();
			while (i.MoveNext())
			{
				((ObjectContainerBase)i.Current).ShutdownHook();
			}
		}

		public virtual int Size()
		{
			return _containers.Size();
		}
	}
}
