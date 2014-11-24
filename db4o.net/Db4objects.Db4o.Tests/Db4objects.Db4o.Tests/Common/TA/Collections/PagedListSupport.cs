/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using System;
using Db4objects.Db4o;
using Db4objects.Db4o.Config;
using Db4objects.Db4o.Events;
using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Tests.Common.TA.Collections;

namespace Db4objects.Db4o.Tests.Common.TA.Collections
{
	public class PagedListSupport : IConfigurationItem
	{
		public virtual void Apply(IInternalObjectContainer db)
		{
			EventRegistry(db).Updating += new System.EventHandler<Db4objects.Db4o.Events.CancellableObjectEventArgs>
				(new _IEventListener4_19().OnEvent);
		}

		private sealed class _IEventListener4_19
		{
			public _IEventListener4_19()
			{
			}

			public void OnEvent(object sender, Db4objects.Db4o.Events.CancellableObjectEventArgs
				 args)
			{
				CancellableObjectEventArgs cancellable = (CancellableObjectEventArgs)args;
				if (cancellable.Object is Page)
				{
					Page page = (Page)cancellable.Object;
					if (!page.IsDirty())
					{
						cancellable.Cancel();
					}
				}
			}
		}

		private static IEventRegistry EventRegistry(IObjectContainer db)
		{
			return EventRegistryFactory.ForObjectContainer(db);
		}

		public virtual void Prepare(IConfiguration configuration)
		{
		}
		// Nothing to do...
	}
}
