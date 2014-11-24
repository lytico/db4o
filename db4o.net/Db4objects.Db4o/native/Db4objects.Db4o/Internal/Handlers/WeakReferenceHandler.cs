/* Copyright (C) 2004 - 2007   Versant Inc.   http://www.db4o.com */

using System;

namespace Db4objects.Db4o.Internal.Handlers
{
	internal class WeakReferenceHandler
	{
		private readonly WeakReference _reference;
		public object ObjectReference;

		internal WeakReferenceHandler(Object queue, Object objectRef, Object obj)
		{
			_reference = new WeakReference(obj, false);
			ObjectReference = objectRef;
			((WeakReferenceHandlerQueue)queue).Add(this);
		}

		public object Get()
		{
			return _reference.Target;
		}

		public bool IsAlive
		{
			get { return _reference.IsAlive; }
		}
	}
}