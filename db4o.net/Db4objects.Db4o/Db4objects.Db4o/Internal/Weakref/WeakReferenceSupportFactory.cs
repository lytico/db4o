/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;
using Db4objects.Db4o.Internal.Weakref;

namespace Db4objects.Db4o.Internal.Weakref
{
	public class WeakReferenceSupportFactory
	{
		public static IWeakReferenceSupport ForObjectContainer(ObjectContainerBase container
			)
		{
			if (!Platform4.HasWeakReferences())
			{
				return DisabledWeakReferenceSupport();
			}
			if (!container.ConfigImpl.WeakReferences())
			{
				return DisabledWeakReferenceSupport();
			}
			return new EnabledWeakReferenceSupport(container);
		}

		public static IWeakReferenceSupport DisabledWeakReferenceSupport()
		{
			return new _IWeakReferenceSupport_22();
		}

		private sealed class _IWeakReferenceSupport_22 : IWeakReferenceSupport
		{
			public _IWeakReferenceSupport_22()
			{
			}

			public void Stop()
			{
			}

			public void Start()
			{
			}

			public void Purge()
			{
			}

			public object NewWeakReference(ObjectReference referent, object obj)
			{
				return obj;
			}
		}
	}
}
