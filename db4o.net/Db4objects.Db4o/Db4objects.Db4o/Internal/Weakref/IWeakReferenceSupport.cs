/* Copyright (C) 2004 - 2011  Versant Inc.  http://www.db4o.com */

using Db4objects.Db4o.Internal;

namespace Db4objects.Db4o.Internal.Weakref
{
	public interface IWeakReferenceSupport
	{
		object NewWeakReference(ObjectReference referent, object obj);

		void Purge();

		void Start();

		void Stop();
	}
}
